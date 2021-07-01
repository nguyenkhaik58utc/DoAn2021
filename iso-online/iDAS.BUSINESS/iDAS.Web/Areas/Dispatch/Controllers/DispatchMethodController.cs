using iDAS.Core;
using iDAS.Items;

using iDAS.Services;
using Ext.Net.MVC;
using Ext.Net;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Utilities;
namespace iDAS.Web.Areas.Dispatch.Controllers
{

    [BaseSystem(Name = "Danh mục phương thức chuyển ", Icon = "PackageIn", IsActive = true, IsShow = true, Position = 2, Parent = DispatchCommonMenuController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class DispatchMethodController : BaseController
    {
        DispatchMethodService dispatchMethodSV = new DispatchMethodService();
        string store = "stMethod";
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters)
        {
            int totalCount;
            var lst = dispatchMethodSV.GetAll(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(lst, totalCount);
        }
        [BaseSystem(Name = "Thêm")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Insert(string name, bool isused, string des)
        {
            try
            {
                var objItem = new DispatchMethodItem
                {
                    Name = name.Trim(),

                    Note = des,
                    IsActive = isused,
                    CreateAt = DateTime.Now,
                    CreateBy = User.ID,
                };
                //string ck = checkValid(objItem);
                //if (ck.Equals(""))
                //{
                objItem.CreateBy = User.ID;
                dispatchMethodSV.Insert(objItem);
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                X.GetCmp<Store>(store).Reload();
                //}
                //else
                //    Common.ShowExtMsg(MessageBox.Button.OK, MessageBox.Icon.ERROR, "Thông báo", ck);

                return this.Direct();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                return this.Direct("Error");
            }
        }
        [BaseSystem(Name = "Sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Update(int id, string name, bool isused, string des)
        {
            try
            {
                var objItem = new DispatchMethodItem
                {
                    ID = id,
                    Name = name.Trim(),
                    Note = des,
                    IsActive = isused,
                    CreateAt = DateTime.Now,
                    CreateBy = User.ID,
                };
                //string ck = checkValid(objItem, id);
                //if (ck.Equals(""))
                //{
                objItem.UpdateBy = User.ID;
                dispatchMethodSV.Update(objItem);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                X.GetCmp<Store>(store).Reload();
                //}
                //else
                //    Common.ShowExtMsg(MessageBox.Button.OK, MessageBox.Icon.ERROR, "Thông báo", ck);
                return this.Direct();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                return this.Direct("Error");
            }
        }
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id)
        {
            try
            {
                string ck = checkValidDelete(id);
                if (ck.Equals(""))
                {
                    dispatchMethodSV.Delete(id);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    X.GetCmp<Store>(store).Reload();
                }
                else
                    Common.ShowExtMsg(MessageBox.Button.OK, MessageBox.Icon.WARNING, "Thông báo", ck);

                return this.Direct();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
                return this.Direct("Error");
            }
        }
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrm(int id = 0)
        {
            if (id > 0)
            {
                var obj = dispatchMethodSV.GetByID(id);
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
            }
            return null;
        }
        private string checkValid(DispatchSecurityItem objDraft, int id = 0)
        {
            bool ckexit = true;
            objDraft.Name = objDraft.Name.Trim();
            if (id > 0)
            {
                var docItem = dispatchMethodSV.GetByID(id);
                if (docItem != null)
                {
                    if (docItem.Name.Trim().ToUpper().Equals(objDraft.Name.ToUpper()))
                        ckexit = false;
                }
            }
            if (ckexit)
            {
                var doc = dispatchMethodSV.GetByName(objDraft.Name);
                if (doc != null)
                {
                    return "Phương thức chuyển đã tồn tại trên hệ thống. Vui lòng nhập lại Phương thức chuyển!";
                }
            }
            return "";
        }
        private string checkValidDelete(int id)
        {
            //Ktra truong hop Ma TL da ton tai trong he thong
            if (id > 0)
            {
                var docItem = dispatchMethodSV.CheckNotIsUse(id);
                if (!docItem)
                {
                    return "Phương thức chuyển đã được sử dụng trong bảng khác trên hệ thống nên không được phép Xóa!";
                }
            }
            return "";
        }
        private DispatchSecurityItem convertToTypeItem(string name, bool inused, string des, int id = 0)
        {
            var objItem = new DispatchSecurityItem
            {
                Name = name.Trim(),

                Note = des,
                IsActive = inused,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
            };
            if (id > 0) objItem.ID = id;
            return objItem;
        }
    }
}
