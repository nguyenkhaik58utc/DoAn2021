using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Utilities;
using iDAS.Web.API;
using iDAS.Web.Areas.Human.Model;
using iDAS.Web.Areas.Human.Models;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Linq;

namespace iDAS.Web.Areas.Human.Controllers
{
    public class StatisticController : BaseController
    {
        private HumanStatisticAPI api = new HumanStatisticAPI();

        // GET: Human/Statistic
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewEmpoyeeList(string depId = "", string titleId = "", string urlStore = "", StoreParameter param = null)
        {
            ViewData["StoreUrlProfile"] = urlStore;
            ViewData["StoreParamProfileStatiscal"] = param;
            ViewData["depId"] = depId;
            ViewData["titleId"] = titleId;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewData = ViewData
            };
        }

        public ActionResult GetEmpTotal(string node)
        {
            var lstAll = api.GetEmployeeTotal().Result;

            NodeCollection nodes = new NodeCollection();
            int id = node == "root" ? 0 : System.Convert.ToInt32(node);
            //var lsDataNodes = new HumanEmployeeStatistiaclSV().GenerateDataStatistical(id);
            foreach (var dataNode in lstAll)
            {
                if(dataNode.ParentID == id)
                {
                    Node nodeItem = new Node();
                    nodeItem.NodeID = dataNode.DepartmentID.ToString();
                    nodeItem.Text = dataNode.DepartmentName;
                    nodeItem.Icon = dataNode.ParentID == 0 ? Icon.House : Icon.Door;
                    nodeItem.Leaf = dataNode.Leaf;
                    nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Name", Value = dataNode.DepartmentName, Mode = ParameterMode.Value });
                    nodeItem.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = dataNode.ParentID.ToString(), Mode = ParameterMode.Value });
                    nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Data1", Value = dataNode.EmpTotal.ToString(), Mode = ParameterMode.Value });
                    nodes.Add(nodeItem);
                }    
            }
            return this.Content(nodes.ToJson());
        }

        public ActionResult LoadTotalDetail(StoreRequestParameters parameters, int depId)
        {
            try
            {
                int totalCount = 0;
                var result = api.GetListEmp(parameters.Limit, parameters.Page, depId).Result;
                foreach (V3_HumanEmployeeResponse emp in result.Data)
                {
                    List<string> strIDChucDanh = new List<string>();
                    string strChucDanh = "";

                    if (emp.IsTrial)
                        strChucDanh = "Thử việc";
                    else
                    {
                        foreach (var hgpI in emp.ListDepartmentTitle)
                        {
                            strChucDanh += hgpI.TitleName + " -- " + hgpI.DepartmentName + "</br>";
                            strIDChucDanh.Add(hgpI.ID.ToString());
                        }
                    }
                    emp.ListIDDepartmentTitle = string.Join("|", strIDChucDanh);
                    emp.DepartmentTitle = strChucDanh;
                }
                if (result.TotalRows != null)
                    totalCount = result.TotalRows.Value;
                return this.Store(new Paging<V3_HumanEmployeeResponse>(result.Data, totalCount));
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }

        public ActionResult LoadTotalDetailTitle(StoreRequestParameters parameters, int depId, int titleId)
        {
            try
            {
                int totalCount = 0;
                var result = api.GetListEmpByTitle(parameters.Limit, parameters.Page, titleId, depId).Result;
                foreach (V3_HumanEmployeeResponse emp in result.Data)
                {
                    List<string> strIDChucDanh = new List<string>();
                    string strChucDanh = "";

                    if (emp.IsTrial)
                        strChucDanh = "Thử việc";
                    else
                    {
                        foreach (var hgpI in emp.ListDepartmentTitle)
                        {
                            strChucDanh += hgpI.TitleName + " -- " + hgpI.DepartmentName + "</br>";
                            strIDChucDanh.Add(hgpI.ID.ToString());
                        }
                    }
                    emp.ListIDDepartmentTitle = string.Join("|", strIDChucDanh);
                    emp.DepartmentTitle = strChucDanh;
                }
                if (result.TotalRows != null)
                    totalCount = result.TotalRows.Value;
                return this.Store(new Paging<V3_HumanEmployeeResponse>(result.Data, totalCount));
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }

        public ActionResult GetGridPanel(string containerId)
        {
            List<EmployeeTitleTotal> lstData = api.GetEmployeeTitleTotal().Result;

            List<Dep> lstDep = new List<Dep>();
            for (int i = 0; i < lstData.Count; i++)
            {
                if (!lstDep.Exists(x => x.DepartmentID == lstData[i].DepartmentID) )
                {
                    Dep d = new Dep();
                    d.DepartmentID = lstData[i].DepartmentID;
                    d.DepartmentName = lstData[i].DepartmentName;
                    lstDep.Add(d);
                }    
            }

            this.BuildGridPanel("grdStatistic", Region.South, "Thống kê theo chức danh", lstDep, lstData).AddTo(containerId);

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

        private Ext.Net.GridPanel BuildGridPanel(string id, Region reg, string title, List<Dep> lstDep, List<EmployeeTitleTotal> lstData)
        {
            var grd = new Ext.Net.GridPanel
            {
                Region = reg,
                ID = id,
                Margin = 1,
                Header = false,
                ColumnLines = true,
                ForceFit = true,
                MinHeight = 250,
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
                Text = "TitleID",
                DataIndex = "TitleID",
                Hidden = true
            });
            grd.ColumnModel.Columns.Add(new Column
            {
                Text = "Chức danh",
                DataIndex = "TitleName"
            });

            for (int i = 0; i < lstDep.Count; i++)
            {
                string[] arr = { lstDep[i].DepartmentID.ToString() };
                Renderer r = new Renderer();
                r.Fn = string.Format("renderDetail");

                grd.ColumnModel.Columns.Add(new Column
                {
                    Text = lstDep[i].DepartmentName,
                    DataIndex = string.Format("dep_{0}", lstDep[i].DepartmentID),
                    Tag = lstDep[i].DepartmentID,
                    Renderer = r
                });
            }

            return grd;
        }

        private Store BuildStore(List<Dep> lstDep, List<EmployeeTitleTotal> lstData)
        {
            var mode = new Ext.Net.Model();
            mode.Fields.Add(new ModelField("TitleID"));
            mode.Fields.Add(new ModelField("TitleName"));
            for (int i = 0; i < lstDep.Count; i++)
            {
                mode.Fields.Add(new ModelField(string.Format("dep_{0}", lstDep[i].DepartmentID)));
            }

            Store store = new Store();
            store.Model.Add(mode);

            store.DataSource = getDataSource(lstDep, lstData);
            return store;
        }

        private DataTable getDataSource(List<Dep> lstDep, List<EmployeeTitleTotal> lstData)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TitleID");
            dt.Columns.Add("TitleName");
            for (int i = 0; i < lstDep.Count; i++)
            {
                dt.Columns.Add(string.Format("dep_{0}", lstDep[i].DepartmentID));
            }

            List<EmployeeTitleStatistic> lstStatistic = new List<EmployeeTitleStatistic>();

            // Tổng hợp dữ liệu list object
            for(int i=0; i<lstData.Count; i++)
            {
                EmployeeTitleStatistic sta = lstStatistic.Find(x => x.TitleID == lstData[i].TitleID);

                if (sta == null)
                {
                    sta = new EmployeeTitleStatistic();
                    sta.Departments = new List<Dep>();

                    sta.TitleID = lstData[i].TitleID;
                    sta.TitleName = lstData[i].TitleName;
                }

                Dep d = new Dep();
                d.DepartmentID = lstData[i].DepartmentID;
                d.DepartmentName = lstData[i].DepartmentName;
                d.Count = lstData[i].EmpTotal;
                sta.Departments.Add(d);

                lstStatistic.Add(sta);
            }

            for (int i = 0; i < lstStatistic.Count; i++)
            {
                DataRow r = dt.NewRow();
                r["TitleID"] = lstStatistic[i].TitleID;
                r["TitleName"] = lstStatistic[i].TitleName;

                for (int j = 0; j < lstStatistic[i].Departments.Count; j++)
                {
                    if (lstStatistic[i].Departments[j].DepartmentID > 0)
                    {
                        var colName = string.Format("dep_{0}", lstStatistic[i].Departments[j].DepartmentID);
                        r[colName] = lstStatistic[i].Departments[j].Count;
                    }
                }
                dt.Rows.Add(r);
            }

            return dt;
        }

    }
}