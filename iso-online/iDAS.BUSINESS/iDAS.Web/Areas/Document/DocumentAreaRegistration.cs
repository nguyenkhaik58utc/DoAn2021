using System.Web.Mvc;
using iDAS.Core;
namespace iDAS.Web.Areas.Document
{
    [BaseSystem(Name = "Quản lý tài liệu", Icon = "Book", IsActive = true, IsShow = true, Position = 6)]
    public class DocumentAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Document";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Document_default",
                "Document/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
