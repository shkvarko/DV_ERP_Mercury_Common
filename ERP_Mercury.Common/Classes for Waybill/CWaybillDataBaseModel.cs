using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ERP_Mercury.Common
{
    public static class CWaybillDataBaseModel
    {
        #region Журнал накладных

        /// <summary>
        /// Возвращает таблицу с накладными за указанный период
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="dtBeginDate">начало периода для выборки</param>
        /// <param name="dtEndDate">конец периода для выборки</param>
        /// <param name="uuidCompanyId">УИ компании</param>
        /// <param name="uuidStockId">УИ склада</param>
        /// <param name="uuidStockId">УИ склада</param>
        /// <param name="uuidPaymentTypeId">УИ формы оплаты</param>
        /// <param name="strErr">текст ошибки</param>
        /// <param name="SelectWaybillInfoFromSuppl">признак "информация для накладной запрашивается из заказа"</param>
        /// <param name="OnlyUnShippedWaybills">признак "только не отгруженные накладные"</param>
        /// <returns>таблицу</returns>
        public static System.Data.DataTable GetWaybillTable(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid Waybill_Guid, System.Data.DataTable dtWaybillIdList,
            System.DateTime dtBeginDate, System.DateTime dtEndDate,
            System.Guid uuidCompanyId, System.Guid uuidStockId,
            System.Guid uuidPaymentTypeId, System.Guid uuidCustomerId,
            ref System.String strErr, System.Boolean SelectWaybillInfoFromSuppl = false, 
            System.Boolean OnlyUnShippedWaybills = false)
        {
            System.Data.DataTable dtReturn = new System.Data.DataTable();

            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_Id", typeof(System.Int32)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Suppl_Guid", typeof(System.Guid)));
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
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybillShipMode_Description", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybillShipMode_IsActive", typeof(System.Boolean)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybillShipMode_IsDefault", typeof(System.Boolean)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Rtt_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Rtt_Name", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Address_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Address_FullName", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("PaymentType_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("PaymentType_Name", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_Num", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_BeginDate", typeof(System.DateTime)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_DeliveryDate", typeof(System.DateTime)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybillParent_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_Bonus", typeof(System.Boolean)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybillState_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybillState_Id", typeof(System.Int32)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybillState_Name", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_ShipDate", typeof(System.DateTime)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_Description", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_CurrencyRate", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_AllPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_RetAllPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_AllDiscount", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_TotalPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_AmountPaid", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_Saldo", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_CurrencyAllPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_CurrencyRetAllPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_CurrencyAllDiscount", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_CurrencyTotalPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_CurrencyAmountPaid", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_CurrencySaldo", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_Quantity", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_RetQuantity", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_LeavQuantity", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_Weight", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_ShowInDeliveryList", typeof(System.Boolean)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_CanShip", typeof(System.Boolean)));

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

                if ((Waybill_Guid.CompareTo(System.Guid.Empty) == 0) || (dtWaybillIdList != null))
                {
                    if (dtWaybillIdList == null)
                    {

                        cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetWaybillList]", objProfile.GetOptionsDllDBName());
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_DateBegin", System.Data.DbType.Date));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_DateEnd", System.Data.DbType.Date));
                        cmd.Parameters["@Waybill_DateBegin"].Value = dtBeginDate;
                        cmd.Parameters["@Waybill_DateEnd"].Value = dtEndDate;

                        if (uuidCompanyId.CompareTo(System.Guid.Empty) != 0)
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_CompanyGuid", System.Data.DbType.Guid));
                            cmd.Parameters["@Waybill_CompanyGuid"].Value = uuidCompanyId;
                        }
                        if (uuidStockId.CompareTo(System.Guid.Empty) != 0)
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_StockGuid", System.Data.DbType.Guid));
                            cmd.Parameters["@Waybill_StockGuid"].Value = uuidStockId;
                        }
                        if (uuidPaymentTypeId.CompareTo(System.Guid.Empty) != 0)
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_PaymentTypeGuid", System.Data.DbType.Guid));
                            cmd.Parameters["@Waybill_PaymentTypeGuid"].Value = uuidPaymentTypeId;
                        }
                        if (uuidCustomerId.CompareTo(System.Guid.Empty) != 0)
                        {
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_CustomerGuid", System.Data.DbType.Guid));
                            cmd.Parameters["@Waybill_CustomerGuid"].Value = uuidCustomerId;
                        }
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OnlyUnShippedWaybills", System.Data.SqlDbType.Bit));
                        cmd.Parameters["@OnlyUnShippedWaybills"].Value = OnlyUnShippedWaybills;
                    }
                    else
                    {
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetWaybillListByIDList]", objProfile.GetOptionsDllDBName());
                        cmd.Parameters.AddWithValue("@tWaybillList", dtWaybillIdList);
                        cmd.Parameters["@tWaybillList"].SqlDbType = System.Data.SqlDbType.Structured;
                        cmd.Parameters["@tWaybillList"].TypeName = "dbo.udt_IDList";
                    }
                }
                else
                {
                    if (SelectWaybillInfoFromSuppl == true)
                    {
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetWaybillFromSuppl]", objProfile.GetOptionsDllDBName());
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Suppl_Guid", System.Data.DbType.Guid));
                        cmd.Parameters["@Suppl_Guid"].Value = Waybill_Guid;
                    }
                    else
                    {
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetWaybill]", objProfile.GetOptionsDllDBName());
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_Guid", System.Data.DbType.Guid));
                        cmd.Parameters["@Waybill_Guid"].Value = Waybill_Guid;
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
                        newRow["Waybill_Guid"] = ((rs["Waybill_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Waybill_Guid"] : System.Guid.Empty);
                        newRow["WaybillParent_Guid"] = ((rs["WaybillParent_Guid"] != System.DBNull.Value) ? (System.Guid)rs["WaybillParent_Guid"] : System.Guid.Empty);
                        newRow["Suppl_Guid"] = ((rs["Suppl_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Suppl_Guid"] : System.Guid.Empty);
                        newRow["Waybill_Id"] = ((rs["Waybill_Id"] != System.DBNull.Value) ? (System.Int32)rs["Waybill_Id"] : 0);
                        newRow["Stock_Guid"] = ((rs["Stock_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Stock_Guid"] : System.Guid.Empty);
                        newRow["Stock_Id"] = ((rs["Stock_Id"] != System.DBNull.Value) ? (System.Int32)rs["Stock_Id"] : 0);
                        newRow["Stock_Name"] = ((rs["Stock_Name"] != System.DBNull.Value) ? System.Convert.ToString(rs["Stock_Name"]) : System.String.Empty);
                        newRow["Stock_IsActive"] = ((rs["Stock_IsActive"] != System.DBNull.Value) ? System.Convert.ToBoolean(rs["Stock_IsActive"]) : false);
                        newRow["Stock_IsTrade"] = ((rs["Stock_IsTrade"] != System.DBNull.Value) ? System.Convert.ToBoolean(rs["Stock_IsTrade"]) : false);

                        newRow["Waybill_CanShip"] = ((rs["Waybill_CanShip"] != System.DBNull.Value) ? System.Convert.ToBoolean(rs["Waybill_CanShip"]) : false);
                        newRow["Depart_Guid"] = ((rs["Depart_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Depart_Guid"] : System.Guid.Empty);
                        newRow["Depart_Code"] = ((rs["Depart_Code"] != System.DBNull.Value) ? System.Convert.ToString(rs["Depart_Code"]) : System.String.Empty);

                        newRow["CustomerStateType_Guid"] = ((rs["CustomerStateType_Guid"] != System.DBNull.Value) ? (System.Guid)rs["CustomerStateType_Guid"] : System.Guid.Empty);
                        newRow["CustomerStateType_ShortName"] = ((rs["CustomerStateType_ShortName"] != System.DBNull.Value) ? System.Convert.ToString(rs["CustomerStateType_ShortName"]) : System.String.Empty);

                        newRow["Warehouse_Guid"] = ((rs["Warehouse_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Warehouse_Guid"] : System.Guid.Empty);
                        newRow["WarehouseType_Guid"] = ((rs["WarehouseType_Guid"] != System.DBNull.Value) ? (System.Guid)rs["WarehouseType_Guid"] : System.Guid.Empty);

                        newRow["Company_Guid"] = ((rs["Company_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Company_Guid"] : System.Guid.Empty);
                        newRow["Company_Id"] = ((rs["Company_Id"] != System.DBNull.Value) ? (System.Int32)rs["Company_Id"] : 0);
                        newRow["Company_Acronym"] = ((rs["Company_Acronym"] != System.DBNull.Value) ? System.Convert.ToString(rs["Company_Acronym"]) : System.String.Empty );
                        newRow["Company_Name"] = ((rs["Company_Name"] != System.DBNull.Value) ? System.Convert.ToString(rs["Company_Name"]) : System.String.Empty );

                        newRow["Customer_Guid"] = ((rs["Customer_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Customer_Guid"] : System.Guid.Empty);
                        newRow["Customer_Id"] = ((rs["Customer_Id"] != System.DBNull.Value) ? (System.Int32)rs["Customer_Id"] : 0);
                        newRow["Customer_Name"] = ((rs["Customer_Name"] != System.DBNull.Value) ? System.Convert.ToString(rs["Customer_Name"]) : System.String.Empty);
                        newRow["Customer_Name"] = ((rs["Customer_Name"] != System.DBNull.Value) ? System.Convert.ToString(rs["Customer_Name"]) : System.String.Empty);

                        newRow["Currency_Guid"] = ((rs["Currency_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Currency_Guid"] : System.Guid.Empty);
                        newRow["Currency_Abbr"] = ((rs["Currency_Abbr"] != System.DBNull.Value) ? System.Convert.ToString(rs["Currency_Abbr"]) : System.String.Empty);

                        newRow["ChildDepart_Guid"] = ((rs["ChildDepart_Guid"] != System.DBNull.Value) ? (System.Guid)rs["ChildDepart_Guid"] : System.Guid.Empty);
                        newRow["CustomerChild_Guid"] = ((rs["CustomerChild_Guid"] != System.DBNull.Value) ? (System.Guid)rs["CustomerChild_Guid"] : System.Guid.Empty);
                        newRow["ChildDepart_Code"] = ((rs["ChildDepart_Code"] != System.DBNull.Value) ? System.Convert.ToString(rs["ChildDepart_Code"]) : System.String.Empty);
                        newRow["ChildDepart_Name"] = ((rs["ChildDepart_Name"] != System.DBNull.Value) ? System.Convert.ToString(rs["ChildDepart_Name"]) : System.String.Empty);

                        newRow["Rtt_Guid"] = ((rs["Rtt_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Rtt_Guid"] : System.Guid.Empty);
                        newRow["Rtt_Name"] = ((rs["Rtt_Name"] != System.DBNull.Value) ? System.Convert.ToString(rs["Rtt_Name"]) : System.String.Empty);
                        newRow["Rtt_Name"] = ((rs["Rtt_Name"] != System.DBNull.Value) ? System.Convert.ToString(rs["Rtt_Name"]) : System.String.Empty);

                        newRow["Address_Guid"] = ((rs["Address_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Address_Guid"] : System.Guid.Empty);
                        newRow["Address_FullName"] = ((rs["Address_FullName"] != System.DBNull.Value) ? System.Convert.ToString(rs["Address_FullName"]) : System.String.Empty);

                        newRow["PaymentType_Guid"] = ((rs["PaymentType_Guid"] != System.DBNull.Value) ? (System.Guid)rs["PaymentType_Guid"] : System.Guid.Empty);
                        newRow["PaymentType_Name"] = ((rs["PaymentType_Name"] != System.DBNull.Value) ? System.Convert.ToString(rs["PaymentType_Name"]) : System.String.Empty);

                        newRow["Waybill_BeginDate"] = ((rs["Waybill_BeginDate"] != System.DBNull.Value) ? rs["Waybill_BeginDate"] : System.DBNull.Value);
                        newRow["Waybill_ShipDate"] = ((rs["Waybill_ShipDate"] != System.DBNull.Value) ? rs["Waybill_ShipDate"] : System.DBNull.Value);
                        newRow["Waybill_DeliveryDate"] = ((rs["Waybill_DeliveryDate"] != System.DBNull.Value) ? rs["Waybill_DeliveryDate"] : System.DBNull.Value);

                        newRow["Waybill_Num"] = rs["Waybill_Num"];
                        newRow["Waybill_Bonus"] = rs["Waybill_Bonus"];
                        newRow["WaybillState_Guid"] = rs["WaybillState_Guid"];
                        newRow["WaybillState_Id"] = rs["WaybillState_Id"];
                        newRow["WaybillState_Name"] = rs["WaybillState_Name"];

                        newRow["WaybillShipMode_Guid"] = rs["WaybillShipMode_Guid"];
                        newRow["WaybillShipMode_Id"] = rs["WaybillShipMode_Id"];
                        newRow["WaybillShipMode_Name"] = rs["WaybillShipMode_Name"];

                        newRow["Waybill_Description"] = rs["Waybill_Description"];

                        newRow["Waybill_AllPrice"] = rs["Waybill_AllPrice"];
                        newRow["Waybill_RetAllPrice"] = rs["Waybill_RetAllPrice"];
                        newRow["Waybill_AllDiscount"] = rs["Waybill_AllDiscount"];
                        newRow["Waybill_AmountPaid"] = rs["Waybill_AmountPaid"];
                        newRow["Waybill_CurrencyAllPrice"] = rs["Waybill_CurrencyAllPrice"];
                        newRow["Waybill_CurrencyRetAllPrice"] = rs["Waybill_CurrencyRetAllPrice"];
                        newRow["Waybill_CurrencyAllDiscount"] = rs["Waybill_CurrencyAllDiscount"];
                        newRow["Waybill_CurrencyAmountPaid"] = rs["Waybill_CurrencyAmountPaid"];

                        newRow["Waybill_Quantity"] = System.Convert.ToDecimal( rs["Waybill_Quantity"] );
                        newRow["Waybill_RetQuantity"] = System.Convert.ToDecimal( rs["Waybill_RetQuantity"] );
                        newRow["Waybill_Weight"] = System.Convert.ToDecimal(rs["Waybill_Weight"]);
                        newRow["Waybill_ShowInDeliveryList"] = rs["Waybill_ShowInDeliveryList"];

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
                strErr += (String.Format("\nНе удалось получить таблицу с накладными.\nТекст ошибки: {0}", f.Message));
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
        /// <param name="uuidWaybillId">уи накладной</param>
        /// <param name="strErr">строка с сообщением об ошибке</param>
        /// <param name="bTableFromSuppl">признак "приложение запрашивается из заказа"</param>
        /// <returns>приложение к накладной в виде объекта класса DataTable</returns>
        public static System.Data.DataTable GetWaybilItemTable(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidWaybillId, ref System.String strErr, System.Boolean bTableFromSuppl = false)
        {
            System.Data.DataTable dtReturn = new System.Data.DataTable();

            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_Id", typeof(System.Int32)));
            dtReturn.Columns.Add(new System.Data.DataColumn("SupplItem_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Waybill_Guid", typeof(System.Guid)));

            dtReturn.Columns.Add(new System.Data.DataColumn("Parts_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Measure_Guid", typeof(System.Guid)));
            
            dtReturn.Columns.Add(new System.Data.DataColumn("ProductOwnerName", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("PARTS_NAME", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("PARTS_ARTICLE", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Measure_ShortName", typeof(System.String)));

            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_Quantity", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_RetQuantity", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_LeavQuantity", typeof(System.Double)));

            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_Price", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_Discount", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_DiscountPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_AllPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_TotalPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_PriceImporter", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_LeavTotalPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_CurrencyPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_CurrencyDiscountPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_CurrencyAllPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_CurrencyTotalPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_CurrencyleavTotalPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybItem_NDSPercent", typeof(System.Double)));

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

                if (bTableFromSuppl == true)
                {
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetWaybItemsFromSuppl]", objProfile.GetOptionsDllDBName());
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Suppl_Guid", System.Data.DbType.Guid));
                    cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                    cmd.Parameters["@Suppl_Guid"].Value = uuidWaybillId;
                }
                else
                {
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetWaybItems]", objProfile.GetOptionsDllDBName());
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_Guid", System.Data.DbType.Guid));
                    cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                    cmd.Parameters["@Waybill_Guid"].Value = uuidWaybillId;
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

                        newRow["WaybItem_Guid"] = ((rs["WaybItem_Guid"] != System.DBNull.Value) ? (System.Guid)rs["WaybItem_Guid"] : System.Guid.Empty);
                        newRow["SupplItem_Guid"] = ((rs["SupplItem_Guid"] != System.DBNull.Value) ? (System.Guid)rs["SupplItem_Guid"] : System.Guid.Empty);
                        newRow["Waybill_Guid"] = ((rs["Waybill_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Waybill_Guid"] : System.Guid.Empty);
                        newRow["WaybItem_Id"] = ((rs["WaybItem_Id"] != System.DBNull.Value) ? (System.Int32)rs["WaybItem_Id"] : 0);
                        newRow["Measure_Guid"] = ((rs["Measure_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Measure_Guid"] : System.Guid.Empty);
                        newRow["Parts_Guid"] = ((rs["Parts_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Parts_Guid"] : System.Guid.Empty);

                        newRow["ProductOwnerName"] = ((rs["ProductOwnerName"] != System.DBNull.Value) ? System.Convert.ToString(rs["ProductOwnerName"]) : System.String.Empty);
                        newRow["PARTS_NAME"] = ((rs["PARTS_NAME"] != System.DBNull.Value) ? System.Convert.ToString(rs["PARTS_NAME"]) : System.String.Empty);
                        newRow["PARTS_ARTICLE"] = ((rs["PARTS_ARTICLE"] != System.DBNull.Value) ? System.Convert.ToString(rs["PARTS_ARTICLE"]) : System.String.Empty);
                        newRow["Measure_ShortName"] = ((rs["Measure_ShortName"] != System.DBNull.Value) ? System.Convert.ToString(rs["Measure_ShortName"]) : System.String.Empty);

                        newRow["WaybItem_Quantity"] = rs["WaybItem_Quantity"];
                        newRow["WaybItem_RetQuantity"] = rs["WaybItem_RetQuantity"];
                        newRow["WaybItem_LeavQuantity"] = rs["WaybItem_LeavQuantity"];

                        newRow["WaybItem_Price"] = rs["WaybItem_Price"];
                        newRow["WaybItem_Discount"] = rs["WaybItem_Discount"];
                        newRow["WaybItem_DiscountPrice"] = rs["WaybItem_DiscountPrice"];
                        newRow["WaybItem_AllPrice"] = rs["WaybItem_AllPrice"];
                        newRow["WaybItem_TotalPrice"] = rs["WaybItem_TotalPrice"];
                        newRow["WaybItem_PriceImporter"] = rs["WaybItem_PriceImporter"];
                        newRow["WaybItem_LeavTotalPrice"] = rs["WaybItem_LeavTotalPrice"];

                        newRow["WaybItem_CurrencyPrice"] = rs["WaybItem_CurrencyPrice"];
                        newRow["WaybItem_CurrencyPrice"] = rs["WaybItem_CurrencyPrice"];
                        newRow["WaybItem_CurrencyDiscountPrice"] = rs["WaybItem_CurrencyDiscountPrice"];
                        newRow["WaybItem_CurrencyAllPrice"] = rs["WaybItem_CurrencyAllPrice"];
                        newRow["WaybItem_CurrencyTotalPrice"] = rs["WaybItem_CurrencyTotalPrice"];
                        newRow["WaybItem_CurrencyleavTotalPrice"] = rs["WaybItem_CurrencyleavTotalPrice"];
                        newRow["WaybItem_NDSPercent"] = rs["WaybItem_NDSPercent"];

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

        #region Создание накладной из заказа
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
        /// <returns>код возврата хранимой процедуры</returns>
        public static System.Int32 CreateWaybillFromSuppl(UniXP.Common.CProfile objProfile, 
            System.Guid Suppl_Guid, System.DateTime Document_Date, 
            System.String Document_Num, System.Boolean DocumentSendToStock, 
            ref System.Guid Waybill_Guid, ref System.Guid SupplState_Guid, ref System.String strErr)
        {
            System.Int32 iRet = 0;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    iRet = -1;
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Не удалось получить соединение с базой данных.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return iRet;
                }
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddWaybillFromSupplInIB]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Suppl_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Document_Date", System.Data.SqlDbType.DateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Document_Num", System.Data.SqlDbType.NVarChar, 128));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DocumentSendToStock", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_Guid", System.Data.SqlDbType.UniqueIdentifier) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SupplState_Guid", System.Data.SqlDbType.UniqueIdentifier) { Direction = System.Data.ParameterDirection.Output });
                
                cmd.Parameters["@Suppl_Guid"].Value = Suppl_Guid;
                cmd.Parameters["@Document_Date"].Value = Document_Date;
                cmd.Parameters["@Document_Num"].Value = Document_Num;
                cmd.Parameters["@DocumentSendToStock"].Value = DocumentSendToStock;
                cmd.ExecuteNonQuery();

                iRet = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;

                if (iRet == 0)
                {
                    Waybill_Guid = (System.Guid)cmd.Parameters["@Waybill_Guid"].Value;
                    SupplState_Guid = (System.Guid)cmd.Parameters["@SupplState_Guid"].Value;
                }

                cmd.Dispose();
                DBConnection.Close();
            }
            catch (System.Exception f)
            {
                strErr = "Не удалось сформировать накладную.\n\nТекст ошибки: " + f.Message;
            }
            return iRet;
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
            System.Guid Suppl_Guid,  ref System.String strErr)
        {
            System.Guid Waybill_Guid = System.Guid.Empty;

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    strErr += ("\nНе удалось получить соединение с базой данных.");
                    return Waybill_Guid;
                }
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetSupplWaybillGuid]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Suppl_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_Guid", System.Data.SqlDbType.UniqueIdentifier) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });

                cmd.Parameters["@Suppl_Guid"].Value = Suppl_Guid;
                cmd.ExecuteNonQuery();

                System.Int32 iRet = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;

                if ((iRet == 0) && (cmd.Parameters["@Waybill_Guid"].Value != System.DBNull.Value))
                {
                    Waybill_Guid = (System.Guid)cmd.Parameters["@Waybill_Guid"].Value;
                }

                cmd.Dispose();
                DBConnection.Close();
            }
            catch (System.Exception f)
            {
                strErr = "Не удалось получить ссылку на накладную.\n\nТекст ошибки: " + f.Message;
            }
            return Waybill_Guid;
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_CanCreateWaybillFromSuppl]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Suppl_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CanCreateWaybillFromSuppl", System.Data.SqlDbType.Bit) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });

                cmd.Parameters["@Suppl_Guid"].Value = Suppl_Guid;
                cmd.ExecuteNonQuery();

                System.Int32 iRet = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                bRet = (System.Boolean)cmd.Parameters["@CanCreateWaybillFromSuppl"].Value;

                cmd.Dispose();
                DBConnection.Close();
            }
            catch (System.Exception f)
            {
                strErr = "CanCreateWaybillFromSuppl.\n\nТекст ошибки: " + f.Message;
            }
            return bRet;
        }
        #endregion

        #region Настройки по умолчанию для журнала накладных
        /// <summary>
        /// Запрашивает настройки по-умолчанию для журнала накладных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="CanViewPaymentType2">признак "имеется доступ на просмотр накладных по форме оплаты №2"</param>
        /// <param name="DefPaymentTypeId">код формы оплаты по умолчанию</param>
        /// <param name="BlockOtherPaymentType">признак "блокировать остальные формы оплаты, кроме того, что по умолчанию"</param>
        /// <param name="CompanyAcronymForPaymentType1">сокр-е наименование компании для формы оплаты №1</param>
        /// <param name="CompanyAcronymForPaymentType2">сокр-е наименование компании для формы оплаты №2</param>
        /// <param name="ERROR_NUM">код возврата хранимой процедуры</param>
        /// <param name="strErr">текст ошибки</param>
        /// <returns>код возврата хранимой процедуры</returns>
        public static System.Int32 GetWaybillListSettingsDefault(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Boolean CanViewPaymentType2, ref System.Int32 DefPaymentTypeId, ref System.Boolean BlockOtherPaymentType,
            ref System.String CompanyAcronymForPaymentType1, ref System.String CompanyAcronymForPaymentType2, 
            ref System.Int32 ERROR_NUM, ref System.String strErr)
        {
            System.Int32 iRet = -1;

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
                        return iRet;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetWaybillListSettingsDefault]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CanViewPaymentType2", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DefPaymentTypeId", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BlockOtherPaymentType", System.Data.SqlDbType.Bit) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyAcronymForPaymentType1", System.Data.SqlDbType.NVarChar, 16) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyAcronymForPaymentType2", System.Data.SqlDbType.NVarChar, 16) { Direction = System.Data.ParameterDirection.Output });

                cmd.Parameters["@CanViewPaymentType2"].Value = CanViewPaymentType2;

                iRet = cmd.ExecuteNonQuery();

                iRet = System.Convert.ToInt32(cmd.Parameters["@ERROR_NUM"].Value);

                if (cmd.Parameters["@ERROR_NUM"].Value != System.DBNull.Value) { ERROR_NUM = (System.Convert.ToInt32(cmd.Parameters["@ERROR_NUM"].Value)); }
                if (cmd.Parameters["@ERROR_MES"].Value != System.DBNull.Value) { strErr += (System.Convert.ToString(cmd.Parameters["@ERROR_MES"].Value)); }
                if (cmd.Parameters["@DefPaymentTypeId"].Value != System.DBNull.Value) { DefPaymentTypeId = (System.Convert.ToInt32(cmd.Parameters["@DefPaymentTypeId"].Value)); }
                if (cmd.Parameters["@BlockOtherPaymentType"].Value != System.DBNull.Value) { BlockOtherPaymentType = (System.Convert.ToBoolean(cmd.Parameters["@BlockOtherPaymentType"].Value)); } 
                if (cmd.Parameters["@CompanyAcronymForPaymentType1"].Value != System.DBNull.Value) { CompanyAcronymForPaymentType1 = (System.Convert.ToString(cmd.Parameters["@CompanyAcronymForPaymentType1"].Value)); }
                if (cmd.Parameters["@CompanyAcronymForPaymentType2"].Value != System.DBNull.Value) { CompanyAcronymForPaymentType2 = (System.Convert.ToString(cmd.Parameters["@CompanyAcronymForPaymentType2"].Value)); }

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                strErr += (String.Format("\nНе удалось получить настройки для журнала накладных.\nТекст ошибки: {0}", f.Message));
            }

            return iRet;
        }
        #endregion

        #region Сохранение накладной в БД
        /// <summary>
        /// Добавляет в БД информацию о накладной
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
            System.DateTime Waybill_BeginDate, System.DateTime Waybill_DeliveryDate,  System.Guid WaybillParent_Guid, System.Boolean Waybill_Bonus,
            System.Guid WaybillState_Guid, System.Guid WaybillShipMode_Guid, System.DateTime Waybill_ShipDate, 
            System.String Waybill_Description, System.Double Waybill_CurrencyRate, System.Boolean Waybill_ShowInDeliveryList,
            System.Data.DataTable WaybillTablePart,
            ref System.Guid Waybill_Guid, ref System.Int32 Waybill_Id, ref System.Guid SupplState_Guid, ref System.String strErr,
            System.Boolean SetWaybillInQueue = false, System.Boolean DocumentSendToStock = false)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddWaybill]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_Id", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SupplState_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_BeginDate", System.Data.SqlDbType.Date));
                if (WaybillState_Guid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillState_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@WaybillState_Guid"].Value = WaybillState_Guid;
                }
                if (WaybillParent_Guid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillParent_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@WaybillParent_Guid"].Value = WaybillParent_Guid;
                }
                if (CustomerChild_Guid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerChild_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@CustomerChild_Guid"].Value = CustomerChild_Guid;
                }
                if ( System.DateTime.Compare( Waybill_ShipDate, System.DateTime.MinValue ) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_ShipDate", System.Data.SqlDbType.Date));
                    cmd.Parameters["@Waybill_ShipDate"].Value = Waybill_ShipDate;
                }

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Suppl_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_Bonus", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Depart_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillShipMode_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PaymentType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_Num", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_DeliveryDate", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Rtt_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Address_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_CurrencyRate", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_ShowInDeliveryList", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DocumentSendToStock", System.Data.SqlDbType.Bit));

                cmd.Parameters.AddWithValue("@tWaybItms", WaybillTablePart);
                cmd.Parameters["@tWaybItms"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tWaybItms"].TypeName = "dbo.udt_WaybItms";

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters["@Suppl_Guid"].Value = Suppl_Guid;
                cmd.Parameters["@Waybill_BeginDate"].Value = Waybill_BeginDate;
                cmd.Parameters["@Waybill_Bonus"].Value = Waybill_Bonus;
                cmd.Parameters["@Depart_Guid"].Value = Depart_Guid;
                cmd.Parameters["@Customer_Guid"].Value = Customer_Guid;
                cmd.Parameters["@WaybillShipMode_Guid"].Value = WaybillShipMode_Guid;
                cmd.Parameters["@PaymentType_Guid"].Value = PaymentType_Guid;
                cmd.Parameters["@Waybill_Description"].Value = Waybill_Description;
                cmd.Parameters["@Waybill_Num"].Value = Waybill_Num;
                cmd.Parameters["@Waybill_DeliveryDate"].Value = Waybill_DeliveryDate;
                cmd.Parameters["@Rtt_Guid"].Value = Rtt_Guid;
                cmd.Parameters["@Address_Guid"].Value = Address_Guid;
                cmd.Parameters["@Stock_Guid"].Value = Stock_Guid;
                cmd.Parameters["@Company_Guid"].Value = Company_Guid;
                cmd.Parameters["@Waybill_CurrencyRate"].Value = Waybill_CurrencyRate;
                cmd.Parameters["@Waybill_ShowInDeliveryList"].Value = Waybill_ShowInDeliveryList;
                cmd.Parameters["@DocumentSendToStock"].Value = DocumentSendToStock;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                strErr += (System.Convert.ToString(cmd.Parameters["@ERROR_MES"].Value));

                if (iRes == 0)
                {
                    Waybill_Guid = (System.Guid)cmd.Parameters["@Waybill_Guid"].Value;
                    Waybill_Id = (System.Int32)cmd.Parameters["@Waybill_Id"].Value;
                    SupplState_Guid = (System.Guid)cmd.Parameters["@SupplState_Guid"].Value;

                    strErr = "Заказ переведен в накладную.";
                }
                else
                {
                    strErr = strErr.Replace("\r", "\n");
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
        #endregion

        #region Объединение накладных
        /// <summary>
        /// Объедениение накладных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="MainWaybill_Guid">УИ "главной" накладной</param>
        /// <param name="MainDepart_Guid">УИ подразделения "главной" накладной</param>
        /// <param name="MainWaybill_Num">УИ накладной "главной" накладной</param>
        /// <param name="ChildWaybillList">список идентификаторов "дочерних" накладных</param>
        /// <param name="strErr">текст ошибки</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean AddJoinDepartToDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid MainWaybill_Guid, System.Guid MainDepart_Guid, System.String MainWaybill_Num,
            System.Data.DataTable ChildWaybillList,  ref System.String strErr )
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddJoinDepart]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@MainWaybill_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@MainDepart_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@MainWaybill_Num", System.Data.DbType.String));

                cmd.Parameters.AddWithValue("@tChildWaybillList", ChildWaybillList);
                cmd.Parameters["@tChildWaybillList"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tChildWaybillList"].TypeName = "dbo.udt_GuidList";

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });

                cmd.Parameters["@MainWaybill_Guid"].Value = MainWaybill_Guid;
                cmd.Parameters["@MainDepart_Guid"].Value = MainDepart_Guid;
                cmd.Parameters["@MainWaybill_Num"].Value = MainWaybill_Num;
                
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                strErr += (System.Convert.ToString(cmd.Parameters["@ERROR_MES"].Value));

                if (iRes == 0)
                {
                    strErr = "Объединение накладных завершено.";
                }
                else
                {
                    strErr = strErr.Replace("\r", "\n");
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_CanCancelWaybill]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CanCancelWaybill", System.Data.SqlDbType.Bit) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });

                cmd.Parameters["@Waybill_Guid"].Value = Waybill_Guid;
                cmd.ExecuteNonQuery();

                System.Int32 iRet = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                bRet = (System.Boolean)cmd.Parameters["@CanCancelWaybill"].Value;

                cmd.Dispose();
                DBConnection.Close();
            }
            catch (System.Exception f)
            {
                strErr = "CanCancelWaybill.\n\nТекст ошибки: " + f.Message;
            }
            return bRet;
        }
        #endregion

        #region Аннулирование накладной
        /// <summary>
        /// Аннулирование накладной
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="Waybill_Guid">УИ накладной</param>
        /// <param name="WaybillState_Guid">УИ состояния накладной</param>
        /// <param name="strErr">текст ошибки</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean CancelWaybill(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid Waybill_Guid, ref System.Guid WaybillState_Guid, ref System.String strErr)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_CancelWaybill]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillState_Guid", System.Data.SqlDbType.UniqueIdentifier) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });

                cmd.Parameters["@Waybill_Guid"].Value = Waybill_Guid;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                strErr += (System.Convert.ToString(cmd.Parameters["@ERROR_MES"].Value));

                if (iRes == 0)
                {
                    WaybillState_Guid = (System.Guid)cmd.Parameters["@WaybillState_Guid"].Value;

                    strErr = "Накладная аннулирована.";
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

        #region Отгрузка накладной
        /// <summary>
        /// Отгрузка товара по накладной
        /// </summary>
        /// <param name="objProfile">Профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="Waybill_Guid">УИ накладной на отгрузку</param>
        /// <param name="Waybill_ShipDate">Дата отгрузки</param>
        /// <param name="SetWaybillShipMode_Guid">УИ варианта отгрузки</param>
        /// <param name="ShipDescription">Примечание</param>
        /// <param name="WaybillState_Guid">УИ текущего состояния накладной</param>
        /// <param name="ERROR_NUM">целочисленный код ошибки</param>
        /// <param name="ERROR_MES">текст ошибки</param>
        /// <returns>0 - накладная отгружена; <>0 - ошибка</returns>
        public static System.Int32 ShippedProductsByWaybill(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid Waybill_Guid, System.DateTime Waybill_ShipDate, 
            System.Guid SetWaybillShipMode_Guid, System.String ShipDescription,
            ref System.Guid WaybillState_Guid, ref System.Int32 ERROR_NUM, ref System.String ERROR_MES)
        {
            System.Int32 iRet = -1;

            if( Waybill_Guid.CompareTo(System.Guid.Empty) == 0)
            {
                ERROR_MES += ("\nНе указан идентификатор накладной.");
                return iRet;
            }

            if (Waybill_ShipDate.CompareTo(System.DateTime.MinValue) == 0)
            {
                ERROR_MES += ("\nУкажите, пожалуйста, дату отгрузки.");
                return iRet;
            }

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        ERROR_MES += ("\nНе удалось получить соединение с базой данных.");
                        return iRet;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_ShipProductsByWaybill]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_ShipDate", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SetWaybillShipMode_Guid", System.Data.SqlDbType.UniqueIdentifier));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillState_Guid", System.Data.SqlDbType.UniqueIdentifier) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });

                cmd.Parameters["@Waybill_Guid"].Value = Waybill_Guid;
                cmd.Parameters["@Waybill_ShipDate"].Value = Waybill_ShipDate;
                cmd.Parameters["@SetWaybillShipMode_Guid"].Value = SetWaybillShipMode_Guid;

                if (ShipDescription.Trim().Length > 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ShipDescription", System.Data.SqlDbType.NVarChar, 48));
                    cmd.Parameters["@ShipDescription"].Value = ShipDescription;
                }

                iRet = cmd.ExecuteNonQuery();

                iRet = System.Convert.ToInt32(cmd.Parameters["@ERROR_NUM"].Value);

                if (cmd.Parameters["@ERROR_NUM"].Value != System.DBNull.Value) { ERROR_NUM = (System.Convert.ToInt32(cmd.Parameters["@ERROR_NUM"].Value)); }
                if (cmd.Parameters["@ERROR_MES"].Value != System.DBNull.Value) { ERROR_MES += (System.Convert.ToString(cmd.Parameters["@ERROR_MES"].Value)); }
                if (cmd.Parameters["@WaybillState_Guid"].Value != System.DBNull.Value) { WaybillState_Guid = (System.Guid)cmd.Parameters["@WaybillState_Guid"].Value; }

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                ERROR_MES += (String.Format("\nНе удалось выполнить отгрузку накладной.\nТекст ошибки: {0}", f.Message));
            }

            return iRet;
        }
        
        #endregion

        #region Установка признака "накладную можно отгружать" 
        /// <summary>
        /// Устанавливает признак "накладную можно отгружать"  для списка накладных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="WaybillGuidList">список идентификаторов накладных</param>
        /// <param name="CanShip">признак "накладную можно отгружать"</param>
        /// <param name="strErr">текст ошибки</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean SetShipRemarkForWaybillList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Data.DataTable WaybillGuidList, System.Boolean CanShip,  ref System.String strErr)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_SetShipRemarkForWaybills]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CanShip", System.Data.SqlDbType.Bit));

                cmd.Parameters.AddWithValue("@tWaybillList", WaybillGuidList);
                cmd.Parameters["@tWaybillList"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tWaybillList"].TypeName = "dbo.udt_GuidList";

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });

                cmd.Parameters["@CanShip"].Value = CanShip;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                strErr += (System.Convert.ToString(cmd.Parameters["@ERROR_MES"].Value));

                if (iRes == 0)
                {
                    strErr = "Операция завершена.";
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
