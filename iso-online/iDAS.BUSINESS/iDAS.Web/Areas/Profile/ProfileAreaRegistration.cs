using System.Web.Mvc;
using iDAS.Core;
namespace iDAS.Web.Areas.Profile
{
     [BaseSystem(Name = "Quản Lý Hồ Sơ", Icon = "Bookmark", IsActive = true, IsShow = true, Position = 5)]
    public class ProfileAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Profile";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Profile_default",
                "Profile/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
