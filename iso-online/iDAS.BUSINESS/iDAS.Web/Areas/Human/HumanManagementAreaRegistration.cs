using iDAS.Core;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Human
{
    [BaseSystem(Name = "Quản Lý Nhân Sự", IsActive = true, Icon = "Group", IsShow = true, Position = 2)]
    public class HumanAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Human";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Human_default",
                "Human/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
