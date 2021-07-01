using System.Web.Mvc;

namespace iDAS.Web.Areas.Problem
{
    public class ProblemAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Problem";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Problem_default",
                "Problem/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}