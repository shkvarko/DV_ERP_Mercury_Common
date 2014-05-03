using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ERP_Mercury.Common
{
    public partial class frmAddMemberToGroup : DevExpress.XtraEditors.XtraForm
    {
        #region Переменные, Свойства, Константы
        /// <summary>
        /// профайл
        /// </summary>
        private UniXP.Common.CProfile m_objProfile;
        /// <summary>
        /// Список выбранных объектов
        /// </summary>
        private List<CConditionObject> m_objSelectedObjectsList;
        /// <summary>
        /// Список выбранных объектов
        /// </summary>
        public List<CConditionObject> SelectedObjectsList
        {
            get { return m_objSelectedObjectsList; }
        }
        private const System.String m_strAllObjectNamePrfx = "Все ";
        #endregion

        #region Конструктор
        public frmAddMemberToGroup(UniXP.Common.CProfile objProfile)
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_objSelectedObjectsList = new List<CConditionObject>();
        }
        #endregion

        #region Выбрать объект
        private void checklboxObjectType_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            try
            {
                btnFind.Enabled = (checklboxObjectType.CheckedItems.Count > 0);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "checklboxObjectType_SelectedIndexChanged.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        private void checklboxObjectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                btnFind.Enabled = (checklboxObjectType.CheckedItems.Count > 0);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "checklboxObjectType_SelectedIndexChanged.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        private void checklboxObjectType_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                btnFind.Enabled = (checklboxObjectType.CheckedItems.Count > 0);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "checklboxObjectType_SelectedValueChanged.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        /// <summary>
        /// Открывает форму
        /// </summary>
        public void OpenObjectsList()
        {
            try
            {
                checklboxObjectType.Items.Clear();
                lstboxObjects.Items.Clear();
                m_objSelectedObjectsList.Clear();
                List<CConditionObjectType> objConditionObjectTypeList = CConditionObjectType.GetConditionObjectTypeList(m_objProfile, null);
                // заполним список типами объектов
                foreach (CConditionObjectType objObjectType in objConditionObjectTypeList)
                {
                    checklboxObjectType.Items.Add( objObjectType, false );
                }
                objConditionObjectTypeList = null;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка поиска объектов для группы.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                ShowDialog();
                Cursor = Cursors.Default;
            }

            return ;
        }
        /// <summary>
        /// Обновляет список объектов для выбранных типов
        /// </summary>
        private void RefreshObjectListForSelectedType()
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                lstboxObjects.Items.Clear();

                if (checklboxObjectType.CheckedItems.Count == 0) { return; }

                for (System.Int32 i = 0; i < checklboxObjectType.CheckedItems.Count; i++)
                {
                    CConditionObjectType objType = (CConditionObjectType)checklboxObjectType.CheckedItems[i];
                    if (objType != null)
                    {
                        // запрашиваем для данного типа список возможных объектов
                        List<CConditionObject> objObjectsForThisTypeList = CConditionObject.GetAllObjectsForObjectType(m_objProfile, objType);
                        if (objObjectsForThisTypeList != null)
                        {
                            //lstboxObjects.Items.Add(new CConditionObject(System.Guid.Empty, m_strAllObjectNamePrfx + objType.Name, objType));
                            // каждый элемент списка добавляем в lstboxObjects
                            foreach (CConditionObject objItem in objObjectsForThisTypeList)
                            {
                                lstboxObjects.Items.Add( objItem, CheckState.Unchecked );
                            }
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка поиска объектов для группы.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                lstboxObjects.UnSelectAll();
                Cursor = Cursors.Default;
            }

            return;
        }
        private void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                if (checklboxObjectType.CheckedItems.Count > 0)
                {
                    RefreshObjectListForSelectedType();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка поиска объектов для группы.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        #endregion

        #region Подтвердить выбор
        /// <summary>
        /// Формирует список выбранных объектов
        /// </summary>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        private System.Boolean ApplySelectedObjects()
        {
            System.Boolean bRet = false;
            try
            {
                m_objSelectedObjectsList.Clear();
                if ((lstboxObjects.CheckedItems.Count > 0) &&
                    (((CConditionObject)lstboxObjects.CheckedItems[0]).ID.CompareTo(System.Guid.Empty) == 0))
                {
                    //выбрана строка "Все"
                    m_objSelectedObjectsList.Add((CConditionObject)lstboxObjects.CheckedItems[0]);
                    lstboxObjects.UnSelectAll();
                    lstboxObjects.SelectedItem = lstboxObjects.Items[0];
                }
                else
                {
                    for (System.Int32 i = 0; i < lstboxObjects.CheckedItems.Count; i++)
                    {
                        m_objSelectedObjectsList.Add((CConditionObject)lstboxObjects.CheckedItems[i]);
                    }
                }
                bRet = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка формирования списка выбранных объектов.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return bRet;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (ApplySelectedObjects() == true)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Текст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                m_objSelectedObjectsList.Clear();
                DialogResult = DialogResult.Cancel;
                Close();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Текст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        #endregion

        private void FindObject(System.String strSearchText, System.Boolean bStartFromBegin)
        {
            try
            {
                if( (strSearchText.Length > 0) && (lstboxObjects.Items.Count > 0 ) )
                {
                    if (bStartFromBegin == true)
                    {
                        for (System.Int32 i = 0; i < lstboxObjects.Items.Count; i++)
                        {
                            if (((CConditionObject)lstboxObjects.Items[i].Value).Name.ToUpper().IndexOf(strSearchText.ToUpper(), 0) >= 0)
                            {
                                lstboxObjects.SelectedItem = lstboxObjects.Items[i];
                                break;
                            }
                        }
                    }
                    else
                    {
                        System.Int32 iSelectedIndex = lstboxObjects.SelectedIndex;
                        if (iSelectedIndex < 0) { iSelectedIndex = 0; }
                        else if (iSelectedIndex < (lstboxObjects.Items.Count - 1)) { iSelectedIndex++; }
                        for (System.Int32 i = iSelectedIndex; i < lstboxObjects.Items.Count; i++)
                        {
                            if (((CConditionObject)lstboxObjects.Items[i].Value).Name.IndexOf(strSearchText, 0) >= 0)
                            {
                                lstboxObjects.SelectedItem = lstboxObjects.Items[i];
                                break;
                            }
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "FindObject.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return ;
        }

        private void txtFindObject_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                if ((lstboxObjects.Enabled == true) && (lstboxObjects.ItemCount > 0))
                {
                    if ( e.NewValue != null )
                    {
                        System.String objFindItem = (System.String)e.NewValue;
                        FindObject(objFindItem, true);
                        //if ( objFindItem.Length > 0 )
                        //{
                        //    for (System.Int32 i = 0; i < lstboxObjects.Items.Count; i++)
                        //    {
                        //        if ( ( (  CConditionObject)lstboxObjects.Items[i].Value  ).Name.IndexOf(objFindItem, 0) >= 0)
                        //        {
                        //            lstboxObjects.SelectedItem = lstboxObjects.Items[i];
                        //            break;
                        //        }
                        //    }
                        //}
                    }
                }
            }
            catch
            {
            }
        }

        private void txtFindObject_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void txtFindObject_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F3)
                {
                    FindObject(txtFindObject.Text, false);
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "txtFindObject_KeyDown.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;

        }
    }
}
