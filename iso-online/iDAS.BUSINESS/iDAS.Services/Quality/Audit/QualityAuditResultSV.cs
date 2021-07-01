using iDAS.Base;
using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
    public class QualityAuditResultSV
    {
        private QualityAuditResultDA resultDA = new QualityAuditResultDA();
        private iDAS.Services.QualityNCSV nCService = new iDAS.Services.QualityNCSV();
        private QualityCriteriaCategorySV criteriaCategoryService = new QualityCriteriaCategorySV();
        private QualityCriteriaDA criteriaDA = new QualityCriteriaDA();
        public QualityAuditResultItem GetByAuditIDAndCriterialID_Q(int departmentId, int qualityAuditProgramId, int qualityAuditProgramIsoId, int criteriaId)
        {
            var dbo = resultDA.Repository;
            QualityAuditResultItem obj = new QualityAuditResultItem();
            obj.ISOIndexCriteriaID = criteriaId;
            List<int> lstQualityNCID = dbo.QualityAuditResults.Where(t => t.ISOIndexCriteriaID == criteriaId && t.QualityNCID.HasValue).Select(t => t.QualityNCID.Value).ToList();
            var source = dbo.QualityCAPARequirements
                 .Where(i => lstQualityNCID.Contains(i.QualityNCID) && !i.IsComplete)
                .Select(item => new QualityCAPARequireItem()
                {
                    ID = item.ID,
                    Name = item.Name
                })
                .ToList();
            var result = resultDA.GetQuery(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoId && t.ISOIndexCriteriaID == criteriaId && t.HumanDepartmentID == departmentId).FirstOrDefault();
            if (result != null)
            {
                obj.AuditAt = result.AuditAt;
                obj.DepartmentID = result.HumanDepartmentID;
                obj.IsPass = result.IsPass;
                obj.QualityNCID = result.QualityNCID;
                obj.AuditNote = result.AuditNote;
                obj.ResultPass = result.AuditAt.HasValue && result.IsPass ? true : false;
                obj.ResultNotPass = result.AuditAt.HasValue && !result.IsPass ? true : false;
                obj.IsObs = result.QualityNC != null ? result.QualityNC.IsObs : false;
                obj.IsMaximum = result.QualityNC != null ? result.QualityNC.IsMaximum : false;
                obj.IsMedium = result.QualityNC != null ? result.QualityNC.IsMedium : false;
                obj.Create = new HumanEmployeeSV().GetEmployeeView(result.AuditBy);
            }
            else
            {
                obj.DepartmentID = departmentId;
                obj.IsPass = null;
            }
            obj.IsHasCAPARequirement = source.Count > 0 ? true : false;
            obj.QualityCAPARequirementID = source.Count > 0 ? source.FirstOrDefault().ID : 0;
            return obj;
        }
        public bool Delete(int id)
        {
            var dbo = resultDA.Repository;
            var obj = dbo.QualityAuditResults.Where(t => t.ID == id).FirstOrDefault();
            if (obj != null && obj.QualityNCID.HasValue)
            {
                if (dbo.QualityNCs.Where(t => t.ID == obj.QualityNCID).FirstOrDefault().IsVerify)
                    return false;
                else
                {
                    var objdelete = dbo.QualityNCs.Where(t => t.ID == obj.QualityNCID).FirstOrDefault();
                    dbo.QualityNCEmployees.RemoveRange(dbo.QualityNCEmployees.Where(i => i.QualityNCID == obj.QualityNCID));
                    dbo.QualityNCRoles.RemoveRange(dbo.QualityNCRoles.Where(i => i.QualityNCID == obj.QualityNCID));
                    dbo.QualityNCs.Remove(dbo.QualityNCs.FirstOrDefault(i => i.ID == obj.QualityNCID));
                    dbo.QualityNCs.Remove(objdelete);
                    dbo.SaveChanges();
                    resultDA.Delete(obj);
                    resultDA.Save();
                    return true;
                }
            }
            else
            {
                resultDA.Delete(obj);
                resultDA.Save();
                return true;
            }
        }
    }
}
