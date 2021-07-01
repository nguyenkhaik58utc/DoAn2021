using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Utilities;
namespace iDAS.Web.Areas.Quality.Controllers
{
    public class AuditResultIncorrectController : BaseController
    {
        //
        // GET: /Quality/AuditResultIncorrect/
        private AuditResultNCSV auditResultIncorrectSV = new AuditResultNCSV();
        public ActionResult LoadIncorrect(StoreRequestParameters parameters, int AuditID = 0)
        {
            int totalCount;
            var Task = auditResultIncorrectSV.GetAll(parameters.Page, parameters.Limit, out totalCount, AuditID);
            return this.Store(Task, totalCount);
        }
        public ActionResult InsertIncorrect(QualityNCItem incorrectItem, int AuditID)
        {
            bool success = false;
            try
            {
                if (ModelState.IsValid)
                {
                    incorrectItem.CreateBy = User.ID;
                    auditResultIncorrectSV.Insert(incorrectItem, AuditID);
                    success = true;
                }
            }
            catch
            {
                Ultility.ShowMessageBox(message: RequestMessage.CreateError, icon: MessageBox.Icon.ERROR);
            }
            return this.FormPanel(success);
        }

    }
}
