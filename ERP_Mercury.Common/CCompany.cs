using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ERP_Mercury.Common
{

    #region Класс "Компания"
    public class CCompany : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Сокращение
        /// </summary>
        private System.String m_strAbbr;
        /// <summary>
        /// Сокращение
        /// </summary>
        public System.String Abbr
        {
            get { return m_strAbbr; }
            set { m_strAbbr = value; }
        }

        /// <summary>
        /// Уникальный идентификатор в InterBase
        /// </summary>
        private System.Int32 m_IbID;//+
        /// <summary>
        /// Уникальный идентификатор в InterBase
        /// </summary>
        public System.Int32 InterBaseID
        {
            get { return m_IbID; }
            set { m_IbID = value; }
        }

        /*
        /// <summary>
        /// Код компании
        /// </summary>
        private System.String m_strCode;
        /// <summary>
        /// Код клиента
        /// </summary>
        public System.String Code
        {
            get { return m_strCode; }
            set { m_strCode = value; }
        }
        */
        /*
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
        */
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

        ///// <summary>
        /////  Название компании
        ///// </summary>
        //private System.String m_strName; //- // по-моему поле всё-таки должно быть
        ///// <summary>
        ///// Название компании
        ///// </summary>
        //public System.String Name
        //{
        //    get { return m_strName; }
        //    set { m_strName = value; }
        //}

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

        /// <summary>
        /// Список типов компаний
        /// </summary>
        private CCompanyType m_objCompanyType;
        /// <summary>
        /// Список типов компаний
        /// </summary>
        public CCompanyType CompanyType
        {
            get { return m_objCompanyType; }
            set { m_objCompanyType = value; }
        }

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

        // ---------- поля ...XXXforDelete ----------

        private List<CAddress> m_objAddressForDeleteList;
        public List<CAddress> AddressForDeleteList
        {
            get { return m_objAddressForDeleteList; }
            set { m_objAddressForDeleteList = value; }
        }
        private List<CPhone> m_objPhoneForDeleteList;
        public List<CPhone> PhoneForDeleteList
        {
            get { return m_objPhoneForDeleteList; }
            set { m_objPhoneForDeleteList = value; }
        }
        private List<CLicence> m_objLicenceForDeleteList;
        public List<CLicence> LicenceForDeleteList
        {
            get { return m_objLicenceForDeleteList; }
            set { m_objLicenceForDeleteList = value; }
        }
        private List<CContact> m_objContactForDeleteList;
        public List<CContact> ContactForDeleteList
        {
            get { return m_objContactForDeleteList; }
            set { m_objContactForDeleteList = value; }
        }
        private List<CAccount> m_objAccountForDeleteList;
        public List<CAccount> AccountForDeleteList
        {
            get { return m_objAccountForDeleteList; }
            set { m_objAccountForDeleteList = value; }
        }
        private List<CRtt> m_objRttForDeleteList;
        public List<CRtt> RttForDeleteList
        {
            get { return m_objRttForDeleteList; }
            set { m_objRttForDeleteList = value; }
        }
        private List<CEMail> m_objEMailForDeleteList;
        public List<CEMail> EMailForDeleteList
        {
            get { return m_objEMailForDeleteList; }
            set { m_objEMailForDeleteList = value; }
        }
        private const System.Int32 iCommandTimeOutForIB = 120;

        // ---------- end полей XXXforDelete ----------

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

        /// <summary>
        /// Форма собственности
        /// </summary>
        private CStateTypeCompany m_objStateType;
        /// <summary>
        /// Форма собственности
        /// </summary>
        public CStateTypeCompany StateType
        {
            get { return m_objStateType; }
            set { m_objStateType = value; }
        }
        public System.String StateTypeName
        {
            get { return ((StateType == null) ? System.String.Empty : StateType.Name); }
        }

        private System.Int32 m_idDefCustomerId;
        public System.Int32 DefCustomerId
        {
            get { return m_idDefCustomerId; }
            set { m_idDefCustomerId = value; }
        }
        #endregion

        #region Конструктор
        public CCompany()
            : base()
        {
            // До переноса этот конструктор (по умолчанию) был пустой 
            //m_uuidID = System.Guid.Empty; // эти поля инициализируются в конструкторе CBusinesObject
            m_IbID = 0;
            m_objCompanyType = null;
            m_strDescription = "";
            Name = ""; // было закоментированно 28.09.11
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
            m_objStateType = null;
            m_idDefCustomerId = 0;
        }
        public CCompany(System.Guid uuidId, System.String strName, System.String strAbbr)
        {
            ID = uuidId;
            Name = strName;
            m_strAbbr = strAbbr;
            m_idDefCustomerId = 0;
        }
              
        public CCompany(System.Guid uuidID, System.Int32 IbID, System.String strName, System.String strAbbr, System.String strDescription, System.String strOKPO, System.String strOKULP, System.String strUNP,
                        System.Boolean IsActive, CCompanyType objCompanyType, CStateTypeCompany objStateTypeCompany)
        {
            ID = uuidID;
            m_IbID = IbID;
            Name = strName;
            m_strAbbr = strAbbr; // strAbbr == strAcronym => Acronym
            m_strDescription = strDescription;
            m_strOKPO = strOKPO;
            m_strOKULP = strOKULP;
            m_strUNP = strUNP;
            m_IsActive = IsActive;
            m_objCompanyType = objCompanyType;
            m_objStateType = objStateTypeCompany;

            m_objEMailList = null;
            m_objPhoneList = null;
            m_objAddressList = null;
            m_objAccountList = null;
            m_objLicenceList = null;
            m_objContactList = null;
            m_idDefCustomerId = 0;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список компаний
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список компаний</returns>
        public static List<CCompany> GetCompanyList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CCompany> objList = new List<CCompany>();
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
                        objList.Add(new CCompany((System.Guid)rs["Company_Guid"],
                            ((rs["Company_Id"] == System.DBNull.Value) ? 0 : (System.Int32)rs["Company_Id"]),
                            (System.String)rs["Company_Name"],
                            (System.String)rs["Company_Acronym"],
                            (System.String)rs["Company_Description"],
                            (System.String)rs["Company_OKPO"],
                            ((rs["Company_OKULP"] == System.DBNull.Value) ? "" : (System.String)rs["Company_OKULP"]),
                            (System.String)rs["Company_UNN"],
                            (System.Boolean)rs["Company_IsActive"],
                            // тип компании
                                new CCompanyType((System.Guid)rs["CompanyType_Guid"],
                                                 (System.String)rs["CompanyType_name"]),
                            /*,
                            ((rs["CompanyType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["CompanyType_Description"]),
                            (System.Boolean)rs["CompanyType_IsActive"])*/
                            // форма собственности
                                new CStateTypeCompany((System.Guid)rs["CustomerStateType_Guid"],
                                (System.String)rs["CustomerStateType_Name"],
                                (System.String)rs["CustomerStateType_ShortName"],
                                (System.Boolean)rs["CustomerStateType_IsActive"])
                                ));

                        objList.Last<CCompany>().DefCustomerId = ((rs["DefCustomerId"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["DefCustomerId"]));
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
            return objList;
        }
        /// <summary>
        /// Возвращает список компаний не входящих в "черный список"
        /// </summary>
        /// <param name="objCustomer">клиент</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список компаний</returns>
        public static List<CCompany> GetCompanyListFreeBlackList(CCustomer objCustomer, UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CCompany> objList = new List<CCompany>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCompanyFreeBlackList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Customer_Guid"].Value = objCustomer.ID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CCompany((System.Guid)rs["Company_Guid"],
                            (System.String)rs["Company_Name"], (System.String)rs["Company_Acronym"]));
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
                "Не удалось получить список компаний вне черного списка.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }

        /// <summary>
        /// Возвращает список компаний, с которыми работал перевозчик за указанный период
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="uuidCarrierId">УИ перевозчика</param>
        /// <param name="dtBeginDate">начало периода</param>
        /// <param name="dtEndDate">конец периода</param>
        /// <returns>список компаний</returns>
        public static List<CCompany> GetCompanyListForRouteSheet(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, 
            System.Guid uuidCarrierId, System.DateTime dtBeginDate, System.DateTime dtEndDate  )
        {
            List<CCompany> objList = new List<CCompany>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetCompanyForRouteSheetFromERP_Report]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Carrier_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters["@Carrier_Guid"].Value = uuidCarrierId;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BeginDate", System.Data.SqlDbType.Date));
                cmd.Parameters["@BeginDate"].Value = dtBeginDate;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EndDate", System.Data.SqlDbType.Date));
                cmd.Parameters["@EndDate"].Value = dtEndDate;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CCompany((System.Guid)rs["Company_Guid"],
                            ((rs["Company_Id"] == System.DBNull.Value) ? 0 : (System.Int32)rs["Company_Id"]),
                            (System.String)rs["Company_Name"],
                            (System.String)rs["Company_Acronym"],
                            (System.String)rs["Company_Description"],
                            (System.String)rs["Company_OKPO"],
                            ((rs["Company_OKULP"] == System.DBNull.Value) ? "" : (System.String)rs["Company_OKULP"]),
                            (System.String)rs["Company_UNN"],
                            (System.Boolean)rs["Company_IsActive"],
                            // тип компании
                                new CCompanyType((System.Guid)rs["CompanyType_Guid"],
                                                 (System.String)rs["CompanyType_name"]),
                            /*,
                            ((rs["CompanyType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["CompanyType_Description"]),
                            (System.Boolean)rs["CompanyType_IsActive"])*/
                            // форма собственности
                                new CStateTypeCompany((System.Guid)rs["CustomerStateType_Guid"],
                                (System.String)rs["CustomerStateType_Name"],
                                (System.String)rs["CustomerStateType_ShortName"],
                                (System.Boolean)rs["CustomerStateType_IsActive"])
                                ));
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
            return objList;
        }

        /// <summary>
        /// Возвращает список компаний у которых установлен признак "активна"
        /// </summary>
        /// <param name="objCustomer">клиент</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список компаний</returns>
        public static List<CCompany> GetCompanyListActive (UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CCompany> objList = new List<CCompany>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCompanyActive]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                //cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                //cmd.Parameters["@Customer_Guid"].Value = objCustomer.ID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CCompany((System.Guid)rs["Company_Guid"],
                            (System.String)rs["Company_Name"], (System.String)rs["Company_Acronym"]));
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
                "Не удалось получить список компаний вне черного списка.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }

        /// <summary>
        /// Возвращает компанию по её р/с
        /// </summary>
        /// <param name="objCustomer">клиент</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список компаний</returns>
        public static List<CCompany> GetCompanyByAccount(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, System.String sAccount)
        {
            List<CCompany> objList = new List<CCompany>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCompanyByAccount]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_Account", System.Data.SqlDbType.NVarChar, 13));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Earning_Account"].Value = sAccount;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CCompany((System.Guid)rs["Company_Guid"],
                            (System.String)rs["Company_Name"], (System.String)rs["Company_Acronym"]));
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
                "Не удалось получить список компаний вне черного списка.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }

        #endregion

        #region IsAllParametersValid
        /// <summary>
        /// Проверка свойств компании перед сохранением
        /// </summary>
        /// <param name="strErr">текст с ошибкой</param>
        /// <returns>true - все свойства корректны; false - ошибка</returns>
        public System.Boolean IsAllParametersValid(ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (this.Name.Trim() == "")
                {
                    strErr = "Компания: Необходимо указать название компании!";
                    return bRet;
                }
                if (this.UNP.Trim() == "")
                {
                    strErr = "Компания: Необходимо указать УНП компании!";
                    return bRet;
                }
                if (this.OKPO.Trim() == "")
                {
                    strErr = "Компания: Необходимо указать ОКПО компании!";
                    return bRet;
                }
                if (this.OKULP.Trim() == "")
                {
                    strErr = "Компания: Необходимо указать ОКЮЛП компании!";
                    return bRet;
                }
                if (this.StateType == null)
                {
                    strErr = "Компания: Необходимо указать форму собственности компании!";
                    return bRet;
                }
                /*if (this.ActiveType == null)
                {
                    strErr = "Компания: Необходимо указать признак активности клиента!";
                    return bRet;
                }*/
                if ((this.AddressList == null) || (this.AddressList.Count == 0))
                {
                    strErr = "Компания: Необходимо указать адрес!";
                    return bRet;
                }
                if ((this.AddressList != null) && (this.AddressList.Count > 0))
                {
                    System.Boolean bIsAllAddressValid = true;
                    foreach (CAddress objItem in this.AddressList)
                    {
                        if (objItem.IsAllParametersValid(ref strErr) == false)
                        {
                            bIsAllAddressValid = false;
                            break;
                        }
                    }
                    if (bIsAllAddressValid == false)
                    {
                        return bRet;
                    }
                }
                if ((this.ContactList != null) && (this.ContactList.Count > 0))
                {
                    System.Boolean bIsAllContactValid = true;
                    foreach (CContact objItem in this.ContactList)
                    {
                        if (objItem.IsAllParametersValid(ref strErr) == false)
                        {
                            bIsAllContactValid = false;
                            break;
                        }
                    }
                    if (bIsAllContactValid == false)
                    {
                        return bRet;
                    }
                }
                if ((this.LicenceList != null) && (this.LicenceList.Count > 0))
                {
                    System.Boolean bIsAllLicenceValid = true;
                    foreach (CLicence objItem in this.LicenceList)
                    {
                        if (objItem.IsAllParametersValid(ref strErr) == false)
                        {
                            bIsAllLicenceValid = false;
                            break;
                        }
                    }
                    if (bIsAllLicenceValid == false)
                    {
                        return bRet;
                    }
                }
                //else
                //{
                //    strErr = "Клиент: Необходимо указать лицензию клиента!";
                //    return bRet;
                //}


                if ((this.AccountList != null) && (this.AccountList.Count > 0))
                {
                    System.Boolean bIsAllAccountValid = true;
                    foreach (CAccount objItem in this.AccountList)
                    {
                        if (objItem.IsAllParametersValid(ref strErr) == false)
                        {
                            bIsAllAccountValid = false;
                            break;
                        }
                    }
                    if (bIsAllAccountValid == false)
                    {
                        return bRet;
                    }
                }
                else
                {
                    strErr = "Компания: Необходимо указать расчетный счет компании!";
                    return bRet;
                }

                // Здесь написать проверку
               // if ((this.AccountList != null) && (this.AccountList.Count > 0))
               // {
                   // System.Boolean bIsAllAccountValid = true;


                //}
                
                if ((this.EMailList != null) && (this.EMailList.Count > 0))
                {
                    System.Boolean bIsAllEMailValid = true;
                    foreach (CEMail objItem in this.EMailList)
                    {
                        if (objItem.IsAllParametersValid(ref strErr) == false)
                        {
                            bIsAllEMailValid = false;
                            break;
                        }
                    }
                    if (bIsAllEMailValid == false)
                    {
                        return bRet;
                    }
                }
                if ((this.PhoneList != null) && (this.PhoneList.Count > 0))
                {
                    System.Boolean bIsAllPhoneValid = true;
                    foreach (CPhone objItem in this.PhoneList)
                    {
                        if (objItem.IsAllParametersValid(ref strErr) == false)
                        {
                            bIsAllPhoneValid = false;
                            break;
                        }
                    }
                    if (bIsAllPhoneValid == false)
                    {
                        return bRet;
                    }
                }

                
                /*if ((this.RttList != null) && (this.RttList.Count > 0))
                {
                    System.Boolean bIsAllRttValid = true;
                    foreach (CRtt objItem in this.RttList)
                    {
                        if (objItem.IsChanged == true)
                        {
                            if (objItem.IsAllParametersValid(ref strErr) == false)
                            {
                                bIsAllRttValid = false;
                                break;
                            }
                        }
                    }
                    if (bIsAllRttValid == false)
                    {
                        return bRet;
                    }
                }*/

                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr = "Ошибка проверки свойств клиента. Текст ошибки: " + f.Message;
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
        public System.Boolean Add(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            System.Boolean bSaveInIB = false;

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
                    cmd.CommandTimeout = iCommandTimeOutForIB;
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }
                cmd.CommandTimeout = iCommandTimeOutForIB;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddCompany]", objProfile.GetOptionsDllDBName()); ;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Acronym", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_OKPO", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_OKULP", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_UNP", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_IsActive", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerStateType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@CompanyType_Guid"].Value = this.CompanyType.ID;
                cmd.Parameters["@Company_Acronym"].Value = this.Abbr;
                cmd.Parameters["@Company_Description"].Value = this.Description;
                cmd.Parameters["@Company_Name"].Value = this.Name;
                cmd.Parameters["@Company_OKPO"].Value = this.OKPO;
                cmd.Parameters["@Company_OKULP"].Value = this.OKULP;
                cmd.Parameters["@Company_UNP"].Value = this.UNP;
                cmd.Parameters["@Company_IsActive"].Value = this.IsActive;
                cmd.Parameters["@CustomerStateType_Guid"].Value = this.StateType.ID;
              
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    this.ID = (System.Guid)cmd.Parameters["@Company_Guid"].Value;
                    // теперь списки контактов, адресов, лицензий
                    // возможна ситуация, когда при сохранении нового клиента произошла ошибка,
                    // при этом контакты и адреса получили идентификаторы
                    // их нужно сбросить в Empty
                    if ((this.ContactList != null) && (this.ContactList.Count > 0))
                    {
                        foreach (CContact objContact in this.ContactList)
                        {
                            objContact.ID = System.Guid.Empty;
                            if ((objContact.AddressList != null) && (objContact.AddressList.Count > 0))
                            {
                                foreach (CAddress objAddress in objContact.AddressList) { objAddress.ID = System.Guid.Empty; }
                            }
                            if ((objContact.EMailList != null) && (objContact.EMailList.Count > 0))
                            {
                                foreach (CEMail objEMail in objContact.EMailList) { objEMail.ID = System.Guid.Empty; }
                            }
                            if ((objContact.PhoneList != null) && (objContact.PhoneList.Count > 0))
                            {
                                foreach (CPhone objPhone in objContact.PhoneList) { objPhone.ID = System.Guid.Empty; }
                            }
                        }
                        iRes = (CContact.SaveContactList(this.ContactList, null, EnumObject.Company, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1; // ОШИБКА ЗДЕСЬ
                    }
                    if (iRes == 0)
                    {
                        if ((this.AddressList != null) && (this.AddressList.Count > 0) && (this.AddressForDeleteList != null))
                        {
                            foreach (CAddress objAddress in this.AddressList) { objAddress.ID = System.Guid.Empty; }
                            iRes = (CAddress.SaveAddressList(this.AddressList, null, EnumObject.Company, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.LicenceList != null) && (this.LicenceList.Count > 0))
                        {
                            foreach (CLicence objLicence in this.LicenceList) { objLicence.ID = System.Guid.Empty; }
                            iRes = (CLicence.SaveLicenceListForCompany(this.LicenceList, null, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.PhoneList != null) && (this.PhoneList.Count > 0))
                        {
                            foreach (CPhone objPhone in this.PhoneList) { objPhone.ID = System.Guid.Empty; }
                            iRes = (CPhone.SavePhoneList(this.PhoneList, null, EnumObject.Company, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.EMailList != null) && (this.EMailList.Count > 0))
                        {
                            foreach (CEMail objEMail in this.EMailList) { objEMail.ID = System.Guid.Empty; }
                            iRes = (CEMail.SaveEMailList(this.EMailList, null, EnumObject.Company, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.AccountList != null) && (this.AccountList.Count > 0))
                        {
                            foreach (CAccount objAccount in this.AccountList) { objAccount.ID = System.Guid.Empty; }
                            iRes = (CAccount.SaveAccountList(this.AccountList, null, EnumObject.Company, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1; // сделал. остановился здесь
                        }
                    }

                   // теперь все это нужно записать в InterBase
                    // ** временно
                    
                    if (iRes == 0)
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddCompanyToIB]", objProfile.GetOptionsDllDBName()); ;
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Id", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                        cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                        cmd.Parameters["@Company_Guid"].Value = this.ID;
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
                            // нам пришлось откатить транзакцию
                            // нужно пройтись по объектам, связанным с компанией, и обнулить значение ID у тех из них,
                            // которые являются новыми, и их описания нет в БД

                            // если мы откатываем транзакцию, а запись в InterBase уже прошла, то нужно удалить в IB компанию
                            
                            // **временно
                            
                            if (bSaveInIB == true)
                            {
                                DeleteCompanyFromIB(objProfile, cmd, ref strErr);
                            }
                            
                            this.ID = System.Guid.Empty;
                            

                            if ((this.AddressList != null) && (this.AddressList.Count > 0))
                            {
                                foreach (CAddress objAddress in this.AddressList)
                                {
                                    if (objAddress.IsNewObject == true) { objAddress.ID = System.Guid.Empty; }
                                }
                            }
                            if ((this.ContactList != null) && (this.ContactList.Count > 0))
                            {
                                foreach (CContact objAddress in this.ContactList)
                                {
                                    if (objAddress.IsNewObject == true)
                                    {
                                        objAddress.ID = System.Guid.Empty;
                                        objAddress.ClearIDFromChildObject();
                                    }
                                }
                            }
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
        /// <summary>
        /// Проверка свойств перед сохранением
        /// </summary>
        /// <param name="CompanyName">наименование компании</param>
        /// <param name="CompanyAddress">адрес</param>
        /// <param name="CompanyAcronym">аббревиатура</param>
        /// <param name="CompanyAccount">расчётный счёт</param>
        /// <param name="CompanyBankCode">код банка</param>
        /// <param name="CompanyBankName">наименование банка</param>
        /// <param name="CompanyUNN">УНП</param>
        /// <param name="CompanyOKPO">ОКПО</param>
        /// <param name="CompanyOKULP">ОКЮЛП</param>
        /// <param name="CompanyLicenceImport">лицензия импортёра</param>
        /// <param name="CompanyLicenceTrade">лицензия продавца</param>
        /// <param name="CompanyIsActive">признак "активна"</param>
        /// <param name="Company_id">УИ компании в InterBase</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - все свойства прошли проверку; false - ошибка проверки, либо не все свойства прошли проверку</returns>
        public static System.Boolean IsAllParametersValidForIB(System.String CompanyName, System.String CompanyAddress, System.String CompanyAcronym,
            System.String CompanyAccount, System.String CompanyBankCode, System.String CompanyBankName, System.String CompanyUNN,
            System.String CompanyOKPO, System.String CompanyOKULP, System.String CompanyLicenceImport, System.String CompanyLicenceTrade,
            System.Boolean CompanyIsActive, ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (CompanyName.Trim().Length == 0)
                {
                    strErr +=( "Наименование компании не должно быть пустым значением!" );
                    return bRet;
                }
                if (CompanyAcronym.Trim().Length == 0)
                {
                    strErr += ("Аббревиатура компании не должна быть пустым значением!");
                    return bRet;
                }
                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr +=( f.Message );
            }

            return bRet;
        }
        /// <summary>
        /// Добавление записи в справочник компаний в InterBase
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="CompanyName">наименование компании</param>
        /// <param name="CompanyAddress">адрес</param>
        /// <param name="CompanyAcronym">аббревиатура</param>
        /// <param name="CompanyAccount">расчётный счёт</param>
        /// <param name="CompanyBankCode">код банка</param>
        /// <param name="CompanyBankName">наименование банка</param>
        /// <param name="CompanyUNN">УНП</param>
        /// <param name="CompanyOKPO">ОКПО</param>
        /// <param name="CompanyOKULP">ОКЮЛП</param>
        /// <param name="CompanyLicenceImport">лицензия импортёра</param>
        /// <param name="CompanyLicenceTrade">лицензия продавца</param>
        /// <param name="CompanyIsActive">признак "активна"</param>
        /// <param name="Company_id">УИ компании в InterBase</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата хранимой процедуры</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean AddCompanyToIB(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, 
            System.String CompanyName, System.String CompanyAddress, System.String CompanyAcronym,
            System.String CompanyAccount, System.String CompanyBankCode, System.String CompanyBankName, System.String CompanyUNN,
            System.String CompanyOKPO, System.String CompanyOKULP, System.String CompanyLicenceImport, System.String CompanyLicenceTrade,
            System.Boolean CompanyIsActive, ref System.Int32 Company_id, ref System.String strErr, ref System.Int32 iRes  )
        {
            System.Boolean bRet = false;
            if (IsAllParametersValidForIB(CompanyName, CompanyAddress, CompanyAcronym,
                CompanyAccount, CompanyBankCode, CompanyBankName, CompanyUNN,
                CompanyOKPO, CompanyOKULP, CompanyLicenceImport, CompanyLicenceTrade, CompanyIsActive, ref strErr ) == false) { return bRet; }

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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddCompanyToIB]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Id", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyAddress", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyAcronym", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyAccount", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyBankCode", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyBankName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyUNN", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyOKPO", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyOKULP", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyLicenceImport", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyLicenceTrade", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyIsActive", System.Data.SqlDbType.Bit));


                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@CompanyName"].Value = CompanyName;
                cmd.Parameters["@CompanyAddress"].Value = CompanyAddress;
                cmd.Parameters["@CompanyAcronym"].Value = CompanyAcronym;
                cmd.Parameters["@CompanyAccount"].Value = CompanyAccount;
                cmd.Parameters["@CompanyBankCode"].Value = CompanyBankCode;
                cmd.Parameters["@CompanyBankName"].Value = CompanyBankName;
                cmd.Parameters["@CompanyUNN"].Value = CompanyUNN;
                cmd.Parameters["@CompanyOKPO"].Value = CompanyOKPO;
                cmd.Parameters["@CompanyOKULP"].Value = CompanyOKULP;
                cmd.Parameters["@CompanyLicenceImport"].Value = CompanyLicenceImport;
                cmd.Parameters["@CompanyLicenceTrade"].Value = CompanyLicenceTrade;
                cmd.Parameters["@CompanyIsActive"].Value = CompanyIsActive;
                cmd.ExecuteNonQuery();
                
                iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                
                if (iRes == 0)
                {
                    Company_id = (System.Int32)cmd.Parameters["@Company_id"].Value;
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
        public static System.Boolean IsAllParametersValid( System.Guid CompanyType_Guid, System.String Company_Acronym,
            System.String Company_Name, System.String Company_OKPO, System.String Company_OKULP, System.String Company_UNP,
            System.Guid CustomerStateType_Guid,
            ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (CompanyType_Guid.CompareTo(System.Guid.Empty) == 0)
                {
                    strErr += ("Необходимо указать тип компании!");
                    return bRet;
                }
                if (CustomerStateType_Guid.CompareTo(System.Guid.Empty) == 0)
                {
                    strErr += ("Необходимо указать форму собственности!");
                    return bRet;
                }
                if (Company_Acronym.Trim().Length == 0)
                {
                    strErr += ("Необходимо указать аббревиатуру компании!");
                    return bRet;
                }
                if (Company_Name.Trim().Length == 0)
                {
                    strErr += ("Необходимо указать наименование компании!");
                    return bRet;
                }
                if (Company_OKPO.Trim().Length == 0)
                {
                    strErr += ("Необходимо указать ОКПО компании!");
                    return bRet;
                }
                if (Company_OKULP.Trim().Length == 0)
                {
                    strErr += ("Необходимо указать ОКЮЛП компании!");
                    return bRet;
                }
                if (Company_UNP.Trim().Length == 0)
                {
                    strErr += ("Необходимо указать УНП компании!");
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
        /// <summary>
        /// Добавляет запись в справочник компаний
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="Company_Id">УИ компании в InterBase</param>
        /// <param name="CompanyType_Guid">УИ типа компании</param>
        /// <param name="Company_Acronym">аббревиатура</param>
        /// <param name="Company_Description">примечание</param>
        /// <param name="Company_Name">наименование компании</param>
        /// <param name="Company_OKPO">ОКПО</param>
        /// <param name="Company_OKULP">ОКЮЛП</param>
        /// <param name="Company_UNP">УНП</param>
        /// <param name="Company_IsActive">признак "активна"</param>
        /// <param name="CustomerStateType_Guid">УИ формы собственности</param>
        /// <param name="ContactList">список контактов</param>
        /// <param name="ContactForDeleteList">список контактов для удаления</param>
        /// <param name="AddressList">список адресов</param>
        /// <param name="AddressForDeleteList">список адресов для удаления</param>
        /// <param name="LicenceList">список лицензий</param>
        /// <param name="LicenceForDeleteList">список лицензий для удаления</param>
        /// <param name="PhoneList">список телефонов</param>
        /// <param name="PhoneForDeleteList">список телефонов для удаления</param>
        /// <param name="EMailList">список электронных адресов</param>
        /// <param name="EMailForDeleteList">список электронных адресов для удаления</param>
        /// <param name="AccountList">список расчётных счетов</param>
        /// <param name="AccountForDeleteList">список расчётных счетов для удаления</param>
        /// <param name="Company_Guid">УИ компании</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата хранимой процедуры</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean AddCompanyToDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Int32 Company_Id,
            System.Guid CompanyType_Guid, System.String Company_Acronym, System.String Company_Description, System.String Company_Name,
            System.String Company_OKPO, System.String Company_OKULP, System.String Company_UNP, System.Boolean Company_IsActive, 
            System.Guid CustomerStateType_Guid,
            List<CContact> ContactList, List<CContact> ContactForDeleteList, 
            List<CAddress> AddressList, List<CAddress> AddressForDeleteList,
            List<CLicence> LicenceList, List<CLicence> LicenceForDeleteList,
            List<CPhone> PhoneList, List<CPhone> PhoneForDeleteList,
            List<CEMail> EMailList, List<CEMail> EMailForDeleteList,
            List<CAccount> AccountList, List<CAccount> AccountForDeleteList,
            ref System.Guid Company_Guid, ref System.String strErr, ref System.Int32 iRes)
        {
            System.Boolean bRet = false;

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;

            try
            {
                if (IsAllParametersValid(CompanyType_Guid, Company_Acronym, Company_Name, Company_OKPO,
                    Company_OKULP, Company_UNP, CustomerStateType_Guid, ref strErr) == false)
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
                    cmd.CommandTimeout = iCommandTimeOutForIB;
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }
                cmd.CommandTimeout = iCommandTimeOutForIB;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddCompany]", objProfile.GetOptionsDllDBName()); ;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Id", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Acronym", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_OKPO", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_OKULP", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_UNP", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_IsActive", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerStateType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Company_Id"].Value = Company_Id;
                cmd.Parameters["@CompanyType_Guid"].Value = CompanyType_Guid;
                cmd.Parameters["@Company_Acronym"].Value = Company_Acronym;
                cmd.Parameters["@Company_Description"].Value = Company_Description;
                cmd.Parameters["@Company_Name"].Value = Company_Name;
                cmd.Parameters["@Company_OKPO"].Value = Company_OKPO;
                cmd.Parameters["@Company_OKULP"].Value = Company_OKULP;
                cmd.Parameters["@Company_UNP"].Value = Company_UNP;
                cmd.Parameters["@Company_IsActive"].Value = Company_IsActive;
                cmd.Parameters["@CustomerStateType_Guid"].Value = CustomerStateType_Guid;

                cmd.ExecuteNonQuery();
                iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;

                if (iRes == 0)
                {
                    Company_Guid = (System.Guid)cmd.Parameters["@Company_Guid"].Value;
                    // теперь списки контактов, адресов, лицензий
                    // возможна ситуация, когда при сохранении нового клиента произошла ошибка,
                    // при этом контакты и адреса получили идентификаторы
                    // их нужно сбросить в Empty
                    if ((ContactList != null) && (ContactList.Count > 0))
                    {
                        //foreach (CContact objContact in ContactList)
                        //{
                        //    objContact.ID = System.Guid.Empty;
                        //    if ((objContact.AddressList != null) && (objContact.AddressList.Count > 0))
                        //    {
                        //        foreach (CAddress objAddress in objContact.AddressList) { objAddress.ID = System.Guid.Empty; }
                        //    }
                        //    if ((objContact.EMailList != null) && (objContact.EMailList.Count > 0))
                        //    {
                        //        foreach (CEMail objEMail in objContact.EMailList) { objEMail.ID = System.Guid.Empty; }
                        //    }
                        //    if ((objContact.PhoneList != null) && (objContact.PhoneList.Count > 0))
                        //    {
                        //        foreach (CPhone objPhone in objContact.PhoneList) { objPhone.ID = System.Guid.Empty; }
                        //    }
                        //}
                        iRes = (CContact.SaveContactList(ContactList, null, EnumObject.Company, Company_Guid, objProfile, cmd, ref strErr) == true) ? 0 : -1; // ОШИБКА ЗДЕСЬ
                    }
                    if (iRes == 0)
                    {
                        if ((AddressList != null) && (AddressList.Count > 0) && (AddressForDeleteList != null))
                        {
                            //foreach (CAddress objAddress in AddressList) { objAddress.ID = System.Guid.Empty; }
                            iRes = (CAddress.SaveAddressList(AddressList, AddressForDeleteList, EnumObject.Company, Company_Guid, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((LicenceList != null) && (LicenceList.Count > 0))
                        {
                            //foreach (CLicence objLicence in LicenceList) { objLicence.ID = System.Guid.Empty; }
                            iRes = (CLicence.SaveLicenceListForCompany(LicenceList, LicenceForDeleteList, Company_Guid, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((PhoneList != null) && (PhoneList.Count > 0))
                        {
                            //foreach (CPhone objPhone in PhoneList) { objPhone.ID = System.Guid.Empty; }
                            iRes = (CPhone.SavePhoneList(PhoneList, PhoneForDeleteList, EnumObject.Company, Company_Guid, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((EMailList != null) && (EMailList.Count > 0))
                        {
                            //foreach (CEMail objEMail in EMailList) { objEMail.ID = System.Guid.Empty; }
                            iRes = (CEMail.SaveEMailList(EMailList, EMailForDeleteList, EnumObject.Company, Company_Guid, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((AccountList != null) && (AccountList.Count > 0))
                        {
                            //foreach (CAccount objAccount in AccountList) { objAccount.ID = System.Guid.Empty; }
                            iRes = (CAccount.SaveAccountList(AccountList, AccountForDeleteList, EnumObject.Company, Company_Guid, objProfile, cmd, ref strErr) == true) ? 0 : -1; // сделал. остановился здесь
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

        #region Update
        /// <summary>
        /// Сохраняет в базе данных изменения в описании компании
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="objContactDeletedList">список удаленных контактов</param>
        /// <param name="objAddressDeletedList">список удаленных адресов</param>
        /// <param name="objLicenceDeletedList">список удаленных лицензий</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Update(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
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
                    cmd.CommandTimeout = iCommandTimeOutForIB;
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }
                cmd.CommandTimeout = iCommandTimeOutForIB;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditCompany]", objProfile.GetOptionsDllDBName()); ;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerStateType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Acronym", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Id", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_UNP", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_OKPO", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_OKULP", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_IsActive", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Company_Guid"].Value = this.ID;
                cmd.Parameters["@Company_Name"].Value = this.Name;
                cmd.Parameters["@Company_Acronym"].Value = this.Abbr;
                cmd.Parameters["@Company_Id"].Value = this.InterBaseID; // для IB // здесь null
                cmd.Parameters["@Company_Description"].Value = this.Description;
                cmd.Parameters["@Company_UNP"].Value = this.UNP;
                cmd.Parameters["@Company_OKPO"].Value = this.OKPO;
                cmd.Parameters["@Company_OKULP"].Value = this.OKULP;
                cmd.Parameters["@Company_IsActive"].Value = this.IsActive;
                cmd.Parameters["@CustomerStateType_Guid"].Value = this.StateType.ID;
                cmd.Parameters["@CompanyType_Guid"].Value = this.CompanyType.ID;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    // теперь списки контактов, адресов, лицензий и телефонов
                    if ((this.ContactList != null) || (this.ContactForDeleteList != null))
                    {
                        iRes = (CContact.SaveContactList(this.ContactList, this.ContactForDeleteList, EnumObject.Company, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1; //*    
                    }
                    if (iRes == 0)
                    {
                        if ((this.AddressList != null) || (this.AddressForDeleteList != null))
                        {
                            iRes = (CAddress.SaveAddressList(this.AddressList, this.AddressForDeleteList, EnumObject.Company, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1; 
                            iRes = (CAddress.SaveAddressList(this.AddressList, this.AddressForDeleteList, EnumObject.Contact, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1; 
                            //возможно нужно написать здесь такую же строчку, но передавать EnumObject.Contact
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.LicenceList != null) || (this.LicenceForDeleteList != null))
                        {
                            iRes = (CLicence.SaveLicenceListForCompany(this.LicenceList, this.LicenceForDeleteList, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.PhoneList != null) || (this.PhoneForDeleteList != null))
                        {
                            iRes = (CPhone.SavePhoneList(this.PhoneList, this.PhoneForDeleteList, EnumObject.Company, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1; // вроде исправил, но нужно проверить
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.EMailList != null) || (this.EMailForDeleteList != null))
                        {
                            iRes = (CEMail.SaveEMailList(this.EMailList, this.EMailForDeleteList, EnumObject.Company, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1; //*
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.AccountList != null) || (this.AccountForDeleteList != null))
                        {
                            iRes = (CAccount.SaveAccountList(this.AccountList, this.AccountForDeleteList, EnumObject.Company, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1; //*
                        }
                    }
                    /* if (iRes == 0)
                     {
                         if (this.CompanyType != null) // По моеиму, не нужно, т.к. CompanyType_Guid сохраняется в ХП выше.
                         {
                             //iRes = (CCustomerType.SaveCustomerTypeListForCustomer(this.ID, this.CustomerTypeList, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                             iRes = (CCompanyType.SaveCompanyTypeListForCompany(this.ID, this.CompanyType, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                         }
                     }*/
                    /*if (iRes == 0)
                    {
                        if (this.TargetBuyList != null)
                        {
                            iRes = (CTargetBuy.SaveTargetBuyListForCustomer(this.ID, this.TargetBuyList, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }*/
                    /*if (iRes == 0)
                    {
                        if ((this.RttList != null) || (this.RttForDeleteList != null))
                        {
                            iRes = (CRtt.SaveRttList(this.RttList, this.RttForDeleteList, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }*/
                    /*
                    if (iRes == 0)
                    {
                        // торговая сеть
                        if (this.m_objDistributionNetwork != null)
                        {
                            this.m_objDistributionNetwork.SaveCustomerToDistrNet(this.ID, objProfile, cmd, ref strErr);
                        }
                        else
                        {
                            CDistributionNetwork.RemoveCustomerFromDistrNet(this.ID, objProfile, cmd, ref strErr);
                        }
                    }*/


                    // теперь все это нужно записать в InterBase
                    // нужно, но временно отключено
                    if (iRes == 0)
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditCompanyToIB]", objProfile.GetOptionsDllDBName()); ;
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                        cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                        cmd.Parameters["@Company_Guid"].Value = this.ID;
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
                            /*
                            if ((this.RttList != null) && (this.RttList.Count > 0))
                            {
                                foreach (CRtt objRtt in this.RttList)
                                {
                                    if (objRtt.IsNewObject == true)
                                    {
                                        objRtt.ID = System.Guid.Empty;
                                        objRtt.ClearIDFromChildObject();
                                    }
                                }
                            }*/
                            if ((this.AddressList != null) && (this.AddressList.Count > 0))
                            {
                                foreach (CAddress objAddress in this.AddressList)
                                {
                                    if (objAddress.IsNewObject == true) { objAddress.ID = System.Guid.Empty; }
                                }
                            }
                            if ((this.ContactList != null) && (this.ContactList.Count > 0))
                            {
                                foreach (CContact objAddress in this.ContactList)
                                {
                                    if (objAddress.IsNewObject == true)
                                    {
                                        objAddress.ID = System.Guid.Empty;
                                        objAddress.ClearIDFromChildObject();
                                    }
                                }
                            }
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
        /// <summary>
        /// Редактирует запись в справочнике компаний
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="Company_Guid">УИ компании</param>
        /// <param name="CompanyType_Guid">УИ типа компании</param>
        /// <param name="Company_Acronym">аббревиатура</param>
        /// <param name="Company_Description">примечание</param>
        /// <param name="Company_Name">наименование компании</param>
        /// <param name="Company_OKPO">ОКПО</param>
        /// <param name="Company_OKULP">ОКЮЛП</param>
        /// <param name="Company_UNP">УНП</param>
        /// <param name="Company_IsActive">признак "активна"</param>
        /// <param name="CustomerStateType_Guid">УИ формы собственности</param>
        /// <param name="ContactList">список контактов</param>
        /// <param name="ContactForDeleteList">список контактов для удаления</param>
        /// <param name="AddressList">список адресов</param>
        /// <param name="AddressForDeleteList">список адресов для удаления</param>
        /// <param name="LicenceList">список лицензий</param>
        /// <param name="LicenceForDeleteList">список лицензий для удаления</param>
        /// <param name="PhoneList">список телефонов</param>
        /// <param name="PhoneForDeleteList">список телефонов для удаления</param>
        /// <param name="EMailList">список электронных адресов</param>
        /// <param name="EMailForDeleteList">список электронных адресов для удаления</param>
        /// <param name="AccountList">список расчётных счетов</param>
        /// <param name="AccountForDeleteList">список расчётных счетов для удаления</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата хранимой процедуры</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean EditCompanyInDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid Company_Guid, System.Guid CompanyType_Guid, System.String Company_Acronym, System.String Company_Description, System.String Company_Name,
            System.String Company_OKPO, System.String Company_OKULP, System.String Company_UNP, System.Boolean Company_IsActive,
            System.Guid CustomerStateType_Guid,
            List<CContact> ContactList, List<CContact> ContactForDeleteList,
            List<CAddress> AddressList, List<CAddress> AddressForDeleteList,
            List<CLicence> LicenceList, List<CLicence> LicenceForDeleteList,
            List<CPhone> PhoneList, List<CPhone> PhoneForDeleteList,
            List<CEMail> EMailList, List<CEMail> EMailForDeleteList,
            List<CAccount> AccountList, List<CAccount> AccountForDeleteList,
            ref System.String strErr, ref System.Int32 iRes)
        {
            System.Boolean bRet = false;

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;

            try
            {
                if (IsAllParametersValid(CompanyType_Guid, Company_Acronym, Company_Name, Company_OKPO,
                    Company_OKULP, Company_UNP, CustomerStateType_Guid, ref strErr) == false)
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
                    cmd.CommandTimeout = iCommandTimeOutForIB;
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }
                cmd.CommandTimeout = iCommandTimeOutForIB;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditCompany]", objProfile.GetOptionsDllDBName()); ;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Acronym", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_OKPO", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_OKULP", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_UNP", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_IsActive", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CustomerStateType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Company_Guid"].Value = Company_Guid;
                cmd.Parameters["@CompanyType_Guid"].Value = CompanyType_Guid;
                cmd.Parameters["@Company_Acronym"].Value = Company_Acronym;
                cmd.Parameters["@Company_Description"].Value = Company_Description;
                cmd.Parameters["@Company_Name"].Value = Company_Name;
                cmd.Parameters["@Company_OKPO"].Value = Company_OKPO;
                cmd.Parameters["@Company_OKULP"].Value = Company_OKULP;
                cmd.Parameters["@Company_UNP"].Value = Company_UNP;
                cmd.Parameters["@Company_IsActive"].Value = Company_IsActive;
                cmd.Parameters["@CustomerStateType_Guid"].Value = CustomerStateType_Guid;

                cmd.ExecuteNonQuery();
                iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;

                if (iRes == 0)
                {
                    // теперь списки контактов, адресов, лицензий
                    // возможна ситуация, когда при сохранении нового клиента произошла ошибка,
                    // при этом контакты и адреса получили идентификаторы
                    // их нужно сбросить в Empty
                    if (((ContactList != null) && (ContactList.Count > 0)) || ((ContactForDeleteList != null) && (ContactForDeleteList.Count > 0)))
                    {
                        //foreach (CContact objContact in ContactList)
                        //{
                        //    objContact.ID = System.Guid.Empty;
                        //    if ((objContact.AddressList != null) && (objContact.AddressList.Count > 0))
                        //    {
                        //        foreach (CAddress objAddress in objContact.AddressList) { objAddress.ID = System.Guid.Empty; }
                        //    }
                        //    if ((objContact.EMailList != null) && (objContact.EMailList.Count > 0))
                        //    {
                        //        foreach (CEMail objEMail in objContact.EMailList) { objEMail.ID = System.Guid.Empty; }
                        //    }
                        //    if ((objContact.PhoneList != null) && (objContact.PhoneList.Count > 0))
                        //    {
                        //        foreach (CPhone objPhone in objContact.PhoneList) { objPhone.ID = System.Guid.Empty; }
                        //    }
                        //}
                        iRes = (CContact.SaveContactList(ContactList, ContactForDeleteList, EnumObject.Company, Company_Guid, objProfile, cmd, ref strErr) == true) ? 0 : -1; // ОШИБКА ЗДЕСЬ
                    }
                    if (iRes == 0)
                    {
                        if (((AddressList != null) && (AddressList.Count > 0)) || ((AddressForDeleteList != null) && (AddressForDeleteList.Count > 0)))
                        {
                            //foreach (CAddress objAddress in AddressList) { objAddress.ID = System.Guid.Empty; }
                            iRes = (CAddress.SaveAddressList(AddressList, AddressForDeleteList, EnumObject.Company, Company_Guid, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if (((LicenceList != null) && (LicenceList.Count > 0)) || ((LicenceForDeleteList != null) && (LicenceForDeleteList.Count > 0)))
                        {
                            //foreach (CLicence objLicence in LicenceList) { objLicence.ID = System.Guid.Empty; }
                            iRes = (CLicence.SaveLicenceListForCompany(LicenceList, LicenceForDeleteList, Company_Guid, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if (((PhoneList != null) && (PhoneList.Count > 0)) || ((PhoneForDeleteList != null) && (PhoneForDeleteList.Count > 0)))
                        {
                            //foreach (CPhone objPhone in PhoneList) { objPhone.ID = System.Guid.Empty; }
                            iRes = (CPhone.SavePhoneList(PhoneList, PhoneForDeleteList, EnumObject.Company, Company_Guid, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if (((EMailList != null) && (EMailList.Count > 0)) || ((EMailForDeleteList != null) && (EMailForDeleteList.Count > 0)))
                        {
                            //foreach (CEMail objEMail in EMailList) { objEMail.ID = System.Guid.Empty; }
                            iRes = (CEMail.SaveEMailList(EMailList, EMailForDeleteList, EnumObject.Company, Company_Guid, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    if (iRes == 0)
                    {
                        if (((AccountList != null) && (AccountList.Count > 0)) || ((AccountForDeleteList != null) && (AccountForDeleteList.Count > 0)))
                        {
                            //foreach (CAccount objAccount in AccountList) { objAccount.ID = System.Guid.Empty; }
                            iRes = (CAccount.SaveAccountList(AccountList, AccountForDeleteList, EnumObject.Company, Company_Guid, objProfile, cmd, ref strErr) == true) ? 0 : -1; // сделал. остановился здесь
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
        /// <summary>
        /// Редактирование записи в справочнике компаний в InterBase
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="Company_id">УИ компании в InterBase</param>
        /// <param name="CompanyName">наименование компании</param>
        /// <param name="CompanyAddress">адрес</param>
        /// <param name="CompanyAcronym">аббревиатура</param>
        /// <param name="CompanyAccount">расчётный счёт</param>
        /// <param name="CompanyBankCode">код банка</param>
        /// <param name="CompanyBankName">наименование банка</param>
        /// <param name="CompanyUNN">УНП</param>
        /// <param name="CompanyOKPO">ОКПО</param>
        /// <param name="CompanyOKULP">ОКЮЛП</param>
        /// <param name="CompanyLicenceImport">лицензия импортёра</param>
        /// <param name="CompanyLicenceTrade">лицензия продавца</param>
        /// <param name="CompanyIsActive">признак "активна"</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата хранимой процедуры</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean EditCompanyInIB(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Int32 Company_id,
            System.String CompanyName, System.String CompanyAddress, System.String CompanyAcronym,
            System.String CompanyAccount, System.String CompanyBankCode, System.String CompanyBankName, System.String CompanyUNN,
            System.String CompanyOKPO, System.String CompanyOKULP, System.String CompanyLicenceImport, System.String CompanyLicenceTrade,
            System.Boolean CompanyIsActive,  ref System.String strErr, ref System.Int32 iRes)
        {
            System.Boolean bRet = false;
            if (IsAllParametersValidForIB(CompanyName, CompanyAddress, CompanyAcronym,
                CompanyAccount, CompanyBankCode, CompanyBankName, CompanyUNN,
                CompanyOKPO, CompanyOKULP, CompanyLicenceImport, CompanyLicenceTrade, CompanyIsActive, ref strErr) == false) { return bRet; }

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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditCompanyToIB]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Id", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyAddress", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyAcronym", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyAccount", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyBankCode", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyBankName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyUNN", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyOKPO", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyOKULP", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyLicenceImport", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyLicenceTrade", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyIsActive", System.Data.SqlDbType.Bit));


                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Company_id"].Value = Company_id;
                cmd.Parameters["@CompanyName"].Value = CompanyName;
                cmd.Parameters["@CompanyAddress"].Value = CompanyAddress;
                cmd.Parameters["@CompanyAcronym"].Value = CompanyAcronym;
                cmd.Parameters["@CompanyAccount"].Value = CompanyAccount;
                cmd.Parameters["@CompanyBankCode"].Value = CompanyBankCode;
                cmd.Parameters["@CompanyBankName"].Value = CompanyBankName;
                cmd.Parameters["@CompanyUNN"].Value = CompanyUNN;
                cmd.Parameters["@CompanyOKPO"].Value = CompanyOKPO;
                cmd.Parameters["@CompanyOKULP"].Value = CompanyOKULP;
                cmd.Parameters["@CompanyLicenceImport"].Value = CompanyLicenceImport;
                cmd.Parameters["@CompanyLicenceTrade"].Value = CompanyLicenceTrade;
                cmd.Parameters["@CompanyIsActive"].Value = CompanyIsActive;
                cmd.ExecuteNonQuery();

                iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;

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

        #region Remove
        /// <summary>
        /// Удаляет из базы данных компании
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Remove(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
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
                    cmd.CommandTimeout = iCommandTimeOutForIB;
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                //сперва удаляем в InterBase
                // ** временно
                System.Int32 iRes = DeleteCompanyFromIB(objProfile, cmd, ref strErr);

                // ** временно раскоментил (нужно закоментить)
                //System.Int32 iRes = 0;

                if (iRes == 0) // если всё OK
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteCompany]", objProfile.GetOptionsDllDBName());
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters["@Company_Guid"].Value = this.ID;
                    cmd.ExecuteNonQuery();
                    iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                //iRes = DeleteCompanyFromIB(objProfile, cmd, ref strErr);

                bRet = (iRes == 0);

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


        /// <summary>
        /// Удаляем компанию из InterBase
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>0 - удачное завершение операции; <>0 - ошибка</returns>
        private System.Int32 DeleteCompanyFromIB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
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
                    cmdSQL.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteCompanyFromIB]", objProfile.GetOptionsDllDBName()); ;
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmdSQL.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                    cmdSQL.Parameters["@Company_Guid"].Value = this.ID;
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
        /// <summary>
        /// Удаление записи в справочнике компаний в InterBase
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="Company_Id">УИ компании в InterBase</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRes">код возврата хранимой процедуры</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean RemoveCompanyFromIB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Int32 Company_Id, ref System.String strErr, ref System.Int32 iRes)
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteCompanyFromIB]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Id", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Company_Id"].Value = Company_Id;
                cmd.ExecuteNonQuery();

                iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;


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

        public static System.Boolean RemoveCompanyFomDB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid Company_Guid, ref System.String strErr, ref System.Int32 iRes)
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteCompany]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Company_Guid"].Value = Company_Guid;
                cmd.ExecuteNonQuery();
                
                iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;

                bRet = (iRes == 0);
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

        public override string ToString()
        {
            //return m_strAbbr; // 31.10.2011
            return Name;
        }
    }
    #endregion

    #region Класс "Тип компании"
    /// <summary>
    /// Класс "Тип компании"
    /// </summary>
    public class CCompanyType : CBusinessObject
    {
        #region "Свойства"
        /*
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
        public System.String Description
        {
            get { return m_strDescription; }
            set { m_strDescription = value; }
        }*/
        #endregion

        public CCompanyType()
            : base()
        {
        }
        public CCompanyType(Guid uuidId, String strName/*, String strDescription, Boolean bIsActive*/)
        {
            ID = uuidId;
            Name = strName;
            //m_strDescription = strDescription;
            //m_IsActive = bIsActive;
        }

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
                        objList.Add(new CCompanyType((System.Guid) rs["CompanyType_Guid"],
                                                     (System.String) rs["CompanyType_Name"]));
                        /*,
                            ((rs["CompanyType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["CompanyType_Description"]),
                            (System.Boolean)rs["CompanyType_IsActive"]));*/
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

        public static System.String GetCompanyTypeListForCompany(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidCompanyId)
        {
            //List<CCompanyType> objList = new List<CCompanyType>();
            System.String strCompanyType = "";
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
                        return strCompanyType;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCompanyCompanyType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Company_Guid"].Value = uuidCompanyId;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        strCompanyType = (System.String)rs["CompanyType_Name"];
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
                "Не удалось получить список типов клиентов.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return strCompanyType;
        }


        /*
        public static List<CCompanyType> GetCompanyTypeListForCompany(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidCompanyId)
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCompanyCompanyType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Company_Guid"].Value = uuidCompanyId;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CCompanyType((System.Guid)rs["CompanyType_Guid"],
                            (System.String)rs["CompanyType_Name"],
                            (System.String)rs["CompanyType_Description"],
                            (System.Boolean)rs["CompanyType_IsActive"]));
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
                "Не удалось получить список типов клиентов.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }*/
        
        #endregion
       
        #region Перегруженный toString
        public override string ToString()
        {
            return (this.Name);
        }
        #endregion

        #region Сохранить список типов компаний для компании
        /// <summary>
        /// Сохраняет в БД привязку типов компаний к компании
        /// </summary>
        /// <param name="uuidCompanyId">уникальный идентификатор клиента</param>
        /// <param name="objCompanyTypeList">список типов клиентов</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean SaveCompanyTypeListForCompany(System.Guid uuidCompanyId, List<CCompanyType> objCompanyTypeList,
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

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteCompanyTypeFromCompany]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Company_Guid"].Value = uuidCompanyId;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    if ((objCompanyTypeList != null) && (objCompanyTypeList.Count > 0))
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddCompanyTypeToCompany]", objProfile.GetOptionsDllDBName());
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters["@Customer_Guid"].Value = uuidCompanyId;

                        foreach (CCompanyType objCompanyType in objCompanyTypeList)
                        {
                            cmd.Parameters["@CompanyType_Guid"].Value = objCompanyType.ID;
                            cmd.ExecuteNonQuery();
                            iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                            if (iRes != 0) { break; }
                        }

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


    } // CCompanyType
    #endregion
    
    #region Класс "Форма собственности"
    /// <summary>
    /// Класс "Форма собственности"
    /// </summary>
    public class CStateTypeCompany : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Признак активности
        /// </summary>
        private System.Boolean m_IsActive;
        /// <summary>
        /// Признак активности
        /// </summary>
        [DisplayName("Активен")]
        [Description("Признак активности")]
        [Category("2. Дополнительно")]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public System.Boolean IsActive
        {
            get { return m_IsActive; }
            set { m_IsActive = value; }
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

        #region Конструкторы
        public CStateTypeCompany() : base()
        {
        }
        public CStateTypeCompany(System.Guid uuidId, System.String strName, System.String strShortName,  System.Boolean bIsActive)
        {
            ID = uuidId;
            Name = strName;
            m_IsActive = bIsActive;
            m_strShortName = strShortName;
        }
        #endregion

        #region Список объектов
        public static List<CStateTypeCompany> GetStateTypeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CStateTypeCompany> objList = new List<CStateTypeCompany>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetCustomerStateType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CStateTypeCompany((System.Guid)rs["CustomerStateType_Guid"],
                            (System.String)rs["CustomerStateType_Name"],
                            (System.String)rs["CustomerStateType_ShortName"],
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
        #endregion

        #region Перегруженный toString
        public override string ToString()
        {
            return (this.Name + "\t" + "[" + this.ShortName + "]");
            ;
        }
        #endregion
    }
     #endregion
}



