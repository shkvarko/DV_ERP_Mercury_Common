using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ERP_Mercury.Common
{
    /// <summary>
    /// Класс "Тип цены"
    /// </summary>
    public class CPriceType : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// TypeConverter для списка валют
        /// </summary>
        class CurrencyConverter : TypeConverter
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

                CPriceType objPriceType = (CPriceType)context.Instance;
                System.Collections.Generic.List<CCurrency> objList = objPriceType.GetAllCurrencyList();

                return new StandardValuesCollection(objList);
            }
        }
        /// <summary>
        /// Сокращение
        /// </summary>
        private System.String m_strAbr;
        /// <summary>
        /// Сокращение
        /// </summary>
        [DisplayName("Сокращение")]
        [Description("Сокращенное наименование, используется в заголовках столбцов MS Excel")]
        [Category("1. Обязательные значения")]
        public System.String Abr
        {
            get { return m_strAbr; }
            set { m_strAbr = value; }
        }
        /// <summary>
        /// Номер столбца в MS Excel по умолчанию
        /// </summary>
        private System.Int32 m_iColumnID;
        /// <summary>
        /// Номер столбца в MS Excel по умолчанию
        /// </summary>
        [DisplayName("№ столбца")]
        [Description("№ столбца в файле MS Excel с расчетом цен")]
        [Category("1. Обязательные значения")]
        public System.Int32 ColumnID
        {
            get { return m_iColumnID; }
            set { m_iColumnID = value; }
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
        /// Признак "показывать в прайс-листе"
        /// </summary>
        private System.Boolean m_bIsShowInPrice;
        /// <summary>
        /// Признак "показывать в прайс-листе"
        /// </summary>
        [DisplayName("Показывать в прайсе")]
        [Description("Отображать цену в прайс-листе для общего доступа")]
        [Category("1. Обязательные значения")]
        public System.Boolean IsShowInPrice
        {
            get { return m_bIsShowInPrice; }
            set { m_bIsShowInPrice = value; }
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

        private List<CCurrency> m_objAllCurrencyList;
        public List<CCurrency> GetAllCurrencyList()
        {
            return m_objAllCurrencyList;
        }

        /// <summary>
        /// Валюта
        /// </summary>
        private CCurrency m_objCurrency;
        /// <summary>
        /// Валюта
        /// </summary>
        [DisplayName("Валюта")]
        [Description("Валюта")]
        [Category("1. Обязательные значения")]
        [TypeConverter(typeof(CurrencyConverter))]
        [ReadOnly(false)]
        [BrowsableAttribute(false)]
        public CCurrency Currency
        {
            get { return m_objCurrency; }
            set { m_objCurrency = value; }
        }
        /// <summary>
        /// Валюта
        /// </summary>
        [DisplayName("Валюта")]
        [Description("Валюта")]
        [Category("1. Обязательные значения")]
        [TypeConverter(typeof(CurrencyConverter))]
        public System.String CurrencyCode
        {
            get { return m_objCurrency.CurrencyAbbr; }
            set { SetCurrencyValue(value); }
        }
        private void SetCurrencyValue(System.String strCurrencyAbbr)
        {
            try
            {
                if (m_objAllCurrencyList == null) { m_objCurrency = null; }
                else
                {
                    foreach (CCurrency objCurrency in m_objAllCurrencyList)
                    {
                        if (objCurrency.CurrencyAbbr == strCurrencyAbbr)
                        {
                            m_objCurrency = objCurrency;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось установить значение валюты.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }


        #endregion

        #region Конструктор
        public CPriceType()
            : base()
        {
            m_bIsActive = false;
            m_iColumnID = 0;
            m_objAllCurrencyList = null;
            m_objCurrency = null;
            m_strAbr = "";
            m_strDescription = "";
            m_bIsShowInPrice = false;
        }
        public CPriceType(System.Guid uuidId, System.String strName, System.String strAbr, System.String strDescription,
            System.Boolean bIsActive, CCurrency objCurrency, System.Int32 iColumnID, System.Boolean bIsShowInPrice
            )
        {
            ID = uuidId;
            Name = strName;
            m_strAbr = strAbr;
            m_strDescription = strDescription;
            m_bIsActive = bIsActive;
            m_objCurrency = objCurrency;
            m_iColumnID = iColumnID;
            m_bIsShowInPrice = bIsShowInPrice;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список типов цен
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список типов цен</returns>
        public static List<CPriceType> GetPriceTypeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CPriceType> objList = new List<CPriceType>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetPartsubtypePriceType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {

                        objList.Add(new CPriceType(
                            (System.Guid)rs["PartsubtypePriceType_Guid"],
                            (System.String)rs["PartsubtypePriceType_Name"],
                            (System.String)rs["PartsubtypePriceType_Abbr"],
                            ((rs["PartsubtypePriceType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["PartsubtypePriceType_Description"]),
                            System.Convert.ToBoolean(rs["PartsubtypePriceType_IsActive"]),
                            new CCurrency(
                            (System.Guid)rs["Currency_Guid"],
                            (System.String)rs["Currency_Name"],
                            (System.String)rs["Currency_Abbr"],
                            (System.String)rs["Currency_Code"]
                            ),
                            System.Convert.ToInt32(rs["PartsubtypePriceType_ColumnIdDefault"]),
                            System.Convert.ToBoolean(rs["PartsubtypePriceType_ShowInPriceList"])
                            ));
                    }
                }
                rs.Dispose();

                List<CCurrency> objCurrencyList = CCurrency.GetCurrencyList(objProfile, cmd);
                if ((objList != null) && (objCurrencyList != null))
                {
                    foreach (CPriceType objSPriceType in objList)
                    {
                        objSPriceType.m_objAllCurrencyList = objCurrencyList;
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
                "Не удалось получить список типов цен.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Загружает в m_objAllProductLineList список товарных линий
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        public void InitCurrencyList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            try
            {
                this.m_objAllCurrencyList = CCurrency.GetCurrencyList(objProfile, cmdSQL);
                if ((this.m_objAllCurrencyList != null) && (this.m_objAllCurrencyList.Count > 0))
                {
                    this.Currency = this.m_objAllCurrencyList[0];
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось загрузить список валют.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                if (this.Currency == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать валюту!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.Abr == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать сокращенное наименование!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.ColumnID <= 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать номер столбца в MS Excel!", "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddPartsubtypePriceType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsubtypePriceType_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsubtypePriceType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsubtypePriceType_Abbr", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsubtypePriceType_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsubtypePriceType_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsubtypePriceType_ShowInPriceList", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsubtypePriceType_ColumnIdDefault", System.Data.SqlDbType.Int));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@PartsubtypePriceType_Name"].Value = this.Name;
                cmd.Parameters["@PartsubtypePriceType_Abbr"].Value = this.Abr;
                cmd.Parameters["@PartsubtypePriceType_Description"].Value = this.Description;
                cmd.Parameters["@PartsubtypePriceType_IsActive"].Value = this.IsActive;
                cmd.Parameters["@PartsubtypePriceType_ShowInPriceList"].Value = this.IsShowInPrice;
                cmd.Parameters["@Currency_Guid"].Value = this.Currency.ID;
                cmd.Parameters["@PartsubtypePriceType_ColumnIdDefault"].Value = this.ColumnID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@PartsubtypePriceType_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания типа цены.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать тип цены.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeletePartsubtypePriceType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsubtypePriceType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@PartsubtypePriceType_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления типа цены.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить тип цены.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditPartsubtypePriceType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsubtypePriceType_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsubtypePriceType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsubtypePriceType_Abbr", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsubtypePriceType_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsubtypePriceType_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsubtypePriceType_ShowInPriceList", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PartsubtypePriceType_ColumnIdDefault", System.Data.SqlDbType.Int));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@PartsubtypePriceType_Guid"].Value = this.ID;
                cmd.Parameters["@PartsubtypePriceType_Name"].Value = this.Name;
                cmd.Parameters["@PartsubtypePriceType_Abbr"].Value = this.Abr;
                cmd.Parameters["@PartsubtypePriceType_Description"].Value = this.Description;
                cmd.Parameters["@PartsubtypePriceType_IsActive"].Value = this.IsActive;
                cmd.Parameters["@PartsubtypePriceType_ShowInPriceList"].Value = this.IsShowInPrice;
                cmd.Parameters["@Currency_Guid"].Value = this.Currency.ID;
                cmd.Parameters["@PartsubtypePriceType_ColumnIdDefault"].Value = this.ColumnID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения типа цены. Текст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства типа цены. Текст ошибки: " + f.Message, "Внимание",
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
}
