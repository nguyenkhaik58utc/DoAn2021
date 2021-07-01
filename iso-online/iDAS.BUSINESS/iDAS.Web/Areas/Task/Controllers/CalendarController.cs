using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ext.Net;
using Ext.Net.MVC;
using System.Web.Mvc;
using iDAS.Services;
using iDAS.Utilities;
using iDAS.Items;

namespace iDAS.Web.Areas.Task.Controllers
{
    [BaseSystem(Name = "Thiết lập lịch", Icon = "Calendar", IsActive = true, IsShow = true, Position = 2, Parent = CalendarGroupController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class CalendarController : BaseController
    {
        //
        // GET: /Task/Calendars/
        private TaskCalendarSV calendarsService = new TaskCalendarSV();
        private CalendarOverrideSV calendarOverrideSV = new CalendarOverrideSV();
        private CalendarTimeOverrideSV calendarTimeOverrideSV = new CalendarTimeOverrideSV();
        private CalendarCategorySV calendarCategorySV = new CalendarCategorySV();
        private readonly string storeDayOverride = "stDateOverride";
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            var data = calendarsService.GetEventAll();
            string style = "";
            foreach (var item in data)
            {
            
                style += ".box-" + item.ID + "{background: " + item.Color + "}";
               
            }
            ViewBag.Style = style;
            ViewBag.Calendar = data;
            return View();
        }
        private Node createNodeTask(CalendarCategoryItem dataNode, int departmentId)
        {
            Node nodeItem = new Node();
            nodeItem.NodeID = dataNode.ID.ToString();
            if (dataNode.HumanDepartmentID != departmentId)
            {
                nodeItem.Cls = "clsUnView";
            }
            nodeItem.Text = !dataNode.Leaf ? dataNode.Name.ToUpper() : dataNode.Name;
            nodeItem.Icon = !dataNode.Leaf ? Icon.Folder : Icon.TagBlue;
            nodeItem.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = dataNode.ParentID.ToString(), Mode = ParameterMode.Value });
            nodeItem.Leaf = dataNode.Leaf;
            return nodeItem;
        }
        public ActionResult LoadTreeCalendar(string node, int dpId = 0)
        {
            try
            {
                NodeCollection nodes = new NodeCollection();
                var cateId = node == "root" ? 0 : System.Convert.ToInt32(node);
                var cates = calendarCategorySV.GetTreeCateCalendar(cateId, dpId);
                foreach (var cate in cates)
                {
                    nodes.Add(createNodeTask(cate, dpId));
                }
                return this.Content(nodes.ToJson());
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }      
        public static SelectedRowCollection InitiallySelectedRows
        {
            get
            {
                return new SelectedRowCollection()
                {
                    new SelectedRow(0)             
                };
            }
        }
        [BaseSystem(Name = "Thiết lập lịch cuối tuần", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult WeekendSetCalendar(int cateId)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewData = new ViewDataDictionary { { "CateID", cateId } } };
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult CheckExitsDate(int cateId, string date)
        {
            var check = calendarOverrideSV.CheckExitsDate(cateId, DateTime.Parse(date));
            if (!check)
            {
                calendarOverrideSV.Insert(cateId, "[Ngày thay đổi]", DateTime.Parse(date), User.ID);
            }
            X.GetCmp<Store>(storeDayOverride).Reload();
            return this.Direct(check);
        }
        [BaseSystem(Name = "Sửa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Update(int id, int cateId, string title, string date)
        {
            var check = calendarOverrideSV.CheckExitsDateEdit(id, cateId, DateTime.Parse(date));
            if (!check)
            {
                calendarOverrideSV.Update(id, title, DateTime.Parse(date), User.ID);
            }
            else
            {
                Ultility.CreateMessageBox("Thông báo", message: "Ngày đã tồn tại vui lòng chọn ngày thay đổi khác!", icon: MessageBox.Icon.INFO);
            }
            return this.Direct();
        }
        [BaseSystem(Name = "Thiết lập thời gian", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateDayOverride(int id)
        {
            try
            {
                var obj = calendarOverrideSV.GetByID(id);
                return new Ext.Net.MVC.PartialViewResult { Model = obj };
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(string stringId)
        {
            try
            {
                calendarOverrideSV.Delete(stringId);
                X.GetCmp<Store>(storeDayOverride).Reload();
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                return this.Direct();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
                return this.Direct("Error");
            }
        }
    }
}
