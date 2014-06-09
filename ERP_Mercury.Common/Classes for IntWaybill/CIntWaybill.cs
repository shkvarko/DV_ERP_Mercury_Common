using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ERP_Mercury.Common
{
    /// <summary>
    /// Класс "Строка табличной части накладной на внутренее перемещение"
    /// </summary>
    public class CIntWaybillItem
    {
        #region Уникальные идентификаторы
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public System.Guid ID { get; set; }
        /// <summary>
        /// Уникальный идентификатор записи в InterBase
        /// </summary>
        public System.Int32 Ib_ID { get; set; }
        /// <summary>
        /// Уникальный идентификатор строки в приложении к заказу на внутренее перемещение
        /// </summary>
        public System.Guid IntOrderItemID { get; set; }
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
        /// <summary>
        /// Слкращенное наименование единицы измерения
        /// </summary>
        public System.String MeasureShortName
        {
            get { return ((Measure == null) ? "" : Measure.ShortName); }
            set { MeasureShortName = value; }
        }
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
        /// Надбавка в процентах
        /// </summary>
        public System.Double MarkUpPercent { get; set; }
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
        public System.Double Sum { get { return (Quantity * Price); } }
        /// <summary>
        /// Сумма с учетом скидки (в национальной валюте)
        /// </summary>
        public System.Double SumWithDiscount { get { return (Quantity * PriceWithDiscount); } }
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

        #region Конструктор

        public CIntWaybillItem()
        {
            ID = System.Guid.Empty;
            Ib_ID = 0;
            Product = null;
            Measure = null;
            Quantity = 0;
            QuantityReturn = 0;
            NDSPercent = 0;
            MarkUpPercent = 0;
            DiscountPercent = 0;
            PriceImporter = 0;
            Price = 0;
            PriceWithDiscount = 0;
            PriceInAccountingCurrency = 0;
            PriceWithDiscountInAccountingCurrency = 0;
        }

        #endregion

        #region Приложение к накладной на внутреннее перемещение
        /// <summary>
        /// Возвращает приложение к накладной на внутреннее перемещение
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="uuidIntWaybillId">уи накладной на внутреннее перемещение</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="bTableFromIntOrder">признак "приложение запрашивается из заказа на внутреннее перемещение"</param>
        /// <returns>приложение к накладной, как список объектов класса CWaybillItem</returns>
        public static List<CIntWaybillItem> GetIntWaybillTablePart(UniXP.Common.CProfile objProfile,
            System.Guid uuidIntWaybillId, ref System.String strErr, System.Boolean bTableFromIntOrder = false)
        {
            List<CIntWaybillItem> objList = new List<CIntWaybillItem>();

            try
            {
                System.Data.DataTable dtList = CIntWaybillDataBaseModel.GetIntWaybilItemTable(objProfile, null, uuidIntWaybillId, ref strErr, bTableFromIntOrder);
                if (dtList != null)
                {
                    CIntWaybillItem objWaybillItem = null;
                    foreach (System.Data.DataRow objItem in dtList.Rows)
                    {
                        objWaybillItem = new CIntWaybillItem();

                        objWaybillItem.ID = ((objItem["WaybItem_Guid"] != System.DBNull.Value) ? (System.Guid)objItem["WaybItem_Guid"] : System.Guid.Empty);
                        objWaybillItem.Ib_ID = ((objItem["WaybItem_Id"] != System.DBNull.Value) ? System.Convert.ToInt32(objItem["WaybItem_Id"]) : 0);
                        objWaybillItem.IntOrderItemID = ((objItem["SupplItem_Guid"] != System.DBNull.Value) ? (System.Guid)objItem["SupplItem_Guid"] : System.Guid.Empty);
                        objWaybillItem.Product = ((objItem["Parts_Guid"] != System.DBNull.Value) ? new CProduct()
                        {
                            ID = (System.Guid)objItem["Parts_Guid"],
                            Name = System.Convert.ToString(objItem["PARTS_NAME"]),
                            Article = System.Convert.ToString(objItem["PARTS_ARTICLE"]),
                            ProductTradeMark = new CProductTradeMark() { Name = System.Convert.ToString(objItem["ProductOwnerName"]) }
                        } : null);

                        objWaybillItem.Measure = ((objItem["Measure_Guid"] != System.DBNull.Value) ? new CMeasure()
                        {
                            ID = (System.Guid)objItem["Measure_Guid"],
                            ShortName = System.Convert.ToString(objItem["Measure_ShortName"])
                        } : null);

                        objWaybillItem.Quantity = ((objItem["WaybItem_Quantity"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["WaybItem_Quantity"]) : 0);
                        objWaybillItem.QuantityReturn = ((objItem["WaybItem_RetQuantity"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["WaybItem_RetQuantity"]) : 0);

                        objWaybillItem.NDSPercent = ((objItem["WaybItem_NDSPercent"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["WaybItem_NDSPercent"]) : 0);
                        objWaybillItem.MarkUpPercent = ((objItem["WaybItem_MarkUpPercent"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["WaybItem_MarkUpPercent"]) : 0);
                        objWaybillItem.PriceImporter = ((objItem["WaybItem_PriceImporter"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["WaybItem_PriceImporter"]) : 0);
                        objWaybillItem.Price = ((objItem["WaybItem_Price"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["WaybItem_Price"]) : 0);
                        objWaybillItem.DiscountPercent = ((objItem["WaybItem_Discount"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["WaybItem_Discount"]) : 0);
                        objWaybillItem.PriceWithDiscount = ((objItem["WaybItem_DiscountPrice"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["WaybItem_DiscountPrice"]) : 0);
                        objWaybillItem.PriceInAccountingCurrency = ((objItem["WaybItem_CurrencyPrice"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["WaybItem_CurrencyPrice"]) : 0);
                        objWaybillItem.PriceWithDiscountInAccountingCurrency = ((objItem["WaybItem_CurrencyDiscountPrice"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["WaybItem_CurrencyDiscountPrice"]) : 0);


                        if (objWaybillItem != null) { objList.Add(objWaybillItem); }

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

                        objList.Add(new WaybillItemForExport(iWAYBITMS_ID, iPARTS_ID, strPARTS_FULLNAME,
                                        strOWNER_NAME, strMEASURE_SHORTNAME, dblWAYBITMS_QUANTITY,
                                        dblWAYBITMS_BASEPRICE, strWAYBITMS_PERCENT, dblWAYBITMS_DOUBLEPERCENT,
                                        dblWAYBITMS_TOTALPRICEWITHOUTNDS, dblWAYBITMS_NDS, strWAYBITMS_STRNDS,
                                        dblWAYBITMS_NDSTOTALPRICE, dblWAYBITMS_TOTALPRICE, dblWAYBITMS_WEIGHT,
                                        strWAYBITMS_PLACES, iWAYBITMS_INTPLACES, strWAYBITMS_TARA,
                                        strWAYBITMS_QTYINPLACE, strPARTS_CERTIFICATE, strWAYBITMS_NOTES,
                                        dblWAYBITMS_PRICEWITHOUTNDS, strCOUNTRY_NAME, uuidParts_Guid,
                                        strParts_Barcode, strParts_BarcodeList));

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
                if (objWaybillItemsList.Count > 0)
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
    /// Класс "Накладная на внутренее перемещение"
    /// </summary>
    public class CIntWaybill
    {
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
        public System.Guid IntOrderID { get; set; }
        /// <summary>
        /// Номер накладной
        /// </summary>
        public System.String DocNum { get; set; }
        /// <summary>
        /// Дата создания накладной
        /// </summary>
        public System.DateTime BeginDate { get; set; }
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
        /// Склад-источник
        /// </summary>
        public CStock StockSrc { get; set; }
        /// <summary>
        /// Наименование склада-источника
        /// </summary>
        public System.String StockSrcName
        {
            get { return ((StockSrc == null) ? System.String.Empty : StockSrc.Name); }
        }
        /// <summary>
        /// Компания-источник
        /// </summary>
        public CCompany CompanySrc { get; set; }
        /// <summary>
        /// Сокращенное наименование компании-источника
        /// </summary>
        public System.String CompanySrcAcronym
        {
            get { return ((CompanySrc == null) ? System.String.Empty : CompanySrc.Abbr); }
        }
        /// <summary>
        /// Склад-назначение
        /// </summary>
        public CStock StockDst { get; set; }
        /// <summary>
        /// Наименование склада-назначения
        /// </summary>
        public System.String StockDstName
        {
            get { return ((StockDst == null) ? System.String.Empty : StockDst.Name); }
        }
        /// <summary>
        /// Компания-назначение
        /// </summary>
        public CCompany CompanyDst { get; set; }
        /// <summary>
        /// Сокращенное наименование компании-назначения
        /// </summary>
        public System.String CompanyDstAcronym
        {
            get { return ((CompanyDst == null) ? System.String.Empty : CompanyDst.Abbr); }
        }
        #endregion

        #region Состояние документа
        /// <summary>
        /// Состояние накладной
        /// </summary>
        public CIntWaybillState DocState { get; set; }
        /// <summary>
        /// Состояние накладной
        /// </summary>
        public System.String DocStateName
        {
            get { return ((DocState == null) ? System.String.Empty : DocState.Name); }
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
        /// Сумма накладной без учета скидки (в национальной валюте) с учетом возврата
        /// </summary>
        public System.Double SumWaybillWithReturn { get; set; }
        /// <summary>
        /// Сумма скидки по накладной (в национальной валюте)
        /// </summary>
        public System.Double SumDiscount { get; set; }
        /// <summary>
        /// Сумма накладной с учетом скидки (в национальной валюте)
        /// </summary>
        public System.Double SumWithDiscount { get { return (SumWaybill - SumDiscount); } }
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
        public List<CIntWaybillItem> WaybillItemList { get; set; }
        #endregion

        #region Конструктор
        public CIntWaybill()
        {
            ID = System.Guid.Empty;
            Ib_ID = 0;
            ParentID = System.Guid.Empty;
            IntOrderID = System.Guid.Empty;
            DocNum = System.String.Empty;
            BeginDate = System.DateTime.MinValue;
            WaybillShipMode = null;
            SalesMan = null;
            Depart = null;
            StockSrc = null;
            CompanySrc = null;
            StockDst = null;
            CompanyDst = null;
            DocState = null;
            Currency = null;
            Description = System.String.Empty;
            WaybillItemList = null;
            Quantity = 0;
            QuantityReturn = 0;
            PricingCurrencyRate = 0;
            SumWaybill = 0;
            SumWaybillWithReturn = 0;
            SumDiscount = 0;
            SumWaybillInAccountingCurrency = 0;
            SumDiscountInAccountingCurrency = 0;

        }
        #endregion
    }
}
