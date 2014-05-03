using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP_Mercury.Common
{
    public class CCreditLimit
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
        /// Компанию
        /// </summary>
        private CCompany m_objCompany;
        /// <summary>
        /// Компанию
        /// </summary>
        public CCompany Company
        {
            get { return m_objCompany; }
            set { m_objCompany = value; }
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
        /// Утвержденная сумма кредита
        /// </summary>
        private double m_ApprovedCurrencyValue;
        /// <summary>
        /// Утвержденная сумма кредита
        /// </summary>
        public double ApprovedCurrencyValue
        {
            get { return m_ApprovedCurrencyValue; }
            set { m_ApprovedCurrencyValue = value; }
        }
        /// <summary>
        /// Разрешенная сумма кредита
        /// </summary>
        private double m_CurrencyValue;
        /// <summary>
        /// Разрешенная сумма кредита
        /// </summary>
        public double CurrencyValue
        {
            get { return m_CurrencyValue; }
            set { m_CurrencyValue = value; }
        }
        /// <summary>
        /// Утвержденная отсрочка (дней)
        /// </summary>
        private int m_ApprovedDays;
        /// <summary>
        /// Утвержденная отсрочка (дней)
        /// </summary>
        public int ApprovedDays
        {
            get { return m_ApprovedDays; }
            set { m_ApprovedDays = value; }
        }
        /// <summary>
        /// Разрешенная отсрочка (дней)
        /// </summary>
        private int m_Days;
        /// <summary>
        /// Разрешенная отсрочка (дней)
        /// </summary>
        public int Days
        {
            get { return m_Days; }
            set { m_Days = value; }
        }

        #endregion

        #region Конструктор
        public CCreditLimit()
        {
            m_uuidID = System.Guid.Empty;
            m_objCustomer = null;
            m_objCompany = null;
            m_objCurrency = null;
            m_ApprovedCurrencyValue = 0;
            m_CurrencyValue = 0;
            m_ApprovedDays = 0;
            m_Days = 0;
        }
        public CCreditLimit(System.Guid uuidID, CCustomer objCustomer, CCompany objCompany, CCurrency objCurrency,
            double mApprovedCurrencyValue, double mCurrencyValue, int iApprovedDays, int iDays)
        {
            m_uuidID = uuidID;
            m_objCustomer = objCustomer;
            m_objCompany = objCompany;
            m_objCurrency = objCurrency;
            m_ApprovedCurrencyValue = mApprovedCurrencyValue;
            m_CurrencyValue = mCurrencyValue;
            m_ApprovedDays = iApprovedDays;
            m_Days = iDays;
        }
        #endregion

        #region Список кредитных лимитов
        /// <summary>
        /// Возвращает список кредитных лимитов для заданного клиента
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objCustomer">клиент</param>
        /// <returns>список кредитных лимитов</returns>
        public static List<CCreditLimit> GetCreditLimitList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, CCustomer objCustomer )
        {
            List<CCreditLimit> objList = new List<CCreditLimit>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetCustomerLimitForCustomer]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Customer_Guid"].Value = objCustomer.ID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CCompany objCompany = null;
                    CCurrency objCurrency = null;

                    while (rs.Read())
                    {
                        objCurrency = new CCurrency((System.Guid)rs["Currency_Guid"], (System.String)rs["Currency_Name"],
                            (System.String)rs["Currency_Abbr"], (System.String)rs["Currency_Code"]);
                        objCompany = new CCompany((System.Guid)rs["Company_Guid"],
                            (System.String)rs["Company_Name"], (System.String)rs["Company_Acronym"]);
                        objList.Add(new CCreditLimit((System.Guid)rs["CustomerLimit_Guid"], objCustomer, objCompany, objCurrency, 
                            System.Convert.ToDouble(rs["CustomerLimit_ApprovedCurrencyValue"]),
                            System.Convert.ToDouble(rs["CustomerLimit_CurrencyValue"]),
                            System.Convert.ToInt32(rs["CustomerLimit_ApprovedDays"]),
                            System.Convert.ToInt32(rs["CustomerLimit_Days"])));
                    }
                }
                rs.Close();
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
                "Не удалось получить список кредитных лимитов для заданного клиента.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Возвращает признак того, находится ли клиент в "черном списке" по кредитномулимиту
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objCustomer">клиент</param>
        /// <returns>true - клиент в "черном списке"; false - клиента НЕТ в "черном списке"</returns>
        public static System.Boolean IsCustomerInBlackList(UniXP.Common.CProfile objProfile,
           System.Data.SqlClient.SqlCommand cmdSQL, CCustomer objCustomer)
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetIsCustomerInBlackList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@IsInBlackList", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@IsInBlackList"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Customer_Guid"].Value = objCustomer.ID;
                cmd.ExecuteNonQuery();
                bRet = System.Convert.ToBoolean(cmd.Parameters["@IsInBlackList"].Value);
                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить информацию о вхождении клиента в черный список.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return bRet;
        }
        #endregion

        #region Сохранение кредитных лимитов в БД
        /// <summary>
        /// Сохраняет в БД информацию о кредитных лимитах
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objCreditLimitList">список кредитных лимитов</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean SaveCreditLimitList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            List<CCreditLimit> objCreditLimitList, CCustomer objCustomer, CCurrency objCurrency, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (objCreditLimitList == null) { return bRet; }

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
                System.Data.DataTable addedCategories = new System.Data.DataTable();
                addedCategories.Columns.Add(new System.Data.DataColumn("Customer_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedCategories.Columns.Add(new System.Data.DataColumn("Company_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedCategories.Columns.Add(new System.Data.DataColumn("Currency_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedCategories.Columns.Add(new System.Data.DataColumn("CustomerLimit_ApprovedCurrencyValue", typeof(System.Data.SqlTypes.SqlMoney)));
                addedCategories.Columns.Add(new System.Data.DataColumn("CustomerLimit_CurrencyValue", typeof(System.Data.SqlTypes.SqlMoney)));
                addedCategories.Columns.Add(new System.Data.DataColumn("CustomerLimit_ApprovedDays", typeof( float )));
                addedCategories.Columns.Add(new System.Data.DataColumn("CustomerLimit_Days", typeof( float )));

                System.Data.DataRow newRow = null;
                foreach (CCreditLimit objItem in objCreditLimitList)
                {
                    newRow = addedCategories.NewRow();
                    newRow["Customer_Guid"] = objCustomer.ID;
                    newRow["Company_Guid"] = objItem.Company.ID;
                    newRow["Currency_Guid"] = objCurrency.ID;
                    newRow["CustomerLimit_ApprovedCurrencyValue"] = System.Convert.ToDecimal( objItem.ApprovedCurrencyValue );
                    newRow["CustomerLimit_CurrencyValue"] =  System.Convert.ToDecimal( objItem.CurrencyValue );
                    newRow["CustomerLimit_ApprovedDays"] = objItem.ApprovedDays;
                    newRow["CustomerLimit_Days"] = objItem.Days;
                    addedCategories.Rows.Add(newRow);
                }
                if (objCreditLimitList.Count > 0)
                {
                    addedCategories.AcceptChanges();
                }

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AssignCustomerLimit]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.AddWithValue("@tCreditLimitList", addedCategories);
                cmd.Parameters["@tCreditLimitList"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tCreditLimitList"].TypeName = "dbo.udt_CreditLimitList";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Customer_Guid"].Value = objCustomer.ID;
                cmd.Parameters["@Currency_Guid"].Value = objCurrency.ID;
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

                            // 2010.04.22
                            // временно пишем в ERP
                            cmd.Parameters.Clear();
                            cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AssignCustomerLimit_ERP]", objProfile.GetOptionsDllDBName());
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Guid", System.Data.SqlDbType.UniqueIdentifier));
                            cmd.Parameters.AddWithValue("@tCreditLimitList", addedCategories);
                            cmd.Parameters["@tCreditLimitList"].SqlDbType = System.Data.SqlDbType.Structured;
                            cmd.Parameters["@tCreditLimitList"].TypeName = "dbo.udt_CreditLimitList";
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                            cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                            cmd.Parameters["@Customer_Guid"].Value = objCustomer.ID;
                            cmd.Parameters["@Currency_Guid"].Value = objCurrency.ID;
                            cmd.ExecuteNonQuery();
                            iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

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

        #region Расчет кредитного лимита
        /// <summary>
        /// Расчет кредитного лимита для заданного клиента
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objCustomer">клиент</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean CalcCreditLimitListForCustomer(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            CCustomer objCustomer, ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (objCustomer == null) { return bRet; }

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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_ProcessCreditLimitForCustomer]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Customer_Guid"].Value = objCustomer.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;

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

    public class CCreditLimitArjive : CCreditLimit
    {
        #region Свойства
        /// <summary>
        /// Дата изменения записи
        /// </summary>
        private System.DateTime m_objDateUpdate;
        /// <summary>
        /// Дата изменения записи
        /// </summary>
        public System.DateTime DateUpdate
        {
            get { return m_objDateUpdate; }
        }
        /// <summary>
        /// Тип операции (наименование)
        /// </summary>
        private System.String m_strActionTypeName;
        /// <summary>
        /// Тип операции (наименование)
        /// </summary>
        public System.String ActionTypeName
        {
            get { return m_strActionTypeName; }
        }
        /// <summary>
        /// Тот, кто все это натворил
        /// </summary>
        private System.String m_strUserName;
        /// <summary>
        /// Тот, кто все это натворил
        /// </summary>
        public System.String UserName
        {
            get { return m_strUserName; }
        }
        /// <summary>
        /// Клиент
        /// </summary>
        public System.String CustomerName
        {
            get { return ( ( Customer == null ) ? "" : Customer.ShortName ); }
        }
        /// <summary>
        /// Компания
        /// </summary>
        public System.String CompanyName
        {
            get { return ( ( Company == null ) ? "" : Company.Abbr ); }
        }
        /// <summary>
        /// Валюта
        /// </summary>
        public System.String CurrencyName
        {
            get { return ((Currency == null) ? "" : Currency.CurrencyAbbr); }
        }
        #endregion

        #region Конструктор
        public CCreditLimitArjive() : base()
        {
            m_strActionTypeName = "";
            m_strUserName = "";
        }
        public CCreditLimitArjive( System.DateTime dtDateUpdate, System.String strActionTypeName, System.String strUserName, 
            System.Guid uuidID, CCustomer objCustomer, CCompany objCompany, CCurrency objCurrency,
            double mApprovedCurrencyValue, double mCurrencyValue, int iApprovedDays, int iDays )
        {
            m_objDateUpdate = dtDateUpdate;
            m_strActionTypeName = strActionTypeName;
            m_strUserName = strUserName;

            ID = uuidID;
            Customer = objCustomer;
            Company = objCompany;
            Currency = objCurrency;
            ApprovedCurrencyValue = mApprovedCurrencyValue;
            CurrencyValue = mCurrencyValue;
            ApprovedDays = iApprovedDays;
            Days = iDays;
        }
        #endregion

        #region Журнал изменений
        /// <summary>
        /// Возвращает список кредитных лимитов для заданного клиента
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objCustomer">клиент</param>
        /// <returns>список кредитных лимитов</returns>
        public static List<CCreditLimitArjive> GetCreditLimitArjiveList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, CCustomer objCustomer, System.DateTime BeginDate, System.DateTime EndDate )
        {
            List<CCreditLimitArjive> objList = new List<CCreditLimitArjive>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetCustomerLimitArjiveForCustomer]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BeginDate", System.Data.SqlDbType.DateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EndDate", System.Data.SqlDbType.DateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Customer_Guid"].Value = objCustomer.ID;
                cmd.Parameters["@BeginDate"].Value = BeginDate;
                cmd.Parameters["@EndDate"].Value = EndDate;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CCompany objCompany = null;
                    CCurrency objCurrency = null;

                    while (rs.Read())
                    {
                        objCurrency = new CCurrency((System.Guid)rs["Currency_Guid"], (System.String)rs["Currency_Name"],
                            (System.String)rs["Currency_Abbr"], (System.String)rs["Currency_Code"]);
                        objCompany = new CCompany((System.Guid)rs["Company_Guid"],
                            (System.String)rs["Company_Name"], (System.String)rs["Company_Acronym"]);
                        objList.Add(new CCreditLimitArjive(System.Convert.ToDateTime(rs["Record_Updated"]),
                            (System.String)rs["Action_TypeName"], (System.String)rs["Record_UserUdpated"],
                            (System.Guid)rs["CustomerLimit_Guid"], objCustomer, objCompany, objCurrency,
                            System.Convert.ToDouble(rs["CustomerLimit_ApprovedCurrencyValue"]),
                            System.Convert.ToDouble(rs["CustomerLimit_CurrencyValue"]),
                            System.Convert.ToInt32(rs["CustomerLimit_ApprovedDays"]),
                            System.Convert.ToInt32(rs["CustomerLimit_Days"])));
                    }
                }
                rs.Close();
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
                "Не удалось получить список кредитных лимитов для заданного клиента из архива.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        #endregion
    }
}
