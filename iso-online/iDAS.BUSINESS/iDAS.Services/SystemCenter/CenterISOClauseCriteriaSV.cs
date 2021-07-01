using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.DA;
using iDAS.Items;
using iDAS.Core;

namespace iDAS.Services
{
    public class ISOStandartCriteriaSV
    {
        private CenterISOIndexCriteriaDA centerISOIndexCriteriaDA = new CenterISOIndexCriteriaDA();
        private iDAS.Services.QualityNCSV nCService = new iDAS.Services.QualityNCSV();
        private QualityAuditResultDA qualityAuditResultDA = new QualityAuditResultDA();

        public CenterISOIndexCriteriaItem GetByID(int id)
        {
            var item = centerISOIndexCriteriaDA.GetById(id);
            var obj = new CenterISOIndexCriteriaItem()
            {
                ID = item.ID,
                Name = item.Name,
                Content = item.Content,
                ISOIndexID = item.ISOIndexID,
                IsDelete = item.IsDelete,
                IsUse = item.IsUse,
            };
            return obj;
        }
        public List<CenterISOIndexCriteriaItem> GetByIsoIndexID(int page, int pageSize, out int totalCount, int isoIndexID)
        {
            var result = centerISOIndexCriteriaDA.GetQuery(p => p.ISOIndexID == isoIndexID && !p.IsDelete && p.IsUse)
                             .OrderBy(a => a.Name)
                             .Select(item => new CenterISOIndexCriteriaItem()
                             {
                                 ID = item.ID,
                                 Name = item.Name,
                                 Content = item.Content,
                                 ISOIndexID = item.ISOIndexID,
                                 IsDelete = item.IsDelete,
                                 IsUse = item.IsUse,
                                 CreateAt = item.CreateAt,
                             })
                             .OrderByDescending(item => item.CreateAt)
                             .Page(page, pageSize, out totalCount)
                             .ToList();
            return result;
        }
        //private int GetNCID(int criteriaId)
        //{
        //    var rs = qualityAuditResultDA.GetQuery(t => t.ISOIndexCriteriaID == criteriaId).OrderByDescending(t => t.Index).ToList();
        //    if (rs.Count != 0)
        //    {
        //        if (rs.FirstOrDefault().QualityNC != null)
        //            return (int)rs.FirstOrDefault().QualityNCID;
        //        return 0;
        //    }
        //    return 0;

        //}
        //public bool SetEditResult(int criteriaId)
        //{
        //    var rs = qualityAuditResultDA.GetQuery(t => t.ISOIndexCriteriaID == criteriaId).OrderByDescending(t => t.Index).FirstOrDefault();
        //    if (rs != null)
        //    {
        //        if (rs.QualityNC == null || rs.QualityNC.IsCAPA)
        //            return true;
        //        return false;
        //    }
        //    else
        //        return true;

        //}
        //public void Delete_Q(int qualityAuditId, int criteriaId)
        //{
        //    var result = qualityAuditResultDA.GetQuery(t => t.QualityAuditProgramISOIndexID == qualityAuditId && t.ISOIndexCriteriaID == criteriaId).FirstOrDefault();
        //    if (result != null)
        //        qualityAuditResultDA.Delete(result);
        //    qualityAuditResultDA.Save();

