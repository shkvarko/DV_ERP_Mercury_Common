using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraTreeList;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;

namespace ERP_Mercury.Common
{
    /// <summary>
    /// Перечень справочников
    /// </summary>
    public enum EnumDirectSimple
    {
        StateType = 0,
        CustomerType = 1,
        AddressType = 2,
        AddressPrefix = 3,
        Flat = 4,
        Building = 5,
        SubBuilding = 6,
        Country = 7,
        Region = 8,
        LocalityPrefix = 9,
        City = 10,
        Departament = 11,
        JobPosition = 12,
        PhoneType = 13,
        Phone = 14,
        CustomerActiveType = 15,
        LicenceType = 16,
        RttType = 17,
        RttActiveType = 18,
        RttSpecCode = 19,
        TargetBuy = 20,
        EquipmentType = 21,
        SizeEq = 22,
        Availability = 23,
        ProductOwner = 24,
        Segmentation = 25,
        DistribNet = 26,
        AccountType = 27,
        Oblast = 28,
        CustomerCategory = 29,
        ConditionObjectType = 30,
        ConditionGroupType = 31,
        RuleType = 32, 
        StoredProcedure = 33,
        ProductVTM = 34,
        ProductType = 35,
        ProductLine = 36,
        ProductSubType = 37,
        ProductTradeMark = 38,
        ProductCategory = 39,
        VendorContractType = 40,
        VendorPaymentDocType = 41,
        CarrierRateType = 42,
        Driver = 43,
        Vehicle = 44,
        AdvancedExpenseType = 45,
        RouteSheetType = 46,
        PriceType = 47,
        RegionDelivery = 48,
        AgreementType = 49,
        AgreementState = 50,
        AgrementBasement = 51,
        AgrementReason = 52,
        AgreementDeliveryCondition = 53,
        AgreementPaymentCondition = 54,
        Stock = 55,
        WareHouse=56,
        WareHouseType=57,
        ChildCustCode = 58,
        VendorType = 59,
        Currency =60,
        Measure=61,
        CertificateType = 62,
        LotOrderState = 63,
        KLPState = 64,
        ConstType = 65,
        Const = 66,
        Surcharges = 67,
        VendorContractDelayType = 68,
        NDSRate = 69,
        SegmentationChanel = 70,
        SegmentationMarket = 71,
        SegmentationSubChanel = 72,
        AccountPlan = 73,
        BudgetProject = 74,
        LotState = 75, 
        EarningType = 76, 
        WaybillState = 77,
        WaybillShipMode = 78,
        BackWaybillState = 79,
        WaybillBackReason = 80,
        IntWaybillState = 81,
        IntWaybillShipMode = 82,
        IntOrderState = 83,
        IntOrderShipMode = 84
    }

    public partial class ctrlDatabaseDirectory : UserControl
    {
        #region Переменные, Свойства, Константы
        private UniXP.Common.CProfile m_objProfile;
        private UniXP.Common.MENUITEM m_objMenuItem;
        private EnumDirectSimple m_DirectSimple;
        private System.Int32 m_iSn;
        
        #endregion

        #region Конструктор
        public ctrlDatabaseDirectory(UniXP.Common.CProfile objProfile, EnumDirectSimple DirectSimple, UniXP.Common.MENUITEM objMenuItem)
        {
            InitializeComponent();

            m_objProfile = objProfile;
            m_DirectSimple = DirectSimple;
            m_objMenuItem = objMenuItem;
            m_iSn = -1;
        }
        #endregion

        #region Список объектов
        /// <summary>
        /// Заполнение списка объектов
        /// </summary>
        /// <returns>true - успешное завершение; false - ошибка</returns>
        private System.Boolean bLoadTreeList()
        {
            System.Boolean bRet = false;
            System.String strErr = "";

            try
            {
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).BeginInit();
                this.SuspendLayout();
                this.treeList.FocusedNodeChanged -= new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeList_FocusedNodeChanged);

                treeList.Enabled = false;
                treeList.Nodes.Clear();

