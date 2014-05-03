using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 

namespace ERP_Mercury.Common
{
    /// <summary>
    /// Класс "Товарная марка"
    /// </summary>
    public class CProductOwner
    {
        #region Переменные, свойства
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        private System.Guid m_uuidID;
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public System.Guid uuidID
        {
            get { return m_uuidID; }
            set { m_uuidID = value; }
        }
        /// <summary>
        /// Имя
        /// </summary>
        private System.String m_strName;
        /// <summary>
        /// Имя
        /// </summary>
        public System.String Name
        {
            get { return m_strName; }
            set { m_strName = value; }
        }
        /// <summary>
        /// Признак "активен"
        /// </summary>
        private System.Boolean m_bIsActive;
        /// <summary>
        /// Признак "активен"
        /// </summary>
        public System.Boolean IsActive
        {
            get { return m_bIsActive; }
            set { m_bIsActive = value; }
        }
        private System.Int32 m_iIb_Id;
        public System.Int32 Ib_Id
        {
            get { return m_iIb_Id; }
            set { m_iIb_Id = value; }
        }
        #endregion

        #region Конструкторы
        public CProductOwner()
        {
            this.m_uuidID = System.Guid.Empty;
            this.m_strName = "";
            this.m_bIsActive = false;
        }
        public CProductOwner(System.Guid uuidID, System.String strName, System.Boolean bActive)
        {
            this.m_uuidID = uuidID;
            this.m_strName = strName;
            this.m_bIsActive = bActive;
        }
        #endregion

        #region Список товарных марок
        /// <summary>
        /// Возвращает список товарных марок
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <returns>список товарных марок</returns>
        public static List<CProductOwner> GetProductOwnerList(UniXP.Common.CProfile objProfile)
        {
            List<CProductOwner> objList = new List<CProductOwner>();

            System.Data.SqlClient.SqlConnection DBConnection = objProfile.GetDBSource();
            if (DBConnection == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Не удалось получить список товарных марок.\nОтсутствует соединение с БД.", "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return objList;
            }

            try
            {
                // соединение с БД получено, прописываем команду на выборку данных
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetProductCatalog]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CProductOwner objProductOwner = null;
                    while (rs.Read())
                    {
                        objProductOwner = new CProductOwner((System.Guid)rs["Owner_Guid"], (System.String)rs["Owner_Name"], System.Convert.ToBoolean(rs["Owner_IsActive"]));
                        objProductOwner.Ib_Id = System.Convert.ToInt32(rs["Owner_Id"]);
                        objList.Add(objProductOwner);
                    }
                }

                rs.Close();
                rs.Dispose();
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Не удалось получить список товарных марок.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
			finally // очищаем занимаемые ресурсы
            {
                DBConnection.Close();
            }
            return objList;
        }

