using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraExport;
using DevExpress.XtraGrid.Export;
using DevExpress.XtraEditors;
using System.Data.OleDb;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;

namespace ERP_Mercury.Common
{
    public partial class ctrlOrderForCustomer : UserControl
    {
        #region Свойства
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;
        private List<CProduct> m_objPartsList;
        private List<CCustomer> m_objCustomerList;

        private COrder m_objSelectedOrder;
        private System.Guid m_uuidSelectedStockID;

        private System.Boolean m_bIsChanged;
        private System.Boolean m_bCustomerInBlackList;

        private System.Boolean m_bDisableEvents;
        private System.Boolean m_bNewObject;
        private System.Boolean m_bIsReadOnly;
        private System.String m_strXLSImportFilePath;
        private System.Int32 m_iXLSSheetImport;
        private List<System.String> m_SheetList;
        private System.Double m_CurratePricing;
        private System.Int32 m_SUMM_IS_PASS;
        private System.Boolean m_bIsAutoCreatePriceMode;

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
        private System.Guid m_iPaymentType1;
        private System.Guid m_iPaymentType2;
        private const System.String m_strPaymentType1 = "58636EC5-F64A-462C-90B1-7686ADFE70F9";
        private const System.String m_strPaymentType2 = "E872B5E3-83FF-4B1A-925D-0F1B3C4D5C85";
        private const System.String m_strReportsDirectory = "templates";
        private const System.String m_strReportSuppl = "Suppl.xlsx";
        private const System.Int32 m_iDebtorInfoPanelHeight = 117;
        private const System.Int32 m_iDebtorInfoPanelIndex = 1;
        #endregion

        #region События
        // Создаем закрытое поле, ссылающееся на заголовок списка делегатов
        private EventHandler<ChangeOrderForCustomerPropertieEventArgs> m_ChangeOrderForCustomerProperties;
        // Создаем в классе член-событие
        public event EventHandler<ChangeOrderForCustomerPropertieEventArgs> ChangeOrderForCustomerProperties
        {
            add
            {
                // берем закрытую блокировку и добавляем обработчик
                // (передаваемый по значению) в список делегатов
                m_ChangeOrderForCustomerProperties += value;
            }
            remove
            {
                // берем закрытую блокировку и удаляем обработчик
                // (передаваемый по значению) из списка делегатов
                m_ChangeOrderForCustomerProperties -= value;
            }
        }
        /// <summary>
        /// Инициирует событие и уведомляет о нем зарегистрированные объекты
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnChangeOrderForCustomerProperties(ChangeOrderForCustomerPropertieEventArgs e)
        {
            // Сохраняем поле делегата во временном поле для обеспечение безопасности потока
            EventHandler<ChangeOrderForCustomerPropertieEventArgs> temp = m_ChangeOrderForCustomerProperties;
            // Если есть зарегистрированные объектв, уведомляем их
            if (temp != null) temp(this, e);
        }
        public void SimulateChangeOrderProperties(COrder objOrder, enumActionSaveCancel enActionType, System.Boolean bIsNewOrder)
        {
            // Создаем объект, хранящий информацию, которую нужно передать
            // объектам, получающим уведомление о событии
            ChangeOrderForCustomerPropertieEventArgs e = new ChangeOrderForCustomerPropertieEventArgs(objOrder, enActionType, bIsNewOrder);

            // Вызываем виртуальный метод, уведомляющий наш объект о возникновении события
            // Если нет типа, переопределяющего этот метод, наш объект уведомит все объекты, 
            // подписавшиеся на уведомление о событии
            OnChangeOrderForCustomerProperties(e);
        }
        #endregion

