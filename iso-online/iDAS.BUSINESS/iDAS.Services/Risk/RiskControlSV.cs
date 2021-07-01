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
    public class RiskControlSV
    {
        private RiskControlDA RiskControlDA = new RiskControlDA();
        public List<RiskControlItem> GetAll(ModelFilter filter)
        {
            var RiskControl = RiskControlDA.GetQuery()
                        .Select(item => new RiskControlItem()
                        {
                            ID = item.ID,
                            Controls = item.Controls,
                            LikeLiHood = item.LikeLiHood,
                            RiskID = item.RiskID,
                            RiskTreatmentID = item.RiskTreatmentID,
                            IsEdit = item.IsEdit,
                            IsAccept = item.IsAccept,
                            IsApproval = item.IsApproval,
                            ApprovalAt = item.ApprovalAt,
                            ApprovalBy = item.ApprovalBy,
                            ApprovalNote = item.Note,
                            CompleteTime = item.CompleteTime,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            return RiskControl;
        }
        public List<RiskItem> GetRisk(ModelFilter filter, int departmentId)
        {
            var dbo = RiskControlDA.Repository;
            return dbo.Risks.Where(i => i.HumanDepartmentID == departmentId)
                        .Select(item => new RiskItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            IsFromLegal = item.IsFromLegal,
                            IsFromRegulatory = item.IsFromRegulatory,
                            IsFromRequire = item.IsFromRequire,
                            IsFromAction = item.IsFromAction,
                            RiskCategoryID = item.RiskCategoryID,
                            CreateAt = item.CreateAt
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
        }
        public List<RiskControlItem> GetByRisk(ModelFilter filter, int RiskID)
        {
            var RiskControl = RiskControlDA.GetQuery(i => i.RiskID == RiskID)
                        .Select(item => new RiskControlItem()
                        {
                            ID = item.ID,
                            Controls = item.Controls,
                            LikeLiHood = item.LikeLiHood,
                            Impact = item.NowImpact,
                            Consequence = item.NowConsequence,
                            NowLikeLiHood = item.NowLikeLiHood,
                            NowImpact = item.NowImpact,
                            NowConsequence = item.NowConsequence,
                            RiskID = item.RiskID,
                            RiskTreatmentID = item.RiskTreatmentID,
                            IsEdit = item.IsEdit,
                            IsAccept = item.IsAccept,
                            IsApproval = item.IsApproval,
                            ApprovalAt = item.ApprovalAt,
                            ApprovalBy = item.ApprovalBy,
                            ApprovalNote = item.Note,
                            CompleteTime = item.CompleteTime,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            return RiskControl;
        }
        public List<RiskLibrarySolutionItem> GetSolution(ModelFilter filter, string listIdSolutions)
        {
            var dbo = RiskControlDA.Repository;
            var solutionIds = listIdSolutions.Split(',').Select(i => int.Parse(i));
            return dbo.RiskLibrarySolutions.Where(i => solutionIds.Contains(i.ID))
                    .Select(item => new RiskLibrarySolutionItem()
                                {
                                    ID = item.ID,
                                    Description = item.Description,
                                    Content = item.Content
                                }
                            ).ToList();
        }
        public List<RiskControlItem> GetForAudit(ModelFilter filter, int RiskID)
        {
            var dbo = RiskControlDA.Repository;
            var RiskControl = RiskControlDA.GetQuery(i => i.RiskID == RiskID && i.IsAccept && i.IsApproval)
                        .Select(item => new RiskControlItem()
                        {
                            ID = item.ID,
                            Controls = item.Controls,
                            LikeLiHood = item.LikeLiHood,
                            RiskID = item.RiskID,
                            RiskTreatmentID = item.RiskTreatmentID,
                            IsEdit = item.IsEdit,
                            IsAccept = item.IsAccept,
                            IsApproval = item.IsApproval,
                            ApprovalAt = item.ApprovalAt,
                            ApprovalBy = item.ApprovalBy,
                            ApprovalNote = item.Note,
                            CompleteTime = item.CompleteTime,
                            CreateAt = item.CreateAt,
                            Impact = item.Risk.Impact,
                            Consequence = item.Risk.Consequence,
                            RiskAuditID = dbo.RiskAudits.FirstOrDefault(t => t.RiskControlID == item.ID) != null ? dbo.RiskAudits.FirstOrDefault(t => t.RiskControlID == item.ID).ID : 0,
                            IsAcceptAudit = dbo.RiskAudits.FirstOrDefault(t => t.RiskControlID == item.ID) != null ? dbo.RiskAudits.FirstOrDefault(t => t.RiskControlID == item.ID).IsAccept : false,
                            QualityNCID = dbo.RiskAudits.FirstOrDefault(t => t.RiskControlID == item.ID) != null ? dbo.RiskAudits.FirstOrDefault(t => t.RiskControlID == item.ID).QualityNCID.HasValue ? dbo.RiskAudits.FirstOrDefault(t => t.RiskControlID == item.ID).QualityNCID.Value : 0 : 0,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            return RiskControl;
        }
        public RiskControlItem GetById(int Id)
        {
            var dbo = RiskControlDA.Repository;
            var RiskControl = RiskControlDA.GetQuery(i => i.ID == Id)
                        .Select(item => new RiskControlItem()
                        {
                            ID = item.ID,
                            Controls = item.Controls,
                            NowLikeLiHood = item.NowLikeLiHood,
                            NowImpact = item.Risk.Impact,
                            NowConsequence = item.Risk.Consequence,
                            NowRiskValue = item.NowLikeLiHood * item.Risk.Impact * item.Risk.Consequence,
                            NowRiskLevelID = dbo.RiskLevels
                                    .Where(i => (i.PointMin <= item.NowLikeLiHood * item.Risk.Impact * item.Risk.Consequence) && (i.PointMax >= item.NowLikeLiHood * item.Risk.Impact * item.Risk.Consequence))
                                    .Select(i => i.ID).FirstOrDefault(),
                            LikeLiHood = item.LikeLiHood,
                            Impact = item.Risk.Impact,
                            Consequence = item.Risk.Consequence,
                            RiskValue = item.LikeLiHood * item.Risk.Impact * item.Risk.Consequence,
                            RiskLevelID = dbo.RiskLevels
                                    .Where(i => (i.PointMin <= item.LikeLiHood * item.Risk.Impact * item.Risk.Consequence) && (i.PointMax >= item.LikeLiHood * item.Risk.Impact * item.Risk.Consequence))
                                    .Select(i => i.ID).FirstOrDefault(),
                            RiskID = item.RiskID,
                            RiskTreatmentID = item.RiskTreatmentID,
                            RiskCategoryID = item.Risk.RiskCategoryID,
                            IsEdit = item.IsEdit,
                            IsAccept = item.IsAccept,
                            IsApproval = item.IsApproval,
                            ApprovalAt = item.ApprovalAt,
                            ApprovalBy = item.ApprovalBy,
                            Approval = new HumanEmployeeViewItem()
                            {
                                ID = item.ApprovalBy.HasValue ? item.ApprovalBy.Value : 0,
                                Name = dbo.HumanEmployees.Where(m => m.ID == item.ApprovalBy).Select(i => i.Name).FirstOrDefault(),
                                Role = dbo.HumanOrganizations.Where(m => m.HumanEmployeeID == item.ApprovalBy).Select(i => i.HumanRole.Name).FirstOrDefault(),
                                Department = dbo.HumanOrganizations.Where(m => m.HumanEmployeeID == item.ApprovalBy)
                                        .Select(i => i.HumanRole.HumanDepartment.Name).FirstOrDefault()
                            },
                            ApprovalNote = item.Note,
                            CompleteTime = item.CompleteTime,
                            CreateAt = item.CreateAt
                        })
                        .First();
            var solutions = dbo.RiskControlSolutions.Where(i => i.RiskControlID == Id).ToList();
            if (solutions.Count > 0)
            {
                RiskControl.ListSolutionIds = solutions.Select(i => i.RiskLibrarySolutionID.ToString()).Aggregate((curent, next) => curent + ',' + next);
            }
            return RiskControl;
        }
        public void Update(RiskControlItem item, int userID)
        {
            var dbo = RiskControlDA.Repository;
            var RiskControl = dbo.RiskControls.FirstOrDefault(i => i.ID == item.ID);
            RiskControl.LikeLiHood = item.LikeLiHood;
            RiskControl.Controls = item.Controls;
            RiskControl.RiskID = item.RiskID;
            RiskControl.IsEdit = item.IsEdit;
            RiskControl.IsAccept = item.IsAccept;
            RiskControl.IsApproval = item.IsApproval;
            RiskControl.ApprovalAt = item.ApprovalAt;
            RiskControl.ApprovalBy = item.ApprovalBy;
            RiskControl.Note = item.ApprovalNote;
            RiskControl.CompleteTime = item.CompleteTime;
            RiskControl.UpdateAt = DateTime.Now;
            RiskControl.UpdateBy = userID;
            if (!string.IsNullOrEmpty(item.ListSolutionIds))
            {
                var ids = item.ListSolutionIds.Split(',').Select(i => int.Parse(i));
                var oldItems = dbo.RiskControlSolutions.Where(i => i.RiskControlID == item.ID);
                var deleteSolution = oldItems.Where(i => !ids.Contains(i.ID));
                dbo.RiskControlSolutions.RemoveRange(deleteSolution);
                var insertSolution =
                        ids.Where(id => !oldItems.Select(i => i.ID).Contains(id))
                        .Select(solutionId => new RiskControlSolution()
                        {
                            RiskControlID = RiskControl.ID,
                            RiskLibrarySolutionID = solutionId
                        });
                dbo.RiskControlSolutions.AddRange(insertSolution);
            }
            else
            {
                dbo.RiskControlSolutions.RemoveRange(dbo.RiskControlSolutions.Where(i => i.RiskControlID == item.ID));
            }
            dbo.SaveChanges();
        }
        public void Insert(RiskControlItem item, int userID)
        {
            var RiskControl = new RiskControl()
            {
                Controls = item.Controls,
                LikeLiHood = item.LikeLiHood,
                RiskID = item.RiskID,
                IsEdit = item.IsEdit,
                IsAccept = item.IsAccept,
                IsApproval = item.IsApproval,
                ApprovalAt = item.ApprovalAt,
                ApprovalBy = item.ApprovalBy,
                Note = item.ApprovalNote,
                CompleteTime = item.CompleteTime,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            RiskControlDA.Insert(RiskControl);
            if (!string.IsNullOrEmpty(item.ListSolutionIds))
            {
                var riskSolutions = item.ListSolutionIds.Split(',').Select(i => int.Parse(i))
                        .Select(solutionId => new RiskControlSolution()
                        {
                            RiskControlID = RiskControl.ID,
                            RiskLibrarySolutionID = solutionId
                        });
                RiskControlDA.Repository.RiskControlSolutions.AddRange(riskSolutions);
            };
            RiskControlDA.Save();
        }
        public void InsertRiskValues(RiskControlItem item, int userID)
        {
            var riskControl = new RiskControl()
            {
                RiskID = item.RiskID,
                IsEdit = true,
                NowImpact = item.NowImpact,
                NowLikeLiHood = item.NowLikeLiHood,
                NowConsequence = item.NowConsequence,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            RiskControlDA.Insert(riskControl);
            RiskControlDA.Save();
        }
        public void Approve(RiskControlItem item, int userID)
        {
            var RiskControl = RiskControlDA.GetById(item.ID);
            RiskControl.IsEdit = item.IsEdit;
            RiskControl.IsAccept = item.IsAccept;
            RiskControl.IsApproval = item.IsApproval;
            RiskControl.ApprovalAt = item.ApprovalAt;
            RiskControl.ApprovalBy = item.ApprovalBy;
            RiskControl.Note = item.ApprovalNote;
            RiskControl.CompleteTime = item.CompleteTime;
            RiskControl.UpdateAt = DateTime.Now;
            RiskControl.UpdateBy = userID;
            RiskControlDA.Save();
        }
        public void Delete(int id)
        {
            RiskControlDA.Delete(id);
            RiskControlDA.Save();
        }
    }
}
