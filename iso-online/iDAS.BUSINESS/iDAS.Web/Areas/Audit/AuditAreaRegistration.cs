﻿using iDAS.Core;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Audit
{
    public class AuditAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Audit";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Audit_default",
                "Audit/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
