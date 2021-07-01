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
namespace iDAS.Web.Areas.Department.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Bảng tin nội bộ", IsActive = true, Icon = "LayoutHeader", IsShow = true, Position = 4)]
    public class BroadController : BaseController
    {
        private DepartmentBroadCategorySV categorySV = new DepartmentBroadCategorySV();
        private DepartmentBroadSV BroadSV = new DepartmentBroadSV();
        #region Danh sách tin nội bộ
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem danh sách")]
        public ActionResult Index()
        {
            return View();
        }
        public StoreResult LoadCategory(string node)
        {
            var nodes = new NodeCollection();
            var lsDataNode = categorySV.GetTreeData(node);
            foreach (var dataNode in lsDataNode)
            {
                Node nodeItem = new Node();
                nodeItem.NodeID = dataNode.ID.ToString();
                nodeItem.Text = dataNode.Name.ToUpper();
                nodeItem.Icon = Icon.Folder;
                nodeItem.Leaf = dataNode.Leaf;
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = dataNode.ParentID.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Name", Value = dataNode.Name.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "ID", Value = JSON.Serialize(dataNode.ID), Mode = ParameterMode.Value });
                nodes.Add(nodeItem);
            }
            return this.Store(nodes);
        }
        public ActionResult LoadData(StoreRequestParameters parameters, int cateId = 0)
        {
            List<DepartmentBroadItem> lstData;
            int total;
            lstData = BroadSV.GetByCategory(parameters.Page, parameters.Limit, out total, cateId);
            return this.Store(new Paging<DepartmentBroadItem>(lstData, total));
        }
        #endregion

        #region nhóm tin

        #region Thêm mới
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thêm nhóm tin")]
        [HttpGet]
        public ActionResult InsertCategory(int parentId)
        {
            try
            {
                var data = new DepartmentBroadCategoryItem()
                {
                    ParentID = parentId,
                };
                return new Ext.Net.MVC.PartialViewResult { ViewName = "InsertCategory", Model = data };
            }
            catch
            {
                return this.Direct();
            }
        }
        [HttpPost]
        public ActionResult InsertCategory(DepartmentBroadCategoryItem objNew)
        {
            try
            {
                if (categorySV.Insert(objNew, User.ID))
                {
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Thông báo",
                        Message = "Tên nhóm tin đã tồn tại!"
                    });
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
            }
            return this.Direct();
        }
        #endregion

        #region Cập nhật
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Sửa nhóm tin")]
        [HttpGet]
        public ActionResult UpdateCategory(int id)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "UpdateCategory", Model = categorySV.GetById(id) };
            }
            catch
            {
                return this.Direct();
            }
        }
        [HttpPost]
        public ActionResult UpdateCategory(DepartmentBroadCategoryItem objEdit)
        {
            try
            {
                if (categorySV.Update(objEdit, User.ID))
                {
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
        #endregion

        #region Xem chi tiết
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem chi tiết nhóm tin")]
        public ActionResult DetailCategory(int id)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "DetailCategory", Model = categorySV.GetById(id) };
            }
            catch
            {
                return this.Direct();
            }
        }
        #endregion

        #region Xóa nhóm tin
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xóa")]
        public ActionResult DeleteCategory(int id)
        {
            try
            {
                if (categorySV.Delete(id))
                {
                    X.GetCmp<Store>("stBroadCategory").Reload();
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    return this.Direct();
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Thông báo",
                        Message = "nhóm tin đã được sử dụng không được xóa!"
                    });
                }
            }
            catch { }
            X.Msg.Show(new MessageBoxConfig
            {
                Buttons = MessageBox.Button.OK,
                Icon = MessageBox.Icon.ERROR,
                Title = "Thông báo",
                Message = "nhóm tin đã được sử dụng không được xóa!"
            });
            return this.Direct("Error");
        }
        #endregion

        #endregion

        #region tin nội bộ

        #region Thêm
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thêm")]
        [HttpGet]
        public ActionResult Insert(int cateId, string cateName)
        {
            var data = new DepartmentBroadItem()
            {
                CategoryID = cateId,
                CategoryName = cateName
            };
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Insert", Model = data };
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Insert(DepartmentBroadItem objNew)
        {
            try
            {
                if (BroadSV.Insert(objNew, User.ID))
                {
                    X.GetCmp<Store>("StoreTreeBroad").Reload();
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Thông báo",
                        Message = "Tên tin nội bộ  đã tồn tại!"
                    });
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError);
            }
            return this.Direct();
        }
        #endregion

        #region Sửa
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Sửa")]
        [HttpGet]
        public ActionResult Update(int id)
        {
            var data = BroadSV.GetById(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(DepartmentBroadItem objEdit)
        {
            try
            {
                if (BroadSV.Update(objEdit, User.ID))
                {
                    X.GetCmp<FormPanel>("frBroad").Reset();
                    X.GetCmp<Store>("StoreTreeBroad").Reload();
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
        #endregion

        #region Xóa
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xóa")]
        public ActionResult Delete(int id)
        {
            try
            {
                BroadSV.Delete(id);
                X.GetCmp<Store>("StoreTreeBroad").Reload();
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
                    Message = "tin nội bộ đã được sử dụng không được xóa!"
                });
                return this.Direct("Error");
            }
        }
        #endregion

        #region Xem
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem")]
        public ActionResult Detail(int id)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = BroadSV.GetById(id) };
            }
            catch
            {
                return this.Direct();
            }
        }
        #endregion

        #region Đọc tin bài
        public ActionResult Content(int id)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Content", Model = BroadSV.GetById(id) };
            }
            catch
            {
                return this.Direct();
            }
        }
        #endregion

        #endregion
    }
}
