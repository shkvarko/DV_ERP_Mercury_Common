using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
  
namespace ERP_Mercury.Common
{
    /// <summary>
    /// Класс "Тип места хранение"
    /// </summary>
    public class CWareHouseType : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Признак "Активен"
        /// </summary>
        private System.Boolean m_bIsAcitve;
        /// <summary>
        /// Признак "Активен"
        /// </summary>
        [DisplayName("Тип хранилища, активен")]
        [Description("Признак, что тип хранилища активен")]
        [Category("2. Дополнительно")]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public System.Boolean IsAcitve
        {
            get { return m_bIsAcitve; }
            set { m_bIsAcitve = value; }
        }
        #endregion

        #region Конструктор
         public CWareHouseType() : base()
        {
            m_bIsAcitve = false;
        }
         public CWareHouseType(System.Guid uuidId, System.String strName, System.Boolean bIsAcitve)
            : base( uuidId, strName )
        {
            m_bIsAcitve = bIsAcitve;
            //Name = strName; // ???
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список объектов "Тип места хранение"
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
         /// <returns>список объектов "Тип места хранение"</returns>
        public static List<CWareHouseType> GetWareHouseTypeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
         List<CWareHouseType> objList = new List<CWareHouseType>();
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

             cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetWarehouseType]", objProfile.GetOptionsDllDBName());
             cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
             cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
             cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
             cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
             System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
             if (rs.HasRows)
             {
                 while (rs.Read())
                 {
                     objList.Add(new CWareHouseType((System.Guid)rs["WareHouseType_Guid"], (System.String)rs["WareHouseType_Name"], System.Convert.ToBoolean(rs["WarehouseType_IsActive"])));
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
             "Не удалось получить список объектов \"Тип места хранение\".\n\nТекст ошибки: " + f.Message, "Внимание",
             System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
         }
         return objList;
        }
        #endregion
        
