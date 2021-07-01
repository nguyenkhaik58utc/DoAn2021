using System.Web.Mvc;
using iDAS.Core;

namespace iDAS.Web.Areas.Customer
{
    [BaseSystem(Name = "Quản Lý Khách Hàng ", IsActive = true, Icon = "Group", IsShow = true, Position = 8)]
    public class CustomerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Customer";
            }
        }
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Customer_default",
                "Customer/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
