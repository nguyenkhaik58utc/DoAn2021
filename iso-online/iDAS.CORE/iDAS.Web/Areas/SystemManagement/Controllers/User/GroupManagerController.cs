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

namespace iDAS.Web.Areas.SystemManagement.Controllers
{
    public partial class UserManagerController : BaseController
    {
        private SystemGroupService service = new SystemGroupService();
        private Node createNote(SystemGroupItem group) {
            Node node = new Node();
            node.NodeID = group.ID.ToString();
            node.Expanded = true;
            node.Text = group.Name;
            node.Icon = Icon.House;
            node.CustomAttributes.Add(new ConfigItem { Name = "Description", Value = group.Description, Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsActive", Value = JSON.Serialize(group.IsActive), Mode = ParameterMode.Value });
            var leaf = service.GetGroupsByParentID(group.ID).Count() <= 0;
            if (leaf)
            {
                node.Leaf = leaf;
            }
            return node;
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
                    group = new SystemGroupItem()
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
        public ActionResult RemoteAppend(string parentId, string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text)) {
                    return this.RemoteTree(false);
                }
                var group = new SystemGroupItem()
                {
                    Name = text,
                    ParentID = Convert.ToInt32(parentId),
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
