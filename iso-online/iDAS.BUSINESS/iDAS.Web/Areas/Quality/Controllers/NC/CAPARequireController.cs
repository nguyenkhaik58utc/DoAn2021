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
namespace iDAS.Web.Areas.Quality.Controllers
{
    [SystemAuthorize(CheckAuthorize =false)]
    [BaseSystem(Name = "Quản lý khắc phục", Icon = "CssGo", IsActive = true, IsShow = true, Position = 7)]
    public class CAPARequireController : BaseController
    {
        private QualityCAPARequirementSV CAPARequireSV = new QualityCAPARequirementSV();
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem danh sách")]
        public ActionResult Index(int focusId = 0)
        {
            ViewBag.FocusId = focusId;
            return View();
        }
        public ActionResult LoadCAPARequire(StoreRequestParameters parameters, int id = 0, int focusId = 0)
        {
            int totalCount;
            var result = CAPARequireSV.GetByDepartment(parameters.Page, parameters.Limit, out totalCount, id, focusId);
            return this.Store(result, totalCount);
        }
        public ActionResult LoadCAPARequireNotCorrective(int qualityCriteriaID = 0)
        {

            var result = CAPARequireSV.GetNotCorrectiveByCriteriaID(qualityCriteriaID);
            return this.Store(result);
        }
        public ActionResult LoadCAPARequireNotCorrective_Q(int qualityCriteriaID = 0)
        {

            var result = CAPARequireSV.GetNotCorrectiveByCriteriaID_Q(qualityCriteriaID);
            return this.Store(result);
        }
        
    }
}