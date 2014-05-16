using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace ERP_Mercury.Common
{
    /// <summary>
    /// Класс "Причина возврата товара"
    /// </summary>
    public class CWaybillBackReason : CBusinessObject
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
        [Description("Целочисленный уникальный идентификатор причины возарата товара")]
        [Category("1. Обязательные значения")]
        public System.Int32 WaybillBackReasonId { get; set; }
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
        public CWaybillBackReason() : base()
        {
            WaybillBackReasonId = -1;
            IsActive = false;
            Description = System.String.Empty;
            IsDefault = false;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список причин возврата товара
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>список причин возврата товара</returns>
        public static List<CWaybillBackReason> GetWaybillBackReasonList(UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            List<CWaybillBackReason> objList = new List<CWaybillBackReason>();

            try
            {
                // вызов статического метода из класса, связанного с БД
                System.Data.DataTable dtList = CWaybillStateDataBaseModel.GetWaybillStateList(objProfile, null, ref strErr);
                if (dtList != null)
                {
                    CWaybillBackReason objListItem = null;
                    foreach (System.Data.DataRow objItem in dtList.Rows)
                    {
                        objListItem = new CWaybillBackReason();
                        objListItem.ID = new Guid(System.Convert.ToString(objItem["WaybillBackReason_Guid"]));
                        objListItem.Name = ((objItem["WaybillBackReason_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(objItem["WaybillBackReason_Name"]));
                        objListItem.Description = ((objItem["WaybillBackReason_Description"] == System.DBNull.Value) ? "" : System.Convert.ToString(objItem["WaybillBackReason_Description"]));
                        objListItem.WaybillBackReasonId = ((objItem["WaybillBackReason_Id"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(System.Convert.ToString(objItem["WaybillBackReason_Id"])));

                        if (objItem["WaybillBackReason_IsActive"] != System.DBNull.Value)
                        {
                            objListItem.IsActive = System.Convert.ToBoolean(System.Convert.ToString(objItem["WaybillBackReason_IsActive"]));
                        }

                        if (objItem["WaybillBackReason_IsDefault"] != System.DBNull.Value)
                        {
                            objListItem.IsDefault = System.Convert.ToBoolean(System.Convert.ToString(objItem["WaybillBackReason_IsDefault"]));
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

            System.Boolean bRet = CWaybillBackReasonDataBaseModel.RemoveObjectFromDataBase(this.ID, objProfile, ref strErr);
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

            System.Boolean bRet = CWaybillBackReasonDataBaseModel.AddNewObjectToDataBase(
                this.Name, this.Description, this.IsActive, this.IsDefault,
                this.WaybillBackReasonId, ref GUID_ID, objProfile, ref strErr);
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

            System.Boolean bRet = CWaybillBackReasonDataBaseModel.EditObjectInDataBase(this.ID,
                this.Name, this.Description, this.IsActive, this.IsDefault, this.WaybillBackReasonId, objProfile, ref strErr);
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
    public static class CWaybillBackReasonDataBaseModel
    {
        #region Добавление новой записи
        /// <summary>
        /// Проверка свойств объекта перед сохранением в базе данных
        /// </summary>
        /// <param name="WaybillBackReason_Name">наименование причины возврата товара</param>
        /// <param name="WaybillBackReason_Id">код причины возврата товара</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - проверка пройдена; false - проверка НЕ пройдена</returns>
        public static System.Boolean IsAllParametersValid(System.String WaybillBackReason_Name, System.Int32 WaybillBackReason_Id,
            ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (WaybillBackReason_Name.Trim() == "")
                {
                    strErr += ("Необходимо указать наименование причины возврата товара!");
                    return bRet;
                }
                if (WaybillBackReason_Id < 0)
                {
                    strErr += ("Необходимо указать код причины возврата товара!");
                    return bRet;
                }
                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr += ("Ошибка проверки свойств объекта \"причина возврата товара\". Текст ошибки: " + f.Message);
            }
            return bRet;
        }

        /// <summary>
        /// Добавление новой записи с описанием причины возврата товара в базу данных
        /// </summary>
        /// <param name="WaybillBackReason_Name">наименование причины возврата товара</param>
        /// <param name="WaybillBackReason_Description">примечание</param>
        /// <param name="WaybillBackReason_IsActive">признак "запись активна"</param>
        /// <param name="WaybillBackReason_IsDefault">признак "использовать по умолчанию"</param>
        /// <param name="WaybillBackReason_Id">код вида оплаты</param>
        /// <param name="WaybillBackReason_Guid">УИ записи</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean AddNewObjectToDataBase(System.String WaybillBackReason_Name,
            System.String WaybillBackReason_Description, System.Boolean WaybillBackReason_IsActive,
            System.Boolean WaybillBackReason_IsDefault,
            System.Int32 WaybillBackReason_Id, ref System.Guid WaybillBackReason_Guid,
            UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;

            if (IsAllParametersValid(WaybillBackReason_Name, WaybillBackReason_Id, ref strErr) == false) { return bRet; }

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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddWaybillBackReason]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillBackReason_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillBackReason_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillBackReason_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillBackReason_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillBackReason_IsDefault", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillBackReason_Id", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@WaybillBackReason_Name"].Value = WaybillBackReason_Name;
                cmd.Parameters["@WaybillBackReason_Description"].Value = WaybillBackReason_Description;
                cmd.Parameters["@WaybillBackReason_IsActive"].Value = WaybillBackReason_IsActive;
                cmd.Parameters["@WaybillBackReason_IsDefault"].Value = WaybillBackReason_IsDefault;
                cmd.Parameters["@WaybillBackReason_Id"].Value = WaybillBackReason_Id;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    WaybillBackReason_Guid = (System.Guid)cmd.Parameters["@WaybillBackReason_Guid"].Value;
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

                strErr += ("Не удалось создать объект \"причина возврата товара\". Текст ошибки: " + f.Message);
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
        /// <param name="WaybillBackReason_Guid">УИ записи</param>
        /// <param name="WaybillBackReason_Name">наименование причины возврата товара</param>
        /// <param name="WaybillBackReason_Description">примечание</param>
        /// <param name="WaybillBackReason_IsActive">признак "запись активна"</param>
        /// <param name="WaybillBackReason_IsDefault">признак "использовать по умолчанию"</param>
        /// <param name="WaybillBackReason_Id">код вида оплаты</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean EditObjectInDataBase(System.Guid WaybillBackReason_Guid, System.String WaybillBackReason_Name,
            System.String WaybillBackReason_Description, System.Boolean WaybillBackReason_IsActive,
            System.Boolean WaybillBackReason_IsDefault,   System.Int32 WaybillBackReason_Id,
            UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;

            if (IsAllParametersValid(WaybillBackReason_Name, WaybillBackReason_Id, ref strErr) == false) { return bRet; }

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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditWaybillBackReason]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillBackReason_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillBackReason_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillBackReason_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillBackReason_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillBackReason_IsDefault", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillBackReason_Id", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@WaybillBackReason_Guid"].Value = WaybillBackReason_Guid;
                cmd.Parameters["@WaybillBackReason_Name"].Value = WaybillBackReason_Name;
                cmd.Parameters["@WaybillBackReason_Description"].Value = WaybillBackReason_Description;
                cmd.Parameters["@WaybillBackReason_IsActive"].Value = WaybillBackReason_IsActive;
                cmd.Parameters["@WaybillBackReason_IsDefault"].Value = WaybillBackReason_IsDefault;
                cmd.Parameters["@WaybillBackReason_Id"].Value = WaybillBackReason_Id;
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

                strErr += ("Не удалось внести изменения в объект \"причина возврата товара\". Текст ошибки: " + f.Message);
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
        /// <param name="WaybillBackReason_Guid">уи причины возврата товара</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - запись удалена; false - ошибка</returns>
        public static System.Boolean RemoveObjectFromDataBase(System.Guid WaybillBackReason_Guid,
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteWaybillBackReason]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillBackReason_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@WaybillBackReason_Guid"].Value = WaybillBackReason_Guid;
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

                strErr += ("Не удалось удалить объект \"причина возврата товара\". Текст ошибки: " + f.Message);
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
        /// Возвращает список объектов "причина возврата товара" из базы данных в виде таблицы значений
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>таблицу</returns>
        public static System.Data.DataTable GetWaybillBackReasonList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            System.Data.DataTable dtReturn = new System.Data.DataTable();

            dtReturn.Columns.Add(new System.Data.DataColumn("WaybillBackReason_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybillBackReason_Name", typeof(System.Data.SqlTypes.SqlString)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybillBackReason_Description", typeof(System.Data.SqlTypes.SqlString)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybillBackReason_IsActive", typeof(System.Data.SqlTypes.SqlBoolean)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybillBackReason_IsDefault", typeof(System.Data.SqlTypes.SqlBoolean)));
            dtReturn.Columns.Add(new System.Data.DataColumn("WaybillBackReason_Id", typeof(System.Data.SqlTypes.SqlInt32)));

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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetWaybillBackReason]", objProfile.GetOptionsDllDBName());
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
                        newRow["WaybillBackReason_Guid"] = (System.Guid)rs["WaybillBackReason_Guid"];
                        newRow["WaybillBackReason_Name"] = ((rs["WaybillBackReason_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["WaybillBackReason_Name"]));
                        newRow["WaybillBackReason_Description"] = ((rs["WaybillBackReason_Description"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["WaybillBackReason_Description"]));
                        newRow["WaybillBackReason_IsActive"] = ((rs["WaybillBackReason_IsActive"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["WaybillBackReason_IsActive"]));
                        newRow["WaybillBackReason_IsDefault"] = ((rs["WaybillBackReason_IsDefault"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["WaybillBackReason_IsDefault"]));
                        newRow["WaybillBackReason_Id"] = ((rs["WaybillBackReason_Id"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["WaybillBackReason_Id"]));

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
