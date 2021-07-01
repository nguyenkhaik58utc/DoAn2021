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

namespace iDAS.Web.Areas.Human.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Thống kê nhân sự tổng hợp", Icon = "ChartBar", IsActive = true, IsShow = true, Position = 1, Parent = GroupStatisticalController.Code)]
    public class EmployeeStatisticalController : BaseController
    {
        //
        // GET: /Human/EmployeeStatistical/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetData(string node, string fromDate, string toDate)
        {
            NodeCollection nodes = new NodeCollection();
            int id = node == "root" ? 0 : System.Convert.ToInt32(node);            
            var lsDataNodes = new HumanEmployeeStatistiaclSV().GenerateDataStatistical(id);
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
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Data7", Value = dataNode.Data7.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Data1", Value = dataNode.Data1.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Data21", Value = dataNode.Data21.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Data8", Value = dataNode.Data8.ToString(), Mode = ParameterMode.Value });
                nodeItem.CustomAttributes.Add(new ConfigItem { Name = "Data9", Value = dataNode.Data9.ToString(), Mode = ParameterMode.Value });
                nodes.Add(nodeItem);
            }
            return this.Content(nodes.ToJson());
        }

        public ActionResult ViewDetailStatistical(string depId="",string urlStore = "", StoreParameter param = null)
        {
            ViewData["StoreUrlProfile"] = urlStore;
            ViewData["StoreParamProfileStatiscal"] = param;
            ViewData["depId"] = depId;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewData = ViewData
            };
        }
        public ActionResult ViewListContract(string depId = "", string urlStore = "", StoreParameter param = null)
        {
            ViewData["StoreUrlProfile"] = urlStore;
            ViewData["StoreParamProfileStatiscal"] = param;
            ViewData["depId"] = depId;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewData = ViewData
            };
        }
        public ActionResult LoadTotal(StoreRequestParameters parameters, int depId)
        {
            
            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                List<HumanEmployeeItem> lst = new List<HumanEmployeeItem>();
                lst = new HumanEmployeeStatistiaclSV().GetByDepID(filter, depId);
                foreach (HumanEmployeeItem emp in lst)
                {
                    string strChucDanh = "";
                    foreach (HumanGroupPositionItem hgpI in emp.lstHumanGrPos)
                    {
                        strChucDanh += hgpI.Name + " -- " + hgpI.GroupName + "</br>";
                    }
                    emp.ChucDanh = strChucDanh;
                }
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult renderO30(StoreRequestParameters parameters, int depId)
        {

            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                List<HumanEmployeeItem> lst = new List<HumanEmployeeItem>();
                lst = new HumanEmployeeStatistiaclSV().GetByDepIDO30(filter, depId);
                foreach (HumanEmployeeItem emp in lst)
                {
                    string strChucDanh = "";
                    foreach (HumanGroupPositionItem hgpI in emp.lstHumanGrPos)
                    {
                        strChucDanh += hgpI.Name + " -- " + hgpI.GroupName + "</br>";
                    }
                    emp.ChucDanh = strChucDanh;
                }
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult renderU30(StoreRequestParameters parameters, int depId)
        {

            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                List<HumanEmployeeItem> lst = new List<HumanEmployeeItem>();
                lst = new HumanEmployeeStatistiaclSV().GetByDepIDU30(filter, depId);
                foreach (HumanEmployeeItem emp in lst)
                {
                    string strChucDanh = "";
                    foreach (HumanGroupPositionItem hgpI in emp.lstHumanGrPos)
                    {
                        strChucDanh += hgpI.Name + " -- " + hgpI.GroupName + "</br>";
                    }
                    emp.ChucDanh = strChucDanh;
                }
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult renderMale(StoreRequestParameters parameters, int depId)
        {

            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                List<HumanEmployeeItem> lst = new List<HumanEmployeeItem>();
                lst = new HumanEmployeeStatistiaclSV().GetByDepIDGender(filter, depId,true);
                foreach (HumanEmployeeItem emp in lst)
                {
                    string strChucDanh = "";
                    foreach (HumanGroupPositionItem hgpI in emp.lstHumanGrPos)
                    {
                        strChucDanh += hgpI.Name + " -- " + hgpI.GroupName + "</br>";
                    }
                    emp.ChucDanh = strChucDanh;
                }
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult renderFemale(StoreRequestParameters parameters, int depId)
        {

            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                List<HumanEmployeeItem> lst = new List<HumanEmployeeItem>();
                lst = new HumanEmployeeStatistiaclSV().GetByDepIDGender(filter, depId,false);
                foreach (HumanEmployeeItem emp in lst)
                {
                    string strChucDanh = "";
                    foreach (HumanGroupPositionItem hgpI in emp.lstHumanGrPos)
                    {
                        strChucDanh += hgpI.Name + " -- " + hgpI.GroupName + "</br>";
                    }
                    emp.ChucDanh = strChucDanh;
                }
                return this.Store(lst, filter.PageTotal);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult renderContract(StoreRequestParameters parameters, int depId)
        {

            try
            {
                ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
                List<HumanProfileContractItem> lst = new List<HumanProfileContractItem>();
                lst = new HumanEmployeeStatistiaclSV().GetContractByDepID(filter, depId);                
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
