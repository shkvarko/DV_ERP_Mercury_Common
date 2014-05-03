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
    public partial class ctrlAgreementWithCarrier : UserControl
    {
        #region Свойства
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;

        private CAgreementWithCarrier m_objSelectedAgreement;
        private CAgreementType m_objAgreementType;

        private System.Boolean m_bIsChanged;

        private System.Boolean m_bDisableEvents;
        private System.Boolean m_bNewObject;
        private System.Boolean m_bIsReadOnly;

        private const System.Int32 iMinControlItemHeight = 20;
        private const System.Int32 iPanel1WidthDef = 350;
        #endregion

        #region События
        // Создаем закрытое поле, ссылающееся на заголовок списка делегатов
        private EventHandler<ChangeAgreementWithCarrierPropertieEventArgs> m_ChangeAgreementProperties;
        // Создаем в классе член-событие
        public event EventHandler<ChangeAgreementWithCarrierPropertieEventArgs> ChangeAgreementProperties
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
        protected virtual void OnChangeAgreementWithCarrierProperties(ChangeAgreementWithCarrierPropertieEventArgs e)
        {
            // Сохраняем поле делегата во временном поле для обеспечение безопасности потока
            EventHandler<ChangeAgreementWithCarrierPropertieEventArgs> temp = m_ChangeAgreementProperties;
            // Если есть зарегистрированные объектв, уведомляем их
            if (temp != null) temp(this, e);
        }
        public void SimulateChangeAgreementWithCarrierProperties(CAgreementWithCarrier objAgreement, enumActionSaveCancel enActionType, System.Boolean bIsNewAgreement)
        {
            // Создаем объект, хранящий информацию, которую нужно передать
            // объектам, получающим уведомление о событии
            ChangeAgreementWithCarrierPropertieEventArgs e = new ChangeAgreementWithCarrierPropertieEventArgs(objAgreement, enActionType, bIsNewAgreement);

            // Вызываем виртуальный метод, уведомляющий наш объект о возникновении события
            // Если нет типа, переопределяющего этот метод, наш объект уведомит все объекты, 
            // подписавшиеся на уведомление о событии
            OnChangeAgreementWithCarrierProperties(e);
        }
        #endregion

        #region Конструктор
        public ctrlAgreementWithCarrier(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem)
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

            LoadComboBoxItems();
            m_bIsReadOnly = false;
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
                btnSave.Enabled = (m_bIsChanged && (ValidateProperties() == true));
                btnCancel.Enabled = true;
                if (m_bIsChanged == true)
                {
                    SimulateChangeAgreementWithCarrierProperties(m_objSelectedAgreement, enumActionSaveCancel.Unkown, m_bNewObject);
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
                cboxCustomer.Properties.Items.AddRange(CCarrier.GetCarrierList(m_objProfile, null));

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
        public void EditAgreement(ERP_Mercury.Common.CAgreementWithCarrier objAgreement)
        {
            if (objAgreement == null) { return; }
            m_bDisableEvents = true;
            m_bNewObject = false;
            ShowWarningPnl(false);
            try
            {
                m_objSelectedAgreement = objAgreement;

                //this.SuspendLayout();

                ClearControls();

                txtAgreementNum.Text = m_objSelectedAgreement.Agreement.Name;
                txtDescription.Text = m_objSelectedAgreement.Agreement.Description;
                dtBeginDate.DateTime = m_objSelectedAgreement.Agreement.BeginDate;
                dtEndDate.DateTime = m_objSelectedAgreement.Agreement.EndDate;

                cboxAgreementState.SelectedItem = (m_objSelectedAgreement.Agreement.AgreementState == null) ? null : cboxAgreementState.Properties.Items.Cast<CAgreementState>().Single<CAgreementState>(x => x.ID.CompareTo(m_objSelectedAgreement.Agreement.AgreementState.ID) == 0);
                cboxCompany.SelectedItem = (m_objSelectedAgreement.Company == null) ? null : cboxCompany.Properties.Items.Cast<CCompany>().Single<CCompany>(x => x.ID.CompareTo(m_objSelectedAgreement.Company.ID) == 0);
                cboxCustomer.SelectedItem = (m_objSelectedAgreement.Carrier == null) ? null : cboxCustomer.Properties.Items.Cast<CCarrier>().Single<CCarrier>(x => x.ID.CompareTo(m_objSelectedAgreement.Carrier.ID) == 0);

                SetPropertiesModified(false);
                //tabControl.SelectedTabPage = tabGeneral;
                btnCancel.Enabled = true;
                btnCancel.Focus();

                SetModeReadOnly(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования договора с перевозчиком. Текст ошибки: " + f.Message);
            }
            finally
            {
                //this.ResumeLayout(false);
                //tabControl.SelectedTabPage = tabGeneral;
                m_bDisableEvents = false;
            }
            return;
        }
        #endregion

        #region Новый договор с перевозчиком
        /// <summary>
        /// Новый договор с перевозчиком
        /// </summary>
        public void NewAgreement()
        {
            m_bDisableEvents = true;
            m_bNewObject = true;
            ShowWarningPnl(false);
            try
            {
                this.Refresh();

                m_objSelectedAgreement = new CAgreementWithCarrier();
                m_objSelectedAgreement.AgreementType = m_objAgreementType;
                m_objSelectedAgreement.Agreement = new CAgreement();
                m_objSelectedAgreement.Agreement.BeginDate = System.DateTime.Today;
                m_objSelectedAgreement.Agreement.EndDate = new DateTime(System.DateTime.Today.Year, 12, 31);

 //               this.SuspendLayout();

                ClearControls();

                txtAgreementNum.Text = m_objSelectedAgreement.Agreement.Name;
                txtDescription.Text = m_objSelectedAgreement.Agreement.Description;
                dtBeginDate.DateTime = m_objSelectedAgreement.Agreement.BeginDate;
                dtEndDate.DateTime = m_objSelectedAgreement.Agreement.EndDate;

                SetModeReadOnly(false);
                btnEdit.Enabled = false;
                SetPropertiesModified(true);

            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка создания договора с перевозчиком. Текст ошибки: " + f.Message);
            }
            finally
            {
//                this.ResumeLayout(false);
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
                SimulateChangeAgreementWithCarrierProperties(m_objSelectedAgreement, enumActionSaveCancel.Cancel, m_bNewObject);
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
            CAgreementWithCarrier objAgreementForSave = new CAgreementWithCarrier();
            try
            {
                objAgreementForSave.ID = m_objSelectedAgreement.ID;
                objAgreementForSave.Agreement = m_objSelectedAgreement.Agreement;
                objAgreementForSave.Agreement.BeginDate = dtBeginDate.DateTime;
                objAgreementForSave.Agreement.EndDate = dtEndDate.DateTime;
                objAgreementForSave.Agreement.AgreementType = m_objAgreementType;
                objAgreementForSave.Agreement.Description = txtDescription.Text;
                objAgreementForSave.Agreement.Name = txtAgreementNum.Text;
                objAgreementForSave.Agreement.AgreementState = ((cboxAgreementState.SelectedItem == null) ? null : (CAgreementState)cboxAgreementState.SelectedItem);
                objAgreementForSave.Carrier = ((cboxCustomer.SelectedItem == null) ? null : (CCarrier)cboxCustomer.SelectedItem);
                objAgreementForSave.Company = ((cboxCompany.SelectedItem == null) ? null : (CCompany)cboxCompany.SelectedItem);
                System.String strErr = "";
                if (m_bNewObject == true)
                {
                    // новый договор
                    bOkSave = objAgreementForSave.AddAgreementWithCarrier(m_objProfile, null, ref strErr);
                }
                else
                {
                    bOkSave = objAgreementForSave.EditAgreementWithCarrier(m_objProfile, null, ref strErr);
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
                    m_objSelectedAgreement.Carrier = objAgreementForSave.Carrier;
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
                if (bSaveChanges() == true)
                {
                    SimulateChangeAgreementWithCarrierProperties(m_objSelectedAgreement, enumActionSaveCancel.Save, m_bNewObject);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в описании договора с клиентом. Текст ошибки: " + f.Message);
            }
            return;
        }

        #endregion



    }

    /// <summary>
    /// Тип, хранящий информацию, которая передается получателям уведомления о событии
    /// </summary>
    public class ChangeAgreementWithCarrierPropertieEventArgs : EventArgs
    {
        private readonly CAgreementWithCarrier m_objAgreement;
        public CAgreementWithCarrier Agreement
        { get { return m_objAgreement; } }

        private readonly enumActionSaveCancel m_enActionType;
        public enumActionSaveCancel ActionType
        { get { return m_enActionType; } }

        private readonly System.Boolean m_bIsNewAgreement;
        public System.Boolean IsNewAgreement
        { get { return m_bIsNewAgreement; } }

        public ChangeAgreementWithCarrierPropertieEventArgs(CAgreementWithCarrier objAgreement, enumActionSaveCancel enActionType, System.Boolean bIsNewAgreement)
        {
            m_objAgreement = objAgreement;
            m_enActionType = enActionType;
            m_bIsNewAgreement = bIsNewAgreement;
        }
    }

}
