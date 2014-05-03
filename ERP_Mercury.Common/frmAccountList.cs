using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using OfficeOpenXml;

namespace ERP_Mercury.Common
{
    public partial class frmAccountList : DevExpress.XtraEditors.XtraForm
    {
        #region Свойства
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;
        private System.Boolean m_bOnlyView;
        private System.Boolean m_bIsChanged;
        private System.Boolean m_bDisableEvents;
        private System.Boolean m_bNewObject;
        //private System.Boolean m_bIsReadOnly;


        private List<CBank> m_objBankList;
        private List<CAccount> m_objAccountList;
        private CAccount m_objSelectedAccount;
        public CAccount SelectedAccount
        {
            get { return m_objSelectedAccount; }
        }
        private DevExpress.XtraGrid.Views.Base.ColumnView ColumnView
        {
            get { return gridControlAccountList.MainView as DevExpress.XtraGrid.Views.Base.ColumnView; }
        }
        // потоки
        public System.Threading.Thread ThreadLoadBankList { get; set; }
        public System.Threading.Thread ThreadLoadAccountList { get; set; }

        public System.Threading.ManualResetEvent EventStopThread { get; set; }
        public System.Threading.ManualResetEvent EventThreadStopped { get; set; }

        public delegate void LoadBankListDelegate(List<CBank> objBankList, System.Int32 iRowCountInLis);
        public LoadBankListDelegate m_LoadBankListDelegate;

        public delegate void LoadAccountListDelegate(List<CAccount> objAccountList, System.Int32 iRowCountInList);
        public LoadAccountListDelegate m_LoadAccountListDelegate;


        private const System.Int32 iThreadSleepTime = 1000;
        private const System.String strWaitCustomer = "ждите... идет заполнение списка";
        private System.Boolean m_bThreadFinishJob;
        private const System.String strRegistryTools = "\\AccountListTools\\";
        private const System.Int32 iWaitingpanelIndex = 0;
        private const System.Int32 iWaitingpanelHeight = 35;
        private const System.String m_strModeReadOnly = "Режим просмотра";
        private const System.String m_strModeEdit = "Режим редактирования";
        #endregion

        public frmAccountList(UniXP.Common.MENUITEM objMenuItem)
        {
            InitializeComponent();

            m_objMenuItem = objMenuItem;
            m_objProfile = objMenuItem.objProfile;
            m_bThreadFinishJob = false;
            m_objBankList = new List<CBank>();
            m_objAccountList = new List<CAccount>();
            m_objSelectedAccount = null;

            AddGridColumns();
            RestoreLayoutFromRegistry();

            SearchProcessWoring.Visible = false;
            tabControl.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
            m_bOnlyView = false;
            m_bIsChanged = false;
            m_bDisableEvents = false;
            m_bNewObject = false;
        }

