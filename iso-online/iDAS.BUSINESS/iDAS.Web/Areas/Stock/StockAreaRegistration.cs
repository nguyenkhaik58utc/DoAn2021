
using iDAS.Core;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Stock
{
    [BaseSystem(Name = "Quản Lý Kho", IsActive = true, Icon = "Door", IsShow = true, Position = 10)]
    public class StockAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Stock";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Stock_default",
                "Stock/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
