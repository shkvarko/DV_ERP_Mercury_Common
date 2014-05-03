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
    public partial class ctrlCustomerLimit : UserControl
    {
        #region Свойства, переменные
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;
        private CCustomer m_objSelectedCustomer;
        private System.Boolean m_bIsChanged;
        private System.Boolean m_bDisableEvents;
        private System.Boolean m_bIsReadOnly;
        private List<CCreditLimit> m_objCreditLimitList;
        private const System.Int32 iPanelInfoHeight = 35;
        private const System.Int32 iPanelInfoMinHeight = 23;
        #endregion

        #region События
        // Создаем закрытое поле, ссылающееся на заголовок списка делегатов
        private EventHandler<ChangeCustomerLimitPropertieEventArgs> m_ChangeCustomerLimitProperties;
        // Создаем в классе член-событие
        public event EventHandler<ChangeCustomerLimitPropertieEventArgs> ChangeCustomerLimitProperties
        {
            add
            {
                // берем закрытую блокировку и добавляем обработчик
                // (передаваемый по значению) в список делегатов
                m_ChangeCustomerLimitProperties += value;
            }
            remove
            {
                // берем закрытую блокировку и удаляем обработчик
                // (передаваемый по значению) из списка делегатов
                m_ChangeCustomerLimitProperties -= value;
            }
        }
        /// <summary>
        /// Инициирует событие и уведомляет о нем зарегистрированные объекты
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnChangeCustomerLimitProperties(ChangeCustomerLimitPropertieEventArgs e)
        {
            // Сохраняем поле делегата во временном поле для обеспечение безопасности потока
            EventHandler<ChangeCustomerLimitPropertieEventArgs> temp = m_ChangeCustomerLimitProperties;
            // Если есть зарегистрированные объектв, уведомляем их
            if (temp != null) temp(this, e);
        }
        public void SimulateChangeCustomerLimitProperties(CCustomer objCustomer, enumActionSaveCancel enActionType)
        {
            // Создаем объект, хранящий информацию, которую нужно передать
            // объектам, получающим уведомление о событии
            ChangeCustomerLimitPropertieEventArgs e = new ChangeCustomerLimitPropertieEventArgs(objCustomer, enActionType);

            // Вызываем виртуальный метод, уведомляющий наш объект о возникновении события
            // Если нет типа, переопределяющего этот метод, наш объект уведомит все объекты, 
            // подписавшиеся на уведомление о событии
            OnChangeCustomerLimitProperties(e);
        }
        #endregion

        #region Конструктор
        public ctrlCustomerLimit(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem)
        {
            InitializeComponent();
            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_bIsChanged = false;
            m_bDisableEvents = false;

            m_objSelectedCustomer = null;
            m_objCreditLimitList = null;

            cboxCurrency.Properties.Items.Clear();
            List<CCurrency> objCurrencyList = CCurrency.GetCurrencyList(m_objProfile, null);
            if (objCurrencyList != null)
            {
                foreach (CCurrency objCurrency in objCurrencyList) { cboxCurrency.Properties.Items.Add(objCurrency); }
            }
            objCurrencyList = null;
            if (cboxCurrency.Properties.Items.Count > 0) { cboxCurrency.SelectedItem = cboxCurrency.Properties.Items[0]; }
            CheckClientsRight();
        }
        /// <summary>
        /// Проверка динамических прав
        /// </summary>
        private void CheckClientsRight()
        {
            try
            {
                System.Boolean bEditCustomerCurrentLimit = m_objProfile.GetClientsRight().GetState( ERP_Mercury.Global.Consts.strDR_EditCustomerCurrentLimit );
                System.Boolean bEditCustomerApprovedLimit = m_objProfile.GetClientsRight().GetState( ERP_Mercury.Global.Consts.strDR_EditCustomerApprovedLimit );
                if( ( bEditCustomerCurrentLimit == false) && ( bEditCustomerApprovedLimit == false ) )
                {
                    btnEdit.Visible = false;
                    btnPrint.Visible = false;
                    btnSave.Visible = false;
                }

                colDayApprov.OptionsColumn.ReadOnly = (bEditCustomerApprovedLimit == false);
                colDayApprov.ImageIndex = (bEditCustomerApprovedLimit == false) ? 0 : -1;
                colMoneyApprov.OptionsColumn.ReadOnly = (bEditCustomerApprovedLimit == false);
                colMoneyApprov.ImageIndex = (bEditCustomerApprovedLimit == false) ? 0 : -1;

                colDay.OptionsColumn.ReadOnly = (bEditCustomerCurrentLimit == false);
                colDay.ImageIndex = (bEditCustomerCurrentLimit == false) ? 0 : -1;
                colMoney.OptionsColumn.ReadOnly = (bEditCustomerCurrentLimit == false);
                colMoney.ImageIndex = (bEditCustomerCurrentLimit == false) ? 0 : -1;
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
            m_bIsChanged = bModified;
            btnSave.Enabled = m_bIsChanged;
            btnCancel.Enabled = btnSave.Enabled;
            if (m_bIsChanged == true)
            {
                SimulateChangeCustomerLimitProperties(m_objSelectedCustomer, enumActionSaveCancel.Unkown);
                cboxCurrency.Properties.ReadOnly = bModified;
            }
            EnableCompanyBtns();
        }
        private void cboxCurrency_SelectedValueChanged(object sender, EventArgs e)
        {
            if ((m_bDisableEvents == true) || (cboxCurrency.SelectedItem == null)) { return; }
            try
            {
                LoadCustomerLimitByCurrency((CCurrency)cboxCurrency.SelectedItem);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования кредитного лимита клиента. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void treeList_CellValueChanged(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                if ((e.Column == colMoneyApprov) && (e.Value != null))
                {
                    e.Node.SetValue(colMoney, e.Value);
                }
                if ((e.Column == colDayApprov) && (e.Value != null))
                {
                    e.Node.SetValue(colDay, e.Value);
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
        private void treeList_CellValueChanging(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
            try
            {
                //if (m_bDisableEvents == true) { return; }
                //if ((e.Column == colMoneyApprov) && (e.Value != null))
                //{
                //    e.Node.SetValue(colMoney, e.Value);
                //}
                //if ((e.Column == colDayApprov) && (e.Value != null))
                //{
                //    e.Node.SetValue(colDay, e.Value);
                //}
                //SetPropertiesModified(true);
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
                cboxCurrency.Properties.ReadOnly = bSet;
                listBoxCompany.Enabled = !bSet;

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
                btnSave.Enabled = false;
                EnableCompanyBtns();
                //SetPropertiesModified(true);

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

        #region Редактирование кредитного лимита клиента
        /// <summary>
        /// Загружает в дерево кредитные лимиты выбранного клиента для указанной валюты
        /// </summary>
        /// <param name="objCurrency">валюта</param>
        private void LoadCustomerLimitByCurrency(CCurrency objCurrency)
        {
            if (objCurrency == null) { return; }
            try
            {
                this.tableLayoutPanelBgrnd.SuspendLayout();
                this.tableLayoutPaneltree.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();
                treeList.Nodes.Clear();
                treeListArjive.Nodes.Clear();
                if (m_objCreditLimitList != null)
                {
                    foreach (CCreditLimit objCreditLimit in m_objCreditLimitList)
                    {
                        if ((objCreditLimit.Currency == null) || (objCreditLimit.Currency.ID.CompareTo(objCurrency.ID) != 0)) { continue; }
                        treeList.AppendNode(new object[] { objCreditLimit.Company.Abbr, objCreditLimit.ApprovedCurrencyValue, objCreditLimit.ApprovedDays, objCreditLimit.CurrencyValue, objCreditLimit.Days }, null).Tag = objCreditLimit;
                        foreach (CCompany objItem in listBoxCompany.Items)
                        {
                            if (objItem.ID.CompareTo(objCreditLimit.Company.ID) == 0)
                            {
                                listBoxCompany.Items.Remove(objItem);
                                break;
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
                this.tableLayoutPaneltree.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования кредитного лимита клиента. Текст ошибки: " + f.Message);
            }
            finally
            {
                m_bDisableEvents = false;
                EnableCompanyBtns();
            }
            return;

        }

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
        /// Загружает значение категории клиента в дерево
        /// </summary>
        /// <param name="objCustomer"></param>
        public void LoadCustomerLimit(CCustomer objCustomer)
        {
            m_bDisableEvents = true;
            if (objCustomer == null) { return; }
            try
            {
                m_objSelectedCustomer = objCustomer;
                lblCustomerIfo.Text = objCustomer.FullName;
                dtBeginDate.DateTime = System.DateTime.Today;
                dtEndDate.DateTime = System.DateTime.Today;

                listBoxCompany.Items.Clear();


                List<CCompany> objCompanyList = CCompany.GetCompanyListFreeBlackList(m_objSelectedCustomer, m_objProfile, null);
                if( objCompanyList != null )
                {
                    foreach( CCompany objCompany in objCompanyList ){listBoxCompany.Items.Add(objCompany);}
                }
                objCompanyList = null;

                m_objCreditLimitList = CCreditLimit.GetCreditLimitList(m_objProfile, null, m_objSelectedCustomer);

                if( cboxCurrency.Properties.Items.Count > 0 )
                {
                    if ((m_objCreditLimitList != null) && (m_objCreditLimitList.Count > 0))
                    {
                        foreach (CCurrency objItem in cboxCurrency.Properties.Items)
                        {
                            if (objItem.ID.CompareTo(m_objCreditLimitList[0].Currency.ID) == 0)
                            {
                                cboxCurrency.SelectedItem = objItem;
                                break;
                            }
                        }
                    }
                    else
                    {
                        cboxCurrency.SelectedItem = cboxCurrency.Properties.Items[0];
                    }
                    LoadCustomerLimitByCurrency((CCurrency)cboxCurrency.SelectedItem);
                }
                System.Boolean bBlackList = CCreditLimit.IsCustomerInBlackList(m_objProfile, null, m_objSelectedCustomer);
                tableLayoutPanelBgrnd.RowStyles[1].Height = (bBlackList ? iPanelInfoHeight : iPanelInfoMinHeight);
                panelControlInfo.Visible = bBlackList;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования кредитного лимита клиента. Текст ошибки: " + f.Message);
            }
            finally
            {
                m_bDisableEvents = false;
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
                m_bDisableEvents = false;
            }
            return;
        }
        #endregion

        #region Добавить/Удалить компанию
        /// <summary>
        /// Включает/Выключает кнопки добавления/удаления компаний в дерево
        /// </summary>
        private void EnableCompanyBtns()
        {
            try
            {
                btnAddCompany.Enabled = (((listBoxCompany.Items.Count > 0) && (listBoxCompany.SelectedItem != null)) && (m_bIsReadOnly == false) );
                btnAddAllCompany.Enabled = ((listBoxCompany.Items.Count > 0) && (m_bIsReadOnly == false));
                btnRemoveCompany.Enabled = ((treeList.FocusedNode != null) && (m_bIsReadOnly == false));
                btnRemoveAllCompany.Enabled = ((treeList.Nodes.Count > 0) && (m_bIsReadOnly == false));
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования кредитного лимита клиента. Текст ошибки: " + f.Message);
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
        private void AddCompany(CCompany objCompany)
        {
            try
            {
                if (objCompany == null) { return; }
                System.Boolean bCompanyExists = false;
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    if (((CCreditLimit)objNode.Tag).Company.ID.CompareTo(objCompany.ID) == 0)
                    {
                        bCompanyExists = true;
                        break;
                    }
                }
                if (bCompanyExists == false)
                {
                    CCreditLimit objNewCreditLimit = new CCreditLimit(System.Guid.Empty, m_objSelectedCustomer, objCompany, (CCurrency)cboxCurrency.SelectedItem, 0, 0, 0, 0);
                    treeList.AppendNode(new object[] { objNewCreditLimit.Company.Abbr, objNewCreditLimit.ApprovedCurrencyValue, objNewCreditLimit.ApprovedDays, 
                        objNewCreditLimit.CurrencyValue, objNewCreditLimit.Days }, null).Tag = objNewCreditLimit;

                    System.Int32 iRemoveIndx = -1;
                    for (System.Int32 i = 0; i < listBoxCompany.Items.Count; i++)
                    {
                        if (((CCompany)listBoxCompany.GetItemValue(i)).ID.CompareTo(objCompany.ID) == 0)
                        {
                            iRemoveIndx = i;
                            break;
                        }
                    }
                    if (iRemoveIndx >= 0) { listBoxCompany.Items.RemoveAt(iRemoveIndx); }
                    SetPropertiesModified(true);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования кредитного лимита клиента. Текст ошибки: " + f.Message);
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
                for (System.Int32 i = 0; i < listBoxCompany.Items.Count; i++)
                {
                    foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes)
                    {
                        if (objNode.Tag == null) { continue; }
                        if (((CCreditLimit)objNode.Tag).Company.ID.CompareTo(((CCompany)listBoxCompany.GetItemValue(i)).ID) == 0)
                        {
                            bCompanyExists = true;
                            break;
                        }
                    }

                    if (bCompanyExists == false)
                    {
                        CCreditLimit objNewCreditLimit = new CCreditLimit(System.Guid.Empty, m_objSelectedCustomer, ((CCompany)listBoxCompany.GetItemValue(i)), (CCurrency)cboxCurrency.SelectedItem, 0, 0, 0, 0);
                        treeList.AppendNode(new object[] { objNewCreditLimit.Company.Abbr, objNewCreditLimit.ApprovedCurrencyValue, objNewCreditLimit.CurrencyValue, 
                        objNewCreditLimit.ApprovedDays, objNewCreditLimit.Days }, null).Tag = objNewCreditLimit;
                    }
                }
                listBoxCompany.Items.Clear();
                SetPropertiesModified(true);

                this.tableLayoutPanelBgrnd.ResumeLayout(false);
                this.tableLayoutPaneltree.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования кредитного лимита клиента. Текст ошибки: " + f.Message);
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
                if( (treeList.FocusedNode == null) || (treeList.FocusedNode.Tag == null) ) { return; }
                CCompany objSelectedCompany = ((CCreditLimit)treeList.FocusedNode.Tag).Company;
                System.Boolean bCompanyExists = false;
                for (System.Int32 i = 0; i < listBoxCompany.Items.Count; i++)
                {
                    if (((CCompany)listBoxCompany.GetItemValue(i)).ID.CompareTo(objSelectedCompany.ID) == 0)
                    {
                        bCompanyExists = true;
                        break;
                    }
                }
                if (bCompanyExists == false)
                {
                    listBoxCompany.Items.Add(objSelectedCompany);
                }
                treeList.Nodes.Remove(treeList.FocusedNode);
                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования кредитного лимита клиента. Текст ошибки: " + f.Message);
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
                ((System.ComponentModel.ISupportInitialize)(this.listBoxCompany)).BeginInit();
                this.listBoxCompany.Items.Clear();

                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    listBoxCompany.Items.Add( ( (CCreditLimit)objNode.Tag ).Company);
                }
                treeList.Nodes.Clear();
                SetPropertiesModified(true);

                this.tableLayoutPanelBgrnd.ResumeLayout(false);
                this.tableLayoutPaneltree.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.listBoxCompany)).EndInit();
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования кредитного лимита клиента. Текст ошибки: " + f.Message);
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
                AddCompany(( CCompany )listBoxCompany.SelectedValue);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования кредитного лимита клиента. Текст ошибки: " + f.Message);
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
                if( (listBoxCompany.Enabled == true) && (listBoxCompany.SelectedValue != null))
                { AddCompany((CCompany)listBoxCompany.SelectedValue); }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования кредитного лимита клиента. Текст ошибки: " + f.Message);
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
                SendMessageToLog("Ошибка редактирования кредитного лимита клиента. Текст ошибки: " + f.Message);
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
                SendMessageToLog("Ошибка редактирования кредитного лимита клиента. Текст ошибки: " + f.Message);
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
                SendMessageToLog("Ошибка редактирования кредитного лимита клиента. Текст ошибки: " + f.Message);
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
            List<CCreditLimit> objCreditLimitListForSave = new List<CCreditLimit>();
            try
            {
                CCreditLimit objCreditLimit = null;
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    objCreditLimit = (CCreditLimit)objNode.Tag;
                    objCreditLimit.ApprovedCurrencyValue = System.Convert.ToDouble( objNode.GetValue(colMoneyApprov) );
                    objCreditLimit.CurrencyValue = System.Convert.ToDouble(objNode.GetValue(colMoney));
                    objCreditLimit.ApprovedDays = System.Convert.ToInt32(objNode.GetValue(colDayApprov));
                    objCreditLimit.Days = System.Convert.ToInt32(objNode.GetValue(colDay));

                    objCreditLimitListForSave.Add(objCreditLimit);
                }

                if (objCreditLimitListForSave != null)
                {
                    System.String strErr = "";
                    bRet = CCreditLimit.SaveCreditLimitList( m_objProfile, null, objCreditLimitListForSave, m_objSelectedCustomer, (CCurrency)cboxCurrency.SelectedItem, ref strErr);
                    if (bRet == false) { SendMessageToLog(strErr); }
                    else { m_objSelectedCustomer.CreditLimitList = objCreditLimitListForSave; }
                }

                objCreditLimitListForSave = null;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в кредитном лимите клиента. Текст ошибки: " + f.Message);
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
                    SimulateChangeCustomerLimitProperties(m_objSelectedCustomer, enumActionSaveCancel.Save);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в кредитном лимите клиента. Текст ошибки: " + f.Message);
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
                SendMessageToLog("Ошибка отмены изменений в кредитного лимита клиента. Текст ошибки: " + f.Message);
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
                SimulateChangeCustomerLimitProperties(m_objSelectedCustomer, enumActionSaveCancel.Cancel);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка отмены изменений в кредитного лимита клиента. Текст ошибки: " + f.Message);
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
    public partial class ChangeCustomerLimitPropertieEventArgs : EventArgs
    {
        private readonly CCustomer m_objCustomer;
        public CCustomer Customer
        { get { return m_objCustomer; } }

        private readonly enumActionSaveCancel m_enActionType;
        public enumActionSaveCancel ActionType
        { get { return m_enActionType; } }


        public ChangeCustomerLimitPropertieEventArgs(CCustomer objCustomer, enumActionSaveCancel enActionType)
        {
            m_objCustomer = objCustomer;
            m_enActionType = enActionType;
        }
    }
}
