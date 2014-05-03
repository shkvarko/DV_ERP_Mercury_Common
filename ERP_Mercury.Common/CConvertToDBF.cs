using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace ERP_Mercury.Common
{
    public class CConvertToDBF
    {

        public static void GetFileNameAndPath(string completePath, ref string fileName, ref string folderPath)
        {
            string[] fileSep = completePath.Split('\\');
            for (int iCount = 0; iCount < fileSep.Length; iCount++)
            {
                if (iCount == fileSep.Length - 2)
                {
                    if (fileSep.Length == 2)
                    {
                        folderPath += fileSep[iCount] + "\\";
                    }
                    else
                    {
                        folderPath += fileSep[iCount];
                    }
                }
                else
                {
                    if (fileSep[iCount].IndexOf(".") > 0)
                    {
                        fileName = fileSep[iCount];
                        fileName = fileName.Substring(0, fileName.IndexOf("."));
                    }
                    else
                    {
                        folderPath += fileSep[iCount] + "\\";
                    }
                }
            }
        }

        // This function takes Dataset (to be exported) and filePath as input parameter and return // bool status as output parameter
        // comments are written inside the function to describe the functionality
        public static bool EхportDBF(DataSet dsExport, string filePath)
        {
            string tableName = string.Empty;
            string folderPath = string.Empty;
            bool returnStatus = false;
            try
            {
                // This function give the Folder name and table name to use in 
                // the connection string and create table statement.
                GetFileNameAndPath(filePath, ref tableName, ref folderPath);
                // here you can use DBASE IV also 
                string jetOleDbConString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source={0}; Extended Properties=dBASE IV";
                string connString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + folderPath + "; Extended Properties=DBASE III;";
                string createStatement = "Create Table " + tableName + " ( ";
                string insertStatement = "Insert Into " + tableName + " Values ( ";

                string insertTemp = string.Empty;
                OleDbCommand cmd = new OleDbCommand();

                OleDbConnection conn = new System.Data.OleDb.OleDbConnection();
                conn.ConnectionString = String.Format(jetOleDbConString, folderPath);
                System.IO.File.Delete(System.IO.Path.Combine(folderPath, "Parts.dbf"));

                if (dsExport.Tables[0].Columns.Count <= 0) { throw new Exception(); }
                // This for loop to create "Create table statement" for DBF
                // Here I am creating varchar(250) datatype for all column.
                // for formatting If you don't have to format data before 
                // export then you can make a clone of dsExport data and transfer // data in to that no need to add datatable, datarow and 
                // datacolumn in the code.
                for (int iCol = 0; iCol < dsExport.Tables[0].Columns.Count; iCol++)
                {
                    createStatement += dsExport.Tables[0].Columns[iCol].ColumnName.ToString();
                    if (iCol == dsExport.Tables[0].Columns.Count - 1)
                    {
                        createStatement += " varchar(250) )";
                    }
                    else
                    {
                        createStatement += " varchar(250), ";
                    }
                }
                //Create Temp Dateset 
                DataSet dsCreateTable = new DataSet();
                //Open the connection 
                conn.Open();
                //Create the DBF table
                DataSet dsFill = new DataSet();
                OleDbDataAdapter daInsertTable = new OleDbDataAdapter(createStatement, conn);
                daInsertTable.Fill(dsFill);
                //Adding One DataTable into the dsCreatedTable dataset
                DataTable dt = new DataTable();
                dsCreateTable.Tables.Add(dt);
                System.String strAddValue = "";
                for (int row = 0; row < dsExport.Tables[0].Rows.Count; row++)
                {
                    insertTemp = insertStatement;
                    //Adding Rows to the dsCreatedTable dataset
                    DataRow dr = dsCreateTable.Tables[0].NewRow();
                    dsCreateTable.Tables[0].Rows.Add(dr);
                    for (int col = 0; col < dsExport.Tables[0].Columns.Count; col++)
                    {
                        if (row == 0)
                        {
                            //Adding Columns to the dsCreatedTable dataset
                            DataColumn dc = new DataColumn();
                            dsCreateTable.Tables[0].Columns.Add(dc);
                        }
                        // Remove Special character if any like dot,semicolon,colon,comma // etc
                        dsExport.Tables[0].Rows[row][col].ToString().Replace("LF", "");
                        // do the formating if you want like modify the Date symbol , //thousand saperator etc.
                        strAddValue = dsExport.Tables[0].Rows[row][col].ToString().Trim();
                        strAddValue = strAddValue.Replace("'", "''");
                        dsCreateTable.Tables[0].Rows[row][col] = strAddValue;// dsExport.Tables[0].Rows[row][col].ToString().Trim();

                        // Create Insert Statement
                        if (col == dsExport.Tables[0].Columns.Count - 1)
                        {
                            insertTemp += "'" + dsCreateTable.Tables[0].Rows[row][col] + "' ) ;";
                        }
                        else
                        {
                            insertTemp += "'" + dsCreateTable.Tables[0].Rows[row][col] + "' , ";
                        }

                    } // inner for loop close
                    // This lines of code insert Row One by one to above created 
                    // datatable.
                    //        insertTemp.Replace("'", "''");
                    daInsertTable = new OleDbDataAdapter(insertTemp, conn);
                    daInsertTable.Fill(dsFill);
                } // close outer for loop
                MessageBox.Show("Exported done Successfully to DBF File.");
                returnStatus = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не произвести экспорт данных в файл dbf.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return returnStatus;
        } // close function

        public static void ExportDirectoriesToFileDBF(UniXP.Common.CProfile objProfile, string folderPath,  UniXP.Common.MENUITEM objMenuItem)
        {
            try
            {
                // ВТМ
                List<CProductVtm> objProductVtmList = CProductVtm.GetProductVtmList(objProfile, null);
                EхportVTMDBF(objProductVtmList, folderPath);
                objProductVtmList = null;
                objMenuItem.SimulateNewMessage("экспортирован справочник ВТМ");

                // Товарные марки
                List<CProductTradeMark> objProductTradeMarkList = CProductTradeMark.GetProductTradeMarkList(objProfile, null);
                EхportOWNERToDBF(objProductTradeMarkList, folderPath);
                objProductTradeMarkList = null;
                objMenuItem.SimulateNewMessage("экспортирован справочник товарных марок");

                // Товарные группы
                EхportPARTTYPEToDBF(objProfile, null, folderPath);
                objMenuItem.SimulateNewMessage("экспортирован справочник Товарных групп");

                // Товарные подгруппы
                EхportPARTSUBTYPEToDBF(objProfile, null, folderPath);
                objMenuItem.SimulateNewMessage("экспортирован справочник Товарных подгрупп");

                // Единицы измерения
                EхportMEASUREToDBF(objProfile, null, folderPath);
                objMenuItem.SimulateNewMessage("экспортирован справочник Единиц измерения");

                // Товары
                EхportPARTSToDBF(objProfile, null, folderPath);
                objMenuItem.SimulateNewMessage("экспортирован справочник Товаров");

                // Штрих-коды
                //EхportBARCODEToDBF(objProfile, null, folderPath);

                // Цены
                EхportPRICEToDBF(objProfile, null, folderPath);
                objMenuItem.SimulateNewMessage("экспортирован справочник Цен");

                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Экспорт данных завершен.", "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не произвести экспорт данных в файл dbf.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                
            }
            return;
        }

        public static bool EхportPartsDBF(List<CProduct> objProductList, string folderPath)
        {
            string tableName = string.Empty;
            bool returnStatus = false;
            try
            {
                System.IO.File.Delete(System.IO.Path.Combine(folderPath, "PARTS.DBF"));

                string jetOleDbConString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source={0}; Extended Properties=dBASE IV";
                OleDbConnection conn = new System.Data.OleDb.OleDbConnection();
                conn.ConnectionString = String.Format(jetOleDbConString, folderPath);

                System.Data.OleDb.OleDbCommand oleDbCommandCreateTable = new System.Data.OleDb.OleDbCommand();
                System.Data.OleDb.OleDbCommand oleDbJetInsertCommand = new System.Data.OleDb.OleDbCommand();
                oleDbCommandCreateTable.Connection = conn;
                oleDbJetInsertCommand.Connection = conn;

                oleDbCommandCreateTable.CommandText = "CREATE TABLE PARTS (PARTS_Id Integer, PARTS_NAME CHAR (74))";

                conn.Open();

                oleDbCommandCreateTable.ExecuteNonQuery();

                oleDbJetInsertCommand.CommandText = "INSERT INTO PARTS (PARTS_Id, PARTS_NAME) VALUES (?, ?)";
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("PARTS_Id", System.Data.OleDb.OleDbType.Integer, 0, "PARTS_Id"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("PARTS_NAME", System.Data.OleDb.OleDbType.VarWChar, 74, "PARTS_NAME"));

                foreach (CProduct obProduct in objProductList)
                {
                    oleDbJetInsertCommand.Parameters["PARTS_Id"].Value = System.Convert.ToInt32(obProduct.ID_Ib);
                    oleDbJetInsertCommand.Parameters["PARTS_NAME"].Value = obProduct.ProductFullName;

                    oleDbJetInsertCommand.ExecuteNonQuery();
                }

                conn.Close();
                returnStatus = true;

                //                MessageBox.Show("Exported done Successfully to DBF File.");
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не произвести экспорт данных в файл dbf.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return returnStatus;
        } // close function

        /// <summary>
        /// Экспорт данных справочника ВТМ
        /// </summary>
        /// <param name="objProductVtmList">список ВТМ</param>
        /// <param name="folderPath">каталог</param>
        /// <returns>TRUE - удачное завершение операции; FALSE - ошибка</returns>
        public static bool EхportVTMDBF(List<CProductVtm> objProductVtmList, string folderPath)
        {
            string tableName = string.Empty;
            bool returnStatus = false;
            try
            {
                System.IO.File.Delete(System.IO.Path.Combine(folderPath, "VTM.DBF"));

                string jetOleDbConString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source={0}; Extended Properties=dBASE IV";
                OleDbConnection conn = new System.Data.OleDb.OleDbConnection();
                conn.ConnectionString = String.Format(jetOleDbConString, folderPath);

                System.Data.OleDb.OleDbCommand oleDbCommandCreateTable = new System.Data.OleDb.OleDbCommand();
                System.Data.OleDb.OleDbCommand oleDbJetInsertCommand = new System.Data.OleDb.OleDbCommand();
                oleDbCommandCreateTable.Connection = conn;
                oleDbJetInsertCommand.Connection = conn;

                oleDbCommandCreateTable.CommandText = "CREATE TABLE VTM (VTM_ID Integer, VTM_NAME CHAR (52), VTM_SNAME CHAR (48), VTM_NACT Integer )";

                conn.Open();

                oleDbCommandCreateTable.ExecuteNonQuery();

                oleDbJetInsertCommand.CommandText = "INSERT INTO VTM (VTM_ID, VTM_NAME, VTM_SNAME, VTM_NACT) VALUES (?, ?, ?, ?)";
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("VTM_ID", System.Data.OleDb.OleDbType.Integer, 0, "VTM_ID"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("VTM_NAME", System.Data.OleDb.OleDbType.VarWChar, 52, "VTM_NAME"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("VTM_SNAME", System.Data.OleDb.OleDbType.VarWChar, 48, "VTM_SNAME"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("VTM_NACT", System.Data.OleDb.OleDbType.Integer, 0, "VTM_NACT"));

                foreach (CProductVtm obItem in objProductVtmList)
                {
                    oleDbJetInsertCommand.Parameters["VTM_ID"].Value = System.Convert.ToInt32(obItem.ID_Ib);
                    oleDbJetInsertCommand.Parameters["VTM_NAME"].Value = obItem.Name;
                    oleDbJetInsertCommand.Parameters["VTM_SNAME"].Value = obItem.ShortName;
                    oleDbJetInsertCommand.Parameters["VTM_NACT"].Value = ((obItem.IsActive == true) ? 0 : 1);

                    oleDbJetInsertCommand.ExecuteNonQuery();
                }

                conn.Close();
                returnStatus = true;

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось произвести экспорт справочника ВТМ в файл dbf.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return returnStatus;
        } // close function

        /// <summary>
        /// Экспорт данных справочника ВТМ
        /// </summary>
        /// <param name="objProductVtmList">список ВТМ</param>
        /// <param name="folderPath">каталог</param>
        /// <returns>TRUE - удачное завершение операции; FALSE - ошибка</returns>
        public static bool EхportOWNERToDBF(List<CProductTradeMark> objProductTradeMarkList, string folderPath)
        {
            string tableName = string.Empty;
            bool returnStatus = false;
            try
            {
                System.IO.File.Delete(System.IO.Path.Combine(folderPath, "OWNER.DBF"));

                string jetOleDbConString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source={0}; Extended Properties=dBASE IV";
                OleDbConnection conn = new System.Data.OleDb.OleDbConnection();
                conn.ConnectionString = String.Format(jetOleDbConString, folderPath);

                System.Data.OleDb.OleDbCommand oleDbCommandCreateTable = new System.Data.OleDb.OleDbCommand();
                System.Data.OleDb.OleDbCommand oleDbJetInsertCommand = new System.Data.OleDb.OleDbCommand();
                oleDbCommandCreateTable.Connection = conn;
                oleDbJetInsertCommand.Connection = conn;

                oleDbCommandCreateTable.CommandText = "CREATE TABLE OWNER (VTM_ID Integer, OWNER_ID Integer, NAME CHAR (52) )";

                conn.Open();

                oleDbCommandCreateTable.ExecuteNonQuery();

                oleDbJetInsertCommand.CommandText = "INSERT INTO OWNER (VTM_ID, OWNER_ID, NAME) VALUES (?, ?, ?)";
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("VTM_ID", System.Data.OleDb.OleDbType.Integer, 0, "VTM_ID"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("OWNER_ID", System.Data.OleDb.OleDbType.Integer, 0, "OWNER_ID"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("NAME", System.Data.OleDb.OleDbType.VarWChar, 52, "NAME"));

                foreach (CProductTradeMark obItem in objProductTradeMarkList)
                {
                    oleDbJetInsertCommand.Parameters["VTM_ID"].Value = obItem.ProductVtm.ID_Ib;
                    oleDbJetInsertCommand.Parameters["OWNER_ID"].Value = System.Convert.ToInt32(obItem.ID_Ib);
                    oleDbJetInsertCommand.Parameters["NAME"].Value = obItem.Name;

                    oleDbJetInsertCommand.ExecuteNonQuery();
                }

                conn.Close();
                returnStatus = true;

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось произвести экспорт справочника товарных марок в файл dbf.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return returnStatus;
        } // close function

        /// <summary>
        /// Экспорт данных справочника товарных групп
        /// </summary>
        /// <param name="objProductVtmList">список товарных групп</param>
        /// <param name="folderPath">каталог</param>
        /// <returns>TRUE - удачное завершение операции; FALSE - ошибка</returns>
        public static bool EхportPARTTYPEToDBF(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, string folderPath)
        {
            string tableName = string.Empty;
            bool returnStatus = false;
            try
            {
                System.IO.File.Delete(System.IO.Path.Combine(folderPath, "TYPE.DBF"));

                string jetOleDbConString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source={0}; Extended Properties=dBASE IV";
                OleDbConnection conn = new System.Data.OleDb.OleDbConnection();
                conn.ConnectionString = String.Format(jetOleDbConString, folderPath);

                System.Data.OleDb.OleDbCommand oleDbCommandCreateTable = new System.Data.OleDb.OleDbCommand();
                System.Data.OleDb.OleDbCommand oleDbJetInsertCommand = new System.Data.OleDb.OleDbCommand();
                oleDbCommandCreateTable.Connection = conn;
                oleDbJetInsertCommand.Connection = conn;

                oleDbCommandCreateTable.CommandText = "CREATE TABLE TYPE (TYPE_ID Integer, NAME CHAR (52), SHORTNAME CHAR (24) )";

                conn.Open();

                oleDbCommandCreateTable.ExecuteNonQuery();

                oleDbJetInsertCommand.CommandText = "INSERT INTO TYPE ( TYPE_ID, NAME, SHORTNAME ) VALUES (?, ?, ?)";
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("TYPE_ID", System.Data.OleDb.OleDbType.Integer, 0, "TYPE_ID"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("NAME", System.Data.OleDb.OleDbType.VarWChar, 52, "NAME"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("SHORTNAME", System.Data.OleDb.OleDbType.VarWChar, 24, "SHORTNAME"));

                System.Data.SqlClient.SqlConnection DBConnection = null;
                System.Data.SqlClient.SqlCommand cmd = null;
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                            "Не удалось получить соединение с базой данных.", "Внимание",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return returnStatus;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartTypeListFromIBForExport]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        oleDbJetInsertCommand.Parameters["SHORTNAME"].Value = ((System.Convert.ToString(rs["PARTTYPE_SHORTNAME"]).Length > 52) ? (System.Convert.ToString(rs["PARTTYPE_SHORTNAME"]).Substring(0, 52)) : (System.Convert.ToString(rs["PARTTYPE_SHORTNAME"])));
                        oleDbJetInsertCommand.Parameters["NAME"].Value = ((System.Convert.ToString(rs["PARTTYPE_FULLNAME"]).Length > 24) ? (System.Convert.ToString(rs["PARTTYPE_FULLNAME"]).Substring(0, 24)) : (System.Convert.ToString(rs["PARTTYPE_FULLNAME"])));
                        oleDbJetInsertCommand.Parameters["TYPE_ID"].Value = System.Convert.ToInt32(rs["PARTTYPE_ID"]);

                        oleDbJetInsertCommand.ExecuteNonQuery();
                    }
                }
                rs.Dispose();

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }

                conn.Close();
                returnStatus = true;

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось произвести экспорт справочника товарных групп в файл dbf.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return returnStatus;
        } // close function

        /// <summary>
        /// Экспорт данных справочника товарных подгрупп
        /// </summary>
        /// <param name="objProductVtmList">список товарных групп</param>
        /// <param name="folderPath">каталог</param>
        /// <returns>TRUE - удачное завершение операции; FALSE - ошибка</returns>
        public static bool EхportPARTSUBTYPEToDBF(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, string folderPath)
        {
            string tableName = string.Empty;
            bool returnStatus = false;
            try
            {
                System.IO.File.Delete(System.IO.Path.Combine(folderPath, "SUBTYPE.DBF"));

                string jetOleDbConString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source={0}; Extended Properties=dBASE IV";
                OleDbConnection conn = new System.Data.OleDb.OleDbConnection();
                conn.ConnectionString = String.Format(jetOleDbConString, folderPath);

                System.Data.OleDb.OleDbCommand oleDbCommandCreateTable = new System.Data.OleDb.OleDbCommand();
                System.Data.OleDb.OleDbCommand oleDbJetInsertCommand = new System.Data.OleDb.OleDbCommand();
                oleDbCommandCreateTable.Connection = conn;
                oleDbJetInsertCommand.Connection = conn;

                oleDbCommandCreateTable.CommandText = "CREATE TABLE SUBTYPE (SUBTYPE_ID Integer, NAME CHAR (48) )";

                conn.Open();

                oleDbCommandCreateTable.ExecuteNonQuery();

                oleDbJetInsertCommand.CommandText = "INSERT INTO SUBTYPE ( SUBTYPE_ID, NAME ) VALUES (?, ?)";
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("SUBTYPE_ID", System.Data.OleDb.OleDbType.Integer, 0, "SUBTYPE_ID"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("NAME", System.Data.OleDb.OleDbType.VarWChar, 48, "NAME"));

                System.Data.SqlClient.SqlConnection DBConnection = null;
                System.Data.SqlClient.SqlCommand cmd = null;
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                            "Не удалось получить соединение с базой данных.", "Внимание",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return returnStatus;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartSubTypeListFromIBForExport]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        oleDbJetInsertCommand.Parameters["SUBTYPE_ID"].Value = System.Convert.ToInt32(rs["PARTSUBTYPE_ID"]);
                        oleDbJetInsertCommand.Parameters["NAME"].Value = ((System.Convert.ToString(rs["PARTSUBTYPE_SHORTNAME"]).Length > 48) ? (System.Convert.ToString(rs["PARTSUBTYPE_SHORTNAME"]).Substring(0, 48)) : (System.Convert.ToString(rs["PARTSUBTYPE_SHORTNAME"])));

                        oleDbJetInsertCommand.ExecuteNonQuery();
                    }
                }
                rs.Dispose();

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }

                conn.Close();
                returnStatus = true;

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось произвести экспорт справочника товарных групп в файл dbf.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return returnStatus;

        } // close function

        /// <summary>
        /// Экспорт данных справочника товаров
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="folderPath">каталог</param>
        /// <returns>TRUE - удачное завершение операции; FALSE - ошибка</returns>
        public static bool EхportPARTSToDBF(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, string folderPath)
        {
            string tableName = string.Empty;
            bool returnStatus = false;
            try
            {
                System.IO.File.Delete(System.IO.Path.Combine(folderPath, "PARTS.DBF"));

                string jetOleDbConString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source={0}; Extended Properties=dBASE IV";
                OleDbConnection conn = new System.Data.OleDb.OleDbConnection();
                conn.ConnectionString = String.Format(jetOleDbConString, folderPath);

                System.Data.OleDb.OleDbCommand oleDbCommandCreateTable = new System.Data.OleDb.OleDbCommand();
                System.Data.OleDb.OleDbCommand oleDbJetInsertCommand = new System.Data.OleDb.OleDbCommand();
                oleDbCommandCreateTable.Connection = conn;
                oleDbJetInsertCommand.Connection = conn;

                oleDbCommandCreateTable.CommandText = "CREATE TABLE PARTS (PARTS_ID Integer, TYPE_ID Integer, SUBTYPE_ID Integer,  PARTS_NAME CHAR (52), ARTICLE CHAR (20), MEASURE_ID Integer, NOTVALID Integer,  OWNER_ID Integer, PRICE Double, WEIGTH Double, PACK_QTY Integer, CERT CHAR(250) )";

                conn.Open();

                oleDbCommandCreateTable.ExecuteNonQuery();

                oleDbJetInsertCommand.CommandText = "INSERT INTO PARTS ( PARTS_ID, TYPE_ID, SUBTYPE_ID,  PARTS_NAME, ARTICLE, MEASURE_ID, NOTVALID,  OWNER_ID, PRICE, WEIGTH, PACK_QTY, CERT ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("PARTS_ID", System.Data.OleDb.OleDbType.Integer, 0, "PARTS_ID"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("TYPE_ID", System.Data.OleDb.OleDbType.Integer, 0, "TYPE_ID"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("SUBTYPE_ID", System.Data.OleDb.OleDbType.Integer, 0, "SUBTYPE_ID"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("PARTS_NAME", System.Data.OleDb.OleDbType.VarWChar, 52, "PARTS_NAME"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("ARTICLE", System.Data.OleDb.OleDbType.VarWChar, 20, "ARTICLE"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("MEASURE_ID", System.Data.OleDb.OleDbType.Integer, 0, "MEASURE_ID"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("NOTVALID", System.Data.OleDb.OleDbType.Integer, 0, "NOTVALID"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("OWNER_ID", System.Data.OleDb.OleDbType.Integer, 0, "OWNER_ID"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("PRICE", System.Data.OleDb.OleDbType.Double, 0, "PRICE"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("WEIGTH", System.Data.OleDb.OleDbType.Double, 0, "WEIGTH"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("PACK_QTY", System.Data.OleDb.OleDbType.Integer, 0, "PACK_QTY"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("CERT", System.Data.OleDb.OleDbType.VarWChar, 250, "CERT"));

                System.Data.SqlClient.SqlConnection DBConnection = null;
                System.Data.SqlClient.SqlCommand cmd = null;
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                            "Не удалось получить соединение с базой данных.", "Внимание",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return returnStatus;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartsListFromIBForExport]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        oleDbJetInsertCommand.Parameters["PARTS_ID"].Value = System.Convert.ToInt32(rs["PARTS_ID"]);
                        oleDbJetInsertCommand.Parameters["PARTS_NAME"].Value = ((System.Convert.ToString(rs["PARTS_NAME"]).Length > 52) ? (System.Convert.ToString(rs["PARTS_NAME"]).Substring(0, 52)) : (System.Convert.ToString(rs["PARTS_NAME"])));
                        oleDbJetInsertCommand.Parameters["ARTICLE"].Value = ((System.Convert.ToString(rs["PARTS_ARTICLE"]).Length > 20) ? (System.Convert.ToString(rs["PARTS_ARTICLE"]).Substring(0, 20)) : (System.Convert.ToString(rs["PARTS_ARTICLE"])));
                        oleDbJetInsertCommand.Parameters["TYPE_ID"].Value = System.Convert.ToInt32(rs["PARTTYPE_ID"]);
                        oleDbJetInsertCommand.Parameters["SUBTYPE_ID"].Value = System.Convert.ToInt32(rs["PARTSUBTYPE_ID"]);
                        oleDbJetInsertCommand.Parameters["MEASURE_ID"].Value = System.Convert.ToInt32(rs["MEASURE_ID"]);
                        oleDbJetInsertCommand.Parameters["NOTVALID"].Value = System.Convert.ToInt32(rs["PARTS_NOTVALID"]);
                        oleDbJetInsertCommand.Parameters["OWNER_ID"].Value = System.Convert.ToInt32(rs["OWNER_ID"]);
                        oleDbJetInsertCommand.Parameters["PRICE"].Value = System.Convert.ToDouble(rs["PARTS_VENDORPRICE"]);
                        oleDbJetInsertCommand.Parameters["WEIGTH"].Value = System.Convert.ToDouble(rs["PARTS_WEIGHT"]);
                        oleDbJetInsertCommand.Parameters["PACK_QTY"].Value = System.Convert.ToInt32(rs["PARTS_PACKQTY"]);
                        oleDbJetInsertCommand.Parameters["CERT"].Value = ((System.Convert.ToString(rs["PARTS_NAME"]).Length > 250) ? (System.Convert.ToString(rs["PARTS_CERTIFICATE"]).Substring(0, 250)) : (System.Convert.ToString(rs["PARTS_CERTIFICATE"])));

                        oleDbJetInsertCommand.ExecuteNonQuery();
                    }
                }
                rs.Dispose();

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }




                conn.Close();
                returnStatus = true;

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось произвести экспорт справочника товаров в файл dbf.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return returnStatus;
        } // close function


        /// <summary>
        /// Экспорт данных справочника единиц измерения
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="folderPath">каталог</param>
        /// <returns>TRUE - удачное завершение операции; FALSE - ошибка</returns>
        public static bool EхportMEASUREToDBF(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, string folderPath)
        {
            string tableName = string.Empty;
            bool returnStatus = false;
            try
            {
                System.IO.File.Delete(System.IO.Path.Combine(folderPath, "MEASURE.DBF"));

                string jetOleDbConString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source={0}; Extended Properties=dBASE IV";
                OleDbConnection conn = new System.Data.OleDb.OleDbConnection();
                conn.ConnectionString = String.Format(jetOleDbConString, folderPath);

                System.Data.OleDb.OleDbCommand oleDbCommandCreateTable = new System.Data.OleDb.OleDbCommand();
                System.Data.OleDb.OleDbCommand oleDbJetInsertCommand = new System.Data.OleDb.OleDbCommand();
                oleDbCommandCreateTable.Connection = conn;
                oleDbJetInsertCommand.Connection = conn;

                oleDbCommandCreateTable.CommandText = "CREATE TABLE MEASURE (MEASURE_ID Integer, TYPE_ID Integer, SHORTNAME CHAR (16), FULLNAME CHAR (24) )";

                conn.Open();

                oleDbCommandCreateTable.ExecuteNonQuery();

                oleDbJetInsertCommand.CommandText = "INSERT INTO MEASURE ( MEASURE_ID, TYPE_ID, SHORTNAME, FULLNAME ) VALUES (?, ?, ?, ?)";
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("MEASURE_ID", System.Data.OleDb.OleDbType.Integer, 0, "MEASURE_ID"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("TYPE_ID", System.Data.OleDb.OleDbType.Integer, 0, "TYPE_ID"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("SHORTNAME", System.Data.OleDb.OleDbType.VarWChar, 16, "SHORTNAME"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("FULLNAME", System.Data.OleDb.OleDbType.VarWChar, 24, "FULLNAME"));

                System.Data.SqlClient.SqlConnection DBConnection = null;
                System.Data.SqlClient.SqlCommand cmd = null;
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                            "Не удалось получить соединение с базой данных.", "Внимание",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return returnStatus;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetMeasureFromIBForExport]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        oleDbJetInsertCommand.Parameters["MEASURE_ID"].Value = System.Convert.ToInt32(rs["MEASURE_ID"]);
                        oleDbJetInsertCommand.Parameters["SHORTNAME"].Value = ((System.Convert.ToString(rs["MEASURE_SHORTNAME"]).Length > 16) ? (System.Convert.ToString(rs["MEASURE_SHORTNAME"]).Substring(0, 16)) : (System.Convert.ToString(rs["MEASURE_SHORTNAME"])));
                        oleDbJetInsertCommand.Parameters["FULLNAME"].Value = ((System.Convert.ToString(rs["MEASURE_FULLNAME"]).Length > 24) ? (System.Convert.ToString(rs["MEASURE_FULLNAME"]).Substring(0, 24)) : (System.Convert.ToString(rs["MEASURE_FULLNAME"])));
                        oleDbJetInsertCommand.Parameters["TYPE_ID"].Value = System.Convert.ToInt32(rs["PARTTYPE_ID"]);

                        oleDbJetInsertCommand.ExecuteNonQuery();
                    }
                }
                rs.Dispose();

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }




                conn.Close();
                returnStatus = true;

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось произвести экспорт справочника едениц измерения в файл dbf.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return returnStatus;
        } // close function

        /// <summary>
        /// Экспорт данных справочника штрих-кодов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="folderPath">каталог</param>
        /// <returns>TRUE - удачное завершение операции; FALSE - ошибка</returns>
        public static bool EхportBARCODEToDBF(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, string folderPath)
        {
            string tableName = string.Empty;
            bool returnStatus = false;
            try
            {
                System.IO.File.Delete(System.IO.Path.Combine(folderPath, "BARCODE.DBF"));

                string jetOleDbConString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source={0}; Extended Properties=dBASE IV";
                OleDbConnection conn = new System.Data.OleDb.OleDbConnection();
                conn.ConnectionString = String.Format(jetOleDbConString, folderPath);

                System.Data.OleDb.OleDbCommand oleDbCommandCreateTable = new System.Data.OleDb.OleDbCommand();
                System.Data.OleDb.OleDbCommand oleDbJetInsertCommand = new System.Data.OleDb.OleDbCommand();
                oleDbCommandCreateTable.Connection = conn;
                oleDbJetInsertCommand.Connection = conn;

                oleDbCommandCreateTable.CommandText = "CREATE TABLE BARCODE (PARTS_ID Integer, BARCODE CHAR (13) )";

                conn.Open();

                oleDbCommandCreateTable.ExecuteNonQuery();

                oleDbJetInsertCommand.CommandText = "INSERT INTO BARCODE ( PARTS_ID, BARCODE ) VALUES (?, ?)";
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("PARTS_ID", System.Data.OleDb.OleDbType.Integer, 0, "PARTS_ID"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("BARCODE", System.Data.OleDb.OleDbType.VarWChar, 13, "BARCODE"));

                System.Data.SqlClient.SqlConnection DBConnection = null;
                System.Data.SqlClient.SqlCommand cmd = null;
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                            "Не удалось получить соединение с базой данных.", "Внимание",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return returnStatus;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetBarCodeFromIBForExport]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        oleDbJetInsertCommand.Parameters["PARTS_ID"].Value = System.Convert.ToInt32(rs["PARTS_ID"]);
                        oleDbJetInsertCommand.Parameters["BARCODE"].Value = ((System.Convert.ToString(rs["BARCODE_BARCODE"]).Length > 13) ? (System.Convert.ToString(rs["BARCODE_BARCODE"]).Substring(0, 13)) : (System.Convert.ToString(rs["BARCODE_BARCODE"])));

                        oleDbJetInsertCommand.ExecuteNonQuery();
                    }
                }
                rs.Dispose();

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }

                conn.Close();
                returnStatus = true;

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось произвести экспорт справочника штрих-кодов в файл dbf.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return returnStatus;
        } // close function


        /// <summary>
        /// Экспорт справочника цен
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="folderPath">каталог</param>
        /// <returns>TRUE - удачное завершение операции; FALSE - ошибка</returns>
        public static bool EхportPRICEToDBF(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, string folderPath)
        {
            string tableName = string.Empty;
            bool returnStatus = false;
            try
            {
                System.IO.File.Delete(System.IO.Path.Combine(folderPath, "PRICE.DBF"));

                string jetOleDbConString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source={0}; Extended Properties=dBASE IV";
                OleDbConnection conn = new System.Data.OleDb.OleDbConnection();
                conn.ConnectionString = String.Format(jetOleDbConString, folderPath);

                System.Data.OleDb.OleDbCommand oleDbCommandCreateTable = new System.Data.OleDb.OleDbCommand();
                System.Data.OleDb.OleDbCommand oleDbJetInsertCommand = new System.Data.OleDb.OleDbCommand();
                oleDbCommandCreateTable.Connection = conn;
                oleDbJetInsertCommand.Connection = conn;

                oleDbCommandCreateTable.CommandText = "CREATE TABLE PRICE ( PARTS_ID Integer, PRICE0 Double,  PRICE3 Double,  PRICE12_2 Double,  PRICE4 Double,  PRICE5 Double,  PRICE9 Double,  PRICES_ID Integer,  SUBTYPE_ID Integer )";

                conn.Open();

                oleDbCommandCreateTable.ExecuteNonQuery();

                oleDbJetInsertCommand.CommandText = "INSERT INTO PRICE ( PARTS_ID, PRICE0,  PRICE3,  PRICE12_2,  PRICE4,  PRICE5,  PRICE9,  PRICES_ID,  SUBTYPE_ID ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)";
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("PARTS_ID", System.Data.OleDb.OleDbType.Integer, 0, "PARTS_ID"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("PRICE0", System.Data.OleDb.OleDbType.Double, 0, "PRICE0"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("PRICE3", System.Data.OleDb.OleDbType.Double, 0, "PRICE3"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("PRICE12_2", System.Data.OleDb.OleDbType.Double, 0, "PRICE12_2"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("PRICE4", System.Data.OleDb.OleDbType.Double, 0, "PRICE4"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("PRICE5", System.Data.OleDb.OleDbType.Double, 0, "PRICE5"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("PRICE9", System.Data.OleDb.OleDbType.Double, 0, "PRICE9"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("PRICES_ID", System.Data.OleDb.OleDbType.Integer, 0, "PRICES_ID"));
                oleDbJetInsertCommand.Parameters.Add(new System.Data.OleDb.OleDbParameter("SUBTYPE_ID", System.Data.OleDb.OleDbType.Integer, 0, "SUBTYPE_ID"));

                System.Data.SqlClient.SqlConnection DBConnection = null;
                System.Data.SqlClient.SqlCommand cmd = null;
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                            "Не удалось получить соединение с базой данных.", "Внимание",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return returnStatus;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPricesFromIBForExport]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        oleDbJetInsertCommand.Parameters["PARTS_ID"].Value = System.Convert.ToInt32(rs["PARTS_ID"]);
                        oleDbJetInsertCommand.Parameters["PRICE0"].Value = System.Convert.ToDouble(rs["PRICE0"]);
                        oleDbJetInsertCommand.Parameters["PRICE3"].Value = System.Convert.ToDouble(rs["PRICE6"]);
                        oleDbJetInsertCommand.Parameters["PRICE12_2"].Value = System.Convert.ToDouble(rs["PRICE12_2"]);
                        oleDbJetInsertCommand.Parameters["PRICE4"].Value = System.Convert.ToDouble(rs["PRICE4"]);
                        oleDbJetInsertCommand.Parameters["PRICE5"].Value = System.Convert.ToDouble(rs["PRICE5"]);
                        oleDbJetInsertCommand.Parameters["PRICE9"].Value = System.Convert.ToDouble(rs["PRICE9"]);
                        oleDbJetInsertCommand.Parameters["PRICES_ID"].Value = System.Convert.ToInt32(rs["prices_id"]);
                        oleDbJetInsertCommand.Parameters["SUBTYPE_ID"].Value = System.Convert.ToDouble(rs["partsubtype_id"]);

                        oleDbJetInsertCommand.ExecuteNonQuery();
                    }
                }
                rs.Dispose();

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }

                conn.Close();
                returnStatus = true;

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось произвести экспорт справочника цен в файл dbf.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return returnStatus;
        } // close function
    }
}
