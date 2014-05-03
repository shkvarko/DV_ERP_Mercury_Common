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
    public partial class frmGroupProperties : DevExpress.XtraEditors.XtraForm
    {
        #region Переменные, Свойства, Константы
        /// <summary>
        /// профайл
        /// </summary>
        private UniXP.Common.CProfile m_objProfile;
        /// <summary>
        /// Группа
        /// </summary>
        private CConditionGroup m_objGroup;
        /// <summary>
        /// Список типов групп
        /// </summary>
        private List<CConditionGroupType> m_objConditionGroupTypList;
        private System.Boolean m_bNewGroup;
        private System.Boolean m_bGroupStructureIsChanged;
        private System.Boolean m_bGroupPropertiesIsChanged;
        private const System.String m_strNewGroupName = "Группа";
        #endregion

        #region Конструктор
        public frmGroupProperties(UniXP.Common.CProfile objProfile)
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_bNewGroup = false;
            m_objGroup = null;
            m_objConditionGroupTypList = CConditionGroupType.GetConditionGroupTypeList(m_objProfile, null);
            cboxGroupType.Properties.Items.Clear();
            if (m_objConditionGroupTypList != null)
            {
                foreach (CConditionGroupType objGroupType in m_objConditionGroupTypList)
                {
                    cboxGroupType.Properties.Items.Add(objGroupType);
                }
            }
            m_bGroupPropertiesIsChanged = false;
            m_bGroupStructureIsChanged = false;
            DialogResult = DialogResult.None;
        }
        #endregion

        #region Новая группа
        /// <summary>
        /// Новая группа
        /// </summary>
        public void NewGroup()
        {
            try
            {
                m_objGroup = new CConditionGroup();
                m_objGroup.PermitObjectList = new List<CConditionObject>();
                m_objGroup.ProhibitObjectList = new List<CConditionObject>();

                txtName.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.EditValueChanging);
                cboxGroupType.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.EditValueChanging);
                txtDscrpn.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.EditValueChanging);

                txtName.Text = m_strNewGroupName;
                if (cboxGroupType.Properties.Items.Count > 0)
                {
                    cboxGroupType.SelectedItem = cboxGroupType.Properties.Items[0];
                }
                txtDscrpn.Text = "";
                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка создания группы.\n\nТекст ошибки : " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                txtName.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.EditValueChanging);
                cboxGroupType.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.EditValueChanging);
                txtDscrpn.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.EditValueChanging);

                btnDelete.Enabled = false;
                btnDeleteExpn.Enabled = false;
                SetStructureModified(false);

                m_bNewGroup = true;
                ShowDialog();
                this.Cursor = Cursors.Default;
            }
            return;
        }
        #endregion

        #region Индикация изменений
        private void SetPropertiesModified(System.Boolean bModified)
        {
            m_bGroupPropertiesIsChanged = bModified;
            btnSave.Enabled = ( bModified || m_bGroupStructureIsChanged );
            btnCancel.Enabled = btnSave.Enabled;
        }
        private void SetStructureModified(System.Boolean bModified)
        {
            m_bGroupStructureIsChanged = bModified;
            btnSave.Enabled = (bModified || m_bGroupPropertiesIsChanged);
            btnCancel.Enabled = btnSave.Enabled;
        }
        private void EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка изменения значения.\n\nТекст ошибки : " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
        private void treeList_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            try
            {
                if ((e.Node != null) && (e.Node.Tag != null))
                {
                    btnDelete.Enabled = true;
                }
                else
                {
                    btnDelete.Enabled = false;
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка смены записи.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        private void treeListExpn_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            try
            {
                if ((e.Node != null) && (e.Node.Tag != null))
                {
                    btnDeleteExpn.Enabled = true;
                }
                else
                {
                    btnDeleteExpn.Enabled = false;
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка смены записи.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        #endregion

        #region Редактировать группу
        /// <summary>
        /// Редактирование группы
        /// </summary>
        /// <param name="objGroup">редактируемая группа</param>
        public void EditGroup( CConditionGroup objGroup )
        {
            try
            {
                m_objGroup = objGroup;

                txtName.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.EditValueChanging);
                cboxGroupType.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.EditValueChanging);
                txtDscrpn.EditValueChanging -= new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.EditValueChanging);

                txtName.Text = m_objGroup.Name;
                txtDscrpn.Text = m_objGroup.Description;
                if (cboxGroupType.Properties.Items.Count > 0)
                {
                    for( System.Int32 i=0; i< cboxGroupType.Properties.Items.Count; i++ )
                    {
                         if (((CConditionGroupType)cboxGroupType.Properties.Items[i]).Name == m_objGroup.GroupType.Name)
                        {
                            cboxGroupType.SelectedItem = cboxGroupType.Properties.Items[i];
                            break;
                        }
                    }
                }

                treeList.Nodes.Clear();
                treeListExpn.Nodes.Clear();

                if (m_objGroup.LoadGroupStructure(m_objProfile, null) == true)
                {
                    foreach (CConditionObject objObject in m_objGroup.PermitObjectList)
                    {
                        DevExpress.XtraTreeList.Nodes.TreeListNode objNode =
                                treeList.AppendNode(new object[] { objObject.Name }, null);
                        objNode.Tag = objObject;
                    }
                    foreach(CConditionObject objObjectExpn in m_objGroup.ProhibitObjectList)
                    {
                        DevExpress.XtraTreeList.Nodes.TreeListNode objNodeExpn =
                                treeListExpn.AppendNode(new object[] { objObjectExpn.Name }, null);
                        objNodeExpn.Tag = objObjectExpn;
                    }
                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка редактирования свойств группы.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                txtName.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.EditValueChanging);
                cboxGroupType.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.EditValueChanging);
                txtDscrpn.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.EditValueChanging);

                btnDelete.Enabled = false;
                btnDeleteExpn.Enabled = false;

                treeList.FocusedNode = null;
                treeListExpn.FocusedNode = null;

                SetPropertiesModified(false);
                SetStructureModified(false);
                DialogResult = DialogResult.None;

                ShowDialog();
                this.Cursor = Cursors.Default;
            }
            return;
        }
        #endregion

        #region Изменить состав группы
        /// <summary>
        /// Добавить элемент в группу
        /// </summary>
        /// <param name="bExpn">добавить в группу исключений</param>
        private void AddItemToGroup( System.Boolean bExpn )
        {
            frmAddMemberToGroup objFrmAddMemberToGroup = new frmAddMemberToGroup(m_objProfile);
            try
            {
                objFrmAddMemberToGroup.OpenObjectsList();

                if ((objFrmAddMemberToGroup.DialogResult == DialogResult.OK) &&
                    (objFrmAddMemberToGroup.SelectedObjectsList.Count > 0))
                {
                    System.Boolean bFind = false;
                    CConditionObject objInNode = null;
                    foreach (CConditionObject objItem in objFrmAddMemberToGroup.SelectedObjectsList)
                    {
                        bFind = false;
                        objInNode = null;
                        if (bExpn == true)
                        {
                            if (objItem.ID.CompareTo(System.Guid.Empty) == 0)
                            {
                                // была выбрана строка "Все"
                                // удаляем все объекты заданного типа и оставляем только строку "Все"
                                System.Int32 iNodeCount = treeListExpn.Nodes.Count;
                                for (System.Int32 i = (iNodeCount - 1); i >= 0; i--)
                                {
                                    objInNode = (CConditionObject)treeListExpn.Nodes[i].Tag;
                                    if ((objInNode != null) && (objInNode.ObjectType.ID.CompareTo(objItem.ObjectType.ID) == 0))
                                    {
                                        treeListExpn.Nodes.RemoveAt(i);
                                    }
                                }
                                DevExpress.XtraTreeList.Nodes.TreeListNode objNodeAll =
                                        treeListExpn.AppendNode(new object[] { objItem.Name }, null);
                                objNodeAll.Tag = objItem;
                                SetStructureModified(true);
                                break;
                            }
                            else
                            {
                                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListExpn.Nodes)
                                {
                                    objInNode = (CConditionObject)objNode.Tag;
                                    if ((objInNode != null) && (objInNode.ObjectType.ID.CompareTo(objItem.ObjectType.ID) == 0) &&
                                        (objInNode.ID.CompareTo(objItem.ID) == 0))
                                    {
                                        bFind = true;
                                        break;
                                    }
                                }
                                if (bFind == false)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode =
                                            treeListExpn.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                    SetStructureModified(true);
                                }
                            }
                        }
                        else
                        {
                            // в том случае, если выбрана строка "Все", то нужно удалить все объекты заданного типа и выйти из цикла
                            if (objItem.ID.CompareTo(System.Guid.Empty) == 0)
                            {
                                // была выбрана строка "Все"
                                // удаляем все объекты заданного типа и оставляем только строку "Все"
                                System.Int32 iNodeCount = treeList.Nodes.Count;
                                for (System.Int32 i = ( iNodeCount - 1 ); i >= 0; i--)
                                {
                                    objInNode = (CConditionObject)treeList.Nodes[i].Tag;
                                    if ((objInNode != null) && (objInNode.ObjectType.ID.CompareTo(objItem.ObjectType.ID) == 0))
                                    {
                                        treeList.Nodes.RemoveAt(i);
                                    }
                                }
                                DevExpress.XtraTreeList.Nodes.TreeListNode objNodeAll =
                                        treeList.AppendNode(new object[] { objItem.Name }, null);
                                objNodeAll.Tag = objItem;
                                SetStructureModified(true);
                                break;
                            }
                            else
                            {
                                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode2 in treeList.Nodes)
                                {
                                    objInNode = (CConditionObject)objNode2.Tag;
                                    if ((objInNode != null) && (objInNode.ObjectType.ID.CompareTo(objItem.ObjectType.ID) == 0) &&
                                        (objInNode.ID.CompareTo(objItem.ID) == 0))
                                    {
                                        bFind = true;
                                        break;
                                    }
                                }
                                if (bFind == false)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode2 =
                                            treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode2.Tag = objItem;
                                    SetStructureModified(true);
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка добавления объекта в группу.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                objFrmAddMemberToGroup.Dispose();
                objFrmAddMemberToGroup = null;
            }
            return ;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                AddItemToGroup(false);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка добавления элемента в группу.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void btnAddExpn_Click(object sender, EventArgs e)
        {
            try
            {
                AddItemToGroup(true);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка добавления элемента в группу.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// Удалить элемент из группы
        /// </summary>
        /// <param name="bExpn">удалить из группы исключений</param>
        private void DeleteItemFromGroup(System.Boolean bExpn)
        {
            try
            {
                if (bExpn == true)
                {
                    // исключения
                    if (treeListExpn.FocusedNode != null)
                    {
                        treeListExpn.Nodes.Remove(treeListExpn.FocusedNode);
                    }
                }
                else
                {
                    if (treeList.FocusedNode != null)
                    {
                        treeList.Nodes.Remove(treeList.FocusedNode);
                    }
                }
                SetStructureModified(true);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка удаления элемента из группы.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteItemFromGroup(false);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка удаления элемента из группы.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        private void btnDeleteExpn_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteItemFromGroup(true);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка удаления элемента из группы.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        #endregion

        #region Сохранить изменения
        /// <summary>
        /// Сохраняет изменения в свойствах и составе группы в базе данных
        /// </summary>
        /// <returns>true - успешное завершение; false - ошибка</returns>
        private System.Boolean SavePropertiesInDB()
        {
            System.Boolean bRet = false;
            try
            {
                if( ( m_bGroupPropertiesIsChanged == false ) && ( m_bGroupStructureIsChanged == false ) )
                {
                    bRet = true;
                }
                else
                {
                    if (m_objGroup == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Ошибка сохранения изменений в базе данных.\nНе удалось создать объект группы.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return bRet;
                    }
                    m_objGroup.Name = txtName.Text;
                    m_objGroup.Description = txtDscrpn.Text;
                    m_objGroup.GroupType = (CConditionGroupType)cboxGroupType.SelectedItem;
                    if (m_objGroup.GroupType == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Ошибка сохранения изменений в базе данных.\nНе указан тип группы.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return bRet;
                    }
                    // нужно определиться со списком членов группы и списком исключений
                    m_objGroup.PermitObjectList.Clear();
                    foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNodePermit in treeList.Nodes)
                    {
                        m_objGroup.PermitObjectList.Add((CConditionObject)objNodePermit.Tag);
                    }
                    m_objGroup.ProhibitObjectList.Clear();
                    foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNodeProhibit in treeListExpn.Nodes)
                    {
                        m_objGroup.ProhibitObjectList.Add((CConditionObject)objNodeProhibit.Tag);
                    }
                    // проверка значений закончена, теперь попробуем сохранить это все в БД
                    if (m_bNewGroup == true)
                    {
                        // новая группа
                        bRet = m_objGroup.AddGroupToDB(m_objProfile);
                    }
                    else
                    {
                        // ранее созданная группа
                        if( ( m_bGroupPropertiesIsChanged == true ) && ( m_bGroupStructureIsChanged == true ) )
                        {
                            // изменились свойства и список членов группы
                            bRet = m_objGroup.EditGroupInDB( m_objProfile, true );
                        }
                        if ((m_bGroupPropertiesIsChanged == true) && (m_bGroupStructureIsChanged == false))
                        {
                            // изменились только свойства
                            bRet = m_objGroup.EditGroupInDB(m_objProfile, false);
                        }
                        if ((m_bGroupPropertiesIsChanged == false) && (m_bGroupStructureIsChanged == true))
                        {
                            // изменился только список членов группы
                            bRet = m_objGroup.SaveGroupItems(m_objProfile);
                        }
                    }

                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка сохранения изменений в базе данных.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return bRet;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (SavePropertiesInDB() == true)
                {
                    DialogResult = DialogResult.OK;
                    this.FormClosing -= new FormClosingEventHandler(this.frmGroupProperties_FormClosing);
                    Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка сохранения изменений.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        #endregion

        #region Отменить изменения
        /// <summary>
        /// Отмена внесенных изменений
        /// </summary>
        private void CancelChanges()
        {
            try
            {
                if (m_bGroupPropertiesIsChanged || m_bGroupStructureIsChanged)
                {
                    DialogResult = DialogResult.Cancel;
                }
                this.FormClosing -= new FormClosingEventHandler(this.frmGroupProperties_FormClosing);
                Close();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка отмены изменений.\n\nТекст ошибки: " + f.Message, "Внимание",
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
                CancelChanges();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка отмены изменений.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        #endregion

        #region Закрытие формы
        private void frmGroupProperties_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (m_bGroupPropertiesIsChanged || m_bGroupStructureIsChanged)
                {
                    if (DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Свойства группы были изменены.\nВыйти из формы без сохранения изменений?", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.YesNo,
                        System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                    // запускаем процесс сохранения
                    {
                        e.Cancel = true;
                        return;
                    }
                    else
                    {
                        DialogResult = DialogResult.Cancel;
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("frmGroupProperties_FormClosing\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        #endregion

    }
}
