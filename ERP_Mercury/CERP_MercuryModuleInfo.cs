using System;
using System.Reflection;
using UniXP.Common;

namespace ERP_Mercury
   {

      
        public class CERPModuleInfo : UniXP.Common.CClientModuleInfo
        {
            public CERPModuleInfo()
             : base( Assembly.GetExecutingAssembly(),
                   UniXP.Common.EnumDLLType.typeOptions,
                   new System.Guid( "{A6319AD0-08C0-49ED-B25B-659BAB622B15}" ),
                   new System.Guid( "{A6319AD0-08C0-49ED-B25B-659BAB622B15}" ),
                   ERP_Mercury.Properties.Resources.IMAGES_ERPMERCURY, ERP_Mercury.Properties.Resources.IMAGES_ERPMERCURYSMALL )
             {
             }

            /// <summary>
            /// Выполняет операции по проверке правильности установки модуля в системе.
            /// </summary>
            /// <param name="objProfile">Профиль пользователя.</param>
            public override System.Boolean Check( UniXP.Common.CProfile objProfile )
            {
                return true;
            }
            /// <summary>
            /// Выполняет операции по установке модуля в систему.
            /// </summary>
            /// <param name="objProfile">Профиль пользователя.</param>
            public override System.Boolean Install( UniXP.Common.CProfile objProfile )
            {
                return true;
            }
            /// <summary>
            /// Выполняет операции по удалению модуля из системы.
            /// </summary>
            /// <param name="objProfile">Профиль пользователя.</param>
            public override System.Boolean UnInstall( UniXP.Common.CProfile objProfile )
            {
                return true;
            }
            /// <summary>
            /// Производит действия по обновлению при установке новой версии подключаемого модуля.
            /// </summary>
            /// <param name="objProfile">Профиль пользователя.</param>
            public override System.Boolean Update( UniXP.Common.CProfile objProfile )
            {
                return true;
            }
            /// <summary>
            /// Возвращает список доступных классов в данном модуле.
            /// </summary>
            public override UniXP.Common.CModuleClassInfo GetClassInfo()
            {
                return null;
            }
        }

        public class ModuleInfo : PlugIn.IModuleInfo
        {
            public UniXP.Common.CClientModuleInfo GetModuleInfo()
            {
                return new CERPModuleInfo();
            }
        }
   }
