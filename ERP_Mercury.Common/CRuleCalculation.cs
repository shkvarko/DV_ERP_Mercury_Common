using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP_Mercury.Common
{
    /// <summary>
    /// Класс "Тип правила"
    /// </summary>
    public class CRuleType : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// описание
        /// </summary>
        private System.String m_Description;
        /// <summary>
        /// описание
        /// </summary>
        public System.String Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        #endregion

        #region Конструктор
        public CRuleType()
            : base()
        {
            m_Description = "";
        }
        public CRuleType(System.Guid uuidID, System.String strName, System.String strDescription)
        {
            ID = uuidID;
            Name = strName;
            m_Description = strDescription;
        }
        #endregion

        #region Список типов правил
        public static List<CRuleType> GetRuleTypeList(
            UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL )
        {
            List<CRuleType> objList = new List<CRuleType>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetRuleType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CRuleType((System.Guid)rs["RuleType_Guid"], (System.String)rs["RuleType_Name"],
                            ((rs["RuleType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["RuleType_Description"])));
                    }
                }
                rs.Dispose();
                if( cmdSQL == null )
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список типов правил.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        #endregion

        #region Add
        /// <summary>
        /// Добавить запись в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public override System.Boolean Add(UniXP.Common.CProfile objProfile)
        {
            System.Boolean bRet = false;
            if (IsAllParametersValid() == false) { return bRet; }

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            System.String strErr = "";
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Не удалось получить соединение с базой данных.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return bRet;
                }
                DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddRuleType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RuleType_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RuleType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RuleType_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RuleType_Name"].Value = this.Name;
                cmd.Parameters["@RuleType_Description"].Value = this.m_Description;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@RuleType_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания типа правила.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать тип правила.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        #endregion

        #region Remove
        /// <summary>
        /// Удалить запись из БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="uuidID">уникальный идентификатор объекта</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public override System.Boolean Remove(UniXP.Common.CProfile objProfile)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Не удалось получить соединение с базой данных.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return bRet;
                }
                DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteRuleType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RuleType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RuleType_Guid"].Value = this.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                bRet = (iRes == 0);


                if (bRet == true)
                {
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    // откатываем транзакцию
                    DBTransaction.Rollback();
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления типа правила.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить тип правила.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }

        #endregion

        #region Update
        /// <summary>
        /// Сохранить изменения в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public override System.Boolean Update(UniXP.Common.CProfile objProfile)
        {
            System.Boolean bRet = false;
            if (IsAllParametersValid() == false) { return bRet; }

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            System.String strErr = "";
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Не удалось получить соединение с базой данных.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return bRet;
                }
                DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditRuleType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RuleType_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RuleType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RuleType_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RuleType_Guid"].Value = this.ID;
                cmd.Parameters["@RuleType_Name"].Value = this.Name;
                cmd.Parameters["@RuleType_Description"].Value = this.m_Description;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения типа правила.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства типа правила.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }

        #endregion

        public override string ToString()
        {
            return Name;
        }

    }
    /// <summary>
    /// Класс "Хранимая процедура для правила"
    /// </summary>
    public class CStoredProcedure : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// описание
        /// </summary>
        private System.String m_Description;
        /// <summary>
        /// описание
        /// </summary>
        public System.String Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        #endregion

        #region Конструктор
        public CStoredProcedure()
            : base()
        {
            m_Description = "";
        }
        public CStoredProcedure(System.Guid uuidID, System.String strName, System.String strDescription)
        {
            ID = uuidID;
            Name = strName;
            m_Description = strDescription;
        }
        #endregion

        #region Список хранимых процедур
        /// <summary>
        /// Возвращает список зарегистрированных хранимых процедур
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <returns>список зарегистрированных хранимых процедур</returns>
        public static List<CStoredProcedure> GetStoredProcedureList(
            UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CStoredProcedure> objList = new List<CStoredProcedure>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetRuleCalculationStoredProcedureList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.String strDscrpn = "";
                    while (rs.Read())
                    {
                        strDscrpn = (rs["StoredProcedure_Description"] == System.DBNull.Value) ? "" : (System.String)rs["StoredProcedure_Description"];
                        objList.Add(new CStoredProcedure((System.Guid)rs["StoredProcedure_Guid"], (System.String)rs["StoredProcedure_Name"], strDscrpn));
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
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список хранимых процедур.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }

        #endregion

        #region Add
        /// <summary>
        /// Добавить запись в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public override System.Boolean Add(UniXP.Common.CProfile objProfile)
        {
            System.Boolean bRet = false;
            if (IsAllParametersValid() == false) { return bRet; }

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            System.String strErr = "";
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Не удалось получить соединение с базой данных.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return bRet;
                }
                DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddRuleCalculationStoredProcedure]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@StoredProcedure_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@StoredProcedure_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@StoredProcedure_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@StoredProcedure_Name"].Value = this.Name;
                cmd.Parameters["@StoredProcedure_Description"].Value = this.m_Description;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@StoredProcedure_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка регистрации хранимой процедуры.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось зарегистрировать хранимую процедуру.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        #endregion

        #region Remove
        /// <summary>
        /// Удалить запись из БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="uuidID">уникальный идентификатор объекта</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public override System.Boolean Remove(UniXP.Common.CProfile objProfile)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Не удалось получить соединение с базой данных.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return bRet;
                }
                DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteRuleCalculationStoredProcedure]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@StoredProcedure_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@StoredProcedure_Guid"].Value = this.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                bRet = (iRes == 0);


                if (bRet == true)
                {
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    // откатываем транзакцию
                    DBTransaction.Rollback();
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления регистрации хранимой процедуры.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить регистрацию хранимой процедуры.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }

        #endregion

        #region Update
        /// <summary>
        /// Сохранить изменения в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public override System.Boolean Update(UniXP.Common.CProfile objProfile)
        {
            System.Boolean bRet = false;
            if (IsAllParametersValid() == false) { return bRet; }

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            System.String strErr = "";
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Не удалось получить соединение с базой данных.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return bRet;
                }
                DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditRuleCalculationStoredProcedure]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@StoredProcedure_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@StoredProcedure_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@StoredProcedure_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@StoredProcedure_Guid"].Value = this.ID;
                cmd.Parameters["@StoredProcedure_Name"].Value = this.Name;
                cmd.Parameters["@StoredProcedure_Description"].Value = this.m_Description;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения регистрации хранимой процедуры.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить регистрацию хранимой процедуры.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }

        #endregion

        public override string ToString()
        {
            return Name;
        }
    }
    /// <summary>
    /// Класс "Тип данных для дополнительного параметра хранимой процедуры"
    /// </summary>
    public class CParamDataType
    {
        #region Свойства
        /// <summary>
        /// уникальный идентификатор
        /// </summary>
        private System.Int32 m_iID;
        /// <summary>
        /// уникальный идентификатор
        /// </summary>
        public System.Int32 ID
        {
            get { return m_iID; }
        }
        /// <summary>
        /// наименование
        /// </summary>
        private System.String m_Name;
        /// <summary>
        /// наименование
        /// </summary>
        public System.String Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        #endregion

        #region Конструктор
        public CParamDataType(System.Int32 iID, System.String strName)
        {
            m_iID = iID;
            m_Name = strName;
        }
        #endregion

        public override string ToString()
        {
            return Name;
        }
    }
    /// <summary>
    /// Класс "Дополнительный параметр  для вычисления цены"
    /// </summary>
    public class CAdvancedParam
    {
        #region Свойства
        /// <summary>
        /// уникальный идентификатор
        /// </summary>
        private System.Guid m_uuidID;
        /// <summary>
        /// уникальный идентификатор
        /// </summary>
        public System.Guid ID
        {
            get { return m_uuidID; }
        }
        /// <summary>
        /// наименование
        /// </summary>
        private System.String m_Name;
        /// <summary>
        /// наименование
        /// </summary>
        public System.String Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        /// <summary>
        /// описание
        /// </summary>
        private System.String m_Description;
        /// <summary>
        /// описание
        /// </summary>
        public System.String Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        /// <summary>
        /// Тип данных
        /// </summary>
        private CParamDataType m_objParamDataType;
        /// <summary>
        /// Тип данных
        /// </summary>
        public CParamDataType DataType
        {
            get { return m_objParamDataType; }
            set { m_objParamDataType = value; }
        }
        /// <summary>
        /// Значение
        /// </summary>
        private System.String m_strValue;
        /// <summary>
        /// Значение
        /// </summary>
        public System.String Value
        {
            get { return m_strValue; }
            set { m_strValue = value; }
        }
        private System.String m_strGroupName;
        public System.String GroupName
        {
            get { return m_strGroupName; }
            set { m_strGroupName = value; }
        }

        #endregion

        #region Конструктор
        public CAdvancedParam(System.Guid uuidID, System.String strName,
            System.String strDescription, CParamDataType objParamDataType)
        {
            m_uuidID = uuidID;
            m_Name = strName;
            m_Description = strDescription;
            m_objParamDataType = objParamDataType;
            m_strValue = "";
            m_strGroupName = "";
        }
        #endregion

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// Класс "Правило (действие)"
    /// </summary>
    public class CRuleCalculation
    {
        #region Свойства
        /// <summary>
        /// уникальный идентификатор
        /// </summary>
        private System.Guid m_uuidID;
        /// <summary>
        /// уникальный идентификатор
        /// </summary>
        public System.Guid ID
        {
            get { return m_uuidID; }
            set { m_uuidID = value; }
        }
        /// <summary>
        /// наименование
        /// </summary>
        private System.String m_Name;
        /// <summary>
        /// наименование
        /// </summary>
        public System.String Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        /// <summary>
        /// описание
        /// </summary>
        private System.String m_Description;
        /// <summary>
        /// описание
        /// </summary>
        public System.String Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }
        /// <summary>
        /// Тип правила
        /// </summary>
        private CRuleType m_objRuleType;
        /// <summary>
        /// Тип правила
        /// </summary>
        public CRuleType RuleType
        {
            get { return m_objRuleType; }
            set { m_objRuleType = value; }
        }

        /// <summary>
        /// Хранимая процедура
        /// </summary>
        private CStoredProcedure m_objStoredProcedure;
        /// <summary>
        /// Хранимая процедура
        /// </summary>
        public CStoredProcedure StoredProcedure
        {
            get { return m_objStoredProcedure; }
            set { m_objStoredProcedure = value; }
        }
        /// <summary>
        /// Список дополнительных параметров
        /// </summary>
        private List<CAdvancedParam> m_objAdvancedParamList;
        /// <summary>
        /// Список дополнительных параметров
        /// </summary>
        public List<CAdvancedParam> AdvancedParamList
        {
            get { return m_objAdvancedParamList; }
            set { m_objAdvancedParamList = value; }
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
        public CRuleCalculation()
        {
            m_uuidID = System.Guid.Empty;
            m_Name = "";
            m_Description = "";
            m_objStoredProcedure = null;
            m_objRuleType = null;
            m_objAdvancedParamList = null;
            m_xmldocAdvancedParamList = null;
        }
        public CRuleCalculation(System.Guid uuidID, System.String strName, System.String strDescription,
            CStoredProcedure objStoredProcedure, CRuleType objRuleType)
        {
            m_uuidID = uuidID;
            m_Name = strName;
            m_Description = strDescription;
            m_objStoredProcedure = objStoredProcedure;
            m_objRuleType = objRuleType;
            m_objAdvancedParamList = null;
            m_xmldocAdvancedParamList = null;
        }
        #endregion

        #region Список правил
        /// <summary>
        /// Возвращает список правил
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список правил</returns>
        public static List<CRuleCalculation> GetRuleCalculationList(
            UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CRuleCalculation> objList = new List<CRuleCalculation>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetRuleCalculation]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.String strDscrpn = "";
                    System.String strDscrpnSP = "";
                    System.String strRuleTypeDscrpn = "";
                    while (rs.Read())
                    {
                        strDscrpn = (rs["RuleCalculation_Description"] == System.DBNull.Value) ? "" : (System.String)rs["RuleCalculation_Description"];
                        strDscrpnSP = (rs["StoredProcedure_Description"] == System.DBNull.Value) ? "" : (System.String)rs["StoredProcedure_Description"];
                        strRuleTypeDscrpn = (rs["RuleType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["RuleType_Description"];

                        objList.Add(new CRuleCalculation((System.Guid)rs["RuleCalculation_Guid"],
                            (System.String)rs["RuleCalculation_Name"], strDscrpn,
                            new CStoredProcedure((System.Guid)rs["StoredProcedure_Guid"], (System.String)rs["StoredProcedure_Name"], strDscrpnSP),
                            new CRuleType((System.Guid)rs["RuleType_Guid"], (System.String)rs["RuleType_Name"], strRuleTypeDscrpn)));
                    }
                }
                rs.Dispose();
                // а теперь нужно загрузить список параметров для каждого правила
                foreach (CRuleCalculation objRuleCalculation in objList)
                {
                    objRuleCalculation.LoadParamList2(objProfile, cmd, objRuleCalculation.StoredProcedure.Name);
                }

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список правил.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        public System.Boolean LoadParamList2(
            UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, System.String strSPName)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (this.m_objAdvancedParamList == null)
                {
                    this.m_objAdvancedParamList = new List<CAdvancedParam>();
                }
                this.m_objAdvancedParamList.Clear();

                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                            "Не удалось получить соединение с базой данных.", "Внимание",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                CAdvancedParam objAdvParam = null;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetInfoAboutAdvParams]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@StoredProcedure_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SPParamsInfo", System.Data.SqlDbType.Xml, 8));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@SPParamsInfo"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@StoredProcedure_Name"].Value = strSPName;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    System.Xml.XmlDocument docInfoAboutParam = new System.Xml.XmlDocument();
                    docInfoAboutParam.LoadXml((System.String)cmd.Parameters["@SPParamsInfo"].Value);
                    if ((docInfoAboutParam != null) && (docInfoAboutParam.ChildNodes.Count > 0))
                    {
                        this.m_xmldocAdvancedParamList = docInfoAboutParam;
                        foreach (System.Xml.XmlNode objNode in docInfoAboutParam.ChildNodes)
                        {
                            if (objNode.Name == "SP_Param")
                            {
                                foreach (System.Xml.XmlNode objChildNode in objNode.ChildNodes)
                                {
                                    if (objChildNode.Name == "Param")
                                    {
                                        // мы добрались до списка элементов, описывающих параметры процедуры
                                        objAdvParam = new CAdvancedParam(System.Guid.Empty, objChildNode.Attributes["Name"].Value, "", new CParamDataType(0, objChildNode.Attributes["Type"].Value));
                                        this.m_objAdvancedParamList.Add(objAdvParam);
                                    }
                                }
                            }
                        }
                    }
                }

                bRet = true;
                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список параметров для правила.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return bRet;
        }
        #endregion

        #region Добавить новое правило в базу данных
        /// <summary>
        /// Проверка свойств правила перед сохранением в базе данных
        /// </summary>
        /// <returns>true - все свойства определены; false - не все свойства определены</returns>
        private System.Boolean IsAllParametersValid()
        {
            System.Boolean bRet = false;
            try
            {
                if (this.Name == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать имя правила.", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.StoredProcedure == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать хранимую процедуру, связанную с правилом.", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.m_objRuleType == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать тип правила.", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                bRet = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка проверки свойств правила.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return bRet;
        }
        /// <summary>
        /// добавляет новое правило в базу данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean AddRuleCalculationToDB(UniXP.Common.CProfile objProfile)
        {
            System.Boolean bRet = false;
            if (IsAllParametersValid() == false) { return bRet; }

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            System.String strErr = "";
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Не удалось получить соединение с базой данных.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return bRet;
                }
                DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddRuleCalculation]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RuleCalculation_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@StoredProcedure_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RuleType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RuleCalculation_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                if (this.Description != "")
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RuleCalculation_Description", System.Data.DbType.String));
                    cmd.Parameters["@RuleCalculation_Description"].Value = this.Description;
                }
                cmd.Parameters["@StoredProcedure_Guid"].Value = this.StoredProcedure.ID;
                cmd.Parameters["@RuleType_Guid"].Value = this.RuleType.ID;
                cmd.Parameters["@RuleCalculation_Name"].Value = this.Name;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.m_uuidID = (System.Guid)cmd.Parameters["@RuleCalculation_Guid"].Value;
                    bRet = true; 
                    if (bRet == true)
                    {
                        // подтверждаем транзакцию
                        DBTransaction.Commit();
                    }
                    else
                    {
                        // откатываем транзакцию
                        DBTransaction.Rollback();
                        DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания правила.\n\nТекст ошибки: " + strErr, "Ошибка",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                else
                {
                    DBTransaction.Rollback();
                    System.String strErrText = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания правила.\n\nТекст ошибки: " + strErrText, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать правилo.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        #endregion

        #region Изменить свойства правила в базе данных
        /// <summary>
        /// изменяет свойства правила в базе данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="bSaveGroupList">признак "сохранить изменения в списке групп"</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean EditRuleCalculationInDB(UniXP.Common.CProfile objProfile, System.Boolean bSaveGroupList)
        {
            System.Boolean bRet = false;
            if (IsAllParametersValid() == false) { return bRet; }

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            System.String strErr = "";
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Не удалось получить соединение с базой данных.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return bRet;
                }
                DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditRuleCalculation]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RuleCalculation_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@StoredProcedure_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RuleType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RuleCalculation_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                if (this.Description != "")
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RuleCalculation_Description", System.Data.DbType.String));
                    cmd.Parameters["@RuleCalculation_Description"].Value = this.Description;
                }
                cmd.Parameters["@RuleCalculation_Guid"].Value = this.ID;
                cmd.Parameters["@StoredProcedure_Guid"].Value = this.StoredProcedure.ID;
                cmd.Parameters["@RuleType_Guid"].Value = this.RuleType.ID;
                cmd.Parameters["@RuleCalculation_Name"].Value = this.Name;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes == 0)
                {
                    bRet = true; // (bSaveGroupList == true) ? this.SaveConditionGroupList(objProfile, cmd, ref strErr) : true;

                    if (bRet == true)
                    {
                        // подтверждаем транзакцию
                        DBTransaction.Commit();
                    }
                    else
                    {
                        // откатываем транзакцию
                        DBTransaction.Rollback();
                        DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств правила.\n\nТекст ошибки: " + strErr, "Ошибка",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                else
                {
                    DBTransaction.Rollback();
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств правила.\n\nТекст ошибки: " +
                        (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства правила.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        #endregion

        #region Удалить правило из базы данных
        /// <summary>
        /// Удаляет группу из базы данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmd">SQL-команда</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        private System.Boolean DeleteRuleCalculation(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmd, ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (cmd == null)
                {
                    strErr = "Не удалось получить соединение с базой данных.";
                    return bRet;
                }
                cmd.Parameters.Clear();

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteRuleCalculation]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RuleCalculation_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RuleCalculation_Guid"].Value = this.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                strErr = "Не удалось удалить правило.\n\nТекст ошибки: " + f.Message;
                //DevExpress.XtraEditors.XtraMessageBox.Show(
                //"Не удалось удалить правило.\n\nТекст ошибки: " + f.Message, "Внимание",
                //System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return bRet;
        }
        /// <summary>
        /// Удаляет сведения о правиле из базы данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean DeleteRuleCalculationFromDB(UniXP.Common.CProfile objProfile)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            System.String strErr = "";
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Не удалось получить соединение с базой данных.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return bRet;
                }
                DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                bRet = this.DeleteRuleCalculation(objProfile, cmd, ref strErr);

                if (bRet == true)
                {
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    // откатываем транзакцию
                    DBTransaction.Rollback();
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления правила.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить правило.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        #endregion

        public override string ToString()
        {
            return Name;
        }
    }

    #region Заказ

    //public enum enPDASupplState
    //{
    //    Unkown = -1,
    //    Created = 0,
    //    Deleted = 1,
    //    Transfered = 2,
    //    Processed = 3,
    //    CreditControl = 4,
    //    General = 5,
    //    CalcPricesFalse = 7,
    //    CreateSupplInIBFalse = 8,
    //    FindStock = 10,
    //    CalcPricesOk = 60,
    //    AutoCalcPricesOk = 70
    //}

    //public class CPDASupplItms
    //{
    //    #region Свойства
    //    /// <summary>
    //    /// Наименование товарной марки
    //    /// </summary>
    //    private System.String m_strProductOwnerName;
    //    /// <summary>
    //    /// Наименование товарной марки
    //    /// </summary>
    //    public System.String ProductOwnerName
    //    {
    //        get { return m_strProductOwnerName; }
    //        set { m_strProductOwnerName = value; }
    //    }
    //    /// <summary>
    //    /// Наименование товара
    //    /// </summary>
    //    private System.String m_strProductName;
    //    /// <summary>
    //    /// Наименование товара
    //    /// </summary>
    //    public System.String ProductName
    //    {
    //        get { return m_strProductName; }
    //        set { m_strProductName = value; }
    //    }
    //    /// <summary>
    //    /// Цена, руб.
    //    /// </summary>
    //    private System.Double m_Price;
    //    /// <summary>
    //    /// Цена, руб.
    //    /// </summary>
    //    public System.Double Price
    //    {
    //        get { return m_Price; }
    //        set { m_Price = value; }
    //    }
    //    /// <summary>
    //    /// Цена, eur
    //    /// </summary>
    //    private System.Double m_CurrencyPrice;
    //    /// <summary>
    //    /// Цена, eur
    //    /// </summary>
    //    public System.Double CurrencyPrice
    //    {
    //        get { return m_CurrencyPrice; }
    //        set { m_CurrencyPrice = value; }
    //    }
    //    /// <summary>
    //    /// Цена со скидкой, руб.
    //    /// </summary>
    //    private System.Double m_DiscountPrice;
    //    /// <summary>
    //    /// Цена со скидкой, руб.
    //    /// </summary>
    //    public System.Double DiscountPrice
    //    {
    //        get { return m_DiscountPrice; }
    //        set { m_DiscountPrice = value; }
    //    }
    //    /// <summary>
    //    /// Цена со скидкой, eur
    //    /// </summary>
    //    private System.Double m_CurrencyDiscountPrice;
    //    /// <summary>
    //    /// Цена со скидкой, eur
    //    /// </summary>
    //    public System.Double CurrencyDiscountPrice
    //    {
    //        get { return m_CurrencyDiscountPrice; }
    //        set { m_CurrencyDiscountPrice = value; }
    //    }
    //    /// <summary>
    //    /// Сумма, руб.
    //    /// </summary>
    //    private System.Double m_AllPrice;
    //    /// <summary>
    //    /// Сумма, руб.
    //    /// </summary>
    //    public System.Double AllPrice
    //    {
    //        get { return m_AllPrice; }
    //        set { m_AllPrice = value; }
    //    }
    //    /// <summary>
    //    /// Сумма, eur.
    //    /// </summary>
    //    private System.Double m_AllCurrencyPrice;
    //    /// <summary>
    //    /// Сумма, eur.
    //    /// </summary>
    //    public System.Double AllCurrencyPrice
    //    {
    //        get { return m_AllCurrencyPrice; }
    //        set { m_AllCurrencyPrice = value; }
    //    }
    //    /// <summary>
    //    /// Скидка, %.
    //    /// </summary>
    //    private System.Double m_DiscountPercent;
    //    /// <summary>
    //    /// Скидка, %.
    //    /// </summary>
    //    public System.Double DiscountPercent
    //    {
    //        get { return m_DiscountPercent; }
    //        set { m_DiscountPercent = value; }
    //    }
    //    /// <summary>
    //    /// Сумма со скидкой, руб
    //    /// </summary>
    //    private System.Double m_TotalPrice;
    //    /// <summary>
    //    /// Сумма со скидкой, руб
    //    /// </summary>
    //    public System.Double TotalPrice
    //    {
    //        get { return m_TotalPrice; }
    //        set { m_TotalPrice = value; }
    //    }
    //    /// <summary>
    //    /// Сумма со скидкой, eur
    //    /// </summary>
    //    private System.Double m_TotalCurrencyPrice;
    //    /// <summary>
    //    /// Сумма со скидкой, eur
    //    /// </summary>
    //    public System.Double TotalCurrencyPrice
    //    {
    //        get { return m_TotalCurrencyPrice; }
    //        set { m_TotalCurrencyPrice = value; }
    //    }
    //    /// <summary>
    //    /// Количество товара в заказе
    //    /// </summary>
    //    private System.Int32 m_QtyOrder;
    //    /// <summary>
    //    /// Количество товара в заказе
    //    /// </summary>
    //    public System.Int32 QtyOrder
    //    {
    //        get { return m_QtyOrder; }
    //        set { m_QtyOrder = value; }
    //    }
    //    /// <summary>
    //    /// Количество товара в заказе
    //    /// </summary>
    //    private System.Int32 m_Qty;
    //    /// <summary>
    //    /// Количество товара в заказе
    //    /// </summary>
    //    public System.Int32 Qty
    //    {
    //        get { return m_Qty; }
    //        set { m_Qty = value; }
    //    }
    //    /// <summary>
    //    /// Признак "Товар импортера"
    //    /// </summary>
    //    private System.Boolean m_bIsImporter;
    //    /// <summary>
    //    /// Признак "Товар импортера"
    //    /// </summary>
    //    public System.Boolean IsImporter
    //    {
    //        get { return m_bIsImporter; }
    //        set { m_bIsImporter = value; }
    //    }
    //    /// <summary>
    //    /// Надбавка, %
    //    /// </summary>
    //    private System.Double m_ChargePercent;
    //    /// <summary>
    //    /// Надбавка, %
    //    /// </summary>
    //    public System.Double ChargePercent
    //    {
    //        get { return m_ChargePercent; }
    //        set { m_ChargePercent = value; }
    //    }
    //    /// <summary>
    //    /// Ретро-скидка, %
    //    /// </summary>
    //    private System.Double m_DiscountRetro;
    //    /// <summary>
    //    /// Ретро-скидка, %
    //    /// </summary>
    //    public System.Double DiscountRetro
    //    {
    //        get { return m_DiscountRetro; }
    //        set { m_DiscountRetro = value; }
    //    }
    //    /// <summary>
    //    /// Фиксированная скидка
    //    /// </summary>
    //    private System.Double m_DiscountFix;
    //    /// <summary>
    //    /// Фиксированная скидка
    //    /// </summary>
    //    public System.Double DiscountFix
    //    {
    //        get { return m_DiscountFix; }
    //        set { m_DiscountFix = value; }
    //    }
    //    /// <summary>
    //    /// Скидка за оборудование
    //    /// </summary>
    //    private System.Double m_DiscountTradeEquip;
    //    /// <summary>
    //    /// Скидка за оборудование
    //    /// </summary>
    //    public System.Double DiscountTradeEquip
    //    {
    //        get { return m_DiscountTradeEquip; }
    //        set { m_DiscountTradeEquip = value; }
    //    }
    //    /// <summary>
    //    /// Ставка НДС, %
    //    /// </summary>
    //    private System.Double m_NDSPercent;
    //    /// <summary>
    //    /// Ставка НДС, %
    //    /// </summary>
    //    public System.Double NDSPercent
    //    {
    //        get { return m_NDSPercent; }
    //        set { m_NDSPercent = value; }
    //    }
    //    /// <summary>
    //    /// Примечание
    //    /// </summary>
    //    private System.String m_strDescription;
    //    /// <summary>
    //    /// Примечание
    //    /// </summary>
    //    public System.String Description
    //    {
    //        get { return m_strDescription; }
    //        set { m_strDescription = value; }
    //    }
    //    #endregion

    //    #region Конструктор
    //    public CPDASupplItms(System.String strProductName, System.Int32 iQty, System.Int32 iQtyOrder,
    //        System.Double mPice, System.Double mAllPrice, System.Double mCurrencyPrice, System.Double mAllCurrencyPrice,
    //        System.Double mDiscountPercent, System.Double mTotalPrice, System.Double mTotalCurrencyPrice,
    //        System.String strProductOwnerName, System.Boolean bIsImporter, System.Double moneyChargePercent,
    //        System.Double moneyDiscountRetro, System.Double moneyDiscountFix, System.Double moneyDiscountTradeEquip,
    //        System.Double moneyNDSPercent, System.Double moneyDiscountPrice, System.Double moneyCurrencyDiscountPrice,
    //        System.String strDescription)
    //    {
    //        m_strProductName = strProductName;
    //        m_Qty = iQty;
    //        m_QtyOrder = iQtyOrder;
    //        m_Price = mPice;
    //        m_CurrencyPrice = mCurrencyPrice;
    //        m_AllPrice = mAllPrice;
    //        m_AllCurrencyPrice = mAllCurrencyPrice;
    //        m_TotalPrice = mTotalPrice;
    //        m_TotalCurrencyPrice = mTotalCurrencyPrice;
    //        m_DiscountPercent = mDiscountPercent;
    //        m_strProductOwnerName = strProductOwnerName;
    //        m_bIsImporter = bIsImporter;
    //        m_ChargePercent = moneyChargePercent;
    //        m_DiscountRetro = moneyDiscountRetro;
    //        m_NDSPercent = moneyNDSPercent;
    //        m_DiscountPrice = moneyDiscountPrice;
    //        m_DiscountFix = moneyDiscountFix;
    //        m_DiscountTradeEquip = moneyDiscountTradeEquip;
    //        m_CurrencyDiscountPrice = moneyCurrencyDiscountPrice;
    //        m_strDescription = strDescription;
    //    }
    //    #endregion
    //}
    //public class CPDASuppl
    //{
    //    #region Свойства
    //    /// <summary>
    //    /// Уникальный идентификатор
    //    /// </summary>
    //    private System.Guid m_uuidID;
    //    /// <summary>
    //    /// Уникальный идентификатор
    //    /// </summary>
    //    public System.Guid Id
    //    {
    //        get { return m_uuidID; }
    //        set { m_uuidID = value; }
    //    }
    //    /// <summary>
    //    /// Номер заказа
    //    /// </summary>
    //    private System.Int32 m_iSupplNum;
    //    /// <summary>
    //    /// Номер заказа
    //    /// </summary>
    //    public System.Int32 Num
    //    {
    //        get { return m_iSupplNum; }
    //        set { m_iSupplNum = value; }
    //    }
    //    /// <summary>
    //    /// Номер версии заказа
    //    /// </summary>
    //    private System.Int32 m_iSupplSubNum;
    //    /// <summary>
    //    /// Номер версии заказа
    //    /// </summary>
    //    public System.Int32 SubNum
    //    {
    //        get { return m_iSupplSubNum; }
    //        set { m_iSupplSubNum = value; }
    //    }
    //    /// <summary>
    //    /// Дата заказа
    //    /// </summary>
    //    private System.DateTime m_dtBeginDate;
    //    /// <summary>
    //    /// Дата заказа
    //    /// </summary>
    //    public System.DateTime BeginDate
    //    {
    //        get { return m_dtBeginDate; }
    //        set { m_dtBeginDate = value; }
    //    }
    //    /// <summary>
    //    /// Клиент (имя)
    //    /// </summary>
    //    private System.String m_strCustomerName;
    //    /// <summary>
    //    /// Клиент (имя)
    //    /// </summary>
    //    public System.String CustomerName
    //    {
    //        get { return m_strCustomerName; }
    //        set { m_strCustomerName = value; }
    //    }
    //    /// <summary>
    //    /// Код дочернего клиента
    //    /// </summary>
    //    private System.String m_strChildCustomerCode;
    //    /// <summary>
    //    /// Код дочернего клиента
    //    /// </summary>
    //    public System.String ChildCustomerCode
    //    {
    //        get { return m_strChildCustomerCode; }
    //        set { m_strChildCustomerCode = value; }
    //    }
    //    /// <summary>
    //    /// Код торгового представителя
    //    /// </summary>
    //    private System.String m_strDepartCode;
    //    /// <summary>
    //    /// Код торгового представителя
    //    /// </summary>
    //    public System.String DepartCode
    //    {
    //        get { return m_strDepartCode; }
    //        set { m_strDepartCode = value; }
    //    }
    //    /// <summary>
    //    /// Наименование Склада
    //    /// </summary>
    //    private System.String m_strStockName;
    //    /// <summary>
    //    /// Наименование Склада
    //    /// </summary>
    //    public System.String StockName
    //    {
    //        get { return m_strStockName; }
    //        set { m_strStockName = value; }
    //    }
    //    /// <summary>
    //    /// Наименование Компании
    //    /// </summary>
    //    private System.String m_strCompanyName;
    //    /// <summary>
    //    /// Наименование Компании
    //    /// </summary>
    //    public System.String CompanyName
    //    {
    //        get { return m_strCompanyName; }
    //        set { m_strCompanyName = value; }
    //    }
    //    /// <summary>
    //    /// Список групп, куда входит клиент
    //    /// </summary>
    //    private System.String m_strGroupList;
    //    /// <summary>
    //    /// Список групп, куда входит клиент
    //    /// </summary>
    //    public System.String GroupList
    //    {
    //        get { return m_strGroupList; }
    //        set { m_strGroupList = value; }
    //    }
    //    /// <summary>
    //    /// Состояние
    //    /// </summary>
    //    private enPDASupplState m_PDASupplState;
    //    /// <summary>
    //    /// Состояние
    //    /// </summary>
    //    public enPDASupplState State
    //    {
    //        get { return m_PDASupplState; }
    //        set { m_PDASupplState = value; }
    //    }
    //    /// <summary>
    //    /// Состояние
    //    /// </summary>
    //    public System.String StateName
    //    {
    //        get { return GetStateName(); }
    //    }
    //    private System.String GetStateName()
    //    {
    //        System.String strRet = "";
    //        try
    //        {
    //            switch (m_PDASupplState)
    //            {
    //                case enPDASupplState.Unkown:
    //                    {
    //                        strRet = "-";
    //                        break;
    //                    }
    //                case enPDASupplState.Created:
    //                    {
    //                        strRet = "создан";
    //                        break;
    //                    }
    //                case enPDASupplState.Deleted:
    //                    {
    //                        strRet = "удален";
    //                        break;
    //                    }
    //                case enPDASupplState.Transfered:
    //                    {
    //                        strRet = "передан";
    //                        break;
    //                    }
    //                case enPDASupplState.Processed:
    //                    {
    //                        strRet = "обработан";
    //                        break;
    //                    }
    //                case enPDASupplState.CreditControl:
    //                    {
    //                        strRet = "кредитный контроль";
    //                        break;
    //                    }
    //                case enPDASupplState.General:
    //                    {
    //                        strRet = "основной";
    //                        break;
    //                    }
    //                case enPDASupplState.CalcPricesFalse:
    //                    {
    //                        strRet = "ошибка при расчете цен";
    //                        break;
    //                    }
    //                case enPDASupplState.CreateSupplInIBFalse:
    //                    {
    //                        strRet = "ошибка при создании протокола";
    //                        break;
    //                    }
    //                case enPDASupplState.FindStock:
    //                    {
    //                        strRet = "определены склады";
    //                        break;
    //                    }
    //                case enPDASupplState.CalcPricesOk:
    //                    {
    //                        strRet = "расчитаны цены";
    //                        break;
    //                    }
    //                case enPDASupplState.AutoCalcPricesOk:
    //                    {
    //                        strRet = "автоматически расчитаны цены и создан протокол";
    //                        break;
    //                    }
    //                default:
    //                    break;
    //            }
    //        }
    //        catch
    //        {
    //        }
    //        return strRet;
    //    }
    //    /// <summary>
    //    /// Сумма, руб.
    //    /// </summary>
    //    private System.Double m_AllPrice;
    //    /// <summary>
    //    /// Сумма, руб.
    //    /// </summary>
    //    public System.Double AllPrice
    //    {
    //        get { return m_AllPrice; }
    //        set { m_AllPrice = value; }
    //    }
    //    /// <summary>
    //    /// Скидка, руб.
    //    /// </summary>
    //    private System.Double m_AllDiscount;
    //    /// <summary>
    //    /// Скидка, руб.
    //    /// </summary>
    //    public System.Double AllDiscount
    //    {
    //        get { return m_AllDiscount; }
    //        set { m_AllDiscount = value; }
    //    }
    //    /// <summary>
    //    /// Сумма со скидкой, руб
    //    /// </summary>
    //    private System.Double m_AllDiscountPrice;
    //    /// <summary>
    //    /// Сумма со скидкой, руб
    //    /// </summary>
    //    public System.Double AllDiscountPrice
    //    {
    //        get { return m_AllDiscountPrice; }
    //        set { m_AllDiscountPrice = value; }
    //    }
    //    /// <summary>
    //    /// Сумма, eur.
    //    /// </summary>
    //    private System.Double m_AllCurrencyPrice;
    //    /// <summary>
    //    /// Сумма, eur.
    //    /// </summary>
    //    public System.Double AllCurrencyPrice
    //    {
    //        get { return m_AllCurrencyPrice; }
    //        set { m_AllCurrencyPrice = value; }
    //    }
    //    /// <summary>
    //    /// Скидка, eur.
    //    /// </summary>
    //    private System.Double m_AllCurrencyDiscount;
    //    /// <summary>
    //    /// Скидка, eur.
    //    /// </summary>
    //    public System.Double AllCurrencyDiscount
    //    {
    //        get { return m_AllCurrencyDiscount; }
    //        set { m_AllCurrencyDiscount = value; }
    //    }
    //    /// <summary>
    //    /// Сумма со скидкой, eur
    //    /// </summary>
    //    private System.Double m_AllCurrencyDiscountPrice;
    //    /// <summary>
    //    /// Сумма со скидкой, eur
    //    /// </summary>
    //    public System.Double AllCurrencyDiscountPrice
    //    {
    //        get { return m_AllCurrencyDiscountPrice; }
    //        set { m_AllCurrencyDiscountPrice = value; }
    //    }
    //    /// <summary>
    //    /// Количество позиций в заказе
    //    /// </summary>
    //    private System.Int32 m_PositionQty;
    //    /// <summary>
    //    /// Количество позиций в заказе
    //    /// </summary>
    //    public System.Int32 PositionQty
    //    {
    //        get { return m_PositionQty; }
    //        set { m_PositionQty = value; }
    //    }
    //    /// <summary>
    //    /// Количество товара в заказе
    //    /// </summary>
    //    private System.Int32 m_Qty;
    //    /// <summary>
    //    /// Количество товара в заказе
    //    /// </summary>
    //    public System.Int32 Qty
    //    {
    //        get { return m_Qty; }
    //        set { m_Qty = value; }
    //    }
    //    /// <summary>
    //    /// Заказанное количество товара в заказе
    //    /// </summary>
    //    private System.Int32 m_OrderQty;
    //    /// <summary>
    //    /// Заказанное количество товара в заказе
    //    /// </summary>
    //    public System.Int32 OrderQty
    //    {
    //        get { return m_OrderQty; }
    //        set { m_OrderQty = value; }
    //    }
    //    /// <summary>
    //    /// Список продукции в заказе
    //    /// </summary>
    //    private List<CPDASupplItms> m_objSupplItmsList;
    //    /// <summary>
    //    /// Список продукции в заказе
    //    /// </summary>
    //    public List<CPDASupplItms> SupplItmsList
    //    {
    //        get { return m_objSupplItmsList; }
    //        set { m_objSupplItmsList = value; }
    //    }

    //    #endregion

    //    #region Конструктор

    //    public CPDASuppl(System.Guid uuidID, System.Int32 iSupplNum, System.Int32 iSupplSubNum, System.DateTime dtBeginDate,
    //        System.String strCustomerName, System.String strChildCustomerCode, System.String strDepartCode,
    //        enPDASupplState PDASupplState, System.Int32 iPosQty, System.Int32 iQty,
    //        System.Double mAllPrice, System.Double mAllDiscount, System.Double mAllDiscountPrice,
    //        System.Double mAllCurrencyPrice, System.Double mAllCurrencyDiscount, System.Double mAllCurrencyDiscountPrice,
    //        System.String strStockName, System.String strCompanyName, System.String strGroupList, System.Int32 iOrderQty)
    //    {
    //        m_uuidID = uuidID;
    //        m_iSupplNum = iSupplNum;
    //        m_iSupplSubNum = iSupplSubNum;
    //        m_dtBeginDate = dtBeginDate;
    //        m_strCustomerName = strCustomerName;
    //        m_strChildCustomerCode = strChildCustomerCode;
    //        m_strDepartCode = strDepartCode;
    //        m_PDASupplState = PDASupplState;
    //        m_PositionQty = iPosQty;
    //        m_Qty = iQty;
    //        m_OrderQty = iOrderQty;
    //        m_AllPrice = mAllPrice;
    //        m_AllDiscount = mAllDiscount;
    //        m_AllDiscountPrice = mAllDiscountPrice;
    //        m_AllCurrencyPrice = mAllCurrencyPrice;
    //        m_AllCurrencyDiscount = mAllCurrencyDiscount;
    //        m_AllCurrencyDiscountPrice = mAllCurrencyDiscountPrice;
    //        m_objSupplItmsList = null;
    //        m_strCompanyName = strCompanyName;
    //        m_strStockName = strStockName;
    //        m_strGroupList = strGroupList;
    //    }
    //    #endregion

    //    #region Список заказов
    //    /// <summary>
    //    /// Возвращает список заказов
    //    /// </summary>
    //    /// <param name="objProfile"></param>
    //    /// <returns></returns>
    //    public static List<CPDASuppl> GetSupplList(UniXP.Common.CProfile objProfile, System.Guid uuidSupplId,
    //        System.DateTime dtBeginDate, System.DateTime dtEndDate)
    //    {
    //        List<CPDASuppl> objList = new List<CPDASuppl>();
    //        System.Data.SqlClient.SqlConnection DBConnection = null;
    //        System.Data.SqlClient.SqlCommand cmd = null;
    //        System.Int32 iCmdTimeOut = 120;
    //        try
    //        {
    //            DBConnection = objProfile.GetDBSourceAsynch(); // .GetDBSource();
    //            if (DBConnection == null)
    //            {
    //                DevExpress.XtraEditors.XtraMessageBox.Show(
    //                    "Не удалось получить соединение с базой данных.", "Внимание",
    //                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
    //                return objList;
    //            }
    //            cmd = new System.Data.SqlClient.SqlCommand();
    //            cmd.Connection = DBConnection;
    //            cmd.CommandTimeout = iCmdTimeOut;
    //            cmd.CommandType = System.Data.CommandType.StoredProcedure;
    //            cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetPDASuppl]", objProfile.GetOptionsDllDBName());
    //            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
    //            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Suppl_Guid", System.Data.SqlDbType.UniqueIdentifier));
    //            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BeginDate", System.Data.SqlDbType.DateTime));
    //            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EndDate", System.Data.SqlDbType.DateTime));
    //            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
    //            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
    //            cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
    //            if (dtBeginDate == dtEndDate)
    //            {
    //                cmd.Parameters["@BeginDate"].Value = dtBeginDate;
    //                cmd.Parameters["@EndDate"].Value = dtEndDate.AddDays(1);
    //            }
    //            else
    //            {
    //                cmd.Parameters["@BeginDate"].Value = dtBeginDate;
    //                cmd.Parameters["@EndDate"].Value = dtEndDate;
    //            }
    //            if (uuidSupplId.CompareTo(System.Guid.Empty) == 0)
    //            {
    //                cmd.Parameters["@Suppl_Guid"].IsNullable = true;
    //                cmd.Parameters["@Suppl_Guid"].Value = null;
    //            }
    //            else
    //            {
    //                cmd.Parameters["@Suppl_Guid"].IsNullable = false;
    //                cmd.Parameters["@Suppl_Guid"].Value = uuidSupplId;
    //            }
    //            System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
    //            if (rs.HasRows)
    //            {
    //                System.Int32 iSubNum = 0;
    //                System.String strChildCode = "";
    //                while (rs.Read())
    //                {
    //                    iSubNum = (rs["SUPPL_SUBNUM"] == System.DBNull.Value) ? 0 : (System.Int32)rs["SUPPL_SUBNUM"];
    //                    strChildCode = (rs["ChildCustomerCode"] == System.DBNull.Value) ? "" : (System.String)rs["ChildCustomerCode"];
    //                    objList.Add(new CPDASuppl((System.Guid)rs["GUID_ID"], (System.Int32)rs["SUPPL_NUM"], iSubNum, (System.DateTime)rs["SUPPL_BEGINDATE"],
    //                        (System.String)rs["CUSTOMER_NAME"], strChildCode, (System.String)rs["Depart_Code"],
    //                        (enPDASupplState)((System.Byte)rs["SUPPL_STATE"]), (System.Int32)rs["POSQUANTITY"], System.Convert.ToInt32(rs["QUANTITY"]),
    //                        System.Convert.ToDouble(rs["SUPPL_ALLPRICE"]), System.Convert.ToDouble(rs["SUPPL_ALLDISCOUNT"]),
    //                        System.Convert.ToDouble(rs["SUPPL_TOTALPRICE"]), System.Convert.ToDouble(rs["SUPPL_CURRENCYALLPRICE"]),
    //                        System.Convert.ToDouble(rs["SUPPL_CURRENCYALLDISCOUNT"]), System.Convert.ToDouble(rs["SUPPL_CURRENCYTOTALPRICE"]),
    //                        (System.String)rs["STOCK_NAME"], (System.String)rs["COMPANY_NAME"], (System.String)rs["GroupList"], System.Convert.ToInt32(rs["ORDERQUANTITY"])));
    //                }
    //            }
    //            rs.Dispose();
    //            cmd.Dispose();
    //            DBConnection.Close();
    //        }
    //        catch (System.Exception f)
    //        {
    //            DevExpress.XtraEditors.XtraMessageBox.Show(
    //            "Не удалось получить список заказов.\n\nТекст ошибки: " + f.Message, "Внимание",
    //            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
    //        }
    //        return objList;
    //    }
    //    #endregion

    //    #region Рассчитать цену в заказе
    //    /// <summary>
    //    /// Расчет цены заказа
    //    /// </summary>
    //    /// <param name="objProfile">профайл</param>
    //    /// <param name="uuidSupplId">идентификатор заказа</param>
    //    /// <param name="strErr">сообщение об ошибке</param>
    //    /// <returns>результат работы хранимой процедуры в виде целого числа</returns>
    //    public static System.Int32 ProcessSuppl(UniXP.Common.CProfile objProfile, System.Guid uuidSupplId, ref System.String strErr)
    //    {
    //        System.Int32 iRet = 0;
    //        System.Data.SqlClient.SqlConnection DBConnection = null;
    //        System.Data.SqlClient.SqlCommand cmd = null;
    //        System.Data.SqlClient.SqlTransaction DBTransaction = null;
    //        try
    //        {
    //            DBConnection = objProfile.GetDBSource();
    //            if (DBConnection == null)
    //            {
    //                iRet = -1;
    //                DevExpress.XtraEditors.XtraMessageBox.Show(
    //                    "Не удалось получить соединение с базой данных.", "Внимание",
    //                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
    //                return iRet;
    //            }
    //            DBTransaction = DBConnection.BeginTransaction();
    //            cmd = new System.Data.SqlClient.SqlCommand();
    //            cmd.Connection = DBConnection;
    //            cmd.Transaction = DBTransaction;
    //            cmd.CommandType = System.Data.CommandType.StoredProcedure;
    //            cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_ProcessPricesInPDASuppl]", objProfile.GetOptionsDllDBName());
    //            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
    //            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Suppl_Guid", System.Data.SqlDbType.UniqueIdentifier));
    //            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
    //            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
    //            cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
    //            cmd.Parameters["@Suppl_Guid"].Value = uuidSupplId;
    //            cmd.ExecuteNonQuery();

    //            iRet = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
    //            strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;

    //            DBTransaction.Commit();
    //            cmd.Dispose();
    //            DBConnection.Close();
    //        }
    //        catch (System.Exception f)
    //        {
    //            DBTransaction.Rollback();
    //            DevExpress.XtraEditors.XtraMessageBox.Show(
    //            "Не удалось получить список заказов.\n\nТекст ошибки: " + f.Message, "Внимание",
    //            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
    //        }
    //        return iRet;
    //    }
    //    #endregion

    //    #region Обнулить цены в заказе
    //    /// <summary>
    //    /// Обнуляет цены в заказе
    //    /// </summary>
    //    /// <param name="objProfile">профайл</param>
    //    /// <param name="uuidSupplId">уникальный идентификатор заказа</param>
    //    /// <param name="strErr">сообщение об ошибке</param>
    //    /// <returns>результат работы хранимой процедуры в виде целого числа</returns>
    //    public static System.Int32 ClearPricesInSuppl(UniXP.Common.CProfile objProfile, System.Guid uuidSupplId, ref System.String strErr)
    //    {
    //        System.Int32 iRet = 0;
    //        System.Data.SqlClient.SqlConnection DBConnection = null;
    //        System.Data.SqlClient.SqlCommand cmd = null;
    //        System.Data.SqlClient.SqlTransaction DBTransaction = null;
    //        try
    //        {
    //            DBConnection = objProfile.GetDBSource();
    //            if (DBConnection == null)
    //            {
    //                iRet = -1;
    //                DevExpress.XtraEditors.XtraMessageBox.Show(
    //                    "Не удалось получить соединение с базой данных.", "Внимание",
    //                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
    //                return iRet;
    //            }
    //            DBTransaction = DBConnection.BeginTransaction();
    //            cmd = new System.Data.SqlClient.SqlCommand();
    //            cmd.Connection = DBConnection;
    //            cmd.Transaction = DBTransaction;
    //            cmd.CommandType = System.Data.CommandType.StoredProcedure;
    //            cmd.CommandText = System.String.Format("[{0}].[dbo].[SP_ClearPricesInPDASuppl]", objProfile.GetOptionsDllDBName());
    //            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
    //            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Suppl_Guid", System.Data.SqlDbType.UniqueIdentifier));
    //            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
    //            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
    //            cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
    //            cmd.Parameters["@Suppl_Guid"].Value = uuidSupplId;
    //            cmd.ExecuteNonQuery();

    //            iRet = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
    //            strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;

    //            if (iRet == 0)
    //            {
    //                DBTransaction.Commit();
    //            }
    //            else
    //            {
    //                DBTransaction.Rollback();
    //            }
    //            cmd.Dispose();
    //            DBConnection.Close();
    //        }
    //        catch (System.Exception f)
    //        {
    //            DBTransaction.Rollback();
    //            strErr = "Не удалось обнулить цены в заказе.\n\nТекст ошибки: " + f.Message;
    //        }
    //        return iRet;
    //    }
    //    #endregion

    //    #region Загрузить список продукции заказа
    //    /// <summary>
    //    /// Загружает список продукции заказа
    //    /// </summary>
    //    /// <param name="objProfile">профайл</param>
    //    /// <returns>true - удачное завершение операции; false - ошибка</returns>
    //    public System.Boolean LoadProductList(UniXP.Common.CProfile objProfile)
    //    {
    //        System.Boolean bRet = false;
    //        System.Data.SqlClient.SqlConnection DBConnection = null;
    //        System.Data.SqlClient.SqlCommand cmd = null;
    //        try
    //        {
    //            if (this.SupplItmsList == null) { this.SupplItmsList = new List<CPDASupplItms>(); }
    //            else { this.SupplItmsList.Clear(); }

    //            DBConnection = objProfile.GetDBSource();
    //            if (DBConnection == null)
    //            {
    //                DevExpress.XtraEditors.XtraMessageBox.Show(
    //                    "Не удалось получить соединение с базой данных.", "Внимание",
    //                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
    //                return bRet;
    //            }
    //            cmd = new System.Data.SqlClient.SqlCommand();
    //            cmd.Connection = DBConnection;
    //            cmd.CommandType = System.Data.CommandType.StoredProcedure;
    //            cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetPDASupplItms]", objProfile.GetOptionsDllDBName());
    //            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
    //            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Suppl_Guid", System.Data.SqlDbType.UniqueIdentifier));
    //            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
    //            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
    //            cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
    //            cmd.Parameters["@Suppl_Guid"].Value = this.m_uuidID;
    //            System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
    //            if (rs.HasRows)
    //            {
    //                while (rs.Read())
    //                {
    //                    this.SupplItmsList.Add(new CPDASupplItms(((System.String)rs["PARTS_NAME"] + " " + (System.String)rs["PARTS_ARTICLE"]),
    //                        System.Convert.ToInt32(rs["SPLITMS_QUANTITY"]), System.Convert.ToInt32(rs["SPLITMS_ORDERQTY"]),
    //                        System.Convert.ToDouble(rs["SPLITMS_PRICE"]), System.Convert.ToDouble(rs["SPLITMS_ALLPRICE"]),
    //                        System.Convert.ToDouble(rs["SPLITMS_CURRENCYPRICE"]), System.Convert.ToDouble(rs["SPLITMS_CURRENCYALLPRICE"]),
    //                        System.Convert.ToDouble(rs["SPLITMS_DISCOUNT"]), System.Convert.ToDouble(rs["SPLITMS_TOTALPRICE"]),
    //                        System.Convert.ToDouble(rs["SPLITMS_CURRENCYTOTALPRICE"]), (System.String)rs["ProductOwnerName"],
    //                        (System.Boolean)rs["PropertieImporter"], System.Convert.ToDouble(rs["PropertieCharge"]),
    //                        System.Convert.ToDouble(rs["RetroDiscount"]), System.Convert.ToDouble(rs["FixDiscount"]),
    //                        System.Convert.ToDouble(rs["TradeEquipDiscount"]), System.Convert.ToDouble(rs["NDSPercent"]),
    //                        System.Convert.ToDouble(rs["SPLITMS_DISCOUNTPRICE"]), System.Convert.ToDouble(rs["SPLITMS_CURRENCYDISCOUNTPRICE"]),
    //                        (System.String)rs["SplItmsDescription"]
    //                        ));
    //                }
    //            }
    //            rs.Dispose();
    //            cmd.Dispose();
    //            DBConnection.Close();
    //            bRet = true;
    //        }
    //        catch (System.Exception f)
    //        {
    //            DevExpress.XtraEditors.XtraMessageBox.Show(
    //            "Не удалось получить содержимое заказа.\n\nТекст ошибки: " + f.Message, "Внимание",
    //            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
    //        }
    //        return true;

    //    }
    //    #endregion
#endregion
}
