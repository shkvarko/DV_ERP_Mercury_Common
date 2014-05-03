using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ERP_Mercury.Common
{
    public enum enumCertificateActionTypeWithImage
    {
        Nothing = 0,
        EditImage = 1,
        DeleteImage = 2
    }

    /// <summary>
    /// Класс "Типы документов о качестве товара"
    /// </summary>
    public class CCertificateType : CBusinessObject
    {
        #region Свойства
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
        public CCertificateType()
            : base()
        {
            this.m_strDescription = "";
            this.m_bIsActive = true;
        }
        public CCertificateType(System.Guid uuidID, System.String strName, System.String strDescription, System.Boolean bIsActive)
        {
            this.ID = uuidID;
            this.Name = strName;
            this.m_strDescription = strDescription;
            this.m_bIsActive = bIsActive;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список типов документов о качестве товара
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список типов документов о качестве товара</returns>
        public static List<CCertificateType> GetCertificateTypeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CCertificateType> objList = new List<CCertificateType>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCertificateType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CCertificateType(
                            (System.Guid)rs["CertificateType_Guid"],
                            (System.String)rs["CertificateType_Name"], 
                            ( ( rs["CertificateType_Description"] == System.DBNull.Value ) ? "" : System.Convert.ToString(rs["CertificateType_Description"]) ),
                            System.Convert.ToBoolean(rs["CertificateType_IsActive"])
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
                "Не удалось получить список типов документов о качестве товаров.\n\nТекст ошибки: " + f.Message, "Внимание",
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddCertificateType]", objProfile.GetOptionsDllDBName());

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CertificateType_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CertificateType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CertificateType_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CertificateType_IsActive", System.Data.DbType.Boolean));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));//**
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@CertificateType_Name"].Value = this.Name;
                cmd.Parameters["@CertificateType_Description"].Value = this.Description;
                cmd.Parameters["@CertificateType_IsActive"].Value = this.IsActive;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }


                if (iRes != 0)
                {
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка добавления в справочник новой записи.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                strErr = f.Message;
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось добавить в справочник новую запись.\n\nТекст ошибки: " + f.Message, "Внимание",
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

        #region Update
        /// <summary>
        /// редактирование записи в справочнике типов сертификатов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public override System.Boolean Update(UniXP.Common.CProfile objProfile)
        {
            System.Boolean bRet = false;
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditCertificateType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CertificateType_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CertificateType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CertificateType_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CertificateType_IsActive", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@CertificateType_Guid"].Value = this.ID;
                cmd.Parameters["@CertificateType_Name"].Value = this.Name;
                cmd.Parameters["@CertificateType_Description"].Value = this.Description;
                cmd.Parameters["@CertificateType_IsActive"].Value = this.IsActive;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                if (iRes != 0)
                {
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств записи.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                strErr = f.Message;
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства записи в справочнике типов сертификатов.\n\nТекст ошибки: " + f.Message, "Внимание",
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
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public override System.Boolean Remove(UniXP.Common.CProfile objProfile)
        {
            System.Boolean bRet = false;
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteCertificateType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CertificateType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@CertificateType_Guid"].Value = this.ID;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes != 0)
                {
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления записи.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                strErr = f.Message;
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить запись в справочнике типов сертификатов.\n\nТекст ошибки: " + f.Message, "Внимание",
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

    public class CCertificate
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
        /// Тип документа о качестве
        /// </summary>
        private CCertificateType m_objCertificateType;
        /// <summary>
        /// Тип документа о качестве
        /// </summary>
        public CCertificateType CertificateType
        {
            get { return m_objCertificateType; }
            set { m_objCertificateType = value; }
        }
        /// <summary>
        /// Номер
        /// </summary>
        private System.String m_strNum;
        /// <summary>
        /// Номер
        /// </summary>
        public System.String Num
        {
            get { return m_strNum; }
            set { m_strNum = value; }
        }
        /// <summary>
        /// Дата выдачи
        /// </summary>
        private System.DateTime m_dtBeginDate;
        /// <summary>
        /// Дата выдачи
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
        /// Каким органом выдан
        /// </summary>
        private System.String m_strWhoGive;
        /// <summary>
        /// Каким органом выдан
        /// </summary>
        public System.String WhoGive
        {
            get { return m_strWhoGive; }
            set { m_strWhoGive = value; }
        }
        /// <summary>
        /// Наименование для ТТН
        /// </summary>
        public System.String NumForWaybill { get; set; }
        /// <summary>
        /// Примечание
        /// </summary>
        public System.String Description { get; set; }
        /// <summary>
        /// Признак "Запись активна"
        /// </summary>
        public System.Boolean IsActive { get; set; }
        /// <summary>
        /// Наименование файла с изображением
        /// </summary>
        public System.String ImageCertificateFileName { get; set; }
        /// <summary>
        /// Изображение
        /// </summary>
        public byte[] ImageCertificate { get; set; }
        /// <summary>
        /// Признак "Наличие изображения"
        /// </summary>
        public System.Boolean ExistImage { get; set; }
        /// <summary>
        /// Действие над изображением
        /// </summary>
        public enumCertificateActionTypeWithImage ActionType { get; set; }
        public static readonly string STR_SetCertificateNum = "укажите номер документа...";
        public static readonly string STR_SetWhoGiveCertificate = "укажите орган, выдавший документ...";

        #endregion

        #region Конструктор
        public CCertificate()
        {
            m_uuidID = System.Guid.Empty;
            m_strNum = "";
            m_objCertificateType = null;
            m_dtBeginDate = System.DateTime.Today;
            m_dtEndDate = System.DateTime.MaxValue;
            m_strWhoGive = "";
            NumForWaybill = System.String.Empty;
            Description = System.String.Empty;
            IsActive = true;
            ImageCertificateFileName = System.String.Empty;
            ImageCertificate = null;
            ExistImage = false;
            ActionType = enumCertificateActionTypeWithImage.Nothing;
        }
        public CCertificate( CCertificateType objCertificateType, System.Guid uuidId, System.String strNum,  System.DateTime dtBeginDate,
           System.DateTime dtEndDate, System.String strWhoGive)
        {
            m_uuidID = uuidId;
            m_strNum = strNum;
            m_objCertificateType = objCertificateType;
            m_dtBeginDate = dtBeginDate;
            m_dtEndDate = dtEndDate;
            m_strWhoGive = strWhoGive;
            NumForWaybill = System.String.Empty;
            Description = System.String.Empty;
            IsActive = true;
            ImageCertificateFileName = System.String.Empty;
            ImageCertificate = null;
            ExistImage = false;
            ActionType = enumCertificateActionTypeWithImage.Nothing;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список документов о качестве товара
        /// </summary>
        /// <param name="objProduct">товар</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список типов документов о качестве товара</returns>
        public static List<CCertificate> GetCertificateTypeList( CProduct objProduct, UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CCertificate> objList = new List<CCertificate>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartsCertificate]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Parts_Guid"].Value = objProduct.ID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CCertificate(new CCertificateType(
                            (System.Guid)rs["CertificateType_Guid"],
                            (System.String)rs["CertificateType_Name"],
                            ((rs["CertificateType_Description"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["CertificateType_Description"])),
                            System.Convert.ToBoolean(rs["CertificateType_IsActive"])
                            ), (System.Guid)rs["Certificate_Guid"], System.Convert.ToString(rs["Certificate_Num"]),
                            System.Convert.ToDateTime(rs["Certificate_BeginDate"]),
                            ((rs["Certificate_EndDate"] == System.DBNull.Value) ? System.DateTime.MaxValue : System.Convert.ToDateTime(rs["Certificate_EndDate"])),
                            System.Convert.ToString(rs["Certificate_WhoGive"])
                            ) 
                            { 
                                NumForWaybill = ((rs["Certificate_NumForWaybill"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Certificate_NumForWaybill"]) ),
                                Description = ((rs["Certificate_Description"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Certificate_Description"])),
                                IsActive = ((rs["Certificate_IsActive"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["Certificate_IsActive"])),
                                ExistImage = ((rs["ExistImage"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["ExistImage"])), 
                                ImageCertificateFileName = ((rs["Certificate_ImageFileFullName"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Certificate_ImageFileFullName"]) )
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
                "Не удалось получить список документов о качестве товаров.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }

        /// <summary>
        /// Загружает из БД изображение товара
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="uuidCertificateID">УИ сертификата</param>
        /// <param name="arAttach">изображение в виде массива байт</param>
        /// <param name="strCertificateFileName">название файла с изображением</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean LoadImageFromDB(UniXP.Common.CProfile objProfile, System.Guid uuidCertificateID,
            ref byte[] arAttach, ref System.String strCertificateFileName, 
            ref System.String strErr)
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
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand() 
                { 
                    Connection = DBConnection, CommandType = System.Data.CommandType.StoredProcedure,
                    CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartCertificateImage]", objProfile.GetOptionsDllDBName()) 
                };
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Certificate_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Certificate_Guid"].Value = uuidCertificateID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    // набор данных непустой
                    rs.Read();
                    if (rs["Certificate_Image"] != System.DBNull.Value)
                    {
                        strCertificateFileName = ((rs["Certificate_ImageFileFullName"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["Certificate_ImageFileFullName"]));
                        arAttach = (byte[])rs["Certificate_Image"];
                    }

                }
                rs.Close();
                rs.Dispose();
                bRet = true;

            }
            catch (System.Exception f)
            {
                strErr = "Не удалось получить изображение сертификата. Текст ошибки: " + f.Message;
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }

        #endregion

        #region Привязка сертификатов к товару

        public static System.Boolean IsAllPropertiesCorrect(  CCertificate objCertificate, ref System.String strErr)
        {
            System.Boolean bRet = true;
            try
            {
                if((objCertificate.CertificateType == null) || (objCertificate.CertificateType.ID.CompareTo(System.Guid.Empty) == 0))
                {
                    strErr += ("\nНе указан тип документа о качестве товара.");
                    bRet = false;
                }
                if ((objCertificate.Num == System.String.Empty) || (objCertificate.Num == CCertificate.STR_SetCertificateNum))
                {
                    strErr += ("\nНе указан номер документа о качестве товара.");
                    bRet = false;
                }
                if ((objCertificate.WhoGive == System.String.Empty) || (objCertificate.WhoGive == CCertificate.STR_SetWhoGiveCertificate))
                {
                    strErr += ("\nНе указан государственный орган, выдавший документ о качестве товара.");
                    bRet = false;
                }
            }
            catch (System.Exception f)
            {
                strErr = "Проверка данных в сертификате. Текст ошибки: " + f.Message;
            }

            return bRet;
        }

        /// <summary>
        /// Привязывает список сертификатов к товару в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objCertificateList">список сертификатов</param>
        /// <param name="ProductId">УИ товара</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean AssignCertificateListWithProduct( UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            List<CCertificate> objCertificateList, System.Guid ProductId, ref System.String strErr)
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
                System.Data.DataTable addedCategories = new System.Data.DataTable();
                addedCategories.Columns.Add(new System.Data.DataColumn("Certificate_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedCategories.Columns.Add(new System.Data.DataColumn("CertificateType_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedCategories.Columns.Add(new System.Data.DataColumn("Certificate_Num", typeof(System.Data.SqlTypes.SqlString)));
                addedCategories.Columns.Add(new System.Data.DataColumn("Certificate_WhoGive", typeof(System.Data.SqlTypes.SqlString)));
                addedCategories.Columns.Add(new System.Data.DataColumn("Certificate_BeginDate", typeof(System.Data.SqlTypes.SqlDateTime)));
                addedCategories.Columns.Add(new System.Data.DataColumn("Certificate_EndDate", typeof(System.Data.SqlTypes.SqlDateTime)));

                addedCategories.Columns.Add(new System.Data.DataColumn("Certificate_NumForWaybill", typeof(System.Data.SqlTypes.SqlString)));
                addedCategories.Columns.Add(new System.Data.DataColumn("Certificate_IsActive", typeof(System.Data.SqlTypes.SqlBoolean)));
                addedCategories.Columns.Add(new System.Data.DataColumn("Certificate_Description", typeof(System.Data.SqlTypes.SqlString)));

                addedCategories.Columns.Add(new System.Data.DataColumn("Certificate_Image", typeof(System.Data.SqlTypes.SqlBinary)));
                addedCategories.Columns.Add(new System.Data.DataColumn("Certificate_ImageFileFullName", typeof(System.Data.SqlTypes.SqlString)));

                
                addedCategories.Columns.Add(new System.Data.DataColumn("ActionType_Id", typeof(System.Data.SqlTypes.SqlInt32)));

                System.Data.DataRow newRow = null;
                if ((objCertificateList != null) || (objCertificateList.Count > 0))
                {
                    foreach (CCertificate objItem in objCertificateList)
                    {
                        newRow = addedCategories.NewRow();
                        newRow["Certificate_Guid"] = objItem.ID;
                        newRow["CertificateType_Guid"] = objItem.CertificateType.ID;
                        newRow["Certificate_Num"] = objItem.Num;
                        newRow["Certificate_WhoGive"] = objItem.WhoGive;
                        newRow["Certificate_BeginDate"] = objItem.BeginDate;
                        if ((objItem.EndDate.CompareTo(System.DateTime.MaxValue) == 0) || (objItem.EndDate.CompareTo(System.DateTime.MinValue) == 0))
                        {
                            newRow["Certificate_EndDate"] = System.DBNull.Value;
                        }
                        else
                        {
                            newRow["Certificate_EndDate"] = objItem.EndDate;
                        }

                        newRow["Certificate_NumForWaybill"] = objItem.NumForWaybill;
                        newRow["Certificate_IsActive"] = objItem.IsActive;
                        newRow["Certificate_Description"] = objItem.Description;
                        newRow["ActionType_Id"] = System.Convert.ToInt32( objItem.ActionType );
                        if( objItem.ImageCertificate != null )
                        {
                            newRow["Certificate_Image"] = objItem.ImageCertificate;
                        }
                        newRow["Certificate_ImageFileFullName"] = objItem.ImageCertificateFileName;

                        addedCategories.Rows.Add(newRow);
                    }
                }
                addedCategories.AcceptChanges();

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AssignCertificateWithPart]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Guid", System.Data.SqlDbType.UniqueIdentifier));
                //cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CERTIFICATE_IMAGE", System.Data.SqlDbType.Image));

                cmd.Parameters.AddWithValue("@tCertificate", addedCategories);
                cmd.Parameters["@tCertificate"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tCertificate"].TypeName = "dbo.udt_Certificate";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Parts_Guid"].Value = ProductId;
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
        /// Вносит изменения в поле "Сертификат" в справочнике товаров
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strCertificate">информация о качестве товара</param>
        /// <param name="uuidProductId">уи товара</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean UpdateProductCertificate(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.String strCertificate, System.Guid uuidProductId, ref System.String strErr)
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

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditProductCertificate]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Parts_Certificate", System.Data.SqlDbType.NVarChar, 256));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Parts_Guid"].Value = uuidProductId;
                cmd.Parameters["@Parts_Certificate"].Value = strCertificate;
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

        #endregion

    }
}
