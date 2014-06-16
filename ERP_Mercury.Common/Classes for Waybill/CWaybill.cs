using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ERP_Mercury.Common
{
    /// <summary>
    /// Класс "Строка табличной части расходной накладной"
    /// </summary>
    public class CWaybillItem
    {
        #region Уникальные идентификаторы
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public System.Guid ID { get; set; }
        /// <summary>
        /// Уникальный идентификатор накладной в InterBase
        /// </summary>
        public System.Int32 Ib_ID { get; set; }
        /// <summary>
        /// Уникальный идентификатор строки в приложении к заказу
        /// </summary>
        public System.Guid SupplItemID { get; set; }
        #endregion

        #region Товарная позиция
        /// <summary>
        /// Товар
        /// </summary>
        public CProduct Product { get; set; }
        /// <summary>
        /// Наименование товара
        /// </summary>
        public System.String ProductFullName
        {
            get { return ((Product == null) ? "" : Product.ProductFullName); }
            set { ProductFullName = value; }
        }
        /// <summary>
        /// Единица измерения
        /// </summary>
        public CMeasure Measure { get; set; }
        #endregion

        #region Итоговые суммы, количество
        /// <summary>
        /// Количество проданное
        /// </summary>
        public System.Double Quantity { get; set; }
        /// <summary>
        /// Количество возвращенное
        /// </summary>
        public System.Double QuantityReturn { get; set; }
        /// <summary>
        /// Количество проданное с учетом возврата
        /// </summary>
        public System.Double QuantityWithReturn
        {
            get { return (Quantity - QuantityReturn); }
        }

        /// <summary>
        /// Цена первого поставщика (в национальной валюте)
        /// </summary>
        public System.Double PriceImporter { get; set; }
        /// <summary>
        /// Ставка НДС в процентах
        /// </summary>
        public System.Double NDSPercent { get; set; }
        /// <summary>
        /// Цена отпускная (в национальной валюте)
        /// </summary>
        public System.Double Price { get; set; }
        /// <summary>
        /// Размер скидки в процентах
        /// </summary>
        public System.Double DiscountPercent { get; set; }
        /// <summary>
        /// Цена отпускная с учетом скидки
        /// </summary>
        public System.Double PriceWithDiscount { get; set; }

        /// <summary>
        /// Сумма без учета скидки (в национальной валюте)
        /// </summary>
        public System.Double Sum { get { return ( Quantity * Price ); } }
        /// <summary>
        /// Сумма с учетом скидки (в национальной валюте)
        /// </summary>
        public System.Double SumWithDiscount { get { return ( Quantity * PriceWithDiscount ); } }
        /// <summary>
        /// Сумма с учетом скидки и с учетом возврата (в национальной валюте)
        /// </summary>
        public System.Double SumWithDiscountAndReturn { get { return (QuantityWithReturn * PriceWithDiscount); } }

        /// <summary>
        /// Цена отпускная (в валюте учета)
        /// </summary>
        public System.Double PriceInAccountingCurrency { get; set; }
        /// <summary>
        /// Цена отпускная с учетом скидки (в валюте учета)
        /// </summary>
        public System.Double PriceWithDiscountInAccountingCurrency { get; set; }
        /// <summary>
        /// Сумма без учета скидки (в валюте учета)
        /// </summary>
        public System.Double SumInAccountingCurrency { get { return (Quantity * PriceInAccountingCurrency); } }
        /// <summary>
        /// Сумма с учетом скидки (в валюте учета)
        /// </summary>
        public System.Double SumWithDiscountInAccountingCurrency { get { return (Quantity * PriceWithDiscountInAccountingCurrency); } }
        /// <summary>
        /// Сумма с учетом скидки и с учетом возврата (в валюте учета)
        /// </summary>
        public System.Double SumWithDiscountAndReturnInAccountingCurrency { get { return (QuantityWithReturn * PriceWithDiscountInAccountingCurrency); } }
        #endregion

        #region Приложение к накладной
        /// <summary>
        /// Возвращает приложение к накладной
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="uuidWaybillId">уи накладной</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="bTableFromSuppl">признак "приложение запрашивается из заказа"</param>
        /// <returns>приложение к накладной, как список объектов класса CWaybillItem</returns>
        public static List<CWaybillItem> GetWaybillTablePart(UniXP.Common.CProfile objProfile,
            System.Guid uuidWaybillId, ref System.String strErr, System.Boolean bTableFromSuppl = false)
        {
            List<CWaybillItem> objList = new List<CWaybillItem>();

            try
            {
                System.Data.DataTable dtList = CWaybillDataBaseModel.GetWaybilItemTable(objProfile, null, uuidWaybillId, ref strErr, bTableFromSuppl);
                if (dtList != null)
                {
                    CWaybillItem objWaybillItem = null;
                    foreach (System.Data.DataRow objItem in dtList.Rows)
                    {
                        objWaybillItem = new CWaybillItem();

                        objWaybillItem.ID = ((objItem["WaybItem_Guid"] != System.DBNull.Value) ? (System.Guid)objItem["WaybItem_Guid"] : System.Guid.Empty );
                        objWaybillItem.Ib_ID = ((objItem["WaybItem_Id"] != System.DBNull.Value) ? System.Convert.ToInt32(objItem["WaybItem_Id"]) : 0);
                        objWaybillItem.SupplItemID = ((objItem["SupplItem_Guid"] != System.DBNull.Value) ? (System.Guid)objItem["SupplItem_Guid"] : System.Guid.Empty);
                        objWaybillItem.Product = ((objItem["Parts_Guid"] != System.DBNull.Value) ? new CProduct()
                        {
                            ID = (System.Guid)objItem["Parts_Guid"],
                            Name = System.Convert.ToString(objItem["PARTS_NAME"]),
                            Article = System.Convert.ToString(objItem["PARTS_ARTICLE"]),
                            ProductTradeMark = new CProductTradeMark() { Name = System.Convert.ToString(objItem["ProductOwnerName"])}
                        } : null);

                        objWaybillItem.Measure = ((objItem["Measure_Guid"] != System.DBNull.Value) ? new CMeasure()
                        {
                            ID = (System.Guid)objItem["Measure_Guid"], ShortName = System.Convert.ToString(objItem["Measure_ShortName"])
                        } : null);

                        objWaybillItem.Quantity = ((objItem["WaybItem_Quantity"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["WaybItem_Quantity"]) : 0);
                        objWaybillItem.QuantityReturn = ((objItem["WaybItem_RetQuantity"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["WaybItem_RetQuantity"]) : 0);

                        objWaybillItem.NDSPercent = ((objItem["WaybItem_NDSPercent"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["WaybItem_NDSPercent"]) : 0);
                        objWaybillItem.PriceImporter = ((objItem["WaybItem_PriceImporter"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["WaybItem_PriceImporter"]) : 0);
                        objWaybillItem.Price = ((objItem["WaybItem_Price"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["WaybItem_Price"]) : 0);
                        objWaybillItem.DiscountPercent = ((objItem["WaybItem_Discount"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["WaybItem_Discount"]) : 0);
                        objWaybillItem.PriceWithDiscount = ((objItem["WaybItem_DiscountPrice"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["WaybItem_DiscountPrice"]) : 0);
                        objWaybillItem.PriceInAccountingCurrency = ((objItem["WaybItem_CurrencyPrice"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["WaybItem_CurrencyPrice"]) : 0);
                        objWaybillItem.PriceWithDiscountInAccountingCurrency = ((objItem["WaybItem_CurrencyDiscountPrice"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["WaybItem_CurrencyDiscountPrice"]) : 0);


                        if( objWaybillItem != null ) { objList.Add( objWaybillItem ); }

                    }
                }

                dtList = null;

            }
            catch (System.Exception f)
            {
                strErr += (String.Format("\nНе удалось получить приложение к накладной.\nТекст ошибки: {0}", f.Message));
            }
            return objList;
        }

        /// <summary>
        /// Возвращает приложение к накладной
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="uuidWaybillId">уи накладной</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>приложение к накладной, как список структур WaybillItemForExport</returns>
        public static List<WaybillItemForExport> GetWaybillTablePartForExportToExcel(UniXP.Common.CProfile objProfile,
            System.Guid uuidWaybillId, ref System.String strErr)
        {
            List<WaybillItemForExport> objList = new List<WaybillItemForExport>();

            try
            {
                System.Data.DataTable dtList = CWaybillDataBaseModel.GetWaybilItemTableForExportToExcel(objProfile, null, uuidWaybillId, ref strErr);
                if (dtList != null)
                {
                    System.Int32 iWAYBITMS_ID; 
                    System.Int32 iPARTS_ID; 
                    System.String strPARTS_FULLNAME;
                    System.String strOWNER_NAME; 
                    System.String strMEASURE_SHORTNAME; 
                    System.Double dblWAYBITMS_QUANTITY;
                    System.Double dblWAYBITMS_BASEPRICE; 
                    System.String strWAYBITMS_PERCENT; 
                    System.Double dblWAYBITMS_DOUBLEPERCENT;
                    System.Double dblWAYBITMS_TOTALPRICEWITHOUTNDS; 
                    System.Double dblWAYBITMS_NDS; 
                    System.String strWAYBITMS_STRNDS;
                    System.Double dblWAYBITMS_NDSTOTALPRICE; 
                    System.Double dblWAYBITMS_TOTALPRICE; 
                    System.Double dblWAYBITMS_WEIGHT;
                    System.String strWAYBITMS_PLACES; 
                    System.Int32 iWAYBITMS_INTPLACES; 
                    System.String strWAYBITMS_TARA;
                    System.String strWAYBITMS_QTYINPLACE; 
                    System.String strPARTS_CERTIFICATE; 
                    System.String strWAYBITMS_NOTES;
                    System.Double dblWAYBITMS_PRICEWITHOUTNDS; 
                    System.String strCOUNTRY_NAME; 
                    System.Guid uuidParts_Guid;
                    System.String strParts_Barcode;
                    System.String strParts_BarcodeList;

                    foreach (System.Data.DataRow objItem in dtList.Rows)
                    {
                        iWAYBITMS_ID = ((objItem["WAYBITMS_ID"] != System.DBNull.Value) ? System.Convert.ToInt32(objItem["WAYBITMS_ID"]) : 0);
                        iPARTS_ID = ((objItem["PARTS_ID"] != System.DBNull.Value) ? System.Convert.ToInt32(objItem["PARTS_ID"]) : 0);
                        strPARTS_FULLNAME = ((objItem["PARTS_FULLNAME"] != System.DBNull.Value) ? System.Convert.ToString(objItem["PARTS_FULLNAME"]) : System.String.Empty);
                        strOWNER_NAME = ((objItem["OWNER_NAME"] != System.DBNull.Value) ? System.Convert.ToString(objItem["OWNER_NAME"]) : System.String.Empty);
                        strMEASURE_SHORTNAME = ((objItem["MEASURE_SHORTNAME"] != System.DBNull.Value) ? System.Convert.ToString(objItem["MEASURE_SHORTNAME"]) : System.String.Empty);
                        dblWAYBITMS_QUANTITY = ((objItem["WAYBITMS_QUANTITY"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["WAYBITMS_QUANTITY"]) : 0);
                        dblWAYBITMS_BASEPRICE = ((objItem["WAYBITMS_BASEPRICE"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["WAYBITMS_BASEPRICE"]) : 0);
                        strWAYBITMS_PERCENT = ((objItem["WAYBITMS_PERCENT"] != System.DBNull.Value) ? System.Convert.ToString(objItem["WAYBITMS_PERCENT"]) : System.String.Empty);
                        dblWAYBITMS_DOUBLEPERCENT = ((objItem["WAYBITMS_DOUBLEPERCENT"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["WAYBITMS_DOUBLEPERCENT"]) : 0);
                        dblWAYBITMS_TOTALPRICEWITHOUTNDS = ((objItem["WAYBITMS_TOTALPRICEWITHOUTNDS"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["WAYBITMS_TOTALPRICEWITHOUTNDS"]) : 0);
                        dblWAYBITMS_NDS = ((objItem["WAYBITMS_NDS"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["WAYBITMS_NDS"]) : 0);
                        strWAYBITMS_STRNDS = ((objItem["WAYBITMS_STRNDS"] != System.DBNull.Value) ? System.Convert.ToString(objItem["WAYBITMS_STRNDS"]) : System.String.Empty);
                        dblWAYBITMS_NDSTOTALPRICE = ((objItem["WAYBITMS_NDSTOTALPRICE"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["WAYBITMS_NDSTOTALPRICE"]) : 0);
                        dblWAYBITMS_TOTALPRICE = ((objItem["WAYBITMS_TOTALPRICE"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["WAYBITMS_TOTALPRICE"]) : 0);
                        dblWAYBITMS_WEIGHT = ((objItem["WAYBITMS_WEIGHT"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["WAYBITMS_WEIGHT"]) : 0);
                        strWAYBITMS_PLACES = ((objItem["WAYBITMS_PLACES"] != System.DBNull.Value) ? System.Convert.ToString(objItem["WAYBITMS_PLACES"]) : System.String.Empty);
                        iWAYBITMS_INTPLACES = ((objItem["WAYBITMS_INTPLACES"] != System.DBNull.Value) ? System.Convert.ToInt32(objItem["WAYBITMS_INTPLACES"]) : 0);
                        strWAYBITMS_TARA = ((objItem["WAYBITMS_TARA"] != System.DBNull.Value) ? System.Convert.ToString(objItem["WAYBITMS_TARA"]) : System.String.Empty);
                        strWAYBITMS_QTYINPLACE = ((objItem["WAYBITMS_QTYINPLACE"] != System.DBNull.Value) ? System.Convert.ToString(objItem["WAYBITMS_QTYINPLACE"]) : System.String.Empty);
                        strPARTS_CERTIFICATE = ((objItem["PARTS_CERTIFICATE"] != System.DBNull.Value) ? System.Convert.ToString(objItem["PARTS_CERTIFICATE"]) : System.String.Empty);
                        strWAYBITMS_NOTES = ((objItem["WAYBITMS_NOTES"] != System.DBNull.Value) ? System.Convert.ToString(objItem["WAYBITMS_NOTES"]) : System.String.Empty);
                        dblWAYBITMS_PRICEWITHOUTNDS = ((objItem["WAYBITMS_PRICEWITHOUTNDS"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["WAYBITMS_PRICEWITHOUTNDS"]) : 0);
                        strCOUNTRY_NAME = ((objItem["COUNTRY_NAME"] != System.DBNull.Value) ? System.Convert.ToString(objItem["COUNTRY_NAME"]) : System.String.Empty);
                        uuidParts_Guid = ((objItem["Parts_Guid"] != System.DBNull.Value) ? (System.Guid)objItem["Parts_Guid"] : System.Guid.Empty);
                        strParts_Barcode = ((objItem["Parts_Barcode"] != System.DBNull.Value) ? System.Convert.ToString(objItem["Parts_Barcode"]) : System.String.Empty);
                        strParts_BarcodeList = ((objItem["Parts_BarcodeList"] != System.DBNull.Value) ? System.Convert.ToString(objItem["Parts_BarcodeList"]) : System.String.Empty);

                        objList.Add( new WaybillItemForExport( iWAYBITMS_ID, iPARTS_ID, strPARTS_FULLNAME,
                                        strOWNER_NAME, strMEASURE_SHORTNAME, dblWAYBITMS_QUANTITY,
                                        dblWAYBITMS_BASEPRICE, strWAYBITMS_PERCENT, dblWAYBITMS_DOUBLEPERCENT,
                                        dblWAYBITMS_TOTALPRICEWITHOUTNDS, dblWAYBITMS_NDS, strWAYBITMS_STRNDS,
                                        dblWAYBITMS_NDSTOTALPRICE, dblWAYBITMS_TOTALPRICE, dblWAYBITMS_WEIGHT,
                                        strWAYBITMS_PLACES, iWAYBITMS_INTPLACES, strWAYBITMS_TARA,
                                        strWAYBITMS_QTYINPLACE, strPARTS_CERTIFICATE, strWAYBITMS_NOTES,
                                        dblWAYBITMS_PRICEWITHOUTNDS, strCOUNTRY_NAME, uuidParts_Guid,
                                        strParts_Barcode, strParts_BarcodeList  ) );

                    }
                }

                dtList = null;

            }
            catch (System.Exception f)
            {
                strErr += (String.Format("\nНе удалось получить приложение к накладной.\nТекст ошибки: {0}", f.Message));
            }
            return objList;
        }

        /// <summary>
        /// Преобразует список записей к табличному виду
        /// </summary>
        /// <param name="objWaybillItemsList">список строк из приложения к накладной</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>таблица с приложением к накладной</returns>
        public static System.Data.DataTable ConvertListToTable(List<CWaybillItem> objWaybillItemsList, ref System.String strErr)
        {
            System.Data.DataTable objTable = new System.Data.DataTable();
            try
            {
                objTable.Columns.Add(new System.Data.DataColumn("WaybItem_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                objTable.Columns.Add(new System.Data.DataColumn("SupplItem_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                objTable.Columns.Add(new System.Data.DataColumn("Parts_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                objTable.Columns.Add(new System.Data.DataColumn("Measure_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                objTable.Columns.Add(new System.Data.DataColumn("WaybItem_Quantity", typeof(int)));
                objTable.Columns.Add(new System.Data.DataColumn("WaybItem_RetQuantity", typeof(int)));
                objTable.Columns.Add(new System.Data.DataColumn("WaybItem_PriceImporter", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("WaybItem_Price", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("WaybItem_Discount", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("WaybItem_DiscountPrice", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("WaybItem_CurrencyPrice", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("WaybItem_CurrencyDiscountPrice", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("WaybItem_NDSPercent", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("WaybItem_Id", typeof(int)));

                System.Data.DataRow newRow = null;
                foreach (CWaybillItem objItem in objWaybillItemsList)
                {
                    newRow = objTable.NewRow();
                    newRow["WaybItem_Guid"] = objItem.ID;
                    newRow["SupplItem_Guid"] = objItem.SupplItemID;
                    newRow["Parts_Guid"] = objItem.Product.ID;
                    newRow["Measure_Guid"] = objItem.Measure.ID;
                    newRow["WaybItem_Quantity"] = objItem.Quantity;
                    newRow["WaybItem_RetQuantity"] = objItem.QuantityReturn;
                    newRow["WaybItem_PriceImporter"] = (System.Data.SqlTypes.SqlMoney)objItem.PriceImporter;
                    newRow["WaybItem_Price"] = (System.Data.SqlTypes.SqlMoney)objItem.Price;
                    newRow["WaybItem_Discount"] = (System.Data.SqlTypes.SqlMoney)objItem.DiscountPercent;
                    newRow["WaybItem_DiscountPrice"] = (System.Data.SqlTypes.SqlMoney)objItem.PriceWithDiscount;
                    newRow["WaybItem_CurrencyPrice"] = (System.Data.SqlTypes.SqlMoney)objItem.PriceInAccountingCurrency;
                    newRow["WaybItem_CurrencyDiscountPrice"] = (System.Data.SqlTypes.SqlMoney)objItem.PriceWithDiscountInAccountingCurrency;
                    newRow["WaybItem_NDSPercent"] = (System.Data.SqlTypes.SqlMoney)objItem.NDSPercent;
                    newRow["WaybItem_Id"] = objItem.Ib_ID;
                    objTable.Rows.Add(newRow);
                }
                if( objWaybillItemsList.Count > 0 )
                {
                    objTable.AcceptChanges();
                }

            }
            catch (System.Exception f)
            {
                objTable = null;
                strErr += (String.Format("ConvertListToTable. Текст ошибки: {0}", f.Message));
            }
			finally // очищаем занимаемые ресурсы
            {
            }
            return objTable;
        }

        #endregion
    }


    
    /// <summary>
    /// Класс "Расходная накладная (продажа товара клиенту)"
    /// </summary>
    public class CWaybill
    {
        #region Конструктор
        public CWaybill()
        {
            ID = System.Guid.Empty;
            Ib_ID = 0;
            ParentID = System.Guid.Empty;
            SupplID = System.Guid.Empty;
            DocNum = System.String.Empty;
            BeginDate = System.DateTime.MinValue;
            WaybillShipMode = null;
            Customer = null;
            ChildDepart = null;
            SalesMan = null;
            Depart = null;
            Stock = null;
            Company = null;
            WaybillState = null;
            PaymentType = null;
            Currency = null;
            Description = System.String.Empty;
            WaybillItemList = null;
            Quantity = 0;
            PricingCurrencyRate = 0;
            SumWaybill = 0;
            SumDiscount = 0;
            SumWaybillInAccountingCurrency = 0;
            SumDiscountInAccountingCurrency = 0;
        }
        #endregion

        #region Уникальные идентификаторы
        /// <summary>
        /// Уникальный идентификатор накладной
        /// </summary>
        public System.Guid ID { get; set; }
        /// <summary>
        /// Уникальный идентификатор накладной в InterBase
        /// </summary>
        public System.Int32 Ib_ID { get; set; }
        /// <summary>
        /// Уникальный идентификатор накладной-родителя
        /// </summary>
        public System.Guid ParentID { get; set; }
        /// <summary>
        /// Уникальный идентификатор заказа, из которого сформирована накладная
        /// </summary>
        public System.Guid SupplID { get; set; }
        /// <summary>
        /// Номер накладной
        /// </summary>
        public System.String DocNum { get; set; }
        /// <summary>
        /// Дата создания накладной
        /// </summary>
        public System.DateTime BeginDate { get; set; }
        #endregion

        #region Доставка
        /// <summary>
        /// Дата доставки накладной
        /// </summary>
        public System.DateTime DeliveryDate { get; set; }
        /// <summary>
        /// Розничная торговая точка
        /// </summary>
        public CRtt Rtt { get; set; }
        /// <summary>
        /// Адрес доставки
        /// </summary>
        public CAddress AddressDelivery { get; set; }
        /// <summary>
        /// Признак "Отображать в листе доставки"
        /// </summary>
        public System.Boolean ShowInDeliveryList { get; set; }
        #endregion

        #region Отгрузка
        /// <summary>
        /// Дата отгрузки накладной
        /// </summary>
        public System.DateTime ShipDate { get; set; }
        /// <summary>
        /// Вид отгрузки накладной
        /// </summary>
        public CWaybillShipMode WaybillShipMode { get; set; }
        /// <summary>
        /// Вид отгрузки накладной
        /// </summary>
        public System.String WaybillShipModeName
        {
            get { return ((WaybillShipMode == null) ? System.String.Empty : WaybillShipMode.Name); }
        }
        /// <summary>
        /// Признак "Заказ является бонусом"
        /// </summary>
        public System.Boolean IsBonus { get; set; }
        #endregion

        #region Клиент
        /// <summary>
        /// Клиент
        /// </summary>
        public CCustomer Customer { get; set; }
        /// <summary>
        /// Наименование клиента
        /// </summary>
        public System.String CustomerName
        {
            get { return ((Customer == null) ? System.String.Empty : ((Customer.StateType == null) ? Customer.FullName : (Customer.FullName + " " + Customer.StateType.ShortName))); }
        }
        /// <summary>
        /// Дочернее подразделение
        /// </summary>
        public CChildDepart ChildDepart { get; set; }
        /// <summary>
        /// Код дочернего подразделения
        /// </summary>
        public System.String ChildDepartCode
        {
            get { return ((ChildDepart == null) ? System.String.Empty : ChildDepart.Code); }
        }
        #endregion

        #region Торговый представитель
        /// <summary>
        /// Торговый представитель
        /// </summary>
        public CSalesMan SalesMan { get; set; }
        /// <summary>
        /// Фамилия торгового представителя
        /// </summary>
        public System.String SalesManName
        {
            get { return (((SalesMan == null) || (SalesMan.User == null)) ? "" : SalesMan.User.LastName); }
        }
        /// <summary>
        /// Подразделение
        /// </summary>
        public CDepart Depart { get; set; }
        /// <summary>
        /// Код подразделения
        /// </summary>
        public System.String DepartCode
        {
            get { return ((Depart == null) ? "" : Depart.DepartCode); }
        }
        #endregion

        #region Склад
        /// <summary>
        /// Склад
        /// </summary>
        public CStock Stock { get; set; }
        /// <summary>
        /// Наименование склада
        /// </summary>
        public System.String StockName
        {
            get { return ((Stock == null) ? System.String.Empty : Stock.Name); }
        }
        /// <summary>
        /// Компания
        /// </summary>
        public CCompany Company { get; set; }
        /// <summary>
        /// Сокращенное наименование компании
        /// </summary>
        public System.String CompanyAcronym
        {
            get { return ((Company == null) ? System.String.Empty : Company.Abbr); }
        }
        #endregion

        #region Состояние документа
        /// <summary>
        /// Состояние накладной
        /// </summary>
        public CWaybillState WaybillState { get; set; }
        /// <summary>
        /// Состояние накладной
        /// </summary>
        public System.String WaybillStateName
        {
            get { return ((WaybillState == null) ? System.String.Empty : WaybillState.Name); }
        }
        #endregion

        #region Форма оплаты
        /// <summary>
        /// Форма оплаты
        /// </summary>
        public CPaymentType PaymentType { get; set; }
        /// <summary>
        /// Наименование формы оплаты
        /// </summary>
        public System.String PaymentTypeName
        {
            get { return ((PaymentType == null) ? System.String.Empty : PaymentType.Name); }
        }
        #endregion

        #region Итоговые суммы, количество, вес
        /// <summary>
        /// Количество проданное
        /// </summary>
        public System.Double Quantity { get; set; }
        /// <summary>
        /// Количество возвращенное
        /// </summary>
        public System.Double QuantityReturn { get; set; }
        /// <summary>
        /// Количество проданное с учетом возврата
        /// </summary>
        public System.Double QuantityWithReturn
        {
            get { return (Quantity - QuantityReturn); }
        }
        /// <summary>
        /// Валюта
        /// </summary>
        public CCurrency Currency { get; set; }
        /// <summary>
        /// Валюта
        /// </summary>
        public System.String CurrencyCode
        {
            get { return ((Currency == null) ? System.String.Empty : Currency.CurrencyAbbr); }
        }
        /// <summary>
        /// Курс ценообразования на дату формаирования накладной
        /// </summary>
        public System.Double PricingCurrencyRate { get; set; }
        /// <summary>
        /// Сумма накладной без учета скидки (в национальной валюте)
        /// </summary>
        public System.Double SumWaybill { get; set; }
        /// <summary>
        /// Сумма скидки по накладной (в национальной валюте)
        /// </summary>
        public System.Double SumDiscount { get; set; }
        /// <summary>
        /// Сумма накладной с учетом скидки (в национальной валюте)
        /// </summary>
        public System.Double SumWithDiscount { get { return (SumWaybill - SumDiscount); } }
        /// <summary>
        /// Сумма оплаты по накладной (в национальной валюте)
        /// </summary>
        public System.Double SumPayment { get; set; }
        /// <summary>
        /// Сумма возврата по накладной (в национальной валюте)
        /// </summary>
        public System.Double SumReturn { get; set; }
        /// <summary>
        /// Сумма текущей задолженности по накладной (в национальной валюте)
        /// </summary>
        public System.Double SumSaldo { get { return (SumPayment - ( SumWaybill - SumDiscount - SumReturn ) ); } }
        /// <summary>
        /// Сумма накладной без учета скидки (в валюте учета)
        /// </summary>
        public System.Double SumWaybillInAccountingCurrency { get; set; }
        /// <summary>
        /// Сумма скидки по накладной (в валюте учета)
        /// </summary>
        public System.Double SumDiscountInAccountingCurrency { get; set; }
        /// <summary>
        /// Сумма накладной с учетом скидки (в валюте учета)
        /// </summary>
        public System.Double SumWithDiscountInAccountingCurrency { get { return (SumWaybillInAccountingCurrency - SumDiscountInAccountingCurrency); } }
        /// <summary>
        /// Сумма оплаты по накладной (в валюте учета)
        /// </summary>
        public System.Double SumPaymentInAccountingCurrency { get; set; }
        /// <summary>
        /// Сумма возврата по накладной (в валюте учета)
        /// </summary>
        public System.Double SumReturnInAccountingCurrency { get; set; }
        /// <summary>
        /// Сумма текущей задолженности по накладной (в валюте учета)
        /// </summary>
        public System.Double SumSaldoInAccountingCurrency { get { return ( SumPaymentInAccountingCurrency - (SumWaybillInAccountingCurrency - SumDiscountInAccountingCurrency - SumReturnInAccountingCurrency ) ); } }
        /// <summary>
        /// Вес товара в накладной
        /// </summary>
        public System.Double Weight { get; set; }
        #endregion

        #region Дополнительная информация
        /// <summary>
        /// Примечание
        /// </summary>
        public System.String Description { get; set; }
        #endregion

        #region Табличная часть
        /// <summary>
        /// Приложение к накладной
        /// </summary>
        public List<CWaybillItem> WaybillItemList { get; set; }
        #endregion

        #region Журнал накладных
        /// <summary>
        /// Возвращает список накладных за указанный период
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="Waybill_Guid">УИ документа</param>
        /// <param name="SelectWaybillInfoFromSuppl">признак "информация для накладной запрашивается из заказа"</param>
        /// <param name="dtBeginDate">начало периода для выборки</param>
        /// <param name="dtEndDate">конец периода для выборки</param>
        /// <param name="uuidCompanyId">УИ компании</param>
        /// <param name="uuidStockId">УИ склада</param>
        /// <param name="uuidPaymentTypeId">УИ формы оплаты</param>
        /// <param name="uuidCustomerId">УИ клиента</param>
        /// <param name="strErr">текст ошибки</param>
        /// <param name="OnlyUnShippedWaybills">признак "только не отгруженные накладные"</param>
        /// <returns>список объектов класса "CWaybill"</returns>
        public static List<CWaybill> GetWaybillList(UniXP.Common.CProfile objProfile, System.Guid Waybill_Guid, System.Boolean SelectWaybillInfoFromSuppl,
            System.DateTime dtBeginDate, System.DateTime dtEndDate,
            System.Guid uuidCompanyId, System.Guid uuidStockId,
            System.Guid uuidPaymentTypeId, System.Guid uuidCustomerId,
            ref System.String strErr, System.Boolean OnlyUnShippedWaybills = false)
        {
            List<CWaybill> objList = new List<CWaybill>();

            try
            {
                // вызов статического метода из класса, связанного с БД
                System.Data.DataTable dtList = CWaybillDataBaseModel.GetWaybillTable(objProfile, null, Waybill_Guid, dtBeginDate, dtEndDate,
                    uuidCompanyId, uuidStockId, uuidPaymentTypeId, uuidCustomerId, ref strErr, SelectWaybillInfoFromSuppl, OnlyUnShippedWaybills);
                if (dtList != null)
                {
                    CWaybill objWaybill = null;
                    System.Int32 objWaybill_Ib_ID = 0;
                    foreach (System.Data.DataRow objItem in dtList.Rows)
                    {
                        objWaybill = new CWaybill();
                        objWaybill_Ib_ID = System.Convert.ToInt32(objItem["Waybill_Id"]);
                        objWaybill.ID = ((objItem["Waybill_Guid"] != System.DBNull.Value) ? new System.Guid(System.Convert.ToString(objItem["Waybill_Guid"])) : System.Guid.Empty);
                        objWaybill.ParentID = ((objItem["WaybillParent_Guid"] != System.DBNull.Value) ? new System.Guid(System.Convert.ToString(objItem["WaybillParent_Guid"])) : System.Guid.Empty);
                        objWaybill.SupplID = ((objItem["Suppl_Guid"] != System.DBNull.Value) ? new System.Guid(System.Convert.ToString(objItem["Suppl_Guid"])) : System.Guid.Empty);
                        objWaybill.Ib_ID = ((objItem["Waybill_Id"] != System.DBNull.Value) ? System.Convert.ToInt32(objItem["Waybill_Id"]) : 0);
                        objWaybill.Stock = ((objItem["Stock_Guid"] != System.DBNull.Value) ? new CStock()
                        {
                            ID = (System.Guid)objItem["Stock_Guid"],
                            IBId = System.Convert.ToInt32(objItem["Stock_Id"]),
                            Name = System.Convert.ToString(objItem["Stock_Name"]),
                            IsAcitve = System.Convert.ToBoolean(objItem["Stock_IsActive"]),
                            IsTrade = System.Convert.ToBoolean(objItem["Stock_IsTrade"]),
                            WareHouse = new CWarehouse() { ID = (System.Guid)objItem["Warehouse_Guid"] },
                            WareHouseType = new CWareHouseType() { ID = (System.Guid)objItem["WarehouseType_Guid"] }
                        } : null);
                        objWaybill.Company = ((objItem["Company_Guid"] != System.DBNull.Value) ? new CCompany()
                        {
                            ID = (System.Guid)objItem["Company_Guid"],
                            InterBaseID = System.Convert.ToInt32(objItem["Company_Id"]),
                            Abbr = System.Convert.ToString(objItem["Company_Acronym"]),
                            Name = System.Convert.ToString(objItem["Company_Name"])
                        } : null);

                        objWaybill.Customer = ((objItem["Customer_Guid"] != System.DBNull.Value) ? new CCustomer()
                        {
                            ID = (System.Guid)objItem["Customer_Guid"],
                            InterBaseID = System.Convert.ToInt32(objItem["Customer_Id"]),
                            ShortName = System.Convert.ToString(objItem["Customer_Name"]),
                            FullName = System.Convert.ToString(objItem["Customer_Name"])
                        } : null);

                        objWaybill.Customer.StateType = ((objItem["CustomerStateType_Guid"] != System.DBNull.Value) ? new CStateType()
                        {
                            ID = (System.Guid)objItem["CustomerStateType_Guid"],
                            ShortName = System.Convert.ToString(objItem["CustomerStateType_ShortName"])
                        } : null);

                        objWaybill.Currency = ((objItem["Currency_Guid"] != System.DBNull.Value) ? new CCurrency()
                        {
                            ID = (System.Guid)objItem["Currency_Guid"],
                            CurrencyAbbr = System.Convert.ToString(objItem["Currency_Abbr"])
                        } : null);

                        objWaybill.Depart = ((objItem["Depart_Guid"] != System.DBNull.Value) ? new CDepart()
                        {
                            uuidID = (System.Guid)objItem["Depart_Guid"],
                            DepartCode = System.Convert.ToString(objItem["Depart_Code"])
                        } : null);

                        objWaybill.ChildDepart = ((objItem["ChildDepart_Guid"] != System.DBNull.Value) ? new CChildDepart()
                        {
                            ID = (System.Guid)objItem["ChildDepart_Guid"],
                            Code = System.Convert.ToString(objItem["ChildDepart_Code"]),
                            Name = System.Convert.ToString(objItem["ChildDepart_Name"])
                        } : null);

                        objWaybill.Rtt = ((objItem["Rtt_Guid"] != System.DBNull.Value) ? new CRtt()
                        {
                            ID = (System.Guid)objItem["Rtt_Guid"],
                            ShortName = System.Convert.ToString(objItem["Rtt_Name"]),
                            FullName = System.Convert.ToString(objItem["Rtt_Name"])
                        } : null);

                        objWaybill.AddressDelivery = ((objItem["Address_Guid"] != System.DBNull.Value) ? new CAddress()
                        {
                            ID = (System.Guid)objItem["Address_Guid"],
                            Name = System.Convert.ToString(objItem["Address_FullName"])
                        } : null);

                        objWaybill.PaymentType = ((objItem["PaymentType_Guid"] != System.DBNull.Value) ? new CPaymentType(
                            (System.Guid)objItem["PaymentType_Guid"], System.Convert.ToString(objItem["PaymentType_Name"])) : null);

                        objWaybill.BeginDate = ((objItem["Waybill_BeginDate"] != System.DBNull.Value) ? System.Convert.ToDateTime(objItem["Waybill_BeginDate"]) : System.DateTime.MinValue);
                        if (objItem["Waybill_ShipDate"] != System.DBNull.Value)
                        {
                            objWaybill.ShipDate = System.Convert.ToDateTime(objItem["Waybill_ShipDate"]);
                        }
                        //objWaybill.ShipDate = ((objItem["Waybill_ShipDate"] != System.DBNull.Value) ? System.Convert.ToDateTime(objItem["Waybill_ShipDate"]) : System.DateTime.MinValue);
                        objWaybill.DeliveryDate = ((objItem["Waybill_DeliveryDate"] != System.DBNull.Value) ? System.Convert.ToDateTime(objItem["Waybill_DeliveryDate"]) : System.DateTime.MinValue);
                        objWaybill.DocNum = ((objItem["Waybill_Num"] != System.DBNull.Value) ? System.Convert.ToString(objItem["Waybill_Num"]) : System.String.Empty);
                        objWaybill.IsBonus = ((objItem["Waybill_Bonus"] != System.DBNull.Value) ? System.Convert.ToBoolean(objItem["Waybill_Bonus"]) : false);

                        objWaybill.WaybillState = ((objItem["WaybillState_Guid"] != System.DBNull.Value) ? new CWaybillState()
                        {
                            ID = (System.Guid)objItem["WaybillState_Guid"],
                            WaybillStateId = System.Convert.ToInt32(objItem["WaybillState_Id"]),
                            Name = System.Convert.ToString(objItem["WaybillState_Name"])
                        } : null);

                        objWaybill.WaybillShipMode = ((objItem["WaybillShipMode_Guid"] != System.DBNull.Value) ? new CWaybillShipMode()
                        {
                            ID = (System.Guid)objItem["WaybillShipMode_Guid"],
                            WaybillShipModeId = System.Convert.ToInt32(objItem["WaybillShipMode_Id"]),
                            Name = System.Convert.ToString(objItem["WaybillShipMode_Name"])
                        } : null);

                        objWaybill.Description = ((objItem["Waybill_Description"] != System.DBNull.Value) ? System.Convert.ToString(objItem["Waybill_Description"]) : System.String.Empty);

                        objWaybill.SumWaybill = ((objItem["Waybill_AllPrice"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["Waybill_AllPrice"]) : 0);
                        objWaybill.SumReturn = ((objItem["Waybill_RetAllPrice"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["Waybill_RetAllPrice"]) : 0);
                        objWaybill.SumDiscount = ((objItem["Waybill_AllDiscount"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["Waybill_AllDiscount"]) : 0);
                        objWaybill.SumPayment = ((objItem["Waybill_AmountPaid"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["Waybill_AmountPaid"]) : 0);

                        objWaybill.SumWaybillInAccountingCurrency = ((objItem["Waybill_CurrencyAllPrice"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["Waybill_CurrencyAllPrice"]) : 0);
                        objWaybill.SumReturnInAccountingCurrency = ((objItem["Waybill_CurrencyRetAllPrice"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["Waybill_CurrencyRetAllPrice"]) : 0);
                        objWaybill.SumDiscountInAccountingCurrency = ((objItem["Waybill_CurrencyAllDiscount"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["Waybill_CurrencyAllDiscount"]) : 0);
                        objWaybill.SumPaymentInAccountingCurrency = ((objItem["Waybill_CurrencyAmountPaid"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["Waybill_CurrencyAmountPaid"]) : 0);

                        objWaybill.Quantity = ((objItem["Waybill_Quantity"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["Waybill_Quantity"]) : 0);
                        objWaybill.QuantityReturn = ((objItem["Waybill_RetQuantity"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["Waybill_RetQuantity"]) : 0);
                        objWaybill.Weight = ((objItem["Waybill_Weight"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["Waybill_Weight"]) : 0);
                        objWaybill.ShowInDeliveryList = ((objItem["Waybill_ShowInDeliveryList"] != System.DBNull.Value) ? System.Convert.ToBoolean(objItem["Waybill_ShowInDeliveryList"]) : false);

                        if (objWaybill != null) { objList.Add(objWaybill); }

                    }
                }

                dtList = null;

            }
            catch (System.Exception f)
            {
                strErr += (String.Format("\nНе удалось получить список накладных.\nТекст ошибки: {0}", f.Message));
            }
            return objList;
        }
        /// <summary>
        /// Возвращает объект класса "Накладная"
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="Waybill_Guid">УИ накладной</param>
        /// <param name="strErr">текст ошибки</param>
        /// <returns>объект класса "Накладная"</returns>
        public static CWaybill GetWaybill(UniXP.Common.CProfile objProfile, System.Guid Waybill_Guid,
            ref System.String strErr)
        {
            CWaybill objWaybill = null;

            try
            {
                // вызов статического метода из класса, связанного с БД
                List<CWaybill> objList = CWaybill.GetWaybillList(objProfile, Waybill_Guid, false,
                    System.DateTime.MinValue, System.DateTime.MinValue,
                    System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, ref strErr);
                if ((objList != null) && (objList.Count > 0))
                {
                    objWaybill = objList[0];
                }
                objList = null;

            }
            catch (System.Exception f)
            {
                strErr += (String.Format("\nНе удалось получить информацию о накладной.\nТекст ошибки: {0}", f.Message));
            }
            return objWaybill;
        }
        
        #endregion

        #region Формирование накладной из заказа
        /// <summary>
        /// Формирует накладную из заказа
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="Suppl_Guid">УИ заказа</param>
        /// <param name="Document_Date">дата накладной</param>
        /// <param name="Document_Num">номер накладной</param>
        /// <param name="DocumentSendToStock">признак "для склада"</param>
        /// <param name="Waybill_Guid">УИ накладной</param>
        /// <param name="SupplState_Guid">УИ состояния заказа</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - накладная успешно сформирована; false - ошибка</returns>
        public static System.Boolean CreateWaybillFromSuppl(UniXP.Common.CProfile objProfile,
            System.Guid Suppl_Guid, System.DateTime Document_Date,
            System.String Document_Num, System.Boolean DocumentSendToStock,
            ref System.Guid Waybill_Guid, ref System.Guid SupplState_Guid, ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                System.Int32 iRet = CWaybillDataBaseModel.CreateWaybillFromSuppl( objProfile, Suppl_Guid, Document_Date, Document_Num, DocumentSendToStock, 
                    ref Waybill_Guid, ref SupplState_Guid, ref strErr );

                bRet = (iRet == 0);
            }
            catch (System.Exception f)
            {
                strErr = "Не удалось сформировать накладную.\n\nТекст ошибки: " + f.Message;
            }
            return bRet;
        }
        #endregion

        #region Ссылка на накладную для заказа
        /// <summary>
        /// Возвращает ссылку на накладную для заказа
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="Suppl_Guid">УИ заказа</param>
        /// <param name="strErr">текст ошибки</param>
        /// <returns>УИ накладной</returns>
        public static System.Guid GetWaybillGuidForSuppl(UniXP.Common.CProfile objProfile,
            System.Guid Suppl_Guid, ref System.String strErr)
        {
            return CWaybillDataBaseModel.GetWaybillGuidForSuppl(objProfile, Suppl_Guid, ref strErr);
        }
        #endregion

        #region Проверка, можно ли перевести заказ в накладную
        /// <summary>
        /// Проверка, можно ли перевести заказ в накладную
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="Suppl_Guid">УИ заказа</param>
        /// <param name="strErr">текст ошибки</param>
        /// <returns>true - можно; false - нельзя</returns>
        public static System.Boolean CanCreateWaybillFromSuppl(UniXP.Common.CProfile objProfile,
            System.Guid Suppl_Guid, ref System.String strErr)
        {
            return CWaybillDataBaseModel.CanCreateWaybillFromSuppl( objProfile, Suppl_Guid, ref strErr);
        }
        #endregion

        #region Создание новой накладной
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Suppl_Guid">УИ заказа</param>
        /// <param name="Stock_Guid">УИ склада отгрузки</param>
        /// <param name="Company_Guid">УИ компании</param>
        /// <param name="Depart_Guid">УИ торгового подразделения</param>
        /// <param name="Customer_Guid">УИ клиента</param>
        /// <param name="Rtt_Guid">УИ РТТ</param>
        /// <param name="Address_Guid">УИ адреса доставки</param>
        /// <param name="PaymentType_Guid">УИ формы оплаты</param>
        /// <param name="Waybill_Num">номер накладной</param>
        /// <param name="Waybill_BeginDate">дата накладной</param>
        /// <param name="Waybill_DeliveryDate">дата доставки</param>
        /// <param name="WaybillShipMode_Guid">УИ вида отгрузки</param>
        /// <param name="WaybillTablePart">приложение к накладной (товары)</param>
        /// <param name="strErr">текст ошибки</param>
        /// <returns>true - значения всех полей удовлетворяют требованиям; false - проверка не пройдена</returns>
        public static System.Boolean CheckAllPropertiesForSave(
            System.Guid Suppl_Guid, System.Guid Stock_Guid, System.Guid Company_Guid, System.Guid Depart_Guid,
            System.Guid Customer_Guid, System.Guid Rtt_Guid, System.Guid Address_Guid, 
            System.Guid PaymentType_Guid, System.String Waybill_Num,
            System.DateTime Waybill_BeginDate, System.DateTime Waybill_DeliveryDate,  
            System.Guid WaybillShipMode_Guid, 
            System.Data.DataTable WaybillTablePart, ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (Waybill_BeginDate == System.DateTime.MinValue)
                {
                    strErr = "Укажите, пожалуйста, дату накладной.";
                    return bRet;
                }
                if ((Waybill_DeliveryDate == System.DateTime.MinValue) || (System.DateTime.Compare(Waybill_DeliveryDate, Waybill_BeginDate) < 0))
                {
                    strErr = "Дата доставки не может быть раньше даты накладной.\nПроверьте, пожалуйста, дату доставки.";
                    return bRet;
                }
                if (Depart_Guid == System.Guid.Empty)
                {
                    strErr = "Укажите, пожалуйста, подразделение.";
                    return bRet;
                }
                if (Stock_Guid == System.Guid.Empty)
                {
                    strErr = "Укажите, пожалуйста, склад отгрузки.";
                    return bRet;
                }
                if (Company_Guid == System.Guid.Empty)
                {
                    strErr = "Укажите, пожалуйста, компанию.";
                    return bRet;
                }
                if (Suppl_Guid == System.Guid.Empty)
                {
                    strErr = "Укажите, пожалуйста, заказ, на основании которого создается накладная.";
                    return bRet;
                }
                if (Customer_Guid == System.Guid.Empty)
                {
                    strErr = "Укажите, пожалуйста, клиента.";
                    return bRet;
                }
                if (PaymentType_Guid == System.Guid.Empty)
                {
                    strErr = "Укажите, пожалуйста, форму оплаты.";
                    return bRet;
                }
                if (Rtt_Guid == System.Guid.Empty)
                {
                    strErr = "Укажите, пожалуйста, розничную точку.";
                    return bRet;
                }
                if (Address_Guid == System.Guid.Empty)
                {
                    strErr = "Укажите, пожалуйста, адрес доставки.";
                    return bRet;
                }
                if (WaybillShipMode_Guid == System.Guid.Empty)
                {
                    strErr = "Укажите, пожалуйста, вид отгрузки.";
                    return bRet;
                }
                if (Waybill_Num.Trim().Length == 0)
                {
                    strErr = "Укажите, пожалуйста, номер накладной.";
                    return bRet;
                }
                if ((WaybillTablePart == null) || (WaybillTablePart.Rows.Count == 0))
                {
                    strErr = "Приложение к накладной не содержит записей. Добавьте, пожалуйста, хотя бы одну позицию.";
                    return bRet;
                }

                bRet = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "CheckAllPropertiesForSave.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return bRet;
        }
        /// <summary>
        /// Сохраняет новую накладную
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="Suppl_Guid">УИ заказа</param>
        /// <param name="Stock_Guid">УИ склада отгрузки</param>
        /// <param name="Company_Guid">УИ компании</param>
        /// <param name="Depart_Guid">УИ торгового подразделения</param>
        /// <param name="Customer_Guid">УИ клиента</param>
        /// <param name="CustomerChild_Guid">УИ дочернего клиента</param>
        /// <param name="Rtt_Guid">УИ РТТ</param>
        /// <param name="Address_Guid">УИ адреса доставки</param>
        /// <param name="PaymentType_Guid">УИ формы оплаты</param>
        /// <param name="Waybill_Num">номер накладной</param>
        /// <param name="Waybill_BeginDate">дата накладной</param>
        /// <param name="Waybill_DeliveryDate">дата доставки</param>
        /// <param name="WaybillParent_Guid">УИ родительской накладной</param>
        /// <param name="Waybill_Bonus">признак "бонусная отгрузка</param>
        /// <param name="WaybillState_Guid">УИ состояния накладной</param>
        /// <param name="WaybillShipMode_Guid">УИ вида отгрузки</param>
        /// <param name="Waybill_ShipDate">дата отгрузки</param>
        /// <param name="Waybill_Description">примечание</param>
        /// <param name="Waybill_CurrencyRate">курс ценообразования</param>
        /// <param name="Waybill_ShowInDeliveryList">признак "отображать в листе доставки"</param>
        /// <param name="WaybillTablePart">приложение к накладной (товары)</param>
        /// <param name="Waybill_Guid">УИ накладной</param>
        /// <param name="Waybill_Id">УИ накладной в InterBase</param>
        /// <param name="SupplState_Guid">УИ текущего состояния заказа</param>
        /// <param name="strErr">текст ошибки</param>
        /// <param name="SetWaybillInQueue">признак "поместить в очередь для обработки"</param>
        /// <param name="DocumentSendToStock">признак "уведомить склад"</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean AddNewWaybillToDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid Suppl_Guid, System.Guid Stock_Guid, System.Guid Company_Guid, System.Guid Depart_Guid,
            System.Guid Customer_Guid, System.Guid CustomerChild_Guid, System.Guid Rtt_Guid, System.Guid Address_Guid,
            System.Guid PaymentType_Guid, System.String Waybill_Num,
            System.DateTime Waybill_BeginDate, System.DateTime Waybill_DeliveryDate, System.Guid WaybillParent_Guid, System.Boolean Waybill_Bonus,
            System.Guid WaybillState_Guid, System.Guid WaybillShipMode_Guid, System.DateTime Waybill_ShipDate,
            System.String Waybill_Description, System.Double Waybill_CurrencyRate, System.Boolean Waybill_ShowInDeliveryList,
            System.Data.DataTable WaybillTablePart,
            ref System.Guid Waybill_Guid, ref System.Int32 Waybill_Id, ref System.Guid SupplState_Guid, ref System.String strErr,
            System.Boolean SetWaybillInQueue = false, System.Boolean DocumentSendToStock = false)
        {
            System.Boolean bRet = false;
            try
            {
                bRet = CWaybillDataBaseModel.AddNewWaybillToDB(objProfile, null,
                    Suppl_Guid, Stock_Guid, Company_Guid, Depart_Guid,
                    Customer_Guid, CustomerChild_Guid, Rtt_Guid, Address_Guid,
                    PaymentType_Guid, Waybill_Num,
                    Waybill_BeginDate, Waybill_DeliveryDate, WaybillParent_Guid, Waybill_Bonus,
                    WaybillState_Guid,  WaybillShipMode_Guid,  Waybill_ShipDate,
                    Waybill_Description, Waybill_CurrencyRate, Waybill_ShowInDeliveryList,
                    WaybillTablePart, ref Waybill_Guid, ref Waybill_Id, ref SupplState_Guid, ref strErr, 
                    SetWaybillInQueue, DocumentSendToStock);
            }
            catch (System.Exception f)
            {
                strErr += ("\n" + f.Message);
            }
            finally
            {
            }
            return bRet;
        }


        #endregion

        #region Объединение накладных
        /// <summary>
        /// Преобразует список записей к табличному виду
        /// </summary>
        /// <param name="objChildWaybillList">список уникальных идентификаторов</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>таблица</returns>
        public static System.Data.DataTable ConvertWaybillGuidListToTable(List<System.Guid> objChildWaybillList, ref System.String strErr)
        {
            System.Data.DataTable objTable = new System.Data.DataTable();
            try
            {
                objTable.Columns.Add(new System.Data.DataColumn("Item_Guid", typeof(System.Data.SqlTypes.SqlGuid)));

                System.Data.DataRow newRow = null;
                foreach (System.Guid ItemID in objChildWaybillList)
                {
                    newRow = objTable.NewRow();
                    newRow["Item_Guid"] = ItemID;
                    objTable.Rows.Add(newRow);
                }
                if (objChildWaybillList.Count > 0)
                {
                    objTable.AcceptChanges();
                }

            }
            catch (System.Exception f)
            {
                objTable = null;
                strErr += (String.Format("ConvertWaybillGuidListToTable. Текст ошибки: {0}", f.Message));
            }
			finally // очищаем занимаемые ресурсы
            {
            }
            return objTable;
        }

        /// <summary>
        /// Проверка значений перед объединением накладных
        /// </summary>
        /// <param name="MainWaybill_Guid">УИ "главной" накладной</param>
        /// <param name="MainDepart_Guid">УИ подразделения "главной" накладной</param>
        /// <param name="MainWaybill_Num">УИ накладной "главной" накладной</param>
        /// <param name="ChildWaybillList">список идентификаторов "дочерних" накладных</param>
        /// <param name="strErr">текст ошибки</param>
        /// <returns>true - проверка пройдена; false - проверка не пройдена</returns>
        public static System.Boolean CheckAllPropertiesBeforeUnionWaybill(
            System.Guid MainWaybill_Guid, System.Guid MainDepart_Guid, System.String MainWaybill_Num,
            System.Data.DataTable ChildWaybillList, ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (MainWaybill_Guid.CompareTo(System.Guid.Empty) == 0)
                {
                    strErr = "Укажите, пожалуйста, \"основную\" накладную.";
                    return bRet;
                }
                if (MainDepart_Guid.CompareTo(System.Guid.Empty) == 0)
                {
                    strErr = "Укажите, пожалуйста, подразделение.";
                    return bRet;
                }
                if (MainWaybill_Num.Trim().Length == 0)
                {
                    strErr = "Укажите, пожалуйста, номер накладной.";
                    return bRet;
                }
                if ((ChildWaybillList == null) || (ChildWaybillList.Rows.Count == 0))
                {
                    strErr = "Список накладных для объединения не содержит записей. Добавьте, пожалуйста, хотя бы одну позицию.";
                    return bRet;
                }

                bRet = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "CheckAllPropertiesBeforeUnionWaybill.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return bRet;
        }
        /// <summary>
        /// Объединение накладных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="MainWaybill_Guid">УИ "главной" накладной</param>
        /// <param name="MainDepart_Guid">УИ подразделения "главной" накладной</param>
        /// <param name="MainWaybill_Num">УИ накладной "главной" накладной</param>
        /// <param name="ChildWaybillList">список идентификаторов "дочерних" накладных</param>
        /// <param name="strErr">текст ошибки</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean JoinWaybillInDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid MainWaybill_Guid, System.Guid MainDepart_Guid, System.String MainWaybill_Num,
            List<System.Guid> ChildWaybillList,  ref System.String strErr )
        {
            System.Boolean bRet = false;
            try
            {
                System.Data.DataTable dtChildWaybillList = ConvertWaybillGuidListToTable(ChildWaybillList, ref strErr);

                if (dtChildWaybillList != null)
                {
                    if (CheckAllPropertiesBeforeUnionWaybill(MainWaybill_Guid, MainDepart_Guid, MainWaybill_Num, dtChildWaybillList, ref strErr) == true)
                    {
                        bRet = CWaybillDataBaseModel.AddJoinDepartToDB(objProfile, null,
                            MainWaybill_Guid, MainDepart_Guid, MainWaybill_Num, dtChildWaybillList, ref strErr);
                    }
                }
            }
            catch (System.Exception f)
            {
                strErr += ("\n" + f.Message);
            }
            finally
            {
            }
            return bRet;
        }

        #endregion

        #region Проверка, можно ли аннулировать накладную
        /// <summary>
        /// Проверка, можно ли аннулировать накладную
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="Waybill_Guid">УИ накладной</param>
        /// <param name="strErr">текст ошибки</param>
        /// <returns>true - можно; false - нельзя</returns>
        public static System.Boolean CanCancelWaybill(UniXP.Common.CProfile objProfile,
            System.Guid Waybill_Guid, ref System.String strErr)
        {
            return CWaybillDataBaseModel.CanCancelWaybill(objProfile, Waybill_Guid, ref strErr);
        }
        #endregion

        #region Аннулирование накладной
        /// <summary>
        /// Аннулирование накладной
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="Waybill_Guid">УИ накладной</param>
        /// <param name="WaybillState_Guid">УИ состояния накладной</param>
        /// <param name="strErr">текст ошибки</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean CancelWaybill(UniXP.Common.CProfile objProfile,
            System.Guid Waybill_Guid, ref System.Guid WaybillState_Guid, ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                bRet = CWaybillDataBaseModel.CancelWaybill(objProfile, null,
                    Waybill_Guid, ref WaybillState_Guid, ref strErr);
            }
            catch (System.Exception f)
            {
                strErr += ("\n" + f.Message);
            }
            finally
            {
            }
            return bRet;
        }
        #endregion

        #region Отгрузка накладной
        /// <summary>
        /// Отгрузка товара по накладной
        /// </summary>
        /// <param name="objProfile">Профайл</param>
        /// <param name="Waybill_Guid">УИ накладной на отгрузку</param>
        /// <param name="Waybill_ShipDate">Дата отгрузки</param>
        /// <param name="SetWaybillShipMode_Guid">УИ варианта отгрузки</param>
        /// <param name="ShipDescription">Примечание</param>
        /// <param name="WaybillState_Guid">УИ текущего состояния накладной</param>
        /// <param name="iErr">целочисленный код ошибки</param>
        /// <param name="strErr">текст ошибки</param>
        /// <returns>0 - накладная отгружена; <>0 - ошибка</returns>
        public static System.Int32 ShippedProductsByWaybill(UniXP.Common.CProfile objProfile, 
                    System.Guid Waybill_Guid, System.DateTime Waybill_ShipDate,
                    System.Guid SetWaybillShipMode_Guid, System.String ShipDescription,
                    ref System.Guid WaybillState_Guid, ref System.Int32 iErr, ref System.String strErr)
        {
            System.Int32 iRet = -1;
            try
            {
                iRet = CWaybillDataBaseModel.ShippedProductsByWaybill(objProfile, null,
                    Waybill_Guid, Waybill_ShipDate, SetWaybillShipMode_Guid, ShipDescription, 
                    ref WaybillState_Guid, ref iErr, ref strErr);
            }
            catch (System.Exception f)
            {
                strErr += ("\n" + f.Message);
            }
            finally
            {
            }
            return iRet;
        }
        #endregion

    }


    #region Экспорт информации о накладной
    /// <summary>
    /// Структура для экспорта табличной части накладной в MS Excel
    /// </summary>
    public struct WaybillItemForExport
    {
            private System.Int32 m_WAYBITMS_ID;
            private System.Int32 m_PARTS_ID;
            private System.String m_PARTS_FULLNAME;
            private System.String m_OWNER_NAME;
            private System.String m_MEASURE_SHORTNAME;
            private System.Double m_WAYBITMS_QUANTITY;
            private System.Double m_WAYBITMS_BASEPRICE;
            private System.String m_WAYBITMS_PERCENT;
            private System.Double m_WAYBITMS_DOUBLEPERCENT;
            private System.Double m_WAYBITMS_TOTALPRICEWITHOUTNDS;
            private System.Double m_WAYBITMS_NDS;
            private System.String m_WAYBITMS_STRNDS;
            private System.Double m_WAYBITMS_NDSTOTALPRICE;
            private System.Double m_WAYBITMS_TOTALPRICE;
            private System.Double m_WAYBITMS_WEIGHT;
            private System.String m_WAYBITMS_PLACES;
            private System.Int32 m_WAYBITMS_INTPLACES;
            private System.String m_WAYBITMS_TARA;
            private System.String m_WAYBITMS_QTYINPLACE;
            private System.String m_PARTS_CERTIFICATE;
            private System.String m_WAYBITMS_NOTES;
            private System.Double m_WAYBITMS_PRICEWITHOUTNDS;
            private System.String m_COUNTRY_NAME;
            private System.Guid m_Parts_Guid;
            private System.String m_Parts_Barcode;
            private System.String m_Parts_BarcodeList;

            public System.Int32 WAYBITMS_ID {  get { return m_WAYBITMS_ID; } }
            public System.Int32 PARTS_ID {  get { return m_PARTS_ID; } }
            public System.String PARTS_FULLNAME { get { return m_PARTS_FULLNAME; } }
            public System.String OWNER_NAME { get { return m_OWNER_NAME; } }
            public System.String MEASURE_SHORTNAME { get { return m_MEASURE_SHORTNAME; } }
            public System.Double WAYBITMS_QUANTITY { get { return m_WAYBITMS_QUANTITY; } }
            public System.Double WAYBITMS_BASEPRICE { get { return m_WAYBITMS_BASEPRICE; } }
            public System.String WAYBITMS_PERCENT { get { return m_WAYBITMS_PERCENT; } }
            public System.Double WAYBITMS_DOUBLEPERCENT { get { return m_WAYBITMS_DOUBLEPERCENT; } }
            public System.Double WAYBITMS_TOTALPRICEWITHOUTNDS { get { return m_WAYBITMS_TOTALPRICEWITHOUTNDS; } }
            public System.Double WAYBITMS_NDS { get { return m_WAYBITMS_NDS; } }
            public System.String WAYBITMS_STRNDS { get { return m_WAYBITMS_STRNDS; } }
            public System.Double WAYBITMS_NDSTOTALPRICE { get { return m_WAYBITMS_NDSTOTALPRICE; } }
            public System.Double WAYBITMS_TOTALPRICE { get { return m_WAYBITMS_TOTALPRICE; } }
            public System.Double WAYBITMS_WEIGHT { get { return m_WAYBITMS_WEIGHT; } }
            public System.String WAYBITMS_PLACES { get { return m_WAYBITMS_PLACES; } }
            public System.Int32 WAYBITMS_INTPLACES { get { return m_WAYBITMS_INTPLACES; } }
            public System.String WAYBITMS_TARA { get { return m_WAYBITMS_TARA; } }
            public System.String WAYBITMS_QTYINPLACE { get { return m_WAYBITMS_QTYINPLACE; } }
            public System.String PARTS_CERTIFICATE { get { return m_PARTS_CERTIFICATE; } }
            public System.String WAYBITMS_NOTES { get { return m_WAYBITMS_NOTES; } }
            public System.Double WAYBITMS_PRICEWITHOUTNDS { get { return m_WAYBITMS_PRICEWITHOUTNDS; } }
            public System.String COUNTRY_NAME { get { return m_COUNTRY_NAME; } }
            public System.Guid Parts_Guid { get { return m_Parts_Guid; } }
            public System.String Parts_Barcode { get { return m_Parts_Barcode; } }
            public System.String Parts_BarcodeList { get { return m_Parts_BarcodeList; } }

            public WaybillItemForExport(System.Int32 iWAYBITMS_ID, System.Int32 iPARTS_ID, System.String strPARTS_FULLNAME,
                System.String strOWNER_NAME, System.String strMEASURE_SHORTNAME, System.Double dblWAYBITMS_QUANTITY,
                System.Double dblWAYBITMS_BASEPRICE, System.String strWAYBITMS_PERCENT, System.Double dblWAYBITMS_DOUBLEPERCENT,
                System.Double dblWAYBITMS_TOTALPRICEWITHOUTNDS, System.Double dblWAYBITMS_NDS, System.String strWAYBITMS_STRNDS,
                System.Double dblWAYBITMS_NDSTOTALPRICE, System.Double dblWAYBITMS_TOTALPRICE, System.Double dblWAYBITMS_WEIGHT,
                System.String strWAYBITMS_PLACES, System.Int32 iWAYBITMS_INTPLACES, System.String strWAYBITMS_TARA,
                System.String strWAYBITMS_QTYINPLACE, System.String strPARTS_CERTIFICATE, System.String strWAYBITMS_NOTES,
                System.Double dblWAYBITMS_PRICEWITHOUTNDS, System.String strCOUNTRY_NAME, System.Guid uuidParts_Guid,
                System.String strParts_Barcode, System.String strParts_BarcodeList)
            {
                m_WAYBITMS_ID = iWAYBITMS_ID;
                m_PARTS_ID = iPARTS_ID;
                m_PARTS_FULLNAME = strPARTS_FULLNAME;
                m_OWNER_NAME = strOWNER_NAME;
                m_MEASURE_SHORTNAME = strMEASURE_SHORTNAME;
                m_WAYBITMS_QUANTITY = dblWAYBITMS_QUANTITY;
                m_WAYBITMS_BASEPRICE = dblWAYBITMS_BASEPRICE;
                m_WAYBITMS_PERCENT = strWAYBITMS_PERCENT;
                m_WAYBITMS_DOUBLEPERCENT = dblWAYBITMS_DOUBLEPERCENT;
                m_WAYBITMS_TOTALPRICEWITHOUTNDS = dblWAYBITMS_TOTALPRICEWITHOUTNDS;
                m_WAYBITMS_NDS = dblWAYBITMS_NDS;
                m_WAYBITMS_STRNDS = strWAYBITMS_STRNDS;
                m_WAYBITMS_NDSTOTALPRICE = dblWAYBITMS_NDSTOTALPRICE;
                m_WAYBITMS_TOTALPRICE = dblWAYBITMS_TOTALPRICE;
                m_WAYBITMS_WEIGHT = dblWAYBITMS_WEIGHT;
                m_WAYBITMS_PLACES = strWAYBITMS_PLACES;
                m_WAYBITMS_INTPLACES = iWAYBITMS_INTPLACES;
                m_WAYBITMS_TARA = strWAYBITMS_TARA;
                m_WAYBITMS_QTYINPLACE = strWAYBITMS_QTYINPLACE;
                m_PARTS_CERTIFICATE = strPARTS_CERTIFICATE;
                m_WAYBITMS_NOTES = strWAYBITMS_NOTES;
                m_WAYBITMS_PRICEWITHOUTNDS = dblWAYBITMS_PRICEWITHOUTNDS;
                m_COUNTRY_NAME = strCOUNTRY_NAME;
                m_Parts_Guid = uuidParts_Guid;
                m_Parts_Barcode = strParts_Barcode;
                m_Parts_BarcodeList = strParts_BarcodeList;
                
            }
    }
    #endregion

}
