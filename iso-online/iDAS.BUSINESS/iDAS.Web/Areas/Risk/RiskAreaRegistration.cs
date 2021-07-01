using iDAS.Core;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Risk
{
     [BaseSystem(Name = "Quản Lý Rủi Ro", IsActive = true, Icon = "FolderBug", IsShow = true, Position = 4)]
    public class RiskAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Risk";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Risk_default",
                "Risk/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
