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
    public partial class frmFilterForPartsList : DevExpress.XtraEditors.XtraForm
    {
        #region Свойства
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;

        private List<CProductSubType> m_objProductSubTypeList;
        private List<CProductTradeMark> m_objProductTradeMarkList;
        private List<CProductType> m_objProductTypeList;
        /// <summary>
        /// Выбранный список подгрупп
        /// </summary>
        public List<CProductSubType> SelectedProductSubTypeList {get; set;}
        /// <summary>
        /// Выбранный список тованых марок
        /// </summary>
        public List<CProductTradeMark> SelectedProductTradeMarkList { get; set; }
        /// <summary>
        /// Выбранный список групп
        /// </summary>
        public List<CProductType> SelectedProductTypeList { get; set; }

        /// <summary>
        /// Проверка на установку фильтра
        /// </summary>
        public System.Boolean bFilterIsSet
        {
            get { return ((SelectedProductSubTypeList.Count > 0) || (SelectedProductTradeMarkList.Count > 0) || (SelectedProductTypeList.Count > 0)); }
        }
        private System.String m_strProductTradeMark = "Товарные марки: ";
        private System.String m_strProductType = "Товарные группы: ";
        private System.String m_strProductSubType = "Товарные подгруппы: ";
        #endregion

        public frmFilterForPartsList(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem,
            List<CProductSubType> objProductSubTypeList, List<CProductTradeMark> objProductTradeMarkList,
            List<CProductType> objProductTypeList)
        {
            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;

            m_objProductSubTypeList = objProductSubTypeList;
            m_objProductTradeMarkList = objProductTradeMarkList;
            m_objProductTypeList = objProductTypeList;

            SelectedProductSubTypeList = new List<CProductSubType>();
            SelectedProductTradeMarkList = new List<CProductTradeMark>();
            SelectedProductTypeList = new List<CProductType>();

            InitializeComponent();
            LoadSourceList();

        }

        private void LoadSourceList()
        {
            try
            {
                checkListPartSubType.Items.Clear();
                if (m_objProductSubTypeList != null)
                {
                    foreach (CProductSubType objItem in m_objProductSubTypeList)
                    {
                        checkListPartSubType.Items.Add(objItem, false);
                    }
                }
                checkListProductTradeMark.Items.Clear();
                if (m_objProductTradeMarkList != null)
                {
                    foreach (CProductTradeMark objItem in m_objProductTradeMarkList)
                    {
                        checkListProductTradeMark.Items.Add(objItem, false);
                    }
                }
                checkListProductType.Items.Clear();
                if ( m_objProductTypeList != null)
                {
                    foreach (CProductType objItem in m_objProductTypeList)
                    {
                        checkListProductType.Items.Add(objItem, false);
                    }
                }
                lblProductSubTypeFilter.Text = m_strProductSubType;
                lblProductTypeFilter.Text = m_strProductType;
                lblTradeMarkFilter.Text = m_strProductTradeMark;

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка загрузки списка товарных подгрупп.\n\nТекст ошибки: " + f.Message, "Ошибка",
                     System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                Close();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка btnCancel_Click.\n\nТекст ошибки: " + f.Message, "Ошибка",
                     System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void SetSelectedObjectList()
        {
            try
            {
                SelectedProductSubTypeList.Clear();
                System.Int32 iProductSubTypeCount = checkListPartSubType.Items.Count;

                for (System.Int32 i = 0; i < iProductSubTypeCount; i++)
                {
                    if (checkListPartSubType.Items[i].CheckState == CheckState.Checked)
                    {
                        SelectedProductSubTypeList.Add((CProductSubType)checkListPartSubType.Items[i].Value);
                    }
                }

                SelectedProductTypeList.Clear();
                System.Int32 iProductTypeCount = checkListProductType.Items.Count;

                for (System.Int32 i = 0; i < iProductTypeCount; i++)
                {
                    if (checkListProductType.Items[i].CheckState == CheckState.Checked)
                    {
                        SelectedProductTypeList.Add((CProductType)checkListProductType.Items[i].Value);
                    }
                }

                SelectedProductTradeMarkList.Clear();
                System.Int32 iProductTradeMarkCount = checkListProductTradeMark.Items.Count;

                for (System.Int32 i = 0; i < iProductTradeMarkCount; i++)
                {
                    if (checkListProductTradeMark.Items[i].CheckState == CheckState.Checked)
                    {
                        SelectedProductTradeMarkList.Add((CProductTradeMark)checkListProductTradeMark.Items[i].Value);
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка SetSelectedProductSubTypeList.\n\nТекст ошибки: " + f.Message, "Ошибка",
                     System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SetSelectedObjectList();
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка btnCancel_Click.\n\nТекст ошибки: " + f.Message, "Ошибка",
                     System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void UnCheckFilter()
        {
            try
            {
                System.Int32 iProductSubTypeCount = checkListPartSubType.Items.Count;
                for (System.Int32 i = 0; i < iProductSubTypeCount; i++)
                {
                    if (checkListPartSubType.Items[i].CheckState == CheckState.Checked)
                    {
                        checkListPartSubType.Items[i].CheckState = CheckState.Unchecked;
                    }
                }
                lstBoxSelectedSubType.Items.Clear();

                System.Int32 iProductTypeCount = checkListProductType.Items.Count;
                for (System.Int32 i = 0; i < iProductTypeCount; i++)
                {
                    if (checkListProductType.Items[i].CheckState == CheckState.Checked)
                    {
                        checkListProductType.Items[i].CheckState = CheckState.Unchecked;
                    }
                }
                lstBoxSelectedProductType.Items.Clear();

                System.Int32 iProductTradeMarkCount = checkListProductTradeMark.Items.Count;
                for (System.Int32 i = 0; i < iProductTradeMarkCount; i++)
                {
                    if (checkListProductTradeMark.Items[i].CheckState == CheckState.Checked)
                    {
                        checkListProductTradeMark.Items[i].CheckState = CheckState.Unchecked;
                    }
                }
                lstBoxSelectedProductTradeMark.Items.Clear();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка UnCheckFilter.\n\nТекст ошибки: " + f.Message, "Ошибка",
                     System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void btnCancelFilter_Click(object sender, EventArgs e)
        {
            try
            {
                UnCheckFilter();
                SelectedProductSubTypeList.Clear();
                SelectedProductTradeMarkList.Clear();
                SelectedProductTypeList.Clear();

                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка btnCancelFilter_Click.\n\nТекст ошибки: " + f.Message, "Ошибка",
                     System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void txtFindObject_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                if (sender == txtFindObject)
                {
                    if ((checkListPartSubType.Enabled == true) && (checkListPartSubType.ItemCount > 0))
                    {
                        if (e.NewValue != null)
                        {
                            System.String objFindItem = (System.String)e.NewValue;
                            if (objFindItem.Length > 0)
                            {
                                for (System.Int32 i = 0; i < checkListPartSubType.Items.Count; i++)
                                {
                                    if (((CProductSubType)checkListPartSubType.Items[i].Value).Name.IndexOf(objFindItem, 0) >= 0)
                                    {
                                        checkListPartSubType.SelectedItem = checkListPartSubType.Items[i];
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                if (sender == txtFindObjectProductType)
                {
                    if (( checkListProductType.Enabled == true) && (checkListProductType.ItemCount > 0))
                    {
                        if (e.NewValue != null)
                        {
                            System.String objFindItem = (System.String)e.NewValue;
                            if (objFindItem.Length > 0)
                            {
                                for (System.Int32 i = 0; i < checkListProductType.Items.Count; i++)
                                {
                                    if (((CProductType)checkListProductType.Items[i].Value).Name.IndexOf(objFindItem, 0) >= 0)
                                    {
                                        checkListProductType.SelectedItem = checkListProductType.Items[i];
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                if (sender == txtFindObjectTradeMark)
                {
                    if ((checkListProductTradeMark.Enabled == true) && (checkListProductTradeMark.ItemCount > 0))
                    {
                        if (e.NewValue != null)
                        {
                            System.String objFindItem = (System.String)e.NewValue;
                            if (objFindItem.Length > 0)
                            {
                                for (System.Int32 i = 0; i < checkListProductTradeMark.Items.Count; i++)
                                {
                                    if (((CProductTradeMark)checkListProductTradeMark.Items[i].Value).Name.IndexOf(objFindItem, 0) >= 0)
                                    {
                                        checkListProductTradeMark.SelectedItem = checkListProductTradeMark.Items[i];
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void checkListPartSubType_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            try
            {
                if (sender == checkListPartSubType)
                {
                    CProductSubType objSelectedSubType = (CProductSubType)checkListPartSubType.Items[e.Index].Value;
                    System.Boolean bExistsInSelected = false;
                    System.Int32 iExistsIndex = -1;
                    for (System.Int32 i = 0; i < lstBoxSelectedSubType.Items.Count; i++)
                    {
                        if (((CProductSubType)lstBoxSelectedSubType.Items[i]).ID.CompareTo(objSelectedSubType.ID) == 0)
                        {
                            bExistsInSelected = true;
                            iExistsIndex = i;
                            break;
                        }
                    }

                    if ((e.State == CheckState.Checked) && (bExistsInSelected == false))
                    {
                        lstBoxSelectedSubType.Items.Add(objSelectedSubType);
                    }
                    else if ((e.State == CheckState.Unchecked) && (bExistsInSelected == true))
                    {
                        lstBoxSelectedSubType.Items.RemoveAt(iExistsIndex);
                    }
                }

                if (sender == checkListProductTradeMark)
                {
                    CProductTradeMark objSelectedProductTradeMark = (CProductTradeMark)checkListProductTradeMark.Items[e.Index].Value;
                    System.Boolean bExistsInSelected = false;
                    System.Int32 iExistsIndex = -1;
                    for (System.Int32 i = 0; i < lstBoxSelectedProductTradeMark.Items.Count; i++)
                    {
                        if (((CProductTradeMark)lstBoxSelectedProductTradeMark.Items[i]).ID.CompareTo(objSelectedProductTradeMark.ID) == 0)
                        {
                            bExistsInSelected = true;
                            iExistsIndex = i;
                            break;
                        }
                    }

                    if ((e.State == CheckState.Checked) && (bExistsInSelected == false))
                    {
                        lstBoxSelectedProductTradeMark.Items.Add(objSelectedProductTradeMark);
                    }
                    else if ((e.State == CheckState.Unchecked) && (bExistsInSelected == true))
                    {
                        lstBoxSelectedProductTradeMark.Items.RemoveAt(iExistsIndex);
                    }
                }

                if (sender == checkListProductType)
                {
                    CProductType objSelectedProductType = (CProductType)checkListProductType.Items[e.Index].Value;
                    System.Boolean bExistsInSelected = false;
                    System.Int32 iExistsIndex = -1;
                    for (System.Int32 i = 0; i < lstBoxSelectedProductType.Items.Count; i++)
                    {
                        if (((CProductType)lstBoxSelectedProductType.Items[i]).ID.CompareTo(objSelectedProductType.ID) == 0)
                        {
                            bExistsInSelected = true;
                            iExistsIndex = i;
                            break;
                        }
                    }

                    if ((e.State == CheckState.Checked) && (bExistsInSelected == false))
                    {
                        lstBoxSelectedProductType.Items.Add(objSelectedProductType);
                    }
                    else if ((e.State == CheckState.Unchecked) && (bExistsInSelected == true))
                    {
                        lstBoxSelectedProductType.Items.RemoveAt(iExistsIndex);
                    }
                }

                // итоговые условия фильтрации
                lblProductSubTypeFilter.Text = m_strProductSubType;
                lblProductTypeFilter.Text = m_strProductType;
                lblTradeMarkFilter.Text = m_strProductTradeMark;

                System.Int32 iSelectedProductTradeMarkCount = lstBoxSelectedProductTradeMark.Items.Count;
                System.Int32 iSelectedProductTypeCount = lstBoxSelectedProductType.Items.Count;
                System.Int32 iSelectedProductSubTypeCount = lstBoxSelectedSubType.Items.Count;

                foreach (System.Object objItem in lstBoxSelectedProductTradeMark.Items)
                {
                    lblTradeMarkFilter.Text += (" " + ((CProductTradeMark)objItem).Name);
                    if (iSelectedProductTradeMarkCount > 1)
                        lblTradeMarkFilter.Text += ",";

                    iSelectedProductTradeMarkCount--;
                }

                foreach (System.Object objItem in lstBoxSelectedProductType.Items)
                {
                    lblProductTypeFilter.Text += (" " + ((CProductType)objItem).Name);
                    if (iSelectedProductTypeCount > 1)
                        lblProductTypeFilter.Text += ",";

                    iSelectedProductTypeCount--;
                }

                foreach (System.Object objItem in lstBoxSelectedSubType.Items)
                {
                    lblProductSubTypeFilter.Text += (" " + ((CProductSubType)objItem).Name);
                    if (iSelectedProductSubTypeCount > 1)
                        lblProductSubTypeFilter.Text += ",";

                    iSelectedProductSubTypeCount--;
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка checkListPartSubType_ItemCheck.\n\nТекст ошибки: " + f.Message, "Ошибка",
                     System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }


    }
}
