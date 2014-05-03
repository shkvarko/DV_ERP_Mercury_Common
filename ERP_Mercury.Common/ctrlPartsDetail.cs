using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using DevExpress.XtraExport;
using DevExpress.XtraGrid.Export;
using DevExpress.XtraEditors;
using System.Diagnostics;

namespace ERP_Mercury.Common
{
    enum enTabProperties
    {
        All = 0,
        Properties = 1,
        LinkedToStock = 2,
        Kit = 3,
        Certificate = 4,
        Image = 5
    }

    public partial class ctrlPartsDetail : UserControl
    {
        #region Свойства
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;
        private List<CProduct> m_objProductList;
        private DevExpress.XtraGrid.Views.Base.ColumnView ColumnView
        {
            get { return gridControlProductList.MainView as DevExpress.XtraGrid.Views.Base.ColumnView; }
        }
        private System.Boolean m_bIsComboBoxFill;
        private System.Boolean m_bDisableEvents;
        private System.Boolean m_bIsChanged;
        public System.Boolean IsChangedCertificate {get; set;}
        public System.Boolean IsChangedKit { get; set; }
        public System.Boolean IsChangedImage { get; set; }
        public System.Boolean IsChangedLinkedToStock { get; set; }
        public System.Boolean IsChanged
        {
            get { return m_bIsChanged; }
        }
        private const System.Int32 icolIsProductCompositeWidth = 60;
        private CProduct m_objSelectedProduct;
        private List<CProduct> m_objSelectedProductList;
        private List<CProductSubType> m_objProductSubTypeList;
        private List<CProductTradeMark> m_objProductTradeMarkList;
        private List<CProductType> m_objProductTypeList;
        private frmPartsList m_objFrmPartsList;
 
        frmFilterForPartsList m_objFrmFilterForPartsList;
        /// <summary>
        /// выбранный размер скидки
        /// </summary>
        public System.Double SelectedDiscountPercent
        {
            get { return ( ( checkEditDiscount.Checked == true ) ? System.Convert.ToDouble(calcEditDiscountPercent.Value) : 0); }
        }
        private const System.String strWarningAboutFilter = "Внимание! Установлен фильтр.";

