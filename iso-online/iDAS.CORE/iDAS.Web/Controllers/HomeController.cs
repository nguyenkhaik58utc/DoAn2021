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
using System.Security.Cryptography;
using iDAS.Base;
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
        
        //[OutputCache(Location= OutputCacheLocation.Server, Duration = 3600, VaryByParam = "moduleCode")]
        public ActionResult LoadMenu(string moduleCode, bool expand = false)
        {
            var nodes = new NodeCollection(false);
            if (expand)
            {
                var functions = new SystemFunctionService().GetFunctionsMenu(moduleCode);
                foreach (var function in functions)
                {
                    if (!string.IsNullOrEmpty(function.ParentCode)) break;
                    nodes.Add(getNode(functions, function));
                }
            }
            return this.Content(nodes.ToJson());
        }

        //[OutputCache(Location = OutputCacheLocation.Server, Duration = 3600, VaryByParam = "none")]
        public ActionResult LoadMenuSystem(string containerId)
        {
            var modules = new SystemModuleService().GetModulesMenu();
            //if (modules.Count <= 0) 
            {
                var service = new SystemService();
                service.UpdateSystemAuto(User.ID);
                modules = new SystemModuleService().GetModulesMenu();
            }
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

        private Node getNode(List<FunctionMenuItem> functions, FunctionMenuItem function)
        {
            Node node = createNode(function);
            var childrens = getChildrens(functions, function);
            foreach (var children in childrens)
            {
                node.Children.Add(getNode(functions, children));
            }
            return node;
        }
        private Node createNode(FunctionMenuItem function)
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
        private List<FunctionMenuItem> getChildrens(List<FunctionMenuItem> functions, FunctionMenuItem function)
        {
            var childrens = (from f in functions where f.ParentCode == function.Code select f).ToList();
            return childrens;
        }
    }
}
