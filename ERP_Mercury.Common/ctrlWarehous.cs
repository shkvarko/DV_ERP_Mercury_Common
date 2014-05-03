using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ERP_Mercury.Common
{
    public partial class ctrlWarehous : UserControl
    {

        #region Свойства, переменные
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;

        private CWarehouse m_objSelectedWareHouse;

        private System.Boolean m_bIsChanged;

        public ctrlAddress frmAddress;


        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemAddress;


       // private System.Boolean m_bDisableEvents;
        private System.Boolean m_bNewWareHouse;
        private System.Boolean m_bIsReadOnly;

        private const System.Int32 iMinControlItemHeight = 20;
        private const System.Int32 iPanel1WidthDef = 350;

        #endregion
        
        #region События
        // Создаем закрытое поле, ссылающееся на заголовок списка делегатов
        private EventHandler<ChangeWarehousePropertieEventArgs> m_ChangeWarehouseProperties;
        // Создаем в классе член-событие
        public event EventHandler<ChangeWarehousePropertieEventArgs> ChangeWarehouseProperties
        {
            add
            {
                // берем закрытую блокировку и добавляем обработчик
                // (передаваемый по значению) в список делегатов
                m_ChangeWarehouseProperties += value;
            }
            remove
            {
                // берем закрытую блокировку и удаляем обработчик
                // (передаваемый по значению) из списка делегатов
                m_ChangeWarehouseProperties -= value;
            }
        }
        /// <summary>
        /// Инициирует событие и уведомляет о нем зарегистрированные объекты
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnChangeWarehouseProperties(ChangeWarehousePropertieEventArgs e)
        {
            // Сохраняем поле делегата во временном поле для обеспечение безопасности потока
            EventHandler<ChangeWarehousePropertieEventArgs> temp = m_ChangeWarehouseProperties;
            // Если есть зарегистрированные объектв, уведомляем их
            if (temp != null) temp(this, e);
        }
        public void SimulateChangeWarehouseProperties(CWarehouse objWarehouse, enumActionSaveCancel enActionType, System.Boolean bIsNewWarehouse)
        {
            // Создаем объект, хранящий информацию, которую нужно передать
            // объектам, получающим уведомление о событии
            ChangeWarehousePropertieEventArgs e = new ChangeWarehousePropertieEventArgs(objWarehouse, enActionType, bIsNewWarehouse);

            // Вызываем виртуальный метод, уведомляющий наш объект о возникновении события
            // Если нет типа, переопределяющего этот метод, наш объект уведомит все объекты, 
            // подписавшиеся на уведомление о событии
            OnChangeWarehouseProperties(e);
        }
        #endregion

        #region Конструктор
        public ctrlWarehous(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem)
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_bIsChanged = false;
            //m_bDisableEvents = false;
            m_bNewWareHouse = false;

            m_objSelectedWareHouse = null;


            frmAddress = new ERP_Mercury.Common.ctrlAddress(ERP_Mercury.Common.EnumObject.Warehouse, m_objProfile, m_objMenuItem, System.Guid.Empty);
            frmAddress.InitAddressControl();
            
            layoutControlAddress.Size = new Size(layoutControlAddress.Size.Width, (frmAddress.Size.Height));
            layoutControlAddress.MaximumSize = new Size(layoutControlAddress.Size.Width, layoutControlAddress.Size.Height);

            layoutControlGroupAddress.Size = new Size(layoutControlGroupAddress.Size.Width, (frmAddress.Size.Height));
           
            
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
            
            /*
            this.frmAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top // | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right))));*/

            this.frmAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));


            frmAddress.ChangeAddressPropertie += OnChangeAddressPropertie;

            
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
                if (m_objProfile.GetClientsRight().GetState(ERP_Mercury.Global.Consts.strDR_EditWarehouseCard) == false) // проверить что возвращается здесь. Здесь возвращается false, как будто нету прав, а они есть. разобраться
                {
                    btnEdit.Visible = false;
                    btnPrint.Visible = false;
                    btnSave.Visible = false;
                    //this.Controls.Remove(tableLayoutPanel4);
                }
                else
                {
                    btnEdit.Visible = true;
                    btnCancel.Visible = true;
                    //btnPrint.Visible = true;
                    btnSave.Visible = true;

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
                // форма 
                cboxTypeWarehouse.Properties.Items.Clear();
                List<CWareHouseType> objWareHouseTypeList = CWareHouseType.GetWareHouseTypeList(m_objProfile, null);
                if (objWareHouseTypeList != null)
                {
                    cboxTypeWarehouse.Properties.Items.AddRange(objWareHouseTypeList);
                }

                //objStateTypeList = null;

                //objDistributionNetworkList = null;

                //2011.06.03 if (frmAddress != null) { frmAddress.InitAllLists(); }
                //if (frmContact != null) { frmContact.LoadComboBoxItems(); }
                //if (frmRtt != null) { frmRtt.LoadComboBoxItems(); }

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
                SimulateChangeWarehouseProperties(m_objSelectedWareHouse, enumActionSaveCancel.Unkown, m_bNewWareHouse);
            }
        }
        /*
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
        }*/

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
        
        #region Редактировать хранилища
        /// <summary>
        /// !! Загружает свойства хранилища для редактирования/подробного порсмотра
        /// </summary>
        /// <param name="objWarehouse">компания</param>
        public void EditWarehouse(ERP_Mercury.Common.CWarehouse objWarehouse)
        {
            if (objWarehouse == null) { return; }
            //m_bDisableEvents = true;
            m_bNewWareHouse = false;
            
            ShowWarningPnl(false);
            try
            {
                //System.String strErr = "";

                m_objSelectedWareHouse = objWarehouse;
                
                // ЗДЕСЬ !!!!!!!!!!!!!!!!!!!!!
                // Заполняем не здесь, а ниже. Здесь (в этом блоке и чуть выше) заполняются combobox-ы,
                // которые НЕ возвращаются из конструкотора. А заполняются те, которые имеют тип данных List <>
                //m_objSelectedWareHouse.CompanyType = CCompanyType.GetCompanyTypeListForCompany(m_objProfile, null, m_objSelectedWareHouse.ID); // написать метод GetCompanyTypeListForCompany)

                
                if (m_objSelectedWareHouse.AddressForDeleteList == null) { m_objSelectedWareHouse.AddressForDeleteList = new List<CAddress>(); }
                else { m_objSelectedWareHouse.AddressForDeleteList.Clear(); }
              
               
                this.SuspendLayout();

                txtFullName.Text = "";
                txtCod.Text = "";
                cboxTypeWarehouse.SelectedItem = null;
                
                txtFullName.Text = m_objSelectedWareHouse.Name;
                txtCod.Text = Convert.ToString(m_objSelectedWareHouse.IBId);

                checkActive.Checked = m_objSelectedWareHouse.IsAcitve;
                checkIsForShiping.Checked = m_objSelectedWareHouse.IsForShipping;


                // Тип хранилища
                if ((m_objSelectedWareHouse.WarehouseType != null) && (cboxTypeWarehouse.Properties.Items.Count > 0))
                {
                    foreach (Object objWarehouseTypes in cboxTypeWarehouse.Properties.Items)
                    {
                        if (((CWareHouseType)objWarehouseTypes).ID.CompareTo(m_objSelectedWareHouse.WarehouseType.ID) == 0)
                        {
                            cboxTypeWarehouse.SelectedItem = objWarehouseTypes;
                            break;
                        }
                    }
                }


                // Адреса
                frmAddress.LoadAddressList(m_objSelectedWareHouse.ID, null);
               
                frmAddress.ClearAllPropertiesControls();
               
                SetPropertiesModified(false);
                //tabControl.SelectedTabPage = tabGeneral;
                btnCancel.Enabled = true;
                btnCancel.Focus();

                SetModeReadOnly(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования хранилища. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.ResumeLayout(false);

                //tabControl.SelectedTabPage = tabGeneral;
                //m_bDisableEvents = false;
            }
            return;
        }
        #endregion

        #region Новый клиент
        /// <summary>
        /// Новое хранилище
        /// </summary>
        public void NewWarehouse()
        {
            //m_bDisableEvents = true;
            m_bNewWareHouse = true;
            ShowWarningPnl(false);
            try
            {
                this.Refresh();
                frmAddress.StartThreadWithLoadData();

                m_objSelectedWareHouse = new ERP_Mercury.Common.CWarehouse();

                if (m_objSelectedWareHouse.AddressForDeleteList == null) { m_objSelectedWareHouse.AddressForDeleteList = new List<CAddress>(); }
                else { m_objSelectedWareHouse.AddressForDeleteList.Clear(); }

                this.SuspendLayout();

                txtFullName.Text = "";
                txtCod.Text = "";
                checkActive.Checked = true;
                checkIsForShiping.Checked = true;

                cboxTypeWarehouse.SelectedItem = null;

                txtFullName.Text = m_objSelectedWareHouse.Name;

                // Адреса
                frmAddress.LoadAddressList(m_objSelectedWareHouse.ID, null);

                frmAddress.ClearAllPropertiesControls();

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
                //m_bDisableEvents = false;
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
                SendMessageToLog("Ошибка отмены изменений в описании хранилища. Текст ошибки: " + f.Message);
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
                SimulateChangeWarehouseProperties(m_objSelectedWareHouse, enumActionSaveCancel.Cancel, m_bNewWareHouse);
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
            CWarehouse objWarehouseForSave = new CWarehouse();
            try
            {
                objWarehouseForSave.ID = m_objSelectedWareHouse.ID;
                objWarehouseForSave.Name = txtFullName.Text;
                
                // проверить, как работатет этот кусочек кода
                objWarehouseForSave.IsAcitve = checkActive.Checked;
                objWarehouseForSave.IsForShipping = checkIsForShiping.Checked;

                if (txtCod.Text == "")
                {
                    objWarehouseForSave.IBId = 0;
                }
                else
                {
                    objWarehouseForSave.IBId = Convert.ToInt32(txtCod.Text);
                }

                if (cboxTypeWarehouse.SelectedItem != null)
                {
                    objWarehouseForSave.WarehouseType = (CWareHouseType)cboxTypeWarehouse.SelectedItem;
                }

                if (objWarehouseForSave.AddressList == null) { objWarehouseForSave.AddressList = new List<CAddress>(); }
                objWarehouseForSave.AddressList.Clear();

                objWarehouseForSave.AddressList.AddRange(frmAddress.AddressList);

                if (objWarehouseForSave.AddressForDeleteList == null) { objWarehouseForSave.AddressForDeleteList = new List<CAddress>(); }
                objWarehouseForSave.AddressForDeleteList.Clear();

                objWarehouseForSave.AddressForDeleteList.AddRange(frmAddress.AddressDeletedList);
               
                System.String strErr = "";
                if (m_bNewWareHouse == true)
                {
                    // новая компания
                    bOkSave = objWarehouseForSave.Add(m_objProfile, null, ref strErr);
                }
                else
                {
                    //  Update в CWarehouse
                    // дописать потом
                    bOkSave = objWarehouseForSave.Update(m_objProfile, null, ref strErr);
                }
                SendMessageToLog(strErr);
                if (bOkSave == true)
                {

                    m_objSelectedWareHouse.ID = objWarehouseForSave.ID;
                    m_objSelectedWareHouse.Name = objWarehouseForSave.Name;

                    m_objSelectedWareHouse.WarehouseType = objWarehouseForSave.WarehouseType;

                    m_objSelectedWareHouse.IsAcitve = objWarehouseForSave.IsAcitve;
                    m_objSelectedWareHouse.IsForShipping = objWarehouseForSave.IsForShipping;

                    m_objSelectedWareHouse.AddressList = objWarehouseForSave.AddressList;

                    if (frmAddress.AddressDeletedList != null) { frmAddress.AddressDeletedList.Clear(); }
                    if (m_objSelectedWareHouse.AddressForDeleteList != null) { m_objSelectedWareHouse.AddressForDeleteList.Clear(); }

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
                SendMessageToLog("Ошибка сохранения изменений в описании хранилища. Текст ошибки: " + f.Message);
            }
            finally
            {
                objWarehouseForSave = null;
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
                txtFullName.Properties.ReadOnly = bSet;
                checkActive.Properties.ReadOnly = bSet;
                checkIsForShiping.Properties.ReadOnly = bSet;

                cboxTypeWarehouse.Properties.ReadOnly = bSet;
                
                frmAddress.SetChanceEditProperties(!bSet);

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
                frmAddress.StartThreadWithLoadData(); // потоки

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

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                lblWarningInfo.Text = "";
               
                this.Cursor = Cursors.WaitCursor;
                frmAddress.ConfirmChanges();
                if (bSaveChanges() == true)
                {
                    SimulateChangeWarehouseProperties(m_objSelectedWareHouse, enumActionSaveCancel.Save, m_bNewWareHouse);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в описании хранилища. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            return;
        }

        private void layoutControlAddress_GroupExpandChanged(object sender, DevExpress.XtraLayout.Utils.LayoutGroupEventArgs e)
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

        private void ctrlWarehous_Resize(object sender, EventArgs e)
        {
            layoutControlAddress.MaximumSize = new Size(this.Size.Width - iMinControlItemHeight, layoutControlAddress.MaximumSize.Height);
        }

    }

    
    /// <summary>
    /// Класс – хранящий информацию, которая передается получателям уведомления о событии
    /// </summary>
    public partial class ChangeWarehousePropertieEventArgs : EventArgs
    {
        private readonly CWarehouse m_objWarehouse;
        public CWarehouse Warehouse
        {
            get { return m_objWarehouse; }
        }

        private readonly enumActionSaveCancel m_enActionType;
        public enumActionSaveCancel ActionType
        {
            get { return m_enActionType; }
        }

        private readonly System.Boolean m_bIsNewWarehouse;
        public System.Boolean IsNewWarehouse
        {
            get { return m_bIsNewWarehouse; }
        }

        public ChangeWarehousePropertieEventArgs(CWarehouse objWarehouse, enumActionSaveCancel enActionType, System.Boolean bIsNewobjWarehouse)
        {
            m_objWarehouse = objWarehouse;
            m_enActionType = enActionType;
            m_bIsNewWarehouse = bIsNewobjWarehouse;
        }
    } 
    

}
