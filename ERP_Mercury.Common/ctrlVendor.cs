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
    public partial class ctrlVendor : UserControl
    {
        #region Свойства, переменные
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;

        private CVendor m_objSelectedVendor;

        private System.Boolean m_bIsChanged;

        public ctrlAddress frmAddress;
        public ctrlContact frmContact;
      

        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemAddress;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemContact;
        //private DevExpress.XtraLayout.LayoutControlItem layoutControlItemRtt;

        private System.Boolean m_bDisableEvents;
        private System.Boolean m_bNewVendor;
        private System.Boolean m_bIsReadOnly;

        private const System.Int32 iMinControlItemHeight = 20;
        private const System.Int32 iPanel1WidthDef = 350;

        #endregion

        #region События
        // Создаем закрытое поле, ссылающееся на заголовок списка делегатов
        private EventHandler<ChangeVendorPropertieEventArgs> m_ChangeVendorProperties;
        // Создаем в классе член-событие
        public event EventHandler<ChangeVendorPropertieEventArgs> ChangeVendorProperties
        {
            add
            {
                // берем закрытую блокировку и добавляем обработчик
                // (передаваемый по значению) в список делегатов
                m_ChangeVendorProperties += value;
            }
            remove
            {
                // берем закрытую блокировку и удаляем обработчик
                // (передаваемый по значению) из списка делегатов
                m_ChangeVendorProperties -= value;
            }
        }
        /// <summary>
        /// Инициирует событие и уведомляет о нем зарегистрированные объекты
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnChangeVendorProperties(ChangeVendorPropertieEventArgs e)
        {
            // Сохраняем поле делегата во временном поле для обеспечение безопасности потока
            EventHandler<ChangeVendorPropertieEventArgs> temp = m_ChangeVendorProperties;
            // Если есть зарегистрированные объектв, уведомляем их
            if (temp != null) temp(this, e);
        }
        public void SimulateChangeVendorProperties(CVendor objVendor, enumActionSaveCancel enActionType, System.Boolean bIsNewVendor)
        {
            // Создаем объект, хранящий информацию, которую нужно передать
            // объектам, получающим уведомление о событии
            ChangeVendorPropertieEventArgs e = new ChangeVendorPropertieEventArgs(objVendor, enActionType, bIsNewVendor);

            // Вызываем виртуальный метод, уведомляющий наш объект о возникновении события
            // Если нет типа, переопределяющего этот метод, наш объект уведомит все объекты, 
            // подписавшиеся на уведомление о событии
            OnChangeVendorProperties(e);
        }
        #endregion

        #region Конструктор
        public ctrlVendor(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem)
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_bIsChanged = false;
            m_bDisableEvents = false;
            m_bNewVendor = false;

            m_objSelectedVendor = null;


            frmAddress = new ERP_Mercury.Common.ctrlAddress(ERP_Mercury.Common.EnumObject.Vendor, m_objProfile, m_objMenuItem, System.Guid.Empty);
            frmAddress.InitAddressControl();

            frmContact = new ERP_Mercury.Common.ctrlContact(ERP_Mercury.Common.EnumObject.Vendor, m_objProfile, m_objMenuItem, System.Guid.Empty);
            
            //frmRtt = new ctrlRtt(m_objProfile, m_objMenuItem);

            layoutControlAddress.Size = new Size(layoutControlAddress.Size.Width, (frmAddress.Size.Height));
            layoutControlAddress.MaximumSize = new Size(layoutControlAddress.Size.Width, layoutControlAddress.Size.Height);

            layoutControlContact.Size = new Size(layoutControlContact.Size.Width, (frmContact.Size.Height));
            layoutControlContact.MaximumSize = new Size(layoutControlContact.Size.Width, layoutControlContact.Size.Height);

            //layoutControlRtt.Size = new Size(layoutControlRtt.Size.Width, (frmRtt.Size.Height));
            //layoutControlRtt.MaximumSize = new Size(layoutControlRtt.Size.Width, layoutControlRtt.Size.Height);

            layoutControlGroupAddress.Size = new Size(layoutControlGroupAddress.Size.Width, (frmAddress.Size.Height));
            layoutControlGroupConact.Size = new Size(layoutControlGroupConact.Size.Width, (frmContact.Size.Height));
            //layoutControlGroupRtt.Size = new Size(layoutControlGroupRtt.Size.Width, (frmRtt.Size.Height));

            
            this.layoutControlItemAddress = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemAddress.Parent = layoutControlGroupAddress;
            this.layoutControlItemAddress.Control = frmAddress;
            this.layoutControlItemAddress.CustomizationFormText = "layoutControlItemAddress";
            this.layoutControlItemAddress.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItemAddress.Name = "layoutControlItemAddress";
            this.layoutControlItemAddress.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItemAddress.Size = new System.Drawing.Size(642, 38);
            this.layoutControlItemAddress.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlItemAddress.Text = "layoutControlItemAddress";
            this.layoutControlItemAddress.TextSize = new System.Drawing.Size(93, 20);
            this.layoutControlItemAddress.TextVisible = false;
            this.frmAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));


            this.layoutControlItemContact = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemContact.Parent = layoutControlGroupConact;
            this.layoutControlItemContact.Control = frmContact;
            this.layoutControlItemContact.CustomizationFormText = "layoutControlItemContact";
            this.layoutControlItemContact.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItemContact.Name = "layoutControlItemContact";
            this.layoutControlItemContact.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItemContact.Size = new System.Drawing.Size(642, 38);
            this.layoutControlItemContact.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlItemContact.Text = "layoutControlItemContact";
            this.layoutControlItemContact.TextSize = new System.Drawing.Size(93, 20);
            this.layoutControlItemContact.TextVisible = false;
            this.frmContact.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));

            frmAddress.ChangeAddressPropertie += OnChangeAddressPropertie;

            frmContact.ChangeContactProperties += OnChangeContactPropertie;

            //frmRtt.ChangeRttProperties += OnChangeRttPropertie;
            //frmRtt.ChangeControlRttSize += OnChangeControlRttSize;

            // Закоментировано из-за ошибки Ошибка редактирования свойств клиента Cant find resize tree node
            /*
            layoutControlGroupAddress.Expanded = true;
            layoutControlGroupConact.Expanded = true;
            //layoutControlGroupRtt.Expanded = true;
            layoutControlGroupAdvProperties.Expanded = true;
            */
            LoadComboBoxItems();
            m_bIsReadOnly = false;
            
            CheckClientsRight();
        }
        #endregion

        #region Выпадающие списки
        /// <summary>
        /// Проверка динамических прав
        /// </summary>
        private void CheckClientsRight()
        {
            try
            {
                if (m_objProfile.GetClientsRight().GetState(ERP_Mercury.Global.Consts.strDR_EditVendorCard) == false) 
                {
                    btnEdit.Visible = false;
                    btnPrint.Visible = false;
                    btnSave.Visible = false;
                }
                else
                {
                    btnEdit.Visible = true;
                    //btnPrint.Visible = true;
                    btnSave.Visible = true;
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


        /// <summary>
        /// Обновление выпадающих списков
        /// </summary>
        /// <returns>true - все списки успешно обновлены; false - ошибка</returns>
        public System.Boolean LoadComboBoxItems()
        {
            System.Boolean bRet = false;
            try
            {
                // типы расчетных счетов
                repItemcboxAccountType.Items.Clear();
                List<CAccountType> objAccountTypeList = CAccountType.GetAccountTypeList(m_objProfile, null);
                if (objAccountTypeList != null)
                {
                    repItemcboxAccountType.Items.AddRange(objAccountTypeList);
                }

                // банки
                repItemcboxAccountBank.Items.Clear();
                List<CBank> objBankList = CBank.GetBankList(m_objProfile, null, null);
                if (objBankList != null)
                {
                    repItemcboxAccountBank.Items.AddRange(objBankList);
                }

                // валюты
                repItemcboxAccountCurrency.Items.Clear();
                List<CCurrency> objCurrencyList = CCurrency.GetCurrencyList(m_objProfile, null);
                if (objCurrencyList != null)
                {
                    repItemcboxAccountCurrency.Items.AddRange(objCurrencyList);
                }

                // тип хранилища
                cboxTypeVendor.Properties.Items.Clear();
                List<CVendorType> objCVendorTypeList = CVendorType.GetVendorTypeList(m_objProfile, null); //List<CDistributionNetwork> objDistributionNetworkList = CDistributionNetwork.GetDistributionNetworkList(m_objProfile, null);
                if (objCVendorTypeList != null)
                {
                    cboxTypeVendor.Properties.Items.AddRange(objCVendorTypeList);
                }

                //objDistributionNetworkList = null;

                //2011.06.03 if (frmAddress != null) { frmAddress.InitAllLists(); }
                if (frmContact != null) { frmContact.LoadComboBoxItems(); }
                //if (frmRtt != null) { frmRtt.LoadComboBoxItems(); }

                bRet = true;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка обновления выпадающих списков.\n Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return bRet;
        }

        #endregion

        #region Индикация изменений
        
        private void SetPropertiesModified(System.Boolean bModified)
        {
            //ValidateProperties();
            //if (m_bIsChanged == bModified) { return; }
            m_bIsChanged = bModified;
            btnSave.Enabled = m_bIsChanged;
            btnCancel.Enabled = btnSave.Enabled;
            if (m_bIsChanged == true)
            {
                SimulateChangeVendorProperties(m_objSelectedVendor, enumActionSaveCancel.Unkown, m_bNewVendor);
            }
        }

        #endregion

        #region События и обработчики
        private void OnChangeAddressPropertie(Object sender, ERP_Mercury.Common.ChangeAddressPropertieEventArgs e)
        {
            try
            {
                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "OnChangeAddressPropertie.\n Текст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally // очищаем занимаемые ресурсы
            {
            }

            return;
        }
        private void OnChangeContactPropertie(Object sender, ERP_Mercury.Common.ChangeContactPropertieEventArgs e)
        {
            try
            {
                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "OnChangeContactPropertie.\n Текст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally // очищаем занимаемые ресурсы
            {
            }

            return;
        }
        #endregion

        #region Расчетные счета
        /// <summary>
        /// Добавляет в список расчетный счет
        /// </summary>
        private void AddAccount()
        {
            try
            {
                if (treeListAccounts.Enabled == false) { treeListAccounts.Enabled = true; }
                if (m_objSelectedVendor.AccountList == null) { m_objSelectedVendor.AccountList = new List<CAccount>(); }
                System.Boolean bNotFullNode = false;
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objItem in treeListAccounts.Nodes)
                {
                    if ((System.String)objItem.GetValue(colAccountNumber) == "")
                    {
                        treeListAccounts.FocusedNode = objItem;
                        bNotFullNode = true;
                        break;
                    }
                }
                if (bNotFullNode == true) { return; }

                CAccount objAccount = new CAccount();

                objAccount.IsMain = false;
                DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListAccounts.AppendNode(new object[] { objAccount.AccountType, objAccount.AccountNumber, objAccount.Currency, objAccount.Bank, objAccount.Description, objAccount.IsMain }, null);
                objNode.Tag = objAccount;
                treeListAccounts.FocusedNode = objNode;
                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка добавления расчетного счета в список. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// Удаляет расчетный счет из списка
        /// </summary>
        /// <param name="objNode">удаляемый узел в дереве</param>
        private void DeleteAccount(DevExpress.XtraTreeList.Nodes.TreeListNode objNode)
        {
            try
            {
                if ((objNode == null) || (treeListAccounts.Nodes.Count == 0)) { return; }

                if (m_objSelectedVendor.AccountForDeleteList == null) { m_objSelectedVendor.AccountForDeleteList = new List<CAccount>(); }
                DevExpress.XtraTreeList.Nodes.TreeListNode objPrevNode = objNode.PrevNode;
                m_objSelectedVendor.AccountForDeleteList.Add((CAccount)objNode.Tag);

                treeListAccounts.Nodes.Remove(objNode);
                if (objPrevNode == null)
                {

                    if (treeListAccounts.Nodes.Count > 0)
                    {
                        treeListAccounts.FocusedNode = treeListAccounts.Nodes[0];
                    }
                }
                else
                {
                    treeListAccounts.FocusedNode = objPrevNode;
                }

                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка удаления расчетного счета. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void mitemAddAccount_Click(object sender, EventArgs e)
        {
            try
            {
                AddAccount();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("mitemAddAccount_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void mitemDeleteAccount_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteAccount(treeListAccounts.FocusedNode);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("mitemDeleteAccount_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void treeListAccounts_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (m_bIsReadOnly == true) { return; }
                if (e.Button == MouseButtons.Right)
                {
                    // попробуем определить, что же у нас под мышкой
                    DevExpress.XtraTreeList.TreeListHitInfo hi = ((DevExpress.XtraTreeList.TreeList)sender).CalcHitInfo(new Point(e.X, e.Y));
                    if ((hi == null) || (hi.Node == null))
                    {
                        mitemDeleteAccount.Enabled = false;
                    }
                    else
                    {
                        // выделяем узел
                        mitemDeleteAccount.Enabled = true;
                        hi.Node.TreeList.FocusedNode = hi.Node;
                    }
                    contextMenuStripAccount.Show(((DevExpress.XtraTreeList.TreeList)sender), new Point(e.X, e.Y));
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListAccounts_MouseClick. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        /// <summary>
        /// выполняет проверку номера расчетного счета
        /// </summary>
        /// <param name="strAccount">номер лицензии</param>
        /// <returns>true - ошибок нет; false - номер расчетного счета не соответсвует установленным требованиям</returns>
        private System.Boolean IsAccountValid(System.String strAccount)
        {
            System.Boolean bRet = false;
            try
            {
                if (strAccount.Trim() == "") { return bRet; }
                
                // по правильности здесь будет регулярное выражение
                bRet = true;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка проверки номера расчетного счета. Номер: " + strAccount + ". Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return bRet;
        }
        /// <summary>
        /// Проверяет, дублируется ли номер расченого счета в списке
        /// </summary>
        /// <param name="strAccount">номер расченого счета</param>
        /// <param name="iAccountPos">позиция расченого счета в списке</param>
        /// <returns>true - дублируется; false - не ублируется</returns>
        private System.Boolean IsAccountDublicate(System.String strAccount, System.Int32 iAccountPos)
        {
            System.Boolean bRet = false;

            try
            {
                // проверим, возможно такой номер р/с уже есть в списке
                System.Boolean bDublicate = false;
                for (System.Int32 i2 = 0; i2 < treeListAccounts.Nodes.Count; i2++)
                {
                    if (i2 != iAccountPos)
                    {
                        if (((System.String)treeListAccounts.Nodes[i2].GetValue(colAccountNumber)) == strAccount)
                        {
                            bDublicate = true;
                            break;
                        }
                    }
                }

                bRet = bDublicate;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка проверки номера расченого счета. Номер: " + strAccount + ". Текст ошибки: " + f.Message);
            }

            return bRet;
        }

        /// <summary>
        /// Проверяет, дублируется ли комбинация из номер р/с, валюты, банка в списке
        /// </summary>
        private System.Boolean IsAccountFullDublicate()
        {
            System.Boolean bRet = false;
            int i, j;
            try
            {
                System.Boolean bDublicate = false;
                for (i = 0; i < treeListAccounts.Nodes.Count; i++)
                    for (j = i + 1; j < treeListAccounts.Nodes.Count; j++)
                    {
                        if (i != treeListAccounts.Nodes.Count - 1)
                        {
                            if (((System.String)treeListAccounts.Nodes[i].GetValue(colAccountNumber)) == ((System.String)treeListAccounts.Nodes[j].GetValue(colAccountNumber)))
                            {
                                if (treeListAccounts.Nodes[i].GetValue(colAccountCurrency).ToString() == treeListAccounts.Nodes[j].GetValue(colAccountCurrency).ToString())
                                {
                                    if (treeListAccounts.Nodes[i].GetValue(colAccountBank).ToString() == treeListAccounts.Nodes[j].GetValue(colAccountBank).ToString())
                                    {
                                        //MessageBox.Show("Дубликат р/с, валюты и кодов банка");
                                        bDublicate = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                bRet = bDublicate;
            }
            catch (Exception f)
            {
                SendMessageToLog("Ошибка проверки уникальность комбинации номера р/с, валюты, банка." + "Текст ошибки: " + f.Message);
            }

            return bRet;
        }

        /// <summary>
        /// Проверяет, установлен ли хоть один р/с, в качестве основного
        /// </summary>
        private System.Boolean IsAccountOneIsMain()
        {
            System.Boolean bRet = false;
            int i, j=0;
            try
            {
                System.Boolean bDublicate = false;
                for (i = 0; i < treeListAccounts.Nodes.Count; i++)
                {
                    if (treeListAccounts.Nodes[i].GetValue(colAccountIsMain).ToString() != "True")
                    {
                        j++;
                    }
                }
                if (treeListAccounts.Nodes.Count==j)
                {
                    // ни один р/с не установлен в качестве основного.
                    bDublicate = true;
                }
                
                bRet = bDublicate;
            }
            catch (Exception f)
            {
                SendMessageToLog("Ошибка проверки установлен ли хоть один номер р/с основным" + "Текст ошибки: " + f.Message);
            }

            return bRet;
        }


        private void treeListAccounts_InvalidNodeException(object sender, DevExpress.XtraTreeList.InvalidNodeExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
        }
        private void treeListAccounts_ValidateNode(object sender, DevExpress.XtraTreeList.ValidateNodeEventArgs e)
        {
            if (m_bDisableEvents == true) { return; }
            try
            {
                if (e.Node[colAccountType] == null)
                {
                    e.Valid = false;
                    treeListAccounts.SetColumnError(/*colLicenceNum*/ colAccountType , "Необходимо указать тип расчетного счета.");
                }
                if ((e.Node[colAccountNumber] == null) || (IsAccountValid((System.String)e.Node[colAccountNumber]) == false))
                {
                    e.Valid = false;
                    treeListAccounts.SetColumnError(/*colLicenceNum*/ colAccountNumber, "Номер расчетного счета не соответсвует принятым требованиям.");
                }
                else
                {
                    /*if (IsAccountDublicate((System.String)e.Node[colAccountNumber], treeListAccounts.GetNodeIndex(e.Node)) == true)
                    {
                        //e.Valid = false;
                        //treeListAccounts.SetColumnError(colAccountNumber, "Такой номер расчетного счета уже есть в списке.");
                    }*/



                    // 19.12.2011
                    /* 
                    if (IsAccountOneIsMain())
                    {
                        e.Valid = false;
                        treeListAccounts.SetColumnError(colAccountIsMain, "Один из р/с должен быть основным");
                    }
                    */

                    if (IsAccountFullDublicate() )
                    {
                        e.Valid = false;
                        treeListAccounts.SetColumnError(colAccountNumber, "Такой номер расчетного счета уже есть в списке.");
                        treeListAccounts.SetColumnError(colAccountCurrency, "Такая валюта уже есть в списке");
                        treeListAccounts.SetColumnError(colAccountBank, "Такой банк уже есть в списке");
                    }
                    else
                    {
                        SetPropertiesModified(true);
                    }
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListAccounts_ValidateNode. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void treeListAccounts_CellValueChanging(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
            // написать обработчик здесь. Скоприровать код из  treeListAccounts_CellValueChanded и дополнить своим
            if (m_bDisableEvents == true) { return; }
            try
            {
                System.Int32 iPosNode = treeListAccounts.GetNodeIndex(e.Node);

                if ((e.Column == colAccountNumber) && (e.Value != null))
                {
                    if (IsAccountValid((System.String)e.Value) == false)
                    {
                        treeListAccounts.SetColumnError(colAccountNumber, "Номер расчетного счета не соответсвует принятым требованиям.");
                    }
                    else
                    {
                        // проверим, возможно такой номер р/с уже есть в списке
                        /* if (IsAccountDublicate((System.String)e.Value, iPosNode) == true)
                         {

                             //treeListAccounts.SetColumnError(colAccountNumber, "Такой номер расчетного счета уже есть в списке.");
                         }
                         else
                         {*/
                        ((CAccount)e.Node.Tag).AccountNumber = (System.String)e.Value;
                        //}
                    }
                }

                if ((e.Column == colAccountIsMain) && 
                    (e.Value != null) && 
                    (((System.Boolean)e.Value) == true))
                {
                    // главным може быть только один р/с,
                    // поэтому все остальные нужно сделать неглавными
                    for (System.Int32 i = 0; i < treeListAccounts.Nodes.Count; i++)
                    {
                        if (i != iPosNode)
                        {
                            treeListAccounts.Nodes[i].SetValue(colAccountIsMain, false);
                            ((CAccount)treeListAccounts.Nodes[i].Tag).IsMain = false;
                            e.Node.SetValue(colAccountIsMain, e.Value);
                        }
                    }
                    //m_objSelectedContact.PhoneList[iPosNode].IsMain = true;
                    ((CAccount)e.Node.Tag).IsMain = true;
                    e.Node.SetValue(colAccountIsMain, true);
                }

                if ((e.Column == colAccountIsMain) && (e.Value != null) && (((System.Boolean)e.Value) == false))
                {
                    ((CAccount)e.Node.Tag).IsMain = false;
                    e.Node.SetValue(colAccountIsMain, false);
                }


                if ((e.Column == colAccountDscrpn) && (e.Value != null))
                {
                    if (((System.String)e.Value).Trim().Length > 0)
                    {
                        ((CAccount)e.Node.Tag).Description = ((System.String)e.Value).Trim();
                    }
                }

                if ((e.Column == colAccountCurrency) && (e.Value != null))
                {
                    ((CAccount)e.Node.Tag).Currency = (CCurrency)e.Value;
                }

                if ((e.Column == colAccountType) && (e.Value != null))
                {
                    ((CAccount)e.Node.Tag).AccountType = (CAccountType)e.Value;
                }
                if ((e.Column == colAccountBank) && (e.Value != null))
                {
                    ((CAccount)e.Node.Tag).Bank = (CBank)e.Value;
                }

                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListAccounts_CellValueChanged. Текст ошибки: " + f.Message);
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
        private void SetWarningInfo(System.String strMessage)
        {
            try
            {
                lblWarningInfo.Text = strMessage;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "SetWarningInfo.\n Текст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
        private void ShowWarningPnl(System.Boolean bShow)
        {
            System.Int32 iRowWarnigHeith = 45;
            System.Int32 iRowBtnHeith = 30;
            try
            {
                if (bShow == true)
                {
                    tableLayoutPanel4.RowStyles[0].Height = iRowWarnigHeith;
                }
                else
                {
                    tableLayoutPanel4.RowStyles[0].Height = 0;
                }
                tableLayoutPanel4.Size = new Size(tableLayoutPanel4.Size.Width,
                    (System.Convert.ToInt32(tableLayoutPanel4.RowStyles[0].Height + iRowBtnHeith)));
                tableLayoutPanel4.Refresh();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "ShowWarningPnl.\n Текст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Редактировать поставщика
        /// <summary>
        /// Загружает свойства поставщика для редактирования/подробного просмотра
        /// </summary>
        /// <param name="objVendor">компания</param
        public void EditVendor(ERP_Mercury.Common.CVendor objVendor)
        {
            if (objVendor == null) { return; }
            m_bDisableEvents = true;
            m_bNewVendor = false;
            ShowWarningPnl(false);
            try
            {
                System.String strErr = "";
                m_objSelectedVendor = objVendor;

                m_objSelectedVendor.AccountList = CAccount.GetAccountListForVendor(m_objProfile, null, m_objSelectedVendor.ID, ref strErr);
                
                // ЗДЕСЬ !!!!!!!!!!!!!!!!!!!!!
                // Заполняем не здесь, а ниже. Здесь (в этом блоке и чуть выше) заполняются combobox-ы,
                // которые НЕ возвращаются из конструкотора. А заполняются те, которые имеют тип данных List <>
                //m_objSelectedVendor.CompanyType = CCompanyType.GetCompanyTypeListForCompany(m_objProfile, null, m_objSelectedVendor.ID); // написать метод GetCompanyTypeListForCompany)
                
                if (m_objSelectedVendor.ContactForDeleteList == null) { m_objSelectedVendor.ContactForDeleteList = new List<CContact>();}
                else { m_objSelectedVendor.ContactForDeleteList.Clear(); }
                if (m_objSelectedVendor.AddressForDeleteList == null) { m_objSelectedVendor.AddressForDeleteList = new List<CAddress>(); }
                else { m_objSelectedVendor.AddressForDeleteList.Clear(); }
                if (m_objSelectedVendor.AccountForDeleteList == null) { m_objSelectedVendor.AccountForDeleteList = new List<CAccount>(); }
                else { m_objSelectedVendor.AccountForDeleteList.Clear(); }
               
                this.SuspendLayout();

                txtFullName.Text = "";
                txtCod.Text = "";
                txtDescription.Text = "";
                cboxTypeVendor.SelectedItem = null;
                // checkActive.Checked инициализировать false не нужно, так как он всё равно перезаписвывается ниже
               
                treeListAccounts.Nodes.Clear();
                
                //checkListBoxTargetBuy.Items.Clear(); 

                txtFullName.Text = m_objSelectedVendor.Name;
                txtCod.Text = Convert.ToString(m_objSelectedVendor.ID_Ib);
                txtDescription.Text = m_objSelectedVendor.Description;

                checkActive.Checked = m_objSelectedVendor.IsActive;

                // Тип поставщика
                if ((m_objSelectedVendor.TypeVendor != null) && (cboxTypeVendor.Properties.Items.Count > 0))
                {
                    foreach (Object objVendorTypes in cboxTypeVendor.Properties.Items)
                    {
                        if (((CVendorType)objVendorTypes).ID.CompareTo(m_objSelectedVendor.TypeVendor.ID) == 0)
                        {
                            cboxTypeVendor.SelectedItem = objVendorTypes;  //((CCompanyType)objCompanyTypes).Name; //objCompanyTypes;
                            break;
                        }
                    }
                }
                
                // Расчетные счета
                if (m_objSelectedVendor.AccountList != null)
                {
                    foreach (CAccount objAccount in m_objSelectedVendor.AccountList)
                    {
                        DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListAccounts.AppendNode(new object[] { objAccount.AccountType, objAccount.AccountNumber, objAccount.Currency, objAccount.Bank, objAccount.Description, objAccount.IsMain }, null);
                        objNode.Tag = objAccount;
                    }
                }

                // Адреса
                frmAddress.LoadAddressList(m_objSelectedVendor.ID, null);
                // Контакты
                frmContact.LoadContactList(m_objSelectedVendor.ID, m_objSelectedVendor.ContactList);
                
                frmAddress.ClearAllPropertiesControls();
                frmContact.ClearAllPropertiesControls();

                SetPropertiesModified(false);
                //tabControl.SelectedTabPage = tabGeneral;
                btnCancel.Enabled = true;
                btnCancel.Focus();

                SetModeReadOnly(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования компании. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.ResumeLayout(false);

                //tabControl.SelectedTabPage = tabGeneral;
                m_bDisableEvents = false;
            }
            return;
        }
        #endregion


        #region Новый поставщик
        /// <summary>
        /// Новый клиент
        /// </summary>
        public void NewVendor()
        {
            m_bDisableEvents = true;
            m_bNewVendor = true;
            ShowWarningPnl(false);
            try
            {
                this.Refresh();
                frmAddress.StartThreadWithLoadData();
                frmContact.frmAddress.StartThreadWithLoadData();

                m_objSelectedVendor = new ERP_Mercury.Common.CVendor();
                if (m_objSelectedVendor.ContactForDeleteList == null) { m_objSelectedVendor.ContactForDeleteList = new List<CContact>(); }
                else { m_objSelectedVendor.ContactForDeleteList.Clear(); }
                if (m_objSelectedVendor.AddressForDeleteList == null) { m_objSelectedVendor.AddressForDeleteList = new List<CAddress>(); }
                else { m_objSelectedVendor.AddressForDeleteList.Clear(); }
                /* if (m_objSelectedVendor.PhoneForDeleteList == null) { m_objSelectedVendor.PhoneForDeleteList = new List<CPhone>(); }
                else { m_objSelectedVendor.PhoneForDeleteList.Clear(); }
                if (m_objSelectedVendor.LicenceForDeleteList == null) { m_objSelectedVendor.LicenceForDeleteList = new List<CLicence>(); }
                else { m_objSelectedVendor.LicenceForDeleteList.Clear(); } */
                if (m_objSelectedVendor.AccountForDeleteList == null) { m_objSelectedVendor.AccountForDeleteList = new List<CAccount>(); }
                else { m_objSelectedVendor.AccountForDeleteList.Clear(); }
                /*if (m_objSelectedVendor.EMailForDeleteList == null) { m_objSelectedVendor.EMailForDeleteList = new List<CEMail>(); }
                else { m_objSelectedVendor.EMailForDeleteList.Clear(); }*/

                this.SuspendLayout();

                txtFullName.Text = "";
                txtDescription.Text = "";
                txtCod.Text = "";
                checkActive.Checked = true;
                cboxTypeVendor.SelectedItem = null;
                
                treeListAccounts.Nodes.Clear();
                //treeListEMail.Nodes.Clear();
                
                //checkListBoxTargetBuy.Items.Clear();

                txtFullName.Text = m_objSelectedVendor.Name;
                txtDescription.Text = m_objSelectedVendor.Description;

                // Адреса
                frmAddress.LoadAddressList(m_objSelectedVendor.ID, null);
                // Контакты
                frmContact.LoadContactList(m_objSelectedVendor.ID, m_objSelectedVendor.ContactList);


                // Проверить в отладке, а пока временно закоментированно 12.12.11
                // Адреса
                //frmAddress.LoadAddressList(m_objSelectedVendor.ID, null);
                // Контакты
                //frmContact.LoadContactList(m_objSelectedVendor.ID, m_objSelectedVendor.ContactList);
               

                frmAddress.ClearAllPropertiesControls();
                frmContact.ClearAllPropertiesControls();

                SetModeReadOnly(false);
                btnEdit.Enabled = false;
                SetPropertiesModified(true);

            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка создания клиента. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.ResumeLayout(false);
                m_bDisableEvents = false;
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
                SendMessageToLog("Ошибка отмены изменений в описании поставщика. Текст ошибки: " + f.Message);
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
                SimulateChangeVendorProperties(m_objSelectedVendor, enumActionSaveCancel.Cancel, m_bNewVendor);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка отмены изменений в описании поставщика. Текст ошибки: " + f.Message);
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
            System.Boolean bOkSave = false;
            CVendor objVendorForSave = new CVendor();
            try
            {
                objVendorForSave.ID = m_objSelectedVendor.ID;
                objVendorForSave.Name = txtFullName.Text;
                objVendorForSave.Description = txtDescription.Text;

                // проверить, как работатет этот кусочек кода
                objVendorForSave.IsActive = checkActive.Checked;
                
                if (txtCod.Text == "")
                {
                    objVendorForSave.ID_Ib = 0;
                }
                else
                {
                    objVendorForSave.ID_Ib = Convert.ToInt32(txtCod.Text); // заглушка на случай ввода Id для IB вручную
                }

                if (cboxTypeVendor.SelectedItem != null)
                {
                    objVendorForSave.TypeVendor = (CVendorType)cboxTypeVendor.SelectedItem;
                }


                if (objVendorForSave.AccountList == null) { objVendorForSave.AccountList = new List<CAccount>(); }
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListAccounts.Nodes)
                {
                    if (objNode.Tag == null) { continue; }
                    objVendorForSave.AccountList.Add((CAccount)objNode.Tag);
                }
                objVendorForSave.AccountForDeleteList = m_objSelectedVendor.AccountForDeleteList;

                //---
                objVendorForSave.ContactList = frmContact.ContactList;
                objVendorForSave.ContactForDeleteList = frmContact.ContactDeletedList;
                //---

                if (objVendorForSave.AddressList == null) { objVendorForSave.AddressList = new List<CAddress>(); }
                objVendorForSave.AddressList.Clear();
                objVendorForSave.AddressList.AddRange(frmAddress.AddressList);
                // возможно здесь стоит добавть строчку, которая закоментирована ниже
                //objVendorForSave.AddressList.AddRange(frmContact.frmAddress.AddressList);
                
                if (objVendorForSave.AddressForDeleteList == null) { objVendorForSave.AddressForDeleteList = new List<CAddress>(); }
                objVendorForSave.AddressForDeleteList.Clear();
                objVendorForSave.AddressForDeleteList.AddRange(frmAddress.AddressDeletedList);
                objVendorForSave.AddressForDeleteList.AddRange(frmContact.frmAddress.AddressDeletedList);
                

                System.String strErr = "";
                if (m_bNewVendor == true)
                {
                    // новый поставщик
                    // !!!!!!! остановился здесь 12.12 !!!!!!!!!!
                    bOkSave = objVendorForSave.Add(m_objProfile, null, ref strErr);
                }
                else
                {
                    //  Update в CVendor
                    // дописать потом
                    bOkSave = objVendorForSave.Update(m_objProfile, null, ref strErr);
                }
                SendMessageToLog(strErr);
                if (bOkSave == true)
                {

                    m_objSelectedVendor.ID = objVendorForSave.ID;
                    m_objSelectedVendor.Name = objVendorForSave.Name;
                    m_objSelectedVendor.Description = objVendorForSave.Description;

                    m_objSelectedVendor.TypeVendor = objVendorForSave.TypeVendor;
                    m_objSelectedVendor.IsActive = objVendorForSave.IsActive;

                    // проверить как работает эта строка
                    m_objSelectedVendor.AccountList = objVendorForSave.AccountList;

                    m_objSelectedVendor.ContactList = objVendorForSave.ContactList;
                    m_objSelectedVendor.AddressList = objVendorForSave.AddressList;
                   

                    if (frmAddress.AddressDeletedList != null) { frmAddress.AddressDeletedList.Clear(); }
                    if (frmContact.ContactDeletedList != null) { frmContact.ContactDeletedList.Clear(); }

                    if (m_objSelectedVendor.AddressForDeleteList != null) { m_objSelectedVendor.AddressForDeleteList.Clear(); }
                    if (m_objSelectedVendor.ContactForDeleteList != null) { m_objSelectedVendor.ContactForDeleteList.Clear(); }

                    if (m_objSelectedVendor.AccountForDeleteList != null) { m_objSelectedVendor.AccountForDeleteList.Clear(); }

                    ShowWarningPnl(false);
                    bRet = true;
                }
                else
                {
                    SetWarningInfo(strErr);
                    ShowWarningPnl(true);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в описании хранилища. Текст ошибки: " + f.Message);
            }
            finally
            {
                objVendorForSave = null;
            }
            return bRet;
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
                txtFullName.Properties.ReadOnly = bSet;
                txtDescription.Properties.ReadOnly = bSet;
                checkActive.Properties.ReadOnly = bSet;

                cboxTypeVendor.Properties.ReadOnly = bSet;

                treeListAccounts.OptionsBehavior.Editable = !bSet;

                frmAddress.SetChanceEditProperties(!bSet);
                frmContact.SetChanceEditProperties(!bSet);

                m_bIsReadOnly = bSet;
                btnEdit.Enabled = bSet;
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
                this.Refresh();
                frmAddress.StartThreadWithLoadData(); // потоки

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


        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                lblWarningInfo.Text = "";

                if (IsAccountFullDublicate())
                {
                    SetWarningInfo("Комбинация расчётного счёта, валюты и банка должна быть уникальной."); ShowWarningPnl(true);
                }
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    frmAddress.ConfirmChanges();
                    frmContact.ConfirmChanges();
                    if (bSaveChanges() == true)
                    {
                        SimulateChangeVendorProperties(m_objSelectedVendor, enumActionSaveCancel.Save, m_bNewVendor);
                    }
                }
                    
               
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в описании поставщика. Текст ошибки: " + f.Message);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            return;
        }
        #endregion

    }


    /// <summary>
    /// Класс – хранящий информацию, которая передается получателям уведомления о событии
    /// </summary>
    public partial class ChangeVendorPropertieEventArgs : EventArgs
    {
        private readonly CVendor m_objVendor;
        public CVendor Vendor
        {
            get { return m_objVendor; }
        }

        private readonly enumActionSaveCancel m_enActionType;
        public enumActionSaveCancel ActionType
        {
            get { return m_enActionType; }
        }

        private readonly System.Boolean m_bIsNewVendor;
        public System.Boolean IsNewVendor
        {
            get { return m_bIsNewVendor; }
        }

        public ChangeVendorPropertieEventArgs(CVendor objVendor, enumActionSaveCancel enActionType, System.Boolean bIsNewVendor)
        {
            m_objVendor = objVendor;
            m_enActionType = enActionType;
            m_bIsNewVendor = bIsNewVendor;
        }
    }

}
