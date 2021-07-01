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
    public class QualityAuditPlanSV
    {

        private QualityAuditPlanDA auditDA = new QualityAuditPlanDA();
        private CenterISOStandardDA iSODA = new CenterISOStandardDA();
        private ISOStandardSV iSOService = new ISOStandardSV();
        public List<QualityAuditPlanItem> GetAllIsApproval(int page, int pageSize, out int total)
        {
            List<QualityAuditPlanItem> lst = new List<QualityAuditPlanItem>();
            var audits = auditDA.GetQuery(t => t.IsAccept).OrderByDescending(item => item.CreateAt)
            .Page(page, pageSize, out total)
            .ToList();
            if (audits.Count > 0)
            {
                foreach (var item in audits)
                {
                    lst.Add(new QualityAuditPlanItem
                    {
                        ID = item.ID,
                        Name = item.Name,
                        ApprovalBy = item.ApporvalBy,
                        ApprovalAt = item.ApprovalAt,
                        Code = item.Code,
                        ISOID = item.ISOID,
                        ISOName = iSOService.GetNameByID(item.ISOID),
                        CreateBy = item.CreateBy,
                        EndTime = item.EndTime,
                        IsAccept = item.IsAccept,
                        IsApproval = item.IsApproval,
                        IsEdit = item.IsEdit,
                        IsNew = item.IsNew,
                        IsPass = item.IsPass,
                        AuditNote = item.AuditNote,
                        Note = item.Note,
                        Require = item.Requirement,
                        Scope = item.Scope,
                        StartTime = item.StartTime,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy,
                        CreateAt = item.CreateAt
                    });
                }
            }
            return lst;
        }
        public List<QualityAuditPlanItem> GetAll(ModelFilter filter, int focusId = 0)
        {
            List<QualityAuditPlanItem> lst = new List<QualityAuditPlanItem>();
            var dbo = auditDA.Repository;
            IQueryable<iDAS.Base.QualityAuditPlan> query = dbo.QualityAuditPlans;
            if (focusId != 0)
            {
                filter.SortName = null;
                query = query.OrderBy(i => i.ID == focusId ? false : true);
            }
            var audits = query.Filter(filter)
            .ToList();
            if (audits.Count > 0)
            {
                foreach (var item in audits)
                {
                    lst.Add(new QualityAuditPlanItem
                    {
                        ID = item.ID,
                        Name = item.Name,
                        ApprovalBy = item.ApporvalBy,
                        ApprovalAt = item.ApprovalAt,
                        Code = item.Code,
                        ISOID = item.ISOID,
                        ISOName = iSOService.GetNameByID(item.ISOID),
                        CreateBy = item.CreateBy,
                        EndTime = item.EndTime,
                        IsAccept = item.IsAccept,
                        IsApproval = item.IsApproval,
                        IsEdit = item.IsEdit,
                        IsNew = item.IsNew,
                        Note = item.Note,
                        Require = item.Requirement,
                        Scope = item.Scope,
                        StartTime = item.StartTime,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy,
                        CreateAt = item.CreateAt
                    });
                }
            }
            return lst;
        }
        public void Approve(QualityAuditPlanItem objEdit, int userId)
        {
            QualityAuditPlan obj = auditDA.GetById(objEdit.ID);
            obj.IsAccept = objEdit.IsAccept;
            obj.IsApproval = true;
            obj.Note = objEdit.ApprovalNote;
            obj.ApprovalAt = DateTime.Now;
            obj.IsEdit = objEdit.IsEdit;
            auditDA.Update(obj);
            auditDA.Save();
        }
        public void SendApprove(QualityAuditPlanItem objEdit, int userId)
        {
            QualityAuditPlan obj = auditDA.GetById(objEdit.ID);
            obj.ApporvalBy = objEdit.Approval.ID;
            obj.IsEdit = false;
            obj.IsApproval = false;
            obj.IsAccept = obj.IsAccept;
            auditDA.Update(obj);
            auditDA.Save();
        }
        public QualityAuditPlanItem GetById(int id)
        {
            var dbo = auditDA.Repository;
            var employeeSV = new HumanEmployeeSV();
            var item = auditDA.GetById(id);
            QualityAuditPlanItem obj = new QualityAuditPlanItem();
            obj.ID = item.ID;
            obj.Name = item.Name;
            obj.ApprovalBy = item.ApporvalBy;
            obj.ApprovalAt = item.ApprovalAt;
            obj.Code = item.Code;
            obj.CreateBy = item.CreateBy;
            obj.CreateByEmployeeID = item.CreateBy.HasValue && dbo.HumanUsers.FirstOrDefault(o => o.ID == item.CreateBy) != null ? dbo.HumanUsers.FirstOrDefault(o => o.ID == item.CreateBy.Value).HumanEmployeeID : null;
            obj.Approval = employeeSV.GetEmployeeView(item.ApporvalBy);
            obj.Create = employeeSV.GetEmployeeView(item.CreateBy);
            obj.EndTime = item.EndTime;
            obj.IsAccept = item.IsAccept;
            obj.IsApproval = item.IsApproval;
            obj.IsEdit = item.IsEdit;
            obj.IsNew = item.IsNew;
            obj.Note = item.Note;
            obj.ISOID = item.ISOID;
            obj.Require = item.Requirement;
            obj.Scope = item.Scope;
            obj.StartTime = item.StartTime;
            obj.UpdateAt = item.UpdateAt;
            obj.UpdateBy = item.UpdateBy;
            obj.CreateAt = item.CreateAt;
            obj.Note = item.Note;
            obj.ApprovalNote = item.Note;
            obj.CreateByName = dbo.HumanUsers.FirstOrDefault(u => u.ID == item.CreateBy).HumanEmployee.Name;
            return obj;
        }
        public QualityAuditPlanItem GetResultISO(int id)
        {
            var dbo = auditDA.Repository;

            var employeeSV = new HumanEmployeeSV();
            var item = auditDA.GetById(id);
            var ncs = dbo.QualityAuditResults.Where(t => t.QualityAuditProgramISO.QualityAuditProgram.QualityAuditPlanID == id && t.QualityNCID.HasValue).Select(t => t.QualityNCID).ToArray();
            QualityAuditPlanItem obj = new QualityAuditPlanItem();
            obj.ID = item.ID;
            obj.IsPass = item.IsPass;
            obj.AuditNote = item.AuditNote;
            obj.NumberISOIndexPass = dbo.QualityAuditProgramISOes.Where(t => t.QualityAuditProgram.QualityAuditPlanID == id && t.IsPass && t.AuditAt.HasValue && t.AuditBy.HasValue).Count();
            obj.NumberISOIndexNotPass = dbo.QualityAuditProgramISOes.Where(t => t.QualityAuditProgram.QualityAuditPlanID == id && !t.IsPass && t.AuditAt.HasValue && t.AuditBy.HasValue).Count();
            obj.NumberISOIndexNotAudit = dbo.QualityAuditProgramISOes.Where(t => t.QualityAuditProgram.QualityAuditPlanID == id && t.QualityAuditProgram.IsAccept && t.QualityAuditProgram.IsApproval && !t.AuditAt.HasValue && !t.AuditBy.HasValue).Count();
            obj.TotalISOIndexAudit = dbo.QualityAuditProgramISOes.Where(t => t.QualityAuditProgram.QualityAuditPlanID == id && t.QualityAuditProgram.IsAccept && t.QualityAuditProgram.IsApproval).Count();
            if (ncs.Count() > 0)
            {
                obj.NumberM = dbo.QualityNCs.Where(t => ncs.Contains(t.ID) && t.IsMaximum).Count();
                obj.NumberMedium = dbo.QualityNCs.Where(t => ncs.Contains(t.ID) && t.IsMedium).Count();
                obj.NumberObs = dbo.QualityNCs.Where(t => ncs.Contains(t.ID) && t.IsObs).Count();
            }
            return obj;
        }
        public void Insert(QualityAuditPlanItem objNew, int userId)
        {

            QualityAuditPlan obj = new QualityAuditPlan();
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            obj.Name = objNew.Name;
            obj.ApporvalBy = objNew.Approval.ID;
            obj.Code = objNew.Code;
            obj.StartTime = objNew.StartTime;
            obj.EndTime = objNew.EndTime;
            obj.ISOID = objNew.ISOID;
            obj.IsAccept = objNew.IsAccept;
            obj.IsApproval = objNew.IsApproval;
            obj.IsEdit = true;
            obj.IsNew = true;
            obj.Note = objNew.Note;
            obj.Requirement = objNew.Require;
            obj.Scope = objNew.Scope;
            auditDA.Insert(obj);
            auditDA.Save();
        }
        public void Delete(string stringId)
        {
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            auditDA.DeleteRange(ids);
            auditDA.Save();
        }
        public void Update(QualityAuditPlanItem objEdit, int userId)
        {
            QualityAuditPlan obj = auditDA.GetById(objEdit.ID);
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            obj.Name = objEdit.Name;
            obj.ISOID = objEdit.ISOID;
            obj.ApporvalBy = objEdit.Approval.ID;
            obj.Code = objEdit.Code;
            obj.StartTime = objEdit.StartTime;
            obj.EndTime = objEdit.EndTime;
            obj.IsAccept = objEdit.IsAccept;
            obj.IsApproval = objEdit.IsApproval;
            obj.IsNew = obj.IsNew;
            obj.Note = objEdit.Note;
            obj.Requirement = objEdit.Require;
            obj.Scope = objEdit.Scope;
            auditDA.Update(obj);
            auditDA.Save();
        }
        public void UpdateResultISO(QualityAuditPlanItem objEdit, int userId, int employeeID)
        {
            QualityAuditPlan obj = auditDA.GetById(objEdit.ID);
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            obj.IsPass = objEdit.IsPass;
            obj.AuditNote = objEdit.AuditNote;
            auditDA.Update(obj);
            auditDA.Save();
        }
        public bool CheckCodeExits(string code)
        {
            var rs = auditDA.GetQuery(t => t.Code.ToUpper() == code.ToUpper()).ToList();
            if (rs.Count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CheckCodeExitEdits(int id, string code)
        {
            var rs = auditDA.GetQuery().Where(t => t.Code.ToUpper() == code.ToUpper() && t.ID != id).ToList();
            if (rs.Count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
