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

namespace iDAS.Web.Areas.Quality.Controllers
{

    [BaseSystem(Name = "Tiêu chuẩn đánh giá", IsActive = true, IsShow = false, Position = 4)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class StandardController : BaseController
    {
        //
        // GET: /Task/Criteria/
        private QualityCriteriaSV criteriaSV = new QualityCriteriaSV();
        public ActionResult GetData(StoreRequestParameters parameters, int cateId)
        {
            List<QualityCriteriaItem> lstData;
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            lstData = criteriaSV.GetByCateId(filter, cateId);
            return this.Store(lstData);
        }
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult FormList(int cateId)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "FrmList", ViewData = new ViewDataDictionary { { "cateId", cateId } } };
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
        ///// <summary>
        ///// Hàm insert dữ liệu
        ///// </summary>
        ///// <param name="name"></param>
        ///// <param name="description"></param>
        ///// <param name="isuse"></param>
        ///// <returns></returns>
        //[BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        //[SystemAuthorize(CheckAuthorize = true)]
        //public ActionResult FormAdd()
        //{
        //    try
        //    {
        //        return new Ext.Net.MVC.PartialViewResult { ViewName = "Create" };
        //    }
        //    catch (Exception ex)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //}
        //public ActionResult Insert(QualityCriteriaItem objNew)
        //{
        //    try
        //    {
        //        if (criteriaSV.Insert(objNew, User.ID))
        //        {
        //            X.GetCmp<FormPanel>("frCriteria").Reset();
        //            X.GetCmp<Store>("stMnCriteria").Reload();
        //            X.GetCmp<GridPanel>("grMnCriteria").Refresh();
        //            Ultility.CreateNotification(message: RequestMessage.InsertSuccess);
        //        }
        //        else
        //        {
        //            X.Msg.Show(new MessageBoxConfig
        //            {
        //                Buttons = MessageBox.Button.OK,
        //                Icon = MessageBox.Icon.ERROR,
        //                Title = "Thông báo",
        //                Message = "Tên tiêu chí đánh giá đã tồn tại!"
        //            });
        //        }

        //    }
        //    catch
        //    {
        //        return this.Direct("Error");
        //    }
        //    return this.Direct();
        //}
        //public ActionResult ShowUpdate(int id)
        //{
        //    try
        //    {
        //        return new Ext.Net.MVC.PartialViewResult { ViewName = "Edit", Model = criteriaSV.GetById(id) };
        //    }
        //    catch (Exception ex)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //}
        //public ActionResult ShowDetail(int id)
        //{
        //    try
        //    {
        //        return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = criteriaSV.GetById(id) };
        //    }
        //    catch (Exception ex)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //}
        ///// <summary>
        ///// Hàm update dữ liệu
        ///// </summary>
        ///// <param name="name"></param>
        ///// <param name="description"></param>
        ///// <param name="isuse"></param>
        ///// <returns></returns>
        //[BaseSystem(Name = "Sửa", IsActive = true, IsShow = false)]
        //public ActionResult Update(QualityCriteriaItem objEdit)
        //{
        //    try
        //    {
        //        if (criteriaSV.Update(objEdit, User.ID))
        //        {
        //            X.GetCmp<FormPanel>("frCriteria").Reset();
        //            X.GetCmp<Store>("stMnCriteria").Reload();
        //            X.GetCmp<GridPanel>("grMnCriteria").Refresh();
        //            Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
        //        }
        //        else
        //        {
        //            Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
        //        }
        //        return this.Direct();
        //    }
        //    catch (Exception ex)
        //    {
        //        return this.Direct("Error");
        //    }
        //}
        //[BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        //public ActionResult Delete(string stringId)
        //{
        //    try
        //    {
        //        criteriaSV.Delete(stringId);
        //        X.GetCmp<Store>("stMnCriteria").Reload();
        //        Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
        //        return this.Direct();
        //    }
        //    catch (Exception ex)
        //    {
        //        X.Msg.Show(new MessageBoxConfig
        //        {
        //            Buttons = MessageBox.Button.OK,
        //            Icon = MessageBox.Icon.ERROR,
        //            Title = "Thông báo",
        //            Message = "Tiêu chí đánh giá đã được sử dụng không được xóa!"
        //        });
        //        return this.Direct("Error");
        //    }
        //}
    }
}
