using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
    public class RiskRegulatorySV
    {
        private iDASBusinessEntities dbo = new RiskRegulatoryDA().Repository;
        public List<RiskRegulatoryItem> GetByDepartment(ModelFilter filter, int departmentId)
        {
            var riskItem = dbo.RiskRegulatories
                    .Where(t => t.Risk.HumanDepartmentID == departmentId)
                    .Select(i => new { risk = i.Risk, Regulatory = i.DepartmentRegulatory }).Distinct()
                    .Select(item => new RiskRegulatoryItem()
                    {
                        ID = item.risk.ID,
                        DepartmentRegulatoryName = item.Regulatory.Name,
                        RiskName = item.risk.Name,
                        RiskWeakness = item.risk.Weakness,
                        RiskOwner = new HumanEmployeeViewItem
                        {
                            Name = dbo.HumanEmployees.Where(m => m.ID == item.risk.HumanEmployeeID).Select(i => i.Name).FirstOrDefault(),
                        },
                        LikeLiHood = item.risk.LikeLiHood,
                        Impact = item.risk.Impact,
                        Consequence = item.risk.Consequence,
                        RiskLevelID = dbo.RiskLevels
                                    .Where(i => (i.PointMin <= item.risk.LikeLiHood * item.risk.Impact * item.risk.Consequence)
                                    && (i.PointMax >= item.risk.LikeLiHood * item.risk.Impact * item.risk.Consequence))
                                    .Select(i => i.ID).FirstOrDefault(),
                        IsSend = item.risk.IsSend,
                        CreateAt = item.risk.CreateAt
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
                             RegulatoryName = item.RiskRegulatories.Select(i => i.DepartmentRegulatory.Name).FirstOrDefault(),
                             RegulatoryID = item.RiskRegulatories.Select(i => i.ID).FirstOrDefault(),
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
                             LikeLiHood = item.LikeLiHood,
                             Impact = item.Impact,
                             Consequence = item.Consequence,
                             RiskValue = item.LikeLiHood * item.Impact * item.Consequence,
                             RiskLevelID = dbo.RiskLevels
                                     .Where(i => (i.PointMin <= item.LikeLiHood * item.Impact * item.Consequence)
                                     && (i.PointMax >= item.LikeLiHood * item.Impact * item.Consequence))
                                     .Select(i => i.ID).FirstOrDefault(),
                             Weakness = item.Weakness,
                             Action = item.Action
                         })
                         .First();
            return risk;
        }
        public void Insert(RiskItem item, int userID)
        {
            var insertItem = dbo.Risks.Add(new Risk()
            {
                Name = item.Name,
                HumanEmployeeID = item.RiskOwner.ID,
                HumanDepartmentID = item.Department.ID,
                Weakness = item.Weakness,
                RiskCategoryID = item.RiskCategoryID,
                IsFromRegulatory = true,
                Action = item.Action,
                LikeLiHood = item.LikeLiHood,
                Impact = item.Impact,
                Consequence = item.Consequence,
                CreateAt = DateTime.Now,
                CreateBy = userID
            });
            dbo.RiskRegulatories.Add(new RiskRegulatory()
            {
                RiskID = insertItem.ID,
                DepartmentRegulatoryID = item.RegulatoryID,
                CreateAt = DateTime.Now,
                CreateBy = userID
            });
            dbo.SaveChanges();
        }
        public void Update(RiskItem item, int userID)
        {
            var risk = dbo.Risks.Where(i => i.ID == item.ID).FirstOrDefault();
            if (!risk.IsSend)
            {
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
                var riskRegulatory = risk.RiskRegulatories.FirstOrDefault();
                if (riskRegulatory != dbo.RiskRegulatories.Where(i => i.ID == item.RegulatoryID).FirstOrDefault())
                {
                    if (riskRegulatory != null)
                    {
                        dbo.RiskRegulatories.Remove(riskRegulatory);
                    }
                    risk.RiskRegulatories.Add(new RiskRegulatory()
                    {
                        RiskID = item.ID,
                        DepartmentRegulatoryID = item.RegulatoryID,
                        CreateAt = DateTime.Now,
                        CreateBy = userID
                    });
                }
                dbo.SaveChanges();
            }
        }
        public void DeleteRisk(int riskId)
        {
            dbo.RiskRegulatories.RemoveRange(dbo.RiskRegulatories.Where(t => t.RiskID == riskId));
            dbo.Risks.Remove(dbo.Risks.FirstOrDefault(i => i.ID == riskId));
            dbo.SaveChanges();
        }
        public void Delete(int id)
        {
            dbo.RiskRegulatories.Remove(dbo.RiskRegulatories.FirstOrDefault(t => t.ID == id));
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