        #region Открытие формы
        private void frmAccount_Shown(object sender, EventArgs e)
        {
            try
            {
                LoadComboBox();

                StartThreadLoadCustomerList();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("frmAccount_Shown().\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        #endregion

        #region Настройки грида
        private void AddGridColumns()
        {
            ColumnView.Columns.Clear();

            AddGridColumn(ColumnView, "ID", "Идентификатор");
            AddGridColumn(ColumnView, "AccountTypeName", "Тип счёта");
            AddGridColumn(ColumnView, "CurrencyCode", "Валюта счёта");
            AddGridColumn(ColumnView, "AccountNumber", "№ счёта");
            AddGridColumn(ColumnView, "BankName", "Банк");
            AddGridColumn(ColumnView, "AccountNumber", "Счёт");

            foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in ColumnView.Columns)
            {
                objColumn.OptionsColumn.AllowEdit = false;
                objColumn.OptionsColumn.AllowFocus = false;
                objColumn.OptionsColumn.ReadOnly = true;

                //if (objColumn.FieldName == "VendorName")
                //{
                //    objColumn.BestFit();
                //}
                if(objColumn.FieldName == "ID")
                {
                    objColumn.Visible = false;
                }

                if ((objColumn.FieldName == "Value") || (objColumn.FieldName == "Expense") || (objColumn.FieldName == "Saldo"))
                {
                    objColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    objColumn.DisplayFormat.FormatString = "### ### ##0.00";
                    objColumn.SummaryItem.FieldName = objColumn.FieldName;
                    objColumn.SummaryItem.DisplayFormat = "Итого: {0:### ### ##0.00}";
                    objColumn.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                }
            }
        }
        private void AddGridColumn(DevExpress.XtraGrid.Views.Base.ColumnView view, string fieldName, string caption) { AddGridColumn(view, fieldName, caption, null); }
        private void AddGridColumn(DevExpress.XtraGrid.Views.Base.ColumnView view, string fieldName, string caption, DevExpress.XtraEditors.Repository.RepositoryItem item) { AddGridColumn(view, fieldName, caption, item, "", DevExpress.Utils.FormatType.None); }
        private void AddGridColumn(DevExpress.XtraGrid.Views.Base.ColumnView view, string fieldName, string caption, DevExpress.XtraEditors.Repository.RepositoryItem item, string format, DevExpress.Utils.FormatType type)
        {
            DevExpress.XtraGrid.Columns.GridColumn column = view.Columns.AddField(fieldName);
            column.Caption = caption;
            column.ColumnEdit = item;
            column.DisplayFormat.FormatType = type;
            column.DisplayFormat.FormatString = format;
            column.VisibleIndex = view.VisibleColumns.Count;
        }

        #region Настройки внешнего вида журналов
        /// <summary>
        /// Считывает настройки журналов из реестра
        /// </summary>
        public void RestoreLayoutFromRegistry()
        {
            System.String strReestrPath = this.m_objProfile.GetRegKeyBase();
            strReestrPath += (strRegistryTools);
            try
            {
                gridViewAccountList.RestoreLayoutFromRegistry(String.Format("{0}{1}", strReestrPath, gridViewAccountList.Name));
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                String.Format("Ошибка загрузки настроек журнала счетов.\n\nТекст ошибки : {0}", f.Message), "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally // очищаем занимаемые ресурсы
            {
            }

            return;
        }
        /// <summary>
        /// Записывает настройки журналов в реестр
        /// </summary>
        public void SaveLayoutToRegistry()
        {
            System.String strReestrPath = this.m_objProfile.GetRegKeyBase();
            strReestrPath += (strRegistryTools);
            try
            {
                gridViewAccountList.SaveLayoutToRegistry(strReestrPath + gridViewAccountList.Name);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка записи настроек журнала счетов.\n\nТекст ошибки : " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally // очищаем занимаемые ресурсы
            {
            }

            return;
        }
        #endregion

        #endregion

        #region Потоки
        /// <summary>
        /// Стартует поток, в котором загружается список клиентов
        /// </summary>
        public void StartThreadLoadCustomerList()
        {
            try
            {
                // инициализируем делегаты
                m_LoadBankListDelegate = new LoadBankListDelegate(LoadBankList);
                m_objBankList.Clear();

                barBtnAdd.Enabled = false;
                barBtnEdit.Enabled = false;
                barBtnDelete.Enabled = false;
                barBtnRefresh.Enabled = false;

                gridControlAccountList.MouseDoubleClick -= new MouseEventHandler(gridControlAccountList_MouseDoubleClick);

                // запуск потока
                this.ThreadLoadBankList = new System.Threading.Thread(LoadBankListInThread);
                this.ThreadLoadBankList.Start();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("StartThreadWithLoadData().\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }
        /// <summary>
        /// Загружает список клиентов
        /// </summary>
        public void LoadBankListInThread()
        {
            try
            {
                List<CBank> objBankList = CBank.GetBankList(m_objProfile, null, null);


                List<CBank> objAddBankList = new List<CBank>();
                if((objBankList != null) && (objBankList.Count > 0))
                {
                    System.Int32 iRecCount = 0;
                    System.Int32 iRecAllCount = 0;
                    foreach (CBank objBank in objBankList)
                    {
                        objAddBankList.Add( objBank );
                        iRecCount++;
                        iRecAllCount++;

                        if (iRecCount == 1000)
                        {
                            iRecCount = 0;
                            Thread.Sleep(1000);
                            this.Invoke(m_LoadBankListDelegate, new Object[] { objAddBankList, iRecAllCount });
                            objAddBankList.Clear();
                        }

                    }
                    if (iRecCount != 1000)
                    {
                        iRecCount = 0;
                        this.Invoke(m_LoadBankListDelegate, new Object[] { objAddBankList, iRecAllCount });
                        objAddBankList.Clear();
                    }

                }

                objBankList = null;
                objAddBankList = null;
                this.Invoke(m_LoadBankListDelegate, new Object[] { null, 0 });
                this.m_bThreadFinishJob = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadBankListInThread.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// загружает в combobox список банков
        /// </summary>
        /// <param name="objBankList">список банков</param>
        /// <param name="iRowCountInList">количество строк, которые требуется загрузить в combobox</param>
        private void LoadBankList(List<CBank> objBankList, System.Int32 iRowCountInList)
        {
            try
            {
                cboxBank.Text = strWaitCustomer;
                if ((objBankList != null) && (objBankList.Count > 0) && (cboxBank.Properties.Items.Count < iRowCountInList))
                {
                    cboxBank.Properties.Items.AddRange(objBankList);
                    editorAccountBank.Properties.Items.AddRange(objBankList);
                    m_objBankList.AddRange(objBankList);
                }
                else
                {
                    cboxBank.Text = "";
                    barBtnAdd.Enabled = !m_bOnlyView;
                    barBtnEdit.Enabled = (gridViewAccountList.FocusedRowHandle >= 0);
                    barBtnDelete.Enabled = ((!m_bOnlyView) && (gridViewAccountList.FocusedRowHandle >= 0));
                    barBtnRefresh.Enabled = true;

                    gridControlAccountList.MouseDoubleClick += new MouseEventHandler(gridControlAccountList_MouseDoubleClick);
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadBankList.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        private void barBtnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                StartThreadLoadAccountList();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("barBtnRefresh_Click.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;

        }
        private void txtAccount_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((e.KeyChar == (char)Keys.Enter) && (barBtnRefresh.Visible == true))
                {
                    StartThreadLoadAccountList();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("txtAccount_KeyPress.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        /// <summary>
        /// Стартует поток, в котором загружается список счетов
        /// </summary>
        public void StartThreadLoadAccountList()
        {
            try
            {
                // инициализируем делегаты
                m_LoadAccountListDelegate = new LoadAccountListDelegate(LoadAccountListInGrid);
                m_objAccountList.Clear();

                barBtnAdd.Enabled = false;
                barBtnEdit.Enabled = false;
                barBtnDelete.Enabled = false;
                barBtnRefresh.Enabled = false;

                gridControlAccountList.DataSource = null;
                SearchProcessWoring.Visible = true;

                //gridControlEarningList.MouseDoubleClick -= new MouseEventHandler(gridControlEarningList_MouseDoubleClick);

                // запуск потока
                this.ThreadLoadAccountList = new System.Threading.Thread(LoadAccountListInThread);
                this.ThreadLoadAccountList.Start();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("StartThreadLoadAccountList().\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }


        /// <summary>
        /// Загружает список счетов
        /// </summary>
        public void LoadAccountListInThread()
        {
            try
            {
                System.Guid uuidBankId = (((cboxBank.SelectedItem == null) || (System.Convert.ToString(cboxBank.SelectedItem) == "") || (cboxBank.Text == strWaitCustomer)) ? System.Guid.Empty : ((CBank)cboxBank.SelectedItem).ID);
                System.String txtAccountNumber = txtAccount.Text.Trim();

                System.String strErr = "";
                List<CAccount> objAccountList = CAccountDatabaseModel.GetAccountList( m_objProfile,  null, uuidBankId, txtAccountNumber, ref strErr );

                List<CAccount> objAddAccountList = new List<CAccount>();
                if ((objAccountList != null) && (objAccountList.Count > 0))
                {
                    System.Int32 iRecCount = 0;
                    System.Int32 iRecAllCount = 0;
                    foreach (CAccount objAccount in objAccountList)
                    {
                        objAddAccountList.Add(objAccount);
                        iRecCount++;
                        iRecAllCount++;

                        if (iRecCount == 1000)
                        {
                            iRecCount = 0;
                            Thread.Sleep(1000);
                            this.Invoke(m_LoadAccountListDelegate, new Object[] { objAddAccountList, iRecAllCount });
                            objAddAccountList.Clear();
                        }

                    }
                    if (iRecCount != 1000)
                    {
                        iRecCount = 0;
                        this.Invoke(m_LoadAccountListDelegate, new Object[] { objAddAccountList, iRecAllCount });
                        objAddAccountList.Clear();
                    }

                }

                objAccountList = null;
                objAddAccountList = null;
                this.Invoke(m_LoadAccountListDelegate, new Object[] { null, 0 });
                this.m_bThreadFinishJob = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadAccountListInThread.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        /// <summary>
        /// загружает в журнал список счетов
        /// </summary>
        /// <param name="objAccountList">список счетов</param>
        /// <param name="iRowCountInList">количество строк, которые требуется загрузить в журнал</param>
        private void LoadAccountListInGrid(List<CAccount> objAccountList, System.Int32 iRowCountInList)
        {
            try
            {
                if ((objAccountList != null) && (objAccountList.Count > 0) && (gridViewAccountList.RowCount < iRowCountInList))
                {
                    m_objAccountList.AddRange(objAccountList);
                    if (gridControlAccountList.DataSource == null)
                    {
                        gridControlAccountList.DataSource = m_objAccountList;
                    }
                    gridControlAccountList.RefreshDataSource();
                }
                else
                {
                    SearchProcessWoring.Visible = false;

                    barBtnAdd.Enabled = !m_bOnlyView;
                    barBtnEdit.Enabled = (gridViewAccountList.FocusedRowHandle >= 0);
                    barBtnDelete.Enabled = ((!m_bOnlyView) && (gridViewAccountList.FocusedRowHandle >= 0));
                    barBtnRefresh.Enabled = true;
                    gridControlAccountList.RefreshDataSource();

                    Cursor = Cursors.Default;

                    //gridControlEarningList.MouseDoubleClick += new MouseEventHandler(gridControlEarningList_MouseDoubleClick);
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("LoadAccountListInGrid.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }
            return;
        }

        #endregion

        #region Выпадающие списки
        /// <summary>
        /// Загружает собдержимое выпадающих списков
        /// </summary>
        private void LoadComboBox()
        {
            try
            {
                cboxBank.Properties.Items.Clear();
                cboxBank.Properties.Items.Add(new CBank());
                editorAccountBank.Properties.Items.Clear();

                editorAccountType.Properties.Items.Clear();
                editorAccountType.Properties.Items.AddRange( CAccountType.GetAccountTypeList(m_objProfile, null) );

                editorAccountCurrency.Properties.Items.Clear();
                editorAccountCurrency.Properties.Items.AddRange(CCurrency.GetCurrencyList(m_objProfile, null) );
            }
            catch (System.Exception f)
            {
                SendMessageToLog(String.Format("LoadComboBox. Текст ошибки: {0}", f.Message));
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
                    "SendMessageToLog.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        #endregion

        #region Свойства платежа
        private void gridViewAccountList_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                FocusedAccountChanged();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("gridViewAccountList_FocusedRowChanged. Текст ошибки: " + f.Message);
            }

            return;
        }

        private void gridViewAccountList_RowCountChanged(object sender, EventArgs e)
        {
            try
            {
                FocusedAccountChanged();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("gridViewAccountList_RowCountChanged. Текст ошибки: " + f.Message);
            }

            return;
        }

        /// <summary>
        /// Возвращает ссылку на выбранный в списке расчётный счёт
        /// </summary>
        /// <returns>ссылка на расчётный счёт</returns>
        private CAccount GetSelectedAccount()
        {
            CAccount objRet = null;
            try
            {
                if ((((DevExpress.XtraGrid.Views.Grid.GridView)gridControlAccountList.MainView).RowCount > 0) &&
                    (((DevExpress.XtraGrid.Views.Grid.GridView)gridControlAccountList.MainView).FocusedRowHandle >= 0))
                {
                    System.Guid uuidID = (System.Guid)(((DevExpress.XtraGrid.Views.Grid.GridView)gridControlAccountList.MainView)).GetFocusedRowCellValue("ID");

                    objRet = m_objAccountList.SingleOrDefault<CAccount>(x => x.ID.CompareTo(uuidID) == 0);
                }
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка поиска выбранного расчётного счёта. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return objRet;
        }

        /// <summary>
        /// Определяет, какой расчётный счёт выбран в журнале и отображает его свойства
        /// </summary>
        private void FocusedAccountChanged()
        {
            try
            {
                ShowAccountProperties(GetSelectedAccount());

                barBtnAdd.Enabled = !m_bOnlyView;
                barBtnEdit.Enabled = (gridViewAccountList.FocusedRowHandle >= 0);
                barBtnDelete.Enabled = ((!m_bOnlyView) && (gridViewAccountList.FocusedRowHandle >= 0));

                btnConfirmSelection.Enabled = (gridViewAccountList.FocusedRowHandle >= 0);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Отображение свойств расчётного счёта. Текст ошибки: " + f.Message);
            }

            return;
        }

        /// <summary>
        /// Отображает свойства расчётного счёта, выбранного в списке
        /// </summary>
        /// <param name="objAccount">расчётный счёт</param>
        private void ShowAccountProperties(CAccount objAccount)
        {
            try
            {
                this.tableLayoutPanelAccountProperties.SuspendLayout();

                txtAccountType.Text = "";
                txtAccountNum.Text = "";
                txtAccountCurrency.Text = "";
                txtAccountBank.Text = "";

                if (objAccount != null)
                {
                    txtAccountType.Text = objAccount.AccountTypeName;
                    txtAccountNum.Text = objAccount.AccountNumber;
                    txtAccountCurrency.Text = objAccount.CurrencyCode;
                    txtAccountBank.Text = objAccount.BankName;
                }

                this.tableLayoutPanelAccountProperties.ResumeLayout(false);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Отображение свойств расчётного счёта. Текст ошибки: " + f.Message);
            }
            return;
        }

        #endregion

        #region Режим просмотра/редактирования
        /// <summary>
        /// Устанавливает режим просмотра/редактирования
        /// </summary>
        /// <param name="bSet">true - режим просмотра; false - режим редактирования</param>
        private void SetModeReadOnly(System.Boolean bSet)
        {
            try
            {
                editorAccountType.Properties.ReadOnly = bSet;
                editorAccountCurrency.Properties.ReadOnly = bSet;
                editorAccountNumber.Properties.ReadOnly = bSet;
                editorAccountBank.Properties.ReadOnly = bSet;
                editorAccountDescrpn.Properties.ReadOnly = bSet;

                btnEdit.Enabled = bSet;

                lblEditMode.Text = ((bSet == true) ? m_strModeReadOnly : m_strModeEdit);
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
                SendMessageToLog("btnEdit_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;

        }
        private void SetPropertiesModified(System.Boolean bModified)
        {
            try
            {
                m_bIsChanged = bModified;
                btnSave.Enabled = (m_bIsChanged && (ValidateProperties() == true));
                btnCancel.Enabled = m_bIsChanged;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("SetPropertiesModified. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }

        #endregion

        #region Индикация изменений
        /// <summary>
        /// Проверяет содержимое элементов управления
        /// </summary>
        private System.Boolean ValidateProperties()
        {
            System.Boolean bRet = true;
            try
            {
                editorAccountType.Properties.Appearance.BackColor = ((editorAccountType.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                editorAccountCurrency.Properties.Appearance.BackColor = ((editorAccountCurrency.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                editorAccountBank.Properties.Appearance.BackColor = ((editorAccountBank.SelectedItem == null) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);
                editorAccountNumber.Properties.Appearance.BackColor = ((editorAccountNumber.Text.Trim() == System.String.Empty) ? System.Drawing.Color.Tomato : System.Drawing.Color.White);

                bRet = ((editorAccountType.SelectedItem != null) && (editorAccountCurrency.SelectedItem != null) &&
                    (editorAccountBank.SelectedItem != null) && (editorAccountNumber.Text.Trim() != System.String.Empty)
                    );

            }
            catch (System.Exception f)
            {
                SendMessageToLog("ValidateProperties. Текст ошибки: " + f.Message);
            }

            return bRet;
        }
        private void cboxAccountPropertie_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }

                SetPropertiesModified(true);
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog(String.Format("Ошибка изменения свойства {0}. Текст ошибки: {1}", ((DevExpress.XtraEditors.ComboBoxEdit)sender).ToolTip, f.Message));
            }
            finally
            {
            }

            return;
        }


        private void txtAccountPropertie_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                if (e.NewValue != null)
                {
                    SetPropertiesModified(true);
                    if ((sender.GetType().Name == "TextEdit") &&
                        (((DevExpress.XtraEditors.TextEdit)sender).Properties.ReadOnly == false))
                    {
                        System.String strValue = (System.String)e.NewValue;
                        ((DevExpress.XtraEditors.TextEdit)sender).Properties.Appearance.BackColor = (strValue == "") ? System.Drawing.Color.Tomato : System.Drawing.Color.White;
                    }
                }
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog(String.Format("Ошибка изменения свойств расчётного счёта. Текст ошибки: {0}", f.Message));
            }
            finally
            {
            }

            return;
        }

        #endregion

        #region Редактировать расчётный счёт
        private void barBtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                EditAccount(GetSelectedAccount(), false);

                SetModeReadOnly(false);
                btnEdit.Enabled = false;
                SetPropertiesModified(true);

            }//try
            catch (System.Exception f)
            {
                SendMessageToLog(f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }

        private void gridControlAccountList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                EditAccount(GetSelectedAccount(), false);

            }//try
            catch (System.Exception f)
            {
                SendMessageToLog(f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }
        /// <summary>
        /// очистка содержимого элементов управления
        /// </summary>
        private void ClearControls()
        {
            try
            {
                editorAccountType.SelectedItem = null;
                editorAccountCurrency.SelectedItem = null;
                editorAccountNumber.Text = "";
                editorAccountBank.SelectedItem = null;
                editorAccountDescrpn.Text = "";
            }
            catch (System.Exception f)
            {
                SendMessageToLog("ClearControls. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        /// <summary>
        /// Загружает свойства расчётного счёта для редактирования
        /// </summary>
        /// <param name="objAccount">расчётный счёт</param>
        /// <param name="bNewObject">признак "новый расчётный счёт"</param>
        public void EditAccount(CAccount objAccount, System.Boolean bNewObject)
        {
            if (objAccount == null) { return; }
            m_bDisableEvents = true;
            m_bNewObject = bNewObject;
            try
            {
                m_objSelectedAccount = objAccount;

                this.tableLayoutPanelBackground.SuspendLayout();

                ClearControls();

                editorAccountType.SelectedItem = (m_objSelectedAccount.AccountType == null) ? null : editorAccountType.Properties.Items.Cast<CAccountType>().SingleOrDefault<CAccountType>(x => x.ID.CompareTo(m_objSelectedAccount.AccountType.ID) == 0);
                editorAccountCurrency.SelectedItem = (m_objSelectedAccount.Currency == null) ? null : editorAccountCurrency.Properties.Items.Cast<CCurrency>().SingleOrDefault<CCurrency>(x => x.ID.CompareTo(m_objSelectedAccount.Currency.ID) == 0);
                editorAccountNumber.Text = m_objSelectedAccount.AccountNumber;
                editorAccountBank.SelectedItem = (m_objSelectedAccount.Bank == null) ? null : editorAccountBank.Properties.Items.Cast<CBank>().SingleOrDefault<CBank>(x => x.ID.CompareTo(m_objSelectedAccount.Bank.ID) == 0); 
                editorAccountDescrpn.Text = m_objSelectedAccount.Description;

                SetPropertiesModified(false);
                ValidateProperties();

                SetModeReadOnly(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования расчётного счёта. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.tableLayoutPanelBackground.ResumeLayout(false);
                m_bDisableEvents = false;
                btnCancel.Enabled = true;
                tabControl.SelectedTabPage = tabPageEditor;
            }
            return;
        }
        #endregion

        #region Новый расчётный счёт
        private void barBtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                NewAccount();

            }//try
            catch (System.Exception f)
            {
                SendMessageToLog(f.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            return;
        }

        /// <summary>
        /// Новый расчётный счёт
        /// </summary>
        public void NewAccount()
        {
            try
            {
                m_bNewObject = true;
                m_bDisableEvents = true;

                m_objSelectedAccount = new CAccount();

                this.tableLayoutPanelBackground.SuspendLayout();

                ClearControls();

                btnEdit.Enabled = false;
                btnCancel.Enabled = true;

                SetModeReadOnly(false);
                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка регистрации расчётного счёта. Текст ошибки: " + f.Message);
            }
            finally
            {
                tableLayoutPanelBackground.ResumeLayout(false);
                m_bDisableEvents = false;
                tabControl.SelectedTabPage = tabPageEditor;
            }
            return;
        }

        #endregion

        #region Удалить расчётный счёт
        /// <summary>
        /// Удаляет расчётный счёт
        /// </summary>
        private void DeleteAccount(CAccount objAccount)
        {
            if (objAccount == null) { return; }
            System.String strErr = "";

            try
            {
                System.Int32 iFocusedRowHandle = gridViewAccountList.FocusedRowHandle;
                if (DevExpress.XtraEditors.XtraMessageBox.Show("Подтвердите, пожалуйста, удаление расчётного счёта.\n\n№" + objAccount.AccountNumber, "Подтверждение",
                    System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == DialogResult.No) { return; }

                if ( CAccountDatabaseModel.RemoveObjectFromDataBase( objAccount.ID, m_objProfile, ref strErr) == true)
                {
                    StartThreadLoadAccountList();
                }
                else
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Предупреждение",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

                    SendMessageToLog("Удаление расчётного счёта. Текст ошибки: " + strErr);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Удаление расчётного счёта. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void barBtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteAccount(GetSelectedAccount());
            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("Удаление расчётного счёта. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        #endregion

        #region Отмена
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Cancel();
        }
        /// <summary>
        /// Отмена внесенных изменений
        /// </summary>
        private void Cancel()
        {
            try
            {
                tabControl.SelectedTabPage = tabPageViewer;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка отмены изменений. Текст ошибки: " + f.Message);
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
        private System.Boolean bSaveAccountPropertiesInDataBase(ref System.String strErr)
        {
            System.Boolean bRet = false;
            System.Boolean bOkSave = false;

            Cursor = Cursors.WaitCursor;
            try
            {
                System.Guid Bank_Guid = ((editorAccountBank.SelectedItem == null) ? (System.Guid.Empty) : ((CBank)editorAccountBank.SelectedItem).ID);
                System.Guid Currency_Guid = ((editorAccountCurrency.SelectedItem == null) ? (System.Guid.Empty) : ((CCurrency)editorAccountCurrency.SelectedItem).ID); 
                System.String Account_Number = editorAccountNumber.Text ;
                System.String Account_Description = editorAccountDescrpn.Text;
                System.Guid AccountType_Guid = ((editorAccountType.SelectedItem == null) ? (System.Guid.Empty) : ((CAccountType)editorAccountType.SelectedItem).ID);
                System.Guid Account_Guid = ((m_bNewObject == true) ? System.Guid.Empty : m_objSelectedAccount.ID);

                // проверка значений
                if (CAccountDatabaseModel.IsAllParametersValid( Bank_Guid, Currency_Guid, Account_Number, Account_Description, AccountType_Guid,
                    ref strErr) == true)
                {
                    if (m_bNewObject == true)
                    {
                        // новый счёт
                        bOkSave = CAccountDatabaseModel.AddNewObjectToDataBase( Bank_Guid, Currency_Guid, Account_Number, 
                            Account_Description, AccountType_Guid, ref Account_Guid, m_objProfile, ref strErr );

                        if (bOkSave == true)
                        {
                            m_objSelectedAccount.ID = Account_Guid;
                        }
                    }
                    else
                    {
                        bOkSave = CAccountDatabaseModel.EditObjectInDataBase( Account_Guid, Bank_Guid, Currency_Guid,
                            Account_Number, Account_Description, AccountType_Guid, m_objProfile, ref strErr );
                    }
                }

                if (bOkSave == true)
                {
                    m_objSelectedAccount.AccountNumber = Account_Number;
                    m_objSelectedAccount.Description = Account_Description;
                    m_objSelectedAccount.Bank = editorAccountBank.Properties.Items.Cast<CBank>().SingleOrDefault<CBank>(x => x.ID.Equals(Bank_Guid));
                    m_objSelectedAccount.Currency = editorAccountCurrency.Properties.Items.Cast<CCurrency>().SingleOrDefault<CCurrency>(x => x.ID.Equals(Currency_Guid));
                    m_objSelectedAccount.AccountType = editorAccountType.Properties.Items.Cast<CAccountType>().SingleOrDefault<CAccountType>(x => x.ID.Equals(Currency_Guid));
                }

                bRet = bOkSave;
            }
            catch (System.Exception f)
            {
                strErr = f.Message;
                SendMessageToLog("Ошибка сохранения изменений в расчётном счёте. Текст ошибки: " + f.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            return bRet;
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                System.String strErr = "";
                if( bSaveAccountPropertiesInDataBase( ref strErr ) == true )
                {
                    tabControl.SelectedTabPage = tabPageViewer;
                }
                else
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(strErr, "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }

            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в платеже. Текст ошибки: " + f.Message);
            }
            return;
        }

        #endregion

        #region Печать
        /// <summary>
        /// Экспорт журнала расчётных счетов в MS Excel
        /// </summary>
        /// <param name="strFileName">имя файла MS Excel</param>
        private void ExportToExcelAccountList(string strFileName)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                System.IO.FileInfo newFile = new System.IO.FileInfo(strFileName);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new System.IO.FileInfo(strFileName);
                }

                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(this.Text);

                    foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in gridViewAccountList.Columns)
                    {
                        if (objColumn.Visible == false) { continue; }

                        worksheet.Cells[1, objColumn.VisibleIndex + 1].Value = objColumn.Caption;
                    }

                    using (var range = worksheet.Cells[1, 1, 1, gridViewAccountList.Columns.Count])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Font.Size = 14;
                        //range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        //range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                        //range.Style.Font.Color.SetColor(Color.White);
                    }

                    System.Int32 iCurrentRow = 2;
                    System.Int32 iRowsCount = gridViewAccountList.RowCount;
                    for (System.Int32 i = 0; i < iRowsCount; i++)
                    {
                        foreach (DevExpress.XtraGrid.Columns.GridColumn objColumn in gridViewAccountList.Columns)
                        {
                            if (objColumn.Visible == false) { continue; }

                            worksheet.Cells[iCurrentRow, objColumn.VisibleIndex + 1].Value = gridViewAccountList.GetRowCellValue(i, objColumn);
                            if (objColumn.FieldName == "Date")
                            {
                                worksheet.Cells[iCurrentRow, objColumn.VisibleIndex + 1].Style.Numberformat.Format = "DD.MM.YYYY";
                            }
                        }
                        iCurrentRow++;
                    }

                    iCurrentRow--;
                    worksheet.Cells[1, 1, iCurrentRow, gridViewAccountList.Columns.Count].AutoFitColumns(0);
                    worksheet.Cells[1, 1, iCurrentRow, gridViewAccountList.Columns.Count].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[1, 1, iCurrentRow, gridViewAccountList.Columns.Count].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[1, 1, iCurrentRow, gridViewAccountList.Columns.Count].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[1, 1, iCurrentRow, gridViewAccountList.Columns.Count].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    worksheet.PrinterSettings.FitToWidth = 1;

                    worksheet = null;

                    package.Save();

                    try
                    {
                        using (System.Diagnostics.Process process = new System.Diagnostics.Process())
                        {
                            process.StartInfo.FileName = strFileName;
                            process.StartInfo.Verb = "Open";
                            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                            process.Start();
                        }
                    }
                    catch
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(this, "Системе не удалось найти приложение, чтобы открыть файл.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка экспорта в MS Excel.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        /// <summary>
        /// Экспорт информации по расётному счёту в MS Excel
        /// </summary>
        /// <param name="strFileName">имя файла MS Excel</param>
        private void ExportToExcelAccount(string strFileName)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                System.IO.FileInfo newFile = new System.IO.FileInfo(strFileName);
                if (newFile.Exists)
                {
                    newFile.Delete();
                    newFile = new System.IO.FileInfo(strFileName);
                }

                using (ExcelPackage package = new ExcelPackage(newFile))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(this.Text);
                    const System.Int32 iColumnIndxCaption = 1;
                    const System.Int32 iColumnIndxValue = 2;
                    System.Int32 iPropertiesCount = 0;

                    worksheet.Cells[System.Convert.ToInt32(lblAccountType.Tag), iColumnIndxCaption].Value = lblAccountType.Text;
                    worksheet.Cells[System.Convert.ToInt32(lblAccountType.Tag), iColumnIndxValue].Value = editorAccountType.Text;
                    iPropertiesCount++;

                    worksheet.Cells[System.Convert.ToInt32(lblAccountCurrency.Tag), iColumnIndxCaption].Value = lblAccountCurrency.Text;
                    worksheet.Cells[System.Convert.ToInt32(lblAccountCurrency.Tag), iColumnIndxValue].Value = editorAccountCurrency.Text;
                    iPropertiesCount++;

                    worksheet.Cells[System.Convert.ToInt32(lblAccountNumber.Tag), iColumnIndxCaption].Value = lblAccountNumber.Text;
                    worksheet.Cells[System.Convert.ToInt32(lblAccountNumber.Tag), iColumnIndxValue].Value = editorAccountNumber.Text;
                    iPropertiesCount++;

                    worksheet.Cells[System.Convert.ToInt32(lblEarningBank.Tag), iColumnIndxCaption].Value = lblEarningBank.Text;
                    worksheet.Cells[System.Convert.ToInt32(lblEarningBank.Tag), iColumnIndxValue].Value = editorAccountBank.Text; 
                    iPropertiesCount++;

                    worksheet.Cells[System.Convert.ToInt32(lblAccountDecrpn.Tag), iColumnIndxCaption].Value = lblAccountDecrpn.Text;
                    worksheet.Cells[System.Convert.ToInt32(lblAccountDecrpn.Tag), iColumnIndxValue].Value = editorAccountDescrpn.Text;
                    iPropertiesCount++;

                    using (var range = worksheet.Cells[1, iColumnIndxCaption, iPropertiesCount, iColumnIndxCaption])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Font.Size = 12;
                        //range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        //range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
                        //range.Style.Font.Color.SetColor(Color.White);
                    }

                    worksheet.Cells[1, iColumnIndxCaption, iPropertiesCount, iColumnIndxValue].AutoFitColumns(0);

                    worksheet.Cells[1, iColumnIndxCaption, iPropertiesCount, iColumnIndxValue].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[1, iColumnIndxCaption, iPropertiesCount, iColumnIndxValue].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[1, iColumnIndxCaption, iPropertiesCount, iColumnIndxValue].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[1, iColumnIndxCaption, iPropertiesCount, iColumnIndxValue].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                    worksheet.PrinterSettings.FitToWidth = 1;

                    worksheet = null;

                    package.Save();

                    try
                    {
                        using (System.Diagnostics.Process process = new System.Diagnostics.Process())
                        {
                            process.StartInfo.FileName = strFileName;
                            process.StartInfo.Verb = "Open";
                            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                            process.Start();
                        }
                    }
                    catch
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(this, "Системе не удалось найти приложение, чтобы открыть файл.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка экспорта в MS Excel.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void barbtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                ExportToExcelAccountList(String.Format("{0}{1}.xlsx", System.IO.Path.GetTempPath(), this.Text));
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnPrint_Click. Текст ошибки: " + f.Message);
            }

            return;
        }
        private void btnPrintAccountProperties_Click(object sender, EventArgs e)
        {
            try
            {
                ExportToExcelAccount(String.Format("{0}{1}.xlsx", System.IO.Path.GetTempPath(), "расчётный счёт"));
            }
            catch (System.Exception f)
            {
                SendMessageToLog("btnPrint_Click. Текст ошибки: " + f.Message);
            }

            return;
        }
        #endregion

        #region Подтверждение/отмена выбора расчётного счета в списке
        private void btnConfirmSelection_Click(object sender, EventArgs e)
        {
            try
            {
                m_objSelectedAccount = GetSelectedAccount();
                if (SelectedAccount == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Укажите, пожалуйста, расчётный счёт в списке", "Внимание",
                       System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
                else
                {
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                    Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "btnConfirmSelection_Click.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        private void btnCancelSelection_Click(object sender, EventArgs e)
        {
            try
            {
                m_objSelectedAccount = null;
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                Close();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "btnCancelSelection_Click.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        
        #endregion


    }
}
