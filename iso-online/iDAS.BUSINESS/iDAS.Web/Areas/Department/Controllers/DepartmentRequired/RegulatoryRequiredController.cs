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
namespace iDAS.Web.Areas.Department.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Quy định", IsActive = true, Icon = "Building", IsShow = true, Position = 2, Parent = DepartmentRequiredController.Code)]
    public class RegulatoryRequiredController : BaseController
    {
        private DepartmentRegulatoryAuditResultSV RegulatoryAuditResultSV = new DepartmentRegulatoryAuditResultSV();
        private DepartmentRegulatorySV RegulatorySV = new DepartmentRegulatorySV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters, int departmentId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = RegulatoryAuditResultSV.GetByDeparmtent(filter, User.Administrator, User.EmployeeID, departmentId);
            return this.Store(result, filter.PageTotal);
        }
        #region Cập nhật
        [BaseSystem(Name = "Thêm mới và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Update(int id = 0, int departmentId = 0, string departmentName = "")
        {
            bool isPermiss = CheckPermission("Update", departmentId);
            if (isPermiss)
            {
                var data = new DepartmentRegulatoryItem();
                if (id != 0)
                {
                    data = RegulatorySV.GetById(id);
                    data.ActionForm = Form.Edit;
                }
                else
                {
                    data.DepartmentApply = new HumanDepartmentViewItem() { ID = departmentId, Name = departmentName };
                    data.ActionForm = Form.Add;
                }
                data.IsRoot = RegulatoryAuditResultSV.CheckRootDepartment(departmentId: departmentId);
                if (!data.IsRoot && data.IsApplyAll && !User.Administrator)
                {
                    Ultility.CreateMessageBox(RequestMessage.Notify, RequestMessage.NotPermission, MessageBox.Icon.WARNING);
                    return this.Direct();
                }
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
            }
            Ultility.CreateMessageBox(RequestMessage.Notify, RequestMessage.NotPermission, MessageBox.Icon.WARNING);
            return this.Direct();
        }
        [HttpPost]
        public ActionResult Update(DepartmentRegulatoryItem data)
        {
            int id = data.ID;
            try
            {
                if (id != 0)
                {
                    RegulatorySV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    id = RegulatorySV.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winRegulatoryForm").Close();
                X.GetCmp<Store>("StoreRegulatoryRequired").Reload();
            }
            return this.Direct(id);
        }
        #endregion

        #region Xóa
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id, int departmentId)
        {
            bool isPermiss = CheckPermission("Delete", departmentId);
            if (isPermiss)
            {
                bool success = false;
                try
                {
                    if (id != 0)
                    {
                        if (RegulatorySV.Delete(id))
                        {
                            Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                            X.GetCmp<Store>("StoreRegulatoryRequired").Reload();
                            success = true;
                        }
                        else
                        {
                            Ultility.CreateNotification(message: RequestMessage.DataUsing, error: true);
                        }
                    }
                }
                catch
                {
                    Ultility.CreateMessageBox(title: RequestMessage.Notify, message: RequestMessage.DataUsing, icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                }
                return this.Direct(success);
            }
            Ultility.CreateMessageBox(RequestMessage.Notify, RequestMessage.NotPermission, MessageBox.Icon.WARNING);
            return this.Direct();
        }
        #endregion

        #region Xem chi tiết
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int id = 0)
        {
            var data = new DepartmentRegulatoryItem();
            if (id != 0)
            {
                data = RegulatorySV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region Lấy danh sách ISO
        public ActionResult GetISO()
        {
            return this.Store(RegulatorySV.GetISO());
        }
        #endregion

        #region xem nội dung
        [HttpGet]
        public ActionResult Content(int id = 0)
        {
            var data = new DepartmentRegulatoryItem();
            if (id != 0)
            {
                data.Content = RegulatorySV.GetContent(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Content", Model = data };
        }
        #endregion

        #region Đánh giá
        [BaseSystem(Name = "Đánh giá")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Audit(int departmentId, int RegulatoryId, int id = 0)
        {
            if (!RegulatorySV.GetIsUse(RegulatoryId))
            {
                Ultility.CreateMessageBox(RequestMessage.Notify, message: "Bản ghi không sử dụng");
                return this.Direct();
            }
            var data = new DepartmentRegulatoryAuditResultItem();
            if (id != 0)
            {
                data = RegulatoryAuditResultSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.HumanDepartmentID = departmentId;
                data.DepartmentRegulatoryID = RegulatoryId;
                data.ActionForm = Form.Add;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Audit", Model = data };
        }
        [HttpPost]
        public ActionResult Audit(DepartmentRegulatoryAuditResultItem data)
        {
            int id = data.ID;
            try
            {
                if (id != 0)
                {
                    RegulatoryAuditResultSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    id = RegulatoryAuditResultSV.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winAudit").Close();
                X.GetCmp<Store>("StoreRegulatoryRequired").Reload();
            }
            return this.Direct(id);
        }
        #endregion

        #region Phòng ban không đạt
        public ActionResult Department(int RegulatoryId, int departmentId)
        {
            var data = RegulatoryAuditResultSV.GetDepartmentFails(RegulatoryId, departmentId);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Department", Model = data };
        }
        #endregion

        #region Thêm mới điểm không phù hợp
        [HttpGet]
        public ActionResult ShowNC(int id, int ncId = 0)
        {
            if (ncId == 0)
            {
                var param = new
                {
                    Area = "Quality",
                    urlSubmit = Url.Action("InsertNC", "RegulatoryRequired", new { Area = "Department" }),
                    Name = "RegulatoryAuditResultId",
                    Value = id,
                    Mode = 1,
                };
                return this.RedirectToActionPermanent("InsertView", "NC", param);
            }
            else
            {
                var param = new
                {
                    Area = "Quality",
                    id = ncId
                };
                var ncItem = new QualityNCSV().GetByID(ncId);
                if (ncItem != null && ncItem.IsVerify)
                {
                    return this.RedirectToActionPermanent("DetailView", "NC", param);
                }
                else
                {
                    return this.RedirectToActionPermanent("UpdateView", "NC", param);
                }

            }
        }
        public ActionResult InsertNC(QualityNCItem item, int RegulatoryAuditResultId)
        {
            var success = false;
            try
            {

                item.CreateBy = User.ID;
                RegulatoryAuditResultSV.InsertNC(RegulatoryAuditResultId, item);
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                success = true;
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                throw;
            }
            return this.FormPanel(success);
        }
        #endregion

        #region Lấy dữ liệu mẫu
        public ActionResult InsertDataTemplate(int id)
        {
            try
            {
                RegulatorySV.InsertDataTemplate(id, User.ID);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct();
        }
        #endregion
    }
}
