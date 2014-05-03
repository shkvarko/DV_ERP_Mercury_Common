using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ERP_Mercury.Common
{
    /// <summary>
    /// Класс "Банк"
    /// </summary>
    public class CBank : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Ссылка на идентификатор головного банка
        /// </summary>
        private System.Guid m_uuidParentId;
        /// <summary>
        /// Ссылка на идентификатор головного банка
        /// </summary>
        public System.Guid ParentId
        {
            get { return m_uuidParentId; }
            set { m_uuidParentId = value; }
        }
        /// <summary>
        /// Код банка
        /// </summary>
        private System.String m_strCode;
        /// <summary>
        /// Код банка
        /// </summary>
        public System.String Code
        {
            get { return m_strCode; }
            set { m_strCode = value; }
        }
        /// <summary>
        /// УНН банка
        /// </summary>
        private System.String m_strUNN;
        /// <summary>
        /// УНН банка
        /// </summary>
        public System.String UNN
        {
            get { return m_strUNN; }
            set { m_strUNN = value; }
        }
        /// <summary>
        /// МФО банка
        /// </summary>
        private System.String m_strMFO;
        /// <summary>
        /// МФО банка
        /// </summary>
        public System.String MFO
        {
            get { return m_strMFO; }
            set { m_strMFO = value; }
        }
        /// <summary>
        /// Веб-страница банка
        /// </summary>
        private System.String m_strWWW;
        /// <summary>
        /// Веб-страница банка
        /// </summary>
        public System.String WWW
        {
            get { return m_strWWW; }
            set { m_strWWW = value; }
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
        /// Адрес
        /// </summary>
        private List<CAddress> m_objAddress;
        /// <summary>
        /// Адрес
        /// </summary>
        public List<CAddress> Address
        {
            get { return m_objAddress; }
            set { m_objAddress = value; }
        }

        #endregion

        #region Конструктор
        public CBank()
        {
            ID = System.Guid.Empty;
            Name = "";
            m_bIsActive = true;
            m_objAddress = null;
            m_objContactList = null;
            m_strCode = "";
            m_strDescription = "";
            m_strMFO = "";
            m_strUNN = "";
            m_strWWW = "";
            m_uuidParentId = System.Guid.Empty;
        }
        public CBank(System.Guid uuidId, System.String strName, System.Guid Bank_ParentGuid, System.String Bank_Code,
            System.String Bank_Description, System.String Bank_UNN, System.String Bank_MFO, System.String Bank_WWW, System.Boolean Bank_IsActive)
        {
            ID = uuidId;
            Name = strName;
            m_bIsActive = Bank_IsActive;
            m_objAddress = null;
            m_objContactList = null;
            m_strCode = Bank_Code;
            m_strDescription = Bank_Description;
            m_strMFO = Bank_MFO;
            m_strUNN = Bank_UNN;
            m_strWWW = Bank_WWW;
            m_uuidParentId = Bank_ParentGuid;
        }
        #endregion

        #region Список банков
        public static List<CBank> GetBankList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, DevExpress.XtraTreeList.TreeList objTreeList)
        {
            List<CBank> objList = new List<CBank>();
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (objTreeList != null) { objTreeList.Nodes.Clear(); }
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetBank]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.String strDscrpn = "";
                    System.String strWWW = "";
                    System.Guid ParentId = System.Guid.Empty;
                    CBank objBank = null;
                    DevExpress.XtraTreeList.Nodes.TreeListNode objCurrentParentNode = null;
                    while (rs.Read())
                    {
                        strDscrpn = (rs["Bank_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Bank_Description"];
                        strWWW = (rs["Bank_WWW"] == System.DBNull.Value) ? "" : (System.String)rs["Bank_WWW"];
                        ParentId = (rs["Bank_ParentGuid"] == System.DBNull.Value) ? System.Guid.Empty : (System.Guid)rs["Bank_ParentGuid"];

                        objBank = new CBank((System.Guid)rs["Bank_Guid"], (System.String)rs["Bank_Name"], ParentId,
                            (System.String)rs["Bank_Code"], strDscrpn, (System.String)rs["Bank_UNN"], (System.String)rs["Bank_MFO"],
                            strWWW, (System.Boolean)rs["Bank_IsActive"]);

                        objList.Add(objBank);

                        if (objTreeList != null)
                        {
                            if (objBank.ParentId.CompareTo(System.Guid.Empty) == 0)
                            {
                                objCurrentParentNode = objTreeList.AppendNode(new object[] { objBank.Name, objBank.Code, (System.Int32)rs["BranchCount"], (System.String)rs["Bank_Address"] }, null);
                                objCurrentParentNode.Tag = objBank;
                            }
                            else
                            {
                                objTreeList.AppendNode(new object[] { objBank.Name, objBank.Code, (System.Int32)rs["BranchCount"], (System.String)rs["Bank_Address"] }, objCurrentParentNode).Tag = objBank;
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
                "Не удалось получить список банков.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }

        /// <summary>
        /// Возвращает банк по его коду
        /// </summary>
        /// <param name="sBankCod">код банка</param>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список компаний</returns>
        public static List<CBank> GetBankByBankCod(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, System.String sBankCod)
        {
            List<CBank> objList = new List<CBank>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetBankByBankCod]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Bank_BankCod", System.Data.SqlDbType.NVarChar, 3));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Bank_BankCod"].Value = sBankCod;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.String strDscrpn = "";
                    System.String strWWW = "";
                    System.Guid ParentId = System.Guid.Empty;
                    CBank objBank = null;
                    while (rs.Read())
                    {
                        strDscrpn = (rs["Bank_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Bank_Description"];
                        strWWW = (rs["Bank_WWW"] == System.DBNull.Value) ? "" : (System.String)rs["Bank_WWW"];
                        ParentId = (rs["Bank_ParentGuid"] == System.DBNull.Value) ? System.Guid.Empty : (System.Guid)rs["Bank_ParentGuid"];

                        objBank = new CBank((System.Guid)rs["Bank_Guid"], (System.String)rs["Bank_Name"], ParentId,
                            (System.String)rs["Bank_Code"], strDscrpn, (System.String)rs["Bank_UNN"], (System.String)rs["Bank_MFO"],
                            strWWW, (System.Boolean)rs["Bank_IsActive"]);

                        objList.Add(objBank);
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
                "Не удалось получить банк по его коду.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        #endregion

        #region Добавить информацию о банке в базу данных
        /// <summary>
        /// Проверка свойств контакта перед сохранением
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
                    strErr = "Необходимо указать наименование банка!";
                    return bRet;
                }
                if ((this.Code.Trim() == "") || (this.Code.Trim().Length != 3))
                {
                    strErr = "Код банка должен содержать 3 символа!";
                    return bRet;
                }
                if (this.MFO.Trim() == "")
                {
                    strErr = "Необходимо указать МФО банка!";
                    return bRet;
                }
                if (this.UNN.Trim() == "")
                {
                    strErr = "Необходимо указать УНН банка!";
                    return bRet;
                }
                if (this.Address == null)
                {
                    strErr = "Необходимо указать адрес банка!";
                    return bRet;
                }
                if ((this.Address != null) && (this.Address.Count > 0))
                {
                    System.Boolean bIsAllAddressValid = true;
                    foreach (CAddress objItem in this.Address)
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

                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr = "Ошибка проверки свойств. Текст ошибки: " + f.Message;
            }
            return bRet;
        }
        /// <summary>
        /// Добавляет в базу данных информацию о банке
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Add(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddBank]", objProfile.GetOptionsDllDBName()); ;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Bank_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Bank_ParentGuid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Bank_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Bank_Code", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Bank_UNN", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Bank_MFO", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Bank_WWW", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Bank_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Bank_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Bank_Name"].Value = this.Name;
                cmd.Parameters["@Bank_Code"].Value = this.Code;
                cmd.Parameters["@Bank_UNN"].Value = this.UNN;
                cmd.Parameters["@Bank_MFO"].Value = this.MFO;
                cmd.Parameters["@Bank_IsActive"].Value = this.IsActive;

                if (this.WWW.Trim() != "")
                {
                    cmd.Parameters["@Bank_WWW"].IsNullable = false;
                    cmd.Parameters["@Bank_WWW"].Value = this.WWW.Trim();
                }
                else
                {
                    cmd.Parameters["@Bank_WWW"].IsNullable = true;
                    cmd.Parameters["@Bank_WWW"].Value = null;
                }
                if (this.Description.Trim() != "")
                {
                    cmd.Parameters["@Bank_Description"].IsNullable = false;
                    cmd.Parameters["@Bank_Description"].Value = this.Description;
                }
                else
                {
                    cmd.Parameters["@Bank_Description"].IsNullable = true;
                    cmd.Parameters["@Bank_Description"].Value = null;
                }
                if (this.ParentId.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters["@Bank_ParentGuid"].IsNullable = false;
                    cmd.Parameters["@Bank_ParentGuid"].Value = this.ParentId;
                }
                else
                {
                    cmd.Parameters["@Bank_ParentGuid"].IsNullable = true;
                    cmd.Parameters["@Bank_ParentGuid"].Value = null;
                }

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    this.ID = (System.Guid)cmd.Parameters["@Bank_Guid"].Value;
                    // теперь списки контактов и и адресов
                    if ((this.ContactList != null) && (this.ContactList.Count > 0))
                    {
                        iRes = (CContact.SaveContactList(this.ContactList, null, EnumObject.Bank, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                    }
                    if (iRes == 0)
                    {
                        if ((this.Address != null) && (this.Address.Count > 0))
                        {
                            iRes = (CAddress.SaveAddressList(this.Address, null, EnumObject.Bank, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
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

        #region Сохранить в базе данных изменения в описании банка
        /// <summary>
        /// Сохраняет в базе данных изменения в описании банка
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <param name="objContactDeletedList">список удаленных контактов</param>
        /// <param name="objAddressDeletedList">список удаленных адресов</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Update(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            ref System.String strErr, List<CContact> objContactDeletedList, List<CAddress> objAddressDeletedList)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditBank]", objProfile.GetOptionsDllDBName()); ;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Bank_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Bank_ParentGuid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Bank_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Bank_Code", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Bank_UNN", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Bank_MFO", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Bank_WWW", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Bank_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Bank_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Bank_Guid"].Value = this.ID;
                cmd.Parameters["@Bank_Name"].Value = this.Name;
                cmd.Parameters["@Bank_Code"].Value = this.Code;
                cmd.Parameters["@Bank_UNN"].Value = this.UNN;
                cmd.Parameters["@Bank_MFO"].Value = this.MFO;
                cmd.Parameters["@Bank_IsActive"].Value = this.IsActive;

                if (this.WWW.Trim() != "")
                {
                    cmd.Parameters["@Bank_WWW"].IsNullable = false;
                    cmd.Parameters["@Bank_WWW"].Value = this.WWW.Trim();
                }
                else
                {
                    cmd.Parameters["@Bank_WWW"].IsNullable = true;
                    cmd.Parameters["@Bank_WWW"].Value = null;
                }
                if (this.Description.Trim() != "")
                {
                    cmd.Parameters["@Bank_Description"].IsNullable = false;
                    cmd.Parameters["@Bank_Description"].Value = this.Description;
                }
                else
                {
                    cmd.Parameters["@Bank_Description"].IsNullable = true;
                    cmd.Parameters["@Bank_Description"].Value = null;
                }
                if (this.ParentId.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters["@Bank_ParentGuid"].IsNullable = false;
                    cmd.Parameters["@Bank_ParentGuid"].Value = this.ParentId;
                }
                else
                {
                    cmd.Parameters["@Bank_ParentGuid"].IsNullable = true;
                    cmd.Parameters["@Bank_ParentGuid"].Value = null;
                }

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    // теперь списки контактов и и адресов
                    if ((this.ContactList != null) && (this.ContactList.Count > 0))
                    {
                        iRes = (CContact.SaveContactList(this.ContactList, objContactDeletedList, EnumObject.Bank, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                    }
                    if (iRes == 0)
                    {
                        if ((this.Address != null) && (this.Address.Count > 0))
                        {
                            iRes = (CAddress.SaveAddressList(this.Address, objAddressDeletedList, EnumObject.Bank, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
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

        #region Удалить из базы данных описание банка
        /// <summary>
        /// Удаляет из базы данных описание банка
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteBank]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Bank_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Bank_Guid"].Value = this.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes == 0)
                {
                    bRet = true;
                }
                else
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
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
            return (Code + ' ' + Name);
        }
    }
    /// <summary>
    /// Класс "Валюта"
    /// </summary>
    public class CCurrency : CBusinessObject
    {
        #region Свойства

        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        //private System.Guid m_uuidID;
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        /*[BrowsableAttribute(false)]
        public System.Guid ID
        {
            get { return m_uuidID; }
            set { m_uuidID = value; }
        }*/

        /// <summary>
        /// Наименование
        /// </summary>
        //private System.String m_strCurrencyName;
        /// <summary>
        /// Наименование
        /// </summary>
        /*[DisplayName("Наименование валюты")]
        [Description("Наименование валюты")]
        [Category("1. Обязательные значения")]
        [BrowsableAttribute(false)]
        public System.String CurrencyName
        {
            get { return m_strCurrencyName; }
            set { m_strCurrencyName = value; }
        }*/

        /// <summary>
        /// Сокращённое наименование
        /// </summary>
        private System.String m_strCurrencyShortName;
        /// <summary>
        /// Сокращённое наименование
        /// </summary>
        [DisplayName("Сокращённое наименование валюты")]
        [Description("Сокращённое наименование валюты")]
        [Category("1. Обязательные значения")]
        public System.String CurrencyShortName
        {
            get { return m_strCurrencyShortName; }
            set { m_strCurrencyShortName = value; }
        }

        /// <summary>
        /// Наименование Eng
        /// </summary>
        private System.String m_strCurrencyEngName;
        /// <summary>
        /// Наименование Eng
        /// </summary>
        [DisplayName("Наименование валюты на английском")]
        [Description("Наименование валюты на английском")]
        [Category("1. Обязательные значения")]
        public System.String CurrencyEngName
        {
            get { return m_strCurrencyEngName; }
            set { m_strCurrencyEngName = value; }
        }
        /// <summary>
        /// Обозначение (аббревиатура)
        /// </summary>
        private System.String m_strCurrencyAbbr;
        /// <summary>
        /// Обозначение (аббревиатура)
        /// </summary>
        [DisplayName("Обозначение (аббревиатура) валюты")]
        [Description("Обозначение (аббревиатура) валюты")]
        [Category("1. Обязательные значения")]
        public System.String CurrencyAbbr
        {
            get { return m_strCurrencyAbbr; }
            set { m_strCurrencyAbbr = value; }
        }

        /// <summary>
        /// Обозначение (аббревиатура)
        /// </summary>
        private System.String m_strCurrencyOldAbbr;
        /// <summary>
        /// Обозначение (аббревиатура)
        /// </summary>
        //[DisplayName("Обозначение (аббревиатура)")]
        //[Description("Обозначение (аббревиатура) валюты")]
        //[Category("1. Обязательные значения")]
        [BrowsableAttribute(false)]
        public System.String CurrencyOldAbbr
        {
            get { return m_strCurrencyOldAbbr; }
            set { m_strCurrencyOldAbbr = value; }
        }

        /// <summary>
        /// Код
        /// </summary>
        private System.String m_strCurrencyCode;
        /// <summary>
        /// Код
        /// </summary>
        [DisplayName("Код валюты")]
        [Description("Код валюты")]
        [Category("1. Обязательные значения")]
        public System.String CurrencyCode
        {
            get { return m_strCurrencyCode; }
            set { m_strCurrencyCode = value; }
        }
        /// <summary>
        /// Описание валюты
        /// </summary>
        private System.String m_strDescription;
        /// <summary>
        /// Описание валюты
        /// </summary>
        [DisplayName("Примечание")]
        [Description("Примечание")]
        [Category("2. Дополнительно")]
        public System.String Description
        {
            get { return m_strDescription; }
            set { m_strDescription = value; }
        }
        /// <summary>
        /// Признак "Главная валюта"
        /// </summary>
        private System.Boolean m_bIsMain;
        /// <summary>
        /// Признак "Активен"
        /// </summary>
        [DisplayName("Основная валюта учёта")]
        [Description("Основная валюта учёта")]
        [Category("1. Обязательные значения")]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public System.Boolean IsMain
        {
            get { return m_bIsMain; }
            set { m_bIsMain = value; }
        }

        /// <summary>
        /// Признак "Национальная валюта"
        /// </summary>
        [BrowsableAttribute(false)]
        public System.Boolean IsNationalCurrency {get; set;}

        #endregion

        #region Конструктор
        public CCurrency()
            : base()
        {
            m_strCurrencyShortName = "";
            m_strCurrencyEngName = "";
            m_strCurrencyAbbr = "";
            m_strDescription = "";
            m_strCurrencyCode = "";
            IsNationalCurrency = false;
        }

        public CCurrency(System.Guid uuidID, System.String strCurrencyName, System.String strCurrencyAbbr,
            System.String strCurrencyCode)
        {
            //m_uuidID = uuidID;                    // этот код был в оригинале, до того, как было добавлено наследование от CBusinessObject 29.12.11
            //m_strCurrencyName = strCurrencyName;  // этот код был в оригинале, до того, как было добавлено наследование от CBusinessObject 29.12.11
            ID = uuidID;
            Name = strCurrencyName;
            m_strCurrencyAbbr = strCurrencyAbbr;
            m_strCurrencyCode = strCurrencyCode;
            IsNationalCurrency = false;
        }
        public CCurrency(System.Guid uuidID, System.String strCurrencyName, System.String strCurrencyShortName, System.String strCurrencyEngName, System.String strCurrencyAbbr,
    System.String strCurrencyCode, System.String strDescription, System.Boolean bIsMain)
        {
            ID = uuidID;
            Name = strCurrencyName;
            //m_uuidID = uuidID;
            //m_strCurrencyName = strCurrencyName;
            m_strCurrencyShortName = strCurrencyShortName;
            m_strCurrencyEngName = strCurrencyEngName;
            m_strCurrencyAbbr = strCurrencyAbbr;
            m_strCurrencyCode = strCurrencyCode;
            m_strDescription = strDescription;
            m_bIsMain = bIsMain;
            //---
            m_strCurrencyOldAbbr = strCurrencyAbbr;
            IsNationalCurrency = false;
        }
        #endregion

        #region Список валют
        public static List<CCurrency> GetCurrencyList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CCurrency> objList = new List<CCurrency>();
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetCurrencyList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CCurrency((System.Guid)rs["Currency_Guid"],
                            (System.String)rs["Currency_Name"],
                            (System.String)rs["Currency_Abbr"],
                            (System.String)rs["Currency_Code"]) 
                            { 
                                IsNationalCurrency = System.Convert.ToBoolean(rs["IsNationalCurrency"]),
                                IsMain = System.Convert.ToBoolean(rs["Currency_IsMain"])
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
                "Не удалось получить список валют.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }

        public static List<CCurrency> GetCurrencyListForCurrency(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CCurrency> objList = new List<CCurrency>();
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCurrencyList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CCurrency((System.Guid)rs["Currency_Guid"], (System.String)rs["Currency_Name"],
                            ((rs["Currency_ShortName"] == System.DBNull.Value) ? "" : (System.String)rs["Currency_ShortName"]),
                            (System.String)rs["Currency_EngName"],
                            (System.String)rs["Currency_Abbr"], (System.String)rs["Currency_Code"],
                            ((rs["Currency_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Currency_Description"]), (System.Boolean)rs["Currency_IsMain"]));
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
                "Не удалось получить список валют.\n\nТекст ошибки: " + f.Message, "Внимание",
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

                if (this.CurrencyShortName == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать краткое наименование валюты!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }

                if (this.CurrencyEngName == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать наименование валюты на английском языке!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }

                if (this.CurrencyAbbr == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать обозначение (аббревиатуру) валюты!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.CurrencyAbbr.Count() != 3)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Обозначение (аббревиатура) валюты должено состоять из трёх символов!", "Внимание", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }

                if (this.CurrencyCode == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать код валюты!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }

                Regex r = new Regex(@"\D");
                Match m = r.Match(this.CurrencyCode); //Результат

                if (m.Success == true)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Код валюты должен состоять только из цифр!", "Внимание", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }

                if (this.CurrencyCode.Count() != 3)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Код валюты должен состоять из трёх цифр!", "Внимание", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddCurrency]", objProfile.GetOptionsDllDBName());

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Abbr", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Code", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_ShortName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_EngName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_IsMain", System.Data.DbType.Boolean));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));//**
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Currency_Abbr"].Value = this.CurrencyAbbr;
                cmd.Parameters["@Currency_Code"].Value = this.CurrencyCode;
                cmd.Parameters["@Currency_Name"].Value = this.Name;
                cmd.Parameters["@Currency_ShortName"].Value = this.CurrencyShortName;
                cmd.Parameters["@Currency_EngName"].Value = this.CurrencyEngName;
                cmd.Parameters["@Currency_Description"].Value = this.Description;
                cmd.Parameters["@Currency_IsMain"].Value = this.IsMain;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    this.ID = (System.Guid)cmd.Parameters["@Currency_Guid"].Value; // Полученный после выполнения usp_AddCurrency, GUID 

                    cmd.Parameters.Clear();
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddCurrencyToIB]", objProfile.GetOptionsDllDBName()); ;
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Code_output", System.Data.SqlDbType.NVarChar, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                    cmd.Parameters["@Currency_Guid"].Value = this.ID;
                    cmd.ExecuteNonQuery();

                    iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value; // ориганальный вариант написания
                    //iRes = (System.Int32)cmd.Parameters["@ERROR_NUM"].Value; 

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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств валюты.\n\nТекст ошибки: " + strErr, "Ошибка",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditCurrency]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Abbr", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Code", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_ShortName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_EngName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_IsMain", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Currency_Guid"].Value = this.ID;
                cmd.Parameters["@Currency_Abbr"].Value = this.CurrencyAbbr;
                cmd.Parameters["@Currency_Code"].Value = this.CurrencyCode;
                cmd.Parameters["@Currency_Name"].Value = this.Name;
                cmd.Parameters["@Currency_ShortName"].Value = this.CurrencyShortName;
                cmd.Parameters["@Currency_EngName"].Value = this.CurrencyEngName;
                cmd.Parameters["@Currency_Description"].Value = this.Description;
                cmd.Parameters["@Currency_IsMain"].Value = this.IsMain;

                string OldCod = this.CurrencyOldAbbr;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditCurrencyToIB]", objProfile.GetOptionsDllDBName()); ;
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Code_Old", System.Data.DbType.String));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                    cmd.Parameters["@Currency_Guid"].Value = this.ID;
                    cmd.Parameters["@Currency_Code_Old"].Value = OldCod;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств валюты.\n\nТекст ошибки: " + strErr, "Ошибка",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteCurrency]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Currency_Guid"].Value = this.ID;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes != 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления валюты.\n\nТекст ошибки: " +
                                                               (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                                                               System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                else
                {
                    // теперь все это нужно записать в InterBase
                    if (iRes == 0)
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteCurrencyFromIB]", objProfile.GetOptionsDllDBName()); ;
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Code", System.Data.SqlDbType.VarChar));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                        cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                        cmd.Parameters["@Currency_Code"].Value = this.CurrencyAbbr;

                        cmd.ExecuteNonQuery();
                        iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                        if (iRes != 0 && cmd.Parameters["@ERROR_MES"].Value != System.DBNull.Value)
                        {
                            DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления валюты в InterBase.\n\nТекст ошибки: " + (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        }
                        else
                        {
                            if (iRes != 0)
                            {
                                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления валюты в InterBase.\n\n Код ошибки: " + iRes.ToString(), "Ошибка", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                            }
                        }
                    }
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

        public override string ToString()
        {
            return m_strCurrencyAbbr;
        }

    }

    public class CCurrencyRate : CBusinessObject
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
        /// Дата курса валюты
        /// </summary>
        private System.DateTime m_dtCurRateDate;
        /// <summary>
        /// Дата курса валюты
        /// </summary>
        public System.DateTime CurRateDate
        {
            get { return m_dtCurRateDate; }
            set { m_dtCurRateDate = value; }
        }

        /// <summary>
        /// Уникальный идентификатор валют IN
        /// </summary>
        private System.Guid m_uiCurrencyInGuid;
        /// <summary>
        /// Уникальный идентификатор валют IN
        /// </summary>
        public System.Guid CurrencyInGuide
        {
            get { return m_uiCurrencyInGuid; }
            set { m_uiCurrencyInGuid = value; }
        }

        /// <summary>
        /// Аббревиатура IN
        /// </summary>
        private System.String m_strCurrencyAbbrIn;
        /// <summary>
        /// Аббревиатура IN
        /// </summary>
        public System.String CurrencyAbbrIn
        {
            get { return m_strCurrencyAbbrIn; }
            set { m_strCurrencyAbbrIn = value; }
        }

        /// <summary>
        /// Уникальный идентификатор валют OUT
        /// </summary>
        private System.Guid m_uiCurrencyOutGuid;
        /// <summary>
        /// Уникальный идентификатор валют OUT
        /// </summary>
        public System.Guid CurrencyOutGuide
        {
            get { return m_uiCurrencyOutGuid; }
            set { m_uiCurrencyOutGuid = value; }
        }

        /// <summary>
        /// Аббревиатура OUT
        /// </summary>
        private System.String m_strCurrencyAbbrOut;
        /// <summary>
        /// Аббревиатура OUT
        /// </summary>
        public System.String CurrencyAbbrOut
        {
            get { return m_strCurrencyAbbrOut; }
            set { m_strCurrencyAbbrOut = value; }
        }

        /// <summary>
        /// Значение курса валюты
        /// </summary>
        private System.Decimal m_dCurrencyRateValue;
        /// <summary>
        /// Значение курса валюты
        /// </summary>
        public System.Decimal CurrencyRateValue
        {
            get { return m_dCurrencyRateValue; }
            set { m_dCurrencyRateValue = value; }
        }
        /// <summary>
        /// Признак ценообразования
        /// </summary>
        private System.Boolean m_bCurrencyIsPricing;
        /// <summary>
        /// Признак ценообразования
        /// </summary>
        public System.Boolean CurrencyIsPricing
        {
            get { return m_bCurrencyIsPricing; }
            set { m_bCurrencyIsPricing = value; }
        }

        /// <summary>
        /// Признак курса, который используется в ERP_Mercury
        /// </summary>
        private System.Boolean m_bCurrencyRateIsERP;
        /// <summary>
        /// Признак ценообразования
        /// </summary>
        public System.Boolean CurrencyRateIsERP
        {
            get { return m_bCurrencyRateIsERP; }
            set { m_bCurrencyRateIsERP = value; }
        }

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        /// <summary>
        /// Список курсов валют
        /// </summary>
        private List<CCurrencyRate> m_objCurrRateList;
        /// <summary>
        /// Список курсов валют
        /// </summary>
        public List<CCurrencyRate> CurrRateList
        {
            get { return m_objCurrRateList; }
            set { m_objCurrRateList = value; }
        }

        private List<CCurrencyRate> m_objCurrRateForDeleteList;
        public List<CCurrencyRate> CurrRateForDeleteList
        {
            get { return m_objCurrRateForDeleteList; }
            set { m_objCurrRateForDeleteList = value; }
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        #endregion

        #region Конструктор
        public CCurrencyRate()
            : base()
        {
            m_IbID = 0;
            m_dtCurRateDate = System.DateTime.MinValue;
            m_uiCurrencyInGuid = System.Guid.Empty;
            m_strCurrencyAbbrIn = "";
            m_uiCurrencyOutGuid = System.Guid.Empty;
            m_strCurrencyAbbrOut = "";
            m_dCurrencyRateValue = 0;
            m_bCurrencyIsPricing = false;
            m_bCurrencyRateIsERP = false;
        }
        public CCurrencyRate(System.Guid uuidId, System.Int32 iIbId, System.DateTime dtCurRateDate, System.Guid uiCurrencyInGuid, System.String strCurrencyAbbrIn, System.Guid uiCurrencyOutGuid, System.String strCurrencyAbbrOut, System.Decimal dCurrencyRateValue, System.String strName, System.Boolean bIsPricing)
            : base(uuidId, strName)
        {
            m_IbID = iIbId;
            m_dtCurRateDate = dtCurRateDate;
            m_uiCurrencyInGuid = uiCurrencyInGuid;
            m_strCurrencyAbbrIn = strCurrencyAbbrIn;
            m_uiCurrencyOutGuid = uiCurrencyOutGuid;
            m_strCurrencyAbbrOut = strCurrencyAbbrOut;
            m_dCurrencyRateValue = dCurrencyRateValue;
            m_bCurrencyIsPricing = bIsPricing;
            m_bCurrencyRateIsERP = false;
            // strName -- просто заглушка, данных не содержит
        }
        #endregion

        #region Cписок объектов
        /// <summary>
        /// Возвращает список объектов "Курсы валют"
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список объектов "Место хранения"</returns>
        public static List<CCurrencyRate> GetCurrencyRateList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, System.DateTime dtBegin, System.DateTime dtEnd)
        {
            List<CCurrencyRate> objList = new List<CCurrencyRate>();
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("Не удалось получить соединение с базой данных.", "Внимание", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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

                // System.String sBegin = dtBegin.ToString("yyyy-MM-dd");
                //System.String sEnd = dtEnd.ToString("yyyy-MM-dd");

                //System.String sBegin = Convert.ToString(dtBegin);
                //System.String sEnd= Convert.ToString(dtEnd);

                //sBegin= sBegin.ToString("dd-mm-yyyy");

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCurrencyRateList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                //cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CurrencyRate_Date1", System.Data.DbType.String));
                //cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CurrencyRate_Date2", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CurrencyRate_Date1", System.Data.DbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CurrencyRate_Date2", System.Data.DbType.Date));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                //dtBegin = dtBegin.GetDateTimeFormats();

                cmd.Parameters["@CurrencyRate_Date1"].Value = dtBegin; //sBegin; // Convert.ToDateTime("18.08.2011");  
                cmd.Parameters["@CurrencyRate_Date2"].Value = dtEnd; //.GetDateTimeFormats(); //sEnd; //Convert.ToDateTime("18.08.2011"); 

                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CCurrencyRate(
                                                      (System.Guid)rs["CurrencyRate_Guid"],
                                                      ((rs["CurrencyRate_Id"] == System.DBNull.Value) ? 0 : (System.Int32)rs["CurrencyRate_Id"]),
                                                      (System.DateTime)rs["CurrencyRate_Date"],
                                                      (System.Guid)rs["Currency_In_Guid"],
                                                      (System.String)rs["Currency_Abbr_IN"],
                                                      (System.Guid)rs["Currency_Out_Guid"],
                                                      (System.String)rs["Currency_Abbr_OUT"],
                                                      (System.Decimal)rs["CurrencyRate_Value"],
                                                      (System.String)rs["TempName"],
                                                      (System.Boolean)rs["CurrencyRate_Pricing"]
                                                      ));
                    }
                }
                rs.Close();
                rs.Dispose(); // //

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список объектов \"Курсы валют\".\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }



        /// <summary>
        /// Возвращает список объектов "Курс ценообразования"
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список объектов "Место хранения"</returns>
        public static List<CCurrencyRate> GetCurrencyRateListPricing(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL/*, System.DateTime dtBegin, System.DateTime dtEnd*/)
        {
            List<CCurrencyRate> objList = new List<CCurrencyRate>();
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("Не удалось получить соединение с базой данных.", "Внимание", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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

                // System.String sBegin = dtBegin.ToString("yyyy-MM-dd");
                //System.String sEnd = dtEnd.ToString("yyyy-MM-dd");

                //System.String sBegin = Convert.ToString(dtBegin);
                //System.String sEnd= Convert.ToString(dtEnd);

                //sBegin= sBegin.ToString("dd-mm-yyyy");

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCurrencyRateListPricing]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                //cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CurrencyRate_Date1", System.Data.DbType.String));
                //cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CurrencyRate_Date2", System.Data.DbType.String));
                //cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CurrencyRate_Date1", System.Data.DbType.Date));
                //cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CurrencyRate_Date2", System.Data.DbType.Date));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                //dtBegin = dtBegin.GetDateTimeFormats();

                //cmd.Parameters["@CurrencyRate_Date1"].Value = dtBegin; //sBegin; // Convert.ToDateTime("18.08.2011");  
                //cmd.Parameters["@CurrencyRate_Date2"].Value = dtEnd; //.GetDateTimeFormats(); //sEnd; //Convert.ToDateTime("18.08.2011"); 

                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CCurrencyRate(
                                                      (System.Guid)rs["CurrencyRate_Guid"],
                                                      ((rs["CurrencyRate_Id"] == System.DBNull.Value) ? 0 : (System.Int32)rs["CurrencyRate_Id"]),
                                                      (System.DateTime)rs["CurrencyRate_Date"],
                                                      (System.Guid)rs["Currency_In_Guid"],
                                                      (System.String)rs["Currency_Abbr_IN"],
                                                      (System.Guid)rs["Currency_Out_Guid"],
                                                      (System.String)rs["Currency_Abbr_OUT"],
                                                      (System.Decimal)rs["CurrencyRate_Value"],
                                                      (System.String)rs["TempName"],
                                                      (System.Boolean)rs["CurrencyRate_Pricing"]
                                                      ));
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
                "Не удалось получить список объектов \"Курсы валют\".\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }

        /// <summary>
        /// Возвращает курс пересчёта
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="CurrencyIn">валюта (из какой)</param>
        /// <param name="CurrencyOut">валюта (в какую)</param>
        /// <param name="dtBegin">дата</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>курс</returns>
        public static System.Decimal GetCurrencyRate(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid CurrencyIn, System.Guid CurrencyOut,
            System.DateTime dtBegin, ref System.String strErr)
        {
            System.Decimal objRet = 0;
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCurrencyRate]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CurrencyIn", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CurrencyOut", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BEGINDATE", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Rate_Value", System.Data.SqlDbType.Money) { Direction = System.Data.ParameterDirection.Output });

                cmd.Parameters["@CurrencyIn"].Value = CurrencyIn;
                cmd.Parameters["@CurrencyOut"].Value = CurrencyOut;
                cmd.Parameters["@BEGINDATE"].Value = dtBegin;

                cmd.ExecuteNonQuery();

                System.Int32 iRet = System.Convert.ToInt32(cmd.Parameters["@ERROR_NUM"].Value);
                if (iRet == 0)
                {
                    objRet = System.Convert.ToDecimal(cmd.Parameters["@Rate_Value"].Value);
                }
                else
                {
                    strErr += System.Convert.ToString(cmd.Parameters["@ERROR_MES"].Value);
                }

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                strErr += ("Запрос курса. Текст ошибки: " + f.Message);
            }
            return objRet;
        }
        #endregion

        #region Валидация
        /// <summary>
        /// Проверка свойств контакта перед сохранением
        /// </summary>
        /// <param name="strErr">текст с ошибкой</param>
        /// <returns>true - все свойства корректны; false - ошибка</returns>
        public System.Boolean IsAllParametersValid(ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (this.CurrencyAbbrIn.Trim() == "")
                {
                    strErr = "Необходимо указать Необходимо указать аббревиатуру валюты !";
                    return bRet;
                }
                if (this.CurrencyAbbrOut.Trim() == "")
                {
                    strErr = "Необходимо указать аббревиатуру кода валюты !";
                    return bRet;
                }
                // временно закоментил ==0.0 Разобраться почему не работает и исправить
                if ( /*(this.CurrencyRateValue == 0.0) ||*/ (this.CurrencyRateValue == 0))
                {
                    strErr = "Необходимо указать курс валюты !";
                    return bRet;
                }

                // здесь написать проверку, чтобы во всех гридах были одинаковые даты.


                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr = "Ошибка проверки свойств. Текст ошибки: " + f.Message;
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddCurrencyRate]", objProfile.GetOptionsDllDBName());

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CurrencyRate_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CurrencyRate_Date", System.Data.DbType.DateTime));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_In_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Out_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CurrencyRate_Value", System.Data.DbType.Decimal)); // возможно нужен другой тип, не Double
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CurrencyRate_Pricing", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));//**
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@CurrencyRate_Date"].Value = this.CurRateDate;     // здесь поля пустые, протсо найти нужный обехет который был заполнен ранее
                cmd.Parameters["@Currency_In_Guid"].Value = this.CurrencyInGuide;
                cmd.Parameters["@Currency_Out_Guid"].Value = this.CurrencyOutGuide;
                cmd.Parameters["@CurrencyRate_Value"].Value = this.CurrencyRateValue;
                cmd.Parameters["@CurrencyRate_Pricing"].Value = this.CurrencyIsPricing;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    this.ID = (System.Guid)cmd.Parameters["@CurrencyRate_Guid"].Value; // Полученный после выполнения, GUID 

                    cmd.Parameters.Clear();
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddCurrencyRateToIB]", objProfile.GetOptionsDllDBName()); ;
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    //cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Code_output", System.Data.SqlDbType.NVarChar, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    //cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Id", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CurrencyRate_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                    cmd.Parameters["@CurrencyRate_Guid"].Value = this.ID;
                    cmd.ExecuteNonQuery();

                    iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value; // ориганальный вариант написания

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

                    //--------------------
                    // Этот фрагмент удалаяет из ERP_Mercury 4 валютные пары. Остаётся 5 пар
                    /*
                    if (iRes==0)
                    {
                        System.Boolean bRes;

                        if (!this.CurrencyRateIsERP)
                        {
                            bRes = Remove(objProfile, cmd, ref strErr, false);
                            if (bRes)
                            {
                                iRes = 0;
                            }
                            else
                            {
                                iRes = 1;
                            }
                        }
                    }
                    */
                    //---------------------
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
                    /*DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств валюты.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);*/
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
                "Не удалось изменить курс валюты.\n\nТекст ошибки: " + f.Message, "Внимание",
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
        /// Сохраняет в БД список курсов валют
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="objAccountList">список расчетных счетов</param>
        /// <param name="enObjectWithAddress">тип владельца расчетных счетов</param>
        /// <param name="uuidObjectId">идентификатор владельца расчетных счетов</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean SaveCurrencyRateList(List<CCurrencyRate> objCurrencyRateList, List<CCurrencyRate> objCurrencyRateForDeleteList,
            /*EnumObject enObject, */ /*System.Guid uuidObjectId, */ UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            if ((objCurrencyRateList == null) && (objCurrencyRateForDeleteList == null)) { return true; }
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                // для начала проверим, что нам пришло в списке
                if ((objCurrencyRateList != null) && (objCurrencyRateList.Count > 0))
                {
                    System.Boolean bIsAllValid = true;
                    foreach (CCurrencyRate objItem in objCurrencyRateList)
                    {
                        if (objItem.IsAllParametersValid(ref strErr) == false)
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
                if ((objCurrencyRateForDeleteList != null) && (objCurrencyRateForDeleteList.Count > 0))
                {
                    foreach (CCurrencyRate objCurrRate in objCurrencyRateForDeleteList)
                    {
                        if (objCurrRate.ID.CompareTo(System.Guid.Empty) == 0) { continue; }
                        // Раскоментить, когда будет написан метод Remove 27.01.2012
                        iRes = (objCurrRate.Remove(objProfile, cmd, ref strErr, true) == true) ? 0 : 1;
                        if (iRes != 0) { break; }
                    }
                }

                if (iRes == 0)
                {
                    if ((objCurrencyRateList != null) && (objCurrencyRateList.Count > 0))
                    {
                        // теперь в цикле добавим в БД каждый член из списка
                        foreach (CCurrencyRate objCurrRate in objCurrencyRateList)
                        {
                            if (objCurrRate.ID.CompareTo(System.Guid.Empty) == 0)
                            {
                                // ADD
                                iRes = (objCurrRate.Add(objProfile, null, ref strErr) == true) ? 0 : -1;
                            }
                            else
                            {
                                // UPDATE
                                iRes = (objCurrRate.Update(objProfile, cmd, ref strErr) == true) ? 0 : -1;
                                // вставить прогресс бар где-то здесь
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

        #region Update
        /// <summary>
        /// Сохранить изменения в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Update(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            System.Boolean bRet = false;

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            strErr = "";
            try
            {
                if (this.IsAllParametersValid(ref strErr) == false)
                {
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditCurrencyRate]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CurrencyRate_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CurrencyRate_Id", System.Data.DbType.Int32));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CurrencyRate_Date", System.Data.DbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_In_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Out_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CurrencyRate_Value", System.Data.DbType.Decimal));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CurrencyRate_Pricing", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@CurrencyRate_Guid"].Value = this.ID;
                cmd.Parameters["@CurrencyRate_Id"].Value = this.InterBaseID;
                cmd.Parameters["@CurrencyRate_Date"].Value = this.CurRateDate;
                cmd.Parameters["@Currency_In_Guid"].Value = this.CurrencyInGuide;
                cmd.Parameters["@Currency_Out_Guid"].Value = this.CurrencyOutGuide;
                cmd.Parameters["@CurrencyRate_Value"].Value = this.CurrencyRateValue;
                cmd.Parameters["@CurrencyRate_Pricing"].Value = this.CurrencyIsPricing;

                //string OldCod = this.CurrencyOldAbbr;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditCurrencyRateToIB]", objProfile.GetOptionsDllDBName()); ;
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CurrencyRate_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                    cmd.Parameters["@CurrencyRate_Guid"].Value = this.ID;
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
                    /*DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения курсов валюты.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);*/
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
                "Не удалось изменить курс валюты.\n\nТекст ошибки: " + f.Message, "Внимание",
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
        /// <param name="uuidID">уникальный идентификатор объекта</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean Remove(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr, System.Boolean bIbDel)
        {
            System.Int32 iRes = 1;
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


                if (bIbDel == true)
                {
                    //сперва удаляем в InterBase
                    // временно закоментим
                    iRes = DeleteCurRateFromIB(objProfile, cmd, ref strErr);
                }
                else
                {
                    // закоментировать
                    //iRes = 0;
                }


                if (iRes == 0) // если всё OK
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteCurrencyRate]", objProfile.GetOptionsDllDBName());
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CurrencyRate_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters["@CurrencyRate_Guid"].Value = this.ID;
                    cmd.ExecuteNonQuery();
                    iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }

                //iRes = DeleteCurRateFromIB(objProfile, cmd, ref strErr);

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
        /// Удаляем курс валют из InterBase
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>0 - удачное завершение операции; <>0 - ошибка</returns>

        private System.Int32 DeleteCurRateFromIB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
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
                    cmdSQL.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteCurrencyRateFromIB]", objProfile.GetOptionsDllDBName()); ;
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CurrencyRate_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmdSQL.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                    cmdSQL.Parameters["@CurrencyRate_Guid"].Value = this.ID;
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

    /// <summary>
    /// Класс "Тип расчетного счета"
    /// </summary>
    public class CAccountType : CBusinessObject
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
        /// Примечание
        /// </summary>
        private System.String m_strDescription;
        /// <summary>
        /// Примечание
        /// </summary>
        [DisplayName("Описание")]
        [Description("Описание")]
        [Category("2. Дополнительно")]
        public System.String Description
        {
            get { return m_strDescription; }
            set { m_strDescription = value; }
        }
        #endregion

        #region Конструктор
        public CAccountType()
            : base()
        {
        }
        public CAccountType(System.Guid uuidId, System.String strName, System.String strDescription,
            System.Boolean bIsActive)
        {
            ID = uuidId;
            Name = strName;
            m_IsActive = bIsActive;
            m_strDescription = strDescription;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращае список типов расчетных счетов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список типов расчетных счетов</returns>
        public static List<CAccountType> GetAccountTypeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CAccountType> objList = new List<CAccountType>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetAccountType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CAccountType((System.Guid)rs["AccountType_Guid"],
                            (System.String)rs["AccountType_Name"],
                            ((rs["AccountType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["AccountType_Description"]),
                            (System.Boolean)rs["AccountType_IsActive"]));
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
                "Не удалось получить список типов расчетных счетов.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddAccountType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AccountType_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AccountType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AccountType_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@AccountType_Name"].Value = this.Name;
                cmd.Parameters["@AccountType_Description"].Value = this.Description;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@AccountType_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания типа расчетного счета.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось создать тип расчетного счета.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteAccountType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AccountType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@AccountType_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления типа расчетного счета.\n\nТекст ошибки: " +
                    (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить тип расчетного счета.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditAccountType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AccountType_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AccountType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AccountType_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AccountType_IsActive", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@AccountType_Guid"].Value = this.ID;
                cmd.Parameters["@AccountType_Name"].Value = this.Name;
                cmd.Parameters["@AccountType_Description"].Value = this.Description;
                cmd.Parameters["@AccountType_IsActive"].Value = this.IsActive;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения типа расчетного счета.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства типа расчетного счета.\n\nТекст ошибки: " + f.Message, "Внимание",
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
    /// Класс "Расчетный счет"
    /// </summary>
    public class CAccount
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
        /// Номер расчетного счета
        /// </summary>
        private System.String m_strAccountNumber;
        /// <summary>
        /// Номер расчетного счета
        /// </summary>
        public System.String AccountNumber
        {
            get { return m_strAccountNumber; }
            set { m_strAccountNumber = value; }
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
        /// Банк
        /// </summary>
        private CBank m_objBank;
        /// <summary>
        /// Банк
        /// </summary>
        public CBank Bank
        {
            get { return m_objBank; }
            set { m_objBank = value; }
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
        /// Тип расчетного счета
        /// </summary>
        private CAccountType m_objAccountType;
        /// <summary>
        /// Тип расчетного счета
        /// </summary>
        public CAccountType AccountType
        {
            get { return m_objAccountType; }
            set { m_objAccountType = value; }
        }
        /// <summary>
        /// Признак "Основной р/с"
        /// </summary>
        private System.Boolean m_bIsMain;
        /// <summary>
        /// Признак "Основной р/с"
        /// </summary>
        public System.Boolean IsMain
        {
            get { return m_bIsMain; }
            set { m_bIsMain = value; }
        }
        public System.String BankName
        {
            get { return ( ( Bank == null ) ? "" : Bank.ToString());}
        }
        public System.String AccountTypeName
        {
            get { return ((AccountType == null) ? "" : AccountType.Name); }
        }
        public System.String CurrencyCode
        {
            get { return ((Currency == null) ? "" : Currency.CurrencyAbbr); }
        }
        #endregion

        #region Конструктор
        public CAccount()
        {
            m_uuidID = System.Guid.Empty;
            m_objBank = null;
            m_objCurrency = null;
            m_objAccountType = null;
            m_strAccountNumber = "";
            m_strDescription = "";
            m_bIsMain = false;
        }
        public CAccount(System.Guid uuidID, CBank objBank, CCurrency objCurrency,
            System.String strAccountNumber, System.String strDescription, CAccountType objAccountType)
        {
            m_uuidID = uuidID;
            m_objBank = objBank;
            m_objCurrency = objCurrency;
            m_objAccountType = objAccountType;
            m_strAccountNumber = strAccountNumber;
            m_strDescription = strDescription;
        }

        public CAccount(System.Guid uuidID, CBank objBank, CCurrency objCurrency,
            System.String strAccountNumber, System.String strDescription, CAccountType objAccountType, System.Boolean bIsMain)
        {
            m_uuidID = uuidID;
            m_objBank = objBank;
            m_objCurrency = objCurrency;
            m_objAccountType = objAccountType;
            m_strAccountNumber = strAccountNumber;
            m_strDescription = strDescription;
            m_bIsMain = bIsMain;
        }

        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список расчетных счетов для клиента
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="uuidCustomerId">уникальный идентификатор клиента</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>список расчетных счетов</returns>
        public static List<CAccount> GetAccountListForCustomer(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidCustomerId, ref System.String strErr)
        {
            List<CAccount> objList = new List<CAccount>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetAccountForCustomer]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Customer_Guid"].Value = uuidCustomerId;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.String strAccDscrpn = "";
                    System.String strBankDscrpn = "";
                    System.String strWWW = "";
                    System.Guid ParentId = System.Guid.Empty;
                    CBank objBank = null;
                    CCurrency objCurrency = null;
                    CAccountType objAccountType = null;
                    while (rs.Read())
                    {
                        objCurrency = new CCurrency((System.Guid)rs["Currency_Giud"], (System.String)rs["Currency_Name"],
                            (System.String)rs["Currency_Abbr"], (System.String)rs["Currency_Code"]);

                        objAccountType = new CAccountType((System.Guid)rs["AccountType_Guid"],
                            (System.String)rs["AccountType_Name"],
                            ((rs["AccountType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["AccountType_Description"]),
                            (System.Boolean)rs["AccountType_IsActive"]);

                        strBankDscrpn = (rs["Bank_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Bank_Description"];
                        strWWW = (rs["Bank_WWW"] == System.DBNull.Value) ? "" : (System.String)rs["Bank_WWW"];
                        ParentId = (rs["Bank_ParentGuid"] == System.DBNull.Value) ? System.Guid.Empty : (System.Guid)rs["Bank_ParentGuid"];

                        objBank = new CBank((System.Guid)rs["Bank_Guid"], (System.String)rs["Bank_Name"], ParentId,
                            (System.String)rs["Bank_Code"], strBankDscrpn, (System.String)rs["Bank_UNN"], (System.String)rs["Bank_MFO"],
                            strWWW, (System.Boolean)rs["Bank_IsActive"]);

                        strAccDscrpn = (rs["Account_Ddescription"] == System.DBNull.Value) ? "" : (System.String)rs["Account_Ddescription"];

                        objList.Add(new CAccount((System.Guid)rs["Account_Guid"], objBank, objCurrency,
                            (System.String)rs["Account_Number"], strAccDscrpn, objAccountType));
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
                strErr = "Не удалось получить список расчетных счетов.\n\nТекст ошибки: " + f.Message;
            }
            return objList;
        }


        /// <summary>
        /// Возвращает список расчетных счетов для компании
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="uuidCustomerId">уникальный идентификатор компании</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>список расчетных счетов</returns>
        public static List<CAccount> GetAccountListForCompany(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidCompanyId, ref System.String strErr)
        {
            List<CAccount> objList = new List<CAccount>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetAccountForCompany]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Company_Guid"].Value = uuidCompanyId;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.String strAccDscrpn = "";
                    System.String strBankDscrpn = "";
                    System.String strWWW = "";
                    System.Guid ParentId = System.Guid.Empty;
                    CBank objBank = null;
                    CCurrency objCurrency = null;
                    CAccountType objAccountType = null;
                    while (rs.Read())
                    {
                        objCurrency = new CCurrency((System.Guid)rs["Currency_Giud"], (System.String)rs["Currency_Name"],
                            (System.String)rs["Currency_Abbr"], (System.String)rs["Currency_Code"]);

                        objAccountType = new CAccountType((System.Guid)rs["AccountType_Guid"],
                            (System.String)rs["AccountType_Name"],
                            ((rs["AccountType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["AccountType_Description"]),
                            (System.Boolean)rs["AccountType_IsActive"]);

                        strBankDscrpn = (rs["Bank_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Bank_Description"];
                        strWWW = (rs["Bank_WWW"] == System.DBNull.Value) ? "" : (System.String)rs["Bank_WWW"];
                        ParentId = (rs["Bank_ParentGuid"] == System.DBNull.Value) ? System.Guid.Empty : (System.Guid)rs["Bank_ParentGuid"];

                        objBank = new CBank((System.Guid)rs["Bank_Guid"], (System.String)rs["Bank_Name"], ParentId,
                            (System.String)rs["Bank_Code"], strBankDscrpn, (System.String)rs["Bank_UNN"], (System.String)rs["Bank_MFO"],
                            strWWW, (System.Boolean)rs["Bank_IsActive"]);

                        strAccDscrpn = (rs["Account_Ddescription"] == System.DBNull.Value) ? "" : (System.String)rs["Account_Ddescription"];

                        objList.Add(new CAccount((System.Guid)rs["Account_Guid"], objBank, objCurrency,
                            (System.String)rs["Account_Number"], strAccDscrpn, objAccountType, (System.Boolean)rs["CompanyAccount_IsMain"]));
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
                strErr = "Не удалось получить список расчетных счетов.\n\nТекст ошибки: " + f.Message;
            }
            return objList;
        }

        /// <summary>
        /// Возвращает список расчетных счетов для перевозчика
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="uuidCarrierId">уникальный идентификатор перевозчика</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>список расчетных счетов</returns>
        public static List<CAccount> GetAccountListForCarrier(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidCarrierId, ref System.String strErr)
        {
            List<CAccount> objList = new List<CAccount>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetAccountForCarrier]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Carrier_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Carrier_Guid"].Value = uuidCarrierId;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.String strAccDscrpn = "";
                    System.String strBankDscrpn = "";
                    System.String strWWW = "";
                    System.Guid ParentId = System.Guid.Empty;
                    CBank objBank = null;
                    CCurrency objCurrency = null;
                    CAccountType objAccountType = null;
                    while (rs.Read())
                    {
                        objCurrency = new CCurrency((System.Guid)rs["Currency_Giud"], (System.String)rs["Currency_Name"],
                            (System.String)rs["Currency_Abbr"], (System.String)rs["Currency_Code"]);

                        objAccountType = new CAccountType((System.Guid)rs["AccountType_Guid"],
                            (System.String)rs["AccountType_Name"],
                            ((rs["AccountType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["AccountType_Description"]),
                            (System.Boolean)rs["AccountType_IsActive"]);

                        strBankDscrpn = (rs["Bank_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Bank_Description"];
                        strWWW = (rs["Bank_WWW"] == System.DBNull.Value) ? "" : (System.String)rs["Bank_WWW"];
                        ParentId = (rs["Bank_ParentGuid"] == System.DBNull.Value) ? System.Guid.Empty : (System.Guid)rs["Bank_ParentGuid"];

                        objBank = new CBank((System.Guid)rs["Bank_Guid"], (System.String)rs["Bank_Name"], ParentId,
                            (System.String)rs["Bank_Code"], strBankDscrpn, (System.String)rs["Bank_UNN"], (System.String)rs["Bank_MFO"],
                            strWWW, (System.Boolean)rs["Bank_IsActive"]);

                        strAccDscrpn = (rs["Account_Ddescription"] == System.DBNull.Value) ? "" : (System.String)rs["Account_Ddescription"];

                        objList.Add(new CAccount((System.Guid)rs["Account_Guid"], objBank, objCurrency,
                            (System.String)rs["Account_Number"], strAccDscrpn, objAccountType, (System.Boolean)rs["CompanyAccount_IsMain"]));
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
                strErr = "Не удалось получить список расчетных счетов.\n\nТекст ошибки: " + f.Message;
            }
            return objList;
        }



        /// <summary>
        /// Возвращает список расчетных счетов для поставщика
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="uuidCustomerId">уникальный идентификатор поставщика</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>список расчетных счетов</returns>
        public static List<CAccount> GetAccountListForVendor(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidVendorId, ref System.String strErr)
        {
            List<CAccount> objList = new List<CAccount>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetAccountForVendor]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Vendor_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Vendor_Guid"].Value = uuidVendorId;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.String strAccDscrpn = "";
                    System.String strBankDscrpn = "";
                    System.String strWWW = "";
                    System.Guid ParentId = System.Guid.Empty;
                    CBank objBank = null;
                    CCurrency objCurrency = null;
                    CAccountType objAccountType = null;
                    while (rs.Read())
                    {
                        objCurrency = new CCurrency((System.Guid)rs["Currency_Giud"], (System.String)rs["Currency_Name"],
                            (System.String)rs["Currency_Abbr"], (System.String)rs["Currency_Code"]);

                        objAccountType = new CAccountType((System.Guid)rs["AccountType_Guid"],
                            (System.String)rs["AccountType_Name"],
                            ((rs["AccountType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["AccountType_Description"]),
                            (System.Boolean)rs["AccountType_IsActive"]);

                        strBankDscrpn = (rs["Bank_Description"] == System.DBNull.Value) ? "" : (System.String)rs["Bank_Description"];
                        strWWW = (rs["Bank_WWW"] == System.DBNull.Value) ? "" : (System.String)rs["Bank_WWW"];
                        ParentId = (rs["Bank_ParentGuid"] == System.DBNull.Value) ? System.Guid.Empty : (System.Guid)rs["Bank_ParentGuid"];

                        objBank = new CBank((System.Guid)rs["Bank_Guid"], (System.String)rs["Bank_Name"], ParentId,
                            (System.String)rs["Bank_Code"], strBankDscrpn, (System.String)rs["Bank_UNN"], (System.String)rs["Bank_MFO"],
                            strWWW, (System.Boolean)rs["Bank_IsActive"]);

                        strAccDscrpn = (rs["Account_Ddescription"] == System.DBNull.Value) ? "" : (System.String)rs["Account_Ddescription"];

                        objList.Add(new CAccount((System.Guid)rs["Account_Guid"], objBank, objCurrency,
                            (System.String)rs["Account_Number"], strAccDscrpn, objAccountType, (System.Boolean)rs["VendorAccount_IsMain"]));
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
                strErr = "Не удалось получить список расчетных счетов.\n\nТекст ошибки: " + f.Message;
            }
            return objList;
        }

        #endregion

        #region Добавить в БД расчетный счет
        /// <summary>
        /// Проверка свойств расчетного счета перед сохранением
        /// </summary>
        /// <param name="strErr">текст с ошибкой</param>
        /// <returns>true - все свойства корректны; false - ошибка</returns>
        public System.Boolean IsAllParametersValid(ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                Regex r = new Regex(@"\D");
                Match m = r.Match(this.m_strAccountNumber); //Результат

                if (this.m_strAccountNumber == "")
                {
                    strErr = "Необходимо указать номер расчетного счета!";
                    return bRet;
                }
                //if (m.Success == true)
                //{
                //    strErr = "Расчётный счёт должен состоять только из цифр!";
                //    return bRet;
                //}
                /*if (this.m_strAccountNumber.Count() != 13)
                {
                    strErr = "Расчётный счёт должен состоять из 13 цифр!";                    
                    return bRet;
                }*/
                if (this.m_objBank == null)
                {
                    strErr = "Необходимо указать банк!";
                    return bRet;
                }
                if (this.m_objCurrency == null)
                {
                    strErr = "Необходимо указать валюту расчетного счета!";
                    return bRet;
                }
                if (this.m_objAccountType == null)
                {
                    strErr = "Необходимо указать тип расчетного счета!";
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
        /// <summary>
        /// Добавляет расчетный счет в базу данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="enObjectWithAddress">тип владельца расчетного счета</param>
        /// <param name="uuidObjectId">идентификатор владельца расчетного счета</param>
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

                System.String strAddCmd = "";
                System.String strParamOwnerIdName = "";
                switch (enObjectWithAddress)
                {
                    case EnumObject.Customer:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[sp_AddAccountToCustomer]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Customer_Guid";
                            break;
                        }
                    case EnumObject.Company:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[usp_AddAccountToCompany]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Company_Guid";
                            cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Account_IsMain", System.Data.SqlDbType.Bit));
                            cmd.Parameters["@Account_IsMain"].Value = this.IsMain;  // добавил
                            break;
                        }
                    case EnumObject.Carrier:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[usp_AddAccountToCarrier]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Carrier_Guid";
                            break;
                        }
                    case EnumObject.Vendor:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[usp_AddAccountToVendor]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Vendor_Guid";
                            //cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Account_IsMain", System.Data.SqlDbType.Bit));
                            //cmd.Parameters["@Account_IsMain"].Value = this.IsMain;
                            break;
                        }
                    default:
                        break;
                }
                //if (cmd.CommandText != strAddCmd)
                //{
                cmd.Parameters.Clear();

                cmd.CommandText = strAddCmd;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Account_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(strParamOwnerIdName, System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Account_Number", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Bank_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AccountType_Guid", System.Data.SqlDbType.UniqueIdentifier));


                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                //}
                cmd.Parameters[strParamOwnerIdName].Value = uuidObjectId;
                cmd.Parameters["@Account_Number"].Value = this.AccountNumber;
                cmd.Parameters["@Bank_Guid"].Value = this.Bank.ID;
                cmd.Parameters["@Currency_Guid"].Value = this.Currency.ID;
                cmd.Parameters["@AccountType_Guid"].Value = this.AccountType.ID;


                if (this.Description != "")
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Account_Ddescription", System.Data.DbType.String));
                    cmd.Parameters["@Account_Ddescription"].Value = this.Description;
                }
                if (enObjectWithAddress == EnumObject.Company || enObjectWithAddress == EnumObject.Vendor)
                {
                    // проверить, как работает этот фрагмент кода
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Account_IsMain", System.Data.SqlDbType.Bit)); // !!!
                    cmd.Parameters["@Account_IsMain"].Value = this.IsMain; // !!!
                }

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    // 2009.08.11
                    // в том случае, если расчетный счет сохраняется из его владельца, и вдруг нужно откатить транзакцию, то
                    // ID должен остаться пустым
                    //this.m_uuidID = (System.Guid)cmd.Parameters["@Account_Guid"].Value;
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
        /// <summary>
        /// Сохраняет в БД список расчетных счетов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="objAccountList">список расчетных счетов</param>
        /// <param name="enObjectWithAddress">тип владельца расчетных счетов</param>
        /// <param name="uuidObjectId">идентификатор владельца расчетных счетов</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public static System.Boolean SaveAccountList(List<CAccount> objAccountList, List<CAccount> objAccountForDeleteList,
            EnumObject enObjectWithAddress, System.Guid uuidObjectId,
            UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            if ((objAccountList == null) && (objAccountForDeleteList == null)) { return true; }
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            try
            {
                // для начала проверим, что нам пришло в списке
                if ((objAccountList != null) && (objAccountList.Count > 0))
                {
                    System.Boolean bIsAllValid = true;
                    foreach (CAccount objItem in objAccountList)
                    {
                        if (objItem.IsAllParametersValid(ref strErr) == false)
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
                if ((objAccountForDeleteList != null) && (objAccountForDeleteList.Count > 0))
                {
                    foreach (CAccount objAccounts in objAccountForDeleteList)
                    {
                        if (objAccounts.ID.CompareTo(System.Guid.Empty) == 0) { continue; }
                        iRes = (objAccounts.Remove(enObjectWithAddress, uuidObjectId, objProfile, cmd, ref strErr) == true) ? 0 : 1;
                        if (iRes != 0) { break; }
                    }
                }

                if (iRes == 0)
                {
                    if ((objAccountList != null) && (objAccountList.Count > 0))
                    {
                        // теперь в цикле добавим в БД каждый член из списка
                        foreach (CAccount objAccount in objAccountList)
                        {
                            if (objAccount.ID.CompareTo(System.Guid.Empty) == 0)
                            {
                                // новый расчетный счет
                                iRes = (objAccount.Add(enObjectWithAddress, uuidObjectId, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                            }
                            else
                            {
                                iRes = (objAccount.Update(enObjectWithAddress, uuidObjectId, objProfile, cmd, ref strErr) == true) ? 0 : -1;
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

        #region Изменение свойств расчетного счета в БД
        /// <summary>
        /// Изменение свойств расчетного счета в БД
        /// </summary>
        /// <param name="enObjectWithAddress">тип владельца расчетного счета</param>
        /// <param name="uuidObjectId">идентификатор владельца расчетного счета</param>
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

                System.String strAddCmd = "";
                System.String strParamOwnerIdName = "";
                switch (enObjectWithAddress)
                {
                    case EnumObject.Customer:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[sp_EditAccountCustomer]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Customer_Guid";
                            break;
                        }
                    case EnumObject.Company:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[usp_EditAccountCompany]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Company_Guid";
                            break;
                        }
                    case EnumObject.Carrier:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[usp_EditAccountCarrier]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Carrier_Guid";
                            break;
                        }
                    case EnumObject.Vendor:
                        {
                            strAddCmd = System.String.Format("[{0}].[dbo].[usp_EditAccountVendor]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Vendor_Guid";
                            break;
                        }
                    default:
                        break;
                }
                //if (cmd.CommandText != strAddCmd)
                //{
                cmd.Parameters.Clear();

                cmd.CommandText = strAddCmd;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(strParamOwnerIdName, System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Account_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Account_Number", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Bank_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AccountType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Account_IsMain", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters[strParamOwnerIdName].Value = uuidObjectId;
                cmd.Parameters["@Account_Guid"].Value = this.ID;
                cmd.Parameters["@Account_Number"].Value = this.AccountNumber;
                cmd.Parameters["@Bank_Guid"].Value = this.Bank.ID;
                cmd.Parameters["@Currency_Guid"].Value = this.Currency.ID;
                cmd.Parameters["@AccountType_Guid"].Value = this.AccountType.ID;
                cmd.Parameters["@Account_IsMain"].Value = this.IsMain;

                if (this.Description != "")
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Account_Ddescription", System.Data.DbType.String));
                    cmd.Parameters["@Account_Ddescription"].Value = this.Description;
                }

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = "Ошибка редактирования расчетного счета. Текст ошибки: " + (System.String)cmd.Parameters["@ERROR_MES"].Value;
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
                strErr = "Ошибка редактирования расчетного счета. Текст ошибки: " + f.Message;
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

        #region Удалить расчетный счет из базы данных
        /// <summary>
        /// Удаляет трасчетный счет из базы данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="enObjectWithAddress">тип владельца расчетного счета</param>
        /// <param name="uuidObjectId">идентификатор владельца расчетного счета</param>
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

                cmd.Parameters.Clear();
                System.String strDeleteCmd = "";
                System.String strParamOwnerIdName = "";
                switch (enObjectWithAddress)
                {
                    case EnumObject.Customer:
                        {
                            strDeleteCmd = System.String.Format("[{0}].[dbo].[sp_DeleteAccountFromCustomer]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Customer_Guid";
                            break;
                        }
                    case EnumObject.Company:
                        {
                            strDeleteCmd = System.String.Format("[{0}].[dbo].[usp_DeleteAccountFromCompany]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Company_Guid";
                            break;
                        }
                    case EnumObject.Carrier:
                        {
                            strDeleteCmd = System.String.Format("[{0}].[dbo].[usp_DeleteAccountFromCarrier]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Carrier_Guid";
                            break;
                        }
                    case EnumObject.Vendor:
                        {
                            strDeleteCmd = System.String.Format("[{0}].[dbo].[usp_DeleteAccountFromVendor]", objProfile.GetOptionsDllDBName());
                            strParamOwnerIdName = "@Vendor_Guid";
                            break;
                        }
                    default:
                        break;
                }
                cmd.CommandText = strDeleteCmd;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Account_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter(strParamOwnerIdName, System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters[strParamOwnerIdName].Value = uuidObjectId;
                cmd.Parameters["@Account_Guid"].Value = this.ID;
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

        public override string ToString()
        {
            return m_strAccountNumber;
        }
    }

    public class CAccountDatabaseModel
    {
        #region Добавить объект в базу данных
        /// <summary>
        /// Проверка значений полей объекта перед сохранением в базе данных
        /// </summary>
        public static System.Boolean IsAllParametersValid(System.Guid Bank_Guid, System.Guid Currency_Guid,
            System.String Account_Number,  System.String Account_Description,
            System.Guid AccountType_Guid, 
            ref System.String strErr)
        {

            
            System.Boolean bRet = false;
            System.Int32 iWarningCount = 0;
            try
            {
                if (Bank_Guid.Equals(System.Guid.Empty) == true)
                {
                    strErr += ("\nНеобходимо указать банк!");
                    iWarningCount++;
                }
                if (Currency_Guid.Equals(System.Guid.Empty) == true)
                {
                    strErr += ("\nНеобходимо указать валюту!");
                    iWarningCount++;
                }
                if (AccountType_Guid.Equals(System.Guid.Empty) == true)
                {
                    strErr += ("\nНеобходимо тип расчётного счёта!");
                    iWarningCount++;
                }
                if (System.String.IsNullOrEmpty(Account_Number) == true)
                {
                    strErr += ("\nНеобходимо указать расчётный счёт!");
                    iWarningCount++;
                }

                bRet = (iWarningCount == 0);
            }
            catch (System.Exception f)
            {
                strErr += (String.Format("Ошибка проверки свойств объекта 'расчётный счёт'. Текст ошибки: {0}", f.Message));
            }
            return bRet;
        }
        /// <summary>
        /// Добавляет запись в базу данных
        /// </summary>
        public static System.Boolean AddNewObjectToDataBase( System.Guid Bank_Guid, System.Guid Currency_Guid,
            System.String Account_Number, System.String Account_Description, System.Guid AccountType_Guid,
            ref System.Guid Account_Guid,
            UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;

            if( IsAllParametersValid( Bank_Guid, Currency_Guid, Account_Number, Account_Description, AccountType_Guid,
                ref strErr) == false)
            { return bRet; }

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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddAccount]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Account_Guid", System.Data.SqlDbType.UniqueIdentifier) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Bank_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Account_Number", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Account_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AccountType_Guid", System.Data.SqlDbType.UniqueIdentifier));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });

                if (Account_Description.Equals(System.String.Empty) == true)
                {
                    cmd.Parameters["@Account_Description"].IsNullable = true;
                    cmd.Parameters["@Account_Description"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@Account_Description"].IsNullable = false;
                    cmd.Parameters["@Account_Description"].Value = Account_Description;
                }

                cmd.Parameters["@Bank_Guid"].Value = Bank_Guid;
                cmd.Parameters["@Currency_Guid"].Value = Currency_Guid;
                cmd.Parameters["@Account_Number"].Value = Account_Number;
                cmd.Parameters["@AccountType_Guid"].Value = AccountType_Guid;


                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    // подтверждаем транзакцию
                    DBTransaction.Commit();

                    Account_Guid = (System.Guid)cmd.Parameters["@Account_Guid"].Value;
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
                strErr += ("Не удалось создать объект 'расчётный счёт'. Текст ошибки: " + f.Message);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }

        #endregion

        #region Редактировать объект в базе данных
        public static System.Boolean EditObjectInDataBase(System.Guid Account_Guid, System.Guid Bank_Guid, System.Guid Currency_Guid,
            System.String Account_Number, System.String Account_Description, System.Guid AccountType_Guid,
            UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;

            if (IsAllParametersValid(Bank_Guid, Currency_Guid, Account_Number, Account_Description, AccountType_Guid,
                ref strErr) == false)
            { return bRet; }

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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditAccount]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Account_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Bank_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Currency_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Account_Number", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Account_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AccountType_Guid", System.Data.SqlDbType.UniqueIdentifier));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000) { Direction = System.Data.ParameterDirection.Output });

                if (Account_Description.Equals(System.String.Empty) == true)
                {
                    cmd.Parameters["@Account_Description"].IsNullable = true;
                    cmd.Parameters["@Account_Description"].Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters["@Account_Description"].IsNullable = false;
                    cmd.Parameters["@Account_Description"].Value = Account_Description;
                }

                cmd.Parameters["@Account_Guid"].Value = Account_Guid;
                cmd.Parameters["@Bank_Guid"].Value = Bank_Guid;
                cmd.Parameters["@Currency_Guid"].Value = Currency_Guid;
                cmd.Parameters["@Account_Number"].Value = Account_Number;
                cmd.Parameters["@AccountType_Guid"].Value = AccountType_Guid;


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
                strErr += ("Не удалось изменить свойства объекта 'расчётный счёт'. Текст ошибки: " + f.Message);
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
        public static System.Boolean RemoveObjectFromDataBase(System.Guid Account_Guid,
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteAccount]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Account_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Account_Guid"].Value = Account_Guid;
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

                strErr += ("Не удалось удалить объект 'расчётный счёт'. Текст ошибки: " + f.Message);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        #endregion

        #region Список счетов
        public static List<CAccount> GetAccountList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidBankId, System.String strAccountNumber,
            ref System.String strErr)
        {
            List<CAccount> objList = new List<CAccount>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetAccount]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                if (strAccountNumber.Trim() != System.String.Empty)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Account_Number", System.Data.DbType.String));
                    cmd.Parameters["@Account_Number"].Value = strAccountNumber;
                }
                if (uuidBankId.Equals( System.Guid.Empty ) == false)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Bank_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@Bank_Guid"].Value = uuidBankId;
                }
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.String strAccDscrpn = "";
                    System.Guid ParentId = System.Guid.Empty;
                    CBank objBank = null;
                    CCurrency objCurrency = null;
                    CAccountType objAccountType = null;
                    while (rs.Read())
                    {
                        objCurrency = new CCurrency() { ID = (System.Guid)rs["Currency_Giud"], CurrencyAbbr = (System.String)rs["Currency_Abbr"], CurrencyCode = (System.String)rs["Currency_Code"] };

                        objAccountType = new CAccountType() { ID = (System.Guid)rs["AccountType_Guid"], Name = (System.String)rs["AccountType_Name"] };

                        ParentId = (rs["Bank_ParentGuid"] == System.DBNull.Value) ? System.Guid.Empty : (System.Guid)rs["Bank_ParentGuid"];

                        objBank = new CBank((System.Guid)rs["Bank_Guid"], (System.String)rs["Bank_Name"], ParentId,
                            (System.String)rs["Bank_Code"], "", (System.String)rs["Bank_UNN"], (System.String)rs["Bank_MFO"],
                            "", (System.Boolean)rs["Bank_IsActive"]);

                        strAccDscrpn = (rs["Account_Ddescription"] == System.DBNull.Value) ? "" : (System.String)rs["Account_Ddescription"];

                        objList.Add(new CAccount((System.Guid)rs["Account_Guid"], objBank, objCurrency,
                            (System.String)rs["Account_Number"], strAccDscrpn, objAccountType, (System.Boolean)rs["CompanyAccount_IsMain"]));
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
                strErr = "Не удалось получить список расчетных счетов.\n\nТекст ошибки: " + f.Message;
            }
            return objList;
        }
        #endregion

    }

}
