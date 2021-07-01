using iDAS.Core;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Web
{
    [BaseSystem(Name = "Quản Lý Website", IsActive = true, IsShow = true, Position = 6, Icon = "FolderPage")]
    public class WebAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Web";
            }
        }
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Web_default",
                "Web/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
