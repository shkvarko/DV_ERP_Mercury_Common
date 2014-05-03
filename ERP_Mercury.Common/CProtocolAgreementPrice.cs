using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP_Mercury.Common
{
    /// <summary>
    /// Класс "Строка приложения к ПСЦ"
    /// </summary>
    public class CProtocolAgreementPriceItem
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
        /// № п/п
        /// </summary>
        private System.Int32 m_iOrderId;
        /// <summary>
        /// № п/п
        /// </summary>
        public System.Int32 OrderId
        {
            get { return m_iOrderId; }
            set { m_iOrderId = value; }
        }
        /// <summary>
        /// Товар
        /// </summary>
        private CProduct m_objProduct;
        /// <summary>
        /// Товар
        /// </summary>
        public CProduct Product
        {
            get { return m_objProduct; }
            set { m_objProduct = value; }
        }

        /// <summary>
        /// Цена по прейскуранту, руб.
        /// </summary>
        private System.Double m_dblPrice;
        /// <summary>
        /// Цена по прейскуранту, руб.
        /// </summary>
        public System.Double Price
        {
            get { return m_dblPrice; }
            set { m_dblPrice = value; }
        }
        /// <summary>
        /// % скидки
        /// </summary>
        private System.Double m_dblDiscountPercent;
        /// <summary>
        /// % скидки
        /// </summary>
        public System.Double DiscountPercent
        {
            get { return m_dblDiscountPercent; }
            set { m_dblDiscountPercent = value; }
        }
        /// <summary>
        /// Ставка НДС, %
        /// </summary>
        private System.Double m_dblNDSPercent;
        /// <summary>
        /// Ставка НДС, %
        /// </summary>
        public System.Double NDSPercent
        {
            get { return m_dblNDSPercent; }
            set { m_dblNDSPercent = value; }
        }
        /// <summary>
        /// Отпускная цена с учетом скидки, руб.
        /// </summary>
        private System.Double m_dblPriceWithDiscount;
        /// <summary>
        /// Отпускная цена с учетом скидки, руб.
        /// </summary>
        public System.Double PriceWithDiscount
        {
            get { return m_dblPriceWithDiscount; }
            set { m_dblPriceWithDiscount = value; }
        }
        /// <summary>
        /// Отпускная цена с учетом скидки и НДС, руб.
        /// </summary>
        private System.Double m_dblPriceWithDiscountNDS;
        /// <summary>
        /// Отпускная цена с учетом скидки и НДС, руб.
        /// </summary>
        public System.Double PriceWithDiscountNDS
        {
            get { return m_dblPriceWithDiscountNDS; }
            set { m_dblPriceWithDiscountNDS = value; }
        }

        #endregion

        #region Конструктор
        public CProtocolAgreementPriceItem()
        {
            m_uuidID = System.Guid.Empty;
            m_iOrderId = 0;
            m_objProduct = null;
            m_dblPrice = 0;
            m_dblDiscountPercent = 0;
            m_dblNDSPercent = 0;
            m_dblPriceWithDiscount = 0;
            m_dblPriceWithDiscountNDS = 0;
        }
        public CProtocolAgreementPriceItem( System.Guid uuidID, System.Int32 iOrderId, CProduct objProduct, System.Double dblPrice,
           System.Double dblDiscountPercent, System.Double dblPriceWithDiscount, System.Double dblNDSPercent, 
            System.Double dblPriceWithDiscountNDS )
        {
            m_uuidID = uuidID;
            m_iOrderId = iOrderId;
            m_objProduct = objProduct;
            m_dblPrice = dblPrice;
            m_dblDiscountPercent = dblDiscountPercent;
            m_dblNDSPercent = dblNDSPercent;
            m_dblPriceWithDiscount = dblPriceWithDiscount;
            m_dblPriceWithDiscountNDS = dblPriceWithDiscountNDS;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает содержимое ПСЦ
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="uuidProtocolAgreementPriceId">уи ПСЦ</param>
        /// <returns>список приложение к ПСЦ</returns>
        public static List<CProtocolAgreementPriceItem> GetProtocolAgreementPriceItemList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidProtocolAgreementPriceId)
        {
            List<CProtocolAgreementPriceItem> objList = new List<CProtocolAgreementPriceItem>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetProtocolAgreementPriceItems]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProtocolAgreementPrice_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters["@ProtocolAgreementPrice_Guid"].Value = uuidProtocolAgreementPriceId;
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CProtocolAgreementPriceItem objPSCItem = null;
                    while (rs.Read())
                    {
                        objPSCItem = new CProtocolAgreementPriceItem();
                        objPSCItem.m_uuidID = (System.Guid)rs["ProtocolAgreementPriceItem_Guid"];
                        objPSCItem.m_iOrderId = (rs["ProtocolAgreementPriceItem_OrderId"] == System.DBNull.Value) ? 0 : (System.Int32)rs["ProtocolAgreementPriceItem_OrderId"];
                        objPSCItem.Price = (rs["ProtocolAgreementPriceItem_Price"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["ProtocolAgreementPriceItem_Price"]);
                        objPSCItem.DiscountPercent = (rs["ProtocolAgreementPriceItem_DiscountPercent"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["ProtocolAgreementPriceItem_DiscountPercent"]);
                        objPSCItem.NDSPercent = (rs["ProtocolAgreementPriceItem_NDSPercent"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["ProtocolAgreementPriceItem_NDSPercent"]);
                        objPSCItem.PriceWithDiscountNDS = (rs["ProtocolAgreementPriceItem_lPriceWithDiscountNDS"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["ProtocolAgreementPriceItem_lPriceWithDiscountNDS"]);
                        objPSCItem.PriceWithDiscount = (rs["ProtocolAgreementPriceItem_PriceWithDiscount"] == System.DBNull.Value) ? 0 : System.Convert.ToDouble(rs["ProtocolAgreementPriceItem_PriceWithDiscount"]);

                        objPSCItem.Product = new CProduct();
                        objPSCItem.Product.ID = (System.Guid)rs["Parts_Guid"];
                        objPSCItem.Product.ID_Ib = System.Convert.ToInt32( rs["Parts_Id"] );
                        objPSCItem.Product.Name = System.Convert.ToString( rs["Parts_Name"] );
                        objPSCItem.Product.Article = System.Convert.ToString( rs["Parts_Article"] );
                        objPSCItem.Product.CodeTNVD = ((rs["Parts_CodeTNVD"] == null) ? "" : System.Convert.ToString(rs["Parts_CodeTNVD"]));
                        if (rs["Barcode"] != System.DBNull.Value)
                        {
                            objPSCItem.Product.BarcodeList = new List<System.String>();
                            objPSCItem.Product.BarcodeList.Add(System.Convert.ToString(rs["Barcode"]));
                        }

                        objPSCItem.Product.Measure = new CMeasure( (System.Guid)rs["Measure_Guid"],  System.Convert.ToString( rs["Measure_Name"] ), System.Convert.ToString( rs["Measure_ShortName"] ) );
                        objPSCItem.Product.ProductTradeMark = new CProductTradeMark(
                                  (System.Guid)rs["Owner_Guid"],
                                  (System.String)rs["Owner_Name"], (System.String)rs["Owner_Name"],
                                  System.Convert.ToInt32(rs["Owner_Id"]), 0, "", true,
                                  new CProductVtm((System.Guid)rs["Vtm_Guid"], (System.String)rs["Vtm_Name"], System.Convert.ToInt32(rs["Vtm_Id"]), (System.String)rs["Vtm_Name"], "", true));                                
                        objPSCItem.Product.ProductType = new CProductType(
                                (System.Guid)rs["Parttype_Guid"],
                                (System.String)rs["Parttype_Name"],
                                System.Convert.ToInt32(rs["Parttype_Id"]),
                                (System.String)rs["Parttype_DemandsName"],
                                System.Convert.ToDouble(rs["Parttype_NDSRate"]), "", true);
                        if (rs["Partsubtype_Guid"] != System.DBNull.Value)
                        {
                            objPSCItem.Product.ProductSubType = new CProductSubType(
                                    (System.Guid)rs["Partsubtype_Guid"],
                                    (System.String)rs["Partsubtype_Name"],
                                    System.Convert.ToInt32(rs["Partsubtype_Id"]), "", true,
                                    new CProductLine(
                                    (System.Guid)rs["Partline_Guid"],
                                    (System.String)rs["Partline_Name"],
                                    System.Convert.ToInt32(rs["Partline_Id"]), "", true)
                                    );
                        }
                        objPSCItem.Product.PackQuantity = System.Convert.ToInt32(rs["Parts_PackQuantity"]);
                        objList.Add( objPSCItem );
                    }
                    objPSCItem = null;
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
                "Не удалось получить приложение к ПСЦ.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        #endregion

        #region Сохранение приложения к ПСЦ в БД

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
                if (this.Product == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Укажите, пожалуйста, товар!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.Price == 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Укажите, пожалуйста, цену!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.PriceWithDiscountNDS == 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Укажите, пожалуйста, цену со скидкой!", "Внимание",
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

        /// <summary>
        /// Сохранение приложения к ПСЦ в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="objPriceItemList">приложение к ПСЦ</param>
        /// <param name="uuidProtocolAgreementPriceId">уи ПСЦ</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean SaveProtocolAgreementPriceItemList(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL,
            List<CProtocolAgreementPriceItem> objPriceItemList, System.Guid uuidProtocolAgreementPriceId, ref System.String strErr)
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
                System.Data.DataTable addedItems = new System.Data.DataTable();
                addedItems.Columns.Add(new System.Data.DataColumn("ProtocolAgreementPriceItem_OrderId", typeof(System.Data.SqlTypes.SqlInt32)));
                addedItems.Columns.Add(new System.Data.DataColumn("ProtocolAgreementPriceItem_Price", typeof(System.Data.SqlTypes.SqlDouble)));
                addedItems.Columns.Add(new System.Data.DataColumn("ProtocolAgreementPriceItem_DiscountPercent", typeof(System.Data.SqlTypes.SqlDouble)));
                addedItems.Columns.Add(new System.Data.DataColumn("ProtocolAgreementPriceItem_NDSPercent", typeof(System.Data.SqlTypes.SqlDouble)));
                addedItems.Columns.Add(new System.Data.DataColumn("ProtocolAgreementPriceItem_lPriceWithDiscountNDS", typeof(System.Data.SqlTypes.SqlDouble)));
                addedItems.Columns.Add(new System.Data.DataColumn("Parts_Guid", typeof(System.Data.SqlTypes.SqlGuid)));
                addedItems.Columns.Add(new System.Data.DataColumn("ProtocolAgreementPriceItem_PriceWithDiscount", typeof(System.Data.SqlTypes.SqlDouble)));

                System.Data.DataRow newRow = null;
                foreach (CProtocolAgreementPriceItem objItem in objPriceItemList)
                {
                    newRow = addedItems.NewRow();
                    newRow["ProtocolAgreementPriceItem_OrderId"] = objItem.OrderId;
                    newRow["ProtocolAgreementPriceItem_Price"] = objItem.Price;
                    newRow["ProtocolAgreementPriceItem_DiscountPercent"] = objItem.DiscountPercent;
                    newRow["ProtocolAgreementPriceItem_NDSPercent"] = objItem.NDSPercent;
                    newRow["ProtocolAgreementPriceItem_lPriceWithDiscountNDS"] = objItem.PriceWithDiscountNDS;
                    newRow["Parts_Guid"] = objItem.Product.ID;
                    newRow["ProtocolAgreementPriceItem_PriceWithDiscount"] = objItem.PriceWithDiscount;
                    addedItems.Rows.Add(newRow);
                }
                addedItems.AcceptChanges();

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddProtocolAgreementItemList]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.AddWithValue("@tProtocolAgreementPriceItem", addedItems);
                cmd.Parameters["@tProtocolAgreementPriceItem"].SqlDbType = System.Data.SqlDbType.Structured;
                cmd.Parameters["@tProtocolAgreementPriceItem"].TypeName = "dbo.udt_ProtocolAgreementPriceItem";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProtocolAgreementPrice_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters["@ProtocolAgreementPrice_Guid"].Value = uuidProtocolAgreementPriceId;
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

        #region Расчет отпускной цены
        /// <summary>
        /// Расчет отпускной цены с учетом скидки
        /// </summary>
        /// <param name="dblPrice">цена</param>
        /// <param name="dblDiscountPercent">размер скидки в процента</param>
        /// <returns>отпускная цена с учетом скидки и НДС</returns>
        public static System.Double CalcPriceWithDiscount( System.Double dblPrice, System.Double dblDiscountPercent, System.Boolean bRoundPrice )
        {
            System.Double dblRes = 0;
            try
            {
                if (dblDiscountPercent == 0)
                {
                    dblRes = dblPrice;
                }
                else
                {
                    System.Double dblPriceWithDiscountNds = (dblPrice * (1 - (dblDiscountPercent / 100)));
                    if (bRoundPrice == true)
                    {
                        dblPriceWithDiscountNds = dblPriceWithDiscountNds / 100;
                        if ((dblPriceWithDiscountNds - Math.Truncate(dblPriceWithDiscountNds)) < 0.5)
                        {
                            dblPriceWithDiscountNds = (Math.Truncate(dblPriceWithDiscountNds) + 0.5) * 100;
                        }
                        else if ((dblPriceWithDiscountNds - Math.Truncate(dblPriceWithDiscountNds)) > 0.5)
                        {
                            dblPriceWithDiscountNds = (Math.Truncate(dblPriceWithDiscountNds) + 1) * 100;
                        }
                        else if ((dblPriceWithDiscountNds - Math.Truncate(dblPriceWithDiscountNds)) == 0.5)
                        {
                            dblPriceWithDiscountNds = dblPriceWithDiscountNds * 100;
                        }
                    }

                    dblRes = dblPriceWithDiscountNds;
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка расчета цены с учетом скидки.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return dblRes;

        }
        /// <summary>
        /// Добавляет НДС к цене
        /// </summary>
        /// <param name="dblPrice">цена с учетом скидки</param>
        /// <param name="dblNDSPercent">ставка НДС в процентах</param>
        /// <returns>отпускная цена с учетом скидки и НДС</returns>
        public static System.Double AddNDSToPrice(System.Double dblPrice, System.Double dblNDSPercent)
        {
            System.Double dblRes = 0;
            try
            {
                dblRes = (dblPrice * (1 + (dblNDSPercent / 100)));
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка НДС к цене.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return dblRes;

        }
        #endregion
    }
    /// <summary>
    /// Класс "Протокол согласования цен (ПСЦ)"
    /// </summary>
    public class CProtocolAgreementPrice
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
        /// Дата начала действия
        /// </summary>
        private System.DateTime m_dtBeginDate;
        /// <summary>
        /// Дата начала действия
        /// </summary>
        public System.DateTime BeginDate
        {
            get { return m_dtBeginDate; }
            set { m_dtBeginDate = value; }
        }
        /// <summary>
        /// Дата окончания действия
        /// </summary>
        private System.DateTime m_dtEndDate;
        /// <summary>
        /// Дата окончания действия
        /// </summary>
        public System.DateTime EndDate
        {
            get { return m_dtEndDate; }
            set { m_dtEndDate = value; }
        }
        /// <summary>
        /// Номер документа
        /// </summary>
        private System.String m_strDocumentNumber;
        /// <summary>
        /// Номер документа
        /// </summary>
        public System.String DocumentNumber
        {
            get { return m_strDocumentNumber; }
            set { m_strDocumentNumber = value; }
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
        /// Признак активности
        /// </summary>
        private System.Boolean m_bIsActive;
        /// <summary>
        /// Признак активности
        /// </summary>
        public System.Boolean IsActive
        {
            get { return m_bIsActive; }
            set { m_bIsActive = value; }
        }
        /// <summary>
        /// Состояние документа
        /// </summary>
        private CAgreementState m_objAgreementSate;
        /// <summary>
        /// Состояние документа
        /// </summary>
        public CAgreementState AgreementSate
        {
            get { return m_objAgreementSate; }
            set { m_objAgreementSate = value; }
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
        public System.String CompanyName
        {
            get { return ((m_objCompany == null) ? "" : m_objCompany.Name); }
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
        public System.String CustomerName
        {
            get { return ((m_objCustomer == null) ? "" : m_objCustomer.FullName); }
        }
        public System.String AgreementNum
        {
            get { return ((this == null) ? "" : this.DocumentNumber); }
        }
        public System.String AgreementBeginDate
        {
            get { return ((this == null) ? "" : this.BeginDate.ToShortDateString()); }
        }
        public System.String AgreementEndDate
        {
            get { return ((this == null) ? "" : this.EndDate.ToShortDateString()); }
        }
        /// <summary>
        /// Приложение к ПСЦ
        /// </summary>
        private List<CProtocolAgreementPriceItem> m_objPriceItemList;
        /// <summary>
        /// Приложение к ПСЦ
        /// </summary>
        public List<CProtocolAgreementPriceItem> PriceItemList
        {
            get { return m_objPriceItemList; }
            set { m_objPriceItemList = value; }
        }
        #endregion

        #region Конструктор
        public CProtocolAgreementPrice()
        {
            m_uuidID = System.Guid.Empty;
            m_strDocumentNumber = "";
            m_bIsActive = true;
            m_dtBeginDate = System.DateTime.Today;
            m_dtEndDate = System.DateTime.Today;
            m_strDescription = "";
            m_objAgreementSate = null;
            m_objCompany = null;
            m_objCustomer = null;
            m_objPriceItemList = null;
        }

        public CProtocolAgreementPrice( System.Guid uuidID, System.String strDocumentNumber, System.Boolean bIsActive,
            System.DateTime dtBeginDate, System.DateTime dtEndDate, System.String strDescription,
            CAgreementState objAgreementSate, CCompany objCompany, CCustomer objCustomer )
        {
            m_uuidID = uuidID;
            m_strDocumentNumber = strDocumentNumber;
            m_bIsActive = bIsActive;
            m_dtBeginDate = dtBeginDate;
            m_dtEndDate = dtEndDate;
            m_strDescription = strDescription;
            m_objAgreementSate = objAgreementSate;
            m_objCompany = objCompany;
            m_objCustomer = objCustomer;
            m_objPriceItemList = null;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает список ПСЦ
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="dtBeginDate">период с</param>
        /// <param name="dtEndDate">период по</param>
        /// <param name="uuidCompanyId">уи компании</param>
        /// <param name="uuidCustomerId">уи клиента</param>
        /// <returns>список ПСЦ</returns>
        public static List<CProtocolAgreementPrice> GetProtocolAgreementPriceList( UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, System.DateTime dtBeginDate, System.DateTime dtEndDate,
            System.Guid uuidCompanyId, System.Guid uuidCustomerId )
        {
            List<CProtocolAgreementPrice> objList = new List<CProtocolAgreementPrice>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetProtocolAgreementPrice]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BeginDate", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EndDate", System.Data.SqlDbType.Date));
                cmd.Parameters["@BeginDate"].Value = dtBeginDate;
                cmd.Parameters["@EndDate"].Value = dtEndDate;
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                if (uuidCompanyId.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@Company_Guid"].Value = uuidCompanyId;
                }
                if (uuidCustomerId.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@Customer_Guid"].Value = uuidCustomerId;
                }

                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    CProtocolAgreementPrice objPSC = null;
                    while (rs.Read())
                    {
                        objPSC = new CProtocolAgreementPrice();
                        objPSC.m_uuidID = (System.Guid)rs["ProtocolAgreementPrice_Guid"];
                        objPSC.m_strDocumentNumber = (rs["ProtocolAgreementPrice_Num"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["ProtocolAgreementPrice_Num"]);
                        objPSC.m_strDescription = (rs["ProtocolAgreementPrice_Descriptoin"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["ProtocolAgreementPrice_Descriptoin"]);
                        objPSC.m_dtBeginDate = (rs["ProtocolAgreementPrice_BeginDate"] == System.DBNull.Value) ? System.DateTime.MinValue : System.Convert.ToDateTime(rs["ProtocolAgreementPrice_BeginDate"]);
                        objPSC.m_dtEndDate = (rs["ProtocolAgreementPrice_EndDate"] == System.DBNull.Value) ? System.DateTime.MinValue : System.Convert.ToDateTime(rs["ProtocolAgreementPrice_EndDate"]);
                        objPSC.m_bIsActive = (rs["ProtocolAgreementPrice_IsActive"] == System.DBNull.Value) ? false : System.Convert.ToBoolean(rs["ProtocolAgreementPrice_IsActive"]);
                        objPSC.m_objAgreementSate = (rs["AgreementState_Guid"] == System.DBNull.Value) ? null : new CAgreementState(
                            (System.Guid)rs["AgreementState_Guid"],
                            (System.String)rs["AgreementState_Name"],
                            ((rs["AgreementState_Description"] == System.DBNull.Value) ? "" : (System.String)rs["AgreementState_Description"])
                            );
                        objPSC.Company = new CCompany((System.Guid)rs["Company_Guid"],
                            ((rs["Company_Id"] == System.DBNull.Value) ? 0 : (System.Int32)rs["Company_Id"]),
                            (System.String)rs["Company_Name"],
                            (System.String)rs["Company_Acronym"],
                            (System.String)rs["Company_Description"],
                            (System.String)rs["Company_OKPO"],
                            ((rs["Company_OKULP"] == System.DBNull.Value) ? "" : (System.String)rs["Company_OKULP"]),
                            (System.String)rs["Company_UNN"],
                            true,
                            // тип компании
                                new CCompanyType((System.Guid)rs["CompanyType_Guid"],
                                                 (System.String)rs["CompanyType_name"]),
                            // форма собственности
                                new CStateTypeCompany()
                                );
                        objPSC.Customer = new CCustomer((System.Guid)rs["Customer_Guid"], (System.Int32)rs["CUSTOMER_ID"], (System.String)rs["Customer_Code"],
                            (System.String)rs["CUSTOMER_NAME"], (System.String)rs["CUSTOMER_NAME"],
                            (System.String)rs["Customer_UNP"], (System.String)rs["Customer_OKPO"],
                            ((rs["Customer_OKULP"] == System.DBNull.Value) ? "" : (System.String)rs["Customer_OKULP"]),
                            // признак активности
                            new CCustomerActiveType((System.Guid)rs["CustomerActiveType_Guid"], (System.String)rs["CustomerActiveType_Name"]),
                            // форма собственности
                            new CStateType((System.Guid)rs["CustomerStateType_Guid"],
                            (System.String)rs["CustomerStateType_Name"],
                            (System.String)rs["CustomerStateType_ShortName"],
                            (System.Boolean)rs["CustomerStateType_IsActive"])
                            );


                        objList.Add(objPSC);
                    }
                    objPSC = null;
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
                "Не удалось список ПСЦ.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        #endregion

        #region Добавить запись в БД

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
                if (this.DocumentNumber == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Укажите, пожалуйста, номер ПСЦ!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.BeginDate == System.DateTime.MinValue)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Укажите, пожалуйста, дату начала действия ПСЦ!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.EndDate == System.DateTime.MinValue)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Укажите, пожалуйста, дату окончания действия ПСЦ!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.AgreementSate == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Укажите, пожалуйста, состояние ПСЦ!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.Customer == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Укажите, пожалуйста, клиента!", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.Company == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Укажите, пожалуйста, компанию!", "Внимание",
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

        /// <summary>
        /// Добавить запись в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean AddProtocolAgreementPrice(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            System.Boolean bRet = false;
            if (IsAllParametersValid() == false) { return bRet; }

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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddProtocolAgreementPrice]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProtocolAgreementPrice_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProtocolAgreementPrice_Num", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProtocolAgreementPrice_Descriptoin", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProtocolAgreementPrice_BeginDate", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProtocolAgreementPrice_EndDate", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProtocolAgreementPrice_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AgreementState_Guid", System.Data.SqlDbType.UniqueIdentifier));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@ProtocolAgreementPrice_Num"].Value = this.DocumentNumber;
                cmd.Parameters["@ProtocolAgreementPrice_Descriptoin"].Value = this.Description;
                cmd.Parameters["@ProtocolAgreementPrice_BeginDate"].Value = this.BeginDate;
                cmd.Parameters["@ProtocolAgreementPrice_EndDate"].Value = this.EndDate;
                cmd.Parameters["@Customer_Guid"].Value = this.Customer.ID;
                cmd.Parameters["@Company_Guid"].Value = this.Company.ID;
                cmd.Parameters["@AgreementState_Guid"].Value = this.AgreementSate.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.ID = (System.Guid)cmd.Parameters["@ProtocolAgreementPrice_Guid"].Value;
                    // приложение
                    iRes = ( CProtocolAgreementPriceItem.SaveProtocolAgreementPriceItemList(objProfile, cmd, this.PriceItemList, this.ID, ref strErr) == true ) ? 0 : -1;
                }
                else
                {
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
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

        #region Редактировать запись в БД
        /// <summary>
        /// Редактировать запись в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean EditProtocolAgreementPrice(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            System.Boolean bRet = false;
            if (IsAllParametersValid() == false) { return bRet; }

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
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditProtocolAgreementPrice]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProtocolAgreementPrice_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProtocolAgreementPrice_Num", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProtocolAgreementPrice_Descriptoin", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProtocolAgreementPrice_BeginDate", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProtocolAgreementPrice_EndDate", System.Data.SqlDbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProtocolAgreementPrice_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Customer_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Company_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@AgreementState_Guid", System.Data.SqlDbType.UniqueIdentifier));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@ProtocolAgreementPrice_Guid"].Value = this.ID;
                cmd.Parameters["@ProtocolAgreementPrice_Num"].Value = this.DocumentNumber;
                cmd.Parameters["@ProtocolAgreementPrice_Num"].Value = this.DocumentNumber;
                cmd.Parameters["@ProtocolAgreementPrice_Descriptoin"].Value = this.Description;
                cmd.Parameters["@ProtocolAgreementPrice_BeginDate"].Value = this.BeginDate;
                cmd.Parameters["@ProtocolAgreementPrice_EndDate"].Value = this.EndDate;
                cmd.Parameters["@Customer_Guid"].Value = this.Customer.ID;
                cmd.Parameters["@Company_Guid"].Value = this.Company.ID;
                cmd.Parameters["@AgreementState_Guid"].Value = this.AgreementSate.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    // приложение
                    iRes = (CProtocolAgreementPriceItem.SaveProtocolAgreementPriceItemList(objProfile, cmd, this.PriceItemList, this.ID, ref strErr) == true) ? 0 : -1;
                }
                else
                {
                    strErr = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
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

        #region Удалить запись из БД
        /// <summary>
        /// Удалить запись из БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean RemoveProtocolAgreementPrice(UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteProtocolAgreementPrice]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProtocolAgreementPrice_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@ProtocolAgreementPrice_Guid"].Value = this.ID;
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
    /// <summary>
    /// Информация о текущем остатке на складах отгрузки
    /// </summary>
    public class CStockRestInfo
    {
        #region Свойства
        /// <summary>
        /// УИ товара
        /// </summary>
        public System.Guid uuidPartsID { get; set; }
        /// <summary>
        /// Текущий остаток с учетом резерва
        /// </summary>
        public System.Double dblStockRestQuantity { get; set; }
        #endregion

        #region Конструктор
        public CStockRestInfo()
        {
            uuidPartsID = System.Guid.Empty;
            dblStockRestQuantity = 0;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Возвращает информацию о текущем остатке в виде списка объектов класса CStockRestInfo
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список объектов класса CStockRestInfo</returns>
        public static List<CStockRestInfo> GetStockRestInfoList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL )
        {
            List<CStockRestInfo> objList = new List<CStockRestInfo>();
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetCurrentStockRestFromIB]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        if( ( rs["Parts_Guid"] != System.DBNull.Value ) && ( rs["Quantity"] != System.DBNull.Value ) )
                        {
                            objList.Add( new CStockRestInfo() { uuidPartsID = (System.Guid)rs["Parts_Guid"], dblStockRestQuantity = System.Convert.ToDouble(rs["Quantity"])  } );
                        }
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
                "Не удалось получить текущий остаток.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }

        #endregion
    }
}
