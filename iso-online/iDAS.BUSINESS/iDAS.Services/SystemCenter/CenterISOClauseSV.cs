using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class ISOClauseSV
    {
        private CenterISOIndexDA iSOClauseDA = new CenterISOIndexDA();
        private CenterModuleSV centerModuleSV = new CenterModuleSV();
        private ISOStandartCriteriaSV iSOStandartCriteriaSV = new ISOStandartCriteriaSV();
        private QualityAuditDepartmentDA qualityAuditDepartmentDA = new QualityAuditDepartmentDA();
        public List<CenterISOIndexCriteriaItem> GetByIsoIndexID(int page, int pageSize, out int totalCount, int isoIndexID)
        {
            return iSOStandartCriteriaSV.GetByIsoIndexID(page, pageSize, out totalCount, isoIndexID);
        }
        public List<CenterModuleItem> GetModulesByISOID(int isoId)
        {
         return  centerModuleSV.GetModulesByISOID(isoId);
        }
        private string GetListModuleByClause(int clauseId)
        {
            var dbo = iSOClauseDA.Repository;
            string lst = "";
            var no = 1;
            //Vinh sửa theo yêu cầu của nhung.Them dieu kien where && t.CenterModule.SystemCode == "BUSINESS"
            var data = dbo.ISOIndexModules.Where(t => t.ISOIndexID == clauseId && t.CenterModule.SystemCode.Equals("BUSINESS") && !t.IsDelete && t.IsUse).Select(t => t.CenterModule.Name).ToList();
            foreach (var item in data)
            {
                lst += no + ". " + item + "<br/>";
                no++;
            }
            return lst;
        }
        private string GetListModuleByISO(int isoId)
        {
            var dbo = iSOClauseDA.Repository;
            string lst = "";
            var no = 1;
            //Vinh sửa theo yêu cầu của nhung.Them dieu kien where && t.CenterModule.SystemCode == "BUSINESS"
            var data = dbo.ISOStandardModules.Where(t => t.ISOStandardID == isoId && t.CenterModule.SystemCode.Equals("BUSINESS") && t.IsActive && t.IsShow).Select(t => t.CenterModule.Name).ToList();
            foreach (var item in data)
            {
                lst += no + ". " + item + "<br/>";
                no++;
            }
            return lst;
        }
        public List<CenterISOClauseItem> GetAll(int page, int pageSize, out int total, int isoId)
        {
            var dbo = iSOClauseDA.Repository;
            List<CenterISOClauseItem> lst = new List<CenterISOClauseItem>();
            var standards = dbo.ISOIndexes
                          .Where(t => t.ISOStandardID == isoId)
                          .Where(t=>t.IsActive && !t.IsDelete)
                          .OrderByDescending(item => item.CreateAt)
                          .Page(page, pageSize, out total)
                          .ToList();
            if (standards.Count > 0)
            {
                foreach (var item in standards)
                {
                    lst.Add(new CenterISOClauseItem()
                    {
                        ID = item.ID,
                        Name = item.Name,
                        ListModuleName = GetListModuleByClause(item.ID),
                        Clause = item.Clause,
                        CreateAt = item.CreateAt
                    });
                }
            }
            return lst;
        }
        public List<CenterISOClauseItem> GetAll(int isoId)
        {
            var dbo = iSOClauseDA.Repository;
            List<CenterISOClauseItem> lst = new List<CenterISOClauseItem>();
            var standards = dbo.ISOIndexes
                          .Where(t => t.ISOStandardID == isoId)
                          .Where(t => t.IsActive && !t.IsDelete)
                          .OrderByDescending(item => item.CreateAt)
                          .ToList();
            if (standards.Count > 0)
            {
                foreach (var item in standards)
                {
                    lst.Add(new CenterISOClauseItem()
                    {
                        ID = item.ID,
                        Name = item.Name,
                        ListModuleName = GetListModuleByClause(item.ID),
                        Clause = item.Clause,
                        CreateAt = item.CreateAt
                    });
                }
            }
            return lst;
        }
        public List<CenterISOClauseItem> GetAllEdit(int isoId, int programId)
        {
            var dbo = iSOClauseDA.Repository;
            var dbobusiness = qualityAuditDepartmentDA.Repository;
            List<CenterISOClauseItem> lst = new List<CenterISOClauseItem>();
            var standards = dbo.ISOIndexes
                          .Where(t => t.ISOStandardID == isoId)
                          .Where(t => t.IsActive && !t.IsDelete)
                          .OrderByDescending(item => item.CreateAt)
                          .ToList();
            if (standards.Count > 0)
            {
                foreach (var item in standards)
                {
                    lst.Add(new CenterISOClauseItem()
                    {
                        ID = item.ID,
                        Name = item.Name,
                        ListModuleName = GetListModuleByClause(item.ID),
                        Clause = item.Clause,
                        CreateAt = item.CreateAt,
                        IsSelected = dbobusiness.QualityAuditProgramISOes.Any(t=>t.QualityAuditProgramID==programId && t.ISOIndexID==item.ID)
                    });
                }
            }
            return lst;
        }
        public CenterISOClauseItem GetByID(int id)
        {
            var standards = iSOClauseDA.GetQuery(t => t.ID == id)
                          .Select(item => new CenterISOClauseItem()
                          {
                              ID = item.ID,
                              Name = item.Name,
                              Note = item.Note,
                              Clause = item.Clause,
                              CreateAt = item.CreateAt
                          })
                          .FirstOrDefault();
            return standards;
        }
    }
}
