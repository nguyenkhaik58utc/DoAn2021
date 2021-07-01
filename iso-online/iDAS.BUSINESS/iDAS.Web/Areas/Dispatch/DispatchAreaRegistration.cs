using iDAS.Core;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Dispatch
{
     [BaseSystem(Name = "Quản Lý Công Văn", Icon = "Package", IsActive = true, IsShow = true, Position = 7)]
    public class DispatchAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Dispatch";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Dispatch_default",
                "Dispatch/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
