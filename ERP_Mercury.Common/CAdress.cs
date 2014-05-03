
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ERP_Mercury.Common
{

    #region Класс "Страна"
    /// <summary>
    /// Класс "Страна"
    /// </summary>
    public class CCountry : CBusinessObject
    {
        #region Свойства
        [DisplayName("УИ в InterBase")]
        [Description("уникальный идентификатор записи в InterBase")]
        [Category("1. Обязательные значения")]
        [ReadOnly(true)]
        public System.Int32 ID_Ib {get; set;}
        /// <summary>
        /// Описание
        /// </summary>
        private System.String m_strCode;
        /// <summary>
        /// Описание
        /// </summary>
        [DisplayName("Код страны (символьный)")]
        [Description("международный код страны (символьный)")]
        [Category("1. Обязательные значения")]
        public System.String Code
        {
            get { return m_strCode; }
            set { m_strCode = value; }
        }
        [DisplayName("Код страны (цифровой)")]
        [Description("международный код страны (цифровой)")]
        [Category("1. Обязательные значения")]
        public System.String CodeNumeric {get; set;}
        #endregion

        #region Конструктор
        public CCountry()
            : base()
        {
            m_strCode = "";
            CodeNumeric = "";
            ID_Ib = 0;
        }
        public CCountry(System.Guid uuidId, System.String strName, System.String strCode)
        {
            this.ID = uuidId;
            this.Name = strName;
            this.m_strCode = strCode;
            CodeNumeric = "";
            ID_Ib = 0;
        }
        #endregion

        #region Список объектов
        public static List<CCountry> GetCountryList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CCountry> objList = new List<CCountry>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetCountry]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CCountry((System.Guid)rs["Country_Guid"],
                            (System.String)rs["Country_Name"], (System.String)rs["Country_Code"]) { CodeNumeric = (System.String)rs["Country_ISOCode"], ID_Ib = System.Convert.ToInt32(rs["Country_Id"]) });
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
                "Не удалось получить список стран.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                if (this.m_strCode == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать код!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.CodeNumeric == "")
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddCountry]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Country_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Country_Id", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Country_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Country_Code", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Country_ISOCode", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Country_Name"].Value = this.Name;
                cmd.Parameters["@Country_Code"].Value = this.m_strCode;
                cmd.Parameters["@Country_ISOCode"].Value = this.CodeNumeric;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if ((iRes == 0) || (iRes == 1))
                {
                    this.ID = (System.Guid)cmd.Parameters["@Country_Guid"].Value;
                    this.ID_Ib = System.Convert.ToInt32(cmd.Parameters["@Country_Id"].Value);

                    if (iRes == 1)
                    {
                        strErr = this.Name + " присутствует в базе данных. Код страны: " + this.ID_Ib.ToString();
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
                strErr = ("Не удалось зарегистрировать информацию о стране.\n\nТекст ошибки: " + f.Message);
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteCountry]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Country_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Country_Guid"].Value = this.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                bRet = (iRes == 0);


                if (bRet == false)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления информации о стране.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить информацию о стране.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditCountry]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Country_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Country_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Country_Code", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Country_ISOCode", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Country_Guid"].Value = this.ID;
                cmd.Parameters["@Country_Name"].Value = this.Name;
                cmd.Parameters["@Country_Code"].Value = this.m_strCode;
                cmd.Parameters["@Country_ISOCode"].Value = this.CodeNumeric;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения информации о стране.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить информацию о стране.\n\nТекст ошибки: " + f.Message, "Внимание",
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

    #region Область

    /// <summary>
    /// TypeConverter для списка стран
    /// </summary>
    class CountryTypeConverter : TypeConverter
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

            COblast objOblast = (COblast)context.Instance;
            System.Collections.Generic.List<CCountry> objList = objOblast.GetAllCountryList();

            return new StandardValuesCollection(objList);
        }
    }

    /// <summary>
    /// Класс "Область"
    /// </summary>
    public class COblast : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Описание
        /// </summary>
        private System.String m_strCode;
        /// <summary>
        /// Описание
        /// </summary>
        [DisplayName("Код области")]
        [Description("Код области")]
        [Category("1. Обязательные значения")]
        public System.String Code
        {
            get { return m_strCode; }
            set { m_strCode = value; }
        }
        /// <summary>
        /// Страна
        /// </summary>
        private CCountry m_objCountry;
        /// <summary>
        /// Страна
        /// </summary>
        [DisplayName("Страна")]
        [Description("Страна, в которую входит регион")]
        [Category("1. Обязательные значения")]
        [TypeConverter(typeof(CountryTypeConverter))]
        [ReadOnly(false)]
        [BrowsableAttribute(false)]
        public CCountry Country
        {
            get { return m_objCountry; }
            set { m_objCountry = value; }
        }
        /// <summary>
        /// Страна
        /// </summary>
        [DisplayName("Страна")]
        [Description("Страна, в которую входит регион")]
        [Category("1. Обязательные значения")]
        [TypeConverter(typeof(CountryTypeConverter))]
        public System.String CountryName
        {
            get { return m_objCountry.Name; }
            set { SetCountryValue(value); }
        }
        private void SetCountryValue(System.String strCountryName)
        {
            try
            {
                if (m_objAllCountryList == null) { m_objCountry = null; }
                else
                {
                    foreach (CCountry objCountry in m_objAllCountryList)
                    {
                        if (objCountry.Name == strCountryName)
                        {
                            m_objCountry = objCountry;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось установить значение страны.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }
        private List<CCountry> m_objAllCountryList;
        #endregion

        public COblast()
            : base()
        {
            m_strCode = "";
            m_objCountry = null;
            m_objAllCountryList = null;
        }
        public COblast(System.Guid uuidId, System.String strName, System.String strCode, CCountry objCountry)
        {
            this.ID = uuidId;
            this.Name = strName;
            this.m_strCode = strCode;
            this.m_objCountry = objCountry;
            this.m_objAllCountryList = null;
        }

        #region Список объектов
        public List<CCountry> GetAllCountryList()
        {
            return m_objAllCountryList;
        }
        public static List<COblast> GetOblastList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<COblast> objList = new List<COblast>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetOblast]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new COblast((System.Guid)rs["Oblast_Guid"], (System.String)rs["Oblast_Name"], (System.String)rs["Oblast_Code"],
                            new CCountry((System.Guid)rs["Country_Guid"], (System.String)rs["Country_Name"], (System.String)rs["Country_Code"])));
                    }
                }
                rs.Dispose();

                List<CCountry> objCountry = CCountry.GetCountryList(objProfile, cmd);
                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
                if (objCountry != null)
                {
                    foreach (COblast objOblast in objList)
                    {
                        objOblast.m_objAllCountryList = objCountry;
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список областей.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Загружает в AllCountryList список сран
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        public void InitCountryList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            try
            {
                if (this.m_objAllCountryList != null) { this.m_objAllCountryList.Clear(); }
                this.m_objAllCountryList = CCountry.GetCountryList(objProfile, cmdSQL);
                if( (this.m_objAllCountryList != null) && (this.m_objAllCountryList.Count > 0))
                {
                    this.Country = this.m_objAllCountryList[0];
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось загрузить список стран.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return ;
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
                if (this.m_strCode == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать код!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.m_objCountry == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать страну!", "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddOblast]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Oblast_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Oblast_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Oblast_Code", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Country_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Oblast_Name"].Value = this.Name;
                cmd.Parameters["@Oblast_Code"].Value = this.m_strCode;
                cmd.Parameters["@Country_Guid"].Value = this.m_objCountry.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@Oblast_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания области.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать область.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteOblast]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Region_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Oblast_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления области.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить область.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditOblast]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Oblast_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Country_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Oblast_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Oblast_Code", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Oblast_Guid"].Value = this.ID;
                cmd.Parameters["@Oblast_Name"].Value = this.Name;
                cmd.Parameters["@Oblast_Code"].Value = this.m_strCode;
                cmd.Parameters["@Country_Guid"].Value = this.m_objCountry.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств области.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства области.\n\nТекст ошибки: " + f.Message, "Внимание",
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

    #region Класс "Регион"
    /// <summary>
    /// TypeConverter для списка областей
    /// </summary>
    class OblastTypeConverter : TypeConverter
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

            CRegion objRegion = (CRegion)context.Instance;
            System.Collections.Generic.List<COblast> objList = objRegion.GetAllOblastList();

            return new StandardValuesCollection(objList);
        }
    }


    /// <summary>
    /// Класс "Регион"
    /// </summary>
    public class CRegion : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Описание
        /// </summary>
        private System.String m_strCode;
        /// <summary>
        /// Описание
        /// </summary>
        [DisplayName("Код региона")]
        [Description("Код региона")]
        [Category("1. Обязательные значения")]
        public System.String Code
        {
            get { return m_strCode; }
            set { m_strCode = value; }
        }
        /// <summary>
        /// Область
        /// </summary>
        private COblast m_objOblast;
        /// <summary>
        /// Область
        /// </summary>
        [DisplayName("Область")]
        [Description("Страна, в которую входит район")]
        [Category("1. Обязательные значения")]
        [TypeConverter(typeof(OblastTypeConverter))]
        [ReadOnly(false)]
        [BrowsableAttribute(false)]
        public COblast Oblast
        {
            get { return m_objOblast; }
            set { m_objOblast = value; }
        }
        /// <summary>
        /// Область
        /// </summary>
        [DisplayName("Область")]
        [Description("Область, в которую входит район")]
        [Category("1. Обязательные значения")]
        [TypeConverter(typeof(OblastTypeConverter))]
        public System.String OblastName
        {
            get { return m_objOblast.Name; }
            set { SetOblastValue(value); }
        }
        private void SetOblastValue(System.String strOblastName)
        {
            try
            {
                if (m_objAllOblastList == null) { m_objOblast = null; }
                else
                {
                    foreach (COblast objOblast in m_objAllOblastList)
                    {
                        if (objOblast.Name == strOblastName)
                        {
                            m_objOblast = objOblast;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось установить значение области.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return ;
       }
        private List<COblast> m_objAllOblastList;
        #endregion

        public CRegion()
            : base()
        {
            m_strCode = "";
            m_objOblast = null;
            m_objAllOblastList = null;
        }
        public CRegion(System.Guid uuidId, System.String strName, System.String strCode, COblast objOblast)
        {
            this.ID = uuidId;
            this.Name = strName;
            this.m_strCode = strCode;
            this.m_objOblast = objOblast;
            this.m_objAllOblastList = null;
        }

        #region Список объектов
        public List<COblast> GetAllOblastList()
        {
            return m_objAllOblastList;
        }
        public static List<CRegion> GetRegionList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CRegion> objList = new List<CRegion>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetRegion]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CRegion( (System.Guid)rs["Region_Guid"], (System.String)rs["Region_Name"], (System.String)rs["Region_Code"],
                            new COblast((System.Guid)rs["Oblast_Guid"], (System.String)rs["Oblast_Name"], (System.String)rs["Oblast_Code"], 
                                new CCountry((System.Guid)rs["Country_Guid"], (System.String)rs["Country_Name"], (System.String)rs["Country_Code"])
                            )));
                    }
                }
                rs.Dispose();

                List<COblast> objOblast = COblast.GetOblastList(objProfile, cmd);
                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
                if (objOblast != null)
                {
                    foreach (CRegion objRegion in objList)
                    {
                        objRegion.m_objAllOblastList = objOblast;
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список регионов.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Загружает в m_objAllOblastList список областей
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        public void InitOblastList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            try
            {
                this.m_objAllOblastList = null;
                this.m_objAllOblastList = COblast.GetOblastList(objProfile, cmdSQL);
                if ((this.m_objAllOblastList != null) && (this.m_objAllOblastList.Count > 0))
                {
                    this.Oblast = this.m_objAllOblastList[0];
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось загрузить список областей.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return ;
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
                if (this.m_strCode == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать код!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.m_objOblast == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать область!", "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddRegion]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Region_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Region_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Region_Code", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Oblast_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Region_Name"].Value = this.Name;
                cmd.Parameters["@Region_Code"].Value = this.m_strCode;
                cmd.Parameters["@Oblast_Guid"].Value = this.m_objOblast.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@Region_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания региона.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать регион.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteRegion]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Region_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Region_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления региона.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditRegion]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Region_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Oblast_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Region_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Region_Code", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Region_Guid"].Value = this.ID;
                cmd.Parameters["@Region_Name"].Value = this.Name;
                cmd.Parameters["@Region_Code"].Value = this.m_strCode;
                cmd.Parameters["@Oblast_Guid"].Value = this.m_objOblast.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств региона.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства региона.\n\nТекст ошибки: " + f.Message, "Внимание",
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

    #region Класс "Тип населенного пункта"
    /// <summary>
    /// Класс "Тип населенного пункта"
    /// </summary>
    public class CLocalityPrefix : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Признак "по-умолчанию"
        /// </summary>
        private System.Boolean m_IsDefault;
        /// <summary>
        /// Признак "по-умолчанию"
        /// </summary>
        [DisplayName("По-умолчанию")]
        [Description("Признак \"По-умолчанию\"")]
        [Category("2. Дополнительно")]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public System.Boolean IsDefault
        {
            get { return m_IsDefault; }
            set { m_IsDefault = value; }
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

        public CLocalityPrefix()
            : base()
        {
            m_IsDefault = false;
            m_strShortName = "";
        }
        public CLocalityPrefix(System.Guid uuidId, System.String strName, System.String strShortName,
            System.Boolean bIsDefault)
        {
            ID = uuidId;
            Name = strName;
            m_IsDefault = bIsDefault;
            m_strShortName = strShortName;
        }

        #region Список объектов
        public static List<CLocalityPrefix> GetLocalityPrefixList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CLocalityPrefix> objList = new List<CLocalityPrefix>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetLocalityPrefix]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CLocalityPrefix((System.Guid)rs["LocalityPrefix_Guid"], (System.String)rs["LocalityPrefix_Name"],
                            (System.String)rs["LocalityPrefix_NameShort"], (System.Boolean)rs["LocalityPrefix_IsDefault"]));
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
                "Не удалось получить список типов населенных пунктов.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddLocalityPrefix]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LocalityPrefix_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LocalityPrefix_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LocalityPrefix_NameShort", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LocalityPrefix_IsDefault", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@LocalityPrefix_Name"].Value = this.Name;
                cmd.Parameters["@LocalityPrefix_NameShort"].Value = this.ShortName;
                cmd.Parameters["@LocalityPrefix_IsDefault"].Value = this.IsDefault;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@LocalityPrefix_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания типа населенного пункта.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать тип населенного пункта.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteLocalityPrefix]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LocalityPrefix_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@LocalityPrefix_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления типа населенного пункта.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить тип населенного пункта.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditLocalityPrefix]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LocalityPrefix_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LocalityPrefix_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LocalityPrefix_NameShort", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LocalityPrefix_IsDefault", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@LocalityPrefix_Guid"].Value = this.ID;
                cmd.Parameters["@LocalityPrefix_Name"].Value = this.Name;
                cmd.Parameters["@LocalityPrefix_NameShort"].Value = this.ShortName;
                cmd.Parameters["@LocalityPrefix_IsDefault"].Value = this.IsDefault;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств типа населенного пункта.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства типа населенного пункта.\n\nТекст ошибки: " + f.Message, "Внимание",
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

    #region Класс "Город"
    /// <summary>
    /// TypeConverter для списка регионов
    /// </summary>
    class RegionTypeConverter : TypeConverter
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

            CCity objCity = (CCity)context.Instance;
            System.Collections.Generic.List<CRegion> objList = objCity.GetAllRegionList();

            return new StandardValuesCollection(objList);
        }
    }

    /// <summary>
    /// TypeConverter для типа населенных пунктов
    /// </summary>
    class LocalityPrefixTypeConverter : TypeConverter
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

            CCity objCity = (CCity)context.Instance;
            System.Collections.Generic.List<CLocalityPrefix> objList = objCity.GetAllLocalityPrefixList();

            return new StandardValuesCollection(objList);
        }
    }

    /// <summary>
    /// Класс "Город"
    /// </summary>
    public class CCity : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Регион
        /// </summary>
        private CRegion m_objRegion;
        /// <summary>
        /// Регион
        /// </summary>
        [DisplayName("Регион")]
        [Description("Регион, в который входит город")]
        [Category("1. Обязательные значения")]
        [TypeConverter(typeof(RegionTypeConverter))]
        [ReadOnly(false)]
        [BrowsableAttribute(false)]
        public CRegion Region
        {
            get { return m_objRegion; }
            set { m_objRegion = value; }
        }
        /// <summary>
        /// Регион
        /// </summary>
        [DisplayName("Регион")]
        [Description("Регион, в который входит город")]
        [Category("1. Обязательные значения")]
        [TypeConverter(typeof(RegionTypeConverter))]
        public System.String RegionName
        {
            get { return m_objRegion.Name; }
            set { SetRegionValue(value); }
        }
        private void SetRegionValue(System.String strRegionName)
        {
            try
            {
                if (m_objAllRegionList == null) { m_objRegion = null; }
                else
                {
                    foreach (CRegion objRegion in m_objAllRegionList)
                    {
                        if (objRegion.Name == strRegionName)
                        {
                            m_objRegion = objRegion;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось установить значение региона.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }
        /// <summary>
        /// Тип населенного пункта
        /// </summary>
        private CLocalityPrefix m_objLocalityPrefix;
        /// <summary>
        /// Тип населенного пункта
        /// </summary>
        [DisplayName("Тип населенного пункта")]
        [Description("Тип населенного пункта")]
        [Category("1. Обязательные значения")]
        [TypeConverter(typeof(LocalityPrefixTypeConverter))]
        [ReadOnly(false)]
        [BrowsableAttribute(false)]
        public CLocalityPrefix LocalityPrefix
        {
            get { return m_objLocalityPrefix; }
            set { m_objLocalityPrefix = value; }
        }
        /// <summary>
        /// Тип населенного пункта
        /// </summary>
        [DisplayName("Тип населенного пункта")]
        [Description("Тип населенного пункта")]
        [Category("1. Обязательные значения")]
        [TypeConverter(typeof(LocalityPrefixTypeConverter))]
        public System.String LocalityPrefixName
        {
            get { return m_objLocalityPrefix.Name; }
            set { SetLocalityPrefixValue(value); }
        }
        private void SetLocalityPrefixValue(System.String strLocalityPrefixName)
        {
            try
            {
                if (m_objAllLocalityPrefixList == null) { m_objLocalityPrefix = null; }
                else
                {
                    foreach (CLocalityPrefix objLocalityPrefix in m_objAllLocalityPrefixList)
                    {
                        if (objLocalityPrefix.Name == strLocalityPrefixName)
                        {
                            m_objLocalityPrefix = objLocalityPrefix;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось установить значение типа населенного пункта.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }
        private List<CRegion> m_objAllRegionList;
        private List<CLocalityPrefix> m_objAllLocalityPrefixList;
        private List<System.String> m_objStreetList;
        [BrowsableAttribute(false)]
        public List<System.String> StreetList
        {
            get { return m_objStreetList; }
        }
        private System.String m_strDeliveryDirectionName;
        [BrowsableAttribute(false)]
        public System.String DeliveryDirectionName
        {
            get { return m_strDeliveryDirectionName; }
        }

        #endregion

        public CCity()
            : base()
        {
            m_objRegion = null;
            m_objAllRegionList = null;
            m_objAllLocalityPrefixList = null;
            m_objStreetList = null;
            m_strDeliveryDirectionName = "";
        }
        public CCity(System.Guid uuidId, System.String strName, CRegion objRegion, CLocalityPrefix objLocalityPrefix)
        {
            this.ID = uuidId;
            this.Name = strName;
            this.m_objRegion = objRegion;
            this.m_objLocalityPrefix = objLocalityPrefix;
            this.m_objAllRegionList = null;
            this.m_objAllLocalityPrefixList = null;
            this.m_objStreetList = null;
            m_strDeliveryDirectionName = "";
        }

        #region Список объектов
        public List<CRegion> GetAllRegionList()
        {
            return m_objAllRegionList;
        }
        public List<CLocalityPrefix> GetAllLocalityPrefixList()
        {
            return m_objAllLocalityPrefixList;
        }
        public static List<CCity> GetCityList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, 
            System.Boolean bLoadStreetList)
        {
            List<CCity> objList = new List<CCity>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetCity]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CRegion objRegion = null;
                    CLocalityPrefix objLocalityPrefix = null;
                    CCity objCity = null;

                    while (rs.Read())
                    {
                        objRegion = new CRegion((System.Guid)rs["Region_Guid"], (System.String)rs["Region_Name"], (System.String)rs["Region_Code"],
                            new COblast((System.Guid)rs["Oblast_Guid"], (System.String)rs["Oblast_Name"], (System.String)rs["Oblast_Code"],
                                new CCountry((System.Guid)rs["Country_Guid"], (System.String)rs["Country_Name"], (System.String)rs["Country_Code"])));
                        
                        objLocalityPrefix = new CLocalityPrefix((System.Guid)rs["LocalityPrefix_Guid"], (System.String)rs["LocalityPrefix_Name"],
                            (System.String)rs["LocalityPrefix_NameShort"], (System.Boolean)rs["LocalityPrefix_IsDefault"]);

                        objCity = new CCity((System.Guid)rs["City_Guid"], (System.String)rs["City_Name"], objRegion, objLocalityPrefix);
                        if( rs["DirectionDelivery"] != System.DBNull.Value )
                        {
                            objCity.m_strDeliveryDirectionName = System.Convert.ToString(rs["DirectionDelivery"]);
                        }
                        objList.Add(objCity );
                    }
                }
                rs.Dispose();

                List<CRegion> objRegionList = CRegion.GetRegionList(objProfile, cmd);
                List<CLocalityPrefix> objLocalityPrefixList = CLocalityPrefix.GetLocalityPrefixList(objProfile, cmd);

                if (bLoadStreetList == true)
                {
                    foreach (CCity objCity2 in objList)
                    {
                        objCity2.m_objStreetList = new List<string>();
                        objCity2.LoadStreetList(objProfile, cmd);
                    }
                }

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
                // а это для выпадающих списков
                if( (objRegionList != null) && (objLocalityPrefixList != null))
                {
                    foreach (CCity objCity in objList)
                    {
                        objCity.m_objAllRegionList = objRegionList;
                        objCity.m_objAllLocalityPrefixList = objLocalityPrefixList;
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список городов.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Загружает список улиц
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        private void LoadStreetList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
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
                        return ;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetCityStreets]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@City_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@City_Guid"].Value = this.ID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    if (this.m_objStreetList == null) { this.m_objStreetList = new List<string>(); }
                    while (rs.Read())
                    {
                        this.m_objStreetList.Add((System.String)rs["Address_Name"]);
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
                "Не удалось получить список улиц.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return ;
        }
        /// <summary>
        /// Загружает в m_objAllRegionList список районов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        public void  InitRegionList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            try
            {
                this.m_objAllRegionList = CRegion.GetRegionList(objProfile, cmdSQL);
                if ((this.m_objAllRegionList != null) && (this.m_objAllRegionList.Count > 0))
                {
                    this.Region = this.m_objAllRegionList[0];
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось загрузить список районов.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return ;
        }
        public void InitLocalityPrefixList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            try
            {
                this.m_objAllLocalityPrefixList = CLocalityPrefix.GetLocalityPrefixList( objProfile, cmdSQL );
                if ((this.m_objAllLocalityPrefixList != null) && (this.m_objAllLocalityPrefixList.Count > 0))
                {
                    this.LocalityPrefix = this.m_objAllLocalityPrefixList[ 0 ];
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось загрузить список типов населенных пунктов.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                if (this.m_objRegion == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать регион!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.m_objLocalityPrefix == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать тип населенного пункта!", "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddCity]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@City_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@City_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Region_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LocalityPrefix_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@City_Name"].Value = this.Name;
                cmd.Parameters["@Region_Guid"].Value = this.m_objRegion.ID;
                cmd.Parameters["@LocalityPrefix_Guid"].Value = this.m_objLocalityPrefix.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@City_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания города.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать город.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteCity]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@City_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@City_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления города.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить город.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditCity]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@City_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Region_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@LocalityPrefix_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@City_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@City_Guid"].Value = this.ID;
                cmd.Parameters["@City_Name"].Value = this.Name;
                cmd.Parameters["@Region_Guid"].Value = this.m_objRegion.ID;
                cmd.Parameters["@LocalityPrefix_Guid"].Value = this.m_objLocalityPrefix.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств города.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства города.\n\nТекст ошибки: " + f.Message, "Внимание",
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

    #region Класс "Тип адреса"
    /// <summary>
    /// Класс "Тип адреса"
    /// </summary>
    public class CAddressType : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Признак "по-умолчанию"
        /// </summary>
        private System.Boolean m_IsDefault;
        /// <summary>
        /// Признак "по-умолчанию"
        /// </summary>
        [DisplayName("По-умолчанию")]
        [Description("Признак \"По-умолчанию\"")]
        [Category("2. Дополнительно")]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public System.Boolean IsDefault
        {
            get { return m_IsDefault; }
            set { m_IsDefault = value; }
        }
        /// <summary>
        /// Описание
        /// </summary>
        private System.String m_strDescription;
        /// <summary>
        /// Описание
        /// </summary>
        [DisplayName("Примечание")]
        [Description("Дополнительная информация")]
        [Category("2. Дополнительно")]
        public System.String Description
        {
            get { return m_strDescription; }
            set { m_strDescription = value; }
        }
        #endregion

        public CAddressType()
            : base()
        {
            m_IsDefault = false;
            m_strDescription = "";
        }
        public CAddressType(System.Guid uuidId, System.String strName, System.String strDescription, System.Boolean bIsDefault)
        {
            this.ID = uuidId;
            this.Name = strName;
            this.m_IsDefault = bIsDefault;
            this.Description = strDescription;
        }

        #region Список объектов
        public static List<CAddressType> GetAddressTypeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CAddressType> objList = new List<CAddressType>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetAddressType]", objProfile.GetOptionsDllDBName());
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
                        strDscrpn = (rs["AddressType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["AddressType_Description"];
                        objList.Add(new CAddressType((System.Guid)rs["AddressType_Guid"],
                            (System.String)rs["AddressType_Name"], strDscrpn, (System.Boolean)rs["AddressType_IsDefault"]));
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
                "Не удалось получить список типов адресов.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddAddressType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AddressType_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AddressType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@AddressType_Name"].Value = this.Name;
                if (this.Description != "")
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AddressType_Description", System.Data.DbType.String));
                    cmd.Parameters["@AddressType_Description"].Value = this.Description;
                }
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@AddressType_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания типа адреса.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать тип адреса.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteAdressType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AddressType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@AddressType_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления типа адреса.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить тип адреса.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditAddressType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AddressType_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AddressType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@AddressType_Guid"].Value = this.ID;
                cmd.Parameters["@AddressType_Name"].Value = this.Name;
                if (this.Description != "")
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AddressType_Description", System.Data.DbType.String));
                    cmd.Parameters["@AddressType_Description"].Value = this.Description;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения типа адреса.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства типа адреса.\n\nТекст ошибки: " + f.Message, "Внимание",
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

    #region Класс "Префикс Адреса"
    public class CAddressPrefix : CBusinessObject
    {
        #region Свойства
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
        /// <summary>
        /// Признак "по-умолчанию"
        /// </summary>
        private System.Boolean m_IsDefault;
        /// <summary>
        /// Признак "по-умолчанию"
        /// </summary>
        [DisplayName("По-умолчанию")]
        [Description("Признак \"По-умолчанию\"")]
        [Category("2. Дополнительно")]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public System.Boolean IsDefault
        {
            get { return m_IsDefault; }
            set { m_IsDefault = value; }
        }
        #endregion

        public CAddressPrefix()
            : base()
        {
            m_IsDefault = false;
            m_strShortName = "";
        }
        public CAddressPrefix(System.Guid uuidId, System.String strName, System.String strShortName, System.Boolean bIsDefault)
        {
            ID = uuidId;
            Name = strName;
            m_strShortName = strShortName;
            m_IsDefault = bIsDefault;
        }

        #region Список объектов
        public static List<CAddressPrefix> GetAddressPrefixList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CAddressPrefix> objList = new List<CAddressPrefix>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetAddressPrefix]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CAddressPrefix((System.Guid)rs["AddressPrefix_Guid"],
                            (System.String)rs["AddressPrefix_Name"], (System.String)rs["AddressPrefix_NameShort"], 
                            (System.Boolean)rs["AddressPrefix_IsDefault"]));
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
                "Не удалось получить список префиксов адреса.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddAddressPrefix]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AddressPrefix_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AddressPrefix_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AddressPrefix_NameShort", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@AddressPrefix_Name"].Value = this.Name;
                cmd.Parameters["@AddressPrefix_NameShort"].Value = this.ShortName;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@AddressPrefix_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания префикса адреса.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать префикс адреса.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteAddressPrefix]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AddressPrefix_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@AddressPrefix_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления префикса адреса.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить префикс адреса.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditAddressPrefix]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AddressPrefix_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AddressPrefix_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AddressPrefix_NameShort", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@AddressPrefix_Guid"].Value = this.ID;
                cmd.Parameters["@AddressPrefix_Name"].Value = this.Name;
                cmd.Parameters["@AddressPrefix_NameShort"].Value = this.ShortName;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения префикса адреса.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства префикса адреса.\n\nТекст ошибки: " + f.Message, "Внимание",
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

    #region Класс "Строение"
    /// <summary>
    /// Класс "Строение"
    /// </summary>
    public class CBuilding : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Признак "по-умолчанию"
        /// </summary>
        private System.Boolean m_IsDefault;
        /// <summary>
        /// Признак "по-умолчанию"
        /// </summary>
        [DisplayName("По-умолчанию")]
        [Description("Признак \"По-умолчанию\"")]
        [Category("2. Дополнительно")]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public System.Boolean IsDefault
        {
            get { return m_IsDefault; }
            set { m_IsDefault = value; }
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

        public CBuilding()
            : base()
        {
            m_IsDefault = false;
            m_strShortName = "";
        }
        public CBuilding(System.Guid uuidId, System.String strName, System.String strShortName,
            System.Boolean bIsDefault)
        {
            ID = uuidId;
            Name = strName;
            m_IsDefault = bIsDefault;
            m_strShortName = strShortName;
        }

        #region Список объектов
        public static List<CBuilding> GetBuildingList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CBuilding> objList = new List<CBuilding>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetBuilding]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CBuilding((System.Guid)rs["Building_Guid"], (System.String)rs["Building_Name"],
                            (System.String)rs["Building_NameShort"], (System.Boolean)rs["Building_IsDefault"]));
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
                "Не удалось получить список строений.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddBuilding]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Building_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Building_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Building_NameShort", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Building_IsDefault", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Building_Name"].Value = this.Name;
                cmd.Parameters["@Building_NameShort"].Value = this.ShortName;
                cmd.Parameters["@Building_IsDefault"].Value = this.IsDefault;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@Building_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания строения.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать строение.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteBuilding]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Building_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Building_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления строения.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить сроение.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditBuilding]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Building_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Building_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Building_NameShort", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Building_IsDefault", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Building_Guid"].Value = this.ID;
                cmd.Parameters["@Building_Name"].Value = this.Name;
                cmd.Parameters["@Building_NameShort"].Value = this.ShortName;
                cmd.Parameters["@Building_IsDefault"].Value = this.IsDefault;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения строения.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства строения.\n\nТекст ошибки: " + f.Message, "Внимание",
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

    #region Класс "ПодСтроение"
    /// <summary>
    /// Класс "ПодСтроение"
    /// </summary>
    public class CSubBuilding : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Признак "по-умолчанию"
        /// </summary>
        private System.Boolean m_IsDefault;
        /// <summary>
        /// Признак "по-умолчанию"
        /// </summary>
        [DisplayName("По-умолчанию")]
        [Description("Признак \"По-умолчанию\"")]
        [Category("2. Дополнительно")]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public System.Boolean IsDefault
        {
            get { return m_IsDefault; }
            set { m_IsDefault = value; }
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

        public CSubBuilding()
            : base()
        {
            m_IsDefault = false;
            m_strShortName = "";
        }
        public CSubBuilding(System.Guid uuidId, System.String strName, System.String strShortName,
            System.Boolean bIsDefault)
        {
            ID = uuidId;
            Name = strName;
            m_IsDefault = bIsDefault;
            m_strShortName = strShortName;
        }

        #region Список объектов
        public static List<CSubBuilding> GetSubBuildingList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CSubBuilding> objList = new List<CSubBuilding>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetSubBuilding]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CSubBuilding((System.Guid)rs["SubBuilding_Guid"], (System.String)rs["SubBuilding_Name"],
                            (System.String)rs["SubBuilding_NameShort"], (System.Boolean)rs["SubBuilding_IsDefault"]));
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
                "Не удалось получить список подстроений.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddSubBuilding]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SubBuilding_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SubBuilding_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SubBuilding_NameShort", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SubBuilding_IsDefault", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@SubBuilding_Name"].Value = this.Name;
                cmd.Parameters["@SubBuilding_NameShort"].Value = this.ShortName;
                cmd.Parameters["@SubBuilding_IsDefault"].Value = this.IsDefault;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@SubBuilding_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания подстроения.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать подстроение.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteSubBuilding]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SubBuilding_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@SubBuilding_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления подстроения.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить подстроение.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditSubBuilding]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SubBuilding_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SubBuilding_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SubBuilding_NameShort", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SubBuilding_IsDefault", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@SubBuilding_Guid"].Value = this.ID;
                cmd.Parameters["@SubBuilding_Name"].Value = this.Name;
                cmd.Parameters["@SubBuilding_NameShort"].Value = this.ShortName;
                cmd.Parameters["@SubBuilding_IsDefault"].Value = this.IsDefault;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения подстроения.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства подстроения.\n\nТекст ошибки: " + f.Message, "Внимание",
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

    #region Класс "Помещение"
    /// <summary>
    /// Класс "Тип Помещение"
    /// </summary>
    public class CFlat : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Признак "по-умолчанию"
        /// </summary>
        private System.Boolean m_IsDefault;
        /// <summary>
        /// Признак "по-умолчанию"
        /// </summary>
        [DisplayName("По-умолчанию")]
        [Description("Признак \"По-умолчанию\"")]
        [Category("2. Дополнительно")]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public System.Boolean IsDefault
        {
            get { return m_IsDefault; }
            set { m_IsDefault = value; }
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

        public CFlat()
            : base()
        {
            m_IsDefault = false;
            m_strShortName = "";
        }
        public CFlat(System.Guid uuidId, System.String strName, System.String strShortName,
            System.Boolean bIsDefault)
        {
            ID = uuidId;
            Name = strName;
            m_IsDefault = bIsDefault;
            m_strShortName = strShortName;
        }

        #region Список объектов
        public static List<CFlat> GetFlatList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CFlat> objList = new List<CFlat>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetFlat]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CFlat((System.Guid)rs["Flat_Guid"], (System.String)rs["Flat_Name"],
                            (System.String)rs["Flat_NameShort"], (System.Boolean)rs["Flat_IsDefault"]));
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
                "Не удалось получить список помещений.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddFlat]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Flat_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Flat_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Flat_NameShort", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Flat_IsDefault", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Flat_Name"].Value = this.Name;
                cmd.Parameters["@Flat_NameShort"].Value = this.ShortName;
                cmd.Parameters["@Flat_IsDefault"].Value = this.IsDefault;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@Flat_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания помещения.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать помещение.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteFlat]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Flat_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Flat_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления помещения.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить помещение.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditFlat]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Flat_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Flat_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Flat_NameShort", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Flat_IsDefault", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Flat_Guid"].Value = this.ID;
                cmd.Parameters["@Flat_Name"].Value = this.Name;
                cmd.Parameters["@Flat_NameShort"].Value = this.ShortName;
                cmd.Parameters["@Flat_IsDefault"].Value = this.IsDefault;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения помещения.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства помещения.\n\nТекст ошибки: " + f.Message, "Внимание",
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


    /// <summary>
    /// Объекты, у которых есть списки адресов
    /// </summary>
    public enum EnumObject
    {
        Bank = 0,
        Customer = 1,
        Contact = 2,
        Rtt = 3,
        RttCustomer = 4,
        City = 5,
        Company =6,
        AgreementWithCustomer=7,
        Carrier = 8,
        Warehouse=9,
        Vendor=10
    }

    /// <summary>
    /// Класс "Адрес"
    /// </summary>
    public class CAddress
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
        /// Почтовый индекс
        /// </summary>
        private System.String m_strPostIndex;
        /// <summary>
        /// Почтовый индекс
        /// </summary>
        public System.String PostIndex
        {
            get { return m_strPostIndex; }
            set { m_strPostIndex = value; }
        }
        /// <summary>
        /// Тип адреса
        /// </summary>
        private CAddressType m_objAddressType;
        /// <summary>
        /// Тип адреса
        /// </summary>
        public CAddressType AddressType
        {
            get { return m_objAddressType; }
            set { m_objAddressType = value; }
        }
        /// <summary>
        /// Город
        /// </summary>
        private CCity m_objCity;
        /// <summary>
        /// Город
        /// </summary>
        public CCity City
        {
            get { return m_objCity; }
            set { m_objCity = value; }
        }
        /// <summary>
        /// Тип улицы
        /// </summary>
        private CAddressPrefix m_objAddressPrefix;
        /// <summary>
        /// Тип улицы
        /// </summary>
        public CAddressPrefix AddressPrefix
        {
            get { return m_objAddressPrefix; }
            set { m_objAddressPrefix = value; }
        }
        /// <summary>
        /// название улицы
        /// </summary>
        private System.String m_strName;
        /// <summary>
        /// название улицы
        /// </summary>
        public System.String Name
        {
            get { return m_strName; }
            set { m_strName = value; }
        }
        /// <summary>
        /// тип дома
        /// </summary>
        private CBuilding m_objBuilding;
        /// <summary>
        /// тип дома
        /// </summary>
        public CBuilding Building
        {
            get { return m_objBuilding; }
            set { m_objBuilding = value; }
        }
        /// <summary>
        /// номер дома
        /// </summary>
        private System.String m_strBuildCode;
        /// <summary>
        /// номер дома
        /// </summary>
        public System.String BuildCode
        {
            get { return m_strBuildCode; }
            set { m_strBuildCode = value; }
        }
        /// <summary>
        /// корпус дома
        /// </summary>
        private CSubBuilding m_objSubBuilding;
        /// <summary>
        /// корпус дома
        /// </summary>
        public CSubBuilding SubBuilding
        {
            get { return m_objSubBuilding; }
            set { m_objSubBuilding = value; }
        }
        /// <summary>
        /// номер корпуса дома
        /// </summary>
        private System.String m_strSubBuildCode;
        /// <summary>
        /// номер корпуса дома
        /// </summary>
        public System.String SubBuildCode
        {
            get { return m_strSubBuildCode; }
            set { m_strSubBuildCode = value; }
        }
        /// <summary>
        /// Квартира
        /// </summary>
        private CFlat m_objFlat;
        /// <summary>
        /// Квартира
        /// </summary>
        public CFlat Flat
        {
            get { return m_objFlat; }
            set { m_objFlat = value; }
        }
        /// <summary>
        /// номер квартиры
        /// </summary>
        private System.String m_strFlatCode;
        /// <summary>
        /// номер квартиры
        /// </summary>
        public System.String FlatCode
        {
            get { return m_strFlatCode; }
            set { m_strFlatCode = value; }
        }
        /// <summary>
        /// представление адреса для карточки
        /// </summary>
        public System.String FullName
        {
            get { return GetFullName(); }
        }
        /// <summary>
        /// Возвращает представление адреса для карточки
        /// </summary>
        /// <returns>представление адреса для карточки</returns>
        private System.String GetFullName()
        {
            System.String strRet = "";
            try
            {
                System.String strCity = ((this.City == null) ? "" : (this.City.LocalityPrefix.ShortName + " " + this.City.Name));
                System.String strStreet = ((this.AddressPrefix == null) ? "" : (this.AddressPrefix.ShortName + " " + this.Name));
                System.String strBuilding = ((this.Building == null) ? "" : (this.Building.ShortName + " " + this.BuildCode));
                strRet += strCity + " " + strStreet + " " + strBuilding;
                if (m_objSubBuilding != null) { strRet += (" " + m_objSubBuilding.ShortName + " " + m_strSubBuildCode); }
                if (m_objFlat != null) { strRet += (" " + m_objFlat.ShortName + " " + m_strFlatCode); }
                if (m_strDescription != "") { strRet += (" " + this.m_strDescription); }
            }
            catch
            {
                strRet = "";
            }
            finally
            {
            }
            return strRet;
        }
        /// <summary>
        /// Визитная карточка
        /// </summary>
        public System.String VisitingCard
        {
            get
            {
                return 
                    ( ( m_objAddressType == null ) ? "" : m_objAddressType.Name ) + "\n" +
                    ((m_objCity == null) ? "" : ("обл. " + m_objCity.Region.Oblast.Name + ", р-н " + m_objCity.Region.Name)) + " " +
                     m_strPostIndex + " " + 
                    ( ( m_objCity == null ) ? "" : m_objCity.Name ) + 
                    ( ( m_objAddressPrefix == null ) ? "" : ( " " + m_objAddressPrefix.ShortName + " " + m_strName ) ) +
                    ( ( m_objBuilding == null ) ? "" : ( ", " + m_objBuilding.ShortName + " " + m_strBuildCode ) + 
                    ( ( m_objSubBuilding == null ) ? "" : ( " " + m_objSubBuilding.ShortName + " " + m_strSubBuildCode ) ) + 
                    ( ( m_objFlat == null ) ? "" : ( " " + m_objFlat.ShortName + " " + m_strFlatCode ) )
                    ) +
                    ((m_strDescription == "") ? "" : (" " + m_strDescription));
            }
        }
        /// <summary>
        /// Визитная карточка
        /// </summary>
        public System.String VisitingCard2
        {
            get
            {
                return
                    ((m_objAddressType == null) ? "" : m_objAddressType.Name) + ": " +
                    ((m_objCity == null) ? "" : ("обл. " + m_objCity.Region.Oblast.Name + ", р-н " + m_objCity.Region.Name)) + " " +
                     m_strPostIndex + " " +
                    ( ( m_objCity == null ) ? "" : m_objCity.Name ) +
                    ((m_objAddressPrefix == null) ? "" : (" " + m_objAddressPrefix.ShortName + " " + m_strName)) +
                    ( ( m_objBuilding == null ) ? "" : ( ", " + m_objBuilding.ShortName + " " + m_strBuildCode ) + 
                    ( ( m_objSubBuilding == null ) ? "" : ( " " + m_objSubBuilding.ShortName + " " + m_strSubBuildCode ) ) +
                    ((m_objFlat == null) ? "" : (" " + m_objFlat.ShortName + " " + m_strFlatCode))
                    ) +
                    ((m_strDescription == "") ? "" : (" " + m_strDescription));
            }
        }
        /// <summary>
        /// Признак того, что объект новый и в БД о нем информации нет
        /// </summary>
        private System.Boolean m_bNewObject;
        /// <summary>
        /// Признак того, что объект новый и в БД о нем информации нет
        /// </summary>
        public System.Boolean IsNewObject
        {
            get { return m_bNewObject; }
            set 
            { 
                m_bNewObject = value;
                if (m_bNewObject == true)
                {
                    if (ID.CompareTo(System.Guid.Empty) != 0) { ID = System.Guid.Empty; }
                }
            }
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
        private System.Boolean m_bChecked;
        public System.Boolean Checked
        {
            get { return m_bChecked; }
            set { m_bChecked = value; }
        }
        #endregion

        #region Конструктор
        public CAddress()
        {
            m_uuidID = System.Guid.Empty;
            m_objAddressPrefix = null;
            m_objAddressType = null;
            m_objBuilding = null;
            m_objCity = null;
            m_objFlat = null;
            m_objSubBuilding = null;
            m_strBuildCode = "";
            m_strFlatCode = "";
            m_strName = "";
            m_strPostIndex = "";
            m_strSubBuildCode = "";
            m_bNewObject = false;
            m_strDescription = "";
            m_bChecked = false;
        }
        public CAddress(System.Guid uuidID, CAddressType objAddressType, CCity objCity, System.String strPostIndex,
            CAddressPrefix objAddressPrefix,  System.String strName,
            CBuilding objBuilding, System.String strBuildCode, 
            CSubBuilding objSubBuilding, System.String strSubBuildCode,
            System.String strFlatCode, CFlat objFlat, System.String strDescription)
        {
            m_uuidID = uuidID;
            m_objAddressPrefix = objAddressPrefix;
            m_objAddressType = objAddressType;
            m_objBuilding = objBuilding;
            m_objCity = objCity;
            m_objFlat = objFlat;
            m_objSubBuilding = objSubBuilding;
            m_strBuildCode = strBuildCode;
            m_strFlatCode = strFlatCode;
            m_strName = strName;
            m_strPostIndex = strPostIndex;
            m_strSubBuildCode = strSubBuildCode;
            m_bNewObject = false;
            m_strDescription = strDescription;
            m_bChecked = false;
        }
        #endregion

        #region Список адресов
        /// <summary>
        /// Возвращает список адресов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="enObjectWithAddress">ссылка на тип владельца адресов</param>
        /// <param name="uuidObjectId">уникальный идентификатор владельца адресов</param>
        /// <returns>список адресов</returns>
        public static List<CAddress> GetAddressList(UniXP.Common.CProfile objProfile, 
            System.Data.SqlClient.SqlCommand cmdSQL,
            EnumObject enObjectWithAddress, System.Guid uuidObjectId)
        {
            List<CAddress> objList = new List<CAddress>();
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
                switch (enObjectWithAddress)
                {
                    case EnumObject.Bank:
                        {
                            cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetAddressForBank]", objProfile.GetOptionsDllDBName());
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Bank_Guid", System.Data.SqlDbType.UniqueIdentifier));
                            cmd.Parameters["@Bank_Guid"].Value = uuidObjectId;
                            break;
                        }
                    case EnumObject.Contact:
                        {
                            cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetAddressForContact]", objProfile.GetOptionsDllDBName());
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Contact_Guid", System.Data.SqlDbType.UniqueIdentifier));
                            cmd.Parameters["@Contact_Guid"].Value = uuidObjectId;
                            break;
                        }
                    case EnumObject.Customer:
                        {
                            cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetAddressForCustomer]", objProfile.GetOptionsDllDBName());
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                            cmd.Parameters["@Customer_Guid"].Value = uuidObjectId;
                            break;
                        }
                    case EnumObject.Rtt:
                        {
                            cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetAddressForRtt]", objProfile.GetOptionsDllDBName());
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Rtt_Guid", System.Data.SqlDbType.UniqueIdentifier));
                            cmd.Parameters["@Rtt_Guid"].Value = uuidObjectId;
                            break;
                        }
                    case EnumObject.RttCustomer:
                        {
                            cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetAddressForCustomerAll]", objProfile.GetOptionsDllDBName());
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Rtt_Guid", System.Data.SqlDbType.UniqueIdentifier));
                            cmd.Parameters["@Rtt_Guid"].Value = uuidObjectId;
                            break;
                        }
                    case EnumObject.City:
                        {
                            cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetAddressForCity]", objProfile.GetOptionsDllDBName());
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@City_Guid", System.Data.SqlDbType.UniqueIdentifier));
                            cmd.Parameters["@City_Guid"].Value = uuidObjectId;
                            break;
                        }
                    case EnumObject.Company :
                        {
                            cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetAddressForCompany]", objProfile.GetOptionsDllDBName());
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                            cmd.Parameters["@Company_Guid"].Value = uuidObjectId;
                            break;
                        }
                    case EnumObject.Carrier:
                        {
                            cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetAddressForCarrier]", objProfile.GetOptionsDllDBName());
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Carrier_Guid", System.Data.SqlDbType.UniqueIdentifier));
                            cmd.Parameters["@Carrier_Guid"].Value = uuidObjectId;
                            break;
                        }
                    case EnumObject.Warehouse:
                        {
                            cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetAddressForWarehouse]", objProfile.GetOptionsDllDBName());
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Warehouse_Guid", System.Data.SqlDbType.UniqueIdentifier));
                            cmd.Parameters["@Warehouse_Guid"].Value = uuidObjectId;
                            break;
                        }
                    case EnumObject.Vendor:
                        {
                            cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetAddressForVendor]", objProfile.GetOptionsDllDBName());
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Guid", System.Data.SqlDbType.UniqueIdentifier));
                            cmd.Parameters["@Vendor_Guid"].Value = uuidObjectId;
                            break;
                        }

                    default:
                        break;
                }
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CAddressType objAddressType = null; 
                    CCity objCity = null;
                    System.String strPostIndex = "";
                    CAddressPrefix objAddressPrefix = null;  
                    System.String strName = "";
                    System.String strDescription = "";
                    CBuilding objBuilding = null; 
                    System.String strBuildCode = "";
                    CSubBuilding objSubBuilding = null; 
                    System.String strSubBuildCode = "";
                    System.String strFlatCode = "";
                    CFlat objFlat = null;
                    CRegion objRegion = null;
                    CLocalityPrefix objLocalityPrefix = null;

                    System.String strAddressTypeDscrpn = "";
                    while (rs.Read())
                    {
                        strAddressTypeDscrpn = (rs["AddressType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["AddressType_Description"];
                        objAddressType = new CAddressType((System.Guid)rs["AddressType_Guid"], (System.String)rs["AddressType_Name"], strAddressTypeDscrpn, (System.Boolean)rs["AddressType_IsDefault"]);

                        objAddressPrefix = null;
                        if (rs["AddressPrefix_Guid"] != System.DBNull.Value)
                        {
                            objAddressPrefix = new CAddressPrefix((System.Guid)rs["AddressPrefix_Guid"],
                                (System.String)rs["AddressPrefix_Name"], (System.String)rs["AddressPrefix_NameShort"], (System.Boolean)rs["AddressPrefix_IsDefault"]);
                        }
                        objBuilding = null;
                        if (rs["Building_Guid"] != System.DBNull.Value)
                        {
                            objBuilding = new CBuilding((System.Guid)rs["Building_Guid"], (System.String)rs["BuildingNameShort"],
                                (System.String)rs["BuildingNameShort"], false);
                        }
                        objSubBuilding = null;
                        if (rs["SubBuilding_Guid"] != System.DBNull.Value)
                        {
                            objSubBuilding = new CSubBuilding((System.Guid)rs["SubBuilding_Guid"], (System.String)rs["SubBuildingNameShort"],
                                (System.String)rs["SubBuildingNameShort"], false);
                        }
                        objFlat = null;
                        if (rs["Flat_Guid"] != System.DBNull.Value)
                        {
                            objFlat = new CFlat((System.Guid)rs["Flat_Guid"], (System.String)rs["FlatNameShort"],
                                (System.String)rs["FlatNameShort"], false);
                        }

                        objRegion = new CRegion((System.Guid)rs["Region_Guid"], (System.String)rs["Region_Name"], (System.String)rs["Region_Code"],
                            new COblast((System.Guid)rs["Oblast_Guid"], (System.String)rs["Oblast_Name"], (System.String)rs["Oblast_Code"],
                                new CCountry((System.Guid)rs["Country_Guid"], (System.String)rs["Country_Name"], (System.String)rs["Country_Code"])));

                        objLocalityPrefix = new CLocalityPrefix((System.Guid)rs["LocalityPrefix_Guid"], (System.String)rs["LocalityPrefix_Name"],
                            (System.String)rs["LocalityPrefix_NameShort"], (System.Boolean)rs["LocalityPrefix_IsDefault"]);

                        objCity = new CCity((System.Guid)rs["City_Guid"], (System.String)rs["City_Name"], objRegion, objLocalityPrefix);

                        strPostIndex = (System.String)rs["Address_Postindex"];
                        strName = ( rs["Address_Name"] == System.DBNull.Value ) ? "" : (System.String)rs["Address_Name"];
                        strBuildCode = (rs["Address_BuildCode"] == System.DBNull.Value) ? "" : (System.String)rs["Address_BuildCode"];
                        strSubBuildCode = (rs["Address_SubBuildingCode"] == System.DBNull.Value) ? "" : (System.String)rs["Address_SubBuildingCode"];
                        strFlatCode = (rs["Address_FlatCode"] == System.DBNull.Value) ? "" : (System.String)rs["Address_FlatCode"];
                        strDescription = (rs["Address_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Address_Description"];
                        objList.Add(new CAddress((System.Guid)rs["Address_Guid"], objAddressType, objCity, strPostIndex, 
                            objAddressPrefix, strName, objBuilding, strBuildCode, objSubBuilding, strSubBuildCode,
                            strFlatCode, objFlat, strDescription));
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
                "Не удалось получить список адресов.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }

        /// <summary>
        /// Обновляет информацию для адреса
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objAddress">объект класса "Адрес"</param>
        /// <param name="uuidObjectId">уникальный идентификатор адреса</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean Init(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, CAddress objAddress, System.Guid uuidObjectId)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetAddress]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Address_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters["@Address_Guid"].Value = uuidObjectId;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CAddressType objAddressType = null;
                    CCity objCity = null;
                    System.String strPostIndex = "";
                    CAddressPrefix objAddressPrefix = null;
                    System.String strName = "";
                    System.String strDescription = "";
                    CBuilding objBuilding = null;
                    System.String strBuildCode = "";
                    CSubBuilding objSubBuilding = null;
                    System.String strSubBuildCode = "";
                    System.String strFlatCode = "";
                    CFlat objFlat = null;
                    CRegion objRegion = null;
                    CLocalityPrefix objLocalityPrefix = null;

                    System.String strAddressTypeDscrpn = "";
                    while (rs.Read())
                    {
                        strAddressTypeDscrpn = (rs["AddressType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["AddressType_Description"];
                        objAddressType = new CAddressType((System.Guid)rs["AddressType_Guid"], (System.String)rs["AddressType_Name"], strAddressTypeDscrpn, (System.Boolean)rs["AddressType_IsDefault"]);

                        objAddressPrefix = null;
                        if (rs["AddressPrefix_Guid"] != System.DBNull.Value)
                        {
                            objAddressPrefix = new CAddressPrefix((System.Guid)rs["AddressPrefix_Guid"],
                                (System.String)rs["AddressPrefix_Name"], (System.String)rs["AddressPrefix_NameShort"], (System.Boolean)rs["AddressPrefix_IsDefault"]);
                        }
                        objBuilding = null;
                        if (rs["Building_Guid"] != System.DBNull.Value)
                        {
                            objBuilding = new CBuilding((System.Guid)rs["Building_Guid"], (System.String)rs["BuildingNameShort"],
                                (System.String)rs["BuildingNameShort"], false);
                        }
                        objSubBuilding = null;
                        if (rs["SubBuilding_Guid"] != System.DBNull.Value)
                        {
                            objSubBuilding = new CSubBuilding((System.Guid)rs["SubBuilding_Guid"], (System.String)rs["SubBuildingNameShort"],
                                (System.String)rs["SubBuildingNameShort"], false);
                        }
                        objFlat = null;
                        if (rs["Flat_Guid"] != System.DBNull.Value)
                        {
                            objFlat = new CFlat((System.Guid)rs["Flat_Guid"], (System.String)rs["FlatNameShort"],
                                (System.String)rs["FlatNameShort"], false);
                        }

                        objRegion = new CRegion((System.Guid)rs["Region_Guid"], (System.String)rs["Region_Name"], (System.String)rs["Region_Code"],
                            new COblast((System.Guid)rs["Oblast_Guid"], (System.String)rs["Oblast_Name"], (System.String)rs["Oblast_Code"],
                                new CCountry((System.Guid)rs["Country_Guid"], (System.String)rs["Country_Name"], (System.String)rs["Country_Code"])));

                        objLocalityPrefix = new CLocalityPrefix((System.Guid)rs["LocalityPrefix_Guid"], (System.String)rs["LocalityPrefix_Name"],
                            (System.String)rs["LocalityPrefix_NameShort"], (System.Boolean)rs["LocalityPrefix_IsDefault"]);

                        objCity = new CCity((System.Guid)rs["City_Guid"], (System.String)rs["City_Name"], objRegion, objLocalityPrefix);

                        strPostIndex = (System.String)rs["Address_Postindex"];
                        strName = (rs["Address_Name"] == System.DBNull.Value) ? "" : (System.String)rs["Address_Name"];
                        strBuildCode = (rs["Address_BuildCode"] == System.DBNull.Value) ? "" : (System.String)rs["Address_BuildCode"];
                        strSubBuildCode = (rs["Address_SubBuildingCode"] == System.DBNull.Value) ? "" : (System.String)rs["Address_SubBuildingCode"];
                        strFlatCode = (rs["Address_FlatCode"] == System.DBNull.Value) ? "" : (System.String)rs["Address_FlatCode"];
                        strDescription = (rs["Address_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Address_Description"];

                        objAddress.ID = (System.Guid)rs["Address_Guid"];
                        objAddress.AddressType = objAddressType;
                        objAddress.City = objCity;
                        objAddress.PostIndex = strPostIndex;
                        objAddress.AddressPrefix = objAddressPrefix;
                        objAddress.Name = strName;
                        objAddress.Building = objBuilding;
                        objAddress.BuildCode = strBuildCode;
                        objAddress.SubBuildCode = strSubBuildCode;
                        objAddress.FlatCode = strFlatCode;
                        objAddress.Flat = objFlat;
                        objAddress.Description = strDescription;

                        bRet = true;

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
                "Не удалось получить данные адреса.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return bRet;
        }
        #endregion

        #region Сохранить в БД список адресов
        /// <summary>
        /// Проверка свойств адреса перед сохранением
        /// </summary>
        /// <param name="strErr">текст с ошибкой</param>
        /// <returns>true - все свойства корректны; false - ошибка</returns>
        public System.Boolean IsAllParametersValid( ref System.String strErr )
        {
            System.Boolean bRet = false;
            try
            {
                if (this.AddressType == null)
                {
                    strErr = "Адрес: Необходимо указать тип адреса!";
                    return bRet;
                }
                if (this.City == null)
                {
                    strErr = "Адрес: Необходимо указать населенный пункт!";
                    return bRet;
                }
                if (this.PostIndex == "")
                {
                    strErr = "Адрес: Необходимо указать почтовый индекс!";
                    return bRet;
                }
                if (this.AddressPrefix == null)
                {
                    this.Name = "";
                    this.Building = null;
                    this.BuildCode = "";
                    this.SubBuilding = null;
                    this.SubBuildCode = "";
                    this.Flat = null;
                    this.FlatCode = "";
                }
                if (this.Name == "")
                {
                    this.AddressPrefix = null;
                    this.Building = null;
                    this.BuildCode = "";
                    this.SubBuilding = null;
                    this.SubBuildCode = "";
                    this.Flat = null;
                    this.FlatCode = "";
                }
                else
                {
                    if (this.AddressPrefix == null)
                    {
                        strErr = "Адрес: Необходимо указать тип улицы!";
                        return bRet;
                    }
                }

                if (this.BuildCode == "")
                {
                    this.Building = null;
                }
                else
                {
                    if (this.Building == null)
                    {
                        strErr = "Адрес: Необходимо указать тип дома!";
                        return bRet;
                    }
                }
                
                if (this.SubBuildCode == "")
                {
                    this.SubBuilding = null;
                }
                else
                {
                    if (this.SubBuilding == null)
                    {
                        strErr = "Адрес: Необходимо указать тип корпуса и его номер!";
                        return bRet;
                    }
                }

                if ( this.FlatCode == "")
                {
                    this.Flat = null;
                }
                else
                {
                    if (this.Flat == null)
                    {
                        strErr = "Адрес: Необходимо указать тип помещения и его номер!";
                        return bRet;
                    }
                }

                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr = "Адрес: Ошибка проверки свойств. Текст ошибки: " + f.Message;
            }
            return bRet;
        }
        /// <summary>
        /// Сохраняет в БД адреса
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="objAddressList">список адресов</param>
        /// <param name="enObjectWithAddress">тип владельца адресов</param>
        /// <param name="uuidObjectId">идентификатор владельца адресов</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean SaveAddressList(List<CAddress> objAddressList, List<CAddress> objAddressForDeleteList,
            EnumObject enObjectWithAddress, System.Guid uuidObjectId, 
            UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            if (((objAddressList == null) || (objAddressList.Count == 0)) && ((objAddressForDeleteList == null) || (objAddressForDeleteList.Count == 0))) { return true; }
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                // для начала проверим, что нам пришло в списке
                if ((objAddressList != null) && (objAddressList.Count > 0))
                {
                    System.Boolean bIsAllValid = true;
                    foreach (CAddress objItem in objAddressList)
                    {
                        if (objItem.IsAllParametersValid( ref strErr) == false)
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
                // Проверить вот здесь, приходит ли заполненным objAddressForDeleteList
                if ((objAddressForDeleteList != null) && (objAddressForDeleteList.Count > 0))
                {
                    foreach (CAddress objAddres in objAddressForDeleteList)
                    {
                        if (objAddres.ID.CompareTo(System.Guid.Empty) == 0) { continue; }
                        iRes = (objAddres.Remove(enObjectWithAddress, uuidObjectId, objProfile, cmd, ref strErr) == true) ? 0 : 1; 
                        if (iRes != 0) { break; }
                    }
                }

                if (iRes == 0)
                {
                    // удалили адреса, теперь добавим или сохраним
                    if ((objAddressList != null) && (objAddressList.Count > 0))
                    {
                        // теперь в цикле добавим в БД каждый член из списка
                        foreach (CAddress objAddress in objAddressList)
                        {
                            if (objAddress.ID.CompareTo(System.Guid.Empty) == 0)
                            {
                                // новый адрес
                                iRes = (objAddress.Add(enObjectWithAddress, uuidObjectId, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                            }
                            else
                            {
                                iRes = (objAddress.Update(enObjectWithAddress, uuidObjectId, objProfile, cmd, ref strErr) == true) ? 0 : -1;
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

                bRet = ( iRes == 0 );
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

        #region Добавить новый адрес в базу данных
        /// <summary>
        /// Добавляет новый адрес в базу данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="enObjectWithAddress">тип владельца адресов</param>
        /// <param name="uuidObjectId">идентификатор владельца адресов</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Add(EnumObject enObjectWithAddress, System.Guid uuidObjectId, 
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

                cmd.Parameters.Clear();
                System.String strAddCmd = "";
                System.String strParamOwnerIdName = "";
                switch (enObjectWithAddress)
                {
                    case EnumObject.Bank:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[sp_AddAddressToBank]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Bank_Guid";
                            break;
                        }
                    case EnumObject.Contact:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[sp_AddAddressToContact]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Contact_Guid";
                            break;
                        }
                    case EnumObject.Customer:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[sp_AddAddressToCustomer]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Customer_Guid";
                            break;
                        }
                    case EnumObject.Rtt:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[sp_AddAddressToRtt]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Rtt_Guid";
                            break;
                        }
                    case EnumObject.Company:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[usp_AddAddressToCompany]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Company_Guid";
                            break;
                        }
                    case EnumObject.Carrier:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[usp_AddAddressToCarrier]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Carrier_Guid";
                            break;
                        }
                    case EnumObject.Warehouse:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[usp_AddAddressToWarehouse]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Warehouse_Guid";
                            break;                           
                        }
                    case EnumObject.Vendor:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[usp_AddAddressToVendor]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Vendor_Guid";
                            break;
                        }

                    default:
                        break;
                }
                cmd.CommandText = strAddCmd;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(strParamOwnerIdName, System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Address_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AddressType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@City_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AddressPrefix_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Building_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SubBuilding_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Flat_Guid", System.Data.SqlDbType.UniqueIdentifier));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Address_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Address_Postindex", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Address_BuildCode", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Address_SubBuildingCode", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Address_FlatCode", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Address_Description", System.Data.DbType.String));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters[strParamOwnerIdName].Value = uuidObjectId;

                cmd.Parameters["@AddressType_Guid"].Value = this.AddressType.ID;
                cmd.Parameters["@City_Guid"].Value = this.City.ID;
                //cmd.Parameters["@AddressPrefix_Guid"].Value = this.AddressPrefix.ID;
                //cmd.Parameters["@Address_Name"].Value = this.m_strName;
                //cmd.Parameters["@Building_Guid"].Value = this.m_objBuilding.ID;
                //cmd.Parameters["@Address_BuildCode"].Value = this.m_strBuildCode;
                cmd.Parameters["@Address_Postindex"].Value = this.m_strPostIndex;
                cmd.Parameters["@Address_Description"].Value = this.m_strDescription;

                if (this.m_objAddressPrefix != null)
                {
                    cmd.Parameters["@AddressPrefix_Guid"].IsNullable = false;
                    cmd.Parameters["@Address_Name"].IsNullable = false;
                    cmd.Parameters["@AddressPrefix_Guid"].Value = this.AddressPrefix.ID;
                    cmd.Parameters["@Address_Name"].Value = this.m_strName;
                }
                else
                {
                    cmd.Parameters["@AddressPrefix_Guid"].IsNullable = true;
                    cmd.Parameters["@Address_Name"].IsNullable = true;
                    cmd.Parameters["@AddressPrefix_Guid"].Value = null;
                    cmd.Parameters["@Address_Name"].Value = null;
                }
                if (this.m_objBuilding != null)
                {
                    cmd.Parameters["@Building_Guid"].IsNullable = false;
                    cmd.Parameters["@Address_BuildCode"].IsNullable = false;
                    cmd.Parameters["@Building_Guid"].Value = this.m_objBuilding.ID;
                    cmd.Parameters["@Address_BuildCode"].Value = this.m_strBuildCode;
                }
                else
                {
                    cmd.Parameters["@Building_Guid"].IsNullable = true;
                    cmd.Parameters["@Address_BuildCode"].IsNullable = true;
                    cmd.Parameters["@Building_Guid"].Value = null;
                    cmd.Parameters["@Address_BuildCode"].Value = null;
                }
                if (this.m_objSubBuilding != null)
                {
                    cmd.Parameters["@SubBuilding_Guid"].IsNullable = false;
                    cmd.Parameters["@Address_SubBuildingCode"].IsNullable = false;
                    cmd.Parameters["@SubBuilding_Guid"].Value = this.m_objSubBuilding.ID;
                    cmd.Parameters["@Address_SubBuildingCode"].Value = this.m_strSubBuildCode;
                }
                else
                {
                    cmd.Parameters["@SubBuilding_Guid"].IsNullable = true;
                    cmd.Parameters["@Address_SubBuildingCode"].IsNullable = true;
                    cmd.Parameters["@SubBuilding_Guid"].Value = null;
                    cmd.Parameters["@Address_SubBuildingCode"].Value = null;
                }
                if (this.m_objFlat != null)
                {
                    cmd.Parameters["@Flat_Guid"].IsNullable = false;
                    cmd.Parameters["@Address_FlatCode"].IsNullable = false;
                    cmd.Parameters["@Flat_Guid"].Value = this.m_objFlat.ID;
                    cmd.Parameters["@Address_FlatCode"].Value = this.m_strFlatCode;
                }
                else
                {
                    cmd.Parameters["@Flat_Guid"].IsNullable = true;
                    cmd.Parameters["@Address_FlatCode"].IsNullable = true;
                    cmd.Parameters["@Flat_Guid"].Value = null;
                    cmd.Parameters["@Address_FlatCode"].Value = null;
                }
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    this.m_uuidID = (System.Guid)cmd.Parameters["@Address_Guid"].Value;
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

        #region Изменить свойства адреса в базе данных
        /// <summary>
        /// Изменяет свойства адреса
        /// </summary>
        /// <param name="enObjectWithAddress">тип владельца адресов</param>
        /// <param name="uuidObjectId">идентификатор владельца адресов</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Update(EnumObject enObjectWithAddress, System.Guid uuidObjectId,
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

                // сперва удалим список адресов
                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditAddress]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Address_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AddressType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@City_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AddressPrefix_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Building_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SubBuilding_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Flat_Guid", System.Data.SqlDbType.UniqueIdentifier));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Address_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Address_Postindex", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Address_BuildCode", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Address_SubBuildingCode", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Address_FlatCode", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Address_Description", System.Data.DbType.String));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AddressOwnerType", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AddressOwnerId", System.Data.SqlDbType.UniqueIdentifier));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Address_Guid"].Value = this.ID;
                cmd.Parameters["@AddressType_Guid"].Value = this.AddressType.ID;
                cmd.Parameters["@City_Guid"].Value = this.City.ID;
                //cmd.Parameters["@AddressPrefix_Guid"].Value = this.AddressPrefix.ID;
                //cmd.Parameters["@Building_Guid"].Value = this.Building.ID;
                //cmd.Parameters["@Address_BuildCode"].Value = this.BuildCode;
                //cmd.Parameters["@Address_Name"].Value = this.Name;
                cmd.Parameters["@Address_Postindex"].Value = this.PostIndex;
                cmd.Parameters["@AddressOwnerType"].Value = System.Convert.ToInt32( enObjectWithAddress );
                cmd.Parameters["@AddressOwnerId"].Value = uuidObjectId;
                cmd.Parameters["@Address_Description"].Value = this.m_strDescription;

                if (this.m_objAddressPrefix != null)
                {
                    cmd.Parameters["@AddressPrefix_Guid"].IsNullable = false;
                    cmd.Parameters["@Address_Name"].IsNullable = false;
                    cmd.Parameters["@AddressPrefix_Guid"].Value = this.AddressPrefix.ID;
                    cmd.Parameters["@Address_Name"].Value = this.m_strName;
                }
                else
                {
                    cmd.Parameters["@AddressPrefix_Guid"].IsNullable = true;
                    cmd.Parameters["@Address_Name"].IsNullable = true;
                    cmd.Parameters["@AddressPrefix_Guid"].Value = null;
                    cmd.Parameters["@Address_Name"].Value = null;
                }
                if (this.m_objBuilding != null)
                {
                    cmd.Parameters["@Building_Guid"].IsNullable = false;
                    cmd.Parameters["@Address_BuildCode"].IsNullable = false;
                    cmd.Parameters["@Building_Guid"].Value = this.m_objBuilding.ID;
                    cmd.Parameters["@Address_BuildCode"].Value = this.m_strBuildCode;
                }
                else
                {
                    cmd.Parameters["@Building_Guid"].IsNullable = true;
                    cmd.Parameters["@Address_BuildCode"].IsNullable = true;
                    cmd.Parameters["@Building_Guid"].Value = null;
                    cmd.Parameters["@Address_BuildCode"].Value = null;
                }
                if (this.m_objSubBuilding != null)
                {
                    cmd.Parameters["@SubBuilding_Guid"].IsNullable = false;
                    cmd.Parameters["@Address_SubBuildingCode"].IsNullable = false;
                    cmd.Parameters["@SubBuilding_Guid"].Value = this.SubBuilding.ID;
                    cmd.Parameters["@Address_SubBuildingCode"].Value = this.SubBuildCode;
                }
                else
                {
                    cmd.Parameters["@SubBuilding_Guid"].IsNullable = true;
                    cmd.Parameters["@Address_SubBuildingCode"].IsNullable = true;
                    cmd.Parameters["@SubBuilding_Guid"].Value = null;
                    cmd.Parameters["@Address_SubBuildingCode"].Value = null;
                }
                if (this.m_objFlat != null)
                {
                    cmd.Parameters["@Flat_Guid"].IsNullable = false;
                    cmd.Parameters["@Address_FlatCode"].IsNullable = false;
                    cmd.Parameters["@Flat_Guid"].Value = this.Flat.ID;
                    cmd.Parameters["@Address_FlatCode"].Value = this.FlatCode;
                }
                else
                {
                    cmd.Parameters["@Flat_Guid"].IsNullable = true;
                    cmd.Parameters["@Address_FlatCode"].IsNullable = true;
                    cmd.Parameters["@Flat_Guid"].Value = null;
                    cmd.Parameters["@Address_FlatCode"].Value = null;
                }
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = "Ошибка редактирования адреса. Текст ошибки: " + (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    if (this.ID.CompareTo((System.Guid)cmd.Parameters["@Address_Guid"].Value) != 0)
                    {
                        this.ID = (System.Guid)cmd.Parameters["@Address_Guid"].Value;
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
                strErr = "Ошибка редактирования адреса. Текст ошибки: " + f.Message;
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

        #region Удалить адрес из базы данных
        /// <summary>
        /// Удаляет адрес из базы данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="enObjectWithAddress">тип владельца адресов</param>
        /// <param name="uuidObjectId">идентификатор владельца адресов</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Remove(EnumObject enObjectWithAddress, System.Guid uuidObjectId,
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

                // сперва удалим список адресов
                cmd.Parameters.Clear();
                System.String strDeleteCmd = "";
                System.String strParamOwnerIdName = "";
                switch (enObjectWithAddress)
                {
                    case EnumObject.Bank:
                        {
                            strDeleteCmd = System.String.Format("[{0}].[dbo].[sp_DeleteAddressFromBank]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Bank_Guid";
                            break;
                        }
                    case EnumObject.Customer:
                        {
                            strDeleteCmd = System.String.Format("[{0}].[dbo].[sp_DeleteAddressFromCustomer]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Customer_Guid";
                            break;
                        }
                    case EnumObject.Rtt:
                        {
                            strDeleteCmd = System.String.Format("[{0}].[dbo].[sp_DeleteAddressFromRtt]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Rtt_Guid";
                            break;
                        }
                    case EnumObject.Company:
                        {
                            strDeleteCmd = System.String.Format("[{0}].[dbo].[usp_DeleteAddressFromCompany]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Company_Guid";
                            break;
                        }
                    case EnumObject.Contact: // добавил
                        {
                            strDeleteCmd = System.String.Format("[{0}].[dbo].[usp_DeleteAddressFromContact]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@temp";
                            break;
                        }
                    case EnumObject.Carrier: 
                        {
                            strDeleteCmd = System.String.Format("[{0}].[dbo].[usp_DeleteAddressFromCarrier]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Carrier_Guid";
                            break;
                        }
                    case EnumObject.Warehouse:
                        {
                            strDeleteCmd = System.String.Format("[{0}].[dbo].[usp_DeleteAddressFromWarehouse]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Warehouse_Guid";
                            break;
                        }
                    case EnumObject.Vendor:
                        {
                            strDeleteCmd = System.String.Format("[{0}].[dbo].[usp_DeleteAddressFromVendor]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Vendor_Guid";
                            break;
                        }
                    default:
                        break;
                }
                cmd.CommandText = strDeleteCmd;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Address_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(strParamOwnerIdName, System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters[strParamOwnerIdName].Value = uuidObjectId;
                cmd.Parameters["@Address_Guid"].Value = this.ID; // adress_Guid
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

        public override string ToString()
        {
            return FullName;
        }

    }
}
