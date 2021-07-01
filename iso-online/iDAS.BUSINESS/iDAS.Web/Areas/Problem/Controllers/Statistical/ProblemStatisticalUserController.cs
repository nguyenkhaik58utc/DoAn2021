using Ext.Net;
using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using iDAS.Web.API;
using System;
using System.Web.Mvc;
using iDAS.Utilities;
using Ext.Net.MVC;
using iDAS.Web.Areas.Problem.Models;
using System.Data;
using System.Collections.Generic;
using ISO.API.Models;
using System.Web.UI;

namespace iDAS.Web.Areas.Problem.Controllers
{
    [BaseSystem(Name = "Thống kê theo cá nhân", Icon = "ChartBar", IsActive = true, IsShow = true, Position = 81, Parent = "ProblemEventStatistical")]
    public class ProblemStatisticalUserController : BaseController
    {
        private DepartmentSV departmentSV = new DepartmentSV();
        ProblemStatisticAPI api = new ProblemStatisticAPI();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StatisticTypeDetail(Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null, int type = 0, int userID = 0, int criticalLevelID = 0, int departmentID = 0)
        {
            ViewData["fromDate"] = fromDate;
            ViewData["toDate"] = toDate;
            ViewData["type"] = type;
            ViewData["userID"] = userID;
            ViewData["criticalLevelID"] = criticalLevelID;
            ViewData["departmentID"] = departmentID;

            return new Ext.Net.MVC.PartialViewResult() { ViewName = "ProblemStatisticalDetail1", ViewData = ViewData };
        }

        public ActionResult StatisticOneUserDetail(Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null, int type = 0, int criticalLevelID = 0, string strUserID = null)
        {
            ViewData["fromDate"] = fromDate;
            ViewData["toDate"] = toDate;
            ViewData["type"] = type;
            ViewData["userID"] = int.Parse(strUserID.Split('_')[1]);
            ViewData["criticalLevelID"] = criticalLevelID;
            ViewData["departmentID"] = 0;

            return new Ext.Net.MVC.PartialViewResult() { ViewName = "ProblemStatisticalDetail1", ViewData = ViewData };
        }

        public ActionResult GetStatisticDetailForUser(StoreRequestParameters parameters, string fromDate = null, string toDate = null, int type = 0, int userID = 0, int criticalLevelID = 0, int departmentID = 0)
        {
            DateTime fromDate1 = DateTime.Parse(fromDate);
            DateTime toDate1 = DateTime.Parse(toDate);
            ResponseModel<List<ProblemEventListDTO>> resp = new ResponseModel<List<ProblemEventListDTO>>();
            if(departmentID > 0)
                resp = api.GetStatisticForUserDetail(parameters.Page, parameters.Limit, fromDate1, toDate1, type, userID, criticalLevelID, departmentID);
            else
                resp = api.GetStatisticOneUserDetail(parameters.Page, parameters.Limit, fromDate1, toDate1, userID, criticalLevelID, type);

            int total = 0;
            if (resp.TotalRows != null)
                total = resp.TotalRows.Value;
            return this.Store(new Paging<ProblemEventListDTO>(resp.Data, total));
        }

