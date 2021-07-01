using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;

namespace iDAS.Services
{
    public class HumanAuditTickResultSV
    {
        private HumanAuditTickResultDA HumanAuditTickResultDA = new HumanAuditTickResultDA();
        private HumanAuditTickResultBonusScoreDA bnScoreDA = new HumanAuditTickResultBonusScoreDA();
        public void InsertEmployeeAudits(List<HumanAuditTickResultItem> tickResultItems, int auditTickId, int userId)
        {
            var dbo = HumanAuditTickResultDA.Repository;
            var lstEmployeeResult = tickResultItems.Select(r => r.AuditEmployeeResult).ToList();
            var insertItems = tickResultItems.Select(
                item => new HumanAuditTickResult()
                {
                    HumanAuditTickID = item.HumanAuditTickID,
                    AuditEmployeeResult = item.AuditEmployeeResult,
                    CreateBy = userId,
                    CreateAt = DateTime.Now
                }
                ).ToList();
            dbo.HumanAuditTickResults.AddRange(insertItems);
            var tick = dbo.HumanAuditTicks.Where(i => i.ID == auditTickId).FirstOrDefault();
            tick.IsEmployeeAuditted = true;
            tick.EmployeeAuditResult = dbo.HumanAuditGradationCriteriaPoints
                    .Where(i => lstEmployeeResult.Contains(i.ID))
                    .Sum(i => i.Point * i.HumanAuditGradationCriteria.Factory
                        * i.HumanAuditGradationCriteria.HumanAuditGradationRole.Factory);
            dbo.SaveChanges();
        }
        public List<HumanAuditTickResultCriteriaItem> GetAnswerDetail(int auditTickId)
        {
            var dbo = HumanAuditTickResultDA.Repository;
            var gradationRoleID = dbo.HumanAuditTicks.Where(i => i.ID == auditTickId)
                    .Select(i => i.HumanAuditGradationRoleID)
                    .FirstOrDefault();
            var roleID = dbo.HumanAuditTicks.Where(i => i.ID == auditTickId)
                            .Select(i => i.HumanAuditGradationRole.HumanRoleID).FirstOrDefault();
            var audits = dbo.HumanAuditGradationCriteriaPoints
                 .Where(i => i.HumanAuditGradationCriteria.HumanAuditGradationRoleID == gradationRoleID)
                 .Select(item => new HumanAuditTickResultCriteriaItem()
                 {
                     Criteria = item.HumanAuditGradationCriteria.Name,
                     CriteriaPoint = item.Name,
                     Point = item.Point,
                     Factory = item.HumanAuditGradationCriteria.HumanAuditGradationRole.Factory
                               * item.HumanAuditGradationCriteria.Factory,
                     IsEmployeeSelect = dbo.HumanAuditTickResults
                        .Any(i => i.AuditEmployeeResult == item.ID && i.HumanAuditTickID == auditTickId),
                     IsManagementSelect = dbo.HumanAuditTickResults
                        .Any(i => i.AuditManagementResult == item.ID && i.HumanAuditTickID == auditTickId),
                     IsLeaderSelect = dbo.HumanAuditTickResults
                        .Any(i => i.AuditLeaderResult == item.ID && i.HumanAuditTickID == auditTickId)
                 })
                .OrderByDescending(i => i.Point)
                 .OrderBy(i => i.Criteria)
                 .ToList();
            var results = new List<HumanAuditTickResultCriteriaItem>();
            var index = 0;
            var criteriaCompare = "";
            foreach (var audit in audits)
            {

                if (criteriaCompare != audit.Criteria)
                {
                    index++;
                    criteriaCompare = audit.Criteria;
                }
                results.Add(
                    new HumanAuditTickResultCriteriaItem()
                    {
                        Criteria = index.ToString() + ". " + audit.Criteria,
                        CriteriaPoint = audit.CriteriaPoint,
                        Point = audit.Point,
                        Factory = audit.Factory,
                        IsEmployeeSelect = audit.IsEmployeeSelect,
                        IsManagementSelect = audit.IsManagementSelect,
                        IsLeaderSelect = audit.IsLeaderSelect
                    }
                    );
            }
            return results;
        }
        public List<HumanAuditTickResultCriteriaItem> GetAnswer(int auditTickId)
        {
            var dbo = HumanAuditTickResultDA.Repository;
            var gradationRoleID = dbo.HumanAuditTicks.Where(i => i.ID == auditTickId)
                 .Select(i => i.HumanAuditGradationRoleID)
                 .FirstOrDefault();
            var roleID = dbo.HumanAuditTicks.Where(i => i.ID == auditTickId)
                            .Select(i => i.HumanAuditGradationRole.HumanRoleID).FirstOrDefault();
            var audits = dbo.HumanAuditGradationCriteriaPoints
                 .Where(i => i.HumanAuditGradationCriteria.HumanAuditGradationRoleID == gradationRoleID)
                 .Select(item => new HumanAuditTickResultCriteriaItem()
                 {
                     Criteria = item.HumanAuditGradationCriteria.Name,
                     CriteriaPoint = item.Name,
                     CriteriaPointID = item.ID,
                     Point = item.Point,
                     Factory = item.HumanAuditGradationCriteria.HumanAuditGradationRole.Factory
                               * item.HumanAuditGradationCriteria.Factory,
                     IsEmployeeSelect = dbo.HumanAuditTickResults
                        .Any(i => i.AuditEmployeeResult == item.ID && i.HumanAuditTickID == auditTickId),
                     IsManagementSelect = dbo.HumanAuditTickResults
                        .Any(i => i.AuditManagementResult == item.ID && i.HumanAuditTickID == auditTickId),
                     IsLeaderSelect = dbo.HumanAuditTickResults
                        .Any(i => i.AuditLeaderResult == item.ID && i.HumanAuditTickID == auditTickId)
                 })
                 .OrderBy(i => i.Criteria)
                 .ToList();
            var results = new List<HumanAuditTickResultCriteriaItem>();
            var index = 0;
            var criteriaCompare = "";
            foreach (var audit in audits)
            {

                if (criteriaCompare != audit.Criteria)
                {
                    index++;
                    criteriaCompare = audit.Criteria;
                }
                results.Add(
                    new HumanAuditTickResultCriteriaItem()
                    {
                        ID = audit.ID,
                        CriteriaID = audit.CriteriaID,
                        Criteria = index.ToString() + ". " + audit.Criteria,
                        CriteriaPoint = audit.CriteriaPoint,
                        CriteriaPointID = audit.CriteriaPointID,
                        Point = audit.Point,
                        Factory = audit.Factory,
                        IsEmployeeSelect = audit.IsEmployeeSelect,
                        IsManagementSelect = audit.IsManagementSelect,
                        IsLeaderSelect = audit.IsLeaderSelect
                    }
                );
            }
            return results;
        }
        public void UpdateManagementResult(int id, int pointId, int userId)
        {
            var dbo = HumanAuditTickResultDA.Repository;
            var pointIds = dbo.HumanAuditGradationCriterias.Where(c => c.HumanAuditGradationCriteriaPoints.Any(p => p.ID == pointId))
                            .SelectMany(i => i.HumanAuditGradationCriteriaPoints).Select(i => i.ID);
            var result = dbo.HumanAuditTickResults.Where(i => i.HumanAuditTickID == id && pointIds.Any(p => p == i.AuditEmployeeResult || p == i.AuditManagementResult || p == i.AuditLeaderResult)).FirstOrDefault();
            if (result != null)
            {
                result.AuditManagementResult = pointId;
            }
            else
            {
                dbo.HumanAuditTickResults.Add(new HumanAuditTickResult()
                    {
                        HumanAuditTickID = id,
                        AuditManagementResult = pointId
                    });
            }
            dbo.SaveChanges();
        }
        public void UpdateLeaderResult(int id, int pointId, int userId)
        {
            var dbo = HumanAuditTickResultDA.Repository;
            var pointIds = dbo.HumanAuditGradationCriterias.Where(c => c.HumanAuditGradationCriteriaPoints.Any(p => p.ID == pointId))
                           .SelectMany(i => i.HumanAuditGradationCriteriaPoints).Select(i => i.ID);
            var result = dbo.HumanAuditTickResults.Where(i => i.HumanAuditTickID == id && pointIds.Any(p => p == i.AuditEmployeeResult || p == i.AuditManagementResult || p == i.AuditLeaderResult)).FirstOrDefault();
            if (result != null)
            {
                result.AuditLeaderResult = pointId;
            }
            else
            {
                dbo.HumanAuditTickResults.Add(new HumanAuditTickResult()
                {
                    HumanAuditTickID = id,
                    AuditLeaderResult = pointId
                });
            }
            dbo.SaveChanges();
        }

