using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ERP_Mercury.Common;
using System.Reflection;
using OfficeOpenXml;
using Excel = Microsoft.Office.Interop.Excel;

namespace ERP_Mercury.Common
{
    public enum enumImportCustomerOrderMode
    {
        Unkown = -1,
        ImportDataInOrderByIDSettings = 0,
        ImportDataInOrderSettings = 1
    }

    public enum enumImportOrderType
    {
        Unkown = -1,
        Order = 0,
        intOrder = 1
    }

    public partial class frmImportXLSData : DevExpress.XtraEditors.XtraForm
    {
        #region Свойства
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;
        /// <summary>
        /// Список настроек
        /// </summary>
        private CSettingForImportData m_objSettingForImportData;
        private System.String m_strFileFullName;
        /// <summary>
        /// Имя файла
        /// </summary>
        public System.String FileFullName
        {
            get { return m_strFileFullName; }
        }
        public System.Int32 SelectedSheetId { get; set; }
        public List<System.String> SheetList;
        public System.String DeparCode { get; set; }
        private List<System.String> m_objDepartCodeList;
        private System.Int32 m_iPayFormId;
        private System.Int32 m_iCustomerId;
        public CCustomer SelectedCustomer { get; set; }
        public CChildDepart SelectedChildDepart { get; set; }
        public CRtt SelectedRtt {get; set;}
        private CStock m_objSelectedStock;
        private CPaymentType m_objPaymentType;
        private System.Data.DataTable m_objdtOrderItems;
        private System.Double m_dblDiscountPercent;
        private System.Boolean m_bcheckMultiplicity;
        private CPriceType m_objPriceType;

        private List<CCustomer> m_objCustomerList;

        private const System.String strNodeSettingname = "ColumnItem";
        private const System.String strDepartCodePrefix = "Подразделение: ";
        private const System.String strCustomerPrefix = "Клиент: ";
        private const System.String strRttPrefix = "РТТ: ";
        private enumImportCustomerOrderMode m_ImportCustomerOrderMode;
        private enumImportOrderType m_ImportOrderType;

        private const System.String strImportDataInOrderSettings = "ImportDataInOrderSettings";
        private const System.String strImportDataInOrderByIDSettings = "ImportDataInOrderByIDSettings";
        private const System.String strImportDataInIntOrderByIDSettings = "ImportDataInIntOrderByIDSettings";
        #endregion

        #region Конструктор
        public frmImportXLSData(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem, 
            List<System.String> objDepartCodeList, List<CCustomer> objCustomerList ) 
        {
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ru-RU");
            ci.NumberFormat.CurrencyDecimalSeparator = ".";
            ci.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(MyResolveEventHandler);

            InitializeComponent();

            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_strFileFullName = "";
            m_objSettingForImportData = null;
            btnLoadDataFromFile.Enabled = false;
            DeparCode = "";
            m_objDepartCodeList = objDepartCodeList;
            m_iPayFormId = 0;
            m_objCustomerList = objCustomerList;
            SelectedCustomer = null;
            SelectedChildDepart = null;
            SelectedRtt = null;
            m_objSelectedStock = null;
            m_iCustomerId = 0;
            m_objdtOrderItems = null;
            m_objPaymentType = null;
            m_dblDiscountPercent = 0;
            SelectedSheetId = 0;
            m_bcheckMultiplicity = false;
            SheetList = null;
            m_ImportCustomerOrderMode = enumImportCustomerOrderMode.Unkown;
            m_objPriceType = null;
        }

        private System.Reflection.Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            System.Reflection.Assembly MyAssembly = null;
            System.Reflection.Assembly objExecutingAssemblies = System.Reflection.Assembly.GetExecutingAssembly();

            System.Reflection.AssemblyName[] arrReferencedAssmbNames = objExecutingAssemblies.GetReferencedAssemblies();

            //Loop through the array of referenced assembly names.
            System.String strDllName = "";
            foreach (System.Reflection.AssemblyName strAssmbName in arrReferencedAssmbNames)
            {

                //Check for the assembly names that have raised the "AssemblyResolve" event.
                if (strAssmbName.FullName.Substring(0, strAssmbName.FullName.IndexOf(",")) == args.Name.Substring(0, args.Name.IndexOf(",")))
                {
                    strDllName = args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll";
                    break;
                }

            }
            if (strDllName != "")
            {
                System.String strFileFullName = "";
                System.Boolean bError = false;
                foreach (System.String strPath in this.m_objProfile.ResourcePathList)
                {
                    //Load the assembly from the specified path. 
                    strFileFullName = strPath + "\\" + strDllName;
                    if (System.IO.File.Exists(strFileFullName))
                    {
                        try
                        {
                            MyAssembly = System.Reflection.Assembly.LoadFrom(strFileFullName);
                            break;
                        }
                        catch (System.Exception f)
                        {
                            bError = true;
                            DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка загрузки библиотеки: " +
                                strFileFullName + "\n\nТекст ошибки: " + f.Message, "Ошибка",
                                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        }
                    }
                    if (bError) { break; }
                }
            }

            //Return the loaded assembly.
            if (MyAssembly == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Не удалось найти библиотеку: " +
                                strDllName, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

            }
            return MyAssembly;
        }

        #endregion

