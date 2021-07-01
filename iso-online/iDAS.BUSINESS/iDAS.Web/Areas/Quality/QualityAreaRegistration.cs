using iDAS.Core;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Quality
{
     [BaseSystem(Name = "Quản Lý Chất Lượng", IsActive = true, Icon = "AwardStarGold2", IsShow = true, Position = 3)]
    public class QualityAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Quality";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Quality_default",
                "Quality/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