        //}
        public bool CheckForUpdate(int ncId=0) {
            var dbo = qualityAuditResultDA.Repository;
            if (ncId == 0)
                return true;
            else
            {
                if (dbo.QualityNCs.Where(t => t.ID == ncId).FirstOrDefault().IsVerify)
                    return false;
                else
                {
                    return true;
                }
            }
        }
        public void InsertOrUpdate(QualityAuditResultItem obj, int userId, int employeeId)
        {
            var dbo = qualityAuditResultDA.Repository;
            var result = qualityAuditResultDA.GetQuery(t => t.QualityAuditProgramISOID == obj.QualityAuditProgramISOID && t.ISOIndexCriteriaID == obj.ISOIndexCriteriaID && t.HumanDepartmentID== obj.DepartmentID).FirstOrDefault();
            if (result != null)
            {
                result.AuditNote = obj.AuditNote;
                result.ISOIndexCriteriaID = obj.ISOIndexCriteriaID;
                if (result.QualityAuditProgramISOID != 0 || result.QualityAuditProgramISOID != null)
                {
                    result.QualityAuditProgramISOID = obj.QualityAuditProgramISOID;
                }
                else
                {
                    result.QualityAuditProgramISOID = obj.QualityAuditProgramISOID;
                }
                result.IsPass = (bool)obj.IsPass;
                result.HumanDepartmentID = obj.DepartmentID;
                result.UpdateAt = DateTime.Now;
                result.UpdateBy = userId;
                result.AuditAt = DateTime.Now;
                result.AuditBy = employeeId;
                if (obj.QualityNCID.HasValue && obj.IsPass.Value)
                {
                    var objdelete = dbo.QualityNCs.Where(t => t.ID == obj.QualityNCID).FirstOrDefault();
                    dbo.QualityNCEmployees.RemoveRange(dbo.QualityNCEmployees.Where(i => i.QualityNCID == obj.QualityNCID));
                    dbo.QualityNCRoles.RemoveRange(dbo.QualityNCRoles.Where(i => i.QualityNCID == obj.QualityNCID));
                    dbo.QualityNCs.Remove(dbo.QualityNCs.FirstOrDefault(i => i.ID == obj.QualityNCID));
                    dbo.QualityNCs.Remove(objdelete);
                    dbo.SaveChanges();
                }
                qualityAuditResultDA.Update(result);
                if (obj.IsCorrertive)
                {
                    var capa = dbo.QualityCAPARequirements.Where(t => t.ID == obj.QualityCAPARequirementID).FirstOrDefault();
                    capa.IsAuditBack = true;
                    capa.AuditBackPass = obj.IsPass;
                    capa.AuditBackTime = DateTime.Now;
                    capa.AuditBackNote = obj.AuditNote;
                    capa.AuditBackBy = userId;
                }
            }
            else
            {
                QualityAuditResult objI = new QualityAuditResult();
                objI.AuditNote = obj.AuditNote;
                objI.ISOIndexCriteriaID = obj.ISOIndexCriteriaID;
                if (obj.QualityAuditProgramISOID == 0 || obj.QualityAuditProgramISOID == null)
                {
                    objI.QualityAuditProgramISOID = obj.QualityAuditProgramISOID;
                }
                else
                {
                    objI.QualityAuditProgramISOID = obj.QualityAuditProgramISOID;
                }
                objI.IsPass = (bool)obj.IsPass;
                objI.CreateAt = DateTime.Now;
                objI.CreateBy = userId;
                objI.AuditAt = DateTime.Now;
                objI.AuditBy = employeeId;
                objI.HumanDepartmentID = obj.DepartmentID;
                qualityAuditResultDA.Insert(objI);
                if (obj.IsCorrertive)
                {
                    var capa = dbo.QualityCAPARequirements.Where(t => t.ID == obj.QualityCAPARequirementID).FirstOrDefault();
                    capa.IsAuditBack = true;
                    capa.AuditBackPass = obj.IsPass;
                    capa.AuditBackTime = DateTime.Now;
                    capa.AuditBackNote = obj.AuditNote;
                    capa.AuditBackBy = userId;
                }
            }
            qualityAuditResultDA.Save();
        }

