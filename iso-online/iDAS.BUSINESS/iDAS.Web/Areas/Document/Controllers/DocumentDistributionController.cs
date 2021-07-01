using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using Ext.Net.MVC;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Utilities;
using iDAS.Web.Controllers;


namespace iDAS.Web.Areas.Document.Controllers
{
    
    [BaseSystem(Name = "Tài liệu phân phối", Icon = "Page", IsActive = true, IsShow = true, Position = 3, Parent = GroupDocumentsController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class DocumentDistributionController : BaseController
    {
        //
        // GET: /Document/DocumentDistribution/
        DocumentDistributionSV documentDistributionSV = new DocumentDistributionSV();
        public ActionResult Index()
        {
            ViewData["UserLogOn"] = User.EmployeeID;
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters, int DepartId)
        {
            int totalCount;
            var lst = documentDistributionSV.GetAllDocDistributionByDeptID(parameters.Page, parameters.Limit, out totalCount,DepartId);
            return this.Store(lst, totalCount);
        }
        public ActionResult Delete(int id)
        {
            try
            {
                documentDistributionSV.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                X.GetCmp<Store>("StoreDocumentDistributionList").Reload();
            }
            catch (Exception)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError);
            }
            return this.Direct();
        }
    }
}