        public List<HumanAuditTickResultBonusScoreItem> GetDataPointBonus(ModelFilter filter, int TickResultId)
        {
            var rs = bnScoreDA.GetQuery(x => x.HumanAuditTickID == TickResultId).Select(item => new HumanAuditTickResultBonusScoreItem()
                {
                    ID= item.ID,
                    AuditTickResultID = item.HumanAuditTickID,
                    Point =item.Point,
                    Note = item.Note,
                    CreateAt = item.CreateAt
                }).Filter(filter).ToList();
            foreach(var i in rs)
            {
                if (i.Point > 0)
                    i.CT = "+";
                else 
                {
                    i.CT="-";
                    i.Point = Math.Abs(i.Point);
                }
            }
            return rs;
        }
        public void UpdateDataPoint(List<HumanAuditTickResultBonusScoreItem> lstItem, int rsID, int p)
        {
            var dbo = HumanAuditTickResultDA.Repository;
            var lstOld = bnScoreDA.GetQuery(x => x.HumanAuditTickID == rsID).ToList();
            if(lstOld.Count>0) bnScoreDA.DeleteRange(lstOld);
            bnScoreDA.Save();
            foreach (var item in lstItem)
            {
                var objNew = new HumanAuditTickResultBonusScore();
                objNew.HumanAuditTickID = rsID;
                if (item.CT == "+")
                    objNew.Point = item.Point;
                else
                    objNew.Point = -item.Point;
                objNew.Note = item.Note;
                objNew.CreateAt = DateTime.Now;
                objNew.CreateBy = p;
                dbo.HumanAuditTickResultBonusScores.Add(objNew);
            }
            dbo.SaveChanges();
        }
    }
}
