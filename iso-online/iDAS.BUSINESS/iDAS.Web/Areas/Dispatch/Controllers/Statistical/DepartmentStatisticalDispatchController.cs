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


namespace iDAS.Web.Areas.Dispatch.Controllers
{
    [BaseSystem(Name = "Thống kê công văn theo phòng ban", Icon = "ChartPie", IsActive = true, IsShow = true, Position = 1, Parent = GroupStatisticalDispatchMenuController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class DepartmentStatisticalDispatchController : BaseController
    {
        //
        // GET: /Dispatch/DepartmentStatisticalDispatch/
        private DispatchToSV dispatchToSV = new DispatchToSV();
        private DispatchGoSV dispatchGoSV = new DispatchGoSV();
        private DepartmentSV departmentSV = new DepartmentSV();
        public ActionResult Index()
        {
            ViewData["UserLogOn"] = User.EmployeeID;
            return View();
        }
        public ActionResult ShowFrmDetail(int id = 0)
        {
            var obj = dispatchGoSV.GetVerifyInfoByID(id, User.EmployeeID);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
        }
        public ActionResult GetDataMoveIn(int id = 0)
        {
            List<DispatchGoObjectItem> lst = new List<DispatchGoObjectItem>();
            if (id > 0)
            {
                lst = dispatchGoSV.GetByIDForStatistical(id);
            }
            return this.Store(lst);
        }
        public ActionResult GetData(string node, int securityLevelId = 0, int urgencyId = 0, int categoryId = 0, int methodId = 0, string fromDate = "", string toDate = "")
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
            var lsDataNodes = new ChartModel().GetDepartmentStatisticalDispatch(id, securityLevelId, urgencyId, categoryId, methodId, searchFromDate, searchToDate);
            foreach (var dataNode in lsDataNodes)
            {
                Node nodeItem = new Node();
                nodeItem.NodeID = dataNode.DepartmentID.ToString();
                nodeItem.Text = dataNode.DepartmentName;
                nodeItem.Icon = dataNode.ParentID == 0 ? Icon.House : Icon.Door;
                nodeItem.Leaf = dataNode.Leaf;
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "DepartmentName", Value = dataNode.DepartmentName.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = dataNode.ParentID.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TotalDispatchTo", Value = dataNode.TotalDispatchTo.ToString(), Mode = ParameterMode.Value });
                // nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TotalDispatchToNew", Value = dataNode.TotalDispatchToNew.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TotalDispatchToVerify", Value = dataNode.TotalDispatchToVerify.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TotalDispatchGo", Value = dataNode.TotalDispatchGo.ToString(), Mode = ParameterMode.Value });
                // nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TotalDispatchGoApproval", Value = dataNode.TotalDispatchGoApproval.ToString(), Mode = ParameterMode.Value });
                // nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TotalDispatchGoNotApproval", Value = dataNode.TotalDispatchGoNotApproval.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TotalDispatchGoVerify", Value = dataNode.TotalDispatchGoVerify.ToString(), Mode = ParameterMode.Value });
                nodes.Add(nodeItem);
            }
            return this.Content(nodes.ToJson());
        }
        public StoreResult GetDataColumn(int id=0, int securityLevelId=0, int urgencyId=0, int categoryId=0, int methodId=0, string fromDate="", string toDate="")
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
            return new StoreResult(ChartModel.GenerateDataDispatchColumn(id, securityLevelId, urgencyId, categoryId, methodId, searchFromDate, searchToDate));
        }
        public ActionResult DispatchToViewStatistical(string urlStore = "", StoreParameter param = null, int securityLevelId = 0, int urgencyId = 0, int categoryId = 0, int methodId = 0, string fromDate = "", string toDate = "")
        {
            ViewData["StoreUrlDispatch"] = urlStore;
            ViewData["StoreParamDispatchStatiscal"] = param;
            ViewData["FromDate"] = fromDate;
            ViewData["ToDate"] = toDate;
            ViewData["securityLevelId"] = securityLevelId;
            ViewData["urgencyId"] = urgencyId;
            ViewData["categoryId"] = categoryId;
            ViewData["methodId"] = methodId;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewData = ViewData
            };
        }
        public ActionResult DispatchGoViewStatistical(string urlStore = "", StoreParameter param = null, int securityLevelId = 0, int urgencyId = 0, int categoryId = 0, int methodId = 0, string fromDate = "", string toDate = "")
        {
            ViewData["StoreUrlDispatchGo"] = urlStore;
            ViewData["StoreParamDispatchGoStatiscal"] = param;
            ViewData["FromDate"] = fromDate;
            ViewData["ToDate"] = toDate;
            ViewData["securityLevelId"] = securityLevelId;
            ViewData["urgencyId"] = urgencyId;
            ViewData["categoryId"] = categoryId;
            ViewData["methodId"] = methodId;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewData = ViewData
            };
        }
        /// <summary>
        /// Dung cho thong ke
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="departmentId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public ActionResult LoadTotalDispatchTo(StoreRequestParameters parameters, int departmentId = 0, int securityLevelId = 0, int urgencyId = 0, int categoryId = 0, int methodId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                List<DispatchToItem> lst = new List<DispatchToItem>();
                if (departmentSV.GetByID(departmentId).ParentID == 0)
                    lst = dispatchToSV.GetTotalDispatchToFromParentDepartment(filter, securityLevelId, urgencyId, categoryId, methodId, searchFromDate, searchToDate);
                else
                    lst = dispatchToSV.GetTotalDispatchToFromDepartment(filter, departmentId, securityLevelId, urgencyId, categoryId, methodId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadTotalDispatchToVerify(StoreRequestParameters parameters, int departmentId = 0, int securityLevelId = 0, int urgencyId = 0, int categoryId = 0, int methodId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                List<DispatchToItem> lst = new List<DispatchToItem>();
                if (departmentSV.GetByID(departmentId).ParentID == 0)
                    lst = dispatchToSV.GetTotalDispatchToVerifyFromParentDepartment(filter, securityLevelId, urgencyId, categoryId, methodId, searchFromDate, searchToDate);
                else
                    lst = dispatchToSV.GetTotalDispatchToVerifyFromDepartment(filter, departmentId, securityLevelId, urgencyId, categoryId, methodId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadTotalDispatchGo(StoreRequestParameters parameters, int departmentId = 0, int securityLevelId = 0, int urgencyId = 0, int categoryId = 0, int methodId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                List<DispatchGoItem> lst = new List<DispatchGoItem>();
                if (departmentSV.GetByID(departmentId).ParentID == 0)
                    lst = dispatchGoSV.GetTotalDispatchGoFromParentDepartment(filter, securityLevelId, urgencyId, categoryId, methodId, searchFromDate, searchToDate);
                else
                    lst = dispatchGoSV.GetTotalDispatchGoFromDepartment(filter, departmentId, securityLevelId, urgencyId, categoryId, methodId, searchFromDate, searchToDate);
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadTotalDispatchGoVerify(StoreRequestParameters parameters, int departmentId = 0, int securityLevelId = 0, int urgencyId = 0, int categoryId = 0, int methodId = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                DateTime searchFromDate = DateTime.MinValue;
                DateTime searchToDate = DateTime.MaxValue;
                searchFromDate = !string.IsNullOrWhiteSpace(fromDate) ? iDAS.Utilities.Convert.ToDateTime(fromDate) : DateTime.Now;
                searchToDate = !string.IsNullOrWhiteSpace(toDate) ? iDAS.Utilities.Convert.ToDateTime(toDate) : DateTime.Now;
                List<DispatchGoItem> lst = new List<DispatchGoItem>();
                if (departmentSV.GetByID(departmentId).ParentID == 0)
                    lst = dispatchGoSV.GetTotalDispatchGoVerifyFromParentDepartment(filter, securityLevelId, urgencyId, categoryId, methodId, searchFromDate, searchToDate);
                else
                    lst = dispatchGoSV.GetTotalDispatchGoVerifyFromDepartment(filter, departmentId, securityLevelId, urgencyId, categoryId, methodId, searchFromDate, searchToDate);
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
