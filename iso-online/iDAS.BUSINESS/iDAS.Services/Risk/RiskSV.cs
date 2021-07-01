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
    public class RiskSV
    {
        private RiskDA RiskDA = new RiskDA();
        private iDASBusinessEntities dbo = new RiskDA().Repository;
        private HumanEmployeeSV employeeSV = new HumanEmployeeSV();
        public List<RiskItem> GetForAudit(ModelFilter filter, int departmentId)
        {
            var dbo = RiskDA.Repository;
            var tasks = new List<RiskItem>();
            tasks = dbo.Risks.Where(t => t.HumanDepartmentID == departmentId).Select(item => new RiskItem
            {
                ID = item.ID,
                Name = item.Name,
                HumanDepartmentID = item.HumanDepartmentID,
                HumanEmployeeID = item.HumanEmployeeID,
                IsFromLegal = item.IsFromLegal,
                IsFromRegulatory = item.IsFromRegulatory,
                IsFromAction = item.IsFromAction,
                IsFromRequire = item.IsFromRequire,
                RiskCategoryID = item.RiskCategoryID,
                RiskOwner = new HumanEmployeeViewItem
                {
                    ID = item.HumanEmployeeID != null ? (int)item.HumanEmployeeID : 0,
                    Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == item.HumanEmployeeID).Name,
                    Role = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == item.HumanEmployeeID).HumanRole.Name,
                    Department = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == item.HumanEmployeeID).HumanRole.HumanDepartment.Name,

                },
                Weakness = item.Weakness,
                CreateAt = item.CreateAt,
                CreateBy = item.CreateBy,
                UpdateAt = item.UpdateAt,
                UpdateBy = item.UpdateBy
            })
               .Distinct()
               .Filter(filter)
               .ToList();

            return tasks;
        }
        public List<RiskLegalItem> GetRiskFromActionByDepartment(ModelFilter filter, int departmentId)
        {
            var riskItem = dbo.Risks
                    .Where(t => t.HumanDepartmentID == departmentId && t.IsFromAction)
                    .Select(item => new RiskLegalItem()
                    {
                        ID = item.ID,
                        DepartmentLegalName = item.Name,
                        RiskName = item.Name,
                        RiskWeakness = item.Weakness,
                        RiskOwner = new HumanEmployeeViewItem
                        {
                            Name = dbo.HumanEmployees.Where(m => m.ID == item.HumanEmployeeID).Select(i => i.Name).FirstOrDefault(),
                        },
                        LikeLiHood = item.LikeLiHood,
                        Impact = item.Impact,
                        Consequence = item.Consequence,
                        RiskLevelID = dbo.RiskLevels
                                    .Where(i => (i.PointMin <= item.LikeLiHood * item.Impact * item.Consequence)
                                    && (i.PointMax >= item.LikeLiHood * item.Impact * item.Consequence))
                                    .Select(i => i.ID).FirstOrDefault(),
                        IsSend = item.IsSend,
                        CreateAt = item.CreateAt
                    })
                     .Filter(filter)
                     .ToList();
            return riskItem;
        }
        public RiskItem GetById(int id)
        {
            var risk = dbo.Risks.Where(i => i.ID == id)
                         .Select(item => new RiskItem()
                         {
                             ID = item.ID,
                             Name = item.Name,
                             HumanDepartmentID = item.HumanDepartmentID,
                             HumanEmployeeID = item.HumanEmployeeID,
                             RiskCategoryID = item.RiskCategoryID,
                             LegalName = item.RiskLegals.Select(i => i.DepartmentLegal.Name).FirstOrDefault(),
                             LegalID = item.RiskLegals.Select(i => i.ID).FirstOrDefault(),
                             RiskOwner = new HumanEmployeeViewItem
                             {
                                 ID = item.HumanEmployeeID != null ? (int)item.HumanEmployeeID : 0,
                                 Name = dbo.HumanEmployees.FirstOrDefault(m => m.ID == item.HumanEmployeeID).Name,
                                 Role = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == item.HumanEmployeeID).HumanRole.Name,
                                 Department = dbo.HumanOrganizations.FirstOrDefault(m => m.HumanEmployeeID == item.HumanEmployeeID).HumanRole.HumanDepartment.Name,

                             },
                             Department = new HumanDepartmentViewItem
                             {
                                 ID = item.HumanDepartmentID,
                                 Name = item.HumanDepartment.Name
                             },
                             Weakness = item.Weakness,
                             Action = item.Action,
                             LikeLiHood = item.LikeLiHood,
                             Impact = item.Impact,
                             Consequence = item.Consequence,
                             RiskValue = item.LikeLiHood * item.Impact * item.Consequence,
                             RiskLevelID = dbo.RiskLevels
                                     .Where(i => (i.PointMin <= item.LikeLiHood * item.Impact * item.Consequence)
                                     && (i.PointMax >= item.LikeLiHood * item.Impact * item.Consequence))
                                     .Select(i => i.ID).FirstOrDefault()
                         })
                         .First();
            return risk;
        }
        public void InsertRiskFromAction(RiskItem item, int userID)
        {
            var insertItem = dbo.Risks.Add(new Risk()
            {
                Name = item.Name,
                HumanEmployeeID = item.RiskOwner.ID,
                HumanDepartmentID = item.Department.ID,
                Weakness = item.Weakness,
                RiskCategoryID = item.RiskCategoryID,
                IsFromAction = true,
                Action = item.Action,
                LikeLiHood = item.LikeLiHood,
                Impact = item.Impact,
                Consequence = item.Consequence,
                CreateAt = DateTime.Now,
                CreateBy = userID
            });
            dbo.SaveChanges();
        }
        public void UpdateRiskFromAction(RiskItem item, int userID)
        {
            var risk = dbo.Risks.Where(i => i.ID == item.ID).FirstOrDefault();
            risk.HumanDepartmentID = item.Department.ID;
            risk.HumanEmployeeID = item.RiskOwner.ID;
            risk.Name = item.Name;
            risk.Weakness = item.Weakness;
            risk.Action = item.Action;
            risk.RiskCategoryID = item.RiskCategoryID;
            risk.LikeLiHood = item.LikeLiHood;
            risk.Impact = item.Impact;
            risk.Consequence = item.Consequence;
            risk.UpdateAt = DateTime.Now;
            risk.UpdateBy = userID;
            dbo.SaveChanges();
        }
        public void Delete(int id)
        {
            dbo.RiskLegals.Remove(dbo.RiskLegals.FirstOrDefault(t => t.ID == id));
            dbo.SaveChanges();
        }
        public RiskItem GetAnalytic(int Id)
        {
            var riskControl = dbo.Risks.Where(i => i.ID == Id)
                        .OrderByDescending(i => i.CreateAt)
                        .Select(item => new RiskItem()
                        {
                            ID = item.ID,
                            LikeLiHood = item.LikeLiHood,
                            Impact = item.Impact,
                            Consequence = item.Consequence,
                            RiskValue = item.LikeLiHood * item.Impact * item.Consequence,
                            RiskLevelID = dbo.RiskLevels
                                    .Where(i => (i.PointMin <= item.LikeLiHood * item.Impact * item.Consequence)
                                    && (i.PointMax >= item.LikeLiHood * item.Impact * item.Consequence))
                                    .Select(i => i.ID).FirstOrDefault(),
                            IsNew = !item.IsSend,
                        })
                        .FirstOrDefault();
            return riskControl;
        }
        public bool Analytic(RiskItem item, int userId)
        {
            var risk = dbo.Risks.Where(i => i.ID == item.ID).FirstOrDefault();
            risk.LikeLiHood = item.LikeLiHood;
            risk.Impact = item.Impact;
            risk.Consequence = item.Consequence;
            if (item.IsNew)
            {

                risk.IsSend = false;
            }
            else
            {
                if (risk.IsSend == true)
                {
                    return false;
                }
            }
            risk.UpdateAt = DateTime.Now;
            risk.UpdateBy = userId;
            dbo.SaveChanges();
            return true;
        }
        public void SendRisk(int id, int userID)
        {
            var risk = dbo.Risks.Where(i => i.ID == id).FirstOrDefault();
            if (risk != null && risk.IsSend != true)
            {
                new RiskControlSV().InsertRiskValues(
                    new RiskControlItem()
                    {
                        RiskID = id,
                        IsEdit = true,
                        NowImpact = risk.Impact,
                        NowLikeLiHood = risk.LikeLiHood,
                        NowConsequence = risk.Consequence,
                        CreateAt = DateTime.Now,
                        CreateBy = userID
                    },
                                userID
                    );
            }
            risk.IsSend = true;
            dbo.SaveChanges();
        }
    }
}
