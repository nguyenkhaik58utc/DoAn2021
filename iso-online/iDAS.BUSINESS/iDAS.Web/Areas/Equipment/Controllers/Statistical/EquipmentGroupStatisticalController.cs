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
    [BaseSystem(Name = "Thiết bị đo", Icon = "ChartPie", IsActive = true, IsShow = true, Position = 3, Parent = EquipmentStatisticalController.Code)]
    public class EquipmentGroupStatisticalController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData(string node, string fromDate, string toDate)
        {
            NodeCollection nodes = new NodeCollection();
            int id = node == "root" ? 0 : System.Convert.ToInt32(node);
            DateTime searchFromDate = DateTime.MinValue;
            DateTime searchToDate = DateTime.MaxValue;
            if (!string.IsNullOrWhiteSpace(fromDate))
            {
                searchFromDate = System.Convert.ToDateTime(fromDate);
            }
            else
            {
                searchFromDate = DateTime.Now;
            }
            if (!string.IsNullOrWhiteSpace(toDate))
            {
                searchToDate = System.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1));
            }
            else
            {
                searchToDate = DateTime.Now;
            }
            var lsDataNodes = new EquipmentStatisticalSV().GenerateDataEquipmentCategoryStatistical(id, searchFromDate, searchToDate);
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
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Data21", Value = dataNode.Data21.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Data31", Value = dataNode.Data31.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Data41", Value = dataNode.Data41.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Data51", Value = dataNode.Data51.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Data61", Value = dataNode.Data61.ToString(), Mode = ParameterMode.Value });
                nodes.Add(nodeItem);
            }
            return this.Content(nodes.ToJson());
        }

        public ActionResult ViewEquipmentMeasures(string urlStore = "", StoreParameter param = null, string fromDate = "", string toDate = "")
        {
            ViewData["StoreUrlProfile"] = urlStore;
            ViewData["StoreParamProfileStatiscal"] = param;
            ViewData["FromDate"] = fromDate;
            ViewData["ToDate"] = toDate;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewData = ViewData
            };
        }
        public ActionResult LoadTotalEq(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                List<EquipmentMeasureItem> lst = new List<EquipmentMeasureItem>();
                lst = new EquipmentMeasureSV().GetByCategory(filter, cateId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult renderTotalUse(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                List<EquipmentMeasureItem> lst = new List<EquipmentMeasureItem>();
                lst = new EquipmentMeasureSV().GetByCategoryUse(filter, cateId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult renderUseCKP(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                List<EquipmentMeasureItem> lst = new List<EquipmentMeasureItem>();
                lst = new EquipmentMeasureSV().GetUseCKP(filter, cateId, searchFromDate, searchToDate, true);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult renderUseDKP(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                List<EquipmentMeasureItem> lst = new List<EquipmentMeasureItem>();
                lst = new EquipmentMeasureSV().GetUseCKP(filter, cateId, searchFromDate, searchToDate, false);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult renderUsePass(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                List<EquipmentMeasureItem> lst = new List<EquipmentMeasureItem>();
                lst = new EquipmentMeasureSV().GetUsePass(filter, cateId, searchFromDate, searchToDate, true);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult renderUseNotPass(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                List<EquipmentMeasureItem> lst = new List<EquipmentMeasureItem>();
                lst = new EquipmentMeasureSV().GetUsePass(filter, cateId, searchFromDate, searchToDate, false);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult renderTotalNotUse(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                List<EquipmentMeasureItem> lst = new List<EquipmentMeasureItem>();
                lst = new EquipmentMeasureSV().GetTotalNotUse(filter, cateId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult renderNoteUseCKP(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                List<EquipmentMeasureItem> lst = new List<EquipmentMeasureItem>();
                lst = new EquipmentMeasureSV().GetNotUseErrors(filter, cateId, searchFromDate, searchToDate, true);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult renderNoteUseDKP(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                List<EquipmentMeasureItem> lst = new List<EquipmentMeasureItem>();
                lst = new EquipmentMeasureSV().GetNotUseErrors(filter, cateId, searchFromDate, searchToDate, false);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult renderNotUsePass(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                List<EquipmentMeasureItem> lst = new List<EquipmentMeasureItem>();
                lst = new EquipmentMeasureSV().NotUsePass(filter, cateId, searchFromDate, searchToDate, true);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult renderNotUseNotPass(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                List<EquipmentMeasureItem> lst = new List<EquipmentMeasureItem>();
                lst = new EquipmentMeasureSV().NotUsePass(filter, cateId, searchFromDate, searchToDate, false);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public StoreResult GetDataColumn(int id, string fromDate, string toDate)
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
            var data = new EquipmentStatisticalSV().GenerateDataEquipmentMeasuresCategoryStatisticalByID(id, searchFromDate, searchToDate);
            List<ChartModel> lstMode = new List<ChartModel>();
            if (data.Count > 0)
            {
                lstMode.Add(new ChartModel()
                {
                    Name = "Tổng số thiết bị",
                    Data2 = data[0].Data2,
                    Data1 = data[0].Data1,
                    Data3 = data[0].Data21
                });
                lstMode.Add(new ChartModel()
                {
                    Name = "Thiết bị kiểm định đạt",
                    Data1 = data[0].Data3 + data[0].Data31,
                    Data2 = data[0].Data3,
                    Data3 = data[0].Data31
                });
                lstMode.Add(new ChartModel()
                {
                    Name = "Thiết bị kiểm định không đạt",
                    Data1 = data[0].Data4 + data[0].Data41,
                    Data2 = data[0].Data4,
                    Data3 = data[0].Data41
                });
                lstMode.Add(new ChartModel()
                {
                    Name = "Thiết bị hiệu chuẩn đạt",
                    Data1 = data[0].Data5 + data[0].Data1,
                    Data2 = data[0].Data5,
                    Data3 = data[0].Data51
                });
                lstMode.Add(new ChartModel()
                {
                    Name = "Thiết bị hiệu chuẩn không đạt",
                    Data1 = data[0].Data6 + data[0].Data61,
                    Data2 = data[0].Data6,
                    Data3 = data[0].Data61
                });
            }
            return new StoreResult(lstMode);
        }
    }
}
