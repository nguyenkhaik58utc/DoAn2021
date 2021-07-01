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
using iDAS.Utilities;
namespace iDAS.Web.Areas.Accounting.Controllers
{
    [BaseSystem(Name = "Hợp Đồng Cung Cấp", IsActive = true, Icon = "ApplicationSideContract", IsShow = true, Position = 2)]
    public class ContractController : BaseController
    {
        private AccountingContractSV AccountingContractSV = new AccountingContractSV();
        [BaseSystem(Name = "Xem danh sách")]
        public ActionResult Index(int Id = 0)
        {
            return View();
        }
        public ActionResult LoadContractAccounting(StoreRequestParameters parameters)
        {
            int totalCount;
            var result = AccountingContractSV.GetContract(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(result, totalCount);
        }

        #region Thiết lập đề xuất thanh toán
        public ActionResult AccountingPaymentForm(int id)
        {
            var data = new AccountingPaymentItem() { CustomerContractID = id };
            return new Ext.Net.MVC.PartialViewResult { ViewName = "AccountingPayment", Model = data };
        }
        public ActionResult LoadAccountingPayment(StoreRequestParameters parameters, int id)
        {
            int totalCount;
            var result = AccountingContractSV.GetAccountingPayment(parameters.Page, parameters.Limit, out totalCount, id);
            return this.Store(result, totalCount);
        }

        #region Cập nhật đề xuất thanh toán
        public ActionResult AccountingPaymentUpdateForm(int id, int customerContractId)
        {
            var data = new AccountingPaymentItem();
            if (id != 0)
            {
                data = AccountingContractSV.GetAccountingPaymentById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.ActionForm = Form.Add;
                data.CustomerContractID = customerContractId;
                data.TotalContract = AccountingContractSV.GetTotalByID(customerContractId);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "UpdateAccounting", Model = data };
        }
        public ActionResult UpdateAccounting(AccountingPaymentItem data)
        {
            try
            {
                if (data.ID != 0)
                {
                    AccountingContractSV.AccountingPaymentUpdate(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    AccountingContractSV.AccountingPaymentInsert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.SendError, error: true);
            }
            return this.Direct();
        }
        #endregion

        public ActionResult AccountingPaymentDeatailForm(int ID)
        {
            var data = new AccountingPaymentItem();
            if (ID != 0)
            {
                data = AccountingContractSV.GetAccountingPaymentById(ID);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "DetailAccounting", Model = data };
        }
        public ActionResult DeleteAccountingPayment(int ID = 0)
        {
            try
            {
                if (ID != 0)
                {
                    AccountingContractSV.AccountingPaymentDelete(ID);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreAccountingPayment").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Gửi duyệt
        public ActionResult SendApproveForm(int ID)
        {
            var data = new CustomerContractItem();
            if (ID != 0)
            {
                data = AccountingContractSV.GetContract(ID);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "SendApprove", Model = data };
        }
        public ActionResult SendApprove(CustomerContractItem data)
        {
            var success = false;
            try
            {
                AccountingContractSV.SendApprove(data);
                success = true;
                X.GetCmp<Store>("StoreCustomerContract").Reload();
                Ultility.CreateNotification(message: RequestMessage.SendSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.SendError, error: true);
            }
            return this.FormPanel(success);
        }
        #endregion

        #region Phê duyệt
        public ActionResult ApproveForm(int ID)
        {
            var data = new CustomerContractItem();
            if (ID != 0)
            {
                data = AccountingContractSV.GetContract(ID);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Approve", Model = data };
        }
        public ActionResult Approve(CustomerContractItem data)
        {
            try
            {
                AccountingContractSV.Approve(data);
                Ultility.CreateNotification(message: RequestMessage.ApproveSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.ApproveError, error: true);
            }
            return this.Direct();
        }
        #endregion

        #region Kế hoạch thu hồi công nợ
        public ActionResult AccountingPlanForm(int id)
        {
            var data = new AccountingPaymentItem() { CustomerContractID = id };
            return new Ext.Net.MVC.PartialViewResult { ViewName = "AccountingPlan", Model = data };
        }
        public ActionResult LoadPlan(StoreRequestParameters parameters, int contractId)
        {
            int totalCount;
            var result = AccountingContractSV.GetPlan(parameters.Page, parameters.Limit, out totalCount, contractId);
            return this.Store(result, totalCount);
        }
        public ActionResult LoadAccountingPaymentForAccounting(StoreRequestParameters parameters, int id)
        {
            int totalCount;
            var result = AccountingContractSV.GetAccountingPaymentForAccounting(parameters.Page, parameters.Limit, out totalCount, id);
            return this.Store(result, totalCount);
        }
        public ActionResult AccountingPlanUpdateForm(int id, int contractId)
        {
            ViewData["ContractID"] = contractId;
            if (id == 0)
            {
                return new Ext.Net.MVC.PartialViewResult
                {
                    ViewName = "UpdatePlan",
                    Model = new AccountingPaymentPlanItem()
                    {
                        TotalContract = AccountingContractSV.GetTotalByID(contractId),
                    },
                    ViewData = ViewData
                };
            }
            var data = AccountingContractSV.GetPlan(id);
            if (data != null && data.QualityPlanID != 0 && data.IsEdit == false)
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "DetailPlan", Model = data, ViewData = ViewData };
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "UpdatePlan", Model = data, ViewData = ViewData };
        }
        public ActionResult AccountingPaymentPlanDetailForm(int id, int contractId)
        {
            ViewData["ContractID"] = contractId;
            var data = AccountingContractSV.GetPlan(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "DetailPlan", Model = data, ViewData = ViewData };
        }

        public ActionResult UpdatePlan(AccountingPaymentPlanItem data)
        {
            var success = false;
            try
            {
                AccountingContractSV.UpdatePlan(data, User.ID);
                success = true;
                X.GetCmp<Store>("StoreAccountingPayment").Reload();
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.FormPanel(success);
        }
        public ActionResult DeletePlan(int id)
        {
            try
            {
                AccountingContractSV.DeleteAccountingPlan(id);
                X.GetCmp<Store>("StoreAccountingPayment").Reload();
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            return this.Direct();
        }
        public ActionResult ApprovePlanForm(int id, int contractId)
        {
            ViewData["ContractID"] = contractId;
            var data = AccountingContractSV.GetPlan(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "ApprovePlan", Model = data, ViewData = ViewData };
        }
        public ActionResult ApprovePlan(AccountingPaymentPlanItem data)
        {
            var success = false;
            try
            {
                AccountingContractSV.Approve(data, User.ID);
                success = true;
                X.GetCmp<Store>("StoreAccountingPayment").Reload();
                Ultility.CreateNotification(message: RequestMessage.ApproveSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.ApproveError, error: true);
            }
            return this.FormPanel(success);
        }
        public ActionResult RealPaymentForm(int id, int contractId)
        {
            ViewData["ContractID"] = contractId;
            var data = AccountingContractSV.GetPlan(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "RealPayment", Model = data, ViewData = ViewData };
        }
        public ActionResult RealPayment(AccountingPaymentPlanItem data)
        {
            try
            {
                AccountingContractSV.RealPayment(data, User.ID);
                X.GetCmp<Store>("StoreAccountingPayment").Reload();
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct();
        }
        #endregion

        #region Kết thúc hợp đồng
        public ActionResult EndContractform(int ID)
        {
            var data = new CustomerContractItem();
            if (ID != 0)
            {
                data = AccountingContractSV.GetContract(ID);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "EndContract", Model = data };
        }
        public ActionResult EndContract(CustomerContractItem data)
        {
            try
            {
                AccountingContractSV.EndContract(data, User.ID);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreCustomerContract").Reload();
            }
            return this.Direct();
        }
        #endregion
    }
}