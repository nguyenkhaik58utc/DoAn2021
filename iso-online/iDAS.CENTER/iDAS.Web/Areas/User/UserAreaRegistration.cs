using iDAS.Core;
using System.Web.Mvc;

namespace iDAS.Web.Areas.User
{
    [BaseSystem(Name = "Quản Lý Người Dùng", IsActive = true, IsShow = true, Position = 2, Icon = "UserHome")]
    public class UserAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "User";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "User_default",
                "User/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
