using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace ERP_Mercury.Common
{
    /// <summary>
    /// Класс "Единица измерения"
    /// </summary>
    public class CMeasure : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Сокращенное наименование
        /// </summary>
        private System.String m_strShortName;
        /// <summary>
        /// Сокращенное наименование
        /// </summary>
        [DisplayName("Сокращённое наименование")]
        [Description("Сокращённое наименование единицы измерения")]
        [Category("1. Обязательные значения")]
        public System.String ShortName
        {
            get { return m_strShortName; }
            set { m_strShortName = value; }
        }
        #endregion

        #region Конструктор
        public CMeasure()
            : base()
        {
            this.m_strShortName = "";
        }
        public CMeasure(System.Guid uuidID, System.String strName)
        {
            this.ID = uuidID;
            this.Name = strName;
            this.m_strShortName = "";
        }
        public CMeasure(System.Guid uuidID, System.String strName, System.String strShortName)
        {
            this.ID = uuidID;
            this.Name = strName;
            this.m_strShortName = strShortName;
        }
        #endregion

        #region Список объектов
        public static List<CMeasure> GetMeasureList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CMeasure> objList = new List<CMeasure>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetMeasure]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CMeasure(
                            (System.Guid)rs["Measure_Guid"],
                            (System.String)rs["Measure_Name"],
                            (System.String)rs["Measure_ShortName"] 
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
                "Не удалось получить список единиц измерения.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Запрашивает свойства объекта из БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public System.Boolean Init(UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    strErr += ("Не удалось получить соединение с базой данных.");
                    return bRet;
                }
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetMeasure]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Measure_Guid", System.Data.DbType.Guid));
                cmd.Parameters["@Measure_Guid"].Value = this.ID;

                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    rs.Read();

                    this.Name = (System.String)rs["Measure_Name"];
                    this.ShortName = (System.String)rs["Measure_ShortName"];

                    bRet = true;
                }
                rs.Dispose();
                cmd.Dispose();
                DBConnection.Close();
            }
            catch (System.Exception f)
            {
                strErr += (f.Message);
            }
            return bRet;
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
                System.String strErr = "";
                bRet = this.IsAllParametersValid(ref strErr);

                if (bRet == false)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
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
        /// Проверка свойств перед сохранением
        /// </summary>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - ошибок нет; false - ошибка</returns>
        public System.Boolean IsAllParametersValid(ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (this.Name == "")
                {
                    strErr += ("Необходимо указать название!");
                    return bRet;
                }
                if (this.ShortName == "")
                {
                    strErr += ("Необходимо указать сокращенное наименование!");
                    return bRet;
                }

                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr += (f.Message);
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddMeasure]", objProfile.GetOptionsDllDBName());

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Measure_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Measure_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Measure_ShortName", System.Data.DbType.String));
                

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));//**
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Measure_Name"].Value = this.Name;
                cmd.Parameters["@Measure_ShortName"].Value = this.ShortName;
                
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств единицы измерения.\n\nТекст ошибки: " + strErr, "Ошибка",
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
                "Не удалось изменить свойства склада.\n\nТекст ошибки: " + f.Message, "Внимание",
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
        /// <summary>
        /// Добавить запись в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="uuidPartTypeId">УИ товарной группы</param>
        /// <param name="iRes">код результата</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Add(UniXP.Common.CProfile objProfile, System.Guid uuidPartTypeId, ref System.Int32 iRes, ref System.String strErr)
        {
            System.Boolean bRet = false;
            if (this.IsAllParametersValid(ref strErr) == false) { return bRet; }

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddMeasure2]", objProfile.GetOptionsDllDBName());

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Measure_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Measure_Id", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Measure_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Measure_ShortName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartType_Guid", System.Data.SqlDbType.UniqueIdentifier));


                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));//**
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Measure_Name"].Value = this.Name;
                cmd.Parameters["@Measure_ShortName"].Value = this.ShortName;
                cmd.Parameters["@PartType_Guid"].Value = uuidPartTypeId;

                cmd.ExecuteNonQuery();
                iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if ((iRes == 0) || (iRes == 1))
                {
                    this.ID = (System.Guid)cmd.Parameters["@Measure_Guid"].Value;

                    if (iRes == 1)
                    {
                        strErr = this.Name + " присутствует в базе данных. УИ: " + this.ID.ToString();
                    }
                }
                else
                {
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                cmd.Dispose();
                bRet = ((iRes == 0) || (iRes == 1));
            }
            catch (System.Exception f)
            {
                strErr = ("Не удалось зарегистрировать единицу измерения.\n\nТекст ошибки: " + f.Message);
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteMeasure]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Measure_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Measure_Guid"].Value = this.ID;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes != 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления единицы измерения.\n\nТекст ошибки: " +
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
                    /* DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления валюты.\n\nТекст ошибки: " +
                     (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                         System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);*/
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить регион.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditMeasure]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Measure_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Measure_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Measure_ShortName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Measure_Guid"].Value = this.ID;
                cmd.Parameters["@Measure_Name"].Value = this.Name;
                cmd.Parameters["@Measure_ShortName"].Value = this.ShortName;
                
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств единицы измерения.\n\nТекст ошибки: " + strErr, "Ошибка",
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
                "Не удалось изменить свойства единицы измерения.\n\nТекст ошибки: " + f.Message, "Внимание",
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
    /// Класс "Владелец товарной марки"
    /// </summary>
    public class CProductVtm : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// УИ в InterBase
        /// </summary>
        private System.Int32 m_ID_Ib;
        /// <summary>
        /// УИ в InterBase
        /// </summary>
        [DisplayName("УИ в InterBase")]
        [Description("уникальный идентификатор записи в InterBase")]
        [Category("1. Обязательные значения")]
        [ReadOnly(true)]
        public System.Int32 ID_Ib
        {
            get { return m_ID_Ib; }
            set { m_ID_Ib = value; }
        }
        /// <summary>
        /// Сокращенное наименование
        /// </summary>
        private System.String m_strShortName;
        [DisplayName("Сокращение")]
        [Description("Сокращенное наименование")]
        [Category("1. Обязательные значения")]
        public System.String ShortName
        {
            get { return m_strShortName; }
            set { m_strShortName = value; }
        }
        /// <summary>
        /// Примечание
        /// </summary>
        private System.String m_strDescription;
        /// <summary>
        /// Примечание
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
        public CProductVtm()
            : base()
        {
            m_ID_Ib = 0;
            m_strShortName = "";
            m_strDescription = "";
            m_bIsActive = false;
        }
        public CProductVtm(System.Guid uuidId, System.String strName, System.Int32 iID_Ib, System.String strShortName,
            System.String strDescription, System.Boolean bIsActive)
        {
            ID = uuidId;
            Name = strName;
            m_ID_Ib = iID_Ib;
            m_strShortName = strShortName;
            m_strDescription = strDescription;
            m_bIsActive = bIsActive;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список объектов класса "CProductVtm"
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список объектов класса "CProductVtm"</returns>
        public static List<CProductVtm> GetProductVtmList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CProductVtm> objList = new List<CProductVtm>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetVtm]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CProductVtm(
                            (System.Guid)rs["Vtm_Guid"],
                            (System.String)rs["Vtm_Name"],
                            System.Convert.ToInt32(rs["Vtm_Id"]),
                            (System.String)rs["Vtm_ShortName"],
                            ((rs["Vtm_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Vtm_Description"]),
                            System.Convert.ToBoolean(rs["Vtm_IsActive"])
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
                "Не удалось получить список владельцев товарных марок.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Запрашивает свойства объекта из БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public System.Boolean Init(UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    strErr += ("Не удалось получить соединение с базой данных.");
                    return bRet;
                }
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetVtm]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vtm_Guid", System.Data.DbType.Guid));
                cmd.Parameters["@Vtm_Guid"].Value = this.ID;
                
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    rs.Read();

                    this.Name = (System.String)rs["Vtm_Name"];
                    this.ShortName = (System.String)rs["Vtm_ShortName"];
                    this.ID_Ib = System.Convert.ToInt32(rs["Vtm_Id"]);
                    this.IsActive = System.Convert.ToBoolean(rs["Vtm_IsActive"]);
                    this.Description = ((rs["Vtm_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Vtm_Description"]);

                    bRet = true;
                }
                rs.Dispose();
                cmd.Dispose();
                DBConnection.Close();
            }
            catch (System.Exception f)
            {
                strErr += (f.Message);
            }
            return bRet;
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
                System.String strErr = "";
                bRet = this.IsAllParametersValid(ref strErr);

                if (bRet == false)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show( strErr, "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
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
        /// Проверка свойств перед сохранением
        /// </summary>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - ошибок нет; false - ошибка</returns>
        public System.Boolean IsAllParametersValid( ref System.String strErr )
        {
            System.Boolean bRet = false;
            try
            {
                if (this.Name == "")
                {
                    strErr += ("Необходимо указать название!");
                    return bRet;
                }
                if (this.m_strShortName == "")
                {
                    strErr += ("Необходимо указать сокращенное наименование!");
                    return bRet;
                }

                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr += (f.Message);
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

            System.String strErr = "";
            System.Int32 iRes = 0;

            try
            {
                bRet = this.Add(objProfile, ref iRes, ref strErr);

                if (bRet == false)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось зарегистрировать ВТМ.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return bRet;
        }

        /// <summary>
        /// Добавить запись в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="iRes">код результата</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Add(UniXP.Common.CProfile objProfile, ref System.Int32 iRes, ref System.String strErr )
        {
            System.Boolean bRet = false;
            if (this.IsAllParametersValid( ref strErr) == false) { return bRet; }

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddVtm]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vtm_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vtm_Id", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vtm_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vtm_ShortName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vtm_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vtm_IsActive", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                if (this.ID_Ib != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@InVtm_Id", System.Data.SqlDbType.Int));
                    cmd.Parameters["@InVtm_Id"].Value = this.ID_Ib;
                }
                
                cmd.Parameters["@Vtm_Name"].Value = this.Name;
                cmd.Parameters["@Vtm_ShortName"].Value = this.ShortName;
                cmd.Parameters["@Vtm_IsActive"].Value = this.IsActive;
                cmd.Parameters["@Vtm_Description"].Value = this.Description;
                cmd.ExecuteNonQuery();
                iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if ((iRes == 0) || (iRes == 1))
                {
                    this.ID = (System.Guid)cmd.Parameters["@Vtm_Guid"].Value;
                    this.ID_Ib = System.Convert.ToInt32(cmd.Parameters["@Vtm_Id"].Value);

                    if (iRes == 1)
                    {
                        strErr = this.Name + " присутствует в базе данных. Код ВТМ: " + this.ID_Ib.ToString();
                    }
                }
                else
                {
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                cmd.Dispose();
                bRet = ((iRes == 0) || (iRes == 1));
            }
            catch (System.Exception f)
            {
                strErr = ("Не удалось создать ВТМ.\n\nТекст ошибки: " + f.Message);
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
                //cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteVtm]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vtm_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Vtm_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления ВТМ.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                //DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить ВТМ.\n\nТекст ошибки: " + f.Message, "Внимание",
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
            //System.Data.SqlClient.SqlTransaction DBTransaction = null;
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
                //DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                //cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditVtm]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vtm_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vtm_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vtm_ShortName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vtm_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vtm_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Vtm_Guid"].Value = this.ID;
                cmd.Parameters["@Vtm_Name"].Value = this.Name;
                cmd.Parameters["@Vtm_ShortName"].Value = this.ShortName;
                cmd.Parameters["@Vtm_IsActive"].Value = this.IsActive;
                cmd.Parameters["@Vtm_Description"].Value = this.Description;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    // подтверждаем транзакцию
                    //DBTransaction.Commit();
                }
                else
                {
                    //DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения ВТМ.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                //DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства ВТМ.\n\nТекст ошибки: " + f.Message, "Внимание",
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
    /// Класс "Товарная группа"
    /// </summary>
    public class CProductType : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// УИ в InterBase
        /// </summary>
        private System.Int32 m_ID_Ib;
        /// <summary>
        /// УИ в InterBase
        /// </summary>
        [DisplayName("УИ в InterBase")]
        [Description("уникальный идентификатор записи в InterBase")]
        [Category("1. Обязательные значения")]
        [ReadOnly(true)]
        public System.Int32 ID_Ib
        {
            get { return m_ID_Ib; }
            set { m_ID_Ib = value; }
        }
        /// <summary>
        /// Сокращенное наименование
        /// </summary>
        private System.String m_strDemandsName;
        [DisplayName("Группа")]
        [Description("Наименование группы")]
        [Category("1. Обязательные значения")]
        public System.String DemandsName
        {
            get { return m_strDemandsName; }
            set { m_strDemandsName = value; }
        }
        /// <summary>
        /// Ставка НДС, %
        /// </summary>
        private System.Double m_NDSRate;
         /// <summary>
        /// Ставка НДС, %
        /// </summary>
        [DisplayName("НДС, %")]
        [Description("Ставка НДС, %")]
        [Category("1. Обязательные значения")]
        public System.Double NDSRate
        {
            get { return m_NDSRate; }
            set { m_NDSRate = value; }
        }
        /// <summary>
        /// Примечание
        /// </summary>
        private System.String m_strDescription;
        /// <summary>
        /// Примечание
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
        public CProductType()
            : base()
        {
            m_ID_Ib = 0;
            m_NDSRate = 0;
            m_strDemandsName = "";
            m_strDescription = "";
            m_bIsActive = false;
        }
        public CProductType(System.Guid uuidId, System.String strName, System.Int32 iID_Ib, System.String strDemandsName, 
            System.Double NDSPercent, System.String strDescription, System.Boolean bIsActive)
        {
            ID = uuidId;
            Name = strName;
            m_ID_Ib = iID_Ib;
            m_NDSRate = NDSPercent;
            m_strDemandsName = strDemandsName;
            m_strDescription = strDescription;
            m_bIsActive = bIsActive;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список товарных групп
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список товарных групп</returns>
        public static List<CProductType> GetProductTypeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CProductType> objList = new List<CProductType>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CProductType(
                            (System.Guid)rs["Parttype_Guid"],
                            (System.String)rs["Parttype_Name"],
                            System.Convert.ToInt32(rs["Parttype_Id"]),
                            (System.String)rs["Parttype_DemandsName"],
                            System.Convert.ToDouble(rs["Parttype_NDSRate"]),
                            ((rs["Parttype_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Parttype_Description"]),
                            System.Convert.ToBoolean(rs["Parttype_IsActive"])
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
                "Не удалось получить список товарных групп.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Запрашивает свойства объекта из БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public System.Boolean Init(UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    strErr += ("Не удалось получить соединение с базой данных.");
                    return bRet;
                }
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parttype_Guid", System.Data.DbType.Guid));
                cmd.Parameters["@Parttype_Guid"].Value = this.ID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    rs.Read();
                    this.ID_Ib = System.Convert.ToInt32(rs["Parttype_Id"]);
                    this.Name = (System.String)rs["Parttype_Name"];
                    this.DemandsName = (System.String)rs["Parttype_DemandsName"];
                    this.Description = ((rs["Parttype_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Parttype_Description"]);
                    this.NDSRate = System.Convert.ToDouble(rs["Parttype_NDSRate"]);
                    this.IsActive = System.Convert.ToBoolean(rs["Parttype_IsActive"]);

                    bRet = true;
                }
                rs.Dispose();
                cmd.Dispose();
                DBConnection.Close();
            }
            catch (System.Exception f)
            {
                strErr += (f.Message); 
            }

            return bRet;
        }
        /// <summary>
        /// Возвращает товарную группу, используемую для расчета плана, когда разбивка осуществляется только по товарной марке
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>объект класса "Товарная группа"</returns>
        public static CProductType GetProductTypeDefaultForCalcPlan(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            CProductType objRet = null;
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
                        return objRet;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartTypeDefaultForPlanCalc]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    rs.Read();
                    objRet = new CProductType(
                            (System.Guid)rs["Parttype_Guid"],
                            (System.String)rs["Parttype_Name"],
                            System.Convert.ToInt32(rs["Parttype_Id"]),
                            (System.String)rs["Parttype_DemandsName"],
                            System.Convert.ToDouble(rs["Parttype_NDSRate"]),
                            ((rs["Parttype_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Parttype_Description"]),
                            System.Convert.ToBoolean(rs["Parttype_IsActive"])
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
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить товарную группу.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objRet;
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
                System.String strErr = "";

                bRet = this.IsAllParametersValid(ref strErr);

                if (bRet == false)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show( strErr, "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
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
        /// Проверка свойств перед сохранением
        /// </summary>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - все обязательные значения заполнены; false - ошибка</returns>
        public System.Boolean IsAllParametersValid( ref System.String strErr )
        {
            System.Boolean bRet = false;
            try
            {
                if (this.Name == "")
                {
                    strErr += ("Необходимо указать название!");
                    return bRet;
                }
                if (this.DemandsName == "")
                {
                    strErr += ("Необходимо указать наименование группы!");
                    return bRet;
                }
                if (this.NDSRate < 0)
                {
                    strErr += ("Ставка НДС не может быть отрицательной!");
                    return bRet;
                }

                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr += (f.Message);
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
            System.Int32 iRes = 0;
            System.String strErr = "";

            try
            {
                bRet = this.Add(objProfile, ref iRes, ref strErr);

                if (bRet == false)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show( strErr, "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать товарную группу.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return bRet;
        }

        /// <summary>
        /// Добавить запись в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="iRes">код результата выполнения хранимой процедуры</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Add( UniXP.Common.CProfile objProfile, ref System.Int32 iRes, ref System.String strErr )
        {
            System.Boolean bRet = false;
            if ( this.IsAllParametersValid( ref strErr ) == false) { return bRet; }

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    strErr += ("Не удалось получить соединение с базой данных.");
                    return bRet;
                }
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddPartType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parttype_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parttype_Id", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parttype_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parttype_NDSRate", System.Data.DbType.Double));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parttype_DemandsName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parttype_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parttype_IsActive", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                if (this.ID_Ib != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@InParttype_Id", System.Data.SqlDbType.Int));
                    cmd.Parameters["@InParttype_Id"].Value = this.ID_Ib;
                }

                cmd.Parameters["@Parttype_Name"].Value = this.Name;
                cmd.Parameters["@Parttype_DemandsName"].Value = this.DemandsName;
                cmd.Parameters["@Parttype_NDSRate"].Value = this.NDSRate;
                cmd.Parameters["@Parttype_IsActive"].Value = this.IsActive;
                cmd.Parameters["@Parttype_Description"].Value = this.Description;
                cmd.ExecuteNonQuery();
                iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if( (iRes == 0) || (iRes == 1))
                {
                    this.ID = (System.Guid)cmd.Parameters["@Parttype_Guid"].Value;
                    this.ID_Ib = System.Convert.ToInt32(cmd.Parameters["@Parttype_Id"].Value);

                    if (iRes == 1)
                    {
                        strErr = this.Name + " присутствует в базе данных. Код группы: " + this.ID_Ib.ToString();
                    }
                }
                else
                {
                    strErr += ( (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value );
                }

                cmd.Dispose();
                bRet = ((iRes == 0) || (iRes == 1));
            }
            catch (System.Exception f)
            {
                strErr += (f.Message);
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
                //cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeletePartType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parttype_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Parttype_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления товарной группы.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                //DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить товарную группу.\n\nТекст ошибки: " + f.Message, "Внимание",
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
            //System.Data.SqlClient.SqlTransaction DBTransaction = null;
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
                //DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                //cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditPartType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parttype_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parttype_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parttype_NDSRate", System.Data.DbType.Double));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parttype_DemandsName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parttype_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parttype_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Parttype_Guid"].Value = this.ID;
                cmd.Parameters["@Parttype_Name"].Value = this.Name;
                cmd.Parameters["@Parttype_DemandsName"].Value = this.DemandsName;
                cmd.Parameters["@Parttype_NDSRate"].Value = this.NDSRate;
                cmd.Parameters["@Parttype_IsActive"].Value = this.IsActive;
                cmd.Parameters["@Parttype_Description"].Value = this.Description;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    // подтверждаем транзакцию
                    //DBTransaction.Commit();
                }
                else
                {
                    //DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения товарной группы.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                //DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства товарной группы.\n\nТекст ошибки: " + f.Message, "Внимание",
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
    /// Класс "Товарная линия"
    /// </summary>
    public class CProductLine : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// УИ в InterBase
        /// </summary>
        private System.Int32 m_ID_Ib;
        /// <summary>
        /// УИ в InterBase
        /// </summary>
        [DisplayName("УИ в InterBase")]
        [Description("уникальный идентификатор записи в InterBase")]
        [Category("1. Обязательные значения")]
        [ReadOnly(true)]
        public System.Int32 ID_Ib
        {
            get { return m_ID_Ib; }
            set { m_ID_Ib = value; }
        }
        /// <summary>
        /// Примечание
        /// </summary>
        private System.String m_strDescription;
        /// <summary>
        /// Примечание
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
        public CProductLine()
            : base()
        {
            m_ID_Ib = 0;
            m_strDescription = "";
            m_bIsActive = false;
        }
        public CProductLine(System.Guid uuidId, System.String strName, System.Int32 iID_Ib, 
            System.String strDescription, System.Boolean bIsActive)
        {
            ID = uuidId;
            Name = strName;
            m_ID_Ib = iID_Ib;
            m_strDescription = strDescription;
            m_bIsActive = bIsActive;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список товарных линий
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список товарных линий</returns>
        public static List<CProductLine> GetProductLineList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CProductLine> objList = new List<CProductLine>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartline]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CProductLine(
                            (System.Guid)rs["Partline_Guid"],
                            (System.String)rs["Partline_Name"],
                            System.Convert.ToInt32(rs["Partline_Id"]),
                            ((rs["Partline_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Partline_Description"]),
                            System.Convert.ToBoolean(rs["Partline_IsActive"])
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
                "Не удалось получить список товарных линий.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Запрашивает свойства объекта из БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public System.Boolean Init(UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    strErr += ("Не удалось получить соединение с базой данных.");
                    return bRet;
                }
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartline]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partline_Guid", System.Data.DbType.Guid));
                cmd.Parameters["@Partline_Guid"].Value = this.ID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    rs.Read();
                    this.ID_Ib = System.Convert.ToInt32(rs["Partline_Id"]);
                    this.Name = (System.String)rs["Partline_Name"];
                    this.Description = ((rs["Partline_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Partline_Description"]);
                    this.IsActive = System.Convert.ToBoolean(rs["Partline_IsActive"]);

                    bRet = true;
                }
                rs.Dispose();
                cmd.Dispose();
                DBConnection.Close();
            }
            catch (System.Exception f)
            {
                strErr += (f.Message);
            }
            return bRet;
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
                System.String strErr = "";

                bRet = this.IsAllParametersValid(ref strErr);

                if (bRet == false)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
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
        /// Проверка свойств перед сохранением
        /// </summary>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - все обязательные значения заполнены; false - ошибка</returns>
        public System.Boolean IsAllParametersValid( ref System.String strErr )
        {
            System.Boolean bRet = false;
            try
            {
                if (this.Name == "")
                {
                    strErr += ("Необходимо указать название товарной линии!");
                    return bRet;
                }

                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr += ("Проверка обязательных параметров (товарная линия): " + f.Message);
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
            System.Int32 iRes = 0;
            System.String strErr = "";
            try
            {
                bRet = this.Add(objProfile, ref iRes, ref strErr);

                if (bRet == false)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                     "Не удалось зарегистрировать товарную линию. " + strErr, "Внимание",
                     System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось зарегистрировать товарную линию.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return bRet;
        }
        /// <summary>
        /// Добавить запись в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="iRes">код результата выполнения хранимой процедуры</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Add(UniXP.Common.CProfile objProfile, ref System.Int32 iRes, ref System.String strErr)
        {
            System.Boolean bRet = false;
            if (IsAllParametersValid(ref strErr) == false) { return bRet; }

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    strErr += ("Не удалось получить соединение с базой данных.");
                    return bRet;
                }
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddPartLine]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partline_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partline_Id", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partline_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partline_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partline_IsActive", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                if (this.ID_Ib != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@InPartline_Id", System.Data.SqlDbType.Int));
                    cmd.Parameters["@InPartline_Id"].Value = this.ID_Ib;
                }
                
                cmd.Parameters["@Partline_Name"].Value = this.Name;
                cmd.Parameters["@Partline_IsActive"].Value = this.IsActive;
                cmd.Parameters["@Partline_Description"].Value = this.Description;
                cmd.ExecuteNonQuery();
                iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if ((iRes == 0) || (iRes == 1))
                {
                    this.ID = (System.Guid)cmd.Parameters["@Partline_Guid"].Value;
                    this.ID_Ib = System.Convert.ToInt32(cmd.Parameters["@Partline_Id"].Value);

                    if (iRes == 1)
                    {
                        strErr = this.Name + " присутствует в базе данных. Код линии: " + this.ID_Ib.ToString();
                    }
                }
                else
                {
                    strErr += ((cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value);
                }

                cmd.Dispose();
                bRet = ((iRes == 0) || (iRes == 1));
            }
            catch (System.Exception f)
            {
                strErr += (f.Message);
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
                //cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeletePartLine]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partline_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Partline_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления товарной линии.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                //DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить товарную линию.\n\nТекст ошибки: " + f.Message, "Внимание",
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
            //System.Data.SqlClient.SqlTransaction DBTransaction = null;
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
                //DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                //cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditPartLine]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partline_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partline_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partline_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partline_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Partline_Guid"].Value = this.ID;
                cmd.Parameters["@Partline_Name"].Value = this.Name;
                cmd.Parameters["@Partline_IsActive"].Value = this.IsActive;
                cmd.Parameters["@Partline_Description"].Value = this.Description;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    // подтверждаем транзакцию
                    //DBTransaction.Commit();
                }
                else
                {
                    //DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения товарной линии. Текст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                //DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства товарной линии. Текст ошибки: " + f.Message, "Внимание",
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
    /// Класс "Товарная подгруппа"
    /// </summary>
    public class CProductSubType : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// TypeConverter для списка регионов
        /// </summary>
        class ProductLineTypeConverter : TypeConverter
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

                CProductSubType objProductSubType = (CProductSubType)context.Instance;
                System.Collections.Generic.List<CProductLine> objList = objProductSubType.GetAllProductLineList();

                return new StandardValuesCollection(objList);
            }
        }

        /// <summary>
        /// УИ в InterBase
        /// </summary>
        private System.Int32 m_ID_Ib;
        /// <summary>
        /// УИ в InterBase
        /// </summary>
        [DisplayName("УИ в InterBase")]
        [Description("уникальный идентификатор записи в InterBase")]
        [Category("1. Обязательные значения")]
        [ReadOnly(true)]
        public System.Int32 ID_Ib
        {
            get { return m_ID_Ib; }
            set { m_ID_Ib = value; }
        }
        /// <summary>
        /// Товарная линия
        /// </summary>
        private CProductLine m_objProductLine;
        /// <summary>
        /// Товарная линия
        /// </summary>
        [DisplayName("Товарная линия")]
        [Description("Товарная линия")]
        [Category("1. Обязательные значения")]
        [TypeConverter(typeof(ProductLineTypeConverter))]
        [ReadOnly(false)]
        [BrowsableAttribute(false)]
        public CProductLine ProductLine
        {
            get { return m_objProductLine; }
            set { m_objProductLine = value; }
        }
        /// <summary>
        /// Товарная линия
        /// </summary>
        [DisplayName("Товарная линия")]
        [Description("Товарная линия")]
        [Category("1. Обязательные значения")]
        [TypeConverter(typeof(ProductLineTypeConverter))]
        public System.String ProductLineName
        {
            get { return m_objProductLine.Name; }
            set { SetProductLineValue(value); }
        }
        private void SetProductLineValue(System.String strProductLineName)
        {
            try
            {
                if (m_objAllProductLineList == null) { m_objProductLine = null; }
                else
                {
                    foreach (CProductLine objProductLine in m_objAllProductLineList)
                    {
                        if (objProductLine.Name == strProductLineName)
                        {
                            m_objProductLine = objProductLine;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось установить значение товарной линии.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }
        /// <summary>
        /// Примечание
        /// </summary>
        private System.String m_strDescription;
        /// <summary>
        /// Примечание
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
        [DisplayName("Активен")]
        [Description("Признак активности записи")]
        [Category("1. Обязательные значения")]
        public System.Boolean IsActive
        {
            get { return m_bIsActive; }
            set { m_bIsActive = value; }
        }
        private List<CProductLine> m_objAllProductLineList;
        public List<CProductLine> GetAllProductLineList()
        {
            return m_objAllProductLineList;
        }
        public System.Double VendorTariff { get; set; }
        public System.Double NDS { get; set; }
        #endregion

        #region Конструктор
          public CProductSubType()
            : base()
            {
                m_ID_Ib = 0;
                m_strDescription = "";
                m_objProductLine = null;
                m_objAllProductLineList = null;
                m_bIsActive = false;
                VendorTariff = 0;
                NDS = 0;
            }
          public CProductSubType(System.Guid uuidId, System.String strName, System.Int32 iID_Ib,
            System.String strDescription, System.Boolean bIsActive, CProductLine objProductLine)
            {
                ID = uuidId;
                Name = strName;
                m_objProductLine = objProductLine;
                m_objAllProductLineList = null;
                m_ID_Ib = iID_Ib;
                m_strDescription = strDescription;
                m_bIsActive = bIsActive;
                VendorTariff = 0;
                NDS = 0;
            }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список товарных подгрупп
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список товарных подгрупп</returns>
        public static List<CProductSubType> GetProductSubTypeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
          List<CProductSubType> objList = new List<CProductSubType>();
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

              cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartSubType]", objProfile.GetOptionsDllDBName());
              cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
              cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
              cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
              cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
              System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
              if (rs.HasRows)
              {
                  while (rs.Read())
                  {
                      objList.Add(new CProductSubType(
                            (System.Guid)rs["Partsubtype_Guid"],
                            (System.String)rs["Partsubtype_Name"],
                            System.Convert.ToInt32(rs["Partsubtype_Id"]),
                            ((rs["Partsubtype_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Partsubtype_Description"]),
                            System.Convert.ToBoolean(rs["Partsubtype_IsActive"]),
                            new CProductLine(
                            (System.Guid)rs["Partline_Guid"],
                            (System.String)rs["Partline_Name"],
                            System.Convert.ToInt32(rs["Partline_Id"]),
                            ((rs["Partline_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Partline_Description"]),
                            System.Convert.ToBoolean(rs["Partline_IsActive"])
                            )
                          )
                          );
                  }
              }
              rs.Dispose();

              List<CProductLine> objProductLineList = CProductLine.GetProductLineList(objProfile, cmd);
              if ((objList != null) && (objProductLineList != null))
              {
                  foreach (CProductSubType objSubType in objList)
                  {
                      objSubType.m_objAllProductLineList = objProductLineList;
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
              "Не удалось получить список товарных подгрупп.\n\nТекст ошибки: " + f.Message, "Внимание",
              System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
          }
          return objList;
        }
        /// <summary>
        /// Возвращает список товарных подгрупп
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <returns>список товарных подгрупп</returns>
        public static List<CProductSubType> GetProductSubTypeList2(UniXP.Common.CProfile objProfile)
        {
            List<CProductSubType> objList = new List<CProductSubType>();
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
                    return objList;
                }
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartSubTypeForGuid]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CProductSubType(
                              (System.Guid)rs["Partsubtype_Guid"],
                              (System.String)rs["Partsubtype_Name"],
                              System.Convert.ToInt32(rs["Partsubtype_Id"]),
                              ((rs["Partsubtype_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Partsubtype_Description"]),
                              System.Convert.ToBoolean(rs["Partsubtype_IsActive"]),
                              new CProductLine(
                              (System.Guid)rs["Partline_Guid"],
                              (System.String)rs["Partline_Name"],
                              System.Convert.ToInt32(rs["Partline_Id"]),
                              ((rs["Partline_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Partline_Description"]),
                              System.Convert.ToBoolean(rs["Partline_IsActive"])
                              )
                            )
                            );
                    }
                }
                rs.Dispose();
                cmd.Dispose();
                DBConnection.Close();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список товарных подгрупп.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Запрашивает свойства объекта из БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public System.Boolean Init(UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    strErr += ("Не удалось получить соединение с базой данных.");
                    return bRet;
                }
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartSubTypeForGuid]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Guid", System.Data.DbType.Guid));
                cmd.Parameters["@Partsubtype_Guid"].Value = this.ID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    rs.Read();

                    this.ID_Ib = System.Convert.ToInt32(rs["Partsubtype_Id"]);
                    this.Name = (System.String)rs["Partsubtype_Name"];
                    this.Description = ((rs["Partsubtype_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Partsubtype_Description"]);
                    this.IsActive = System.Convert.ToBoolean(rs["Partsubtype_IsActive"]);
                    this.ProductLine = new CProductLine(
                              (System.Guid)rs["Partline_Guid"],
                              (System.String)rs["Partline_Name"],
                              System.Convert.ToInt32(rs["Partline_Id"]),
                              ((rs["Partline_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Partline_Description"]),
                              System.Convert.ToBoolean(rs["Partline_IsActive"])
                              );

                    bRet = true;
                }
                rs.Dispose();
                cmd.Dispose();
                DBConnection.Close();
            }
            catch (System.Exception f)
            {
                strErr += (f.Message);
            }
            return bRet;
        }

        /// <summary>
        /// Загружает в m_objAllProductLineList список товарных линий
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        public void InitProductLineList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            try
            {
                this.m_objAllProductLineList = CProductLine.GetProductLineList(objProfile, cmdSQL);
                if ((this.m_objAllProductLineList != null) && (this.m_objAllProductLineList.Count > 0))
                {
                    this.ProductLine = this.m_objAllProductLineList[0];
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось загрузить список товарных линий.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }
        /// <summary>
        /// Возвращает список товарных подгрупп
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список товарных подгрупп</returns>
        public static List<CProductSubType> GetProductSubTypeLightList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CProductSubType> objList = new List<CProductSubType>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartSubTypeLight]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CProductSubType(
                              (System.Guid)rs["Partsubtype_Guid"],
                              (System.String)rs["Partsubtype_Name"],
                              System.Convert.ToInt32(rs["Partsubtype_Id"]),
                              ((rs["Partsubtype_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Partsubtype_Description"]),
                              System.Convert.ToBoolean(rs["Partsubtype_IsActive"]),
                              null
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
                "Не удалось получить список товарных подгрупп.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                System.String strErr = "";

                bRet = this.IsAllParametersValid(ref strErr);

                if (bRet == false)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
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
        /// Проверка свойств перед сохранением
        /// </summary>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - все обязательные значения заполнены; false - ошибка</returns>
        public System.Boolean IsAllParametersValid(ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (this.Name == "")
                {
                    strErr += ("Необходимо указать название товарной подгруппы!");
                    return bRet;
                }
                if (this.m_objProductLine == null)
                {
                    strErr += ("Необходимо указать товарную линию!");
                    return bRet;
                }

                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr += ("Проверка обязательных параметров (товарная подгруппа): " + f.Message);
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddPartSubType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Id", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partline_Guid", System.Data.DbType.Guid));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Partsubtype_Name"].Value = this.Name;
                cmd.Parameters["@Partsubtype_IsActive"].Value = this.IsActive;
                cmd.Parameters["@Partsubtype_Description"].Value = this.Description;
                cmd.Parameters["@Partline_Guid"].Value = this.ProductLine.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@Partsubtype_Guid"].Value;
                    this.ID_Ib = System.Convert.ToInt32(cmd.Parameters["@Partsubtype_Id"].Value);
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания товарной подгруппы.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать товарную подгруппу.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        /// <summary>
        /// Добавить запись в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="iRes">код результата выполнения хранимой процедуры</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Add(UniXP.Common.CProfile objProfile, ref System.Int32 iRes, ref System.String strErr)
        {
            System.Boolean bRet = false;
            if (IsAllParametersValid(ref strErr) == false) { return bRet; }

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    strErr += ("Не удалось получить соединение с базой данных.");
                    return bRet;
                }
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddPartSubType2]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Id", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partline_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_NDS", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_VendorTariff", System.Data.SqlDbType.Money));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Partsubtype_Name"].Value = this.Name;
                cmd.Parameters["@Partsubtype_IsActive"].Value = this.IsActive;
                cmd.Parameters["@Partsubtype_Description"].Value = this.Description;
                cmd.Parameters["@Partline_Guid"].Value = this.ProductLine.ID;
                cmd.Parameters["@Partsubtype_NDS"].Value = this.NDS;
                cmd.Parameters["@Partsubtype_VendorTariff"].Value = this.VendorTariff;
                if (this.ID_Ib != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@InPartsubtype_Id", System.Data.SqlDbType.Int));
                    cmd.Parameters["@InPartsubtype_Id"].Value = this.ID_Ib;
                }

                cmd.ExecuteNonQuery();
                iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if( (iRes == 0) || (iRes == 1) )
                {
                    this.ID = (System.Guid)cmd.Parameters["@Partsubtype_Guid"].Value;
                    this.ID_Ib = System.Convert.ToInt32(cmd.Parameters["@Partsubtype_Id"].Value);

                    if (iRes == 1)
                    {
                        strErr = this.Name + " присутствует в базе данных. Код подгруппы: " + this.ID_Ib.ToString();
                    }
                }
                else
                {
                    strErr += ((cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value);
                }

                cmd.Dispose();
                bRet = ((iRes == 0) || (iRes == 1));
            }
            catch (System.Exception f)
            {
                strErr += (f.Message);
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeletePartsubtype]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Partsubtype_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления товарной подгруппы.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить товарную подгруппу.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditPartSubType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partline_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Partsubtype_Guid"].Value = this.ID;
                cmd.Parameters["@Partline_Guid"].Value = this.ProductLine.ID;
                cmd.Parameters["@Partsubtype_Name"].Value = this.Name;
                cmd.Parameters["@Partsubtype_IsActive"].Value = this.IsActive;
                cmd.Parameters["@Partsubtype_Description"].Value = this.Description;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения товарной подгруппы. Текст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства товарной подгруппы. Текст ошибки: " + f.Message, "Внимание",
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
    /// Класс "Товарная марка"
    /// </summary>
    public class CProductTradeMark : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// TypeConverter для списка регионов
        /// </summary>
        class ProductVtmTypeConverter : TypeConverter
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

                CProductTradeMark objProductOwner = (CProductTradeMark)context.Instance;
                System.Collections.Generic.List<CProductVtm> objList = objProductOwner.GetAllVtmList();

                return new StandardValuesCollection(objList);
            }
        }

        /// <summary>
        /// УИ в InterBase
        /// </summary>
        private System.Int32 m_ID_Ib;
        /// <summary>
        /// УИ в InterBase
        /// </summary>
        [DisplayName("УИ в InterBase")]
        [Description("уникальный идентификатор записи в InterBase")]
        [Category("1. Обязательные значения")]
        [ReadOnly(true)]
        public System.Int32 ID_Ib
        {
            get { return m_ID_Ib; }
            set { m_ID_Ib = value; }
        }
        /// <summary>
        /// Сокращенное наименование
        /// </summary>
        private System.String m_strShortName;
        /// <summary>
        /// Сокращенное наименование
        /// </summary>
        [DisplayName("Сокращение")]
        [Description("Сокращенное наименование")]
        [Category("1. Обязательные значения")]
        public System.String ShortName
        {
            get { return m_strShortName; }
            set { m_strShortName = value; }
        }
        /// <summary>
        /// Количество дней процессирования
        /// </summary>
        private System.Int32 m_iProcessDayCount;
        /// <summary>
        /// Количество дней процессирования
        /// </summary>
        [DisplayName("Дней процессирования")]
        [Description("Количество дней процессирования заказа")]
        [Category("1. Обязательные значения")]
        public System.Int32 ProcessDayCount
        {
            get { return m_iProcessDayCount; }
            set { m_iProcessDayCount = value; }
        }
        /// <summary>
        /// ВТМ
        /// </summary>
        private CProductVtm m_objProductVtm;
        /// <summary>
        /// Владелец товарной марки
        /// </summary>
        [DisplayName("ВТМ")]
        [Description("Владелец товарной марки")]
        [Category("1. Обязательные значения")]
        [TypeConverter(typeof(ProductVtmTypeConverter))]
        [ReadOnly(false)]
        [BrowsableAttribute(false)]
        public CProductVtm ProductVtm
        {
            get { return m_objProductVtm; }
            set { m_objProductVtm = value; }
        }
        /// <summary>
        /// Товарная линия
        /// </summary>
        [DisplayName("ВТМ")]
        [Description("Владелец товарной марки")]
        [Category("1. Обязательные значения")]
        [TypeConverter(typeof(ProductVtmTypeConverter))]
        public System.String VtmName
        {
            get { return m_objProductVtm.Name; }
            set { SetVtmValue(value); }
        }
        private void SetVtmValue(System.String strVtmName)
        {
            try
            {
                if (m_objAllVtmList == null) { m_objProductVtm = null; }
                else
                {
                    foreach (CProductVtm objProductVtm in m_objAllVtmList)
                    {
                        if (objProductVtm.Name == strVtmName)
                        {
                            m_objProductVtm = objProductVtm;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось установить значение ВТМ.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }
        /// <summary>
        /// Примечание
        /// </summary>
        private System.String m_strDescription;
        /// <summary>
        /// Примечание
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
        [DisplayName("Активен")]
        [Description("Признак активности записи")]
        [Category("1. Обязательные значения")]
        public System.Boolean IsActive
        {
            get { return this.m_bIsActive; }
            set { m_bIsActive = value; }
        }
        private List<CProductVtm> m_objAllVtmList;
        public List<CProductVtm> GetAllVtmList()
        {
            return m_objAllVtmList;
        }
        #endregion

        #region Конструктор
          public CProductTradeMark()
            : base()
            {
                m_ID_Ib = 0;
                m_iProcessDayCount = 0;
                m_strShortName = "";
                m_strDescription = "";
                m_objProductVtm = null;
                m_objAllVtmList = null;
                m_bIsActive = false;
            }
          public CProductTradeMark(System.Guid uuidId, System.String strName, System.String strShortName, 
              System.Int32 iID_Ib, System.Int32 iProcessDayCount,  System.String strDescription,
              System.Boolean bIsActive, CProductVtm objProductVtm)
            {
                ID = uuidId;
                Name = strName;
                m_strShortName = strShortName;
                m_objProductVtm = objProductVtm;
                m_objAllVtmList = null;
                m_ID_Ib = iID_Ib;
                m_strDescription = strDescription;
                m_bIsActive = bIsActive;
                m_iProcessDayCount = iProcessDayCount;
            }
      #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список товарных марок
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список товарных марок</returns>
        public static List<CProductTradeMark> GetProductTradeMarkList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CProductTradeMark> objList = new List<CProductTradeMark>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetOwner]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CProductTradeMark(
                            (System.Guid)rs["Owner_Guid"],
                            (System.String)rs["Owner_Name"], (System.String)rs["Owner_ShortName"],
                            System.Convert.ToInt32(rs["Owner_Id"]),
                            ((rs["Owner_ProcessDaysCount"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Owner_ProcessDaysCount"])),
                            ((rs["Owner_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Owner_Description"]),
                            System.Convert.ToBoolean(rs["Owner_IsActive"]),
                            new CProductVtm(
                                (System.Guid)rs["Vtm_Guid"],
                                (System.String)rs["Vtm_Name"],
                                System.Convert.ToInt32(rs["Vtm_Id"]),
                                (System.String)rs["Vtm_ShortName"],
                                ((rs["Vtm_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Vtm_Description"]),
                                System.Convert.ToBoolean(rs["Vtm_IsActive"])
                                )
                            )
                            );
                    }
                }
                rs.Dispose();

                List<CProductVtm> objProductVtmList = CProductVtm.GetProductVtmList(objProfile, cmd);
                if ((objList != null) && (objProductVtmList != null))
                {
                    foreach (CProductTradeMark objProductTradeMark in objList)
                    {
                        objProductTradeMark.m_objAllVtmList = objProductVtmList;
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
                "Не удалось получить список товарных марок.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Запрашивает свойства объекта из БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public System.Boolean Init(UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    strErr += ("Не удалось получить соединение с базой данных.");
                    return bRet;
                }
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetOwner]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Owner_Guid", System.Data.DbType.Guid));
                cmd.Parameters["@Owner_Guid"].Value = this.ID;

                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    rs.Read();

                    this.Name = (System.String)rs["Owner_Name"];
                    this.ShortName = (System.String)rs["Owner_ShortName"];
                    this.ID_Ib = System.Convert.ToInt32(rs["Owner_Id"]);
                    this.IsActive = System.Convert.ToBoolean(rs["Owner_IsActive"]);
                    this.Description = ((rs["Owner_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Owner_Description"]);
                    this.ProcessDayCount = ((rs["Owner_ProcessDaysCount"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Owner_ProcessDaysCount"]));
                    this.ProductVtm = new CProductVtm(
                                (System.Guid)rs["Vtm_Guid"],
                                (System.String)rs["Vtm_Name"],
                                System.Convert.ToInt32(rs["Vtm_Id"]),
                                (System.String)rs["Vtm_ShortName"],
                                ((rs["Vtm_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Vtm_Description"]),
                                System.Convert.ToBoolean(rs["Vtm_IsActive"])
                                );
                    bRet = true;
                }
                rs.Dispose();
                cmd.Dispose();
                DBConnection.Close();
            }
            catch (System.Exception f)
            {
                strErr += (f.Message);
            }

            return bRet;
        }
        /// <summary>
        /// Загружает в m_objAllVtmList список ВТМ
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        public void InitProductVtmList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            try
            {
                this.m_objAllVtmList = CProductVtm.GetProductVtmList(objProfile, cmdSQL);
                if ((this.m_objAllVtmList != null) && (this.m_objAllVtmList.Count > 0))
                {
                    this.ProductVtm = this.m_objAllVtmList[0];
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось загрузить список товарных ВТМ.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                System.String strErr = "";

                bRet = this.IsAllParametersValid(ref strErr);
                if (bRet == false)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show( strErr, "Внимание", 
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
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
        /// Проверка свойств перед сохранением
        /// </summary>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - ошибок нет; false - ошибка</returns>
        public System.Boolean IsAllParametersValid( ref System.String strErr )
        {
            System.Boolean bRet = false;
            try
            {
                if (this.Name == "")
                {
                    strErr += ("Необходимо указать название!");
                    return bRet;
                }
                if (this.ShortName == "")
                {
                    strErr += ("Необходимо указать сокращенное наименование!");
                    return bRet;
                }
                if (this.ProductVtm == null)
                {
                    strErr += ("Необходимо указать сокращенное ВТМ!");
                    return bRet;
                }
                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr += (f.Message);
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

            System.String strErr = "";
            System.Int32 iRes = 0;

            try
            {
                bRet = this.Add(objProfile, ref iRes, ref strErr);

                if (bRet == false)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось зарегистрировать товарную марку.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return bRet;
        }

        /// <summary>
        /// Добавить запись в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="iRes">код результата</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Add(UniXP.Common.CProfile objProfile, ref System.Int32 iRes, ref System.String strErr)
        {
            System.Boolean bRet = false;
            if (IsAllParametersValid(ref strErr) == false) { return bRet; }

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddOwner]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Owner_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Owner_Id", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Owner_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Owner_ShortName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Owner_ProcessDaysCount", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Owner_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Owner_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vtm_Guid", System.Data.DbType.Guid));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                if (this.ID_Ib != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@InOwner_Id", System.Data.SqlDbType.Int));
                    cmd.Parameters["@InOwner_Id"].Value = this.ID_Ib;
                }

                cmd.Parameters["@Owner_Name"].Value = this.Name;
                cmd.Parameters["@Owner_ShortName"].Value = this.ShortName;
                cmd.Parameters["@Owner_ProcessDaysCount"].Value = this.ProcessDayCount;
                cmd.Parameters["@Owner_IsActive"].Value = this.IsActive;
                cmd.Parameters["@Owner_Description"].Value = this.Description;
                cmd.Parameters["@Vtm_Guid"].Value = this.ProductVtm.ID;
                cmd.ExecuteNonQuery();
                iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if( (iRes == 0) || (iRes == 1) )
                {
                    this.ID = (System.Guid)cmd.Parameters["@Owner_Guid"].Value;
                    this.ID_Ib = System.Convert.ToInt32(cmd.Parameters["@Owner_Id"].Value);

                    if (iRes == 1)
                    {
                        strErr += ( this.Name + " присутствует в базе данных. Код ТМ: " + this.ID_Ib.ToString() );
                    }
                }
                else
                {
                    strErr += ( (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value );
                }

                cmd.Dispose();
                bRet = ((iRes == 0) || (iRes == 1));
            }
            catch (System.Exception f)
            {
                strErr += (f.Message);
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
                  //cmd.Transaction = DBTransaction;
                  cmd.CommandType = System.Data.CommandType.StoredProcedure;
                  cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteOwner]", objProfile.GetOptionsDllDBName());
                  cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                  cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Owner_Guid", System.Data.SqlDbType.UniqueIdentifier));
                  cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                  cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                  cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                  cmd.Parameters["@Owner_Guid"].Value = this.ID;
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
                      DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления товарной марки.\n\nТекст ошибки: " +
                      (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                          System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                  }
                  cmd.Dispose();
              }
              catch (System.Exception f)
              {
                  //DBTransaction.Rollback();
                  DevExpress.XtraEditors.XtraMessageBox.Show(
                  "Не удалось удалить товарную марку.\n\nТекст ошибки: " + f.Message, "Внимание",
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
              //System.Data.SqlClient.SqlTransaction DBTransaction = null;
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
                  //DBTransaction = DBConnection.BeginTransaction();
                  cmd = new System.Data.SqlClient.SqlCommand();
                  cmd.Connection = DBConnection;
                  //cmd.Transaction = DBTransaction;
                  cmd.CommandType = System.Data.CommandType.StoredProcedure;
                  cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditOwner]", objProfile.GetOptionsDllDBName());
                  cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                  cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Owner_Guid", System.Data.DbType.Guid));
                  cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vtm_Guid", System.Data.DbType.Guid));
                  cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Owner_Name", System.Data.DbType.String));
                  cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Owner_ShortName", System.Data.DbType.String));
                  cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Owner_ProcessDaysCount", System.Data.SqlDbType.Int));
                  cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Owner_Description", System.Data.DbType.String));
                  cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Owner_IsActive", System.Data.SqlDbType.Bit));
                  cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                  cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                  cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                  cmd.Parameters["@Owner_Guid"].Value = this.ID;
                  cmd.Parameters["@Owner_Name"].Value = this.Name;
                  cmd.Parameters["@Owner_ShortName"].Value = this.ShortName;
                  cmd.Parameters["@Owner_ProcessDaysCount"].Value = this.ProcessDayCount;
                  cmd.Parameters["@Owner_IsActive"].Value = this.IsActive;
                  cmd.Parameters["@Owner_Description"].Value = this.Description;
                  cmd.Parameters["@Vtm_Guid"].Value = this.ProductVtm.ID;
                  cmd.ExecuteNonQuery();
                  System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                  if (iRes == 0)
                  {
                      // подтверждаем транзакцию
                      //DBTransaction.Commit();
                  }
                  else
                  {
                      //DBTransaction.Rollback();
                      strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                      DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения товарной марки. Текст ошибки: " + strErr, "Ошибка",
                          System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                  }

                  cmd.Dispose();
                  bRet = (iRes == 0);
              }
              catch (System.Exception f)
              {
                  //DBTransaction.Rollback();
                  DevExpress.XtraEditors.XtraMessageBox.Show(
                  "Не удалось изменить свойства товарной марки. Текст ошибки: " + f.Message, "Внимание",
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
    /// Класс "Категория товара"
    /// </summary>
    public class CProductCategory : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// УИ в InterBase
        /// </summary>
        private System.Int32 m_ID_Ib;
        /// <summary>
        /// УИ в InterBase
        /// </summary>
        [DisplayName("УИ в InterBase")]
        [Description("уникальный идентификатор записи в InterBase")]
        [Category("1. Обязательные значения")]
        [ReadOnly(true)]
        public System.Int32 ID_Ib
        {
            get { return m_ID_Ib; }
            set { m_ID_Ib = value; }
        }
        /// <summary>
        /// Примечание
        /// </summary>
        private System.String m_strDescription;
        /// <summary>
        /// Примечание
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
        [DisplayName("Активен")]
        [Description("Признак активности записи")]
        [Category("1. Обязательные значения")]
        public System.Boolean IsActive
        {
            get { return m_bIsActive; }
            set { m_bIsActive = value; }
        }
        /// <summary>
        /// Дата последнего изменения
        /// </summary>
        private System.DateTime m_RecordUpdated;
        /// <summary>
        /// Дата последнего изменения
        /// </summary>
        [DisplayName("Дата изменения")]
        [Description("дата/время последнего изменения записи")]
        [Category("3. Журнал")]
        [ReadOnly(true)]
        public System.DateTime RecordUpdated
        {
            get { return m_RecordUpdated; }
            set { m_RecordUpdated = value; }
        }
        /// <summary>
        /// Пользователь, вносивший последние изменения
        /// </summary>
        private System.String m_strRecordUserUdpated;
        /// <summary>
        /// Пользователь, вносивший последние изменения
        /// </summary>
        [DisplayName("Пользователь")]
        [Description("Пользователь, вносивший последние изменения")]
        [Category("3. Журнал")]
        [ReadOnly(true)]
        public System.String RecordUserUdpated
        {
            get { return m_strRecordUserUdpated; }
            set { m_strRecordUserUdpated = value; }
        }
        private const System.Int32 iCommandTimeOutForIB = 120;

        #endregion

        #region Конструктор
        public CProductCategory()
            : base()
        {
            m_ID_Ib = 0;
            m_strDescription = "";
            m_bIsActive = false;
        }
        public CProductCategory(System.Guid uuidId, System.String strName, System.Int32 iID_Ib, 
            System.String strDescription, System.Boolean bIsActive)
        {
            ID = uuidId;
            Name = strName;
            m_ID_Ib = iID_Ib;
            m_strDescription = strDescription;
            m_bIsActive = bIsActive;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список товарных категорий
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список товарных категорий</returns>
        public static List<CProductCategory> GetProductCategoryList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CProductCategory> objList = new List<CProductCategory>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartsCategory]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CProductCategory(
                            (System.Guid)rs["PartsCategory_Guid"],
                            (System.String)rs["PartsCategory_Name"],
                            System.Convert.ToInt32(rs["PartsCategory_Id"]),
                            ((rs["PartsCategory_Description"] == System.DBNull.Value) ? "" : (System.String)rs["PartsCategory_Description"]),
                            System.Convert.ToBoolean(rs["PartsCategory_IsActive"])
                            )
                            );
                        if (rs["Record_Updated"] != System.DBNull.Value)
                        {
                            objList[objList.Count - 1].m_RecordUpdated = System.Convert.ToDateTime(rs["Record_Updated"]);
                        }
                        if (rs["Record_UserUdpated"] != System.DBNull.Value)
                        {
                            objList[objList.Count - 1].m_strRecordUserUdpated = System.Convert.ToString(rs["Record_UserUdpated"]);
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
                "Не удалось получить список товарных категорий.\n\nТекст ошибки: " + f.Message, "Внимание",
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
        /// Добавляет в базу данных информацию о категории товара
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public override System.Boolean Add(UniXP.Common.CProfile objProfile)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            System.Boolean bSaveInIB = false;
            System.String strErr = "";
            try
            {
                if (this.IsAllParametersValid() == false)
                {
                    return bRet;
                }

                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    return bRet;
                }
                DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandTimeout = iCommandTimeOutForIB;
                cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.CommandTimeout = iCommandTimeOutForIB;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddPartsCategory]", objProfile.GetOptionsDllDBName()); ;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsCategory_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsCategory_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsCategory_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsCategory_IsActive", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@PartsCategory_Name"].Value = this.Name;
                cmd.Parameters["@PartsCategory_Description"].Value = this.Description;
                cmd.Parameters["@PartsCategory_IsActive"].Value = this.IsActive;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@PartsCategory_Guid"].Value;
                    // теперь все это нужно записать в InterBase
                        cmd.Parameters.Clear();
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddPartsCategoryToIB]", objProfile.GetOptionsDllDBName()); ;
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsCategory_Guid", System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                        cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                        cmd.Parameters["@PartsCategory_Guid"].Value = this.ID;
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
                                DeletePartsCategoryFromIB(objProfile, cmd, ref strErr);
                            }

                            this.ID = System.Guid.Empty;
                        }
                    }
                    DBConnection.Close();
                bRet = (iRes == 0);
            }
            catch //(System.Exception f)
            {
                if (DBTransaction != null)
                {
                    DBTransaction.Rollback();
                }
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

        #region Сохранить в базе данных изменения в описании категории товара
        /// <summary>
        /// Сохраняет в базе данных изменения в описании категории товара
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="objContactDeletedList">список удаленных контактов</param>
        /// <param name="objAddressDeletedList">список удаленных адресов</param>
        /// <param name="objLicenceDeletedList">список удаленных лицензий</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public override System.Boolean Update(UniXP.Common.CProfile objProfile)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            System.String strErr = "";
            try
            {
                if (this.IsAllParametersValid() == false)
                {
                    return bRet;
                }

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

                cmd.CommandTimeout = iCommandTimeOutForIB;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditPartsCategory]", objProfile.GetOptionsDllDBName()); ;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsCategory_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsCategory_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsCategory_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsCategory_IsActive", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@PartsCategory_Guid"].Value = this.ID;
                cmd.Parameters["@PartsCategory_Name"].Value = this.Name;
                cmd.Parameters["@PartsCategory_Description"].Value = this.Description;
                cmd.Parameters["@PartsCategory_IsActive"].Value = this.IsActive;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    // теперь все это нужно записать в InterBase
                    if (iRes == 0)
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditPartsCategoryToIB]", objProfile.GetOptionsDllDBName()); ;
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsCategory_Guid", System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                        cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                        cmd.Parameters["@PartsCategory_Guid"].Value = this.ID;
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
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                if(DBTransaction != null)
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

        #region Удалить из базы данных описание категории товара
        /// <summary>
        /// Удаляет из базы данных описание категории товара
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
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
                        return bRet;
                    }
                    DBTransaction = DBConnection.BeginTransaction();
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.CommandTimeout = iCommandTimeOutForIB;
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //сперва удаляем в InterBase
                    System.String strErr = "";
                System.Int32 iRes = DeletePartsCategoryFromIB(objProfile, cmd, ref strErr);
                if (iRes == 0)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeletePartsCategory]", objProfile.GetOptionsDllDBName());
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsCategory_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters["@PartsCategory_Guid"].Value = this.ID;
                    cmd.ExecuteNonQuery();
                    iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
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
                DBConnection.Close();
            }
            catch //(System.Exception f)
            {
                if (DBTransaction != null)
                {
                    DBTransaction.Rollback();
                }
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
        /// Удаляет категорию товара из InterBase
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>0 - удачное завершение операции; <>0 - ошибка</returns>
        private System.Int32 DeletePartsCategoryFromIB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
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
                    cmdSQL.CommandText = System.String.Format("[{0}].[dbo].[usp_DeletePartsCategoryToIB]", objProfile.GetOptionsDllDBName()); ;
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsCategory_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmdSQL.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                    cmdSQL.Parameters["@PartsCategory_Guid"].Value = this.ID;
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
            return Name;
        }

    }
    /// <summary>
    /// Класс "Товар"
    /// </summary>
    public class CProduct : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// УИ в InterBase
        /// </summary>
        private System.Int32 m_ID_Ib;
        /// <summary>
        /// УИ в InterBase
        /// </summary>
        public System.Int32 ID_Ib
        {
            get { return m_ID_Ib; }
            set { m_ID_Ib = value; }
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
        /// Оригинальное название товара
        /// </summary>
        private System.String m_strOriginalName;
        /// <summary>
        /// Оригинальное название товара
        /// </summary>
        public System.String OriginalName
        {
            get { return m_strOriginalName; }
            set { m_strOriginalName = value; }
        }
        /// <summary>
        /// артикул товара
        /// </summary>
        private System.String m_strArticle;
        /// <summary>
        /// артикул товара
        /// </summary>
        public System.String Article
        {
            get { return m_strArticle; }
            set { m_strArticle = value; }
        }
        /// <summary>
        /// Количество в упаковке
        /// </summary>
        private System.Int32 m_iPackQuantity;
        /// <summary>
        /// Количество в упаковке
        /// </summary>
        public System.Int32 PackQuantity
        {
            get { return m_iPackQuantity; }
            set { m_iPackQuantity = value; }
        }
        /// <summary>
        /// Количество в упаковке для расчета
        /// </summary>
        private System.Int32 m_iPackQuantityForCalc;
        /// <summary>
        /// Количество в упаковке для расчета
        /// </summary>
        public System.Int32 PackQuantityForCalc
        {
            get { return m_iPackQuantityForCalc; }
            set { m_iPackQuantityForCalc = value; }
        }
        /// <summary>
        /// Количество в коробке
        /// </summary>
        private System.Int32 m_iBoxQuantity;
        /// <summary>
        /// Количество в коробке
        /// </summary>
        public System.Int32 BoxQuantity
        {
            get { return m_iBoxQuantity; }
            set { m_iBoxQuantity = value; }
        }
        /// <summary>
        /// Вес единицы товара в кг.
        /// </summary>
        private System.Decimal m_Weight;
        /// <summary>
        /// Вес единицы товара в кг.
        /// </summary>
        public System.Decimal Weight
        {
            get { return m_Weight; }
            set { m_Weight = value; }
        }
        /// <summary>
        /// Вес пластиковой тары в кг.
        /// </summary>
        private System.Decimal m_PlasticContainerWeight;
        /// <summary>
        /// Вес пластиковой тары в кг.
        /// </summary>
        public System.Decimal PlasticContainerWeight
        {
            get { return m_PlasticContainerWeight; }
            set { m_PlasticContainerWeight = value; }
        }
        /// <summary>
        /// Вес бумажной тары в кг.
        /// </summary>
        private System.Decimal m_PaperContainerWeight;
        /// <summary>
        /// Вес бумажной тары в кг.
        /// </summary>
        public System.Decimal PaperContainerWeight
        {
            get { return m_PaperContainerWeight; }
            set { m_PaperContainerWeight = value; }
        }
        /// <summary>
        /// Содержание спирта, %
        /// </summary>
        private System.Decimal m_AlcoholicContentPercent;
        /// <summary>
        /// Содержание спирта, %
        /// </summary>
        public System.Decimal AlcoholicContentPercent
        {
            get { return m_AlcoholicContentPercent; }
            set { m_AlcoholicContentPercent = value; }
        }
        /// <summary>
        /// Тариф поставщика
        /// </summary>
        private System.Decimal m_dcmlVendorPrice;
        /// <summary>
        /// Тариф поставщика
        /// </summary>
        public System.Decimal VendorPrice
        {
            get { return m_dcmlVendorPrice; }
            set { m_dcmlVendorPrice = value; }
        }
        /// <summary>
        /// Тариф таможенный
        /// </summary>
        private System.Decimal m_dcmlCustomTarif;
        /// <summary>
        /// Тариф таможенный
        /// </summary>
        public System.Decimal CustomTarif
        {
            get { return m_dcmlCustomTarif; }
            set { m_dcmlCustomTarif = value; }
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
        /// Признак "Неактуален"
        /// </summary>
        private System.Boolean m_bNotValid;
        /// <summary>
        /// Признак "Неактуален"
        /// </summary>
        public System.Boolean IsNotValid
        {
            get { return m_bNotValid; }
            set { m_bNotValid = value; }
        }
        /// <summary>
        /// Признак "Подтверждение неактуальности"
        /// </summary>
        private System.Boolean m_bActualNotValid;
        /// <summary>
        /// Признак "Подтверждение неактуальности"
        /// </summary>
        public System.Boolean IsActualNotValid
        {
            get { return m_bActualNotValid; }
            set { m_bActualNotValid = value; }
        }
        /// <summary>
        /// Состояние товара
        /// </summary>
        public System.String ActualStateName
        {
            get
            {
                System.String strRet = "";
                if ((IsActualNotValid == false) && (IsNotValid == false))
                {
                    strRet = "Активен";
                }
                else if ( (IsNotValid == true ) && (IsActualNotValid == false))
                {
                    strRet = "Вывод из ассортимента";
                }
                else if ((IsNotValid == true) && (IsActualNotValid == true))
                {
                    strRet = "Вывод из ассортимента, нет на остатках";
                }
                else if ((IsNotValid == false) && (IsActualNotValid == true))
                {
                    strRet = "Нет на остатках";
                }
                return strRet;
            }
        }
        /// <summary>
        /// Сертификат
        /// </summary>
        private System.String m_strCertificate;
        /// <summary>
        /// Сертификат
        /// </summary>
        public System.String Certificate
        {
            get { return m_strCertificate; }
            set { m_strCertificate = value; }
        }
        /// <summary>
        /// Валюта для тарифа поставщика
        /// </summary>
        private CCurrency m_objCurrency;
        /// <summary>
        /// Валюта для тарифа поставщика
        /// </summary>
        public CCurrency Currency
        {
            get { return m_objCurrency; }
            set { m_objCurrency = value; }
        }
        /// <summary>
        /// Страна производства
        /// </summary>
        private CCountry m_objCountry;
        /// <summary>
        /// Страна производства
        /// </summary>
        public CCountry Country
        {
            get { return m_objCountry; }
            set { m_objCountry = value; }
        }
        /// <summary>
        /// Список штрих-кодов
        /// </summary>
        private List<System.String> m_BarcodeList;
        /// <summary>
        /// Список штрих-кодов
        /// </summary>
        public List<System.String> BarcodeList
        {
            get { return m_BarcodeList; }
            set { m_BarcodeList = value; }
        }
        public System.String BarcodeListString
        {
            get 
            {
                System.String strBarcodeListString = "";
                if ((m_BarcodeList != null) && (m_BarcodeList.Count > 0))
                {
                    foreach( System.String strItem in m_BarcodeList )
                    {
                        strBarcodeListString += (strItem + " ");
                    }
                }
                return strBarcodeListString;
            }
        }
        public System.String BarcodeListString2
        {
            get
            {
                System.String strBarcodeListString = "";
                if ((m_BarcodeList != null) && (m_BarcodeList.Count > 0))
                {
                    foreach (System.String strItem in m_BarcodeList)
                    {
                        strBarcodeListString += ("\"" + strItem + "\" ");
                    }
                }
                return strBarcodeListString;
            }
        }
        public System.String BarcodeFirst
        {
            get
            {
                System.String strBarcodeListString = "";
                if ((m_BarcodeList != null) && (m_BarcodeList.Count > 0))
                {
                    strBarcodeListString = m_BarcodeList[0];
                }
                return strBarcodeListString;
            }
        }
        public System.String BarcodeListString3 { get; set; }

        /// <summary>
        /// Товарная марка
        /// </summary>
        private CProductTradeMark m_objProductTradeMark;
        /// <summary>
        /// Товарная марка
        /// </summary>
        public CProductTradeMark ProductTradeMark
        {
            get { return m_objProductTradeMark; }
            set { m_objProductTradeMark = value; }
        }
        /// <summary>
        /// Товарная группа
        /// </summary>
        private CProductType m_objProductType;
        /// <summary>
        /// Товарная группа
        /// </summary>
        public CProductType ProductType
        {
            get { return m_objProductType; }
            set { m_objProductType = value; }
        }
        /// <summary>
        /// Товарная подгруппа
        /// </summary>
        private CProductSubType m_objProductSubType;
        /// <summary>
        /// Товарная подгруппа
        /// </summary>
        public CProductSubType ProductSubType
        {
            get { return m_objProductSubType; }
            set { m_objProductSubType = value; }
        }
        /// <summary>
        /// Код ТНВЭД
        /// </summary>
        private System.String m_strCodeTNVD;
        /// <summary>
        /// Код ТНВЭД
        /// </summary>
        public System.String CodeTNVD
        {
            get { return m_strCodeTNVD; }
            set { m_strCodeTNVD = value; }
        }
        /// <summary>
        /// Референс
        /// </summary>
        private System.String m_strReference;
        /// <summary>
        /// Референс
        /// </summary>
        public System.String Reference
        {
            get { return m_strReference; }
            set { m_strReference = value; }
        }
        /// <summary>
        /// Единица измерения
        /// </summary>
        private CMeasure m_objMeasure;
        /// <summary>
        /// Единица измерения
        /// </summary>
        public CMeasure Measure
        {
            get { return m_objMeasure; }
            set { m_objMeasure = value; }
        }
        /// <summary>
        /// Категория товара
        /// </summary>
        private CProductCategory m_objProductCategory;
        /// <summary>
        /// Категория товара
        /// </summary>
        public CProductCategory ProductCategory
        {
            get { return m_objProductCategory; }
            set { m_objProductCategory = value; }
        }
        /// <summary>
        /// Изображение товара
        /// </summary>
        private System.Drawing.Image m_objImageProduct;
        /// <summary>
        /// Изображение товара
        /// </summary>
        public System.Drawing.Image ImageProduct
        {
            get { return m_objImageProduct; }
            set { m_objImageProduct = value; }
        }
        /// <summary>
        /// Код товарной марки
        /// </summary>
        public System.Int32 ProductTradeMarkIbID
        {
            get { return (m_objProductTradeMark == null ? 0 : m_objProductTradeMark.ID_Ib); }
        }
        /// <summary>
        /// Наименование товарной марки
        /// </summary>
        public System.String ProductTradeMarkName
        {
            get { return (m_objProductTradeMark == null ? "" : m_objProductTradeMark.Name); }
        }
        /// <summary>
        /// Код товарной группы
        /// </summary>
        public System.Int32 ProductTypeIbID
        {
            get { return (m_objProductType == null ? 0 : m_objProductType.ID_Ib); }
        }
        /// <summary>
        /// Наименование товарной группы
        /// </summary>
        public System.String ProductTypeName
        {
            get { return (m_objProductType == null ? "" : m_objProductType.Name); }
        }
        /// <summary>
        /// Наименование товарной подгруппы
        /// </summary>
        public System.String ProductSubTypeName
        {
            get { return (m_objProductSubType == null ? "" : m_objProductSubType.Name); }
        }
        /// <summary>
        /// Код товарной подгруппы
        /// </summary>
        public System.Int32 ProductSubTypeIbID
        {
            get { return (m_objProductSubType == null ? 0 : m_objProductSubType.ID_Ib); }
        }
        /// <summary>
        /// Ставка НДС для подгруппы
        /// </summary>
        public System.Double ProductSubTypeNDS
        {
            get { return (m_objProductSubType == null ? 0 : m_objProductSubType.NDS); }
        }
        /// <summary>
        /// Тариф поставщика для подгруппы
        /// </summary>
        public System.Double ProductSubTypeVendorTariff
        {
            get { return (m_objProductSubType == null ? 0 : m_objProductSubType.VendorTariff); }
        }
        /// <summary>
        /// Наименование товарной линии
        /// </summary>
        public System.String ProductLineName
        {
            get { return (m_objProductSubType == null ? "" : m_objProductSubType.ProductLine.Name); }
        }
        /// <summary>
        /// Код товарной линии
        /// </summary>
        public System.Int32 ProductLineIbID
        {
            get { return (m_objProductSubType == null ? 0 : m_objProductSubType.ProductLine.ID_Ib); }
        }
        /// <summary>
        /// Полное наименование товара
        /// </summary>
        public System.String ProductFullName
        {
            get { return (String.Format("{0}  {1}", Name, Article)); }
        }
        /// <summary>
        /// Наименование категории товара
        /// </summary>
        public System.String ProductCategoryName
        {
            get { return (m_objProductCategory == null ? "" : m_objProductCategory.Name); }
        }
        /// <summary>
        /// Наименование единицы измерения
        /// </summary>
        public System.String ProductMeasureName
        {
            get { return (m_objMeasure == null ? "" : m_objMeasure.Name); }
        }
        /// <summary>
        /// Страна ввоза
        /// </summary>
        public System.String CountryImportName
        {
            get { return (m_objCountry == null ? "" : m_objCountry.Name); }
        }
        /// <summary>
        /// Код валюты
        /// </summary>
        public System.String CurrencyCode
        {
            get { return (m_objCurrency == null ? "" : m_objCurrency.CurrencyAbbr); }
        }
        /// <summary>
        /// Отпускная цена без НДС
        /// </summary>
        private System.Double m_dblPriceImporter;
        /// <summary>
        /// Отпускная цена без НДС
        /// </summary>
        public System.Double PriceImporter
        {
            get { return m_dblPriceImporter; }
        }
        /// <summary>
        /// Вкл. (любой "Вкл" )
        /// </summary>
        private System.Boolean m_bIsCheck;
        /// <summary>
        /// Вкл. (любой "Вкл" )
        /// </summary>
        public System.Boolean IsCheck
        {
            get { return m_bIsCheck; }
            set { m_bIsCheck = value; }
        }
        /// <summary>
        /// Количество в наборе
        /// </summary>
        public System.Int32 QuantityInKit { get; set; }
        /// <summary>
        /// Номер по порядку в наборе
        /// </summary>
        public System.Int32 OrderIdInKit { get; set; }
        /// <summary>
        /// Признак "Товар назначен складу отгрузки"
        /// </summary>
        public System.Boolean IsIncludeInStockShip { get; set; }

        #region свойства для оформления заказа
        /// <summary>
        /// Остаток на складе
        /// </summary>
        public System.Double CustomerOrderStockQty { get; set; }
        /// <summary>
        /// Резерв на складе
        /// </summary>
        public System.Double CustomerOrderResQty { get; set; }
        /// <summary>
        /// Минимальное количество для возврата
        /// </summary>
        public System.Double CustomerOrderMinRetailQty { get; set; }
        /// <summary>
        /// Количество в упаковке
        /// </summary>
        public System.Double CustomerOrderPackQty { get; set; }

        #endregion
        
        #endregion

        #region Конструктор
        public CProduct()
            : base()
        {
            m_AlcoholicContentPercent = 0;
            m_bActualNotValid = false;
            m_BarcodeList = null;
            m_bIsActive = false;
            m_bNotValid = false;
            m_dcmlVendorPrice = 0;
            m_iBoxQuantity = 0;
            m_ID_Ib = 0;
            m_iPackQuantity = 0;
            m_iPackQuantityForCalc = 0;
            m_objCountry = null;
            m_objCurrency = null;
            m_objProductTradeMark = null;
            m_objProductSubType = null;
            m_objProductType = null;
            m_PaperContainerWeight = 0;
            m_PlasticContainerWeight = 0;
            m_strArticle = "";
            m_strCertificate = "";
            m_strCodeTNVD = "";
            m_strOriginalName = "";
            m_strReference = "";
            m_strShortName = "";
            m_Weight = 0;
            m_objMeasure = null;
            m_objProductCategory = null;
            m_objImageProduct = null;
            m_dcmlCustomTarif = 0;
            m_dblPriceImporter = 0;
            m_bIsCheck = false;
            IsIncludeInStockShip = false;
            CustomerOrderMinRetailQty = 0;
            CustomerOrderPackQty = 0;
            CustomerOrderResQty = 0;
            CustomerOrderStockQty = 0;
            BarcodeListString3 = "";
        }
        public CProduct(System.Guid uuidID, System.Int32 iID_Ib, System.String strName, System.String strOriginalName,
            System.String strShortName, System.String strArticle, CProductTradeMark objProductTradeMark,
            CProductType objProductType, CProductSubType objProductSubType, CCountry objCountry,
            CCurrency objCurrency, System.Decimal dcmlVendorPrice, System.Int32 iBoxQuantity,
            System.Int32 iPackQuantity, System.Int32 iPackQuantityForCalc, System.Decimal dcmlWeight, System.Decimal dcmlPaperContainerWeight,
            System.Decimal dcmlPlasticContainerWeight, System.Decimal dcmlAlcoholicContentPercent,
            System.Boolean bIsActive, System.Boolean bNotValid,
            System.Boolean bActualNotValid, System.String strCertificate, System.String strCodeTNVD,
            System.String strReference, CMeasure objMeasure, CProductCategory objProductCategory, System.Double dblPriceImporter)
        {
            ID = uuidID;
            Name = strName;
            m_AlcoholicContentPercent = dcmlAlcoholicContentPercent;
            m_bActualNotValid = bActualNotValid;
            m_BarcodeList = null;
            m_bIsActive = bIsActive;
            m_bNotValid = bNotValid;
            m_dcmlVendorPrice = dcmlVendorPrice;
            m_iBoxQuantity = iBoxQuantity;
            m_ID_Ib = iID_Ib;
            m_iPackQuantity = iPackQuantity;
            m_iPackQuantityForCalc = iPackQuantityForCalc;
            m_objCountry = objCountry;
            m_objCurrency = objCurrency;
            m_objProductTradeMark = objProductTradeMark;
            m_objProductSubType = objProductSubType;
            m_objProductType = objProductType;
            m_PaperContainerWeight = dcmlPaperContainerWeight;
            m_PlasticContainerWeight = dcmlPlasticContainerWeight;
            m_strArticle = strArticle;
            m_strCertificate = strCertificate;
            m_strCodeTNVD = strCodeTNVD;
            m_strOriginalName = strOriginalName;
            m_strReference = strReference;
            m_strShortName = strShortName;
            m_Weight = dcmlWeight;
            m_objMeasure = objMeasure;
            m_objProductCategory = objProductCategory;
            m_objImageProduct = null;
            m_dcmlCustomTarif = 0;
            m_dblPriceImporter = dblPriceImporter;
            m_bIsCheck = false;
            QuantityInKit = 0;
            OrderIdInKit = 0;
            IsIncludeInStockShip = false;
            CustomerOrderMinRetailQty = 0;
            CustomerOrderPackQty = 0;
            CustomerOrderResQty = 0;
            CustomerOrderStockQty = 0;
            BarcodeListString3 = "";
        }
        public CProduct(System.Guid uuidID, System.Int32 iID_Ib, System.String strName, System.String strOriginalName,
            System.String strShortName, System.String strArticle, CProductTradeMark objProductTradeMark,
            CProductType objProductType, CProductSubType objProductSubType, CCountry objCountry,
            CCurrency objCurrency, System.Decimal dcmlVendorPrice, System.Int32 iBoxQuantity,
            System.Int32 iPackQuantity, System.Int32 iPackQuantityForCalc, System.Decimal dcmlWeight, System.Decimal dcmlPaperContainerWeight,
            System.Decimal dcmlPlasticContainerWeight, System.Decimal dcmlAlcoholicContentPercent,
            System.Boolean bIsActive, System.Boolean bNotValid,
            System.Boolean bActualNotValid, System.String strCertificate, System.String strCodeTNVD,
            System.String strReference, CMeasure objMeasure, CProductCategory objProductCategory, System.Double dblPriceImporter,
            System.Boolean bIsCheck)
        {
            ID = uuidID;
            Name = strName;
            m_AlcoholicContentPercent = dcmlAlcoholicContentPercent;
            m_bActualNotValid = bActualNotValid;
            m_BarcodeList = null;
            m_bIsActive = bIsActive;
            m_bNotValid = bNotValid;
            m_dcmlVendorPrice = dcmlVendorPrice;
            m_iBoxQuantity = iBoxQuantity;
            m_ID_Ib = iID_Ib;
            m_iPackQuantity = iPackQuantity;
            m_iPackQuantityForCalc = iPackQuantityForCalc;
            m_objCountry = objCountry;
            m_objCurrency = objCurrency;
            m_objProductTradeMark = objProductTradeMark;
            m_objProductSubType = objProductSubType;
            m_objProductType = objProductType;
            m_PaperContainerWeight = dcmlPaperContainerWeight;
            m_PlasticContainerWeight = dcmlPlasticContainerWeight;
            m_strArticle = strArticle;
            m_strCertificate = strCertificate;
            m_strCodeTNVD = strCodeTNVD;
            m_strOriginalName = strOriginalName;
            m_strReference = strReference;
            m_strShortName = strShortName;
            m_Weight = dcmlWeight;
            m_objMeasure = objMeasure;
            m_objProductCategory = objProductCategory;
            m_objImageProduct = null;
            m_dcmlCustomTarif = 0;
            m_dblPriceImporter = dblPriceImporter;
            m_bIsCheck = bIsCheck;
            IsIncludeInStockShip = false;
            CustomerOrderMinRetailQty = 0;
            CustomerOrderPackQty = 0;
            CustomerOrderResQty = 0;
            CustomerOrderStockQty = 0;
            BarcodeListString3 = "";
        }
        public CProduct( System.String strBarCode )
            : base()
        {
            m_AlcoholicContentPercent = 0;
            m_bActualNotValid = false;
            m_BarcodeList = new List<string>();
            if (strBarCode != "") { m_BarcodeList.Add(strBarCode); }
            m_bIsActive = false;
            m_bNotValid = false;
            m_dcmlVendorPrice = 0;
            m_iBoxQuantity = 0;
            m_ID_Ib = 0;
            m_iPackQuantity = 0;
            m_iPackQuantityForCalc = 0;
            m_objCountry = null;
            m_objCurrency = null;
            m_objProductTradeMark = null;
            m_objProductSubType = null;
            m_objProductType = null;
            m_PaperContainerWeight = 0;
            m_PlasticContainerWeight = 0;
            m_strArticle = "";
            m_strCertificate = "";
            m_strCodeTNVD = "";
            m_strOriginalName = "";
            m_strReference = "";
            m_strShortName = "";
            m_Weight = 0;
            m_objMeasure = null;
            m_objProductCategory = null;
            m_objImageProduct = null;
            m_dcmlCustomTarif = 0;
            m_dblPriceImporter = 0;
            m_bIsCheck = false;
            IsIncludeInStockShip = false;
            CustomerOrderMinRetailQty = 0;
            CustomerOrderPackQty = 0;
            CustomerOrderResQty = 0;
            CustomerOrderStockQty = 0;
            BarcodeListString3 = "";
        }

        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список товаров, входящих в прайс-лист
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>Возвращает список товаров, входящих в прайс-лист</returns>
        public static List<CProduct> GetProductListForPriceList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CProduct> objList = null;
            try
            {
                objList = GetPartsList(objProfile, cmdSQL, false, true, System.Guid.Empty, System.Guid.Empty);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список товаров.\n\nТекст ошибки " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Возвращает список товаров
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="bOnlyNew">отображать только новинки</param>
        /// <returns>список товаров</returns>
        public static List<CProduct> GetProductList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Boolean bOnlyNew, System.Boolean bForLotOrder = false)
        {
            List<CProduct> objList = null;
            try
            {
                objList = GetPartsList(objProfile, cmdSQL, bOnlyNew, false, System.Guid.Empty, System.Guid.Empty, bForLotOrder);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список товаров.\n\nТекст ошибки " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Возвращает список товаров
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="bOnlyNew">признак "Только новинки"</param>
        /// <param name="bForPriceList">признак "Список товаров из прайса"</param>
        /// <returns>список товаров</returns>
        public static List<CProduct> GetPartsList( UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Boolean bOnlyNew, System.Boolean bForPriceList, System.Guid uuidStockId, System.Guid uuidOrderTypeId, 
            System.Boolean bForLotOrder = false)
        {
            List<CProduct> objList = new List<CProduct>();
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Int32 iPartsId = 0;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPart]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                if (bOnlyNew == true)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OnlyNew", System.Data.SqlDbType.Bit));
                    cmd.Parameters["@OnlyNew"].Value = bOnlyNew;
                }
                if (bForPriceList == true)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ForPriceList", System.Data.SqlDbType.Bit));
                    cmd.Parameters["@ForPriceList"].Value = bForPriceList;
                }
                if (uuidStockId.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@Stock_Guid"].Value = uuidStockId;
                }
                if (uuidOrderTypeId.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OrderType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@OrderType_Guid"].Value = uuidOrderTypeId;
                }
                if (bForLotOrder == true)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ForLotOrder", System.Data.SqlDbType.Bit));
                    cmd.Parameters["@ForLotOrder"].Value = bForLotOrder;
                }

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CCurrency objCurrency = null;
                    CCountry objCountry = null;
                    CProductTradeMark objProductTradeMark = null;
                    CProductType objProductType = null;
                    CProductSubType objProductSubType = null;
                    CMeasure objMeasure = null;
                    CProductCategory objProductCategory = null;

                    while (rs.Read())
                    {
                        objCurrency = null;
                        objCountry = null;
                        objProductTradeMark = null;
                        objProductType = null;
                        objProductSubType = null;
                        objProductCategory = null;
                        iPartsId = System.Convert.ToInt32(rs["Parts_Id"]);

                        // товарная марка
                        if (rs["Owner_Guid"] != System.DBNull.Value)
                        {
                            objProductTradeMark = new CProductTradeMark(
                                  (System.Guid)rs["Owner_Guid"],
                                  (System.String)rs["Owner_Name"], (System.String)rs["Owner_ShortName"],
                                  System.Convert.ToInt32(rs["Owner_Id"]),
                                  ((rs["Owner_ProcessDaysCount"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Owner_ProcessDaysCount"])),
                                  ((rs["Owner_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Owner_Description"]),
                                  System.Convert.ToBoolean(rs["Owner_IsActive"]),
                                  new CProductVtm(
                                      (System.Guid)rs["Vtm_Guid"],
                                      (System.String)rs["Vtm_Name"],
                                      System.Convert.ToInt32(rs["Vtm_Id"]),
                                      (System.String)rs["Vtm_ShortName"],
                                      ((rs["Vtm_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Vtm_Description"]),
                                      System.Convert.ToBoolean(rs["Vtm_IsActive"])
                                      )
                                      );
                        }
                        // товарная группа
                        if (rs["Parttype_Guid"] != System.DBNull.Value)
                        {
                            objProductType = new CProductType(
                                (System.Guid)rs["Parttype_Guid"],
                                (System.String)rs["Parttype_Name"],
                                System.Convert.ToInt32(rs["Parttype_Id"]),
                                (System.String)rs["Parttype_DemandsName"],
                                System.Convert.ToDouble(rs["Parttype_NDSRate"]),
                                ((rs["Parttype_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Parttype_Description"]),
                                System.Convert.ToBoolean(rs["Parttype_IsActive"])
                                );
                        }
                        // товарная подгруппа
                        if (rs["Partsubtype_Guid"] != System.DBNull.Value)
                        {
                            objProductSubType = new CProductSubType(
                                (System.Guid)rs["Partsubtype_Guid"],
                                (System.String)rs["Partsubtype_Name"],
                                System.Convert.ToInt32(rs["Partsubtype_Id"]),
                                ((rs["Partsubtype_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Partsubtype_Description"]),
                                System.Convert.ToBoolean(rs["Partsubtype_IsActive"]),
                                new CProductLine(
                                (System.Guid)rs["Partline_Guid"],
                                (System.String)rs["Partline_Name"],
                                System.Convert.ToInt32(rs["Partline_Id"]),
                                ((rs["Partline_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Partline_Description"]),
                                System.Convert.ToBoolean(rs["Partline_IsActive"])
                                )
                              );
                        }
                        // страна производства
                        if (rs["Country_Guid"] != System.DBNull.Value)
                        {
                            objCountry = new CCountry((System.Guid)rs["Country_Guid"],
                            (System.String)rs["Country_Name"], (System.String)rs["Country_Code"]);
                        }
                        // категория товара
                        if (rs["PartsCategory_Guid"] != System.DBNull.Value)
                        {
                            objProductCategory = new CProductCategory((System.Guid)rs["PartsCategory_Guid"],
                                (System.String)rs["PartsCategory_Name"], System.Convert.ToInt32(rs["PartsCategory_Id"]),
                                ((rs["PartsCategory_Description"] == System.DBNull.Value) ? "" : (System.String)rs["PartsCategory_Description"]),
                                System.Convert.ToBoolean(rs["PartsCategory_IsActive"])
                            );
                        }
                        // валюта
                        if (rs["Currency_Guid"] != System.DBNull.Value)
                        {
                            objCurrency = new CCurrency((System.Guid)rs["Currency_Guid"], "",
                            (System.String)rs["Currency_Abbr"], (System.String)rs["Currency_Code"]);
                        }
                        // единица измерения
                        if (rs["Measure_Guid"] != System.DBNull.Value)
                        {
                            objMeasure = new CMeasure((System.Guid)rs["Measure_Guid"], (System.String)rs["Measure_Name"], (System.String)rs["Measure_ShortName"]);
                        }

                        objList.Add(new CProduct((System.Guid)rs["Parts_Guid"], System.Convert.ToInt32(rs["Parts_Id"]),
                            (System.String)rs["Parts_Name"], (System.String)rs["Parts_OriginalName"],
                            (System.String)rs["Parts_ShortName"], (System.String)rs["Parts_Article"],
                            objProductTradeMark, objProductType, objProductSubType, objCountry, objCurrency,
                            ((rs["Parts_VendorPrice"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["Parts_VendorPrice"])),
                            ((rs["Parts_BoxQuantity"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Parts_BoxQuantity"])),
                            ((rs["Parts_PackQuantity"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Parts_PackQuantity"])),
                            ((rs["Parts_PackQuantityForCalc"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Parts_PackQuantityForCalc"])),
                            ((rs["Parts_Weight"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["Parts_Weight"])),
                            ((rs["Parts_PaperContainerWeight"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["Parts_PaperContainerWeight"])),
                            ((rs["Parts_PlasticContainerWeight"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["Parts_PlasticContainerWeight"])),
                            ((rs["Parts_AlcoholicContentPercent"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["Parts_AlcoholicContentPercent"])),
                            ((rs["Parts_IsActive"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["Parts_IsActive"])),
                            ((rs["Parts_NotValid"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["Parts_NotValid"])),
                            ((rs["Parts_ActualNotValid"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["Parts_ActualNotValid"])),
                            ((rs["Parts_Certificate"] == System.DBNull.Value) ? "" : (System.String)rs["Parts_Certificate"]),
                            ((rs["Parts_CodeTNVD"] == System.DBNull.Value) ? "" : (System.String)rs["Parts_CodeTNVD"]),
                            ((rs["Parts_Reference"] == System.DBNull.Value) ? "" : (System.String)rs["Parts_Reference"]),
                            objMeasure, objProductCategory, System.Convert.ToDouble(rs["Parts0"]), System.Convert.ToBoolean(rs["PartsIsCheck"])
                            ) 
                            { IsIncludeInStockShip = System.Convert.ToBoolean(rs["IsProductIncludeInStock"]), 
                                CustomerOrderPackQty = ((rs["Parts_PackQuantity"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Parts_PackQuantity"])) 
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
                "Не удалось получить список товаров.\n\nТекст ошибки для товара с кодом: " + iPartsId.ToString() + " - " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Возвращает список товаров, входящих в состав набора
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="uuidParentPartsID">УИ набора</param>
        /// <returns>список товаров</returns>
        public static List<CProduct> GetPartsDetail(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, 
            System.Guid uuidParentPartsID)
        {
            List<CProduct> objList = new List<CProduct>();
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Int32 iPartsId = 0;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartDetail]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parent_Parts_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters["@Parent_Parts_Guid"].Value = uuidParentPartsID;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CCurrency objCurrency = null;
                    CCountry objCountry = null;
                    CProductTradeMark objProductTradeMark = null;
                    CProductType objProductType = null;
                    CProductSubType objProductSubType = null;
                    CMeasure objMeasure = null;
                    CProductCategory objProductCategory = null;
                    CProduct objProduct = null;

                    while (rs.Read())
                    {
                        objCurrency = null;
                        objCountry = null;
                        objProductTradeMark = null;
                        objProductType = null;
                        objProductSubType = null;
                        objProductCategory = null;
                        iPartsId = System.Convert.ToInt32(rs["Parts_Id"]);

                        // товарная марка
                        if (rs["Owner_Guid"] != System.DBNull.Value)
                        {
                            objProductTradeMark = new CProductTradeMark(
                                  (System.Guid)rs["Owner_Guid"],
                                  (System.String)rs["Owner_Name"], (System.String)rs["Owner_ShortName"],
                                  System.Convert.ToInt32(rs["Owner_Id"]),
                                  ((rs["Owner_ProcessDaysCount"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Owner_ProcessDaysCount"])),
                                  ((rs["Owner_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Owner_Description"]),
                                  System.Convert.ToBoolean(rs["Owner_IsActive"]),
                                  new CProductVtm(
                                      (System.Guid)rs["Vtm_Guid"],
                                      (System.String)rs["Vtm_Name"],
                                      System.Convert.ToInt32(rs["Vtm_Id"]),
                                      (System.String)rs["Vtm_ShortName"],
                                      ((rs["Vtm_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Vtm_Description"]),
                                      System.Convert.ToBoolean(rs["Vtm_IsActive"])
                                      )
                                      );
                        }
                        // товарная группа
                        if (rs["Parttype_Guid"] != System.DBNull.Value)
                        {
                            objProductType = new CProductType(
                                (System.Guid)rs["Parttype_Guid"],
                                (System.String)rs["Parttype_Name"],
                                System.Convert.ToInt32(rs["Parttype_Id"]),
                                (System.String)rs["Parttype_DemandsName"],
                                System.Convert.ToDouble(rs["Parttype_NDSRate"]),
                                ((rs["Parttype_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Parttype_Description"]),
                                System.Convert.ToBoolean(rs["Parttype_IsActive"])
                                );
                        }
                        // товарная подгруппа
                        if (rs["Partsubtype_Guid"] != System.DBNull.Value)
                        {
                            objProductSubType = new CProductSubType(
                                (System.Guid)rs["Partsubtype_Guid"],
                                (System.String)rs["Partsubtype_Name"],
                                System.Convert.ToInt32(rs["Partsubtype_Id"]),
                                ((rs["Partsubtype_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Partsubtype_Description"]),
                                System.Convert.ToBoolean(rs["Partsubtype_IsActive"]),
                                new CProductLine(
                                (System.Guid)rs["Partline_Guid"],
                                (System.String)rs["Partline_Name"],
                                System.Convert.ToInt32(rs["Partline_Id"]),
                                ((rs["Partline_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Partline_Description"]),
                                System.Convert.ToBoolean(rs["Partline_IsActive"])
                                )
                              );
                        }
                        // страна производства
                        if (rs["Country_Guid"] != System.DBNull.Value)
                        {
                            objCountry = new CCountry((System.Guid)rs["Country_Guid"],
                            (System.String)rs["Country_Name"], (System.String)rs["Country_Code"]);
                        }
                        // категория товара
                        if (rs["PartsCategory_Guid"] != System.DBNull.Value)
                        {
                            objProductCategory = new CProductCategory((System.Guid)rs["PartsCategory_Guid"],
                            (System.String)rs["PartsCategory_Name"], System.Convert.ToInt32(rs["PartsCategory_Id"]),
                            ((rs["PartsCategory_Description"] == System.DBNull.Value) ? "" : (System.String)rs["PartsCategory_Description"]),
                            System.Convert.ToBoolean(rs["PartsCategory_IsActive"])
                            );
                        }
                        // валюта
                        if (rs["Currency_Guid"] != System.DBNull.Value)
                        {
                            objCurrency = new CCurrency((System.Guid)rs["Currency_Guid"], "",
                            (System.String)rs["Currency_Abbr"], (System.String)rs["Currency_Code"]);
                        }
                        // единица измерения
                        if (rs["Measure_Guid"] != System.DBNull.Value)
                        {
                            objMeasure = new CMeasure((System.Guid)rs["Measure_Guid"], (System.String)rs["Measure_Name"]);
                        }

                        objProduct = new CProduct((System.Guid)rs["Parts_Guid"], System.Convert.ToInt32(rs["Parts_Id"]),
                            (System.String)rs["Parts_Name"], (System.String)rs["Parts_OriginalName"],
                            (System.String)rs["Parts_ShortName"], (System.String)rs["Parts_Article"],
                            objProductTradeMark, objProductType, objProductSubType, objCountry, objCurrency,
                            ((rs["Parts_VendorPrice"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["Parts_VendorPrice"])),
                            ((rs["Parts_BoxQuantity"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Parts_BoxQuantity"])),
                            ((rs["Parts_PackQuantity"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Parts_PackQuantity"])),
                            ((rs["Parts_PackQuantityForCalc"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Parts_PackQuantityForCalc"])),
                            ((rs["Parts_Weight"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["Parts_Weight"])),
                            ((rs["Parts_PaperContainerWeight"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["Parts_PaperContainerWeight"])),
                            ((rs["Parts_PlasticContainerWeight"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["Parts_PlasticContainerWeight"])),
                            ((rs["Parts_AlcoholicContentPercent"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["Parts_AlcoholicContentPercent"])),
                            ((rs["Parts_IsActive"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["Parts_IsActive"])),
                            ((rs["Parts_NotValid"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["Parts_NotValid"])),
                            ((rs["Parts_ActualNotValid"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["Parts_ActualNotValid"])),
                            ((rs["Parts_Certificate"] == System.DBNull.Value) ? "" : (System.String)rs["Parts_Certificate"]),
                            ((rs["Parts_CodeTNVD"] == System.DBNull.Value) ? "" : (System.String)rs["Parts_CodeTNVD"]),
                            ((rs["Parts_Reference"] == System.DBNull.Value) ? "" : (System.String)rs["Parts_Reference"]),
                            objMeasure, objProductCategory, System.Convert.ToDouble(rs["Parts0"]), System.Convert.ToBoolean(rs["PartsIsCheck"])
                            );
                        objProduct.QuantityInKit = System.Convert.ToInt32(rs["PARTSDETAIL_QUANTITY"]);
                        objProduct.OrderIdInKit = System.Convert.ToInt32(rs["PARTSDETAIL_ORDERID"]);
                        objList.Add(objProduct);
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
                "Не удалось получить список товаров.\n\nТекст ошибки для товара с кодом: " + iPartsId.ToString() + " - " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }

        /// <summary>
        /// Загружает из БД изображение товара
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public System.Boolean LoadImageFromDB (UniXP.Common.CProfile objProfile, ref System.String strErr )
        {
            System.Boolean bRet = false;
            //Создаем соединение с базой данных
            System.Data.SqlClient.SqlConnection DBConnection = objProfile.GetDBSource();
            if (DBConnection == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Не удалось получить соединение с БД.", "Внимание",
                 System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return bRet;
            }
            try
            {
                // соединение с БД получено, прописываем команду на выборку данных
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartImage]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Parts_Guid"].Value = this.ID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    // набор данных непустой
                    rs.Read();
                    if (rs["Parts_Image"] != System.DBNull.Value)
                    {
                        byte[] arAttach = (byte[])rs["Parts_Image"];

                        System.IO.MemoryStream ms = new System.IO.MemoryStream( arAttach );
                        this.ImageProduct = System.Drawing.Image.FromStream( ms );
                        ms.Close();
                        ms = null;
                    }

                }
                rs.Close();
                rs.Dispose();
                bRet = true;

            }
            catch (System.Exception f)
            {
                strErr = "Не удалось загрузить изображение товара. Текст ошибки: " + f.Message;
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        /// <summary>
        /// Загружает список штрих-кодов товара
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public System.Boolean LoadBarCodeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, 
            ref System.String strErr)
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

                // соединение с БД получено, прописываем команду на выборку данных
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartBarcodeList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Parts_Guid"].Value = this.ID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    // набор данных непустой
                    if (this.BarcodeList == null) { this.BarcodeList = new List<string>(); }
                    else { this.BarcodeList.Clear(); }
                    while (rs.Read())
                    {
                        this.BarcodeList.Add(System.Convert.ToString(rs["Barcode"]));
                    }

                }
                rs.Close();
                rs.Dispose();
                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr = "Не удалось загрузить список штрих-кодов товара. Текст ошибки: " + f.Message;
            }
            finally
            {
            }
            return bRet;
        }
        /// <summary>
        /// Возвращает список объектов "Товар" с заполненными списками штрих-кодов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>список объектов "Товар" с заполненными списками штрих-кодов</returns>
        public static List<CProduct> GetPartsWithsBarcodeList(UniXP.Common.CProfile objProfile, 
            System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr )
        {
            List<CProduct> objList = new List<CProduct>();
            
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartsBarcodeList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();

                if (System.Convert.ToInt32(cmd.Parameters["@ERROR_NUM"].Value) != 0) 
                {
                    strErr += ("\nНе удалось получить список товаров.\n\nТекст ошибки: " + System.Convert.ToString(cmd.Parameters["@ERROR_MES"].Value ) );
                }

                if (rs.HasRows)
                {
                    CProduct objProduct = null;
                    System.Boolean bExistInList = false;
                    while (rs.Read())
                    {
                        objProduct = null;
                        bExistInList = false;

                        if (rs["Barcode"] != System.DBNull.Value)
                        {
                            objProduct = objList.FirstOrDefault<CProduct>(x => x.ID.CompareTo((System.Guid)rs["Parts_Guid"]) == 0);
                            if (objProduct == null)
                            {
                                objProduct = new CProduct();
                                objProduct.ID = (System.Guid)rs["Parts_Guid"];
                                objProduct.BarcodeList = new List<string>();
                            }
                            else
                            {
                                bExistInList = true;
                            }

                            objProduct.BarcodeList.Add(System.Convert.ToString(rs["Barcode"]));

                            if (bExistInList == false)
                            {
                                objList.Add(objProduct);
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
                strErr += ("\nНе удалось получить список товаров.\n\nТекст ошибки: " + f.Message);
            }
            return objList;
        }

        /// <summary>
        /// Поиск товара по уникальному коду в InterBase
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="iPartsId">код товара в InterBase</param>
        /// <param name="bIsAdvCode">признак "код поставщика"</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>объект класса "Товар"</returns>
        public static CProduct FindProductByPartsId(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Int32 iPartsId, System.Boolean bIsAdvCode, ref System.String strErr )
        {
            CProduct objRet = null;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr += ("Не удалось получить соединение с базой данных.");
                        return objRet;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartById]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IsAdvCode", System.Data.SqlDbType.Bit));
                cmd.Parameters["@IsAdvCode"].Value = bIsAdvCode;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Id", System.Data.SqlDbType.Int));
                cmd.Parameters["@Parts_Id"].Value = iPartsId;

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CCurrency objCurrency = null;
                    CCountry objCountry = null;
                    CProductTradeMark objProductTradeMark = null;
                    CProductType objProductType = null;
                    CProductSubType objProductSubType = null;
                    CMeasure objMeasure = null;
                    CProductCategory objProductCategory = null;

                    rs.Read();
                    {
                        objCurrency = null;
                        objCountry = null;
                        objProductTradeMark = null;
                        objProductType = null;
                        objProductSubType = null;
                        objProductCategory = null;
                        iPartsId = System.Convert.ToInt32(rs["Parts_Id"]);

                        // товарная марка
                        if (rs["Owner_Guid"] != System.DBNull.Value)
                        {
                            objProductTradeMark = new CProductTradeMark(
                                  (System.Guid)rs["Owner_Guid"],
                                  (System.String)rs["Owner_Name"], (System.String)rs["Owner_ShortName"],
                                  System.Convert.ToInt32(rs["Owner_Id"]),
                                  ((rs["Owner_ProcessDaysCount"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Owner_ProcessDaysCount"])),
                                  ((rs["Owner_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Owner_Description"]),
                                  System.Convert.ToBoolean(rs["Owner_IsActive"]),
                                  new CProductVtm(
                                      (System.Guid)rs["Vtm_Guid"],
                                      (System.String)rs["Vtm_Name"],
                                      System.Convert.ToInt32(rs["Vtm_Id"]),
                                      (System.String)rs["Vtm_ShortName"],
                                      ((rs["Vtm_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Vtm_Description"]),
                                      System.Convert.ToBoolean(rs["Vtm_IsActive"])
                                      )
                                      );
                        }
                        // товарная группа
                        if (rs["Parttype_Guid"] != System.DBNull.Value)
                        {
                            objProductType = new CProductType(
                                (System.Guid)rs["Parttype_Guid"],
                                (System.String)rs["Parttype_Name"],
                                System.Convert.ToInt32(rs["Parttype_Id"]),
                                (System.String)rs["Parttype_DemandsName"],
                                System.Convert.ToDouble(rs["Parttype_NDSRate"]),
                                ((rs["Parttype_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Parttype_Description"]),
                                System.Convert.ToBoolean(rs["Parttype_IsActive"])
                                );
                        }
                        // товарная подгруппа
                        if (rs["Partsubtype_Guid"] != System.DBNull.Value)
                        {
                            objProductSubType = new CProductSubType(
                                (System.Guid)rs["Partsubtype_Guid"],
                                (System.String)rs["Partsubtype_Name"],
                                System.Convert.ToInt32(rs["Partsubtype_Id"]),
                                ((rs["Partsubtype_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Partsubtype_Description"]),
                                System.Convert.ToBoolean(rs["Partsubtype_IsActive"]),
                                new CProductLine(
                                (System.Guid)rs["Partline_Guid"],
                                (System.String)rs["Partline_Name"],
                                System.Convert.ToInt32(rs["Partline_Id"]),
                                ((rs["Partline_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Partline_Description"]),
                                System.Convert.ToBoolean(rs["Partline_IsActive"])
                                )
                              );
                        }
                        // страна производства
                        if (rs["Country_Guid"] != System.DBNull.Value)
                        {
                            objCountry = new CCountry((System.Guid)rs["Country_Guid"],
                            (System.String)rs["Country_Name"], (System.String)rs["Country_Code"]);
                        }
                        // категория товара
                        if (rs["PartsCategory_Guid"] != System.DBNull.Value)
                        {
                            objProductCategory = new CProductCategory((System.Guid)rs["PartsCategory_Guid"],
                                (System.String)rs["PartsCategory_Name"], System.Convert.ToInt32(rs["PartsCategory_Id"]),
                                ((rs["PartsCategory_Description"] == System.DBNull.Value) ? "" : (System.String)rs["PartsCategory_Description"]),
                                System.Convert.ToBoolean(rs["PartsCategory_IsActive"])
                            );
                        }
                        // валюта
                        if (rs["Currency_Guid"] != System.DBNull.Value)
                        {
                            objCurrency = new CCurrency((System.Guid)rs["Currency_Guid"], "",
                            (System.String)rs["Currency_Abbr"], (System.String)rs["Currency_Code"]);
                        }
                        // единица измерения
                        if (rs["Measure_Guid"] != System.DBNull.Value)
                        {
                            objMeasure = new CMeasure((System.Guid)rs["Measure_Guid"], (System.String)rs["Measure_Name"], (System.String)rs["Measure_ShortName"]);
                        }

                        objRet = new CProduct((System.Guid)rs["Parts_Guid"], System.Convert.ToInt32(rs["Parts_Id"]),
                            (System.String)rs["Parts_Name"], (System.String)rs["Parts_OriginalName"],
                            (System.String)rs["Parts_ShortName"], (System.String)rs["Parts_Article"],
                            objProductTradeMark, objProductType, objProductSubType, objCountry, objCurrency,
                            ((rs["Parts_VendorPrice"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["Parts_VendorPrice"])),
                            ((rs["Parts_BoxQuantity"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Parts_BoxQuantity"])),
                            ((rs["Parts_PackQuantity"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Parts_PackQuantity"])),
                            ((rs["Parts_PackQuantityForCalc"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Parts_PackQuantityForCalc"])),
                            ((rs["Parts_Weight"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["Parts_Weight"])),
                            ((rs["Parts_PaperContainerWeight"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["Parts_PaperContainerWeight"])),
                            ((rs["Parts_PlasticContainerWeight"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["Parts_PlasticContainerWeight"])),
                            ((rs["Parts_AlcoholicContentPercent"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["Parts_AlcoholicContentPercent"])),
                            ((rs["Parts_IsActive"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["Parts_IsActive"])),
                            ((rs["Parts_NotValid"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["Parts_NotValid"])),
                            ((rs["Parts_ActualNotValid"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["Parts_ActualNotValid"])),
                            ((rs["Parts_Certificate"] == System.DBNull.Value) ? "" : (System.String)rs["Parts_Certificate"]),
                            ((rs["Parts_CodeTNVD"] == System.DBNull.Value) ? "" : (System.String)rs["Parts_CodeTNVD"]),
                            ((rs["Parts_Reference"] == System.DBNull.Value) ? "" : (System.String)rs["Parts_Reference"]),
                            objMeasure, objProductCategory, System.Convert.ToDouble(rs["Parts0"]), System.Convert.ToBoolean(rs["PartsIsCheck"])
                            )
                        {
                            IsIncludeInStockShip = System.Convert.ToBoolean(rs["IsProductIncludeInStock"]),
                            CustomerOrderPackQty = ((rs["Parts_PackQuantity"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["Parts_PackQuantity"]))
                        };

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
                strErr += ("Не удалось получить список товаров.\n\nТекст ошибки для товара с кодом: " + iPartsId.ToString() + " - " + f.Message);
            }
            return objRet;
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
                if (this.Article == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать артикул!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.OriginalName == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать оригинальное наименование!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.ProductTradeMark == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать товарную марку!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.ProductType == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать товарную группу!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.ProductSubType == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать товарную подгруппу!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.Currency == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать валюту!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.Measure == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать единицу измерения!", "Внимание",
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

        public override System.Boolean Add(UniXP.Common.CProfile objProfile)
        {
            return false;
        }
        /// <summary>
        /// Добавить запись в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Import(UniXP.Common.CProfile objProfile, ref System.String strErr, System.Guid Vendor_Guid )
        {
            System.Boolean bRet = false;
            if (IsAllParametersValid() == false) { return bRet; }

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            //System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddPart]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Id", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_OriginalName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_ShortName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Article", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Certificate", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_CodeTNVD", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Reference", System.Data.DbType.String));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_PackQuantity", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_PackQuantityForCalc", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_BoxQuantity", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Weight", System.Data.SqlDbType.Decimal));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_PlasticContainerWeight", System.Data.SqlDbType.Decimal));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_PaperContainerWeight", System.Data.SqlDbType.Decimal));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_AlcoholicContentPercent", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_VendorPrice", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_CustomTariff", System.Data.SqlDbType.Money));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_NotValid", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_ActualNotValid", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Measure_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Country_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsCategory_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Owner_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parttype_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Id", System.Data.DbType.Int32));
                //cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Barcode", System.Data.DbType.String));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                if (this.ID_Ib != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@InParts_Id", System.Data.SqlDbType.Int));
                    cmd.Parameters["@InParts_Id"].Value = this.ID_Ib;
                }
                if (Vendor_Guid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@Vendor_Guid"].Value = Vendor_Guid;
                }

                cmd.Parameters["@Parts_Name"].Value = this.Name;
                cmd.Parameters["@Parts_OriginalName"].Value = this.OriginalName;
                cmd.Parameters["@Parts_ShortName"].Value = this.ShortName;
                cmd.Parameters["@Parts_Article"].Value = this.Article;
                cmd.Parameters["@Parts_Certificate"].Value = this.Certificate;
                cmd.Parameters["@Parts_CodeTNVD"].Value = this.CodeTNVD;
                cmd.Parameters["@Parts_Reference"].Value = this.Reference;
                cmd.Parameters["@Currency_Guid"].Value = this.Currency.ID;
                cmd.Parameters["@Measure_Guid"].Value = this.Measure.ID;
                cmd.Parameters["@Owner_Guid"].Value = this.ProductTradeMark.ID;
                cmd.Parameters["@Parttype_Guid"].Value = this.ProductType.ID;
                cmd.Parameters["@Partsubtype_Id"].Value = this.ProductSubType.ID_Ib;
                //cmd.Parameters["@Partsubtype_Guid"].Value = this.ProductSubType.ID;
                if (this.Country == null)
                {
                    cmd.Parameters["@Country_Guid"].IsNullable = true;
                    cmd.Parameters["@Country_Guid"].Value = null;
                }
                else
                {
                    cmd.Parameters["@Country_Guid"].IsNullable = false;
                    cmd.Parameters["@Country_Guid"].Value = this.Country.ID;
                }
                if (this.ProductCategory == null)
                {
                    cmd.Parameters["@PartsCategory_Guid"].IsNullable = true;
                    cmd.Parameters["@PartsCategory_Guid"].Value = null;
                }
                else
                {
                    cmd.Parameters["@PartsCategory_Guid"].IsNullable = false;
                    cmd.Parameters["@PartsCategory_Guid"].Value = this.ProductCategory.ID;
                }
                if ((this.BarcodeList == null) || (this.BarcodeList.Count == 0))
                {
                    cmd.Parameters["@Barcode"].IsNullable = true;
                    cmd.Parameters["@Barcode"].Value = null;
                }
                else
                {
                    cmd.Parameters["@Barcode"].IsNullable = false;
                    cmd.Parameters["@Barcode"].Value = this.BarcodeList[0];
                }

                if (this.ImageProduct != null)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Image", System.Data.SqlDbType.Image));

                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    this.ImageProduct.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                    int lung = Convert.ToInt32(ms.Length);
                    byte[] arAttach = ms.ToArray();

                    ms.Close();
                    ms = null;

                    cmd.Parameters["@Parts_Image"].Value = arAttach;
                }
                cmd.Parameters["@Parts_PackQuantity"].Value = this.PackQuantity;
                cmd.Parameters["@Parts_PackQuantityForCalc"].Value = this.PackQuantityForCalc;
                cmd.Parameters["@Parts_BoxQuantity"].Value = this.BoxQuantity;
                cmd.Parameters["@Parts_Weight"].Value = this.Weight;
                cmd.Parameters["@Parts_PlasticContainerWeight"].Value = this.PlasticContainerWeight;
                cmd.Parameters["@Parts_PaperContainerWeight"].Value = this.PaperContainerWeight;
                cmd.Parameters["@Parts_AlcoholicContentPercent"].Value = this.AlcoholicContentPercent;
                cmd.Parameters["@Parts_VendorPrice"].Value = this.VendorPrice;
                cmd.Parameters["@Parts_CustomTariff"].Value = this.CustomTarif;
                cmd.Parameters["@Parts_IsActive"].Value = this.IsActive;
                cmd.Parameters["@Parts_NotValid"].Value = this.IsNotValid;
                cmd.Parameters["@Parts_ActualNotValid"].Value = this.IsActualNotValid;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if ((iRes == 0) || (iRes == 1))
                {
                    this.ID = (System.Guid)cmd.Parameters["@Parts_Guid"].Value;
                    this.ID_Ib = System.Convert.ToInt32(cmd.Parameters["@Parts_Id"].Value);

                    if (iRes == 1)
                    {
                        strErr = this.ProductFullName + " присутствует в базе данных. Код товара: " + this.ID_Ib.ToString();
                    }
                }
                else
                {
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                cmd.Dispose();
                bRet = ((iRes == 0) || (iRes == 1));
            }
            catch (System.Exception f)
            {
                strErr = f.Message;
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
                cmd.CommandTimeout = 600;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditPart]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_OriginalName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_ShortName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Article", System.Data.DbType.String));
                //cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Certificate", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_CodeTNVD", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Reference", System.Data.DbType.String));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_PackQuantity", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_PackQuantityForCalc", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_BoxQuantity", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Weight", System.Data.SqlDbType.Decimal));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_PlasticContainerWeight", System.Data.SqlDbType.Decimal));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_PaperContainerWeight", System.Data.SqlDbType.Decimal));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_AlcoholicContentPercent", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_VendorPrice", System.Data.SqlDbType.Money));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_NotValid", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_ActualNotValid", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Measure_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Country_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsCategory_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Owner_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parttype_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Partsubtype_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Parts_Guid"].Value = this.ID;
                cmd.Parameters["@Parts_Name"].Value = this.Name;
                cmd.Parameters["@Parts_OriginalName"].Value = this.OriginalName;
                cmd.Parameters["@Parts_ShortName"].Value = this.ShortName;
                cmd.Parameters["@Parts_Article"].Value = this.Article;
                //cmd.Parameters["@Parts_Certificate"].Value = this.Certificate;
                cmd.Parameters["@Parts_CodeTNVD"].Value = this.CodeTNVD;
                cmd.Parameters["@Parts_Reference"].Value = this.Reference;
                cmd.Parameters["@Currency_Guid"].Value = this.Currency.ID;
                cmd.Parameters["@Measure_Guid"].Value = this.Measure.ID;
                cmd.Parameters["@Owner_Guid"].Value = this.ProductTradeMark.ID;
                cmd.Parameters["@Parttype_Guid"].Value = this.ProductType.ID;
                cmd.Parameters["@Partsubtype_Guid"].Value = this.ProductSubType.ID;
                if (this.Country == null)
                {
                    cmd.Parameters["@Country_Guid"].IsNullable = true;
                    cmd.Parameters["@Country_Guid"].Value = null;
                }
                else
                {
                    cmd.Parameters["@Country_Guid"].IsNullable = false;
                    cmd.Parameters["@Country_Guid"].Value = this.Country.ID;
                }
                if (this.ProductCategory == null)
                {
                    cmd.Parameters["@PartsCategory_Guid"].IsNullable = true;
                    cmd.Parameters["@PartsCategory_Guid"].Value = null;
                }
                else
                {
                    cmd.Parameters["@PartsCategory_Guid"].IsNullable = false;
                    cmd.Parameters["@PartsCategory_Guid"].Value = this.ProductCategory.ID;
                }
                if ( this.ImageProduct != null )
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Image", System.Data.SqlDbType.Image));

                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    System.Drawing.Bitmap tmp = new System.Drawing.Bitmap(this.ImageProduct);

                    tmp.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                    int lung = Convert.ToInt32( ms.Length );
                    byte[] arAttach = ms.ToArray();

                    ms.Close();
                    ms = null;
                    tmp = null;

                    cmd.Parameters["@Parts_Image"].Value = arAttach;
                }
                cmd.Parameters["@Parts_PackQuantity"].Value = this.PackQuantity;
                cmd.Parameters["@Parts_PackQuantityForCalc"].Value = this.PackQuantityForCalc;
                cmd.Parameters["@Parts_BoxQuantity"].Value = this.BoxQuantity;
                cmd.Parameters["@Parts_Weight"].Value = this.Weight;
                cmd.Parameters["@Parts_PlasticContainerWeight"].Value = this.PlasticContainerWeight;
                cmd.Parameters["@Parts_PaperContainerWeight"].Value = this.PaperContainerWeight;
                cmd.Parameters["@Parts_AlcoholicContentPercent"].Value = this.AlcoholicContentPercent;
                cmd.Parameters["@Parts_VendorPrice"].Value = this.VendorPrice;
                cmd.Parameters["@Parts_IsActive"].Value = this.IsActive;
                cmd.Parameters["@Parts_NotValid"].Value = this.IsNotValid;
                cmd.Parameters["@Parts_ActualNotValid"].Value = this.IsActualNotValid;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    // теперь штрих-коды
                    cmd.Parameters.Clear();
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteBarCodesFromPart]", objProfile.GetOptionsDllDBName());
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Guid", System.Data.DbType.Guid));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters["@Parts_Guid"].Value = this.ID;
                    cmd.ExecuteNonQuery();
                    iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                    if ((iRes == 0) && (this.BarcodeList != null) && (this.BarcodeList.Count > 0))
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddBarCodeToPart]", objProfile.GetOptionsDllDBName());
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Guid", System.Data.DbType.Guid));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BARCODE_TEXT", System.Data.DbType.String));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                        cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters["@Parts_Guid"].Value = this.ID;
                        foreach (System.String strItem in this.BarcodeList)
                        {
                            cmd.Parameters["@BARCODE_TEXT"].Value = strItem;
                            cmd.ExecuteNonQuery();
                            iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                            if (iRes != 0) { break; }
                        }
                    }

                    if (iRes == 0)
                    {
                        // подтверждаем транзакцию
                        DBTransaction.Commit();
                    }
                    else
                    {
                        DBTransaction.Rollback();
                        strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                        DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения записи с информацией о товаре. Текст ошибки: " + strErr, "Ошибка",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения записи с информацией о товаре. Текст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства записи с информацией о товаре. Текст ошибки: " + f.Message +
                ( (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value ), "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }

        public  System.Boolean UpdateInIB(UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;
            if (IsAllParametersValid() == false) { return bRet; }

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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditPartsToIB2]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Id", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PARTS_ORIGNAME", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_ShortName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Article", System.Data.DbType.String));
                //cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PARTS_CERTIFICATE", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PARTS_CODETNVD", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PARTS_REFERENCE", System.Data.DbType.String));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PARTS_PACKQTY", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PARTS_PACKQTYFORCALC", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PARTS_BOXQTY", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PARTS_WEIGHT", System.Data.SqlDbType.Decimal));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PARTS_PLASTICCONTAINERWEIGHT", System.Data.SqlDbType.Decimal));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PARTS_PAPERCONTAINERWEIGHT", System.Data.SqlDbType.Decimal));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PARTS_ALCOHOLICCONTENTPERCENT", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PARTS_VENDORPRICE", System.Data.SqlDbType.Money));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PARTS_NOTVALID", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PARTS_ACTUALNOTVALID", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CURRENCY_CODE", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@MEASURE_GUID", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@COUNTRY_PRO_NAME", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PARTSCATEGORY_ID", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OWNER_ID", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PARTTYPE_ID", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PARTSUBTYPE_ID", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Parts_Guid"].Value = this.ID;
                cmd.Parameters["@Parts_Id"].Value = this.ID_Ib;
                cmd.Parameters["@Parts_Name"].Value = this.Name;
                cmd.Parameters["@PARTS_ORIGNAME"].Value = this.OriginalName;
                cmd.Parameters["@Parts_ShortName"].Value = this.ShortName;
                cmd.Parameters["@Parts_Article"].Value = this.Article;
                //cmd.Parameters["@PARTS_CERTIFICATE"].Value = this.Certificate;
                cmd.Parameters["@PARTS_CODETNVD"].Value = this.CodeTNVD;
                cmd.Parameters["@PARTS_REFERENCE"].Value = this.Reference;
                cmd.Parameters["@CURRENCY_CODE"].Value = this.Currency.CurrencyAbbr;
                cmd.Parameters["@MEASURE_GUID"].Value = this.Measure.ID;
                cmd.Parameters["@OWNER_ID"].Value = this.ProductTradeMark.ID_Ib;
                cmd.Parameters["@PARTTYPE_ID"].Value = this.ProductType.ID_Ib;
                cmd.Parameters["@PARTSUBTYPE_ID"].Value = this.ProductSubType.ID_Ib;
                if (this.Country == null)
                {
                    cmd.Parameters["@COUNTRY_PRO_NAME"].IsNullable = true;
                    cmd.Parameters["@COUNTRY_PRO_NAME"].Value = null;
                }
                else
                {
                    cmd.Parameters["@COUNTRY_PRO_NAME"].IsNullable = false;
                    cmd.Parameters["@COUNTRY_PRO_NAME"].Value = this.Country.Name;
                }
                if (this.ProductCategory == null)
                {
                    cmd.Parameters["@PARTSCATEGORY_ID"].IsNullable = true;
                    cmd.Parameters["@PARTSCATEGORY_ID"].Value = null;
                }
                else
                {
                    cmd.Parameters["@PARTSCATEGORY_ID"].IsNullable = false;
                    cmd.Parameters["@PARTSCATEGORY_ID"].Value = this.ProductCategory.ID_Ib;
                }
                cmd.Parameters["@PARTS_PACKQTY"].Value = this.PackQuantity;
                cmd.Parameters["@PARTS_PACKQTYFORCALC"].Value = this.PackQuantityForCalc;
                cmd.Parameters["@PARTS_BOXQTY"].Value = this.BoxQuantity;
                cmd.Parameters["@PARTS_WEIGHT"].Value = this.Weight;
                cmd.Parameters["@PARTS_PLASTICCONTAINERWEIGHT"].Value = this.PlasticContainerWeight;
                cmd.Parameters["@PARTS_PAPERCONTAINERWEIGHT"].Value = this.PaperContainerWeight;
                cmd.Parameters["@PARTS_ALCOHOLICCONTENTPERCENT"].Value = this.AlcoholicContentPercent;
                cmd.Parameters["@PARTS_VENDORPRICE"].Value = this.VendorPrice;
                cmd.Parameters["@PARTS_NOTVALID"].Value = this.IsNotValid;
                cmd.Parameters["@PARTS_ACTUALNOTVALID"].Value = this.IsActualNotValid;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения записи с информацией о товаре. Текст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                else
                {
                    // теперь штрих-коды
                    cmd.Parameters.Clear();
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteBarCodesFromPartsInIB]", objProfile.GetOptionsDllDBName());
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Guid", System.Data.DbType.Guid));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Id", System.Data.SqlDbType.Int));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters["@Parts_Guid"].Value = this.ID;
                    cmd.Parameters["@Parts_Id"].Value = this.ID_Ib;
                    cmd.ExecuteNonQuery();
                    iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                    if ((iRes == 0) && (this.BarcodeList != null) && (this.BarcodeList.Count > 0 ))
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddBarCodesFromPartsInIB]", objProfile.GetOptionsDllDBName());
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Guid", System.Data.DbType.Guid));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Id", System.Data.SqlDbType.Int));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BARCODE_TEXT", System.Data.DbType.String));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                        cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters["@Parts_Guid"].Value = this.ID;
                        cmd.Parameters["@Parts_Id"].Value = this.ID_Ib;
                        foreach (System.String strItem in this.BarcodeList)
                        {
                            cmd.Parameters["@BARCODE_TEXT"].Value = strItem;
                            cmd.ExecuteNonQuery();
                            iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                            if (iRes != 0)
                            {
                                strErr += (System.Convert.ToString(cmd.Parameters["@ERROR_MES"].Value));
                                break; 
                            }
                        }
                    }

                }
                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства записи с информацией о товаре в программе \"Контракт\". Текст ошибки: " + f.Message +
                ((cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value), "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        /// <summary>
        /// Сохраняет в БД состав набора
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="objChildProductList">модержимое набора</param>
        /// <param name="uuidParentProductId">уи набора</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean SaveProductKit(UniXP.Common.CProfile objProfile,
           List<CProduct> objChildProductList, System.Guid uuidParentProductId, ref System.String strErr)
        {
            System.Boolean bRet = false;
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

                System.Data.DataTable addedItems = new System.Data.DataTable();
                addedItems.Columns.Add(new System.Data.DataColumn("Product_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedItems.Columns.Add(new System.Data.DataColumn("PARTSDETAIL_ORDERID", typeof(System.Data.SqlTypes.SqlInt32)));
                addedItems.Columns.Add(new System.Data.DataColumn("PARTSDETAIL_QUANTITY", typeof(System.Data.SqlTypes.SqlInt32)));

                System.Data.DataRow newRow = null;
                foreach (CProduct objItem in objChildProductList)
                {
                    newRow = addedItems.NewRow();
                    newRow["Product_Guid"] = objItem.ID;
                    newRow["PARTSDETAIL_ORDERID"] = objChildProductList.IndexOf(objItem);
                    newRow["PARTSDETAIL_QUANTITY"] = objItem.QuantityInKit;
                    addedItems.Rows.Add(newRow);
                }
                addedItems.AcceptChanges();

                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_SetProductKit]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ParentParts_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.AddWithValue("@tProductKit", addedItems);
                cmd.Parameters["@tProductKit"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tProductKit"].TypeName = "dbo.udt_ProductKit";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@ParentParts_Guid"].Value = uuidParentProductId;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                strErr = (cmd.Parameters["@ERROR_MES"].Value != System.DBNull.Value) ? (System.String)cmd.Parameters["@ERROR_MES"].Value : "";

                cmd.Dispose();

                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                strErr = "Не удалось сохранить состав набора. Текст ошибки: " + f.Message;
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeletePart]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Parts_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления записи с информацией о товаре.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить запись с информацией о товаре.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        #endregion

        #region Генерация штрих-кода
        /// <summary>
        /// Возвращает новый штрих-код
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>строка со сштрих-кодом</returns>
        public static System.String GenerateBarCode( UniXP.Common.CProfile objProfile, ref System.String strErr )
        {
            System.String strRet = "";

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    strErr = "Не удалось получить соединение с базой данных.";
                    return strRet;
                }
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GenerateBarCode]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@strBarcodeNew", System.Data.SqlDbType.NVarChar, 13));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@strBarcodeNew"].Direction = System.Data.ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    strRet =(System.String)cmd.Parameters["@strBarcodeNew"].Value;
                }
                else
                {
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                strErr = f.Message;
            }
            finally
            {
                DBConnection.Close();
            }
            return strRet;
        }
        #endregion

        public override string ToString()
        {
            return ProductFullName;
        }
    }


    /// <summary>
    /// Строка настройки для операций с товарами
    /// </summary>
    public class CSettingItemForOperationWithParts
    {
        #region Свойства
        /// <summary>
        /// Название параметра
        /// </summary>
        private System.String m_strParamName;
        /// <summary>
        /// Название параметра
        /// </summary>
        public System.String ParamName
        {
            get { return m_strParamName; }
            set { m_strParamName = value; }
        }
        /// <summary>
        /// Номер столбца
        /// </summary>
        private System.Int32 m_iColumnID;
        /// <summary>
        /// Номер столбца
        /// </summary>
        public System.Int32 ColumnID
        {
            get { return m_iColumnID; }
            set { m_iColumnID = value; }
        }

        #endregion

        #region Конструктор
        public CSettingItemForOperationWithParts()
        {
            m_iColumnID = 0;
            m_strParamName = "";
        }
        public CSettingItemForOperationWithParts(System.Int32 iColumnID, System.String strParamName)
        {
            m_iColumnID = iColumnID;
            m_strParamName = strParamName;
        }
        #endregion
    }

    /// <summary>
    /// Класс "Настройки для расчета цен"
    /// </summary>
    public class CSettingForOperationWithParts
    {
        #region Постоянные
        public static readonly System.String strImportSettingName = "импорт данных в справочник товаров";
        public static readonly System.String strImportParamNameStartRow = "Начальная строка  данными";
        public static readonly System.String strImportParamNameID = "Код товара";
        public static readonly System.String strImportParamNameReference = "Референс";
        public static readonly System.String strImportParamNameProductName = "Наименование товара";
        public static readonly System.String strImportParamNameAlcohol = "Спирт, %";
        public static readonly System.String strImportParamNameProductArticle = "Артикул товара";
        public static readonly System.String strImportParamNameBarCode = "Штрих-код";
        public static readonly System.String strImportParamNameCountryImport = "Страна ввоза";
        public static readonly System.String strImportParamNameProductOriginalName = "Наименование товара оригинальное";
        public static readonly System.String strImportParamNamePartTypeID = "Код ассортиментной группы";
        public static readonly System.String strImportParamNameOwnerID = "Код ТМ";
        public static readonly System.String strImportParamNamePartTypeName = "Ассортиментная группа";
        public static readonly System.String strImportParamNameOwnerName = "ТМ";
        public static readonly System.String strImportParamNameMeasure = "Единица измерения";
        public static readonly System.String strImportParamNamePackQTY = "Количество товара в коробке, штук";
        public static readonly System.String strImportParamNamePackQuantityForCalc = "Количество в упаковке (для расчёта), штук";
        public static readonly System.String strImportParamNameWeigth = "Вес ед., кг.";
        public static readonly System.String strImportParamNameCustom = "Таможенный тариф, %";
        public static readonly System.String strImportParamNameCurrency = "Валюта";
        public static readonly System.String strImportParamNamePrice = "Цена";
        public static readonly System.String strImportParamNamePartLineID = "Код товарной линии";
        public static readonly System.String strImportParamNamePartLineName = "Товарная линия";
        public static readonly System.String strImportParamNamePartSubTypeID = "Код товарной подгруппы";
        public static readonly System.String strImportParamNamePartSubTypeName = "Товарная подгруппа";
        public static readonly System.String strImportParamNameVTMID = "Код ВТМ";
        public static readonly System.String strImportParamNameVTMName = "ВТМ";
        public static readonly System.String strImportParamNameNDSValue = "НДС, %";
        public static readonly System.String strImportParamNamePriceExw = "Цена exw";
        public static readonly System.String strImportParamNameCodeTNVED = "Код ТНВЭД";
        #endregion

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
        /// Список "параметр = значение"
        /// </summary>
        private List<CSettingItemForOperationWithParts> m_objSettingsList;
        /// <summary>
        /// Список "параметр = значение"
        /// </summary>
        public List<CSettingItemForOperationWithParts> SettingsList
        {
            get { return m_objSettingsList; }
            set { m_objSettingsList = value; }
        }
        /// <summary>
        /// Настройка в виде xml
        /// </summary>
        private System.Xml.XmlDocument m_objXMLSettings;
        /// <summary>
        /// Настройка в виде xml
        /// </summary>
        public System.Xml.XmlDocument XMLSettings
        {
            get { return m_objXMLSettings; }
            set { m_objXMLSettings = value; }
        }
        /// <summary>
        /// Возвращает номер столбца для параметра
        /// </summary>
        /// <param name="strParamName">Имя параметра</param>
        /// <returns>номер столбца</returns>
        public System.Int32 GetColumnNumForParam(System.String strParamName)
        {
            System.Int32 iRes = 0;
            try
            {
                if (m_objSettingsList == null) { return iRes; }
                m_objSettingsList.Single<CSettingItemForOperationWithParts>(x => x.ParamName == strParamName);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "GetColumnNumForParam.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return iRes;
        }
        /// <summary>
        /// Возвращает номер столбца для параметра
        /// </summary>
        /// <param name="strParamName">Имя параметра</param>
        /// <param name="objTreeList">дерево со списком параметров</param>
        /// <param name="objParamNameColumn">столбец с названиями параметров</param>
        /// <param name="objColumnId">столбец с номерами столбцов Excel</param>
        /// <returns>номер столбца</returns>
        public static System.Int32 GetColumnNumForParam(System.String strParamName,
            DevExpress.XtraTreeList.TreeList objTreeList,
            DevExpress.XtraTreeList.Columns.TreeListColumn objParamNameColumn,
            DevExpress.XtraTreeList.Columns.TreeListColumn objColumnId
             )
        {
            System.Int32 iRes = 0;
            try
            {
                if (objTreeList == null) { return iRes; }
                if (objParamNameColumn == null) { return iRes; }
                if (objColumnId == null) { return iRes; }
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in objTreeList.Nodes)
                {
                    if (System.Convert.ToString(objNode.GetValue(objParamNameColumn)) == strParamName)
                    {
                        iRes = System.Convert.ToInt32(objNode.GetValue(objColumnId));
                        break;
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "GetColumnNumForParam.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return iRes;
        }


        #endregion

        #region Конструктор
        public CSettingForOperationWithParts()
        {
            m_uuidID = System.Guid.Empty;
            m_objSettingsList = null;
            m_objXMLSettings = null;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список настроек
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список настроек</returns>
        public static List<CSettingForOperationWithParts> GetSettingForOperationWithPartsList( UniXP.Common.CProfile objProfile, 
            System.Data.SqlClient.SqlCommand cmdSQL )
        {
            List<CSettingForOperationWithParts> objList = new List<CSettingForOperationWithParts>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartsOperationSettings]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CSettingForOperationWithParts objSettingForCalcPrice = null;
                    while (rs.Read())
                    {
                        objSettingForCalcPrice = new CSettingForOperationWithParts();
                        objSettingForCalcPrice.ID = (System.Guid)rs["PartsOperationSettings_Guid"];
                        objSettingForCalcPrice.SettingsList = new List<CSettingItemForOperationWithParts>();

                        objSettingForCalcPrice.XMLSettings = new System.Xml.XmlDocument();
                        objSettingForCalcPrice.XMLSettings.LoadXml(rs.GetSqlXml(2).Value);

                        foreach (System.Xml.XmlNode objNode in objSettingForCalcPrice.XMLSettings.ChildNodes)
                        {
                            foreach (System.Xml.XmlNode objChildNode in objNode.ChildNodes)
                            {
                                objSettingForCalcPrice.SettingsList.Add(new CSettingItemForOperationWithParts(System.Convert.ToInt32(objChildNode.Attributes["ColumnNum"].Value), objChildNode.Attributes["CalcParamName"].Value));
                            }
                        }

                        objList.Add(objSettingForCalcPrice);

                    }

                    objSettingForCalcPrice = null;
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
                "Не удалось получить список настроек.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Возвращает настройку по заданному имени
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strSettingName">Наименование настройки</param>
        /// <returns>настройка</returns>
        public static CSettingForOperationWithParts GetSettingForOperationWithPartsByName(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.String strSettingName)
        {
            CSettingForOperationWithParts objSetting = null;
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
                        return objSetting;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartsOperationSettings]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsOperationSettings_Name", System.Data.SqlDbType.Text));
                cmd.Parameters["@PartsOperationSettings_Name"].Value = strSettingName;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    rs.Read();
                    {
                        objSetting = new CSettingForOperationWithParts();
                        objSetting.ID = (System.Guid)rs["PartsOperationSettings_Guid"];
                        objSetting.SettingsList = new List<CSettingItemForOperationWithParts>();

                        objSetting.XMLSettings = new System.Xml.XmlDocument();
                        objSetting.XMLSettings.LoadXml(rs.GetSqlXml(2).Value);

                        foreach (System.Xml.XmlNode objNode in objSetting.XMLSettings.ChildNodes)
                        {
                            foreach (System.Xml.XmlNode objChildNode in objNode.ChildNodes)
                            {
                                objSetting.SettingsList.Add(new CSettingItemForOperationWithParts(System.Convert.ToInt32(objChildNode.Attributes["ColumnNum"].Value), objChildNode.Attributes["CalcParamName"].Value));
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
                "Не удалось получить настройку.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objSetting;
        }
        #endregion

        #region Сохранение настроек в базе данных
        /// <summary>
        /// Сохраняет в БД настройки для экспорта тарифов товарных подгрупп в MS Excel
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">строка с сообщением об ошибке</param>
        /// <returns>true - успешное завершение операции; false - ошибка</returns>
        public System.Boolean SaveExportSettingForCalcPriceList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            ref System.String strErr)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditPartsOperationSettings]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsOperationSettings_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsOperationSettings_XML", System.Data.SqlDbType.Xml));
                cmd.Parameters["@PartsOperationSettings_Guid"].Value = this.ID;
                cmd.Parameters["@PartsOperationSettings_XML"].Value = this.XMLSettings.InnerXml;
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
    }

    /// <summary>
    /// Класс "Тип заказа"
    /// </summary>
    public class COrderType
    {
        #region Свойства
        /// <summary>
        /// уникальный идентификатор
        /// </summary>
        private System.Guid m_uuidId;
        /// <summary>
        /// уникальный идентификатор
        /// </summary>
        public System.Guid Id
        {
            get { return m_uuidId; }
            set { m_uuidId = value; }
        }
        /// <summary>
        /// Имя
        /// </summary>
        private System.String m_strName;
        /// <summary>
        /// Имя
        /// </summary>
        public System.String Name
        {
            get { return m_strName; }
            set { m_strName = value; }
        }
        #endregion

        #region Конструктор
        public COrderType(System.Guid uuidId, System.String strName)
        {
            m_uuidId = uuidId;
            m_strName = strName;
        }
        #endregion

        #region Список типов заказа
        /// <summary>
        /// возвращает список типов заказов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <returns>список типов заказов</returns>
        public static List<COrderType> GetOrderTypeList(UniXP.Common.CProfile objProfile)
        {
            List<COrderType> objList = new List<COrderType>();
            // подключаемся к БД
            System.Data.SqlClient.SqlConnection DBConnection = objProfile.GetDBSource();
            if (DBConnection == null)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Отсутствует соединение с БД.", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return objList;
            }

            try
            {
                // соединение с БД получено, прописываем команду на выборку данных
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetOrderType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();

                if (rs.HasRows)
                {

                    while (rs.Read())
                    {
                        objList.Add(new COrderType((System.Guid)rs["OrderType_Guid"], (System.String)rs["OrderType_Name"]));
                    }
                }
                rs.Close();
                rs.Dispose();
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список типов заказов.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
			finally // очищаем занимаемые ресурсы
            {
                DBConnection.Close();
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
    /// Привязка товара к складам
    /// </summary>
    public class CProductLinkToStock
    {
        #region Свойства
        /// <summary>
        /// Склад 
        /// </summary>
        private CStock m_objStock;
        /// <summary>
        /// Склад 
        /// </summary>
        public CStock Stock
        {
            get { return m_objStock; }
            set { m_objStock = value; }
        }
        /// <summary>
        /// Тип заказа
        /// </summary>
        private COrderType m_objOrderType;
        /// <summary>
        /// Тип заказа
        /// </summary>
        public COrderType OrderType
        {
            get { return m_objOrderType; }
            set { m_objOrderType = value; }
        }
        /// <summary>
        /// Признак "назначен"
        /// </summary>
        private System.Boolean m_bLink;
        /// <summary>
        /// Признак "назначен"
        /// </summary>
        public System.Boolean bLink
        {
            get { return m_bLink; }
            set { m_bLink = value; }
        }

        #endregion

        #region Список привязок к складам для товара
        /// <summary>
        /// Возвращает список привязок к складам для товара
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objProduct">товар</param>
        /// <returns>список привязок к складам для товара</returns>
        public static List<CProductLinkToStock> GetProductLinkToStockList(UniXP.Common.CProfile objProfile, 
            System.Data.SqlClient.SqlCommand cmdSQL, CProduct objProduct)
        {
            List<CProductLinkToStock> objList = new List<CProductLinkToStock>();
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Int32 iPartsId = 0;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetProductLinkToStock]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Parts_Guid"].Value = objProduct.ID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CProductLinkToStock objProductLinkToStock = null;

                    while (rs.Read())
                    {
                        objProductLinkToStock = new CProductLinkToStock();
                        objProductLinkToStock.Stock = new CStock();
                        objProductLinkToStock.Stock.ID = (System.Guid)rs["Stock_Guid"];
                        objProductLinkToStock.Stock.Name = (System.String)rs["Stock_Name"];
                        objProductLinkToStock.Stock.WareHouse = new CWarehouse();
                        objProductLinkToStock.Stock.WareHouse.ID = (System.Guid)rs["Warehouse_Guid"];
                        objProductLinkToStock.Stock.WareHouse.Name = (System.String)rs["Warehouse_Name"];
                        objProductLinkToStock.Stock.Company = new CCompany();
                        objProductLinkToStock.Stock.Company.ID = (System.Guid)rs["Company_Guid"];
                        objProductLinkToStock.Stock.Company.Abbr = (System.String)rs["Company_Acronym"];
                        objProductLinkToStock.OrderType = new COrderType((System.Guid)rs["OrderType_Guid"], (System.String)rs["OrderType_Name"]);
                        objProductLinkToStock.bLink = System.Convert.ToBoolean(rs["bLink"]);
                        objList.Add(objProductLinkToStock);
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
                "Не удалось получить список привязок к складам для товара.\n\nТекст ошибки для товара с кодом: " + iPartsId.ToString() + " - " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        #endregion

        #region Привязка товара к складам отгрузки
        /// <summary>
        /// Устанавливает привязку товара к складам отгрузки
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objProduct">товар</param>
        /// <param name="objListOnlyLinkTrue">объект "привязка к складам"</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean SetProductLinkToStockList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, CProduct objProduct, List<CProductLinkToStock> objListOnlyLinkTrue, 
            ref System.String strErr )
        {
            System.Boolean bRet = false;

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Int32 iPartsId = 0;
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
                // сперва разорвем привязку
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteProductLinkToStock]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Parts_Guid"].Value = objProduct.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if( (iRes == 0) && (objListOnlyLinkTrue != null) && (objListOnlyLinkTrue.Count > 0))
                {
                    // а теперь установим привязку
                    cmd.Parameters.Clear();
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddProductLinkToStock]", objProfile.GetOptionsDllDBName());
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@OrderType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters["@Parts_Guid"].Value = objProduct.ID;
                    foreach (CProductLinkToStock objLink in objListOnlyLinkTrue )
                    {
                        if (objLink.bLink == true)
                        {
                            iRes = 0;
                            cmd.Parameters["@Stock_Guid"].Value = objLink.Stock.ID;
                            cmd.Parameters["@OrderType_Guid"].Value = objLink.OrderType.Id;
                            cmd.ExecuteNonQuery();
                            iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                            if (iRes != 0) { break; }
                        }
                    }

                }

                if (iRes != 0)
                {
                    strErr = "Не удалось сохранить привязку товара к складам отгрузки. Текст ошибки :" +  (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                bRet = (iRes == 0);

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список привязок к складам для товара.\n\nТекст ошибки для товара с кодом: " + iPartsId.ToString() + " - " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return bRet;
        }
        #endregion

        public override string ToString()
        {
            System.String strWareHouse = "" ;
            System.String strCompany = "";

            if ((m_objStock != null) && (m_objStock.WareHouse != null)) { strWareHouse = m_objStock.WareHouse.Name; }
            if ((m_objStock != null) && (m_objStock.Company != null)) { strCompany = m_objStock.Company.Abbr; }

            return( strWareHouse + " - " + strCompany );            
        }

    }

}
