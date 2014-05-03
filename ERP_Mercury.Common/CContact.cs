using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ERP_Mercury.Common
{

    #region Класс "Отдел"
    /// <summary>
    /// TypeConverter для списка отделов
    /// </summary>
    class DepartamentConverterForContact : TypeConverter
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

            CCity objCity = (CCity)context.Instance;
            System.Collections.Generic.List<CDepartament> objList = null;
            //System.Collections.Generic.List<CDepartament> objList = objCity.GetAllRegionList();

            return new StandardValuesCollection(objList);
        }
    }

    public class CDepartament : CBusinessObject
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
        [Description("Дополнительная информация")]
        [Category("2. Дополнительно")]
        public System.String Description
        {
            get { return m_strDescription; }
            set { m_strDescription = value; }
        }
        #endregion

        public CDepartament()
            : base()
        {
            m_strDescription = "";
        }
        public CDepartament(System.Guid uuidId, System.String strName, System.String strDescription)
        {
            this.ID = uuidId;
            this.Name = strName;
            this.Description = strDescription;
        }

        #region Список объектов
        public static List<CDepartament> GetDepartamentList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CDepartament> objList = new List<CDepartament>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetDepartment]", objProfile.GetOptionsDllDBName());
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
                        strDscrpn = (rs["Department_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Department_Description"];
                        objList.Add(new CDepartament((System.Guid)rs["Department_Guid"],
                            (System.String)rs["Department_Name"], strDscrpn));
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
                "Не удалось получить список отделов.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddDepartment]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Department_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Department_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Department_Name"].Value = this.Name;
                if (this.Description != "")
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Department_Description", System.Data.DbType.String));
                    cmd.Parameters["@Department_Description"].Value = this.Description;
                }
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@Department_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания отдела.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать одел.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteDepartment]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Department_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Department_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления отдела.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить отдел.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditDepartment]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Department_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Department_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Department_Guid"].Value = this.ID;
                cmd.Parameters["@Department_Name"].Value = this.Name;
                if (this.Description != "")
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Department_Description", System.Data.DbType.String));
                    cmd.Parameters["@Department_Description"].Value = this.Description;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения отдела.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства отдела.\n\nТекст ошибки: " + f.Message, "Внимание",
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
    #endregion

    #region Класс "Должность"
    /// <summary>
    /// TypeConverter для списка отделов
    /// </summary>
    class JobPositionConverterForContact : TypeConverter
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

            CCity objCity = (CCity)context.Instance;
            System.Collections.Generic.List<CJobPosition> objList = null;
            //System.Collections.Generic.List<CDepartament> objList = objCity.GetAllRegionList();

            return new StandardValuesCollection(objList);
        }
    }
    /// <summary>
    /// Класс "Должность
    /// </summary>
    public class CJobPosition : CBusinessObject
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
        [Description("Дополнительная информация")]
        [Category("2. Дополнительно")]
        public System.String Description
        {
            get { return m_strDescription; }
            set { m_strDescription = value; }
        }
        #endregion

        public CJobPosition()
            : base()
        {
            m_strDescription = "";
        }
        public CJobPosition(System.Guid uuidId, System.String strName, System.String strDescription)
        {
            this.ID = uuidId;
            this.Name = strName;
            this.Description = strDescription;
        }

        #region Список объектов
        public static List<CJobPosition> GetJobPositionList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CJobPosition> objList = new List<CJobPosition>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetJobPosition]", objProfile.GetOptionsDllDBName());
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
                        strDscrpn = (rs["JobPosition_Description"] == System.DBNull.Value) ? "" : (System.String)rs["JobPosition_Description"];
                        objList.Add(new CJobPosition((System.Guid)rs["JobPosition_Guid"],
                            (System.String)rs["JobPosition_Name"], strDscrpn));
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
                "Не удалось получить список занимаемых должностей.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddJobPosition]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@JobPosition_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@JobPosition_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@JobPosition_Name"].Value = this.Name;
                if (this.Description != "")
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@JobPosition_Description", System.Data.DbType.String));
                    cmd.Parameters["@JobPosition_Description"].Value = this.Description;
                }
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@JobPosition_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания должности.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать должность.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteJobPosition]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@JobPosition_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@JobPosition_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления должности.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить должность.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditJobPosition]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@JobPosition_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@JobPosition_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@JobPosition_Guid"].Value = this.ID;
                cmd.Parameters["@JobPosition_Name"].Value = this.Name;
                if (this.Description != "")
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@JobPosition_Description", System.Data.DbType.String));
                    cmd.Parameters["@JobPosition_Description"].Value = this.Description;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств должности.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства должности.\n\nТекст ошибки: " + f.Message, "Внимание",
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
    #endregion

    #region Класс "Тип телефонного номера"
    ///// <summary>
    ///// TypeConverter для списка отделов
    ///// </summary>
    //class PhoneTypeConverterForPhone : TypeConverter
    //{
    //    /// <summary>
    //    /// Будем предоставлять выбор из списка
    //    /// </summary>
    //    public override bool GetStandardValuesSupported(
    //      ITypeDescriptorContext context)
    //    {
    //        return true;
    //    }
    //    /// <summary>
    //    /// ... и только из списка
    //    /// </summary>
    //    public override bool GetStandardValuesExclusive(
    //      ITypeDescriptorContext context)
    //    {
    //        // false - можно вводить вручную
    //        // true - только выбор из списка
    //        return true;
    //    }

    //    /// <summary>
    //    /// А вот и список
    //    /// </summary>
    //    public override StandardValuesCollection GetStandardValues(
    //      ITypeDescriptorContext context)
    //    {
    //        // возвращаем список строк из настроек программы
    //        // (базы данных, интернет и т.д.)

    //        CPhone objPhone = (CPhone)context.Instance;
    //        System.Collections.Generic.List<CPhoneType> objList = objPhone.GetAllPhoneTypeList();

    //        return new StandardValuesCollection(objList);
    //    }
    //}
    /// <summary>
    /// Класс "Тип телефонного номера"
    /// </summary>
    public class CPhoneType : CBusinessObject
    {
        public CPhoneType()
            : base()
        {
        }
        public CPhoneType(System.Guid uuidId, System.String strName)
        {
            this.ID = uuidId;
            this.Name = strName;
        }

        #region Список объектов
        public static List<CPhoneType> GetPhoneTypeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CPhoneType> objList = new List<CPhoneType>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetPhoneType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CPhoneType((System.Guid)rs["PhoneType_Guid"],
                            (System.String)rs["PhoneType_Name"]));
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
                "Не удалось получить список типов телефонных адресов.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddPhoneType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PhoneType_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PhoneType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@PhoneType_Name"].Value = this.Name;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@PhoneType_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания типа телефонного номера.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать тип телефонного номера.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeletePhoneType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PhoneType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@PhoneType_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления типа телефонного номера.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить тип телефонного номера.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditPhoneType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PhoneType_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PhoneType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@PhoneType_Guid"].Value = this.ID;
                cmd.Parameters["@PhoneType_Name"].Value = this.Name;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств типа телефонного номера.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства типа телефонного номера.\n\nТекст ошибки: " + f.Message, "Внимание",
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

    #endregion

    #region Класс "Телефонный номер"
    /// <summary>
    /// Класс "Телефонный номер"
    /// </summary>
    public class CPhone
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
        /// Телефонный номер
        /// </summary>
        private System.String m_strPhoneNumber;
        /// <summary>
        /// Телефонный номер
        /// </summary>
        public System.String PhoneNumber
        {
            get { return m_strPhoneNumber; }
            set { m_strPhoneNumber = value; }
        }
        /// <summary>
        /// Признак "Активен"
        /// </summary>
        private System.Boolean m_bIsActive;
        /// <summary>
        /// Признак "Активен"
        /// </summary>
        public System.Boolean IsActive
        {
            get { return m_bIsActive; }
            set { m_bIsActive = value; }
        }
        /// <summary>
        /// Признак "Основной адрес"
        /// </summary>
        private System.Boolean m_bIsMain;
        /// <summary>
        /// Признак "Основной адрес"
        /// </summary>
        public System.Boolean IsMain
        {
            get { return m_bIsMain; }
            set { m_bIsMain = value; }
        }
        /// <summary>
        /// Тип номера
        /// </summary>
        private CPhoneType m_objPhoneType;
        /// <summary>
        /// Тип номера
        /// </summary>
        public CPhoneType PhoneType
        {
            get { return m_objPhoneType; }
            set { m_objPhoneType = value; }
        }
        #endregion

        #region Конструкторы
        public CPhone()
            : base()
        {
            m_uuidID = System.Guid.Empty;
            m_strPhoneNumber = "";
            m_objPhoneType = null;
            m_bIsActive = true;
            m_bIsMain = true;
        }
        public CPhone(System.Guid uuidId, System.String strPhoneNumber, CPhoneType objPhoneType, System.Boolean bIsActive, System.Boolean bIsMain)
        {
            m_uuidID = uuidId;
            m_strPhoneNumber = strPhoneNumber;
            m_objPhoneType = objPhoneType;
            m_bIsActive = bIsActive;
            m_bIsMain = bIsMain;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список телефонов для контакта
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="uuidContactId">уникальный идентификатор контакта</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>список телефонов</returns>
        public static List<CPhone> GetPhoneListForContact(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidContactId, ref System.String strErr)
        {
            List<CPhone> objList = new List<CPhone>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetPhoneForContact]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Contact_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Contact_Guid"].Value = uuidContactId;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CPhone((System.Guid)rs["Phone_Guid"], (System.String)rs["Phone_Number"],
                            new CPhoneType((System.Guid)rs["PhoneType_Guid"], (System.String)rs["PhoneType_Name"]), 
                            (System.Boolean)rs["ContactPhone_IsActive"], (System.Boolean)rs["ContactPhone_IsMain"]));
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
                strErr = "Не удалось получить список телефонных номеров.\n\nТекст ошибки: " + f.Message;
            }
            return objList;
        }
        /// <summary>
        /// Возвращает список телефонов для клиента
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="uuidCustomerId">уникальный идентификатор клиента</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>список телефонов</returns>
        public static List<CPhone> GetPhoneListForCustomer(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidCustomerId, ref System.String strErr)
        {
            List<CPhone> objList = new List<CPhone>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetPhoneForCustomer]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Customer_Guid"].Value = uuidCustomerId;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CPhone((System.Guid)rs["Phone_Guid"], (System.String)rs["Phone_Number"],
                            new CPhoneType((System.Guid)rs["PhoneType_Guid"], (System.String)rs["PhoneType_Name"]),
                            (System.Boolean)rs["ContactPhone_IsActive"], (System.Boolean)rs["ContactPhone_IsMain"]));
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
                strErr = "Не удалось получить список телефонных номеров.\n\nТекст ошибки: " + f.Message;
            }
            return objList;
        }

        /// <summary>
        /// Возвращает список телефонов для компании
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="uuidCompanyId">уникальный идентификатор компании</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>список телефонов</returns>
        public static List<CPhone> GetPhoneListForCompany(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidCompanyId, ref System.String strErr)
        { 
            List<CPhone> objList = new List<CPhone>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPhoneForCompany]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Company_Guid"].Value = uuidCompanyId;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CPhone((System.Guid)rs["Phone_Guid"], (System.String)rs["Phone_Number"],
                            new CPhoneType((System.Guid)rs["PhoneType_Guid"], (System.String)rs["PhoneType_Name"]),
                            (System.Boolean)rs["ContactPhone_IsActive"], (System.Boolean)rs["ContactPhone_IsMain"]));
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
                strErr = "Не удалось получить список телефонных номеров.\n\nТекст ошибки: " + f.Message;
            }
            return objList;
        }
                
                    
                

        #endregion

        #region Добавить в БД телефонный номер
        /// <summary>
        /// Проверка свойств электронного адреса перед сохранением
        /// </summary>
        /// <param name="strErr">текст с ошибкой</param>
        /// <returns>true - все свойства корректны; false - ошибка</returns>
        public System.Boolean IsAllParametersValid(ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (this.PhoneNumber == "")
                {
                    strErr = "Телефонный номер: Необходимо указать номер!";
                    return bRet;
                }
                if (this.m_objPhoneType == null)
                {
                    strErr = "Телефонный номер: Необходимо указать тип телефонного номера!";
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
        /// Добавляет телефонный номер в базу данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="enObjectWithAddress">тип владельца телефонного номера</param>
        /// <param name="uuidObjectId">идентификатор владельца телефонного номера</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Add(EnumObject enObjectWithAddress, System.Guid uuidObjectId,
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
                System.String strParamOwnerIdName = "";
                switch (enObjectWithAddress)
                {
                    case EnumObject.Contact:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[sp_AddPhoneToContact]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Contact_Guid";
                            break;
                        }
                    case EnumObject.Customer:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[sp_AddPhoneToCustomer]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Customer_Guid";
                            break;
                        }
                    case EnumObject.Company:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[usp_AddPhoneToCompany]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Company_Guid";
                            break;
                        }
                    case EnumObject.Vendor:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[usp_AddPhoneToVendor]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Vendor_Guid";
                            break;
                        }

                    default:
                        break;
                }
                //if (cmd.CommandText != strAddCmd)
                //{
                    cmd.Parameters.Clear();

                    cmd.CommandText = strAddCmd;
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Phone_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(strParamOwnerIdName, System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Phone_Number", System.Data.DbType.String));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PhoneType_Guid", System.Data.SqlDbType.UniqueIdentifier));

                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                //}
                cmd.Parameters[strParamOwnerIdName].Value = uuidObjectId;
                cmd.Parameters["@Phone_Number"].Value = this.PhoneNumber;
                cmd.Parameters["@PhoneType_Guid"].Value = this.PhoneType.ID;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    // 2009.08.11
                    // в том случае, если телефон сохраняется из его владельца, и вдруг нужно откатить транзакцию, то
                    // ID должен остаться пустым
                    //this.m_uuidID = (System.Guid)cmd.Parameters["@Phone_Guid"].Value;
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
        /// Сохраняет в БД список телефонных номеров
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="objAddressList">список телефонных номеров</param>
        /// <param name="enObjectWithAddress">тип владельца телефонных номеров</param>
        /// <param name="uuidObjectId">идентификатор владельца телефонных номеров</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean SavePhoneList(List<CPhone> objPhoneList, List<CPhone> objPhoneForDeleteList,
            EnumObject enObjectWithAddress, System.Guid uuidObjectId,
            UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            if (((objPhoneList == null) || (objPhoneList.Count == 0)) && ((objPhoneForDeleteList == null) || (objPhoneForDeleteList.Count == 0))) { return true; }
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                // для начала проверим, что нам пришло в списке
                if ((objPhoneList != null) && (objPhoneList.Count > 0))
                {
                    System.Boolean bIsAllValid = true;
                    foreach (CPhone objItem in objPhoneList)
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
                if ((objPhoneForDeleteList != null) && (objPhoneForDeleteList.Count > 0))
                {
                    foreach (CPhone objPhones in objPhoneForDeleteList)
                    {
                        if (objPhones.ID.CompareTo(System.Guid.Empty) == 0) { continue; }
                        iRes = (objPhones.Remove(enObjectWithAddress, uuidObjectId, objProfile, cmd, ref strErr) == true) ? 0 : 1;
                        if (iRes != 0) { break; }
                    }
                }

                if (iRes == 0)
                {
                    if ((objPhoneList != null) && (objPhoneList.Count > 0))
                    {
                        // теперь в цикле добавим в БД каждый член из списка
                        foreach (CPhone objPhone in objPhoneList)
                        {
                            if (objPhone.ID.CompareTo(System.Guid.Empty) == 0)
                            {
                                // новый телефонный номер
                                iRes = (objPhone.Add(enObjectWithAddress, uuidObjectId, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                            }
                            else
                            {
                                iRes = (objPhone.Update(enObjectWithAddress, uuidObjectId, objProfile, cmd, ref strErr) == true) ? 0 : -1;
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

        #region Изменение свойств телефонного номера в БД
        /// <summary>
        /// Изменение свойств телефонного номера в БД
        /// </summary>
        /// <param name="enObjectWithAddress">тип владельца телефонного номера</param>
        /// <param name="uuidObjectId">идентификатор владельца телефонного номера</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Update(EnumObject enObjectWithAddress, System.Guid uuidObjectId,
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
                System.String strParamOwnerIdName = "";
                switch (enObjectWithAddress)
                {
                    case EnumObject.Contact:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[sp_EditPhoneContact]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Contact_Guid";
                            break;
                        }
                    case EnumObject.Customer:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[sp_EditPhoneCustomer]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Customer_Guid";
                            break;
                        }
                    case EnumObject.Company:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[usp_EditPhoneCompany]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Company_Guid";
                            break;
                        }
                    case EnumObject.Vendor:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[usp_EditPhoneVendor]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Vendor_Guid";
                            break;
                        }
                    default:
                        break;
                }
                //if (cmd.CommandText != strAddCmd)
                //{
                    cmd.Parameters.Clear();

                    cmd.CommandText = strAddCmd;
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(strParamOwnerIdName, System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Phone_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Phone_Number", System.Data.DbType.String));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PhoneType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Phone_IsActive", System.Data.SqlDbType.Bit));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Phone_IsMain", System.Data.SqlDbType.Bit));

                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                //}
                cmd.Parameters[strParamOwnerIdName].Value = uuidObjectId;
                cmd.Parameters["@Phone_Guid"].Value = this.ID;
                cmd.Parameters["@Phone_Number"].Value = this.PhoneNumber;
                cmd.Parameters["@PhoneType_Guid"].Value = this.PhoneType.ID;
                cmd.Parameters["@Phone_IsActive"].Value = this.IsActive;
                cmd.Parameters["@Phone_IsMain"].Value = this.IsMain;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = "Ошибка редактирования телефонного номера. Текст ошибки: " + (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    if (this.ID.CompareTo((System.Guid)cmd.Parameters["@Phone_Guid"].Value) != 0)
                    {
                        this.ID = (System.Guid)cmd.Parameters["@Phone_Guid"].Value;
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
                strErr = "Ошибка редактирования телефонного номера. Текст ошибки: " + f.Message;
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

        #region Удалить телефонный номер из базы данных
        /// <summary>
        /// Удаляет телефонный номер из базы данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="enObjectWithAddress">тип владельца телефонного номера</param>
        /// <param name="uuidObjectId">идентификатор владельца телефонного номера</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Remove(EnumObject enObjectWithAddress, System.Guid uuidObjectId,
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
                System.String strParamOwnerIdName = "";
                switch (enObjectWithAddress)
                {
                    case EnumObject.Contact:
                        {
                            strDeleteCmd = System.String.Format("[{0}].[dbo].[sp_DeletePhoneFromContact]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Contact_Guid";
                            break;
                        }
                    case EnumObject.Customer:
                        {
                            strDeleteCmd = System.String.Format("[{0}].[dbo].[sp_DeletePhoneFromCustomer]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Customer_Guid";
                            break;
                        }
                    case EnumObject.Company:
                        {
                            strDeleteCmd = System.String.Format("[{0}].[dbo].[usp_DeletePhoneFromCompany]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Company_Guid";
                            break;   
                        }
                    case EnumObject.Vendor:
                        {
                            strDeleteCmd = System.String.Format("[{0}].[dbo].[usp_DeletePhoneFromVendor]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Vendor_Guid";
                            break;
                        }
                    default:
                        break;
                }
                cmd.CommandText = strDeleteCmd;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Phone_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(strParamOwnerIdName, System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters[strParamOwnerIdName].Value = uuidObjectId;
                cmd.Parameters["@Phone_Guid"].Value = this.ID;
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

        public override string ToString()
        {
            return m_strPhoneNumber;
        }
    }
    #endregion

    #region Класс "Электронный адрес"
    /// <summary>
    /// Класс "Электронный адрес"
    /// </summary>
    public class CEMail
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
        /// Электронный адрес
        /// </summary>
        private System.String m_strEMail;
        /// <summary>
        /// Электронный адрес
        /// </summary>
        public System.String EMail
        {
            get { return m_strEMail; }
            set { m_strEMail = value; }
        }
        /// <summary>
        /// Признак "Активен"
        /// </summary>
        private System.Boolean m_bIsActive;
        /// <summary>
        /// Признак "Активен"
        /// </summary>
        public System.Boolean IsActive
        {
            get { return m_bIsActive; }
            set { m_bIsActive = value; }
        }
        /// <summary>
        /// Признак "Основной адрес"
        /// </summary>
        private System.Boolean m_bIsMain;
        /// <summary>
        /// Признак "Основной адрес"
        /// </summary>
        public System.Boolean IsMain
        {
            get { return m_bIsMain; }
            set { m_bIsMain = value; }
        }
        #endregion

        #region Конструкторы 
        public CEMail()
        {
            m_uuidID = System.Guid.Empty;
            m_strEMail = "";
            m_bIsActive = true;
            m_bIsMain = true;
        }
        public CEMail(System.Guid uuidId, System.String strEMail, System.Boolean bIsActive, System.Boolean bIsMain)
        {
            m_uuidID = uuidId;
            m_strEMail = strEMail;
            m_bIsActive = bIsActive;
            m_bIsMain = bIsMain;
        }
        #endregion

        #region Список электронных адресов
        /// <summary>
        /// Возвращает список электронных адресов для контакта
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="uuidContactId">уникальный идентификатор контакта</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>список электронных адресов</returns>
        public static List<CEMail> GetEMailListForContact(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, EnumObject enObjectWithAddress, System.Guid uuidContactId, ref System.String strErr)
        {
            List<CEMail> objList = new List<CEMail>();
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

                System.String strAddCmd = "";
                System.String strParamOwnerIdName = "";
                switch (enObjectWithAddress)
                {
                    case EnumObject.Contact:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[sp_GetEMailForContact]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Contact_Guid";
                            break;
                        }
                    case EnumObject.Customer:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[sp_GetEMailForCustomer]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Customer_Guid";
                            break;
                        }
                    case EnumObject.Company:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[usp_GetEMailForCompany]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Company_Guid";
                            break;   
                        }
                    default:
                        break;
                }
                cmd.CommandText = strAddCmd;
                //cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetEMailForContact]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(strParamOwnerIdName, System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters[strParamOwnerIdName].Value = uuidContactId;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CEMail((System.Guid)rs["EMail_Guid"], (System.String)rs["EMail_Address"],
                            (System.Boolean)rs["EMail_IsActive"], (System.Boolean)rs["EMail_IsMain"]));
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
                strErr = "Не удалось получить список электронных адресов.\n\nТекст ошибки: " + f.Message;
            }
            return objList;
        }
        #endregion

        #region Добавить в БД электронный адрес
        /// <summary>
        /// Проверка свойств электронного адреса перед сохранением
        /// </summary>
        /// <param name="strErr">текст с ошибкой</param>
        /// <returns>true - все свойства корректны; false - ошибка</returns>
        public System.Boolean IsAllParametersValid(ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if ((this.EMail == "") || (this.EMail.Contains('@') == false) || (this.EMail.Contains('.') == false))
                {
                    strErr = "Email: Неверный электронный адрес!";
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
        /// Сохраняет в БД список электронных адресов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="objAddressList">список электронных адресов</param>
        /// <param name="enObjectWithAddress">тип владельца адресов</param>
        /// <param name="uuidObjectId">идентификатор владельца адресов</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean SaveEMailList(List<CEMail> objAddressList, List<CEMail> objAddressForDeleteList,
            EnumObject enObjectWithAddress, System.Guid uuidObjectId,
            UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            if (((objAddressList == null) || (objAddressList.Count == 0)) && ((objAddressForDeleteList == null) || (objAddressForDeleteList.Count == 0))) { return true; }
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                // для начала проверим, что нам пришло в списке
                if ((objAddressList != null) && (objAddressList.Count > 0))
                {
                    System.Boolean bIsAllValid = true;
                    foreach (CEMail objItem in objAddressList)
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
                if ((objAddressForDeleteList != null) && (objAddressForDeleteList.Count > 0))
                {
                    foreach (CEMail objAddres in objAddressForDeleteList)
                    {
                        if (objAddres.ID.CompareTo(System.Guid.Empty) == 0) { continue; }
                        iRes = (objAddres.Remove(enObjectWithAddress, uuidObjectId, objProfile, cmd, ref strErr) == true) ? 0 : 1;
                        if (iRes != 0) { break; }
                    }
                }
                if (iRes == 0)
                {
                    if ((objAddressList != null) && (objAddressList.Count > 0))
                    {
                        // теперь в цикле добавим в БД каждый член из списка
                        foreach (CEMail objAddress in objAddressList)
                        {
                            if (objAddress.ID.CompareTo(System.Guid.Empty) == 0)
                            {
                                iRes = (objAddress.Add(enObjectWithAddress, uuidObjectId, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                            }
                            else
                            {
                                iRes = (objAddress.Update(enObjectWithAddress, uuidObjectId, objProfile, cmd, ref strErr) == true) ? 0 : -1;
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
        /// <summary>
        /// Добавляет электронный адрес в базу данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="enObjectWithAddress">тип владельца адресов</param>
        /// <param name="uuidObjectId">идентификатор владельца адресов</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Add(EnumObject enObjectWithAddress, System.Guid uuidObjectId,
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
                System.String strParamOwnerIdName = "";
                switch (enObjectWithAddress)
                {
                    case EnumObject.Contact:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[sp_AddEmailToContact]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Contact_Guid";
                            break;
                        }
                    case EnumObject.Customer:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[sp_AddEmailToCustomer]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Customer_Guid";
                            break;
                        }
                    case EnumObject.Company:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[usp_AddEmailToCompany]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Company_Guid";
                            break;
                        }
                    case EnumObject.Vendor:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[usp_AddEmailToVendor]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Vendor_Guid";
                            break;
                        }

                    default:
                        break;
                }
                cmd.CommandText = strAddCmd;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EMail_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(strParamOwnerIdName, System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EMail_Address", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EMail_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EMail_IsMain", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters[strParamOwnerIdName].Value = uuidObjectId;
                cmd.Parameters["@EMail_Address"].Value = this.EMail;
                cmd.Parameters["@EMail_IsActive"].Value = this.IsActive;
                cmd.Parameters["@EMail_IsMain"].Value = this.IsMain;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    // 2009.08.11
                    // в том случае, если адрес сохраняется из его владельца, и вдруг нужно откатить транзакцию, то
                    // ID должен остаться пустым
                    //this.m_uuidID = (System.Guid)cmd.Parameters["@EMail_Guid"].Value;
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

        #region Изменение свойств электронного адреса в БД
        /// <summary>
        /// Изменение свойств электронного адреса в БД
        /// </summary>
        /// <param name="enObjectWithAddress">тип владельца адресов</param>
        /// <param name="uuidObjectId">идентификатор владельца адресов</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Update(EnumObject enObjectWithAddress, System.Guid uuidObjectId,
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
                System.String strParamOwnerIdName = "";
                switch (enObjectWithAddress)
                {
                    case EnumObject.Contact:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[sp_EditEmailContact]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Contact_Guid";
                            break;
                        }
                    case EnumObject.Customer:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[sp_EditEmailCustomer]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Customer_Guid";
                            break;
                        }
                    case EnumObject.Company:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[usp_EditEmailCompany]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Company_Guid";
                            break;
                        }
                    case EnumObject.Vendor:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[usp_EditEmailVendor]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Vendor_Guid";
                            break;
                        }
                    default:
                        break;
                }
                //if (cmd.CommandText != strAddCmd)
                //{
                //    cmd.Parameters.Clear();

                    cmd.CommandText = strAddCmd;
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(strParamOwnerIdName, System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EMail_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EMail_Address", System.Data.DbType.String));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EMail_IsActive", System.Data.SqlDbType.Bit));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EMail_IsMain", System.Data.SqlDbType.Bit));

                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                //}
                cmd.Parameters[strParamOwnerIdName].Value = uuidObjectId;
                cmd.Parameters["@EMail_Guid"].Value = this.ID;
                cmd.Parameters["@EMail_Address"].Value = this.EMail;
                cmd.Parameters["@EMail_IsActive"].Value = this.IsActive;
                cmd.Parameters["@EMail_IsMain"].Value = this.IsMain;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = "Ошибка редактирования электронного адреса. Текст ошибки: " + (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    if (this.ID.CompareTo((System.Guid)cmd.Parameters["@EMail_Guid"].Value) != 0)
                    {
                        this.ID = (System.Guid)cmd.Parameters["@EMail_Guid"].Value;
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
                strErr = "Ошибка редактирования электронного адреса. Текст ошибки: " + f.Message;
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

        #region Удалить электронный адрес из базы данных
        /// <summary>
        /// Удаляет электронный адрес из базы данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="enObjectWithAddress">тип владельца адресов</param>
        /// <param name="uuidObjectId">идентификатор владельца адресов</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Remove(EnumObject enObjectWithAddress, System.Guid uuidObjectId,
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

                // сперва удалим список адресов
                cmd.Parameters.Clear();
                System.String strDeleteCmd = "";
                System.String strParamOwnerIdName = "";
                switch (enObjectWithAddress)
                {
                    case EnumObject.Contact:
                        {
                            strDeleteCmd = System.String.Format("[{0}].[dbo].[sp_DeleteEMailFromContact]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Contact_Guid";
                            break;
                        }
                    case EnumObject.Customer:
                        {
                            strDeleteCmd = System.String.Format("[{0}].[dbo].[sp_DeleteEMailFromCustomer]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Customer_Guid";
                            break;
                        }
                    case EnumObject.Company:
                        {
                            strDeleteCmd = System.String.Format("[{0}].[dbo].[usp_DeleteEMailFromCompany]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Company_Guid";
                            break;
                        }
                    case EnumObject.Vendor:
                        {
                            strDeleteCmd = System.String.Format("[{0}].[dbo].[usp_DeleteEMailFromVendor]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Vendor_Guid";
                            break;
                        }

                    default:
                        break;
                }
                cmd.CommandText = strDeleteCmd;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EMail_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(strParamOwnerIdName, System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters[strParamOwnerIdName].Value = uuidObjectId;
                cmd.Parameters["@EMail_Guid"].Value = this.ID;
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

        public override string ToString()
        {
            return m_strEMail;
        }
    }
    #endregion

    /// <summary>
    /// Класс "Контакт"
    /// </summary>
    public class CContact
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
        /// Погоняло (кликуха)
        /// </summary>
        private System.String m_strNickName;
        /// <summary>
        /// Погоняло (кликуха)
        /// </summary>
        public System.String NickName
        {
            get { return m_strNickName; }
            set { m_strNickName = value; }
        }
        /// <summary>
        /// Девичья фамилия
        /// </summary>
        private System.String m_strSpouseName;
        /// <summary>
        /// Девичья фамилия
        /// </summary>
        public System.String SpouseName
        {
            get { return m_strSpouseName; }
            set { m_strSpouseName = value; }
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
        /// интернет-страница
        /// </summary>
        private System.String m_strWWW;
        /// <summary>
        /// интернет-страница
        /// </summary>
        public System.String WWW
        {
            get { return m_strWWW; }
            set { m_strWWW = value; }
        }
        /// <summary>
        /// Отдел, в котором числится контакт
        /// </summary>
        private CDepartament m_objDepartment;
        /// <summary>
        /// Отдел, в котором числится контакт
        /// </summary>
        public CDepartament Department
        {
            get { return m_objDepartment; }
            set { m_objDepartment = value; }
        }
        /// <summary>
        /// Должность
        /// </summary>
        private CJobPosition m_objJobPosition;
        /// <summary>
        /// Должность
        /// </summary>
        public CJobPosition JobPosition
        {
            get { return m_objJobPosition; }
            set { m_objJobPosition = value; }
        }
        /// <summary>
        /// Список электронных адресов
        /// </summary>
        private List<CEMail> m_objEMailList;
        /// <summary>
        /// Список электронных адресов
        /// </summary>
        public List<CEMail> EMailList
        {
            get { return m_objEMailList; }
            set { m_objEMailList = value; }
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
        /// Список телефонов
        /// </summary>
        private List<CPhone> m_objPhoneList;
        /// <summary>
        /// Список телефонов
        /// </summary>
        public List<CPhone> PhoneList
        {
            get { return m_objPhoneList; }
            set { m_objPhoneList = value; }
        }
        public System.String PhoneString
        {
            get
            {
                System.String strPhone = "";
                if (m_objPhoneList != null)
                {
                    for (System.Int32 i = 0; i < m_objPhoneList.Count; i++)
                    {
                        strPhone += (m_objPhoneList[i]);
                        if (i < (m_objPhoneList.Count - 1))
                        {
                            strPhone += ";";
                        }
                    }
                }
                if (strPhone != "") { strPhone = " тел.: " + strPhone; }
                return strPhone;
            }
        }
        /// <summary>
        /// Признак "Активен"
        /// </summary>
        private System.Boolean m_bIsActive;
        /// <summary>
        /// Признак "Активен"
        /// </summary>
        public System.Boolean IsActive
        {
            get { return m_bIsActive; }
            set { m_bIsActive = value; }
        }
        private List<CAddress> m_objAddressForDeleteList;
        public List<CAddress> AddressForDeleteList
        {
            get { return m_objAddressForDeleteList; }
            set { m_objAddressForDeleteList = value; }
        }
        private List<CPhone> m_objPhoneForDeleteList;
        public List<CPhone> PhoneForDeleteList
        {
            get { return m_objPhoneForDeleteList; }
            set { m_objPhoneForDeleteList = value; }
        }
        private List<CEMail> m_objEMailForDeleteList;
        public List<CEMail> EMailForDeleteList
        {
            get { return m_objEMailForDeleteList; }
            set { m_objEMailForDeleteList = value; }
        }
        /// <summary>
        /// Визитная карточка
        /// </summary>
        public System.String VisitingCard
        {
            get
            {
                return (m_strLastName + " " + m_strFirstName +
                    (m_objDepartment == null ? "" : ("\n\n" + m_objDepartment.Name)) +
                    (m_objJobPosition == null ? "" : ("\n" + m_objJobPosition.Name)));
            }
        }
        /// <summary>
        /// Визитная карточка
        /// </summary>
        public System.String VisitingCard2
        {
            get
            {
                return (m_strLastName + " " + m_strFirstName +
                    (m_objDepartment == null ? "" : (", " + m_objDepartment.Name)) +
                    (m_objJobPosition == null ? "" : (": " + m_objJobPosition.Name)) +
                    (PhoneString == "" ? "" : (", " + PhoneString))
                    );
            }
        }
        /// <summary>
        /// Дата рождения
        /// </summary>
        private System.DateTime m_Birthday;
        /// <summary>
        /// Дата рождения
        /// </summary>
        public System.DateTime Birthday
        {
            get { return m_Birthday; }
            set { m_Birthday = value; }
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
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "ClearIDFromChildObject.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Конструктор
        public CContact()
        {
            m_uuidID = System.Guid.Empty;
            m_strFirstName = "";
            m_strMiddleName = "";
            m_strLastName = "";
            m_strNickName = "";
            m_strSpouseName = "";
            m_strDescription = "";
            m_strWWW = "";
            m_objDepartment = null;
            m_objJobPosition = null;
            m_objAddressList = null;
            m_objPhoneList = null;
            m_objEMailList = null;
            m_bIsActive = true;
            m_objAddressForDeleteList = null;
            m_objPhoneForDeleteList = null;
            m_objEMailForDeleteList = null;
            m_Birthday = System.DateTime.MinValue;
            m_bNewObject = false;
        }

        public CContact(System.Guid uuidID, System.String strFirstName, System.String strMiddleName,
            System.String strLastName, System.String strNickName, System.String strSpouseName, System.String strWWW,
            System.String strDescription, CDepartament objDepartment, CJobPosition objJobPosition,
            System.Boolean bIsActive, System.DateTime ContactBirthday)
        {
            m_uuidID = uuidID;
            m_strFirstName = strFirstName;
            m_strMiddleName = strMiddleName;
            m_strLastName = strLastName;
            m_strNickName = strNickName;
            m_strSpouseName = strSpouseName;
            m_strDescription = strDescription;
            m_strWWW = strWWW;
            m_objDepartment = objDepartment;
            m_objJobPosition = objJobPosition;
            m_objAddressList = null;
            m_objPhoneList = null;
            m_objEMailList = null;
            m_bIsActive = bIsActive;
            m_objAddressForDeleteList = null;
            m_objPhoneForDeleteList = null;
            m_objEMailForDeleteList = null;
            m_Birthday = ContactBirthday;
            m_bNewObject = false;
        }
        #endregion

        #region Список контактов
        /// <summary>
        /// Возвращает список контактов (без телефонов, email, адресов)
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="enObjectWithContact">ссылка на тип владельца контактов</param>
        /// <param name="uuidObjectId">уникальный идентификатор владельца контактов</param>
        /// <returns>список контактов</returns>
        public static List<CContact> GetContactList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL,
            EnumObject enObjectWithContact, System.Guid uuidObjectId)
        {
            List<CContact> objList = new List<CContact>();
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
                switch (enObjectWithContact)
                {
                    case EnumObject.Bank:
                        {
                            cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetContactForBank]", objProfile.GetOptionsDllDBName());
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Bank_Guid", System.Data.SqlDbType.UniqueIdentifier));
                            cmd.Parameters["@Bank_Guid"].Value = uuidObjectId;
                            break;
                        }
                    case EnumObject.Customer:
                        {
                            cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetContactForCustomer]", objProfile.GetOptionsDllDBName());
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                            cmd.Parameters["@Customer_Guid"].Value = uuidObjectId;
                            break;
                        }
                    case EnumObject.Rtt:
                        {
                            cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetContactForRtt]", objProfile.GetOptionsDllDBName());
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Rtt_Guid", System.Data.SqlDbType.UniqueIdentifier));
                            cmd.Parameters["@Rtt_Guid"].Value = uuidObjectId;
                            break;
                        }
                    case EnumObject.Company:
                        {
                            cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetContactForCompany]", objProfile.GetOptionsDllDBName());
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                            cmd.Parameters["@Company_Guid"].Value = uuidObjectId;
                            break;
                        }
                    case EnumObject.Vendor:
                        {
                            cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetContactForVendor]", objProfile.GetOptionsDllDBName());
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Guid", System.Data.SqlDbType.UniqueIdentifier));
                            cmd.Parameters["@Vendor_Guid"].Value = uuidObjectId;
                            break;
                        }
                    default:
                        break;
                }
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CDepartament objDepartament = null;
                    CJobPosition objJobPosition = null;
                    System.String Contact_NickName = "";
                    System.String Contact_SpouseName = "";
                    System.String Contact_Description = "";
                    System.String Department_Description = "";
                    System.String JobPosition_Description = "";
                    System.String Contact_WWW = "";
                    System.DateTime Contact_BirthDay = System.DateTime.MinValue;

                    while (rs.Read())
                    {
                        Contact_NickName = (rs["Contact_NickName"] == System.DBNull.Value) ? "" : (System.String)rs["Contact_NickName"];
                        Contact_SpouseName = (rs["Contact_SpouseName"] == System.DBNull.Value) ? "" : (System.String)rs["Contact_SpouseName"];
                        Contact_Description = (rs["Contact_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Contact_Description"];
                        Department_Description = (rs["Department_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Department_Description"];
                        JobPosition_Description = (rs["JobPosition_Description"] == System.DBNull.Value) ? "" : (System.String)rs["JobPosition_Description"];
                        Contact_WWW = (rs["Contact_WWW"] == System.DBNull.Value) ? "" : (System.String)rs["Contact_WWW"];
                        Contact_BirthDay = (rs["Contact_Birthday"] == System.DBNull.Value) ? System.DateTime.MinValue : (System.DateTime)rs["Contact_Birthday"];

                        objDepartament = new CDepartament((System.Guid)rs["Department_Guid"], (System.String)rs["Department_Name"], Department_Description);
                        objJobPosition = new CJobPosition((System.Guid)rs["JobPosition_Guid"], (System.String)rs["JobPosition_Name"], JobPosition_Description);

                        objList.Add(new CContact((System.Guid)rs["Contact_Guid"], (System.String)rs["Contact_FirstName"],
                            (System.String)rs["Contact_MiddleName"], (System.String)rs["Contact_LastName"], Contact_NickName,
                            Contact_SpouseName, Contact_WWW, Contact_Description, objDepartament, objJobPosition, 
                            (System.Boolean)rs["Contact_IsActive"], Contact_BirthDay));

                    }
                }
                rs.Close();
                if ((objList != null) && (objList.Count > 0))
                {
                    System.String strErr = "";
                    foreach (CContact objContact in objList)
                    {
                        objContact.LoadContactProperties(objProfile, cmd, ref strErr);
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
                "Не удалось получить список контактов.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Загружает в контакт списки адресов, телефонов, почтовых адресов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public System.Boolean LoadContactProperties(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr )
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

                // сперва запросим список телефонов
                // затем список email, а потом список адресов
                this.m_objPhoneList = CPhone.GetPhoneListForContact(objProfile, cmd, this.m_uuidID, ref strErr);
                this.m_objEMailList = CEMail.GetEMailListForContact(objProfile, cmd, EnumObject.Contact, this.m_uuidID, ref strErr);
                this.m_objAddressList = CAddress.GetAddressList(objProfile, cmd, EnumObject.Contact, this.m_uuidID);

                bRet = ((this.m_objPhoneList != null) && (this.m_objEMailList != null) && (this.m_objAddressList != null));

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                strErr = "Не удалось загрузить свойства контакта.\n\nТекст ошибки: " + f.Message;
            }
            return bRet;
        }
        #endregion

        #region Сохранить в БД список контактов
        /// <summary>
        /// Сохраняет в БД контакты
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="objAddressList">список адресов</param>
        /// <param name="enObjectWithAddress">тип владельца адресов</param>
        /// <param name="uuidObjectId">идентификатор владельца адресов</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean SaveContactList(List<CContact> objContactList, List<CContact> objContactForDeleteList,
            EnumObject enObjectWithAddress, System.Guid uuidObjectId,
            UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            if (((objContactList == null) || (objContactList.Count == 0)) && ((objContactForDeleteList == null) || (objContactForDeleteList.Count == 0))) { return true; }
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                // для начала проверим, что нам пришло в списке
                if ((objContactList != null) && (objContactList.Count > 0))
                {
                    System.Boolean bIsAllValid = true;
                    foreach (CContact objItem in objContactList)
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
                if ((objContactForDeleteList != null) && (objContactForDeleteList.Count > 0))
                {
                    foreach (CContact objContact in objContactForDeleteList)
                    {
                        if (objContact.ID.CompareTo(System.Guid.Empty) == 0) { continue; }
                        iRes = (objContact.Remove(enObjectWithAddress, uuidObjectId, objProfile, cmd, ref strErr) == true) ? 0 : 1;
                        if (iRes != 0) { break; }
                    }
                }

                if (iRes == 0)
                {
                    // удалили, теперь добавим или сохраним
                    if ((objContactList != null) && (objContactList.Count > 0))
                    {
                        // теперь в цикле добавим в БД каждый член из списка
                        foreach (CContact objContact in objContactList)
                        {
                            if (objContact.ID.CompareTo(System.Guid.Empty) == 0)
                            {
                                // новый контакт
                                iRes = (objContact.Add(enObjectWithAddress, uuidObjectId, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                            }
                            else
                            {
                                iRes = (objContact.Update(enObjectWithAddress, uuidObjectId, objProfile, cmd, ref strErr ) == true) ? 0 : -1;
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

        #region Добавить контакт в базу данных
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
                if (this.Department == null)
                {
                    strErr = "Контакт " + this.LastName + ". Необходимо указать отдел!";
                    return bRet;
                }
                if (this.JobPosition == null)
                {
                    strErr = "Контакт " + this.LastName + ". Необходимо указать должность!";
                    return bRet;
                }
                if (this.LastName == "")
                {
                    strErr = "Контакт " + this.LastName + ". Необходимо указать Фамилию!";
                    return bRet;
                }
                if (this.MiddleName == "")
                {
                    strErr = "Контакт " + this.LastName + ". Необходимо указать Отчество!";
                    return bRet;
                }
                if (this.FirstName == "")
                {
                    strErr = "Контакт " + this.LastName + ". Необходимо указать Имя!";
                    return bRet;
                }
                if ((this.EMailList != null) && (this.EMailList.Count > 0))
                {
                    System.Boolean bIsAllEMailValid = true;
                    foreach (CEMail objItem in this.EMailList)
                    {
                        if (objItem.IsAllParametersValid(ref strErr) == false)
                        {
                            bIsAllEMailValid = false;
                            break;
                        }
                    }
                    if (bIsAllEMailValid == false)
                    {
                        return bRet;
                    }
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
                if ((this.PhoneList != null) && (this.PhoneList.Count > 0))
                {
                    System.Boolean bIsAllPhoneValid = true;
                    foreach (CPhone objItem in this.PhoneList)
                    {
                        if (objItem.IsAllParametersValid(ref strErr) == false)
                        {
                            bIsAllPhoneValid = false;
                            break;
                        }
                    }
                    if (bIsAllPhoneValid == false)
                    {
                        return bRet;
                    }
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
        /// Добавляет в базу данных новый контакт
        /// </summary>
        /// <param name="enObjectWithAddress">тип владельца контакта</param>
        /// <param name="uuidObjectId">уникальный идентификатор владельца контакта</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Add(EnumObject enObjectWithAddress, System.Guid uuidObjectId,
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

                cmd.Parameters.Clear();
                System.String strAddCmd = "";
                System.String strParamOwnerIdName = "";
                switch (enObjectWithAddress)
                {
                    case EnumObject.Bank:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[sp_AddContactToBank]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Bank_Guid";
                            break;
                        }
                    case EnumObject.Customer:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[sp_AddContactToCustomer]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Customer_Guid";
                            break;
                        }
                    case EnumObject.Rtt:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[sp_AddContactToRtt]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Rtt_Guid";
                            break;
                        }
                    case EnumObject.Company:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[usp_AddContactToCompany]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Company_Guid";
                            break;
                        }
                    case EnumObject.Vendor:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[usp_AddContactToVendor]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Vendor_Guid";
                            break;
                        }
                    case EnumObject.AgreementWithCustomer:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[usp_AddContactToAgreementWithCustomer]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@AgreementWithCustomer_Guid";
                            break;
                        }
                    default:
                        break;
                }
                cmd.CommandText = strAddCmd;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(strParamOwnerIdName, System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Contact_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Department_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@JobPosition_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Contact_FirstName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Contact_MiddleName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Contact_LastName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Contact_IsActive", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Contact_NickName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Contact_SpouseName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Contact_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Contact_WWW", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Contact_Birthday", System.Data.DbType.Date));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters[strParamOwnerIdName].Value = uuidObjectId;

                cmd.Parameters["@Department_Guid"].Value = this.Department.ID;
                cmd.Parameters["@JobPosition_Guid"].Value = this.JobPosition.ID;
                cmd.Parameters["@Contact_FirstName"].Value = this.FirstName;
                cmd.Parameters["@Contact_MiddleName"].Value = this.MiddleName;
                cmd.Parameters["@Contact_LastName"].Value = this.LastName;
                cmd.Parameters["@Contact_IsActive"].Value = this.IsActive;

                if (this.NickName != "")
                {
                    cmd.Parameters["@Contact_NickName"].IsNullable = false;
                    cmd.Parameters["@Contact_NickName"].Value = this.NickName;
                }
                else
                {
                    cmd.Parameters["@Contact_NickName"].IsNullable = true;
                    cmd.Parameters["@Contact_NickName"].Value = null;
                }
                if (this.SpouseName != "")
                {
                    cmd.Parameters["@Contact_SpouseName"].IsNullable = false;
                    cmd.Parameters["@Contact_SpouseName"].Value = this.SpouseName;
                }
                else
                {
                    cmd.Parameters["@Contact_SpouseName"].IsNullable = true;
                    cmd.Parameters["@Contact_SpouseName"].Value = null;
                }
                if (this.Description != "")
                {
                    cmd.Parameters["@Contact_Description"].IsNullable = false;
                    cmd.Parameters["@Contact_Description"].Value = this.Description;
                }
                else
                {
                    cmd.Parameters["@Contact_Description"].IsNullable = true;
                    cmd.Parameters["@Contact_Description"].Value = null;
                }
                if (this.WWW != "")
                {
                    cmd.Parameters["@Contact_WWW"].IsNullable = false;
                    cmd.Parameters["@Contact_WWW"].Value = this.WWW;
                }
                else
                {
                    cmd.Parameters["@Contact_WWW"].IsNullable = true;
                    cmd.Parameters["@Contact_WWW"].Value = null;
                }
                if (this.Birthday != System.DateTime.MinValue)
                {
                    cmd.Parameters["@Contact_Birthday"].IsNullable = false;
                    cmd.Parameters["@Contact_Birthday"].Value = this.Birthday;
                }
                else
                {
                    cmd.Parameters["@Contact_Birthday"].IsNullable = true;
                    cmd.Parameters["@Contact_Birthday"].Value = null;
                }


                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    this.m_uuidID = (System.Guid)cmd.Parameters["@Contact_Guid"].Value;
                    // теперь списки телефонов, электронных адресов и адресов
                    if ((this.PhoneList != null) && (this.PhoneList.Count > 0))
                    {
                        iRes = (CPhone.SavePhoneList(this.PhoneList, null, EnumObject.Contact, this.m_uuidID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                    }
                    if (iRes == 0)
                    {
                        if ((this.EMailList != null) && (this.EMailList.Count > 0))
                        {
                            iRes = (CEMail.SaveEMailList(this.EMailList, null, EnumObject.Contact, this.m_uuidID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.AddressList != null) && (this.AddressList.Count > 0))
                        {
                            iRes = (CAddress.SaveAddressList(this.AddressList, null, EnumObject.Contact, this.m_uuidID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
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
                if( (cmdSQL == null) && (DBTransaction != null) )
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

        #region Изменить свойства контакта в базе данных
        public System.Boolean Update(EnumObject enObjectWithAddress, System.Guid uuidObjectId,
            UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr )
            //List<CAddress> objAddressForDeleteList, List<CPhone> objPhoneForDeleteList, List<CEMail> objEMailForDeleteList)
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditContact]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Contact_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Department_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@JobPosition_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Contact_FirstName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Contact_MiddleName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Contact_LastName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Contact_IsActive", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Contact_NickName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Contact_SpouseName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Contact_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Contact_WWW", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Contact_Birthday", System.Data.DbType.Date));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Contact_Guid"].Value = this.ID;
                cmd.Parameters["@Department_Guid"].Value = this.Department.ID;
                cmd.Parameters["@JobPosition_Guid"].Value = this.JobPosition.ID;
                cmd.Parameters["@Contact_FirstName"].Value = this.FirstName;
                cmd.Parameters["@Contact_MiddleName"].Value = this.MiddleName;
                cmd.Parameters["@Contact_LastName"].Value = this.LastName;
                cmd.Parameters["@Contact_IsActive"].Value = this.IsActive;

                if (this.NickName != "")
                {
                    cmd.Parameters["@Contact_NickName"].IsNullable = false;
                    cmd.Parameters["@Contact_NickName"].Value = this.NickName;
                }
                else
                {
                    cmd.Parameters["@Contact_NickName"].IsNullable = true;
                    cmd.Parameters["@Contact_NickName"].Value = null;
                }
                if (this.SpouseName != "")
                {
                    cmd.Parameters["@Contact_SpouseName"].IsNullable = false;
                    cmd.Parameters["@Contact_SpouseName"].Value = this.SpouseName;
                }
                else
                {
                    cmd.Parameters["@Contact_SpouseName"].IsNullable = true;
                    cmd.Parameters["@Contact_SpouseName"].Value = null;
                }
                if (this.Description != "")
                {
                    cmd.Parameters["@Contact_Description"].IsNullable = false;
                    cmd.Parameters["@Contact_Description"].Value = this.Description;
                }
                else
                {
                    cmd.Parameters["@Contact_Description"].IsNullable = true;
                    cmd.Parameters["@Contact_Description"].Value = null;
                }
                if (this.WWW != "")
                {
                    cmd.Parameters["@Contact_WWW"].IsNullable = false;
                    cmd.Parameters["@Contact_WWW"].Value = this.WWW;
                }
                else
                {
                    cmd.Parameters["@Contact_WWW"].IsNullable = true;
                    cmd.Parameters["@Contact_WWW"].Value = null;
                }
                if (this.Birthday != System.DateTime.MinValue)
                {
                    cmd.Parameters["@Contact_Birthday"].IsNullable = false;
                    cmd.Parameters["@Contact_Birthday"].Value = this.Birthday;
                }
                else
                {
                    cmd.Parameters["@Contact_Birthday"].IsNullable = true;
                    cmd.Parameters["@Contact_Birthday"].Value = null;
                }

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    // теперь списки телефонов, электронных адресов и адресов
                    if ((this.PhoneList != null) && (this.PhoneList.Count > 0))
                    {
                        iRes = (CPhone.SavePhoneList(this.PhoneList, this.PhoneForDeleteList, EnumObject.Contact, this.m_uuidID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                    }
                    if (iRes == 0)
                    {
                        if ((this.EMailList != null) && (this.EMailList.Count > 0))
                        {
                            iRes = (CEMail.SaveEMailList(this.EMailList, this.EMailForDeleteList, EnumObject.Contact, this.m_uuidID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.AddressList != null) && (this.AddressList.Count > 0))
                        {
                            iRes = (CAddress.SaveAddressList(this.AddressList, this.AddressForDeleteList, EnumObject.Contact, this.m_uuidID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
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

        #region Удалить контакт из базы данных
        /// <summary>
        /// Удаляет контакт из базы данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="enObjectWithAddress">тип владельца контакта</param>
        /// <param name="uuidObjectId">идентификатор владельца адресов</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Remove(EnumObject enObjectWithAddress, System.Guid uuidObjectId,
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

                // сперва удалим список адресов
                cmd.Parameters.Clear();
                System.String strDeleteCmd = "";
                switch (enObjectWithAddress)
                {
                    case EnumObject.Bank:
                        {
                            strDeleteCmd = System.String.Format("[{0}].[dbo].[sp_DeleteContactFromBank]", objProfile.GetOptionsDllDBName());
                            break;
                        }
                    case EnumObject.Customer:
                        {
                            strDeleteCmd = System.String.Format("[{0}].[dbo].[sp_DeleteContactFromCustomer]", objProfile.GetOptionsDllDBName());
                            break;
                        }
                    case EnumObject.Rtt:
                        {
                            strDeleteCmd = System.String.Format("[{0}].[dbo].[sp_DeleteContactFromRtt]", objProfile.GetOptionsDllDBName());
                            break;
                        }
                    case EnumObject.Company:
                        {
                            strDeleteCmd = System.String.Format("[{0}].[dbo].[usp_DeleteContactFromCompany]", objProfile.GetOptionsDllDBName());
                            break;
                        }
                    case EnumObject.Vendor:
                        {
                            strDeleteCmd = System.String.Format("[{0}].[dbo].[usp_DeleteContactFromVendor]", objProfile.GetOptionsDllDBName());
                            break;
                        }
                        
                    default:
                        break;
                }
                cmd.CommandText = strDeleteCmd;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Contact_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Contact_Guid"].Value = this.ID;
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
