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
    public partial class ctrlSetting : UserControl
    {
        #region Свойства, переменные
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;
        private CSetting m_objSetting;
        public CSetting Setting
        {
            get { return m_objSetting; }
        }
        private System.Boolean m_bIsChanged;
        private System.Boolean m_bDisableEvents;
        private System.Boolean m_bIsReadOnly;
        #endregion

        #region События
        // Создаем закрытое поле, ссылающееся на заголовок списка делегатов
        private EventHandler<ChangeSettingEventArgs> m_ChangeSetting;
        // Создаем в классе член-событие
        public event EventHandler<ChangeSettingEventArgs> ChangeSetting
        {
            add
            {
                // берем закрытую блокировку и добавляем обработчик
                // (передаваемый по значению) в список делегатов
                m_ChangeSetting += value;
            }
            remove
            {
                // берем закрытую блокировку и удаляем обработчик
                // (передаваемый по значению) из списка делегатов
                m_ChangeSetting -= value;
            }
        }
        /// <summary>
        /// Инициирует событие и уведомляет о нем зарегистрированные объекты
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnChangeSetting(ChangeSettingEventArgs e)
        {
            // Сохраняем поле делегата во временном поле для обеспечение безопасности потока
            EventHandler<ChangeSettingEventArgs> temp = m_ChangeSetting;
            // Если есть зарегистрированные объектв, уведомляем их
            if (temp != null) temp(this, e);
        }
        public void SimulateChangeSetting(enumActionSaveCancel enActionType)
        {
            // Создаем объект, хранящий информацию, которую нужно передать
            // объектам, получающим уведомление о событии
            ChangeSettingEventArgs e = new ChangeSettingEventArgs(enActionType);

            // Вызываем виртуальный метод, уведомляющий наш объект о возникновении события
            // Если нет типа, переопределяющего этот метод, наш объект уведомит все объекты, 
            // подписавшиеся на уведомление о событии
            OnChangeSetting(e);
        }
        #endregion

        #region Конструктор
        public ctrlSetting(UniXP.Common.CProfile objProfile, UniXP.Common.MENUITEM objMenuItem)
        {
            m_objProfile = objProfile;
            m_objMenuItem = objMenuItem;
            m_objSetting = null;
            m_bIsChanged = false;
            m_bDisableEvents = false;

            InitializeComponent();
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
        }
        private void treeListParams_CellValueChanging(object sender, DevExpress.XtraTreeList.CellValueChangedEventArgs e)
        {
            try
            {
                if (m_bDisableEvents == true) { return; }
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
        private void treeListParams_BeforeFocusNode(object sender, DevExpress.XtraTreeList.BeforeFocusNodeEventArgs e)
        {
            colParamValue.OptionsColumn.AllowEdit = (e.Node.HasChildren == false);
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
                treeListParams.Enabled = !bSet;
                treeListParams.FocusedNode = treeListParams.Nodes[0];
                treeListParams_BeforeFocusNode(treeListParams, new DevExpress.XtraTreeList.BeforeFocusNodeEventArgs(null, treeListParams.Nodes[0]));

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

        #region Редактирование свойств настройки
        /// <summary>
        /// Загружает cвойства настройки
        /// </summary>
        /// <param name="objSetting">настройка</param>
        public void LoadSettingProperties(CSetting objSetting)
        {
            m_bDisableEvents = true;
            if (objSetting == null) { return; }
            try
            {
                m_objSetting = objSetting;
                lblSettingName.Text = m_objSetting.Name;

                tableLayoutPanel1.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(this.treeListParams)).BeginInit();

                treeListParams.Nodes.Clear();

                if (m_objSetting.xmldocAdvancedParamList != null)
                {
                    foreach (System.Xml.XmlNode objNode in m_objSetting.xmldocAdvancedParamList.ChildNodes)
                    {
                        AddNode(treeListParams, null, objNode);
                    }
                }

                //if (m_objSetting.ParamList != null)
                //{
                //    System.String strGroupName = "";
                //    DevExpress.XtraTreeList.Nodes.TreeListNode objGroupNode = null;
                //    foreach (CAdvancedParam objParam in m_objSetting.ParamList)
                //    {
                //        if ((objParam.GroupName != "") && (objParam.GroupName != strGroupName))
                //        {
                //            strGroupName = objParam.GroupName;
                //            objGroupNode = treeListParams.AppendNode(new object[] { strGroupName, null }, null);
                //        }
                //        treeListParams.AppendNode(new object[] { objParam.Name, objParam.Value }, objGroupNode).Tag = objParam;
                //    }
                //}
                treeListParams.ExpandAll();
                this.tableLayoutPanel1.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(this.treeListParams)).EndInit();

                SetPropertiesModified(false);

                SetModeReadOnly(true);
                btnCancel.Enabled = true;
                btnCancel.Focus();
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка редактирования описания настройки. Текст ошибки: " + f.Message);
            }
            finally
            {
                m_bDisableEvents = false;
            }
            return;
        }
        public void AddNode( DevExpress.XtraTreeList.TreeList objTreeList, 
            DevExpress.XtraTreeList.Nodes.TreeListNode objRootNode, System.Xml.XmlNode objNode )
        {
            try
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode objAppendedNode = objTreeList.AppendNode(new object[] { objNode.Name, objNode.Value }, objRootNode);
                if (objNode.ChildNodes.Count != 0)
                {
                    foreach (System.Xml.XmlNode objNodeChild in objNode.ChildNodes)
                    {
                        AddNode(objTreeList, objAppendedNode, objNodeChild);
                    }
                }
                else
                {
                    foreach (System.Xml.XmlAttribute objAttr in objNode.Attributes)
                    {
                        objTreeList.AppendNode(new object[] { objAttr.Name, objAttr.Value }, objAppendedNode);
                    }
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка AddNode. Текст ошибки: " + f.Message);
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
                SendMessageToLog("Ошибка отмены изменений в описании настройки. Текст ошибки: " + f.Message);
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
                SimulateChangeSetting(enumActionSaveCancel.Cancel);
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка отмены изменений в описании настройки. Текст ошибки: " + f.Message);
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

            try
            {
                //CAdvancedParam objAdvancedParam = null;
                CSetting objSettingForSave = new CSetting();
                objSettingForSave.Name = m_objSetting.Name;
                objSettingForSave.ID = m_objSetting.ID;
                objSettingForSave.xmldocAdvancedParamList = m_objSetting.xmldocAdvancedParamList;
                objSettingForSave.ParamList = new List<CAdvancedParam>();
                if (objSettingForSave.xmldocAdvancedParamList != null)
                {
                    for( System.Int32 i=0; i< objSettingForSave.xmldocAdvancedParamList.ChildNodes.Count; i++ )
                    {
                        SetNode( objSettingForSave.xmldocAdvancedParamList.ChildNodes[i], treeListParams.Nodes[i] );
                    }
                }
                System.String strErr = "";
                if (objSettingForSave.SaveSettingInDB(m_objProfile, null, ref strErr) == true)
                {
                    m_objSetting.xmldocAdvancedParamList = objSettingForSave.xmldocAdvancedParamList;
                    bRet = true;
                }
                else
                {
                    SendMessageToLog("Ошибка сохранения изменений в описании настройки. Текст ошибки: " + strErr);
                }

            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в описании настройки. Текст ошибки: " + f.Message);
            }
            finally
            {
            }
            return bRet;
        }

        private void SetNode(System.Xml.XmlNode objXmlNode, DevExpress.XtraTreeList.Nodes.TreeListNode objNode )
        {
            try
            {
                if ((objXmlNode.ChildNodes.Count == 0) && (objXmlNode.Attributes.Count > 0) && (objNode.HasChildren == true))
                {
                    foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objChildNode in objNode.Nodes)
                    {
                        objXmlNode.Attributes[System.Convert.ToString(objChildNode.GetValue(colParamName))].Value = System.Convert.ToString(objChildNode.GetValue(colParamValue));
                    }
                }
                else if ( (objXmlNode.ChildNodes.Count > 0) && (objNode.HasChildren == true))
                {
                    for( System.Int32 i = 0; i< objXmlNode.ChildNodes.Count; i++)
                    {
                        SetNode( objXmlNode.ChildNodes[ i ], objNode.Nodes[i] );
                    }
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка SetNode. Текст ошибки: " + f.Message);
            }
            return;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (bSaveChanges() == true)
                {
                    SimulateChangeSetting(enumActionSaveCancel.Save);
                    SetModeReadOnly(true);
                }
            }
            catch (System.Exception f)
            {
                SendMessageToLog("Ошибка сохранения изменений в описании настройки. Текст ошибки: " + f.Message);
            }
            return;
        }

        #endregion

    }

    /// <summary>
    /// Тип, хранящий информацию, которая передается получателям уведомления о событии
    /// </summary>
    public class ChangeSettingEventArgs : EventArgs
    {
        private readonly enumActionSaveCancel m_enActionType;
        public enumActionSaveCancel ActionType
        { get { return m_enActionType; } }


        public ChangeSettingEventArgs(enumActionSaveCancel enActionType)
        {
            m_enActionType = enActionType;
        }
    }

}
