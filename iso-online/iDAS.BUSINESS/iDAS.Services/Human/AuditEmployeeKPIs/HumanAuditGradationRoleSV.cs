using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
    public class HumanAuditGradationRoleSV
    {
        private HumanAuditGradationRoleDA HumanAuditGradationRoleDA = new HumanAuditGradationRoleDA();
        public decimal GetMinValue(int cateId, int facRole = 1)
        {
            var dbo = HumanAuditGradationRoleDA.Repository;
            var data = dbo.HumanAuditGradationCriterias.Where(t => t.HumanAuditGradationRoleID == cateId).Select(t => t.ID).ToList();
            decimal total = 0;
            foreach (var item in data)
            {
                total += dbo.HumanAuditGradationCriteriaPoints.Where(t => t.HumanAuditGradationCriteriaID == item).OrderBy(t => t.Point).FirstOrDefault() != null ? dbo.HumanAuditGradationCriteriaPoints.Where(t => t.HumanAuditGradationCriteriaID == item).OrderBy(t => t.Point).FirstOrDefault().Point * dbo.HumanAuditGradationCriterias.Where(t => t.ID == item).FirstOrDefault().Factory : 0;
            }
            return total * facRole;
        }
        public decimal GetMaxValue(int cateId, int facRole = 1)
        {
            var dbo = HumanAuditGradationRoleDA.Repository;
            var data = dbo.HumanAuditGradationCriterias.Where(t => t.HumanAuditGradationRoleID == cateId).Select(t => t.ID).ToList();
            decimal total = 0;
            foreach (var item in data)
            {
                total += dbo.HumanAuditGradationCriteriaPoints.Where(t => t.HumanAuditGradationCriteriaID == item).OrderByDescending(t => t.Point).FirstOrDefault() != null ? dbo.HumanAuditGradationCriteriaPoints.Where(t => t.HumanAuditGradationCriteriaID == item).OrderByDescending(t => t.Point).FirstOrDefault().Point * dbo.HumanAuditGradationCriterias.Where(t => t.ID == item).FirstOrDefault().Factory : 0;
            }
            return total * facRole;
        }
        public List<HumanAuditGradationRoleItem> GetAll(ModelFilter filter, int humanAuditGradationId = 0)
        {
            List<HumanAuditGradationRoleItem> lst = new List<HumanAuditGradationRoleItem>();
            var dbo = HumanAuditGradationRoleDA.Repository;
            var roleAudits = dbo.HumanAuditDepartments.Where(t => t.HumanAuditGradationID == humanAuditGradationId)
                 .SelectMany(i => i.HumanAuditGradationRoles)
                 .Select(item=>new HumanAuditGradationRoleItem()
                 {
                     ID = item.ID,
                     DepartmentName = item.HumanAuditDepartment.HumanDepartment.Name,
                     RoleName = item.HumanRole.Name,
                     Factory = item.Factory,
                     IsAudit = item.IsAudit,
                     UpdateAt = item.UpdateAt,
                     UpdateBy = item.UpdateBy,
                     CreateAt = item.CreateAt
                 })
                 .Filter(filter)
                 .ToList();
            if (roleAudits.Count > 0)
            {
                foreach (var item in roleAudits)
                {
                    lst.Add(new HumanAuditGradationRoleItem()
                 {
                     ID = item.ID,
                     DepartmentName = item.DepartmentName,
                     RoleName = item.RoleName,
                     Factory = item.Factory,
                     IsAudit = item.IsAudit,
                     UpdateAt = item.UpdateAt,
                     UpdateBy = item.UpdateBy,
                     CreateAt = item.CreateAt,
                     MinValue = GetMinValue(item.ID, item.Factory),
                     MaxValue = GetMaxValue(item.ID, item.Factory),
                 });
                }
            }
            return lst;
        }
        public void UpdateByID(int id, string field, string oldValue, string newValue)
        {
            var dbo = HumanAuditGradationRoleDA.Repository;
            var obj = HumanAuditGradationRoleDA.GetById(id);
            if (obj != null)
            {
                if (field.Equals("Factory"))
                    obj.Factory = int.Parse(newValue==null?"1":newValue);
                else
                    obj.IsAudit = newValue.Equals("true") ? true : false;
            }
            HumanAuditGradationRoleDA.Update(obj);
            HumanAuditGradationRoleDA.Save();
        }
        public List<HumanAuditGradationRoleItem> GetGradationByRoleIDs(ModelFilter filter, List<int> list, int focusId)
        {
            var dbo = HumanAuditGradationRoleDA.Repository;
            IQueryable<iDAS.Base.HumanAuditGradationRole> query = dbo.HumanAuditGradationRoles.Where(x => list.Contains(x.HumanRoleID));
            if (focusId != 0)
            {
                filter.SortName = null;
                query = query.OrderBy(i => i.HumanAuditDepartment.HumanAuditGradation.ID == focusId ? false : true);
            }
            var gradationAudits = query
                 .Filter(filter)
                 .Select(item => new HumanAuditGradationRoleItem()
                 {
                     GradationID = item.HumanAuditDepartment.HumanAuditGradation.ID,
                     GradationName = item.HumanAuditDepartment.HumanAuditGradation.Name,
                     GradationContent = item.HumanAuditDepartment.HumanAuditGradation.Content,
                     //HumanAuditLevelCategoryID = item.HumanAuditLevelCategoryID.HasValue?item.HumanAuditLevelCategoryID.Value:0,
                     GradationEndTime = item.HumanAuditDepartment.HumanAuditGradation.EndTime,
                     GradationIsPerform = item.HumanAuditDepartment.HumanAuditGradation.IsPerform,
                     GradationStartTime = item.HumanAuditDepartment.HumanAuditGradation.StartTime,
                     ID = item.ID,
                     Status = item.HumanAuditTicks.Any(),
                     RoleName = item.HumanRole.Name
                 })
                 .ToList();
            return gradationAudits;
        }
        public int GetTickIDByEmpID_GradiRoleID(int GraRoleId, int empID, int UserID)
        {
            var dbo = HumanAuditGradationRoleDA.Repository;
            var item = dbo.HumanAuditTicks.FirstOrDefault(x => x.HumanAuditGradationRoleID == GraRoleId && x.HumanEmployeeID == empID);
            if (item != null)
            {
                return item.ID;
            }
            else
            {
                HumanAuditTick obj = new HumanAuditTick();
                //  obj.HumanAuditGradationID = auditPhaseId;
                //  obj.EmployeeAuditManagementID = GetDepartmentByEmployeeID(employeeId, auditPhaseId).EmployeeAuditManagementID;
                // obj.EmployeeAuditLeaderID = GetDepartmentByEmployeeID(employeeId, auditPhaseId).EmployeeAuditLeaderID;
                obj.HumanEmployeeID = empID;
                obj.HumanAuditGradationRoleID = GraRoleId;
                obj.CreateAt = DateTime.Now;
                obj.Time = DateTime.Now;
                obj.CreateBy = UserID;
                dbo.HumanAuditTicks.Add(obj);
                dbo.SaveChanges();
                return obj.ID;
            }
        }

        public List<HumanAuditGradationRoleStatisticalItem> GetAllStatistical(int humanAuditGradationId)
        {
            List<HumanAuditGradationRoleStatisticalItem> lst = new List<HumanAuditGradationRoleStatisticalItem>();
            var dbo = HumanAuditGradationRoleDA.Repository;
            var roleAudits = dbo.HumanAuditDepartments.Where(t => t.HumanAuditGradationID == humanAuditGradationId)
                 .SelectMany(i => i.HumanAuditGradationRoles)
                 .ToList();
            if (roleAudits.Count > 0)
            {
                foreach (var item in roleAudits)
                {
                    foreach(var x in item.HumanAuditLevels.OrderByDescending(x=>x.MaxPoint))
                    {
                        lst.Add(new HumanAuditGradationRoleStatisticalItem()
                        {
                            HumanAuditGradationRoleID = item.ID,
                            DepartmentName = item.HumanAuditDepartment.HumanDepartment.Name,
                            RoleName = item.HumanRole.Name,
                            //DepartmentRoleName = item.HumanAuditDepartment.HumanDepartment.Name + " : " + item.HumanRole.Name,
                            AuditLevel = x.Name,
                            AuditLevelID = x.ID,
                            count = item.HumanAuditTicks.Count(i=>i.HumanAuditLevelID == x.ID)
                        });
                    }
                }
            }
            return lst;
        }

        public List<HumanAuditTickItem> GetResultByLevel(ModelFilter filter, int humanAuditGradationRoleID, int auditLevelID)
        {
            var dbo = HumanAuditGradationRoleDA.Repository;
            var data = dbo.HumanAuditTicks.Where(t => t.HumanAuditGradationRoleID == humanAuditGradationRoleID && t.HumanAuditLevelID == auditLevelID)
               .Select(item => new HumanAuditTickItem()
               {
                   ID = item.ID,
                   EmployeeName = item.HumanEmployee.Name,
                   FileID = item.HumanEmployee.AttachmentFileID,
                   FileName = item.HumanEmployee.AttachmentFile != null ? item.HumanEmployee.AttachmentFile.Name : string.Empty,
                   Email = item.HumanEmployee.Email,
                   Phone = item.HumanEmployee.Phone,
                   RoleName = dbo.HumanRoles.Where(t => t.ID == item.HumanAuditGradationRole.HumanRoleID).FirstOrDefault() != null ? dbo.HumanRoles.Where(t => t.ID == item.HumanAuditGradationRole.HumanRoleID).FirstOrDefault().Name : string.Empty,
                   CreateBy = item.CreateBy,
                   UpdateAt = item.UpdateAt,
                   UpdateBy = item.UpdateBy,
                   CreateAt = item.CreateAt,
                   IsSave = item.IsSave,
                   LevelName = item.HumanAuditLevel != null ? item.HumanAuditLevel.Name : string.Empty,
                   HumanAuditLevelID = item.HumanAuditLevelID.HasValue ? item.HumanAuditLevelID.Value : 0,
                   Comments = item.Comments,
                   TotalEmployeeAuditResult = item.EmployeeAuditResult,
                   TotalManagementAuditResult = item.EmployeeAuditManagementResult,
                   TotalLeaderAuditResult = item.EmployeeAuditLeaderResult,

               })
                .Filter(filter)
                .ToList();
            foreach (var item in data)
            {
                if (dbo.HumanAuditTickResultBonusScores.Any(x => x.HumanAuditTickID == item.ID))
                {
                    item.TotalPointBonus = dbo.HumanAuditTickResultBonusScores.Where(x => x.HumanAuditTickID == item.ID).Sum(x => x.Point);
                }
            }
            return data;
        }
    }
}
