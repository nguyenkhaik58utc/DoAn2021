using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class DepartmentRequirmentAuditResultSV
    {
        private DepartmentRequirmentAuditResultDA DepartmentRequirmentAuditResultDA = new DepartmentRequirmentAuditResultDA();
        private iDASBusinessEntities dbo = new DepartmentRequirmentAuditResultDA().Repository;
        private IEnumerable<HumanDepartment> getChilds(IEnumerable<HumanDepartment> e, int id)
        {
            var deparmentStart = e.Where(a => a.ID == id);
            var departmentNext = e.Where(a => a.ParentID == id);
            deparmentStart.Concat(departmentNext);
            return deparmentStart.Concat(departmentNext.SelectMany(a => getChilds(e, a.ID)));
        }
        public List<DepartmentRequirmentAuditResultItem> GetByDeparmtent(ModelFilter filter, bool isAdministrator, int employeeId, int departmentId)
        {
            if (CheckPermission(employeeId, departmentId) || isAdministrator)
            {
                bool isRootDepartment = CheckRootDepartment(departmentId);
                var RequirmentDepartments = dbo.DepartmentRegulatories
                            .Where(i => isRootDepartment || i.IsApplyAll || i.ListApplyDepartment.StartsWith(departmentId.ToString() + ",")
                                                         || i.ListApplyDepartment.Contains("," + departmentId.ToString() + ","))

                           .OrderByDescending(i => i.CreateAt).Filter(filter)
                            .Select(item => new
                            {
                                ID = item.ID,
                                Name = item.Name,
                                ISOID = item.ISOID,
                                IsUse = item.IsUse,
                                IsApplyAll = item.IsApplyAll,
                                RequirmentDepartment = dbo.DepartmentRequirmentAuditResults
                                    .Where(i => i.HumanDepartmentID == departmentId && i.DepartmentRequirmentID == item.ID)
                                    .OrderBy(i => i.AuditAt).FirstOrDefault()
                            })
                            .Select(item => new DepartmentRequirmentAuditResultItem()
                            {
                                ID = item.RequirmentDepartment == null ? 0 : item.RequirmentDepartment.ID,
                                DepartmentRequirmentID = item.ID,
                                RequirmentName = item.Name,
                                ISOID = item.ISOID,
                                IsApplyAll = item.IsApplyAll,
                                IsUse = item.IsUse,
                                QualityNCID = item.RequirmentDepartment == null ? null : item.RequirmentDepartment.QualityNCID,
                                IsObs = item.RequirmentDepartment == null ? false : item.RequirmentDepartment.QualityNC == null ? false : item.RequirmentDepartment.QualityNC.IsObs,
                                IsMedium = item.RequirmentDepartment == null ? false : item.RequirmentDepartment.QualityNC == null ? false : item.RequirmentDepartment.QualityNC.IsMedium,
                                IsMaximum = item.RequirmentDepartment == null ? false : item.RequirmentDepartment.QualityNC == null ? false : item.RequirmentDepartment.QualityNC.IsMaximum,
                                Evidence = item.RequirmentDepartment == null ? "" : item.RequirmentDepartment.Evidence,
                                IsPass = item.RequirmentDepartment == null ? false : item.RequirmentDepartment.IsPass,
                                ResultName = item.RequirmentDepartment == null ? null : (item.RequirmentDepartment.IsPass ? "Đạt" : "Không Đạt"),
                                AuditAt = item.RequirmentDepartment == null ? null : item.RequirmentDepartment.AuditAt,
                            })
                            .ToList();
                var ISODA = new CenterISOStandardDA();
                var result = new List<DepartmentRequirmentAuditResultItem>();
                if (RequirmentDepartments != null && RequirmentDepartments.Count > 0)
                {
                    foreach (var RequirmentDeparmtent in RequirmentDepartments)
                    {
                        RequirmentDeparmtent.ISOName = ISODA.GetQuery(iso => iso.ID == RequirmentDeparmtent.ISOID).Select(i => i.Code).FirstOrDefault();
                        result.Add(RequirmentDeparmtent);
                    }
                }
                return RequirmentDepartments;
            }
            return null;
        }
        private bool CheckPermission(int employeeId, int departmentId)
        {
            var deptIds = dbo.HumanEmployees.Where(i => i.ID == employeeId)
                               .SelectMany(i => i.HumanOrganizations)
                               .Select(i => i.HumanRole)
                               .Select(i => i.HumanDepartment.ID).ToList();
            var deptIdsPermiss = getDepartmentChilds(dbo.HumanDepartments, deptIds).Select(i => i.ID);
            if (deptIdsPermiss.Contains(departmentId)) return true;
            return false;
        }
        private IEnumerable<HumanDepartment> getDepartmentChilds(IEnumerable<HumanDepartment> query, List<int> Ids)
        {
            var childs = query.Where(i => Ids.Contains(i.ParentID)).Where(i => !i.IsDestroy);
            var root = query.Where(i => Ids.Contains(i.ID)).Where(i => !i.IsDestroy);
            root.Concat(childs);
            return root.Concat(childs.SelectMany(i => getDepartmentChilds(query, new List<int> { i.ID })));
        }
        public bool CheckRootDepartment(int departmentId)
        {
            return dbo.HumanDepartments.Any(i => i.ID == departmentId && i.ParentID == 0);
        }
        public List<DepartmentRequirmentAuditResultItem> GetAuditResult(int departmentId, int RequirmentId)
        {
            var DepartmentRequirmentAuditResult = DepartmentRequirmentAuditResultDA.GetQuery(i => i.HumanDepartmentID == departmentId && i.DepartmentRequirmentID == RequirmentId)
                        .Select(item => new DepartmentRequirmentAuditResultItem()
                        {
                            ID = item.ID,
                            DepartmentRequirmentID = item.DepartmentRequirmentID,
                            RequirmentName = item.DepartmentRequirment.Name,
                            Evidence = item.Evidence,
                            IsPass = item.IsPass,
                            AuditAt = item.AuditAt,
                        })
                        .OrderByDescending(item => item.RequirmentName)
                        .ToList();
            return DepartmentRequirmentAuditResult;
        }
        public DepartmentRequirmentAuditResultItem GetById(int Id)
        {
            var dbo = DepartmentRequirmentAuditResultDA.Repository;
            var DepartmentRequirmentAuditResult = DepartmentRequirmentAuditResultDA.GetQuery(i => i.ID == Id)
                        .Select(item => new DepartmentRequirmentAuditResultItem()
                        {
                            ID = item.ID,
                            DepartmentRequirmentID = item.DepartmentRequirmentID,
                            RequirmentName = item.DepartmentRequirment != null ? "" : item.DepartmentRequirment.Name,
                            HumanDepartmentID = item.HumanDepartmentID,
                            QualityNCID = item.QualityNCID,
                            HumanEmployee = new HumanEmployeeViewItem()
                            {
                                ID = item.AuditBy != null ? (int)item.AuditBy : 0,
                                Name = dbo.HumanEmployees.Where(i => i.ID == item.AuditBy).Select(i => i.Name).FirstOrDefault(),
                                Role = dbo.HumanOrganizations.Where(i => i.HumanEmployeeID == item.AuditBy).Select(i => i.HumanRole.Name).FirstOrDefault(),
                                Department = dbo.HumanOrganizations.Where(i => i.HumanEmployeeID == item.AuditBy).Select(i => i.HumanRole.HumanDepartment.Name).FirstOrDefault(),
                            },
                            Evidence = item.Evidence,
                            AuditNote = item.AuditNote,
                            IsPass = item.IsPass,
                            AuditAt = item.AuditAt,
                        })
                        .FirstOrDefault();
            return DepartmentRequirmentAuditResult;
        }
        public List<HumanDepartmentItem> GetDepartmentFails(int RequirmentId, int departmentId)
        {
            var departmentChildIDs = getChilds(dbo.HumanDepartments, departmentId).Select(i => i.ID);
            var result = DepartmentRequirmentAuditResultDA.GetQuery(i => i.DepartmentRequirmentID == RequirmentId & !i.IsPass
                            && departmentChildIDs.Contains(i.HumanDepartmentID))
                    .Select(item => new HumanDepartmentItem()
                    {
                        Name = item.HumanDepartment == null ? "" : item.HumanDepartment.Name
                    }).ToList();
            return result;
        }
        public void Update(DepartmentRequirmentAuditResultItem item, int userID)
        {
            var DepartmentRequirmentAuditResult = DepartmentRequirmentAuditResultDA.GetById(item.ID);
            DepartmentRequirmentAuditResult.ID = item.ID;
            DepartmentRequirmentAuditResult.QualityNCID = item.QualityNCID;
            DepartmentRequirmentAuditResult.DepartmentRequirmentID = item.DepartmentRequirmentID;
            DepartmentRequirmentAuditResult.HumanDepartmentID = item.HumanDepartmentID;
            DepartmentRequirmentAuditResult.AuditBy = item.HumanEmployee.ID;
            DepartmentRequirmentAuditResult.Evidence = item.Evidence;
            DepartmentRequirmentAuditResult.IsPass = item.IsPass;
            DepartmentRequirmentAuditResult.AuditAt = item.AuditAt;
            DepartmentRequirmentAuditResult.AuditNote = item.AuditNote;
            DepartmentRequirmentAuditResult.UpdateAt = DateTime.Now;
            DepartmentRequirmentAuditResult.UpdateBy = userID;
            DepartmentRequirmentAuditResultDA.Save();
        }
        public int Insert(DepartmentRequirmentAuditResultItem item, int userID)
        {
            var DepartmentRequirmentAuditResult = new DepartmentRequirmentAuditResult()
            {
                DepartmentRequirmentID = item.DepartmentRequirmentID,
                HumanDepartmentID = item.HumanDepartmentID,
                AuditBy = item.HumanEmployee.ID,
                QualityNCID = item.QualityNCID,
                Evidence = item.Evidence,
                IsPass = item.IsPass,
                AuditAt = item.AuditAt,
                AuditNote = item.AuditNote,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            DepartmentRequirmentAuditResultDA.Insert(DepartmentRequirmentAuditResult);
            DepartmentRequirmentAuditResultDA.Save();
            return DepartmentRequirmentAuditResult.ID;
        }
        public bool Delete(int id)
        {
            DepartmentRequirmentAuditResultDA.Delete(id);
            DepartmentRequirmentAuditResultDA.Save();
            return true;
        }
        public void InsertNC(int id, QualityNCItem item)
        {
            var qualityId = new QualityNCSV().Insert(item);
            var auditResult = DepartmentRequirmentAuditResultDA.GetById(id);
            auditResult.QualityNCID = qualityId;
            DepartmentRequirmentAuditResultDA.Save();
        }
    }
}
