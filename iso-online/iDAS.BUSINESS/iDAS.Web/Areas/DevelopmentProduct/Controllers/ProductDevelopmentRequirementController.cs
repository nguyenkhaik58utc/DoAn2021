using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.DevelopmentProduct.Controllers
{
    [BaseSystem(Name = "Yêu cầu phát triển sản phẩm", Icon = "RubyAdd", IsActive = true, IsShow = true, Position = 1)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ProductDevelopmentRequirementController : BaseController
    {
        //
        // GET: /DevelopmentProduct/ProductDevelopmentRequirement/
        private ProductDevelopmentRequirementSV procDevSV = new ProductDevelopmentRequirementSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters)
        {
            List<ProductDevelopmentRequirementItem> lstData;
            int total;
            lstData = procDevSV.GetAll(parameters.Page, parameters.Limit, out total);            
            return this.Store(new Paging<ProductDevelopmentRequirementItem>(lstData, total));
        }
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmAdd()
        {
            var data = new ProductDevelopmentRequirementItem();
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        [BaseSystem(Name = "Sửa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int id = 0)
        {
            var data = procDevSV.GetByID(id);
            if (data.IsWork)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.WARNING,
                    Title = "Thông báo",
                    Message = "Phiếu yêu cầu đã được thực hiện không được sửa!"

                });
                return this.Direct();
            }
            else
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };         
            }
        }
        public ActionResult GetStockProduct()
        {
            var data = new StockProductSV().GetAllforCombobox();
            return this.Store(data);
        }
        public ActionResult GetStockProductNotDev()
        {
            var data = new StockProductSV().GetAllforComboboxNotDev();
            return this.Store(data);
        }
        public ActionResult Update(ProductDevelopmentRequirementItem data,bool val)
        {
            try
            {
                if (data.ID == 0)
                {
                    data.IsWork = false;
                    procDevSV.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    if(val)
                    X.GetCmp<Window>("winNewProcDev").Close();
                }
                else
                {
                    procDevSV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    if (val)
                    X.GetCmp<Window>("winNewProcDev").Close();
                }
            }catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("stMnCommand").Reload();
            }
            return this.Direct();
        }
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(string stringId = "")
        {
            try
            {
                if (stringId != "")
                {
                    procDevSV.Delete(stringId);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("stMnCommand").Reload();
            }
            return this.Direct();
        }
        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowDetail(int id)
        {
            var obj = procDevSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Detail", Model = obj };
        }
    }
}
