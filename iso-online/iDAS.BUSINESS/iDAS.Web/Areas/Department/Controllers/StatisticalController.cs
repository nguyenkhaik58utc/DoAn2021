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
using iDAS.Web.ExtExtend;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Department.Controllers
{
    [BaseSystem(Name = "Thống kê", Icon = "ChartBar", IsActive = true, IsShow = true, Position = 6)]
    [SystemAuthorize(CheckAuthorize = false)]
    public partial class StatisticalController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadMenuSystem()
        {
            var modules = new BusinessModuleSV().GetModulesMenu();
            return this.Store(modules);
        }
        public ActionResult LoadFunction(string moduleCode)
        {
            var nodes = new NodeCollection(false);
            var functions = new BusinessFunctionSV().GetFunctionsMenu(moduleCode);
            foreach (var function in functions)
            {
                if (!string.IsNullOrEmpty(function.ParentCode)) break;
                nodes.Add(getNode(functions, function));
            }
            return this.Content(nodes.ToJson());
        }
        private Node getNode(List<BusinessMenuFunctionItem> functions, BusinessMenuFunctionItem function)
        {
            Node node = createNode(function);
            var childrens = getChildrens(functions, function);
            foreach (var children in childrens)
            {
                node.Children.Add(getNode(functions, children));
            }
            return node;
        }
        private Node createNode(BusinessMenuFunctionItem function)
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
        private List<BusinessMenuFunctionItem> getChildrens(List<BusinessMenuFunctionItem> functions, BusinessMenuFunctionItem function)
        {
            var childrens = (from f in functions where f.ParentCode == function.Code select f).ToList();
            return childrens;
        }
        public ActionResult LoadEmployeePermission(StoreRequestParameters parameters, string moduleCode, string functionCode)
        {
            int totalCount = 0;
            var result = new List<HumanEmployeeItem>();
            result = new HumanEmployeeSV().GetEmployeeByFunctionCode(parameters.Page, parameters.Limit, out totalCount, moduleCode,functionCode);
            foreach (HumanEmployeeItem emp in result)
            {
                string strChucDanh = "";
                foreach (HumanGroupPositionItem hgpI in emp.lstHumanGrPos)
                {
                    strChucDanh += hgpI.Name + " -- " + hgpI.GroupName + "</br>";
                }
                emp.ChucDanh = strChucDanh;
            }

            return this.Store(result, totalCount);
        }
    }
}
