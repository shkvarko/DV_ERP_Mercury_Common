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
    public partial class ctrlAgreementWithCustomer : UserControl
    {
        #region Свойства
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;

        private CAgreementWithCustomer m_objSelectedAgreement;
        private CAgreementType m_objAgreementType;

        private System.Boolean m_bIsChanged;

        public ctrlContact frmContact;

        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemContact;

        private System.Boolean m_bDisableEvents;
        private System.Boolean m_bNewObject;
        private System.Boolean m_bIsReadOnly;

        private const System.Int32 iMinControlItemHeight = 20;
        private const System.Int32 iPanel1WidthDef = 350;
        private const System.Int32 iHeightContactListPanel = 120;
        private const System.Int32 iContactListPanelIndex = 6;
        #endregion

        #region События
        // Создаем закрытое поле, ссылающееся на заголовок списка делегатов
        private EventHandler<ChangeAgreementPropertieEventArgs> m_ChangeAgreementProperties;
        // Создаем в классе член-событие
        public event EventHandler<ChangeAgreementPropertieEventArgs> ChangeAgreementProperties
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
        protected virtual void OnChangeAgreementProperties(ChangeAgreementPropertieEventArgs e)
        {
            // Сохраняем поле делегата во временном поле для обеспечение безопасности потока
            EventHandler<ChangeAgreementPropertieEventArgs> temp = m_ChangeAgreementProperties;
            // Если есть зарегистрированные объектв, уведомляем их
            if (temp != null) temp(this, e);
        }
        public void SimulateChangeAgreementProperties(CAgreementWithCustomer objAgreement, enumActionSaveCancel enActionType, System.Boolean bIsNewAgreement)
        {
            // Создаем объект, хранящий информацию, которую нужно передать
            // объектам, получающим уведомление о событии
            ChangeAgreementPropertieEventArgs e = new ChangeAgreementPropertieEventArgs(objAgreement, enActionType, bIsNewAgreement);

            // Вызываем виртуальный метод, уведомляющий наш объект о возникновении события
            // Если нет типа, переопределяющего этот метод, наш объект уведомит все объекты, 
            // подписавшиеся на уведомление о событии
            OnChangeAgreementProperties(e);
        }
        #endregion

        #region Конструктор
        public ctrlAgreementWithCustomer(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem)
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_bIsChanged = false;
            m_bDisableEvents = false;
            m_bNewObject = false;

            m_objSelectedAgreement = null;
            m_objAgreementType = null;
            List<CAgreementType> objAgreementTypeList = CAgreementType.GetAgreementTypeList(m_objProfile, null);
            if (objAgreementTypeList != null)
            {
                m_objAgreementType = objAgreementTypeList.Single<CAgreementType>(x => x.Name == ERP_Mercury.Global.Consts.strAgreementWithCustomerTypeName);
                objAgreementTypeList = null;
            }

            frmContact = new ERP_Mercury.Common.ctrlContact(ERP_Mercury.Common.EnumObject.Customer, m_objProfile, m_objMenuItem, System.Guid.Empty);

            //layoutControlContact.Size = new Size(layoutControlContact.Size.Width, (frmContact.Size.Height));
            //layoutControlContact.MaximumSize = new Size(layoutControlContact.Size.Width, layoutControlContact.Size.Height);

            //layoutControlGroupConact.Size = new Size(layoutControlGroupConact.Size.Width, (frmContact.Size.Height));

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


            frmContact.ChangeContactProperties += OnChangeContactPropertie;

            layoutControlGroupConact.Expanded = true;

            LoadComboBoxItems();
            m_bIsReadOnly = false;

            //CheckClientsRight();
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

        #region Индикация изменений
        private void SetPropertiesModified(System.Boolean bModified)
        {
            try
            {
                m_bIsChanged = bModified;
                btnSave.Enabled = ( m_bIsChanged && ( ValidateProperties() == true ) );
                //btnCancel.Enabled = btnSave.Enabled;
                if (m_bIsChanged == true)
                {
                    SimulateChangeAgreementProperties(m_objSelectedAgreement, enumActionSaveCancel.Unkown, m_bNewObject);
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

                if ((sender == cboxNum) || (sender == cboxCategory)) { SetArgeementNum(); }

                if( (sender == cboxCustomer) && ( cboxCustomer.SelectedItem != null ) && ( m_bNewObject == true ))
                {
                    if (treeListConacts.Nodes.Count > 0) { treeListConacts.Nodes.Clear(); }
                    List<CContact> objContactList = CContact.GetContactList(m_objProfile, null, EnumObject.Customer, ((CCustomer)cboxCustomer.SelectedItem).ID);
                    if (objContactList != null)
                    {
                        foreach (CContact objContact in objContactList)
                        {
                            treeListConacts.AppendNode(new object[] { objContact.VisitingCard2 }, null).Tag = objContact;
                        }
                    }
                    objContactList = null;

                    if( treeListConacts.Nodes.Count > 0 )
                    {
                        treeListConacts.FocusedNode = treeListConacts.Nodes[0];
                        AddNewContact();
                    }
                }
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

                    if ((sender == txtExtraNum) || (sender == txtSubNum)) { SetArgeementNum(); }
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
                SendMessageToLog("OnChangeAddressPropertie.  Текст ошибки: " + f.Message);
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
                SendMessageToLog("OnChangeContactPropertie. Текст ошибки: " + f.Message);
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

                // Основания действия
                cboxBasement.Properties.Items.Clear();
                cboxBasement.Properties.Items.AddRange( CAgrementBasement.GetAgrementBasementList(m_objProfile, null));

                // Цель приобретения
                cboxReason.Properties.Items.Clear();
                cboxReason.Properties.Items.AddRange( CAgrementReason.GetAgrementReasonList( m_objProfile, null ) );

                // Условия доставки
                cboxDeliveryCondition.Properties.Items.Clear();
                cboxDeliveryCondition.Properties.Items.AddRange( CAgreementDeliveryCondition.GetAgreementDeliveryConditionList( m_objProfile, null ) );

                // Условия оплаты
                cboxPaymentCondition.Properties.Items.Clear();
                cboxPaymentCondition.Properties.Items.AddRange( CAgreementPaymentCondition.GetAgreementPaymentConditionList( m_objProfile, null ) );

                // Состояние
                cboxAgreementState.Properties.Items.Clear();
                cboxAgreementState.Properties.Items.AddRange( CAgreementState.GetAgreementStateList( m_objProfile, null ) );
                
                // Нумерация договора
                cboxNum.Properties.Items.Clear();
                for (System.Int32 i = 0; i < cboxCompany.Properties.Items.Count; i++)
                {
                    cboxNum.Properties.Items.Add(((CCompany)cboxCompany.Properties.Items[i]).Abbr);
                }
                cboxCategory.Properties.Items.Clear();
                string[] strCategoryList = CAgreementWithCustomer.SrcCategoryList;
                foreach (System.String strItem in strCategoryList)
                {
                    cboxCategory.Properties.Items.Add( strItem );
                }

                if (frmContact != null) { frmContact.LoadComboBoxItems(); }

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
                cboxBasement.Properties.ReadOnly = bSet;
                cboxCompany.Properties.ReadOnly = bSet;
                cboxCustomer.Properties.ReadOnly = bSet;
                cboxDeliveryCondition.Properties.ReadOnly = bSet;
                cboxPaymentCondition.Properties.ReadOnly = bSet;
                cboxReason.Properties.ReadOnly = bSet;
                calcDaysPaymentDelay.Properties.ReadOnly = bSet;

                dtBeginDate.Properties.ReadOnly = bSet;
                dtEndDate.Properties.ReadOnly = bSet;
                cboxNum.Properties.ReadOnly = bSet;
                cboxCategory.Properties.ReadOnly = bSet;
                txtExtraNum.Properties.ReadOnly = bSet;
                txtSubNum.Properties.ReadOnly = bSet;

                frmContact.SetChanceEditProperties(!bSet);

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
                frmContact.frmAddress.StartThreadWithLoadData();

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

        #region Редактировать договор
        /// <summary>
        /// Проверяет содержимое элементов управления
        /// </summary>
        private System.Boolean ValidateProperties()
        {
            System.Boolean bRet = true;
            try
            {
                cboxAgreementState.Properties.Appearance.BackColor = ((cboxAgreementState.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                cboxBasement.Properties.Appearance.BackColor = ((cboxBasement.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                cboxCompany.Properties.Appearance.BackColor = ((cboxCompany.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                cboxCustomer.Properties.Appearance.BackColor = ((cboxCustomer.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                cboxDeliveryCondition.Properties.Appearance.BackColor = ((cboxDeliveryCondition.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                cboxPaymentCondition.Properties.Appearance.BackColor = ((cboxPaymentCondition.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                cboxReason.Properties.Appearance.BackColor = ((cboxReason.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);

                txtAgreementNum.Properties.Appearance.BackColor = ((txtAgreementNum.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                dtBeginDate.Properties.Appearance.BackColor = ((dtBeginDate.DateTime == System.DateTime.MinValue) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                dtEndDate.Properties.Appearance.BackColor = ((dtEndDate.DateTime == System.DateTime.MinValue) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);

                if (cboxAgreementState.SelectedItem == null) { bRet = false; }
                if (cboxBasement.SelectedItem == null) { bRet = false; }
                if (cboxCompany.SelectedItem == null) { bRet = false; }
                if (cboxCustomer.SelectedItem == null) { bRet = false; }
                if (cboxDeliveryCondition.SelectedItem == null) { bRet = false; }
                if (cboxPaymentCondition.SelectedItem == null) { bRet = false; }
                if (cboxReason.SelectedItem == null) { bRet = false; }
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
                cboxBasement.SelectedItem = null;
                cboxCompany.SelectedItem = null;
                cboxCustomer.SelectedItem = null;
                cboxDeliveryCondition.SelectedItem = null;
                cboxPaymentCondition.SelectedItem = null;
                cboxReason.SelectedItem = null;
                calcDaysPaymentDelay.Value = 0;
                cboxNum.SelectedItem = null;
                cboxCategory.SelectedItem = null;
                txtSubNum.Text = "";
                txtExtraNum.Text = "";
                treeListConacts.Nodes.Clear();
                tableLayoutPanel3.RowStyles[iContactListPanelIndex].Height = ((m_bNewObject == true) ? iHeightContactListPanel : 0);
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
        /// Загружает свойства договора для редактирования
        /// </summary>
        /// <param name="objAgreement">клиент</param>
        public void EditAgreement( ERP_Mercury.Common.CAgreementWithCustomer objAgreement )
        {
            if (objAgreement == null) { return; }
            m_bDisableEvents = true;
            m_bNewObject = false;
            ShowWarningPnl(false);
            try
            {
                m_objSelectedAgreement = objAgreement;

                this.SuspendLayout();

                ClearControls();

                txtAgreementNum.Text = m_objSelectedAgreement.Agreement.Name;
                txtDescription.Text = m_objSelectedAgreement.Agreement.Description;
                dtBeginDate.DateTime = m_objSelectedAgreement.Agreement.BeginDate;
                dtEndDate.DateTime = m_objSelectedAgreement.Agreement.EndDate;
                calcDaysPaymentDelay.Value = m_objSelectedAgreement.DaysPaymentDelay;

                cboxAgreementState.SelectedItem = (m_objSelectedAgreement.Agreement.AgreementState == null) ? null : cboxAgreementState.Properties.Items.Cast<CAgreementState>().Single<CAgreementState>(x => x.ID.CompareTo(m_objSelectedAgreement.Agreement.AgreementState.ID) == 0);
                cboxBasement.SelectedItem = (m_objSelectedAgreement.Basement == null) ? null : cboxBasement.Properties.Items.Cast<CAgrementBasement>().Single<CAgrementBasement>(x => x.ID.CompareTo(m_objSelectedAgreement.Basement.ID) == 0);
                cboxCompany.SelectedItem = (m_objSelectedAgreement.Company == null) ? null : cboxCompany.Properties.Items.Cast<CCompany>().Single<CCompany>(x => x.ID.CompareTo(m_objSelectedAgreement.Company.ID) == 0);
                cboxCustomer.SelectedItem = (m_objSelectedAgreement.Customer == null) ? null : cboxCustomer.Properties.Items.Cast<CCustomer>().Single<CCustomer>(x => x.ID.CompareTo(m_objSelectedAgreement.Customer.ID) == 0);
                cboxDeliveryCondition.SelectedItem = (m_objSelectedAgreement.DeliveryCondition == null) ? null : cboxDeliveryCondition.Properties.Items.Cast<CAgreementDeliveryCondition>().Single<CAgreementDeliveryCondition>(x => x.ID.CompareTo(m_objSelectedAgreement.DeliveryCondition.ID) == 0);
                cboxPaymentCondition.SelectedItem = (m_objSelectedAgreement.PaymentCondition == null) ? null : cboxPaymentCondition.Properties.Items.Cast<CAgreementPaymentCondition>().Single<CAgreementPaymentCondition>(x => x.ID.CompareTo(m_objSelectedAgreement.PaymentCondition.ID) == 0);
                cboxReason.SelectedItem = (m_objSelectedAgreement.Reason == null) ? null : cboxReason.Properties.Items.Cast<CAgrementReason>().Single<CAgrementReason>(x => x.ID.CompareTo(m_objSelectedAgreement.Reason.ID) == 0);

                if (m_objSelectedAgreement.AgreementNumCompany != "")
                {
                    for (System.Int32 i = 0; i < cboxNum.Properties.Items.Count; i++)
                    {
                        if (((System.String)cboxNum.Properties.Items[i]) == m_objSelectedAgreement.AgreementNumCompany)
                        {
                            cboxNum.SelectedIndex = i;
                            break;
                        }
                    }
                }
                if (m_objSelectedAgreement.AgreementNumCategory != "")
                {
                    for (System.Int32 i = 0; i < cboxCategory.Properties.Items.Count; i++)
                    {
                        if (((System.String)cboxCategory.Properties.Items[i]) == m_objSelectedAgreement.AgreementNumCategory)
                        {
                            cboxCategory.SelectedIndex = i;
                            break;
                        }
                    }
                }
                txtSubNum.Text = m_objSelectedAgreement.AgreementNumID;
                txtExtraNum.Text = m_objSelectedAgreement.AgreementNumYear;

                // Контакты
                if (m_objSelectedAgreement.Contact != null)
                {
                    List<CContact> objContactList = new List<CContact>();
                    objContactList.Add(m_objSelectedAgreement.Contact);
                    frmContact.LoadContactList(m_objSelectedAgreement.ID, objContactList);
                }
                frmContact.ClearAllPropertiesControls();

                SetPropertiesModified(false);
                //tabControl.SelectedTabPage = tabGeneral;
                btnCancel.Enabled = true;
                btnCancel.Focus();

                SetModeReadOnly(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования договора с клиентом. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.ResumeLayout(false);
                //tabControl.SelectedTabPage = tabGeneral;
                m_bDisableEvents = false;
                btnEdit.Enabled = (m_objProfile.GetClientsRight().GetState(ERP_Mercury.Global.Consts.strDR_EditAgreementWithCustomerCard) == true);
            }
            return;
        }
        /// <summary>
        /// Формирует итоговый номер договора
        /// </summary>
        private void SetArgeementNum()
        {
            try
            {
                System.String strCompany = ( ( cboxNum.SelectedItem == null ) ? "" : cboxNum.Text );
                System.String strCategory = (cboxCategory.SelectedItem == null) ? "" : cboxCategory.Text;
                System.String strSubNum = txtSubNum.Text;
                System.String strExtraNum = txtExtraNum.Text;

                txtAgreementNum.Text = CAgreementWithCustomer.GenerateAgreementNum(strCompany, strCategory, strSubNum, strExtraNum);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка формирования номера договора с клиентом. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        #endregion

        #region Новый договор с клиентом
        /// <summary>
        /// Новый договор с клиентом
        /// </summary>
        public void NewAgreement()
        {
            Cursor = Cursors.WaitCursor;
            m_bDisableEvents = true;
            m_bNewObject = true;

            //SetWarningInfo("идёт загрузка данных...");
            //ShowWarningPnl(true);

            ShowWarningPnl(false);
            try
            {


                if (frmContact.frmAddress.AllListsIsLoad == false)
                {
                    System.String strWait = "идёт загрузка данных...";

                    txtAgreementNum.Text = "";
                    txtDescription.Text = "";
                    cboxAgreementState.Text = strWait;
                    cboxBasement.Text = strWait;
                    cboxCompany.Text = strWait;
                    cboxCustomer.Text = strWait;
                    cboxDeliveryCondition.Text = strWait;
                    cboxPaymentCondition.Text = strWait;
                    cboxReason.Text = strWait;
                    calcDaysPaymentDelay.Value = 0;
                    cboxCategory.Text = strWait;

                    this.Refresh();
                    frmContact.frmAddress.StartThreadWithLoadData();
                }

                m_objSelectedAgreement = new CAgreementWithCustomer();
                m_objSelectedAgreement.AgreementType = m_objAgreementType;
                m_objSelectedAgreement.Agreement = new CAgreement();
                m_objSelectedAgreement.Agreement.BeginDate = System.DateTime.Today;
                m_objSelectedAgreement.Agreement.EndDate = new DateTime( System.DateTime.Today.Year, 12, 31 );

       //         this.SuspendLayout();


                // Контакты
                frmContact.ClearAllPropertiesControls();
                frmContact.ClearContactListTree();

                ClearControls();

                txtAgreementNum.Text = m_objSelectedAgreement.Agreement.Name;
                txtDescription.Text = m_objSelectedAgreement.Agreement.Description;
                dtBeginDate.DateTime = m_objSelectedAgreement.Agreement.BeginDate;
                dtEndDate.DateTime = m_objSelectedAgreement.Agreement.EndDate;
                calcDaysPaymentDelay.Value = m_objSelectedAgreement.DaysPaymentDelay;
                txtExtraNum.Text = System.DateTime.Today.Year.ToString().Substring(2, 2);

                SetModeReadOnly(false);
                btnEdit.Enabled = false;
                SetPropertiesModified(true);

            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка создания договора с клиентом. Текст ошибки: " + f.Message);
            }
            finally
            {
          //      this.ResumeLayout(false);
                m_bDisableEvents = false;
                Cursor = Cursors.Default;
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
                SimulateChangeAgreementProperties(m_objSelectedAgreement, enumActionSaveCancel.Cancel, m_bNewObject);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка отмены изменений в описании договора с клиентом. Текст ошибки: " + f.Message);
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
            CAgreementWithCustomer objAgreementForSave = new CAgreementWithCustomer();
            try
            {
                objAgreementForSave.ID = m_objSelectedAgreement.ID;
                objAgreementForSave.uuiID = m_objSelectedAgreement.uuiID;
                objAgreementForSave.StmntID = m_objSelectedAgreement.StmntID;
                objAgreementForSave.Agreement = m_objSelectedAgreement.Agreement;
                objAgreementForSave.Agreement.BeginDate = dtBeginDate.DateTime;
                objAgreementForSave.Agreement.EndDate = dtEndDate.DateTime;
                objAgreementForSave.Agreement.AgreementType = m_objAgreementType;
                objAgreementForSave.Agreement.Description = txtDescription.Text;
                objAgreementForSave.Agreement.Name = txtAgreementNum.Text;
                objAgreementForSave.Agreement.AgreementState = ((cboxAgreementState.SelectedItem == null) ? null : (CAgreementState)cboxAgreementState.SelectedItem);
                objAgreementForSave.Basement = ((cboxBasement.SelectedItem == null) ? null : (CAgrementBasement)cboxBasement.SelectedItem);
                objAgreementForSave.Reason = ((cboxReason.SelectedItem == null) ? null : (CAgrementReason)cboxReason.SelectedItem);
                objAgreementForSave.Customer = ((cboxCustomer.SelectedItem == null) ? null : (CCustomer)cboxCustomer.SelectedItem);
                objAgreementForSave.Company = ((cboxCompany.SelectedItem == null) ? null : (CCompany)cboxCompany.SelectedItem);
                objAgreementForSave.Contact = (((frmContact.ContactList == null) || (frmContact.ContactList.Count == 0)) ? null : frmContact.ContactList[0]);
                objAgreementForSave.DeliveryCondition = ((cboxDeliveryCondition.SelectedItem == null) ? null : (CAgreementDeliveryCondition)cboxDeliveryCondition.SelectedItem);
                objAgreementForSave.PaymentCondition = ((cboxPaymentCondition.SelectedItem == null) ? null : (CAgreementPaymentCondition)cboxPaymentCondition.SelectedItem);
                objAgreementForSave.DaysPaymentDelay = System.Convert.ToInt32(calcDaysPaymentDelay.Value);
                System.String strErr = "";
                if (m_bNewObject == true)
                {
                    // новый клиент
                    if (objAgreementForSave.AddAgreementWithCustomerIB(m_objProfile, null, ref strErr) == true)
                    {
                        bOkSave = objAgreementForSave.AddAgreementWithCustomer(m_objProfile, null, ref strErr);
                    }
                }
                else
                {
                    if (objAgreementForSave.EditAgreementWithCustomerIB(m_objProfile, null, ref strErr) == true)
                    {
                        bOkSave = objAgreementForSave.EditAgreementWithCustomer(m_objProfile, null, ref strErr);
                    }
                }
                SendMessageToLog(strErr);
                if (bOkSave == true)
                {
                    m_objSelectedAgreement.ID = objAgreementForSave.ID;
                    m_objSelectedAgreement.Agreement = objAgreementForSave.Agreement;
                    m_objSelectedAgreement.Agreement.BeginDate = objAgreementForSave.Agreement.BeginDate;
                    m_objSelectedAgreement.Agreement.EndDate = objAgreementForSave.Agreement.EndDate;
                    m_objSelectedAgreement.Agreement.AgreementType = objAgreementForSave.Agreement.AgreementType;
                    m_objSelectedAgreement.Agreement.Description = objAgreementForSave.Agreement.Description;
                    m_objSelectedAgreement.Agreement.Name = objAgreementForSave.Agreement.Name;
                    m_objSelectedAgreement.Agreement.AgreementState = objAgreementForSave.Agreement.AgreementState;
                    m_objSelectedAgreement.Basement = objAgreementForSave.Basement;
                    m_objSelectedAgreement.Reason = objAgreementForSave.Reason;
                    m_objSelectedAgreement.Customer = objAgreementForSave.Customer;
                    m_objSelectedAgreement.Company = objAgreementForSave.Company;
                    m_objSelectedAgreement.Contact = objAgreementForSave.Contact;
                    m_objSelectedAgreement.DeliveryCondition = objAgreementForSave.DeliveryCondition;
                    m_objSelectedAgreement.PaymentCondition = objAgreementForSave.PaymentCondition;
                    m_objSelectedAgreement.DaysPaymentDelay = objAgreementForSave.DaysPaymentDelay;

                    if (frmContact.ContactDeletedList != null) { frmContact.ContactDeletedList.Clear(); }

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
                SendMessageToLog("Ошибка сохранения изменений в описании договора с клиентом. Текст ошибки: " + f.Message);
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
                if ((frmContact.ContactList == null) || (frmContact.ContactList.Count == 0))
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Необходимо указать, в лице кого заключён договор.", "Информация",
                       System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

                    return;
                }
                frmContact.ConfirmChanges();
                if (bSaveChanges() == true)
                {
                    SimulateChangeAgreementProperties(m_objSelectedAgreement, enumActionSaveCancel.Save, m_bNewObject);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в описании договора с клиентом. Текст ошибки: " + f.Message);
            }
            return;
        }

        #endregion

        #region Печать данных договора
        //private void ExportToExcel(CCustomer objCustomer)
        //{
        //    Excel.Application oXL = null;
        //    Excel._Workbook oWB;
        //    Excel._Worksheet oSheet;
        //    //Excel.Range oRng;
        //    System.Int32 iLastIndxRowForPrint = 0;

        //    try
        //    {
        //        this.Cursor = Cursors.WaitCursor;
        //        //Start Excel and get Application object.
        //        oXL = new Excel.Application();
        //        //oXL.Visible = true;

        //        //Get a new workbook.
        //        oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
        //        //oSheet = (Excel._Worksheet)oWB.ActiveSheet;
        //        oSheet = (Excel._Worksheet)oWB.Worksheets[1];
        //        System.Int32 iSheetsCount = oWB.Worksheets.Count;

        //        for (System.Int32 i = iSheetsCount; i > 1; i--)
        //        {
        //            ((Excel._Worksheet)oWB.Worksheets[i]).Delete();
        //        }

        //        //Клиент
        //        oSheet.Name = "Клиент";
        //        oSheet.Cells[2, 1] = "Код клиента"; // lblInfo.Text.Replace('\t', ' ');
        //        oSheet.Cells[2, 2] = objCustomer.InterBaseID;
        //        oSheet.Cells[3, 1] = "Наименование клиента";
        //        oSheet.Cells[3, 2] = objCustomer.FullName;
        //        oSheet.Cells[4, 1] = "Форма собственности";
        //        oSheet.Cells[4, 2] = (objCustomer.StateType == null) ? "" : objCustomer.StateType.ShortName;
        //        oSheet.Cells[5, 1] = "Сетевой клиент";
        //        oSheet.Cells[5, 2] = (objCustomer.DistributionNetwork == null) ? "" : objCustomer.DistributionNetwork.Name;
        //        oSheet.Cells[6, 1] = "УНП";
        //        oSheet.Cells[6, 2] = objCustomer.UNP;
        //        oSheet.Cells[7, 1] = "ОКПО";
        //        oSheet.Cells[7, 2] = objCustomer.OKPO;
        //        oSheet.Cells[8, 1] = "Юр. Адрес";
        //        oSheet.Cells[8, 2] = ((frmAddress.AddressList == null) || (frmAddress.AddressList.Count == 0)) ? "" : frmAddress.AddressList[0].VisitingCard2;
        //        oSheet.Cells[9, 1] = "Лицензии";
        //        iLastIndxRowForPrint = 10;
        //        if ((objCustomer.LicenceList != null) && (objCustomer.LicenceList.Count > 0))
        //        {
        //            foreach (CLicence objLicence in objCustomer.LicenceList)
        //            {
        //                oSheet.Cells[iLastIndxRowForPrint, 2] = "№" + objLicence.LicenceNum + " " + objLicence.LicenceType.Name + " " + objLicence.BeginDate.ToShortDateString() + " - " + objLicence.EndDate.ToShortDateString() + " выдана " + objLicence.WhoGive;
        //                iLastIndxRowForPrint++;
        //            }
        //        }
        //        oSheet.Cells[iLastIndxRowForPrint, 1] = "Расчетные счета";
        //        iLastIndxRowForPrint++;
        //        if ((objCustomer.AccountList != null) && (objCustomer.AccountList.Count > 0))
        //        {
        //            foreach (CAccount objAccount in objCustomer.AccountList)
        //            {
        //                oSheet.Cells[iLastIndxRowForPrint, 2] = "№" + objAccount.AccountNumber + " " + objAccount.Currency.CurrencyAbbr + " " + objAccount.Bank.Code + " " + objAccount.Bank.Name;
        //                iLastIndxRowForPrint++;
        //            }
        //        }
        //        oSheet.Cells[iLastIndxRowForPrint, 1] = "Контакты";
        //        iLastIndxRowForPrint++;
        //        if ((frmContact.ContactList != null) && (frmContact.ContactList.Count > 0))
        //        {
        //            foreach (CContact objContact in frmContact.ContactList)
        //            {
        //                oSheet.Cells[iLastIndxRowForPrint, 2] = objContact.VisitingCard2;
        //                iLastIndxRowForPrint++;
        //            }
        //        }
        //        oSheet.get_Range(oSheet.Cells[1, 1], oSheet.Cells[iLastIndxRowForPrint, 1]).Font.Bold = true;
        //        oSheet.get_Range(oSheet.Cells[1, 1], oSheet.Cells[iLastIndxRowForPrint, 1]).Font.Size = 12;
        //        oSheet.get_Range("A1", "A1").EntireColumn.AutoFit();
        //        oSheet.get_Range("B1", "B1").EntireColumn.AutoFit();
        //        oSheet.get_Range(oSheet.Cells[1, 2], oSheet.Cells[iLastIndxRowForPrint, 2]).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

        //        //РТТ
        //        if ((frmRtt.RttList != null) && (frmRtt.RttList.Count > 0))
        //        {
        //            foreach (CRtt objRtt in frmRtt.RttList)
        //            {
        //                oSheet = (Excel._Worksheet)oWB.Worksheets.Add(Missing.Value, Missing.Value, 1, Excel.XlSheetType.xlWorksheet);
        //                oSheet.Name = objRtt.Code;
        //                oSheet.Move(Missing.Value, (Excel._Worksheet)oWB.Worksheets[oWB.Worksheets.Count]);

        //                oSheet.Cells[2, 1] = "Код РТТ"; // lblInfo.Text.Replace('\t', ' ');
        //                oSheet.Cells[2, 2] = objRtt.Code;
        //                oSheet.Cells[3, 1] = "Наименование РТТ";
        //                oSheet.Cells[3, 2] = objRtt.FullName;
        //                oSheet.Cells[4, 1] = "Тип лицензии";
        //                oSheet.Cells[4, 2] = (objRtt.LicenceType == null) ? "" : objRtt.LicenceType.Name;
        //                oSheet.Cells[5, 1] = "Признак активности";
        //                oSheet.Cells[5, 2] = (objRtt.RttActiveType == null) ? "" : objRtt.RttActiveType.Name;
        //                oSheet.Cells[6, 1] = "Спецкод";
        //                oSheet.Cells[6, 2] = (objRtt.RttSpecCode == null) ? "" : objRtt.RttSpecCode.Name;
        //                oSheet.Cells[7, 1] = "Адрес";
        //                oSheet.Cells[7, 2] = ((frmAddress.AddressList == null) || (frmAddress.AddressList.Count == 0)) ? "" : frmAddress.AddressList[0].VisitingCard2;
        //                oSheet.Cells[8, 1] = "Режим работы";
        //                oSheet.Cells[8, 2] = (objRtt.ActionPeriod == null) ? "" : objRtt.ActionPeriod.ShortShedule;
        //                oSheet.Cells[9, 1] = "Сегментация";
        //                oSheet.Cells[9, 2] = (objRtt.Segmentation == null) ? "" : objRtt.Segmentation.Code;
        //                oSheet.Cells[10, 1] = "Оборудование";
        //                iLastIndxRowForPrint = 11;
        //                if ((objRtt.TradeEquipmentList != null) && (objRtt.TradeEquipmentList.Count > 0))
        //                {
        //                    foreach (CTradeEquipment objTradeEquipment in objRtt.TradeEquipmentList)
        //                    {
        //                        oSheet.Cells[iLastIndxRowForPrint, 2] = objTradeEquipment.EquipmentType.ProductCatalogName;
        //                        oSheet.Cells[iLastIndxRowForPrint, 3] = objTradeEquipment.EquipmentType.Code;
        //                        oSheet.Cells[iLastIndxRowForPrint, 4] = ((objTradeEquipment.EquipmentType == null) ? "" : objTradeEquipment.EquipmentType.Name);
        //                        oSheet.Cells[iLastIndxRowForPrint, 5] = ((objTradeEquipment.Availability == null) ? "" : objTradeEquipment.Availability.Name);
        //                        oSheet.Cells[iLastIndxRowForPrint, 6] = objTradeEquipment.EquipmentType.SizeName;
        //                        oSheet.Cells[iLastIndxRowForPrint, 7] = objTradeEquipment.Quantity;
        //                        iLastIndxRowForPrint++;
        //                    }
        //                }
        //                oSheet.Cells[iLastIndxRowForPrint, 1] = "Присутствие ТМ";
        //                iLastIndxRowForPrint++;
        //                if ((objRtt.ProductCatalogList != null) && (objRtt.ProductCatalogList.Count > 0))
        //                {
        //                    foreach (CProductCatalog objProductCatalog in objRtt.ProductCatalogList)
        //                    {
        //                        oSheet.Cells[iLastIndxRowForPrint, 2] = objProductCatalog.Name;
        //                        iLastIndxRowForPrint++;
        //                    }
        //                }
        //                oSheet.Cells[iLastIndxRowForPrint, 1] = "Контакты";
        //                iLastIndxRowForPrint++;
        //                if ((objRtt.ContactList != null) && (objRtt.ContactList.Count > 0))
        //                {
        //                    foreach (CContact objContact in objRtt.ContactList)
        //                    {
        //                        oSheet.Cells[iLastIndxRowForPrint, 2] = objContact.VisitingCard2;
        //                        iLastIndxRowForPrint++;
        //                    }
        //                }

        //                oSheet.get_Range(oSheet.Cells[1, 1], oSheet.Cells[iLastIndxRowForPrint, 1]).Font.Bold = true;
        //                oSheet.get_Range(oSheet.Cells[1, 1], oSheet.Cells[iLastIndxRowForPrint, 1]).Font.Size = 12;
        //                oSheet.get_Range("A1", "A1").EntireColumn.AutoFit();
        //                oSheet.get_Range(oSheet.Cells[1, 2], oSheet.Cells[iLastIndxRowForPrint, 2]).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
        //            }
        //        }
        //        ((Excel._Worksheet)oWB.Worksheets[1]).Activate();

        //        oXL.Visible = true;
        //        oXL.UserControl = true;
        //    }
        //    catch (System.Exception f)
        //    {
        //        if (oXL != null) { oXL.Quit(); }
        //        oXL = null;
        //        DevExpress.XtraEditors.XtraMessageBox.Show(
        //            "Ошибка экспорта в MS Excel.\n\nТекст ошибки: " + f.Message, "Ошибка",
        //           System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        //    }
        //    finally
        //    {
        //        this.Cursor = System.Windows.Forms.Cursors.Default;
        //    }
        //}
        #endregion

        #region Load
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
        private void ctrlAgreement_Resize(object sender, EventArgs e)
        {
            layoutControlContact.MaximumSize = new Size(this.Size.Width - iMinControlItemHeight, layoutControlContact.MaximumSize.Height);
        }
        #endregion


        #region Автоматическое добавление записи в раздел "В лице кого заключён договор"

        private void AddNewContact()
        {
            try
            {
                if (cboxCustomer.SelectedItem == null) { return; }
                if (treeListConacts.Nodes.Count == 0) { return; }
                if ((treeListConacts.FocusedNode == null) || (treeListConacts.FocusedNode.Tag == null)) { return; }

                frmContact.NewContact((CContact)treeListConacts.FocusedNode.Tag);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListConacts_MouseDoubleClick. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        private void treeListConacts_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            AddNewContact();
        }
        #endregion
    }

    /// <summary>
    /// Тип, хранящий информацию, которая передается получателям уведомления о событии
    /// </summary>
    public class ChangeAgreementPropertieEventArgs : EventArgs
    {
        private readonly CAgreementWithCustomer m_objAgreement;
        public CAgreementWithCustomer Agreement
        { get { return m_objAgreement; } }

        private readonly enumActionSaveCancel m_enActionType;
        public enumActionSaveCancel ActionType
        { get { return m_enActionType; } }

        private readonly System.Boolean m_bIsNewAgreement;
        public System.Boolean IsNewAgreement
        { get { return m_bIsNewAgreement; } }

        public ChangeAgreementPropertieEventArgs(CAgreementWithCustomer objAgreement, enumActionSaveCancel enActionType, System.Boolean bIsNewAgreement)
        {
            m_objAgreement = objAgreement;
            m_enActionType = enActionType;
            m_bIsNewAgreement = bIsNewAgreement;
        }
    }

}
