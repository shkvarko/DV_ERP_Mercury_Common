using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace ERP_Mercury.Common
{
    public partial class ctrlCompany : UserControl
    {
        #region Свойства, переменные

        private UniXP.Common.CProfile m_objProfile;
        UniXP.Common.MENUITEM m_objMenuItem;
        private CCompany m_objSelectedCompany;

        private System.Boolean m_bIsChanged;

        public ctrlAddress frmAddress;
        public ctrlContact frmContact;

        private System.Boolean m_bDisableEvents;
        private System.Boolean m_bNewCompany;
        private System.Boolean m_bIsReadOnly;

        private const System.Int32 iMinControlItemHeight = 20;
        private const System.Int32 iPanel1WidthDef = 350;
        private const System.Int32 iThreadSleepTime = 1000;

        private List<CStateTypeCompany> m_objStateTypeCompanyList;
        private List<CPhoneType> m_objPhoneTypeList;
        private List<CLicenceType> m_objLicenceTypeList;
        private List<CAccountType> m_objAccountTypeList;
        private List<CBank> m_objBankList;
        private List<CCurrency> m_objCurrencyList;
        private List<CCompanyType> m_objCompanyTypeList;
        private List<CDepartament> m_objDepartamentList;
        private List<CJobPosition> m_objJobPositionList;
        private List<CAddress> m_objAddressList;

        private List<CCountry> m_objCountryList;
        private List<COblast> m_objOblastList;
        private List<CRegion> m_objRegionList;
        private List<CCity> m_objCityList;

        public delegate void LoadComboBoxForEditorDelegate(
           List<CDepartament> objDepartamentList, List<CJobPosition> objJobPositionList, List<CAddress> objAddressList,
           List<CCountry> objCountryList, List<COblast> objOblastList, List<CRegion> objRegionList, List<CCity> objCityList,
           List<CAddressType> objAddressTypeList, List<CAddressPrefix> objAddressPrefixList, List<CBuilding> objBuildingList,
           List<CSubBuilding> objSubBuildingList, List<CFlat> objFlatList           
           );
        public LoadComboBoxForEditorDelegate m_LoadComboBoxForEditorDelegate;
        public System.Threading.Thread ThreadComboBoxForEditor { get; set; }

        private System.Boolean m_bNewPhone;
        private System.Boolean m_bNewAccount;
        private System.Boolean m_bNewEmail;
        private System.Boolean m_bNewLicence;

        #endregion

        #region События
       // Создаем закрытое поле, ссылающееся на заголовок списка делегатов
        private EventHandler<ChangeCompanyPropertieEventArgs> m_ChangeCompanyProperties;
        // Создаем в классе член-событие
        public event EventHandler<ChangeCompanyPropertieEventArgs> ChangeCompanyProperties
        {
            add
            {
                // берем закрытую блокировку и добавляем обработчик
                // (передаваемый по значению) в список делегатов
                m_ChangeCompanyProperties += value;
            }
            remove
            {
                // берем закрытую блокировку и удаляем обработчик
                // (передаваемый по значению) из списка делегатов
                m_ChangeCompanyProperties -= value;
            }
        }
        /// <summary>
        /// Инициирует событие и уведомляет о нем зарегистрированные объекты
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnChangeCompanyProperties(ChangeCompanyPropertieEventArgs e)
        {
            // Сохраняем поле делегата во временном поле для обеспечение безопасности потока
            EventHandler<ChangeCompanyPropertieEventArgs> temp = m_ChangeCompanyProperties;
            // Если есть зарегистрированные объектв, уведомляем их
            if (temp != null) temp(this, e);
        }
        public void SimulateChangeCompanyProperties(CCompany objCompany, enumActionSaveCancel enActionType, System.Boolean bIsNewCompany)
        {
            // Создаем объект, хранящий информацию, которую нужно передать
            // объектам, получающим уведомление о событии
            ChangeCompanyPropertieEventArgs e = new ChangeCompanyPropertieEventArgs(objCompany, enActionType, bIsNewCompany);

            // Вызываем виртуальный метод, уведомляющий наш объект о возникновении события
            // Если нет типа, переопределяющего этот метод, наш объект уведомит все объекты, 
            // подписавшиеся на уведомление о событии
            OnChangeCompanyProperties(e);
        }
        #endregion

        #region Конструктор
        public ctrlCompany(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem)
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_bIsChanged = false;
            m_bDisableEvents = false;
            m_bNewCompany = false;

            m_objSelectedCompany = null;

            m_objStateTypeCompanyList = null;
            m_objPhoneTypeList = null;
            m_objLicenceTypeList = null;
            m_objAccountTypeList = null;
            m_objBankList = null;
            m_objCurrencyList = null;
            m_objCompanyTypeList = null;
            m_objDepartamentList = null;
            m_objJobPositionList = null;
            m_objAddressList = null;
            m_objCountryList = null;
            m_objOblastList = null;
            m_objRegionList = null;
            m_objCityList = null;

            m_bNewPhone = false;
            m_bNewAccount = false;
            m_bNewEmail = false;
            m_bNewLicence = false;


            m_bIsReadOnly = false;
            
            CheckClientsRight();
        }
        #endregion

        #region Выпадающие списки
        /// <summary>
        /// Проверка динамических прав
        /// </summary>
        private void CheckClientsRight()
        {
            try
            {               
                if (m_objProfile.GetClientsRight().GetState(ERP_Mercury.Global.Consts.strDR_EditCompanyCard) == false) // проверить что возвращается здесь. Здесь возвращается false, как будто нету прав, а они есть. разобраться
                {
                    btnEdit.Visible = false;
                    btnPrint.Visible = false;
                    btnSave.Visible = false;
                }
                else
                {
                    btnEdit.Visible = true;
                    btnSave.Visible = true;
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("CheckClientsRight. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        /// <summary>
        /// Загружает выпадающие списки в редактор
        /// </summary>
        public void StartThreadComboBoxForEditor()
        {
            try
            {
                frmAddress = new ERP_Mercury.Common.ctrlAddress(ERP_Mercury.Common.EnumObject.Company, m_objProfile, m_objMenuItem, System.Guid.Empty);
                frmContact = new ERP_Mercury.Common.ctrlContact(ERP_Mercury.Common.EnumObject.Company, m_objProfile, m_objMenuItem, System.Guid.Empty, false);

                layoutControlAddress.Controls.Add(frmAddress, 0, 0);

                this.frmAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));

                frmAddress.ChangeAddressPropertie += OnChangeAddressPropertie;


                layoutControlContact.Controls.Add(frmContact, 0, 0);

                this.frmContact.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));

                frmContact.ChangeContactProperties += OnChangeContactPropertie;

                cboxState.Properties.Items.Clear();
                cboxTypeCompany.Properties.Items.Clear();
                cboxPhoneType.Properties.Items.Clear();
                cboxLicenseType.Properties.Items.Clear();
                cboxAccountBank.Properties.Items.Clear();
                cboxAccountCurrency.Properties.Items.Clear();
                cboxAccountType.Properties.Items.Clear();

                repItemcboxPhoneType.Items.Clear();
                repItemCBoxLicenceType.Items.Clear();
                repItemcboxAccountType.Items.Clear();
                repItemcboxAccountBank.Items.Clear();
                repItemcboxAccountCurrency.Items.Clear();

                m_objStateTypeCompanyList = CStateTypeCompany.GetStateTypeList(m_objProfile, null);
                m_objCompanyTypeList = CCompanyType.GetCompanyTypeList(m_objProfile, null);
                m_objPhoneTypeList = CPhoneType.GetPhoneTypeList(m_objProfile, null);
                m_objLicenceTypeList = CLicenceType.GetLicenceTypeList(m_objProfile, null);
                m_objAccountTypeList = CAccountType.GetAccountTypeList(m_objProfile, null);
                m_objBankList = CBank.GetBankList(m_objProfile, null, null);
                m_objCurrencyList = CCurrency.GetCurrencyList(m_objProfile, null);

                if (m_objStateTypeCompanyList != null)
                {
                    cboxState.Properties.Items.AddRange(m_objStateTypeCompanyList);
                }
                if (m_objCompanyTypeList != null)
                {
                    cboxTypeCompany.Properties.Items.AddRange(m_objCompanyTypeList);
                }

                if ((m_objPhoneTypeList != null) && (m_objPhoneTypeList.Count > 0))
                {
                    cboxPhoneType.Properties.Items.AddRange(m_objPhoneTypeList);
                }
                if ((m_objLicenceTypeList != null) && (m_objLicenceTypeList.Count > 0))
                {
                    cboxLicenseType.Properties.Items.AddRange(m_objLicenceTypeList);
                }
                if ((m_objAccountTypeList != null) && (m_objAccountTypeList.Count > 0))
                {
                    cboxAccountType.Properties.Items.AddRange(m_objAccountTypeList);
                }
                if ((m_objBankList != null) && (m_objBankList.Count > 0))
                {
                    cboxAccountBank.Properties.Items.AddRange(m_objBankList);
                }
                if ((m_objCurrencyList != null) && (m_objCurrencyList.Count > 0))
                {
                    cboxAccountCurrency.Properties.Items.AddRange(m_objCurrencyList);
                }


                // инициализируем делегаты
                m_LoadComboBoxForEditorDelegate = new LoadComboBoxForEditorDelegate(LoadComboBoxForEditor);

                // запуск потока
                this.ThreadComboBoxForEditor = new System.Threading.Thread(LoadComboBoxForEditorInThread);
                this.ThreadComboBoxForEditor.Start();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("StartThreadComboBoxForEditor().\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        /// <summary>
        /// Загружает выпадающие списки в редактор (метод, выполняемый в потоке)
        /// </summary>
        public void LoadComboBoxForEditorInThread()
        {
            try
            {
                m_objDepartamentList = CDepartament.GetDepartamentList(m_objProfile, null);
                m_objJobPositionList = CJobPosition.GetJobPositionList(m_objProfile, null);

                m_objCountryList = CCountry.GetCountryList(m_objProfile, null);
                m_objOblastList = COblast.GetOblastList(m_objProfile, null);
                m_objRegionList = CRegion.GetRegionList(m_objProfile, null);
                m_objCityList = CCity.GetCityList(m_objProfile, null, true);

                List<CAddressType> objAddressTypeList = CAddressType.GetAddressTypeList(m_objProfile, null);
                List<CAddressPrefix> objAddressPrefixList = CAddressPrefix.GetAddressPrefixList(m_objProfile, null);
                List<CBuilding> objBuildingList = CBuilding.GetBuildingList(m_objProfile, null);
                List<CSubBuilding> objSubBuildingList = CSubBuilding.GetSubBuildingList(m_objProfile, null);
                List<CFlat> objFlatList = CFlat.GetFlatList(m_objProfile, null);

                this.Invoke(m_LoadComboBoxForEditorDelegate,
                    new Object[] {
                                    m_objDepartamentList, m_objJobPositionList, m_objAddressList,
                                    m_objCountryList, m_objOblastList, m_objRegionList, m_objCityList,
                                    objAddressTypeList, objAddressPrefixList, objBuildingList,
                                    objSubBuildingList, objFlatList
                                  });

                this.Invoke(m_LoadComboBoxForEditorDelegate,
                    new Object[] { null, null, null, null, null, null, null, null, null, null, null, null});
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadComboBoxForDecodeEditorInThread.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// Загрузка выпадающих списков значений в редакторы
        /// </summary>
        /// <param name="objDepartamentList">отделы</param>
        /// <param name="objJobPositionList">должности</param>
        /// <param name="objAddressList">адреса</param>
        /// <param name="objCountryList">страны</param>
        /// <param name="objOblastList">области</param>
        /// <param name="objRegionList">районы</param>
        /// <param name="objCityList">города</param>
        /// <param name="objAddressTypeList">типы улиц</param>
        /// <param name="objAddressPrefixList">типы строений</param>
        /// <param name="objBuildingList"></param>
        /// <param name="objSubBuildingList">типы корпусов</param>
        /// <param name="objFlatList">типы помещений</param>
        private void LoadComboBoxForEditor( 
           List<CDepartament> objDepartamentList, List<CJobPosition> objJobPositionList, List<CAddress> objAddressList,
           List<CCountry> objCountryList, List<COblast> objOblastList, List<CRegion> objRegionList, List<CCity> objCityList,
           List<CAddressType> objAddressTypeList, List<CAddressPrefix> objAddressPrefixList, List<CBuilding> objBuildingList,
           List<CSubBuilding> objSubBuildingList, List<CFlat> objFlatList
            )
        {
            try
            {
                if (
                    (objJobPositionList != null) || (objAddressList != null)
                    )
                {
                    if (frmContact != null) { frmContact.LoadComboBoxItems(objDepartamentList, objJobPositionList, m_objPhoneTypeList); }
                    if (frmAddress != null)
                    {
                        frmAddress.LoadComboBox(m_objCountryList, m_objOblastList, m_objRegionList, m_objCityList,
                                    objAddressTypeList, objAddressPrefixList, objBuildingList,
                                    objSubBuildingList, objFlatList);
                    }

                }
                else
                {
                    // процесс загрузки данных завершён
                    Thread.Sleep(iThreadSleepTime);
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadComboBoxForDecodeEditor.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        #endregion

        #region Индикация изменений
        private void SetPropertiesModified(System.Boolean bModified)
        {
            try
            {
                m_bIsChanged = bModified;
                btnSave.Enabled = (m_bIsChanged && (ValidateProperties() == true));
                if (m_bIsChanged == true)
                {
                    SimulateChangeCompanyProperties(m_objSelectedCompany, enumActionSaveCancel.Unkown, m_bNewCompany);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("SetPropertiesModified. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;

        }

        private void cboxCompanyPropertie_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                SetPropertiesModified(true);
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения свойств " + ((DevExpress.XtraEditors.ComboBoxEdit)sender).ToolTip + ". Текст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        private void txtCompanyPropertie_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                if (e.NewValue != null)
                {
                    SetPropertiesModified(true);
                    if ((sender.GetType().Name == "TextEdit") &&
                        (((DevExpress.XtraEditors.TextEdit)sender).Properties.ReadOnly == false))
                    {
                        System.String strValue = (System.String)e.NewValue;
                        ((DevExpress.XtraEditors.TextEdit)sender).Properties.Appearance.BackColor = (strValue == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White;
                    }
                }
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка изменения свойств договора. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }


        #endregion

        #region События и обработчики
        private void OnChangeAddressPropertie(Object sender, ERP_Mercury.Common.ChangeAddressPropertieEventArgs e)
        {
            try
            {
                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "OnChangeAddressPropertie.\n Текст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally // очищаем занимаемые ресурсы
            {
            }

            return;
        }
        private void OnChangeContactPropertie(Object sender, ERP_Mercury.Common.ChangeContactPropertieEventArgs e)
        {
            try
            {
                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "OnChangeContactPropertie.\n Текст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally // очищаем занимаемые ресурсы
            {
            }

            return;
        }

        private void txtUNP_InvalidValue(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            e.ErrorText = "Неверное значение.\nУНП клиента состоит из 9-ти символов.\nДля отмены нажмите \"Esc\"";
        }
        private void txtOKPO_InvalidValue(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            e.ErrorText = "Неверное значение.\nОКПО клиента состоит из 12-ти символов.\nДля отмены нажмите \"Esc\"";
        }
        private void txtOKULP_InvalidValue(object sender, DevExpress.XtraEditors.Controls.InvalidValueExceptionEventArgs e)
        {
            e.ErrorText = "Неверное значение.\nОКЮЛП клиента состоит из 9-ти символов.\nДля отмены нажмите \"Esc\"";
        }
        #endregion

        #region Телефонные номера
        /// <summary>
        /// Добавляет в список телефонный номер
        /// </summary>
        private void AddPhone()
        {
            try
            {
                if (treeListPhone.Enabled == false) { treeListPhone.Enabled = true; }
                if (m_objSelectedCompany.PhoneList == null) { m_objSelectedCompany.PhoneList = new List<CPhone>(); }
                
                System.Boolean bNotFullNode = false;
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objItem in treeListPhone.Nodes)
                {
                    if ((objItem.GetValue(colPhoneNumber) == null) || ((System.String)objItem.GetValue(colPhoneNumber) == "") )
                    {
                        treeListPhone.FocusedNode = objItem;
                        bNotFullNode = true;
                        break;
                    }
                }
                if (bNotFullNode == true) { return; }

                m_bNewPhone = true;

                treeListPhone.FocusedNode = null;

                //txtPhoneNumber.Text = "000000";
                cboxPhoneType.SelectedItem = ((cboxPhoneType.Properties.Items.Count > 0) ? (CPhoneType)cboxPhoneType.Properties.Items[0] : null);
                checkPhoneIsActive.Checked = true;
                checkPhoneIsMain.Checked = false;

                txtPhoneNumber.SelectAll();
                btnNewPhone.Enabled = false;
                lblNewPhone.Visible = true;

                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка добавления телефонного номера. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// Удаляет телефонный номер из списка
        /// </summary>
        /// <param name="objNode">узел с телефонным номером</param>
        private void DeletePhone(DevExpress.XtraTreeList.Nodes.TreeListNode objNode)
        {
            try
            {
                if ((objNode == null) || (treeListPhone.Nodes.Count == 0))
                {
                    txtPhoneNumber.Text = System.String.Empty;
                    cboxPhoneType.SelectedItem = null;
                    checkPhoneIsActive.Checked = false;
                    checkPhoneIsMain.Checked = false;

                    treeListPhone.FocusedNode = ((treeListPhone.Nodes.Count > 0) ? treeListPhone.Nodes[0] : null);

                    btnNewPhone.Enabled = true;
                    m_bNewPhone = false;
                    lblNewPhone.Visible = false;

                    return;
                }

                if (m_objSelectedCompany.PhoneForDeleteList == null) { m_objSelectedCompany.PhoneForDeleteList = new List<CPhone>(); }
                DevExpress.XtraTreeList.Nodes.TreeListNode objPrevNode = objNode.PrevNode;
                m_objSelectedCompany.PhoneForDeleteList.Add((CPhone)objNode.Tag);

                treeListPhone.Nodes.Remove(objNode);
                if (objPrevNode == null)
                {
                    treeListPhone.FocusedNode = ( (treeListPhone.Nodes.Count > 0) ? treeListPhone.Nodes[0] : null );
                }
                else
                {
                    treeListPhone.FocusedNode = objPrevNode;
                }

                LoadPhoneNodeInfoInEditor(treeListPhone.FocusedNode);

                btnNewPhone.Enabled = true;
                m_bNewPhone = false;
                lblNewPhone.Visible = false;

                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка удаления телефонного номера. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        /// <summary>
        /// выполняет проверку телефонного номера
        /// </summary>
        /// <param name="strPhoneNumber">телефонный номер</param>
        /// <param name="objPhoneType">тип телефонного номера</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - ошибок нет; false - телефонный номер не соответсвует установленным требованиям</returns>
        private System.Boolean IsPhoneValid( System.String strPhoneNumber, CPhoneType objPhoneType, ref System.String strErr  )
        {
            System.Boolean bRet = false;
            try
            {
                if (strPhoneNumber.Trim().Length == 0)
                {
                    strErr+= ("\nУкажите, пожалуйста, номер телефона!");
                    return bRet;
                }
                if (objPhoneType == null)
                {
                    strErr += ("\nУкажите, пожалуйста, тип телефонного номера!");
                    return bRet;
                }
                bRet = true;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка проверки телефонного номера: " + f.Message);
            }
            finally
            {
            }
            return bRet;
        }
        /// <summary>
        /// Проверяет, дублируется ли телефонный номер в списке
        /// </summary>
        /// <param name="strPhone">телефонный номер</param>
        /// <param name="iPhonePos">позиция телефонного номера в списке</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - дублируется; false - не ублируется</returns>
        private System.Boolean IsPhoneDublicate(System.String strPhone, System.Int32 iPhonePos, ref System.String strErr)
        {
            System.Boolean bRet = false;

            try
            {
                // проверим, возможно такой телефонный номер уже есть в списке
                System.Boolean bDublicate = false;
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListPhone.Nodes)
                {
                    if (treeListPhone.GetNodeIndex(objNode) != iPhonePos)
                    {
                        if (((System.String)objNode.GetValue(colPhoneNumber)) == strPhone)
                        {
                            bDublicate = true;
                            strErr += (String.Format("Указанный номер уже присутствует в списке: {0}", strPhone));
                            break;
                        }
                    }
                }

                bRet = bDublicate;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка проверки телефонного номера. Текст ошибки: " + f.Message);
            }

            return bRet;
        }
        
        private void LoadPhoneNodeInfoInEditor(DevExpress.XtraTreeList.Nodes.TreeListNode objNode)
        {
            try
            {
                txtPhoneNumber.Text = "";
                cboxPhoneType.SelectedItem = null;
                checkPhoneIsMain.Checked = false;
                checkPhoneIsActive.Checked = false;

                if ((treeListPhone.Nodes.Count == 0) || (objNode == null) || (objNode.Tag == null))
                {
                    return;
                }
                else
                {
                    CPhone objPhone = (CPhone)objNode.Tag;
                    txtPhoneNumber.Text = objPhone.PhoneNumber;
                    cboxPhoneType.SelectedItem = ((cboxPhoneType.Properties.Items.Count == 0) ? null : cboxPhoneType.Properties.Items.Cast<CPhoneType>().SingleOrDefault<CPhoneType>(x => x.ID.CompareTo(objPhone.PhoneType.ID) == 0));
                    checkPhoneIsMain.Checked = objPhone.IsMain;
                    checkPhoneIsActive.Checked = objPhone.IsActive;
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("LoadPhoneNodeInfoInEditor. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private void treeListPhone_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            try
            {
                LoadPhoneNodeInfoInEditor(treeListPhone.FocusedNode);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListPhone_FocusedNodeChanged. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private void ConfirmChangesInPhone()
        {
            try
            {
                if ((m_bNewPhone == false) && (treeListPhone.Nodes.Count == 0))
                {
                    m_bNewPhone = true;
                }

                if (m_bNewPhone == false)
                {
                    if ((treeListPhone.Nodes.Count == 0) || (treeListPhone.FocusedNode == null) || (treeListPhone.FocusedNode.Tag == null))
                    {
                        return;
                    }
                }

                System.String strPhoneNumber = txtPhoneNumber.Text;
                CPhoneType objPhoneType = ((cboxPhoneType.SelectedItem != null) ? (CPhoneType)cboxPhoneType.SelectedItem : null);
                System.Boolean bPhoneIsMain = checkPhoneIsMain.Checked;
                System.Boolean bPhoneIsActive = checkPhoneIsActive.Checked;
                System.String strErr = System.String.Empty;
                System.Int32 iPosNode = treeListPhone.GetNodeIndex(treeListPhone.FocusedNode);

                if (IsPhoneValid(strPhoneNumber, objPhoneType, ref strErr) == false)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show( ( "Телефонный номер не соответсвует принятым требованиям.\n" + strErr), "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    
                    txtPhoneNumber.SelectAll();
                    return;
                }

                if (IsPhoneDublicate(strPhoneNumber, iPosNode, ref strErr) == true)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                    txtPhoneNumber.SelectAll();
                    return;
                }

                if (m_bNewPhone == true)
                {
                    CPhone objPhoneNew = new CPhone();

                    treeListPhone.FocusedNodeChanged -= new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(treeListPhone_FocusedNodeChanged);

                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListPhone.AppendNode(new object[] { null, null, null, null }, null);
                    objNode.Tag = objPhoneNew;
                    treeListPhone.FocusedNode = objNode;

                    treeListPhone.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(treeListPhone_FocusedNodeChanged);
                }

                CPhone objPhone = (CPhone)treeListPhone.FocusedNode.Tag;
                
                objPhone.PhoneNumber = strPhoneNumber;
                objPhone.PhoneType = objPhoneType;
                objPhone.IsMain = bPhoneIsMain;
                objPhone.IsActive = bPhoneIsActive;

                treeListPhone.FocusedNode.SetValue(colPhoneNumber, objPhone.PhoneNumber);
                treeListPhone.FocusedNode.SetValue(colPhoneType, objPhone.PhoneType);
                treeListPhone.FocusedNode.SetValue(colPhoneIsMain, objPhone.IsMain);
                treeListPhone.FocusedNode.SetValue(colPhoneIsActive, objPhone.IsActive);

                // главным може быть только один телефонный номер,
                // поэтому все остальные нужно сделать не главными
                if( objPhone.IsMain == true )
                {
                    foreach( DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListPhone.Nodes )
                    {
                        if( ( treeListPhone.GetNodeIndex( objNode ) != iPosNode ) && ( System.Convert.ToBoolean( objNode.GetValue( colPhoneIsMain ) ) == true ) )
                        {
                            objNode.SetValue( colPhoneIsMain, false );
                            ( (CPhone)objNode.Tag ).IsMain = false;
                        }
                    }
                }

                btnNewPhone.Enabled = true;
                m_bNewPhone = false;
                lblNewPhone.Visible = false;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("ConfirmChangesInPhone. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private void btnNewPhone_Click(object sender, EventArgs e)
        {
            try
            {
                AddPhone();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnPhoneAdd_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private void btnDeletePhone_Click(object sender, EventArgs e)
        {
            try
            {
                DeletePhone(treeListPhone.FocusedNode);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnDeletePhone_Click.\n Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;

        }

        private void btnSavePhone_Click(object sender, EventArgs e)
        {
            try
            {
                ConfirmChangesInPhone();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnSavePhone_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        #endregion

        #region Электронные адреса
        private void btnNewEmail_Click(object sender, EventArgs e)
        {
            try
            {
                AddEMail();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnNewEmail_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// Добавляет в список электронных адресов новую строку
        /// </summary>
        private void AddEMail()
        {
            try
            {
                if (treeListEMail.Enabled == false) { treeListEMail.Enabled = true; }
                if (m_objSelectedCompany.EMailList == null) { m_objSelectedCompany.EMailList = new List<CEMail>(); }

                System.Boolean bNotFullNode = false;
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objItem in treeListEMail.Nodes)
                {
                    if ((System.String)objItem.GetValue(colEMailAddress) == "")
                    {
                        treeListEMail.FocusedNode = objItem;
                        bNotFullNode = true;
                        break;
                    }
                }
                if (bNotFullNode == true) { return; }
                
                m_bNewEmail = true;
                treeListEMail.FocusedNode = null;

                txtEmailAddress.Text = System.String.Empty;
                checkEmailIsActive.Checked = true;
                checkEmailIsMain.Checked = false;

                txtEmailAddress.SelectAll();
                btnNewEmail.Enabled = false;
                lblNewEmail.Visible = true;

                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка добавления электронного адреса.\nТекст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void btnDeleteEmail_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteEMail(treeListEMail.FocusedNode);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnDeleteEmail_Click.\n Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// Удаляет электронный адрес из списка
        /// </summary>
        /// <param name="objNode">узел с электронным адресом</param>
        private void DeleteEMail(DevExpress.XtraTreeList.Nodes.TreeListNode objNode)
        {
            try
            {
                if ((objNode == null) || ( treeListEMail.Nodes.Count == 0))
                {
                    txtEmailAddress.Text = System.String.Empty;
                    checkEmailIsActive.Checked = false;
                    checkEmailIsMain.Checked = false;

                    treeListEMail.FocusedNode = ((treeListEMail.Nodes.Count > 0) ? treeListEMail.Nodes[0] : null);

                    btnNewEmail.Enabled = true;
                    m_bNewEmail = false;
                    lblNewEmail.Visible = false;

                    return;
                }

                if (m_objSelectedCompany.EMailForDeleteList == null) { m_objSelectedCompany.EMailForDeleteList = new List<CEMail>(); }
                DevExpress.XtraTreeList.Nodes.TreeListNode objPrevNode = objNode.PrevNode;
                m_objSelectedCompany.EMailForDeleteList.Add((CEMail)objNode.Tag);

                treeListEMail.Nodes.Remove(objNode);
                if (objPrevNode == null)
                {
                    treeListEMail.FocusedNode = ((treeListEMail.Nodes.Count > 0) ? treeListEMail.Nodes[0] : null);
                }
                else
                {
                    treeListEMail.FocusedNode = objPrevNode;
                }

                LoadEmailNodeInfoInEditor(treeListEMail.FocusedNode);

                btnNewEmail.Enabled = true;
                m_bNewEmail = false;
                lblNewEmail.Visible = false;

                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка удаления электронного адреса.\nТекст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// выполняет проверку электронного адреса
        /// </summary>
        /// <param name="strEMail">электронный адрес</param>
        /// <returns>true - ошибок нет; false - адрес не соответсвует установленным требованиям</returns>
        private System.Boolean IsEMailValid(System.String strEMail, ref System.String strErr)
        {
            System.Boolean bRet = true;
            try
            {
                // электронный адрес должен содержать "@" и "."
                if (strEMail.Trim() == "") 
                { 
                    strErr += ("электронный адрес должен содержать непустые символы.");
                    bRet = false;
                }
                if (strEMail.IndexOf("@") < 0)
                {
                    strErr += ("электронный адрес должен содержать \"@\".");
                    bRet = false;
                }
                //if (strEMail.IndexOf(".") < 0) { return bRet; }
                //{
                //    strErr += ("электронный адрес должен содержать \".\"");
                //    bRet = false;
                //}
            }
            catch (System.Exception f)
            {
                SendMessageToLog(String.Format("Ошибка проверки электронного адреса.\nАдрес: {0}\nТекст ошибки: {1}", strEMail, f.Message));
            }
            finally
            {
            }
            return bRet;
        }
        /// <summary>
        /// Проверяет, дублируется ли адрес в списке
        /// </summary>
        /// <param name="strEMail">электронный адрес</param>
        /// <param name="iEMailPos">позиция адреса в списке</param>
        /// <returns>true - дублируется; false - не ублируется</returns>
        private System.Boolean IsEMailDublicate(System.String strEMail, System.Int32 iEMailPos, ref System.String strErr)
        {
            System.Boolean bRet = false;

            try
            {
                // проверим, возможно такой адрес уже есть в списке
                System.Boolean bDublicate = false;

                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListEMail.Nodes)
                {
                    if (treeListEMail.GetNodeIndex(objNode) != iEMailPos)
                    {
                        if (((System.String)objNode.GetValue(colEMailAddress)) == strEMail)
                        {
                            bDublicate = true;
                            strErr += (String.Format("Указанный электронный адрес уже присутствует в списке: {0}", strEMail));
                            break;
                        }
                    }
                }

                bRet = bDublicate;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка проверки электронного адреса.\nАдрес: " + strEMail + "\nТекст ошибки: " + f.Message);
            }

            return bRet;
        }

        private void LoadEmailNodeInfoInEditor(DevExpress.XtraTreeList.Nodes.TreeListNode objNode)
        {
            try
            {
                txtEmailAddress.Text = "";
                checkEmailIsActive.Checked = false;
                checkEmailIsMain.Checked = false;

                if (( treeListEMail.Nodes.Count == 0) || (objNode == null) || (objNode.Tag == null))
                {
                    return;
                }
                else
                {
                    CEMail objEMail = (CEMail)objNode.Tag;

                    txtEmailAddress.Text = objEMail.EMail;
                    checkEmailIsActive.Checked = objEMail.IsActive;
                    checkEmailIsMain.Checked = objEMail.IsMain;
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("LoadEmailNodeInfoInEditor. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

        private void treeListEMail_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            try
            {
                LoadEmailNodeInfoInEditor(treeListEMail.FocusedNode);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListEMail_FocusedNodeChanged. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

        private void ConfirmChangesInEmail()
        {
            try
            {
                if ((m_bNewEmail == false) && (treeListEMail.Nodes.Count == 0))
                {
                    m_bNewEmail = true;
                }

                if (m_bNewEmail == false)
                {
                    if ((treeListEMail.Nodes.Count == 0) || (treeListEMail.FocusedNode == null) || (treeListEMail.FocusedNode.Tag == null))
                    {
                        return;
                    }
                }

                System.String strEmailAddress = txtEmailAddress.Text;
                System.String strErr = System.String.Empty;
                System.Int32 iPosNode = treeListEMail.GetNodeIndex(treeListEMail.FocusedNode);

                if (IsEMailValid(strEmailAddress, ref strErr) == false)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(("Электронный адрес не соответсвует принятым требованиям.\n" + strErr), "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                    txtEmailAddress.SelectAll();
                    return;
                }

                if (IsEMailDublicate( strEmailAddress, iPosNode, ref strErr) == true)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                    txtEmailAddress.SelectAll();
                    return;
                }

                if (m_bNewEmail == true)
                {
                    CEMail objEmailNew = new CEMail();

                    treeListEMail.FocusedNodeChanged -= new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(treeListEMail_FocusedNodeChanged);

                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListEMail.AppendNode(new object[] { null, null, null }, null);
                    objNode.Tag = objEmailNew;
                    treeListEMail.FocusedNode = objNode;

                    treeListEMail.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(treeListEMail_FocusedNodeChanged);
                }

                CEMail objEmail = (CEMail)treeListEMail.FocusedNode.Tag;

                objEmail.EMail = txtEmailAddress.Text;
                objEmail.IsMain = checkEmailIsMain.Checked;
                objEmail.IsActive = checkEmailIsActive.Checked;

                treeListEMail.FocusedNode.SetValue(colEMailAddress, objEmail.EMail);
                treeListEMail.FocusedNode.SetValue(colIsActiveEMail, objEmail.IsActive);
                treeListEMail.FocusedNode.SetValue(colIsMainEMail, objEmail.IsMain);

                // главным може быть только один адрес
                // поэтому все остальные нужно сделать не главными
                if (objEmail.IsMain == true)
                {
                    foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListEMail.Nodes)
                    {
                        if ((treeListEMail.GetNodeIndex(objNode) != iPosNode) && (System.Convert.ToBoolean(objNode.GetValue(colIsMainEMail)) == true))
                        {
                            objNode.SetValue(colIsMainEMail, false);
                            ((CEMail)objNode.Tag).IsMain = false;
                        }
                    }
                }

                btnNewEmail.Enabled = true;
                m_bNewEmail = false;
                lblNewEmail.Visible = false;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("ConfirmChangesInEmail. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private void btnEmailAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ConfirmChangesInEmail();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnSavePhone_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        #endregion

        #region Лицензии
        private void btnNewLicence_Click(object sender, EventArgs e)
        {
            try
            {
                AddLicence();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnSavePhone_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// режим вставки новой записи
        /// </summary>
        private void AddLicence()
        {
            try
            {

                if (treeListLicence.Enabled == false) { treeListLicence.Enabled = true; }
                if (m_objSelectedCompany.LicenceList == null) { m_objSelectedCompany.LicenceList = new List<CLicence>(); }
                System.Boolean bNotFullNode = false;
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objItem in treeListLicence.Nodes)
                {
                    if ((System.String)objItem.GetValue(colLicenceNum) == "")
                    {
                        treeListLicence.FocusedNode = objItem;
                        bNotFullNode = true;
                        break;
                    }
                }
                if (bNotFullNode == true) { return; }

                m_bNewLicence = true;

                treeListLicence.FocusedNode = null;

                cboxLicenseType.SelectedItem = ((cboxLicenseType.Properties.Items.Count > 0) ? (CLicenceType)cboxLicenseType.Properties.Items[0] : null);
                dtLicenseBeginDate.DateTime = System.DateTime.Today;
                dtLicenseEndDate.DateTime = System.DateTime.Today;

                txtLicenseNumber.SelectAll();
                btnNewLicence.Enabled = false;
                lblNewLicence.Visible = true;

                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка добавления лицензии. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void btnDeleteLicence_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteLicence(treeListPhone.FocusedNode);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnDeleteLicence_Click.\n Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        /// <summary>
        /// Удаляет лицензию из списка
        /// </summary>
        /// <param name="objNode">удаляемый узел в дереве</param>
        private void DeleteLicence(DevExpress.XtraTreeList.Nodes.TreeListNode objNode)
        {
            try
            {
                if ((objNode == null) || (treeListLicence.Nodes.Count == 0))
                {
                    txtLicenseNumber.Text = System.String.Empty;
                    cboxLicenseType.SelectedItem = null;
                    dtLicenseBeginDate.EditValue = null;
                    dtLicenseBeginDate.EditValue = null;
                    txtLicenseWhoGive.Text = System.String.Empty;

                    treeListLicence.FocusedNode = ((treeListLicence.Nodes.Count > 0) ? treeListLicence.Nodes[0] : null);

                    btnNewLicence.Enabled = true;
                    m_bNewLicence = false;
                    lblNewLicence.Visible = false;

                    return;
                }

                if (m_objSelectedCompany.LicenceForDeleteList == null) { m_objSelectedCompany.LicenceForDeleteList = new List<CLicence>(); }
                DevExpress.XtraTreeList.Nodes.TreeListNode objPrevNode = objNode.PrevNode;
                m_objSelectedCompany.LicenceForDeleteList.Add((CLicence)objNode.Tag);

                treeListLicence.Nodes.Remove(objNode);
                if (objPrevNode == null)
                {
                    treeListLicence.FocusedNode = ((treeListLicence.Nodes.Count > 0) ? treeListLicence.Nodes[0] : null);
                }
                else
                {
                    treeListLicence.FocusedNode = objPrevNode;
                }

                LoadLicenseNodeInfoInEditor(treeListPhone.FocusedNode);

                btnNewLicence.Enabled = true;
                m_bNewLicence = false;
                lblNewLicence.Visible = false;

                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка удаления лицензии. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// выполняет проверку номера лицензии
        /// </summary>
        /// <param name="strLicence">номер лицензии</param>
        /// <returns>true - ошибок нет; false - телефонный номер не соответсвует установленным требованиям</returns>
        private System.Boolean IsLicenceValid(System.String strLicence, CLicenceType objLicenceType, System.DateTime dtBeginDate, System.DateTime dtEndDate, 
            System.String strWhoGive, ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (strLicence.Trim() == "") 
                { 
                    strErr +=( "Укажите, пожалуйста, номер лицензии." );
                    return bRet; 
                }
                if (strWhoGive.Trim() == "")
                {
                    strErr += ("Укажите, пожалуйста, орган, выдавший лицензию.");
                    return bRet;
                }
                if (System.DateTime.Compare(dtBeginDate, dtEndDate) > 0)
                {
                    strErr += ("Срок действия лицензии должен быть больше даты выдачи.");
                    return bRet;
                }
                if (objLicenceType == null)
                {
                    strErr += ("Укажите, пожалуйста, тип лицензии!");
                    return bRet;
                }

                bRet = true;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка проверки номера лицензии. Номер: " + strLicence + ". Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return bRet;
        }
        /// <summary>
        /// Проверяет, дублируется ли номер лицензии в списке
        /// </summary>
        /// <param name="strLicence">номер лицензии</param>
        /// <param name="iLicencePos">позиция лицензии в списке</param>
        /// <returns>true - дублируется; false - не ублируется</returns>
        private System.Boolean IsLicenceDublicate(System.String strLicence, System.Int32 iLicencePos, ref System.String strErr)
        {
            System.Boolean bRet = false;

            try
            {
                System.Boolean bDublicate = false;
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListLicence.Nodes)
                {
                    if (treeListLicence.GetNodeIndex(objNode) != iLicencePos)
                    {
                        if (((System.String)objNode.GetValue(colLicenceNum)) == strLicence)
                        {
                            bDublicate = true;
                            strErr += (String.Format("Указанный номер лицензии уже присутствует в списке: {0}", strLicence));
                            break;
                        }
                    }
                }

                bRet = bDublicate;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка проверки номера лицензии. Номер: " + strLicence + ". Текст ошибки: " + f.Message);
            }

            return bRet;
        }

        private void LoadLicenseNodeInfoInEditor(DevExpress.XtraTreeList.Nodes.TreeListNode objNode)
        {
            try
            {
                txtLicenseNumber.Text = "";
                txtLicenseWhoGive.Text = "";
                dtLicenseBeginDate.EditValue = null;
                dtLicenseEndDate.EditValue = null;
                cboxLicenseType.SelectedItem = null;
                
                if (( treeListLicence.Nodes.Count == 0) || (objNode == null) || (objNode.Tag == null))
                {
                    return;
                }
                else
                {
                    CLicence objLicence = (CLicence)objNode.Tag;

                    txtLicenseNumber.Text = objLicence.LicenceNum;
                    txtLicenseWhoGive.Text = objLicence.WhoGive;
                    dtLicenseBeginDate.EditValue = objLicence.BeginDate;
                    dtLicenseEndDate.EditValue = objLicence.EndDate;
                    cboxLicenseType.SelectedItem = ((cboxLicenseType.Properties.Items.Count == 0) ? null : cboxLicenseType.Properties.Items.Cast<CLicenceType>().SingleOrDefault<CLicenceType>(x => x.ID.CompareTo(objLicence.LicenceType.ID) == 0));
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("LoadLicenseNodeInfoInEditor. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private void treeListLicence_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            try
            {
                LoadLicenseNodeInfoInEditor(treeListLicence.FocusedNode);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListLicence_FocusedNodeChanged. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private void ConfirmChangesInLicence()
        {
            try
            {
                if ((m_bNewLicence == false) && (treeListLicence.Nodes.Count == 0))
                {
                    m_bNewLicence = true;
                }

                if ( m_bNewLicence == false)
                {
                    if ((treeListLicence.Nodes.Count == 0) || (treeListLicence.FocusedNode == null) || (treeListLicence.FocusedNode.Tag == null))
                    {
                        return;
                    }
                }

                System.String strLicenceNum = txtLicenseNumber.Text.Trim();
                CLicenceType objLicenceType = ((cboxLicenseType.SelectedItem != null) ? (CLicenceType)cboxLicenseType.SelectedItem : null);
                System.DateTime dtBeginDate = dtLicenseBeginDate.DateTime;
                System.DateTime dtEndDate = dtLicenseEndDate.DateTime;
                System.String strWhoGive = txtLicenseWhoGive.Text.Trim();

                System.String strErr = System.String.Empty;
                System.Int32 iPosNode = treeListLicence.GetNodeIndex(treeListLicence.FocusedNode);

                if ( IsLicenceValid(strLicenceNum, objLicenceType, dtBeginDate, dtEndDate, strWhoGive,  ref strErr) == false)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(("Лицензия не соответсвует принятым требованиям оформления.\n" + strErr), "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                    txtPhoneNumber.SelectAll();
                    return;
                }

                if (IsLicenceDublicate(strLicenceNum, iPosNode, ref strErr) == true)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                    txtPhoneNumber.SelectAll();
                    return;
                }

                if (m_bNewLicence == true)
                {
                    CLicence objLicenceNew = new CLicence();

                    treeListLicence.FocusedNodeChanged -= new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(treeListLicence_FocusedNodeChanged);

                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListLicence.AppendNode(new object[] { null, null, null, null, null }, null);
                    objNode.Tag = objLicenceNew;
                    treeListLicence.FocusedNode = objNode;

                    treeListLicence.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(treeListLicence_FocusedNodeChanged);
                }

                CLicence objLicence = (CLicence)treeListLicence.FocusedNode.Tag;

                objLicence.LicenceNum = txtLicenseNumber.Text.Trim();
                objLicence.LicenceType = (CLicenceType)cboxLicenseType.SelectedItem;
                objLicence.BeginDate = dtLicenseBeginDate.DateTime;
                objLicence.EndDate = dtLicenseEndDate.DateTime;
                objLicence.WhoGive = txtLicenseWhoGive.Text.Trim();

                treeListLicence.FocusedNode.SetValue(colLicenceNum, objLicence.LicenceNum);
                treeListLicence.FocusedNode.SetValue(colLicenceType, objLicence.LicenceType);
                treeListLicence.FocusedNode.SetValue(colLicenceBeginDate, objLicence.BeginDate);
                treeListLicence.FocusedNode.SetValue(colLicenceEndDate, objLicence.EndDate);
                treeListLicence.FocusedNode.SetValue(colWho, objLicence.WhoGive);

                btnNewLicence.Enabled = true;
                m_bNewLicence = false;
                lblNewLicence.Visible = false;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("ConfirmChangesInLicence. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private void btnLicenseAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ConfirmChangesInLicence();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnLicenseAdd_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        #endregion

        #region Расчетные счета
        private void btnNewAccount_Click(object sender, EventArgs e)
        {
            try
            {
                AddAccount();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnNewAccount_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// Добавляет в список расчетный счет
        /// </summary>
        private void AddAccount()
        {
            try
            {
                if (treeListAccounts.Enabled == false) { treeListAccounts.Enabled = true; }
                if (m_objSelectedCompany.AccountList == null) { m_objSelectedCompany.AccountList = new List<CAccount>(); }
                System.Boolean bNotFullNode = false;
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objItem in treeListAccounts.Nodes)
                {
                    if ((System.String)objItem.GetValue(colAccountNumber) == "")
                    {
                        treeListAccounts.FocusedNode = objItem;
                        bNotFullNode = true;
                        break;
                    }
                }
                if (bNotFullNode == true) { return; }

                m_bNewAccount = true;

                treeListAccounts.FocusedNode = null;


                cboxAccountType.SelectedItem = ((cboxAccountType.Properties.Items.Count > 0) ? (CAccountType)cboxAccountType.Properties.Items[0] : null);
                checkAccountIsMain.Checked = false;

                txtAccountNum.SelectAll();
                btnNewAccount.Enabled = false;
                lblNewAccount.Visible = true;

                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка добавления расчетного счета в список. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteAccount(treeListAccounts.FocusedNode);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnDeleteAccount_Click.\n Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;

        }
        /// <summary>
        /// Удаляет расчетный счет из списка
        /// </summary>
        /// <param name="objNode">удаляемый узел в дереве</param>
        private void DeleteAccount(DevExpress.XtraTreeList.Nodes.TreeListNode objNode)
        {
            try
            {
                if ((objNode == null) || (treeListAccounts.Nodes.Count == 0))
                {
                    txtAccountNum.Text = System.String.Empty;
                    txtAccountDecsription.Text = System.String.Empty;
                    cboxAccountType.SelectedItem = null;
                    cboxAccountBank.SelectedItem = null;
                    cboxAccountCurrency.SelectedItem = null;
                    checkAccountIsMain.Checked = false;

                    treeListAccounts.FocusedNode = ((treeListAccounts.Nodes.Count > 0) ? treeListPhone.Nodes[0] : null);

                    btnNewAccount.Enabled = true;
                    m_bNewAccount = false;
                    lblNewAccount.Visible = false;

                    return;
                }

                if (m_objSelectedCompany.AccountForDeleteList == null) { m_objSelectedCompany.AccountForDeleteList = new List<CAccount>(); }
                DevExpress.XtraTreeList.Nodes.TreeListNode objPrevNode = objNode.PrevNode;
                m_objSelectedCompany.AccountForDeleteList.Add((CAccount)objNode.Tag);

                treeListAccounts.Nodes.Remove(objNode);
                if (objPrevNode == null)
                {
                    treeListAccounts.FocusedNode = ((treeListAccounts.Nodes.Count > 0) ? treeListAccounts.Nodes[0] : null);
                }
                else
                {
                    treeListAccounts.FocusedNode = objPrevNode;
                }

                LoadPhoneNodeInfoInEditor(treeListAccounts.FocusedNode);

                btnNewAccount.Enabled = true;
                m_bNewAccount = false;
                lblNewAccount.Visible = false;

                SetPropertiesModified(true);

            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка удаления расчетного счета. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        /// <summary>
        /// выполняет проверку номера расчетного счета
        /// </summary>
        /// <param name="strAccount">номер лицензии</param>
        /// <returns>true - ошибок нет; false - номер расчетного счета не соответсвует установленным требованиям</returns>
        private System.Boolean IsAccountValid(System.String strAccount, CAccountType objAccountType, CBank objBank, CCurrency objCurrency, 
            ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (strAccount.Trim() == "") 
                {
                    strErr += ("\nУкажите, пожалуйста, номер расчётного счёта!");
                    return bRet;
                }
                if (objAccountType == null)
                {
                    strErr += ("\nУкажите, пожалуйста, тип расчётного счёта!");
                    return bRet;
                }
                if (objBank == null)
                {
                    strErr += ("\nУкажите, пожалуйста, банк!");
                    return bRet;
                }
                if (objCurrency == null)
                {
                    strErr += ("\nУкажите, пожалуйста, валюту!");
                    return bRet;
                }

                bRet = true;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка проверки номера расчетного счета. Номер: " + strAccount + ". Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return bRet;
        }
        /// <summary>
        /// Проверяет, дублируется ли номер расченого счета в списке
        /// </summary>
        /// <param name="strAccount">номер расченого счета</param>
        /// <param name="iAccountPos">позиция расченого счета в списке</param>
        /// <returns>true - дублируется; false - не ублируется</returns>
        private System.Boolean IsAccountDublicate(System.String strAccount, System.Int32 iAccountPos, ref System.String strErr)
        {
            System.Boolean bRet = false;

            try
            {
                // проверим, возможно такой номер р/с уже есть в списке
                System.Boolean bDublicate = false;
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListAccounts.Nodes)
                {
                    if (treeListAccounts.GetNodeIndex(objNode) != iAccountPos)
                    {
                        if (((System.String)objNode.GetValue(colPhoneNumber)) == strAccount)
                        {
                            bDublicate = true;
                            strErr += (String.Format("Расчётный счёт с указанным номером уже присутствует в списке: {0}", strAccount));
                            break;
                        }
                    }
                }

                bRet = bDublicate;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка проверки номера расченого счета. Номер: " + strAccount + ". Текст ошибки: " + f.Message);
            }

            return bRet;
        }

        /// <summary>
        /// Проверяет, дублируется ли комбинация из номер р/с, валюты, банка в списке
        /// </summary>
        private System.Boolean IsAccountFullDublicate()
        {
            System.Boolean bRet = false;
            int i, j;
            try
            {
                System.Boolean bDublicate = false;
                for ( i = 0; i < treeListAccounts.Nodes.Count; i++)
                for ( j = i+1; j < treeListAccounts.Nodes.Count; j++)
                {
                    if (i != treeListAccounts.Nodes.Count - 1)
                    {
                        if (((System.String)treeListAccounts.Nodes[i].GetValue(colAccountNumber)) == ((System.String)treeListAccounts.Nodes[j].GetValue(colAccountNumber)))
                        {
                            if (treeListAccounts.Nodes[i].GetValue(colAccountCurrency).ToString() == treeListAccounts.Nodes[j].GetValue(colAccountCurrency).ToString())
                            {
                                if (treeListAccounts.Nodes[i].GetValue(colAccountBank).ToString() == treeListAccounts.Nodes[j].GetValue(colAccountBank).ToString())
                                {
                                    //MessageBox.Show("Дубликат р/с, валюты и кодов банка");
                                    bDublicate = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                bRet = bDublicate;
            }
            catch (Exception f)
            {
                SendMessageToLog("Ошибка проверки уникальность комбинации номера р/с, валюты, банка." + "Текст ошибки: " + f.Message);
            }

            return bRet;
        }

        private void LoadAccountNodeInfoInEditor(DevExpress.XtraTreeList.Nodes.TreeListNode objNode)
        {
            try
            {
                txtAccountNum.Text = "";
                txtAccountDecsription.Text = "";
                cboxAccountBank.SelectedItem = null;
                cboxAccountCurrency.SelectedItem = null;
                cboxAccountType.SelectedItem = null;
                checkAccountIsMain.Checked = false;

                if ((treeListAccounts.Nodes.Count == 0) || (objNode == null) || (objNode.Tag == null))
                {
                    return;
                }
                else
                {
                    CAccount objAccount = (CAccount)objNode.Tag;

                    txtAccountNum.Text = objAccount.AccountNumber;
                    txtAccountDecsription.Text = objAccount.Description;
                    cboxAccountBank.SelectedItem = ((cboxAccountBank.Properties.Items.Count == 0) ? null : cboxAccountBank.Properties.Items.Cast<CBank>().SingleOrDefault<CBank>(x => x.ID.CompareTo(objAccount.Bank.ID) == 0));
                    cboxAccountCurrency.SelectedItem = ((cboxAccountCurrency.Properties.Items.Count == 0) ? null : cboxAccountCurrency.Properties.Items.Cast<CCurrency>().SingleOrDefault<CCurrency>(x => x.ID.CompareTo(objAccount.Currency.ID) == 0));
                    cboxAccountType.SelectedItem = ((cboxAccountType.Properties.Items.Count == 0) ? null : cboxAccountType.Properties.Items.Cast<CAccountType>().SingleOrDefault<CAccountType>(x => x.ID.CompareTo(objAccount.AccountType.ID) == 0));
                    checkAccountIsMain.Checked = objAccount.IsMain;
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("LoadAccountNodeInfoInEditor. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void treeListAccounts_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            try
            {
                LoadAccountNodeInfoInEditor(treeListAccounts.FocusedNode);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListAccounts_FocusedNodeChanged. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private void ConfirmChangesInAccount()
        {
            try
            {
                if ((m_bNewAccount == false) && (treeListAccounts.Nodes.Count == 0))
                {
                    m_bNewAccount = true;
                }

                if (m_bNewAccount == false)
                {
                    if ((treeListAccounts.Nodes.Count == 0) || (treeListAccounts.FocusedNode == null) || (treeListAccounts.FocusedNode.Tag == null))
                    {
                        return;
                    }
                }

                System.String strAccountNumber = txtAccountNum.Text.Trim();
                CBank objBank = ((cboxAccountBank.SelectedItem != null) ? (CBank)cboxAccountBank.SelectedItem : null);
                CCurrency objCurrency = ((cboxAccountCurrency.SelectedItem != null) ? (CCurrency)cboxAccountCurrency.SelectedItem : null);
                CAccountType objAccountType = ((cboxAccountType.SelectedItem != null) ? (CAccountType)cboxAccountType.SelectedItem : null); 
                System.Boolean bIsMain = checkAccountIsMain.Checked;
                System.String strDescription = txtAccountDecsription.Text.Trim();
                
                System.String strErr = System.String.Empty;
                System.Int32 iPosNode = treeListPhone.GetNodeIndex(treeListPhone.FocusedNode);

                if ( IsAccountValid( strAccountNumber, objAccountType, objBank, objCurrency,  ref strErr) == false)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(("Расчётный счёт не соответсвует принятым требованиям.\n" + strErr), "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                    txtAccountNum.SelectAll();
                    return;
                }

                if (IsAccountDublicate(strAccountNumber, iPosNode, ref strErr) == true)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                    txtAccountNum.SelectAll();
                    return;
                }

                if (m_bNewAccount == true)
                {
                    CAccount objAccountNew = new CAccount();

                    treeListAccounts.FocusedNodeChanged -= new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(treeListAccounts_FocusedNodeChanged);

                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListAccounts.AppendNode(new object[] { null, null, null, null, null, null }, null);
                    objNode.Tag = objAccountNew;
                    treeListAccounts.FocusedNode = objNode;

                    treeListAccounts.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(treeListAccounts_FocusedNodeChanged);
                }

                CAccount objAccount = (CAccount)treeListAccounts.FocusedNode.Tag;

                objAccount.AccountNumber = strAccountNumber;
                objAccount.Bank = objBank;
                objAccount.Currency = objCurrency;
                objAccount.AccountType = objAccountType;
                objAccount.IsMain = bIsMain;
                objAccount.Description = strDescription;

                treeListAccounts.FocusedNode.SetValue(colAccountBank, objAccount.Bank);
                treeListAccounts.FocusedNode.SetValue(colAccountCurrency, objAccount.Currency);
                treeListAccounts.FocusedNode.SetValue(colAccountType, objAccount.AccountType);
                treeListAccounts.FocusedNode.SetValue(colAccountNumber, objAccount.AccountNumber);
                treeListAccounts.FocusedNode.SetValue(colAccountDscrpn, objAccount.Description);
                treeListAccounts.FocusedNode.SetValue(colAccountIsMain, objAccount.IsMain);

                // главным може быть только один расчётный счёт
                // поэтому все остальные нужно сделать не главными
                if (objAccount.IsMain == true)
                {
                    foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListAccounts.Nodes)
                    {
                        if ((treeListAccounts.GetNodeIndex(objNode) != iPosNode) && (System.Convert.ToBoolean(objNode.GetValue(colAccountIsMain)) == true))
                        {
                            objNode.SetValue(colAccountIsMain, false);
                            ((CAccount)objNode.Tag).IsMain = false;
                        }
                    }
                }

                btnNewAccount.Enabled = true;
                m_bNewAccount = false;
                lblNewAccount.Visible = false;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("ConfirmChangesInAccount. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private void btnAccountAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ConfirmChangesInAccount();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnAccountAdd_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;

        }

        #endregion

        #region Журнал сообщений
        private void SendMessageToLog(System.String strMessage)
        {
            try
            {
                m_objMenuItem.SimulateNewMessage(strMessage);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "SendMessageToLog.\n Текст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        private void SetWarningInfo(System.String strMessage)
        {
            try
            {
                lblWarningInfo.Text = strMessage;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "SetWarningInfo.\n Текст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
        private void ShowWarningPnl(System.Boolean bShow)
        {
            System.Int32 iRowWarnigHeith = 45;
            System.Int32 iRowBtnHeith = 30;
            try
            {
                if (bShow == true)
                {
                    tableLayoutPanel2.RowStyles[1].Height = iRowWarnigHeith;
                }
                else
                {
                    tableLayoutPanel2.RowStyles[1].Height = 0;
                }
                tableLayoutPanel2.Size = new Size(tableLayoutPanel2.Size.Width,
                    (System.Convert.ToInt32(tableLayoutPanel2.RowStyles[1].Height + iRowBtnHeith)));
                tableLayoutPanel2.Refresh();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "ShowWarningPnl.\n Текст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Редактировать компанию
        /// <summary>
        /// Проверяет содержимое элементов управления
        /// </summary>
        private System.Boolean ValidateProperties()
        {
            System.Boolean bRet = true;
            try
            {
                cboxTypeCompany.Properties.Appearance.BackColor = ((cboxTypeCompany.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                cboxState.Properties.Appearance.BackColor = ((cboxState.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                txtFullName.Properties.Appearance.BackColor = ((txtFullName.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                txtAсronym.Properties.Appearance.BackColor = ((txtAсronym.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                txtUNP.Properties.Appearance.BackColor = ((txtUNP.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                txtOKULP.Properties.Appearance.BackColor = ((txtOKULP.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                txtOKPO.Properties.Appearance.BackColor = ((txtOKPO.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);

                if (cboxTypeCompany.SelectedItem == null) { bRet = false; }
                if (cboxState.SelectedItem == null) { bRet = false; }
                if (txtFullName.Text.Trim().Length == 0) { bRet = false; }
                if (txtAсronym.Text.Trim().Length == 0) { bRet = false; }
                if (txtUNP.Text.Trim().Length == 0) { bRet = false; }
                if (txtFullName.Text.Trim().Length == 0) { bRet = false; }

            }
            catch (System.Exception f)
            {
                SendMessageToLog("ValidateProperties. Текст ошибки: " + f.Message);
            }

            return bRet;
        }
        /// <summary>
        /// очистка содержимого элементов управления
        /// </summary>
        private void ClearControls()
        {
            try
            {
                txtFullName.Text = "";
                txtAсronym.Text = "";
                txtUNP.Text = "";
                txtOKULP.Text = "";
                txtOKPO.Text = "";
                txtDescription.Text = "";
                txtCod.Text = "";
                cboxState.SelectedItem = null;
                cboxTypeCompany.SelectedItem = null;
                checkActive.Checked = false;

                treeListAccounts.Nodes.Clear();
                treeListEMail.Nodes.Clear();
                treeListPhone.Nodes.Clear();
                treeListLicence.Nodes.Clear();

                if (m_objSelectedCompany.ContactForDeleteList == null) { m_objSelectedCompany.ContactForDeleteList = new List<CContact>(); }
                else { m_objSelectedCompany.ContactForDeleteList.Clear(); }
                if (m_objSelectedCompany.AddressForDeleteList == null) { m_objSelectedCompany.AddressForDeleteList = new List<CAddress>(); }
                else { m_objSelectedCompany.AddressForDeleteList.Clear(); }
                if (m_objSelectedCompany.PhoneForDeleteList == null) { m_objSelectedCompany.PhoneForDeleteList = new List<CPhone>(); }
                else { m_objSelectedCompany.PhoneForDeleteList.Clear(); }
                if (m_objSelectedCompany.LicenceForDeleteList == null) { m_objSelectedCompany.LicenceForDeleteList = new List<CLicence>(); }
                else { m_objSelectedCompany.LicenceForDeleteList.Clear(); }
                if (m_objSelectedCompany.AccountForDeleteList == null) { m_objSelectedCompany.AccountForDeleteList = new List<CAccount>(); }
                else { m_objSelectedCompany.AccountForDeleteList.Clear(); }
                if (m_objSelectedCompany.EMailForDeleteList == null) { m_objSelectedCompany.EMailForDeleteList = new List<CEMail>(); }
                else { m_objSelectedCompany.EMailForDeleteList.Clear(); }

            }
            catch (System.Exception f)
            {
                SendMessageToLog("ClearControls. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// Загружает свойства компании для редактирования/подробного порсмотра
        /// </summary>
        /// <param name="objCompany">компания</param
        public void EditCompany(ERP_Mercury.Common.CCompany objCompany)
        {
            if (objCompany == null) { return; }

            m_bDisableEvents = true;
            m_bNewCompany = false;
            ShowWarningPnl(false); 
            try
            {
                System.String strErr = "";

                m_objSelectedCompany = objCompany;
                if (m_objSelectedCompany.AddressList != null)
                {
                    m_objSelectedCompany.AddressList.Clear();
                }                
                
                m_objSelectedCompany.PhoneList = CPhone.GetPhoneListForCompany(m_objProfile, null, m_objSelectedCompany.ID, ref strErr);
                m_objSelectedCompany.LicenceList = CLicence.GetLicenceListForCompany(m_objSelectedCompany.ID, m_objProfile, null); 
                m_objSelectedCompany.AccountList = CAccount.GetAccountListForCompany(m_objProfile, null, m_objSelectedCompany.ID, ref strErr);
                m_objSelectedCompany.EMailList = CEMail.GetEMailListForContact(m_objProfile, null, EnumObject.Company, m_objSelectedCompany.ID, ref strErr);
                
                ClearControls();

                LoadSelectedCompanyCompanyPropertiesInControls();

                SetPropertiesModified(false);

                btnCancel.Enabled = true;
                btnCancel.Focus();

                SetModeReadOnly(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования компании. Текст ошибки: " + f.Message);
            }
            finally
            {
                m_bDisableEvents = false;
            }
            return;
        }

        private void LoadSelectedCompanyCompanyPropertiesInControls()
        {
            if (m_objSelectedCompany == null) { return; }
            try
            {
                txtFullName.Text = m_objSelectedCompany.Name;
                txtCod.Text = Convert.ToString(m_objSelectedCompany.InterBaseID);
                txtAсronym.Text = m_objSelectedCompany.Abbr;
                txtUNP.Text = m_objSelectedCompany.UNP;
                txtOKPO.Text = m_objSelectedCompany.OKPO;
                txtOKULP.Text = m_objSelectedCompany.OKULP;
                txtDescription.Text = m_objSelectedCompany.Description;
                checkActive.Checked = m_objSelectedCompany.IsActive;
 
                cboxState.SelectedItem = (m_objSelectedCompany.StateType == null) ? null : cboxState.Properties.Items.Cast<CStateTypeCompany>().SingleOrDefault<CStateTypeCompany>(x => x.ID.CompareTo(m_objSelectedCompany.StateType.ID) == 0);
                cboxTypeCompany.SelectedItem = (m_objSelectedCompany.CompanyType == null) ? null : cboxTypeCompany.Properties.Items.Cast<CCompanyType>().SingleOrDefault<CCompanyType>(x => x.ID.CompareTo(m_objSelectedCompany.CompanyType.ID) == 0);

                // Лицензии
                if (m_objSelectedCompany.LicenceList != null)
                {
                    foreach (CLicence objLicence in m_objSelectedCompany.LicenceList)
                    {
                        treeListLicence.AppendNode(new object[] { objLicence.LicenceType, objLicence.LicenceNum, objLicence.BeginDate, objLicence.EndDate, objLicence.WhoGive }, null).Tag = objLicence;
                    }
                    treeListLicence.FocusedNode = ((treeListLicence.Nodes.Count > 0) ? treeListLicence.Nodes[0] : null);
                    LoadLicenseNodeInfoInEditor(treeListLicence.FocusedNode);
                }

                // Телефоны
                if (m_objSelectedCompany.PhoneList != null)
                {
                    foreach (CPhone objPhone in m_objSelectedCompany.PhoneList)
                    {
                        treeListPhone.AppendNode(new object[] { objPhone.PhoneType, objPhone.PhoneNumber, objPhone.IsActive, objPhone.IsMain }, null).Tag = objPhone;
                    }
                    treeListPhone.FocusedNode = ((treeListPhone.Nodes.Count > 0) ? treeListPhone.Nodes[0] : null);
                    LoadPhoneNodeInfoInEditor(treeListPhone.FocusedNode);
                }
                // Электронные адреса
                if (m_objSelectedCompany.EMailList != null)
                {
                    foreach (CEMail objEMail in m_objSelectedCompany.EMailList)
                    {
                        treeListEMail.AppendNode(new object[] { objEMail.EMail, objEMail.IsActive, objEMail.IsMain }, null).Tag = objEMail;
                    }

                    treeListEMail.FocusedNode = ((treeListEMail.Nodes.Count > 0) ? treeListEMail.Nodes[0] : null);
                    LoadEmailNodeInfoInEditor(treeListEMail.FocusedNode);
                }
                // Расчетные счета
                if (m_objSelectedCompany.AccountList != null)
                {
                    foreach (CAccount objAccount in m_objSelectedCompany.AccountList)
                    {
                        treeListAccounts.AppendNode(new object[] { objAccount.AccountType, objAccount.AccountNumber, objAccount.Currency, objAccount.Bank, objAccount.Description, objAccount.IsMain }, null).Tag = objAccount;
                    }
                    treeListAccounts.FocusedNode = ((treeListAccounts.Nodes.Count > 0) ? treeListAccounts.Nodes[0] : null);
                    LoadAccountNodeInfoInEditor(treeListAccounts.FocusedNode);
                }

                //layoutControlGroupAccounts.Expanded = true;

                // Адреса
                frmAddress.ClearAddressList();
                frmAddress.ClearAllPropertiesControls();
                frmContact.ClearAllPropertiesControls();
                frmAddress.LoadAddressList(m_objSelectedCompany.ID, m_objSelectedCompany.AddressList);

                // Контакты
                frmContact.ClearContactListTree();
                frmContact.ClearAllPropertiesControls();
                frmContact.LoadContactList(m_objSelectedCompany.ID, m_objSelectedCompany.ContactList);

            }
            catch (System.Exception f)
            {
                SendMessageToLog("LoadCompanyPropertiesInControls. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Refresh();
                //frmAddress.StartThreadWithLoadData(); // потоки
                //frmContact.frmAddress.StartThreadWithLoadData();

                SetModeReadOnly(false);
                btnEdit.Enabled = false;
                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("SetModeReadOnly. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        #endregion

        #region Новая компания
        /// <summary>
        /// Новый клиент
        /// </summary>
        public void NewCompany()
        {
            m_bDisableEvents = true;
            m_bNewCompany = true;
            ShowWarningPnl(false);
            try
            {
                this.Refresh();
                
                //frmAddress.StartThreadWithLoadData();
                //frmContact.frmAddress.StartThreadWithLoadData();

                m_objSelectedCompany = new ERP_Mercury.Common.CCompany();

                //this.SuspendLayout();

                ClearControls();

                LoadSelectedCompanyCompanyPropertiesInControls();

                SetModeReadOnly(false);
                btnEdit.Enabled = false;
                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка создания клиента. Текст ошибки: " + f.Message);
            }
            finally
            {
                //this.ResumeLayout(false);
                m_bDisableEvents = false;
            }
            return;
        }

        #endregion

        #region Отмена
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Cancel();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка отмены изменений в описании клиента. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        /// <summary>
        /// Отмена внесенных изменений
        /// </summary>
        private void Cancel()
        {
            try
            {
                SimulateChangeCompanyProperties(m_objSelectedCompany, enumActionSaveCancel.Cancel, m_bNewCompany);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка отмены изменений в описании клиента. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;

        }
        #endregion

        #region Сохранить изменения
        /// <summary>
        /// Сохраняет изменения в базе данных
        /// </summary>
        /// <returns>true - удачное завершение операции;false - ошибка</returns>
        private System.Boolean bSaveChanges()
        {
            System.Boolean bRet = false;
            System.Boolean bOkSave = false;
            CCompany objCompanyForSave = new CCompany();

            try
            {
                objCompanyForSave.ID = m_objSelectedCompany.ID;
                objCompanyForSave.InterBaseID = m_objSelectedCompany.InterBaseID;
                objCompanyForSave.Name = txtFullName.Text;
                objCompanyForSave.Abbr =txtAсronym.Text;
                objCompanyForSave.UNP = txtUNP.Text;
                objCompanyForSave.OKPO = txtOKPO.Text;
                objCompanyForSave.OKULP = txtOKULP.Text;
                objCompanyForSave.Description = txtDescription.Text;
                objCompanyForSave.IsActive = checkActive.Checked;

                objCompanyForSave.StateType = ((cboxState.SelectedItem == null) ? null : (CStateTypeCompany)cboxState.SelectedItem);
                objCompanyForSave.CompanyType = ((cboxTypeCompany.SelectedItem == null) ? null : (CCompanyType)cboxTypeCompany.SelectedItem);

                if (objCompanyForSave.PhoneList == null) { objCompanyForSave.PhoneList = new List<CPhone>(); }
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListPhone.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    objCompanyForSave.PhoneList.Add((CPhone)objNode.Tag);
                }
                objCompanyForSave.PhoneForDeleteList = m_objSelectedCompany.PhoneForDeleteList;

                if (objCompanyForSave.LicenceList == null) { objCompanyForSave.LicenceList = new List<CLicence>(); }
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListLicence.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    objCompanyForSave.LicenceList.Add((CLicence)objNode.Tag);
                }
                objCompanyForSave.LicenceForDeleteList = m_objSelectedCompany.LicenceForDeleteList;

                if (objCompanyForSave.AccountList == null) { objCompanyForSave.AccountList = new List<CAccount>(); }
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListAccounts.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    objCompanyForSave.AccountList.Add((CAccount)objNode.Tag);
                }
                objCompanyForSave.AccountForDeleteList = m_objSelectedCompany.AccountForDeleteList;

                if (objCompanyForSave.EMailList == null) { objCompanyForSave.EMailList = new List<CEMail>(); }
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListEMail.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    objCompanyForSave.EMailList.Add((CEMail)objNode.Tag);
                }
                objCompanyForSave.EMailForDeleteList = m_objSelectedCompany.EMailForDeleteList;
                
                objCompanyForSave.ContactList = frmContact.ContactList;
                objCompanyForSave.ContactForDeleteList = frmContact.ContactDeletedList;

                if (objCompanyForSave.AddressList == null) { objCompanyForSave.AddressList = new List<CAddress>(); }
                objCompanyForSave.AddressList.Clear();
                objCompanyForSave.AddressList.AddRange(frmAddress.AddressList);

                if (objCompanyForSave.AddressForDeleteList == null) { objCompanyForSave.AddressForDeleteList = new List<CAddress>(); }
                objCompanyForSave.AddressForDeleteList.Clear();
                objCompanyForSave.AddressForDeleteList.AddRange(frmAddress.AddressDeletedList);
                objCompanyForSave.AddressForDeleteList.AddRange(frmContact.frmAddress.AddressDeletedList);

                System.String strErr = "";
                System.Int32 iRes = 0;

                System.String CompanyName = objCompanyForSave.Name;
                System.String CompanyAddress = (((objCompanyForSave.AddressList != null) && (objCompanyForSave.AddressList.Count > 0)) ? objCompanyForSave.AddressList[0].VisitingCard2 : System.String.Empty);
                System.String CompanyAcronym = objCompanyForSave.Abbr;
                System.String CompanyAccount = (((objCompanyForSave.AccountList != null) && (objCompanyForSave.AccountList.Count > 0)) ? objCompanyForSave.AccountList[0].AccountNumber : System.String.Empty);
                System.String CompanyBankCode = (((objCompanyForSave.AccountList != null) && (objCompanyForSave.AccountList.Count > 0)) ? objCompanyForSave.AccountList[0].Bank.Code : System.String.Empty);  
                System.String CompanyBankName = (((objCompanyForSave.AccountList != null) && (objCompanyForSave.AccountList.Count > 0)) ? objCompanyForSave.AccountList[0].BankName : System.String.Empty);
                System.String CompanyUNN = objCompanyForSave.UNP;
                System.String CompanyOKPO = objCompanyForSave.OKPO;
                System.String CompanyOKULP = objCompanyForSave.OKULP;
                System.String CompanyLicenceImport = (((objCompanyForSave.LicenceList != null) && (objCompanyForSave.LicenceList.Count > 0)) ? (objCompanyForSave.LicenceList[0].LicenceNum + " от " + objCompanyForSave.LicenceList[0].BeginDate.ToShortDateString() ) : System.String.Empty );
                System.String CompanyLicenceTrade = (((objCompanyForSave.LicenceList != null) && (objCompanyForSave.LicenceList.Count > 0)) ? (objCompanyForSave.LicenceList[0].LicenceNum + " от " + objCompanyForSave.LicenceList[0].BeginDate.ToShortDateString()) : System.String.Empty);
                System.Boolean CompanyIsActive = objCompanyForSave.IsActive;
                System.Int32 Company_id = objCompanyForSave.InterBaseID;

                System.Guid CompanyType_Guid = objCompanyForSave.CompanyType.ID; 
                System.String Company_Description = objCompanyForSave.Description; 
                System.Guid CustomerStateType_Guid = objCompanyForSave.StateType.ID;
                System.Guid Company_Guid = objCompanyForSave.ID;

                if (m_bNewCompany == true)
                {
                    // новая запись
                    if (CCompany.AddCompanyToIB(m_objProfile, null, CompanyName, CompanyAddress, CompanyAcronym,
                            CompanyAccount, CompanyBankCode, CompanyBankName, CompanyUNN,
                            CompanyOKPO, CompanyOKULP, CompanyLicenceImport, CompanyLicenceTrade,
                            CompanyIsActive, ref Company_id, ref strErr, ref iRes) == true)
                    {
                        bOkSave = CCompany.AddCompanyToDB(m_objProfile, null, Company_id, CompanyType_Guid, CompanyAcronym, Company_Description, CompanyName,
                            CompanyOKPO, CompanyOKULP, CompanyUNN, CompanyIsActive, CustomerStateType_Guid,
                            objCompanyForSave.ContactList, objCompanyForSave.ContactForDeleteList,
                            objCompanyForSave.AddressList, objCompanyForSave.AddressForDeleteList,
                            objCompanyForSave.LicenceList, objCompanyForSave.LicenceForDeleteList,
                            objCompanyForSave.PhoneList, objCompanyForSave.PhoneForDeleteList,
                            objCompanyForSave.EMailList, objCompanyForSave.EMailForDeleteList,
                            objCompanyForSave.AccountList, objCompanyForSave.AccountForDeleteList,
                            ref Company_Guid, ref strErr, ref iRes);
                    }
                }
                else
                {
                    if (CCompany.EditCompanyInIB(m_objProfile, null, Company_id, CompanyName, CompanyAddress, CompanyAcronym,
                            CompanyAccount, CompanyBankCode, CompanyBankName, CompanyUNN,
                            CompanyOKPO, CompanyOKULP, CompanyLicenceImport, CompanyLicenceTrade,
                            CompanyIsActive, ref strErr, ref iRes) == true)
                    {
                        bOkSave = CCompany.EditCompanyInDB(m_objProfile, null, Company_Guid, CompanyType_Guid, 
                            CompanyAcronym, Company_Description, CompanyName,
                            CompanyOKPO, CompanyOKULP, CompanyUNN, CompanyIsActive, CustomerStateType_Guid,
                            objCompanyForSave.ContactList, objCompanyForSave.ContactForDeleteList,
                            objCompanyForSave.AddressList, objCompanyForSave.AddressForDeleteList,
                            objCompanyForSave.LicenceList, objCompanyForSave.LicenceForDeleteList,
                            objCompanyForSave.PhoneList, objCompanyForSave.PhoneForDeleteList,
                            objCompanyForSave.EMailList, objCompanyForSave.EMailForDeleteList,
                            objCompanyForSave.AccountList, objCompanyForSave.AccountForDeleteList,
                            ref strErr, ref iRes);
                    }
                }
                SendMessageToLog(strErr);
                if (bOkSave == true)
                {
                    m_objSelectedCompany.ID = objCompanyForSave.ID;
                    m_objSelectedCompany.Name = objCompanyForSave.Name;
                    m_objSelectedCompany.Abbr = objCompanyForSave.Abbr;
                    m_objSelectedCompany.InterBaseID = objCompanyForSave.InterBaseID;  
                    m_objSelectedCompany.Description = objCompanyForSave.Description;
                    m_objSelectedCompany.UNP = objCompanyForSave.UNP;
                    m_objSelectedCompany.OKPO = objCompanyForSave.OKPO;
                    m_objSelectedCompany.OKULP = objCompanyForSave.OKULP;
                    m_objSelectedCompany.IsActive = objCompanyForSave.IsActive;
                    m_objSelectedCompany.StateType = objCompanyForSave.StateType;
                    m_objSelectedCompany.CompanyType = objCompanyForSave.CompanyType;
                    m_objSelectedCompany.PhoneList = objCompanyForSave.PhoneList;
                    m_objSelectedCompany.EMailList = objCompanyForSave.EMailList;
                    m_objSelectedCompany.LicenceList = objCompanyForSave.LicenceList;
                    m_objSelectedCompany.ContactList = objCompanyForSave.ContactList;
                    m_objSelectedCompany.AddressList = objCompanyForSave.AddressList;

                    if (frmAddress.AddressDeletedList != null) { frmAddress.AddressDeletedList.Clear(); }
                    if (frmContact.ContactDeletedList != null) { frmContact.ContactDeletedList.Clear(); }

                    if (m_objSelectedCompany.AddressForDeleteList != null) { m_objSelectedCompany.AddressForDeleteList.Clear(); }

                    if (m_objSelectedCompany.LicenceForDeleteList != null) { m_objSelectedCompany.LicenceForDeleteList.Clear(); }
                    if (m_objSelectedCompany.PhoneForDeleteList != null) { m_objSelectedCompany.PhoneForDeleteList.Clear(); }
                    if (m_objSelectedCompany.AccountForDeleteList != null) { m_objSelectedCompany.AccountForDeleteList.Clear(); }
                    if (m_objSelectedCompany.EMailForDeleteList != null) { m_objSelectedCompany.EMailForDeleteList.Clear(); }

                    ShowWarningPnl(false);
                    bRet = true; 
                }
                else
                {
                    SetWarningInfo(strErr);
                    ShowWarningPnl(true);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в описании компании. Текст ошибки: " + f.Message);
            }
            finally
            {
                objCompanyForSave = null;
            }
            return bRet;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                lblWarningInfo.Text = "";
                if (IsAccountFullDublicate())
                {
                    lblWarningInfo.Text = "Комбинация расчётного счёта, валюты и банка должна быть уникальной.";
                }
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    frmAddress.ConfirmChanges();
                    frmContact.ConfirmChanges();
                    if (bSaveChanges() == true)
                    {
                       SimulateChangeCompanyProperties(m_objSelectedCompany, enumActionSaveCancel.Save, m_bNewCompany);
                    }
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в описании клиента. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            return;
        }

        #endregion
        
        #region Режим просмотра/редактирования
        /// <summary>
        /// Устанавливает режим просмотра/редактирования
        /// </summary>
        /// <param name="bSet">true - режим просмотра; false - режим редактирования</param>
        public void SetModeReadOnly(System.Boolean bSet)
        {
            try
            {
                txtFullName.Properties.ReadOnly = bSet;
                txtAсronym.Properties.ReadOnly = bSet;
                txtUNP.Properties.ReadOnly = bSet;
                txtOKPO.Properties.ReadOnly = bSet;
                txtOKULP.Properties.ReadOnly = bSet;
                txtDescription.Properties.ReadOnly = bSet;
                checkActive.Properties.ReadOnly = bSet;

                txtPhoneNumber.Properties.ReadOnly = bSet;
                cboxPhoneType.Properties.ReadOnly = bSet;
                checkPhoneIsActive.Properties.ReadOnly = bSet;
                checkPhoneIsMain.Properties.ReadOnly = bSet;
                btnNewPhone.Enabled = !bSet;
                btnSavePhone.Enabled = !bSet;
                btnDeletePhone.Enabled = !bSet;

                txtEmailAddress.Properties.ReadOnly = bSet;
                checkEmailIsActive.Properties.ReadOnly = bSet;
                checkEmailIsMain.Properties.ReadOnly = bSet;
                btnNewEmail.Enabled = !bSet;
                btnEmailAdd.Enabled = !bSet;
                btnDeleteEmail.Enabled = !bSet;

                txtLicenseNumber.Properties.ReadOnly = bSet;
                txtLicenseWhoGive.Properties.ReadOnly = bSet;
                cboxLicenseType.Properties.ReadOnly = bSet;
                dtLicenseBeginDate.Properties.ReadOnly = bSet;
                dtLicenseEndDate.Properties.ReadOnly = bSet;
                btnNewLicence.Enabled = !bSet;
                btnLicenseAdd.Enabled = !bSet;
                btnDeleteLicence.Enabled = !bSet;

                txtAccountNum.Properties.ReadOnly = bSet;
                txtAccountDecsription.Properties.ReadOnly = bSet;
                cboxAccountType.Properties.ReadOnly = bSet;
                cboxAccountBank.Properties.ReadOnly = bSet;
                cboxAccountCurrency.Properties.ReadOnly = bSet;
                checkAccountIsMain.Properties.ReadOnly = bSet;
                btnNewAccount.Enabled = !bSet;
                btnAccountAdd.Enabled = !bSet;
                btnDeleteAccount.Enabled = !bSet;


                cboxState.Properties.ReadOnly = bSet;
                cboxTypeCompany.Properties.ReadOnly = bSet;

                frmAddress.SetChanceEditProperties(!bSet);
                frmContact.SetChanceEditProperties(!bSet);

                lblNewPhone.Visible = false;

                m_bIsReadOnly = bSet;
                btnEdit.Enabled = bSet;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("SetModeReadOnly. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        #endregion

        #region layoutControl

        private void layoutControlPhone_GroupExpandChanged(object sender, DevExpress.XtraLayout.Utils.LayoutGroupEventArgs e)
        {
            if (e.Group.Expanded == true)
            {
                if (e.Group == layoutControlGroupPhone)
                {
                    layoutControlGroupLicense.Expanded = false;
                    layoutControlGroupAccounts.Expanded = false;
                    layoutControlGroupEMail.Expanded = false;

                    if ((treeListPhone.Nodes.Count > 0) && (treeListPhone.FocusedNode == null))
                    {
                        treeListPhone.FocusedNode = treeListPhone.Nodes[0];
                        LoadPhoneNodeInfoInEditor(treeListPhone.FocusedNode);
                    }
                }
                if (e.Group == layoutControlGroupLicense)
                {
                    layoutControlGroupPhone.Expanded = false;
                    layoutControlGroupAccounts.Expanded = false;
                    layoutControlGroupEMail.Expanded = false;

                    if ((treeListLicence.Nodes.Count > 0) && (treeListLicence.FocusedNode == null))
                    {
                        treeListLicence.FocusedNode = treeListLicence.Nodes[0];
                        LoadLicenseNodeInfoInEditor(treeListLicence.FocusedNode);
                    }
                }
                if (e.Group == layoutControlGroupAccounts)
                {
                    layoutControlGroupPhone.Expanded = false;
                    layoutControlGroupLicense.Expanded = false;
                    layoutControlGroupEMail.Expanded = false;

                    if ((treeListAccounts.Nodes.Count > 0) && (treeListAccounts.FocusedNode == null))
                    {
                        treeListAccounts.FocusedNode = treeListAccounts.Nodes[0];
                        LoadAccountNodeInfoInEditor(treeListAccounts.FocusedNode);
                    }
                }
                if (e.Group == layoutControlGroupEMail)
                {
                    layoutControlGroupPhone.Expanded = false;
                    layoutControlGroupLicense.Expanded = false;
                    layoutControlGroupAccounts.Expanded = false;

                    if ((treeListEMail.Nodes.Count > 0) && (treeListEMail.FocusedNode == null))
                    {
                        treeListEMail.FocusedNode = treeListEMail.Nodes[0];
                        LoadEmailNodeInfoInEditor(treeListEMail.FocusedNode);
                    }
                }
            }
        }
        #endregion

        #region ClearControl

        public void ClearOutControls()
        {
            frmAddress = null;
            frmContact = null;
        }

        public void OpenAccountList()
        {
            layoutControlGroupAccounts.Expanded = true;
        }
        #endregion

    }


    /// <summary>
    /// Класс – хранящий информацию, которая передается получателям уведомления о событии
    /// </summary>
    public partial class ChangeCompanyPropertieEventArgs : EventArgs
    {
        private readonly CCompany m_objCompany;
        public CCompany Company
        {
            get { return m_objCompany; }
        }

        private readonly enumActionSaveCancel m_enActionType;
        public enumActionSaveCancel ActionType
        {
            get { return m_enActionType; }
        }

        private readonly System.Boolean m_bIsNewCompany;
        public System.Boolean IsNewCompany
        {
            get { return m_bIsNewCompany; }
        }

        public ChangeCompanyPropertieEventArgs(CCompany objCompany, enumActionSaveCancel enActionType, System.Boolean bIsNewCompany)
        {
            m_objCompany = objCompany;
            m_enActionType = enActionType;
            m_bIsNewCompany = bIsNewCompany;
        }
    }
}