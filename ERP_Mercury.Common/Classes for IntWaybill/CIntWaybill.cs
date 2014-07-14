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
        /// Количество с учетом возврата
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
        /// Цена (в национальной валюте)
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
        /// Цена розничная
        /// </summary>
        public System.Double PriceRetail { get; set; }

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
        public System.Double SumRetail { get { return (Quantity * PriceRetail); } }

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
            PriceRetail = 0;
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

                        objWaybillItem.ID = ((objItem["IntWaybItem_Guid"] != System.DBNull.Value) ? (System.Guid)objItem["IntWaybItem_Guid"] : System.Guid.Empty);
                        objWaybillItem.IntOrderItemID = ((objItem["IntOrderItem_Guid"] != System.DBNull.Value) ? (System.Guid)objItem["IntOrderItem_Guid"] : System.Guid.Empty);
                        objWaybillItem.Ib_ID = ((objItem["IntWaybillItem_Id"] != System.DBNull.Value) ? System.Convert.ToInt32(objItem["IntWaybillItem_Id"]) : 0);
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

                        objWaybillItem.Quantity = ((objItem["IntWaybItem_Quantity"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["IntWaybItem_Quantity"]) : 0);
                        objWaybillItem.QuantityReturn = ((objItem["IntWaybItem_RetQuantity"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["IntWaybItem_RetQuantity"]) : 0);

                        objWaybillItem.NDSPercent = ((objItem["IntWaybItem_NDSPercent"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["IntWaybItem_NDSPercent"]) : 0);
                        objWaybillItem.MarkUpPercent = ((objItem["IntWaybItem_MarkUpPercent"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["IntWaybItem_MarkUpPercent"]) : 0);
                        objWaybillItem.PriceImporter = ((objItem["IntWaybItem_PriceImporter"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["IntWaybItem_PriceImporter"]) : 0);
                        objWaybillItem.Price = ((objItem["IntWaybItem_Price"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["IntWaybItem_Price"]) : 0);
                        objWaybillItem.DiscountPercent = ((objItem["IntWaybItem_Discount"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["IntWaybItem_Discount"]) : 0);
                        objWaybillItem.PriceWithDiscount = ((objItem["IntWaybItem_DiscountPrice"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["IntWaybItem_DiscountPrice"]) : 0);
                        objWaybillItem.PriceRetail = ((objItem["IntWaybItem_RetailPrice"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["IntWaybItem_RetailPrice"]) : 0);


                        if (objWaybillItem != null) { objList.Add(objWaybillItem); }

                    }
                }

                dtList = null;

            }
            catch (System.Exception f)
            {
                strErr += (String.Format("\nНе удалось получить приложение к накладной на внутреннее перемещение.\nТекст ошибки: {0}", f.Message));
            }
            return objList;
        }

        /// <summary>
        /// Преобразует список записей к табличному виду
        /// </summary>
        /// <param name="objWaybillItemsList">список строк из приложения к накладной на внутреннее перемещение</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>таблица с приложением к накладной</returns>
        public static System.Data.DataTable ConvertListToTable(List<CIntWaybillItem> objIntWaybillItemsList, ref System.String strErr)
        {
            System.Data.DataTable objTable = new System.Data.DataTable();
            try
            {
                objTable.Columns.Add(new System.Data.DataColumn("IntWaybItem_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                objTable.Columns.Add(new System.Data.DataColumn("IntOrderItem_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                objTable.Columns.Add(new System.Data.DataColumn("Parts_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                objTable.Columns.Add(new System.Data.DataColumn("Measure_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                objTable.Columns.Add(new System.Data.DataColumn("IntWaybItem_Quantity", typeof(int)));
                objTable.Columns.Add(new System.Data.DataColumn("IntWaybItem_RetQuantity", typeof(int)));
                objTable.Columns.Add(new System.Data.DataColumn("IntWaybItem_PriceImporter", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("IntWaybItem_Price", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("IntWaybItem_Discount", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("IntWaybItem_DiscountPrice", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("IntWaybItem_RetailPrice", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("IntWaybItem_NDSPercent", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("IntWaybItem_MarkUpPercent", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("IntWaybItem_Id", typeof(int)));

                System.Data.DataRow newRow = null;
                foreach (CIntWaybillItem objItem in objIntWaybillItemsList)
                {
                    newRow = objTable.NewRow();
                    newRow["IntWaybItem_Guid"] = objItem.ID;
                    newRow["IntOrderItem_Guid"] = objItem.IntOrderItemID;
                    newRow["Parts_Guid"] = objItem.Product.ID;
                    newRow["Measure_Guid"] = objItem.Measure.ID;
                    newRow["IntWaybItem_Quantity"] = objItem.Quantity;
                    newRow["IntWaybItem_RetQuantity"] = objItem.QuantityReturn;
                    newRow["IntWaybItem_PriceImporter"] = (System.Data.SqlTypes.SqlMoney)objItem.PriceImporter;
                    newRow["IntWaybItem_Price"] = (System.Data.SqlTypes.SqlMoney)objItem.Price;
                    newRow["IntWaybItem_Discount"] = (System.Data.SqlTypes.SqlMoney)objItem.DiscountPercent;
                    newRow["IntWaybItem_DiscountPrice"] = (System.Data.SqlTypes.SqlMoney)objItem.PriceWithDiscount;
                    newRow["IntWaybItem_RetailPrice"] = (System.Data.SqlTypes.SqlMoney)objItem.PriceRetail;
                    newRow["IntWaybItem_NDSPercent"] = (System.Data.SqlTypes.SqlMoney)objItem.NDSPercent;
                    newRow["IntWaybItem_MarkUpPercent"] = (System.Data.SqlTypes.SqlMoney)objItem.MarkUpPercent;
                    newRow["IntWaybItem_Id"] = objItem.Ib_ID;
                    objTable.Rows.Add(newRow);
                }
                if (objIntWaybillItemsList.Count > 0)
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
        /// <summary>
        /// Признак "Накладную можно отгружать"
        /// </summary>
        public System.Boolean CanShip { get; set; }
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
            CanShip = false;
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

        #region Отгрузка накладной
        /// <summary>
        /// Отгрузка товара по накладной
        /// </summary>
        /// <param name="objProfile">Профайл</param>
        /// <param name="IntWaybill_Guid">УИ накладной на отгрузку</param>
        /// <param name="IntWaybill_ShipDate">Дата отгрузки</param>
        /// <param name="SetIntWaybillShipMode_Guid">УИ варианта отгрузки</param>
        /// <param name="IntWaybillState_Guid">УИ текущего состояния накладной</param>
        /// <param name="iErr">целочисленный код ошибки</param>
        /// <param name="strErr">текст ошибки</param>
        /// <returns>0 - накладная отгружена; <>0 - ошибка</returns>
        public static System.Int32 ShippedProductsByIntWaybill(UniXP.Common.CProfile objProfile,
            System.Guid IntWaybill_Guid, System.DateTime IntWaybill_ShipDate, System.Guid SetIntWaybillShipMode_Guid,
            ref System.Guid IntWaybillState_Guid, ref System.Int32 iErr, ref System.String strErr)
        {
            System.Int32 iRet = -1;
            try
            {
                iRet = CIntWaybillDataBaseModel.ShippedProductsByIntWaybill(objProfile, null,
                    IntWaybill_Guid, IntWaybill_ShipDate, SetIntWaybillShipMode_Guid, 
                    ref IntWaybillState_Guid, ref iErr, ref strErr);
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

        #region Установка признака "накладную можно отгружать"
        /// <summary>
        /// Проверка значений перед установкой признака "накладную можно отгружать"
        /// </summary>
        /// <param name="WaybillGuidList">список идентификаторов  накладных</param>
        /// <param name="strErr">текст ошибки</param>
        /// <returns>true - проверка пройдена; false - проверка не пройдена</returns>
        public static System.Boolean CheckAllPropertiesBeforeSetShipRemark(
            System.Data.DataTable WaybillGuidList, ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if ((WaybillGuidList == null) || (WaybillGuidList.Rows.Count == 0))
                {
                    strErr = "Список накладных не содержит записей. Добавьте, пожалуйста, хотя бы одну позицию.";
                    return bRet;
                }

                bRet = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "CheckAllPropertiesBeforeSetShipRemark.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return bRet;
        }
        /// <summary>
        /// Установка признака "накладную можно отгружать"
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="WaybillGuidList">список идентификаторов накладных</param>
        /// <param name="CanShip">признак "накладную можно отгружать"</param>
        /// <param name="strErr">текст ошибки</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean SetShipRemarkForWaybillList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
             List<System.Guid> WaybillGuidList, System.Boolean CanShip, ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                System.Data.DataTable dtWaybillList = CWaybill.ConvertWaybillGuidListToTable(WaybillGuidList, ref strErr);

                if (dtWaybillList != null)
                {
                    if (CheckAllPropertiesBeforeSetShipRemark(dtWaybillList, ref strErr) == true)
                    {
                        bRet = CIntWaybillDataBaseModel.SetShipRemarkForWaybillList(objProfile, null,
                           dtWaybillList, CanShip, ref strErr);
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

    }
}
