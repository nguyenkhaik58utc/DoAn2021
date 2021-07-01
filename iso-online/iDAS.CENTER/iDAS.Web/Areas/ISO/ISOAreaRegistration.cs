using iDAS.Core;
using System.Web.Mvc;

namespace iDAS.Web.Areas.ISO
{
    [BaseSystem(Name = "Quản Lý ISO", IsActive = true, IsShow = true, Position = 3, Icon = "FolderTable")]
    public class ISOAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ISO";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ISO_default",
                "ISO/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
