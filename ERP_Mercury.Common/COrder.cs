using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;



namespace ERP_Mercury.Common
{
    /// <summary>
    /// Перечень состояний заказа
    /// </summary>
    public enum enPDASupplState
    {
        Unkown = -1,
        Created = 0,
        Deleted = 1,
        Transfered = 2,
        Processed = 3,
        CreditControl = 4,
        General = 5,
        CalcPricesFalse = 7,
        CreateSupplInIBFalse = 8,
        FindStock = 10,
        Print = 11,
        Confirm = 12,
        TTN = 13,
        Shipped = 14,
        CalcPricesOk = 60,
        AutoCalcPricesOk = 70,
        OutPartsOfStock = 80,
        MakedForRecalcPrices = 90
    }

    /// <summary>
    /// Класс "Состояние заказа"
    /// </summary>
    public class COrderState : CBusinessObject
    {
        #region Свойства
        [DisplayName("УИ в InterBase")]
        [Description("уникальный идентификатор записи в InterBase")]
        [Category("1. Обязательные значения")]
        [ReadOnly(true)]
        public System.Int32 StateId {get; set;}
        /// <summary>
        /// Примечание
        /// </summary>
        [DisplayName("Примечание")]
        [Description("Примечание")]
        [Category("2. Необязательные значения")]
        public System.String Description {get; set;}
        /// <summary>
        /// Признак "Активен"
        /// </summary>
        [DisplayName("Активен")]
        [Description("Признак активности записи")]
        [Category("1. Обязательные значения")]
        public System.Boolean IsActive {get; set;}
        #endregion

        #region Конструктор
        public COrderState()
            : base()
        {
            StateId = 0;
            Description = "";
            IsActive = true;
        }
        public COrderState(System.Guid uuidID, System.String strName, System.Int32 iStateId, System.String strDescription, 
            System.Boolean bIsActive)
        {
            ID = uuidID;
            Name = strName;
            StateId = iStateId;
            Description = strDescription;
            IsActive = bIsActive;
        }
        #endregion

