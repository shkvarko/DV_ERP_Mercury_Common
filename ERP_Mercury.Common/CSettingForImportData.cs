using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ERP_Mercury.Common
{
    public class CSettingItemForImportData
    {
        #region Свойства
        /// <summary>
        /// Идентификатор параметра
        /// </summary>
        public System.Int32 TOOLS_ID { get; set; }
        /// <summary>
        /// Параметр (наименование)
        /// </summary>
        public System.String TOOLS_NAME { get; set; }
        /// <summary>
        /// Параметр (наименование для отображения пользователю)
        /// </summary>
        public System.String TOOLS_USERNAME { get; set; }
        /// <summary>
        /// Описание параметра
        /// </summary>
        public System.String TOOLS_DESCRIPTION { get; set; }
        /// <summary>
        /// Значение параметра
        /// </summary>
        public System.Int32 TOOLS_VALUE { get; set; }
        /// <summary>
        /// Тип параметра
        /// </summary>
        public System.Int32 TOOLSTYPE_ID { get; set; }
        #endregion

        #region Конструктор
        public CSettingItemForImportData()
        {
            TOOLS_ID = 0;
            TOOLS_NAME = "";
            TOOLS_USERNAME = "";
            TOOLS_DESCRIPTION = "";
            TOOLS_VALUE = 0;
            TOOLSTYPE_ID = 0;
        }
        #endregion
    }

    public class CSettingForImportData
    {
        #region Свойства
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public System.Guid ID { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public System.String Name { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public System.String UserName { get; set; }
        /// <summary>
        /// Список параметров в xml-виде
        /// </summary>
        public System.Xml.XmlDocument XMLSettings { get; set; }
        /// <summary>
        /// Список параметров
        /// </summary>
        public List<CSettingItemForImportData> SettingsList { get; set; }

        public static readonly System.String strFieldNameSTARTROW = "STARTROW";
        public static readonly System.String strFieldNameARTICLE = "ARTICLE";
        public static readonly System.String strFieldNameNAME2 = "NAME2";
        public static readonly System.String strFieldNameQUANTITY = "QUANTITY";
        public static readonly System.String strFieldNameQUANTITY_COFIRM = "QUANTITY_COFIRM";
        public static readonly System.String strFieldNameQUANTITY_INDOC = "QUANTITY_INDOC";
        public static readonly System.String strFieldNamePRICE = "PRICE";
        public static readonly System.String strFieldNamePRICE_EXW = "PRICE_EXW";
        public static readonly System.String strFieldNamePRICE_INVOICE = "PRICE_INVOICE";
        public static readonly System.String strFieldNamePRICE_FORCALCCOSTPRICE = "PRICE_FORCALCCOSTPRICE";
        public static readonly System.String strFieldNameMARKUP = "MARKUP";
        public static readonly System.String strFieldNameDEPART_CODE = "DEPART_CODE";
        public static readonly System.String strFieldNameCUSTOMER_ID = "CUSTOMER_ID";
        public static readonly System.String strFieldNameRTT_CODE = "RTT_CODE";
        public static readonly System.String strFieldNameFULLNAME = "FULLNAME";
        public static readonly System.String strFieldNamePARTS_ID = "PARTS_ID";
        public static readonly System.String strFieldNameCOUNTRY = "COUNTRY";
        public static readonly System.String strFieldNameTARIFF = "TARIFF";
        public static readonly System.String strFieldNameEXPDATE = "EXPDATE";
        public static readonly System.Int32 iImportTypeForSuppl = 2;
        public static readonly System.Int32 iImportTypeForLotOrderByPartsFullName = 3;
        public static readonly System.Int32 iImportTypeForLotOrderByPartsId = 4;
        public static readonly System.String strFieldNameOWNER_ID = "OWNER_ID";
        public static readonly System.String strFieldNamePARTTYPE_ID = "PARTTYPE_ID";
        public static readonly System.String strFieldNamePARTSUBTYPE_ID = "PARTSUBTYPE_ID";
        public static readonly System.String strFieldNameALLPRICE = "ALLPRICE";

        #endregion

        #region Конструктор
        public CSettingForImportData()
        {
            ID = System.Guid.Empty;
            Name = System.String.Empty;
            UserName = System.String.Empty;
            SettingsList = null;
            XMLSettings = null;
        }
        #endregion

        #region Список параметров
        /// <summary>
        /// Возвращает список настроект
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="SettingNamesListForInitParams">список имён настроек, параметры которых необходимо проинициализировать</param>
        /// <param name="strErr">текст ошибки</param>
        /// <returns>список настроект</returns>
        public static List<CSettingForImportData> GetSettingslist(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,  
            List<System.String> SettingNamesListForInitParams, ref System.String strErr)
        {
            List<CSettingForImportData> objRet = new List<CSettingForImportData>();
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr += ("\nНе удалось получить соединение с базой данных.");
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetSettings]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CSettingForImportData objItem = null;
                    while (rs.Read())
                    {
                        objItem = new CSettingForImportData();
                        objItem.ID = (System.Guid)rs["Settings_Guid"];
                        objItem.Name = ((rs["Settings_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Settings_Name"]));
                        objItem.UserName = ((rs["Settings_UserName"] == System.DBNull.Value) ? System.Convert.ToString(rs["Settings_Name"]) : System.Convert.ToString(rs["Settings_UserName"]));
                        objItem.SettingsList = new List<CSettingItemForImportData>();

                        objItem.XMLSettings = new System.Xml.XmlDocument();
                        objItem.XMLSettings.LoadXml(rs.GetSqlXml(2).Value);

                        if (SettingNamesListForInitParams.SingleOrDefault<String>(x => x == objItem.Name ) != null)
                        {
                            foreach (System.Xml.XmlNode objNode in objItem.XMLSettings.ChildNodes)
                            {
                                foreach (System.Xml.XmlNode objChildNode in objNode.ChildNodes)
                                {
                                    objItem.SettingsList.Add(new CSettingItemForImportData()
                                    {
                                        TOOLS_ID = System.Convert.ToInt32(objChildNode.Attributes["TOOLS_ID"].Value),
                                        TOOLS_NAME = System.Convert.ToString(objChildNode.Attributes["TOOLS_NAME"].Value),
                                        TOOLS_USERNAME = System.Convert.ToString(objChildNode.Attributes["TOOLS_USERNAME"].Value),
                                        TOOLS_DESCRIPTION = System.Convert.ToString(objChildNode.Attributes["TOOLS_DESCRIPTION"].Value),
                                        TOOLS_VALUE = System.Convert.ToInt32(objChildNode.Attributes["TOOLS_VALUE"].Value),
                                        TOOLSTYPE_ID = System.Convert.ToInt32(objChildNode.Attributes["TOOLSTYPE_ID"].Value)

                                    });
                                }
                            }
                        }

                        objRet.Add(objItem);

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
                strErr += (String.Format("Не удалось получить список настроек.\n\nТекст ошибки: {0}", f.Message));
            }
            return objRet;
        }


        /// <summary>
        /// Возвращает настройки для импорта данных в заказ
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="iImportTypeId">вариант импорта данных в приложение к заказу</param>
        /// <returns>список настроек</returns>
        public static CSettingForImportData GetSettingForImportData(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, 
            System.Int32 iImportTypeId = 0)
        {
            CSettingForImportData objRet = null;
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

                switch (iImportTypeId)
                {
                    case 0 :
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetImportDataInOrderSettings]", objProfile.GetOptionsDllDBName());
                        break;
                    case 1:
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetImportDataInOrderByIDSettings]", objProfile.GetOptionsDllDBName());
                        break;
                    default:
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetImportDataInOrderSettings]", objProfile.GetOptionsDllDBName());
                        break;

                }
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {

                    rs.Read();
                    {
                        objRet = new CSettingForImportData();
                        objRet.ID = (System.Guid)rs["Settings_Guid"];
                        objRet.Name = System.Convert.ToString(rs["Settings_Name"]);
                        objRet.SettingsList = new List<CSettingItemForImportData>();

                        objRet.XMLSettings = new System.Xml.XmlDocument();
                        objRet.XMLSettings.LoadXml(rs.GetSqlXml(2).Value);

                        foreach (System.Xml.XmlNode objNode in objRet.XMLSettings.ChildNodes)
                        {
                            foreach (System.Xml.XmlNode objChildNode in objNode.ChildNodes)
                            {
                                objRet.SettingsList.Add(new CSettingItemForImportData()
                                {
                                    TOOLS_ID = System.Convert.ToInt32(objChildNode.Attributes["TOOLS_ID"].Value),
                                    TOOLS_NAME = System.Convert.ToString(objChildNode.Attributes["TOOLS_NAME"].Value),
                                    TOOLS_USERNAME = System.Convert.ToString(objChildNode.Attributes["TOOLS_USERNAME"].Value),
                                    TOOLS_DESCRIPTION = System.Convert.ToString(objChildNode.Attributes["TOOLS_DESCRIPTION"].Value),
                                    TOOLS_VALUE = System.Convert.ToInt32(objChildNode.Attributes["TOOLS_VALUE"].Value),
                                    TOOLSTYPE_ID = System.Convert.ToInt32(objChildNode.Attributes["TOOLSTYPE_ID"].Value)

                                });
                            }
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
                "Не удалось получить список настроек.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objRet;
        }

        /// <summary>
        /// Возвращает настройки для импорта данных в заказ
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список настроек</returns>
        public static CSettingForImportData GetSettingForImportDataInLotOrderByPartsFullName(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            CSettingForImportData objRet = null;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetImportDataInLotOrderByPartsFullNameSettings]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {

                    rs.Read();
                    {
                        objRet = new CSettingForImportData();
                        objRet.ID = (System.Guid)rs["Settings_Guid"];
                        objRet.Name = System.Convert.ToString(rs["Settings_Name"]);
                        objRet.SettingsList = new List<CSettingItemForImportData>();

                        objRet.XMLSettings = new System.Xml.XmlDocument();
                        objRet.XMLSettings.LoadXml(rs.GetSqlXml(2).Value);

                        foreach (System.Xml.XmlNode objNode in objRet.XMLSettings.ChildNodes)
                        {
                            foreach (System.Xml.XmlNode objChildNode in objNode.ChildNodes)
                            {
                                objRet.SettingsList.Add(new CSettingItemForImportData()
                                {
                                    TOOLS_ID = System.Convert.ToInt32(objChildNode.Attributes["TOOLS_ID"].Value),
                                    TOOLS_NAME = System.Convert.ToString(objChildNode.Attributes["TOOLS_NAME"].Value),
                                    TOOLS_USERNAME = System.Convert.ToString(objChildNode.Attributes["TOOLS_USERNAME"].Value),
                                    TOOLS_DESCRIPTION = System.Convert.ToString(objChildNode.Attributes["TOOLS_DESCRIPTION"].Value),
                                    TOOLS_VALUE = System.Convert.ToInt32(objChildNode.Attributes["TOOLS_VALUE"].Value),
                                    TOOLSTYPE_ID = System.Convert.ToInt32(objChildNode.Attributes["TOOLSTYPE_ID"].Value)

                                });
                            }
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
                "Не удалось получить список настроек.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objRet;
        }
        /// <summary>
        /// Возвращает настройки для импорта данных в заказ
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список настроек</returns>
        public static CSettingForImportData GetSettingForImportDataInLotOrderByPartsId(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            CSettingForImportData objRet = null;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetImportDataInLotOrderByPartsIdSettings]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {

                    rs.Read();
                    {
                        objRet = new CSettingForImportData();
                        objRet.ID = (System.Guid)rs["Settings_Guid"];
                        objRet.Name = System.Convert.ToString(rs["Settings_Name"]);
                        objRet.SettingsList = new List<CSettingItemForImportData>();

                        objRet.XMLSettings = new System.Xml.XmlDocument();
                        objRet.XMLSettings.LoadXml(rs.GetSqlXml(2).Value);

                        foreach (System.Xml.XmlNode objNode in objRet.XMLSettings.ChildNodes)
                        {
                            foreach (System.Xml.XmlNode objChildNode in objNode.ChildNodes)
                            {
                                objRet.SettingsList.Add(new CSettingItemForImportData()
                                {
                                    TOOLS_ID = System.Convert.ToInt32(objChildNode.Attributes["TOOLS_ID"].Value),
                                    TOOLS_NAME = System.Convert.ToString(objChildNode.Attributes["TOOLS_NAME"].Value),
                                    TOOLS_USERNAME = System.Convert.ToString(objChildNode.Attributes["TOOLS_USERNAME"].Value),
                                    TOOLS_DESCRIPTION = System.Convert.ToString(objChildNode.Attributes["TOOLS_DESCRIPTION"].Value),
                                    TOOLS_VALUE = System.Convert.ToInt32(objChildNode.Attributes["TOOLS_VALUE"].Value),
                                    TOOLSTYPE_ID = System.Convert.ToInt32(objChildNode.Attributes["TOOLSTYPE_ID"].Value)

                                });
                            }
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
                "Не удалось получить список настроек.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objRet;
        }

        /// <summary>
        /// Возвращает настройки для импорта данных в приход на склад
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список настроек</returns>
        public static CSettingForImportData GetSettingForImportDataInLotByPartsId(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            CSettingForImportData objRet = null;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetSettingsForImportDataInLot]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {

                    rs.Read();
                    {
                        objRet = new CSettingForImportData();
                        objRet.ID = (System.Guid)rs["Settings_Guid"];
                        objRet.Name = System.Convert.ToString(rs["Settings_Name"]);
                        objRet.SettingsList = new List<CSettingItemForImportData>();

                        objRet.XMLSettings = new System.Xml.XmlDocument();
                        objRet.XMLSettings.LoadXml(rs.GetSqlXml(2).Value);

                        foreach (System.Xml.XmlNode objNode in objRet.XMLSettings.ChildNodes)
                        {
                            foreach (System.Xml.XmlNode objChildNode in objNode.ChildNodes)
                            {
                                objRet.SettingsList.Add(new CSettingItemForImportData()
                                {
                                    TOOLS_ID = System.Convert.ToInt32(objChildNode.Attributes["TOOLS_ID"].Value),
                                    TOOLS_NAME = System.Convert.ToString(objChildNode.Attributes["TOOLS_NAME"].Value),
                                    TOOLS_USERNAME = System.Convert.ToString(objChildNode.Attributes["TOOLS_USERNAME"].Value),
                                    TOOLS_DESCRIPTION = System.Convert.ToString(objChildNode.Attributes["TOOLS_DESCRIPTION"].Value),
                                    TOOLS_VALUE = System.Convert.ToInt32(objChildNode.Attributes["TOOLS_VALUE"].Value),
                                    TOOLSTYPE_ID = System.Convert.ToInt32(objChildNode.Attributes["TOOLSTYPE_ID"].Value)

                                });
                            }
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
                "Не удалось получить список настроек.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objRet;
        }

        /// <summary>
        /// Возвращает настройки для импорта данных в приложение к плану
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список настроек</returns>
        public static CSettingForImportData GetSettingForImportDataInPlanByDepartCustomerSubtype(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            CSettingForImportData objRet = null;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetImportDataInPlanByDepartCustomerSubtypeSettings]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {

                    rs.Read();
                    {
                        objRet = new CSettingForImportData();
                        objRet.ID = (System.Guid)rs["Settings_Guid"];
                        objRet.Name = System.Convert.ToString(rs["Settings_Name"]);
                        objRet.SettingsList = new List<CSettingItemForImportData>();

                        objRet.XMLSettings = new System.Xml.XmlDocument();
                        objRet.XMLSettings.LoadXml(rs.GetSqlXml(2).Value);

                        foreach (System.Xml.XmlNode objNode in objRet.XMLSettings.ChildNodes)
                        {
                            foreach (System.Xml.XmlNode objChildNode in objNode.ChildNodes)
                            {
                                objRet.SettingsList.Add(new CSettingItemForImportData()
                                {
                                    TOOLS_ID = System.Convert.ToInt32(objChildNode.Attributes["TOOLS_ID"].Value),
                                    TOOLS_NAME = System.Convert.ToString(objChildNode.Attributes["TOOLS_NAME"].Value),
                                    TOOLS_USERNAME = System.Convert.ToString(objChildNode.Attributes["TOOLS_USERNAME"].Value),
                                    TOOLS_DESCRIPTION = System.Convert.ToString(objChildNode.Attributes["TOOLS_DESCRIPTION"].Value),
                                    TOOLS_VALUE = System.Convert.ToInt32(objChildNode.Attributes["TOOLS_VALUE"].Value),
                                    TOOLSTYPE_ID = System.Convert.ToInt32(objChildNode.Attributes["TOOLSTYPE_ID"].Value)

                                });
                            }
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
                "Не удалось получить список настроек.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objRet;
        }

        #endregion

        #region Сохранение настроек в базе данных
        /// <summary>
        /// Сохраняет в БД настройки для импорта данных в заказ
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">строка с сообщением об ошибке</param>
        /// <returns>true - успешное завершение операции; false - ошибка</returns>
        public System.Boolean SaveExportSetting(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                bRet = CSetting.SaveSettingInDB(this.ID, this.XMLSettings.InnerXml, objProfile, cmdSQL, ref strErr);
            }
            catch (System.Exception f)
            {
                strErr += (f.Message);
            }
            finally
            {
            }
            return bRet;
        }
        #endregion

        public override string ToString()
        {
            return UserName;
        }

    }
}
