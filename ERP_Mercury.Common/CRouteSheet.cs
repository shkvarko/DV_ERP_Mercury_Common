using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ERP_Mercury.Common
{
    #region Уникальные идентификаторы заказа
    /// <summary>
    /// Класс "Уникальные идентификаторы заказа"
    /// </summary>
    public class CSupplIdentifier
    {
        private System.Guid m_uuidSupplGuid;
        public System.Guid SupplGuid
        {
            get { return m_uuidSupplGuid; }
            set { m_uuidSupplGuid = value; }
        }
        private System.Int32 m_iSupplId;
        public System.Int32 SupplId
        {
            get { return m_iSupplId; }
            set { m_iSupplId = value; }
        }
        private System.Boolean m_bIsBlockInDeliveryList;
        public System.Boolean IsBlockInDeliveryList
        {
            get { return m_bIsBlockInDeliveryList; }
            set { m_bIsBlockInDeliveryList = value; }
        }
        public CSupplIdentifier(System.Guid uuidSupplGuid, System.Int32 iSupplId, System.Boolean bIsBlockInDeliveryList)
        {
            m_uuidSupplGuid = uuidSupplGuid;
            m_iSupplId = iSupplId;
            m_bIsBlockInDeliveryList = bIsBlockInDeliveryList;
        }
    }
    #endregion

    #region Тип тарифа перевозчика
    /// <summary>
    /// Класс "Тип тарифа перевозчика"
    /// </summary>
    public class CarrierRateType : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Признак "Активен"
        /// </summary>
        private System.Boolean m_bIsActive;
        /// <summary>
        /// Признак "Активен"
        /// </summary>
        [DisplayName("Активен")]
        [Description("Признак активности записи")]
        [Category("1. Обязательные значения")]
        public System.Boolean IsActive
        {
            get { return m_bIsActive; }
            set { m_bIsActive = value; }
        }
        private System.Double m_dblRateValue;
        [Browsable(false)]
        public System.Double RateValue
        {
            get { return m_dblRateValue; }
            set { m_dblRateValue = value; }
        }

        private CNDSRate m_objNDS;
        [Browsable(false)]
        public CNDSRate NDS
        {
            get { return m_objNDS; }
            set { m_objNDS = value; }
        }

        #endregion

        #region Конструктор
        public CarrierRateType()
            : base()
        {
            m_bIsActive = false;
            m_dblRateValue = 0;
            m_objNDS = null;
        }
        public CarrierRateType(System.Guid uuidId, System.String strName, System.Boolean bIsActive)
        {
            ID = uuidId;
            Name = strName;
            m_bIsActive = bIsActive;
            m_dblRateValue = 0;
            m_objNDS = null;
        }
        public CarrierRateType(System.Guid uuidId, System.String strName, System.Boolean bIsActive, System.Double dblRateValue)
        {
            ID = uuidId;
            Name = strName;
            m_bIsActive = bIsActive;
            m_dblRateValue = dblRateValue;
            m_objNDS = null;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список типов тарифов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список типов тарифов</returns>
        public static List<CarrierRateType> GetCarrierRateTypeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CarrierRateType> objList = new List<CarrierRateType>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCarrierRateType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CarrierRateType(
                            (System.Guid)rs["CarrierRateType_Guid"],
                            (System.String)rs["CarrierRateType_Name"],
                            System.Convert.ToBoolean(rs["CarrierRateType_IsActive"])
                            )
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
                "Не удалось получить список типов тарифов.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Возвращает список типов тарифов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objCarrier">перевозчик</param>
        /// <returns>список типов тарифов</returns>
        public static List<CarrierRateType> GetCarrierRateTypeListForCarrier(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, CCarrier objCarrier)
        {
            List<CarrierRateType> objList = new List<CarrierRateType>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCarrierRateType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Carrier_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Carrier_Guid"].Value = objCarrier.ID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CarrierRateType(
                            (System.Guid)rs["CarrierRateType_Guid"],
                            (System.String)rs["CarrierRateType_Name"],
                            System.Convert.ToBoolean(rs["CarrierRateType_IsActive"]),
                            System.Convert.ToDouble(rs["CarrierRateType_Value"])
                            )
                            {
                                NDS = new CNDSRate()
                                {
                                    ID = (System.Guid)rs["NDSRate_Guid"],
                                    Name = System.Convert.ToString(rs["NDSRate_Name"]),
                                    RateValue = System.Convert.ToDouble(rs["NDSRate_Value"]),
                                    IsActive = System.Convert.ToBoolean(rs["NDSRate_IsActive"])
                                }
                            }
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
                "Не удалось получить список типов тарифов.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddCarrierRateType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CarrierRateType_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CarrierRateType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CarrierRateType_IsActive", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@CarrierRateType_Name"].Value = this.Name;
                cmd.Parameters["@CarrierRateType_IsActive"].Value = this.IsActive;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@CarrierRateType_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания типа договора с клиентом.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать тип тарифа.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditCarrierRateType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CarrierRateType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CarrierRateType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CarrierRateType_IsActive", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@CarrierRateType_Guid"].Value = this.ID;
                cmd.Parameters["@CarrierRateType_Name"].Value = this.Name;
                cmd.Parameters["@CarrierRateType_IsActive"].Value = this.IsActive;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств типа тарифа.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства типа тарифа.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteCarrierRateType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CarrierRateType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@CarrierRateType_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления описания типа тарифа.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить описание типа тарифа.\n\nТекст ошибки: " + f.Message, "Внимание",
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

    #region Тип дополнительных расходов
    /// <summary>
    /// Класс "Тип дополнительных расходов"
    /// </summary>
    public class CRouteSheetAdvancedExpenseType : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Признак "Активен"
        /// </summary>
        private System.Boolean m_bIsActive;
        /// <summary>
        /// Признак "Активен"
        /// </summary>
        [DisplayName("Активен")]
        [Description("Признак активности записи")]
        [Category("1. Обязательные значения")]
        public System.Boolean IsActive
        {
            get { return m_bIsActive; }
            set { m_bIsActive = value; }
        }
        #endregion

        #region Конструктор
        public CRouteSheetAdvancedExpenseType()
            : base()
        {
            m_bIsActive = false;
        }
        public CRouteSheetAdvancedExpenseType(System.Guid uuidId, System.String strName, System.Boolean bIsActive)
        {
            ID = uuidId;
            Name = strName;
            m_bIsActive = bIsActive;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список типов дополнительных расходов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список типов дополнительных расходов</returns>
        public static List<CRouteSheetAdvancedExpenseType> GeAdvancedExpenseTypeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CRouteSheetAdvancedExpenseType> objList = new List<CRouteSheetAdvancedExpenseType>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetRouteSheetAdvancedExpenseType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CRouteSheetAdvancedExpenseType(
                            (System.Guid)rs["RouteSheetAdvancedExpenseType_Guid"],
                            (System.String)rs["RouteSheetAdvancedExpenseType_Name"],
                            System.Convert.ToBoolean(rs["RouteSheetAdvancedExpenseType_IsActive"])
                            )
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
                "Не удалось получить список типов дополнительных расходов.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                    DevExpress.XtraEditors.XtraMessageBox.Show( "Необходимо указать название!", "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddRouteSheetAdvancedExpenseType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheetAdvancedExpenseType_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheetAdvancedExpenseType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheetAdvancedExpenseType_IsActive", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RouteSheetAdvancedExpenseType_Name"].Value = this.Name;
                cmd.Parameters["@RouteSheetAdvancedExpenseType_IsActive"].Value = this.IsActive;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@RouteSheetAdvancedExpenseType_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания типа дополнительных расходов.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать тип тарифа.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditRouteSheetAdvancedExpenseType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheetAdvancedExpenseType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheetAdvancedExpenseType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheetAdvancedExpenseType_IsActive", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RouteSheetAdvancedExpenseType_Guid"].Value = this.ID;
                cmd.Parameters["@RouteSheetAdvancedExpenseType_Name"].Value = this.Name;
                cmd.Parameters["@RouteSheetAdvancedExpenseType_IsActive"].Value = this.IsActive;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств типа дополнительных расходов.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства типа дополнительных расходов.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteRouteSheetAdvancedExpenseType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheetAdvancedExpenseType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RouteSheetAdvancedExpenseType_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления описания типа дополнительных расходов.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить описание типа дополнительных расходов.\n\nТекст ошибки: " + f.Message, "Внимание",
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

    #region Дополнительные расходы
    /// <summary>
    /// Класс "Дополнительные расходы"
    /// </summary>
    public class CAdvancedExpense
    {
        #region Свойства
        /// <summary>
        /// Тип доп расходов
        /// </summary>
        private CRouteSheetAdvancedExpenseType m_objAdvancedExpenseType;
        /// <summary>
        /// Тип доп расходов
        /// </summary>
        public CRouteSheetAdvancedExpenseType AdvancedExpenseType
        {
            get { return m_objAdvancedExpenseType; }
            set { m_objAdvancedExpenseType = value; }
        }
        /// <summary>
        /// № п/п
        /// </summary>
        private System.Int32 m_iID;
        /// <summary>
        /// № п/п
        /// </summary>
        public System.Int32 ID
        {
            get { return m_iID; }
            set { m_iID = value; }
        }
        /// <summary>
        /// Сумма
        /// </summary>
        private System.Decimal m_AllMoney;
        /// <summary>
        /// Сумма
        /// </summary>
        public System.Decimal AllMoney
        {
            get { return m_AllMoney; }
            set { m_AllMoney = value; }
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
        public System.String AdvancedExpenseTypeName
        {
            get { return ((m_objAdvancedExpenseType == null) ? "" : m_objAdvancedExpenseType.Name); }
        }
        #endregion

        #region Конструктор
        public CAdvancedExpense()
        {
            m_iID = 0;
            m_AllMoney = 0;
            m_strDescription = "";
            m_objAdvancedExpenseType = null;
        }
        public CAdvancedExpense( CRouteSheetAdvancedExpenseType objAdvancedExpenseType, System.Int32 iID, System.Decimal mAllMoney, 
            System.String strDescription)
        {
            m_iID = iID;
            m_AllMoney = mAllMoney;
            m_strDescription = strDescription;
            m_objAdvancedExpenseType = objAdvancedExpenseType;
        }
        #endregion

        #region Список доп расходов
        /// <summary>
        /// Возвращает доп расходов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objRouteSheet">путевой лист</param>
        /// <returns>список доп расходов</returns>
        public static List<CAdvancedExpense> GetAdvancedExpenseListForCarrier(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, CRouteSheet objRouteSheet)
        {
            List<CAdvancedExpense> objList = new List<CAdvancedExpense>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetRouteSheetAdvancedExpense]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheet_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RouteSheet_Guid"].Value = objRouteSheet.ID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CAdvancedExpense( new CRouteSheetAdvancedExpenseType(
                            (System.Guid)rs["RouteSheetAdvancedExpenseType_Guid"],
                            (System.String)rs["RouteSheetAdvancedExpenseType_Name"],
                            System.Convert.ToBoolean(rs["RouteSheetAdvancedExpenseType_IsActive"])
                            ), System.Convert.ToInt32( rs["RouteSheetAdvancedExpense_Id"] ), 
                            System.Convert.ToDecimal( rs["RouteSheetAdvancedExpense_AllMoney"] ), 
                            ( ( rs["RouteSheetAdvancedExpense_Description"] == System.DBNull.Value ) ? "" : System.Convert.ToString(rs["RouteSheetAdvancedExpense_Description"]) )
                            )
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
                "Не удалось получить список доп расходов.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        #endregion
    }
    #endregion

    #region Водитель
    /// <summary>
    /// Класс "Водитель"
    /// </summary>
    public class CDriver : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Фамилия
        /// </summary>
        private System.String m_strLastName;
        /// <summary>
        /// Фамилия
        /// </summary>
        [DisplayName("Фамилия")]
        [Description("Фамилия")]
        [Category("1. Обязательные значения")]
        public System.String LastName
        {
            get { return m_strLastName; }
            set { m_strLastName = value; }
        }
        /// <summary>
        /// Отчество
        /// </summary>
        private System.String m_strMiddleName;
        /// <summary>
        /// Отчество
        /// </summary>
        [DisplayName("Отчество")]
        [Description("Отчество")]
        [Category("1. Обязательные значения")]
        public System.String MiddleName
        {
            get { return m_strMiddleName; }
            set { m_strMiddleName = value; }
        }
        /// <summary>
        /// Признак "Активен"
        /// </summary>
        private System.Boolean m_bIsActive;
        /// <summary>
        /// Признак "Активен"
        /// </summary>
        [DisplayName("Активен")]
        [Description("Признак активности записи")]
        [Category("1. Обязательные значения")]
        public System.Boolean IsActive
        {
            get { return m_bIsActive; }
            set { m_bIsActive = value; }
        }
        private System.Boolean m_bChecked;

        [Browsable(false)]
        public System.Boolean Checked
        {
            get { return m_bChecked; }
            set { m_bChecked = value; }
        }
        #endregion

        #region Конструктор
        public CDriver()
            : base()
        {
            m_bIsActive = false;
            m_strLastName = "";
            m_strMiddleName = "";
            Checked = false;
        }
        public CDriver(System.Guid uuidId, System.String strName, System.String strLastName,
            System.String strMiddleName, System.Boolean bIsActive)
        {
            ID = uuidId;
            Name = strName;
            m_bIsActive = bIsActive;
            m_strLastName = strLastName;
            m_strMiddleName = strMiddleName;
            Checked = false;

        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список водителей
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список водителей</returns>
        public static List<CDriver> GetDriverList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CDriver> objList = new List<CDriver>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetDriver]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CDriver((System.Guid)rs["Driver_Guid"],
                            (System.String)rs["Driver_FirstName"], (System.String)rs["Driver_LastName"],
                            (System.String)rs["Driver_MiddleName"], System.Convert.ToBoolean(rs["Driver_IsActive"])
                            )
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
                "Не удалось получить список водителей.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Возвращает список водителей
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objCarrier">перевозчик</param>
        /// <returns>список водителей</returns>
        public static List<CDriver> GetDriverListForCarrier(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, CCarrier objCarrier)
        {
            List<CDriver> objList = new List<CDriver>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetDriver]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Carrier_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Carrier_Guid"].Value = objCarrier.ID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CDriver((System.Guid)rs["Driver_Guid"],
                            (System.String)rs["Driver_FirstName"], (System.String)rs["Driver_LastName"],
                            (System.String)rs["Driver_MiddleName"], System.Convert.ToBoolean(rs["Driver_IsActive"])
                            )
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
                "Не удалось получить список водителей.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                if (this.MiddleName == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать отчество!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.LastName == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать фамилию!", "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddDriver]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Driver_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Driver_LastName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Driver_FirstName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Driver_MiddleName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Driver_IsActive", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Driver_LastName"].Value = this.m_strLastName;
                cmd.Parameters["@Driver_FirstName"].Value = this.Name;
                cmd.Parameters["@Driver_MiddleName"].Value = this.m_strMiddleName;
                cmd.Parameters["@Driver_IsActive"].Value = this.IsActive;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@Driver_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания записи о водителе.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать запись о водителе.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditDriver]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Driver_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Driver_LastName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Driver_FirstName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Driver_MiddleName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Driver_IsActive", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Driver_Guid"].Value = this.ID;
                cmd.Parameters["@Driver_LastName"].Value = this.m_strLastName;
                cmd.Parameters["@Driver_FirstName"].Value = this.Name;
                cmd.Parameters["@Driver_MiddleName"].Value = this.m_strMiddleName;
                cmd.Parameters["@Driver_IsActive"].Value = this.IsActive;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств водителя.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства водителя.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteDriver]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Driver_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Driver_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления описания водителя.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить описание водителя.\n\nТекст ошибки: " + f.Message, "Внимание",
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
            return GetFIO();
        }
        /// <summary>
        /// Возвращает ФИО
        /// </summary>
        /// <returns></returns>
        public System.String GetFIO()
        {
            return (LastName + " " + Name + " " + MiddleName);
        }
    }


    /// <summary>
    /// TypeConverter для списка водителей
    /// </summary>
    class DriverTypeConverter : TypeConverter
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

            CVehicle objVehicle = (CVehicle)context.Instance;
            System.Collections.Generic.List<CDriver> objList = objVehicle.GetAllDriverList();

            return new StandardValuesCollection(objList);
        }
    }
    #endregion

    #region Автомобиль
    /// <summary>
    /// Класс "Автомобиль"
    /// </summary>
    public class CVehicle : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// гос. номер
        /// </summary>
        private System.String m_strGosNumber;
        /// <summary>
        /// гос. номер
        /// </summary>
        [DisplayName("гос. номер")]
        [Description("гос. номер")]
        [Category("1. Обязательные значения")]
        public System.String GosNumber
        {
            get { return m_strGosNumber; }
            set { m_strGosNumber = value; }
        }
        /// <summary>
        /// Владелец транспорта
        /// </summary>
        private System.String m_strOwner;
        /// <summary>
        /// Владелец транспорта
        /// </summary>
        [DisplayName("Владелец")]
        [Description("Владелец транспорта")]
        [Category("1. Обязательные значения")]
        public System.String Owner
        {
            get { return m_strOwner; }
            set { m_strOwner = value; }
        }
        /// <summary>
        /// Признак "Грузоподъёмность"
        /// </summary>
        private System.Double m_Capacity;
        /// <summary>
        /// Признак "Грузоподъёмность"
        /// </summary>
        [DisplayName("Грузоподъёмность, кг.")]
        [Description("Грузоподъёмность, кг.")]
        [Category("1. Обязательные значения")]
        public System.Double Capacity
        {
            get { return m_Capacity; }
            set { m_Capacity = value; }
        }
        /// <summary>
        /// Признак "Объём"
        /// </summary>
        private System.Double m_Volume;
        /// <summary>
        /// Признак "Объём"
        /// </summary>
        [DisplayName("Объём, м3.")]
        [Description("Объём, м3")]
        [Category("1. Обязательные значения")]
        public System.Double Volume
        {
            get { return m_Volume; }
            set { m_Volume = value; }
        }
        /// <summary>
        /// Водитель
        /// </summary>
        private CDriver m_objDriver;
        /// <summary>
        /// Водитель
        /// </summary>
        [DisplayName("Водитель")]
        [Description("Водитель, работающий на этой машине")]
        [Category("1. Обязательные значения")]
        [TypeConverter(typeof(DriverTypeConverter))]
        [ReadOnly(false)]
        [BrowsableAttribute(false)]
        public CDriver Driver
        {
            get { return m_objDriver; }
            set { m_objDriver = value; }
        }
        /// <summary>
        /// Водитель
        /// </summary>
        [DisplayName("Водитель")]
        [Description("Водитель, работающий на этой машине")]
        [Category("1. Обязательные значения")]
        [TypeConverter(typeof(DriverTypeConverter))]
        public System.String DriverName
        {
            get { return m_objDriver.GetFIO(); }
            set { SetDriverValue(value); }
        }
        private void SetDriverValue(System.String strDriverName)
        {
            try
            {
                if (m_objAllDriverList == null) { m_objDriver = null; }
                else
                {
                    foreach (CDriver objDriver in m_objAllDriverList)
                    {
                        if (objDriver.GetFIO() == strDriverName)
                        {
                            m_objDriver = objDriver;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось установить значение водителя.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }
        private List<CDriver> m_objAllDriverList;
        private System.Boolean m_bChecked;
        [Browsable(false)]
        public System.Boolean Checked
        {
            get { return m_bChecked; }
            set { m_bChecked = value; }
        }
        [BrowsableAttribute(false)]
        public System.String FullName
        {
            get { return (Name + " " + GosNumber); }
        }

        #endregion

        #region Конструктор
        public CVehicle()
            : base()
        {
            m_Capacity = 0;
            m_Volume = 0;
            m_strOwner = "";
            m_strGosNumber = "";
            m_objDriver = null;
            m_objAllDriverList = null;
            Checked = false;
        }
        public CVehicle(System.Guid uuidId, System.String strName, System.String strOwner,
            System.String strGosNumber, System.Double dCapacity, System.Double dVolume, CDriver objDriver)
        {
            ID = uuidId;
            Name = strName;
            m_Capacity = dCapacity;
            m_Volume = dVolume;
            m_strOwner = strOwner;
            m_strGosNumber = strGosNumber;
            m_objDriver = objDriver;
            m_objAllDriverList = null;
            Checked = false;
        }
        #endregion

        #region Список объектов
        public List<CDriver> GetAllDriverList()
        {
            return m_objAllDriverList;
        }
        public static List<CVehicle> GetVehicleList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CVehicle> objList = new List<CVehicle>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetVehicle]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CVehicle((System.Guid)rs["Vehicle_Guid"], (System.String)rs["Vehicle_Mark"],
                            (System.String)rs["Vehicle_Owner"], (System.String)rs["Vehicle_Number"],
                            System.Convert.ToDouble(rs["Vehicle_Capacity"]), System.Convert.ToDouble(rs["Vehicle_Volume"]),
                            new CDriver((System.Guid)rs["Driver_Guid"],
                            (System.String)rs["Driver_FirstName"], (System.String)rs["Driver_LastName"],
                            (System.String)rs["Driver_MiddleName"], System.Convert.ToBoolean(rs["Driver_IsActive"])
                            )
                        )
                        );
                    }
                }
                rs.Dispose();

                List<CDriver> objDriver = CDriver.GetDriverList(objProfile, cmd);
                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
                if (objDriver != null)
                {
                    foreach (CVehicle objVehicle in objList)
                    {
                        objVehicle.m_objAllDriverList = objDriver;
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список автомобилей.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Возвращает список автомобилей
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objCarrier">перевозчик</param>
        /// <returns>список автомобилей</returns>
        public static List<CVehicle> GetVehicleListForCarrier(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, CCarrier objCarrier )
        {
            List<CVehicle> objList = new List<CVehicle>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetVehicle]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Carrier_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Carrier_Guid"].Value = objCarrier.ID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CVehicle((System.Guid)rs["Vehicle_Guid"], (System.String)rs["Vehicle_Mark"],
                            (System.String)rs["Vehicle_Owner"], (System.String)rs["Vehicle_Number"],
                            System.Convert.ToDouble(rs["Vehicle_Capacity"]), System.Convert.ToDouble(rs["Vehicle_Volume"]),
                            new CDriver((System.Guid)rs["Driver_Guid"],
                            (System.String)rs["Driver_FirstName"], (System.String)rs["Driver_LastName"],
                            (System.String)rs["Driver_MiddleName"], System.Convert.ToBoolean(rs["Driver_IsActive"])
                            )
                        )
                        );
                    }
                }
                rs.Dispose();

                List<CDriver> objDriver = CDriver.GetDriverList(objProfile, cmd);
                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
                if (objDriver != null)
                {
                    foreach (CVehicle objVehicle in objList)
                    {
                        objVehicle.m_objAllDriverList = objDriver;
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список автомобилей.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Загружает в m_objAllDriverList список водителей
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        public void InitDriverList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            try
            {
                this.m_objAllDriverList = null;
                this.m_objAllDriverList = CDriver.GetDriverList(objProfile, cmdSQL);
                if ((this.m_objAllDriverList != null) && (this.m_objAllDriverList.Count > 0))
                {
                    this.Driver = this.m_objAllDriverList[0];
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось загрузить список водителей.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
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
                if (this.m_strGosNumber == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать гос. номер!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.m_strOwner == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать владельца!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.m_Capacity <= 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать грузоподъёмность!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.m_Volume < 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать объём!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.m_objDriver == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать водителя!", "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddVehicle]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vehicle_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vehicle_Number", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vehicle_Mark", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vehicle_Owner", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vehicle_Capacity", System.Data.SqlDbType.Decimal));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vehicle_Volume", System.Data.SqlDbType.Decimal));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Driver_Guid", System.Data.SqlDbType.UniqueIdentifier));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Vehicle_Number"].Value = this.GosNumber;
                cmd.Parameters["@Vehicle_Mark"].Value = this.Name;
                cmd.Parameters["@Vehicle_Owner"].Value = this.Owner;
                cmd.Parameters["@Vehicle_Capacity"].Value = this.Capacity;
                cmd.Parameters["@Vehicle_Volume"].Value = this.Volume;
                cmd.Parameters["@Driver_Guid"].Value = this.Driver.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@Vehicle_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания записи об автомобиле.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать запись об автомобиле.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditVehicle]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vehicle_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vehicle_Number", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vehicle_Mark", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vehicle_Owner", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vehicle_Capacity", System.Data.SqlDbType.Decimal));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vehicle_Volume", System.Data.SqlDbType.Decimal));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Driver_Guid", System.Data.SqlDbType.UniqueIdentifier));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Vehicle_Guid"].Value = this.ID;
                cmd.Parameters["@Vehicle_Number"].Value = this.GosNumber;
                cmd.Parameters["@Vehicle_Mark"].Value = this.Name;
                cmd.Parameters["@Vehicle_Owner"].Value = this.Owner;
                cmd.Parameters["@Vehicle_Capacity"].Value = this.Capacity;
                cmd.Parameters["@Vehicle_Volume"].Value = this.Volume;
                cmd.Parameters["@Driver_Guid"].Value = this.Driver.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств записи об автомобиле.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства записи об автомобиле.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteVehicle]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vehicle_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Vehicle_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления описания автомобиля.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить описание автомобиля.\n\nТекст ошибки: " + f.Message, "Внимание",
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
            return (Name + " " + GosNumber);
        }
    }
    #endregion

    #region Перевозчик
    /// <summary>
    /// Класс "Перевозчик"
    /// </summary>
    public class CCarrier : CBusinessObject
    {
        #region Свойства
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
        /// Список водителей
        /// </summary>
        private List<CDriver> m_objDriverList;
        /// <summary>
        /// Список водителей
        /// </summary>
        public List<CDriver> DriverList
        {
            get { return m_objDriverList; }
            set { m_objDriverList = value; }
        }
        /// <summary>
        /// Список автомобилей
        /// </summary>
        private List<CVehicle> m_objVehicleList;
        /// <summary>
        /// Список автомобилей
        /// </summary>
        public List<CVehicle> VehicleList
        {
            get { return m_objVehicleList; }
            set { m_objVehicleList = value; }
        }
        /// <summary>
        /// Список тарифов
        /// </summary>
        private List<CarrierRateType> m_objCarrierRateTypeList;
        /// <summary>
        /// Список тарифов
        /// </summary>
        public List<CarrierRateType> CarrierRateTypeList
        {
            get { return m_objCarrierRateTypeList; }
            set { m_objCarrierRateTypeList = value; }
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
        /// Уникальный Номер Плательщика
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
        /// Тип тарифа по умолчанию
        /// </summary>
        private CarrierRateType m_objCarrierRateType;
        /// <summary>
        /// Тип тарифа по умолчанию
        /// </summary>
        public CarrierRateType CarrierRateType
        {
            get { return m_objCarrierRateType; }
            set { m_objCarrierRateType = value; }
        }
        /// <summary>
        /// Ставка тарифа по умолчанию
        /// </summary>
        private System.Decimal m_RateTypeValue;
        /// <summary>
        /// Ставка тарифа по умолчанию
        /// </summary>
        public System.Decimal RateTypeValue
        {
            get { return m_RateTypeValue; }
            set { m_RateTypeValue = value; }
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
        private List<CAccount> m_objAccountForDeleteList;
        public List<CAccount> AccountForDeleteList
        {
            get { return m_objAccountForDeleteList; }
            set { m_objAccountForDeleteList = value; }
        }
        private List<CAddress> m_objAddressForDeleteList;
        public List<CAddress> AddressForDeleteList
        {
            get { return m_objAddressForDeleteList; }
            set { m_objAddressForDeleteList = value; }
        }



        #endregion

        #region Конструктор
        public CCarrier()
            : base()
        {
            m_bIsActive = false;
            m_objDriverList = null;
            m_objVehicleList = null;
            m_objStateType = null;
            m_objCarrierRateType = null;
            m_objCarrierRateTypeList = null;
            m_RateTypeValue = 0;
            m_strUNP = "";
            m_objAccountForDeleteList = null;
            m_objAddressForDeleteList = null;
        }
        public CCarrier(System.Guid uuidId, System.String strName, CStateType objStateType, System.Boolean bIsActive,
            CarrierRateType objCarrierRateType, System.Decimal dcmlRateTypeValue, System.String strUNP)
        {
            ID = uuidId;
            Name = strName;
            m_bIsActive = bIsActive;
            m_objDriverList = null;
            m_objVehicleList = null;
            m_objCarrierRateTypeList = null;
            m_objStateType = objStateType;
            m_objCarrierRateType = objCarrierRateType;
            m_RateTypeValue = dcmlRateTypeValue;
            m_strUNP = strUNP;
            m_objAccountForDeleteList = null;
            m_objAddressForDeleteList = null;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список перевозчиков
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список перевозчиков</returns>
        public static List<CCarrier> GetCarrierList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CCarrier> objList = new List<CCarrier>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCarrier]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CCarrier(
                            (System.Guid)rs["Carrier_Guid"],
                            (System.String)rs["Carrier_Name"],
                            new CStateType((System.Guid)rs["CustomerStateType_Guid"], (System.String)rs["CustomerStateType_Name"],
                               (System.String)rs["CustomerStateType_ShortName"], System.Convert.ToBoolean(rs["CustomerStateType_IsActive"])),
                            System.Convert.ToBoolean(rs["Carrier_IsActive"]),
                            new CarrierRateType((System.Guid)rs["CarrierRateType_Guid"], (System.String)rs["CarrierRateType_Name"], System.Convert.ToBoolean(rs["CarrierRateType_IsActive"])),
                            System.Convert.ToDecimal(rs["CarrierRateType_Value"]),
                            ((rs["Carrier_UNP"] == System.DBNull.Value) ? "" : (System.String)rs["Carrier_UNP"])
                            )
                            );
                    }
                }
                rs.Close();
                rs.Dispose();
                System.String strErr = "";

                if ((objList != null) && (objList.Count > 0))
                {
                    foreach (CCarrier objCarrier in objList)
                    {
                        objCarrier.DriverList = CDriver.GetDriverListForCarrier(objProfile, cmd, objCarrier);
                        objCarrier.VehicleList = CVehicle.GetVehicleListForCarrier(objProfile, cmd, objCarrier);
                        objCarrier.CarrierRateTypeList = CarrierRateType.GetCarrierRateTypeListForCarrier(objProfile, cmd, objCarrier);
                        objCarrier.AccountList = CAccount.GetAccountListForCarrier(objProfile, cmd, objCarrier.ID, ref strErr);
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
                "Не удалось получить список перевозчиков.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                if (this.StateType == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать форму собственности!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.CarrierRateType == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать тип тарифа по умолчанию (руб/км, руб/час).", "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddCarrier]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Carrier_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Carrier_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Carrier_UNP", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerStateType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Carrier_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CarrierRateType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CarrierRateType_Value", System.Data.SqlDbType.Money));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Carrier_Name"].Value = this.Name;
                cmd.Parameters["@CustomerStateType_Guid"].Value = this.StateType.ID;
                cmd.Parameters["@Carrier_IsActive"].Value = this.IsActive;
                cmd.Parameters["@CarrierRateType_Guid"].Value = this.CarrierRateType.ID;
                cmd.Parameters["@CarrierRateType_Value"].Value = this.RateTypeValue;
                cmd.Parameters["@Carrier_UNP"].Value = this.UNP;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@Carrier_Guid"].Value;

                    if ((this.DriverList != null) && (this.DriverList.Count > 0))
                    {
                        iRes = ((SaveDriverListForCarrier(objProfile, cmd, this.DriverList, this, ref strErr) == true) ? 0 : -1);
                    }

                    if (iRes == 0)
                    {
                        if ((this.VehicleList != null) && (this.VehicleList.Count > 0))
                        {
                            iRes = ((SaveVehicleListForCarrier(objProfile, cmd, this.VehicleList, this, ref strErr) == true) ? 0 : -1);
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.CarrierRateTypeList != null) && (this.CarrierRateTypeList.Count > 0))
                        {
                            iRes = ((SaveCarrierRateTypeForCarrier(objProfile, cmd, this.CarrierRateTypeList, this, ref strErr) == true) ? 0 : -1);
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.AddressList != null) && (this.AddressList.Count > 0) && (this.AddressForDeleteList != null))
                        {
                            foreach (CAddress objAddress in this.AddressList) { objAddress.ID = System.Guid.Empty; }
                            iRes = (CAddress.SaveAddressList(this.AddressList, null, EnumObject.Carrier, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.AccountList != null) && (this.AccountList.Count > 0))
                        {
                            foreach (CAccount objAccount in this.AccountList) { objAccount.ID = System.Guid.Empty; }
                            iRes = (CAccount.SaveAccountList(this.AccountList, this.AccountForDeleteList, EnumObject.Carrier, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        // подтверждаем транзакцию
                        DBTransaction.Commit();
                    }
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания записи о перевозчике.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать запись о перевозчике.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        /// <summary>
        /// Сохраняет в БД информацию о списке водителей у перевозчика
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objDriverList">список водителей</param>
        /// <param name="objCarrier">перевозчик</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean SaveDriverListForCarrier(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            List<CDriver> objDriverList, CCarrier objCarrier, ref System.String strErr)
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
                System.Data.DataTable addedCategories = new System.Data.DataTable();
                addedCategories.Columns.Add(new System.Data.DataColumn("Carrier_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedCategories.Columns.Add(new System.Data.DataColumn("Driver_Guid", typeof(System.Data.SqlTypes.SqlGuid)));

                System.Data.DataRow newRow = null;
                if ((objDriverList == null) || (objDriverList.Count == 0))
                {
                    newRow = addedCategories.NewRow();
                    newRow["Carrier_Guid"] = objCarrier.ID;
                    newRow["Driver_Guid"] = System.DBNull.Value;
                    addedCategories.Rows.Add(newRow);
                }
                else
                {
                    foreach (CDriver objItem in objDriverList)
                    {
                        newRow = addedCategories.NewRow();
                        newRow["Carrier_Guid"] = objCarrier.ID;
                        newRow["Driver_Guid"] = objItem.ID;
                        addedCategories.Rows.Add(newRow);
                    }
                }
                addedCategories.AcceptChanges();

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AssignCarrierWithDriver]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.AddWithValue("@tCarrierDriver", addedCategories);
                cmd.Parameters["@tCarrierDriver"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tCarrierDriver"].TypeName = "dbo.udt_CarrierDriver";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
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
        /// Сохраняет в БД информацию о списке тарифов у перевозчика
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objCarrierRateTypeList">список тарифов</param>
        /// <param name="objCarrier">перевозчик</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean SaveCarrierRateTypeForCarrier(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            List<CarrierRateType> objCarrierRateTypeList, CCarrier objCarrier, ref System.String strErr)
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
                System.Data.DataTable addedCategories = new System.Data.DataTable();
                addedCategories.Columns.Add(new System.Data.DataColumn("Carrier_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedCategories.Columns.Add(new System.Data.DataColumn("CarrierRateType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedCategories.Columns.Add(new System.Data.DataColumn("CarrierRateType_Value", typeof(System.Data.SqlTypes.SqlDouble)));
                addedCategories.Columns.Add(new System.Data.DataColumn("NDSRate_Guid", typeof(System.Data.SqlTypes.SqlGuid)));

                System.Data.DataRow newRow = null;
                if ((objCarrierRateTypeList == null) || (objCarrierRateTypeList.Count == 0))
                {
                    newRow = addedCategories.NewRow();
                    newRow["Carrier_Guid"] = objCarrier.ID;
                    newRow["CarrierRateType_Guid"] = System.DBNull.Value;
                    newRow["NDSRate_Guid"] = System.DBNull.Value;
                    addedCategories.Rows.Add(newRow);
                }
                else
                {
                    foreach (CarrierRateType objItem in objCarrierRateTypeList)
                    {
                        newRow = addedCategories.NewRow();
                        newRow["Carrier_Guid"] = objCarrier.ID;
                        newRow["CarrierRateType_Guid"] = objItem.ID;
                        newRow["CarrierRateType_Value"] = objItem.RateValue;
                        newRow["NDSRate_Guid"] = objItem.NDS.ID;
                        addedCategories.Rows.Add(newRow);
                    }
                }
                addedCategories.AcceptChanges();

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AssignCarrierWithRate]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.AddWithValue("@tCarrierRate", addedCategories);
                cmd.Parameters["@tCarrierRate"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tCarrierRate"].TypeName = "dbo.udt_CarrierCarrierRateType";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
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
        /// Сохраняет в БД информацию о списке автомобилей у перевозчика
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objVehicleList">список автомобилей</param>
        /// <param name="objCarrier">перевозчик</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean SaveVehicleListForCarrier(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            List<CVehicle> objVehicleList, CCarrier objCarrier, ref System.String strErr)
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
                System.Data.DataTable addedCategories = new System.Data.DataTable();
                addedCategories.Columns.Add(new System.Data.DataColumn("Carrier_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedCategories.Columns.Add(new System.Data.DataColumn("Vehicle_Guid", typeof(System.Data.SqlTypes.SqlGuid)));

                System.Data.DataRow newRow = null;
                if ((objVehicleList == null) || (objVehicleList.Count == 0))
                {
                    newRow = addedCategories.NewRow();
                    newRow["Carrier_Guid"] = objCarrier.ID;
                    newRow["Vehicle_Guid"] = System.DBNull.Value;
                    addedCategories.Rows.Add(newRow);
                }
                else
                {
                    foreach (CVehicle objItem in objVehicleList)
                    {
                        newRow = addedCategories.NewRow();
                        newRow["Carrier_Guid"] = objCarrier.ID;
                        newRow["Vehicle_Guid"] = objItem.ID;
                        addedCategories.Rows.Add(newRow);
                    }
                }
                addedCategories.AcceptChanges();

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AssignCarrierWithVehicle]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.AddWithValue("@tCarrierVehicle", addedCategories);
                cmd.Parameters["@tCarrierVehicle"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tCarrierVehicle"].TypeName = "dbo.udt_CarrierVehicle";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditCarrier]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Carrier_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Carrier_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerStateType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Carrier_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CarrierRateType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CarrierRateType_Value", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Carrier_UNP", System.Data.DbType.String));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Carrier_Guid"].Value = this.ID;
                cmd.Parameters["@Carrier_Name"].Value = this.Name;
                cmd.Parameters["@CustomerStateType_Guid"].Value = this.StateType.ID;
                cmd.Parameters["@Carrier_IsActive"].Value = this.IsActive;
                cmd.Parameters["@CarrierRateType_Guid"].Value = this.CarrierRateType.ID;
                cmd.Parameters["@CarrierRateType_Value"].Value = this.RateTypeValue;
                cmd.Parameters["@Carrier_UNP"].Value = this.UNP;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    iRes = ((SaveDriverListForCarrier(objProfile, cmd, this.DriverList, this, ref strErr) == true) ? 0 : -1);
                    if (iRes == 0)
                    {
                        iRes = ((SaveVehicleListForCarrier(objProfile, cmd, this.VehicleList, this, ref strErr) == true) ? 0 : -1);
                    }
                    if (iRes == 0)
                    {
                        if ((this.CarrierRateTypeList != null) && (this.CarrierRateTypeList.Count > 0))
                        {
                            iRes = ((SaveCarrierRateTypeForCarrier(objProfile, cmd, this.CarrierRateTypeList, this, ref strErr) == true) ? 0 : -1);
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.AddressList != null) && (this.AddressList.Count > 0) && (this.AddressForDeleteList != null))
                        {
                            //foreach (CAddress objAddress in this.AddressList) { objAddress.ID = System.Guid.Empty; }
                            iRes = (CAddress.SaveAddressList(this.AddressList, this.AddressForDeleteList, EnumObject.Carrier, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.AccountList != null) && (this.AccountList.Count > 0))
                        {
                            //foreach (CAccount objAccount in this.AccountList) { objAccount.ID = System.Guid.Empty; }
                            iRes = (CAccount.SaveAccountList(this.AccountList, this.AccountForDeleteList, EnumObject.Carrier, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        // подтверждаем транзакцию
                        DBTransaction.Commit();
                    }
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств записи о перевозчике.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства записи о перевозчике.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteCarrier]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Carrier_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Carrier_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления записи о перевозчике.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить запись о перевозчике.\n\nТекст ошибки: " + f.Message, "Внимание",
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
            return (Name + " " + (StateType == null ? "" : StateType.ShortName));
        }
    }
    #endregion

    #region Тип путевого листа
    /// <summary>
    /// Класс "Тип путевого листа"
    /// </summary>
    public class CRouteSheetType : CBusinessObject
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
        /// <summary>
        /// Признак "Использоавть по умолчанию"
        /// </summary>
        private System.Boolean m_bIsDefault;
        /// <summary>
        /// Признак "Использоавть по умолчанию"
        /// </summary>
        [DisplayName("Признак \"Использовать по умолчанию\"")]
        [Description("Определяет, использовать ли данное значение по умолчанию в выпадающих списках")]
        [Category("1. Обязательные значения")]
        [ReadOnly(false)]
        [BrowsableAttribute(false)]
        public System.Boolean IsDefault
        {
            get { return m_bIsDefault; }
            set { m_bIsDefault = value; }
        }
        #endregion

        #region Конструктор
        public CRouteSheetType()
            : base()
        {
            m_strDescription = "";
            m_bIsDefault = false;
        }
        public CRouteSheetType(System.Guid uuidId, System.String strName, System.String strDscrpn, System.Boolean bIsDefault)
            : base(uuidId, strName)
        {
            m_strDescription = strDscrpn;
            m_bIsDefault = bIsDefault;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список объектов "Тип путевого листа"
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список объектов "Тип путевого листа"</returns>
        public static List<CRouteSheetType> GetRouteSheetTypeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CRouteSheetType> objList = new List<CRouteSheetType>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetRouteSheetType]", objProfile.GetOptionsDllDBName());
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
                        strDscrpn = (rs["RouteSheetType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["RouteSheetType_Description"];
                        objList.Add(new CRouteSheetType((System.Guid)rs["RouteSheetType_Guid"], (System.String)rs["RouteSheetType_Name"], strDscrpn, System.Convert.ToBoolean(rs["RouteSheetType_IsDefault"])));
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
                "Не удалось получить список объектов \"Тип путевого листа\".\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddRouteSheetType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheetType_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheetType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheetType_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RouteSheetType_Name"].Value = this.Name;
                if (this.m_strDescription == "")
                {
                    cmd.Parameters["@RouteSheetType_Description"].IsNullable = true;
                    cmd.Parameters["@RouteSheetType_Description"].Value = null;
                }
                else
                {
                    cmd.Parameters["@RouteSheetType_Description"].IsNullable = false;
                    cmd.Parameters["@RouteSheetType_Description"].Value = this.m_strDescription;
                }
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@RouteSheetType_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания объектa \"Тип путевого листа\".\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать объект \"Тип путевого листа\".\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteRouteSheetType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheetType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RouteSheetType_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления объектa \"Тип путевого листа\".\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить объект \"Тип путевого листа\".\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditRouteSheetType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheetType_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheetType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheetType_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RouteSheetType_Guid"].Value = this.ID;
                cmd.Parameters["@RouteSheetType_Name"].Value = this.Name;
                if (this.m_strDescription == "")
                {
                    cmd.Parameters["@RouteSheetType_Description"].IsNullable = true;
                    cmd.Parameters["@RouteSheetType_Description"].Value = null;
                }
                else
                {
                    cmd.Parameters["@RouteSheetType_Description"].IsNullable = false;
                    cmd.Parameters["@RouteSheetType_Description"].Value = this.m_strDescription;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств объектa \"Тип путевого листа\".\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства объектa \"Тип путевого листа\".\n\nТекст ошибки: " + f.Message, "Внимание",
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

    #region Регион доставки
    /// <summary>
    /// Класс "Регион доставки"
    /// </summary>
    public class CRegionDelivery : CBusinessObject
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
        /// <summary>
        /// Признак "Активен"
        /// </summary>
        private System.Boolean m_bIsActive;
        /// <summary>
        /// Признак "Активен"
        /// </summary>
        [DisplayName("Признак \"Активен\"")]
        [Description("Определяет, актуальна ли запись")]
        [Category("1. Обязательные значения")]
        [ReadOnly(false)]
        [BrowsableAttribute(false)]
        public System.Boolean IsActive
        {
            get { return m_bIsActive; }
            set { m_bIsActive = value; }
        }
        #endregion

        #region Конструктор
        public CRegionDelivery()
            : base()
        {
            m_strDescription = "";
            m_bIsActive = false;
        }
        public CRegionDelivery(System.Guid uuidId, System.String strName, System.String strDscrpn, System.Boolean bIsActive)
            : base(uuidId, strName)
        {
            m_strDescription = strDscrpn;
            m_bIsActive = bIsActive;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список объектов "Регион доставки"
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список объектов "Регион доставки"</returns>
        public static List<CRegionDelivery> GetRegionDeliveryList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CRegionDelivery> objList = new List<CRegionDelivery>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetRegionDelivery]", objProfile.GetOptionsDllDBName());
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
                        strDscrpn = (rs["RegionDelivery_Description"] == System.DBNull.Value) ? "" : (System.String)rs["RegionDelivery_Description"];
                        objList.Add(new CRegionDelivery((System.Guid)rs["RegionDelivery_Guid"], (System.String)rs["RegionDelivery_Name"], 
                            strDscrpn, System.Convert.ToBoolean(rs["RegionDelivery_IsActive"])));
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
                "Не удалось получить список объектов \"Регион доставки\".\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddRegionDelivery]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RegionDelivery_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RegionDelivery_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RegionDelivery_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RegionDelivery_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RegionDelivery_Name"].Value = this.Name;
                cmd.Parameters["@RegionDelivery_IsActive"].Value = this.IsActive;
                if (this.m_strDescription == "")
                {
                    cmd.Parameters["@RegionDelivery_Description"].IsNullable = true;
                    cmd.Parameters["@RegionDelivery_Description"].Value = null;
                }
                else
                {
                    cmd.Parameters["@RegionDelivery_Description"].IsNullable = false;
                    cmd.Parameters["@RegionDelivery_Description"].Value = this.m_strDescription;
                }
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@RegionDelivery_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания объектa \"Тип путевого листа\".\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать объект \"Регион доставки\".\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteRegionDelivery]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheetType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RegionDelivery_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления объектa \"Регион доставки\".\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить объект \"Регион доставки\".\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditRegionDelivery]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RegionDelivery_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RegionDelivery_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RegionDelivery_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RegionDelivery_IsActive", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RegionDelivery_Guid"].Value = this.ID;
                cmd.Parameters["@RegionDelivery_Name"].Value = this.Name;
                cmd.Parameters["@RegionDelivery_IsActive"].Value = this.IsActive;
                if (this.m_strDescription == "")
                {
                    cmd.Parameters["@RegionDelivery_Description"].IsNullable = true;
                    cmd.Parameters["@RegionDelivery_Description"].Value = null;
                }
                else
                {
                    cmd.Parameters["@RegionDelivery_Description"].IsNullable = false;
                    cmd.Parameters["@RegionDelivery_Description"].Value = this.m_strDescription;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств объектa \"Регион доставки\".\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства объектa \"Регион доставки\".\n\nТекст ошибки: " + f.Message, "Внимание",
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

    #region Документ на отгрузку для путевого листа

    public class CRouteSheetWaybill
    {
        #region Свойства
        /// <summary>
        /// Клиент
        /// </summary>
        private CCustomer m_objCustomer;
        /// <summary>
        /// Клиент
        /// </summary>
        public CCustomer Customer
        {
            get { return m_objCustomer; }
            set { m_objCustomer = value; }
        }
        /// <summary>
        /// Склад
        /// </summary>
        private CWarehouse m_objWareHouse;
        /// <summary>
        /// Склад
        /// </summary>
        public CWarehouse WareHouse
        {
            get { return m_objWareHouse; }
            set { m_objWareHouse = value; }
        }
        /// <summary>
        /// Компания
        /// </summary>
        private CCompany m_objCompany;
        /// <summary>
        /// Компания
        /// </summary>
        public CCompany Company
        {
            get { return m_objCompany; }
            set { m_objCompany = value; }
        }
        /// <summary>
        /// Валюта
        /// </summary>
        private CCurrency m_objCurrency;
        /// <summary>
        /// Валюта
        /// </summary>
        public CCurrency Currency
        {
            get { return m_objCurrency; }
            set { m_objCurrency = value; }
        }
        /// <summary>
        /// Вес
        /// </summary>
        private System.Double m_Weight;
        /// <summary>
        /// Вес
        /// </summary>
        public System.Double Weight
        {
            get { return m_Weight; }
            set { m_Weight = value; }
        }
        /// <summary>
        /// Вес
        /// </summary>
        public System.String WeightString
        {
            get { return System.String.Format("{0:### ### ##0.000}", m_Weight); }
        }
        /// <summary>
        /// Количество
        /// </summary>
        private System.Double m_Quantity;
        /// <summary>
        /// Количество
        /// </summary>
        public System.Double Quantity
        {
            get { return m_Quantity; }
            set { m_Quantity = value; }
        }
        /// <summary>
        /// Количество
        /// </summary>
        public System.String QuantityString
        {
            get { return System.String.Format("{0:### ### ##0}", m_Quantity); }
        }
        /// <summary>
        /// Сумма
        /// </summary>
        private System.Double m_AllMoney;
        /// <summary>
        /// Сумма
        /// </summary>
        public System.Double AllMoney
        {
            get { return m_AllMoney; }
            set { m_AllMoney = value; }
        }
        /// <summary>
        /// Сумма
        /// </summary>
        public System.String AllMoneyString
        {
            get { return System.String.Format("{0:### ### ##0.00}", m_AllMoney); }
        }
        /// <summary>
        /// Номер накладной
        /// </summary>
        private System.String m_strWaybillNum;
        /// <summary>
        /// Номер накладной
        /// </summary>
        public System.String WaybillNum
        {
            get { return m_strWaybillNum; }
            set { m_strWaybillNum = value; }
        }
        /// <summary>
        /// дата накладной
        /// </summary>
        private System.DateTime m_dtWaybillDate;
        /// <summary>
        /// дата накладной
        /// </summary>
        public System.DateTime WaybillDate
        {
            get { return m_dtWaybillDate; }
            set { m_dtWaybillDate = value; }
        }
        /// <summary>
        /// дата накладной
        /// </summary>
        public System.String WaybillShortDate
        {
            get { return m_dtWaybillDate.ToShortDateString(); }
        }
        /// <summary>
        /// Клиент
        /// </summary>
        public System.String CustomerName
        {
            get {  return ( ( m_objCustomer == null ) ? "" : m_objCustomer.FullName  ); }
        }
        /// <summary>
        /// Компания
        /// </summary>
        public System.String CompanyAbbr
        {
            get { return ((m_objCompany == null) ? "" : m_objCompany.Abbr); }
        }
        /// <summary>
        /// Склад
        /// </summary>
        public System.String WarehouseName
        {
            get { return ((m_objWareHouse == null) ? "" : m_objWareHouse.Name); }
        }
        /// <summary>
        /// Валюта
        /// </summary>
        public System.String CurrencyCode
        {
            get { return ((m_objCurrency == null) ? "" : m_objCurrency.CurrencyAbbr); }
        }
        /// <summary>
        /// Вид расходов
        /// </summary>
        private CRouteSheetAdvancedExpenseType m_objRouteSheetAdvancedExpenseType;
        /// <summary>
        /// Вид расходов
        /// </summary>
        public CRouteSheetAdvancedExpenseType ExpenseType
        {
            get { return m_objRouteSheetAdvancedExpenseType; }
            set { m_objRouteSheetAdvancedExpenseType = value; }
        }
        public System.String ExpenseTypeName
        {
            get { return ((m_objRouteSheetAdvancedExpenseType == null) ? "" : m_objRouteSheetAdvancedExpenseType.Name ); }
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
        #endregion

        #region Конструктор
        public CRouteSheetWaybill()
        {
            m_objCustomer = null;
            m_objCompany = null;
            m_objWareHouse = null;
            m_objCurrency = null;
            m_strWaybillNum = "";
            m_dtWaybillDate = System.DateTime.MinValue;
            m_Quantity = 0;
            m_Weight = 0;
            m_AllMoney = 0;
            m_objRouteSheetAdvancedExpenseType = null;
            m_strDescription = "";
        }
        #endregion

        #region Список записей
        /// <summary>
        /// Возвращает список накладных для путевого листа
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQl-команда</param>
        /// <param name="objRouteSheet">путевой лист</param>
        /// <returns>список накладных для путевого листа</returns>
        public static List<CRouteSheetWaybill> GetRouteSheetWaybillList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, CRouteSheet objRouteSheet)
        {
            List<CRouteSheetWaybill> objList = new List<CRouteSheetWaybill>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetRouteSheetWaybill]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheet_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RouteSheet_Guid"].Value = objRouteSheet.ID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CRouteSheetWaybill objRouteSheetWaybill = null;
                    while (rs.Read())
                    {
                        objRouteSheetWaybill = new CRouteSheetWaybill();
                        objRouteSheetWaybill.m_strWaybillNum = System.Convert.ToString(rs["Waybill_Num"]);
                        objRouteSheetWaybill.m_dtWaybillDate = System.Convert.ToDateTime(rs["Waybill_Date"]);
                        objRouteSheetWaybill.m_AllMoney = System.Convert.ToDouble(rs["Waybill_Money"]);
                        objRouteSheetWaybill.m_Quantity = System.Convert.ToDouble(rs["Waybill_Quantity"]);
                        objRouteSheetWaybill.m_Weight = System.Convert.ToDouble(rs["Waybill_Weight"]);

                        objRouteSheetWaybill.m_objCustomer = new CCustomer();
                        objRouteSheetWaybill.m_objCustomer.ID = (System.Guid)rs["Customer_Guid"];
                        objRouteSheetWaybill.m_objCustomer.FullName = System.Convert.ToString(rs["Customer_Name"]);

                        objRouteSheetWaybill.m_objCompany = new CCompany((System.Guid)rs["Company_Guid"], System.Convert.ToString(rs["Company_Name"]), System.Convert.ToString(rs["Company_Acronym"]));

                        objRouteSheetWaybill.m_objWareHouse = new CWarehouse();
                        objRouteSheetWaybill.m_objWareHouse.ID = (System.Guid)rs["Warehouse_Guid"];
                        objRouteSheetWaybill.m_objWareHouse.Name = System.Convert.ToString(rs["Warehouse_Name"]);

                        objRouteSheetWaybill.m_objCurrency = new CCurrency((System.Guid)rs["Currency_Guid"], System.Convert.ToString(rs["Currency_Name"]), System.Convert.ToString(rs["Currency_Abbr"]), System.Convert.ToString(rs["Currency_Code"]));

                        if( rs["RouteSheetAdvancedExpenseType_Guid"] != System.DBNull.Value )
                        {
                            objRouteSheetWaybill.m_objRouteSheetAdvancedExpenseType = new CRouteSheetAdvancedExpenseType((System.Guid)rs["RouteSheetAdvancedExpenseType_Guid"], 
                                    System.Convert.ToString(rs["RouteSheetAdvancedExpenseType_Name"]), System.Convert.ToBoolean(rs["RouteSheetAdvancedExpenseType_IsActive"]));
                        }

                        if (rs["RouteSheetWaybill_Description"] != System.DBNull.Value)
                        {
                            objRouteSheetWaybill.m_strDescription = System.Convert.ToString(rs["RouteSheetWaybill_Description"]);
                        }

                        objList.Add( objRouteSheetWaybill );
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
                "Не удалось получить список накладных для путевого листа.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        #endregion
    }

    #endregion

    #region Информация о времени на разгрузку по путевому листу
    /// <summary>
    /// Класс "Информация о времени на разгрузку по путевому листу"
    /// </summary>
    public class CRouteSheetAcceptanceTime
    {
        /// <summary>
        /// Розничная точка
        /// </summary>
        public CRtt Rtt {get; set;}
        /// <summary>
        /// Время, затраченное на приёмкутовара в минутах
        /// </summary>
        public System.Double AcceptanceTime { get; set; }

        public CRouteSheetAcceptanceTime()
        {
            Rtt = null;
            AcceptanceTime = 0;
        }
        /// <summary>
        /// Возвращает список РТТ с указанием времени на приёмку товара
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objRouteSheet">Путевой лист</param>
        /// <returns>список РТТ с указанием времени на приёмку товара</returns>
        public static List<CRouteSheetAcceptanceTime> GetAcceptanceTimeForRouteSheet(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, CRouteSheet objRouteSheet)
        {
            List<CRouteSheetAcceptanceTime> objList = new List<CRouteSheetAcceptanceTime>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetRouteSheetAcceptance]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheet_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RouteSheet_Guid"].Value = objRouteSheet.ID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add( new CRouteSheetAcceptanceTime() { 
                            Rtt = new CRtt() { ID = (System.Guid)rs["Rtt_Guid"], FullName = System.Convert.ToString(rs["RttAddress"])}, 
                            AcceptanceTime = System.Convert.ToDouble(rs["AcceptanceTime"]) } );
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
                objList = null;

                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список РТТ с указанием времени на приёмку товара.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }

    }
    #endregion

    #region Путевой лист
    /// <summary>
    /// Класс "Путевой лист"
    /// </summary>
    public class CRouteSheet : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Перевозчик
        /// </summary>
        private CCarrier m_objCarrier;
        /// <summary>
        /// Перевозчик
        /// </summary>
        public CCarrier Carrier
        {
            get { return m_objCarrier; }
            set { m_objCarrier = value; }
        }
        /// <summary>
        /// Водитель
        /// </summary>
        private CDriver m_objDriver;
        /// <summary>
        /// Водитель
        /// </summary>
        public CDriver Driver
        {
            get { return m_objDriver; }
            set { m_objDriver = value; }
        }
        /// <summary>
        /// Автомобиль
        /// </summary>
        private CVehicle m_objVehicle;
        /// <summary>
        /// Автомобиль
        /// </summary>
        public CVehicle Vehicle
        {
            get { return m_objVehicle; }
            set { m_objVehicle = value; }
        }
        /// <summary>
        /// Тариф (тип)
        /// </summary>
        private CarrierRateType m_objRateType;
        /// <summary>
        /// Тариф (тип)
        /// </summary>
        public CarrierRateType RateType
        {
            get { return m_objRateType; }
            set { m_objRateType = value; }
        }
        /// <summary>
        /// Тип путевого листа
        /// </summary>
        private CRouteSheetType m_objRouteSheetType;
        /// <summary>
        /// Тип путевого листа
        /// </summary>
        public CRouteSheetType RouteSheetType
        {
            get { return m_objRouteSheetType; }
            set { m_objRouteSheetType = value; }
        }
        /// <summary>
        /// Регион доставки
        /// </summary>
        private CRegionDelivery m_objRegionDelivery;
        /// <summary>
        /// Регион доставки
        /// </summary>
        public CRegionDelivery RegionDelivery
        {
            get { return m_objRegionDelivery; }
            set { m_objRegionDelivery = value; }
        }
        /// <summary>
        /// Дата
        /// </summary>
        private System.DateTime m_objDate;
        /// <summary>
        /// Дата
        /// </summary>
        public System.DateTime Date
        {
            get { return m_objDate; }
            set { m_objDate = value; }
        }
        /// <summary>
        /// Дата отгрузки
        /// </summary>
        private System.DateTime m_objShipDate;
        /// <summary>
        /// Дата отгрузки
        /// </summary>
        public System.DateTime ShipDate
        {
            get { return m_objShipDate; }
            set { m_objShipDate = value; }
        }
        /// <summary>
        /// Количество
        /// </summary>
        private System.Double m_Quantity;
        /// <summary>
        /// Количество
        /// </summary>
        public System.Double Quantity
        {
            get { return m_Quantity; }
            set { m_Quantity = value; }
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
        /// Признак "Путевой лист закрыт"
        /// </summary>
        public System.Boolean IsClosed
        {
            get { return !(m_bIsActive); }
        }
        /// <summary>
        /// Ставка тарифа
        /// </summary>
        private System.Decimal m_CarrierRateTypeValue;
        /// <summary>
        /// Ставка тарифа
        /// </summary>
        public System.Decimal CarrierRateTypeValue
        {
            get { return m_CarrierRateTypeValue; }
            set { m_CarrierRateTypeValue = value; }
        }
        /// <summary>
        /// Сумма
        /// </summary>
        private System.Decimal m_SumMoney;
        /// <summary>
        /// Сумма
        /// </summary>
        public System.Decimal SumMoney
        {
            get { return m_SumMoney; }
            set { m_SumMoney = value; }
        }
        /// <summary>
        /// Сумма дополнительных расходов
        /// </summary>
        private System.Decimal m_SumAdvExpenseMoney;
        /// <summary>
        /// Сумма дополнительных расходов
        /// </summary>
        public System.Decimal SumAdvExpenseMoney
        {
            get { return m_SumAdvExpenseMoney; }
            set { m_SumAdvExpenseMoney = value; }
        }
        /// <summary>
        /// Итоговая сумма
        /// </summary>
        public System.Decimal AllMoney
        {
            get { return ( m_SumMoney + m_SumAdvExpenseMoney ); }
        }
        /// <summary>
        /// Cписок дополнительных расходов
        /// </summary>
        private List<CAdvancedExpense> m_objAdvancedExpenseList;
        /// <summary>
        /// Cписок дополнительных расходов
        /// </summary>
        public List<CAdvancedExpense> AdvancedExpenseList
        {
            get { return m_objAdvancedExpenseList; }
            set { m_objAdvancedExpenseList = value; }
        }
        /// <summary>
        /// Список накладных
        /// </summary>
        private List<CRouteSheetWaybill> m_objRouteSheetWaybillList;
        /// <summary>
        /// Список накладных
        /// </summary>
        public List<CRouteSheetWaybill> RouteSheetWaybillList
        {
            get { return m_objRouteSheetWaybillList; }
            set { m_objRouteSheetWaybillList = value; }
        }
        /// <summary>
        /// информация о времени, затраченном на приёмку товара
        /// </summary>
        public List<CRouteSheetAcceptanceTime> AcceptanceTimeList {get; set;}
        /// <summary>
        /// Реквизиты перевозчика
        /// </summary>
        public System.String CarrirerName
        {
            get { return ((this.m_objCarrier == null) ? "" : m_objCarrier.Name); }
        }
        /// <summary>
        /// Реквизиты водителя
        /// </summary>
        public System.String DriverName
        {
            get { return ((this.m_objDriver == null) ? "" : m_objDriver.GetFIO()); }
        }
        /// <summary>
        /// Реквизиты автомобиля
        /// </summary>
        public System.String VehicleName
        {
            get { return ((this.m_objVehicle == null) ? "" : m_objVehicle.FullName); }
        }
        public System.String RouteSheetFullName
        {
            get { return ("№" + Name + " от " + Date.ToShortDateString() + " " + ((m_objCarrier == null) ? " перевозчик " : m_objCarrier.Name)); }
        }
        public System.String RouteSheetTypeName
        {
            get { return ((this.m_objRouteSheetType == null) ? "" : m_objRouteSheetType.Name); }
        }
        public System.String RegionDeliveryName
        {
            get { return ((this.m_objRegionDelivery == null) ? "" : m_objRegionDelivery.Name); }
        }
        /// <summary>
        /// Оплачено
        /// </summary>
        public System.Decimal Payment { get; set; }
        /// <summary>
        /// Дата последней оплаты
        /// </summary>
        public System.DateTime PaymentDate { get; set; }
        /// <summary>
        /// Дата последней оплаты
        /// </summary>
        public System.String PaymentDateToShortDateString
        {
            get { return (((PaymentDate.CompareTo(System.DateTime.MinValue) == 0) || (Payment == 0)) ? "" : PaymentDate.ToShortDateString()); }
        }
        /// <summary>
        /// Сальдо
        /// </summary>
        public System.Decimal Saldo { get; set; }
        #endregion

        #region Конструктор
        public CRouteSheet()
            : base()
        {
            m_bIsActive = false;
            m_objCarrier = null;
            m_objDriver = null;
            m_objRateType = null;
            m_objVehicle = null;
            m_objAdvancedExpenseList = null;
            m_Quantity = 0;
            m_objDate = System.DateTime.MinValue;
            m_objShipDate = System.DateTime.MinValue;
            m_CarrierRateTypeValue = 0;
            m_SumMoney = 0;
            m_SumAdvExpenseMoney = 0;
            m_objRouteSheetType = null;
            m_objRouteSheetWaybillList = null;
            m_objRegionDelivery = null;
            AcceptanceTimeList = null;
            Payment = 0;
            //PaymentDate = System.DateTime.MinValue;
            Saldo = 0;

        }
        public CRouteSheet(System.Guid uuidId, System.String strName, System.DateTime objDate, System.Boolean bIsActive,
            CCarrier objCarrier, CDriver objDriver, CarrierRateType objRateType, CVehicle objVehicle, System.Double dQuantity,
            System.Decimal mCarrierRateTypeValue, System.Decimal mSumMoney, System.Decimal mSumAdvExpenseMoney,
            CRouteSheetType objRouteSheetType, CRegionDelivery objRegionDelivery, System.DateTime objShipDate)
        {
            ID = uuidId;
            Name = strName;
            m_bIsActive = bIsActive;
            m_objCarrier = objCarrier;
            m_objDriver = objDriver;
            m_objRateType = objRateType;
            m_objVehicle = objVehicle;
            m_Quantity = dQuantity;
            m_objDate = objDate;
            m_objShipDate = objShipDate;
            m_objAdvancedExpenseList = null;
            m_CarrierRateTypeValue = mCarrierRateTypeValue;
            m_SumMoney = mSumMoney;
            m_SumAdvExpenseMoney = mSumAdvExpenseMoney;
            m_objRouteSheetType = objRouteSheetType;
            m_objRouteSheetWaybillList = null;
            m_objRegionDelivery = objRegionDelivery;
            AcceptanceTimeList = null;
            Payment = 0;
            //PaymentDate = System.DateTime.MinValue;
            Saldo = 0;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список путевых листов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список путевых листов</returns>
        public static List<CRouteSheet> GetRouteSheetList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.DateTime dtBeginDate, System.DateTime dtEndDate, CCarrier obCarrier, System.Boolean bIsActive)
        {
            List<CRouteSheet> objList = new List<CRouteSheet>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetRouteSheet]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                if ( ( obCarrier != null ) && ( obCarrier.ID.CompareTo( System.Guid.Empty ) != 0 ) )
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Carrier_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@Carrier_Guid"].Value = obCarrier.ID;
                }
                if (dtBeginDate != null)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BeginDate", System.Data.SqlDbType.Date));
                    cmd.Parameters["@BeginDate"].Value = dtBeginDate;
                }
                if (dtEndDate != null)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EndDate", System.Data.SqlDbType.Date));
                    cmd.Parameters["@EndDate"].Value = dtEndDate;
                }
                if (bIsActive == true)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OnlyActive", System.Data.SqlDbType.Bit));
                    cmd.Parameters["@OnlyActive"].Value = bIsActive;
                }
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CCarrier objCarrier = null;
                    CDriver objDriver = null;
                    CarrierRateType objRateType = null;
                    CVehicle objVehicle = null;
                    CRouteSheetType objRouteSheetType = null;
                    CRegionDelivery objRegionDelivery = null;
                    while (rs.Read())
                    {
                        objRateType = new CarrierRateType(
                            (System.Guid)rs["CarrierRateType_Guid"],
                            (System.String)rs["CarrierRateType_Name"],
                            System.Convert.ToBoolean(rs["CarrierRateType_IsActive"])
                            );
                        objDriver = new CDriver((System.Guid)rs["Driver_Guid"],
                            (System.String)rs["Driver_FirstName"], (System.String)rs["Driver_LastName"],
                            (System.String)rs["Driver_MiddleName"], System.Convert.ToBoolean(rs["Driver_IsActive"])
                            );
                        objVehicle = new CVehicle((System.Guid)rs["Vehicle_Guid"], (System.String)rs["Vehicle_Mark"],
                            "", (System.String)rs["Vehicle_Number"],
                            System.Convert.ToDouble(rs["Vehicle_Capacity"]), System.Convert.ToDouble(rs["Vehicle_Volume"]),
                            new CDriver((System.Guid)rs["Driver_Guid"],
                            (System.String)rs["Driver_FirstName"], (System.String)rs["Driver_LastName"],
                            (System.String)rs["Driver_MiddleName"], System.Convert.ToBoolean(rs["Driver_IsActive"])
                            )
                        );
                        objCarrier = new CCarrier(
                            (System.Guid)rs["Carrier_Guid"],
                            (System.String)rs["Carrier_Name"],
                            new CStateType((System.Guid)rs["CustomerStateType_Guid"], (System.String)rs["CustomerStateType_Name"],
                               (System.String)rs["CustomerStateType_ShortName"], System.Convert.ToBoolean(rs["CustomerStateType_IsActive"])),
                            System.Convert.ToBoolean(rs["Carrier_IsActive"]), null, 0,
                            ((rs["Carrier_UNP"] == System.DBNull.Value) ? "" : (System.String)rs["Carrier_UNP"])
                            );
                        objRouteSheetType = new CRouteSheetType( (System.Guid)rs["RouteSheetType_Guid"], (System.String)rs["RouteSheetType_Name"], "", false );

                        objRegionDelivery = new CRegionDelivery((System.Guid)rs["RegionDelivery_Guid"], (System.String)rs["RegionDelivery_Name"],
                            "", System.Convert.ToBoolean(rs["RegionDelivery_IsActive"]));

                        objList.Add(new CRouteSheet(
                            (System.Guid)rs["RouteSheet_Guid"],
                            (System.String)rs["RouteSheet_Number"], System.Convert.ToDateTime(rs["RouteSheet_Date"]), System.Convert.ToBoolean(rs["RouteSheet_IsActive"]),
                            objCarrier, objDriver, objRateType, objVehicle, System.Convert.ToDouble(rs["RouteSheet_Quantity"]),
                            System.Convert.ToDecimal(rs["CarrierRateType_Value"]),
                            System.Convert.ToDecimal(rs["RouteSheet_AllMoney"]),
                            System.Convert.ToDecimal(rs["SumAdvExpenseMoney"]),
                            objRouteSheetType, objRegionDelivery, System.Convert.ToDateTime(rs["RouteSheet_ShipDate"])
                            ) { Payment = System.Convert.ToDecimal(rs["RouteSheet_Payment"]),
                                PaymentDate = ((rs["RouteSheet_PaymentDate"] == System.DBNull.Value) ? System.DateTime.MinValue : System.Convert.ToDateTime(rs["RouteSheet_PaymentDate"])),
                                Saldo = System.Convert.ToDecimal(rs["RouteSheet_Saldo"])
                              }
                            );
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
                "Не удалось получить список путевых листов.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                if (this.Carrier == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать перевозчика!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.Driver == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать водителя!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.RouteSheetType == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать тип путевого листа!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.RateType == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать тип тарифа!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.Vehicle == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать автомобиль!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.RegionDelivery == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать регион доставки!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.Quantity < 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать количество часов(километров)!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.CarrierRateTypeValue < 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ставка тарифа не должна быть отрицательной!", "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddRouteSheet]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheet_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheet_Number", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheet_Date", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheet_ShipDate", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Carrier_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Driver_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vehicle_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CarrierRateType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheet_Quantity", System.Data.SqlDbType.Float));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheet_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CarrierRateType_Value", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheetType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RegionDelivery_Guid", System.Data.SqlDbType.UniqueIdentifier));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RouteSheet_Number"].Value = this.Name;
                cmd.Parameters["@RouteSheet_Date"].Value = this.Date;
                cmd.Parameters["@RouteSheet_ShipDate"].Value = this.ShipDate;
                cmd.Parameters["@RouteSheet_IsActive"].Value = this.IsActive;
                cmd.Parameters["@RouteSheet_Quantity"].Value = this.Quantity;
                cmd.Parameters["@Carrier_Guid"].Value = this.Carrier.ID;
                cmd.Parameters["@Driver_Guid"].Value = this.Driver.ID;
                cmd.Parameters["@Vehicle_Guid"].Value = this.Vehicle.ID;
                cmd.Parameters["@CarrierRateType_Guid"].Value = this.RateType.ID;
                cmd.Parameters["@CarrierRateType_Value"].Value = this.CarrierRateTypeValue;
                cmd.Parameters["@RouteSheetType_Guid"].Value = this.RouteSheetType.ID;
                cmd.Parameters["@RegionDelivery_Guid"].Value = this.RegionDelivery.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@RouteSheet_Guid"].Value;
                    if ((this.AdvancedExpenseList != null) && (this.AdvancedExpenseList.Count > 0))
                    {
                        // список дополнительных расходов
                        iRes = ((SaveAdvancedExpenseForRouteSheet(objProfile, cmd, this.AdvancedExpenseList, this, ref strErr) == true) ? 0 : -1);
                    }
                    if (iRes == 0)
                    {
                        // перечень накладных по путевому листу
                        if (this.RouteSheetWaybillList != null)
                        {
                            iRes = ((SaveWaybillListForRouteSheet(objProfile, cmd, this.RouteSheetWaybillList, this, ref strErr) == true) ? 0 : -1);
                        }
                    }
                    if (iRes == 0)
                    {
                        // информация о времени на разгрузку товара
                        if (this.AcceptanceTimeList != null)
                        {
                            iRes = (( SaveAcceptanceTimeForRouteSheet(objProfile, cmd, this.AcceptanceTimeList, this, ref strErr) == true) ? 0 : -1);
                        }
                    }
                    if (iRes == 0)
                    {
                        // подтверждаем транзакцию
                        DBTransaction.Commit();
                    }
                }
                if (iRes != 0)
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания записи о путевом листе.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать запись о путевом листе.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        /// <summary>
        /// Сохраняет в БД информацию о списке дополнительных расходов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objAdvancedExpenseList">список водителей</param>
        /// <param name="objCRouteSheet">путевой лист</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean SaveAdvancedExpenseForRouteSheet(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            List<CAdvancedExpense> objAdvancedExpenseList, CRouteSheet objCRouteSheet, ref System.String strErr)
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
                System.Data.DataTable addedCategories = new System.Data.DataTable();
                addedCategories.Columns.Add(new System.Data.DataColumn("RouteSheet_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedCategories.Columns.Add(new System.Data.DataColumn("RouteSheetAdvancedExpenseType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedCategories.Columns.Add(new System.Data.DataColumn("RouteSheetAdvancedExpense_Id", typeof(System.Data.SqlTypes.SqlInt32)));
                addedCategories.Columns.Add(new System.Data.DataColumn("RouteSheetAdvancedExpense_AllMoney", typeof(System.Data.SqlTypes.SqlMoney)));
                addedCategories.Columns.Add(new System.Data.DataColumn("RouteSheetAdvancedExpense_Description", typeof(System.Data.SqlTypes.SqlString)));

                System.Data.DataRow newRow = null;
                if ((objAdvancedExpenseList != null) && (objAdvancedExpenseList.Count > 0))
                {
                    foreach (CAdvancedExpense objItem in objAdvancedExpenseList)
                    {
                        newRow = addedCategories.NewRow();
                        newRow["RouteSheet_Guid"] = objCRouteSheet.ID;
                        newRow["RouteSheetAdvancedExpenseType_Guid"] = objItem.AdvancedExpenseType.ID;
                        newRow["RouteSheetAdvancedExpense_Id"] = objItem.ID;
                        newRow["RouteSheetAdvancedExpense_AllMoney"] = objItem.AllMoney;
                        newRow["RouteSheetAdvancedExpense_Description"] = objItem.Description;
                        addedCategories.Rows.Add(newRow);
                    }
                }
                addedCategories.AcceptChanges();

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AssignRouteSheetAdvancedExpense]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.AddWithValue("@tRouteSheetAdvancedExpense", addedCategories);
                cmd.Parameters["@tRouteSheetAdvancedExpense"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tRouteSheetAdvancedExpense"].TypeName = "dbo.udt_RouteSheetAdvancedExpense";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
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
        /// Привязка путевого листа к списку заказов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQl-команда</param>
        /// <param name="objSupplIdentifierList">список идентификаторов заказов</param>
        /// <param name="objCRouteSheet">путевой лист</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean SaveSupplListForRouteSheet(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            List<CSupplIdentifier> objSupplIdentifierList, CRouteSheet objCRouteSheet, System.Boolean bAssign, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                // сперва пишем в InterBase и если всё хорошо, то пишем в SQL Server
                if (SaveSupplListForRouteSheetInIB(objProfile, null, objSupplIdentifierList, objCRouteSheet, bAssign, ref strErr) == false)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Не удалось назначить путевой лист заказу в программе \"Контракт\"\nИзменения отменены.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);

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
                System.Data.DataTable addedCategories = new System.Data.DataTable();
                addedCategories.Columns.Add(new System.Data.DataColumn("Suppl_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedCategories.Columns.Add(new System.Data.DataColumn("Suppl_Id", typeof(System.Data.SqlTypes.SqlInt32)));
                addedCategories.Columns.Add(new System.Data.DataColumn("RouteSheet_Guid", typeof(System.Data.SqlTypes.SqlGuid)));

                System.Data.DataRow newRow = null;
                if ((objSupplIdentifierList != null) && (objSupplIdentifierList.Count > 0))
                {
                    foreach (CSupplIdentifier objItem in objSupplIdentifierList)
                    {
                        newRow = addedCategories.NewRow();
                        newRow["Suppl_Guid"] = objItem.SupplGuid;
                        newRow["Suppl_Id"] = objItem.SupplId;
                        newRow["RouteSheet_Guid"] = objCRouteSheet.ID;
                        addedCategories.Rows.Add(newRow);
                    }
                }
                addedCategories.AcceptChanges();


                cmd.Parameters.Clear();
                cmd.CommandTimeout = 720;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AssignRouteSheetWithSuppl]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.AddWithValue("@tSupplIdentifier", addedCategories);
                cmd.Parameters["@tSupplIdentifier"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tSupplIdentifier"].TypeName = "dbo.udt_SupplIdentifier";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AssignFlag", System.Data.SqlDbType.Bit));
                cmd.Parameters["@AssignFlag"].Value = bAssign;
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

                //// если у нас все хорошо получилось, то попробуем провернуть аналогичные действия в InterBase
                //if (iRes == 0)
                //{
                //    bRet = SaveSupplListForRouteSheetInIB(objProfile, cmdSQL, objSupplIdentifierList, objCRouteSheet, bAssign, ref strErr);
                //}

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
        /// Привязка путевого листа к списку заказов в InterBase
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQl-команда</param>
        /// <param name="objSupplIdentifierList">список идентификаторов заказов</param>
        /// <param name="objCRouteSheet">путевой лист</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean SaveSupplListForRouteSheetInIB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            List<CSupplIdentifier> objSupplIdentifierList, CRouteSheet objCRouteSheet, System.Boolean bAssign, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            //System.Data.SqlClient.SqlTransaction DBTransaction = null;
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
                    //DBTransaction = DBConnection.BeginTransaction();
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    //cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }
                System.Data.DataTable addedCategories = new System.Data.DataTable();
                addedCategories.Columns.Add(new System.Data.DataColumn("Suppl_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedCategories.Columns.Add(new System.Data.DataColumn("Suppl_Id", typeof(System.Data.SqlTypes.SqlInt32)));
                addedCategories.Columns.Add(new System.Data.DataColumn("RouteSheet_Guid", typeof(System.Data.SqlTypes.SqlGuid)));

                System.Data.DataRow newRow = null;
                if ((objSupplIdentifierList != null) && (objSupplIdentifierList.Count > 0))
                {
                    foreach (CSupplIdentifier objItem in objSupplIdentifierList)
                    {
                        newRow = addedCategories.NewRow();
                        newRow["Suppl_Guid"] = objItem.SupplGuid;
                        newRow["Suppl_Id"] = objItem.SupplId;
                        newRow["RouteSheet_Guid"] = objCRouteSheet.ID;
                        addedCategories.Rows.Add(newRow);
                    }
                }

                //newRow = addedCategories.NewRow();
                //newRow["Suppl_Guid"] = new System.Guid("7EDE6B08-7761-4A60-8C0A-57F138A922C5");
                //newRow["Suppl_Id"] = 676123; // objItem.SupplId;
                //newRow["RouteSheet_Guid"] = objCRouteSheet.ID;
                //addedCategories.Rows.Add(newRow);
                
                addedCategories.AcceptChanges();

                cmd.Parameters.Clear();
                cmd.CommandTimeout = 720;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AssignRouteSheetWithSupplInIB]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.AddWithValue("@tSupplIdentifier", addedCategories);
                cmd.Parameters["@tSupplIdentifier"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tSupplIdentifier"].TypeName = "dbo.udt_SupplIdentifier";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AssignFlag", System.Data.SqlDbType.Bit));
                cmd.Parameters["@AssignFlag"].Value = bAssign;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                if (cmdSQL == null)
                {
                    DBConnection.Close();
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
        /// <summary>
        /// Сохраняет в БД список накладных для путевого листа
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objWaybillList">список накладных</param>
        /// <param name="objCRouteSheet">путевой лист</param>
        /// <param name="strErr">строка с сообщением об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean SaveWaybillListForRouteSheet(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            List<CRouteSheetWaybill> objWaybillList, CRouteSheet objCRouteSheet, ref System.String strErr)
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
                System.Data.DataTable addedCategories = new System.Data.DataTable();

                addedCategories.Columns.Add(new System.Data.DataColumn("RouteSheet_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedCategories.Columns.Add(new System.Data.DataColumn("Warehouse_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedCategories.Columns.Add(new System.Data.DataColumn("Customer_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedCategories.Columns.Add(new System.Data.DataColumn("Waybill_Num", typeof(System.Data.SqlTypes.SqlString)));
                addedCategories.Columns.Add(new System.Data.DataColumn("Waybill_Date", typeof(System.Data.SqlTypes.SqlDateTime)));
                addedCategories.Columns.Add(new System.Data.DataColumn("Currency_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedCategories.Columns.Add(new System.Data.DataColumn("Waybill_Quantity", typeof(System.Data.SqlTypes.SqlMoney)));
                addedCategories.Columns.Add(new System.Data.DataColumn("Waybill_Money", typeof(System.Data.SqlTypes.SqlMoney)));
                addedCategories.Columns.Add(new System.Data.DataColumn("Waybill_Weight", typeof(System.Data.SqlTypes.SqlMoney)));
                addedCategories.Columns.Add(new System.Data.DataColumn("Company_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedCategories.Columns.Add(new System.Data.DataColumn("RouteSheetAdvancedExpenseType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedCategories.Columns.Add(new System.Data.DataColumn("RouteSheetWaybill_Description", typeof(System.Data.SqlTypes.SqlString)));

                System.Data.DataRow newRow = null;
                if (objWaybillList != null)
                {
                    if (objWaybillList.Count > 0)
                    {
                        foreach (CRouteSheetWaybill objItem in objWaybillList)
                        {
                            newRow = addedCategories.NewRow();
                            newRow["RouteSheet_Guid"] = objCRouteSheet.ID;
                            newRow["Warehouse_Guid"] = objItem.WareHouse.ID;
                            newRow["Customer_Guid"] = objItem.Customer.ID;
                            newRow["Waybill_Num"] = objItem.WaybillNum;
                            newRow["Waybill_Date"] = objItem.WaybillDate;
                            newRow["Currency_Guid"] = objItem.Currency.ID;
                            newRow["Waybill_Quantity"] = (System.Data.SqlTypes.SqlMoney)objItem.Quantity;
                            newRow["Waybill_Money"] = (System.Data.SqlTypes.SqlMoney)objItem.AllMoney;
                            newRow["Waybill_Weight"] = (System.Data.SqlTypes.SqlMoney)objItem.Weight;
                            newRow["Company_Guid"] = objItem.Company.ID;
                            newRow["RouteSheetAdvancedExpenseType_Guid"] = objItem.ExpenseType.ID;
                            newRow["RouteSheetWaybill_Description"] = objItem.Description;
                            addedCategories.Rows.Add(newRow);
                        }
                    }
                    else
                    {
                        newRow = addedCategories.NewRow();
                        newRow["RouteSheet_Guid"] = objCRouteSheet.ID;
                        newRow["Warehouse_Guid"] = System.Guid.Empty;
                        newRow["Customer_Guid"] = System.Guid.Empty;
                        newRow["Waybill_Num"] = "";
                        newRow["Waybill_Date"] = System.DateTime.Today;
                        newRow["Currency_Guid"] = System.Guid.Empty;
                        newRow["Waybill_Quantity"] = 0;
                        newRow["Waybill_Money"] = 0;
                        newRow["Waybill_Weight"] = 0;
                        newRow["Company_Guid"] = System.Guid.Empty;
                        newRow["RouteSheetAdvancedExpenseType_Guid"] = System.Guid.Empty;
                        newRow["RouteSheetWaybill_Description"] = "";
                        addedCategories.Rows.Add(newRow);
                    }
                }
                 addedCategories.AcceptChanges();

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AssignRouteSheetWaybill]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.AddWithValue("@tRouteSheetWaybill", addedCategories);
                cmd.Parameters["@tRouteSheetWaybill"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tRouteSheetWaybill"].TypeName = "dbo.udt_RouteSheetWaybill";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
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
        /// Сохраняет в БД информацию о времени, затраченном на разгрузку товара
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objAcceptanceTimeList">информация о времени, затраченном на разгрузку товара</param>
        /// <param name="objCRouteSheet">путевой лист</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean SaveAcceptanceTimeForRouteSheet(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            List<CRouteSheetAcceptanceTime> objAcceptanceTimeList, CRouteSheet objCRouteSheet, ref System.String strErr)
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
                System.Data.DataTable addedCategories = new System.Data.DataTable();
                addedCategories.Columns.Add(new System.Data.DataColumn("RouteSheet_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedCategories.Columns.Add(new System.Data.DataColumn("Rtt_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedCategories.Columns.Add(new System.Data.DataColumn("AcceptanceTime", typeof(System.Data.SqlTypes.SqlDouble)));

                System.Data.DataRow newRow = null;
                if ((objAcceptanceTimeList != null) && (objAcceptanceTimeList.Count > 0))
                {
                    foreach (CRouteSheetAcceptanceTime objItem in objAcceptanceTimeList)
                    {
                        newRow = addedCategories.NewRow();
                        newRow["RouteSheet_Guid"] = objCRouteSheet.ID;
                        newRow["Rtt_Guid"] = objItem.Rtt.ID;
                        newRow["AcceptanceTime"] = objItem.AcceptanceTime;
                        addedCategories.Rows.Add(newRow);
                    }
                }
                else
                {
                    newRow = addedCategories.NewRow();
                    newRow["RouteSheet_Guid"] = objCRouteSheet.ID;
                    newRow["Rtt_Guid"] = System.Guid.Empty;
                    newRow["AcceptanceTime"] = 0;
                    addedCategories.Rows.Add(newRow);
                }
                addedCategories.AcceptChanges();

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AssignRouteSheetAcceptanceTime]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.AddWithValue("@tRouteSheetAcceptanceTime", addedCategories);
                cmd.Parameters["@tRouteSheetAcceptanceTime"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tRouteSheetAcceptanceTime"].TypeName = "dbo.udt_RouteSheetAcceptanceTime";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditRouteSheet]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheet_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheet_Number", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheet_Date", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheet_ShipDate", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Carrier_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Driver_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vehicle_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CarrierRateType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheet_Quantity", System.Data.SqlDbType.Float));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheet_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CarrierRateType_Value", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheetType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RegionDelivery_Guid", System.Data.SqlDbType.UniqueIdentifier));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RouteSheet_Guid"].Value = this.ID;
                cmd.Parameters["@RouteSheet_Number"].Value = this.Name;
                cmd.Parameters["@RouteSheet_Date"].Value = this.Date;
                cmd.Parameters["@RouteSheet_ShipDate"].Value = this.ShipDate;
                cmd.Parameters["@RouteSheet_IsActive"].Value = this.IsActive;
                cmd.Parameters["@RouteSheet_Quantity"].Value = this.Quantity;
                cmd.Parameters["@Carrier_Guid"].Value = this.Carrier.ID;
                cmd.Parameters["@Driver_Guid"].Value = this.Driver.ID;
                cmd.Parameters["@Vehicle_Guid"].Value = this.Vehicle.ID;
                cmd.Parameters["@CarrierRateType_Guid"].Value = this.RateType.ID;
                cmd.Parameters["@RouteSheetType_Guid"].Value = this.RouteSheetType.ID;
                cmd.Parameters["@CarrierRateType_Value"].Value = this.CarrierRateTypeValue;
                cmd.Parameters["@RegionDelivery_Guid"].Value = this.RegionDelivery.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    if ((this.AdvancedExpenseList != null) && (this.AdvancedExpenseList.Count > 0))
                    {
                        iRes = ((SaveAdvancedExpenseForRouteSheet(objProfile, cmd, this.AdvancedExpenseList, this, ref strErr) == true) ? 0 : -1);
                    }
                    if (iRes == 0)
                    {
                        if(this.RouteSheetWaybillList != null)
                        {
                            iRes = ((SaveWaybillListForRouteSheet(objProfile, cmd, this.RouteSheetWaybillList, this, ref strErr) == true) ? 0 : -1);
                        }
                    }
                    if (iRes == 0)
                    {
                        // информация о времени на разгрузку товара
                        if (this.AcceptanceTimeList != null)
                        {
                            iRes = ((SaveAcceptanceTimeForRouteSheet(objProfile, cmd, this.AcceptanceTimeList, this, ref strErr) == true) ? 0 : -1);
                        }
                    }
                    if (iRes == 0)
                    {
                        // подтверждаем транзакцию
                        DBTransaction.Commit();
                    }
                }
                if (iRes != 0)
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств записи о путевом листе.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства записи о путевом листе.\n\nТекст ошибки: " + f.Message, "Внимание",
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
            //System.Data.SqlClient.SqlTransaction DBTransaction = null;
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
                //DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandTimeout = ERP_Mercury.Global.Consts.iComandTimeOutForIB;

                //cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteRouteSheet]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheet_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RouteSheet_Guid"].Value = this.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                bRet = (iRes == 0);


                if (bRet == true)
                {
                    // подтверждаем транзакцию
                    //DBTransaction.Commit();
                }
                else
                {
                    // откатываем транзакцию
                    //DBTransaction.Rollback();
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления записи о путевом листе.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                //DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить запись о путевом листе.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }

        #endregion

        #region Оплата
        /// <summary>
        /// Оплата путевого листа
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="uuidRouteSheetId">уи путевого листа</param>
        /// <param name="dcmlPaymentValue">сумма платежа</param>
        /// <param name="dtPaymentDate">дата платежа</param>
        /// <param name="dcmlPaymentValueFact">сумма зарегистрированной оплаты</param>
        /// <param name="dcmlSaldoCurrent">сальдо после оплаты</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean PaymentRouteSheet(UniXP.Common.CProfile objProfile, System.Guid uuidRouteSheetId, System.Decimal dcmlPaymentValue,
            System.DateTime dtPaymentDate, ref System.Decimal dcmlPaymentValueFact, ref System.Decimal dcmlPaymentValueCurrent, ref System.Decimal dcmlSaldoCurrent, ref System.String strErr)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_PayRouteSheet]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheet_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheet_Payment", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheet_PaymentDate", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheet_PaymentOut", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheet_PaymentCurrentOut", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RouteSheet_SaldoOut", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@RouteSheet_PaymentOut"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RouteSheet_PaymentCurrentOut"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RouteSheet_SaldoOut"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@RouteSheet_Guid"].Value = uuidRouteSheetId;
                cmd.Parameters["@RouteSheet_Payment"].Value = dcmlPaymentValue;
                cmd.Parameters["@RouteSheet_PaymentDate"].Value = dtPaymentDate;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                dcmlPaymentValueFact = System.Convert.ToDecimal(cmd.Parameters["@RouteSheet_PaymentOut"].Value);
                dcmlPaymentValueCurrent = System.Convert.ToDecimal(cmd.Parameters["@RouteSheet_PaymentCurrentOut"].Value);
                dcmlSaldoCurrent = System.Convert.ToDecimal(cmd.Parameters["@RouteSheet_SaldoOut"].Value);

                if (iRes == 0)
                {
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                if (iRes != 0)
                {
                    DBTransaction.Rollback();
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                strErr = f.Message;
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
            return ( Name );
        }
    }
    #endregion
}