        #region IsAllParametersValid
        /// <summary>
        /// Проверка свойств типа хранилища перед сохранением
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
                    strErr = "Тип хранилище: Необходимо указать название типа хранилища!";
                    return bRet;
                }

             
                // У хранилища, может не быть указан адрес
                /*
                if ((this.AddressList == null) || (this.AddressList.Count == 0))
                {
                    strErr = "Хранилище: Необходимо указать адрес!";
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
                */

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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddWarehouseType]", objProfile.GetOptionsDllDBName()); //sp_EditRegion

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WarehouseType_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));            
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WarehouseType_IsActive", System.Data.DbType.Boolean));  
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WarehouseType_Name", System.Data.DbType.String));       
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000)); 
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@WarehouseType_IsActive"].Value = this.IsAcitve;
                cmd.Parameters["@WarehouseType_Name"].Value = this.Name;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@WarehouseType_Guid"].Value;
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    DBTransaction.Rollback();
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка создания типа хранилища.\n\nТекст ошибки: " + strErr, "Ошибка",
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
            //if (IsAllParametersValid() == false) { return bRet; }

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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditWareHouseType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WareHouseType_Guid", System.Data.DbType.Guid));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WareHouseType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WarehouseType_IsActive", System.Data.DbType.Boolean)); 
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@WareHouseType_Guid"].Value = this.ID;
                cmd.Parameters["@WareHouseType_Name"].Value = this.Name;
                cmd.Parameters["@WarehouseType_IsActive"].Value = this.IsAcitve;

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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств типа хранилища.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteWareHouseType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WareHouseType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@WareHouseType_Guid"].Value = this.ID;
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

        public override string ToString()
        {
            return Name;
        }
    }

    #region Класс "Хранилище"
    /// <summary>
    /// TypeConverter для списка типов хранилищ
    /// </summary>
    class WarehouseTypeConverter : TypeConverter
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

            //CStock objStockType = (CStock)context.Instance;
            CWarehouse objWarehouseType = (CWarehouse)context.Instance;
            
            System.Collections.Generic.List<CWareHouseType> objList = objWarehouseType.GetAllWareHouseTypeList(); // добавить функцию, когда её напишу


            return new StandardValuesCollection(objList);
        }
    }

    /// <summary>
    /// Класс "Место хранения"
    /// </summary>
    public class CWarehouse : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Тип хранилища
        /// </summary>
        private CWareHouseType m_objWarehouseType;
        /// <summary>
        /// Тип хранилища
        /// </summary>
        [DisplayName("Тип хранилища")]
        [Description("Тип хранилища")]
        [Category("1. Обязательные значения")]
        [TypeConverter(typeof(WarehouseTypeConverter))]
        [ReadOnly(true)]
        [BrowsableAttribute(false)]
        public CWareHouseType WarehouseType
        {
            get { return m_objWarehouseType; }
            set { m_objWarehouseType = value; }
        }
        /// <summary>
        /// Тип хранилища
        /// </summary>
        [DisplayName("Тип хранилища")]
        [Description("Тип хранилища")]
        [Category("1. Обязательные значения")]
        [TypeConverter(typeof(WarehouseTypeConverter))]
        public System.String WarehouseTypeName
        {
            get { return m_objWarehouseType.Name; } 
            set { SetWareHouseTypeValue(value); }
        }
        
        private void SetWareHouseTypeValue(System.String strWareHouseTypeName)
        {
            try
            {
                if (m_objWareHouseTypeList == null) { m_objWarehouseType = null; }
                else
                {
                    foreach (CWareHouseType objWareHouseType in m_objWareHouseTypeList)
                    {
                        if (objWareHouseType.Name == strWareHouseTypeName)
                        {
                            m_objWarehouseType = objWareHouseType;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось установить значение типа хранилища.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }

        private List<CWareHouseType> m_objWareHouseTypeList;


        /// <summary>
        /// Признак "Активен"
        /// </summary>
        private System.Boolean m_bIsAcitve;
        /// <summary>
        /// Признак "Активен"
        /// </summary>
        [DisplayName("Хранилище активно")]
        [Description("Признак, что хранилище активно")]
        [Category("2. Дополнительно")]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public System.Boolean IsAcitve
        {
            get { return m_bIsAcitve; }
            set { m_bIsAcitve = value; }
        }
        /// <summary>
        /// Код хранилища в IB
        /// </summary>
        private System.Int32 m_IBId;
        /// <summary>
        /// Код хранилища в IB
        /// </summary>
        [DisplayName("Код хранилища в IB")]
        [Description("Код хранилища в IB")]
        [Category("3. Справочные данные")]
        [ReadOnly(true)]
        public System.Int32 IBId
        {
            get { return m_IBId; }
            set { m_IBId = value; }
        }
        /// <summary>
        /// Признак "Склад для отгрузок"
        /// </summary>
        private System.Boolean m_IsForShipping;
        /// <summary>
        /// Признак "Склад для отгрузок"
        /// </summary>
        [DisplayName("Хранилище для отгрузок")]
        [Description("Хранилище для отгрузок")]
        [Category("2. Дополнительно")]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public System.Boolean IsForShipping
        {
            get { return m_IsForShipping; }
            set { m_IsForShipping = value; }
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

        private List<CAddress> m_objAddressForDeleteList;
        public List<CAddress> AddressForDeleteList
        {
            get { return m_objAddressForDeleteList; }
            set { m_objAddressForDeleteList = value; }
        }

        private const System.Int32 iCommandTimeOutForIB = 120;

        #endregion

        #region Конструктор
         public CWarehouse() : base()
        {
            m_objWarehouseType = null;
            m_bIsAcitve = false;
            m_IBId = 0;
            m_IsForShipping = false;
            m_objWareHouseTypeList = null;
        }
         public CWarehouse(System.Guid uuidId, System.String strName, CWareHouseType objWareHouseType,
             System.Boolean bIsAcitve, System.Int32 iIBId, System.Boolean bIsForShipping)
            : base( uuidId, strName )
        {
            m_objWarehouseType = objWareHouseType;
            m_bIsAcitve = bIsAcitve;
            m_IBId = iIBId;
            m_IsForShipping = bIsForShipping;
            this.m_objWareHouseTypeList = null;
        }
        #endregion

        #region Список объектов

         public List<CWareHouseType> GetAllWareHouseTypeList()
         {
             return m_objWareHouseTypeList;
         }
        
        /// <summary>
        /// Возвращает список объектов "Место хранения"
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список объектов "Место хранения"</returns>
         public static List<CWarehouse> GetWareHouseList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
         {
             List<CWarehouse> objList = new List<CWarehouse>();
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

                 cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetWarehouse]", objProfile.GetOptionsDllDBName());
                 cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                 cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                 cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                 cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                 System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                 if (rs.HasRows)
                 {
                     while (rs.Read())
                     {
                         objList.Add(new CWarehouse((System.Guid)rs["Warehouse_Guid"], (System.String)rs["Warehouse_Name"],
                             new CWareHouseType((System.Guid)rs["WareHouseType_Guid"], (System.String)rs["WareHouseType_Name"], System.Convert.ToBoolean(rs["WarehouseType_IsActive"])),
                             System.Convert.ToBoolean(rs["Warehouse_IsActive"]), (System.Int32)rs["Warehouse_Id"], System.Convert.ToBoolean(rs["Warehouse_IsForShipping"]))
                             );
                     }
                 }
                 rs.Close();
                 rs.Dispose();
                 //------------------
                 List<CWareHouseType> objWareHouseTypeList = CWareHouseType.GetWareHouseTypeList(objProfile, cmd);
                 //------------------
                 if (cmdSQL == null)
                 {
                     cmd.Dispose();
                     DBConnection.Close();
                 }

                 // а это для выпадающих списков
                 if (objWareHouseTypeList != null) 
                 {
                     foreach (CWarehouse objWarehouse in objList)
                     {
                         objWarehouse.m_objWareHouseTypeList = objWareHouseTypeList;
                     }
                 }
             }
             catch (System.Exception f)
             {
                 DevExpress.XtraEditors.XtraMessageBox.Show(
                 "Не удалось получить список объектов \"Место хранения\".\n\nТекст ошибки: " + f.Message, "Внимание",
                 System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
             }
             return objList;
         }
        /// <summary>
         /// Возвращает список объектов "Место хранения"
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
         /// <returns>список объектов "Место хранения"</returns>
         public static List<CWarehouse> GetWareHouseListForShedule(UniXP.Common.CProfile objProfile, 
             System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidShedule)
        {
            List<CWarehouse> objList = new List<CWarehouse>();
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

             cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetWarehouse]", objProfile.GetOptionsDllDBName());
             cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
             cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Shedule_Guid", System.Data.SqlDbType.UniqueIdentifier));
             cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
             cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
             cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
             cmd.Parameters["@Shedule_Guid"].Value = uuidShedule;
             System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
             if (rs.HasRows)
             {
                 while (rs.Read())
                 {
                     objList.Add( new CWarehouse( (System.Guid)rs["Warehouse_Guid"], (System.String)rs["Warehouse_Name"],
                         new CWareHouseType((System.Guid)rs["WareHouseType_Guid"], (System.String)rs["WareHouseType_Name"], System.Convert.ToBoolean(rs["WarehouseType_IsActive"])),
                         System.Convert.ToBoolean(rs["Warehouse_IsActive"]), (System.Int32)rs["Warehouse_Id"], System.Convert.ToBoolean(rs["Warehouse_IsForShipping"]))
                         );
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
             "Не удалось получить список объектов \"Место хранения\".\n\nТекст ошибки: " + f.Message, "Внимание",
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
                     strErr = "Хранилище: Необходимо указать название хранилища!";
                     return bRet;
                 }
               
                 if (this.WarehouseType == null)
                 {
                     strErr = "Хранилище: Необходимо указать форму собственности хранилища!";
                     return bRet;
                 }
                 

                 // У хранилища, может не быть указан адрес
                 /*
                 if ((this.AddressList == null) || (this.AddressList.Count == 0))
                 {
                     strErr = "Хранилище: Необходимо указать адрес!";
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
                 */

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
        public System.Boolean Add (UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddWarehouse]", objProfile.GetOptionsDllDBName()); ;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Warehouse_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WarehouseType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Warehouse_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Warehouse_IsActive", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Warehouse_IsForShipping", System.Data.DbType.Boolean));
                
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@WarehouseType_Guid"].Value = this.WarehouseType.ID;
                cmd.Parameters["@Warehouse_Name"].Value = this.Name;
                cmd.Parameters["@Warehouse_IsActive"].Value = this.IsAcitve;
                cmd.Parameters["@Warehouse_IsForShipping"].Value = this.IsForShipping;
                
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    this.ID = (System.Guid)cmd.Parameters["@Warehouse_Guid"].Value;
                    // теперь списки адресов
                    // возможна ситуация, когда при сохранении нового клиента произошла ошибка,
                    // при этом адреса получили идентификаторы
                    // их нужно сбросить в Empty

                    if (iRes == 0)
                    {
                        if ((this.AddressList != null) && (this.AddressList.Count > 0) /*&& (this.AddressForDeleteList != null)*/ )
                        {
                            foreach (CAddress objAddress in this.AddressList) { objAddress.ID = System.Guid.Empty; }
                            iRes = (CAddress.SaveAddressList(this.AddressList, null, EnumObject.Warehouse, this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    
                    
                    // теперь все это нужно записать в InterBase
                    
                    if (iRes == 0)
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddWarehouseToIB]", objProfile.GetOptionsDllDBName()); ;
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Warehouse_Id", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Warehouse_Guid", System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                        cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                        cmd.Parameters["@Warehouse_Guid"].Value = this.ID;
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
                            if (bSaveInIB == true)
                            {
                                DeleteWarehouseFromIB(objProfile, cmd, ref strErr);
                            }

                            this.ID = System.Guid.Empty;

                            if ((this.AddressList != null) && (this.AddressList.Count > 0))
                            {
                                foreach (CAddress objAddress in this.AddressList)
                                {
                                    if (objAddress.IsNewObject == true) { objAddress.ID = System.Guid.Empty; }
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
        /// Сохраняет в базе данных изменения в описании хранилища
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditWarehouse]", objProfile.GetOptionsDllDBName()); ;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Warehouse_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Warehouse_Id", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Warehouse_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WarehouseType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Warehouse_IsActive", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Warehouse_IsForShipping", System.Data.DbType.Boolean));              
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                
                cmd.Parameters["@Warehouse_Guid"].Value = this.ID;
                cmd.Parameters["@Warehouse_Id"].Value = this.IBId;
                cmd.Parameters["@Warehouse_Name"].Value = this.Name;
                cmd.Parameters["@WarehouseType_Guid"].Value = this.WarehouseType.ID;

                cmd.Parameters["@Warehouse_IsActive"].Value = this.IsAcitve;
                cmd.Parameters["@Warehouse_IsForShipping"].Value = this.IsForShipping;
               
                cmd.ExecuteNonQuery();

                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    // теперь списки контактов, адресов, лицензий и телефонов
                    if (iRes == 0)
                    {
                        if ((this.AddressList != null) || (this.AddressForDeleteList != null))
                        {
                            iRes = (CAddress.SaveAddressList(this.AddressList, this.AddressForDeleteList, EnumObject.Warehouse , this.ID, objProfile, cmd, ref strErr) == true) ? 0 : -1;
                        }
                    }
                    
                    // остановился здесь

                    // теперь все это нужно записать в InterBase
                    // нужно, но временно отключено
                    if (iRes == 0)
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditWarehouseToIB]", objProfile.GetOptionsDllDBName()); ;
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Warehouse_Guid", System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                        cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                        cmd.Parameters["@Warehouse_Guid"].Value = this.ID;
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
                System.Int32 iRes = DeleteWarehouseFromIB (objProfile, cmd, ref strErr);
                
                if (iRes == 0) // если всё OK
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteWarehouse]", objProfile.GetOptionsDllDBName());
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Warehouse_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters["@Warehouse_Guid"].Value = this.ID;
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

                        DevExpress.XtraEditors.XtraMessageBox.Show("Невозможно удалить хранилище. Техническая информация:\n\nТекст ошибки: " + (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
        /// Удаляем хранилище из InterBase
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>0 - удачное завершение операции; <>0 - ошибка</returns>
        private System.Int32 DeleteWarehouseFromIB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
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
                    cmdSQL.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteWarehouseFromIB]", objProfile.GetOptionsDllDBName()); ;
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Warehouse_IbID", System.Data.SqlDbType.Int));
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmdSQL.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                    cmdSQL.Parameters["@Warehouse_IbID"].Value = this.IBId;
                    
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
    #endregion


    #region Класс "Склад"

    /// <summary>
    /// TypeConverter для списка компаний
    /// </summary>
    class CompanyConverter : TypeConverter
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

            CStock objStockType = (CStock)context.Instance;
            System.Collections.Generic.List<CCompany> objList = objStockType.GetAllCompanyList();

            return new StandardValuesCollection(objList);
        }
    }
    
    /*
    /// <summary>
    /// TypeConverter для списка типов хранилищ
    /// </summary>
    class WarehouseTypeConverter : TypeConverter
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

            CStock objStockType = (CStock)context.Instance;
            System.Collections.Generic.List<CWareHouseType> objList = objStockType.GetAllWareHouseTypeList(); // добавить функцию, когда её напишу
            
            return new StandardValuesCollection(objList);
        }
    }
    */

    /// <summary>
    /// TypeConverter для списка хранилищ
    /// </summary>
    class WarehouseConverter : TypeConverter
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

            CStock objStockType = (CStock)context.Instance;
            System.Collections.Generic.List<CWarehouse> objList = objStockType.GetAllWareHouseList(); // добавить функцию, когда её напишу

            return new StandardValuesCollection(objList);
        }
    }


    /// <summary>
    /// Класс "Склад"
    /// </summary>
    [TypeConverter(typeof(PropertySorter))]
    public class CStock : CBusinessObject
    {
        #region Свойства     
        //---------------------
        //private System.Guid m_uuidID;
        ///// <summary>
        ///// Уникальный идентификатор
        ///// </summary>
        //[DisplayName("Уникальный идентификатор")]
        //[Description("Уникальный идентификатор объекта")]
        //[ReadOnly(true)]
        //[Category("1. Обязательные значения")]
        //[PropertyOrder(40)]
        //public System.Guid ID
        //{
        //    get { return m_uuidID; }
        //    set { m_uuidID = value; }
        //}



        /// <summary>
        /// Имя
        /// </summary>
        private System.String m_strName;
        /// <summary>
        /// Имя
        /// </summary>
        [DisplayName("Наименование склада")]
        [Description("Наименование объекта")]
        [Category("1. Обязательные значения")]
        [PropertyOrder(30)]
        public System.String Name
        {
            //get { return m_strName; }
            //get { return ((this.WareHouse == null) ? "" : Convert.ToString(this.WareHouse.Name)); }
            get { return ((m_strName == "") ? ( this.WareHouse == null ? "" : this.WareHouse.Name ) : m_strName); } // если пользователь выбрал хранилище -- подставить его в поле наименование, если хочет ввести что-то своё -- использовать это значение
            set { m_strName = value; }
        }
        
        //---------------------
        /// <summary>
        /// Код склада в IB
        /// </summary>
        private System.Int32 m_IBId;
        /// <summary>
        /// Код склада в IB
        /// </summary>
        [DisplayName("Номер склада в IB")]
        [Description("Номер склада в IB")]
        [Category("3. Справочные данные")]
        [ReadOnly(true)]
        public System.Int32 IBId
        {
            get { return m_IBId; }
            set { m_IBId = value; }
        }
        /// <summary>
        /// Признак "Активен"
        /// </summary>
        private System.Boolean m_bIsAcitve;
        /// <summary>
        /// Признак "Активен"
        /// </summary>
        [DisplayName("Склад активен")]
        [Description("Признак, что склад активен")]
        [Category("2. Дополнительно")]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public System.Boolean IsAcitve
        {
            get { return m_bIsAcitve; }
            set { m_bIsAcitve = value; }
        }

        /// <summary>
        /// Признак "Склад для отгрузки"
        /// </summary>
        private System.Boolean m_bIsTrade;
        /// <summary>
        /// Признак "Склад для отгрузки"
        /// </summary>
        [DisplayName("Со склада ведётся отгрузка")]
        [Description("Признак, что со склада ведётся отгрузка")]
        [Category("2. Дополнительно")]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public System.Boolean IsTrade
        {
            get { return m_bIsTrade; }
            set { m_bIsTrade = value; }
        }
        
        /// <summary>
        /// Признак "Склад доступен для внутреннего перемещения"
        /// </summary>
        private System.Boolean m_bInTransfer;
        /// <summary>
        /// Признак "Склад доступен для внутреннего перемещения"
        /// </summary>
        [DisplayName("Склад доступен для внутреннего перемещения")]
        [Description("Признак, что склад может использоваться в операциях внутреннего перемещения")]
        [Category("2. Дополнительно")]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public System.Boolean InTransfer
        {
            get { return m_bInTransfer; }
            set { m_bInTransfer = value; }
        }
        
        /// <summary>
        /// Место хранения
        /// </summary>
        private CWarehouse m_objWareHouse;
        /// <summary>
        /// Место хранения
        /// </summary>
        [DisplayName("Хранилище")]
        [Description("Место хранения товара")]
        [Category("1. Обязательные значения")]
        [PropertyOrder(20)]
        [TypeConverter(typeof(WarehouseConverter))]
        [ReadOnly(false)]
        [BrowsableAttribute(false)]
        public CWarehouse WareHouse
        {
            get { return m_objWareHouse; }
            set { m_objWareHouse = value; }
        }
        /// <summary>
        /// Место хранения
        /// </summary>
        [DisplayName("Хранилище")]
        [Description("Место хранения товара")]
        [Category("1. Обязательные значения")]
        [PropertyOrder(20)]
        [TypeConverter(typeof(WarehouseConverter))]
        public System.String WareHouseName
        {
            get { return m_objWareHouse.Name; }
            set { SetWareHouseValue(value); }
        }

        private void SetWareHouseValue(System.String strWareHouse)
        {
            try
            {
                if (m_objAllWareHouseList == null) { m_objWareHouse = null; }
                else
                {
                    foreach (CWarehouse objWarehouse in m_objAllWareHouseList)
                    {
                        if (objWarehouse.Name == strWareHouse)
                        {
                            m_objWareHouse = objWarehouse;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось установить значение склада.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }

        private List<CWarehouse> m_objAllWareHouseList;

        
        // ----------------------- begin -----------------------
        /// <summary>
        /// Тип места хранения
        /// </summary>
        private CWareHouseType m_objWareHouseType;
        /// <summary>
        /// Тип места хранения
        /// </summary>
        [DisplayName("Тип места хранения")]
        [Description("Тип места хранения")]
        //[Category("1. Обязательные значения")]
        [Category("3. Справочные данные")]
        [TypeConverter(typeof(WarehouseTypeConverter))]
        [ReadOnly(true)]
        [BrowsableAttribute(false)]
        public CWareHouseType WareHouseType
        {
            //get { return m_objWareHouseType; }
            //set { m_objWareHouseType = value; }
            get { return m_objWareHouse.WarehouseType; } // т.к. WarehouseType вложен в Warehouse при возврате из метода GetStockListForWarehouse
            set { m_objWareHouse.WarehouseType = value; }
        }
        /// <summary>
        /// Тип места хранения
        /// </summary>
        [DisplayName("Тип места хранения")]
        [Description("Тип места хранения")]
        //[Category("1. Обязательные значения")]
        [Category("3. Справочные данные")]
        [TypeConverter(typeof(WarehouseTypeConverter))]
        [ReadOnly(true)]
        public System.String WarehouseTypeName
        {
            get { return m_objWareHouse.WarehouseType.Name; } 
            set { SetWareHouseTypeValue(value); }
        }
        
        private void SetWareHouseTypeValue(System.String strWareHouseTypeName)
        {
            try
            {
                if (m_objWareHouseTypeList == null) { m_objWareHouseType = null; }
                else
                {
                    foreach (CWareHouseType objWareHouseType in m_objWareHouseTypeList)
                    {
                        if (objWareHouseType.Name == strWareHouseTypeName)
                        {
                            m_objWareHouseType = objWareHouseType;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось установить значение типа хранилища.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }
    
        private List<CWareHouseType> m_objWareHouseTypeList; // может перенести ???

        
        //----- I (begin) -----
        /// <summary>
        /// Компания
        /// </summary>
        private CCompany m_objCompany;
        /// <summary>
        /// Компания
        /// </summary>
        [DisplayName("Компания")]
        [Description("Компания")]
        [Category("1. Обязательные значения")]
        [PropertyOrder(10)]
        [TypeConverter(typeof(CompanyConverter))]
        [ReadOnly(false)]
        [BrowsableAttribute(false)]
        public CCompany Company
        {
            get { return m_objCompany; } // здесь должен быть Name (название компании)
            set { m_objCompany = value; }
        }
        //----- I (end) -----


        //----- II (begin) -----
        [DisplayName("Компания")]
        [Description("Компания")]
        [Category("1. Обязательные значения")]
        [PropertyOrder(10)]
        [TypeConverter(typeof(CompanyConverter))]
        public System.String CompanyName
        {
            //get { return ((this.Company == null) ? "" : this.Company.Name); }
            get { return ((this.Company == null) ? "" : m_objCompany.Name); }
            set { SetCompanyValue(value); }
        }

        private void SetCompanyValue(System.String strCompanyName)
        {
            try
            {
                if (m_objAllCompanyList == null) { m_objCompany = null; }
                else
                {
                    foreach (CCompany objCompany in m_objAllCompanyList)
                    {
                        if (objCompany.Name == strCompanyName)
                        {
                            m_objCompany = objCompany;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось установить значение компаний.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }

        private List<CCompany>  m_objAllCompanyList;


        [DisplayName("Акроним компании")]
        [Description("Сокращённое название компании")]
        [Category("3. Справочные данные")]
        public System.String CompanyAbbr
        {
            get { return ((this.Company == null) ? "" : this.Company.Abbr); }    // так как  есть только get, свойство доступно только для чтения. Аналогично [ReadOnly(true)]
        }
        /*
        public System.String WareHouseTypeName
        {
            get { return ((this.WareHouse == null) ? "" : Convert.ToString(this.WareHouse.WarehouseType)); }
        }
        */
        
        #endregion

        #region Конструктор
        public CStock() : base()
        {
            m_objCompany = null;
            m_objWareHouse = null;
            m_bIsAcitve = false;
            m_bIsTrade = false;
            m_bInTransfer = false;
            m_IBId = 0;
            m_objWareHouseTypeList = null;
            m_objAllCompanyList = null;
            m_objAllWareHouseList = null;
            //m_strWareHouseType = "";
            m_strName = "";//**
            //m_strAbbr = "";
        }

        public CStock(System.Guid uuidId, System.String strName, System.Int32 iIBId, System.Boolean bIsAcitve,
           System.Boolean bIsTrade, CWarehouse objWarehouse, CCompany objCompany)
            : base(uuidId, strName)
        {
            m_IBId = iIBId;    //** проверить, как  работает этот конструктор с IT-dev
            ID = uuidId; //**
            m_IBId = iIBId;
            m_bIsAcitve = bIsAcitve;
            m_bIsTrade = bIsTrade;
            m_objWareHouse = objWarehouse;
            m_objCompany = objCompany;
            this.m_objWareHouseTypeList = null;
            this.m_objAllCompanyList = null;
            this.m_objAllWareHouseList = null;
        }

        public CStock( System.Guid uuidId, System.String strName, System.Int32 iIBId, System.Boolean bIsAcitve, 
            System.Boolean bIsTrade, System.Boolean bInTransfer, CWarehouse objWarehouse, CCompany objCompany )
            : base( uuidId, strName )
        {
            m_IBId = iIBId;
            ID = uuidId;  //**
            m_strName = strName;//**
            m_bIsAcitve = bIsAcitve;
            m_bIsTrade = bIsTrade;
            m_bInTransfer = bInTransfer;
            m_objWareHouse = objWarehouse;
            m_objCompany = objCompany;   // ??? не надо ли здесь скинуть в null ? Посмотреть в отладке
            this.m_objWareHouseTypeList = null;
            this.m_objAllCompanyList  = null;
            this.m_objAllWareHouseList = null;
        }

        #endregion

        #region Список объектов
      
        public List<CWareHouseType> GetAllWareHouseTypeList()
        {
            return m_objWareHouseTypeList;
        }

        public List<CCompany> GetAllCompanyList()
        {
            return m_objAllCompanyList;
        }

        public List<CWarehouse> GetAllWareHouseList()
        {
            return m_objAllWareHouseList;
        }
        
        /// <summary>
        /// Возвращает список объектов "Склад"
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список объектов "Склад"</returns>
        public static List<CStock> GetStockList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CStock> objList = new List<CStock>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetStock]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CStock((System.Guid)rs["Stock_Guid"], 
                                              (System.String)rs["Stock_Name"], 
                                              (System.Int32)rs["Stock_Id"], 
                                               System.Convert.ToBoolean(rs["Stock_IsActive"]), 
                                               System.Convert.ToBoolean(rs["Stock_IsTrade"]),
                                               //System.Convert.ToBoolean(rs["Stock_InTransfer"]),
                                    new CWarehouse((System.Guid)rs["Warehouse_Guid"], 
                                                   (System.String)rs["Warehouse_Name"],
                                        new CWareHouseType((System.Guid)rs["WareHouseType_Guid"], 
                                                      (System.String)rs["WareHouseType_Name"], 
                                                       System.Convert.ToBoolean(rs["WarehouseType_IsActive"]) ),
                                                       System.Convert.ToBoolean(rs["Warehouse_IsActive"]), 
                                                      (System.Int32)rs["Warehouse_Id"], 
                                                       System.Convert.ToBoolean(rs["Warehouse_IsForShipping"])),
                                    new CCompany((System.Guid)rs["Company_Guid"],
                                                 (System.String)rs["Company_Name"], 
                                                 (System.String)rs["Company_Acronym"])
                             )
                            );
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
                "Не удалось получить список объектов \"Склад\".\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }

        /// <summary>
        /// Возвращает список объектов "Склад" для использовани в складах и хранилищах
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список объектов "Склад"</returns>
        public static List<CStock> GetStockListForWarehouse(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CStock> objList = new List<CStock>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetStock]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CStock((System.Guid)rs["Stock_Guid"],
                                              (System.String)rs["Stock_Name"],
                                              (System.Int32)rs["Stock_Id"],
                                               System.Convert.ToBoolean(rs["Stock_IsActive"]),
                                               System.Convert.ToBoolean(rs["Stock_IsTrade"]),
                                               System.Convert.ToBoolean(rs["Stock_InTransfer"]),
                                    new CWarehouse((System.Guid)rs["Warehouse_Guid"],
                                                   (System.String)rs["Warehouse_Name"],
                                        new CWareHouseType((System.Guid)rs["WareHouseType_Guid"],
                                                      (System.String)rs["WareHouseType_Name"],
                                                       System.Convert.ToBoolean(rs["WarehouseType_IsActive"])),
                                                       System.Convert.ToBoolean(rs["Warehouse_IsActive"]),
                                                      (System.Int32)rs["Warehouse_Id"],
                                                       System.Convert.ToBoolean(rs["Warehouse_IsForShipping"])),
                                    new CCompany((System.Guid)rs["Company_Guid"],
                                                 (System.String)rs["Company_Name"],
                                                 (System.String)rs["Company_Acronym"])
                             )
                            );
                    }
                }
                rs.Close();
                rs.Dispose();
                // ------------------

                List<CWareHouseType> objWareHouseTypeList = CWareHouseType.GetWareHouseTypeList(objProfile, cmd); // заполнен 5 значений
                List<CCompany> objCompanyList = CCompany.GetCompanyList(objProfile, cmd);
                List<CWarehouse> objWareHouseList = CWarehouse.GetWareHouseList(objProfile, cmd);

                // ------------------)))
                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
                // а это для выпадающих списков
                if ((objWareHouseTypeList != null) && (objCompanyList != null))
                {
                    foreach (CStock objStock in objList)
                    {
                        objStock.m_objWareHouseTypeList = objWareHouseTypeList;
                        objStock.m_objAllCompanyList = objCompanyList;
                        objStock.m_objAllWareHouseList = objWareHouseList;
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список объектов \"Склад\".\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }

        
        /// <summary>
        /// Возвращает список объектов "Склад"
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список объектов "Склад"</returns>
        public static List<CStock> GetStockListFromERP(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            List<CStock> objList = new List<CStock>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetStockFromERP]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CStock(
                            (System.Guid)rs["Stock_Guid"], (System.String)rs["Stock_Name"], (System.Int32)rs["Stock_Id"], System.Convert.ToBoolean(rs["Stock_IsActive"]), System.Convert.ToBoolean(rs["Stock_IsTrade"]), /* System.Convert.ToBoolean(1), */ //System.Convert.ToBoolean(rs["Stock_InTransfer"]),
                            new CWarehouse((System.Guid)rs["Warehouse_Guid"], (System.String)rs["Warehouse_Name"],
                             new CWareHouseType((System.Guid)rs["WareHouseType_Guid"], (System.String)rs["WareHouseType_Name"], System.Convert.ToBoolean(rs["WarehouseType_IsActive"])),
                             System.Convert.ToBoolean(rs["Warehouse_IsActive"]), (System.Int32)rs["Warehouse_Id"], false),
                             new CCompany((System.Guid)rs["Company_Guid"], (System.String)rs["Company_Name"], (System.String)rs["Company_Acronym"])
                             )
                            );
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
                "Не удалось получить список объектов \"Склад\".\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }

        /// <summary> 
        /// Загружает в m_objAllRegionList список районов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        public void InitWareHouseTypeList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            try
            {
                // !!!!!!!!!!!! возможно нужно загружать в m_objWareHouse.WarehouseType или в m_objWareHouse.m_objWareHouseTypeList (а может и нет, проверить) ??? т.к. WareHouseType вложен WareHouse !!!!!!!!!!
                this.m_objWareHouseTypeList = CWareHouseType.GetWareHouseTypeList(objProfile, cmdSQL);
                if ((this.m_objWareHouseTypeList != null) && (this.m_objWareHouseTypeList.Count > 0))
                {
                    this.WareHouseType = this.m_objWareHouseTypeList[0];
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось загрузить список районов.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }


        /// <summary> 
        /// Загружает в m_objAllCompanyList список районов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        public void InitCompanyList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            try
            {
                this.m_objAllCompanyList = CCompany.GetCompanyList(objProfile, cmdSQL);
                if ((this.m_objAllCompanyList != null) && (this.m_objAllCompanyList.Count > 0))
                {
                    this.Company = this.m_objAllCompanyList[0];
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось загрузить список компаний.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }

        /// <summary> 
        /// Загружает в m_objAllWareHouseList список районов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        public void InitWareHouseList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            try
            {
                this.m_objAllWareHouseList = CWarehouse.GetWareHouseList(objProfile, cmdSQL);
                if ((this.m_objAllWareHouseList != null) && (this.m_objAllWareHouseList.Count > 0))
                {
                    this.WareHouse = this.m_objAllWareHouseList[0];
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось загрузить список компаний.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                    "Необходимо указать название склада!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                /*if (this.m_strCode == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать код!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }*/
                if (this.WareHouse == null /* || this.WareHouse.Name == "-"*/ )
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо выбрать место хранения!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.Company == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо выбрать компанию!", "Внимание",
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
                
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddStock]", objProfile.GetOptionsDllDBName()); //sp_EditRegion

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null)); //**
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Warehouse_Guid", System.Data.DbType.Guid));     //*
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WarehouseType_Guid", System.Data.DbType.Guid)); //*
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.DbType.Guid));       //*
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_IsActive", System.Data.DbType.Boolean));  //*
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_IsTrade", System.Data.DbType.Boolean));   //*
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_InTransfer", System.Data.DbType.Boolean));//*
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_Name", System.Data.DbType.String));       //*
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));//**
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000)); //**
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Stock_Guid"].Value = this.ID;
                cmd.Parameters["@Warehouse_Guid"].Value = this.WareHouse.ID; // m_objWareHouse.ID;
                cmd.Parameters["@WarehouseType_Guid"].Value = this.WareHouseType.ID; //m_objWareHouseType.ID;
                cmd.Parameters["@Company_Guid"].Value = this.Company.ID; //m_objCompany.ID;
                cmd.Parameters["@Stock_IsActive"].Value = this.IsAcitve;
                cmd.Parameters["@Stock_IsTrade"].Value = this.IsTrade;
                cmd.Parameters["@Stock_InTransfer"].Value = this.InTransfer;
                cmd.Parameters["@Stock_Name"].Value = this.Name;
                //cmd.Parameters["@Stock_Id"].Value = this.IBId;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    
                    this.ID = (System.Guid)cmd.Parameters["@Stock_Guid"].Value; // Полученный после выполнения usp_AddStock, GUID 
                    
                    cmd.Parameters.Clear();
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddStockToIB]", objProfile.GetOptionsDllDBName()); ;
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_Id", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                    cmd.Parameters["@Stock_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств склада.\n\nТекст ошибки: " + strErr, "Ошибка",
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
            // Здесь написать код сохранения в БД склада...
            // Переписать этот метод (взят образец из Регион-а)
            
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditStock]", objProfile.GetOptionsDllDBName()); //sp_EditRegion
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_Guid", System.Data.DbType.Guid)); // Region_GUID
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Warehouse_Guid", System.Data.DbType.Guid)); // Oblast_GUID
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@WarehouseType_Guid", System.Data.DbType.Guid)); // Oblast_GUID
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.DbType.Guid)); // Oblast_GUID
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_IsActive", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_IsTrade", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_InTransfer", System.Data.DbType.Boolean));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_Id", System.Data.DbType.String)); // возможно, здесь нужно исправить String на Int // Region_Cod
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@Stock_Guid"].Value = this.ID;
                cmd.Parameters["@Warehouse_Guid"].Value = this.WareHouse.ID; // m_objWareHouse.ID;
                cmd.Parameters["@WarehouseType_Guid"].Value = this.WareHouseType.ID; //m_objWareHouseType.ID;
                cmd.Parameters["@Company_Guid"].Value = this.Company.ID; //m_objCompany.ID;
                cmd.Parameters["@Stock_IsActive"].Value = this.IsAcitve;
                cmd.Parameters["@Stock_IsTrade"].Value = this.IsTrade;
                cmd.Parameters["@Stock_InTransfer"].Value = this.InTransfer;
                cmd.Parameters["@Stock_Name"].Value = this.Name;
                cmd.Parameters["@Stock_Id"].Value = this.IBId;
                
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditStockToIB]", objProfile.GetOptionsDllDBName()); ;
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                    cmd.Parameters["@Stock_Guid"].Value = this.ID;
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств склада.\n\nТекст ошибки: " + strErr, "Ошибка",
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteStock]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@Stock_Guid"].Value = this.ID;
               
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
               
                if (iRes != 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления склада.\n\nТекст ошибки: " +
                                                               (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка",
                                                               System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                else
                {
                    // теперь все это нужно записать в InterBase
                    if (iRes == 0)
                    {                     
                        cmd.Parameters.Clear();
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteStockFromIB]", objProfile.GetOptionsDllDBName()); ;
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        //cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_Guid", System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_IbID", System.Data.SqlDbType.Int));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                        cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                        //cmd.Parameters["@Stock_Guid"].Value = this.ID;
                        cmd.Parameters["@Stock_IbID"].Value = this.IBId;
                       
                        cmd.ExecuteNonQuery();
                        iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                        if (iRes != 0 &&  cmd.Parameters["@ERROR_MES"].Value != System.DBNull.Value )
                        {
                            DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления склада в InterBase.\n\nТекст ошибки: " + (System.String)cmd.Parameters["@ERROR_MES"].Value, "Ошибка", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        }
                        else
                        {
                            if (iRes != 0)
                            {
                               DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления склада в InterBase.\n\n Код ошибки: " + iRes.ToString(), "Ошибка", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления склада.\n\nТекст ошибки: " +
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

        /*
        /// <summary>
        /// Удаляем склад из InterBase
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>0 - удачное завершение операции; <>0 - ошибка</returns>
        private System.Int32 DeleteStockFromIB(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
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
                    cmdSQL.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteStockFromIB]", objProfile.GetOptionsDllDBName()); ;
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Stock_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                    cmdSQL.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                    cmdSQL.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                    cmdSQL.Parameters["@Stock_Guid"].Value = this.ID;
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
        }*/

       
        #endregion

        //2013.06.17 к имени хранилища добавлено сокращённое наименование компании
        //public override string ToString()
        //{
        //    return ((CBusinessObject)this).Name;
        //}
        public override string ToString()
        {
            return (String.Format("{0} {1}", ((CBusinessObject)this).Name, CompanyAbbr));
        }
    }

    // Класс для настраиваемой сортировки в propertyGrid
    public class PropertySorter : ExpandableObjectConverter
    {
        #region Методы

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Возвращает упорядоченный список свойств
        /// </summary>
        public override PropertyDescriptorCollection GetProperties(
          ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            PropertyDescriptorCollection pdc =
              TypeDescriptor.GetProperties(value, attributes);

            ArrayList orderedProperties = new ArrayList();

            foreach (PropertyDescriptor pd in pdc)
            {
                Attribute attribute = pd.Attributes[typeof(PropertyOrderAttribute)];

                if (attribute != null)
                {
                    // атрибут есть - используем номер п/п из него
                    PropertyOrderAttribute poa = (PropertyOrderAttribute)attribute;
                    orderedProperties.Add(new PropertyOrderPair(pd.Name, poa.Order));
                }
                else
                {
                    // атрибута нет – считаем, что 0
                    orderedProperties.Add(new PropertyOrderPair(pd.Name, 0));
                }
            }

            // сортируем по Order-у
            orderedProperties.Sort();

            // формируем список имен свойств
            ArrayList propertyNames = new ArrayList();

            foreach (PropertyOrderPair pop in orderedProperties)
                propertyNames.Add(pop.Name);

            // возвращаем
            return pdc.Sort((string[])propertyNames.ToArray(typeof(string)));
        }

        #endregion
    }

    #region PropertyOrder Attribute

    /// <summary>
    /// Атрибут для задания сортировки
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropertyOrderAttribute : Attribute
    {
        private int _order;
        public PropertyOrderAttribute(int order)
        {
            _order = order;
        }

        public int Order
        {
            get { return _order; }
        }
    }

    #endregion

    #region PropertyOrderPair

    /// <summary>
    /// Пара имя/номер п/п с сортировкой по номеру
    /// </summary>
    public class PropertyOrderPair : IComparable
    {
        private int _order;
        private string _name;

        public string Name
        {
            get { return _name; }
        }

        public PropertyOrderPair(string name, int order)
        {
            _order = order;
            _name = name;
        }

        /// <summary>
        /// Собственно метод сравнения
        /// </summary>
        public int CompareTo(object obj)
        {
            int otherOrder = ((PropertyOrderPair)obj)._order;

            if (otherOrder == _order)
            {
                // если Order одинаковый - сортируем по именам
                string otherName = ((PropertyOrderPair)obj)._name;
                return string.Compare(_name, otherName);
            }
            else if (otherOrder > _order)
                return -1;

            return 1;
        }
    }

    #endregion
    
    #endregion
}
