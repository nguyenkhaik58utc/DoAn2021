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
    public class HumanAuditGradationSV
    {
        private HumanAuditGradationDA humanAuditGradationDA = new HumanAuditGradationDA();
        public List<HumanAuditGradationItem> GetAll(ModelFilter filter, int focusId = 0)
        {
            var dbo = humanAuditGradationDA.Repository;
            IQueryable<iDAS.Base.HumanAuditGradation> query = dbo.HumanAuditGradations;
            if (focusId != 0)
            {
                filter.SortName = null;
                query = query.OrderBy(i => i.ID == focusId ? false : true);
            }
            var audits = query
                 .Filter(filter)
                 .Select(item => new HumanAuditGradationItem()
                 {
                     ID = item.ID,
                     Name = item.Name,
                     CreateBy = item.CreateBy,
                     Content = item.Content,
                     EndTime = item.EndTime,
                     IsPerform = item.IsPerform,
                     StartTime = item.StartTime,
                     UpdateAt = item.UpdateAt,
                     UpdateBy = item.UpdateBy,
                     CreateAt = item.CreateAt
                 })
                 .ToList();
            return audits;
        }
        public HumanAuditGradationItem GetByID(int id)
        {
            HumanAuditGradationItem obj = new HumanAuditGradationItem();
            var employeeSV = new HumanEmployeeSV();
            var dbo = humanAuditGradationDA.Repository;
            var phaseAudit = humanAuditGradationDA.GetById(id);
            if (phaseAudit != null)
            {
                obj.ID = phaseAudit.ID;
                obj.Name = phaseAudit.Name;
                obj.UpdateBy = phaseAudit.UpdateBy;
                obj.CreateBy = phaseAudit.CreateBy;
                obj.StartTime = phaseAudit.StartTime;
                obj.IsPerform = phaseAudit.IsPerform;
                obj.EndTime = phaseAudit.EndTime;
                obj.Content = phaseAudit.Content;
            }
            return obj;
        }
        public bool CheckNameExits(string name)
        {
            var rs = humanAuditGradationDA.GetQuery(t => t.Name.ToUpper() == name.ToUpper()).ToList();
            if (rs.Count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CheckNameEditExits(int id, string name)
        {
            var rs = humanAuditGradationDA.GetQuery(t => t.Name.ToUpper() == name.ToUpper() && t.ID != id).ToList();
            if (rs.Count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int Insert(HumanAuditGradationItem objNew, int userId)
        {
            var obj = new HumanAuditGradation();
            obj.Name = objNew.Name.Trim();
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            obj.IsPerform = objNew.IsPerform;
            obj.Content = objNew.Content;
            obj.EndTime = objNew.EndTime;
            obj.StartTime = objNew.StartTime;
            humanAuditGradationDA.Insert(obj);
            humanAuditGradationDA.Save();
            return obj.ID;
        }
        public void Update(HumanAuditGradationItem objEdit, int userId, List<HumanAuditDepartmentItem> lstDepartment)
        {
            var dbo = humanAuditGradationDA.Repository;
            var obj = humanAuditGradationDA.GetById(objEdit.ID);
            obj.Name = objEdit.Name.Trim();
            obj.IsPerform = objEdit.IsPerform;
            obj.Content = objEdit.Content;
            obj.EndTime = objEdit.EndTime;
            obj.StartTime = objEdit.StartTime;
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            humanAuditGradationDA.Update(obj);
            humanAuditGradationDA.Save();
            var arrIdPoint = lstDepartment.Select(t => t.ID).ToList();
            var s = dbo.HumanAuditDepartments.Where(t => t.HumanAuditGradationID == objEdit.ID && !arrIdPoint.Contains(t.ID)).ToList();
            if (s.Count > 0)
            {
                dbo.HumanAuditDepartments.RemoveRange(s);
                dbo.SaveChanges();
            }
            foreach (var item in lstDepartment)
            {
                var roles = dbo.HumanRoles
                  .Where(i => i.HumanDepartmentID == item.HumanDepartmentID)
                  .Where(i => i.IsActive)
                  .Where(i => !i.IsDelete)
                  .Select(i => i.ID)
                  .ToList();
                if (item.ID == 0)
                {
                    HumanAuditDepartment objPoint = new HumanAuditDepartment();
                    objPoint.CreateAt = DateTime.Now;
                    objPoint.CreateBy = userId;
                    objPoint.HumanAuditGradationID = objEdit.ID;
                    objPoint.HumanDepartmentID = item.HumanDepartmentID;
                    objPoint.EmployeeAuditLeaderID = item.EmployeeAuditLeaderID;
                    objPoint.EmployeeAuditManagementID = item.EmployeeAuditManagementID;
                    dbo.HumanAuditDepartments.Add(objPoint);
                    dbo.SaveChanges();
                    if (roles.Count > 0)
                    {
                        foreach (var role in roles)
                        {
                            if (dbo.HumanAuditGradationRoles.Where(t => t.HumanRoleID == role && t.HumanAuditDepartment.HumanAuditGradationID == objEdit.ID).FirstOrDefault() == null)
                            {
                                HumanAuditGradationRole objRole = new HumanAuditGradationRole();
                                objRole.HumanAuditDepartmentID = objPoint.ID;
                                objRole.HumanRoleID = role;
                                objRole.CreateAt = DateTime.Now;
                                objRole.CreateBy = userId;
                                objRole.IsAudit = true;
                                objRole.Factory = 1;
                                dbo.HumanAuditGradationRoles.Add(objRole);
                                dbo.SaveChanges();
                            }
                        }
                    }
                }
                else
                {
                    var objEditPoint = dbo.HumanAuditDepartments.Where(t => t.ID == item.ID).FirstOrDefault();
                    objEditPoint.UpdateAt = DateTime.Now;
                    objEditPoint.UpdateBy = userId;
                    objEditPoint.HumanAuditGradationID = objEdit.ID;
                    objEditPoint.HumanDepartmentID = item.HumanDepartmentID;
                    objEditPoint.EmployeeAuditLeaderID = item.EmployeeAuditLeaderID;
                    objEditPoint.EmployeeAuditManagementID = item.EmployeeAuditManagementID;
                    dbo.SaveChanges();
                    if (roles.Count > 0)
                    {
                        foreach (var role in roles)
                        { 
                            if (dbo.HumanAuditGradationRoles.Where(t => t.HumanRoleID == role && t.HumanAuditDepartment.HumanAuditGradationID == objEdit.ID).FirstOrDefault() == null)
                            {
                                HumanAuditGradationRole objRole = new HumanAuditGradationRole();
                                objRole.HumanAuditDepartmentID = objEditPoint.ID;
                                objRole.HumanRoleID = role;
                                objRole.CreateAt = DateTime.Now;
                                objRole.CreateBy = userId;
                                objRole.IsAudit = true;
                                objRole.Factory = 1;
                                dbo.HumanAuditGradationRoles.Add(objRole);
                                dbo.SaveChanges();
                            }
                        }
                    }
                }
            }
        }
        public void Delete(int id)
        {
            var dbo = humanAuditGradationDA.Repository;
            var departments = dbo.HumanAuditDepartments.Where(t => t.HumanAuditGradationID == id).Select(t => t.ID).ToList();
            var auditRoles = dbo.HumanAuditGradationRoles.Where(t => departments.Contains(t.HumanAuditDepartmentID)).Select(t=>t.ID).ToList();
            var criterias = dbo.HumanAuditGradationCriterias.Where(t => auditRoles.Contains(t.HumanAuditGradationRoleID)).Select(t => t.ID).ToList();
            var s = dbo.HumanAuditGradationCriteriaPoints.Where(t => criterias.Contains(t.HumanAuditGradationCriteriaID)).ToList();
            dbo.HumanAuditGradationCriteriaPoints.RemoveRange(s);
            dbo.SaveChanges();
            dbo.HumanAuditGradationCriterias.RemoveRange(dbo.HumanAuditGradationCriterias.Where(t => auditRoles.Contains(t.HumanAuditGradationRoleID)));
            dbo.SaveChanges();
            dbo.HumanAuditLevels.RemoveRange(dbo.HumanAuditLevels.Where(t => auditRoles.Contains(t.HumanAuditGradationRoleID)));
            dbo.SaveChanges();
            dbo.HumanAuditGradationRoles.RemoveRange(dbo.HumanAuditGradationRoles.Where(t => departments.Contains(t.HumanAuditDepartmentID)));
            dbo.SaveChanges();
            dbo.HumanAuditDepartments.RemoveRange(dbo.HumanAuditDepartments.Where(t => t.HumanAuditGradationID == id));
            dbo.SaveChanges();
            humanAuditGradationDA.Delete(id);
            humanAuditGradationDA.Save();
        }

        public List<HumanAuditGradationItem> GetPlanByDate(int page, int pageSize, out int total, DateTime searchFromDate, DateTime searchToDate)
        {
            var dbo = humanAuditGradationDA.Repository;
            IQueryable<iDAS.Base.HumanAuditGradation> query = dbo.HumanAuditGradations.Where(s => (s.EndTime <= searchToDate && s.StartTime >= searchFromDate) || (s.EndTime <= searchToDate && s.EndTime >= searchFromDate) || (s.StartTime <= searchToDate && s.StartTime >= searchFromDate));
            var audits = query
                 .Select(item => new HumanAuditGradationItem()
                 {
                     ID = item.ID,
                     Name = item.Name,
                     CreateBy = item.CreateBy,
                     Content = item.Content,
                     EndTime = item.EndTime,
                     IsPerform = item.IsPerform,
                     StartTime = item.StartTime,
                     UpdateAt = item.UpdateAt,
                     UpdateBy = item.UpdateBy,
                     CreateAt = item.CreateAt
                 })
                 .OrderByDescending(item => item.CreateAt)
                            .Page(page, pageSize, out total)
                 .ToList();
            return audits;
        }
    }
}
