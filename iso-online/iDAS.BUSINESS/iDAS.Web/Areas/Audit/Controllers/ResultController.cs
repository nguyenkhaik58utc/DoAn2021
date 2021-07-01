using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using iDAS.DA;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Audit.Controllers
{
    public class ResultController : BaseController
    {
        private AuditResultSV resultSV = new AuditResultSV();
        private QualityCriteriaSV qualityCriteriaSV = new QualityCriteriaSV();
        public ActionResult LoadResult(StoreRequestParameters parameters, int cateId, int auditId = 0)
        {
            List<AuditResultItem> lstData;
            int totalCount;
            lstData = resultSV.GetByAuditID(parameters.Page, parameters.Limit, out totalCount, cateId, auditId);
            return this.Store(lstData, totalCount);
        }
        public ActionResult LoadPoint(int min = 0, int max = 0)
        {
            List<PointItem> lst = new List<PointItem>();
            for (int i = min; i <= max; i++)
            {
                lst.Add(new PointItem
                {
                    ID = i,
                    Point = i
                });
            }
            return this.Store(lst);
        }
        public ActionResult FrmEdit(int auditId = 0, int criteriaId = 0, int cateId = 0)
        {
            var objsd = resultSV.GetCateByID(cateId);
            if (!User.Administrator && (objsd.Audit == null || objsd.Audit.ID != User.EmployeeID))
            {
                Ultility.CreateMessageBox(title: "Thông báo", message: "Bạn không có quyền đánh giá công đoạn này", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                return this.Direct();
            }
            else
            {
                var min = resultSV.GetMin(criteriaId);
                var max = resultSV.GetMax(criteriaId);
                var obj = resultSV.GetByAuditIDAndCriterialID(auditId, criteriaId);
                var criteria = qualityCriteriaSV.GetById(criteriaId);
                return new Ext.Net.MVC.PartialViewResult() { ViewName = "Edit", Model = obj, ViewData = new ViewDataDictionary { { "auditId", auditId }, { "criteriaId", criteriaId }, { "criteriaName", criteria.Name }, { "criteriaNote", criteria.Note }, { "min", min }, { "max", max } } };
            }
        }
        public ActionResult FrmDetail(int auditId, int criteriaId)
        {
            var obj = resultSV.GetByAuditIDAndCriterialID(auditId, criteriaId);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Detail", Model = obj };
        }
        public ActionResult Update(AuditResultItem obj)
        {
            try
            {
                resultSV.InsertOrUpdate(obj, User.ID, User.EmployeeID);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw;
            }
            return this.Direct();
        }
        public ActionResult Delete(int auditId, int criteriaId)
        {
            try
            {
                resultSV.Delete(auditId, criteriaId);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
                throw;
            }
            return this.Direct();
        }
    }
}
