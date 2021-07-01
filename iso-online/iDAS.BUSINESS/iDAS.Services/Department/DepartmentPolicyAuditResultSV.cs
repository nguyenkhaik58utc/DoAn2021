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
    //public class DepartmentPolicyAuditResultSV
    //{
    //    private DepartmentPolicyAuditResultDA DepartmentPolicyAuditResultDA = new DepartmentPolicyAuditResultDA();
    //    private IEnumerable<HumanDepartment> getChilds(IEnumerable<HumanDepartment> e, int id)
    //    {
    //        var deparmentStart = e.Where(a => a.ID == id);
    //        var departmentNext = e.Where(a => a.ParentID == id);
    //        deparmentStart.Concat(departmentNext);
    //        return deparmentStart.Concat(departmentNext.SelectMany(a => getChilds(e, a.ID)));
    //    }
    //    public List<DepartmentPolicyAuditResultItem> GetByDeparmtent(int page, int pageSize, out int totalCount, int departmentId)
    //    {
    //        var dbo = DepartmentPolicyAuditResultDA.Repository;
    //        var departmentChildIDs = getChilds(dbo.HumanDepartments, departmentId).Select(i => i.ID);
    //        var policyDepartments = dbo.DepartmentPolicies
    //                    .Select(item => new
    //                    {
    //                        ID = item.ID,
    //                        Name = item.Name,
    //                        ISOID = item.ISOID,
    //                        IsUse = item.IsUse,
    //                        TotalDepartmentFail = dbo.DepartmentPolicyAuditResults.Where(i => i.DepartmentPolicyID == item.ID && departmentChildIDs.Contains(i.HumanDepartmentID) && !i.IsPass).Count(),
    //                        PolicyDepartment = dbo.DepartmentPolicyAuditResults.Where(i => i.HumanDepartmentID == departmentId && i.DepartmentPolicyID == item.ID)
    //                            .OrderBy(i => i.AuditAt).FirstOrDefault()
    //                    })
    //                    .Select(item => new DepartmentPolicyAuditResultItem()
    //                    {
    //                        ID = item.PolicyDepartment == null ? 0 : item.PolicyDepartment.ID,
    //                        DepartmentPolicyID = item.ID,
    //                        PolicyName = item.Name,
    //                        ISOID = item.ISOID,
    //                        IsUse = item.IsUse,
    //                        QualityNCID = item.PolicyDepartment == null ? null : item.PolicyDepartment.QualityNCID,
    //                        IsObs = item.PolicyDepartment == null ? false : item.PolicyDepartment.QualityNC == null ? false : item.PolicyDepartment.QualityNC.IsObs,
    //                        IsMedium = item.PolicyDepartment == null ? false : item.PolicyDepartment.QualityNC == null ? false : item.PolicyDepartment.QualityNC.IsMedium,
    //                        IsMaximum = item.PolicyDepartment == null ? false : item.PolicyDepartment.QualityNC == null ? false : item.PolicyDepartment.QualityNC.IsMaximum,
    //                        Evidence = item.PolicyDepartment == null ? "" : item.PolicyDepartment.Evidence,
    //                        ResultName = item.PolicyDepartment == null ? null : (item.PolicyDepartment.IsPass ? "Đạt" : "Không Đạt"),
    //                        AuditAt = item.PolicyDepartment == null ? null : item.PolicyDepartment.AuditAt,
    //                        TotalFail = item.TotalDepartmentFail
    //                    })
    //                    .OrderByDescending(item => item.PolicyName)
    //                    .Page(page, pageSize, out totalCount)
    //                    .ToList();
    //        var ISODA = new CenterISOStandardDA();
    //        var result = new List<DepartmentPolicyAuditResultItem>();
    //        if (policyDepartments != null && policyDepartments.Count > 0)
    //        {
    //            foreach (var policyDeparmtent in policyDepartments)
    //            {
    //                policyDeparmtent.ISOName = ISODA.GetQuery(iso => iso.ID == policyDeparmtent.ISOID).Select(i => i.Name).FirstOrDefault();
    //                result.Add(policyDeparmtent);
    //            }
    //        }
    //        return result;
    //    }
    //    public List<DepartmentPolicyAuditResultItem> GetAuditResult(int departmentId, int policyId)
    //    {
    //        var DepartmentPolicyAuditResult = DepartmentPolicyAuditResultDA.GetQuery(i => i.HumanDepartmentID == departmentId && i.DepartmentPolicyID == policyId)
    //                    .Select(item => new DepartmentPolicyAuditResultItem()
    //                    {
    //                        ID = item.ID,
    //                        DepartmentPolicyID = item.DepartmentPolicyID,
    //                        PolicyName = item.DepartmentPolicy.Name,
    //                        Evidence = item.Evidence,
    //                        IsPass = item.IsPass,
    //                        AuditAt = item.AuditAt,
    //                    })
    //                    .OrderByDescending(item => item.PolicyName)
    //                    .ToList();
    //        return DepartmentPolicyAuditResult;
    //    }
    //    public DepartmentPolicyAuditResultItem GetById(int Id)
    //    {
    //        var dbo = DepartmentPolicyAuditResultDA.Repository;
    //        var DepartmentPolicyAuditResult = DepartmentPolicyAuditResultDA.GetQuery(i => i.ID == Id)
    //                    .Select(item => new DepartmentPolicyAuditResultItem()
    //                    {
    //                        ID = item.ID,
    //                        DepartmentPolicyID = item.DepartmentPolicyID,
    //                        PolicyName = item.DepartmentPolicy != null ? "" : item.DepartmentPolicy.Name,
    //                        HumanDepartmentID = item.HumanDepartmentID,
    //                        QualityNCID = item.QualityNCID,
    //                        HumanEmployee = new HumanEmployeeViewItem()
    //                        {
    //                            ID = item.AuditBy != null ? (int)item.AuditBy : 0,
    //                            Name = dbo.HumanEmployees.Where(i => i.ID == item.AuditBy).Select(i => i.Name).FirstOrDefault(),
    //                            Role = dbo.HumanOrganizations.Where(i => i.HumanEmployeeID == item.AuditBy).Select(i => i.HumanRole.Name).FirstOrDefault(),
    //                            Department = dbo.HumanOrganizations.Where(i => i.HumanEmployeeID == item.AuditBy).Select(i => i.HumanRole.HumanDepartment.Name).FirstOrDefault(),
    //                        },
    //                        Evidence = item.Evidence,
    //                        AuditNote = item.AuditNote,
    //                        IsPass = item.IsPass,
    //                        AuditAt = item.AuditAt,
    //                    })
    //                    .First();
    //        return DepartmentPolicyAuditResult;
    //    }
    //    public List<HumanDepartmentItem> GetDepartmentFails(int policyId, int departmentId)
    //    {
    //        var dbo = DepartmentPolicyAuditResultDA.Repository;
    //        var departmentChildIDs = getChilds(dbo.HumanDepartments, departmentId).Select(i => i.ID);
    //        var result = DepartmentPolicyAuditResultDA.GetQuery(i => i.DepartmentPolicyID == policyId & !i.IsPass
    //                        && departmentChildIDs.Contains(i.HumanDepartmentID))
    //                .Select(item => new HumanDepartmentItem()
    //                {
    //                    Name = item.HumanDepartment == null ? "" : item.HumanDepartment.Name
    //                }).ToList();
    //        return result;
    //    }
    //    public void Update(DepartmentPolicyAuditResultItem item, int userID)
    //    {
    //        var DepartmentPolicyAuditResult = DepartmentPolicyAuditResultDA.GetById(item.ID);
    //        DepartmentPolicyAuditResult.ID = item.ID;
    //        DepartmentPolicyAuditResult.QualityNCID = item.QualityNCID;
    //        DepartmentPolicyAuditResult.DepartmentPolicyID = item.DepartmentPolicyID;
    //        DepartmentPolicyAuditResult.HumanDepartmentID = item.HumanDepartmentID;
    //        DepartmentPolicyAuditResult.AuditBy = item.HumanEmployee.ID;
    //        DepartmentPolicyAuditResult.Evidence = item.Evidence;
    //        DepartmentPolicyAuditResult.IsPass = item.IsPass;
    //        DepartmentPolicyAuditResult.AuditAt = item.AuditAt;
    //        DepartmentPolicyAuditResult.AuditNote = item.AuditNote;
    //        DepartmentPolicyAuditResult.UpdateAt = DateTime.Now;
    //        DepartmentPolicyAuditResult.UpdateBy = userID;
    //        DepartmentPolicyAuditResultDA.Save();
    //    }
    //    public int Insert(DepartmentPolicyAuditResultItem item, int userID)
    //    {
    //        var DepartmentPolicyAuditResult = new DepartmentPolicyAuditResult()
    //        {
    //            DepartmentPolicyID = item.DepartmentPolicyID,
    //            HumanDepartmentID = item.HumanDepartmentID,
    //            AuditBy = item.HumanEmployee.ID,
    //            QualityNCID = item.QualityNCID,
    //            Evidence = item.Evidence,
    //            IsPass = item.IsPass,
    //            AuditAt = item.AuditAt,
    //            AuditNote = item.AuditNote,
    //            CreateAt = DateTime.Now,
    //            CreateBy = userID
    //        };
    //        DepartmentPolicyAuditResultDA.Insert(DepartmentPolicyAuditResult);
    //        DepartmentPolicyAuditResultDA.Save();
    //        return DepartmentPolicyAuditResult.ID;
    //    }
    //    public bool Delete(int id)
    //    {
    //        DepartmentPolicyAuditResultDA.Delete(id);
    //        DepartmentPolicyAuditResultDA.Save();
    //        return true;
    //    }
    //    public void InsertNC(int id, QualityNCItem item)
    //    {
    //        var qualityId = new QualityNCSV().Insert(item);
    //        var auditResult = DepartmentPolicyAuditResultDA.GetById(id);
    //        auditResult.QualityNCID = qualityId;
    //        DepartmentPolicyAuditResultDA.Save();
    //    }
    //}
}
