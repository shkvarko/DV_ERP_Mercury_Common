using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;

namespace ERP_Mercury.Common
{
    public partial class ctrlCustomer : UserControl
    {
        #region Свойства, переменные
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;

        private CCustomer m_objSelectedCustomer;

        private System.Boolean m_bIsChanged;

        public ctrlAddress frmAddress;
        public ctrlContact frmContact;
        public ctrlRtt frmRtt;

        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemAddress;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemContact;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemRtt;

        private System.Boolean m_bDisableEvents;
        private System.Boolean m_bNewCustomer;
        private System.Boolean m_bIsReadOnly;

        private const System.Int32 iMinControlItemHeight = 20;
        private const System.Int32 iPanel1WidthDef = 350;

        #endregion

        #region События
        // Создаем закрытое поле, ссылающееся на заголовок списка делегатов
        private EventHandler<ChangeCustomerPropertieEventArgs> m_ChangeCustomerProperties;
        // Создаем в классе член-событие
        public event EventHandler<ChangeCustomerPropertieEventArgs> ChangeCustomerProperties
        {
            add
            {
                // берем закрытую блокировку и добавляем обработчик
                // (передаваемый по значению) в список делегатов
                m_ChangeCustomerProperties += value;
            }
            remove
            {
                // берем закрытую блокировку и удаляем обработчик
                // (передаваемый по значению) из списка делегатов
                m_ChangeCustomerProperties -= value;
            }
        }
        /// <summary>
        /// Инициирует событие и уведомляет о нем зарегистрированные объекты
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnChangeCustomerProperties(ChangeCustomerPropertieEventArgs e)
        {
            // Сохраняем поле делегата во временном поле для обеспечение безопасности потока
            EventHandler<ChangeCustomerPropertieEventArgs> temp = m_ChangeCustomerProperties;
            // Если есть зарегистрированные объектв, уведомляем их
            if (temp != null) temp(this, e);
        }
        public void SimulateChangeCustomerProperties(CCustomer objCustomer, enumActionSaveCancel enActionType, System.Boolean bIsNewCustomer)
        {
            // Создаем объект, хранящий информацию, которую нужно передать
            // объектам, получающим уведомление о событии
            ChangeCustomerPropertieEventArgs e = new ChangeCustomerPropertieEventArgs(objCustomer, enActionType, bIsNewCustomer);

            // Вызываем виртуальный метод, уведомляющий наш объект о возникновении события
            // Если нет типа, переопределяющего этот метод, наш объект уведомит все объекты, 
            // подписавшиеся на уведомление о событии
            OnChangeCustomerProperties(e);
        }
        #endregion

        #region Конструктор
        public ctrlCustomer(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem)
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_bIsChanged = false;
            m_bDisableEvents = false;
            m_bNewCustomer = false;

            m_objSelectedCustomer = null;


            frmAddress = new ERP_Mercury.Common.ctrlAddress(ERP_Mercury.Common.EnumObject.Customer, m_objProfile, m_objMenuItem, System.Guid.Empty);
            //frmAddress.InitAddressControl();

            frmContact = new ERP_Mercury.Common.ctrlContact(ERP_Mercury.Common.EnumObject.Customer, m_objProfile, m_objMenuItem, System.Guid.Empty);
            frmRtt = new ctrlRtt(m_objProfile, m_objMenuItem);

            layoutControlAddress.Size = new Size(layoutControlAddress.Size.Width, (frmAddress.Size.Height));
            layoutControlAddress.MaximumSize = new Size(layoutControlAddress.Size.Width, layoutControlAddress.Size.Height);

            layoutControlContact.Size = new Size(layoutControlContact.Size.Width, (frmContact.Size.Height));
            layoutControlContact.MaximumSize = new Size(layoutControlContact.Size.Width, layoutControlContact.Size.Height);

            layoutControlRtt.Size = new Size(layoutControlRtt.Size.Width, (frmRtt.Size.Height));
            layoutControlRtt.MaximumSize = new Size(layoutControlRtt.Size.Width, layoutControlRtt.Size.Height);

            layoutControlGroupAddress.Size = new Size(layoutControlGroupAddress.Size.Width, (frmAddress.Size.Height));
            layoutControlGroupConact.Size = new Size(layoutControlGroupConact.Size.Width, (frmContact.Size.Height));
            layoutControlGroupRtt.Size = new Size(layoutControlGroupRtt.Size.Width, (frmRtt.Size.Height));

