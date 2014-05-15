using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ERP_Mercury.Common
{

    /// <summary>
    /// Класс "Строка табличной части накладной на возврат товара от клиента"
    /// </summary>
    public class CBackWaybillItem
    {
        #region Конструктор
        #endregion

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
        /// Уникальный идентификатор строки в приложении к накладной
        /// </summary>
        public System.Guid WaybItemID { get; set; }
        /// <summary>
        /// Уникальный идентификатор строки в приложении к накладной
        /// </summary>
        public System.Int32 WaybItemIb_ID { get; set; }
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
        public System.Double Sum { get { return (Quantity * Price); } }
        /// <summary>
        /// Сумма с учетом скидки (в национальной валюте)
        /// </summary>
        public System.Double SumWithDiscount { get { return (Quantity * PriceWithDiscount); } }
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
        #endregion

        #region Приложение к накладной
        /// <summary>
        /// Возвращает приложение к накладной
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="uuidBackWaybillId">уи накладной на возврат товара(отгрузку товара)</param>
        /// <param name="Waybill_DateBegin">начало периода для поиска накладных</param>
        /// <param name="Waybill_DateEnd">конец периода для поиска накладных</param>
        /// <param name="strErr">строка с сообщением об ошибке</param>
        /// <param name="bTableFromWaybilll">признак "приложение запрашивается из накладной"</param>
        /// <returns>приложение к накладной, как список объектов класса CWaybillItem</returns>
        public static List<CBackWaybillItem> GetWaybillTablePart(UniXP.Common.CProfile objProfile,
            System.Guid uuidBackWaybillId, System.DateTime Waybill_DateBegin, System.DateTime Waybill_DateEnd,
            ref System.String strErr, System.Boolean bTableFromWaybilll = false)
        {
            List<CBackWaybillItem> objList = new List<CBackWaybillItem>();

            try
            {
                System.Data.DataTable dtList = CBackWaybillDataBaseModel.GetBackWaybilItemTable( objProfile, null,
                    uuidBackWaybillId, Waybill_DateBegin, Waybill_DateEnd, ref strErr, bTableFromWaybilll );
                if (dtList != null)
                {
                    CBackWaybillItem objWaybillItem = null;
                    foreach (System.Data.DataRow objItem in dtList.Rows)
                    {
                        objWaybillItem = new CBackWaybillItem();


                        objWaybillItem.ID = ((objItem["BackWaybItem_Guid"] != System.DBNull.Value) ? (System.Guid)objItem["BackWaybItem_Guid"] : System.Guid.Empty);
                        objWaybillItem.Ib_ID = ((objItem["BackWaybItem_Id"] != System.DBNull.Value) ? System.Convert.ToInt32(objItem["BackWaybItem_Id"]) : 0);
                        objWaybillItem.WaybItemID = ((objItem["WaybItem_Guid"] != System.DBNull.Value) ? (System.Guid)objItem["WaybItem_Guid"] : System.Guid.Empty);
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

                        objWaybillItem.Quantity = ((objItem["BackWaybItem_Quantity"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["BackWaybItem_Quantity"]) : 0);

                        objWaybillItem.NDSPercent = ((objItem["BackWaybItem_NDSPercent"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["BackWaybItem_NDSPercent"]) : 0);
                        objWaybillItem.PriceImporter = ((objItem["BackWaybItem_PriceImporter"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["BackWaybItem_PriceImporter"]) : 0);
                        objWaybillItem.Price = ((objItem["BackWaybItem_Price"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["BackWaybItem_Price"]) : 0);
                        objWaybillItem.DiscountPercent = ((objItem["BackWaybItem_Discount"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["BackWaybItem_Discount"]) : 0);
                        objWaybillItem.PriceWithDiscount = ((objItem["BackWaybItem_DiscountPrice"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["BackWaybItem_DiscountPrice"]) : 0);
                        objWaybillItem.PriceInAccountingCurrency = ((objItem["BackWaybItem_CurrencyPrice"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["BackWaybItem_CurrencyPrice"]) : 0);
                        objWaybillItem.PriceWithDiscountInAccountingCurrency = ((objItem["BackWaybItem_CurrencyDiscountPrice"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["BackWaybItem_CurrencyDiscountPrice"]) : 0);


                        if (objWaybillItem != null) { objList.Add(objWaybillItem); }

                    }
                }

                dtList = null;

            }
            catch (System.Exception f)
            {
                strErr += (String.Format("\nНе удалось получить приложение к накладной на возврат товара.\nТекст ошибки: {0}", f.Message));
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
        public static List<BackWaybillItemForExport> GetWaybillTablePartForExportToExcel(UniXP.Common.CProfile objProfile,
            System.Guid uuidWaybillId, ref System.String strErr)
        {
            List<BackWaybillItemForExport> objList = new List<BackWaybillItemForExport>();

            try
            {
                System.Data.DataTable dtList = CBackWaybillDataBaseModel.GetWaybilItemTableForExportToExcel(objProfile, null, uuidWaybillId, ref strErr);
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

                        objList.Add(new BackWaybillItemForExport(iWAYBITMS_ID, iPARTS_ID, strPARTS_FULLNAME,
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
        public static System.Data.DataTable ConvertListToTable(List<CBackWaybillItem> objWaybillItemsList, ref System.String strErr)
        {
            System.Data.DataTable objTable = new System.Data.DataTable();
            try
            {
                objTable.Columns.Add(new System.Data.DataColumn("BackWaybItem_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                objTable.Columns.Add(new System.Data.DataColumn("WaybItem_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                objTable.Columns.Add(new System.Data.DataColumn("Parts_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                objTable.Columns.Add(new System.Data.DataColumn("Measure_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                objTable.Columns.Add(new System.Data.DataColumn("BackWaybItem_Quantity", typeof(int)));
                objTable.Columns.Add(new System.Data.DataColumn("BackWaybItem_PriceImporter", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("BackWaybItem_Price", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("BackWaybItem_Discount", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("BackWaybItem_DiscountPrice", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("BackWaybItem_CurrencyPrice", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("BackWaybItem_CurrencyDiscountPrice", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("BackWaybItem_NDSPercent", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("BackWaybItem_Id", typeof(int)));

                System.Data.DataRow newRow = null;
                foreach (CBackWaybillItem objItem in objWaybillItemsList)
                {
                    newRow = objTable.NewRow();
                    newRow["BackWaybItem_Guid"] = objItem.ID;
                    newRow["WaybItem_Guid"] = objItem.WaybItemID;
                    newRow["Parts_Guid"] = objItem.Product.ID;
                    newRow["Measure_Guid"] = objItem.Measure.ID;
                    newRow["BackWaybItem_Quantity"] = objItem.Quantity;
                    newRow["BackWaybItem_PriceImporter"] = (System.Data.SqlTypes.SqlMoney)objItem.PriceImporter;
                    newRow["BackWaybItem_Price"] = (System.Data.SqlTypes.SqlMoney)objItem.Price;
                    newRow["BackWaybItem_Discount"] = (System.Data.SqlTypes.SqlMoney)objItem.DiscountPercent;
                    newRow["BackWaybItem_DiscountPrice"] = (System.Data.SqlTypes.SqlMoney)objItem.PriceWithDiscount;
                    newRow["BackWaybItem_CurrencyPrice"] = (System.Data.SqlTypes.SqlMoney)objItem.PriceInAccountingCurrency;
                    newRow["BackWaybItem_CurrencyDiscountPrice"] = (System.Data.SqlTypes.SqlMoney)objItem.PriceWithDiscountInAccountingCurrency;
                    newRow["BackWaybItem_NDSPercent"] = (System.Data.SqlTypes.SqlMoney)objItem.NDSPercent;
                    newRow["BackWaybItem_Id"] = objItem.Ib_ID;
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
    /// Класс "Накладная на возврат товара от клиента"
    /// </summary>
    public class CBackWaybill
    {
        #region Конструктор
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
        /// Уникальный идентификатор накладной, из которой сформирована накладная на возврат
        /// </summary>
        public System.Guid WaybillID { get; set; }
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

        #region Причина возврата
        /// <summary>
        /// Причина возврата товара
        /// </summary>
        public CWaybillBackReason WaybillBackReason { get; set; }
        /// <summary>
        /// Причина возврата товара
        /// </summary>
        public System.String WaybillBackReasonName
        {
            get { return ((WaybillBackReason == null) ? System.String.Empty : WaybillBackReason.Name); }
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
        public List<CBackWaybillItem> WaybillItemList { get; set; }
        #endregion

        #region Журнал накладных
        /// <summary>
        /// Возвращает список накладных за указанный период
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="BackWaybill_Guid">уи накладной на возврат от клиента</param> 
        /// <param name="Waybill_Guid">уи накладной на отгрузку клиенту</param> 
        /// <param name="Waybill_DateBegin">начало периода для выборки</param>
        /// <param name="Waybill_DateEnd">конец периода для выборки</param>
        /// <param name="Waybill_CompanyGuid">УИ компании</param>
        /// <param name="Waybill_StockGuid">УИ склада</param>
        /// <param name="Waybill_PaymentTypeGuid">УИ формы оплаты</param>
        /// <param name="Waybill_CustomerGuid">УИ клиента</param>
        /// <param name="strErr">текст ошибки</param>
        /// <param name="SelectWaybillInfoFromWaybill">признак "информация для накладной на возврат запрашивается из накладной на отгрузку"</param>
        /// <returns>список объектов класса "CBackWaybill"</returns>
        public static List<CBackWaybill> GetBackWaybillList(UniXP.Common.CProfile objProfile,
            System.Guid BackWaybill_Guid, System.Guid Waybill_Guid,
            System.DateTime Waybill_DateBegin, System.DateTime Waybill_DateEnd,
            System.Guid Waybill_CompanyGuid, System.Guid Waybill_StockGuid,
            System.Guid Waybill_PaymentTypeGuid, System.Guid Waybill_CustomerGuid,
            ref System.String strErr, System.Boolean SelectWaybillInfoFromWaybill = false)
        {
            List<CBackWaybill> objList = new List<CBackWaybill>();

            try
            {
                // вызов статического метода из класса, связанного с БД
                System.Data.DataTable dtList = CBackWaybillDataBaseModel.GetBackWaybillTable( objProfile,
                    null, BackWaybill_Guid, Waybill_Guid, Waybill_DateBegin, Waybill_DateEnd,
                    Waybill_CompanyGuid, Waybill_StockGuid, Waybill_PaymentTypeGuid, Waybill_CustomerGuid,
                    ref strErr, SelectWaybillInfoFromWaybill);
                if (dtList != null)
                {
                    CBackWaybill objWaybill = null;
                    foreach (System.Data.DataRow objItem in dtList.Rows)
                    {
                        objWaybill = new CBackWaybill();
                        objWaybill.ID = ((objItem["BackWaybill_Guid"] != System.DBNull.Value) ? new System.Guid(System.Convert.ToString(objItem["BackWaybill_Guid"])) : System.Guid.Empty);
                        objWaybill.ParentID = ((objItem["BackWaybillParent_Guid"] != System.DBNull.Value) ? new System.Guid(System.Convert.ToString(objItem["BackWaybillParent_Guid"])) : System.Guid.Empty);
                        objWaybill.WaybillID = ((objItem["Waybill_Guid"] != System.DBNull.Value) ? new System.Guid(System.Convert.ToString(objItem["Waybill_Guid"])) : System.Guid.Empty);
                        objWaybill.Ib_ID = ((objItem["BackWaybill_Id"] != System.DBNull.Value) ? System.Convert.ToInt32(objItem["BackWaybill_Id"]) : 0);
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


                        objWaybill.PaymentType = ((objItem["PaymentType_Guid"] != System.DBNull.Value) ? new CPaymentType(
                            (System.Guid)objItem["PaymentType_Guid"], System.Convert.ToString(objItem["PaymentType_Name"])) : null);

                        objWaybill.BeginDate = ((objItem["BackWaybill_BeginDate"] != System.DBNull.Value) ? System.Convert.ToDateTime(objItem["BackWaybill_BeginDate"]) : System.DateTime.MinValue);
                        if (objItem["BackWaybill_ShipDate"] != System.DBNull.Value)
                        {
                            objWaybill.ShipDate = System.Convert.ToDateTime(objItem["BackWaybill_ShipDate"]);
                        }
                        objWaybill.DocNum = ((objItem["BackWaybill_Num"] != System.DBNull.Value) ? System.Convert.ToString(objItem["BackWaybill_Num"]) : System.String.Empty);

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

                        objWaybill.WaybillBackReason = ((objItem["WaybillBackReason_Guid"] != System.DBNull.Value) ? new CWaybillBackReason()
                        {
                            ID = (System.Guid)objItem["WaybillBackReason_Guid"],
                            WaybillBackReasonId = System.Convert.ToInt32(objItem["WaybillBackReason_Id"]),
                            Name = System.Convert.ToString(objItem["WaybillBackReason_Name"])
                        } : null);

                        objWaybill.Description = ((objItem["BackWaybill_Description"] != System.DBNull.Value) ? System.Convert.ToString(objItem["BackWaybill_Description"]) : System.String.Empty);

                        objWaybill.SumWaybill = ((objItem["BackWaybill_AllPrice"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["BackWaybill_AllPrice"]) : 0);
                        objWaybill.SumDiscount = ((objItem["BackWaybill_AllDiscount"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["BackWaybill_AllDiscount"]) : 0);

                        objWaybill.SumWaybillInAccountingCurrency = ((objItem["BackWaybill_CurrencyAllPrice"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["BackWaybill_CurrencyAllPrice"]) : 0);
                        objWaybill.SumDiscountInAccountingCurrency = ((objItem["BackWaybill_CurrencyAllDiscount"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["BackWaybill_CurrencyAllDiscount"]) : 0);

                        objWaybill.Quantity = ((objItem["BackWaybill_Quantity"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["BackWaybill_Quantity"]) : 0);

                        if (objWaybill != null) { objList.Add(objWaybill); }

                    }
                }

                dtList = null;

            }
            catch (System.Exception f)
            {
                strErr += (String.Format("\nНе удалось получить список накладных на возврат товара.\nТекст ошибки: {0}", f.Message));
            }
            return objList;
        }
        /// <summary>
        /// Возвращает объект класса "Накладная"
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="BackWaybill_Guid">УИ накладной</param>
        /// <param name="strErr">текст ошибки</param>
        /// <returns>объект класса "Накладная"</returns>
        public static CBackWaybill GetBackWaybill(UniXP.Common.CProfile objProfile, System.Guid BackWaybill_Guid,
            ref System.String strErr)
        {
            CBackWaybill objBackWaybill = null;

            try
            {
                List<CBackWaybill> objList = CBackWaybill.GetBackWaybillList(objProfile,
                    BackWaybill_Guid, System.Guid.Empty, System.DateTime.MinValue, System.DateTime.MinValue,
                    System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty,
                    ref strErr, false);

                if ( (objList != null) && ( objList.Count > 0 ) )
                {
                    objBackWaybill = objList[0];
                }

                objList = null;

            }
            catch (System.Exception f)
            {
                strErr += (String.Format("\nНе удалось получить информацию о накладной на возврат товара.\nТекст ошибки: {0}", f.Message));
            }
            return objBackWaybill;
        }

        #endregion

        #region Проверка, можно ли провести возврат по накладной
        /// <summary>
        /// Проверка, можно ли провести возврат по накладной
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="Waybill_Guid">УИ накладной</param>
        /// <param name="strErr">текст ошибки</param>
        /// <returns>true - можно; false - нельзя</returns>
        public static System.Boolean CanCreateWaybillFromSuppl(UniXP.Common.CProfile objProfile,
            System.Guid Waybill_Guid, ref System.String strErr)
        {
            return CBackWaybillDataBaseModel.CanCreateBackWaybillFromSuppl(objProfile, Waybill_Guid, ref strErr);
        }
        #endregion

    }

    #region Экспорт информации о накладной
    /// <summary>
    /// Структура для экспорта табличной части накладной в MS Excel
    /// </summary>
    public struct BackWaybillItemForExport
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

        public System.Int32 WAYBITMS_ID { get { return m_WAYBITMS_ID; } }
        public System.Int32 PARTS_ID { get { return m_PARTS_ID; } }
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

        public BackWaybillItemForExport(System.Int32 iWAYBITMS_ID, System.Int32 iPARTS_ID, System.String strPARTS_FULLNAME,
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
