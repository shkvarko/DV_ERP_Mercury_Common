using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Card;
using DevExpress.XtraGrid.Views.Grid;

namespace ERP_Mercury.Common
{
    public partial class ctrlContact : UserControl
    {
        #region Свойства, переменные
        private EnumObject m_enObjectWithAddress;
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;
        public List<CContact> ContactList
        {
            get
            {
                List<CContact> objContactList = new List<CContact>();
                try
                {
                    foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListContacts.Nodes)
                    {
                        if (objNode.Tag != null)
                        {
                            objContactList.Add((CContact)objNode.Tag);
                        }
                    }
                }
                catch
                {
                    objContactList = null;
                }
                return objContactList;
            }
        }
        private List<CContact> m_objContactDeletedList;
        public List<CContact> ContactDeletedList
        {
            get { return m_objContactDeletedList; }
            set { m_objContactDeletedList = value; }
        }
        private CContact m_objSelectedContact;
        private System.Guid m_uuidOwnerId;

        private System.Boolean m_bIsChanged;
        /// <summary>
        /// блокировка возможности включения режима редактирования
        /// </summary>
//        private System.Boolean m_bBlockChanceEdit;

        public ctrlAddress frmAddress;
        private System.Boolean m_bDisableEvents;
        private System.Boolean m_bNewContact;
        private System.Boolean m_bIsReadOnly;

        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemAddress;
        private Size m_treeListSize;

        private const System.Int32 iMinControlItemHeight = 20;
        private const System.Int32 iAdvControlItemHeight = 40;
        #endregion

        #region События
        // Создаем закрытое поле, ссылающееся на заголовок списка делегатов
        private EventHandler<ChangeContactPropertieEventArgs> m_ChangeContactPropertie;
        // Создаем в классе член-событие
        public event EventHandler<ChangeContactPropertieEventArgs> ChangeContactProperties
        {
            add
            {
                // берем закрытую блокировку и добавляем обработчик
                // (передаваемый по значению) в список делегатов
                m_ChangeContactPropertie += value;
            }
            remove
            {
                // берем закрытую блокировку и удаляем обработчик
                // (передаваемый по значению) из списка делегатов
                m_ChangeContactPropertie -= value;
            }
        }
        /// <summary>
        /// Инициирует событие и уведомляет о нем зарегистрированные объекты
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnChangeContactProperties(ChangeContactPropertieEventArgs e)
        {
            // Сохраняем поле делегата во временном поле для обеспечение безопасности потока
            EventHandler<ChangeContactPropertieEventArgs> temp = m_ChangeContactPropertie;
            // Если есть зарегистрированные объектв, уведомляем их
            if (temp != null) temp(this, e);
        }
        public void SimulateChangeContactProperties(CContact objContact, enumActionSaveCancel enActionType, System.Boolean bIsNewContact)
        {
            // Создаем объект, хранящий информацию, которую нужно передать
            // объектам, получающим уведомление о событии
            ChangeContactPropertieEventArgs e = new ChangeContactPropertieEventArgs(objContact, enActionType, bIsNewContact);

            // Вызываем виртуальный метод, уведомляющий наш объект о возникновении события
            // Если нет типа, переопределяющего этот метод, наш объект уведомит все объекты, 
            // подписавшиеся на уведомление о событии
            OnChangeContactProperties(e);
        }
        #endregion

        #region Конструктор
        public ctrlContact(EnumObject enObjectWithAddress, UniXP.Common.CProfile objProfile,
            UniXP.Common.MENUITEM objMenuItem, System.Guid uuidOwnerId, System.Boolean bLoadComboBox = true)
        {
            InitializeComponent();

            m_enObjectWithAddress = enObjectWithAddress;
            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_uuidOwnerId = uuidOwnerId;
            m_bIsChanged = false;
            m_bDisableEvents = false;
            //m_bBlockChanceEdit = false;
            m_bNewContact = false;

            m_objContactDeletedList = null;

            m_objSelectedContact = null;
            m_bIsReadOnly = false;
            m_treeListSize = new Size(tableLayoutPanel8.Size.Width, tableLayoutPanel8.Size.Height);

            frmAddress = new ERP_Mercury.Common.ctrlAddress(ERP_Mercury.Common.EnumObject.Contact, m_objProfile, m_objMenuItem, System.Guid.Empty);
            //frmAddress.InitAddressControl();

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
            this.frmAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            frmAddress.ChangeAddressPropertie += OnChangeAddressPropertie;

            layoutControlAddress.MaximumSize = new Size(layoutControlAddress.Size.Width, layoutControlAddress.Size.Height);
            layoutControlDscrpn.MaximumSize = new Size(layoutControlDscrpn.Size.Width, layoutControlDscrpn.Size.Height);
            layoutControlInternet.MaximumSize = new Size(layoutControlInternet.Size.Width, layoutControlInternet.Size.Height);
            layoutControlJob.MaximumSize = new Size(layoutControlJob.Size.Width, layoutControlJob.Size.Height);
            layoutControlPhone.MaximumSize = new Size(layoutControlPhone.Size.Width, layoutControlPhone.Size.Height);

            layoutControlGroupAddress.Expanded = false;
            layoutControlGroupDscrpn.Expanded = false;
            layoutControlGroupInternet.Expanded = false;
            layoutControlGroupJob.Expanded = false;
            layoutControlGroupPhone.Expanded = false;

            if (bLoadComboBox == true)
            {
                LoadComboBoxItems();
            }
        }
        #endregion

