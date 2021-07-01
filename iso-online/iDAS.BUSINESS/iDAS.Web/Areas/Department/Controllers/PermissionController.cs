using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Department.Controllers
{
    [BaseSystem(Name = "Phân quyền tổ chức", IsActive = true, IsShow = true, Position = 2, Icon = "HouseKey")]
    [SystemAuthorize(CheckAuthorize = false)]
    public partial class PermissionController : BaseController
    {
        private PermissionSV permissionSV = new PermissionSV();
        private Node createNodeFunction(BusinessFunctionItem function)
        {
            Node node = new Node()
            {
                NodeID = function.ModuleCode + "," + function.Code,
                Text = function.Name,
                Icon = Ultility.ConvertToIcon(function.Icon),
                HrefTarget = string.Empty,
                Leaf = function.IsGroup == false,
            };
            node.CustomAttributes.Add(new ConfigItem { Name = "IsPermission", Value = JSON.Serialize(function.IsPermission), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsGroup", Value = JSON.Serialize(function.IsGroup), Mode = ParameterMode.Value });
            return node;
        }
        private Node createNodeModule(BusinessModuleItem module)
        {
            Node node = new Node()
            {
                NodeID = module.Code,
                Text = module.Name.ToUpper(),
                Icon = Ultility.ConvertToIcon(module.Icon),
                HrefTarget = string.Empty,
                Leaf = false
            };
            node.CustomAttributes.Add(new ConfigItem { Name = "IsPermission", Value = JSON.Serialize(module.IsPermission), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsGroup", Value = JSON.Serialize(false), Mode = ParameterMode.Value });
            return node;
        }

        [SystemAuthorize(CheckAuthorize = true, FunctionCode = "Department", ActionCode ="Permission")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadModuleFunctions(string node = "root", int roleId = 0, bool isGroup = false)
        {
            NodeCollection nodes = new NodeCollection();
            if (node == "root")
            {
                var modules = permissionSV.GetPermissionModules(roleId);
                foreach (var module in modules)
                {
                    nodes.Add(createNodeModule(module));
                }
            }
            else
            {
                var functions = permissionSV.GetPermissionFunctions(node, roleId, isGroup);
                foreach (var function in functions)
                {
                    nodes.Add(createNodeFunction(function));
                }
            }
            return this.Content(nodes.ToJson());
        }
        public ActionResult LoadData(string code, int departmentId, bool isModule = false, bool isGroup = false, bool isFunction = false)
        {
            var data = permissionSV.GetPermissions(code, departmentId, isModule, isGroup, isFunction);
            return this.Store(data);
        }
       
        public ActionResult Config(string code, int departmentId, bool isModule = false, bool isGroup = false, bool isFunction = false)
        {
            var grid = X.GetCmp<GridPanel>("PermissionPanel");
            var store = X.GetCmp<Store>("StorePermissions");

            var departmentColumn = Html.X().Column().DataIndex("DepartmentName").Text("Phòng ban").Sortable(false).Hidden(true).Flex(1);
            var roleIDColumn = Html.X().Column().Hidden(true).DataIndex("RoleID");
            var roleNameColumn = Html.X().Column().Locked(true).Text("Chức danh").Sortable(false).Width(260).MenuDisabled(true).Align(Alignment.Center).DataIndex("RoleName");

            var groupColumn1 = Html.X().Column().Text("Module").MenuDisabled(true);

            ModelField field1 = new ModelField("RoleID", ModelFieldType.Int);
            ModelField field2 = new ModelField("RoleName", ModelFieldType.String);
            ModelField field3 = new ModelField("DepartmentName", ModelFieldType.String);
            store.AddField(field1);
            store.AddField(field2);
            store.AddField(field3);

            var functions = permissionSV.GetFunctions(code, isModule, isGroup, isFunction);
            var moduleCode = code.Split(',')[0];
            foreach (var function in functions)
            {
                var groupColumn2 = Html.X().Column().MenuDisabled(true).Text("Chức năng");
                var actions = permissionSV.GetActions(code, function);
                for (int i = 0; i < actions.Count; i++)
                {
                    var dataIndex = moduleCode + "/" + function + "/" + actions[i].Code;
                    CheckColumn actionColumn = new CheckColumn();
                    actionColumn.Text = actions[i].Name;
                    actionColumn.Width = actions[i].Name.Length * 6 + 30;
                    actionColumn.DataIndex = dataIndex;
                    actionColumn.Editable = true;
                    actionColumn.Sortable = false;
                    actionColumn.Resizable = false;
                    actionColumn.Filterable = false;
                    actionColumn.MenuDisabled = true;
                    actionColumn.Listeners.CheckChange.Handler = "setPermission(item.dataIndex, record.data.RoleID, record.get(item.dataIndex));";
                    groupColumn2.Columns(actionColumn);
                    ModelField field = new ModelField(dataIndex, ModelFieldType.Boolean);
                    store.AddField(field);
                }
                if (actions.Count > 0)
                {
                    groupColumn1.Text(actions[0].ModuleName.ToUpper());
                    groupColumn2.Text(actions[0].FunctionName);
                    groupColumn1.Columns(groupColumn2);
                }
            }

            List<Column> columns = new List<Column>();
            columns.Add(departmentColumn.ToComponent());
            columns.Add(roleIDColumn.ToComponent());
            columns.Add(roleNameColumn.ToComponent());
            columns.Add(groupColumn1);
            grid.Reconfigure(columns);
            store.Reload();

            return this.Direct();
        }
        public ActionResult SetPermission(string code, int roleId, bool isPermission)
        {
            try
            {
                permissionSV.SetPermissionRole(code, roleId, isPermission);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct(true);
        }
        public ActionResult SetPermissionAll(string code, int roleId, bool isPermission, bool isModule = false, bool isGroup = false, bool isFunction = false)
        {
            try
            {
                permissionSV.SetPermissionRole(code, roleId, isPermission, isModule, isGroup, isFunction);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct(true);
        }
    }
}