                switch (this.m_DirectSimple)
                {
                    case EnumDirectSimple.StateType:
                        {
                            // формы собственности
                            List<ERP_Mercury.Common.CStateType> objList = ERP_Mercury.Common.CStateType.GetStateTypeList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CStateType objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.CustomerType:
                        {
                            // типы клиентов
                            List<ERP_Mercury.Common.CCustomerType> objList = ERP_Mercury.Common.CCustomerType.GetCustomerTypeList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CCustomerType objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.AddressType:
                        {
                            // типы адресов
                            List<ERP_Mercury.Common.CAddressType> objList = ERP_Mercury.Common.CAddressType.GetAddressTypeList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CAddressType objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.AddressPrefix:
                        {
                            // префиксы адресов
                            List<ERP_Mercury.Common.CAddressPrefix> objList = ERP_Mercury.Common.CAddressPrefix.GetAddressPrefixList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CAddressPrefix objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.Flat:
                        {
                            // префиксы адресов
                            List<ERP_Mercury.Common.CFlat> objList = ERP_Mercury.Common.CFlat.GetFlatList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CFlat objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.Building:
                        {
                            List<ERP_Mercury.Common.CBuilding> objList = ERP_Mercury.Common.CBuilding.GetBuildingList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CBuilding objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.SubBuilding:
                        {
                            List<ERP_Mercury.Common.CSubBuilding> objList = ERP_Mercury.Common.CSubBuilding.GetSubBuildingList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CSubBuilding objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.Country:
                        {
                            // страны
                            List<ERP_Mercury.Common.CCountry> objList = ERP_Mercury.Common.CCountry.GetCountryList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CCountry objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.Region:
                        {
                            // регионы
                            List<ERP_Mercury.Common.CRegion> objList = ERP_Mercury.Common.CRegion.GetRegionList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CRegion objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.LocalityPrefix:
                        {
                            // типы населенных пунктов
                            List<ERP_Mercury.Common.CLocalityPrefix> objList = ERP_Mercury.Common.CLocalityPrefix.GetLocalityPrefixList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CLocalityPrefix objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.City:
                        {
                            // города
                            List<ERP_Mercury.Common.CCity> objList = ERP_Mercury.Common.CCity.GetCityList(m_objProfile, null, false);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CCity objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.Departament:
                        {
                            // отделы
                            List<ERP_Mercury.Common.CDepartament> objList = ERP_Mercury.Common.CDepartament.GetDepartamentList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CDepartament objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.JobPosition:
                        {
                            // должности
                            List<ERP_Mercury.Common.CJobPosition> objList = ERP_Mercury.Common.CJobPosition.GetJobPositionList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CJobPosition objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.PhoneType:
                        {
                            // типы телефонных номеров
                            List<ERP_Mercury.Common.CPhoneType> objList = ERP_Mercury.Common.CPhoneType.GetPhoneTypeList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CPhoneType objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.CustomerActiveType:
                        {
                            // признак активности клиента
                            List<ERP_Mercury.Common.CCustomerActiveType> objList = ERP_Mercury.Common.CCustomerActiveType.GetCustomerActiveTypeList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CCustomerActiveType objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.LicenceType:
                        {
                            // тип лицензии
                            List<ERP_Mercury.Common.CLicenceType> objList = ERP_Mercury.Common.CLicenceType.GetLicenceTypeList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CLicenceType objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.RttType:
                        {
                            // тип РТТ
                            List<ERP_Mercury.Common.CRttType> objList = ERP_Mercury.Common.CRttType.GetRttTypeList( m_objProfile, null );

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CRttType objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.RttActiveType:
                        {
                            // признак активности РТТ
                            List<ERP_Mercury.Common.CRttActiveType> objList = ERP_Mercury.Common.CRttActiveType.GetRttActiveTypeList( m_objProfile, null );

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CRttActiveType objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.RttSpecCode:
                        {
                            // спецкод РТТ
                            List<ERP_Mercury.Common.CRttSpecCode> objList = ERP_Mercury.Common.CRttSpecCode.GetRttSpecCodeList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CRttSpecCode objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.TargetBuy:
                        {
                            // спецкод РТТ
                            List<ERP_Mercury.Common.CTargetBuy> objList = ERP_Mercury.Common.CTargetBuy.GetTargetBuyList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CTargetBuy objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.EquipmentType:
                        {
                            // тип оборудования
                            List<ERP_Mercury.Common.CEquipmentType> objList = ERP_Mercury.Common.CEquipmentType.GetEquipmentTypeList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CEquipmentType objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Code }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.SizeEq:
                        {
                            // размер
                            List<ERP_Mercury.Common.CSizeEq> objList = ERP_Mercury.Common.CSizeEq.GetSizeEqList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CSizeEq objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.Availability:
                        {
                            // возможность установки
                            List<ERP_Mercury.Common.CAvailability> objList = ERP_Mercury.Common.CAvailability.GetAvailabilityList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CAvailability objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.ProductOwner:
                        {
                            // каталог продукции
                            List<ERP_Mercury.Common.CProductCatalog> objList = ERP_Mercury.Common.CProductCatalog.GetProductCatalogListAll(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CProductCatalog objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.Segmentation:
                        {
                            // сегментация
                            List<ERP_Mercury.Common.CSalesSegmentation> objList = ERP_Mercury.Common.CSalesSegmentation.GetSegmentationList( m_objProfile, null );

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CSalesSegmentation objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Code }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.DistribNet:
                        {
                            // торговая сеть
                            List<ERP_Mercury.Common.CDistributionNetwork> objList = ERP_Mercury.Common.CDistributionNetwork.GetDistributionNetworkList( m_objProfile, null );

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CDistributionNetwork objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.AccountType:
                        {
                            // тип расчетного счета
                            List<ERP_Mercury.Common.CAccountType> objList = ERP_Mercury.Common.CAccountType.GetAccountTypeList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CAccountType objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.Oblast:
                        {
                            // область
                            List<ERP_Mercury.Common.COblast> objList = ERP_Mercury.Common.COblast.GetOblastList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.COblast objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.CustomerCategory:
                        {
                            // категория клиента
                            List<ERP_Mercury.Common.CCustomerCategory> objList = ERP_Mercury.Common.CCustomerCategory.GetCustomerCategoryList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CCustomerCategory objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.ConditionObjectType:
                        {
                            // тип объекта, входящего в группу
                            List<ERP_Mercury.Common.CConditionObjectType> objList = ERP_Mercury.Common.CConditionObjectType.GetConditionObjectTypeList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CConditionObjectType objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.ConditionGroupType:
                        {
                            // тип группы
                            List<ERP_Mercury.Common.CConditionGroupType> objList = ERP_Mercury.Common.CConditionGroupType.GetConditionGroupTypeList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CConditionGroupType objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.RuleType:
                        {
                            // тип правила
                            List<ERP_Mercury.Common.CRuleType> objList = ERP_Mercury.Common.CRuleType.GetRuleTypeList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CRuleType objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.StoredProcedure:
                        {
                            // хранимая процедура для правила
                            List<ERP_Mercury.Common.CStoredProcedure> objList = ERP_Mercury.Common.CStoredProcedure.GetStoredProcedureList( m_objProfile, null );

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CStoredProcedure objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.ProductVTM:
                        {
                            // владелец товарной марки
                            List<ERP_Mercury.Common.CProductVtm> objList = ERP_Mercury.Common.CProductVtm.GetProductVtmList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CProductVtm objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.ProductType:
                        {
                            // товарная группа
                            List<ERP_Mercury.Common.CProductType> objList = ERP_Mercury.Common.CProductType.GetProductTypeList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CProductType objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.ProductLine:
                        {
                            // товарная линия
                            List<ERP_Mercury.Common.CProductLine> objList = ERP_Mercury.Common.CProductLine.GetProductLineList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CProductLine objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.ProductSubType:
                        {
                            // товарная подгруппа
                            List<ERP_Mercury.Common.CProductSubType> objList = ERP_Mercury.Common.CProductSubType.GetProductSubTypeList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CProductSubType objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.ProductTradeMark:
                        {
                            // товарная марка
                            List<ERP_Mercury.Common.CProductTradeMark> objList = ERP_Mercury.Common.CProductTradeMark.GetProductTradeMarkList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CProductTradeMark objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.ProductCategory:
                        {
                            // категория товара
                            List<ERP_Mercury.Common.CProductCategory> objList = ERP_Mercury.Common.CProductCategory.GetProductCategoryList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CProductCategory objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.VendorContractType:
                        {
                            // тип договора с поставщиком
                            List<ERP_Mercury.Common.CVendorContractType> objList = ERP_Mercury.Common.CVendorContractType.GetVendorContractTypeList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CVendorContractType objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.VendorPaymentDocType:
                        {
                            // тип платежного документа
                            List<ERP_Mercury.Common.CVendorPaymentDocType> objList = ERP_Mercury.Common.CVendorPaymentDocType.GetVendorPaymentDocTypeList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CVendorPaymentDocType objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.CarrierRateType:
                        {
                            // тип тарифа перевозчика
                            List<ERP_Mercury.Common.CarrierRateType> objList = ERP_Mercury.Common.CarrierRateType.GetCarrierRateTypeList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CarrierRateType objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.Driver:
                        {
                            // водитель
                            List<ERP_Mercury.Common.CDriver> objList = ERP_Mercury.Common.CDriver.GetDriverList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CDriver objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.GetFIO() }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.Vehicle:
                        {
                            // автомобиль
                            List<ERP_Mercury.Common.CVehicle> objList = ERP_Mercury.Common.CVehicle.GetVehicleList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CVehicle objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { ( objItem.Name + " " + objItem.GosNumber ) }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.AdvancedExpenseType:
                        {
                            // тип дополнительных расходов
                            List<ERP_Mercury.Common.CRouteSheetAdvancedExpenseType> objList = ERP_Mercury.Common.CRouteSheetAdvancedExpenseType.GeAdvancedExpenseTypeList( m_objProfile, null );

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CRouteSheetAdvancedExpenseType objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.RouteSheetType:
                        {
                            // тип путевых листов
                            List<ERP_Mercury.Common.CRouteSheetType> objList = ERP_Mercury.Common.CRouteSheetType.GetRouteSheetTypeList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CRouteSheetType objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.PriceType:
                        {
                            // тип цены
                            List<ERP_Mercury.Common.CPriceType> objList = ERP_Mercury.Common.CPriceType.GetPriceTypeList( m_objProfile, null );

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CPriceType objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.RegionDelivery:
                        {
                            // регион доставки
                            List<ERP_Mercury.Common.CRegionDelivery> objList = ERP_Mercury.Common.CRegionDelivery.GetRegionDeliveryList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CRegionDelivery objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.AgreementState:
                        {
                            // состояние договора
                            List<ERP_Mercury.Common.CAgreementState> objList = ERP_Mercury.Common.CAgreementState.GetAgreementStateList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CAgreementState objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.AgreementType:
                        {
                            // тип договора
                            List<ERP_Mercury.Common.CAgreementType> objList = ERP_Mercury.Common.CAgreementType.GetAgreementTypeList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CAgreementType objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.AgrementBasement:
                        {
                            // основание действия для договора
                            List<ERP_Mercury.Common.CAgrementBasement> objList = ERP_Mercury.Common.CAgrementBasement.GetAgrementBasementList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CAgrementBasement objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.AgrementReason:
                        {
                            // цель приобретения товара
                            List<ERP_Mercury.Common.CAgrementReason> objList = ERP_Mercury.Common.CAgrementReason.GetAgrementReasonList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CAgrementReason objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.AgreementDeliveryCondition:
                        {
                            // условия доставки товара
                            List<ERP_Mercury.Common.CAgreementDeliveryCondition> objList = ERP_Mercury.Common.CAgreementDeliveryCondition.GetAgreementDeliveryConditionList( m_objProfile, null );

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CAgreementDeliveryCondition objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.AgreementPaymentCondition:
                        {
                            // условия оплаты товара
                            List<ERP_Mercury.Common.CAgreementPaymentCondition> objList = ERP_Mercury.Common.CAgreementPaymentCondition.GetAgreementPaymentConditionList( m_objProfile, null );

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CAgreementPaymentCondition objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.Stock:
                        {
                            treeList.BeginUpdate();
                                treeList.Columns.Add();
                                treeList.Columns[0].Caption = "Компания";
                                treeList.Columns[0].VisibleIndex = 0;                  
                                treeList.Columns[0].AppearanceHeader.Options.UseTextOptions = true;
                                treeList.Columns[0].AppearanceHeader.TextOptions.HAlignment =  DevExpress.Utils.HorzAlignment.Center; // выравнивание заголовка
                                //treeList.Columns[0].Name = "CompanyName";
                            
                                treeList.Columns.Add();
                                treeList.Columns[1].Caption = "Наименование склада";
                                treeList.Columns[1].VisibleIndex = 1;
                                treeList.Columns[1].AppearanceHeader.Options.UseTextOptions = true;
                                treeList.Columns[1].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                                //treeList.Columns[1].Name = "StockName";
                            treeList.EndUpdate();

                            // склады
                            List<ERP_Mercury.Common.CStock> objList = ERP_Mercury.Common.CStock.GetStockListForWarehouse(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CStock objItem in objList)
                                {
                                    //DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null); // предыдущий вариант, только склад
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.CompanyName, objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }

                            break;
                        }
                    case EnumDirectSimple.WareHouse:
                        {
                            // места хранения
                            List<ERP_Mercury.Common.CWarehouse> objList = ERP_Mercury.Common.CWarehouse.GetWareHouseList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CWarehouse objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.WareHouseType :
                        {
                            // тип хранилища
                            List<ERP_Mercury.Common.CWareHouseType> objList = ERP_Mercury.Common.CWareHouseType.GetWareHouseTypeList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CWareHouseType objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.ChildCustCode:
                        {
                            // дочернее подразделение
                            List<ERP_Mercury.Common.CChildDepart> objList = ERP_Mercury.Common.CChildDepart.GetChildDepartList(m_objProfile, null, System.Guid.Empty);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CChildDepart objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Code }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.VendorType:
                        {
                            // тип поставщика
                            List<ERP_Mercury.Common.CVendorType> objList = ERP_Mercury.Common.CVendorType.GetVendorTypeList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CVendorType objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.Currency :
                        {
                            // валюта
                            treeList.BeginUpdate();
                            treeList.Columns.Add();
                            treeList.Columns[0].Caption = "Наименование валюты";
                            treeList.Columns[0].VisibleIndex = 0;
                            treeList.Columns[0].AppearanceHeader.Options.UseTextOptions = true;
                            treeList.Columns[0].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center; // выравнивание заголовка
                            //treeList.Columns[0].Name = "CompanyName";

                            treeList.Columns.Add();
                            treeList.Columns[1].Caption = "Аббревиатура валюты";
                            treeList.Columns[1].VisibleIndex = 1;
                            treeList.Columns[1].AppearanceHeader.Options.UseTextOptions = true;
                            treeList.Columns[1].AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            //treeList.Columns[1].Name = "StockName";
                            treeList.EndUpdate();

                            
                            List<ERP_Mercury.Common.CCurrency> objList = ERP_Mercury.Common.CCurrency.GetCurrencyListForCurrency(m_objProfile, null);
                            
                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CCurrency objItem in objList)
                                {
                                    //DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null); // предыдущий вариант, только склад
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name, objItem.CurrencyAbbr}, null);
                                    objNode.Tag = objItem;
                                }
                            }

                            break;


                            // валюта

                            /*
                            List<ERP_Mercury.Common.CCurrency> objList = ERP_Mercury.Common.CCurrency.GetCurrencyListForCurrency(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CCurrency objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.CurrencyAbbr }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                            */ 
                        }
                    case EnumDirectSimple.Measure :
                        {
                            // единицы измерения
                            List<ERP_Mercury.Common.CMeasure> objList = ERP_Mercury.Common.CMeasure.GetMeasureList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CMeasure objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.CertificateType:
                        {
                            // типы документов о качестве товара
                            List<ERP_Mercury.Common.CCertificateType> objList = ERP_Mercury.Common.CCertificateType.GetCertificateTypeList( m_objProfile, null );

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CCertificateType objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.LotOrderState:
                        {
                            // состояния договоров
                            List<ERP_Mercury.Common.CLotOrderState> objList = ERP_Mercury.Common.CLotOrderState.GetLotOrderStateList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CLotOrderState objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.KLPState:
                        {
                            // состояния КЛП
                            List<ERP_Mercury.Common.CKLPState> objList = ERP_Mercury.Common.CKLPState.GetKLPStateList( m_objProfile, null );

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CKLPState objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.ConstType:
                        {
                            // типы констант
                            List<ERP_Mercury.Common.CConstType> objList = ERP_Mercury.Common.CConstType.GetConstTypeList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CConstType objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.Const:
                        {
                            // константы
                            List<ERP_Mercury.Common.CConst> objList = ERP_Mercury.Common.CConst.GetConstList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CConst objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.Surcharges:
                        {
                            // типы дополнительных расходов
                            List<ERP_Mercury.Common.CSurcharges> objList = ERP_Mercury.Common.CSurcharges.GetSurchargesList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CSurcharges objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.VendorContractDelayType:
                        {
                            // виды отсрочки платежа
                            List<ERP_Mercury.Common.CVendorContractDelayType> objList = ERP_Mercury.Common.CVendorContractDelayType.GetVendorContractDelayTypeList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CVendorContractDelayType objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.NDSRate:
                        {
                            // ставки НДС
                            List<ERP_Mercury.Common.CNDSRate> objList = ERP_Mercury.Common.CNDSRate.GetNDSRateList(m_objProfile, null);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CNDSRate objItem in objList)
                                {
                                    DevExpress.XtraTreeList.Nodes.TreeListNode objNode = treeList.AppendNode(new object[] { objItem.Name }, null);
                                    objNode.Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.SegmentationChanel:
                        {
                            // канал сбыта
                            List<ERP_Mercury.Common.CSegmentationChanel> objList =
                                ERP_Mercury.Common.CSegmentationChanelDataBaseModel.GetSegmentationChanelList(m_objProfile, null, ref strErr);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CSegmentationChanel objItem in objList)
                                {
                                    treeList.AppendNode(new object[] { objItem.Name }, null).Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.SegmentationMarket:
                        {
                            // канал сбыта
                            List<ERP_Mercury.Common.CSegmentationMarket> objList =
                                ERP_Mercury.Common.CSegmentationMarketDataBaseModel.GetSegmentationMarketList(m_objProfile, null, ref strErr);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CSegmentationMarket objItem in objList)
                                {
                                    treeList.AppendNode(new object[] { objItem.Name }, null).Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.SegmentationSubChanel:
                        {
                            // каталог продукции
                            List<ERP_Mercury.Common.CSegmentationSubChannel> objList = ERP_Mercury.Common.CSegmentationSubChanelDataBaseModel.GetSegmentationSubChannelList(m_objProfile, null, ref strErr);

                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CSegmentationSubChannel objItem in objList)
                                {
                                    treeList.AppendNode(new object[] { (String.Format("{0} {1}", objItem.Code, objItem.Name)) }, null).Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.AccountPlan:
                        {
                            // план счетов
                            List<CAccountPlan> objList = CAccountPlanDataBaseModel.GetAccountPlanList(m_objProfile, null, ref strErr);
                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CAccountPlan objItem in objList)
                                {
                                    treeList.AppendNode(new object[] { objItem.FullName }, null).Tag = objItem;
                                }
                            }
                            break;

                        }
                    case EnumDirectSimple.BudgetProject:
                        {
                            // проект
                            List<CBudgetProject> objList = CBudgetProjectDataBaseModel.GetBudgetProjectList(m_objProfile, null, ref strErr);
                            if (objList != null)
                            {
                                foreach (ERP_Mercury.Common.CBudgetProject objItem in objList)
                                {
                                    treeList.AppendNode(new object[] { objItem.Name }, null).Tag = objItem;
                                }
                            }
                            break;

                        }
                    case EnumDirectSimple.LotState:
                        {
                            // состояние заказа
                            List<CLotState> objList = CLotState.GetLotStateList(m_objProfile, null, System.Guid.Empty);
                            if (objList != null)
                            {
                                foreach (CLotState objItem in objList)
                                {
                                    treeList.AppendNode(new object[] { objItem.Name }, null).Tag = objItem;
                                }
                            }
                            break;

                        }
                    case EnumDirectSimple.EarningType:
                        {
                            // вид оплаты
                            List<CEarningType> objList = CEarningType.GetEarningTypeList(m_objProfile, ref strErr);
                            if (objList != null)
                            {
                                foreach (CEarningType objItem in objList)
                                {
                                    treeList.AppendNode(new object[] { objItem.Name }, null).Tag = objItem;
                                }
                            }
                            break;

                        }
                    case EnumDirectSimple.WaybillState:
                        {
                            // состояние накладной
                            List<CWaybillState> objList = CWaybillState.GetWaybillStateList(m_objProfile, ref strErr);
                            if (objList != null)
                            {
                                foreach (CWaybillState objItem in objList)
                                {
                                    treeList.AppendNode(new object[] { objItem.Name }, null).Tag = objItem;
                                }
                            }
                            break;

                        }
                    case EnumDirectSimple.WaybillShipMode:
                        {
                            // вид отгрузки накладной
                            List<CWaybillShipMode> objList = CWaybillShipMode.GetWaybillShipModeList(m_objProfile, ref strErr);
                            if (objList != null)
                            {
                                foreach (CWaybillShipMode objItem in objList)
                                {
                                    treeList.AppendNode(new object[] { objItem.Name }, null).Tag = objItem;
                                }
                            }
                            break;

                        }
                    case EnumDirectSimple.BackWaybillState:
                        {
                            // состояние накладной на возврат товара
                            List<CBackWaybillState> objList = CBackWaybillState.GetBackWaybillStateList( m_objProfile, ref strErr );
                            if (objList != null)
                            {
                                foreach (CBackWaybillState objItem in objList)
                                {
                                    treeList.AppendNode(new object[] { objItem.Name }, null).Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.WaybillBackReason:
                        {
                            // причина возврата товара
                            List<CWaybillBackReason> objList = CWaybillBackReason.GetWaybillBackReasonList( m_objProfile, ref strErr );
                            if (objList != null)
                            {
                                foreach (CWaybillBackReason objItem in objList)
                                {
                                    treeList.AppendNode(new object[] { objItem.Name }, null).Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.IntWaybillShipMode:
                        {
                            // вид отгрузки накладной на внутреннее перемещение
                            List<CIntWaybillShipMode> objList = CIntWaybillShipMode.GetIntWaybillShipModeList(m_objProfile, ref strErr);
                            if (objList != null)
                            {
                                foreach (CIntWaybillShipMode objItem in objList)
                                {
                                    treeList.AppendNode(new object[] { objItem.Name }, null).Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.IntWaybillState:
                        {
                            // состояние накладной на внутреннее перемещение
                            List<CIntWaybillState> objList = CIntWaybillState.GetIntWaybillStateList(m_objProfile, ref strErr);
                            if (objList != null)
                            {
                                foreach (CIntWaybillState objItem in objList)
                                {
                                    treeList.AppendNode(new object[] { objItem.Name }, null).Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.IntOrderShipMode:
                        {
                            // вид отгрузки заказа на внутреннее перемещение
                            List<CIntOrderShipMode> objList = CIntOrderShipMode.GetIntOrderShipModeList(m_objProfile, ref strErr);
                            if (objList != null)
                            {
                                foreach (CIntOrderShipMode objItem in objList)
                                {
                                    treeList.AppendNode(new object[] { objItem.Name }, null).Tag = objItem;
                                }
                            }
                            break;
                        }
                    case EnumDirectSimple.IntOrderState:
                        {
                            // состояние заказа на внутреннее перемещение
                            List<CIntOrderState> objList = CIntOrderState.GetIntOrderStateList(m_objProfile, ref strErr);
                            if (objList != null)
                            {
                                foreach (CIntOrderState objItem in objList)
                                {
                                    treeList.AppendNode(new object[] { objItem.Name }, null).Tag = objItem;
                                }
                            }
                            break;
                        }

                    default:
                        break;
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
                SetModified(false);
                treeList.Enabled = true;
                treeList.FocusedNode = null;
                ((System.ComponentModel.ISupportInitialize)(this.treeList)).EndInit();
                this.ResumeLayout(false);
                this.treeList.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeList_FocusedNodeChanged);
                SendMessageToLog("Закгружен список объектов");
                treeList.FocusedNode = (treeList.Nodes.Count > 0) ? treeList.Nodes[0] : null;
            }

            return bRet;
        }
        /// <summary>
        /// Обновляет список 
        /// </summary>
        /// <returns>true - успешное завершение; false - ошибка</returns>
        public System.Boolean bRefreshTreeList()
        {
            System.Boolean bRet = false;
            try
            {
                // обновляем информацию
                bRet = bLoadTreeList();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка обновления информации.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return bRet;
        }

        private void barBtnRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                // обновляем информацию
                bRefreshTreeList();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка обновления списка.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK,
                   System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }

        #endregion

        #region Загрузка формы
        private void ctrlDatabaseDirectory_Load(object sender, EventArgs e)
        {
            try
            {
                if (bLoadTreeList() == false)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка открытия формы.", "Ошибка",
                      System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка открытия формы.\n\nТекст ошибки:\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }
        #endregion

        #region Навигация по списку объектов
        private void treeList_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            try
            {
                this.m_bCancelEvents = true;
                propertyGrid.SelectedObject = null;
                
                if ((treeList.Nodes.Count > 0) && (treeList.FocusedNode != null) && (treeList.FocusedNode.Tag != null))
                {
                    propertyGrid.SelectedObject = treeList.FocusedNode.Tag;
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка смены узла.\n\nТекст ошибки:\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                this.m_bCancelEvents = false;
                SetModified(false);
            }

            return;

        }
        #endregion

        #region Индикация изменений

        private System.Boolean m_bIsModified;
        public System.Boolean Modified
        {
            get { return m_bIsModified; }
        }
        private System.Boolean m_bCancelEvents;
        /// <summary>
        /// Устанавливает индикатор "изменена запись"
        /// </summary>
        private void SetModified(System.Boolean bModified)
        {
            try
            {
                m_bIsModified = bModified;
                btnSave.Enabled = bModified;
                btnCancel.Enabled = bModified;
                treeList.Enabled = !bModified; 
                barBtnRefresh.Enabled = !bModified;
                barBtnAdd.Enabled = !bModified;
                barBtnDelete.Enabled = !bModified;
                barBtnPrint.Enabled = !bModified;
                if (bModified) { EnableControls(true); }           
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка метода SetModified().\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }
        /// <summary>
        /// Возвращает признак того, изменялась ли запись
        /// </summary>
        /// <returns>true - изменялась; false - не изменялась</returns>
        private System.Boolean IsModified()
        {
            System.Boolean bRes = false;
            try
            {
                return m_bIsModified;
            }
            catch (System.Exception e)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(e.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            return bRes;
        }
        /// <summary>
        /// Включает отключает элементы управления
        /// </summary>
        /// <param name="bEnable">включить/выключить</param>
        private void EnableControls(System.Boolean bEnable)
        {
            try
            {
                propertyGrid.Enabled = bEnable;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка отключения/включения элементов управления.\n\nТекст ошибки:\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            try
            {
                if ((this.m_bCancelEvents == false) && (treeList.Nodes.Count > 0) &&
                   (treeList.FocusedNode != null) && (treeList.FocusedNode.Tag != null))
                    {
                       if (treeList.Columns.TreeList.VisibleColumns.Count==2 )
                        {
                            if (this.m_DirectSimple == EnumDirectSimple.Currency)
                            {
                                treeList.FocusedNode.SetValue(treeList.Columns[1], (ERP_Mercury.Common.CBusinessObject)treeList.FocusedNode.Tag);
                                treeList.FocusedNode.SetValue(treeList.Columns[0], (((ERP_Mercury.Common.CBusinessObject)treeList.FocusedNode.Tag).Name)); // добавил для справочника валют 06.01.12
                                SetModified(true);
                            }
                            else
                            {
                                treeList.FocusedNode.SetValue(treeList.Columns[1], (ERP_Mercury.Common.CBusinessObject)treeList.FocusedNode.Tag);
                                SetModified(true);
                            }                           
                        }
                        else
                        {
                            SetModified(true);
                            switch (this.m_DirectSimple)
                            {
                                case EnumDirectSimple.ChildCustCode:
                                    treeList.FocusedNode.SetValue(colName, ((ERP_Mercury.Common.CChildDepart)treeList.FocusedNode.Tag).Code);
                                    break;
                                case EnumDirectSimple.VendorContractType:
                                    if (e.ChangedItem.PropertyDescriptor.Name == "IsCanPaymentDelay")
                                    {
                                        if (System.Convert.ToBoolean(e.ChangedItem.Value) == false)
                                        {
                                            ((ERP_Mercury.Common.CVendorContractType)treeList.FocusedNode.Tag).CreditPeriodDay = 0;
                                        }
                                    }
                                    if (e.ChangedItem.PropertyDescriptor.Name == "CreditPeriodDay")
                                    {
                                        if (((ERP_Mercury.Common.CVendorContractType)treeList.FocusedNode.Tag).IsCanPaymentDelay == false)
                                        {
                                            ((ERP_Mercury.Common.CVendorContractType)treeList.FocusedNode.Tag).CreditPeriodDay = 0;
                                        }
                                    }
                                    
                                    
                                    break;
                                default:
                                    treeList.FocusedNode.SetValue(colName, ((ERP_Mercury.Common.CBusinessObject)treeList.FocusedNode.Tag).Name);
                                    break;
                            }
                        }
                    }   
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                "Ошибка функции propertyGrid_PropertyValueChanged.\n\nТекст ошибки: " + f.Message, "Внимание",
                System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;

        }
        #endregion

        #region Сохранение изменений
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bSaveChange();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка сохранения изменений.\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }
        

        /// <summary>
        /// Сохраняет изменения в БД
        /// </summary>
        /// <returns>true - успешное завершение; false - ошибка</returns>
        private System.Boolean bSaveChange()
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            System.Boolean bRet = false;
            try
            {

                // все свойства определены - можно сохранять
                if (((ERP_Mercury.Common.CBusinessObject)propertyGrid.SelectedObject).ID.CompareTo(System.Guid.Empty) == 0)
                {
                    // новый 
                    bRet = ((ERP_Mercury.Common.CBusinessObject)propertyGrid.SelectedObject).Add(m_objProfile);
                    if (bRet) { bRet = bRefreshTreeList(); }
                }
                else
                {
                    // ранее сохраненный
                    bRet = ((ERP_Mercury.Common.CBusinessObject)propertyGrid.SelectedObject).Update(m_objProfile);
                    if (bRet)
                    {
                        SetModified(false);
                        SendMessageToLog("Изменения успешно сохранены.");
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Не удалось сохранить изменения в БД.\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }

            return bRet;
        }

        #endregion

        #region Отмена изменений
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (DevExpress.XtraEditors.XtraMessageBox.Show(
                   "Отменить все сделанные изменения?", "Подтверждение",
                   System.Windows.Forms.MessageBoxButtons.YesNoCancel,
                   System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    CancelChange();
                    SendMessageToLog("Изменения отменены.");
                    SelectNodeAfterCanselUpdate();
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                   "Ошибка отмены внесенных изменений.\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }
        /// <summary>
        /// Отменяет сделанные в списке изменения
        /// </summary>
        /// <returns></returns>
        private void CancelChange()
        {
            try
            {
                if (IsModified())
                {
                    if (bRefreshTreeList() == false)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show(
                          "Ошибка отмены внесенных изменений.", "Ошибка",
                          System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка отмены внесенных изменений.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            return;
        }

        #endregion

        #region Удаление объекта
        /// <summary>
        /// Удаляет из БД запись
        /// </summary>
        /// <param name="iRowHndl">указатель на измененнную запись</param>
        /// <returns>true - удачное завершение операции; false - ошибка</returns>
        private System.Boolean bDeleteObject()
        {
            System.Boolean bRet = false;
            try
            {
                if (treeList.Nodes.Count == 0)
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Список объектов пуст.", "Внимание",
                       System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return bRet;
                }

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                // удаление
                System.Int32 iRowHndl = treeList.GetNodeIndex(treeList.FocusedNode);
                System.String strName = ((ERP_Mercury.Common.CBusinessObject)propertyGrid.SelectedObject).Name;
                if (((ERP_Mercury.Common.CBusinessObject)propertyGrid.SelectedObject).ID.CompareTo(System.Guid.Empty) == 0)
                {
                    bRet = true;
                }
                else
                {
                    bRet = ((ERP_Mercury.Common.CBusinessObject)propertyGrid.SelectedObject).Remove(this.m_objProfile);
                }
                if (bRet)
                {
                    treeList.FocusedNodeChanged -= new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeList_FocusedNodeChanged);
                    treeList.Nodes.RemoveAt(iRowHndl);
                    if (treeList.Nodes.Count > 0)
                    {
                        System.Int32 iSelectedIndex = (iRowHndl > 0) ? (iRowHndl - 1) : 0;
                        treeList.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeList_FocusedNodeChanged); // эталонный вариант
                        treeList.FocusedNode = treeList.Nodes[iSelectedIndex];
                        if (iSelectedIndex == 0 || iSelectedIndex == treeList.Nodes.Count-1)
                        {
                            if ((treeList.Nodes.Count > 0) && (treeList.FocusedNode != null) && (treeList.FocusedNode.Tag != null))
                            {
                                propertyGrid.SelectedObject = treeList.FocusedNode.Tag;
                            }
                        }
                    }

                    SetModified(false);
                    SendMessageToLog("Объект \"" + strName + "\" удален.");

                    bRet = true;
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка метода bDeleteAccTrnEvent.\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            return bRet;
        }

        private void barBtnDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (treeList.Nodes.Count == 0) { return; }
                if ((treeList.FocusedNode == null) || (propertyGrid.SelectedObject == null))
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("Необходимо выбрать запись для удаления.", "Внимание",
                       System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return;
                }

                if ((DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Вы действительно хотите удалить " + (System.String)treeList.FocusedNode.GetValue(colName) + " ?", "Подтверждение",
                    System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == DialogResult.Yes))
                {
                    if (bDeleteObject() == false)
                    {
                        switch (this.m_DirectSimple)
                        {
                            case EnumDirectSimple.Currency:
                                {
                                    ;
                                    break;
                                }
                            case EnumDirectSimple.Measure:
                                {
                                    ;
                                    break;
                                }
                            default:
                                {
                                    DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка удаления записи!", "Внимание", System.Windows.Forms.MessageBoxButtons.OK, 
                                                                               System.Windows.Forms.MessageBoxIcon.Error);
                                    break;
                                }       
                        }

                    }
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка удаления записи.\n\nТекст ошибки:\n" + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
            }

            return;
        }

        #endregion

        #region Новый объект
        private void barBtnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                NewObject();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка создания нового объекта.\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            return;
        }

        /// <summary>
        /// Создание нового объекта
        /// </summary>
        private void NewObject()
        {
            System.String strErr = "";
            try
            {
                DevExpress.XtraTreeList.Nodes.TreeListNode objNewNode = treeList.AppendNode(null, null);
                switch (this.m_DirectSimple)
                {
                    case EnumDirectSimple.StateType:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CStateType();
                            break;
                        }
                    case EnumDirectSimple.CustomerType:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CCustomerType();
                            break;
                        }
                    case EnumDirectSimple.AddressType:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CAddressType();
                            break;
                        }
                    case EnumDirectSimple.AddressPrefix:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CAddressPrefix();
                            break;
                        }
                    case EnumDirectSimple.Flat:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CFlat();
                            break;
                        }
                    case EnumDirectSimple.Building:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CBuilding();
                            break;
                        }
                    case EnumDirectSimple.SubBuilding:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CSubBuilding();
                            break;
                        }
                    case EnumDirectSimple.Country:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CCountry();
                            break;
                        }
                    case EnumDirectSimple.Region:
                        {
                            CRegion objRegion = new ERP_Mercury.Common.CRegion();
                            objRegion.InitOblastList( m_objProfile, null );
                            objNewNode.Tag = objRegion;
                            break;
                        }
                    case EnumDirectSimple.LocalityPrefix:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CLocalityPrefix();
                            break;
                        }
                    case EnumDirectSimple.City:
                        {
                            CCity objCity = new ERP_Mercury.Common.CCity();
                            objCity.InitRegionList(m_objProfile, null);
                            objCity.InitLocalityPrefixList(m_objProfile, null);

                            objNewNode.Tag = objCity;
                            break;
                        }
                    case EnumDirectSimple.Departament:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CDepartament();
                            break;
                        }
                    case EnumDirectSimple.JobPosition:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CJobPosition();
                            break;
                        }
                    case EnumDirectSimple.PhoneType:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CPhoneType();
                            break;
                        }
                    case EnumDirectSimple.Phone:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CPhone();
                            break;
                        }
                    case EnumDirectSimple.CustomerActiveType:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CCustomerActiveType();
                            break;
                        }
                    case EnumDirectSimple.LicenceType:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CLicenceType();
                            break;
                        }
                    case EnumDirectSimple.TargetBuy:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CTargetBuy();
                            break;
                        }
                    case EnumDirectSimple.RttType:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CRttType();
                            break;
                        }
                    case EnumDirectSimple.RttActiveType:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CRttActiveType();
                            break;
                        }
                    case EnumDirectSimple.RttSpecCode:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CRttSpecCode();
                            break;
                        }
                    case EnumDirectSimple.EquipmentType:
                        {
                            CEquipmentType objEquipmentType = new ERP_Mercury.Common.CEquipmentType();
                            objEquipmentType.InitProductCatalogList(m_objProfile, null);
                            objNewNode.Tag = objEquipmentType;
                            break;
                        }
                    case EnumDirectSimple.SizeEq:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CSizeEq();
                            break;
                        }
                    case EnumDirectSimple.Availability:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CAvailability();
                            break;
                        }
                    case EnumDirectSimple.ProductOwner:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CProductCatalog();
                            break;
                        }
                    case EnumDirectSimple.Segmentation:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CSalesSegmentation();
                            break;
                        }
                    case EnumDirectSimple.DistribNet:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CDistributionNetwork();
                            break;
                        }
                    case EnumDirectSimple.AccountType:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CAccountType();
                            break;
                        }
                    case EnumDirectSimple.Oblast:
                        {
                            COblast objOblast = new ERP_Mercury.Common.COblast();
                            objOblast.InitCountryList(m_objProfile, null);
                            objNewNode.Tag = objOblast;
                            break;
                        }
                    case EnumDirectSimple.CustomerCategory:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CCustomerCategory();
                            break;
                        }
                    case EnumDirectSimple.ConditionObjectType:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CConditionObjectType();
                            break;
                        }
                    case EnumDirectSimple.ConditionGroupType:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CConditionGroupType();
                            break;
                        }
                    case EnumDirectSimple.RuleType:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CRuleType();
                            break;
                        }
                    case EnumDirectSimple.StoredProcedure:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CStoredProcedure();
                            break;
                        }
                    case EnumDirectSimple.ProductVTM:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CProductVtm();
                            break;
                        }
                    case EnumDirectSimple.ProductType:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CProductType();
                            break;
                        }
                    case EnumDirectSimple.ProductLine:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CProductLine();
                            break;
                        }
                    case EnumDirectSimple.ProductSubType:
                        {
                            CProductSubType objProductSubType = new ERP_Mercury.Common.CProductSubType();
                            objProductSubType.InitProductLineList(m_objProfile, null);

                            objNewNode.Tag = objProductSubType;
                            break;
                        }
                    case EnumDirectSimple.ProductTradeMark:
                        {
                            CProductTradeMark objProductTradeMark = new ERP_Mercury.Common.CProductTradeMark();
                            objProductTradeMark.InitProductVtmList(m_objProfile, null);

                            objNewNode.Tag = objProductTradeMark;
                            break;
                        }
                    case EnumDirectSimple.ProductCategory:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CProductCategory();
                            break;
                        }
                    case EnumDirectSimple.VendorContractType:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CVendorContractType();
                            break;
                        }
                    case EnumDirectSimple.VendorPaymentDocType:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CVendorPaymentDocType();
                            break;
                        }
                    case EnumDirectSimple.CarrierRateType:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CarrierRateType();
                            break;
                        }
                    case EnumDirectSimple.Driver:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CDriver();
                            break;
                        }
                    case EnumDirectSimple.Vehicle:
                        {
                            CVehicle objVehicle = new ERP_Mercury.Common.CVehicle();
                            objVehicle.InitDriverList(m_objProfile, null);
                            objNewNode.Tag = objVehicle;
                            break;
                        }
                    case EnumDirectSimple.AdvancedExpenseType:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CRouteSheetAdvancedExpenseType();
                            break;
                        }
                    case EnumDirectSimple.RouteSheetType:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CRouteSheetType();
                            break;
                        }
                    case EnumDirectSimple.PriceType:
                        {
                            CPriceType objPriceType = new ERP_Mercury.Common.CPriceType();
                            objPriceType.InitCurrencyList(m_objProfile, null);
                            objNewNode.Tag = objPriceType;
                            break;
                        }
                    case EnumDirectSimple.RegionDelivery:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CRegionDelivery();
                            break;
                        }
                    case EnumDirectSimple.AgreementState:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CAgreementState();
                            break;
                        }
                    case EnumDirectSimple.AgreementType:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CAgreementType();
                            break;
                        }
                    case EnumDirectSimple.AgrementBasement:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CAgrementBasement();
                            break;
                        }
                    case EnumDirectSimple.AgrementReason:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CAgrementReason();
                            break;
                        }
                    case EnumDirectSimple.AgreementDeliveryCondition:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CAgreementDeliveryCondition();
                            break;
                        }
                    case EnumDirectSimple.AgreementPaymentCondition:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CAgreementPaymentCondition();
                            break;
                        }
                    case EnumDirectSimple.Stock:
                        {
                            CStock objStock = new ERP_Mercury.Common.CStock();
                            //objStock.InitWareHouseTypeList(m_objProfile, null); // т.к. при создании нового объекта, место хранения неизвестно и тип места хранения, так же ещё не определён
                            objStock.InitWareHouseList(m_objProfile, null);
                            objStock.InitCompanyList(m_objProfile, null);

                            objNewNode.Tag = objStock;

                            break;
                        }
                    case EnumDirectSimple.WareHouse:
                        {
                            // не используется. WareHouse можно удалять
                            // CWarehouse objWarehouse = new ERP_Mercury.Common.CWarehouse();
                            break;
                        }
                    case EnumDirectSimple.WareHouseType :
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CWareHouseType();
                            break;  
                        }
                    case EnumDirectSimple.ChildCustCode:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CChildDepart();
                            break;
                        }
                    case EnumDirectSimple.VendorType:
                        {
                            /*DevExpress.XtraEditors.XtraMessageBox.Show("Новый тип поставщика не добавлен ! \n В данный момент такая возможность отсутствует.", "Запрещено добавить новый тип поставщика",
                            System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information );
                            objNewNode.Visible = false;*/
                            //return;
                            objNewNode.Tag = new ERP_Mercury.Common.CVendorType();
                            break;
                        }
                    case EnumDirectSimple.Currency:
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CCurrency();
                            break;
                        }
                    case EnumDirectSimple.Measure :
                        {
                            objNewNode.Tag = new ERP_Mercury.Common.CMeasure();
                            break;
                        }
                    case EnumDirectSimple.CertificateType:
                        {
                            // типы документов о качестве товара
                            objNewNode.Tag = new ERP_Mercury.Common.CCertificateType();
                            break;
                        }
                    case EnumDirectSimple.Const:
                        {
                            // константы
                            CConst objConst = new ERP_Mercury.Common.CConst();
                            objConst.InitConstTypeList(m_objProfile, null);

                            objNewNode.Tag = objConst;
                            break;
                        }
                    case EnumDirectSimple.ConstType:
                        {
                            // типы констант
                            objNewNode.Tag = new ERP_Mercury.Common.CConstType();
                            break;
                        }
                    case EnumDirectSimple.LotOrderState:
                        {
                            // состояние заказа
                            objNewNode.Tag = new ERP_Mercury.Common.CLotOrderState();
                            break;
                        }
                    case EnumDirectSimple.KLPState:
                        {
                            // состояние КЛП
                            objNewNode.Tag = new ERP_Mercury.Common.CKLPState();
                            break;
                        }
                    case EnumDirectSimple.Surcharges:
                        {
                            // типы дополнительных расходов
                            objNewNode.Tag = new ERP_Mercury.Common.CSurcharges();
                            break;
                        }
                    case EnumDirectSimple.VendorContractDelayType:
                        {
                            // виды отсрочки платежа
                            objNewNode.Tag = new ERP_Mercury.Common.CVendorContractDelayType() { IsActive = true };
                            break;
                        }
                    case EnumDirectSimple.NDSRate:
                        {
                            // ставки НДС
                            objNewNode.Tag = new ERP_Mercury.Common.CNDSRate() { RateValue = 0, IsActive = true };
                            break;
                        }
                    case EnumDirectSimple.SegmentationChanel:
                        {
                            // канал сбыта
                            objNewNode.Tag = new ERP_Mercury.Common.CSegmentationChanel();
                            break;
                        }
                    case EnumDirectSimple.SegmentationMarket:
                        {
                            // рынок сбыта
                            objNewNode.Tag = new ERP_Mercury.Common.CSegmentationMarket();
                            break;
                        }
                    case EnumDirectSimple.SegmentationSubChanel:
                        {
                            CSegmentationSubChannel objSegmentationSubChannel = new ERP_Mercury.Common.CSegmentationSubChannel();

                            objSegmentationSubChannel.InitSegmentationChannelList(m_objProfile, ref strErr);

                            objNewNode.Tag = objSegmentationSubChannel;
                            break;
                        }
                    case EnumDirectSimple.AccountPlan:
                        {
                            // счёт из плаена счетов
                            objNewNode.Tag = new CAccountPlan(); 
                            break;
                        }
                    case EnumDirectSimple.BudgetProject:
                        {
                            // проект
                            objNewNode.Tag = new CBudgetProject();
                            break;
                        }
                    case EnumDirectSimple.LotState:
                        {
                            // состояние заказа
                            objNewNode.Tag = new CLotState();
                            break;
                        }
                    case EnumDirectSimple.EarningType:
                        {
                            // вид оплаты
                            objNewNode.Tag = new CEarningType();
                            break;
                        }
                    case EnumDirectSimple.WaybillShipMode:
                        {
                            // вид отгрузки
                            objNewNode.Tag = new CWaybillShipMode();
                            break;
                        }
                    case EnumDirectSimple.WaybillState:
                        {
                            // состояние накладной
                            objNewNode.Tag = new CWaybillState();
                            break;
                        }
                    case EnumDirectSimple.BackWaybillState:
                        {
                            // состояние накладной на возврат товара
                            objNewNode.Tag = new CBackWaybillState();
                            break;
                        }
                    case EnumDirectSimple.WaybillBackReason:
                        {
                            // причина возврата товара
                            objNewNode.Tag = new CWaybillBackReason();
                            break;
                        }
                    case EnumDirectSimple.IntWaybillShipMode:
                        {
                            // вид отгрузки накладной на внутреннее перемещение
                            objNewNode.Tag = new CIntWaybillShipMode();
                            break;
                        }
                    case EnumDirectSimple.IntWaybillState:
                        {
                            // состояние накладной на внутреннее перемещение
                            objNewNode.Tag = new CIntWaybillState();
                            break;
                        }
                    case EnumDirectSimple.IntOrderShipMode:
                        {
                            // вид отгрузки заказа на внутреннее перемещение
                            objNewNode.Tag = new CIntOrderShipMode();
                            break;
                        }
                    case EnumDirectSimple.IntOrderState:
                        {
                            // состояние заказа на внутреннее перемещение
                            objNewNode.Tag = new CIntOrderState();
                            break;
                        }
                    default:
                        break;
                }
                treeList.FocusedNode = objNewNode;

                this.treeList_FocusedNodeChanged(treeList, new DevExpress.XtraTreeList.FocusedNodeChangedEventArgs(null, objNewNode));
                SetModified(true);

            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка создания нового объекта.\n\nТекст ошибки: " + f.Message, "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            return;
        }

        #endregion

        #region Печать

        private void barBtnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (DevExpress.XtraPrinting.PrintHelper.IsPrintingAvailable)
                    DevExpress.XtraPrinting.PrintHelper.ShowPreview(treeList);
                else
                    MessageBox.Show("XtraPrinting Library is not found...", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Cursor.Current = Cursors.Default;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("Ошибка печати\n" + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
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

        #region Экспорт в MS Excel
        /// <summary>
        /// Экспорт содержимого справочника в MS Excel
        /// </summary>
        private void ExportToExcel()
        {
            Excel.Application oXL;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;

            try
            {
                oXL = new Excel.Application();

                //Get a new workbook.
                oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
                oSheet = (Excel._Worksheet)oWB.Worksheets[1];

               switch (this.m_DirectSimple)
                {
                    case EnumDirectSimple.ProductLine:
                        {
                            // товарная линия
                            oSheet.Cells[1, 1] = "Код товарной линии";
                            oSheet.Cells[1, 2] = "Товарная линия";

                            break;
                        }
                    case EnumDirectSimple.ProductType:
                        {
                            // товарная группа
                            oSheet.Cells[1, 1] = "Код товарной группы";
                            oSheet.Cells[1, 2] = "Товарная группа";
                            break;
                        }
                    case EnumDirectSimple.ProductVTM:
                        {
                            // ВТМ
                            oSheet.Cells[1, 1] = "Код ВТМ";
                            oSheet.Cells[1, 2] = "ВТМ";
                            break;
                        }
                    case EnumDirectSimple.ProductTradeMark:
                        {
                            // Товарная марка
                            oSheet.Cells[1, 1] = "Код товарной марки";
                            oSheet.Cells[1, 2] = "Товарная марка";
                            break;
                        }
                    default:
                        {
                            oSheet.Cells[1, 1] = "Наименование";
                            break;
                        }
                }

                ERP_Mercury.Common.CBusinessObject objBusinessObject = null;
                System.Int32 iCurrentRecordIndex = 2;
                foreach ( DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes )
                {
                    if( objNode.Tag == null ) { continue; }
                    objBusinessObject = ((ERP_Mercury.Common.CBusinessObject)objNode.Tag);

                    switch (this.m_DirectSimple)
                    {
                        case EnumDirectSimple.ProductLine:
                            {
                                // товарная линия
                                oSheet.Cells[iCurrentRecordIndex, 1] = ((CProductLine)objBusinessObject).ID_Ib;
                                oSheet.Cells[iCurrentRecordIndex, 2] = objBusinessObject.Name;

                                break;
                            }
                        case EnumDirectSimple.ProductType:
                            {
                                // товарная группа
                                oSheet.Cells[iCurrentRecordIndex, 1] = ((CProductType)objBusinessObject).ID_Ib;
                                oSheet.Cells[iCurrentRecordIndex, 2] = objBusinessObject.Name;
                                break;
                            }
                        case EnumDirectSimple.ProductVTM:
                            {
                                // ВТМ
                                oSheet.Cells[iCurrentRecordIndex, 1] = ((CProductVtm)objBusinessObject).ID_Ib;
                                oSheet.Cells[iCurrentRecordIndex, 2] = objBusinessObject.Name;
                                break;
                            }
                        case EnumDirectSimple.ProductTradeMark:
                            {
                                // Товарная марка
                                oSheet.Cells[iCurrentRecordIndex, 1] = ((CProductTradeMark)objBusinessObject).ID_Ib;
                                oSheet.Cells[iCurrentRecordIndex, 2] = objBusinessObject.Name;
                                break;
                            }
                        default:
                            {
                                oSheet.Cells[iCurrentRecordIndex, 1] = objBusinessObject.Name;
                                break;
                            }
                    }

                    iCurrentRecordIndex++;


                    oSheet.get_Range("A1", "Z1").Font.Size = 12;
                    oSheet.get_Range("A1", "Z1").Font.Bold = true;
                    oSheet.get_Range("A1", "Z1").VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                    oSheet.get_Range("A1", "Z1").EntireColumn.AutoFit();

                    oSheet.get_Range("A1", "A1").AutoFilter(1, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);

                }

                oXL.Visible = true;
                oXL.UserControl = true;
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Ошибка экспорта в MS Excel.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                oSheet = null;
                oWB = null;
                oXL = null;
            }
        }

        private void barBtnExportToExcel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                ExportToExcel();
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Не произвести экспорт данных в MS Excel.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        #endregion

        private void SelectNodeAfterCanselUpdate()
        {
            try
            {
                if (m_iSn >= 0)
                {
                    treeList.SetFocusedNode(treeList.Nodes[m_iSn]);
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Не удалось восстановить фокус на редактируемой записи.\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        private void treeList_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                TreeListHitInfo hitInfo = treeList.CalcHitInfo(new Point(e.X, e.Y));
                if( hitInfo.HitInfoType == HitInfoType.Cell )
                {
                    m_iSn = ( (hitInfo.Node != null) ? hitInfo.Node.Id : -1) ;
                }
            }
            catch (System.Exception f)
            {
                DevExpress.XtraEditors.XtraMessageBox.Show(
                    "Метод \"treeList_MouseClick\".\n\nТекст ошибки: " + f.Message, "Ошибка",
                   System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            return;
        }

        private void txtFindObjectTradeMark_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                if (e.NewValue != null)
                {
                    System.String objFindItem = (System.String)e.NewValue;
                    if (objFindItem.Length > 0)
                    {
                        foreach (DevExpress.XtraTreeList.Nodes.TreeListNode objNode in treeList.Nodes)
                        {
                            if (System.Convert.ToString(objNode.GetValue(colName)).IndexOf(objFindItem, 0) >= 0)
                            {
                                treeList.FocusedNode = objNode;
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }



    }
}
