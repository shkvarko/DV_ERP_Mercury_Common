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
    public partial class ctrlAddress : UserControl
    {
        #region Свойства, переменные
        private EnumObject m_enObjectWithAddress;
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;
        public List<CAddress> AddressList
        {
            get
            {
                List<CAddress> objAddressList = new List<CAddress>();
                try
                {
                    foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListAddress.Nodes)
                    {
                        if (objNode.Tag != null)
                        {
                            objAddressList.Add((CAddress)objNode.Tag);
                        }
                    }
                }
                catch
                {
                    objAddressList = null;
                }
                return objAddressList;
            }
        }
        private List<CAddress> m_objAddressDeletedList;
        public List<CAddress> AddressDeletedList
        {
            get { return m_objAddressDeletedList; }
            set { m_objAddressDeletedList = value; }
        }
        private CAddress m_objSelectedAddress;
        private System.Guid m_uuidOwnerId;
        private List<CCountry> m_objCountryList;
        private List<COblast> m_objOblastList;
        private List<CRegion> m_objRegionList;
        private List<CCity> m_objCityList;
        private System.Boolean m_bNewAddress;
        private System.Boolean m_bIsChanged;
        public System.Boolean IsChanged
        {
            get { return m_bIsChanged; }
        }
        private System.Boolean m_bIsReadOnly;
        /// <summary>
        /// блокировка возможности включения режима редактирования
        /// </summary>
        private System.Boolean m_bBlockChanceEdit;
        private System.Boolean m_bDisableEvents;
        private System.Boolean m_bDisableTreeListEvents;
        private System.Boolean m_bInitAllLists;
        public System.Boolean AllListsIsLoad { get { return m_bInitAllLists; } }
        private const System.String strAllRegionName = "Все регионы";
        private const System.Int32 iPanel1WidthDef = 200;
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
        public delegate void LoadAddressDelegate();
        public LoadAddressDelegate m_LoadAddressDelegate;

        private const System.Int32 iThreadSleepTime = 1000;
        private System.Boolean m_bThreadFinishJob;
        #endregion

        #region События
        // Создаем закрытое поле, ссылающееся на заголовок списка делегатов
        private EventHandler<ChangeAddressPropertieEventArgs> m_ChangeAddressPropertie;
        // Создаем в классе член-событие
        public event EventHandler<ChangeAddressPropertieEventArgs> ChangeAddressPropertie
        {
            add
            {
                // берем закрытую блокировку и добавляем обработчик
                // (передаваемый по значению) в список делегатов
                m_ChangeAddressPropertie += value;
            }
            remove
            {
                // берем закрытую блокировку и удаляем обработчик
                // (передаваемый по значению) из списка делегатов
                m_ChangeAddressPropertie -= value;
            }
        }
        /// <summary>
        /// Инициирует событие и уведомляет о нем зарегистрированные объекты
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnChangeAddressPropertie(ChangeAddressPropertieEventArgs e)
        {
            // Сохраняем поле делегата во временном поле для обеспечение безопасности потока
            EventHandler<ChangeAddressPropertieEventArgs> temp = m_ChangeAddressPropertie;
            // Если есть зарегистрированные объектв, уведомляем их
            if (temp != null) temp(this, e);
        }
        public void SimulateChangeAddressPropertie(CAddress objAddress, enumActionSaveCancel enActionType, System.Boolean bIsNewAddress)
        {
            // Создаем объект, хранящий информацию, которую нужно передать
            // объектам, получающим уведомление о событии
            ChangeAddressPropertieEventArgs e = new ChangeAddressPropertieEventArgs(objAddress, enActionType, bIsNewAddress);

            // Вызываем виртуальный метод, уведомляющий наш объект о возникновении события
            // Если нет типа, переопределяющего этот метод, наш объект уведомит все объекты, 
            // подписавшиеся на уведомление о событии
            OnChangeAddressPropertie(e);
        }
        #endregion

        #region Конструктор
        public ctrlAddress(EnumObject enObjectWithAddress, UniXP.Common.CProfile objProfile,
            UniXP.Common.MENUITEM objMenuItem, System.Guid uuidOwnerId)
        {
            InitializeComponent();

            m_enObjectWithAddress = enObjectWithAddress;
            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_uuidOwnerId = uuidOwnerId;
            m_bIsChanged = false;
            m_bNewAddress = false;
            m_bIsReadOnly = false;
            m_bBlockChanceEdit = false;

            m_objAddressDeletedList = new List<CAddress>();
            m_objCountryList = null;
            m_objOblastList = null;
            m_objRegionList = null;
            m_objCityList = null;

            m_objSelectedAddress = null;
            m_bInitAllLists = false;
            m_bThreadFinishJob = false;

            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            m_bDisableTreeListEvents = false;
            btnSelectAddress.Visible = (m_enObjectWithAddress == EnumObject.Rtt);

            //StartThreadWithLoadData();
            
            //LoadAllLists();

            //InitAllLists();
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            m_bDisableEvents = false;
            m_bDisableTreeListEvents = false; 
            btnSelectAddress.Visible = (m_enObjectWithAddress == EnumObject.Rtt);
        }
        #endregion

        #region Загрузить список адресов
        /// <summary>
        /// Загружает список адресов в дерево
        /// </summary>
        /// <param name="uuidOwnerId">идентификатор владельца адресов</param>
        /// <param name="objAddressList">список адресов</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public System.Boolean LoadAddressList(System.Guid uuidOwnerId, List<CAddress> objAddressListSrc)
        {
            System.Boolean bRet = false;
            try
            {
                m_bDisableEvents = true;
                treeListAddress.Nodes.Clear();
                
                m_uuidOwnerId = uuidOwnerId;

                if( (objAddressListSrc == null) || (objAddressListSrc.Count == 0))
                {
                    List<CAddress> objAddressList = CAddress.GetAddressList(m_objProfile, null, m_enObjectWithAddress, m_uuidOwnerId);
                    if (objAddressList != null)
                    {
                        foreach (CAddress objAddress in objAddressList)
                        {
                            DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListAddress.AppendNode(new object[] { objAddress.VisitingCard2 }, null);
                            objNode.Tag = objAddress;
                        }
                    }
                    objAddressList = null;
                }
                else
                {
                    foreach (CAddress objAddress in objAddressListSrc)
                    {
                        DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListAddress.AppendNode(new object[] { objAddress.VisitingCard2 }, null);
                        objNode.Tag = objAddress;
                    }
                }

                if (treeListAddress.Nodes.Count > 0)
                {
                    treeListAddress.FocusedNode = treeListAddress.Nodes[0];
                    ShowAddressDetail((CAddress)treeListAddress.FocusedNode.Tag);
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
                m_bDisableEvents = false;
                m_bDisableTreeListEvents = false;
            }

            return bRet;
        }
        public void SetChanceEditProperties(System.Boolean bChance)
        {
            try
            {
                if (bChance == false)
                {
                    // отключить возможнось войти в режим редактирования
                    m_bBlockChanceEdit = true;
                    btnAddAddress.Enabled = bChance;
                    btnEditAddress.Enabled = bChance;
                    btnDeleteAddress.Enabled = bChance;

                    btnCancel.Enabled = bChance;
                    btnSave.Enabled = bChance;
                }
                else
                {
                    // включить возможнось войти в режим редактирования
                    m_bBlockChanceEdit = false;
                    SetReadOnlyPropertiesControls(true);
                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("SetChanceEditProperties.\n\nТекст ошибки:\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return ;
        }
        /// <summary>
        /// Включает/отключает элементы управления для отображения свойств адреса
        /// </summary>
        /// <param name="bEnable">признак "включить/выключить"</param>
        private void SetReadOnlyPropertiesControls(System.Boolean bEnable)
        {
            try
            {
                cboxCountry.Properties.ReadOnly = bEnable;
                cboxOblast.Properties.ReadOnly = bEnable;
                cboxRegion.Properties.ReadOnly = bEnable;
                cboxCity.Properties.ReadOnly = bEnable;
                txtPostIndex.Properties.ReadOnly = bEnable;
                txtBuildingCode.Properties.ReadOnly = bEnable;
                txtSubBuildingCode.Properties.ReadOnly = bEnable;
                txtFlatCode.Properties.ReadOnly = bEnable;
                cboxAddressPrefix.Properties.ReadOnly = bEnable;
                cboxAddressType.Properties.ReadOnly = bEnable;
                cboxBuilding.Properties.ReadOnly = bEnable;
                cboxSubBuilding.Properties.ReadOnly = bEnable;
                cboxFlat.Properties.ReadOnly = bEnable;
                cboxStreet.Properties.ReadOnly = bEnable;
                txtDescription.Properties.ReadOnly = bEnable;

                m_bIsReadOnly = bEnable;
                if (bEnable == true)
                {
                    // включен режим "только просмотр"
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnAddAddress.Enabled = true;
                    btnEditAddress.Enabled = (treeListAddress.FocusedNode != null);
                    btnDeleteAddress.Enabled = (treeListAddress.FocusedNode != null);
                    splitContainerControl.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
                }
                else
                {
                    // включен режим "редактирование"
                    btnSave.Enabled = m_bIsChanged;
                    btnCancel.Enabled = true;
                    btnAddAddress.Enabled = false;
                    btnEditAddress.Enabled = !m_bIsChanged;
                    btnDeleteAddress.Enabled = false;
                    splitContainerControl.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel2;
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("DisableEnablePropertiesControls. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return ;
        }
        /// <summary>
        /// Отображает в режиме просмотра свойства адреса в элементах управления
        /// </summary>
        /// <param name="objAddress">адрес</param>
        private void ShowAddressDetail(CAddress objAddress)
        {
            m_bDisableEvents = true;
            try
            {
                if (objAddress != null) { m_objSelectedAddress = objAddress; }
                LoadAddressPropertiesInControls(m_objSelectedAddress);

                SetPropertiesModified(false);
                SetReadOnlyPropertiesControls(true);
                if (m_bBlockChanceEdit == true)
                {
                    //родительский элемент еще не отменял блокировку возможности включить режим редактирования
                    SetChanceEditProperties(false);
                }
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка отображения свойств адреса.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                m_bDisableEvents = false;
            }

            return;
        }
        private void LoadAddressPropertiesInControls(CAddress objAddress)
        {
            // страна
            cboxCountry.Properties.Items.Clear();
            cboxCountry.Properties.Items.Add(objAddress.City.Region.Oblast.Country);
            cboxCountry.SelectedItem = cboxCountry.Properties.Items[0];
            // область
            cboxOblast.Properties.Items.Clear();
            cboxOblast.Properties.Items.Add(objAddress.City.Region.Oblast);
            cboxOblast.SelectedItem = cboxOblast.Properties.Items[0];
            // район
            cboxRegion.Properties.Items.Clear();
            cboxRegion.Properties.Items.Add(objAddress.City.Region);
            cboxRegion.SelectedItem = cboxRegion.Properties.Items[0];
            // населенный пункт
            cboxCity.Properties.Items.Clear();
            cboxCity.Properties.Items.Add(objAddress.City);
            cboxCity.SelectedItem = cboxCity.Properties.Items[0];
            // улица
            cboxStreet.Properties.Items.Clear();
            if (objAddress.Name != "")
            {
                cboxStreet.Properties.Items.Add(objAddress.Name);
                cboxStreet.SelectedItem = cboxStreet.Properties.Items[0];
            }
            // тип адреса
            foreach (object objAddressType in cboxAddressType.Properties.Items)
            {
                if (((CAddressType)objAddressType).ID.CompareTo(objAddress.AddressType.ID) == 0)
                {
                    cboxAddressType.SelectedItem = objAddressType;
                    break;
                }
            }
            // тип улицы
            if (objAddress.AddressPrefix != null)
            {
                foreach (object objAddressPrefix in cboxAddressPrefix.Properties.Items)
                {
                    if (((CAddressPrefix)objAddressPrefix).ID.CompareTo(objAddress.AddressPrefix.ID) == 0)
                    {
                        cboxAddressPrefix.SelectedItem = objAddressPrefix;
                        break;
                    }
                }
            }
            // тип строения
            if (objAddress.Building != null)
            {
                foreach (object objBuilding in cboxBuilding.Properties.Items)
                {
                    if (((CBuilding)objBuilding).ID.CompareTo(objAddress.Building.ID) == 0)
                    {
                        cboxBuilding.SelectedItem = objBuilding;
                        break;
                    }
                }
            }
            txtBuildingCode.Text = "";
            txtBuildingCode.Text = objAddress.BuildCode;
            // тип подстроения
            txtSubBuildingCode.Text = "";
            if (objAddress.SubBuilding != null)
            {
                foreach (object objSubBuilding in cboxSubBuilding.Properties.Items)
                {
                    if (((CSubBuilding)objSubBuilding).ID.CompareTo(objAddress.SubBuilding.ID) == 0)
                    {
                        cboxSubBuilding.SelectedItem = objSubBuilding;
                        break;
                    }
                }
                txtSubBuildingCode.Text = objAddress.SubBuildCode;
            }
            // помещение
            txtFlatCode.Text = "";
            if (objAddress.Flat != null)
            {
                foreach (object objFlat in cboxFlat.Properties.Items)
                {
                    if (((CFlat)objFlat).ID.CompareTo(objAddress.Flat.ID) == 0)
                    {
                        cboxFlat.SelectedItem = objFlat;
                        break;
                    }
                }
                txtFlatCode.Text = objAddress.FlatCode;
            }
            // почтовый индекс
            txtPostIndex.Text = "";
            txtPostIndex.Text = objAddress.PostIndex;
            // примечание
            txtDescription.Text = objAddress.Description;
            return;
        }

        private void treeListAddress_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            try
            {
                if (m_bDisableTreeListEvents == true) { return; }
                if ((e.Node == null) || (e.Node.Tag == null) || (e.Node.TreeList.Nodes.Count == 0)) { return; }
                ShowAddressDetail((CAddress)e.Node.Tag);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка смены адреса.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                SetPropertiesModified(false);
            }

            return;
        }
        private void SelectAddress()
        {
            try
            {
                if (m_objSelectedAddress == null) { return; }
                //if (m_bIsChanged == false) { return; }

                frmSelectAddressForRtt obj_frmSelectAddressForRtt = new frmSelectAddressForRtt(m_objProfile, m_uuidOwnerId);
                if (obj_frmSelectAddressForRtt != null)
                {
                    obj_frmSelectAddressForRtt.LoadAddressList();
                    DialogResult objDlg = obj_frmSelectAddressForRtt.ShowDialog();
                    if( (objDlg == DialogResult.OK) && (obj_frmSelectAddressForRtt.AddressSelected != null))
                    {
                        LoadAddressPropertiesInControls(obj_frmSelectAddressForRtt.AddressSelected);
                    }
                    obj_frmSelectAddressForRtt = null;
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка смены адреса.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                SetPropertiesModified(false);
            }

            return;
        }
        private void btnSelectAddress_Click(object sender, EventArgs e)
        {
            try
            {
                SelectAddress();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnSelectAddress_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private void treeListAddress_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (m_bIsChanged == true) { return; }
                if (e.Button == MouseButtons.Right)
                {
                    // попробуем определить, что же у нас под мышкой
                    DevExpress.XtraTreeList.TreeListHitInfo hi = ((DevExpress.XtraTreeList.TreeList)sender).CalcHitInfo(new Point(e.X, e.Y));
                    if ((hi != null) && (hi.Node != null))
                    {
                        // выделяем узел
                        hi.Node.TreeList.FocusedNode = hi.Node;
                    }
                    menuAdd.Enabled = btnAddAddress.Enabled;
                    menuDelete.Enabled = btnDeleteAddress.Enabled;
                    menuEdit.Enabled = btnEditAddress.Enabled;

                    contextMenuStrip.Show(((DevExpress.XtraTreeList.TreeList)sender), new Point(e.X, e.Y));
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListEMail_MouseClick. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;

        }

        private void treeListAddress_BeforeFocusNode(object sender, DevExpress.XtraTreeList.BeforeFocusNodeEventArgs e)
        {
            try
            {
                if (m_bDisableTreeListEvents == true) { return; } 
                if ((m_bIsChanged == true) && (e.Node != null))
                {
                    DialogResult dlgRes = DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Адрес был изменен. Подтвердить изменения?", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question);
                    switch (dlgRes)
                    {
                        case DialogResult.Yes:
                            {
                                e.CanFocus = SaveChanges();
                                break;
                            }
                        case DialogResult.No:
                            {
                                e.CanFocus = true;
                                break;
                            }
                        case DialogResult.Cancel:
                            {
                                e.CanFocus = false;
                                break;
                            }
                        default:
                            {
                                e.CanFocus = false;
                                break;
                            }
                    }
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

        #region Потоки

        public void InitAddressControl()
        {
           try
           {
               if ((thrAddress != null) && (thrAddress.IsAlive))
               {
                   thrAddress.Abort();
                   while (!thrAddress.Join(0))
                       Application.DoEvents();
               }

               thrAddress = null;
               thrAddress = new System.Threading.Thread(LoadAllLists);
               thrAddress.IsBackground = true;

               thrAddress.Start();
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

        public void StartThreadWithLoadData()
        {
            try
            {
                // инициализируем события
                this.m_EventStopThread = new System.Threading.ManualResetEvent(false);
                this.m_EventThreadStopped = new System.Threading.ManualResetEvent(false);

                // инициализируем делегаты
                m_LoadAddressDelegate = new LoadAddressDelegate(LoadAllLists);

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
                LoadAllLists2();

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

        private void LoadAllLists2()
        {
            try
            {
                this.Invoke(m_LoadAddressDelegate);
                return;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadAllLists2 " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            return;
        }

        private void LoadAllLists()
        {
            try
            {
                if (m_bInitAllLists == true) { return; }
                m_bDisableEvents = true;
                m_objCountryList = CCountry.GetCountryList(m_objProfile, null);
                m_objOblastList = COblast.GetOblastList(m_objProfile, null);
                m_objRegionList = CRegion.GetRegionList(m_objProfile, null);
                m_objCityList = CCity.GetCityList(m_objProfile, null, true);
                //m_objRegionList.Insert( 0, new CRegion( System.Guid.Empty, strAllRegionName, "", null ) );

                List<CAddressType> objAddressTypeList = CAddressType.GetAddressTypeList(m_objProfile, null);
                List<CAddressPrefix> objAddressPrefixList = CAddressPrefix.GetAddressPrefixList(m_objProfile, null);
                List<CBuilding> objBuildingList = CBuilding.GetBuildingList(m_objProfile, null);
                List<CSubBuilding> objSubBuildingList = CSubBuilding.GetSubBuildingList(m_objProfile, null);
                List<CFlat> objFlatList = CFlat.GetFlatList(m_objProfile, null);

                cboxAddressType.Properties.Items.Clear();
                if (objAddressTypeList != null)
                {
                    cboxAddressType.Properties.Items.AddRange(objAddressTypeList);
                }

                cboxAddressPrefix.Properties.Items.Clear();
                if (objAddressPrefixList != null)
                {
                    cboxAddressPrefix.Properties.Items.AddRange(objAddressPrefixList);
                }

                cboxBuilding.Properties.Items.Clear();
                if (objBuildingList != null)
                {
                    cboxBuilding.Properties.Items.AddRange(objBuildingList);
                }

                cboxSubBuilding.Properties.Items.Clear();
                if (objSubBuildingList != null)
                {
                    cboxSubBuilding.Properties.Items.AddRange(objSubBuildingList);
                }

                cboxFlat.Properties.Items.Clear();
                if (objFlatList != null)
                {
                    cboxFlat.Properties.Items.AddRange(objFlatList);
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка обновления выпадающих списков.\n\nТекст ошибки:\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                m_bDisableEvents = false;
                m_bInitAllLists = true;

            }

            return;
        }
        /// <summary>
        /// Обновление выпадающих списков
        /// </summary>
        /// <param name="objCountryList">страны</param>
        /// <param name="objOblastList">области</param>
        /// <param name="objRegionList">районы</param>
        /// <param name="objCityList">населённые пункты</param>
        /// <param name="objAddressTypeList">типы адресов</param>
        /// <param name="objAddressPrefixList">типы улиц</param>
        /// <param name="objBuildingList">типы строений</param>
        /// <param name="objSubBuildingList">типы корпусов строений</param>
        /// <param name="objFlatList">типы помещений</param>
        public void LoadComboBox(List<CCountry> objCountryList, List<COblast> objOblastList, List<CRegion> objRegionList, List<CCity> objCityList,
           List<CAddressType> objAddressTypeList, List<CAddressPrefix> objAddressPrefixList, List<CBuilding> objBuildingList,
           List<CSubBuilding> objSubBuildingList, List<CFlat> objFlatList)
        {
            m_bDisableEvents = true;
            try
            {
                m_objCountryList = objCountryList;
                m_objOblastList = objOblastList;
                m_objRegionList = objRegionList;
                m_objCityList = objCityList;

                cboxAddressType.Properties.Items.Clear();
                if (objAddressTypeList != null)
                {
                    cboxAddressType.Properties.Items.AddRange(objAddressTypeList);
                }

                cboxAddressPrefix.Properties.Items.Clear();
                if (objAddressPrefixList != null)
                {
                    cboxAddressPrefix.Properties.Items.AddRange(objAddressPrefixList);
                }

                cboxBuilding.Properties.Items.Clear();
                if (objBuildingList != null)
                {
                    cboxBuilding.Properties.Items.AddRange(objBuildingList);
                }

                cboxSubBuilding.Properties.Items.Clear();
                if (objSubBuildingList != null)
                {
                    cboxSubBuilding.Properties.Items.AddRange(objSubBuildingList);
                }

                cboxFlat.Properties.Items.Clear();
                if (objFlatList != null)
                {
                    cboxFlat.Properties.Items.AddRange(objFlatList);
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка обновления выпадающих списков.\nLoadComboBox\n\nТекст ошибки:\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                m_bDisableEvents = false;
                m_bInitAllLists = true;
            }
            return;
        }

        #endregion


        #region Инициализация и фильтрация выпадающих списков
        /// <summary>
        /// Создает необходимые списки и заполняет их значениями
        /// </summary>
        public void InitAllLists()
        {
            try
            {
                if (m_bInitAllLists == true) { return; }
                m_bDisableEvents = true;
                m_objCountryList = CCountry.GetCountryList( m_objProfile, null );
                m_objOblastList = COblast.GetOblastList(m_objProfile, null);
                m_objRegionList = CRegion.GetRegionList( m_objProfile, null );
                m_objCityList = CCity.GetCityList( m_objProfile, null, true );
                //m_objRegionList.Insert( 0, new CRegion( System.Guid.Empty, strAllRegionName, "", null ) );

                List<CAddressType> objAddressTypeList = CAddressType.GetAddressTypeList(m_objProfile, null);
                List<CAddressPrefix> objAddressPrefixList = CAddressPrefix.GetAddressPrefixList(m_objProfile, null);
                List<CBuilding> objBuildingList = CBuilding.GetBuildingList(m_objProfile, null);
                List<CSubBuilding> objSubBuildingList = CSubBuilding.GetSubBuildingList(m_objProfile, null);
                List<CFlat> objFlatList = CFlat.GetFlatList(m_objProfile, null);

                cboxAddressType.Properties.Items.Clear();
                if (objAddressTypeList != null)
                {
                    cboxAddressType.Properties.Items.AddRange(objAddressTypeList);

                    //foreach (CAddressType objAddressType in objAddressTypeList)
                    //{
                    //    cboxAddressType.Properties.Items.Add(objAddressType);
                    //}
                }

                cboxAddressPrefix.Properties.Items.Clear();
                if (objAddressPrefixList != null)
                {
                    cboxAddressPrefix.Properties.Items.AddRange(objAddressPrefixList);

                    //foreach (CAddressPrefix objAddressPrefix in objAddressPrefixList)
                    //{
                    //    cboxAddressPrefix.Properties.Items.Add(objAddressPrefix);
                    //}
                }

                cboxBuilding.Properties.Items.Clear();
                if (objBuildingList != null)
                {
                    cboxBuilding.Properties.Items.AddRange(objBuildingList);

                    //foreach (CBuilding objBuilding in objBuildingList)
                    //{
                    //    cboxBuilding.Properties.Items.Add(objBuilding);
                    //}
                }

                cboxSubBuilding.Properties.Items.Clear();
                if (objSubBuildingList != null)
                {
                    cboxSubBuilding.Properties.Items.AddRange(objSubBuildingList);
                    
                    //foreach (CSubBuilding objSubBuilding in objSubBuildingList)
                    //{
                    //    cboxSubBuilding.Properties.Items.Add(objSubBuilding);
                    //}
                }

                cboxFlat.Properties.Items.Clear();
                if (objFlatList != null)
                {
                    cboxFlat.Properties.Items.AddRange(objFlatList);

                    //foreach (CFlat objFlat in objFlatList)
                    //{
                    //    cboxFlat.Properties.Items.Add(objFlat);
                    //}
                }

                //objAddressTypeList =  null;
                //objAddressPrefixList = null;
                //objBuildingList = null;
                //objSubBuildingList = null;
                //objFlatList = null;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка обновления выпадающих списков.\n\nТекст ошибки:\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                m_bDisableEvents = false;
                m_bInitAllLists = true;
            }

            return ;
        }
        /// <summary>
        /// Фильтрация списка стран
        /// </summary>
        /// <param name="bSuspend">признак "отключить элемент управления"</param>
        private void LoadCountryList(System.Boolean bSuspend)
        {
            if (m_objCountryList == null) { return; }
            try
            {
                cboxOblast.EditValueChanged -= new EventHandler(cboxOblast_EditValueChanged);
                cboxRegion.EditValueChanged -= new EventHandler(cboxRegion_EditValueChanged);
                cboxCity.EditValueChanged -= new EventHandler(cboxCity_EditValueChanged);
                if (bSuspend == true)
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    ((System.ComponentModel.ISupportInitialize)(this.cboxCountry.Properties)).BeginInit();
                    ((System.ComponentModel.ISupportInitialize)(this.cboxOblast.Properties)).BeginInit();
                    ((System.ComponentModel.ISupportInitialize)(this.cboxRegion.Properties)).BeginInit();
                    ((System.ComponentModel.ISupportInitialize)(this.cboxCity.Properties)).BeginInit();
                    ((System.ComponentModel.ISupportInitialize)(this.cboxStreet.Properties)).BeginInit();
                    this.SuspendLayout();
                }

                cboxOblast.Properties.Items.Clear();
                cboxOblast.EditValue = null;
                cboxRegion.Properties.Items.Clear();
                cboxRegion.EditValue = null;
                cboxStreet.Properties.Items.Clear();
                cboxStreet.EditValue = null;
                cboxCity.Properties.Items.Clear();
                cboxCity.EditValue = null;

                if (m_objCountryList.Count > 0)
                {
                    cboxCountry.Properties.Items.AddRange(m_objCountryList);

                    //foreach (CCountry objCountry in m_objCountryList)
                    //{
                    //    cboxCountry.Properties.Items.Add(objCountry);
                    //}
                    cboxCountry.SelectedItem = cboxCountry.Properties.Items[0];

                    // страна выбрана, загружаем список областей
                    LoadOblastListForCountry((CCountry)cboxCountry.SelectedItem, false);

                    //if ((cboxRegion.Properties.Items.Count > 0) &&
                    //    (((CRegion)cboxRegion.Properties.Items[0]).Name == strAllRegionName))
                    //{
                    //    cboxRegion.SelectedItem = (CRegion)cboxRegion.Properties.Items[0];
                    //    LoadCityListForRegion((CRegion)cboxRegion.Properties.Items[0], false);
                    //}
                }
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка построения списка стран.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                if (bSuspend == true)
                {
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    ((System.ComponentModel.ISupportInitialize)(this.cboxCountry.Properties)).EndInit();
                    ((System.ComponentModel.ISupportInitialize)(this.cboxOblast.Properties)).EndInit();
                    ((System.ComponentModel.ISupportInitialize)(this.cboxRegion.Properties)).EndInit();
                    ((System.ComponentModel.ISupportInitialize)(this.cboxStreet.Properties)).EndInit();
                    ((System.ComponentModel.ISupportInitialize)(this.cboxCity.Properties)).EndInit();
                    this.ResumeLayout(false);
                }
                cboxOblast.EditValueChanged += new EventHandler(cboxOblast_EditValueChanged);
                cboxRegion.EditValueChanged += new EventHandler(cboxRegion_EditValueChanged);
                cboxCity.EditValueChanged += new EventHandler(cboxCity_EditValueChanged);
            }

            return;
        }
        /// <summary>
        /// Фильтрация списка регионов для страны
        /// </summary>
        /// <param name="objCountry">страна</param>
        /// <param name="bSuspend">признак "отключить элемент управления"</param>
        private void LoadOblastListForCountry(CCountry objCountry, System.Boolean bSuspend)
        {
            if (objCountry == null) { return; }
            try
            {
                cboxRegion.EditValueChanged -= new EventHandler(cboxRegion_EditValueChanged);
                cboxCity.EditValueChanged -= new EventHandler(cboxCity_EditValueChanged);
                if (bSuspend == true)
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    ((System.ComponentModel.ISupportInitialize)(this.cboxOblast.Properties)).BeginInit();
                    ((System.ComponentModel.ISupportInitialize)(this.cboxRegion.Properties)).BeginInit();
                    ((System.ComponentModel.ISupportInitialize)(this.cboxCity.Properties)).BeginInit();
                    ((System.ComponentModel.ISupportInitialize)(this.cboxStreet.Properties)).BeginInit();
                    this.SuspendLayout();
                }

                cboxOblast.Properties.Items.Clear();
                cboxOblast.EditValue = null;
                cboxRegion.Properties.Items.Clear();
                cboxRegion.EditValue = null;
                cboxCity.Properties.Items.Clear();
                cboxCity.EditValue = null;
                cboxStreet.Properties.Items.Clear();
                cboxStreet.EditValue = null;
                txtPostIndex.Text = "";

                if ((m_objOblastList != null) && (m_objOblastList.Count > 0))
                {
                    foreach (COblast objOblast in m_objOblastList)
                    {
                        if (objOblast.Country.ID.CompareTo(objCountry.ID) == 0)
                        {
                            cboxOblast.Properties.Items.Add(objOblast);
                        }
                    }
                }
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка построения списка областей.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                if (bSuspend == true)
                {
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    ((System.ComponentModel.ISupportInitialize)(this.cboxOblast.Properties)).EndInit();
                    ((System.ComponentModel.ISupportInitialize)(this.cboxRegion.Properties)).EndInit();
                    ((System.ComponentModel.ISupportInitialize)(this.cboxStreet.Properties)).EndInit();
                    ((System.ComponentModel.ISupportInitialize)(this.cboxCity.Properties)).EndInit();
                    this.ResumeLayout(false);
                }
                cboxCity.EditValueChanged += new EventHandler(cboxCity_EditValueChanged);
                cboxRegion.EditValueChanged += new EventHandler(cboxRegion_EditValueChanged);
            }

            return;
        }
        /// <summary>
        /// Фильтрация списка районов для области
        /// </summary>
        /// <param name="objOblast">область</param>
        /// <param name="bSuspend">признак "отключить элемент управления"</param>
        private void LoadRegionListForOblast(COblast objOblast, System.Boolean bSuspend)
        {
            if (objOblast == null) { return; }
            try
            {
                cboxCity.EditValueChanged -= new EventHandler(cboxCity_EditValueChanged);
                if (bSuspend == true)
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    ((System.ComponentModel.ISupportInitialize)(this.cboxRegion.Properties)).BeginInit();
                    ((System.ComponentModel.ISupportInitialize)(this.cboxCity.Properties)).BeginInit();
                    ((System.ComponentModel.ISupportInitialize)(this.cboxStreet.Properties)).BeginInit();
                    this.SuspendLayout();
                }

                cboxRegion.Properties.Items.Clear();
                cboxRegion.EditValue = null;
                cboxStreet.Properties.Items.Clear();
                cboxStreet.EditValue = null;
                cboxCity.Properties.Items.Clear();
                cboxCity.EditValue = null;
                txtPostIndex.Text = "";

                if ((m_objRegionList != null) && (m_objRegionList.Count > 0))
                {
                    List<CRegion> objRegionList = new List<CRegion>();

                    foreach (CRegion objRegion in m_objRegionList)
                    {
                        if (objRegion.Oblast.ID.CompareTo(objOblast.ID) == 0)
                        {
                            objRegionList.Add(objRegion);
                            //cboxRegion.Properties.Items.Add(objRegion);
                        }
                        //if (objRegion.Name == strAllRegionName)
                        //{
                        //    cboxRegion.Properties.Items.Add(objRegion);
                        //}
                        //else
                        //{
                        //    if (objRegion.Oblast.ID.CompareTo(objOblast.ID) == 0)
                        //    {
                        //        cboxRegion.Properties.Items.Add(objRegion);
                        //    }
                        //}
                    }

                    cboxRegion.Properties.Items.AddRange(objRegionList);
                }
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка построения списка районов.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                if (bSuspend == true)
                {
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    ((System.ComponentModel.ISupportInitialize)(this.cboxRegion.Properties)).EndInit();
                    ((System.ComponentModel.ISupportInitialize)(this.cboxStreet.Properties)).EndInit();
                    ((System.ComponentModel.ISupportInitialize)(this.cboxCity.Properties)).EndInit();
                    this.ResumeLayout(false);
                }
                cboxCity.EditValueChanged += new EventHandler(cboxCity_EditValueChanged);
            }

            return;
        }
        /// <summary>
        /// Фильтрация списка городов для заданного района
        /// </summary>
        /// <param name="objRegion">район</param>
        /// <param name="bSuspend">признак "отключить элемент управления"</param>
        private void LoadCityListForRegion(CRegion objRegion, System.Boolean bSuspend)
        {
            if (objRegion == null) { return; }
            try
            {
                if (bSuspend == true)
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    ((System.ComponentModel.ISupportInitialize)(this.cboxCity.Properties)).BeginInit();
                    ((System.ComponentModel.ISupportInitialize)(this.cboxStreet.Properties)).BeginInit();
                    this.SuspendLayout();
                }

                cboxStreet.Properties.Items.Clear();
                cboxStreet.EditValue = null;
                cboxCity.Properties.Items.Clear();
                cboxCity.EditValue = null;

                if ((m_objCityList != null) && (m_objCityList.Count > 0))
                {
                    if (objRegion.Name == strAllRegionName)
                    {
                        cboxCity.Properties.Items.AddRange(m_objCityList);
                        //foreach (CCity objCity in m_objCityList)
                        //{
                        //    cboxCity.Properties.Items.Add(objCity);
                        //}
                    }
                    else
                    {
                        List<CCity> objCityList = new List<CCity>();
                        foreach (CCity objCity in m_objCityList)
                        {
                            if (objCity.Region.ID.CompareTo(objRegion.ID) == 0)
                            {
                                objCityList.Add(objCity);
                                //cboxCity.Properties.Items.Add(objCity);
                            }
                        }
                        cboxCity.Properties.Items.AddRange(objCityList);
                    }                    
                }
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка построения списка городов.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                if (bSuspend == true)
                {
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    ((System.ComponentModel.ISupportInitialize)(this.cboxStreet.Properties)).EndInit();
                    ((System.ComponentModel.ISupportInitialize)(this.cboxCity.Properties)).EndInit();
                    this.ResumeLayout(false);
                }
            }

            return;
        }
        /// <summary>
        /// Фильтрация списка улиц для заданного города
        /// </summary>
        /// <param name="objCity">город</param>
        /// <param name="bSuspend">признак "отключить элемент управления"</param>
        private void LoadStreetListForCity(CCity objCity, System.Boolean bSuspend)
        {
            if (objCity == null) { return; }
            try
            {
                if (bSuspend == true)
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    ((System.ComponentModel.ISupportInitialize)(this.cboxStreet.Properties)).BeginInit();
                    this.SuspendLayout();
                }

                cboxStreet.Properties.Items.Clear();
                cboxStreet.EditValue = null;

                if ((objCity.StreetList != null) && (objCity.StreetList.Count > 0))
                {
                    cboxStreet.Properties.Items.AddRange(objCity.StreetList);

                    //foreach (System.String strStreet in objCity.StreetList)
                    //{
                    //    cboxStreet.Properties.Items.Add(strStreet);
                    //}
                }
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка построения списка улиц.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                if (bSuspend == true)
                {
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    ((System.ComponentModel.ISupportInitialize)(this.cboxStreet.Properties)).EndInit();
                    this.ResumeLayout(false);
                }
            }

            return;
        }
        private void cboxCountry_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                if (cboxCountry.SelectedItem != null)
                {
                    LoadOblastListForCountry((CCountry)cboxCountry.SelectedItem, true);
                    SetPropertiesModified(true);
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения значения страны.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;

        }
        private void cboxOblast_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                if (cboxOblast.SelectedItem != null)
                {
                    LoadRegionListForOblast((COblast)cboxOblast.SelectedItem, true);
                    SetPropertiesModified(true);
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения значения области.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        private void cboxRegion_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                if (cboxRegion.SelectedItem != null)
                {
                    LoadCityListForRegion((CRegion)cboxRegion.SelectedItem, true);
                    SetPropertiesModified(true);
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения значения региона.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        private void cboxCity_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                if (cboxCity.SelectedItem != null)
                {
                    LoadStreetListForCity((CCity)cboxCity.SelectedItem, true);
                    SetPropertiesModified(true);
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения значения города.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        #endregion

        #region Новый адрес
        /// <summary>
        /// Открывает карточку адреса в режиме вставки
        /// </summary>
        private void NewAddress()
        {
            if (m_objCountryList == null) { return; }
            m_bDisableEvents = true;
            m_bDisableTreeListEvents = true;
            m_bNewAddress = true;

            try
            {
                ClearAllPropertiesControls();

                m_objAddressDeletedList.Clear();
                cboxOblast.Properties.Items.Clear();
                cboxOblast.EditValue = null;
                cboxRegion.Properties.Items.Clear();
                cboxRegion.EditValue = null;
                cboxStreet.Properties.Items.Clear();
                cboxStreet.EditValue = null;
                cboxCity.Properties.Items.Clear();
                cboxCity.EditValue = null;
                cboxCountry.Properties.Items.Clear();
                cboxCountry.EditValue = null;
                txtPostIndex.Text = "";
                txtBuildingCode.Text = "";
                txtSubBuildingCode.Text = "";
                txtFlatCode.Text = "";

                SetReadOnlyPropertiesControls(false);

                LoadCountryList(true);

                foreach (Object objAddressType in cboxAddressType.Properties.Items)
                {
                    if (((CAddressType)objAddressType).IsDefault == true)
                    {
                        cboxAddressType.SelectedItem = objAddressType;
                        break;
                    }
                }

                foreach (Object objAddressPrefix in cboxAddressPrefix.Properties.Items)
                {
                    if (((CAddressPrefix)objAddressPrefix).IsDefault == true)
                    {
                        cboxAddressPrefix.SelectedItem = objAddressPrefix;
                        break;
                    }
                }

                foreach (Object objBuilding in cboxBuilding.Properties.Items)
                {
                    if (((CBuilding)objBuilding).IsDefault == true)
                    {
                        cboxBuilding.SelectedItem = objBuilding;
                        break;
                    }
                }

                foreach (Object objSubBuilding in cboxSubBuilding.Properties.Items)
                {
                    if (((CSubBuilding)objSubBuilding).IsDefault == true)
                    {
                        cboxSubBuilding.SelectedItem = objSubBuilding;
                        break;
                    }
                }

                foreach (Object objFlat in cboxFlat.Properties.Items)
                {
                    if (((CFlat)objFlat).IsDefault == true)
                    {
                        cboxFlat.SelectedItem = objFlat;
                        break;
                    }
                }

                cboxOblast.Focus();
                m_objSelectedAddress = new CAddress();
                m_objSelectedAddress.IsNewObject = true;
                DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListAddress.AppendNode(new object[] { m_objSelectedAddress.VisitingCard }, null);
                objNode.Tag = m_objSelectedAddress;
                treeListAddress.FocusedNode = objNode;

                // включаем/выключаем кнопки
                btnEditAddress.Enabled = false;
                btnAddAddress.Enabled = false;
                btnCancel.Enabled = true;
                btnSave.Enabled = true;
                btnDeleteAddress.Enabled = false;


            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка добавления нового адреса.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                SetPropertiesModified(true);

                m_bDisableEvents = false;
                m_bDisableTreeListEvents = false;
            }

            return;
        }
        private void menuAdd_Click(object sender, EventArgs e)
        {
            try
            {
                NewAddress();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка добавления нового адреса.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        private void btnAddAddress_Click(object sender, EventArgs e)
        {
            try
            {
                NewAddress();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка добавления нового адреса.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        #endregion

        #region Редактировать адрес
        /// <summary>
        /// Открывает карточку адреса в режиме редактирования
        /// </summary>
        /// <param name="objAddress">адрес</param>
        /// 
        private void EditAddress(CAddress objAddress)
        {
            if (objAddress == null) { return; }
            try
            {
                m_bDisableEvents = true;
                m_bDisableTreeListEvents = true;

                m_objSelectedAddress = objAddress;
                m_objAddressDeletedList.Clear();

                CCountry objSelectedCountry = m_objSelectedAddress.City.Region.Oblast.Country;
                COblast objSelectedOblast = m_objSelectedAddress.City.Region.Oblast;
                CRegion objSelectedRegion = m_objSelectedAddress.City.Region;
                CCity objSelectedCity = m_objSelectedAddress.City;

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

                ClearAllPropertiesControls();

                if (m_objCountryList.Count > 0)
                {
                    // устанавливаем значение страны
                    foreach (CCountry objCountry in m_objCountryList)
                    {
                        cboxCountry.Properties.Items.Add(objCountry);
                        if (objCountry.ID.CompareTo(objSelectedCountry.ID) == 0)
                        {
                            cboxCountry.SelectedItem = objCountry;
                        }
                    }
                    // загружаем список областей
                    LoadOblastListForCountry((CCountry)cboxCountry.SelectedItem, false);
                    if (cboxOblast.Properties.Items.Count > 0)
                    {
                        foreach (Object objOblast in cboxOblast.Properties.Items)
                        {
                            if (((COblast)objOblast).ID.CompareTo(objSelectedOblast.ID) == 0)
                            {
                                cboxOblast.SelectedItem = objOblast;
                            }
                        }
                    }
                    if (cboxOblast.SelectedItem != null)
                    {
                        // загружаем список регионов
                        LoadRegionListForOblast((COblast)cboxOblast.SelectedItem, false);
                        if (cboxRegion.Properties.Items.Count > 0)
                        {
                            foreach (Object objRegion in cboxRegion.Properties.Items)
                            {
                                if (((CRegion)objRegion).ID.CompareTo(objSelectedRegion.ID) == 0)
                                {
                                    cboxRegion.SelectedItem = objRegion;
                                    break;
                                }
                            }
                            // список городов
                            if (cboxRegion.SelectedItem != null)
                            {
                                LoadCityListForRegion((CRegion)cboxRegion.SelectedItem, false);
                                if (cboxCity.Properties.Items.Count > 0)
                                {
                                    foreach (Object objCity in cboxCity.Properties.Items)
                                    {
                                        if (((CCity)objCity).ID.CompareTo(objSelectedCity.ID) == 0)
                                        {
                                            cboxCity.SelectedItem = objCity;
                                            break;
                                        }
                                    }
                                }
                                // список улиц
                                if (cboxCity.SelectedItem != null)
                                {
                                    LoadStreetListForCity((CCity)cboxCity.SelectedItem, false);
                                    if (cboxStreet.Properties.Items.Count > 0)
                                    {
                                        foreach (Object objStreet in cboxStreet.Properties.Items)
                                        {
                                            if (((System.String)objStreet) == m_objSelectedAddress.Name)
                                            {
                                                cboxStreet.SelectedItem = objStreet;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    txtPostIndex.Text = m_objSelectedAddress.PostIndex;
                    txtBuildingCode.Text = m_objSelectedAddress.BuildCode;
                    txtSubBuildingCode.Text = m_objSelectedAddress.SubBuildCode;
                    txtFlatCode.Text = m_objSelectedAddress.FlatCode;
                    txtDescription.Text = m_objSelectedAddress.Description;

                    // тип адреса
                    foreach (Object objAddressType in cboxAddressType.Properties.Items)
                    {
                        if (((CAddressType)objAddressType).ID.CompareTo(m_objSelectedAddress.AddressType.ID) == 0)
                        {
                            cboxAddressType.SelectedItem = objAddressType;
                            break;
                        }
                    }
                    // тип улицы
                    if (m_objSelectedAddress.AddressPrefix != null)
                    {
                        foreach (Object objAddressPrefix in cboxAddressPrefix.Properties.Items)
                        {
                            if (((CAddressPrefix)objAddressPrefix).ID.CompareTo(m_objSelectedAddress.AddressPrefix.ID) == 0)
                            {
                                cboxAddressPrefix.SelectedItem = objAddressPrefix;
                                break;
                            }
                        }
                    }
                    // тип здания
                    if (m_objSelectedAddress.Building != null)
                    {
                        foreach (Object objBuilding in cboxBuilding.Properties.Items)
                        {
                            if (((CBuilding)objBuilding).ID.CompareTo(m_objSelectedAddress.Building.ID) == 0)
                            {
                                cboxBuilding.SelectedItem = objBuilding;
                                break;
                            }
                        }
                    }

                    // тип корпуса
                    if (m_objSelectedAddress.SubBuilding != null)
                    {
                        foreach (Object objSubBuilding in cboxSubBuilding.Properties.Items)
                        {
                            if (((CSubBuilding)objSubBuilding).ID.CompareTo(m_objSelectedAddress.SubBuilding.ID) == 0)
                            {
                                cboxSubBuilding.SelectedItem = objSubBuilding;
                                break;
                            }
                        }
                    }

                    // тип помещения
                    if (m_objSelectedAddress.Flat != null)
                    {
                        foreach (Object objFlat in cboxFlat.Properties.Items)
                        {
                            if (((CFlat)objFlat).ID.CompareTo( m_objSelectedAddress.Flat.ID ) == 0)
                            {
                                cboxFlat.SelectedItem = objFlat;
                                break;
                            }
                        }
                    }
                }
                SetPropertiesModified(false);
                SetReadOnlyPropertiesControls(false);

                cboxOblast.Focus();

            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка редактирования адреса.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                m_bDisableEvents = false;
                m_bDisableTreeListEvents = false;
            }

            return;
        }
        /// <summary>
        /// Возвращает ссылку на выбранный адрес
        /// </summary>
        /// <returns>адрес</returns>
        private CAddress GetSelectedAddress()
        {
            CAddress objRet = null;
            try
            {
                if ((treeListAddress.Nodes.Count > 0) && (treeListAddress.FocusedNode != null) && (treeListAddress.FocusedNode.Tag != null))
                {
                    objRet = (CAddress)treeListAddress.FocusedNode.Tag;
                }
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка поиска выбранного адреса.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return objRet;
        }

        private void menuEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if ((treeListAddress.FocusedNode == null) || (treeListAddress.FocusedNode.Tag == null)) { return; }
                EditAddress((CAddress)treeListAddress.FocusedNode.Tag);

                btnEditAddress.Enabled = false;
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения свойств адреса.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        #endregion

        #region Удалить адрес 
        /// <summary>
        /// Очищает содержимое элементов управления для отображения свойств адреса
        /// </summary>
        public void ClearAllPropertiesControls()
        {
            m_bDisableEvents = true;
            try
            {
                // страна
                cboxCountry.SelectedItem = null;
                // область
                cboxOblast.SelectedItem = null;
                // район
                cboxRegion.SelectedItem = null;
                // населенный пункт
                cboxCity.SelectedItem = null;
                // улица
                cboxStreet.SelectedItem = null;
                // тип адреса
                cboxAddressType.SelectedItem = null;
                // тип улицы
                cboxAddressPrefix.SelectedItem = null;
                // тип строения
                cboxBuilding.SelectedItem = null;
                txtBuildingCode.Text = "";
                // тип подстроения
                cboxSubBuilding.SelectedItem = null;
                txtSubBuildingCode.Text = "";
                // помещение
                cboxFlat.SelectedItem = null;
                txtFlatCode.Text = "";
                // почтовый индекс
                txtPostIndex.Text = "";
                txtBuildingCode.Text = "";
                txtSubBuildingCode.Text = "";
                txtFlatCode.Text = "";
                txtDescription.Text = "";

                SetReadOnlyPropertiesControls(true);
                SimulateChangeAddressPropertie(m_objSelectedAddress, enumActionSaveCancel.Unkown, m_bNewAddress);
                
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog( "ClearAllPropertiesControls. Текст ошибки: " + f.Message );
            }
            finally
            {
                m_bDisableEvents = false;
            }

            return;
        }
        public void ClearAddressList()
        {
            m_bDisableEvents = true;
            try
            {
                treeListAddress.Nodes.Clear();
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("ClearAddressList. Текст ошибки: " + f.Message);
            }
            finally
            {
                m_bDisableEvents = false;
            }

            return;
        }
        /// <summary>
        /// Удаляет адрес из списка
        /// </summary>
        /// <param name="objNode">адрес</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        private System.Boolean DeleteAddress(DevExpress.XtraTreeList.Nodes.TreeListNode objNode)
        {
            System.Boolean bRet = false;
            if ((objNode == null) || (objNode.Tag == null)) { return bRet; }
            try
            {
                if (m_objAddressDeletedList == null) { m_objAddressDeletedList = new List<CAddress>(); }
                m_objAddressDeletedList.Add((CAddress)objNode.Tag);

                System.Int32 iPosNode = treeListAddress.GetNodeIndex(objNode);
                DevExpress.XtraTreeList.Nodes.TreeListNode objPrevNode = objNode.PrevNode;

                treeListAddress.Nodes.RemoveAt(iPosNode); //
                if (objPrevNode == null)
                {
                    if (treeListAddress.Nodes.Count > 0)
                    {
                        treeListAddress.FocusedNode = treeListAddress.Nodes[ 0 ];
                    }
                }
                else
                {
                    treeListAddress.FocusedNode = objPrevNode; // 
                }

                if (treeListAddress.FocusedNode == null) 
                { 
                    ClearAllPropertiesControls();
                    btnAddAddress.Enabled = true;
                    btnEditAddress.Enabled = false;
                    btnDeleteAddress.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                }
                else
                {
                    btnAddAddress.Enabled = true;
                    btnEditAddress.Enabled = true;
                    btnDeleteAddress.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                }

                SimulateChangeAddressPropertie(m_objSelectedAddress, enumActionSaveCancel.Unkown, m_bNewAddress);
                bRet = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления адреса.\n\nТекст ошибки:\n" + f.Message, "Ошибка",
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
                DeleteAddress(treeListAddress.FocusedNode);
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка удаления адреса.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        private void btnDeleteAddress_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteAddress(treeListAddress.FocusedNode); // удаление только из дерева, но не из БД
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка удаления адреса.\nТекст ошибки: " + f.Message, "Ошибка",
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
            ValidateProperties();
            m_bIsChanged = bModified;
            btnSave.Enabled = m_bIsChanged;
            btnCancel.Enabled = btnSave.Enabled;
            if (m_bIsChanged == true)
            {
                SimulateChangeAddressPropertie(m_objSelectedAddress, enumActionSaveCancel.Unkown, m_bNewAddress);
            }
        }

        private void txtPostIndex_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (m_bDisableEvents == true) { return; }
            try
            {
                if (e.NewValue != null)
                {
                    System.String strNewValue = e.NewValue.ToString();
                    if (strNewValue.Length > 0)
                    {
                        if (strNewValue.Length > 8)
                        {
                            e.Cancel = true;
                        }
                    }
                }
                SetPropertiesModified(true);
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения почтового индекса.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        private void ValidateProperties()
        {
            try
            {
                if ((treeListAddress.Nodes.Count == 0) || (treeListAddress.FocusedNode == null))
                {
                    txtBuildingCode.Properties.Appearance.BackColor = System.Drawing.Color.White;
                    cboxCity.Properties.Appearance.BackColor = System.Drawing.Color.White;
                    txtPostIndex.Properties.Appearance.BackColor = System.Drawing.Color.White;
                    cboxStreet.Properties.Appearance.BackColor = System.Drawing.Color.White;
                }
                else
                {
                    txtSubBuildingCode.Properties.Appearance.BackColor = ((txtSubBuildingCode.Text == "") && (cboxSubBuilding.SelectedItem != null)) ? System.Drawing.Color.Tomato : System.Drawing.Color.White;
                    txtBuildingCode.Properties.Appearance.BackColor = ((txtBuildingCode.Text == "") && (cboxBuilding.SelectedItem != null)) ? System.Drawing.Color.Tomato : System.Drawing.Color.White;
                    cboxCity.Properties.Appearance.BackColor = (cboxCity.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White;
                    txtPostIndex.Properties.Appearance.BackColor = ((txtPostIndex.Text == "") || (txtPostIndex.Text.Length < 6)) ? System.Drawing.Color.Tomato : System.Drawing.Color.White;
                    cboxStreet.Properties.Appearance.BackColor = ( ( cboxStreet.Text == "" ) && ( cboxAddressPrefix.SelectedItem != null )) ? System.Drawing.Color.Tomato : System.Drawing.Color.White;
                }

            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения свойств адреса.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        private void txtAddressProperties_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (m_bDisableEvents == true) { return; }
            try
            {
                if (e.NewValue != null)
                {
                    SetPropertiesModified(true);
                }
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения свойств адреса.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        private void AddressProperties_SelectedValueChanged(object sender, EventArgs e)
        {
            if (m_bDisableEvents == true) { return; }
            try
            {
                SetPropertiesModified(true);
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения свойств адреса.\nТекст ошибки: " + f.Message, "Ошибка",
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
                if (m_objSelectedAddress == null) { return; }
                if (m_bNewAddress == true)
                {
                    ClearAllPropertiesControls();
                    SetReadOnlyPropertiesControls(true);

                    if (treeListAddress.FocusedNode != null)
                    {
                        System.Int32 iNodeIndx = treeListAddress.GetNodeIndex(treeListAddress.FocusedNode);
                        treeListAddress.Nodes.Remove(treeListAddress.FocusedNode);
                        if (treeListAddress.Nodes.Count > 0)
                        {
                            treeListAddress.FocusedNode = treeListAddress.Nodes[treeListAddress.Nodes.Count - 1];
                        }
                    }
                }
                else
                {
                    if ((m_objSelectedAddress != null) && (m_bNewAddress == false))
                    {
                        ShowAddressDetail(m_objSelectedAddress);
                    }
                }

                btnAddAddress.Enabled = true;
                btnEditAddress.Enabled = (treeListAddress.FocusedNode != null);
                btnDeleteAddress.Enabled = (treeListAddress.FocusedNode != null);
                btnCancel.Enabled = false;
                btnSave.Enabled = false;

                SimulateChangeAddressPropertie(m_objSelectedAddress, enumActionSaveCancel.Cancel, m_bNewAddress);

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

        #region Сохранить изменения в адресе
        /// <summary>
        /// Сохраняет изменения в адресе
        /// </summary>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        private System.Boolean SaveChanges()
        {
            System.Boolean bRet = false;
            try
            {
                // сперва нужно проверить, все ли нужные свойства указаны
                if (cboxCity.SelectedItem == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Укажите, пожалуйста, населенный пункт!", "Внимание",
                       System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                //if (cboxStreet.SelectedItem == null)
                //{
                //    DevExpress.XtraEditors.XtraMessageBox.Show("Укажите, пожалуйста, улицу!", "Внимание",
                //       System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                //    return bRet;
                //}
                //if ( ( cboxBuilding.SelectedItem == null ) || ( txtBuildingCode.Text == "" ) )
                //{
                //    DevExpress.XtraEditors.XtraMessageBox.Show("Укажите, пожалуйста, номер здания!", "Внимание",
                //       System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                //    return bRet;
                //}
                if ( txtPostIndex.Text == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Укажите, пожалуйста, почтовый индекс!", "Внимание",
                       System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }

                System.Boolean bNewAddress = ((m_objSelectedAddress == null) || (m_objSelectedAddress.ID.CompareTo(System.Guid.Empty) == 0));
                m_objSelectedAddress.PostIndex = txtPostIndex.Text;
                m_objSelectedAddress.AddressType = (CAddressType)cboxAddressType.SelectedItem;
                m_objSelectedAddress.City = (CCity)cboxCity.SelectedItem;
                m_objSelectedAddress.AddressPrefix = (cboxAddressPrefix.SelectedItem == null) ? null : (CAddressPrefix)cboxAddressPrefix.SelectedItem;
                m_objSelectedAddress.Name = (m_objSelectedAddress.AddressPrefix == null) ? "" : (System.String)cboxStreet.Text;
                m_objSelectedAddress.Building = (cboxBuilding.SelectedItem == null) ? null : (CBuilding)cboxBuilding.SelectedItem;
                m_objSelectedAddress.BuildCode = (m_objSelectedAddress.Building == null) ? "" : txtBuildingCode.Text;
                m_objSelectedAddress.Description = txtDescription.Text;
                if ((cboxSubBuilding.SelectedItem != null) && (txtSubBuildingCode.Text != ""))
                {
                    m_objSelectedAddress.SubBuilding = (CSubBuilding)cboxSubBuilding.SelectedItem;
                    m_objSelectedAddress.SubBuildCode = txtSubBuildingCode.Text;
                }
                else
                {
                    m_objSelectedAddress.SubBuilding = null;
                    m_objSelectedAddress.SubBuildCode = "";
                }
                if ((cboxFlat.SelectedItem != null) && (txtFlatCode.Text != ""))
                {
                    m_objSelectedAddress.Flat = (CFlat)cboxFlat.SelectedItem;
                    m_objSelectedAddress.FlatCode = txtFlatCode.Text;
                }
                else
                {
                    m_objSelectedAddress.Flat = null;
                    m_objSelectedAddress.FlatCode = "";
                }

                System.String strErr = "";
                if (m_objSelectedAddress.IsAllParametersValid(ref strErr) == true)
                {
                    treeListAddress.FocusedNode.SetValue(colFullAddress, m_objSelectedAddress.VisitingCard2);

                    btnAddAddress.Enabled = true;
                    btnEditAddress.Enabled = true;
                    btnDeleteAddress.Enabled = true;
                    btnCancel.Enabled = false;
                    btnSave.Enabled = false;

                    SetReadOnlyPropertiesControls(true);
                    bRet = true;
                }
                else
                {
                    SendMessageToLog("Ошибка сохранения изменений в описании адреса. Текст ошибки: " + strErr);
                }
                
                bRet = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка сохранения изменений в адресе.\n\nТекст ошибки:\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                //if (m_objSelectedAddress == null) { return; }
                //if (SaveChanges() == true)
                //{
                //    SimulateChangeAddressPropertie(m_objSelectedAddress, enumActionSaveCancel.Save, m_bNewAddress);
                //    m_bNewAddress = false;
                //}
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка сохранения изменений в адресе.\n\nТекст ошибки:\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }
        public void ConfirmChanges()
        {
            try
            {
                if (m_objSelectedAddress == null) { return; }
                if (m_bIsChanged == false) { return; }
                if (SaveChanges() == true)
                {
                    SimulateChangeAddressPropertie(m_objSelectedAddress, enumActionSaveCancel.Save, m_bNewAddress);
                    m_bNewAddress = false;
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в адресе. Текст ошибки: " + f.Message);
            }
            return;
        }

        #endregion

        private void ctrlAddress_Load(object sender, EventArgs e)
        {
            splitContainerControl.SplitterPosition = iPanel1WidthDef;
        }

    }


    /// <summary>
    /// Тип, хранящий информацию, которая передается получателям уведомления о событии
    /// </summary>
    public partial class ChangeAddressPropertieEventArgs : EventArgs
    {
        private readonly CAddress m_objAddress;
        public CAddress Address
        { get { return m_objAddress; } }

        private readonly enumActionSaveCancel m_enActionType;
        public enumActionSaveCancel ActionType
        { get { return m_enActionType; } }

        private readonly System.Boolean m_bIsNewAddress;
        public System.Boolean IsNewAddress
        { get { return m_bIsNewAddress; } }

        public ChangeAddressPropertieEventArgs(CAddress objAddress, enumActionSaveCancel enActionType, System.Boolean bIsNewAddress)
        {
            m_objAddress = objAddress;
            m_enActionType = enActionType;
            m_bIsNewAddress = bIsNewAddress;
        }
    }

}