        #region Список объектов
        public static List<COrderState> GetOrderStateList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<COrderState> objList = new List<COrderState>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetSupplState]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new COrderState(
                            (System.Guid)rs["SupplState_Guid"],
                            System.Convert.ToString(rs["SupplState_Name"]),
                            System.Convert.ToInt32(rs["SupplState_Id"]),
                            ((rs["SupplState_Description"] == System.DBNull.Value) ? null : System.Convert.ToString(rs["SupplState_Description"])),
                            System.Convert.ToBoolean(rs["SupplState_IsActive"])
                            )
                            );
                    }
                }

                //cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetOrderState]", objProfile.GetOptionsDllDBName());
                //cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                //cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                //cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                //cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                //System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                //if (rs.HasRows)
                //{
                //    while (rs.Read())
                //    {
                //        objList.Add(new COrderState(
                //            (System.Guid)rs["OrderState_Guid"],
                //            System.Convert.ToString(rs["OrderState_Name"]),
                //            System.Convert.ToInt32(rs["OrderState_Id"]),
                //            ((rs["OrderState_Description"] == System.DBNull.Value) ? null : System.Convert.ToString(rs["OrderState_Description"])),
                //            System.Convert.ToBoolean(rs["OrderState_IsActive"])
                //            )
                //            );
                //    }
                //}
                
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
                "Не удалось получить список единиц измерения.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                    "Необходимо указать наименование единицы измерения!", "Внимание",
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddOrderState]", objProfile.GetOptionsDllDBName());

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OrderState_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OrderState_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OrderState_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OrderState_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OrderState_Id", System.Data.SqlDbType.Int));


                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));//**
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@OrderState_Name"].Value = this.Name;
                cmd.Parameters["@OrderState_Description"].Value = this.Description;
                cmd.Parameters["@OrderState_IsActive"].Value = this.IsActive;
                cmd.Parameters["@OrderState_Id"].Value = this.StateId;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["OrderState_Guid"].Value; 
                }
                else
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }


                if (iRes == 0)
                {
                    if (DBTransaction != null)
                    {
                        // подтверждаем транзакцию
                        DBTransaction.Commit();
                    }
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка добавления записи в справочник состояний зазказа.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                if (DBTransaction != null)
                {
                    DBTransaction.Rollback();
                }
                strErr = f.Message;
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка добавления записи в справочник состояний зазказа.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteOrderState]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OrderState_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@OrderState_Guid"].Value = this.ID;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes != 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления записи в справочнике состояний заказа.\n\nТекст ошибки: " +
                        (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

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
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка удаления записи в справочнике состояний заказа.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditOrderState]", objProfile.GetOptionsDllDBName());

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OrderState_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OrderState_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OrderState_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OrderState_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OrderState_Id", System.Data.SqlDbType.Int));


                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));//**
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@OrderState_Guid"].Value = this.ID;
                cmd.Parameters["@OrderState_Name"].Value = this.Name;
                cmd.Parameters["@OrderState_Description"].Value = this.Description;
                cmd.Parameters["@OrderState_IsActive"].Value = this.IsActive;
                cmd.Parameters["@OrderState_Id"].Value = this.StateId;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                if (iRes == 0)
                {
                    if (DBTransaction != null)
                    {
                        // подтверждаем транзакцию
                        DBTransaction.Commit();
                    }
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения записи в справочнике состояний заказа.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                if (DBTransaction != null)
                {
                    DBTransaction.Rollback();
                }
                strErr = f.Message;
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка изменения записи в справочнике состояний заказа.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
    /// Класс "Вид платежа"
    /// </summary>
    public class CPaymentType
    {
        #region Переменные, свойства
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        private System.Guid m_uuidId;
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public System.Guid ID
        {
            get { return m_uuidId; }
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
        }
        public System.Int32 Payment_Id { get; set; }
        #endregion

        #region Конструктор
        public CPaymentType(System.Guid uuidId, System.String strName)
        {
            m_uuidId = uuidId;
            m_strName = strName;
            Payment_Id = 0;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список типов платежей
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="uuidPaymentTypeID">УИ типа платежа</param>
        /// <returns>список типов платежей</returns>
        public static List<CPaymentType> GetPaymentTypeList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidPaymentTypeID)
        {
            List<CPaymentType> objList = new List<CPaymentType>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPaymentType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PaymentType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                if (uuidPaymentTypeID.CompareTo(System.Guid.Empty) == 0)
                {
                    cmd.Parameters["@PaymentType_Guid"].IsNullable = true;
                    cmd.Parameters["@PaymentType_Guid"].Value = null;

                }
                else
                {
                    cmd.Parameters["@PaymentType_Guid"].IsNullable = false;
                    cmd.Parameters["@PaymentType_Guid"].Value = uuidPaymentTypeID;
                }
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CPaymentType((System.Guid)rs["PaymentType_Guid"], (System.String)rs["PaymentType_Name"]) { Payment_Id = System.Convert.ToInt32(rs["PaymentType_Id"]) });
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
                "Не удалось получить список типов платежей.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        #endregion

        public override string ToString()
        {
            return Name;
        }

    }

    /// <summary>
    /// Договор для заказа
    /// </summary>
    public class CStmnt
    {
        #region Свойства
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public System.Guid ID { get; set; }
        /// <summary>
        /// Номер
        /// </summary>
        public System.String FullNum { get; set; }
        /// <summary>
        /// Дата
        /// </summary>
        public System.DateTime BeginDate { get; set; }
        /// <summary>
        /// Печатать скидку в ТТН
        /// </summary>
        public System.Boolean IsPrintDiscountInWaybill { get; set; }
        /// <summary>
        /// Блокировка предоставления скидок
        /// </summary>
        public System.Boolean IsBlockDiscount { get; set; }

        #endregion

        #region Конструктор
        public CStmnt()
        {
            ID = System.Guid.Empty;
            FullNum = "";
            BeginDate = System.DateTime.MinValue;
            IsBlockDiscount = false;
            IsPrintDiscountInWaybill = false;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список договоров
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="uuidCustomerID">уи клиента</param>
        /// <param name="uuidCompanyID">уи компании</param>
        /// <param name="uuidOrderID">уи заказа</param>
        /// <returns>список договоров</returns>
        public static List<CStmnt> GetStmntList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidCustomerID, System.Guid uuidCompanyID, System.Guid uuidOrderID)
        {
            List<CStmnt> objList = new List<CStmnt>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetStmnt]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                if (uuidCustomerID.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@Customer_Guid"].IsNullable = false;
                    cmd.Parameters["@Customer_Guid"].Value = uuidCustomerID;
                }
                if (uuidCompanyID.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@Company_Guid"].IsNullable = false;
                    cmd.Parameters["@Company_Guid"].Value = uuidCompanyID;
                }
                if (uuidOrderID.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Order_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@Order_Guid"].IsNullable = false;
                    cmd.Parameters["@Order_Guid"].Value = uuidOrderID;
                }
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(
                            new CStmnt()
                                {
                                    ID = (System.Guid)rs["AgreementWithCustomer_Guid"],
                                    FullNum = (System.String)rs["Stmnt_FullNum"],
                                    BeginDate = System.Convert.ToDateTime(rs["Stmnt_BeginDate"]),
                                    IsBlockDiscount = System.Convert.ToBoolean(rs["Stmnt_BlockDiscount"]),
                                    IsPrintDiscountInWaybill = System.Convert.ToBoolean(rs["Stmnt_PrintDiscountInWaybill"])
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
                "Не удалось получить список договоров.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        #endregion

        public override string ToString()
        {
            return (String.Format("{0} от {1}", FullNum.Trim(), BeginDate.ToShortDateString()));
        }

    }

    /// <summary>
    /// Класс "Строка в приложении к заказу"
    /// </summary>
    public class COrderItem
    {
        #region Свойства
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public System.Guid ID { get; set; }
        /// <summary>
        /// Товар
        /// </summary>
        public CProduct Product { get; set; }
        /// <summary>
        /// Наименование товара
        /// </summary>
        public System.String ProductFullName
        {
            get { return ((Product == null) ? "" : Product.ProductFullName); }
            set { ProductFullName = value; }
        }
        /// <summary>
        /// Единица измерения
        /// </summary>
        public CMeasure Measure { get; set; }
        /// <summary>
        /// Заказанное количество
        /// </summary>
        public System.Double QuantityOrdered { get; set; }
        /// <summary>
        /// Цена первого поставщика
        /// </summary>
        public System.Double PriceImporter { get; set; }
        /// <summary>
        /// Цена отпускная
        /// </summary>
        public System.Double Price { get; set; }
        /// <summary>
        /// Размер скидки в процентах
        /// </summary>
        public System.Double DiscountPercent { get; set; }
        /// <summary>
        /// Цена отпускная с учетом скидки
        /// </summary>
        public System.Double PriceWithDiscount { get; set; }
        /// <summary>
        /// Ставка НДС в процентах
        /// </summary>
        public System.Double NDSPercent { get; set; }
        /// <summary>
        /// Сумма заказа
        /// </summary>
        public System.Double SumOrdered { get { return (Price * QuantityOrdered); } }
        /// <summary>
        /// Сумма заказа с учетом скидки
        /// </summary>
        public System.Double SumOrderedWithDiscount { get { return (PriceWithDiscount * QuantityOrdered); } }
        /// <summary>
        /// Отпускная цена в валюте учета
        /// </summary>
        public System.Double PriceInAccountingCurrency { get; set; }
        /// <summary>
        /// Отпускная цена с учетом скидки в валюте учета
        /// </summary>
        public System.Double PriceWithDiscountInAccountingCurrency { get; set; }
        /// <summary>
        /// Сумма заказа в валюте учета
        /// </summary>
        public System.Double SumOrderedInAccountingCurrency { get { return (PriceInAccountingCurrency * QuantityOrdered); } }
        /// <summary>
        /// Сумма заказа в валюте учета со скидкой
        /// </summary>
        public System.Double SumOrderedWithDiscountInAccountingCurrency { get { return (PriceWithDiscountInAccountingCurrency * QuantityOrdered); } }
        /// <summary>
        /// Зарезервированное количество
        /// </summary>
        public System.Double QuantityReserved { get; set; }
        /// <summary>
        /// Сумма резерва
        /// </summary>
        public System.Double SumReserved { get { return (Price * QuantityReserved); } }
        /// <summary>
        /// Сумма резерва с учетом скидки
        /// </summary>
        public System.Double SumReservedWithDiscount { get { return (PriceWithDiscount * QuantityReserved); } }
        /// <summary>
        /// Сумма резерва в валюте учета
        /// </summary>
        public System.Double SumReservedInAccountingCurrency { get { return (PriceInAccountingCurrency * QuantityReserved); } }
        /// <summary>
        /// Сумма резерва с учетом скидки в валюте учета
        /// </summary>
        public System.Double SumReservedWithDiscountInAccountingCurrency { get { return (PriceWithDiscountInAccountingCurrency * QuantityReserved); } }
        /// <summary>
        /// Количество в остатке
        /// </summary>
        public System.Double QuantityInstock { get; set; }

        #endregion

        #region Конструктор
        public COrderItem()
        {
            ID = System.Guid.Empty;
            Product = null;
            Measure = null;
            QuantityOrdered = 0;
            QuantityReserved = 0;
            DiscountPercent = 0;
            NDSPercent = 0;
            PriceImporter = 0;
            Price = 0;
            PriceWithDiscount = 0;
            PriceInAccountingCurrency = 0;
            PriceWithDiscountInAccountingCurrency = 0;
            QuantityInstock = 0;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Преобразует список записей к табличному виду
        /// </summary>
        /// <param name="objOrderItemsList">список строк из заказа</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>таблица с приложением к заказу</returns>
        public static System.Data.DataTable ConvertListToTable(List<COrderItem> objOrderItemsList, ref System.String strErr)
        {
            System.Data.DataTable objTable = new System.Data.DataTable();
            try
            {
                objTable.Columns.Add(new System.Data.DataColumn("Orderitms_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                objTable.Columns.Add(new System.Data.DataColumn("Parts_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                objTable.Columns.Add(new System.Data.DataColumn("Measure_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                objTable.Columns.Add(new System.Data.DataColumn("OrderItms_Quantity", typeof(float)));
                objTable.Columns.Add(new System.Data.DataColumn("OrderItms_QuantityOrdered", typeof(float)));
                objTable.Columns.Add(new System.Data.DataColumn("OrderItms_PriceImporter", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("OrderItms_Price", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("OrderItms_DiscountPercent", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("OrderItms_PriceWithDiscount", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("OrderItms_NDSPercent", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("OrderItms_PriceInAccountingCurrency", typeof(System.Data.SqlTypes.SqlMoney)));
                objTable.Columns.Add(new System.Data.DataColumn("OrderItms_PriceWithDiscountInAccountingCurrency", typeof(System.Data.SqlTypes.SqlMoney)));

                System.Data.DataRow newRow = null;
                foreach (COrderItem objItem in objOrderItemsList)
                {
                    newRow = objTable.NewRow();
                    newRow["Orderitms_Guid"] = objItem.ID;
                    newRow["Parts_Guid"] = objItem.Product.ID;
                    newRow["Measure_Guid"] = objItem.Measure.ID;
                    newRow["OrderItms_Quantity"] = objItem.QuantityReserved;
                    newRow["OrderItms_QuantityOrdered"] = objItem.QuantityOrdered;
                    newRow["OrderItms_PriceImporter"] = (System.Data.SqlTypes.SqlMoney)objItem.PriceImporter;
                    newRow["OrderItms_Price"] = (System.Data.SqlTypes.SqlMoney)objItem.Price;
                    newRow["OrderItms_DiscountPercent"] = (System.Data.SqlTypes.SqlMoney)objItem.DiscountPercent;
                    newRow["OrderItms_PriceWithDiscount"] = (System.Data.SqlTypes.SqlMoney)objItem.PriceWithDiscount;
                    newRow["OrderItms_NDSPercent"] = (System.Data.SqlTypes.SqlMoney)objItem.NDSPercent;
                    newRow["OrderItms_PriceInAccountingCurrency"] = (System.Data.SqlTypes.SqlMoney)objItem.PriceInAccountingCurrency;
                    newRow["OrderItms_PriceWithDiscountInAccountingCurrency"] = (System.Data.SqlTypes.SqlMoney)objItem.PriceWithDiscountInAccountingCurrency;
                    objTable.Rows.Add(newRow);
                }
                if (objOrderItemsList.Count > 0)
                {
                    objTable.AcceptChanges();
                }

            }
            catch (System.Exception f)
            {
                objTable = null;
                strErr += (String.Format("ConvertListToTable. Текст ошибки: {0}", f.Message));
            }
			finally // очищаем занимаемые ресурсы
            {
            }
            return objTable;
        }

        #endregion
    }

    /// <summary>
    /// Класс "Заказ"
    /// </summary>
    public class COrder
    {
        #region Свойства
        /// <summary>
        /// Уникальный идентификатор заказа
        /// </summary>
        public System.Guid ID { get; set; }
        /// <summary>
        /// Уникальный идентификатор заказа в InterBase
        /// </summary>
        public System.Int32 Ib_ID { get; set; }
        /// <summary>
        /// Номер
        /// </summary>
        public System.Int32 Num { get; set; }
        /// <summary>
        /// Версия
        /// </summary>
        public System.Int32 SubNum { get; set; }
        /// <summary>
        /// Дата создания заказа
        /// </summary>
        public System.DateTime BeginDate { get; set; }
        /// <summary>
        /// Дата доставки заказа
        /// </summary>
        public System.DateTime DeliveryDate { get; set; }
        /// <summary>
        /// Признак "Заказ является бонусом"
        /// </summary>
        public System.Boolean IsBonus { get; set; }
        /// <summary>
        /// Клиент
        /// </summary>
        public CCustomer Customer { get; set; }
        /// <summary>
        /// Наименование клиента
        /// </summary>
        public System.String CustomerName
        {
            get { return ((Customer == null) ? "" : Customer.FullName); }
        }
        /// <summary>
        /// Дочернее подразделение
        /// </summary>
        public CChildDepart ChildDepart { get; set; }
        /// <summary>
        /// Код дочернего подразделения
        /// </summary>
        public System.String ChildDepartCode
        {
            get { return ((ChildDepart == null) ? System.String.Empty : ChildDepart.Code); }
        }
        /// <summary>
        /// Торговый представитель
        /// </summary>
        public CSalesMan SalesMan { get; set; }
        /// <summary>
        /// Фамилия торгового представителя
        /// </summary>
        public System.String SalesManName
        {
            get { return (((SalesMan == null) || (SalesMan.User == null)) ? "" : SalesMan.User.LastName); }
        }
        /// <summary>
        /// Подразделение
        /// </summary>
        public CDepart Depart { get; set; }
        /// <summary>
        /// Код подразделения
        /// </summary>
        public System.String DepartCode
        {
            get { return ((Depart == null) ? "" : Depart.DepartCode); }
        }
        /// <summary>
        /// Склад
        /// </summary>
        public CStock Stock { get; set; }
        /// <summary>
        /// Наименование склада
        /// </summary>
        public System.String StockName
        {
            get { return ((Stock == null) ? "" : Stock.Name); }
        }
        /// <summary>
        /// Наименование компании
        /// </summary>
        public System.String CompanyAbbr
        {
            get { return ((Stock == null) ? "" : Stock.CompanyAbbr); }
        }
        /// <summary>
        /// Товар - для виртуального набора
        /// </summary>
        public CProduct Product { get; set; }
        /// <summary>
        /// Розничная торговая точка
        /// </summary>
        public CRtt Rtt { get; set; }
        /// <summary>
        /// Адрес доставки
        /// </summary>
        public CAddress AddressDelivery { get; set; }
        /// <summary>
        /// Состояние заказа
        /// </summary>
        public COrderState OrderState { get; set; }
        /// <summary>
        /// Наименование состояния заказа
        /// </summary>
        public System.String OrderStateName
        {
            get { return ((OrderState == null) ? "" : OrderState.Name); }
        }
        /// <summary>
        /// Код состояния заказа
        /// </summary>
        public System.Int32 OrderStateId
        {
            get { return ((OrderState == null) ? -1 : OrderState.StateId); }
        }
        /// <summary>
        /// Тип заказа
        /// </summary>
        public COrderType OrderType { get; set; }
        /// <summary>
        /// Наименование типа заказа
        /// </summary>
        public System.String OrderTypeName
        {
            get { return ((OrderType == null) ? "" : OrderType.Name); }
        }
        /// <summary>
        /// Форма оплаты
        /// </summary>
        public CPaymentType PaymentType { get; set; }
        /// <summary>
        /// Наименование формы оплаты
        /// </summary>
        public System.String PaymentTypeName
        {
            get { return ((PaymentType == null) ? "" : PaymentType.Name); }
        }
        /// <summary>
        /// Примечание
        /// </summary>
        public System.String Description { get; set; }
        /// <summary>
        /// Приложение к заказу
        /// </summary>
        public List<COrderItem> OrderItemList { get; set; }
        /// <summary>
        /// Количество позиций
        /// </summary>
        public System.Double PosQuantity { get; set; }
        /// <summary>
        /// Заказанное количество
        /// </summary>
        public System.Double QuantityOrdered { get; set; }
        /// <summary>
        /// Сумма заказа
        /// </summary>
        public System.Double SumOrdered { get; set; }
        /// <summary>
        /// Сумма заказа с учетом скидки
        /// </summary>
        public System.Double SumOrderedWithDiscount { get; set; }
        /// <summary>
        /// Сумма заказа в валюте учета
        /// </summary>
        public System.Double SumOrderedInAccountingCurrency { get; set; }
        /// <summary>
        /// Сумма заказа с учетом скидки в валюте учета
        /// </summary>
        public System.Double SumOrderedWithDiscountInAccountingCurrency { get; set; }
        /// <summary>
        /// Зарезервированное количество
        /// </summary>
        public System.Double QuantityReserved { get; set; }
        /// <summary>
        /// Сумма резерва
        /// </summary>
        public System.Double SumReserved { get; set; }
        /// <summary>
        /// Сумма резерва с учетом скидки
        /// </summary>
        public System.Double SumReservedWithDiscount { get; set; }
        /// <summary>
        /// Сумма резерва в валюте учета
        /// </summary>
        public System.Double SumReservedInAccountingCurrency { get; set; }
        /// <summary>
        /// Сумма резерва с учетом скидки в валюте учета
        /// </summary>
        public System.Double SumReservedWithDiscountInAccountingCurrency { get; set; }
        /// <summary>
        /// Направление доставки (наименование)
        /// </summary>
        public System.String DirectionDeliveryName { get; set; }
        /// <summary>
        /// Направление доставки (населенный пункт)
        /// </summary>
        public System.String DirectionDeliveryCityName { get; set; }

        #endregion

        #region Конструктор
        public COrder()
        {
            ID = System.Guid.Empty;
            Ib_ID = 0;
            Description = "";
            IsBonus = false;
            Num = 0;
            SubNum = 0;
            BeginDate = System.DateTime.MinValue;
            DeliveryDate = System.DateTime.Today;
            OrderItemList = null;
            OrderState = null;
            OrderType = null;
            PaymentType = null;
            Customer = null;
            ChildDepart = null;
            Depart = null;
            SalesMan = null;
            AddressDelivery = null;
            Rtt = null;
            Product = null;
            PosQuantity = 0;
            QuantityOrdered = 0;
            SumOrdered = 0;
            SumOrderedWithDiscount = 0;
            SumOrderedInAccountingCurrency = 0;
            SumOrderedWithDiscountInAccountingCurrency = 0;
            QuantityReserved = 0;
            SumReserved = 0;
            SumReservedWithDiscount = 0;
            SumReservedInAccountingCurrency = 0;
            SumReservedWithDiscountInAccountingCurrency = 0;
            DirectionDeliveryName = System.String.Empty;
            DirectionDeliveryCityName = System.String.Empty;
        }
        #endregion

        #region Итоговые суммы
        /// <summary>
        /// Вычисление заказанного количества по документу
        /// </summary>
        /// <returns>заказанное количество по документу</returns>
        public System.Double CalculateQuantityOrdered()
        {
            System.Double dblRet = 0;
            try
            {
                dblRet = ((OrderItemList == null) ? 0 : OrderItemList.Sum(p => p.QuantityOrdered));
            }
            catch
            {
                dblRet = 0;
            }
			finally // очищаем занимаемые ресурсы
            {
            }
            return dblRet;
        }
        /// <summary>
        /// Вычисление зарезервированного количества по документу
        /// </summary>
        /// <returns>зарезервированное количество по документу</returns>
        public System.Double CalculateQuantityReserved()
        {
            System.Double dblRet = 0;
            try
            {
                dblRet = ((OrderItemList == null) ? 0 : OrderItemList.Sum(p => p.QuantityReserved));
            }
            catch
            {
                dblRet = 0;
            }
			finally // очищаем занимаемые ресурсы
            {
            }
            return dblRet;
        }
        /// <summary>
        /// Вычисление суммы заказа
        /// </summary>
        /// <returns>сумма заказа</returns>
        public System.Double CalculateSumOrdered()
        {
            System.Double dblRet = 0;
            try
            {
                dblRet = ((OrderItemList == null) ? 0 : OrderItemList.Sum(p=>p.SumOrdered));
            }
            catch
            {
                dblRet = 0; 
            }
			finally // очищаем занимаемые ресурсы
            {
            }
            return dblRet;
        }
        /// <summary>
        /// Вычисление суммы резерва по заказу
        /// </summary>
        /// <returns>сумма резерва по заказу</returns>
        public System.Double CalculateSumReserved()
        {
            System.Double dblRet = 0;
            try
            {
                dblRet = ((OrderItemList == null) ? 0 : OrderItemList.Sum(p => p.SumReserved));
            }
            catch
            {
                dblRet = 0;
            }
			finally // очищаем занимаемые ресурсы
            {
            }
            return dblRet;
        }
        /// <summary>
        /// Вычисление суммы заказа с учетом скидки
        /// </summary>
        /// <returns>сумма заказа с учетом скидки</returns>
        public System.Double CalculateSumOrderedWithDiscount()
        {
            System.Double dblRet = 0;
            try
            {
                dblRet = ((OrderItemList == null) ? 0 : OrderItemList.Sum(p => p.SumOrderedWithDiscount));
            }
            catch 
            {
                dblRet = 0;
            }
			finally // очищаем занимаемые ресурсы
            {
            }
            return dblRet;
        }
        /// <summary>
        /// Вычисление суммы резерва по заказу с учетом скидки
        /// </summary>
        /// <returns>сумма резерва по заказу с учетом скидки</returns>
        public System.Double CalculateSumReservedWithDiscount()
        {
            System.Double dblRet = 0;
            try
            {
                dblRet = ((OrderItemList == null) ? 0 : OrderItemList.Sum(p => p.SumReservedWithDiscount));
            }
            catch
            {
                dblRet = 0;
            }
			finally // очищаем занимаемые ресурсы
            {
            }
            return dblRet;
        }
        /// <summary>
        /// Вычисление суммы заказа в валюте учета
        /// </summary>
        /// <returns>сумма заказа в валюте учета</returns>
        public System.Double CalculateSumOrderedInAccountingCurrency()
        {
            System.Double dblRet = 0;
            try
            {
                dblRet = ((OrderItemList == null) ? 0 : OrderItemList.Sum(p => p.SumOrderedInAccountingCurrency));
            }
            catch
            {
                dblRet = 0;
            }
			finally // очищаем занимаемые ресурсы
            {
            }
            return dblRet;
        }
        /// <summary>
        /// Вычисление суммы резерва по заказу в валюте учета
        /// </summary>
        /// <returns>сумма резерва по заказу в валюте учета</returns>
        public System.Double CalculateSumReservedInAccountingCurrency()
        {
            System.Double dblRet = 0;
            try
            {
                dblRet = ((OrderItemList == null) ? 0 : OrderItemList.Sum(p => p.SumReservedInAccountingCurrency));
            }
            catch
            {
                dblRet = 0;
            }
			finally // очищаем занимаемые ресурсы
            {
            }
            return dblRet;
        }
        /// <summary>
        /// Вычисление суммы заказа с учетом скидки в валюте учета
        /// </summary>
        /// <returns>сумма заказа с учетом скидки в валюте учета</returns>
        public System.Double CalculateSumOrderedWithDiscountInAccountingCurrency()
        {
            System.Double dblRet = 0;
            try
            {
                dblRet = ((OrderItemList == null) ? 0 : OrderItemList.Sum(p => p.SumOrderedWithDiscountInAccountingCurrency));
            }
            catch
            {
                dblRet = 0;
            }
			finally // очищаем занимаемые ресурсы
            {
            }
            return dblRet;
        }
        /// <summary>
        /// Вычисление суммы резерва по заказу с учетом скидки в валюте учета
        /// </summary>
        /// <returns>сумма резерва по заказу с учетом скидки в валюте учета</returns>
        public System.Double CalculateSumReservedWithDiscountInAccountingCurrency()
        {
            System.Double dblRet = 0;
            try
            {
                dblRet = ((OrderItemList == null) ? 0 : OrderItemList.Sum(p => p.SumReservedWithDiscountInAccountingCurrency));
            }
            catch
            {
                dblRet = 0;
            }
			finally // очищаем занимаемые ресурсы
            {
            }
            return dblRet;
        }
        #endregion

        #region Реквизиты по умолчанию при оформлении нового заказа

        public static System.Boolean GetOrderDefParams(UniXP.Common.CProfile objProfile, 
            System.Guid In_OrderType_Guid, System.Guid In_PaymentType_Guid, ref System.Boolean SetChildDepartNull, ref System.Boolean SetDepartValue,
            ref System.Boolean SetChildDepartValue, ref System.Guid Depart_Guid, ref System.Guid OrderType_Guid, ref System.Guid PaymentType_Guid, 
            ref System.String strErr)
        {
            System.Boolean bRet = false;

            bRet = COrderRepository.GetOrderDefParams( objProfile, null,
                 In_OrderType_Guid,  In_PaymentType_Guid, ref SetChildDepartNull, ref SetDepartValue, ref SetChildDepartValue,
                ref Depart_Guid, ref OrderType_Guid, ref PaymentType_Guid, ref strErr);

            return bRet;

        }

        #endregion

        #region Проверка на возможность редактирования заказа
        /// <summary>
        /// Возвращает признак того, можно ли редактировать заказ
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="Suppl_Guid">УИ заказа</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - редактировать можно; false - редактировать нельзя</returns>
        public static System.Boolean IsPossibleEditOrder(UniXP.Common.CProfile objProfile, System.Guid Suppl_Guid, ref System.String strErr)
        {
            System.Boolean bRet = false;

            bRet = COrderRepository.IsPossibleEditOrder(objProfile, null, Suppl_Guid, ref strErr);

            return bRet;

        }

        #endregion
    }

    #region Бланк заказа
    public struct OrderBlankCustomerInfo
    {
        public System.Int32 CustomerId;
        public System.String CustomerName;
        public System.String RttCode;
        public System.String RttAddress;
    }

    public struct OrderBlankProductInfo
    {
        public System.String ProductArticle;
        public System.String ProductName;
        public System.Double ProductStockQty;
        public System.Double ProductPackQty;
        public System.Double ProductPrice;
    }
    #endregion

}
