using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace ERP_Mercury.Common
{
    /// <summary>
    /// Класс "Состояние накладной"
    /// </summary>
    public class CWaybillState : CBusinessObject
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
        [DisplayName("Код состояния накладной")]
        [Description("Целочисленный уникальный идентификатор состояния накладной")]
        [Category("1. Обязательные значения")]
        public System.Int32 WaybillStateId { get; set; }
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
        public CWaybillState()
            : base()
        {
            WaybillStateId = -1;
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
        /// <returns>список состояний накладной</returns>
        public static List<CWaybillState> GetWaybillStateList(UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            List<CWaybillState> objList = new List<CWaybillState>();

            try
            {
                // вызов статического метода из класса, связанного с БД
                System.Data.DataTable dtList = CWaybillStateDataBaseModel.GetWaybillStateList(objProfile, null, ref strErr);
                if (dtList != null)
                {
                    CWaybillState objListItem = null;
                    foreach (System.Data.DataRow objItem in dtList.Rows)
                    {
                        objListItem = new CWaybillState();
                        objListItem.ID = new Guid(System.Convert.ToString(objItem["WaybillState_Guid"]));
                        objListItem.Name = ((objItem["WaybillState_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(objItem["WaybillState_Name"]));
                        objListItem.Description = ((objItem["WaybillState_Description"] == System.DBNull.Value) ? "" : System.Convert.ToString(objItem["WaybillState_Description"]));
                        objListItem.WaybillStateId = ((objItem["WaybillState_Id"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(System.Convert.ToString(objItem["WaybillState_Id"])));

                        if (objItem["WaybillState_IsActive"] != System.DBNull.Value)
                        {
                            objListItem.IsActive = System.Convert.ToBoolean(System.Convert.ToString(objItem["WaybillState_IsActive"]));
                        }

                        if (objItem["WaybillState_IsDefault"] != System.DBNull.Value)
                        {
                            objListItem.IsDefault = System.Convert.ToBoolean(System.Convert.ToString(objItem["WaybillState_IsDefault"]));
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

            System.Boolean bRet = CWaybillStateDataBaseModel.RemoveObjectFromDataBase(this.ID, objProfile, ref strErr);
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

            System.Boolean bRet = CWaybillStateDataBaseModel.AddNewObjectToDataBase(
                this.Name, this.Description, this.IsActive, this.IsDefault,
                this.WaybillStateId, ref GUID_ID, objProfile, ref strErr);
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

            System.Boolean bRet = CWaybillStateDataBaseModel.EditObjectInDataBase(this.ID,
                this.Name, this.Description, this.IsActive, this.IsDefault, this.WaybillStateId, objProfile, ref strErr);
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
    public static class CWaybillStateDataBaseModel
    {
        #region Добавление новой записи
        /// <summary>
        /// Проверка свойств объекта перед сохранением в базе данных
        /// </summary>
        /// <param name="WaybillState_Name">наименование состояния накладной</param>
        /// <param name="WaybillState_Id">код состояния накладной</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - проверка пройдена; false - проверка НЕ пройдена</returns>
        public static System.Boolean IsAllParametersValid(System.String WaybillState_Name, System.Int32 WaybillState_Id,
            ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (WaybillState_Name.Trim() == "")
                {
                    strErr += ("Необходимо указать наименование состояния накладной!");
                    return bRet;
                }
                if (WaybillState_Id < 0)
                {
                    strErr += ("Необходимо указать код состояния накладной!");
                    return bRet;
                }
                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr += ("Ошибка проверки свойств объекта \"состояние накладной\". Текст ошибки: " + f.Message);
            }
            return bRet;
        }

        /// <summary>
        /// Добавление новой записи с описанием состояния накладной в базу данных
        /// </summary>
        /// <param name="WaybillState_Name">наименование состояния накладной</param>
        /// <param name="WaybillState_Description">примечание</param>
        /// <param name="WaybillState_IsActive">признак "запись активна"</param>
        /// <param name="WaybillState_IsDefault">признак "использовать по умолчанию"</param>
        /// <param name="WaybillState_Id">код вида оплаты</param>
        /// <param name="WaybillState_Guid">УИ записи</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean AddNewObjectToDataBase(System.String WaybillState_Name,
            System.String WaybillState_Description, System.Boolean WaybillState_IsActive,
            System.Boolean WaybillState_IsDefault,
            System.Int32 WaybillState_Id, ref System.Guid WaybillState_Guid,
            UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;

            if (IsAllParametersValid(WaybillState_Name, WaybillState_Id, ref strErr) == false) { return bRet; }

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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddWaybillState]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillState_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillState_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillState_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillState_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillState_IsDefault", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillState_Id", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@WaybillState_Name"].Value = WaybillState_Name;
                cmd.Parameters["@WaybillState_Description"].Value = WaybillState_Description;
                cmd.Parameters["@WaybillState_IsActive"].Value = WaybillState_IsActive;
                cmd.Parameters["@WaybillState_IsDefault"].Value = WaybillState_IsDefault;
                cmd.Parameters["@WaybillState_Id"].Value = WaybillState_Id;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    WaybillState_Guid = (System.Guid)cmd.Parameters["@WaybillState_Guid"].Value;
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

                strErr += ("Не удалось создать объект \"состояние накладной\". Текст ошибки: " + f.Message);
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
        /// <param name="WaybillState_Guid">УИ записи</param>
        /// <param name="WaybillState_Name">наименование состояния накладной</param>
        /// <param name="WaybillState_Description">примечание</param>
        /// <param name="WaybillState_IsActive">признак "запись активна"</param>
        /// <param name="WaybillState_IsDefault">признак "использовать по умолчанию"</param>
        /// <param name="WaybillState_Id">код вида оплаты</param>
        /// <param name="WaybillState_Guid">УИ записи</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean EditObjectInDataBase(System.Guid WaybillState_Guid, System.String WaybillState_Name,
            System.String WaybillState_Description, System.Boolean WaybillState_IsActive,
            System.Boolean WaybillState_IsDefault,   System.Int32 WaybillState_Id,  
            UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;

            if( IsAllParametersValid( WaybillState_Name, WaybillState_Id, ref strErr ) == false) { return bRet; }

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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditWaybillState]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillState_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillState_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillState_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillState_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillState_IsDefault", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillState_Id", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@WaybillState_Guid"].Value = WaybillState_Guid;
                cmd.Parameters["@WaybillState_Name"].Value = WaybillState_Name;
                cmd.Parameters["@WaybillState_Description"].Value = WaybillState_Description;
                cmd.Parameters["@WaybillState_IsActive"].Value = WaybillState_IsActive;
                cmd.Parameters["@WaybillState_IsDefault"].Value = WaybillState_IsDefault;
                cmd.Parameters["@WaybillState_Id"].Value = WaybillState_Id;
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

                strErr += ("Не удалось внести изменения в объект \"состояние накладной\". Текст ошибки: " + f.Message);
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
        /// <param name="WaybillState_Guid">уи состояния накладной</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - запись удалена; false - ошибка</returns>
        public static System.Boolean RemoveObjectFromDataBase(System.Guid WaybillState_Guid,
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteWaybillState]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillState_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@WaybillState_Guid"].Value = WaybillState_Guid;
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

                strErr += ("Не удалось удалить объект \"состояние накладной\". Текст ошибки: " + f.Message);
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
        /// Возвращает список объектов "Состояние накладной" из базы данных в виде таблицы значений
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>таблицу</returns>
        public static System.Data.DataTable GetWaybillStateList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            System.Data.DataTable dtReturn = new System.Data.DataTable();

            dtReturn.Columns.Add(new System.Data.DataColumn("WaybillState_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybillState_Name", typeof(System.Data.SqlTypes.SqlString)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybillState_Description", typeof(System.Data.SqlTypes.SqlString)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybillState_IsActive", typeof(System.Data.SqlTypes.SqlBoolean)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybillState_IsDefault", typeof(System.Data.SqlTypes.SqlBoolean)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybillState_Id", typeof(System.Data.SqlTypes.SqlInt32)));

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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetWaybillState]", objProfile.GetOptionsDllDBName());
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
                        newRow["WaybillState_Guid"] = (System.Guid)rs["WaybillState_Guid"];
                        newRow["WaybillState_Name"] = ((rs["WaybillState_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["WaybillState_Name"]));
                        newRow["WaybillState_Description"] = ((rs["WaybillState_Description"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["WaybillState_Description"]));
                        newRow["WaybillState_IsActive"] = ((rs["WaybillState_IsActive"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["WaybillState_IsActive"]));
                        newRow["WaybillState_IsDefault"] = ((rs["WaybillState_IsDefault"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["WaybillState_IsDefault"]));
                        newRow["WaybillState_Id"] = ((rs["WaybillState_Id"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["WaybillState_Id"]));

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
                strErr += ("\nНе удалось получить список объектов \"состояние накладной\". Текст ошибки: " + f.Message);
            }
            return dtReturn;
        }
        #endregion
    }


}