        public ActionResult LoadData(string node, bool viewDeactive = false, bool viewCancel = false, bool viewMerge = false, bool viewDelete = false, string departmentIds = "")
        {
            try
            {
                NodeCollection nodes = new NodeCollection();
                var departmentID = node == "root" ? 0 : System.Convert.ToInt32(node);
                var departments = departmentSV.GetDepartments(departmentID, viewDeactive, viewCancel, viewMerge, viewDelete, departmentIds);
                foreach (var department in departments)
                {
                    nodes.Add(createNodeDepartment(department));
                }

                // get user
                if(departmentID > 0)
                {
                    var lstUser = api.GetUserByDepartment(departmentID).Result;
                    if(lstUser != null && lstUser.Count > 0)
                    {
                        for (int i = 0; i < lstUser.Count; i++)
                            nodes.Add(createNodeUser(lstUser[i], departmentID));
                    }
                }
                
                return this.Content(nodes.ToJson());
            }
            catch(Exception ex)
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        private Node createNodeDepartment(HumanDepartmentItem department)
        {
            Node node = new Node();
            var css = "DepartmentActive";
            if (department.IsActive == false)
            {
                css = "DepartmentDeactive";
            }
            if (department.IsMerge == true)
            {
                css = "DepartmentMerge";
            }
            if (department.IsCancel == true)
            {
                css = "DepartmentCancel";
            }
            if (department.IsDelete == true)
            {
                css = "DepartmentDelete";
            }
            //if (department.IsSelected)
            //    css = "ForEmployee";
            //else
            //    css = "NotAllow";
            node.NodeID = department.ID.ToString();
            node.Text = department.Name.ToUpper();
            node.Cls = css;
            node.Icon = department.ParentID == 0 ? Icon.House : Icon.Door;
            node.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = department.ParentID.ToString(), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "UpdateAt", Value = department.UpdateAtView, Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "EstablishedDate", Value = department.EstablishedDateView, Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsCancel", Value = JSON.Serialize(department.IsCancel), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsDelete", Value = JSON.Serialize(department.IsDelete), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsActive", Value = JSON.Serialize(department.IsActive), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsSelected", Value = JSON.Serialize(department.IsSelected), Mode = ParameterMode.Value });
            node.Leaf = false;
            return node;
        }

