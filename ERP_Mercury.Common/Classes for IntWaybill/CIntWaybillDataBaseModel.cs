using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP_Mercury.Common
{
    public class CIntWaybillDataBaseModel
    {

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

                if (bTableFromIntOrder == true)
                {
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetIntWaybItemsFromIntOrder]", objProfile.GetOptionsDllDBName());
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Suppl_Guid", System.Data.DbType.Guid));
                    cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                    cmd.Parameters["@Suppl_Guid"].Value = uuidIntWaybillId;
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
