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
namespace iDAS.Web.Areas.Equipment.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Nhóm Thiết Bị Sản Xuất", IsActive = true, IsShow = true, Position = 1, Icon = "Group", Parent = GroupCategoryController.Code)]
    public class EquipmentProductionCategoryController : BaseController
    {
        private EquipmentProductionCategorySV CategoryServices = new EquipmentProductionCategorySV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            CategoryServices.UpdateRoot(User.ID);
            return PartialView();
        }
        public ActionResult LoadData(string node)
        {
            NodeCollection nodes = new NodeCollection();
            int nodeId = node == "root" ? 0 : System.Convert.ToInt32(node);
            var lsDataNodes = CategoryServices.GetTreeData(nodeId);
            foreach (var dataNode in lsDataNodes)
            {
                Node nodeItem = new Node();
                nodeItem.NodeID = dataNode.ID.ToString();
                nodeItem.Text = dataNode.Name;
                nodeItem.Icon = dataNode.ParentID == 0 ? Icon.House : Icon.Door;
                nodeItem.Leaf = dataNode.Leaf;
                if (!dataNode.Leaf) nodeItem.Expanded = true;
                else nodeItem.Expanded = false;
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = dataNode.ParentID.ToString(), Mode = ParameterMode.Value });
                nodes.Add(nodeItem);
            }
            return this.Content(nodes.ToJson());
        }

        #region Cập nhật nhóm thiết bị sản xuất
        [BaseSystem(Name = "Thêm mới và sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int parentId, int categoryId = 0)
        {
            var data = new EquipmentProductionCategoryItem();
            if (categoryId != 0)
            {
                data = CategoryServices.GetById(categoryId);
                data.ActionForm = Form.Edit;
            }
            else
            {
                data.ParentID = parentId;
                data.ActionForm = Form.Add;
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        public ActionResult Update(EquipmentProductionCategoryItem data)
        {
            int id = 0;
            try
            {
                if (data.ID != 0)
                {
                    if (CategoryServices.checkNameExist(data.ParentID, data.Name))
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.ERROR,
                            Title = "Thông báo",
                            Message = "Tên nhóm đã tồn tại!"
                        });
                    }
                    else
                    {
                        CategoryServices.Update(data, User.ID);
                        Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                        id = data.ID;
                    }
                }
                else
                {
                    if (CategoryServices.checkNameExist(data.ParentID, data.Name))
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.ERROR,
                            Title = "Thông báo",
                            Message = "Tên nhóm đã tồn tại!"
                        });
                    }
                    else
                    {
                        id = CategoryServices.Insert(data, User.ID);
                        Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    }
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct(id);
        }
        #endregion

        #region Xóa nhóm thiết bị sản xuất
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id = 0)
        {
            bool success = false;
            try
            {
                if (id != 0)
                {
                    if (CategoryServices.Delete(id))
                    {
                        Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                        success = true;
                    }
                    else
                    {
                        Ultility.CreateMessageBox(title: RequestMessage.Notify, message: RequestMessage.DataUsing, icon: MessageBox.Icon.ERROR, button: MessageBox.Button.OK);
                    }

                }
            }
            catch
            {
                Ultility.CreateMessageBox(title: RequestMessage.Notify, message: RequestMessage.DataUsing, icon: MessageBox.Icon.ERROR, button: MessageBox.Button.OK);
            }
            return this.Direct(success);
        }
        #endregion

        #region Xem chi tiết nhóm thiết bị sản xuất
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DetailForm(int id = 0)
        {
            var data = new EquipmentProductionCategoryItem();
            if (id != 0)
            {
                data = CategoryServices.GetById(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        #region Dùng chung
        public ActionResult List()
        {
            return PartialView();
        }
        #endregion
    }
}
