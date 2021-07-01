using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using iDAS.Services;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web;
using iDAS.Items;
using System.IO;
using iDAS.Utilities;
namespace iDAS.Web.Areas.Audit.Controllers
{
    public class AuditNCController : BaseController
    {
        private QualityNCSV NCSV = new QualityNCSV();
        public ActionResult AuditNCWindow(int auditId = 0)
        {
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewName = "AuditNCWindow",
                ViewData = ViewData,
            };
        }
        public ActionResult LoadNC(StoreRequestParameters parameters, int auditId = 0)
        {
            return View();
        }
        /// <summary>
        /// Xác nhận cho đánh giá
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public ActionResult AuditNCUpdate(QualityNCItem item, int qualityCriteriaID=0, int auditId = 0)
        {
            bool success = false;
            try
            {
                if (item.ID != 0)
                {
                    NCSV.Update(item, User.ID);
                }
                else
                {
                    NCSV.InsertFromAuditTask(item, qualityCriteriaID, auditId);
                }
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                success = true;
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw;
            }
            return this.FormPanel(success);
        }
    }
}
