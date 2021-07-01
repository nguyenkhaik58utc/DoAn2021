using iDAS.Core;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Accounting
{
    [BaseSystem(Name = "Quản Lý Kế Toán", IsActive = true, Icon = "Group", IsShow = true, Position = 11)]
    public class AccountingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Accounting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Accounting_default",
                "Accounting/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
