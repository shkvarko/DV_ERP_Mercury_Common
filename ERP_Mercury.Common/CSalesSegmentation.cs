using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ERP_Mercury.Common
{
    
    /// <summary>
    /// Класс "Канал продаж"
    /// </summary>
    public class CSegmentationChanel : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Код сегментации
        /// </summary>
        [DisplayName("Код канала")]
        [Description("Код канала продаж")]
        [Category("1. Обязательные значения")]
        public System.String Code { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        [DisplayName("Примечание")]
        [Description("Примечание")]
        [Category("2. Необязательные значения")]
        public System.String Description{ get; set; }
        #endregion

        #region Конструктор
        public CSegmentationChanel()
        {
            ID = System.Guid.Empty;
            Name = "";
            Code = "";
            Description = "";
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
                if (this.Code == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать название!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
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
            System.String strErr = "";

            System.Guid SegmentationChannel_Guid = System.Guid.Empty;

            System.Boolean bRet = CSegmentationChanelDataBaseModel.AddNewObjectTodataBase( this.Code, this.Name, this.Description, 
                ref SegmentationChannel_Guid, objProfile, ref strErr );
            if (bRet == true)
            {
                this.ID = SegmentationChannel_Guid;
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
            System.String strErr = "";

            System.Guid SegmentationChannel_Guid = System.Guid.Empty;

            System.Boolean bRet = CSegmentationChanelDataBaseModel.RemoveObjectFromDataBase( this.ID, objProfile, ref strErr );
            if (bRet == false)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
            System.String strErr = "";

            System.Guid SegmentationChannel_Guid = System.Guid.Empty;

            System.Boolean bRet = CSegmentationChanelDataBaseModel.EditObjectInDataBase( this.ID, this.Code, this.Name, this.Description,
                objProfile, ref strErr);
            if (bRet == false)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return bRet;
        }

        #endregion

        public override string ToString()
        {
            return Code;
        }

    }

    /// <summary>
    /// Класс "Рынок сбыта"
    /// </summary>
    public class CSegmentationMarket : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// Код сегментации
        /// </summary>
        [DisplayName("Код рынка сбыта")]
        [Description("Код рынка сбыта")]
        [Category("1. Обязательные значения")]
        public System.String Code { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        [DisplayName("Примечание")]
        [Description("Примечание")]
        [Category("2. Необязательные значения")]
        public System.String Description{ get; set; }
        #endregion

        #region Конструктор
        public CSegmentationMarket()
        {
            ID = System.Guid.Empty;
            Name = "";
            Code = "";
            Description = "";
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
                if (this.Code == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать название!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
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
            System.String strErr = "";

            System.Guid SegmentationChannel_Guid = System.Guid.Empty;

            System.Boolean bRet = CSegmentationMarketDataBaseModel.AddNewObjectToDataBase( this.Code, this.Name, this.Description,
                ref SegmentationChannel_Guid, objProfile, ref strErr );
            if (bRet == true)
            {
                this.ID = SegmentationChannel_Guid;
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
            System.String strErr = "";

            System.Guid SegmentationChannel_Guid = System.Guid.Empty;

            System.Boolean bRet = CSegmentationMarketDataBaseModel.RemoveObjectFromDataBase( this.ID, objProfile, ref strErr );
            if (bRet == false)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
            System.String strErr = "";

            System.Guid SegmentationChannel_Guid = System.Guid.Empty;

            System.Boolean bRet = CSegmentationMarketDataBaseModel.EditObjectInDataBase(this.ID, this.Code, this.Name, this.Description,
                objProfile, ref strErr);
            if (bRet == false)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return bRet;
        }

        #endregion

        public override string ToString()
        {
            return Code;
        }

    }
    /// <summary>
    /// Класс "Сегмент"
    /// </summary>
    public class CSegmentationSubChannel : CBusinessObject
    {
        #region Свойства
        /// <summary>
        /// TypeConverter для списка регионов
        /// </summary>
        class SegmentationChannelTypeConverter : TypeConverter
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

                CSegmentationSubChannel objSegmentationSubChannel = (CSegmentationSubChannel)context.Instance;
                System.Collections.Generic.List<CSegmentationChanel> objList = objSegmentationSubChannel.GetAllChannelList();

                return new StandardValuesCollection(objList);
            }
        }
        /// <summary>
        /// Код
        /// </summary>
        [DisplayName("Код субканала")]
        [Description("Код субканала продаж")]
        [Category("1. Обязательные значения")]
        public System.String Code { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        [DisplayName("Примечание")]
        [Description("Примечание")]
        [Category("2. Необязательные значения")]
        public System.String Description { get; set; }

        /// <summary>
        /// Канал сбыта
        /// </summary>
        [DisplayName("Канал сбыта")]
        [Description("Канал сбыта")]
        [Category("1. Обязательные значения")]
        [TypeConverter(typeof(SegmentationChannelTypeConverter))]
        [ReadOnly(false)]
        [BrowsableAttribute(false)]
        public CSegmentationChanel SegmentationChanel{ get; set; }
        /// <summary>
        /// Канал сбыта
        /// </summary>
        [DisplayName("Канал сбыта")]
        [Description("Канал сбыта")]
        [Category("1. Обязательные значения")]
        [TypeConverter(typeof(SegmentationChannelTypeConverter))]
        public System.String ChannelCode
        {
            get { return SegmentationChanel.Code; }
            set { SetChanelValue(value); }
        }
        private void SetChanelValue(System.String strChanelCode)
        {
            try
            {
                if (m_objAllSegmentationChanelList == null) { SegmentationChanel = null; }
                else
                {
                    SegmentationChanel = m_objAllSegmentationChanelList.SingleOrDefault<CSegmentationChanel>(x => x.Code.Equals(strChanelCode) == true);
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось установить значение канала сбыта.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }
        private List<CSegmentationChanel> m_objAllSegmentationChanelList;
        public List<CSegmentationChanel> GetAllChannelList()
        {
            return m_objAllSegmentationChanelList;
        }

        public void SetSegmentationChanelList(List<CSegmentationChanel> objList)
        {
            this.m_objAllSegmentationChanelList = objList;
        }

        #endregion

        #region Конструктор
        public CSegmentationSubChannel()
        {
            ID = System.Guid.Empty;
            Name = "";
            Code = "";
            Description = "";
            SegmentationChanel = null;
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
            System.String strErr = "";

            System.Guid SegmentationSubChannel_Guid = System.Guid.Empty;
            System.Guid SegmentationChannel_Guid = ( ( this.SegmentationChanel == null ) ? System.Guid.Empty :this.SegmentationChanel.ID ) ;

            System.Boolean bRet = CSegmentationSubChanelDataBaseModel.AddNewObjectToDataBase(this.Code, this.Name, this.Description, SegmentationChannel_Guid,
                ref SegmentationSubChannel_Guid, objProfile, ref strErr);
            if (bRet == true)
            {
                this.ID = SegmentationChannel_Guid;
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
            System.String strErr = "";

            System.Boolean bRet = CSegmentationSubChanelDataBaseModel.RemoveObjectFromDataBase(this.ID, objProfile, ref strErr);
            if (bRet == false)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
            System.String strErr = "";

            System.Guid SegmentationChannel_Guid = ((this.SegmentationChanel == null) ? System.Guid.Empty : this.SegmentationChanel.ID);
            System.Boolean bRet = CSegmentationSubChanelDataBaseModel.EditObjectInDataBase(this.ID, this.Code, this.Name, this.Description, SegmentationChannel_Guid,
                objProfile, ref strErr);
            if (bRet == false)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return bRet;
        }

        #endregion

        #region Загрузка списка каналов
        public void InitSegmentationChannelList(UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            try
            {
                this.m_objAllSegmentationChanelList = CSegmentationChanelDataBaseModel.GetSegmentationChanelList( objProfile, null, ref strErr );
                if ((this.m_objAllSegmentationChanelList != null) && (this.m_objAllSegmentationChanelList.Count > 0))
                {
                    this.SegmentationChanel = this.m_objAllSegmentationChanelList[0];
                }
            }
            catch (System.Exception f)
            {
                strErr += ("Не удалось загрузить список каналов сбыта. Текст ошибки: " + f.Message);
            }
            return;
        }
        #endregion

        public override string ToString()
        {
            System.String strInfo = ((SegmentationChanel == null) ? "" : ("канал: " + SegmentationChanel.Code + ";\tсегмент: " + Code + " " + Name));
            return (strInfo);
        }
    }

    public static class CSegmentationChanelDataBaseModel
    {
        #region Добавить объект в базу данных
        /// <summary>
        /// Проверка значений полей объекта перед сохранением в базе данных
        /// </summary>
        /// <param name="SegmentationChannel_Code"></param>
        /// <param name="SegmentationChannel_Name"></param>
        /// <param name="SegmentationChannel_Description"></param>
        /// <param name="strErr"></param>
        /// <returns></returns>
        public static  System.Boolean IsAllParametersValid(System.String SegmentationChannel_Code, 
            System.String SegmentationChannel_Name, System.String SegmentationChannel_Description, 
            ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (SegmentationChannel_Code == "")
                {
                    strErr += ("Необходимо указать код канала продаж!");
                    return bRet;
                }
                if (SegmentationChannel_Name == "")
                {
                    strErr += ("Необходимо указать название канала продаж!");
                    return bRet;
                }
                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr += ("Ошибка проверки свойств канала продаж. Текст ошибки: " + f.Message);
            }
            return bRet;
        }
        /// <summary>
        /// Добавляет запись в базу данных
        /// </summary>
        /// <param name="SegmentationChannel_Code"></param>
        /// <param name="SegmentationChannel_Name"></param>
        /// <param name="SegmentationChannel_Description"></param>
        /// <param name="SegmentationChannel_Guid"></param>
        /// <param name="objProfile"></param>
        /// <param name="strErr"></param>
        /// <returns></returns>
        public static System.Boolean AddNewObjectTodataBase(System.String SegmentationChannel_Code, 
            System.String SegmentationChannel_Name, System.String SegmentationChannel_Description,
            ref System.Guid SegmentationChannel_Guid,
            UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;

            if ( IsAllParametersValid( SegmentationChannel_Code,  SegmentationChannel_Name, SegmentationChannel_Description, ref strErr) == false) { return bRet; }

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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddSegmentationChannel]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationChannel_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationChannel_Code", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationChannel_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationChannel_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@SegmentationChannel_Code"].Value = SegmentationChannel_Code;
                cmd.Parameters["@SegmentationChannel_Name"].Value = SegmentationChannel_Name;
                if( SegmentationChannel_Description == "")
                {
                    cmd.Parameters["@SegmentationChannel_Description"].IsNullable = true;
                    cmd.Parameters["@SegmentationChannel_Description"].Value = null;
                }
                else
                {
                    cmd.Parameters["@SegmentationChannel_Description"].IsNullable = false;
                    cmd.Parameters["@SegmentationChannel_Description"].Value = SegmentationChannel_Description;
                }
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    SegmentationChannel_Guid = (System.Guid)cmd.Parameters["@SegmentationChannel_Guid"].Value;
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

                strErr += ("Не удалось создать объект 'канал продаж'. Текст ошибки: " + f.Message);
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
        /// редактирует запись в базе данных
        /// </summary>
        /// <param name="SegmentationChannel_Guid"></param>
        /// <param name="SegmentationChannel_Code"></param>
        /// <param name="SegmentationChannel_Name"></param>
        /// <param name="SegmentationChannel_Description"></param>
        /// <param name="objProfile"></param>
        /// <param name="strErr"></param>
        /// <returns></returns>
        public static System.Boolean EditObjectInDataBase(System.Guid SegmentationChannel_Guid, System.String SegmentationChannel_Code,
           System.String SegmentationChannel_Name, System.String SegmentationChannel_Description,
           UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;

            if (IsAllParametersValid(SegmentationChannel_Code, SegmentationChannel_Name, SegmentationChannel_Description, ref strErr) == false) { return bRet; }

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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditSegmentationChannel]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationChannel_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationChannel_Code", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationChannel_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationChannel_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@SegmentationChannel_Guid"].Value = SegmentationChannel_Guid;
                cmd.Parameters["@SegmentationChannel_Code"].Value = SegmentationChannel_Code;
                cmd.Parameters["@SegmentationChannel_Name"].Value = SegmentationChannel_Name;
                if (SegmentationChannel_Description == "")
                {
                    cmd.Parameters["@SegmentationChannel_Description"].IsNullable = true;
                    cmd.Parameters["@SegmentationChannel_Description"].Value = null;
                }
                else
                {
                    cmd.Parameters["@SegmentationChannel_Description"].IsNullable = false;
                    cmd.Parameters["@SegmentationChannel_Description"].Value = SegmentationChannel_Description;
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
                    strErr += ((cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();

                strErr += ("Не удалось внести изменения в объект 'канал продаж'. Текст ошибки: " + f.Message);
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
        /// <param name="SegmentationChannel_Guid"></param>
        /// <param name="objProfile"></param>
        /// <param name="strErr"></param>
        /// <returns></returns>
        public static System.Boolean RemoveObjectFromDataBase(System.Guid SegmentationChannel_Guid, 
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteSegmentationChannel]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationChannel_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@SegmentationChannel_Guid"].Value = SegmentationChannel_Guid;
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

                strErr += ("Не удалось удалить объект 'канал продаж'. Текст ошибки: " + f.Message);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список объектов "канал продаж"
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список объектов "сегментация"</returns>
        public static List<CSegmentationChanel> GetSegmentationChanelList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            List<CSegmentationChanel> objList = new List<CSegmentationChanel>();
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr += ("\nНе удалось получить соединение с базой данных.");
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetSegmentationChannel]", objProfile.GetOptionsDllDBName());
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
                        strDscrpn = (rs["SegmentationChannel_Description"] == System.DBNull.Value) ? "" : (System.String)rs["SegmentationChannel_Description"];
                        objList.Add(
                            new CSegmentationChanel()
                            {
                                ID = (System.Guid)rs["SegmentationChannel_Guid"],
                                Code = System.Convert.ToString(rs["SegmentationChannel_Code"]),
                                Name = System.Convert.ToString(rs["SegmentationChannel_Name"]),
                                Description = strDscrpn
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
                strErr += ("\nНе удалось получить список объектов 'канал продаж'. Текст ошибки: " + f.Message);
            }
            return objList;
        }
        #endregion

    }

    public static class CSegmentationMarketDataBaseModel
    {
        #region Добавить объект в базу данных
        /// <summary>
        /// Проверка значений полей объекта перед сохранением в базе данных
        /// </summary>
        /// <param name="SegmentationChannel_Code"></param>
        /// <param name="SegmentationChannel_Name"></param>
        /// <param name="SegmentationChannel_Description"></param>
        /// <param name="strErr"></param>
        /// <returns></returns>
        public static System.Boolean IsAllParametersValid(System.String SegmentationChannel_Code,
            System.String SegmentationChannel_Name, System.String SegmentationChannel_Description,
            ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (SegmentationChannel_Code == "")
                {
                    strErr += ("Необходимо указать код рынка сбыта!");
                    return bRet;
                }
                if (SegmentationChannel_Name == "")
                {
                    strErr += ("Необходимо указать название рынка сбыта!");
                    return bRet;
                }
                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr += ("Ошибка проверки свойств объекта 'рынок сбыта'. Текст ошибки: " + f.Message);
            }
            return bRet;
        }
        /// <summary>
        /// Добавляет запись в базу данных
        /// </summary>
        /// <param name="SegmentationChannel_Code"></param>
        /// <param name="SegmentationChannel_Name"></param>
        /// <param name="SegmentationChannel_Description"></param>
        /// <param name="SegmentationChannel_Guid"></param>
        /// <param name="objProfile"></param>
        /// <param name="strErr"></param>
        /// <returns></returns>
        public static System.Boolean AddNewObjectToDataBase(System.String SegmentationMarket_Code,
            System.String SegmentationMarket_Name, System.String SegmentationMarket_Description,
            ref System.Guid SegmentationMarket_Guid,
            UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;

            if (IsAllParametersValid(SegmentationMarket_Code, SegmentationMarket_Name, SegmentationMarket_Description, ref strErr) == false) { return bRet; }

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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddSegmentationMarket]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationMarket_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationMarket_Code", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationMarket_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationMarket_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@SegmentationMarket_Code"].Value = SegmentationMarket_Code;
                cmd.Parameters["@SegmentationMarket_Name"].Value = SegmentationMarket_Name;
                if (SegmentationMarket_Description == "")
                {
                    cmd.Parameters["@SegmentationMarket_Description"].IsNullable = true;
                    cmd.Parameters["@SegmentationMarket_Description"].Value = null;
                }
                else
                {
                    cmd.Parameters["@SegmentationMarket_Description"].IsNullable = false;
                    cmd.Parameters["@SegmentationMarket_Description"].Value = SegmentationMarket_Description;
                }
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    SegmentationMarket_Guid = (System.Guid)cmd.Parameters["@SegmentationMarket_Guid"].Value;
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

                strErr += ("Не удалось создать объект 'Рынок сбыта'. Текст ошибки: " + f.Message);
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
        /// редактирует запись в базе данных
        /// </summary>
        /// <param name="SegmentationMarket_Guid"></param>
        /// <param name="?"></param>
        /// <param name="?"></param>
        /// <param name="?"></param>
        /// <param name="objProfile"></param>
        /// <param name="strErr"></param>
        /// <returns></returns>
        public static System.Boolean EditObjectInDataBase(System.Guid SegmentationMarket_Guid, 
            System.String SegmentationMarket_Code,  System.String SegmentationMarket_Name, System.String SegmentationMarket_Description,
           UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;

            if (IsAllParametersValid(SegmentationMarket_Code, SegmentationMarket_Name, SegmentationMarket_Description, ref strErr) == false) { return bRet; }

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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditSegmentationMarket]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationMarket_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationMarket_Code", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationMarket_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationMarket_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@SegmentationMarket_Guid"].Value = SegmentationMarket_Guid;
                cmd.Parameters["@SegmentationMarket_Code"].Value = SegmentationMarket_Code;
                cmd.Parameters["@SegmentationMarket_Name"].Value = SegmentationMarket_Name;
                if (SegmentationMarket_Description == "")
                {
                    cmd.Parameters["@SegmentationMarket_Description"].IsNullable = true;
                    cmd.Parameters["@SegmentationMarket_Description"].Value = null;
                }
                else
                {
                    cmd.Parameters["@SegmentationMarket_Description"].IsNullable = false;
                    cmd.Parameters["@SegmentationMarket_Description"].Value = SegmentationMarket_Description;
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
                    strErr += ((cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();

                strErr += ("Не удалось внести изменения в объект 'рынок сбыта'. Текст ошибки: " + f.Message);
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
        /// <param name="SegmentationMarket_Guid"></param>
        /// <param name="objProfile"></param>
        /// <param name="strErr"></param>
        /// <returns></returns>
        public static System.Boolean RemoveObjectFromDataBase(System.Guid SegmentationMarket_Guid,
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteSegmentationMarket]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationMarket_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@SegmentationMarket_Guid"].Value = SegmentationMarket_Guid;
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

                strErr += ("Не удалось удалить объект 'рынок сбыта'. Текст ошибки: " + f.Message);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список объектов "рынок сбыта"
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список объектов "рынок сбыта"</returns>
        public static List<CSegmentationMarket> GetSegmentationMarketList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            List<CSegmentationMarket> objList = new List<CSegmentationMarket>();
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr += ("\nНе удалось получить соединение с базой данных.");
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetSegmentationMarket]", objProfile.GetOptionsDllDBName());
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
                        strDscrpn = (rs["SegmentationMarket_Description"] == System.DBNull.Value) ? "" : (System.String)rs["SegmentationMarket_Description"];
                        objList.Add(
                            new CSegmentationMarket()
                            {
                                ID = (System.Guid)rs["SegmentationMarket_Guid"],
                                Code = System.Convert.ToString(rs["SegmentationMarket_Code"]),
                                Name = System.Convert.ToString(rs["SegmentationMarket_Name"]),
                                Description = strDscrpn
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
                strErr += ("\nНе удалось получить список объектов 'рынок сбыта'. Текст ошибки: " + f.Message);
            }
            return objList;
        }

        public static System.Boolean GetSegmentationMarketInfo(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid SegmentationMarket_Guid, out System.String SegmentationMarket_Code,  out System.String SegmentationMarket_Name,
            ref System.String strErr)
        {
            System.Boolean bRet = false;

            SegmentationMarket_Code = "";
            SegmentationMarket_Name = "";

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr += ("\nНе удалось получить соединение с базой данных.");
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetSegmentationMarket]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationMarket_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@SegmentationMarket_Guid"].Value = SegmentationMarket_Guid;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    rs.Read();
                    {
                        SegmentationMarket_Code = System.Convert.ToString(rs["SegmentationMarket_Code"]);
                        SegmentationMarket_Name = System.Convert.ToString(rs["SegmentationMarket_Name"]);
                    }
                    bRet = true;
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
                strErr += ("\nНе удалось получить информацию о свойствах объекта 'рынок сбыта'. Текст ошибки: " + f.Message);
            }
            return bRet;
        }

        #endregion

    }

    public static class CSegmentationSubChanelDataBaseModel
    {
        #region Добавить объект в базу данных
        /// <summary>
        /// Проверка значений полей объекта перед сохранением в базе данных
        /// </summary>
        /// <param name="SegmentationSubChannel_Code"></param>
        /// <param name="SegmentationSubChannel_Name"></param>
        /// <param name="SegmentationSubChannel_Description"></param>
        /// <param name="SegmentationChannel_Guid"></param>
        /// <param name="strErr"></param>
        /// <returns></returns>
        public static System.Boolean IsAllParametersValid(System.String SegmentationSubChannel_Code,
            System.String SegmentationSubChannel_Name, System.String SegmentationSubChannel_Description, 
            System.Guid SegmentationChannel_Guid,   ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (SegmentationSubChannel_Code == "")
                {
                    strErr += ("Необходимо указать код субканала сбыта!");
                    return bRet;
                }
                if (SegmentationSubChannel_Name == "")
                {
                    strErr += ("Необходимо указать название субканала сбыта!");
                    return bRet;
                }
                if (SegmentationChannel_Guid.Equals( System.Guid.Empty ) == true )
                {
                    strErr += ("Необходимо указать канал сбыта!");
                    return bRet;
                }
                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr += ("Ошибка проверки свойств объекта 'субканал сбыта'. Текст ошибки: " + f.Message);
            }
            return bRet;
        }
        /// <summary>
        /// Добавляет запись в базу данных
        /// </summary>
        /// <param name="SegmentationSubChannel_Code"></param>
        /// <param name="SegmentationSubChannel_Name"></param>
        /// <param name="SegmentationSubChannel_Description"></param>
        /// <param name="SegmentationChannel_Guid"></param>
        /// <param name="SegmentationSubChannel_Guid"></param>
        /// <param name="objProfile"></param>
        /// <param name="strErr"></param>
        /// <returns></returns>
        public static System.Boolean AddNewObjectToDataBase(System.String SegmentationSubChannel_Code,
            System.String SegmentationSubChannel_Name, System.String SegmentationSubChannel_Description,
            System.Guid SegmentationChannel_Guid,
            ref System.Guid SegmentationSubChannel_Guid,
            UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;

            if (IsAllParametersValid(SegmentationSubChannel_Code, SegmentationSubChannel_Name, SegmentationSubChannel_Description, 
                    SegmentationChannel_Guid, ref strErr) == false) { return bRet; }

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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddSegmentationSubChannel]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationSubChannel_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationChannel_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationSubChannel_Code", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationSubChannel_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationSubChannel_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@SegmentationChannel_Guid"].Value = SegmentationChannel_Guid;
                cmd.Parameters["@SegmentationSubChannel_Code"].Value = SegmentationSubChannel_Code;
                cmd.Parameters["@SegmentationSubChannel_Name"].Value = SegmentationSubChannel_Name;
                if (SegmentationSubChannel_Description == "")
                {
                    cmd.Parameters["@SegmentationSubChannel_Description"].IsNullable = true;
                    cmd.Parameters["@SegmentationSubChannel_Description"].Value = null;
                }
                else
                {
                    cmd.Parameters["@SegmentationSubChannel_Description"].IsNullable = false;
                    cmd.Parameters["@SegmentationSubChannel_Description"].Value = SegmentationSubChannel_Description;
                }
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    SegmentationSubChannel_Guid = (System.Guid)cmd.Parameters["@SegmentationSubChannel_Guid"].Value;
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

                strErr += ("Не удалось создать объект 'субканал сбыта'. Текст ошибки: " + f.Message);
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
        /// редактирует запись в базе данных
        /// </summary>
        /// <param name="SegmentationSubChannel_Guid"></param>
        /// <param name="SegmentationSubChannel_Code"></param>
        /// <param name="SegmentationSubChannel_Name"></param>
        /// <param name="SegmentationSubChannel_Description"></param>
        /// <param name="SegmentationChannel_Guid"></param>
        /// <param name="objProfile"></param>
        /// <param name="strErr"></param>
        /// <returns></returns>
        public static System.Boolean EditObjectInDataBase(System.Guid SegmentationSubChannel_Guid, System.String SegmentationSubChannel_Code,
            System.String SegmentationSubChannel_Name, System.String SegmentationSubChannel_Description,
            System.Guid SegmentationChannel_Guid,
            UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;

            if (IsAllParametersValid(SegmentationSubChannel_Code, SegmentationSubChannel_Name, SegmentationSubChannel_Description,
                    SegmentationChannel_Guid, ref strErr) == false) { return bRet; }

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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditSegmentationSubChannel]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationSubChannel_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationChannel_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationSubChannel_Code", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationSubChannel_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationSubChannel_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@SegmentationSubChannel_Guid"].Value = SegmentationSubChannel_Guid;
                cmd.Parameters["@SegmentationSubChannel_Code"].Value = SegmentationSubChannel_Code;
                cmd.Parameters["@SegmentationSubChannel_Name"].Value = SegmentationSubChannel_Name;
                cmd.Parameters["@SegmentationChannel_Guid"].Value = SegmentationChannel_Guid;
                if (SegmentationSubChannel_Description == "")
                {
                    cmd.Parameters["@SegmentationSubChannel_Description"].IsNullable = true;
                    cmd.Parameters["@SegmentationSubChannel_Description"].Value = null;
                }
                else
                {
                    cmd.Parameters["@SegmentationSubChannel_Description"].IsNullable = false;
                    cmd.Parameters["@SegmentationSubChannel_Description"].Value = SegmentationSubChannel_Description;
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
                    strErr += ((cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value);
                }

                cmd.Dispose();
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();

                strErr += ("Не удалось внести изменения в объект 'субканал сбыта'. Текст ошибки: " + f.Message);
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
        public static System.Boolean RemoveObjectFromDataBase(System.Guid SegmentationMarket_Guid,
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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteSegmentationSubChannel]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationSubChannel_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@SegmentationSubChannel_Guid"].Value = SegmentationMarket_Guid;
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

                strErr += ("Не удалось удалить объект 'субканал сбыта'. Текст ошибки: " + f.Message);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список объектов "субканал сбыта"
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список объектов "рынок сбыта"</returns>
        public static List<CSegmentationSubChannel> GetSegmentationSubChannelList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            List<CSegmentationSubChannel> objList = new List<CSegmentationSubChannel>();
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr += ("\nНе удалось получить соединение с базой данных.");
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetSegmentationSubChannel]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Clear();
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
                        strDscrpn = (rs["SegmentationSubChannel_Description"] == System.DBNull.Value) ? "" : (System.String)rs["SegmentationSubChannel_Description"];
                        objList.Add(
                            new CSegmentationSubChannel()
                            {
                                ID = (System.Guid)rs["SegmentationSubChannel_Guid"],
                                Code = System.Convert.ToString(rs["SegmentationSubChannel_Code"]),
                                Name = System.Convert.ToString(rs["SegmentationSubChannel_Name"]),
                                Description = strDscrpn,
                                SegmentationChanel = new CSegmentationChanel()
                                {
                                    ID = (System.Guid)rs["SegmentationChannel_Guid"],
                                    Code = System.Convert.ToString(rs["SegmentationChannel_Code"]),
                                    Name = System.Convert.ToString(rs["SegmentationChannel_Name"])
                                }, 
                                 
                            }
                            );
                    }
                }
                rs.Dispose();

                List<CSegmentationChanel> objSegmentationChanelList = CSegmentationChanelDataBaseModel.GetSegmentationChanelList( objProfile, null, ref strErr );
                foreach (CSegmentationSubChannel objItem in objList)
                {
                    objItem.SetSegmentationChanelList(objSegmentationChanelList);
                }

                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                strErr += ("\nНе удалось получить список объектов 'субканал сбыта'. Текст ошибки: " + f.Message);
            }
            return objList;
        }
        /// <summary>
        /// Возвращает информацию о свойствах объекта "Субканал сбыта" для указанного идентификатора
        /// </summary>
        /// <param name="objProfile"></param>
        /// <param name="cmdSQL"></param>
        /// <param name="SegmentationSubChannel_Guid"></param>
        /// <param name="SegmentationChannel_Guid"></param>
        /// <param name="SegmentationSubChannel_Code"></param>
        /// <param name="SegmentationSubChannel_Name"></param>
        /// <param name="SegmentationChannel_Code"></param>
        /// <param name="SegmentationChannel_Name"></param>
        /// <param name="strErr"></param>
        /// <returns></returns>
        public static System.Boolean GetSegmentationSubChannelInfo(UniXP.Common.CProfile objProfile,  System.Data.SqlClient.SqlCommand cmdSQL,
            System.Guid SegmentationSubChannel_Guid, out System.Guid SegmentationChannel_Guid, out System.String SegmentationSubChannel_Code, 
            out System.String SegmentationSubChannel_Name, out System.String SegmentationChannel_Code, out System.String SegmentationChannel_Name,
            ref System.String strErr)
        {
            System.Boolean bRet = false;

            SegmentationSubChannel_Code = "";
            SegmentationSubChannel_Name = "";
            SegmentationChannel_Guid = System.Guid.Empty;
            SegmentationChannel_Code = "";
            SegmentationChannel_Name = "";

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr += ("\nНе удалось получить соединение с базой данных.");
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetSegmentationSubChannel]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SegmentationSubChannel_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@SegmentationSubChannel_Guid"].Value = SegmentationSubChannel_Guid; 
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    rs.Read();
                    {
                        SegmentationSubChannel_Code  =  System.Convert.ToString(rs["SegmentationSubChannel_Code"]);
                        SegmentationSubChannel_Name = System.Convert.ToString(rs["SegmentationSubChannel_Name"]);
                        SegmentationChannel_Guid = (System.Guid)rs["SegmentationChannel_Guid"];
                        SegmentationChannel_Code = System.Convert.ToString(rs["SegmentationChannel_Code"]);
                        SegmentationChannel_Name = System.Convert.ToString(rs["SegmentationChannel_Name"]);
                    }
                    bRet = true;
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
                strErr += ("\nНе удалось получить информацию о свойствах объекта 'субканал сбыта'. Текст ошибки: " + f.Message);
            }
            return bRet;
        }

        #endregion

    }



}
