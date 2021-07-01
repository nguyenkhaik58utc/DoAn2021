using System.Web.Mvc;
using iDAS.Core;

namespace iDAS.Web.Areas.SystemManagement
{
    [BaseSystem(Name = "Quản Lý Hệ Thống", IsActive = true, IsShow = true)]
    public class SystemManagementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SystemManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SystemManagement_default",
                "SystemManagement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