        public List<QualityAuditResultItem> GetByQualityAuditID_Q(ModelFilter filter, int qualityAuditProgramId, int cateId, int qualityAuditProgramIsoId)
        {
            var dbo = centerISOIndexCriteriaDA.Repository;
            var dboBussiness = qualityAuditResultDA.Repository;
            List<QualityAuditResultItem> lst = new List<QualityAuditResultItem>();
            var department = dboBussiness.QualityAuditProgramDepartments.Where(t => t.QualityAuditProgramID == qualityAuditProgramId).Select(t => t.HumanDepartmentID).ToList();
            if (department.Count > 0)
            {
                var result = centerISOIndexCriteriaDA.GetQuery().Where(t => t.ISOIndexID == cateId).OrderBy(t => t.Name).Filter(filter).ToList();
                foreach (var depart in department)
	            {
                if (result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        lst.Add(new QualityAuditResultItem
                        {
                            ID = dboBussiness.QualityAuditResults.FirstOrDefault(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoId && t.ISOIndexCriteriaID == item.ID && t.HumanDepartmentID == depart) != null ? 
                            dboBussiness.QualityAuditResults.FirstOrDefault(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoId && t.ISOIndexCriteriaID == item.ID && t.HumanDepartmentID == depart).ID : 0,
                            ISOIndexCriteriaID = item.ID,
                            ISOIndexCriteriaName = item.Name,
                            DepartmentID = depart,
                            DepartmentName = dboBussiness.HumanDepartments.Where(t => t.ID == depart).FirstOrDefault().Name,
                            AuditAt = dboBussiness.QualityAuditResults.FirstOrDefault(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoId && t.ISOIndexCriteriaID == item.ID && t.HumanDepartmentID == depart) != null ? dboBussiness.QualityAuditResults.FirstOrDefault(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoId && t.ISOIndexCriteriaID == item.ID && t.HumanDepartmentID == depart).AuditAt : null,
                            IsPass = dboBussiness.QualityAuditResults.FirstOrDefault(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoId && t.ISOIndexCriteriaID == item.ID && t.HumanDepartmentID == depart) != null ? dboBussiness.QualityAuditResults.FirstOrDefault(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoId && t.ISOIndexCriteriaID == item.ID && t.HumanDepartmentID == depart).IsPass : false,
                            AuditNote = dboBussiness.QualityAuditResults.FirstOrDefault(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoId && t.ISOIndexCriteriaID == item.ID && t.HumanDepartmentID == depart) != null ? dboBussiness.QualityAuditResults.FirstOrDefault(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoId && t.ISOIndexCriteriaID == item.ID && t.HumanDepartmentID == depart).AuditNote : string.Empty,
                            ResultPass = dboBussiness.QualityAuditResults.FirstOrDefault(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoId && t.ISOIndexCriteriaID == item.ID && t.HumanDepartmentID == depart) != null ? dboBussiness.QualityAuditResults.FirstOrDefault(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoId && t.ISOIndexCriteriaID == item.ID && t.HumanDepartmentID == depart).IsPass ? true : false : false,
                            ResultNotPass = dboBussiness.QualityAuditResults.FirstOrDefault(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoId && t.ISOIndexCriteriaID == item.ID && t.HumanDepartmentID == depart) != null ? !dboBussiness.QualityAuditResults.FirstOrDefault(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoId && t.ISOIndexCriteriaID == item.ID && t.HumanDepartmentID == depart).IsPass ? true : false : false,
                            
                            IsObs = dboBussiness.QualityAuditResults.FirstOrDefault(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoId && t.ISOIndexCriteriaID == item.ID && t.HumanDepartmentID == depart) != null ? 
                            dboBussiness.QualityAuditResults.FirstOrDefault(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoId && t.ISOIndexCriteriaID == item.ID && t.HumanDepartmentID == depart).IsPass ? false :
                            dboBussiness.QualityAuditResults.FirstOrDefault(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoId && t.ISOIndexCriteriaID == item.ID && t.HumanDepartmentID == depart) != null ? 
                            dboBussiness.QualityAuditResults.FirstOrDefault(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoId && t.ISOIndexCriteriaID == item.ID && t.HumanDepartmentID == depart).QualityNC != null ? 
                            dboBussiness.QualityAuditResults.FirstOrDefault(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoId && t.ISOIndexCriteriaID == item.ID && t.HumanDepartmentID == depart).QualityNC.IsObs : false : false : false,
                            
                            IsMaximum = dboBussiness.QualityAuditResults.FirstOrDefault(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoId && t.ISOIndexCriteriaID == item.ID && t.HumanDepartmentID == depart) != null ?
                            dboBussiness.QualityAuditResults.FirstOrDefault(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoId && t.ISOIndexCriteriaID == item.ID && t.HumanDepartmentID == depart).IsPass ? false :
                            dboBussiness.QualityAuditResults.FirstOrDefault(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoId && t.ISOIndexCriteriaID == item.ID && t.HumanDepartmentID == depart) != null ? 
                            dboBussiness.QualityAuditResults.FirstOrDefault(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoId && t.ISOIndexCriteriaID == item.ID && t.HumanDepartmentID == depart).QualityNC != null ? 
                            dboBussiness.QualityAuditResults.FirstOrDefault(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoId && t.ISOIndexCriteriaID == item.ID && t.HumanDepartmentID == depart).QualityNC.IsMaximum : false : false : false,
                            
                            
                            IsMedium = dboBussiness.QualityAuditResults.FirstOrDefault(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoId && t.ISOIndexCriteriaID == item.ID && t.HumanDepartmentID == depart) != null ?
                            dboBussiness.QualityAuditResults.FirstOrDefault(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoId && t.ISOIndexCriteriaID == item.ID && t.HumanDepartmentID == depart).IsPass ? false :
                            dboBussiness.QualityAuditResults.FirstOrDefault(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoId && t.ISOIndexCriteriaID == item.ID && t.HumanDepartmentID == depart) != null ?
                            dboBussiness.QualityAuditResults.FirstOrDefault(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoId && t.ISOIndexCriteriaID == item.ID && t.HumanDepartmentID == depart).QualityNC != null ? 
                            dboBussiness.QualityAuditResults.FirstOrDefault(t => t.QualityAuditProgramISOID == qualityAuditProgramIsoId && t.ISOIndexCriteriaID == item.ID && t.HumanDepartmentID == depart).QualityNC.IsMedium : false : false : false

                        });
                    }
                }
                }
            }
            return lst;
        }
    }
}
