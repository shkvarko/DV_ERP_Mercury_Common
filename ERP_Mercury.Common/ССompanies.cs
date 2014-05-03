using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ERP_Mercury.Common
{

    #region Класс "Компании"
    /// <summary>
    /// Класс "Компания"
    /// </summary>
    public class CCompanies
    {
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
        /// Уникальный идентификатор в InterBase
        /// </summary>
        private System.Int32 m_IbID;
        /// <summary>
        /// Уникальный идентификатор в InterBase
        /// </summary>
        public System.Int32 InterBaseID
        {
            get { return m_IbID; }
            set { m_IbID = value; }
        }

        /// <summary>
        /// Тип компании
        /// </summary>
        private CCompanyType m_objCompanyType;
        /// <summary>
        /// Тип компании
        /// </summary>
        public CCompanyType TypeCompany
        {
            get { return m_objCompanyType; }
            set { m_objCompanyType = value; }
        }

        /// <summary>
        /// Псевдоним (acronym) компании
        /// </summary>
        private System.String m_strAcronym;
        /// <summary>
        /// Псевдоним (acronym) компании
        /// </summary>
        public System.String Acronym
        {
            get { return m_strAcronym; }
            set { m_strAcronym = value; }
        }

        /// <summary>
        /// Описание компании
        /// </summary>
        private System.String m_strDescription;
        /// <summary>
        /// Описание компании
        /// </summary>
        public System.String Description
        {
            get { return m_strDescription; }
            set { m_strDescription = value; }
        }

        /// <summary>
        ///  Название компании
        /// </summary>
        private System.String m_strName;
        /// <summary>
        /// Название компании
        /// </summary>
        public System.String Name
        {
            get { return m_strName; }
            set { m_strName = value; }
        }

        /// <summary>
        /// общестатистический код предприятий и организаций
        /// </summary>
        private System.String m_strOKPO;
        /// <summary>
        /// общестатистический код предприятий и организаций
        /// </summary>
        public System.String OKPO
        {
            get { return m_strOKPO; }
            set { m_strOKPO = value; }
        }

        /// <summary>
        /// код в соответствии с Общегосударственным Классификатором Юридических Лиц и Предпринимателей
        /// 9 символов
        /// </summary>
        private System.String m_strOKULP;
        /// <summary>
        /// код в соответствии с Общегосударственным Классификатором Юридических Лиц и Предпринимателей
        /// </summary>
        public System.String OKULP
        {
            get { return m_strOKULP; }
            set { m_strOKULP = value; }
        }

        /// <summary>
        /// Уникальный Номер Плательщика
        /// 9 символов
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
        /// Признак активности
        /// </summary>
        private System.Boolean m_IsActive;
        /// <summary>
        /// Признак активности
        /// </summary>
        //[TypeConverter(typeof(BooleanTypeConverter))]
        public System.Boolean IsActive
        {
            get { return m_IsActive; }
            set { m_IsActive = value; }
        }

        // Поля Record_Update заполняется на уровне СУБД, соответственно ему не нужно свойство 


        /// <summary>
        /// Список электронных адресов
        /// </summary>
        private List<CEMail> m_objEMailList;
        /// <summary>
        /// Список электронных адресов
        /// </summary>
        public List<CEMail> EMailList
        {
            get { return m_objEMailList; }
            set { m_objEMailList = value; }
        }

        /// <summary>
        /// Список телефонов
        /// </summary>
        private List<CPhone> m_objPhoneList;
        /// <summary>
        /// Список телефонов
        /// </summary>
        public List<CPhone> PhoneList
        {
            get { return m_objPhoneList; }
            set { m_objPhoneList = value; }
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

        /// <summary>
        /// Список лицензий
        /// </summary>
        private List<CLicence> m_objLicenceList;
        /// <summary>
        /// Список лицензий
        /// </summary>
        public List<CLicence> LicenceList
        {
            get { return m_objLicenceList; }
            set { m_objLicenceList = value; }
        }

        /// <summary>
        /// Список контактов
        /// </summary>
        private List<CContact> m_objContactList;
        /// <summary>
        /// Список контактов
        /// </summary>
        public List<CContact> ContactList
        {
            get { return m_objContactList; }
            set { m_objContactList = value; }
        }


        #region Конструктор
        public CCompanies()
        {
            m_uuidID = System.Guid.Empty;
            m_IbID = 0;
            m_objCompanyType = null;
            m_strAcronym = "";
            m_strDescription = "";
            m_strName = "";
            m_strOKPO = "";
            m_strOKULP = "";
            m_strUNP = "";
            m_IsActive = false;
            m_objEMailList = null;
            m_objPhoneList = null;
            m_objAddressList = null;
            m_objAccountList = null;
            m_objLicenceList = null;
            m_objContactList = null;
        }

        public CCompanies(System.Guid uuidId, System.String strName, System.String strAcronym)
        {
            m_uuidID = uuidId;
            m_strName = strName;
            m_strAcronym = strAcronym;
        }

        public CCompanies(System.Guid uuidID, System.Int32 IbID, CCompanyType objCompanyType, System.String strAcronym, System.String strDescription,
            System.String strName, System.String strOKPO, System.String strOKULP, System.String strUNP, System.Boolean IsActive)
        {
            m_uuidID = uuidID;
            m_IbID = IbID;
            m_objCompanyType = objCompanyType;
            m_strAcronym = strAcronym;
            m_strDescription = strDescription;
            m_strName = strName;
            m_strOKPO = "";
            m_strOKULP = "";
            m_strUNP = "";
            m_strOKPO = strOKPO;
            m_strOKULP = strOKULP;
            m_strUNP = strUNP;
            m_IsActive = IsActive;
            m_objEMailList = null;
            m_objPhoneList = null;
            m_objAddressList = null;
            m_objAccountList = null;
            m_objLicenceList = null;
            m_objContactList = null;
        }
        #endregion

        #region Список свойств

        /// <summary>
        /// Возвращает список компаний
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список компаний</returns>
        public static List<CCompanies> GetCompanyList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CCompanies> objCompanyList = new List<CCompanies>();
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
                        return objCompanyList;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetCompany]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objCompanyList.Add(new CCompanies((System.Guid)rs["Company_Guid"], (System.String)rs["Company_Name"], (System.String)rs["Company_Acronym"]));
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
                "Не удалось получить список компаний.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objCompanyList;
        }



        #endregion
    }
    #endregion

    #region Класс "Тип компании"
    /// <summary>
    /// Класс "Тип компании"
    /// </summary>
    public class CCompanyType : CBusinessObject
    {
        #region "Свойства"

        private System.Boolean m_IsActive;
        /// <summary>
        /// Признак активности
        /// </summary>
        public System.Boolean IsActive
        {
            get { return m_IsActive; }
            set { m_IsActive = value; }
        }

        /// <summary>
        /// Описание
        /// </summary>
        private System.String m_strDescription;
        /// <summary>
        /// Описание
        /// </summary>
        public System.String ShortName
        {
            get { return m_strDescription; }
            set { m_strDescription = value; }
        }
        #endregion

        public CCompanyType()
            : base()
        {
        }
        public CCompanyType(Guid uuidId, String strName, String strDescription, Boolean bIsActive)
        {
            ID = uuidId;
            Name = strName;
            m_strDescription = strDescription;
            m_IsActive = bIsActive;
        }
        /*  public CCompanyType(Guid uuidID, string strName)
              : base(uuidID, strName)
          {

          }*/


        #region Список объектов
        public static List<CCompanyType> GetCompanyTypeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CCompanyType> objList = new List<CCompanyType>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCompanyType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CCompanyType((System.Guid)rs["CustomerStateType_Guid"],
                            (System.String)rs["CustomerStateType_Name"],
                            (System.String)rs["CustomerStateType_Description"],
                            (System.Boolean)rs["CustomerStateType_IsActive"]));
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
                "Не удалось получить список форм собственности.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
    }

}
    #endregion

    #endregion

   
    