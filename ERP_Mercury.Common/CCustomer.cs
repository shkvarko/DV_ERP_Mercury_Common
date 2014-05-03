using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;


namespace ERP_Mercury.Common
{
    #region Класс "Форма собственности"
    /// <summary>
    /// Класс "Форма собственности"
    /// </summary>
    public class CStateType : CBusinessObject
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
        /// Сокращенное наименование
        /// </summary>
        private System.String m_strShortName;
        /// <summary>
        /// Сокращенное наименование
        /// </summary>
        [DisplayName("Аббревиатура")]
        [Description("Сокращенное наименование")]
        [Category("2. Дополнительно")]
        public System.String ShortName
        {
            get { return m_strShortName; }
            set { m_strShortName = value; }
        }
        #endregion

        public CStateType()
            : base()
        {
        }
        public CStateType(System.Guid uuidId, System.String strName, System.String strShortName,
            System.Boolean bIsActive)
        {
            ID = uuidId;
            Name = strName;
            m_IsActive = bIsActive;
            m_strShortName = strShortName;
        }

        #region Список объектов
        public static List<CStateType> GetStateTypeList( UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CStateType> objList = new List<CStateType>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetCustomerStateType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add( new CStateType( (System.Guid)rs["CustomerStateType_Guid"],
                            (System.String)rs["CustomerStateType_Name"],
                            (System.String)rs["CustomerStateType_ShortName"],
                            (System.Boolean)rs["CustomerStateType_IsActive"]));
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
                "Не удалось получить список форм собственности.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }

        /// <summary>
        /// Получить форму собственности
        /// </summary>
        /// <param name="objProfile"> профайл </param>
        /// <param name="cmdSQL"> cmdSQL </param>
        /// <param name="uuidCompny"> guid компании </param>
        /// <returns>Строка с формой собственности</returns>
        public static System.String GetStateTypeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidCompny)
        {
            System.String strCompanyState="";
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
                        return strCompanyState;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCompanyStateType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Company_Guid"].Value = uuidCompny;

                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        strCompanyState =(System.String) rs["CustomerStateType_Name"];
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
                "Не удалось получить список форм собственности.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return strCompanyState;
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
                if (this.ShortName == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать сокращенное наименование!", "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddCustomerStateType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerStateType_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerStateType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerStateType_ShortName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@CustomerStateType_Name"].Value = this.Name;
                cmd.Parameters["@CustomerStateType_ShortName"].Value = this.ShortName;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@CustomerStateType_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания формы собственности.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать форму собственности.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteCustomerStateType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerStateType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@CustomerStateType_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления формы собственности.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить форму собственности.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditCustomerStateType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerStateType_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerStateType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerStateType_ShortName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerStateType_IsActive", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@CustomerStateType_Guid"].Value = this.ID;
                cmd.Parameters["@CustomerStateType_Name"].Value = this.Name;
                cmd.Parameters["@CustomerStateType_ShortName"].Value = this.ShortName;
                cmd.Parameters["@CustomerStateType_IsActive"].Value = this.IsActive;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения формы собственности.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства формы собственности.\n\nТекст ошибки: " + f.Message, "Внимание",
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
            return (this.Name + "\t" + "[" + this.ShortName + "]");
;
        }
    }
    #endregion

    #region Класс "Тип клиента"
    /// <summary>
    /// Класс "Тип клиента"
    /// </summary>
    public class CCustomerType : CBusinessObject
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
        /// Сокращенное наименование
        /// </summary>
        private System.String m_strShortName;
        /// <summary>
        /// Сокращенное наименование
        /// </summary>
        [DisplayName("Аббревиатура")]
        [Description("Сокращенное наименование")]
        [Category("2. Дополнительно")]
        public System.String ShortName
        {
            get { return m_strShortName; }
            set { m_strShortName = value; }
        }
        #endregion

        public CCustomerType()
            : base()
        {
        }
        public CCustomerType(System.Guid uuidId, System.String strName, System.String strShortName,
            System.Boolean bIsActive)
        {
            ID = uuidId;
            Name = strName;
            m_IsActive = bIsActive;
            m_strShortName = strShortName;
        }

        #region Список объектов
        public static List<CCustomerType> GetCustomerTypeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CCustomerType> objList = new List<CCustomerType>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetCustomerType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CCustomerType((System.Guid)rs["CustomerType_Guid"],
                            (System.String)rs["CustomerType_Name"],
                            (System.String)rs["CustomerType_ShortName"],
                            (System.Boolean)rs["CustomerType_IsActive"]));
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
                "Не удалось получить список типов клиентов.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Возвращает список типов клиентов, назначенных клиенту с указанным идентификатором
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="uuidCustomerId">уникальный идентификатор клиента</param>
        /// <returns>список типов клиентов</returns>
        public static List<CCustomerType> GetCustomerTypeListForCustomer(UniXP.Common.CProfile objProfile, 
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidCustomerId)
        {
            List<CCustomerType> objList = new List<CCustomerType>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetCustomerCustomerType]", objProfile.GetOptionsDllDBName());
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
                        objList.Add(new CCustomerType((System.Guid)rs["CustomerType_Guid"],
                            (System.String)rs["CustomerType_Name"],
                            (System.String)rs["CustomerType_ShortName"],
                            (System.Boolean)rs["CustomerType_IsActive"]));
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
                "Не удалось получить список типов клиентов.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                if (this.ShortName == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать сокращенное наименование!", "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddCustomerType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerType_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerType_ShortName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@CustomerType_Name"].Value = this.Name;
                cmd.Parameters["@CustomerType_ShortName"].Value = this.ShortName;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@CustomerType_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания типа клиента.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать тип клиента.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteCustomerType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@CustomerType_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления типа клиента.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить тип клиента.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditCustomerType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerType_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerType_ShortName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerType_IsActive", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@CustomerType_Guid"].Value = this.ID;
                cmd.Parameters["@CustomerType_Name"].Value = this.Name;
                cmd.Parameters["@CustomerType_ShortName"].Value = this.ShortName;
                cmd.Parameters["@CustomerType_IsActive"].Value = this.IsActive;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения типа клиента.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства типа клиента.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }

        #endregion

        #region Сохранить список типов клиентов для клиента
        /// <summary>
        /// Сохраняет в БД привязку типов клиентов к клиенту
        /// </summary>
        /// <param name="uuidCustomerId">уникальный идентификатор клиента</param>
        /// <param name="objCustomerTypeList">список типов клиентов</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean SaveCustomerTypeListForCustomer(System.Guid uuidCustomerId, 
            List<CCustomerType> objCustomerTypeList,
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteCustomerTypeFromCustomer]", objProfile.GetOptionsDllDBName());
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
                    if ((objCustomerTypeList != null) && (objCustomerTypeList.Count > 0))
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddCustomerTypeToCustomer]", objProfile.GetOptionsDllDBName());
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters["@Customer_Guid"].Value = uuidCustomerId;

                        foreach (CCustomerType objCustomerType in objCustomerTypeList)
                        {
                            cmd.Parameters["@CustomerType_Guid"].Value = objCustomerType.ID;
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
    #endregion

    #region Класс "Активность/Состояние клиента"
    /// <summary>
    /// Класс "Активность/Состояние клиента"
    /// </summary>
    public class CCustomerActiveType : CBusinessObject
    {
        public CCustomerActiveType()
            : base()
        {
        }
        public CCustomerActiveType(System.Guid uuidId, System.String strName)
            : base( uuidId, strName )
        {
        }

        #region Список объектов
        public static List<CCustomerActiveType> GetCustomerActiveTypeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CCustomerActiveType> objList = new List<CCustomerActiveType>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetCustomerActiveType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CCustomerActiveType((System.Guid)rs["CustomerActiveType_Guid"],
                            (System.String)rs["CustomerActiveType_Name"]));
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
                "Не удалось получить список признаков активности клиента.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddCustomerActiveType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerActiveType_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerActiveType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@CustomerActiveType_Name"].Value = this.Name;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@CustomerActiveType_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания признака активности клиента.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать признак активности клиента.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteCustomerActiveType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerActiveType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@CustomerActiveType_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления признака активности клиента.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить признак активности клиента.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditCustomerActiveType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerActiveType_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerActiveType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@CustomerActiveType_Guid"].Value = this.ID;
                cmd.Parameters["@CustomerActiveType_Name"].Value = this.Name;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств признака активности клиента.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства признака активности клиента.\n\nТекст ошибки: " + f.Message, "Внимание",
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

    #region Дочернее подразделение
    public class CChildDepart : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Код дочернего подразделения
        /// </summary>
        private System.String m_strCode;
        /// </summary>
        /// Код дочернего подразделения
        /// </summary>
        [DisplayName("Код")]
        [Description("Код дочернего подразделения")]
        [Category("1. Обязательные значения")]
        [PropertyOrder(20)]
        public System.String Code
        {
            get { return m_strCode; }
            set { m_strCode = value; }
        }
        /// <summary>
        /// Электронный адрес
        /// </summary>
        private System.String m_strEmail;
        /// <summary>
        /// Электронный адрес
        /// </summary>
        [DisplayName("Email")]
        [Description("Электронный адрес")]
        [Category("2. Необязательные значения")]
        public System.String Email
        {
            get { return m_strEmail; }
            set { m_strEmail = value; }
        }
        private System.Decimal m_dblMaxDebt;
        /// <summary>
        /// Лимит, сумма
        /// </summary>
        [DisplayName("Лимит, сумма")]
        [Description("Лимит отгрузки, сумма")]
        [Category("1. Обязательные значения")]
        [PropertyOrder(30)]
        public System.Decimal MaxDebt
        {
            get { return m_dblMaxDebt; }
            set { m_dblMaxDebt = value; }
        }
        /// <summary>
        /// Отсрочка, дней
        /// </summary>
        private System.Decimal m_dblMaxDelay;
        /// <summary>
        /// Отсрочка платежа, дней
        /// </summary>
        [DisplayName("Отсрочка платежа, дней")]
        [Description("Лимит отгрузки, сумма")]
        [Category("1. Обязательные значения")]
        [PropertyOrder(40)]
        public System.Decimal MaxDelay
        {
            get { return m_dblMaxDelay; }
            set { m_dblMaxDelay = value; }
        }
        /// <summary>
        /// Признак "основной"
        /// </summary>
        private System.Boolean m_bMain;
        /// <summary>
        /// Признак "основной"
        /// </summary>
        [DisplayName("Основной")]
        [Description("Код является основным для клиента")]
        [Category("2. Необязательные значения")]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public System.Boolean IsMain
        {
            get { return m_bMain; }
            set { m_bMain = value; }
        }
        /// <summary>
        /// Признак "заблокирован"
        /// </summary>
        private System.Boolean m_bBlock;
        /// <summary>
        /// Признак "заблокирован"
        /// </summary>
        [DisplayName("Заблокирован")]
        [Description("Подразделение заблокировано")]
        [Category("2. Необязательные значения")]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public System.Boolean IsBlock
        {
            get { return m_bBlock; }
            set { m_bBlock = value; }
        }

        #endregion

        #region Конструктор
        public CChildDepart()
            : base()
        {
            m_strCode = "";
            m_strEmail = "";
            m_bMain = false;
            m_bBlock = false;
            m_dblMaxDebt = 0;
            m_dblMaxDelay = 0;
        }
       public CChildDepart(System.Guid uuidId, System.String strCode, System.String strName, System.String strEmail,
           System.Decimal dblMaxDebt, System.Decimal dblMaxDelay, System.Boolean bIsMain, System.Boolean bIsBlock )
        {
            ID = uuidId;
            Name = strName;
            m_strCode = strCode;
            m_strEmail = strEmail;
            m_dblMaxDebt = dblMaxDebt;
            m_dblMaxDelay = dblMaxDelay;
            m_bMain = bIsMain;
            m_bBlock = bIsBlock;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список дочерних подразделений
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
       /// <returns>список дочерних подразделений</returns>
       public static List<CChildDepart> GetChildDepartList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, 
           System.Guid uuidCustomerID )
       {
           List<CChildDepart> objList = new List<CChildDepart>();
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

               cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetChildDepart]", objProfile.GetOptionsDllDBName());
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.DbType.Guid)); 
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
               cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
               if (uuidCustomerID.CompareTo( System.Guid.Empty ) == 0)
               {
                   cmd.Parameters["@Customer_Guid"].IsNullable = true;
                   cmd.Parameters["@Customer_Guid"].Value = null;
               }
               else
               {
                   cmd.Parameters["@Customer_Guid"].IsNullable = false;
                   cmd.Parameters["@Customer_Guid"].Value = uuidCustomerID;
               }
               System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
               if (rs.HasRows)
               {
                   while (rs.Read())
                   {
                       objList.Add(new CChildDepart((System.Guid)rs["ChildDepart_Guid"], (System.String)rs["ChildDepart_Code"],
                           System.Convert.ToString(rs["ChildDepart_Name"]),
                           ((rs["ChildDepart_Email"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["ChildDepart_Email"])),
                           ((rs["ChildDepart_MaxDebt"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["ChildDepart_MaxDebt"])),
                           ((rs["ChildDepart_MaxDelay"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["ChildDepart_MaxDelay"])),
                           ((rs["ChildDepart_Main"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["ChildDepart_Main"])),
                           ((rs["ChildDepart_NotActive"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["ChildDepart_NotActive"]))
                           ));
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
               "Не удалось получить список дочерних подразделений.\n\nТекст ошибки: " + f.Message, "Внимание",
               System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
           }
           return objList;
       }

       public static CChildDepart GetChildDepart(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
   System.Guid uuidChildDepartID)
       {
           CChildDepart objList = new CChildDepart();
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

               cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetChildDepart2]", objProfile.GetOptionsDllDBName());
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ChildDepart_Guid", System.Data.DbType.Guid));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
               cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
               cmd.Parameters["@ChildDepart_Guid"].IsNullable = false;
               cmd.Parameters["@ChildDepart_Guid"].Value = uuidChildDepartID;
               System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
               if (rs.HasRows)
               {
                   rs.Read();
                   {
                       objList = new CChildDepart((System.Guid)rs["ChildDepart_Guid"], (System.String)rs["ChildDepart_Code"],
                           System.Convert.ToString(rs["ChildDepart_Name"]),
                           ((rs["ChildDepart_Email"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["ChildDepart_Email"])),
                           ((rs["ChildDepart_MaxDebt"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["ChildDepart_MaxDebt"])),
                           ((rs["ChildDepart_MaxDelay"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["ChildDepart_MaxDelay"])),
                           ((rs["ChildDepart_Main"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["ChildDepart_Main"])),
                           ((rs["ChildDepart_NotActive"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["ChildDepart_NotActive"]))
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
               "Не удалось получить список дочерних подразделений.\n\nТекст ошибки: " + f.Message, "Внимание",
               System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
           }
           return objList;
       }

        #endregion

        #region Add
        /// <summary>
        /// Проверка значений перед сохранением в БД
        /// </summary>
        /// <returns></returns>
        public override bool IsAllParametersValid()
        {
            System.Boolean bRet = false;
            try
            {
                if (m_strCode.Trim() == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Укажите, пожалуйста, код дочернего подразделения.", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                }

                bRet = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Проверка на заполнение обязательных значений.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
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
           System.Boolean bEditCustomerChildList = objProfile.GetClientsRight().GetState(ERP_Mercury.Global.Consts.strDR_EditCustomerChildDepartLimit);
           if (bEditCustomerChildList == false)
           {
               DevExpress.XtraEditors.XtraMessageBox.Show("У Вас недостаточно прав для этой операции.", "Внимание",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
               return bRet;
           }

           if (IsAllParametersValid() == false) { return bRet; }

           System.Data.SqlClient.SqlConnection DBConnection = null;
           System.Data.SqlClient.SqlCommand cmd = null;
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
               cmd = new System.Data.SqlClient.SqlCommand();
               cmd.Connection = DBConnection;
               cmd.CommandType = System.Data.CommandType.StoredProcedure;
               cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddChildDepart]", objProfile.GetOptionsDllDBName());
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ChildDepart_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ChildDepart_Code", System.Data.DbType.String));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ChildDepart_Name", System.Data.DbType.String));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ChildDepart_Email", System.Data.DbType.String));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ChildDepart_MaxDebt", System.Data.SqlDbType.Money));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ChildDepart_MaxDelay", System.Data.SqlDbType.Money));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ChildDepart_Main", System.Data.SqlDbType.Bit));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ChildDepart_NotActive", System.Data.SqlDbType.Bit));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
               cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
               cmd.Parameters["@ChildDepart_Code"].Value = this.Code;
               cmd.Parameters["@ChildDepart_Name"].Value = this.Name;
               cmd.Parameters["@ChildDepart_Email"].Value = this.Email;
               cmd.Parameters["@ChildDepart_MaxDebt"].Value = this.MaxDebt;
               cmd.Parameters["@ChildDepart_MaxDelay"].Value = this.MaxDelay;
               cmd.Parameters["@ChildDepart_Main"].Value = this.IsMain;
               cmd.Parameters["@ChildDepart_NotActive"].Value = this.IsBlock;
               cmd.ExecuteNonQuery();
               System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
               if (iRes == 0)
               {
                   this.ID = (System.Guid)cmd.Parameters["@ChildDepart_Guid"].Value;
               }
               else
               {
                   strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                   DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания кода дочернего подразделения.\n\nТекст ошибки: " + strErr, "Ошибка",
                       System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
               }

               cmd.Dispose();
               bRet = (iRes == 0);
           }
           catch (System.Exception f)
           {
               DevExpress.XtraEditors.XtraMessageBox.Show(
               "Не удалось создать код дочернего подразделения.\n\nТекст ошибки: " + f.Message, "Внимание",
               System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
           }
           finally
           {
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
           System.Boolean bEditCustomerChildList = objProfile.GetClientsRight().GetState(ERP_Mercury.Global.Consts.strDR_EditCustomerChildDepartLimit);
           if (bEditCustomerChildList == false)
           {
               DevExpress.XtraEditors.XtraMessageBox.Show("У Вас недостаточно прав для этой операции.", "Внимание",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
               return bRet;
           }

           if (IsAllParametersValid() == false) { return bRet; }

           System.Data.SqlClient.SqlConnection DBConnection = null;
           System.Data.SqlClient.SqlCommand cmd = null;
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
               cmd = new System.Data.SqlClient.SqlCommand();
               cmd.Connection = DBConnection;
               cmd.CommandType = System.Data.CommandType.StoredProcedure;
               cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditChildDepart]", objProfile.GetOptionsDllDBName());
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ChildDepart_Guid", System.Data.DbType.Guid));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ChildDepart_Code", System.Data.DbType.String));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ChildDepart_Name", System.Data.DbType.String));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ChildDepart_Email", System.Data.DbType.String));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ChildDepart_MaxDebt", System.Data.SqlDbType.Money));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ChildDepart_MaxDelay", System.Data.SqlDbType.Money));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ChildDepart_Main", System.Data.SqlDbType.Bit));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ChildDepart_NotActive", System.Data.SqlDbType.Bit));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
               cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
               cmd.Parameters["@ChildDepart_Guid"].Value = this.ID;
               cmd.Parameters["@ChildDepart_Code"].Value = this.Code;
               cmd.Parameters["@ChildDepart_Name"].Value = this.Name;
               cmd.Parameters["@ChildDepart_Email"].Value = this.Email;
               cmd.Parameters["@ChildDepart_MaxDebt"].Value = this.MaxDebt;
               cmd.Parameters["@ChildDepart_MaxDelay"].Value = this.MaxDelay;
               cmd.Parameters["@ChildDepart_Main"].Value = this.IsMain;
               cmd.Parameters["@ChildDepart_NotActive"].Value = this.IsBlock;
               cmd.ExecuteNonQuery();
               System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
               if (iRes != 0)
               {
                   strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                   DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств дочернего подразделения.\n\nТекст ошибки: " + strErr, "Ошибка",
                       System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
               }

               cmd.Dispose();
               bRet = (iRes == 0);
           }
           catch (System.Exception f)
           {
               DevExpress.XtraEditors.XtraMessageBox.Show(
               "Не удалось изменить свойства дочернего подразделения клиента.\n\nТекст ошибки: " + f.Message, "Внимание",
               System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
           }
           finally
           {
               DBConnection.Close();
           }
           return bRet;
       }
        /// <summary>
        /// Сохраняет в БД привязку дочерних подразделений к клиенту
        /// </summary>
        /// <param name="objChildDepartList">список подразделений</param>
        /// <param name="uuidCustomerID">УИ клиента</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
       public static System.Boolean SaveCustomerChildListInDB(List<CChildDepart> objChildDepartList, System.Guid uuidCustomerID, 
           UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
       {
           System.Boolean bRet = false;
           System.Data.SqlClient.SqlConnection DBConnection = null;
           System.Data.SqlClient.SqlCommand cmd = null;
           try
           {
               if (objChildDepartList == null) { objChildDepartList = new List<CChildDepart>(); }
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
               }

               System.Data.DataTable addedCategories = new System.Data.DataTable();
               addedCategories.Columns.Add(new System.Data.DataColumn("Customer_Guid", typeof(System.Data.SqlTypes.SqlGuid)));

               System.Data.DataRow newRow = null;
               foreach (CChildDepart objChildDepart in objChildDepartList)
               {
                   newRow = addedCategories.NewRow();
                   newRow[0] = objChildDepart.ID;
                   addedCategories.Rows.Add(newRow);
               }
               if (objChildDepartList.Count > 0)
               {
                   addedCategories.AcceptChanges();
               }

               cmd.Parameters.Clear();
               cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AssignCustomerChild]", objProfile.GetOptionsDllDBName());
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
               cmd.Parameters.AddWithValue("@tChildDepartList", addedCategories);
               cmd.Parameters["@tChildDepartList"].SqlDbType = System.Data.SqlDbType.Structured;
               cmd.Parameters["@tChildDepartList"].TypeName = "dbo.udt_CustomerListForDepart";
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
               cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

               cmd.Parameters["@Customer_Guid"].Value = uuidCustomerID;
               cmd.ExecuteNonQuery();
               System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
               if (iRes != 0)
               {
                   strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
               }

               if (cmdSQL == null)
               {
                   cmd.Dispose();
                   cmd = null;
               }
               bRet = (iRes == 0);
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
           System.Boolean bEditCustomerChildList = objProfile.GetClientsRight().GetState(ERP_Mercury.Global.Consts.strDR_EditCustomerChildDepartLimit);
           if (bEditCustomerChildList == false)
           {
               DevExpress.XtraEditors.XtraMessageBox.Show("У Вас недостаточно прав для этой операции.", "Внимание",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
               return bRet;
           }
           System.Data.SqlClient.SqlConnection DBConnection = null;
           System.Data.SqlClient.SqlCommand cmd = null;
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
               cmd = new System.Data.SqlClient.SqlCommand();
               cmd.Connection = DBConnection;
               cmd.CommandType = System.Data.CommandType.StoredProcedure;
               cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteChildDepart]", objProfile.GetOptionsDllDBName());
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ChildDepart_Guid", System.Data.SqlDbType.UniqueIdentifier));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
               cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
               cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
               cmd.Parameters["@ChildDepart_Guid"].Value = this.ID;
               cmd.ExecuteNonQuery();
               System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
               bRet = (iRes == 0);

               if (bRet == false)
               {
                   DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления дочернего подразделения.\n\nТекст ошибки: " +
                   (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                       System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
               }
               cmd.Dispose();
           }
           catch (System.Exception f)
           {
               DevExpress.XtraEditors.XtraMessageBox.Show(
               "Не удалось удалить дочернее подразделение.\n\nТекст ошибки: " + f.Message, "Внимание",
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
            return Code;
        }
    }
    #endregion

    #region Лицензия
    /// <summary>
    /// Класс "Тип лицензии"
    /// </summary> 
    public class CLicenceType : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Примечание
        /// </summary>
        private System.String m_strDscrpn;
        /// <summary>
        /// Примечание
        /// </summary>
        [DisplayName("Примечание")]
        [Description("Примечание")]
        [Category("2. Дополнительно")]
        public System.String Description
        {
            get { return m_strDscrpn; }
            set { m_strDscrpn = value; }
        }
        #endregion

        public CLicenceType()
            : base()
        {
            m_strDscrpn = "";
        }
        public CLicenceType(System.Guid uuidId, System.String strName, System.String strDscrpn )
        {
            ID = uuidId;
            Name = strName;
            m_strDscrpn = strDscrpn;
        }

        #region Список объектов
        /// <summary>
        /// Возвращает список типов лицензий
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список типов лицензий</returns>
        public static List<CLicenceType> GetLicenceTypeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CLicenceType> objList = new List<CLicenceType>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetLicenceType]", objProfile.GetOptionsDllDBName());
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
                        strDscrpn = (rs["LicenceType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["LicenceType_Description"];
                        objList.Add( new CLicenceType( (System.Guid)rs["LicenceType_Guid"],
                            (System.String)rs["LicenceType_Name"], strDscrpn ) );
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
                "Не удалось получить список типов лицензий.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddLicenceType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LicenceType_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LicenceType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LicenceType_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@LicenceType_Name"].Value = this.Name;
                cmd.Parameters["@LicenceType_Description"].Value = this.Description;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@LicenceType_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания типа лицензии.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать тип лицензии.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteLicenceType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LicenceType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@LicenceType_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления типа лицензии.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить тип лицензии.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditLicenceType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LicenceType_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LicenceType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LicenceType_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@LicenceType_Guid"].Value = this.ID;
                cmd.Parameters["@LicenceType_Name"].Value = this.Name;
                cmd.Parameters["@LicenceType_Description"].Value = this.Description;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения типа лицензии.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства типа лицензии.\n\nТекст ошибки: " + f.Message, "Внимание",
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
    /// Класс "Лицензия клиента"
    /// </summary>
    public class CLicence
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
        /// <summary>
        /// Номер лицензии
        /// </summary>
        private System.String m_strLicenceNum;
        /// <summary>
        /// Номер лицензии
        /// </summary>
        public System.String LicenceNum
        {
            get { return m_strLicenceNum; }
            set { m_strLicenceNum = value; }
        }
        /// <summary>
        /// Когда выдана
        /// </summary>
        private System.DateTime m_dtBeginDate;
        /// <summary>
        /// Когда выдана
        /// </summary>
        public System.DateTime BeginDate
        {
            get { return m_dtBeginDate; }
            set { m_dtBeginDate = value; }
        }
        /// <summary>
        /// Срок действия
        /// </summary>
        private System.DateTime m_dtEndDate;
        /// <summary>
        /// Срок действия
        /// </summary>
        public System.DateTime EndDate
        {
            get { return m_dtEndDate; }
            set { m_dtEndDate = value; }
        }
        /// <summary>
        /// Кем выдана
        /// </summary>
        private System.String m_strWhoGive;
        /// <summary>
        /// Кем выдана
        /// </summary>
        public System.String WhoGive
        {
            get { return m_strWhoGive; }
            set { m_strWhoGive = value; }
        }
        #endregion

        public CLicence()
        {
            m_uuidID = System.Guid.Empty;
            m_objLicenceType = null;
            m_strLicenceNum = "";
            m_dtBeginDate = System.DateTime.Today;
            m_dtEndDate = System.DateTime.Today;
            m_strWhoGive = "";
        }
        public CLicence(System.Guid uuidID, CLicenceType objLicenceType, System.String strLicenceNum,
            System.DateTime dtBeginDate, System.DateTime dtEndDate, System.String strWhoGive)
        {
            m_uuidID = uuidID;
            m_objLicenceType = objLicenceType;
            m_strLicenceNum = strLicenceNum;
            m_dtBeginDate = dtBeginDate;
            m_dtEndDate = dtEndDate;
            m_strWhoGive = strWhoGive;
        }

        #region Список объектов
        /// <summary>
        /// Возвращает список лицензий для клиента
        /// </summary>
        /// <param name="CustomerId">уи клиента</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список типов лицензий</returns>
        public static List<CLicence> GetLicenceList( System.Guid CustomerId, UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CLicence> objList = new List<CLicence>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetCustomerLicence]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Customer_Guid"].Value = CustomerId;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.String strDscrpn = "";
                    while (rs.Read())
                    {
                        strDscrpn = (rs["LicenceType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["LicenceType_Description"];
                        objList.Add( new CLicence( ( System.Guid )rs["Licence_Guid"],                            
                            new CLicenceType( ( System.Guid )rs["LicenceType_Guid"], ( System.String )rs["LicenceType_Name"], strDscrpn),
                            ( System.String )rs["Licence_Num"], Convert.ToDateTime( rs["Licence_BeginDate"] ), Convert.ToDateTime( rs["Licence_EndDate"] ),
                            ( System.String )rs["Licence_WhoGive"] ) );
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
                "Не удалось получить список лицензий.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }

        public static List<CLicence> GetLicenceListForCompany( System.Guid CompanyId, UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CLicence> objList = new List<CLicence>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCompanyLicence]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Company_Guid"].Value = CompanyId;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.String strDscrpn = "";
                    while (rs.Read())
                    {
                        strDscrpn = (rs["LicenceType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["LicenceType_Description"];
                        objList.Add(new CLicence((System.Guid)rs["Licence_Guid"],
                            new CLicenceType((System.Guid)rs["LicenceType_Guid"], (System.String)rs["LicenceType_Name"], strDscrpn),
                            (System.String)rs["Licence_Num"], Convert.ToDateTime(rs["Licence_BeginDate"]), Convert.ToDateTime(rs["Licence_EndDate"]),
                            (System.String)rs["Licence_WhoGive"]));
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
                "Не удалось получить список лицензий.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
            
        }
        #endregion

        #region Удалить лицензию из базы данных
        /// <summary>
        /// Удаляет лицензию из базы данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="uuidObjectId">идентификатор клиента</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Remove( System.Guid uuidObjectId,
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
                strDeleteCmd = System.String.Format("[{0}].[dbo].[sp_DeleteCustomerLicence]", objProfile.GetOptionsDllDBName());
                cmd.CommandText = strDeleteCmd;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Licence_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Licence_Guid"].Value = this.ID;
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

        /// <summary>
        /// Удаляет лицензию компании из базы данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="uuidObjectId">идентификатор клиента</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean RemoveFromCompany(System.Guid uuidObjectId,
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
                strDeleteCmd = System.String.Format("[{0}].[dbo].[usp_DeleteCompanyLicence]", objProfile.GetOptionsDllDBName());
                cmd.CommandText = strDeleteCmd;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Licence_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Licence_Guid"].Value = this.ID;
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

        #region Добавить в БД лицензию
        /// <summary>
        /// Проверка свойств лицензии перед сохранением
        /// </summary>
        /// <param name="strErr">текст с ошибкой</param>
        /// <returns>true - все свойства корректны; false - ошибка</returns>
        public System.Boolean IsAllParametersValid(ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (this.LicenceNum == "")
                {
                    strErr = "Необходимо указать номер лицензии!";
                    return bRet;
                }
                if (this.m_objLicenceType == null)
                {
                    strErr = "Необходимо указать тип лицензии!";
                    return bRet;
                }
                if (this.m_strWhoGive.Trim().Length == 0)
                {
                    strErr = "Необходимо указать кто выдал лицензию!";
                    return bRet;
                }
                if (this.m_dtEndDate <= this.m_dtBeginDate)
                {
                    strErr = "Срок действия должен быть больше даты выдачи!";
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
        /// Добавляет лицензию в базу данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="uuidObjectId">идентификатор клиента</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Add( System.Guid uuidObjectId,
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
                strAddCmd = System.String.Format("[{0}].[dbo].[sp_AddLicenceToCustomer]", objProfile.GetOptionsDllDBName());

                cmd.Parameters.Clear();
                cmd.CommandText = strAddCmd;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Licence_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Licence_Num", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Licence_WhoGive", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Licence_BeginDate", System.Data.DbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Licence_EndDate", System.Data.DbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LicenceType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Licence_Num"].Value = this.LicenceNum;
                cmd.Parameters["@Licence_WhoGive"].Value = this.WhoGive;
                cmd.Parameters["@LicenceType_Guid"].Value = this.LicenceType.ID;
                cmd.Parameters["@Licence_BeginDate"].Value = this.BeginDate;
                cmd.Parameters["@Licence_EndDate"].Value = this.EndDate;
                cmd.Parameters["@Customer_Guid"].Value = uuidObjectId;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    // 2009.08.11
                    // в том случае, если лицензия сохраняется из ее владельца, и вдруг нужно откатить транзакцию, то
                    // ID должен остаться пустым
                    //this.m_uuidID = (System.Guid)cmd.Parameters["@Licence_Guid"].Value;
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
        /// Добавляет лицензию для компании в базу данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="uuidObjectId">идентификатор клиента</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean AddForCompany(System.Guid uuidObjectId,
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
                strAddCmd = System.String.Format("[{0}].[dbo].[usp_AddLicenceToCompany]", objProfile.GetOptionsDllDBName());

                cmd.Parameters.Clear();
                cmd.CommandText = strAddCmd;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Licence_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Licence_Num", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Licence_WhoGive", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Licence_BeginDate", System.Data.DbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Licence_EndDate", System.Data.DbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LicenceType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Licence_Num"].Value = this.LicenceNum;
                cmd.Parameters["@Licence_WhoGive"].Value = this.WhoGive;
                cmd.Parameters["@LicenceType_Guid"].Value = this.LicenceType.ID;
                cmd.Parameters["@Licence_BeginDate"].Value = this.BeginDate;
                cmd.Parameters["@Licence_EndDate"].Value = this.EndDate;
                cmd.Parameters["@Company_Guid"].Value = uuidObjectId;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    // 2009.08.11
                    // в том случае, если лицензия сохраняется из ее владельца, и вдруг нужно откатить транзакцию, то
                    // ID должен остаться пустым
                    //this.m_uuidID = (System.Guid)cmd.Parameters["@Licence_Guid"].Value;
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
        /// Сохраняет в БД список лицензий для клиента
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="objAddressList">список телефонных номеров</param>
        /// <param name="enObjectWithAddress">тип владельца телефонных номеров</param>
        /// <param name="uuidObjectId">идентификатор владельца телефонных номеров</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean SaveLicenceList(List<CLicence> objLicenceList, List<CLicence> objLicenceForDeleteList,
            System.Guid uuidObjectId,
            UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            if (((objLicenceList == null) || (objLicenceList.Count == 0)) && ((objLicenceForDeleteList == null) || (objLicenceForDeleteList.Count == 0))) { return true; }
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                // для начала проверим, что нам пришло в списке
                if ((objLicenceList != null) && (objLicenceList.Count > 0))
                {
                    System.Boolean bIsAllValid = true;
                    foreach (CLicence objItem in objLicenceList)
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
                if ((objLicenceForDeleteList != null) && (objLicenceForDeleteList.Count > 0))
                {
                    foreach (CLicence objLicence in objLicenceForDeleteList)
                    {
                        if (objLicence.ID.CompareTo(System.Guid.Empty) == 0) { continue; }
                        iRes = (objLicence.Remove(uuidObjectId, objProfile, cmd, ref strErr) == true) ? 0 : 1;
                        if (iRes != 0) { break; }
                    }
                }

                if (iRes == 0)
                {
                    if ((objLicenceList != null) && (objLicenceList.Count > 0))
                    {
                        // теперь в цикле добавим в БД каждый член из списка
                        foreach (CLicence objLicence in objLicenceList)
                        {
                            if (objLicence.ID.CompareTo(System.Guid.Empty) == 0)
                            {
                                // новая лицензия
                                iRes = (objLicence.Add(uuidObjectId, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                            }
                            else
                            {
                                iRes = (objLicence.Update(uuidObjectId, objProfile, cmd, ref strErr) == true) ? 0 : -1;
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
        /// Сохраняет в БД список лицензий для компании
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="objAddressList">список телефонных номеров</param>
        /// <param name="enObjectWithAddress">тип владельца телефонных номеров</param>
        /// <param name="uuidObjectId">идентификатор владельца телефонных номеров</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean SaveLicenceListForCompany(List<CLicence> objLicenceList, List<CLicence> objLicenceForDeleteList,
            System.Guid uuidObjectId,
            UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            if (((objLicenceList == null) || (objLicenceList.Count == 0)) && ((objLicenceForDeleteList == null) || (objLicenceForDeleteList.Count == 0))) { return true; }
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                // для начала проверим, что нам пришло в списке
                if ((objLicenceList != null) && (objLicenceList.Count > 0))
                {
                    System.Boolean bIsAllValid = true;
                    foreach (CLicence objItem in objLicenceList)
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
                if ((objLicenceForDeleteList != null) && (objLicenceForDeleteList.Count > 0))
                {
                    foreach (CLicence objLicence in objLicenceForDeleteList)
                    {
                        if (objLicence.ID.CompareTo(System.Guid.Empty) == 0) { continue; }
                        iRes = (objLicence.RemoveFromCompany(uuidObjectId, objProfile, cmd, ref strErr) == true) ? 0 : 1;
                        if (iRes != 0) { break; }
                    }
                }

                if (iRes == 0)
                {
                    if ((objLicenceList != null) && (objLicenceList.Count > 0))
                    {
                        // теперь в цикле добавим в БД каждый член из списка
                        foreach (CLicence objLicence in objLicenceList)
                        {
                            if (objLicence.ID.CompareTo(System.Guid.Empty) == 0)
                            {
                                // новая лицензия
                                iRes = (objLicence.AddForCompany(uuidObjectId, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                            }
                            else
                            {
                                iRes = (objLicence.UpdateForCompany(uuidObjectId, objProfile, cmd, ref strErr) == true) ? 0 : -1;
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

        #region Изменение свойств лицензии в БД
        /// <summary>
        /// Изменение свойств лицензии в БД
        /// </summary>
        /// <param name="uuidObjectId">идентификатор владельца</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Update(System.Guid uuidObjectId,
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
                strAddCmd = System.String.Format("[{0}].[dbo].[sp_EditLicenceToCustomer]", objProfile.GetOptionsDllDBName());

                cmd.Parameters.Clear();
                cmd.CommandText = strAddCmd;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Licence_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Licence_Num", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Licence_WhoGive", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Licence_BeginDate", System.Data.DbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Licence_EndDate", System.Data.DbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LicenceType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Licence_Guid"].Value = this.ID;
                cmd.Parameters["@Licence_Num"].Value = this.LicenceNum;
                cmd.Parameters["@Licence_WhoGive"].Value = this.WhoGive;
                cmd.Parameters["@LicenceType_Guid"].Value = this.LicenceType.ID;
                cmd.Parameters["@Licence_BeginDate"].Value = this.BeginDate;
                cmd.Parameters["@Licence_EndDate"].Value = this.EndDate;
                cmd.Parameters["@Customer_Guid"].Value = uuidObjectId;
                cmd.ExecuteNonQuery();

                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = "Ошибка редактирования лицензии. Текст ошибки: " + (System.String)cmd.Parameters["@ERROR_MES"].Value;
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
                strErr = "Ошибка редактирования лицензии. Текст ошибки: " + f.Message;
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
        /// Изменение свойств лицензии в БД для компании
        /// </summary>
        /// <param name="uuidObjectId">идентификатор владельца</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean UpdateForCompany(System.Guid uuidObjectId,
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
                strAddCmd = System.String.Format("[{0}].[dbo].[usp_EditLicenceToCompany]", objProfile.GetOptionsDllDBName());

                cmd.Parameters.Clear();
                cmd.CommandText = strAddCmd;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Licence_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Licence_Num", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Licence_WhoGive", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Licence_BeginDate", System.Data.DbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Licence_EndDate", System.Data.DbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LicenceType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Licence_Guid"].Value = this.ID;
                cmd.Parameters["@Licence_Num"].Value = this.LicenceNum;
                cmd.Parameters["@Licence_WhoGive"].Value = this.WhoGive;
                cmd.Parameters["@LicenceType_Guid"].Value = this.LicenceType.ID;
                cmd.Parameters["@Licence_BeginDate"].Value = this.BeginDate;
                cmd.Parameters["@Licence_EndDate"].Value = this.EndDate;
                cmd.Parameters["@Company_Guid"].Value = uuidObjectId;
                cmd.ExecuteNonQuery();

                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = "Ошибка редактирования лицензии. Текст ошибки: " + (System.String)cmd.Parameters["@ERROR_MES"].Value;
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
                strErr = "Ошибка редактирования лицензии. Текст ошибки: " + f.Message;
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

    #endregion

    #region Торговая сеть
    /// <summary>
    /// Класс "Торговая сеть"
    /// </summary>
    public class CDistributionNetwork : CBusinessObject
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
        ///// <summary>
        ///// Список клиентов
        ///// </summary>
        //private List<CCustomer> m_objCustomerList;
        ///// <summary>
        ///// Список клиентов
        ///// </summary>
        //[DisplayName("Список клиентов")]
        //[Description("Список клиентов торговой сети")]
        //[Category("2. Необязательные значения")]
        //public List<CCustomer> CustomerList
        //{
        //    get { return m_objCustomerList; }
        //    set { m_objCustomerList = value; }
        //}
        #endregion

        #region Конструктор
         public CDistributionNetwork()
            : base()
        {
            m_strDescription = "";
            //m_objCustomerList = null;
        }
        public CDistributionNetwork(System.Guid uuidId, System.String strName, System.String strDscrpn)
            : base( uuidId, strName )
        {
            m_strDescription = strDscrpn;
            //m_objCustomerList = null;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список объектов "Торговая сеть"
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список объектов "тип оборудования"</returns>
        public static List<CDistributionNetwork> GetDistributionNetworkList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CDistributionNetwork> objList = new List<CDistributionNetwork>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetDistribNet]", objProfile.GetOptionsDllDBName());
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
                        strDscrpn = (rs["DistribNet_Description"] == System.DBNull.Value) ? "" : (System.String)rs["DistribNet_Description"];
                        objList.Add(new CDistributionNetwork((System.Guid)rs["DistribNet_Guid"], (System.String)rs["DistribNet_Name"], strDscrpn));
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
                "Не удалось получить список объектов \"Торговая сеть\".\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        ///// <summary>
        ///// Загружает список клиентов торговой сети
        ///// </summary>
        ///// <param name="objProfile">профайл</param>
        ///// <param name="cmdSQL">SQL-команда</param>
        //public void LoadCustomerList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        //{
        //    System.Data.SqlClient.SqlConnection DBConnection = null;
        //    System.Data.SqlClient.SqlCommand cmd = null;
        //    try
        //    {
        //        if (this.m_objCustomerList == null) { this.m_objCustomerList = new List<CCustomer>(); }
        //        else { this.m_objCustomerList.Clear(); }
        //        if (cmdSQL == null)
        //        {
        //            DBConnection = objProfile.GetDBSource();
        //            if (DBConnection == null)
        //            {
        //                DevExpress.XtraEditors.XtraMessageBox.Show(
        //                    "Не удалось получить соединение с базой данных.", "Внимание",
        //                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        //                return ;
        //            }
        //            cmd = new System.Data.SqlClient.SqlCommand();
        //            cmd.Connection = DBConnection;
        //            cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //        }
        //        else
        //        {
        //            cmd = cmdSQL;
        //            cmd.Parameters.Clear();
        //        }

        //        cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetDistribNetItems]", objProfile.GetOptionsDllDBName());
        //        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
        //        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DistribNet_Guid", System.Data.SqlDbType.UniqueIdentifier));
        //        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
        //        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
        //        cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
        //        cmd.Parameters["@DistribNet_Guid"].Value = this.ID;
        //        System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
        //        if (rs.HasRows)
        //        {
        //            while (rs.Read())
        //            {
        //                this.m_objCustomerList.Add(new CCustomer((System.Guid)rs["Customer_Guid"], (System.Int32)rs["CUSTOMER_ID"], (System.String)rs["Customer_Code"],
        //                    (System.String)rs["CUSTOMER_NAME"], (System.String)rs["CUSTOMER_NAME"],
        //                    (System.String)rs["Customer_UNP"], (System.String)rs["Customer_OKPO"], (System.String)rs["Customer_OKULP"],
        //                    // признак активности
        //                    new CCustomerActiveType((System.Guid)rs["CustomerActiveType_Guid"], (System.String)rs["CustomerActiveType_Name"]),
        //                    // форма собственности
        //                    new CStateType((System.Guid)rs["CustomerStateType_Guid"],
        //                    (System.String)rs["CustomerStateType_Name"],
        //                    (System.String)rs["CustomerStateType_ShortName"],
        //                    (System.Boolean)rs["CustomerStateType_IsActive"])
        //                    ));
        //            }
        //        }
        //        rs.Dispose();
        //        if (cmdSQL == null)
        //        {
        //            cmd.Dispose();
        //            DBConnection.Close();
        //        }
        //    }
        //    catch (System.Exception f)
        //    {
        //        DevExpress.XtraEditors.XtraMessageBox.Show(
        //        "Не удалось получить список оклиентов для торговой сети.\n\nТекст ошибки: " + f.Message, "Внимание",
        //        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
        //    }
        //    return ;
        //}
        /// <summary>
        /// Возвращает объект "Торговая сеть" для заданного клиента
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="uuidCustomerId">уи клиента</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>объект "Торговая сеть"</returns>
        public static CDistributionNetwork GetDistributionNetworkForCustomer(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidCustomerId, ref System.String strErr)
        {
            CDistributionNetwork objDistrNet = null;
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
                        return objDistrNet;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetDistribNetForCustomer]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Customer_Guid"].Value = uuidCustomerId;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    rs.Read();
                    objDistrNet = new CDistributionNetwork((System.Guid)rs["DistribNet_Guid"], (System.String)rs["DistribNet_Name"],
                        ((rs["DistribNet_Description"] == System.DBNull.Value) ? "" : (System.String)rs["DistribNet_Description"])
                        );
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
                strErr = "Не удалось получить торговую сеть для клиента.\n\nТекст ошибки: " + f.Message;
            }
            return objDistrNet;
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddDistribNet]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DistribNet_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DistribNet_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DistribNet_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@DistribNet_Name"].Value = this.Name;
                if (this.m_strDescription == "")
                {
                    cmd.Parameters["@DistribNet_Description"].IsNullable = true;
                    cmd.Parameters["@DistribNet_Description"].Value = null;
                }
                else
                {
                    cmd.Parameters["@DistribNet_Description"].IsNullable = false;
                    cmd.Parameters["@DistribNet_Description"].Value = this.m_strDescription;
                }
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@DistribNet_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания объектa \"Торговая сеть\".\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать объект \"Торговая сеть\".\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        /// <summary>
        /// Связывает клиента с торговой сетью
        /// </summary>
        /// <param name="uuidCustomerId">ссылка на клиента</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public System.Boolean SaveCustomerToDistrNet(System.Guid uuidCustomerId,
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddDistribNetItem]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DistribNet_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Customer_Guid"].Value = uuidCustomerId;
                cmd.Parameters["@DistribNet_Guid"].Value = this.ID;
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteDistribNet]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DistribNet_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@DistribNet_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления объектa \"Торговая сеть\".\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить объект \"Торговая сеть\".\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }

        /// <summary>
        /// Удаляет клиента из торговой сети
        /// </summary>
        /// <param name="uuidCustomerId">ссылка на клиента</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean RemoveCustomerFromDistrNet(System.Guid uuidCustomerId,
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteCustomerFromDistribNet]", objProfile.GetOptionsDllDBName());
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditDistribNet]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DistribNet_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DistribNet_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DistribNet_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@DistribNet_Guid"].Value = this.ID;
                cmd.Parameters["@DistribNet_Name"].Value = this.Name;
                if (this.m_strDescription == "")
                {
                    cmd.Parameters["@DistribNet_Description"].IsNullable = true;
                    cmd.Parameters["@DistribNet_Description"].Value = null;
                }
                else
                {
                    cmd.Parameters["@DistribNet_Description"].IsNullable = false;
                    cmd.Parameters["@DistribNet_Description"].Value = this.m_strDescription;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств объектa \"Торговая сеть\".\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства объектa \"Торговая сеть\".\n\nТекст ошибки: " + f.Message, "Внимание",
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

    #region Компания
    /// <summary>
    /// Класс CCompany был исключён из СCustomer и физически перенесён в CCompany.cs, однако namespace не изменился – ERP_Mercury.Common
    /// </summary>
    
    #endregion

    #region Категория клиента
    /// <summary>
    /// Класс "Категория клиента"
    /// </summary>
    public class CCustomerCategory : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Код
        /// </summary>
        private System.String m_strCode;
        /// <summary>
        /// Код
        /// </summary>
        [DisplayName("Код")]
        [Description("Код категории клиента")]
        [Category("1. Обязательные значения")]
        public System.String Code
        {
            get { return m_strCode; }
            set { m_strCode = value; }
        }
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
        /// Сокращенное наименование
        /// </summary>
        private System.String m_strDescription;
        /// <summary>
        /// Сокращенное наименование
        /// </summary>
        [DisplayName("Описание")]
        [Description("Дополнительная информация")]
        [Category("2. Дополнительно")]
        public System.String Description
        {
            get { return m_strDescription; }
            set { m_strDescription = value; }
        }
        #endregion

        #region Конструктор
        public CCustomerCategory() : base()
        {
        }
        public CCustomerCategory( System.Guid uuidId, System.String strCode, System.String strName, 
            System.String strDscrpn, System.Boolean bIsActive )
        {
            ID = uuidId;
            Name = strName;

            m_strCode = strCode;
            m_IsActive = bIsActive;
            m_strDescription = strDscrpn;
        }
        #endregion

        #region Список объектов
        public static List<CCustomerCategory> GetCustomerCategoryList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CCustomerCategory> objList = new List<CCustomerCategory>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetCustomerCategory]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CCustomerCategory((System.Guid)rs["CustomerCategory_Guid"],
                            (System.String)rs["CustomerCategory_Code"],
                            (System.String)rs["CustomerCategory_Name"], 
                            ( rs["CustomerCategory_Description"] == System.DBNull.Value ? "" : (System.String)rs["CustomerCategory_Description"] ),
                            (System.Boolean)rs["CustomerCategory_IsActive"]));
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
                "Не удалось получить список категорий клиентов.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                if (this.Code == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать код!", "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddCustomerCategory]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerCategory_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerCategory_Code", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerCategory_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerCategory_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerCategory_IsActive", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@CustomerCategory_Code"].Value = this.m_strCode;
                cmd.Parameters["@CustomerCategory_Name"].Value = this.Name;
                cmd.Parameters["@CustomerCategory_IsActive"].Value = this.m_IsActive;
                if (this.m_strDescription == "")
                {
                    cmd.Parameters["@CustomerCategory_Description"].IsNullable = true;
                    cmd.Parameters["@CustomerCategory_Description"].Value = null;
                }
                else
                {
                    cmd.Parameters["@CustomerCategory_Description"].IsNullable = false;
                    cmd.Parameters["@CustomerCategory_Description"].Value = this.m_strDescription;
                }
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@CustomerCategory_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания категории клиента.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать категорию клиента.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditCustomerCategory]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerCategory_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerCategory_Code", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerCategory_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerCategory_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerCategory_IsActive", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@CustomerCategory_Guid"].Value = this.ID;
                cmd.Parameters["@CustomerCategory_Code"].Value = this.m_strCode;
                cmd.Parameters["@CustomerCategory_Name"].Value = this.Name;
                cmd.Parameters["@CustomerCategory_IsActive"].Value = this.m_IsActive;
                if (this.m_strDescription == "")
                {
                    cmd.Parameters["@CustomerCategory_Description"].IsNullable = true;
                    cmd.Parameters["@CustomerCategory_Description"].Value = null;
                }
                else
                {
                    cmd.Parameters["@CustomerCategory_Description"].IsNullable = false;
                    cmd.Parameters["@CustomerCategory_Description"].Value = this.m_strDescription;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения категории клиента.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства категории клиента.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteCustomerCategory]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerCategory_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@CustomerCategory_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления категории клиента.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить категорию клиента.\n\nТекст ошибки: " + f.Message, "Внимание",
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
    /// Класс "Категория клиента по компании"
    /// </summary>
    public class CCustomerCategoryCompany
    {
        #region Свойства
        /// <summary>
        /// Категория клиента
        /// </summary>
        private CCustomerCategory m_objCustomerCategory;
        /// <summary>
        /// Категория клиента
        /// </summary>
        public CCustomerCategory CustomerCategory
        {
            get { return m_objCustomerCategory; }
            set { m_objCustomerCategory = value; }
        }
        /// <summary>
        /// Компания
        /// </summary>
        private CCompany m_objCompany;
        /// <summary>
        /// Компания
        /// </summary>
        public CCompany objCompany
        {
            get { return m_objCompany; }
            set { m_objCompany = value; }
        }
        #endregion

        #region Конструктор
        public CCustomerCategoryCompany(CCustomerCategory objCustomerCategory, CCompany objCompany)
        {
            m_objCustomerCategory = objCustomerCategory;
            m_objCompany = objCompany;
        }
        #endregion

        #region Список категорий по компаниям
        /// <summary>
        /// Возвращает список категорий по компаниям для клиента с заданным идентификатором
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="uuidCustomerId">идентификатор клиента</param>
        /// <returns>список категорий по компаниям</returns>
        public static List<CCustomerCategoryCompany> GetCategoryCompanyList(UniXP.Common.CProfile objProfile, 
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidCustomerId)
        {
            List<CCustomerCategoryCompany> objList = new List<CCustomerCategoryCompany>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetCategoryForCustomer]", objProfile.GetOptionsDllDBName());
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
                        objList.Add(new CCustomerCategoryCompany( new CCustomerCategory( (System.Guid)rs["CustomerCategory_Guid"],
                            (System.String)rs["CustomerCategory_Code"], (System.String)rs["CustomerCategory_Name"],
                            (rs["CustomerCategory_Description"] == System.DBNull.Value ? "" : (System.String)rs["CustomerCategory_Description"]),
                            (System.Boolean)rs["CustomerCategory_IsActive"]), 
                            new CCompany((System.Guid)rs["Company_Guid"], (System.String)rs["Company_Name"], (System.String)rs["Company_Acronym"])));
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
                "Не удалось получить список категорий.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        #endregion

        #region Привязка клиента к категориям по компаниям
        /// <summary>
        /// Устанавливает связь клиента и категории в разрезе компаний
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="uuidCustomerId">уи клиента</param>
        /// <param name="objCategoryCompanyList">список "категория-компания"</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean SetCategoryCompanyListForCustomer(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, 
            System.Guid uuidCustomerId, List<CCustomerCategoryCompany> objCategoryCompanyList, ref System.String strErr )
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (objCategoryCompanyList == null) { return bRet; }

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
                addedCategories.Columns.Add(new System.Data.DataColumn("Company_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedCategories.Columns.Add(new System.Data.DataColumn("CustomerCategory_Guid", typeof(System.Data.SqlTypes.SqlGuid)));

                System.Data.DataRow newRow = null;
                foreach (CCustomerCategoryCompany objItem in objCategoryCompanyList)
                {
                    newRow = addedCategories.NewRow();
                    newRow["Company_Guid"] = objItem.objCompany.ID;
                    newRow["CustomerCategory_Guid"] = objItem.CustomerCategory.ID;
                    addedCategories.Rows.Add(newRow);
                }
                if (objCategoryCompanyList.Count > 0)
                {
                    addedCategories.AcceptChanges();
                }
                
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AssignCustomerCategoryCompany]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.AddWithValue("@tCategoryCompany", addedCategories);
                cmd.Parameters["@tCategoryCompany"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tCategoryCompany"].TypeName = "dbo.udt_CategoryCompany";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Customer_Guid"].Value = uuidCustomerId;
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
    #endregion


    /// <summary>
    /// Класс "Клиент"
    /// </summary>
    public class CCustomer
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
        /// Уникальный идентификатор в InterBase
        /// </summary>
        private System.Int32 m_IbID;
        /// <summary>
        /// Уникальный идентификатор в InterBase
        /// </summary>
        public System.Int32 InterBaseID
        {
            get { return m_IbID; }
            set { m_IbID = value; }
        }
        /// <summary>
        /// Код клиента
        /// </summary>
        private System.String m_strCode;
        /// <summary>
        /// Код клиента
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
        /// Уникальный Номер Плательщика
        /// 9 символов
        /// </summary>
        private System.String m_strUNP;
        /// <summary>
        /// Уникальный Номер Плательщика
        /// </summary>
        public System.String UNP
        {
            get { return m_strUNP; }
            set { m_strUNP = value; }
        }
        /// <summary>
        /// общестатистический код предприятий и организаций
        /// Длина ОКПО равна двенадцати цифровым десятичным знакам, 
        /// 2009.08.19
        /// Нона сказала, что 8-ми
        /// из которых первые семь не имеют дополнительной смысловой нагрузки; 
        /// восьмой знак представляет собой расчетное контрольное число; 
        /// девятый знак соответствует первому знаку кода территории, на которой зарегистрирован респондент, в соответствии с общегосударственным классификатором ОКРБ 003-94 "Система обозначений объектов административно-территориального деления", 
        /// а три последних знака служат для выделения обособленных подразделений юридического лица.
        /// ОКПО представляет собой номер респондента, восемь знаков которого уникальны, 
        /// сохраняются за ним на весь период его деятельности с момента его первичной постановки на учет в органах государственной статистики и до ликвидации (прекращения деятельности). После ликвидации (прекращения деятельности) респондента его ОКПО не может быть повторно присвоен другому респонденту.
        /// </summary>
        private System.String m_strOKPO;
        /// <summary>
        /// общестатистический код предприятий и организаций
        /// </summary>
        public System.String OKPO
        {
            get { return m_strOKPO; }
            set { m_strOKPO = value; }
        }
        /// <summary>
        /// код в соответствии с Общегосударственным Классификатором Юридических Лиц и Предпринимателей
        /// 9 символов
        /// </summary>
        private System.String m_strOKULP;
        /// <summary>
        /// код в соответствии с Общегосударственным Классификатором Юридических Лиц и Предпринимателей
        /// </summary>
        public System.String OKULP
        {
            get { return m_strOKULP; }
            set { m_strOKULP = value; }
        }
        /// <summary>
        /// Признак активности клиента
        /// </summary>
        private CCustomerActiveType m_objCustomerActiveType;
        /// <summary>
        /// Признак активности клиента
        /// </summary>
        public CCustomerActiveType ActiveType
        {
            get { return m_objCustomerActiveType; }
            set { m_objCustomerActiveType = value; }
        }
        /// <summary>
        /// Форма собственности
        /// </summary>
        private CStateType m_objStateType;
        /// <summary>
        /// Форма собственности
        /// </summary>
        public CStateType StateType
        {
            get { return m_objStateType; }
            set { m_objStateType = value; }
        }
        /// <summary>
        /// Торговая сеть
        /// </summary>
        private CDistributionNetwork m_objDistributionNetwork;
        /// <summary>
        /// Торговая сеть
        /// </summary>
        public CDistributionNetwork DistributionNetwork
        {
            get { return m_objDistributionNetwork; }
            set { m_objDistributionNetwork = value; }
        }
        /// <summary>
        /// Список типов клиентов
        /// </summary>
        private List<CCustomerType> m_objCustomerTypeList;
        /// <summary>
        /// Список типов клиентов
        /// </summary>
        public List<CCustomerType> CustomerTypeList
        {
            get { return m_objCustomerTypeList; }
            set { m_objCustomerTypeList = value; }
        }
        /// <summary>
        /// Список целей приобретения
        /// </summary>
        private List<CTargetBuy> m_objTargetBuyList;
        /// <summary>
        /// Список целей приобретения
        /// </summary>
        public List<CTargetBuy> TargetBuyList
        {
            get { return m_objTargetBuyList; }
            set { m_objTargetBuyList = value; }
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
        /// Список лицензий
        /// </summary>
        private List<CLicence> m_objLicenceList;
        /// <summary>
        /// Список лицензий
        /// </summary>
        public List<CLicence> LicenceList
        {
            get { return m_objLicenceList; }
            set { m_objLicenceList = value; }
        }
        /// <summary>
        /// Список расченых счетов
        /// </summary>
        private List<CAccount> m_objAccountList;
        /// <summary>
        /// Список расченых счетов
        /// </summary>
        public List<CAccount> AccountList
        {
            get { return m_objAccountList; }
            set { m_objAccountList = value; }
        }
        /// <summary>
        /// Список РТТ
        /// </summary>
        private List<CRtt> m_objRttList;
        /// <summary>
        /// Список РТТ
        /// </summary>
        public List<CRtt> RttList
        {
            get { return m_objRttList; }
            set { m_objRttList = value; }
        }
        /// <summary>
        /// Список категорий по компаниям
        /// </summary>
        private List<CCustomerCategoryCompany> m_objCategoryCompanyList;
        /// <summary>
        /// Список категорий по компаниям
        /// </summary>
        public List<CCustomerCategoryCompany> CategoryCompanyList
        {
            get { return m_objCategoryCompanyList; }
            set { m_objCategoryCompanyList = value; }
        }
        /// <summary>
        /// Список кредитных лимитов
        /// </summary>
        private List<CCreditLimit> m_objCreditLimitList;
        /// <summary>
        /// Список кредитных лимитов
        /// </summary>
        public List<CCreditLimit> CreditLimitList
        {
            get { return m_objCreditLimitList; }
            set { m_objCreditLimitList = value; }
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
        private List<CLicence> m_objLicenceForDeleteList;
        public List<CLicence> LicenceForDeleteList
        {
            get { return m_objLicenceForDeleteList; }
            set { m_objLicenceForDeleteList = value; }
        }
        private List<CContact> m_objContactForDeleteList;
        public List<CContact> ContactForDeleteList
        {
            get { return m_objContactForDeleteList; }
            set { m_objContactForDeleteList = value; }
        }
        private List<CAccount> m_objAccountForDeleteList;
        public List<CAccount> AccountForDeleteList
        {
            get { return m_objAccountForDeleteList; }
            set { m_objAccountForDeleteList = value; }
        }
        private List<CRtt> m_objRttForDeleteList;
        public List<CRtt> RttForDeleteList
        {
            get { return m_objRttForDeleteList; }
            set { m_objRttForDeleteList = value; }
        }
        private List<CEMail> m_objEMailForDeleteList;
        public List<CEMail> EMailForDeleteList
        {
            get { return m_objEMailForDeleteList; }
            set { m_objEMailForDeleteList = value; }
        }
        /// <summary>
        /// Список дочерних подразделений
        /// </summary>
        private List<CChildDepart> m_objChildDepartList;
        /// <summary>
        /// Список дочерних подразделений
        /// </summary>
        public List<CChildDepart> ChildDepartList
        {
            get { return m_objChildDepartList; }
            set { m_objChildDepartList = value; }
        }

        private const System.Int32 iCommandTimeOutForIB = 120;
        public System.String ChildDepartCode { get; set; }
        #endregion

        #region Конструктор
        public CCustomer()
        {
            m_uuidID = System.Guid.Empty;
            m_IbID = 0;
            m_strUNP = "";
            m_strOKPO = "";
            m_strOKULP = "";
            m_strCode = "";
            m_strShortName = "";
            m_strFullName = "";
            m_objAddressList = null;
            m_objContactList = null;
            m_objCustomerActiveType = null;
            m_objLicenceList = null;
            m_objAccountList = null;
            m_objPhoneList = null;
            m_objStateType = null;
            m_objEMailList = null;
            m_objAddressForDeleteList = null;
            m_objPhoneForDeleteList = null;
            m_objLicenceForDeleteList = null;
            m_objContactForDeleteList = null;
            m_objAccountForDeleteList = null;
            m_objCustomerTypeList = null;
            m_objTargetBuyList = null;
            m_objRttList = null;
            m_objRttForDeleteList = null;
            m_objEMailForDeleteList = null;
            m_objDistributionNetwork = null;
            m_objCategoryCompanyList = null;
            m_objCreditLimitList = null;
            m_objChildDepartList = null;
            ChildDepartCode = "";
        }
        public CCustomer(System.Guid uuidID, System.Int32 IbID, System.String strCode, System.String strShortName, System.String strFullName,
            System.String strUNP, System.String strOKPO, System.String strOKULP, 
            CCustomerActiveType objCustomerActiveType, CStateType objStateType)
        {
            m_uuidID = uuidID;
            m_IbID = IbID;
            m_strUNP = strUNP;
            m_strOKPO = strOKPO;
            m_strOKULP = strOKULP;
            m_strCode = strCode;
            m_strShortName = strShortName;
            m_strFullName = strFullName;
            m_objStateType = objStateType;
            m_objCustomerActiveType = objCustomerActiveType;
            m_objAddressList = null;
            m_objContactList = null;
            m_objLicenceList = null;
            m_objAccountList = null;
            m_objPhoneList = null;
            m_objEMailList = null;
            m_objAddressForDeleteList = null;
            m_objPhoneForDeleteList = null;
            m_objLicenceForDeleteList = null;
            m_objContactForDeleteList = null;
            m_objAccountForDeleteList = null;
            m_objCustomerTypeList = null;
            m_objTargetBuyList = null;
            m_objRttList = null;
            m_objRttForDeleteList = null;
            m_objEMailForDeleteList = null;
            m_objDistributionNetwork = null;
            m_objCategoryCompanyList = null;
            m_objCreditLimitList = null;
            m_objChildDepartList = null;
            ChildDepartCode = "";
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список клиентов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objDepart">Подразделение</param>
        /// <returns>список клиентов</returns>
        public static List<CCustomer> GetCustomerListWithoutAdvancedProperties(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, CDepart objDepart)
        {
            List<CCustomer> objList = new List<CCustomer>();
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
                if (objDepart == null)
                {
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetCustomerList]", objProfile.GetOptionsDllDBName());
                }
                else
                {
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCustomerListForDepart]", objProfile.GetOptionsDllDBName());
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Depart_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@Depart_Guid"].Value = objDepart.uuidID;
                }
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CCustomer((System.Guid)rs["Customer_Guid"], (System.Int32)rs["CUSTOMER_ID"], (System.String)rs["Customer_Code"],
                            (System.String)rs["CUSTOMER_NAME"], (System.String)rs["CUSTOMER_NAME"],
                            (System.String)rs["Customer_UNP"], (System.String)rs["Customer_OKPO"],
                            ((rs["Customer_OKULP"] == System.DBNull.Value) ? "" : (System.String)rs["Customer_OKULP"]),
                            // признак активности
                            new CCustomerActiveType((System.Guid)rs["CustomerActiveType_Guid"], (System.String)rs["CustomerActiveType_Name"]),
                            // форма собственности
                            new CStateType((System.Guid)rs["CustomerStateType_Guid"],
                            (System.String)rs["CustomerStateType_Name"],
                            (System.String)rs["CustomerStateType_ShortName"],
                            (System.Boolean)rs["CustomerStateType_IsActive"])
                            ));
                        objList.Last<CCustomer>().ChildDepartCode = ((rs["ChildDepart_Code"] == null) ? "" : System.Convert.ToString(rs["ChildDepart_Code"]));
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
                "Не удалось получить список клиентов.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }

        /// <summary>
        /// Возвращает список клиентов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список клиентов</returns>
        public static List<CCustomer> GetCustomerList(UniXP.Common.CProfile objProfile, 
            System.Data.SqlClient.SqlCommand cmdSQL )
        {
            List<CCustomer> objList = new List<CCustomer>();
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

                objList = GetCustomerListWithoutAdvancedProperties(objProfile, cmd, null);

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список клиентов.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Запрашивает дополнительные свойства клиента
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

                // список телефонов
                this.PhoneList = CPhone.GetPhoneListForCustomer(objProfile, cmdSQL, this.m_uuidID, ref strErr);

                // список адресов 
                this.AddressList = CAddress.GetAddressList(objProfile, cmdSQL, EnumObject.Customer, this.m_uuidID);

                // список контактов
                this.ContactList = CContact.GetContactList(objProfile, cmdSQL, EnumObject.Customer, this.m_uuidID);

                // список лицензий
                this.LicenceList = CLicence.GetLicenceList(this.m_uuidID, objProfile, cmdSQL);

                // список расчетных счетов
                this.AccountList = CAccount.GetAccountListForCustomer(objProfile, cmdSQL, this.m_uuidID, ref strErr);

                // список типов клиентов
                this.CustomerTypeList = CCustomerType.GetCustomerTypeListForCustomer(objProfile, cmdSQL, this.m_uuidID);

                // список электронных адресов
                this.EMailList = CEMail.GetEMailListForContact(objProfile, cmdSQL, EnumObject.Customer, this.m_uuidID, ref strErr);

                // список торговых точек
                this.m_objRttList = CRtt.GetRttList(objProfile, cmdSQL, this.m_uuidID);

                // торговая сеть
                this.m_objDistributionNetwork = CDistributionNetwork.GetDistributionNetworkForCustomer(objProfile, cmdSQL, this.m_uuidID, ref strErr);

                bRet = ((this.PhoneList != null) && (this.AddressList != null) && (this.ContactList != null) &&
                    (this.LicenceList != null));

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось запросить дополнительные свойства клиента.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return bRet;
        }

        /// <summary>
        /// Возвращает объект "Клиент" для заданного р/с клиента
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strAccount">р/с</param>
        /// <param name="strBankCod">код банка</param>
        /// <returns>объект Customer</returns>
        public static CCustomer GetCustomerByAccountAndBankCod(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, System.String strAccount, System.String strBankCod, ref System.String strErr)
        {
            CCustomer objCustomer = null;
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
                        return objCustomer;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCustomerByAccountAndBankCode]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Account", System.Data.SqlDbType.NVarChar));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BankCod", System.Data.SqlDbType.NVarChar));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Account"].Value = strAccount;
                cmd.Parameters["@BankCod"].Value = strBankCod;

                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    rs.Read();
                    objCustomer = new CCustomer((System.Guid)rs["Customer_Guid"], 0, "", (System.String)rs["Customer_Name"], "", "", "", "", null, null);
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
                strErr = "Не удалось получить торговую сеть для клиента.\n\nТекст ошибки: " + f.Message;
            }
            return objCustomer;
        }
        /// <summary>
        /// Ворзвращает список клиентов для дочернего подразделения
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="uuidChildCustId">уи дочернего подразделения</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>список клиентов</returns>
        public static List<CCustomer> GetCustomerListForChildDepart(UniXP.Common.CProfile objProfile, 
            System.Data.SqlClient.SqlCommand cmdSQL, 
            System.Guid uuidChildCustId, ref System.String strErr)
        {
            List<CCustomer> objList = new List<CCustomer>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetCustomerList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ChildDepart_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@ChildDepart_Guid"].Value = uuidChildCustId;

                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CCustomer((System.Guid)rs["Customer_Guid"], (System.Int32)rs["CUSTOMER_ID"], (System.String)rs["Customer_Code"],
                            (System.String)rs["CUSTOMER_NAME"], (System.String)rs["CUSTOMER_NAME"],
                            (System.String)rs["Customer_UNP"], (System.String)rs["Customer_OKPO"],
                            ((rs["Customer_OKULP"] == System.DBNull.Value) ? "" : (System.String)rs["Customer_OKULP"]),
                            // признак активности
                            new CCustomerActiveType((System.Guid)rs["CustomerActiveType_Guid"], (System.String)rs["CustomerActiveType_Name"]),
                            // форма собственности
                            new CStateType((System.Guid)rs["CustomerStateType_Guid"],
                            (System.String)rs["CustomerStateType_Name"],
                            (System.String)rs["CustomerStateType_ShortName"],
                            (System.Boolean)rs["CustomerStateType_IsActive"])
                            ));
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
                strErr = "Не удалось получить список клиентов для дочернего подразделения.\n\nТекст ошибки: " + f.Message;
            }
            return objList;
        }
        #endregion

        #region Добавить информацию о клиенте в базу данных
        /// <summary>
        /// Проверка свойств клиента перед сохранением
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
                    strErr = "Клиент: Необходимо указать название клиента!";
                    return bRet;
                }
                //if (this.UNP.Trim() == "")
                //{
                //    strErr = "Необходимо указать УНП клиента!";
                //    return bRet;
                //}
                //if (this.OKPO.Trim() == "")
                //{
                //    strErr = "Необходимо указать ОКПО клиента!";
                //    return bRet;
                //}
                //if (this.OKULP.Trim() == "")
                //{
                //    strErr = "Необходимо указать ОКЮЛП клиента!";
                //    return bRet;
                //}
                if (this.StateType == null)
                {
                    strErr = "Клиент: Необходимо указать форму собственности клиента!";
                    return bRet;
                }
                if (this.ActiveType == null)
                {
                    strErr = "Клиент: Необходимо указать признак активности клиента!";
                    return bRet;
                }
                if ((this.AddressList == null) || (this.AddressList.Count == 0))
                {
                    strErr = "Клиент: Необходимо указать адрес!";
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
                if ((this.LicenceList != null) && (this.LicenceList.Count > 0))
                {
                    System.Boolean bIsAllLicenceValid = true;
                    foreach (CLicence objItem in this.LicenceList)
                    {
                        if (objItem.IsAllParametersValid(ref strErr) == false)
                        {
                            bIsAllLicenceValid = false;
                            break;
                        }
                    }
                    if (bIsAllLicenceValid == false)
                    {
                        return bRet;
                    }
                }
                //else
                //{
                //    strErr = "Клиент: Необходимо указать лицензию клиента!";
                //    return bRet;
                //}
                if ((this.AccountList != null) && (this.AccountList.Count > 0))
                {
                    System.Boolean bIsAllAccountValid = true;
                    foreach (CAccount objItem in this.AccountList)
                    {
                        if (objItem.IsAllParametersValid(ref strErr) == false)
                        {
                            bIsAllAccountValid = false;
                            break;
                        }
                    }
                    if (bIsAllAccountValid == false)
                    {
                        return bRet;
                    }
                }
                //else
                //{
                //    strErr = "Клиент: Необходимо указать расчетный счет клиента!";
                //    return bRet;
                //}
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
                if ((this.RttList != null) && (this.RttList.Count > 0))
                {
                    System.Boolean bIsAllRttValid = true;
                    foreach (CRtt objItem in this.RttList)
                    {
                        if (objItem.IsChanged == true)
                        {
                            if (objItem.IsAllParametersValid(ref strErr) == false)
                            {
                                bIsAllRttValid = false;
                                break;
                            }
                        }
                    }
                    if (bIsAllRttValid == false)
                    {
                        return bRet;
                    }
                }

                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr = "Ошибка проверки свойств клиента. Текст ошибки: " + f.Message;
            }
            return bRet;
        }
        /// <summary>
        /// Добавляет в базу данных информацию о клиенте
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Add(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            System.Boolean bSaveInIB = false;

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
                    cmd.CommandTimeout = iCommandTimeOutForIB;
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }
                cmd.CommandTimeout = iCommandTimeOutForIB;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddCustomer]", objProfile.GetOptionsDllDBName()); ;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerStateType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerActiveType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CUSTOMER_NAME", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_ShortName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Code", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_UNP", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_OKPO", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_OKULP", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@CUSTOMER_NAME"].Value = this.FullName;
                cmd.Parameters["@Customer_ShortName"].Value = this.ShortName;
                cmd.Parameters["@Customer_Code"].Value = this.Code;
                cmd.Parameters["@Customer_UNP"].Value = this.UNP;
                cmd.Parameters["@Customer_OKPO"].Value = this.OKPO;
                cmd.Parameters["@Customer_OKULP"].Value = this.OKULP;
                cmd.Parameters["@CustomerStateType_Guid"].Value = this.StateType.ID;
                cmd.Parameters["@CustomerActiveType_Guid"].Value = this.ActiveType.ID;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    this.ID = (System.Guid)cmd.Parameters["@Customer_Guid"].Value;
                    // теперь списки контактов, адресов, лицензий
                    // возможна ситуация, когда при сохранении нового клиента произошла ошибка,
                    // при этом контакты и адреса получили идентификаторы
                    // их нужно сбросить в Empty
                    if ((this.ContactList != null) && (this.ContactList.Count > 0))
                    {
                        foreach (CContact objContact in this.ContactList) 
                        { 
                            objContact.ID = System.Guid.Empty;
                            if ((objContact.AddressList != null) && (objContact.AddressList.Count > 0))
                            {
                                foreach (CAddress objAddress in objContact.AddressList) { objAddress.ID = System.Guid.Empty; }
                            }
                            if ((objContact.EMailList != null) && (objContact.EMailList.Count > 0))
                            {
                                foreach (CEMail objEMail in objContact.EMailList) { objEMail.ID = System.Guid.Empty; }
                            }
                            if ((objContact.PhoneList != null) && (objContact.PhoneList.Count > 0))
                            {
                                foreach (CPhone objPhone in objContact.PhoneList) { objPhone.ID = System.Guid.Empty; }
                            }
                        }
                        iRes = (CContact.SaveContactList(this.ContactList, null, EnumObject.Customer, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                    }
                    if (iRes == 0)
                    {
                        if ((this.AddressList != null) && (this.AddressList.Count > 0) && ( this.AddressForDeleteList != null ))
                        {
                            foreach (CAddress objAddress in this.AddressList) { objAddress.ID = System.Guid.Empty; }
                            iRes = (CAddress.SaveAddressList(this.AddressList, null, EnumObject.Customer, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.LicenceList != null) && (this.LicenceList.Count > 0))
                        {
                            foreach (CLicence objLicence in this.LicenceList) { objLicence.ID = System.Guid.Empty; }
                            iRes = (CLicence.SaveLicenceList(this.LicenceList, null, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.PhoneList != null) && (this.PhoneList.Count > 0))
                        {
                            foreach (CPhone objPhone in this.PhoneList) { objPhone.ID = System.Guid.Empty; }
                            iRes = (CPhone.SavePhoneList(this.PhoneList, null, EnumObject.Customer, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.EMailList != null) && (this.EMailList.Count > 0))
                        {
                            foreach (CEMail objEMail in this.EMailList) { objEMail.ID = System.Guid.Empty; }
                            iRes = (CEMail.SaveEMailList(this.EMailList, null, EnumObject.Customer, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.AccountList != null) && (this.AccountList.Count > 0))
                        {
                            foreach (CAccount objAccount in this.AccountList) { objAccount.ID = System.Guid.Empty; }
                            iRes = (CAccount.SaveAccountList(this.AccountList, null, EnumObject.Customer, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if ( this.CustomerTypeList != null )
                        {
                            iRes = ( CCustomerType.SaveCustomerTypeListForCustomer( this.ID, this.CustomerTypeList, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if (this.TargetBuyList != null)
                        {
                            iRes = ( CTargetBuy.SaveTargetBuyListForCustomer( this.ID, this.TargetBuyList, objProfile, cmd, ref strErr ) == true ) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.RttList != null) && (this.RttList.Count > 0))
                        {
                            foreach (CRtt objRtt in this.RttList) 
                            { 
                                objRtt.ID = System.Guid.Empty;
                                if ((objRtt.AddressList != null) && (objRtt.AddressList.Count > 0))
                                {
                                    foreach (CAddress objAddress in objRtt.AddressList) { objAddress.ID = System.Guid.Empty; }
                                }
                                if ((objRtt.ContactList != null) && (objRtt.ContactList.Count > 0))
                                {
                                    foreach (CContact objContact in objRtt.ContactList) 
                                    { 
                                        objContact.ID = System.Guid.Empty;
                                        if ((objContact.AddressList != null) && (objContact.AddressList.Count > 0))
                                        {
                                            foreach (CAddress objAddress in objContact.AddressList) { objAddress.ID = System.Guid.Empty; }
                                        }
                                        if ((objContact.EMailList != null) && (objContact.EMailList.Count > 0))
                                        {
                                            foreach (CEMail objEMail in objContact.EMailList) { objEMail.ID = System.Guid.Empty; }
                                        }
                                        if ((objContact.PhoneList != null) && (objContact.PhoneList.Count > 0))
                                        {
                                            foreach (CPhone objPhone in objContact.PhoneList) { objPhone.ID = System.Guid.Empty; }
                                        }
                                    }
                                }
                            }
                            iRes = (CRtt.SaveRttList(this.RttList, null, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        // торговая сеть
                        if (this.m_objDistributionNetwork != null)
                        {
                            this.m_objDistributionNetwork.SaveCustomerToDistrNet(this.ID, objProfile, cmd, ref strErr);
                        }
                    }
                    
                    // теперь все это нужно записать в InterBase
                    if (iRes == 0)
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddCustomerToIB]", objProfile.GetOptionsDllDBName()); ;
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Id", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                        cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                        cmd.Parameters["@Customer_Guid"].Value = this.ID;
                        cmd.ExecuteNonQuery();
                        iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                        if (cmd.Parameters["@ERROR_MES"].Value != System.DBNull.Value)
                        {
                            strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                        }
                        else
                        {
                            if (iRes != 0)
                            {
                                strErr = "InterBase. Код ошибки: " + iRes.ToString();
                            }
                        }


                        bSaveInIB = (iRes == 0);
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
                            // нам пришлось откатить транзакцию
                            // нужно пройтись по объектам, связанным с клиентом, и обнулить значение ID у тех из них,
                            // которые являются новыми, и их описания нет в БД
                            
                            // если мы откатываем транзакцию, а запись в InterBase уже прошла, то нужно удалить в IB клиента
                            if (bSaveInIB == true)
                            {
                                DeleteCustomerFromIB(objProfile, cmd, ref strErr);
                            }

                            this.ID = System.Guid.Empty;
                            if ((this.RttList != null) && (this.RttList.Count > 0))
                            {
                                foreach (CRtt objRtt in this.RttList)
                                {
                                    if (objRtt.IsNewObject == true) 
                                    { 
                                        objRtt.ID = System.Guid.Empty;
                                        objRtt.ClearIDFromChildObject();
                                    }
                                }
                            }
                            if ((this.AddressList != null) && (this.AddressList.Count > 0))
                            {
                                foreach (CAddress objAddress in this.AddressList)
                                {
                                    if (objAddress.IsNewObject == true) { objAddress.ID = System.Guid.Empty; }
                                }
                            }
                            if ((this.ContactList != null) && (this.ContactList.Count > 0))
                            {
                                foreach (CContact objAddress in this.ContactList)
                                {
                                    if (objAddress.IsNewObject == true) 
                                    { 
                                        objAddress.ID = System.Guid.Empty;
                                        objAddress.ClearIDFromChildObject();
                                    }
                                }
                            }
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

        #region Сохранить в базе данных изменения в описании клиента
        /// <summary>
        /// Сохраняет в базе данных изменения в описании клиента
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="objContactDeletedList">список удаленных контактов</param>
        /// <param name="objAddressDeletedList">список удаленных адресов</param>
        /// <param name="objLicenceDeletedList">список удаленных лицензий</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Update( UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            ref System.String strErr )
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
                    cmd.CommandTimeout = iCommandTimeOutForIB;
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }
                cmd.CommandTimeout = iCommandTimeOutForIB;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditCustomer]", objProfile.GetOptionsDllDBName()); ;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerStateType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerActiveType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CUSTOMER_NAME", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_ShortName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Code", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_UNP", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_OKPO", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_OKULP", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Customer_Guid"].Value = this.ID;
                cmd.Parameters["@CUSTOMER_NAME"].Value = this.FullName;
                cmd.Parameters["@Customer_ShortName"].Value = this.ShortName;
                cmd.Parameters["@Customer_Code"].Value = this.Code;
                cmd.Parameters["@Customer_UNP"].Value = this.UNP;
                cmd.Parameters["@Customer_OKPO"].Value = this.OKPO;
                cmd.Parameters["@Customer_OKULP"].Value = this.OKULP;
                cmd.Parameters["@CustomerStateType_Guid"].Value = this.StateType.ID;
                cmd.Parameters["@CustomerActiveType_Guid"].Value = this.ActiveType.ID;

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
                        iRes = (CContact.SaveContactList(this.ContactList, this.ContactForDeleteList, EnumObject.Customer, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                    }
                    if (iRes == 0)
                    {
                        if ( ( this.AddressList != null ) || ( this.AddressForDeleteList != null ) )
                        {
                            iRes = (CAddress.SaveAddressList(this.AddressList, this.AddressForDeleteList, EnumObject.Customer, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.LicenceList != null) || (this.LicenceForDeleteList != null))
                        {
                            iRes = (CLicence.SaveLicenceList(this.LicenceList, this.LicenceForDeleteList, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.PhoneList != null) || (this.PhoneForDeleteList != null))
                        {
                            iRes = (CPhone.SavePhoneList(this.PhoneList, this.PhoneForDeleteList, EnumObject.Customer, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.EMailList != null) || (this.EMailForDeleteList != null))
                        {
                            iRes = (CEMail.SaveEMailList(this.EMailList, this.EMailForDeleteList, EnumObject.Customer, this.ID, objProfile, cmd, ref strErr ) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.AccountList != null) || (this.AccountForDeleteList != null))
                        {
                            iRes = (CAccount.SaveAccountList(this.AccountList, this.AccountForDeleteList, EnumObject.Customer, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if (this.CustomerTypeList != null)
                        {
                            iRes = (CCustomerType.SaveCustomerTypeListForCustomer(this.ID, this.CustomerTypeList, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if (this.TargetBuyList != null)
                        {
                            iRes = (CTargetBuy.SaveTargetBuyListForCustomer(this.ID, this.TargetBuyList, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.RttList != null) || (this.RttForDeleteList != null))
                        {
                            iRes = (CRtt.SaveRttList(this.RttList, this.RttForDeleteList, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        // торговая сеть
                        if (this.m_objDistributionNetwork != null)
                        {
                            this.m_objDistributionNetwork.SaveCustomerToDistrNet(this.ID, objProfile, cmd, ref strErr);
                        }
                        else
                        {
                            CDistributionNetwork.RemoveCustomerFromDistrNet(this.ID, objProfile, cmd, ref strErr);
                        }
                    }
                    // теперь все это нужно записать в InterBase
                    if (iRes == 0)
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditCustomerToIB]", objProfile.GetOptionsDllDBName()); ;
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                        cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                        cmd.Parameters["@Customer_Guid"].Value = this.ID;
                        cmd.ExecuteNonQuery();
                        iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                        if (cmd.Parameters["@ERROR_MES"].Value != System.DBNull.Value)
                        {
                            strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                        }
                        else
                        {
                            if (iRes != 0)
                            {
                                strErr = "InterBase. Код ошибки: " + iRes.ToString();
                            }
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
                            if ((this.RttList != null) && (this.RttList.Count > 0))
                            {
                                foreach (CRtt objRtt in this.RttList)
                                {
                                    if (objRtt.IsNewObject == true)
                                    {
                                        objRtt.ID = System.Guid.Empty;
                                        objRtt.ClearIDFromChildObject();
                                    }
                                }
                            }
                            if ((this.AddressList != null) && (this.AddressList.Count > 0))
                            {
                                foreach (CAddress objAddress in this.AddressList)
                                {
                                    if (objAddress.IsNewObject == true) { objAddress.ID = System.Guid.Empty; }
                                }
                            }
                            if ((this.ContactList != null) && (this.ContactList.Count > 0))
                            {
                                foreach (CContact objAddress in this.ContactList)
                                {
                                    if (objAddress.IsNewObject == true)
                                    {
                                        objAddress.ID = System.Guid.Empty;
                                        objAddress.ClearIDFromChildObject();
                                    }
                                }
                            }
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

        #region Удалить из базы данных описание клиента
        /// <summary>
        /// Удаляет из базы данных описание клиента
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Remove(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
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
                    cmd.CommandTimeout = iCommandTimeOutForIB;
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                //сперва удаляем в InterBase
                System.Int32 iRes = DeleteCustomerFromIB(objProfile, cmd, ref strErr);
                if (iRes == 0)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteCustomer]", objProfile.GetOptionsDllDBName());
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters["@Customer_Guid"].Value = this.ID;
                    cmd.ExecuteNonQuery();
                    iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                bRet = (iRes == 0);

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
        /// <summary>
        /// Удаляет клиента из InterBase
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>0 - удачное завершение операции; <>0 - ошибка</returns>
        private System.Int32 DeleteCustomerFromIB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            System.Int32 iRet = -1;
            try
            {
                if (cmdSQL == null)
                {
                    strErr = "Не удалось получить соединение с базой данных.";
                    return iRet;
                }
                else
                {
                    cmdSQL.Parameters.Clear();
                    cmdSQL.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteCustomerFromIB]", objProfile.GetOptionsDllDBName()); ;
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmdSQL.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                    cmdSQL.Parameters["@Customer_Guid"].Value = this.ID;
                    cmdSQL.ExecuteNonQuery();
                    iRet = (System.Int32)cmdSQL.Parameters["@RETURN_VALUE"].Value;
                    strErr = (System.String)cmdSQL.Parameters["@ERROR_MES"].Value;
                }

            }
            catch (System.Exception f)
            {
                strErr = f.Message;
            }
            finally
            {
            }
            return iRet;
        }

        #endregion

        public override string ToString()
        {
            return ShortName;
        }

    }

    public enum enumActionSaveCancel
    {
        Unkown = -1,
        Save = 0,
        Cancel = 1
    }

    } // namespace ERP_Mercury.Common
