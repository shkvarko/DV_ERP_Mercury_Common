using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;


namespace ERP_Mercury.Common
{
    public partial class frmImportProductList : DevExpress.XtraEditors.XtraForm
    {
        #region Свойства

        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;
        private CSettingForOperationWithParts m_objImportSetting;
        private List<CProduct> m_objProductList;
        private DevExpress.XtraGrid.Views.Base.ColumnView ColumnView
        {
            get { return gridControlProductList.MainView as DevExpress.XtraGrid.Views.Base.ColumnView; }
        }
        private CProductSubType m_objProductSubTypeForNewProduct;
        private const System.String m_strOmportMode_0 = "Проверка на уникальное сочетание наименования и артикула товара.\r\nКоды марки и группы соответствует кодам в справочнике.";
        private const System.String m_strOmportMode_1 = "Проверка на уникальное значение кода товара.\r\nКоды марки и группы соответствует кодам в справочнике.\r\nОбязательно наличие кода товара в системе поставщика.";
        private const System.String m_strOmportMode_2 = "Импорт всей информации о товаре: ВТМ, марка, группа, подгруппа, линия.\r\nВсе коды и наименования соответствуют данным в системе поставщика.";
        private const System.Int32 m_iImportMode_2 = 2;
        #endregion

        #region Конструктор
        public frmImportProductList(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem, CProductSubType objSubTypeForNewProduct)
        {
            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_objImportSetting = null;
            m_objProductList = null;
            m_objProductSubTypeForNewProduct = objSubTypeForNewProduct;

            InitializeComponent();

            AddGridColumns();
            radioGroupImportMode.SelectedIndex = 0;

            if( m_objProfile.GetClientsRight().GetState( Global.Consts.strDR_ImportAllPartsDirectories ) == false )
            {
                radioGroupImportMode.Properties.Items.RemoveAt(m_iImportMode_2);
            }
            SetInitialParams();
        }
        #endregion

