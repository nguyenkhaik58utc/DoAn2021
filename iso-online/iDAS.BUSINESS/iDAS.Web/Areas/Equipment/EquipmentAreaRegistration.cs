using iDAS.Core;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Equipment
{
    [BaseSystem(Name = "Quản Lý Thiết Bị", IsActive = true, Icon = "ServerConnect", IsShow = true, Position = 12)]
    public class EquipmentAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Equipment";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Equipment_default",
                "Equipment/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
