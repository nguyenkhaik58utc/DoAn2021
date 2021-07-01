using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Services;
using iDAS.Items;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Quality.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Danh mục tiêu chí đánh giá", IsActive = true, Icon = "PageGear", IsShow = true, Position = 1, Parent = GroupMenuPlanController.Code)]
    public class CriteriaCategoryController : BaseController
    {
        private QualityCriteriaCategorySV criteriaCategorySV = new QualityCriteriaCategorySV();
        #region Danh sách tiêu chí
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem danh sách")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData()
        {
            List<QualityCriteriaCategoryItem> cate = new List<QualityCriteriaCategoryItem>();
            cate = criteriaCategorySV.GetData(User.EmployeeID, User.Administrator);
            return this.Store(cate);
        }
        public StoreResult GetData(string node)
        {
            var nodes = new NodeCollection();
            var lsDataNode = criteriaCategorySV.GetTreeData(node);
            foreach (var dataNode in lsDataNode)
            {
                Node nodeItem = new Node();
                nodeItem.NodeID = dataNode.ID.ToString();
                nodeItem.Expanded = false;
                nodeItem.Text = dataNode.Name.ToUpper();
                nodeItem.Icon = Icon.Folder;
                nodeItem.Leaf = dataNode.Leaf;
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = dataNode.ParentID.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Name", Value = dataNode.Name.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "ID", Value = JSON.Serialize(dataNode.ID), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsUse", Value = JSON.Serialize(dataNode.IsUse), Mode = ParameterMode.Value });
                nodes.Add(nodeItem);
            }
            return this.Store(nodes);
        }
        #endregion

        #region Thêm mới tiêu chí đánh giá
        public ActionResult FormAdd(int parentId, bool isTerm = false, int iSOStandardId = 0, bool hiddenAuditor = false)
        {
            try
            {
                var data = new QualityCriteriaCategoryItem()
                {
                    ParentID = parentId,
                    IsTerm = isTerm,
                    ISOStandartID = iSOStandardId
                };
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Create", Model = data, ViewData = new ViewDataDictionary { { "hiddenAuditor", hiddenAuditor } } };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thêm mới")]
        public ActionResult Insert(QualityCriteriaCategoryItem objNew)
        {
            try
            {
                objNew.CreateBy = User.ID;
                if (criteriaCategorySV.Insert(objNew))
                {
                    X.GetCmp<FormPanel>("frCriteriaCategory").Reset();
                    X.GetCmp<Store>("stCriteriaCategory").Reload();
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Thông báo",
                        Message = "Tên bộ tiêu chí đánh giá đã tồn tại!"
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

        #region Cập nhật tiêu chí đánh giá
        public ActionResult ShowUpdate(int id, bool hiddenAuditor = false)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Edit", Model = criteriaCategorySV.GetById(id), ViewData = new ViewDataDictionary { { "hiddenAuditor", hiddenAuditor } } };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Sửa")]
        public ActionResult Update(QualityCriteriaCategoryItem objEdit)
        {
            try
            {
                if (criteriaCategorySV.Update(objEdit, User.ID))
                {
                    X.GetCmp<FormPanel>("frCriteriaCategory").Reset();
                    X.GetCmp<Store>("stCriteriaCategory").Reload();
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

        #region Xem chi tiết tiêu chí đánh giá
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem chi tiết")]
        public ActionResult ShowDetail(int id, bool hiddenAuditor = false)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = criteriaCategorySV.GetById(id), ViewData = new ViewDataDictionary { { "hiddenAuditor", hiddenAuditor } } };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region Xóa tiêu chí đánh giá
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xóa")]
        public ActionResult Delete(int id)
        {
            try
            {
                if (criteriaCategorySV.Delete(id))
                {
                    X.GetCmp<Store>("stCriteriaCategory").Reload();
                    Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                    return this.Direct();
                }
            }
            catch { }
            X.Msg.Show(new MessageBoxConfig
            {
                Buttons = MessageBox.Button.OK,
                Icon = MessageBox.Icon.ERROR,
                Title = "Thông báo",
                Message = "Bộ tiêu chí đánh giá đã được sử dụng không được xóa!"
            });
            return this.Direct("Error");
        }
        #endregion

        #region Form tiêu chí dùng chung
        public ActionResult WindowCriteria()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "WindowCriteria", ViewData = ViewData };
        }
        public StoreResult GetDataUsed(string node)
        {
            var nodes = new NodeCollection();
            var lsDataNode = criteriaCategorySV.GetTreeData(node);
            foreach (var dataNode in lsDataNode)
            {
                Node nodeItem = new Node();
                nodeItem.NodeID = dataNode.ID.ToString();
                nodeItem.Expanded = false;
                nodeItem.Text = dataNode.Name.ToUpper();
                nodeItem.Icon = Icon.Folder;
                nodeItem.Leaf = dataNode.Leaf;
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = dataNode.ParentID.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Name", Value = dataNode.Name.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "ID", Value = JSON.Serialize(dataNode.ID), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "IsUse", Value = JSON.Serialize(dataNode.IsUse), Mode = ParameterMode.Value });
                nodes.Add(nodeItem);
            }
            return this.Store(nodes);
        }
        #endregion
    }
}
