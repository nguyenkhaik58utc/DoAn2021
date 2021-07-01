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
    public class ISOClauseController : BaseController
    {
        //
        // GET: /Quality/ISOStandard/
        private ISOClauseSV iSOStandardSV = new ISOClauseSV();
        public ActionResult GetData(StoreRequestParameters parameters, int isoId)
        {
            int total;
            var lstData = iSOStandardSV.GetAll(parameters.Page, parameters.Limit, out total, isoId);
            return this.Store(new Paging<CenterISOClauseItem>(lstData, total));
        }
        public ActionResult GetDataModule(int isoId)
        {
            var lstData = iSOStandardSV.GetModulesByISOID(isoId);
            return this.Store(lstData);
        }
        public ActionResult GetDataISOIndex(StoreRequestParameters parameters, int isoId)
        {
            List<CenterISOClauseItem> lstData;
            int total;
            lstData = iSOStandardSV.GetAll(parameters.Page, parameters.Limit, out total, isoId);
            return this.Store(lstData, total);
        }
        public ActionResult GetCriteria(StoreRequestParameters parameters, int cateId = 0)
        {
            List<CenterISOIndexCriteriaItem> lstData;
            int total;
            lstData = iSOStandardSV.GetByIsoIndexID(parameters.Page, parameters.Limit, out total, cateId);
            return this.Store(new Paging<CenterISOIndexCriteriaItem>(lstData, total));
        }
        public ActionResult DetailForm(int id)
        {
            var data = iSOStandardSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        public ActionResult Detail()
        {
            return View();
        }
    }
}
