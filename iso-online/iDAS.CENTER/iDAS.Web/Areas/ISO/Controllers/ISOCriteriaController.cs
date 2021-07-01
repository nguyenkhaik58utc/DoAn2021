using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Services;
using iDAS.Items;
using iDAS.Core;
using iDAS.Utilities;
using System.Collections;
using iDAS.Base;
using iDAS.Web.Areas.Systems;

namespace iDAS.Web.Areas.ISO.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    public class ISOCriteriaController : BaseController
    {
        ISOIndexCriteriaSV ISOIndexCriteriaSV = new ISOIndexCriteriaSV();
        public ActionResult List(int id = 0)
        {
            var data = new ISOIndexCriteriaItem() { ISOIndexID = id };
            return new Ext.Net.MVC.PartialViewResult { ViewName = "List", Model = data };
        }
        public ActionResult LoadISOCriteria(StoreRequestParameters parameters, int id = 0)
        {
            int totalCount;
            var modules = ISOIndexCriteriaSV.GetByIsoIndexID(parameters.Page, parameters.Limit, out totalCount, id);
            return this.Store(modules, totalCount);
        }

        #region Thêm mới
        [BaseSystem(Name = "Thêm mới", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult InsertForm(int ISOIndexId)
        {
            var data = new ISOIndexCriteriaItem() { ISOIndexID = ISOIndexId };
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Insert", Model = data };
        }
        public ActionResult Insert(ISOIndexCriteriaItem data)
        {
            data.CreateBy = User.ID;
            ISOIndexCriteriaSV.Insert(data);
            Ultility.CreateNotification(message: Message.InsertSuccess);
            X.GetCmp<Store>("ISOIndexCriteriaStore").Reload();
            return this.Direct();
        }
        #endregion

        #region Cập nhật
        [BaseSystem(Name = "Sửa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int id)
        {
            var data = ISOIndexCriteriaSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        public ActionResult Update(ISOIndexCriteriaItem data)
        {
            data.UpdateBy = User.ID;
            ISOIndexCriteriaSV.Update(data);
            Ultility.CreateNotification(message: Message.InsertSuccess);
            X.GetCmp<Store>("ISOIndexCriteriaStore").Reload();
            return this.Direct();
        }
        #endregion

        #region Chi tiết
        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int id)
        {
            var data = ISOIndexCriteriaSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region Xóa
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id)
        {
            if (id > 0)
            {
                try
                {
                    ISOIndexCriteriaSV.Delete(id);
                    Ultility.CreateNotification(message: Message.DeleteSuccess);
                    X.GetCmp<Store>("ISOIndexCriteriaStore").Reload();
                }
                catch (Exception ex)
                {
                    Ultility.CreateNotification(message: Message.DeleteDatabaseError);
                }
            }
            return this.Direct();
        }
        #endregion
    }
}
