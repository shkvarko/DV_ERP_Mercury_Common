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
        /// Номер розничной накладной
        /// </summary>
        public System.String RetailDocNum { get; set; }
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
        public CIntWaybillShipMode WaybillShipMode { get; set; }
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
        /// Сумма розничной накладной
        /// </summary>
        public System.Double SumRetail { get; set; }
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
        public System.Boolean IsForStock { get; set; }
        public System.Boolean IsSend { get; set; }
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
            RetailDocNum = System.String.Empty; 
            BeginDate = System.DateTime.MinValue;
            WaybillShipMode = null;
            SalesMan = null;
            Depart = null;
            StockSrc = null;
            CompanySrc = null;
            StockDst = null;
            CompanyDst = null;
            DocState = null;
            PaymentType = null;
            Currency = null;
            Description = System.String.Empty;
            IsForStock = false;
            IsSend = false;
            WaybillItemList = null;
            Quantity = 0;
            QuantityReturn = 0;
            PricingCurrencyRate = 0;
            SumWaybill = 0;
            SumRetail = 0;
            SumWaybillWithReturn = 0;
            SumDiscount = 0;
            SumWaybillInAccountingCurrency = 0;
            SumDiscountInAccountingCurrency = 0;

        }
        #endregion

        #region Журнал накладных
        /// <summary>
        /// Возвращает список накладных за указанный период
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="IntWaybill_Guid">УИ документа</param>
        /// <param name="IntWaybill_DateBegin">начало периода для выборки</param>
        /// <param name="IntWaybill_DateEnd">конец периода для выборки</param>
        /// <param name="IntWaybill_SrcCompanyGuid">УИ компании "Откуда"</param>
        /// <param name="IntWaybill_SrcStockGuid">УИ склада "Откуда"<</param>
        /// <param name="IntWaybill_DstCompanyGuid">УИ компании "Куда"</param>
        /// <param name="IntWaybill_DstStockGuid">УИ склада "Куда"<</param>
        /// <param name="Waybill_PaymentTypeGuid">УИ формы оплаты</param>
        /// <param name="strErr">текст ошибки</param>
        /// <param name="SelectIntWaybillInfoFromIntOrder">признак "информация для накладной запрашивается из заказа"</param>
        /// <param name="OnlyUnShippedWaybills">признак "запрос только НЕ отгруженных накладных"</param>
        /// <returns>список объектов класса "CIntWaybill"</returns>
        public static List<CIntWaybill> GetWaybillList(UniXP.Common.CProfile objProfile,
            System.Guid IntWaybill_Guid,
            System.DateTime IntWaybill_DateBegin, System.DateTime IntWaybill_DateEnd,
            System.Guid IntWaybill_SrcCompanyGuid, System.Guid IntWaybill_SrcStockGuid,
            System.Guid IntWaybill_DstCompanyGuid, System.Guid IntWaybill_DstStockGuid,
            System.Guid Waybill_PaymentTypeGuid, ref System.String strErr,
            System.Boolean SelectIntWaybillInfoFromIntOrder = false,
            System.Boolean OnlyUnShippedWaybills = false
            )
        {
            List<CIntWaybill> objList = new List<CIntWaybill>();

            try
            {
                // вызов статического метода из класса, связанного с БД
                System.Data.DataTable dtList = CIntWaybillDataBaseModel.GetWaybillTable(objProfile, null, IntWaybill_Guid,
                    IntWaybill_DateBegin, IntWaybill_DateEnd, IntWaybill_SrcCompanyGuid, IntWaybill_SrcStockGuid,
                    IntWaybill_DstCompanyGuid, IntWaybill_DstStockGuid,
                    Waybill_PaymentTypeGuid, ref strErr,
                    SelectIntWaybillInfoFromIntOrder,  OnlyUnShippedWaybills);
                if (dtList != null)
                {
                    CIntWaybill objWaybill = null;
                    System.Int32 objWaybill_Ib_ID = 0;
                    foreach (System.Data.DataRow objItem in dtList.Rows)
                    {
                        objWaybill = new CIntWaybill();
                        objWaybill_Ib_ID = System.Convert.ToInt32(objItem["IntWaybill_Id"]);
                        objWaybill.ID = ((objItem["IntWaybill_Guid"] != System.DBNull.Value) ? new System.Guid(System.Convert.ToString(objItem["IntWaybill_Guid"])) : System.Guid.Empty);
                        objWaybill.ParentID = ((objItem["IntWaybillParent_Guid"] != System.DBNull.Value) ? new System.Guid(System.Convert.ToString(objItem["IntWaybillParent_Guid"])) : System.Guid.Empty);
                        objWaybill.IntOrderID = ((objItem["IntOrder_Guid"] != System.DBNull.Value) ? new System.Guid(System.Convert.ToString(objItem["IntOrder_Guid"])) : System.Guid.Empty);
                        objWaybill.StockSrc = ((objItem["SrcStock_Guid"] != System.DBNull.Value) ? new CStock()
                        {
                            ID = (System.Guid)objItem["SrcStock_Guid"],
                            IBId = System.Convert.ToInt32(objItem["SrcStock_Id"]),
                            Name = System.Convert.ToString(objItem["SrcStock_Name"]),
                            IsAcitve = System.Convert.ToBoolean(objItem["SrcStock_IsActive"]),
                            IsTrade = System.Convert.ToBoolean(objItem["SrcStock_IsTrade"]),
                            WareHouse = new CWarehouse() { ID = (System.Guid)objItem["SrcWarehouse_Guid"] },
                            WareHouseType = new CWareHouseType() { ID = (System.Guid)objItem["SrcWarehouseType_Guid"] }
                        } : null);
                        objWaybill.CompanySrc = ((objItem["SrcCompany_Guid"] != System.DBNull.Value) ? new CCompany()
                        {
                            ID = (System.Guid)objItem["SrcCompany_Guid"],
                            InterBaseID = System.Convert.ToInt32(objItem["SrcCompany_Id"]),
                            Abbr = System.Convert.ToString(objItem["SrcCompany_Acronym"]),
                            Name = System.Convert.ToString(objItem["SrcCompany_Name"])
                        } : null);
                        objWaybill.StockDst = ((objItem["DstStock_Guid"] != System.DBNull.Value) ? new CStock()
                        {
                            ID = (System.Guid)objItem["DstStock_Guid"],
                            IBId = System.Convert.ToInt32(objItem["DstStock_Id"]),
                            Name = System.Convert.ToString(objItem["DstStock_Name"]),
                            IsAcitve = System.Convert.ToBoolean(objItem["DstStock_IsActive"]),
                            IsTrade = System.Convert.ToBoolean(objItem["DstStock_IsTrade"]),
                            WareHouse = new CWarehouse() { ID = (System.Guid)objItem["DstWarehouse_Guid"] },
                            WareHouseType = new CWareHouseType() { ID = (System.Guid)objItem["DstWarehouseType_Guid"] }
                        } : null);
                        objWaybill.CompanyDst = ((objItem["DstCompany_Guid"] != System.DBNull.Value) ? new CCompany()
                        {
                            ID = (System.Guid)objItem["DstCompany_Guid"],
                            InterBaseID = System.Convert.ToInt32(objItem["DstCompany_Id"]),
                            Abbr = System.Convert.ToString(objItem["DstCompany_Acronym"]),
                            Name = System.Convert.ToString(objItem["DstCompany_Name"])
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


                        objWaybill.PaymentType = ((objItem["PaymentType_Guid"] != System.DBNull.Value) ? new CPaymentType(
                            (System.Guid)objItem["PaymentType_Guid"], System.Convert.ToString(objItem["PaymentType_Name"])) : null);

                        objWaybill.BeginDate = ((objItem["IntWaybill_BeginDate"] != System.DBNull.Value) ? System.Convert.ToDateTime(objItem["IntWaybill_BeginDate"]) : System.DateTime.MinValue);
                        if (objItem["IntWaybill_ShipDate"] != System.DBNull.Value)
                        {
                            objWaybill.ShipDate = System.Convert.ToDateTime(objItem["IntWaybill_ShipDate"]);
                        }
                        objWaybill.DocNum = ((objItem["IntWaybill_Num"] != System.DBNull.Value) ? System.Convert.ToString(objItem["IntWaybill_Num"]) : System.String.Empty);
                        objWaybill.RetailDocNum = ((objItem["RetailWaybill_Num"] != System.DBNull.Value) ? System.Convert.ToString(objItem["RetailWaybill_Num"]) : System.String.Empty);

                        objWaybill.DocState = ((objItem["IntWaybillState_Guid"] != System.DBNull.Value) ? new CIntWaybillState()
                        {
                            ID = (System.Guid)objItem["IntWaybillState_Guid"],
                            IntWaybillStateId = System.Convert.ToInt32(objItem["IntWaybillState_Id"]),
                            Name = System.Convert.ToString(objItem["IntWaybillState_Name"])
                        } : null);

                        objWaybill.WaybillShipMode = ((objItem["IntWaybillShipMode_Guid"] != System.DBNull.Value) ? new CIntWaybillShipMode()
                        {
                            ID = (System.Guid)objItem["IntWaybillShipMode_Guid"],
                            WaybillShipModeId = System.Convert.ToInt32(objItem["IntWaybillShipMode_Id"]),
                            Name = System.Convert.ToString(objItem["IntWaybillShipMode_Name"])
                        } : null);

                        objWaybill.Description = ((objItem["IntWaybill_Description"] != System.DBNull.Value) ? System.Convert.ToString(objItem["IntWaybill_Description"]) : System.String.Empty);
                        objWaybill.IsForStock = ((objItem["IntWaybill_ForStock"] != System.DBNull.Value) ? System.Convert.ToBoolean(objItem["IntWaybill_ForStock"]) : false);
                        objWaybill.IsSend = ((objItem["IntWaybill_Send"] != System.DBNull.Value) ? System.Convert.ToBoolean(objItem["IntWaybill_Send"]) : false);

                        objWaybill.SumWaybill = ((objItem["IntWaybill_AllPrice"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["IntWaybill_AllPrice"]) : 0);
                        objWaybill.SumDiscount = 0; // ((objItem["IntWaybill_AllDiscount"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["IntWaybill_AllDiscount"]) : 0);
                        objWaybill.SumRetail = ((objItem["IntWaybill_RetailAllPrice"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["IntWaybill_RetailAllPrice"]) : 0);

                        objWaybill.Quantity = ((objItem["IntWaybill_Quantity"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["IntWaybill_Quantity"]) : 0);

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
        /// Возвращает объект класса "Накладная на внутреннее перемещение"
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="Waybill_Guid">УИ накладной</param>
        /// <param name="strErr">текст ошибки</param>
        /// <returns>объект класса "Накладная"</returns>
        public static CIntWaybill GetWaybill(UniXP.Common.CProfile objProfile, System.Guid IntWaybill_Guid,
            ref System.String strErr)
        {
            CIntWaybill objWaybill = null;

            try
            {
                // вызов статического метода из класса, связанного с БД
                List<CIntWaybill> objList = CIntWaybill.GetWaybillList(objProfile, IntWaybill_Guid, System.DateTime.MinValue, System.DateTime.MinValue,
                    System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, ref strErr, false, false);
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

    }
}
