using System.Web.Mvc;

namespace iDAS.Web.Areas.Timekeeping
{
    public class TimekeepingAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Timekeeping";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Timekeeping_default",
                "Timekeeping/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}