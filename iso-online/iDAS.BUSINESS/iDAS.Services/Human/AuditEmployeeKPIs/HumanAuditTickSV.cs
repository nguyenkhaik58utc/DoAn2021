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
    public class HumanAuditTickSV
    {
        private HumanAuditTickDA HumanAuditTickDA = new HumanAuditTickDA();
        private HumanAuditTickResultBonusScoreDA bnScoreDA = new HumanAuditTickResultBonusScoreDA();
        public string GetNameGradation(int tickId = 0)
        {
            var dbo = HumanAuditTickDA.Repository;
            return dbo.HumanAuditTicks.Where(t => t.ID == tickId).FirstOrDefault().HumanAuditGradationRole.HumanAuditDepartment.HumanAuditGradation.Name;
        }
        public int GetLeader(int tickId=0)
        {
            var dbo = HumanAuditTickDA.Repository;
            return dbo.HumanAuditTicks.Where(t => t.ID == tickId).FirstOrDefault().HumanAuditGradationRole.HumanAuditDepartment.EmployeeAuditLeaderID;
        }
        public int GetMangement(int tickId = 0)
        {
            var dbo = HumanAuditTickDA.Repository;
            return dbo.HumanAuditTicks.Where(t => t.ID == tickId).FirstOrDefault().HumanAuditGradationRole.HumanAuditDepartment.EmployeeAuditManagementID;
        }
        public List<HumanAuditTickItem> GetResult(ModelFilter fillter, int auditPhaseId)
        {
            var dbo = HumanAuditTickDA.Repository;
            var data = dbo.HumanAuditTicks.Where(t => t.HumanAuditGradationRole.HumanAuditDepartment.HumanAuditGradationID == auditPhaseId)
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
                .Filter(fillter)
                .ToList();
            foreach(var item in data)
            {
                if (dbo.HumanAuditTickResultBonusScores.Any(x => x.HumanAuditTickID == item.ID))
                {
                    item.TotalPointBonus = dbo.HumanAuditTickResultBonusScores.Where(x=>x.HumanAuditTickID == item.ID).Sum(x => x.Point);
                }
            }
            return data;
        }
        public List<HumanAuditTickItem> GetResultByDepartment(ModelFilter fillter, int departmentId, int auditPhaseId)
        {
            var dbo = HumanAuditTickDA.Repository;
            var data = dbo.HumanAuditTicks
                    .Where(t => t.HumanAuditGradationRole.HumanAuditDepartment.HumanAuditGradationID == auditPhaseId)
                    .Where(t => t.HumanAuditGradationRole.HumanAuditDepartment.HumanDepartmentID == departmentId)
                    .Select(item => new HumanAuditTickItem()
                    {
                        ID = item.ID,
                        EmployeeName = item.HumanEmployee.Name,
                        FileID = item.HumanEmployee.AttachmentFileID,
                        FileName = item.HumanEmployee.AttachmentFile != null ? item.HumanEmployee.AttachmentFile.Name : string.Empty,
                        Email = item.HumanEmployee.Email,
                        Phone = item.HumanEmployee.Phone,
                        RoleName = dbo.HumanRoles.Where(t=>t.ID==item.HumanAuditGradationRole.HumanRoleID).FirstOrDefault()!=null?dbo.HumanRoles.Where(t=>t.ID==item.HumanAuditGradationRole.HumanRoleID).FirstOrDefault().Name:string.Empty,
                        CreateBy = item.CreateBy,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy,
                        CreateAt = item.CreateAt,
                        IsSave = item.IsSave,
                        //  HumanRoleID = item.HumanRoleID,
                        LevelName = item.HumanAuditLevel != null ? item.HumanAuditLevel.Name : string.Empty,
                        HumanAuditLevelID = item.HumanAuditLevelID.HasValue ? item.HumanAuditLevelID.Value : 0,
                        Comments = item.Comments,
                        TotalEmployeeAuditResult = item.EmployeeAuditResult,
                        TotalManagementAuditResult = item.EmployeeAuditManagementResult,
                        TotalLeaderAuditResult = item.EmployeeAuditLeaderResult,
                        //TotalPointBonus = (item.HumanAuditTickResults.FirstOrDefault() != null && item.HumanAuditTickResults.FirstOrDefault().HumanAuditTickResultBonusScores.Count > 0) ? 0 : item.HumanAuditTickResults.FirstOrDefault().HumanAuditTickResultBonusScores.Select(X => X.Point).Sum()
                    })
                .Filter(fillter)
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
        public HumanAuditTickItem GetByID(int id)
        {
            var dbo = HumanAuditTickDA.Repository;
            var data = HumanAuditTickDA.GetById(id);
            var employeeSV = new HumanEmployeeSV();
            var obj = new HumanAuditTickItem();
            if (data != null)
                obj.ID = data.ID;
            obj.Comments = data.Comments;
            obj.HumanEmployeeID = data.HumanEmployeeID;
            obj.Name = data.HumanAuditGradationRole.HumanAuditDepartment.HumanAuditGradation.Name;
            obj.LevelName = data.HumanAuditLevel != null ? data.HumanAuditLevel.Name : string.Empty;
            obj.StartDate = data.HumanAuditGradationRole.HumanAuditDepartment.HumanAuditGradation.StartTime;
            obj.EndDate = data.HumanAuditGradationRole.HumanAuditDepartment.HumanAuditGradation.EndTime;
            obj.HumanAuditDepartmentID = data.HumanAuditGradationRole.HumanAuditDepartmentID;
            obj.TotalEmployeeAuditResult = data.EmployeeAuditResult;
            obj.HumanRoleID = data.HumanAuditGradationRole.HumanRoleID;
            obj.TotalManagementAuditResult = data.EmployeeAuditManagementResult;
            obj.TotalLeaderAuditResult = data.EmployeeAuditLeaderResult;
            obj.HumanAuditLevelID = data.HumanAuditLevelID.HasValue ? data.HumanAuditLevelID.Value : 0;
            obj.EmployeeAudit = employeeSV.GetEmployeeView(data.HumanEmployeeID);
            obj.TotalPointBonus = dbo.HumanAuditTickResultBonusScores.Any(x => x.HumanAuditTickID == data.ID) ? dbo.HumanAuditTickResultBonusScores.Where(x => x.HumanAuditTickID == data.ID).Sum(x => x.Point) : 0;
            //obj.TotalPoint = obj.EmployeeAuditLeaderResult + obj.TotalPointBonus;
            return obj;
        }

        public List<HumanAuditGradationItem> GetGradationByManagement(ModelFilter filter, int employeeId)
        {
            var dbo = HumanAuditTickDA.Repository;
            var results =
                  dbo.HumanAuditGradations
                .Where(i => i.HumanAuditDepartments.Any(t => t.EmployeeAuditManagementID == employeeId))
                 .Filter(filter)
                 .Select(item => new HumanAuditGradationItem()
                 {
                     ID = item.ID,
                     Name = item.Name,
                     IsPerform = item.IsPerform
                 })
                 .ToList();
            return results;
        }
        public List<HumanAuditGradationItem> GetGradationByLeader(ModelFilter filter, int employeeId)
        {
            var dbo = HumanAuditTickDA.Repository;
            var results =
                  dbo.HumanAuditGradations
                .Where(i => i.HumanAuditDepartments.Any(t => t.EmployeeAuditLeaderID == employeeId))
                 .Filter(filter)
                 .Select(item => new HumanAuditGradationItem()
                 {
                     ID = item.ID,
                     Name = item.Name,
                     IsPerform = item.IsPerform
                 })
                 .ToList();
            return results;
        }
        public List<HumanAuditTickItem> GetByEmployee(ModelFilter filter, int employeeId, int focusId = 0)
        {
            var dbo = HumanAuditTickDA.Repository;
            IQueryable<iDAS.Base.HumanAuditTick> query = dbo.HumanAuditTicks;
            if (focusId != 0)
            {
                filter.SortName = null;
                query = query.OrderBy(i => i.ID == focusId ? false : true);
            }
            var audits = query
                 .Where(i => i.HumanEmployeeID == employeeId)
                 .Select(item => new HumanAuditTickItem()
                 {
                     ID = item.ID,
                     HumanAuditGradationID = item.HumanAuditGradationRole.HumanAuditDepartment.HumanAuditGradationID,
                     HumanAuditGradationRoleID = item.HumanAuditGradationRoleID,
                     HumanAuditGradationRole = item.HumanAuditGradationRole.HumanAuditDepartment.HumanAuditGradation.Name + " ( " + item.HumanAuditGradationRole.HumanRole.Name + " )",
                     IsEmployeeAuditted = item.IsEmployeeAuditted,
                     CreateAt = item.CreateAt
                 })
                 .OrderBy(i => i.CreateAt)
                 .Filter(filter)
                 .ToList();
            return audits;
        }
        public List<HumanAuditTickItem> GetByManagement(ModelFilter filter, int employeeId, int gradationId, int focusId = 0)
        {
            var dbo = HumanAuditTickDA.Repository;
            IQueryable<iDAS.Base.HumanAuditTick> query = dbo.HumanAuditTicks;
            if (focusId != 0)
            {
                filter.SortName = null;
                query = query.OrderBy(i => i.ID == focusId ? false : true);
            }
            var audits = query
               .Where(i => i.HumanAuditGradationRole.HumanAuditDepartment.EmployeeAuditManagementID == employeeId
                   && i.HumanAuditGradationRole.HumanAuditDepartment.HumanAuditGradationID == gradationId)
                 .Filter(filter)
                 .Select(item => new HumanAuditTickItem()
                 {
                     ID = item.ID,
                     HumanAuditGradationRoleID = item.HumanAuditGradationRoleID,
                     HumanAuditGradationRole = item.HumanEmployee.Name + " (" + item.HumanAuditGradationRole.HumanRole.Name + " )",
                     IsManagementAuditted = item.IsManagementAuditted,
                     CreateAt = item.CreateAt
                 })
                 .OrderBy(i => i.CreateAt)
                 .Filter(filter)
                 .ToList();
            return audits;
        }
        public List<HumanAuditTickItem> GetByLeader(ModelFilter filter, int employeeId, int gradationId, int focusId = 0)
        {
            var dbo = HumanAuditTickDA.Repository;
            IQueryable<iDAS.Base.HumanAuditTick> query = dbo.HumanAuditTicks;
            if (focusId != 0)
            {
                filter.SortName = null;
                query = query.OrderBy(i => i.ID == focusId ? false : true);
            }
            var audits = query
                 .Where(i => i.HumanAuditGradationRole.HumanAuditDepartment.EmployeeAuditLeaderID == employeeId
                   && i.HumanAuditGradationRole.HumanAuditDepartment.HumanAuditGradationID == gradationId)
                 .Select(item => new HumanAuditTickItem()
                 {
                     ID = item.ID,
                     HumanAuditGradationRoleID = item.HumanAuditGradationRoleID,
                     HumanAuditGradationRole = item.HumanEmployee.Name + " (" + item.HumanAuditGradationRole.HumanRole.Name + " )",
                     IsLeaderAuditted = item.IsLeaderAuditted,
                     CreateAt = item.CreateAt
                 })
                 .OrderBy(i => i.CreateAt)
                 .Filter(filter)
                 .ToList();
            return audits;
        }
        public List<ObjectAudit> GetRoleAuditGradation(int auditGradationId)
        {
            var dbo = HumanAuditTickDA.Repository;
            var departmentIDs = dbo.HumanAuditDepartments
                .Where(t => t.HumanAuditGradationID == auditGradationId)
                .Select(t => t.ID)
                .Distinct()
                .ToList();
            var roleIDs = dbo.HumanAuditGradationRoles
                .Where(i => departmentIDs.Contains(i.HumanAuditDepartmentID) && i.IsAudit)
                .Select(item => new ObjectAudit()
                {
                    GradationRoleID = item.ID,
                    HumanRoleID = item.HumanRoleID
                })
                .Distinct()
                .ToList();

            return roleIDs;
        }
        public List<int> GetEmployees(int roleId)
        {
            var dbo = HumanAuditTickDA.Repository;
            var employeeIds = dbo.HumanRoles
                .Where(i => i.ID == roleId)
                .SelectMany(i => i.HumanOrganizations)
                .Select(item => item.HumanEmployee.ID)
                .ToList();
            return employeeIds;
        }
        public HumanAuditDepartment GetDepartmentByEmployeeID(int employeeId, int auditPhaseId)
        {
            HumanAuditDepartment obj = new HumanAuditDepartment();
            var dbo = HumanAuditTickDA.Repository;
            var roleIds = dbo.HumanEmployees
                .Where(i => i.ID == employeeId)
                .SelectMany(t => t.HumanOrganizations)
                .Select(t => t.HumanRole.ID)
                .ToList();
            var departmentIds = dbo.HumanRoles
                           .Where(i => roleIds.Contains(i.ID))
                           .Select(i => i.HumanDepartmentID).Distinct()
                           .ToList();
            obj = dbo.HumanAuditDepartments.Where(t => t.HumanAuditGradationID == auditPhaseId && departmentIds.Contains(t.HumanDepartmentID)).FirstOrDefault();
            return obj;
        }
        public int Insert(int humanAuditGradationRoleID, int userId, int employeeId)
        {
            var dbo = HumanAuditTickDA.Repository;

            if (dbo.HumanAuditTicks.Where(t => t.HumanEmployeeID == employeeId && t.HumanAuditGradationRoleID == humanAuditGradationRoleID)
                .FirstOrDefault() == null)
            {
                HumanAuditTick obj = new HumanAuditTick();
                obj.HumanAuditGradationRoleID = humanAuditGradationRoleID;
                obj.HumanEmployeeID = employeeId;
                obj.CreateAt = DateTime.Now;
                obj.Time = DateTime.Now;
                obj.CreateBy = userId;
                dbo.HumanAuditTicks.Add(obj);
                dbo.SaveChanges();
                return obj.ID;
            }
            else
                return 0;
        }
        public void SetPerform(int auditPhaseId)
        {
            var dbo = HumanAuditTickDA.Repository;
            var auditphase = dbo.HumanAuditGradations.Where(t => t.ID == auditPhaseId).FirstOrDefault();
            auditphase.IsPerform = true;
            dbo.SaveChanges();
        }
        public List<HumanAuditGradationCriteriaItem> GetCriteriaByTickID(int id)
        {
            var dbo = HumanAuditTickDA.Repository;
            return
                dbo.HumanAuditTicks.Where(i => i.ID == id)
                .Select(i => i.HumanAuditGradationRole)
                .SelectMany(i => i.HumanAuditGradationCriterias)
                .Select(item => new HumanAuditGradationCriteriaItem()
                    {
                        ID = item.ID,
                        Name = item.Name,
                        HumanAuditCriteriaCategoryName = item.HumanAuditCriteriaCategoryName,
                        Answer = item.HumanAuditGradationCriteriaPoints.Select(
                          p => new HumanAuditGradationCriteriaPointItem()
                          {
                              ID = p.ID,
                              Name = p.Name,
                              Point = p.Point
                          }
                        ).OrderBy(i => i.Point).ToList(),
                        CreateAt = item.CreateAt
                    }
                )
                .OrderByDescending(i => i.CreateAt)
                .ToList();
        }
        public void Update(HumanAuditTickItem objEdit, int userId)
        {
            var obj = HumanAuditTickDA.GetById(objEdit.ID);
            obj.HumanAuditLevelID = objEdit.HumanAuditLevelID;
            obj.Time = DateTime.Now;
            obj.IsSave = false;
            obj.Comments = objEdit.Comments;
            obj.UpdateBy = userId;
            obj.UpdateAt = DateTime.Now;
            HumanAuditTickDA.Update(obj);
            HumanAuditTickDA.Save();
        }
        public void UpdateIsSave(int id, int userId)
        {
            var obj = HumanAuditTickDA.GetById(id);
            obj.IsSave = true;
            obj.UpdateBy = userId;
            obj.UpdateAt = DateTime.Now;
            HumanAuditTickDA.Update(obj);
            HumanAuditTickDA.Save();
        }
        public void SendLeader(int ID, int userId)
        {
            var obj = HumanAuditTickDA.GetById(ID);
            var lstManagmentResult = obj.HumanAuditTickResults.Select(i => i.AuditManagementResult).ToList();
            obj.IsManagementAuditted = true;
            obj.EmployeeAuditManagementResult = HumanAuditTickDA.Repository.HumanAuditGradationCriteriaPoints
                    .Where(i => lstManagmentResult.Contains(i.ID))
                    .Sum(i => i.Point * i.HumanAuditGradationCriteria.Factory 
                        * i.HumanAuditGradationCriteria.HumanAuditGradationRole.Factory); ;
            obj.UpdateBy = userId;
            obj.UpdateAt = DateTime.Now;
            HumanAuditTickDA.Update(obj);
            HumanAuditTickDA.Save();
        }
        public void Approval(int ID, decimal employeeAuditResult, decimal employeeAuditManagementResult, decimal employeeAuditLeaderResult, int userId)
        {
            var obj = HumanAuditTickDA.GetById(ID);
            var lstLeaderResult = obj.HumanAuditTickResults.Select(i => i.AuditLeaderResult).ToList();
            obj.IsLeaderAuditted = true;
            obj.EmployeeAuditLeaderResult = HumanAuditTickDA.Repository.HumanAuditGradationCriteriaPoints
                   .Where(i => lstLeaderResult.Contains(i.ID))
                   .Sum(i => i.Point * i.HumanAuditGradationCriteria.Factory
                       * i.HumanAuditGradationCriteria.HumanAuditGradationRole.Factory); ;
            obj.UpdateBy = userId;
            obj.UpdateAt = DateTime.Now;
            HumanAuditTickDA.Update(obj);
            HumanAuditTickDA.Save();
        }

        
    }
}
