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

namespace iDAS.Web.Areas.User.Controllers
{
    [BaseSystem(Name = "Quản lý tổ chức", IsActive = true, IsShow = true, Position = 1, Icon = "Neighbourhood")]
    public class GroupController : BaseController
    {
        private DepartmentSV service = new DepartmentSV();
        private RoleSV roleService = new RoleSV();
        private Node createNote(DepartmentItem group) {
            Node node = new Node();
            node.NodeID = group.ID.ToString();
            node.Expanded = true;
            node.Text = group.Name.ToUpper();
            node.Icon = Icon.House;
            node.CustomAttributes.Add(new Ext.Net.ConfigItem { Name = "Description", Value = group.Description, Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new Ext.Net.ConfigItem { Name = "IsActive", Value = JSON.Serialize(group.IsActive), Mode = ParameterMode.Value });
            var leaf = service.GetGroupsByParentID(group.ID).Count() <= 0;
            if (leaf)
            {
                node.Leaf = leaf;
            }
            return node;
        }

        [BaseSystem(Name = "Xem Danh sách", IsActive = true, IsShow = true)]
        [SystemAuthorize]
        public ActionResult Index()
        {
            return View();
        }
        [BaseSystem(Name = "Xem Chi Tiết Quyền")]
        [SystemAuthorize]
        public ActionResult ShowDetailRole(int id)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "DetailRole", Model = roleService.GetByID(id) };
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
        [BaseSystem(Name = "Xem Chi Tiết Nhóm")]
        [SystemAuthorize]
        public ActionResult ShowDetailGroup(int id)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "DetailGroup", Model = service.GetByID(id) };
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
        [BaseSystem(Name = "Xem Chi Tiết Quyền Coppy")]
        [SystemAuthorize]
        public ActionResult ShowCoppyRole()
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "CoppyRole"};
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult LoadGroup(int? id =null) {
            if (id == null)
            {
                return null;
            }
            var group = service.GetByID(id.Value);
            return this.Store(group);
        }
        public ActionResult LoadGroups(string node)
        {
            NodeCollection nodes = new NodeCollection(false);
            if (node == "root")
            {
                var group = service.GetGroupsByParentID(0).FirstOrDefault();
                if (group == null)
                {
                    group = new DepartmentItem()
                    {
                        ParentID = 0,
                        Name = string.Empty,
                    };
                    int id;
                    service.Insert(group, User.ID, out id);
                    group.ID = id;
                }
                nodes.Add(createNote(group));
            }
            else
            {
                var id = Convert.ToInt32(node);
                var groups = service.GetGroupsByParentID(id);
                foreach (var group in groups)
                {
                    nodes.Add(createNote(group));
                }
            }

            return this.Content(nodes.ToJson());
        }
        [BaseSystem(Name = "Sửa")]
        [SystemAuthorize]
        public ActionResult RemoteEdit(string id, string field, string newValue, string oldValue)
        {
            try
            {
                var group = service.GetByID(Convert.ToInt32(id));
                switch (field)
                {
                    case "text": group.Name = newValue; break;
                    case "Description": group.Description = newValue; break;
                    case "IsActive": group.IsActive = Convert.ToBoolean(newValue); break;
                };
                service.Update(group, User.ID);
                return this.RemoteTree();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true);
                return this.Direct();
            }
        }
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize]
        public ActionResult RemoteRemove(string id, string parentId)
        {
            try
            {
                service.Delete(Convert.ToInt32(id));
                X.GetCmp<Store>("StoreGroups").Reload();
                return this.RemoteTree();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.DeleteError, error: true);
                return this.Direct();
            }
        }
        [BaseSystem(Name = "Thêm")]
        [SystemAuthorize]
        public ActionResult RemoteAppend(string parentId, string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text)) {
                    return this.RemoteTree(false);
                }
                var group = new DepartmentItem()
                {
                    Name = text,
                    ParentID = Convert.ToInt32(parentId)                   
                };
                int id;
                service.Insert(group, User.ID, out id);
                return this.RemoteTree(null, id.ToString(), new { text = group.Name, icon = Ultility.GetIconUrl(Icon.House.ToString()) });
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.InsertError, error: true);
                return this.Direct();
            }
        }
        [BaseSystem(Name = "Chuyển")]
        [SystemAuthorize]
        public ActionResult RemoteMove(string[] ids, string targetId, string[] parentIds, string point)
        {
            try
            {
                var group = service.GetByID(Convert.ToInt32(ids[0].ToString().Trim('[','\\','\"',']')));
                group.ParentID = Convert.ToInt32(targetId);
                service.Update(group, User.ID);
                return this.RemoteTree();
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true);
                return this.Direct();
            }
        }
    }
}
