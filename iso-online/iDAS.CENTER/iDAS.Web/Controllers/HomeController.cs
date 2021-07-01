using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using iDAS.Core;
using iDAS.Services;
using Ext.Net.MVC;
using Ext.Net;
using System.Threading;
using iDAS.Items;

namespace iDAS.Web.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    public class HomeController : BaseController
    {
        [OutputCache(Duration = 0, VaryByParam = "none")]
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Migration()
        {
            var result = BaseDbContext.Instance.MigrationDbContext<iDAS.Base.iDASCenterEntities>(BaseDatabase.GetDatabaseCenter());
            return this.Direct(result);
        }
        //[OutputCache(Location = OutputCacheLocation.Server, Duration = 3600, VaryByParam = "none")]
        public ActionResult LoadMenuSystem(string containerId)
        {
            var modules = new ModuleSV().GetModulesMenu(SystemConfig.Config.SystemCode);
            return new Ext.Net.MVC.PartialViewResult
            {
                RenderMode = RenderMode.AddTo,
                ContainerId = containerId,
                Model = modules,
                WrapByScriptTag = false,
            };
        }

        //[OutputCache(Location = OutputCacheLocation.Client, Duration = 3600, VaryByParam = "*")]
        public ActionResult LoadHeaderSystem(string containerId)
        {
            return new Ext.Net.MVC.PartialViewResult
            {
                RenderMode = RenderMode.AddTo,
                ContainerId = containerId,
                WrapByScriptTag = false,
            };
        }

        //[OutputCache(Location= OutputCacheLocation.Server, Duration = 3600, VaryByParam = "moduleCode")]
        public ActionResult LoadMenu(string moduleCode, bool expand = false)
        {
            var nodes = new NodeCollection(false);
            if (expand)
            {
                var functions = new FunctionSV().GetFunctionsMenu(SystemConfig.Config.SystemCode, moduleCode);
                foreach (var function in functions)
                {
                    if (!string.IsNullOrEmpty(function.ParentCode)) break;
                    nodes.Add(getNode(functions, function));
                }
            }
            return this.Content(nodes.ToJson());
        }

        private Node getNode(List<MenuFunctionItem> functions, MenuFunctionItem function)
        {
            Node node = createNode(function);
            var childrens = getChildrens(functions, function);
            foreach (var children in childrens)
            {
                node.Children.Add(getNode(functions, children));
            }
            return node;
        }
        private Node createNode(MenuFunctionItem function)
        {
            Node node = new Node()
            {
                NodeID = function.ModuleCode + function.Code,
                Text = function.Name,
                Icon = Ultility.ConvertToIcon(function.Icon),
                HrefTarget = function.IsGroup == false ? function.Url : string.Empty,
                Leaf = function.IsGroup == false,
            };
            return node;
        }
        private List<MenuFunctionItem> getChildrens(List<MenuFunctionItem> functions, MenuFunctionItem function)
        {
            var childrens = (from f in functions where f.ParentCode == function.Code select f).ToList();
            return childrens;
        }
    }
}