        private Node createNodeUser(HumanEmployeeDTO emp, int departmentID)
        {
            Node node = new Node();
            var css = "ForEmployee";

            node.NodeID = string.Format("user_{0}", emp.ID);
            node.Text = emp.Name;
            node.Cls = css;
            node.Icon = Icon.User;
            node.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = departmentID.ToString(), Mode = ParameterMode.Value });
            node.Leaf = true;
            return node;
        }


        public ActionResult GetGridPanel(string containerId, Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null, string strDepartmentIDorUserID = null)
        {
            int departmentID = 0;
            int userID = 0;
            if (strDepartmentIDorUserID != null && int.TryParse(strDepartmentIDorUserID, out departmentID) == false)
            {
                var arr = strDepartmentIDorUserID.Split('_');
                userID = int.Parse(arr[1]);
            }

            X.GetCmp<Panel>(containerId).RemoveAll();

            // Thống kê theo department được chọn
            if (departmentID > 0)
            {
                ProblemEventReport_ByDepartment data = api.GetStatisticUser(fromDate, toDate, departmentID).Data;

                if (data.problemEventReport_ByDepartmentTypeDTOs != null && data.problemEventReport_ByDepartmentTypeDTOs.Count > 0)
                {
                    List<ProblemType> lstType = new List<ProblemType>();
                    List<User> lstUser = new List<User>();
                    for (int i = 0; i < data.problemEventReport_ByDepartmentTypeDTOs.Count; i++)
                    {
                        if (data.problemEventReport_ByDepartmentTypeDTOs[i].ProblemTypeID > 0 && data.problemEventReport_ByDepartmentTypeDTOs[i].UserID > 0)
                        {
                            if (!lstType.Exists(x => x.ProblemTypeID == data.problemEventReport_ByDepartmentTypeDTOs[i].ProblemTypeID))
                                lstType.Add(new ProblemType() { ProblemTypeID = data.problemEventReport_ByDepartmentTypeDTOs[i].ProblemTypeID, ProblemTypeName = data.problemEventReport_ByDepartmentTypeDTOs[i].ProblemTypeName });

                            if (!lstUser.Exists(x => x.UserID == data.problemEventReport_ByDepartmentTypeDTOs[i].UserID))
                                lstUser.Add(new User() { UserID = data.problemEventReport_ByDepartmentTypeDTOs[i].UserID, UserName = data.problemEventReport_ByDepartmentTypeDTOs[i].Name });
                        }
                    }
                    this.BuildGridPanel("grdStatisticUserType", Region.Center, "Thống kê theo loại", lstType, lstUser, data.problemEventReport_ByDepartmentTypeDTOs).AddTo(containerId);
                }
                else
                    this.BuildGridPanel("grdStatisticUserType", Region.Center, "Thống kê theo loại", new List<ProblemType>(), new List<User>(), data.problemEventReport_ByDepartmentTypeDTOs).AddTo(containerId);

                if (data.problemEventReport_ByDepartmentCriticalDTOs != null && data.problemEventReport_ByDepartmentCriticalDTOs.Count > 0)
                {
                    List<CriticalLevel> lstCri = new List<CriticalLevel>();
                    List<User> lstUserCri = new List<User>();

                    for (int i = 0; i < data.problemEventReport_ByDepartmentCriticalDTOs.Count; i++)
                    {
                        if (data.problemEventReport_ByDepartmentCriticalDTOs[i].CriticalLevelID > 0 && data.problemEventReport_ByDepartmentCriticalDTOs[i].UserID > 0)
                        {
                            if (!lstCri.Exists(x => x.CriticalLevelID == data.problemEventReport_ByDepartmentCriticalDTOs[i].CriticalLevelID))
                                lstCri.Add(new CriticalLevel() { CriticalLevelID = data.problemEventReport_ByDepartmentCriticalDTOs[i].CriticalLevelID, CriticalLevelName = data.problemEventReport_ByDepartmentCriticalDTOs[i].CriticalLevelName });

                            if (!lstUserCri.Exists(x => x.UserID == data.problemEventReport_ByDepartmentCriticalDTOs[i].UserID))
                                lstUserCri.Add(new User() { UserID = data.problemEventReport_ByDepartmentCriticalDTOs[i].UserID, UserName = data.problemEventReport_ByDepartmentCriticalDTOs[i].Name });
                        }
                    }
                    this.BuildGridPanelCri("grdStatisticUserCri", Region.South, "Thống kê theo mức độ nghiêm trọng", lstCri, lstUserCri, data.problemEventReport_ByDepartmentCriticalDTOs).AddTo(containerId);
                }
                else
                    this.BuildGridPanelCri("grdStatisticUserCri", Region.South, "Thống kê theo mức độ nghiêm trọng", new List<CriticalLevel>(), new List<User>(), data.problemEventReport_ByDepartmentCriticalDTOs).AddTo(containerId);
            }
            // Thống kê theo user được chọn
            else
            {
                List<ProblemEventReport_GetByUserDTO> lstData = api.GetStatisticOneUser(fromDate, toDate, userID).Data;

                if (lstData != null && lstData.Count > 0)
                {
                    List<ProblemType> lstType = new List<ProblemType>();
                    List<CriticalLevel> lstCri = new List<CriticalLevel>();

                    for (int i = 0; i < lstData.Count; i++)
                    {
                        if (lstData[i].ProblemTypeID > 0 && lstData[i].CriticalLevelID > 0)
                        {
                            if (!lstType.Exists(x => x.ProblemTypeID == lstData[i].ProblemTypeID))
                                lstType.Add(new ProblemType() { ProblemTypeID = lstData[i].ProblemTypeID, ProblemTypeName = lstData[i].ProblemTypeName });

                            if (!lstCri.Exists(x => x.CriticalLevelID == lstData[i].CriticalLevelID))
                                lstCri.Add(new CriticalLevel() { CriticalLevelID = lstData[i].CriticalLevelID, CriticalLevelName = lstData[i].CriticalLevelName });
                        }
                    }
                    this.BuildGridPanelOneUser("grdStatisticOneUser", Region.Center, "Thống kê theo cá nhân", lstCri, lstType, lstData).AddTo(containerId);
                    var v = new GridPanel();
                    v.ID = "grdStatisticUserCri";
                    v.Region = Region.South;
                    //v.Title = "test123";
                    v.Header = false;
                    v.AddTo(containerId);
                }
                else
                {
                    this.BuildGridPanelOneUser("grdStatisticOneUser", Region.Center, "Thống kê theo cá nhân", new List<CriticalLevel>(), new List<ProblemType>(), lstData).AddTo(containerId);
                    var v = new GridPanel();
                    v.Region = Region.South;
                    v.AddTo(containerId);
                }    
            }

            return this.Direct();
        }

        private Toolbar getToolbar(string title)
        {
            var btnEx = new Button();
            btnEx.Icon = Icon.PageExcel;
            btnEx.Text = "Xuất excel";
            btnEx.Handler = "exStatisticUserType();";

            Toolbar t = new Toolbar();
            t.Items.Add(new Label() { Html = string.Format("<B>{0}</B>", title) });
            t.Items.Add(btnEx);
            return t;
        }

        private Toolbar getToolbarCri(string title)
        {
            var btnEx = new Button();
            btnEx.Icon = Icon.PageExcel;
            btnEx.Text = "Xuất excel";
            btnEx.Handler = "exStatisticUserCri();";

            Toolbar t = new Toolbar();
            t.Items.Add(new Label() { Html = string.Format("<B>{0}</B>", title) });
            t.Items.Add(btnEx);
            return t;
        }

        private Toolbar getToolbarOneUser(string title)
        {
            var btnEx = new Button();
            btnEx.Icon = Icon.PageExcel;
            btnEx.Text = "Xuất excel";
            btnEx.Handler = "exStatisticOneUser();";

            Toolbar t = new Toolbar();
            t.Items.Add(new Label() { Html = string.Format("<B>{0}</B>", title) });
            t.Items.Add(btnEx);
            return t;
        }

        private Ext.Net.GridPanel BuildGridPanel(string userID, Region reg, string title, List<ProblemType> lstType, List<User> lstUser, List<ProblemEventReport_ByDepartmentTypeDTO> lstData)
        {
            var grd = new Ext.Net.GridPanel
            {
                Region = reg,
                ID = userID,
                Margin = 1,
                Header = false,
                ColumnLines = true,
                ForceFit = true,
                MinHeight = 300,
                TopBar =
                {
                    getToolbar(title)
                },
                Store =
                {
                    this.BuildStore(lstType, lstUser, lstData)
                },
                SelectionModel =
                {
                    new RowSelectionModel() { Mode = SelectionMode.Single }
                },
                View =
                {
                   new Ext.Net.GridView()
                   {
                        StripeRows = true,
                        TrackOver = true
                   }
                }
            };
            //Html.X().RowNumbererColumn().Text("STT").DataIndex("ID").Width(30).Align(Alignment.Center),
            grd.ColumnModel.Columns.Add(new Column
            {
                Text = "UserID",
                DataIndex = "UserName",
                Hidden = true
            });
            grd.ColumnModel.Columns.Add(new Column
            {
                Text = "Họ tên",
                DataIndex = "UserName"
            });
            // col type
            for (int i = 0; i < lstType.Count; i++)
            {
                string[] arr = { lstType[i].ProblemTypeID.ToString() };
                Renderer r = new Renderer();
                r.Fn = string.Format("renderDetailForType");

                grd.ColumnModel.Columns.Add(new Column
                {
                    Text = lstType[i].ProblemTypeName,
                    DataIndex = string.Format("type_{0}", lstType[i].ProblemTypeID),
                    Tag = lstType[i].ProblemTypeID,
                    Renderer = r
                });
            }
            return grd;
        }
        private Ext.Net.GridPanel BuildGridPanelCri(string id, Region reg, string title, List<CriticalLevel> lstCri, List<User> lstUser, List<ProblemEventReport_ByDepartmentCriticalDTO> lstData)
        {
            var grd = new Ext.Net.GridPanel
            {
                Region = reg,
                ID = id,
                Margin = 1,
                Header = false,
                ColumnLines = true,
                ForceFit = true,
                MinHeight = 300,
                TopBar =
                {
                    getToolbarCri(title)
                },
                Store =
                {
                    this.BuildStoreCri(lstCri,lstUser, lstData)
                },
                SelectionModel =
                {
                    new RowSelectionModel() { Mode = SelectionMode.Single }
                },
                View =
                {
                   new Ext.Net.GridView()
                   {
                        StripeRows = true,
                        TrackOver = true
                   }
                }
            };
            //Html.X().RowNumbererColumn().Text("STT").DataIndex("ID").Width(30).Align(Alignment.Center),
            grd.ColumnModel.Columns.Add(new Column
            {
                Text = "UserID",
                DataIndex = "UserID",
                Hidden = true
            });
            grd.ColumnModel.Columns.Add(new Column
            {
                Text = "Họ tên",
                DataIndex = "UserName"
            });
            // col cri
            for (int i = 0; i < lstCri.Count; i++)
            {
                string[] arr = { lstCri[i].CriticalLevelID.ToString() };
                Renderer r = new Renderer();
                r.Fn = string.Format("renderDetailForCri");

                grd.ColumnModel.Columns.Add(new Column
                {
                    Text = lstCri[i].CriticalLevelName,
                    DataIndex = string.Format("cri_{0}", lstCri[i].CriticalLevelID),
                    Tag = lstCri[i].CriticalLevelID,
                    Renderer = r
                });
            }
            return grd;
        }
        private Ext.Net.GridPanel BuildGridPanelOneUser(string id, Region reg, string title, List<CriticalLevel> lstCri, List<ProblemType> lstType, List<ProblemEventReport_GetByUserDTO> lstData)
        {
            var grd = new Ext.Net.GridPanel
            {
                Region = reg,
                ID = id,
                Margin = 1,
                Header = false,
                ColumnLines = true,
                ForceFit = true,
                MinHeight = 300,
                TopBar =
                {
                    getToolbarOneUser(title)
                },
                Store =
                {
                    this.BuildStoreOneUser(lstCri, lstType, lstData)
                },
                SelectionModel =
                {
                    new RowSelectionModel() { Mode = SelectionMode.Single }
                },
                View =
                {
                   new Ext.Net.GridView()
                   {
                        StripeRows = true,
                        TrackOver = true
                   }
                }
            };
            //Html.X().RowNumbererColumn().Text("STT").DataIndex("ID").Width(30).Align(Alignment.Center),
            grd.ColumnModel.Columns.Add(new Column
            {
                Text = "TypeID",
                DataIndex = "TypeID",
                Hidden = true
            });
            grd.ColumnModel.Columns.Add(new Column
            {
                Text = "Loại",
                DataIndex = "TypeName"
            });
            // col cri
            for (int i = 0; i < lstCri.Count; i++)
            {
                string[] arr = { lstCri[i].CriticalLevelID.ToString() };
                Renderer r = new Renderer();
                r.Fn = string.Format("renderDetailOneUser");

                grd.ColumnModel.Columns.Add(new Column
                {
                    Text = lstCri[i].CriticalLevelName,
                    DataIndex = string.Format("cri_{0}", lstCri[i].CriticalLevelID),
                    Tag = lstCri[i].CriticalLevelID,
                    Renderer = r
                });
            }
            return grd;
        }

        private Store BuildStore(List<ProblemType> lstType, List<User> lstUser, List<ProblemEventReport_ByDepartmentTypeDTO> lstData)
        {
            var mode = new Model();

            mode.Fields.Add(new ModelField("UserID"));
            mode.Fields.Add(new ModelField("UserName"));
            for (int i = 0; i < lstType.Count; i++)
            {
                mode.Fields.Add(new ModelField(string.Format("type_{0}", lstType[i].ProblemTypeID)));
            }

            Store store = new Store();
            store.Model.Add(mode);

            //store.DataSource = new Companies().GetAllCompanies();
            store.DataSource = getDataSource(lstType, lstUser, lstData);
            return store;
        }
        private Store BuildStoreCri(List<CriticalLevel> lstCri, List<User> lstUser, List<ProblemEventReport_ByDepartmentCriticalDTO> lstData)
        {
            var mode = new Model();

            mode.Fields.Add(new ModelField("UserID"));
            mode.Fields.Add(new ModelField("UserName"));
            for (int i = 0; i < lstCri.Count; i++)
            {
                mode.Fields.Add(new ModelField(string.Format("cri_{0}", lstCri[i].CriticalLevelID)));
            }

            Store store = new Store();
            store.Model.Add(mode);

            //store.DataSource = new Companies().GetAllCompanies();
            store.DataSource = getDataSourceCri(lstCri, lstUser, lstData);
            return store;
        }
        private Store BuildStoreOneUser(List<CriticalLevel> lstCri, List<ProblemType> lstType, List<ProblemEventReport_GetByUserDTO> lstData)
        {
            var mode = new Model();

            mode.Fields.Add(new ModelField("TypeID"));
            mode.Fields.Add(new ModelField("TypeName"));
            for (int i = 0; i < lstCri.Count; i++)
            {
                mode.Fields.Add(new ModelField(string.Format("cri_{0}", lstCri[i].CriticalLevelID)));
            }

            Store store = new Store();
            store.Model.Add(mode);

            store.DataSource = getDataSourceOneUser(lstCri, lstType, lstData);
            return store;
        }

        private DataTable getDataSource(List<ProblemType> lstType, List<User> lstUser, List<ProblemEventReport_ByDepartmentTypeDTO> lstData)
        {
            // add column
            DataTable dt = new DataTable();
            dt.Columns.Add("UserID");
            dt.Columns.Add("UserName");
            for (int i = 0; i < lstType.Count; i++)
            {
                dt.Columns.Add(string.Format("type_{0}", lstType[i].ProblemTypeID));
            }

            // add row value
            for (int i = 0; i < lstUser.Count; i++)
            {
                DataRow r = dt.NewRow();
                r["UserID"] = lstUser[i].UserID;
                r["UserName"] = lstUser[i].UserName;

                for (int j = 0; j < lstData.Count; j++)
                {
                    if (lstData[j].UserID == lstUser[i].UserID)
                    {
                        var colName = string.Format("type_{0}", lstData[j].ProblemTypeID);
                        r[colName] = lstData[j].Count;
                    }
                }
                dt.Rows.Add(r);
            }

            return dt;
        }

        private DataTable getDataSourceCri(List<CriticalLevel> lstCri, List<User> lstUser, List<ProblemEventReport_ByDepartmentCriticalDTO> lstData)
        {
            // add column
            DataTable dt = new DataTable();
            dt.Columns.Add("UserID");
            dt.Columns.Add("UserName");
            for (int i = 0; i < lstCri.Count; i++)
            {
                dt.Columns.Add(string.Format("cri_{0}", lstCri[i].CriticalLevelID));
            }

            // add row value
            for (int i = 0; i < lstUser.Count; i++)
            {
                DataRow r = dt.NewRow();
                r["UserID"] = lstUser[i].UserID;
                r["UserName"] = lstUser[i].UserName;

                for (int j = 0; j < lstData.Count; j++)
                {
                    if (lstData[j].UserID == lstUser[i].UserID)
                    {
                        var colName = string.Format("cri_{0}", lstData[j].CriticalLevelID);
                        r[colName] = lstData[j].Count;
                    }
                }
                dt.Rows.Add(r);
            }

            return dt;
        }

        private DataTable getDataSourceOneUser(List<CriticalLevel> lstCri, List<ProblemType> lstType, List<ProblemEventReport_GetByUserDTO> lstData)
        {
            // add column
            DataTable dt = new DataTable();
            dt.Columns.Add("TypeID");
            dt.Columns.Add("TypeName");
            for (int i = 0; i < lstCri.Count; i++)
            {
                dt.Columns.Add(string.Format("cri_{0}", lstCri[i].CriticalLevelID));
            }

            // add row value
            for (int i = 0; i < lstType.Count; i++)
            {
                DataRow r = dt.NewRow();
                r["TypeID"] = lstType[i].ProblemTypeID;
                r["TypeName"] = lstType[i].ProblemTypeName;

                for (int j = 0; j < lstData.Count; j++)
                {
                    if (lstData[j].ProblemTypeID == lstType[i].ProblemTypeID)
                    {
                        var colName = string.Format("cri_{0}", lstData[j].CriticalLevelID);
                        r[colName] = lstData[j].Count;
                    }
                }
                dt.Rows.Add(r);
            }

            return dt;
        }
    }
}