        #region Первоначальные установки
        private void SetInitialParams()
        {
            try
            {
                txtID_Ib.Text = "";
                cboxSheetList.Properties.Items.Clear();
                treeListSettings.Nodes.Clear();

                gridControlProductList.DataSource = null;
                btnImport.Enabled = false;
                memoEditLog.Text = "";


                m_objImportSetting = CSettingForOperationWithParts.GetSettingForOperationWithPartsByName( m_objProfile, null, CSettingForOperationWithParts.strImportSettingName );

                if (m_objImportSetting != null)
                {
                    foreach (CSettingItemForOperationWithParts objPriceType in m_objImportSetting.SettingsList)
                    {
                        treeListSettings.AppendNode(new object[] { objPriceType.ParamName, objPriceType.ColumnID }, null).Tag = objPriceType;
                    }
                }

                cboxVendor.Properties.Items.Clear();
                cboxVendor.Properties.Items.AddRange( CVendor.GetVendorList(m_objProfile, null) );
                if (cboxVendor.Properties.Items.Count > 0)
                {
                    cboxVendor.SelectedIndex = 0;
                }

                SetModeImportData();
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "SetInitialParams.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        private void AddGridColumns()
        {
            ColumnView.Columns.Clear();

            AddGridColumn(ColumnView, "IsCheck", "");
            AddGridColumn(ColumnView, "ID_Ib", "Код");
            AddGridColumn(ColumnView, "Reference", "Референс");
            AddGridColumn(ColumnView, "Name", "Наименование товара");
            AddGridColumn(ColumnView, "AlcoholicContentPercent", "Спирт");
            AddGridColumn(ColumnView, "Article", "Артикул");
            AddGridColumn(ColumnView, "CountryImportName", "Страна ввоза");
            AddGridColumn(ColumnView, "OriginalName", "Оригинальное название");
            AddGridColumn(ColumnView, "ProductTypeName", "Ассортиментная группа");
            AddGridColumn(ColumnView, "ProductTypeIbID", "Код ассортиментной группы");
            AddGridColumn(ColumnView, "ProductTradeMarkName", "Тованая марка");
            AddGridColumn(ColumnView, "ProductTradeMarkIbID", "Код товарной марки");
            AddGridColumn(ColumnView, "ProductMeasureName", "Единица измерения");
            AddGridColumn(ColumnView, "PackQuantity", "Количество товара в коробке, штук");
            AddGridColumn(ColumnView, "Weight", "Вес, кг.");
            AddGridColumn(ColumnView, "CustomTarif", "Таможенный тариф");
            AddGridColumn(ColumnView, "CurrencyCode", "Валюта");
            AddGridColumn(ColumnView, "VendorPrice", "Цена");
            AddGridColumn(ColumnView, "PackQuantityForCalc", "Кол-во в уп-ке, штук");
            AddGridColumn(ColumnView, "BarcodeFirst", "Штрих-код");
            AddGridColumn(ColumnView, "ProductLineIbID", "Код товарной линии");
            AddGridColumn(ColumnView, "ProductLineName", "Товарная линия");
            AddGridColumn(ColumnView, "ProductSubTypeIbID", "Код товарной подгруппы");
            AddGridColumn(ColumnView, "ProductSubTypeName", "Товарная подгруппа");
            AddGridColumn(ColumnView, "ProductSubTypeNDS", "НДС, %");
            AddGridColumn(ColumnView, "ProductSubTypeVendorTariff", "Цена exw");
            AddGridColumn(ColumnView, "CodeTNVD", "Код ТНВЭД");

            foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in ColumnView.Columns)
            {
                objColumn.OptionsColumn.AllowEdit = false;
                objColumn.OptionsColumn.AllowFocus = false;
                objColumn.OptionsColumn.ReadOnly = true;
            }
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

        private void SetModeImportData()
        {
            try
            {
                this.LblInfoImportMode.Text = "";
                switch (radioGroupImportMode.SelectedIndex)
                {
                    case 0:
                    case 1:
                        LblInfoImportMode.Text = ((radioGroupImportMode.SelectedIndex == 0) ? m_strOmportMode_0 : m_strOmportMode_1);
                        checkImportProductLine.Checked = false;
                        checkImportProductSubType.Checked = false;
                        checkImportProductType.Checked = false;
                        checkImportTradeMark.Checked = false;
                        checkImportVTM.Checked = false;
                        checkImportMeasure.Checked = false;
                        groupUpdateAllDirectories.Enabled = false;
                        break;
                    case 2:
                        groupUpdateAllDirectories.Enabled = true;
                        LblInfoImportMode.Text = m_strOmportMode_2;
                        checkImportProductLine.Checked = true;
                        checkImportProductSubType.Checked = true;
                        checkImportProductType.Checked = true;
                        checkImportTradeMark.Checked = true;
                        checkImportVTM.Checked = true;
                        checkImportMeasure.Checked = true;
                        break;
                    default:
                        break;
                }

                lblSelectedImportMode.Text = ( " №" + (radioGroupImportMode.SelectedIndex + 1).ToString() + " " + radioGroupImportMode.Properties.Items[radioGroupImportMode.SelectedIndex].Description );
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "SetModeImportData.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        private void radioGroupImportMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SetModeImportData();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "radioGroupImportMode_SelectedIndexChanged.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        #endregion

        #region Выбор файла
        private void btnFileOpenDialog_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.Refresh();
                    if ((openFileDialog.FileName != "") && (System.IO.File.Exists(openFileDialog.FileName) == true))
                    {
                        txtID_Ib.Text = openFileDialog.FileName;
                        ReadSheetListFromXLSFile(txtID_Ib.Text);

                        btnImport.Enabled = true;
                        memoEditLog.Text = "";
                    }
                }
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "btnFileOpenDialog_Click.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        /// <summary>
        /// Считывает коллекцию листов в файле MS Excel
        /// </summary>
        /// <param name="strFileName">имя файла MS Excel</param>
        private void ReadSheetListFromXLSFile(System.String strFileName)
        {
            if (strFileName == "") { return; }
            if (System.IO.File.Exists(strFileName) == false)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                     "файл \"" + strFileName + "\" не найден.", "Ошибка",
                     System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }

            Excel.Application oXL = null;
            Excel._Workbook oWB;

            System.Int32 iStartRow = 8;
            System.Int32 iCurrentRow = iStartRow;
            object m = Type.Missing;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                oXL = new Excel.Application();
                oWB = (Excel._Workbook)(oXL.Workbooks.Open(strFileName, 0, true, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value));
                cboxSheetList.Properties.Items.Clear();

                foreach (Excel._Worksheet objSheet in oWB.Worksheets)
                {
                    cboxSheetList.Properties.Items.Add( objSheet.Name );
                }

                oWB.Close(Missing.Value, Missing.Value, Missing.Value);
                oXL.Quit();

            }
            catch (System.Exception f)
            {
                oXL = null;
                oWB = null;
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Не удалось получить список листов в файле.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                oWB = null;
                oXL = null;
                if (cboxSheetList.Properties.Items.Count > 0) { cboxSheetList.SelectedIndex = 0; }
                gridControlProductList.DataSource = null;
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            return;
        }
        /// <summary>
        /// Считывает информацию из фала MS Excel
        /// </summary>
        /// <param name="strFileName">имя файла MS Excel</param>
        private void ReadDataFromXLSFile(System.String strFileName)
        {
            if (strFileName == "") { return; }
            if (System.IO.File.Exists(strFileName) == false)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                     "файл \"" + strFileName + "\" не найден.", "Ошибка",
                     System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }

            this.tableLayoutPanel2.SuspendLayout();

            Excel.Application oXL = null;
            Excel._Workbook oWB;

            System.Int32 iStartRow = CSettingForOperationWithParts.GetColumnNumForParam( CSettingForOperationWithParts.strImportParamNameStartRow, treeListSettings, colSettingsName, colSettingsColumnNum);
            System.Int32 iCurrentRow = iStartRow;
            object m = Type.Missing;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (m_objProductList == null) { m_objProductList = new List<CProduct>(); }
                else { m_objProductList.Clear(); }

                System.Int32 iColumnPartsId = CSettingForOperationWithParts.GetColumnNumForParam(CSettingForOperationWithParts.strImportParamNameID, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnReference = CSettingForOperationWithParts.GetColumnNumForParam(CSettingForOperationWithParts.strImportParamNameReference, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnProductName = CSettingForOperationWithParts.GetColumnNumForParam(CSettingForOperationWithParts.strImportParamNameProductName, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnAlcohol = CSettingForOperationWithParts.GetColumnNumForParam(CSettingForOperationWithParts.strImportParamNameAlcohol, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnProductArticle = CSettingForOperationWithParts.GetColumnNumForParam(CSettingForOperationWithParts.strImportParamNameProductArticle, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnBarCode = CSettingForOperationWithParts.GetColumnNumForParam(CSettingForOperationWithParts.strImportParamNameBarCode, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnCountryImport = CSettingForOperationWithParts.GetColumnNumForParam(CSettingForOperationWithParts.strImportParamNameCountryImport, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnProductOriginalName = CSettingForOperationWithParts.GetColumnNumForParam(CSettingForOperationWithParts.strImportParamNameProductOriginalName, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnPartTypeID = CSettingForOperationWithParts.GetColumnNumForParam(CSettingForOperationWithParts.strImportParamNamePartTypeID, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnOwnerID = CSettingForOperationWithParts.GetColumnNumForParam(CSettingForOperationWithParts.strImportParamNameOwnerID, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnPartTypeName = CSettingForOperationWithParts.GetColumnNumForParam(CSettingForOperationWithParts.strImportParamNamePartTypeName, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnOwnerName = CSettingForOperationWithParts.GetColumnNumForParam(CSettingForOperationWithParts.strImportParamNameOwnerName, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnPackQTY = CSettingForOperationWithParts.GetColumnNumForParam(CSettingForOperationWithParts.strImportParamNamePackQTY, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnWeigth = CSettingForOperationWithParts.GetColumnNumForParam(CSettingForOperationWithParts.strImportParamNameWeigth, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnCustom = CSettingForOperationWithParts.GetColumnNumForParam(CSettingForOperationWithParts.strImportParamNameCustom, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnCurrency = CSettingForOperationWithParts.GetColumnNumForParam(CSettingForOperationWithParts.strImportParamNameCurrency, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnPrice = CSettingForOperationWithParts.GetColumnNumForParam(CSettingForOperationWithParts.strImportParamNamePrice, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnMeasure = CSettingForOperationWithParts.GetColumnNumForParam(CSettingForOperationWithParts.strImportParamNameMeasure, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnPackQuantityForCalc = CSettingForOperationWithParts.GetColumnNumForParam(CSettingForOperationWithParts.strImportParamNamePackQuantityForCalc, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnPartLineID = CSettingForOperationWithParts.GetColumnNumForParam(CSettingForOperationWithParts.strImportParamNamePartLineID, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnPartLineName = CSettingForOperationWithParts.GetColumnNumForParam(CSettingForOperationWithParts.strImportParamNamePartLineName, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnPartSubTypeID = CSettingForOperationWithParts.GetColumnNumForParam(CSettingForOperationWithParts.strImportParamNamePartSubTypeID, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnPartSubTypeName = CSettingForOperationWithParts.GetColumnNumForParam(CSettingForOperationWithParts.strImportParamNamePartSubTypeName, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnVTMID = CSettingForOperationWithParts.GetColumnNumForParam(CSettingForOperationWithParts.strImportParamNameVTMID, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnVTMName = CSettingForOperationWithParts.GetColumnNumForParam(CSettingForOperationWithParts.strImportParamNameVTMName, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnNDSValue = CSettingForOperationWithParts.GetColumnNumForParam(CSettingForOperationWithParts.strImportParamNameNDSValue, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnPriceExw = CSettingForOperationWithParts.GetColumnNumForParam(CSettingForOperationWithParts.strImportParamNamePriceExw, treeListSettings, colSettingsName, colSettingsColumnNum);
                System.Int32 iColumnCodeTNVED = CSettingForOperationWithParts.GetColumnNumForParam(CSettingForOperationWithParts.strImportParamNameCodeTNVED, treeListSettings, colSettingsName, colSettingsColumnNum); 

                oXL = new Excel.Application();
                oWB = (Excel._Workbook)(oXL.Workbooks.Open(strFileName, 0, true, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value));

                System.Boolean bStopRead = false;
                System.String strTmp = "";

                Excel._Worksheet objSheet = (Excel._Worksheet)oWB.Worksheets[ this.cboxSheetList.SelectedIndex + 1 ];
                CProduct objProduct = null;

                iCurrentRow = iStartRow;
                bStopRead = false;
                //strTmp = System.Convert.ToString(objSheet2.get_Range(objSheet2.Cells[iCurrentRow, 2], objSheet2.Cells[iCurrentRow, 2]).Value2);
                while (bStopRead == false)
                {
                    // пробежим по строкам и считаем информацию
                    strTmp = System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnProductName], objSheet.Cells[iCurrentRow, iColumnProductName]).Value2);
                    if (strTmp == "")
                    {
                        bStopRead = true;
                    }
                    else
                    {
                        if (System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnProductName], objSheet.Cells[iCurrentRow, iColumnProductName]).Value2) == "")
                        {
                            iCurrentRow++;
                            continue;
                        }

                        objProduct = new CProduct();
                        objProduct.IsCheck = true;

                        if (System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnPartsId], objSheet.Cells[iCurrentRow, iColumnPartsId]).Value2) != "")
                        {
                            objProduct.ID_Ib = System.Convert.ToInt32(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnPartsId], objSheet.Cells[iCurrentRow, iColumnPartsId]).Value2);
                        }
                        objProduct.Reference = System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnReference], objSheet.Cells[iCurrentRow, iColumnReference]).Value2);
                        objProduct.Name = System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnProductName], objSheet.Cells[iCurrentRow, iColumnProductName]).Value2);
                        objProduct.AlcoholicContentPercent = System.Convert.ToDecimal(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnAlcohol], objSheet.Cells[iCurrentRow, iColumnAlcohol]).Value2);
                        objProduct.CustomTarif = System.Convert.ToDecimal(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnCustom], objSheet.Cells[iCurrentRow, iColumnCustom]).Value2);
                        objProduct.Article = System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnProductArticle], objSheet.Cells[iCurrentRow, iColumnProductArticle]).Value2);
                        if (System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnBarCode], objSheet.Cells[iCurrentRow, iColumnBarCode]).Value2) != "")
                        {
                            objProduct.BarcodeList = new List<string>();
                            objProduct.BarcodeList.Add(System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnBarCode], objSheet.Cells[iCurrentRow, iColumnBarCode]).Value2));
                        }
                        objProduct.Country = new CCountry();
                        objProduct.Country.Name = System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnCountryImport], objSheet.Cells[iCurrentRow, iColumnCountryImport]).Value2);
                        objProduct.OriginalName = System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnProductOriginalName], objSheet.Cells[iCurrentRow, iColumnProductOriginalName]).Value2);

                        objProduct.ProductType = new CProductType();
                        objProduct.ProductType.Name = System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnPartTypeName], objSheet.Cells[iCurrentRow, iColumnPartTypeName]).Value2);
                        objProduct.ProductType.ID_Ib = System.Convert.ToInt32(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnPartTypeID], objSheet.Cells[iCurrentRow, iColumnPartTypeID]).Value2);
                        objProduct.ProductType.IsActive = true;
                        objProduct.ProductType.DemandsName = "-";
                        objProduct.ProductType.NDSRate = System.Convert.ToDouble(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnNDSValue], objSheet.Cells[iCurrentRow, iColumnNDSValue]).Value2);

                        objProduct.ProductTradeMark = new CProductTradeMark();
                        objProduct.ProductTradeMark.Name = System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnOwnerName], objSheet.Cells[iCurrentRow, iColumnOwnerName]).Value2);
                        objProduct.ProductTradeMark.ID_Ib = System.Convert.ToInt32(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnOwnerID], objSheet.Cells[iCurrentRow, iColumnOwnerID]).Value2);
                        objProduct.ProductTradeMark.ShortName = objProduct.ProductTradeMark.Name;
                        objProduct.ProductTradeMark.IsActive = true;
                        objProduct.ProductTradeMark.ProductVtm = new CProductVtm();
                        objProduct.ProductTradeMark.ProductVtm.ID_Ib = System.Convert.ToInt32(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnVTMID], objSheet.Cells[iCurrentRow, iColumnVTMID]).Value2);
                        objProduct.ProductTradeMark.ProductVtm.Name = System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnVTMName], objSheet.Cells[iCurrentRow, iColumnVTMName]).Value2);
                        objProduct.ProductTradeMark.ProductVtm.ShortName = objProduct.ProductTradeMark.ProductVtm.Name;
                        objProduct.ProductTradeMark.ProductVtm.IsActive = true;

                        objProduct.ProductSubType = new CProductSubType();
                        objProduct.ProductSubType.IsActive = true;
                        if (System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnPartSubTypeID], objSheet.Cells[iCurrentRow, iColumnPartSubTypeID]).Value2) != "")
                        {
                            objProduct.ProductSubType.ID_Ib = System.Convert.ToInt32(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnPartSubTypeID], objSheet.Cells[iCurrentRow, iColumnPartSubTypeID]).Value2);
                        }
                        if (System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnPartSubTypeName], objSheet.Cells[iCurrentRow, iColumnPartSubTypeName]).Value2) != "")
                        {
                            objProduct.ProductSubType.Name = System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnPartSubTypeName], objSheet.Cells[iCurrentRow, iColumnPartSubTypeName]).Value2);
                        }
                        if (System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnNDSValue], objSheet.Cells[iCurrentRow, iColumnNDSValue]).Value2) != "")
                        {
                            objProduct.ProductSubType.NDS = System.Convert.ToDouble(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnNDSValue], objSheet.Cells[iCurrentRow, iColumnNDSValue]).Value2);
                        }
                        if (System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnPriceExw], objSheet.Cells[iCurrentRow, iColumnPriceExw]).Value2) != "")
                        {
                            objProduct.ProductSubType.VendorTariff = System.Convert.ToDouble(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnPriceExw], objSheet.Cells[iCurrentRow, iColumnPriceExw]).Value2);
                        }

                        if (iColumnCodeTNVED > 0)
                        {
                            if (System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnCodeTNVED], objSheet.Cells[iCurrentRow, iColumnCodeTNVED]).Value2) != "")
                            {
                                objProduct.CodeTNVD = System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnCodeTNVED], objSheet.Cells[iCurrentRow, iColumnCodeTNVED]).Value2);
                            }
                        }

                        objProduct.ProductSubType.ProductLine = new CProductLine();
                        objProduct.ProductSubType.ProductLine.IsActive = true;
                        if (System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnPartLineID], objSheet.Cells[iCurrentRow, iColumnPartLineID]).Value2) != "")
                        {
                            objProduct.ProductSubType.ProductLine.ID_Ib = System.Convert.ToInt32(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnPartLineID], objSheet.Cells[iCurrentRow, iColumnPartLineID]).Value2);
                        }
                        if (System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnPartLineName], objSheet.Cells[iCurrentRow, iColumnPartLineName]).Value2) != "")
                        {
                            objProduct.ProductSubType.ProductLine.Name = System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnPartLineName], objSheet.Cells[iCurrentRow, iColumnPartLineName]).Value2);
                        }

                        objProduct.Measure = new CMeasure();
                        objProduct.Measure.ShortName = System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnMeasure], objSheet.Cells[iCurrentRow, iColumnMeasure]).Value2);
                        objProduct.Measure.Name = objProduct.Measure.ShortName;

                        objProduct.PackQuantity = System.Convert.ToInt32(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnPackQTY], objSheet.Cells[iCurrentRow, iColumnPackQTY]).Value2);
                        objProduct.Weight = System.Convert.ToDecimal(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnWeigth], objSheet.Cells[iCurrentRow, iColumnWeigth]).Value2);
                        objProduct.Currency = new CCurrency(System.Guid.Empty, "", System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnCurrency], objSheet.Cells[iCurrentRow, iColumnCurrency]).Value2), "");
                        objProduct.VendorPrice = System.Convert.ToDecimal(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnPrice], objSheet.Cells[iCurrentRow, iColumnPrice]).Value2);
                        objProduct.PackQuantityForCalc = System.Convert.ToInt32(objSheet.get_Range(objSheet.Cells[iCurrentRow, iColumnPackQuantityForCalc], objSheet.Cells[iCurrentRow, iColumnPackQuantityForCalc]).Value2);


                        iCurrentRow++;
                        m_objProductList.Add(objProduct);
                    }

                }


                oWB.Close(Missing.Value, Missing.Value, Missing.Value);
                oXL.Quit();
            }
            catch (System.Exception f)
            {
                oWB = null;
                oXL = null;
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка экспорта в MS Excel.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                oWB = null;
                oXL = null;
                gridControlProductList.DataSource = m_objProductList;

                this.tableLayoutPanel2.ResumeLayout(false);
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            return;
        }
        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID_Ib.Text != "")
                {
                    ReadDataFromXLSFile(txtID_Ib.Text);
                }
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "btnImport_Click.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        private void gridViewProductList_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "IsCheck")
                {
                    System.Drawing.Image img = ERP_Mercury.Common.Properties.Resources.warning;
                    if ((e.CellValue != null) && (System.Convert.ToBoolean(e.CellValue) == false))
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

        #endregion

        #region Импорт записей в БД
        /// <summary>
        /// Импорт списка товаров в БД
        /// </summary>
        private void SavePartslistTDB()
        {
            try
            {
                memoEditLog.Text = "";
                if ((m_objProductList == null) || (m_objProductList.Count == 0)) { return; }
                this.Cursor = Cursors.WaitCursor;
                this.tableLayoutPanel2.SuspendLayout();

                // нам понадобится уи страны, валюты, единицы измерения, товарной марки и группы
                List<CCountry> objCountryList = CCountry.GetCountryList(m_objProfile, null);
                List<CCurrency> objCurrencyList = CCurrency.GetCurrencyList(m_objProfile, null);
                List<CMeasure> objMeasureList = CMeasure.GetMeasureList(m_objProfile, null);
                List<CProductTradeMark> objProductOwnerList = CProductTradeMark.GetProductTradeMarkList( m_objProfile, null );
                List<CProductType> objProductTypeList = CProductType.GetProductTypeList(m_objProfile, null);

                List<CProduct> objFailProductlist = new List<CProduct>();
                System.Guid objSelectedVendorGuid = ((cboxVendor.SelectedItem == null) ? System.Guid.Empty : ((CVendor)cboxVendor.SelectedItem).ID);
                // пройдемся по списку и попытаемся сохранить
                // в том случае, если запись успешно сохранена, мы удаляем её из списка
                System.String strErr = ""; 
                foreach (CProduct objProduct in m_objProductList)
                {
                    // страна
                    var Country =
                    from num in objCountryList
                    where num.Name == objProduct.Country.Name
                    select num;

                    if ((Country != null) && (Country.Count<CCountry>() > 0))
                    { objProduct.Country = Country.ToList<CCountry>().First<CCountry>(); }
                    else
                    {
                        objFailProductlist.Add(objProduct);
                        memoEditLog.Text += "\r\n" + objProduct.Name + " Ошибка: Не удалось найти в справочнике страну с указанным наименованием.";
                        continue;
                    }

                    // валюта
                    var Currency =
                    from num in objCurrencyList
                    where num.CurrencyAbbr == objProduct.Currency.CurrencyAbbr
                    select num;

                    if ((Currency != null) && (Currency.Count<CCurrency>() > 0))
                    { objProduct.Currency = Currency.ToList<CCurrency>().First<CCurrency>(); }
                    else
                    {
                        objFailProductlist.Add(objProduct);
                        memoEditLog.Text += "\r\n" + objProduct.Name + " Ошибка: Не удалось найти в справочнике валюту с указанным кодом.";
                        continue;
                    }

                    // единица измерения
                    var Measure =
                    from num in objMeasureList
                    where num.ShortName == objProduct.Measure.ShortName
                    select num;

                    if ((Measure != null) && (Measure.Count<CMeasure>() > 0))
                    { objProduct.Measure = Measure.ToList<CMeasure>().First<CMeasure>(); }
                    else
                    {
                        objFailProductlist.Add(objProduct);
                        memoEditLog.Text += "\r\n" + objProduct.Name + " Ошибка: Не удалось найти в справочнике единицу измерения с указанным наименованием.";
                        continue;
                    }

                    // товарная марка
                    var ProductOwner =
                    from num in objProductOwnerList
                    where num.ID_Ib == objProduct.ProductTradeMark.ID_Ib
                    select num;

                    if ((ProductOwner != null) && (ProductOwner.Count<CProductTradeMark>() > 0))
                    { objProduct.ProductTradeMark = ProductOwner.ToList<CProductTradeMark>().First<CProductTradeMark>(); }
                    else
                    {
                        objFailProductlist.Add(objProduct);
                        memoEditLog.Text += "\r\n" + objProduct.Name + " Ошибка: Не удалось найти в справочнике товарную марку с указанным кодом.";
                        continue;
                    }

                    // товарная группа
                    var ProductType =
                    from num in objProductTypeList
                    where num.ID_Ib == objProduct.ProductType.ID_Ib
                    select num;

                    if ((ProductType != null) && (ProductType.Count<CProductType>() > 0))
                    { objProduct.ProductType = ProductType.ToList<CProductType>().First<CProductType>(); }
                    else
                    {
                        objFailProductlist.Add(objProduct);
                        memoEditLog.Text += "\r\n" + objProduct.Name + " Ошибка: Не удалось найти в справочнике товарную группу с указанным кодом.";
                        continue;
                    }

                    objProduct.ProductSubType = m_objProductSubTypeForNewProduct;

                    if (objProduct.Reference.Trim() == "")
                    {
                        objFailProductlist.Add(objProduct);
                        memoEditLog.Text += "\r\n" + objProduct.Name + " Ошибка: Референс товара является обязательным параметром.";
                        continue;
                    }

                    if (objProduct.Import(m_objProfile, ref strErr, objSelectedVendorGuid) == false)
                    {
                        // позиция обработалась с ошибкой
                        objFailProductlist.Add(objProduct);
                        memoEditLog.Text += "\r\n" + objProduct.Name + ": " + strErr;                        
                    }
                    
                }
                objCountryList = null;
                objCurrencyList = null;
                objMeasureList = null;
                objProductOwnerList = null;
                objProductTypeList = null;

                if (objFailProductlist.Count > 0)
                {
                    m_objProductList.Clear();
                    foreach (CProduct objProduct in objFailProductlist)
                    {
                        m_objProductList.Add(objProduct);
                    }

                    objFailProductlist = null;
                }
                else
                {
                    objFailProductlist = null;
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Во время импорта списка товаров возникла ошибка.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                this.tableLayoutPanel2.ResumeLayout(false);
                this.Cursor = Cursors.Default;
            }

            return;
        }
        /// <summary>
        /// Импорт списка товаров в БД
        /// </summary>
        private void SavePartslistToDB_1()
        {
            try
            {
                memoEditLog.Text = "";
                if ((m_objProductList == null) || (m_objProductList.Count == 0)) { return; }
                this.Cursor = Cursors.WaitCursor;
                this.tableLayoutPanel2.SuspendLayout();
                gridControlProductList.DataSource = null;

                // нам понадобится уи страны, валюты, единицы измерения, товарной марки и группы
                List<CCountry> objCountryList = CCountry.GetCountryList(m_objProfile, null);
                List<CCurrency> objCurrencyList = CCurrency.GetCurrencyList(m_objProfile, null);
                List<CMeasure> objMeasureList = CMeasure.GetMeasureList(m_objProfile, null);
                List<CProductTradeMark> objProductOwnerList = CProductTradeMark.GetProductTradeMarkList(m_objProfile, null);
                List<CProductType> objProductTypeList = CProductType.GetProductTypeList(m_objProfile, null);
                List<CProductLine> objProductLineList = CProductLine.GetProductLineList(m_objProfile, null);
                List<CProductSubType> objProductSubtypeList = CProductSubType.GetProductSubTypeList2(m_objProfile);

                List<CProduct> objFailProductlist = new List<CProduct>();

                System.Int32 iResExecutionSP = 0;
                System.String strErr = "";

                // выбран вариант импорта всей информации об ассортименте товара
                // необходимо проверить наличие товарых марок, групп, подгрупп и товарных линий в справочниках и в случае необходимости добавить или отредактировать запись

                // ВТМ
                #region Обновление справочника ВТМ
                memoEditLog.Text += "\r\n" + "обновление справочника ВТМ...";
                System.Threading.Thread.Sleep(1000);
                memoEditLog.Refresh();
                foreach (CProduct objProduct in m_objProductList)
                {
                    if ((objProduct.ProductTradeMark.ProductVtm.ID_Ib == 0) || (objProduct.ProductTradeMark.ProductVtm.Name == ""))
                    {
                        objProduct.IsCheck = false;
                        if (objProduct.ProductTradeMark.ProductVtm.ID_Ib == 0)
                        {
                            objFailProductlist.Add(objProduct);
                            memoEditLog.Text += "\r\n" + objProduct.Name + " Ошибка: Не указан код ВТМ.";
                        }
                        if (objProduct.ProductTradeMark.ProductVtm.Name == "")
                        {
                            objFailProductlist.Add(objProduct);
                            memoEditLog.Text += "\r\n" + objProduct.Name + " Ошибка: Не указано наименование ВТМ.";
                        }
                    }
                    else
                    {
                        if (checkImportVTM.Checked == true)
                        {
                            // добавляем ВТМ в БД
                            // в том случае, если такой ВТМ уже зарегистрирован, будут обновлены значения уникальных идентификаторов без добавления новой записи в справочник
                            iResExecutionSP = 0;
                            strErr = "";
                            if (objProduct.ProductTradeMark.ProductVtm.Add(m_objProfile, ref iResExecutionSP, ref strErr) == true)
                            {
                                objProduct.ProductTradeMark.ProductVtm.Init(m_objProfile, ref strErr);
                                objProduct.IsCheck = true;
                            }
                            else
                            {
                                objProduct.IsCheck = false;
                                objFailProductlist.Add(objProduct);
                                memoEditLog.Text += "\r\n" + objProduct.ProductTradeMark.ProductVtm.Name + " Ошибка: Не удалось зарегистрировать ВТМ. " + strErr;
                            }
                        }
                    }

                }
                #endregion

                // Товарная марка
                #region Обновление справочника товарных марок
                memoEditLog.Text += "\r\n" + "обновление справочника Товарных марок...";
                System.Threading.Thread.Sleep(1000);
                memoEditLog.Refresh();
                foreach (CProduct objProduct in m_objProductList)
                {
                    if( objProduct.IsCheck == false ) { continue; }
                    if ((objProduct.ProductTradeMark.ID_Ib == 0) || (objProduct.ProductTradeMark.Name == ""))
                    {
                        objProduct.IsCheck = false;
                        if (objProduct.ProductTradeMark.ID_Ib == 0)
                        {
                            objFailProductlist.Add(objProduct);
                            memoEditLog.Text += "\r\n" + objProduct.Name + " Ошибка: Не указан код товарной марки.";
                        }
                        if (objProduct.ProductTradeMark.Name == "")
                        {
                            objFailProductlist.Add(objProduct);
                            memoEditLog.Text += "\r\n" + objProduct.Name + " Ошибка: Не указано наименование товарной марки.";
                        }
                    }
                    else
                    {
                        if (checkImportTradeMark.Checked == true)
                        {
                            // добавляем ТМ в БД
                            // в том случае, если такая ТМ уже зарегистрирована, будут обновлены значения уникальных идентификаторов без добавления новой записи в справочник
                            iResExecutionSP = 0;
                            strErr = "";
                            if (objProduct.ProductTradeMark.Add(m_objProfile, ref iResExecutionSP, ref strErr) == true)
                            {
                                objProduct.ProductTradeMark.Init(m_objProfile, ref strErr);
                                objProduct.IsCheck = true;
                            }
                            else
                            {
                                objProduct.IsCheck = false;
                                objFailProductlist.Add(objProduct);
                                memoEditLog.Text += "\r\n" + objProduct.ProductTradeMark.Name + " Ошибка: Не удалось зарегистрировать товарную марку. " + strErr;
                            }
                        }
                        else
                        {
                            List<CProductTradeMark> objFilterProductTradeMarkList = objProductOwnerList.Where(n => n.Name == objProduct.ProductTradeMark.Name).ToList<CProductTradeMark>();
                            if ((objFilterProductTradeMarkList != null) && (objFilterProductTradeMarkList.Count > 0))
                            {
                                objProduct.ProductTradeMark = objFilterProductTradeMarkList.First<CProductTradeMark>();
                                objProduct.IsCheck = true;
                            }
                            else
                            {
                                objProduct.IsCheck = false;
                            }
                            if (objProduct.IsCheck == false)
                            {
                                objFailProductlist.Add(objProduct);
                                memoEditLog.Text += "\r\n" + objProduct.ProductTradeMark.Name + " Ошибка: Не удалось найти в справочнике товарную марку с указанным наименованием.";
                            }
                        }
                    }

                }
                #endregion

                // Товарная группа
                #region Обновление справочника товарных групп
                memoEditLog.Text += "\r\n" + "обновление справочника Товарных групп...";
                System.Threading.Thread.Sleep(1000);
                memoEditLog.Refresh();
                foreach (CProduct objProduct in m_objProductList)
                {
                    if( objProduct.IsCheck == false ) { continue; }
                    if ((objProduct.ProductType.ID_Ib == 0) || (objProduct.ProductType.Name == ""))
                    {
                        objProduct.IsCheck = false;
                        if (objProduct.ProductType.ID_Ib == 0)
                        {
                            objFailProductlist.Add(objProduct);
                            memoEditLog.Text += "\r\n" + objProduct.Name + " Ошибка: Не указан код товарной группы.";
                        }
                        if (objProduct.ProductType.Name == "")
                        {
                            objFailProductlist.Add(objProduct);
                            memoEditLog.Text += "\r\n" + objProduct.Name + " Ошибка: Не указано наименование товарной группы.";
                        }
                    }
                    else
                    {
                        if ( checkImportProductType.Checked == true)
                        {
                            // добавляем товарную группу в БД
                            // в том случае, если такая товарная группа уже зарегистрирована, будут обновлены значения уникальных идентификаторов без добавления новой записи в справочник
                            iResExecutionSP = 0;
                            strErr = "";
                            if (objProduct.ProductType.Add(m_objProfile, ref iResExecutionSP, ref strErr) == true)
                            {
                                objProduct.ProductType.Init(m_objProfile, ref strErr);
                                objProduct.IsCheck = true;
                            }
                            else
                            {
                                objProduct.IsCheck = false;
                                objFailProductlist.Add(objProduct);
                                memoEditLog.Text += "\r\n" + objProduct.ProductType.Name + " Ошибка: Не удалось зарегистрировать товарную группу. " + strErr;
                            }
                        }
                        else
                        {
                            List<CProductType> objFilterProductTypeList = objProductTypeList.Where(n => n.Name == objProduct.ProductType.Name).ToList<CProductType>();
                            if ((objFilterProductTypeList != null) && (objFilterProductTypeList.Count > 0))
                            {
                                objProduct.ProductType = objFilterProductTypeList.First<CProductType>();
                                objProduct.IsCheck = true;
                            }
                            else
                            {
                                objProduct.IsCheck = false;
                            }
                            if (objProduct.IsCheck == false)
                            {
                                objFailProductlist.Add(objProduct);
                                memoEditLog.Text += "\r\n" + objProduct.ProductTradeMark.Name + " Ошибка: Не удалось найти в справочнике товарную группу с указанным наименованием.";
                            }
                        }
                    }

                }
                #endregion

                // Товарная линия
                #region Обновление справочника товарных линий
                memoEditLog.Text += "\r\n" + "обновление справочника Товарных линий...";
                System.Threading.Thread.Sleep(1000);
                memoEditLog.Refresh();
                foreach (CProduct objProduct in m_objProductList)
                {
                    if( objProduct.IsCheck == false ) { continue; }
                    if ((objProduct.ProductSubType.ProductLine.ID_Ib == 0) || (objProduct.ProductSubType.ProductLine.Name == ""))
                    {
                        objProduct.IsCheck = false;
                        if (objProduct.ProductSubType.ProductLine.ID_Ib == 0)
                        {
                            objFailProductlist.Add(objProduct);
                            memoEditLog.Text += "\r\n" + objProduct.Name + " Ошибка: Не указан код товарной линии.";
                        }
                        if (objProduct.ProductSubType.ProductLine.Name == "")
                        {
                            objFailProductlist.Add(objProduct);
                            memoEditLog.Text += "\r\n" + objProduct.Name + " Ошибка: Не указано наименование товарной линии.";
                        }
                    }
                    else
                    {
                        if ( checkImportProductLine.Checked == true)
                        {
                            // добавляем товарную линию в БД
                            // в том случае, если такая товарная группа уже зарегистрирована, будут обновлены значения уникальных идентификаторов без добавления новой записи в справочник
                            iResExecutionSP = 0;
                            strErr = "";
                            if (objProduct.ProductSubType.ProductLine.Add(m_objProfile, ref iResExecutionSP, ref strErr) == true)
                            {
                                objProduct.ProductSubType.ProductLine.Init(m_objProfile, ref strErr);
                                objProduct.IsCheck = true;
                            }
                            else
                            {
                                objProduct.IsCheck = false;
                                objFailProductlist.Add(objProduct);
                                memoEditLog.Text += "\r\n" + objProduct.ProductType.Name + " Ошибка: Не удалось зарегистрировать товарную линию. " + strErr;
                            }
                        }
                        else
                        {
                            List<CProductLine> objFilterProductLineList = objProductLineList.Where(n => n.Name == objProduct.ProductSubType.ProductLine.Name).ToList<CProductLine>();
                            if ((objFilterProductLineList != null) && (objFilterProductLineList.Count > 0))
                            {
                                objProduct.ProductSubType.ProductLine = objFilterProductLineList.First<CProductLine>();
                                objProduct.IsCheck = true;
                            }
                            else
                            {
                                objProduct.IsCheck = false;
                            }
                            if (objProduct.IsCheck == false)
                            {
                                objFailProductlist.Add(objProduct);
                                memoEditLog.Text += "\r\n" + objProduct.ProductSubType.ProductLine.Name + " Ошибка: Не удалось найти в справочнике товарную линию с указанным наименованием.";
                            }
                        }
                    }

                }
                #endregion

                // Товарная подгруппа
                #region Обновление справочника товарных подгрупп
                memoEditLog.Text += "\r\n" + "обновление справочника Товарных подгрупп...";
                System.Threading.Thread.Sleep(1000);
                memoEditLog.Refresh();
                foreach (CProduct objProduct in m_objProductList)
                {
                    if (objProduct.IsCheck == false) { continue; }
                    if ((objProduct.ProductSubType.ID_Ib == 0) || (objProduct.ProductSubType.Name == ""))
                    {
                        objProduct.IsCheck = false;
                        if (objProduct.ProductSubType.ID_Ib == 0)
                        {
                            objFailProductlist.Add(objProduct);
                            memoEditLog.Text += "\r\n" + objProduct.Name + " Ошибка: Не указан код товарной подгруппы.";
                        }
                        if (objProduct.ProductSubType.Name == "")
                        {
                            objFailProductlist.Add(objProduct);
                            memoEditLog.Text += "\r\n" + objProduct.Name + " Ошибка: Не указано наименование товарной подгруппы.";
                        }
                    }
                    else
                    {
                        if (checkImportProductSubType.Checked == true)
                        {
                            // добавляем товарную подгруппу в БД
                            // в том случае, если такая товарная подгруппа уже зарегистрирована, будут обновлены значения уникальных идентификаторов без добавления новой записи в справочник
                            iResExecutionSP = 0;
                            strErr = "";
                            if (objProduct.ProductSubType.Add(m_objProfile, ref iResExecutionSP, ref strErr) == true)
                            {
                                objProduct.ProductSubType.Init(m_objProfile, ref strErr);
                                objProduct.IsCheck = true;
                            }
                            else
                            {
                                objProduct.IsCheck = false;
                                objFailProductlist.Add(objProduct);
                                memoEditLog.Text += "\r\n" + objProduct.ProductSubType.Name + " Ошибка: Не удалось зарегистрировать товарную подгруппу. " + strErr;
                            }
                        }
                        else
                        {
                            List<CProductSubType> objFilterProductSubTypeList = objProductSubtypeList.Where(n => n.Name == objProduct.ProductSubType.Name).ToList<CProductSubType>();
                            if ((objFilterProductSubTypeList != null) && (objFilterProductSubTypeList.Count > 0))
                            {
                                objProduct.ProductSubType = objFilterProductSubTypeList.First<CProductSubType>();
                                objProduct.IsCheck = true;
                            }
                            else
                            {
                                objProduct.ProductSubType = m_objProductSubTypeForNewProduct;
                                objProduct.IsCheck = true;
                            }
                            if (objProduct.IsCheck == false)
                            {
                                objFailProductlist.Add(objProduct);
                                memoEditLog.Text += "\r\n" + objProduct.ProductSubType.Name + " Ошибка: Не удалось найти в справочнике товарную подгруппу с указанным наименованием.";
                            }
                        }
                    }

                }
                #endregion

                // Единица измерения
                #region Обновление справочника единиц измерения
                memoEditLog.Text += "\r\n" + "обновление справочника Единиц измерения...";
                System.Threading.Thread.Sleep(1000);
                memoEditLog.Refresh();
                foreach (CProduct objProduct in m_objProductList)
                {
                    if (objProduct.IsCheck == false) { continue; }
                    if (objProduct.Measure.Name == "")
                    {
                        objProduct.IsCheck = false;
                        if (objProduct.Measure.Name == "")
                        {
                            objFailProductlist.Add(objProduct);
                            memoEditLog.Text += "\r\n" + objProduct.Name + " Ошибка: Не указано наименование единицы измерения.";
                        }
                    }
                    else
                    {
                        if (checkImportMeasure.Checked == true)
                        {
                            // добавляем товарную группу в БД
                            // в том случае, если такая товарная группа уже зарегистрирована, будут обновлены значения уникальных идентификаторов без добавления новой записи в справочник
                            iResExecutionSP = 0;
                            strErr = "";
                            if (objProduct.Measure.Add(m_objProfile, objProduct.ProductType.ID, ref iResExecutionSP, ref strErr) == true)
                            {
                                objProduct.Measure.Init(m_objProfile, ref strErr);
                                objProduct.IsCheck = true;
                            }
                            else
                            {
                                objProduct.IsCheck = false;
                                objFailProductlist.Add(objProduct);
                                memoEditLog.Text += "\r\n" + objProduct.ProductType.Name + " Ошибка: Не удалось зарегистрировать единицу измерения. " + strErr;
                            }
                        }
                        else
                        {
                            List<CMeasure> objFilterMeasureList = objMeasureList.Where(n => n.ShortName.Contains(objProduct.Measure.ShortName) == true).ToList<CMeasure>();
                            if ((objFilterMeasureList != null) && (objFilterMeasureList.Count > 0))
                            {
                                objProduct.Measure = objFilterMeasureList.First<CMeasure>();
                                objProduct.IsCheck = true;
                            }
                            else
                            {
                                objProduct.IsCheck = false;
                            }
                            if (objProduct.IsCheck == false)
                            {
                                objFailProductlist.Add(objProduct);
                                memoEditLog.Text += "\r\n" + objProduct.Measure.ShortName + " Ошибка: Не удалось найти в справочнике единицу измерения с указанным наименованием.";
                            }
                        }
                    }

                }
                #endregion

                // пройдемся по списку и попытаемся сохранить
                // в том случае, если запись успешно сохранена, мы удаляем её из списка
                #region Обновление справочника товаров

                foreach (CProduct objProduct in m_objProductList)
                {
                    if (objProduct.IsCheck == false) { continue; }

                    // страна
                    var Country =
                    from num in objCountryList
                    where num.Name == objProduct.Country.Name
                    select num;

                    if ((Country != null) && (Country.Count<CCountry>() > 0))
                    { objProduct.Country = Country.ToList<CCountry>().First<CCountry>(); }
                    else
                    {
                        objFailProductlist.Add(objProduct);
                        memoEditLog.Text += "\r\n" + objProduct.Name + " Ошибка: Не удалось найти в справочнике страну с указанным наименованием: " + objProduct.Country.Name;
                        continue;
                    }

                    // валюта
                    var Currency =
                    from num in objCurrencyList
                    where num.CurrencyAbbr == objProduct.Currency.CurrencyAbbr
                    select num;

                    if ((Currency != null) && (Currency.Count<CCurrency>() > 0))
                    { objProduct.Currency = Currency.ToList<CCurrency>().First<CCurrency>(); }
                    else
                    {
                        objFailProductlist.Add(objProduct);
                        memoEditLog.Text += "\r\n" + objProduct.Name + " Ошибка: Не удалось найти в справочнике валюту с указанным кодом: " + objProduct.Currency.CurrencyAbbr;
                        continue;
                    }

                    //// единица измерения
                    //var Measure =
                    //from num in objMeasureList
                    //where num.ShortName.Contains(objProduct.Measure.ShortName)
                    //select num;

                    //if ((Measure != null) && (Measure.Count<CMeasure>() > 0))
                    //{ objProduct.Measure = Measure.ToList<CMeasure>().First<CMeasure>(); }
                    //else
                    //{
                    //    objFailProductlist.Add(objProduct);
                    //    memoEditLog.Text += "\r\n" + objProduct.Name + " Ошибка: Не удалось найти в справочнике единицу измерения с указанным наименованием: " + objProduct.Measure.ShortName;
                    //    continue;
                    //}

                    //if( objProduct.Reference.Trim() == "")
                    //{
                    //    //objFailProductlist.Add(objProduct);
                    //    memoEditLog.Text += "\r\n" + objProduct.Name + " Внимание! Референс товара является обязательным параметром.";
                    //    //continue;
                    //}

                    if (objProduct.Import(m_objProfile, ref strErr, System.Guid.Empty) == false)
                    {
                        // позиция обработалась с ошибкой
                        objFailProductlist.Add(objProduct);
                        memoEditLog.Text += "\r\n" + objProduct.Name + ": " + strErr;
                    }

                }
                #endregion

                objCountryList = null;
                objCurrencyList = null;
                objMeasureList = null;
                objProductOwnerList = null;
                objProductTypeList = null;
                objProductLineList = null;
                objProductSubtypeList = null;

                if (objFailProductlist.Count > 0)
                {
                    m_objProductList.Clear();
                    foreach (CProduct objProduct in objFailProductlist)
                    {
                        m_objProductList.Add(objProduct);
                    }

                    objFailProductlist = null;
                }
                else
                {
                    objFailProductlist = null;
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }

            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Во время импорта списка товаров возникла ошибка.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                this.tableLayoutPanel2.ResumeLayout(false);
                gridControlProductList.DataSource = m_objProductList;
                gridControlProductList.RefreshDataSource();

                this.Cursor = Cursors.Default;
            }

            return;
        }
                

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                switch (radioGroupImportMode.SelectedIndex)
                {
                    case 0:
                    case 1:
                        SavePartslistTDB();
                        break;
                    case 2:
                        SavePartslistToDB_1();
                        break;
                    default:
                        break;
                }
                
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "btnOk_Click.\nТекст ошибки: " + f.Message, "Ошибка",
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
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                Close();
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "btnCancel_Click.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        #endregion


    }
}
