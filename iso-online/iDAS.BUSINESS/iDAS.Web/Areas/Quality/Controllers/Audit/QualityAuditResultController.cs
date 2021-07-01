using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Items;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Services;
using iDAS.DA;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Quality.Controllers
{
    public class QualityAuditResultController : BaseController
    {
        //
        // GET: /Quality/QualityAuditResult/
        private QualityAuditResultSV qualityAuditResultSV = new QualityAuditResultSV();
        private ISOStandartCriteriaSV iSOStandartCriteriaSV = new ISOStandartCriteriaSV();
        public ActionResult LoadResult_Q(StoreRequestParameters parameters, int qualityAuditProgramId, int cateId, int qualityAuditProgramIsoId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lstData = iSOStandartCriteriaSV.GetByQualityAuditID_Q(filter, qualityAuditProgramId, cateId, qualityAuditProgramIsoId);
            return this.Store(lstData, filter.PageTotal);
        }
        public ActionResult FrmEdit_Q(int departmentId, int qualityAuditProgramId, int qualityAuditProgramIsoId = 0, int criteriaId = 0)
        {
            var obj = qualityAuditResultSV.GetByAuditIDAndCriterialID_Q(departmentId, qualityAuditProgramId, qualityAuditProgramIsoId, criteriaId);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Edit", Model = obj, ViewData = new ViewDataDictionary { { "qualityAuditProgramId", qualityAuditProgramId }, { "qualityAuditProgramIsoId", qualityAuditProgramIsoId }, { "criteriaId", criteriaId } } };
        }
        public ActionResult FrmDetail_Q(int departmentId, int qualityAuditProgramId, int qualityAuditProgramIsoId = 0, int criteriaId = 0)
        {
            var obj = qualityAuditResultSV.GetByAuditIDAndCriterialID_Q(departmentId, qualityAuditProgramId, qualityAuditProgramIsoId, criteriaId);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Detail", Model = obj, ViewData = new ViewDataDictionary { { "qualityAuditProgramId", qualityAuditProgramId } } };
        }
        public ActionResult Update(QualityAuditResultItem obj)
        {
            try
            {
                if (!iSOStandartCriteriaSV.CheckForUpdate(obj.QualityNCID.HasValue ? obj.QualityNCID.Value : 0))
                {
                    Ultility.CreateMessageBox(title: "Thông báo", message: "Tiêu chí đã sinh ra sự không phù hợp không được thay đổi!", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                }
                else
                {
                    iSOStandartCriteriaSV.InsertOrUpdate(obj, User.ID, User.EmployeeID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw;
            }
            return this.Direct();
        }
        public ActionResult Delete_Q(int id)
        {
            try
            {
                if (id != 0)
                {
                    if (qualityAuditResultSV.Delete(id))
                    {
                        Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    }
                    else
                    {
                        Ultility.CreateMessageBox(title: RequestMessage.Notify, message: "Tiêu chí đã sinh ra điểm không phù hợp không được phép xóa!", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                    }
                }
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
