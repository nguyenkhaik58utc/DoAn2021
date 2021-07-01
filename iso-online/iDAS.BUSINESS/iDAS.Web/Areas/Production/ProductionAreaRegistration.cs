using iDAS.Core;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Production
{
    [BaseSystem(Name = "Quản Lý Sản Xuất", IsActive = true, Icon = "Wrench", IsShow = true, Position = 13)]
    public class ProductionAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Production";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Production_default",
                "Production/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
