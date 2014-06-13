using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP_Mercury.Common
{
    public class CIntWaybillDataBaseModel
    {
        #region Журнал накладных

        /// <summary>
        /// Возвращает таблицу с накладными за указанный период
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
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
        /// <returns>таблицу</returns>
        public static System.Data.DataTable GetWaybillTable(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid IntWaybill_Guid,
            System.DateTime IntWaybill_DateBegin, System.DateTime IntWaybill_DateEnd,
            System.Guid IntWaybill_SrcCompanyGuid, System.Guid IntWaybill_SrcStockGuid,
            System.Guid IntWaybill_DstCompanyGuid, System.Guid IntWaybill_DstStockGuid,
            System.Guid Waybill_PaymentTypeGuid, ref System.String strErr, 
            System.Boolean SelectIntWaybillInfoFromIntOrder = false,
            System.Boolean OnlyUnShippedWaybills = false
            )
        {
            System.Data.DataTable dtReturn = new System.Data.DataTable();


            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybill_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybill_Id", typeof(System.Int32)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrder_Guid", typeof(System.Guid)));

            dtReturn.Columns.Add(new System.Data.DataColumn("SrcStock_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("SrcStock_Name", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("SrcStock_Id", typeof(System.Int32)));
            dtReturn.Columns.Add(new System.Data.DataColumn("SrcStock_IsActive", typeof(System.Boolean)));
            dtReturn.Columns.Add(new System.Data.DataColumn("SrcStock_IsTrade", typeof(System.Boolean)));
            dtReturn.Columns.Add(new System.Data.DataColumn("SrcWarehouse_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("SrcWarehouseType_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("SrcCompany_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("SrcCompany_Id", typeof(System.Int32)));
            dtReturn.Columns.Add(new System.Data.DataColumn("SrcCompany_Acronym", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("SrcCompany_Name", typeof(System.String)));

            dtReturn.Columns.Add(new System.Data.DataColumn("DstStock_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("DstStock_Name", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("DstStock_Id", typeof(System.Int32)));
            dtReturn.Columns.Add(new System.Data.DataColumn("DstStock_IsActive", typeof(System.Boolean)));
            dtReturn.Columns.Add(new System.Data.DataColumn("DstStock_IsTrade", typeof(System.Boolean)));
            dtReturn.Columns.Add(new System.Data.DataColumn("DstWarehouse_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("DstWarehouseType_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("DstCompany_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("DstCompany_Id", typeof(System.Int32)));
            dtReturn.Columns.Add(new System.Data.DataColumn("DstCompany_Acronym", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("DstCompany_Name", typeof(System.String)));

            dtReturn.Columns.Add(new System.Data.DataColumn("Currency_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Currency_Abbr", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Depart_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Depart_Code", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("PaymentType_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("PaymentType_Name", typeof(System.String)));

            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybillShipMode_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybillShipMode_Id", typeof(System.Int32)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybillShipMode_Name", typeof(System.String)));
            
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybillState_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybillState_Id", typeof(System.Int32)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybillState_Name", typeof(System.String)));

            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybill_Num", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("RetailWaybill_Num", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybill_BeginDate", typeof(System.DateTime)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybillParent_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybill_ShipDate", typeof(System.DateTime)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybill_Description", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybill_CurrencyRate", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybill_AllPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybill_RetailAllPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybill_AllDiscount", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybill_TotalPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybill_Quantity", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybill_ForStock", typeof(System.Boolean)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybill_Send", typeof(System.Boolean)));

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

                if (IntWaybill_Guid.CompareTo(System.Guid.Empty) == 0)
                {
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetIntWaybillList]", objProfile.GetOptionsDllDBName());
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntWaybill_DateBegin", System.Data.DbType.Date));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntWaybill_DateEnd", System.Data.DbType.Date));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OnlyUnShippedWaybills", System.Data.DbType.Guid));
                    cmd.Parameters["@IntWaybill_DateBegin"].Value = IntWaybill_DateBegin;
                    cmd.Parameters["@IntWaybill_DateEnd"].Value = IntWaybill_DateEnd;
                    cmd.Parameters["@OnlyUnShippedWaybills"].Value = OnlyUnShippedWaybills;

                    if (IntWaybill_SrcCompanyGuid.CompareTo(System.Guid.Empty) != 0)
                    {
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntWaybill_SrcCompanyGuid", System.Data.DbType.Guid));
                        cmd.Parameters["@IntWaybill_SrcCompanyGuid"].Value = IntWaybill_SrcCompanyGuid;
                    }
                    if (IntWaybill_SrcStockGuid.CompareTo(System.Guid.Empty) != 0)
                    {
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntWaybill_SrcStockGuid", System.Data.DbType.Guid));
                        cmd.Parameters["@IntWaybill_SrcStockGuid"].Value = IntWaybill_SrcStockGuid;
                    }

                    if (IntWaybill_DstCompanyGuid.CompareTo(System.Guid.Empty) != 0)
                    {
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntWaybill_DstCompanyGuid", System.Data.DbType.Guid));
                        cmd.Parameters["@IntWaybill_DstCompanyGuid"].Value = IntWaybill_DstCompanyGuid;
                    }
                    if (IntWaybill_DstStockGuid.CompareTo(System.Guid.Empty) != 0)
                    {
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntWaybill_DstStockGuid", System.Data.DbType.Guid));
                        cmd.Parameters["@IntWaybill_DstStockGuid"].Value = IntWaybill_DstStockGuid;
                    }

                    if (Waybill_PaymentTypeGuid.CompareTo(System.Guid.Empty) != 0)
                    {
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_PaymentTypeGuid", System.Data.DbType.Guid));
                        cmd.Parameters["@Waybill_PaymentTypeGuid"].Value = Waybill_PaymentTypeGuid;
                    }
                }
                else
                {
                    if (SelectIntWaybillInfoFromIntOrder == true)
                    {
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetIntWaybillFromIntOrder]", objProfile.GetOptionsDllDBName());
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrder_Guid", System.Data.DbType.Guid));
                        cmd.Parameters["@IntOrder_Guid"].Value = IntWaybill_Guid;
                    }
                    else
                    {
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetIntWaybill]", objProfile.GetOptionsDllDBName());
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntWaybill_Guid", System.Data.DbType.Guid));
                        cmd.Parameters["@IntWaybill_Guid"].Value = IntWaybill_Guid;
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
                        newRow["IntWaybill_Guid"] = ((rs["IntWaybill_Guid"] != System.DBNull.Value) ? (System.Guid)rs["IntWaybill_Guid"] : System.Guid.Empty);
                        newRow["IntWaybillParent_Guid"] = ((rs["IntWaybillParent_Guid"] != System.DBNull.Value) ? (System.Guid)rs["IntWaybillParent_Guid"] : System.Guid.Empty);
                        newRow["IntOrder_Guid"] = ((rs["IntOrder_Guid"] != System.DBNull.Value) ? (System.Guid)rs["IntOrder_Guid"] : System.Guid.Empty);
                        newRow["IntWaybill_Id"] = ((rs["IntWaybill_Id"] != System.DBNull.Value) ? (System.Int32)rs["IntWaybill_Id"] : 0);
                        
                        newRow["SrcStock_Guid"] = ((rs["SrcStock_Guid"] != System.DBNull.Value) ? (System.Guid)rs["SrcStock_Guid"] : System.Guid.Empty);
                        newRow["SrcStock_Id"] = ((rs["SrcStock_Id"] != System.DBNull.Value) ? (System.Int32)rs["SrcStock_Id"] : 0);
                        newRow["SrcStock_Name"] = ((rs["SrcStock_Name"] != System.DBNull.Value) ? System.Convert.ToString(rs["SrcStock_Name"]) : System.String.Empty);
                        newRow["SrcStock_IsActive"] = ((rs["SrcStock_IsActive"] != System.DBNull.Value) ? System.Convert.ToBoolean(rs["SrcStock_IsActive"]) : false);
                        newRow["SrcStock_IsTrade"] = ((rs["SrcStock_IsTrade"] != System.DBNull.Value) ? System.Convert.ToBoolean(rs["SrcStock_IsTrade"]) : false);

                        newRow["SrcCompany_Guid"] = ((rs["SrcCompany_Guid"] != System.DBNull.Value) ? (System.Guid)rs["SrcCompany_Guid"] : System.Guid.Empty);
                        newRow["SrcCompany_Id"] = ((rs["SrcCompany_Id"] != System.DBNull.Value) ? (System.Int32)rs["SrcCompany_Id"] : 0);
                        newRow["SrcCompany_Acronym"] = ((rs["SrcCompany_Acronym"] != System.DBNull.Value) ? System.Convert.ToString(rs["SrcCompany_Acronym"]) : System.String.Empty);
                        newRow["SrcCompany_Name"] = ((rs["SrcCompany_Name"] != System.DBNull.Value) ? System.Convert.ToString(rs["SrcCompany_Name"]) : System.String.Empty);
                        newRow["SrcWarehouse_Guid"] = ((rs["SrcWarehouse_Guid"] != System.DBNull.Value) ? (System.Guid)rs["SrcWarehouse_Guid"] : System.Guid.Empty);
                        newRow["SrcWarehouseType_Guid"] = ((rs["SrcWarehouseType_Guid"] != System.DBNull.Value) ? (System.Guid)rs["SrcWarehouseType_Guid"] : System.Guid.Empty);


                        newRow["DstStock_Guid"] = ((rs["DstStock_Guid"] != System.DBNull.Value) ? (System.Guid)rs["DstStock_Guid"] : System.Guid.Empty);
                        newRow["DstStock_Id"] = ((rs["DstStock_Id"] != System.DBNull.Value) ? (System.Int32)rs["DstStock_Id"] : 0);
                        newRow["DstStock_Name"] = ((rs["DstStock_Name"] != System.DBNull.Value) ? System.Convert.ToString(rs["DstStock_Name"]) : System.String.Empty);
                        newRow["DstStock_IsActive"] = ((rs["DstStock_IsActive"] != System.DBNull.Value) ? System.Convert.ToBoolean(rs["DstStock_IsActive"]) : false);
                        newRow["DstStock_IsTrade"] = ((rs["DstStock_IsTrade"] != System.DBNull.Value) ? System.Convert.ToBoolean(rs["DstStock_IsTrade"]) : false);
                        newRow["DstWarehouse_Guid"] = ((rs["DstWarehouse_Guid"] != System.DBNull.Value) ? (System.Guid)rs["DstWarehouse_Guid"] : System.Guid.Empty);
                        newRow["DstWarehouseType_Guid"] = ((rs["DstWarehouseType_Guid"] != System.DBNull.Value) ? (System.Guid)rs["DstWarehouseType_Guid"] : System.Guid.Empty);

                        newRow["DstCompany_Guid"] = ((rs["DstCompany_Guid"] != System.DBNull.Value) ? (System.Guid)rs["DstCompany_Guid"] : System.Guid.Empty);
                        newRow["DstCompany_Id"] = ((rs["DstCompany_Id"] != System.DBNull.Value) ? (System.Int32)rs["DstCompany_Id"] : 0);
                        newRow["DstCompany_Acronym"] = ((rs["DstCompany_Acronym"] != System.DBNull.Value) ? System.Convert.ToString(rs["DstCompany_Acronym"]) : System.String.Empty);
                        newRow["DstCompany_Name"] = ((rs["DstCompany_Name"] != System.DBNull.Value) ? System.Convert.ToString(rs["DstCompany_Name"]) : System.String.Empty);

                        newRow["Depart_Guid"] = ((rs["Depart_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Depart_Guid"] : System.Guid.Empty);
                        newRow["Depart_Code"] = ((rs["Depart_Code"] != System.DBNull.Value) ? System.Convert.ToString(rs["Depart_Code"]) : System.String.Empty);

                        newRow["Currency_Guid"] = ((rs["Currency_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Currency_Guid"] : System.Guid.Empty);
                        newRow["Currency_Abbr"] = ((rs["Currency_Abbr"] != System.DBNull.Value) ? System.Convert.ToString(rs["Currency_Abbr"]) : System.String.Empty);

                        newRow["PaymentType_Guid"] = ((rs["PaymentType_Guid"] != System.DBNull.Value) ? (System.Guid)rs["PaymentType_Guid"] : System.Guid.Empty);
                        newRow["PaymentType_Name"] = ((rs["PaymentType_Name"] != System.DBNull.Value) ? System.Convert.ToString(rs["PaymentType_Name"]) : System.String.Empty);

                        newRow["IntWaybillState_Guid"] = rs["IntWaybillState_Guid"];
                        newRow["IntWaybillState_Id"] = rs["IntWaybillState_Id"];
                        newRow["IntWaybillState_Name"] = rs["IntWaybillState_Name"];

                        newRow["IntWaybillShipMode_Guid"] = rs["IntWaybillShipMode_Guid"];
                        newRow["IntWaybillShipMode_Id"] = rs["IntWaybillShipMode_Id"];
                        newRow["IntWaybillShipMode_Name"] = rs["IntWaybillShipMode_Name"];

                        newRow["IntWaybill_BeginDate"] = ((rs["IntWaybill_BeginDate"] != System.DBNull.Value) ? rs["IntWaybill_BeginDate"] : System.DBNull.Value);
                        newRow["IntWaybill_ShipDate"] = ((rs["IntWaybill_ShipDate"] != System.DBNull.Value) ? rs["IntWaybill_ShipDate"] : System.DBNull.Value);

                        newRow["IntWaybill_Num"] = rs["IntWaybill_Num"];
                        newRow["RetailWaybill_Num"] = rs["RetailWaybill_Num"];

                        newRow["IntWaybill_Description"] = rs["IntWaybill_Description"];

                        newRow["IntWaybill_AllPrice"] = rs["IntWaybill_AllPrice"];
                        newRow["IntWaybill_RetailAllPrice"] = rs["IntWaybill_RetailAllPrice"];
                        newRow["IntWaybill_AllDiscount"] = rs["IntWaybill_AllDiscount"];
                        newRow["IntWaybill_Quantity"] = System.Convert.ToDecimal(rs["IntWaybill_Quantity"]);

                        newRow["IntWaybill_ForStock"] = rs["IntWaybill_ForStock"];
                        newRow["IntWaybill_Send"] = rs["IntWaybill_Send"];

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
        /// Возвращает таблицу с приложением к накладной на внутреннее перемещение
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">объект "SQL-команда"</param>
        /// <param name="uuidIntWaybillId">уи накладной на внутреннее перемещение</param>
        /// <param name="strErr">строка с сообщением об ошибке</param>
        /// <param name="bTableFromIntOrder">признак "приложение запрашивается из заказа на внутреннее перемещение"</param>
        /// <returns>приложение к накладной в виде объекта класса DataTable</returns>
        public static System.Data.DataTable GetIntWaybilItemTable(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidIntWaybillId, ref System.String strErr, System.Boolean bTableFromIntOrder = false)
        {
            System.Data.DataTable dtReturn = new System.Data.DataTable();

            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybItem_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybillItem_Id", typeof(System.Int32)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderItem_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybill_Guid", typeof(System.Guid)));

            dtReturn.Columns.Add(new System.Data.DataColumn("Parts_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Measure_Guid", typeof(System.Guid)));

            dtReturn.Columns.Add(new System.Data.DataColumn("ProductOwnerName", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("PARTS_NAME", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("PARTS_ARTICLE", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Measure_ShortName", typeof(System.String)));

            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybItem_Quantity", typeof(System.Double)));

            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybItem_Price", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybItem_PriceImporter", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybItem_RetailPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybItem_Discount", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybItem_DiscountPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybItem_AllPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybItem_TotalPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybItem_RetailAllPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybItem_NDSPercent", typeof(System.Double)));

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

                if (bTableFromIntOrder == true)
                {
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetIntWaybItemsFromIntOrder]", objProfile.GetOptionsDllDBName());
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrder_Guid", System.Data.DbType.Guid));
                    cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                    cmd.Parameters["@IntOrder_Guid"].Value = uuidIntWaybillId;
                }
                else
                {
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetIntWaybItems]", objProfile.GetOptionsDllDBName());
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Waybill_Guid", System.Data.DbType.Guid));
                    cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                    cmd.Parameters["@IntWaybill_Guid"].Value = uuidIntWaybillId;
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

                        newRow["IntWaybItem_Guid"] = ((rs["IntWaybItem_Guid"] != System.DBNull.Value) ? (System.Guid)rs["IntWaybItem_Guid"] : System.Guid.Empty);
                        newRow["IntOrderItem_Guid"] = ((rs["IntOrderItem_Guid"] != System.DBNull.Value) ? (System.Guid)rs["IntOrderItem_Guid"] : System.Guid.Empty);
                        newRow["IntWaybill_Guid"] = ((rs["IntWaybill_Guid"] != System.DBNull.Value) ? (System.Guid)rs["IntWaybill_Guid"] : System.Guid.Empty);
                        newRow["IntWaybillItem_Id"] = ((rs["IntWaybillItem_Id"] != System.DBNull.Value) ? (System.Int32)rs["IntWaybillItem_Id"] : 0);
                        newRow["Measure_Guid"] = ((rs["Measure_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Measure_Guid"] : System.Guid.Empty);
                        newRow["Parts_Guid"] = ((rs["Parts_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Parts_Guid"] : System.Guid.Empty);

                        newRow["ProductOwnerName"] = ((rs["ProductOwnerName"] != System.DBNull.Value) ? System.Convert.ToString(rs["ProductOwnerName"]) : System.String.Empty);
                        newRow["PARTS_NAME"] = ((rs["PARTS_NAME"] != System.DBNull.Value) ? System.Convert.ToString(rs["PARTS_NAME"]) : System.String.Empty);
                        newRow["PARTS_ARTICLE"] = ((rs["PARTS_ARTICLE"] != System.DBNull.Value) ? System.Convert.ToString(rs["PARTS_ARTICLE"]) : System.String.Empty);
                        newRow["Measure_ShortName"] = ((rs["Measure_ShortName"] != System.DBNull.Value) ? System.Convert.ToString(rs["Measure_ShortName"]) : System.String.Empty);

                        newRow["IntWaybItem_Quantity"] = rs["IntWaybItem_Quantity"];

                        newRow["IntWaybItem_Price"] = rs["IntWaybItem_Price"];
                        newRow["IntWaybItem_PriceImporter"] = rs["IntWaybItem_PriceImporter"];
                        newRow["IntWaybItem_RetailPrice"] = rs["IntWaybItem_RetailPrice"];
                        newRow["IntWaybItem_Discount"] = rs["IntWaybItem_Discount"];
                        newRow["IntWaybItem_DiscountPrice"] = rs["IntWaybItem_DiscountPrice"];
                        newRow["IntWaybItem_NDSPercent"] = rs["IntWaybItem_NDSPercent"];

                        newRow["IntWaybItem_AllPrice"] = rs["IntWaybItem_AllPrice"];
                        newRow["IntWaybItem_TotalPrice"] = rs["IntWaybItem_TotalPrice"];
                        newRow["IntWaybItem_RetailAllPrice"] = rs["IntWaybItem_RetailAllPrice"];

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
                strErr += (String.Format("\nНе удалось получить таблицу с приложением к накладной на внутреннее перемещение.\nТекст ошибки: {0}", f.Message));
            }
            return dtReturn;
        }

        /// <summary>
        /// Возвращает таблицу с приложением к накладной на внутреннее перемещение
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">объект "SQL-команда"</param>
        /// <param name="uuidIntWaybillId">уи накладной на внутреннее перемещение</param>
        /// <param name="strErr">строка с сообщением об ошибке</param>
        /// <returns>приложение к накладной на внутреннее перемещение в виде объекта класса DataTable</returns>
        public static System.Data.DataTable GetIntWaybilItemTableForExportToExcel(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidIntWaybillId, ref System.String strErr)
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetIntWaybillProduction4StockFromIB]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntWaybill_Guid", System.Data.DbType.Guid));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@IntWaybill_Guid"].Value = uuidIntWaybillId;

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
                strErr += (String.Format("\nНе удалось получить таблицу с приложением к накладной на внутренее перемещение.\nТекст ошибки: {0}", f.Message));
            }
            return dtReturn;
        }

        #endregion


    }
}
