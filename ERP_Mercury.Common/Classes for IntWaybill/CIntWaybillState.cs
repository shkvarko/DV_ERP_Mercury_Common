using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace ERP_Mercury.Common
{
    /// <summary>
    /// Класс "Состояние накладной на внутреннее перемещение"
    /// </summary>
    public class CIntWaybillState : CBusinessObject
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
        [DisplayName("Код состояния накладной на внутреннее перемещение")]
        [Description("Целочисленный уникальный идентификатор состояния накладной на внутреннее перемещение")]
        [Category("1. Обязательные значения")]
        public System.Int32 IntWaybillStateId { get; set; }
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
        public CIntWaybillState()
            : base()
        {
            IntWaybillStateId = -1;
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
        public static List<CIntWaybillState> GetIntWaybillStateList(UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            List<CIntWaybillState> objList = new List<CIntWaybillState>();

            try
            {
                // вызов статического метода из класса, связанного с БД
                System.Data.DataTable dtList = CIntWaybillStateDataBaseModel.GetWaybillStateList(objProfile, null, ref strErr);
                if (dtList != null)
                {
                    CIntWaybillState objListItem = null;
                    foreach (System.Data.DataRow objItem in dtList.Rows)
                    {
                        objListItem = new CIntWaybillState();
                        objListItem.ID = new Guid(System.Convert.ToString(objItem["IntWaybillState_Guid"]));
                        objListItem.Name = ((objItem["IntWaybillState_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(objItem["IntWaybillState_Name"]));
                        objListItem.Description = ((objItem["IntWaybillState_Description"] == System.DBNull.Value) ? "" : System.Convert.ToString(objItem["IntWaybillState_Description"]));
                        objListItem.IntWaybillStateId = ((objItem["IntWaybillState_Id"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(System.Convert.ToString(objItem["IntWaybillState_Id"])));

                        if (objItem["IntWaybillState_IsActive"] != System.DBNull.Value)
                        {
                            objListItem.IsActive = System.Convert.ToBoolean(System.Convert.ToString(objItem["IntWaybillState_IsActive"]));
                        }

                        if (objItem["IntWaybillState_IsDefault"] != System.DBNull.Value)
                        {
                            objListItem.IsDefault = System.Convert.ToBoolean(System.Convert.ToString(objItem["IntWaybillState_IsDefault"]));
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

            System.Boolean bRet = CIntWaybillStateDataBaseModel.RemoveObjectFromDataBase(this.ID, objProfile, ref strErr);
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

            System.Boolean bRet = CIntWaybillStateDataBaseModel.AddNewObjectToDataBase(
                this.Name, this.Description, this.IsActive, this.IsDefault,
                this.IntWaybillStateId, ref GUID_ID, objProfile, ref strErr);
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

            System.Boolean bRet = CIntWaybillStateDataBaseModel.EditObjectInDataBase(this.ID,
                this.Name, this.Description, this.IsActive, this.IsDefault, this.IntWaybillStateId, objProfile, ref strErr);
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
    public static class CIntWaybillStateDataBaseModel
    {
        #region Добавление новой записи
        /// <summary>
        /// Проверка свойств объекта перед сохранением в базе данных
        /// </summary>
        /// <param name="IntWaybillState_Name">наименование состояния накладной на внутреннее перемещение</param>
        /// <param name="IntWaybillState_Id">код состояния накладной на внутреннее перемещение</param>
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
                    strErr += ("Необходимо указать наименование состояния накладной на внутреннее перемещение!");
                    return bRet;
                }
                if (IntWaybillState_Id < 0)
                {
                    strErr += ("Необходимо указать код состояния накладной на внутреннее перемещение!");
                    return bRet;
                }
                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr += ("Ошибка проверки свойств объекта \"состояние накладной на внутреннее перемещение\". Текст ошибки: " + f.Message);
            }
            return bRet;
        }

        /// <summary>
        /// Добавление новой записи с описанием состояния накладной на внутреннее перемещение в базу данных
        /// </summary>
        /// <param name="IntWaybillState_Name">наименование состояния накладной на внутреннее перемещение</param>
        /// <param name="IntWaybillState_Description">примечание</param>
        /// <param name="IntWaybillState_IsActive">признак "запись активна"</param>
        /// <param name="IntWaybillState_IsDefault">признак "использовать по умолчанию"</param>
        /// <param name="IntWaybillState_Id">код вида оплаты</param>
        /// <param name="IntWaybillState_Guid">УИ записи</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean AddNewObjectToDataBase(System.String IntWaybillState_Name,
            System.String IntWaybillState_Description, System.Boolean IntWaybillState_IsActive,
            System.Boolean IntWaybillState_IsDefault,
            System.Int32 IntWaybillState_Id, ref System.Guid IntWaybillState_Guid,
            UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;

            if (IsAllParametersValid(IntWaybillState_Name, IntWaybillState_Id, ref strErr) == false) { return bRet; }

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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddIntWaybillState]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntWaybillState_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntWaybillState_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntWaybillState_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntWaybillState_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntWaybillState_IsDefault", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntWaybillState_Id", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@IntWaybillState_Name"].Value = IntWaybillState_Name;
                cmd.Parameters["@IntWaybillState_Description"].Value = IntWaybillState_Description;
                cmd.Parameters["@IntWaybillState_IsActive"].Value = IntWaybillState_IsActive;
                cmd.Parameters["@IntWaybillState_IsDefault"].Value = IntWaybillState_IsDefault;
                cmd.Parameters["@IntWaybillState_Id"].Value = IntWaybillState_Id;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    IntWaybillState_Guid = (System.Guid)cmd.Parameters["@IntWaybillState_Guid"].Value;
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

                strErr += ("Не удалось создать объект \"состояние накладной на внутреннее перемещение\". Текст ошибки: " + f.Message);
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
        /// <param name="IntWaybillState_Guid">УИ записи</param>
        /// <param name="IntWaybillState_Name">наименование состояния накладной на внутреннее перемещение</param>
        /// <param name="IntWaybillState_Description">примечание</param>
        /// <param name="IntWaybillState_IsActive">признак "запись активна"</param>
        /// <param name="IntWaybillState_IsDefault">признак "использовать по умолчанию"</param>
        /// <param name="IntWaybillState_Id">код вида оплаты</param>
        /// <param name="IntWaybillState_Guid">УИ записи</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean EditObjectInDataBase(System.Guid IntWaybillState_Guid, System.String IntWaybillState_Name,
            System.String IntWaybillState_Description, System.Boolean IntWaybillState_IsActive,
            System.Boolean IntWaybillState_IsDefault,   System.Int32 IntWaybillState_Id,  
            UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;

            if( IsAllParametersValid( IntWaybillState_Name, IntWaybillState_Id, ref strErr ) == false) { return bRet; }

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
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntWaybillState_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntWaybillState_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntWaybillState_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntWaybillState_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntWaybillState_IsDefault", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IntWaybillState_Id", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@IntWaybillState_Guid"].Value = IntWaybillState_Guid;
                cmd.Parameters["@IntWaybillState_Name"].Value = IntWaybillState_Name;
                cmd.Parameters["@IntWaybillState_Description"].Value = IntWaybillState_Description;
                cmd.Parameters["@IntWaybillState_IsActive"].Value = IntWaybillState_IsActive;
                cmd.Parameters["@IntWaybillState_IsDefault"].Value = IntWaybillState_IsDefault;
                cmd.Parameters["@IntWaybillState_Id"].Value = IntWaybillState_Id;
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

                strErr += ("Не удалось внести изменения в объект \"состояние накладной на внутреннее перемещение\". Текст ошибки: " + f.Message);
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
        /// <param name="IntWaybillState_Guid">уи состояния накладной на внутреннее перемещение</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - запись удалена; false - ошибка</returns>
        public static System.Boolean RemoveObjectFromDataBase(System.Guid IntWaybillState_Guid,
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteIntWaybillState]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WaybillState_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@IntWaybillState_Guid"].Value = IntWaybillState_Guid;
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

                strErr += ("Не удалось удалить объект \"состояние накладной на внутреннее перемещение\". Текст ошибки: " + f.Message);
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
        /// Возвращает список объектов "Состояние документа" из базы данных в виде таблицы значений
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>таблицу</returns>
        public static System.Data.DataTable GetWaybillStateList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            System.Data.DataTable dtReturn = new System.Data.DataTable();

            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybillState_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybillState_Name", typeof(System.Data.SqlTypes.SqlString)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybillState_Description", typeof(System.Data.SqlTypes.SqlString)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybillState_IsActive", typeof(System.Data.SqlTypes.SqlBoolean)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybillState_IsDefault", typeof(System.Data.SqlTypes.SqlBoolean)));
            dtReturn.Columns.Add(new System.Data.DataColumn("IntWaybillState_Id", typeof(System.Data.SqlTypes.SqlInt32)));

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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetIntWaybillState]", objProfile.GetOptionsDllDBName());
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
                        newRow["IntWaybillState_Guid"] = (System.Guid)rs["IntWaybillState_Guid"];
                        newRow["IntWaybillState_Name"] = ((rs["IntWaybillState_Name"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["IntWaybillState_Name"]));
                        newRow["IntWaybillState_Description"] = ((rs["IntWaybillState_Description"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["IntWaybillState_Description"]));
                        newRow["IntWaybillState_IsActive"] = ((rs["IntWaybillState_IsActive"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["IntWaybillState_IsActive"]));
                        newRow["IntWaybillState_IsDefault"] = ((rs["IntWaybillState_IsDefault"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["IntWaybillState_IsDefault"]));
                        newRow["IntWaybillState_Id"] = ((rs["IntWaybillState_Id"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["IntWaybillState_Id"]));

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
                strErr += ("\nНе удалось получить список объектов \"состояние накладной на внутреннее перемещение\". Текст ошибки: " + f.Message);
            }
            return dtReturn;
        }
        #endregion
    }


}
