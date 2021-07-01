using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;
using iDAS.DA;
using iDAS.Items;
namespace iDAS.Services
{
    public class QualityAuditEmployeeSV
    {
        private QualityAuditEmployeeDA auditObjectDA = new QualityAuditEmployeeDA();
        public List<QualityAuditEmployeeItem> GetAll(int page, int pageSize, out int total, int auditProgramId)
        {
            var objects = auditObjectDA.GetQuery()
                          .Select(item => new QualityAuditEmployeeItem()
                          {
                              ID = item.ID,
                              AuditProgramID = item.QualityAuditProgramID,
                              EmployeeName = item.HumanEmployee.Name,
                              FileID = item.HumanEmployee.AttachmentFileID,
                              FileName = item.HumanEmployee.AttachmentFile.Name,
                              Email = item.HumanEmployee.Email,
                              EmployeeID = item.HumanEmployeeID,
                              IsAuditor = item.IsAuditor,
                              IsCaptain = item.IsCaptain,
                              Phone = item.HumanEmployee.Phone,
                              Role = item.HumanEmployee.HumanOrganizations.FirstOrDefault() != null ? item.HumanEmployee.HumanOrganizations.FirstOrDefault().HumanRole != null ? item.HumanEmployee.HumanOrganizations.FirstOrDefault().HumanRole.Name : string.Empty : string.Empty,
                              CreateBy = item.CreateBy,
                              IsAbsent = item.IsAbsent,
                              AbsentReason = item.AbsentReason,
                              UpdateAt = item.UpdateAt,
                              UpdateBy = item.UpdateBy,
                              CreateAt = item.CreateAt
                          })
                          .OrderByDescending(item => item.CreateAt)
                          .Where(t => t.AuditProgramID == auditProgramId)
                          .Page(page, pageSize, out total)
                          .ToList();
            return objects;
        }
        public QualityAuditEmployeeItem GetByID(int id)
        {
            var objects = auditObjectDA.GetById(id);
            QualityAuditEmployeeItem obj = new QualityAuditEmployeeItem();
            obj.ID = objects.ID;
            obj.AuditProgramID = objects.QualityAuditProgramID;
            obj.FileID = objects.HumanEmployee.AttachmentFileID;
            obj.FileName =objects.HumanEmployee.AttachmentFile!=null? objects.HumanEmployee.AttachmentFile.Name:"";
            obj.EmployeeName = objects.HumanEmployee.Name;
            obj.Phone = objects.HumanEmployee.Phone;
            obj.Role = objects.HumanEmployee.HumanOrganizations.FirstOrDefault() != null ? objects.HumanEmployee.HumanOrganizations.FirstOrDefault().HumanRole != null ? objects.HumanEmployee.HumanOrganizations.FirstOrDefault().HumanRole.Name : string.Empty : string.Empty;
            obj.Email = objects.HumanEmployee.Email;
            obj.EmployeeID = objects.HumanEmployeeID;
            obj.IsAuditor = objects.IsAuditor;
            obj.IsAbsent = objects.IsAbsent;
            obj.AbsentReason = objects.AbsentReason;
            obj.IsCaptain = objects.IsCaptain;
            obj.CreateBy = objects.CreateBy;
            obj.UpdateAt = objects.UpdateAt;
            obj.UpdateBy = objects.UpdateBy;
            obj.CreateAt = objects.CreateAt;
            return obj;
        }
        public void InsertRange(List<QualityAuditEmployeeItem> items, int userID)
        {
            var rprs = new List<QualityAuditProgramEmployee>();
            rprs = items.Select(item => new QualityAuditProgramEmployee()
            {
                QualityAuditProgramID = item.AuditProgramID,
                HumanEmployeeID = item.EmployeeID,
                IsAuditor = item.IsAuditor,
                IsCaptain = item.IsCaptain,
                CreateAt = DateTime.Now,
                CreateBy = userID
            }).ToList();
            auditObjectDA.InsertRange(rprs);
            auditObjectDA.Save();
        }
        public void UpdateRange(List<QualityAuditEmployeeItem> items, int programId, int userID)
        {
            var dbo = auditObjectDA.Repository;
            var employes = items.Select(t => t.EmployeeID).ToArray();
            var delete = dbo.QualityAuditProgramEmployees.Where(t => t.QualityAuditProgramID == programId && !employes.Contains(t.HumanEmployeeID)).ToList();
            if (delete.Count > 0)
            {
                dbo.QualityAuditProgramEmployees.RemoveRange(delete);
                dbo.SaveChanges();
            }
            var rprs = new List<QualityAuditProgramEmployee>();
            foreach (var item in items)
            {
                if (!dbo.QualityAuditProgramEmployees.Any(t => t.QualityAuditProgramID == programId && t.HumanEmployeeID == item.EmployeeID))
                {
                    var rpr = new QualityAuditProgramEmployee();
                    rpr.QualityAuditProgramID = item.AuditProgramID;
                    rpr.HumanEmployeeID = item.EmployeeID;
                    rpr.IsAuditor = item.IsAuditor;
                    rpr.IsCaptain = item.IsCaptain;
                    rpr.CreateAt = DateTime.Now;
                    rpr.CreateBy = userID;
                    rprs.Add(rpr);
                }
                else
                {
                    var objedit = dbo.QualityAuditProgramEmployees.Where(t => t.QualityAuditProgramID == programId && t.HumanEmployeeID == item.EmployeeID).FirstOrDefault();
                    objedit.IsAuditor = item.IsAuditor;
                    objedit.IsCaptain = item.IsCaptain;
                    objedit.UpdateAt = DateTime.Now;
                    objedit.UpdateBy = userID;
                    auditObjectDA.Update(objedit);
                    auditObjectDA.Save();
                }
            }
            auditObjectDA.InsertRange(rprs);
            auditObjectDA.Save();
        }
        public void Update(QualityAuditEmployeeItem objEdit, int userId)
        {
            var obj = auditObjectDA.GetById(objEdit.ID);
            if (obj != null)
            {
                obj.IsAbsent = objEdit.IsAbsent;
                obj.AbsentReason = objEdit.AbsentReason;
                obj.UpdateAt = DateTime.Now;
                obj.UpdateBy = userId;
                auditObjectDA.Update(obj);
                auditObjectDA.Save();
            }
        }

    }
}
