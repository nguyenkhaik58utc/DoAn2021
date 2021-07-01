using iDAS.Base;
using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Utilities;

namespace iDAS.Services
{
    public class SupplierAuditPlanSV
    {
        SupplierAuditPlanDA suppAuditPlanDA = new SupplierAuditPlanDA();
        QualityPlanDA qualityPlanDA = new QualityPlanDA();
        private QualityPlanSV qualityPlanSV = new QualityPlanSV();
        public SupplierAuditPlanItem GetById(int Id)
        {
            var dbo = suppAuditPlanDA.Repository;
            int _QualityPlanID = suppAuditPlanDA.GetById(Id).QualityPlanID;
            var PlanItem = dbo.QualityPlans.Where(i => i.ID == _QualityPlanID)
                        .Select(item => new SupplierAuditPlanItem()
                        {
                            ID = Id,
                            QualityPlanID = item.ID,                        
                            Name = item.Name,
                            TargetID = item.QualityTargetID,
                            Department = new iDAS.Items.HumanDepartmentViewItem()
                            {
                                ID = item.DepartmentID,
                                Name = dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.DepartmentID).Name,
                            },                            
                            ParentID = item.ParentID,
                            ParentName = dbo.QualityPlans.FirstOrDefault(p => p.ID == item.ParentID).Name,
                            Type = item.Type,
                            ApprovalNote = item.Note,
                            Cost = item.Cost,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Content = item.Content,
                            IsApproval = item.IsApproval,
                            ApprovalAt = item.ApprovalAt,
                            ApprovalBy = item.ApprovalBy,
                            IsAccept = item.IsAccept,
                            IsEdit = item.IsEdit,
                            IsDelete = item.IsDelete,
                            Approval = new iDAS.Items.HumanEmployeeViewItem()
                            {
                                ID = item.ApprovalBy != null ? (int)item.ApprovalBy : 0,
                                Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == item.ApprovalBy).Name,
                                Role = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == item.ApprovalBy).HumanRole.Name,
                                Department = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == item.ApprovalBy).HumanRole.HumanDepartment.Name,
                            },
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            CreateByName = dbo.HumanEmployees.FirstOrDefault(a => a.ID == dbo.HumanUsers.FirstOrDefault(i => i.ID == item.CreateBy).HumanEmployeeID).Name,
                            UpdateAt = item.UpdateAt,
                            UpdateByName = dbo.HumanEmployees.FirstOrDefault(a => a.ID == dbo.HumanUsers.FirstOrDefault(i => i.ID == item.UpdateBy).HumanEmployeeID).Name,
                            UpdateBy = item.UpdateBy,
                                                    
                        })
                        .First();
            PlanItem.TargetName = dbo.QualityTargets.FirstOrDefault(i => i.ID == PlanItem.TargetID) != null ?
                "Đến ngày: " + dbo.QualityTargets.FirstOrDefault(m => m.ID == PlanItem.TargetID).CompleteAt.ToString() + " " + iDAS.Utilities.Common.GetTypeName(dbo.QualityTargets.FirstOrDefault(m => m.ID == PlanItem.TargetID).Type) + dbo.QualityTargets.FirstOrDefault(m => m.ID == PlanItem.TargetID).Value.ToString() + " " + dbo.QualityTargets.FirstOrDefault(m => m.ID == PlanItem.TargetID).Unit : string.Empty;
            return PlanItem;
        }
        public List<SupplierAuditPlanItem> GetAll(ModelFilter filter, int focusId)
        {
            var dbo = suppAuditPlanDA.Repository;
            int[] lstPlanId = GetListPlanID();
            IQueryable<iDAS.Base.QualityPlan> query = dbo.QualityPlans.Where(i => lstPlanId.Contains(i.ID))
                        .OrderByDescending(item => item.CreateAt);
                        if (focusId != 0)
                        {
                            filter.SortName = null;
                            query = query.OrderBy(item => dbo.SupplierAuditPlans.Where(p => p.QualityPlanID == item.ID).Select(i => i.ID).FirstOrDefault() == focusId 
                                ? false : true);
                        }
                var lst = query.Filter(filter)
                        .Select(item=>new SupplierAuditPlanItem()
                        {
                            ID = dbo.SupplierAuditPlans.Where(p=>p.QualityPlanID==item.ID).Select(i=>i.ID).FirstOrDefault(),
                            QualityPlanID = item.ID,
                            Name = item.Name,
                            TargetID = item.QualityTargetID,
                            Department = new iDAS.Items.HumanDepartmentViewItem()
                            {
                                ID = item.DepartmentID,
                                Name = dbo.HumanDepartments.FirstOrDefault(d => d.ID == item.DepartmentID).Name
                            },
                            ParentID = item.ParentID,
                            Type = item.Type,
                            Cost = item.Cost,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Content = item.Content,
                            IsEdit = item.IsEdit,
                            IsApproval = item.IsApproval,
                            IsAccept = item.IsAccept,
                            ApprovalAt = item.ApprovalAt,
                            ApprovalBy = item.ApprovalBy,
                            IsDelete = item.IsDelete,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .ToList();
            return lst;
        }
        public List<SupplierAuditPlanItem> GetAllAccept(int page, int pageSize, out int totalCount)
        {
            var dbo = suppAuditPlanDA.Repository;
            int[] lstPlanId = GetListPlanID();
            var lst = dbo.QualityPlans.Where(i => lstPlanId.Contains(i.ID) && i.IsAccept)
                        .Select(item => new SupplierAuditPlanItem()
                        {
                            ID = dbo.SupplierAuditPlans.FirstOrDefault(ii => ii.QualityPlanID == item.ID).ID,
                            QualityPlanID = item.ID,
                            Name = item.Name,
                            TargetID = item.QualityTargetID,
                            Department = new iDAS.Items.HumanDepartmentViewItem()
                            {
                                ID = item.DepartmentID,
                                Name = dbo.HumanDepartments.FirstOrDefault(d => d.ID == item.DepartmentID).Name
                            },
                            ParentID = item.ParentID,
                            Type = item.Type,
                            //ServiceName = item.ServicePlans.FirstOrDefault().ServiceCommandContract.CustomerContract.CustomerOrders.FirstOrDefault().Service.Name,
                            Cost = item.Cost,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Content = item.Content,
                            IsEdit = item.IsEdit,
                            IsApproval = item.IsApproval,
                            IsAccept = item.IsAccept,
                            ApprovalAt = item.ApprovalAt,
                            ApprovalBy = item.ApprovalBy,
                            IsDelete = item.IsDelete,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();

            return lst;
        }
        
        private int[] GetListPlanID()
        {
            try
            {
                int[] data = suppAuditPlanDA.GetQuery().Select(t => t.QualityPlanID).ToArray();
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public int Insert(QualityPlanItem item)
        {
            return qualityPlanSV.Insert(item);
        }
        public void Update(SupplierAuditPlanItem item, int userID)
        {

            var pI = qualityPlanDA.GetById(item.QualityPlanID);
            pI.Name = item.Name;
            pI.QualityTargetID = item.TargetID;
            pI.DepartmentID = item.Department.ID;
            pI.ParentID = item.ParentID;
            pI.Type = item.Type;
            pI.Content = item.Content;
            pI.Cost = item.Cost;
            pI.StartTime = item.StartTime;
            pI.EndTime = item.EndTime;
            pI.IsApproval = item.IsApproval;
            pI.IsEdit = item.IsEdit;
            pI.IsAccept = item.IsAccept;
            pI.ApprovalAt = item.ApprovalAt;
            pI.ApprovalBy = item.ApprovalBy;
            pI.Note = item.ApprovalNote;
            pI.UpdateAt = DateTime.Now;
            pI.UpdateBy = userID;
            qualityPlanDA.Update(pI);
            qualityPlanDA.Save();
        }

        public void InsertSuppPlan(int planId,int userID)
        {
            var obj = new SupplierAuditPlan();
            obj.QualityPlanID = planId;            
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userID;
            suppAuditPlanDA.Insert(obj);
            suppAuditPlanDA.Save();
        }
        public int InsertSupplierPlan(int planId, int userID)
        {
            var obj = new SupplierAuditPlan();
            obj.QualityPlanID = planId;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userID;
            suppAuditPlanDA.Insert(obj);
            suppAuditPlanDA.Save();
            return obj.ID;
        }
        public void Update(int planId, int userId)
        {            
            var obj = suppAuditPlanDA.GetById(planId);            
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            suppAuditPlanDA.Update(obj);
            suppAuditPlanDA.Save();

        }
        public void Approve(SupplierAuditPlanItem PlanApprovalItem)
        {
            var pl = qualityPlanDA.GetById(PlanApprovalItem.QualityPlanID);
            pl.IsApproval = true;
            pl.IsEdit = PlanApprovalItem.IsEdit;
            pl.ApprovalAt = PlanApprovalItem.ApprovalAt;
            pl.IsAccept = PlanApprovalItem.IsAccept;
            pl.Note = PlanApprovalItem.ApprovalNote;
            qualityPlanDA.Update(pl);
            qualityPlanDA.Save();
        }

        public void Delete(int id)
        {
            var obj = suppAuditPlanDA.GetById(id);
            qualityPlanDA.Delete(obj.QualityPlanID);
            qualityPlanDA.Save();
            suppAuditPlanDA.Delete(id);
            suppAuditPlanDA.Save();
        }

        public List<SupplierAuditPlanItem> GetAllApprove(int page, int pageSize, out int totalCount,DateTime fromDate,DateTime toDate)
        {
            var dbo = suppAuditPlanDA.Repository;
            int[] lstPlanId = GetListPlanID();
            var lst = dbo.QualityPlans.Where(i => lstPlanId.Contains(i.ID) && i.IsAccept)
                .Where(i => (i.StartTime >= fromDate && i.StartTime <= toDate) || (i.EndTime >= fromDate && i.EndTime <= toDate) || (i.StartTime <= fromDate && i.EndTime >= toDate))
                        .Select(item => new SupplierAuditPlanItem()
                        {
                            ID = dbo.SupplierAuditPlans.FirstOrDefault(ii => ii.QualityPlanID == item.ID).ID,
                            QualityPlanID = item.ID,
                            Name = item.Name,
                            TargetID = item.QualityTargetID,
                            Department = new iDAS.Items.HumanDepartmentViewItem()
                            {
                                ID = item.DepartmentID,
                                Name = dbo.HumanDepartments.FirstOrDefault(d => d.ID == item.DepartmentID).Name
                            },                            
                            ParentID = item.ParentID,
                            Type = item.Type,
                            //ServiceName = item.ServicePlans.FirstOrDefault().ServiceCommandContract.CustomerContract.CustomerOrders.FirstOrDefault().Service.Name,
                            Cost = item.Cost,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Content = item.Content,
                            IsEdit = item.IsEdit,
                            IsApproval = item.IsApproval,
                            IsAccept = item.IsAccept,
                            ApprovalAt = item.ApprovalAt,
                            ApprovalBy = item.ApprovalBy,
                            IsDelete = item.IsDelete,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            foreach(var item in lst)
            {
                var lstSuppAudit = dbo.SupplierAuditPlans.FirstOrDefault(ii => ii.ID == item.ID).SupplierAudits;
                item.SupplierAuditCount = lstSuppAudit.Count();
                item.SupplierAuditChecked = lstSuppAudit.Where(x => x.SupplierAuditResults.Count() >0 || x.IsPass==true).Count();
                item.SupplierAuditNotCheck = item.SupplierAuditCount - item.SupplierAuditChecked;
                item.SupplierAuditPassCount = lstSuppAudit.Where(x => x.IsPass.HasValue && x.IsPass.Value == true).Count();
                item.SupplierAuditNotPassCount = item.SupplierAuditChecked - item.SupplierAuditPassCount;
            }
            return lst;
        }
        public List<SupplierAuditPlanItem> GetSuppAuditStatistical(ModelFilter filter, int planID, DateTime fromDate, DateTime toDate)
        {
            var dbo = suppAuditPlanDA.Repository;
            int[] lstPlanId = GetListPlanID();
            var lst = dbo.QualityPlans.Where(i => lstPlanId.Contains(i.ID) && i.IsAccept)
                .Where(i => (i.StartTime >= fromDate && i.StartTime <= toDate) || (i.EndTime >= fromDate && i.EndTime <= toDate) || (i.StartTime <= fromDate && i.EndTime >= toDate))
                        .Select(item => new SupplierAuditPlanItem()
                        {
                            ID = dbo.SupplierAuditPlans.FirstOrDefault(ii => ii.QualityPlanID == item.ID).ID,
                            QualityPlanID = item.ID,
                            Name = item.Name,
                            TargetID = item.QualityTargetID,
                            Department = new iDAS.Items.HumanDepartmentViewItem()
                            {
                                ID = item.DepartmentID,
                                Name = dbo.HumanDepartments.FirstOrDefault(d => d.ID == item.DepartmentID).Name
                            },
                            ParentID = item.ParentID,
                            Type = item.Type,
                            //ServiceName = item.ServicePlans.FirstOrDefault().ServiceCommandContract.CustomerContract.CustomerOrders.FirstOrDefault().Service.Name,
                            Cost = item.Cost,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Content = item.Content,
                            IsEdit = item.IsEdit,
                            IsApproval = item.IsApproval,
                            IsAccept = item.IsAccept,
                            ApprovalAt = item.ApprovalAt,
                            ApprovalBy = item.ApprovalBy,
                            IsDelete = item.IsDelete,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                        .ToList();
            
            return lst;
        }
        
    }
}