        #region Открыть форму с настройками для импорта приложения в заказ
        /// <summary>
        /// Открывает форму в режиме импорта данных в приложение к заказу
        /// </summary>
        /// <param name="objOrderItemList">приложение к заказу</param>
        /// <param name="objCustomer">клиент</param>
        /// <param name="strDeparCode">подразделение</param>
        /// <param name="iPayFormId">форма оплаты</param>
        /// <param name="objProductList">остаток товара</param>
        public void OpenForImportPartsInSuppl( CCustomer objCustomer, CRtt objRtt,
            System.String strDeparCode, System.Int32 iPayFormId, CStock objStock, CPaymentType objPaymentType,
            System.Double dblDiscountPercent, System.Data.DataTable objdtOrderItems, System.String strFileName,
            System.Int32 iSelectedSheetId, List<System.String> SheetList, System.Boolean bcheckMultiplicity
            )
        {
            try
            {
                SelectedCustomer = objCustomer;
                SelectedRtt = objRtt;
                m_objSelectedStock = objStock;
                m_iCustomerId = 0;
                m_iPayFormId = iPayFormId;
                m_ImportOrderType = enumImportOrderType.Order;
                DeparCode = strDeparCode;
                m_objPaymentType = objPaymentType;
                m_dblDiscountPercent = dblDiscountPercent;
                m_objdtOrderItems = objdtOrderItems;
                m_bcheckMultiplicity = bcheckMultiplicity;
                m_ImportOrderType = enumImportOrderType.Unkown;

                lblOrderInfo.Text = strDeparCode + " " + ((objCustomer == null) ? "" : objCustomer.FullName) + " " +
                    ((SelectedRtt == null) ? "" : SelectedRtt.VisitingCard);

                if (SelectedCustomer != null)
                {
                    m_iCustomerId = SelectedCustomer.InterBaseID;
                    Text = "клиент: " + SelectedCustomer.FullName;
                }

                SetInitialParams();

                txtID_Ib.Text = strFileName;
                cboxSheet.Properties.Items.Clear();
                if (SheetList != null)
                {
                    cboxSheet.Properties.Items.AddRange(SheetList);
                    cboxSheet.SelectedIndex = iSelectedSheetId;
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "OpenForImportPartsInSuppl.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                ShowDialog();
            }

            return;

        }

        /// <summary>
        /// Открывает форму в режиме "импорт приложения в заказ на внутреннее перемещение"
        /// </summary>
        /// <param name="objSrcStock">Склад-источник</param>
        /// <param name="dblDiscountPercent">Скидка, %</param>
        /// <param name="objdtOrderItems">приложение к заказу</param>
        /// <param name="strFileName">путь к файлу MS Excel с данными для импорта</param>
        /// <param name="iSelectedSheetId">номер выбранного листа в файле MS Excel</param>
        /// <param name="SheetList">список листов в файле MS Excel</param>
        /// <param name="bcheckMultiplicity">признак "учитывать кратность в упаковке"</param>
        /// <param name="objPriceType">уровень цены</param>
        /// <param name="strDocNum">№ заказа</param>
        public void OpenForImportPartsInInOrder(CStock objSrcStock, 
            System.Double dblDiscountPercent, System.Data.DataTable objdtOrderItems, System.String strFileName,
            System.Int32 iSelectedSheetId, List<System.String> SheetList, System.Boolean bcheckMultiplicity, 
            CPriceType objPriceType, System.String strDocNum
            )
        {
            try
            {
                m_objSelectedStock = objSrcStock;
                m_iCustomerId = 0;
                m_ImportOrderType = enumImportOrderType.intOrder;
                m_dblDiscountPercent = dblDiscountPercent;
                m_objdtOrderItems = objdtOrderItems;
                m_bcheckMultiplicity = bcheckMultiplicity;
                m_objPriceType = objPriceType;

                lblOrderInfo.Text = (String.Format("Заказ №{0} перемещение со склада {1}-{2}", strDocNum, objSrcStock.CompanyAbbr, objSrcStock.WareHouseName ));

                SetInitialParams();

                txtID_Ib.Text = strFileName;
                cboxSheet.Properties.Items.Clear();
                if (SheetList != null)
                {
                    cboxSheet.Properties.Items.AddRange(SheetList);
                    cboxSheet.SelectedIndex = iSelectedSheetId;
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "OpenForImportPartsInInOrder.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                ShowDialog();
            }

            return;

        }
        
        #endregion

        #region Первоначальные установки
        private void SetInitialParams()
        {
            try
            {
                txtID_Ib.Text = "";
                cboxSheet.Properties.Items.Clear();
                treeListSettings.Nodes.Clear();
                
                cboxSettings.Properties.Items.Clear();

                System.String strErr = System.String.Empty;
                List<System.String> SettingNamesListForInitParams = new List<string>();

                switch (m_ImportOrderType)
                {
                    case enumImportOrderType.Order:
                        SettingNamesListForInitParams.Add(strImportDataInOrderByIDSettings);
                        SettingNamesListForInitParams.Add(strImportDataInOrderSettings);
                        break;
                    case enumImportOrderType.intOrder:
                        SettingNamesListForInitParams.Add(strImportDataInIntOrderByIDSettings);
                        break;
                    default:
                        break;
                }

                List<CSettingForImportData> objSettingsList = CSettingForImportData.GetSettingslist(m_objProfile, null, SettingNamesListForInitParams, ref strErr);
                if ((objSettingsList != null) && (objSettingsList.Count > 0))
                {
                    foreach (CSettingForImportData objItem in objSettingsList)
                    {
                        if (SettingNamesListForInitParams.SingleOrDefault<System.String>(x => x == objItem.Name) != null)
                        {
                            cboxSettings.Properties.Items.Add(objItem);
                        }
                    }

                    switch (m_ImportOrderType)
                    {
                        case enumImportOrderType.Order:
                            cboxSettings.SelectedItem = cboxSettings.Properties.Items.Cast<CSettingForImportData>().SingleOrDefault<CSettingForImportData>(x => x.Name == strImportDataInOrderByIDSettings);
                            break;
                        case enumImportOrderType.intOrder:
                            cboxSettings.SelectedItem = ((cboxSettings.Properties.Items.Count > 0) ? cboxSettings.Properties.Items[0] : null);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show( strErr, "Внимание!",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }

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
        /// <summary>
        /// Загружает информацию о настройках для импорта данных в дерево значений
        /// </summary>
        /// <param name="objSettingForImportData">настройка для импорта данных</param>
        private void LoadListSettings(CSettingForImportData objSettingForImportData)
        {
            treeListSettings.Nodes.Clear();
            m_objSettingForImportData = objSettingForImportData;
            m_ImportCustomerOrderMode = enumImportCustomerOrderMode.Unkown;

            if (m_objSettingForImportData == null) { return; }

            try
            {
                if (m_objSettingForImportData.Name == strImportDataInOrderByIDSettings)
                {
                    m_ImportCustomerOrderMode = enumImportCustomerOrderMode.ImportDataInOrderByIDSettings;
                    checkEditImportPrices.Checked = false;
                }
                else if (m_objSettingForImportData.Name == strImportDataInOrderSettings)
                {
                    m_ImportCustomerOrderMode = enumImportCustomerOrderMode.ImportDataInOrderSettings;
                    checkEditImportPrices.Checked = true;
                }
                else if (m_objSettingForImportData.Name == strImportDataInIntOrderByIDSettings)
                {
                    m_ImportCustomerOrderMode = enumImportCustomerOrderMode.ImportDataInOrderByIDSettings;
                    checkEditImportPrices.Checked = true;
                }

                foreach (CSettingItemForImportData objSetting in m_objSettingForImportData.SettingsList)
                {
                    treeListSettings.AppendNode(new object[] { objSetting.TOOLS_USERNAME, System.String.Format("{0:### ### ##0}", objSetting.TOOLS_VALUE), objSetting.TOOLS_DESCRIPTION }, null).Tag = objSetting;
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "LoadListSettings.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        private void cboxSettings_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                LoadListSettings(((cboxSettings.SelectedItem == null) ? null : (CSettingForImportData)cboxSettings.SelectedItem));
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "cboxSettings_SelectedValueChanged.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
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
                        System.String strFileName = openFileDialog.FileName;
                        txtID_Ib.Text = strFileName;
                        ReadSheetListFromXLSFile(ref strFileName);

                        if (strFileName != txtID_Ib.Text)
                        {
                            txtID_Ib.Text = strFileName;
                        }
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
        private void ReadSheetListFromXLSFile( ref System.String strFileName )
        {
            if (strFileName == "") { return; }
            if (System.IO.File.Exists(strFileName) == false)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                     String.Format("файл \"{0}\" не найден.", strFileName), "Ошибка",
                     System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }

            Excel.Application oXL = null;
            Excel._Workbook oWB;

            object m = Type.Missing;

            try
            {
                cboxSheet.Properties.Items.Clear();
                this.Cursor = Cursors.WaitCursor;
                oXL = new Excel.Application();
                oWB = (Excel._Workbook)(oXL.Workbooks.Open(strFileName, 0, true, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value));

                foreach (Excel._Worksheet objSheet in oWB.Worksheets)
                {
                    cboxSheet.Properties.Items.Add(objSheet.Name);
                }

                if (System.IO.Path.GetExtension(strFileName).ToUpper() == ".DBF")
                {
                    System.String strNewFileName = (System.IO.Path.GetTempPath() + System.Guid.NewGuid().ToString() + ".XLSX");

                    System.IO.FileInfo newFile = new System.IO.FileInfo(strNewFileName);
                    if (newFile.Exists)
                    {
                        newFile.Delete();  // ensures we create a new workbook
                    }
                    // Excel.XlFileFormat.xlExcel12
                    oWB.SaveAs(strNewFileName, Missing.Value, Missing.Value, Missing.Value, Missing.Value, 
                        Missing.Value, Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                    strFileName = strNewFileName;
                }

                oWB.Close(Missing.Value, Missing.Value, Missing.Value);
                oXL.Quit();

            }
            catch (System.Exception f)
            {
                oXL = null;
                oWB = null;
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка экспорта в MS Excel.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                oWB = null;
                oXL = null;
                cboxSheet.SelectedItem = ((cboxSheet.Properties.Items.Count > 0) ? cboxSheet.Properties.Items[0] : null);
                btnLoadDataFromFile.Enabled = (cboxSheet.SelectedItem != null);

                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            return;
        }
        /// <summary>
        /// Возвращает значение параметра настройки по его наименованию
        /// </summary>
        /// <param name="strSettingName">имя параметра</param>
        /// <returns>значение параметра настройки</returns>
        private System.Int32 GetSettingValueByName(System.String strSettingName)
        {
            System.Int32 iRet = 0;
            try
            {
                CSettingItemForImportData objSetting = null;
                foreach( DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListSettings.Nodes )
                {
                    if (objNode.Tag == null) { continue; }
                    objSetting = (CSettingItemForImportData)objNode.Tag;
                    foreach (CSettingItemForImportData objItem in m_objSettingForImportData.SettingsList)
                    {
                        if (objSetting.TOOLS_NAME == strSettingName)
                        {
                            iRet = System.Convert.ToInt32(objNode.GetValue(colSettingsColumnNum));
                            break;
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "GetSettingValueByName.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return iRet;
        }

        /// <summary>
        /// Считывает информацию из фала MS Excel
        /// </summary>
        /// <param name="strFileName">имя файла MS Excel</param>
        private void ReadDataFromXLSFile(System.String strFileName)
        {
            this.Cursor = Cursors.WaitCursor;
            System.String strCaption = this.Text;

            System.IO.FileInfo newFile = new System.IO.FileInfo(strFileName);
            if (newFile.Exists == false)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка экспорта в MS Excel.\n\nНе найден файл: " + strFileName, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            //treeListImportOrder.CellValueChanged -= new DevExpress.XtraTreeList.CellValueChangedEventHandler(treeListImportOrder_CellValueChanged);
            treeListImportOrder.Nodes.Clear();
            listEditLog.Items.Clear();

            try
            {
                this.Cursor = Cursors.WaitCursor;
                treeListImportOrder.Nodes.Clear();

                System.Int32 iStartRow = GetSettingValueByName( CSettingForImportData.strFieldNameSTARTROW );
                System.Int32 iColumnARTICLE = GetSettingValueByName( CSettingForImportData.strFieldNameARTICLE );
                System.Int32 iColumnNAME2 = GetSettingValueByName( CSettingForImportData.strFieldNameNAME2 );
                System.Int32 iColumnPRICE = GetSettingValueByName( CSettingForImportData.strFieldNamePRICE );
                System.Int32 iColumnQUANTITY = GetSettingValueByName( CSettingForImportData.strFieldNameQUANTITY );
                System.Int32 iColumnMARKUP = GetSettingValueByName( CSettingForImportData.strFieldNameMARKUP );
                System.Int32 iColumnPARTS_ID = GetSettingValueByName(CSettingForImportData.strFieldNamePARTS_ID);

                System.Int32 iRowCustomerId = GetSettingValueByName( CSettingForImportData.strFieldNameCUSTOMER_ID );
                System.Int32 iRowRttCode=  GetSettingValueByName( CSettingForImportData.strFieldNameRTT_CODE );
                System.Int32 iRowDepartCode=  GetSettingValueByName( CSettingForImportData.strFieldNameDEPART_CODE );
                System.String strARTICLE = ""; 
                System.String strNAME2 = ""; 
                System.String strQUANTITY = ""; 
                System.String strPRICE = ""; 
                System.String strMARKUP = ""; 
                System.String strCustomerId = "";
                System.String strRttCode = "";
                System.String strDepartCode = "";
                System.String strPARTS_ID = "";
                System.Int32 iCustomerId = 0;

                System.Int32 iCurrentRow = iStartRow;

                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[cboxSheet.Text];
                    if (worksheet != null)
                    {

                        System.Boolean bStopRead = false;
                        System.Boolean bErrExists = false;
                        System.String strFrstColumn = "";
                        System.Int32 i = 1;
                        System.Decimal iQuantity = 0;
                        System.Double dblPRICE = 0;
                        System.Double dblMARKUP = 0;
                        System.Int32 iPartsId = 0;
                        System.Decimal dclmultiplicity = 0;
                        CProduct objProduct = null;

                        #region Поиск подразделения, клиента и РТТ
                        //// код подразделения
                        //if ((iRowDepartCode != 0) && (DeparCode == ""))
                        //{
                        //    strDepartCode = System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iRowDepartCode, 1], objSheet.Cells[iRowDepartCode, 1]).Value2);
                        //    if (strDepartCode != "")
                        //    {
                        //        DeparCode = m_objDepartCodeList.Single<System.String>(x => x == strDepartCode);
                        //        lblDepartCode.Text = (strDepartCodePrefix + DeparCode);
                        //    }
                        //}


                        //// код клиента и РТТ
                        //if (iRowCustomerId != 0)
                        //{
                        //    strCustomerId = System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iRowCustomerId, 1], objSheet.Cells[iRowCustomerId, 1]).Value2);
                        //    iCustomerId = System.Convert.ToInt32(strCustomerId);

                        //    if (m_iPayFormId == 1)
                        //    {
                        //        if ((SelectedCustomer != null) && (SelectedCustomer.InterBaseID != iCustomerId))
                        //        {
                        //            listEditLog.Items.Add("Код клиента, указанный в файле не соответсвует клиенту, выбранному в журнале договоров!");
                        //            m_iCustomerId = 0;
                        //        }
                        //        else if (m_iCustomerId == 0)
                        //        {
                        //            try
                        //            {
                        //                SelectedCustomer = m_objCustomerList.Single<CCustomer>(x => x.InterBaseID == iCustomerId);
                        //            }
                        //            catch
                        //            {
                        //                m_iCustomerId = 0;
                        //                SelectedCustomer = null;
                        //            }
                        //        }
                        //    }
                        //    else if (m_iPayFormId == 2)
                        //    {
                        //        if (m_iCustomerId == 0)
                        //        {
                        //            try
                        //            {
                        //                SelectedCustomer = m_objCustomerList.Single<CCustomer>(x => x.InterBaseID == iCustomerId);
                        //                m_iCustomerId = SelectedCustomer.InterBaseID;
                        //            }
                        //            catch
                        //            {
                        //                m_iCustomerId = 0;
                        //                SelectedCustomer = null;
                        //            }
                        //        }
                        //    }

                        //    List<CChildDepart> objChildDepartList = CChildDepart.GetChildDepartList(m_objProfile, null, SelectedCustomer.ID);
                        //    List<CRtt> objRttList = CRtt.GetRttList(m_objProfile, null, SelectedCustomer.ID);

                        //    if (SelectedCustomer != null)
                        //    {
                        //        lblCustomer.Text = strCustomerPrefix + SelectedCustomer.FullName;
                        //    }

                        //    if ((m_iPayFormId == 2) && (objChildDepartList != null) && (objChildDepartList.Count > 0))
                        //    {
                        //        SelectedChildDepart = objChildDepartList[0];
                        //        lblCustomer.Text = strCustomerPrefix + objChildDepartList[0].Name;
                        //    }

                        //    if ((objRttList == null) || (objRttList.Count == 0))
                        //    {
                        //        listEditLog.Items.Add("Внимание! Для клиента не найдены зарегистрированные РТТ.");
                        //    }
                        //    else if (iRowRttCode != 0)
                        //    {
                        //        strRttCode = System.Convert.ToString(objSheet.get_Range(objSheet.Cells[iRowRttCode, 1], objSheet.Cells[iRowRttCode, 1]).Value2);
                        //        try
                        //        {
                        //            SelectedRtt = objRttList.Single<CRtt>(x => x.Code == strRttCode);
                        //        }
                        //        catch
                        //        {
                        //            SelectedRtt = null;
                        //        }
                        //        if (SelectedRtt == null)
                        //        {
                        //            listEditLog.Items.Add("Внимание! Код РТТ, указанный в файле не соответсвует розничной точке выбанного клиента!");
                        //        }
                        //        else
                        //        {
                        //            lblRtt.Text = (strRttPrefix + SelectedRtt.VisitingCard);
                        //        }
                        //    }

                        //}

                        #endregion

                        while (bStopRead == false)
                        {
                            bErrExists = false;
                            strARTICLE = "";
                            strNAME2 = "";
                            strQUANTITY = "";
                            strPRICE = "";
                            strMARKUP = "";
                            strCustomerId = "";
                            strRttCode = "";
                            strDepartCode = "";
                            strPARTS_ID = "";

                            // пробежим по строкам и считаем информацию
                            strFrstColumn = System.Convert.ToString(worksheet.Cells[iCurrentRow, 1].Value);
                            if (strFrstColumn == "")
                            {
                                bStopRead = true;
                            }
                            else
                            {
                                if (m_ImportCustomerOrderMode == enumImportCustomerOrderMode.ImportDataInOrderSettings)
                                {
                                    strARTICLE = System.Convert.ToString(worksheet.Cells[iCurrentRow, iColumnARTICLE].Value);
                                    strNAME2 = System.Convert.ToString(worksheet.Cells[iCurrentRow, iColumnNAME2].Value);
                                    strMARKUP = System.Convert.ToString(worksheet.Cells[iCurrentRow, iColumnMARKUP].Value);
                                    strDepartCode = System.Convert.ToString(worksheet.Cells[iCurrentRow, iRowDepartCode].Value);
                                    strCustomerId = System.Convert.ToString(worksheet.Cells[iCurrentRow, iRowCustomerId].Value);
                                    strRttCode = System.Convert.ToString(worksheet.Cells[iCurrentRow, iRowRttCode].Value);
                                }
                                strPARTS_ID = System.Convert.ToString(worksheet.Cells[iCurrentRow, iColumnPARTS_ID].Value);
                                strQUANTITY = System.Convert.ToString(worksheet.Cells[iCurrentRow, iColumnQUANTITY].Value);
                                strPRICE = System.Convert.ToString(worksheet.Cells[iCurrentRow, iColumnPRICE].Value);


                                iQuantity = 0;
                                iPartsId = 0;
                                dblPRICE = 0;
                                dblMARKUP = 0;
                                objProduct = null;

                                // преобразуем строки в числа
                                if (strPARTS_ID == "")
                                {
                                    iPartsId = 0;
                                }
                                else
                                {
                                    // код товара в InterBase
                                    try
                                    {
                                        iPartsId = System.Convert.ToInt32(strPARTS_ID);
                                    }
                                    catch
                                    {
                                        bErrExists = true;
                                        listEditLog.Items.Add(String.Format("{0} ошибка преобразования кода товара в числовой формат.", i));
                                    }
                                }

                                if (strQUANTITY == "")
                                {
                                    iQuantity = 0;
                                    listEditLog.Items.Add(String.Format("{0} не указано количество.", i));
                                }
                                else
                                {
                                    // количество
                                    try
                                    {
                                        iQuantity = System.Convert.ToDecimal(strQUANTITY);
                                    }
                                    catch
                                    {
                                        bErrExists = true;
                                        iQuantity = 0;
                                        listEditLog.Items.Add(String.Format("{0} ошибка преобразования количества товара в числовой формат.", i));
                                    }
                                }

                                // цена
                                try
                                {
                                    dblPRICE = System.Convert.ToDouble(strPRICE);
                                    if (dblPRICE < 0) { dblPRICE = 0; }
                                }
                                catch
                                {
                                    bErrExists = true;
                                    dblPRICE = 0;
                                    listEditLog.Items.Add(String.Format("{0} ошибка преобразования цены  в числовой формат.", i));
                                }
                                // скидка
                                try
                                {
                                    if (strMARKUP == "") { dblMARKUP = 0; }
                                    else { dblMARKUP = System.Convert.ToDouble(strMARKUP); }
                                }
                                catch
                                {
                                    bErrExists = true;
                                    listEditLog.Items.Add(String.Format("{0} ошибка преобразования скидки в числовой формат.", i));
                                }

                                if ((bErrExists == false) && (bStopRead == false) && (iQuantity > 0))
                                {
                                    objProduct = COrderRepository.GetPartsInstock(m_objProfile, null, strNAME2, strARTICLE, m_objSelectedStock.ID, iPartsId);

                                    if (objProduct == null)
                                    {
                                        bErrExists = true;
                                        listEditLog.Items.Add(String.Format("{0} товар не найдет. артикул товара: {1} наименование: {2} код: {3}", i, strARTICLE, strNAME2, iPartsId));

                                        treeListImportOrder.AppendNode(new object[] { null, iPartsId, strARTICLE, 
                                                strNAME2, 0, 0, iQuantity, dblPRICE, ( System.Convert.ToDouble( iQuantity ) * dblPRICE ), "НЕ импортирован, товар не найдет"  }, null).Tag = null;

                                        treeListImportOrder.Refresh();

                                        listEditLog.Items.Add(String.Format("{0} OK ", i));
                                        listEditLog.Refresh();

                                    }
                                    else
                                    {
                                        dclmultiplicity = System.Convert.ToDecimal(objProduct.CustomerOrderMinRetailQty);

                                        if( (m_bcheckMultiplicity == true) && ( dclmultiplicity > 0 ) )
                                        {
                                            if ((iQuantity % dclmultiplicity) != 0)
                                            {
                                                iQuantity = (((int)iQuantity / (int)dclmultiplicity) * dclmultiplicity) + dclmultiplicity;
                                                if (iQuantity > System.Convert.ToDecimal(objProduct.CustomerOrderStockQty))
                                                {
                                                    iQuantity = System.Convert.ToDecimal(objProduct.CustomerOrderStockQty);
                                                }
                                            }
                                        }

                                        treeListImportOrder.AppendNode(new object[] { objProduct.ID, objProduct.ID_Ib, objProduct.Article, 
                                                objProduct.Name, objProduct.CustomerOrderResQty, objProduct.CustomerOrderStockQty, 
                                                iQuantity,  dblPRICE, ( System.Convert.ToDouble( iQuantity ) * dblPRICE ) }, null).Tag = objProduct;

                                        treeListImportOrder.Refresh();

                                        listEditLog.Items.Add(String.Format("{0} OK ", i));
                                        listEditLog.Refresh();
                                    }
                                }
                                else if ((bErrExists == true) && (bStopRead == false))
                                {
                                    // ошибка, необходимо пометить запись
                                    treeListImportOrder.AppendNode(new object[] { null, iPartsId, strARTICLE, 
                                                strNAME2, 0, 0, iQuantity, dblPRICE, "НЕ импортирован"  }, null).Tag = null;

                                    treeListImportOrder.Refresh();

                                    listEditLog.Items.Add(String.Format("{0} OK ", i));
                                    listEditLog.Refresh();
                                }

                            }

                            iCurrentRow++;
                            i++;
                            strFrstColumn = System.Convert.ToString(worksheet.Cells[iCurrentRow, 1].Value);
                            listEditLog.Refresh();

                            this.Text = String.Format("обрабатывается запись №{0}", i);
                            this.Refresh();


                        } //while (bStopRead == false)
                    }
                    worksheet = null;
                }


            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка импорта данных из MS Excel.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                treeListImportOrder.BestFitColumns();
                this.Text = strCaption;
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            return;
        }

        /// <summary>
        /// Считывает информацию из фала MS Excel
        /// </summary>
        /// <param name="strFileName">имя файла MS Excel</param>
        private void ReadDataFromXLSFileForIntOrderByID(System.String strFileName)
        {
            this.Cursor = Cursors.WaitCursor;
            System.String strCaption = this.Text;

            System.IO.FileInfo newFile = new System.IO.FileInfo(strFileName);
            if (newFile.Exists == false)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка экспорта в MS Excel.\n\nНе найден файл: " + strFileName, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            //treeListImportOrder.CellValueChanged -= new DevExpress.XtraTreeList.CellValueChangedEventHandler(treeListImportOrder_CellValueChanged);
            treeListImportOrder.Nodes.Clear();
            listEditLog.Items.Clear();

            try
            {
                this.Cursor = Cursors.WaitCursor;
                treeListImportOrder.Nodes.Clear();

                System.Int32 iStartRow = GetSettingValueByName(CSettingForImportData.strFieldNameSTARTROW);
                System.Int32 iColumnARTICLE = GetSettingValueByName(CSettingForImportData.strFieldNameARTICLE);
                System.Int32 iColumnNAME2 = GetSettingValueByName(CSettingForImportData.strFieldNameNAME2);
                System.Int32 iColumnPRICE = GetSettingValueByName(CSettingForImportData.strFieldNamePRICE);
                System.Int32 iColumnQUANTITY = GetSettingValueByName(CSettingForImportData.strFieldNameQUANTITY);
                System.Int32 iColumnMARKUP = GetSettingValueByName(CSettingForImportData.strFieldNameMARKUP);
                System.Int32 iColumnPARTS_ID = GetSettingValueByName(CSettingForImportData.strFieldNamePARTS_ID);

                System.Int32 iRowCustomerId = GetSettingValueByName(CSettingForImportData.strFieldNameCUSTOMER_ID);
                System.Int32 iRowRttCode = GetSettingValueByName(CSettingForImportData.strFieldNameRTT_CODE);
                System.Int32 iRowDepartCode = GetSettingValueByName(CSettingForImportData.strFieldNameDEPART_CODE);
                System.String strARTICLE = "";
                System.String strNAME2 = "";
                System.String strQUANTITY = "";
                System.String strPRICE = "";
                System.String strMARKUP = "";
                System.String strPARTS_ID = "";

                System.Int32 iCurrentRow = iStartRow;

                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[cboxSheet.Text];
                    if (worksheet != null)
                    {

                        System.Boolean bStopRead = false;
                        System.Boolean bErrExists = false;
                        System.String strFrstColumn = "";
                        System.Int32 i = 1;
                        System.Decimal iQuantity = 0;
                        System.Double dblPRICE = 0;
                        System.Double dblMARKUP = 0;
                        System.Int32 iPartsId = 0;
                        System.Decimal dclmultiplicity = 0;
                        CProduct objProduct = null;

                        while (bStopRead == false)
                        {
                            bErrExists = false;
                            strARTICLE = "";
                            strNAME2 = "";
                            strQUANTITY = "";
                            strPRICE = "";
                            strMARKUP = "";
                            strPARTS_ID = "";

                            // пробежим по строкам и считаем информацию
                            strFrstColumn = System.Convert.ToString(worksheet.Cells[iCurrentRow, 1].Value);
                            if (strFrstColumn == "")
                            {
                                bStopRead = true;
                            }
                            else
                            {
                                if (m_ImportCustomerOrderMode == enumImportCustomerOrderMode.ImportDataInOrderSettings)
                                {
                                    strARTICLE = System.Convert.ToString(worksheet.Cells[iCurrentRow, iColumnARTICLE].Value);
                                    strNAME2 = System.Convert.ToString(worksheet.Cells[iCurrentRow, iColumnNAME2].Value);
                                    strMARKUP = System.Convert.ToString(worksheet.Cells[iCurrentRow, iColumnMARKUP].Value);
                                }
                                else if (m_ImportCustomerOrderMode == enumImportCustomerOrderMode.ImportDataInOrderByIDSettings)
                                {
                                    strPARTS_ID = System.Convert.ToString(worksheet.Cells[iCurrentRow, iColumnPARTS_ID].Value);
                                    strQUANTITY = System.Convert.ToString(worksheet.Cells[iCurrentRow, iColumnQUANTITY].Value);
                                    strPRICE = System.Convert.ToString(worksheet.Cells[iCurrentRow, iColumnPRICE].Value);
                                }

                                iQuantity = 0;
                                iPartsId = 0;
                                dblPRICE = 0;
                                dblMARKUP = 0;
                                objProduct = null;

                                // преобразуем строки в числа
                                if (strPARTS_ID == "")
                                {
                                    iPartsId = 0;
                                }
                                else
                                {
                                    // код товара в InterBase
                                    try
                                    {
                                        iPartsId = System.Convert.ToInt32(strPARTS_ID);
                                    }
                                    catch
                                    {
                                        bErrExists = true;
                                        listEditLog.Items.Add(String.Format("{0} ошибка преобразования кода товара в числовой формат.", i));
                                    }
                                }

                                if (strQUANTITY == "")
                                {
                                    iQuantity = 0;
                                    listEditLog.Items.Add(String.Format("{0} не указано количество.", i));
                                }
                                else
                                {
                                    // количество
                                    try
                                    {
                                        iQuantity = System.Convert.ToDecimal(strQUANTITY);
                                    }
                                    catch
                                    {
                                        bErrExists = true;
                                        iQuantity = 0;
                                        listEditLog.Items.Add(String.Format("{0} ошибка преобразования количества товара в числовой формат.", i));
                                    }
                                }

                                // цена
                                try
                                {
                                    dblPRICE = System.Convert.ToDouble(strPRICE);
                                    if (dblPRICE < 0) { dblPRICE = 0; }
                                }
                                catch
                                {
                                    bErrExists = true;
                                    dblPRICE = 0;
                                    listEditLog.Items.Add(String.Format("{0} ошибка преобразования цены  в числовой формат.", i));
                                }
                                // скидка
                                try
                                {
                                    if (strMARKUP == "") { dblMARKUP = 0; }
                                    else { dblMARKUP = System.Convert.ToDouble(strMARKUP); }
                                }
                                catch
                                {
                                    bErrExists = true;
                                    listEditLog.Items.Add(String.Format("{0} ошибка преобразования скидки в числовой формат.", i));
                                }

                                if ((bErrExists == false) && (bStopRead == false) && (iQuantity > 0))
                                {
                                    objProduct = COrderRepository.GetPartsInstock(m_objProfile, null, strNAME2, strARTICLE, m_objSelectedStock.ID, iPartsId);

                                    if (objProduct == null)
                                    {
                                        bErrExists = true;
                                        listEditLog.Items.Add(String.Format("{0} товар не найдет. артикул товара: {1} наименование: {2} код: {3}", i, strARTICLE, strNAME2, iPartsId));

                                        treeListImportOrder.AppendNode(new object[] { null, iPartsId, strARTICLE, 
                                                strNAME2, 0, 0, iQuantity, dblPRICE, ( System.Convert.ToDouble( iQuantity ) * dblPRICE ), "НЕ импортирован, товар не найдет"  }, null).Tag = null;

                                        treeListImportOrder.Refresh();

                                        listEditLog.Items.Add(String.Format("{0} OK ", i));
                                        listEditLog.Refresh();

                                    }
                                    else
                                    {
                                        dclmultiplicity = System.Convert.ToDecimal(objProduct.CustomerOrderMinRetailQty);

                                        if ((m_bcheckMultiplicity == true) && (dclmultiplicity > 0))
                                        {
                                            if ((iQuantity % dclmultiplicity) != 0)
                                            {
                                                iQuantity = (((int)iQuantity / (int)dclmultiplicity) * dclmultiplicity) + dclmultiplicity;
                                                if (iQuantity > System.Convert.ToDecimal(objProduct.CustomerOrderStockQty))
                                                {
                                                    iQuantity = System.Convert.ToDecimal(objProduct.CustomerOrderStockQty);
                                                }
                                            }
                                        }

                                        treeListImportOrder.AppendNode(new object[] { objProduct.ID, objProduct.ID_Ib, objProduct.Article, 
                                                objProduct.Name, objProduct.CustomerOrderResQty, objProduct.CustomerOrderStockQty, 
                                                iQuantity,  dblPRICE, ( System.Convert.ToDouble( iQuantity ) * dblPRICE ) }, null).Tag = objProduct;

                                        treeListImportOrder.Refresh();

                                        listEditLog.Items.Add(String.Format("{0} OK ", i));
                                        listEditLog.Refresh();
                                    }
                                }
                                else if ((bErrExists == true) && (bStopRead == false))
                                {
                                    // ошибка, необходимо пометить запись
                                    treeListImportOrder.AppendNode(new object[] { null, iPartsId, strARTICLE, 
                                                strNAME2, 0, 0, iQuantity, dblPRICE, "НЕ импортирован"  }, null).Tag = null;

                                    treeListImportOrder.Refresh();

                                    listEditLog.Items.Add(String.Format("{0} OK ", i));
                                    listEditLog.Refresh();
                                }

                            }

                            iCurrentRow++;
                            i++;
                            strFrstColumn = System.Convert.ToString(worksheet.Cells[iCurrentRow, 1].Value);
                            listEditLog.Refresh();

                            this.Text = String.Format("обрабатывается запись №{0}", i);
                            this.Refresh();


                        } //while (bStopRead == false)
                    }
                    worksheet = null;
                }


            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка импорта данных из MS Excel.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                treeListImportOrder.BestFitColumns();
                this.Text = strCaption;
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            return;
        }

        private void btnLoadDataFromFile_Click(object sender, EventArgs e)
        {
            try
            {
                switch (m_ImportOrderType)
                {
                    case  enumImportOrderType.Order:
                        ReadDataFromXLSFile(txtID_Ib.Text);
                        break;
                    case  enumImportOrderType.intOrder:
                        ReadDataFromXLSFileForIntOrderByID(txtID_Ib.Text);
                        break;
                    default:
                        break;
                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "btnLoadDataFromFile_Click.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void cboxSheet_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                btnLoadDataFromFile.Enabled = (cboxSheet.SelectedItem != null);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "cboxSheet_SelectedValueChanged.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        #endregion

        #region Подтвердить выбор
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtID_Ib.Text == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Укажите файл шаблона MS Excel.", "Предупреждение",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }

                switch (m_ImportOrderType)
                {
                    case enumImportOrderType.Order:
                        ImportdataToOrderItems();
                        break;
                    case enumImportOrderType.intOrder:
                        ImportdataToIntOrderItems();
                        break;
                    default:
                        break;
                }

                if (treeListImportOrder.Nodes.Count == 0)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
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
                this.Close();

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

        #region Сохранить настройки в БД

        private void SaveSettings()
        {
            try
            {
                if ((m_objSettingForImportData != null) && (m_objSettingForImportData.SettingsList != null))
                {
                    CSettingItemForImportData objSetting = null;
                    foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListSettings.Nodes)
                    {
                        if (objNode.Tag == null) { continue; }
                        objSetting = (CSettingItemForImportData)objNode.Tag;
                        foreach (CSettingItemForImportData objItem in m_objSettingForImportData.SettingsList)
                        {
                            if (objSetting.TOOLS_ID == objItem.TOOLS_ID)
                            {
                                objItem.TOOLS_VALUE = objSetting.TOOLS_VALUE;
                                objItem.TOOLS_DESCRIPTION = objSetting.TOOLS_DESCRIPTION;
                                break;
                            }
                        }
                    }
                }

                System.String strErr = "";
                if (SaveXMLSettings(ref strErr) == false)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Ошибка сохранения настроек в базе данных.\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "SaveSettings.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return ;
        }

        private System.Boolean SaveXMLSettings(ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                System.Xml.XmlNodeList nodeList = m_objSettingForImportData.XMLSettings.GetElementsByTagName(strNodeSettingname);
                if (nodeList != null)
                {
                    CSettingItemForImportData objSetting = null;
                    foreach (System.Xml.XmlNode xmlNode in nodeList)
                    {
                        foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListSettings.Nodes)
                        {
                            if (objNode.Tag == null) { continue; }
                            objSetting = (CSettingItemForImportData)objNode.Tag;

                            if ( objSetting.TOOLS_ID.ToString() == xmlNode.Attributes[0].Value)
                            {
                                xmlNode.Attributes[3].InnerText = System.Convert.ToString(objNode.GetValue(colSettingsDescription));
                                xmlNode.Attributes[4].InnerText = System.Convert.ToString(objNode.GetValue(colSettingsColumnNum));
                            }
                        }
                    }
                    // теперь и в Базе данных
                    bRet = m_objSettingForImportData.SaveExportSetting( m_objProfile, null, ref strErr );
                }
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "SaveXMLSettingsForSheet.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return bRet;

        }

        private void btnSaveSetings_Click(object sender, EventArgs e)
        {
            try
            {
                SaveSettings();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка сохранения настроек.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return ;

        }
        #endregion

        #region Импорт содержимого файла в приложение к заказу
        /// <summary>
        /// Импорт данных в приложение к заказу
        /// </summary>
        private void ImportdataToOrderItems()
        {
            System.String strCaption = this.Text;
            if (treeListImportOrder == null) { return; }
            try
            {
                Cursor = Cursors.WaitCursor;
                this.Text = "идёт импорт данных в приложение к заказу...";

                this.tableLayoutPanel1.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeListImportOrder)).BeginInit();

                //m_objdtOrderItems.Clear();
                System.Data.DataRow newRowOrderItems = null;
                CProduct objProduct = null;
                System.Double PriceImporter = 0;
                System.Double Price = 0;
                System.Double dblOrderPrice = 0;
                System.Double PriceWithDiscount = 0;
                System.Double NDSPercent = 0;
                System.Double PriceInAccountingCurrency = 0;
                System.Double PriceWithDiscountInAccountingCurrency = 0;
                System.String strErr = "";
                System.Int32 i = 0;
                List<DevExpress.XtraTreeList.Nodes.TreeListNode> objNodeForDeleteList = new List<DevExpress.XtraTreeList.Nodes.TreeListNode>();
                System.Boolean bErrExists = false;
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListImportOrder.Nodes)
                {
                    i++;
                    bErrExists = false;
                    if (objNode.Tag == null) { continue; }
                    objProduct = (CProduct)objNode.Tag;

                    PriceImporter = 0;
                    Price = 0;
                    dblOrderPrice = 0;
                    PriceWithDiscount = 0;
                    NDSPercent = 0;
                    PriceInAccountingCurrency = 0;
                    PriceWithDiscountInAccountingCurrency = 0;
                    strErr = "";

                     // количество
                     if( System.Convert.ToDouble( objNode.GetValue( colOrderQty ) ) <= 0 ) 
                     {
                       bErrExists = true;
                       listEditLog.Items.Add( i.ToString() +  "Количество должно быть больше нуля. Количество в заказе: " + System.Convert.ToString( objNode.GetValue( colOrderQty ) ) );
                     }

                     if( System.Convert.ToDouble( objNode.GetValue( colStockQty ) ) <= 0 ) 
                     {
                       bErrExists = true;
                       listEditLog.Items.Add(i.ToString() + "Товар отсутствует на складе. " + objProduct.ProductFullName);
                     }

                     dblOrderPrice = System.Convert.ToDouble(objNode.GetValue(colOrderPrice));

                     if (bErrExists == false)
                     {
                         newRowOrderItems = m_objdtOrderItems.NewRow();

                         //newRowOrderItems["OrderItemsID"] = System.Guid.NewGuid();
                         newRowOrderItems["ProductID"] = objProduct.ID;
                         newRowOrderItems["ProductID"] = objProduct.ID;
                         newRowOrderItems["MeasureID"] = objProduct.Measure.ID;

                         // 2014.01.03
                         // отключаю автоматическое редактирование заказанного количества

                         //if (System.Convert.ToDouble(objNode.GetValue(colOrderQty)) > System.Convert.ToDouble(objNode.GetValue(colStockQty)))
                         //{
                         //    newRowOrderItems["OrderedQuantity"] = System.Convert.ToDouble(objNode.GetValue(colStockQty));
                         //    newRowOrderItems["QuantityReserved"] = System.Convert.ToDouble(objNode.GetValue(colStockQty));
                         //}
                         //else
                         {
                             newRowOrderItems["OrderedQuantity"] = System.Convert.ToDouble(objNode.GetValue(colOrderQty));
                             newRowOrderItems["QuantityReserved"] = System.Convert.ToDouble(objNode.GetValue(colOrderQty));
                         }

                         newRowOrderItems["OrderPackQty"] = objProduct.CustomerOrderMinRetailQty;
                         newRowOrderItems["OrderItems_QuantityInstock"] = objProduct.CustomerOrderStockQty;
                         newRowOrderItems["OrderItems_MeasureName"] = objProduct.Measure.ShortName;
                         newRowOrderItems["OrderItems_PartsName"] = objProduct.Name;
                         newRowOrderItems["OrderItems_PartsArticle"] = objProduct.Article;

                         // 2014.01.08
                         // реализовать универсальный метод
                         // в GetPriceForOrderItem передавать информацию о том, что цена импортируется из прайса
                         if (COrderRepository.GetPriceForOrderItem(m_objProfile, objProduct.ID, m_objSelectedStock.ID,
                             m_objPaymentType.ID, m_dblDiscountPercent,
                             ref PriceImporter, ref Price, ref PriceWithDiscount, ref NDSPercent, ref PriceInAccountingCurrency,
                             ref PriceWithDiscountInAccountingCurrency, ref strErr, dblOrderPrice, checkEditImportPrices.Checked) == true)
                         {

                             newRowOrderItems["PriceImporter"] = PriceImporter;
                             newRowOrderItems["Price"] = Price;
                             newRowOrderItems["DiscountPercent"] = m_dblDiscountPercent;
                             newRowOrderItems["PriceWithDiscount"] = PriceWithDiscount;
                             newRowOrderItems["PriceInAccountingCurrency"] = PriceInAccountingCurrency;
                             newRowOrderItems["PriceWithDiscountInAccountingCurrency"] = PriceWithDiscountInAccountingCurrency;
                             newRowOrderItems["NDSPercent"] = NDSPercent;

                             objNodeForDeleteList.Add(objNode);
                             listEditLog.Items.Add(String.Format("{0} OK", i));
                         }
                         else
                         {
                             listEditLog.Items.Add(String.Format("{0} Запрос цены для позиции: {1}", i, strErr));
                         }

                         m_objdtOrderItems.Rows.Add(newRowOrderItems);
                     }
                }

                newRowOrderItems = null;
                m_objdtOrderItems.AcceptChanges();

                if (objNodeForDeleteList != null)
                {
                    foreach( DevExpress.XtraTreeList.Nodes.TreeListNode objNode in objNodeForDeleteList )
                    {
                        treeListImportOrder.Nodes.Remove(objNode);
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка импорта данных в приложение к заказу.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                this.tableLayoutPanel1.ResumeLayout(false);
                this.tableLayoutPanel1.PerformLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeListImportOrder)).EndInit();

                this.Text = strCaption;

                Cursor = Cursors.Default;
            }

            return;

        }

        /// <summary>
        /// Импорт данных в приложение к заказу
        /// </summary>
        private void ImportdataToIntOrderItems()
        {
            System.String strCaption = this.Text;
            if (treeListImportOrder == null) { return; }
            try
            {
                Cursor = Cursors.WaitCursor;
                this.Text = "идёт импорт данных в приложение к заказу...";

                this.tableLayoutPanel1.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeListImportOrder)).BeginInit();

                //m_objdtOrderItems.Clear();
                System.Data.DataRow newRowOrderItems = null;
                CProduct objProduct = null;
                System.Double PriceImporter = 0;
                System.Double Price = 0;
                System.Double dblOrderPrice = 0;
                System.Double PriceWithDiscount = 0;
                System.Double PriceRetail = 0;
                System.Double NDSPercent = 0;
                System.Double MarkUpPercent = 0;
                System.String strErr = "";
                System.Int32 i = 0;
                List<DevExpress.XtraTreeList.Nodes.TreeListNode> objNodeForDeleteList = new List<DevExpress.XtraTreeList.Nodes.TreeListNode>();
                System.Boolean bErrExists = false;
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListImportOrder.Nodes)
                {
                    i++;
                    bErrExists = false;
                    if (objNode.Tag == null) { continue; }
                    objProduct = (CProduct)objNode.Tag;

                    PriceImporter = 0;
                    Price = 0;
                    dblOrderPrice = 0;
                    PriceWithDiscount = 0;
                    PriceRetail = 0;
                    NDSPercent = 0;
                    MarkUpPercent = 0;
                    strErr = System.String.Empty;

                    // количество
                    if (System.Convert.ToDouble(objNode.GetValue(colOrderQty)) <= 0)
                    {
                        bErrExists = true;
                        listEditLog.Items.Add(String.Format("{0}Количество должно быть больше нуля. Количество в заказе: {1}", i, System.Convert.ToString(objNode.GetValue(colOrderQty))));
                    }

                    if (System.Convert.ToDouble(objNode.GetValue(colStockQty)) <= 0)
                    {
                        bErrExists = true;
                        listEditLog.Items.Add(String.Format("{0}Товар отсутствует на складе. {1}", i, objProduct.ProductFullName));
                    }

                    dblOrderPrice = System.Convert.ToDouble(objNode.GetValue(colOrderPrice));

                    if (bErrExists == false)
                    {
                        newRowOrderItems = m_objdtOrderItems.NewRow();

                        newRowOrderItems["ProductID"] = objProduct.ID;
                        newRowOrderItems["MeasureID"] = objProduct.Measure.ID;

                        // 2014.01.03
                        // отключаю автоматическое редактирование заказанного количества

                        //if (System.Convert.ToDouble(objNode.GetValue(colOrderQty)) > System.Convert.ToDouble(objNode.GetValue(colStockQty)))
                        //{
                        //    newRowOrderItems["OrderedQuantity"] = System.Convert.ToDouble(objNode.GetValue(colStockQty));
                        //    newRowOrderItems["Quantity"] = System.Convert.ToDouble(objNode.GetValue(colStockQty));
                        //}
                        //else
                        {
                            newRowOrderItems["Quantity"] = System.Convert.ToDouble(objNode.GetValue(colOrderQty));
                        }

                        newRowOrderItems["OrderItems_QuantityInstock"] = objProduct.CustomerOrderStockQty;
                        newRowOrderItems["OrderItems_MeasureName"] = objProduct.Measure.ShortName;
                        newRowOrderItems["OrderItems_PartsName"] = objProduct.Name;
                        newRowOrderItems["OrderItems_PartsArticle"] = objProduct.Article;

                        // 2014.01.08
                        // реализовать универсальный метод
                        // в GetPriceForOrderItem передавать информацию о том, что цена импортируется из прайса

                        if (CIntOrder.GetPriceForOrderItem(m_objProfile, objProduct.ID, m_objSelectedStock.ID, m_objPriceType.ID, m_dblDiscountPercent,
                            ref PriceImporter, ref Price, ref PriceWithDiscount, ref PriceRetail, ref NDSPercent, ref MarkUpPercent, 
                            ref strErr, dblOrderPrice, checkEditImportPrices.Checked) == true)
                        {
                            newRowOrderItems["PriceImporter"] = PriceImporter;
                            newRowOrderItems["Price"] = Price;
                            newRowOrderItems["DiscountPercent"] = m_dblDiscountPercent;
                            newRowOrderItems["PriceWithDiscount"] = PriceWithDiscount;
                            newRowOrderItems["PriceRetail"] = PriceRetail;
                            newRowOrderItems["NDSPercent"] = NDSPercent;
                            newRowOrderItems["MarkUp"] = MarkUpPercent;

                            objNodeForDeleteList.Add(objNode);
                            listEditLog.Items.Add(String.Format("{0} OK", i));
                        }
                        else
                        {
                            listEditLog.Items.Add(String.Format("{0} Запрос цены для позиции: {1}", i, strErr));
                        }

                        m_objdtOrderItems.Rows.Add(newRowOrderItems);
                    }
                }

                newRowOrderItems = null;
                m_objdtOrderItems.AcceptChanges();

                if (objNodeForDeleteList != null)
                {
                    foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in objNodeForDeleteList)
                    {
                        treeListImportOrder.Nodes.Remove(objNode);
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка импорта данных в приложение к заказу.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                this.tableLayoutPanel1.ResumeLayout(false);
                this.tableLayoutPanel1.PerformLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeListImportOrder)).EndInit();

                this.Text = strCaption;

                Cursor = Cursors.Default;
            }

            return;

        }
        #endregion

        #region Закрытие формы
        private void frmImportXLSData_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                m_strFileFullName = txtID_Ib.Text;
                SelectedSheetId = cboxSheet.SelectedIndex;
                SheetList = new List<string>();
                foreach (object objItem in cboxSheet.Properties.Items)
                {
                    SheetList.Add(System.Convert.ToString(objItem));
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "frmImportXLSData_FormClosed.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        #endregion

        #region Отрисовка ячеек дерева
        private void treeListImportOrder_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
        {
            try
            {
                System.Double dblStockQty = System.Convert.ToDouble(e.Node.GetValue(colStockQty));
                System.Double dblOrderQty = System.Convert.ToDouble(e.Node.GetValue(colOrderQty));
                
                if (dblStockQty == 0)
                {
                    if (e.Column == colStockQty)
                    {
                        e.Appearance.DrawString(e.Cache, e.CellText,
                                    new Rectangle(e.Bounds.Location.X, e.Bounds.Location.Y,
                                    e.Bounds.Size.Width - 3, e.Bounds.Size.Height), new System.Drawing.SolidBrush(Color.Red));
                        e.Handled = true;
                    }
                }
                else if ((dblStockQty > 0) && (dblOrderQty > dblStockQty))
                {
                    if (e.Column == colOrderQty)
                    {
                        e.Appearance.DrawString(e.Cache, e.CellText,
                                    new Rectangle(e.Bounds.Location.X, e.Bounds.Location.Y,
                                    e.Bounds.Size.Width - 3, e.Bounds.Size.Height), new System.Drawing.SolidBrush(Color.Green));
                        e.Handled = true;
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "treeListImportOrder_CustomDrawNodeCell.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void treeListImportOrder_CustomDrawNodeImages(object sender, DevExpress.XtraTreeList.CustomDrawNodeImagesEventArgs e)
        {
            try
            {
                if (treeListImportOrder.Nodes.Count == 0) { return; }
                if (e.Node == null) { return; }

                System.Boolean bProductCheck = (e.Node.Tag != null);
                System.Boolean bProductQty = (System.Convert.ToDecimal(e.Node.GetValue(colOrderQty)) > 0);

                int Y = e.SelectRect.Top + (e.SelectRect.Height - imglNodes.Images[0].Height) / 2;

                if ((bProductCheck == false) || (bProductQty == false))
                {
                    try
                    {
                        //ControlPaint.DrawImageDisabled(e.Graphics, imglNodes.Images[1], e.SelectRect.X, Y, Color.Black);
                        e.Graphics.DrawImage(imglNodes.Images[1], new Point(e.SelectRect.X, Y));
                        e.Handled = true;
                    }
                    catch { }
                }
                else
                {
                    try
                    {
                        e.Graphics.DrawImage(imglNodes.Images[0], new Point(e.SelectRect.X, Y));
                        e.Handled = true;
                    }
                    catch { }
                }
            }
            catch (System.Exception f)
            {
                System.Windows.Forms.MessageBox.Show(null, "treeListImportPlan_CustomDrawNodeImages\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        #endregion

    }
}
