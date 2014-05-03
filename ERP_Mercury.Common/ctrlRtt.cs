using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;

namespace ERP_Mercury.Common
{
    public partial class ctrlRtt : UserControl
    {
        #region Свойства, переменные
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemAddress;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemContact;

        private CRtt m_objSelectedRtt;
        private CCustomer m_objCustomerRtt;

        private System.Boolean m_bIsChanged;

        public ctrlAddress frmAddress;
        public ctrlContact frmContact;

        private System.Boolean m_bDisableEvents;
        private System.Boolean m_bNewRtt;
        private System.Boolean m_bIsReadOnly;
        /// <summary>
        /// блокировка возможности включения режима редактирования
        /// </summary>
        private System.Boolean m_bBlockChanceEdit;

        public List<CRtt> RttList
        {
            get 
            {
                List<CRtt> objRttList = new List<CRtt>();
                try
                {
                    foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeListRtt.Nodes)
                    {
                        if (objNode.Tag != null)
                        {
                            objRttList.Add((CRtt)objNode.Tag);
                        }
                    }
                }
                catch
                {
                    objRttList = null;
                }
                return objRttList; 
            }
        }
        private List<CRtt> m_objRttDeletedList;
        public List<CRtt> RttDeletedList
        {
            get { return m_objRttDeletedList; }
            set { m_objRttDeletedList = value; }
        }
        
   //     private List<CSalesSegmentation> m_objSegmentationList;

        private const System.String AllName = "---";
        private const System.Int32 iMinControlItemHeight = 20;
        private const System.Int32 iAdvControlItemHeight = 40;

        #endregion

        #region События
        // Создаем закрытое поле, ссылающееся на заголовок списка делегатов
        private EventHandler<ChangeRttPropertieEventArgs> m_ChangeRttProperties;
        // Создаем в классе член-событие
        public event EventHandler<ChangeRttPropertieEventArgs> ChangeRttProperties
        {
            add
            {
                // берем закрытую блокировку и добавляем обработчик
                // (передаваемый по значению) в список делегатов
                m_ChangeRttProperties += value;
            }
            remove
            {
                // берем закрытую блокировку и удаляем обработчик
                // (передаваемый по значению) из списка делегатов
                m_ChangeRttProperties -= value;
            }
        }
        /// <summary>
        /// Инициирует событие и уведомляет о нем зарегистрированные объекты
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnChangeRttProperties(ChangeRttPropertieEventArgs e)
        {
            // Сохраняем поле делегата во временном поле для обеспечение безопасности потока
            EventHandler<ChangeRttPropertieEventArgs> temp = m_ChangeRttProperties;
            // Если есть зарегистрированные объектв, уведомляем их
            if (temp != null) temp(this, e);
        }
        public void SimulateChangeRttProperties(CRtt objRtt, enumActionSaveCancel enActionType, System.Boolean bIsNewRtt)
        {
            // Создаем объект, хранящий информацию, которую нужно передать
            // объектам, получающим уведомление о событии
            ChangeRttPropertieEventArgs e = new ChangeRttPropertieEventArgs(objRtt, enActionType, bIsNewRtt);

            // Вызываем виртуальный метод, уведомляющий наш объект о возникновении события
            // Если нет типа, переопределяющего этот метод, наш объект уведомит все объекты, 
            // подписавшиеся на уведомление о событии
            OnChangeRttProperties(e);
        }

        private EventHandler<ChangeControlRttSizeEventArgs> m_ChangeControlRttSize;
        // Создаем в классе член-событие
        public event EventHandler<ChangeControlRttSizeEventArgs> ChangeControlRttSize
        {
            add
            {
                // берем закрытую блокировку и добавляем обработчик
                // (передаваемый по значению) в список делегатов
                m_ChangeControlRttSize += value;
            }
            remove
            {
                // берем закрытую блокировку и удаляем обработчик
                // (передаваемый по значению) из списка делегатов
                m_ChangeControlRttSize -= value;
            }
        }
        /// <summary>
        /// Инициирует событие и уведомляет о нем зарегистрированные объекты
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnChangeControlRttSize(ChangeControlRttSizeEventArgs e)
        {
            // Сохраняем поле делегата во временном поле для обеспечение безопасности потока
            EventHandler<ChangeControlRttSizeEventArgs> temp = m_ChangeControlRttSize;
            // Если есть зарегистрированные объектв, уведомляем их
            if (temp != null) temp(this, e);
        }
        public void SimulateChangeControlRttSize( Size objSize )
        {
            // Создаем объект, хранящий информацию, которую нужно передать
            // объектам, получающим уведомление о событии
            ChangeControlRttSizeEventArgs e = new ChangeControlRttSizeEventArgs(objSize);

            // Вызываем виртуальный метод, уведомляющий наш объект о возникновении события
            // Если нет типа, переопределяющего этот метод, наш объект уведомит все объекты, 
            // подписавшиеся на уведомление о событии
            OnChangeControlRttSize(e);
        }
        #endregion

        #region Конструктор
        public ctrlRtt(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem)
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_bIsChanged = false;
            m_bDisableEvents = false;
            m_bNewRtt = false;
            m_bBlockChanceEdit = false;

            m_objSelectedRtt = null;
            m_objCustomerRtt = null;
            //m_objSegmentationList = null;

            m_objRttDeletedList = new List<CRtt>();

            frmAddress = new ERP_Mercury.Common.ctrlAddress(ERP_Mercury.Common.EnumObject.Rtt, m_objProfile, m_objMenuItem, System.Guid.Empty);
           // frmAddress.InitAddressControl();

            frmContact = new ERP_Mercury.Common.ctrlContact(ERP_Mercury.Common.EnumObject.Rtt, m_objProfile, m_objMenuItem, System.Guid.Empty);

            layoutControlAddress.Size = new Size(layoutControlAddress.Size.Width, (frmAddress.Size.Height));
            layoutControlAddress.MaximumSize = new Size(layoutControlAddress.Size.Width, layoutControlAddress.Size.Height);

            layoutControlContact.Size = new Size(layoutControlContact.Size.Width, (frmContact.Size.Height));
            layoutControlContact.MaximumSize = new Size(layoutControlContact.Size.Width, layoutControlContact.Size.Height);
 
            layoutControlGroupAddress.Size = new Size(layoutControlGroupAddress.Size.Width, (frmAddress.Size.Height));
            layoutControlGroupConact.Size = new Size(layoutControlGroupConact.Size.Width, (frmContact.Size.Height));

            layoutControlCustomer.MaximumSize = new Size(layoutControlCustomer.Size.Width, layoutControlCustomer.Size.Height);
            layoutControlShedule.MaximumSize = new Size(layoutControlShedule.Size.Width, layoutControlShedule.Size.Height);
            layoutControlSegment.MaximumSize = new Size(layoutControlSegment.Size.Width, layoutControlSegment.Size.Height);
            layoutControlEquipment.MaximumSize = new Size(layoutControlEquipment.Size.Width, layoutControlEquipment.Size.Height);
            layoutControlProductCatalog.MaximumSize = new Size(layoutControlProductCatalog.Size.Width, layoutControlProductCatalog.Size.Height);

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

            LoadComboBoxItems();
            m_bIsReadOnly = false;

