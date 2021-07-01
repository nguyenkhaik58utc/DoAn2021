using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;

namespace iDAS.Services
{
  public  class HumanPhaseAuditSV
    {
      private HumanPhaseAuditDA humanPhaseAuditDA = new HumanPhaseAuditDA();
        public List<HumanPhaseAuditItem> GetAllIsApproval(int page, int pageSize, out int total)
        {
            var audits = humanPhaseAuditDA.GetQuery(t => t.IsAccept)
                 .Select(item => new HumanPhaseAuditItem()
                 {
                     ID = item.ID,
                     Name = item.Name,
                     ApprovalAt = item.ApprovalAt,
                     CreateBy = item.CreateBy,
                     StartDate = item.StartDate,
                     IsAccept = item.IsAccept,
                     IsApproval = item.IsApproval,
                     IsEdit = item.IsEdit,
                     Contents = item.Contents,
                     EndDate = item.EndDate,
                     UpdateAt = item.UpdateAt,
                     UpdateBy = item.UpdateBy,
                     CreateAt = item.CreateAt
                 })
                 .OrderByDescending(item => item.CreateAt)
                 .Page(page, pageSize, out total)
                 .ToList();
            return audits;
        }
        public List<HumanPhaseAuditItem> GetAll(ModelFilter filter, int focusId = 0)
        {
            var dbo = humanPhaseAuditDA.Repository;
            IQueryable<iDAS.Base.HumanPhaseAudit> query = dbo.HumanPhaseAudits;
            if (focusId != 0)
            {
                filter.SortName = null;
                query = query.OrderBy(i => i.ID == focusId ? false : true);
            }
            var audits = query
                 .Filter(filter)
                 .Select(item => new HumanPhaseAuditItem()
                 {
                     ID = item.ID,
                     Name = item.Name,
                     ApprovalAt = item.ApprovalAt,
                     CreateBy = item.CreateBy,
                     StartDate = item.StartDate,
                     IsAccept = item.IsAccept,
                     IsApproval = item.IsApproval,
                     IsEdit = item.IsEdit,
                     Contents = item.Contents,
                     EndDate = item.EndDate,
                     UpdateAt = item.UpdateAt,
                     UpdateBy = item.UpdateBy,
                     CreateAt = item.CreateAt
                 })
                 .ToList();
            return audits;
        }
        public List<HumanPhaseAuditItem> GetIsApproval(ModelFilter filter)
        {
            var dbo = humanPhaseAuditDA.Repository;
            var audits = dbo.HumanPhaseAudits.Where(t=>t.IsAccept && t.IsApproval)
                 .OrderByDescending(t=>t.CreateAt)
                 .Filter(filter)
                 .Select(item => new HumanPhaseAuditItem()
                 {
                     ID = item.ID,
                     Name = item.Name,
                     ApprovalAt = item.ApprovalAt,
                     CreateBy = item.CreateBy,
                     HumanCategoryQuestionID = item.HumanCategoryQuestionID,
                     StartDate = item.StartDate,
                     IsAccept = item.IsAccept,
                     IsApproval = item.IsApproval,
                     IsEdit = item.IsEdit,
                     Contents = item.Contents,
                     EndDate = item.EndDate,
                     UpdateAt = item.UpdateAt,
                     UpdateBy = item.UpdateBy,
                     CreateAt = item.CreateAt
                 })
                 .ToList();
            return audits;
        }
        public List<HumanPhaseAuditItem> GetByEmployeeID(ModelFilter filter, int employeeId, int focusId)
        {
            var dbo = humanPhaseAuditDA.Repository;
             IQueryable<iDAS.Base.HumanPhaseAudit> query =  dbo.HumanPhaseAudits.Where(t => t.IsAccept && t.IsApproval && t.HumanEmployeeAudits.Where(x=>x.HumanEmployeeID==employeeId).Count()>0)
            .OrderByDescending(t => t.CreateAt);
            if (focusId != 0)
            {
                filter.SortName = null;
                query = query.OrderBy(i => i.ID == focusId ? false : true);
            }
            var audits = query
                 .Filter(filter)
                 .Select(item => new HumanPhaseAuditItem()
                 {
                     ID = item.ID,
                     Name = item.Name,
                     ApprovalAt = item.ApprovalAt,
                     CreateBy = item.CreateBy,
                     HumanCategoryQuestionID = item.HumanCategoryQuestionID,
                     StartDate = item.StartDate,
                     IsAccept = item.IsAccept,
                     IsApproval = item.IsApproval,
                     IsEdit = item.IsEdit,
                     Contents = item.Contents,
                     EndDate = item.EndDate,
                     UpdateAt = item.UpdateAt,
                     UpdateBy = item.UpdateBy,
                     CreateAt = item.CreateAt
                 })
                 .ToList();
            return audits;
        }
        public List<int> GetEmployeeAudit(int id)
        {
            var dbo = humanPhaseAuditDA.Repository;
            var data = dbo.HumanEmployeeAudits.Where(t => t.HumanPhaseAuditID == id).Select(t => t.HumanEmployeeID.Value).ToList();
            return data;
        }
        public void SendApprove(HumanPhaseAuditItem objEdit, int userId)
        {
            HumanPhaseAudit obj = humanPhaseAuditDA.GetById(objEdit.ID);
            obj.ApprovalBy = objEdit.Approval.ID;
            obj.IsEdit = false;
            obj.IsApproval = false;
            humanPhaseAuditDA.Update(obj);
            humanPhaseAuditDA.Save();

        }
        public void Approve(HumanPhaseAuditItem objEdit, int userId)
        {
            HumanPhaseAudit obj = humanPhaseAuditDA.GetById(objEdit.ID);
            obj.IsAccept = objEdit.IsAccept;
            obj.IsApproval = true;
            obj.ApprovalNote = objEdit.ApprovalNote;
            obj.ApprovalAt = DateTime.Now;
            obj.IsEdit = objEdit.IsEdit;
            humanPhaseAuditDA.Update(obj);
            humanPhaseAuditDA.Save();

        }
        public HumanPhaseAuditItem GetByID(int id)
        {
            HumanPhaseAuditItem obj = new HumanPhaseAuditItem();
            var employeeSV = new HumanEmployeeSV();
            var dbo = humanPhaseAuditDA.Repository;
            var phaseAudit = humanPhaseAuditDA.GetById(id);
            if (phaseAudit != null)
            {
                obj.ID = phaseAudit.ID;
                obj.ApprovalNote = phaseAudit.ApprovalNote;
                obj.Name = phaseAudit.Name;
                obj.Contents = phaseAudit.Contents;
                obj.HumanCategoryQuestionID = phaseAudit.HumanCategoryQuestionID;
                obj.IsEdit = phaseAudit.IsEdit;
                obj.NumberSendInDay = phaseAudit.NumberSendInDay;
                obj.Approval = employeeSV.GetEmployeeView(phaseAudit.ApprovalBy);
                obj.StartDate = phaseAudit.StartDate;
                obj.IsAccept = phaseAudit.IsAccept;
                obj.CreateByEmployeeID = phaseAudit.CreateBy.HasValue && dbo.HumanUsers.FirstOrDefault(o => o.ID == phaseAudit.CreateBy) != null ? dbo.HumanUsers.FirstOrDefault(o => o.ID == phaseAudit.CreateBy).HumanEmployeeID : null;
                obj.UpdateByEmployeeID = phaseAudit.UpdateBy.HasValue && dbo.HumanUsers.FirstOrDefault(o => o.ID == phaseAudit.UpdateBy) != null ? dbo.HumanUsers.FirstOrDefault(o => o.ID == phaseAudit.UpdateBy).HumanEmployeeID : null;
                obj.IsApproval = phaseAudit.IsApproval;
                obj.UpdateBy = phaseAudit.UpdateBy;
                obj.CreateBy = phaseAudit.CreateBy;
                obj.EndDate = phaseAudit.EndDate;
                obj.IsDelete = phaseAudit.IsDelete;
            }
            return obj;
        }
        public bool CheckNameExits(string name)
        {
            var rs = humanPhaseAuditDA.GetQuery(t => t.Name.ToUpper() == name.ToUpper()).ToList();
            if (rs.Count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int Insert(HumanPhaseAuditItem objNew, int userId)
        {
            var obj = new HumanPhaseAudit();
            obj.Name = objNew.Name.Trim();
            obj.StartDate = objNew.StartDate;
            obj.HumanCategoryQuestionID = objNew.HumanCategoryQuestionID;
            obj.EndDate = objNew.EndDate;
            obj.NumberSendInDay = objNew.NumberSendInDay;
            obj.ApprovalBy = objNew.Approval.ID;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            obj.IsAccept = false;
            obj.IsApproval = false;
            obj.IsDelete = false;
            obj.IsEdit = objNew.IsEdit;
            obj.Contents = objNew.Contents.Trim();
            humanPhaseAuditDA.Insert(obj);
            humanPhaseAuditDA.Save();
            return obj.ID;

        }
        public void Update(HumanPhaseAuditItem objEdit,  int userId)
        {
            var obj = humanPhaseAuditDA.GetById(objEdit.ID);
            obj.Name = objEdit.Name.Trim();
            obj.StartDate = objEdit.StartDate;
            obj.EndDate = objEdit.EndDate;
            obj.NumberSendInDay = objEdit.NumberSendInDay;
            obj.IsApproval = objEdit.IsApproval;
            obj.ApprovalBy = objEdit.Approval.ID;
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            obj.IsEdit = objEdit.IsEdit;
            obj.Contents = objEdit.Contents.Trim();
            humanPhaseAuditDA.Update(obj);
            humanPhaseAuditDA.Save();
        }
        public void Delete(string stringId)
        {
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            humanPhaseAuditDA.DeleteRange(ids);
            humanPhaseAuditDA.Save();
        }

    }
}
