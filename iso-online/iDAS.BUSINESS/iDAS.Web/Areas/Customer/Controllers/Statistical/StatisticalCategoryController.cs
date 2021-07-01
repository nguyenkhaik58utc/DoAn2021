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
using System.Data;
namespace iDAS.Web.Areas.Customer.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Thống kê khách hàng", Icon = "ChartBar", IsActive = true, IsShow = true, Position = 2, Parent = GroupStatisticalController.Code)]
    public class StatisticalCategoryController : BaseController
    {
        private CustomerStatisticalCategorySV customerStatisticalSV = new CustomerStatisticalCategorySV();
        public static List<FieldItem> lstFieldItem = new List<FieldItem>();
        public static List<string> _lstParam = new List<string>();        
        [BaseSystem(Name = "Xem thông tin")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(string node, string startTime, string endTime)
        {
            NodeCollection nodes = new NodeCollection();
            int id = node == "root" ? 0 : System.Convert.ToInt32(node);
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
            var lsDataNodes = customerStatisticalSV.GetCustomerCategorys(id, searchStartTime, searchEndTime);
            foreach (var dataNode in lsDataNodes)
            {
                Node nodeItem = new Node();
                nodeItem.NodeID = dataNode.CategoryID.ToString();
                nodeItem.Text = dataNode.CategoryName;
                nodeItem.Icon = dataNode.ParentID == 0 ? Icon.House : Icon.Door;
                nodeItem.Leaf = dataNode.Leaf;
                //else nodeItem.Expanded = false;
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "CategoryName", Value = dataNode.CategoryName.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = dataNode.ParentID.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "SumCustomer", Value = dataNode.SumCustomer.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "CustomerNormal", Value = dataNode.CustomerNormal.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "CustomerNormalContacts", Value = dataNode.CustomerNormalContacts.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "CustomerNormalNeed", Value = dataNode.CustomerNormalNeed.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "CustomerPotential", Value = dataNode.CustomerPotential.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "CustomerPotentialSendPrice", Value = dataNode.CustomerPotentialSendPrice.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "CustomerPotentialSignContract", Value = dataNode.CustomerPotentialSignContract.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "CustomerNotContract", Value = dataNode.CustomerNotContract.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "CustomerPotentialRateSuccess", Value = dataNode.CustomerPotentialRateSuccess.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "CustomerOfficial", Value = dataNode.CustomerOfficial.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "CustomerOfficialContacts", Value = dataNode.CustomerOfficialContacts.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "CustomerOfficialHasPotential", Value = dataNode.CustomerOfficialHasPotential.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "CustomerOfficialContract", Value = dataNode.CustomerOfficialContract.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "NumberContract", Value = dataNode.NumberContract.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TotalContract", Value = dataNode.TotalContract.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "NumberContractSucess", Value = dataNode.NumberContractSucess.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "TotalContractSucess", Value = dataNode.TotalContractSucess.ToString(), Mode = ParameterMode.Value });
                nodes.Add(nodeItem);
            }
            return this.Content(nodes.ToJson());
        }
        public ActionResult Chart(int id = 0, string name = "")
        {
            ViewData["ID"] = id;
            ViewData["Name"] = name;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Chart", ViewData = ViewData };
        }
        public StoreResult LoadChart(int id = 0)
        {
            return new StoreResult(customerStatisticalSV.GetCategoryChart(id, DateTime.Now.Year));
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

        // Khách hàng hiện tại
        public StoreResult SumCustomer(StoreRequestParameters parameters, int groupId, string startTime, string endTime)
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
            var result = customerStatisticalSV.SumCustomer(filter, groupId, searchStartTime, searchEndTime);
            return this.Store(result, filter.PageTotal);
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
        // Khách hàng hiện tại
        public StoreResult CustomerPotential(StoreRequestParameters parameters, int groupId, string startTime, string endTime)
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
            var result = customerStatisticalSV.CustomerPotential(filter, groupId, searchStartTime, searchEndTime);
            return this.Store(result, filter.PageTotal);
        }
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
        // Khách hàng không ký hợp đồng
        public StoreResult CustomerNotContract(StoreRequestParameters parameters, int groupId, string startTime, string endTime)
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
            var result = customerStatisticalSV.CustomerNotContract(filter, groupId, searchStartTime, searchEndTime);
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
        // Khách hàng hiện tại
        public StoreResult CustomerOfficial(StoreRequestParameters parameters, int groupId, string startTime, string endTime)
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
            var result = customerStatisticalSV.CustomerOfficial(filter, groupId, searchStartTime, searchEndTime);
            return this.Store(result, filter.PageTotal);
        }
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
        // Khách hàng thực tế tiềm năng
        public StoreResult CustomerOfficialHasPotential(StoreRequestParameters parameters, int groupId, string startTime, string endTime)
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
            var result = customerStatisticalSV.CustomerOfficialHasPotential(filter, groupId, searchStartTime, searchEndTime);
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
        #region Export Excel
        [ValidateInput(false)]
        public ActionResult ShowFrmExport(string arrObject, string node, string startTime, string endTime)
        {
            FileExportItem data = new FileExportItem();
            _lstParam = new List<string>() { node, startTime, endTime };
            lstFieldItem = Ext.Net.JSON.Deserialize<FieldItem[]>(arrObject).ToList();
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Export", Model = data };
        }
        public DirectResult ExportExcel(FileExportItem data)
        {
            bool sucess = false;
            string err = "";
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
            int id = _lstParam[0] == "root" ? 0 : System.Convert.ToInt32(_lstParam[0]);
            var lst = customerStatisticalSV.GetDataExport(id, searchStartTime, searchEndTime);            
            Dictionary<string, string> dictNameValue = new Dictionary<string, string>();
            foreach (FieldItem f in lstFieldItem)
            {
                if (!f.hidden)
                    dictNameValue.Add(f.dataIndex, f.text);
            }
            if (data.TypeExport == (int)Common.Type_Export.ToFile)
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
                        data._source = Server.MapPath(direction);
                        //DataSet ds = new DataSet();
                        //ds.Tables.Add(ExportToExcel.ListToDataTable<CustomerStatisticalCategoryItem>(lst, dictNameValue));
                        sucess = ExportToExcel.CreateExcelDocument<CustomerStatisticalCategoryItem>(lst, data._source, dictNameValue, "temp", this.Response,true,false);
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
            else
            {
                var direction = Common.FileTempExport + "temp" + DateTime.UtcNow.Ticks.ToString() + ".xlsx";
                data._source = Server.MapPath(direction);
                DataSet ds = new DataSet();
                ds.Tables.Add(ExportToExcel.ListToDataTable<CustomerStatisticalCategoryItem>(lst, dictNameValue));
                err = ExportToExcel.CreateExcelDocumentAsFile(ds, data._source, dictNameValue, "");
                if (err == "success")
                    sucess = true;
                //ExportWord.exportWord();
            }
            if (sucess)
                return this.Direct(data._source);
            else
            {
                Ultility.CreateMessageBox(title: RequestMessage.Notify, message: err);
                return this.Direct("Error");
            }
        }
        [ValidateInput(false)]
        public ActionResult GetFile(string source)
        {
            ExportToExcel.dowloadFile(source, this.Response);
            return this.Direct();
        }
        public ActionResult GetField(StoreRequestParameters parameters)
        {
            foreach (FieldItem f in lstFieldItem)
            {
                if (f.text.Trim() == "" || f.dataIndex.Trim() == "" || f.text == "&#160;")
                    f.hidden = true;
            }
            var obj = lstFieldItem.FirstOrDefault(i => i.dataIndex == "text");
            if (obj != null)
            {
                lstFieldItem.Remove(obj);
                int i = 0;
                foreach (FieldItem item in lstFieldItem)
                {
                    i++;
                    item.position = i;
                }
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
