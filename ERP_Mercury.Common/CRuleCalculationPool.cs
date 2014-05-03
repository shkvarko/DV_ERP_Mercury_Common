using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP_Mercury.Common
{
    /// <summary>
    /// Варианты возможных действий при отработке шага в пуле правил
    /// </summary>
    public enum enRuleActionType
    {
        Unkown = -1,
        Exit = 0,
        GoToNextStep = 1,
        GoToStep = 2
    }
    /// <summary>
    /// Класс "Действие после отработки шага"
    /// </summary>
    public class CPoolItemAction
    {
        #region Свойства
        /// <summary>
        /// тип действия
        /// </summary>
        private enRuleActionType m_enRuleActionType;
        /// <summary>
        /// тип действия
        /// </summary>
        public enRuleActionType enumRuleActionType
        {
            get { return m_enRuleActionType; }
            set { m_enRuleActionType = value; }
        }
        /// <summary>
        /// идентификатор шага
        /// </summary>
        private System.Guid m_uuidStepID;
        /// <summary>
        /// идентификатор шага
        /// </summary>
        public System.Guid StepID
        {
            get { return m_uuidStepID; }
            set { m_uuidStepID = value; }
        }
        /// <summary>
        /// имя
        /// </summary>
        private System.String m_strName;
        /// <summary>
        /// имя
        /// </summary>
        public System.String Name
        {
            get { return m_strName; }
            set { m_strName = value; }
        }
        private const System.String m_strExit = "Выход";
        private const System.String m_strNextStep = "Переход к следующему шагу";
        public static System.String GoToStepPrefix = "Перейти к: шаг №";
        #endregion

        #region Конструктор
        public CPoolItemAction()
        {
            m_enRuleActionType = enRuleActionType.Unkown;
            m_uuidStepID = System.Guid.Empty;
            m_strName = "";
        }
        public CPoolItemAction(System.Guid uuidStepID, enRuleActionType objRuleActionType, System.String strName)
        {
            m_enRuleActionType = objRuleActionType;
            m_uuidStepID = uuidStepID;
            if (strName == "")
            {
                switch (objRuleActionType)
                {
                    case enRuleActionType.Exit:
                        {
                            strName = m_strExit;
                            break;
                        }
                    case enRuleActionType.GoToNextStep:
                        {
                            strName = m_strNextStep;
                            break;
                        }
                    default:
                        break;
                }
            }
            m_strName = strName;
        }
        #endregion

        #region Список возможных действий
        /// <summary>
        /// Возвращает список действий
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <returns>список действий</returns>
        public static List<CPoolItemAction> GetPoolItemActionList(UniXP.Common.CProfile objProfile)
        {
            List<CPoolItemAction> objList = new List<CPoolItemAction>();
            try
            {
                System.Data.SqlClient.SqlConnection DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Отсутствует соединение с базой данных.", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return objList;
                }

                // соединение с БД получено, прописываем команду на выборку данных
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetRulePoolAction]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    while (rs.Read())
                    {
                        objList.Add( new CPoolItemAction( System.Guid.Empty, (enRuleActionType)rs["RulePoolAction_Id"], (System.String)rs["RulePoolAction_Name"] ) );
                    }
                }
                rs.Close();
                rs.Dispose();
                cmd.Dispose();
                DBConnection.Close();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список действий.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        #endregion

        public override string ToString()
        {
            return Name;
        }
    }
    /// <summary>
    /// Класс "Шаг в пуле правил"
    /// </summary>
    public class CPoolRuleCalculationStep
    {
        #region Свойства
        /// <summary>
        /// уникальный идентификатор
        /// </summary>
        private System.Guid m_uuidID;
        /// <summary>
        /// уникальный идентификатор
        /// </summary>
        public System.Guid ID
        {
            get { return m_uuidID; }
        }
        /// <summary>
        /// наименование
        /// </summary>
        private System.String m_strName;
        /// <summary>
        /// наименование
        /// </summary>
        public System.String Name
        {
            get { return m_strName; }
            set { m_strName = value; }
        }
        /// <summary>
        /// Номер шага в списке
        /// </summary>
        private System.Int32 m_iStepID;
        /// <summary>
        /// Номер шага в списке
        /// </summary>
        public System.Int32 StepID
        {
            get { return m_iStepID; }
            set { m_iStepID = value; }
        }
        /// <summary>
        /// Правило вычисления цены
        /// </summary>
        private CRuleCalculation m_objRuleCalculation;
        /// <summary>
        /// Правило вычисления цены
        /// </summary>
        public CRuleCalculation RuleCalculation
        {
            get { return m_objRuleCalculation; }
            set { m_objRuleCalculation = value; }
        }
        /// <summary>
        /// Действие в результате успешной отработки шага
        /// </summary>
        private CPoolItemAction m_ActionSuccess;
        /// <summary>
        /// Действие в результате успешной отработки шага
        /// </summary>
        public CPoolItemAction ActionSuccess
        {
            get { return m_ActionSuccess; }
            set { m_ActionSuccess = value; }
        }
        /// <summary>
        /// Действие в результате неудачной отработки шага
        /// </summary>
        private CPoolItemAction m_ActionFailure;
        /// <summary>
        /// Действие в результате неудачной отработки шага
        /// </summary>
        public CPoolItemAction ActionFailure
        {
            get { return m_ActionFailure; }
            set { m_ActionFailure = value; }
        }
        /// <summary>
        /// признак "шаг включен"
        /// </summary>
        /// <summary>
        /// признак "шаг включен"
        private System.Boolean m_bEnable;
        public System.Boolean Enable
        {
            get { return m_bEnable; }
            set { m_bEnable = value; }
        }
        /// <summary>
        /// Дата начала действия правила
        /// </summary>
        private System.DateTime m_dtBeginDate;
        /// <summary>
        /// Дата начала действия правила
        /// </summary>
        public System.DateTime BeginDate
        {
            get { return m_dtBeginDate; }
            set { m_dtBeginDate = value; }
        }
        /// <summary>
        /// Дата окончания действия правила
        /// </summary>
        private System.DateTime m_dtEndDate;
        /// <summary>
        /// Дата окончания действия правила
        /// </summary>
        public System.DateTime EndDate
        {
            get { return m_dtEndDate; }
            set { m_dtEndDate = value; }
        }
        /// <summary>
        /// Список групп объектов (допустимых/не допустимых)
        /// </summary>
        private List<CConditionGroup> m_objConditionGroupList;
        /// <summary>
        /// Список групп объектов (допустимых/не допустимых)
        /// </summary>
        public List<CConditionGroup> ConditionGroupList
        {
            get { return m_objConditionGroupList; }
            set { m_objConditionGroupList = value; }
        }
        /// <summary>
        /// Список параметров
        /// </summary>
        private List<CAdvancedParam> m_objAdvancedParamList;
        /// <summary>
        /// Список параметров
        /// </summary>
        public List<CAdvancedParam> AdvancedParamList
        {
            get { return m_objAdvancedParamList; }
            set { m_objAdvancedParamList = value; }
        }
        /// <summary>
        /// Список параметров в xml - виде
        /// </summary>
        private System.Xml.XmlDocument m_xmldocAdvancedParamList;
        /// <summary>
        /// Список параметров в xml - виде
        /// </summary>
        public System.Xml.XmlDocument xmldocAdvancedParamList
        {
            get { return m_xmldocAdvancedParamList; }
            set { m_xmldocAdvancedParamList = value; }
        }
        /// <summary>
        /// Описание
        /// </summary>
        private System.String m_strDescription;
        /// <summary>
        /// Описание
        /// </summary>
        public System.String Description
        {
            get { return m_strDescription; }
            set { m_strDescription = value; }
        }
        /// <summary>
        /// Описание акции
        /// </summary>
        public System.String RulePool_Description { get; set; }
        /// <summary>
        /// Признак "Отображать описание для торговых представителей"
        /// </summary>
        public System.Boolean RulePool_IsCanShow { get; set; }

        #endregion

        #region Конструктор
        public CPoolRuleCalculationStep()
        {
            m_uuidID = System.Guid.Empty;
            m_iStepID = 0;
            m_strName = "";
            m_objRuleCalculation = null;
            m_ActionFailure = null;
            m_ActionSuccess = null;
            m_bEnable = false;
            m_dtBeginDate = System.DateTime.Now;
            m_dtEndDate = System.DateTime.Now;
            m_objConditionGroupList = null;
            m_objAdvancedParamList = null;
            m_xmldocAdvancedParamList = null;
            m_strDescription = "";
            RulePool_Description = "";
            RulePool_IsCanShow = false;
        }
        public CPoolRuleCalculationStep(System.Guid uuidID, System.Int32 iStepID, System.String strName,
            CRuleCalculation objRuleCalculation, CPoolItemAction ActionSuccess, CPoolItemAction ActionFailure,
            System.Boolean bEnable, System.DateTime BeginDate, System.DateTime EndDate, System.String strDescription)
        {
            m_uuidID = uuidID;
            m_iStepID = iStepID;
            m_strName = strName;
            m_objRuleCalculation = objRuleCalculation;
            m_ActionFailure = ActionFailure;
            m_ActionSuccess = ActionSuccess;
            m_bEnable = bEnable;
            m_dtBeginDate = BeginDate;
            m_dtEndDate = EndDate;
            m_objConditionGroupList = null;
            m_objAdvancedParamList = null;
            m_xmldocAdvancedParamList = null;
            m_strDescription = strDescription;
            RulePool_Description = "";
            RulePool_IsCanShow = false;
        }
        #endregion

        #region Список шагов в пуле правил
        /// <summary>
        /// Возвращает список шагов в пуле правил
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>список шагов в пуле правил</returns>
        public static List<CPoolRuleCalculationStep> GetRuleCalculationList( System.Guid uuidRulePoolStepGuid,
            UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL, System.Guid uuidRuleType)
        {
            List<CPoolRuleCalculationStep> objList = new List<CPoolRuleCalculationStep>();
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                            "Не удалось получить соединение с базой данных.", "Внимание",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return objList;
                    }
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetRulePoolSteps]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                if (uuidRulePoolStepGuid.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RulePool_StepGuid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@RulePool_StepGuid"].Value = uuidRulePoolStepGuid;
                }
                if (uuidRuleType.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RuleType_Guid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@RuleType_Guid"].Value = uuidRuleType;
                }
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.String strDscrpn = "";
                    System.String strDscrpnSP = "";
                    System.String strSuccessActionName = "";
                    System.String strFailureActionName = "";
                    System.Guid uuidSuccessActionStepGuid = System.Guid.Empty;
                    System.Guid uuidFailureActionStepGuid = System.Guid.Empty;
                    CPoolItemAction objSuccessAction = null;
                    CPoolItemAction objFailureAction = null;

                    CPoolRuleCalculationStep objStep = null;
                    CRuleCalculation objRule = null;
                    CAdvancedParam objAdvParam = null;
                    while (rs.Read())
                    {
                        strDscrpn = (rs["RuleCalculation_Description"] == System.DBNull.Value) ? "" : (System.String)rs["RuleCalculation_Description"];
                        strDscrpnSP = (rs["StoredProcedure_Description"] == System.DBNull.Value) ? "" : (System.String)rs["StoredProcedure_Description"];
                        uuidSuccessActionStepGuid = (rs["SuccessAction_StepGuid"] == System.DBNull.Value) ? System.Guid.Empty : (System.Guid)rs["SuccessAction_StepGuid"];
                        uuidFailureActionStepGuid = (rs["FailureAction_StepGuid"] == System.DBNull.Value) ? System.Guid.Empty : (System.Guid)rs["FailureAction_StepGuid"];
                        strSuccessActionName = (rs["SuccessActionName"] == System.DBNull.Value) ? "" : (System.String)rs["SuccessActionName"];
                        strFailureActionName = (rs["FailureActionName"] == System.DBNull.Value) ? "" : (System.String)rs["FailureActionName"];
                        // правило
                        objRule = new CRuleCalculation((System.Guid)rs["RuleCalculation_Guid"],
                            (System.String)rs["RuleCalculation_Name"], strDscrpn,
                            new CStoredProcedure((System.Guid)rs["StoredProcedure_Guid"],
                            (System.String)rs["StoredProcedure_Name"], strDscrpnSP),
                            new CRuleType((System.Guid)rs["RuleType_Guid"], (System.String)rs["RuleType_Name"], (rs["RuleType_Description"] == System.DBNull.Value ? "" : (System.String)rs["RuleType_Description"])));
                        // действие в случае успешной отработки шага
                        objSuccessAction = new CPoolItemAction(uuidSuccessActionStepGuid, (enRuleActionType)rs["SuccessAction_Id"], strSuccessActionName);
                        // действие в случае неуспешной отработки шага
                        objFailureAction = new CPoolItemAction(uuidFailureActionStepGuid, (enRuleActionType)rs["FailureAction_Id"], strFailureActionName);
                        // шаг в пуле
                        objStep = new CPoolRuleCalculationStep((System.Guid)rs["RulePool_StepGuid"], 
                            (System.Int32)rs["RulePool_StepId"], (System.String)rs["RulePool_StepName"],
                            objRule, objSuccessAction, objFailureAction, (System.Boolean)rs["RulePool_StepEnable"],
                            (System.DateTime)rs["RulePool_BeginDate"], (System.DateTime)rs["RulePool_EndDate"],
                            (System.String)rs["RulePoolStepDescription"])
                            {
                                RulePool_Description = ((rs["RulePool_Description"] != System.DBNull.Value) ? System.Convert.ToString(rs["RulePool_Description"]) : ""),
                                RulePool_IsCanShow = System.Convert.ToBoolean(rs["RulePool_IsCanShow"])
                            };
                        if (rs["RulePool_StepParamsValue"] != System.DBNull.Value)
                        {
                            System.Xml.XmlDocument docInfoAboutParam = new System.Xml.XmlDocument();
                            docInfoAboutParam.LoadXml((System.String)rs["RulePool_StepParamsValue"]);
                            if ((docInfoAboutParam != null) && (docInfoAboutParam.ChildNodes.Count > 0))
                            {
                                objStep.m_xmldocAdvancedParamList = docInfoAboutParam;
                                foreach (System.Xml.XmlNode objNode in docInfoAboutParam.ChildNodes)
                                {
                                    if (objNode.Name == "SP_Param")
                                    {
                                        foreach (System.Xml.XmlNode objChildNode in objNode.ChildNodes)
                                        {
                                            if (objChildNode.Name == "Param")
                                            {
                                                // мы добрались до списка элементов, описывающих параметры процедуры
                                                objAdvParam = new CAdvancedParam(System.Guid.Empty, objChildNode.Attributes["Name"].Value, "", new CParamDataType(0, objChildNode.Attributes["Type"].Value));
                                                objAdvParam.Value = objChildNode.Attributes["Value"].Value;
                                                if (objStep.m_objAdvancedParamList == null) { objStep.m_objAdvancedParamList = new List<CAdvancedParam>(); }
                                                objStep.m_objAdvancedParamList.Add(objAdvParam);
                                            }
                                        }
                                    }
                                }
                            }
                        }


                        objList.Add(objStep);
                    }
                    objSuccessAction = null;
                    objFailureAction = null;
                    objStep = null;
                    objRule = null;
                    objAdvParam = null;
                }
                rs.Dispose();

                // а теперь нужно загрузить список групп для каждого шага
                foreach (CPoolRuleCalculationStep objStep in objList)
                {
                    objStep.LoadConditionGroupsList(objProfile, cmd);
                }
                // это еще не все! а теперь нужно загрузить список параметров для действия
                foreach (CPoolRuleCalculationStep objStep2 in objList)
                {
                    objStep2.RuleCalculation.LoadParamList2(objProfile, cmd, objStep2.RuleCalculation.StoredProcedure.Name);
                }
                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список шагов в пуле правил.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return objList;
        }
        /// <summary>
        /// Загружает список связанных с шагом групп объектов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmdSQL">SQL-команда</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        private System.Boolean LoadConditionGroupsList(
            UniXP.Common.CProfile objProfile, System.Data.SqlClient.SqlCommand cmdSQL)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {
                if (this.m_objConditionGroupList == null)
                {
                    this.m_objConditionGroupList = new List<CConditionGroup>();
                }
                this.m_objConditionGroupList.Clear();

                if (cmdSQL == null)
                {
                    DBConnection = objProfile.GetDBSource();
                    if (DBConnection == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                            "Не удалось получить соединение с базой данных.", "Внимание",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                        return bRet;
                    }
                    cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Connection = DBConnection;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                }
                else
                {
                    cmd = cmdSQL;
                    cmd.Parameters.Clear();
                }

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_GetConditionGroupListForRulePoolStep]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RulePool_StepGuid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RulePool_StepGuid"].Value = this.m_uuidID;
                System.Data.SqlClient.SqlDataReader rs = cmd.ExecuteReader();
                if (rs.HasRows)
                {
                    System.String strDscrpn = "";
                    System.String strDscrpnType = "";
                    while (rs.Read())
                    {
                        strDscrpn = (rs["ConditionGroup_Description"] == System.DBNull.Value) ? "" : (System.String)rs["ConditionGroup_Description"];
                        strDscrpnType = (rs["ConditionGroupType_Description"] == System.DBNull.Value) ? "" : (System.String)rs["ConditionGroupType_Description"];

                        this.m_objConditionGroupList.Add(new CConditionGroup((System.Guid)rs["ConditionGroup_Guid"],
                            (System.String)rs["ConditionGroup_Name"], strDscrpn,
                            new CConditionGroupType((System.Guid)rs["ConditionGroupType_Guid"],
                            (System.String)rs["ConditionGroupType_Name"], strDscrpnType)));
                    }
                }
                rs.Dispose();
                bRet = true;
                if (cmdSQL == null)
                {
                    cmd.Dispose();
                    DBConnection.Close();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось получить список групп для шага.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return bRet;
        }
        #endregion

        #region Сохранить в БД список групп, связанных с шагом в пуле правил
        /// <summary>
        /// Сохраняет в БД список групп, связанных с шагом в пуле правил
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmd">SQL-команда</param>
        /// <param name="strErr">строка с текстом ошибки</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        private System.Boolean SaveConditionGroupList(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmd, ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (cmd == null)
                {
                    strErr = "Не удалось получить соединение с базой данных.";
                    return bRet;
                }
                if (this.ConditionGroupList == null)
                {
                    strErr = "Не определен список групп.";
                    return true;
                }

                // сперва удалим список групп
                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteFromRulePoolConditionGroups]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RulePool_StepGuid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RulePool_StepGuid"].Value = this.m_uuidID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;

                if (iRes == 0)
                {
                    if ((this.ConditionGroupList != null) && (this.ConditionGroupList.Count > 0))
                    {
                        // теперь в цикле добавим в БД каждый член из списка
                        cmd.Parameters.Clear();
                        cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddToRulePoolConditionGroup]", objProfile.GetOptionsDllDBName());
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ConditionGroup_Guid", System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RulePool_StepGuid", System.Data.SqlDbType.UniqueIdentifier));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                        cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters["@RulePool_StepGuid"].Value = this.m_uuidID;
                        iRes = 0;
                        foreach (CConditionGroup objItem in this.ConditionGroupList)
                        {
                            cmd.Parameters["@ConditionGroup_Guid"].Value = objItem.ID;
                            cmd.ExecuteNonQuery();
                            iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                            if (iRes != 0)
                            {
                                strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                bRet = (iRes == 0);

            }
            catch (System.Exception f)
            {
                strErr = "Не удалось сохранить список групп, связанных с шагом в пуле правил.\n\nТекст ошибки: " + f.Message;
                //DevExpress.XtraEditors.XtraMessageBox.Show(
                //"Не удалось сохранить список групп, связанных с правилом.\n\nТекст ошибки: " + f.Message, "Внимание",
                //System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return bRet;
        }
        /// <summary>
        /// Сохраняет в БД список групп, связанных с правилом
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean SaveConditionGroupList(UniXP.Common.CProfile objProfile)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            System.String strErr = "";
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Не удалось получить соединение с базой данных.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return bRet;
                }
                DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                bRet = this.SaveConditionGroupList(objProfile, cmd, ref strErr);

                if (bRet == true)
                {
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    // откатываем транзакцию
                    DBTransaction.Rollback();
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка сохранения списка групп, связанных с шагом в пуле правил.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось сохраненить список групп, связанных с шагом в пуле правил.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        #endregion

        #region Добавить шаг в базу данных
        /// <summary>
        /// Проверка свойств шага перед сохранением в базе данных
        /// </summary>
        /// <returns>true - все свойства определены; false - не все свойства определены</returns>
        private System.Boolean IsAllParametersValid()
        {
            System.Boolean bRet = false;
            try
            {
                if (this.Name == "")
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать имя шага.", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.RuleCalculation == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать правило, связанное с шагом.", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if ( this.RuleCalculation.xmldocAdvancedParamList != null)
                {
                    if (this.AdvancedParamList == null)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Необходимо определить парамеры, связанные с хранимой процедурой.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                        return bRet;
                    }
                }
                if (this.ActionSuccess == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать действие, выполняемое в случае успешной отработки шана.", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                if (this.ActionFailure == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Необходимо указать действие, выполняемое в случае неуспешной отработки шана.", "Внимание",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }
                bRet = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка проверки свойств шага.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return bRet;
        }
        /// <summary>
        /// добавляет шаг в базу данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean AddStepToDB(UniXP.Common.CProfile objProfile)
        {
            System.Boolean bRet = false;
            if (IsAllParametersValid() == false) { return bRet; }

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            System.String strErr = "";
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Не удалось получить соединение с базой данных.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return bRet;
                }
                DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_AddStepToRulePool]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RulePool_StepGuid", System.Data.SqlDbType.UniqueIdentifier, 4, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RuleCalculation_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RulePool_StepId", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RulePool_StepName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SuccessAction_Id", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FailureAction_Id", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RulePool_StepEnable", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RulePool_BeginDate", System.Data.DbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RulePool_EndDate", System.Data.DbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RulePool_StepParamsValue", System.Data.SqlDbType.Xml));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RulePool_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RulePool_IsCanShow", System.Data.SqlDbType.Bit));

                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                if (this.ActionSuccess.StepID.CompareTo( System.Guid.Empty ) != 0 )
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SuccessAction_StepGuid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@SuccessAction_StepGuid"].Value = this.ActionSuccess.StepID;
                }
                if (this.ActionFailure.StepID.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FailureAction_StepGuid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@FailureAction_StepGuid"].Value = this.ActionFailure.StepID;
                }
                cmd.Parameters["@RuleCalculation_Guid"].Value = this.RuleCalculation.ID;
                cmd.Parameters["@RulePool_StepId"].Value = this.StepID;
                cmd.Parameters["@RulePool_StepName"].Value = this.Name;
                cmd.Parameters["@SuccessAction_Id"].Value = System.Convert.ToInt32( this.ActionSuccess.enumRuleActionType );
                cmd.Parameters["@FailureAction_Id"].Value = System.Convert.ToInt32( this.ActionFailure.enumRuleActionType );
                cmd.Parameters["@RulePool_StepEnable"].Value = this.Enable;
                cmd.Parameters["@RulePool_BeginDate"].Value = this.BeginDate;
                cmd.Parameters["@RulePool_EndDate"].Value = this.EndDate;
                cmd.Parameters["@RulePool_StepParamsValue"].Value = this.xmldocAdvancedParamList.InnerXml;
                cmd.Parameters["@RulePool_IsCanShow"].Value = this.RulePool_IsCanShow;
                cmd.Parameters["@RulePool_StepParamsValue"].Value = this.xmldocAdvancedParamList.InnerXml;

                cmd.Parameters["@RulePool_IsCanShow"].Value = this.RulePool_IsCanShow;
                cmd.Parameters["@RulePool_StepParamsValue"].Value = this.xmldocAdvancedParamList.InnerXml;

                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes == 0)
                {
                    this.m_uuidID = (System.Guid)cmd.Parameters["@RulePool_StepGuid"].Value;
                    bRet = true;
                    if (bRet == true)
                    {
                        bRet = this.SaveConditionGroupList(objProfile, cmd, ref strErr);
                        if (bRet == true)
                        {
                            // подтверждаем транзакцию
                            DBTransaction.Commit();
                        }
                    }
                    else
                    {
                        // откатываем транзакцию
                        DBTransaction.Rollback();
                        DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка добавления шага.\n\nТекст ошибки: " + strErr, "Ошибка",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
                else
                {
                    DBTransaction.Rollback();
                    System.String strErrText = (cmd.Parameters["@ERROR_MES"].Value == System.DBNull.Value) ? "" : (System.String)cmd.Parameters["@ERROR_MES"].Value;
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка добавления шага.\n\nТекст ошибки: " + strErrText, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось добавить шаг.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        #endregion

        #region Изменить свойства шага в базе данных
        /// <summary>
        /// Редактирование свойств шага в базе данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmd">SQL-команда</param>
        /// <param name="strErr">текс ошибки</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public System.Boolean EditStepInDB(UniXP.Common.CProfile objProfile,
           System.Data.SqlClient.SqlCommand cmd, ref System.String strErr)
        {
            System.Boolean bRet = false;
            if (IsAllParametersValid() == false) { return bRet; }
            try
            {
                if (cmd == null)
                {
                    strErr = "Не удалось получить соединение с базой данных.";
                    return bRet;
                }
                System.Int32 iRes = 0;
                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_EditStepInRulePool]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RulePool_StepGuid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RuleCalculation_Guid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RulePool_StepId", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RulePool_StepName", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SuccessAction_Id", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FailureAction_Id", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RulePool_StepEnable", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RulePool_BeginDate", System.Data.DbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RulePool_EndDate", System.Data.DbType.Date));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RulePool_StepParamsValue", System.Data.SqlDbType.Xml));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                if (this.ActionSuccess.StepID.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SuccessAction_StepGuid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@SuccessAction_StepGuid"].Value = this.ActionSuccess.StepID;
                }
                if (this.ActionFailure.StepID.CompareTo(System.Guid.Empty) != 0)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@FailureAction_StepGuid", System.Data.SqlDbType.UniqueIdentifier));
                    cmd.Parameters["@FailureAction_StepGuid"].Value = this.ActionFailure.StepID;
                }
                cmd.Parameters["@RulePool_StepGuid"].Value = this.ID;
                cmd.Parameters["@RuleCalculation_Guid"].Value = this.RuleCalculation.ID;
                cmd.Parameters["@RulePool_StepId"].Value = this.StepID;
                cmd.Parameters["@RulePool_StepName"].Value = this.Name;
                cmd.Parameters["@SuccessAction_Id"].Value = System.Convert.ToInt32(this.ActionSuccess.enumRuleActionType);
                cmd.Parameters["@FailureAction_Id"].Value = System.Convert.ToInt32(this.ActionFailure.enumRuleActionType);
                cmd.Parameters["@RulePool_StepEnable"].Value = this.Enable;
                cmd.Parameters["@RulePool_BeginDate"].Value = this.BeginDate;
                cmd.Parameters["@RulePool_EndDate"].Value = this.EndDate;
                cmd.Parameters["@RulePool_StepParamsValue"].Value = this.xmldocAdvancedParamList.InnerXml;
                cmd.ExecuteNonQuery();
                iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                else
                {
                    bRet = this.SaveConditionGroupList(objProfile, cmd, ref strErr);
                }
                //bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                strErr = "Не удалось изменить свойства шага.\n\nТекст ошибки: " + f.Message;
            }
            return bRet;
        }
        /// <summary>
        /// изменяет свойства шага в базе данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean EditStepInDB(UniXP.Common.CProfile objProfile)
        {
            System.Boolean bRet = false;
            if (IsAllParametersValid() == false) { return bRet; }

            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            System.String strErr = "";
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Не удалось получить соединение с базой данных.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return bRet;
                }
                DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                bRet = this.EditStepInDB(objProfile, cmd, ref strErr);

                if (bRet == true)
                {
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    // откатываем транзакцию
                    DBTransaction.Rollback();
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка изменения свойств шага.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось изменить свойства шага.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }

        /// <summary>
        /// Редактирует описание акции
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="RulePool_StepGuid">УИ правила</param>
        /// <param name="ClearRulePoolDescription">признак "Удалить описание"</param>
        /// <param name="RulePool_Description">Описание</param>
        /// <param name="RulePool_IsCanShow">признак "Отображать торговым представителям"</param>
        /// <param name="strErr">сообщение об ошибке</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        public static System.Boolean MakeDescriptionToStepInRulePool(UniXP.Common.CProfile objProfile,
            System.Guid RulePool_StepGuid, System.Boolean ClearRulePoolDescription,
            System.String RulePool_Description, System.Boolean RulePool_IsCanShow, ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                System.Data.SqlClient.SqlConnection DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Не удалось получить соединение с базой данных.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return bRet;
                }
                System.Data.SqlClient.SqlTransaction DBTransaction = DBConnection.BeginTransaction();
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand()
                {
                    Connection = DBConnection,
                    Transaction = DBTransaction,
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_MakeDescriptionToStepInRulePool]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RulePool_StepGuid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ClearRulePoolDescription", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RulePool_Description", System.Data.DbType.String));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RulePool_IsCanShow", System.Data.SqlDbType.Bit));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;

                cmd.Parameters["@RulePool_StepGuid"].Value = RulePool_StepGuid;
                cmd.Parameters["@ClearRulePoolDescription"].Value = ClearRulePoolDescription;
                cmd.Parameters["@RulePool_Description"].Value = RulePool_Description;
                cmd.Parameters["@RulePool_IsCanShow"].Value = RulePool_IsCanShow;
                cmd.ExecuteNonQuery();

                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                bRet = (iRes == 0);
                if (bRet == true)
                {
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    // откатываем транзакцию
                    DBTransaction.Rollback();
                }

                cmd.Dispose();

            }
            catch (System.Exception f)
            {
                strErr = "Не удалось изменить описание акции.\n\nТекст ошибки: " + f.Message;
            }
            return bRet;
        }

        #endregion

        #region Удалить шаг в базе данных
        /// <summary>
        /// Удаляет шаг пула из базы данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmd">SQL-команда</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean DeleteStep(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmd, ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (cmd == null)
                {
                    strErr = "Не удалось получить соединение с базой данных.";
                    return bRet;
                }
                cmd.Parameters.Clear();

                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_DeleteStepFromRulePool]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RulePool_StepGuid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters["@RulePool_StepGuid"].Value = this.ID;
                cmd.ExecuteNonQuery();
                System.Int32 iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                if (iRes != 0)
                {
                    strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                }
                bRet = (iRes == 0);
            }
            catch (System.Exception f)
            {
                strErr = "Не удалось удалить шаг из базы данных.\n\nТекст ошибки: " + f.Message;
            }
            return bRet;
        }
        /// <summary>
        /// Удаляет сведения о шаге из базы данных
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean DeleteStepFromDB(UniXP.Common.CProfile objProfile)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            System.String strErr = "";
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Не удалось получить соединение с базой данных.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return bRet;
                }
                DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                bRet = this.DeleteStep(objProfile, cmd, ref strErr);

                if (bRet == true)
                {
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    // откатываем транзакцию
                    DBTransaction.Rollback();
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления шага из базы данных.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось удалить шаг из базы данных.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        #endregion

        public override string ToString()
        {
            return Name;
        }

    }
    /// <summary>
    /// Класс "Список правил для обработки заказа"
    /// </summary>
    public class CPoolRule
    {
        #region Свойства
        /// <summary>
        /// Список шагов
        /// </summary>
        private List<CPoolRuleCalculationStep> m_Steps;
        /// <summary>
        /// Список шагов
        /// </summary>
        public List<CPoolRuleCalculationStep> Steps
        {
            get { return m_Steps; }
        }
        #endregion

        #region Конструктор
        public CPoolRule()
        {
            m_Steps = null;
        }
        #endregion

        #region Загрузить список шагов
        /// <summary>
        /// Загружает из базы данных список шагов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean LoadSteps(UniXP.Common.CProfile objProfile)
        {
            System.Boolean bRet = false;
            try
            {
                this.m_Steps = CPoolRuleCalculationStep.GetRuleCalculationList(System.Guid.Empty, objProfile, null, System.Guid.Empty);
                bRet = (this.m_Steps != null);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось загрузить список шагов.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return bRet;
        }
        /// <summary>
        /// Загружает из базы данных список шагов
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="uuidRuleType">идетнификатор типа правила</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean LoadStepsForRuleType(UniXP.Common.CProfile objProfile, System.Guid uuidRuleType)
        {
            System.Boolean bRet = false;
            try
            {
                this.m_Steps = CPoolRuleCalculationStep.GetRuleCalculationList(System.Guid.Empty, objProfile, null, uuidRuleType);
                bRet = (this.m_Steps != null);
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось загрузить список шагов.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return bRet;
        }
        #endregion

        #region Изменить нумерацию шагов
        /// <summary>
        /// Сохраняет в БД список групп, связанных с правилом
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmd">SQL-команда</param>
        /// <param name="strErr">строка с текстом ошибки</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        private System.Boolean ChangeStepNumbersInDB(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmd, ref System.String strErr)
        {
            System.Boolean bRet = false;
            try
            {
                if (cmd == null)
                {
                    strErr = "Не удалось получить соединение с базой данных.";
                    return bRet;
                }
                if (this.m_Steps == null)
                {
                    strErr = "Не определен список шагов.";
                    return true;
                }

                cmd.Parameters.Clear();
                cmd.CommandText = System.String.Format("[{0}].[dbo].[sp_ChangeStepIDInRulePool]", objProfile.GetOptionsDllDBName());
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RulePool_StepGuid", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RulePool_StepId", System.Data.SqlDbType.Int));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_NUM", System.Data.SqlDbType.Int, 8, System.Data.ParameterDirection.Output, false, ((System.Byte)(0)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ERROR_MES", System.Data.SqlDbType.NVarChar, 4000));
                cmd.Parameters["@ERROR_MES"].Direction = System.Data.ParameterDirection.Output;
                System.Int32 iRes = 0;
                foreach (CPoolRuleCalculationStep objStep in this.m_Steps)
                {
                    cmd.Parameters["@RulePool_StepGuid"].Value = objStep.ID;
                    cmd.Parameters["@RulePool_StepId"].Value = objStep.StepID;
                    cmd.ExecuteNonQuery();
                    iRes = (System.Int32)cmd.Parameters["@RETURN_VALUE"].Value;
                    if (iRes != 0)
                    {
                        strErr = (System.String)cmd.Parameters["@ERROR_MES"].Value;
                        break;
                    }
                }
                bRet = (iRes == 0);

            }
            catch (System.Exception f)
            {
                strErr = "Не удалось изменить нумерацию шагов в списке.\n\nТекст ошибки: " + f.Message;
            }
            return bRet;
        }

        #endregion

        #region Удаление шагов
        /// <summary>
        /// Сохраняет в БД список групп, связанных с правилом
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <param name="cmd">SQL-команда</param>
        /// <param name="strErr">строка с текстом ошибки</param>
        /// <param name="objDeletedStepsList">список удаляемых шагов</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        private System.Boolean DeleteStepsFromDB(UniXP.Common.CProfile objProfile,
            System.Data.SqlClient.SqlCommand cmd, ref System.String strErr, List<CPoolRuleCalculationStep> objDeletedStepsList)
        {
            System.Boolean bRet = false;
            if ((objDeletedStepsList == null) || (objDeletedStepsList.Count == 0)) { return true; }
            try
            {
                if (cmd == null)
                {
                    strErr = "Не удалось получить соединение с базой данных.";
                    return bRet;
                }

                foreach (CPoolRuleCalculationStep objStep in objDeletedStepsList)
                {
                    bRet = objStep.DeleteStep(objProfile, cmd, ref strErr);
                    if (bRet == false) { break; }

                }
            }
            catch (System.Exception f)
            {
                strErr = "Не удалось удалить шаги в списке.\n\nТекст ошибки: " + f.Message;
            }
            return bRet;
        }

        #endregion

        #region Сохранить изменения в списке
        /// <summary>
        /// Сохраняет в БД изменения в спске
        /// </summary>
        /// <param name="objProfile">профайл</param>
        /// <returns>true - удачное завершение; false - ошибка</returns>
        public System.Boolean SaveChangesInStepList(UniXP.Common.CProfile objProfile,
            List<CPoolRuleCalculationStep> objDeletedStepsList)
        {
            System.Boolean bRet = false;
            System.Data.SqlClient.SqlConnection DBConnection = null;
            System.Data.SqlClient.SqlCommand cmd = null;
            System.Data.SqlClient.SqlTransaction DBTransaction = null;
            System.String strErr = "";
            try
            {
                DBConnection = objProfile.GetDBSource();
                if (DBConnection == null)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show(
                        "Не удалось получить соединение с базой данных.", "Внимание",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    return bRet;
                }
                DBTransaction = DBConnection.BeginTransaction();
                cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Connection = DBConnection;
                cmd.Transaction = DBTransaction;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                // сперва удаляем шаги
                bRet = this.DeleteStepsFromDB(objProfile, cmd, ref strErr, objDeletedStepsList);
                if (bRet == true)
                {
                    // теперь сохраняем изменения
                    foreach (CPoolRuleCalculationStep objStep in this.Steps)
                    {
                        bRet = objStep.EditStepInDB(objProfile, cmd, ref strErr);
                        if (bRet == false) { break; }
                    }
                    // теперь меняем нумерацию
                    if (bRet == true)
                    {
                        bRet = this.ChangeStepNumbersInDB(objProfile, cmd, ref strErr);
                    }
                }

                if (bRet == true)
                {
                    // подтверждаем транзакцию
                    DBTransaction.Commit();
                }
                else
                {
                    // откатываем транзакцию
                    DBTransaction.Rollback();
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка сохранения изменений в списке.\n\nТекст ошибки: " + strErr, "Ошибка",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                cmd.Dispose();
            }
            catch (System.Exception f)
            {
                DBTransaction.Rollback();
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Не удалось сохраненить изменения в списке.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.Close();
            }
            return bRet;
        }
        #endregion
    }

}
