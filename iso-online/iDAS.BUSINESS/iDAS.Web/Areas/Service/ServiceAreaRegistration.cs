using iDAS.Core;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Service
{
    [BaseSystem(Name = "Quản Lý Cung Cấp Dịch Vụ", Icon = "Ruby", IsActive = true, IsShow = true, Position = 9)]
    public class ServiceAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Service";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Service_default",
                "Service/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
