using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ERP_Mercury.Common
{
    /// <summary>
    /// Класс для работы с базой данных в контексте объектов класса COrder
    /// </summary>
    public static class COrderRepository
    {
        #region Заказ
        /// <summary>
        /// Возвращает список заказов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="dtBeginDate">начало периода, в который попадает дата создания заказа</param>
        /// <param name="dtEndDate">конец периода, в который попадает дата создания заказа</param>
        /// <param name="uuidCustomerId">уи клиента, на которого выписан заказ</param>
        /// <param name="uuidCompanyId">уи компании, со склада которой выписан заказ</param>
        /// <param name="uuidStockId">уи склада, с остатков которого выписан заказ</param>
        /// <param name="uuidPaymentTypeId">уи формы оплаты</param>
        /// <param name="uuidOrderId">уи заказа</param>
        /// <param name="bSearchByOrderDeliveryDate">признак "поиск по дате доставки"</param>
        /// <returns>список заказов</returns>
        public static List<COrder> GetOrderList(UniXP.Common.CProfile objProfile, System.DateTime dtBeginDate,
            System.DateTime dtEndDate, System.Guid uuidCustomerId, System.Guid uuidCompanyId, System.Guid uuidStockId,
            System.Guid uuidPaymentTypeId, System.Guid uuidOrderId, System.Boolean bSearchByOrderDeliveryDate = false )
        {
            List<COrder> objList = new List<COrder>();
            // подключаемся к БД
            System.Data.SqlClient.SqlConnection DBConnection = objProfile.GetDBSource();
            if (DBConnection == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Отсутствует соединение с БД.", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return objList;
            }

            try
            {
                // соединение с БД получено, прописываем команду на выборку данных
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetSuppl]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                if (uuidOrderId.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Order_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@Order_Guid"].Value = uuidOrderId;
                }
                else
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SearchByOrderDeliveryDate", System.Data.SqlDbType.Bit));
                    cmd.Parameters["@SearchByOrderDeliveryDate"].Value = bSearchByOrderDeliveryDate;
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BeginDate", System.Data.SqlDbType.Date));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EndDate", System.Data.SqlDbType.Date));
                    cmd.Parameters["@BeginDate"].Value = dtBeginDate;
                    cmd.Parameters["@EndDate"].Value = dtEndDate;

                    if (uuidCustomerId.CompareTo(System.Guid.Empty) != 0)
                    {
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters["@Customer_Guid"].Value = uuidCustomerId;
                    }
                    if (uuidCompanyId.CompareTo(System.Guid.Empty) != 0)
                    {
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters["@Company_Guid"].Value = uuidCompanyId;
                    }
                    if (uuidStockId.CompareTo(System.Guid.Empty) != 0)
                    {
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_Guid", System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters["@Stock_Guid"].Value = uuidStockId;
                    }
                    if (uuidPaymentTypeId.CompareTo(System.Guid.Empty) != 0)
                    {
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PaymentType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters["@PaymentType_Guid"].Value = uuidPaymentTypeId;
                    }

                }

                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    COrder objOrder = null;
                    

                    while (rs.Read())
                    {
                        objOrder = new COrder();
                        
                        objOrder.ID = (System.Guid)rs["Suppl_Guid"];
                        objOrder.Num = System.Convert.ToInt32(rs["Suppl_Num"]);
                        objOrder.SubNum = System.Convert.ToInt32(rs["Suppl_Version"]);
                        objOrder.BeginDate = System.Convert.ToDateTime(rs["Suppl_BeginDate"]);
                        objOrder.Product = new CProduct();
                        objOrder.DeliveryDate = System.Convert.ToDateTime(rs["Suppl_DeliveryDate"]);
                        objOrder.IsBonus = System.Convert.ToBoolean(rs["Suppl_Bonus"]);
                        objOrder.Description = ((rs["Suppl_Note"] == System.DBNull.Value) ? System.String.Empty : System.Convert.ToString(rs["Suppl_Note"]));
                        objOrder.Customer = ((rs["Customer_Guid"] == System.DBNull.Value) ? null : new CCustomer() { ID = (System.Guid)rs["Customer_Guid"], InterBaseID = System.Convert.ToInt32(rs["Customer_Id"]), ShortName = System.Convert.ToString(rs["Customer_Name"]), FullName = System.Convert.ToString(rs["Customer_Name"]) });
                        objOrder.ChildDepart = ((rs["CustomerChild_Guid"] == System.DBNull.Value) ? null : new CChildDepart() { ID = (System.Guid)rs["ChildDepart_Guid"], Code = System.Convert.ToString(rs["ChildDepart_Code"]), Name = System.Convert.ToString(rs["ChildDepart_Name"]) });
                        objOrder.Depart = ((rs["Depart_Guid"] == System.DBNull.Value) ? null : new CDepart() { uuidID = (System.Guid)rs["Depart_Guid"], DepartCode = System.Convert.ToString(rs["Depart_Code"]) } );
                        objOrder.SalesMan = ((rs["Salesman_Guid"] == System.DBNull.Value) ? null : new CSalesMan() { uuidID = (System.Guid)rs["Salesman_Guid"], ID = System.Convert.ToInt32(rs["Salesman_Id"]), User = new CUser() { uuidID = (System.Guid)rs["User_Guid"], LastName = System.Convert.ToString(rs["User_LastName"]), FirstName = System.Convert.ToString(rs["User_FirstName"]) } });
                        objOrder.OrderState = ((rs["SupplState_Guid"] == System.DBNull.Value) ? null : new COrderState() { ID = (System.Guid)rs["SupplState_Guid"], Name = System.Convert.ToString(rs["SupplState_Name"]), StateId = System.Convert.ToInt32(rs["SupplState_Id"]) } );
                        objOrder.OrderType = ((rs["OrderType_Guid"] == System.DBNull.Value) ? null : new COrderType((System.Guid)rs["OrderType_Guid"], System.Convert.ToString(rs["OrderType_Name"])));
                        objOrder.PaymentType = ((rs["PaymentType_Guid"] == System.DBNull.Value) ? null : new CPaymentType((System.Guid)rs["PaymentType_Guid"], System.Convert.ToString(rs["PaymentType_Name"])) { Payment_Id = System.Convert.ToInt32(rs["PaymentType_Id"]) } );
                        objOrder.AddressDelivery = ((rs["Address_Guid"] == System.DBNull.Value) ? null : new CAddress() { ID = (System.Guid)rs["Address_Guid"], Name = System.Convert.ToString(rs["AddressName"]) });
                        objOrder.Rtt = ((rs["Rtt_Guid"] == System.DBNull.Value) ? null : new CRtt() { ID = (System.Guid)rs["Rtt_Guid"], Code = System.Convert.ToString(rs["Rtt_Code"]), ShortName = System.Convert.ToString(rs["Rtt_Name"]), FullName = System.Convert.ToString(rs["Rtt_Name"]) });
                        objOrder.Stock = ((rs["Stock_Guid"] == System.DBNull.Value) ? null : new CStock()
                        {
                            ID = (System.Guid)rs["Stock_Guid"],
                            IBId = ((rs["Stock_Id"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Stock_Id"])),
                            Company = ((rs["Company_Guid"] == System.DBNull.Value) ? null : new CCompany() { ID = (System.Guid)rs["Company_Guid"], Abbr = System.Convert.ToString(rs["Company_Acronym"]), Name = System.Convert.ToString(rs["Company_Name"]) } ),
                            WareHouse = ((rs["Warehouse_Guid"] == System.DBNull.Value) ? null : new CWarehouse() { ID = (System.Guid)rs["Warehouse_Guid"], Name = System.Convert.ToString(rs["Warehouse_Name"]), IBId = System.Convert.ToInt32(rs["Warehouse_Id"]) })
                        });
                        objOrder.PosQuantity = ((rs["POSQUANTITY"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["POSQUANTITY"]));
                        objOrder.QuantityReserved = ((rs["QUANTITY"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["QUANTITY"]));
                        objOrder.QuantityOrdered = ((rs["ORDERQUANTITY"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["ORDERQUANTITY"]));
                        objOrder.SumReserved = ((rs["Suppl_AllPrice"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["Suppl_AllPrice"]));
                        objOrder.SumReservedWithDiscount = ((rs["Suppl_TotalPrice"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["Suppl_TotalPrice"]));
                        objOrder.SumReservedInAccountingCurrency = ((rs["Suppl_CurrencyAllPrice"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["Suppl_CurrencyAllPrice"]));
                        objOrder.SumReservedWithDiscountInAccountingCurrency = ((rs["Suppl_CurrencyTotalPrice"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["Suppl_CurrencyTotalPrice"]));
                        objOrder.Ib_ID = ((rs["Suppl_Id"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Suppl_Id"]));
                        objOrder.DirectionDeliveryName = ((rs["DirectionDeliveryName"] == System.DBNull.Value) ? System.String.Empty : System.Convert.ToString(rs["DirectionDeliveryName"]));
                        objOrder.DirectionDeliveryCityName = ((rs["DirectionDeliveryCityName"] == System.DBNull.Value) ? System.String.Empty : System.Convert.ToString(rs["DirectionDeliveryCityName"]));

                        objList.Add(objOrder);
                    }
                }
                rs.Close();
                rs.Dispose();
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список заказов.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
    			finally // очищаем занимаемые ресурсы
            {
                DBConnection.Close();
            }
            return objList;
        }

        /// <summary>
        /// Удаляет заказ из БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="uuidOrderId">уникальный идентификатор зааза</param>
        /// <param name="bRemoveOnlyDeclaration">true - удалить только приложение</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean RemoveFromDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid uuidOrderId, System.Boolean bRemoveOnlyDeclaration, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr = "Не удалось получить соединение с базой данных.";
                        return bRet;
                    }
                    DBTransaction = DBConnection.BeginTransaction();
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteOrder]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Order_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@bOnlyDeclaration", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Order_Guid"].Value = uuidOrderId;
                cmd.Parameters["@bOnlyDeclaration"].Value = bRemoveOnlyDeclaration;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes == 0)
                {
                    bRet = true;
                }

                if (cmdSQL == null)
                {
                    if (bRet == true)
                    {
                        // подтверждаем транзакцию
                        DBTransaction.Commit();
                    }
                    else
                    {
                        // откатываем транзакцию
                        DBTransaction.Rollback();
                    }
                    cmd.Dispose();
                    cmd = null;
                }
            }
            catch (System.Exception f)
            {
                if ((cmdSQL == null) && (DBTransaction != null))
                {
                    DBTransaction.Rollback();
                }
                strErr = f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return bRet;
        }
        /// <summary>
        /// Помечает заказ в БД как удаленный
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="uuidOrderId">уникальный идентификатор зааза</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean MakeDeletedDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid uuidOrderId, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr = "Не удалось получить соединение с базой данных.";
                        return bRet;
                    }
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_MakeOrderDeleted]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Order_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Order_Guid"].Value = uuidOrderId;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes == 0)
                {
                    bRet = true;
                }
                else
                {
                    strErr += (System.Convert.ToString(cmd.Parameters["@ERROR_MES"].Value));
                }

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
            }
            catch (System.Exception f)
            {
                strErr = f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return bRet;
        }
        
        /// <summary>
        /// Проверяет заполнение свойств заказа перед сохранением в БД
        /// </summary>
        /// <param name="Order_BeginDate">дата заказа</param>
        /// <param name="Depart_Guid">уи подразделения</param>
        /// <param name="Salesman_Guid">уи торгового представителя</param>
        /// <param name="Customer_Guid">уи клиента</param>
        /// <param name="OrderType_Guid">уи типа заказа</param>
        /// <param name="PaymentType_Guid">уи формы оплаты</param>
        /// <param name="Order_DeliveryDate">дата доставки заказа</param>
        /// <param name="Rtt_Guid">уи РТТ</param>
        /// <param name="Address_Guid">уи адреса доставки</param>
        /// <param name="addedCategories">приложение к заказу</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - все обязательные поля заполнены; false - не все обязазательные поля заполнены</returns>
        public static System.Boolean CheckAllPropertiesForSave(
            System.DateTime Order_BeginDate,
            System.Guid Depart_Guid, System.Guid Salesman_Guid, System.Guid Customer_Guid,
            System.Guid OrderType_Guid, System.Guid PaymentType_Guid, System.DateTime Order_DeliveryDate,
            System.Guid Rtt_Guid, System.Guid Address_Guid,
            System.Data.DataTable addedCategories, ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (Order_BeginDate == System.DateTime.MinValue)
                {
                    strErr = "Укажите, пожалуйста, дату заказа.";
                    return bRet;
                }
                if ((Order_DeliveryDate == System.DateTime.MinValue) || (System.DateTime.Compare(Order_DeliveryDate, Order_BeginDate) < 0))
                {
                    strErr = "Дата доставки не может быть раньше даты заказа.\nПроверьте, пожалуйста, дату доставки.";
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
                if (Customer_Guid == System.Guid.Empty)
                {
                    strErr = "Укажите, пожалуйста, клиента.";
                    return bRet;
                }
                if (OrderType_Guid == System.Guid.Empty)
                {
                    strErr = "Укажите, пожалуйста, тип заказа.";
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
                if ((addedCategories == null) || (addedCategories.Rows.Count == 0))
                {
                    strErr = "Приложение к заказу не содержит записей. Добавьте, пожалуйста, хотя бы одну позицию.";
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
        /// Сохраняет новый заказ в БД
        /// </summary>
        /// <param name="objProfile"></param>
        /// <param name="cmdSQL"></param>
        /// <param name="Order_BeginDate"></param>
        /// <param name="OrderState_Guid"></param>
        /// <param name="Order_MoneyBonus"></param>
        /// <param name="Depart_Guid"></param>
        /// <param name="Salesman_Guid"></param>
        /// <param name="Customer_Guid"></param>
        /// <param name="CustomerChild_Guid"></param>
        /// <param name="OrderType_Guid"></param>
        /// <param name="PaymentType_Guid"></param>
        /// <param name="Order_Description"></param>
        /// <param name="Order_DeliveryDate"></param>
        /// <param name="Rtt_Guid"></param>
        /// <param name="Address_Guid"></param>
        /// <param name="Stock_Guid"></param>
        /// <param name="Parts_Guid"></param>
        /// <param name="Order_Guid"></param>
        /// <param name="strErr"></param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean AddNewOrderToDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.DateTime Order_BeginDate, System.Guid OrderState_Guid, System.Boolean Order_MoneyBonus,
            System.Guid Depart_Guid, System.Guid Salesman_Guid, System.Guid Customer_Guid, System.Guid CustomerChild_Guid,
            System.Guid OrderType_Guid, System.Guid PaymentType_Guid, System.String Order_Description, System.DateTime Order_DeliveryDate,
            System.Guid Rtt_Guid, System.Guid Address_Guid, System.Guid Stock_Guid, System.Guid Parts_Guid,
            System.Data.DataTable addedCategories, System.Boolean bCalcPrices, System.Guid Stmnt_Guid,
            ref System.Guid Order_Guid, ref System.Int32 Order_Id, ref System.String strErr, System.Boolean SetOrderInQueue = false)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            //System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr = "Не удалось получить соединение с базой данных.";
                        return bRet;
                    }
                    //DBTransaction = DBConnection.BeginTransaction();
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    //cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }
                cmd.CommandTimeout = 600;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddOrder]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Order_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Suppl_Id", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Order_BeginDate", System.Data.SqlDbType.Date));
                if (OrderState_Guid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OrderState_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@OrderState_Guid"].Value = CustomerChild_Guid;
                }
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Order_MoneyBonus", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Depart_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Salesman_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                if (CustomerChild_Guid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerChild_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@CustomerChild_Guid"].Value = CustomerChild_Guid;
                }
                if (Stmnt_Guid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stmnt_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@Stmnt_Guid"].Value = Stmnt_Guid;
                }
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OrderType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PaymentType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Order_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Order_DeliveryDate", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Rtt_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Address_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SetOrderInQueue", System.Data.SqlDbType.Bit));

                if (Parts_Guid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@Parts_Guid"].Value = Parts_Guid;
                }
                cmd.Parameters.AddWithValue("@tOrderItms", addedCategories);
                cmd.Parameters["@tOrderItms"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tOrderItms"].TypeName = "dbo.udt_OrderItms";
                if (bCalcPrices == true)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@bCalcPrices", System.Data.SqlDbType.Bit));
                    cmd.Parameters["@bCalcPrices"].Value = bCalcPrices;
                }
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Customer_Guid"].Value = Customer_Guid;
                cmd.Parameters["@Order_BeginDate"].Value = Order_BeginDate;
                cmd.Parameters["@Order_MoneyBonus"].Value = Order_MoneyBonus;
                cmd.Parameters["@Depart_Guid"].Value = Depart_Guid;
                cmd.Parameters["@Salesman_Guid"].Value = Salesman_Guid;
                cmd.Parameters["@OrderType_Guid"].Value = OrderType_Guid;
                cmd.Parameters["@PaymentType_Guid"].Value = PaymentType_Guid;
                cmd.Parameters["@Order_Description"].Value = Order_Description;
                cmd.Parameters["@Order_DeliveryDate"].Value = Order_DeliveryDate;
                cmd.Parameters["@Rtt_Guid"].Value = Rtt_Guid;
                cmd.Parameters["@Address_Guid"].Value = Address_Guid;
                cmd.Parameters["@Stock_Guid"].Value = Stock_Guid;
                cmd.Parameters["@SetOrderInQueue"].Value = SetOrderInQueue;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes == 0)
                {
                    Order_Guid = (System.Guid)cmd.Parameters["@Order_Guid"].Value;
                    Order_Id = (System.Int32)cmd.Parameters["@Suppl_Id"].Value;
                    //bRet = SaveOrderItemListToDB(objProfile, cmd, Order_Guid, addedCategories, ref strErr); ;
                }
                else
                {
                    strErr += (System.Convert.ToString(cmd.Parameters["@ERROR_MES"].Value));
                }
                bRet = (iRes == 0);
                if (cmdSQL == null)
                {
                    //if (bRet == true)
                    //{
                    //    // подтверждаем транзакцию
                    //    DBTransaction.Commit();
                    //}
                    //else
                    //{
                    //    // откатываем транзакцию
                    //    DBTransaction.Rollback();
                    //}
                    cmd.Dispose();
                    cmd = null;
                }

            }
            catch (System.Exception f)
            {
                //if ((cmdSQL == null) && (DBTransaction != null))
                //{
                //    DBTransaction.Rollback();
                //}
                strErr = f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return bRet;
        }

        /// <summary>
        /// Сохраняет изменения в заказе
        /// </summary>
        /// <param name="objProfile"></param>
        /// <param name="cmdSQL"></param>
        /// <param name="Order_BeginDate"></param>
        /// <param name="OrderState_Guid"></param>
        /// <param name="Order_MoneyBonus"></param>
        /// <param name="Depart_Guid"></param>
        /// <param name="Salesman_Guid"></param>
        /// <param name="Customer_Guid"></param>
        /// <param name="CustomerChild_Guid"></param>
        /// <param name="OrderType_Guid"></param>
        /// <param name="PaymentType_Guid"></param>
        /// <param name="Order_Description"></param>
        /// <param name="Order_DeliveryDate"></param>
        /// <param name="Rtt_Guid"></param>
        /// <param name="Address_Guid"></param>
        /// <param name="Stock_Guid"></param>
        /// <param name="Parts_Guid"></param>
        /// <param name="Order_Guid"></param>
        /// <param name="strErr"></param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean EditOrderInDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.DateTime Order_BeginDate, System.Guid OrderState_Guid, System.Boolean Order_MoneyBonus,
            System.Guid Depart_Guid, System.Guid Salesman_Guid, System.Guid Customer_Guid, System.Guid CustomerChild_Guid,
            System.Guid OrderType_Guid, System.Guid PaymentType_Guid, System.String Order_Description, System.DateTime Order_DeliveryDate,
            System.Guid Rtt_Guid, System.Guid Address_Guid, System.Guid Stock_Guid, System.Guid Parts_Guid,
            System.Data.DataTable addedCategories,
            System.Guid Order_Guid, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr = "Не удалось получить соединение с базой данных.";
                        return bRet;
                    }
                    DBTransaction = DBConnection.BeginTransaction();
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                cmd.CommandTimeout = 600;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditOrder]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Order_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Order_BeginDate", System.Data.SqlDbType.Date));
                if (OrderState_Guid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OrderState_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@OrderState_Guid"].Value = CustomerChild_Guid;
                }
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Order_MoneyBonus", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Depart_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Salesman_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                if (CustomerChild_Guid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerChild_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@CustomerChild_Guid"].Value = CustomerChild_Guid;
                }
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OrderType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PaymentType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Order_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Order_DeliveryDate", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Rtt_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Address_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_Guid", System.Data.SqlDbType.UniqueIdentifier));
                if (Parts_Guid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@Parts_Guid"].Value = Parts_Guid;
                }
                cmd.Parameters["@Order_Guid"].Value = Order_Guid;
                cmd.Parameters["@Customer_Guid"].Value = Customer_Guid;
                cmd.Parameters["@Order_BeginDate"].Value = Order_BeginDate;
                cmd.Parameters["@Order_MoneyBonus"].Value = Order_MoneyBonus;
                cmd.Parameters["@Depart_Guid"].Value = Depart_Guid;
                cmd.Parameters["@Salesman_Guid"].Value = Salesman_Guid;
                cmd.Parameters["@OrderType_Guid"].Value = OrderType_Guid;
                cmd.Parameters["@PaymentType_Guid"].Value = PaymentType_Guid;
                cmd.Parameters["@Order_Description"].Value = Order_Description;
                cmd.Parameters["@Order_DeliveryDate"].Value = Order_DeliveryDate;
                cmd.Parameters["@Rtt_Guid"].Value = Rtt_Guid;
                cmd.Parameters["@Address_Guid"].Value = Address_Guid;
                cmd.Parameters["@Stock_Guid"].Value = Stock_Guid;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes == 0)
                {
                    bRet = SaveOrderItemListToDB(objProfile, cmd, Order_Guid, addedCategories, ref strErr);
                }

                if (cmdSQL == null)
                {
                    if (bRet == true)
                    {
                        // подтверждаем транзакцию
                        DBTransaction.Commit();
                    }
                    else
                    {
                        // откатываем транзакцию
                        DBTransaction.Rollback();
                    }
                    cmd.Dispose();
                    cmd = null;
                }
            }
            catch (System.Exception f)
            {
                if ((cmdSQL == null) && (DBTransaction != null))
                {
                    DBTransaction.Rollback();
                }
                strErr = f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return bRet;
        }
        
        /// <summary>
        /// Пересет цен в заказе
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="Order_Guid">УИ заказа</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции;false - ошибка</returns>
        public static System.Boolean RecalcPricesInOrder(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid Order_Guid, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr = "Не удалось получить соединение с базой данных.";
                        return bRet;
                    }
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_RecalcPricesInOrder]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Order_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters["@Order_Guid"].Value = Order_Guid;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes != 0)
                {
                    strErr += (System.Convert.ToString(cmd.Parameters["@ERROR_MES"].Value));
                }
                bRet = (iRes == 0);
                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    cmd = null;
                }

            }
            catch (System.Exception f)
            {
                strErr += f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return bRet;
        }

        /// <summary>
        /// Возвращает информацию о дебиторской задолженности клиента
        /// </summary>
        /// <param name="objProfile"></param>
        /// <param name="cmdSQL"></param>
        /// <param name="Customer_Guid"></param>
        /// <param name="CustomerChild_Guid"></param>
        /// <param name="PaymentType_Guid"></param>
        /// <param name="Company_Guid"></param>
        /// <param name="Order_Guid"></param>
        /// <param name="WAYBILL_TOTALPRICE"></param>
        /// <param name="WAYBILL_CURRENCYTOTALPRICE"></param>
        /// <param name="WAYBILL_MONEYBONUS"></param>
        /// <param name="WAYBILL_SHIPPED_SALDO"></param>
        /// <param name="WAYBILL_SHIPPED_DEBTDAYS"></param>
        /// <param name="WAYBILL_SALDO"></param>
        /// <param name="WAYBILL_DEBTDAYS"></param>
        /// <param name="INITIAL_DEBT"></param>
        /// <param name="INITIAL_DEBTDAYS"></param>
        /// <param name="SUPPL_SALDO"></param>
        /// <param name="EARNING_SALDO"></param>
        /// <param name="CUSTOMER_LIMITPRICE"></param>
        /// <param name="CUSTOMER_LIMITDAYS"></param>
        /// <param name="OVERDRAFT"></param>
        /// <param name="CUSTOMER_DEBTPRICE"></param>
        /// <param name="CUSTOMER_DEBTDAYS"></param>
        /// <param name="SUMM_IS_PASS"></param>
        /// <param name="strErr"></param>
        /// <returns></returns>
        public static System.Boolean GetCustomerDebtInfoFromIB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid Customer_Guid, System.Guid CustomerChild_Guid, System.Guid PaymentType_Guid, System.Guid Company_Guid,
            System.Guid Order_Guid, System.Double WAYBILL_TOTALPRICE, System.Double WAYBILL_CURRENCYTOTALPRICE,
            System.Boolean WAYBILL_MONEYBONUS, ref System.Double WAYBILL_SHIPPED_SALDO, ref System.Int32 WAYBILL_SHIPPED_DEBTDAYS,
            ref System.Double WAYBILL_SALDO, ref System.Int32 WAYBILL_DEBTDAYS, ref System.Double INITIAL_DEBT,
            ref System.Int32 INITIAL_DEBTDAYS, ref System.Double SUPPL_SALDO, ref System.Double EARNING_SALDO,
            ref System.Double CUSTOMER_LIMITPRICE, ref System.Int32 CUSTOMER_LIMITDAYS, ref System.Double OVERDRAFT,
            ref System.Double CUSTOMER_DEBTPRICE, ref System.Int32 CUSTOMER_DEBTDAYS, ref System.Int32 SUMM_IS_PASS,
            ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr = "Не удалось получить соединение с базой данных.";
                        return bRet;
                    }
                    DBTransaction = DBConnection.BeginTransaction();
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                cmd.CommandTimeout = 600;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCustomerDebtInfoFromIB]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                if (Order_Guid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Order_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@Order_Guid"].Value = Order_Guid;
                }
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters["@Customer_Guid"].Value = Customer_Guid;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PaymentType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters["@PaymentType_Guid"].Value = PaymentType_Guid;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters["@Company_Guid"].Value = Company_Guid;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WAYBILL_TOTALPRICE", System.Data.SqlDbType.Money));
                cmd.Parameters["@WAYBILL_TOTALPRICE"].Value = WAYBILL_TOTALPRICE;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WAYBILL_CURRENCYTOTALPRICE", System.Data.SqlDbType.Money));
                cmd.Parameters["@WAYBILL_CURRENCYTOTALPRICE"].Value = WAYBILL_CURRENCYTOTALPRICE;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WAYBILL_MONEYBONUS", System.Data.SqlDbType.Bit));
                cmd.Parameters["@WAYBILL_MONEYBONUS"].Value = WAYBILL_MONEYBONUS;

                if (CustomerChild_Guid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerChild_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@CustomerChild_Guid"].Value = CustomerChild_Guid;
                }
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WAYBILL_SHIPPED_SALDO", System.Data.SqlDbType.Money));
                cmd.Parameters["@WAYBILL_SHIPPED_SALDO"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WAYBILL_SHIPPED_DEBTDAYS", System.Data.SqlDbType.Int));
                cmd.Parameters["@WAYBILL_SHIPPED_DEBTDAYS"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WAYBILL_SALDO", System.Data.SqlDbType.Money));
                cmd.Parameters["@WAYBILL_SALDO"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WAYBILL_DEBTDAYS", System.Data.SqlDbType.Int));
                cmd.Parameters["@WAYBILL_DEBTDAYS"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@INITIAL_DEBT", System.Data.SqlDbType.Money));
                cmd.Parameters["@INITIAL_DEBT"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@INITIAL_DEBTDAYS", System.Data.SqlDbType.Int));
                cmd.Parameters["@INITIAL_DEBTDAYS"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SUPPL_SALDO", System.Data.SqlDbType.Money));
                cmd.Parameters["@SUPPL_SALDO"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EARNING_SALDO", System.Data.SqlDbType.Money));
                cmd.Parameters["@EARNING_SALDO"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CUSTOMER_LIMITPRICE", System.Data.SqlDbType.Money));
                cmd.Parameters["@CUSTOMER_LIMITPRICE"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CUSTOMER_LIMITDAYS", System.Data.SqlDbType.Int));
                cmd.Parameters["@CUSTOMER_LIMITDAYS"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OVERDRAFT", System.Data.SqlDbType.Money));
                cmd.Parameters["@OVERDRAFT"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CUSTOMER_DEBTPRICE", System.Data.SqlDbType.Money));
                cmd.Parameters["@CUSTOMER_DEBTPRICE"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SUMM_IS_PASS", System.Data.SqlDbType.Int));
                cmd.Parameters["@SUMM_IS_PASS"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CUSTOMER_DEBTDAYS", System.Data.SqlDbType.Int));
                cmd.Parameters["@CUSTOMER_DEBTDAYS"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes == 0)
                {
                    WAYBILL_SHIPPED_SALDO = ((cmd.Parameters["@WAYBILL_SHIPPED_SALDO"].Value == DBNull.Value) ? 0 : System.Convert.ToDouble(cmd.Parameters["@WAYBILL_SHIPPED_SALDO"].Value));
                    WAYBILL_SHIPPED_DEBTDAYS = ((cmd.Parameters["@WAYBILL_SHIPPED_DEBTDAYS"].Value == DBNull.Value) ? 0 : System.Convert.ToInt32(cmd.Parameters["@WAYBILL_SHIPPED_DEBTDAYS"].Value));
                    WAYBILL_SALDO = ((cmd.Parameters["@WAYBILL_SALDO"].Value == DBNull.Value) ? 0 : System.Convert.ToDouble(cmd.Parameters["@WAYBILL_SALDO"].Value));
                    WAYBILL_DEBTDAYS = ((cmd.Parameters["@WAYBILL_DEBTDAYS"].Value == DBNull.Value) ? 0 : System.Convert.ToInt32(cmd.Parameters["@WAYBILL_DEBTDAYS"].Value));

                    SUPPL_SALDO = ((cmd.Parameters["@SUPPL_SALDO"].Value == DBNull.Value) ? 0 : System.Convert.ToDouble(cmd.Parameters["@SUPPL_SALDO"].Value));
                    INITIAL_DEBT = ((cmd.Parameters["@INITIAL_DEBT"].Value == DBNull.Value) ? 0 : System.Convert.ToDouble(cmd.Parameters["@INITIAL_DEBT"].Value));
                    EARNING_SALDO = ((cmd.Parameters["@EARNING_SALDO"].Value == DBNull.Value) ? 0 : System.Convert.ToDouble(cmd.Parameters["@EARNING_SALDO"].Value));
                    CUSTOMER_LIMITPRICE = ((cmd.Parameters["@CUSTOMER_LIMITPRICE"].Value == DBNull.Value) ? 0 : System.Convert.ToDouble(cmd.Parameters["@CUSTOMER_LIMITPRICE"].Value));
                    OVERDRAFT = ((cmd.Parameters["@OVERDRAFT"].Value == DBNull.Value) ? 0 : System.Convert.ToDouble(cmd.Parameters["@OVERDRAFT"].Value));
                    CUSTOMER_DEBTPRICE = ((cmd.Parameters["@CUSTOMER_DEBTPRICE"].Value == DBNull.Value) ? 0 : System.Convert.ToDouble(cmd.Parameters["@CUSTOMER_DEBTPRICE"].Value));
                    INITIAL_DEBTDAYS = ((cmd.Parameters["@INITIAL_DEBTDAYS"].Value == DBNull.Value) ? 0 : System.Convert.ToInt32(cmd.Parameters["@INITIAL_DEBTDAYS"].Value));
                    CUSTOMER_LIMITDAYS = ((cmd.Parameters["@CUSTOMER_LIMITDAYS"].Value == DBNull.Value) ? 0 : System.Convert.ToInt32(cmd.Parameters["@CUSTOMER_LIMITDAYS"].Value));
                    CUSTOMER_DEBTDAYS = ((cmd.Parameters["@CUSTOMER_DEBTDAYS"].Value == DBNull.Value) ? 0 : System.Convert.ToInt32(cmd.Parameters["@CUSTOMER_DEBTDAYS"].Value));
                    SUMM_IS_PASS = ((cmd.Parameters["@SUMM_IS_PASS"].Value == DBNull.Value) ? 0 : System.Convert.ToInt32(cmd.Parameters["@SUMM_IS_PASS"].Value));

                    bRet = true;

                }
                else
                {
                    strErr += (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                if (cmdSQL == null)
                {
                    if (bRet == true)
                    {
                        // подтверждаем транзакцию
                        DBTransaction.Commit();
                    }
                    else
                    {
                        // откатываем транзакцию
                        DBTransaction.Rollback();
                    }
                    cmd.Dispose();
                    cmd = null;
                }
            }
            catch (System.Exception f)
            {
                if ((cmdSQL == null) && (DBTransaction != null))
                {
                    DBTransaction.Rollback();
                }
                strErr += f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return bRet;
        }
        
        /// <summary>
        /// Проверяет, включен ли клиент в "чёрный список"
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="Customer_Guid">УИ клиента</param>
        /// <param name="Company_Guid">УИ компании</param>
        /// <param name="bIsCustomerInBL">признак "включен в BL"</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean IsCustomerInBL(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid Customer_Guid, System.Guid Company_Guid, ref System.Boolean bIsCustomerInBL, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr = "Не удалось получить соединение с базой данных.";
                        return bRet;
                    }
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_IsCustomerInBL]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters["@Customer_Guid"].Value = Customer_Guid;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters["@Company_Guid"].Value = Company_Guid;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@bIsCustomerInBlackList", System.Data.SqlDbType.Bit));
                cmd.Parameters["@bIsCustomerInBlackList"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes == 0)
                {
                    bIsCustomerInBL = System.Convert.ToBoolean(cmd.Parameters["@bIsCustomerInBlackList"].Value);
                    bRet = true;

                }
                else
                {
                    strErr += (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
            }
            catch (System.Exception f)
            {
                strErr += f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return bRet;
        }
        
        /// <summary>
        /// Возвращает значение курса ценообразования
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="dtCurrencyDate">дата</param>
        /// <param name="dblCurerncyRate">курс ценообразования</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true- удачное завершение операции; false - ошибка</returns>
        public static System.Boolean GetCurrencyRatePricing(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.DateTime dtCurrencyDate, ref System.Double dblCurerncyRate, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr = "Не удалось получить соединение с базой данных.";
                        return bRet;
                    }
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCurrencyRatePricing]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Date", System.Data.SqlDbType.DateTime));
                cmd.Parameters["@Currency_Date"].Value = dtCurrencyDate;

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CurrencyRatePricing", System.Data.SqlDbType.Float));
                cmd.Parameters["@CurrencyRatePricing"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes == 0)
                {
                    dblCurerncyRate = ((cmd.Parameters["@CurrencyRatePricing"].Value == DBNull.Value) ? 0 : System.Convert.ToDouble(cmd.Parameters["@CurrencyRatePricing"].Value));

                    bRet = true;
                }
                else
                {
                    strErr += (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
            }
            catch (System.Exception f)
            {
                strErr += f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return bRet;
        }

        /// <summary>
        /// Возвращает значения реквизитов для оформления нового заказа
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="In_OrderType_Guid">УИ типа заказа</param>
        /// <param name="In_PaymentType_Guid">УИ формы оплаты</param>
        /// <param name="SetChildDepartNull">признак "сбросить значение дочернего клиента"</param>
        /// <param name="SetDepartValue">признак "установить значение торгового подразделения"</param>
        /// <param name="SetChildDepartValue">признак "установить значение дочернего клиента"</param>
        /// <param name="Depart_Guid">УИ торгового подразделения</param>
        /// <param name="OrderType_Guid">УИ типа заказа</param>
        /// <param name="PaymentType_Guid">УИ формы оплаты</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean GetOrderDefParams( UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid In_OrderType_Guid, System.Guid In_PaymentType_Guid, ref System.Boolean SetChildDepartNull, ref System.Boolean SetDepartValue,
            ref System.Boolean SetChildDepartValue, ref System.Guid Depart_Guid, ref System.Guid OrderType_Guid, ref System.Guid PaymentType_Guid, 
            ref System.String strErr )
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr = "Не удалось получить соединение с базой данных.";
                        return bRet;
                    }
                    DBTransaction = DBConnection.BeginTransaction();
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                cmd.CommandTimeout = 600;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetSupplDefParams]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                if (In_OrderType_Guid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@In_OrderType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@In_OrderType_Guid"].Value = In_OrderType_Guid;
                }
                if (In_PaymentType_Guid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@In_PaymentType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@In_PaymentType_Guid"].Value = In_PaymentType_Guid;
                }

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SetChildDepartNull", System.Data.SqlDbType.Bit) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SetDepartValue", System.Data.SqlDbType.Bit) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SetChildDepartValue", System.Data.SqlDbType.Bit) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Depart_Guid", System.Data.SqlDbType.UniqueIdentifier) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OrderType_Guid", System.Data.SqlDbType.UniqueIdentifier) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PaymentType_Guid", System.Data.SqlDbType.UniqueIdentifier) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes == 0)
                {
                    SetChildDepartNull = ((cmd.Parameters["@SetChildDepartNull"].Value == DBNull.Value) ? false : System.Convert.ToBoolean(cmd.Parameters["@SetChildDepartNull"].Value));
                    SetDepartValue = ((cmd.Parameters["@SetDepartValue"].Value == DBNull.Value) ? false : System.Convert.ToBoolean(cmd.Parameters["@SetDepartValue"].Value));
                    SetChildDepartValue = ((cmd.Parameters["@SetChildDepartValue"].Value == DBNull.Value) ? false : System.Convert.ToBoolean(cmd.Parameters["@SetChildDepartValue"].Value));
                    OrderType_Guid = ((cmd.Parameters["@OrderType_Guid"].Value == DBNull.Value) ? System.Guid.Empty : (System.Guid)cmd.Parameters["@OrderType_Guid"].Value);
                    PaymentType_Guid = ((cmd.Parameters["@PaymentType_Guid"].Value == DBNull.Value) ? System.Guid.Empty : (System.Guid)cmd.Parameters["@PaymentType_Guid"].Value);
                    Depart_Guid = ((cmd.Parameters["@Depart_Guid"].Value == DBNull.Value) ? System.Guid.Empty : (System.Guid)cmd.Parameters["@Depart_Guid"].Value);

                    bRet = true;

                }
                else
                {
                    strErr += (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                if (cmdSQL == null)
                {
                    if (bRet == true)
                    {
                        // подтверждаем транзакцию
                        DBTransaction.Commit();
                    }
                    else
                    {
                        // откатываем транзакцию
                        DBTransaction.Rollback();
                    }
                    cmd.Dispose();
                    cmd = null;
                }
            }
            catch (System.Exception f)
            {
                if ((cmdSQL == null) && (DBTransaction != null))
                {
                    DBTransaction.Rollback();
                }
                strErr += f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return bRet;
        }
        /// <summary>
        /// Возвращает признак того, можно ли редактировать заказ
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="Suppl_Guid">УИ заказа</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - редактировать можно; false - редактировать нельзя</returns>
        public static System.Boolean IsPossibleEditOrder(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid Suppl_Guid, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr = "Не удалось получить соединение с базой данных.";
                        return bRet;
                    }
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                cmd.CommandTimeout = 600;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_CheckPossibleEditSuppl]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Suppl_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters["@Suppl_Guid"].Value = Suppl_Guid;

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IsPossible", System.Data.SqlDbType.Bit) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes == 0)
                {
                    bRet = ((cmd.Parameters["@IsPossible"].Value == DBNull.Value) ? false : System.Convert.ToBoolean(cmd.Parameters["@IsPossible"].Value));
                }
                else
                {
                    strErr += (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    cmd = null;
                }
            }
            catch (System.Exception f)
            {
                strErr += f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return bRet;
        }

        #endregion

        #region Приложение к заказу
        /// <summary>
        /// Возвращает приложение к заказу
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="uuidOrderId">уникальный идентификатор заказа</param>
        /// <returns>список строк из заказа</returns>
        public static List<COrderItem> GetOrderItemList(UniXP.Common.CProfile objProfile, System.Guid uuidOrderId)
        {
            List<COrderItem> objList = new List<COrderItem>();
            // подключаемся к БД
            System.Data.SqlClient.SqlConnection DBConnection = objProfile.GetDBSource();
            if (DBConnection == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Отсутствует соединение с БД.", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return objList;
            }

            try
            {
                // соединение с БД получено, прописываем команду на выборку данных
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetOrderItms]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Order_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Order_Guid"].Value = uuidOrderId;

                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    COrderItem objOrderItem = null;
                    while (rs.Read())
                    {
                        objOrderItem = new COrderItem();

                        objOrderItem.ID = (System.Guid)rs["Order_Guid"];
                        objOrderItem.Product = new CProduct();
                        objOrderItem.Product.ID = (System.Guid)rs["Parts_Guid"];
                        objOrderItem.Product.ID_Ib = System.Convert.ToInt32(rs["Parts_Id"]);
                        objOrderItem.Product.Name = (System.String)rs["Parts_Name"];
                        objOrderItem.Product.OriginalName = (System.String)rs["Parts_OriginalName"];
                        objOrderItem.Product.ShortName = (System.String)rs["Parts_ShortName"];
                        objOrderItem.Product.Article = (System.String)rs["Parts_Article"];
                        objOrderItem.Product.ProductTradeMark = new CProductTradeMark() { ID = (System.Guid)rs["Owner_Guid"], ID_Ib = System.Convert.ToInt32(rs["Owner_Id"]), Name = (System.String)rs["Owner_Name"], ShortName = (System.String)rs["Owner_ShortName"], ProductVtm = new CProductVtm() { ID = (System.Guid)rs["Vtm_Guid"], ID_Ib = System.Convert.ToInt32(rs["Vtm_Id"]), Name = (System.String)rs["Vtm_Name"] } };
                        objOrderItem.Product.ProductType = new CProductType() { ID = (System.Guid)rs["Parttype_Guid"], ID_Ib = System.Convert.ToInt32(rs["Parttype_Id"]), Name = (System.String)rs["Parttype_Name"] };
                        objOrderItem.Product.ProductSubType = new CProductSubType() { ID = (System.Guid)rs["Partsubtype_Guid"], ID_Ib = System.Convert.ToInt32(rs["Partsubtype_Id"]), Name = (System.String)rs["Partsubtype_Name"], ProductLine = new CProductLine() { ID = (System.Guid)rs["Partline_Guid"], ID_Ib = System.Convert.ToInt32(rs["Partline_Id"]), Name = (System.String)rs["Partline_Name"] } };
                        objOrderItem.Measure = new CMeasure((System.Guid)rs["Measure_Guid"], (System.String)rs["Measure_Name"], (System.String)rs["Measure_ShortName"]);
                        objOrderItem.QuantityOrdered = System.Convert.ToDouble(rs["OrderItms_QuantityOrdered"]);
                        objOrderItem.QuantityReserved = System.Convert.ToDouble(rs["OrderItms_Quantity"]);
                        objOrderItem.NDSPercent = ((rs["OrderItms_NDSPercent"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["OrderItms_NDSPercent"]));
                        objOrderItem.DiscountPercent =  ((rs["OrderItms_DiscountPercent"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["OrderItms_DiscountPercent"]));
                        objOrderItem.PriceImporter =  ((rs["OrderItms_PriceImporter"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["OrderItms_PriceImporter"]));
                        objOrderItem.Price = ((rs["OrderItms_Price"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["OrderItms_Price"]));
                        objOrderItem.PriceWithDiscount =  ((rs["OrderItms_PriceWithDiscount"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["OrderItms_PriceWithDiscount"]));
                        objOrderItem.PriceInAccountingCurrency = ((rs["OrderItms_PriceInAccountingCurrency"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["OrderItms_PriceInAccountingCurrency"]));
                        objOrderItem.PriceWithDiscountInAccountingCurrency =  ((rs["OrderItms_PriceWithDiscountInAccountingCurrency"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["OrderItms_PriceWithDiscountInAccountingCurrency"]));
                        
                        objList.Add(objOrderItem);
                    }
                }
                rs.Close();
                rs.Dispose();
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить приложение к заказу.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
    			finally // очищаем занимаемые ресурсы
            {
                DBConnection.Close();
            }
            return objList;
        }
        /// <summary>
        /// Сохраняет в БД содержимое заказа
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="uuidOrderId">уникальный идентификатор заказа</param>
        /// <param name="addedCategories">тиаблица с приложением к заказу</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean SaveOrderItemListToDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid uuidOrderId, System.Data.DataTable addedCategories, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (addedCategories == null) { return bRet; }

                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr = "Не удалось получить соединение с базой данных.";
                        return bRet;
                    }
                    DBTransaction = DBConnection.BeginTransaction();
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                cmd.Parameters.Clear();
                cmd.CommandTimeout = 600;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddOrderItms]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Order_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.AddWithValue("@tOrderItms", addedCategories);
                cmd.Parameters["@tOrderItms"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tOrderItms"].TypeName = "dbo.udt_OrderItms";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Order_Guid"].Value = uuidOrderId;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                if (cmdSQL == null)
                {
                    if (iRes == 0)
                    {
                        // подтверждаем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Commit();
                        }
                    }
                    else
                    {
                        // откатываем транзакцию
                        if (DBTransaction != null)
                        {
                            DBTransaction.Rollback();
                        }
                    }
                    DBConnection.Close();
                }
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                if ((cmdSQL == null) && (DBTransaction != null))
                {
                    DBTransaction.Rollback();
                }
                strErr = f.Message;
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return bRet;
        }

        /// <summary>
        /// Возвращает остаток товара на складе
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="uuidStockId">уи склада</param>
        /// <param name="iMovementTypeId">тип движения (0 - заказа на отгрузку, 1 - внутреннее перемещение)</param>
        /// <returns>список товаров</returns>
        public static List<CProduct> GetPartsInstockList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid uuidStockId, System.Guid uuidOrderId, System.Boolean bForWaybill = false, 
            System.Boolean bWaybillCreateFromSuppl = false, System.Int32 iMovementTypeId = 0 )
        {
            List<CProduct> objList = new List<CProduct>();
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Int32 iPartsId = 0;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                            "Не удалось получить соединение с базой данных.", "Внимание",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return objList;
                    }
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                if (bForWaybill == true)
                {
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartInstockForWaybill]", objProfile.GetOptionsDllDBName());
                }
                else
                {
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartInstock]", objProfile.GetOptionsDllDBName());
                }
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters["@Stock_Guid"].Value = uuidStockId;
                if (bForWaybill == true)
                {
                    if (uuidOrderId != System.Guid.Empty)
                    {
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_Guid", System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters["@Waybill_Guid"].Value = uuidOrderId;
                    }
                }
                else
                {
                    if (uuidOrderId != System.Guid.Empty)
                    {
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Order_Guid", System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters["@Order_Guid"].Value = uuidOrderId;
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@MovementTypeId", System.Data.SqlDbType.Int));
                        cmd.Parameters["@MovementTypeId"].Value = iMovementTypeId;
                    }
                }

                if ((bWaybillCreateFromSuppl == true) && (uuidOrderId != System.Guid.Empty ))
                {
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartInstockForWaybillFromSuppl]", objProfile.GetOptionsDllDBName());
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Suppl_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@Suppl_Guid"].Value = uuidOrderId;
                }

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CCurrency objCurrency = null;
                    CCountry objCountry = null;
                    CProductTradeMark objProductTradeMark = null;
                    CProductType objProductType = null;
                    CProductSubType objProductSubType = null;
                    CMeasure objMeasure = null;
                    CProductCategory objProductCategory = null;

                    while (rs.Read())
                    {
                        objCurrency = null;
                        objCountry = null;
                        objProductTradeMark = null;
                        objProductType = null;
                        objProductSubType = null;
                        objProductCategory = null;
                        iPartsId = System.Convert.ToInt32(rs["Parts_Id"]);

                        // товарная марка
                        if (rs["Owner_Guid"] != System.DBNull.Value)
                        {
                            objProductTradeMark = new CProductTradeMark(
                                  (System.Guid)rs["Owner_Guid"],
                                  (System.String)rs["Owner_Name"], (System.String)rs["Owner_ShortName"],
                                  System.Convert.ToInt32(rs["Owner_Id"]),
                                  ((rs["Owner_ProcessDaysCount"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Owner_ProcessDaysCount"])),
                                  ((rs["Owner_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Owner_Description"]),
                                  System.Convert.ToBoolean(rs["Owner_IsActive"]),
                                  new CProductVtm(
                                      (System.Guid)rs["Vtm_Guid"],
                                      (System.String)rs["Vtm_Name"],
                                      System.Convert.ToInt32(rs["Vtm_Id"]),
                                      (System.String)rs["Vtm_ShortName"],
                                      ((rs["Vtm_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Vtm_Description"]),
                                      System.Convert.ToBoolean(rs["Vtm_IsActive"])
                                      )
                                      );
                        }
                        // товарная группа
                        if (rs["Parttype_Guid"] != System.DBNull.Value)
                        {
                            objProductType = new CProductType(
                                (System.Guid)rs["Parttype_Guid"],
                                (System.String)rs["Parttype_Name"],
                                System.Convert.ToInt32(rs["Parttype_Id"]),
                                (System.String)rs["Parttype_DemandsName"],
                                System.Convert.ToDouble(rs["Parttype_NDSRate"]),
                                ((rs["Parttype_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Parttype_Description"]),
                                System.Convert.ToBoolean(rs["Parttype_IsActive"])
                                );
                        }
                        // товарная подгруппа
                        if (rs["Partsubtype_Guid"] != System.DBNull.Value)
                        {
                            objProductSubType = new CProductSubType(
                                (System.Guid)rs["Partsubtype_Guid"],
                                (System.String)rs["Partsubtype_Name"],
                                System.Convert.ToInt32(rs["Partsubtype_Id"]),
                                ((rs["Partsubtype_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Partsubtype_Description"]),
                                System.Convert.ToBoolean(rs["Partsubtype_IsActive"]),
                                new CProductLine(
                                (System.Guid)rs["Partline_Guid"],
                                (System.String)rs["Partline_Name"],
                                System.Convert.ToInt32(rs["Partline_Id"]),
                                ((rs["Partline_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Partline_Description"]),
                                System.Convert.ToBoolean(rs["Partline_IsActive"])
                                )
                              );
                        }
                        // страна производства
                        if (rs["Country_Guid"] != System.DBNull.Value)
                        {
                            objCountry = new CCountry((System.Guid)rs["Country_Guid"],
                            (System.String)rs["Country_Name"], (System.String)rs["Country_Code"]);
                        }
                        // категория товара
                        if (rs["PartsCategory_Guid"] != System.DBNull.Value)
                        {
                            objProductCategory = new CProductCategory((System.Guid)rs["PartsCategory_Guid"],
                            (System.String)rs["PartsCategory_Name"], System.Convert.ToInt32(rs["PartsCategory_Id"]),
                            ((rs["PartsCategory_Description"] == System.DBNull.Value) ? "" : (System.String)rs["PartsCategory_Description"]),
                            System.Convert.ToBoolean(rs["PartsCategory_IsActive"])
                            );
                        }
                        // валюта
                        if (rs["Currency_Guid"] != System.DBNull.Value)
                        {
                            objCurrency = new CCurrency((System.Guid)rs["Currency_Guid"], "",
                            (System.String)rs["Currency_Abbr"], (System.String)rs["Currency_Code"]);
                        }
                        // единица измерения
                        if (rs["Measure_Guid"] != System.DBNull.Value)
                        {
                            objMeasure = new CMeasure((System.Guid)rs["Measure_Guid"], (System.String)rs["Measure_Name"], (System.String)rs["Measure_ShortName"]);
                        }

                        objList.Add(new CProduct((System.Guid)rs["Parts_Guid"], System.Convert.ToInt32(rs["Parts_Id"]),
                            (System.String)rs["Parts_Name"], (System.String)rs["Parts_OriginalName"],
                            (System.String)rs["Parts_ShortName"], (System.String)rs["Parts_Article"],
                            objProductTradeMark, objProductType, objProductSubType, objCountry, objCurrency,
                            ((rs["Parts_VendorPrice"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["Parts_VendorPrice"])),
                            ((rs["Parts_BoxQuantity"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Parts_BoxQuantity"])),
                            ((rs["Parts_PackQuantity"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Parts_PackQuantity"])),
                            ((rs["Parts_PackQuantityForCalc"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Parts_PackQuantityForCalc"])),
                            ((rs["Parts_Weight"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["Parts_Weight"])),
                            ((rs["Parts_PaperContainerWeight"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["Parts_PaperContainerWeight"])),
                            ((rs["Parts_PlasticContainerWeight"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["Parts_PlasticContainerWeight"])),
                            ((rs["Parts_AlcoholicContentPercent"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["Parts_AlcoholicContentPercent"])),
                            ((rs["Parts_IsActive"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["Parts_IsActive"])),
                            ((rs["Parts_NotValid"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["Parts_NotValid"])),
                            ((rs["Parts_ActualNotValid"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["Parts_ActualNotValid"])),
                            ((rs["Parts_Certificate"] == System.DBNull.Value) ? "" : (System.String)rs["Parts_Certificate"]),
                            ((rs["Parts_CodeTNVD"] == System.DBNull.Value) ? "" : (System.String)rs["Parts_CodeTNVD"]),
                            ((rs["Parts_Reference"] == System.DBNull.Value) ? "" : (System.String)rs["Parts_Reference"]),
                            objMeasure, objProductCategory, System.Convert.ToDouble(rs["Parts0"]), System.Convert.ToBoolean(rs["PartsIsCheck"])
                            )
                            {
                                IsIncludeInStockShip = System.Convert.ToBoolean(rs["IsProductIncludeInStock"]),
                                CustomerOrderStockQty = System.Convert.ToDouble(rs["STOCK_QTY"]),
                                CustomerOrderResQty = System.Convert.ToDouble(rs["STOCK_RESQTY"]),
                                CustomerOrderMinRetailQty = System.Convert.ToDouble(rs["PARTS_MINRETAILQTY"]),
                                CustomerOrderPackQty = System.Convert.ToDouble(rs["PARTS_PACKQTY"])
                            }
                            );

                    }
                }
                rs.Dispose();

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список товаров.\n\nТекст ошибки для товара с кодом: " + iPartsId.ToString() + " - " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }

        /// <summary>
        /// Возвращает остаток товара на складе
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strProductName">наименование товара</param>
        /// <param name="strProductArticle">артикул товараа</param>
        /// <param name="uuidStockId">уи склада</param>
        /// <param name="iPartsInId">уи товара в InterBase</param>
        /// <returns>остаток товаров</returns>
        public static CProduct GetPartsInstock(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.String strProductName, System.String strProductArticle, System.Guid uuidStockId, System.Int32 iPartsInId = 0)
        {
            CProduct objRet = null;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Int32 iPartsId = 0;
            System.Int32 iCheck = 0;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                            "Не удалось получить соединение с базой данных.", "Внимание",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return objRet;
                    }
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                cmd.CommandTimeout = 600;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartInstockByPartName]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters["@Stock_Guid"].Value = uuidStockId;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Name", System.Data.SqlDbType.NVarChar, 200));
                cmd.Parameters["@Parts_Name"].Value = strProductName;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Article", System.Data.SqlDbType.NVarChar, 200));
                cmd.Parameters["@Parts_Article"].Value = strProductArticle;

                if (iPartsInId != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsIn_Id", System.Data.SqlDbType.Int));
                    cmd.Parameters["@PartsIn_Id"].Value = iPartsInId;
                }

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CCurrency objCurrency = null;
                    CCountry objCountry = null;
                    CProductTradeMark objProductTradeMark = null;
                    CProductType objProductType = null;
                    CProductSubType objProductSubType = null;
                    CMeasure objMeasure = null;
                    CProductCategory objProductCategory = null;
                    

                    rs.Read();
                    {
                        objCurrency = null;
                        objCountry = null;
                        objProductTradeMark = null;
                        objProductType = null;
                        objProductSubType = null;
                        objProductCategory = null;

                        if (rs["Parts_Guid"] != System.DBNull.Value)
                        {
                            if (rs["Parts_Id"] != System.DBNull.Value)
                            {
                                iPartsId = System.Convert.ToInt32(rs["Parts_Id"]);
                            }
                            iCheck = 1;
                            // товарная марка
                            if (rs["Owner_Guid"] != System.DBNull.Value)
                            {
                                objProductTradeMark = new CProductTradeMark(
                                      (System.Guid)rs["Owner_Guid"],
                                      (System.String)rs["Owner_Name"], (System.String)rs["Owner_ShortName"],
                                      System.Convert.ToInt32(rs["Owner_Id"]),
                                      ((rs["Owner_ProcessDaysCount"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Owner_ProcessDaysCount"])),
                                      ((rs["Owner_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Owner_Description"]),
                                      System.Convert.ToBoolean(rs["Owner_IsActive"]),
                                      new CProductVtm(
                                          (System.Guid)rs["Vtm_Guid"],
                                          (System.String)rs["Vtm_Name"],
                                          System.Convert.ToInt32(rs["Vtm_Id"]),
                                          (System.String)rs["Vtm_ShortName"],
                                          ((rs["Vtm_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Vtm_Description"]),
                                          System.Convert.ToBoolean(rs["Vtm_IsActive"])
                                          )
                                          );
                            }
                            iCheck = 2;
                            // товарная группа
                            if (rs["Parttype_Guid"] != System.DBNull.Value)
                            {
                                objProductType = new CProductType(
                                    (System.Guid)rs["Parttype_Guid"],
                                    (System.String)rs["Parttype_Name"],
                                    System.Convert.ToInt32(rs["Parttype_Id"]),
                                    (System.String)rs["Parttype_DemandsName"],
                                    System.Convert.ToDouble(rs["Parttype_NDSRate"]),
                                    ((rs["Parttype_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Parttype_Description"]),
                                    System.Convert.ToBoolean(rs["Parttype_IsActive"])
                                    );
                            }
                            iCheck = 3;
                            // товарная подгруппа
                            if (rs["Partsubtype_Guid"] != System.DBNull.Value)
                            {
                                objProductSubType = new CProductSubType(
                                    (System.Guid)rs["Partsubtype_Guid"],
                                    (System.String)rs["Partsubtype_Name"],
                                    System.Convert.ToInt32(rs["Partsubtype_Id"]),
                                    ((rs["Partsubtype_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Partsubtype_Description"]),
                                    System.Convert.ToBoolean(rs["Partsubtype_IsActive"]),
                                    new CProductLine(
                                    (System.Guid)rs["Partline_Guid"],
                                    (System.String)rs["Partline_Name"],
                                    System.Convert.ToInt32(rs["Partline_Id"]),
                                    ((rs["Partline_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Partline_Description"]),
                                    System.Convert.ToBoolean(rs["Partline_IsActive"])
                                    )
                                  );
                            }
                            iCheck = 4;
                            // страна производства
                            if (rs["Country_Guid"] != System.DBNull.Value)
                            {
                                objCountry = new CCountry((System.Guid)rs["Country_Guid"],
                                (System.String)rs["Country_Name"], (System.String)rs["Country_Code"]);
                            }
                            iCheck = 5;
                            // категория товара
                            if (rs["PartsCategory_Guid"] != System.DBNull.Value)
                            {
                                objProductCategory = new CProductCategory((System.Guid)rs["PartsCategory_Guid"],
                                (System.String)rs["PartsCategory_Name"], System.Convert.ToInt32(rs["PartsCategory_Id"]),
                                ((rs["PartsCategory_Description"] == System.DBNull.Value) ? "" : (System.String)rs["PartsCategory_Description"]),
                                System.Convert.ToBoolean(rs["PartsCategory_IsActive"])
                                );
                            }
                            iCheck = 6;
                            // валюта
                            if (rs["Currency_Guid"] != System.DBNull.Value)
                            {
                                objCurrency = new CCurrency((System.Guid)rs["Currency_Guid"], "",
                                (System.String)rs["Currency_Abbr"], (System.String)rs["Currency_Code"]);
                            }
                            iCheck = 7;
                            // единица измерения
                            if (rs["Measure_Guid"] != System.DBNull.Value)
                            {
                                objMeasure = new CMeasure((System.Guid)rs["Measure_Guid"], (System.String)rs["Measure_Name"], (System.String)rs["Measure_ShortName"]);
                            }
                            iCheck = 8;

                            objRet = new CProduct(
                                ( (rs["Parts_Guid"] == System.DBNull.Value) ? System.Guid.Empty : (System.Guid)rs["Parts_Guid"] ), 
                                ( (rs["Parts_Id"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Parts_Id"]) ),
                                ( (rs["Parts_Name"] == System.DBNull.Value) ? "" : (System.String)rs["Parts_Name"] ), 
                                ( (rs["Parts_OriginalName"] == System.DBNull.Value) ? "" : (System.String)rs["Parts_OriginalName"] ),
                                ((rs["Parts_ShortName"] == System.DBNull.Value) ? "" : (System.String)rs["Parts_ShortName"]),
                                ((rs["Parts_Article"] == System.DBNull.Value) ? "" : (System.String)rs["Parts_Article"]),
                                objProductTradeMark, objProductType, objProductSubType, objCountry, objCurrency,
                                ((rs["Parts_VendorPrice"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["Parts_VendorPrice"])),
                                ((rs["Parts_BoxQuantity"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Parts_BoxQuantity"])),
                                ((rs["Parts_PackQuantity"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Parts_PackQuantity"])),
                                ((rs["Parts_PackQuantityForCalc"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Parts_PackQuantityForCalc"])),
                                ((rs["Parts_Weight"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["Parts_Weight"])),
                                ((rs["Parts_PaperContainerWeight"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["Parts_PaperContainerWeight"])),
                                ((rs["Parts_PlasticContainerWeight"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["Parts_PlasticContainerWeight"])),
                                ((rs["Parts_AlcoholicContentPercent"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["Parts_AlcoholicContentPercent"])),
                                ((rs["Parts_IsActive"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["Parts_IsActive"])),
                                ((rs["Parts_NotValid"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["Parts_NotValid"])),
                                ((rs["Parts_ActualNotValid"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["Parts_ActualNotValid"])),
                                ((rs["Parts_Certificate"] == System.DBNull.Value) ? "" : (System.String)rs["Parts_Certificate"]),
                                ((rs["Parts_CodeTNVD"] == System.DBNull.Value) ? "" : (System.String)rs["Parts_CodeTNVD"]),
                                ((rs["Parts_Reference"] == System.DBNull.Value) ? "" : (System.String)rs["Parts_Reference"]),
                                objMeasure, objProductCategory, System.Convert.ToDouble(rs["Parts0"]), System.Convert.ToBoolean(rs["PartsIsCheck"])
                                )
                            {
                                IsIncludeInStockShip = ((rs["IsProductIncludeInStock"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["IsProductIncludeInStock"])),
                                CustomerOrderStockQty = ((rs["STOCK_QTY"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["STOCK_QTY"])),
                                CustomerOrderResQty = ((rs["STOCK_RESQTY"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["STOCK_RESQTY"])),
                                CustomerOrderMinRetailQty = ((rs["PARTS_MINRETAILQTY"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["PARTS_MINRETAILQTY"])),
                                CustomerOrderPackQty = ((rs["PARTS_PACKQTY"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["PARTS_PACKQTY"]))
                            };

                        }
                    }
                }
                rs.Dispose();

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список товаров.\n\nТекст ошибки: " + f.Message + "\n iCheck = " + iCheck.ToString(), "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objRet;
        }

        /// <summary>
        /// Запрашивает цены
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="uuidPartsId">уи товара</param>
        /// <param name="uuidStockId">уи склада</param>
        /// <param name="uuidPaymentTypeId">уи формы оплаты</param>
        /// <param name="dblDiscountPercent">скидка, %</param>
        /// <param name="PriceImporter">цена импортёра, руб.</param>
        /// <param name="Price">цена отпускная с НДС, руб.</param>
        /// <param name="PriceWithDiscount">цена отпускная с учетом скидки с НДС, руб.</param>
        /// <param name="NDSPercent">ставка НДС, %</param>
        /// <param name="PriceInAccountingCurrency">цена отпускная в валюте учёта</param>
        /// <param name="PriceWithDiscountInAccountingCurrency">цена отпускная с учетом скидки в валюте учёта</param>
        /// <param name="strErr">сообщение об ошибке</param>
        public static System.Boolean GetPriceForOrderItem(UniXP.Common.CProfile objProfile, System.Guid uuidPartsId,
            System.Guid uuidStockId, System.Guid uuidPaymentTypeId,
            System.Double dblDiscountPercent, ref System.Double PriceImporter, ref System.Double Price,
            ref System.Double PriceWithDiscount, ref System.Double NDSPercent,
            ref System.Double PriceInAccountingCurrency, ref System.Double PriceWithDiscountInAccountingCurrency,
            ref System.String strErr, System.Double PriceInput = 0, System.Boolean InputPriceIsFixed = false )
        {
            System.Boolean bRet = false;

            PriceImporter = 0;
            Price = 0;
            PriceWithDiscount = 0;
            NDSPercent = 0;
            PriceInAccountingCurrency = 0;
            PriceWithDiscountInAccountingCurrency = 0;

            // подключаемся к БД
            System.Data.SqlClient.SqlConnection DBConnection = objProfile.GetDBSource();
            if (DBConnection == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Отсутствует соединение с БД.", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return bRet;
            }

            try
            {
                // соединение с БД получено, прописываем команду на выборку данных
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.CommandTimeout = 600;
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPrice]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PaymentType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DiscountPercent", System.Data.SqlDbType.Decimal));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceInput", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@InputPriceIsFixed", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@NDSPercent", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceImporter", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Price", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceWithDiscount", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceInAccountingCurrency", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceWithDiscountInAccountingCurrency", System.Data.SqlDbType.Money));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@NDSPercent"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@PriceImporter"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Price"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@PriceWithDiscount"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@PriceInAccountingCurrency"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@PriceWithDiscountInAccountingCurrency"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Parts_Guid"].Value = uuidPartsId;
                cmd.Parameters["@Stock_Guid"].Value = uuidStockId;
                cmd.Parameters["@PaymentType_Guid"].Value = uuidPaymentTypeId;
                cmd.Parameters["@DiscountPercent"].Value = dblDiscountPercent;
                cmd.Parameters["@PriceInput"].Value = PriceInput;
                cmd.Parameters["@InputPriceIsFixed"].Value = InputPriceIsFixed;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    PriceImporter = System.Convert.ToDouble(cmd.Parameters["@PriceImporter"].Value);
                    Price = System.Convert.ToDouble(cmd.Parameters["@Price"].Value);
                    PriceWithDiscount = System.Convert.ToDouble(cmd.Parameters["@PriceWithDiscount"].Value);
                    NDSPercent = System.Convert.ToDouble(cmd.Parameters["@NDSPercent"].Value);
                    PriceInAccountingCurrency = System.Convert.ToDouble(cmd.Parameters["@PriceInAccountingCurrency"].Value);
                    PriceWithDiscountInAccountingCurrency = System.Convert.ToDouble(cmd.Parameters["@PriceWithDiscountInAccountingCurrency"].Value);

                    bRet = true;
                }
                else
                {
                    bRet = false;
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить цены.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
    			finally // очищаем занимаемые ресурсы
            {
                DBConnection.Close();
            }
            return bRet;
        }

        /// <summary>
        /// Возвращает информацию о том, какой режим расчёта цен включён
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <returns>true - автоматический режим; false - ручной режим</returns>
        public static System.Boolean IsAutoCreatePriceMode(UniXP.Common.CProfile objProfile)
        {
            System.Boolean bRet = false;
            // подключаемся к БД
            System.Data.SqlClient.SqlConnection DBConnection = objProfile.GetDBSource();
            if (DBConnection == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Отсутствует соединение с БД.", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return bRet;
            }

            try
            {
                // соединение с БД получено, прописываем команду на выборку данных
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetAutoCreatePriceInfo]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@iAutoCreatePrice", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@ERROR_NUM"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@iAutoCreatePrice"].Direction = System.Data.ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Convert.ToInt32(cmd.Parameters["@ERROR_NUM"].Value));
                if (iRes == 0)
                {
                    bRet = ((System.Convert.ToInt32(cmd.Parameters["@iAutoCreatePrice"].Value) == 0) ? false : true);
                }
                else
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Не удалось определить режим рассчёта цен.\n\nТекст ошибки: " + System.Convert.ToString(cmd.Parameters["@ERROR_MES"].Value), "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось определить режим рассчёта цен.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
    			finally // очищаем занимаемые ресурсы
            {
                DBConnection.Close();
            }
            return bRet;
        }


        #endregion

        #region Бланк заказа
        /// <summary>
        /// Возвращает список товаров на складах отгрузки указанной компании
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="uuidCompanyId">УИ компании</param>
        /// <returns>список товаров</returns>
        public static List<OrderBlankProductInfo> GetOrderBlankProductInfo(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid uuidCompanyId)
        {
            List<OrderBlankProductInfo> objList = new List<OrderBlankProductInfo>();
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                            "Не удалось получить соединение с базой данных.", "Внимание",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return objList;
                    }
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartInstock2]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters["@Company_Guid"].Value = uuidCompanyId;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new OrderBlankProductInfo()
                        {
                            ProductArticle = System.Convert.ToString(rs["Parts_Article"]),
                            ProductName = System.Convert.ToString(rs["Parts_Name"]),
                            ProductStockQty = System.Convert.ToDouble(rs["STOCK_QTY"]),
                            ProductPackQty = System.Convert.ToDouble(rs["PARTS_PACKQTY"]),
                            ProductPrice = System.Convert.ToDouble(rs["Price2"])
                        });
                    }
                }
                rs.Dispose();

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список товаров.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Возвращает список РТТ
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список РТТ</returns>
        public static List<OrderBlankCustomerInfo> GetOrderBlankCustomerInfo(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<OrderBlankCustomerInfo> objList = new List<OrderBlankCustomerInfo>();
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                            "Не удалось получить соединение с базой данных.", "Внимание",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return objList;
                    }
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetRttList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new OrderBlankCustomerInfo()
                        {
                            CustomerId = System.Convert.ToInt32(rs["Customer_Id"]),
                            CustomerName = System.Convert.ToString(rs["Customer_Name"]),
                            RttCode = System.Convert.ToString(rs["Rtt_Code"]),
                            RttAddress = System.Convert.ToString(rs["RttAddress"])
                        });
                    }
                }
                rs.Dispose();

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список РТТ.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        #endregion
    }
}
