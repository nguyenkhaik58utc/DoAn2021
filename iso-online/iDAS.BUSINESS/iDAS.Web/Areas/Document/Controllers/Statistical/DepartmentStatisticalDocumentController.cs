using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Services;
using iDAS.Items;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Document.Controllers
{
    [BaseSystem(Name = "Thống kê tài liệu theo phòng ban", Icon = "ChartPie", IsActive = true, IsShow = true, Position = 2, Parent = GroupStatisticalDocumentController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class DepartmentStatisticalDocumentController : BaseController
    {
        //
        // GET: /Task/DepartmentStatisticalTask/
        private DocumentSV DocumentSV = new DocumentSV();
        private DepartmentSV departmentSV = new DepartmentSV();
        [BaseSystem(Name = "Xem dữ liệu thống kê", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
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
            var lsDataNodes = new ChartModel().GetDepartmentStatisticalDocument(id, searchFromDate, searchToDate);
            foreach (var dataNode in lsDataNodes)
            {
                Node nodeItem = new Node();
                nodeItem.NodeID = dataNode.DepartmentID.ToString();
                nodeItem.Text = dataNode.DepartmentName;
                nodeItem.Icon = dataNode.ParentID == 0 ? Icon.House : Icon.Door;
                nodeItem.Leaf = dataNode.Leaf;
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "DepartmentName", Value = dataNode.DepartmentName.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = dataNode.ParentID.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TotalDocument", Value = dataNode.TotalDocument.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TotalDocumentIn", Value = dataNode.TotalDocumentIn.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TotalDocumentInIssued", Value = dataNode.TotalDocumentInIssued.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TotalDocumentInObsolete", Value = dataNode.TotalDocumentInObsolete.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TotalDocumentIssued", Value = dataNode.TotalDocumentIssued.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TotalDocumentObsolete", Value = dataNode.TotalDocumentObsolete.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TotalDocumentOut", Value = dataNode.TotalDocumentOut.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TotalDocumentOutIssued", Value = dataNode.TotalDocumentOutIssued.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TotalDocumentOutObsolete", Value = dataNode.TotalDocumentOutObsolete.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "NumberDistribution", Value = dataNode.NumberDistribution.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "NumberOfDistribution", Value = dataNode.NumberOfDistribution.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "NumberOfThuhoi", Value = dataNode.NumberOfThuhoi.ToString(), Mode = ParameterMode.Value });
                nodes.Add(nodeItem);
            }
            return this.Content(nodes.ToJson());
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
            return new StoreResult(ChartModel.GenerateDataDocumentColumn(id, searchFromDate, searchToDate));
        }
        public ActionResult DocumentViewStatistical(string urlStore = "", StoreParameter param = null, string fromDate = "", string toDate = "")
        {
            ViewData["StoreUrlDocument"] = urlStore;
            ViewData["StoreParamDocumentStatiscal"] = param;
            ViewData["FromDate"] = fromDate;
            ViewData["ToDate"] = toDate;
            ViewData["UserLogOn"] = User.EmployeeID;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewData = ViewData
            };
        }
        /// <summary>
        /// View Document statistical
        /// </summary>
        /// <param name="node"></param>
        /// <param name="departmentId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public ActionResult LoadTotalDocument(StoreRequestParameters parameters, int departmentId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                List<DocumentItem> lst = new List<DocumentItem>();
                if (departmentSV.GetByID(departmentId).ParentID == 0)
                    lst = DocumentSV.GetTotalDocumentFromParentDepartment(filter, searchFromDate, searchToDate);
                else
                    lst = DocumentSV.GetTotalDocumentFromDepartment(filter, departmentId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadTotalDocumentIssued(StoreRequestParameters parameters, int departmentId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                List<DocumentItem> lst = new List<DocumentItem>();
                if (departmentSV.GetByID(departmentId).ParentID == 0)
                    lst = DocumentSV.GetDocumentIssuedFromParentDepartment(filter, searchFromDate, searchToDate);
                else
                    lst = DocumentSV.GetDocumentIssuedFromDepartment(filter, departmentId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadTotalDocumentObsolete(StoreRequestParameters parameters, int departmentId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                List<DocumentItem> lst = new List<DocumentItem>();
                if (departmentSV.GetByID(departmentId).ParentID == 0)
                    lst = DocumentSV.GetDocumentObsoleteFromParentDepartment(filter, searchFromDate, searchToDate);
                else
                    lst = DocumentSV.GetDocumentObsoleteFromDepartment(filter, departmentId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadTotalDocumentIn(StoreRequestParameters parameters, int departmentId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                List<DocumentItem> lst = new List<DocumentItem>();
                if (departmentSV.GetByID(departmentId).ParentID == 0)
                    lst = DocumentSV.GetDocumentInFromParentDepartment(filter, searchFromDate, searchToDate);
                else
                    lst = DocumentSV.GetDocumentInFromDepartment(filter, departmentId, searchFromDate, searchToDate);

                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadTotalDocumentInIssued(StoreRequestParameters parameters, int departmentId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                List<DocumentItem> lst = new List<DocumentItem>();
                if (departmentSV.GetByID(departmentId).ParentID == 0)
                    lst = DocumentSV.GetDocumentInIssuedFromParentDepartment(filter, searchFromDate, searchToDate);
                else
                    lst = DocumentSV.GetDocumentInIssuedFromDepartment(filter, departmentId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadTotalDocumentInObsolete(StoreRequestParameters parameters, int departmentId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                List<DocumentItem> lst = new List<DocumentItem>();
                if (departmentSV.GetByID(departmentId).ParentID == 0)
                    lst = DocumentSV.GetDocumentInObsoleteFromParentDepartment(filter, searchFromDate, searchToDate);
                else
                    lst = DocumentSV.GetDocumentInObsoleteFromDepartment(filter, departmentId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadTotalDocumentOut(StoreRequestParameters parameters, int departmentId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                List<DocumentItem> lst = new List<DocumentItem>();
                if (departmentSV.GetByID(departmentId).ParentID == 0)
                    lst = DocumentSV.GetDocumentOutFromParentDepartment(filter, searchFromDate, searchToDate);
                else
                    lst = DocumentSV.GetDocumentOutFromDepartment(filter, departmentId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadTotalDocumentOutIssued(StoreRequestParameters parameters, int departmentId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                List<DocumentItem> lst = new List<DocumentItem>();
                if (departmentSV.GetByID(departmentId).ParentID == 0)
                    lst = DocumentSV.GetDocumentOutIssuedFromParentDepartment(filter, searchFromDate, searchToDate);
                else
                    lst = DocumentSV.GetDocumentOutIssuedFromDepartment(filter, departmentId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadTotalDocumentOutObsolete(StoreRequestParameters parameters, int departmentId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                List<DocumentItem> lst = new List<DocumentItem>();
                if (departmentSV.GetByID(departmentId).ParentID == 0)
                    lst = DocumentSV.GetDocumentOutObsoleteFromParentDepartment(filter, searchFromDate, searchToDate);
                else
                    lst = DocumentSV.GetDocumentOutObsoleteFromDepartment(filter, departmentId, searchFromDate, searchToDate);

                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadNumberDistribution(StoreRequestParameters parameters, int departmentId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                List<DocumentItem> lst = new List<DocumentItem>();
                if (departmentSV.GetByID(departmentId).ParentID == 0)
                    lst = DocumentSV.GetNumberDistributionFromParentDepartment(filter, searchFromDate, searchToDate);
                else
                    lst = DocumentSV.GetNumberDistributionFromDepartment(filter, departmentId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadNumberOfDistribution(StoreRequestParameters parameters, int departmentId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                List<DocumentItem> lst = new List<DocumentItem>();
                if (departmentSV.GetByID(departmentId).ParentID == 0)
                    lst = DocumentSV.GetNumberOfDistributionFromParentDepartment(filter, searchFromDate, searchToDate);
                else
                    lst = DocumentSV.GetNumberOfDistributionFromDepartment(filter, departmentId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadNumberOfThuhoi(StoreRequestParameters parameters, int departmentId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                List<DocumentItem> lst = new List<DocumentItem>();
                if (departmentSV.GetByID(departmentId).ParentID == 0)
                    lst = DocumentSV.GetNumberOfThuhoiFromParentDepartment(filter, searchFromDate, searchToDate);
                else
                    lst = DocumentSV.GetNumberOfThuhoiFromDepartment(filter, departmentId, searchFromDate, searchToDate);
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
