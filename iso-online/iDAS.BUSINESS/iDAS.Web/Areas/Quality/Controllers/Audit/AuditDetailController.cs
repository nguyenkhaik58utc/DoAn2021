using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Items;
using iDAS.Services;

namespace iDAS.Web.Areas.Quality.Controllers
{
    public class AuditDetailController : BaseController
    {
        //
        // GET: /Quality/AuditDetail/
        private QualityAuditProgramSV auditProgramService = new QualityAuditProgramSV();
        public ActionResult ShowFrmAdd(int programId, int auditPlanId)
        {
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Create", ViewData = new ViewDataDictionary { { "programId", programId }, { "auditPlanId", auditPlanId } } };
        }
        
    }
}
