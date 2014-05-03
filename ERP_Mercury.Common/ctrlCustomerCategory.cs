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
    public partial class ctrlCustomerCategory : UserControl
    {
        #region Свойства, переменные
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;
        private CCustomer m_objSelectedCustomer;
        private System.Boolean m_bIsChanged;
        private System.Boolean m_bDisableEvents;
        private System.Boolean m_bIsReadOnly;
        #endregion

        #region События
        // Создаем закрытое поле, ссылающееся на заголовок списка делегатов
        private EventHandler<ChangeCustomerCategoryPropertieEventArgs> m_ChangeCustomerCategoryProperties;
        // Создаем в классе член-событие
        public event EventHandler<ChangeCustomerCategoryPropertieEventArgs> ChangeCustomerCategoryProperties
        {
            add
            {
                // берем закрытую блокировку и добавляем обработчик
                // (передаваемый по значению) в список делегатов
                m_ChangeCustomerCategoryProperties += value;
            }
            remove
            {
                // берем закрытую блокировку и удаляем обработчик
                // (передаваемый по значению) из списка делегатов
                m_ChangeCustomerCategoryProperties -= value;
            }
        }
        /// <summary>
        /// Инициирует событие и уведомляет о нем зарегистрированные объекты
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnChangeCustomerCategoryProperties(ChangeCustomerCategoryPropertieEventArgs e)
        {
            // Сохраняем поле делегата во временном поле для обеспечение безопасности потока
            EventHandler<ChangeCustomerCategoryPropertieEventArgs> temp = m_ChangeCustomerCategoryProperties;
            // Если есть зарегистрированные объектв, уведомляем их
            if (temp != null) temp(this, e);
        }
        public void SimulateChangeCustomerCategoryProperties(CCustomer objCustomer, enumActionSaveCancel enActionType)
        {
            // Создаем объект, хранящий информацию, которую нужно передать
            // объектам, получающим уведомление о событии
            ChangeCustomerCategoryPropertieEventArgs e = new ChangeCustomerCategoryPropertieEventArgs(objCustomer, enActionType);

            // Вызываем виртуальный метод, уведомляющий наш объект о возникновении события
            // Если нет типа, переопределяющего этот метод, наш объект уведомит все объекты, 
            // подписавшиеся на уведомление о событии
            OnChangeCustomerCategoryProperties(e);
        }
        #endregion

        #region Конструктор
        public ctrlCustomerCategory(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem)
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_bIsChanged = false;
            m_bDisableEvents = false;

            m_objSelectedCustomer = null;

            InitializeTreeList();
            CheckClientsRight();
        }
        /// <summary>
        /// Построение столбцов и строк в дереве
        /// </summary>
        private void InitializeTreeList()
        {
            try
            {
                List<CCustomerCategory> objCustomerCategoryList = CCustomerCategory.GetCustomerCategoryList(m_objProfile, null);
                if (objCustomerCategoryList != null)
                {
                    DevExpress.XtraTreeList.Columns.TreeListColumn colCustomerCategory = null;
                    foreach (CCustomerCategory objCustomerCategory in objCustomerCategoryList)
                    {
                        colCustomerCategory = this.treeList.Columns.Add();
                        colCustomerCategory.ColumnEdit = repItemCheckEdit;
                        colCustomerCategory.Caption = objCustomerCategory.Code;
                        colCustomerCategory.FieldName = objCustomerCategory.Code;
                        colCustomerCategory.MinWidth = 50;
                        colCustomerCategory.Name = objCustomerCategory.ID.ToString();
                        colCustomerCategory.OptionsColumn.AllowSort = false;
                        colCustomerCategory.Visible = true;
                        colCustomerCategory.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        colCustomerCategory.Tag = objCustomerCategory;
                    }
                }
                objCustomerCategoryList = null;

                List<CCompany> objCompanyList = CCompany.GetCompanyList(m_objProfile, null);
                if (objCompanyList != null)
                {
                    foreach (CCompany objCompany in objCompanyList)
                    {
                        treeList.AppendNode(new object[] { objCompany.Abbr }, null).Tag = objCompany;
                    }
                }
                objCompanyList = null;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка установки начальных настроек дерева. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return ;
        }
        /// <summary>
        /// Проверка динамических прав
        /// </summary>
        private void CheckClientsRight()
        {
            try
            {
                if (m_objProfile.GetClientsRight().GetState(ERP_Mercury.Global.Consts.strDR_EditCustomerCategory) == false)
                {
                    btnEdit.Visible = false;
                    btnPrint.Visible = false;
                    btnSave.Visible = false;
                }

                //objClientRights = null;
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

        #region Редактирование категории клиента
        /// <summary>
        /// Загружает значение категории клиента в дерево
        /// </summary>
        /// <param name="objCustomer"></param>
        public void LoadCustomerCategory(CCustomer objCustomer)
        {
            m_bDisableEvents = true;
            if (objCustomer == null) { return; }
            try
            {
                m_objSelectedCustomer = objCustomer;
                lblCustomerIfo.Text = objCustomer.FullName;

                this.tableLayoutPanelBgrnd.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();

                m_objSelectedCustomer.CategoryCompanyList = CCustomerCategoryCompany.GetCategoryCompanyList(m_objProfile, null, m_objSelectedCustomer.ID);
                if( m_objSelectedCustomer.CategoryCompanyList != null )
                {
                    System.Guid uuidCompanyId = System.Guid.Empty;
                    System.Guid uuidCategoryId = System.Guid.Empty;

                    foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes)
                    {
                        foreach (DevExpress.XtraTreeList.Columns.TreeListColumn objColumn in treeList.Columns)
                        {
                            if (objColumn == colCompany) { continue; }
                            objNode.SetValue(objColumn, false);
                            if (objNode.Tag == null) { continue; }
                            if (objColumn.Tag == null) { continue; }

                            uuidCompanyId = ((CCompany)objNode.Tag).ID;
                            uuidCategoryId = ((CCustomerCategory)objColumn.Tag).ID;

                            foreach (CCustomerCategoryCompany objCustomerCategoryCompany in m_objSelectedCustomer.CategoryCompanyList)
                            {
                                if ((objCustomerCategoryCompany.objCompany.ID.CompareTo(uuidCompanyId) == 0) &&
                                    (objCustomerCategoryCompany.CustomerCategory.ID.CompareTo(uuidCategoryId) == 0))
                                {
                                    objNode.SetValue(objColumn, true);
                                }
                            }
                        }
                    }
                }

                SetPropertiesModified(false);

                SetModeReadOnly(true);
                btnEdit.Enabled = true;
                btnCancel.Enabled = true;
                btnCancel.Focus();

                this.tableLayoutPanelBgrnd.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования категории клиента. Текст ошибки: " + f.Message);
            }
            finally
            {
                m_bDisableEvents = false;
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

                m_bIsReadOnly = bSet;
                btnEdit.Enabled = bSet;
                btnSave.Enabled = !bSet;
                btnCancel.Enabled = !bSet;
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

        #region Индикация изменений
        private void SetPropertiesModified(System.Boolean bModified)
        {
            m_bIsChanged = bModified;
            btnSave.Enabled = m_bIsChanged;
            btnCancel.Enabled = btnSave.Enabled;
            if (m_bIsChanged == true)
            {
                SimulateChangeCustomerCategoryProperties(m_objSelectedCustomer, enumActionSaveCancel.Unkown);
            }
        }
        private void treeList_CellValueChanging(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                if ((e.Column != null) && (e.Column != colCompany))
                {
                    if (System.Convert.ToBoolean(e.Value) == true)
                    {
                        m_bDisableEvents = true;
                        foreach (DevExpress.XtraTreeList.Columns.TreeListColumn objColumn in treeList.Columns)
                        {
                            if ((objColumn != colCompany) && (objColumn != e.Column))
                            {
                                e.Node.SetValue(objColumn, false);
                            }
                        }
                        m_bDisableEvents = false;
                    }
                }
                SetPropertiesModified(true);
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения свойств. Текст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        private void treeList_MouseClick(object sender, MouseEventArgs e)
        {
            DevExpress.XtraTreeList.TreeListHitInfo hi = treeList.CalcHitInfo(new Point(e.X, e.Y));
            if ((hi != null) && (hi.Column != null) && (hi.Column != colCompany) && ( hi.HitInfoType == DevExpress.XtraTreeList.HitInfoType.Column))
            {
                SendMessageToLog(hi.HitInfoType.ToString());
                this.tableLayoutPanelBgrnd.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();

                System.Int32 iCellFalseCount = 0;
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes)
                {
                    if (System.Convert.ToBoolean(objNode.GetValue(hi.Column)) == false) 
                    { 
                        iCellFalseCount++;
                        break;
                    }
                }

                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes)
                {
                    objNode.SetValue(hi.Column, (iCellFalseCount > 0));
                    if (iCellFalseCount > 0)
                    {
                        foreach (DevExpress.XtraTreeList.Columns.TreeListColumn objColumn in treeList.Columns)
                        {
                            if ((objColumn != colCompany) && (objColumn != hi.Column))
                            {
                                objNode.SetValue(objColumn, false);
                            }
                        }
                    }
                }

                this.tableLayoutPanelBgrnd.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();

                if (m_bDisableEvents == true) { return; }
                SetPropertiesModified(true);
            }

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

        #region Сохранить изменения
        /// <summary>
        /// Сохраняет изменения в базе данных
        /// </summary>
        /// <returns>true - удачное завершение операции;false - ошибка</returns>
        private System.Boolean bSaveChanges()
        {
            System.Boolean bRet = false;
            List<CCustomerCategoryCompany> objCategoryCompanyListForSave = new List<CCustomerCategoryCompany>();
            try
            {
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes)
                {
                    foreach (DevExpress.XtraTreeList.Columns.TreeListColumn objColumn in treeList.Columns)
                    {
                        if (objColumn == colCompany) { continue; }
                        if (objNode.Tag == null) { continue; }
                        if (objColumn.Tag == null) { continue; }

                        if (System.Convert.ToBoolean(objNode.GetValue(objColumn)) == true)
                        {
                            objCategoryCompanyListForSave.Add(new CCustomerCategoryCompany((CCustomerCategory)objColumn.Tag, (CCompany)objNode.Tag));
                        }
                    }
                }

                if (objCategoryCompanyListForSave != null)
                {
                    System.String strErr = "";
                    bRet = CCustomerCategoryCompany.SetCategoryCompanyListForCustomer(m_objProfile, null, m_objSelectedCustomer.ID, objCategoryCompanyListForSave, ref strErr);
                    if (bRet == false) { SendMessageToLog(strErr); }
                    m_objSelectedCustomer.CategoryCompanyList = objCategoryCompanyListForSave;
                }

                objCategoryCompanyListForSave = null;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в категории клиента. Текст ошибки: " + f.Message);
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
                    SimulateChangeCustomerCategoryProperties(m_objSelectedCustomer, enumActionSaveCancel.Save);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в категории клиента. Текст ошибки: " + f.Message);
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
                SendMessageToLog("Ошибка отмены изменений в категории клиента. Текст ошибки: " + f.Message);
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
                SimulateChangeCustomerCategoryProperties(m_objSelectedCustomer, enumActionSaveCancel.Cancel);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка отмены изменений в категории клиента. Текст ошибки: " + f.Message);
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
    public partial class ChangeCustomerCategoryPropertieEventArgs : EventArgs
    {
        private readonly CCustomer m_objCustomer;
        public CCustomer Customer
        { get { return m_objCustomer; } }

        private readonly enumActionSaveCancel m_enActionType;
        public enumActionSaveCancel ActionType
        { get { return m_enActionType; } }


        public ChangeCustomerCategoryPropertieEventArgs(CCustomer objCustomer, enumActionSaveCancel enActionType)
        {
            m_objCustomer = objCustomer;
            m_enActionType = enActionType;
        }
    }

}
