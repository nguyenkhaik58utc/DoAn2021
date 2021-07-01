using Ext.Net;
using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net.MVC;
using iDAS.Services;
using iDAS.Items;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Quality.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Tiêu chí đánh giá", IsActive = true, IsShow = false, Position = 4)]
    public class CriteriaController : BaseController
    {
        private QualityCriteriaSV criteriaSV = new QualityCriteriaSV();
        public ActionResult GetData(StoreRequestParameters parameters, int cateId = 0)
        {
            List<QualityCriteriaItem> lstData;
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            lstData = criteriaSV.GetByCateId(filter, cateId);
            return this.Store(lstData, filter.PageTotal);
        }
        public ActionResult GetCriteria(StoreRequestParameters parameters, int cateId = 0)
        {
            List<QualityCriteriaItem> lstData;
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            lstData = criteriaSV.GetByCateId(filter, cateId);
            return this.Store(lstData,filter.PageTotal);
        }
        public ActionResult GetCriteriaUsed(StoreRequestParameters parameters, string strCateId)
        {
            List<QualityCriteriaItem> lstData;
            int total;
            lstData = criteriaSV.GetCriteriaUsedByCateIds(parameters.Page, parameters.Limit, out total, strCateId);
            return this.Store(new Paging<QualityCriteriaItem>(lstData, total));
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thêm mới")]
        public ActionResult FormAdd(int cateId, string cateName)
        {
            var data = new QualityCriteriaItem()
            {
                CategoryID = cateId,
                CategoryName = cateName
            };
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Create", Model = data };
        }
        public ActionResult FormList(int cateId)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "FrmList", ViewData = new ViewDataDictionary { { "cateId", cateId } } };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        /// <summary>
        /// Hàm insert dữ liệu
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="isuse"></param>
        /// <returns></returns>
        public ActionResult Insert(QualityCriteriaItem objNew)
        {
            try
            {
                if (criteriaSV.Insert(objNew, User.ID) > 0)
                {
                    X.GetCmp<Store>("StoreTreeCriteria").Reload();
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Thông báo",
                        Message = "Tên tiêu chí đánh giá đã tồn tại!"
                    });
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError);
            }
            return this.Direct();
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Sửa")]
        public ActionResult ShowUpdate(int id)
        {
            var data = criteriaSV.GetById(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Edit", Model = data };

        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem chi tiết")]
        public ActionResult ShowDetail(int id)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = criteriaSV.GetById(id) };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        /// <summary>
        /// Hàm update dữ liệu
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="isuse"></param>
        /// <returns></returns>
        public ActionResult Update(QualityCriteriaItem objEdit)
        {
            try
            {
                if (criteriaSV.Update(objEdit, User.ID))
                {
                    X.GetCmp<FormPanel>("frCriteria").Reset();
                    X.GetCmp<Store>("StoreTreeCriteria").Reload();
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                }
                return this.Direct();
            }
            catch
            {
                return this.Direct("Error");
            }
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xóa")]
        public ActionResult Delete(int id)
        {
            try
            {
                criteriaSV.Delete(id);
                X.GetCmp<Store>("StoreTreeCriteria").Reload();
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                return this.Direct();
            }
            catch
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "Thông báo",
                    Message = "Tiêu chí đánh giá đã được sử dụng không được xóa!"
                });
                return this.Direct("Error");
            }
        }
    }
}
