﻿using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Services;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Profile.Controllers
{
    [BaseSystem(Name = "Thống kê hồ sơ theo danh mục", Icon = "ChartPie", IsActive = true, IsShow = true, Position = 1, Parent = GroupStatisticalMenuController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ListProfileStatisticalController : BaseController
    {
        //
        // GET: /Profile/ListProfileStatistical/
        private ProfileService profileSV = new ProfileService();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData(StoreRequestParameters parameters, int departmentId, string fromDate, string toDate)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var result = new List<iDAS.Services.ChartModel.ListProfileStatisticalInfo>();
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
                result = ChartModel.StatisticalCateProfile(filter, departmentId, searchFromDate, searchToDate);
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
            return new StoreResult(ChartModel.GenerateDataCateProfileByDepartment(id, searchFromDate, searchToDate));
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
            return new StoreResult(ChartModel.GenerateDataCateProfileByDepartmentColumn(id, searchFromDate, searchToDate));
        }      
        public StoreResult GetDataCatePie(int id, string fromDate, string toDate)
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
            return new StoreResult(ChartModel.GenerateDataCateList(id, searchFromDate, searchToDate));
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
            return new StoreResult(ChartModel.GenerateDataCateListColumn(id, searchFromDate, searchToDate));
        }
        /// <summary>
        /// View profile statistical
        /// </summary>
        /// <param name="node"></param>
        /// <param name="departmentId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public ActionResult LoadTotalProfileUse(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                var lst = profileSV.GetUseFromList(filter, cateId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadTotalProfileBorrow(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                var lst = profileSV.GetBorrowFromList(filter, cateId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadTotalProfileBorrowOverTime(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                var lst = profileSV.GetBorrowOverTimeFromList(filter, cateId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadTotalProfileNotUse(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                var lst = profileSV.GetNotUseFromList(filter, cateId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadTotalProfileRequestCancel(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                var lst = profileSV.GetRequestCancelFromList(filter, cateId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadTotalProfileCancel(StoreRequestParameters parameters, int cateId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate).AddDays(1).Subtract(new TimeSpan(1)) : DateTime.Now;
                var lst = profileSV.GetCancelFromList(filter, cateId, searchFromDate, searchToDate);
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
