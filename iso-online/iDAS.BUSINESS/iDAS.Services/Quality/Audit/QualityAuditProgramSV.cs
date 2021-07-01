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
    public class QualityAuditProgramSV
    {
        private QualityAuditProgramDA auditProgramDA = new QualityAuditProgramDA();
        private CenterISOIndexCriteriaDA centerISOIndexCriteriaDA = new CenterISOIndexCriteriaDA();
        public void SendApproveProgram(QualityAuditProgramItem objEdit, int userId)
        {
            QualityAuditProgram obj = auditProgramDA.GetById(objEdit.ID);
            obj.ApprovalBy = objEdit.Approval.ID;
            obj.IsEdit = false;
            obj.IsApproval = false;
            obj.IsAccept = obj.IsAccept;
            auditProgramDA.Update(obj);
            auditProgramDA.Save();
        }
        public void Approve(QualityAuditProgramItem objEdit, int userId)
        {
            QualityAuditProgram obj = auditProgramDA.GetById(objEdit.ID);
            obj.IsAccept = objEdit.IsAccept;
            obj.IsApproval = true;
            obj.ApprovalNote = objEdit.ApprovalNote;
            obj.ApprovalAt = DateTime.Now;
            obj.IsEdit = objEdit.IsEdit;
            auditProgramDA.Update(obj);
            auditProgramDA.Save();
        }
        private string GetListDepartment(int qualityAuditProgramId)
        {
            var dbo = auditProgramDA.Repository;
            string lst = "";
            var no = 1;
            var data = dbo.QualityAuditProgramDepartments.Where(t => t.QualityAuditProgramID == qualityAuditProgramId).Select(t => t.HumanDepartment.Name).ToList();
            foreach (var item in data)
            {
                lst += no + ". " + item + "<br/>";
                no++;
            }
            return lst;
        }
        private string GetListISOIndexAudit(int qualityAuditProgramId)
        {
            var dbo = auditProgramDA.Repository;
            var dboCenter = centerISOIndexCriteriaDA.Repository;
            string lst = "";
            var isoIndexs = dbo.QualityAuditProgramISOes.Where(t => t.QualityAuditProgramID == qualityAuditProgramId).Select(t => t.ISOIndexID).ToArray();
            if (isoIndexs.Count() > 0)
            {
                var data = dboCenter.ISOIndexes.Where(t => isoIndexs.Contains(t.ID) && t.IsActive && !t.IsDelete).Select(t => t.Clause + " " + t.Name).ToList();
                foreach (var item in data)
                {
                    lst += item + "<br/>";
                }

            }
            return lst;
        }
        private string GetListEmployee(int qualityAuditProgramId)
        {
            var dbo = auditProgramDA.Repository;
            string lst = "";
            var data = dbo.QualityAuditProgramEmployees.Where(p => p.QualityAuditProgramID == qualityAuditProgramId)
                .Select(t => t.HumanEmployee.Name).ToList();
            lst += "<b>Thành phần tham gia:</b><br/>";
            foreach (var item in data)
            {
                lst += item + "<br/>";
            }
            return lst;
        }
        private string GetListModuleAudit(int qualityAuditProgramId)
        {
            var dbo = auditProgramDA.Repository;
            var dboCenter = centerISOIndexCriteriaDA.Repository;
            string lstModule = "";
            var no = 1;
            var isoIndexs = dbo.QualityAuditProgramISOes.Where(t => t.QualityAuditProgramID == qualityAuditProgramId).Select(t => t.ISOIndexID).ToArray();
            if (isoIndexs.Count() > 0)
            {
                var module = dboCenter.ISOIndexModules.Where(t => isoIndexs.Contains(t.ISOIndexID) && !t.IsDelete && t.IsUse && t.CenterModule.SystemCode.Equals("BUSINESS")).Select(t => t.CenterModule.Name).Distinct().ToList();
                foreach (var item in module)
                {
                    lstModule += no + ". " + item + "<br/>";
                    no++;
                }
            }
            return lstModule;
        }
        public List<QualityAuditProgramItem> GetAll(ModelFilter filter, int auditPlanId, int focusId)
        {
            var dbo = auditProgramDA.Repository;
            List<QualityAuditProgramItem> lst = new List<QualityAuditProgramItem>();
            IQueryable<iDAS.Base.QualityAuditProgram> query = dbo.QualityAuditPrograms
                        .Where(t => t.QualityAuditPlanID == auditPlanId || (auditPlanId == 0 && t.ID == focusId));
            if (focusId != 0)
            {
                filter.SortName = null;
                query = query.OrderBy(i => i.ID == focusId ? false : true);
            }
            var objects = query.OrderBy(i => i.StartTime)
                        .Filter(filter)
                          .ToList();
            if (objects.Count() > 0)
            {
                foreach (var item in objects)
                {
                    lst.Add(new QualityAuditProgramItem()
                    {
                        ID = item.ID,
                        AuditPlanID = item.QualityAuditPlanID,
                        Note = item.Note,
                        Address = item.Address,
                        Content = item.Content,
                        DatePerform = ((DateTime)item.StartTime),
                        strDatePerform = "Ngày thứ " + (item.StartTime.Value.Date.Subtract(item.QualityAuditPlan.StartTime.Date).Days + 1).ToString()
                                            + " (" + item.StartTime.Value.ToString("dd/MM/yyy") + ")",
                        strTimePerform = ((DateTime)item.StartTime).ToString("HH:mm") + " - " + ((DateTime)item.EndTime).ToString("HH:mm"),
                        StartTime = (DateTime)item.StartTime,
                        EndTime = (DateTime)item.EndTime,
                        ApprovalNote = item.ApprovalNote,
                        IsAccept = item.IsAccept,
                        IsApproval = item.IsApproval,
                        IsEdit = item.IsEdit,
                        IsNew = item.IsNew,
                        Name = item.Name,
                        CreateBy = item.CreateBy,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy,
                        CreateAt = item.CreateAt,
                        ListDepartment = GetListDepartment(item.ID),
                        ListISOIndexAudit = GetListISOIndexAudit(item.ID),
                        ListEmployee = GetListEmployee(item.ID),
                        ListModuleAudit = GetListModuleAudit(item.ID)
                    });
                }
            }
            return lst;
        }
        public QualityAuditProgramItem GetByID(int id)
        {
            var employeeSV = new HumanEmployeeSV();
            var dbo = auditProgramDA.Repository;
            var program = auditProgramDA.GetById(id);
            QualityAuditProgramItem obj = new QualityAuditProgramItem();
            obj.ID = program.ID;
            obj.StartTime = (DateTime)program.StartTime;
            obj.EndTime = (DateTime)program.EndTime;
            obj.Content = program.Content;
            obj.Address = program.Address;
            obj.CreateBy = program.CreateBy;
            obj.AuditPlanID = program.QualityAuditPlanID;
            obj.Approval = employeeSV.GetEmployeeView(program.ApprovalBy);
            obj.Create = employeeSV.GetEmployeeView(program.CreateBy);
            obj.CreateByEmployeeID = program.CreateBy.HasValue && dbo.HumanUsers.FirstOrDefault(o => o.ID == program.CreateBy) != null ? dbo.HumanUsers.FirstOrDefault(o => o.ID == program.CreateBy.Value).HumanEmployeeID : null;
            obj.Note = program.Note;
            obj.ApprovalNote = program.ApprovalNote;
            obj.IsAccept = program.IsAccept;
            obj.ApprovalBy = (int)program.ApprovalBy;
            obj.IsApproval = program.IsApproval;
            obj.IsEdit = program.IsEdit;
            obj.IsNew = program.IsNew;
            obj.Name = program.Name;
            obj.UpdateAt = program.UpdateAt;
            obj.UpdateBy = program.UpdateBy;
            obj.CreateAt = program.CreateAt;
            return obj;
        }
        public int InsertProgram(QualityAuditProgramItem objNew, int userId)
        {
            QualityAuditProgram obj = new QualityAuditProgram();
            obj.QualityAuditPlanID = objNew.AuditPlanID;
            obj.Content = objNew.Content;
            obj.EndTime = objNew.EndTime;
            obj.StartTime = objNew.StartTime;
            obj.Address = objNew.Address;
            obj.Note = objNew.Note;
            obj.IsAccept = false;
            obj.IsApproval = false;
            obj.ApprovalBy = objNew.Approval.ID;
            obj.IsEdit = true;
            obj.IsNew = true;
            obj.Name = objNew.Name;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            auditProgramDA.Insert(obj);
            auditProgramDA.Save();
            return obj.ID;
        }
        public void Update(QualityAuditProgramItem objEdit, int userId)
        {
            QualityAuditProgram obj = auditProgramDA.GetById(objEdit.ID);
            obj.Note = objEdit.Note;
            obj.Address = objEdit.Address;
            obj.Content = objEdit.Content;
            obj.EndTime = objEdit.EndTime;
            obj.StartTime = objEdit.StartTime;
            obj.Name = objEdit.Name;
            obj.ApprovalBy = objEdit.Approval.ID;
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            auditProgramDA.Update(obj);
            auditProgramDA.Save();
        }
        public void Delete(int id)
        {
            var dbo = auditProgramDA.Repository;
            dbo.QualityAuditProgramEmployees.RemoveRange(dbo.QualityAuditProgramEmployees.Where(t => t.QualityAuditProgramID == id));
            dbo.QualityAuditProgramDepartments.RemoveRange(dbo.QualityAuditProgramDepartments.Where(t => t.QualityAuditProgramID == id));
            dbo.QualityAuditProgramISOes.RemoveRange(dbo.QualityAuditProgramISOes.Where(t => t.QualityAuditProgramID == id));
            dbo.SaveChanges();
            var obj = auditProgramDA.GetById(id);
            auditProgramDA.Delete(obj);
            auditProgramDA.Save();
        }
        public List<QualityAuditRecordedVoteItem> GetRecordedVotes(ModelFilter filter, int programId)
        {
            var dbo = auditProgramDA.Repository;
            var objects = dbo.QualityAuditRecordedVotes
                        .Where(t => t.QualityAuditProgramID == programId)
                        .Filter(filter)
                          .Select(item => new QualityAuditRecordedVoteItem()
                          {
                              ID = item.ID,
                              HumanDepartmentName = item.HumanDepartment.Name,
                              AuditName = dbo.HumanEmployees.Where(t => t.ID == item.AuditBy).FirstOrDefault().Name,
                              AuditAt = item.AuditAt,
                              Contents = item.Contents,
                              CreateBy = item.CreateBy,
                              UpdateAt = item.UpdateAt,
                              UpdateBy = item.UpdateBy,
                              CreateAt = item.CreateAt
                          })
                          .ToList();
            return objects;
        }
    }
}