        /// <summary>
        /// Возвращает список команд
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="objDepartTeam">команда</param>
        /// <returns>список товарных марок</returns>
        public static List<CProductOwner> GetProductOwnerListForDepartTeam(UniXP.Common.CProfile objProfile,
            CDepartTeam objDepartTeam, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CProductOwner> objList = new List<CProductOwner>();
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;

            if (objDepartTeam == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Не удалось получить список товарных марок.\nНе определена команда торговых представителей.", "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return objList;
            }

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

            try
            {
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetProductOwnersForDepartTeam]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DepartTeam_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@DepartTeam_Guid"].Value = objDepartTeam.uuidID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CProductOwner((System.Guid)rs["Owner_Guid"], (System.String)rs["Owner_Name"], System.Convert.ToBoolean(rs["Owner_IsActive"])));
                    }
                }

                rs.Close();
                rs.Dispose();
                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Не удалось получить список товарных марок.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
			finally // очищаем занимаемые ресурсы
            {
            }
            return objList;
        }

        #endregion

        #region Привязка товарных марок к команде
        /// <summary>
        /// Привязка списка товарных марок к команде
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objProductOwnerList">список товарных марок</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean SetProductOwnerListForDepartTeam(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            CDepartTeam objDepartTeam, List<CProductOwner> objProductOwnerList, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (objProductOwnerList == null) { return bRet; }

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

                System.Data.DataTable addedCategories = new System.Data.DataTable();
                addedCategories.Columns.Add(new System.Data.DataColumn("Customer_Guid", typeof(System.Data.SqlTypes.SqlGuid)));

                System.Data.DataRow newRow = null;
                foreach (CProductOwner objItem in objProductOwnerList)
                {
                    newRow = addedCategories.NewRow();
                    newRow[0] = objItem.uuidID;
                    addedCategories.Rows.Add(newRow);
                }
                if (objProductOwnerList.Count > 0)
                {
                    addedCategories.AcceptChanges();
                }


                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddProductOwnerToDepartTeam]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DepartTeam_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.AddWithValue("@tProductOwnerList", addedCategories);
                cmd.Parameters["@tProductOwnerList"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tProductOwnerList"].TypeName = "dbo.udt_CustomerListForDepart";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@DepartTeam_Guid"].Value = objDepartTeam.uuidID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                //if (iRes == 0)
                //{
                //    cmd.Parameters.Clear();
                //    cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddProductOwnerToDepartTeam]", objProfile.GetOptionsDllDBName());
                //    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                //    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DepartTeam_Guid", System.Data.SqlDbType.UniqueIdentifier));
                //    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductOwner_Guid", System.Data.SqlDbType.UniqueIdentifier));
                //    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                //    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                //    cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                //    cmd.Parameters["@DepartTeam_Guid"].Value = objDepartTeam.uuidID;
                //    foreach (CProductOwner objItem in objProductOwnerList)
                //    {
                //        cmd.Parameters["@ProductOwner_Guid"].Value = objItem.uuidID;
                //        cmd.ExecuteNonQuery();
                //        iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                //        if (iRes != 0)
                //        {
                //            strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                //            break;
                //        }
                //    }
                //}
                //else
                //{
                //    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                //}

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

        public override string ToString()
        {
            return this.m_strName;
        }
    }
    /// <summary>
    /// Класс "Команда торговых представителей"
    /// </summary>
    public class CDepartTeam
    {
        #region Переменные, Свойства, Константы
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        private System.Guid m_uuidID;
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public System.Guid uuidID
        {
            get { return m_uuidID; }
            set { m_uuidID = value; }
        }
        /// <summary>
        /// Имя
        /// </summary>
        private System.String m_strDepartTeamName;
        /// <summary>
        /// Имя
        /// </summary>
        public System.String DepartTeamName
        {
            get { return m_strDepartTeamName; }
            set { m_strDepartTeamName = value; }
        }
        /// <summary>
        /// Описание
        /// </summary>
        private System.String m_strDepartTeamDescrpn;
        /// <summary>
        /// Описание
        /// </summary>
        public System.String DepartTeamDescrpn
        {
            get { return m_strDepartTeamDescrpn; }
            set { m_strDepartTeamDescrpn = value; }
        }
        /// <summary>
        /// Признак "активен"
        /// </summary>
        private System.Boolean m_bDepartTeamIsActive;
        /// <summary>
        /// Признак "активен"
        /// </summary>
        public System.Boolean DepartTeamIsActive
        {
            get { return m_bDepartTeamIsActive; }
            set { m_bDepartTeamIsActive = value; }
        }
        /// <summary>
        /// Список товарных марок
        /// </summary>
        private List<CProductOwner> m_objProductOwnerList;
        /// <summary>
        /// Список товарных марок
        /// </summary>
        public List<CProductOwner> ProductOwnerList
        {
            get { return m_objProductOwnerList; }
            set { m_objProductOwnerList = value; }
        }
        /// <summary>
        /// Список подразделений
        /// </summary>
        private List<CDepart> m_objDepartList;
        /// <summary>
        /// Список подразделений
        /// </summary>
        public List<CDepart> DepartList
        {
            get { return m_objDepartList; }
            set { m_objDepartList = value; }
        }

        #endregion

        #region Конструкторы
        public CDepartTeam(System.Guid uuidID, System.String strDepartTeamName,
            System.String strDepartTeamDescrpn, System.Boolean bDepartTeamActive)
        {
            this.m_uuidID = uuidID;
            this.m_strDepartTeamName = strDepartTeamName;
            this.m_strDepartTeamDescrpn = strDepartTeamDescrpn;
            this.m_bDepartTeamIsActive = bDepartTeamActive;
            this.m_objProductOwnerList = null;
            this.m_objDepartList = null;
        }
        #endregion

        #region Список команд
        /// <summary>
        /// Возвращает список команд
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <returns>список команд</returns>
        public static List<CDepartTeam> GetDepartTeamList(UniXP.Common.CProfile objProfile, 
            System.Data.SqlClient.SqlCommand cmdSQL, System.Boolean bInitDetails)
        {
            List<CDepartTeam> objList = new List<CDepartTeam>();

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

            try
            {
                // соединение с БД получено, прописываем команду на выборку данных
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetDepartTeam]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.String strDescrpn = "";
                    while (rs.Read())
                    {
                        strDescrpn = (rs["DepartTeam_Description"] == System.DBNull.Value) ? "" : (System.String)rs["DepartTeam_Description"];
                        objList.Add(new CDepartTeam((System.Guid)rs["DepartTeam_Guid"], (System.String)rs["DepartTeam_Name"], strDescrpn, System.Convert.ToBoolean(rs["DepartTeam_IsActive"])));
                    }
                }

                rs.Close();
                rs.Dispose();

                if ((objList != null) && (objList.Count > 0))
                {
                    if (bInitDetails == true)
                    {
                        foreach (CDepartTeam objItem in objList)
                        {
                            objItem.m_objProductOwnerList = CProductOwner.GetProductOwnerListForDepartTeam(objProfile, objItem, cmd);
                        }
                    }
                }
                //// попробуем запросить список подразделений
                //if ((objList != null) && (objList.Count > 0))
                //{
                //    if (bInitDetails == true)
                //    {
                //        foreach (CDepartTeam objItem in objList)
                //        {
                //            objItem.m_objDepartList = CDepart.GetDepartListForDepartTeam(objProfile, objItem, cmd);
                //        }
                //    }
                //}
                if (cmdSQL == null)
                {
                    cmd.Dispose();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Не удалось получить список команд.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
			finally // очищаем занимаемые ресурсы
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return objList;
        }
        #endregion

        #region Добавить команду в базу данных
        /// <summary>
        /// Проверка свойств объекта перед сохранением
        /// </summary>
        /// <param name="strErr">текст с ошибкой</param>
        /// <returns>true - все свойства корректны; false - ошибка</returns>
        public System.Boolean IsAllParametersValid(ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (this.DepartTeamName == "")
                {
                    strErr = "Необходимо указать название команды!";
                    return bRet;
                }

                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr = "Ошибка проверки свойств. Текст ошибки: " + f.Message;
            }
            return bRet;
        }
        /// <summary>
        /// Добавляет в базу данных новую команду
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean AddToDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (this.IsAllParametersValid(ref strErr) == false)
                {
                    return bRet;
                }

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
                }

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddDepartTeam]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DepartTeam_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DepartTeam_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DepartTeam_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DepartTeam_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@DepartTeam_Name"].Value = this.DepartTeamName;
                cmd.Parameters["@DepartTeam_IsActive"].Value = this.DepartTeamIsActive;

                if (this.DepartTeamDescrpn != "")
                {
                    cmd.Parameters["@DepartTeam_Description"].IsNullable = false;
                    cmd.Parameters["@DepartTeam_Description"].Value = this.DepartTeamDescrpn;
                }
                else
                {
                    cmd.Parameters["@DepartTeam_Description"].IsNullable = true;
                    cmd.Parameters["@DepartTeam_Description"].Value = null;
                }

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    this.m_uuidID = (System.Guid)cmd.Parameters["@DepartTeam_Guid"].Value;
                    // список назначенных товарных марок
                    if (this.ProductOwnerList != null)
                    {
                        iRes = (CProductOwner.SetProductOwnerListForDepartTeam(objProfile, cmd, this,
                            this.ProductOwnerList, ref strErr) == true) ? 0 : 1;
                    }
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
                    cmd.Dispose();
                    cmd = null;
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

        #region Редактирование свойств команды в БД
        /// <summary>
        /// Изменяет свойства подразделения в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean EditInDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (this.IsAllParametersValid(ref strErr) == false)
                {
                    return bRet;
                }

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
                }

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditDepartTeam]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DepartTeam_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DepartTeam_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DepartTeam_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DepartTeam_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@DepartTeam_Guid"].Value = this.uuidID;
                cmd.Parameters["@DepartTeam_Name"].Value = this.DepartTeamName;
                cmd.Parameters["@DepartTeam_IsActive"].Value = this.DepartTeamIsActive;

                if (this.DepartTeamDescrpn != "")
                {
                    cmd.Parameters["@DepartTeam_Description"].IsNullable = false;
                    cmd.Parameters["@DepartTeam_Description"].Value = this.DepartTeamDescrpn;
                }
                else
                {
                    cmd.Parameters["@DepartTeam_Description"].IsNullable = true;
                    cmd.Parameters["@DepartTeam_Description"].Value = null;
                }

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    // список назначенных товарных марок
                    if ( this.ProductOwnerList != null)
                    {
                        iRes = ( CProductOwner.SetProductOwnerListForDepartTeam(objProfile, cmd, this,
                            this.ProductOwnerList, ref strErr) == true ) ? 0 : 1;
                    }
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
                    cmd.Dispose();
                    cmd = null;
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

        #region Удаление команды из БД
        /// <summary>
        /// Удаление описания команды из БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean RemoveFromDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteDepartTeam]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DepartTeam_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@DepartTeam_Guid"].Value = this.m_uuidID;
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
        #endregion

        public override string ToString()
        {
            return this.DepartTeamName;
        }
    }
    /// <summary>
    /// Класс "Подразделение"
    /// </summary>
    public class CDepart
    {
        #region Переменные, Свойства, Константы
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        private System.Guid m_uuidID;
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public System.Guid uuidID
        {
            get { return m_uuidID; }
            set { m_uuidID = value; }
        }
        /// <summary>
        /// Код
        /// </summary>
        private System.String m_strDepartCode;
        /// <summary>
        /// Код
        /// </summary>
        public System.String DepartCode
        {
            get { return m_strDepartCode; }
            set { m_strDepartCode = value; }
        }
        /// <summary>
        /// Описание
        /// </summary>
        private System.String m_strDepartDescription;
        /// <summary>
        /// Описание
        /// </summary>
        public System.String DepartDescription
        {
            get { return m_strDepartDescription; }
            set { m_strDepartDescription = value; }
        }
        /// <summary>
        /// Признак "активен"
        /// </summary>
        private System.Boolean m_bDepartIsActive;
        /// <summary>
        /// Признак "активен"
        /// </summary>
        public System.Boolean DepartIsActive
        {
            get { return m_bDepartIsActive; }
            set { m_bDepartIsActive = value; }
        }
        /// <summary>
        /// Торговый представитель
        /// </summary>
        private CSalesMan m_objSalesMan;
        /// <summary>
        /// Торговый представитель
        /// </summary>
        public CSalesMan SalesMan
        {
            get { return m_objSalesMan; }
            set { m_objSalesMan = value; }
        }
        /// <summary>
        /// Команда
        /// </summary>
        private CDepartTeam m_objDepartTeam;
        /// <summary>
        /// Команда
        /// </summary>
        public CDepartTeam DepartTeam
        {
            get { return m_objDepartTeam; }
            set { m_objDepartTeam = value; }
        }
        /// <summary>
        /// Список назначенных клиентов
        /// </summary>
        private List<CCustomer> m_objCustomerList;
        /// <summary>
        /// Список назначенных клиентов
        /// </summary>
        public List<CCustomer> CustomerList
        {
            get { return m_objCustomerList; }
            set { m_objCustomerList = value; }
        }

        #endregion

        #region Конструктор
        public CDepart()
        {
            m_uuidID = System.Guid.Empty;
            m_strDepartCode = "";
            m_strDepartDescription = "";
            m_objDepartTeam = null;
            m_objSalesMan = null;
            m_bDepartIsActive = false;
            m_objCustomerList = null;
        }
        public CDepart(System.Guid uuidID, System.String strDepartCode, System.String strDepartDescription,
            CDepartTeam objDepartTeam, CSalesMan objSalesMan, System.Boolean bDepartIsActive)
        {
            m_uuidID = uuidID;
            m_strDepartCode = strDepartCode;
            m_strDepartDescription = strDepartDescription;
            m_objDepartTeam = objDepartTeam;
            m_objSalesMan = objSalesMan;
            m_bDepartIsActive = bDepartIsActive;
            m_objCustomerList = null;
        }
        #endregion

        #region Список подразделений
        /// <summary>
        /// Возвращает список подразделений для заданной команды
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="DepartTeamGuid">УИ торговой команды</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="bOnlyActiveDepart">признак "только активные подразделения"</param>
        /// <returns>список торговых подразделений</returns>
        public static List<CDepart> GetDepartListForTeam(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid DepartTeamGuid, ref System.String strErr, System.Boolean bOnlyActiveDepart = true )
        {
            List<CDepart> objList = null;
            try
            {
                objList = GetDepartAllList(objProfile, cmdSQL, ref strErr);
                
                if ((objList != null) && (objList.Count > 0))
                {
                    objList = objList.Where<CDepart>(x => ((x.DepartTeam != null) && (x.DepartTeam.uuidID.CompareTo(DepartTeamGuid) == 0))).ToList<CDepart>();

                    if ((objList != null) && (objList.Count > 0) && (bOnlyActiveDepart == true))
                    {
                        objList = objList.Where<CDepart>( x=>x.DepartIsActive == true ).ToList<CDepart>();
                    }
                }
            }
            catch (System.Exception f)
            {
                strErr += ("Не удалось получить список подразделений. Текст ошибки: " + f.Message);
            }
			finally // очищаем занимаемые ресурсы
            {
            }
            return objList;

        }
        /// <summary>
        /// Возвращает список подразделений из БД (T_Depart)
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список пользователей</returns>
        public static List<CDepart> GetDepartList(UniXP.Common.CProfile objProfile,  System.Data.SqlClient.SqlCommand cmdSQL )
        {
            List<CDepart> objList = null;
            System.String strErr = System.String.Empty;
            try
            {
                objList = GetDepartAllList(objProfile, cmdSQL, ref strErr);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Не удалось получить список подразделений.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
			finally // очищаем занимаемые ресурсы
            {
            }
            return objList;
        }
        /// <summary>
        /// Возвращает список всех подразделений
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>список торговых подразделений</returns>
        public static List<CDepart> GetDepartAllList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            List<CDepart> objList = new List<CDepart>();

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;

            if (cmdSQL == null)
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    strErr += ("Не удалось получить соединение с базой данных.");
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

            try
            {
                // соединение с БД получено, прописываем команду на выборку данных
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetDepart]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CDepart objDepart = null;
                    while (rs.Read())
                    {
                        objDepart = new CDepart();

                        objDepart.uuidID = (System.Guid)rs["Depart_Guid"];
                        objDepart.DepartCode = (System.String)rs["Depart_Code"];
                        objDepart.DepartDescription = (rs["Depart_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Depart_Description"];
                        objDepart.DepartIsActive = (rs["Depart_IsActive"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["Depart_IsActive"]);

                        if (rs["DepartTeam_Guid"] != System.DBNull.Value)
                        {
                            objDepart.DepartTeam = new CDepartTeam((System.Guid)rs["DepartTeam_Guid"], (System.String)rs["DepartTeam_Name"], (rs["DepartTeam_Description"] == System.DBNull.Value ? "" : (System.String)rs["DepartTeam_Description"]), System.Convert.ToBoolean(rs["DepartTeam_IsActive"]));
                        }
                        if (rs["Salesman_Guid"] != System.DBNull.Value)
                        {
                            objDepart.SalesMan = new CSalesMan();
                            objDepart.SalesMan.uuidID = (System.Guid)rs["Salesman_Guid"];
                            objDepart.SalesMan.ID = (System.Int32)rs["Salesman_Id"];
                            objDepart.SalesMan.Description = ((rs["Salesman_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Salesman_Description"]);
                            objDepart.SalesMan.IsActive = ((rs["Salesman_IsActive"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["Salesman_IsActive"]));

                            if (rs["User_Guid"] != System.DBNull.Value)
                            {
                                objDepart.SalesMan.User = new CUser((System.Guid)rs["User_Guid"], (System.String)rs["User_LoginName"], (System.String)rs["User_LastName"],
                                    (rs["User_FirstName"] == System.DBNull.Value ? "" : (System.String)rs["User_FirstName"]),
                                    (rs["User_MiddleName"] == System.DBNull.Value ? "" : (System.String)rs["User_MiddleName"]),
                                    (rs["User_Password"] == System.DBNull.Value ? "" : (System.String)rs["User_Password"]),
                                    (rs["User_Description"] == System.DBNull.Value ? "" : (System.String)rs["User_Description"]),
                                    (rs["User_IsActive"] == System.DBNull.Value ? false : System.Convert.ToBoolean(rs["User_IsActive"])));
                            }

                        }
                        objList.Add(objDepart);

                    }
                }

                rs.Close();
                rs.Dispose();

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    cmd = null;
                }

            }
            catch (System.Exception f)
            {
                strErr += ("Не удалось получить список подразделений. Текст ошибки: " + f.Message);
            }
			finally // очищаем занимаемые ресурсы
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return objList;
        }
        
        public static List<CDepart> GetDepartList(UniXP.Common.CProfile objProfile, System.Boolean bInitCustomerList = false)
        {
            List<CDepart> objList = new List<CDepart>();

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;

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

            try
            {
                // соединение с БД получено, прописываем команду на выборку данных
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetDepart]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CDepart objDepart = null;
                    while (rs.Read())
                    {
                        objDepart = new CDepart();

                        objDepart.uuidID = (System.Guid)rs["Depart_Guid"];
                        objDepart.DepartCode = (System.String)rs["Depart_Code"];
                        objDepart.DepartDescription = (rs["Depart_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Depart_Description"];
                        objDepart.DepartIsActive = (rs["Depart_IsActive"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["Depart_IsActive"]);

                        if (rs["DepartTeam_Guid"] != System.DBNull.Value)
                        {
                            objDepart.DepartTeam = new CDepartTeam((System.Guid)rs["DepartTeam_Guid"], (System.String)rs["DepartTeam_Name"], (rs["DepartTeam_Description"] == System.DBNull.Value ? "" : (System.String)rs["DepartTeam_Description"]), System.Convert.ToBoolean(rs["DepartTeam_IsActive"]));
                        }
                        if (rs["Salesman_Guid"] != System.DBNull.Value)
                        {
                            objDepart.SalesMan = new CSalesMan();
                            objDepart.SalesMan.uuidID = (System.Guid)rs["Salesman_Guid"];
                            objDepart.SalesMan.ID = (System.Int32)rs["Salesman_Id"];
                            objDepart.SalesMan.Description = ((rs["Salesman_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Salesman_Description"]);
                            objDepart.SalesMan.IsActive = ((rs["Salesman_IsActive"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["Salesman_IsActive"]));

                            if (rs["User_Guid"] != System.DBNull.Value)
                            {
                                objDepart.SalesMan.User = new CUser((System.Guid)rs["User_Guid"], (System.String)rs["User_LoginName"], (System.String)rs["User_LastName"],
                                    (rs["User_FirstName"] == System.DBNull.Value ? "" : (System.String)rs["User_FirstName"]),
                                    (rs["User_MiddleName"] == System.DBNull.Value ? "" : (System.String)rs["User_MiddleName"]),
                                    (rs["User_Password"] == System.DBNull.Value ? "" : (System.String)rs["User_Password"]),
                                    (rs["User_Description"] == System.DBNull.Value ? "" : (System.String)rs["User_Description"]),
                                    (rs["User_IsActive"] == System.DBNull.Value ? false : System.Convert.ToBoolean(rs["User_IsActive"])));
                            }

                        }
                        objList.Add(objDepart);

                    }
                }

                rs.Close();

                if (bInitCustomerList == true)
                {
                    foreach (CDepart objItem in objList)
                    {
                        objItem.CustomerList = CCustomer.GetCustomerListWithoutAdvancedProperties(objProfile, cmd, objItem);
                    }
                }

                rs.Dispose();

                cmd.Dispose();
                cmd = null;

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Не удалось получить список подразделений.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
			finally // очищаем занимаемые ресурсы
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return objList;
        }
        #endregion

        #region Добавить подразделение в базу данных
        /// <summary>
        /// Проверка свойств объекта перед сохранением
        /// </summary>
        /// <param name="strErr">текст с ошибкой</param>
        /// <returns>true - все свойства корректны; false - ошибка</returns>
        public System.Boolean IsAllParametersValid(ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (this.m_strDepartCode == "")
                {
                    strErr = "Необходимо указать код!";
                    return bRet;
                }
                if (this.m_objDepartTeam == null)
                {
                    strErr = "Необходимо указать команду!";
                    return bRet;
                }

                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr = "Ошибка проверки свойств. Текст ошибки: " + f.Message;
            }
            return bRet;
        }
        /// <summary>
        /// Добавляет в базу данных новое подразделение
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean AddToDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (this.IsAllParametersValid(ref strErr) == false)
                {
                    return bRet;
                }

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
                }

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddDepartCode]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Depart_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Depart_Code", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DepartTeam_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Salesman_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Depart_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Depart_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Depart_Code"].Value = this.m_strDepartCode;
                cmd.Parameters["@DepartTeam_Guid"].Value = this.m_objDepartTeam.uuidID;
                cmd.Parameters["@Depart_IsActive"].Value = this.m_bDepartIsActive;

                if (this.m_strDepartDescription != "")
                {
                    cmd.Parameters["@Depart_Description"].IsNullable = false;
                    cmd.Parameters["@Depart_Description"].Value = this.m_strDepartDescription;
                }
                else
                {
                    cmd.Parameters["@Depart_Description"].IsNullable = true;
                    cmd.Parameters["@Depart_Description"].Value = null;
                }

                if( this.m_objSalesMan != null)
                {
                    cmd.Parameters["@Salesman_Guid"].IsNullable = false;
                    cmd.Parameters["@Salesman_Guid"].Value = this.m_objSalesMan.uuidID;
                }
                else
                {
                    cmd.Parameters["@Salesman_Guid"].IsNullable = true;
                    cmd.Parameters["@Salesman_Guid"].Value = null;
                }

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    this.m_uuidID = (System.Guid)cmd.Parameters["@Depart_Guid"].Value;
                    // список назначенных клиентов
                    //iRes = (this.SaveCustomerListInDB(objProfile, cmd, ref strErr) == true) ? 0 : -1;
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
                    cmd.Dispose();
                    cmd = null;
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

        #region Редактирование свойств подразделения в БД
        /// <summary>
        /// Изменяет свойства подразделения в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean EditInDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (this.IsAllParametersValid(ref strErr) == false)
                {
                    return bRet;
                }

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
                }

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditDepartCode]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Depart_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DepartTeam_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Salesman_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Depart_Code", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Depart_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Depart_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Depart_Guid"].Value = this.m_uuidID;
                cmd.Parameters["@Depart_Code"].Value = this.m_strDepartCode;
                cmd.Parameters["@DepartTeam_Guid"].Value = this.m_objDepartTeam.uuidID;
                cmd.Parameters["@Depart_IsActive"].Value = this.m_bDepartIsActive;

                if (this.m_strDepartDescription != "")
                {
                    cmd.Parameters["@Depart_Description"].IsNullable = false;
                    cmd.Parameters["@Depart_Description"].Value = this.m_strDepartDescription;
                }
                else
                {
                    cmd.Parameters["@Depart_Description"].IsNullable = true;
                    cmd.Parameters["@Depart_Description"].Value = null;
                }

                if (this.m_objSalesMan != null)
                {
                    cmd.Parameters["@Salesman_Guid"].IsNullable = false;
                    cmd.Parameters["@Salesman_Guid"].Value = this.m_objSalesMan.uuidID;
                }
                else
                {
                    cmd.Parameters["@Salesman_Guid"].IsNullable = true;
                    cmd.Parameters["@Salesman_Guid"].Value = null;
                }

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    // список назначенных клиентов
                    iRes = (this.SaveCustomerListInDB(objProfile, cmd, ref strErr) == true) ? 0 : -1;
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
                    cmd.Dispose();
                    cmd = null;
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
        /// Сохраняет в БД список клиентов, с которыми связано подразделение
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        private System.Boolean SaveCustomerListInDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if( this.CustomerList == null ){this.CustomerList = new List<CCustomer>();}
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
                }

                System.Data.DataTable addedCategories = new System.Data.DataTable();
                addedCategories.Columns.Add(new System.Data.DataColumn("Customer_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                
                System.Data.DataRow newRow = null;
                foreach( CCustomer objCustomer in this.CustomerList )
                {
                    newRow = addedCategories.NewRow();
                    newRow[ 0 ] = objCustomer.ID;
                    addedCategories.Rows.Add(newRow);
                }
                if (this.CustomerList.Count > 0)
                {
                    addedCategories.AcceptChanges();
                }

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AssignCustomerDepart]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Depart_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.AddWithValue("@tCustomerList", addedCategories);
                cmd.Parameters["@tCustomerList"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tCustomerList"].TypeName = "dbo.udt_CustomerListForDepart";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Depart_Guid"].Value = this.uuidID;
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
                    cmd.Dispose();
                    cmd = null;
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

        #region Удаление подразделения из БД
        /// <summary>
        /// Удаление описания подразделения из БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean RemoveFromDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteDepart]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Depart_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Depart_Guid"].Value = this.m_uuidID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes == 0)
                {
                    bRet = true;
                }
                else
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
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
        #endregion

        public override string ToString()
        {
            return this.m_strDepartCode;
        }
    }
    /// <summary>
    /// Класс "Пользователь"
    /// </summary>
    public class CUser
    {
        #region Переменные, свойства
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        private System.Guid m_uuidID;
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public System.Guid uuidID
        {
            get { return m_uuidID; }
            set { m_uuidID = value; }
        }
        /// <summary>
        /// Имя
        /// </summary>
        private System.String m_strFirstName;
        /// <summary>
        /// Имя
        /// </summary>
        public System.String FirstName
        {
            get { return m_strFirstName; }
            set { m_strFirstName = value; }
        }
        /// <summary>
        /// Отчество
        /// </summary>
        private System.String m_strMiddleName;
        /// <summary>
        /// Отчество
        /// </summary>
        public System.String MiddleName
        {
            get { return m_strMiddleName; }
            set { m_strMiddleName = value; }
        }
        /// <summary>
        /// Фамилия
        /// </summary>
        private System.String m_strLastName;
        /// <summary>
        /// Фамилия
        /// </summary>
        public System.String LastName
        {
            get { return m_strLastName; }
            set { m_strLastName = value; }
        }
        /// <summary>
        /// Логин
        /// </summary>
        private System.String m_strLoginName;
        /// <summary>
        /// Логин
        /// </summary>
        public System.String LoginName
        {
            get { return m_strLoginName; }
            set { m_strLoginName = value; }
        }
        /// <summary>
        /// Пароль
        /// </summary>
        private System.String m_strPassword;
        /// <summary>
        /// Пароль
        /// </summary>
        public System.String Password
        {
            get { return m_strPassword; }
            set { m_strPassword = value; }
        }
        /// <summary>
        /// Описание
        /// </summary>
        private System.String m_strDescription;
        /// <summary>
        /// Описание
        /// </summary>
        public System.String Description
        {
            get { return m_strDescription; }
            set { m_strDescription = value; }
        }
        /// <summary>
        /// Признак "Активен"
        /// </summary>
        private System.Boolean m_IsActive;
        /// <summary>
        /// Признак "Активен"
        /// </summary>
        public System.Boolean IsActive
        {
            get { return m_IsActive; }
            set { m_IsActive = value; }
        }
        private System.Int32 m_iUserId;
        public System.Int32 iUserId
        {
            get { return m_iUserId; }
            set { m_iUserId = value; }
        }
        #endregion

        #region Конструктор
        public CUser()
        {
            m_IsActive = false;
            m_strDescription = "";
            m_strFirstName = "";
            m_strLastName = "";
            m_strMiddleName = "";
            m_strLoginName = "";
            m_strPassword = "";
            m_uuidID = System.Guid.Empty;
            m_iUserId = 0;
        }
        public CUser( System.Guid uuidID, System.String strLoginName, System.String strLastName, System.String strFirstName,
            System.String strMiddleName, System.String strPassword, System.String strDescription, System.Boolean bIsActive)
        {
            m_IsActive = bIsActive;
            m_strDescription = strDescription;
            m_strFirstName = strFirstName;
            m_strLastName = strLastName;
            m_strMiddleName = strMiddleName;
            m_strLoginName = strLoginName;
            m_strPassword = strPassword;
            m_uuidID = uuidID;
            m_iUserId = 0;
        }
        #endregion

        #region Список пользователей
        /// <summary>
        /// Возвращает список пользователей из БД (T_User)
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список пользователей</returns>
        public static List<CUser> GetUserList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CUser> objList = new List<CUser>();

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

            try
            {
                // соединение с БД получено, прописываем команду на выборку данных
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetUserList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(
                            new CUser((System.Guid)rs["User_Guid"], (System.String)rs["User_LoginName"], (System.String)rs["User_LastName"],
                                (rs["User_FirstName"] == System.DBNull.Value ? "" : (System.String)rs["User_FirstName"]),
                                (rs["User_MiddleName"] == System.DBNull.Value ? "" : (System.String)rs["User_MiddleName"]),
                                (rs["User_Password"] == System.DBNull.Value ? "" : (System.String)rs["User_Password"]),
                                (rs["User_Description"] == System.DBNull.Value ? "" : (System.String)rs["User_Description"]),
                                (rs["User_IsActive"] == System.DBNull.Value ? false : System.Convert.ToBoolean(rs["User_IsActive"])))
                                );
                    }
                }

                rs.Close();
                rs.Dispose();

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    cmd = null;
                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Не удалось получить список пользователей.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
			finally // очищаем занимаемые ресурсы
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return objList;
        }
        #endregion

        #region Добавить пользователя в базу данных
        /// <summary>
        /// Проверка свойств объекта перед сохранением
        /// </summary>
        /// <param name="strErr">текст с ошибкой</param>
        /// <returns>true - все свойства корректны; false - ошибка</returns>
        public System.Boolean IsAllParametersValid(ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (this.m_strLoginName == "")
                {
                    strErr = "Необходимо указать логин!";
                    return bRet;
                }

                if (this.m_strLastName == "")
                {
                    strErr = "Необходимо указать фамилию!";
                    return bRet;
                }

                if (this.m_strPassword == "")
                {
                    strErr = "Необходимо указать пароль!";
                    return bRet;
                }

                if (this.m_strFirstName == "")
                {
                    strErr = "Необходимо указать имя!";
                    return bRet;
                }

                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr = "Ошибка проверки свойств. Текст ошибки: " + f.Message;
            }
            return bRet;
        }
        /// <summary>
        /// Добавляет в базу данных нового пользователя
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean AddToDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (this.IsAllParametersValid(ref strErr) == false)
                {
                    return bRet;
                }

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
                }

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddUserInfo]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@User_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@User_FirstName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@User_MiddleName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@User_LastName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@User_LoginName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@User_Password", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@User_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@User_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@User_FirstName"].Value = this.m_strFirstName;
                cmd.Parameters["@User_MiddleName"].Value = this.m_strMiddleName;
                cmd.Parameters["@User_LastName"].Value = this.m_strLastName;
                cmd.Parameters["@User_LoginName"].Value = this.m_strLoginName;
                cmd.Parameters["@User_Password"].Value = this.m_strPassword;
                cmd.Parameters["@User_IsActive"].Value = this.m_IsActive;

                if (this.m_strDescription != "")
                {
                    cmd.Parameters["@User_Description"].IsNullable = false;
                    cmd.Parameters["@User_Description"].Value = this.m_strDescription;
                }
                else
                {
                    cmd.Parameters["@User_Description"].IsNullable = true;
                    cmd.Parameters["@User_Description"].Value = null;
                }

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    this.m_uuidID = (System.Guid)cmd.Parameters["@User_Guid"].Value;
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
                    cmd.Dispose();
                    cmd = null;
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

        #region Редактирование свойств пользователя в БД
        /// <summary>
        /// Изменяет свойства пользователя в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean EditInDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (this.IsAllParametersValid(ref strErr) == false)
                {
                    return bRet;
                }

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
                }

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditUser]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@User_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@User_FirstName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@User_MiddleName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@User_LastName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@User_LoginName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@User_Password", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@User_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@User_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@User_Guid"].Value = this.m_uuidID;
                cmd.Parameters["@User_FirstName"].Value = this.m_strFirstName;
                cmd.Parameters["@User_MiddleName"].Value = this.m_strMiddleName;
                cmd.Parameters["@User_LastName"].Value = this.m_strLastName;
                cmd.Parameters["@User_LoginName"].Value = this.m_strLoginName;
                cmd.Parameters["@User_Password"].Value = this.m_strPassword;
                cmd.Parameters["@User_IsActive"].Value = this.m_IsActive;

                if (this.m_strDescription != "")
                {
                    cmd.Parameters["@User_Description"].IsNullable = false;
                    cmd.Parameters["@User_Description"].Value = this.m_strDescription;
                }
                else
                {
                    cmd.Parameters["@User_Description"].IsNullable = true;
                    cmd.Parameters["@User_Description"].Value = null;
                }

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
                    cmd.Dispose();
                    cmd = null;
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

        #region Удаление пользователя из БД
        /// <summary>
        /// Удаление описания пользователя из БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean RemoveFromDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteUser]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@User_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@User_Guid"].Value = this.m_uuidID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes == 0)
                {
                    bRet = true;
                }
                else
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
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
        #endregion

        public override string ToString()
        {
            return ( this.m_strLastName + " " + this.m_strFirstName );
        }
    }
    /// <summary>
    /// Класс "Торговый представитель"
    /// </summary>
    public class CSalesMan
    {
        #region Переменные, свойства
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        private System.Guid m_uuidID;
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public System.Guid uuidID
        {
            get { return m_uuidID; }
            set { m_uuidID = value; }
        }
        /// <summary>
        /// Уникальный идентификатор IB
        /// </summary>
        private System.Int32 m_iID;
        /// <summary>
        /// Уникальный идентификатор IB
        /// </summary>
        public System.Int32 ID
        {
            get { return m_iID; }
            set { m_iID = value; }
        }
        /// <summary>
        /// Пользователь
        /// </summary>
        private CUser m_objUser;
        /// <summary>
        /// Пользователь
        /// </summary>
        public CUser User
        {
            get { return m_objUser; }
            set { m_objUser = value; }
        }
        /// <summary>
        /// Описание
        /// </summary>
        private System.String m_strDescription;
        /// <summary>
        /// Описание
        /// </summary>
        public System.String Description
        {
            get { return m_strDescription; }
            set { m_strDescription = value; }
        }
        /// <summary>
        /// Признак "Активен"
        /// </summary>
        private System.Boolean m_IsActive;
        /// <summary>
        /// Признак "Активен"
        /// </summary>
        public System.Boolean IsActive
        {
            get { return m_IsActive; }
            set { m_IsActive = value; }
        }
        #endregion

        #region Конструктор
        public CSalesMan()
        {
            m_uuidID = System.Guid.Empty;
            m_iID = 0;
            m_objUser = null;
            m_strDescription = "";
            m_IsActive = false;
        }
        public CSalesMan(System.Guid uuidID, System.Int32 iID, CUser objUser, System.String strDscrpn, System.Boolean bActive)
        {
            m_uuidID = uuidID;
            m_iID = iID;
            m_objUser = objUser;
            m_strDescription = strDscrpn;
            m_IsActive = bActive;
        }
        #endregion

        #region Список торговых представителей
        public static List<CSalesMan> GetSalesManList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CSalesMan> objList = new List<CSalesMan>();

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

            try
            {
                // соединение с БД получено, прописываем команду на выборку данных
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetSalesman]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CSalesMan objSalesMan = null;
                    while (rs.Read())
                    {
                        objSalesMan = new CSalesMan();
                        objSalesMan.uuidID = (System.Guid)rs["Salesman_Guid"];
                        objSalesMan.ID = (System.Int32)rs["Salesman_Id"];
                        objSalesMan.IsActive = ( rs["Salesman_IsActive"] == System.DBNull.Value ) ? false : System.Convert.ToBoolean(rs["Salesman_IsActive"]);
                        objSalesMan.Description = ( rs["Salesman_Description"] == System.DBNull.Value ) ? "" : (System.String)rs["Salesman_Description"];

                        if( rs["User_Guid"] != System.DBNull.Value )
                        {
                            objSalesMan.User = new CUser((System.Guid)rs["User_Guid"], (System.String)rs["User_LoginName"], (System.String)rs["User_LastName"],
                                ( rs["User_FirstName"] == System.DBNull.Value ? "" : (System.String)rs["User_FirstName"]),
                                ( rs["User_MiddleName"] == System.DBNull.Value ? "" : (System.String)rs["User_MiddleName"] ),
                                ( rs["User_Password"] == System.DBNull.Value ? "" : (System.String)rs["User_Password"]),
                                ( rs["User_Description"] == System.DBNull.Value ? "" : (System.String)rs["User_Description"]),
                                ( rs["User_IsActive"] == System.DBNull.Value ? false : System.Convert.ToBoolean(rs["User_IsActive"])));
                        }

                        objList.Add(objSalesMan);
                    }
                }

                rs.Close();
                rs.Dispose();

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    cmd = null;
                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Не удалось получить список торговых представителей.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
			finally // очищаем занимаемые ресурсы
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return objList;
        }
        /// <summary>
        /// Возвращает список торговых представителей для указанного подразделения
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="Depart_Guid">УИ торгового подразделения</param>
        /// <param name="strErr">текст ошибки</param>
        /// <returns>список объектов класса "CSalesMan"</returns>
        public static List<CSalesMan> GetSalesManListForDepart(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid Depart_Guid, ref System.String strErr )
        {
            List<CSalesMan> objList = new List<CSalesMan>();

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;

            if (cmdSQL == null)
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    strErr += ("\nНе удалось получить соединение с базой данных.");
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

            try
            {
                // соединение с БД получено, прописываем команду на выборку данных
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetSalesman]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Depart_Guid", System.Data.DbType.Guid));
                cmd.Parameters["@Depart_Guid"].Value = Depart_Guid;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CSalesMan objSalesMan = null;
                    while (rs.Read())
                    {
                        objSalesMan = new CSalesMan();
                        objSalesMan.uuidID = (System.Guid)rs["Salesman_Guid"];
                        objSalesMan.ID = (System.Int32)rs["Salesman_Id"];
                        objSalesMan.IsActive = (rs["Salesman_IsActive"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["Salesman_IsActive"]);
                        objSalesMan.Description = (rs["Salesman_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Salesman_Description"];

                        if (rs["User_Guid"] != System.DBNull.Value)
                        {
                            objSalesMan.User = new CUser((System.Guid)rs["User_Guid"], (System.String)rs["User_LoginName"], (System.String)rs["User_LastName"],
                                (rs["User_FirstName"] == System.DBNull.Value ? "" : (System.String)rs["User_FirstName"]),
                                (rs["User_MiddleName"] == System.DBNull.Value ? "" : (System.String)rs["User_MiddleName"]),
                                (rs["User_Password"] == System.DBNull.Value ? "" : (System.String)rs["User_Password"]),
                                (rs["User_Description"] == System.DBNull.Value ? "" : (System.String)rs["User_Description"]),
                                (rs["User_IsActive"] == System.DBNull.Value ? false : System.Convert.ToBoolean(rs["User_IsActive"])));
                        }

                        objList.Add(objSalesMan);
                    }
                }

                rs.Close();
                rs.Dispose();

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    cmd = null;
                }

            }
            catch (System.Exception f)
            {
                strErr += ("\nНе удалось получить список торговых представителей.\n\nТекст ошибки: " + f.Message);
            }
			finally // очищаем занимаемые ресурсы
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
            }
            return objList;
        }

        #endregion

        #region Добавить торгового представителя в базу данных
        /// <summary>
        /// Проверка свойств контакта перед сохранением
        /// </summary>
        /// <param name="strErr">текст с ошибкой</param>
        /// <returns>true - все свойства корректны; false - ошибка</returns>
        public System.Boolean IsAllParametersValid(ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (this.m_objUser == null)
                {
                    strErr = "Необходимо указать пользователя!";
                    return bRet;
                }

                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr = "Ошибка проверки свойств. Текст ошибки: " + f.Message;
            }
            return bRet;
        }
        /// <summary>
        /// Добавляет в базу данных нового торгового представителя
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean AddToDB( UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (this.IsAllParametersValid(ref strErr) == false)
                {
                    return bRet;
                }

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
                }

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddSalesman]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Salesman_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Salesman_Id", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@User_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Salesman_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Salesman_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@User_Guid"].Value = this.m_objUser.uuidID;
                cmd.Parameters["@Salesman_IsActive"].Value = this.m_IsActive;

                if (this.m_strDescription != "")
                {
                    cmd.Parameters["@Salesman_Description"].IsNullable = false;
                    cmd.Parameters["@Salesman_Description"].Value = this.m_strDescription;
                }
                else
                {
                    cmd.Parameters["@Salesman_Description"].IsNullable = true;
                    cmd.Parameters["@Salesman_Description"].Value = null;
                }

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    this.m_uuidID = (System.Guid)cmd.Parameters["@Salesman_Guid"].Value;
                    this.m_iID = (System.Int32)cmd.Parameters["@Salesman_Id"].Value; 
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
                    cmd.Dispose();
                    cmd = null;
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

        #region Редактирование свойств торгового представителя в БД
        /// <summary>
        /// Изменяет свойства торгового представителя в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean EditInDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (this.IsAllParametersValid(ref strErr) == false)
                {
                    return bRet;
                }

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
                }

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditSalesman]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Salesman_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@User_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Salesman_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Salesman_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Salesman_Guid"].Value = this.m_uuidID;
                cmd.Parameters["@User_Guid"].Value = this.m_objUser.uuidID;
                cmd.Parameters["@Salesman_IsActive"].Value = this.m_IsActive;

                if (this.m_strDescription != "")
                {
                    cmd.Parameters["@Salesman_Description"].IsNullable = false;
                    cmd.Parameters["@Salesman_Description"].Value = this.m_strDescription;
                }
                else
                {
                    cmd.Parameters["@Salesman_Description"].IsNullable = true;
                    cmd.Parameters["@Salesman_Description"].Value = null;
                }

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
                    cmd.Dispose();
                    cmd = null;
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

        #region Удаление торгового представителя из БД
        /// <summary>
        /// Удаление описания торгового представителя из БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean RemoveFromDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteSalesman]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Salesman_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Salesman_Guid"].Value = this.m_uuidID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes == 0)
                {
                    bRet = true;
                }
                else
                {
                    strErr = System.Convert.ToString(cmd.Parameters["@ERROR_MES"].Value);
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
        #endregion

        public override string ToString()
        {
            return ( ( this.m_objUser == null ) ? "" : ( this.m_objUser.LastName + " " + this.m_objUser.FirstName ) );
        }
    }
}
