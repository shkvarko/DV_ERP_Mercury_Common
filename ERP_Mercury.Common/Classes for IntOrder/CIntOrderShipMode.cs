using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace ERP_Mercury.Common
{
    /// <summary>
    /// Класс "Вид отгрузки заказа на внутреннее перемещение"
    /// </summary>
    public class CIntOrderShipMode : CBusinessObject
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
        [Description("Целочисленный уникальный идентификатор вида отгрузки")]
        [Category("1. Обязательные значения")]
        public System.Int32 IntOrderShipModeId { get; set; }
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
        public CIntOrderShipMode()
            : base()
        {
            IntOrderShipModeId = -1;
            IsActive = false;
            Description = System.String.Empty;
            IsDefault = false;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список видов отгрузки заказа на внутреннее перемещение
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>список видов отгрузки накладной</returns>
        public static List<CIntOrderShipMode> GetIntOrderShipModeList(UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            List<CIntOrderShipMode> objList = new List<CIntOrderShipMode>();

            try
            {
                // вызов статического метода из класса, связанного с БД
                System.Data.DataTable dtList = CIntOrderShipModeDataBaseModel.GetIntOrderShipModeList(objProfile, null, ref strErr);
                if (dtList != null)
                {
                    CIntOrderShipMode objListItem = null;
                    foreach (System.Data.DataRow objItem in dtList.Rows)
                    {
                        objListItem = new CIntOrderShipMode();
                        objListItem.ID = new Guid(System.Convert.ToString(objItem["IntOrderShipMode_Guid"]));
                        objListItem.Name = ((objItem["IntOrderShipMode_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(objItem["IntOrderShipMode_Name"]));
                        objListItem.Description = ((objItem["IntOrderShipMode_Description"] == System.DBNull.Value) ? "" : System.Convert.ToString(objItem["IntOrderShipMode_Description"]));
                        objListItem.IntOrderShipModeId = ((objItem["IntOrderShipMode_Id"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(System.Convert.ToString(objItem["IntOrderShipMode_Id"])));

                        if (objItem["IntOrderShipMode_IsActive"] != System.DBNull.Value)
                        {
                            objListItem.IsActive = System.Convert.ToBoolean(System.Convert.ToString(objItem["IntOrderShipMode_IsActive"]));
                        }

                        if (objItem["IntOrderShipMode_IsDefault"] != System.DBNull.Value)
                        {
                            objListItem.IsDefault = System.Convert.ToBoolean(System.Convert.ToString(objItem["IntOrderShipMode_IsDefault"]));
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

            System.Boolean bRet = CIntOrderShipModeDataBaseModel.AddNewObjectToDataBase(
                this.Name, this.Description, this.IsActive, this.IsDefault,
                this.IntOrderShipModeId, ref GUID_ID, objProfile, ref strErr);
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

            System.Boolean bRet = CIntOrderShipModeDataBaseModel.EditObjectInDataBase(this.ID,
                this.Name, this.Description, this.IsActive, this.IsDefault, this.IntOrderShipModeId, objProfile, ref strErr);
            if (bRet == false)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
            System.String strErr = "";

            System.Boolean bRet = CIntOrderShipModeDataBaseModel.RemoveObjectFromDataBase(this.ID, objProfile, ref strErr);
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
    public static class CIntOrderShipModeDataBaseModel
    {
        #region Добавление новой записи
        /// <summary>
        /// Проверка свойств объекта перед сохранением в базе данных
        /// </summary>
        /// <param name="IntOrderShipMode_Name">наименование вида отгрузки заказа на внутреннее перемещение</param>
        /// <param name="IntOrderShipMode_Id">код вида отгрузки заказа на внутреннее перемещение</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - проверка пройдена; false - проверка НЕ пройдена</returns>
        public static System.Boolean IsAllParametersValid(System.String IntOrderShipMode_Name, System.Int32 IntOrderShipMode_Id,
            ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (IntOrderShipMode_Name.Trim() == "")
                {
                    strErr += ("Необходимо указать наименование вида отгрузки заказа на внутреннее перемещение!");
                    return bRet;
                }
                if (IntOrderShipMode_Id < 0)
                {
                    strErr += ("Необходимо указать код вида отгрузки заказа на внутреннее перемещение!");
                    return bRet;
                }
                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr += ("Ошибка проверки свойств объекта \"вид отгрузки заказа на внутреннее перемещение\". Текст ошибки: " + f.Message);
            }
            return bRet;
        }

        /// <summary>
        /// Добавление новой записи с описанием вида отгрузки заказа на внутреннее перемещение в базу данных
        /// </summary>
        /// <param name="IntOrderShipMode_Name">наименование вида отгрузки заказа на внутреннее перемещение</param>
        /// <param name="IntOrderShipMode_Description">примечание</param>
        /// <param name="IntOrderShipMode_IsActive">признак "запись активна"</param>
        /// <param name="IntOrderShipMode_IsDefault">признак "использовать по умолчанию"</param>
        /// <param name="IntOrderShipMode_Id">код вида вида отгрузки</param>
        /// <param name="IntOrderShipMode_Guid">УИ записи</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean AddNewObjectToDataBase(System.String IntOrderShipMode_Name,
            System.String IntOrderShipMode_Description, System.Boolean IntOrderShipMode_IsActive,
            System.Boolean IntOrderShipMode_IsDefault,
            System.Int32 IntOrderShipMode_Id, ref System.Guid IntOrderShipMode_Guid,
            UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;

            if (IsAllParametersValid(IntOrderShipMode_Name, IntOrderShipMode_Id, ref strErr) == false) { return bRet; }

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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddIntOrderShipMode]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderShipMode_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderShipMode_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderShipMode_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderShipMode_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderShipMode_IsDefault", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderState_Id", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@IntOrderShipMode_Name"].Value = IntOrderShipMode_Name;
                cmd.Parameters["@IntOrderShipMode_Description"].Value = IntOrderShipMode_Description;
                cmd.Parameters["@IntOrderShipMode_IsActive"].Value = IntOrderShipMode_IsActive;
                cmd.Parameters["@IntOrderShipMode_IsDefault"].Value = IntOrderShipMode_IsDefault;
                cmd.Parameters["@IntOrderShipMode_Id"].Value = IntOrderShipMode_Id;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    IntOrderShipMode_Guid = (System.Guid)cmd.Parameters["@IntOrderShipMode_Guid"].Value;
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

                strErr += ("Не удалось создать объект \"вид отгрузки заказа на внутреннее перемещение\". Текст ошибки: " + f.Message);
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
        /// <param name="IntOrderShipMode_Guid">УИ записи</param>
        /// <param name="IntOrderShipMode_Name">наименование вида отгрузки заказа на внутреннее перемещение</param>
        /// <param name="IntOrderShipMode_Description">примечание</param>
        /// <param name="IntOrderShipMode_IsActive">признак "запись активна"</param>
        /// <param name="IntOrderShipMode_IsDefault">признак "использовать по умолчанию"</param>
        /// <param name="IntOrderShipMode_Id">код вида отгрузки заказа на внутреннее перемещение</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean EditObjectInDataBase(System.Guid IntOrderShipMode_Guid, System.String IntOrderShipMode_Name,
            System.String IntOrderShipMode_Description, System.Boolean IntOrderShipMode_IsActive,
            System.Boolean IntOrderShipMode_IsDefault, System.Int32 IntOrderShipMode_Id,
            UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;

            if (IsAllParametersValid(IntOrderShipMode_Name, IntOrderShipMode_Id, ref strErr) == false) { return bRet; }

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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditIntOrderShipMode]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderShipMode_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderShipMode_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderShipMode_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderShipMode_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderShipMode_IsDefault", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderShipMode_Id", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@IntOrderShipMode_Guid"].Value = IntOrderShipMode_Guid;
                cmd.Parameters["@IntOrderShipMode_Name"].Value = IntOrderShipMode_Name;
                cmd.Parameters["@IntOrderShipMode_Description"].Value = IntOrderShipMode_Description;
                cmd.Parameters["@IntOrderShipMode_IsActive"].Value = IntOrderShipMode_IsActive;
                cmd.Parameters["@IntOrderShipMode_IsDefault"].Value = IntOrderShipMode_IsDefault;
                cmd.Parameters["@IntOrderShipMode_Id"].Value = IntOrderShipMode_Id;
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

                strErr += ("Не удалось внести изменения в объект \"вид отгрузки заказа на внутреннее перемещение\". Текст ошибки: " + f.Message);
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
        /// <param name="IntOrderShipMode_Guid">уи вида отгрузки заказа на внутреннее перемещение</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - запись удалена; false - ошибка</returns>
        public static System.Boolean RemoveObjectFromDataBase(System.Guid IntOrderShipMode_Guid,
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteIntOrderShipMode]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntOrderShipMode_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@IntOrderShipMode_Guid"].Value = IntOrderShipMode_Guid;
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

                strErr += ("Не удалось удалить объект \"вид отгрузки заказа на внутреннее перемещение\". Текст ошибки: " + f.Message);
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
        /// Возвращает список объектов "вид отгрузки заказа на внутреннее перемещение" из базы данных в виде таблицы значений
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>таблица</returns>
        public static System.Data.DataTable GetIntOrderShipModeList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            System.Data.DataTable dtReturn = new System.Data.DataTable();

            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderShipMode_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderShipMode_Name", typeof(System.Data.SqlTypes.SqlString)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderShipMode_Description", typeof(System.Data.SqlTypes.SqlString)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderShipMode_IsActive", typeof(System.Data.SqlTypes.SqlBoolean)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderShipMode_IsDefault", typeof(System.Data.SqlTypes.SqlBoolean)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntOrderShipMode_Id", typeof(System.Data.SqlTypes.SqlInt32)));

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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetIntOrderShipMode]", objProfile.GetOptionsDllDBName());
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
                        newRow["IntOrderShipMode_Guid"] = (System.Guid)rs["IntOrderShipMode_Guid"];
                        newRow["IntOrderShipMode_Name"] = ((rs["IntOrderShipMode_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["IntOrderShipMode_Name"]));
                        newRow["IntOrderShipMode_Description"] = ((rs["IntOrderShipMode_Description"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["IntOrderShipMode_Description"]));
                        newRow["IntOrderShipMode_IsActive"] = ((rs["IntOrderShipMode_IsActive"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["IntOrderShipMode_IsActive"]));
                        newRow["IntOrderShipMode_IsDefault"] = ((rs["IntOrderShipMode_IsDefault"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["IntOrderShipMode_IsDefault"]));
                        newRow["IntOrderShipMode_Id"] = ((rs["IntOrderShipMode_Id"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["IntOrderShipMode_Id"]));

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
                strErr += ("\nНе удалось получить список объектов \"вид отгрузки заказа на внутреннее перемещение\". Текст ошибки: " + f.Message);
            }
            return dtReturn;
        }
        #endregion
    }

}
