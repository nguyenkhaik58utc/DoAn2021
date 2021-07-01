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
namespace iDAS.Web.Areas.Accounting.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Danh Mục Phân Loại Nợ", Icon = "Money", IsActive = true, IsShow = true, Position = 1)]
    public class DebtTypeController : BaseController
    {
        private AccountingDebtTypeSV DebtTypeSV = new AccountingDebtTypeSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadDebtType(StoreRequestParameters parameters)
        {
            int totalCount;
            var result = DebtTypeSV.GetAll(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(result, totalCount);
        }
        #region Cập nhật
        [BaseSystem(Name = "Sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int id = 0)
        {
            var data = new AccountingDebtTypeItem();
            if (id != 0)
            {
                data = DebtTypeSV.GetById(id);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.ActionForm = Form.Add;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        public ActionResult Update(AccountingDebtTypeItem data)
        {
            try
            {
                if (data.ID != 0)
                {
                    DebtTypeSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    DebtTypeSV.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Window>("winDebtType").Close();
                X.GetCmp<Store>("StoreDebtType").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Xóa
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id = 0)
        {
            try
            {
                if (id != 0)
                {
                    if (DebtTypeSV.Delete(id))
                    {
                        Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                        X.GetCmp<Store>("StoreDebtType").Reload();
                    }
                    else
                    {
                        Ultility.CreateMessageBox(title: RequestMessage.Notify, message: RequestMessage.DataUsing, icon: MessageBox.Icon.ERROR, button: MessageBox.Button.OK);
                    }
                }
            }
            catch
            {
                Ultility.CreateMessageBox(title: RequestMessage.Notify, message: RequestMessage.DataUsing, icon: MessageBox.Icon.ERROR, button: MessageBox.Button.OK);
            }
            return this.Direct();
        }
        #endregion

        #region Xem chi tiết
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int id = 0)
        {
            var data = new AccountingDebtTypeItem();
            if (id != 0)
            {
                data = DebtTypeSV.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

    }
}
