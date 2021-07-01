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
    public class DepartmentLegalAuditResultSV
    {
        private DepartmentLegalAuditResultDA DepartmentLegalAuditResultDA = new DepartmentLegalAuditResultDA();
        private IEnumerable<HumanDepartment> getChilds(IEnumerable<HumanDepartment> e, int id)
        {
            var deparmentStart = e.Where(a => a.ID == id);
            var departmentNext = e.Where(a => a.ParentID == id);
            deparmentStart.Concat(departmentNext);
            return deparmentStart.Concat(departmentNext.SelectMany(a => getChilds(e, a.ID)));
        }
        public List<DepartmentLegalAuditResultItem> GetByDeparmtent(ModelFilter filter, bool isAdministrator, int employeeId, int departmentId)
        {
            var dbo = DepartmentLegalAuditResultDA.Repository;
            if (CheckPermission(employeeId, departmentId) || isAdministrator)
            {
                bool isRootDepartment = CheckRootDepartment(departmentId);
                var LegalDepartments = dbo.DepartmentLegals
                    .Where(i => isRootDepartment || i.IsApplyAll || i.ListApplyDepartment.StartsWith(departmentId.ToString() + ",")
                                                         || i.ListApplyDepartment.Contains("," + departmentId.ToString() + ","))
                            .Filter(filter)
                            .Select(item => new
                            {
                                ID = item.ID,
                                Name = item.Name,
                                IsUse = item.IsUse,
                                IsApplyAll = item.IsApplyAll,
                                ISOID = item.ISOID,
                                LegalDepartment = dbo.DepartmentLegalAuditResults.Where(i => i.HumanDepartmentID == departmentId && i.DepartmentLegalID == item.ID)
                                    .OrderBy(i => i.AuditAt).FirstOrDefault()
                            })
                            .Select(item => new DepartmentLegalAuditResultItem()
                            {
                                ID = item.LegalDepartment == null ? 0 : item.LegalDepartment.ID,
                                DepartmentLegalID = item.ID,
                                LegalName = item.Name,
                                ISOID = item.ISOID,
                                IsApplyAll = item.IsApplyAll,
                                IsUse = item.IsUse,
                                QualityNCID = item.LegalDepartment == null ? null : item.LegalDepartment.QualityNCID,
                                IsObs = item.LegalDepartment == null ? false : item.LegalDepartment.QualityNC == null ? false : item.LegalDepartment.QualityNC.IsObs,
                                IsMedium = item.LegalDepartment == null ? false : item.LegalDepartment.QualityNC == null ? false : item.LegalDepartment.QualityNC.IsMedium,
                                IsMaximum = item.LegalDepartment == null ? false : item.LegalDepartment.QualityNC == null ? false : item.LegalDepartment.QualityNC.IsMaximum,
                                Evidence = item.LegalDepartment == null ? "" : item.LegalDepartment.Evidence,
                                IsPass = item.LegalDepartment == null ? false : item.LegalDepartment.IsPass,
                                ResultName = item.LegalDepartment == null ? null : (item.LegalDepartment.IsPass ? "Đạt" : "Không Đạt"),
                                AuditAt = item.LegalDepartment == null ? null : item.LegalDepartment.AuditAt,
                            })
                            .ToList();
                var ISODA = new CenterISOStandardDA();
                var result = new List<DepartmentLegalAuditResultItem>();
                if (LegalDepartments != null && LegalDepartments.Count > 0)
                {
                    foreach (var LegalDeparmtent in LegalDepartments)
                    {
                        LegalDeparmtent.ISOName = ISODA.GetQuery(iso => iso.ID == LegalDeparmtent.ISOID).Select(i => i.Name).FirstOrDefault();
                        result.Add(LegalDeparmtent);
                    }
                }
                return result;
            }
            return null;
        }
        private bool CheckPermission(int employeeId, int departmentId)
        {
            var dbo = DepartmentLegalAuditResultDA.Repository;
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
            var dbo = DepartmentLegalAuditResultDA.Repository;
            return dbo.HumanDepartments.Any(i => i.ID == departmentId && i.ParentID == 0);
        }
        public List<DepartmentLegalAuditResultItem> GetAuditResult(int departmentId, int LegalId)
        {
            var DepartmentLegalAuditResult = DepartmentLegalAuditResultDA.GetQuery(i => i.HumanDepartmentID == departmentId && i.DepartmentLegalID == LegalId)
                        .Select(item => new DepartmentLegalAuditResultItem()
                        {
                            ID = item.ID,
                            DepartmentLegalID = item.DepartmentLegalID,
                            LegalName = item.DepartmentLegal.Name,
                            Evidence = item.Evidence,
                            IsPass = item.IsPass,
                            AuditAt = item.AuditAt,
                        })
                        .OrderByDescending(item => item.LegalName)
                        .ToList();
            return DepartmentLegalAuditResult;
        }
        public DepartmentLegalAuditResultItem GetById(int Id)
        {
            var dbo = DepartmentLegalAuditResultDA.Repository;
            var DepartmentLegalAuditResult = DepartmentLegalAuditResultDA.GetQuery(i => i.ID == Id)
                        .Select(item => new DepartmentLegalAuditResultItem()
                        {
                            ID = item.ID,
                            DepartmentLegalID = item.DepartmentLegalID,
                            LegalName = item.DepartmentLegal != null ? "" : item.DepartmentLegal.Name,
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
                        .First();
            return DepartmentLegalAuditResult;
        }
        public List<HumanDepartmentItem> GetDepartmentFails(int LegalId, int departmentId)
        {
            var dbo = DepartmentLegalAuditResultDA.Repository;
            var departmentChildIDs = getChilds(dbo.HumanDepartments, departmentId).Select(i => i.ID);
            var result = DepartmentLegalAuditResultDA.GetQuery(i => i.DepartmentLegalID == LegalId & !i.IsPass
                            && departmentChildIDs.Contains(i.HumanDepartmentID))
                    .Select(item => new HumanDepartmentItem()
                    {
                        Name = item.HumanDepartment == null ? "" : item.HumanDepartment.Name
                    }).ToList();
            return result;
        }
        public void Update(DepartmentLegalAuditResultItem item, int userID)
        {
            var DepartmentLegalAuditResult = DepartmentLegalAuditResultDA.GetById(item.ID);
            DepartmentLegalAuditResult.ID = item.ID;
            DepartmentLegalAuditResult.QualityNCID = item.QualityNCID;
            DepartmentLegalAuditResult.DepartmentLegalID = item.DepartmentLegalID;
            DepartmentLegalAuditResult.HumanDepartmentID = item.HumanDepartmentID;
            DepartmentLegalAuditResult.AuditBy = item.HumanEmployee.ID;
            DepartmentLegalAuditResult.Evidence = item.Evidence;
            DepartmentLegalAuditResult.IsPass = item.IsPass;
            DepartmentLegalAuditResult.AuditAt = item.AuditAt;
            DepartmentLegalAuditResult.AuditNote = item.AuditNote;
            DepartmentLegalAuditResult.UpdateAt = DateTime.Now;
            DepartmentLegalAuditResult.UpdateBy = userID;
            DepartmentLegalAuditResultDA.Save();
        }
        public int Insert(DepartmentLegalAuditResultItem item, int userID)
        {
            var DepartmentLegalAuditResult = new DepartmentLegalAuditResult()
            {
                DepartmentLegalID = item.DepartmentLegalID,
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
            DepartmentLegalAuditResultDA.Insert(DepartmentLegalAuditResult);
            DepartmentLegalAuditResultDA.Save();
            return DepartmentLegalAuditResult.ID;
        }
        public bool Delete(int id)
        {
            DepartmentLegalAuditResultDA.Delete(id);
            DepartmentLegalAuditResultDA.Save();
            return true;
        }
        public void InsertNC(int id, QualityNCItem item)
        {
            var qualityId = new QualityNCSV().Insert(item);
            var auditResult = DepartmentLegalAuditResultDA.GetById(id);
            auditResult.QualityNCID = qualityId;
            DepartmentLegalAuditResultDA.Save();
        }
    }
}
