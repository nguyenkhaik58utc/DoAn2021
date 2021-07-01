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
    public class DepartmentRegulatoryAuditResultSV
    {
        private DepartmentRegulatoryAuditResultDA DepartmentRegulatoryAuditResultDA = new DepartmentRegulatoryAuditResultDA();
        private iDASBusinessEntities dbo = new DepartmentRegulatoryAuditResultDA().Repository;
        private IEnumerable<HumanDepartment> getChilds(IEnumerable<HumanDepartment> e, int id)
        {
            var deparmentStart = e.Where(a => a.ID == id);
            var departmentNext = e.Where(a => a.ParentID == id);
            deparmentStart.Concat(departmentNext);
            return deparmentStart.Concat(departmentNext.SelectMany(a => getChilds(e, a.ID)));
        }
        public List<DepartmentRegulatoryAuditResultItem> GetByDeparmtent(ModelFilter filter, bool isAdministrator, int employeeId, int departmentId)
        {
            if (CheckPermission(employeeId, departmentId) || isAdministrator)
            {
                bool isRootDepartment = CheckRootDepartment(departmentId);
                var regulatoryDepartments = dbo.DepartmentRegulatories
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
                                RegulatoryDepartment = dbo.DepartmentRegulatoryAuditResults
                                    .Where(i => i.HumanDepartmentID == departmentId && i.DepartmentRegulatoryID == item.ID)
                                    .OrderBy(i => i.AuditAt).FirstOrDefault()
                            })
                            .Select(item => new DepartmentRegulatoryAuditResultItem()
                            {
                                ID = item.RegulatoryDepartment == null ? 0 : item.RegulatoryDepartment.ID,
                                DepartmentRegulatoryID = item.ID,
                                RegulatoryName = item.Name,
                                ISOID = item.ISOID,
                                IsApplyAll = item.IsApplyAll,
                                IsUse = item.IsUse,
                                QualityNCID = item.RegulatoryDepartment == null ? null : item.RegulatoryDepartment.QualityNCID,
                                IsObs = item.RegulatoryDepartment == null ? false : item.RegulatoryDepartment.QualityNC == null ? false : item.RegulatoryDepartment.QualityNC.IsObs,
                                IsMedium = item.RegulatoryDepartment == null ? false : item.RegulatoryDepartment.QualityNC == null ? false : item.RegulatoryDepartment.QualityNC.IsMedium,
                                IsMaximum = item.RegulatoryDepartment == null ? false : item.RegulatoryDepartment.QualityNC == null ? false : item.RegulatoryDepartment.QualityNC.IsMaximum,
                                Evidence = item.RegulatoryDepartment == null ? "" : item.RegulatoryDepartment.Evidence,
                                IsPass = item.RegulatoryDepartment == null ? false : item.RegulatoryDepartment.IsPass,
                                ResultName = item.RegulatoryDepartment == null ? null : (item.RegulatoryDepartment.IsPass ? "Đạt" : "Không Đạt"),
                                AuditAt = item.RegulatoryDepartment == null ? null : item.RegulatoryDepartment.AuditAt,
                            })
                            .ToList();
                var ISODA = new CenterISOStandardDA();
                var result = new List<DepartmentRegulatoryAuditResultItem>();
                if (regulatoryDepartments != null && regulatoryDepartments.Count > 0)
                {
                    foreach (var RegulatoryDeparmtent in regulatoryDepartments)
                    {
                        RegulatoryDeparmtent.ISOName = ISODA.GetQuery(iso => iso.ID == RegulatoryDeparmtent.ISOID).Select(i => i.Code).FirstOrDefault();
                        result.Add(RegulatoryDeparmtent);
                    }
                }
                return regulatoryDepartments;
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
        public List<DepartmentRegulatoryAuditResultItem> GetAuditResult(int departmentId, int RegulatoryId)
        {
            var DepartmentRegulatoryAuditResult = DepartmentRegulatoryAuditResultDA.GetQuery(i => i.HumanDepartmentID == departmentId && i.DepartmentRegulatoryID == RegulatoryId)
                        .Select(item => new DepartmentRegulatoryAuditResultItem()
                        {
                            ID = item.ID,
                            DepartmentRegulatoryID = item.DepartmentRegulatoryID,
                            RegulatoryName = item.DepartmentRegulatory.Name,
                            Evidence = item.Evidence,
                            IsPass = item.IsPass,
                            AuditAt = item.AuditAt,
                        })
                        .OrderByDescending(item => item.RegulatoryName)
                        .ToList();
            return DepartmentRegulatoryAuditResult;
        }
        public DepartmentRegulatoryAuditResultItem GetById(int Id)
        {
            var dbo = DepartmentRegulatoryAuditResultDA.Repository;
            var DepartmentRegulatoryAuditResult = DepartmentRegulatoryAuditResultDA.GetQuery(i => i.ID == Id)
                        .Select(item => new DepartmentRegulatoryAuditResultItem()
                        {
                            ID = item.ID,
                            DepartmentRegulatoryID = item.DepartmentRegulatoryID,
                            RegulatoryName = item.DepartmentRegulatory != null ? "" : item.DepartmentRegulatory.Name,
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
            return DepartmentRegulatoryAuditResult;
        }
        public List<HumanDepartmentItem> GetDepartmentFails(int RegulatoryId, int departmentId)
        {
            var departmentChildIDs = getChilds(dbo.HumanDepartments, departmentId).Select(i => i.ID);
            var result = DepartmentRegulatoryAuditResultDA.GetQuery(i => i.DepartmentRegulatoryID == RegulatoryId & !i.IsPass
                            && departmentChildIDs.Contains(i.HumanDepartmentID))
                    .Select(item => new HumanDepartmentItem()
                    {
                        Name = item.HumanDepartment == null ? "" : item.HumanDepartment.Name
                    }).ToList();
            return result;
        }
        public void Update(DepartmentRegulatoryAuditResultItem item, int userID)
        {
            var DepartmentRegulatoryAuditResult = DepartmentRegulatoryAuditResultDA.GetById(item.ID);
            DepartmentRegulatoryAuditResult.ID = item.ID;
            DepartmentRegulatoryAuditResult.QualityNCID = item.QualityNCID;
            DepartmentRegulatoryAuditResult.DepartmentRegulatoryID = item.DepartmentRegulatoryID;
            DepartmentRegulatoryAuditResult.HumanDepartmentID = item.HumanDepartmentID;
            DepartmentRegulatoryAuditResult.AuditBy = item.HumanEmployee.ID;
            DepartmentRegulatoryAuditResult.Evidence = item.Evidence;
            DepartmentRegulatoryAuditResult.IsPass = item.IsPass;
            DepartmentRegulatoryAuditResult.AuditAt = item.AuditAt;
            DepartmentRegulatoryAuditResult.AuditNote = item.AuditNote;
            DepartmentRegulatoryAuditResult.UpdateAt = DateTime.Now;
            DepartmentRegulatoryAuditResult.UpdateBy = userID;
            DepartmentRegulatoryAuditResultDA.Save();
        }
        public int Insert(DepartmentRegulatoryAuditResultItem item, int userID)
        {
            var DepartmentRegulatoryAuditResult = new DepartmentRegulatoryAuditResult()
            {
                DepartmentRegulatoryID = item.DepartmentRegulatoryID,
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
            DepartmentRegulatoryAuditResultDA.Insert(DepartmentRegulatoryAuditResult);
            DepartmentRegulatoryAuditResultDA.Save();
            return DepartmentRegulatoryAuditResult.ID;
        }
        public bool Delete(int id)
        {
            DepartmentRegulatoryAuditResultDA.Delete(id);
            DepartmentRegulatoryAuditResultDA.Save();
            return true;
        }
        public void InsertNC(int id, QualityNCItem item)
        {
            var qualityId = new QualityNCSV().Insert(item);
            var auditResult = DepartmentRegulatoryAuditResultDA.GetById(id);
            auditResult.QualityNCID = qualityId;
            DepartmentRegulatoryAuditResultDA.Save();
        }
    }
}
