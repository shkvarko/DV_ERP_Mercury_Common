using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace ERP_Mercury.Common
{
    /// <summary>
    /// ����� "��� ������"
    /// </summary>
    public class CEarningType : CBusinessObject
    {
        #region ��������
        /// <summary>
        /// ����������
        /// </summary>
        [DisplayName("����������")]
        [Description("����������")]
        [Category("2. �������������� ��������")]
        public System.String Description { get; set; }
        /// <summary>
        /// ����� � ������� ���������
        /// </summary>
        [DisplayName("��� ���� ������")]
        [Description("������������� ���������� ������������� ���� ������")]
        [Category("1. ������������ ��������")]
        public System.Int32 EarningTypeId { get; set; }
        /// <summary>
        /// ������� "�������"
        /// </summary>
        [DisplayName("�������")]
        [Description("������� ���������� ������")]
        [Category("1. ������������ ��������")]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public System.Boolean IsActive { get; set; }
        /// <summary>
        /// ������� "�� ���������"
        /// </summary>
        [DisplayName("�� ���������")]
        [Description("���� �������� ����������� � \"��\", �� ��� ������ ��������������� �� ��������� � ����� ����������")]
        [Category("1. ������������ ��������")]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public System.Boolean IsDefault { get; set; }
        /// <summary>
        /// ������� "����������� � "���������""
        /// </summary>
        [DisplayName("����������� � \"���������\"")]
        [Description("���� �������� ����������� � \"��\", �� ����� � ������ ����� ������ ����������� � \"���������\"")]
        [Category("1. ������������ ��������")]
        [TypeConverter(typeof(BooleanTypeConverter))]
        public System.Boolean IsDublicateInIB { get; set; }
        #endregion

        #region �����������
        public CEarningType()
            : base()
        {
            EarningTypeId = -1;
            IsActive = false;
            Description = "";
            IsDefault = false;
            IsDublicateInIB = false;

        }
        #endregion

        #region ������ ��������
        /// <summary>
        /// ���������� ������ ����� �����
        /// </summary>
        /// <param name="objProfile">�������</param>
        /// <param name="cmdSQL">SQL-�������</param>
        /// <param name="strErr">��������� �� ������</param>
        /// <returns>������ ����� �����</returns>
        public static List<CEarningType> GetEarningTypeList(UniXP.Common.CProfile objProfile, ref System.String strErr )
        {
            return CEarningTypeDataBaseModel.GetEarningTypeList(objProfile, null, ref strErr);
        }
        #endregion

        #region Remove
        /// <summary>
        /// ������� ������ �� ��
        /// </summary>
        /// <param name="objProfile">�������</param>
        /// <param name="uuidID">���������� ������������� �������</param>
        /// <returns>true - ������� ����������; false - ������</returns>
        public override System.Boolean Remove(UniXP.Common.CProfile objProfile)
        {
            System.String strErr = "";

            System.Boolean bRet = CEarningTypeDataBaseModel.RemoveObjectFromDataBase(this.ID, objProfile, ref strErr);
            if (bRet == false)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "��������",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return bRet;
        }
        #endregion

        #region Add
        /// <summary>
        /// �������� ������ � ��
        /// </summary>
        /// <param name="objProfile">�������</param>
        /// <returns>true - ������� ����������; false - ������</returns>
        public override System.Boolean Add(UniXP.Common.CProfile objProfile)
        {
            System.String strErr = "";

            System.Guid GUID_ID = System.Guid.Empty;

            System.Boolean bRet = CEarningTypeDataBaseModel.AddNewObjectToDataBase(
                this.Name, this.Description, this.IsActive, this.IsDefault, this.IsDublicateInIB,
                this.EarningTypeId, ref GUID_ID, objProfile, ref strErr);
            if (bRet == true)
            {
                this.ID = GUID_ID;
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "��������",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return bRet;
        }
        #endregion

        #region Update
        /// <summary>
        /// ��������� ��������� � ��
        /// </summary>
        /// <param name="objProfile">�������</param>
        /// <returns>true - ������� ����������; false - ������</returns>
        public override System.Boolean Update(UniXP.Common.CProfile objProfile)
        {
            System.String strErr = "";

            System.Boolean bRet = CEarningTypeDataBaseModel.EditObjectInDataBase( this.ID, 
                this.Name,  this.Description, this.IsActive, this.IsDefault, this.IsDublicateInIB, this.EarningTypeId, objProfile, ref strErr);
            if (bRet == false)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "��������",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
    /// ����� ��� ������ � ����� ������
    /// </summary>
    public static class CEarningTypeDataBaseModel
    {
        #region ���������� ����� ������
        /// <summary>
        /// �������� ������� ������� ����� ����������� � ���� ������
        /// </summary>
        /// <param name="EarningType_Name">������������ ���� ������</param>
        /// <param name="EarningType_Id">��� ���� ������</param>
        /// <param name="strErr">��������� �� ������</param>
        /// <returns>true - �������� ��������; false - �������� �� ��������</returns>
        public static System.Boolean IsAllParametersValid(System.String EarningType_Name, System.Int32 EarningType_Id,
            ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (EarningType_Name.Trim() == "")
                {
                    strErr += ("���������� ������� ������������ ���� ������!");
                    return bRet;
                }
                if (EarningType_Id < 0)
                {
                    strErr += ("���������� ������� ��� ���� ������!");
                    return bRet;
                }
                bRet = true;
            }
            catch (System.Exception f)
            {
                strErr += ("������ �������� ������� ������� \"��� ������\". ����� ������: " + f.Message);
            }
            return bRet;
        }
        /// <summary>
        /// ���������� ����� ������ � ��������� ���� ������ � ���� ������
        /// </summary>
        /// <param name="EarningType_Name">������������ ���� ������</param>
        /// <param name="EarningType_Description">����������</param>
        /// <param name="EarningType_IsActive">������� "������ �������"</param>
        /// <param name="EarningType_IsDefault">������� "������������ �� ���������"</param>
        /// <param name="EarningType_DublicateInIB">������� "����������� ������ � "��������""</param>
        /// <param name="EarningType_Id">��� ���� ������</param>
        /// <param name="EarningType_Guid">�� ������</param>
        /// <param name="objProfile">�������</param>
        /// <param name="strErr">��������� �� ������</param>
        /// <returns>true - ������� ���������� ��������; false - ������</returns>
        public static System.Boolean AddNewObjectToDataBase(System.String EarningType_Name,
            System.String EarningType_Description, System.Boolean EarningType_IsActive,
            System.Boolean EarningType_IsDefault, System.Boolean EarningType_DublicateInIB, 
            System.Int32 EarningType_Id,  ref System.Guid EarningType_Guid, 
            UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;

            if (IsAllParametersValid(EarningType_Name, EarningType_Id, ref strErr) == false) { return bRet; }

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;

            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    strErr += ("�� ������� �������� ���������� � ����� ������.");
                    return bRet;
                }
                DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_AddEarningType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EarningType_Guid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EarningType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EarningType_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EarningType_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EarningType_IsDefault", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EarningType_DublicateInIB", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EarningType_Id", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@EarningType_Name"].Value = EarningType_Name;
                cmd.Parameters["@EarningType_Description"].Value = EarningType_Description;
                cmd.Parameters["@EarningType_IsActive"].Value = EarningType_IsActive;
                cmd.Parameters["@EarningType_IsDefault"].Value = EarningType_IsDefault;
                cmd.Parameters["@EarningType_DublicateInIB"].Value = EarningType_DublicateInIB;
                cmd.Parameters["@EarningType_Id"].Value = EarningType_Id;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    EarningType_Guid = (System.Guid)cmd.Parameters["@EarningType_Guid"].Value;
                    // ������������ ����������
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

                strErr += ("�� ������� ������� ������ \"��� ������\". ����� ������: " + f.Message);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }

        #endregion

        #region ������������� ������ � ���� ������
        /// <summary>
        /// �������������� ������ � ���� ������
        /// </summary>
        /// <param name="EarningType_Guid">�� ������</param>
        /// <param name="EarningType_Name">������������ ���� ������</param>
        /// <param name="EarningType_Description">����������</param>
        /// <param name="EarningType_IsActive">������� "������ �������"</param>
        /// <param name="EarningType_IsDefault">������� "������������ �� ���������"</param>
        /// <param name="EarningType_DublicateInIB">������� "����������� ������ � "��������""</param>
        /// <param name="EarningType_Id">��� ���� ������</param>
        /// <param name="objProfile">�������</param>
        /// <param name="strErr">��������� �� ������</param>
        /// <returns>true - ������� ���������� ��������; false - ������</returns>
        public static System.Boolean EditObjectInDataBase(System.Guid EarningType_Guid, 
            System.String EarningType_Name,  System.String EarningType_Description, System.Boolean EarningType_IsActive,
            System.Boolean EarningType_IsDefault, System.Boolean EarningType_DublicateInIB, System.Int32 EarningType_Id,  
            UniXP.Common.CProfile objProfile, ref System.String strErr)
        {
            System.Boolean bRet = false;

            if (IsAllParametersValid(EarningType_Name, EarningType_Id, ref strErr) == false) { return bRet; }

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;

            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    strErr += ("�� ������� �������� ���������� � ����� ������.");
                    return bRet;
                }
                DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_EditEarningType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EarningType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EarningType_Name", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EarningType_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EarningType_IsActive", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EarningType_IsDefault", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EarningType_DublicateInIB", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EarningType_Id", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@EarningType_Guid"].Value = EarningType_Guid;
                cmd.Parameters["@EarningType_Name"].Value = EarningType_Name;
                cmd.Parameters["@EarningType_Description"].Value = EarningType_Description;
                cmd.Parameters["@EarningType_IsActive"].Value = EarningType_IsActive;
                cmd.Parameters["@EarningType_IsDefault"].Value = EarningType_IsDefault;
                cmd.Parameters["@EarningType_DublicateInIB"].Value = EarningType_DublicateInIB;
                cmd.Parameters["@EarningType_Id"].Value = EarningType_Id;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    // ������������ ����������
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

                strErr += ("�� ������� ������ ��������� � ������ \"��� ������\". ����� ������: " + f.Message);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        #endregion

        #region ������� ������ �� ���� ������
        /// <summary>
        /// ������� ������ �� ���� ������
        /// </summary>
        /// <param name="EarningType_Guid">�� ���� ������</param>
        /// <param name="objProfile">�������</param>
        /// <param name="strErr">��������� �� ������</param>
        /// <returns>true - ������ �������; false - ������</returns>
        public static System.Boolean RemoveObjectFromDataBase(System.Guid EarningType_Guid,
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
                    strErr += ("�� ������� �������� ���������� � ����� ������.");
                    return bRet;
                }
                DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_DeleteEarningType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EarningType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@EarningType_Guid"].Value = EarningType_Guid;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    // ������������ ����������
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

                strErr += ("�� ������� ������� ������ \"��� ������\". ����� ������: " + f.Message);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        #endregion

        #region ������ ��������
        /// <summary>
        /// ���������� ������ �������� "��� ������" �� ���� ������
        /// </summary>
        /// <param name="objProfile">�������</param>
        /// <param name="cmdSQL">SQL-�������</param>
        /// <param name="strErr">��������� �� ������</param>
        /// <returns>������ �������� "��� ������"</returns>
        public static List<CEarningType> GetEarningTypeList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmdSQL, ref System.String strErr)
        {
            List<CEarningType> objList = new List<CEarningType>();
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        strErr += ("\n�� ������� �������� ���������� � ����� ������.");
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

                cmd.CommandText = System.String.Format("[{0}].[dbo].[usp_GetEarningType]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add(new CEarningType()
                        {
                            ID = (System.Guid)rs["EarningType_Guid"],
                            Name = System.Convert.ToString(rs["EarningType_Name"]),
                            Description = ((rs["EarningType_Description"] == System.DBNull.Value) ? "" : System.Convert.ToString(rs["EarningType_Description"])),
                            IsActive = System.Convert.ToBoolean(rs["EarningType_IsActive"]),
                            IsDefault = System.Convert.ToBoolean(rs["EarningType_IsDefault"]),
                            IsDublicateInIB = System.Convert.ToBoolean(rs["EarningType_DublicateInIB"]),
                            EarningTypeId = System.Convert.ToInt32(rs["EarningType_Id"])
                        });
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
                strErr += ("\n�� ������� �������� ������ �������� \"��� ������\". ����� ������: " + f.Message);
            }
            return objList;
        }
        #endregion
    }

}
