using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace ERP_Mercury.Common
{
    public class CIntOrderDataBaseModel
    {
        #region Журнал заказов на внутреннее перемещение

        /// <summary>
        /// Возвращает таблицу с заказами за указанный период
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="IntOrder_Guid">УИ документа</param>
        /// <param name="IntOrder_DateBegin">начало периода для выборки</param>
        /// <param name="IntOrder_DateEnd">конец периода для выборки</param>
        /// <param name="IntOrder_SrcCompanyGuid">УИ компании "Откуда"</param>
        /// <param name="IntOrder_SrcStockGuid">УИ склада "Откуда"<</param>
        /// <param name="IntOrder_DstCompanyGuid">УИ компании "Куда"</param>
        /// <param name="IntOrder_DstStockGuid">УИ склада "Куда"<</param>
        /// <param name="IntOrder_PaymentTypeGuid">УИ формы оплаты</param>
        /// <param name="strErr">текст ошибки</param>
        /// <param name="OnlyUnShippedOrders">признак "запрос только НЕ отгруженных заказов"</param>
        /// <returns>таблицу</returns>
        public static System.Data.DataTable GetIntOrderTable(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid IntOrder_Guid,
            System.DateTime IntOrder_DateBegin, System.DateTime IntOrder_DateEnd,
            System.Guid IntOrder_SrcCompanyGuid, System.Guid IntOrder_SrcStockGuid,
            System.Guid IntOrder_DstCompanyGuid, System.Guid IntOrder_DstStockGuid,
            System.Guid IntOrder_PaymentTypeGuid, ref System.String strErr,
            System.Boolean OnlyUnShippedOrders = false
            )
        {
            System.Data.DataTable dtReturn = new System.Data.DataTable();


            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrder_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrder_Id", typeof(System.Int32)));

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

            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderShipMode_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderShipMode_Id", typeof(System.Int32)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderShipMode_Name", typeof(System.String)));

            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderState_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderState_Id", typeof(System.Int32)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderState_Name", typeof(System.String)));

            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrder_Num", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderParent_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrder_BeginDate", typeof(System.DateTime)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrder_ShipDate", typeof(System.DateTime)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrder_Description", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrder_CurrencyRate", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrder_AllPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrder_RetailAllPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrder_AllDiscount", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrder_TotalPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrder_Quantity", typeof(System.Double)));

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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetIntOrderList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrder_DateBegin", System.Data.DbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrder_DateEnd", System.Data.DbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OnlyUnShippedOrders", System.Data.DbType.Guid));
                cmd.Parameters["@IntOrder_DateBegin"].Value = IntOrder_DateBegin;
                cmd.Parameters["@IntOrder_DateEnd"].Value = IntOrder_DateEnd;
                cmd.Parameters["@OnlyUnShippedOrders"].Value = OnlyUnShippedOrders;

                if (IntOrder_Guid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrder_Guid", System.Data.DbType.Guid));
                    cmd.Parameters["@IntOrder_Guid"].Value = IntOrder_Guid;
                }

                if (IntOrder_SrcCompanyGuid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrder_SrcCompanyGuid", System.Data.DbType.Guid));
                    cmd.Parameters["@IntOrder_SrcCompanyGuid"].Value = IntOrder_SrcCompanyGuid;
                }
                if (IntOrder_SrcStockGuid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrder_SrcStockGuid", System.Data.DbType.Guid));
                    cmd.Parameters["@IntOrder_SrcStockGuid"].Value = IntOrder_SrcStockGuid;
                }

                if (IntOrder_DstCompanyGuid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrder_DstCompanyGuid", System.Data.DbType.Guid));
                    cmd.Parameters["@IntOrder_DstCompanyGuid"].Value = IntOrder_DstCompanyGuid;
                }
                if (IntOrder_DstStockGuid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrder_DstStockGuid", System.Data.DbType.Guid));
                    cmd.Parameters["@IntOrder_DstStockGuid"].Value = IntOrder_DstStockGuid;
                }

                if (IntOrder_PaymentTypeGuid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrder_PaymentTypeGuid", System.Data.DbType.Guid));
                    cmd.Parameters["@IntOrder_PaymentTypeGuid"].Value = IntOrder_PaymentTypeGuid;
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
                        newRow["IntOrder_Guid"] = ((rs["IntOrder_Guid"] != System.DBNull.Value) ? (System.Guid)rs["IntOrder_Guid"] : System.Guid.Empty);
                        newRow["IntOrderParent_Guid"] = ((rs["IntOrderParent_Guid"] != System.DBNull.Value) ? (System.Guid)rs["IntOrderParent_Guid"] : System.Guid.Empty);
                        newRow["IntOrder_Id"] = ((rs["IntOrder_Id"] != System.DBNull.Value) ? (System.Int32)rs["IntOrder_Id"] : 0);

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

                        newRow["IntOrderState_Guid"] = rs["IntOrderState_Guid"];
                        newRow["IntOrderState_Id"] = rs["IntOrderState_Id"];
                        newRow["IntOrderState_Name"] = rs["IntOrderState_Name"];

                        newRow["IntOrderShipMode_Guid"] = rs["IntOrderShipMode_Guid"];
                        newRow["IntOrderShipMode_Id"] = rs["IntOrderShipMode_Id"];
                        newRow["IntOrderShipMode_Name"] = rs["IntOrderShipMode_Name"];

                        newRow["IntOrder_BeginDate"] = ((rs["IntOrder_BeginDate"] != System.DBNull.Value) ? rs["IntOrder_BeginDate"] : System.DBNull.Value);
                        newRow["IntOrder_ShipDate"] = ((rs["IntOrder_ShipDate"] != System.DBNull.Value) ? rs["IntOrder_ShipDate"] : System.DBNull.Value);

                        newRow["IntOrder_Num"] = rs["IntOrder_Num"];

                        newRow["IntOrder_Description"] = rs["IntOrder_Description"];

                        newRow["IntOrder_AllPrice"] = rs["IntOrder_AllPrice"];
                        newRow["IntOrder_RetailAllPrice"] = rs["IntOrder_RetailAllPrice"];
                        newRow["IntOrder_AllDiscount"] = rs["IntOrder_AllDiscount"];
                        newRow["IntOrder_Quantity"] = System.Convert.ToDecimal(rs["IntOrder_Quantity"]);

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
                strErr += (String.Format("\nНе удалось получить таблицу с заказами на внутреннее перемещение.\nТекст ошибки: {0}", f.Message));
            }
            return dtReturn;
        }
        #endregion

        #region Приложение к заказу на внутреннее перемещение
        /// <summary>
        /// Возвращает таблицу с приложением к заказу на внутреннее перемещение
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">объект "SQL-команда"</param>
        /// <param name="uuidIntOrderId">уи заказа на внутреннее перемещение</param>
        /// <param name="strErr">строка с сообщением об ошибке</param>
        /// <returns>приложение к накладной в виде объекта класса DataTable</returns>
        public static System.Data.DataTable GetIntOrderItemTable( UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidIntOrderId, 
            ref System.String strErr )
        {
            System.Data.DataTable dtReturn = new System.Data.DataTable();

            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderItem_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderItem_Id", typeof(System.Int32)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrder_Guid", typeof(System.Guid)));

            dtReturn.Columns.Add(new System.Data.DataColumn("Parts_Guid", typeof(System.Guid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Parts_Id", typeof(System.Int32)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Measure_Guid", typeof(System.Guid)));

            dtReturn.Columns.Add(new System.Data.DataColumn("ProductOwnerName", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("PARTS_NAME", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("PARTS_ARTICLE", typeof(System.String)));
            dtReturn.Columns.Add(new System.Data.DataColumn("Measure_ShortName", typeof(System.String)));

            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderItem_Quantity", typeof(System.Double)));

            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderItem_Price", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderItem_PriceImporter", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderItem_RetailPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderItem_Discount", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderItem_DiscountPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderItem_AllPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderItem_TotalPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderItem_RetailAllPrice", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderItem_NDSPercent", typeof(System.Double)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderItem_MarkUpPercent", typeof(System.Double)));

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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetIntOrderItems]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrder_Guid", System.Data.DbType.Guid));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@IntOrder_Guid"].Value = uuidIntOrderId;

                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                System.Int32 iRecordCount = 0;
                if (rs.HasRows)
                {
                    System.Data.DataRow newRow = null;
                    while (rs.Read())
                    {
                        iRecordCount++;

                        newRow = dtReturn.NewRow();

                        newRow["IntOrderItem_Guid"] = ((rs["IntOrderItem_Guid"] != System.DBNull.Value) ? (System.Guid)rs["IntOrderItem_Guid"] : System.Guid.Empty);
                        newRow["IntOrder_Guid"] = ((rs["IntOrder_Guid"] != System.DBNull.Value) ? (System.Guid)rs["IntOrder_Guid"] : System.Guid.Empty);
                        newRow["IntOrderItem_Id"] = ((rs["IntOrderItem_Id"] != System.DBNull.Value) ? (System.Int32)rs["IntOrderItem_Id"] : 0);
                        newRow["Measure_Guid"] = ((rs["Measure_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Measure_Guid"] : System.Guid.Empty);
                        newRow["Parts_Guid"] = ((rs["Parts_Guid"] != System.DBNull.Value) ? (System.Guid)rs["Parts_Guid"] : System.Guid.Empty);
                        newRow["Parts_Id"] = ( (rs["Parts_Id"] != System.DBNull.Value) ? System.Convert.ToInt32(rs["Parts_Id"]) : 0 );

                        newRow["ProductOwnerName"] = ((rs["ProductOwnerName"] != System.DBNull.Value) ? System.Convert.ToString(rs["ProductOwnerName"]) : System.String.Empty);
                        newRow["PARTS_NAME"] = ((rs["PARTS_NAME"] != System.DBNull.Value) ? System.Convert.ToString(rs["PARTS_NAME"]) : System.String.Empty);
                        newRow["PARTS_ARTICLE"] = ((rs["PARTS_ARTICLE"] != System.DBNull.Value) ? System.Convert.ToString(rs["PARTS_ARTICLE"]) : System.String.Empty);
                        newRow["Measure_ShortName"] = ((rs["Measure_ShortName"] != System.DBNull.Value) ? System.Convert.ToString(rs["Measure_ShortName"]) : System.String.Empty);

                        newRow["IntOrderItem_Quantity"] = rs["IntOrderItem_Quantity"];

                        newRow["IntOrderItem_Price"] = rs["IntOrderItem_Price"];
                        newRow["IntOrderItem_PriceImporter"] = rs["IntOrderItem_PriceImporter"];
                        newRow["IntOrderItem_RetailPrice"] = rs["IntOrderItem_RetailPrice"];
                        newRow["IntOrderItem_Discount"] = rs["IntOrderItem_Discount"];
                        newRow["IntOrderItem_DiscountPrice"] = rs["IntOrderItem_DiscountPrice"];
                        newRow["IntOrderItem_NDSPercent"] = rs["IntOrderItem_NDSPercent"];
                        newRow["IntOrderItem_MarkUpPercent"] = rs["IntOrderItem_MarkUpPercent"];
                        newRow["IntOrderItem_AllPrice"] = rs["IntOrderItem_AllPrice"];
                        newRow["IntOrderItem_TotalPrice"] = rs["IntOrderItem_TotalPrice"];
                        newRow["IntOrderItem_RetailAllPrice"] = rs["IntOrderItem_RetailAllPrice"];

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
                strErr += (String.Format("\nНе удалось получить таблицу с приложением к заказу на внутреннее перемещение.\nТекст ошибки: {0}", f.Message));
            }
            return dtReturn;
        }

        #endregion

        #region Аннулирование заказа
        /// <summary>
        /// Аннулирование заказа
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="IntOrder_Guid">УИ заказа</param>
        /// <param name="IntOrderState_Guid">УИ состояния заказа</param>
        /// <param name="strErr">текст ошибки</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean CancelIntOrder(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid IntOrder_Guid, ref System.Guid IntOrderState_Guid, ref System.String strErr)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_CancelIntOrder]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrder_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderState_Guid", System.Data.SqlDbType.UniqueIdentifier) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });

                cmd.Parameters["@Waybill_Guid"].Value = IntOrder_Guid;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                strErr += (System.Convert.ToString(cmd.Parameters["@ERROR_MES"].Value));

                if (iRes == 0)
                {
                    IntOrderState_Guid = (System.Guid)cmd.Parameters["@IntOrderState_Guid"].Value;

                    strErr = "Заказ на внутреннее перемещение аннулирован.";
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

        #region Сохранение заказа в БД
        /// <summary>
        /// Добавляет заказ в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
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
        public static System.Boolean AddNewDocToDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.DateTime Doc_BeginDate, System.String Doc_Num, System.DateTime Doc_ShipDate,
            System.Guid DocShipMode_Guid, System.Guid PaymentType_Guid, 
            System.Guid StockSrc_Guid, System.Guid StockDst_Guid,
            System.Guid Depart_Guid, System.Guid Salesman_Guid, 
            System.String Doc_Description, System.Guid DocParent_Guid,
            System.Data.DataTable DocTablePart, ref System.String strErr,
            ref System.Guid Doc_Guid, ref System.Int32 Doc_Id, ref System.Guid DocState_Guid, 
            System.Boolean DocumentSendToStock = false)
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
                        strErr += ("Не удалось получить соединение с базой данных.");
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddIntOrder]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrder_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrder_Id", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderState_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderShipMode_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrder_BeginDate", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Depart_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Salesman_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PaymentType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrder_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrder_Num", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SrcStock_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DstStock_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.AddWithValue("@tIntOrderItms", DocTablePart);
                cmd.Parameters["@tIntOrderItms"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tIntOrderItms"].TypeName = "dbo.udt_IntOrderItms";

                if (DocParent_Guid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderParent_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@IntOrderParent_Guid"].Value = DocParent_Guid;
                }
                if (System.DateTime.Compare(Doc_ShipDate, System.DateTime.MinValue) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrder_ShipDate", System.Data.SqlDbType.Date));
                    cmd.Parameters["@IntOrder_ShipDate"].Value = Doc_ShipDate;
                }

                cmd.Parameters["@IntOrder_BeginDate"].Value = Doc_BeginDate;
                cmd.Parameters["@Depart_Guid"].Value = Depart_Guid;
                cmd.Parameters["@Salesman_Guid"].Value = Salesman_Guid;
                cmd.Parameters["@SrcStock_Guid"].Value = StockSrc_Guid;
                cmd.Parameters["@DstStock_Guid"].Value = StockDst_Guid;
                cmd.Parameters["@IntOrderShipMode_Guid"].Value = DocShipMode_Guid;
                cmd.Parameters["@PaymentType_Guid"].Value = PaymentType_Guid;
                cmd.Parameters["@IntOrder_Description"].Value = Doc_Description;
                cmd.Parameters["@IntOrder_Num"].Value = Doc_Num;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                strErr += (System.Convert.ToString(cmd.Parameters["@ERROR_MES"].Value));

                if (iRes == 0)
                {
                    Doc_Guid = (System.Guid)cmd.Parameters["@IntOrder_Guid"].Value;
                    Doc_Id = (System.Int32)cmd.Parameters["@IntOrder_Id"].Value;
                    DocState_Guid = (System.Guid)cmd.Parameters["@IntOrderState_Guid"].Value;

                    strErr = "Заказ успешно сохранен.";
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

        /// <summary>
        /// Редактирует реквизиты заказа в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
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
        public static System.Boolean EditDocInDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditIntOrder]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrder_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderState_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderShipMode_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrder_BeginDate", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Depart_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Salesman_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PaymentType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrder_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrder_Num", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SrcStock_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DstStock_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.AddWithValue("@tIntOrderItms", DocTablePart);
                cmd.Parameters["@tIntOrderItms"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tIntOrderItms"].TypeName = "dbo.udt_IntOrderItms";

                if (DocParent_Guid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderParent_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@IntOrderParent_Guid"].Value = DocParent_Guid;
                }
                if (System.DateTime.Compare(Doc_ShipDate, System.DateTime.MinValue) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrder_ShipDate", System.Data.SqlDbType.Date));
                    cmd.Parameters["@IntOrder_ShipDate"].Value = Doc_ShipDate;
                }

                cmd.Parameters["@IntOrder_Guid"].Value = Doc_Guid;
                cmd.Parameters["@IntOrderState_Guid"].Value = DocState_Guid;
                cmd.Parameters["@IntOrder_BeginDate"].Value = Doc_BeginDate;
                cmd.Parameters["@Depart_Guid"].Value = Depart_Guid;
                cmd.Parameters["@Salesman_Guid"].Value = Salesman_Guid;
                cmd.Parameters["@SrcStock_Guid"].Value = StockSrc_Guid;
                cmd.Parameters["@DstStock_Guid"].Value = StockDst_Guid;
                cmd.Parameters["@IntOrderShipMode_Guid"].Value = DocShipMode_Guid;
                cmd.Parameters["@PaymentType_Guid"].Value = PaymentType_Guid;
                cmd.Parameters["@IntOrder_Description"].Value = Doc_Description;
                cmd.Parameters["@IntOrder_Num"].Value = Doc_Num;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                strErr += (System.Convert.ToString(cmd.Parameters["@ERROR_MES"].Value));

                if (iRes == 0)
                {
                    strErr = "Заказ успешно сохранен.";
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

        #region Цена для позиции в заказе
        public static System.Boolean GetPriceForOrderItem(UniXP.Common.CProfile objProfile, System.Guid uuidPartsId,
            System.Guid uuidStockSrcId, System.Guid uuidPartsubtypePriceTypeId, System.Double dblDiscountPercent, 
            ref System.Double PriceImporter, ref System.Double Price, ref System.Double PriceWithDiscount,
            ref System.Double PriceRetail, ref System.Double NDSPercent, ref System.Double MarkUpPercent, ref System.String strErr,
            System.Double PriceInput = 0, System.Boolean InputPriceIsFixed = false
            )
        {
            System.Boolean bRet = false;

            PriceImporter = 0;
            Price = 0;
            PriceWithDiscount = 0;
            PriceRetail = 0;
            NDSPercent = 0;
            MarkUpPercent = 0;

            bRet = true;

            // подключаемся к БД
            System.Data.SqlClient.SqlConnection DBConnection = objProfile.GetDBSource();
            if (DBConnection == null)
            {
                strErr += ("Отсутствует соединение с БД.");
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPriceForIntOrder]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsubtypePriceType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DiscountPercent", System.Data.SqlDbType.Decimal));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceInput", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@InputPriceIsFixed", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@NDSPercent", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@MarkUpPercent", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceImporter", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Price", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceWithDiscount", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PriceRetail", System.Data.SqlDbType.Money));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@NDSPercent"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@MarkUpPercent"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@PriceImporter"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Price"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@PriceWithDiscount"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@PriceRetail"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Parts_Guid"].Value = uuidPartsId;
                cmd.Parameters["@Stock_Guid"].Value = uuidStockSrcId;
                cmd.Parameters["@PartsubtypePriceType_Guid"].Value = uuidPartsubtypePriceTypeId;
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
                    PriceRetail = System.Convert.ToDouble(cmd.Parameters["@PriceRetail"].Value);
                    NDSPercent = System.Convert.ToDouble(cmd.Parameters["@NDSPercent"].Value);
                    MarkUpPercent = System.Convert.ToDouble(cmd.Parameters["@MarkUpPercent"].Value);

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
                strErr += (String.Format("Не удалось получить цены для позиции. Текст ошибки: {0}", f.Message));
            }

            return bRet;
        }
        #endregion

    }
}
