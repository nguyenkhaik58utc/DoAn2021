using iDAS.Core;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Department
{
    [BaseSystem(Name = "Quản Lý Tổ Chức",Icon = "House", IsActive = true, IsShow = true, Position = 1)]
    public class DepartmentAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Department";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Department_default",
                "Department/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
