using iDAS.Core;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Customer
{
    [BaseSystem(Name = "Quản Lý Khách Hàng", IsActive = true, IsShow = true, Position = 4, Icon = "UserSuit")]
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
