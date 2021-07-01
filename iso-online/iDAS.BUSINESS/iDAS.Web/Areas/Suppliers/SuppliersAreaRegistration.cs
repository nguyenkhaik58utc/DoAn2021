using iDAS.Core;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Suppliers
{
    [BaseSystem(Name = "Quản Lý Nhà Cung Cấp ", IsActive = true, Icon = "Group", IsShow = true, Position = 14)]
    public class SuppliersAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Suppliers";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Suppliers_default",
                "Suppliers/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
