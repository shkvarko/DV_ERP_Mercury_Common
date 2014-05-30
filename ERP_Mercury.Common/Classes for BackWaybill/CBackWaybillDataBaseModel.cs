using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP_Mercury.Common
{
    public class CBackWaybillDataBaseModel
    {

        #region Журнал накладных

        /// <summary>
        /// Возвращает таблицу с накладными за указанный период
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
        /// <returns>таблицу</returns>
        public static System.Data.DataTable GetBackWaybillTable(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid BackWaybill_Guid, System.Guid Waybill_Guid,
            System.DateTime Waybill_DateBegin, System.DateTime Waybill_DateEnd,
            System.Guid Waybill_CompanyGuid, System.Guid Waybill_StockGuid,
            System.Guid Waybill_PaymentTypeGuid, System.Guid Waybill_CustomerGuid,
            ref System.String strErr, System.Boolean SelectWaybillInfoFromWaybill = false)
        {
            System.Data.DataTable dtReturn = new System.Data.DataTable();

            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybill_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybill_Id", typeof(System.Int32)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Stock_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Stock_Name", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Stock_Id", typeof(System.Int32)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Stock_IsActive", typeof(System.Boolean)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Stock_IsTrade", typeof(System.Boolean)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Warehouse_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WarehouseType_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Company_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Company_Id", typeof(System.Int32)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Company_Acronym", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Company_Name", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Currency_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Currency_Abbr", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Depart_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Depart_Code", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Customer_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Customer_Id", typeof(System.Int32)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Customer_Name", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("CustomerChild_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("CustomerStateType_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("CustomerStateType_ShortName", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("ChildDepart_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("ChildDepart_Code", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("ChildDepart_Name", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybillShipMode_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybillShipMode_Id", typeof(System.Int32)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybillShipMode_Name", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("PaymentType_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("PaymentType_Name", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybill_Num", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybill_BeginDate", typeof(System.DateTime)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybillParent_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybillState_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybillState_Id", typeof(System.Int32)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybillState_Name", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybillBackReason_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybillBackReason_Id", typeof(System.Int32)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybillBackReason_Name", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybill_ShipDate", typeof(System.DateTime)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybill_Description", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybill_CurrencyRate", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybill_AllPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybill_AllDiscount", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybill_TotalPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybill_CurrencyAllPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybill_CurrencyAllDiscount", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybill_CurrencyTotalPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybill_Quantity", typeof(System.Double)));

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;

            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr += ("Не удалось получить соединение с базой данных.");
                        return dtReturn;
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

                if ((Waybill_Guid.CompareTo(System.Guid.Empty) == 0) && (BackWaybill_Guid.CompareTo(System.Guid.Empty) == 0))
                {
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetBackWaybillList]", objProfile.GetOptionsDllDBName());
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_DateBegin", System.Data.DbType.Date));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_DateEnd", System.Data.DbType.Date));
                    cmd.Parameters["@Waybill_DateBegin"].Value = Waybill_DateBegin;
                    cmd.Parameters["@Waybill_DateEnd"].Value = Waybill_DateEnd;

                    if (Waybill_CompanyGuid.CompareTo(System.Guid.Empty) != 0)
                    {
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_CompanyGuid", System.Data.DbType.Guid));
                        cmd.Parameters["@Waybill_CompanyGuid"].Value = Waybill_CompanyGuid;
                    }
                    if (Waybill_StockGuid.CompareTo(System.Guid.Empty) != 0)
                    {
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_StockGuid", System.Data.DbType.Guid));
                        cmd.Parameters["@Waybill_StockGuid"].Value = Waybill_StockGuid;
                    }
                    if (Waybill_PaymentTypeGuid.CompareTo(System.Guid.Empty) != 0)
                    {
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_PaymentTypeGuid", System.Data.DbType.Guid));
                        cmd.Parameters["@Waybill_PaymentTypeGuid"].Value = Waybill_PaymentTypeGuid;
                    }
                    if (Waybill_CustomerGuid.CompareTo(System.Guid.Empty) != 0)
                    {
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_CustomerGuid", System.Data.DbType.Guid));
                        cmd.Parameters["@Waybill_CustomerGuid"].Value = Waybill_CustomerGuid;
                    }
                }
                else
                {
                    if (SelectWaybillInfoFromWaybill == true)
                    {
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_Guid", System.Data.DbType.Guid));
                        cmd.Parameters["@Waybill_Guid"].Value = Waybill_Guid;
                    }
                    else
                    {
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BackWaybill_Guid", System.Data.DbType.Guid));
                        cmd.Parameters["@BackWaybill_Guid"].Value = BackWaybill_Guid;
                    }
                }
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });


                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                System.Int32 iRecordCount = 0;
                if (rs.HasRows)
                {
                    System.Data.DataRow newRow = null;
                    while (rs.Read())
                    {
                        iRecordCount++;

                        newRow = dtReturn.NewRow();

                        newRow["BackWaybill_Guid"] = ((rs["BackWaybill_Guid"] != System.DBNull.Value) ? (System.Guid)rs["BackWaybill_Guid"] : System.Guid.Empty);
                        newRow["BackWaybillParent_Guid"] = ((rs["BackWaybillParent_Guid"] != System.DBNull.Value) ? (System.Guid)rs["BackWaybillParent_Guid"] : System.Guid.Empty);
                        newRow["Waybill_Guid"] = ((rs["Waybill_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Waybill_Guid"] : System.Guid.Empty);
                        newRow["BackWaybill_Id"] = ((rs["BackWaybill_Id"] != System.DBNull.Value) ? (System.Int32)rs["BackWaybill_Id"] : 0);
                        newRow["Stock_Guid"] = ((rs["Stock_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Stock_Guid"] : System.Guid.Empty);
                        newRow["Stock_Id"] = ((rs["Stock_Id"] != System.DBNull.Value) ? (System.Int32)rs["Stock_Id"] : 0);
                        newRow["Stock_Name"] = ((rs["Stock_Name"] != System.DBNull.Value) ? System.Convert.ToString(rs["Stock_Name"]) : System.String.Empty);
                        newRow["Stock_IsActive"] = ((rs["Stock_IsActive"] != System.DBNull.Value) ? System.Convert.ToBoolean(rs["Stock_IsActive"]) : false);
                        newRow["Stock_IsTrade"] = ((rs["Stock_IsTrade"] != System.DBNull.Value) ? System.Convert.ToBoolean(rs["Stock_IsTrade"]) : false);

                        newRow["Depart_Guid"] = ((rs["Depart_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Depart_Guid"] : System.Guid.Empty);
                        newRow["Depart_Code"] = ((rs["Depart_Code"] != System.DBNull.Value) ? System.Convert.ToString(rs["Depart_Code"]) : System.String.Empty);

                        newRow["CustomerStateType_Guid"] = ((rs["CustomerStateType_Guid"] != System.DBNull.Value) ? (System.Guid)rs["CustomerStateType_Guid"] : System.Guid.Empty);
                        newRow["CustomerStateType_ShortName"] = ((rs["CustomerStateType_ShortName"] != System.DBNull.Value) ? System.Convert.ToString(rs["CustomerStateType_ShortName"]) : System.String.Empty);

                        newRow["Warehouse_Guid"] = ((rs["Warehouse_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Warehouse_Guid"] : System.Guid.Empty);
                        newRow["WarehouseType_Guid"] = ((rs["WarehouseType_Guid"] != System.DBNull.Value) ? (System.Guid)rs["WarehouseType_Guid"] : System.Guid.Empty);

                        newRow["Company_Guid"] = ((rs["Company_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Company_Guid"] : System.Guid.Empty);
                        newRow["Company_Id"] = ((rs["Company_Id"] != System.DBNull.Value) ? (System.Int32)rs["Company_Id"] : 0);
                        newRow["Company_Acronym"] = ((rs["Company_Acronym"] != System.DBNull.Value) ? System.Convert.ToString(rs["Company_Acronym"]) : System.String.Empty);
                        newRow["Company_Name"] = ((rs["Company_Name"] != System.DBNull.Value) ? System.Convert.ToString(rs["Company_Name"]) : System.String.Empty);

                        newRow["Customer_Guid"] = ((rs["Customer_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Customer_Guid"] : System.Guid.Empty);
                        newRow["Customer_Id"] = ((rs["Customer_Id"] != System.DBNull.Value) ? (System.Int32)rs["Customer_Id"] : 0);
                        newRow["Customer_Name"] = ((rs["Customer_Name"] != System.DBNull.Value) ? System.Convert.ToString(rs["Customer_Name"]) : System.String.Empty);

                        newRow["Currency_Guid"] = ((rs["Currency_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Currency_Guid"] : System.Guid.Empty);
                        newRow["Currency_Abbr"] = ((rs["Currency_Abbr"] != System.DBNull.Value) ? System.Convert.ToString(rs["Currency_Abbr"]) : System.String.Empty);

                        newRow["ChildDepart_Guid"] = ((rs["ChildDepart_Guid"] != System.DBNull.Value) ? (System.Guid)rs["ChildDepart_Guid"] : System.Guid.Empty);
                        newRow["CustomerChild_Guid"] = ((rs["CustomerChild_Guid"] != System.DBNull.Value) ? (System.Guid)rs["CustomerChild_Guid"] : System.Guid.Empty);
                        newRow["ChildDepart_Code"] = ((rs["ChildDepart_Code"] != System.DBNull.Value) ? System.Convert.ToString(rs["ChildDepart_Code"]) : System.String.Empty);
                        newRow["ChildDepart_Name"] = ((rs["ChildDepart_Name"] != System.DBNull.Value) ? System.Convert.ToString(rs["ChildDepart_Name"]) : System.String.Empty);

                        newRow["PaymentType_Guid"] = ((rs["PaymentType_Guid"] != System.DBNull.Value) ? (System.Guid)rs["PaymentType_Guid"] : System.Guid.Empty);
                        newRow["PaymentType_Name"] = ((rs["PaymentType_Name"] != System.DBNull.Value) ? System.Convert.ToString(rs["PaymentType_Name"]) : System.String.Empty);

                        newRow["BackWaybill_BeginDate"] = ((rs["BackWaybill_BeginDate"] != System.DBNull.Value) ? rs["BackWaybill_BeginDate"] : System.DBNull.Value);
                        newRow["BackWaybill_ShipDate"] = ((rs["BackWaybill_ShipDate"] != System.DBNull.Value) ? rs["BackWaybill_ShipDate"] : System.DBNull.Value);

                        newRow["BackWaybill_Num"] = rs["BackWaybill_Num"];

                        newRow["BackWaybillState_Guid"] = rs["BackWaybillState_Guid"];
                        newRow["BackWaybillState_Id"] = rs["BackWaybillState_Id"];
                        newRow["BackWaybillState_Name"] = rs["BackWaybillState_Name"];

                        newRow["WaybillShipMode_Guid"] = rs["WaybillShipMode_Guid"];
                        newRow["WaybillShipMode_Id"] = rs["WaybillShipMode_Id"];
                        newRow["WaybillShipMode_Name"] = rs["WaybillShipMode_Name"];

                        newRow["WaybillBackReason_Guid"] = rs["WaybillBackReason_Guid"];
                        newRow["WaybillBackReason_Id"] = rs["WaybillBackReason_Id"];
                        newRow["WaybillBackReason_Name"] = rs["WaybillBackReason_Name"];

                        newRow["BackWaybill_Description"] = rs["BackWaybill_Description"];

                        newRow["BackWaybill_AllPrice"] = rs["BackWaybill_AllPrice"];
                        newRow["BackWaybill_CurrencyRate"] = rs["BackWaybill_CurrencyRate"];

                        newRow["BackWaybill_AllDiscount"] = 0; // rs["BackWaybill_AllDiscount"];
                        newRow["BackWaybill_CurrencyAllPrice"] = rs["BackWaybill_CurrencyAllPrice"];
                        newRow["BackWaybill_CurrencyAllDiscount"] = 0; // rs["BackWaybill_CurrencyAllDiscount"];

                        newRow["BackWaybill_Quantity"] = System.Convert.ToDecimal(rs["BackWaybill_Quantity"]);

                        dtReturn.Rows.Add(newRow);
                    }

                    dtReturn.AcceptChanges();
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
                strErr += (String.Format("\nНе удалось получить таблицу с накладными на возврат товара от клиента.\nТекст ошибки: {0}", f.Message));
            }
            return dtReturn;
        }
        #endregion

        #region Приложение к накладной
        /// <summary>
        /// Возвращает таблицу с приложением к накладной
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">объект "SQL-команда"</param>
        /// <param name="uuidBackWaybillId">уи накладной на возврат товара(отгрузку товара)</param>
        /// <param name="Waybill_DateBegin">начало периода для поиска накладных</param>
        /// <param name="Waybill_DateEnd">конец периода для поиска накладных</param>
        /// <param name="strErr">строка с сообщением об ошибке</param>
        /// <param name="bTableFromWaybilll">признак "приложение запрашивается из накладной"</param>
        /// <returns>приложение к накладной в виде объекта класса DataTable</returns>
        public static System.Data.DataTable GetBackWaybilItemTable(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid uuidBackWaybillId, System.DateTime Waybill_DateBegin, System.DateTime Waybill_DateEnd, 
            ref System.String strErr, System.Boolean bTableFromWaybilll = false)
        {
            System.Data.DataTable dtReturn = new System.Data.DataTable();

            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybItem_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybItem_Id", typeof(System.Int32)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_Id", typeof(System.Int32)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybill_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_Num", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_BeginDate", typeof(System.DateTime)));

            dtReturn.Columns.Add(new System.Data.DataColumn("Parts_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Measure_Guid", typeof(System.Guid)));

            dtReturn.Columns.Add(new System.Data.DataColumn("ProductOwnerName", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("PARTS_NAME", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("PARTS_ARTICLE", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Measure_ShortName", typeof(System.String)));

            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybItem_Quantity", typeof(System.Double)));

            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybItem_Price", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybItem_Discount", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybItem_DiscountPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybItem_AllPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybItem_TotalPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybItem_PriceImporter", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybItem_CurrencyPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybItem_CurrencyDiscountPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybItem_CurrencyAllPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybItem_CurrencyTotalPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("BackWaybItem_NDSPercent", typeof(System.Double)));

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;

            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr += ("Не удалось получить соединение с базой данных.");
                        return dtReturn;
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

                if (bTableFromWaybilll == true)
                {
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetBackWaybItemsFromWaybill]", objProfile.GetOptionsDllDBName());
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                    if (uuidBackWaybillId.CompareTo(System.Guid.Empty) != 0)
                    {
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_Guid", System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters["@Waybill_Guid"].Value = uuidBackWaybillId;
                    }
                    else
                    {
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_DateBegin", System.Data.DbType.Date));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_DateEnd", System.Data.DbType.Date));

                        cmd.Parameters["@Waybill_DateBegin"].Value = Waybill_DateBegin;
                        cmd.Parameters["@Waybill_DateEnd"].Value = Waybill_DateEnd;
                    }

                }
                else
                {
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetBackWaybItems]", objProfile.GetOptionsDllDBName());
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BackWaybill_Guid", System.Data.DbType.Guid));
                    cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                    cmd.Parameters["@BackWaybill_Guid"].Value = uuidBackWaybillId;
                }

                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                System.Int32 iRecordCount = 0;
                if (rs.HasRows)
                {
                    System.Data.DataRow newRow = null;
                    while (rs.Read())
                    {
                        iRecordCount++;

                        newRow = dtReturn.NewRow();

                        newRow["BackWaybItem_Guid"] = ((rs["BackWaybItem_Guid"] != System.DBNull.Value) ? (System.Guid)rs["BackWaybItem_Guid"] : System.Guid.Empty);
                        newRow["WaybItem_Guid"] = ((rs["WaybItem_Guid"] != System.DBNull.Value) ? (System.Guid)rs["WaybItem_Guid"] : System.Guid.Empty);
                        newRow["BackWaybill_Guid"] = ((rs["BackWaybill_Guid"] != System.DBNull.Value) ? (System.Guid)rs["BackWaybill_Guid"] : System.Guid.Empty);
                        newRow["BackWaybItem_Id"] = ((rs["BackWaybItem_Id"] != System.DBNull.Value) ? (System.Int32)rs["BackWaybItem_Id"] : 0);

                        newRow["Waybill_Num"] = ((rs["Waybill_Num"] != System.DBNull.Value) ? System.Convert.ToString(rs["Waybill_Num"]) : System.String.Empty);
                        if (rs["Waybill_BeginDate"] != System.DBNull.Value)
                        {
                            newRow["Waybill_BeginDate"] = rs["Waybill_BeginDate"];
                        }
                        
                        newRow["Measure_Guid"] = ((rs["Measure_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Measure_Guid"] : System.Guid.Empty);
                        newRow["Parts_Guid"] = ((rs["Parts_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Parts_Guid"] : System.Guid.Empty);

                        newRow["ProductOwnerName"] = ((rs["ProductOwnerName"] != System.DBNull.Value) ? System.Convert.ToString(rs["ProductOwnerName"]) : System.String.Empty);
                        newRow["PARTS_NAME"] = ((rs["PARTS_NAME"] != System.DBNull.Value) ? System.Convert.ToString(rs["PARTS_NAME"]) : System.String.Empty);
                        newRow["PARTS_ARTICLE"] = ((rs["PARTS_ARTICLE"] != System.DBNull.Value) ? System.Convert.ToString(rs["PARTS_ARTICLE"]) : System.String.Empty);
                        newRow["Measure_ShortName"] = ((rs["Measure_ShortName"] != System.DBNull.Value) ? System.Convert.ToString(rs["Measure_ShortName"]) : System.String.Empty);

                        newRow["BackWaybItem_Quantity"] = rs["BackWaybItem_Quantity"];

                        newRow["BackWaybItem_Price"] = rs["BackWaybItem_Price"];
                        newRow["BackWaybItem_Discount"] = 0; // rs["BackWaybItem_Discount"];
                        newRow["BackWaybItem_DiscountPrice"] = rs["BackWaybItem_Price"];
                        newRow["BackWaybItem_AllPrice"] = rs["BackWaybItem_AllPrice"];
                        newRow["BackWaybItem_TotalPrice"] = rs["BackWaybItem_AllPrice"];
                        newRow["BackWaybItem_PriceImporter"] = rs["BackWaybItem_PriceImporter"];

                        newRow["BackWaybItem_CurrencyPrice"] = rs["BackWaybItem_CurrencyPrice"];
                        newRow["BackWaybItem_CurrencyDiscountPrice"] = rs["BackWaybItem_CurrencyPrice"];
                        newRow["BackWaybItem_CurrencyAllPrice"] = rs["BackWaybItem_CurrencyAllPrice"];
                        newRow["BackWaybItem_CurrencyTotalPrice"] = rs["BackWaybItem_CurrencyAllPrice"];
                        newRow["BackWaybItem_NDSPercent"] = 0; // rs["WaybItem_NDSPercent"];

                        dtReturn.Rows.Add(newRow);
                    }

                    dtReturn.AcceptChanges();
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
                strErr += (String.Format("\nНе удалось получить таблицу с приложением к накладной на возврат товара.\nТекст ошибки: {0}", f.Message));
            }
            return dtReturn;
        }

        /// <summary>
        /// Возвращает таблицу с приложением к накладной
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">объект "SQL-команда"</param>
        /// <param name="uuidWaybillId">уи накладной</param>
        /// <param name="strErr">строка с сообщением об ошибке</param>
        /// <returns>приложение к накладной в виде объекта класса DataTable</returns>
        public static System.Data.DataTable GetWaybilItemTableForExportToExcel(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidWaybillId, ref System.String strErr)
        {
            System.Data.DataTable dtReturn = new System.Data.DataTable();

            dtReturn.Columns.Add(new System.Data.DataColumn("WAYBITMS_ID", typeof(System.Int32)));
            dtReturn.Columns.Add(new System.Data.DataColumn("PARTS_ID", typeof(System.Int32)));
            dtReturn.Columns.Add(new System.Data.DataColumn("PARTS_FULLNAME", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("OWNER_NAME", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("MEASURE_SHORTNAME", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WAYBITMS_QUANTITY", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WAYBITMS_BASEPRICE", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WAYBITMS_PERCENT", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WAYBITMS_DOUBLEPERCENT", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WAYBITMS_TOTALPRICEWITHOUTNDS", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WAYBITMS_NDS", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WAYBITMS_STRNDS", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WAYBITMS_NDSTOTALPRICE", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WAYBITMS_TOTALPRICE", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WAYBITMS_WEIGHT", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WAYBITMS_PLACES", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WAYBITMS_INTPLACES", typeof(System.Int32)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WAYBITMS_TARA", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WAYBITMS_QTYINPLACE", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("PARTS_CERTIFICATE", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WAYBITMS_NOTES", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WAYBITMS_PRICEWITHOUTNDS", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("COUNTRY_NAME", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Parts_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Parts_Barcode", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Parts_BarcodeList", typeof(System.String)));

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;

            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr += ("Не удалось получить соединение с базой данных.");
                        return dtReturn;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetWaybillProduction4StockFromIB]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_Guid", System.Data.DbType.Guid));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Waybill_Guid"].Value = uuidWaybillId;

                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                System.Int32 iRecordCount = 0;
                if (rs.HasRows)
                {
                    System.Data.DataRow newRow = null;
                    while (rs.Read())
                    {
                        iRecordCount++;

                        newRow = dtReturn.NewRow();

                        newRow["WAYBITMS_ID"] = rs["WAYBITMS_ID"];
                        newRow["PARTS_ID"] = rs["PARTS_ID"];
                        newRow["PARTS_FULLNAME"] = rs["PARTS_FULLNAME"];
                        newRow["OWNER_NAME"] = rs["OWNER_NAME"];
                        newRow["MEASURE_SHORTNAME"] = rs["MEASURE_SHORTNAME"];
                        newRow["WAYBITMS_QUANTITY"] = rs["WAYBITMS_QUANTITY"];
                        newRow["WAYBITMS_BASEPRICE"] = rs["WAYBITMS_BASEPRICE"];
                        newRow["WAYBITMS_PERCENT"] = rs["WAYBITMS_PERCENT"];
                        newRow["WAYBITMS_DOUBLEPERCENT"] = rs["WAYBITMS_DOUBLEPERCENT"];
                        newRow["WAYBITMS_TOTALPRICEWITHOUTNDS"] = rs["WAYBITMS_TOTALPRICEWITHOUTNDS"];
                        newRow["WAYBITMS_NDS"] = rs["WAYBITMS_NDS"];
                        newRow["WAYBITMS_STRNDS"] = rs["WAYBITMS_STRNDS"];
                        newRow["WAYBITMS_NDSTOTALPRICE"] = rs["WAYBITMS_NDSTOTALPRICE"];
                        newRow["WAYBITMS_TOTALPRICE"] = rs["WAYBITMS_TOTALPRICE"];
                        newRow["WAYBITMS_WEIGHT"] = rs["WAYBITMS_WEIGHT"];
                        newRow["WAYBITMS_PLACES"] = rs["WAYBITMS_PLACES"];
                        newRow["WAYBITMS_INTPLACES"] = rs["WAYBITMS_INTPLACES"];
                        newRow["WAYBITMS_TARA"] = rs["WAYBITMS_TARA"];
                        newRow["WAYBITMS_QTYINPLACE"] = rs["WAYBITMS_QTYINPLACE"];
                        newRow["PARTS_CERTIFICATE"] = rs["PARTS_CERTIFICATE"];
                        newRow["WAYBITMS_NOTES"] = rs["WAYBITMS_NOTES"];
                        newRow["WAYBITMS_PRICEWITHOUTNDS"] = rs["WAYBITMS_PRICEWITHOUTNDS"];
                        newRow["COUNTRY_NAME"] = rs["COUNTRY_NAME"];
                        newRow["Parts_Guid"] = rs["Parts_Guid"];
                        newRow["Parts_Barcode"] = rs["Parts_Barcode"];
                        newRow["Parts_BarcodeList"] = rs["Parts_BarcodeList"];

                        dtReturn.Rows.Add(newRow);
                    }

                    dtReturn.AcceptChanges();
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
                strErr += (String.Format("\nНе удалось получить таблицу с приложением к накладной.\nТекст ошибки: {0}", f.Message));
            }
            return dtReturn;
        }

        #endregion

        #region Проверка, можно ли оформить возврат по накладной
        /// <summary>
        /// Проверка, можно ли оформить возврат по накладной
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="Waybill_Guid">УИ накладной</param>
        /// <param name="strErr">текст ошибки</param>
        /// <returns>true - можно; false - нельзя</returns>
        public static System.Boolean CanCreateBackWaybillFromSuppl(UniXP.Common.CProfile objProfile,
            System.Guid Waybill_Guid, ref System.String strErr)
        {
            System.Boolean bRet = false;

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    strErr += ("\nНе удалось получить соединение с базой данных.");
                    return bRet;
                }
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_CanCreateBackWaybillFromWaybill]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CanCreateBackWaybillFromWaybill", System.Data.SqlDbType.Bit) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });

                cmd.Parameters["@Waybill_Guid"].Value = Waybill_Guid;
                cmd.ExecuteNonQuery();

                System.Int32 iRet = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                bRet = (System.Boolean)cmd.Parameters["@CanCreateBackWaybillFromWaybill"].Value;

                cmd.Dispose();
                DBConnection.Close();
            }
            catch (System.Exception f)
            {
                strErr = "CanCreateBackWaybillFromWaybill.\n\nТекст ошибки: " + f.Message;
            }
            return bRet;
        }
        #endregion

        #region Список позиций, доступных к возврату
        /// <summary>
        /// Возвращает таблицу со списком позиций для возврата
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">объект "SQL-команда"</param>
        /// <param name="Waybill_Guid">уи накладной</param>
        /// <param name="Customer_Guid">уи накладной</param>
        /// <param name="Stock_Guid">уи накладной</param>
        /// <param name="PaymentType_Guid">уи накладной</param>
        /// <param name="ShipDate_Begin">уи накладной</param>
        /// <param name="ShipDate_End">уи накладной</param>
        /// <param name="strErr">строка с сообщением об ошибке</param>
        /// <returns>список позиций для возврата в виде объекта класса DataTable</returns>
        public static System.Data.DataTable GetTableSrcForBackWaybillItem(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, 
            System.Guid Waybill_Guid, 
            System.Guid Customer_Guid,  System.Guid Stock_Guid,  System.Guid PaymentType_Guid, System.DateTime ShipDate_Begin, System.DateTime ShipDate_End,
            ref System.String strErr)
        {
            System.Data.DataTable dtReturn = new System.Data.DataTable();

            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_Num", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_BeginDate", typeof(System.DateTime)));

            dtReturn.Columns.Add(new System.Data.DataColumn("Parts_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Measure_Guid", typeof(System.Guid)));

            dtReturn.Columns.Add(new System.Data.DataColumn("PartsOwner_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("PartsPartType_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Parts_Name", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Parts_Article", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Measure_ShortName", typeof(System.String)));

            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_Quantity", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_LeavQuantity", typeof(System.Double)));

            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_NDSPercent", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_PriceImporter", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_Price", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_Discount", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_DiscountPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_CurrencyPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_CurrencyDiscountPrice", typeof(System.Double)));

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;

            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr += ("Не удалось получить соединение с базой данных.");
                        return dtReturn;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetSrcForBackWaybillItems]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });

                if (Waybill_Guid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_Guid", System.Data.DbType.Guid));
                    cmd.Parameters["@Waybill_Guid"].Value = Waybill_Guid;
                }
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PaymentType_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ShipDate_Begin", System.Data.DbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ShipDate_End", System.Data.DbType.Date));

                cmd.Parameters["@Customer_Guid"].Value = Customer_Guid;
                cmd.Parameters["@Stock_Guid"].Value = Stock_Guid;
                cmd.Parameters["@PaymentType_Guid"].Value = PaymentType_Guid;
                cmd.Parameters["@ShipDate_Begin"].Value = ShipDate_Begin;
                cmd.Parameters["@ShipDate_End"].Value = ShipDate_End;

                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                System.Int32 iRecordCount = 0;
                if (rs.HasRows)
                {
                    System.Data.DataRow newRow = null;
                    while (rs.Read())
                    {
                        iRecordCount++;

                        newRow = dtReturn.NewRow();

                        newRow["WaybItem_Guid"] = ((rs["WaybItem_Guid"] != System.DBNull.Value) ? (System.Guid)rs["WaybItem_Guid"] : System.Guid.Empty);
                        newRow["Waybill_Guid"] = ((rs["Waybill_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Waybill_Guid"] : System.Guid.Empty);
                        newRow["Measure_Guid"] = ((rs["Measure_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Measure_Guid"] : System.Guid.Empty);
                        newRow["Parts_Guid"] = ((rs["Parts_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Parts_Guid"] : System.Guid.Empty);

                        newRow["PartsOwner_Guid"] = ((rs["PartsOwner_Guid"] != System.DBNull.Value) ? (System.Guid)rs["PartsOwner_Guid"] : System.Guid.Empty);
                        newRow["PartsPartType_Guid"] = ((rs["PartsPartType_Guid"] != System.DBNull.Value) ? (System.Guid)rs["PartsPartType_Guid"] : System.Guid.Empty);

                        newRow["Parts_Name"] = ((rs["Parts_Name"] != System.DBNull.Value) ? System.Convert.ToString(rs["Parts_Name"]) : System.String.Empty);
                        newRow["Parts_Article"] = ((rs["Parts_Article"] != System.DBNull.Value) ? System.Convert.ToString(rs["Parts_Article"]) : System.String.Empty);
                        newRow["Measure_ShortName"] = ((rs["Measure_ShortName"] != System.DBNull.Value) ? System.Convert.ToString(rs["Measure_ShortName"]) : System.String.Empty);

                        newRow["WaybItem_Quantity"] = rs["WaybItem_Quantity"];
                        newRow["WaybItem_LeavQuantity"] = rs["WaybItem_LeavQuantity"];

                        newRow["WaybItem_NDSPercent"] = rs["WaybItem_NDSPercent"];
                        newRow["WaybItem_PriceImporter"] = rs["WaybItem_PriceImporter"];
                        newRow["WaybItem_Price"] = rs["WaybItem_Price"];
                        newRow["WaybItem_Discount"] = rs["WaybItem_Discount"];
                        newRow["WaybItem_DiscountPrice"] = rs["WaybItem_DiscountPrice"];

                        newRow["WaybItem_CurrencyPrice"] = rs["WaybItem_CurrencyPrice"];
                        newRow["WaybItem_CurrencyDiscountPrice"] = rs["WaybItem_CurrencyDiscountPrice"];

                        newRow["Waybill_Num"] = rs["Waybill_Num"];
                        newRow["Waybill_BeginDate"] = rs["Waybill_BeginDate"];
                        dtReturn.Rows.Add(newRow);
                    }

                    dtReturn.AcceptChanges();
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
                strErr += (String.Format("\nНе удалось получить таблицу со списком позиций для возврата.\nТекст ошибки: {0}", f.Message));
            }
            return dtReturn;
        }


        #endregion

        #region Сохранение накладной в БД
        /// <summary>
        /// Добавляет в БД информацию о накладной
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="Waybill_Guid">УИ накладной на отгрузку</param>
        /// <param name="Stock_Guid">УИ склада отгрузки</param>
        /// <param name="Company_Guid">УИ компании</param>
        /// <param name="Depart_Guid">УИ торгового подразделения</param>
        /// <param name="Customer_Guid">УИ клиента</param>
        /// <param name="CustomerChild_Guid">УИ дочернего клиента</param>
        /// <param name="PaymentType_Guid">УИ формы оплаты</param>
        /// <param name="WaybillBackReason_Guid">УИ причины возврата товара</param>
        /// <param name="BackWaybill_Num">номер возвратной накладной</param>
        /// <param name="BackWaybill_BeginDate">дата возвратной накладной</param>
        /// <param name="BackWaybillParent_Guid">УИ родительской возвратной накладной</param>
        /// <param name="BackWaybillState_Guid">УИ состояния возвратной накладной</param>
        /// <param name="WaybillShipMode_Guid">УИ вида отгрузки</param>
        /// <param name="BackWaybill_ShipDate">дата отгрузки возвратной накладной</param>
        /// <param name="BackWaybill_Description">примечание</param>
        /// <param name="BackWaybill_CurrencyRate">курс ценообразования</param>
        /// <param name="WaybillTablePart">приложение к накладной (товары)</param>
        /// <param name="BackWaybill_Guid">УИ возвратной накладной</param>
        /// <param name="BackWaybill_Id">УИ возвратной накладной в InterBase</param>
        /// <param name="strErr">текст ошибки</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean AddNewWaybillToDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid Waybill_Guid, System.Guid Stock_Guid, System.Guid Company_Guid, System.Guid Depart_Guid,
            System.Guid Customer_Guid, System.Guid CustomerChild_Guid,
            System.Guid PaymentType_Guid, System.Guid WaybillBackReason_Guid, System.String BackWaybill_Num,
            System.DateTime BackWaybill_BeginDate, System.Guid BackWaybillParent_Guid,
            System.Guid BackWaybillState_Guid, System.Guid WaybillShipMode_Guid, System.DateTime BackWaybill_ShipDate,
            System.String BackWaybill_Description, System.Double BackWaybill_CurrencyRate, 
            System.Data.DataTable WaybillTablePart,
            ref System.Guid BackWaybill_Guid, ref System.Int32 BackWaybill_Id, ref System.String strErr )
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddBackWaybill]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BackWaybill_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BackWaybill_Id", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));

                if (BackWaybillState_Guid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BackWaybillState_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@BackWaybillState_Guid"].Value = BackWaybillState_Guid;
                }
                if (BackWaybillParent_Guid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BackWaybillParent_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@BackWaybillParent_Guid"].Value = BackWaybillParent_Guid;
                }
                if (CustomerChild_Guid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerChild_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@CustomerChild_Guid"].Value = CustomerChild_Guid;
                }
                if (System.DateTime.Compare(BackWaybill_ShipDate, System.DateTime.MinValue) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BackWaybill_ShipDate", System.Data.SqlDbType.Date));
                    cmd.Parameters["@BackWaybill_ShipDate"].Value = BackWaybill_ShipDate;
                }
                if (Waybill_Guid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@Waybill_Guid"].Value = Waybill_Guid;
                }

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BackWaybill_BeginDate", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Depart_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillShipMode_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PaymentType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillBackReason_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BackWaybill_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BackWaybill_Num", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BackWaybill_CurrencyRate", System.Data.SqlDbType.Money));

                cmd.Parameters.AddWithValue("@tBackWaybItms", WaybillTablePart);
                cmd.Parameters["@tBackWaybItms"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tBackWaybItms"].TypeName = "dbo.udt_BackWaybItms";

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters["@Waybill_Guid"].Value = Waybill_Guid;
                cmd.Parameters["@BackWaybill_BeginDate"].Value = BackWaybill_BeginDate;
                cmd.Parameters["@Depart_Guid"].Value = Depart_Guid;
                cmd.Parameters["@Customer_Guid"].Value = Customer_Guid;
                cmd.Parameters["@WaybillShipMode_Guid"].Value = WaybillShipMode_Guid;
                cmd.Parameters["@PaymentType_Guid"].Value = PaymentType_Guid;
                cmd.Parameters["@WaybillBackReason_Guid"].Value = WaybillBackReason_Guid;
                cmd.Parameters["@BackWaybill_Description"].Value = BackWaybill_Description;
                cmd.Parameters["@BackWaybill_Num"].Value = BackWaybill_Num;
                cmd.Parameters["@Stock_Guid"].Value = Stock_Guid;
                cmd.Parameters["@Company_Guid"].Value = Company_Guid;
                cmd.Parameters["@BackWaybill_CurrencyRate"].Value = BackWaybill_CurrencyRate;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                strErr += (System.Convert.ToString(cmd.Parameters["@ERROR_MES"].Value));

                if (iRes == 0)
                {
                    BackWaybill_Guid = (System.Guid)cmd.Parameters["@BackWaybill_Guid"].Value;
                    BackWaybill_Id = (System.Int32)cmd.Parameters["@BackWaybill_Id"].Value;
                    
                    strErr = "Накладная успешно сохранена.";
                }
                else
                {
                    strErr = strErr.Replace("\r", "\n");
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
        #endregion


    }
}
