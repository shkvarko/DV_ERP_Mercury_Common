using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ERP_Mercury.Common
{
    public partial class ctrlCustomerChild : UserControl
    {
        #region Свойства, переменные
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;
        private CCustomer m_objSelectedCustomer;
        private System.Boolean m_bIsChanged;
//        private System.Boolean m_bDisableEvents;
        private System.Boolean m_bIsReadOnly;
        private List<CChildDepart> m_objChildDepartList;
        private const System.Int32 iPanelInfoHeight = 35;
        private const System.Int32 iPanelInfoMinHeight = 23;
        #endregion

        #region События
        // Создаем закрытое поле, ссылающееся на заголовок списка делегатов
        private EventHandler<ChangeCustomerChildListPropertieEventArgs> m_ChangeCustomerChildListProperties;
        // Создаем в классе член-событие
        public event EventHandler<ChangeCustomerChildListPropertieEventArgs> ChangeCustomerChildListProperties
        {
            add
            {
                // берем закрытую блокировку и добавляем обработчик
                // (передаваемый по значению) в список делегатов
                m_ChangeCustomerChildListProperties += value;
            }
            remove
            {
                // берем закрытую блокировку и удаляем обработчик
                // (передаваемый по значению) из списка делегатов
                m_ChangeCustomerChildListProperties -= value;
            }
        }
        /// <summary>
        /// Инициирует событие и уведомляет о нем зарегистрированные объекты
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnChangeCustomerChildListProperties(ChangeCustomerChildListPropertieEventArgs e)
        {
            // Сохраняем поле делегата во временном поле для обеспечение безопасности потока
            EventHandler<ChangeCustomerChildListPropertieEventArgs> temp = m_ChangeCustomerChildListProperties;
            // Если есть зарегистрированные объектв, уведомляем их
            if (temp != null) temp(this, e);
        }
        public void SimulateChangeCustomerChildListProperties(CCustomer objCustomer, enumActionSaveCancel enActionType)
        {
            // Создаем объект, хранящий информацию, которую нужно передать
            // объектам, получающим уведомление о событии
            ChangeCustomerChildListPropertieEventArgs e = new ChangeCustomerChildListPropertieEventArgs(objCustomer, enActionType);

            // Вызываем виртуальный метод, уведомляющий наш объект о возникновении события
            // Если нет типа, переопределяющего этот метод, наш объект уведомит все объекты, 
            // подписавшиеся на уведомление о событии
            OnChangeCustomerChildListProperties(e);
        }
        #endregion

        #region Конструктор
        public ctrlCustomerChild(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem)
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_bIsChanged = false;
            //m_bDisableEvents = false;

            m_objSelectedCustomer = null;
            m_objChildDepartList = null;
            CheckClientsRight();
        }

        /// <summary>
        /// Проверка динамических прав
        /// </summary>
        private void CheckClientsRight()
        {
            try
            {
                System.Boolean bEditCustomerChildList = m_objProfile.GetClientsRight().GetState(ERP_Mercury.Global.Consts.strDR_EditCustomerChildDepartLimit);
                if( bEditCustomerChildList == false )
                {
                    btnEdit.Visible = false;
                    btnPrint.Visible = false;
                    btnSave.Visible = false;
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("CheckClientsRight. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        #endregion

        #region Журнал сообщений
        private void SendMessageToLog(System.String strMessage)
        {
            try
            {
                m_objMenuItem.SimulateNewMessage(strMessage);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "SendMessageToLog.\n Текст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        #endregion

        #region Индикация изменений
        private void SetPropertiesModified(System.Boolean bModified)
        {
            try
            {
                m_bIsChanged = bModified;
                btnSave.Enabled = m_bIsChanged;
                btnEdit.Enabled = !(bModified);
                //btnCancel.Enabled = btnSave.Enabled;

                EnableCompanyBtns();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "SetPropertiesModified. Текст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;

        }
        #endregion

        #region Режим просмотра/редактирования
        /// <summary>
        /// Устанавливает режим просмотра/редактирования
        /// </summary>
        /// <param name="bSet">true - режим просмотра; false - режим редактирования</param>
        public void SetModeReadOnly(System.Boolean bSet)
        {
            try
            {
                treeList.OptionsBehavior.Editable = !bSet;
                treeList.Enabled = !bSet;
                listBoxChildDepart.Enabled = !bSet;

                m_bIsReadOnly = bSet;
                btnEdit.Enabled = bSet;
                btnSave.Enabled = !bSet;
                btnCancel.Enabled = true;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("SetModeReadOnly. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                SetModeReadOnly(false);
                btnEdit.Enabled = false;
                btnSave.Enabled = false;
                EnableCompanyBtns();
                SetPropertiesModified(true);

            }
            catch (System.Exception f)
            {
                SendMessageToLog("SetModeReadOnly. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        #endregion

        #region Редактирование списка подразделений

        private void LoadCustomerArjiveLimit()
        {
            if (m_objSelectedCustomer == null) { return; }
            try
            {
                this.tableLayoutPanelBgrnd.SuspendLayout();
                this.tableLayoutPaneltree.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();
                treeListArjive.Nodes.Clear();
                treeListArjive.DataSource = CCreditLimitArjive.GetCreditLimitArjiveList(m_objProfile, null, m_objSelectedCustomer, dtBeginDate.DateTime, dtEndDate.DateTime);

                this.tableLayoutPanelBgrnd.ResumeLayout(false);
                this.tableLayoutPaneltree.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка загрузки журнала кредитного лимита клиента. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;

        }

        /// <summary>
        /// Загружает дочерних клиентов
        /// </summary>
        /// <param name="objCustomer">клиент</param>
        public void LoadCustomerChildList(CCustomer objCustomer)
        {
            //m_bDisableEvents = true;
            if (objCustomer == null) { return; }
            try
            {
                m_objSelectedCustomer = objCustomer;
                lblCustomerIfo.Text = objCustomer.FullName;

                listBoxChildDepart.Items.Clear();
                treeList.Nodes.Clear();

                m_objChildDepartList = CChildDepart.GetChildDepartList( m_objProfile, null, m_objSelectedCustomer.ID);

                foreach (CChildDepart objChildDepart in m_objChildDepartList)
                {
                    treeList.AppendNode(new object[] { objChildDepart.Code, objChildDepart.Name, objChildDepart.MaxDelay, objChildDepart.MaxDebt }, null).Tag = objChildDepart;
                }

                listBoxChildDepart.DataSource = CChildDepart.GetChildDepartList(m_objProfile, null, System.Guid.Empty);

            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования кредитного лимита клиента. Текст ошибки: " + f.Message);
            }
            finally
            {
                //m_bDisableEvents = false;
                SetPropertiesModified(false);

                DisableCompanyBtns();
            }
            return;
        }
        private void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                LoadCustomerArjiveLimit();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка обновления журнала изменений. Текст ошибки: " + f.Message);
            }
            finally
            {
                //m_bDisableEvents = false;
            }
            return;
        }
        #endregion

        #region Добавить/Удалить дочернее подразделение
        /// <summary>
        /// Включает/Выключает кнопки добавления/удаления компаний в дерево
        /// </summary>
        private void EnableCompanyBtns()
        {
            try
            {
                btnAddCompany.Enabled = ( (listBoxChildDepart.SelectedItem != null) && (m_bIsReadOnly == false));
                btnAddAllCompany.Enabled = (m_bIsReadOnly == false);
                btnRemoveCompany.Enabled = ((treeList.FocusedNode != null) && (m_bIsReadOnly == false));
                btnRemoveAllCompany.Enabled = ((treeList.Nodes.Count > 0) && (m_bIsReadOnly == false));
            }
            catch (System.Exception f)
            {
                SendMessageToLog("EnableCompanyBtns. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void DisableCompanyBtns()
        {
            try
            {
                btnAddCompany.Enabled = false;
                btnAddAllCompany.Enabled = false;
                btnRemoveCompany.Enabled = false;
                btnRemoveAllCompany.Enabled = false;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("DisableCompanyBtns. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// Добавляет в дерево строку ля указанной компании
        /// </summary>
        /// <param name="objCompany">компания</param>
        private void AddCompany(CChildDepart objChildDepart)
        {
            try
            {
                if (objChildDepart == null) { return; }
                System.Boolean bCompanyExists = false;
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    if (((CChildDepart)objNode.Tag).ID.CompareTo(objChildDepart.ID) == 0)
                    {
                        bCompanyExists = true;
                        break;
                    }
                }
                if (bCompanyExists == false)
                {
                    treeList.AppendNode(new object[] { objChildDepart.Code, objChildDepart.Name, objChildDepart.MaxDelay, objChildDepart.MaxDebt }, null).Tag = objChildDepart;

                    System.Int32 iRemoveIndx = -1;
                    for (System.Int32 i = 0; i < listBoxChildDepart.Items.Count; i++)
                    {
                        if (((CChildDepart)listBoxChildDepart.GetItemValue(i)).ID.CompareTo(objChildDepart.ID) == 0)
                        {
                            iRemoveIndx = i;
                            break;
                        }
                    }
                    if (iRemoveIndx >= 0) { listBoxChildDepart.Items.RemoveAt(iRemoveIndx); }
                    SetPropertiesModified(true);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования списка подразделений. Текст ошибки: " + f.Message);
            }
            finally
            {
                EnableCompanyBtns();
            }
            return;
        }
        /// <summary>
        /// добавляет в дерево все компании
        /// </summary>
        private void AddAllCompany()
        {
            try
            {
                this.tableLayoutPanelBgrnd.SuspendLayout();
                this.tableLayoutPaneltree.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();
                System.Boolean bCompanyExists = false;
                CChildDepart objChildDepart = null;
                for (System.Int32 i = 0; i < listBoxChildDepart.Items.Count; i++)
                {
                    objChildDepart = (CChildDepart)listBoxChildDepart.GetItemValue(i);

                    foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes)
                    {
                        if (objNode.Tag == null) { continue; }

                        if (((CChildDepart)objNode.Tag).ID.CompareTo(objChildDepart.ID) == 0)
                        {
                            bCompanyExists = true;
                            break;
                        }
                    }

                    if (bCompanyExists == false)
                    {
                        treeList.AppendNode(new object[] { objChildDepart.Code, objChildDepart.Name, objChildDepart.MaxDelay, objChildDepart.MaxDebt }, null).Tag = objChildDepart; 
                    }
                }
                listBoxChildDepart.Items.Clear();
                SetPropertiesModified(true);

                this.tableLayoutPanelBgrnd.ResumeLayout(false);
                this.tableLayoutPaneltree.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования списка подразделений. Текст ошибки: " + f.Message);
            }
            finally
            {
                EnableCompanyBtns();
            }
            return;
        }
        /// <summary>
        /// Удаляет выбранный узел
        /// </summary>
        private void DeleteSelectedCompany()
        {
            try
            {
                if ((treeList.FocusedNode == null) || (treeList.FocusedNode.Tag == null)) { return; }

                CChildDepart objSelectedChildDepart = ((CChildDepart)treeList.FocusedNode.Tag);
                System.Boolean bCompanyExists = false;
                for (System.Int32 i = 0; i < listBoxChildDepart.Items.Count; i++)
                {
                    if (((CChildDepart)listBoxChildDepart.GetItemValue(i)).ID.CompareTo(objSelectedChildDepart.ID) == 0)
                    {
                        bCompanyExists = true;
                        break;
                    }
                }
                if (bCompanyExists == false)
                {
                    listBoxChildDepart.Items.Add(objSelectedChildDepart);
                }
                treeList.Nodes.Remove(treeList.FocusedNode);
                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования списка подразделений. Текст ошибки: " + f.Message);
            }
            finally
            {
                EnableCompanyBtns();
            }
            return;
        }
        /// <summary>
        /// Удаляет все узлы в дереве
        /// </summary>
        private void DeleteAllCompany()
        {
            try
            {
                this.tableLayoutPanelBgrnd.SuspendLayout();
                this.tableLayoutPaneltree.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();
                ((System.ComponentModel.ISupportInitialize)(this.listBoxChildDepart)).BeginInit();
                this.listBoxChildDepart.Items.Clear();

                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    listBoxChildDepart.Items.Add((CChildDepart)objNode.Tag);
                }
                treeList.Nodes.Clear();
                SetPropertiesModified(true);

                this.tableLayoutPanelBgrnd.ResumeLayout(false);
                this.tableLayoutPaneltree.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.listBoxChildDepart)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования списка подразделений. Текст ошибки: " + f.Message);
            }
            finally
            {
                EnableCompanyBtns();
            }
            return;
        }
        private void btnAddCompany_Click(object sender, EventArgs e)
        {
            try
            {
                AddCompany((CChildDepart)listBoxChildDepart.SelectedValue);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования списка подразделений. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private void listBoxCompany_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if ((listBoxChildDepart.Enabled == true) && (listBoxChildDepart.SelectedValue != null))
                { AddCompany((CChildDepart)listBoxChildDepart.SelectedValue); }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования списка подразделений. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void btnRemoveCompany_Click(object sender, EventArgs e)
        {
            try
            {
                if ((treeList.FocusedNode == null) || (treeList.FocusedNode.Tag == null)) { return; }
                DeleteSelectedCompany();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования списка подразделений. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private void btnAddAllCompany_Click(object sender, EventArgs e)
        {
            try
            {
                AddAllCompany();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования списка подразделений. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        private void btnRemoveAllCompany_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteAllCompany();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования списка подразделений. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        #endregion

        #region Сохранить изменения
        /// <summary>
        /// Сохраняет изменения в базе данных
        /// </summary>
        /// <returns>true - удачное завершение операции;false - ошибка</returns>
        private System.Boolean bSaveChanges()
        {
            System.Boolean bRet = false;
            List<CChildDepart> objCreditChildDepartListForSave = new List<CChildDepart>();
            try
            {
                CChildDepart objChildDepart = null;
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    objChildDepart = (CChildDepart)objNode.Tag;

                    objCreditChildDepartListForSave.Add(objChildDepart);
                }

                if (objCreditChildDepartListForSave != null)
                {
                    System.String strErr = "";
                    bRet = CChildDepart.SaveCustomerChildListInDB(objCreditChildDepartListForSave, m_objSelectedCustomer.ID,
                        m_objProfile, null, ref strErr);
                    if (bRet == false) { SendMessageToLog(strErr); }
                    else { m_objSelectedCustomer.ChildDepartList = objCreditChildDepartListForSave; }
                }

                objCreditChildDepartListForSave = null;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в списке дочерних подразделений клиента. Текст ошибки: " + f.Message);
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
                if (bSaveChanges() == true)
                {
                    SimulateChangeCustomerChildListProperties(m_objSelectedCustomer, enumActionSaveCancel.Save);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в списке дочерних подразделений клиента. Текст ошибки: " + f.Message);
            }
            return;
        }

        #endregion

        #region Отмена
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Cancel();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка отмены изменений в списке дочерних подразделений клиента. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        /// <summary>
        /// Отмена внесенных изменений
        /// </summary>
        private void Cancel()
        {
            try
            {
                SimulateChangeCustomerChildListProperties(m_objSelectedCustomer, enumActionSaveCancel.Cancel);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка отмены изменений в списке дочерних подразделений клиента. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;

        }
        #endregion
             
    }

    /// <summary>
    /// Тип, хранящий информацию, которая передается получателям уведомления о событии
    /// </summary>
    public partial class ChangeCustomerChildListPropertieEventArgs : EventArgs
    {
        private readonly CCustomer m_objCustomer;
        public CCustomer Customer
        { get { return m_objCustomer; } }

        private readonly enumActionSaveCancel m_enActionType;
        public enumActionSaveCancel ActionType
        { get { return m_enActionType; } }


        public ChangeCustomerChildListPropertieEventArgs(CCustomer objCustomer, enumActionSaveCancel enActionType)
        {
            m_objCustomer = objCustomer;
            m_enActionType = enActionType;
        }
    }

}
