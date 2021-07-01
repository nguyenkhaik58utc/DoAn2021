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
   public class HumanEmployeeAuditSV
    {
       private HumanEmployeeAuditDA auditObjectDA = new HumanEmployeeAuditDA();
        public List<HumanEmployeeAuditItem> GetAll(int page, int pageSize, out int total, int phaseAuditId)
        {
            var dbo = auditObjectDA.Repository;
            var objects = auditObjectDA.GetQuery()
                          .Select(item => new HumanEmployeeAuditItem()
                          {
                              ID = item.ID,
                              HumanPhaseAuditID = item.HumanPhaseAuditID,
                              EmployeeName = item.HumanEmployee.Name,
                              FileID = item.HumanEmployee.AttachmentFileID,
                              FileName = item.HumanEmployee.AttachmentFile.Name,
                              Email = item.HumanEmployee.Email,
                              HumanEmployeeID = item.HumanEmployeeID,
                              Phone = item.HumanEmployee.Phone,
                              Role = item.HumanEmployee.HumanOrganizations.FirstOrDefault() != null ? item.HumanEmployee.HumanOrganizations.FirstOrDefault().HumanRole != null ? item.HumanEmployee.HumanOrganizations.FirstOrDefault().HumanRole.Name : string.Empty : string.Empty,
                              CreateBy = item.CreateBy,
                              NumberCorrect = dbo.HumanResultQuestions.Where(t=>t.HumanEmployeeAuditID==item.ID&&t.IsResult).Count(),
                              NumberUnCorrect = dbo.HumanResultQuestions.Where(t => t.HumanEmployeeAuditID == item.ID && !t.IsResult).Count(),
                              UpdateAt = item.UpdateAt,
                              UpdateBy = item.UpdateBy,
                              CreateAt = item.CreateAt
                          })
                          .OrderByDescending(item => item.CreateAt)
                          .Where(t => t.HumanPhaseAuditID == phaseAuditId)
                          .Page(page, pageSize, out total)
                          .ToList();
            return objects;
        }
        public HumanEmployeeAuditItem GetByID(int id)
        {
            var objects = auditObjectDA.GetById(id);
            HumanEmployeeAuditItem obj = new HumanEmployeeAuditItem();
            obj.ID = objects.ID;
            obj.HumanPhaseAuditID = objects.HumanPhaseAuditID;
            obj.FileID = objects.HumanEmployee.AttachmentFileID;
            obj.FileName = objects.HumanEmployee.AttachmentFile.Name;
            obj.EmployeeName = objects.HumanEmployee.Name;
            obj.Phone = objects.HumanEmployee.Phone;
            obj.Role = objects.HumanEmployee.HumanOrganizations.FirstOrDefault() != null ? objects.HumanEmployee.HumanOrganizations.FirstOrDefault().HumanRole != null ? objects.HumanEmployee.HumanOrganizations.FirstOrDefault().HumanRole.Name : string.Empty : string.Empty;
            obj.Email = objects.HumanEmployee.Email;
            obj.HumanEmployeeID = objects.HumanEmployeeID;
            obj.CreateBy = objects.CreateBy;
            obj.UpdateAt = objects.UpdateAt;
            obj.UpdateBy = objects.UpdateBy;
            obj.CreateAt = objects.CreateAt;
            return obj;
        }
        public int GetByPhaseAuditIDAndEmployeeID(int phaseAuditID, int employeeId)
        {
            var objects = auditObjectDA.GetQuery(t => t.HumanEmployeeID == employeeId && t.HumanPhaseAuditID == phaseAuditID).FirstOrDefault();
            if (objects != null)
                return objects.ID;
            return 0;
        }
        public void Insert(HumanEmployeeAuditItem objNew, int userId)
        {
            HumanEmployeeAudit obj = new HumanEmployeeAudit();
            obj.HumanPhaseAuditID = objNew.HumanPhaseAuditID;
            obj.HumanEmployeeID = objNew.HumanEmployeeID;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            auditObjectDA.Insert(obj);
            auditObjectDA.Save();
        }
        public void InsertRange(List<HumanEmployeeAuditItem> items, int userID)
        {
            var rprs = new List<HumanEmployeeAudit>();
            rprs = items.Select(item => new HumanEmployeeAudit()
            {
                HumanPhaseAuditID = item.HumanPhaseAuditID,
                HumanEmployeeID = item.HumanEmployeeID,
                CreateAt = DateTime.Now,
                CreateBy = userID
            }).ToList();
            auditObjectDA.InsertRange(rprs);
            auditObjectDA.Save();
        }
        public void InsertObject(string stringId, int userId, int phaseAuditId)
        {
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            var dbo = auditObjectDA.Repository;
            List<HumanEmployeeAudit> objectAudit = new List<HumanEmployeeAudit>();
            foreach (var item in ids)
            {
                objectAudit.Add(new HumanEmployeeAudit
                {
                    HumanEmployeeID = (int)item,
                    HumanPhaseAuditID = phaseAuditId,
                    CreateAt = DateTime.Now,
                    CreateBy = userId
                });
            }
            auditObjectDA.InsertRange(objectAudit);
            auditObjectDA.Save();
        }
        public void Delete(int id)
        {
            var obj = auditObjectDA.GetById(id);
            auditObjectDA.Delete(obj);
            auditObjectDA.Save();
        }
    }
}
