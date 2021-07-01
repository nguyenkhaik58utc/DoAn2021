using Ext.Net;
using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net.MVC;

namespace iDAS.Web.Areas.Web.Controllers
{
    [BaseSystem(Name = "Sơ đồ website", IsActive = true, IsShow = true, Position = 1, Icon = "SitemapColor")]
    [SystemAuthorize(CheckAuthorize = false)] 
    public class SitemapController : BaseController
    {
        private SitemapSV sitemapSV = new SitemapSV();
        private Node createNodeSitemap(SitemapItem sitemap)
        {
            Node node = new Node();
            node.NodeID = sitemap.ID.ToString();
            node.Text = sitemap.Text;
            node.Icon = sitemap.ID == 0 ? Icon.World : Icon.Page;
            node.CustomAttributes.Add(new ConfigItem { Name = "Tooltip", Value = sitemap.Tooltip, Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsActive", Value = JSON.Serialize(sitemap.IsActive), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsMenuTop", Value = JSON.Serialize(sitemap.IsMenuTop), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsMenuRight", Value = JSON.Serialize(sitemap.IsMenuRight), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsPageDynamic", Value = JSON.Serialize(sitemap.IsPageDynamic), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsSelected", Value = JSON.Serialize(sitemap.IsSelected), Mode = ParameterMode.Value });
            node.Leaf = !sitemap.IsParent;
            node.Expanded = true;
            return node;
        }

        #region View
        [BaseSystem(Name = "Xem")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(string node)
        {
            try
            {
                NodeCollection nodes = new NodeCollection();
                if (node == "root")
                {
                    SitemapItem sitemap = new SitemapItem();
                    sitemap.Text = "Sơ đồ website";
                    sitemap.ID = 0;
                    sitemap.IsActive = true;
                    sitemap.IsParent = true;
                    nodes.Add(createNodeSitemap(sitemap));
                }
                else
                {
                    var sitemapID = System.Convert.ToInt32(node);
                    var sitemamps = sitemapSV.GetSitemaps(sitemapID);
                    foreach (var sitemap in sitemamps)
                    {
                        nodes.Add(createNodeSitemap(sitemap));
                    }
                }

                return this.Content(nodes.ToJson());
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        #endregion
        #region Create
        [BaseSystem(Name = "Thêm")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Create(int sitemapId = 0)
        {
            try
            {
                ViewData["SitemapID"] = sitemapId;
                return new Ext.Net.MVC.PartialViewResult { ViewData = ViewData };
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(SitemapItem sitemap)
        {
            object result = null;
            try
            {
                if (ModelState.IsValid)
                {
                    sitemapSV.Insert(sitemap);
                    Ultilities.ShowMessageRequest(RequestStatus.CreateSuccess);
                    var node = new
                    { 
                        id = sitemap.ID, 
                        text = sitemap.Text, 
                        Tooltip = sitemap.Tooltip, 
                        IsPageDynamic = sitemap.IsPageDynamic,
                        IsActive = sitemap.IsActive,
                        IsMenuTop = sitemap.IsMenuTop,
                        IsMenuRight = sitemap.IsMenuRight,
                        leaf = !sitemap.IsParent,
                        iconCls = "icon-page",
                    };
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(node);
                }
                else
                {
                    Ultilities.ShowMessageRequest(RequestStatus.ValidError);
                }
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct(result);
        }
        #endregion
        #region Update
        [BaseSystem(Name = "Sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Update(int sitemapId = 0)
        {
            try
            {
                var data = sitemapSV.GetSitemap(sitemapId);
                return new Ext.Net.MVC.PartialViewResult { Model = data };
            }
            catch
            {
                Ultilities .ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(SitemapItem sitemap)
        {
            var success = false;
            try
            {
                sitemapSV.Update(sitemap);
                Ultilities.ShowMessageRequest(RequestStatus.UpdateSuccess);
                success = true;
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
                success = false;
            }
            return this.FormPanel(success);
        }
        #endregion
        #region Delete
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete()
        {
            return this.Direct(true);
        }
        [HttpPost]
        public ActionResult Delete(int sitemapId = 0)
        {
            try
            {
                sitemapSV.Delete(sitemapId);
                Ultilities.ShowMessageRequest(RequestStatus.DeleteSuccess);
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.DeleteError);
            }
            return this.Direct();
        }
        #endregion
        #region Detail
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Detail(int sitemapId = 0)
        {
            try
            {
                var data = sitemapSV.GetSitemap(sitemapId);
                return new Ext.Net.MVC.PartialViewResult { Model = data };
            }
            catch
            {
                Ultilities.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        #endregion
        #region Window
        public ActionResult Window(string fn = "", string sitemapId = "")
        {
            ViewData["Fn"] = fn;
            ViewData["SitemapID"] = sitemapId;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewData = ViewData,
            };
        }
        #endregion
    }
}
