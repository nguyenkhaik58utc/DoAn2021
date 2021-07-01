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

    [BaseSystem(Name = "Thống kê theo đơn vị yêu cầu", Icon = "ChartBar", IsActive = true, IsShow = true, Position = 83, Parent = "ProblemEventStatistical")]
    public class ProblemStatisticalEventRequestDepController : BaseController
    {
        ProblemStatisticAPI api = new ProblemStatisticAPI();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StatisticTypeDetail(Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null, int type = 0, int requestID = 0, int criticalLevelID = 0)
        {
            ViewData["fromDate"] = fromDate;
            ViewData["toDate"] = toDate;
            ViewData["type"] = type;
            ViewData["requestID"] = requestID;
            ViewData["criticalLevelID"] = criticalLevelID;

            //return View("ProblemStatisticalDetail", ViewData = ViewData);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "ProblemStatisticalEventRequestDepDetail", ViewData = ViewData };
        }

        public ActionResult GetStatisticDetailByType(StoreRequestParameters parameters, string fromDate = null, string toDate = null, int type = 0, int requestID = 0, int criticalLevelID = 0)
        {
            DateTime fromDate1 = DateTime.Parse(fromDate);
            DateTime toDate1 = DateTime.Parse(toDate);
            ResponseModel<List<ProblemEventListDTO>> resp = new ResponseModel<List<ProblemEventListDTO>>();
            if (type > 0)
                resp = api.GetStatisticByTypeEventRequestDetail(parameters.Page, parameters.Limit, fromDate1, toDate1, type, requestID);
            else
                resp = api.GetStatisticByCriticalEventRequestDetail(parameters.Page, parameters.Limit, fromDate1, toDate1, criticalLevelID, requestID);

            int total = 0;
            if (resp.TotalRows != null)
                total = resp.TotalRows.Value;
            return this.Store(new Paging<ProblemEventListDTO>(resp.Data, total));
        }

        public ActionResult GetGridPanel(string containerId, Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null)
        {
            List<ProblemTypeEventRequestDepResModel> lstData = api.GetStatisticByTypeEventRequest(fromDate, toDate).Data;
            int indexDel = lstData.FindIndex(x => x.ProblemTypeID == 0);
            if (indexDel > -1)
                lstData.RemoveAt(indexDel);

            List<EventReqDep> lstDep = new List<EventReqDep>();
            for (int i = 0; i < lstData.Count; i++)
            {
                for (int j = 0; j < lstData[i].EventRequestDep.Count; j++)
                {
                    if (!lstDep.Exists(x => x.EventRequestDepID == lstData[i].EventRequestDep[j].EventRequestDepID) && lstData[i].EventRequestDep[j].EventRequestDepID > 0)
                        lstDep.Add(lstData[i].EventRequestDep[j]);
                }
            }
            this.BuildGridPanel("grdStatistic", Region.Center, "Phân loại", lstDep, lstData).AddTo(containerId);

            List<ProblemCriticalEventRequestDepResModel> lstDataCri = api.GetStatisticByCriticalLevelEventRequest(fromDate, toDate).Data;
            int indexDelCri = lstDataCri.FindIndex(x => x.ProblemCriticalLevelID == 0);
            if (indexDelCri > -1)
                lstDataCri.RemoveAt(indexDelCri);

            List<EventReqDep> lstDepCri = new List<EventReqDep>();
            for (int i = 0; i < lstDataCri.Count; i++)
            {
                for (int j = 0; j < lstDataCri[i].EventRequestDep.Count; j++)
                {
                    if (!lstDepCri.Exists(x => x.EventRequestDepID == lstDataCri[i].EventRequestDep[j].EventRequestDepID) && lstDataCri[i].EventRequestDep[j].EventRequestDepID > 0)
                        lstDepCri.Add(lstDataCri[i].EventRequestDep[j]);
                }
            }
            this.BuildGridPanelCri("grdStatisticCri", Region.South, "Mức độ nghiêm trọng", lstDepCri, lstDataCri).AddTo(containerId);

            return this.Direct();
        }

        private Ext.Net.GridPanel BuildGridPanel(string id, Region reg, string title, List<EventReqDep> lstDep, List<ProblemTypeEventRequestDepResModel> lstData)
        {
            var grd = new Ext.Net.GridPanel
            {
                Region = reg,
                ID = id,
                Margin = 1,
                Header = true,
                ColumnLines = true,
                ForceFit = true,
                //Border = false,
                MinHeight = 300,
                Title = title,
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
                Text = "EventRequestDepID",
                DataIndex = "EventRequestDepName",
                Hidden = true
            });
            grd.ColumnModel.Columns.Add(new Column
            {
                Text = "Đơn vị",
                DataIndex = "EventRequestDepName"
            });
            // col department
            for (int i = 0; i < lstData.Count; i++)
            {
                string[] arr = { lstData[i].ProblemTypeID.ToString() };
                Renderer r = new Renderer();
                r.Fn = string.Format("renderDetail");

                grd.ColumnModel.Columns.Add(new Column
                {
                    Text = lstData[i].ProblemTypeName,
                    DataIndex = string.Format("dep_{0}", lstData[i].ProblemTypeID),
                    Tag = lstData[i].ProblemTypeID,
                    Renderer = r
                });
            }
            return grd;
        }
        private Ext.Net.GridPanel BuildGridPanelCri(string id, Region reg, string title, List<EventReqDep> lstDep, List<ProblemCriticalEventRequestDepResModel> lstData)
        {
            var grd = new Ext.Net.GridPanel
            {
                Region = reg,
                ID = id,
                Margin = 1,
                Header = true,
                ColumnLines = true,
                ForceFit = true,
                //Border = false,
                MinHeight = 300,
                Title = title,
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
                Text = "EventRequestDepID",
                DataIndex = "EventRequestDepName",
                Hidden = true
            });
            grd.ColumnModel.Columns.Add(new Column
            {
                Text = "Đơn vị",
                DataIndex = "EventRequestDepName"
            });
            // col department
            for (int i = 0; i < lstData.Count; i++)
            {
                string[] arr = { lstData[i].ProblemCriticalLevelID.ToString() };
                Renderer r = new Renderer();
                r.Fn = string.Format("renderDetailCri");

                grd.ColumnModel.Columns.Add(new Column
                {
                    Text = lstData[i].ProblemCriticalLevelName,
                    DataIndex = string.Format("dep_{0}", lstData[i].ProblemCriticalLevelID),
                    Tag = lstData[i].ProblemCriticalLevelID,
                    Renderer = r
                });
            }
            return grd;
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

        private Store BuildStore(List<EventReqDep> lstDep, List<ProblemTypeEventRequestDepResModel> lstData)
        {
            var mode = new Model();

            mode.Fields.Add(new ModelField("EventRequestDepID"));
            mode.Fields.Add(new ModelField("EventRequestDepName"));
            for (int i = 0; i < lstData.Count; i++)
            {
                mode.Fields.Add(new ModelField(string.Format("dep_{0}", lstData[i].ProblemTypeID)));
            }

            Store store = new Store();
            store.Model.Add(mode);

            //store.DataSource = new Companies().GetAllCompanies();
            store.DataSource = getDataSource(lstDep, lstData);
            return store;
        }
        private Store BuildStoreCri(List<EventReqDep> lstDep, List<ProblemCriticalEventRequestDepResModel> lstData)
        {
            var mode = new Model();

            mode.Fields.Add(new ModelField("EventRequestDepID"));
            mode.Fields.Add(new ModelField("EventRequestDepName"));
            for (int i = 0; i < lstData.Count; i++)
            {
                mode.Fields.Add(new ModelField(string.Format("dep_{0}", lstData[i].ProblemCriticalLevelID)));
            }

            Store store = new Store();
            store.Model.Add(mode);

            //store.DataSource = new Companies().GetAllCompanies();
            store.DataSource = getDataSourceCri(lstDep, lstData);
            return store;
        }

        private DataTable getDataSource(List<EventReqDep> lstDep, List<ProblemTypeEventRequestDepResModel> lstData)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("EventRequestDepID");
            dt.Columns.Add("EventRequestDepName");
            for (int i = 0; i < lstData.Count; i++)
            {
                dt.Columns.Add(string.Format("dep_{0}", lstData[i].ProblemTypeID));
            }

            for (int i = 0; i < lstDep.Count; i++)
            {
                DataRow r = dt.NewRow();
                r["EventRequestDepID"] = lstDep[i].EventRequestDepID;
                r["EventRequestDepName"] = lstDep[i].EventRequestDepName;
                for (int index = 0; index < lstData.Count; index++)
                {
                    for (int j = 0; j < lstData[index].EventRequestDep.Count; j++)
                    {
                        if (lstData[index].EventRequestDep[j].EventRequestDepID > 0 && lstData[index].EventRequestDep[j].EventRequestDepID == lstDep[i].EventRequestDepID)
                        {
                            var colName = string.Format("dep_{0}", lstData[index].ProblemTypeID);
                            r[colName] = lstData[index].EventRequestDep[j].EventCount;
                        }
                    }
                }
                dt.Rows.Add(r);
            }

            return dt;
        }

        private DataTable getDataSourceCri(List<EventReqDep> lstDep, List<ProblemCriticalEventRequestDepResModel> lstData)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("EventRequestDepID");
            dt.Columns.Add("EventRequestDepName");
            for (int i = 0; i < lstData.Count; i++)
            {
                dt.Columns.Add(string.Format("dep_{0}", lstData[i].ProblemCriticalLevelID));
            }

            for (int i = 0; i < lstDep.Count; i++)
            {
                DataRow r = dt.NewRow();
                r["EventRequestDepID"] = lstDep[i].EventRequestDepID;
                r["EventRequestDepName"] = lstDep[i].EventRequestDepName;
                for (int index = 0; index < lstData.Count; index++)
                {
                    for (int j = 0; j < lstData[index].EventRequestDep.Count; j++)
                    {
                        if (lstData[index].EventRequestDep[j].EventRequestDepID > 0 && lstData[index].EventRequestDep[j].EventRequestDepID == lstDep[i].EventRequestDepID)
                        {
                            var colName = string.Format("dep_{0}", lstData[index].ProblemCriticalLevelID);
                            r[colName] = lstData[index].EventRequestDep[j].EventCount;
                        }
                    }
                }
                dt.Rows.Add(r);
            }

            return dt;
        }
    }
}