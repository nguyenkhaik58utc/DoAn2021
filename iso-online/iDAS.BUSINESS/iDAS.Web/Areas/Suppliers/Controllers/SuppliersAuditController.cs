using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;
using iDAS.Web.Controllers;

namespace iDAS.Web.Areas.Suppliers.Controllers
{
    [BaseSystem(Name = "Đánh giá NCC", Icon = "RubyAdd", IsActive = true, IsShow = true, Position = 3)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class SuppliersAuditController : BaseController
    {
        //
        // GET: /Suppliers/SuppliersAudit/
        SupplierAuditSV suppAuditSV = new SupplierAuditSV();
        SupplierAuditPlanSV suppAuditPlanSV = new SupplierAuditPlanSV();
        SupplierAuditResultSV suppAuditResultSV = new SupplierAuditResultSV();
        SuppliersOrderSV SuppOrderSV = new SuppliersOrderSV();
        public static List<SupplierAuditResultItem> lstSuppAuditResult = new List<SupplierAuditResultItem>();
        public static List<SupplierAuditResultItem> lstData1 = new List<SupplierAuditResultItem>();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetPlan(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            List<SupplierAuditPlanItem> lstData = new List<SupplierAuditPlanItem>();
            int total;
            lstData = suppAuditPlanSV.GetAllAccept(parameters.Page,parameters.Limit,out total);
            return this.Store(lstData, total);

        }

        public ActionResult GetGroupQuantity(int supplierAuditID)
        {

            List<TreeCriteriaCategoryItem> lstData = new List<TreeCriteriaCategoryItem>();            
            lstData = new QualityCriteriaCategorySV().GetTreeData("0");
            return this.Store(lstData, lstData.Count);
        }
        public ActionResult GetData(StoreRequestParameters parameters,int planId=0)
        {
            List<SupplierAuditItem> lstData = new List<SupplierAuditItem>();
            int total;
            if(planId>0)
            {
                lstData = suppAuditSV.GetAllByPlanId(parameters.Page, parameters.Limit, out total,planId);
            }   
            
            return this.Store(lstData, lstData.Count);
        }
        public ActionResult GetListSuppAudit(StoreRequestParameters parameters, int suppID = 0)
        {
            List<SupplierAuditItem> lstData = new List<SupplierAuditItem>();
            int total;
            if (suppID > 0)
            {
                lstData = suppAuditSV.GetAllBySuppID(parameters.Page, parameters.Limit, out total, suppID);
            }   
            
            return this.Store(lstData, lstData.Count);
        }
        public ActionResult GetQualityCriteria(StoreRequestParameters parameters, string categoryID, int supplierAuditID)
        {
            List<QualityCriteriaItem> lstData = new List<QualityCriteriaItem>();
            int total;
            if (!string.IsNullOrEmpty(categoryID))
            {
                lstData = new QualityCriteriaSV().GetCriteriaUsedByCateId(parameters.Page, parameters.Limit, out total, categoryID);
            }
            int _id = 0;
            lstSuppAuditResult = suppAuditResultSV.getBySuppAuditIDCategoryID(supplierAuditID, int.Parse(categoryID));
            //Xử lý để tránh trường hợp thêm mới khi chọn những tiêu chí đã được đánh giá
            foreach(QualityCriteriaItem item in lstData)
            {
                int count=lstSuppAuditResult.Where(x => x.QualityCriteria == item.ID).ToList().Count;
                if (lstSuppAuditResult.Count == 0 || count == 0)
                    lstSuppAuditResult.Add(new SupplierAuditResultItem
                    {
                        ID = _id++,
                        QualityCriteria = item.ID,
                        SupplierAuditID = supplierAuditID,
                        QualityCriteria1 = new QualityCriteriaItem
                        {
                            Name = item.Name,
                            CategoryName = item.CategoryName,
                            MinPoint = item.MinPoint,
                            MaxPoint = item.MaxPoint,
                            Factory = item.Factory
                        },
                        Factory = item.Factory
                    });
            }
            
            return this.Store(lstSuppAuditResult, lstSuppAuditResult.Count);

        }
        public ActionResult GetSuppAuditResult(StoreRequestParameters parameters, int supplierAuditID)
        {
            
            lstData1 = suppAuditResultSV.getBySuppAuditID(supplierAuditID);
            return this.Store(lstData1, lstData1.Count);

        }
        public ActionResult LoadOrder(StoreRequestParameters parameters,int supplierID)
        {
            int totalCount;
            return this.Store(SuppOrderSV.GetOrderListBySuppID(parameters.Page, parameters.Limit, out totalCount, supplierID), totalCount);
        }
        public ActionResult HandleChanges(int ID, decimal? point, decimal? totalpoint)
        {
            var item= lstSuppAuditResult.First(i => i.ID == ID);
            item.Point = point.HasValue?point.Value:0;
            item.TotalPoint = point.HasValue ? point*item.Factory : 0;
            return this.Direct();
        }
        public ActionResult HandleChangesSummary(int id, bool ispass)
        {
            try
            {
                suppAuditSV.UpdateIsPass(id, ispass);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }                       
            return this.Direct();
        }
        public ActionResult HandleChangesIndex(int ID, decimal? point, decimal? totalpoint)
        {
            var item = lstData1.First(i => i.ID == ID);
            item.Point = point.HasValue ? point.Value : 0;
            item.TotalPoint = point.HasValue ? point * item.Factory : 0;
            return this.Direct();
        }
        public ActionResult ShowFrmDeal(int suppid)
        {
            var data = new SupplierItem();
            data.ID = suppid;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Deal", Model = data };
        }
        
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = true)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmAdd(int id)
        {
            var data = new SupplierAuditItem();
            data.ID = id;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Insert", Model = data };
        }        
        public ActionResult AddResults()
        {
            try
            {
                
                foreach(SupplierAuditResultItem suppAuditResult in lstSuppAuditResult)
                {
                    //suppAuditResultSV.DeletebysupplierAuditID(suppAuditResult.ID);
                    suppAuditResultSV.Insert(suppAuditResult,User.ID);
                }
                //suppAuditSV.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StorePlanIndex").Reload();
                X.GetCmp<Window>("frmAuditResult").Close();
            }
            return this.Direct();
        }

        public ActionResult SaveResults()
        {
            try
            {
                foreach (SupplierAuditResultItem suppAuditResult in lstData1)
                {

                    suppAuditResultSV.Update(suppAuditResult, User.ID);
                }
                //suppAuditSV.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StorePlanIndex").Reload();
                
            }
            return this.Direct();
        }
    }
}