            this.layoutControlItemAddress = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemAddress.Parent = layoutControlGroupAddress;
            this.layoutControlItemAddress.Control = frmAddress;
            this.layoutControlItemAddress.CustomizationFormText = "layoutControlItemAddress";
            this.layoutControlItemAddress.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItemAddress.Name = "layoutControlItemAddress";
            this.layoutControlItemAddress.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItemAddress.Size = new System.Drawing.Size(642, 38);
            this.layoutControlItemAddress.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlItemAddress.Text = "layoutControlItemAddress";
            this.layoutControlItemAddress.TextSize = new System.Drawing.Size(93, 20);
            this.layoutControlItemAddress.TextVisible = false;
            this.frmAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));


            this.layoutControlItemContact = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemContact.Parent = layoutControlGroupConact;
            this.layoutControlItemContact.Control = frmContact;
            this.layoutControlItemContact.CustomizationFormText = "layoutControlItemContact";
            this.layoutControlItemContact.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItemContact.Name = "layoutControlItemContact";
            this.layoutControlItemContact.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItemContact.Size = new System.Drawing.Size(642, 38);
            this.layoutControlItemContact.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlItemContact.Text = "layoutControlItemContact";
            this.layoutControlItemContact.TextSize = new System.Drawing.Size(93, 20);
            this.layoutControlItemContact.TextVisible = false;
            this.frmContact.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));


            this.layoutControlItemRtt = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemRtt.Parent = layoutControlGroupRtt;
            this.layoutControlItemRtt.Control = frmRtt;
            this.layoutControlItemRtt.CustomizationFormText = "layoutControlItemRtt";
            this.layoutControlItemRtt.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItemRtt.Name = "layoutControlItemRtt";
            this.layoutControlItemRtt.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItemRtt.Size = new System.Drawing.Size(642, 38);
            this.layoutControlItemRtt.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlItemRtt.Text = "layoutControlItemRtt";
            this.layoutControlItemRtt.TextSize = new System.Drawing.Size(93, 20);
            this.layoutControlItemRtt.TextVisible = false;
            this.frmRtt.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));

            frmAddress.ChangeAddressPropertie += OnChangeAddressPropertie;

            frmContact.ChangeContactProperties += OnChangeContactPropertie;

            frmRtt.ChangeRttProperties += OnChangeRttPropertie;
            frmRtt.ChangeControlRttSize += OnChangeControlRttSize;

            /*
            layoutControlGroupAddress.Expanded = true;
            layoutControlGroupConact.Expanded = true;
            layoutControlGroupRtt.Expanded = true;
            layoutControlGroupAdvProperties.Expanded = true;
            */
            LoadComboBoxItems();
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
                //UniXP.Common.CClientRights objClientRights = m_objProfile.GetClientsRight();
                if (m_objProfile.GetClientsRight().GetState(ERP_Mercury.Global.Consts.strDR_EditCustomerCard) == false)
                {
                    btnEdit.Visible = false;
                    btnPrint.Visible = false;
                    btnSave.Visible = false;
                    //this.Controls.Remove(tableLayoutPanel4 );
                }

                //objClientRights = null;
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
        /// Обновление выпадающих списков
        /// </summary>
        /// <returns>true - все списки успешно обновлены; false - ошибка</returns>
        public System.Boolean LoadComboBoxItems()
        {
            System.Boolean bRet = false;
            try
            {

                // форма собственности
                cboxState.Properties.Items.Clear();
                List<CStateType> objStateTypeList = CStateType.GetStateTypeList( m_objProfile, null );
                if (objStateTypeList != null)
                {
                    cboxState.Properties.Items.AddRange(objStateTypeList);

                    //foreach (CStateType objStateType in objStateTypeList)
                    //{
                    //    cboxState.Properties.Items.Add(objStateType);
                    //}
                }
                //objStateTypeList = null;

                // признак активности
                cboxActiveType.Properties.Items.Clear();
                List<CCustomerActiveType> objCustomerActiveTypeList = CCustomerActiveType.GetCustomerActiveTypeList(m_objProfile, null);
                if (objCustomerActiveTypeList != null)
                {
                    cboxActiveType.Properties.Items.AddRange(objCustomerActiveTypeList);

                    //foreach (CCustomerActiveType objCustomerActiveType in objCustomerActiveTypeList)
                    //{
                    //    cboxActiveType.Properties.Items.Add( objCustomerActiveType );
                    //}
                }
                //objCustomerActiveTypeList = null;

                // типы телефонов
                repItemcboxPhoneType.Items.Clear();
                List<CPhoneType> objPhoneTypeList = CPhoneType.GetPhoneTypeList(m_objProfile, null);
                if (objPhoneTypeList != null)
                {
                    repItemcboxPhoneType.Items.AddRange( objPhoneTypeList );

                    //foreach (CPhoneType objPhoneType in objPhoneTypeList)
                    //{
                    //    repItemcboxPhoneType.Items.Add(objPhoneType);
                    //}
                }

                // типы лицензий
                repItemCBoxLicenceType.Items.Clear();
                List<CLicenceType> objLicenceTypeList = CLicenceType.GetLicenceTypeList(m_objProfile, null);
                if (objPhoneTypeList != null)
                {
                    repItemCBoxLicenceType.Items.AddRange(objLicenceTypeList);

                    //foreach (CLicenceType objLicenceType in objLicenceTypeList)
                    //{
                    //    repItemCBoxLicenceType.Items.Add(objLicenceType);
                    //}
                }

                // типы расчетных счетов
                repItemcboxAccountType.Items.Clear();
                List<CAccountType> objAccountTypeList = CAccountType.GetAccountTypeList(m_objProfile, null);
                if (objAccountTypeList != null)
                {
                    repItemcboxAccountType.Items.AddRange(objAccountTypeList);

                    //foreach (CAccountType objAccountType in objAccountTypeList)
                    //{
                    //    repItemcboxAccountType.Items.Add( objAccountType );
                    //}
                }

                // банки
                repItemcboxAccountBank.Items.Clear();
                List<CBank> objBankList = CBank.GetBankList(m_objProfile, null, null);
                if (objBankList != null)
                {
                    repItemcboxAccountBank.Items.AddRange(objBankList);

                    //foreach (CBank objBank in objBankList)
                    //{
                    //    repItemcboxAccountBank.Items.Add(objBank);
                    //}
                }

                // валюты
                repItemcboxAccountCurrency.Items.Clear();
                List<CCurrency> objCurrencyList = CCurrency.GetCurrencyList(m_objProfile, null);
                if (objCurrencyList != null)
                {
                    repItemcboxAccountCurrency.Items.AddRange(objCurrencyList);

                    //foreach (CCurrency objCurrency in objCurrencyList)
                    //{
                    //    repItemcboxAccountCurrency.Items.Add(objCurrency);
                    //}
                }

                // сетевой клиент
                cboxDistrNet.Properties.Items.Clear();
                List<CDistributionNetwork> objDistributionNetworkList = CDistributionNetwork.GetDistributionNetworkList( m_objProfile, null );
                if (objDistributionNetworkList != null)
                {
                    // добавим пустышку
                    cboxDistrNet.Properties.Items.Add(new CDistributionNetwork());
                    cboxDistrNet.Properties.Items.AddRange(objDistributionNetworkList);

                    //foreach (CDistributionNetwork objDistributionNetwork in objDistributionNetworkList)
                    //{
                    //    cboxDistrNet.Properties.Items.Add(objDistributionNetwork);
                    //}
                }
                //objDistributionNetworkList = null;

                //2011.06.03 if (frmAddress != null) { frmAddress.InitAllLists(); }
                if (frmContact != null) { frmContact.LoadComboBoxItems(); }
                if (frmRtt != null) { frmRtt.LoadComboBoxItems(); }

                bRet = true;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка обновления выпадающих списков.\n Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return bRet;
        }
        #endregion

        #region Индикация изменений
        private void SetPropertiesModified(System.Boolean bModified)
        {
            //ValidateProperties();
            //if (m_bIsChanged == bModified) { return; }
            m_bIsChanged = bModified;
            btnSave.Enabled = m_bIsChanged;
            btnCancel.Enabled = btnSave.Enabled;
            if (m_bIsChanged == true)
            {
                SimulateChangeCustomerProperties(m_objSelectedCustomer, enumActionSaveCancel.Unkown, m_bNewCustomer);
            }
        }
        private void cboxCustomerPropertie_SelectedValueChanged(object sender, EventArgs e)
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
        private void checkListBoxCustomerType_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                SetPropertiesModified(true);
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения списка типов. Текст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        private void txtCustomerPropertie_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                if (e.NewValue != null)
                {
                    SetPropertiesModified(true);
                    if ( (sender.GetType().Name == "TextEdit") && 
                        ( (( DevExpress.XtraEditors.TextEdit )sender).Properties.ReadOnly == false ) )
                    {
                        System.String strValue = (System.String)e.NewValue;
                        ((DevExpress.XtraEditors.TextEdit)sender).Properties.Appearance.BackColor = (strValue == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White;
                    }
                }
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения свойств клиента. Текст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
        private void OnChangeRttPropertie(Object sender, ERP_Mercury.Common.ChangeRttPropertieEventArgs e)
        {
            try
            {
                SetPropertiesModified(true);
                if (e.ActionType != enumActionSaveCancel.Unkown)
                {
                    layoutControlGroupRtt.Expanded = false;
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "OnChangeRttPropertie.\n Текст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally // очищаем занимаемые ресурсы
            {
            }

            return;
        }
        private void OnChangeControlRttSize(Object sender, ChangeControlRttSizeEventArgs e)
        {
            try
            {
                //в элементе управления "РТТ" произошло событие "нужно изменить размер"
                // попробуем из родительского контола установить новый размер контрола РТТ
                //layoutControlRtt.MaximumSize = e.ControlRttSize;
                //layoutControlRtt.Size = layoutControlRtt.MaximumSize;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "OnChangeControlRttSize.\n Текст ошибки: " + f.Message, "Внимание",
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
                if (m_objSelectedCustomer.PhoneList == null) { m_objSelectedCustomer.PhoneList = new List<CPhone>(); }
                System.Boolean bNotFullNode = false;
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objItem in treeListPhone.Nodes)
                {
                    if ((System.String)objItem.GetValue(colPhoneNumber) == "")
                    {
                        treeListPhone.FocusedNode = objItem;
                        bNotFullNode = true;
                        break;
                    }
                }
                if (bNotFullNode == true) { return; }

                CPhone objPhone = new CPhone();
                //m_objSelectedContact.PhoneList.Add(objPhone);
                DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListPhone.AppendNode(new object[] { objPhone.PhoneType, objPhone.PhoneNumber, objPhone.IsActive, objPhone.IsMain }, null);
                objNode.Tag = objPhone;
                treeListPhone.FocusedNode = objNode;
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
                if ((objNode == null) || (treeListPhone.Nodes.Count == 0)) { return; }

                if (m_objSelectedCustomer.PhoneForDeleteList == null) { m_objSelectedCustomer.PhoneForDeleteList = new List<CPhone>(); }
                DevExpress.XtraTreeList.Nodes.TreeListNode objPrevNode = objNode.PrevNode;
                m_objSelectedCustomer.PhoneForDeleteList.Add((CPhone)objNode.Tag);

                treeListPhone.Nodes.Remove(objNode);
                if (objPrevNode == null)
                {

                    if (treeListPhone.Nodes.Count > 0)
                    {
                        treeListPhone.FocusedNode = treeListPhone.Nodes[0];
                    }
                }
                else
                {
                    treeListPhone.FocusedNode = objPrevNode;
                }

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
        private void mitemAddPhone_Click(object sender, EventArgs e)
        {
            try
            {
                AddPhone();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("mitemAddPhone_Click.\n Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void mitemDeletePhone_Click(object sender, EventArgs e)
        {
            try
            {
                DeletePhone(treeListPhone.FocusedNode);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("mitemDeletePhone_Click.\n Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void treeListPhone_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (m_bIsReadOnly == true) { return; }
                if (e.Button == MouseButtons.Right)
                {
                    // попробуем определить, что же у нас под мышкой
                    DevExpress.XtraTreeList.TreeListHitInfo hi = ((DevExpress.XtraTreeList.TreeList)sender).CalcHitInfo(new Point(e.X, e.Y));
                    if ((hi == null) || (hi.Node == null))
                    {
                        mitemDeletePhone.Enabled = false;
                    }
                    else
                    {
                        // выделяем узел
                        mitemDeletePhone.Enabled = true;
                        hi.Node.TreeList.FocusedNode = hi.Node;
                    }
                    contextMenuStripPhone.Show(((DevExpress.XtraTreeList.TreeList)sender), new Point(e.X, e.Y));
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListPhone_MouseClick. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// выполняет проверку телефонного номера
        /// </summary>
        /// <param name="strPhone">телефонный номер</param>
        /// <returns>true - ошибок нет; false - телефонный номер не соответсвует установленным требованиям</returns>
        private System.Boolean IsPhoneValid(System.String strPhone)
        {
            System.Boolean bRet = false;
            try
            {
                // телефонный номер должен содержать "(", ")", "+", "[0-9]"
                if (strPhone.Trim() == "") { return bRet; }

                // по правильности здесь будет регулярное выражение
                bRet = true;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка проверки телефонного номера.\nНомер: " + strPhone + " Текст ошибки: " + f.Message);
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
        /// <returns>true - дублируется; false - не ублируется</returns>
        private System.Boolean IsPhoneDublicate(System.String strPhone, System.Int32 iPhonePos)
        {
            System.Boolean bRet = false;

            try
            {
                // проверим, возможно такой телефонный номер уже есть в списке
                System.Boolean bDublicate = false;
                for (System.Int32 i2 = 0; i2 < treeListPhone.Nodes.Count; i2++)
                {
                    if (i2 != iPhonePos)
                    {
                        if (((System.String)treeListPhone.Nodes[i2].GetValue(colPhoneNumber)) == strPhone)
                        {
                            bDublicate = true;
                            break;
                        }
                    }
                }

                bRet = bDublicate;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка проверки телефонного номера. Номер: " + strPhone + " Текст ошибки: " + f.Message);
            }

            return bRet;
        }
        private void treeListPhone_InvalidNodeException(object sender, DevExpress.XtraTreeList.InvalidNodeExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
        }
        private void treeListPhone_ValidateNode(object sender, DevExpress.XtraTreeList.ValidateNodeEventArgs e)
        {
            if (m_bDisableEvents == true) { return; }
            try
            {
                if ((e.Node[colPhoneNumber] == null) || (IsPhoneValid((System.String)e.Node[colPhoneNumber]) == false))
                {
                    e.Valid = false;
                    treeListPhone.SetColumnError(colPhoneNumber, "Телефонный номер не соответсвует принятым требованиям.");
                }
                else
                {
                    if (IsPhoneDublicate((System.String)e.Node[colPhoneNumber], treeListPhone.GetNodeIndex(e.Node)) == true)
                    {
                        e.Valid = false;
                        treeListPhone.SetColumnError(colPhoneNumber, "Такой телефонный номер уже есть в списке.");
                    }
                    else
                    {
                        SetPropertiesModified(true);
                    }
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListPhone_ValidateNode. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void treeListPhone_CellValueChanged(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
        }

        #endregion

        #region Электронные адреса
        /// <summary>
        /// Добавляет в список электронных адресов новую строку
        /// </summary>
        private void AddEMail()
        {
            try
            {
                if (treeListEMail.Enabled == false) { treeListEMail.Enabled = true; }
                if (m_objSelectedCustomer.EMailList == null) { m_objSelectedCustomer.EMailList = new List<CEMail>(); }
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

                CEMail objEMail = new CEMail();
                //m_objSelectedCustomer.EMailList.Add(objEMail);
                DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListEMail.AppendNode(new object[] { objEMail.EMail, objEMail.IsActive, objEMail.IsMain }, null);
                objNode.Tag = objEMail;
                treeListEMail.FocusedNode = objNode;
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
        /// <summary>
        /// Удаляет электронный адрес из списка
        /// </summary>
        /// <param name="objNode">узел с электронным адресом</param>
        private void DeleteEMail(DevExpress.XtraTreeList.Nodes.TreeListNode objNode)
        {
            try
            {
                if ((objNode == null) || (treeListEMail.Nodes.Count == 0)) { return; }

                if (m_objSelectedCustomer.EMailForDeleteList == null) { m_objSelectedCustomer.EMailForDeleteList = new List<CEMail>(); }
                DevExpress.XtraTreeList.Nodes.TreeListNode objPrevNode = objNode.PrevNode;
                m_objSelectedCustomer.EMailForDeleteList.Add((CEMail)objNode.Tag);

                treeListEMail.Nodes.Remove(objNode);
                if (objPrevNode == null)
                {

                    if (treeListEMail.Nodes.Count > 0)
                    {
                        treeListEMail.FocusedNode = treeListEMail.Nodes[0];
                    }
                }
                else
                {
                    treeListEMail.FocusedNode = objPrevNode;
                }

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
        private System.Boolean IsEMailValid(System.String strEMail)
        {
            System.Boolean bRet = false;
            try
            {
                // электронный адрес должен содержать "@" и "."
                if (strEMail.Trim() == "") { return bRet; }
                if (strEMail.IndexOf("@") < 0) { return bRet; }
                if (strEMail.IndexOf(".") < 0) { return bRet; }
                bRet = true;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка проверки электронного адреса.\nАдрес: " + strEMail + "\nТекст ошибки: " + f.Message);
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
        private System.Boolean IsEMailDublicate(System.String strEMail, System.Int32 iEMailPos)
        {
            System.Boolean bRet = false;

            try
            {
                // проверим, возможно такой адрес уже есть в списке
                System.Boolean bDublicate = false;
                for (System.Int32 i2 = 0; i2 < treeListEMail.Nodes.Count; i2++)
                {
                    if (i2 != iEMailPos)
                    {
                        if (((System.String)treeListEMail.Nodes[i2].GetValue(colEMailAddress)) == strEMail)
                        {
                            bDublicate = true;
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
        private void mitemAddEMail_Click(object sender, EventArgs e)
        {
            try
            {
                AddEMail();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("mitemAddEMail_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void treeListEMail_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if ((e.KeyCode == Keys.Down) && (treeListEMail.FocusedNode != null))
                {
                    AddEMail();
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListEMail_KeyDown. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void mitemDeleteEMail_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteEMail(treeListEMail.FocusedNode);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("mitemDeleteEMail_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private void treeListEMail_ValidateNode(object sender, DevExpress.XtraTreeList.ValidateNodeEventArgs e)
        {
            if (m_bDisableEvents == true) { return; }
            try
            {
                if ((e.Node[colEMailAddress] == null) || (IsEMailValid((System.String)e.Node[colEMailAddress]) == false))
                {
                    e.Valid = false;
                    treeListEMail.SetColumnError(colEMailAddress, "Адрес не соответсвует принятым требованиям.");
                }
                else
                {
                    if (IsEMailDublicate((System.String)e.Node[colEMailAddress], treeListEMail.GetNodeIndex(e.Node)) == true)
                    {
                        e.Valid = false;
                        treeListEMail.SetColumnError(colEMailAddress, "Такой адрес уже есть в списке.");
                    }
                    else
                    {
                        SetPropertiesModified(true);
                    }
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListEMail_ValidateNode.\nТекст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void treeListEMail_CellValueChanged(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
            if (m_bDisableEvents == true) { return; }
            try
            {
                System.Int32 iPosNode = treeListEMail.GetNodeIndex(e.Node);

                if ((e.Column == colEMailAddress) && (e.Value != null))
                {
                    if (IsEMailValid((System.String)e.Value) == false)
                    {
                        treeListEMail.SetColumnError(colEMailAddress, "Адрес не соответсвует принятым требованиям.");
                    }
                    else
                    {
                        // проверим, возможно такой адрес уже есть в списке
                        if (IsEMailDublicate((System.String)e.Value, iPosNode) == true)
                        {
                            treeListEMail.SetColumnError(colEMailAddress, "Такой адрес уже есть в списке.");
                        }
                        else
                        {
                            ((CEMail)e.Node.Tag).EMail = (System.String)e.Value;
                        }
                    }
                }
                if ((e.Column == colIsMainEMail) && (e.Value != null) && (((System.Boolean)e.Value) == true))
                {
                    // главным може быть только один адрес,
                    // поэтому все остальные нужно сделать неглавными
                    for (System.Int32 i = 0; i < treeListEMail.Nodes.Count; i++)
                    {
                        if (i != iPosNode)
                        {
                            treeListEMail.Nodes[i].SetValue(colIsMainEMail, false);
                            ((CEMail)treeListEMail.Nodes[i].Tag).IsMain = false;
                        }
                    }
                    ((CEMail)e.Node.Tag).IsMain = true;
                    e.Node.SetValue(colIsActiveEMail, true);
                }
                if ((e.Column == colIsActiveEMail) && (e.Value != null))
                {
                    ((CEMail)e.Node.Tag).IsActive = (System.Boolean)e.Value;
                }

                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListEMail_CellValueChanged.\nТекст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private void treeListEMail_InvalidNodeException(object sender, DevExpress.XtraTreeList.InvalidNodeExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
        }
        private void treeListEMail_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (m_bIsReadOnly == true) { return; }
                if (e.Button == MouseButtons.Right)
                {
                    // попробуем определить, что же у нас под мышкой
                    DevExpress.XtraTreeList.TreeListHitInfo hi = ((DevExpress.XtraTreeList.TreeList)sender).CalcHitInfo(new Point(e.X, e.Y));
                    if ((hi == null) || (hi.Node == null))
                    {
                        mitemDeleteEMail.Enabled = false;
                    }
                    else
                    {
                        // выделяем узел
                        mitemDeleteEMail.Enabled = true;
                        hi.Node.TreeList.FocusedNode = hi.Node;
                    }
                    contextMenuStripEMail.Show(((DevExpress.XtraTreeList.TreeList)sender), new Point(e.X, e.Y));
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListEMail_MouseClick.\nТекст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        #endregion

        #region Лицензии
        /// <summary>
        /// Добавляет в список лицензию
        /// </summary>
        private void AddLicence()
        {
            try
            {
                if (treeListLicence.Enabled == false) { treeListLicence.Enabled = true; }
                if (m_objSelectedCustomer.LicenceList == null) { m_objSelectedCustomer.LicenceList = new List<CLicence>(); }
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

                CLicence objLicence = new CLicence();
                //m_objSelectedContact.PhoneList.Add(objPhone);
                DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListLicence.AppendNode(new object[] { objLicence.LicenceType, objLicence.LicenceNum, 
                    objLicence.BeginDate, objLicence.EndDate, objLicence.WhoGive }, null);
                objNode.Tag = objLicence;
                treeListLicence.FocusedNode = objNode;
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
        /// <summary>
        /// Удаляет лицензию из списка
        /// </summary>
        /// <param name="objNode">удаляемый узел в дереве</param>
        private void DeleteLicence(DevExpress.XtraTreeList.Nodes.TreeListNode objNode)
        {
            try
            {
                if ((objNode == null) || (treeListLicence.Nodes.Count == 0)) { return; }

                if (m_objSelectedCustomer.LicenceForDeleteList == null) { m_objSelectedCustomer.LicenceForDeleteList = new List<CLicence>(); }
                DevExpress.XtraTreeList.Nodes.TreeListNode objPrevNode = objNode.PrevNode;
                m_objSelectedCustomer.LicenceForDeleteList.Add((CLicence)objNode.Tag);

                treeListLicence.Nodes.Remove(objNode);
                if (objPrevNode == null)
                {

                    if (treeListLicence.Nodes.Count > 0)
                    {
                        treeListLicence.FocusedNode = treeListLicence.Nodes[0];
                    }
                }
                else
                {
                    treeListLicence.FocusedNode = objPrevNode;
                }
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
        private void mitemAddLicence_Click(object sender, EventArgs e)
        {
            try
            {
                AddLicence();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("mitemAddLicence_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void mitemDeleteLicence_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteLicence(treeListLicence.FocusedNode);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("mitemDeleteLicence_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void treeListLicence_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (m_bIsReadOnly == true) { return; }
                if (e.Button == MouseButtons.Right)
                {
                    // попробуем определить, что же у нас под мышкой
                    DevExpress.XtraTreeList.TreeListHitInfo hi = ((DevExpress.XtraTreeList.TreeList)sender).CalcHitInfo(new Point(e.X, e.Y));
                    if ((hi == null) || (hi.Node == null))
                    {
                        mitemDeleteLicence.Enabled = false;
                    }
                    else
                    {
                        // выделяем узел
                        mitemDeleteLicence.Enabled = true;
                        hi.Node.TreeList.FocusedNode = hi.Node;
                    }
                    contextMenuStripLicence.Show(((DevExpress.XtraTreeList.TreeList)sender), new Point(e.X, e.Y));
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListLicence_MouseClick. Текст ошибки: " + f.Message);
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
        private System.Boolean IsLicenceValid(System.String strLicence)
        {
            System.Boolean bRet = false;
            try
            {
                // номер лицензии должен содержать "[0-9]"
                if (strLicence.Trim() == "") { return bRet; }

                // по правильности здесь будет регулярное выражение
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
        private System.Boolean IsLicenceDublicate(System.String strLicence, System.Int32 iLicencePos)
        {
            System.Boolean bRet = false;

            try
            {
                // проверим, возможно такой номер лицензии уже есть в списке
                System.Boolean bDublicate = false;
                for (System.Int32 i2 = 0; i2 < treeListLicence.Nodes.Count; i2++)
                {
                    if (i2 != iLicencePos)
                    {
                        if (((System.String)treeListLicence.Nodes[i2].GetValue(colLicenceNum)) == strLicence)
                        {
                            bDublicate = true;
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
        private void treeListLicence_InvalidNodeException(object sender, DevExpress.XtraTreeList.InvalidNodeExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
        }
        private void treeListLicence_ValidateNode(object sender, DevExpress.XtraTreeList.ValidateNodeEventArgs e)
        {
            if (m_bDisableEvents == true) { return; }
            try
            {
                if ((e.Node[colLicenceNum] == null) || (IsLicenceValid((System.String)e.Node[colLicenceNum]) == false))
                {
                    e.Valid = false;
                    treeListLicence.SetColumnError(colLicenceNum, "Номер лицензии не соответсвует принятым требованиям.");
                }
                else
                {
                    if (IsLicenceDublicate((System.String)e.Node[colLicenceNum], treeListLicence.GetNodeIndex(e.Node)) == true)
                    {
                        e.Valid = false;
                        treeListLicence.SetColumnError(colLicenceNum, "Такой номер лицензии уже есть в списке.");
                    }
                    else
                    {
                        SetPropertiesModified(true);
                    }
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog( "treeListLicence_ValidateNode. Текст ошибки: " + f.Message );
            }
            finally
            {
            }
            return;
        }
        private void treeListLicence_CellValueChanged(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
            if (m_bDisableEvents == true) { return; }
            try
            {
                System.Int32 iPosNode = treeListLicence.GetNodeIndex(e.Node);

                if ((e.Column == colLicenceNum) && (e.Value != null))
                {
                    if ( IsLicenceValid((System.String)e.Value) == false)
                    {
                        treeListPhone.SetColumnError(colPhoneNumber, "Номер лицензии не соответсвует принятым требованиям.");
                    }
                    else
                    {
                        // проверим, возможно такой номер лицензии уже есть в списке
                        if ( IsLicenceDublicate((System.String)e.Value, iPosNode) == true)
                        {
                            treeListPhone.SetColumnError(colPhoneNumber, "Такой номер лицензии уже есть в списке.");
                        }
                        else
                        {
                            //m_objSelectedContact.PhoneList[iPosNode].PhoneNumber = (System.String)e.Value;
                            ((CLicence)e.Node.Tag).LicenceNum = (System.String)e.Value;
                        }
                    }
                }
                if ((e.Column == colWho) && (e.Value != null)  )
                {
                    if( ((System.String)e.Value).Trim().Length > 0 )
                    {
                        ((CLicence)e.Node.Tag).WhoGive = ((System.String)e.Value).Trim();
                    }
                }
                if ((e.Column == colLicenceType) && (e.Value != null))
                {
                    ((CLicence)e.Node.Tag).LicenceType = (CLicenceType)e.Value;
                }
                if ((e.Column == colLicenceBeginDate) && (e.Value != null))
                {
                    ((CLicence)e.Node.Tag).BeginDate = (System.DateTime)e.Value;
                }
                if ((e.Column == colLicenceEndDate) && (e.Value != null))
                {
                    ((CLicence)e.Node.Tag).EndDate = (System.DateTime)e.Value;
                }

                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListLicence_CellValueChanged. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        #endregion

        #region РАсчетные счета
        /// <summary>
        /// Добавляет в список расчетный счет
        /// </summary>
        private void AddAccount()
        {
            try
            {
                if (treeListAccounts.Enabled == false) { treeListAccounts.Enabled = true; }
                if (m_objSelectedCustomer.AccountList == null) { m_objSelectedCustomer.AccountList = new List<CAccount>(); }
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

                CAccount objAccount = new CAccount();

                DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListAccounts.AppendNode(new object[] { objAccount.AccountType, objAccount.AccountNumber, objAccount.Currency, objAccount.Bank, objAccount.Description }, null);
                objNode.Tag = objAccount;
                treeListAccounts.FocusedNode = objNode;
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
        /// <summary>
        /// Удаляет расчетный счет из списка
        /// </summary>
        /// <param name="objNode">удаляемый узел в дереве</param>
        private void DeleteAccount(DevExpress.XtraTreeList.Nodes.TreeListNode objNode)
        {
            try
            {
                if ((objNode == null) || ( treeListAccounts.Nodes.Count == 0)) { return; }

                if (m_objSelectedCustomer.AccountForDeleteList == null) { m_objSelectedCustomer.AccountForDeleteList = new List<CAccount>(); }
                DevExpress.XtraTreeList.Nodes.TreeListNode objPrevNode = objNode.PrevNode;
                m_objSelectedCustomer.AccountForDeleteList.Add((CAccount)objNode.Tag);

                treeListAccounts.Nodes.Remove(objNode);
                if (objPrevNode == null)
                {

                    if (treeListAccounts.Nodes.Count > 0)
                    {
                        treeListAccounts.FocusedNode = treeListAccounts.Nodes[0];
                    }
                }
                else
                {
                    treeListAccounts.FocusedNode = objPrevNode;
                }

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
        private void mitemAddAccount_Click(object sender, EventArgs e)
        {
            try
            {
                AddAccount();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("mitemAddAccount_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void mitemDeleteAccount_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteAccount( treeListAccounts.FocusedNode);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("mitemDeleteAccount_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void treeListAccounts_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (m_bIsReadOnly == true) { return; }
                if (e.Button == MouseButtons.Right)
                {
                    // попробуем определить, что же у нас под мышкой
                    DevExpress.XtraTreeList.TreeListHitInfo hi = ((DevExpress.XtraTreeList.TreeList)sender).CalcHitInfo(new Point(e.X, e.Y));
                    if ((hi == null) || (hi.Node == null))
                    {
                        mitemDeleteAccount.Enabled = false;
                    }
                    else
                    {
                        // выделяем узел
                        mitemDeleteAccount.Enabled = true;
                        hi.Node.TreeList.FocusedNode = hi.Node;
                    }
                    contextMenuStripAccount.Show(((DevExpress.XtraTreeList.TreeList)sender), new Point(e.X, e.Y));
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListAccounts_MouseClick. Текст ошибки: " + f.Message);
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
        private System.Boolean IsAccountValid(System.String strAccount)
        {
            System.Boolean bRet = false;
            try
            {
                if( strAccount.Trim() == "" ) { return bRet; }

                // по правильности здесь будет регулярное выражение
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
        private System.Boolean IsAccountDublicate(System.String strAccount, System.Int32 iAccountPos)
        {
            System.Boolean bRet = false;

            try
            {
                // проверим, возможно такой номер лицензии уже есть в списке
                System.Boolean bDublicate = false;
                for (System.Int32 i2 = 0; i2 < treeListAccounts.Nodes.Count; i2++)
                {
                    if (i2 != iAccountPos)
                    {
                        if (((System.String)treeListAccounts.Nodes[i2].GetValue(colAccountNumber)) == strAccount)
                        {
                            bDublicate = true;
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
        private void treeListAccounts_InvalidNodeException(object sender, DevExpress.XtraTreeList.InvalidNodeExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
        }
        private void treeListAccounts_ValidateNode(object sender, DevExpress.XtraTreeList.ValidateNodeEventArgs e)
        {
            if (m_bDisableEvents == true) { return; }
            try
            {
                if ( e.Node[colAccountType] == null )
                {
                    e.Valid = false;
                    treeListAccounts.SetColumnError(colLicenceNum, "Необходимо указать тип расчетного счета.");
                }
                if ((e.Node[colAccountNumber] == null) || (IsAccountValid((System.String)e.Node[colAccountNumber]) == false))
                {
                    e.Valid = false;
                    treeListAccounts.SetColumnError(colLicenceNum, "Номер расчетного счета не соответсвует принятым требованиям.");
                }
                else
                {
                    if (IsAccountDublicate((System.String)e.Node[colAccountNumber], treeListAccounts.GetNodeIndex(e.Node)) == true)
                    {
                        e.Valid = false;
                        treeListAccounts.SetColumnError(colAccountNumber, "Такой номер расчетного счета уже есть в списке.");
                    }
                    else
                    {
                        SetPropertiesModified(true);
                    }
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog( "treeListAccounts_ValidateNode. Текст ошибки: " + f.Message );
            }
            finally
            {
            }
            return;
        }
        private void treeListAccounts_CellValueChanged(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
            if (m_bDisableEvents == true) { return; }
            try
            {
                System.Int32 iPosNode = treeListAccounts.GetNodeIndex(e.Node);

                if ((e.Column == colAccountNumber) && (e.Value != null))
                {
                    if (IsAccountValid((System.String)e.Value) == false)
                    {
                        treeListAccounts.SetColumnError(colAccountNumber, "Номер расчетного счета не соответсвует принятым требованиям.");
                    }
                    else
                    {
                        // проверим, возможно такой номер лицензии уже есть в списке
                        if (IsAccountDublicate((System.String)e.Value, iPosNode) == true)
                        {
                            treeListAccounts.SetColumnError(colAccountNumber, "Такой номер расчетного счета уже есть в списке.");
                        }
                        else
                        {
                            ((CAccount)e.Node.Tag).AccountNumber = (System.String)e.Value;
                        }
                    }
                }
                if ((e.Column == colAccountDscrpn) && (e.Value != null))
                {
                    if (((System.String)e.Value).Trim().Length > 0)
                    {
                        ((CAccount)e.Node.Tag).Description = ((System.String)e.Value).Trim();
                    }
                }
                if ((e.Column == colAccountCurrency) && (e.Value != null))
                {
                    ((CAccount)e.Node.Tag).Currency = (CCurrency)e.Value;
                }
                if ((e.Column == colAccountType) && (e.Value != null))
                {
                    ((CAccount)e.Node.Tag).AccountType = (CAccountType)e.Value;
                }
                if ((e.Column == colAccountBank) && (e.Value != null))
                {
                    ((CAccount)e.Node.Tag).Bank = (CBank)e.Value;
                }

                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListAccounts_CellValueChanged. Текст ошибки: " + f.Message);
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
                    tableLayoutPanel4.RowStyles[0].Height = iRowWarnigHeith;
                }
                else
                {
                    tableLayoutPanel4.RowStyles[0].Height = 0;
                }
                tableLayoutPanel4.Size = new Size(tableLayoutPanel4.Size.Width,
                    (System.Convert.ToInt32(tableLayoutPanel4.RowStyles[0].Height + iRowBtnHeith)));
                tableLayoutPanel4.Refresh();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "ShowWarningPnl.\n Текст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Редактировать клиента
        /// <summary>
        /// Загружает свойства клиента для редактирования !!!!!!!!!!
        /// </summary>
        /// <param name="objCustomer">клиент</param>
        public void EditCustomer(ERP_Mercury.Common.CCustomer objCustomer)
        {
            if (objCustomer == null) { return; }
            m_bDisableEvents = true;
            m_bNewCustomer = false;
            ShowWarningPnl(false);
            try
            {
                System.String strErr = "";

                m_objSelectedCustomer = objCustomer;
                m_objSelectedCustomer.PhoneList = CPhone.GetPhoneListForCustomer(m_objProfile, null, m_objSelectedCustomer.ID, ref strErr);
                m_objSelectedCustomer.LicenceList = CLicence.GetLicenceList(m_objSelectedCustomer.ID, m_objProfile, null);
                m_objSelectedCustomer.AccountList = CAccount.GetAccountListForCustomer(m_objProfile, null, m_objSelectedCustomer.ID, ref strErr);
                m_objSelectedCustomer.EMailList = CEMail.GetEMailListForContact(m_objProfile, null, EnumObject.Customer, m_objSelectedCustomer.ID, ref strErr);
                m_objSelectedCustomer.CustomerTypeList = CCustomerType.GetCustomerTypeListForCustomer(m_objProfile, null, m_objSelectedCustomer.ID);
                m_objSelectedCustomer.TargetBuyList = CTargetBuy.GetTargetBuyForCustomer(m_objProfile, null, m_objSelectedCustomer.ID);
                m_objSelectedCustomer.RttList = CRtt.GetRttList(m_objProfile, null, m_objSelectedCustomer.ID);
                m_objSelectedCustomer.DistributionNetwork = CDistributionNetwork.GetDistributionNetworkForCustomer(m_objProfile, null, m_objSelectedCustomer.ID, ref strErr);

                if (m_objSelectedCustomer.ContactForDeleteList == null) { m_objSelectedCustomer.ContactForDeleteList = new List<CContact>(); }
                else { m_objSelectedCustomer.ContactForDeleteList.Clear(); }
                if (m_objSelectedCustomer.AddressForDeleteList == null) { m_objSelectedCustomer.AddressForDeleteList = new List<CAddress>(); }
                else { m_objSelectedCustomer.AddressForDeleteList.Clear(); }
                if (m_objSelectedCustomer.RttForDeleteList == null) { m_objSelectedCustomer.RttForDeleteList = new List<CRtt>(); }
                else { m_objSelectedCustomer.RttForDeleteList.Clear(); }
                if (m_objSelectedCustomer.PhoneForDeleteList == null) { m_objSelectedCustomer.PhoneForDeleteList = new List<CPhone>(); }
                else { m_objSelectedCustomer.PhoneForDeleteList.Clear(); }
                if (m_objSelectedCustomer.LicenceForDeleteList == null) { m_objSelectedCustomer.LicenceForDeleteList = new List<CLicence>(); }
                else { m_objSelectedCustomer.LicenceForDeleteList.Clear(); }
                if (m_objSelectedCustomer.AccountForDeleteList == null) { m_objSelectedCustomer.AccountForDeleteList = new List<CAccount>(); }
                else { m_objSelectedCustomer.AccountForDeleteList.Clear(); }
                if (m_objSelectedCustomer.EMailForDeleteList == null) { m_objSelectedCustomer.EMailForDeleteList = new List<CEMail>(); }
                else { m_objSelectedCustomer.EMailForDeleteList.Clear(); }

                this.SuspendLayout();

                txtFullName.Text = "";
                txtShortName.Text = "";
                txtCode.Text = "";
                txtUNP.Text = "";
                txtOKPO.Text = "";
                txtOKULP.Text = "";
                cboxActiveType.SelectedItem = null;
                cboxState.SelectedItem = null;
                cboxDistrNet.SelectedItem = null;
                treeListLicence.Nodes.Clear();
                treeListPhone.Nodes.Clear();
                treeListAccounts.Nodes.Clear();
                treeListEMail.Nodes.Clear();
                //treeListRtt.Nodes.Clear();

                checkListBoxTargetBuy.Items.Clear();

                txtFullName.Text = m_objSelectedCustomer.FullName;
                txtShortName.Text = m_objSelectedCustomer.ShortName;
                txtCode.Text = m_objSelectedCustomer.Code;
                txtUNP.Text = m_objSelectedCustomer.UNP;
                txtOKPO.Text = m_objSelectedCustomer.OKPO;
                txtOKULP.Text = m_objSelectedCustomer.OKULP;
                if ((m_objSelectedCustomer.ActiveType != null) && (cboxActiveType.Properties.Items.Count > 0))
                {
                    foreach (Object objActiveType in cboxActiveType.Properties.Items)
                    {
                        if (((CCustomerActiveType)objActiveType).ID.CompareTo(m_objSelectedCustomer.ActiveType.ID) == 0)
                        {
                            cboxActiveType.SelectedItem = objActiveType;
                            break;
                        }
                    }
                }
                if ((m_objSelectedCustomer.StateType != null) && ( cboxState.Properties.Items.Count > 0))
                {
                    foreach (Object objStateType in cboxState.Properties.Items)
                    {
                        if (((CStateType)objStateType).ID.CompareTo(m_objSelectedCustomer.StateType.ID) == 0)
                        {
                            cboxState.SelectedItem = objStateType;
                            break;
                        }
                    }
                }
                if ((m_objSelectedCustomer.DistributionNetwork != null) && ( cboxDistrNet.Properties.Items.Count > 0))
                {
                    foreach (Object objDistrNet in cboxDistrNet.Properties.Items)
                    {
                        if (((CDistributionNetwork)objDistrNet).ID.CompareTo(m_objSelectedCustomer.DistributionNetwork.ID) == 0)
                        {
                            cboxDistrNet.SelectedItem = objDistrNet;
                            break;
                        }
                    }
                }
                // Лицензии
                if (m_objSelectedCustomer.LicenceList != null)
                {
                    foreach (CLicence objLicence in m_objSelectedCustomer.LicenceList)
                    {
                        DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListLicence.AppendNode(new object[] { objLicence.LicenceType, objLicence.LicenceNum, objLicence.BeginDate, objLicence.EndDate, objLicence.WhoGive }, null);
                        objNode.Tag = objLicence;

                    }
                }
                // Телефоны
                if (m_objSelectedCustomer.PhoneList != null)
                {
                    foreach (CPhone objPhone in m_objSelectedCustomer.PhoneList)
                    {
                        DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListPhone.AppendNode(new object[] { objPhone.PhoneType, objPhone.PhoneNumber, objPhone.IsActive, objPhone.IsMain }, null);
                        objNode.Tag = objPhone;
                    }
                }
                // Электронные адреса
                if (m_objSelectedCustomer.EMailList != null)
                {
                    foreach (CEMail objEMail in m_objSelectedCustomer.EMailList)
                    {
                        DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListEMail.AppendNode(new object[] { objEMail.EMail, objEMail.IsActive, objEMail.IsMain }, null);
                        objNode.Tag = objEMail;
                    }
                }
                // Расчетные счета
                if (m_objSelectedCustomer.AccountList != null)
                {
                    foreach (CAccount objAccount in m_objSelectedCustomer.AccountList)
                    {
                        DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListAccounts.AppendNode(new object[] { objAccount.AccountType, objAccount.AccountNumber, objAccount.Currency, objAccount.Bank, objAccount.Description }, null);
                        objNode.Tag = objAccount;
                    }
                }
                // цели приобретения
                List<CTargetBuy> objFullTargetBuyList = CTargetBuy.GetTargetBuyList( m_objProfile, null );
                if ((objFullTargetBuyList != null) && (objFullTargetBuyList.Count > 0))
                {
                    foreach (CTargetBuy objTargetBuy1 in objFullTargetBuyList)
                    {
                        checkListBoxTargetBuy.Items.Add(objTargetBuy1, false);
                    }
                    if (m_objSelectedCustomer.TargetBuyList != null)
                    {
                        foreach (CTargetBuy objTargetBuy in m_objSelectedCustomer.TargetBuyList)
                        {
                            for (System.Int32 i = 0; i < checkListBoxTargetBuy.Items.Count; i++)
                            {
                                if (((CTargetBuy)checkListBoxTargetBuy.Items[i].Value).ID.CompareTo(objTargetBuy.ID) == 0)
                                {
                                    checkListBoxTargetBuy.Items[i].CheckState = CheckState.Checked;
                                    break;
                                }
                            }
                        }
                    }
                }
                objFullTargetBuyList = null;

                // Адреса
                frmAddress.LoadAddressList(m_objSelectedCustomer.ID, null);
                // Контакты
                frmContact.LoadContactList(m_objSelectedCustomer.ID, m_objSelectedCustomer.ContactList);
                // РТТ
                frmRtt.LoadRttList(m_objSelectedCustomer);

                frmRtt.ClearAllPropertiesControls();
                if ((frmRtt.RttList != null) && (frmRtt.RttList.Count > 0))
                {
                    frmRtt.ShowRtt(frmRtt.RttList[0], m_objSelectedCustomer);
                }
                frmAddress.ClearAllPropertiesControls();
                frmContact.ClearAllPropertiesControls();

                SetPropertiesModified(false);
                //tabControl.SelectedTabPage = tabGeneral;
                btnCancel.Enabled = true;
                btnCancel.Focus();

                SetModeReadOnly(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog( "Ошибка редактирования клиента. Текст ошибки: " + f.Message );
            }
            finally
            {
                this.ResumeLayout(false);
                //tabControl.SelectedTabPage = tabGeneral;
                m_bDisableEvents = false;
            }
            return;
        }
        #endregion

        #region Новый клиент
        /// <summary>
        /// Новый клиент
        /// </summary>
        public void NewCustomer()
        {
            m_bDisableEvents = true;
            m_bNewCustomer = true;
            ShowWarningPnl(false);
            try
            {
                this.Refresh();
                frmAddress.StartThreadWithLoadData();
                frmContact.frmAddress.StartThreadWithLoadData();
                frmRtt.frmAddress.StartThreadWithLoadData();

                m_objSelectedCustomer = new ERP_Mercury.Common.CCustomer();
                if (m_objSelectedCustomer.ContactForDeleteList == null) { m_objSelectedCustomer.ContactForDeleteList = new List<CContact>(); }
                else { m_objSelectedCustomer.ContactForDeleteList.Clear(); }
                if (m_objSelectedCustomer.AddressForDeleteList == null) { m_objSelectedCustomer.AddressForDeleteList = new List<CAddress>(); }
                else { m_objSelectedCustomer.AddressForDeleteList.Clear(); }
                if (m_objSelectedCustomer.RttForDeleteList == null) { m_objSelectedCustomer.RttForDeleteList = new List<CRtt>(); }
                else { m_objSelectedCustomer.RttForDeleteList.Clear(); }
                if (m_objSelectedCustomer.PhoneForDeleteList == null) { m_objSelectedCustomer.PhoneForDeleteList = new List<CPhone>(); }
                else { m_objSelectedCustomer.PhoneForDeleteList.Clear(); }
                if (m_objSelectedCustomer.LicenceForDeleteList == null) { m_objSelectedCustomer.LicenceForDeleteList = new List<CLicence>(); }
                else { m_objSelectedCustomer.LicenceForDeleteList.Clear(); }
                if (m_objSelectedCustomer.AccountForDeleteList == null) { m_objSelectedCustomer.AccountForDeleteList = new List<CAccount>(); }
                else { m_objSelectedCustomer.AccountForDeleteList.Clear(); }
                if (m_objSelectedCustomer.EMailForDeleteList == null) { m_objSelectedCustomer.EMailForDeleteList = new List<CEMail>(); }
                else { m_objSelectedCustomer.EMailForDeleteList.Clear(); }

                this.SuspendLayout();

                txtFullName.Text = "";
                txtShortName.Text = "";
                txtCode.Text = "";
                txtUNP.Text = "";
                txtOKPO.Text = "";
                txtOKULP.Text = "";
                cboxActiveType.SelectedItem = null;
                cboxState.SelectedItem = null;
                cboxDistrNet.SelectedItem = null;
                treeListLicence.Nodes.Clear();
                treeListPhone.Nodes.Clear();
                treeListAccounts.Nodes.Clear();
                treeListEMail.Nodes.Clear();
                //treeListRtt.Nodes.Clear();

                checkListBoxTargetBuy.Items.Clear();

                txtFullName.Text = m_objSelectedCustomer.FullName;
                txtShortName.Text = m_objSelectedCustomer.ShortName;
                txtCode.Text = m_objSelectedCustomer.Code;
                txtUNP.Text = m_objSelectedCustomer.UNP;
                txtOKPO.Text = m_objSelectedCustomer.OKPO;
                txtOKULP.Text = m_objSelectedCustomer.OKULP;

                // Адреса
                frmAddress.LoadAddressList(m_objSelectedCustomer.ID, null);
                // Контакты
                frmContact.LoadContactList(m_objSelectedCustomer.ID, m_objSelectedCustomer.ContactList);
                // РТТ
                frmRtt.LoadRttList(m_objSelectedCustomer);
                //// типы клиентов
                //List<CCustomerType> objFullCustomerTypeList = CCustomerType.GetCustomerTypeList(m_objProfile, null);
                //if ((objFullCustomerTypeList != null) && (objFullCustomerTypeList.Count > 0))
                //{
                //    foreach (CCustomerType objCustomerType1 in objFullCustomerTypeList)
                //    {
                //        checkListBoxTargetBuy.Items.Add(objCustomerType1, false);
                //    }
                //    if (m_objSelectedCustomer.CustomerTypeList != null)
                //    {
                //        foreach (CCustomerType objCustomerType in m_objSelectedCustomer.CustomerTypeList)
                //        {
                //            for (System.Int32 i = 0; i < checkListBoxTargetBuy.Items.Count; i++)
                //            {
                //                if (((CCustomerType)checkListBoxTargetBuy.Items[i].Value).ID.CompareTo(objCustomerType.ID) == 0)
                //                {
                //                    checkListBoxTargetBuy.Items[i].CheckState = CheckState.Checked;
                //                    break;
                //                }
                //            }
                //        }
                //    }
                //}
                //objFullCustomerTypeList = null;

                // цели приобретения
                List<CTargetBuy> objFullTargetBuyList = CTargetBuy.GetTargetBuyList(m_objProfile, null);
                if ((objFullTargetBuyList != null) && (objFullTargetBuyList.Count > 0))
                {
                    foreach (CTargetBuy objTargetBuy1 in objFullTargetBuyList)
                    {
                        checkListBoxTargetBuy.Items.Add(objTargetBuy1, false);
                    }
                }
                objFullTargetBuyList = null;

                // Адреса
                frmAddress.LoadAddressList(m_objSelectedCustomer.ID, null);
                // Контакты
                frmContact.LoadContactList(m_objSelectedCustomer.ID, m_objSelectedCustomer.ContactList);
                // РТТ
                frmRtt.LoadRttList(m_objSelectedCustomer);

                frmRtt.ClearAllPropertiesControls();
                frmAddress.ClearAllPropertiesControls();
                frmContact.ClearAllPropertiesControls();

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
                this.ResumeLayout(false);
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
                SimulateChangeCustomerProperties(m_objSelectedCustomer, enumActionSaveCancel.Cancel, m_bNewCustomer);
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
            CCustomer objCustomerForSave = new CCustomer();
            try
            {
                objCustomerForSave.ID = m_objSelectedCustomer.ID;
                objCustomerForSave.FullName = txtFullName.Text;
                objCustomerForSave.ShortName = txtShortName.Text;
                objCustomerForSave.Code = txtCode.Text;
                objCustomerForSave.UNP = txtUNP.Text;
                objCustomerForSave.OKPO = txtOKPO.Text;
                objCustomerForSave.OKULP = txtOKULP.Text;

                if (cboxState.SelectedItem != null)
                {
                    objCustomerForSave.StateType = (CStateType)cboxState.SelectedItem;
                }
                if (cboxActiveType.SelectedItem != null)
                {
                    objCustomerForSave.ActiveType = (CCustomerActiveType)cboxActiveType.SelectedItem;
                }
                if ( cboxDistrNet.SelectedItem != null)
                {
                    CDistributionNetwork objDistrNet = (CDistributionNetwork)cboxDistrNet.SelectedItem;
                    objCustomerForSave.DistributionNetwork = ((objDistrNet.ID.CompareTo(System.Guid.Empty) == 0) ? null : objDistrNet);
                }

                if (objCustomerForSave.PhoneList == null) { objCustomerForSave.PhoneList = new List<CPhone>(); }
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListPhone.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    objCustomerForSave.PhoneList.Add((CPhone)objNode.Tag);
                }
                objCustomerForSave.PhoneForDeleteList = m_objSelectedCustomer.PhoneForDeleteList;

                if (objCustomerForSave.LicenceList == null) { objCustomerForSave.LicenceList = new List<CLicence>(); }
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListLicence.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    objCustomerForSave.LicenceList.Add((CLicence)objNode.Tag);
                }
                objCustomerForSave.LicenceForDeleteList = m_objSelectedCustomer.LicenceForDeleteList;

                if (objCustomerForSave.AccountList == null) { objCustomerForSave.AccountList = new List<CAccount>(); }
                foreach( DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListAccounts.Nodes )
                {
                    if (objNode.Tag == null) { continue; }
                    objCustomerForSave.AccountList.Add((CAccount)objNode.Tag);
                }
                objCustomerForSave.AccountForDeleteList = m_objSelectedCustomer.AccountForDeleteList;

                if (objCustomerForSave.TargetBuyList == null) { objCustomerForSave.TargetBuyList = new List<CTargetBuy>(); }
                objCustomerForSave.TargetBuyList.Clear();
                if (checkListBoxTargetBuy.CheckedItems.Count > 0)
                {
                    for (System.Int32 i = 0; i < checkListBoxTargetBuy.Items.Count; i++)
                    {
                        if (checkListBoxTargetBuy.Items[i].CheckState == CheckState.Checked)
                        {
                            objCustomerForSave.TargetBuyList.Add((CTargetBuy)checkListBoxTargetBuy.Items[i].Value);
                        }
                    }
                }
                
                if (objCustomerForSave.EMailList == null) { objCustomerForSave.EMailList = new List<CEMail>(); }
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListEMail.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    objCustomerForSave.EMailList.Add((CEMail)objNode.Tag);
                }
                objCustomerForSave.EMailForDeleteList = m_objSelectedCustomer.EMailForDeleteList;

                objCustomerForSave.EMailForDeleteList = m_objSelectedCustomer.EMailForDeleteList;
                objCustomerForSave.ContactList = this.frmContact.ContactList;
                objCustomerForSave.ContactForDeleteList = frmContact.ContactDeletedList;
                objCustomerForSave.AddressList = frmAddress.AddressList;
                objCustomerForSave.AddressForDeleteList = frmAddress.AddressDeletedList;
                objCustomerForSave.RttList = frmRtt.RttList;
                objCustomerForSave.RttForDeleteList = frmRtt.RttDeletedList;

                System.String strErr = "";
                if (m_bNewCustomer == true)
                {
                    // новый клиент
                    bOkSave = objCustomerForSave.Add(m_objProfile, null, ref strErr);
                }
                else
                {
                    bOkSave = objCustomerForSave.Update(m_objProfile, null, ref strErr);
                }
                SendMessageToLog(strErr);
                if (bOkSave == true)
                {
                    m_objSelectedCustomer.ID = objCustomerForSave.ID;
                    m_objSelectedCustomer.FullName = objCustomerForSave.FullName;
                    m_objSelectedCustomer.ShortName = objCustomerForSave.ShortName;
                    m_objSelectedCustomer.Code = objCustomerForSave.Code;
                    m_objSelectedCustomer.UNP = objCustomerForSave.UNP;
                    m_objSelectedCustomer.OKPO = objCustomerForSave.OKPO;
                    m_objSelectedCustomer.OKULP = objCustomerForSave.OKULP;
                    m_objSelectedCustomer.StateType = objCustomerForSave.StateType;
                    m_objSelectedCustomer.DistributionNetwork = objCustomerForSave.DistributionNetwork;
                    m_objSelectedCustomer.ActiveType = objCustomerForSave.ActiveType;
                    m_objSelectedCustomer.PhoneList = objCustomerForSave.PhoneList;
                    m_objSelectedCustomer.EMailList = objCustomerForSave.EMailList;
                    m_objSelectedCustomer.LicenceList = objCustomerForSave.LicenceList;
                    m_objSelectedCustomer.ContactList = objCustomerForSave.ContactList;
                    m_objSelectedCustomer.AddressList = objCustomerForSave.AddressList;
                    m_objSelectedCustomer.RttList = objCustomerForSave.RttList;
                    m_objSelectedCustomer.CustomerTypeList = objCustomerForSave.CustomerTypeList;
                    m_objSelectedCustomer.TargetBuyList = objCustomerForSave.TargetBuyList;

                    if (frmAddress.AddressDeletedList != null) { frmAddress.AddressDeletedList.Clear(); }
                    if (frmContact.ContactDeletedList != null) { frmContact.ContactDeletedList.Clear(); }
                    if (frmRtt.RttDeletedList != null) { frmRtt.RttDeletedList.Clear(); }

                    if (m_objSelectedCustomer.AddressForDeleteList != null) { m_objSelectedCustomer.AddressForDeleteList.Clear(); }
                    if (m_objSelectedCustomer.ContactForDeleteList != null) { m_objSelectedCustomer.ContactForDeleteList.Clear(); }
                    if (m_objSelectedCustomer.RttForDeleteList != null) { m_objSelectedCustomer.RttForDeleteList.Clear(); }
                    if (m_objSelectedCustomer.LicenceForDeleteList != null) { m_objSelectedCustomer.LicenceForDeleteList.Clear(); }
                    if (m_objSelectedCustomer.PhoneForDeleteList != null) { m_objSelectedCustomer.PhoneForDeleteList.Clear(); }
                    if (m_objSelectedCustomer.AccountForDeleteList != null) { m_objSelectedCustomer.AccountForDeleteList.Clear(); }
                    if (m_objSelectedCustomer.EMailForDeleteList != null) { m_objSelectedCustomer.EMailForDeleteList.Clear(); }

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
                SendMessageToLog("Ошибка сохранения изменений в описании клиента. Текст ошибки: " + f.Message);
            }
            finally
            {
                objCustomerForSave = null;
            }
            return bRet;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                System.Boolean bRet = false;
                System.String strErr = "";

                frmAddress.ConfirmChanges();
                frmContact.ConfirmChanges();

                bRet = frmRtt.ConfirmChanges( ref strErr );
                if( bRet == true )
                {
                    if (bSaveChanges() == true)
                    {
                        SimulateChangeCustomerProperties(m_objSelectedCustomer, enumActionSaveCancel.Save, m_bNewCustomer);
                    }
                }
                else
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show( ("Сохранение изменений в карточке клиента отменено,\nт.к. не удалось подтвердить изменения в РТТ.\n\n" + strErr ),
                    "Внимание",  System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в описании клиента. Текст ошибки: " + f.Message);
            }
            return;
        }

        #endregion

        #region Режим просмотра/редактирования
        /// <summary>
        /// Устанавливает режим просмотра/редактирования
        /// </summary>
        /// <param name="bSet">true - режим просмотра; false - режим редактирования</param>
        public void SetModeReadOnly( System.Boolean bSet )
        {
            try
            {
                txtFullName.Properties.ReadOnly = bSet;
                txtShortName.Properties.ReadOnly = bSet;
                txtUNP.Properties.ReadOnly = bSet;
                txtOKPO.Properties.ReadOnly = bSet;
                txtOKULP.Properties.ReadOnly = bSet;

                checkListBoxTargetBuy.Enabled = !bSet;
                treeListAccounts.OptionsBehavior.Editable = !bSet;
                treeListLicence.OptionsBehavior.Editable = !bSet;
                treeListPhone.OptionsBehavior.Editable = !bSet;
                treeListEMail.OptionsBehavior.Editable = !bSet;
                cboxActiveType.Properties.ReadOnly = bSet;
                cboxState.Properties.ReadOnly = bSet;
                cboxDistrNet.Properties.ReadOnly = bSet;

                frmAddress.SetChanceEditProperties(!bSet);
                frmContact.SetChanceEditProperties(!bSet);
                frmRtt.SetChanceEditProperties(!bSet);

                //txtFullName.Properties.Appearance.BackColor = (bSet == false) ? System.Drawing.SystemColors.ActiveCaptionText : System.Drawing.SystemColors.InactiveCaptionText;
                //txtShortName.Properties.Appearance.BackColor = (bSet == false) ? System.Drawing.SystemColors.ActiveCaptionText : System.Drawing.SystemColors.InactiveCaptionText;
                //txtUNP.Properties.Appearance.BackColor = (bSet == false) ? System.Drawing.SystemColors.ActiveCaptionText : System.Drawing.SystemColors.InactiveCaptionText;
                //txtOKPO.Properties.Appearance.BackColor = (bSet == false) ? System.Drawing.SystemColors.ActiveCaptionText : System.Drawing.SystemColors.InactiveCaptionText;
                //txtOKULP.Properties.Appearance.BackColor = (bSet == false) ? System.Drawing.SystemColors.ActiveCaptionText : System.Drawing.SystemColors.InactiveCaptionText;

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
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Refresh();
                frmAddress.StartThreadWithLoadData();
                frmContact.frmAddress.StartThreadWithLoadData();
                frmRtt.frmAddress.StartThreadWithLoadData();

                SetModeReadOnly( false );
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

        #region Печать карточки клиента
        private void ExportToExcel( CCustomer objCustomer )
        {
            Excel.Application oXL = null;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;
            //Excel.Range oRng;
            System.Int32 iLastIndxRowForPrint = 0;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                //Start Excel and get Application object.
                oXL = new Excel.Application();
                //oXL.Visible = true;

                //Get a new workbook.
                oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                //oSheet = (Excel._Worksheet)oWB.ActiveSheet;
                oSheet = (Excel._Worksheet)oWB.Worksheets[1];
                System.Int32 iSheetsCount = oWB.Worksheets.Count;

                for (System.Int32 i = iSheetsCount; i >1 ; i--)
                {
                    ((Excel._Worksheet)oWB.Worksheets[i]).Delete();
                }

                //Клиент
                oSheet.Name = "Клиент";
                oSheet.Cells[2, 1] = "Код клиента"; // lblInfo.Text.Replace('\t', ' ');
                oSheet.Cells[2, 2] = objCustomer.InterBaseID;
                oSheet.Cells[3, 1] = "Наименование клиента";
                oSheet.Cells[3, 2] = objCustomer.FullName;
                oSheet.Cells[4, 1] = "Форма собственности";
                oSheet.Cells[4, 2] = (objCustomer.StateType == null) ? "" : objCustomer.StateType.ShortName;
                oSheet.Cells[5, 1] = "Сетевой клиент";
                oSheet.Cells[5, 2] = (objCustomer.DistributionNetwork == null) ? "" : objCustomer.DistributionNetwork.Name;
                oSheet.Cells[6, 1] = "УНП";
                oSheet.Cells[6, 2] = objCustomer.UNP;
                oSheet.Cells[7, 1] = "ОКПО";
                oSheet.Cells[7, 2] = objCustomer.OKPO;
                oSheet.Cells[8, 1] = "Юр. Адрес";
                oSheet.Cells[8, 2] = ((frmAddress.AddressList == null) || (frmAddress.AddressList.Count == 0)) ? "" : frmAddress.AddressList[0].VisitingCard2;
                oSheet.Cells[9, 1] = "Лицензии";
                iLastIndxRowForPrint = 10;
                if ((objCustomer.LicenceList != null) && (objCustomer.LicenceList.Count > 0))
                {
                    foreach( CLicence objLicence in objCustomer.LicenceList )
                    {
                        oSheet.Cells[iLastIndxRowForPrint, 2] = "№" + objLicence.LicenceNum + " " + objLicence.LicenceType.Name + " " + objLicence.BeginDate.ToShortDateString() + " - " + objLicence.EndDate.ToShortDateString() + " выдана " + objLicence.WhoGive;
                        iLastIndxRowForPrint++;
                    }
                }
                oSheet.Cells[iLastIndxRowForPrint, 1] = "Расчетные счета";
                iLastIndxRowForPrint++;
                if ((objCustomer.AccountList != null) && (objCustomer.AccountList.Count > 0))
                {
                    foreach (CAccount objAccount in objCustomer.AccountList)
                    {
                        oSheet.Cells[iLastIndxRowForPrint, 2] = "№" + objAccount.AccountNumber + " " + objAccount.Currency.CurrencyAbbr + " " + objAccount.Bank.Code + " " + objAccount.Bank.Name;
                        iLastIndxRowForPrint++;
                    }
                }
                oSheet.Cells[iLastIndxRowForPrint, 1] = "Контакты";
                iLastIndxRowForPrint++;
                if ((frmContact.ContactList != null) && (frmContact.ContactList.Count > 0))
                {
                    foreach (CContact objContact in frmContact.ContactList)
                    {
                        oSheet.Cells[iLastIndxRowForPrint, 2] = objContact.VisitingCard2;
                        iLastIndxRowForPrint++;
                    }
                }
                oSheet.get_Range(oSheet.Cells[1, 1], oSheet.Cells[iLastIndxRowForPrint, 1]).Font.Bold = true;
                oSheet.get_Range(oSheet.Cells[1, 1], oSheet.Cells[iLastIndxRowForPrint, 1]).Font.Size = 12;
                oSheet.get_Range("A1", "A1").EntireColumn.AutoFit();
                oSheet.get_Range("B1", "B1").EntireColumn.AutoFit();
                oSheet.get_Range(oSheet.Cells[1, 2], oSheet.Cells[iLastIndxRowForPrint, 2]).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

                //РТТ
                if ((frmRtt.RttList != null) && (frmRtt.RttList.Count > 0))
                {
                    foreach (CRtt objRtt in frmRtt.RttList)
                    {
                        oSheet = (Excel._Worksheet)oWB.Worksheets.Add(Missing.Value, Missing.Value, 1, Excel.XlSheetType.xlWorksheet);
                        oSheet.Name = objRtt.Code;
                        oSheet.Move(Missing.Value, (Excel._Worksheet)oWB.Worksheets[oWB.Worksheets.Count]);

                        oSheet.Cells[2, 1] = "Код РТТ"; // lblInfo.Text.Replace('\t', ' ');
                        oSheet.Cells[2, 2] = objRtt.Code;
                        oSheet.Cells[3, 1] = "Наименование РТТ";
                        oSheet.Cells[3, 2] = objRtt.FullName;
                        oSheet.Cells[4, 1] = "Тип лицензии";
                        oSheet.Cells[4, 2] = (objRtt.LicenceType == null) ? "" : objRtt.LicenceType.Name;
                        oSheet.Cells[5, 1] = "Признак активности";
                        oSheet.Cells[5, 2] = (objRtt.RttActiveType == null) ? "" : objRtt.RttActiveType.Name;
                        oSheet.Cells[6, 1] = "Спецкод";
                        oSheet.Cells[6, 2] = (objRtt.RttSpecCode == null) ? "" : objRtt.RttSpecCode.Name;
                        oSheet.Cells[7, 1] = "Адрес";
                        oSheet.Cells[7, 2] = ((frmAddress.AddressList == null) || (frmAddress.AddressList.Count == 0)) ? "" : frmAddress.AddressList[0].VisitingCard2;
                        oSheet.Cells[8, 1] = "Режим работы";
                        oSheet.Cells[8, 2] = (objRtt.ActionPeriod == null) ? "" : objRtt.ActionPeriod.ShortShedule;
                        oSheet.Cells[9, 1] = "Сегментация";
                        oSheet.Cells[9, 2] = (objRtt.Segmentation == null) ? "" : objRtt.Segmentation.Code;
                        oSheet.Cells[10, 1] = "Оборудование";
                        iLastIndxRowForPrint = 11;
                        if ((objRtt.TradeEquipmentList != null) && (objRtt.TradeEquipmentList.Count > 0))
                        {
                            foreach (CTradeEquipment objTradeEquipment in objRtt.TradeEquipmentList)
                            {
                                oSheet.Cells[iLastIndxRowForPrint, 2] = objTradeEquipment.EquipmentType.ProductCatalogName;
                                oSheet.Cells[iLastIndxRowForPrint, 3] = objTradeEquipment.EquipmentType.Code;
                                oSheet.Cells[iLastIndxRowForPrint, 4] = ( (objTradeEquipment.EquipmentType == null) ? "" : objTradeEquipment.EquipmentType.Name );
                                oSheet.Cells[iLastIndxRowForPrint, 5] = ((objTradeEquipment.Availability == null) ? "" : objTradeEquipment.Availability.Name);
                                oSheet.Cells[iLastIndxRowForPrint, 6] = objTradeEquipment.EquipmentType.SizeName;
                                oSheet.Cells[iLastIndxRowForPrint, 7] = objTradeEquipment.Quantity;
                                iLastIndxRowForPrint++;
                            }
                        }
                        oSheet.Cells[iLastIndxRowForPrint, 1] = "Присутствие ТМ";
                        iLastIndxRowForPrint++;
                        if ((objRtt.ProductCatalogList != null) && (objRtt.ProductCatalogList.Count > 0))
                        {
                            foreach (CProductCatalog objProductCatalog in objRtt.ProductCatalogList)
                            {
                                oSheet.Cells[iLastIndxRowForPrint, 2] = objProductCatalog.Name;
                                iLastIndxRowForPrint++;
                            }
                        }
                        oSheet.Cells[iLastIndxRowForPrint, 1] = "Контакты";
                        iLastIndxRowForPrint++;
                        if ((objRtt.ContactList != null) && (objRtt.ContactList.Count > 0))
                        {
                            foreach (CContact objContact in objRtt.ContactList)
                            {
                                oSheet.Cells[iLastIndxRowForPrint, 2] = objContact.VisitingCard2;
                                iLastIndxRowForPrint++;
                            }
                        }

                        oSheet.get_Range(oSheet.Cells[1, 1], oSheet.Cells[iLastIndxRowForPrint, 1]).Font.Bold = true;
                        oSheet.get_Range(oSheet.Cells[1, 1], oSheet.Cells[iLastIndxRowForPrint, 1]).Font.Size = 12;
                        oSheet.get_Range("A1", "A1").EntireColumn.AutoFit();
                        oSheet.get_Range(oSheet.Cells[1, 2], oSheet.Cells[iLastIndxRowForPrint, 2]).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                    }
                }
                ((Excel._Worksheet)oWB.Worksheets[1]).Activate();

                oXL.Visible = true;
                oXL.UserControl = true;
            }
            catch (System.Exception f)
            {
                if (oXL != null) { oXL.Quit(); }
                oXL = null;
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка экспорта в MS Excel.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }
        #endregion

        private void ctrlCustomer_Resize(object sender, EventArgs e)
        {
            layoutControlAddress.MaximumSize = new Size(this.Size.Width - iMinControlItemHeight, layoutControlAddress.MaximumSize.Height);
            layoutControlContact.MaximumSize = new Size(this.Size.Width - iMinControlItemHeight, layoutControlContact.MaximumSize.Height);
            layoutControlRtt.MaximumSize = new Size(this.Size.Width - iMinControlItemHeight, layoutControlRtt.MaximumSize.Height);
        }

        private void layoutControl_GroupExpandChanged(object sender, DevExpress.XtraLayout.Utils.LayoutGroupEventArgs e)
        {
            DevExpress.XtraLayout.LayoutControl LayoutControl = (DevExpress.XtraLayout.LayoutControl)sender;
            if (e.Group.Expanded == true)
            {
                LayoutControl.Size = new Size(LayoutControl.Size.Width, (LayoutControl.MaximumSize.Height));
            }
            else
            {
                LayoutControl.Size = new Size(LayoutControl.Size.Width, iMinControlItemHeight);
            }
        }

        private void layoutControlContact_GroupExpandChanged(object sender, DevExpress.XtraLayout.Utils.LayoutGroupEventArgs e)
        {
            DevExpress.XtraLayout.LayoutControl LayoutControl = (DevExpress.XtraLayout.LayoutControl)sender;
            if (e.Group.Expanded == true)
            {
                LayoutControl.Size = new Size(LayoutControl.Size.Width, (LayoutControl.MaximumSize.Height + iMinControlItemHeight));
            }
            else
            {
                LayoutControl.Size = new Size(LayoutControl.Size.Width, iMinControlItemHeight);
            }
        }

        private void ctrlCustomer_Load(object sender, EventArgs e)
        {
            splitContainerControl.SplitterPosition = iPanel1WidthDef;
        }

        private void layoutControlPhone_GroupExpandChanged(object sender, DevExpress.XtraLayout.Utils.LayoutGroupEventArgs e)
        {
            if (e.Group.Expanded == true)
            {
                if (e.Group == layoutControlGroupPhone)
                {
                    layoutControlGroupLicense.Expanded = false;
                    layoutControlGroupAccounts.Expanded = false;
                    layoutControlGroupEMail.Expanded = false;
                }
                if (e.Group == layoutControlGroupLicense)
                {
                    layoutControlGroupPhone.Expanded = false;
                    layoutControlGroupAccounts.Expanded = false;
                    layoutControlGroupEMail.Expanded = false;
                }
                if (e.Group == layoutControlGroupAccounts)
                {
                    layoutControlGroupPhone.Expanded = false;
                    layoutControlGroupLicense.Expanded = false;
                    layoutControlGroupEMail.Expanded = false;
                }
                if (e.Group == layoutControlGroupEMail)
                {
                    layoutControlGroupPhone.Expanded = false;
                    layoutControlGroupLicense.Expanded = false;
                    layoutControlGroupAccounts.Expanded = false;
                }
            }
        }

        private void treeListPhone_CellValueChanging(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
            if (m_bDisableEvents == true) { return; }
            try
            {
                System.Int32 iPosNode = treeListPhone.GetNodeIndex(e.Node);

                if ((e.Column == colPhoneNumber) && (e.Value != null))
                {
                    if (IsPhoneValid((System.String)e.Value) == false)
                    {
                        treeListPhone.SetColumnError(colPhoneNumber, "Телефонный номер не соответсвует принятым требованиям.");
                    }
                    else
                    {
                        // проверим, возможно такой телефонный номер уже есть в списке
                        if (IsPhoneDublicate((System.String)e.Value, iPosNode) == true)
                        {
                            treeListPhone.SetColumnError(colPhoneNumber, "Такой телефонный номер уже есть в списке.");
                        }
                        else
                        {
                            //m_objSelectedContact.PhoneList[iPosNode].PhoneNumber = (System.String)e.Value;
                            ((CPhone)e.Node.Tag).PhoneNumber = (System.String)e.Value;
                            e.Node.SetValue(colPhoneNumber, e.Value);
                        }
                    }
                }
                if ((e.Column == colPhoneIsMain) && (e.Value != null) && (((System.Boolean)e.Value) == true))
                {
                    // главным може быть только один телефонный номер,
                    // поэтому все остальные нужно сделать неглавными
                    for (System.Int32 i = 0; i < treeListPhone.Nodes.Count; i++)
                    {
                        if (i != iPosNode)
                        {
                            treeListPhone.Nodes[i].SetValue(colPhoneIsMain, false);
                            ((CPhone)treeListPhone.Nodes[i].Tag).IsMain = false;
                            e.Node.SetValue(colPhoneIsMain, e.Value);
                        }
                    }
                    //m_objSelectedContact.PhoneList[iPosNode].IsMain = true;
                    ((CPhone)e.Node.Tag).IsMain = true;
                    e.Node.SetValue(colPhoneIsActive, true);
                    e.Node.SetValue(colPhoneIsMain, true);
                }
                if ((e.Column == colPhoneIsActive) && (e.Value != null))
                {
                    //m_objSelectedContact.PhoneList[iPosNode].IsActive = (System.Boolean)e.Value;
                    ((CPhone)e.Node.Tag).IsActive = (System.Boolean)e.Value;
                    e.Node.SetValue(colPhoneIsActive, e.Value);
                }
                if ((e.Column == colPhoneType) && (e.Value != null))
                {
                    ((CPhone)e.Node.Tag).PhoneType = (CPhoneType)e.Value;
                    e.Node.SetValue(colPhoneType, e.Value);
                }
                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListPhone_CellValueChanged. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_objSelectedCustomer == null) { return; }
                Cursor.Current = Cursors.WaitCursor;
                ExportToExcel( m_objSelectedCustomer );
                Cursor.Current = Cursors.Default;

            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(this, "Ошибка печати\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;

        }
    }

    /// <summary>
    /// Тип, хранящий информацию, которая передается получателям уведомления о событии
    /// </summary>
    public partial class ChangeCustomerPropertieEventArgs : EventArgs
    {
        private readonly CCustomer m_objCustomer;
        public CCustomer Customer
        { get { return m_objCustomer; } }

        private readonly enumActionSaveCancel m_enActionType;
        public enumActionSaveCancel ActionType
        { get { return m_enActionType; } }

        private readonly System.Boolean m_bIsNewCustomer;
        public System.Boolean IsNewCustomer
        { get { return m_bIsNewCustomer; } }

        public ChangeCustomerPropertieEventArgs(CCustomer objCustomer, enumActionSaveCancel enActionType, System.Boolean bIsNewCustomer)
        {
            m_objCustomer = objCustomer;
            m_enActionType = enActionType;
            m_bIsNewCustomer = bIsNewCustomer;
        }
    }

}
