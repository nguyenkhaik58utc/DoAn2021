using iDAS.Core;
using iDAS.Items;
using iDAS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Services;

namespace iDAS.Web.Areas.Task.Controllers
{
    public class CalendarCategoryController : BaseController
    {
        //
        // GET: /Task/CalendarCategory/
        private CalendarCategorySV calendarCategorySV = new CalendarCategorySV();
        public ActionResult FormAdd(int parentId)
        {
            try
            {
                var data = new CalendarCategoryItem()
                {
                    ParentID = parentId,
                };
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Create", Model = data };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public StoreResult GetNodes(string node, int dpId)
        {
            NodeCollection nodes = new NodeCollection(false);
            var cateId = node == "Root" ? 0 : System.Convert.ToInt32(node);
            var cates = calendarCategorySV.GetTreeCateCalendar(cateId, dpId);
            for (int i = 0; i < cates.Count; i++)
            {
                Node newNode = new Node();
                newNode.NodeID = cates[i].ID.ToString();
                newNode.Text = cates[i].Name;
                newNode.Icon = cates[i].Leaf ? Icon.Calendar : Icon.Folder;
                newNode.Leaf = cates[i].Leaf;
                nodes.Add(newNode);
            }

            return this.Store(nodes);
        }
        public ActionResult RemoteEdit(string id, string field, string newValue, string oldValue, int departmentId)
        {
            var obj = calendarCategorySV.Update(id, newValue, departmentId, User.ID);
            return this.RemoteTree(obj.Name);
        }
        public ActionResult RemoteRemove(string id, string parentId)
        {

            try
            {
                calendarCategorySV.Delete(id);
                return this.RemoteTree();
            }
            catch (Exception ex)
            {
                return this.Direct();
            }
        }

        public ActionResult RemoteAppend(string parentId, string text, int departmentId)
        {
            if(parentId=="Root"||parentId=="root")
            {
                parentId = "0";
            }
            var obj =  calendarCategorySV.Insert(parentId, text,departmentId, User.ID);
            return this.RemoteTree(null, obj.ID.ToString(), new { text = obj.Name });
        }

        public ActionResult RemoteInsert(int index, string parentId, string text, int departmentId)
        {
            var obj = calendarCategorySV.Insert(parentId, text, departmentId, User.ID);
            return this.RemoteTree(null, obj.ID.ToString(), new { text = obj.Name });
        }

        public ActionResult RemoteMove(string[] ids, string targetId, string[] parentIds, string point, int departmentId)
        {
           calendarCategorySV.Move(ids, targetId, departmentId, User.ID);
            return this.RemoteTree();
        }
        
    }
}
