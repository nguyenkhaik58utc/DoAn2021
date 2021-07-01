using iDAS.Core;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Systems
{
    [BaseSystem(Name = "Quản Lý Hệ Thống", IsActive = true, IsShow = true, Position = 1, Icon = "ApplicationHome")]
    public class SystemsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Systems";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Systems_default",
                "Systems/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