            layoutControlGroupAddress.Expanded = false;
            layoutControlGroupConact.Expanded = false;
            layoutControlGroupCustomer.Expanded = false;
            layoutControlGroupEquipment.Expanded = false;
            layoutControlGroupSegment.Expanded = false;
            layoutControlGroupShedule.Expanded = false;
            layoutControlGroupProductCatalog.Expanded = false;
        }

        #endregion

        #region Выпадающие списки
        /// <summary>
        /// Обновление выпадающих списков
        /// </summary>
        /// <returns>true - все списки успешно обновлены; false - ошибка</returns>
        public System.Boolean LoadComboBoxItems()
        {
            System.Boolean bRet = false;
            try
            {
                // признак активности
                cboxActiveType.Properties.Items.Clear();
                List<CRttActiveType> objRttActiveTypeList = CRttActiveType.GetRttActiveTypeList(m_objProfile, null);
                if (objRttActiveTypeList != null)
                {
                    cboxActiveType.Properties.Items.AddRange(objRttActiveTypeList);
                    //foreach (CRttActiveType objCustomerActiveType in objRttActiveTypeList)
                    //{
                    //    cboxActiveType.Properties.Items.Add(objCustomerActiveType);
                    //}
                }
               // objRttActiveTypeList = null;

                // спецкод
                cboxSpecCode.Properties.Items.Clear();
                List<CRttSpecCode> objSpecCodeList = CRttSpecCode.GetRttSpecCodeList(m_objProfile, null);
                if (objSpecCodeList != null)
                {
                    cboxSpecCode.Properties.Items.AddRange(objSpecCodeList);
                    //foreach (CRttSpecCode objSpecCode in objSpecCodeList)
                    //{
                    //    cboxSpecCode.Properties.Items.Add(objSpecCode);
                    //}
                }
                //objSpecCodeList = null;

                // оборудование
                treeListEquipment.Nodes.Clear();

                repItemAvailibilityCombo.Items.Clear();
                List<CAvailability> objAvailabilityList = CAvailability.GetAvailabilityList(m_objProfile, null);
                if (objAvailabilityList != null)
                {
                    repItemAvailibilityCombo.Items.AddRange(objAvailabilityList);
                    //foreach (CAvailability objAvailability in objAvailabilityList)
                    //{
                    //    repItemAvailibilityCombo.Items.Add( objAvailability );
                    //}
                }
                //objAvailabilityList = null;

                repItemProductCatalogCombo.Items.Clear();
                checkedListBoxProductCatalog.Items.Clear();
                List<CProductCatalog> objProductCatalogList = CProductCatalog.GetProductCatalogListAll(m_objProfile, null);
                if (objProductCatalogList != null)
                {
                    foreach (CProductCatalog objProductCatalog in objProductCatalogList)
                    {
                        if (objProductCatalog.IsOur == false) { continue; }
                        repItemProductCatalogCombo.Items.Add(objProductCatalog);
                        checkedListBoxProductCatalog.Items.Add(objProductCatalog, false);
                    }
                }
                objProductCatalogList = null;

                repItemEquipmentTypeCombo.Items.Clear();
                List<CEquipmentType> objEquipmentTypeList = CEquipmentType.GetEquipmentTypeList(m_objProfile, null);
                if (objEquipmentTypeList != null)
                {
                    repItemEquipmentTypeCombo.Items.AddRange(objEquipmentTypeList);
                    //foreach (CEquipmentType objEquipmentType in objEquipmentTypeList)
                    //{
                    //    repItemEquipmentTypeCombo.Items.Add(objEquipmentType);
                    //}
                }
                //objEquipmentTypeList = null;

                //repItemSizeEqCombo.Items.Clear();
                //List<CSizeEq> objSizeEqList = CSizeEq.GetSizeEqList(m_objProfile, null);
                //if (objSizeEqList != null)
                //{
                //    foreach (CSizeEq objSizeEq in objSizeEqList)
                //    {
                //        repItemSizeEqCombo.Items.Add( objSizeEq );
                //    }
                //}
                //objSizeEqList = null;

                cboxSegmentLevel1.SelectedItem = null;
                cboxSegmentLevel1.Properties.Items.Clear();
                cboxSegmentLevel2.SelectedItem = null;
                cboxSegmentLevel2.Properties.Items.Clear();
                System.String strErr = "";

                cboxSegmentLevel1.Properties.Items.AddRange(ERP_Mercury.Common.CSegmentationMarketDataBaseModel.GetSegmentationMarketList(m_objProfile, null, ref strErr));
                cboxSegmentLevel2.Properties.Items.AddRange(ERP_Mercury.Common.CSegmentationSubChanelDataBaseModel.GetSegmentationSubChannelList(m_objProfile, null, ref strErr));

                //cboxSegmentLevel3.Properties.Items.Clear();
                //m_objSegmentationList = CSalesSegmentation.GetSegmentationList(m_objProfile, null);
                //if (m_objSegmentationList != null)
                //{
                //    cboxSegmentLevel3.Properties.Items.AddRange(m_objSegmentationList);
                //    //foreach (CSegmentation objSegmentation in m_objSegmentationList)
                //    //{
                //    //    cboxSegmentLevel3.Properties.Items.Add( objSegmentation );
                //    //}
                //}

                //List<System.String> objSegmentationListLevel1 = CSalesSegmentation.GetSegmentationListLevel1(m_objProfile, null);
                //if (objSegmentationListLevel1 != null)
                //{
                //    cboxSegmentLevel1.Properties.Items.Add(AllName);
                //    cboxSegmentLevel1.Properties.Items.AddRange(objSegmentationListLevel1);

                //    //foreach (System.String objSegmentationLevel1 in objSegmentationListLevel1)
                //    //{
                //    //    cboxSegmentLevel1.Properties.Items.Add(objSegmentationLevel1);
                //    //}
                //}
                ////objSegmentationListLevel1 = null;

                //List<System.String> objSegmentationListLevel2 = CSalesSegmentation.GetSegmentationListLevel2(m_objProfile, null);
                //if (objSegmentationListLevel2 != null)
                //{
                //    cboxSegmentLevel2.Properties.Items.Add(AllName);
                //    cboxSegmentLevel2.Properties.Items.AddRange(objSegmentationListLevel2);

                //    //foreach (System.String objSegmentationLevel2 in objSegmentationListLevel2)
                //    //{
                //    //    cboxSegmentLevel2.Properties.Items.Add(objSegmentationLevel2);
                //    //}
                //}
                ////objSegmentationListLevel2 = null;

                // типы лицензий
                cboxLicenceType.Properties.Items.Clear();
                List<CLicenceType> objLicenceTypeList = CLicenceType.GetLicenceTypeList(m_objProfile, null);
                if (objLicenceTypeList != null)
                {
                    // добавим пустышку
                    cboxLicenceType.Properties.Items.Add(new CLicenceType());
                    cboxLicenceType.Properties.Items.AddRange(objLicenceTypeList);

                    //foreach (CLicenceType objLicenceType in objLicenceTypeList)
                    //{
                    //    cboxLicenceType.Properties.Items.Add(objLicenceType);
                    //}
                }
                //objLicenceTypeList = null;
                // 2011.06.03 if (frmAddress != null) { frmAddress.InitAllLists(); }

                bRet = true;
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка обновления выпадающих списков. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return bRet;
        }

        //private void FilterChanelForChanel0(System.String strChanel0)
        //{
        //    try
        //    {
        //        if (strChanel0 == "") { return; }
        //        System.String strChnl = "";
        //        if (strChanel0 == AllName)
        //        {
        //            cboxSegmentLevel2.Properties.Items.Clear();
        //            cboxSegmentLevel2.Properties.Items.Add(AllName);
        //            strChnl = "";
        //            foreach (CSalesSegmentation objSegm in m_objSegmentationList)
        //            {
        //                if (objSegm.Code.Substring(1, 2) != strChnl)
        //                {
        //                    cboxSegmentLevel2.Properties.Items.Add(objSegm.Code.Substring(1, 2));
        //                    strChnl = objSegm.Code.Substring(1, 2);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            cboxSegmentLevel2.SelectedItem = null;
        //            cboxSegmentLevel2.Properties.Items.Clear();
        //            strChnl = "";
        //            foreach (CSalesSegmentation objSegm in m_objSegmentationList)
        //            {
        //                if (objSegm.Code.Substring(0, 1) == strChanel0)
        //                {
        //                    if (objSegm.Code.Substring(1, 2) != strChnl)
        //                    {
        //                        cboxSegmentLevel2.Properties.Items.Add(objSegm.Code.Substring(1, 2));
        //                        strChnl = objSegm.Code.Substring(1, 2);
        //                    }
        //                }
        //            }
        //        }

        //        if (cboxSegmentLevel2.SelectedItem != null)
        //        {
        //            System.String strChanel = (System.String)cboxSegmentLevel2.SelectedItem;
        //            if (cboxSegmentLevel3.SelectedItem != null)
        //            {
        //                System.String strSubChanel = ((CSalesSegmentation)cboxSegmentLevel3.SelectedItem).Code;
        //                if (strSubChanel.Substring(1, 2) != strChanel)
        //                {
        //                    cboxSegmentLevel3.SelectedItem = null;
        //                }
        //            }
        //            cboxSegmentLevel3.Properties.Items.Clear();
        //            foreach (CSalesSegmentation objSegm in m_objSegmentationList)
        //            {
        //                if (objSegm.Code.Substring(1, 2) == strChanel)
        //                {
        //                    cboxSegmentLevel3.Properties.Items.Add(objSegm);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            cboxSegmentLevel3.SelectedItem = null;
        //            cboxSegmentLevel3.Properties.Items.Clear();
        //            foreach (CSalesSegmentation objSegm in m_objSegmentationList)
        //            {
        //                if( objSegm.Code.Substring(0, 1) == strChanel0 )
        //                {
        //                    cboxSegmentLevel3.Properties.Items.Add(objSegm);
        //                }
        //            }
        //        }

        //    }
        //    catch (System.Exception f)
        //    {
        //        SendMessageToLog("Ошибка фильтрации выпадающего списка. FilterChanelForChanel0. Текст ошибки: " + f.Message);
        //    }
        //    finally
        //    {
        //    }
        //    return;
        //}

        //private void FilterSubChanelForChanel(System.String strChanel)
        //{
        //    try
        //    {
        //        if (strChanel == "") { return; }
        //        if (strChanel == AllName)
        //        {
        //            System.String strSubChanel = "";
        //            if (cboxSegmentLevel3.SelectedItem != null)
        //            {
        //                strSubChanel = ( (CSalesSegmentation)cboxSegmentLevel3.SelectedItem ).Code;
        //            }
        //            cboxSegmentLevel3.SelectedItem = null;
        //            cboxSegmentLevel3.Properties.Items.Clear();
        //            foreach (CSalesSegmentation objSegm in m_objSegmentationList)
        //            {
        //                cboxSegmentLevel3.Properties.Items.Add(objSegm);
        //            }
        //            if (strSubChanel != "")
        //            {
        //                foreach (Object obj in cboxSegmentLevel3.Properties.Items)
        //                {
        //                    if (((CSalesSegmentation)obj).Code == strSubChanel)
        //                    {
        //                        cboxSegmentLevel3.SelectedItem = obj;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            cboxSegmentLevel3.SelectedItem = null;
        //            cboxSegmentLevel3.Properties.Items.Clear();
        //            foreach (CSalesSegmentation objSegm in m_objSegmentationList)
        //            {
        //                if (objSegm.Code.Substring(1, 2) == strChanel)
        //                {
        //                    cboxSegmentLevel3.Properties.Items.Add(objSegm);
        //                }
        //            }
        //        }


        //    }
        //    catch (System.Exception f)
        //    {
        //        SendMessageToLog("Ошибка фильтрации выпадающего списка. FilterSubChanelForChanel. Текст ошибки: " + f.Message);
        //    }
        //    finally
        //    {
        //    }
        //    return;
        //}

        #endregion

        #region Индикация изменений
        /// <summary>
        /// Устанавливает значение свойства m_bIsChanged и запускает событие OnChangeRttProperties
        /// </summary>
        /// <param name="bModified">параметр со значениями true|false</param>
        private void SetPropertiesModified(System.Boolean bModified)
        {
            m_bIsChanged = bModified;
            if (btnSave.Enabled != m_bIsChanged) { btnSave.Enabled = m_bIsChanged; }
            if (btnCancel.Enabled != btnSave.Enabled) { btnCancel.Enabled = btnSave.Enabled; }
            if (m_bIsChanged == true)
            {
                SimulateChangeRttProperties( m_objSelectedRtt, enumActionSaveCancel.Unkown, m_bNewRtt );
            }
        }
        /// <summary>
        /// Обработчик события, наступающего при изменении значений в выпадающих списках, связанных со свойствами РТТ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboxRttPropertie_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                SetPropertiesModified(true);
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения свойств " + ((DevExpress.XtraEditors.ComboBoxEdit)sender).ToolTip + ". Текст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        private void SheduleCheckChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                SetPropertiesModified(true);
                DevExpress.XtraEditors.CheckEdit checkBox = (DevExpress.XtraEditors.CheckEdit)sender;
                if ((checkBox.CheckState == CheckState.Checked) && (checkBox.Tag != null))
                {
                    System.Int32 iDayNum = System.Convert.ToInt32( checkBox.Tag );
                    switch (iDayNum)
                    {
                        case 2:
                            {
                                timeStartTuesday.Time = timeStartMonday.Time;
                                timeFinishTuesday.Time = timeFinishMonday.Time;
                                break;
                            }
                        case 3:
                            {
                                timeStartWendsday.Time = timeStartTuesday.Time;
                                timeFinishWendsday.Time = timeFinishTuesday.Time;
                                break;
                            }
                        case 4:
                            {
                                timeStartThursday.Time = timeStartWendsday.Time;
                                timeFinishThursday.Time = timeFinishWendsday.Time;
                                break;
                            }
                        case 5:
                            {
                                timeStartFriday.Time = timeStartThursday.Time;
                                timeFinishFriday.Time = timeFinishThursday.Time;
                                break;
                            }
                        case 6:
                            {
                                timeStartSaturday.Time = timeStartFriday.Time;
                                timeFinishSaturday.Time = timeFinishFriday.Time;
                                break;
                            }
                        case 7:
                            {
                                timeStartSunday.Time = timeStartSaturday.Time;
                                timeFinishSunday.Time = timeFinishSaturday.Time;
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения свойств " + ((DevExpress.XtraEditors.ComboBoxEdit)sender).ToolTip + ". Текст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        /// <summary>
        /// Обработчик события, наступающего при изменении значений в элементах управления, связанных со свойствами РТТ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtRttPropertie_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
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
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения свойств клиента. Текст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        private void checkBoxShedule_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                SetPropertiesModified(true);
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения признака 'Вкл.'. Текст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        private void treeListEquipment_CellValueChanged(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
            if (m_bDisableEvents == true) { return; }
            try
            {
                System.Int32 iPosNode = treeListEquipment.GetNodeIndex(e.Node);
                // тип оборудования
                if ((e.Column == colEquipmentType) && (e.Value != null))
                {
                    ((CTradeEquipment)e.Node.Tag).EquipmentType = (CEquipmentType)e.Value;
                }
                // возможность установки
                if ((e.Column == colAvailibility) && (e.Value != null))
                {
                    ((CTradeEquipment)e.Node.Tag).Availability = (CAvailability)e.Value;
                }
                // Количество
                if ((e.Column == colQty) && (e.Value != null))
                {
                    ((CTradeEquipment)e.Node.Tag).Quantity = System.Convert.ToInt32(e.Value);

                }


                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListEquipment_CellValueChanged. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void treeListEquipment_CellValueChanging(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
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

        #region Торговое оборудование
        /// <summary>
        /// Добавляет оборудование в список
        /// </summary>
        private void AddEquipment()
        {
            try
            {
                if (treeListEquipment.Enabled == false) { treeListEquipment.Enabled = true; }
                if (m_objSelectedRtt.TradeEquipmentList == null) { m_objSelectedRtt.TradeEquipmentList = new List<CTradeEquipment>(); }
                //System.Boolean bNotFullNode = false;
                //foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objItem in treeListEquipment.Nodes)
                //{
                //    if ( (objItem.GetValue(colQty) == null) || (objItem.GetValue(colEquipmentType) == null) || (objItem.GetValue(colAvailibility) == null) )
                //    {
                //        treeListEquipment.FocusedNode = objItem;
                //        bNotFullNode = true;
                //        break;
                //    }
                //}
                //if (bNotFullNode == true) { return; }

                CTradeEquipment objTradeEquipment = new CTradeEquipment();
                objTradeEquipment.Quantity = 1;
                objTradeEquipment.IsNewObject = true;

                treeListEquipment.AppendNode(new object[] {  
                            objTradeEquipment.EquipmentType, objTradeEquipment.Availability, objTradeEquipment.Quantity }, null).Tag = objTradeEquipment;
                treeListEquipment.FocusedNode = treeListEquipment.Nodes.LastNode;
                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка добавления торгового оборудования в список. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// Удаляет оборудование из списка
        /// </summary>
        /// <param name="objNode">удаляемый узел в дереве</param>
        private void DeleteEquipment(DevExpress.XtraTreeList.Nodes.TreeListNode objNode)
        {
            try
            {
                if ((objNode == null) || (treeListEquipment.Nodes.Count == 0)) { return; }

                if (m_objSelectedRtt.TradeEquipmentForDeleteList == null) { m_objSelectedRtt.TradeEquipmentForDeleteList = new List<CTradeEquipment>(); }
                DevExpress.XtraTreeList.Nodes.TreeListNode objPrevNode = objNode.PrevNode;
                m_objSelectedRtt.TradeEquipmentForDeleteList.Add((CTradeEquipment)objNode.Tag);

                treeListEquipment.Nodes.Remove(objNode);
                if (objPrevNode == null)
                {

                    if (treeListEquipment.Nodes.Count > 0)
                    {
                        treeListEquipment.FocusedNode = treeListEquipment.Nodes[0];
                    }
                }
                else
                {
                    treeListEquipment.FocusedNode = objPrevNode;
                }

                SetPropertiesModified(true);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка удаления торгового оборудования. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void mitemAddTradeEquipment_Click(object sender, EventArgs e)
        {
            try
            {
                AddEquipment();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("mitemAddTradeEquipment_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void mitemDeleteTradeEquipment_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteEquipment(treeListEquipment.FocusedNode);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("mitemDeleteTradeEquipment_Click. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        private void treeListEquipment_MouseClick(object sender, MouseEventArgs e)
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
                        mitemDeleteTradeEquipment.Enabled = false;
                    }
                    else
                    {
                        // выделяем узел
                        mitemDeleteTradeEquipment.Enabled = true;
                        hi.Node.TreeList.FocusedNode = hi.Node;
                    }
                    contextMenuStripTradeEquipment.Show(((DevExpress.XtraTreeList.TreeList)sender), new Point(e.X, e.Y));
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListEquipment_MouseClick. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        #endregion

        #region Загрузить список РТТ
        /// <summary>
        /// Загружает список РТТ в дерево
        /// </summary>
        /// <param name="objCustomerRtt">клиент</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public System.Boolean LoadRttList(ERP_Mercury.Common.CCustomer objCustomerRtt)
        {
            System.Boolean bRet = false;
            try
            {
                m_bDisableEvents = true;

                treeListRtt.Nodes.Clear();
                m_objCustomerRtt = objCustomerRtt;

                if (objCustomerRtt.RttList != null)
                {
                    foreach (CRtt objRtt in objCustomerRtt.RttList)
                    {
                        DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListRtt.AppendNode(new object[] { objRtt.VisitingCard }, null);
                        objNode.Tag = objRtt;
                    }
                }
                else
                {
                    // запросим список РТТ из БД
                    objCustomerRtt.RttList = CRtt.GetRttList(m_objProfile, null, m_objCustomerRtt.ID);
                    if (RttList != null)
                    {
                        foreach (CRtt objRtt in objCustomerRtt.RttList)
                        {
                            DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListRtt.AppendNode(new object[] { objRtt.VisitingCard }, null);
                            objNode.Tag = objRtt;
                        }
                    }
                }

                if (treeListRtt.Nodes.Count > 0)
                {
                    treeListRtt.FocusedNode = treeListRtt.Nodes[0];
                    ShowRtt((CRtt)treeListRtt.Nodes[0].Tag, m_objCustomerRtt);
                }
                else
                {
                    ClearAllPropertiesControls();
                }

                bRet = true;

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка обновления списка.\n\nТекст ошибки:\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                m_bDisableEvents = false;
            }

            return bRet;
        }
        private void treeListRtt_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                if ((e.Node == null) || (e.Node.Tag == null) || (e.Node.TreeList.Nodes.Count == 0)) { return; }
                ShowRtt((CRtt)e.Node.Tag, m_objCustomerRtt);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка смены РТТ.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                SetPropertiesModified(false);
            }

            return;
        }
        private void treeListRtt_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (m_bIsChanged == true) { return; }
                    // попробуем определить, что же у нас под мышкой
                    DevExpress.XtraTreeList.TreeListHitInfo hi = ((DevExpress.XtraTreeList.TreeList)sender).CalcHitInfo(new Point(e.X, e.Y));
                    if ((hi != null) && (hi.Node != null))
                    {
                        // выделяем узел
                        hi.Node.TreeList.FocusedNode = hi.Node;
                    }
                    menuAdd.Enabled = btnAddRtt.Enabled;
                    menuDelete.Enabled = btnDeleteRtt.Enabled;
                    menuEdit.Enabled = btnEditRtt.Enabled;

                    contextMenuStrip.Show(((DevExpress.XtraTreeList.TreeList)sender), new Point(e.X, e.Y));
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListRtt_MouseClick.\nТекст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;

        }

        private void treeListRtt_BeforeFocusNode(object sender, DevExpress.XtraTreeList.BeforeFocusNodeEventArgs e)
        {
            try
            {
                if (m_bIsChanged == true)
                {
                    // в описание контакта были внесены изменения
                    DialogResult dlgRes = DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Данные розничной точки были изменены. Подтвердить изменения в описании розничной точки?", "Подтверждение",
                        System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question);
                    switch (dlgRes)
                    {
                        case DialogResult.Yes:
                            {
                                System.String strErr = "";
                                e.CanFocus = bSaveChanges( ref strErr );
                                if (e.CanFocus == false)
                                {
                                    DevExpress.XtraEditors.XtraMessageBox.Show("Изменения в данных РТТ не будут сохранены.\n" + strErr, "Внимание",
                                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                                }
                                break;
                            }
                        case DialogResult.No:
                            {
                                e.CanFocus = true;
                                break;
                            }
                        case DialogResult.Cancel:
                            {
                                e.CanFocus = true;
                                break;
                            }
                        default:
                            {
                                break;
                            }

                    }
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("treeListAddress_BeforeFocusNode. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }

        #endregion

        #region Просмотр свойств РТТ
        /// <summary>
        /// Просмотр свойств РТТ
        /// </summary>
        /// <param name="objRtt">РТТ</param>
        /// <param name="objCustomerRtt">Клиент</param>
        public void ShowRtt(ERP_Mercury.Common.CRtt objRtt, ERP_Mercury.Common.CCustomer objCustomerRtt)
        {
            try
            {
                EditRtt(objRtt, objCustomerRtt);
                SetModeReadOnly(true);
                if (m_bBlockChanceEdit == true)
                {
                    //родительский элемент еще не отменял блокировку возможности включить режим редактирования
                    SetChanceEditProperties(false);
                }

            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка просмотра свойств РТТ. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return;
        }
        #endregion

        #region Редактирование РТТ
        /// <summary>
        /// Редактирование РТТ
        /// </summary>
        /// <param name="objRtt">РТТ</param>
        /// <param name="objCustomerRtt">Клиент</param>
        public void EditRtt(ERP_Mercury.Common.CRtt objRtt, ERP_Mercury.Common.CCustomer objCustomerRtt)
        {
            if (objRtt == null) { return; }
            m_bDisableEvents = true;
            try
            {
                //System.String strErr = "";

                m_objSelectedRtt = objRtt;
                m_objCustomerRtt = objCustomerRtt;

                //if (m_objSelectedRtt.InitAdvancedProperties(m_objProfile, null, ref strErr) == false)
                //{
                //    SendMessageToLog( strErr );
                //}

                if (m_objSelectedRtt.ContactForDeleteList == null) { m_objSelectedRtt.ContactForDeleteList = new List<CContact>(); }
                else { m_objSelectedRtt.ContactForDeleteList.Clear(); }
                if (m_objSelectedRtt.AddressForDeleteList == null) { m_objSelectedRtt.AddressForDeleteList = new List<CAddress>(); }
                else { m_objSelectedRtt.AddressForDeleteList.Clear(); }

                this.SuspendLayout();

                // очистим содержимое элементов управления
                ClearAllPropertiesControls();

                // теперь начнем заполнять
                txtFullName.Text = m_objSelectedRtt.FullName;
                txtCode.Text = m_objSelectedRtt.Code;

                // признак активности
                if ((m_objSelectedRtt.RttActiveType != null) && (cboxActiveType.Properties.Items.Count > 0))
                {
                    foreach (Object objActiveType in cboxActiveType.Properties.Items)
                    {
                        if (((CRttActiveType)objActiveType).ID.CompareTo(m_objSelectedRtt.RttActiveType.ID) == 0)
                        {
                            cboxActiveType.SelectedItem = objActiveType;
                            break;
                        }
                    }
                }
                // спецкод
                if ((m_objSelectedRtt.RttSpecCode != null) && (cboxSpecCode.Properties.Items.Count > 0))
                {
                    foreach (Object objRttSpecCode in cboxSpecCode.Properties.Items)
                    {
                        if (((CRttSpecCode)objRttSpecCode).ID.CompareTo(m_objSelectedRtt.RttSpecCode.ID) == 0)
                        {
                            cboxSpecCode.SelectedItem = objRttSpecCode;
                            break;
                        }
                    }
                }
                // типы лицензий
                if ((m_objSelectedRtt.LicenceType != null) && ( cboxLicenceType.Properties.Items.Count > 0))
                {
                    foreach (Object objLicenceType in cboxLicenceType.Properties.Items)
                    {
                        if (((CLicenceType)objLicenceType).ID.CompareTo(m_objSelectedRtt.LicenceType.ID) == 0)
                        {
                            cboxLicenceType.SelectedItem = objLicenceType;
                            break;
                        }
                    }
                }
                // сегментация
                cboxSegmentLevel1.SelectedItem = (m_objSelectedRtt.SegmentationMarket == null) ? null : cboxSegmentLevel1.Properties.Items.Cast<CSegmentationMarket>().SingleOrDefault<CSegmentationMarket>(x => x.ID.Equals(m_objSelectedRtt.SegmentationMarket.ID));
                cboxSegmentLevel2.SelectedItem = (m_objSelectedRtt.SegmentationSubChannel == null) ? null : cboxSegmentLevel2.Properties.Items.Cast<CSegmentationSubChannel>().SingleOrDefault<CSegmentationSubChannel>(x => x.ID.Equals(m_objSelectedRtt.SegmentationSubChannel.ID));
                if ((cboxSegmentLevel1.SelectedItem != null) && (cboxSegmentLevel2.SelectedItem != null))
                {
                    lblOldSegmenation.Text = "";
                }
                else
                if (m_objSelectedRtt.Segmentation != null)
                {
                    lblOldSegmenation.Text = "*Субканал сбыта: " + m_objSelectedRtt.Segmentation.Code;
                }
                // оборудование
                treeListEquipment.Nodes.Clear();
                if ((m_objSelectedRtt.TradeEquipmentList != null) && (m_objSelectedRtt.TradeEquipmentList.Count > 0))
                {
                    foreach (CTradeEquipment objTradeEquipment in m_objSelectedRtt.TradeEquipmentList)
                    {
                        DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListEquipment.AppendNode(new object[] {  
                            objTradeEquipment.EquipmentType, objTradeEquipment.Availability, objTradeEquipment.Quantity }, null);
                        objNode.Tag = objTradeEquipment;
                    }
                }
                // присутствие тм
                if ((m_objSelectedRtt.ProductCatalogList != null) && (m_objSelectedRtt.ProductCatalogList.Count > 0))
                {
                    foreach (CProductCatalog obProductCatalog in m_objSelectedRtt.ProductCatalogList)
                    {
                        for (System.Int32 i = 0; i < checkedListBoxProductCatalog.Items.Count; i++)
                        {
                            if (((CProductCatalog)checkedListBoxProductCatalog.Items[i].Value).ID.CompareTo(obProductCatalog.ID) == 0)
                            {
                                checkedListBoxProductCatalog.Items[i].CheckState = CheckState.Checked;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    for (System.Int32 i = 0; i < checkedListBoxProductCatalog.Items.Count; i++)
                    {
                        checkedListBoxProductCatalog.Items[i].CheckState = CheckState.Unchecked;
                    }
                }

                // Адреса
                frmAddress.LoadAddressList(m_objSelectedRtt.ID, m_objSelectedRtt.AddressList);
                if( ( frmAddress.AddressList != null ) && ( frmAddress.AddressList.Count > 0 ) )
                {
                    txtAddress.Text = frmAddress.AddressList[0].FullName;
                }
                // Контакты
                frmContact.LoadContactList(m_objSelectedRtt.ID, m_objSelectedRtt.ContactList);
                // Информация о клиенте
                LoadInfoAboutCustomer(m_objCustomerRtt);
                // расписание работы
                ShowRttShedule();
                txtShedule.Text = m_objSelectedRtt.ActionPeriod.ShortShedule;

                SetPropertiesModified(false);

                SetModeReadOnly( false );
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования РТТ. Текст ошибки: " + f.Message);
            }
            finally
            {
                //((System.ComponentModel.ISupportInitialize)(this.tabControl)).EndInit();
                this.ResumeLayout(false);
                //tabControl.SelectedTabPage = tabGeneral;
                m_bDisableEvents = false;
                //tabControl.SelectedTabPage = tabCustomer;
            }
            return;
        }
        /// <summary>
        /// загружает описание клиента
        /// </summary>
        /// <param name="objCustomerRtt">Клиент</param>
        private void LoadInfoAboutCustomer(ERP_Mercury.Common.CCustomer objCustomerRtt)
        {
            try
            {
                memoEditCustomer.Text = "";
                memoEditCustomer.EditValue = "";
                if (objCustomerRtt == null) { return; }
                // Наименование клиента
                memoEditCustomer.EditValue += ("\r\n" + " Наименование: " + "\r\t" + objCustomerRtt.FullName + "\r\n");

                // Юридический адрес
                if ((objCustomerRtt.AddressList != null) && (objCustomerRtt.AddressList.Count > 0))
                {
                    memoEditCustomer.EditValue += ("\r\n" + " Юридический адрес: " + "\r\t" + objCustomerRtt.AddressList[0].FullName + "\r\n");
                }
                // УНН
                memoEditCustomer.EditValue += ("\r\n" + " УНН: " + "\r\t\r\t" + objCustomerRtt.UNP + "\r\n");
                // ОКПО
                memoEditCustomer.EditValue += ("\r\n" + " ОКПО: " + "\r\t\r\t" + objCustomerRtt.OKPO + "\r\n");
                // ОКЮЛП
                memoEditCustomer.EditValue += ("\r\n" + " ОКЮЛП: " + "\r\t\r\t" + objCustomerRtt.OKULP + "\r\n");
                // Лицензии
                if ((objCustomerRtt.LicenceList != null) && (objCustomerRtt.LicenceList.Count > 0))
                {
                    memoEditCustomer.EditValue += ("\r\n" + " Лицензии: " + "\r\n");
                    foreach (CLicence objLicence in objCustomerRtt.LicenceList)
                    {
                        memoEditCustomer.EditValue += ("\r\n" + objLicence.LicenceType.Name + " №" + objLicence.LicenceNum + " выдана " + objLicence.BeginDate.ToShortDateString() + " действительна до " + objLicence.EndDate.ToShortDateString() );
                    }
                }
                // Банковские реквизиты
                if ((objCustomerRtt.AccountList != null) && (objCustomerRtt.AccountList.Count > 0))
                {
                    memoEditCustomer.EditValue += ("\r\n\r\n" + " Банковские реквизиты: " + "\r\n");
                    foreach (CAccount objAccount in objCustomerRtt.AccountList)
                    {
                        memoEditCustomer.EditValue += ("\r\n " + " р/с " +  objAccount.AccountNumber + "\r\n" + objAccount.Bank.Name + " МФО " + objAccount.Bank.MFO);
                        if ((objAccount.Bank.Address != null) && (objAccount.Bank.Address.Count > 0))
                        {
                            memoEditCustomer.EditValue += ("\r\n " + objAccount.Bank.Address[0].FullName);

                        }
                    }
                }
                
            }
            catch (System.Exception f)
            {
                memoEditCustomer.Text = "";
                memoEditCustomer.EditValue = "";
                SendMessageToLog( "Ошибка загрузки описания клиента. Текст ошибки: " + f.Message );
            }
            finally
            {
            }

            return ;
        }
        /// <summary>
        /// Сбрасывает значения в расписании работы
        /// </summary>
        private void ClearRttShedule()
        {
            try
            {
                System.String strNullTimeStart = "01.01.2000 10:00:00";
                System.DateTime dtNullTimeStart = System.DateTime.Parse(strNullTimeStart);

                System.String strNullTimeFiish = "01.01.2000 20:00:00";
                System.DateTime dtNullTimeFiish = System.DateTime.Parse(strNullTimeFiish);

                System.String strNullTimeBreakFiish = "01.01.2000 14:00:00";
                System.DateTime dtNullTimeBreakFiish = System.DateTime.Parse(strNullTimeBreakFiish);

                System.String strNullTimeBreakStart = "01.01.2000 13:00:00";
                System.DateTime dtNullTimeBreakStart = System.DateTime.Parse(strNullTimeBreakStart);

                checkBreak.Checked = false;
                checkMonday.Checked = false;
                checkTuesday.Checked = false;
                checkWendsday.Checked = false;
                checkThursday.Checked = false;
                checkFriday.Checked = false;
                checkSaturday.Checked = false;
                checkSunday.Checked = false;

                timeStartMonday.Time = dtNullTimeStart;
                timeStartThursday.Time = dtNullTimeStart;
                timeStartTuesday.Time = dtNullTimeStart;
                timeStartWendsday.Time = dtNullTimeStart;
                timeStartFriday.Time = dtNullTimeStart;
                timeStartSaturday.Time = dtNullTimeStart;
                timeStartSunday.Time = dtNullTimeStart;
                timeStartBreak.Time = dtNullTimeBreakStart;

                timeFinishMonday.Time = dtNullTimeFiish;
                timeFinishThursday.Time = dtNullTimeFiish;
                timeFinishTuesday.Time = dtNullTimeFiish;
                timeFinishWendsday.Time = dtNullTimeFiish;
                timeFinishFriday.Time = dtNullTimeFiish;
                timeFinishSaturday.Time = dtNullTimeFiish;
                timeFinishSunday.Time = dtNullTimeFiish;
                timeFiishBreak.Time = dtNullTimeBreakFiish;


            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка отображения расписания работы РТТ. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;
        }
        /// <summary>
        /// Загружает в элемент управления расписание работы РТТ
        /// </summary>
        private void ShowRttShedule()
        {
            try
            {
                ClearRttShedule();
                if (m_objSelectedRtt.ActionPeriod != null)
                {
                    foreach (CActionPeriodPoint objPeriodPoint in m_objSelectedRtt.ActionPeriod.ActionPeriodPointList)
                    {
                        switch (objPeriodPoint.DayNum)
                        {
                            case 0:
                                {
                                    checkBreak.Checked = objPeriodPoint.Enable;
                                    timeStartBreak.Time = objPeriodPoint.TimeStart;
                                    timeFiishBreak.Time = objPeriodPoint.TimeFinish;
                                    break;
                                }
                            case 1:
                                {
                                    checkMonday.Checked = objPeriodPoint.Enable;
                                    timeStartMonday.Time = objPeriodPoint.TimeStart;
                                    timeFinishMonday.Time = objPeriodPoint.TimeFinish;
                                    break;
                                }
                            case 2:
                                {
                                    checkTuesday.Checked = objPeriodPoint.Enable;
                                    timeStartTuesday.Time = objPeriodPoint.TimeStart;
                                    timeFinishTuesday.Time = objPeriodPoint.TimeFinish;
                                    break;
                                }
                            case 3:
                                {
                                    checkWendsday.Checked = objPeriodPoint.Enable;
                                    timeStartWendsday.Time = objPeriodPoint.TimeStart;
                                    timeFinishWendsday.Time = objPeriodPoint.TimeFinish;
                                    break;
                                }
                            case 4:
                                {
                                    checkThursday.Checked = objPeriodPoint.Enable;
                                    timeStartThursday.Time = objPeriodPoint.TimeStart;
                                    timeFinishThursday.Time = objPeriodPoint.TimeFinish;
                                    break;
                                }
                            case 5:
                                {
                                    checkFriday.Checked = objPeriodPoint.Enable;
                                    timeStartFriday.Time = objPeriodPoint.TimeStart;
                                    timeFinishFriday.Time = objPeriodPoint.TimeFinish;
                                    break;
                                }
                            case 6:
                                {
                                    checkSaturday.Checked = objPeriodPoint.Enable;
                                    timeStartSaturday.Time = objPeriodPoint.TimeStart;
                                    timeFinishSaturday.Time = objPeriodPoint.TimeFinish;
                                    break;
                                }
                            case 7:
                                {
                                    checkSunday.Checked = objPeriodPoint.Enable;
                                    timeStartSunday.Time = objPeriodPoint.TimeStart;
                                    timeFinishSunday.Time = objPeriodPoint.TimeFinish;
                                    break;
                                }

                        }
                    }
                }


            }
            catch (System.Exception f)
            {
                SendMessageToLog( "Ошибка отображения расписания работы РТТ. Текст ошибки: " + f.Message );
            }
            finally
            {
            }

            return;
        }
        #endregion

        #region Режим просмотра/редактирования
        public void SetChanceEditProperties(System.Boolean bChance)
        {
            try
            {
                if (bChance == false)
                {
                    // отключить возможнось войти в режим редактирования
                    m_bBlockChanceEdit = true;
                    btnAddRtt.Enabled = bChance;
                    btnEditRtt.Enabled = bChance;
                    btnDeleteRtt.Enabled = bChance;

                    btnCancel.Enabled = bChance;
                    btnSave.Enabled = bChance;
                }
                else
                {
                    // включить возможнось войти в режим редактирования
                    m_bBlockChanceEdit = false;
                    SetModeReadOnly(true);
                }
                frmAddress.SetChanceEditProperties(bChance);
                frmAddress.SetChanceEditProperties(bChance);
                frmContact.SetChanceEditProperties(bChance);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("SetChanceEditProperties.\n\nТекст ошибки:\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        /// <summary>
        /// Устанавливает режим просмотра/редактирования
        /// </summary>
        /// <param name="bSet">true - режим просмотра; false - режим редактирования</param>
        public void SetModeReadOnly(System.Boolean bSet)
        {
            try
            {
                txtFullName.Properties.ReadOnly = bSet;

                txtFullName.Properties.ReadOnly = bSet;

                cboxActiveType.Properties.ReadOnly = bSet;
                cboxSpecCode.Properties.ReadOnly = bSet;
                cboxLicenceType.Properties.ReadOnly = bSet;
                cboxSegmentLevel1.Properties.ReadOnly = bSet;
                cboxSegmentLevel2.Properties.ReadOnly = bSet;

                panelMonday.Enabled = !bSet;
                panelThursday.Enabled = !bSet;
                panelWendsday.Enabled = !bSet;
                panelTuesday.Enabled = !bSet;
                panelFriday.Enabled = !bSet;
                panelSaturday.Enabled = !bSet;
                panelSunday.Enabled = !bSet;
                panelBreak.Enabled = !bSet;

                foreach (DevExpress.XtraTreeList.Columns.TreeListColumn objColumn in treeListEquipment.Columns)
                {
                    objColumn.OptionsColumn.ReadOnly = bSet;
                }

                checkedListBoxProductCatalog.Enabled = !bSet;

                m_bIsReadOnly = bSet;
                btnEditRtt.Enabled = bSet;

                if (bSet == true)
                {
                    // включен режим "только просмотр"
                    if (btnSave.Enabled != false) { btnSave.Enabled = false; }
                    if (btnCancel.Enabled != false) { btnCancel.Enabled = false; }
                    if (btnAddRtt.Enabled != true) { btnAddRtt.Enabled = true; }
                    btnEditRtt.Enabled = (treeListRtt.FocusedNode != null);
                    btnDeleteRtt.Enabled = (treeListRtt.FocusedNode != null);
                }
                else
                {
                    // включен режим "редактирование"
                    if (btnSave.Enabled != m_bIsChanged) { btnSave.Enabled = m_bIsChanged; }
                    if (btnCancel.Enabled != true) { btnCancel.Enabled = true; }
                    if (btnAddRtt.Enabled != false) { btnAddRtt.Enabled = false; }
                    if (btnEditRtt.Enabled != (!m_bIsChanged)) { btnEditRtt.Enabled = !m_bIsChanged; }
                    if (btnDeleteRtt.Enabled != false) { btnDeleteRtt.Enabled = false; }
                }
                
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
                if (m_objSelectedRtt == null) { return; } 

                SetModeReadOnly(false);
                SetPropertiesModified(true);

                btnEditRtt.Enabled = false;
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

        #region Новая РТТ
        /// <summary>
        /// Новая РТТ
        /// </summary>
        public void NewRtt()
        {
            m_bDisableEvents = true;
            m_bNewRtt = true;
            try
            {
                m_objSelectedRtt = new CRtt();
                m_objSelectedRtt.IsNewObject = true;

                ClearAllPropertiesControls();
                m_bDisableEvents = true;

                // новая РТТ
                DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeListRtt.AppendNode(new object[] { (m_objSelectedRtt.Code + "\n" + m_objSelectedRtt.FullName) }, null);
                objNode.Tag = m_objSelectedRtt;
                //treeListRtt.FocusedNode = objNode.LastNode; // treeListRtt.FocusedNode = objNode // здесь было закоментирована эта строка
                //-----
                bool temp = m_bIsChanged;
                m_bIsChanged = false;
                treeListRtt.FocusedNode = objNode;
                m_bIsChanged = temp;
                //-----
                SetPropertiesModified(true);
                txtFullName.Focus();
                SetModeReadOnly(false);

                btnEditRtt.Enabled = false;
                btnAddRtt.Enabled = false;
                btnCancel.Enabled = true;
                btnSave.Enabled = true;
                btnDeleteRtt.Enabled = false;

                frmAddress.ClearAllPropertiesControls();
                frmContact.ClearAllPropertiesControls();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка создания РТТ. Текст ошибки: " + f.Message);
            }
            finally
            {
                m_bDisableEvents = false;
            }
            return;
        }
        private void menuAdd_Click(object sender, EventArgs e)
        {
            try
            {
                NewRtt();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка добавления РТТ.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        private void btnAddRtt_Click(object sender, EventArgs e)
        {
            try
            {
                NewRtt();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка добавления РТТ.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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
            try
            {
                Cancel();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка отмены изменений в описании РТТ. Текст ошибки: " + f.Message);
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
                if (m_objSelectedRtt == null) { return; }
                if (m_bNewRtt == true)
                {
                    ClearAllPropertiesControls();
                    SetModeReadOnly(true);
                    if (treeListRtt.FocusedNode != null)
                    {
                        System.Int32 iNodeIndx = treeListRtt.GetNodeIndex(treeListRtt.FocusedNode);
                        treeListRtt.Nodes.Remove(treeListRtt.FocusedNode);
                        if (treeListRtt.Nodes.Count > 0)
                        {
                            treeListRtt.FocusedNode = treeListRtt.Nodes[treeListRtt.Nodes.Count - 1];
                        }
                    }
                }
                else
                {
                    if ((m_objSelectedRtt != null) && (m_bNewRtt == false))
                    {
                        ShowRtt(m_objSelectedRtt, m_objCustomerRtt);
                    }
                }
                btnAddRtt.Enabled = true;
                btnDeleteRtt.Enabled = ( treeListRtt.FocusedNode != null );
                btnEditRtt.Enabled = (treeListRtt.FocusedNode != null);
                btnCancel.Enabled = false;
                btnSave.Enabled = false;
                m_objSelectedRtt.IsChanged = false;
                SimulateChangeRttProperties( m_objSelectedRtt, enumActionSaveCancel.Cancel, m_bNewRtt );
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка отмены изменений в описании РТТ. Текст ошибки: " + f.Message);
            }
            finally
            {
            }

            return;

        }
        #endregion

        #region Сохранить изменения
        /// <summary>
        /// Присваивает свойствам объекта РТТ значения из элементов управления
        /// </summary>
        /// <returns>>true - удачное завершение операции;false - ошибка</returns>
        private System.Boolean bSaveChanges( ref System.String strErr )
        {
            System.Boolean bRet = false;
            try
            {
                frmAddress.ConfirmChanges();
                frmContact.ConfirmChanges();

                m_objSelectedRtt.FullName = txtFullName.Text;
                m_objSelectedRtt.ShortName = txtFullName.Text;
                // признак активности
                if (cboxActiveType.SelectedItem != null)
                {
                    m_objSelectedRtt.RttActiveType = (CRttActiveType)cboxActiveType.SelectedItem;
                }
                // спецкод
                if (cboxSpecCode.SelectedItem != null)
                {
                    m_objSelectedRtt.RttSpecCode = (CRttSpecCode)cboxSpecCode.SelectedItem;
                }
                if ( cboxLicenceType.SelectedItem != null)
                {
                    CLicenceType objLicenceType = (CLicenceType)cboxLicenceType.SelectedItem;
                    m_objSelectedRtt.LicenceType = ((objLicenceType.ID.CompareTo(System.Guid.Empty) == 0) ? null : objLicenceType);
                }
                // сегментация
                //if (cboxSegmentLevel3.SelectedItem != null)
                //{
                //    m_objSelectedRtt.Segmentation = (CSalesSegmentation)cboxSegmentLevel3.SelectedItem;
                //}

                if (cboxSegmentLevel1.SelectedItem != null)
                {
                    m_objSelectedRtt.SegmentationMarket = (CSegmentationMarket)cboxSegmentLevel1.SelectedItem;
                }
                if (cboxSegmentLevel2.SelectedItem != null)
                {
                    m_objSelectedRtt.SegmentationSubChannel = (CSegmentationSubChannel)cboxSegmentLevel2.SelectedItem;
                }

                // торговое оборудование
                if (m_objSelectedRtt.TradeEquipmentList == null)
                { 
                    m_objSelectedRtt.TradeEquipmentList = new List<CTradeEquipment>(); 
                }
                else
                {
                    m_objSelectedRtt.TradeEquipmentList.Clear();
                }
                foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objItem in treeListEquipment.Nodes)
                {
                    if (objItem.Tag != null )
                    {
                        m_objSelectedRtt.TradeEquipmentList.Add((CTradeEquipment)objItem.Tag);
                    }
                }
                // каталоги продукции
                if (m_objSelectedRtt.ProductCatalogList == null)
                {
                    m_objSelectedRtt.ProductCatalogList = new List<CProductCatalog>();
                }
                else
                {
                    m_objSelectedRtt.ProductCatalogList.Clear();
                }
                if (m_objSelectedRtt.ProductCatalogForDeleteList == null)
                {
                    m_objSelectedRtt.ProductCatalogForDeleteList = new List<CProductCatalog>();
                }
                else
                {
                    m_objSelectedRtt.ProductCatalogForDeleteList.Clear();
                }
                for (System.Int32 i = 0; i < checkedListBoxProductCatalog.Items.Count; i++)
                {
                    if (checkedListBoxProductCatalog.Items[i].CheckState == CheckState.Checked)
                    {
                        m_objSelectedRtt.ProductCatalogList.Add((CProductCatalog)checkedListBoxProductCatalog.Items[i].Value);
                    }
                    else
                    {
                        m_objSelectedRtt.ProductCatalogForDeleteList.Add((CProductCatalog)checkedListBoxProductCatalog.Items[i].Value);
                    }
                }


                // контакты и адреса
                m_objSelectedRtt.ContactList = frmContact.ContactList;
                if (frmContact.ContactDeletedList != null)
                {
                    if (m_objSelectedRtt.ContactForDeleteList == null)
                        { m_objSelectedRtt.ContactForDeleteList = new List<CContact>(); }

                    m_objSelectedRtt.ContactForDeleteList.Clear();

                    foreach (CContact objContact in frmContact.ContactDeletedList)
                    {
                        m_objSelectedRtt.ContactForDeleteList.Add(objContact);
                    }
                }
                m_objSelectedRtt.AddressList = frmAddress.AddressList;
                if (frmAddress.AddressDeletedList != null)
                {
                    
                    if (m_objSelectedRtt.AddressForDeleteList == null)
                        { m_objSelectedRtt.AddressForDeleteList = new List<CAddress>(); }
                    m_objSelectedRtt.AddressForDeleteList.Clear();

                    foreach (CAddress objAddress in frmAddress.AddressDeletedList)
                    {
                        m_objSelectedRtt.AddressForDeleteList.Add(objAddress);
                    }
                }

                //m_objSelectedRtt.AddressForDeleteList = frmAddress.AddressDeletedList;
                // расписание работы
                if (m_objSelectedRtt.ActionPeriod == null)
                {
                    m_objSelectedRtt.ActionPeriod = new CRttActionPeriod();
                }
                m_objSelectedRtt.ActionPeriod.InitActionPeriodPointList();
                foreach (CActionPeriodPoint objPoint in m_objSelectedRtt.ActionPeriod.ActionPeriodPointList)
                {
                    switch (objPoint.DayNum)
                    {
                        case 0:
                            {
                                objPoint.Enable = checkBreak.Checked;
                                objPoint.TimeStart = timeStartBreak.Time;
                                objPoint.TimeFinish = timeFiishBreak.Time;
                                break;
                            }
                        case 1:
                            {
                                objPoint.Enable = checkMonday.Checked;
                                objPoint.TimeStart = timeStartMonday.Time;
                                objPoint.TimeFinish = timeFinishMonday.Time;
                                break;
                            }
                        case 2:
                            {
                                objPoint.Enable = checkTuesday.Checked;
                                objPoint.TimeStart = timeStartTuesday.Time;
                                objPoint.TimeFinish = timeFinishTuesday.Time;
                                break;
                            }
                        case 3:
                            {
                                objPoint.Enable = checkWendsday.Checked;
                                objPoint.TimeStart = timeStartWendsday.Time;
                                objPoint.TimeFinish = timeFinishWendsday.Time;
                                break;
                            }
                        case 4:
                            {
                                objPoint.Enable = checkThursday.Checked;
                                objPoint.TimeStart = timeStartThursday.Time;
                                objPoint.TimeFinish = timeFinishThursday.Time;
                                break;
                            }
                        case 5:
                            {
                                objPoint.Enable = checkFriday.Checked;
                                objPoint.TimeStart = timeStartFriday.Time;
                                objPoint.TimeFinish = timeFinishFriday.Time;
                                break;
                            }
                        case 6:
                            {
                                objPoint.Enable = checkSaturday.Checked;
                                objPoint.TimeStart = timeStartSaturday.Time;
                                objPoint.TimeFinish = timeFinishSaturday.Time;
                                break;
                            }
                        case 7:
                            {
                                objPoint.Enable = checkSunday.Checked;
                                objPoint.TimeStart = timeStartSunday.Time;
                                objPoint.TimeFinish = timeFinishSunday.Time;
                                break;
                            }
                        default:
                            break;
                    }
                }
                
                if (frmAddress.AddressDeletedList != null) { frmAddress.AddressDeletedList.Clear(); }
                if (frmContact.ContactDeletedList != null) { frmContact.ContactDeletedList.Clear(); }

                if (m_objSelectedRtt.IsAllParametersValid(ref strErr) == true)
                {
                    treeListRtt.FocusedNode.SetValue(colRttName, m_objSelectedRtt.VisitingCard);
                    m_objSelectedRtt.IsChanged = true;
                    btnAddRtt.Enabled = true;
                    btnEditRtt.Enabled = true;
                    btnDeleteRtt.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    SetModeReadOnly(true);

                    this.m_bIsChanged = false;

                    bRet = true;
                }
                else
                {
                    SendMessageToLog("Ошибка сохранения изменений в описании РТТ. Текст ошибки: " + strErr);
                }
            }
            catch (System.Exception f)
            {
                strErr += ("Ошибка сохранения изменений в описании РТТ. Текст ошибки: " + f.Message);
                SendMessageToLog(strErr);
            }
            finally
            {
            }
            return bRet;
        }
        /// <summary>
        /// Сохраняет изменения в базе данных
        /// </summary>
        /// <returns>true - удачное завершение операции;false - ошибка</returns>
        private System.Boolean bSaveChangesInDB()
        {
            System.Boolean bRet = false;
            System.Boolean bOkSave = false;
            CRtt objRttForSave = new CRtt();
            try
            {
                objRttForSave.ID = m_objSelectedRtt.ID;
                objRttForSave.FullName = txtFullName.Text;
                objRttForSave.ShortName = txtFullName.Text;
                // признак активности
                if (cboxActiveType.SelectedItem != null)
                {
                    objRttForSave.RttActiveType = (CRttActiveType)cboxActiveType.SelectedItem;
                }
                // спецкод
                if (cboxSpecCode.SelectedItem != null)
                {
                    objRttForSave.RttSpecCode = (CRttSpecCode)cboxSpecCode.SelectedItem;
                }
                // контакты и адреса
                objRttForSave.ContactList = frmContact.ContactList;
                objRttForSave.ContactForDeleteList = frmContact.ContactDeletedList;
                objRttForSave.AddressList = frmAddress.AddressList;
                objRttForSave.AddressForDeleteList = frmAddress.AddressDeletedList;
                // расписание работы
                if (objRttForSave.ActionPeriod == null)
                {
                    objRttForSave.ActionPeriod = new CRttActionPeriod();
                }
                objRttForSave.ActionPeriod.InitActionPeriodPointList();
                foreach (CActionPeriodPoint objPoint in objRttForSave.ActionPeriod.ActionPeriodPointList)
                {
                    switch (objPoint.DayNum)
                    {
                        case 0:
                            {
                                objPoint.Enable = checkBreak.Checked;
                                objPoint.TimeStart = timeStartBreak.Time;
                                objPoint.TimeFinish = timeFiishBreak.Time;
                                break;
                            }
                        case 1:
                            {
                                objPoint.Enable = checkMonday.Checked;
                                objPoint.TimeStart = timeStartMonday.Time;
                                objPoint.TimeFinish = timeFinishMonday.Time;
                                break;
                            }
                        case 2:
                            {
                                objPoint.Enable = checkTuesday.Checked;
                                objPoint.TimeStart = timeStartTuesday.Time;
                                objPoint.TimeFinish = timeFinishTuesday.Time;
                                break;
                            }
                        case 3:
                            {
                                objPoint.Enable = checkWendsday.Checked;
                                objPoint.TimeStart = timeStartWendsday.Time;
                                objPoint.TimeFinish = timeFinishWendsday.Time;
                                break;
                            }
                        case 4:
                            {
                                objPoint.Enable = checkThursday.Checked;
                                objPoint.TimeStart = timeStartThursday.Time;
                                objPoint.TimeFinish = timeFinishThursday.Time;
                                break;
                            }
                        case 5:
                            {
                                objPoint.Enable = checkFriday.Checked;
                                objPoint.TimeStart = timeStartFriday.Time;
                                objPoint.TimeFinish = timeFinishFriday.Time;
                                break;
                            }
                        case 6:
                            {
                                objPoint.Enable = checkSaturday.Checked;
                                objPoint.TimeStart = timeStartSaturday.Time;
                                objPoint.TimeFinish = timeFinishSaturday.Time;
                                break;
                            }
                        case 7:
                            {
                                objPoint.Enable = checkSunday.Checked;
                                objPoint.TimeStart = timeStartSunday.Time;
                                objPoint.TimeFinish = timeFinishSunday.Time;
                                break;
                            }
                        default:
                            break;
                    }
                }


                System.String strErr = "";
                if (m_bNewRtt == true)
                {
                    // новый 
                    bOkSave = objRttForSave.Add(m_objProfile, null, m_objCustomerRtt.ID, ref strErr);
                }
                else
                {
                    bOkSave = objRttForSave.Update(m_objProfile, null, ref strErr);
                }
                SendMessageToLog(strErr);
                if (bOkSave == true)
                {
                    m_objSelectedRtt.ID = objRttForSave.ID;
                    m_objSelectedRtt.FullName = objRttForSave.FullName;
                    m_objSelectedRtt.ShortName = objRttForSave.ShortName;
                    m_objSelectedRtt.Code = objRttForSave.Code;
                    m_objSelectedRtt.RttActiveType = objRttForSave.RttActiveType;
                    m_objSelectedRtt.RttSpecCode = objRttForSave.RttSpecCode;
                    m_objSelectedRtt.ActionPeriod = objRttForSave.ActionPeriod;
                    m_objSelectedRtt.ContactList = objRttForSave.ContactList;
                    m_objSelectedRtt.AddressList = objRttForSave.AddressList;

                    if (frmAddress.AddressDeletedList != null) { frmAddress.AddressDeletedList.Clear(); }
                    if (frmContact.ContactDeletedList != null) { frmContact.ContactDeletedList.Clear(); }
                    if (m_objSelectedRtt.AddressForDeleteList != null) { m_objSelectedRtt.AddressForDeleteList.Clear(); }
                    if (m_objSelectedRtt.ContactForDeleteList != null) { m_objSelectedRtt.ContactForDeleteList.Clear(); }

                    bRet = true;
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в описании РТТ. Текст ошибки: " + f.Message);
            }
            finally
            {
                objRttForSave = null;
            }
            return bRet;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                System.String strErr = "";
                if (bSaveChanges(ref strErr) == true)
                {
                    SimulateChangeRttProperties(m_objSelectedRtt, enumActionSaveCancel.Save, m_bNewRtt);
                    m_bNewRtt = false;
                }
                else
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show( "Изменения в данных РТТ не будут сохранены.\n" + strErr, "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в описании РТТ. Текст ошибки: " + f.Message);
            }
            return;
        }
        public System.Boolean ConfirmChanges( ref System.String strErr )
        {
            System.Boolean bRet = false;
            try
            {
                if (m_bIsChanged == false) { return true; }

                if ((RttList.Count == 0) && ((m_objRttDeletedList == null) || (m_objRttDeletedList.Count == 0))) { return true; }

                frmAddress.ConfirmChanges();
                frmContact.ConfirmChanges();

                bRet = bSaveChanges(ref strErr);
                if (bRet == true)
                {
                    SimulateChangeRttProperties(m_objSelectedRtt, enumActionSaveCancel.Save, m_bNewRtt);
                    m_bNewRtt = false;
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в описании РТТ. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return bRet;
        }

        #endregion

        #region Удалить РТТ
        /// <summary>
        /// Очищает содержимое элементов управления для отображения свойств адреса
        /// </summary>
        public void ClearAllPropertiesControls()
        {
            m_bDisableEvents = true;
            try
            {
                // очистим содержимое элементов управления
                txtFullName.Text = "";
                txtCode.Text = "";
                txtAddress.Text = "";
                txtShedule.Text = "";
                memoEditCustomer.Text = "";

                cboxActiveType.SelectedItem = null;
                cboxSpecCode.SelectedItem = null;
                cboxLicenceType.SelectedItem = null;
                cboxSegmentLevel1.SelectedItem = null;
                cboxSegmentLevel2.SelectedItem = null;
                lblOldSegmenation.Text = "";

                ClearRttShedule();

                treeListEquipment.Nodes.Clear();
                for (System.Int32 i = 0; i < checkedListBoxProductCatalog.Items.Count; i++)
                {
                    checkedListBoxProductCatalog.Items[i].CheckState = CheckState.Unchecked;
                }

                frmAddress.ClearAllPropertiesControls();
                frmAddress.ClearAddressList();
                frmContact.ClearAllPropertiesControls();
                frmContact.ClearContactListTree();

                SetModeReadOnly(true);

            }//try
            catch (System.Exception f)
            {
                SendMessageToLog("ClearAllPropertiesControls. Текст ошибки: " + f.Message);
            }
            finally
            {
                m_bDisableEvents = false;
            }

            return;
        }
        /// <summary>
        /// Удаляет РТТ из списка
        /// </summary>
        /// <param name="objNode">узел с РТТ</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        private System.Boolean DeleteRtt(DevExpress.XtraTreeList.Nodes.TreeListNode objNode)
        {
            System.Boolean bRet = false;
            if ((objNode == null) || (objNode.Tag == null)) { return bRet; }
            try
            {
                if (m_objRttDeletedList == null) { m_objRttDeletedList = new List<CRtt>(); }
                m_objRttDeletedList.Add((CRtt)objNode.Tag);

                System.Int32 iPosNode = treeListRtt.GetNodeIndex(objNode);
                DevExpress.XtraTreeList.Nodes.TreeListNode objPrevNode = objNode.PrevNode;

                treeListRtt.Nodes.RemoveAt(iPosNode);
                if (objPrevNode == null)
                {
                    if (treeListRtt.Nodes.Count > 0)
                    {
                        treeListRtt.FocusedNode = treeListRtt.Nodes[0];
                    }
                }
                else
                {
                    treeListRtt.FocusedNode = objPrevNode;
                }

                if (treeListRtt.FocusedNode == null)
                {
                    ClearAllPropertiesControls();

                    btnAddRtt.Enabled = true;
                    btnEditRtt.Enabled = false;
                    btnDeleteRtt.Enabled = false;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                }
                else
                {
                    btnAddRtt.Enabled = true;
                    btnEditRtt.Enabled = true;
                    btnDeleteRtt.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                }

                bRet = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления РТТ.\n\nТекст ошибки:\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return bRet;
        }
        private void menuDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteRtt(treeListRtt.FocusedNode);
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка удаления РТТ.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        private void btnDeleteRtt_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteRtt(treeListRtt.FocusedNode);
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка удаления РТТ.\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }
        #endregion


        private void layoutControl_GroupExpandChanged(object sender, DevExpress.XtraLayout.Utils.LayoutGroupEventArgs e)
        {
            DevExpress.XtraLayout.LayoutControl LayoutControl = (DevExpress.XtraLayout.LayoutControl)sender;
            if (e.Group.Expanded == true)
            {
                LayoutControl.Size = new Size(LayoutControl.Size.Width, (LayoutControl.MaximumSize.Height));
            }
            else
            {
                LayoutControl.Size = new Size(LayoutControl.Size.Width, iMinControlItemHeight);
            }
            if (this.Parent != null)
            {
                SimulateChangeControlRttSize( new Size(this.Parent.Size.Width, (
                        tableLayoutPanel1.Size.Height + 
                        layoutControlGeneralProperties.Height +
                        layoutControlCustomer.Height + layoutControlSegment.Height +
                        layoutControlShedule.Height + layoutControlEquipment.Height +
                        layoutControlAddress.Height + layoutControlContact.Height +
                        layoutControlProductCatalog.Height + iAdvControlItemHeight)));
            }
        }
        private void ctrlRtt_Resize(object sender, EventArgs e)
        {
            layoutControlAddress.MaximumSize = new Size(this.Size.Width, layoutControlAddress.MaximumSize.Height);
            layoutControlContact.MaximumSize = new Size(this.Size.Width, layoutControlContact.MaximumSize.Height);
            layoutControlCustomer.MaximumSize = new Size(this.Size.Width, layoutControlCustomer.MaximumSize.Height);
            layoutControlEquipment.MaximumSize = new Size(this.Size.Width, layoutControlEquipment.MaximumSize.Height);
            layoutControlSegment.MaximumSize = new Size(this.Size.Width, layoutControlSegment.MaximumSize.Height);
            layoutControlShedule.MaximumSize = new Size(this.Size.Width, layoutControlShedule.MaximumSize.Height);
            layoutControlProductCatalog.MaximumSize = new Size(this.Size.Width, layoutControlProductCatalog.MaximumSize.Height);
        }

        private void checkedListBoxProductCatalog_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
                SetPropertiesModified(true);
            }//try
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка изменения списка товарных марок. Текст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;

        }


    }


    /// <summary>
    /// Тип, хранящий информацию, которая передается получателям уведомления о событии
    /// </summary>
    public partial class ChangeRttPropertieEventArgs : EventArgs
    {
        private readonly CRtt m_objRtt;
        public CRtt Rtt
        { get { return m_objRtt; } }

        private readonly enumActionSaveCancel m_enActionType;
        public enumActionSaveCancel ActionType
        { get { return m_enActionType; } }

        private readonly System.Boolean m_bIsNewRtt;
        public System.Boolean IsNewRtt
        { get { return m_bIsNewRtt; } }

        public ChangeRttPropertieEventArgs(CRtt objRtt, enumActionSaveCancel enActionType, System.Boolean bIsNewRtt)
        {
            m_objRtt = objRtt;
            m_enActionType = enActionType;
            m_bIsNewRtt = bIsNewRtt;
        }
    }

    /// <summary>
    /// Тип, хранящий информацию, которая передается получателям уведомления о событии
    /// </summary>
    public partial class ChangeControlRttSizeEventArgs : EventArgs
    {
        private readonly Size m_objSize;
        public Size ControlRttSize
        { get { return m_objSize; } }

        public ChangeControlRttSizeEventArgs( Size objSize )
        {
            m_objSize = objSize;
        }
    }

}
