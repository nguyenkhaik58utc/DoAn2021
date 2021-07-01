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
namespace iDAS.Web.Areas.Customer.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Quản lý chiến dịch", IsActive = true, Icon = "Transmit", IsShow = true, Position = 4, Parent = GroupCommonController.Code)]
    public class CustomerCampaignController : BaseController
    {
        private CustomerCampaignSV CustomerCampaignService = new CustomerCampaignSV();
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem danh sách")]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Lấy danh sách
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ActionResult LoadCustomerCampaign(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = CustomerCampaignService.GetAll(filter);
            return this.Store(result, filter.PageTotal);
        }
        #region Cập nhật
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thêm mới và sửa")]
        public ActionResult UpdateForm(int ID = 0)
        {
            var data = new CustomerCampaignItem();
            if (ID != 0)
            {
                data = CustomerCampaignService.GetById(ID);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.ActionForm = Form.Add;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }

        public ActionResult Update(CustomerCampaignItem data)
        {
            try
            {
                if (data.ID != 0)
                {
                    CustomerCampaignService.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    CustomerCampaignService.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Window>("WinCustomerCompaignUpdate").Close();
                X.GetCmp<Store>("StoreCustomerCampaign").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Xóa
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xóa")]
        public ActionResult Delete(int ID = 0)
        {
            try
            {
                if (ID != 0)
                {
                    CustomerCampaignService.Delete(ID);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreCustomerCampaign").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Xem chi tiết
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem chi tiết")]
        public ActionResult DetailForm(int ID = 0)
        {
            var data = new CustomerCampaignItem();
            if (ID != 0)
            {
                data = CustomerCampaignService.GetById(ID);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region Phê duyệt
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult ApproveForm(int ID = 0, bool isSend = true)
        {
            var data = new CustomerCampaignItem();
            if (ID != 0)
            {
                data = CustomerCampaignService.GetById(ID);
            }
            return isSend ? new Ext.Net.MVC.PartialViewResult { ViewName = "SendApprove", Model = data } : new Ext.Net.MVC.PartialViewResult { ViewName = "Approve", Model = data };

        }
        /// <summary>
        /// Xử lý thông tin gửi duyệt
        /// </summary>
        /// <param name="updateData"></param>
        /// <param name="exit"></param>
        /// <returns></returns>
        public ActionResult SendApprove(CustomerCampaignItem updateData)
        {
            var success = false;
            try
            {
                if (updateData.ID != 0)
                {
                    CustomerCampaignService.SendApproval(updateData);
                    success = true;
                    //if(updateData.IsSendApproval == true)
                    //{
                    //    Ultility.CreateNotification(message: Message.SendSuccess);
                    //}
                    //else
                    //{
                    //    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    //}

                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.SendError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreCustomerCampaign").Reload();
            }
            return this.FormPanel(success);
        }
        /// <summary>
        /// Thực hiện phê duyệt kế hoạch 
        /// </summary>
        /// <param name="updateData"></param>
        /// <returns></returns>
        public ActionResult Appproved(CustomerCampaignItem updateData)
        {
            var success = false;
            try
            {
                if (updateData.ID != 0)
                {
                    updateData.IsPause = updateData.StatusValue == CustomerStatus.Pause ? true : false;
                    updateData.IsFinish = updateData.StatusValue == CustomerStatus.Finish ? true : false;
                    updateData.IsAccept = updateData.IsResult == true;
                    CustomerCampaignService.Approve(updateData);
                    success = true;
                    Ultility.CreateNotification(message: RequestMessage.ApproveSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.ApproveError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreCustomerCampaign").Reload();

            }
            return this.FormPanel(success);
        }
        #endregion

        #region Đánh giá
        public ActionResult AuditForm(int id = 0)
        {
            var data = new CustomerCampaignItem() { ID = id };
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Audit1", Model = data };
        }
        #endregion

    }
}
