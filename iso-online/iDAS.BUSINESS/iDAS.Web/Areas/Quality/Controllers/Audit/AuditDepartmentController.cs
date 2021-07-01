using Ext.Net;
using iDAS.Core;
using iDAS.Base;
using iDAS.Items;
using iDAS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net.MVC;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Quality.Controllers
{
    public class AuditDepartmentController : BaseController
    {
        //
        // GET: /Quality/AuditDepartment/
        private QualityAuditDepartmentSV auditDepartmentSV = new QualityAuditDepartmentSV();
        public ActionResult GetData(StoreRequestParameters parameters, int auditProgramId)
        {
            List<QualityAuditDepartmentItem> lstData;
            int total;
            lstData = auditDepartmentSV.GetAll(parameters.Page, parameters.Limit, out total, auditProgramId);
            return this.Store(new Paging<QualityAuditDepartmentItem>(lstData, total));
        }
        public ActionResult GetDepartment(int auditProgramId)
        {
            List<ComboboxItem> lstData = new List<ComboboxItem>();
            lstData = auditDepartmentSV.GetCombobox(auditProgramId);
            return this.Store(lstData);
        }
        private Node createNote(QualityAuditDepartmentItem group, bool viewCancel = false, bool viewDeleted = false, int auditId = 0)
        {
            Node node = new Node();
            node.NodeID = group.ID.ToString();
            node.Expanded = group.ParentID == 0;
            node.Text = group.Name.ToUpper();
            if (group.IsCancel == true)
            {
                node.Cls = "clsCancel";
            }
            if (group.IsDelete == true)
            {
                node.Cls = "clsDeleted";
            }
            if (group.ParentID == 0)
            {
                node.Icon = Icon.House;
            }
            else
            {
                node.Icon = Icon.Door;
            }
            node.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = group.ParentID.ToString(), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsCancel", Value = JSON.Serialize(group.IsCancel), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsDeleted", Value = JSON.Serialize(group.IsDelete), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsSelected", Value = JSON.Serialize(group.IsSelected), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsUse", Value = JSON.Serialize(group.IsActive), Mode = ParameterMode.Value });
            var leaf = auditDepartmentSV.GetChildrenCount(group.ID, viewCancel, viewDeleted) <= 0;
            if (leaf)
            {
                node.Leaf = leaf;
            }
            return node;
        }
        public ActionResult LoadDepartments(string node, bool viewCancel = false, bool viewDeleted = false, int auditProgramId = 0)
        {
            NodeCollection nodes = new NodeCollection();
            int parentId = node == "root" ? 0 : System.Convert.ToInt32(node);
            var groups = auditDepartmentSV.GetGroupsByParentID(parentId, viewCancel, viewDeleted, auditProgramId);
            foreach (var group in groups)
            {
                nodes.Add(createNote(group, viewCancel, viewDeleted));
            }
            return this.Content(nodes.ToJson());
        }
    }
}
