using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ERP_Mercury.Common
{
    /// <summary>
    /// Класс "Поставщик"
    /// </summary>
    public class CVendor : CBusinessObject
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
        // --------- Добавил 08.12.2011 -------
        /// <summary>
        /// Описание поставщика
        /// </summary>
        private System.String m_strDescription;
        /// <summary>
        /// Описание поставщика
        /// </summary>
        public System.String Description
        {
            get { return m_strDescription; }
            set { m_strDescription = value; }
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
        /// Тип постащика
        /// </summary>
        private CVendorType m_objTypeVendor;
        /// <summary>
        /// Тип поставщика
        /// </summary>
        public CVendorType TypeVendor
        {
            get { return m_objTypeVendor; }
            set { m_objTypeVendor = value; }
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

        // ~~~~~ поля ...XXXforDelete ~~~~~

        private List<CAddress> m_objAddressForDeleteList;
        public List<CAddress> AddressForDeleteList
        {
            get { return m_objAddressForDeleteList; }
            set { m_objAddressForDeleteList = value; }
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
        private const System.Int32 iCommandTimeOutForIB = 120;
        //-------------------------------------
        #endregion

        #region Конструктор
        public CVendor()
            : base()
        {
            m_ID_Ib = 0;
            m_bIsActive = false;
        }
        public CVendor(System.Guid uuidId, System.String strName, System.Int32 iID_Ib, System.Boolean bIsActive)
        {
            ID = uuidId;
            m_ID_Ib = iID_Ib;
            Name = strName;
            m_bIsActive = bIsActive;
        }
        public CVendor(System.Guid uuidId, System.String strName, System.Int32 iID_Ib, System.String strDescription, System.Boolean bIsActive, CVendorType objTypeVendor)
        {
            ID = uuidId;
            m_ID_Ib = iID_Ib;
            Name = strName;
            m_bIsActive = bIsActive;
            m_strDescription = strDescription;
            m_objTypeVendor = objTypeVendor;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список поставщиков
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список поставщиков</returns>
        public static List<CVendor> GetVendorList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CVendor> objList = new List<CVendor>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetVendor]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CVendor(
                            (System.Guid)rs["Vendor_Guid"],
                            (System.String)rs["Vendor_Name"],
                            System.Convert.ToInt32(rs["Vendor_Id"]),
                            System.Convert.ToBoolean(rs["Vendor_IsActive"])
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
                "Не удалось получить список поставщиков.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }

        /// <summary>
        /// Возвращает список поставщиков с дополнительными полями
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список поставщиков</returns>
        public static List<CVendor> GetVendorListForVendor(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CVendor> objList = new List<CVendor>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetVendorForVendor]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CVendor(
                            (System.Guid)rs["Vendor_Guid"],
                            (System.String)rs["Vendor_Name"],
                            System.Convert.ToInt32(rs["Vendor_Id"]),
                            ((rs["Vendor_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Vendor_Description"]),
                            System.Convert.ToBoolean(rs["Vendor_IsActive"]),
                             new CVendorType( (System.Guid)rs["VendorType_Guid"], 
                                             (System.String)rs["VendorType_Name"],
                                             ((rs["VendorType_Id"] == System.DBNull.Value) ? 0 : (System.Int32)rs["VendorType_Id"]),
                                             ((rs["VendorType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["VendorType_Description"]),
                                             (System.Boolean)rs["VendorType_IsActive"])
                                              
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
                "Не удалось получить список поставщиков.\n\nТекст ошибки: " + f.Message, "Внимание",
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

        #region IsAllParametersFullValid
        /// <summary>
        /// Проверка свойств поставщика перед сохранением
        /// </summary>
        /// <param name="strErr">текст с ошибкой</param>
        /// <returns>true - все свойства корректны; false - ошибка</returns>
        public System.Boolean IsAllParametersFullValid(ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (this.Name.Trim() == "")
                {
                    strErr = "Поставщик: Необходимо указать название поставщика!";
                    return bRet;
                }
                if (this.TypeVendor == null)
                {
                    strErr = "Поставщик: Необходимо указать тип поставщика!";
                    return bRet;
                }

                // Проверка указан ли главный р/с
                //var aa = this.AccountList[0].IsMain ; 
                int j=0;
                if ((this.AccountList != null) && (this.AccountList.Count > 0))
                {
                    for (int i = 0; i < this.AccountList.Count ; i++)
                    {
                        if (this.AccountList[i].IsMain == false)
                        {
                            j++;
                        }
                    }
                    if (this.AccountList.Count==j)
                    {
                        strErr = "Поставщик: Одному из р/с необходимо установить признак “главный р/с” ";
                        return bRet;
                    }
                    
                }


                /*
                if ((this.AddressList == null) || (this.AddressList.Count == 0))
                {
                    strErr = "Компания: Необходимо указать адрес!";
                    return bRet;
                }
                
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
                    strErr = "Поставщик: Необходимо указать расчетный счет поставщика!";
                    return bRet;
                }
                */
                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr = "Ошибка проверки свойств поставщика. Текст ошибки: " + f.Message;
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
                if (this.IsAllParametersFullValid(ref strErr) == false)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddVendor]", objProfile.GetOptionsDllDBName()); ;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_IsActive", System.Data.DbType.Boolean));
                
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@VendorType_Guid"].Value = this.TypeVendor.ID;
                cmd.Parameters["@Vendor_Description"].Value = this.Description ;
                cmd.Parameters["@Vendor_Name"].Value = this.Name;
                cmd.Parameters["@Vendor_IsActive"].Value = this.IsActive;
                
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    this.ID = (System.Guid)cmd.Parameters["@Vendor_Guid"].Value;
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
                        iRes = (CContact.SaveContactList(this.ContactList, null, EnumObject.Vendor, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1; 
                    }
                    if (iRes == 0)
                    {
                        if ((this.AddressList != null) && (this.AddressList.Count > 0) && (this.AddressForDeleteList != null))
                        {
                            foreach (CAddress objAddress in this.AddressList) { objAddress.ID = System.Guid.Empty; }
                            iRes = (CAddress.SaveAddressList(this.AddressList, null, EnumObject.Vendor, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                  
                    if (iRes == 0)
                    {
                        if ((this.AccountList != null) && (this.AccountList.Count > 0))
                        {
                            foreach (CAccount objAccount in this.AccountList) { objAccount.ID = System.Guid.Empty; }
                            iRes = (CAccount.SaveAccountList(this.AccountList, null, EnumObject.Vendor, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1; // сделал. остановился здесь
                        }
                    }

                    // теперь все это нужно записать в InterBase
                    // ** временно

                    if (iRes == 0)
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddVendorToIB]", objProfile.GetOptionsDllDBName()); ;
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Id", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Guid", System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                        cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                        cmd.Parameters["@Vendor_Guid"].Value = this.ID;
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
                                // Раскоментировать когда дойду до этого места 11.12.11
                                DeleteVendorFromIB(objProfile, cmd, ref strErr);
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
        #endregion

        #region Update
        /// <summary>
        /// Сохраняет в базе данных изменения в описании поставщика
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="objContactDeletedList">список удаленных контактов</param>
        /// <param name="objAddressDeletedList">список удаленных адресов</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Update(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            System.Boolean bRet = false;

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (this.IsAllParametersFullValid(ref strErr) == false)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditVendor]", objProfile.GetOptionsDllDBName()); ;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Id", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_IsActive", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Vendor_Guid"].Value = this.ID;
                cmd.Parameters["@Vendor_Name"].Value = this.Name;
                cmd.Parameters["@Vendor_Id"].Value = this.ID_Ib; 
                cmd.Parameters["@Vendor_Description"].Value = this.Description;
                cmd.Parameters["@Vendor_IsActive"].Value = this.IsActive;
                cmd.Parameters["@VendorType_Guid"].Value = this.TypeVendor.ID; 

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
                        iRes = (CContact.SaveContactList(this.ContactList, this.ContactForDeleteList, EnumObject.Vendor, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;    
                    }
                    if (iRes == 0)
                    {
                        if ((this.AddressList != null) || (this.AddressForDeleteList != null))
                        {
                            iRes = (CAddress.SaveAddressList(this.AddressList, this.AddressForDeleteList, EnumObject.Vendor, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                            iRes = (CAddress.SaveAddressList(this.AddressList, this.AddressForDeleteList, EnumObject.Contact, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                            //возможно нужно написать здесь такую же строчку, но передавать EnumObject.Contact
                        }
                    }
                    if (iRes == 0)
                    {
                        if ((this.AccountList != null) || (this.AccountForDeleteList != null))
                        {
                            iRes = (CAccount.SaveAccountList(this.AccountList, this.AccountForDeleteList, EnumObject.Vendor, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1; //*
                        }
                    }

                    // теперь все это нужно записать в InterBase
                    // нужно, но временно отключено
                    if (iRes == 0)
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditVendorToIB]", objProfile.GetOptionsDllDBName()); ;
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Guid", System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                        cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                        cmd.Parameters["@Vendor_Guid"].Value = this.ID;
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

        #endregion

        #region Remove
        /// <summary>
        /// Удалить запись из БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="uuidID">уникальный идентификатор объекта</param>
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
                System.Int32 iRes = DeleteVendorFromIB (objProfile, cmd, ref strErr);

                // закоментировать
                //System.Int32 iRes = 0;
              
                if (iRes == 0) // если всё OK
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteVendor]", objProfile.GetOptionsDllDBName());
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters["@Vendor_Guid"].Value = this.ID;
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
        /// Удаляем поставщика из InterBase
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>0 - удачное завершение операции; <>0 - ошибка</returns>
        private System.Int32 DeleteVendorFromIB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
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
                    cmdSQL.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteVendorFromIB]", objProfile.GetOptionsDllDBName()); ;
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmdSQL.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                    cmdSQL.Parameters["@Vendor_Guid"].Value = this.ID;
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
    /// Класс "Тип платежного документа"
    /// </summary>
    public class CVendorPaymentDocType : CBusinessObject
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
        public CVendorPaymentDocType()
            : base()
        {
            m_strDescription = "";
            m_bIsActive = false;
        }
        public CVendorPaymentDocType(System.Guid uuidId, System.String strName,  
            System.String strDescription, System.Boolean bIsActive)
        {
            ID = uuidId;
            Name = strName;
            m_strDescription = strDescription;
            m_bIsActive = bIsActive;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список типов платежных документов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список типов платежных документов</returns>
        public static List<CVendorPaymentDocType> GetVendorPaymentDocTypeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CVendorPaymentDocType> objList = new List<CVendorPaymentDocType>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetVendorPaymenDocType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CVendorPaymentDocType(
                            (System.Guid)rs["VendorPaymenDocType_Guid"],
                            (System.String)rs["VendorPaymenDocType_NAME"],
                            ((rs["VendorPaymenDocType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["VendorPaymenDocType_Description"]),
                            System.Convert.ToBoolean(rs["VendorPaymenDocType_IsActive"])
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
                "Не удалось получить список типов платежных документов.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddVendorPaymenDocType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymenDocType_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymenDocType_NAME", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymenDocType_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymenDocType_IsActive", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@VendorPaymenDocType_NAME"].Value = this.Name;
                cmd.Parameters["@VendorPaymenDocType_IsActive"].Value = this.IsActive;
                cmd.Parameters["@VendorPaymenDocType_Description"].Value = this.Description;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@VendorPaymenDocType_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания типа платежного документа.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать тип платежного документа.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditVendorPaymenDocType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymenDocType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymenDocType_NAME", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymenDocType_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymenDocType_IsActive", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@VendorPaymenDocType_Guid"].Value = this.ID;
                cmd.Parameters["@VendorPaymenDocType_NAME"].Value = this.Name;
                cmd.Parameters["@VendorPaymenDocType_IsActive"].Value = this.IsActive;
                cmd.Parameters["@VendorPaymenDocType_Description"].Value = this.Description;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств типа платежного документа.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства типа платежного документа.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteVendorPaymenDocType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymenDocType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@VendorPaymenDocType_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления описания типа платежного документа.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить описание типа платежного документа.\n\nТекст ошибки: " + f.Message, "Внимание",
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
    /// Класс "Тип договора с клиентом"
    /// </summary>
    public class CVendorContractType : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Признак "Предоставляется отсрочка платежа"
        /// </summary>
        [DisplayName("Предоставляется отсрочка платежа")]
        [Description("Возможность предоставления поставщиком отсрочки по оплате")]
        [Category("1. Обязательные значения")]
        [TypeConverter(typeof(BooleanTypeConverter))]
        [PropertyOrder(1)]
        public System.Boolean IsCanPaymentDelay { get; set; }
        /// <summary>
        /// Срок оплаты
        /// </summary>
        private System.Int32 m_CreditPeriodDay;
        /// <summary>
        /// Срок оплаты
        /// </summary>
        [DisplayName("Срок оплаты")]
        [Description("количество дней, в течение которых должен быть оплачен инвойс")]
        [Category("1. Обязательные значения")]
        [PropertyOrder(2)]
        public System.Int32 CreditPeriodDay
        {
            get { return m_CreditPeriodDay; }
            set { m_CreditPeriodDay = value; }
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
        [Category("2. Дополнительные значения")]
        [TypeConverter(typeof(BooleanTypeConverter))]
        [PropertyOrder(3)]
        public System.Boolean IsActive
        {
            get { return m_bIsActive; }
            set { m_bIsActive = value; }
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
        [Category("3. Необязательные значения")]
        public System.String Description
        {
            get { return m_strDescription; }
            set { m_strDescription = value; }
        }
        #endregion

        #region Конструктор
        public CVendorContractType()
            : base()
        {
            m_CreditPeriodDay = 0;
            m_strDescription = "";
            m_bIsActive = false;
            IsCanPaymentDelay = false;
        }
        public CVendorContractType(System.Guid uuidId, System.String strName, System.Int32 iCreditPeriodDay, 
            System.String strDescription, System.Boolean bIsActive)
        {
            ID = uuidId;
            m_CreditPeriodDay = iCreditPeriodDay;
            Name = strName;
            m_strDescription = strDescription;
            m_bIsActive = bIsActive;
            IsCanPaymentDelay = false;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список типов контрактов с поставщиком
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список типов контрактов с поставщиком</returns>
        public static List<CVendorContractType> GetVendorContractTypeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CVendorContractType> objList = new List<CVendorContractType>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetVendorContractType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CVendorContractType(
                            (System.Guid)rs["VendorContractType_Guid"],
                            (System.String)rs["VendorContractType_NAME"],
                            System.Convert.ToInt32(rs["VendorContractType_CreditPeriodDay"]),
                            ((rs["VendorContractType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["VendorContractType_Description"]),
                            System.Convert.ToBoolean(rs["VendorContractType_IsActive"])
                            ) { IsCanPaymentDelay = System.Convert.ToBoolean(rs["VendorContractType_IsCanPaymentDelay"]) }
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
                "Не удалось получить список типов контрактов с поставщиком.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddVendorContractType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContractType_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContractType_CreditPeriodDay", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContractType_NAME", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContractType_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContractType_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContractType_IsCanPaymentDelay", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@VendorContractType_NAME"].Value = this.Name;
                cmd.Parameters["@VendorContractType_CreditPeriodDay"].Value = this.CreditPeriodDay;
                cmd.Parameters["@VendorContractType_IsActive"].Value = this.IsActive;
                cmd.Parameters["@VendorContractType_IsCanPaymentDelay"].Value = this.IsCanPaymentDelay;
                cmd.Parameters["@VendorContractType_Description"].Value = this.Description;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@VendorContractType_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания типа договора с клиентом.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать тип договора с клиентом.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditVendorContractType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContractType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContractType_CreditPeriodDay", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContractType_NAME", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContractType_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContractType_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContractType_IsCanPaymentDelay", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@VendorContractType_Guid"].Value = this.ID;
                cmd.Parameters["@VendorContractType_NAME"].Value = this.Name;
                cmd.Parameters["@VendorContractType_CreditPeriodDay"].Value = this.CreditPeriodDay;
                cmd.Parameters["@VendorContractType_IsActive"].Value = this.IsActive;
                cmd.Parameters["@VendorContractType_IsCanPaymentDelay"].Value = this.IsCanPaymentDelay;
                cmd.Parameters["@VendorContractType_Description"].Value = this.Description;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств типа договора с клиентом.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства типа договора с клиентом.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteVendorContractType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContractType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@VendorContractType_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления описания типа договора с клиентом.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить описание типа договора с клиентом.\n\nТекст ошибки: " + f.Message, "Внимание",
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
    /// Класс "Вид отсрочки платежа"
    /// </summary>
    public class CVendorContractDelayType : CBusinessObject
    {
        #region Свойства
        [DisplayName("Активен")]
        [Description("Признак активности записи")]
        [Category("2. Дополнительные значения")]
        [TypeConverter(typeof(BooleanTypeConverter))]
        [PropertyOrder(3)]
        public System.Boolean IsActive {get; set;}
        /// <summary>
        /// Примечание
        /// </summary>
        [DisplayName("Примечание")]
        [Description("Примечание")]
        [Category("3. Необязательные значения")]
        public System.String Description {get; set;}
        #endregion

        #region Конструктор
        public CVendorContractDelayType()
            : base()
        {
            Description = "";
            IsActive = false;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список видов отсрочки платежа
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список видов отсрочки платежа</returns>
        public static List<CVendorContractDelayType> GetVendorContractDelayTypeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CVendorContractDelayType> objList = new List<CVendorContractDelayType>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetVendorContractDelayType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add( new CVendorContractDelayType() 
                            { 
                                ID = (System.Guid)rs["VendorContractDelayType_Guid"], 
                                Name = System.Convert.ToString(rs["VendorContractDelayType_NAME"]),
                                Description = ((rs["VendorContractDelayType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["VendorContractDelayType_Description"]), 
                                IsActive = System.Convert.ToBoolean(rs["VendorContractDelayType_IsActive"])

                            } );
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
                "Не удалось получить список видов отсрочки платежа.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddVendorContractDelayType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContractDelayType_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContractDelayType_NAME", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContractDelayType_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContractDelayType_IsActive", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@VendorContractDelayType_NAME"].Value = this.Name;
                cmd.Parameters["@VendorContractDelayType_IsActive"].Value = this.IsActive;
                cmd.Parameters["@VendorContractDelayType_Description"].Value = this.Description;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@VendorContractDelayType_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка регистрации вида отсрочки платежа.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось зарегистрировать вид отсрочки платежа.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditVendorContractDelayType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContractDelayType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContractDelayType_NAME", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContractDelayType_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContractDelayType_IsActive", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@VendorContractDelayType_Guid"].Value = this.ID;
                cmd.Parameters["@VendorContractDelayType_NAME"].Value = this.Name;
                cmd.Parameters["@VendorContractDelayType_IsActive"].Value = this.IsActive;
                cmd.Parameters["@VendorContractDelayType_Description"].Value = this.Description;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств вида отсрочки платежа.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства вида отсрочки платежа.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteVendorContractDelayType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContractDelayType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@VendorContractDelayType_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления описания вида отсрочки платежа.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить описание вида отсрочки платежа.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }

        #endregion

        #region Расчёт даты платежа
        /// <summary>
        /// Расчёт даты оплаты
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="VendorContractDelayType_Guid">УИ вида отсрочки</param>
        /// <param name="VendorInvoice_BeginDate">дата инвойса</param>
        /// <param name="VendorOrderList_ShipDate">дата отгрузки заказа</param>
        /// <param name="VendorOrder_ArrivalToStockDate">дата поступления заказа на склад приёмки товара</param>
        /// <param name="CreditPeriod">дней отсрочки платежа</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="iRet">код возврата хранимой процедуры</param>
        /// <returns>дата платежа</returns>
        public static System.DateTime GetVendorOrderPaymentDate(UniXP.Common.CProfile objProfile, 
            System.Guid VendorContractDelayType_Guid, System.DateTime VendorInvoice_BeginDate,
	        System.DateTime VendorOrderList_ShipDate, System.DateTime VendorOrder_ArrivalToStockDate,
	        System.Int32 CreditPeriod, ref System.String strErr, ref System.Int32 iRet )
        {
            System.DateTime dtRet = System.DateTime.MinValue;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    strErr += ("Не удалось получить соединение с базой данных.");
                    iRet = -1;
                    return dtRet;
                }
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetVendorInvoicePaymentDateByDelayType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Payment_Date", System.Data.SqlDbType.DateTime));
                cmd.Parameters["@Payment_Date"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContractDelayType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorInvoice_BeginDate", System.Data.SqlDbType.DateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorOrderList_ShipDate", System.Data.SqlDbType.DateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorOrder_ArrivalToStockDate", System.Data.SqlDbType.DateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CreditPeriod", System.Data.SqlDbType.Int));

                cmd.Parameters["@VendorContractDelayType_Guid"].Value = VendorContractDelayType_Guid;
                cmd.Parameters["@VendorInvoice_BeginDate"].Value = VendorInvoice_BeginDate;
                cmd.Parameters["@VendorOrderList_ShipDate"].Value = VendorOrderList_ShipDate;
                cmd.Parameters["@VendorOrder_ArrivalToStockDate"].Value = VendorOrder_ArrivalToStockDate;
                cmd.Parameters["@CreditPeriod"].Value = CreditPeriod;

                cmd.ExecuteNonQuery();
                iRet = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;

                if (iRet == 0)
                {
                    dtRet = System.Convert.ToDateTime(cmd.Parameters["@Payment_Date"].Value);
                }

                cmd.Dispose();
                DBConnection.Close();
            }
            catch (System.Exception f)
            {
                strErr += (f.Message);
            }
            return dtRet;
        }
        #endregion

        public override string ToString()
        {
            return Name;
        }
    }
    
    /// <summary>
    /// Класс "Договор с поставщиком"
    /// </summary>
    public class CVendorContract
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
        /// Номер договора
        /// </summary>
        private System.String m_strNumber;
        /// <summary>
        /// Номер договора
        /// </summary>
        public System.String Number
        {
            get { return m_strNumber; }
            set { m_strNumber = value; }
        }
        /// <summary>
        /// Начало действия договора
        /// </summary>
        private System.DateTime m_dtBeginDate;
        /// <summary>
        /// Начало действия договора
        /// </summary>
        public System.DateTime BeginDate
        {
            get { return m_dtBeginDate; }
            set { m_dtBeginDate = value; }
        }
        /// <summary>
        /// Окончание действия договора
        /// </summary>
        private System.DateTime m_dtEndDate;
        /// <summary>
        /// Окончание действия договора
        /// </summary>
        public System.DateTime EndDate
        {
            get { return m_dtEndDate; }
            set { m_dtEndDate = value; }
        }
        /// <summary>
        /// Срок оплаты
        /// </summary>
        private System.Int32 m_CreditPeriodDay;
        /// <summary>
        /// Срок оплаты
        /// </summary>
        public System.Int32 CreditPeriodDay
        {
            get { return m_CreditPeriodDay; }
            set { m_CreditPeriodDay = value; }
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
        /// Тип договора
        /// </summary>
        private CVendorContractType m_objVendorContractType;
        /// <summary>
        /// Тип договора
        /// </summary>
        public CVendorContractType VendorContractType
        {
            get { return m_objVendorContractType; }
            set { m_objVendorContractType = value; }
        }
        /// <summary>
        /// Поставщик
        /// </summary>
        private CVendor m_objVendor;
        /// <summary>
        /// Поставщик
        /// </summary>
        public CVendor Vendor
        {
            get { return m_objVendor; }
            set { m_objVendor = value; }
        }
        /// <summary>
        /// Наименование поставщика
        /// </summary>
        public System.String VendorName
        {
            get { return (m_objVendor == null ? "" : m_objVendor.Name); }
        }
        /// <summary>
        /// Наименование типа договора
        /// </summary>
        public System.String VendorContractTypeName
        {
            get { return (m_objVendorContractType == null ? "" : m_objVendorContractType.Name); }
        }
        public System.String VendorContractFullName
        {
            get 
            {
                System.String strVendorName = this.VendorName;
                System.String strVendorContractTypeName = this.VendorContractTypeName;

                return (strVendorName + " " + this.Number); 
            }
        }
        /// <summary>
        /// Вид отсрочки платежа
        /// </summary>
        public CVendorContractDelayType VendorContractDelayType { get; set; }
        /// <summary>
        /// Срок договора истёк
        /// </summary>
        public System.Boolean IsDelay
        {
            get { return (System.DateTime.Compare(m_dtEndDate, System.DateTime.Today) < 0); }
        }
        #endregion

        #region Конструктор
        public CVendorContract()
        {
            m_uuidID = System.Guid.Empty;
            m_strNumber = "";
            m_objVendorContractType = null;
            m_dtBeginDate = System.DateTime.Today;
            m_dtEndDate = System.DateTime.Today;
            m_CreditPeriodDay = 0;
            m_strDescription = "";
            m_bIsActive = false;
            m_objVendor = null;
            VendorContractDelayType = null;
        }
        public CVendorContract(System.Guid uuidID, System.String strNumber, System.DateTime dtBeginDate, System.DateTime dtEndDate,
            System.Int32 iCreditPeriodDay, System.String strDescription, System.Boolean bIsActive, CVendorContractType objVendorContractType,
            CVendor objVendor, CVendorContractDelayType objVendorContractDelayType)
        {
            m_uuidID = uuidID;
            m_strNumber = strNumber;
            m_objVendorContractType = objVendorContractType;
            m_dtBeginDate = dtBeginDate;
            m_dtEndDate = dtEndDate;
            m_CreditPeriodDay = iCreditPeriodDay;
            m_strDescription = strDescription;
            m_bIsActive = bIsActive;
            m_objVendor = objVendor;
            VendorContractDelayType = objVendorContractDelayType;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список контрактов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objVendor">поставщик</param>
        /// <returns>список контрактов</returns>
        public static List<CVendorContract> GetVendorContractList(UniXP.Common.CProfile objProfile, 
            System.Data.SqlClient.SqlCommand cmdSQL, CVendor objVendor )
        {
            List<CVendorContract> objList = new List<CVendorContract>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetVendorContract]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                if (objVendor != null)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@Vendor_Guid"].Value = objVendor.ID;
                }
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CVendorContractType objVendorContractType = null;
                    CVendorContractDelayType objVendorContractDelayType = null;
                    while (rs.Read())
                    {
                        objVendorContractType = new CVendorContractType(
                            (System.Guid)rs["VendorContractType_Guid"],
                            (System.String)rs["VendorContractType_NAME"],
                            System.Convert.ToInt32(rs["VendorContractType_CreditPeriodDay"]),
                            ((rs["VendorContractType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["VendorContractType_Description"]),
                            System.Convert.ToBoolean(rs["VendorContractType_IsActive"])
                            );

                        if (rs["VendorContractDelayType_Guid"] == System.DBNull.Value)
                        {
                            objVendorContractDelayType = null;
                        }
                        else
                        {
                            objVendorContractDelayType = new CVendorContractDelayType()
                            {
                                ID = (System.Guid)rs["VendorContractDelayType_Guid"],
                                Name = System.Convert.ToString(rs["VendorContractDelayType_NAME"]),
                                Description = ((rs["VendorContractDelayType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["VendorContractDelayType_Description"]),
                                IsActive = System.Convert.ToBoolean(rs["VendorContractDelayType_IsActive"])
                            };
                        }


                        objList.Add(new CVendorContract(
                            (System.Guid)rs["VendorContract_Guid"],
                            (System.String)rs["VendorContract_Num"],
                            System.Convert.ToDateTime( rs["VendorContract_BeginDate"] ),
                            System.Convert.ToDateTime( rs["VendorContract_EndDate"] ),
                            System.Convert.ToInt32(rs["VendorContract_CreditPeriod"]),
                            ((rs["VendorContract_Description"] == System.DBNull.Value) ? "" : (System.String)rs["VendorContract_Description"]),
                            System.Convert.ToBoolean(rs["VendorContract_IsActive"]), objVendorContractType,
                            new CVendor(
                            (System.Guid)rs["Vendor_Guid"],
                            (System.String)rs["Vendor_Name"],
                            System.Convert.ToInt32(rs["Vendor_Id"]),
                            System.Convert.ToBoolean(rs["Vendor_IsActive"])
                            ), objVendorContractDelayType
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
                "Не удалось получить список контрактов с поставщиком.\n\nТекст ошибки: " + f.Message, "Внимание",
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
        public System.Boolean IsAllParametersValid()
        {
            System.Boolean bRet = false;
            try
            {
                if (this.Number == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать номер договора!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.BeginDate > this.EndDate)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Дата начала действия договора не должна превышать дату окончания действия договора.", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if ( this.VendorContractType == null )
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать тип договора!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if ((VendorContractDelayType != null) && (this.CreditPeriodDay <= 0))
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Пожалуйста, укажите срок платежа.", "Внимание",
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
        /// Добавляет в базу данных информацию о контракте с поставщиком
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="objVendor">поставщик</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean AddContractToVendor(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            ref System.String strErr )
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (this.IsAllParametersValid() == false)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddVendorContract]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContract_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContract_CreditPeriod", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContract_Num", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContract_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContract_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContractType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContract_BeginDate", System.Data.SqlDbType.SmallDateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContract_EndDate", System.Data.SqlDbType.SmallDateTime));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@VendorContract_CreditPeriod"].Value = this.CreditPeriodDay;
                cmd.Parameters["@VendorContract_Num"].Value = this.Number;
                cmd.Parameters["@VendorContract_IsActive"].Value = this.IsActive;
                cmd.Parameters["@VendorContract_Description"].Value = this.Description;
                cmd.Parameters["@VendorContract_BeginDate"].Value = this.BeginDate;
                cmd.Parameters["@VendorContract_EndDate"].Value = this.EndDate;
                cmd.Parameters["@VendorContractType_Guid"].Value = this.VendorContractType.ID;
                cmd.Parameters["@Vendor_Guid"].Value = this.Vendor.ID;

                if ((this.VendorContractDelayType != null) && (this.VendorContractDelayType.ID.CompareTo(System.Guid.Empty) != 0))
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContractDelayType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@VendorContractDelayType_Guid"].Value = this.VendorContractDelayType.ID;
                }

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    this.ID = (System.Guid)cmd.Parameters["@VendorContract_Guid"].Value;
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

        #region Update
        /// <summary>
        /// Изменяет реквизиты поставщика в базе данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="objVendor">поставщик</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean EditContractForVendor(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (this.IsAllParametersValid() == false)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditVendorContract]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContract_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContract_CreditPeriod", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContract_Num", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContract_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContract_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContractType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContract_BeginDate", System.Data.SqlDbType.SmallDateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContract_EndDate", System.Data.SqlDbType.SmallDateTime));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@VendorContract_Guid"].Value = this.ID;
                cmd.Parameters["@VendorContract_CreditPeriod"].Value = this.CreditPeriodDay;
                cmd.Parameters["@VendorContract_Num"].Value = this.Number;
                cmd.Parameters["@VendorContract_IsActive"].Value = this.IsActive;
                cmd.Parameters["@VendorContract_Description"].Value = this.Description;
                cmd.Parameters["@VendorContract_BeginDate"].Value = this.BeginDate;
                cmd.Parameters["@VendorContract_EndDate"].Value = this.EndDate;
                cmd.Parameters["@VendorContractType_Guid"].Value = this.VendorContractType.ID;
                cmd.Parameters["@Vendor_Guid"].Value = this.Vendor.ID;
                if ((this.VendorContractDelayType != null) && (this.VendorContractDelayType.ID.CompareTo(System.Guid.Empty) != 0))
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContractDelayType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@VendorContractDelayType_Guid"].Value = this.VendorContractDelayType.ID;
                }
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

        #region Remove
        /// <summary>
        /// Удаляет контракт из базы данных
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
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteVendorContract]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContract_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@VendorContract_Guid"].Value = this.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
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
            return ( VendorContractFullName + " срок оплаты: " + CreditPeriodDay.ToString() );
        }

    }
    /// <summary>
    /// Класс "Инвойс поставщика"
    /// </summary>
    public class CVendorInvoice
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
        /// Номер инвойса
        /// </summary>
        private System.String m_strNumber;
        /// <summary>
        /// Номер инвойса
        /// </summary>
        public System.String Number
        {
            get { return m_strNumber; }
            set { m_strNumber = value; }
        }
        /// <summary>
        /// Дата инвойса
        /// </summary>
        private System.DateTime m_dtBeginDate;
        /// <summary>
        /// Дата инвойса
        /// </summary>
        public System.DateTime BeginDate
        {
            get { return m_dtBeginDate; }
            set { m_dtBeginDate = value; }
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
        /// Сумма инвойса
        /// </summary>
        private double m_AllMoney;
        /// <summary>
        /// Сумма инвойса
        /// </summary>
        public double AllMoney
        {
            get { return m_AllMoney; }
            set { m_AllMoney = value; }
        }
        /// <summary>
        /// Срок оплаты
        /// </summary>
        private System.Int32 m_CreditPeriodDay;
        /// <summary>
        /// Срок оплаты
        /// </summary>
        public System.Int32 CreditPeriodDay
        {
            get { return m_CreditPeriodDay; }
            set { m_CreditPeriodDay = value; }
        }
        /// <summary>
        /// Срок платежа
        /// </summary>
        private System.DateTime m_dtPaymentDate;
        /// <summary>
        /// Срок платежа
        /// </summary>
        public System.DateTime PaymentDate
        {
            get { return m_dtPaymentDate; }
            set { m_dtPaymentDate = value; }
        }
        /// <summary>
        /// Договор
        /// </summary>
        private CVendorContract m_objVendorContract;
        /// <summary>
        /// Договор
        /// </summary>
        public CVendorContract VendorContract
        {
            get { return m_objVendorContract; }
            set { m_objVendorContract = value; }
        }
        /// <summary>
        /// Заказ
        /// </summary>
        private CVendorOrder m_objVendorOrder;
        /// <summary>
        /// Заказ
        /// </summary>
        public CVendorOrder VendorOrder
        {
            get { return m_objVendorOrder; }
            set { m_objVendorOrder = value; }
        }
        /// <summary>
        /// Поставщик
        /// </summary>
        private CVendor m_objVendor;
        /// <summary>
        /// Поставщик
        /// </summary>
        public CVendor Vendor
        {
            get { return m_objVendor; }
            set { m_objVendor = value; }
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
        /// <summary>
        /// Наименование поставщика
        /// </summary>
        public System.String VendorName
        {
            get { return (m_objVendor == null ? "" : m_objVendor.Name); }
        }
        /// <summary>
        /// Наименование валюты
        /// </summary>
        public System.String CurrencyName
        {
            get { return (m_objCurrency == null ? "" : m_objCurrency.CurrencyAbbr); }
        }
        public System.String VendorInvoiceFullName
        {
            get
            {
                System.String strVendorName = this.VendorName;

                return (strVendorName + " №" + this.Number + " сумма: " + this.AllMoney.ToString() + " " + this.CurrencyName);
            }
        }
        /// <summary>
        /// Дата отгрузки заказа
        /// </summary>
        public System.DateTime OrderShipDate
        {
            get { return (m_objVendorOrder == null ? System.DateTime.Today : m_objVendorOrder.ShipDate); }
        }
        /// <summary>
        /// № заказа
        /// </summary>
        public System.String OrderNum
        {
            get { return (m_objVendorOrder == null ? "" : m_objVendorOrder.Number + " " + (m_objVendorOrder.Company == null ? "" : m_objVendorOrder.Company.Abbr)); }
        }
        /// <summary>
        /// Сумма заказа
        /// </summary>
        public double OrderSum
        {
            get { return (m_objVendorOrder == null ? 0 : m_objVendorOrder.AllMoney); }
        }
        /// <summary>
        /// Оплачено
        /// </summary>
        private double m_AllPaymentMoney;
        /// <summary>
        /// Оплачено
        /// </summary>
        public double PaymentMoney
        {
            get { return m_AllPaymentMoney; }
            set { m_AllPaymentMoney = value; }
        }
        public double SaldoMoney
        {
            get { return ( m_AllMoney - m_AllPaymentMoney ); }
        }
        /// <summary>
        /// Компания
        /// </summary>
        public System.String CompanyName
        {
            get { return (m_objVendorOrder == null ? "" : m_objVendorOrder.CompanyName); }
        }
        #endregion

        #region Конструктор
        public CVendorInvoice()
        {
            m_uuidID = System.Guid.Empty;
            m_strNumber = "";
            m_dtBeginDate = System.DateTime.Today;
            m_objCurrency = null;
            m_AllMoney = 0;
            m_objVendor = null;
            m_strDescription = "";
            m_CreditPeriodDay = 0;
            m_dtPaymentDate = System.DateTime.Today;
            m_objVendorContract = null;
            m_objVendorOrder = null;
            m_AllPaymentMoney = 0;
        }
        public CVendorInvoice(System.Guid uuidID, System.String strNumber, System.DateTime dtBeginDate,
            CCurrency objCurrency, double doubleAllMoney, CVendor objVendor, System.String strDescription,
            System.Int32 iCreditPeriodDay, System.DateTime dtPaymentDate, CVendorContract objVendorContract,
            CVendorOrder objVendorOrder, double doubleAllPaymentMoney)
        {
            m_uuidID = uuidID;
            m_strNumber = strNumber;
            m_dtBeginDate = dtBeginDate;
            m_objCurrency = objCurrency;
            m_AllMoney = doubleAllMoney;
            m_objVendor = objVendor;
            m_strDescription = strDescription;
            m_CreditPeriodDay = iCreditPeriodDay;
            m_dtPaymentDate = dtPaymentDate;
            m_objVendorContract = objVendorContract;
            m_objVendorOrder = objVendorOrder;
            m_AllPaymentMoney = doubleAllPaymentMoney;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список инвойсов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objVendor">поставщик</param>
        /// <returns>список инвойсов</returns>
        public static List<CVendorInvoice> GetVendorInvoiceList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, CVendor objVendor, System.DateTime dtBeginDate, 
            System.DateTime dtEndDate)
        {
            List<CVendorInvoice> objList = new List<CVendorInvoice>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetVendorInvoice]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                if (objVendor != null)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@Vendor_Guid"].Value = objVendor.ID;
                }
                if ((dtBeginDate != null) && (dtBeginDate != System.DateTime.MinValue))
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BeginDate", System.Data.SqlDbType.DateTime));
                    cmd.Parameters["@BeginDate"].Value = dtBeginDate;
                }
                if ((dtEndDate != null) && (dtEndDate != System.DateTime.MinValue))
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EndDate", System.Data.SqlDbType.DateTime));
                    cmd.Parameters["@EndDate"].Value = dtEndDate;
                }
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CCurrency objCurrency = null;
                    CVendorContract objVendorContract = null;
                    CVendorOrder objVendorOrder = null;
                    CVendor obVendor = null;

                    while (rs.Read())
                    {
                        objCurrency = new CCurrency((System.Guid)rs["Currency_Guid"], (System.String)rs["Currency_Name"],
                                                    (System.String)rs["Currency_Abbr"], (System.String)rs["Currency_Code"]);
                        obVendor = new CVendor(
                            (System.Guid)rs["Vendor_Guid"],
                            (System.String)rs["Vendor_Name"],
                            System.Convert.ToInt32(rs["Vendor_Id"]),
                            System.Convert.ToBoolean(rs["Vendor_IsActive"]));

                        objVendorContract = new CVendorContract();
                        objVendorContract.ID = (System.Guid)rs["VendorContract_Guid"];
                        objVendorContract.Number = (System.String)rs["VendorContract_Num"];
                        objVendorContract.BeginDate = System.Convert.ToDateTime( rs["VendorContract_BeginDate"] );
                        objVendorContract.EndDate = System.Convert.ToDateTime( rs["VendorContract_EndDate"] );
                        objVendorContract.CreditPeriodDay = System.Convert.ToInt32(rs["VendorContract_CreditPeriod"]);
                        objVendorContract.Vendor = obVendor;

                        objVendorOrder = new CVendorOrder();
                        objVendorOrder.ID = (System.Guid)rs["VendorOrderList_Guid"];
                        objVendorOrder.Number = (System.String)rs["VendorOrderList_OrderNum"];
                        objVendorOrder.ShipDate = System.Convert.ToDateTime(rs["VendorOrderList_ShipDate"]);
                        objVendorOrder.AllMoney = System.Convert.ToDouble(rs["VendorOrderList_AllMoney"]);
                        objVendorOrder.Company = new CCompany((System.Guid)rs["Company_Guid"],
                            (System.String)rs["Company_Name"], (System.String)rs["Company_Acronym"]);

                        objList.Add(new CVendorInvoice(
                        (System.Guid)rs["VendorInvoice_Guid"],
                        (System.String)rs["VendorInvoice_Num"],
                        System.Convert.ToDateTime(rs["VendorInvoice_BeginDate"]), objCurrency,
                        System.Convert.ToDouble(rs["VendorInvoice_AllMoney"]),
                        obVendor,
                        ((rs["VendorInvoice_Description"] == System.DBNull.Value) ? "" : (System.String)rs["VendorInvoice_Description"]),
                        System.Convert.ToInt32(rs["VendorInvoice_CreditPeriod"]),
                        System.Convert.ToDateTime(rs["VendorInvoice_PaymentDate"]),
                        objVendorContract, objVendorOrder,
                        System.Convert.ToDouble(rs["VendorInvoicePaymentSum"])
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
                "Не удалось получить список инвойсов, выставленных поставщиком.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Возвращает список инвойсов для заданного заказа
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objVendorOrderSrc">заказа поставщику</param>
        /// <returns>список инвойсов</returns>
        public static List<CVendorInvoice> GetVendorInvoiceListForOrder(UniXP.Common.CProfile objProfile,
        System.Data.SqlClient.SqlCommand cmdSQL, CVendorOrder objVendorOrderSrc)
        {
            List<CVendorInvoice> objList = new List<CVendorInvoice>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetVendorInvoiceForOrder]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorOrder_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@VendorOrder_Guid"].Value = objVendorOrderSrc.ID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CCurrency objCurrency = null;
                    CVendorContract objVendorContract = null;
                    CVendorOrder objVendorOrder = null;
                    CVendor obVendor = null;

                    while (rs.Read())
                    {
                        objCurrency = new CCurrency((System.Guid)rs["Currency_Guid"], (System.String)rs["Currency_Name"],
                                                    (System.String)rs["Currency_Abbr"], (System.String)rs["Currency_Code"]);
                        obVendor = new CVendor(
                            (System.Guid)rs["Vendor_Guid"],
                            (System.String)rs["Vendor_Name"],
                            System.Convert.ToInt32(rs["Vendor_Id"]),
                            System.Convert.ToBoolean(rs["Vendor_IsActive"]));

                        objVendorContract = new CVendorContract();
                        objVendorContract.ID = (System.Guid)rs["VendorContract_Guid"];
                        objVendorContract.Number = (System.String)rs["VendorContract_Num"];
                        objVendorContract.BeginDate = System.Convert.ToDateTime(rs["VendorContract_BeginDate"]);
                        objVendorContract.EndDate = System.Convert.ToDateTime(rs["VendorContract_EndDate"]);
                        objVendorContract.CreditPeriodDay = System.Convert.ToInt32(rs["VendorContract_CreditPeriod"]);
                        objVendorContract.Vendor = obVendor;

                        objVendorOrder = new CVendorOrder();
                        objVendorOrder.ID = (System.Guid)rs["VendorOrderList_Guid"];
                        objVendorOrder.Number = (System.String)rs["VendorOrderList_OrderNum"];
                        objVendorOrder.ShipDate = System.Convert.ToDateTime(rs["VendorOrderList_ShipDate"]);
                        objVendorOrder.AllMoney = System.Convert.ToDouble(rs["VendorOrderList_AllMoney"]);

                        objList.Add(new CVendorInvoice(
                        (System.Guid)rs["VendorInvoice_Guid"],
                        (System.String)rs["VendorInvoice_Num"],
                        System.Convert.ToDateTime(rs["VendorInvoice_BeginDate"]), objCurrency,
                        System.Convert.ToDouble(rs["VendorInvoice_AllMoney"]),
                        obVendor,
                        ((rs["VendorInvoice_Description"] == System.DBNull.Value) ? "" : (System.String)rs["VendorInvoice_Description"]),
                        System.Convert.ToInt32(rs["VendorInvoice_CreditPeriod"]),
                        System.Convert.ToDateTime(rs["VendorInvoice_PaymentDate"]),
                        objVendorContract, objVendorOrder,
                        System.Convert.ToDouble(rs["VendorInvoicePaymentSum"])
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
                "Не удалось получить список инвойсовля указанного заказа.\n\nТекст ошибки: " + f.Message, "Внимание",
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
        public System.Boolean IsAllParametersValid()
        {
            System.Boolean bRet = false;
            try
            {
                if (this.Number == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать номер договора!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.m_AllMoney <= 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать срок платежа!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.m_objCurrency == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать валюту!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.m_objVendorContract == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать договор!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.m_objVendorOrder == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать заказ!", "Внимание",
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
        /// Добавляет в базу данных информацию об инвойсе, выставленном поставщиком
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="objVendor">поставщик</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean AddInvoiceToVendor(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (this.IsAllParametersValid() == false)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddVendorInvoice]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorInvoice_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorInvoice_AllMoney", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorInvoice_Num", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorInvoice_BeginDate", System.Data.SqlDbType.SmallDateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorInvoice_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContract_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorOrderList_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorInvoice_CreditPeriod", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorInvoice_PaymentDate", System.Data.SqlDbType.SmallDateTime));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@VendorInvoice_Num"].Value = this.Number;
                cmd.Parameters["@VendorInvoice_AllMoney"].Value = System.Convert.ToDecimal( this.m_AllMoney );
                cmd.Parameters["@Currency_Guid"].Value = this.Currency.ID;
                cmd.Parameters["@Vendor_Guid"].Value = this.Vendor.ID;
                cmd.Parameters["@VendorInvoice_BeginDate"].Value = this.BeginDate;
                cmd.Parameters["@VendorInvoice_Description"].Value = this.Description;
                cmd.Parameters["@VendorInvoice_PaymentDate"].Value = this.PaymentDate;
                cmd.Parameters["@VendorContract_Guid"].Value = this.VendorContract.ID;
                cmd.Parameters["@VendorOrderList_Guid"].Value = this.VendorOrder.ID;
                cmd.Parameters["@VendorInvoice_CreditPeriod"].Value = this.CreditPeriodDay;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    this.ID = (System.Guid)cmd.Parameters["@VendorInvoice_Guid"].Value;
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

        #region Update
        /// <summary>
        /// Изменяет информацию в инвойсе в базе данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="objVendor">поставщик</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean EditInvoiceForVendor(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (this.IsAllParametersValid() == false)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditVendorInvoice]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorInvoice_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorInvoice_AllMoney", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorInvoice_Num", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorInvoice_BeginDate", System.Data.SqlDbType.SmallDateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorInvoice_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContract_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorOrderList_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorInvoice_CreditPeriod", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorInvoice_PaymentDate", System.Data.SqlDbType.SmallDateTime));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@VendorInvoice_Guid"].Value = this.ID;
                cmd.Parameters["@VendorInvoice_Num"].Value = this.Number;
                cmd.Parameters["@VendorInvoice_AllMoney"].Value = System.Convert.ToDecimal(this.m_AllMoney);
                cmd.Parameters["@Currency_Guid"].Value = this.Currency.ID;
                cmd.Parameters["@Vendor_Guid"].Value = this.Vendor.ID;
                cmd.Parameters["@VendorInvoice_BeginDate"].Value = this.BeginDate;
                cmd.Parameters["@VendorInvoice_Description"].Value = this.Description;
                cmd.Parameters["@VendorInvoice_PaymentDate"].Value = this.PaymentDate;
                cmd.Parameters["@VendorContract_Guid"].Value = this.VendorContract.ID;
                cmd.Parameters["@VendorOrderList_Guid"].Value = this.VendorOrder.ID;
                cmd.Parameters["@VendorInvoice_CreditPeriod"].Value = this.CreditPeriodDay;
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

        #region Remove
        /// <summary>
        /// Удаляет инвойс из базы данных
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
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteVendorInvoice]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorInvoice_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@VendorInvoice_Guid"].Value = this.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
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
            return (VendorInvoiceFullName);
        }
    }
    /// <summary>
    /// Класс "Платежный документ"
    /// </summary>
    public class CVendorPaymentDoc
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
        /// Номер 
        /// </summary>
        private System.String m_strNumber;
        /// <summary>
        /// Номер 
        /// </summary>
        public System.String Number
        {
            get { return m_strNumber; }
            set { m_strNumber = value; }
        }
        /// <summary>
        /// Дата 
        /// </summary>
        private System.DateTime m_dtBeginDate;
        /// <summary>
        /// Дата 
        /// </summary>
        public System.DateTime BeginDate
        {
            get { return m_dtBeginDate; }
            set { m_dtBeginDate = value; }
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
        /// Сумма 
        /// </summary>
        private double m_AllMoney;
        /// <summary>
        /// Сумма 
        /// </summary>
        public double AllMoney
        {
            get { return m_AllMoney; }
            set { m_AllMoney = value; }
        }
        /// <summary>
        /// Тип документа
        /// </summary>
        private CVendorPaymentDocType m_objVendorPaymentDocType;
        /// <summary>
        /// Тип документа
        /// </summary>
        public CVendorPaymentDocType VendorPaymentDocType
        {
            get { return m_objVendorPaymentDocType; }
            set { m_objVendorPaymentDocType = value; }
        }
        /// <summary>
        /// Инвойс
        /// </summary>
        private CVendorInvoice m_objVendorInvoice;
        /// <summary>
        /// Инвойс
        /// </summary>
        public CVendorInvoice VendorInvoice
        {
            get { return m_objVendorInvoice; }
            set { m_objVendorInvoice = value; }
        }
        /// <summary>
        /// Поставщик
        /// </summary>
        private CVendor m_objVendor;
        /// <summary>
        /// Поставщик
        /// </summary>
        public CVendor Vendor
        {
            get { return m_objVendor; }
            set { m_objVendor = value; }
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
        /// <summary>
        /// Тип документа
        /// </summary>
        public System.String DocTypeName
        {
            get { return (m_objVendorPaymentDocType == null ? "" : m_objVendorPaymentDocType.Name); }
        }
        /// <summary>
        /// Наименование поставщика
        /// </summary>
        public System.String VendorName
        {
            get { return (m_objVendor == null ? "" : m_objVendor.Name); }
        }
        /// <summary>
        /// Наименование валюты
        /// </summary>
        public System.String CurrencyName
        {
            get { return (m_objCurrency == null ? "" : m_objCurrency.CurrencyAbbr); }
        }
        public System.String VendorPaymentDocFullName
        {
            get
            {
                System.String strVendorName = this.VendorName;

                return (strVendorName + " " + DocTypeName + " №" + this.Number + " сумма: " + this.AllMoney.ToString() + " " + this.CurrencyName);
            }
        }
        /// <summary>
        /// Дата инвойса
        /// </summary>
        public System.DateTime InvoiceBeginDate
        {
            get { return (m_objVendorInvoice == null ? System.DateTime.Today : m_objVendorInvoice.BeginDate); }
        }
        /// <summary>
        /// <summary>
        /// № инвойса
        /// </summary>
        public System.String InvoiceNum
        {
            get { return (m_objVendorInvoice == null ? "" : m_objVendorInvoice.Number); }
        }
        /// Дата отгрузки заказа
        /// </summary>
        public System.DateTime OrderShipDate
        {
            get { return (m_objVendorInvoice.VendorOrder == null ? System.DateTime.Today : m_objVendorInvoice.VendorOrder.ShipDate); }
        }
        /// <summary>
        /// № заказа
        /// </summary>
        public System.String OrderNum
        {
            get { return (m_objVendorInvoice.VendorOrder == null ? "" : m_objVendorInvoice.VendorOrder.Number + " " + (m_objVendorInvoice.VendorOrder.Company == null ? "" : m_objVendorInvoice.VendorOrder.Company.Abbr)); }
        }
        #endregion

        #region Конструктор
        public CVendorPaymentDoc()
        {
            m_uuidID = System.Guid.Empty;
            m_strNumber = "";
            m_dtBeginDate = System.DateTime.Today;
            m_objCurrency = null;
            m_AllMoney = 0;
            m_objVendor = null;
            m_objVendorPaymentDocType = null;
            m_strDescription = "";
            m_objVendorInvoice = null;
        }
        public CVendorPaymentDoc( System.Guid uuidID, System.String strNumber, System.DateTime dtBeginDate,
            CVendorPaymentDocType objVendorPaymentDocType, CCurrency objCurrency, double doubleAllMoney,
            CVendor objVendor, System.String strDescription, CVendorInvoice objVendorInvoice )
        {
            m_uuidID = uuidID;
            m_strNumber = strNumber;
            m_dtBeginDate = dtBeginDate;
            m_objCurrency = objCurrency;
            m_AllMoney = doubleAllMoney;
            m_objVendor = objVendor;
            m_strDescription = strDescription;
            m_objVendorInvoice = objVendorInvoice;
            m_objVendorPaymentDocType = objVendorPaymentDocType;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список платежных документов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objVendor">поставщик</param>
        /// <returns>список платежных документов</returns>
        public static List<CVendorPaymentDoc> GetVendorPaymentDocList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, CVendor objVendor, System.DateTime dtBeginDate, 
            System.DateTime dtEndDate )
        {
            List<CVendorPaymentDoc> objList = new List<CVendorPaymentDoc>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetVendorPaymentDoc]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BeginDate", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EndDate", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@BeginDate"].Value = dtBeginDate;
                cmd.Parameters["@EndDate"].Value = dtEndDate;
                if (objVendor != null)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@Vendor_Guid"].Value = objVendor.ID;
                }
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CCurrency objCurrency = null;
                    CVendorPaymentDocType objVendorPaymentDocType = null;
                    CVendorInvoice objVendorInvoice = null;
                    CVendor obVendor = null;

                    while (rs.Read())
                    {
                        objCurrency = new CCurrency((System.Guid)rs["Currency_Guid"], (System.String)rs["Currency_Name"],
                                                    (System.String)rs["Currency_Abbr"], (System.String)rs["Currency_Code"]);
                        obVendor = new CVendor(
                            (System.Guid)rs["Vendor_Guid"],
                            (System.String)rs["Vendor_Name"],
                            System.Convert.ToInt32(rs["Vendor_Id"]),
                            System.Convert.ToBoolean(rs["Vendor_IsActive"]));

                        objVendorPaymentDocType = new CVendorPaymentDocType(
                            (System.Guid)rs["VendorPaymenDocType_Guid"],
                            (System.String)rs["VendorPaymenDocType_NAME"],
                            ((rs["VendorPaymenDocType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["VendorPaymenDocType_Description"]),
                            System.Convert.ToBoolean(rs["VendorPaymenDocType_IsActive"])
                            );

                        objVendorInvoice = new CVendorInvoice();
                        objVendorInvoice.ID = (System.Guid)rs["VendorInvoice_Guid"];
                        objVendorInvoice.Number = (System.String)rs["VendorInvoice_Num"];
                        objVendorInvoice.BeginDate = System.Convert.ToDateTime(rs["VendorInvoice_BeginDate"]);
                        objVendorInvoice.VendorOrder = new CVendorOrder();
                        if (rs["VendorOrderList_Guid"] != System.DBNull.Value)
                        {
                            objVendorInvoice.VendorOrder.ID = (System.Guid)rs["VendorOrderList_Guid"];
                            objVendorInvoice.VendorOrder.Number = (System.String)rs["VendorOrderList_OrderNum"];
                            objVendorInvoice.VendorOrder.ShipDate = System.Convert.ToDateTime(rs["VendorOrderList_ShipDate"]);
                            objVendorInvoice.VendorOrder.Company = new CCompany();
                            objVendorInvoice.VendorOrder.Company.ID = (System.Guid)rs["Company_Guid"];
                            objVendorInvoice.VendorOrder.Company.Abbr = (System.String)rs["Company_Acronym"];
                        }

                        objList.Add(new CVendorPaymentDoc(
                        (System.Guid)rs["VendorPaymentDoc_Guid"],
                        (System.String)rs["VendorPaymentDoc_Num"],
                        System.Convert.ToDateTime(rs["VendorPaymentDoc_BeginDate"]), 
                        objVendorPaymentDocType, 
                        objCurrency,
                        System.Convert.ToDouble(rs["VendorPaymentDoc_AllMoney"]),
                        obVendor,
                        ((rs["VendorPaymentDoc_Description"] == System.DBNull.Value) ? "" : (System.String)rs["VendorPaymentDoc_Description"]),
                        objVendorInvoice
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
                "Не удалось получить список платежных документов.\n\nТекст ошибки: " + f.Message, "Внимание",
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
        public System.Boolean IsAllParametersValid()
        {
            System.Boolean bRet = false;
            try
            {
                if (this.Number == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать номер!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.m_AllMoney == 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать сумму!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.m_objCurrency == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать валюту!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.VendorInvoice == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать инвойс!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.m_objVendorPaymentDocType == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать тип документа!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.Vendor == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать поставщика!", "Внимание",
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
        /// Добавляет в базу данных информацию о платежном документе
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean AddVendorPaymentDoc(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (this.IsAllParametersValid() == false)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddVendorPaymentDoc]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymentDoc_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymentDoc_AllMoney", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymentDoc_Num", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Guid", System.Data.SqlDbType.UniqueIdentifier));
                //cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorOrderList_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorInvoice_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymenDocType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymentDoc_BeginDate", System.Data.SqlDbType.SmallDateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymentDoc_Description", System.Data.DbType.String));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@VendorPaymentDoc_Num"].Value = this.Number;
                cmd.Parameters["@VendorPaymentDoc_AllMoney"].Value = System.Convert.ToDecimal(this.m_AllMoney);
                cmd.Parameters["@VendorPaymenDocType_Guid"].Value = this.VendorPaymentDocType.ID;
                cmd.Parameters["@Currency_Guid"].Value = this.Currency.ID;
                cmd.Parameters["@Vendor_Guid"].Value = this.Vendor.ID;
                //cmd.Parameters["@VendorOrderList_Guid"].Value = this.VendorOrder.ID;
                cmd.Parameters["@VendorInvoice_Guid"].Value = this.VendorInvoice.ID;
                cmd.Parameters["@VendorPaymentDoc_BeginDate"].Value = this.BeginDate;
                cmd.Parameters["@VendorPaymentDoc_Description"].Value = this.Description;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    this.ID = (System.Guid)cmd.Parameters["@VendorPaymentDoc_Guid"].Value;
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

        #region Update
        /// <summary>
        /// Изменяет информацию в платежном документе в базе данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="objVendor">поставщик</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean EditVendorPaymentDoc(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (this.IsAllParametersValid() == false)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditVendorPaymentDoc]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymentDoc_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymentDoc_AllMoney", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymentDoc_Num", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Guid", System.Data.SqlDbType.UniqueIdentifier));
                //cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorOrderList_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorInvoice_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymenDocType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymentDoc_BeginDate", System.Data.SqlDbType.SmallDateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymentDoc_Description", System.Data.DbType.String));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@VendorPaymentDoc_Guid"].Value = this.ID;
                cmd.Parameters["@VendorPaymentDoc_Num"].Value = this.Number;
                cmd.Parameters["@VendorPaymentDoc_AllMoney"].Value = System.Convert.ToDecimal(this.m_AllMoney);
                cmd.Parameters["@VendorPaymenDocType_Guid"].Value = this.VendorPaymentDocType.ID;
                cmd.Parameters["@Currency_Guid"].Value = this.Currency.ID;
                cmd.Parameters["@Vendor_Guid"].Value = this.Vendor.ID;
                cmd.Parameters["@VendorInvoice_Guid"].Value = this.VendorInvoice.ID;
                //cmd.Parameters["@VendorInvoice_Guid"].Value = this.VendorInvoice.ID;
                cmd.Parameters["@VendorPaymentDoc_BeginDate"].Value = this.BeginDate;
                cmd.Parameters["@VendorPaymentDoc_Description"].Value = this.Description;
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

        #region Remove
        /// <summary>
        /// Удаляет платежный документ из базы данных
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
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteVendorPaymentDoc]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymentDoc_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@VendorPaymentDoc_Guid"].Value = this.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
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
            return VendorPaymentDocFullName;
        }
    }
    /// <summary>
    /// Класс "Заказ поставщику"
    /// </summary>
    public class CVendorOrder
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
        /// Номер заказа
        /// </summary>
        private System.String m_strNumber;
        /// <summary>
        /// Номер заказа
        /// </summary>
        public System.String Number
        {
            get { return m_strNumber; }
            set { m_strNumber = value; }
        }
        /// <summary>
        /// Дата отгрузки заказа
        /// </summary>
        private System.DateTime m_dtShipDate;
        /// <summary>
        /// Дата отгрузки заказа
        /// </summary>
        public System.DateTime ShipDate
        {
            get { return m_dtShipDate; }
            set { m_dtShipDate = value; }
        }
        /// <summary>
        /// Срок платежа
        /// </summary>
        private System.DateTime m_dtPaymentDate;
        /// <summary>
        /// Срок платежа
        /// </summary>
        public System.DateTime PaymentDate
        {
            get { return m_dtPaymentDate; }
            set { m_dtPaymentDate = value; }
        }
        /// <summary>
        /// Сумма по инвойсу
        /// </summary>
        private double m_AllMoney;
        /// <summary>
        /// Сумма по инвойсу
        /// </summary>
        public double AllMoney
        {
            get { return m_AllMoney; }
            set { m_AllMoney = value; }
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
        /// Поставщик
        /// </summary>
        private CVendor m_objVendor;
        /// <summary>
        /// Поставщик
        /// </summary>
        public CVendor Vendor
        {
            get { return m_objVendor; }
            set { m_objVendor = value; }
        }
        /// <summary>
        /// Договор
        /// </summary>
        private CVendorContract m_objVendorContract;
        /// <summary>
        /// Договор
        /// </summary>
        public CVendorContract VendorContract
        {
            get { return m_objVendorContract; }
            set { m_objVendorContract = value; }
        }
        /// <summary>
        /// Срок оплаты
        /// </summary>
        private System.Int32 m_CreditPeriodDay;
        /// <summary>
        /// Срок оплаты
        /// </summary>
        public System.Int32 CreditPeriodDay
        {
            get { return m_CreditPeriodDay; }
            set { m_CreditPeriodDay = value; }
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
        /// Наименование поставщика
        /// </summary>
        public System.String VendorName
        {
            get { return (m_objVendor == null ? "" : m_objVendor.Name); }
        }
        /// <summary>
        /// Наименование валюты
        /// </summary>
        public System.String CurrencyName
        {
            get { return ( m_objCurrency == null ? "" : m_objCurrency.CurrencyAbbr ); }
        }
        /// <summary>
        /// Компания
        /// </summary>
        public System.String CompanyName
        {
            get { return ( m_objCompany == null ? "" : m_objCompany.Abbr ); }
        }
        public System.String VendorOrderFullName
        {
            get
            {
                System.String strVendorName = this.VendorName;

                return (strVendorName + " " + this.Number + " " + CompanyName);
            }
        }
        /// <summary>
        /// Оплачено
        /// </summary>
        private double m_AllPaymentMoney;
        /// <summary>
        /// Оплачено
        /// </summary>
        public double PaymentMoney
        {
            get { return m_AllPaymentMoney; }
            set { m_AllPaymentMoney = value; }
        }
        public double SaldoMoney
        {
            get { return (m_AllMoney - m_AllPaymentMoney); }
        }
        /// <summary>
        /// Дата поступления на склад приёмки товара
        /// </summary>
        public System.DateTime ArrivalToStockDate { get; set; }
        /// <summary>
        /// УИ заказа в InterBase (T_LotOrder)
        /// </summary>
        public System.Int32 ID_Ib { get; set; }
        #endregion

        #region Конструктор
        public CVendorOrder()
        {
            m_uuidID = System.Guid.Empty;
            m_strNumber = "";
            m_strDescription = "";
            m_objVendorContract = null;
            m_objVendor = null;
            m_objCurrency = null;
            m_objCompany = null;
            m_dtShipDate = System.DateTime.Today;
            m_dtPaymentDate = System.DateTime.Today;
            m_CreditPeriodDay = 0;
            m_bIsActive = false;
            m_AllMoney = 0;
            m_AllPaymentMoney = 0;
            ArrivalToStockDate = System.DateTime.MinValue;
            ID_Ib = 0;
        }
        public CVendorOrder(System.Guid uuidID, System.String strNumber, System.String strDescription, System.DateTime dtShipDate,
            System.DateTime dtPaymentDate, System.Int32 intCreditPeriodDay, double doubleAllMoney, System.Boolean bIsActive,
            CVendor objVendor, CCompany objCompany, CCurrency objCurrency, CVendorContract objVendorContract, 
            double doubleAllPaymentMoney )
        {
            m_uuidID = uuidID;
            m_strNumber = strNumber;
            m_strDescription = strDescription;
            m_objVendorContract = objVendorContract;
            m_objVendor = objVendor;
            m_objCurrency = objCurrency;
            m_objCompany = objCompany;
            m_dtShipDate = dtShipDate;
            m_dtPaymentDate = dtPaymentDate;
            m_CreditPeriodDay = intCreditPeriodDay;
            m_bIsActive = bIsActive;
            m_AllMoney = doubleAllMoney;
            m_AllPaymentMoney = doubleAllPaymentMoney;
            ArrivalToStockDate = System.DateTime.MinValue;
            ID_Ib = 0;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список заказов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objVendor">поставщик</param>
        /// <returns>список заказов</returns>
        public static List<CVendorOrder> GetVendorOrderList( UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, CVendor objVendor, System.DateTime dtBeginDate,
            System.DateTime dtEndDate )
        {
            List<CVendorOrder> objList = new List<CVendorOrder>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetVendorOrderList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                if (objVendor != null)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@Vendor_Guid"].Value = objVendor.ID;
                }
                if ((dtBeginDate != null) && (dtBeginDate != System.DateTime.MinValue))
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BeginDate", System.Data.SqlDbType.DateTime));
                    cmd.Parameters["@BeginDate"].Value = dtBeginDate;
                }
                if ((dtEndDate != null) && (dtEndDate != System.DateTime.MinValue))
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EndDate", System.Data.SqlDbType.DateTime));
                    cmd.Parameters["@EndDate"].Value = dtEndDate;
                }
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CCurrency objCurrency = null;
                    CCompany objCompany = null;
                    CVendor objectVendor = null;
                    CVendorContract objVendorContract = null;
                    //CVendorInvoice objVendorInvoice = null;

                    while (rs.Read())
                    {
                        objCurrency = new CCurrency((System.Guid)rs["Currency_Guid"], (System.String)rs["Currency_Name"],
                                                    (System.String)rs["Currency_Abbr"], (System.String)rs["Currency_Code"]);
                        
                        objCompany = new CCompany( (System.Guid)rs["Company_Guid"], (System.String)rs["Company_Name"], (System.String)rs["Company_Acronym"]);
                        
                        objectVendor = new CVendor( (System.Guid)rs["Vendor_Guid"], (System.String)rs["Vendor_Name"],
                            System.Convert.ToInt32(rs["Vendor_Id"]), System.Convert.ToBoolean(rs["Vendor_IsActive"]) );

                        objVendorContract = new CVendorContract(
                            (System.Guid)rs["VendorContract_Guid"],
                            (System.String)rs["VendorContract_Num"],
                            System.Convert.ToDateTime(rs["VendorContract_BeginDate"]),
                            System.Convert.ToDateTime(rs["VendorContract_EndDate"]),
                            System.Convert.ToInt32(rs["VendorContract_CreditPeriod"]),
                            ((rs["VendorContract_Description"] == System.DBNull.Value) ? "" : (System.String)rs["VendorContract_Description"]),
                            System.Convert.ToBoolean(rs["Vendor_IsActive"]), 
                            new CVendorContractType( (System.Guid)rs["VendorContractType_Guid"],(System.String)rs["VendorContractType_NAME"],
                                    System.Convert.ToInt32(rs["VendorContractType_CreditPeriodDay"]),
                                    ((rs["VendorContractType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["VendorContractType_Description"]),
                                    System.Convert.ToBoolean(rs["VendorContractType_IsActive"]) ), null,
                            ((rs["VendorContractDelayType_Guid"] == System.DBNull.Value) ? null : new CVendorContractDelayType()
                            {
                                ID = (System.Guid)rs["VendorContractDelayType_Guid"],
                                Name = System.Convert.ToString(rs["VendorContractDelayType_NAME"]),
                                Description = ((rs["VendorContractDelayType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["VendorContractDelayType_Description"]),
                                IsActive = System.Convert.ToBoolean(rs["VendorContractDelayType_IsActive"])

                            } )
                            );

                        objList.Add(new CVendorOrder((System.Guid)rs["VendorOrderList_Guid"], (System.String)rs["VendorOrderList_OrderNum"], 
                            ((rs["VendorOrderList_Description"] == System.DBNull.Value) ? "" : (System.String)rs["VendorOrderList_Description"]),
                            System.Convert.ToDateTime(rs["VendorOrderList_ShipDate"]), System.Convert.ToDateTime(rs["VendorOrderList_PaymentDate"]),
                            System.Convert.ToInt32(rs["VendorOrderList_CreditPeriod"]),
                            System.Convert.ToDouble(rs["VendorOrderList_AllMoney"]), System.Convert.ToBoolean(rs["VendorOrderList_IsActive"]),
                            objectVendor, objCompany, objCurrency, objVendorContract,
                            System.Convert.ToDouble(rs["VendorOrderPaymentSum"])
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
                "Не удалось получить список заказов.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Возвращает список подтверждённых заказов (из InterBase)
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="dtBeginDate">начало периода поиска</param>
        /// <param name="dtEndDate">окончание периода поиска</param>
        /// <returns>список заказов</returns>
        public static List<CVendorOrder> GetUnRegisteredVendorOrderList(UniXP.Common.CProfile objProfile,
            System.DateTime dtBeginDate,  System.DateTime dtEndDate)
        {
            List<CVendorOrder> objList = new List<CVendorOrder>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetLotOrderListFromIB]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                if ((dtBeginDate != null) && (dtBeginDate != System.DateTime.MinValue))
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BeginDate", System.Data.SqlDbType.DateTime));
                    cmd.Parameters["@BeginDate"].Value = dtBeginDate;
                }
                if ((dtEndDate != null) && (dtEndDate != System.DateTime.MinValue))
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EndDate", System.Data.SqlDbType.DateTime));
                    cmd.Parameters["@EndDate"].Value = dtEndDate;
                }
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CCurrency objCurrency = null;
                    CCompany objCompany = null;
                    CVendor objVendor = null;
                    CVendorContract objVendorContract = null;
                    CVendorOrder objVendorOrder = null;

                    while (rs.Read())
                    {
                        objCurrency = new CCurrency() { 
                            ID = (System.Guid)rs["Currency_Guid"], 
                            CurrencyAbbr = (System.String)rs["Currency_Abbr"], 
                            CurrencyCode = (System.String)rs["Currency_Code"] 
                        };

                        objCompany = new CCompany() { 
                            ID = System.Guid.Empty, 
                            Abbr = "", 
                            Name = "" 
                        };

                        objVendor = new CVendor()
                        { 
                            ID = (System.Guid)rs["Vendor_Guid"], 
                            ID_Ib = System.Convert.ToInt32(rs["Vendor_Id"]), 
                            Name = (System.String)rs["Vendor_Name"], 
                            IsActive = System.Convert.ToBoolean(rs["Vendor_IsActive"]) 
                        };

                        objVendorContract = null;

                        objVendorOrder = new CVendorOrder();
                        objVendorOrder.ID_Ib = System.Convert.ToInt32(rs["LOTORDER_ID"]);
                        objVendorOrder.Number = System.Convert.ToString(rs["LOTORDER_NUM"]);
                        objVendorOrder.ShipDate = System.Convert.ToDateTime(rs["LOTORDER_SHIPDATE"]);
                        objVendorOrder.AllMoney = System.Convert.ToDouble(rs["LOTORDER_ALLPRICE"]);
                        objVendorOrder.Vendor = objVendor;
                        objVendorOrder.Company = objCompany;
                        objVendorOrder.Currency = objCurrency;
                        objVendorOrder.VendorContract = objVendorContract;

                        objList.Add( objVendorOrder );
                    }
                }
                rs.Dispose();
                cmd.Dispose();
                DBConnection.Close();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список заказов.\n\nТекст ошибки: " + f.Message, "Внимание",
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
        public System.Boolean IsAllParametersValid()
        {
            System.Boolean bRet = false;
            try
            {
                if (this.Number == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать номер договора!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.m_AllMoney <= 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать срок платежа!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.m_objCurrency == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать валюту!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.Company == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать компанию-импортера!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.CreditPeriodDay <= 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать кредитный срок!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                //if (this.VendorInvoice == null)
                //{
                //    DevExpress.XtraEditors.XtraMessageBox.Show(
                //    "Необходимо указать инвойс!", "Внимание",
                //    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                //    return bRet;
                //}
                if (this.VendorContract == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать договор с поставщиком!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.Vendor == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать поставщика!", "Внимание",
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
        /// Добавляет в базу данных информацию о заказе поставщику
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean AddOrderToVendor(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            ref System.String strErr )
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (this.IsAllParametersValid() == false)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddVendorOrderList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorOrderList_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorOrderList_AllMoney", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorOrderList_OrderNum", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContract_Guid", System.Data.SqlDbType.UniqueIdentifier));
                //cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorInvoice_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorOrderList_ShipDate", System.Data.SqlDbType.SmallDateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorOrderList_PaymentDate", System.Data.SqlDbType.SmallDateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorOrderList_CreditPeriod", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorOrderList_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorOrderList_IsActive", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@VendorOrderList_OrderNum"].Value = this.Number;
                cmd.Parameters["@VendorOrderList_AllMoney"].Value = System.Convert.ToDecimal(this.m_AllMoney);
                cmd.Parameters["@Company_Guid"].Value = this.Company.ID;
                cmd.Parameters["@Currency_Guid"].Value = this.Currency.ID;
                cmd.Parameters["@Vendor_Guid"].Value = this.Vendor.ID;
                cmd.Parameters["@VendorContract_Guid"].Value = this.VendorContract.ID;
                cmd.Parameters["@VendorOrderList_ShipDate"].Value = this.ShipDate;
                cmd.Parameters["@VendorOrderList_PaymentDate"].Value = this.PaymentDate;
                cmd.Parameters["@VendorOrderList_Description"].Value = this.Description;
                cmd.Parameters["@VendorOrderList_IsActive"].Value = this.IsActive;
                cmd.Parameters["@VendorOrderList_CreditPeriod"].Value = this.CreditPeriodDay;
                if (this.ArrivalToStockDate != System.DateTime.MinValue)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorOrder_ArrivalToStockDate", System.Data.SqlDbType.DateTime));
                    cmd.Parameters["@VendorOrder_ArrivalToStockDate"].Value = this.ArrivalToStockDate;
                }
                if (this.ID_Ib != 0 )
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Lotorder_Id", System.Data.SqlDbType.Int));
                    cmd.Parameters["@Lotorder_Id"].Value = this.ID_Ib;
                }
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    this.ID = (System.Guid)cmd.Parameters["@VendorOrderList_Guid"].Value;
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

        #region Update
        /// <summary>
        /// Изменяет информацию заказе в базе данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean EditOrderForVendor(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (this.IsAllParametersValid() == false)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditVendorOrderList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorOrderList_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorOrderList_AllMoney", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorOrderList_OrderNum", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorContract_Guid", System.Data.SqlDbType.UniqueIdentifier));
                //cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorInvoice_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorOrderList_ShipDate", System.Data.SqlDbType.SmallDateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorOrderList_PaymentDate", System.Data.SqlDbType.SmallDateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorOrderList_CreditPeriod", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorOrderList_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorOrderList_IsActive", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@VendorOrderList_Guid"].Value = this.ID;
                cmd.Parameters["@VendorOrderList_OrderNum"].Value = this.Number;
                cmd.Parameters["@VendorOrderList_AllMoney"].Value = System.Convert.ToDecimal(this.m_AllMoney);
                cmd.Parameters["@Company_Guid"].Value = this.Company.ID;
                cmd.Parameters["@Currency_Guid"].Value = this.Currency.ID;
                cmd.Parameters["@Vendor_Guid"].Value = this.Vendor.ID;
                cmd.Parameters["@VendorContract_Guid"].Value = this.VendorContract.ID;
                //cmd.Parameters["@VendorInvoice_Guid"].Value = this.VendorInvoice.ID;
                cmd.Parameters["@VendorOrderList_ShipDate"].Value = this.ShipDate;
                cmd.Parameters["@VendorOrderList_PaymentDate"].Value = this.PaymentDate;
                cmd.Parameters["@VendorOrderList_Description"].Value = this.Description;
                cmd.Parameters["@VendorOrderList_IsActive"].Value = this.IsActive;
                cmd.Parameters["@VendorOrderList_CreditPeriod"].Value = this.CreditPeriodDay;
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

        #region Remove
        /// <summary>
        /// Удаляет заказ из базы данных
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
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteVendorOrderList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorOrderList_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@VendorOrderList_Guid"].Value = this.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
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
            return VendorOrderFullName;
        }
    }
    /// <summary>
    /// Класс "Платеж поставщику"
    /// </summary>
    public class CVendorPayment
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
        /// Дата платежа
        /// </summary>
        private System.DateTime m_dtPaymentDate;
        /// <summary>
        /// Дата платежа
        /// </summary>
        public System.DateTime PaymentDate
        {
            get { return m_dtPaymentDate; }
            set { m_dtPaymentDate = value; }
        }
        /// <summary>
        /// Сумма по инвойсу
        /// </summary>
        private double m_AllPayMoney;
        /// <summary>
        /// Сумма по инвойсу
        /// </summary>
        public double AllPayMoney
        {
            get { return m_AllPayMoney; }
            set { m_AllPayMoney = value; }
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
        /// Поставщик
        /// </summary>
        private CVendor m_objVendor;
        /// <summary>
        /// Поставщик
        /// </summary>
        public CVendor Vendor
        {
            get { return m_objVendor; }
            set { m_objVendor = value; }
        }
        /// <summary>
        /// Инвойс
        /// </summary>
        private CVendorInvoice m_objVendorInvoice;
        /// <summary>
        /// Инвойс
        /// </summary>
        public CVendorInvoice VendorInvoice
        {
            get { return m_objVendorInvoice; }
            set { m_objVendorInvoice = value; }
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
        /// <summary>
        /// Признак "Оплачивать только указанный инвойс"
        /// </summary>
        private System.Boolean m_bIsPayOnlyThisInvoice;
        /// <summary>
        /// Признак "Оплачивать только указанный инвойс"
        /// </summary>
        public System.Boolean IsPayOnlyThisInvoice
        {
            get { return m_bIsPayOnlyThisInvoice; }
            set { m_bIsPayOnlyThisInvoice = value; }
        }
        /// <summary>
        /// Наименование поставщика
        /// </summary>
        public System.String VendorName
        {
            get { return (m_objVendor == null ? "" : m_objVendor.Name); }
        }
        /// <summary>
        /// Наименование валюты
        /// </summary>
        public System.String CurrencyName
        {
            get { return (m_objCurrency == null ? "" : m_objCurrency.CurrencyAbbr); }
        }
        /// <summary>
        /// Компания
        /// </summary>
        public System.String CompanyName
        {
            get { return (m_objCompany == null ? "" : m_objCompany.Abbr); }
        }
        /// <summary>
        /// Инвойс
        /// </summary>
        public System.String InvoiceName
        {
            get { return (m_objVendorInvoice == null ? "" : m_objVendorInvoice.VendorInvoiceFullName); }
        }
        /// <summary>
        /// Сумма заказа
        /// </summary>
        public double InvoicerSum
        {
            get { return (m_objVendorInvoice == null ? 0 : m_objVendorInvoice.AllMoney); }
        }
        /// <summary>
        /// Срок платежа по инвойсу
        /// </summary>
        public System.DateTime InvoicePaymentDate
        {
            get { return (m_objVendorInvoice == null ? System.DateTime.Today : m_objVendorInvoice.PaymentDate); }
        }
        /// <summary>
        /// Заказ
        /// </summary>
        public System.String OrderName
        {
            get { return (m_objVendorInvoice == null ? "" : m_objVendorInvoice.OrderNum); }
        }
        /// <summary>
        /// Сумма заказа
        /// </summary>
        public double OrderSum
        {
            get { return (m_objVendorInvoice == null ? 0 : m_objVendorInvoice.OrderSum); }
        }
        public System.String VendorPaymentFullName
        {
            get
            {
                System.String strVendorName = this.VendorName;

                return (strVendorName + " " + this.AllPayMoney.ToString() + " " + CurrencyName);
            }
        }
        #endregion

        #region Конструктор
        public CVendorPayment()
        {
            m_uuidID = System.Guid.Empty;
            m_strDescription = "";
            m_objVendorInvoice = null;
            m_objVendor = null;
            m_objCurrency = null;
            m_objCompany = null;
            m_dtPaymentDate = System.DateTime.Today;
            m_bIsPayOnlyThisInvoice = false;
            m_AllPayMoney = 0;
        }
        public CVendorPayment( System.Guid uuidID, CVendorInvoice objVendorInvoice, CVendor objVendor, CCurrency objCurrency,
            double doublePayMoney, CCompany objCompany, System.Boolean bIsPayOnlyThisInvoice, System.DateTime dtPaymentDate, System.String strDescription )
        {
            m_uuidID = uuidID;
            m_strDescription = strDescription;
            m_objVendorInvoice = objVendorInvoice;
            m_objVendor = objVendor;
            m_objCurrency = objCurrency;
            m_objCompany = objCompany;
            m_dtPaymentDate = dtPaymentDate;
            m_bIsPayOnlyThisInvoice = bIsPayOnlyThisInvoice;
            m_AllPayMoney = doublePayMoney;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список платежей поставщику
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objVendor">поставщик</param>
        /// <returns>список заказов</returns>
        public static List<CVendorPayment> GetVendorPaymentList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, CVendor objVendor, System.DateTime dtBeginDate,
            System.DateTime dtEndDate)
        {
            List<CVendorPayment> objList = new List<CVendorPayment>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetVendorPaymentList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                if (objVendor != null)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@Vendor_Guid"].Value = objVendor.ID;
                }
                if ((dtBeginDate != null) && (dtBeginDate != System.DateTime.MinValue))
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BeginDate", System.Data.SqlDbType.DateTime));
                    cmd.Parameters["@BeginDate"].Value = dtBeginDate;
                }
                if ((dtEndDate != null) && (dtEndDate != System.DateTime.MinValue))
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EndDate", System.Data.SqlDbType.DateTime));
                    cmd.Parameters["@EndDate"].Value = dtEndDate;
                }
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CCurrency objCurrency = null;
                    CCompany objCompany = null;
                    CVendor objectVendor = null;
                    CVendorInvoice objVendorInvoice = null;
                    CVendorOrder objVendorOrder = null;

                    while (rs.Read())
                    {
                        objCurrency = new CCurrency((System.Guid)rs["Currency_Guid"], (System.String)rs["Currency_Name"],
                                                    (System.String)rs["Currency_Abbr"], (System.String)rs["Currency_Code"]);

                        objCompany = new CCompany((System.Guid)rs["Company_Guid"], (System.String)rs["Company_Name"], (System.String)rs["Company_Acronym"]);

                        objectVendor = new CVendor((System.Guid)rs["Vendor_Guid"], (System.String)rs["Vendor_Name"],
                            System.Convert.ToInt32(rs["Vendor_Id"]), System.Convert.ToBoolean(rs["Vendor_IsActive"]));

                        objVendorOrder = new CVendorOrder();
                        objVendorOrder.ID = (System.Guid)rs["VendorOrderList_Guid"];
                        objVendorOrder.Number = (System.String)rs["VendorOrderList_OrderNum"];
                        objVendorOrder.AllMoney = System.Convert.ToDouble(rs["VendorInvoice_AllMoney"]);

                        objVendorInvoice = new CVendorInvoice(
                            (System.Guid)rs["VendorInvoice_Guid"],
                            (System.String)rs["VendorInvoice_Num"],
                            System.Convert.ToDateTime(rs["VendorInvoice_BeginDate"]),
                            new CCurrency((System.Guid)rs["VendorInvoice_Currency_Guid"], (System.String)rs["VendorInvoice_Currency_Name"],
                                    (System.String)rs["VendorInvoice_Currency_Abbr"], (System.String)rs["VendorInvoice_Currency_Code"]),
                            System.Convert.ToDouble(rs["VendorInvoice_AllMoney"]), null, "", System.Convert.ToInt32(rs["VendorInvoice_CreditPeriod"]),
                            System.Convert.ToDateTime(rs["VendorInvoice_PaymentDate"]), null, objVendorOrder, 0  
                            );

                        objList.Add(new CVendorPayment((System.Guid)rs["VendorPaymentList_Guid"], objVendorInvoice, objectVendor, objCurrency, 
                            System.Convert.ToDouble(rs["VendorPaymentList_PayMoney"]), objCompany, System.Convert.ToBoolean(rs["PayOnlyForThisInvoice"]), 
                            System.Convert.ToDateTime(rs["VendorPaymentList_PayDate"]),
                            ((rs["VendorPaymentList_Description"] == System.DBNull.Value) ? "" : (System.String)rs["VendorPaymentList_Description"])
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
                "Не удалось получить список платежей поставщику.\n\nТекст ошибки: " + f.Message, "Внимание",
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
        public System.Boolean IsAllParametersValid()
        {
            System.Boolean bRet = false;
            try
            {
                if (this.m_AllPayMoney <= 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать срок платежа!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.m_objCurrency == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать валюту!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.Company == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать компанию-импортера!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.VendorInvoice == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать инвойс!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.Vendor == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать поставщика!", "Внимание",
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
        /// Добавляет в базу данных информацию о платеже поставщику
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean AddPaymentToVendor(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (this.IsAllParametersValid() == false)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddVendorPaymentList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymentList_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymentList_PayMoney", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorInvoice_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymentList_PayDate", System.Data.SqlDbType.SmallDateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymentList_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PayOnlyForThisInvoice", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@VendorPaymentList_PayMoney"].Value = System.Convert.ToDecimal(this.m_AllPayMoney);
                cmd.Parameters["@Company_Guid"].Value = this.Company.ID;
                cmd.Parameters["@Currency_Guid"].Value = this.Currency.ID;
                cmd.Parameters["@Vendor_Guid"].Value = this.Vendor.ID;
                cmd.Parameters["@VendorInvoice_Guid"].Value = this.VendorInvoice.ID;
                cmd.Parameters["@VendorPaymentList_PayDate"].Value = this.PaymentDate;
                cmd.Parameters["@VendorPaymentList_Description"].Value = this.Description;
                cmd.Parameters["@PayOnlyForThisInvoice"].Value = this.m_bIsPayOnlyThisInvoice;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    this.ID = (System.Guid)cmd.Parameters["@VendorPaymentList_Guid"].Value;
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

        #region Update
        /// <summary>
        /// Изменяет информацию о платеже в базе данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean EditPaymentForVendor(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                if (this.IsAllParametersValid() == false)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditVendorPaymentList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymentList_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymentList_PayMoney", System.Data.SqlDbType.Money));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorInvoice_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymentList_PayDate", System.Data.SqlDbType.SmallDateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymentList_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PayOnlyForThisInvoice", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@VendorPaymentList_Guid"].Value = this.ID;
                cmd.Parameters["@VendorPaymentList_PayMoney"].Value = System.Convert.ToDecimal(this.m_AllPayMoney);
                cmd.Parameters["@Company_Guid"].Value = this.Company.ID;
                cmd.Parameters["@Currency_Guid"].Value = this.Currency.ID;
                cmd.Parameters["@Vendor_Guid"].Value = this.Vendor.ID;
                cmd.Parameters["@VendorInvoice_Guid"].Value = this.VendorInvoice.ID;
                cmd.Parameters["@VendorPaymentList_PayDate"].Value = this.PaymentDate;
                cmd.Parameters["@VendorPaymentList_Description"].Value = this.Description;
                cmd.Parameters["@PayOnlyForThisInvoice"].Value = this.m_bIsPayOnlyThisInvoice;
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

        #region Remove
        /// <summary>
        /// Удаляет заказ из базы данных
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
                    cmd.Transaction = DBTransaction;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteVendorPaymentList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorPaymentList_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@VendorPaymentList_Guid"].Value = this.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
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
    }

    
    #region Класс "Тип хранилища"
    /// <summary>
    /// Класс "Тип хранилища"
    /// </summary>
    public class CVendorType : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Уникальный идентификатор в InterBase
        /// </summary>
        private System.Int32 m_IbID;
        /// <summary>
        /// Уникальный идентификатор в InterBase
        /// </summary>
        /*[DisplayName("Номер поставщика в IB")]
        [Description("Номер поставщика в IB")]
        [Category("2. Дополнительно")]
        [ReadOnly(true)]*/
        [BrowsableAttribute(false)]
        public System.Int32 InterBaseID
        {
            get { return m_IbID; }
            set { m_IbID = value; }
        }

        /// <summary>
        /// Признак активности
        /// </summary>
        private System.Boolean m_IsActive;  
        /// <summary>
        /// Признак активности
        /// </summary>
        [DisplayName("Поставщик, активен")]
        [Description("Признак, что поставщик активен")]
        [Category("2. Дополнительно")]
        [TypeConverter(typeof(BooleanTypeConverter))]
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
        [DisplayName("Комментарии")]
        [Description("Комментарии к типу поставщика")]
        [Category("2. Дополнительно")]
        public System.String Description
        {
            get { return m_strDescription; }
            set { m_strDescription = value; }
        }
        #endregion

        #region Конструктор
        public CVendorType()
            : base()
        {
        }
        public CVendorType(Guid uuidId, String strName, System.Int32 IbID, String strDescription, Boolean bIsActive)
        {
            ID = uuidId;
            Name = strName;
            m_IbID = IbID;
            m_strDescription = strDescription;
            m_IsActive = bIsActive;
        }
        #endregion

        #region Список объектов
        public static List<CVendorType> GetVendorTypeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CVendorType> objList = new List<CVendorType>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetVendorType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CVendorType( (System.Guid)rs["VendorType_Guid"],
                                                      (System.String)rs["VendorType_Name"],
                                                      ((rs["VendorType_Id"] == System.DBNull.Value) ? 0 : (System.Int32)rs["VendorType_Id"]),
                                                      ((rs["VendorType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["VendorType_Description"]),
                                                      (System.Boolean)rs["VendorType_IsActive"]));
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
                "Не удалось получить список типов хранилища.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }

        #endregion

        #region IsAllParametersValid
        /// <summary>
        /// Проверка свойств типа поставщика перед сохранением
        /// </summary>
        /// <param name="strErr">текст с ошибкой</param>
        /// <returns>true - все свойства корректны; false - ошибка</returns>
        public System.Boolean IsAllParametersValid(ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                // написать тут код с регулярным выражением
                //Regex r = new Regex(@"\s");
                //Match m = r.Match(this.Name) ; //Результат
                
                if (this.Name.Trim() == "" || this.Name.Trim() == " ")
                {
                    strErr = "Тип поставщика: Необходимо указать название типа поставщика!";
                    return bRet;
                }

                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr = "Ошибка проверки свойств хранилища. Текст ошибки: " + f.Message;
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

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            System.String strErr = "";
            try
            {
                if (this.IsAllParametersValid(ref strErr) == false)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Внимание", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return bRet;
                }

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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddVendorType]", objProfile.GetOptionsDllDBName()); 

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorType_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorType_IsActive", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorType_Description", System.Data.DbType.String));
                
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@VendorType_IsActive"].Value = this.IsActive;
                cmd.Parameters["@VendorType_Name"].Value = this.Name;
                cmd.Parameters["@VendorType_Description"].Value = this.Description;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@VendorType_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания типа поставщика.\n\nТекст ошибки: " + strErr, "Ошибка",
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
                strErr += f.Message;
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства типа хранилища.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                if (this.IsAllParametersValid(ref strErr) == false)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Внимание", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return bRet;
                }

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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditVendorType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorType_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorType_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorType_IsActive", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@VendorType_Guid"].Value = this.ID;
                cmd.Parameters["@VendorType_Name"].Value = this.Name;
                cmd.Parameters["@VendorType_Description"].Value = this.Description;
                cmd.Parameters["@VendorType_IsActive"].Value = this.IsActive;

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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств типа поставщика.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства типа поставщика.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                if (DBConnection != null)
                {
                    DBConnection.Close();
                }
                //DBConnection.Close();
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteVendorType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@VendorType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@VendorType_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления типа хранилища.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить тип хранилища.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        #endregion

        // ВНИМАНИЕ !!! Методе для CRUD, которе ниже, не переписаны !!!

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
        
        #region Перегруженный toString
        public override string ToString()
        {
            return (this.Name);
        }
        #endregion

    } // CVendorType
    #endregion
}
