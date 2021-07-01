using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using iDAS.Utilities;
using Ext.Net.MVC;
using iDAS.Items;
using iDAS.Services;
namespace iDAS.Web.Areas.Report.Controllers
{
    public class ReportController : Controller
    {
        private ReportTemplateSV reportTemplateSV = new ReportTemplateSV();

        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult LoadData() 
        {
            try
            {
               // var reportTemplates = reportTemplateSV.GetReportTemplates();
                //return this.Store(reportTemplates);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }

        public ActionResult Upload() {
            try
            {
                return new Ext.Net.MVC.PartialViewResult {  };
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }

        [HttpPost]
        public ActionResult Upload(ReportTemplateItem item)
        {
            var success = false;
            try
            {
                if (ModelState.IsValid)
                {
                    if (true)
                    {
                        reportTemplateSV.Insert(item);
                        Ultility.ShowMessageRequest(RequestStatus.CreateSuccess);
                        success = true;
                    }
                    else
                    {
                        Ultility.ShowMessageRequest(RequestStatus.ExistError);
                        success = false;
                    }
                }
                else
                {
                    Ultility.ShowMessageRequest(RequestStatus.ValidError);
                    success = false;
                }
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
                success = false;
            }
            return this.FormPanel(success);
        }

        public ActionResult Setup(){
            return View();
        } 
    }
}