        #region Конструктор
        public ctrlOrderForCustomer(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem, List<CCustomer> objCustomer)
        {
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ru-RU");
            ci.NumberFormat.CurrencyDecimalSeparator = ".";
            ci.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = ci;

            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_objCustomerList = objCustomer;

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(MyResolveEventHandler);

            InitializeComponent();


            m_bIsChanged = false;
            m_bDisableEvents = false;
            m_bNewObject = false;
            m_bIsAutoCreatePriceMode = false;
            m_uuidSelectedStockID = System.Guid.Empty;

            m_objSelectedOrder = null;
            m_objPartsList = null;
            m_iPaymentType1 = new Guid(m_strPaymentType1 );
            m_iPaymentType2 = new Guid(m_strPaymentType2);
            m_strXLSImportFilePath = "";
            m_iXLSSheetImport = 0;
            m_bCustomerInBlackList = false;
            m_CurratePricing = 0;
            m_SUMM_IS_PASS = 0;

            BeginDate.DateTime = System.DateTime.Today;
            DeliveryDate.DateTime = System.DateTime.Today;

            checkMultiplicity.CheckState = CheckState.Unchecked;

            LoadComboBoxItems();

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
                Customer.Properties.Appearance.BackColor = ((Customer.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                OrderType.Properties.Appearance.BackColor = ((OrderType.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                PaymentType.Properties.Appearance.BackColor = ((PaymentType.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                Rtt.Properties.Appearance.BackColor = ((Rtt.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                AddressDelivery.Properties.Appearance.BackColor = ((AddressDelivery.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                Depart.Properties.Appearance.BackColor = ((Depart.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                SalesMan.Properties.Appearance.BackColor = ((SalesMan.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                Stock.Properties.Appearance.BackColor = ((Stock.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);

                BeginDate.Properties.Appearance.BackColor = ((BeginDate.DateTime == System.DateTime.MinValue) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                DeliveryDate.Properties.Appearance.BackColor = (((DeliveryDate.DateTime == System.DateTime.MinValue) || (DeliveryDate.DateTime.CompareTo(BeginDate.DateTime) < 0)) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);

                if (Customer.SelectedItem == null) { bRet = false; }
                if (OrderType.SelectedItem == null) { bRet = false; }
                if (PaymentType.SelectedItem == null) { bRet = false; }
                if (Rtt.SelectedItem == null) { bRet = false; }
                if (AddressDelivery.SelectedItem == null) { bRet = false; }
                if (SalesMan.SelectedItem == null) { bRet = false; }
                if (Depart.SelectedItem == null) { bRet = false; }
                if (BeginDate.DateTime == System.DateTime.MinValue) { bRet = false; }
                if ((DeliveryDate.DateTime == System.DateTime.MinValue) || (DeliveryDate.DateTime.CompareTo(BeginDate.DateTime) < 0)) { bRet = false; }
                if ( dataSet.Tables[ "OrderItems" ].Rows.Count == 0) { bRet = false; }

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
                    SimulateChangeOrderProperties(m_objSelectedOrder, enumActionSaveCancel.Unkown, m_bNewObject);
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
        private void OrderItems_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                GetCustomerDebtInfo(false);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("OrderItems_RowChanged. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private void cboxOrderPropertie_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                System.String strErr = "";

                if( (sender == Customer) && (Customer.SelectedItem != null))
                {
                    CCustomer objCustomer = (CCustomer)Customer.SelectedItem;
                    // список РТТ
                    LoadRttListForCustomer(objCustomer.ID);
                    // список дочерних подразделений
                    LoadChildDeprtForCustomer(objCustomer.ID);

                    // телефоны клиента
                    objCustomer.PhoneList = CPhone.GetPhoneListForCustomer(m_objProfile, null,  objCustomer.ID, ref strErr);

                    if (Stock.SelectedItem != null)
                    {
                        CCompany objCompany = ( ( CStock )Stock.SelectedItem ).Company;
                        // договора
                        LoadStmntForCustomerCompany(objCustomer.ID, objCompany.ID);
                        objCompany = null;
                    }

                    objCustomer = null;
                    if (AddressDelivery.SelectedItem != null)
                    {
                        Stock.Focus();
                        Stock.SelectAll();
                    }

                    SetDefParamForOrder();
                }

                if (sender == Rtt)
                {
                    AddressDelivery.Properties.Items.Clear();
                    if( Rtt.SelectedItem != null )
                    {
                        AddressDelivery.Properties.Items.AddRange( ( ((CRtt)Rtt.SelectedItem).AddressList ));
                        AddressDelivery.SelectedItem = ((AddressDelivery.Properties.Items.Count == 0) ? null : AddressDelivery.Properties.Items[0]);
                    }
                }

                if ((sender == AddressDelivery) || (sender == PaymentType))
                {
                    ReloadDscrpnByAddress();
                }

                if (sender == Depart)
                {
                    LoadSalesManListForDepart((CDepart)Depart.SelectedItem);
                }

                if (sender == Stock)
                {
                    if (Stock.SelectedItem != null)
                    {
                        m_uuidSelectedStockID = ((CStock)Stock.SelectedItem).ID;

                        if (Customer.SelectedItem != null)
                        {
                            CCompany objCompany = ((CStock)Stock.SelectedItem).Company;
                            LoadStmntForCustomerCompany(((CCustomer)Customer.SelectedItem).ID, objCompany.ID);
                            objCompany = null;
                        }


                        StartThreadWithLoadData();
                    }

                    GetCustomerDebtInfo(true);
                }

                if ((sender == cboxProductTradeMark) || (sender == cboxProductType))
                {
                    SetFilterForPartsCombo();
                }

                if (sender == Customer)
                {
                    ChildDepart.SelectedItem = null;

                    if( (ChildDepart.Properties.Items.Count > 0) && ( PaymentType.SelectedItem != null ) )
                    {
                        if (((CPaymentType)PaymentType.SelectedItem).ID == m_iPaymentType2)
                        {
                            ChildDepart.SelectedItem = ChildDepart.Properties.Items[0];
                        }
                    }

                    GetCustomerDebtInfo( true );
                }
                if (sender == OrderType)
                {
                    SetDefParamForOrder();
                }

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

        private void ReloadDscrpnByAddress()
        {
            try
            {
                if ((AddressDelivery.SelectedItem != null) && (PaymentType.SelectedItem != null) && (((CPaymentType)PaymentType.SelectedItem).ID == m_iPaymentType2))
                {
                    txtDescription.Text = (AddressDelivery.Text);
                    if ((Customer.SelectedItem != null) && (((CCustomer)Customer.SelectedItem).PhoneList != null))
                    {
                        List<CPhone> objPhoneList = ((CCustomer)Customer.SelectedItem).PhoneList;
                        foreach (CPhone objPhone in objPhoneList)
                        {
                            txtDescription.Text += ("  " + objPhone.PhoneNumber);
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("ReloadDscrpnByAddress. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

        private void txtOrderPropertie_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
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
        private void gridViewProductList_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }

                SetPropertiesModified(true);
                
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("gridViewProductList_CellValueChanged. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        private void gridViewProductList_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }

                SetPropertiesModified(true);
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("gridViewProductList_CellValueChanging. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        private void SetPriceInRow(System.Int32 iRowHandle, System.Guid uuidPartsId, System.Guid uuidStockId, System.Guid uuidPaymentTypeId, System.Double dblDiscountPercent)
        {
            try
            {
                System.Double PriceImporter = 0;
                System.Double Price = 0;
                System.Double PriceWithDiscount = 0;
                System.Double NDSPercent = 0;
                System.Double PriceInAccountingCurrency = 0;
                System.Double PriceWithDiscountInAccountingCurrency = 0;
                System.String strErr = "";

                if (COrderRepository.GetPriceForOrderItem(m_objProfile, uuidPartsId, uuidStockId, uuidPaymentTypeId, dblDiscountPercent,
                    ref PriceImporter, ref Price, ref PriceWithDiscount, ref NDSPercent, ref PriceInAccountingCurrency,
                    ref PriceWithDiscountInAccountingCurrency, ref strErr) == true)
                {
                    gridView.SetRowCellValue(iRowHandle, colNDSPercent, NDSPercent);
                    gridView.SetRowCellValue(iRowHandle, colPriceImporter, PriceImporter);
                    gridView.SetRowCellValue(iRowHandle, colPrice, Price);
                    gridView.SetRowCellValue(iRowHandle, colPriceWithDiscount, PriceWithDiscount);
                    gridView.SetRowCellValue(iRowHandle, colPriceInAccountingCurrency, PriceInAccountingCurrency);
                    gridView.SetRowCellValue(iRowHandle, colPriceWithDiscountInAccountingCurrency, PriceWithDiscountInAccountingCurrency);
                }
                else
                {
                    SendMessageToLog("Запрос цены для позиции: " + strErr);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("SetPriceInRow. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private void gridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }

                if ((e.Column == colProductID) && (m_objPartsList != null) && (e.Value != null))
                {

                    CProduct objItem = null;
                    try
                    {
                        objItem = m_objPartsList.Cast<CProduct>().Single<CProduct>(x => x.ID.CompareTo((System.Guid)e.Value) == 0);

                        gridView.SetRowCellValue(e.RowHandle, colMeasureID, objItem.Measure.ID);
                        gridView.SetRowCellValue(e.RowHandle, colOrderItems_MeasureName, objItem.Measure.ShortName);
                        gridView.SetRowCellValue(e.RowHandle, colOrderItems_QuantityInstock, objItem.CustomerOrderStockQty);

                        if (objItem.CustomerOrderStockQty < objItem.CustomerOrderMinRetailQty)
                        {
                            gridView.SetRowCellValue(e.RowHandle, colOrderedQuantity, objItem.CustomerOrderStockQty);
                            gridView.SetRowCellValue(e.RowHandle, colQuantityReserved, objItem.CustomerOrderStockQty);
                        }
                        else
                        {
                            gridView.SetRowCellValue(e.RowHandle, colOrderedQuantity, objItem.CustomerOrderMinRetailQty);
                            gridView.SetRowCellValue(e.RowHandle, colQuantityReserved, objItem.CustomerOrderMinRetailQty);
                        }

                        gridView.SetRowCellValue(e.RowHandle, colNDSPercent, objItem.ProductType.NDSRate);
                        gridView.SetRowCellValue(e.RowHandle, colOrderPackQty, objItem.CustomerOrderMinRetailQty);
                        gridView.SetRowCellValue(e.RowHandle, colOrderItems_PartsArticle, objItem.Article);
                        gridView.SetRowCellValue(e.RowHandle, colOrderItems_PartsName, objItem.Name);

                        System.Guid uuidStockId = ((Stock.SelectedItem == null) ? System.Guid.Empty : ((CStock)Stock.SelectedItem).ID);
                        System.Guid uuidPaymentTypeId = ((PaymentType.SelectedItem == null) ? System.Guid.Empty : ((CPaymentType)PaymentType.SelectedItem).ID);
                        if ((uuidStockId != System.Guid.Empty) && (uuidPaymentTypeId != System.Guid.Empty))
                        {
                            SetPriceInRow(e.RowHandle, objItem.ID, uuidStockId, uuidPaymentTypeId, System.Convert.ToDouble(gridView.GetRowCellValue(e.RowHandle, colDiscountPercent)));
                        }

                        //gridView.FocusedColumn = colOrderedQuantity;

                        gridView.UpdateCurrentRow();
                        
                    }
                    catch
                    {
                    }
                    finally
                    {
                        objItem = null;
                    }
                }
                else if ((e.Column == colDiscountPercent) && (e.Value != null))
                {
                    if( (m_bIsAutoCreatePriceMode == true) && ( m_bIsReadOnly == false ) )
                    {
                        if (System.Convert.ToDouble(gridView.GetRowCellValue(e.RowHandle, colDiscountPercent)) != 0)
                        {
                            gridView.SetRowCellValue(e.RowHandle, colDiscountPercent, 0);
                        }
                    }
                    else
                    {
                        System.Guid uuidStockId = ((Stock.SelectedItem == null) ? System.Guid.Empty : ((CStock)Stock.SelectedItem).ID);
                        System.Guid uuidPaymentTypeId = ((PaymentType.SelectedItem == null) ? System.Guid.Empty : ((CPaymentType)PaymentType.SelectedItem).ID);
                        if ((uuidStockId != System.Guid.Empty) && (uuidPaymentTypeId != System.Guid.Empty))
                        {
                            SetPriceInRow(e.RowHandle, (System.Guid)gridView.GetRowCellValue(e.RowHandle, colProductID), uuidStockId, uuidPaymentTypeId, System.Convert.ToDouble(gridView.GetRowCellValue(e.RowHandle, colDiscountPercent)));
                        }
                    }

                }
                else if ((e.Column == colOrderedQuantity) && (e.Value != null) && (gridView.GetRowCellValue(e.RowHandle, colProductID) != null) &&
                    (gridView.GetRowCellValue(e.RowHandle, colOrderPackQty) != System.DBNull.Value) &&
                    (gridView.GetRowCellValue(e.RowHandle, colOrderItems_QuantityInstock) != System.DBNull.Value))
                {
                    System.Decimal dclOrderedQty = System.Convert.ToDecimal(e.Value);
                    System.Decimal dclmultiplicity = System.Convert.ToDecimal(gridView.GetRowCellValue(e.RowHandle, colOrderPackQty));
                    System.Decimal dclInstockQty = System.Convert.ToDecimal(gridView.GetRowCellValue(e.RowHandle, colOrderItems_QuantityInstock));

                    if (checkMultiplicity.CheckState == CheckState.Checked)
                    {
                        if ((dclOrderedQty % dclmultiplicity) != 0)
                        {
                            dclOrderedQty = (((int)dclOrderedQty / (int)dclmultiplicity) * dclmultiplicity) + dclmultiplicity;
                            if (dclOrderedQty > dclInstockQty)
                            {
                                dclOrderedQty = dclInstockQty;
                            }
                        }
                    }

                    if (System.Convert.ToDecimal(System.Convert.ToDecimal(e.Value)) != dclOrderedQty)
                    {
                        gridView.SetRowCellValue(e.RowHandle, colOrderedQuantity, System.Convert.ToDecimal(dclOrderedQty));
                    }
                    gridView.SetRowCellValue(e.RowHandle, colQuantityReserved, System.Convert.ToDecimal(dclOrderedQty));
                    gridView.UpdateCurrentRow();

                }
                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("gridView_CellValueChanged. Текст ошибки: " + f.Message);
            }
            finally
            {
                
            }
            return;
        }

        private void gridView_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            try
            {
                e.Info.DisplayText = ((e.RowHandle < 0) ? "" : ("№ " + System.String.Format("{0:### ### ##0}", (e.RowHandle + 1))));
            }
            catch (System.Exception f)
            {
                SendMessageToLog("gridView_CustomDrawRowIndicator. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private void repositoryItemLookUpEditProduct_CloseUp(object sender, DevExpress.XtraEditors.Controls.CloseUpEventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }

                if ((e.AcceptValue == true) && (e.Value != null) && (e.Value.GetType().Name != "DBNull"))
                {
                    gridView.SetRowCellValue(gridView.FocusedRowHandle, colProductID, (System.Guid)e.Value);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("repositoryItemLookUpEditProduct_CloseUp. Текст ошибки: " + f.Message);
            }
            finally
            {
                ValidateProperties();
            }
            return;
        }

        private void repositoryItemCalcEditOrderedQuantity_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            //try
            //{
            //    if (m_bDisableEvents == true) { return; }

            //    if (e.NewValue != null)
            //    {
            //        System.Decimal dclOrderedQty = System.Convert.ToDecimal(e.NewValue);
            //        System.Decimal dclmultiplicity = System.Convert.ToDecimal(gridView.GetRowCellValue(gridView.FocusedRowHandle, colOrderPackQty));
            //        System.Decimal dclInstockQty = System.Convert.ToDecimal(gridView.GetRowCellValue(gridView.FocusedRowHandle, colOrderItems_QuantityInstock));

            //        if ((dclOrderedQty % dclmultiplicity) != 0)
            //        {
            //            dclOrderedQty = (((int)dclOrderedQty / (int)dclmultiplicity) * dclmultiplicity) + dclmultiplicity;
            //            if (dclOrderedQty > dclInstockQty)
            //            {
            //                dclOrderedQty = dclInstockQty;
            //            }
            //        }

            //        if (System.Convert.ToDecimal(e.NewValue) != dclOrderedQty)
            //        {
            //            e.NewValue = dclOrderedQty;
            //            OrderItems.Rows[gridView.FocusedRowHandle][OrderedQuantity] = dclOrderedQty;
            //            OrderItems.Rows[gridView.FocusedRowHandle][QuantityReserved] = dclOrderedQty;
            //            OrderItems.AcceptChanges();

            //          //  gridView.SetRowCellValue(gridView.FocusedRowHandle, colOrderedQuantity, System.Convert.ToDecimal(dclOrderedQty));
            //        }
            //        //gridView.SetRowCellValue(gridView.FocusedRowHandle, colQuantityReserved, System.Convert.ToDecimal(dclOrderedQty));
            //        //gridView.UpdateCurrentRow();
            //    }
            //}
            //catch (System.Exception f)
            //{
            //    SendMessageToLog("repositoryItemCalcEditOrderedQuantity_EditValueChanged. Текст ошибки: " + f.Message);
            //}
            //finally
            //{
            //}
            //return;
        }

        private void gridView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if ((gridView.GetRowCellValue(e.RowHandle, colQuantityReserved) != null) &&
                    (gridView.GetRowCellValue(e.RowHandle, colOrderedQuantity) != null))
                {
                    System.Double dblQuantityReserved = System.Convert.ToDouble(gridView.GetRowCellValue(e.RowHandle, colQuantityReserved));
                    System.Double dblOrderedQuantity = System.Convert.ToDouble(gridView.GetRowCellValue(e.RowHandle, colOrderedQuantity));
                    if (dblQuantityReserved != dblOrderedQuantity)
                    {
                        if ((e.Column == colOrderedQuantity) || (e.Column == colQuantityReserved))
                        {
                            Rectangle r = e.Bounds;
                            e.Appearance.DrawString(e.Cache, e.DisplayText, r);

                            e.Handled = true;
                        }
                    }

                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("gridView_CustomDrawCell. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// Устанавливает указанный процент скидки на все позиции в заказе
        /// </summary>
        /// <param name="dblDiscountPercent">размер скидки в процентах</param>
        private void SenDiscountPercent(System.Double dblDiscountPercent)
        {
            try
            {
                tableLayoutPanelBackground.SuspendLayout();

                if (gridView.RowCount == 0) { return; }

                Cursor = Cursors.WaitCursor;
                System.Guid uuidStockId = ((Stock.SelectedItem == null) ? System.Guid.Empty : ((CStock)Stock.SelectedItem).ID);
                System.Guid uuidPaymentTypeId = ((PaymentType.SelectedItem == null) ? System.Guid.Empty : ((CPaymentType)PaymentType.SelectedItem).ID);
                if ((uuidStockId != System.Guid.Empty) && (uuidPaymentTypeId != System.Guid.Empty))
                {
                    for (System.Int32 i = 0; i < gridView.RowCount; i++)
                    {
                        gridView.SetRowCellValue(i, colDiscountPercent, dblDiscountPercent);
                        //SetPriceInRow(i, (System.Guid)gridView.GetRowCellValue(i, colProductID), uuidStockId, uuidPaymentTypeId, System.Convert.ToDouble(gridView.GetRowCellValue(i, colDiscountPercent)));
                    }
                }

            }
            catch (System.Exception f)
            {
                SendMessageToLog("SenDiscountPercent. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.tableLayoutPanelBackground.ResumeLayout(false);
                gridView.RefreshData();
                Cursor = Cursors.Default;
            }
            return;
        }
        private void btnSetDiscount_Click(object sender, EventArgs e)
        {
            try
            {
                SenDiscountPercent(System.Convert.ToDouble(spinEditDiscount.Value));
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnSetDiscount_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.tableLayoutPanelBackground.ResumeLayout(false);
                gridView.RefreshData();
                Cursor = Cursors.Default;
            }
            return;
        }

        /// <summary>
        /// Установка реквизитов при оформлении нового заказа
        /// </summary>
        private void SetDefParamForOrder()
        {
            try
            {
                System.Guid In_OrderType_Guid = ( ( OrderType.SelectedItem == null )  ? System.Guid.Empty : ( ( COrderType )OrderType.SelectedItem ).Id );
                System.Guid In_PaymentType_Guid = ( ( PaymentType.SelectedItem == null ) ? System.Guid.Empty : ( ( CPaymentType )PaymentType.SelectedItem ).ID );

                System.Boolean SetChildDepartNull = false; 
                System.Boolean SetDepartValue = false;
                System.Boolean SetChildDepartValue = false;
                System.Guid Depart_Guid = System.Guid.Empty; 
                System.Guid OrderType_Guid = System.Guid.Empty; 
                System.Guid PaymentType_Guid = System.Guid.Empty;
                System.String strErr = System.String.Empty;

                if (COrder.GetOrderDefParams(m_objProfile,
                    In_OrderType_Guid, In_PaymentType_Guid, ref SetChildDepartNull, ref SetDepartValue, ref SetChildDepartValue,
                    ref Depart_Guid, ref OrderType_Guid, ref PaymentType_Guid, ref strErr) == true)
                {
                    m_bDisableEvents = true;
                    if (In_PaymentType_Guid.CompareTo(PaymentType_Guid) != 0)
                    {
                        PaymentType.SelectedItem = PaymentType.Properties.Items.Cast<CPaymentType>().SingleOrDefault<CPaymentType>(x => x.ID.CompareTo(PaymentType_Guid) == 0);
                    }
                    if (In_OrderType_Guid.CompareTo(OrderType_Guid) != 0)
                    {
                        OrderType.SelectedItem = OrderType.Properties.Items.Cast<COrderType>().SingleOrDefault<COrderType>(x => x.Id.CompareTo(OrderType_Guid) == 0);
                    }
                    if (SetChildDepartNull == true)
                    {
                        ChildDepart.SelectedItem = null;
                    }
                    if (SetDepartValue == true)
                    {
                        Depart.SelectedItem = Depart.Properties.Items.Cast<CDepart>().SingleOrDefault<CDepart>(x => x.uuidID.CompareTo(Depart_Guid) == 0);
                    }
                    if ((SetChildDepartValue == true) && (ChildDepart.Properties.Items.Count > 0))
                    {
                        ChildDepart.SelectedItem = ChildDepart.Properties.Items[0];
                    }
                    m_bDisableEvents = false;

                }

            }

            catch (System.Exception f)
            {
                SendMessageToLog("SetDefParamForOrder. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        #endregion

        #region Выпадающие списки
        /// <summary>
        /// Обновление выпадающих списков
        /// </summary>
        /// <returns>true - все списки успешно обновлены; false - ошибка</returns>
        private System.Boolean LoadComboBoxItems()
        {
            System.Boolean bRet = false;
            try
            {

                Customer.Properties.Items.Clear();
                Customer.Properties.Items.AddRange(m_objCustomerList);

                Rtt.Properties.Items.Clear();
                AddressDelivery.Properties.Items.Clear();
                ChildDepart.Properties.Items.Clear();
                Depart.Properties.Items.Clear();
                cboxProductTradeMark.Properties.Items.Clear();
                cboxProductType.Properties.Items.Clear();

                // Склады
                Stock.Properties.Items.Clear();
                Stock.Properties.Items.AddRange(CStock.GetStockList(m_objProfile, null) ); //.Where<CStock>(x=>x.IsTrade == true).ToList<CStock>());

                // Тип заказа
                OrderType.Properties.Items.Clear();
                OrderType.Properties.Items.AddRange( COrderType.GetOrderTypeList(m_objProfile));

                // Форма оплаты
                PaymentType.Properties.Items.Clear();
                PaymentType.Properties.Items.AddRange(CPaymentType.GetPaymentTypeList(m_objProfile, null, System.Guid.Empty ));

                // Подразделение
                Depart.Properties.Items.Clear();
                Depart.Properties.Items.AddRange(CDepart.GetDepartList(m_objProfile, null));

                // Товары
                dataSet.Tables["Product"].Clear();
                repositoryItemLookUpEditProduct.DataSource = dataSet.Tables["Product"];

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
        /// Загружает список РТТ для клиента с указанным идентификатором
        /// </summary>
        /// <param name="uuidCustomerId">идентификатор клиента</param>
        private void LoadRttListForCustomer( System.Guid uuidCustomerId )
        {
            try
            {
                Rtt.SelectedItem = null;
                AddressDelivery.SelectedItem = null;

                Rtt.Properties.Items.Clear();
                AddressDelivery.Properties.Items.Clear();

                if (uuidCustomerId.CompareTo(System.Guid.Empty) != 0)
                {
                    Rtt.Properties.Items.AddRange(CRtt.GetRttList(m_objProfile, null, uuidCustomerId));
                    Rtt.SelectedItem = ((Rtt.Properties.Items.Count == 0) ? null : Rtt.Properties.Items[0]);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("LoadRttListForCustomer. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// Загружает список дочерних для клиента
        /// </summary>
        /// <param name="uuidCustomerId">идентификатор клиента</param>
        private void LoadChildDeprtForCustomer(System.Guid uuidCustomerId)
        {
            try
            {
                ChildDepart.SelectedItem = null;
                ChildDepart.Properties.Items.Clear();

                if (uuidCustomerId.CompareTo(System.Guid.Empty) != 0)
                {
                    ChildDepart.Properties.Items.AddRange(CChildDepart.GetChildDepartList(m_objProfile, null, uuidCustomerId));
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("LoadChildDeprtForCustomer. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// Список договоров для клиента и компании
        /// </summary>
        /// <param name="uuidCustomerId">уи клиента</param>
        /// <param name="uuidCompanyId">уи компании</param>
        private void LoadStmntForCustomerCompany(System.Guid uuidCustomerId, System.Guid uuidCompanyId)
        {
            try
            {
                Stmnt.SelectedItem = null;
                Stmnt.Properties.Items.Clear();

                if( (uuidCustomerId.CompareTo(System.Guid.Empty) != 0) &&
                    (uuidCompanyId.CompareTo(System.Guid.Empty) != 0))
                {
                    Stmnt.Properties.Items.AddRange(CStmnt.GetStmntList(m_objProfile, null, uuidCustomerId, uuidCompanyId, System.Guid.Empty));
                    if (Stmnt.Properties.Items.Count > 0)
                    {
                        Stmnt.SelectedItem = Stmnt.Properties.Items[0];
                    }
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("LoadStmntForCustomerCompany. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// Запрашивает договор для заказа
        /// </summary>
        /// <param name="uuidOrderId">уи заказа</param>
        private void LoadStmntForOrder(System.Guid uuidOrderId)
        {
            try
            {
                Stmnt.SelectedItem = null;
                Stmnt.Properties.Items.Clear();

                if (uuidOrderId.CompareTo(System.Guid.Empty) != 0)
                {
                    Stmnt.Properties.Items.AddRange(CStmnt.GetStmntList(m_objProfile, null, System.Guid.Empty, System.Guid.Empty, uuidOrderId));
                    if (Stmnt.Properties.Items.Count > 0)
                    {
                        Stmnt.SelectedItem = Stmnt.Properties.Items[0];
                    }
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("LoadStmntForOrder. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// Загружает значения в выпадающий список с торговыми представителями
        /// </summary>
        /// <param name="objDepart">Подразделение</param>
        private void LoadSalesManListForDepart(CDepart objDepart)
        {
            try
            {
                SalesMan.Properties.Items.Clear();
                if (objDepart != null)
                {
                    SalesMan.Properties.Items.Add(objDepart.SalesMan);
                    SalesMan.SelectedItem = SalesMan.Properties.Items[0];
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("LoadSalesManListForDepart. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// Фильтрует выпадающий список с товарами
        /// </summary>
        private void SetFilterForPartsCombo()
        {
            try
            {
                if( gridView.RowCount > 0 )
                {
                    if (DevExpress.XtraEditors.XtraMessageBox.Show("В результате применения фильтра будут удалены товары в заказе,\nне соответствующие заданным условиям.\nУстановить фильтр?", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Warning) == DialogResult.No)
                    {
                        cboxProductTradeMark.SelectedValueChanged -= new EventHandler(cboxOrderPropertie_SelectedValueChanged);
                        cboxProductType.SelectedValueChanged -= new EventHandler(cboxOrderPropertie_SelectedValueChanged);

                        cboxProductTradeMark.SelectedItem = null;
                        cboxProductType.SelectedItem = null;

                        cboxProductTradeMark.SelectedValueChanged += new EventHandler(cboxOrderPropertie_SelectedValueChanged);
                        cboxProductType.SelectedValueChanged += new EventHandler(cboxOrderPropertie_SelectedValueChanged);

                        return;
                    }
                }
                System.Guid TradeMarkId = ((cboxProductTradeMark.SelectedItem == null) ? System.Guid.Empty : ((CProductTradeMark)cboxProductTradeMark.SelectedItem).ID);
                System.Guid PartTypeId = ((cboxProductType.SelectedItem == null) ? System.Guid.Empty : ((CProductType)cboxProductType.SelectedItem).ID);

                //tableLayoutPanelBackground.SuspendLayout();

                List<CProduct> FilteredProductList = m_objPartsList;

                if (TradeMarkId != System.Guid.Empty)
                {
                    FilteredProductList = FilteredProductList.Where<CProduct>(x => x.ProductTradeMark.ID == TradeMarkId).ToList<CProduct>();
                }

                if (PartTypeId != System.Guid.Empty)
                {
                    FilteredProductList = FilteredProductList.Where<CProduct>(x => x.ProductType.ID == PartTypeId).ToList<CProduct>();
                }

                //if (FilteredProductList.Count != m_objPartsList.Count)
                //{
                    dataSet.Tables["Product"].Clear();

                    System.Data.DataRow newRowProduct = null;
                    dataSet.Tables["Product"].Clear();
                    foreach (CProduct objItem in FilteredProductList)
                    {
                        newRowProduct = dataSet.Tables["Product"].NewRow();

                        newRowProduct["ProductID"] = objItem.ID;
                        newRowProduct["Product_MeasureID"] = objItem.Measure.ID;
                        newRowProduct["Product_MeasureName"] = objItem.Measure.ShortName;
                        newRowProduct["ProductFullName"] = objItem.ProductFullName;
                        newRowProduct["CustomerOrderStockQty"] = objItem.CustomerOrderStockQty;
                        newRowProduct["CustomerOrderResQty"] = objItem.CustomerOrderResQty;
                        newRowProduct["CustomerOrderPackQty"] = objItem.CustomerOrderPackQty;
                        
                        dataSet.Tables["Product"].Rows.Add(newRowProduct);
                    }
                    newRowProduct = null;
                    dataSet.Tables["Product"].AcceptChanges();
                //}

                // теперь нужно проверить содержимое заказа на предмет товараов, не попадающих под условия фильтра
                if( ( gridView.RowCount > 0 ) && ( ( TradeMarkId != System.Guid.Empty ) || ( PartTypeId != System.Guid.Empty ) ) )
                {
                    System.Guid PartsGuid = System.Guid.Empty;
                    CProduct objProduct = null;
                    System.Boolean bOk = true;
                    for (System.Int32 i = ( gridView.RowCount - 1 ); i >=0 ; i--)
                    {
                        PartsGuid = (System.Guid)(gridView.GetDataRow(i)["ProductID"]);
                        bOk = true;
                        try
                        {
                            objProduct = m_objPartsList.SingleOrDefault<CProduct>(x => x.ID == PartsGuid);
                        }
                        catch
                        {
                            objProduct = null;
                        }
                        if (objProduct != null)
                        {
                            // проверяем на соответствие марке
                            if (TradeMarkId != System.Guid.Empty)
                            {
                                bOk = (objProduct.ProductTradeMark.ID == TradeMarkId);
                            }
                            if ((bOk == true) && (PartTypeId != System.Guid.Empty))
                            {
                                bOk = (objProduct.ProductTradeMark.ID == PartTypeId);
                            }
                        }
                        if (bOk == false)
                        {
                            gridView.DeleteRow(i);
                        }
                    }

                    objProduct = null;
                    
                }

            }
            catch (System.Exception f)
            {
                SendMessageToLog("SetFilterForPartsCombo. Текст ошибки: " + f.Message);
            }
            finally
            {
                //this.tableLayoutPanelBackground.ResumeLayout( false );
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
                m_objPartsList = objPartsList;

                System.Data.DataRow newRowProduct = null;
                dataSet.Tables["OrderItems"].Clear();
                dataSet.Tables["Product"].Clear();
                foreach (CProduct objItem in m_objPartsList)
                {
                    newRowProduct = dataSet.Tables["Product"].NewRow();
                    newRowProduct["ProductID"] = objItem.ID;
                    newRowProduct["Product_MeasureID"] = objItem.Measure.ID;
                    newRowProduct["Product_MeasureName"] = objItem.Measure.ShortName;
                    newRowProduct["ProductFullName"] = objItem.ProductFullName;
                    newRowProduct["CustomerOrderStockQty"] = objItem.CustomerOrderStockQty;
                    newRowProduct["CustomerOrderResQty"] = objItem.CustomerOrderResQty;
                    newRowProduct["CustomerOrderPackQty"] = objItem.CustomerOrderPackQty;
                    
                    dataSet.Tables["Product"].Rows.Add( newRowProduct );
                }
                newRowProduct = null;
                dataSet.Tables["Product"].AcceptChanges();


                //repItemComboProduct.Items.Clear();
                //repItemComboProduct.Items.AddRange(m_objPartsList);
                //colProduct.ColumnEdit = repItemComboProduct;
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
                m_objPartsList = COrderRepository.GetPartsInstockList(m_objProfile, null, m_uuidSelectedStockID, System.Guid.Empty);
                if (m_objPartsList != null)
                {
                    this.Invoke(m_SetProductListToFormDelegate, new Object[] { m_objPartsList });
                }
            }
            catch (System.Exception f)
            {
                this.Invoke(m_SendMessageToLogDelegate, new Object[] { ("Ошибка обновления списка товаров. Текст ошибки: " + f.Message) });
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
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo("ru-RU");
                ci.NumberFormat.CurrencyDecimalSeparator = ".";
                ci.NumberFormat.NumberDecimalSeparator = ".";
                
                // делаем событиям reset
                this.m_EventStopThread.Reset();
                this.m_EventThreadStopped.Reset();

                this.thrAddress = new System.Threading.Thread(WorkerThreadFunction);
                this.thrAddress.CurrentCulture = ci;
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

        #region Режим просмотра/редактирования
        /// <summary>
        /// Устанавливает режим просмотра/редактирования
        /// </summary>
        /// <param name="bSet">true - режим просмотра; false - режим редактирования</param>
        private void SetModeReadOnly(System.Boolean bSet)
        {
            try
            {
                BeginDate.Properties.ReadOnly = bSet;
                DeliveryDate.Properties.ReadOnly = bSet;
                txtDescription.Properties.ReadOnly = bSet;

                Customer.Properties.ReadOnly = bSet;
                ChildDepart.Properties.ReadOnly = bSet;
                PaymentType.Properties.ReadOnly = bSet;
                OrderType.Properties.ReadOnly = bSet;
                IsBonus.Properties.ReadOnly = bSet;
                Rtt.Properties.ReadOnly = bSet;
                AddressDelivery.Properties.ReadOnly = bSet;
                Depart.Properties.ReadOnly = bSet;
                SalesMan.Properties.ReadOnly = bSet;
                Stock.Properties.ReadOnly = bSet;
                Stmnt.Properties.ReadOnly = bSet;
                

                cboxProductTradeMark.Properties.ReadOnly = bSet;
                cboxProductType.Properties.ReadOnly = bSet;
                spinEditDiscount.Properties.ReadOnly = bSet;

                checkSetOrderInQueue.Enabled = !bSet;

                gridView.OptionsBehavior.Editable = !bSet;
                controlNavigator.Enabled = !bSet; 
                if( m_bIsAutoCreatePriceMode == false )
                { 
                    btnSetDiscount.Enabled = !bSet;
                    checkEditCalcPrices.Enabled = !bSet;
                }

                mitemImport.Enabled = !bSet;

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

        #region Редактировать заказ
        /// <summary>
        /// очистка содержимого элементов управления
        /// </summary>
        private void ClearControls()
        {
            try
            {

                BeginDate.EditValue = null;
                DeliveryDate.EditValue = null;
                txtDescription.Text = "";

                Customer.SelectedItem = null;
                ChildDepart.SelectedItem = null;
                PaymentType.SelectedItem = null;
                OrderType.SelectedItem = null;
                IsBonus.CheckState = CheckState.Unchecked;
                Rtt.SelectedItem = null;
                AddressDelivery.SelectedItem = null;
                Depart.SelectedItem = null;
                SalesMan.SelectedItem = null;
                Stock.SelectedItem = null;
                cboxProductTradeMark.SelectedItem = null;
                cboxProductType.SelectedItem = null;
                spinEditDiscount.Value = 0;
                Stmnt.SelectedItem = null;

                dataSet.Tables["OrderItems"].Clear();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("ClearControls. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// Загружает свойства заказа для редактирования
        /// </summary>
        /// <param name="objOrder">заказ</param>
        /// <param name="bNewObject">признак "новый заказ"</param>
        /// <param name="bOnlyShowMode">признак "отобразить только для просмотра"</param>
        public void EditOrder(COrder objOrder, System.Boolean bNewObject, System.Boolean bOnlyShowMode = false)
        {
            if (objOrder == null) { return; }
            m_bDisableEvents = true;
            m_bNewObject = bNewObject;
            SetCreatePriceMode();
            try
            {
                m_objSelectedOrder = objOrder;
                if (m_objSelectedOrder.ChildDepart != null)
                {
                    m_objSelectedOrder.ChildDepart = CChildDepart.GetChildDepart(m_objProfile, null, m_objSelectedOrder.ChildDepart.ID);
                }

                this.tableLayoutPanelBackground.SuspendLayout();

                ClearControls();
                

                BeginDate.DateTime = m_objSelectedOrder.BeginDate;
                DeliveryDate.DateTime = m_objSelectedOrder.DeliveryDate;
                txtDescription.Text = m_objSelectedOrder.Description;

                Customer.SelectedItem = (m_objSelectedOrder.Customer == null) ? null : Customer.Properties.Items.Cast<CCustomer>().Single<CCustomer>(x => x.ID.CompareTo(m_objSelectedOrder.Customer.ID) == 0);

                AddressDelivery.Properties.Items.Clear();

                LoadRttListForCustomer(m_objSelectedOrder.Customer.ID);
                LoadChildDeprtForCustomer(m_objSelectedOrder.Customer.ID);
                System.String strErr = "";
                m_objSelectedOrder.Customer.PhoneList = CPhone.GetPhoneListForCustomer(m_objProfile, null,
                    m_objSelectedOrder.Customer.ID, ref strErr);


                Rtt.SelectedItem = (m_objSelectedOrder.Rtt == null) ? null : Rtt.Properties.Items.Cast<CRtt>().Single<CRtt>(x => x.ID.CompareTo(m_objSelectedOrder.Rtt.ID) == 0);
                if (Rtt.SelectedItem != null)
                {
                    AddressDelivery.Properties.Items.AddRange((((CRtt)Rtt.SelectedItem).AddressList));
                }

                ChildDepart.SelectedItem = (m_objSelectedOrder.ChildDepart == null) ? null : ChildDepart.Properties.Items.Cast<CChildDepart>().SingleOrDefault<CChildDepart>(x => x.ID.CompareTo(m_objSelectedOrder.ChildDepart.ID) == 0);
                PaymentType.SelectedItem = (m_objSelectedOrder.PaymentType == null) ? null : PaymentType.Properties.Items.Cast<CPaymentType>().SingleOrDefault<CPaymentType>(x => x.ID.CompareTo(m_objSelectedOrder.PaymentType.ID) == 0);
                OrderType.SelectedItem = (m_objSelectedOrder.OrderType == null) ? null : OrderType.Properties.Items.Cast<COrderType>().SingleOrDefault<COrderType>(x => x.Id.CompareTo(m_objSelectedOrder.OrderType.Id) == 0);
                IsBonus.CheckState = ( (m_objSelectedOrder.IsBonus == true) ? CheckState.Checked : CheckState.Unchecked );
                AddressDelivery.SelectedItem = (m_objSelectedOrder.AddressDelivery == null) ? null : AddressDelivery.Properties.Items.Cast<CAddress>().SingleOrDefault<CAddress>(x => x.ID.CompareTo(m_objSelectedOrder.AddressDelivery.ID) == 0);
                Depart.SelectedItem = (m_objSelectedOrder.Depart == null) ? null : Depart.Properties.Items.Cast<CDepart>().SingleOrDefault<CDepart>(x => x.uuidID.CompareTo(m_objSelectedOrder.Depart.uuidID) == 0);
                
                LoadSalesManListForDepart((CDepart)Depart.SelectedItem);

                SalesMan.SelectedItem = (m_objSelectedOrder.SalesMan == null) ? null : SalesMan.Properties.Items.Cast<CSalesMan>().SingleOrDefault<CSalesMan>(x => x.uuidID.CompareTo(m_objSelectedOrder.SalesMan.uuidID) == 0);
                Stock.SelectedItem = (m_objSelectedOrder.Stock == null) ? null : Stock.Properties.Items.Cast<CStock>().SingleOrDefault<CStock>(x => x.ID.CompareTo(m_objSelectedOrder.Stock.ID) == 0);

                if (m_objSelectedOrder.ID.CompareTo(System.Guid.Empty) == 0)
                {
                    if ((Stock.SelectedItem != null) && (Customer.SelectedItem != null))
                    {
                        LoadStmntForCustomerCompany(((CCustomer)Customer.SelectedItem).ID, ((CStock)Stock.SelectedItem).Company.ID);
                    }
                }
                else
                {
                    List<CStmnt> objStmntList = CStmnt.GetStmntList(m_objProfile, null, System.Guid.Empty, System.Guid.Empty, m_objSelectedOrder.ID);
                    if ((objStmntList != null) && (objStmntList.Count > 0))
                    {
                        if ((Stock.SelectedItem != null) && (Customer.SelectedItem != null))
                        {
                            LoadStmntForCustomerCompany(((CCustomer)Customer.SelectedItem).ID, ((CStock)Stock.SelectedItem).Company.ID);
                        }
                        for (System.Int32 i = 0; i < Stmnt.Properties.Items.Count; i++)
                        {
                            if (((CStmnt)Stmnt.Properties.Items[i]).ID.CompareTo(objStmntList[0].ID) == 0)
                            {
                                Stmnt.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                    objStmntList = null;
                }
                cboxProductTradeMark.Properties.Items.Add(new CProductTradeMark() { ID = System.Guid.Empty, Name = "" });
                cboxProductTradeMark.Properties.Items.AddRange( CProductTradeMark.GetProductTradeMarkList(m_objProfile, null ) );
                cboxProductType.Properties.Items.Add(new CProductType() { ID = System.Guid.Empty, Name = "" });
                cboxProductType.Properties.Items.AddRange(CProductType.GetProductTypeList(m_objProfile, null));

                if (Stock.SelectedItem != null)
                {
                    m_uuidSelectedStockID = ((CStock)Stock.SelectedItem).ID;
                    m_objPartsList = COrderRepository.GetPartsInstockList(m_objProfile, null, m_uuidSelectedStockID, m_objSelectedOrder.ID);
                    if (m_objPartsList != null)
                    {
                        SetProductListToForm(m_objPartsList);
                    }
                }

                dataSet.Tables["OrderItems"].Clear();
                System.Data.DataRow newRowOrderItems = null;
                CProduct objProduct = null;
                foreach (COrderItem objItem in m_objSelectedOrder.OrderItemList)
                {
                    newRowOrderItems = dataSet.Tables["OrderItems"].NewRow();

                    newRowOrderItems["OrderItemsID"] = objItem.ID;
                    newRowOrderItems["ProductID"] = objItem.Product.ID;
                    newRowOrderItems["MeasureID"] = objItem.Measure.ID;
                    newRowOrderItems["OrderedQuantity"] = objItem.QuantityOrdered;
                    newRowOrderItems["QuantityReserved"] = objItem.QuantityReserved;

                    if (m_objPartsList != null)
                    {
                        try
                        {
                            objProduct = m_objPartsList.Single<CProduct>(x => x.ID.CompareTo(objItem.Product.ID) == 0);
                        }
                        catch
                        {
                            objProduct = null;
                        }
                        if( objProduct != null )
                        {
                            newRowOrderItems["OrderPackQty"] = objProduct.CustomerOrderMinRetailQty;
                            newRowOrderItems["OrderItems_QuantityInstock"] = objProduct.CustomerOrderStockQty;
                        }
                    }

                    newRowOrderItems["OrderItems_MeasureName"] = objItem.Measure.ShortName;
                    newRowOrderItems["OrderItems_PartsName"] = objItem.Product.Name;
                    newRowOrderItems["OrderItems_PartsArticle"] = objItem.Product.Article;
                    newRowOrderItems["PriceImporter"] = objItem.PriceImporter;
                    newRowOrderItems["Price"] = objItem.Price;

                    if (m_bNewObject == false)
                    {
                        newRowOrderItems["DiscountPercent"] = objItem.DiscountPercent;
                    }
                    else
                    {
                        if( ( m_bNewObject == true ) &&  (m_bIsAutoCreatePriceMode == false) )
                        {
                            newRowOrderItems["DiscountPercent"] = objItem.DiscountPercent;
                        }
                        else
                        {
                            newRowOrderItems["DiscountPercent"] = 0;
                        }
                    }
                    newRowOrderItems["PriceWithDiscount"] = objItem.PriceWithDiscount;
                    newRowOrderItems["NDSPercent"] = objItem.NDSPercent;
                    newRowOrderItems["PriceInAccountingCurrency"] = objItem.PriceInAccountingCurrency;
                    newRowOrderItems["PriceWithDiscountInAccountingCurrency"] = objItem.PriceWithDiscountInAccountingCurrency;

                    dataSet.Tables["OrderItems"].Rows.Add(newRowOrderItems);
                }
                newRowOrderItems = null;
                objProduct = null;
                dataSet.Tables["OrderItems"].AcceptChanges();

                ReloadDscrpnByAddress();

                SetPropertiesModified(false);
                btnCancel.Enabled = true;
                btnCancel.Focus();
                ValidateProperties();

                SetModeReadOnly(true);

                btnEdit.Visible = (bOnlyShowMode == false);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования заказа клиенту. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.tableLayoutPanelBackground.ResumeLayout(false);
                m_bDisableEvents = false;

                GetCustomerDebtInfo(true);
            }
            return;
        }
        #endregion

        #region Копирование заказа
        /// <summary>
        /// Копирует свойства заказа в новый заказ и открывает его для редактирования
        /// </summary>
        /// <param name="objOrder">заказ</param>
        public void CopyOrder(COrder objOrder)
        {
            if (objOrder == null) { return; }

            try
            {
                SetCreatePriceMode();

                m_objSelectedOrder = new COrder();
                m_objSelectedOrder.BeginDate = System.DateTime.Today;
                m_objSelectedOrder.DeliveryDate = System.DateTime.Today;
                m_objSelectedOrder.Description = objOrder.Description;
                m_objSelectedOrder.Customer = objOrder.Customer;
                m_objSelectedOrder.Rtt = objOrder.Rtt;
                m_objSelectedOrder.ChildDepart = objOrder.ChildDepart;
                m_objSelectedOrder.PaymentType = objOrder.PaymentType;
                m_objSelectedOrder.OrderType = objOrder.OrderType;
                m_objSelectedOrder.IsBonus = objOrder.IsBonus;
                m_objSelectedOrder.Depart = objOrder.Depart;
                m_objSelectedOrder.SalesMan = objOrder.SalesMan;
                m_objSelectedOrder.Stock = objOrder.Stock;

                m_objSelectedOrder.AddressDelivery = new CAddress() { ID = objOrder.AddressDelivery.ID };
                CAddress.Init(m_objProfile, null, m_objSelectedOrder.AddressDelivery, m_objSelectedOrder.AddressDelivery.ID);

                m_objSelectedOrder.OrderItemList = COrderRepository.GetOrderItemList(m_objProfile, objOrder.ID);

                EditOrder(m_objSelectedOrder, true);

                if (m_objSelectedOrder.PaymentType != null)
                {
                    for (System.Int32 i = 0; i < gridView.RowCount; i++)
                    {
                        SetPriceInRow(i, (System.Guid)gridView.GetRowCellValue(i, colProductID), m_objSelectedOrder.Stock.ID,
                            m_objSelectedOrder.PaymentType.ID, System.Convert.ToDouble(gridView.GetRowCellValue(i, colDiscountPercent)));
                    }
                }
                    
                SetModeReadOnly(false);
                gridControl.RefreshDataSource();
                gridView.RefreshData();
                
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка копирования заказа клиенту. Текст ошибки: " + f.Message);
            }
            finally
            {
                m_bNewObject = true;
                m_objSelectedOrder.ID = System.Guid.Empty;
                btnSave.Enabled = (ValidateProperties() == true);
            }
            return;
        }

        #endregion

        #region Новый заказ
        /// <summary>
        /// Новый заказ
        /// </summary>
        public void NewOrder(CCustomer objCustomer, CCompany objCompany, CStock objStock, CPaymentType objPaymentType)
        {
            try
            {
                m_bNewObject = true;
                m_bDisableEvents = true;
                SetCreatePriceMode();

                m_objSelectedOrder = new COrder() { 
                    BeginDate = System.DateTime.Today /*  BeginDate.DateTime; // System.DateTime.Today;*/, 
                    DeliveryDate = System.DateTime.Today.AddDays(1) /* DeliveryDate.DateTime; // System.DateTime.Today;*/, 
                    OrderItemList = new List<COrderItem>(), 
                    Customer = objCustomer, 
                    PaymentType = objPaymentType };
                if (objCompany != null)
                {
                    if (Stock.Properties.Items.Count > 0)
                    {
                        for (System.Int32 i = 0; i < Stock.Properties.Items.Count; i++)
                        {
                            if (((CStock)Stock.Properties.Items[i]).Company.ID == objCompany.ID)
                            {
                                m_objSelectedOrder.Stock = (CStock)Stock.Properties.Items[i];
                                break;
                            }
                        }
                    }
                }

                if (objStock != null)
                {
                    if (Stock.Properties.Items.Count > 0)
                    {
                        for (System.Int32 i = 0; i < Stock.Properties.Items.Count; i++)
                        {
                            if (((CStock)Stock.Properties.Items[i]).ID == objStock.ID)
                            {
                                m_objSelectedOrder.Stock = (CStock)Stock.Properties.Items[i];
                                break;
                            }
                        }
                    }
                }

                this.tableLayoutPanelBackground.SuspendLayout();

                ClearControls();

                if (m_objSelectedOrder.Customer != null)
                {
                    LoadRttListForCustomer(m_objSelectedOrder.Customer.ID);
                    if (Rtt.Properties.Items.Count > 0)
                    {
                        m_objSelectedOrder.Rtt = (CRtt)Rtt.Properties.Items[0];
                        AddressDelivery.Properties.Items.Clear();
                        if (Rtt.SelectedItem != null)
                        {
                            AddressDelivery.Properties.Items.AddRange((((CRtt)Rtt.SelectedItem).AddressList));
                            AddressDelivery.SelectedItem = ((AddressDelivery.Properties.Items.Count == 0) ? null : AddressDelivery.Properties.Items[0]);
                            m_objSelectedOrder.AddressDelivery = (CAddress)AddressDelivery.SelectedItem;
                        }
                    }
                    LoadChildDeprtForCustomer(m_objSelectedOrder.Customer.ID);

                    System.String strErr = "";
                    m_objSelectedOrder.Customer.PhoneList = CPhone.GetPhoneListForCustomer(m_objProfile, null,
                        m_objSelectedOrder.Customer.ID, ref strErr);
                }

                BeginDate.DateTime = m_objSelectedOrder.BeginDate;
                DeliveryDate.DateTime = m_objSelectedOrder.DeliveryDate;
                txtDescription.Text = m_objSelectedOrder.Description;

                Customer.SelectedItem = (m_objSelectedOrder.Customer == null) ? null : Customer.Properties.Items.Cast<CCustomer>().Single<CCustomer>(x => x.ID.CompareTo(m_objSelectedOrder.Customer.ID) == 0);
                ChildDepart.SelectedItem = (m_objSelectedOrder.ChildDepart == null) ? null : ChildDepart.Properties.Items.Cast<CChildDepart>().Single<CChildDepart>(x => x.ID.CompareTo(m_objSelectedOrder.ChildDepart.ID) == 0);
                PaymentType.SelectedItem = (m_objSelectedOrder.PaymentType == null) ? ((PaymentType.Properties.Items.Count > 0) ? PaymentType.Properties.Items[0] : null) : (PaymentType.Properties.Items.Cast<CPaymentType>().Single<CPaymentType>(x=>x.ID == m_objSelectedOrder.PaymentType.ID));
                OrderType.SelectedItem = (OrderType.Properties.Items.Count > 0) ? OrderType.Properties.Items[0] : null;
                IsBonus.CheckState = ((m_objSelectedOrder.IsBonus == true) ? CheckState.Checked : CheckState.Unchecked);
                Rtt.SelectedItem = (m_objSelectedOrder.Rtt == null) ? null : Rtt.Properties.Items.Cast<CRtt>().Single<CRtt>(x => x.ID.CompareTo(m_objSelectedOrder.Rtt.ID) == 0);
                AddressDelivery.SelectedItem = (m_objSelectedOrder.AddressDelivery == null) ? null : AddressDelivery.Properties.Items.Cast<CAddress>().Single<CAddress>(x => x.ID.CompareTo(m_objSelectedOrder.AddressDelivery.ID) == 0);
                Depart.SelectedItem = (m_objSelectedOrder.Depart == null) ? null : Depart.Properties.Items.Cast<CDepart>().Single<CDepart>(x => x.uuidID.CompareTo(m_objSelectedOrder.Depart.uuidID) == 0);
                SalesMan.SelectedItem = (m_objSelectedOrder.SalesMan == null) ? null : SalesMan.Properties.Items.Cast<CSalesMan>().Single<CSalesMan>(x => x.ID.CompareTo(m_objSelectedOrder.SalesMan.ID) == 0);
                Stock.SelectedItem = (m_objSelectedOrder.Stock == null) ? null : Stock.Properties.Items.Cast<CStock>().Single<CStock>(x => x.ID.CompareTo(m_objSelectedOrder.Stock.ID) == 0);

                ReloadDscrpnByAddress();

                cboxProductTradeMark.Properties.Items.Add(new CProductTradeMark() { ID = System.Guid.Empty, Name = "" });
                cboxProductTradeMark.Properties.Items.AddRange(CProductTradeMark.GetProductTradeMarkList(m_objProfile, null));
                cboxProductType.Properties.Items.Add(new CProductType() { ID = System.Guid.Empty, Name = "" });
                cboxProductType.Properties.Items.AddRange(CProductType.GetProductTypeList(m_objProfile, null));


                if (Stock.SelectedItem != null)
                {
                    m_uuidSelectedStockID = ((CStock)Stock.SelectedItem).ID;
                    m_objPartsList = COrderRepository.GetPartsInstockList(m_objProfile, null, m_uuidSelectedStockID, System.Guid.Empty);
                    if (m_objPartsList != null)
                    {
                        SetProductListToForm(m_objPartsList);
                    }
                }

                if ((Stock.SelectedItem != null) && (Customer.SelectedItem != null))
                {
                    LoadStmntForCustomerCompany(((CCustomer)Customer.SelectedItem).ID, ((CStock)Stock.SelectedItem).Company.ID);
                }

                btnEdit.Enabled = false;
                btnCancel.Enabled = true;
                btnCancel.Focus();

                SetModeReadOnly(false);

                SetDefParamForOrder();

                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка создания заказа. Текст ошибки: " + f.Message);
            }
            finally
            {
                tableLayoutPanelBackground.ResumeLayout(false);
                m_bDisableEvents = false;

                GetCustomerDebtInfo(true);
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
                SimulateChangeOrderProperties(m_objSelectedOrder, enumActionSaveCancel.Cancel, m_bNewObject);
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
        #endregion

        #region Сохранить изменения
        /// <summary>
        /// Сохраняет изменения в базе данных
        /// </summary>
        /// <returns>true - удачное завершение операции;false - ошибка</returns>
        private System.Boolean bSaveChanges(ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Boolean bOkSave = false;
            Cursor = Cursors.WaitCursor;
            try
            {
                
                System.DateTime Order_BeginDate = BeginDate.DateTime;
                System.Guid OrderState_Guid = ( ( m_objSelectedOrder.OrderState == null ) ? System.Guid.Empty : m_objSelectedOrder.OrderState.ID ); 
                System.Boolean Order_MoneyBonus = ( this.IsBonus.CheckState == CheckState.Checked ) ;
                System.Guid Depart_Guid = ( ( Depart.SelectedItem == null ) ? (System.Guid.Empty) : ( (CDepart)Depart.SelectedItem ).uuidID );
                System.Guid Salesman_Guid = ((SalesMan.SelectedItem == null) ? (System.Guid.Empty) : ((CSalesMan)SalesMan.SelectedItem).uuidID);
                System.Guid Customer_Guid = ( ( Customer.SelectedItem == null ) ? (System.Guid.Empty) : ( (CCustomer)Customer.SelectedItem ).ID );
                System.Guid CustomerChild_Guid = ( ( ChildDepart.SelectedItem == null ) ? (System.Guid.Empty) : ( (CChildDepart)ChildDepart.SelectedItem ).ID );
                System.Guid OrderType_Guid = ( ( OrderType.SelectedItem == null ) ? (System.Guid.Empty) : ( (COrderType)OrderType.SelectedItem ).Id ); 
                System.Guid PaymentType_Guid = ( ( PaymentType.SelectedItem == null ) ? (System.Guid.Empty) : ( (CPaymentType)PaymentType.SelectedItem ).ID ); 
                System.String Order_Description = txtDescription.Text; 
                System.DateTime Order_DeliveryDate = DeliveryDate.DateTime; 
                System.Guid Rtt_Guid = ( ( Rtt.SelectedItem == null ) ? (System.Guid.Empty) : ( (CRtt)Rtt.SelectedItem ).ID );  
                System.Guid Address_Guid = ( ( AddressDelivery.SelectedItem == null ) ? (System.Guid.Empty) : ( (CAddress)AddressDelivery.SelectedItem ).ID );  
                System.Guid Stock_Guid = ( ( Stock.SelectedItem == null ) ? (System.Guid.Empty) : ( (CStock)Stock.SelectedItem ).ID );  
                System.Guid Parts_Guid = System.Guid.Empty;
                System.Guid Stmnt_Guid = ((Stmnt.SelectedItem == null) ? (System.Guid.Empty) : ((CStmnt)Stmnt.SelectedItem).ID);  

                List<COrderItem> objOrderItemList = new List<COrderItem>();
                System.Guid uuidOrderItmsID = System.Guid.Empty;
                System.Guid uuidProductID = System.Guid.Empty;
                System.Guid uuidMeasureID = System.Guid.Empty;
                System.Double dblQuantityOrdered = 0;

                System.Double dblQuantityReserved = 0;
                System.Double dblPriceImporter = 0;
                System.Double dblPrice = 0;
                System.Double dblDiscountPercent = 0;
                System.Double dblPriceWithDiscount = 0;
                System.Double dblNDSPercent = 0;
                System.Double dblPriceInAccountingCurrency = 0;
                System.Double dblPriceWithDiscountInAccountingCurrency = 0;

                dataSet.Tables["OrderItems"].AcceptChanges();

                for (System.Int32 i = 0; i < dataSet.Tables["OrderItems"].Rows.Count; i++)
                {
                    if( (dataSet.Tables["OrderItems"].Rows[i]["ProductID"] == System.DBNull.Value ) || 
                        (dataSet.Tables["OrderItems"].Rows[i]["MeasureID"] == System.DBNull.Value ) ||
                        (dataSet.Tables["OrderItems"].Rows[i]["OrderedQuantity"] == System.DBNull.Value))
                    {
                        continue;
                    }
                    if (m_bNewObject == true)
                    {
                        uuidOrderItmsID = System.Guid.NewGuid();
                    }
                    else
                    {
                        uuidOrderItmsID = ((dataSet.Tables["OrderItems"].Rows[i]["OrderItemsID"] == System.DBNull.Value) ? System.Guid.NewGuid() : (System.Guid)(dataSet.Tables["OrderItems"].Rows[i]["OrderItemsID"]));
                    }
                    uuidProductID = (System.Guid)(dataSet.Tables["OrderItems"].Rows[i]["ProductID"]);
                    uuidMeasureID = (System.Guid)(dataSet.Tables["OrderItems"].Rows[i]["MeasureID"]);
                    dblQuantityOrdered = System.Convert.ToDouble( dataSet.Tables["OrderItems"].Rows[i]["OrderedQuantity"] );
                    dblQuantityReserved = dblQuantityOrdered;
                    dblPriceImporter = System.Convert.ToDouble(dataSet.Tables["OrderItems"].Rows[i]["PriceImporter"]);
                    dblPrice = System.Convert.ToDouble(dataSet.Tables["OrderItems"].Rows[i]["Price"]);
                    dblDiscountPercent = System.Convert.ToDouble(dataSet.Tables["OrderItems"].Rows[i]["DiscountPercent"]);
                    dblPriceWithDiscount = System.Convert.ToDouble(dataSet.Tables["OrderItems"].Rows[i]["PriceWithDiscount"]);
                    dblNDSPercent = System.Convert.ToDouble(dataSet.Tables["OrderItems"].Rows[i]["NDSPercent"]);
                    dblPriceInAccountingCurrency = System.Convert.ToDouble(dataSet.Tables["OrderItems"].Rows[i]["PriceInAccountingCurrency"]);
                    dblPriceWithDiscountInAccountingCurrency = System.Convert.ToDouble(dataSet.Tables["OrderItems"].Rows[i]["PriceWithDiscountInAccountingCurrency"]);

                    objOrderItemList.Add(new COrderItem() { ID = uuidOrderItmsID,  Product = new CProduct() { ID = uuidProductID }, Measure = new CMeasure() { ID = uuidMeasureID }, 
                        QuantityOrdered = dblQuantityOrdered, QuantityReserved = dblQuantityReserved, 
                        PriceImporter = dblPriceImporter, Price = dblPrice, DiscountPercent = dblDiscountPercent, 
                        NDSPercent = dblNDSPercent, PriceWithDiscount = dblPriceWithDiscount, 
                        PriceInAccountingCurrency = dblPriceInAccountingCurrency, 
                        PriceWithDiscountInAccountingCurrency = dblPriceWithDiscountInAccountingCurrency });
                }
                System.Data.DataTable addedCategories = ((gridControl.DataSource == null) ? null : COrderItem.ConvertListToTable( objOrderItemList, ref strErr ) );
                objOrderItemList = null;

                // проверка значений
                if (COrderRepository.CheckAllPropertiesForSave(Order_BeginDate, Depart_Guid, Salesman_Guid, Customer_Guid,
                    OrderType_Guid, PaymentType_Guid, Order_DeliveryDate, Rtt_Guid, Address_Guid, addedCategories, ref strErr) == true)
                {
                    if (m_bNewObject == true)
                    {
                        // новый заказ
                        System.Guid uuidOrderId = System.Guid.Empty;
                        System.Int32 iSupplId = 0;
                        bOkSave = COrderRepository.AddNewOrderToDB(m_objProfile, null, Order_BeginDate, OrderState_Guid,
                            Order_MoneyBonus, Depart_Guid, Salesman_Guid, Customer_Guid, CustomerChild_Guid, OrderType_Guid,
                            PaymentType_Guid, Order_Description, Order_DeliveryDate, Rtt_Guid, Address_Guid, Stock_Guid,
                            Parts_Guid, addedCategories, checkEditCalcPrices.Checked, Stmnt_Guid, ref uuidOrderId, ref iSupplId, ref strErr, checkSetOrderInQueue.Checked);
                        if (bOkSave == true) 
                        { 
                            m_objSelectedOrder.ID = uuidOrderId;
                            m_objSelectedOrder.Ib_ID = iSupplId;
                        }
                    }
                    else
                    {
                        bOkSave = COrderRepository.EditOrderInDB(m_objProfile, null, Order_BeginDate, OrderState_Guid,
                            Order_MoneyBonus, Depart_Guid, Salesman_Guid, Customer_Guid, CustomerChild_Guid, OrderType_Guid,
                            PaymentType_Guid, Order_Description, Order_DeliveryDate, Rtt_Guid, Address_Guid, Stock_Guid,
                            Parts_Guid, addedCategories, m_objSelectedOrder.ID, ref strErr);
                    }
                }

                if (bOkSave == true)
                {
                    m_objSelectedOrder.BeginDate = Order_BeginDate;
                    m_objSelectedOrder.IsBonus = Order_MoneyBonus;
                    m_objSelectedOrder.Depart = ((Depart.SelectedItem == null) ? null : (CDepart)Depart.SelectedItem);
                    m_objSelectedOrder.SalesMan = ((SalesMan.SelectedItem == null) ? null : (CSalesMan)SalesMan.SelectedItem);
                    m_objSelectedOrder.Customer = ((Customer.SelectedItem == null) ? null : (CCustomer)Customer.SelectedItem);
                    m_objSelectedOrder.ChildDepart = ((ChildDepart.SelectedItem == null) ? null : (CChildDepart)ChildDepart.SelectedItem);
                    m_objSelectedOrder.OrderType = ((OrderType.SelectedItem == null) ? null : (COrderType)OrderType.SelectedItem);
                    m_objSelectedOrder.PaymentType = ((PaymentType.SelectedItem == null) ? null : (CPaymentType)PaymentType.SelectedItem);
                    m_objSelectedOrder.Description = txtDescription.Text;
                    m_objSelectedOrder.DeliveryDate = DeliveryDate.DateTime;
                    m_objSelectedOrder.Rtt = ((Rtt.SelectedItem == null) ? null : (CRtt)Rtt.SelectedItem);
                    m_objSelectedOrder.AddressDelivery = ((AddressDelivery.SelectedItem == null) ? null : (CAddress)AddressDelivery.SelectedItem);
                    m_objSelectedOrder.Stock = ((Stock.SelectedItem == null) ? null : (CStock)Stock.SelectedItem);
                }

                bRet = bOkSave;
            }
            catch (System.Exception f)
            {
                strErr = f.Message; 
                SendMessageToLog("Ошибка сохранения изменений в заказе. Текст ошибки: " + f.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            return bRet;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkSetOrderInQueue.Checked == true)
                {
                    if( DevExpress.XtraEditors.XtraMessageBox.Show("В процессе сохранения заказа будет производиться резерв товара.\nПроцедура может занять длительное время.\nПодтвердите, пожалуйста, начало операции.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Information) != DialogResult.Yes )
                    {
                        return;
                    }
                }

                System.String strErr = "";
                if (bSaveChanges( ref strErr ) == true)
                {
                    SimulateChangeOrderProperties( m_objSelectedOrder, enumActionSaveCancel.Save, m_bNewObject);
                    System.String strInfo = ( ( checkSetOrderInQueue.Checked == true ) ? "Заказ добавлен в очередь на автоматическую обработку.\nСостояние заказа: определены склады." : "Заказ успешно обработан: сформирован резерв товара.");

                    DevExpress.XtraEditors.XtraMessageBox.Show(strInfo, "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                }
                else
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show( strErr, "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning );

                }

            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в описании ПСЦ. Текст ошибки: " + f.Message);
            }
            return;
        }

        #endregion

        #region запись в DBGrid

        private void gridView_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (gridView.FocusedRowHandle >= 0)
                    {
                        if (IsValidDataInRow(gridView.FocusedRowHandle) == true)
                        {
                            if (gridView.FocusedRowHandle == (gridView.RowCount - 1))
                            {
                                gridView.AddNewRow();
                                gridView.FocusedColumn = colProductID;
                                e.Handled = true;
                            }
                        }
                    }
                    else
                    {
                        gridView.AddNewRow();
                        gridView.FocusedColumn = colProductID;
                        e.Handled = true;
                    }
                }
                else if (e.KeyCode == Keys.Up)
                {
                    if (gridView.GetDataRow(gridView.FocusedRowHandle)["ProductID"] == System.DBNull.Value)
                    {
                        gridView.CancelUpdateCurrentRow();
                        e.Handled = true;
                    }
                }
                else if (e.KeyCode == Keys.Delete && e.Control)
                {
                    gridView.DeleteSelectedRows();
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("gridView_KeyDown. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;

        }

        private void gridView_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            try
            {
                DataRow row = gridView.GetDataRow(e.RowHandle);

                //row["OrderItemsID"] = System.Guid.NewGuid();
                row["OrderedQuantity"] = 1;
                row["QuantityReserved"] = 1;
                row["PriceImporter"] = 0;
                row["Price"] = 0;
                row["DiscountPercent"] = spinEditDiscount.Value;
                row["PriceWithDiscount"] = 0;
                row["NDSPercent"] = 0;
                row["PriceInAccountingCurrency"] = 0;
                row["PriceWithDiscountInAccountingCurrency"] = 0;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("gridView_InitNewRow. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private void gridView_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            try
            {
                e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("gridView_InvalidRowException. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private void gridView_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                System.Boolean bOK = true;
                if ((gridView.GetDataRow(e.RowHandle)["OrderedQuantity"] == System.DBNull.Value) ||
                    (System.Convert.ToDouble(gridView.GetDataRow(e.RowHandle)["OrderedQuantity"]) < 1))
                {
                    bOK = false;
                    gridView.SetColumnError(colOrderedQuantity, "недопустимое количество", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning);
                }
                if (gridView.GetDataRow(e.RowHandle)["ProductID"] == System.DBNull.Value)
                {
                    bOK = false;
                    gridView.SetColumnError(colProductID, "укажите, пожалуйста, товар", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning);
                }
                if (gridView.GetDataRow(e.RowHandle)["MeasureID"] == System.DBNull.Value)
                {
                    bOK = false;
                    gridView.SetColumnError(colOrderItems_MeasureName, "укажите, пожалуйста, единицу измерения", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Warning);
                }
                e.Valid = bOK;

            }
            catch (System.Exception f)
            {
                SendMessageToLog("gridView_ValidateRow. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private System.Boolean IsValidDataInRow(System.Int32 iRowHandle)
        {
            System.Boolean bRet = true;
            try
            {
                if ((iRowHandle < 0) || (gridView.RowCount < iRowHandle)) { return bRet; }
                else
                {
                    if ((gridView.GetDataRow(iRowHandle)["OrderedQuantity"] == System.DBNull.Value) ||
                        (System.Convert.ToDouble(gridView.GetDataRow(iRowHandle)["OrderedQuantity"]) < 1))
                    {
                        bRet = false;
                    }
                    if (gridView.GetDataRow(iRowHandle)["ProductID"] == System.DBNull.Value)
                    {
                        bRet = false;
                    }
                    if (gridView.GetDataRow(iRowHandle)["MeasureID"] == System.DBNull.Value)
                    {
                        bRet = false;
                    }
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("IsValidDataInRow. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return bRet;

        }

        private void gridView_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("gridView_CellValueChanging. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        #endregion

        #region Экспорт приложения к заказу
        //<sbExportToHTML>
        private void sbExportToHTML_Click(object sender, System.EventArgs e)
        {
            try
            {
                panelProgressBar.Visible = true;

                string fileName = ShowSaveFileDialog("HTML документ", "HTML Documents|*.html");
                if (fileName != "")
                {
                    ExportTo(new DevExpress.XtraExport.ExportHtmlProvider(fileName));
                    OpenFile(fileName);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("sbExportToHTML_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
                panelProgressBar.Visible = false;
            }
            return;

        }
        //</sbExportToHTML>

        //<sbExportToXML>
        private void sbExportToXML_Click(object sender, System.EventArgs e)
        {
            try
            {
                panelProgressBar.Visible = true;

                string fileName = ShowSaveFileDialog("XML документ", "XML Documents|*.xml");
                if (fileName != "")
                {
                    ExportTo(new ExportXmlProvider(fileName));
                    OpenFile(fileName);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("sbExportToXML_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
                panelProgressBar.Visible = false;
            }

            return;
        }
        //</sbExportToXML>

        //<sbExportToXLS>
        private void sbExportToXLS_Click(object sender, System.EventArgs e)
        {
            try
            {
                panelProgressBar.Visible = true;
                string fileName = ShowSaveFileDialog("Microsoft Excel Document", "Microsoft Excel|*.xls");
                if (fileName != "")
                {
                    ExportTo(new ExportXlsProvider(fileName));
                    OpenFile(fileName);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("sbExportToXLS_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
                panelProgressBar.Visible = false;
            }

            return;
        }
        //</sbExportToXLS>

        //<sbExportToTXT>
        private void sbExportToTXT_Click(object sender, System.EventArgs e)
        {
            try
            {
                panelProgressBar.Visible = true;
                string fileName = ShowSaveFileDialog("Text Document", "Text Files|*.txt");
                if (fileName != "")
                {
                    ExportTo(new ExportTxtProvider(fileName));
                    OpenFile(fileName);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("sbExportToTXT_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
                panelProgressBar.Visible = false;
            }

            return;
        }
        //</sbExportToTXT>

        //<sbExportToTXT>
        private void sbExportToDBF_Click(object sender, System.EventArgs e)
        {
            try
            {
                panelProgressBar.Visible = true;
                string fileName = ShowSaveFileDialog("документ DBF", "DBF Files|*.dbf");
                if (fileName != "")
                {
                    if (EхportDBF(ref fileName, false) == true)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("Экспорт данных успешно завершён.\n" + fileName, "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    }
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("sbExportToTXT_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
                panelProgressBar.Visible = false;
            }

            return;
        }

        private void sbExportToDBFCurrency_Click(object sender, System.EventArgs e)
        {
            try
            {
                panelProgressBar.Visible = true;
                string fileName = ShowSaveFileDialog("документ DBF", "DBF Files|*.dbf");
                if (fileName != "")
                {
                    if (EхportDBF( ref fileName, true) == true)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("Экспорт данных успешно завершён.\n" + fileName, "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    }
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("sbExportToTXT_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
                panelProgressBar.Visible = false;
            }

            return;
        }

        private bool EхportDBF(ref string fileName, System.Boolean bCurrencyPrice)
        {
            string tableName = string.Empty;
            bool returnStatus = false;
            try
            {
                System.Int32 imaxLenghtFilename = 8;
                if (System.IO.Path.GetFileNameWithoutExtension(fileName).Length > imaxLenghtFilename)
                {
                    fileName = (System.IO.Path.GetDirectoryName(fileName) + "\\" + System.IO.Path.GetFileNameWithoutExtension(fileName).Trim().Replace(" ", "").Substring(0, imaxLenghtFilename) + ".dbf");
                }
                System.IO.File.Delete(fileName);


                string jetOleDbConString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source={0}; Extended Properties=dBASE IV";
                OleDbConnection conn = new System.Data.OleDb.OleDbConnection();
                conn.ConnectionString = String.Format(jetOleDbConString, System.IO.Path.GetDirectoryName(fileName));

                System.Data.OleDb.OleDbCommand oleDbCommandCreateTable = new System.Data.OleDb.OleDbCommand();
                System.Data.OleDb.OleDbCommand oleDbJetInsertCommand = new System.Data.OleDb.OleDbCommand();
                oleDbCommandCreateTable.Connection = conn;
                oleDbJetInsertCommand.Connection = conn;

                System.String strTableName = System.IO.Path.GetFileNameWithoutExtension(fileName);
                oleDbCommandCreateTable.CommandText = "CREATE TABLE " + strTableName + "(ARTICLE CHAR (20), NAME2 CHAR (52), QUANTITY Integer, PRICE Double, MARKUP Double, PARTS_ID Integer )";

                conn.Open();

                oleDbCommandCreateTable.ExecuteNonQuery();

                oleDbJetInsertCommand.CommandText = "INSERT INTO " + strTableName + " (ARTICLE, NAME2, QUANTITY, PRICE, MARKUP, PARTS_ID) VALUES (?, ?, ?, ?, ?, ?)";
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("ARTICLE", System.Data.OleDb.OleDbType.VarWChar, 20, "ARTICLE"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("NAME2", System.Data.OleDb.OleDbType.VarWChar, 52, "NAME2"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("QUANTITY", System.Data.OleDb.OleDbType.Integer, 0, "QUANTITY"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("PRICE", System.Data.OleDb.OleDbType.Double, 0, "PRICE"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("MARKUP", System.Data.OleDb.OleDbType.Double, 0, "MARKUP"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("PARTS_ID", System.Data.OleDb.OleDbType.Integer, 0, "PARTS_ID"));

                foreach (COrderItem objItem in m_objSelectedOrder.OrderItemList)
                {
                    oleDbJetInsertCommand.Parameters["ARTICLE"].Value = System.Convert.ToString(objItem.Product.Article);
                    oleDbJetInsertCommand.Parameters["NAME2"].Value = System.Convert.ToString(objItem.Product.Name);
                    oleDbJetInsertCommand.Parameters["QUANTITY"].Value = System.Convert.ToInt32(objItem.QuantityReserved);
                    oleDbJetInsertCommand.Parameters["PRICE"].Value = ((bCurrencyPrice == true) ? System.Convert.ToDouble(objItem.PriceInAccountingCurrency) : System.Convert.ToDouble(objItem.Price));
                    oleDbJetInsertCommand.Parameters["MARKUP"].Value = 0;
                    oleDbJetInsertCommand.Parameters["PARTS_ID"].Value = System.Convert.ToInt32(objItem.Product.ID_Ib);

                    oleDbJetInsertCommand.ExecuteNonQuery();
                }

                //for (System.Int32 i = 0; i < gridView.RowCount; i++)
                //{
                //    oleDbJetInsertCommand.Parameters["ARTICLE"].Value = System.Convert.ToString(gridView.GetRowCellValue(i, colOrderItems_PartsArticle));
                //    oleDbJetInsertCommand.Parameters["NAME2"].Value = System.Convert.ToString(gridView.GetRowCellValue(i, colOrderItems_PartsName));
                //    oleDbJetInsertCommand.Parameters["QUANTITY"].Value = System.Convert.ToInt32(gridView.GetRowCellValue(i, colQuantityReserved));
                //    oleDbJetInsertCommand.Parameters["PRICE"].Value = ((bCurrencyPrice == true) ?  System.Convert.ToDouble(gridView.GetRowCellValue(i, colPriceInAccountingCurrency) ) : System.Convert.ToDouble(gridView.GetRowCellValue(i, colPrice)) );
                //    oleDbJetInsertCommand.Parameters["MARKUP"].Value = 0;
                //    oleDbJetInsertCommand.Parameters["PARTS_ID"].Value = System.Convert.ToInt32(gridView.GetRowCellValue(i, colProductID));

                //    oleDbJetInsertCommand.ExecuteNonQuery();

                //}

                conn.Close();
                returnStatus = true;

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось произвести экспорт заказа в файл dbf.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return returnStatus;
        } // close function

        //</sbExportToTXT>

        private void OpenFile(string fileName)
        {
            if (XtraMessageBox.Show("Хотите открыть этот файл?", "Экспорт в...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
            progressBarControl1.Position = 0;
        }

        //<sbExportToHTML>
        private void ExportTo(IExportProvider provider)
        {
            Cursor currentCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;

            this.FindForm().Refresh();
            BaseExportLink link = gridView.CreateExportLink(provider);
            (link as GridViewExportLink).ExpandAll = false;
            link.Progress += new DevExpress.XtraGrid.Export.ProgressEventHandler(Export_Progress);
            link.ExportTo(true);
            provider.Dispose();
            link.Progress -= new DevExpress.XtraGrid.Export.ProgressEventHandler(Export_Progress);

            Cursor.Current = currentCursor;
        }
        //</sbExportToHTML>

        private string ShowSaveFileDialog(string title, string filter)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            string name = Application.ProductName;
            int n = name.LastIndexOf(".") + 1;
            if (n > 0) name = name.Substring(n, name.Length - n);
            dlg.Title = "Экспорт данных в " + title;
            dlg.FileName = name;
            dlg.Filter = filter;
            if (dlg.ShowDialog() == DialogResult.OK) return dlg.FileName;
            return "";
        }

        //<sbExportToHTML>
        private void Export_Progress(object sender, DevExpress.XtraGrid.Export.ProgressEventArgs e)
        {
            if (e.Phase == DevExpress.XtraGrid.Export.ExportPhase.Link)
            {
                progressBarControl1.Position = e.Position;
                this.Update();
            }
        }
        //</sbExportToHTML>

        #endregion

        #region Импорт приложения к заказу
        private void mitmsImportFromExcel_Click(object sender, EventArgs e)
        {
            try
            {
                ImportFromExcel();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("ImportFromExcel. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        public void ImportFromExcel()
        {
            try
            {
                gridView.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gridView_CellValueChanged);

                if (Stock.SelectedItem == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Укажите, пожалуйста, склад отгрузки.", "Внимание",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                    return;
                }
                List<System.String> DepartCodeList = new List<string>();
                for( System.Int32 i = 0; i < Depart.Properties.Items.Count; i++ )
                {
                    DepartCodeList.Add( System.Convert.ToString( Depart.Properties.Items[i] ) );
                }
                
                CCustomer objSelectedCustomer = ( ( Customer.SelectedItem == null ) ? null : (CCustomer)Customer.SelectedItem );
                CRtt objSelectedRtt = ( ( Rtt.SelectedItem == null ) ? null : (CRtt)Rtt.SelectedItem );
                CDepart objSelectedDepart = ((Depart.SelectedItem == null) ? null : (CDepart)Depart.SelectedItem);
                CStock objSelectedStock = ( ( Stock.SelectedItem == null ) ? null : (CStock)Stock.SelectedItem );
                CPaymentType objSelectedPaymentType = ((PaymentType.SelectedItem == null) ? null : (CPaymentType)PaymentType.SelectedItem);

                System.Int32 iPaymentType = ((objSelectedPaymentType == null) ? 0 : (objSelectedPaymentType.ID == m_iPaymentType1) ? 1 : 2);

                frmImportXLSData objFrmImportXLSData = new frmImportXLSData(m_objProfile, m_objMenuItem, DepartCodeList, m_objCustomerList);

                objFrmImportXLSData.OpenForImportPartsInSuppl(objSelectedCustomer, objSelectedRtt,
                    (objSelectedDepart == null ? "" : objSelectedDepart.DepartCode), iPaymentType, objSelectedStock, objSelectedPaymentType,
                    ((m_bIsAutoCreatePriceMode == true) ? 0 : System.Convert.ToDouble(spinEditDiscount.Value)), dataSet.Tables["OrderItems"], m_strXLSImportFilePath,
                    m_iXLSSheetImport, m_SheetList, checkMultiplicity.Checked);

                DialogResult dlgRes = objFrmImportXLSData.DialogResult;

                m_strXLSImportFilePath = objFrmImportXLSData.FileFullName;
                m_iXLSSheetImport = objFrmImportXLSData.SelectedSheetId;
                m_SheetList = objFrmImportXLSData.SheetList;

                if ((objFrmImportXLSData.SelectedCustomer != null) && (objSelectedCustomer != null) && 
                    (objFrmImportXLSData.SelectedCustomer.ID.CompareTo(objSelectedCustomer.ID) == 0) &&
                    ( objFrmImportXLSData.SelectedRtt != null ) )
                {
                    Rtt.SelectedItem = Rtt.Properties.Items.Cast<CRtt>().Single<CRtt>(x => x.ID.CompareTo(objFrmImportXLSData.SelectedRtt.ID) == 0);
                    if (Rtt.SelectedItem != null)
                    {
                        AddressDelivery.Properties.Items.AddRange((((CRtt)Rtt.SelectedItem).AddressList));
                    }
                }

                if ((Customer.SelectedItem == null) && (objFrmImportXLSData.SelectedCustomer != null))
                {
                    Customer.SelectedItem = Customer.Properties.Items.Cast<CCustomer>().Single<CCustomer>(x => x.ID == objFrmImportXLSData.SelectedCustomer.ID);
                    Rtt.SelectedItem = Rtt.Properties.Items.Cast<CRtt>().Single<CRtt>(x => x.ID.CompareTo(objFrmImportXLSData.SelectedRtt.ID) == 0);
                    if (Rtt.SelectedItem != null)
                    {
                        AddressDelivery.Properties.Items.AddRange((((CRtt)Rtt.SelectedItem).AddressList));
                    }

                    if (((CPaymentType)PaymentType.SelectedItem).ID == m_iPaymentType2)
                    {
                        if (ChildDepart.Properties.Items.Count > 0)
                        {
                            ChildDepart.SelectedIndex = 0;
                        }
                    }

                    GetCustomerDebtInfo(true);
                }

                if ((Depart.SelectedItem == null) && (objFrmImportXLSData.DeparCode != ""))
                {
                    Depart.SelectedItem = Depart.Properties.Items.Cast<CDepart>().Single<CDepart>(x => x.DepartCode == objFrmImportXLSData.DeparCode);
                }

                objFrmImportXLSData.Dispose();
                objFrmImportXLSData = null;
                objSelectedCustomer = null;
                objSelectedRtt = null;
                objSelectedDepart = null;
                objSelectedStock = null;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("ImportFromExcel. Текст ошибки: " + f.Message);
            }
            finally
            {
                gridView.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gridView_CellValueChanged);
                btnSave.Enabled = (ValidateProperties() == true);
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
            System.Int32 iStartRow = 10;
            System.Int32 iCurrentRow = iStartRow;
            object m = Type.Missing;
            //Excel.Range oRng;

            try
            {
                // курс ценообразования
                System.String strErr = "";
                if (COrderRepository.GetCurrencyRatePricing(m_objProfile, null, BeginDate.DateTime, ref m_CurratePricing, ref strErr) == false)
                {
                    SendMessageToLog("Не удалось получить значение курса ценообразования: " + strErr );
                    return;
                }

                
                System.String strFileName = "";
                System.String strDLLPath = Application.StartupPath;
                strDLLPath += ( "\\" + m_strReportsDirectory +  "\\" );

                strFileName = strDLLPath + m_strReportSuppl;

                if (System.IO.File.Exists(strFileName) == false)
                {
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
                }

                SendMessageToLog("Идёт экспорт данных в MS Excel... ");
                this.Cursor = Cursors.WaitCursor;
                oXL = new Excel.Application();
                oWB = (Excel._Workbook)(oXL.Workbooks.Open(strFileName, 0, true, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value));
                oSheet = (Excel._Worksheet)oWB.Worksheets[1];


                // форма оплаты №1
                CCustomer objSelectedCustomer = ((Customer.SelectedItem == null) ? null : (CCustomer)Customer.SelectedItem);
                System.String strCustomerName = "";
                System.String strCustomerUNN = "";
                System.String strCustomerChildCode = "";
                System.String strOrderNum = m_objSelectedOrder.Num.ToString();
                System.String strStock = "Склад: " + Stock.Text;
                System.String strSupplId = "№1" + (m_objSelectedOrder.Ib_ID + 1000000).ToString().Substring( 1, 6 );
                System.String strSupplDate = BeginDate.DateTime.ToShortDateString();

                if (ChildDepart.Text != "")
                {
                    strCustomerChildCode += ("[" + ChildDepart.Text + "]");
                }

                if (objSelectedCustomer != null)
                {
                    strCustomerName =( "[" +  objSelectedCustomer.FullName + "]");
                    strCustomerUNN = ( "\\" + objSelectedCustomer.UNP + "/");
                }

                strOrderNum = "Отборочный лист №" + strOrderNum + strCustomerUNN + strCustomerChildCode + strCustomerName;

                oSheet.Cells[3, 1] = strOrderNum;
                oSheet.Cells[3, 4] = strSupplDate;
                oSheet.Cells[5, 1] = strStock;
                oSheet.Cells[7, 1] = strSupplId;

                System.Int32 iRecordNum = 0;
                System.Double dclQuantity = 0;
                System.Double dclPrice = 0;
                System.Double dclPriceWithDiscount = 0;
                System.Double dclDiscount = 0;
                System.String strPartsName = "";
                System.String strMeasureName = "";

                for (System.Int32 i = 0; i < gridView.RowCount; i++)
                {
                    iRecordNum++;
                    try
                    {
                        dclQuantity = System.Convert.ToDouble(gridView.GetRowCellValue(i, colQuantityReserved));
                    }
                    catch
                    {
                        dclQuantity = 0;
                    }
                    try
                    {
                        dclPrice = (System.Convert.ToDouble(gridView.GetRowCellValue(i, colPriceInAccountingCurrency))) * m_CurratePricing;
                    }
                    catch
                    {
                        dclPrice = 0;
                    }
                    try
                    {
                        dclPriceWithDiscount = (System.Convert.ToDouble(gridView.GetRowCellValue(i, colPriceWithDiscountInAccountingCurrency))) * m_CurratePricing;
                    }
                    catch
                    {
                        dclPriceWithDiscount = 0;
                    }
                    try
                    {
                        dclDiscount = System.Convert.ToDouble(gridView.GetRowCellValue(i, colDiscountPercent));
                    }
                    catch
                    {
                        dclDiscount = 0;
                    }
                    strPartsName = System.Convert.ToString(gridView.GetRowCellValue(i, colOrderItems_PartsName )) + " " +
                        System.Convert.ToString(gridView.GetRowCellValue(i, colOrderItems_PartsArticle));
                    strMeasureName = System.Convert.ToString(gridView.GetRowCellValue(i, colOrderItems_MeasureName));

                    oSheet.Cells[iCurrentRow, 1] = strPartsName;
                    oSheet.Cells[iCurrentRow, 2] = dclQuantity;
                    oSheet.Cells[iCurrentRow, 3] = "";
                    oSheet.Cells[iCurrentRow, 4] = strMeasureName;

                    if (i < (gridView.RowCount - 1))
                    {
                        oSheet.get_Range(oSheet.Cells[iCurrentRow, 1], oSheet.Cells[iCurrentRow, 100]).Copy(Missing.Value);
                        oSheet.get_Range(oSheet.Cells[iCurrentRow, 1], oSheet.Cells[iCurrentRow, 1]).Insert(Excel.XlInsertShiftDirection.xlShiftDown, Missing.Value);
                        iCurrentRow++;
                    }

                }
                oSheet.Cells[iCurrentRow + 1, 1] = "Итого:";
                oSheet.get_Range(oSheet.Cells[iCurrentRow + 1, 2], oSheet.Cells[iCurrentRow + 1, 2]).Formula = "=СУММ(R[-" + iRecordNum.ToString() + "]C:R[-1]C)";
                oSheet.get_Range("A1", "A1").EntireColumn.AutoFit();

                if (m_objSelectedOrder.PaymentType.ID == m_iPaymentType2)
                {
                    // форма оплаты №2
                    oSheet = (Excel._Worksheet)oWB.Worksheets[2];
                    strCustomerName = "";
                    strCustomerUNN = "";
                    strCustomerChildCode = ChildDepart.Text;
                    strOrderNum = m_objSelectedOrder.Num.ToString();
                    strStock = ("[" + strStock + "] >> " + strCustomerChildCode);

                    strCustomerUNN = ("/" + objSelectedCustomer.UNP + "/");
                    strOrderNum = "Заказ №" + strOrderNum + strCustomerUNN + " от " + strSupplDate;

                    oSheet.Cells[3, 1] = strOrderNum;
                    oSheet.Cells[5, 1] = strStock;
                    oSheet.Cells[7, 1] = txtDescription.Text;

                    iStartRow = 10;
                    iCurrentRow = iStartRow;
                    iRecordNum = 0;

                    for (System.Int32 i = 0; i < gridView.RowCount; i++)
                    {
                        iRecordNum++;
                        try
                        {
                            dclQuantity = System.Convert.ToDouble(gridView.GetRowCellValue(i, colQuantityReserved));
                        }
                        catch
                        {
                            dclQuantity = 0;
                        }
                        try
                        {
                            dclPrice = (System.Convert.ToDouble(gridView.GetRowCellValue(i, colPriceInAccountingCurrency))) * m_CurratePricing;
                        }
                        catch
                        {
                            dclPrice = 0;
                        }
                        try
                        {
                            dclPriceWithDiscount = (System.Convert.ToDouble(gridView.GetRowCellValue(i, colPriceWithDiscountInAccountingCurrency))) * m_CurratePricing;
                        }
                        catch
                        {
                            dclPriceWithDiscount = 0;
                        }
                        try
                        {
                            dclDiscount = System.Convert.ToDouble(gridView.GetRowCellValue(i, colDiscountPercent));
                        }
                        catch
                        {
                            dclDiscount = 0;
                        }
                        strPartsName = System.Convert.ToString(gridView.GetRowCellValue(i, colOrderItems_PartsName)) + " " +
                            System.Convert.ToString(gridView.GetRowCellValue(i, colOrderItems_PartsArticle));
                        strMeasureName = System.Convert.ToString(gridView.GetRowCellValue(i, colOrderItems_MeasureName));

                        oSheet.Cells[iCurrentRow, 1] = strPartsName;
                        if (dclDiscount <= 0)
                        {
                            oSheet.Cells[iCurrentRow, 2] = "-";
                        }
                        else
                        {
                            oSheet.Cells[iCurrentRow, 2] = dclDiscount;
                        }
                        oSheet.Cells[iCurrentRow, 3] = dclQuantity;
                        oSheet.Cells[iCurrentRow, 4] = dclPrice;
                        oSheet.Cells[iCurrentRow, 5] = ( dclQuantity * dclPrice );
                        oSheet.Cells[iCurrentRow, 6] = (dclQuantity * dclPriceWithDiscount);

                        if (i < (gridView.RowCount - 1))
                        {
                            oSheet.get_Range(oSheet.Cells[iCurrentRow, 1], oSheet.Cells[iCurrentRow, 100]).Copy(Missing.Value);
                            oSheet.get_Range(oSheet.Cells[iCurrentRow, 1], oSheet.Cells[iCurrentRow, 1]).Insert(Excel.XlInsertShiftDirection.xlShiftDown, Missing.Value);
                            iCurrentRow++;
                        }
                    }
                    oSheet.Cells[iCurrentRow + 1, 1] = "Итого:";
                    oSheet.get_Range(oSheet.Cells[iCurrentRow + 1, 3], oSheet.Cells[iCurrentRow + 1, 3]).Formula = "=СУММ(R[-" + ( iRecordNum  ).ToString() + "]C:R[-1]C)";
                    oSheet.get_Range(oSheet.Cells[iCurrentRow + 1, 5], oSheet.Cells[iCurrentRow + 1, 5]).Formula = "=СУММ(R[-" + (iRecordNum ).ToString() + "]C:R[-1]C)";
                    oSheet.get_Range(oSheet.Cells[iCurrentRow + 1, 6], oSheet.Cells[iCurrentRow + 1, 6]).Formula = "=СУММ(R[-" + (iRecordNum ).ToString() + "]C:R[-1]C)";
                    oSheet.get_Range("A1", "A1").EntireColumn.AutoFit();

                }


                ((Excel._Worksheet)oWB.Worksheets[1]).Activate();

                oXL.Visible = true;
                oXL.UserControl = true;

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

        #region Информация о дебиторской задолженности
        private void ClearLabelsCriditControl()
        {
            try
            {
               lblDebtWaybillShipped.Text = "";
               lblDayDebtWaybillShipped.Text = "";
               LDebtWaybill.Text = "";
               LDebtSuppl.Text = "";
               LCustomerLimitMoney.Text = "";
               LCustomerLimitDays.Text = "";
               LOverdraftMoney.Text = "";
               LEarning.Text  = "";
               LOutSumma.Text = "";
               LOutDays.Text = "";
               LCreditControlResult.Text = "";
            }
            catch (System.Exception f)
            {
                SendMessageToLog("ClearLabelsCriditControl. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private void GetCustomerDebtInfo( System.Boolean bReloadInfoFromDB = false )
        {
            try
            {
                ClearLabelsCriditControl();

                if( Customer.SelectedItem == null ){return;}
                if( Stock.SelectedItem == null ){return;}
                if (m_bDisableEvents == true) { return; }

                // необходимо запросить состояние заказа
                // если заказ удален или отгружен, то запрашивать информацию о дебиторке смысла нет
                enPDASupplState enumOrderState = ( enPDASupplState )m_objSelectedOrder.OrderStateId;

                if ((enumOrderState == enPDASupplState.Deleted) || (enumOrderState == enPDASupplState.Shipped) || (enumOrderState == enPDASupplState.TTN))
                {
                    tableLayoutPanelBackground.RowStyles[m_iDebtorInfoPanelIndex].Height = 0;
                    return;
                }
                else
                {
                    tableLayoutPanelBackground.RowStyles[m_iDebtorInfoPanelIndex].Height = m_iDebtorInfoPanelHeight;
                }


                System.Double WAYBILL_TOTALPRICE = System.Convert.ToDouble( colSumReservedWithDiscount.SummaryItem.SummaryValue);
                System.Double WAYBILL_CURRENCYTOTALPRICE = System.Convert.ToDouble(colSumReservedWithDiscountInAccountingCurrency.SummaryItem.SummaryValue);

                System.Boolean bRes = true;

                System.Double WAYBILL_SHIPPED_SALDO = 0;
                System.Int32 WAYBILL_SHIPPED_DEBTDAYS = 0;
                System.Double WAYBILL_SALDO = 0;
                System.Int32 WAYBILL_DEBTDAYS = 0;
                System.Double INITIAL_DEBT = 0;
                System.Int32 INITIAL_DEBTDAYS = 0;
                System.Double SUPPL_SALDO = 0;
                System.Double EARNING_SALDO = 0;
                System.Double CUSTOMER_LIMITPRICE = 0;
                System.Int32 CUSTOMER_LIMITDAYS = 0;
                System.Double OVERDRAFT = 0;
                System.Double CUSTOMER_DEBTPRICE = 0;
                System.Int32 CUSTOMER_DEBTDAYS = 0;
                
                System.String strErr = "";
                System.Double dblOutSumma = 0;
                System.Int32 iOutDays = 0;
                System.Boolean bCustomerInBlackList = false;

                System.Guid PaymentType_Guid = ((PaymentType.SelectedItem == null) ? (System.Guid.Empty) : ((CPaymentType)PaymentType.SelectedItem).ID);

                if (bReloadInfoFromDB == true)
                {
                    // скорее всего выбрали другого клиента или склад, поэтому информацию о задолженности нужно обновить
                    System.Guid Order_Guid = m_objSelectedOrder.ID;
                    System.Guid OrderState_Guid = ((m_objSelectedOrder.OrderState == null) ? System.Guid.Empty : m_objSelectedOrder.OrderState.ID);
                    System.Boolean Order_MoneyBonus = (this.IsBonus.CheckState == CheckState.Checked);
                    System.Guid Customer_Guid = ((Customer.SelectedItem == null) ? (System.Guid.Empty) : ((CCustomer)Customer.SelectedItem).ID);
                    System.Guid CustomerChild_Guid = ((ChildDepart.SelectedItem == null) ? (System.Guid.Empty) : ((CChildDepart)ChildDepart.SelectedItem).ID);
                    System.Guid Company_Guid = ((Stock.SelectedItem == null) ? (System.Guid.Empty) : ((CStock)Stock.SelectedItem).Company.ID);
                    m_SUMM_IS_PASS = 0;

                    bRes = COrderRepository.GetCustomerDebtInfoFromIB(m_objProfile, null, Customer_Guid, CustomerChild_Guid,
                        PaymentType_Guid, Company_Guid, Order_Guid, WAYBILL_TOTALPRICE, WAYBILL_CURRENCYTOTALPRICE,
                        Order_MoneyBonus, ref WAYBILL_SHIPPED_SALDO, ref WAYBILL_SHIPPED_DEBTDAYS, ref WAYBILL_SALDO,
                        ref WAYBILL_DEBTDAYS, ref INITIAL_DEBT, ref INITIAL_DEBTDAYS, ref SUPPL_SALDO, ref EARNING_SALDO,
                        ref CUSTOMER_LIMITPRICE, ref CUSTOMER_LIMITDAYS, ref OVERDRAFT, ref CUSTOMER_DEBTPRICE,
                        ref CUSTOMER_DEBTDAYS, ref m_SUMM_IS_PASS, ref strErr);

                    if (bRes == true)
                    {
                        lblDebtWaybillShipped.Tag = WAYBILL_SHIPPED_SALDO;
                        lblDayDebtWaybillShipped.Tag = WAYBILL_SHIPPED_DEBTDAYS;
                        LDebtWaybill.Tag = WAYBILL_SALDO;
                        LDebtSuppl.Tag = SUPPL_SALDO;
                        LCustomerLimitMoney.Tag = CUSTOMER_LIMITPRICE;
                        LCustomerLimitDays.Tag = CUSTOMER_LIMITDAYS;
                        LOverdraftMoney.Tag = OVERDRAFT;
                        LEarning.Tag = EARNING_SALDO;
                        LOutSumma.Tag = CUSTOMER_DEBTPRICE;
                        LOutDays.Tag = CUSTOMER_DEBTDAYS;

                        if (COrderRepository.IsCustomerInBL(m_objProfile, null, Customer_Guid, Company_Guid,
                            ref bCustomerInBlackList, ref strErr) == false)
                        {
                            SendMessageToLog("IsCustomerInBL. Текст ошибки: " + strErr);
                        }
                        else
                        {
                            m_bCustomerInBlackList = bCustomerInBlackList;
                        }

                    }
                    else
                    {
                        SendMessageToLog("GetCustomerDebtInfo. Текст ошибки: " + strErr);
                    }
                }
                else
                {
                    // зачитываем информацию о дебиторке непосредственно из элементов управления
                    WAYBILL_SHIPPED_SALDO = ( (lblDebtWaybillShipped.Tag != null) ? System.Convert.ToDouble(lblDebtWaybillShipped.Tag) : 0 );
                    WAYBILL_SHIPPED_DEBTDAYS = ((lblDayDebtWaybillShipped.Tag != null) ? System.Convert.ToInt32(lblDayDebtWaybillShipped.Tag) : 0);
                    WAYBILL_SALDO = ((LDebtWaybill.Tag != null) ? System.Convert.ToDouble(LDebtWaybill.Tag) : 0);
                    SUPPL_SALDO = ((LDebtSuppl.Tag != null) ? System.Convert.ToDouble(LDebtSuppl.Tag) : 0);
                    CUSTOMER_LIMITPRICE = ((LCustomerLimitMoney.Tag != null) ? System.Convert.ToDouble(LCustomerLimitMoney.Tag) : 0);
                    CUSTOMER_LIMITDAYS = ((LCustomerLimitDays.Tag != null) ? System.Convert.ToInt32(LCustomerLimitDays.Tag) : 0);
                    OVERDRAFT = ((LOverdraftMoney.Tag != null) ? System.Convert.ToDouble(LOverdraftMoney.Tag) : 0);
                    EARNING_SALDO = ((LEarning.Tag != null) ? System.Convert.ToDouble(LEarning.Tag) : 0);
                    CUSTOMER_DEBTPRICE = ((LOutSumma.Tag != null) ? System.Convert.ToDouble(LOutSumma.Tag) : 0);
                    CUSTOMER_DEBTDAYS = ((LOutDays.Tag != null) ? System.Convert.ToInt32(LOutDays.Tag) : 0);
                }

                    // рисуем итоги
                   lblDebtWaybillShipped.Text = System.String.Format( "{0:### ### ##0}", WAYBILL_SHIPPED_SALDO);
                   lblDayDebtWaybillShipped.Text = System.String.Format( "{0:### ### ##0}", WAYBILL_SHIPPED_DEBTDAYS );
                   LDebtWaybill.Text = System.String.Format( "{0:### ### ##0}", WAYBILL_SALDO);
                   LDebtSuppl.Text = System.String.Format("{0:### ### ##0}", SUPPL_SALDO);
                   LCustomerLimitMoney.Text = System.String.Format( "{0:### ### ##0}", CUSTOMER_LIMITPRICE);
                   LCustomerLimitDays.Text = System.String.Format( "{0:### ### ##0}", CUSTOMER_LIMITDAYS);
                   LOverdraftMoney.Text = System.String.Format( "{0:### ### ##0}", OVERDRAFT);
                   LEarning.Text = System.String.Format("{0:### ### ##0}", EARNING_SALDO);

                    if( PaymentType_Guid == m_iPaymentType2 ) 
                    {
                        dblOutSumma = ( CUSTOMER_LIMITPRICE + OVERDRAFT) - ( WAYBILL_CURRENCYTOTALPRICE + Math.Abs( CUSTOMER_DEBTPRICE ) );
                    }
                    else
                    {
                        dblOutSumma = ( CUSTOMER_LIMITPRICE + OVERDRAFT ) - ( WAYBILL_TOTALPRICE +  Math.Abs( CUSTOMER_DEBTPRICE ) );
                    }
                    iOutDays = ( CUSTOMER_LIMITDAYS -  Math.Abs( CUSTOMER_DEBTDAYS ));

                    if( dblOutSumma > 0 ) { dblOutSumma = 0;}
                    if( iOutDays > 0 ) { iOutDays = 0;}

                    if (bReloadInfoFromDB == false)
                    {
                        m_SUMM_IS_PASS = ( ((dblOutSumma == 0) && (iOutDays == 0)) ? 1 : 0 );
                    }

                    if ((m_SUMM_IS_PASS == 1) && (m_bCustomerInBlackList == false)) 
                    {
                        LCreditControlResult.Text = "заказ проходит кредитный контроль";
                        LCreditControlResult.ForeColor = Color.Green;
                        ImageOk.Visible = true;
                        ImageNot.Visible = false;
                    }
                    else
                    {
                        if (m_bCustomerInBlackList == true) 
                            {LCreditControlResult.Text = "клиент в черном списке";}
                        else
                            {LCreditControlResult.Text = "кредитные условия не выполняются";}

                        LCreditControlResult.ForeColor = Color.Red;
                        ImageOk.Visible = false;
                        ImageNot.Visible = true;
                    }

                    LOutSumma.Text = System.String.Format("{0:### ### ##0}", dblOutSumma);
                    LOutDays.Text = iOutDays.ToString();
                    if( dblOutSumma < 0 ) 
                        {LOutSumma.ForeColor = Color.Red;}
                    else
                        {LOutSumma.ForeColor = Color.Green;}
                    if( iOutDays < 0 ) 
                        {LOutDays.ForeColor = Color.Red;}
                    else
                        {LOutDays.ForeColor = Color.Green;}

            }
            catch (System.Exception f)
            {
                SendMessageToLog( "GetCustomerDebtInfo. Текст ошибки: " + f.Message );
            }
            finally
            {
            }
            return;
        }
        #endregion

        #region Установка режима расчёта цен
        /// <summary>
        /// Установка режима расчёта цен
        /// </summary>
        private void SetCreatePriceMode()
        {
            try
            {
                m_bIsAutoCreatePriceMode = COrderRepository.IsAutoCreatePriceMode(m_objProfile);

                checkEditCalcPrices.Checked = m_bIsAutoCreatePriceMode;
                checkEditCalcPrices.Enabled = (m_bIsAutoCreatePriceMode == false);
                spinEditDiscount.Enabled = (m_bIsAutoCreatePriceMode == false);
                btnSetDiscount.Enabled = (m_bIsAutoCreatePriceMode == false);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("SetCreatePriceMode. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        #endregion

        #region Контекстное меню
        private void mitemClearRows_Click(object sender, EventArgs e)
        {
            dataSet.Tables["OrderItems"].Clear();
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                mitemExport.Enabled = (gridView.RowCount > 0);
                mitemClearRows.Enabled = ((gridView.RowCount > 0) && (m_bIsReadOnly == false));
                mitemImport.Enabled = (m_bIsReadOnly == false);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("contextMenuStrip_Opening. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;

        }
        #endregion

    }

    /// <summary>
    /// Тип, хранящий информацию, которая передается получателям уведомления о событии
    /// </summary>
    public class ChangeOrderForCustomerPropertieEventArgs : EventArgs
    {
        private readonly COrder m_objOrder;
        public COrder Order
        { get { return m_objOrder; } }

        private readonly enumActionSaveCancel m_enActionType;
        public enumActionSaveCancel ActionType
        { get { return m_enActionType; } }

        private readonly System.Boolean m_bIsNewOrder;
        public System.Boolean IsNewOrder
        { get { return m_bIsNewOrder; } }

        public ChangeOrderForCustomerPropertieEventArgs(COrder objOrder, enumActionSaveCancel enActionType, System.Boolean bIsNewOrder)
        {
            m_objOrder = objOrder;
            m_enActionType = enActionType;
            m_bIsNewOrder = bIsNewOrder;
        }
    }
}
