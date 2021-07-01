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
    [BaseSystem(Name = "Luật định", IsActive = true, Icon = "AwardStarGold3", IsShow = true, Position = 3, Parent = DepartmentRequiredController.Code)]
    public class LegalRequiredController : BaseController
    {
        private DepartmentLegalAuditResultSV LegalAuditResultSV = new DepartmentLegalAuditResultSV();
        private DepartmentLegalSV LegalSV = new DepartmentLegalSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters, int departmentId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = LegalAuditResultSV.GetByDeparmtent(filter, User.Administrator, User.EmployeeID, departmentId);
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
                var data = new DepartmentLegalItem();
                if (id != 0)
                {
                    data = LegalSV.GetById(id);
                    data.ActionForm = Form.Edit;
                }
                else
                {
                    data.DepartmentApply = new HumanDepartmentViewItem() { ID = departmentId, Name = departmentName };
                    data.ActionForm = Form.Add;
                }
                data.IsRoot = LegalAuditResultSV.CheckRootDepartment(departmentId: departmentId);
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
        public ActionResult Update(DepartmentLegalItem data)
        {
            int id = data.ID;
            try
            {
                if (id != 0)
                {
                    LegalSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    id = LegalSV.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winLegalForm").Close();
                X.GetCmp<Store>("StoreLegalRequired").Reload();
            }
            return this.Direct(id);
        }
        #endregion

        #region Xóa
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id, int departmentId)
        {
            bool success = false;
            bool isPermiss = CheckPermission("Delete", departmentId);
            if (isPermiss)
            {
                try
                {
                    if (id != 0)
                    {
                        if (LegalSV.Delete(id))
                        {
                            Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                            X.GetCmp<Store>("StoreLegalRequired").Reload();
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
                    Ultility.CreateMessageBox(title: RequestMessage.Notify, message: RequestMessage.DataUsing, icon: MessageBox.Icon.ERROR, button: MessageBox.Button.OK);
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
            var data = new DepartmentLegalItem();
            if (id != 0)
            {
                data = LegalSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region xem nội dung
        [HttpGet]
        public ActionResult Content(int id = 0)
        {
            var data = new DepartmentLegalItem();
            if (id != 0)
            {
                data.Content = LegalSV.GetContent(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Content", Model = data };
        }
        #endregion

        #region Lấy danh sách ISO
        public ActionResult GetISO()
        {
            return this.Store(LegalSV.GetISO());
        }
        #endregion

        #region Đánh giá
        [BaseSystem(Name = "Đánh giá")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Audit(int departmentId, int LegalId, int id = 0)
        {

            if (!LegalSV.GetIsUse(LegalId))
            {
                Ultility.CreateMessageBox(RequestMessage.Notify, message: "Bản ghi không sử dụng");
                return this.Direct();
            }
            var data = new DepartmentLegalAuditResultItem();
            if (id != 0)
            {
                data = LegalAuditResultSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.HumanDepartmentID = departmentId;
                data.DepartmentLegalID = LegalId;
                data.ActionForm = Form.Add;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Audit", Model = data };
        }
        [HttpPost]
        public ActionResult Audit(DepartmentLegalAuditResultItem data)
        {
            int id = data.ID;
            try
            {
                if (id != 0)
                {
                    LegalAuditResultSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    id = LegalAuditResultSV.Insert(data, User.ID);
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
                X.GetCmp<Store>("StoreLegalRequired").Reload();
            }
            return this.Direct(id);
        }
        #endregion

        #region Phòng ban không đạt
        public ActionResult Department(int LegalId, int departmentId)
        {
            var data = LegalAuditResultSV.GetDepartmentFails(LegalId, departmentId);
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
                    urlSubmit = Url.Action("InsertNC", "LegalRequired", new { Area = "Department" }),
                    Name = "LegalAuditResultId",
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
        public ActionResult InsertNC(QualityNCItem item, int LegalAuditResultId)
        {
            var success = false;
            try
            {

                item.CreateBy = User.ID;
                LegalAuditResultSV.InsertNC(LegalAuditResultId, item);
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
                LegalSV.InsertDataTemplate(id, User.ID);
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
