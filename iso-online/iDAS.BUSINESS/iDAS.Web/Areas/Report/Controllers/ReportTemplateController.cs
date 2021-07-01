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
using Ext.Net;
namespace iDAS.Web.Areas.Report.Controllers
{
    public class ReportTemplateController : Controller
    {
        private ReportTemplateSV reportTemplateSV = new ReportTemplateSV();

        public ActionResult Index(string dataReportType, string exportReportUrl, string moduleCode, string functionCode)
        {
            try
            {
                ViewData["DataReportType"] = dataReportType;
                ViewData["ExportReportUrl"] = exportReportUrl;
                ViewData["ModuleReport"] = moduleCode;
                ViewData["FunctionReport"] = functionCode;
                return new Ext.Net.MVC.PartialViewResult { ViewData = ViewData };
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }

        public ActionResult LoadData(string moduleCode, string functionCode) 
        {
            try
            {
                var reportTemplates = reportTemplateSV.GetReportTemplates(moduleCode, functionCode);
                return this.Store(reportTemplates);
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
                    if (!reportTemplateSV.CheckNameExist(item.Name))
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

        public ActionResult Export(int reportTemplateID)
        {
            try
            {
                ViewData["ReportTemplateID"] = reportTemplateID;
                return new Ext.Net.MVC.PartialViewResult { ViewData = ViewData };
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }

        [HttpPost]
        public ActionResult Export(int reportTemplateID,object data1)
        {
            var success = false;
            try
            {
                if (ModelState.IsValid)
                {
                    //var data = new HumanEmployeeSV().GetAll();
                    //DataReport dataReport = new DataReport();
                    //dataReport.Tilte = "iDAS TECHNOLOGY 2016";
                    //dataReport.Date = DateTime.Now.ToShortDateString();
                    //dataReport.Footer = data.Count.ToString(); ;
                    //dataReport.Employees = new List<HumanEmployeeItem>();//data;
                    //dataReport.setNo();
                   // var file = reportTemplateSV.Export(dataReport, reportTemplateID);
                    //this.FormPanel().Success = true;
                    //this.FormPanel().ExtraParams.Add(new Parameter() { Name = "file", Value = file, Mode = ParameterMode.Value });
                    //return this.FormPanel();
                    //return this.File(file,"application/pdf");
                    //X.GetCmp<Hidden>("FileUrl").SetValue(file);
                    success = true;
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
    }
}
