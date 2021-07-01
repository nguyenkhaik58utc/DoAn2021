using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using iDAS.Services;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web;
using iDAS.Items;
using System.IO;
using iDAS.Utilities;
namespace iDAS.Web.Areas.Customer.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Thống kê kinh doanh", Icon = "ChartBar", IsActive = true, IsShow = true, Position = 2, Parent = GroupStatisticalController.Code)]
    public class StatisticalEmployeeController : BaseController
    {
        private CustomerStatisticalEmployeeSV customerStatisticalSV = new CustomerStatisticalEmployeeSV();
        public static List<FieldItem> lstFieldItem = new List<FieldItem>();
        public static List<string> _lstParam = new List<string>();
        public static FileExportItem _data = new FileExportItem();
        [BaseSystem(Name = "Xem thông tin")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult LoadDepartment(string node)
        {
            NodeCollection nodes = new NodeCollection();
            var departmentID = node == "root" ? 0 : System.Convert.ToInt32(node);
            List<HumanDepartmentItem> departments = customerStatisticalSV.GetDepartmentCustomer(departmentID);
            foreach (var department in departments)
            {
                nodes.Add(createNodeDepartment(department));
            }
            return this.Content(nodes.ToJson());
        }
        private Node createNodeDepartment(HumanDepartmentItem department)
        {
            Node node = new Node();
            node.NodeID = department.ID.ToString();
            node.Text = department.Name.ToUpper();
            node.Icon = department.ParentID == 0 ? Icon.House : Icon.Door;
            if (!department.IsActive)
            {
                node.Cls = "DepartmentNotAllow";
            }
            node.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = department.ParentID.ToString(), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsActive", Value = JSON.Serialize(department.IsActive), Mode = ParameterMode.Value });
            node.Leaf = !department.IsParent;
            return node;
        }
        public ActionResult LoadEmployees(StoreRequestParameters parameters, int id, string startTime, string endTime)
        {
            int totalCount = 0;
            var result = new List<CustomerStatisticalItem>();
            if (id != 0)
            {
                DateTime searchStartTime = DateTime.MinValue;
                DateTime searchEndTime = DateTime.MaxValue;
                if (!string.IsNullOrWhiteSpace(startTime))
                {
                    searchStartTime = iDAS.Utilities.Convert.ToDateTime(startTime);
                }
                if (!string.IsNullOrWhiteSpace(endTime))
                {
                    searchEndTime = iDAS.Utilities.Convert.ToDateTime(endTime).AddDays(1).Subtract(new TimeSpan(1));
                }
                result = customerStatisticalSV.GetEmployees(parameters.Page, parameters.Limit, out totalCount, id, searchStartTime, searchEndTime);
            }
            return this.Store(result, totalCount);
        }
        public ActionResult LoadChart(int id = 0)
        {
            var result= customerStatisticalSV.GetEmployeeChart(id, DateTime.Now.Year);
            return new StoreResult(result);
        }
        public ActionResult Customer(int id, string startTime = "", string endTime = "", string storeUrl = "")
        {
            ViewData["ID"] = id;
            ViewData["StartTime"] = startTime;
            ViewData["EndTime"] = endTime;
            ViewData["StoreUrl"] = storeUrl;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Customer", ViewData = ViewData };
        }
        public ActionResult Contract(int id, string startTime = "", string endTime = "", string storeUrl = "")
        {
            ViewData["ID"] = id;
            ViewData["StartTime"] = startTime;
            ViewData["EndTime"] = endTime;
            ViewData["StoreUrl"] = storeUrl;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Contract", ViewData = ViewData };
        }

        #region Thống kế khách hàng tiếp cận
        // Khách hàng hiện tại
        public StoreResult CustomerNormal(StoreRequestParameters parameters, int groupId, string startTime, string endTime)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            DateTime searchStartTime = DateTime.MinValue;
            DateTime searchEndTime = DateTime.MaxValue;
            if (!string.IsNullOrWhiteSpace(startTime))
            {
                searchStartTime = iDAS.Utilities.Convert.ToDateTime(startTime);
            }
            if (!string.IsNullOrWhiteSpace(endTime))
            {
                searchEndTime = iDAS.Utilities.Convert.ToDateTime(endTime).AddDays(1).Subtract(new TimeSpan(1));
            }
            var result = customerStatisticalSV.CustomerNormal(filter, groupId, searchStartTime, searchEndTime);
            return this.Store(result, filter.PageTotal);
        }
        // Khách hàng đã liên hệ
        public StoreResult CustomerNormalContact(StoreRequestParameters parameters, int groupId, string startTime, string endTime)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            DateTime searchStartTime = DateTime.MinValue;
            DateTime searchEndTime = DateTime.MaxValue;
            if (!string.IsNullOrWhiteSpace(startTime))
            {
                searchStartTime = iDAS.Utilities.Convert.ToDateTime(startTime);
            }
            if (!string.IsNullOrWhiteSpace(endTime))
            {
                searchEndTime = iDAS.Utilities.Convert.ToDateTime(endTime).AddDays(1).Subtract(new TimeSpan(1));
            }
            var result = customerStatisticalSV.CustomerNormalContacts(filter, groupId, searchStartTime, searchEndTime);
            return this.Store(result, filter.PageTotal);
        }
        // Khách hàng có nhu cầu
        public StoreResult CustomerNormalNeed(StoreRequestParameters parameters, int groupId, string startTime, string endTime)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            DateTime searchStartTime = DateTime.MinValue;
            DateTime searchEndTime = DateTime.MaxValue;
            if (!string.IsNullOrWhiteSpace(startTime))
            {
                searchStartTime = iDAS.Utilities.Convert.ToDateTime(startTime);
            }
            if (!string.IsNullOrWhiteSpace(endTime))
            {
                searchEndTime = iDAS.Utilities.Convert.ToDateTime(endTime).AddDays(1).Subtract(new TimeSpan(1));
            }
            var result = customerStatisticalSV.CustomerNormalNeed(filter, groupId, searchStartTime, searchEndTime);
            return this.Store(result, filter.PageTotal);
        }
        #endregion

        #region Thống kê khách hàng tiềm năng
        // Khách hàng đã gửi báo giá
        public StoreResult CustomerPotentialSendPrice(StoreRequestParameters parameters, int groupId, string startTime, string endTime)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            DateTime searchStartTime = DateTime.MinValue;
            DateTime searchEndTime = DateTime.MaxValue;
            if (!string.IsNullOrWhiteSpace(startTime))
            {
                searchStartTime = iDAS.Utilities.Convert.ToDateTime(startTime);
            }
            if (!string.IsNullOrWhiteSpace(endTime))
            {
                searchEndTime = iDAS.Utilities.Convert.ToDateTime(endTime).AddDays(1).Subtract(new TimeSpan(1));
            }
            var result = customerStatisticalSV.CustomerPotentialSendPrice(filter, groupId, searchStartTime, searchEndTime);
            return this.Store(result, filter.PageTotal);
        }
        // Khách hàng tiềm năng có hợp đồng
        public StoreResult CustomerPotentialSignContract(StoreRequestParameters parameters, int groupId, string startTime, string endTime)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            DateTime searchStartTime = DateTime.MinValue;
            DateTime searchEndTime = DateTime.MaxValue;
            if (!string.IsNullOrWhiteSpace(startTime))
            {
                searchStartTime = iDAS.Utilities.Convert.ToDateTime(startTime);
            }
            if (!string.IsNullOrWhiteSpace(endTime))
            {
                searchEndTime = iDAS.Utilities.Convert.ToDateTime(endTime).AddDays(1).Subtract(new TimeSpan(1));
            }
            var result = customerStatisticalSV.CustomerPotentialSignContract(filter, groupId, searchStartTime, searchEndTime);
            return this.Store(result, filter.PageTotal);
        }
        #endregion

        #region Thống kê khách hàng thực tế
        // Khách hàng đã liên hệ
        public StoreResult CustomerOfficialContacts(StoreRequestParameters parameters, int groupId, string startTime, string endTime)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            DateTime searchStartTime = DateTime.MinValue;
            DateTime searchEndTime = DateTime.MaxValue;
            if (!string.IsNullOrWhiteSpace(startTime))
            {
                searchStartTime = iDAS.Utilities.Convert.ToDateTime(startTime);
            }
            if (!string.IsNullOrWhiteSpace(endTime))
            {
                searchEndTime = iDAS.Utilities.Convert.ToDateTime(endTime).AddDays(1).Subtract(new TimeSpan(1));
            }
            var result = customerStatisticalSV.CustomerOfficialContact(filter, groupId, searchStartTime, searchEndTime);
            return this.Store(result, filter.PageTotal);
        }
        // Khách hàng thực tế có hợp đồng
        public StoreResult CustomerOfficialContract(StoreRequestParameters parameters, int groupId, string startTime, string endTime)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            DateTime searchStartTime = DateTime.MinValue;
            DateTime searchEndTime = DateTime.MaxValue;
            if (!string.IsNullOrWhiteSpace(startTime))
            {
                searchStartTime = iDAS.Utilities.Convert.ToDateTime(startTime);
            }
            if (!string.IsNullOrWhiteSpace(endTime))
            {
                searchEndTime = iDAS.Utilities.Convert.ToDateTime(endTime).AddDays(1).Subtract(new TimeSpan(1));
            }
            var result = customerStatisticalSV.CustomerOfficialContract(filter, groupId, searchStartTime, searchEndTime);
            return this.Store(result, filter.PageTotal);
        }
        #endregion

        #region Hợp đồng
        public StoreResult NumberContract(StoreRequestParameters parameters, int groupId, string startTime, string endTime)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            DateTime searchStartTime = DateTime.MinValue;
            DateTime searchEndTime = DateTime.MaxValue;
            if (!string.IsNullOrWhiteSpace(startTime))
            {
                searchStartTime = iDAS.Utilities.Convert.ToDateTime(startTime);
            }
            if (!string.IsNullOrWhiteSpace(endTime))
            {
                searchEndTime = iDAS.Utilities.Convert.ToDateTime(endTime).AddDays(1).Subtract(new TimeSpan(1));
            }
            var result = customerStatisticalSV.NumberContract(filter, groupId, searchStartTime, searchEndTime);
            return this.Store(result, filter.PageTotal);
        }
        public StoreResult NumberContractSucess(StoreRequestParameters parameters, int groupId, string startTime, string endTime)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            DateTime searchStartTime = DateTime.MinValue;
            DateTime searchEndTime = DateTime.MaxValue;
            if (!string.IsNullOrWhiteSpace(startTime))
            {
                searchStartTime = iDAS.Utilities.Convert.ToDateTime(startTime);
            }
            if (!string.IsNullOrWhiteSpace(endTime))
            {
                searchEndTime = iDAS.Utilities.Convert.ToDateTime(endTime).AddDays(1).Subtract(new TimeSpan(1));
            }
            var result = customerStatisticalSV.NumberContractSucess(filter, groupId, searchStartTime, searchEndTime);
            return this.Store(result, filter.PageTotal);
        }
        #endregion

        #region ExportExcel
        [ValidateInput(false)]
        public ActionResult ShowFrmExport(int currentPage, int pageSize, string arrObject, int id, string startTime, string endTime)
        {
            FileExportItem data = new FileExportItem();
            data.PageIndex = currentPage;
            data.PageSize = pageSize;
            _lstParam = new List<string>() { id.ToString(), startTime, endTime };
            lstFieldItem = Ext.Net.JSON.Deserialize<FieldItem[]>(arrObject).ToList();
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Export", Model = data };
        }
        public DirectResult ExportExcel(FileExportItem data)
        {
            _data = data;
            if (_data.TypeExport == (int)Common.Type_Export.ToFile)
            {
                var fileUploadField = X.GetCmp<FileUploadField>("fileUpload");
                var direction = Common.FileTempExport + Guid.NewGuid().ToString() + ".xlsx";
                if (fileUploadField.HasFile)
                {
                    if (fileUploadField.PostedFile.ContentLength < 300 * 1024 + 1)
                    {
                        if (!System.IO.File.Exists(Server.MapPath(direction)))
                        {
                            fileUploadField.PostedFile.SaveAs(Server.MapPath(direction));
                        }
                        _data._source = Server.MapPath(direction);
                        //ExportToExcel.CreateExcelDocument<SuppliersOrderItem>(lst, Server.MapPath(direction), dictNameValue, Guid.NewGuid().ToString(), this.Response, true);
                    }
                    else
                    {
                        Ultility.CreateMessageBox(title: RequestMessage.Notify, message: "Chỉ cho phép dung lượng import tối đa là 300KB");
                    }
                }
                else
                {
                    Ultility.CreateMessageBox(title: RequestMessage.Notify, message: RequestMessage.NotFileUpload);
                }
            }
            return this.Direct();
        }
        [ValidateInput(false)]
        public ActionResult GetFile()
        {
            int totalCount = 0;
            DateTime searchStartTime = DateTime.MinValue;
            DateTime searchEndTime = DateTime.MaxValue;
            if (!string.IsNullOrWhiteSpace(_lstParam[1]))
            {
                searchStartTime = iDAS.Utilities.Convert.ToDateTime(_lstParam[1]);
            }
            if (!string.IsNullOrWhiteSpace(_lstParam[2]))
            {
                searchEndTime = iDAS.Utilities.Convert.ToDateTime(_lstParam[2]).AddDays(1);
            }
            var lst = customerStatisticalSV.GetEmployees(_data.PageIndex, _data.PageSize, out totalCount, int.Parse(_lstParam[0]), searchStartTime, searchEndTime);
            Dictionary<string, string> dictNameValue = new Dictionary<string, string>();
            foreach (FieldItem f in lstFieldItem)
            {
                if (!f.hidden)
                    dictNameValue.Add(f.dataIndex, f.text);
            }
            if (_data.TypeExport == (int)Common.Type_Export.Default)
                ExportToExcel.CreateExcelDocument<CustomerStatisticalItem>(lst, "temp", this.Response, dictNameValue, "Báo cáo ...");
            else
            {
                ExportToExcel.CreateExcelDocument<CustomerStatisticalItem>(lst, _data._source, dictNameValue, Guid.NewGuid().ToString(), this.Response, true);
            }
            return this.Direct();
        }
        public ActionResult GetField(StoreRequestParameters parameters)
        {
            foreach (FieldItem f in lstFieldItem)
            {
                if (f.text.Trim() == "" || f.dataIndex.Trim() == "" || f.text == "&#160;")
                    f.hidden = true;
            }
            return this.Store(lstFieldItem);
        }
        [ValidateInput(false)]
        public DirectResult HandleChangesFieldExport(string dataIndex, string text, int position, bool hidden)
        {
            FieldItem obj = lstFieldItem.FirstOrDefault(i => i.dataIndex == dataIndex);
            obj.text = text;
            obj.hidden = hidden;
            if (obj.position > position)
            {
                for (int i = position; i < obj.position; i++)
                {
                    lstFieldItem[i - 1].position++;
                }
                obj.position = position;
                lstFieldItem = lstFieldItem.OrderBy(o => o.position).ToList();
            }
            else if (obj.position < position)
            {
                for (int i = obj.position; i <= position; i++)
                {
                    lstFieldItem[i - 1].position--;
                }
                obj.position = position;
                lstFieldItem = lstFieldItem.OrderBy(o => o.position).ToList();
            }

            X.GetCmp<Store>("stField").Reload();
            return this.Direct();
        }
        #endregion
    }
}
