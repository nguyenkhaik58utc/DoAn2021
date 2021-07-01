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

    [BaseSystem(Name = "Thống kê theo cơ quan thường trú", Icon = "ChartBar", IsActive = true, IsShow = true, Position = 83, Parent = "ProblemEventStatistical")]
    public class ProblemStatisticalResidentAgencyController : BaseController
    {
        ProblemStatisticAPI api = new ProblemStatisticAPI();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StatisticTypeDetail(Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null, int typeID = 0, int residentAgencyID = 0, int departmentID = 0)
        {
            ViewData["fromDate"] = fromDate;
            ViewData["toDate"] = toDate;
            ViewData["typeID"] = typeID;
            ViewData["residentAgencyID"] = residentAgencyID;
            ViewData["departmentID"] = departmentID;

            //return View("ProblemStatisticalDetail", ViewData = ViewData);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "ProblemStatisticalResidentAgencyDetail", ViewData = ViewData };
        }

        public ActionResult GetStatisticDetailByType(StoreRequestParameters parameters, string fromDate = null, string toDate = null, int typeID = 0, int residentAgencyID = 0, int departmentID = 0)
        {
            DateTime fromDate1 = DateTime.Parse(fromDate);
            DateTime toDate1 = DateTime.Parse(toDate);
            ResponseModel<List<ProblemEventListDTO>> resp = new ResponseModel<List<ProblemEventListDTO>>();
            if (typeID > 0)
                resp = api.GetStatisticByTypeResidentAgencyDetail(parameters.Page, parameters.Limit, fromDate1, toDate1, typeID, residentAgencyID);
            else
                resp = api.GetStatisticByDepartmentResidentAgencyDetail(parameters.Page, parameters.Limit, fromDate1, toDate1, departmentID, residentAgencyID);

            int total = 0;
            if (resp.TotalRows != null)
                total = resp.TotalRows.Value;
            return this.Store(new Paging<ProblemEventListDTO>(resp.Data, total));
        }

        public ActionResult GetGridPanel(string containerId, Nullable<DateTime> fromDate = null, Nullable<DateTime> toDate = null)
        {
            List<ProblemTypeEventResidentAgencyResModel> lstData = api.GetStatisticByTypeResidentAgency(fromDate, toDate).Data;
            int indexDel = lstData.FindIndex(x => x.ProblemTypeID == 0);
            if (indexDel > -1)
                lstData.RemoveAt(indexDel);

            List<ResidentAgency> lstDep = new List<ResidentAgency>();
            for (int i = 0; i < lstData.Count; i++)
            {
                for (int j = 0; j < lstData[i].ResidentAgencies.Count; j++)
                {
                    if (!lstDep.Exists(x => x.ResidentAgencyID == lstData[i].ResidentAgencies[j].ResidentAgencyID) && lstData[i].ResidentAgencies[j].ResidentAgencyID > 0)
                        lstDep.Add(lstData[i].ResidentAgencies[j]);
                }
            }
            this.BuildGridPanel("grdStatistic", Region.Center, "Phân loại", lstDep, lstData).AddTo(containerId);

            List<DepartmentEventResidentAgencyResModel> lstDataDerpartment = api.GetStatisticByDepartmentResidentAgency(fromDate, toDate).Data;
            int indexDelDerpartment = lstDataDerpartment.FindIndex(x => x.DepartmentID == 0);
            if (indexDelDerpartment > -1)
                lstDataDerpartment.RemoveAt(indexDelDerpartment);

            List<ResidentAgency> lstDepDerpartment = new List<ResidentAgency>();
            for (int i = 0; i < lstDataDerpartment.Count; i++)
            {
                for (int j = 0; j < lstDataDerpartment[i].ResidentAgencies.Count; j++)
                {
                    if (!lstDepDerpartment.Exists(x => x.ResidentAgencyID == lstDataDerpartment[i].ResidentAgencies[j].ResidentAgencyID) && lstDataDerpartment[i].ResidentAgencies[j].ResidentAgencyID > 0)
                        lstDepDerpartment.Add(lstDataDerpartment[i].ResidentAgencies[j]);
                }
            }
            this.BuildGridPanelDerpartment("grdStatisticDerpartment", Region.South, "Phòng ban", lstDepDerpartment, lstDataDerpartment).AddTo(containerId);

            return this.Direct();
        }

        private Ext.Net.GridPanel BuildGridPanel(string id, Region reg, string title, List<ResidentAgency> lstDep, List<ProblemTypeEventResidentAgencyResModel> lstData)
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
                Text = "residentAgencyID",
                DataIndex = "residentAgencyName",
                Hidden = true
            });
            grd.ColumnModel.Columns.Add(new Column
            {
                Text = "Đơn vị",
                DataIndex = "residentAgencyName"
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
        private Ext.Net.GridPanel BuildGridPanelDerpartment(string id, Region reg, string title, List<ResidentAgency> lstResident, List<DepartmentEventResidentAgencyResModel> lstData)
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
                    getToolbarDerpartment(title)
                },
                Store =
                {
                    this.BuildStoreDerpartment(lstResident, lstData)
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
                Text = "residentAgencyID",
                DataIndex = "residentAgencyName",
                Hidden = true
            });
            grd.ColumnModel.Columns.Add(new Column
            {
                Text = "Đơn vị",
                DataIndex = "residentAgencyName"
            });
            // col department
            for (int i = 0; i < lstData.Count; i++)
            {
                string[] arr = { lstData[i].DepartmentID.ToString() };
                Renderer r = new Renderer();
                r.Fn = string.Format("renderDetailDepartment");

                grd.ColumnModel.Columns.Add(new Column
                {
                    Text = lstData[i].DepartmentName,
                    DataIndex = string.Format("dep_{0}", lstData[i].DepartmentID),
                    Tag = lstData[i].DepartmentID,
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

        private Toolbar getToolbarDerpartment(string title)
        {
            var btnEx = new Button();
            btnEx.Icon = Icon.PageExcel;
            btnEx.Text = "Xuất excel";
            btnEx.Handler = "exStatisticDerpartment();";

            Toolbar t = new Toolbar();
            t.Items.Add(new Label() { Html = string.Format("<B>{0}</B>", title) });
            t.Items.Add(btnEx);
            return t;
        }

        private Store BuildStore(List<ResidentAgency> lstResident, List<ProblemTypeEventResidentAgencyResModel> lstData)
        {
            var mode = new Model();

            mode.Fields.Add(new ModelField("residentAgencyID"));
            mode.Fields.Add(new ModelField("residentAgencyName"));
            for (int i = 0; i < lstData.Count; i++)
            {
                mode.Fields.Add(new ModelField(string.Format("dep_{0}", lstData[i].ProblemTypeID)));
            }

            Store store = new Store();
            store.Model.Add(mode);

            //store.DataSource = new Companies().GetAllCompanies();
            store.DataSource = getDataSource(lstResident, lstData);
            return store;
        }
        private Store BuildStoreDerpartment(List<ResidentAgency> lstResident, List<DepartmentEventResidentAgencyResModel> lstData)
        {
            var mode = new Model();

            mode.Fields.Add(new ModelField("residentAgencyID"));
            mode.Fields.Add(new ModelField("residentAgencyName"));
            for (int i = 0; i < lstData.Count; i++)
            {
                mode.Fields.Add(new ModelField(string.Format("dep_{0}", lstData[i].DepartmentID)));
            }

            Store store = new Store();
            store.Model.Add(mode);

            //store.DataSource = new Companies().GetAllCompanies();
            store.DataSource = getDataSourceDerpartment(lstResident, lstData);
            return store;
        }

        private DataTable getDataSource(List<ResidentAgency> lstResident, List<ProblemTypeEventResidentAgencyResModel> lstData)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("residentAgencyID");
            dt.Columns.Add("residentAgencyName");
            for (int i = 0; i < lstData.Count; i++)
            {
                dt.Columns.Add(string.Format("dep_{0}", lstData[i].ProblemTypeID));
            }

            for (int i = 0; i < lstResident.Count; i++)
            {
                DataRow r = dt.NewRow();
                r["residentAgencyID"] = lstResident[i].ResidentAgencyID;
                r["residentAgencyName"] = lstResident[i].ResidentAgencyName;
                for (int index = 0; index < lstData.Count; index++)
                {
                    for (int j = 0; j < lstData[index].ResidentAgencies.Count; j++)
                    {
                        if (lstData[index].ResidentAgencies[j].ResidentAgencyID > 0 && lstData[index].ResidentAgencies[j].ResidentAgencyID == lstResident[i].ResidentAgencyID)
                        {
                            var colName = string.Format("dep_{0}", lstData[index].ProblemTypeID);
                            r[colName] = lstData[index].ResidentAgencies[j].EventCount;
                        }
                    }
                }
                dt.Rows.Add(r);
            }

            return dt;
        }

        private DataTable getDataSourceDerpartment(List<ResidentAgency> lstResident, List<DepartmentEventResidentAgencyResModel> lstData)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("residentAgencyID");
            dt.Columns.Add("residentAgencyName");
            for (int i = 0; i < lstData.Count; i++)
            {
                dt.Columns.Add(string.Format("dep_{0}", lstData[i].DepartmentID));
            }

            for (int i = 0; i < lstResident.Count; i++)
            {
                DataRow r = dt.NewRow();
                r["residentAgencyID"] = lstResident[i].ResidentAgencyID;
                r["residentAgencyName"] = lstResident[i].ResidentAgencyName;
                for (int index = 0; index < lstData.Count; index++)
                {
                    for (int j = 0; j < lstData[index].ResidentAgencies.Count; j++)
                    {
                        if (lstData[index].ResidentAgencies[j].ResidentAgencyID > 0 && lstData[index].ResidentAgencies[j].ResidentAgencyID == lstResident[i].ResidentAgencyID)
                        {
                            var colName = string.Format("dep_{0}", lstData[index].DepartmentID);
                            r[colName] = lstData[index].ResidentAgencies[j].EventCount;
                        }
                    }
                }
                dt.Rows.Add(r);
            }

            return dt;
        }
    }
}