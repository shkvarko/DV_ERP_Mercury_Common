using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace ERP_Mercury.Common
{

    public class CEarning: CBusinessObject
    {
        #region Свойства
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
        /// Клиент
        /// </summary>
        private CCustomer m_objCustomer;
        /// <summary>
        /// Клиент
        /// </summary>
        public CCustomer Customer
        {
            get { return m_objCustomer; }
            set { m_objCustomer = value; }
        }

        /// <summary>
        /// Валюта
        /// </summary>
        private CCurrency m_objCurrency;
        /// <summary>
        /// Валюта
        /// </summary>
        public CCurrency Currency
        {
            get { return m_objCurrency; }
            set { m_objCurrency = value; }
        }

        /// <summary>
        /// Дата
        /// </summary>
        private System.DateTime m_dDate; //- // по-моему поле всё-таки должно быть
        /// <summary>
        /// Дата
        /// </summary>
        public System.DateTime Date
        {
            get { return m_dDate; }
            set { m_dDate = value; }
        }

        /// <summary>
        /// Номер документа
        /// </summary>
        private System.String m_strDocnom; 
        /// <summary>
        /// Номер документа
        /// </summary>
        public System.String DocNom
        {
            get { return m_strDocnom; }
            set { m_strDocnom = value; }
        }

        /// <summary>
        /// Код банка
        /// </summary>
        private System.String m_strCodeBank;
        /// <summary>
        /// Код банка
        /// </summary>
        public System.String CodeBank
        {
            get { return m_strCodeBank; }
            set { m_strCodeBank = value; }
        }

        /// <summary>
        /// Номер расчетного счета
        /// </summary>
        private System.String m_strAccountNumber;
        /// <summary>
        /// Номер расчетного счета
        /// </summary>
        public System.String AccountNumber
        {
            get { return ( (Account == null) ? "" : Account.AccountNumber ); }
            set { m_strAccountNumber = value; }
        }

        /// <summary>
        /// Сумма платежа
        /// </summary>
        private System.Decimal m_dValue;
        /// <summary>
        /// Сумма платежа
        /// </summary>
        public System.Decimal Value
        {
            get { return m_dValue; }
            set { m_dValue = value; }
        }

        /// <summary>
        /// Сумма расходов
        /// </summary>
        private System.Decimal m_dExpense;
        /// <summary>
        /// Сумма расходов
        /// </summary>
        public System.Decimal Expense
        {
            get { return m_dExpense; }
            set { m_dExpense = value; }
        }

        /// <summary>
        /// Сальдо
        /// </summary>
        private System.Decimal m_dSaldo;
        /// <summary>
        /// Сальдо
        /// </summary>
        public System.Decimal Saldo
        {
            get { return m_dSaldo; }
            set { m_dSaldo = value; }
        }

        /// <summary>
        /// Компания
        /// </summary>
        private CCompany m_objCompany;
        /// <summary>
        /// Компания
        /// </summary>
        public CCompany Company
        {
            get { return m_objCompany; }
            set { m_objCompany = value; }
        }

        /// <summary>
        /// Курс ценообразования
        /// </summary>
        private System.Decimal m_dCurRate;
        /// <summary>
        ///  Курс ценообразования
        /// </summary>
        public System.Decimal CurRate
        {
            get { return m_dCurRate; }
            set { m_dCurRate = value; }
        }

        /// <summary>
        /// Сумма в EUR
        /// </summary>
        private System.Decimal m_dCurValue;
        /// <summary>
        ///  Сумма в EUR
        /// </summary>
        public System.Decimal CurValue
        {
            get { return m_dCurValue; }
            set { m_dCurValue = value; }
        }

        /// <summary>
        /// Текст в произвольной форме с описание плательщика
        /// </summary>
        private System.String m_strCustomrText;
        /// <summary>
        /// Текст в произвольной форме с описание плательщика
        /// </summary>
        public System.String CustomrText
        {
            get { return m_strCustomrText; }
            set { m_strCustomrText = value; }
        }

        /// <summary>
        /// Текст в произвольной форме с описание назначения платежа
        /// </summary>
        private System.String m_strDetailsPayment;
        /// <summary>
        /// Текст в произвольной форме с описание назначения платежа
        /// </summary>
        public System.String DetailsPayment
        {
            get { return m_strDetailsPayment; }
            set { m_strDetailsPayment = value; }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        /// <summary>
        /// Список курсов валют
        /// </summary>
        private List<CEarning> m_objEarningList;
        /// <summary>
        /// Список курсов валют
        /// </summary>
        public List<CEarning> EarningList
        {
            get { return m_objEarningList; }
            set { m_objEarningList = value; }
        }

        private List<CEarning> m_objEarningForDeleteList;
        public List<CEarning> EarningForDeleteList
        {
            get { return m_objEarningForDeleteList; }
            set { m_objEarningForDeleteList = value; }
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

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
        /// Проект-источник
        /// </summary>
        public CBudgetProject BudgetProjectSrc { get; set; }
        /// <summary>
        /// Проект-назначение
        /// </summary>
        public CBudgetProject BudgetProjectDst { get; set; }
        /// <summary>
        /// Проект-назначение
        /// </summary>
        public System.String BudgetProjectDstName { get { return ((BudgetProjectDst == null) ? "" : BudgetProjectDst.Name); } }
        /// <summary>
        /// Компания-плательщик
        /// </summary>
        public CCompany CompanyPayer { get; set; }
        /// <summary>
        /// Дочернее подразделение
        /// </summary>
        public CChildDepart ChildDepart { get; set; }
        /// <summary>
        /// Счёт из плана счетов
        /// </summary>
        public CAccountPlan AccountPlan { get; set; }
        /// <summary>
        /// Счёт из плана счетов
        /// </summary>
        public System.String AccountPlanName { get { return ((AccountPlan == null) ? "" : AccountPlan.FullName); } }
        /// <summary>
        /// Форма оплаты
        /// </summary>
        public CPaymentType PaymentType { get; set; }
        /// <summary>
        /// Признак "Бонус"
        /// </summary>
        public System.Boolean IsBonusEarning { get; set; }
        /// <summary>
        /// Наименование плательщика
        /// </summary>
        public System.String CustomerName
        {
            get { return ((this.Customer == null) ? ERP_Mercury.Global.Consts.strCustomerNotIndefined : this.Customer.FullName); }
        }
        public System.String ChildDepartCode
        {
            get { return ((this.ChildDepart == null) ? "" : this.ChildDepart.Code); }
        }
        public System.String ChildDepartName
        {
            get { return ((this.ChildDepart == null) ? "" : this.ChildDepart.Name); }
        }
        public System.String CompanyCode
        {
            get { return ((this.Company == null) ? "" : this.Company.Abbr); }
        }
        public System.String CurrencyCode
        {
            get { return ((this.Currency == null) ? "" : this.Currency.CurrencyAbbr); }
        }
        public System.String BankName
        {
            get { return ((this.Account == null) ? "" : ((this.Account.Bank == null) ? "" : this.Account.Bank.Name)); }
        }
        /// <summary>
        /// Расчётный счёт
        /// </summary>
        public CAccount Account { get; set; }
        /// <summary>
        /// Номер группы, в которую входит платёж
        /// </summary>
        public System.Int32 GroupKeyId { get; set; }
        /// <summary>
        /// Признак "Платёж включён в ручную разноску"
        /// </summary>
        public System.Boolean IncludeInManualPay { get; set; }
        /// <summary>
        /// Признак "Транзитная проводка"
        /// </summary>
        public System.Boolean IsTransitEarning
        {
            get
            {
                return (((BudgetProjectSrc != null) && (BudgetProjectSrc.ID.CompareTo(System.Guid.Empty) != 0) &&
                    (CompanyPayer != null) && (CompanyPayer.ID.CompareTo(System.Guid.Empty) != 0)));
            }
        }
        /// <summary>
        /// Вид оплаты
        /// </summary>
        public CEarningType EarningType { get; set; }
        /// <summary>
        /// Вид оплаты (наименование)
        /// </summary>
        public System.String EarningTypeName
        {
            get { return ((this.EarningType == null) ? System.String.Empty : this.EarningType.Name); }
        }
        /// <summary>
        /// Вид оплаты (код)
        /// </summary>
        public System.Int32 EarningType_Id
        {
            get { return ((this.EarningType == null) ? -1 : this.EarningType.EarningTypeId); }
        }

        #endregion

        # region Конструктор
        public CEarning() : base()
        {
            m_IbID = 0;
            m_objCustomer = null;
            m_objCurrency = null;
            m_dDate = DateTime.MinValue;
            m_strDocnom = "";
            m_strCodeBank = "";
            m_strAccountNumber = "";
            m_dValue = 0;
            m_dExpense = 0;
            m_dSaldo = 0;
            m_objCompany = null;
            m_dCurRate = 0;
            m_dCurValue = 0;
            m_strCustomrText = "";
            m_strDetailsPayment = "";
            m_objAccountList = null;
            BudgetProjectSrc = null;
            BudgetProjectDst = null;
            CompanyPayer = null;
            ChildDepart = null;
            AccountPlan = null;
            PaymentType = null;
            IsBonusEarning = false;
            Account = null;
            GroupKeyId = 0;
            IncludeInManualPay = false;
            EarningType = null;
        }

        public CEarning(System.Guid uuidID, System.Int32 IbID, CCustomer objCustomer, CCurrency objCurrency, System.DateTime dDate,
            System.String strDocnom, System.String strBank, System.String strAccountNumber, System.Decimal dValue, System.Decimal dExpense,
            System.Decimal dSaldo, CCompany objCompany, System.Decimal dCurRate, System.Decimal dCurValue, System.String strEarningList, System.String strEarningForDeleteList)
        {
            ID = uuidID;
            m_IbID = IbID;
            m_objCustomer = objCustomer;
            m_objCurrency = objCurrency;
            m_dDate = dDate;
            m_strDocnom = strDocnom;
            m_strCodeBank = strBank;
            m_strAccountNumber = strAccountNumber;
            m_dValue = dValue;
            m_dExpense = dExpense;
            m_dSaldo = dSaldo;
            m_objCompany = objCompany;
            m_dCurRate = dCurRate;
            m_dCurValue = dCurValue;
            m_strCustomrText = strEarningList;
            m_strDetailsPayment = strEarningForDeleteList;
            m_objAccountList = null;
            m_objAccountList = null;
            BudgetProjectSrc = null;
            BudgetProjectDst = null;
            CompanyPayer = null;
            ChildDepart = null;
            AccountPlan = null;
            PaymentType = null;
            IsBonusEarning = false;
            Account = null;
            GroupKeyId = 0;
            IncludeInManualPay = false;
            EarningType = null;
        }

        public CEarning(CEarning objEa)
            : base()
        {
            this.ID = objEa.ID;
            this.Name = objEa.Name;
            this.m_IbID = objEa.m_IbID;
            this.m_objCustomer = objEa.m_objCustomer;

            this.m_objCustomer.ID = objEa.m_objCustomer.ID;
            this.m_objCustomer.ShortName = objEa.m_objCustomer.ShortName;

            this.m_objCurrency = objEa.m_objCurrency;
            this.m_dDate = objEa.m_dDate;
            this.m_strDocnom = objEa.m_strDocnom;
            this.m_strCodeBank = objEa.m_strCodeBank;
            this.m_strAccountNumber = objEa.m_strAccountNumber;
            this.m_dValue = objEa.m_dValue;
            this.m_dExpense = objEa.m_dExpense;
            this.m_dSaldo = objEa.m_dSaldo;
            this.m_objCompany =  objEa.m_objCompany;
            this.m_dCurRate = objEa.m_dCurRate;
            this.m_dCurValue = objEa.m_dCurValue;
            this.m_strCustomrText = objEa.m_strCustomrText;
            this.m_strDetailsPayment = objEa.m_strDetailsPayment;
            this.m_objAccountList = objEa.m_objAccountList;
            this.m_objEarningList = objEa.m_objEarningList;
            this.Account = objEa.Account;

            m_objAccountList = null;
            BudgetProjectSrc = null;
            BudgetProjectDst = null;
            CompanyPayer = null;
            ChildDepart = null;
            AccountPlan = null;
            PaymentType = null;
            IsBonusEarning = false;
            GroupKeyId = 0;
            IncludeInManualPay = false;
            EarningType = null;
        }

        #endregion

        public void Clone(CEarning objEa)
        {
            this.ID = objEa.ID;
            this.Name = objEa.Name;
            this.m_IbID = objEa.m_IbID;
            //this.m_objCustomer = objEa.m_objCustomer;

            this.m_objCustomer.ID = objEa.m_objCustomer.ID;
            this.m_objCustomer.ShortName = objEa.m_objCustomer.ShortName;

            this.m_objCurrency = objEa.m_objCurrency;
            this.m_dDate = objEa.m_dDate;
            this.m_strDocnom = objEa.m_strDocnom;
            this.m_strCodeBank = objEa.m_strCodeBank;
            this.m_strAccountNumber = objEa.m_strAccountNumber;
            this.m_dValue = objEa.m_dValue;
            this.m_dExpense = objEa.m_dExpense;
            this.m_dSaldo = objEa.m_dSaldo;
            this.m_objCompany = objEa.m_objCompany;
            this.m_dCurRate = objEa.m_dCurRate;
            this.m_dCurValue = objEa.m_dCurValue;
            this.m_strCustomrText = objEa.m_strCustomrText;
            this.m_strDetailsPayment = objEa.m_strDetailsPayment;
            this.m_objAccountList = objEa.m_objAccountList;

            this.BudgetProjectSrc = objEa.BudgetProjectSrc;
            this.BudgetProjectDst = objEa.BudgetProjectDst;
            this.CompanyPayer = objEa.CompanyPayer;
            this.ChildDepart = objEa.ChildDepart;
            this.AccountPlan = objEa.AccountPlan;
            this.PaymentType = objEa.PaymentType;
            this.IsBonusEarning = objEa.IsBonusEarning;
            this.Account = objEa.Account;
            this.IncludeInManualPay = objEa.IncludeInManualPay;
            this.EarningType = objEa.EarningType;

            (this.m_objEarningList).Add(new CEarning(objEa));
        }

        #region Список объектов
        /// <summary>
        /// Возвращает список выписок
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список компаний</returns>
        public static List<CEarning> GetEarningList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, System.DateTime dtBegin, System.DateTime dtEnd, System.Guid guidCompany)
        {
            List<CEarning> objList = new List<CEarning>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetEarningList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_DateBegin", System.Data.DbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_DateEnd", System.Data.DbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_guidCompany", System.Data.DbType.Guid));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Earning_DateBegin"].Value = dtBegin; //sBegin; // Convert.ToDateTime("18.08.2011");  
                cmd.Parameters["@Earning_DateEnd"].Value = dtEnd; //.GetDateTimeFormats(); //sEnd; //Convert.ToDateTime("18.08.2011"); 
                cmd.Parameters["@Earning_guidCompany"].Value = guidCompany;
                
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CEarning objEarning = null;
                    while (rs.Read())
                    {

                        try
                        {
                            objEarning = new CEarning(
                                                    (System.Guid)rs["Earning_Guid"],
                                                    ((rs["Earning_Id"] == System.DBNull.Value) ? 0 : (System.Int32)rs["Earning_Id"]),
                                                    ((rs["Customer_Guid"] != System.DBNull.Value) ? new CCustomer((System.Guid)rs["Customer_Guid"], 0, "",
                                                                    (System.String)rs["Customer_Name"], "", "", "", "", null, null) : new CCustomer() { ShortName = "--- Клиент не найден ---", FullName = "--- Клиент не найден ---" }),
                                                    new CCurrency((System.Guid)rs["Currency_Guid"],
                                                                    (System.String)rs["Currency_Name"],
                                                                    (System.String)rs["Currency_Abbr"], ""),
                                                    (System.DateTime)rs["Earning_Date"],
                                                    (System.String)rs["Earning_DocNum"],
                                                    (System.String)rs["Bank_Code"],
                                                    (System.String)rs["Earning_Account"],
                                                    (System.Decimal)rs["Earning_Value"],
                                                    (System.Decimal)rs["Earning_Expense"],
                                                    (System.Decimal)rs["Earning_Saldo"],
                                                    new CCompany((System.Guid)rs["Company_Guid"],
                                                                    (System.String)rs["Company_Name"],
                                                                    (System.String)rs["Company_Acronym"]),
                                                    ((rs["Earning_CurrencyRate"] != System.DBNull.Value) ? (System.Decimal)rs["Earning_CurrencyRate"] : 0),
                                                    ((rs["Earning_CurrencyValue"] != System.DBNull.Value) ? (System.Decimal)rs["Earning_CurrencyValue"] : 0),
                                                    ((rs["Earning_CustomerText"] != System.DBNull.Value) ? (System.String)rs["Earning_CustomerText"] : ""),
                                                    ((rs["Earning_DetailsPaymentText"] != System.DBNull.Value) ? (System.String)rs["Earning_DetailsPaymentText"] : "")
                                                    );
                            if (rs["Account_Guid"] != System.DBNull.Value)
                            {
                                objEarning.Account = new CAccount();
                                objEarning.Account.ID = (System.Guid)rs["Account_Guid"];
                                objEarning.Account.AccountNumber = ((rs["Account_Number"] != System.DBNull.Value) ? System.Convert.ToString(rs["Account_Number"]) : "");
                                objEarning.Account.Currency = (( rs["AccountViewCurrency_Giud"] != System.DBNull.Value ) ? new CCurrency() { ID = (System.Guid)rs["AccountViewCurrency_Giud"], CurrencyAbbr = (System.String)rs["AccountViewCurrency_Abbr"], CurrencyCode = (System.String)rs["AccountViewCurrency_Code"] } : null);
                                objEarning.Account.AccountType = ((rs["AccountViewAccountType_Guid"] != System.DBNull.Value) ? new CAccountType() { ID = (System.Guid)rs["AccountViewAccountType_Guid"], Name = (System.String)rs["AccountViewAccountType_Name"] } : null);
                                if( rs["AccountViewBank_Guid"] != System.DBNull.Value )
                                {
                                    objEarning.Account.Bank = new CBank();

                                    objEarning.Account.Bank.ID = ( ( rs["AccountViewBank_Guid"] != System.DBNull.Value ) ? (System.Guid)rs["AccountViewBank_Guid"] : System.Guid.Empty );
                                    objEarning.Account.Bank.Name =( ( rs["AccountViewBank_Name"] != System.DBNull.Value ) ? (System.String)rs["AccountViewBank_Name"] : "" );
                                    objEarning.Account.Bank.Code = ( ( rs["AccountViewBank_Code"] != System.DBNull.Value ) ? (System.String)rs["AccountViewBank_Code"] : "" );
                                    objEarning.Account.Bank.UNN = (( rs["AccountViewBank_UNN"] != System.DBNull.Value ) ? (System.String)rs["AccountViewBank_UNN"] : "" );
                                    objEarning.Account.Bank.MFO = ((rs["AccountViewBank_MFO"] != System.DBNull.Value) ? (System.String)rs["AccountViewBank_MFO"] : "");
                                }

                            }


                            objList.Add(objEarning);
                        }
                        catch (System.Exception g)
                        {
                            DevExpress.XtraEditors.XtraMessageBox.Show(
                            "Платёж с кодом: " + ((rs["Earning_Id"] == System.DBNull.Value) ? 0 : (System.Int32)rs["Earning_Id"]).ToString() +  ".\n\nТекст ошибки: " + g.Message, "Внимание",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        }

                       // objList.Last<CCompany>().DefCustomerId = ((rs["DefCustomerId"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["DefCustomerId"]));
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
        #endregion

        #region Валидация
        /// <summary>
        /// Проверка свойств контакта перед сохранением
        /// </summary>
        /// <param name="strErr">текст с ошибкой</param>
        /// <returns>true - все свойства корректны; false - ошибка</returns>
        public System.Boolean IsAllParametersValid(System.Boolean bValidCustomer,ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (bValidCustomer)
                {
                    if (this.Customer.ID == System.Guid.Empty)
                    {
                        strErr = "Не указан клиент !";
                        return bRet;
                    }
                }


                if (this.Currency.ID == System.Guid.Empty)
                {
                    strErr = "Не указана валюта !";
                    return bRet;
                }

                if (this.Date == System.DateTime.MinValue || this.Date==null  )
                {
                    strErr = "Не указана дата !";
                    return bRet;
                }
                
                if ( System.String.IsNullOrEmpty(this.DocNom))
                {
                    strErr = "Не указан номер документа !";
                    return bRet;
                }

                if (System.String.IsNullOrEmpty(this.CodeBank))
                {
                    strErr = "Не указан код банка !";
                    return bRet;
                }

                if (System.String.IsNullOrEmpty(this.AccountNumber))
                {
                    strErr = "Не указан р/с !";
                    return bRet;
                }

                if (this.Value ==0)
                {
                    strErr = "Платёж равен 0 !";
                    return bRet;
                }

                if (this.Company.ID == System.Guid.Empty)
                {
                    strErr = "Не указана компания !";
                    return bRet;
                }

                if (this.CurRate == 0)
                {
                    strErr = "Курс ценообразования равен 0 !";
                    return bRet;
                }

                if (this.CurValue == 0)
                {
                    strErr = "Курс в EUR равен 0 !";
                    return bRet;
                }
                
                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr = "Ошибка проверки свойств. Текст ошибки: " + f.Message;
            }
            return bRet;
        }
        #endregion

        #region Сохранение в БД 
        ///// <summary>
        ///// Сохраняет в БД список выписок
        ///// </summary>
        ///// <param name="objEarningList">список выписок для сохранения</param>
        ///// <param name="objEarningForDeleteList">список выписок для удаления</param>
        ///// <param name="objProfile">профайл</param>
        ///// <param name="cmdSQL">SQL-команда</param>
        ///// <param name="strErr">сообщение об ошибке</param>
        ///// <returns>true - удачное завершение; false - ошибка</returns>
        //public static System.Boolean SaveEarningList(List<CEarning> objEarningList, List<CEarning> objEarningForDeleteList, UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        //{
        //    if ((objEarningList == null) && (objEarningForDeleteList == null)) { return true; }
        //    System.Boolean bRet = false;
        //    System.Data.SqlClient.SqlConnection DBConnection = null;
        //    System.Data.SqlClient.SqlCommand cmd = null;
        //    System.Data.SqlClient.SqlTransaction DBTransaction = null;
        //    try
        //    {
        //        // для начала проверим, что нам пришло в списке
        //        if ((objEarningList != null) && (objEarningList.Count > 0))
        //        {
        //            System.Boolean bIsAllValid = true;
        //            foreach (CEarning objItem in objEarningList)
        //            {
        //                if (objItem.IsAllParametersValid(ref strErr) == false)
        //                {
        //                    bIsAllValid = false;
        //                    break;
        //                }
        //            }
        //            if (bIsAllValid == false)
        //            {
        //                return bRet;
        //            }
        //        }

        //        if (cmdSQL == null)
        //        {
        //            DBConnection = objProfile.GetDBSource();
        //            if (DBConnection == null)
        //            {
        //                strErr = "Не удалось получить соединение с базой данных.";
        //                return bRet;
        //            }
        //            DBTransaction = DBConnection.BeginTransaction();
        //            cmd = new System.Data.SqlClient.SqlCommand();
        //            cmd.Connection = DBConnection;
        //            cmd.Transaction = DBTransaction;
        //            cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //        }
        //        else
        //        {
        //            cmd = cmdSQL;
        //            cmd.Parameters.Clear();
        //        }

        //        System.Int32 iRes = 0;
        //        if ((objEarningForDeleteList != null) && (objEarningForDeleteList.Count > 0))
        //        {
        //            foreach (CEarning objEarning in objEarningForDeleteList)
        //            {
        //                if (objEarning.ID.CompareTo(System.Guid.Empty) == 0) { continue; }
        //                // Раскоментить, если будет написан метод Remove 27.01.2012
        //                //iRes = (objEarning.Remove(objProfile, cmd, ref strErr, true) == true) ? 0 : 1;
        //                if (iRes != 0) { break; }
        //            }
        //        }

        //        if (iRes == 0)
        //        {
        //            if ((objEarningList != null) && (objEarningList.Count > 0))
        //            {
        //                // теперь в цикле добавим в БД каждый член из списка
        //                foreach (CEarning objEarning in objEarningList)
        //                {
        //                    if (objEarning.ID.CompareTo(System.Guid.Empty) == 0)
        //                    {
        //                        // ADD
        //                        iRes = (objEarning.Add(objProfile, null, ref strErr) == true) ? 0 : -1; // ориганальный вариант
        //                        //iRes = ( MyAdd(objEarning, objProfile, ref strErr/*, object sender, DoWorkEventArgs e*/) == true) ? 0 : -1;
        //                        //iRes = (MyAdd(sender, DoWorkEventArgs e) == true) ? 0 : -1; 
        //                    }
        //                    else
        //                    {
        //                        // UPDATE
        //                        //iRes = (objEarning.Update(objProfile, cmd, ref strErr) == true) ? 0 : -1;
        //                        // вставить прогресс бар где-то здесь
        //                    }

        //                    if (iRes != 0) { break; }
        //                }
        //            }
        //        }

        //        if (cmdSQL == null)
        //        {
        //            if (iRes == 0)
        //            {
        //                // подтверждаем транзакцию
        //                DBTransaction.Commit();
        //            }
        //            else
        //            {
        //                // откатываем транзакцию
        //                DBTransaction.Rollback();
        //            }
        //            DBConnection.Close();
        //        }

        //        bRet = (iRes == 0);
        //    }
        //    catch (System.Exception f)
        //    {
        //        if (cmdSQL == null)
        //        {
        //            DBTransaction.Rollback();
        //        }
        //        strErr = f.Message;
        //    }
        //    finally
        //    {
        //        if (DBConnection != null)
        //        {
        //            DBConnection.Close();
        //        }
        //    }
        //    return bRet;
        //}
        #endregion

        #region ADD
        /// <summary>
        /// Добавить запись в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Add(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, System.Int32 iKey, ref System.String strErr)
        {
            System.Boolean bRet = false, EmptyCustomer = false;
            System.Int32 iRes=0;
            System.String strErrorMain="";
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null; // ориганальный вариант
            System.Boolean bSaveInIB = false;
            
            try
            {
                if (this.IsAllParametersValid(false, ref strErr) == false)
                {
                    if (strErr == "Не указан клиент !")
                    {
                        EmptyCustomer = true;
                    }
                    else
                    {
                        return bRet;
                    }
                    //return bRet;
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

                // Добавляем № р/с
                if ((this.AccountList != null) && (this.AccountList.Count > 0) && (EmptyCustomer==false))
                {
                    foreach (CAccount objAccount in this.AccountList) { objAccount.ID = System.Guid.Empty; }
                    iRes = (CAccount.SaveAccountList(this.AccountList, null, EnumObject.Customer, this.Customer.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                    cmd.Parameters.Clear();
                }

                if (iRes == 0)
                {
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddEarning]", objProfile.GetOptionsDllDBName());

                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));

                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CustomerGuid", System.Data.DbType.Guid));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CurrencyGuid", System.Data.DbType.Guid));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_Date", System.Data.DbType.DateTime));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_DocNum", System.Data.DbType.String));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_BankCode", System.Data.DbType.String));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_Account", System.Data.DbType.String));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_Value", System.Data.DbType.Decimal));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CompanyGuid", System.Data.DbType.Guid));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CurrencyRate", System.Data.DbType.Decimal));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CurrencyValue", System.Data.DbType.Decimal));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CustomerText", System.Data.DbType.String));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_DetailsPaymentText", System.Data.DbType.String));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_iKey", System.Data.DbType.Int32));

                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));//**
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                    if (this.Customer.ID == System.Guid.Empty) { cmd.Parameters["@Earning_CustomerGuid"].Value = System.DBNull.Value;}
                    else { cmd.Parameters["@Earning_CustomerGuid"].Value = this.Customer.ID;}
                    //cmd.Parameters["@Earning_CustomerGuid"].Value = this.Customer.ID;
                    cmd.Parameters["@Earning_CurrencyGuid"].Value = this.Currency.ID;
                    cmd.Parameters["@Earning_Date"].Value = this.Date;
                    cmd.Parameters["@Earning_DocNum"].Value = this.DocNom;
                    cmd.Parameters["@Earning_BankCode"].Value = this.CodeBank;
                    cmd.Parameters["@Earning_Account"].Value = this.AccountNumber;
                    cmd.Parameters["@Earning_Value"].Value = this.Value;

                    cmd.Parameters["@Earning_CompanyGuid"].Value = this.Company.ID; 
                    cmd.Parameters["@Earning_CurrencyRate"].Value = this.CurRate;
                    cmd.Parameters["@Earning_CurrencyValue"].Value = this.CurValue;
                    cmd.Parameters["@Earning_CustomerText"].Value = this.CustomrText;
                    cmd.Parameters["@Earning_DetailsPaymentText"].Value = this.DetailsPayment;
                    cmd.Parameters["@Earning_iKey"].Value = iKey;

                    if( this.AccountPlan != null )
                    {
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AccountPlan_Guid", System.Data.DbType.Guid));
                        cmd.Parameters["@AccountPlan_Guid"].Value = this.AccountPlan.ID;
                    }
                    if (this.BudgetProjectDst != null)
                    {
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BudgetProjectDST_Guid", System.Data.DbType.Guid));
                        cmd.Parameters["@BudgetProjectDST_Guid"].Value = this.BudgetProjectDst.ID;
                    }
                    cmd.ExecuteNonQuery();
                    iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                }
                

                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    this.ID = (System.Guid)cmd.Parameters["@Earning_Guid"].Value; // Полученный после выполнения, GUID 
                    
                    /*
                    // Добавление р/с выполняется перед добавление проводки (банк выписки)
                    if (iRes == 0)
                    {
                        if ((this.AccountList != null) && (this.AccountList.Count > 0))
                        {
                            foreach (CAccount objAccount in this.AccountList) { objAccount.ID = System.Guid.Empty; }
                            iRes = (CAccount.SaveAccountList(this.AccountList, null, EnumObject.Customer , this.Customer.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1; 
                        }
                    }
                    */
                       
                    
                    if (iRes == 0)
                    {
                        // Add в IB
                        cmd.Parameters.Clear();
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddEarningToIB]", objProfile.GetOptionsDllDBName());
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE",System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false,((System.Byte) (0)),((System.Byte) (0)), "",System.Data.DataRowVersion.Current,null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_Id", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_Guid",System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM",System.Data.SqlDbType.Int, 8,System.Data.ParameterDirection.Output,false, ((System.Byte) (0)),((System.Byte) (0)), "",System.Data.DataRowVersion.Current,null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES",System.Data.SqlDbType.NVarChar, 4000));
                        cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                        cmd.Parameters["@Earning_Guid"].Value = this.ID;
                        cmd.ExecuteNonQuery();

                        iRes = (System.Int32) cmd.Parameters["@RETURN_VALUE"].Value; // ориганальный вариант написания

                        if (cmd.Parameters["@ERROR_MES"].Value != System.DBNull.Value)
                        {
                            strErr = (System.String) cmd.Parameters["@ERROR_MES"].Value;
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
                    // откатываем транзакцию
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    strErrorMain = strErr;
                    /*DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств валюты.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);*/

                    // метод который удалил бы из БД все записи о проводках, которые были 
                    // добалены до ошибки, в текущей сесии. 

                    DeleteEarningByGuidKey(objProfile, cmd, iKey, ref strErr);
                    strErr = strErrorMain;

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
                "Не удалось добавить банковские выписки.\n\nТекст ошибки: " + f.Message, "Внимание",
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
        /// Удаляет из базы данных выписки
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean DeleteEarningByGuidKey(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, System.Int32 iKey, ref System.String strErr)
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
                    //cmd.CommandTimeout = iCommandTimeOutForIB;
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
                System.Int32 iRes = DeleteEarningFromIB(objProfile, cmd, iKey, ref strErr);

                // ** временно раскоментил (нужно закоментить)
                //System.Int32 iRes = 0;

                if (iRes == 0) // если всё OK
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteEarningGroup]", objProfile.GetOptionsDllDBName());
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_iKey", System.Data.SqlDbType.Int));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters["@Earning_iKey"].Value = iKey;
                    cmd.ExecuteNonQuery();
                    iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

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
        private System.Int32 DeleteEarningFromIB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, System.Int32 iKey, ref System.String strErr)
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
                    cmdSQL.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteEarningFromIB]", objProfile.GetOptionsDllDBName()); ;
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_iKey", System.Data.SqlDbType.Int ));
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmdSQL.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                    cmdSQL.Parameters["@Earning_iKey"].Value = iKey;
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
        
    }



    public class CEaL : CBusinessObject
    {
        # region свойства
        /// <summary>
        /// Список 
        /// </summary>
        private List<CEarning> m_objEarningList;
        /// <summary>
        /// Список 
        /// </summary>
        public List<CEarning> EarningList
        {
            get { return m_objEarningList; }
            set { m_objEarningList = value; }
        }
        #endregion

        #region Конструктор
        public CEaL()
            : base()
        {
            //m_objEarningList = null;
            m_objEarningList = new List<CEarning>();
        }
        /*
        public CEaL(CEarning objEa)
        {
            m_objEarningList = new List<CEarning>();
            m_objEarningList.Add(objEa);
        }*/
        #endregion

        public /*System.Boolean*/ void Add(CEarning objEa)
        {
            m_objEarningList.Add(objEa);

        }
    }

    public static class CEarningDataBaseModel
    {
        #region Добавить объект в базу данных
        /// <summary>
        /// Проверка значений полей объекта перед сохранением в базе данных
        /// </summary>
        /// <param name="Earning_CustomerGuid"></param>
        /// <param name="Earning_CurrencyGuid"></param>
        /// <param name="Earning_Date"></param>
        /// <param name="Earning_DocNum"></param>
        /// <param name="Earning_AccountGuid"></param>
        /// <param name="Earning_Value"></param>
        /// <param name="Earning_CompanyGuid"></param>
        /// <param name="Earning_CurrencyRate"></param>
        /// <param name="Earning_CurrencyValue"></param>
        /// <param name="Earning_CustomerText"></param>
        /// <param name="Earning_DetailsPaymentText"></param>
        /// <param name="Earning_iKey"></param>
        /// <param name="BudgetProjectSRC_Guid"></param>
        /// <param name="BudgetProjectDST_Guid"></param>
        /// <param name="CompanyPayer_Guid"></param>
        /// <param name="ChildDepart_Guid"></param>
        /// <param name="AccountPlan_Guid"></param>
        /// <param name="PaymentType_Guid"></param>
        /// <param name="Earning_IsBonus"></param>
        /// <param name="EarningType_Guid"></param>
        /// <param name="strErr"></param>
        /// <returns></returns>
        public static System.Boolean IsAllParametersValid(System.Guid Earning_CustomerGuid, System.Guid Earning_CurrencyGuid, 
            System.DateTime Earning_Date, System.String Earning_DocNum,
            System.Guid Earning_AccountGuid, System.Decimal Earning_Value, System.Guid Earning_CompanyGuid, 
            System.Decimal Earning_CurrencyRate, System.Decimal Earning_CurrencyValue, System.String Earning_CustomerText,
            System.String Earning_DetailsPaymentText, System.Int32 Earning_iKey, System.Guid BudgetProjectSRC_Guid, 
            System.Guid BudgetProjectDST_Guid, System.Guid CompanyPayer_Guid, System.Guid ChildDepart_Guid,
            System.Guid AccountPlan_Guid, System.Guid PaymentType_Guid, System.Boolean Earning_IsBonus, System.Guid EarningType_Guid,
            ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Int32 iWarningCount = 0;
            try
            {
                if (Earning_CurrencyGuid.Equals(System.Guid.Empty) == true)
                {
                    strErr += ("\nНеобходимо указать валюту платежа!");
                    iWarningCount++;
                }
                if (BudgetProjectDST_Guid.CompareTo(System.Guid.Empty) == 0)
                {
                    strErr += ("\nНеобходимо указать проект-назначение!");
                    iWarningCount++;
                }
                if (AccountPlan_Guid.CompareTo(System.Guid.Empty) == 0)
                {
                    strErr += ("\nНеобходимо указать счет из плана счетов!");
                    iWarningCount++;
                }
                if (Earning_Date.CompareTo(System.DateTime.MinValue) == 0)
                {
                    strErr += ("\nНеобходимо указать дату платежа!");
                    iWarningCount++;
                }
                if ( Earning_AccountGuid.Equals(System.Guid.Empty) == true)
                {
                    strErr += ("\nНеобходимо указать расчётный счёт!");
                    iWarningCount++;
                }
                if (Earning_Value <= 0)
                {
                    strErr += ("\nСумма платежа должна быть больше нуля!");
                    iWarningCount++;
                }
                if (Earning_CompanyGuid.Equals(System.Guid.Empty) == true)
                {
                    strErr += ("\nНеобходимо указать компанию-получателя средств!");
                    iWarningCount++;
                }
                if (PaymentType_Guid.Equals(System.Guid.Empty) == true)
                {
                    strErr += ("\nНеобходимо указать форму оплаты!");
                    iWarningCount++;
                }
                if (EarningType_Guid.Equals(System.Guid.Empty) == true)
                {
                    strErr += ("\nНеобходимо указать вид оплаты!");
                    iWarningCount++;
                }

                bRet = (iWarningCount == 0);
            }
            catch (System.Exception f)
            {
                strErr += (String.Format("Ошибка проверки свойств объекта 'платёж'. Текст ошибки: {0}", f.Message));
            }
            return bRet;
        }
        /// <summary>
        /// Добавляет запись в базу данных
        /// </summary>
        /// <param name="Earning_CustomerGuid"></param>
        /// <param name="Earning_CurrencyGuid"></param>
        /// <param name="Earning_Date"></param>
        /// <param name="Earning_DocNum"></param>
        /// <param name="Earning_AccountGuid"></param>
        /// <param name="Earning_Value"></param>
        /// <param name="Earning_CompanyGuid"></param>
        /// <param name="Earning_CurrencyRate"></param>
        /// <param name="Earning_CurrencyValue"></param>
        /// <param name="Earning_CustomerText"></param>
        /// <param name="Earning_DetailsPaymentText"></param>
        /// <param name="Earning_iKey"></param>
        /// <param name="BudgetProjectSRC_Guid"></param>
        /// <param name="BudgetProjectDST_Guid"></param>
        /// <param name="CompanyPayer_Guid"></param>
        /// <param name="ChildDepart_Guid"></param>
        /// <param name="AccountPlan_Guid"></param>
        /// <param name="PaymentType_Guid"></param>
        /// <param name="Earning_IsBonus"></param>
        /// <param name="EarningType_Guid"></param>
        /// <param name="Earning_Guid"></param>
        /// <param name="objProfile"></param>
        /// <param name="strErr"></param>
        /// <returns></returns>
        public static System.Boolean AddNewObjectToDataBase(System.Guid Earning_CustomerGuid, System.Guid Earning_CurrencyGuid,
            System.DateTime Earning_Date, System.String Earning_DocNum,
            System.Guid Earning_AccountGuid, System.Decimal Earning_Value, System.Guid Earning_CompanyGuid,
            System.Decimal Earning_CurrencyRate, System.Decimal Earning_CurrencyValue, System.String Earning_CustomerText,
            System.String Earning_DetailsPaymentText, System.Int32 Earning_iKey, System.Guid BudgetProjectSRC_Guid,
            System.Guid BudgetProjectDST_Guid, System.Guid CompanyPayer_Guid, System.Guid ChildDepart_Guid,
            System.Guid AccountPlan_Guid, System.Guid PaymentType_Guid, System.Boolean Earning_IsBonus, System.Guid EarningType_Guid,
            ref System.Guid Earning_Guid,
            UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;

            if( IsAllParametersValid( Earning_CustomerGuid, Earning_CurrencyGuid, 
                Earning_Date, Earning_DocNum, 
                Earning_AccountGuid, Earning_Value, Earning_CompanyGuid, 
                Earning_CurrencyRate, Earning_CurrencyValue, Earning_CustomerText,
                Earning_DetailsPaymentText, Earning_iKey, BudgetProjectSRC_Guid, 
                BudgetProjectDST_Guid, CompanyPayer_Guid, ChildDepart_Guid, 
                AccountPlan_Guid, PaymentType_Guid, Earning_IsBonus, EarningType_Guid,  ref strErr) == false) 
            { return bRet; }

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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddEarningToSQLandIB]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_Guid", System.Data.SqlDbType.UniqueIdentifier) { Direction = System.Data.ParameterDirection.Output});
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CustomerGuid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CurrencyGuid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_Date", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_DocNum", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_AccountGuid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_Value", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CompanyGuid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CurrencyRate", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CurrencyValue", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CustomerText", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_DetailsPaymentText", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_iKey", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BudgetProjectSRC_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BudgetProjectDST_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyPayer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ChildDepart_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AccountPlan_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PaymentType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_IsBonus", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EarningType_Guid", System.Data.SqlDbType.UniqueIdentifier));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output});
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });

                if (Earning_CustomerGuid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@Earning_CustomerGuid"].IsNullable = true;
                    cmd.Parameters["@Earning_CustomerGuid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@Earning_CustomerGuid"].IsNullable = false;
                    cmd.Parameters["@Earning_CustomerGuid"].Value = Earning_CustomerGuid;
                }
                if (BudgetProjectSRC_Guid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@BudgetProjectSRC_Guid"].IsNullable = true;
                    cmd.Parameters["@BudgetProjectSRC_Guid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@BudgetProjectSRC_Guid"].IsNullable = false;
                    cmd.Parameters["@BudgetProjectSRC_Guid"].Value = BudgetProjectSRC_Guid;
                }
                if (BudgetProjectDST_Guid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@BudgetProjectDST_Guid"].IsNullable = true;
                    cmd.Parameters["@BudgetProjectDST_Guid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@BudgetProjectDST_Guid"].IsNullable = false;
                    cmd.Parameters["@BudgetProjectDST_Guid"].Value = BudgetProjectDST_Guid;
                }
                if (CompanyPayer_Guid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@CompanyPayer_Guid"].IsNullable = true;
                    cmd.Parameters["@CompanyPayer_Guid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@CompanyPayer_Guid"].IsNullable = false;
                    cmd.Parameters["@CompanyPayer_Guid"].Value = CompanyPayer_Guid;
                }
                if (ChildDepart_Guid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@ChildDepart_Guid"].IsNullable = true;
                    cmd.Parameters["@ChildDepart_Guid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@ChildDepart_Guid"].IsNullable = false;
                    cmd.Parameters["@ChildDepart_Guid"].Value = ChildDepart_Guid;
                }
                if (AccountPlan_Guid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@AccountPlan_Guid"].IsNullable = true;
                    cmd.Parameters["@AccountPlan_Guid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@AccountPlan_Guid"].IsNullable = false;
                    cmd.Parameters["@AccountPlan_Guid"].Value = AccountPlan_Guid;
                }
                if (PaymentType_Guid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@PaymentType_Guid"].IsNullable = true;
                    cmd.Parameters["@PaymentType_Guid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@PaymentType_Guid"].IsNullable = false;
                    cmd.Parameters["@PaymentType_Guid"].Value = PaymentType_Guid;
                }
                if (EarningType_Guid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@EarningType_Guid"].IsNullable = true;
                    cmd.Parameters["@EarningType_Guid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@EarningType_Guid"].IsNullable = false;
                    cmd.Parameters["@EarningType_Guid"].Value = EarningType_Guid;
                }

                cmd.Parameters["@Earning_CurrencyGuid"].Value = Earning_CurrencyGuid;
                cmd.Parameters["@Earning_Date"].Value = Earning_Date;
                cmd.Parameters["@Earning_DocNum"].Value = Earning_DocNum;
                cmd.Parameters["@Earning_AccountGuid"].Value = Earning_AccountGuid;
                cmd.Parameters["@Earning_Value"].Value = Earning_Value;
                cmd.Parameters["@Earning_CompanyGuid"].Value = Earning_CompanyGuid;
                cmd.Parameters["@Earning_CurrencyRate"].Value = Earning_CurrencyRate;
                cmd.Parameters["@Earning_CurrencyValue"].Value = Earning_CurrencyValue;
                cmd.Parameters["@Earning_CustomerText"].Value = Earning_CustomerText;
                cmd.Parameters["@Earning_DetailsPaymentText"].Value = Earning_DetailsPaymentText;
                cmd.Parameters["@Earning_iKey"].Value = Earning_iKey;
                cmd.Parameters["@Earning_IsBonus"].Value = Earning_IsBonus;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    Earning_Guid = (System.Guid)cmd.Parameters["@Earning_Guid"].Value;
                }
                else
                {
                    strErr += ((cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                strErr += ("Не удалось создать объект 'платёж'. Текст ошибки: " + f.Message);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }

        public static System.Boolean IsAllParametersValidInCEarning(System.Guid Earning_CustomerGuid, 
            System.Guid Earning_CurrencyGuid,
            System.DateTime Earning_Date, System.String Earning_DocNum,
            System.Guid Earning_AccountGuid, System.Decimal Earning_Value, System.Guid Earning_CompanyGuid,
            System.Decimal Earning_CurrencyRate, System.Decimal Earning_CurrencyValue, System.String Earning_CustomerText,
            System.String Earning_DetailsPaymentText, System.Int32 Earning_iKey, System.Guid BudgetProjectSRC_Guid,
            System.Guid BudgetProjectDST_Guid, System.Guid CompanyPayer_Guid, System.Guid ChildDepart_Guid,
            System.Guid AccountPlan_Guid, System.Guid PaymentType_Guid, System.Boolean Earning_IsBonus, System.Guid EarningType_Guid,
            ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Int32 iWarningCount = 0;
            try
            {
                if (ChildDepart_Guid.Equals(System.Guid.Empty) == true)
                {
                    strErr += ("\nНеобходимо указать дочернего клиента!");
                    iWarningCount++;
                }
                if (Earning_CustomerGuid.Equals(System.Guid.Empty) == true)
                {
                    strErr += ("\nНеобходимо указать клиента!");
                    iWarningCount++;
                }
                if (Earning_CurrencyGuid.Equals(System.Guid.Empty) == true)
                {
                    strErr += ("\nНеобходимо указать валюту платежа!");
                    iWarningCount++;
                }
                if (Earning_Date.CompareTo(System.DateTime.MinValue) == 0)
                {
                    strErr += ("\nНеобходимо указать дату платежа!");
                    iWarningCount++;
                }
                if (Earning_Value <= 0)
                {
                    strErr += ("\nСумма платежа должна быть больше нуля!");
                    iWarningCount++;
                }
                if (PaymentType_Guid.Equals(System.Guid.Empty) == true)
                {
                    strErr += ("\nНеобходимо указать форму оплаты!");
                    iWarningCount++;
                }
                if (EarningType_Guid.Equals(System.Guid.Empty) == true)
                {
                    strErr += ("\nНеобходимо указать вид оплаты!");
                    iWarningCount++;
                }
                if (BudgetProjectDST_Guid.CompareTo(System.Guid.Empty) == 0)
                {
                    strErr += ("\nНеобходимо указать проект-назначение!");
                    iWarningCount++;
                }
                if (AccountPlan_Guid.CompareTo(System.Guid.Empty) == 0)
                {
                    strErr += ("\nНеобходимо указать счет из плана счетов!");
                    iWarningCount++;
                }

                bRet = (iWarningCount == 0);
            }
            catch (System.Exception f)
            {
                strErr += (String.Format("Ошибка проверки свойств объекта 'платёж'. Текст ошибки: {0}", f.Message));
            }
            return bRet;
        }
        /// <summary>
        /// Добавляет запись в базу данных
        /// </summary>
        /// <param name="Earning_CustomerGuid"></param>
        /// <param name="Earning_CurrencyGuid"></param>
        /// <param name="Earning_Date"></param>
        /// <param name="Earning_DocNum"></param>
        /// <param name="Earning_AccountGuid"></param>
        /// <param name="Earning_Value"></param>
        /// <param name="Earning_CompanyGuid"></param>
        /// <param name="Earning_CurrencyRate"></param>
        /// <param name="Earning_CurrencyValue"></param>
        /// <param name="Earning_CustomerText"></param>
        /// <param name="Earning_DetailsPaymentText"></param>
        /// <param name="Earning_iKey"></param>
        /// <param name="BudgetProjectSRC_Guid"></param>
        /// <param name="BudgetProjectDST_Guid"></param>
        /// <param name="CompanyPayer_Guid"></param>
        /// <param name="ChildDepart_Guid"></param>
        /// <param name="AccountPlan_Guid"></param>
        /// <param name="PaymentType_Guid"></param>
        /// <param name="Earning_IsBonus"></param>
        /// <param name="EarningType_Guid"></param>
        /// <param name="Earning_Guid"></param>
        /// <param name="objProfile"></param>
        /// <param name="strErr"></param>
        /// <returns></returns>
        public static System.Boolean AddNewCEarningToDataBase(System.Guid Earning_CustomerGuid, System.Guid Earning_CurrencyGuid,
            System.DateTime Earning_Date, System.String Earning_DocNum,
            System.Guid Earning_AccountGuid, System.Decimal Earning_Value, System.Guid Earning_CompanyGuid,
            System.Decimal Earning_CurrencyRate, System.Decimal Earning_CurrencyValue, System.String Earning_CustomerText,
            System.String Earning_DetailsPaymentText, System.Int32 Earning_iKey, System.Guid BudgetProjectSRC_Guid,
            System.Guid BudgetProjectDST_Guid, System.Guid CompanyPayer_Guid, System.Guid ChildDepart_Guid,
            System.Guid AccountPlan_Guid, System.Guid PaymentType_Guid, System.Boolean Earning_IsBonus, System.Guid EarningType_Guid, 
            ref System.Guid Earning_Guid,
            UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;

            if (IsAllParametersValidInCEarning(Earning_CustomerGuid, Earning_CurrencyGuid,
                Earning_Date, Earning_DocNum,
                Earning_AccountGuid, Earning_Value, Earning_CompanyGuid,
                Earning_CurrencyRate, Earning_CurrencyValue, Earning_CustomerText,
                Earning_DetailsPaymentText, Earning_iKey, BudgetProjectSRC_Guid,
                BudgetProjectDST_Guid, CompanyPayer_Guid, ChildDepart_Guid,
                AccountPlan_Guid, PaymentType_Guid, Earning_IsBonus, EarningType_Guid,  ref strErr) == false)
            { return bRet; }

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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddCEarningToSQLandIB]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_Guid", System.Data.SqlDbType.UniqueIdentifier) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CustomerGuid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CurrencyGuid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_Date", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_DocNum", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_AccountGuid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_Value", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CompanyGuid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CurrencyRate", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CurrencyValue", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CustomerText", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_DetailsPaymentText", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_iKey", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BudgetProjectSRC_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BudgetProjectDST_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyPayer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ChildDepart_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AccountPlan_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PaymentType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_IsBonus", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EarningType_Guid", System.Data.SqlDbType.UniqueIdentifier));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });

                cmd.Parameters["@Earning_CustomerGuid"].Value = Earning_CustomerGuid;
                cmd.Parameters["@ChildDepart_Guid"].Value = ChildDepart_Guid;

                if (Earning_AccountGuid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@Earning_AccountGuid"].IsNullable = true;
                    cmd.Parameters["@Earning_AccountGuid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@Earning_AccountGuid"].IsNullable = false;
                    cmd.Parameters["@Earning_AccountGuid"].Value = Earning_AccountGuid;
                }

                if (Earning_CompanyGuid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@Earning_CompanyGuid"].IsNullable = true;
                    cmd.Parameters["@Earning_CompanyGuid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@Earning_CompanyGuid"].IsNullable = false;
                    cmd.Parameters["@Earning_CompanyGuid"].Value = Earning_CompanyGuid;
                }

                if (BudgetProjectSRC_Guid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@BudgetProjectSRC_Guid"].IsNullable = true;
                    cmd.Parameters["@BudgetProjectSRC_Guid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@BudgetProjectSRC_Guid"].IsNullable = false;
                    cmd.Parameters["@BudgetProjectSRC_Guid"].Value = BudgetProjectSRC_Guid;
                }
                if (BudgetProjectDST_Guid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@BudgetProjectDST_Guid"].IsNullable = true;
                    cmd.Parameters["@BudgetProjectDST_Guid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@BudgetProjectDST_Guid"].IsNullable = false;
                    cmd.Parameters["@BudgetProjectDST_Guid"].Value = BudgetProjectDST_Guid;
                }
                if (CompanyPayer_Guid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@CompanyPayer_Guid"].IsNullable = true;
                    cmd.Parameters["@CompanyPayer_Guid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@CompanyPayer_Guid"].IsNullable = false;
                    cmd.Parameters["@CompanyPayer_Guid"].Value = CompanyPayer_Guid;
                }
                if (AccountPlan_Guid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@AccountPlan_Guid"].IsNullable = true;
                    cmd.Parameters["@AccountPlan_Guid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@AccountPlan_Guid"].IsNullable = false;
                    cmd.Parameters["@AccountPlan_Guid"].Value = AccountPlan_Guid;
                }
                if (PaymentType_Guid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@PaymentType_Guid"].IsNullable = true;
                    cmd.Parameters["@PaymentType_Guid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@PaymentType_Guid"].IsNullable = false;
                    cmd.Parameters["@PaymentType_Guid"].Value = PaymentType_Guid;
                }
                if (EarningType_Guid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@EarningType_Guid"].IsNullable = true;
                    cmd.Parameters["@EarningType_Guid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@EarningType_Guid"].IsNullable = false;
                    cmd.Parameters["@EarningType_Guid"].Value = EarningType_Guid;
                }

                cmd.Parameters["@Earning_CurrencyGuid"].Value = Earning_CurrencyGuid;
                cmd.Parameters["@Earning_Date"].Value = Earning_Date;
                cmd.Parameters["@Earning_DocNum"].Value = Earning_DocNum;
                cmd.Parameters["@Earning_Value"].Value = Earning_Value;
                cmd.Parameters["@Earning_CurrencyRate"].Value = Earning_CurrencyRate;
                cmd.Parameters["@Earning_CurrencyValue"].Value = Earning_CurrencyValue;
                cmd.Parameters["@Earning_CustomerText"].Value = Earning_CustomerText;
                cmd.Parameters["@Earning_DetailsPaymentText"].Value = Earning_DetailsPaymentText;
                cmd.Parameters["@Earning_iKey"].Value = Earning_iKey;
                cmd.Parameters["@Earning_IsBonus"].Value = Earning_IsBonus;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    Earning_Guid = (System.Guid)cmd.Parameters["@Earning_Guid"].Value;
                }
                else
                {
                    strErr += ((cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                strErr += ("Не удалось создать объект 'платёж ф2'. Текст ошибки: " + f.Message);
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
        /// Редактирование свойств объекта "Платёж" в базе данных
        /// </summary>
        /// <param name="Earning_Guid"></param>
        /// <param name="Earning_CustomerGuid"></param>
        /// <param name="Earning_CurrencyGuid"></param>
        /// <param name="Earning_Date"></param>
        /// <param name="Earning_DocNum"></param>
        /// <param name="Earning_AccountGuid"></param>
        /// <param name="Earning_Value"></param>
        /// <param name="Earning_CompanyGuid"></param>
        /// <param name="Earning_CurrencyRate"></param>
        /// <param name="Earning_CurrencyValue"></param>
        /// <param name="Earning_CustomerText"></param>
        /// <param name="Earning_DetailsPaymentText"></param>
        /// <param name="Earning_iKey"></param>
        /// <param name="BudgetProjectSRC_Guid"></param>
        /// <param name="BudgetProjectDST_Guid"></param>
        /// <param name="CompanyPayer_Guid"></param>
        /// <param name="ChildDepart_Guid"></param>
        /// <param name="AccountPlan_Guid"></param>
        /// <param name="PaymentType_Guid"></param>
        /// <param name="Earning_IsBonus"></param>
        /// <param name="EarningType_Guid"></param>
        /// <param name="objProfile"></param>
        /// <param name="strErr"></param>
        /// <returns></returns>
        public static System.Boolean EditObjectInDataBase(System.Guid Earning_Guid, System.Guid Earning_CustomerGuid, System.Guid Earning_CurrencyGuid,
            System.DateTime Earning_Date, System.String Earning_DocNum, 
            System.Guid Earning_AccountGuid, System.Decimal Earning_Value, System.Guid Earning_CompanyGuid,
            System.Decimal Earning_CurrencyRate, System.Decimal Earning_CurrencyValue, System.String Earning_CustomerText,
            System.String Earning_DetailsPaymentText, System.Int32 Earning_iKey, System.Guid BudgetProjectSRC_Guid,
            System.Guid BudgetProjectDST_Guid, System.Guid CompanyPayer_Guid, System.Guid ChildDepart_Guid,
            System.Guid AccountPlan_Guid, System.Guid PaymentType_Guid, System.Boolean Earning_IsBonus, System.Guid EarningType_Guid,
            UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;

            if (IsAllParametersValid(Earning_CustomerGuid, Earning_CurrencyGuid,
                Earning_Date, Earning_DocNum, Earning_AccountGuid, Earning_Value, Earning_CompanyGuid,
                Earning_CurrencyRate, Earning_CurrencyValue, Earning_CustomerText,
                Earning_DetailsPaymentText, Earning_iKey, BudgetProjectSRC_Guid,
                BudgetProjectDST_Guid, CompanyPayer_Guid, ChildDepart_Guid,
                AccountPlan_Guid, PaymentType_Guid, Earning_IsBonus, EarningType_Guid,
                ref strErr) == false)
            { return bRet; }

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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditEarningInSQLandIB]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CustomerGuid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CurrencyGuid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_Date", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_DocNum", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_AccountGuid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_Value", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CompanyGuid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CurrencyRate", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CurrencyValue", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CustomerText", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_DetailsPaymentText", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_iKey", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BudgetProjectSRC_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BudgetProjectDST_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyPayer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ChildDepart_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AccountPlan_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PaymentType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_IsBonus", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EarningType_Guid", System.Data.SqlDbType.UniqueIdentifier));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });

                cmd.Parameters["@Earning_Guid"].Value = Earning_Guid;
                if (Earning_CustomerGuid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@Earning_CustomerGuid"].IsNullable = true;
                    cmd.Parameters["@Earning_CustomerGuid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@Earning_CustomerGuid"].IsNullable = false;
                    cmd.Parameters["@Earning_CustomerGuid"].Value = Earning_CustomerGuid;
                }
                if (BudgetProjectSRC_Guid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@BudgetProjectSRC_Guid"].IsNullable = true;
                    cmd.Parameters["@BudgetProjectSRC_Guid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@BudgetProjectSRC_Guid"].IsNullable = false;
                    cmd.Parameters["@BudgetProjectSRC_Guid"].Value = BudgetProjectSRC_Guid;
                }
                if (BudgetProjectDST_Guid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@BudgetProjectDST_Guid"].IsNullable = true;
                    cmd.Parameters["@BudgetProjectDST_Guid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@BudgetProjectDST_Guid"].IsNullable = false;
                    cmd.Parameters["@BudgetProjectDST_Guid"].Value = BudgetProjectDST_Guid;
                }
                if (CompanyPayer_Guid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@CompanyPayer_Guid"].IsNullable = true;
                    cmd.Parameters["@CompanyPayer_Guid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@CompanyPayer_Guid"].IsNullable = false;
                    cmd.Parameters["@CompanyPayer_Guid"].Value = CompanyPayer_Guid;
                }
                if (ChildDepart_Guid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@ChildDepart_Guid"].IsNullable = true;
                    cmd.Parameters["@ChildDepart_Guid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@ChildDepart_Guid"].IsNullable = false;
                    cmd.Parameters["@ChildDepart_Guid"].Value = ChildDepart_Guid;
                }
                if (AccountPlan_Guid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@AccountPlan_Guid"].IsNullable = true;
                    cmd.Parameters["@AccountPlan_Guid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@AccountPlan_Guid"].IsNullable = false;
                    cmd.Parameters["@AccountPlan_Guid"].Value = AccountPlan_Guid;
                }
                if (PaymentType_Guid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@PaymentType_Guid"].IsNullable = true;
                    cmd.Parameters["@PaymentType_Guid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@PaymentType_Guid"].IsNullable = false;
                    cmd.Parameters["@PaymentType_Guid"].Value = PaymentType_Guid;
                }
                if (EarningType_Guid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@EarningType_Guid"].IsNullable = true;
                    cmd.Parameters["@EarningType_Guid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@EarningType_Guid"].IsNullable = false;
                    cmd.Parameters["@EarningType_Guid"].Value = EarningType_Guid;
                }

                cmd.Parameters["@Earning_CurrencyGuid"].Value = Earning_CurrencyGuid;
                cmd.Parameters["@Earning_Date"].Value = Earning_Date;
                cmd.Parameters["@Earning_DocNum"].Value = Earning_DocNum;
                cmd.Parameters["@Earning_AccountGuid"].Value = Earning_AccountGuid;
                cmd.Parameters["@Earning_Value"].Value = Earning_Value;
                cmd.Parameters["@Earning_CompanyGuid"].Value = Earning_CompanyGuid;
                cmd.Parameters["@Earning_CurrencyRate"].Value = Earning_CurrencyRate;
                cmd.Parameters["@Earning_CurrencyValue"].Value = Earning_CurrencyValue;
                cmd.Parameters["@Earning_CustomerText"].Value = Earning_CustomerText;
                cmd.Parameters["@Earning_DetailsPaymentText"].Value = Earning_DetailsPaymentText;
                cmd.Parameters["@Earning_iKey"].Value = Earning_iKey;
                cmd.Parameters["@Earning_IsBonus"].Value = Earning_IsBonus;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr += ((cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                strErr += ("Не удалось внести изменения в объект 'платёж'. Текст ошибки: " + f.Message);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }

        public static System.Boolean EditCEarningInDataBase(System.Guid Earning_Guid, System.Guid Earning_CustomerGuid, System.Guid Earning_CurrencyGuid,
             System.DateTime Earning_Date, System.String Earning_DocNum,
             System.Guid Earning_AccountGuid, System.Decimal Earning_Value, System.Guid Earning_CompanyGuid,
             System.Decimal Earning_CurrencyRate, System.Decimal Earning_CurrencyValue, System.String Earning_CustomerText,
             System.String Earning_DetailsPaymentText, System.Int32 Earning_iKey, System.Guid BudgetProjectSRC_Guid,
             System.Guid BudgetProjectDST_Guid, System.Guid CompanyPayer_Guid, System.Guid ChildDepart_Guid,
             System.Guid AccountPlan_Guid, System.Guid PaymentType_Guid, System.Boolean Earning_IsBonus, System.Guid EarningType_Guid,
             UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;

            if ( IsAllParametersValidInCEarning(Earning_CustomerGuid, Earning_CurrencyGuid,
                Earning_Date, Earning_DocNum, Earning_AccountGuid, Earning_Value, Earning_CompanyGuid,
                Earning_CurrencyRate, Earning_CurrencyValue, Earning_CustomerText,
                Earning_DetailsPaymentText, Earning_iKey, BudgetProjectSRC_Guid,
                BudgetProjectDST_Guid, CompanyPayer_Guid, ChildDepart_Guid,
                AccountPlan_Guid, PaymentType_Guid, Earning_IsBonus, EarningType_Guid,   ref strErr) == false)
            { return bRet; }

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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditCEarningInSQLandIB]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CustomerGuid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CurrencyGuid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_Date", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_DocNum", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_AccountGuid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_Value", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CompanyGuid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CurrencyRate", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CurrencyValue", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_CustomerText", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_DetailsPaymentText", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_iKey", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BudgetProjectSRC_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BudgetProjectDST_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CompanyPayer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ChildDepart_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AccountPlan_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PaymentType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_IsBonus", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EarningType_Guid", System.Data.SqlDbType.UniqueIdentifier));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });

                cmd.Parameters["@Earning_Guid"].Value = Earning_Guid;
                cmd.Parameters["@Earning_CustomerGuid"].Value = Earning_CustomerGuid;
                cmd.Parameters["@ChildDepart_Guid"].Value = ChildDepart_Guid;

                if (Earning_AccountGuid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@Earning_AccountGuid"].IsNullable = true;
                    cmd.Parameters["@Earning_AccountGuid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@Earning_AccountGuid"].IsNullable = false;
                    cmd.Parameters["@Earning_AccountGuid"].Value = Earning_AccountGuid;
                }

                if (Earning_CompanyGuid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@Earning_CompanyGuid"].IsNullable = true;
                    cmd.Parameters["@Earning_CompanyGuid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@Earning_CompanyGuid"].IsNullable = false;
                    cmd.Parameters["@Earning_CompanyGuid"].Value = Earning_CompanyGuid;
                }

                if (BudgetProjectSRC_Guid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@BudgetProjectSRC_Guid"].IsNullable = true;
                    cmd.Parameters["@BudgetProjectSRC_Guid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@BudgetProjectSRC_Guid"].IsNullable = false;
                    cmd.Parameters["@BudgetProjectSRC_Guid"].Value = BudgetProjectSRC_Guid;
                }
                if (BudgetProjectDST_Guid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@BudgetProjectDST_Guid"].IsNullable = true;
                    cmd.Parameters["@BudgetProjectDST_Guid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@BudgetProjectDST_Guid"].IsNullable = false;
                    cmd.Parameters["@BudgetProjectDST_Guid"].Value = BudgetProjectDST_Guid;
                }
                if (CompanyPayer_Guid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@CompanyPayer_Guid"].IsNullable = true;
                    cmd.Parameters["@CompanyPayer_Guid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@CompanyPayer_Guid"].IsNullable = false;
                    cmd.Parameters["@CompanyPayer_Guid"].Value = CompanyPayer_Guid;
                }
                if (AccountPlan_Guid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@AccountPlan_Guid"].IsNullable = true;
                    cmd.Parameters["@AccountPlan_Guid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@AccountPlan_Guid"].IsNullable = false;
                    cmd.Parameters["@AccountPlan_Guid"].Value = AccountPlan_Guid;
                }
                if (PaymentType_Guid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@PaymentType_Guid"].IsNullable = true;
                    cmd.Parameters["@PaymentType_Guid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@PaymentType_Guid"].IsNullable = false;
                    cmd.Parameters["@PaymentType_Guid"].Value = PaymentType_Guid;
                }
                if (EarningType_Guid.Equals(System.Guid.Empty) == true)
                {
                    cmd.Parameters["@EarningType_Guid"].IsNullable = true;
                    cmd.Parameters["@EarningType_Guid"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@EarningType_Guid"].IsNullable = false;
                    cmd.Parameters["@EarningType_Guid"].Value = EarningType_Guid;
                }

                cmd.Parameters["@Earning_CurrencyGuid"].Value = Earning_CurrencyGuid;
                cmd.Parameters["@Earning_Date"].Value = Earning_Date;
                cmd.Parameters["@Earning_DocNum"].Value = Earning_DocNum;
                cmd.Parameters["@Earning_Value"].Value = Earning_Value;
                cmd.Parameters["@Earning_CurrencyRate"].Value = Earning_CurrencyRate;
                cmd.Parameters["@Earning_CurrencyValue"].Value = Earning_CurrencyValue;
                cmd.Parameters["@Earning_CustomerText"].Value = Earning_CustomerText;
                cmd.Parameters["@Earning_DetailsPaymentText"].Value = Earning_DetailsPaymentText;
                cmd.Parameters["@Earning_iKey"].Value = Earning_iKey;
                cmd.Parameters["@Earning_IsBonus"].Value = Earning_IsBonus;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr += ((cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                strErr += ("Не удалось внести изменения в объект 'платёж'. Текст ошибки: " + f.Message);
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
        /// Удаляет запись из БД
        /// </summary>
        /// <param name="SegmentationSubChannel_Guid"></param>
        /// <param name="objProfile"></param>
        /// <param name="strErr"></param>
        /// <returns></returns>
        public static System.Boolean RemoveObjectFromDataBase(System.Guid Earning_Guid,
           UniXP.Common.CProfile objProfile, ref System.String strErr)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteEarning2]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Earning_Guid"].Value = Earning_Guid;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr += ((cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                strErr += ("Не удалось удалить объект 'платёж'. Текст ошибки: " + f.Message);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }

        public static System.Boolean RemoveCEarningFromDataBase(System.Guid Earning_Guid,
           UniXP.Common.CProfile objProfile, ref System.String strErr)
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
                cmd.CommandTimeout = 600;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteCEarning]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Earning_Guid"].Value = Earning_Guid;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr += ((cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                strErr += ("Не удалось удалить объект 'платёж ф2'. Текст ошибки: " + f.Message);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        
        #endregion

        #region Список платежей
        /// <summary>
        /// Возвращает список платежей по форме 1 для компании за указанный период
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="dtBeginDate">начало периода</param>
        /// <param name="dtEndDate">окончание периода</param>
        /// <param name="uuidCompanyId">идентификатор компании</param>
        /// <param name="uuidCustomerId">идентификатор клиента</param>
        /// <param name="strErr"соообщение об ошибке></param>
        /// <returns>список платежей</returns>
        public static List<CEarning> GetEarningList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, 
            System.DateTime dtBeginDate, System.DateTime dtEndDate, System.Guid uuidCompanyId, System.Guid uuidCustomerId,
            ref System.String strErr)
             
        {
            List<CEarning> objList = new List<CEarning>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetEarningList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_DateBegin", System.Data.DbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_DateEnd", System.Data.DbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_guidCompany", System.Data.DbType.Guid));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Earning_DateBegin"].Value = dtBeginDate;
                cmd.Parameters["@Earning_DateEnd"].Value = dtEndDate;
                cmd.Parameters["@Earning_guidCompany"].Value = uuidCompanyId;

                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                System.Int32 iRecordCount = 0;
                if (rs.HasRows)
                {
                    CEarning objEarning = null;
                    while (rs.Read())
                    {
                        iRecordCount++;

                        if (uuidCustomerId.CompareTo(System.Guid.Empty) != 0)
                        {
                            if ((rs["Customer_Guid"] == System.DBNull.Value) || (((System.Guid)rs["Customer_Guid"]).CompareTo(uuidCustomerId) != 0))
                            {
                                continue;
                            }
                        }
                        objEarning = new CEarning();

                            objEarning.ID = (System.Guid)rs["Earning_Guid"];
                            objEarning.InterBaseID = ((rs["Earning_Id"] == System.DBNull.Value) ? 0 : (System.Int32)rs["Earning_Id"]);
                            objEarning.Customer = ((rs["Customer_Guid"] != System.DBNull.Value) ? new CCustomer()
                            {
                                ID = (System.Guid)rs["Customer_Guid"],
                                InterBaseID = System.Convert.ToInt32(rs["CUSTOMER_ID"]),
                                Code = System.Convert.ToString(rs["Customer_Code"]),
                                ShortName = System.Convert.ToString(rs["Customer_Name"]),
                                FullName = System.Convert.ToString(rs["Customer_Name"]),
                                UNP = System.Convert.ToString(rs["Customer_UNP"]),
                                OKPO = System.Convert.ToString(rs["Customer_OKPO"]),
                                OKULP = ((rs["Customer_OKULP"] == System.DBNull.Value) ? "" : (System.String)rs["Customer_OKULP"]),
                                StateType = ((rs["CustomerStateType_Guid"] != System.DBNull.Value) ? new CStateType()
                                {
                                    ID = (System.Guid)rs["CustomerStateType_Guid"],
                                    Name = System.Convert.ToString(rs["CustomerStateType_Name"]),
                                    ShortName = System.Convert.ToString(rs["CustomerStateType_ShortName"]),
                                    IsActive = System.Convert.ToBoolean(rs["CustomerStateType_IsActive"])
                                } : null)
                            } : null);
                            objEarning.Currency = ((rs["Currency_Guid"] != System.DBNull.Value) ? new CCurrency()
                            {
                                ID = (System.Guid)rs["Currency_Guid"],
                                CurrencyAbbr = System.Convert.ToString(rs["Currency_Abbr"]),
                                CurrencyCode = System.Convert.ToString(rs["Currency_Code"]),
                                Name = System.Convert.ToString(rs["Currency_Name"])
                            } : null);
                            objEarning.Date = System.Convert.ToDateTime(rs["Earning_Date"]);
                            objEarning.DocNom = ((rs["Earning_DocNum"] != System.DBNull.Value) ? System.Convert.ToString(rs["Earning_DocNum"]) : "");
                            objEarning.CodeBank = ((rs["Bank_Code"] != System.DBNull.Value) ? System.Convert.ToString(rs["Bank_Code"]) : "");
                            objEarning.AccountNumber = ( ( rs["Account_Number"] != System.DBNull.Value ) ? System.Convert.ToString(rs["Account_Number"]) : "" );
                            objEarning.Value = System.Convert.ToDecimal(rs["Earning_Value"]);
                            objEarning.Expense = System.Convert.ToDecimal(rs["Earning_Expense"]);
                            objEarning.Saldo = System.Convert.ToDecimal(rs["Earning_Saldo"]);
                            objEarning.Company = ((rs["Company_Guid"] != System.DBNull.Value) ? new CCompany()
                            {
                                ID = (System.Guid)rs["Company_Guid"],
                                Name = System.Convert.ToString(rs["Company_Name"]),
                                Abbr = System.Convert.ToString(rs["Company_Acronym"]),
                                UNP = System.Convert.ToString(rs["Company_UNN"]),
                                OKPO = System.Convert.ToString(rs["Company_OKPO"])
                            } : null);
                            objEarning.CurRate = System.Convert.ToDecimal(rs["Earning_CurrencyRate"]);
                            objEarning.CurValue = System.Convert.ToDecimal(rs["Earning_CurrencyValue"]);
                            objEarning.CustomrText = ( ( rs["Earning_CustomerText"] != System.DBNull.Value ) ? System.Convert.ToString(rs["Earning_CustomerText"]) : "");
                            objEarning.DetailsPayment = ((rs["Earning_DetailsPaymentText"] != System.DBNull.Value) ? System.Convert.ToString(rs["Earning_DetailsPaymentText"]) : "" );
                            objEarning.BudgetProjectSrc = ((rs["BudgetProjectSRC_Guid"] != System.DBNull.Value) ? new CBudgetProject()
                            {
                                ID = (System.Guid)rs["BudgetProjectSRC_Guid"],
                                Name = System.Convert.ToString(rs["BudgetProjectSRC_BUDGETPROJECT_NAME"]),
                                IsActive = System.Convert.ToBoolean(rs["BudgetProjectSRC_BUDGETPROJECT_ACTIVE"]),
                                CodeIn1C = System.Convert.ToInt32(rs["BudgetProjectSRC_BUDGETPROJECT_1C_CODE"])
                            } : null);
                            objEarning.BudgetProjectDst = ((rs["BudgetProjectDST_Guid"] != System.DBNull.Value) ? new CBudgetProject()
                            {
                                ID = (System.Guid)rs["BudgetProjectDST_Guid"],
                                Name = System.Convert.ToString(rs["BudgetProjectDST_BUDGETPROJECT_NAME"]),
                                IsActive = System.Convert.ToBoolean(rs["BudgetProjectDST_BUDGETPROJECT_ACTIVE"]),
                                CodeIn1C = System.Convert.ToInt32(rs["BudgetProjectDST_BUDGETPROJECT_1C_CODE"])
                            } : null);
                            objEarning.CompanyPayer = ((rs["CompanyPayer_Guid"] != System.DBNull.Value) ? new CCompany()
                            {
                                ID = (System.Guid)rs["CompanyPayer_Guid"],
                                Name = System.Convert.ToString(rs["CompanyPayerCompany_Name"]),
                                Abbr = System.Convert.ToString(rs["CompanyPayerCompany_Acronym"]),
                                UNP = System.Convert.ToString(rs["CompanyPayerCompany_UNN"]),
                                OKPO = System.Convert.ToString(rs["CompanyPayerCompany_OKPO"])
                            } : null);
                            objEarning.AccountPlan = ((rs["AccountPlan_Guid"] != System.DBNull.Value) ? new CAccountPlan()
                            {
                                ID = (System.Guid)rs["AccountPlan_Guid"],
                                Name = System.Convert.ToString(rs["ACCOUNTPLAN_NAME"]),
                                IsActive = System.Convert.ToBoolean(rs["ACCOUNTPLAN_ACTIVE"]),
                                CodeIn1C = System.Convert.ToString(rs["ACCOUNTPLAN_1C_CODE"])
                            } : null);
                            objEarning.PaymentType = ((rs["PaymentType_Guid"] != System.DBNull.Value) ? new CPaymentType((System.Guid)rs["PaymentType_Guid"], System.Convert.ToString(rs["PaymentType_Name"])) { Payment_Id = System.Convert.ToInt32(rs["PaymentType_Id"])} 
                            : null);
                            objEarning.IsBonusEarning = ((rs["Earning_IsBonus"] != System.DBNull.Value) ? System.Convert.ToBoolean(rs["Earning_IsBonus"]) : false);
                            objEarning.GroupKeyId = ((rs["Earning_iKey"] != System.DBNull.Value) ? System.Convert.ToInt32(rs["Earning_iKey"]) : 0);
                            objEarning.ChildDepart = ((rs["CustomerChild_Guid"] != System.DBNull.Value) ? new CChildDepart()
                            {
                                ID = (System.Guid)rs["ChildDepart_Guid"],
                                Code = System.Convert.ToString(rs["ChildDepart_Code"]),
                                Name = System.Convert.ToString(rs["ChildDepart_Name"]),
                                IsMain = System.Convert.ToBoolean(rs["ChildDepart_Main"]),
                                IsBlock = System.Convert.ToBoolean(rs["ChildDepart_NotActive"]),
                                MaxDebt = System.Convert.ToDecimal(rs["ChildDepart_MaxDebt"]),
                                MaxDelay = System.Convert.ToDecimal(rs["ChildDepart_MaxDelay"])
                            } : null);
                            if (rs["Account_Guid"] != System.DBNull.Value)
                            {
                                objEarning.Account = new CAccount();
                                objEarning.Account.ID = (System.Guid)rs["Account_Guid"];
                                objEarning.Account.AccountNumber = ((rs["Account_Number"] != System.DBNull.Value) ? System.Convert.ToString(rs["Account_Number"]) : "");
                                objEarning.Account.Currency = (( rs["AccountViewCurrency_Giud"] != System.DBNull.Value ) ? new CCurrency() { ID = (System.Guid)rs["AccountViewCurrency_Giud"], CurrencyAbbr = (System.String)rs["AccountViewCurrency_Abbr"], CurrencyCode = (System.String)rs["AccountViewCurrency_Code"] } : null);
                                objEarning.Account.AccountType = ((rs["AccountViewAccountType_Guid"] != System.DBNull.Value) ? new CAccountType() { ID = (System.Guid)rs["AccountViewAccountType_Guid"], Name = (System.String)rs["AccountViewAccountType_Name"] } : null);
                                if( rs["AccountViewBank_Guid"] != System.DBNull.Value )
                                {
                                    objEarning.Account.Bank = new CBank();

                                    objEarning.Account.Bank.ID = ( ( rs["AccountViewBank_Guid"] != System.DBNull.Value ) ? (System.Guid)rs["AccountViewBank_Guid"] : System.Guid.Empty );
                                    objEarning.Account.Bank.Name =( ( rs["AccountViewBank_Name"] != System.DBNull.Value ) ? (System.String)rs["AccountViewBank_Name"] : "" );
                                    objEarning.Account.Bank.Code = ( ( rs["AccountViewBank_Code"] != System.DBNull.Value ) ? (System.String)rs["AccountViewBank_Code"] : "" );
                                    objEarning.Account.Bank.UNN = (( rs["AccountViewBank_UNN"] != System.DBNull.Value ) ? (System.String)rs["AccountViewBank_UNN"] : "" );
                                    objEarning.Account.Bank.MFO = ((rs["AccountViewBank_MFO"] != System.DBNull.Value) ? (System.String)rs["AccountViewBank_MFO"] : "");
                                }

                            }
                            if (rs["EarningType_Guid"] != System.DBNull.Value)
                            {
                                objEarning.EarningType = new CEarningType();
                                objEarning.EarningType.ID = (System.Guid)rs["EarningType_Guid"];
                                objEarning.EarningType.Name = ((rs["EarningType_Name"] != System.DBNull.Value) ? System.Convert.ToString(rs["EarningType_Name"]) : "");
                                if (rs["EarningType_Id"] != System.DBNull.Value)
                                {
                                    objEarning.EarningType.EarningTypeId = System.Convert.ToInt32(rs["EarningType_Id"]);
                                }
                                if (rs["EarningType_IsActive"] != System.DBNull.Value)
                                {
                                    objEarning.EarningType.IsActive = System.Convert.ToBoolean(rs["EarningType_IsActive"]);
                                }
                                if (rs["EarningType_IsDefault"] != System.DBNull.Value)
                                {
                                    objEarning.EarningType.IsDefault = System.Convert.ToBoolean(rs["EarningType_IsDefault"]);
                                }
                                if (rs["EarningType_DublicateInIB"] != System.DBNull.Value)
                                {
                                    objEarning.EarningType.IsDublicateInIB = System.Convert.ToBoolean(rs["EarningType_DublicateInIB"]);
                                }
                            }

                        if (objEarning != null) { objList.Add(objEarning); }
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
                strErr += (String.Format("\nНе удалось получить список платежей.\nТекст ошибки: {0}", f.Message));
            }
            return objList;
        }

        /// <summary>
        /// Возвращает список платежей по форме 2 за указанный период
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="dtBeginDate">начало периода</param>
        /// <param name="dtEndDate">окончание периода</param>
        /// <param name="uuidCompanyId">идентификатор компании</param>
        /// <param name="uuidChildDepartId">идентификатор дочернего клиента</param>
        /// <param name="strErr"соообщение об ошибке></param>
        /// <returns>список платежей</returns>
        public static List<CEarning> GetСEarningList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL,
            System.DateTime dtBeginDate, System.DateTime dtEndDate, System.Guid uuidCompanyId, System.Guid uuidChildDepartId,
            ref System.String strErr)
        {
            List<CEarning> objList = new List<CEarning>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCEarningList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_DateBegin", System.Data.DbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_DateEnd", System.Data.DbType.Date));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Earning_DateBegin"].Value = dtBeginDate;
                cmd.Parameters["@Earning_DateEnd"].Value = dtEndDate;

                if (uuidCompanyId.Equals(System.Guid.Empty) == false)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Earning_guidCompany", System.Data.DbType.Guid));
                    cmd.Parameters["@Earning_guidCompany"].Value = uuidCompanyId;
                }

                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                System.Int32 iRecordCount = 0;
                if (rs.HasRows)
                {
                    CEarning objEarning = null;
                    while (rs.Read())
                    {
                        iRecordCount++;

                        if (uuidChildDepartId.CompareTo(System.Guid.Empty) != 0)
                        {
                            if ((rs["ChildDepart_Guid"] == System.DBNull.Value) || (((System.Guid)rs["ChildDepart_Guid"]).CompareTo(uuidChildDepartId) != 0))
                            {
                                continue;
                            }
                        }
                        objEarning = new CEarning();

                        objEarning.ID = (System.Guid)rs["Earning_Guid"];
                        objEarning.InterBaseID = ((rs["Earning_Id"] == System.DBNull.Value) ? 0 : (System.Int32)rs["Earning_Id"]);
                        objEarning.Customer = ((rs["Customer_Guid"] != System.DBNull.Value) ? new CCustomer()
                        {
                            ID = (System.Guid)rs["Customer_Guid"],
                            InterBaseID = System.Convert.ToInt32(rs["CUSTOMER_ID"]),
                            Code = System.Convert.ToString(rs["Customer_Code"]),
                            ShortName = System.Convert.ToString(rs["Customer_Name"]),
                            FullName = System.Convert.ToString(rs["Customer_Name"]),
                            UNP = System.Convert.ToString(rs["Customer_UNP"]),
                            OKPO = System.Convert.ToString(rs["Customer_OKPO"]),
                            OKULP = ((rs["Customer_OKULP"] == System.DBNull.Value) ? "" : (System.String)rs["Customer_OKULP"]),
                            StateType = ((rs["CustomerStateType_Guid"] != System.DBNull.Value) ? new CStateType()
                            {
                                ID = (System.Guid)rs["CustomerStateType_Guid"],
                                Name = System.Convert.ToString(rs["CustomerStateType_Name"]),
                                ShortName = System.Convert.ToString(rs["CustomerStateType_ShortName"]),
                                IsActive = System.Convert.ToBoolean(rs["CustomerStateType_IsActive"])
                            } : null)
                        } : null);
                        objEarning.Currency = ((rs["Currency_Guid"] != System.DBNull.Value) ? new CCurrency()
                        {
                            ID = (System.Guid)rs["Currency_Guid"],
                            CurrencyAbbr = System.Convert.ToString(rs["Currency_Abbr"]),
                            CurrencyCode = System.Convert.ToString(rs["Currency_Code"]),
                            Name = System.Convert.ToString(rs["Currency_Name"])
                        } : null);
                        objEarning.Date = System.Convert.ToDateTime(rs["Earning_Date"]);
                        objEarning.DocNom = ((rs["Earning_DocNum"] != System.DBNull.Value) ? System.Convert.ToString(rs["Earning_DocNum"]) : "");
                        objEarning.CodeBank = ((rs["Bank_Code"] != System.DBNull.Value) ? System.Convert.ToString(rs["Bank_Code"]) : "");
                        objEarning.AccountNumber = ((rs["Account_Number"] != System.DBNull.Value) ? System.Convert.ToString(rs["Account_Number"]) : "");
                        objEarning.Value = System.Convert.ToDecimal(rs["Earning_Value"]);
                        objEarning.Expense = System.Convert.ToDecimal(rs["Earning_Expense"]);
                        objEarning.Saldo = System.Convert.ToDecimal(rs["Earning_Saldo"]);
                        objEarning.Company = ((rs["Company_Guid"] != System.DBNull.Value) ? new CCompany()
                        {
                            ID = (System.Guid)rs["Company_Guid"],
                            Name = System.Convert.ToString(rs["Company_Name"]),
                            Abbr = System.Convert.ToString(rs["Company_Acronym"]),
                            UNP = System.Convert.ToString(rs["Company_UNN"]),
                            OKPO = System.Convert.ToString(rs["Company_OKPO"])
                        } : null);
                        objEarning.CurRate = System.Convert.ToDecimal(rs["Earning_CurrencyRate"]);
                        objEarning.CurValue = System.Convert.ToDecimal(rs["Earning_CurrencyValue"]);
                        objEarning.CustomrText = ((rs["Earning_CustomerText"] != System.DBNull.Value) ? System.Convert.ToString(rs["Earning_CustomerText"]) : "");
                        objEarning.DetailsPayment = ((rs["Earning_DetailsPaymentText"] != System.DBNull.Value) ? System.Convert.ToString(rs["Earning_DetailsPaymentText"]) : "");
                        objEarning.BudgetProjectSrc = ((rs["BudgetProjectSRC_Guid"] != System.DBNull.Value) ? new CBudgetProject()
                        {
                            ID = (System.Guid)rs["BudgetProjectSRC_Guid"],
                            Name = System.Convert.ToString(rs["BudgetProjectSRC_BUDGETPROJECT_NAME"]),
                            IsActive = System.Convert.ToBoolean(rs["BudgetProjectSRC_BUDGETPROJECT_ACTIVE"]),
                            CodeIn1C = System.Convert.ToInt32(rs["BudgetProjectSRC_BUDGETPROJECT_1C_CODE"])
                        } : null);
                        objEarning.BudgetProjectDst = ((rs["BudgetProjectDST_Guid"] != System.DBNull.Value) ? new CBudgetProject()
                        {
                            ID = (System.Guid)rs["BudgetProjectDST_Guid"],
                            Name = System.Convert.ToString(rs["BudgetProjectDST_BUDGETPROJECT_NAME"]),
                            IsActive = System.Convert.ToBoolean(rs["BudgetProjectDST_BUDGETPROJECT_ACTIVE"]),
                            CodeIn1C = System.Convert.ToInt32(rs["BudgetProjectDST_BUDGETPROJECT_1C_CODE"])
                        } : null);
                        objEarning.CompanyPayer = ((rs["CompanyPayer_Guid"] != System.DBNull.Value) ? new CCompany()
                        {
                            ID = (System.Guid)rs["CompanyPayer_Guid"],
                            Name = System.Convert.ToString(rs["CompanyPayerCompany_Name"]),
                            Abbr = System.Convert.ToString(rs["CompanyPayerCompany_Acronym"]),
                            UNP = System.Convert.ToString(rs["CompanyPayerCompany_UNN"]),
                            OKPO = System.Convert.ToString(rs["CompanyPayerCompany_OKPO"])
                        } : null);
                        objEarning.AccountPlan = ((rs["AccountPlan_Guid"] != System.DBNull.Value) ? new CAccountPlan()
                        {
                            ID = (System.Guid)rs["AccountPlan_Guid"],
                            Name = System.Convert.ToString(rs["ACCOUNTPLAN_NAME"]),
                            IsActive = System.Convert.ToBoolean(rs["ACCOUNTPLAN_ACTIVE"]),
                            CodeIn1C = System.Convert.ToString(rs["ACCOUNTPLAN_1C_CODE"])
                        } : null);
                        objEarning.PaymentType = ((rs["PaymentType_Guid"] != System.DBNull.Value) ? new CPaymentType((System.Guid)rs["PaymentType_Guid"], System.Convert.ToString(rs["PaymentType_Name"])) { Payment_Id = System.Convert.ToInt32(rs["PaymentType_Id"]) }
                        : null);
                        objEarning.IsBonusEarning = ((rs["Earning_IsBonus"] != System.DBNull.Value) ? System.Convert.ToBoolean(rs["Earning_IsBonus"]) : false);
                        objEarning.ChildDepart = ((rs["CustomerChild_Guid"] != System.DBNull.Value) ? new CChildDepart()
                        {
                            ID = (System.Guid)rs["ChildDepart_Guid"],
                            Code = System.Convert.ToString(rs["ChildDepart_Code"]),
                            Name = System.Convert.ToString(rs["ChildDepart_Name"]),
                            IsMain = System.Convert.ToBoolean(rs["ChildDepart_Main"]),
                            IsBlock = System.Convert.ToBoolean(rs["ChildDepart_NotActive"]),
                            MaxDebt = System.Convert.ToDecimal(rs["ChildDepart_MaxDebt"]),
                            MaxDelay = System.Convert.ToDecimal(rs["ChildDepart_MaxDelay"])
                        } : null);
                        if (rs["Account_Guid"] != System.DBNull.Value)
                        {
                            objEarning.Account = new CAccount();
                            objEarning.Account.ID = (System.Guid)rs["Account_Guid"];
                            objEarning.Account.AccountNumber = ((rs["Account_Number"] != System.DBNull.Value) ? System.Convert.ToString(rs["Account_Number"]) : "");
                            objEarning.Account.Currency = ((rs["AccountViewCurrency_Giud"] != System.DBNull.Value) ? new CCurrency() { ID = (System.Guid)rs["AccountViewCurrency_Giud"], CurrencyAbbr = (System.String)rs["AccountViewCurrency_Abbr"], CurrencyCode = (System.String)rs["AccountViewCurrency_Code"] } : null);
                            objEarning.Account.AccountType = ((rs["AccountViewAccountType_Guid"] != System.DBNull.Value) ? new CAccountType() { ID = (System.Guid)rs["AccountViewAccountType_Guid"], Name = (System.String)rs["AccountViewAccountType_Name"] } : null);
                            if (rs["AccountViewBank_Guid"] != System.DBNull.Value)
                            {
                                objEarning.Account.Bank = new CBank();

                                objEarning.Account.Bank.ID = ((rs["AccountViewBank_Guid"] != System.DBNull.Value) ? (System.Guid)rs["AccountViewBank_Guid"] : System.Guid.Empty);
                                objEarning.Account.Bank.Name = ((rs["AccountViewBank_Name"] != System.DBNull.Value) ? (System.String)rs["AccountViewBank_Name"] : "");
                                objEarning.Account.Bank.Code = ((rs["AccountViewBank_Code"] != System.DBNull.Value) ? (System.String)rs["AccountViewBank_Code"] : "");
                                objEarning.Account.Bank.UNN = ((rs["AccountViewBank_UNN"] != System.DBNull.Value) ? (System.String)rs["AccountViewBank_UNN"] : "");
                                objEarning.Account.Bank.MFO = ((rs["AccountViewBank_MFO"] != System.DBNull.Value) ? (System.String)rs["AccountViewBank_MFO"] : "");
                            }

                        }
                        if (rs["EarningType_Guid"] != System.DBNull.Value)
                        {
                            objEarning.EarningType = new CEarningType();
                            objEarning.EarningType.ID = (System.Guid)rs["EarningType_Guid"];
                            objEarning.EarningType.Name = ((rs["EarningType_Name"] != System.DBNull.Value) ? System.Convert.ToString(rs["EarningType_Name"]) : "");
                            if (rs["EarningType_Id"] != System.DBNull.Value)
                            {
                                objEarning.EarningType.EarningTypeId = System.Convert.ToInt32(rs["EarningType_Id"]);
                            }
                            if (rs["EarningType_IsActive"] != System.DBNull.Value)
                            {
                                objEarning.EarningType.IsActive = System.Convert.ToBoolean(rs["EarningType_IsActive"]);
                            }
                            if (rs["EarningType_IsDefault"] != System.DBNull.Value)
                            {
                                objEarning.EarningType.IsDefault = System.Convert.ToBoolean(rs["EarningType_IsDefault"]);
                            }
                            if (rs["EarningType_DublicateInIB"] != System.DBNull.Value)
                            {
                                objEarning.EarningType.IsDublicateInIB = System.Convert.ToBoolean(rs["EarningType_DublicateInIB"]);
                            }
                        }

                        if (objEarning != null) { objList.Add(objEarning); }
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
                strErr += (String.Format("\nНе удалось получить список платежей по форме 2.\nТекст ошибки: {0}", f.Message));
            }
            return objList;
        }
        
        #endregion

        #region Оплаты по расходному документу
        /// <summary>
        /// Возвращает журнал разноски оплат по накладной
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="uuidDocId">УИ накладной</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>список объектов класса "CEarning"</returns>
        public static List<ERP_Mercury.Common.CEarning> GetPaymentHistory(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidDocId, ref System.String strErr)
        {
            List<ERP_Mercury.Common.CEarning> objList = new List<ERP_Mercury.Common.CEarning>();

            if (uuidDocId.CompareTo(System.Guid.Empty) == 0)
            {
                return objList;
            }

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

                cmd.CommandTimeout = 600;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPaymentHistoryFromIB]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Doc_Guid", System.Data.DbType.Guid));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Doc_Guid"].Value = uuidDocId;

                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                System.Int32 iRecordCount = 0;
                if (rs.HasRows)
                {
                    ERP_Mercury.Common.CEarning objEarning = null;
                    while (rs.Read())
                    {
                        iRecordCount++;

                        objEarning = new ERP_Mercury.Common.CEarning();
                        objEarning.InterBaseID = ((rs["EARNING_ID"] == System.DBNull.Value) ? 0 : System.Convert.ToInt32(rs["EARNING_ID"]));
                        objEarning.Date = ((rs["PAYMENTS_OPERDATE"] == System.DBNull.Value) ? System.DateTime.MinValue : (System.DateTime)rs["PAYMENTS_OPERDATE"]);
                        objEarning.Value = ((rs["PAYMENTS_VALUE"] == System.DBNull.Value) ? 0 : System.Convert.ToDecimal(rs["PAYMENTS_VALUE"]));
                        objEarning.Company = new ERP_Mercury.Common.CCompany()
                        {
                            InterBaseID = ((rs["COMPANY_ID"] == System.DBNull.Value) ? 0 : (System.Int32)rs["COMPANY_ID"]),
                            Abbr = ((rs["COMPANY_ACRONYM"] == System.DBNull.Value) ? System.String.Empty : (System.String)rs["COMPANY_ACRONYM"])
                        };
                        objEarning.Customer = new ERP_Mercury.Common.CCustomer()
                        {
                            InterBaseID = ((rs["CUSTOMER_ID"] == System.DBNull.Value) ? 0 : (System.Int32)rs["CUSTOMER_ID"]),
                            FullName = ((rs["CUSTOMER_NAME"] == System.DBNull.Value) ? System.String.Empty : (System.String)rs["CUSTOMER_NAME"])
                        };
                        objEarning.Currency = new CCurrency()
                        {
                             CurrencyAbbr = ((rs["CURRENCY_CODE"] == System.DBNull.Value) ? System.String.Empty : (System.String)rs["CURRENCY_CODE"])
                        };

                        if (objEarning != null) { objList.Add(objEarning); }
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
                strErr += (String.Format("\nНе удалось выполнить отчёт \"журнал разноски оплат\".\nТекст ошибки: {0}", f.Message));
            }
            return objList;
        }
        #endregion
    }

}