        #region Выпадающие списки
        /// <summary>
        /// Обновление выпадающих списков
        /// </summary>
        /// <returns>true - все списки успешно обновлены; false - ошибка</returns>
        public System.Boolean LoadComboBoxItems()
        {
            System.Boolean bRet = false;
            try
            {
                // нужно загрузить значения для выпадающих списков "отдел" и "должность"
                cboxDepartment.Properties.Items.Clear();
                List<CDepartament> objDepartamentList = CDepartament.GetDepartamentList(m_objProfile, null);
                if (objDepartamentList != null)
                {
                    cboxDepartment.Properties.Items.AddRange(objDepartamentList);
                }
                //objDepartamentList = null;

                cboxJobPosition.Properties.Items.Clear();
                List<CJobPosition> objJobPositionList = CJobPosition.GetJobPositionList(m_objProfile, null);
                if (objJobPositionList != null)
                {
                    cboxJobPosition.Properties.Items.AddRange( objJobPositionList );
                }
                //objJobPositionList = null;

                repItemcboxPhoneType.Items.Clear();
                List<CPhoneType> objPhoneTypeList = CPhoneType.GetPhoneTypeList(m_objProfile, null);
                if (objPhoneTypeList != null)
                {
                    repItemcboxPhoneType.Items.AddRange(objPhoneTypeList);
                }

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
        /// <summary>
        /// Заполнение выпадающих списков в редакторе
        /// </summary>
        /// <param name="objDepartamentList">список служб</param>
        /// <param name="objJobPositionList">список должностей</param>
        /// <param name="objPhoneTypeList">список типов телефонных номеров</param>
        public void LoadComboBoxItems(List<CDepartament> objDepartamentList, List<CJobPosition> objJobPositionList,
                List<CPhoneType> objPhoneTypeList )
        {
            try
            {
                cboxDepartment.Properties.Items.Clear();
                cboxJobPosition.Properties.Items.Clear();
                repItemcboxPhoneType.Items.Clear();

                if (objDepartamentList != null)
                {
                    cboxDepartment.Properties.Items.AddRange(objDepartamentList);
                }

                if (objJobPositionList != null)
                {
                    cboxJobPosition.Properties.Items.AddRange(objJobPositionList);
                }

                if (objPhoneTypeList != null)
                {
                    repItemcboxPhoneType.Items.AddRange(objPhoneTypeList);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка обновления выпадающих списков. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return ;
        }

        #endregion

        #region Загрузить список контактов
        /// <summary>
        /// Загружает список контактов
        /// </summary>
        /// <param name="uuidOwnerId">уникальный идентификатор владельца контактов</param>
        /// <param name="objContactListSrc">список контактов</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public System.Boolean LoadContactList(System.Guid uuidOwnerId, List<CContact> objContactListSrc)
        {
            System.Boolean bRet = false;
            try
            {
                treeListContacts.Nodes.Clear();
                m_uuidOwnerId = uuidOwnerId;
                ClearAllPropertiesControls();

                if ((objContactListSrc == null) || (objContactListSrc.Count == 0))
                {
                    List<CContact> objContactList = CContact.GetContactList(m_objProfile, null, m_enObjectWithAddress, m_uuidOwnerId);
                    if (objContactList != null)
                    {
                        System.String strErr = "";
                        foreach (CContact objContact in objContactList)
                        {
                            // для каждого контакта загрузим его свойства 
                            if (objContact.LoadContactProperties(m_objProfile, null, ref strErr) == false)
                            {
                                SendMessageToLog(strErr);
                            }
                            else
                            {
                                // добавляем контакт в дерево
                                DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListContacts.AppendNode(new object[] { objContact.VisitingCard2 }, null);
                                objNode.Tag = objContact;
                            }
                        }
                    }
                    objContactList = null;
                }
                else
                {
                    foreach (CContact objContact in objContactListSrc)
                    {
                        DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListContacts.AppendNode(new object[] { objContact.VisitingCard2 }, null);
                        objNode.Tag = objContact;
                    }
                }

                if (treeListContacts.Nodes.Count > 0)
                {
                    treeListContacts.FocusedNode = treeListContacts.Nodes[0];
                    EditContact((CContact)treeListContacts.Nodes[0].Tag);
                }
                else
                {
                    ClearAllPropertiesControls();
                }

                SetReadOnlyPropertiesControls(true);

                bRet = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка обновления списка.\n\nТекст ошибки:\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return bRet;
        }
        private void EnableDisableTreeListBtns(System.Boolean bEnable)
        {
            if ((bEnable == true) && (treeListContacts.Nodes.Count == 0))
            {
                btnEditContact.Enabled = false;
                btnDeleteContact.Enabled = false;
            }
            else
            {
                btnEditContact.Enabled = bEnable;
                btnDeleteContact.Enabled = bEnable;
            }
            btnAddContact.Enabled = bEnable;
        }
        public void ClearAllPropertiesControls()
        {
            m_bDisableEvents = true;
            try
            {
                // очистим содержимое элементов управления
                txtLastName.Text = "";
                txtFirstName.Text = "";
                txtMiddleName.Text = "";
                txtDescription.Text = "";
                txtNickName.Text = "";
                txtWebPage.Text = "";
                dtBirthday.Text = "";

                cboxDepartment.SelectedItem = null;
                cboxJobPosition.SelectedItem = null;
                treeListEMail.Nodes.Clear();
                treeListPhone.Nodes.Clear();

                frmAddress.ClearAllPropertiesControls();

            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("ClearAllPropertiesControls\nТекст ошибки: " + f.Message);
            }
            finally
            {
                m_bDisableEvents = false;
            }

            return;
        }
        public void ClearContactListTree()
        {
            m_bDisableEvents = true;
            try
            {
                treeListContacts.Nodes.Clear();
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("ClearContactListTree\nТекст ошибки: " + f.Message);
            }
            finally
            {
                m_bDisableEvents = false;
            }

            return;
        }
        public void SetChanceEditProperties(System.Boolean bChance)
        {
            try
            {
                if (bChance == false)
                {
                    // отключить возможнось войти в режим редактирования
                    //m_bBlockChanceEdit = true;
                    btnAddContact.Enabled = bChance;
                    btnEditContact.Enabled = bChance;
                    btnDeleteContact.Enabled = bChance;

                    btnCancel.Enabled = bChance;
                    btnSave.Enabled = bChance;
                }
                else
                {
                    // включить возможнось войти в режим редактирования
                    //m_bBlockChanceEdit = false;
                    SetReadOnlyPropertiesControls(true);
                }
                frmAddress.SetChanceEditProperties(bChance);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("SetChanceEditProperties.\n\nТекст ошибки:\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        /// <summary>
        /// Включает/отключает элементы управления для отображения свойств адреса
        /// </summary>
        /// <param name="bEnable">признак "включить/выключить"</param>
        private void SetReadOnlyPropertiesControls(System.Boolean bEnable)
        {
            try
            {
                txtLastName.Properties.ReadOnly = bEnable;
                txtFirstName.Properties.ReadOnly = bEnable;
                txtMiddleName.Properties.ReadOnly = bEnable;
                txtDescription.Properties.ReadOnly = bEnable;
                txtNickName.Properties.ReadOnly = bEnable;
                txtWebPage.Properties.ReadOnly = bEnable;
                dtBirthday.Properties.ReadOnly = bEnable;

                cboxDepartment.Properties.ReadOnly = bEnable;
                cboxDepartment.Properties.ReadOnly = bEnable;
                cboxJobPosition.Properties.ReadOnly = bEnable;
                treeListEMail.OptionsBehavior.Editable = !bEnable;
                treeListPhone.OptionsBehavior.Editable = !bEnable;

                foreach (DevExpress.XtraTreeList.Columns.TreeListColumn objColumn in treeListEMail.Columns)
                {
                    objColumn.OptionsColumn.ReadOnly = bEnable;
                }

                foreach (DevExpress.XtraTreeList.Columns.TreeListColumn objColumn in treeListPhone.Columns)
                {
                    objColumn.OptionsColumn.ReadOnly = bEnable;
                }

                m_bIsReadOnly = bEnable;
                if (bEnable == true)
                {
                    // включен режим "только просмотр"
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnAddContact.Enabled = true;
                    btnEditContact.Enabled = (treeListContacts.FocusedNode != null);
                    btnDeleteContact.Enabled = (treeListContacts.FocusedNode != null);
                    tableLayoutPanel8.Dock = DockStyle.Fill;
                    tableLayoutPanel8.Size = new Size(m_treeListSize.Width, m_treeListSize.Height);
                    xtraScrollableControl1.Visible = false;
                    m_bIsChanged = false;
                }
                else
                {
                    // включен режим "редактирование"
                    btnSave.Enabled = m_bIsChanged;
                    btnCancel.Enabled = true;
                    btnAddContact.Enabled = false;
                    btnEditContact.Enabled = !m_bIsChanged;
                    btnDeleteContact.Enabled = false;
                    tableLayoutPanel8.Size = new Size(0, 0);
                    xtraScrollableControl1.Visible = true;
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("SetReadOnlyPropertiesControls. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void treeListContacts_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                if ((e.Node == null) || (e.Node.Tag == null) || (e.Node.TreeList.Nodes.Count == 0)) { return; }
                ShowContact((CContact)e.Node.Tag);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка смены Контакта.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                SetPropertiesModified(false);
            }

            return;
        }
        private void treeListContacts_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (m_bIsChanged == true) { return; }
                    // попробуем определить, что же у нас под мышкой
                    DevExpress.XtraTreeList.TreeListHitInfo hi = ((DevExpress.XtraTreeList.TreeList)sender).CalcHitInfo(new Point(e.X, e.Y));
                    if ((hi != null) && (hi.Node != null))
                    {
                        // выделяем узел
                        hi.Node.TreeList.FocusedNode = hi.Node;
                    }
                    menuAdd.Enabled = btnAddContact.Enabled;
                    menuDelete.Enabled = btnDeleteContact.Enabled;
                    menuEdit.Enabled = btnEditContact.Enabled;

                    contextMenuStrip.Show(((DevExpress.XtraTreeList.TreeList)sender), new Point(e.X, e.Y));
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListRtt_MouseClick.\nТекст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;

        }

        private void treeListRtt_BeforeFocusNode(object sender, DevExpress.XtraTreeList.BeforeFocusNodeEventArgs e)
        {
            try
            {
                if (m_bIsChanged == true)
                {
                    //// в описание контакта были внесены изменения
                    //DialogResult dlgRes = DevExpress.XtraEditors.XtraMessageBox.Show(
                    //    "Данные контакта были изменены. Подтвердить изменения в описании контакта?", "Подтверждение",
                    //    System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question);
                    //switch (dlgRes)
                    //{
                    //    case DialogResult.Yes:
                    //        {
                    //            e.CanFocus = bSaveChanges2();
                    //            break;
                    //        }
                    //    case DialogResult.No:
                    //        {
                    //            e.CanFocus = true;
                    //            break;
                    //        }
                    //    case DialogResult.Cancel:
                    //        {
                    //            e.CanFocus = true;
                    //            break;
                    //        }
                    //    default:
                    //        {
                    //            break;
                    //        }

                    //}
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListAddress_BeforeFocusNode. Текст ошибки: " + f.Message);
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
            m_bIsChanged = bModified;
            btnSave.Enabled = m_bIsChanged;
            btnCancel.Enabled = btnSave.Enabled;
            if (m_bIsChanged == true)
            {
                SimulateChangeContactProperties(m_objSelectedContact, enumActionSaveCancel.Unkown, m_bNewContact);
            }
        }
        private void cboxContactPropertie_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                SetPropertiesModified(true);                
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения свойств " + ( ( DevExpress.XtraEditors.ComboBoxEdit )sender ).ToolTip + ".\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            

            return;
        }
        private void txtContactPropertie_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                if (e.NewValue != null)
                {
                    SetPropertiesModified(true);
                }
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения свойств контакта.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        private void OnChangeAddressPropertie(Object sender, ChangeAddressPropertieEventArgs e)
        {
            try
            {
                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "OnChangeAddressPropertie.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally // очищаем занимаемые ресурсы
            {
            }

            return;
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
                if (m_objSelectedContact.EMailList == null) { m_objSelectedContact.EMailList = new List<CEMail>(); }
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
                //m_objSelectedContact.EMailList.Add(objEMail);

                objEMail.IsMain = false;
                DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListEMail.AppendNode(new object[] { objEMail.EMail, objEMail.IsActive, objEMail.IsMain }, null);
                objNode.Tag = objEMail;
                treeListEMail.FocusedNode = objNode;
                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog( "Ошибка добавления электронного адреса.\nТекст ошибки: " + f.Message);
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

                if (m_objSelectedContact.EMailForDeleteList == null) { m_objSelectedContact.EMailForDeleteList = new List<CEMail>(); }
                DevExpress.XtraTreeList.Nodes.TreeListNode objPrevNode = objNode.PrevNode;
                m_objSelectedContact.EMailForDeleteList.Add((CEMail)objNode.Tag);

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
                SendMessageToLog("mitemAddEMail_Click.\n\nТекст ошибки: " + f.Message);
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
                SendMessageToLog("treeListEMail_KeyDown.\n\nТекст ошибки: " + f.Message);
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
                SendMessageToLog("mitemDeleteEMail_Click.\n\nТекст ошибки: " + f.Message);
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
                    if (IsEMailDublicate((System.String)e.Node[colEMailAddress], treeListEMail.GetNodeIndex( e.Node ) ) == true)
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
            /*
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
                            //m_objSelectedContact.EMailList[iPosNode].EMail = (System.String)e.Value;
                            ((CEMail)e.Node.Tag).EMail = (System.String)e.Value;
                        }
                    }
                }

                // Здесь...
                if ((e.Column == colIsMainEMail) && (e.Value != null) && ( ( (System.Boolean)e.Value ) == true ))
                {
                    // главным може быть только один адрес,
                    // поэтому все остальные нужно сделать неглавными
                    for (System.Int32 i = 0; i < treeListEMail.Nodes.Count; i++)
                    {
                        if (i != iPosNode)
                        {
                            treeListEMail.Nodes[i].SetValue(colIsMainEMail, false);
                            ((CEMail)treeListEMail.Nodes[i].Tag).IsMain = false;
                            e.Node.SetValue(colIsMainEMail, e.Value);
                            //m_objSelectedContact.EMailList[i].IsMain = false;
                        }
                    }
                    //m_objSelectedContact.EMailList[iPosNode].IsMain = true;
                    ((CEMail)e.Node.Tag).IsMain = true;
                    e.Node.SetValue(colIsActiveEMail, true);
                    e.Node.SetValue(colIsMainEMail, true);
                }
                if ((e.Column == colIsActiveEMail) && (e.Value != null))
                {
                    //m_objSelectedContact.EMailList[iPosNode].IsActive = (System.Boolean)e.Value;
                    ((CEMail)e.Node.Tag).IsActive = (System.Boolean)e.Value;
                    e.Node.SetValue(colIsActiveEMail, e.Value);
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
             */ 
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

        #region Телефонные номера
        /// <summary>
        /// Добавляет в список телефонный номер
        /// </summary>
        private void AddPhone()
        {
            try
            {
                if (treeListPhone.Enabled == false) { treeListPhone.Enabled = true; }
                if (m_objSelectedContact.PhoneList == null) { m_objSelectedContact.PhoneList = new List<CPhone>(); }
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
                objPhone.PhoneNumber = "(";
                objPhone.IsMain = false;
                //m_objSelectedContact.PhoneList.Add(objPhone);
                DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListPhone.AppendNode(new object[] { objPhone.PhoneType, objPhone.PhoneNumber, objPhone.IsActive, objPhone.IsMain }, null);
                objNode.Tag = objPhone;
                treeListPhone.FocusedNode = objNode;
                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка добавления телефонного номера.\nТекст ошибки: " + f.Message);
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

                if (m_objSelectedContact.PhoneForDeleteList == null) { m_objSelectedContact.PhoneForDeleteList = new List<CPhone>(); }
                DevExpress.XtraTreeList.Nodes.TreeListNode objPrevNode = objNode.PrevNode;
                m_objSelectedContact.PhoneForDeleteList.Add((CPhone)objNode.Tag);

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

            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка удаления телефонного номера.\nТекст ошибки: " + f.Message);
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
                SendMessageToLog("mitemAddPhone_Click.\n\nТекст ошибки: " + f.Message);
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
                SendMessageToLog("mitemDeleteEMail_Click.\n\nТекст ошибки: " + f.Message);
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
                SendMessageToLog("treeListPhone_MouseClick.\nТекст ошибки: " + f.Message);
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
                SendMessageToLog("Ошибка проверки телефонного номера.\nНомер: " + strPhone + "\nТекст ошибки: " + f.Message);
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
                SendMessageToLog("Ошибка проверки телефонного номера.\nНомер: " + strPhone + "\nТекст ошибки: " + f.Message);
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
                        treeListPhone.SetColumnError(colEMailAddress, "Такой телефонный номер уже есть в списке.");
                    }
                    else
                    {
                        SetPropertiesModified(true);
                    }
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListPhone_ValidateNode.\nТекст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        #endregion

        #region Новый контакт
        /// <summary>
        /// Новый контакт
        /// </summary>
        private void NewContact()
        {
            try
            {
                m_bDisableEvents = true;
                m_bNewContact = true;

                m_objSelectedContact = new CContact();
                m_objSelectedContact.IsNewObject = true;

                ClearAllPropertiesControls();

                DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListContacts.AppendNode(new object[] { m_objSelectedContact.VisitingCard }, null);
                objNode.Tag = m_objSelectedContact;
                treeListContacts.FocusedNode = objNode;

                txtLastName.Focus();
                SetPropertiesModified(true);
                SetReadOnlyPropertiesControls(false);
                // включаем/выключаем кнопки
                btnEditContact.Enabled = false;
                btnAddContact.Enabled = false;
                btnCancel.Enabled = true;
                btnSave.Enabled = true;
                btnDeleteContact.Enabled = false;

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка создания контакта.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                m_bDisableEvents = false;
            }
            return;
        }

        /// <summary>
        /// Новый контакт
        /// </summary>
        public void NewContact(CContact objContact)
        {
            try
            {
                m_bDisableEvents = true;
                m_bNewContact = true;

                m_objSelectedContact = objContact;
                //m_objSelectedContact = new CContact();
                m_objSelectedContact.IsNewObject = true;

                ClearAllPropertiesControls();

                DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListContacts.AppendNode(new object[] { m_objSelectedContact.VisitingCard }, null);
                objNode.Tag = m_objSelectedContact;
                treeListContacts.FocusedNode = objNode;

                txtLastName.Focus();
                SetPropertiesModified(true);
                SetReadOnlyPropertiesControls(false);
                // включаем/выключаем кнопки
                btnEditContact.Enabled = false;
                btnAddContact.Enabled = false;
                btnCancel.Enabled = true;
                btnSave.Enabled = true;
                btnDeleteContact.Enabled = false;

                // ФИО
                txtLastName.Text = objContact.LastName;
                txtFirstName.Text = objContact.FirstName;
                txtMiddleName.Text = objContact.MiddleName;
                if (objContact.Birthday != System.DateTime.MinValue)
                {
                    dtBirthday.DateTime = objContact.Birthday;
                }
                // Работа
                if ((objContact.Department != null) && (cboxDepartment.Properties.Items.Count > 0))
                {
                    foreach (Object objDepartament in cboxDepartment.Properties.Items)
                    {
                        if (((CDepartament)objDepartament).ID.CompareTo(objContact.Department.ID) == 0)
                        {
                            cboxDepartment.SelectedItem = objDepartament;
                            break;
                        }
                    }
                }
                if ((objContact.JobPosition != null) && (cboxJobPosition.Properties.Items.Count > 0))
                {
                    foreach (Object objJobPosition in cboxJobPosition.Properties.Items)
                    {
                        if (((CJobPosition)objJobPosition).ID.CompareTo(objContact.JobPosition.ID) == 0)
                        {
                            cboxJobPosition.SelectedItem = objJobPosition;
                            break;
                        }
                    }
                }
                // Интернет
                txtNickName.Text = objContact.NickName;
                txtWebPage.Text = objContact.WWW;
                if (objContact.EMailList != null)
                {
                    foreach (CEMail objEMail in objContact.EMailList)
                    {
                        treeListEMail.AppendNode(new object[] { objEMail.EMail, objEMail.IsActive, objEMail.IsMain }, null).Tag = objEMail;
                    }
                }
                // Телефоны
                if (objContact.PhoneList != null)
                {
                    foreach (CPhone objPhone in objContact.PhoneList)
                    {
                        treeListPhone.AppendNode(new object[] { objPhone.PhoneType, objPhone.PhoneNumber, objPhone.IsActive, objPhone.IsMain }, null).Tag = objPhone;
                    }
                }
                // Примечание
                txtDescription.Text = objContact.Description;
                // Адреса
                frmAddress.LoadAddressList(objContact.ID, objContact.AddressList);

                SetPropertiesModified(true);
                SetReadOnlyPropertiesControls(false);


            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка создания контакта.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                m_bDisableEvents = false;
            }
            return;
        }

        private void barBtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                NewContact();
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка создания контакта.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        private void menuAdd_Click(object sender, EventArgs e)
        {
            try
            {
                NewContact();
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка создания контакта.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        #endregion

        #region Просмотр свойств контакта
        /// <summary>
        /// Просмотр свойств контакта
        /// </summary>
        /// <param name="objContact">контакт</param>
        public void ShowContact(CContact objContact)
        {
            try
            {
                EditContact(objContact);
                SetReadOnlyPropertiesControls(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка просмотра свойств РТТ. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        #endregion

        #region Редактировать контакт
        /// <summary>
        /// Загружает свойства контакта для редактирования
        /// </summary>
        /// <param name="objContact">контакт</param>
        private void EditContact(CContact objContact)
        {
            if (objContact == null) { return; }
            m_bDisableEvents = true;
            try
            {
                m_objSelectedContact = objContact;

                ClearAllPropertiesControls();

                // ФИО
                txtLastName.Text = m_objSelectedContact.LastName;
                txtFirstName.Text = m_objSelectedContact.FirstName;
                txtMiddleName.Text = m_objSelectedContact.MiddleName;
                if( m_objSelectedContact.Birthday != System.DateTime.MinValue )
                {
                    dtBirthday.DateTime = m_objSelectedContact.Birthday;
                }
                // Работа
                if ((m_objSelectedContact.Department != null) && (cboxDepartment.Properties.Items.Count > 0))
                {
                    foreach (Object objDepartament in cboxDepartment.Properties.Items)
                    {
                        if (((CDepartament)objDepartament).ID.CompareTo(m_objSelectedContact.Department.ID) == 0)
                        {
                            cboxDepartment.SelectedItem = objDepartament;
                            break;
                        }
                    }
                }
                if( (m_objSelectedContact.JobPosition != null) && (cboxJobPosition.Properties.Items.Count > 0))
                {
                    foreach (Object objJobPosition in cboxJobPosition.Properties.Items)
                    {
                        if (((CJobPosition)objJobPosition).ID.CompareTo(m_objSelectedContact.JobPosition.ID) == 0)
                        {
                            cboxJobPosition.SelectedItem = objJobPosition;
                            break;
                        }
                    }
                }
                // Интернет
                txtNickName.Text = m_objSelectedContact.NickName;
                txtWebPage.Text = m_objSelectedContact.WWW;
                if (m_objSelectedContact.EMailList != null)
                {
                    foreach (CEMail objEMail in m_objSelectedContact.EMailList)
                    {
                        DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListEMail.AppendNode(new object[] { objEMail.EMail, objEMail.IsActive, objEMail.IsMain }, null);
                        objNode.Tag = objEMail;
                    }
                }
                // Телефоны
                if (m_objSelectedContact.PhoneList != null)
                {
                    foreach (CPhone objPhone in m_objSelectedContact.PhoneList)
                    {
                        DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListPhone.AppendNode(new object[] { objPhone.PhoneType, objPhone.PhoneNumber, objPhone.IsActive, objPhone.IsMain }, null);
                        objNode.Tag = objPhone;
                    }
                }
                // Примечание
                txtDescription.Text = m_objSelectedContact.Description;
                // Адреса
                frmAddress.LoadAddressList(m_objSelectedContact.ID, m_objSelectedContact.AddressList);

                SetPropertiesModified(false);
                SetReadOnlyPropertiesControls(false);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка редактирования контакта.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                m_bDisableEvents = false;
            }
            return;
        }
        private void menuEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if ((treeListContacts.FocusedNode == null) || (treeListContacts.FocusedNode.Tag == null)) { return; }
                EditContact((CContact)treeListContacts.FocusedNode.Tag);

                btnEditContact.Enabled = false;
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения свойств контакта.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        #endregion

        #region Удалить контакт
        private System.Boolean DeleteContact(DevExpress.XtraTreeList.Nodes.TreeListNode objNode)
        {
            System.Boolean bRet = false;
            if ((objNode == null) || (objNode.Tag == null)) { return bRet; }
            try
            {
                if (m_objContactDeletedList == null) { m_objContactDeletedList = new List<CContact>(); }
                m_objContactDeletedList.Add((CContact)objNode.Tag);

                System.Int32 iPosNode = treeListContacts.GetNodeIndex(objNode);
                DevExpress.XtraTreeList.Nodes.TreeListNode objPrevNode = objNode.PrevNode;

                treeListContacts.Nodes.RemoveAt(iPosNode);
                if (objPrevNode == null)
                {
                    if (treeListContacts.Nodes.Count > 0)
                    {
                        treeListContacts.FocusedNode = treeListContacts.Nodes[0];
                    }
                }
                else
                {
                    treeListContacts.FocusedNode = objPrevNode;
                }

                if (treeListContacts.FocusedNode == null)
                {
                    ClearAllPropertiesControls();

                    btnAddContact.Enabled = true;
                    btnEditContact.Enabled = false;
                    btnDeleteContact.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                }
                else
                {
                    btnAddContact.Enabled = true;
                    btnEditContact.Enabled = true;
                    btnDeleteContact.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                }

                bRet = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления контакта.\n\nТекст ошибки:\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return bRet;
        }
        private void menuDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteContact(treeListContacts.FocusedNode);
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка удаления контакта.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
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
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка отмены изменений.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        private void Cancel()
        {
            try
            {
                if (m_objSelectedContact == null) { return; }
                if (m_bNewContact == true)
                {
                    ClearAllPropertiesControls();
                    SetReadOnlyPropertiesControls(true);
                    if ( treeListContacts.FocusedNode != null)
                    {
                        System.Int32 iNodeIndx = treeListContacts.GetNodeIndex(treeListContacts.FocusedNode);
                        treeListContacts.Nodes.Remove(treeListContacts.FocusedNode);
                        if (treeListContacts.Nodes.Count > 0)
                        {
                            treeListContacts.FocusedNode = treeListContacts.Nodes[treeListContacts.Nodes.Count - 1];
                        }
                    }
                }
                else
                {
                    if (( m_objSelectedContact != null) && (m_bNewContact == false))
                    {
                        ShowContact(m_objSelectedContact);
                    }
                }

                btnAddContact.Enabled = true;
                btnEditContact.Enabled = ( treeListContacts.FocusedNode != null );
                btnDeleteContact.Enabled = (treeListContacts.FocusedNode != null);
                btnCancel.Enabled = false;
                btnSave.Enabled = false;

                SimulateChangeContactProperties(m_objSelectedContact, enumActionSaveCancel.Cancel, m_bNewContact);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка отмены изменений в описании контакта. Текст ошибки: " + f.Message);
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
            CContact objContactForSave = new CContact();
            try
            {
                // для сохранения изменений создадим экземпляр класса "Контакт",
                // затем присвоим ему свойства из элементов управления и попробуем сохранить
                // если все пройдет хорошо, то m_objSelectedContact присвоим свойства из нашего экземпляра класса "Контакт"
                // и закроем форму контакта, перейдя к списку контактов
                objContactForSave.ID = m_objSelectedContact.ID;
                objContactForSave.LastName = txtLastName.Text;
                objContactForSave.FirstName = txtFirstName.Text;
                objContactForSave.MiddleName = txtMiddleName.Text;
                objContactForSave.Department = (cboxDepartment.SelectedItem == null) ? null : (CDepartament)cboxDepartment.SelectedItem;
                objContactForSave.JobPosition = (cboxJobPosition.SelectedItem == null) ? null : (CJobPosition)cboxJobPosition.SelectedItem;
                objContactForSave.Description = txtDescription.Text;
                objContactForSave.NickName = txtNickName.Text;
                objContactForSave.WWW = txtWebPage.Text;
                if ((dtBirthday.Text != "") && (dtBirthday.DateTime != System.DateTime.MinValue))
                {
                    objContactForSave.Birthday = dtBirthday.DateTime;
                }
                if (objContactForSave.EMailList == null) { objContactForSave.EMailList = new List<CEMail>(); }
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListEMail.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    objContactForSave.EMailList.Add((CEMail)objNode.Tag);
                }
                if (objContactForSave.PhoneList == null) { objContactForSave.PhoneList = new List<CPhone>(); }
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListPhone.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    objContactForSave.PhoneList.Add((CPhone)objNode.Tag);
                }
                if (frmAddress.AddressList != null)
                {
                    objContactForSave.AddressList = frmAddress.AddressList;
                }
                System.String strErr = "";
                if (objContactForSave.ID.CompareTo(System.Guid.Empty) == 0)
                {
                    // новый контакт
                    bOkSave = objContactForSave.Add(m_enObjectWithAddress, m_uuidOwnerId, m_objProfile, null, ref strErr);
                }
                else
                {
                    ////// !!!!!!!!!!!!!!!!!!!!!!!
                    bOkSave = objContactForSave.Update(m_enObjectWithAddress, m_uuidOwnerId, m_objProfile, null, ref strErr);
                    //    frmAddress.AddressDeletedList, m_objSelectedContact.PhoneForDeleteList, m_objSelectedContact.EMailForDeleteList);
                }
                SendMessageToLog(strErr);
                if (bOkSave == true)
                {
                    // изменения успешно сохранены, теперь нужно значения свойств objContactForSave присвоить m_objSelectedContact
                    m_objSelectedContact.ID = objContactForSave.ID;
                    m_objSelectedContact.LastName = objContactForSave.LastName;
                    m_objSelectedContact.FirstName = objContactForSave.FirstName;
                    m_objSelectedContact.MiddleName = objContactForSave.MiddleName;
                    m_objSelectedContact.Department = objContactForSave.Department;
                    m_objSelectedContact.JobPosition = objContactForSave.JobPosition;
                    m_objSelectedContact.Description = objContactForSave.Description;
                    m_objSelectedContact.PhoneList = objContactForSave.PhoneList;
                    m_objSelectedContact.EMailList = objContactForSave.EMailList;
                    m_objSelectedContact.AddressList = objContactForSave.AddressList;
                    m_objSelectedContact.NickName = objContactForSave.NickName;
                    m_objSelectedContact.WWW = objContactForSave.WWW;
                    bRet = true;
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений.\nТекст ошибки: " + f.Message);
            }
            finally
            {
                objContactForSave = null;
            }
            return bRet;
        }
        private System.Boolean bSaveChanges2()
        {
            System.Boolean bRet = false;
            try
            {
                m_objSelectedContact.LastName = txtLastName.Text;
                m_objSelectedContact.FirstName = txtFirstName.Text;
                m_objSelectedContact.MiddleName = txtMiddleName.Text;
                m_objSelectedContact.Department = (cboxDepartment.SelectedItem == null) ? null : (CDepartament)cboxDepartment.SelectedItem;
                m_objSelectedContact.JobPosition = (cboxJobPosition.SelectedItem == null) ? null : (CJobPosition)cboxJobPosition.SelectedItem;
                m_objSelectedContact.Description = txtDescription.Text;
                m_objSelectedContact.NickName = txtNickName.Text;
                m_objSelectedContact.WWW = txtWebPage.Text;
                if ((dtBirthday.Text != "") && (dtBirthday.DateTime != System.DateTime.MinValue))
                {
                    m_objSelectedContact.Birthday = dtBirthday.DateTime;
                }
                if (m_objSelectedContact.EMailList == null)
                {
                    m_objSelectedContact.EMailList = new List<CEMail>();
                }
                else
                {
                    m_objSelectedContact.EMailList.Clear();
                }
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListEMail.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    m_objSelectedContact.EMailList.Add((CEMail)objNode.Tag);
                }
                if (m_objSelectedContact.PhoneList == null)
                {
                    m_objSelectedContact.PhoneList = new List<CPhone>();
                }
                else
                {
                    m_objSelectedContact.PhoneList.Clear();
                }
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListPhone.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    m_objSelectedContact.PhoneList.Add((CPhone)objNode.Tag);
                }
                if (frmAddress.AddressList != null)
                {
                    m_objSelectedContact.AddressList = frmAddress.AddressList;
                }
                m_objSelectedContact.AddressForDeleteList = frmAddress.AddressDeletedList;

                System.String strErr = "";
                if (m_objSelectedContact.IsAllParametersValid(ref strErr) == true)
                {
                    treeListContacts.FocusedNode.SetValue(colContactName, m_objSelectedContact.VisitingCard2);

                    btnAddContact.Enabled = true;
                    btnEditContact.Enabled = true;
                    btnDeleteContact.Enabled = true;
                    btnCancel.Enabled = false;
                    btnSave.Enabled = false;

                    SetReadOnlyPropertiesControls(true);
                    bRet = true;
                }
                else
                {
                    SendMessageToLog("Ошибка сохранения изменений в описании контакта. Текст ошибки: " + strErr);
                }

                bRet = true;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return bRet;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ConfirmChanges();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return ;
        }
        public void ConfirmChanges()
        {
            try
            {
                if (m_objSelectedContact == null) { return; }
                if (m_bIsChanged == false) { return; }
                

                frmAddress.ConfirmChanges();
                if (bSaveChanges2() == true)
                {
                    SimulateChangeContactProperties(m_objSelectedContact, enumActionSaveCancel.Save, m_bNewContact);
                    m_bNewContact = false;
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений. Текст ошибки: " + f.Message);
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
                    "SendMessageToLog.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        #endregion

        #region Потоки

 
        #endregion

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
        private void ctrlContact_Resize(object sender, EventArgs e)
        {
            layoutControlAddress.MaximumSize = new Size(this.Size.Width, layoutControlAddress.MaximumSize.Height);
            layoutControlDscrpn.MaximumSize = new Size(this.Size.Width, layoutControlDscrpn.MaximumSize.Height);
            layoutControlInternet.MaximumSize = new Size(this.Size.Width, layoutControlInternet.MaximumSize.Height);
            layoutControlJob.MaximumSize = new Size(this.Size.Width, layoutControlJob.MaximumSize.Height);
            layoutControlPhone.MaximumSize = new Size(this.Size.Width, layoutControlPhone.MaximumSize.Height);
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
                SendMessageToLog("treeListPhone_CellValueChanged.\nТекст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
             
        }




        private void treeListEMail_CellValueChanging(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
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
                            //m_objSelectedContact.EMailList[iPosNode].EMail = (System.String)e.Value;
                            ((CEMail)e.Node.Tag).EMail = (System.String)e.Value;
                            e.Node.SetValue(colEMailAddress, e.Value);
                        }
                    }
                }

                // Здесь...
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
                            e.Node.SetValue(colIsMainEMail, e.Value);
                            //m_objSelectedContact.EMailList[i].IsMain = false;
                        }
                    }
                    //m_objSelectedContact.EMailList[iPosNode].IsMain = true;
                    ((CEMail)e.Node.Tag).IsMain = true;
                    e.Node.SetValue(colIsActiveEMail, true);
                    e.Node.SetValue(colIsMainEMail, true);
                }
                if ((e.Column == colIsActiveEMail) && (e.Value != null))
                {
                    //m_objSelectedContact.EMailList[iPosNode].IsActive = (System.Boolean)e.Value;
                    ((CEMail)e.Node.Tag).IsActive = (System.Boolean)e.Value;
                    e.Node.SetValue(colIsActiveEMail, e.Value);
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
    }

    /// <summary>
    /// Тип, хранящий информацию, которая передается получателям уведомления о событии
    /// </summary>
    public partial class ChangeContactPropertieEventArgs : EventArgs
    {
        private readonly CContact m_objContact;
        public CContact Contact
        { get { return m_objContact; } }

        private readonly enumActionSaveCancel m_enActionType;
        public enumActionSaveCancel ActionType
        { get { return m_enActionType; } }

        private readonly System.Boolean m_bIsNewContact;
        public System.Boolean IsNewContact
        { get { return m_bIsNewContact; } }

        public ChangeContactPropertieEventArgs(CContact objContact, enumActionSaveCancel enActionType, System.Boolean bIsNewContact)
        {
            m_objContact = objContact;
            m_enActionType = enActionType;
            m_bIsNewContact = bIsNewContact;
        }
    }

}
