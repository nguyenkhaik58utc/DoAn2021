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
namespace iDAS.Web.Areas.Human.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Tiêu chí xét tuyển", Icon = "TagGreen", IsActive = true, IsShow = true, Position = 1, Parent = GroupRecruitmentController.Code)]
    public class RecruitmentCriteriaController : BaseController
    {
        private HumanRecruitmentCriteriaSV RecruitmentCriteriaService = new HumanRecruitmentCriteriaSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadCriteria(StoreRequestParameters parameters, int departmentID)
        {
            int totalCount;
            return this.Store(RecruitmentCriteriaService.GetByDepartment(parameters.Page, parameters.Limit, out totalCount, departmentID), totalCount);
        }
        #region Cập nhật
        [BaseSystem(Name = "Thêm mới và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Update(int ID = 0)
        {
            var data = new HumanRecruitmentCriteriaItem();
            if (ID != 0)
            {
                data = RecruitmentCriteriaService.GetById(ID);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.ActionForm = Form.Add;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        [HttpPost]
        public ActionResult Update(HumanRecruitmentCriteriaItem updateData)
        {
            try
            {
                if (updateData.ID == 0)
                {
                    RecruitmentCriteriaService.Insert(updateData, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                else
                {
                    RecruitmentCriteriaService.Update(updateData, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winCriteria").Close();
                X.GetCmp<Store>("StoreCriteria").Reload();
            }
            return this.Direct();
        }
        #endregion
        /// <summary>
        /// Thực hiện vào form chi tiết tiêu chí tuyển dụng
        /// Author: Thanhnv. DateTime: 26/12/2014
        /// </summary>
        /// <param name="ID">Mã tiêu chí</param>
        /// <returns></returns>
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int ID)
        {
            var data = RecruitmentCriteriaService.GetById(ID);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DeleteCriteria(int id)
        {
            try
            {
                if (RecruitmentCriteriaService.Delete(id) == true)
                {
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    X.GetCmp<Store>("StoreCriteria").Reload();
                }
                else
                {
                    X.Msg.Confirm("Thông báo", "Có ràng buộc dữ liệu, bạn có muốn tiếp tục xóa không?", new MessageBoxButtonsConfig
                    {
                        Yes = new MessageBoxButtonConfig
                        {
                            Handler = "reDelete(" + id + ")",
                            Text = "Tiếp tục",
                        },
                        No = new MessageBoxButtonConfig
                        {
                            Text = "Hủy"
                        }
                    }).Show();
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError);
            }
            return this.Direct();
        }
        /// <summary>
        /// kích hoạt hoặc hủy kích hoạt tiêu chí
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult Active(int ID, bool IsActive)
        {
            try
            {
                RecruitmentCriteriaService.Active(ID, User.ID, IsActive);
                var messageSucess = IsActive ? RequestMessage.ActiveSuccess : RequestMessage.DeActiveSuccess;
                Ultility.CreateNotification(message: messageSucess);
            }
            catch
            {
                var messageError = IsActive ? RequestMessage.ActiveError : RequestMessage.DeActiveError;
                Ultility.CreateNotification(message: messageError, error: true).Show();
            }
            finally
            {
                X.GetCmp<Store>("StoreCriteria").Reload();
            }
            return this.Direct();
        }
        /// <summary>
        /// Xóa trạng thái bản ghi
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult reDeleteCriteria(int id)
        {
            try
            {
                RecruitmentCriteriaService.IsDelete(id, User.ID);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreCriteria").Reload();
            }
            return this.Direct();
        }
    }
}
