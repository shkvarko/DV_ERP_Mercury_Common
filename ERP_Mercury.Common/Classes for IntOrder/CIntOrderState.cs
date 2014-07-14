using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace ERP_Mercury.Common
{
    /// <summary>
    /// Класс "Состояние заказа на внутреннее перемещение"
    /// </summary>
    public class CIntOrderState : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Примечание
        /// </summary>
        [DisplayName("Примечание")]
        [Description("Примечание")]
        [Category("2. Необязательные значения")]
        public System.String Description { get; set; }
        /// <summary>
        /// Номер в очереди состояний
        /// </summary>
        [DisplayName("Код состояния заказа на внутреннее перемещение")]
        [Description("Целочисленный уникальный идентификатор состояния заказа на внутреннее перемещение")]
        [Category("1. Обязательные значения")]
        public System.Int32 IntOrderStateId { get; set; }
        /// <summary>
        /// Признак "Активен"
        /// </summary>
        [DisplayName("Активен")]
        [Description("Признак активности записи")]
        [Category("1. Обязательные значения")]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public System.Boolean IsActive { get; set; }
        /// <summary>
        /// Признак "По умолчанию"
        /// </summary>
        [DisplayName("По умолчанию")]
        [Description("Если свойство установлено в \"Да\", то состояние устанавливается по умолчанию в новых документах")]
        [Category("1. Обязательные значения")]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public System.Boolean IsDefault { get; set; }
        #endregion

        #region Конструктор
        public CIntOrderState()
            : base()
        {
            IntOrderStateId = -1;
            IsActive = false;
            Description = System.String.Empty;
            IsDefault = false;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список состояний накладной
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>список состояний заказов на внутреннее перемещение</returns>
        public static List<CIntOrderState> GetIntOrderStateList(UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            List<CIntOrderState> objList = new List<CIntOrderState>();

            try
            {
                // вызов статического метода из класса, связанного с БД
                System.Data.DataTable dtList = CIntOrderStateDataBaseModel.GetIntOrderStateList(objProfile, null, ref strErr);
                if (dtList != null)
                {
                    CIntOrderState objListItem = null;
                    foreach (System.Data.DataRow objItem in dtList.Rows)
                    {
                        objListItem = new CIntOrderState();
                        objListItem.ID = new Guid(System.Convert.ToString(objItem["IntOrderState_Guid"]));
                        objListItem.Name = ((objItem["IntOrderState_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(objItem["IntOrderState_Name"]));
                        objListItem.Description = ((objItem["IntOrderState_Description"] == System.DBNull.Value) ? "" : System.Convert.ToString(objItem["IntOrderState_Description"]));
                        objListItem.IntOrderStateId = ((objItem["IntOrderState_Id"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(System.Convert.ToString(objItem["IntOrderState_Id"])));

                        if (objItem["IntOrderState_IsActive"] != System.DBNull.Value)
                        {
                            objListItem.IsActive = System.Convert.ToBoolean(System.Convert.ToString(objItem["IntOrderState_IsActive"]));
                        }

                        if (objItem["IntOrderState_IsDefault"] != System.DBNull.Value)
                        {
                            objListItem.IsDefault = System.Convert.ToBoolean(System.Convert.ToString(objItem["IntOrderState_IsDefault"]));
                        }

                        objList.Add(objListItem);
                    }
                }

                dtList = null;
            }

            catch (System.Exception f)
            {
                strErr += (String.Format(" {0}", f.Message));
            }
            return objList;
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
            System.String strErr = "";

            System.Boolean bRet = CIntOrderStateDataBaseModel.RemoveObjectFromDataBase(this.ID, objProfile, ref strErr);
            if (bRet == false)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Внимание",
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
            System.String strErr = "";

            System.Guid GUID_ID = System.Guid.Empty;

            System.Boolean bRet = CIntOrderStateDataBaseModel.AddNewObjectToDataBase(
                this.Name, this.Description, this.IsActive, this.IsDefault,
                this.IntOrderStateId, ref GUID_ID, objProfile, ref strErr);
            if (bRet == true)
            {
                this.ID = GUID_ID;
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
            System.String strErr = "";

            System.Boolean bRet = CIntOrderStateDataBaseModel.EditObjectInDataBase(this.ID,
                this.Name, this.Description, this.IsActive, this.IsDefault, this.IntOrderStateId, objProfile, ref strErr);
            if (bRet == false)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
    /// Класс для работы с базой данных
    /// </summary>
    public static class CIntOrderStateDataBaseModel
    {
        #region Добавление новой записи
        /// <summary>
        /// Проверка свойств объекта перед сохранением в базе данных
        /// </summary>
        /// <param name="IntWaybillState_Name">наименование состояния заказа на внутреннее перемещение</param>
        /// <param name="IntWaybillState_Id">код состояния заказа на внутреннее перемещение</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - проверка пройдена; false - проверка НЕ пройдена</returns>
        public static System.Boolean IsAllParametersValid(System.String IntWaybillState_Name, System.Int32 IntWaybillState_Id,
            ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (IntWaybillState_Name.Trim() == "")
                {
                    strErr += ("Необходимо указать наименование состояния заказа на внутреннее перемещение!");
                    return bRet;
                }
                if (IntWaybillState_Id < 0)
                {
                    strErr += ("Необходимо указать код состояния заказа на внутреннее перемещение!");
                    return bRet;
                }
                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr += ("Ошибка проверки свойств объекта \"состояние заказа на внутреннее перемещение\". Текст ошибки: " + f.Message);
            }
            return bRet;
        }

        /// <summary>
        /// Добавление новой записи с описанием состояния заказа на внутреннее перемещение в базу данных
        /// </summary>
        /// <param name="IntWaybillState_Name">наименование состояния заказа на внутреннее перемещение</param>
        /// <param name="IntWaybillState_Description">примечание</param>
        /// <param name="IntWaybillState_IsActive">признак "запись активна"</param>
        /// <param name="IntWaybillState_IsDefault">признак "использовать по умолчанию"</param>
        /// <param name="IntWaybillState_Id">код вида оплаты</param>
        /// <param name="IntWaybillState_Guid">УИ записи</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean AddNewObjectToDataBase(System.String IntOrderState_Name,
            System.String IntOrderState_Description, System.Boolean IntOrderState_IsActive,
            System.Boolean IntOrderState_IsDefault,
            System.Int32 IntOrderState_Id, ref System.Guid IntOrderState_Guid,
            UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;

            if (IsAllParametersValid(IntOrderState_Name, IntOrderState_Id, ref strErr) == false) { return bRet; }

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;

            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    strErr += ("Не удалось получить соединение с базой данных.");
                    return bRet;
                }
                DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddIntOrderState]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderState_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderState_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderState_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderState_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderState_IsDefault", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderState_Id", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@IntOrderState_Name"].Value = IntOrderState_Name;
                cmd.Parameters["@IntOrderState_Description"].Value = IntOrderState_Description;
                cmd.Parameters["@IntOrderState_IsActive"].Value = IntOrderState_IsActive;
                cmd.Parameters["@IntOrderState_IsDefault"].Value = IntOrderState_IsDefault;
                cmd.Parameters["@IntOrderState_Id"].Value = IntOrderState_Id;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    IntOrderState_Guid = (System.Guid)cmd.Parameters["@IntOrderState_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr += ((cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();

                strErr += ("Не удалось создать объект \"состояние заказа на внутреннее перемещение\". Текст ошибки: " + f.Message);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }

        #endregion

        #region Редактировать объект в базе данных
        /// <summary>
        /// Редактирование записи в базе данных
        /// </summary>
        /// <param name="IntOrderState_Guid">УИ записи</param>
        /// <param name="IntOrderState_Name">наименование состояния заказа на внутреннее перемещение</param>
        /// <param name="IntOrderState_Description">примечание</param>
        /// <param name="IntOrderState_IsActive">признак "запись активна"</param>
        /// <param name="IntOrderState_IsDefault">признак "использовать по умолчанию"</param>
        /// <param name="IntOrderState_Id">код вида оплаты</param>
        /// <param name="IntOrderState_Guid">УИ записи</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean EditObjectInDataBase(System.Guid IntOrderState_Guid, System.String IntOrderState_Name,
            System.String IntOrderState_Description, System.Boolean IntOrderState_IsActive,
            System.Boolean IntOrderState_IsDefault, System.Int32 IntOrderState_Id,
            UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;

            if (IsAllParametersValid(IntOrderState_Name, IntOrderState_Id, ref strErr) == false) { return bRet; }

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;

            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    strErr += ("Не удалось получить соединение с базой данных.");
                    return bRet;
                }
                DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditIntOrderState]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderState_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderState_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderState_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderState_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderState_IsDefault", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderState_Id", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@IntOrderState_Guid"].Value = IntOrderState_Guid;
                cmd.Parameters["@IntOrderState_Name"].Value = IntOrderState_Name;
                cmd.Parameters["@IntOrderState_Description"].Value = IntOrderState_Description;
                cmd.Parameters["@IntOrderState_IsActive"].Value = IntOrderState_IsActive;
                cmd.Parameters["@IntOrderState_IsDefault"].Value = IntOrderState_IsDefault;
                cmd.Parameters["@IntOrderState_Id"].Value = IntOrderState_Id;
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
                    strErr += ((cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();

                strErr += ("Не удалось внести изменения в объект \"состояние заказа на внутреннее перемещение\". Текст ошибки: " + f.Message);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        #endregion

        #region Удалить объект из базы данных
        /// <summary>
        /// Удаляет запись из базы данных
        /// </summary>
        /// <param name="IntOrderState_Guid">уи состояния заказа на внутреннее перемещение</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - запись удалена; false - ошибка</returns>
        public static System.Boolean RemoveObjectFromDataBase(System.Guid IntOrderState_Guid,
           UniXP.Common.CProfile objProfile, ref System.String strErr)
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
                    strErr += ("Не удалось получить соединение с базой данных.");
                    return bRet;
                }
                DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteIntOrderState]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OrderState_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@IntOrderState_Guid"].Value = IntOrderState_Guid;
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
                    strErr += ((cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();

                strErr += ("Не удалось удалить объект \"состояние заказа на внутреннее перемещение\". Текст ошибки: " + f.Message);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список объектов "Состояние заказа" из базы данных в виде таблицы значений
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>таблицу</returns>
        public static System.Data.DataTable GetIntOrderStateList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            System.Data.DataTable dtReturn = new System.Data.DataTable();

            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderState_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderState_Name", typeof(System.Data.SqlTypes.SqlString)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderState_Description", typeof(System.Data.SqlTypes.SqlString)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderState_IsActive", typeof(System.Data.SqlTypes.SqlBoolean)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderState_IsDefault", typeof(System.Data.SqlTypes.SqlBoolean)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderState_Id", typeof(System.Data.SqlTypes.SqlInt32)));

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
                        return dtReturn;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetIntOrderState]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.Data.DataRow newRow = null;

                    while (rs.Read())
                    {
                        newRow = dtReturn.NewRow();
                        newRow["IntOrderState_Guid"] = (System.Guid)rs["IntOrderState_Guid"];
                        newRow["IntOrderState_Name"] = ((rs["IntOrderState_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["IntOrderState_Name"]));
                        newRow["IntOrderState_Description"] = ((rs["IntOrderState_Description"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["IntOrderState_Description"]));
                        newRow["IntOrderState_IsActive"] = ((rs["IntOrderState_IsActive"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["IntOrderState_IsActive"]));
                        newRow["IntOrderState_IsDefault"] = ((rs["IntOrderState_IsDefault"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["IntOrderState_IsDefault"]));
                        newRow["IntOrderState_Id"] = ((rs["IntOrderState_Id"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["IntOrderState_Id"]));

                        dtReturn.Rows.Add(newRow);
                    }

                    dtReturn.AcceptChanges();
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
                strErr += ("\nНе удалось получить список объектов \"состояние заказа на внутреннее перемещение\". Текст ошибки: " + f.Message);
            }
            return dtReturn;
        }
        #endregion
    }

}