        // потоки
        private System.Threading.Thread thrLoadPartsBarcodes;
        public System.Threading.Thread ThreadrLoadPartsBarcodes
        {
            get { return thrLoadPartsBarcodes; }
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
        public delegate void LoadPartsBarcodesListDelegate(List<CProduct> objProductList);
        public LoadPartsBarcodesListDelegate m_LoadPartsBarcodesListDelegate;

        private const System.Int32 iThreadSleepTime = 1000;
        private const System.String strWaitCustomer = "ждите... идет заполнение списка";
        private System.Boolean m_bThreadFinishJob;

        #endregion

        #region Конструктор
        public ctrlPartsDetail(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem)
        {
            InitializeComponent();
            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_objProductList = null;
            m_objSelectedProduct = null;
            m_bIsComboBoxFill = false;
            m_objSelectedProductList = null;
            m_objFrmPartsList = null;

            AddGridColumns();
            InitializeTreeList();

            RestoreLayoutFromRegistry();

            LoadProductList();
            m_bDisableEvents = false;
            m_bIsChanged = false;
            IsChangedCertificate = false;
            IsChangedKit = false;
            IsChangedImage = false;
            IsChangedLinkedToStock = false;
            pnlDiscount.Visible = false;
            checkEditDiscount.Checked = false;
            calcEditDiscountPercent.Value = 0;
            m_objProductSubTypeList = CProductSubType.GetProductSubTypeLightList(m_objProfile, null);
            m_objProductTradeMarkList = CProductTradeMark.GetProductTradeMarkList(m_objProfile, null);
            m_objProductTypeList = CProductType.GetProductTypeList(m_objProfile, null);

            m_objFrmFilterForPartsList = new frmFilterForPartsList(m_objProfile, m_objMenuItem, m_objProductSubTypeList,
                m_objProductTradeMarkList, m_objProductTypeList);

            lblWarningAboutFilter.Text = "";

            CheckRights();

            tabControl.SelectedTabPage = tabView;
            tabControlPartProperties.SelectedTabPage = tabPagePartProperties;
        }
        public ctrlPartsDetail(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem, List<CProduct> objProductList)
        {
            InitializeComponent();
            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_objProductList = objProductList;
            m_objSelectedProduct = null;
            m_bIsComboBoxFill = false;
            m_objSelectedProductList = null;

            AddGridColumns();
            InitializeTreeList();

            RestoreLayoutFromRegistry();

            m_bDisableEvents = true;
            gridControlProductList.DataSource = m_objProductList; 
            m_bDisableEvents = false;
            
            m_bIsChanged = false;
            IsChangedLinkedToStock = false;
            pnlDiscount.Visible = false;
            checkEditDiscount.Checked = false;
            calcEditDiscountPercent.Value = 0;
            m_objProductSubTypeList = CProductSubType.GetProductSubTypeLightList(m_objProfile, null);
            m_objProductTradeMarkList = CProductTradeMark.GetProductTradeMarkList(m_objProfile, null);
            m_objProductTypeList = CProductType.GetProductTypeList(m_objProfile, null);
            m_objFrmFilterForPartsList = new frmFilterForPartsList(m_objProfile, m_objMenuItem, m_objProductSubTypeList,
                m_objProductTradeMarkList, m_objProductTypeList);
            lblWarningAboutFilter.Text = "";

            CheckRights();

            tabControl.SelectedTabPage = tabView;
            tabControlPartProperties.SelectedTabPage = tabPagePartProperties;

        }
        private void gridControlProductList_Load(object sender, EventArgs e)
        {
            try
            {
                StartThreadWithLoadData();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("gridControlProductList_Load. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        /// <summary>
        /// Построение столбцов и строк в дереве
        /// </summary>
        private void InitializeTreeList()
        {
            try
            {
                List<COrderType> objOrderTypeList = COrderType.GetOrderTypeList(m_objProfile);
                if (objOrderTypeList != null)
                {
                    DevExpress.XtraTreeList.Columns.TreeListColumn colOrderType = null;
                    foreach (COrderType objOrderType in objOrderTypeList)
                    {
                        colOrderType = this.treeList.Columns.Add();
                        colOrderType.ColumnEdit = repItemCheckEdit;
                        colOrderType.Caption = objOrderType.Name;
                        colOrderType.FieldName = objOrderType.Name;
                        colOrderType.MinWidth = 50;
                        colOrderType.Name = objOrderType.Id.ToString();
                        colOrderType.OptionsColumn.AllowSort = false;
                        colOrderType.Visible = true;
                        colOrderType.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        colOrderType.Tag = objOrderType;
                    }
                }
                objOrderTypeList = null;

            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка установки начальных настроек дерева. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

        public void EnablDiscountTools()
        {
            try
            {
                checkEditDiscount.Checked = false;
                pnlDiscount.Visible = true;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("EnablDiscountTools. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        /// <summary>
        /// Проверка на наличие динамических прав на редактирование карточки товара
        /// </summary>
        private void CheckRights()
        {
            try
            {
                System.Boolean bDREditProduct = m_objProfile.GetClientsRight().GetState(ERP_Mercury.Global.Consts.strDR_EditProduct);
                System.Boolean bDREditCertificate = m_objProfile.GetClientsRight().GetState(ERP_Mercury.Global.Consts.strDR_EditCertificate);
                btnEdit.Visible = (bDREditProduct == true);
                txtCertificate.Properties.ReadOnly = (bDREditCertificate == false);
                tabPageCertificate.PageVisible = (bDREditCertificate == true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("EnablDiscountTools. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

        #endregion

        #region Потоки
        public void StartThreadWithLoadData()
        {
            try
            {
                // инициализируем события
                this.m_EventStopThread = new System.Threading.ManualResetEvent(false);
                this.m_EventThreadStopped = new System.Threading.ManualResetEvent(false);

                // инициализируем делегаты
                m_LoadPartsBarcodesListDelegate = new LoadPartsBarcodesListDelegate(LoadPartsBarcodesList);

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

                this.thrLoadPartsBarcodes = new System.Threading.Thread(WorkerThreadFunction);
                this.thrLoadPartsBarcodes.Start();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("StartThread.\n\nТекст ошибки: " + f.Message, "Ошибка",
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
                LoadPartsBarcodesListInThread();

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

                //cboxCustomer.SelectedText = "";
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

        public void LoadPartsBarcodesListInThread()
        {
            try
            {
                System.String strErr = "";
                List<CProduct> objPartsList = CProduct.GetPartsWithsBarcodeList(m_objProfile, null, ref strErr);
                List<CProduct> objAddPartsList = new List<CProduct>();

                if ((objPartsList != null) && (objPartsList.Count > 0))
                {

                    System.Int32 iRecCount = 0;
                    
                    foreach (CProduct objProduct in objPartsList)
                    {
                        objAddPartsList.Add(objProduct);
                        iRecCount++;

                        if (iRecCount == 2000)
                        {
                            iRecCount = 0;
                            Thread.Sleep(1000);
                            this.Invoke(m_LoadPartsBarcodesListDelegate, new Object[] { objAddPartsList });
                            objAddPartsList.Clear();
                        }

                    }
                    if (iRecCount != 1000)
                    {
                        iRecCount = 0;
                        this.Invoke(m_LoadPartsBarcodesListDelegate, new Object[] { objAddPartsList });
                    }

                }

                //this.Invoke(m_LoadCustomerListDelegate, new Object[] { objCustomerList });
                //return;

                objPartsList = null;
                objAddPartsList = null;

                this.Invoke(m_LoadPartsBarcodesListDelegate, new Object[] { null });
                this.m_bThreadFinishJob = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadPartsBarcodesListInThread.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        private void LoadPartsBarcodesList(List<CProduct> objProductList)
        {
            try
            {
                if ((objProductList != null) && (objProductList.Count > 0))
                {
                    CProduct objProduct = null;
                    foreach (CProduct objItem in objProductList)
                    {
                        objProduct = null;
                        objProduct = m_objProductList.FirstOrDefault<CProduct>(x => x.ID.CompareTo(objItem.ID) == 0);
                        if( (objProduct != null) && ( ( objProduct.BarcodeList == null ) || ( objProduct.BarcodeList.Count == 0 ) ) )
                        {
                            objProduct.BarcodeList = objItem.BarcodeList;
                        }
                    }
                    gridControlProductList.RefreshDataSource();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadPartsBarcodesList.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        #endregion
        
        #region Список товаров
        private void AddGridColumns()
        {
            ColumnView.Columns.Clear();
            AddGridColumn(ColumnView, "ID", "Код товара");
            AddGridColumn(ColumnView, "ID_Ib", "Код");
            AddGridColumn(ColumnView, "IsNotValid", "Неактуален");
            AddGridColumn(ColumnView, "IsActualNotValid", "Подтв-е неакт-ти");
            AddGridColumn(ColumnView, "ProductTradeMarkName", "Товарная марка");
            AddGridColumn(ColumnView, "ProductTypeName", "Товарная группа");
            AddGridColumn(ColumnView, "ProductSubTypeName", "Товарная подгруппа");
            AddGridColumn(ColumnView, "ProductLineName", "Товарная линия");
            AddGridColumn(ColumnView, "ProductFullName", "Товар");
            AddGridColumn(ColumnView, "ProductCategoryName", "Категория товара");

            AddGridColumn(ColumnView, "ProductTradeMarkIbID", "Товарная марка (код)");
            AddGridColumn(ColumnView, "ProductTypeIbID", "Товарная группа (код)");
            AddGridColumn(ColumnView, "ProductSubTypeIbID", "Товарная подгруппа (код)");
            AddGridColumn(ColumnView, "ProductLineIbID", "Товарная линия (код)");
            AddGridColumn(ColumnView, "Article", "Артикул товара");
            AddGridColumn(ColumnView, "Name", "Наименование товара");

            AddGridColumn(ColumnView, "PriceImporter", "Цена импортера, руб.");
            AddGridColumn(ColumnView, "Certificate", "Информация о качестве");
            AddGridColumn(ColumnView, "ActualStateName", "Состояние");
            AddGridColumn(ColumnView, "IsCheck", "Вкл.");
            AddGridColumn(ColumnView, "IsIncludeInStockShip", "Назначен складу");
            AddGridColumn(ColumnView, "Reference", "Референс");
            AddGridColumn(ColumnView, "BarcodeListString2", "Штрих-код");
            
            
            foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in ColumnView.Columns)
            {
                objColumn.OptionsColumn.AllowEdit = (objColumn.FieldName == "IsCheck");
                objColumn.OptionsColumn.AllowFocus = (objColumn.FieldName == "IsCheck");
                objColumn.OptionsColumn.ReadOnly = (objColumn.FieldName != "IsCheck");
                objColumn.Visible = (objColumn.FieldName != "ID");
                objColumn.Width = objColumn.GetBestWidth();
                if ((objColumn.FieldName == "IsProductComposite") || (objColumn.FieldName == "IsNotValid") || 
                    (objColumn.FieldName == "IsProductComposite") || (objColumn.FieldName == "IsCheck") ||
                    (objColumn.FieldName == "IsIncludeInStockShip") || (objColumn.FieldName == "BarcodeListString2"))
                {
                    objColumn.Width = icolIsProductCompositeWidth;
                    objColumn.OptionsColumn.FixedWidth = true;
                }
            }
            gridViewProductList.OptionsBehavior.Editable = true;
        }
        private void AddGridColumn(DevExpress.XtraGrid.Views.Base.ColumnView view, string fieldName, string caption) { AddGridColumn(view, fieldName, caption, null); }
        private void AddGridColumn(DevExpress.XtraGrid.Views.Base.ColumnView view, string fieldName, string caption, DevExpress.XtraEditors.Repository.RepositoryItem item) { AddGridColumn(view, fieldName, caption, item, "", DevExpress.Utils.FormatType.None); }
        private void AddGridColumn(DevExpress.XtraGrid.Views.Base.ColumnView view, string fieldName, string caption, DevExpress.XtraEditors.Repository.RepositoryItem item, string format, DevExpress.Utils.FormatType type)
        {
            DevExpress.XtraGrid.Columns.GridColumn column = view.Columns.AddField(fieldName);
            column.Caption = caption;
            column.ColumnEdit = item;
            column.DisplayFormat.FormatType = type;
            column.DisplayFormat.FormatString = format;
            column.VisibleIndex = view.VisibleColumns.Count;
        }
        public void SetAccessToMultiSelect()
        {
            try
            {
                gridViewProductList.OptionsSelection.MultiSelect = true;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка SetAccessToMultiSelect. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return ;
        }
        public List<CProduct> GetSelectedProductList()
        {
            try
            {
                if (m_objSelectedProductList == null)
                { m_objSelectedProductList = new List<CProduct>(); }
                else
                { m_objSelectedProductList.Clear(); }

                int[] arr = gridViewProductList.GetSelectedRows();

                if (arr.Length > 0)
                {
                    for (System.Int32 i = 0; i < arr.Length; i++)
                    {
                        m_objSelectedProductList.Add(m_objProductList[gridViewProductList.GetDataSourceRowIndex(arr[i])]);
                    }
                }
                else
                {
                    for (System.Int32 i = 0; i < gridViewProductList.RowCount; i++)
                    {
                        m_objSelectedProductList.Add(m_objProductList[gridViewProductList.GetDataSourceRowIndex(i)]);
                    }
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка GetSelectedProductList. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return m_objSelectedProductList;
        }

        public List<CProduct> GetSelectedProductListByCheck()
        {
            try
            {
                if (m_objSelectedProductList == null) { m_objSelectedProductList = new List<CProduct>(); }
                else { m_objSelectedProductList.Clear(); }

                var Product =
                from num in m_objProductList
                where num.IsCheck == true
                select num;

                if ((Product != null) && (Product.Count<CProduct>() > 0))
                { m_objSelectedProductList =  Product.ToList<CProduct>(); }

            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка GetSelectedProductListByCheck. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return m_objSelectedProductList;
        }
        /// <summary>
        /// Загружает список товаров
        /// </summary>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public System.Boolean LoadProductList()
        {
            System.Boolean bRet = false;
            m_bDisableEvents = true; 
            this.Cursor = Cursors.WaitCursor;
            try
            {
                this.splitContainerControl.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.gridControlProductList)).BeginInit();

                gridControlProductList.DataSource = null;

                m_objProductList = CProduct.GetProductList( m_objProfile, null, false );

                if (m_objProductList != null)
                {
                    gridControlProductList.DataSource = m_objProductList;

                }

                this.splitContainerControl.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.gridControlProductList)).EndInit();
                splitContainerControl.SplitterPosition = 700;
                splitContainerControl.Refresh();
                bRet = true;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка обновления списка. Текст ошибки: " + f.Message);
            }
            finally
            {
                m_bDisableEvents = false;
                tabControl.SelectedTabPage = tabView;
                this.Cursor = Cursors.Default;
            }

            return bRet;
        }

        /// <summary>
        /// Загружает список товаров для выбранных подгрупп
        /// </summary>
        /// <param name="objProductSubTypeList">список подгрупп</param>
        /// <param name="objProductTypeList">список групп</param>
        /// <param name="objProductTradeMarkList">список товарных марок</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public System.Boolean LoadProductList(List<CProductSubType> objProductSubTypeList, List<CProductType> objProductTypeList, List<CProductTradeMark> objProductTradeMarkList)
        {
            System.Boolean bRet = false;
            m_bDisableEvents = true;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                this.splitContainerControl.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.gridControlProductList)).BeginInit();

                gridControlProductList.DataSource = null;

                m_objProductList.Clear();

                List<CProduct> objAllProductList = CProduct.GetProductList(m_objProfile, null, false);

                if( objAllProductList != null)
                {
                    System.Boolean bProductSubTypeOk = false;
                    System.Boolean bProductTypeOk = false;
                    System.Boolean bProductTradeMark = false;

                    foreach (CProduct objProduct in objAllProductList)
                    {
                        bProductSubTypeOk = false;
                        bProductTypeOk = false;
                        bProductTradeMark = false;

                        if ((objProductSubTypeList != null) && (objProductSubTypeList.Count > 0))
                        {
                            foreach (CProductSubType objProductSubType in objProductSubTypeList)
                            {
                                if (objProduct.ProductSubType.ID.CompareTo(objProductSubType.ID) == 0)
                                {
                                    bProductSubTypeOk = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            bProductSubTypeOk = true;
                        }

                        if ((objProductTypeList != null) && (objProductTypeList.Count > 0))
                        {
                            foreach (CProductType objProductType in objProductTypeList)
                            {
                                if (objProduct.ProductType.ID.CompareTo(objProductType.ID) == 0)
                                {
                                    bProductTypeOk = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            bProductTypeOk = true;
                        }

                        if ((objProductTradeMarkList != null) && (objProductTradeMarkList.Count > 0))
                        {
                            foreach (CProductTradeMark objProductTradeMark in objProductTradeMarkList)
                            {
                                if (objProduct.ProductTradeMark.ID.CompareTo(objProductTradeMark.ID) == 0)
                                {
                                    bProductTradeMark = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            bProductTradeMark = true;
                        }

                        if ((bProductSubTypeOk == true) && (bProductTypeOk == true) && (bProductTradeMark == true))
                        {
                            m_objProductList.Add(objProduct);
                        }
                    }
                        

                }
                objAllProductList = null;
                gridControlProductList.DataSource = m_objProductList;

                this.splitContainerControl.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.gridControlProductList)).EndInit();
                splitContainerControl.SplitterPosition = 500;
                splitContainerControl.Refresh();
                bRet = true;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка обновления списка. Текст ошибки: " + f.Message);
            }
            finally
            {
                m_bDisableEvents = false;
                this.Cursor = Cursors.Default;
            }

            return bRet;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                LoadProductList();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnRefresh_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

        private void btnImportProduct_Click(object sender, EventArgs e)
        {
            try
            {
                CProductSubType objSubTypeForNewProduct = new CProductSubType();
                objSubTypeForNewProduct.ID_Ib = ERP_Mercury.Global.Consts.iPartsubTypeIdForNewProduct;

                frmImportProductList obj_frmImportProductList = new frmImportProductList(m_objProfile, m_objMenuItem, objSubTypeForNewProduct);
                if (obj_frmImportProductList != null)
                {
                    obj_frmImportProductList.ShowDialog();
                    if (obj_frmImportProductList.DialogResult == DialogResult.OK)
                    {
                        //
                    }

                    obj_frmImportProductList.Dispose();
                    obj_frmImportProductList = null;
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка mitemImportParts_Click. Текст ошибки: " + f.Message);
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
                    "SendMessageToLog. Текст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        #endregion

        #region Свойства товара
        /// <summary>
        /// Возвращает ссылку на выбранный в списке товар
        /// </summary>
        /// <returns>ссылка на товар</returns>
        private CProduct GetSelectedProduct()
        {
            CProduct objRet = null;
            try
            {
                if ((((DevExpress.XtraGrid.Views.Grid.GridView)gridControlProductList.MainView).RowCount > 0) &&
                    (((DevExpress.XtraGrid.Views.Grid.GridView)gridControlProductList.MainView).FocusedRowHandle >= 0))
                {
                    System.Guid uuidID = (System.Guid)(((DevExpress.XtraGrid.Views.Grid.GridView)gridControlProductList.MainView)).GetFocusedRowCellValue("ID");

                    if ((m_objProductList != null) && (m_objProductList.Count > 0) && (uuidID.CompareTo(System.Guid.Empty) != 0))
                    {
                        foreach (CProduct objProduct in m_objProductList)
                        {
                            if (objProduct.ID.CompareTo(uuidID) == 0)
                            {
                                objRet = objProduct;
                                break;
                            }
                        }
                    }
                }
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка поиска выбранного товара. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return objRet;
        }
        /// <summary>
        /// Отображает свойства товара
        /// </summary>
        /// <param name="objProduct">товар</param>
        private void ShowProductProperties(CProduct objProduct)
        {
            try
            {
                this.splitContainerControl.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeListPartsDetail)).BeginInit();


                // штрих-коды
                System.String strErr = "";
                if ((objProduct.BarcodeList == null) || (objProduct.BarcodeList.Count == 0))
                {
                    objProduct.LoadBarCodeList(m_objProfile, null, ref strErr);
                }

                treeListPartsDetail.Nodes.Clear();

                if (objProduct != null)
                {
                    treeListPartsDetail.AppendNode(new object[] { "Товар", objProduct.Name }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Артикул", objProduct.Article }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Оригинальное название", objProduct.OriginalName }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Товарная марка", objProduct.ProductTradeMarkName }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Товарная группа", objProduct.ProductTypeName }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Товарная подгруппа", objProduct.ProductSubTypeName }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Товарная линия", objProduct.ProductLineName }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Категория товара", ((objProduct.ProductCategory == null) ? "" : objProduct.ProductCategory.Name) }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Страна производства", ((objProduct.Country == null) ? "" : objProduct.Country.Name) }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Код ТНВЭД", objProduct.CodeTNVD }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Референсе", objProduct.Reference }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Количество в упаковке", objProduct.PackQuantity }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Кол-во для расчета заказа", objProduct.PackQuantityForCalc }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Количество в коробке", objProduct.BoxQuantity }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Вес товара, кг.", System.String.Format("{0:### ### ##0.000}", objProduct.Weight) }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Вес пласт. тары, кг.", System.String.Format("{0:### ### ##0.000}", objProduct.PlasticContainerWeight) }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Вес бумаж. тары, кг.", System.String.Format("{0:### ### ##0.000}", objProduct.PaperContainerWeight) }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Спирт, %", System.String.Format("{0:### ### ##0.000}", objProduct.AlcoholicContentPercent) }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Активен", ((objProduct.IsActive == true) ? "Активен" : "Неактивен") }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Неактуален", ((objProduct.IsNotValid == true) ? "Неактуален" : "Актуален") }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Подв. неактуальности", ((objProduct.IsActualNotValid == true) ? "Подтверждена" : "Не подтверждена") }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Штрих-коды", objProduct.BarcodeListString }, null);
                    treeListPartsDetail.AppendNode(new object[] { "Тариф поставщика", System.String.Format("{0:### ### ##0.000}", objProduct.VendorPrice) }, null);
                }

                this.splitContainerControl.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.treeListPartsDetail)).EndInit();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка отображения свойств товара. Текст ошибки: " + f.Message);
            }
            return;
        }
        private void gridViewProductList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                ShowProductProperties(GetSelectedProduct());
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка смены записи в списке товаров. Текст ошибки: " + f.Message);
            }
        }
        private void txtBarCode_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                if (treeListBarCode.FocusedNode != null)
                {
                    treeListBarCode.FocusedNode.SetValue(colBarCode, txtBarCode.Text);
                    SetPropertiesModified(true, enTabProperties.Properties);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка смены записи в списке штрих-кодов. Текст ошибки: " + f.Message);
            }
        }
        private void AddBarcode()
        {
            try
            {
                treeListBarCode.AppendNode(new object[] { " ", "0000000000000" }, null);
                SetPropertiesModified(true, enTabProperties.Properties);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка добавления нового штрих-кода. Текст ошибки: " + f.Message);
            }
        }
        private void GenerateBarcode()
        {
            try
            {
                System.String strErr = "";
                System.String strNewBarCode = CProduct.GenerateBarCode(m_objProfile, ref strErr);
                if (strNewBarCode != "")
                {
                    treeListBarCode.AppendNode(new object[] { " ", strNewBarCode }, null);
                    SetPropertiesModified(true, enTabProperties.Properties);
                }
                else
                {
                    SendMessageToLog("Ошибка создания нового штрих-кода. Текст ошибки: " + strErr);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка создания нового штрих-кода. Текст ошибки: " + f.Message);
            }
        }

        private void btnAddBarcode_Click(object sender, EventArgs e)
        {
            try
            {
                //AddBarcode();
                GenerateBarcode();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка смены записи в списке штрих-кодов. Текст ошибки: " + f.Message);
            }
        }

        private void RemoveBarcode()
        {
            try
            {
                if ((treeListBarCode.Nodes.Count == 0) || (treeListBarCode.FocusedNode == null)) { return; }
                treeListBarCode.Nodes.Remove( treeListBarCode.FocusedNode ) ;
                SetPropertiesModified(true, enTabProperties.Properties);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка удаления штрих-кода. Текст ошибки: " + f.Message);
            }
        }

        private void btnDeleteBarCode_Click(object sender, EventArgs e)
        {
            try
            {
                RemoveBarcode();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка удаления записи в списке штрих-кодов. Текст ошибки: " + f.Message);
            }
        }

        private void gridViewProductList_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "IsIncludeInStockShip")
                {
                    System.Drawing.Image img = ERP_Mercury.Common.Properties.Resources.warning;
                    if ((e.CellValue != null) && (System.Convert.ToBoolean(e.CellValue) == false) )
                    {
                        Rectangle rImg = new Rectangle(e.Bounds.X - 6 + e.Bounds.Width / 2, e.Bounds.Y + (e.Bounds.Height - img.Size.Height) / 2, img.Width, img.Height);
                        e.Graphics.DrawImage(img, rImg);
                        Rectangle r = e.Bounds;
                        e.Handled = true;
                    }
                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("gridViewProductList_CustomDrawCell\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;

        }
        private void gridViewProductList_RowCountChanged(object sender, EventArgs e)
        {
            try
            {
                ShowProductProperties(GetSelectedProduct());
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка смены записи в списке товаров. Текст ошибки: " + f.Message);
            }
        }

        #endregion

        #region Редактирование товара
        private void gridControlProductList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                CProduct objSelectedProduct = GetSelectedProduct();
                LoadProductDetailInEditor(objSelectedProduct);
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования набора. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }
        /// <summary>
        /// Редактирование набора
        /// </summary>
        /// <param name="objProduct">выбранный набор</param>
        private void LoadProductDetailInEditor(CProduct objProduct)
        {
            try
            {
                if (objProduct == null) { return; }
                m_bDisableEvents = true;
                if (m_bIsComboBoxFill == false) { LoadComboBoxItems(); }
                System.String strErr = "";

                m_objSelectedProduct = objProduct;
                lblCustomerIfo.Text = m_objSelectedProduct.ProductFullName;

                m_objSelectedProduct.LoadBarCodeList(m_objProfile, null, ref strErr);
                treeListBarCode.Nodes.Clear();
                if (m_objSelectedProduct.BarcodeList != null)
                {
                    foreach (System.String strItem in m_objSelectedProduct.BarcodeList)
                    {
                        treeListBarCode.AppendNode(new object[] { " ", strItem }, null);
                    }
                    treeListBarCode.FocusedNode = (treeListBarCode.Nodes.Count > 0) ? treeListBarCode.Nodes[0] : null;
                    txtBarCode.Text = (treeListBarCode.FocusedNode == null) ? "" : System.Convert.ToString(treeListBarCode.FocusedNode.GetValue(colBarCode));
                }

                this.tableLayoutPanelDetail.SuspendLayout();
                cboxProductCategory.SelectedItem = null;
                pictureBox.Image = null;
                // загружаем картинку
                m_objSelectedProduct.LoadImageFromDB(m_objProfile, ref strErr);

                pictureBox.Image = m_objSelectedProduct.ImageProduct;

                // загружаем привязку к складам
                treeList.Nodes.Clear();
                List<CProductLinkToStock> objListProductLinkToStock = CProductLinkToStock.GetProductLinkToStockList(m_objProfile, null, m_objSelectedProduct);
                if( objListProductLinkToStock != null )
                {
                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = null;
                    foreach (CProductLinkToStock objProductLinkToStock in objListProductLinkToStock)
                    {
                        objNode = null;
                        foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objExistNode in treeList.Nodes)
                        {
                            if (objExistNode.Tag == null) { continue; }
                            if (((CProductLinkToStock)objExistNode.Tag).Stock.ID.CompareTo(objProductLinkToStock.Stock.ID) == 0)
                            {
                                objNode = objExistNode;
                                break;
                            }
                        }

                        if (objNode == null)
                        {
                            objNode = treeList.AppendNode(new object[] { objProductLinkToStock.ToString() }, null);
                            objNode.Tag = objProductLinkToStock;
                            foreach (DevExpress.XtraTreeList.Columns.TreeListColumn objColumn in treeList.Columns)
                            {
                                if ((objColumn == colCompany) || (objColumn.Tag == null)) { continue; }
                                objNode.SetValue(objColumn, false);

                                objNode.ImageIndex = -1;
                                objNode.SelectImageIndex = -1;
                                objNode.StateImageIndex = -1;

                            }
                        }
                        foreach (DevExpress.XtraTreeList.Columns.TreeListColumn objColumn in treeList.Columns)
                        {
                            if ((objColumn == colCompany) || (objColumn.Tag == null)) { continue; }
                            if (objProductLinkToStock.OrderType.Id.CompareTo(((COrderType)objColumn.Tag).Id) == 0)
                            {
                                objNode.SetValue(objColumn, objProductLinkToStock.bLink);
                                if (objProductLinkToStock.bLink == true)
                                {
                                    objNode.ImageIndex = 0;
                                    objNode.SelectImageIndex = 0;
                                    objNode.StateImageIndex = 0;
                                }
                            }
                        }

                    }
                }

                txtID_Ib.Text = m_objSelectedProduct.ID_Ib.ToString();
                txtName.Text = m_objSelectedProduct.Name;
                txtOriginalName.Text = m_objSelectedProduct.OriginalName;
                txtShortName.Text = m_objSelectedProduct.ShortName;
                txtArticle.Text = m_objSelectedProduct.Article;
                txtCodeTNVED.Text = m_objSelectedProduct.CodeTNVD;
                txtReference.Text = m_objSelectedProduct.Reference;
                txtCertificate.Text = m_objSelectedProduct.Certificate;
                calcAlcoholicContentPercent.Value = m_objSelectedProduct.AlcoholicContentPercent;
                calcPlasticContainerWeight.Value = m_objSelectedProduct.PlasticContainerWeight;
                calcVendorPrice.Value = m_objSelectedProduct.VendorPrice;
                calcWeight.Value = m_objSelectedProduct.Weight;
                calcPaperContainerWeight.Value = m_objSelectedProduct.PaperContainerWeight;
                spinBoxQuantity.Value = m_objSelectedProduct.BoxQuantity;
                spinPackQuantity.Value = m_objSelectedProduct.PackQuantity;
                spinPackQuantityForCalc.Value = m_objSelectedProduct.PackQuantityForCalc;
                checkActualNotValid.Checked = m_objSelectedProduct.IsActualNotValid;
                checkIsActive.Checked = m_objSelectedProduct.IsActive;
                checkNotValid.Checked = m_objSelectedProduct.IsNotValid;
                txtCertificateNum.Text = "";
                txtCertificateWhoGive.Text = "";
                cboxCertificateType.SelectedItem = null;
                dtCertificateBeginDate.DateTime = System.DateTime.Today;
                dtCertificateEndDate.DateTime = System.DateTime.Today;

                cboxCountry.SelectedItem = null;
                cboxCurrency.SelectedItem = null;
                cboxMeasure.SelectedItem = null;
                cboxProductTradeMark.SelectedItem = null;
                cboxProductType.SelectedItem = null;
                cboxProductSubType.SelectedItem = null;

                if ((m_objSelectedProduct.Country != null) && (cboxCountry.Properties.Items.Count > 0))
                {
                    foreach (object objItem in cboxCountry.Properties.Items)
                    {
                        if (((CCountry)objItem).ID.CompareTo(m_objSelectedProduct.Country.ID) == 0)
                        {
                            cboxCountry.SelectedItem = objItem;
                            break;
                        }
                    }
                }

                if ((m_objSelectedProduct.Currency != null) && (cboxCurrency.Properties.Items.Count > 0))
                {
                    foreach (object objItem in cboxCurrency.Properties.Items)
                    {
                        if (((CCurrency)objItem).ID.CompareTo(m_objSelectedProduct.Currency.ID) == 0)
                        {
                            cboxCurrency.SelectedItem = objItem;
                            break;
                        }
                    }
                }

                if ((m_objSelectedProduct.ProductCategory != null) && ( cboxProductCategory.Properties.Items.Count > 0))
                {
                    foreach (object objItem in cboxProductCategory.Properties.Items)
                    {
                        if (((CProductCategory)objItem).ID.CompareTo(m_objSelectedProduct.ProductCategory.ID) == 0)
                        {
                            cboxProductCategory.SelectedItem = objItem;
                            break;
                        }
                    }
                }

                if ((m_objSelectedProduct.Measure != null) && (cboxMeasure.Properties.Items.Count > 0))
                {
                    foreach (object objItem in cboxMeasure.Properties.Items)
                    {
                        if (((CMeasure)objItem).ID.CompareTo(m_objSelectedProduct.Measure.ID) == 0)
                        {
                            cboxMeasure.SelectedItem = objItem;
                            break;
                        }
                    }
                }

                if ((m_objSelectedProduct.ProductTradeMark != null) && (cboxProductTradeMark.Properties.Items.Count > 0))
                {
                    foreach (object objItem in cboxProductTradeMark.Properties.Items)
                    {
                        if (((CProductTradeMark)objItem).ID.CompareTo(m_objSelectedProduct.ProductTradeMark.ID) == 0)
                        {
                            cboxProductTradeMark.SelectedItem = objItem;
                            break;
                        }
                    }
                }

                if ((m_objSelectedProduct.ProductType != null) && (cboxProductType.Properties.Items.Count > 0))
                {
                    foreach (object objItem in cboxProductType.Properties.Items)
                    {
                        if (((CProductType)objItem).ID.CompareTo(m_objSelectedProduct.ProductType.ID) == 0)
                        {
                            cboxProductType.SelectedItem = objItem;
                            break;
                        }
                    }
                }

                if ((m_objSelectedProduct.ProductSubType != null) && (cboxProductSubType.Properties.Items.Count > 0))
                {
                    foreach (object objItem in cboxProductSubType.Properties.Items)
                    {
                        if (((CProductSubType)objItem).ID.CompareTo(m_objSelectedProduct.ProductSubType.ID) == 0)
                        {
                            cboxProductSubType.SelectedItem = objItem;
                            break;
                        }
                    }
                }

                LoadCertificateForProduct(m_objSelectedProduct);

                LoadKitItemsForProduct(m_objSelectedProduct);

                this.tableLayoutPanelDetail.ResumeLayout(false);

                SetReadOnlyPropertiesControls(true);
                SetPropertiesModified(false, enTabProperties.All);

                tabControl.SelectedTabPage = tabDetail;
                tabControlPartProperties.SelectedTabPage = tabPagePartProperties;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования набора. Текст ошибки: " + f.Message);
            }
            finally
            {
                m_bDisableEvents = false;
                btnCancel.Enabled = true;
            }
            return;
        }
        private void treeListBarCode_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            try
            {
                m_bDisableEvents = true;
                txtBarCode.Text = ((treeListBarCode.FocusedNode == null) ? "" : System.Convert.ToString(treeListBarCode.FocusedNode.GetValue(colBarCode)));
                m_bDisableEvents = false;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("SetModeReadOnlyForProductDetail. Текст ошибки: " + f.Message);
            }
            return;
        }
        /// <summary>
        /// Установка режима просмотра/редактирования состава набора
        /// </summary>
        /// <param name="bReadOnly"></param>
        private void SetModeReadOnlyForProductDetail( System.Boolean bReadOnly )
        {
            try
            {
                btnEdit.Enabled = bReadOnly;
                btnSave.Enabled = !bReadOnly;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("SetModeReadOnlyForProductDetail. Текст ошибки: " + f.Message);
            }
            return;
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                SetReadOnlyPropertiesControls(false);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnEdit_Click. Текст ошибки: " + f.Message);
            }
            return;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ConfirmProductDetail();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnSave_Click. Текст ошибки: " + f.Message);
            }
            return;
        }
        /// <summary>
        /// Сохраняет привязку товара к складам
        /// </summary>
        /// <param name="objProduct">товар</param>
        private System.Boolean SaveLinkToStock( CProduct objProduct )
        {
            System.Boolean bRet = false;
            try
            {
                if (treeList.Nodes.Count > 0)
                {
                    List<CProductLinkToStock> objList = new List<CProductLinkToStock>();
                    CProductLinkToStock objLink = null;
                    foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes)
                    {
                        if (objNode.Tag == null) { continue; }
                        foreach( DevExpress.XtraTreeList.Columns.TreeListColumn objColumn in treeList.Columns )
                        {
                            if( ( objColumn == colCompany ) || ( objColumn.Tag == null ) ) { continue; }
                            if( System.Convert.ToBoolean( objNode.GetValue( objColumn ) ) == true )
                            {
                                objLink = new CProductLinkToStock();
                                objLink.Stock = ( ( CProductLinkToStock )(objNode.Tag ) ).Stock;
                                objLink.OrderType = ( ( COrderType )objColumn.Tag );
                                objLink.bLink = true;

                                objList.Add(objLink);
                            }
                        }                      
                    }

                    System.String strErr = "";
                    if (CProductLinkToStock.SetProductLinkToStockList(m_objProfile, null, objProduct, objList, ref strErr) == false)
                    {
                        SendMessageToLog(strErr);
                    }
                    else
                    {
                        bRet = true;
                    }
                    

                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnSave_Click. Текст ошибки: " + f.Message);
            }
            return bRet;
        }

        /// <summary>
        /// Сохраняет изменения в составе набора
        /// </summary>
        private void ConfirmProductDetail()
        {
            CProduct objProductForSave = null;
            try
            {
                if (m_objSelectedProduct != null)
                {
                    System.String strErr = "";

                    objProductForSave = new CProduct();
                    objProductForSave.ID = m_objSelectedProduct.ID;
                    objProductForSave.ID_Ib = m_objSelectedProduct.ID_Ib;
                    objProductForSave.Name = txtName.Text;
                    objProductForSave.Article = txtArticle.Text;
                    objProductForSave.OriginalName = txtOriginalName.Text;
                    objProductForSave.ShortName = txtShortName.Text;
                    objProductForSave.Reference = txtReference.Text;
                    objProductForSave.CodeTNVD = txtCodeTNVED.Text;
                    objProductForSave.IsActive = checkIsActive.Checked;
                    objProductForSave.IsActualNotValid = checkActualNotValid.Checked;
                    objProductForSave.IsNotValid = checkNotValid.Checked;
                    objProductForSave.VendorPrice = calcVendorPrice.Value;
                    objProductForSave.AlcoholicContentPercent = calcAlcoholicContentPercent.Value;
                    objProductForSave.Weight = calcWeight.Value;
                    objProductForSave.PlasticContainerWeight = calcPlasticContainerWeight.Value;
                    objProductForSave.PaperContainerWeight = calcPaperContainerWeight.Value;
                    objProductForSave.Currency = ((cboxCurrency.SelectedItem == null) ? null : (CCurrency)cboxCurrency.SelectedItem);
                    objProductForSave.Country = ((cboxCountry.SelectedItem == null) ? null : (CCountry)cboxCountry.SelectedItem);
                    objProductForSave.Measure = ((cboxMeasure.SelectedItem == null) ? null : (CMeasure)cboxMeasure.SelectedItem);
                    objProductForSave.ProductCategory = ((cboxProductCategory.SelectedItem == null) ? null : (CProductCategory)cboxProductCategory.SelectedItem);
                    objProductForSave.ProductTradeMark = ((cboxProductTradeMark.SelectedItem == null) ? null : (CProductTradeMark)cboxProductTradeMark.SelectedItem);
                    objProductForSave.ProductType = ((cboxProductType.SelectedItem == null) ? null : (CProductType)cboxProductType.SelectedItem);
                    objProductForSave.ProductSubType = ((cboxProductSubType.SelectedItem == null) ? null : (CProductSubType)cboxProductSubType.SelectedItem);
                    objProductForSave.PackQuantity = System.Convert.ToInt32(spinPackQuantity.Value);
                    objProductForSave.PackQuantityForCalc = System.Convert.ToInt32(spinPackQuantityForCalc.Value);
                    objProductForSave.BoxQuantity = System.Convert.ToInt32(spinBoxQuantity.Value);
                    objProductForSave.ImageProduct = pictureBox.Image;
                    objProductForSave.BarcodeList = new List<string>();
                    foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListBarCode.Nodes)
                    {
                        objProductForSave.BarcodeList.Add(System.Convert.ToString(objNode.GetValue(colBarCode)));
                    }

                    if( (IsChanged == true) || (IsChangedImage == true))
                    {
                        // вносились изменеия в логистические характеристикисвойства товара
                        if (IsAllParamFill() == true)
                        {
                            // сперва мы схраняем информацию в IB
                            if (objProductForSave.UpdateInIB(m_objProfile, ref strErr) == true)
                            {
                                // а теперь в ERP_Mercury
                                if (objProductForSave.Update(m_objProfile) == true)
                                {
                                    SetPropertiesModified(false, enTabProperties.Properties);
                                    SetPropertiesModified(false, enTabProperties.Image);

                                    m_objSelectedProduct.Name = objProductForSave.Name;
                                    m_objSelectedProduct.Article = objProductForSave.Article;
                                    m_objSelectedProduct.OriginalName = objProductForSave.OriginalName;
                                    m_objSelectedProduct.ShortName = objProductForSave.ShortName;
                                    m_objSelectedProduct.Reference = objProductForSave.Reference;
                                    m_objSelectedProduct.CodeTNVD = objProductForSave.CodeTNVD;
                                    
                                    m_objSelectedProduct.IsActive = objProductForSave.IsActive;
                                    m_objSelectedProduct.IsActualNotValid = objProductForSave.IsActualNotValid;
                                    m_objSelectedProduct.IsNotValid = objProductForSave.IsNotValid;
                                    m_objSelectedProduct.VendorPrice = objProductForSave.VendorPrice;
                                    m_objSelectedProduct.AlcoholicContentPercent = objProductForSave.AlcoholicContentPercent;
                                    m_objSelectedProduct.Weight = objProductForSave.Weight;
                                    m_objSelectedProduct.PlasticContainerWeight = objProductForSave.PlasticContainerWeight;
                                    m_objSelectedProduct.PaperContainerWeight = objProductForSave.PaperContainerWeight;
                                    m_objSelectedProduct.Currency = objProductForSave.Currency;
                                    m_objSelectedProduct.Country = objProductForSave.Country;
                                    m_objSelectedProduct.Measure = objProductForSave.Measure;
                                    m_objSelectedProduct.ProductCategory = objProductForSave.ProductCategory;
                                    m_objSelectedProduct.ProductTradeMark = objProductForSave.ProductTradeMark;
                                    m_objSelectedProduct.ProductType = objProductForSave.ProductType;
                                    m_objSelectedProduct.ProductSubType = objProductForSave.ProductSubType;
                                    m_objSelectedProduct.PackQuantity = objProductForSave.PackQuantity;
                                    m_objSelectedProduct.PackQuantityForCalc = objProductForSave.PackQuantityForCalc;
                                    m_objSelectedProduct.BoxQuantity = objProductForSave.BoxQuantity;
                                    m_objSelectedProduct.ImageProduct = objProductForSave.ImageProduct;
                                    m_objSelectedProduct.BarcodeList = objProductForSave.BarcodeList;

                                }
                            }
                        }

                    }

                    // состав набора
                    if( IsChangedKit == true )
                    {
                        List<CProduct> objProductKid = new List<CProduct>();
                        CProduct objItemKid = null;
                        foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in this.treeListKit.Nodes)
                        {
                            if (objNode.Tag == null) { continue; }
                            objItemKid = (CProduct)objNode.Tag;
                            objItemKid.QuantityInKit = System.Convert.ToInt32(objNode.GetValue(colDetailProductQty));
                            objProductKid.Add(objItemKid);
                        }
                        objItemKid = null;

                        if( CProduct.SaveProductKit(m_objProfile, objProductKid, m_objSelectedProduct.ID, ref strErr) == true )
                        {
                            SetPropertiesModified(false, enTabProperties.Kit);
                        }
                    }
                    
                    // сертификаты
                    if ((IsChangedCertificate == true) && 
                        (m_objProfile.GetClientsRight().GetState(ERP_Mercury.Global.Consts.strDR_EditCertificate) == true))
                    {
                        if (IsAllParamsInCertificateListValid(ref strErr) == false)
                        {
                            DevExpress.XtraEditors.XtraMessageBox.Show(strErr + "\n" + "Пожалуйста, исправьте.", "Внимание",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                            return;
                        }

                        objProductForSave.Certificate = txtCertificate.Text;
                        if (CCertificate.UpdateProductCertificate(m_objProfile, null, objProductForSave.Certificate, objProductForSave.ID, ref strErr) == true)
                        {
                            m_objSelectedProduct.Certificate = objProductForSave.Certificate;
                        }
                        else
                        {
                            SendMessageToLog(strErr);
                        }

                        List<CCertificate> objCertificateList = new List<CCertificate>();
                            
                        foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListCertificate.Nodes)
                        {
                            if (objNode.Tag == null) { continue; }
                            objCertificateList.Add((CCertificate)objNode.Tag);
                        }
                        if(CCertificate.AssignCertificateListWithProduct( m_objProfile, null, objCertificateList, objProductForSave.ID, ref strErr ) == true)
                        {
                            SetPropertiesModified(false, enTabProperties.Certificate);
                        }

                    }

                    // склады отгрузки
                    if( IsChangedLinkedToStock == true )
                    {
                        if( SaveLinkToStock(m_objSelectedProduct) == true )
                        {
                            SetPropertiesModified(false, enTabProperties.LinkedToStock);
                        }
                    }

                    if( ( IsChanged == false ) && ( IsChangedCertificate == false ) && ( IsChangedImage == false ) &&
                        ( IsChangedKit == false ) && ( IsChangedLinkedToStock == false ) )
                    {
                        ShowProductProperties(m_objSelectedProduct);
                        tabControl.SelectedTabPage = tabView;
                    }

                }

            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в свойствах товара. Текст ошибки: " + f.Message);
            }
            finally
            {
                objProductForSave = null;
            }
            return;
        }
        /// <summary>
        /// Проверяет все ли обязательные поля заполнены
        /// </summary>
        /// <returns>true - все обязательные поля заполнены; false - не все полязаполнены</returns>
        private System.Boolean IsAllParamFill()
        {
            System.Boolean bRet = false;
            try
            {
                bRet = ((cboxCurrency.SelectedItem != null) && (cboxMeasure.SelectedItem != null) &&
                    (cboxProductSubType.SelectedItem != null) && (cboxProductTradeMark.SelectedItem != null) &&
                    (cboxProductType.SelectedItem != null) && (txtName.Text != "") && (txtOriginalName.Text != "") &&
                     (txtArticle.Text != "")  );
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка проверки заполнения обязательных свойств товара. Текст ошибки: " + f.Message);
            }
            return bRet;
        }
        /// <summary>
        /// Проверяет, все ли поля в реквизитах сертификата заполнены
        /// </summary>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - все обязательные поля заполнены; false - не все полязаполнены</returns>
        private System.Boolean IsAllParamsInCertificateListValid( ref System.String strErr )
        {
            System.Boolean bRet = true;
            try
            {
                CCertificate objCertificate = null;
                System.String strCertificateNum = System.String.Empty;

                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListCertificate.Nodes)
                {
                    if ((objNode.Tag == null) || (objNode.Tag.GetType().Name != "CCertificate" ) )
                    {
                        strErr = "Не удалось определить реквизиты сертификата в списке.";
                        bRet = false;
                        break;
                    }
                    else
                    {
                        objCertificate = (CCertificate)objNode.Tag;
                        if ( CCertificate.IsAllPropertiesCorrect( objCertificate, ref strErr ) == false)
                        {
                            bRet = false;
                            break;
                        }
                        else
                        {
                            if (objCertificate.Num == strCertificateNum)
                            {
                                strErr = "В списке указаны несколько сертификатов с одинаковым номером.\nИсправьте номер или удалите дубликат.";
                                bRet = false;
                                break;
                            }
                            else
                            {
                                strCertificateNum = objCertificate.Num;
                            }
                        }
                    }
                }

                objCertificate = null;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка проверки заполнения обязательных свойств сертификатов. Текст ошибки: " + f.Message);
            }
            return bRet;
        }
        /// <summary>
        /// Проверяет содержимое элементов управления
        /// </summary>
        private void ValidateProperties()
        {
            try
            {
                cboxCountry.Properties.Appearance.BackColor = ( (cboxCountry.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White );
                cboxCurrency.Properties.Appearance.BackColor = ((cboxCurrency.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                cboxMeasure.Properties.Appearance.BackColor = ((cboxMeasure.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                cboxProductSubType.Properties.Appearance.BackColor = ((cboxProductSubType.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                cboxProductTradeMark.Properties.Appearance.BackColor = ((cboxProductTradeMark.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                cboxProductType.Properties.Appearance.BackColor = ((cboxProductType.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                cboxProductCategory.Properties.Appearance.BackColor = ((cboxProductCategory.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);

                txtName.Properties.Appearance.BackColor = ((txtName.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                txtOriginalName.Properties.Appearance.BackColor = ((txtOriginalName.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                txtShortName.Properties.Appearance.BackColor = ((txtShortName.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                txtArticle.Properties.Appearance.BackColor = ((txtArticle.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                txtCodeTNVED.Properties.Appearance.BackColor = ((txtCodeTNVED.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                txtReference.Properties.Appearance.BackColor = ((txtReference.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                calcAlcoholicContentPercent.Properties.Appearance.BackColor = ((calcAlcoholicContentPercent.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                calcPlasticContainerWeight.Properties.Appearance.BackColor = ((calcPlasticContainerWeight.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                calcVendorPrice.Properties.Appearance.BackColor = ((calcVendorPrice.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                calcWeight.Properties.Appearance.BackColor = ((calcWeight.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                calcPaperContainerWeight.Properties.Appearance.BackColor = ((calcPaperContainerWeight.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                spinBoxQuantity.Properties.Appearance.BackColor = ((spinBoxQuantity.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                spinPackQuantity.Properties.Appearance.BackColor = ((spinPackQuantity.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                spinPackQuantityForCalc.Properties.Appearance.BackColor = ((spinPackQuantityForCalc.Text == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White);

                ValidatePropertiesInTabCertificate();
            }
            catch
            {
            }
            return;
        }
        /// <summary>
        /// Проверяет содержимое элементов управления
        /// </summary>
        private void ValidatePropertiesInTabCertificate()
        {
            try
            {
                cboxCertificateType.Properties.Appearance.BackColor = ((cboxCertificateType.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                txtCertificateNum.Properties.Appearance.BackColor = (((txtCertificateNum.Text == "") || (txtCertificateNum.Text == CCertificate.STR_SetCertificateNum)) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                txtCertificateWhoGive.Properties.Appearance.BackColor = (((txtCertificateWhoGive.Text == "") || (txtCertificateWhoGive.Text == CCertificate.STR_SetWhoGiveCertificate)) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("ValidatePropertiesInTabCertificate. Текст ошибки: " + f.Message);
            }
            return;
        }
        /// <summary>
        /// Отменяет изменения в составе набора
        /// </summary>
        private void CancelChanges()
        {
            try
            {
                tabControl.SelectedTabPage = tabView;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка отмены изменений. Текст ошибки: " + f.Message);
            }
            return;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                CancelChanges();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnCancel_Click. Текст ошибки: " + f.Message);
            }
            return;
        }

        private void treeList_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {

                if (btnEdit.Enabled == true) { return; }

                DevExpress.XtraTreeList.TreeListHitInfo hi = treeList.CalcHitInfo(new Point(e.X, e.Y));
                if ((hi != null) && (hi.Column != null) && (hi.Column != colCompany) && (hi.HitInfoType == DevExpress.XtraTreeList.HitInfoType.Column))
                {
                    SendMessageToLog(hi.HitInfoType.ToString());
                    //this.tableLayoutPanelBgrnd.SuspendLayout();
                    //((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();

                    System.Int32 iCellFalseCount = 0;
                    foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes)
                    {
                        if (System.Convert.ToBoolean(objNode.GetValue(hi.Column)) == false)
                        {
                            iCellFalseCount++;
                            break;
                        }
                    }

                    foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes)
                    {
                        objNode.SetValue(hi.Column, (iCellFalseCount > 0));
                        if (iCellFalseCount > 0)
                        {
                            foreach (DevExpress.XtraTreeList.Columns.TreeListColumn objColumn in treeList.Columns)
                            {
                                if ((objColumn != colCompany) && (objColumn != hi.Column))
                                {
                                    objNode.SetValue(objColumn, false);
                                }
                            }
                        }
                    }

                    //this.tableLayoutPanelBgrnd.ResumeLayout(false);
                    //((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();

                    if (m_bDisableEvents == true) { return; }
                    SetPropertiesModified(true, enTabProperties.LinkedToStock);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnCancel_Click. Текст ошибки: " + f.Message);
            }
            return;
        }

        #endregion

        #region Выпадающие списки
        /// <summary>
        /// загружает информацию в выпадающие списки
        /// </summary>
        private void LoadComboBoxItems()
        {
            if (m_bIsComboBoxFill == true) { return; }
            m_bDisableEvents = true;
            this.Cursor = Cursors.WaitCursor;
            try
            {
                this.tableLayoutPanel1.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.cboxCountry.Properties)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.cboxCurrency.Properties)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.cboxMeasure.Properties)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.cboxProductSubType.Properties)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.cboxProductTradeMark.Properties)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.cboxProductType.Properties)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.cboxProductCategory.Properties)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.cboxCertificateType.Properties)).BeginInit();

                cboxCountry.Properties.Items.Clear();
                cboxCurrency.Properties.Items.Clear();
                cboxMeasure.Properties.Items.Clear();
                cboxProductSubType.Properties.Items.Clear();
                cboxProductTradeMark.Properties.Items.Clear();
                cboxProductType.Properties.Items.Clear();
                cboxProductCategory.Properties.Items.Clear();
                cboxCertificateType.Properties.Items.Clear();

                cboxCountry.Properties.Items.AddRange(CCountry.GetCountryList(m_objProfile, null));
                cboxCurrency.Properties.Items.AddRange(CCurrency.GetCurrencyList(m_objProfile, null));
                cboxMeasure.Properties.Items.AddRange(CMeasure.GetMeasureList(m_objProfile, null));
                cboxProductCategory.Properties.Items.AddRange(CProductCategory.GetProductCategoryList(m_objProfile, null));
                cboxProductSubType.Properties.Items.AddRange(CProductSubType.GetProductSubTypeList(m_objProfile, null));
                cboxProductTradeMark.Properties.Items.AddRange(CProductTradeMark.GetProductTradeMarkList(m_objProfile, null));
                cboxProductType.Properties.Items.AddRange(CProductType.GetProductTypeList(m_objProfile, null));
                cboxCertificateType.Properties.Items.AddRange(CCertificateType.GetCertificateTypeList(m_objProfile, null));

                this.tableLayoutPanel1.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.cboxCountry.Properties)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.cboxCurrency.Properties)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.cboxMeasure.Properties)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.cboxProductSubType.Properties)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.cboxProductTradeMark.Properties)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.cboxProductType.Properties)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.cboxProductCategory.Properties)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.cboxCertificateType.Properties)).EndInit();
                m_bIsComboBoxFill = true;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка обновления выпадающих списков. Текст ошибки: " + f.Message);
            }
            finally
            {
                m_bDisableEvents = false;
                this.Cursor = Cursors.Default;
            }

            return ;
        }
        #endregion

        #region Индикация изменений
        private void SetPropertiesModified(System.Boolean bModified, enTabProperties iModifiedType)
        {
            ValidateProperties();

            switch (iModifiedType)
            {
                case enTabProperties.Properties:
                    {
                        m_bIsChanged = bModified;
                        break;
                    }
                case enTabProperties.LinkedToStock:
                    {
                        IsChangedLinkedToStock = bModified;
                        break;
                    }
                case enTabProperties.Kit:
                    {
                        IsChangedKit = bModified;
                        break;
                    }
                case enTabProperties.Image:
                    {
                        IsChangedImage = bModified;
                        break;
                    }
                case enTabProperties.Certificate:
                    {
                        IsChangedCertificate = bModified;
                        break;
                    }
                default:
                    {
                        m_bIsChanged = bModified;
                        IsChangedLinkedToStock = bModified;
                        IsChangedKit = bModified;
                        IsChangedImage = bModified;
                        IsChangedCertificate = bModified;

                        break;
                    }
            }

            // в том случае, если хоть одна закладка изменялась, делаем проверкку на возможность сохранения
            if (m_bIsChanged || IsChangedLinkedToStock || IsChangedKit || IsChangedImage || IsChangedCertificate)
            {
                btnSave.Enabled = IsAllParamFill();
                btnCancel.Enabled = true;
            }
            else if ((m_bIsChanged == false) && (IsChangedLinkedToStock == false) && (IsChangedKit == false) && (IsChangedImage == false) && (IsChangedCertificate == false))
            {
                btnSave.Enabled = false;
                btnCancel.Enabled = true;
            }
            tabPagePartProperties.Image = ((m_bIsChanged == true) ? ERP_Mercury.Common.Properties.Resources.warning : null);
            tabPagePartImage.Image = ((IsChangedImage == true) ? ERP_Mercury.Common.Properties.Resources.warning : null);
            tabPageStock.Image = ((IsChangedLinkedToStock == true) ? ERP_Mercury.Common.Properties.Resources.warning : null);
            tabPageKit.Image = ((IsChangedKit == true) ? ERP_Mercury.Common.Properties.Resources.warning : null);
            tabPageCertificate.Image = ((IsChangedCertificate == true) ? ERP_Mercury.Common.Properties.Resources.warning : null);

        }
        /// <summary>
        /// Включает/отключает элементы управления для отображения свойств
        /// </summary>
        /// <param name="bEnable">признак "включить/выключить"</param>
        private void SetReadOnlyPropertiesControls(System.Boolean bEnable)
        {
            try
            {
                cboxCountry.Properties.ReadOnly = bEnable;
                cboxCurrency.Properties.ReadOnly = bEnable;
                cboxMeasure.Properties.ReadOnly = bEnable;
                // 2012.05.18
                // ! Заменить доступ к подгруппе на доступ по динамическому праву
                cboxProductSubType.Properties.ReadOnly = true; // bEnable;
                cboxProductTradeMark.Properties.ReadOnly = bEnable;
                cboxProductType.Properties.ReadOnly = bEnable;
                cboxProductCategory.Properties.ReadOnly = bEnable;

                txtID_Ib.Properties.ReadOnly = true;
                txtName.Properties.ReadOnly = bEnable;
                txtOriginalName.Properties.ReadOnly = bEnable;
                txtShortName.Properties.ReadOnly = bEnable;
                txtArticle.Properties.ReadOnly = bEnable;
                txtCodeTNVED.Properties.ReadOnly = bEnable;
                txtReference.Properties.ReadOnly = bEnable;
                calcAlcoholicContentPercent.Properties.ReadOnly = bEnable;
                calcPlasticContainerWeight.Properties.ReadOnly = bEnable;
                calcVendorPrice.Properties.ReadOnly = bEnable;
                calcWeight.Properties.ReadOnly = bEnable;
                calcPaperContainerWeight.Properties.ReadOnly = bEnable;
                spinBoxQuantity.Properties.ReadOnly = bEnable;
                spinPackQuantity.Properties.ReadOnly = bEnable;
                spinPackQuantityForCalc.Properties.ReadOnly = bEnable;
                checkActualNotValid.Properties.ReadOnly = bEnable;
                checkIsActive.Properties.ReadOnly = bEnable;
                checkNotValid.Properties.ReadOnly = bEnable;
                treeList.OptionsBehavior.Editable = !bEnable;
                treeListBarCode.OptionsBehavior.Editable = !bEnable;
                treeListKit.OptionsBehavior.Editable = !bEnable;
                btnAddKitItem.Enabled = !bEnable;
                btnDeleteKitItem.Enabled = !bEnable;
                txtBarCode.Properties.ReadOnly = bEnable;

                if (bEnable == true)
                {
                    // включен режим "только просмотр"
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnLoadImage.Enabled = false;
                    btnClearImage.Enabled = false;
                    btnAddBarcode.Enabled = false;
                    btnDeleteBarCode.Enabled = false;
                }
                else
                {
                    // включен режим "редактирование"
                    btnLoadImage.Enabled = true;
                    btnClearImage.Enabled = true;
                    btnAddBarcode.Enabled = true;
                    btnDeleteBarCode.Enabled = true;
                    btnSave.Enabled = m_bIsChanged;
                    btnCancel.Enabled = true;
                }

                SetReadOnlyPropertiesControlsInTabCertificate(bEnable);

                btnEdit.Enabled = bEnable;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("DisableEnablePropertiesControls. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        /// <summary>
        /// Включает/отключает элементы управления для отображения свойств
        /// </summary>
        /// <param name="bEnable">признак "включить/выключить"</param>
        private void SetReadOnlyPropertiesControlsInTabCertificate(System.Boolean bEnable)
        {
            try
            {
                txtCertificate.Properties.ReadOnly = bEnable;
                cboxCertificateType.Properties.ReadOnly = bEnable;
                txtCertificateNum.Properties.ReadOnly = bEnable;
                txtCertificateWhoGive.Properties.ReadOnly = bEnable;
                dtCertificateBeginDate.Properties.ReadOnly = bEnable;
                dtCertificateEndDate.Properties.ReadOnly = bEnable;
                txtCertificateNumForWaybill.Properties.ReadOnly = bEnable;
                checkCertificateIsActive.Properties.ReadOnly = bEnable;
                txtCertificateDescription.Properties.ReadOnly = bEnable;
                checkNotExistsEndDate.Properties.ReadOnly = bEnable;

                mitemAddCertificate.Enabled = !bEnable;
                mitemDeleteCertificate.Enabled = !bEnable;
                btnAddCertificate.Enabled = !bEnable;
                btnDeleteCertificate.Enabled = (!bEnable && (treeListCertificate.FocusedNode != null ) );

                btnCertificateImageView.Enabled = ( treeListCertificate.FocusedNode != null );
                btnCertificateImageLoad.Enabled = !bEnable;
                btnCertificateImageClear.Enabled = !bEnable;

            }
            catch (System.Exception f)
            {
                SendMessageToLog("SetReadOnlyPropertiesControlsInTabCertificate. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        
        private void ProductProperties_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (m_bDisableEvents == true) { return; }
            try
            {
                if (e.NewValue != null)
                {
                    if ((sender == calcAlcoholicContentPercent) || (sender == calcPaperContainerWeight) ||
                        (sender == calcPlasticContainerWeight) || (sender == calcVendorPrice) ||
                        (sender == calcWeight) || (sender == spinBoxQuantity) || (sender == spinPackQuantity)) 
                    {
                        if (sender == calcAlcoholicContentPercent)
                        {
                            if ((System.Convert.ToDecimal(e.NewValue) < 0) || (System.Convert.ToDecimal(e.NewValue) > 100))
                            {
                                e.Cancel = true;
                            }
                            else
                            {
                                SetPropertiesModified(true, enTabProperties.Properties);
                            }
                        }
                        else
                        {
                            if (System.Convert.ToDecimal(e.NewValue) < 0)
                            {
                                e.Cancel = true;
                            }
                            else
                            {
                                SetPropertiesModified(true, enTabProperties.Properties);
                            }
                        }
                    }
                    else if ( ( sender == txtCertificateNum ) || ( sender == txtCertificateWhoGive ) || ( sender == dtCertificateBeginDate ) || 
                            ( sender == dtCertificateEndDate ) || ( sender == cboxCertificateType ) )
                    {
                        DevExpress.XtraTreeList.Nodes.TreeListNode objSelectedNode = treeListCertificate.FocusedNode;
                        if ((objSelectedNode != null) && (objSelectedNode.Tag != null))
                        {
                            CCertificate obCertificate = (CCertificate)objSelectedNode.Tag;
                            if (sender == txtCertificateNum)
                            {
                                objSelectedNode.SetValue(colCertificateNum, System.Convert.ToString(e.NewValue));
                                obCertificate.Num = System.Convert.ToString(e.NewValue);
                            }
                            if (sender == txtCertificateWhoGive)
                            {
                                objSelectedNode.SetValue(colCertificateWhoGive, System.Convert.ToString(e.NewValue));
                                obCertificate.WhoGive = System.Convert.ToString(e.NewValue);
                            }
                            if (sender == dtCertificateBeginDate)
                            {
                                objSelectedNode.SetValue(colCertificateBeginDate, System.Convert.ToDateTime(e.NewValue));
                                obCertificate.BeginDate = System.Convert.ToDateTime(e.NewValue);
                            }
                            if (sender == dtCertificateEndDate)
                            {
                                if (dtCertificateEndDate.EditValue == null)
                                {
                                    objSelectedNode.SetValue(colCertificateEndDate, System.String.Empty);
                                    obCertificate.EndDate = System.DateTime.MaxValue;
                                }
                                else
                                {
                                    objSelectedNode.SetValue(colCertificateEndDate, System.Convert.ToDateTime(e.NewValue));
                                    obCertificate.EndDate = System.Convert.ToDateTime(e.NewValue);
                                }
                            }
                            if (sender == checkCertificateIsActive)
                            {
                                obCertificate.IsActive = System.Convert.ToBoolean(e.NewValue);
                                objSelectedNode.SetValue(colCertificateIsActive, obCertificate.IsActive);
                            }
                        }
                        SetPropertiesModified(true, enTabProperties.Certificate);
                        ValidatePropertiesInTabCertificate();
                    }
                    else if (sender == txtCertificate)
                    {
                        SetPropertiesModified(true, enTabProperties.Certificate);
                    }
                    
                }
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения свойств товара.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        private void checkNotExistsEndDate_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkNotExistsEndDate.CheckState == CheckState.Checked)
                {
                    dtCertificateEndDate.EditValue = null;
                    dtCertificateEndDate.Enabled = false;
                }
                else
                {
                    dtCertificateEndDate.Enabled = true;
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "checkNotExistsEndDate_CheckedChanged.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        private void ProductProperties_SelectedValueChanged(object sender, EventArgs e)
        {
            if (m_bDisableEvents == true) { return; }
            try
            {
                // в том случае, если изменения произошли в элементах управления на закладке "Сертификаты",
                // находим сертификат в списке и синхронизируем его свойства с данными в элементах управления
                if( (sender == cboxCertificateType) || (sender == txtCertificateNum) || (sender == txtCertificateWhoGive) || 
                    (sender == dtCertificateBeginDate) || (sender == dtCertificateEndDate) || (sender == cboxCertificateType) || 
                    ( sender == txtCertificate ) || (sender == txtCertificateNumForWaybill) || (sender == txtCertificateDescription ) ||
                    (sender == checkCertificateIsActive) )
                {
                    DevExpress.XtraTreeList.Nodes.TreeListNode objSelectedNode = treeListCertificate.FocusedNode;

                    if ((objSelectedNode != null) && (objSelectedNode.Tag != null))
                    {
                        CCertificate objCertificate = (CCertificate)objSelectedNode.Tag;

                        objCertificate.CertificateType = ((cboxCertificateType.SelectedItem != null) ? (CCertificateType)cboxCertificateType.SelectedItem : null);
                        objSelectedNode.SetValue(colCertificateType, ((objCertificate.CertificateType == null) ? "" : objCertificate.CertificateType.Name));

                        objCertificate.Num = txtCertificateNum.Text;
                        objSelectedNode.SetValue(colCertificateNum, objCertificate.Num);

                        objCertificate.WhoGive = txtCertificateWhoGive.Text;
                        objSelectedNode.SetValue(colCertificateWhoGive, objCertificate.WhoGive);

                        objCertificate.BeginDate = dtCertificateBeginDate.DateTime;
                        objSelectedNode.SetValue(colCertificateBeginDate, objCertificate.BeginDate);

                        objCertificate.EndDate = dtCertificateEndDate.DateTime;
                        objSelectedNode.SetValue(colCertificateEndDate, objCertificate.EndDate);

                        objCertificate.IsActive = checkCertificateIsActive.Checked;
                        objSelectedNode.SetValue(colCertificateIsActive, objCertificate.IsActive);

                        objCertificate.NumForWaybill = txtCertificateNumForWaybill.Text;
                        objCertificate.Description = txtCertificateDescription.Text;
                        //objCertificate.ImageCertificate = pictureBoxCertificate.Image;

                        objCertificate = null;
                    }
                    SetPropertiesModified(true, enTabProperties.Certificate);
                }
                else
                {
                    SetPropertiesModified(true, enTabProperties.Properties);
                }

            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения свойств товара.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        private void treeList_CellValueChanging(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
            if (m_bDisableEvents == true) { return; }
            try
            {
                IsChangedLinkedToStock = true;
                SetPropertiesModified(true, enTabProperties.LinkedToStock);
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения свойств товара.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

        }
        private void treeList_CellValueChanged(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                if ((e.Column != null) && (e.Column != colCompany))
                {
                    System.Boolean bOR = false;

                    foreach (DevExpress.XtraTreeList.Columns.TreeListColumn objColumn in treeList.Columns)
                    {
                        if (objColumn != colCompany)
                        {
                            if (System.Convert.ToBoolean(e.Node.GetValue(objColumn)) == true)
                            {
                                bOR = true;
                                break;
                            }
                        }
                    }

                    e.Node.ImageIndex = (bOR == true) ? 0 : -1; ;
                    e.Node.SelectImageIndex = (bOR == true) ? 0 : -1; ;
                    e.Node.StateImageIndex = (bOR == true) ? 0 : -1; ;
                }
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения свойств. Текст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        private void gridViewProductList_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            try
            {
                if (System.Convert.ToBoolean(gridViewProductList.GetRowCellValue(e.RowHandle, gridViewProductList.Columns["IsCheck"])) == true)
                {
                    e.Appearance.BackColor = Color.BurlyWood;
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("gridViewProductList_RowStyle. " + f.Message);
            }
            return;
        }

        #endregion

        #region Выделить/Снять выделение в списке
        private void SelectAllRows( System.Boolean bSelect )
        {
            try
            {
                if (gridViewProductList.RowCount > 0)
                {

                    this.splitContainerControl.SuspendLayout();
                    ((System.ComponentModel.ISupportInitialize)(this.gridControlProductList)).BeginInit();

                    for (System.Int32 i = 0; i < gridViewProductList.RowCount; i++)
                    {
                        m_objProductList[gridViewProductList.GetDataSourceRowIndex(i)].IsCheck = bSelect;
                    }
                    gridControlProductList.RefreshDataSource();

                    this.splitContainerControl.ResumeLayout(false);
                    ((System.ComponentModel.ISupportInitialize)(this.gridControlProductList)).EndInit();
                    
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("SelectAllRows. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

        private void mitemSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                SelectAllRows(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("mitemSelectAll_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

        private void mitemDeselectAll_Click(object sender, EventArgs e)
        {
            try
            {
                SelectAllRows(false);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("mitemDeselectAll_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

        private void gridViewProductList_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {

           try
            {
                int[] arr = gridViewProductList.GetSelectedRows();

                if (arr.Length > 1)
                {
                    this.splitContainerControl.SuspendLayout();
                    ((System.ComponentModel.ISupportInitialize)(this.gridControlProductList)).BeginInit();

                    for (System.Int32 i = 0; i < arr.Length; i++)
                    {
                        m_objProductList[gridViewProductList.GetDataSourceRowIndex(arr[i])].IsCheck = true;
                    }
                    gridControlProductList.RefreshDataSource();

                    this.splitContainerControl.ResumeLayout(false);
                    ((System.ComponentModel.ISupportInitialize)(this.gridControlProductList)).EndInit();

                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("gridViewProductList_SelectionChanged. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return ;

        }

        #endregion

        #region Изображение товара
        private void btnClearImage_Click(object sender, EventArgs e)
        {
            try
            {
                pictureBox.Image = null;
                SetPropertiesModified(true, enTabProperties.Image);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка удаления картинки. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return ;
        }

        private void LoadImage()
        {
            openFileDialog.Filter = "JPG files (*.jpg)|*.jpg|BMP files (*.bmp)|*.bmp|All files (*.*)|*.*";
            //openFileDialog.FilterIndex = 2;
            //openFileDialog.RestoreDirectory = true;

            if ( ( openFileDialog.ShowDialog() == DialogResult.OK) && (openFileDialog.FileName != "" ) )
            {
                try
                {
                    pictureBox.Load(openFileDialog.FileName);
                    SetPropertiesModified(true, enTabProperties.Image);
                }
                catch (System.Exception f)
                {
                    SendMessageToLog("Ошибка загрузки картинки. Текст ошибки: " + f.Message);
                }
            }

            return;
        }
        private void btnLoadImage_Click(object sender, EventArgs e)
        {
            try
            {
                LoadImage();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnLoadImage_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        #endregion

        #region Экспорт в MS Excel
        private string ShowSaveFileDialog(string title, string filter)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            //string name = Application.ProductName;
            string name = "Справочник товаров";
            int n = name.LastIndexOf(".") + 1;
            if (n > 0) name = name.Substring(n, name.Length - n);
            dlg.Title = "Экспорт списка товаров " + title;
            dlg.FileName = name;
            dlg.Filter = filter;
            if (dlg.ShowDialog() == DialogResult.OK) return dlg.FileName;
            return "";
        }
        private void OpenFile(string fileName)
        {
            try
            {
                if (DevExpress.XtraEditors.XtraMessageBox.Show("Вы хотите открыть этот файл?", "Экспорт в MS Excel...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        System.Diagnostics.Process process = new System.Diagnostics.Process();
                        process.StartInfo.FileName = fileName;
                        process.StartInfo.Verb = "Open";
                        process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                        process.Start();
                    }
                    catch
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(this, "Cannot find an application on your system suitable for openning the file with exported data.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка метода OpenFile.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        //<sbExportToHTML>
        private void ExportTo(DevExpress.XtraExport.IExportProvider provider,
            DevExpress.XtraGrid.Views.Grid.GridView objGridView)
        {
            try
            {
                Cursor currentCursor = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;

                this.FindForm().Refresh();

                BaseExportLink link = objGridView.CreateExportLink(provider);
                (link as GridViewExportLink).ExpandAll = false;
                //link.Progress += new DevExpress.XtraGrid.Export.ProgressEventHandler(Export_Progress);
                link.ExportTo(true);
                provider.Dispose();
                //link.Progress -= new DevExpress.XtraGrid.Export.ProgressEventHandler(Export_Progress);

                Cursor.Current = currentCursor;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка метода OpenFile.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        private void sbExportToXLS_Click(object sender, System.EventArgs e)
        {
            try
            {
                DevExpress.XtraGrid.Views.Grid.GridView objGridView = gridViewProductList;

                if ((objGridView == null) || (objGridView.RowCount == 0))
                {
                    return;
                }

                string fileName = ShowSaveFileDialog("Microsoft Excel Document", "Microsoft Excel|*.xls");
                if (fileName != "")
                {
                    ExportTo(new ExportXlsProvider(fileName), objGridView);
                    OpenFile(fileName);
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка метода sbExportToXLS_Click.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        #endregion

        #region Настройки внешнего вида журналов
        /// <summary>
        /// Считывает настройки журналов из реестра
        /// </summary>
        public void RestoreLayoutFromRegistry()
        {
            System.String strReestrPath = this.m_objProfile.GetRegKeyBase();
            strReestrPath += ("\\Tools\\");
            try
            {
                gridViewProductList.RestoreLayoutFromRegistry(strReestrPath + gridViewProductList.Name);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка загрузки настроек списка заявок.\n\nТекст ошибки : " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally // очищаем занимаемые ресурсы
            {
            }

            return;
        }
        /// <summary>
        /// Записывает настройки журналов в реестр
        /// </summary>
        public void SaveLayoutToRegistry()
        {
            System.String strReestrPath = this.m_objProfile.GetRegKeyBase();
            strReestrPath += ("\\Tools\\");
            try
            {
                gridViewProductList.SaveLayoutToRegistry( strReestrPath + gridViewProductList.Name );
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка записи настроек списка заявок.\n\nТекст ошибки : " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally // очищаем занимаемые ресурсы
            {
            }

            return;
        }
        #endregion

        #region Фильтрация по подгруппам
        /// <summary>
        /// Устанавливает фильтр по подгруппам
        /// </summary>
        private void SetProductSubTypeFilter()
        {
            try
            {
                if (m_objFrmFilterForPartsList != null)
                {
                    if (m_objFrmFilterForPartsList.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if(m_objFrmFilterForPartsList.bFilterIsSet == true)
                        {
                            // перезагружаем список товаров, но только для выбранных подгрупп
                            LoadProductList(m_objFrmFilterForPartsList.SelectedProductSubTypeList, 
                                m_objFrmFilterForPartsList.SelectedProductTypeList, 
                                m_objFrmFilterForPartsList.SelectedProductTradeMarkList);

                            lblWarningAboutFilter.Text = strWarningAboutFilter;
                        }
                        else
                        {
                            // загружаем весь список товаров
                            LoadProductList();
                            lblWarningAboutFilter.Text = "";
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка фильтрации.Текст ошибки:" + f.Message);
            }
            finally
            {
            }

            return;
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                SetProductSubTypeFilter();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка фильтрации.Текст ошибки:" + f.Message);
            }
            finally
            {
            }

            return;
        }

        #endregion

        #region Экспорт в DBF
        private void btnExportToDBF_Click(object sender, EventArgs e)
        {
            try
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    SendMessageToLog("идет экспорт данных в DBF ... ");
                    this.Refresh();

                    string folderPath = (folderBrowserDialog.SelectedPath + "\\"); // "C:\\Temp\\";

                    Cursor = Cursors.WaitCursor;

                    CConvertToDBF.ExportDirectoriesToFileDBF(m_objProfile, folderPath, m_objMenuItem);

                    Cursor = Cursors.Default;

                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnExportToDBF_Click. Текст ошибки: " + f.Message);
            }
            return;

        }
        #endregion

        #region Информация о качестве
        /// <summary>
        /// Загружает список сертификатов для товара
        /// </summary>
        /// <param name="objProduct">товар</param>
        private void LoadCertificateForProduct(CProduct objProduct)
        {
            try
            {
                treeListCertificate.Nodes.Clear();

                List<CCertificate> objCertificateList = CCertificate.GetCertificateTypeList(objProduct, m_objProfile, null);
                if (objCertificateList == null) { return; }

                foreach (CCertificate objCertificate in objCertificateList)
                {
                    ( treeListCertificate.AppendNode(new object[] { objCertificate.CertificateType.Name, objCertificate.Num, 
                        objCertificate.BeginDate, 
                        ( ( objCertificate.EndDate.CompareTo( System.DateTime.MaxValue ) == 0 ) ? System.String.Empty : objCertificate.EndDate.ToShortDateString() ), 
                        objCertificate.WhoGive, objCertificate.IsActive, objCertificate.ExistImage }, null)).Tag = objCertificate;
                }

                objCertificateList = null;

                if (treeListCertificate.Nodes.Count > 0)
                {
                    treeListCertificate.FocusedNode = treeListCertificate.Nodes[0];
                    LoadCertificateInfoInEditor((CCertificate)treeListCertificate.FocusedNode.Tag);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("LoadCertificateForProduct. Текст ошибки: " + f.Message);
            }
            return;

        }
        /// <summary>
        /// Загружает в элементы управления свойства сертификата
        /// </summary>
        /// <param name="objCertificate">Сертификат</param>
        private void LoadCertificateInfoInEditor(CCertificate objCertificate)
        {
            try
            {
                m_bDisableEvents = true;

                this.tableLayoutPanelCertificateEditor.SuspendLayout();

                cboxCertificateType.SelectedItem = null;
                txtCertificateNum.Text = "";
                txtCertificateWhoGive.Text = "";
                dtCertificateBeginDate.EditValue = null;
                dtCertificateEndDate.EditValue = null;
                txtCertificateNumForWaybill.Text = "";
                txtCertificateDescription.Text = "";
                checkCertificateIsActive.Checked = false;
                pictureBoxCertificate.Image = null;
                btnCertificateImageView.Enabled = false;
                checkNotExistsEndDate.CheckState = CheckState.Unchecked;

                if (objCertificate != null)
                {
                    txtCertificateNum.Text = objCertificate.Num;
                    txtCertificateWhoGive.Text = objCertificate.WhoGive;
                    dtCertificateBeginDate.DateTime = objCertificate.BeginDate;
                    if ((objCertificate.EndDate.CompareTo(System.DateTime.MaxValue) == 0) || (objCertificate.EndDate.CompareTo(System.DateTime.MinValue) == 0))
                    {
                        dtCertificateEndDate.EditValue = null;
                        checkNotExistsEndDate.CheckState = CheckState.Checked;
                    }
                    else
                    {
                        dtCertificateEndDate.EditValue = objCertificate.EndDate;
                    }
                    cboxCertificateType.SelectedItem = (objCertificate.CertificateType == null) ? null : cboxCertificateType.Properties.Items.Cast<CCertificateType>().Single<CCertificateType>(x => x.ID.CompareTo(objCertificate.CertificateType.ID) == 0);

                    txtCertificateNumForWaybill.Text = objCertificate.NumForWaybill;
                    txtCertificateDescription.Text = objCertificate.Description;
                    checkCertificateIsActive.Checked = objCertificate.IsActive;
                    pictureBoxCertificate.Image = ((objCertificate.ExistImage == true) ? ERP_Mercury.Common.Properties.Resources.Document_2_Check : ERP_Mercury.Common.Properties.Resources.Document_2_Error);
                    btnCertificateImageView.Enabled = objCertificate.ExistImage;
                }

            }
            catch (System.Exception f)
            {
                SendMessageToLog("LoadCertificateInfoInEditor. Текст ошибки: " + f.Message);
            }
            finally
            {
                m_bDisableEvents = false;
                this.tableLayoutPanelCertificateEditor.ResumeLayout(false);
                ValidatePropertiesInTabCertificate();
            }
            return;
        }

        private void treeListCertificate_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            try
            {
                if( ( e.Node == null ) || ( e.Node.Tag == null ) ) {return;}

                CCertificate objCertificate = ( CCertificate )e.Node.Tag;

                LoadCertificateInfoInEditor(objCertificate);

            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListCertificate_FocusedNodeChanged. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private void treeListCertificate_CustomDrawNodeImages(object sender, DevExpress.XtraTreeList.CustomDrawNodeImagesEventArgs e)
        {
            try
            {
                if (treeListCertificate.Nodes.Count == 0) { return; }
                if (e.Node == null) { return; }
                int Y = e.SelectRect.Top + (e.SelectRect.Height - imglNodes.Images[0].Height) / 2;
                if (e.Node.Tag != null)
                {
                    System.String strErr = System.String.Empty;
                    if (CCertificate.IsAllPropertiesCorrect((CCertificate)e.Node.Tag, ref strErr) == true)
                    {
                        try
                        {
                            e.Graphics.DrawImage(imglNodes.Images[0], new Point(e.SelectRect.X, Y));
                            e.Handled = true;
                        }
                        catch { }
                    }
                    else
                    {
                        try
                        {
                            e.Graphics.DrawImage(imglNodes.Images[1], new Point(e.SelectRect.X, Y));
                            e.Handled = true;
                        }
                        catch { }
                    }
                }
            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(null, "Ошибка отрисовки картинок в узлах\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        /// <summary>
        /// Добавить новый сертификат
        /// </summary>
        private void AddNewCertificate()
        {
            try
            {
                CCertificate objNewCertificate = new CCertificate() 
                {
                    Num = CCertificate.STR_SetCertificateNum,
                    NumForWaybill = "",
                    WhoGive = CCertificate.STR_SetWhoGiveCertificate,
                    CertificateType = ((cboxCertificateType.Properties.Items.Count > 0) ? (CCertificateType)cboxCertificateType.Properties.Items[0] : null), 
                    BeginDate = System.DateTime.Today, EndDate = new DateTime(System.DateTime.Today.Year, 12, 31), ExistImage = false 
                };

                DevExpress.XtraTreeList.Nodes.TreeListNode objNewNode = treeListCertificate.AppendNode(new object[] { objNewCertificate.CertificateType.Name, objNewCertificate.Num, 
                        objNewCertificate.BeginDate, null, objNewCertificate.WhoGive, 
                        objNewCertificate.IsActive, objNewCertificate.ExistImage }, null);

                objNewNode.Tag = objNewCertificate;

                treeListCertificate.FocusedNode = objNewNode;

                SetPropertiesModified(true, enTabProperties.Certificate);

                LoadCertificateInfoInEditor(objNewCertificate);
            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(null, "Ошибка добавления сертификата в список\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        /// <summary>
        /// Удалить сертификат
        /// </summary>
        private void RemoveCertificate()
        {
            try
            {
                if ((treeListCertificate.Nodes.Count == 0) || (treeListCertificate.FocusedNode == null)) { return; }

                System.Int32 iIndxFocusedNode = treeListCertificate.GetNodeIndex(treeListCertificate.FocusedNode);

                treeListCertificate.Nodes.RemoveAt(iIndxFocusedNode);

                if (treeListCertificate.Nodes.Count > 0)
                {
                    if( iIndxFocusedNode >= treeListCertificate.Nodes.Count)
                    {
                        iIndxFocusedNode = (treeListCertificate.Nodes.Count - 1);
                    }
                    else if( iIndxFocusedNode > 0 )
                    {
                        iIndxFocusedNode = ( iIndxFocusedNode - 1 );
                    }
                    treeListCertificate.FocusedNode = treeListCertificate.Nodes[iIndxFocusedNode];
                }

                LoadCertificateInfoInEditor(((treeListCertificate.FocusedNode != null) && (treeListCertificate.FocusedNode.Tag != null)) ? (CCertificate)treeListCertificate.FocusedNode.Tag : null);

                SetPropertiesModified(true, enTabProperties.Certificate);
            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(null, "Ошибка удаления сертификата из списка.\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void btnCertificateImageView_Click(object sender, EventArgs e)
        {
            try
            {
                if ((treeListCertificate.FocusedNode != null) && (treeListCertificate.FocusedNode.Tag != null))
                {
                    ShowPartsCertificateImage((CCertificate)treeListCertificate.FocusedNode.Tag);
                }
            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(null, "Ошибка загрузки картинки.\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void LoadImageForPartsCertificate( CCertificate objCertificate )
        {
            if (objCertificate == null) { return; }
            openFileDialog.Filter = "JPG files (*.jpg)|*.jpg|BMP files (*.bmp)|*.bmp|All files (*.*)|*.*";

            if ((openFileDialog.ShowDialog() == DialogResult.OK) && (openFileDialog.FileName != ""))
            {
                try
                {

                    // проверяем, существует ли указанный файл
                    if (System.IO.File.Exists(openFileDialog.FileName) == false)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                            String.Format("Указанный файл не найден.\n{0}", openFileDialog.FileName), "Ошибка",
                           System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return;
                    }

                    // создаем файловый поток
                    using (System.IO.FileStream fs = new System.IO.FileStream(openFileDialog.FileName, System.IO.FileMode.Open))
                    {
                        // FileInfo закачиваемого файла
                        System.IO.FileInfo fi = new System.IO.FileInfo(openFileDialog.FileName);
                        System.String strShortName = fi.Name;
                        System.String strExtension = fi.Extension;
                        int lung = Convert.ToInt32(fi.Length);
                        // Считываем содержимое файла в массив байт.
                        objCertificate.ImageCertificate = new byte[lung];
                        fs.Read( objCertificate.ImageCertificate, 0, lung);
                        fs.Close();

                        objCertificate.ImageCertificateFileName = fi.Name;
                        objCertificate.ExistImage = true;
                        objCertificate.ActionType = enumCertificateActionTypeWithImage.EditImage;
                    }

                    pictureBoxCertificate.Image = ((objCertificate.ExistImage == true) ? ERP_Mercury.Common.Properties.Resources.Document_2_Check : ERP_Mercury.Common.Properties.Resources.Document_2_Error);

                    if ((treeListCertificate.FocusedNode != null) && (treeListCertificate.FocusedNode.Tag != null))
                    {
                        treeListCertificate.FocusedNode.SetValue(colCertificateExistImage, objCertificate.ExistImage);
                    }

                    SetPropertiesModified(true, enTabProperties.Certificate);
                }
                catch (System.Exception f)
                {
                    SendMessageToLog("Ошибка загрузки картинки. Текст ошибки: " + f.Message);
                }
            }

            return;
        }

        private void btnCertificateImageLoad_Click(object sender, EventArgs e)
        {
            try
            {
                if ((treeListCertificate.FocusedNode != null) && (treeListCertificate.FocusedNode.Tag != null))
                {
                    LoadImageForPartsCertificate( ( CCertificate )treeListCertificate.FocusedNode.Tag);
                }
            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(null, "Ошибка загрузки картинки.\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void RemoveImageForPartsCertificate(CCertificate objCertificate)
        {
            if (objCertificate == null) { return; }

            try
            {
                objCertificate.ImageCertificate = null;
                objCertificate.ExistImage = false;
                objCertificate.ImageCertificateFileName = System.String.Empty;
                objCertificate.ActionType = enumCertificateActionTypeWithImage.DeleteImage;

                pictureBoxCertificate.Image = ((objCertificate.ExistImage == true) ? ERP_Mercury.Common.Properties.Resources.Document_2_Check : ERP_Mercury.Common.Properties.Resources.Document_2_Error);

                SetPropertiesModified(true, enTabProperties.Certificate);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка удаления картинки. Текст ошибки: " + f.Message);
            }

            return;
        }

        private void btnCertificateImageClear_Click(object sender, EventArgs e)
        {
            try
            {
                if ((treeListCertificate.FocusedNode != null) && (treeListCertificate.FocusedNode.Tag != null))
                {
                    RemoveImageForPartsCertificate((CCertificate)treeListCertificate.FocusedNode.Tag);
                }
            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(null, "Ошибка удаления картинки.\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        /// <summary>
        /// Запускает файл с вложением
        /// </summary>
        private void ShowPartsCertificateImage(CCertificate objCertificate)
        {
            if (objCertificate == null) { return; }
            if (objCertificate.ExistImage == false) { return; }
            try
            {
                System.String strImageFileName = System.String.Empty;
                System.String strErr = System.String.Empty;

                if (objCertificate.ImageCertificate == null)
                {
                    byte[] arImageBytes = null;
                    System.String strCertificateFileName = System.String.Empty;

                    if (CCertificate.LoadImageFromDB(m_objProfile, objCertificate.ID, ref arImageBytes,
                        ref strCertificateFileName, ref strErr) == false)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("Не удалось загрузить картинку из БД.\n" + strErr, "Внимание",
                           System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        objCertificate.ImageCertificate = arImageBytes;
                        objCertificate.ImageCertificateFileName = strCertificateFileName;
                    }
                }

                // Получить путь к системной папке.
                System.String sysFolder = Environment.GetFolderPath(Environment.SpecialFolder.System);
                System.String tmpFolder = Environment.GetEnvironmentVariable("TMP");

                Random rand = new Random();
                int temp = rand.Next(100); 
                rand = null;

                System.String strFileFullName = String.Format(@"{0}\{1}", tmpFolder, objCertificate.ImageCertificateFileName);
                // Удаляем файл с таким именем
                if (System.IO.File.Exists(strFileFullName) == true)
                {
                    System.IO.File.Delete(strFileFullName);
                }
                if (System.IO.File.Exists(strFileFullName) == true)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Файл с указанным именем либо уже открыт,\nлибо у Вас нет соотвествующих прав доступа.\nОбратитесь к системному администратору.\nФайл: " + strFileFullName, "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }

                // создаем файловый поток
                using (System.IO.FileStream fs = new System.IO.FileStream(strFileFullName, System.IO.FileMode.Create))
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(strFileFullName);
                    int lung = Convert.ToInt32(objCertificate.ImageCertificate.Length);
                    // Считываем содержимое вложения в файл
                    fs.Write(objCertificate.ImageCertificate, 0, lung);
                    fs.Close();
                }

                ProcessStartInfo pInfo = new ProcessStartInfo(strFileFullName) { UseShellExecute = true, FileName = strFileFullName };
                Process p = Process.Start(pInfo);

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка просмотра изображения.\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        private void mitemAddCertificate_Click(object sender, EventArgs e)
        {
            try
            {
                AddNewCertificate();
            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(null, "Ошибка добавления сертификата в список.\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void btnAddCertificate_Click(object sender, EventArgs e)
        {
            try
            {
                AddNewCertificate();
            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(null, "Ошибка добавления сертификата в список.\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void mitemDeleteCertificate_Click(object sender, EventArgs e)
        {
            try
            {
                RemoveCertificate();
            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(null, "Ошибка удаления сертификата из списка.\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void btnDeleteCertificate_Click(object sender, EventArgs e)
        {
            try
            {
                RemoveCertificate();
            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(null, "Ошибка удаления сертификата из списка.\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        #endregion

        #region Набор
        /// <summary>
        /// Загружает список сертификатов для товара
        /// </summary>
        /// <param name="objProduct">товар</param>
        private void LoadKitItemsForProduct(CProduct objProduct)
        {
            try
            {
                treeListKit.Nodes.Clear();

                List<CProduct> objProductList = CProduct.GetPartsDetail(m_objProfile, null, objProduct.ID);
                if (objProductList == null) { return; }

                foreach (CProduct objItem in objProductList)
                {
                    (treeListKit.AppendNode(new object[] { objItem.ProductTradeMarkName, objItem.ProductTypeName, 
                        objItem.ProductSubTypeName, objItem.ProductFullName, objItem.QuantityInKit }, null)).Tag = objItem;
                }

                objProductList = null;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("LoadKitItemsForProduct. Текст ошибки: " + f.Message);
            }
            return;

        }

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
                }
                else
                {
                    m_objFrmPartsList.ProductList = objPartsList;
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
        /// Добавить позиции в набор
        /// </summary>
        private void AddPartListToKit()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                
                SetProductListToForm(m_objProductList);

                if (m_objFrmPartsList != null)
                {
                    DialogResult dlgRes = m_objFrmPartsList.ShowDialog();
                    if ((dlgRes == DialogResult.OK) && (m_objFrmPartsList.SelectedProductList != null) && (m_objFrmPartsList.SelectedProductList.Count > 0))
                    {
                        this.tableLayoutPanelKit.SuspendLayout();
                        ((System.ComponentModel.ISupportInitialize)(this.treeListKit)).BeginInit();
                        m_bDisableEvents = true;
                        System.Boolean bNodeExists = false;
                        foreach (CProduct objProduct in m_objFrmPartsList.SelectedProductList)
                        {
                            bNodeExists = false;

                            foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListKit.Nodes)
                            {
                                if (objNode.Tag == null) { continue; }
                                if ((objProduct.ID.CompareTo(((CProduct)objNode.Tag).ID) == 0) || (objProduct.ID.CompareTo(m_objSelectedProduct.ID) == 0))
                                {
                                    bNodeExists = true;
                                    break;
                                }
                            }
                            if (bNodeExists == false)
                            {
                                (treeListKit.AppendNode(new object[] { objProduct.ProductTradeMarkName, objProduct.ProductTypeName, 
                                    objProduct.ProductSubTypeName, objProduct.ProductFullName, 1 }, null)).Tag = objProduct;
                            }

                        }
                        SetPropertiesModified(true, enTabProperties.Kit);

                        m_bDisableEvents = false;
                        this.tableLayoutPanelKit.ResumeLayout(false);
                        ((System.ComponentModel.ISupportInitialize)(this.treeListKit)).EndInit();
                    }
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка AddPartListToKit. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                SetPropertiesModified(true, enTabProperties.Kit);
            }
            return;
        }

        private void btnAddKitItem_Click(object sender, EventArgs e)
        {
            try
            {
                AddPartListToKit();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка добавления товаров в набор. Текст ошибки: " + f.Message);
            }
        }

        /// <summary>
        /// Удаляет выбранный товар из набора
        /// </summary>
        private void DeleteFocusedKitItem()
        {
            try
            {
                if (treeListKit.FocusedNode == null) { return; }
                this.tableLayoutPanelKit.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeListKit)).BeginInit();

                System.Int32 iNodeIndx = treeListKit.Nodes.IndexOf(treeListKit.FocusedNode);
                treeListKit.Nodes.Remove(treeListKit.FocusedNode);
                if (treeListKit.Nodes.Count > 0)
                {
                    treeListKit.FocusedNode = treeListKit.Nodes[(iNodeIndx > 0) ? (iNodeIndx - 1) : 0];
                }

                SetPropertiesModified(true, enTabProperties.Kit);
                this.tableLayoutPanelKit.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.treeListKit)).EndInit();

            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка удаления товара из набора. Текст ошибки: " + f.Message);
            }
            return;
        }

        private void btnDeleteKitItem_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteFocusedKitItem();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка удаления товара из набора. Текст ошибки: " + f.Message);
            }
            return;
        }

        private void treeListKit_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            try
            {
                btnDeleteKitItem.Enabled = (e.Node != null);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка смены узла в дереве набора. Текст ошибки: " + f.Message);
            }
        }

        private void treeListKit_CellValueChanging(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
            if (m_bDisableEvents == true) { return; }
            try
            {
                SetPropertiesModified(true, enTabProperties.Kit);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListKit_CellValueChanging. Текст ошибки: " + f.Message);
            }
            return;
        }


        #endregion

        private void treeListCertificate_BeforeFocusNode(object sender, DevExpress.XtraTreeList.BeforeFocusNodeEventArgs e)
        {
        }



    }
}
