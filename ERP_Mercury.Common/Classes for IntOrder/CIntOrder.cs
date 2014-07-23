using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ERP_Mercury.Common
{
    /// <summary>
    /// Класс "Строка табличной части заказа на внутренее перемещение"
    /// </summary>
    public class CIntOrderItem
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
        /// Количество
        /// </summary>
        public System.Double Quantity { get; set; }
        /// <summary>
        /// Количество возвращенное
        /// </summary>
        public System.Double QuantityReturned { get; set; }
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
        /// Цена с учетом скидки
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
        /// Сумма розничная (в национальной валюте)
        /// </summary>
        public System.Double SumRetail { get { return (Quantity * PriceRetail); } }
        #endregion

        #region Конструктор

        public CIntOrderItem()
        {
            ID = System.Guid.Empty;
            Ib_ID = 0;
            Product = null;
            Measure = null;
            Quantity = 0;
            QuantityReturned = 0;
            NDSPercent = 0;
            MarkUpPercent = 0;
            DiscountPercent = 0;
            PriceImporter = 0;
            Price = 0;
            PriceWithDiscount = 0;
            PriceRetail = 0;
        }

        #endregion

        #region Приложение к заказу на внутреннее перемещение
        /// <summary>
        /// Возвращает приложение к заказу на внутреннее перемещение
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="uuidIntOrderId">уи заказа на внутреннее перемещение</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>приложение к заказу, как список объектов класса CIntOrderItem</returns>
        public static List<CIntOrderItem> GetIntOrderTablePart(UniXP.Common.CProfile objProfile,
            System.Guid uuidIntOrderId, ref System.String strErr )
        {
            List<CIntOrderItem> objList = new List<CIntOrderItem>();

            try
            {
                System.Data.DataTable dtList = CIntOrderDataBaseModel.GetIntOrderItemTable(objProfile, null, uuidIntOrderId, ref strErr );
                if (dtList != null)
                {
                    CIntOrderItem objOrderItem = null;
                    foreach (System.Data.DataRow objItem in dtList.Rows)
                    {
                        objOrderItem = new CIntOrderItem();

                        objOrderItem.ID = ((objItem["IntOrderItem_Guid"] != System.DBNull.Value) ? (System.Guid)objItem["IntOrderItem_Guid"] : System.Guid.Empty);
                        objOrderItem.Ib_ID = ((objItem["IntOrderItem_Id"] != System.DBNull.Value) ? System.Convert.ToInt32(objItem["IntOrderItem_Id"]) : 0);
                        objOrderItem.Product = ((objItem["Parts_Guid"] != System.DBNull.Value) ? new CProduct()
                        {
                            ID = (System.Guid)objItem["Parts_Guid"], 
                            ID_Ib = System.Convert.ToInt32(objItem["Parts_Id"]),
                            Name = System.Convert.ToString(objItem["PARTS_NAME"]),
                            Article = System.Convert.ToString(objItem["PARTS_ARTICLE"]),
                            ProductTradeMark = new CProductTradeMark() { Name = System.Convert.ToString(objItem["ProductOwnerName"]) }
                        } : null);

                        objOrderItem.Measure = ((objItem["Measure_Guid"] != System.DBNull.Value) ? new CMeasure()
                        {
                            ID = (System.Guid)objItem["Measure_Guid"],
                            ShortName = System.Convert.ToString(objItem["Measure_ShortName"])
                        } : null);

                        objOrderItem.Quantity = ((objItem["IntOrderItem_Quantity"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["IntOrderItem_Quantity"]) : 0);

                        objOrderItem.NDSPercent = ((objItem["IntOrderItem_NDSPercent"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["IntOrderItem_NDSPercent"]) : 0);
                        objOrderItem.MarkUpPercent = ((objItem["IntOrderItem_MarkUpPercent"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["IntOrderItem_MarkUpPercent"]) : 0);
                        objOrderItem.PriceImporter = ((objItem["IntOrderItem_PriceImporter"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["IntOrderItem_PriceImporter"]) : 0);
                        objOrderItem.Price = ((objItem["IntOrderItem_Price"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["IntOrderItem_Price"]) : 0);
                        objOrderItem.DiscountPercent = ((objItem["IntOrderItem_Discount"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["IntOrderItem_Discount"]) : 0);
                        objOrderItem.PriceWithDiscount = ((objItem["IntOrderItem_DiscountPrice"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["IntOrderItem_DiscountPrice"]) : 0);
                        objOrderItem.PriceRetail = ((objItem["IntOrderItem_RetailPrice"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["IntOrderItem_RetailPrice"]) : 0);


                        if (objOrderItem != null) { objList.Add(objOrderItem); }

                    }
                }

                dtList = null;

            }
            catch (System.Exception f)
            {
                strErr += (String.Format("\nНе удалось получить приложение к заказу.\nТекст ошибки: {0}", f.Message));
            }
            return objList;
        }


        /// <summary>
        /// Преобразует список записей к табличному виду
        /// </summary>
        /// <param name="objIntOrderItemsList">список строк из приложения к заказу на внутреннее перемещение</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>таблица с приложением к накладной</returns>
        public static System.Data.DataTable ConvertListToTable(List<CIntOrderItem> objIntOrderItemsList, ref System.String strErr)
        {
            System.Data.DataTable objTable = new System.Data.DataTable();
            try
            {
                objTable.Columns.Add(new System.Data.DataColumn("IntOrderItem_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                objTable.Columns.Add(new System.Data.DataColumn("Parts_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                objTable.Columns.Add(new System.Data.DataColumn("Measure_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                objTable.Columns.Add(new System.Data.DataColumn("IntOrderItem_Quantity", typeof(int)));
                objTable.Columns.Add(new System.Data.DataColumn("IntOrderItem_PriceImporter", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("IntOrderItem_Price", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("IntOrderItem_Discount", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("IntOrderItem_DiscountPrice", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("IntOrderItem_RetailPrice", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("IntOrderItem_NDSPercent", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("IntOrderItem_MarkUpPercent", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("IntOrderItem_Id", typeof(int)));

                System.Data.DataRow newRow = null;
                foreach( CIntOrderItem objItem in objIntOrderItemsList )
                {
                    newRow = objTable.NewRow();
                    newRow["IntOrderItem_Guid"] = objItem.ID;
                    newRow["Parts_Guid"] = objItem.Product.ID;
                    newRow["Measure_Guid"] = objItem.Measure.ID;
                    newRow["IntOrderItem_Quantity"] = objItem.Quantity;
                    newRow["IntOrderItem_PriceImporter"] = (System.Data.SqlTypes.SqlMoney)objItem.PriceImporter;
                    newRow["IntOrderItem_Price"] = (System.Data.SqlTypes.SqlMoney)objItem.Price;
                    newRow["IntOrderItem_Discount"] = (System.Data.SqlTypes.SqlMoney)objItem.DiscountPercent;
                    newRow["IntOrderItem_DiscountPrice"] = (System.Data.SqlTypes.SqlMoney)objItem.PriceWithDiscount;
                    newRow["IntOrderItem_RetailPrice"] = (System.Data.SqlTypes.SqlMoney)objItem.PriceRetail;
                    newRow["IntOrderItem_NDSPercent"] = (System.Data.SqlTypes.SqlMoney)objItem.NDSPercent;
                    newRow["IntOrderItem_MarkUpPercent"] = (System.Data.SqlTypes.SqlMoney)objItem.MarkUpPercent;
                    newRow["IntOrderItem_Id"] = objItem.Ib_ID;
                    objTable.Rows.Add(newRow);
                }
                if (objIntOrderItemsList.Count > 0)
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

    
    public class CIntOrder
    {
        #region Уникальные идентификаторы
        /// <summary>
        /// Уникальный идентификатор заказа
        /// </summary>
        public System.Guid ID { get; set; }
        /// <summary>
        /// Уникальный идентификатор заказа в InterBase
        /// </summary>
        public System.Int32 Ib_ID { get; set; }
        /// <summary>
        /// Уникальный идентификатор заказа-родителя
        /// </summary>
        public System.Guid ParentID { get; set; }
        /// <summary>
        /// Номер заказа
        /// </summary>
        public System.String DocNum { get; set; }
        /// <summary>
        /// Дата создания заказа
        /// </summary>
        public System.DateTime BeginDate { get; set; }
        #endregion

        #region Отгрузка
        /// <summary>
        /// Дата отгрузки заказа
        /// </summary>
        public System.DateTime ShipDate { get; set; }
        /// <summary>
        /// Вид отгрузки заказа
        /// </summary>
        public CIntOrderShipMode IntOrderShipMode { get; set; }
        /// <summary>
        /// Вид отгрузки заказа
        /// </summary>
        public System.String IntOrderShipModeName
        {
            get { return ((IntOrderShipMode == null) ? System.String.Empty : IntOrderShipMode.Name); }
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
        /// Состояние заказа
        /// </summary>
        public CIntOrderState DocState { get; set; }
        /// <summary>
        /// Состояние заказа
        /// </summary>
        public System.String DocStateName
        {
            get { return ((DocState == null) ? System.String.Empty : DocState.Name); }
        }
        /// <summary>
        /// Состояние заказа
        /// </summary>
        public System.Int32 DocStateId
        {
            get { return ((DocState == null) ? -1 : DocState.IntOrderStateId); }
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
        /// Количество
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
        /// Курс ценообразования на дату формаирования заказа
        /// </summary>
        public System.Double PricingCurrencyRate { get; set; }
        /// <summary>
        /// Сумма заказа без учета скидки (в национальной валюте)
        /// </summary>
        public System.Double SumOrder { get; set; }
        /// <summary>
        /// Сумма розничная
        /// </summary>
        public System.Double SumRetail { get; set; }
        /// <summary>
        /// Сумма скидки по заказу (в национальной валюте)
        /// </summary>
        public System.Double SumDiscount { get; set; }
        /// <summary>
        /// Сумма заказа с учетом скидки (в национальной валюте)
        /// </summary>
        public System.Double SumWithDiscount { get { return (SumOrder - SumDiscount); } }
        /// <summary>
        /// Вес товара в заказе
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
        /// Приложение к заказу
        /// </summary>
        public List<CIntOrderItem> IntOrderItemList { get; set; }
        #endregion

        #region Конструктор
        public CIntOrder()
        {
            ID = System.Guid.Empty;
            Ib_ID = 0;
            ParentID = System.Guid.Empty;
            DocNum = System.String.Empty;
            BeginDate = System.DateTime.MinValue;
            IntOrderShipMode = null;
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
            IntOrderItemList = null;
            Quantity = 0;
            PricingCurrencyRate = 0;
            SumOrder = 0;
            SumRetail = 0;
            SumDiscount = 0;
        }
        #endregion

        #region Журнал заказов
        /// <summary>
        /// Возвращает список заказов за указанный период
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="IntOrder_Guid">УИ документа</param>
        /// <param name="IntOrder_DateBegin">начало периода для выборки</param>
        /// <param name="IntOrder_DateEnd">конец периода для выборки</param>
        /// <param name="IntOrder_SrcCompanyGuid">УИ компании "Откуда"</param>
        /// <param name="IntOrder_SrcStockGuid">УИ склада "Откуда"<</param>
        /// <param name="IntOrder_DstCompanyGuid">УИ компании "Куда"</param>
        /// <param name="IntOrder_DstStockGuid">УИ склада "Куда"<</param>
        /// <param name="IntOrder_PaymentTypeGuid">УИ формы оплаты</param>
        /// <param name="strErr">текст ошибки</param>
        /// <param name="OnlyUnShippedOrders">признак "запрос только НЕ отгруженных накладных"</param>
        /// <returns>список объектов класса "CIntOrder"</returns>
        public static List<CIntOrder> GetIntOrderList(UniXP.Common.CProfile objProfile,
            System.Guid IntOrder_Guid,
            System.DateTime IntOrder_DateBegin, System.DateTime IntOrder_DateEnd,
            System.Guid IntOrder_SrcCompanyGuid, System.Guid IntOrder_SrcStockGuid,
            System.Guid IntOrder_DstCompanyGuid, System.Guid IntOrder_DstStockGuid,
            System.Guid IntOrder_PaymentTypeGuid, ref System.String strErr,
            System.Boolean OnlyUnShippedOrders = false
            )
        {
            List<CIntOrder> objList = new List<CIntOrder>();

            try
            {
                // вызов статического метода из класса, связанного с БД
                System.Data.DataTable dtList = CIntOrderDataBaseModel.GetIntOrderTable(objProfile, null, IntOrder_Guid,
                    IntOrder_DateBegin, IntOrder_DateEnd, IntOrder_SrcCompanyGuid, IntOrder_SrcStockGuid,
                    IntOrder_DstCompanyGuid, IntOrder_DstStockGuid,
                    IntOrder_PaymentTypeGuid, ref strErr,
                    OnlyUnShippedOrders);
                if (dtList != null)
                {
                    CIntOrder objOrder = null;
                    System.Int32 objOrder_Ib_ID = 0;
                    foreach (System.Data.DataRow objItem in dtList.Rows)
                    {
                        objOrder = new CIntOrder();
                        objOrder_Ib_ID = System.Convert.ToInt32(objItem["IntOrder_Id"]);
                        objOrder.ID = ((objItem["IntOrder_Guid"] != System.DBNull.Value) ? new System.Guid(System.Convert.ToString(objItem["IntOrder_Guid"])) : System.Guid.Empty);
                        objOrder.ParentID = ((objItem["IntOrderParent_Guid"] != System.DBNull.Value) ? new System.Guid(System.Convert.ToString(objItem["IntOrderParent_Guid"])) : System.Guid.Empty);
                        objOrder.StockSrc = ((objItem["SrcStock_Guid"] != System.DBNull.Value) ? new CStock()
                        {
                            ID = (System.Guid)objItem["SrcStock_Guid"],
                            IBId = System.Convert.ToInt32(objItem["SrcStock_Id"]),
                            Name = System.Convert.ToString(objItem["SrcStock_Name"]),
                            IsAcitve = System.Convert.ToBoolean(objItem["SrcStock_IsActive"]),
                            IsTrade = System.Convert.ToBoolean(objItem["SrcStock_IsTrade"]),
                            WareHouse = new CWarehouse() { ID = (System.Guid)objItem["SrcWarehouse_Guid"] },
                            WareHouseType = new CWareHouseType() { ID = (System.Guid)objItem["SrcWarehouseType_Guid"] }
                        } : null);
                        objOrder.CompanySrc = ((objItem["SrcCompany_Guid"] != System.DBNull.Value) ? new CCompany()
                        {
                            ID = (System.Guid)objItem["SrcCompany_Guid"],
                            InterBaseID = System.Convert.ToInt32(objItem["SrcCompany_Id"]),
                            Abbr = System.Convert.ToString(objItem["SrcCompany_Acronym"]),
                            Name = System.Convert.ToString(objItem["SrcCompany_Name"])
                        } : null);
                        objOrder.StockDst = ((objItem["DstStock_Guid"] != System.DBNull.Value) ? new CStock()
                        {
                            ID = (System.Guid)objItem["DstStock_Guid"],
                            IBId = System.Convert.ToInt32(objItem["DstStock_Id"]),
                            Name = System.Convert.ToString(objItem["DstStock_Name"]),
                            IsAcitve = System.Convert.ToBoolean(objItem["DstStock_IsActive"]),
                            IsTrade = System.Convert.ToBoolean(objItem["DstStock_IsTrade"]),
                            WareHouse = new CWarehouse() { ID = (System.Guid)objItem["DstWarehouse_Guid"] },
                            WareHouseType = new CWareHouseType() { ID = (System.Guid)objItem["DstWarehouseType_Guid"] }
                        } : null);
                        objOrder.CompanyDst = ((objItem["DstCompany_Guid"] != System.DBNull.Value) ? new CCompany()
                        {
                            ID = (System.Guid)objItem["DstCompany_Guid"],
                            InterBaseID = System.Convert.ToInt32(objItem["DstCompany_Id"]),
                            Abbr = System.Convert.ToString(objItem["DstCompany_Acronym"]),
                            Name = System.Convert.ToString(objItem["DstCompany_Name"])
                        } : null);

                        objOrder.Currency = ((objItem["Currency_Guid"] != System.DBNull.Value) ? new CCurrency()
                        {
                            ID = (System.Guid)objItem["Currency_Guid"],
                            CurrencyAbbr = System.Convert.ToString(objItem["Currency_Abbr"])
                        } : null);

                        objOrder.Depart = ((objItem["Depart_Guid"] != System.DBNull.Value) ? new CDepart()
                        {
                            uuidID = (System.Guid)objItem["Depart_Guid"],
                            DepartCode = System.Convert.ToString(objItem["Depart_Code"])
                        } : null);


                        objOrder.PaymentType = ((objItem["PaymentType_Guid"] != System.DBNull.Value) ? new CPaymentType(
                            (System.Guid)objItem["PaymentType_Guid"], System.Convert.ToString(objItem["PaymentType_Name"])) : null);

                        objOrder.BeginDate = ((objItem["IntOrder_BeginDate"] != System.DBNull.Value) ? System.Convert.ToDateTime(objItem["IntOrder_BeginDate"]) : System.DateTime.MinValue);
                        if (objItem["IntOrder_ShipDate"] != System.DBNull.Value)
                        {
                            objOrder.ShipDate = System.Convert.ToDateTime(objItem["IntOrder_ShipDate"]);
                        }
                        objOrder.DocNum = ((objItem["IntOrder_Num"] != System.DBNull.Value) ? System.Convert.ToString(objItem["IntOrder_Num"]) : System.String.Empty);

                        objOrder.DocState = ((objItem["IntOrderState_Guid"] != System.DBNull.Value) ? new CIntOrderState()
                        {
                            ID = (System.Guid)objItem["IntOrderState_Guid"],
                            IntOrderStateId = System.Convert.ToInt32(objItem["IntOrderState_Id"]),
                            Name = System.Convert.ToString(objItem["IntOrderState_Name"])
                        } : null);

                        objOrder.IntOrderShipMode = ((objItem["IntOrderShipMode_Guid"] != System.DBNull.Value) ? new CIntOrderShipMode()
                        {
                            ID = (System.Guid)objItem["IntOrderShipMode_Guid"],
                            IntOrderShipModeId = System.Convert.ToInt32(objItem["IntOrderShipMode_Id"]),
                            Name = System.Convert.ToString(objItem["IntOrderShipMode_Name"])
                        } : null);

                        objOrder.Description = ((objItem["IntOrder_Description"] != System.DBNull.Value) ? System.Convert.ToString(objItem["IntOrder_Description"]) : System.String.Empty);

                        objOrder.SumOrder = ((objItem["IntOrder_AllPrice"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["IntOrder_AllPrice"]) : 0);
                        objOrder.SumDiscount = ((objItem["IntOrder_AllDiscount"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["IntOrder_AllDiscount"]) : 0);
                        objOrder.SumRetail = ((objItem["IntOrder_RetailAllPrice"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["IntOrder_RetailAllPrice"]) : 0);

                        objOrder.Quantity = ((objItem["IntOrder_Quantity"] != System.DBNull.Value) ? System.Convert.ToDouble(objItem["IntOrder_Quantity"]) : 0);

                        if (objOrder != null) { objList.Add(objOrder); }

                    }
                }

                dtList = null;

            }
            catch (System.Exception f)
            {
                strErr += (String.Format("\nНе удалось получить список заказов на внутреннее перемещение.\nТекст ошибки: {0}", f.Message));
            }
            return objList;
        }
        /// <summary>
        /// Возвращает объект класса "Заказ на внутреннее перемещение"
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="IntOrder_Guid">УИ заказа</param>
        /// <param name="strErr">текст ошибки</param>
        /// <returns>объект класса "Заказ на внутреннее перемещение"</returns>
        public static CIntOrder GetIntOrder(UniXP.Common.CProfile objProfile, System.Guid IntOrder_Guid,
            ref System.String strErr)
        {
            CIntOrder objOrder = null;

            try
            {
                // вызов статического метода из класса, связанного с БД
                List<CIntOrder> objList = CIntOrder.GetIntOrderList(objProfile, IntOrder_Guid, 
                    System.DateTime.MinValue, System.DateTime.MinValue,
                    System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, System.Guid.Empty, ref strErr, false);
                if ((objList != null) && (objList.Count > 0))
                {
                    objOrder = objList[0];
                }

                objList = null;

            }
            catch (System.Exception f)
            {
                strErr += (String.Format("\nНе удалось получить информацию о заказе.\nТекст ошибки: {0}", f.Message));
            }
            return objOrder;
        }

        #endregion

        #region Цена для позиции в заказе
        /// <summary>
        /// Возвращает цену для позиции в заказе на внутреннее перемещение
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="uuidPartsId">УИ товара</param>
        /// <param name="uuidStockSrcId">УИ склада-источника</param>
        /// <param name="uuidPaymentTypeId">УИ формы оплаты</param>
        /// <param name="dblDiscountPercent">Размер скидки, %</param>
        /// <param name="uuidPartsubtypePriceTypeId">Уровень цены</param>
        /// <param name="PriceImporter">Цена импортера</param>
        /// <param name="Price">Цена</param>
        /// <param name="PriceWithDiscount">Цена с учетом скидки</param>
        /// <param name="PriceRetail">Цена розничная</param>
        /// <param name="NDSPercent">Ставка НДС, %</param>
        /// <param name="MarkUpPercent">Размер надбавки, %</param>
        /// <param name="strErr">текст ошибки</param>
        /// <param name="PriceInput">входная цена</param>
        /// <param name="InputPriceIsFixed">признак "фиксированная входная цена"</param>
        /// <returns>true - цены рассчитаны; false - не удалось получить цены для позиции</returns>
        public static System.Boolean GetPriceForOrderItem(UniXP.Common.CProfile objProfile, System.Guid uuidPartsId,
            System.Guid uuidStockSrcId, System.Guid uuidPartsubtypePriceTypeId, System.Double dblDiscountPercent, 
            ref System.Double PriceImporter, ref System.Double Price, ref System.Double PriceWithDiscount,
            ref System.Double PriceRetail, ref System.Double NDSPercent, ref System.Double MarkUpPercent, ref System.String strErr,
            System.Double PriceInput = 0, System.Boolean InputPriceIsFixed = false)
        {
            System.Boolean bRet = false;

            PriceImporter = 0;
            Price = 0;
            PriceWithDiscount = 0;
            PriceRetail = 0;
            NDSPercent = 0;
            MarkUpPercent = 0;

            try
            {
                bRet = CIntOrderDataBaseModel.GetPriceForOrderItem(objProfile, uuidPartsId,
                    uuidStockSrcId, uuidPartsubtypePriceTypeId, dblDiscountPercent,
                    ref PriceImporter, ref Price, ref PriceWithDiscount,
                    ref PriceRetail, ref NDSPercent, ref MarkUpPercent, ref strErr,
                    PriceInput, InputPriceIsFixed);
            }
            catch (System.Exception f)
            {
                strErr += (String.Format("Не удалось получить цены для позиции. Текст ошибки: {0}", f.Message));
            }

            return bRet;
        }

        #endregion

        #region реквизиты по умолчанию для оформления нового заказа

        public static System.Boolean GetOrderDefParams( UniXP.Common.CProfile objProfile, 
            System.Guid In_PaymentType_Guid,  ref System.Boolean SetDepartValue,
            ref System.Guid Depart_Guid, ref System.Guid PaymentType_Guid,  ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
            }
            catch (System.Exception f)
            {
                strErr += f.Message;
            }
            return bRet;
        }

        #endregion

        #region Аннулирование заказа
        /// <summary>
        /// Аннулирование заказа
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="IntOrder_Guid">УИ заказа</param>
        /// <param name="IntOrderState_Guid">УИ состояния заказа</param>
        /// <param name="strErr">текст ошибки</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean CancelIntOrder(UniXP.Common.CProfile objProfile,
            System.Guid IntOrder_Guid, ref System.Guid IntOrderState_Guid, ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                bRet = CIntOrderDataBaseModel.CancelIntOrder( objProfile, null, IntOrder_Guid, ref IntOrderState_Guid, ref strErr );
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

        #region Сохранение заказа в БД
        /// <summary>
        /// Проверяет заполнение свойств документа перед сохранением в БД
        /// </summary>
        /// <param name="Doc_BeginDate">дата документа</param>
        /// <param name="Doc_Num">номер документа</param>
        /// <param name="ShipMode_Guid">УИ вида отгрузки</param>
        /// <param name="PaymentType_Guid">УИ формы оплаты</param>
        /// <param name="StockSrc_Guid">УИ склада-источника</param>
        /// <param name="StockDst_Guid">УИ склада-назанчения</param>
        /// <param name="Depart_Guid">УИ торгового подразделения</param>
        /// <param name="Salesman_Guid">УИ торгового представителя</param>
        /// <param name="DocTablePart">приложение к документу</param>
        /// <param name="strErr">текст ошибки</param>
        /// <returns>true - все обязательные поля заполнены; false - не все обязазательные поля заполнены</returns>
        public static System.Boolean CheckAllPropertiesForSave(
            System.DateTime Doc_BeginDate, System.String Doc_Num, 
            System.Guid ShipMode_Guid, System.Guid PaymentType_Guid,
            System.Guid StockSrc_Guid, System.Guid StockDst_Guid,
            System.Guid Depart_Guid, System.Guid Salesman_Guid, 
            System.Data.DataTable DocTablePart, ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (Doc_BeginDate == System.DateTime.MinValue)
                {
                    strErr = "Укажите, пожалуйста, дату документа.";
                    return bRet;
                }
                if (Doc_Num.Trim().Length == 0)
                {
                    strErr = "Укажите, пожалуйста, номер документа.";
                    return bRet;
                }
                if (ShipMode_Guid == System.Guid.Empty)
                {
                    strErr = "Укажите, пожалуйста, вид отгрузки.";
                    return bRet;
                }
                if (PaymentType_Guid == System.Guid.Empty)
                {
                    strErr = "Укажите, пожалуйста, форму оплаты.";
                    return bRet;
                }
                if (Depart_Guid == System.Guid.Empty)
                {
                    strErr = "Укажите, пожалуйста, подразделение.";
                    return bRet;
                }
                if (Salesman_Guid == System.Guid.Empty)
                {
                    strErr = "Укажите, пожалуйста, торгового представителя.";
                    return bRet;
                }
                if (StockSrc_Guid == System.Guid.Empty)
                {
                    strErr = "Укажите, пожалуйста, склад-источник.";
                    return bRet;
                }
                if (StockDst_Guid == System.Guid.Empty)
                {
                    strErr = "Укажите, пожалуйста, склад-получатель.";
                    return bRet;
                }
                if ((DocTablePart == null) || (DocTablePart.Rows.Count == 0))
                {
                    strErr = "Приложение к документу не содержит записей. Добавьте, пожалуйста, хотя бы одну позицию.";
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
        /// Добавляет заказ в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="Doc_BeginDate">дата документа</param>
        /// <param name="Doc_Num">номер документа</param>
        /// <param name="Doc_ShipDate">дата отгрузки документа</param>
        /// <param name="DocShipMode_Guid">УИ вида отгрузки</param>
        /// <param name="PaymentType_Guid">УИ вида платежа</param>
        /// <param name="StockSrc_Guid">УИ склада-источника</param>
        /// <param name="StockDst_Guid">УИ склада-получателя</param>
        /// <param name="Depart_Guid">УИ торгового подразделения</param>
        /// <param name="Salesman_Guid">УИ торгового представителя</param>
        /// <param name="Doc_Description">Примечание к документу</param>
        /// <param name="DocParent_Guid">УИ родительского документа</param>
        /// <param name="DocTablePart">приложение к документу</param>
        /// <param name="strErr">текст ошибки</param>
        /// <param name="Doc_Guid">УИ документа</param>
        /// <param name="Doc_Id">УИ документа (InterBase)</param>
        /// <param name="DocState_Guid">УИ состояния документа</param>
        /// <param name="DocumentSendToStock">признак "уведомить склад"</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean AddNewDocToDB(
            UniXP.Common.CProfile objProfile, 
            System.DateTime Doc_BeginDate, System.String Doc_Num, System.DateTime Doc_ShipDate,
            System.Guid DocShipMode_Guid, System.Guid PaymentType_Guid, 
            System.Guid StockSrc_Guid, System.Guid StockDst_Guid,
            System.Guid Depart_Guid, System.Guid Salesman_Guid, 
            System.String Doc_Description, System.Guid DocParent_Guid,
            System.Data.DataTable DocTablePart, ref System.String strErr,
            ref System.Guid Doc_Guid, ref System.Int32 Doc_Id, ref System.Guid DocState_Guid, 
            System.Boolean DocumentSendToStock = false )
        {
            System.Boolean bRet = false;
            try
            {
                bRet = CIntOrderDataBaseModel.AddNewDocToDB(objProfile, null,
                    Doc_BeginDate, Doc_Num, Doc_ShipDate, DocShipMode_Guid, PaymentType_Guid, 
                    StockSrc_Guid, StockDst_Guid, Depart_Guid, Salesman_Guid, 
                    Doc_Description, DocParent_Guid, DocTablePart,
                    ref strErr, ref Doc_Guid, ref Doc_Id, ref DocState_Guid,
                    DocumentSendToStock);
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
        /// <summary>
        /// Редактирует реквизиты заказа в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="Doc_Guid">УИ документа</param>
        /// <param name="DocState_Guid">УИ состояния документа</param>
        /// <param name="Doc_BeginDate">дата документа</param>
        /// <param name="Doc_Num">номер документа</param>
        /// <param name="Doc_ShipDate">дата отгрузки документа</param>
        /// <param name="DocShipMode_Guid">УИ вида отгрузки</param>
        /// <param name="PaymentType_Guid">УИ вида платежа</param>
        /// <param name="StockSrc_Guid">УИ склада-источника</param>
        /// <param name="StockDst_Guid">УИ склада-получателя</param>
        /// <param name="Depart_Guid">УИ торгового подразделения</param>
        /// <param name="Salesman_Guid">УИ торгового представителя</param>
        /// <param name="Doc_Description">Примечание к документу</param>
        /// <param name="DocParent_Guid">УИ родительского документа</param>
        /// <param name="DocTablePart">приложение к документу</param>
        /// <param name="strErr">текст ошибки</param>
        /// <param name="DocumentSendToStock">признак "уведомить склад"</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean EditDocInDB(
            UniXP.Common.CProfile objProfile,
            System.Guid Doc_Guid, System.Guid DocState_Guid,
            System.DateTime Doc_BeginDate, System.String Doc_Num, System.DateTime Doc_ShipDate,
            System.Guid DocShipMode_Guid, System.Guid PaymentType_Guid,
            System.Guid StockSrc_Guid, System.Guid StockDst_Guid,
            System.Guid Depart_Guid, System.Guid Salesman_Guid,
            System.String Doc_Description, System.Guid DocParent_Guid,
            System.Data.DataTable DocTablePart, ref System.String strErr,
            System.Boolean DocumentSendToStock = false)
        {
            System.Boolean bRet = false;
            try
            {
                bRet = CIntOrderDataBaseModel.EditDocInDB(objProfile, null, Doc_Guid, DocState_Guid, 
                    Doc_BeginDate, Doc_Num, Doc_ShipDate, DocShipMode_Guid, PaymentType_Guid,
                    StockSrc_Guid, StockDst_Guid, Depart_Guid, Salesman_Guid,
                    Doc_Description, DocParent_Guid, DocTablePart,
                    ref strErr, 
                    DocumentSendToStock);
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
