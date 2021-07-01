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

namespace iDAS.Web.Areas.Document.Controllers
{
    [BaseSystem(Name = "Thống kê tài liệu theo danh mục", Icon = "ChartPie", IsActive = true, IsShow = true, Position = 1, Parent = GroupStatisticalDocumentController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ListDocumentStatisticalController : BaseController
    {
        //
        // GET: /Document/ListDocumentStatistical/
        private DocumentSV DocumentSV = new DocumentSV();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters, int departmentId, string fromDate, string toDate)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = new List<iDAS.Services.ChartModel.ListStatisticalDocumnetInfo>();
            if (departmentId != 0)
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
                result = ChartModel.StatisticalCateDocument(filter, departmentId, searchFromDate, searchToDate);
            }
            return this.Store(result, filter.PageTotal);
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
            return new StoreResult(ChartModel.GenerateDataCateDocumentByDepartment(id, searchFromDate, searchToDate));
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
            return new StoreResult(ChartModel.GenerateDataCateDocumentByDepartmentColumn(id, searchFromDate, searchToDate));
        }
        public StoreResult GetDataCateColumn(int id, string fromDate, string toDate)
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
            return new StoreResult(ChartModel.GenerateDataCateDocumentListColumn(id, searchFromDate, searchToDate));
        }      

        /// <summary>
        /// View Document statistical
        /// </summary>
        /// <param name="node"></param>
        /// <param name="cateId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public ActionResult LoadTotalDocument(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                List<DocumentItem> lst = new List<DocumentItem>();
                    lst = DocumentSV.GetTotalDocumentFromList(filter, cateId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadTotalDocumentIssued(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)): DateTime.Now;
                List<DocumentItem> lst = new List<DocumentItem>();
                lst = DocumentSV.GetDocumentIssuedFromList(filter, cateId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadTotalDocumentObsolete(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                List<DocumentItem> lst = new List<DocumentItem>();
                lst = DocumentSV.GetDocumentObsoleteFromList(filter, cateId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadTotalDocumentIn(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                List<DocumentItem> lst = new List<DocumentItem>();
                lst = DocumentSV.GetDocumentInFromList(filter, cateId, searchFromDate, searchToDate);

                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadTotalDocumentInIssued(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                List<DocumentItem> lst = new List<DocumentItem>();
                lst = DocumentSV.GetDocumentInIssuedFromList(filter, cateId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadTotalDocumentInObsolete(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                List<DocumentItem> lst = new List<DocumentItem>();
                lst = DocumentSV.GetDocumentInObsoleteFromList(filter, cateId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadTotalDocumentOut(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                List<DocumentItem> lst = new List<DocumentItem>();
                lst = DocumentSV.GetDocumentOutFromList(filter, cateId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadTotalDocumentOutIssued(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                List<DocumentItem> lst = new List<DocumentItem>();
                lst = DocumentSV.GetDocumentOutIssuedFromList(filter, cateId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadTotalDocumentOutObsolete(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                List<DocumentItem> lst = new List<DocumentItem>();
                lst = DocumentSV.GetDocumentOutObsoleteFromList(filter, cateId, searchFromDate, searchToDate);

                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadNumberDistribution(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                List<DocumentItem> lst = new List<DocumentItem>();
                lst = DocumentSV.GetNumberDistributionFromList(filter, cateId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadNumberOfDistribution(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                List<DocumentItem> lst = new List<DocumentItem>();
                lst = DocumentSV.GetNumberOfDistributionFromList(filter, cateId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadNumberOfThuhoi(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                List<DocumentItem> lst = new List<DocumentItem>();
                    lst = DocumentSV.GetNumberOfThuhoiFromList(filter, cateId, searchFromDate, searchToDate);
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
