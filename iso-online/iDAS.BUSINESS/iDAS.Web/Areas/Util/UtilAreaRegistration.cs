using iDAS.Core;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Util
{
    [BaseSystem(Name = "Tiện ích mở rộng ", IsActive = true, Icon = "Group", IsShow = true, Position = 16)]
    public class UtilAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Util";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Util_default",
                "Util/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
