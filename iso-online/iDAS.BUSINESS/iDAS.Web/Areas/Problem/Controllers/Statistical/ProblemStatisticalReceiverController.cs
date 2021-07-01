using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Web.API;
using iDAS.Web.Areas.Problem.Models;
using ISO.API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Problem.Controllers
{
    [BaseSystem(Name = "Thống kê tiếp nhận", Icon = "ChartBar", IsActive = true, IsShow = true, Position = 85, Parent = "ProblemEventStatistical")]
    public class ProblemStatisticalReceiverController : BaseController
    {
        ProblemStatisticAPI api = new ProblemStatisticAPI();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StatisticOnDutyDetail(Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null, bool onDuty = true, int departmentID = 0)
        {
            ViewData["fromDate"] = fromDate;
            ViewData["toDate"] = toDate;
            ViewData["onDuty"] = onDuty;
            ViewData["departmentID"] = departmentID;

            //return View("ProblemStatisticalDetail", ViewData = ViewData);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "ProblemStatisticalDetail", ViewData = ViewData };
        }

        public ActionResult GetStatisticDetailByOnDuty(StoreRequestParameters parameters, string fromDate = null, string toDate = null, bool onDuty = true, int departmentID = 0)
        {
            DateTime fromDate1 = DateTime.Parse(fromDate);
            DateTime toDate1 = DateTime.Parse(toDate);
            ResponseModel<List<ProblemEventListDTO>> resp = new ResponseModel<List<ProblemEventListDTO>>();
            resp = api.GetStatisticByOnDutyDetail(parameters.Page, parameters.Limit, fromDate1, toDate1, onDuty, departmentID);

            int total = 0;
            if (resp.TotalRows != null)
                total = resp.TotalRows.Value;
            return this.Store(new Paging<ProblemEventListDTO>(resp.Data, total));
        }

        public ActionResult GetGridPanel(string containerId, Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null)
        {
            List<ProblemOnDutyResModel> lstData = api.GetStatisticByOnDuty(fromDate, toDate).Data;
            
            List <Dep> lstDep = new List<Dep>();
            for (int i = 0; i < lstData.Count; i++)
            {
                for (int j = 0; j < lstData[i].Departments.Count; j++)
                {
                    if (!lstDep.Exists(x => x.DepartmentID == lstData[i].Departments[j].DepartmentID) && lstData[i].Departments[j].DepartmentID > 0)
                        lstDep.Add(lstData[i].Departments[j]);
                }
            }
            this.BuildGridPanel("grdStatistic", Region.Center, "Tiếp nhận sự cố", lstDep, lstData).AddTo(containerId);

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

        private Ext.Net.GridPanel BuildGridPanel(string id, Region reg, string title, List<Dep> lstDep, List<ProblemOnDutyResModel> lstData)
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
                Text = "OnDuty",
                DataIndex = "OnDuty",
                Hidden = true
            });
            grd.ColumnModel.Columns.Add(new Column
            {
                Text = "Loại",
                DataIndex = "OnDutyName"
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
      
        private Store BuildStore(List<Dep> lstDep, List<ProblemOnDutyResModel> lstData)
        {
            var mode = new Model();

            mode.Fields.Add(new ModelField("OnDuty"));
            mode.Fields.Add(new ModelField("OnDutyName"));
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
       
        private DataTable getDataSource(List<Dep> lstDep, List<ProblemOnDutyResModel> lstData)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("OnDuty");
            dt.Columns.Add("OnDutyName");
            for(int i=0; i<lstDep.Count; i++)
            {
                dt.Columns.Add(string.Format("dep_{0}", lstDep[i].DepartmentID));
            }

            for(int i=0; i< lstData.Count; i++)
            {
                DataRow r = dt.NewRow();
                r["OnDuty"] = lstData[i].OnDuty;
                r["OnDutyName"] = lstData[i].OnDutyName;
                
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
    }
}
