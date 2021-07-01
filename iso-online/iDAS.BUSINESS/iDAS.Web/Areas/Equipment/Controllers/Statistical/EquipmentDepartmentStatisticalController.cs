using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Equipment.Controllers
{
    [BaseSystem(Name = "Phân phối thiết bị đo", Icon = "ChartPie", IsActive = true, IsShow = true, Position = 4, Parent = EquipmentStatisticalController.Code)]
    public class EquipmentDepartmentStatisticalController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData(string node, string fromDate, string toDate, string depID)
        {
            NodeCollection nodes = new NodeCollection();
            int id = node == "root" ? 0 : System.Convert.ToInt32(node);
            int deptID = depID == "" ? 0 : System.Convert.ToInt32(depID);
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
            var lsDataNodes = new EquipmentStatisticalSV().GenerateDataEquipmentPDepartmentStatistical(deptID, id, searchFromDate, searchToDate);
            foreach (var dataNode in lsDataNodes)
            {
                Node nodeItem = new Node();
                nodeItem.NodeID = dataNode.CateID.ToString();
                nodeItem.Text = dataNode.Name;
                nodeItem.Icon = dataNode.ParentID == 0 ? Icon.House : Icon.Door;
                nodeItem.Leaf = dataNode.Leaf;
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Name", Value = dataNode.Name.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = dataNode.ParentID.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Data2", Value = dataNode.Data2.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Data3", Value = dataNode.Data3.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Data4", Value = dataNode.Data4.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Data5", Value = dataNode.Data5.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Data6", Value = dataNode.Data6.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Data1", Value = dataNode.Data1.ToString(), Mode = ParameterMode.Value });
                nodes.Add(nodeItem);
            }
            return this.Content(nodes.ToJson());
        }
        public ActionResult ViewEquipmentMeasures(string urlStore = "", StoreParameter param = null, string fromDate = "", string toDate = "", int depId = 0)
        {
            ViewData["StoreUrlProfile"] = urlStore;
            ViewData["StoreParamProfileStatiscal"] = param;
            ViewData["FromDate"] = fromDate;
            ViewData["ToDate"] = toDate;
            ViewData["DepID"] = depId;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewData = ViewData
            };
        }
        public ActionResult renderTotal(StoreRequestParameters parameters, int cateId = 0, int depId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                List<EquipmentMeasureItem> lst = new List<EquipmentMeasureItem>();
                lst = new EquipmentMeasureSV().GetByCategory_Department(filter, cateId, depId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult renderTotalUse(StoreRequestParameters parameters, int cateId = 0, int depId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                List<EquipmentMeasureItem> lst = new List<EquipmentMeasureItem>();
                lst = new EquipmentMeasureSV().GetByCategory_DepartmentUsing(filter, cateId, depId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult renderErrorsPass(StoreRequestParameters parameters, int cateId = 0, int depId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                List<EquipmentMeasureItem> lst = new List<EquipmentMeasureItem>();
                lst = new EquipmentMeasureSV().GetByCategory_DepartmentErrors(filter, cateId, depId, searchFromDate, searchToDate, true);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult renderErrNotPass(StoreRequestParameters parameters, int cateId = 0, int depId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                List<EquipmentMeasureItem> lst = new List<EquipmentMeasureItem>();
                lst = new EquipmentMeasureSV().GetByCategory_DepartmentErrors(filter, cateId, depId, searchFromDate, searchToDate, false);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult renderMaintainPass(StoreRequestParameters parameters, int cateId = 0, int depId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                List<EquipmentMeasureItem> lst = new List<EquipmentMeasureItem>();
                lst = new EquipmentMeasureSV().GetByCategory_DepartmentMaintain(filter, cateId, depId, searchFromDate, searchToDate, true);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult renderMaintainNotPass(StoreRequestParameters parameters, int cateId = 0, int depId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                List<EquipmentMeasureItem> lst = new List<EquipmentMeasureItem>();
                lst = new EquipmentMeasureSV().GetByCategory_DepartmentMaintain(filter, cateId, depId, searchFromDate, searchToDate, false);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
    }

}
