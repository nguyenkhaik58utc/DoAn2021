using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;
using iDAS.Items;
using iDAS.DA;
namespace iDAS.Services
{
    public class QualityCAPARequirementSV
    {
        private QualityCAPARequirementDA CAPARequireDA = new QualityCAPARequirementDA();
        public List<QualityCAPARequireItem> GetByNC(int page, int pageSize, out int totalCount, int ncID)
        {
            var dbo = CAPARequireDA.Repository;
            var source = dbo.QualityCAPARequirements
                 .Where(i => i.QualityNCID == ncID)
                .Select(item => new QualityCAPARequireItem()
                {
                    ID = item.ID,
                    Name = item.Name,
                    Code = item.Code,
                    NCID = item.QualityNCID,
                    Department = new HumanDepartmentViewItem()
                    {
                        ID = item.DepartmentID,
                        Name = dbo.HumanDepartments.FirstOrDefault(m => m.ID == item.DepartmentID).Name
                    },
                    Content = item.Content,
                    CompleteTime = item.CompleteTime,
                    CompleteRealTime = item.CompleteRealTime,
                    IsCompleteAuto = item.IsComplete,
                    IsComplete = item.IsComplete,
                    ParentID = item.ParentID,
                    CreateBy = item.CreateBy,
                })
                .OrderByDescending(i => i.CreateBy)
                .Page(page, pageSize, out totalCount)
                .ToList();

            return source;
        }
        public List<QualityCAPAItem> GetByDepartment(int page, int pageSize, out int totalCount, int departmentId, int focusId = 0)
        {
            var dbo = CAPARequireDA.Repository;
            IQueryable<iDAS.Base.QualityCAPARequirement> query;
            if (focusId != 0)
            {
                query = dbo.QualityCAPARequirements.Where(t => t.ID == focusId);
            }
            else
            {
                query = dbo.QualityCAPARequirements.Where(i => i.DepartmentID == departmentId || departmentId == 1);
            }
            var source = query
                .Select(item => new QualityCAPAItem()
                            {
                                ID = item.QualityCAPAs.FirstOrDefault() == null ? 0 : item.QualityCAPAs.FirstOrDefault().ID,
                                RequireID = item.ID,
                                Name = item.Name,
                                Code = item.Code,
                                CompleteTime = item.CompleteTime,
                                CompleteRealTime = item.CompleteRealTime,
                                IsComplete = item.IsComplete,
                                CreateBy = item.CreateBy,
                                // -- Trạng thái cho yêu cầu------------------------------------------------------------------------------------------
                                IsEdit = item.QualityCAPAs.FirstOrDefault() == null ? true : item.QualityCAPAs.FirstOrDefault().IsEdit,
                                IsAccept = item.QualityCAPAs.FirstOrDefault() == null ? false : item.QualityCAPAs.FirstOrDefault().IsAccept,
                                IsApproval = item.QualityCAPAs.FirstOrDefault() == null ? false : item.QualityCAPAs.FirstOrDefault().IsApproval,
                                //---------------------------------------------------------------------------------------------------------------------
                                IsPerformming = item.QualityCAPAs.FirstOrDefault() != null
                                            && dbo.QualityCAPATasks.FirstOrDefault(i => i.QualityCAPA.QualityCAPARequirement.ID == item.ID) != null
                                            && dbo.QualityCAPATasks.Where(i => i.QualityCAPA.QualityCAPARequirement.ID == item.ID)
                                                    .FirstOrDefault(i => i.Task.IsComplete == false) != null,
                                IsOverTime = item.QualityCAPAs.FirstOrDefault() != null
                                             && dbo.QualityCAPATasks.FirstOrDefault(i => i.Task.IsComplete == false) != null
                                             && DateTime.Now > item.CompleteTime,
                                IsCaculatorComplete = item.QualityCAPAs.FirstOrDefault() != null
                                            && dbo.QualityCAPATasks.FirstOrDefault(i => i.QualityCAPA.QualityCAPARequirement.ID == item.ID) != null
                                           && dbo.QualityCAPATasks.FirstOrDefault(i => i.QualityCAPA.QualityCAPARequirement.ID == item.ID &&
                                                                                        i.Task.IsComplete == false) == null,
                            }
                 )
                .OrderByDescending(i => i.CreateBy)
                .Page(page, pageSize, out totalCount)
                .ToList();
            return source;
        }
        public List<QualityCAPARequireItem> GetNotCorrectiveByCriteriaID(int qualityCriteriaID)
        {
            var dbo = CAPARequireDA.Repository;
            List<int> lstQualityNCID = dbo.AuditResults.Where(t => t.QualityCriteriaID == qualityCriteriaID && t.QualityNCID.HasValue).Select(t => t.QualityNCID.Value).ToList();
            var source = dbo.QualityCAPARequirements
                 .Where(i => lstQualityNCID.Contains(i.QualityNCID) && !i.IsComplete)
                .Select(item => new QualityCAPARequireItem()
                {
                    ID = item.ID,
                    Name = item.Name
                })
                .ToList();
            return source;
        }
        public List<QualityCAPARequireItem> GetNotCorrectiveByCriteriaID_Q(int qualityCriteriaID)
        {
            var dbo = CAPARequireDA.Repository;
            List<int> lstQualityNCID = dbo.QualityAuditResults.Where(t => t.ISOIndexCriteriaID == qualityCriteriaID && t.QualityNCID.HasValue).Select(t => t.QualityNCID.Value).ToList();
            var source = dbo.QualityCAPARequirements
                 .Where(i => lstQualityNCID.Contains(i.QualityNCID) && !i.IsComplete)
                .Select(item => new QualityCAPARequireItem()
                {
                    ID = item.ID,
                    Name = item.Name
                })
                .ToList();
            return source;
        }
        public QualityCAPARequireItem GetByID(int id)
        {
            var dbo = CAPARequireDA.Repository;
            var CAPA = dbo.QualityCAPARequirements.Where(i => i.ID == id)
                            .Select(item => new QualityCAPARequireItem()
                            {
                                ID = item.ID,
                                Name = item.Name,
                                Code = item.Code,
                                NCID = item.QualityNCID,
                                Department = new HumanDepartmentViewItem()
                                {
                                    ID = item.DepartmentID,
                                    Name = dbo.HumanDepartments.FirstOrDefault(m => m.ID == item.DepartmentID).Name
                                },
                                Content = item.Content,
                                CompleteTime = item.CompleteTime,
                                CompleteRealTime = item.CompleteRealTime,
                                IsCompleteAuto = item.IsComplete,
                                IsComplete = item.IsComplete,
                                ParentID = item.ParentID,
                                Represent = new HumanEmployeeViewItem()
                                {
                                    ID = item.EmployeeID==null?0:(int) item.EmployeeID,
                                    Name = dbo.HumanEmployees.Where(m => m.ID == item.EmployeeID).Select(i=>i.Name).FirstOrDefault(),
                                    Role = dbo.HumanOrganizations.Where(m => m.HumanEmployeeID == item.EmployeeID).Select(i=>i.HumanRole.Name).FirstOrDefault(),
                                    Department = dbo.HumanOrganizations.Where(m => m.HumanEmployeeID == item.EmployeeID)
                                                        .Select(i => i.HumanRole.HumanDepartment.Name).FirstOrDefault()
                                }
                            }
                            )
                            .First();
            return CAPA;
        }
        public void Insert(QualityCAPARequireItem item)
        {
            var CAPA = new QualityCAPARequirement()
            {
                Name = item.Name,
                Code = item.Code,
                QualityNCID = item.NCID,
                DepartmentID = item.Department.ID,
                Content = item.Content,
                CompleteTime = item.CompleteTime,
                CompleteRealTime = item.CompleteRealTime,
                IsCompleteAuto = item.IsComplete,
                IsComplete = item.IsComplete,
                ParentID = item.ParentID,
                EmployeeID = item.Represent.ID,
                CreateBy = item.CreateBy,
                CreateAt = DateTime.Now
            };
            CAPARequireDA.Insert(CAPA);
            CAPARequireDA.Save();
        }
        public void Update(QualityCAPARequireItem item)
        {
            var CAPA = CAPARequireDA.GetById(item.ID);
            CAPA.Name = item.Name;
            CAPA.Code = item.Code;
            CAPA.QualityNCID = item.NCID;
            CAPA.DepartmentID = item.Department.ID;
            CAPA.Content = item.Content;
            CAPA.CompleteTime = item.CompleteTime;
            CAPA.CompleteRealTime = item.CompleteRealTime;
            CAPA.IsCompleteAuto = item.IsComplete;
            CAPA.IsComplete = item.IsComplete;
            CAPA.ParentID = item.ParentID;
            CAPA.EmployeeID = item.Represent.ID;
            CAPA.UpdateAt = DateTime.Now;
            CAPA.UpdateBy = item.UpdateBy;
            CAPARequireDA.Save();
        }
        public void Delete(int id)
        {
            CAPARequireDA.Delete(id);
            CAPARequireDA.Save();
        }
    }
}
