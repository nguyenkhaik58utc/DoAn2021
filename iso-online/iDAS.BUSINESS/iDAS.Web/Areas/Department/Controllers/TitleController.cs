using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Web.API;
using iDAS.Web.Areas.Department.Models;
using System.Web.Mvc;
using System.Threading.Tasks;
using iDAS.Utilities;
using System;

namespace iDAS.Web.Areas.Department.Controllers
{
    [BaseSystem(Name = "Danh mục chức danh", Icon = "PageGear", IsActive = true, IsShow = true)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class TitleController : BaseController
    {
        private TitleAPI api = new TitleAPI();

        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }

        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmAdd()
        {
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Create" };
        }

        [BaseSystem(Name = "Sửa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmUpdate(int id)
        {
            var obj = api.GetByID(id).Result;
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Edit", Model = obj };
        }

        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmDetail(int id)
        {
            var obj = api.GetByID(id).Result;
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Detail", Model = obj };
        }

        public ActionResult GetData(StoreRequestParameters parameters)
        {
            var resp = api.GetData(parameters.Page, parameters.Limit);
            return this.Store(new Paging<TitleDTO>(resp.Data, resp.TotalRows.Value));
        }
        public async Task<ActionResult> GetPosition()
        {
            var result = await api.GetPositionCbo();
            return this.Store(result);
        }

        public  ActionResult Insert(TitleDTO obj, bool val)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    api.Insert(obj, User.ID);
                    if (val)
                    {
                        X.GetCmp<Window>("winNewTitle").Close();
                    }
                    X.GetCmp<Store>("stTitleNew").Reload();
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    return this.Direct();
                }
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                throw;
            }
            return this.Direct();
        }

        public ActionResult Update(TitleDTO objEdit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objEdit.UserID = User.ID;
                    api.Update(objEdit);
                    X.GetCmp<Store>("stTitleNew").Reload();
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    return this.Direct();
                }
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw;
            }
            return this.Direct();
        }

        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id)
        {
            try
            {
                api.Delete(id);
                X.GetCmp<Store>("stTitleNew").Reload();
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                return this.Direct();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
                return this.Direct();
            }
        }
    }
}
