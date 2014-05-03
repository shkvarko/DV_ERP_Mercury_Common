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
    public partial class ctrlProtocolAgreementPrice : UserControl
    {
        #region Свойства
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;
        private List<CProduct> m_objPartsList;
        private frmPartsList m_objFrmPartsList;

        private CProtocolAgreementPrice m_objSelectedAgreement;

        private System.Boolean m_bIsChanged;

        private System.Boolean m_bDisableEvents;
        private System.Boolean m_bNewObject;
        private System.Boolean m_bIsReadOnly;

        private const System.Int32 iMinControlItemHeight = 20;
        private const System.Int32 iPanel1WidthDef = 350;

        // потоки
        private System.Threading.Thread thrAddress;
        public System.Threading.Thread ThreadAddress
        {
            get { return thrAddress; }
        }
        private System.Threading.ManualResetEvent m_EventStopThread;
        public System.Threading.ManualResetEvent EventStopThread
        {
            get { return m_EventStopThread; }
        }
        private System.Threading.ManualResetEvent m_EventThreadStopped;
        public System.Threading.ManualResetEvent EventThreadStopped
        {
            get { return m_EventThreadStopped; }
        }
        public delegate void LoadPartsListDelegate();
        public LoadPartsListDelegate m_LoadPartsListDelegate;

        public delegate void SendMessageToLogDelegate(System.String strMessage);
        public SendMessageToLogDelegate m_SendMessageToLogDelegate;

        public delegate void SetProductListToFormDelegate(List<CProduct> objProductNewList);
        public SetProductListToFormDelegate m_SetProductListToFormDelegate;

        private const System.Int32 iThreadSleepTime = 1000;
        private System.Boolean m_bThreadFinishJob;

        private const System.Double dblNDS = 20;
        #endregion

        #region События
        // Создаем закрытое поле, ссылающееся на заголовок списка делегатов
        private EventHandler<ChangeProtocolAgreementPricePropertieEventArgs> m_ChangeAgreementProperties;
        // Создаем в классе член-событие
        public event EventHandler<ChangeProtocolAgreementPricePropertieEventArgs> ChangeAgreementProperties
        {
            add
            {
                // берем закрытую блокировку и добавляем обработчик
                // (передаваемый по значению) в список делегатов
                m_ChangeAgreementProperties += value;
            }
            remove
            {
                // берем закрытую блокировку и удаляем обработчик
                // (передаваемый по значению) из списка делегатов
                m_ChangeAgreementProperties -= value;
            }
        }
        /// <summary>
        /// Инициирует событие и уведомляет о нем зарегистрированные объекты
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnChangeProtocolAgreementPriceProperties(ChangeProtocolAgreementPricePropertieEventArgs e)
        {
            // Сохраняем поле делегата во временном поле для обеспечение безопасности потока
            EventHandler<ChangeProtocolAgreementPricePropertieEventArgs> temp = m_ChangeAgreementProperties;
            // Если есть зарегистрированные объектв, уведомляем их
            if (temp != null) temp(this, e);
        }
        public void SimulateChangeProtocolAgreementPriceProperties(CProtocolAgreementPrice objAgreement, enumActionSaveCancel enActionType, System.Boolean bIsNewAgreement)
        {
            // Создаем объект, хранящий информацию, которую нужно передать
            // объектам, получающим уведомление о событии
            ChangeProtocolAgreementPricePropertieEventArgs e = new ChangeProtocolAgreementPricePropertieEventArgs(objAgreement, enActionType, bIsNewAgreement);

            // Вызываем виртуальный метод, уведомляющий наш объект о возникновении события
            // Если нет типа, переопределяющего этот метод, наш объект уведомит все объекты, 
            // подписавшиеся на уведомление о событии
            OnChangeProtocolAgreementPriceProperties(e);
        }
        #endregion

        #region Конструктор
        public ctrlProtocolAgreementPrice(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem)
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_bIsChanged = false;
            m_bDisableEvents = false;
            m_bNewObject = false;

            m_objSelectedAgreement = null;
            m_objFrmPartsList = null;
            btnSetToPartList.Visible = false;
            calcNDS.Value = System.Convert.ToDecimal( dblNDS );
            calcEditDiscountPercent.Value = 0;

            LoadComboBoxItems();
            m_bIsReadOnly = false;

            StartThreadWithLoadData();
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
                    tableLayoutPanel2.RowStyles[3].Height = iRowWarnigHeith;
                }
                else
                {
                    tableLayoutPanel2.RowStyles[3].Height = 0;
                }
                tableLayoutPanel2.Size = new Size(tableLayoutPanel2.Size.Width,
                    (System.Convert.ToInt32(tableLayoutPanel2.RowStyles[3].Height + iRowBtnHeith)));
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

        #region Индикация изменений
        /// <summary>
        /// Проверяет содержимое элементов управления
        /// </summary>
        private System.Boolean ValidateProperties()
        {
            System.Boolean bRet = true;
            try
            {
                cboxAgreementState.Properties.Appearance.BackColor = ((cboxAgreementState.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                cboxCompany.Properties.Appearance.BackColor = ((cboxCompany.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                cboxCustomer.Properties.Appearance.BackColor = ((cboxCustomer.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);

                txtAgreementNum.Properties.Appearance.BackColor = ((txtAgreementNum.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                dtBeginDate.Properties.Appearance.BackColor = ((dtBeginDate.DateTime == System.DateTime.MinValue) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                dtEndDate.Properties.Appearance.BackColor = ((dtEndDate.DateTime == System.DateTime.MinValue) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);

                if (cboxAgreementState.SelectedItem == null) { bRet = false; }
                if (cboxCompany.SelectedItem == null) { bRet = false; }
                if (cboxCustomer.SelectedItem == null) { bRet = false; }
                if (txtAgreementNum.Text == "") { bRet = false; }
                if (dtBeginDate.DateTime == System.DateTime.MinValue) { bRet = false; }
                if (dtEndDate.DateTime == System.DateTime.MinValue) { bRet = false; }

            }
            catch (System.Exception f)
            {
                SendMessageToLog("ValidateProperties. Текст ошибки: " + f.Message);
            }

            return bRet;
        }
        private void SetPropertiesModified(System.Boolean bModified)
        {
            try
            {
                m_bIsChanged = bModified;
                btnSave.Enabled = (m_bIsChanged && (ValidateProperties() == true));
                btnCancel.Enabled = m_bIsChanged;
                if (m_bIsChanged == true)
                {
                    SimulateChangeProtocolAgreementPriceProperties(m_objSelectedAgreement, enumActionSaveCancel.Unkown, m_bNewObject);
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
        private void cboxAgreementPropertie_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                SetPropertiesModified(true);
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка изменения свойств " + ((DevExpress.XtraEditors.ComboBoxEdit)sender).ToolTip + ". Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        private void txtAgreementPropertie_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
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
                SendMessageToLog("Ошибка изменения свойств ПСЦ. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        private void treeList_CellValueChanged(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                SetPropertiesModified(true);

                if ((e.Column == colDiscountPercent) || (e.Column == colNDSPercent) || (e.Column == colPrice))
                {
                    System.Double PriceWithDiscount = CProtocolAgreementPriceItem.CalcPriceWithDiscount(System.Convert.ToDouble(e.Node.GetValue(colPrice)),
                        System.Convert.ToDouble(e.Node.GetValue(colDiscountPercent)), false);

                    System.Double PriceWithDiscountNDS = CProtocolAgreementPriceItem.AddNDSToPrice(PriceWithDiscount, System.Convert.ToDouble(e.Node.GetValue(colNDSPercent)));


                    e.Node.SetValue(colPriceWithDiscount, PriceWithDiscount);

                    e.Node.SetValue(colPriceWithDiscountNDS, PriceWithDiscountNDS);
                }

            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка изменения свойств ПСЦ. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;

        }
        private void treeList_CellValueChanging(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                SetPropertiesModified(true);
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка изменения свойств ПСЦ. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
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
                if (m_objProfile.GetClientsRight().GetState(ERP_Mercury.Global.Consts.strDR_EditAgreementWithCustomerCard) == false)
                {
                    btnEdit.Visible = false;
                    btnPrint.Visible = false;
                    btnSave.Visible = false;
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
        /// Обновление выпадающих списков
        /// </summary>
        /// <returns>true - все списки успешно обновлены; false - ошибка</returns>
        public System.Boolean LoadComboBoxItems()
        {
            System.Boolean bRet = false;
            try
            {

                // Компании
                cboxCompany.Properties.Items.Clear();
                cboxCompany.Properties.Items.AddRange(CCompany.GetCompanyList(m_objProfile, null));

                // Клиенты
                cboxCustomer.Properties.Items.Clear();
                cboxCustomer.Properties.Items.AddRange(CCustomer.GetCustomerListWithoutAdvancedProperties(m_objProfile, null, null));

                // Состояние
                cboxAgreementState.Properties.Items.Clear();
                cboxAgreementState.Properties.Items.AddRange(CAgreementState.GetAgreementStateList(m_objProfile, null));

                bRet = true;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка обновления выпадающих списков. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return bRet;
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
                txtAgreementNum.Properties.ReadOnly = bSet;
                txtDescription.Properties.ReadOnly = bSet;
                cboxAgreementState.Properties.ReadOnly = bSet;
                cboxCompany.Properties.ReadOnly = bSet;
                cboxCustomer.Properties.ReadOnly = bSet;
                dtBeginDate.Properties.ReadOnly = bSet;
                dtEndDate.Properties.ReadOnly = bSet;
                btnSetDiscount.Enabled = !bSet;
                btnSetToPartList.Enabled = !bSet;
                btnDeleteNodes.Enabled = !bSet;

                mitemAddParts.Enabled = !bSet;
                mitemDeleteParts.Enabled = !bSet;
                mitemsDeleteAllParts.Enabled = !bSet;
                mitmsLoadStockRest.Enabled = !bSet;
                mitmsRemoveRowsWithNullStockRest.Enabled = !bSet;

                calcEditDiscountPercent.Properties.ReadOnly = bSet;
                calcEditDiscountPercent.Enabled = !bSet;
                
                calcNDS.Properties.ReadOnly = bSet;
                calcNDS.Enabled = !bSet;
                
                btnSetDiscount.Enabled = !bSet;
                treeList.OptionsBehavior.Editable = !bSet;

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

        #region Редактировать ПСЦ
        /// <summary>
        /// очистка содержимого элементов управления
        /// </summary>
        private void ClearControls()
        {
            try
            {
                txtAgreementNum.Text = "";
                txtDescription.Text = "";
                cboxAgreementState.SelectedItem = null;
                cboxCompany.SelectedItem = null;
                cboxCustomer.SelectedItem = null;
                dtBeginDate.DateTime = System.DateTime.Today;
                dtEndDate.DateTime = System.DateTime.Today;

                treeList.Nodes.Clear();

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

        /// <summary>
        /// Загружает свойства ПСЦ для редактирования
        /// </summary>
        /// <param name="objAgreement">клиент</param>
        public void EditProtocolAgreementPrice(CProtocolAgreementPrice objAgreement)
        {
            if (objAgreement == null) { return; }
            m_bDisableEvents = true;
            m_bNewObject = false;

            ShowWarningPnl(false);
            try
            {
                m_objSelectedAgreement = objAgreement;
                m_objSelectedAgreement.PriceItemList = CProtocolAgreementPriceItem.GetProtocolAgreementPriceItemList(m_objProfile,
                    null, m_objSelectedAgreement.ID);

                this.SuspendLayout();

                ClearControls();

                txtAgreementNum.Text = m_objSelectedAgreement.DocumentNumber;
                txtDescription.Text = m_objSelectedAgreement.Description;
                dtBeginDate.DateTime = m_objSelectedAgreement.BeginDate;
                dtEndDate.DateTime = m_objSelectedAgreement.EndDate;

                cboxAgreementState.SelectedItem = (m_objSelectedAgreement.AgreementSate == null) ? null : cboxAgreementState.Properties.Items.Cast<CAgreementState>().Single<CAgreementState>(x => x.ID.CompareTo(m_objSelectedAgreement.AgreementSate.ID) == 0);
                cboxCompany.SelectedItem = (m_objSelectedAgreement.Company == null) ? null : cboxCompany.Properties.Items.Cast<CCompany>().Single<CCompany>(x => x.ID.CompareTo(m_objSelectedAgreement.Company.ID) == 0);
                cboxCustomer.SelectedItem = (m_objSelectedAgreement.Customer == null) ? null : cboxCustomer.Properties.Items.Cast<CCustomer>().Single<CCustomer>(x => x.ID.CompareTo(m_objSelectedAgreement.Customer.ID) == 0);

                if (m_objSelectedAgreement.PriceItemList != null)
                {
                    foreach (CProtocolAgreementPriceItem objItem in m_objSelectedAgreement.PriceItemList)
                    {
                        treeList.AppendNode(new object[] { objItem.Product.ProductTradeMarkName, objItem.Product.ProductTypeName, 
                            objItem.Product.ProductSubTypeName, objItem.Product.ProductFullName, true, objItem.Price, 
                            objItem.DiscountPercent, objItem.PriceWithDiscount, objItem.NDSPercent, objItem.PriceWithDiscountNDS, 0 }, null).Tag = objItem.Product;
                    }
                }
                else
                {
                    treeList.Nodes.Clear();
                }

                SetPropertiesModified(false);
                btnCancel.Enabled = true;
                btnCancel.Focus();

                SetModeReadOnly(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования ПСЦ с клиентом. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.ResumeLayout(false);
                m_bDisableEvents = false;
            }
            return;
        }
        #endregion

        #region Новый ПСЦ
        /// <summary>
        /// Новый ПСЦ
        /// </summary>
        public void NewAgreement()
        {
            try
            {
                m_bNewObject = true;
                m_bDisableEvents = true;

                m_objSelectedAgreement = new CProtocolAgreementPrice();

                this.SuspendLayout();

                ClearControls();

                txtAgreementNum.Text = m_objSelectedAgreement.DocumentNumber;
                txtDescription.Text = m_objSelectedAgreement.Description;
                dtBeginDate.DateTime = m_objSelectedAgreement.BeginDate;
                dtEndDate.DateTime = m_objSelectedAgreement.EndDate;

                cboxAgreementState.SelectedItem = (m_objSelectedAgreement.AgreementSate == null) ? null : cboxAgreementState.Properties.Items.Cast<CAgreementState>().Single<CAgreementState>(x => x.ID.CompareTo(m_objSelectedAgreement.AgreementSate.ID) == 0);
                cboxCompany.SelectedItem = (m_objSelectedAgreement.Company == null) ? null : cboxCompany.Properties.Items.Cast<CCompany>().Single<CCompany>(x => x.ID.CompareTo(m_objSelectedAgreement.Company.ID) == 0);
                cboxCustomer.SelectedItem = (m_objSelectedAgreement.Customer == null) ? null : cboxCustomer.Properties.Items.Cast<CCustomer>().Single<CCustomer>(x => x.ID.CompareTo(m_objSelectedAgreement.Customer.ID) == 0);

                btnEdit.Enabled = false;
                btnCancel.Enabled = true;
                btnCancel.Focus();

                SetModeReadOnly(false);
                SetPropertiesModified(true);

            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка создания ПСЦ. Текст ошибки: " + f.Message);
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
                SendMessageToLog("Ошибка отмены изменений. Текст ошибки: " + f.Message);
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
                SimulateChangeProtocolAgreementPriceProperties(m_objSelectedAgreement, enumActionSaveCancel.Cancel, m_bNewObject);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка отмены изменений в описании ПСЦ. Текст ошибки: " + f.Message);
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
            CProtocolAgreementPrice objAgreementForSave = new CProtocolAgreementPrice();
            try
            {
                objAgreementForSave.ID = m_objSelectedAgreement.ID;
                objAgreementForSave.DocumentNumber = txtAgreementNum.Text;
                objAgreementForSave.BeginDate = dtBeginDate.DateTime;
                objAgreementForSave.EndDate = dtEndDate.DateTime;
                objAgreementForSave.Description = txtDescription.Text;
                objAgreementForSave.AgreementSate = ((cboxAgreementState.SelectedItem == null) ? null : (CAgreementState)cboxAgreementState.SelectedItem);
                objAgreementForSave.Customer = ((cboxCustomer.SelectedItem == null) ? null : (CCustomer)cboxCustomer.SelectedItem);
                objAgreementForSave.Company = ((cboxCompany.SelectedItem == null) ? null : (CCompany)cboxCompany.SelectedItem);

                //приложение
                objAgreementForSave.PriceItemList = new List<CProtocolAgreementPriceItem>();
                CProtocolAgreementPriceItem objItem = null;
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    if (System.Convert.ToBoolean(objNode.GetValue(colProductCheck)) == false) { continue; }
                    objItem = new CProtocolAgreementPriceItem();
                    objItem.Product = (CProduct)objNode.Tag;
                    objItem.Price = System.Convert.ToDouble(objNode.GetValue(colPrice));
                    objItem.DiscountPercent = System.Convert.ToDouble(objNode.GetValue(colDiscountPercent));
                    objItem.PriceWithDiscount = System.Convert.ToDouble(objNode.GetValue(colPriceWithDiscount));
                    objItem.NDSPercent = System.Convert.ToDouble(objNode.GetValue(colNDSPercent));
                    objItem.PriceWithDiscountNDS = System.Convert.ToDouble(objNode.GetValue(colPriceWithDiscountNDS));

                    objAgreementForSave.PriceItemList.Add(objItem);
                }
                objItem = null;

                System.String strErr = "";
                if (m_bNewObject == true)
                {
                    // новый ПСЦ
                    bOkSave = objAgreementForSave.AddProtocolAgreementPrice( m_objProfile, null, ref strErr );
                }
                else
                {
                    bOkSave = objAgreementForSave.EditProtocolAgreementPrice(m_objProfile, null, ref strErr);
                }
                SendMessageToLog(strErr);
                if (bOkSave == true)
                {
                    m_objSelectedAgreement.ID = objAgreementForSave.ID;

                    m_objSelectedAgreement.DocumentNumber = objAgreementForSave.DocumentNumber;
                    m_objSelectedAgreement.BeginDate = objAgreementForSave.BeginDate;
                    m_objSelectedAgreement.EndDate = objAgreementForSave.EndDate;
                    m_objSelectedAgreement.Description = objAgreementForSave.Description;
                    m_objSelectedAgreement.AgreementSate = objAgreementForSave.AgreementSate;
                    m_objSelectedAgreement.Customer = objAgreementForSave.Customer;
                    m_objSelectedAgreement.Company = objAgreementForSave.Company;
                    
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
                SendMessageToLog("Ошибка сохранения изменений в описании ПСЦ. Текст ошибки: " + f.Message);
            }
            finally
            {
                objAgreementForSave = null;
            }
            return bRet;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (bSaveChanges() == true)
                {
                    SimulateChangeProtocolAgreementPriceProperties(m_objSelectedAgreement, enumActionSaveCancel.Save, m_bNewObject);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в описании ПСЦ. Текст ошибки: " + f.Message);
            }
            return;
        }

        #endregion

        #region Потоки
        /// <summary>
        /// Создает форму со списком товара
        /// </summary>
        /// <param name="objPartsList">списком товара</param>
        private void SetProductListToForm(List<CProduct> objPartsList)
        {
            try
            {
                if (m_objFrmPartsList == null)
                {
                    m_objFrmPartsList = new frmPartsList(m_objProfile, m_objMenuItem, objPartsList);
                    btnSetToPartList.Visible = true;
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("SetProductListToForm. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

        /// <summary>
        /// загружает список товаров и список новых товаров
        /// </summary>
        private void LoadPartssList()
        {
            try
            {
                // товары
                m_objPartsList = CProduct.GetProductList(m_objProfile, null, false);
                if (m_objPartsList != null)
                {
                    this.Invoke(m_SetProductListToFormDelegate, new Object[] { m_objPartsList });
                }
            }
            catch (System.Exception f)
            {
                this.Invoke(m_SendMessageToLogDelegate, new Object[] { ("Ошибка обновления списка новинок. Текст ошибки: " + f.Message) });
            }
            finally
            {
                EventStopThread.Set();
            }

            return;
        }

        public void StartThreadWithLoadData()
        {
            try
            {
                // инициализируем события
                this.m_EventStopThread = new System.Threading.ManualResetEvent(false);
                this.m_EventThreadStopped = new System.Threading.ManualResetEvent(false);

                // инициализируем делегаты
                m_LoadPartsListDelegate = new LoadPartsListDelegate(LoadPartssList);
                m_SetProductListToFormDelegate = new SetProductListToFormDelegate(SetProductListToForm);
                m_SendMessageToLogDelegate = new SendMessageToLogDelegate(SendMessageToLog);

                // запуск потока
                StartThread();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("StartThreadWithLoadData().\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        private void StartThread()
        {
            try
            {
                // делаем событиям reset
                this.m_EventStopThread.Reset();
                this.m_EventThreadStopped.Reset();

                this.thrAddress = new System.Threading.Thread(WorkerThreadFunction);
                this.thrAddress.Start();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("StartThread().\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        public void WorkerThreadFunction()
        {
            try
            {
                Run();
            }
            catch (System.Exception e)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("WorkerThreadFunction\n" + e.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            return;
        }

        public void Run()
        {
            try
            {
                LoadPartssList();

                // пока заполняется список товаров будем проверять, не было ли сигнала прекратить все это
                while (this.m_bThreadFinishJob == false)
                {
                    // проверим, а не попросили ли нас закрыться
                    if (this.m_EventStopThread.WaitOne(iThreadSleepTime, true))
                    {
                        this.m_EventThreadStopped.Set();
                        break;
                    }
                }

            }
            catch (System.Exception e)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Run\n" + e.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            return;
        }
        /// <summary>
        /// Делает пометку о необходимости остановить поток
        /// </summary>
        public void TreadIsFree()
        {
            try
            {
                this.m_bThreadFinishJob = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("StopPleaseTread() " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            return;
        }

        private System.Boolean bIsThreadsActive()
        {
            System.Boolean bRet = false;
            try
            {
                bRet = (
                    ((ThreadAddress != null) && (ThreadAddress.IsAlive == true))
                    );
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("bIsThreadsActive.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return bRet;
        }

        private void CloseThreadInAddressEditor()
        {
            try
            {
                if (bIsThreadsActive() == true)
                {
                    if ((ThreadAddress != null) && (ThreadAddress.IsAlive == true))
                    {
                        EventStopThread.Set();
                    }
                }
                while (bIsThreadsActive() == true)
                {
                    if (System.Threading.WaitHandle.WaitAll((new System.Threading.ManualResetEvent[] { EventThreadStopped }), 100, true))
                    {
                        break;
                    }
                    Application.DoEvents();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("bIsThreadsActive.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        #endregion

        #region Добавить товары в приложение к ПСЦ
        /// <summary>
        /// Добавляет в приложение список товаров
        /// </summary>
        private void LoadPartListToTreeList()
        {
            try
            {
                if (m_objFrmPartsList == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Выполняется запрос списка товаров. Пожалуйста, подождите несколько секунд и повторите попытку.", "Информация",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    return;
                }
                this.Cursor = Cursors.WaitCursor;
                if (m_objFrmPartsList != null)
                {
                    m_objFrmPartsList.SelectProductListByChecked = true;
                    m_objFrmPartsList.EnablDiscountTools();
                    DialogResult dlgRes = m_objFrmPartsList.ShowDialog();
                    if ((dlgRes == DialogResult.OK) && (m_objFrmPartsList.SelectedProductList != null) && (m_objFrmPartsList.SelectedProductList.Count > 0))
                    {
                        this.tableLayoutPanel2.SuspendLayout();
                        this.tableLayoutPanel1.SuspendLayout();
                        ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();
                        m_bDisableEvents = true;

                        CProtocolAgreementPriceItem objItem = null;
                        System.Double PriceWithDiscount = 0;
                        System.Double PriceWithDiscountNDS = 0;
                        System.Double DiscountPercent = m_objFrmPartsList.ProductDetailEditor.SelectedDiscountPercent;
                        foreach (CProduct objProduct in m_objFrmPartsList.SelectedProductList)
                        {
                            objItem = new CProtocolAgreementPriceItem();
                            objItem.Product = objProduct;
                            objItem.Price = objProduct.PriceImporter;
                            objItem.DiscountPercent = DiscountPercent;
                            objItem.PriceWithDiscount = 0;
                            objItem.NDSPercent = objProduct.ProductType.NDSRate; // System.Convert.ToDouble(calcNDS.Value);
                            objItem.PriceWithDiscountNDS = 0;

                            PriceWithDiscount = CProtocolAgreementPriceItem.CalcPriceWithDiscount(objItem.Price, DiscountPercent, false);
                            PriceWithDiscountNDS = CProtocolAgreementPriceItem.AddNDSToPrice(PriceWithDiscount, objItem.NDSPercent);

                            objItem.PriceWithDiscount = PriceWithDiscount;
                            objItem.PriceWithDiscountNDS = PriceWithDiscountNDS;
                            
                            treeList.AppendNode(new object[] { objItem.Product.ProductTradeMarkName, objItem.Product.ProductTypeName, 
                            objItem.Product.ProductSubTypeName, objItem.Product.ProductFullName, true, objItem.Price, 
                            objItem.DiscountPercent, objItem.PriceWithDiscount, objItem.NDSPercent, objItem.PriceWithDiscountNDS, 0 }, null).Tag = objProduct;

                        }
                        objItem = null;

                        m_bDisableEvents = false;
                        this.tableLayoutPanel2.ResumeLayout(false);
                        this.tableLayoutPanel1.ResumeLayout(false);
                        ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();

                    }
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка LoadPartListToTreeList. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                SetPropertiesModified(true);
            }
            return;
        }
        private void btnSetToPartList_Click(object sender, EventArgs e)
        {
            try
            {
                LoadPartListToTreeList();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка btnSetToPartList_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            return;
        }
        #endregion

        #region Удалить товары из приложения к ПСЦ
        /// <summary>
        /// Удаляет выделенные записи из списка
        /// </summary>
        private void DeleteFocusedParts()
        {
            try
            {
                List<DevExpress.XtraTreeList.Nodes.TreeListNode> objNodesListForDelete = new List<DevExpress.XtraTreeList.Nodes.TreeListNode>();
                foreach( DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes )
                {
                    if( objNode.Focused == true )
                    {
                        objNodesListForDelete.Add( objNode );
                    }
                }
                foreach( DevExpress.XtraTreeList.Nodes.TreeListNode objNodeDelete in objNodesListForDelete )
                {
                    treeList.Nodes.Remove(objNodeDelete);
                }
                objNodesListForDelete = null;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("DeleteFocusedParts. Текст ошибки: " + f.Message);
            }
            finally
            {
                SetPropertiesModified(true);
                this.Cursor = Cursors.Default;
            }

            return;
        }
        private void btnDeleteNodes_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteFocusedParts();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnDeleteNodes_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            return;
        }
        /// <summary>
        /// Удаляет все записи из списка
        /// </summary>
        private void DeleteAllParts()
        {
            try
            {
                this.tableLayoutPanel2.SuspendLayout();
                this.tableLayoutPanel1.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();

                treeList.ClearNodes();

                this.tableLayoutPanel2.ResumeLayout(false);
                this.tableLayoutPanel1.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("DeleteAllParts. Текст ошибки: " + f.Message);
            }
            finally
            {
                SetPropertiesModified(true);
                this.Cursor = Cursors.Default;
            }
            return;

        }
        private void mitemsDeleteAllParts_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteAllParts();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("mitemsDeleteAllParts_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            return;
        }
        #endregion

        #region Установить скидку
        /// <summary>
        /// Устанавливает для всех позиций указанную скидку
        /// </summary>
        /// <param name="dblDiscountPercent">размер скидки в процентах</param>
        private void SetDiscountToAllProduct(System.Double dblDiscountPercent)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                this.tableLayoutPanel2.SuspendLayout();
                this.tableLayoutPanel1.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();

                System.Double PriceWithDiscount = 0;
                System.Double PriceWithDiscountNDS = 0;

                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    objNode.SetValue(colDiscountPercent, dblDiscountPercent);

                    PriceWithDiscount = CProtocolAgreementPriceItem.CalcPriceWithDiscount(System.Convert.ToDouble(objNode.GetValue(colPrice)), dblDiscountPercent, false);
                    PriceWithDiscountNDS = CProtocolAgreementPriceItem.AddNDSToPrice(PriceWithDiscount, System.Convert.ToDouble(objNode.GetValue(colNDSPercent)));

                    objNode.SetValue(colPriceWithDiscount, PriceWithDiscount);
                    objNode.SetValue(colPriceWithDiscountNDS, PriceWithDiscountNDS);
                }

                this.tableLayoutPanel2.ResumeLayout(false);
                this.tableLayoutPanel1.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("SetDiscountToAllProduct. Текст ошибки: " + f.Message);
            }
            finally
            {
                SetPropertiesModified(true);
                this.Cursor = Cursors.Default;
            }

            return;
        }
        private void btnSetDiscount_Click(object sender, EventArgs e)
        {
            try
            {
                SetDiscountToAllProduct(System.Convert.ToDouble(calcEditDiscountPercent.Value));
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnSetDiscount_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            return;
        }

        #endregion

        #region Печать
        /// <summary>
        /// Передача данных в MS Excel
        /// </summary>
        private void ExportPSCToExcel()
        {
            Excel.Application oXL = null;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;
            System.Int32 iStartRow = 13;
            System.Int32 iCurrentRow = iStartRow;
            object m = Type.Missing;
            //Excel.Range oRng;

            try
            {
                System.String strFileName = "";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.Refresh();
                    if ((openFileDialog.FileName != "") && (System.IO.File.Exists(openFileDialog.FileName) == true))
                    {
                        strFileName = openFileDialog.FileName;
                    }
                }
                else
                {
                    return;
                }
                SendMessageToLog("Идёт экспорт данных в MS Excel... ");
                this.Cursor = Cursors.WaitCursor;
                oXL = new Excel.Application();
                oWB = (Excel._Workbook)(oXL.Workbooks.Open(strFileName, 0, true, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value));
                oSheet = (Excel._Worksheet)oWB.Worksheets[1];

                System.String strAgreementNum = txtAgreementNum.Text;
                System.String strBeginDate = dtBeginDate.DateTime.ToLongDateString();
                System.String strEndDate = dtEndDate.DateTime.ToLongDateString();

                CCustomer objSelectedCustomer = ((cboxCustomer.SelectedItem == null) ? null : (CCustomer)cboxCustomer.SelectedItem);
                System.String strCustomer = "";
                System.String strCustomerState = "";
                if( objSelectedCustomer != null )
                {
                    strCustomer = objSelectedCustomer.FullName;
                    strCustomerState = ( ( objSelectedCustomer.StateType == null ) ? "" : objSelectedCustomer.StateType.ShortName );
                }
                System.String strCompany = ((cboxCompany.SelectedItem == null) ? "" : ((CCompany)cboxCompany.SelectedItem).Name);
                System.String strCustomerHead = "";
                System.String strCompanyHead = "";

                oSheet.Cells[7, 1] = ("Протокол согласования цен №" + strAgreementNum + " от " + strBeginDate);
                oSheet.Cells[8, 1] = ("между " + strCustomerState + " " + strCustomer + " и " + strCompany);
                oSheet.Cells[10, 2] = ("Действует с " + strBeginDate );// + " по " + strEndDate);
                oSheet.Cells[16, 3] = (strCompany);
                oSheet.Cells[16, 12] = (strCustomerState + " " + strCustomer);
                oSheet.Cells[19, 3] = (strCompanyHead);
                oSheet.Cells[19, 3] = (strCompanyHead);
                oSheet.Cells[19, 12] = (strCustomerHead);

                System.Int32 iRecordNum = 0;
                CProduct objProduct = null;
                System.String strBarcode = "";
                System.String strPartsName = "";
                System.String strOwnerName = "";
                System.String strVtmName = "";
                System.String strCountry = "";
                System.String strCodeTNVD = "";
                System.Int32 iProductQtyInPack = 0;

                foreach ( DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes )
                {
                    iRecordNum++;

                    if (objNode.Tag == null) { continue; }
                    if ( System.Convert.ToBoolean( objNode.GetValue( colProductCheck ) ) == false) { continue; }

                    objProduct = (CProduct)objNode.Tag;
                    strBarcode = objProduct.BarcodeListString2;
                    strPartsName = objProduct.ProductFullName;
                    strOwnerName = objProduct.ProductTradeMarkName;
                    strVtmName = ((objProduct.ProductTradeMark == null) ? "" : objProduct.ProductTradeMark.VtmName);
                    strCountry = objProduct.CountryImportName;
                    strCodeTNVD = objProduct.CodeTNVD;
                    iProductQtyInPack = objProduct.PackQuantity;


                    oSheet.Cells[iCurrentRow, 1] = iRecordNum;
                    oSheet.Cells[iCurrentRow, 2] = strBarcode;
                    oSheet.Cells[iCurrentRow, 3] = strCodeTNVD;
                    oSheet.Cells[iCurrentRow, 4] = strPartsName;
                    oSheet.Cells[iCurrentRow, 5] = strOwnerName;
                    oSheet.Cells[iCurrentRow, 6] = strVtmName;
                    oSheet.Cells[iCurrentRow, 7] = strCountry;
                    oSheet.Cells[iCurrentRow, 8] = System.Convert.ToDouble( objNode.GetValue( colPrice ) );
                    oSheet.Cells[iCurrentRow, 9] = System.Convert.ToDouble(objNode.GetValue(colDiscountPercent));
                    oSheet.Cells[iCurrentRow, 10] = System.Convert.ToDouble(objNode.GetValue(colPriceWithDiscount));
                    oSheet.Cells[iCurrentRow, 11] = System.Convert.ToDouble(objNode.GetValue(colNDSPercent));
                    oSheet.Cells[iCurrentRow, 12] = System.Convert.ToDouble(objNode.GetValue(colPriceWithDiscountNDS));
                    oSheet.Cells[iCurrentRow, 16] = iProductQtyInPack;

                    oSheet.get_Range(oSheet.Cells[iCurrentRow, 1], oSheet.Cells[iCurrentRow, 100]).Copy(Missing.Value);
                    oSheet.get_Range(oSheet.Cells[iCurrentRow, 1], oSheet.Cells[iCurrentRow, 1]).Insert(Excel.XlInsertShiftDirection.xlShiftDown, Missing.Value);
                    
  
                     iCurrentRow++;
                }

                oSheet.get_Range(oSheet.Cells[iCurrentRow, 1], oSheet.Cells[iCurrentRow, 100]).Clear();

                // второй вариант ПСЦ
                oSheet = (Excel._Worksheet)oWB.Worksheets[2];
                oSheet.Cells[7, 1] = ("Протокол согласования цен №" + strAgreementNum + " от " + strBeginDate);
                oSheet.Cells[8, 1] = ("между " + strCustomerState + " " + strCustomer + " и " + strCompany);
                oSheet.Cells[10, 2] = ("Действует с " + strBeginDate ); // + " по " + strEndDate);
                oSheet.Cells[16, 3] = (strCompany);
                oSheet.Cells[16, 7] = (strCustomerState + " " + strCustomer);
                oSheet.Cells[19, 3] = (strCompanyHead);
                oSheet.Cells[19, 3] = (strCompanyHead);
                oSheet.Cells[19, 7] = (strCustomerHead);

                iRecordNum = 0;
                objProduct = null;
                strBarcode = "";
                strPartsName = "";
                strOwnerName = "";
                strVtmName = "";
                strCountry = "";
                strCodeTNVD = "";
                iProductQtyInPack = 0;
                iCurrentRow = iStartRow;
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes)
                {
                    iRecordNum++;

                    if (objNode.Tag == null) { continue; }
                    if (System.Convert.ToBoolean(objNode.GetValue(colProductCheck)) == false) { continue; }

                    objProduct = (CProduct)objNode.Tag;
                    strBarcode = objProduct.BarcodeListString2;
                    strPartsName = objProduct.ProductFullName;
                    strOwnerName = objProduct.ProductTradeMarkName;
                    strVtmName = ((objProduct.ProductTradeMark == null) ? "" : objProduct.ProductTradeMark.VtmName);
                    strCountry = objProduct.CountryImportName;
                    strCodeTNVD = objProduct.CodeTNVD;
                    iProductQtyInPack = objProduct.PackQuantity;


                    oSheet.Cells[iCurrentRow, 1] = iRecordNum;
                    oSheet.Cells[iCurrentRow, 2] = strBarcode;
                    oSheet.Cells[iCurrentRow, 3] = strCodeTNVD;
                    oSheet.Cells[iCurrentRow, 4] = strPartsName;
                    oSheet.Cells[iCurrentRow, 5] = strOwnerName;
                    oSheet.Cells[iCurrentRow, 6] = strVtmName;
               //     oSheet.Cells[iCurrentRow, 6] = strCountry;
                    oSheet.Cells[iCurrentRow, 7] = System.Convert.ToDouble(objNode.GetValue(colPrice));
                    oSheet.Cells[iCurrentRow, 8] = System.Convert.ToDouble(objNode.GetValue(colDiscountPercent));
                    oSheet.Cells[iCurrentRow, 9] = System.Convert.ToDouble(objNode.GetValue(colPriceWithDiscount));
                    oSheet.Cells[iCurrentRow, 10] = System.Convert.ToDouble(objNode.GetValue(colNDSPercent));
                    oSheet.Cells[iCurrentRow, 11] = System.Convert.ToDouble(objNode.GetValue(colPriceWithDiscountNDS));
                    oSheet.Cells[iCurrentRow, 12] = iProductQtyInPack;

                    oSheet.get_Range(oSheet.Cells[iCurrentRow, 1], oSheet.Cells[iCurrentRow, 100]).Copy(Missing.Value);
                    oSheet.get_Range(oSheet.Cells[iCurrentRow, 1], oSheet.Cells[iCurrentRow, 1]).Insert(Excel.XlInsertShiftDirection.xlShiftDown, Missing.Value);


                    iCurrentRow++;
                }


                ((Excel._Worksheet)oWB.Worksheets[1]).Activate();

                //oXL.Run("SumString", Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                //    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                //    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                //    Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                //    Missing.Value, Missing.Value, Missing.Value);

                // oWB.RunAutoMacros(Excel.XlRunAutoMacro.);
                oXL.Visible = true;

            }
            catch (System.Exception f)
            {
                oXL = null;
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка экспорта в MS Excel.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                oSheet = null;
                oWB = null;
                oXL = null;
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                ExportPSCToExcel();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnPrint_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
            return;
        }
        #endregion

        #region Загрузить текущий остаток товара
        /// <summary>
        /// загружает информацию об остатке на складах отгрузки для товара в ПСЦ
        /// </summary>
        private void LoadCuurentStockRest()
        {
            try
            {
                m_bDisableEvents = true;
                if (treeList.Nodes.Count == 0) { return; }

                this.SuspendLayout();

                List<CStockRestInfo> objStockRestInfoList = CStockRestInfo.GetStockRestInfoList(this.m_objProfile, null);

                if ((objStockRestInfoList != null) && (objStockRestInfoList.Count > 0))
                {

                    CProduct objProduct = null;
                    CStockRestInfo objStockRestInfo = null;
                    foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes)
                    {
                        if (objNode.Tag == null) { continue; }

                        objProduct = (CProduct)objNode.Tag;
                        try
                        {
                            objStockRestInfo = objStockRestInfoList.Single<CStockRestInfo>(x => x.uuidPartsID == objProduct.ID);
                        }
                        catch
                        {
                            objStockRestInfo = null;
                        }
                        if ((objStockRestInfo != null) && (System.Convert.ToDouble(objNode.GetValue(colStockRest)) != objStockRestInfo.dblStockRestQuantity))
                        {
                            objNode.SetValue(colStockRest, objStockRestInfo.dblStockRestQuantity);
                        }
                    }
                }

            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка запроса текущего остатка. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.ResumeLayout(false);
                m_bDisableEvents = false;
            }
            return;
        }
        /// <summary>
        /// Удаление позиций с нулевым остатком
        /// </summary>
        private void DeleteRowsWhithNullStockRest()
        {
            try
            {
                m_bDisableEvents = true;
                if (treeList.Nodes.Count == 0) { return; }

                this.SuspendLayout();

                System.Int32 iNodesCount = treeList.Nodes.Count;
                for (System.Int32 i = (iNodesCount - 1); i >= 0; i--)
                {
                    if (System.Convert.ToDouble(treeList.Nodes[i].GetValue(colStockRest)) == 0)
                    {
                        treeList.Nodes.RemoveAt(i);
                    }
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка удаления позиций с нулевым остатком. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.ResumeLayout(false);
                m_bDisableEvents = false;
            }
            return;
        }

        private void mitmsLoadStockRest_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                LoadCuurentStockRest();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("mitmsLoadStockRest_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            return;
        }
        private void mitmsRemoveRowsWithNullStockRest_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                DeleteRowsWhithNullStockRest();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("mitmsRemoveRowsWithNullStockRest_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            return;
        }
        #endregion

        private void treeList_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {

        }
    }

    /// <summary>
    /// Тип, хранящий информацию, которая передается получателям уведомления о событии
    /// </summary>
    public class ChangeProtocolAgreementPricePropertieEventArgs : EventArgs
    {
        private readonly CProtocolAgreementPrice m_objAgreement;
        public CProtocolAgreementPrice Agreement
        { get { return m_objAgreement; } }

        private readonly enumActionSaveCancel m_enActionType;
        public enumActionSaveCancel ActionType
        { get { return m_enActionType; } }

        private readonly System.Boolean m_bIsNewAgreement;
        public System.Boolean IsNewAgreement
        { get { return m_bIsNewAgreement; } }

        public ChangeProtocolAgreementPricePropertieEventArgs(CProtocolAgreementPrice objAgreement, enumActionSaveCancel enActionType, System.Boolean bIsNewAgreement)
        {
            m_objAgreement = objAgreement;
            m_enActionType = enActionType;
            m_bIsNewAgreement = bIsNewAgreement;
        }
    }

}
