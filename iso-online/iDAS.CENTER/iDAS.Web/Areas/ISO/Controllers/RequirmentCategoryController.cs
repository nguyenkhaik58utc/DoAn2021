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
namespace iDAS.Web.Areas.ISO.Controllers
{
    public class RequirmentCategoryController : BaseController
    {
        private DepartmentRequirmentCategorySV RequirmentCategorySV = new DepartmentRequirmentCategorySV();
        public ActionResult Index()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Index" };
        }
        public ActionResult LoadCategory()
        {
            var result = RequirmentCategorySV.GetAll();
            return this.Store(result, result.Count);
        }
        public ActionResult Update(int id, string name)
        {
            DepartmentRequirmentCategoryItem data = new DepartmentRequirmentCategoryItem()
            {
                ID = id,
                Name = name
            };
            try
            {
                if (id != 0)
                {
                    RequirmentCategorySV.Update(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(name))
                        id = RequirmentCategorySV.Insert(data, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("storeRequirementCategory").Reload();
            }
            return this.Direct();
        }
        public ActionResult Delete(int id)
        {
            try
            {
                if (id != 0)
                {
                    RequirmentCategorySV.Delete(id);
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: "Dữ liệu đang sử dụng", error: true);
            }
            finally
            {
                X.GetCmp<Store>("storeRequirementCategory").Reload();
            }
            return this.Direct();
        }
    }
}
