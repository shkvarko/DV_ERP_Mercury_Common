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
              "Да" : "Нет";
        }

        public override object ConvertFrom(ITypeDescriptorContext context,
          System.Globalization.CultureInfo culture,
          object value)
        {
            return (string)value == "Да";
        }
    }

    public class CBusinessObject
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        private System.Guid m_uuidID;
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        [DisplayName( "Уникальный идентификатор" )]
        [Description( "Уникальный идентификатор объекта" )]
        [ReadOnly( true )]
        [Category( "1. Обязательные значения" )]
        [PropertyOrder(1)]
        [Browsable(false)]
//        [BrowsableAttribute(false)]
        public System.Guid ID
        {
            get { return m_uuidID; }
            set { m_uuidID = value; }
        }
        /// <summary>
        /// Имя
        /// </summary>
        private System.String m_strName;
        /// <summary>
        /// Имя
        /// </summary>
        [DisplayName( "Наименование" )]
        [Description( "Наименование объекта" )]
        [Category( "1. Обязательные значения" )]
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
        /// Проверка свойств перед сохранением
        /// </summary>
        /// <returns>true - ошибок нет; false - ошибка</returns>
        public virtual System.Boolean IsAllParametersValid()
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
        /// <summary>
        /// Добавить запись в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public virtual System.Boolean Add(UniXP.Common.CProfile objProfile)
        {
            return false;
        }

        /// <summary>
        /// Удалить запись из БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="uuidID">уникальный идентификатор объекта</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public virtual System.Boolean Remove(UniXP.Common.CProfile objProfile)
        {
            return false;
        }

        /// <summary>
        /// Сохранить изменения в БД
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public virtual System.Boolean Update(UniXP.Common.CProfile objProfile)
        {
            return false;
        }

    }
}
