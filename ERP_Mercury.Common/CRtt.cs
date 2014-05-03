using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ERP_Mercury.Common
{
    /// <summary>
    /// Класс "Тип РТТ"
    /// </summary>
    public class CRttType : CBusinessObject
    {
        #region Консруктор
        public CRttType()
            : base()
        {
        }
        public CRttType(System.Guid uuidId, System.String strName)
        {
            ID = uuidId;
            Name = strName;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список типов РТТ
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список типов РТТ</returns>
        public static List<CRttType> GetRttTypeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CRttType> objList = new List<CRttType>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetRttType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CRttType((System.Guid)rs["RttType_Guid"], (System.String)rs["RttType_Name"]));
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
                "Не удалось получить список типов РТТ.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddRttType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RttType_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RttType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RttType_Name"].Value = this.Name;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@RttType_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания типа РТТ.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать тип РТТ.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteRttType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RttType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RttType_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления типа РТТ.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить тип РТТ.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditRttType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RttType_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RttType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RttType_Guid"].Value = this.ID;
                cmd.Parameters["@RttType_Name"].Value = this.Name;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения типа РТТ.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства типа РТТ.\n\nТекст ошибки: " + f.Message, "Внимание",
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
    /// Класс "Тип активности РТТ"
    /// </summary>
    public class CRttActiveType : CBusinessObject
    {
        #region Конструктор
        public CRttActiveType()
            : base()
        {
        }
        public CRttActiveType(System.Guid uuidId, System.String strName)
            : base( uuidId, strName )
        {
        }
        #endregion

        #region Список объектов
        public static List<CRttActiveType> GetRttActiveTypeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CRttActiveType> objList = new List<CRttActiveType>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetRttActiveType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CRttActiveType((System.Guid)rs["RttActiveType_Guid"],
                            (System.String)rs["RttActiveType_Name"]));
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
                "Не удалось получить список признаков активности РТТ.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddRttActiveType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RttActiveType_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RttActiveType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RttActiveType_Name"].Value = this.Name;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@RttActiveType_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания признака активности РТТ.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать признак активности РТТ.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteRttType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RttType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RttType_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления признака активности PTT.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить признак активности PTT.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditRttActiveType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RttActiveType_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RttActiveType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RttActiveType_Guid"].Value = this.ID;
                cmd.Parameters["@RttActiveType_Name"].Value = this.Name;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств признака активности PTT.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства признака активности PTT.\n\nТекст ошибки: " + f.Message, "Внимание",
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
    /// Класс "Цель приобретения"
    /// </summary>
    public class CTargetBuy : CBusinessObject
    {
        #region Конструктор
        public CTargetBuy()
            : base()
        {
        }
        public CTargetBuy(System.Guid uuidId, System.String strName)
            : base( uuidId, strName )
        {
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список объектов "цель приобретения"
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список объектов "цель приобретения"</returns>
        public static List<CTargetBuy> GetTargetBuyList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CTargetBuy> objList = new List<CTargetBuy>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetTargetBuy]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CTargetBuy((System.Guid)rs["TargetBuy_Guid"],
                            (System.String)rs["TargetBuy_Name"]));
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
                "Не удалось получить список объектов 'цель приобретения'.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Возвращает список целей приобретения, назначенных клиенту с указанным идентификатором
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="uuidCustomerId">уникальный идентификатор клиента</param>
        /// <returns>список целей приобретения</returns>
        public static List<CTargetBuy> GetTargetBuyForCustomer(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidCustomerId)
        {
            List<CTargetBuy> objList = new List<CTargetBuy>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetCustomerTargetBuy]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Customer_Guid"].Value = uuidCustomerId;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CTargetBuy((System.Guid)rs["TargetBuy_Guid"],
                            (System.String)rs["TargetBuy_Name"]));
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
                "Не удалось получить список целей приобретения.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddTargetBuy]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@TargetBuy_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@TargetBuy_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@TargetBuy_Name"].Value = this.Name;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@TargetBuy_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания объектa 'цель приобретения'.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать объект 'цель приобретения'.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteTargetBuy]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@TargetBuy_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@TargetBuy_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления объектa 'цель приобретения'.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить объект 'цель приобретения'.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditTargetBuy]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@TargetBuy_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@TargetBuy_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@TargetBuy_Guid"].Value = this.ID;
                cmd.Parameters["@TargetBuy_Name"].Value = this.Name;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств объектa 'цель приобретения'.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства объектa 'цель приобретения'.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }

        #endregion

        #region Сохранить список целей приобретения для клиента
        /// <summary>
        /// Сохраняет в БД привязку целей приобретения к клиенту
        /// </summary>
        /// <param name="uuidCustomerId">уникальный идентификатор клиента</param>
        /// <param name="objTargetBuyList">список целей приобретения</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean SaveTargetBuyListForCustomer(System.Guid uuidCustomerId,
            List<CTargetBuy> objTargetBuyList,
            UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
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

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteTargetBuyFromCustomer]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Customer_Guid"].Value = uuidCustomerId;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    if ((objTargetBuyList != null) && (objTargetBuyList.Count > 0))
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddTargetBuyToCustomer]", objProfile.GetOptionsDllDBName());
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@TargetBuy_Guid", System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters["@Customer_Guid"].Value = uuidCustomerId;

                        foreach (CTargetBuy objTargetBuy in objTargetBuyList)
                        {
                            cmd.Parameters["@TargetBuy_Guid"].Value = objTargetBuy.ID;
                            cmd.ExecuteNonQuery();
                            iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                            if (iRes != 0) { break; }
                        }

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
                    DBConnection.Close();
                }
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                if (cmdSQL == null)
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
            return Name;
        }
    }
    /// <summary>
    /// Класс "Спец код для РТТ"
    /// </summary>
    public class CRttSpecCode : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Описание
        /// </summary>
        private System.String m_strDescription;
        /// <summary>
        /// Описание
        /// </summary>
        [DisplayName("Примечание")]
        [Description("Примечание")]
        [Category("2. Необязательные значения")]
        public System.String Description
        {
            get { return m_strDescription; }
            set { m_strDescription = value; }
        }
        #endregion

        #region Конструктор
        public CRttSpecCode()
        {
            ID = System.Guid.Empty;
            Name = "";
            m_strDescription = "";
        }
        public CRttSpecCode( System.Guid uuidID, System.String strName, System.String strDscrpn )
        {
            ID = uuidID;
            Name = strName;
            m_strDescription = strDscrpn;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список объектов "спец код РТТ"
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список объектов "спец код РТТ"</returns>
        public static List<CRttSpecCode> GetRttSpecCodeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CRttSpecCode> objList = new List<CRttSpecCode>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetRttSpecCode]", objProfile.GetOptionsDllDBName());
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
                        strDscrpn = (rs["RttSpecCode_Description"] == System.DBNull.Value) ? "" : (System.String)rs["RttSpecCode_Description"];
                        objList.Add(new CRttSpecCode((System.Guid)rs["RttSpecCode_Guid"], (System.String)rs["RttSpecCode_Code"], strDscrpn));
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
                "Не удалось получить список объектов 'спец код РТТ'.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddRttSpecCode]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RttSpecCode_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RttSpecCode_Code", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RttSpecCode_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RttSpecCode_Code"].Value = this.Name;
                if (this.m_strDescription == "")
                {
                    cmd.Parameters["@RttSpecCode_Description"].IsNullable = true;
                    cmd.Parameters["@RttSpecCode_Description"].Value = null;
                }
                else
                {
                    cmd.Parameters["@RttSpecCode_Description"].IsNullable = false;
                    cmd.Parameters["@RttSpecCode_Description"].Value = this.m_strDescription;
                }
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@RttSpecCode_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания объектa 'спец код РТТ'.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать объект 'спец код РТТ'.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteRttSpecCode]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RttSpecCode_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RttSpecCode_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления объектa 'спец код РТТ'.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить объект 'спец код РТТ'.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditRttSpecCode]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RttSpecCode_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RttSpecCode_Code", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RttSpecCode_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RttSpecCode_Guid"].Value = this.ID;
                cmd.Parameters["@RttSpecCode_Code"].Value = this.Name;
                if (this.m_strDescription == "")
                {
                    cmd.Parameters["@RttSpecCode_Description"].IsNullable = true;
                    cmd.Parameters["@RttSpecCode_Description"].Value = null;
                }
                else
                {
                    cmd.Parameters["@RttSpecCode_Description"].IsNullable = false;
                    cmd.Parameters["@RttSpecCode_Description"].Value = this.m_strDescription;
                }
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств объектa 'спец код РТТ'.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства объектa 'спец код РТТ'.\n\nТекст ошибки: " + f.Message, "Внимание",
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
    /// TypeConverter для списка регионов
    /// </summary>
    class ProductCatalogConverter : TypeConverter
    {
        /// <summary>
        /// Будем предоставлять выбор из списка
        /// </summary>
        public override bool GetStandardValuesSupported(
          ITypeDescriptorContext context)
        {
            return true;
        }
        /// <summary>
        /// ... и только из списка
        /// </summary>
        public override bool GetStandardValuesExclusive(
          ITypeDescriptorContext context)
        {
            // false - можно вводить вручную
            // true - только выбор из списка
            return true;
        }

        /// <summary>
        /// А вот и список
        /// </summary>
        public override StandardValuesCollection GetStandardValues(
          ITypeDescriptorContext context)
        {
            // возвращаем список строк из настроек программы
            // (базы данных, интернет и т.д.)

            CEquipmentType objEquipmentType = (CEquipmentType)context.Instance;
            System.Collections.Generic.List<CProductCatalog> objList = objEquipmentType.GetAllProductCatalognList();

            return new StandardValuesCollection(objList);
        }
    }
    /// <summary>
    /// Класс "Тип оборудования"
    /// </summary>
    public class CEquipmentType : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Код оборудования
        /// </summary>
        private System.String m_strCode;
        /// <summary>
        /// Код оборудования
        /// </summary>
        [DisplayName("Код оборудования")]
        [Description("Код оборудования")]
        [Category("1. Обязательные значения")]
        public System.String Code
        {
            get { return m_strCode; }
            set { m_strCode = value; }
        }



        //  --------------------------- Begin --------------------------------
        /// <summary>
        /// Каталог продукции
        /// </summary>
        private CProductCatalog m_objProductCatalog;
        /// <summary>
        /// Каталог продукции
        /// </summary>
        [DisplayName("Каталог продукции")]
        [Description("Каталог продукции")]
        [Category("1. Обязательные значения")]
        [TypeConverter(typeof(ProductCatalogConverter))]
        [ReadOnly(false)]
        public System.String ProductCatalogName
        {
            get { return m_objProductCatalog.Name; }
            set { SetProductCatalogValue(value); }
        }
        private void SetProductCatalogValue(System.String strProductCatalogName)
        {
            try
            {
                if (m_objProductCatalogList == null) { m_objProductCatalog = null; }
                else
                {
                    foreach (CProductCatalog objProductCatalog in m_objProductCatalogList)
                    {
                        if (objProductCatalog.Name == strProductCatalogName)
                        {
                            m_objProductCatalog = objProductCatalog;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось установить значение каталога продукции.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }
        public List<CProductCatalog> GetAllProductCatalognList()
        {
            return m_objProductCatalogList;
        }

        /// <summary>
        /// Размер
        /// </summary>
        private System.String m_strSizeName;
        /// <summary>
        /// Размер
        /// </summary>
        [DisplayName("Размер")]
        [Description("Размер")]
        [Category("2. Необязательные значения")]
        public System.String SizeName
        {
            get { return m_strSizeName; }
            set { m_strSizeName = value; }
        }
        /// <summary>
        /// Описание
        /// </summary>
        private System.String m_strDescription;
        /// <summary>
        /// Описание
        /// </summary>
        [DisplayName("Примечание")]
        [Description("Примечание")]
        [Category("2. Необязательные значения")]
        public System.String Description
        {
            get { return m_strDescription; }
            set { m_strDescription = value; }
        }
        private List<CProductCatalog> m_objProductCatalogList;
        #endregion

        #region Конструктор
        public CEquipmentType()
        {
            ID = System.Guid.Empty;
            Name = "";
            m_strDescription = "";
            m_objProductCatalog = null;
            m_strCode = "";
            m_strSizeName = "";
        }
        public CEquipmentType(System.Guid uuidID, System.String strName, System.String strDscrpn,
            CProductCatalog objProductCatalog, System.String strCode, System.String strSizeName)
        {
            ID = uuidID;
            Name = strName;
            m_strDescription = strDscrpn;
            m_objProductCatalog = objProductCatalog;
            m_strCode = strCode;
            m_strSizeName = strSizeName;

        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список объектов "тип оборудования"
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список объектов "тип оборудования"</returns>
        public static List<CEquipmentType> GetEquipmentTypeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CEquipmentType> objList = new List<CEquipmentType>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetEquipmentType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.String strDscrpn = "";
                    System.String strSizeName = "";
                    CProductCatalog objProductCatalog = null;
                    while (rs.Read())
                    {
                        strDscrpn = (rs["EquipmentType_Dscrpn"] == System.DBNull.Value) ? "" : (System.String)rs["EquipmentType_Dscrpn"];
                        strSizeName = (rs["EquipmentType_SizeName"] == System.DBNull.Value) ? "" : (System.String)rs["EquipmentType_SizeName"];
                        objProductCatalog = new CProductCatalog((System.Guid)rs["ProductCatalog_Guid"], (System.String)rs["ProductCatalog_Name"],
                            (rs["ProductCatalog_Description"] == System.DBNull.Value ? "" : (System.String)rs["ProductCatalog_Description"]),
                            System.Convert.ToBoolean(rs["ProductCatalog_IsActive"]),
                            System.Convert.ToBoolean(rs["ProductCatalog_IsOur"]));
                        objList.Add(new CEquipmentType((System.Guid)rs["EquipmentType_Guid"],
                            (System.String)rs["EquipmentType_Name"], strDscrpn, objProductCatalog,
                            (System.String)rs["EquipmentType_Code"],
                            strSizeName));
                    }
                }
                rs.Dispose();
                List<CProductCatalog> objProductCatalogList = CProductCatalog.GetProductCatalogListAll(objProfile, cmd);
                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
                if (objProductCatalogList != null)
                {
                    foreach (CEquipmentType objEquipmentType in objList)
                    {
                        objEquipmentType.m_objProductCatalogList = objProductCatalogList;
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список объектов \"тип оборудования\".\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Загружает в m_objProductCatalog список каталогов продукции
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        public void InitProductCatalogList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            try
            {
                this.m_objProductCatalogList = CProductCatalog.GetProductCatalogListAll(objProfile, cmdSQL);
                if ((this.m_objProductCatalogList != null) && (this.m_objProductCatalogList.Count > 0))
                {
                    this.m_objProductCatalog = this.m_objProductCatalogList[0];
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось загрузить список каталогов продукции.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }
        #endregion

        #region Add
        /// <summary>
        /// Проверка свойств перед сохранением
        /// </summary>
        /// <returns>true - ошибок нет; false - ошибка</returns>
        public override System.Boolean IsAllParametersValid()
        {
            System.Boolean bRet = false;
            try
            {
                if (this.m_strCode == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать код!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.Name == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать название!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.m_objProductCatalog == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать каталог продукции!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }

                bRet = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка проверки свойств.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return bRet;
        }
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddEquipmentType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EquipmentType_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EquipmentType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EquipmentType_Dscrpn", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EquipmentType_Code", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EquipmentType_SizeName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductCatalog_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@EquipmentType_Name"].Value = this.Name;
                cmd.Parameters["@EquipmentType_Code"].Value = this.m_strCode;
                cmd.Parameters["@ProductCatalog_Guid"].Value = this.m_objProductCatalog.ID;
                if (this.m_strDescription == "")
                {
                    cmd.Parameters["@EquipmentType_Dscrpn"].IsNullable = true;
                    cmd.Parameters["@EquipmentType_Dscrpn"].Value = null;
                }
                else
                {
                    cmd.Parameters["@EquipmentType_Dscrpn"].IsNullable = false;
                    cmd.Parameters["@EquipmentType_Dscrpn"].Value = this.m_strDescription;
                }
                if (this.m_strSizeName == "")
                {
                    cmd.Parameters["@EquipmentType_SizeName"].IsNullable = true;
                    cmd.Parameters["@EquipmentType_SizeName"].Value = null;
                }
                else
                {
                    cmd.Parameters["@EquipmentType_SizeName"].IsNullable = false;
                    cmd.Parameters["@EquipmentType_SizeName"].Value = this.m_strDescription;
                }
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@EquipmentType_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания объектa 'тип оборудования'.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать объект 'тип оборудования'.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteEquipmentType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EquipmentType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@EquipmentType_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show( "Ошибка удаления объектa 'тип оборудования'.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить объект 'тип оборудования'.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditEquipmentType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EquipmentType_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EquipmentType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EquipmentType_Dscrpn", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EquipmentType_Code", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EquipmentType_SizeName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductCatalog_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@EquipmentType_Guid"].Value = this.ID;
                cmd.Parameters["@EquipmentType_Name"].Value = this.Name;
                cmd.Parameters["@EquipmentType_Code"].Value = this.m_strCode;
                cmd.Parameters["@ProductCatalog_Guid"].Value = this.m_objProductCatalog.ID;
                if (this.m_strDescription == "")
                {
                    cmd.Parameters["@EquipmentType_Dscrpn"].IsNullable = true;
                    cmd.Parameters["@EquipmentType_Dscrpn"].Value = null;
                }
                else
                {
                    cmd.Parameters["@EquipmentType_Dscrpn"].IsNullable = false;
                    cmd.Parameters["@EquipmentType_Dscrpn"].Value = this.m_strDescription;
                }
                if (this.m_strSizeName == "")
                {
                    cmd.Parameters["@EquipmentType_SizeName"].IsNullable = true;
                    cmd.Parameters["@EquipmentType_SizeName"].Value = null;
                }
                else
                {
                    cmd.Parameters["@EquipmentType_SizeName"].IsNullable = false;
                    cmd.Parameters["@EquipmentType_SizeName"].Value = this.m_strDescription;
                }
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств объектa \"тип оборудования\".\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства объектa \"тип оборудования\".\n\nТекст ошибки: " + f.Message, "Внимание",
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
            return ( m_strCode + " " + ( m_objProductCatalog == null ? "" : m_objProductCatalog.Name  + " " + Name + " " + m_strSizeName) );
        }
    }
    /// <summary>
    /// Размер
    /// </summary>
    public class CSizeEq : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Формат
        /// </summary>
        private System.String m_strFormat;
        /// <summary>
        /// Формат
        /// </summary>
        [DisplayName("Формат")]
        [Description("Формат")]
        [Category("1. Обязательные значения")]
        public System.String SizeFormat
        {
            get { return m_strFormat; }
            set { m_strFormat = value; }
        }
        /// <summary>
        /// Описание
        /// </summary>
        private System.String m_strDescription;
        /// <summary>
        /// Описание
        /// </summary>
        [DisplayName("Примечание")]
        [Description("Примечание")]
        [Category("2. Необязательные значения")]
        public System.String Description
        {
            get { return m_strDescription; }
            set { m_strDescription = value; }
        }
        #endregion

        #region Конструктор
        public CSizeEq()
        {
            ID = System.Guid.Empty;
            Name = "";
            m_strFormat = "";
            m_strDescription = "";
        }
        public CSizeEq(System.Guid uuidID, System.String strName, System.String strFormat, System.String strDscrpn)
        {
            ID = uuidID;
            Name = strName;
            m_strFormat = strFormat;
            m_strDescription = strDscrpn;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список объектов "размер"
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список объектов "размер"</returns>
        public static List<CSizeEq> GetSizeEqList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CSizeEq> objList = new List<CSizeEq>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetSize]", objProfile.GetOptionsDllDBName());
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
                        strDscrpn = (rs["Size_Descrpn"] == System.DBNull.Value) ? "" : (System.String)rs["Size_Descrpn"];
                        objList.Add(new CSizeEq((System.Guid)rs["Size_Guid"], (System.String)rs["Size_Name"], (System.String)rs["Size_Format"], strDscrpn));
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
                "Не удалось получить список объектов 'размер'.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddSize]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Size_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Size_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Size_Format", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Size_Dscrpn", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Size_Name"].Value = this.Name;
                cmd.Parameters["@Size_Format"].Value = this.m_strFormat;
                if (this.m_strDescription == "")
                {
                    cmd.Parameters["@Size_Dscrpn"].IsNullable = true;
                    cmd.Parameters["@Size_Dscrpn"].Value = null;
                }
                else
                {
                    cmd.Parameters["@Size_Dscrpn"].IsNullable = false;
                    cmd.Parameters["@Size_Dscrpn"].Value = this.m_strDescription;
                }
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@Size_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания объектa 'размер'.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать объект 'размер'.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteSize]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Size_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Size_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления объектa 'размер'.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить объект 'размер'.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditSize]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Size_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Size_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Size_Format", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Size_Dscrpn", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Size_Guid"].Value = this.ID;
                cmd.Parameters["@Size_Name"].Value = this.Name;
                cmd.Parameters["@Size_Format"].Value = this.m_strFormat;
                if (this.m_strDescription == "")
                {
                    cmd.Parameters["@Size_Dscrpn"].IsNullable = true;
                    cmd.Parameters["@Size_Dscrpn"].Value = null;
                }
                else
                {
                    cmd.Parameters["@Size_Dscrpn"].IsNullable = false;
                    cmd.Parameters["@Size_Dscrpn"].Value = this.m_strDescription;
                }
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств объектa 'размер'.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства объектa 'размер'.\n\nТекст ошибки: " + f.Message, "Внимание",
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
    /// Класс "Возможность установки оборудования"
    /// </summary>
    public class CAvailability : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Описание
        /// </summary>
        private System.String m_strDescription;
        /// <summary>
        /// Описание
        /// </summary>
        [DisplayName("Примечание")]
        [Description("Примечание")]
        [Category("2. Необязательные значения")]
        public System.String Description
        {
            get { return m_strDescription; }
            set { m_strDescription = value; }
        }
        #endregion

        #region Конструктор
        public CAvailability()
        {
            ID = System.Guid.Empty;
            Name = "";
            m_strDescription = "";
        }
        public CAvailability(System.Guid uuidID, System.String strName, System.String strDscrpn)
        {
            ID = uuidID;
            Name = strName;
            m_strDescription = strDscrpn;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список объектов "возможность установки"
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список объектов "возможность установки"</returns>
        public static List<CAvailability> GetAvailabilityList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CAvailability> objList = new List<CAvailability>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetAvailability]", objProfile.GetOptionsDllDBName());
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
                        strDscrpn = (rs["Availability_Dscrpn"] == System.DBNull.Value) ? "" : (System.String)rs["Availability_Dscrpn"];
                        objList.Add(new CAvailability((System.Guid)rs["Availability_Guid"], (System.String)rs["Availability_Name"], strDscrpn));
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
                "Не удалось получить список объектов 'возможность установки'.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddAvailability]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Availability_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Availability_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Availability_Dscrpn", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Availability_Name"].Value = this.Name;
                if (this.m_strDescription == "")
                {
                    cmd.Parameters["@Availability_Dscrpn"].IsNullable = true;
                    cmd.Parameters["@Availability_Dscrpn"].Value = null;
                }
                else
                {
                    cmd.Parameters["@Availability_Dscrpn"].IsNullable = false;
                    cmd.Parameters["@Availability_Dscrpn"].Value = this.m_strDescription;
                }
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@Availability_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания объектa 'возможность установки'.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать объект 'возможность установки'.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteAvailability]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Availability_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Availability_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления объектa 'возможность установки'.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error );
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить объект 'возможность установки'.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditAvailability]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Availability_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Availability_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Availability_Dscrpn", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Availability_Guid"].Value = this.ID;
                cmd.Parameters["@Availability_Name"].Value = this.Name;
                if (this.m_strDescription == "")
                {
                    cmd.Parameters["@Availability_Dscrpn"].IsNullable = true;
                    cmd.Parameters["@Availability_Dscrpn"].Value = null;
                }
                else
                {
                    cmd.Parameters["@Availability_Dscrpn"].IsNullable = false;
                    cmd.Parameters["@Availability_Dscrpn"].Value = this.m_strDescription;
                }
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств объектa 'возможность установки'.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства объектa 'возможность установки'.\n\nТекст ошибки: " + f.Message, "Внимание",
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
    /// Каталог продукции
    /// </summary>
    public class CProductCatalog : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Признак активности
        /// </summary>
        private System.Boolean m_IsActive;
        /// <summary>
        /// Признак активности
        /// </summary>
        [DisplayName("Активен")]
        [Description("Признак активности")]
        [Category("2. Дополнительно")]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public System.Boolean IsActive
        {
            get { return m_IsActive; }
            set { m_IsActive = value; }
        }
        /// <summary>
        /// Признак "наша продукция"
        /// </summary>
        private System.Boolean m_IsOur;
        /// <summary>
        /// Признак "наша продукция"
        /// </summary>
        [DisplayName("Наша продукция")]
        [Description("Признак \"Наша продукция\"")]
        [Category("2. Дополнительно")]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public System.Boolean IsOur
        {
            get { return m_IsOur; }
            set { m_IsOur = value; }
        }
        /// <summary>
        /// Примечание
        /// </summary>
        private System.String m_strDescription;
        /// <summary>
        /// Примечание
        /// </summary>
        [DisplayName("Описание")]
        [Description("Описание")]
        [Category("2. Дополнительно")]
        public System.String Description
        {
            get { return m_strDescription; }
            set { m_strDescription = value; }
        }
        #endregion

        #region Конструктор
        public CProductCatalog()
            : base()
        {
            m_IsOur = false;
        }
        public CProductCatalog(System.Guid uuidId, System.String strName, System.String strDescription, 
            System.Boolean bIsActive, System.Boolean bIsOur)
        {
            ID = uuidId;
            Name = strName;
            m_IsActive = bIsActive;
            m_strDescription = strDescription;
            m_IsOur = bIsOur;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список объектов "каталог продукции"
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список объектов "каталог продукции"</returns>
        public static List<CProductCatalog> GetProductCatalogList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidRttId)
        {
            List<CProductCatalog> objList = new List<CProductCatalog>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetProductCatalogForRtt]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                if (uuidRttId.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Rtt_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@Rtt_Guid"].Value = uuidRttId;
                    System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                    if (rs.HasRows)
                    {
                        while (rs.Read())
                        {
                            objList.Add(new CProductCatalog((System.Guid)rs["ProductCatalog_Guid"], (System.String)rs["ProductCatalog_Name"],
                                ((rs["ProductCatalog_Description"] == System.DBNull.Value) ? "" : (System.String)rs["ProductCatalog_Description"]),
                                (System.Boolean)rs["ProductCatalog_IsActive"], (System.Boolean)rs["ProductCatalog_IsOur"]));

                        }
                    }
                    rs.Dispose();
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
                "Не удалось получить список объектов \"каталог продукции\".\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        public static List<CProductCatalog> GetProductCatalogListAll(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CProductCatalog> objList = new List<CProductCatalog>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetProductCatalogList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CProductCatalog((System.Guid)rs["ProductCatalog_Guid"], (System.String)rs["ProductCatalog_Name"],
                            ((rs["ProductCatalog_Description"] == System.DBNull.Value) ? "" : (System.String)rs["ProductCatalog_Description"]),
                            (System.Boolean)rs["ProductCatalog_IsActive"], (System.Boolean)rs["ProductCatalog_IsOur"]));

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
                "Не удалось получить список объектов \"каталог продукции\".\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        #endregion

        #region IsAllParametersValid
        /// <summary>
        /// Проверка свойств перед сохранением
        /// </summary>
        /// <returns>true - ошибок нет; false - ошибка</returns>
        public override System.Boolean IsAllParametersValid()
        {
            System.Boolean bRet = false;
            try
            {
                if (this.Name == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать название!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }

                bRet = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка проверки свойств.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return bRet;
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddProductCatalog]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductCatalog_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductCatalog_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductCatalog_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductCatalog_IsOur", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@ProductCatalog_Name"].Value = this.Name;
                cmd.Parameters["@ProductCatalog_Description"].Value = this.Description;
                cmd.Parameters["@ProductCatalog_IsOur"].Value = this.IsOur;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@ProductCatalog_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания каталога продукции.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать каталог продукции.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        /// <summary>
        /// Добавляет РТТ каталог продукции
        /// </summary>
        /// <param name="uuidRttId">УИ РТТ</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        private System.Boolean AddProductCatalogToRtt(System.Guid uuidRttId,
        UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
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

                System.String strAddCmd = "";
                strAddCmd = System.String.Format("[{0}].[dbo].[sp_AddProductCatalogToRtt]", objProfile.GetOptionsDllDBName());

                cmd.Parameters.Clear();
                cmd.CommandText = strAddCmd;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RttProductCatalog_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductCatalog_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Rtt_Guid", System.Data.SqlDbType.UniqueIdentifier));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@ProductCatalog_Guid"].Value = this.ID;
                cmd.Parameters["@Rtt_Guid"].Value = uuidRttId;

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
                if (cmdSQL == null)
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
        /// Сохраняет в БД список каталогов продукции для РТТ
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="objProductCatalogList">каталоги продукции</param>
        /// <param name="objProductCatalogForDeleteList">каталоги продукции на удаление</param>
        /// <param name="uuidObjectId">идентификатор РТТ</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean SaveProductCatalogList(List<CProductCatalog> objProductCatalogList,
            List<CProductCatalog> objProductCatalogForDeleteList, System.Guid uuidObjectId,
            UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            if (((objProductCatalogList == null) || (objProductCatalogList.Count == 0)) && ((objProductCatalogForDeleteList == null) || (objProductCatalogForDeleteList.Count == 0))) { return true; }
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

                System.Int32 iRes = 0;
                if ((objProductCatalogForDeleteList != null) && (objProductCatalogForDeleteList.Count > 0))
                {
                    foreach (CProductCatalog objProductCatalog in objProductCatalogForDeleteList)
                    {

                        iRes = (objProductCatalog.DeleteProductCatalogFromRtt(uuidObjectId, objProfile, cmd, ref strErr) == true) ? 0 : 1;
                        if (iRes != 0) { break; }
                    }
                }

                if (iRes == 0)
                {
                    if ((objProductCatalogList != null) && (objProductCatalogList.Count > 0))
                    {
                        // теперь в цикле добавим в БД каждый член из списка
                        foreach (CProductCatalog objProductCatalog in objProductCatalogList)
                        {
                            iRes = (objProductCatalog.AddProductCatalogToRtt(uuidObjectId, objProfile, cmd, ref strErr) == true) ? 0 : 1;
                            if (iRes != 0) { break; }
                        }
                    }
                }

                if (cmdSQL == null)
                {
                    if (iRes == 0)
                    {
                        // подтверждаем транзакцию
                        DBTransaction.Commit();
                    }
                    else
                    {
                        // откатываем транзакцию
                        DBTransaction.Rollback();
                    }
                    DBConnection.Close();
                }

                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                if (cmdSQL == null)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteProductCatalog]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductCatalog_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@ProductCatalog_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления каталога продукции.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить тип каталог продукции.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        /// <summary>
        /// Удаляет каталог продукции у РТТ
        /// </summary>
        /// <param name="uuidRttId">УИ РТТ</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        private System.Boolean DeleteProductCatalogFromRtt(System.Guid uuidRttId,
        UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
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

                System.String strAddCmd = "";
                strAddCmd = System.String.Format("[{0}].[dbo].[sp_DeleteProductCatalogFromRtt]", objProfile.GetOptionsDllDBName());

                cmd.Parameters.Clear();
                cmd.CommandText = strAddCmd;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductCatalog_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Rtt_Guid", System.Data.SqlDbType.UniqueIdentifier));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@ProductCatalog_Guid"].Value = this.ID;
                cmd.Parameters["@Rtt_Guid"].Value = uuidRttId;

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
                if (cmdSQL == null)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditProductCatalog]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductCatalog_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductCatalog_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductCatalog_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductCatalog_IsActive", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductCatalog_IsOur", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@ProductCatalog_Guid"].Value = this.ID;
                cmd.Parameters["@ProductCatalog_Name"].Value = this.Name;
                cmd.Parameters["@ProductCatalog_Description"].Value = this.Description;
                cmd.Parameters["@ProductCatalog_IsActive"].Value = this.IsActive;
                cmd.Parameters["@ProductCatalog_IsOur"].Value = this.IsOur;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств каталога продукции.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства каталога продукции.\n\nТекст ошибки: " + f.Message, "Внимание",
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
    /// Класс "Оборудование для РТТ"
    /// </summary>
    public class CTradeEquipment
    {
        #region Свойства
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        private System.Guid m_uuidId;
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public System.Guid Id
        {
            get { return m_uuidId; }
            set { m_uuidId = value; }
        }
        /// <summary>
        /// Тип оборудования
        /// </summary>
        private CEquipmentType m_objEquipmentType;
        /// <summary>
        /// Тип оборудования
        /// </summary>
        public CEquipmentType EquipmentType
        {
            get { return m_objEquipmentType; }
            set { m_objEquipmentType = value; }
        }
        /// <summary>
        /// Возможность установки
        /// </summary>
        private CAvailability m_objAvailability;
        /// <summary>
        /// Возможность установки
        /// </summary>
        public CAvailability Availability
        {
            get { return m_objAvailability; }
            set { m_objAvailability = value; }
        }
        /// <summary>
        /// Количество
        /// </summary>
        private System.Int32 m_iQuantity;
        /// <summary>
        /// Количество
        /// </summary>
        public System.Int32 Quantity
        {
            get { return m_iQuantity; }
            set { m_iQuantity = value; }
        }
        /// <summary>
        /// Признак того, что объект новый и в БД о нем информации нет
        /// </summary>
        private System.Boolean m_bNewObject;
        /// <summary>
        /// Признак того, что объект новый и в БД о нем информации нет
        /// </summary>
        public System.Boolean IsNewObject
        {
            get { return m_bNewObject; }
            set
            {
                m_bNewObject = value;
                if (m_bNewObject == true)
                {
                    if (m_uuidId.CompareTo(System.Guid.Empty) != 0) { m_uuidId = System.Guid.Empty; }
                }
            }
        }
        #endregion

        #region Конструктор
        public CTradeEquipment()
        {
            m_uuidId = System.Guid.Empty;
            m_iQuantity = 0;
            m_objAvailability = null;
            m_objEquipmentType = null;
            m_bNewObject = false;
        }
        public CTradeEquipment(System.Guid uuidId, System.Int32 iQty, CAvailability objAvailability,
            CEquipmentType objEquipmentType)
        {
            m_uuidId = uuidId;
            m_iQuantity = iQty;
            m_objAvailability = objAvailability;
            m_objEquipmentType = objEquipmentType;
            m_bNewObject = false;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список объектов "торговое оборудование"
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список объектов "торговое оборудование"</returns>
        public static List<CTradeEquipment> GetTradeEquipmentList(UniXP.Common.CProfile objProfile, 
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidRttId)
        {
            List<CTradeEquipment> objList = new List<CTradeEquipment>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetEquipmentForRtt]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Rtt_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Rtt_Guid"].Value = uuidRttId;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.String strDscrpn = "";
                    System.String strSizeName = "";
                    CProductCatalog objProductCatalog = null;
                    CEquipmentType objEquipmentType = null;
                    while (rs.Read())
                    {
                        strDscrpn = (rs["EquipmentType_Dscrpn"] == System.DBNull.Value) ? "" : (System.String)rs["EquipmentType_Dscrpn"];
                        strSizeName = (rs["EquipmentType_SizeName"] == System.DBNull.Value) ? "" : (System.String)rs["EquipmentType_SizeName"];
                        objProductCatalog = new CProductCatalog((System.Guid)rs["ProductCatalog_Guid"], (System.String)rs["EquipmentType_Name"],
                            (rs["ProductCatalog_Description"] == System.DBNull.Value ? "" : (System.String)rs["ProductCatalog_Description"]),
                            System.Convert.ToBoolean(rs["ProductCatalog_IsActive"]),
                            System.Convert.ToBoolean(rs["ProductCatalog_IsOur"]));
                        objEquipmentType = new CEquipmentType((System.Guid)rs["EquipmentType_Guid"],
                            (System.String)rs["EquipmentType_Name"], strDscrpn, objProductCatalog,
                            (System.String)rs["EquipmentType_Code"],
                            strSizeName);

                        objList.Add(
                            new CTradeEquipment((System.Guid)rs["RttEquipment_Guid"], System.Convert.ToInt32(rs["RttEquipment_EquipmentQuantity"]),
                                new CAvailability((System.Guid)rs["Availability_Guid"], (System.String)rs["Availability_Name"], ""),
                                objEquipmentType )
                            );
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
                "Не удалось получить список объектов \"торговое оборудование\".\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        #endregion

        #region Удалить оборудование
        /// <summary>
        /// Удаляет оборудование из базы данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="uuidObjectId">идентификатор РТТ</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Remove(System.Guid uuidObjectId,
            UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
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

                cmd.Parameters.Clear();
                System.String strDeleteCmd = "";
                strDeleteCmd = System.String.Format("[{0}].[dbo].[sp_DeleteEquipmentRtt]", objProfile.GetOptionsDllDBName());
                cmd.CommandText = strDeleteCmd;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RttEquipment_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RttEquipment_Guid"].Value = this.Id;
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
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                if (cmdSQL == null)
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

        #region Добавить в БД оборудование
        /// <summary>
        /// Проверка свойств оборудования перед сохранением
        /// </summary>
        /// <param name="strErr">текст с ошибкой</param>
        /// <returns>true - все свойства корректны; false - ошибка</returns>
        public System.Boolean IsAllParametersValid(ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (this.m_iQuantity <=0 )
                {
                    strErr = "Необходимо указать количество!";
                    return bRet;
                }
                if (this.m_objEquipmentType == null)
                {
                    strErr = "Необходимо указать тип оборудования!";
                    return bRet;
                }
                if (this.m_objAvailability == null)
                {
                    strErr = "Необходимо указать возможность установки!";
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
        /// Добавляет оборудование в базу данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="uuidObjectId">идентификатор РТТ</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Add(System.Guid uuidObjectId,
         UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
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
                    cmd.Parameters.Clear();
                }

                System.String strAddCmd = "";
                strAddCmd = System.String.Format("[{0}].[dbo].[sp_AddEquipmentToRtt]", objProfile.GetOptionsDllDBName());

                cmd.Parameters.Clear();
                cmd.CommandText = strAddCmd;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RttEquipment_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Equipment_Quantity", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Availability_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EquipmentType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Rtt_Guid", System.Data.SqlDbType.UniqueIdentifier));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Equipment_Quantity"].Value = this.m_iQuantity;
                cmd.Parameters["@Availability_Guid"].Value = this.m_objAvailability.ID;
                cmd.Parameters["@EquipmentType_Guid"].Value = this.m_objEquipmentType.ID;
                cmd.Parameters["@Rtt_Guid"].Value = uuidObjectId;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    this.m_uuidId = (System.Guid)cmd.Parameters["@RttEquipment_Guid"].Value;
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
                if (cmdSQL == null)
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
        /// Сохраняет в БД список оборудования для РТТ
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="objTradeEquipmentList">список оборудования</param>
        /// <param name="objTradeEquipmentForDeleteList">список оборудования на удаление</param>
        /// <param name="uuidObjectId">идентификатор РТТ</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean SaveTradeEquipmentList(List<CTradeEquipment> objTradeEquipmentList, 
            List<CTradeEquipment> objTradeEquipmentForDeleteList,  System.Guid uuidObjectId, 
            UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            if (((objTradeEquipmentList == null) || (objTradeEquipmentList.Count == 0)) && ((objTradeEquipmentForDeleteList == null) || (objTradeEquipmentForDeleteList.Count == 0))) { return true; }
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                // для начала проверим, что нам пришло в списке
                if ((objTradeEquipmentList != null) && (objTradeEquipmentList.Count > 0))
                {
                    System.Boolean bIsAllValid = true;
                    foreach (CTradeEquipment objItem in objTradeEquipmentList)
                    {
                        if (objItem.IsAllParametersValid(ref strErr) == false)
                        {
                            bIsAllValid = false;
                            break;
                        }
                    }
                    if (bIsAllValid == false)
                    {
                        return bRet;
                    }
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
                    cmd.Parameters.Clear();
                }

                System.Int32 iRes = 0;
                if ((objTradeEquipmentForDeleteList != null) && (objTradeEquipmentForDeleteList.Count > 0))
                {
                    foreach ( CTradeEquipment objTradeEquipment in objTradeEquipmentForDeleteList )
                    {
                        if (objTradeEquipment.Id.CompareTo(System.Guid.Empty) == 0) { continue; }
                        iRes = (objTradeEquipment.Remove(uuidObjectId, objProfile, cmd, ref strErr) == true) ? 0 : 1;
                        if (iRes != 0) { break; }
                    }
                }

                if (iRes == 0)
                {
                    if ((objTradeEquipmentList != null) && (objTradeEquipmentList.Count > 0))
                    {
                        // теперь в цикле добавим в БД каждый член из списка
                        foreach (CTradeEquipment objTradeEquipment in objTradeEquipmentList)
                        {
                            if (objTradeEquipment.Id.CompareTo( System.Guid.Empty ) == 0)
                            {
                                // новое оборудование
                                iRes = (objTradeEquipment.Add(uuidObjectId, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                            }
                            else
                            {
                                iRes = (objTradeEquipment.Update(objProfile, cmd, ref strErr) == true) ? 0 : -1;
                            }

                            if (iRes != 0) { break; }
                        }
                    }
                }

                if (cmdSQL == null)
                {
                    if (iRes == 0)
                    {
                        // подтверждаем транзакцию
                        DBTransaction.Commit();
                    }
                    else
                    {
                        // откатываем транзакцию
                        DBTransaction.Rollback();
                    }
                    DBConnection.Close();
                }

                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                if (cmdSQL == null)
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

        #region Изменение свойств оборудования в БД
        /// <summary>
        /// Изменение свойств оборудования в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Update(
            UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
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
                    cmd.Parameters.Clear();
                }

                System.String strAddCmd = "";
                strAddCmd = System.String.Format("[{0}].[dbo].[sp_EditEquipmentRtt]", objProfile.GetOptionsDllDBName());

                cmd.Parameters.Clear();
                cmd.CommandText = strAddCmd;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RttEquipment_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Equipment_Quantity", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Availability_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EquipmentType_Guid", System.Data.SqlDbType.UniqueIdentifier));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@RttEquipment_Guid"].Value = this.Id;
                cmd.Parameters["@Equipment_Quantity"].Value = this.m_iQuantity;
                cmd.Parameters["@Availability_Guid"].Value = this.m_objAvailability.ID;
                cmd.Parameters["@EquipmentType_Guid"].Value = this.m_objEquipmentType.ID;
                cmd.ExecuteNonQuery();

                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = "Ошибка редактирования свойств оборудования. Текст ошибки: " + (System.String)cmd.Parameters["@ERROR_MES"].Value;
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
                if (cmdSQL == null)
                {
                    DBTransaction.Rollback();
                }
                strErr = "Ошибка редактирования свойств оборудования. Текст ошибки: " + f.Message;
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
    /// <summary>
    /// Класс "Код сегментации"
    /// </summary>
    public class CSalesSegmentation : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Код сегментации
        /// </summary>
        private System.String m_strCode;
        /// <summary>
        /// Код сегментации
        /// </summary>
        [DisplayName("Код сегментации")]
        [Description("Код сегментации")]
        [Category("1. Обязательные значения")]
        public System.String Code
        {
            get { return m_strCode; }
            set { m_strCode = value; }
        }
        /// <summary>
        /// Описание
        /// </summary>
        private System.String m_strDescription;
        /// <summary>
        /// Описание
        /// </summary>
        [DisplayName("Примечание")]
        [Description("Примечание")]
        [Category("2. Необязательные значения")]
        public System.String Description
        {
            get { return m_strDescription; }
            set { m_strDescription = value; }
        }
        #endregion

        #region Конструктор
        public CSalesSegmentation()
        {
            ID = System.Guid.Empty;
            Name = "";
            m_strCode = "";
            m_strDescription = "";
        }
        public CSalesSegmentation(System.Guid uuidID, System.String strCode, System.String strName, System.String strDscrpn)
        {
            ID = uuidID;
            m_strCode = strCode;
            Name = strName;
            m_strDescription = strDscrpn;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список объектов "сегментация"
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список объектов "сегментация"</returns>
        public static List<CSalesSegmentation> GetSegmentationList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CSalesSegmentation> objList = new List<CSalesSegmentation>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetSegmentationCode]", objProfile.GetOptionsDllDBName());
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
                        strDscrpn = ( rs["Segmentation_Description"] == System.DBNull.Value ) ? "" : (System.String)rs["Segmentation_Description"];
                        objList.Add(new CSalesSegmentation((System.Guid)rs["Segmentation_Guid"], (System.String)rs["Segmentation_Code"], (System.String)rs["Segmentation_Name"], strDscrpn));
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
                "Не удалось получить список объектов 'сегментация'.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Возвращает список рынков сбыта
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список рынков сбыта</returns>
        public static List<System.String> GetSegmentationListLevel1(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<System.String> objList = new List<System.String>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetSegmentationLevel1]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add( (System.String)rs["SegmentationCodeLevel"] );
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
                "Не удалось получить список рынков сбыта.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Возвращает список каналов сбыта
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список рынков сбыта</returns>
        public static List<System.String> GetSegmentationListLevel2(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<System.String> objList = new List<System.String>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetSegmentationLevel2]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add((System.String)rs["SegmentationCodeLevel"]);
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
                "Не удалось получить список каналов сбыта.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        #endregion

        #region IsAllParametersValid
        /// <summary>
        /// Проверка свойств перед сохранением
        /// </summary>
        /// <returns>true - ошибок нет; false - ошибка</returns>
        public override System.Boolean IsAllParametersValid()
        {
            System.Boolean bRet = false;
            try
            {
                if (this.Name == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать название!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                System.String strErr = "";
                if (CheckSegmentationCode( this.m_strCode, ref strErr ) == false)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show( strErr, "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }

                bRet = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка проверки свойств.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return bRet;
        }
        /// <summary>
        /// Проверяет строку на соответствие правилам оформления кода сегментации
        /// </summary>
        /// <param name="strCode">строка</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - строка соответствует требованиям; false - строка не соответствует требованиям или ошибка</returns>
        public System.Boolean CheckSegmentationCode(System.String strCode, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Int32 iLengthCode = 7;
            try
            {
                if (strCode == "")
                {
                    strErr = "Необходимо указать код сегментации!";
                    return bRet;
                }
                if (strCode.Trim().Length != iLengthCode)
                {
                    strErr = "Код сегментации должен содержать " + System.Convert.ToString(iLengthCode) + " символов!";
                    return bRet;
                }
                // рынок
                if( (strCode.Trim()[0] != 'A') && (strCode.Trim()[0] != 'B') )
                {
                    strErr = "Первый символ должен указывать на рынок: А, В";
                    return bRet;
                }
                // канал сбыта
                System.Boolean bIsChanelValid = true;
                for (System.Int32 i = 1; i <= 2; i++)
                {
                    if ((strCode[i] != '0') && (strCode[i] != '1') && (strCode[i] != '2') && (strCode[i] != '3')
                         && (strCode[i] != '4') && (strCode[i] != '5') && (strCode[i] != '6') && (strCode[i] != '7')
                         && (strCode[i] != '8') && (strCode[i] != '9'))
                    {
                        bIsChanelValid = false;
                        break;
                    }
                }
                if (bIsChanelValid==false)
                {
                    strErr = "Неверно указан канал сбыта: '" + strCode.Substring(1, 2) + "'."; 
                    return bRet;
                }
                System.Boolean bIsSubChanelValid = true;
                for (System.Int32 i2 = 3; i2 <= 6; i2++)
                {
                    if ((strCode[i2] != '0') && (strCode[i2] != '1') && (strCode[i2] != '2') && (strCode[i2] != '3')
                         && (strCode[i2] != '4') && (strCode[i2] != '5') && (strCode[i2] != '6') && (strCode[i2] != '7')
                         && (strCode[i2] != '8') && (strCode[i2] != '9'))
                    {
                        bIsSubChanelValid = false;
                        break;
                    }
                }
                if (bIsSubChanelValid == false)
                {
                    strErr = "Неверно указан субканал сбыта: '" + strCode.Substring(3, 4) + "'.";
                    return bRet;
                }

                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr = "Ошибка проверки свойств кода сегментации. nТекст ошибки: " + f.Message;
            }
            return bRet;
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddSegmentationCode]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Segmentation_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Segmentation_Code", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Segmentation_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Segmentation_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Segmentation_Code"].Value = this.m_strCode;
                cmd.Parameters["@Segmentation_Name"].Value = this.Name;
                if (this.m_strDescription == "")
                {
                    cmd.Parameters["@Segmentation_Description"].IsNullable = true;
                    cmd.Parameters["@Segmentation_Description"].Value = null;
                }
                else
                {
                    cmd.Parameters["@Segmentation_Description"].IsNullable = false;
                    cmd.Parameters["@Segmentation_Description"].Value = this.m_strDescription;
                }
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@Segmentation_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания объектa 'код сегментации'.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать объект 'код сегментации'.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteSegmentationCode]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Segmentation_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Segmentation_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления объектa 'код сегментации'.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить объект 'код сегментации'.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditSegmentationCode]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Segmentation_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Segmentation_Code", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Segmentation_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Segmentation_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Segmentation_Guid"].Value = this.ID;
                cmd.Parameters["@Segmentation_Code"].Value = this.m_strCode;
                cmd.Parameters["@Segmentation_Name"].Value = this.Name;
                if (this.m_strDescription == "")
                {
                    cmd.Parameters["@Segmentation_Description"].IsNullable = true;
                    cmd.Parameters["@Segmentation_Description"].Value = null;
                }
                else
                {
                    cmd.Parameters["@Segmentation_Description"].IsNullable = false;
                    cmd.Parameters["@Segmentation_Description"].Value = this.m_strDescription;
                }
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств объектa 'код сегментации'.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства объектa 'код сегментации'.\n\nТекст ошибки: " + f.Message, "Внимание",
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
            return m_strCode;
        }
    }
    /// <summary>
    /// День в расписании работы
    /// </summary>
    public class CActionPeriodPoint
    {
        #region Свойства
        /// <summary>
        /// Вкл.
        /// </summary>
        private System.Boolean m_bEnable;
        /// <summary>
        /// Вкл.
        /// </summary>
        public System.Boolean Enable
        {
            get { return m_bEnable; }
            set { m_bEnable = value; }
        }
        /// <summary>
        /// День недели №п/п
        /// </summary>
        private System.Int32 m_iDayNum;
        /// <summary>
        /// День недели №п/п
        /// </summary>
        public System.Int32 DayNum
        {
            get { return m_iDayNum; }
        }
        /// <summary>
        /// Время работы
        /// </summary>
        public System.String SheduleString
        {
            get
            {
                if (m_bEnable == true)
                {
                    return ( m_TimeStart.ToShortTimeString() + " - " + m_iTimeFinish.ToShortTimeString() );
                }
                else
                {
                    return "";
                }
            }
        }
        /// <summary>
        /// День недели (наименование)
        /// </summary>
        public System.String DayName
        {
            get { return GetDayName(m_iDayNum); }
        }
        private System.String GetDayName(System.Int32 iDayNumber)
        {
            System.String strRet = "";
            switch (iDayNumber)
            {
                case 0:
                    {
                        strRet = "перерыв ";
                        break;
                    }
                case 1:
                    {
                        strRet = "Пн. ";
                        break;
                    }
                case 2:
                    {
                        strRet = "Вт. ";
                        break;
                    }
                case 3:
                    {
                        strRet = "Ср. ";
                        break;
                    }
                case 4:
                    {
                        strRet = "Чт. ";
                        break;
                    }
                case 5:
                    {
                        strRet = "Птн. ";
                        break;
                    }
                case 6:
                    {
                        strRet = "Сб. ";
                        break;
                    }
                case 7:
                    {
                        strRet = "Вск. ";
                        break;
                    }
                default:
                    {
                        strRet = "";
                        break;
                    }
            }
            return strRet;
        }
        /// <summary>
        /// Начало работы
        /// </summary>
        private System.DateTime m_TimeStart;
        /// <summary>
        /// Начало работы
        /// </summary>
        public System.DateTime TimeStart
        {
            get { return m_TimeStart; }
            set { m_TimeStart = value; }
        }
        /// <summary>
        /// Конец работы
        /// </summary>
        private System.DateTime m_iTimeFinish;
        /// <summary>
        /// Конец работы
        /// </summary>
        public System.DateTime TimeFinish
        {
            get { return m_iTimeFinish; }
            set { m_iTimeFinish = value; }
        }
        #endregion

        #region Конструктор
        public CActionPeriodPoint(System.Int32 iDayNum, System.Boolean bEnable,
            System.DateTime dtTimeStart, System.DateTime dtTimeFinish)
        {
            m_iDayNum = iDayNum;
            m_bEnable = bEnable;
            m_TimeStart = dtTimeStart;
            m_iTimeFinish = dtTimeFinish;
        }
        #endregion

    }
    /// <summary>
    /// Класс "Время работы РТТ"
    /// </summary>
    public class CRttActionPeriod
    {

        #region Свойства
        /// <summary>
        /// Расписание работы
        /// </summary>
        private List<CActionPeriodPoint> m_objActionPeriodPointList;
        /// <summary>
        /// Расписание работы
        /// </summary>
        public List<CActionPeriodPoint> ActionPeriodPointList
        {
            get { return m_objActionPeriodPointList; }
            set { m_objActionPeriodPointList = value; }
        }
        /// <summary>
        /// Расписание в виде сроки
        /// </summary>
        public System.String ShortShedule
        {
            get
            {
                System.String strRet = "";
                System.Boolean bPrevIsCheck = false;
                     
                System.String strCurrShedule = "";
                for (System.Int32 i = 1; i <= 7; i++)
                {
                    if (m_objActionPeriodPointList[i].Enable == true)
                    {
                        if( (i > 1) && (bPrevIsCheck == true))
                        {
                            strRet += (", ");
                        }
                        if (m_objActionPeriodPointList[i].SheduleString != strCurrShedule)
                        {
                            if( (strCurrShedule != "") && (bPrevIsCheck == true))
                            {
                                strRet += (strCurrShedule);
                            }
                            strRet += (" " + m_objActionPeriodPointList[i].DayName);
                            strCurrShedule = m_objActionPeriodPointList[i].SheduleString;
                        }
                        else
                        {
                            strRet += (m_objActionPeriodPointList[i].DayName);
                            if (i == 7) { strRet += (": " + strCurrShedule); }
                        }
                        bPrevIsCheck = true;
                    }
                    else
                    {
                        if (bPrevIsCheck == true)
                        {
                            strRet += (": " + strCurrShedule);
                        }
                        bPrevIsCheck = false;
                    }
                }
                if (m_objActionPeriodPointList[0].Enable == true)
                {
                    strRet += ( " " + m_objActionPeriodPointList[0].DayName + ": " + m_objActionPeriodPointList[0].SheduleString );
                }
                return strRet;
            }
        }
        #endregion

        #region Конструктор
        public CRttActionPeriod()
        {
            m_objActionPeriodPointList = null;
        }
        #endregion

        #region Методы
        /// <summary>
        /// Обнуляет расписание работы
        /// </summary>
        public void InitActionPeriodPointList()
        {
            try
            {
                if (this.m_objActionPeriodPointList == null) { this.m_objActionPeriodPointList = new List<CActionPeriodPoint>(); }
                else { this.m_objActionPeriodPointList.Clear(); }

                System.String strNullTime = "01.01.2000 00:00:00";
                System.DateTime dtNullTime = System.DateTime.Parse(strNullTime);
                for (System.Int32 i = 0; i <= 7; i++)
                {
                    this.m_objActionPeriodPointList.Add(new CActionPeriodPoint(i, false, dtNullTime, dtNullTime));
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось обнулить расписание работы РТТ.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return ;
        }
        /// <summary>
        /// Загружает из БД расписание работы РТТ
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="uuidRttId">уникальный идентификатор РТТ</param>
        public void LoadShedule(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidRttId)
        {
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                // по-хорошему сперва загрузим список с днями, а затем сделаем запрос в БД и проставим время и признак "Вкл."
                InitActionPeriodPointList();

                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                            "Не удалось получить соединение с базой данных.", "Внимание",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return ;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetRttActionPeriod]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Rtt_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Rtt_Guid"].Value = uuidRttId;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.Boolean bEnable = false;
                    System.Int32 iDayNumber = -1;
                    while (rs.Read())
                    {
                        iDayNumber = (System.Int32)rs["Day_Id"];
                        bEnable = System.Convert.ToBoolean(rs["IsEnable"]);
                        foreach (CActionPeriodPoint objPoint in this.m_objActionPeriodPointList)
                        {
                            if (objPoint.DayNum == iDayNumber)
                            {
                                objPoint.Enable = bEnable;
                                if (rs["TimeStart"] != System.DBNull.Value)
                                {
                                    objPoint.TimeStart = System.DateTime.Parse( (objPoint.TimeStart.ToShortDateString() + " " + System.Convert.ToString(rs["TimeStart"] ) ));
                                }
                                if (rs["TimeFinish"] != System.DBNull.Value)
                                {
                                    objPoint.TimeFinish = System.DateTime.Parse((objPoint.TimeFinish.ToShortDateString() + " " + System.Convert.ToString(rs["TimeFinish"])));
                                }
                                break;
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
                "Не удалось получить расписание работы РТТ.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return ;
        }
        /// <summary>
        /// Сохраняет в БД расписание работы РТТ
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="uuidRttId">уи РТТ</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public System.Boolean SaveShedule( UniXP.Common.CProfile objProfile, 
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidRttId, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if ((this.m_objActionPeriodPointList == null) || (this.m_objActionPeriodPointList.Count == 0)) { return bRet; }

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

                System.Int32 iRes = 0;

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddRttActionPeriodToRtt]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Rtt_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IsEnable", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Day_Id", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@TimeStart", System.Data.SqlDbType.DateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@TimeFinish", System.Data.SqlDbType.DateTime));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Rtt_Guid"].Value = uuidRttId;

                foreach (CActionPeriodPoint objPoint in this.m_objActionPeriodPointList)
                {
                    cmd.Parameters["@IsEnable"].Value = objPoint.Enable;
                    cmd.Parameters["@Day_Id"].Value = objPoint.DayNum;
                    cmd.Parameters["@TimeStart"].Value = objPoint.TimeStart;
                    cmd.Parameters["@TimeFinish"].Value = objPoint.TimeFinish;
                    cmd.ExecuteNonQuery();
                    iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                    if (iRes != 0)
                    {
                        strErr = "Ошибка сохранение расписания работы. Текст ошибки: " + (System.String)cmd.Parameters["@ERROR_MES"].Value;
                        break;
                    }

                }

                if (cmdSQL == null)
                {
                    if (iRes == 0)
                    {
                        // подтверждаем транзакцию
                        DBTransaction.Commit();
                    }
                    else
                    {
                        // откатываем транзакцию
                        DBTransaction.Rollback();
                    }
                    DBConnection.Close();
                }

                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                if (cmdSQL == null)
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
    /// <summary>
    /// Класс "Розничная торговая точка"
    /// </summary>
    public class CRtt
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
        /// Код РТТ
        /// </summary>
        private System.String m_strCode;
        /// <summary>
        /// Код РТТ
        /// </summary>
        public System.String Code
        {
            get { return m_strCode; }
            set { m_strCode = value; }
        }
        /// <summary>
        /// Сокращенное наименование
        /// </summary>
        private System.String m_strShortName;
        /// <summary>
        /// Сокращенное наименование
        /// </summary>
        public System.String ShortName
        {
            get { return m_strShortName; }
            set { m_strShortName = value; }
        }
        /// <summary>
        /// Полное наименование
        /// </summary>
        private System.String m_strFullName;
        /// <summary>
        /// Полное наименование
        /// </summary>
        public System.String FullName
        {
            get { return m_strFullName; }
            set { m_strFullName = value; }
        }
        /// <summary>
        /// Тип РТТ
        /// </summary>
        private CRttType m_objRttType;
        /// <summary>
        /// Тип РТТ
        /// </summary>
        public CRttType RttType
        {
            get { return m_objRttType; }
            set { m_objRttType = value; }
        }
        /// <summary>
        /// Состояние РТТ
        /// </summary>
        private CRttActiveType m_objRttActiveType;
        /// <summary>
        /// Состояние РТТ
        /// </summary>
        public CRttActiveType RttActiveType
        {
            get { return m_objRttActiveType; }
            set { m_objRttActiveType = value; }
        }
        /// <summary>
        /// Спец. код
        /// </summary>
        private CRttSpecCode m_objRttSpecCode;
        /// <summary>
        /// Спец. код
        /// </summary>
        public CRttSpecCode RttSpecCode
        {
            get { return m_objRttSpecCode; }
            set { m_objRttSpecCode = value; }
        }
        /// <summary>
        /// Код сегментации
        /// </summary>
        private CSalesSegmentation m_objSegmentation;
        /// <summary>
        /// Код сегментации
        /// </summary>
        public CSalesSegmentation Segmentation
        {
            get { return m_objSegmentation; }
            set { m_objSegmentation = value; }
        }
        /// <summary>
        /// Субканал сбыта
        /// </summary>
        public CSegmentationSubChannel SegmentationSubChannel { get; set; }
        /// <summary>
        /// Рынок сбыта
        /// </summary>
        public CSegmentationMarket SegmentationMarket { get; set; }
        /// <summary>
        /// Расписание работы
        /// </summary>
        private CRttActionPeriod m_objRttActionPeriod;
        /// <summary>
        /// Расписание работы
        /// </summary>
        public CRttActionPeriod ActionPeriod
        {
            get { return m_objRttActionPeriod; }
            set { m_objRttActionPeriod = value; }
        }
        /// <summary>
        /// Список адресов
        /// </summary>
        private List<CAddress> m_objAddressList;
        /// <summary>
        /// Список адресов
        /// </summary>
        public List<CAddress> AddressList
        {
            get { return m_objAddressList; }
            set { m_objAddressList = value; }
        }
        /// <summary>
        /// Список контактов
        /// </summary>
        private List<CContact> m_objContactList;
        /// <summary>
        /// Список контактов
        /// </summary>
        public List<CContact> ContactList
        {
            get { return m_objContactList; }
            set { m_objContactList = value; }
        }
        /// <summary>
        /// Список оборудования
        /// </summary>
        private List<CTradeEquipment> m_objTradeEquipmentList;
        /// <summary>
        /// Список оборудования
        /// </summary>
        public List<CTradeEquipment> TradeEquipmentList
        {
            get { return m_objTradeEquipmentList; }
            set { m_objTradeEquipmentList = value; }
        }
        /// <summary>
        /// Каталог продукции
        /// </summary>
        private List<CProductCatalog> m_objProductCatalogList;
        /// <summary>
        /// Каталог продукции
        /// </summary>
        public List<CProductCatalog> ProductCatalogList
        {
            get { return m_objProductCatalogList; }
            set { m_objProductCatalogList = value; }
        }
        private List<CAddress> m_objAddressForDeleteList;
        public List<CAddress> AddressForDeleteList
        {
            get { return m_objAddressForDeleteList; }
            set { m_objAddressForDeleteList = value; }
        }
        private List<CContact> m_objContactForDeleteList;
        public List<CContact> ContactForDeleteList
        {
            get { return m_objContactForDeleteList; }
            set { m_objContactForDeleteList = value; }
        }
        private List<CTradeEquipment> m_objTradeEquipmentForDeleteList;
        public List<CTradeEquipment> TradeEquipmentForDeleteList
        {
            get { return m_objTradeEquipmentForDeleteList; }
            set { m_objTradeEquipmentForDeleteList = value; }
        }
        private List<CProductCatalog> m_objProductCatalogForDeleteList;
        public List<CProductCatalog> ProductCatalogForDeleteList
        {
            get { return m_objProductCatalogForDeleteList; }
            set { m_objProductCatalogForDeleteList = value; }
        }
        /// <summary>
        /// Визитная карточка
        /// </summary>
        public System.String VisitingCard
        {
            get
            {
                return(
                    ((m_objRttType == null) ? "" : m_objRttType.Name) + " " + 
                    m_strFullName + " " + 
                    (( ( m_objAddressList == null ) || ( m_objAddressList.Count == 0 )) ? "" : m_objAddressList[0].VisitingCard2)
                    );
            }
        }
        /// <summary>
        /// Признак того, что объект новый и в БД о нем информации нет
        /// </summary>
        private System.Boolean m_bNewObject;
        /// <summary>
        /// Признак того, что объект новый и в БД о нем информации нет
        /// </summary>
        public System.Boolean IsNewObject
        {
            get { return m_bNewObject; }
            set
            {
                m_bNewObject = value;
                if (m_bNewObject == true)
                {
                    if (ID.CompareTo(System.Guid.Empty) != 0) { ID = System.Guid.Empty; }
                    ClearIDFromChildObject();
                }
            }
        }
        public void ClearIDFromChildObject()
        {
            try
            {
                if ((this.AddressList != null) && (this.AddressList.Count > 0))
                {
                    foreach (CAddress objAddress in this.AddressList)
                    {
                        if (objAddress.IsNewObject == true) { objAddress.ID = System.Guid.Empty; }
                    }
                }
                if ((this.ContactList != null) && (this.ContactList.Count > 0))
                {
                    foreach (CContact objContact in this.ContactList)
                    {
                        if (objContact.IsNewObject == true) 
                        { 
                            objContact.ID = System.Guid.Empty;
                            objContact.ClearIDFromChildObject();
                        }
                    }
                }
                if ((this.TradeEquipmentList != null) && (this.TradeEquipmentList.Count > 0))
                {
                    foreach (CTradeEquipment objTradeEquipment in this.TradeEquipmentList)
                    {
                        if (objTradeEquipment.IsNewObject == true)
                        {
                            objTradeEquipment.Id = System.Guid.Empty;
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "ClearIDFromChildObject.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Тип лицензии
        /// </summary>
        private CLicenceType m_objLicenceType;
        /// <summary>
        /// Тип лицензии
        /// </summary>
        public CLicenceType LicenceType
        {
            get { return m_objLicenceType; }
            set { m_objLicenceType = value; }
        }
        private System.Boolean m_bObjectIsChanged;
        public System.Boolean IsChanged
        {
            get { return m_bObjectIsChanged; }
            set { m_bObjectIsChanged = value; }
        }
        #endregion

        #region Конструктор
        public CRtt()
        {
            m_uuidID = System.Guid.Empty;
            m_strShortName = "";
            m_strFullName = "";
            m_strCode = "";
            m_objRttType = null;
            m_objRttSpecCode = null;
            m_objSegmentation = null;
            m_objRttActiveType = null;
            m_objRttActionPeriod = null;
            m_objContactList = null;
            m_objAddressList = null;
            m_objTradeEquipmentList = null;
            m_objContactForDeleteList = null;
            m_objAddressForDeleteList = null;
            m_objTradeEquipmentForDeleteList = null;
            m_objProductCatalogForDeleteList = null;
            m_bNewObject = false;
            m_objLicenceType = null;
            m_bObjectIsChanged = false;
            SegmentationSubChannel = null;
            SegmentationMarket = null;
        }
        public CRtt( System.Guid uuidID, System.String strName, System.String strCode, CRttType objRttType,
            CRttActiveType objRttActiveType, CRttSpecCode objSpecCode, CSalesSegmentation objSegmentation, 
            CLicenceType objLicenceType)
        {
            m_uuidID = uuidID;
            m_strShortName = strName;
            m_strFullName = strName;
            m_strCode = strCode;
            m_objRttType = objRttType;
            m_objRttSpecCode = objSpecCode;
            m_objSegmentation = objSegmentation;
            m_objRttActiveType = objRttActiveType;
            m_objLicenceType = objLicenceType;
            m_objRttActionPeriod = null;
            m_objContactList = null;
            m_objAddressList = null;
            m_objTradeEquipmentList = null;
            m_objContactForDeleteList = null;
            m_objAddressForDeleteList = null;
            m_objTradeEquipmentForDeleteList = null;
            m_objProductCatalogForDeleteList = null;
            m_bNewObject = false;
            m_bObjectIsChanged = false;
            SegmentationSubChannel = null;
            SegmentationMarket = null;

        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список РТТ
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="CustomerId">УИ клиента</param>
        /// <returns>список РТТ</returns>
        public static List<CRtt> GetRttList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid CustomerId)
        {
            List<CRtt> objList = new List<CRtt>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetRttList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter( "@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter( "@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier ));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter( "@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter( "@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Customer_Guid"].Value = CustomerId;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CRtt((System.Guid)rs["Rtt_Guid"], (System.String)rs["Rtt_Name"], (System.String)rs["Rtt_Code"],
                            // тип РТТ
                            null,
                            //new CRttType((System.Guid)rs["RttType_Guid"], (System.String)rs["RttType_Name"]), 
                            // признак активности
                            new CRttActiveType((System.Guid)rs["RttActiveType_Guid"], (System.String)rs["RttActiveType_Name"]),
                            // спеу код
                            new CRttSpecCode((System.Guid)rs["RttSpecCode_Guid"], (System.String)rs["RttSpecCode_Code"], ((rs["RttSpecCode_Description"] == System.DBNull.Value) ? "" : (System.String)rs["RttSpecCode_Description"] ) ),
                            // сегментация
                            new CSalesSegmentation((System.Guid)rs["Segmentation_Guid"], (System.String)rs["Segmentation_Code"], (System.String)rs["Segmentation_Name"], ((rs["Segmentation_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Segmentation_Description"])),
                            // тип лицензии
                            ((rs["LicenceType_Guid"] == System.DBNull.Value) ? null : new CLicenceType((System.Guid)rs["LicenceType_Guid"], (System.String)rs["LicenceType_Name"], ""))  )
                            
                            { 
                                SegmentationMarket = ( ( rs["SegmentationMarket_Guid"] == System.DBNull.Value ) ? null : new CSegmentationMarket() {ID = (System.Guid)rs["SegmentationMarket_Guid"]} ),
                                SegmentationSubChannel = ((rs["SegmentationSubChannel_Guid"] == System.DBNull.Value) ? null : new CSegmentationSubChannel() { ID = (System.Guid)rs["SegmentationSubChannel_Guid"] })
                            }
                            );
                    }
                }
                rs.Dispose();

                if (objList != null)
                {
                    System.String strErr = "";
                    foreach (CRtt objRtt in objList)
                    {
                        objRtt.InitAdvancedProperties(objProfile, cmd, ref strErr);
                    }
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
                "Не удалось получить список РТТ.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Возвращает список РТТ для импорта заказа
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="CustomerId">УИ клиента</param>
        /// <returns>список РТТ</returns>
        public static List<CRtt> GetRttListForImportSuppl(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid CustomerId)
        {
            List<CRtt> objList = new List<CRtt>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetRttList_2]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Customer_Guid"].Value = CustomerId;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CRtt() { ID = (System.Guid)rs["Rtt_Guid"], FullName = System.Convert.ToString(rs["RttAddress"]), Code = System.Convert.ToString(rs["Rtt_Code"]) });
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
                "Не удалось получить список РТТ.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }


        /// <summary>
        /// Запрашивает дополнительные свойства РТТ
        /// </summary>
        /// <param name="objProfile"></param>
        /// <param name="cmdSQL"></param>
        /// <param name="strErr"></param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public System.Boolean InitAdvancedProperties(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
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

                // список адресов 
                this.AddressList = CAddress.GetAddressList(objProfile, cmdSQL, EnumObject.Rtt, this.m_uuidID);

                // список контактов
                this.ContactList = CContact.GetContactList(objProfile, cmdSQL, EnumObject.Rtt, this.m_uuidID);

                // оборудование
                this.TradeEquipmentList = CTradeEquipment.GetTradeEquipmentList(objProfile, cmdSQL, this.m_uuidID);

                // каталог продукции
                this.ProductCatalogList = CProductCatalog.GetProductCatalogList(objProfile, cmdSQL, this.m_uuidID);

                // расписание работы
                this.m_objRttActionPeriod = new CRttActionPeriod();
                this.m_objRttActionPeriod.LoadShedule(objProfile, cmdSQL, this.ID);

                // рынок сбыта
                if (this.SegmentationMarket != null)
                {
                    System.String SegmentationMarket_Code;
                    System.String SegmentationMarket_Name; 

                    if (CSegmentationMarketDataBaseModel.GetSegmentationMarketInfo(objProfile, cmdSQL, this.SegmentationMarket.ID,
                        out SegmentationMarket_Code, out SegmentationMarket_Name, ref strErr) == true)
                    {
                        this.SegmentationMarket.Code = SegmentationMarket_Code;
                        this.SegmentationMarket.Name = SegmentationMarket_Name;
                    }
                }

                // субканал сбыта
                if (this.SegmentationSubChannel != null)
                {
                    System.Guid SegmentationChannel_Guid; 
                    System.String SegmentationSubChannel_Code;
                    System.String SegmentationSubChannel_Name; 
                    System.String SegmentationChannel_Code;
                    System.String SegmentationChannel_Name;

                    if (CSegmentationSubChanelDataBaseModel.GetSegmentationSubChannelInfo(objProfile, cmdSQL, this.SegmentationSubChannel.ID,
                        out SegmentationChannel_Guid, out SegmentationSubChannel_Code, out SegmentationSubChannel_Name,
                        out SegmentationChannel_Code, out SegmentationChannel_Name, ref strErr) == true)
                    {
                        this.SegmentationSubChannel.Code = SegmentationSubChannel_Code;
                        this.SegmentationSubChannel.Name = SegmentationSubChannel_Name;
                        this.SegmentationSubChannel.SegmentationChanel = new CSegmentationChanel()
                        {
                            ID = SegmentationChannel_Guid,
                            Code = SegmentationChannel_Code,
                            Name = SegmentationChannel_Name
                        };
                    }
                }

                bRet = ((this.AddressList != null) && (this.ContactList != null) && (this.m_objRttActionPeriod != null));

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                strErr += ("Не удалось запросить дополнительные свойства РТТ. Текст ошибки: " + f.Message);
                //DevExpress.XtraEditors.XtraMessageBox.Show(
                //"Не удалось запросить дополнительные свойства РТТ.\n\nТекст ошибки: " + f.Message, "Внимание",
                //System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return bRet;
        }
        #endregion

        #region Сохранить в БД список РТТ
        /// <summary>
        /// Сохраняет в БД список РТТ
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="objRttList">список РТТ</param>
        /// <param name="objRttForDeleteList">список РТТ на удаление</param>
        /// <param name="uuidObjectId">идентификатор владельца РТТ</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean SaveRttList(List<CRtt> objRttList, List<CRtt> objRttForDeleteList, System.Guid uuidObjectId,
            UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            if (((objRttList == null) || (objRttList.Count == 0)) && ((objRttForDeleteList == null) || (objRttForDeleteList.Count == 0))) { return true; }
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                // для начала проверим, что нам пришло в списке
                if ((objRttList != null) && (objRttList.Count > 0))
                {
                    System.Boolean bIsAllValid = true;
                    foreach (CRtt objItem in objRttList)
                    {
                        if (objItem.IsChanged)
                        {
                            if (objItem.IsAllParametersValid(ref strErr) == false)
                            {
                                bIsAllValid = false;
                                break;
                            }
                        }
                    }
                    if (bIsAllValid == false)
                    {
                        return bRet;
                    }
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
                    cmd.Parameters.Clear();
                }

                System.Int32 iRes = 0;
                if ((objRttForDeleteList != null) && (objRttForDeleteList.Count > 0))
                {
                    foreach (CRtt objRtt in objRttForDeleteList)
                    {
                        if (objRtt.ID.CompareTo(System.Guid.Empty) == 0) { continue; }
                        iRes = (objRtt.Remove( objProfile, cmd, ref strErr ) == true) ? 0 : 1;
                        if (iRes != 0) { break; }
                    }
                }

                if (iRes == 0)
                {
                    // удалили, теперь добавим или сохраним
                    if ((objRttList != null) && (objRttList.Count > 0))
                    {
                        // теперь в цикле добавим в БД каждый член из списка
                        foreach (CRtt objRtt in objRttList)
                        {
                            if (objRtt.ID.CompareTo(System.Guid.Empty) == 0)
                            {
                                // новый 
                                iRes = ( objRtt.Add( objProfile, cmd, uuidObjectId, ref strErr) == true) ? 0 : -1;
                            }
                            else
                            {
                                if (objRtt.IsChanged)
                                {
                                    iRes = (objRtt.Update(objProfile, cmd, ref strErr) == true) ? 0 : -1;
                                }
                            }

                            if (iRes != 0) { break; }
                        }
                    }
                }

                if (cmdSQL == null)
                {
                    if (iRes == 0)
                    {
                        // подтверждаем транзакцию
                        DBTransaction.Commit();
                    }
                    else
                    {
                        // откатываем транзакцию
                        DBTransaction.Rollback();
                    }
                    DBConnection.Close();
                }

                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                if (cmdSQL == null)
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

        #region Добавить информацию о РТТ в базу данных
        /// <summary>
        /// Проверка свойств РТТ перед сохранением
        /// </summary>
        /// <param name="strErr">текст с ошибкой</param>
        /// <returns>true - все свойства корректны; false - ошибка</returns>
        public System.Boolean IsAllParametersValid(ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (this.FullName.Trim() == "")
                {
                    strErr = "РТТ: Необходимо указать название РТТ!";
                    return bRet;
                }
                //if (this.Code.Trim() == "")
                //{
                //    strErr = "Необходимо указать код клиента!";
                //    return bRet;
                //}
                if (this.RttActiveType == null)
                {
                    strErr = "РТТ: Необходимо указать признак активности РТТ!";
                    return bRet;
                }
                if (this.RttSpecCode == null)
                {
                    strErr = "РТТ: Необходимо указать спецкод РТТ!";
                    return bRet;
                }
                //if (this.Segmentation == null)
                //{
                //    strErr = "РТТ: Необходимо указать код сегментации!";
                //    return bRet;
                //}
                //if (this.Segmentation.CheckSegmentationCode( this.Segmentation.Code, ref strErr ) == false )
                //{
                //    return bRet;
                //}
                if (this.SegmentationMarket == null)
                {
                    strErr = "РТТ (Сегментация): Необходимо указать рынок и субканал сбыта!";
                    return bRet;
                }
                if (this.SegmentationSubChannel == null)
                {
                    strErr = "РТТ (Сегментация): Необходимо указать рынок и субканал сбыта!";
                    return bRet;
                }
                if( (this.AddressList == null) || (this.AddressList.Count == 0) )
                {
                    strErr = "РТТ: Необходимо указать адрес!";
                    return bRet;
                }
                if ((this.AddressList != null) && (this.AddressList.Count > 0))
                {
                    System.Boolean bIsAllAddressValid = true;
                    foreach (CAddress objItem in this.AddressList)
                    {
                        if (objItem.IsAllParametersValid(ref strErr) == false)
                        {
                            bIsAllAddressValid = false;
                            break;
                        }
                    }
                    if (bIsAllAddressValid == false)
                    {
                        return bRet;
                    }
                }
                if ((this.ContactList != null) && (this.ContactList.Count > 0))
                {
                    System.Boolean bIsAllContactValid = true;
                    foreach (CContact objItem in this.ContactList)
                    {
                        if (objItem.IsAllParametersValid(ref strErr) == false)
                        {
                            bIsAllContactValid = false;
                            break;
                        }
                    }
                    if (bIsAllContactValid == false)
                    {
                        return bRet;
                    }
                }
                if ((this.TradeEquipmentList != null) && (this.TradeEquipmentList.Count > 0))
                {
                    System.Boolean bIsAllTradeEquipmentValid = true;
                    foreach (CTradeEquipment objItem in this.TradeEquipmentList)
                    {
                        if (objItem.IsAllParametersValid(ref strErr) == false)
                        {
                            bIsAllTradeEquipmentValid = false;
                            break;
                        }
                    }
                    if (bIsAllTradeEquipmentValid == false)
                    {
                        return bRet;
                    }
                }

                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr = "Ошибка проверки свойств РТТ. Текст ошибки: " + f.Message;
            }
            return bRet;
        }
        /// <summary>
        /// Добавляет в базу данных информацию о РТТ
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="uuidCustomerId">УИ клиента</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Add(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, 
               System.Guid uuidCustomerId, ref System.String strErr)
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
                    cmd.Parameters.Clear();
                }
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddRtt]", objProfile.GetOptionsDllDBName()); ;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Rtt_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RttActiveType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RttSpecCode_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationSubChannel_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationMarket_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LicenceType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Rtt_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Rtt_Name"].Value = this.FullName;
                cmd.Parameters["@Customer_Guid"].Value = uuidCustomerId;
                cmd.Parameters["@RttActiveType_Guid"].Value = this.RttActiveType.ID;
                cmd.Parameters["@RttSpecCode_Guid"].Value = this.RttSpecCode.ID;
                cmd.Parameters["@SegmentationSubChannel_Guid"].Value = this.SegmentationSubChannel.ID;
                cmd.Parameters["@SegmentationMarket_Guid"].Value = this.SegmentationMarket.ID;

                if (this.LicenceType == null)
                {
                    cmd.Parameters["@LicenceType_Guid"].IsNullable = true;
                    cmd.Parameters["@LicenceType_Guid"].Value = null;
                }
                else
                {
                    cmd.Parameters["@LicenceType_Guid"].IsNullable = false;
                    cmd.Parameters["@LicenceType_Guid"].Value = this.LicenceType.ID;
                }

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    this.ID = (System.Guid)cmd.Parameters["@Rtt_Guid"].Value;
                    // теперь списки контактов, адресов, расписание работы
                    if ((this.ContactList != null) && (this.ContactList.Count > 0))
                    {
                        iRes = (CContact.SaveContactList(this.ContactList, null, EnumObject.Rtt, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                    }
                    if (iRes == 0)
                    {
                        if ((this.AddressList != null) && (this.AddressList.Count > 0))
                        {
                            iRes = (CAddress.SaveAddressList(this.AddressList, null, EnumObject.Rtt, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.TradeEquipmentList != null) && (this.TradeEquipmentList.Count > 0))
                        {
                            iRes = (CTradeEquipment.SaveTradeEquipmentList(this.TradeEquipmentList, null, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.ProductCatalogList != null) && (this.ProductCatalogList.Count > 0))
                        {
                            iRes = (CProductCatalog.SaveProductCatalogList(this.ProductCatalogList, null, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if( this.ActionPeriod != null)
                        {
                            iRes = ( this.ActionPeriod.SaveShedule( objProfile, cmd, this.ID, ref strErr) == true) ? 0 : -1;
                        }
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

        #region Сохранить в базе данных изменения в описании РТТ
        /// <summary>
        /// Сохраняет в базе данных изменения в описании клиента
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="objContactDeletedList">список удаленных контактов</param>
        /// <param name="objAddressDeletedList">список удаленных адресов</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Update(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            ref System.String strErr)
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
                    cmd.Parameters.Clear();
                }
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditRtt]", objProfile.GetOptionsDllDBName()); ;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Rtt_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RttActiveType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RttSpecCode_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationSubChannel_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationMarket_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LicenceType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Rtt_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Rtt_Guid"].Value = this.ID;
                cmd.Parameters["@Rtt_Name"].Value = this.FullName;
                cmd.Parameters["@SegmentationSubChannel_Guid"].Value = this.SegmentationSubChannel.ID;
                cmd.Parameters["@SegmentationMarket_Guid"].Value = this.SegmentationMarket.ID;
                cmd.Parameters["@RttActiveType_Guid"].Value = this.RttActiveType.ID;
                cmd.Parameters["@RttSpecCode_Guid"].Value = this.RttSpecCode.ID;
                if (this.LicenceType == null)
                {
                    cmd.Parameters["@LicenceType_Guid"].IsNullable = true;
                    cmd.Parameters["@LicenceType_Guid"].Value = null;
                }
                else
                {
                    cmd.Parameters["@LicenceType_Guid"].IsNullable = false;
                    cmd.Parameters["@LicenceType_Guid"].Value = this.LicenceType.ID;
                }

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    // теперь списки контактов, адресов, лицензий и телефонов
                    if ((this.ContactList != null) || (this.ContactForDeleteList != null))
                    {
                        iRes = (CContact.SaveContactList(this.ContactList, this.ContactForDeleteList, EnumObject.Rtt, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                    }
                    if (iRes == 0)
                    {
                        if ((this.AddressList != null) || (this.AddressForDeleteList != null))
                        {
                            iRes = (CAddress.SaveAddressList(this.AddressList, this.AddressForDeleteList, EnumObject.Rtt, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.TradeEquipmentList != null) || (this.TradeEquipmentForDeleteList != null))
                        {
                            iRes = (CTradeEquipment.SaveTradeEquipmentList(this.TradeEquipmentList, this.TradeEquipmentForDeleteList, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.ProductCatalogList != null) || (this.ProductCatalogForDeleteList != null))
                        {
                            iRes = (CProductCatalog.SaveProductCatalogList(this.ProductCatalogList, this.ProductCatalogForDeleteList, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if (this.ActionPeriod != null)
                        {
                            iRes = (this.ActionPeriod.SaveShedule(objProfile, cmd, this.ID, ref strErr) == true) ? 0 : -1;
                        }
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

        #region Удалить описание РТТ из базы данных
        /// <summary>
        /// Удаляет описание РТТ из базы данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Remove( UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
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

                // сперва удалим список адресов
                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteRtt]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Rtt_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Rtt_Guid"].Value = this.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                var aa = cmd.Parameters["@ERROR_MES"].Value;
                var bb = cmd.Parameters["@ERROR_NUM"].Value;

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
                    DBConnection.Close();
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
            return ( ( ( this.AddressList != null ) && ( this.AddressList.Count > 0 ) ) ? ( FullName + " " + this.AddressList[0].FullName ) : FullName );
        }

    }
}
