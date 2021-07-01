using iDAS.Core;
using System.Web.Mvc;

namespace iDAS.Web.Areas.DevelopmentProduct
{
     [BaseSystem(Name = "Phát triển sản phẩm", Icon = "House", IsActive = true, IsShow = true, Position = 14)]
    public class DevelopmentProductAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DevelopmentProduct";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DevelopmentProduct_default",
                "DevelopmentProduct/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
