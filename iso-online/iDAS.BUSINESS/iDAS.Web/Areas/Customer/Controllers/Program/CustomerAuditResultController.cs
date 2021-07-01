using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using iDAS.Services;
using iDAS.Items;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web;
using System.IO;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Customer.Controllers
{
    [SystemAuthorize(CheckAuthorize =false)]
    public class CustomerAuditResultController : BaseController
    {
        private CustomerAssessSV CustomerAuditSV = new CustomerAssessSV();
        private CustomerAssessResultSV CustomerAuditResultSV = new CustomerAssessResultSV();
        public ActionResult AuditForm(int id = 0, int groupCustomerID = 0)
        {
            var data = new CustomerItem();
            data.ID = id;
            //   data.CustomerCategory.GroupIds = groupCustomerID.ToString();
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Audit", Model = data };
        }
        public ActionResult LoadAudit(StoreRequestParameters parameters, int id = 0)
        {
            int totalCount;
            return this.Store(CustomerAuditSV.GetByCustomer(parameters.Page, parameters.Limit, out totalCount, id), totalCount);
        }
        public ActionResult AuditResultForm(int id = 0, int auditID = 0, int categoryID = 0)
        {
            var data = new CustomerAuditResultItems();
            data.CustomerID = id;
            data.AuditID = auditID;
            data.CriteriaCategoryID = categoryID;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "AuditResult", Model = data };
        }
        public ActionResult LoadResult(string node, int cateId, int auditId = 0, int customerId = 0)
        {
            NodeCollection nodes = new NodeCollection();
            int parentId = node == "root" ? 0 : System.Convert.ToInt32(node);
            var groups = CustomerAuditResultSV.GetCriteriasByParentID(parentId, cateId, auditId, customerId);
            foreach (var group in groups)
            {
                nodes.Add(createNote(group, cateId));
            }
            return this.Content(nodes.ToJson());
        }
        private Node createNote(CustomerAssessResultItem group, int cateId)
        {
            Node node = new Node();
            node.NodeID = group.ID.ToString();
            node.Expanded = false;
            node.Text = group.Name.ToUpper();
            node.Leaf = true;
            if (group.IsCategory)
            {
                node.Icon = Icon.Folder;
            }
            else
            {
                node.Icon = Icon.TagBlue;
            }
            node.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = group.ParentID.ToString(), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "ID", Value = JSON.Serialize(group.ID), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "Name", Value = group.Name.ToString(), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "MinPoint", Value = group.IsCategory ? string.Empty : group.MinPoint.ToString(), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "MaxPoint", Value = group.IsCategory ? string.Empty : group.MaxPoint.ToString(), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "Point", Value = group.IsCategory ? string.Empty : group.Point.ToString(), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsCategory", Value = JSON.Serialize(group.IsCategory), Mode = ParameterMode.Value });
            return node;
        }

        public ActionResult FrmEdit(int auditId = 0, int criteriaId = 0, int customerId = 0)
        {
            var obj = CustomerAuditResultSV.GetAuditResult(auditId, criteriaId, customerId);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "EditAuditResult", Model = obj };
        }
        public ActionResult UpdateAuditResult(CustomerAssessResultItem obj)
        {
            try
            {
                CustomerAuditResultSV.InsertOrUpdate(obj, User.ID);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw;
            }
            return this.Direct();
        }
        public ActionResult DeleteAuditResult(int auditId, int criteriaId, int customerId)
        {
            try
            {
                CustomerAuditResultSV.Delete(auditId, criteriaId, customerId);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
                throw;
            }
            return this.Direct();
        }
    }
}
