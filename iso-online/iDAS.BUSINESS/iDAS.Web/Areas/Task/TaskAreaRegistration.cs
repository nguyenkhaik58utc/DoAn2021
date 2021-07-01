using iDAS.Core;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Task
{
    [BaseSystem(Name = "Quản Lý Công Việc", IsActive = true, Icon = "Cog", IsShow = true, Position = 4)]
    public class TaskAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Task";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Task_default",
                "Task/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
