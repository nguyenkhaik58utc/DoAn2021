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
    public class HumanAuditDepartmentSV
    {
        private HumanAuditDepartmentDA auditObjectDA = new HumanAuditDepartmentDA();
        public List<HumanAuditDepartmentItem> GetAll(int page, int pageSize, out int total, int phaseAuditId)
        {
            var dbo = auditObjectDA.Repository;
            var objects = auditObjectDA.GetQuery()
                          .Select(item => new HumanAuditDepartmentItem()
                          {
                              ID = item.ID,
                              HumanAuditGradationID = item.HumanAuditGradationID,
                              HumanDepartmentName = item.HumanDepartment.Name,
                              HumanDepartmentID = item.HumanDepartmentID,
                              EmployeeAuditManagementID = item.EmployeeAuditManagementID,
                              EmployeeAuditLeaderID = item.EmployeeAuditLeaderID,
                              EmployeeAuditLeaderName = dbo.HumanEmployees.Where(t => t.ID == item.EmployeeAuditLeaderID).FirstOrDefault().Name,
                              EmployeeAuditManagementName = dbo.HumanEmployees.Where(t => t.ID == item.EmployeeAuditManagementID).FirstOrDefault().Name,
                              CreateBy = item.CreateBy,
                              UpdateAt = item.UpdateAt,
                              UpdateBy = item.UpdateBy,
                              CreateAt = item.CreateAt
                          })
                          .OrderByDescending(item => item.CreateAt)
                          .Where(t => t.HumanAuditGradationID == phaseAuditId)
                          .Page(page, pageSize, out total)
                          .ToList();
            return objects;
        }
        public HumanAuditDepartmentItem GetByID(int id)
        {
            var dbo = auditObjectDA.Repository;
            var objects = auditObjectDA.GetById(id);
            HumanAuditDepartmentItem obj = new HumanAuditDepartmentItem();
            obj.ID = objects.ID;
            obj.HumanAuditGradationID = objects.HumanAuditGradationID;
            obj.HumanDepartmentName = objects.HumanDepartment.Name;
            obj.HumanDepartmentID = objects.HumanDepartmentID;
            obj.EmployeeAuditLeaderName = dbo.HumanEmployees.Where(t => t.ID == objects.EmployeeAuditLeaderID).FirstOrDefault().Name;
            obj.EmployeeAuditManagementName = dbo.HumanEmployees.Where(t => t.ID == objects.EmployeeAuditManagementID).FirstOrDefault().Name;
            obj.HumanDepartmentID = objects.HumanDepartmentID;
            obj.CreateBy = objects.CreateBy;
            obj.UpdateAt = objects.UpdateAt;
            obj.UpdateBy = objects.UpdateBy;
            obj.CreateAt = objects.CreateAt;
            return obj;
        }
        public int GetByPhaseAuditIDAndDepartmentID(int phaseAuditID, int DepartmentId)
        {
            var objects = auditObjectDA.GetQuery(t => t.HumanDepartmentID == DepartmentId && t.HumanAuditGradationID == phaseAuditID).FirstOrDefault();
            if (objects != null)
                return objects.ID;
            return 0;
        }
        public void Insert(HumanAuditDepartmentItem objNew, int userId)
        {
            HumanAuditDepartment obj = new HumanAuditDepartment();
            obj.HumanAuditGradationID = objNew.HumanAuditGradationID;
            obj.EmployeeAuditLeaderID = objNew.EmployeeAuditLeaderID;
            obj.EmployeeAuditManagementID = objNew.EmployeeAuditManagementID;
            obj.HumanDepartmentID = objNew.HumanDepartmentID;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            auditObjectDA.Insert(obj);
            auditObjectDA.Save();
        }
        public void InsertRange(List<HumanAuditDepartmentItem> items, int userID)
        {
            var dbo = auditObjectDA.Repository;
            if (items.Count() > 0)
            {
                foreach (var item in items)
                {
                    HumanAuditDepartment obj = new HumanAuditDepartment();
                    obj.HumanAuditGradationID = item.HumanAuditGradationID;
                    obj.HumanDepartmentID = item.HumanDepartmentID;
                    obj.EmployeeAuditLeaderID = item.EmployeeAuditLeaderID;
                    obj.EmployeeAuditManagementID = item.EmployeeAuditManagementID;
                    obj.CreateAt = DateTime.Now;
                    obj.CreateBy = userID;
                    auditObjectDA.Insert(obj);
                    auditObjectDA.Save();
                    var roles = dbo.HumanRoles
                     .Where(i => i.HumanDepartmentID == item.HumanDepartmentID)
                     .Where(i => i.IsActive)
                     .Where(i => !i.IsDelete)
                     .Select(i => i.ID)
                     .ToList();
                    if (roles.Count > 0)
                    {
                        foreach (var role in roles)
                        {
                            HumanAuditGradationRole objRole = new HumanAuditGradationRole();
                            objRole.HumanAuditDepartmentID = obj.ID;
                            objRole.HumanRoleID = role;
                            objRole.CreateAt = DateTime.Now;
                            objRole.CreateBy = userID;
                            objRole.IsAudit = true;
                            objRole.Factory = 1;
                            dbo.HumanAuditGradationRoles.Add(objRole);
                            dbo.SaveChanges();
                        }
                    }
                }
            }
        }
        public void InsertObject(string stringId, int userId, int phaseAuditId)
        {
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            var dbo = auditObjectDA.Repository;
            List<HumanAuditDepartment> objectAudit = new List<HumanAuditDepartment>();
            foreach (var item in ids)
            {
                objectAudit.Add(new HumanAuditDepartment
                {
                    HumanDepartmentID = (int)item,
                    HumanAuditGradationID = phaseAuditId,
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
