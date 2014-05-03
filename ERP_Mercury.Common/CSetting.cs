using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP_Mercury.Common
{
    public class CSetting
    {
        #region Свойства
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        private System.Guid m_uuidID;
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public System.Guid ID
        {
            get { return m_uuidID; }
            set { m_uuidID = value; }
        }
        /// <summary>
        /// Наименование
        /// </summary>
        private System.String m_strName;
        /// <summary>
        /// Наименование
        /// </summary>
        public System.String Name
        {
            get { return m_strName; }
            set { m_strName = value; }
        }
        /// <summary>
        /// Примечание
        /// </summary>
        private System.String m_strDescription;
        /// <summary>
        /// Примечание
        /// </summary>
        public System.String Description
        {
            get { return m_strDescription; }
            set { m_strDescription = value; }
        }
        /// <summary>
        /// Список параметров с значениями
        /// </summary>
        private List<CAdvancedParam> m_objParamList;
        /// <summary>
        /// Список параметров с значениями
        /// </summary>
        public List<CAdvancedParam> ParamList
        {
            get { return m_objParamList; }
            set { m_objParamList = value; }
        }
        /// <summary>
        /// Список дополнительных параметров в xml - виде
        /// </summary>
        private System.Xml.XmlDocument m_xmldocAdvancedParamList;
        /// <summary>
        /// Список дополнительных параметров в xml - виде
        /// </summary>
        public System.Xml.XmlDocument xmldocAdvancedParamList
        {
            get { return m_xmldocAdvancedParamList; }
            set { m_xmldocAdvancedParamList = value; }
        }
        #endregion

        #region Конструктор
        public CSetting()
        {
            m_uuidID = System.Guid.Empty;
            m_strName = "";
            m_strDescription = "";
            m_objParamList = null;
            m_xmldocAdvancedParamList = null;
        }
        #endregion

        #region Список настроек
        /// <summary>
        /// Возвращает список настроек
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список настроек</returns>
        public static List<CSetting> GetSettingsList( UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CSetting> objList = new List<CSetting>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetSettings]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CSetting objSetting = null;
                    System.Xml.XmlDocument docInfoAboutParam = null;
                    CAdvancedParam objParam = null;
                    System.Xml.XmlNode objRootNode = null;
                    while (rs.Read())
                    {
                        objSetting = new CSetting();
                        objSetting.ID = (System.Guid)rs["Settings_Guid"];
                        objSetting.Name = System.Convert.ToString(rs["Settings_Name"]);
                        objSetting.ParamList = new List<CAdvancedParam>();
                        if( rs["Settings_XML"] != System.DBNull.Value )
                        {
                            docInfoAboutParam = new System.Xml.XmlDocument();
                            docInfoAboutParam.LoadXml( System.Convert.ToString( rs["Settings_XML"] ) );
                            if ((docInfoAboutParam != null) && (docInfoAboutParam.ChildNodes.Count > 0))
                            {
                                objSetting.xmldocAdvancedParamList = docInfoAboutParam;
                                objRootNode = docInfoAboutParam.ChildNodes[0];
                                //objSetting.Name = objRootNode.Name;

                                foreach (System.Xml.XmlNode objNode in objRootNode.ChildNodes)
                                {
                                    //if (objNode.Name == "CommonParams")
                                    //{
                                        foreach (System.Xml.XmlNode objChildNode in objNode.ChildNodes)
                                        {
                                            //if (objChildNode.Name == "Params")
                                            //{
                                                foreach (System.Xml.XmlAttribute objAttribute in objChildNode.Attributes)
                                                {
                                                    objParam = new CAdvancedParam(System.Guid.Empty, objAttribute.Name, "", new CParamDataType( 0, "" ) );
                                                    objParam.GroupName = objNode.Name;
                                                    objParam.Value = objAttribute.Value;
                                                    objSetting.ParamList.Add(objParam);
                                                }
                                            //}
                                        //}
                                    }
                                }
                            }

                        }

                        objList.Add(objSetting);
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
            return objList;
        }

        #endregion

        #region Сохранение настройки в БД
        /// <summary>
        /// Сохраняет настройку в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean SaveSettingInDB(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_SetSetting]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Settings_Guid", System.Data.SqlDbType.UniqueIdentifier, 8));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Settings_XML", System.Data.SqlDbType.Xml));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Settings_Guid"].Value = this.ID;
                cmd.Parameters["@Settings_XML"].Value = this.m_xmldocAdvancedParamList.InnerXml; //.Value;
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
        /// Сохраняет настройку в БД
        /// </summary>
        /// <param name="uuidSettingId">УИ настройкиb</param>
        /// <param name="strInnerXml">XML-представление</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean SaveSettingInDB(System.Guid uuidSettingId, System.String strInnerXml, UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_SetSetting]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Settings_Guid", System.Data.SqlDbType.UniqueIdentifier, 8));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Settings_XML", System.Data.SqlDbType.Xml));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Settings_Guid"].Value = uuidSettingId;
                cmd.Parameters["@Settings_XML"].Value = strInnerXml; //.Value;
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
        #endregion

    }
}
