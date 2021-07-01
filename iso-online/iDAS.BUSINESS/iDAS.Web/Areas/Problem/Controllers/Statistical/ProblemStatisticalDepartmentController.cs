using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Web.API;
using iDAS.Web.Areas.Problem.Models;
using iDAS.Web.ExtExtend;
using ISO.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Problem.Controllers
{
    [BaseSystem(Name = "Thống kê theo phòng ban", Icon = "ChartBar", IsActive = true, IsShow = true, Position = 83, Parent = "ProblemEventStatistical")]
    public class ProblemStatisticalDepartmentController : BaseController
    {
        ProblemStatisticAPI api = new ProblemStatisticAPI();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StatisticTypeDetail(Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null, int type = 0, int departmentID = 0, int criticalLevelID = 0)
        {
            ViewData["fromDate"] = fromDate;
            ViewData["toDate"] = toDate;
            ViewData["type"] = type;
            ViewData["departmentID"] = departmentID;
            ViewData["criticalLevelID"] = criticalLevelID;

            //return View("ProblemStatisticalDetail", ViewData = ViewData);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "ProblemStatisticalDetail", ViewData = ViewData };
        }

        public ActionResult GetStatisticDetailByType(StoreRequestParameters parameters, string fromDate = null, string toDate = null, int type = 0, int departmentID = 0, int criticalLevelID = 0)
        {
            DateTime fromDate1 = DateTime.Parse(fromDate);
            DateTime toDate1 = DateTime.Parse(toDate);
            ResponseModel<List<ProblemEventListDTO>> resp = new ResponseModel<List<ProblemEventListDTO>>();
            if(type > 0)
                resp = api.GetStatisticByTypeDetail(parameters.Page, parameters.Limit, fromDate1, toDate1, type, departmentID);
            else
                resp = api.GetStatisticByCriticalDetail(parameters.Page, parameters.Limit, fromDate1, toDate1, criticalLevelID, departmentID);

            int total = 0;
            if (resp.TotalRows != null)
                total = resp.TotalRows.Value;
            return this.Store(new Paging<ProblemEventListDTO>(resp.Data, total));
        }

        public ActionResult GetGridPanel(string containerId, Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null)
        {
            List<ProblemTypeResModel> lstData = api.GetStatisticByType(fromDate, toDate).Data;
            int indexDel = lstData.FindIndex(x => x.ProblemTypeID == 0);
            if (indexDel > -1)
                lstData.RemoveAt(indexDel);

            List <Dep> lstDep = new List<Dep>();
            for (int i = 0; i < lstData.Count; i++)
            {
                for (int j = 0; j < lstData[i].Departments.Count; j++)
                {
                    if (!lstDep.Exists(x => x.DepartmentID == lstData[i].Departments[j].DepartmentID) && lstData[i].Departments[j].DepartmentID > 0)
                        lstDep.Add(lstData[i].Departments[j]);
                }
            }
            this.BuildGridPanel("grdStatistic", Region.Center, "Phân loại", lstDep, lstData).AddTo(containerId);

            List<CriticalTypeResModel> lstDataCri = api.GetStatisticByCriticalLevel(fromDate, toDate).Data;
            int indexDelCri = lstDataCri.FindIndex(x => x.CriticalLevelID == 0);
            if (indexDelCri > -1)
                lstDataCri.RemoveAt(indexDelCri);

            List<Dep> lstDepCri = new List<Dep>();
            for (int i = 0; i < lstDataCri.Count; i++)
            {
                for (int j = 0; j < lstDataCri[i].Departments.Count; j++)
                {
                    if (!lstDepCri.Exists(x => x.DepartmentID == lstDataCri[i].Departments[j].DepartmentID) && lstDataCri[i].Departments[j].DepartmentID > 0)
                        lstDepCri.Add(lstDataCri[i].Departments[j]);
                }
            }
            this.BuildGridPanelCri("grdStatisticCri", Region.South, "Mức độ nghiêm trọng", lstDepCri, lstDataCri).AddTo(containerId);

            return this.Direct();
        }

        private Toolbar getToolbar(string title)
        {
            var btnEx = new Button();
            btnEx.Icon = Icon.PageExcel;
            btnEx.Text = "Xuất excel";
            btnEx.Handler = "exStatistic();";

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
            btnEx.Handler = "exStatisticCri();";

            Toolbar t = new Toolbar();
            t.Items.Add(new Label() { Html = string.Format("<B>{0}</B>", title) });
            t.Items.Add(btnEx);
            return t;
        }

        private Ext.Net.GridPanel BuildGridPanel(string id, Region reg, string title, List<Dep> lstDep, List<ProblemTypeResModel> lstData)
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
                    getToolbar(title)
                },
                Store =
                {
                    this.BuildStore(lstDep, lstData)
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
            // col department
            for (int i=0; i<lstDep.Count; i++)
            {
                string[] arr = { lstDep[i].DepartmentID.ToString() };
                Renderer r = new Renderer();
                r.Fn = string.Format("renderDetail");

                grd.ColumnModel.Columns.Add(new Column
                {
                    Text = lstDep[i].DepName,
                    DataIndex = string.Format("dep_{0}", lstDep[i].DepartmentID),
                    Tag = lstDep[i].DepartmentID,
                    Renderer = r
                });
            }
            return grd;
        }
        private Ext.Net.GridPanel BuildGridPanelCri(string id, Region reg, string title, List<Dep> lstDep, List<CriticalTypeResModel> lstData)
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
                    this.BuildStoreCri(lstDep, lstData)
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
                Text = "CriticalLevelID",
                DataIndex = "CriticalLevelID",
                Hidden = true
            });
            grd.ColumnModel.Columns.Add(new Column
            {
                Text = "Độ nghiêm trọng",
                DataIndex = "CriticalLevelName"
            });
            // col department
            for (int i = 0; i < lstDep.Count; i++)
            {
                string[] arr = { lstDep[i].DepartmentID.ToString() };
                Renderer r = new Renderer();
                r.Fn = string.Format("renderDetailCri");

                grd.ColumnModel.Columns.Add(new Column
                {
                    Text = lstDep[i].DepName,
                    DataIndex = string.Format("dep_{0}", lstDep[i].DepartmentID),
                    Tag = lstDep[i].DepartmentID,
                    Renderer = r
                });
            }
            return grd;
        }

        private Store BuildStore(List<Dep> lstDep, List<ProblemTypeResModel> lstData)
        {
            var mode = new Model();

            mode.Fields.Add(new ModelField("TypeID"));
            mode.Fields.Add(new ModelField("TypeName"));
            for (int i=0; i<lstDep.Count; i++)
            {
                mode.Fields.Add(new ModelField(string.Format("dep_{0}", lstDep[i].DepartmentID)));
            }

            Store store = new Store();
            store.Model.Add(mode);

            //store.DataSource = new Companies().GetAllCompanies();
            store.DataSource = getDataSource(lstDep, lstData);
            return store;
        }
        private Store BuildStoreCri(List<Dep> lstDep, List<CriticalTypeResModel> lstData)
        {
            var mode = new Model();

            mode.Fields.Add(new ModelField("CriticalLevelID"));
            mode.Fields.Add(new ModelField("CriticalLevelName"));
            for (int i = 0; i < lstDep.Count; i++)
            {
                mode.Fields.Add(new ModelField(string.Format("dep_{0}", lstDep[i].DepartmentID)));
            }

            Store store = new Store();
            store.Model.Add(mode);

            //store.DataSource = new Companies().GetAllCompanies();
            store.DataSource = getDataSourceCri(lstDep, lstData);
            return store;
        }

        private DataTable getDataSource(List<Dep> lstDep, List<ProblemTypeResModel> lstData)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TypeID");
            dt.Columns.Add("TypeName");
            for(int i=0; i<lstDep.Count; i++)
            {
                dt.Columns.Add(string.Format("dep_{0}", lstDep[i].DepartmentID));
            }

            for(int i=0; i< lstData.Count; i++)
            {
                DataRow r = dt.NewRow();
                r["TypeID"] = lstData[i].ProblemTypeID;
                r["TypeName"] = lstData[i].ProblemTypeName;
                
                for (int j=0; j<lstData[i].Departments.Count; j++)
                {
                    if(lstData[i].Departments[j].DepartmentID > 0)
                    {
                        var colName = string.Format("dep_{0}", lstData[i].Departments[j].DepartmentID);
                        r[colName] = lstData[i].Departments[j].EventCount;
                    }
                }
                dt.Rows.Add(r);
            }    

            return dt;
        }

        private DataTable getDataSourceCri(List<Dep> lstDep, List<CriticalTypeResModel> lstData)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("CriticalLevelID");
            dt.Columns.Add("CriticalLevelName");
            for (int i = 0; i < lstDep.Count; i++)
            {
                dt.Columns.Add(string.Format("dep_{0}", lstDep[i].DepartmentID));
            }

            for (int i = 0; i < lstData.Count; i++)
            {
                DataRow r = dt.NewRow();
                r["CriticalLevelID"] = lstData[i].CriticalLevelID;
                r["CriticalLevelName"] = lstData[i].CriticalLevelName;

                for (int j = 0; j < lstData[i].Departments.Count; j++)
                {
                    if (lstData[i].Departments[j].DepartmentID > 0)
                    {
                        var colName = string.Format("dep_{0}", lstData[i].Departments[j].DepartmentID);
                        r[colName] = lstData[i].Departments[j].EventCount;
                    }
                }
                dt.Rows.Add(r);
            }

            return dt;
        }
    }
}
