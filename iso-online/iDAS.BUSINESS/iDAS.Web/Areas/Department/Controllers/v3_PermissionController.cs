using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Services;
using iDAS.Utilities;
using iDAS.Web.Areas.Department.Models;
using iDAS.Web.Areas.Department.Models.TitleMenuRoleV3;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using iDAS.Web.API.DepartmentV3;
using Ext.Net.Utilities;

namespace iDAS.Web.Areas.Department.Controllers
{
    [BaseSystem(Name = "Phân quyền tổ chức_v3", IsActive = true, IsShow = true, Position = 2, Icon = "HouseKey")]
    [SystemAuthorize(CheckAuthorize = false)]
    public partial class v3_PermissionController : BaseController
    {
        private TitleMenuRoleV3API departmentTitleAPI = new TitleMenuRoleV3API();

        private string baseUrl = Ultilities.APIServer;

        private v3_PermissionSV permissionSV = new v3_PermissionSV();

        [SystemAuthorize(CheckAuthorize = true, FunctionCode = "Department", ActionCode ="Permission")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadData(int departmentId)
        {
            try
            {
                var data = new List<object>();
                List<DepartmentTitleDTO> tmpRsl = new List<DepartmentTitleDTO>();

                tmpRsl = departmentTitleAPI.GetAllTitlesByDepartmentID(departmentId);

                foreach (var item in tmpRsl)
                {
                    var dic = new Dictionary<string, object>();
                    dic.Add("id", item.ID);
                    dic.Add("titleID", item.TitleID);
                    dic.Add("titleName", item.TitleName);
                    dic.Add("departmentID", item.DepartmentID);
                    data.Add(dic);
                }

                return this.Store(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        // HungNM. Get data for combobox Department.
        public ActionResult LoadAllCbbDepartment(StoreRequestParameters parameter)
        {
            try
            {
                var data = new List<object>();
                //List<v3PermissionModel> tmpRsl = new List<v3PermissionModel>();
                List<HumanDepartmentModel> tmpRsl = new List<HumanDepartmentModel>();

                string base64String = Ultilities.GetTokenString();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);

                    RequestModelHumanDepartment paramRequestModelHumanDepartment = new RequestModelHumanDepartment() { pageSize = 0, pageNumber = 0 };
                    object param = new { data = paramRequestModelHumanDepartment, signature = base64String };
                    var uri = "api/HumanDepartmentAPI/GetAll";
                    var content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, Ultilities.ApplicationJson);
                    HttpResponseMessage result = client.PostAsync(uri, content).Result;
                    var rs = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    //Console.WriteLine(rs);
                    var model = JsonConvert.DeserializeObject<ResponseModelHumanDepartment<HumanDepartmentModel>>(rs);
                    tmpRsl = model.Data;
                }

                //tmpRsl = api.GetAllTitlesByDepartmentID(departmentId);

                foreach (var item in tmpRsl)
                {
                    var dic = new Dictionary<string, object>();
                    dic.Add("ID", item.ID);
                    dic.Add("Name", item.Name);
                    data.Add(dic);
                }

                return this.Store(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // HungNM. Get data for combobox Title.
        public ActionResult LoadCbbSelectTitle(StoreRequestParameters parameter, int id)
        {
            try
            {
                var data = new List<object>();
                List<DepartmentTitleDTO> tmpRsl = new List<DepartmentTitleDTO>();

                tmpRsl = departmentTitleAPI.GetAllTitlesByDepartmentID(id);

                if (tmpRsl.Count > 0)
                {
                    foreach (var item in tmpRsl)
                    {
                        var dic = new Dictionary<string, object>();
                        dic.Add("ID", item.TitleID);
                        dic.Add("Name", item.TitleName);
                        data.Add(dic);
                    }
                }
                else
                {
                    var dic = new Dictionary<string, object>();
                    dic.Add("ID", 0);
                    dic.Add("Name", "");
                    data.Add(dic);
                }

                return this.Store(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult PermissionTitleNew1(int DepartmentID, int TitleID)
        {
            var data = new DepartmentTitle();
            data.DepartmentID = DepartmentID;
            data.TitleID = TitleID;

            return new Ext.Net.MVC.PartialViewResult { Model = data };
        }

        public ActionResult GetTitlePermission(StoreRequestParameters parameter, int V3TitleId, int DepartmentID, string node = "root")
        {
            try
            {
                // Lay ra quyen menu tu chuc danh va phong ban
                List<V3DepartmentTitleMenuDTO> tmpRsl = new List<V3DepartmentTitleMenuDTO>();
                tmpRsl = departmentTitleAPI.GetMenuRoleByDepartmentIdAndTitleId(DepartmentID, V3TitleId);

                var data = departmentTitleAPI.GetTreeRollMenuV3API().Result;
                // Check isPermission = true for menu.
                foreach (var i in tmpRsl)
                {
                    data.Where(w => w.ID == i.MenuID).ToList().ForEach(s => s.IsPermission = true);
                }
                return this.Store(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateTitleNew(DepartmentTitle v3OrgDepartmentTitleItem, string dataMenuRole)
        {
            bool success = true;
            if (dataMenuRole != null && dataMenuRole.Length > 2)
            {
                var dataUpdated = JSON.Deserialize<Dictionary<string, object>>(dataMenuRole ?? string.Empty)["Updated"];
                var lstUpdate = ((Newtonsoft.Json.Linq.JArray)dataUpdated);

                string lstAdd = "";
                string lstDelete = "";

                for (int j = 0; j < lstUpdate.Count; j++)
                {
                    if (System.Convert.ToBoolean(lstUpdate[j]["IsPermission"]))
                    {
                        lstAdd += System.Convert.ToInt32(lstUpdate[j]["ID"]) + ",";
                    }
                    else
                    {
                        lstDelete += System.Convert.ToInt32(lstUpdate[j]["ID"]) + ",";
                    }
                }

                CRUDDepartmentTitleMenuV3 crudDepartmentTitleMenuV3 = new CRUDDepartmentTitleMenuV3();
                crudDepartmentTitleMenuV3.DepartmentID = v3OrgDepartmentTitleItem.DepartmentID;
                crudDepartmentTitleMenuV3.TitleID = v3OrgDepartmentTitleItem.TitleID;
                crudDepartmentTitleMenuV3.lstAdd = lstAdd;
                crudDepartmentTitleMenuV3.lstDelete = lstDelete;

                // Luu cac quyen menu theo chuc danh vua thay doi.
                departmentTitleAPI.UpdateMenuRoleDepartmentTitle(crudDepartmentTitleMenuV3);
                X.GetCmp<Store>("stTitle").Reload();
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            return this.Direct(success);
        }

        public ActionResult FormCopy(int? DepartmentID, int? V3TitleId)
        {
            var model = new DepartmentTitle(); // Thay bang bang V3OrgDepartmentTitles
            model.DepartmentID = DepartmentID.HasValue ? System.Convert.ToInt32(DepartmentID) : 0;
            model.TitleID = V3TitleId.HasValue ? System.Convert.ToInt32(V3TitleId) : 0;
            ViewData["DepartmentID"] = model.DepartmentID;
            ViewData["TitleID"] = model.TitleID;
            return new Ext.Net.MVC.PartialViewResult { Model = model, ViewData = ViewData };
        }

        [HttpPost]
        public ActionResult SaveCopy(DepartmentTitle SourceV3OrgDepartmentTitleItem, int? DestinationDepartmentID, int? DestinationV3TitleId)
        {
            bool success = true;
            CopyMenuTitleRoleV3 item = new CopyMenuTitleRoleV3();
            item.srcDepartmentID = SourceV3OrgDepartmentTitleItem.DepartmentID;
            item.srcTitleID = SourceV3OrgDepartmentTitleItem.TitleID;
            item.desDepartmentID = DestinationDepartmentID.HasValue ? System.Convert.ToInt32(DestinationDepartmentID) : 0;
            item.desTitleID = DestinationV3TitleId.HasValue ? System.Convert.ToInt32(DestinationV3TitleId) : 0;

            departmentTitleAPI.CopyMenuRoleDepartmentTitle(item);
            Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            return this.Direct(success);
        }
        // End.
    }

    // HungNM. Add model to get list return from API.
    public class RequestModel
    {
        public int departmentID { get; set; }
    }
    public class RequestModelHumanDepartment
    {
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
    }

    public class ResponseModelHumanDepartment<T> where T : class
    {
        public string messageCode { get; set; }
        public string message { get; set; }
        public List<T> Data { get; set; }
    }
}
