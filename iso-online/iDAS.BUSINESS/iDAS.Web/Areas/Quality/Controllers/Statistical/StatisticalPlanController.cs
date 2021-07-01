using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Services;
using iDAS.Utilities;
using iDAS.Items;

namespace iDAS.Web.Areas.Quality.Controllers
{
    [BaseSystem(Name = "Kế hoạch", Icon = "ChartPie", IsActive = true, IsShow = true, Position = 2, Parent = GroupStatisticalController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class StatisticalPlanController : BaseController
    {
        StatisticalPlanSV StatisticalPlanSV = new StatisticalPlanSV();
        [BaseSystem(Name = "Xem thống kê", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        private Node createNodeDepartment(StatisticalQualityPlanItem department)
        {
            Node node = new Node();
            node.NodeID = department.DepartmentID.ToString();
            node.Text = department.Name.ToUpper();
            node.Icon = Icon.Door;
            node.CustomAttributes.Add(new ConfigItem { Name = "Total", Value = department.Total.ToString(), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "New", Value = JSON.Serialize(department.New), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "Edit", Value = JSON.Serialize(department.Edit), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "Wait", Value = JSON.Serialize(department.Wait), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "NotApproval", Value = JSON.Serialize(department.NotApproval), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "Performing", Value = JSON.Serialize(department.Performing), Mode = ParameterMode.Value });
            node.Leaf = department.IsLeaf;
            return node;
        }
        public ActionResult GetData(string node, string fromDate, string toDate)
        {
            NodeCollection nodes = new NodeCollection();
            var departmentID = node == "root" ? 0 : System.Convert.ToInt32(node);
            DateTime searchFromDate = DateTime.MinValue;
            DateTime searchToDate = DateTime.MaxValue;
            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                searchFromDate = iDAS.Utilities.Convert.ToDateTime(fromDate);
            }
            else
            {
                searchFromDate = DateTime.Now;
            }
            if (!string.IsNullOrWhiteSpace(toDate))
            {
                searchToDate = iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1));
            }
            else
            {
                searchToDate = DateTime.Now;
            }
            var departments = StatisticalPlanSV.GetStatistical(departmentID, searchFromDate, searchToDate);
            foreach (var department in departments)
            {
                nodes.Add(createNodeDepartment(department));
            }
            return this.Content(nodes.ToJson());

        }
        public StoreResult GetDataPie(int id, string fromDate, string toDate)
        {
            DateTime searchFromDate = DateTime.MinValue;
            DateTime searchToDate = DateTime.MaxValue;
            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                searchFromDate = iDAS.Utilities.Convert.ToDateTime(fromDate);
            }
            if (!string.IsNullOrWhiteSpace(toDate))
            {
                searchToDate = iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1));
            }
            return new StoreResult(StatisticalPlanSV.GetCharPie(id, searchFromDate, searchToDate));
        }
        public ActionResult getPlanDetail(int id, string fromDate, string toDate)
        {
            DateTime searchFromDate = DateTime.MinValue;
            DateTime searchToDate = DateTime.MaxValue;
            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                searchFromDate = iDAS.Utilities.Convert.ToDateTime(fromDate);
            }
            if (!string.IsNullOrWhiteSpace(toDate))
            {
                searchToDate = iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1));
            }
            var result = StatisticalPlanSV.getByCateId(id, searchFromDate, searchToDate, isLoadAll: true);
            return View("PlanDetail", result);
        }
    }
}
