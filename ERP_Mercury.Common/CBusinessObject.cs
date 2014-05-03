using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace ERP_Mercury.Common
{
    class BooleanTypeConverter : BooleanConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context,
          System.Globalization.CultureInfo culture,
          object value,
          Type destType)
        {
            return (bool)value ?
              "��" : "���";
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
          System.Globalization.CultureInfo culture,
          object value)
        {
            return (string)value == "��";
        }
    }

    public class CBusinessObject
    {
        /// <summary>
        /// ���������� �������������
        /// </summary>
        private System.Guid m_uuidID;
        /// <summary>
        /// ���������� �������������
        /// </summary>
        [DisplayName( "���������� �������������" )]
        [Description( "���������� ������������� �������" )]
        [ReadOnly( true )]
        [Category( "1. ������������ ��������" )]
        [PropertyOrder(1)]
        [Browsable(false)]
//        [BrowsableAttribute(false)]
        public System.Guid ID
        {
            get { return m_uuidID; }
            set { m_uuidID = value; }
        }
        /// <summary>
        /// ���
        /// </summary>
        private System.String m_strName;
        /// <summary>
        /// ���
        /// </summary>
        [DisplayName( "������������" )]
        [Description( "������������ �������" )]
        [Category( "1. ������������ ��������" )]
        [PropertyOrder(2)]
        public System.String Name
        {
            get { return m_strName; }
            set { m_strName = value; }
        }

        public CBusinessObject()
        {
            m_uuidID = System.Guid.Empty;
            m_strName = "";
        }

        public CBusinessObject(System.Guid uuidID, System.String strName)
        {
            m_uuidID = uuidID;
            m_strName = strName;
        }
        /// <summary>
        /// �������� ������� ����� �����������
        /// </summary>
        /// <returns>true - ������ ���; false - ������</returns>
        public virtual System.Boolean IsAllParametersValid()
        {
            System.Boolean bRet = false;
            try
            {
                if (this.Name == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "���������� ������� ��������!", "��������",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }

                bRet = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "������ �������� �������.\n\n����� ������: " + f.Message, "��������",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return bRet;
        }
        /// <summary>
        /// �������� ������ � ��
        /// </summary>
        /// <param name="objProfile">�������</param>
        /// <returns>true - ������� ����������; false - ������</returns>
        public virtual System.Boolean Add(UniXP.Common.CProfile objProfile)
        {
            return false;
        }

        /// <summary>
        /// ������� ������ �� ��
        /// </summary>
        /// <param name="objProfile">�������</param>
        /// <param name="uuidID">���������� ������������� �������</param>
        /// <returns>true - ������� ����������; false - ������</returns>
        public virtual System.Boolean Remove(UniXP.Common.CProfile objProfile)
        {
            return false;
        }

        /// <summary>
        /// ��������� ��������� � ��
        /// </summary>
        /// <param name="objProfile">�������</param>
        /// <returns>true - ������� ����������; false - ������</returns>
        public virtual System.Boolean Update(UniXP.Common.CProfile objProfile)
        {
            return false;
        }

    }
}
