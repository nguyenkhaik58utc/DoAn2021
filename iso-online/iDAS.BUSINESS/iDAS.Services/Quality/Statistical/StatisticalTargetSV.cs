using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class StatisticalTargetSV
    {
        iDASBusinessEntities dbo = new QualityTargetDA().Repository;
        public List<StatisticalQualityTargetItem> GetStatistical(ModelFilter filter, DateTime fromDate, DateTime toDate)
        {
            var cates = dbo.QualityTargetCategories.Where(i => i.IsActive)
                          .Filter(filter)
                         .Select(item => new
                         {
                             ID = item.ID,
                             Name = item.Name,
                             CreateAt = item.CreateAt
                         })
                        .ToList();
            var result = new List<StatisticalQualityTargetItem>();
            foreach (var cate in cates)
            {
                var item = getByCateId(cate.ID, fromDate, toDate);
                item.CategoryID = cate.ID;
                item.CategoryName = cate.Name;
                result.Add(item);
            }
            return result;
        }
        public List<ChartModel> GetCharPie(int id, DateTime fromDate, DateTime toDate)
        {
            List<ChartModel> data = new List<ChartModel>();
            var obj = getByCateId(id, fromDate, toDate, isLoadAll: true);
            data.Add(new ChartModel()
            {
                Name = "Mới(" + (obj.Total == 0 ? (double)0 : Math.Round((double)obj.New * 100 / obj.Total)) + "%)",
                Data1 = (double)obj.New
            });
            data.Add(new ChartModel()
            {
                Name = "Sửa đổi(" + (obj.Total == 0 ? (double)0 : Math.Round((double)obj.Edit * 100 / obj.Total)) + "%)",
                Data1 = (double)obj.Edit
            });
            data.Add(new ChartModel()
            {
                Name = "Chờ duyệt(" + (obj.Total == 0 ? (double)0 : Math.Round((double)obj.Wait * 100 / obj.Total)) + "%)",
                Data1 = (double)obj.Wait
            });
            data.Add(new ChartModel()
            {
                Name = "Không duyệt(" + (obj.Total == 0 ? (double)0 : Math.Round((double)obj.NotApproval * 100 / obj.Total)) + "%)",
                Data1 = (double)obj.NotApproval
            });
            data.Add(new ChartModel()
            {
                Name = "Đang thực hiện(" + (obj.Total == 0 ? (double)0 : Math.Round((double)obj.Performing * 100 / obj.Total)) + "%)",
                Data1 = (double)obj.Performing
            });
            data.Add(new ChartModel()
            {
                Name = "Chưa hoàn thành(" + (obj.Total == 0 ? (double)0 : Math.Round((double)obj.NotComplete * 100 / obj.Total)) + "%)",
                Data1 = (double)obj.NotComplete
            });
            data.Add(new ChartModel()
            {
                Name = "Đạt(" + (obj.Total == 0 ? (double)0 : Math.Round((double)obj.Ok * 100 / obj.Total)) + "%)",
                Data1 = (double)obj.Ok
            });
            data.Add(new ChartModel()
            {
                Name = "Không đạt(" + (obj.Total == 0 ? (double)0 : Math.Round((double)obj.NotOk * 100 / obj.Total)) + "%)",
                Data1 = (double)obj.NotOk
            });
            return data;
        }
        public StatisticalQualityTargetItem getByCateId(int cateId, DateTime fromDate, DateTime toDate, bool isLoadAll = false)
        {
            var targetTotal = dbo.QualityTargets.Where(item => item.QualityTargetCategoryID == cateId && item.CreateAt <= toDate
                                                 && (item.EndAt == null || item.EndAt >= fromDate));
            var targetEndTotal = targetTotal.Where(item => item.IsEnd);

            var statistical = new StatisticalQualityTargetItem();
            statistical.Total = targetTotal.Count();
            statistical.New = targetTotal.Count(item => item.IsEdit && !item.IsApproval);
            statistical.Edit = targetTotal.Count(item => item.IsEdit && item.IsApproval);
            statistical.Wait = targetTotal.Count(item => !item.IsEdit && !item.IsApproval);
            statistical.NotApproval = targetTotal.Count(item => !item.IsEdit && item.IsApproval && !item.IsAccept);
            statistical.Performing = targetTotal.Count(item => item.IsAccept && !item.IsEnd);
            statistical.EndTotal = targetEndTotal.Count();
            if (isLoadAll)
            {
                if (statistical.EndTotal != 0)
                {
                    var targetCompleteTotal = GetTargetComplete(targetEndTotal);
                    //Hoàn thành
                    statistical.Complete = targetCompleteTotal.Count();
                    statistical.Ok = GetTargetOK(targetCompleteTotal).Count();
                    statistical.NotOk = statistical.Complete - statistical.Ok;
                    //Chưa hoàn thành
                    statistical.NotComplete = statistical.EndTotal - statistical.Complete;
                }
                else
                {
                    //Hoàn thành
                    statistical.Complete = 0;
                    statistical.Ok = 0;
                    statistical.NotOk = 0;
                    //Chưa hoàn thành
                    statistical.NotComplete = 0;
                }
            }
            return statistical;
        }
        private IEnumerable<QualityTarget> GetTargetOK(IEnumerable<QualityTarget> targetSources)
        {
            return targetSources.Where(i => !GetAllTaskByTaget(dbo.QualityTargets.Where(t => t.ID == i.ID))
                                    .Any(t => t.AuditID.HasValue && !t.Audit.IsPass));
        }
        private IEnumerable<QualityTarget> GetTargetComplete(IEnumerable<QualityTarget> targetSources)
        {
            var targetComplete = targetSources.Where(i => !GetAllTaskByTaget(dbo.QualityTargets.Where(t => t.ID == i.ID))
                                .Any(t => !t.IsComplete));
            return targetComplete;
        }
        private IEnumerable<iDAS.Base.Task> GetAllTaskByTaget(IEnumerable<QualityTarget> targetSources)
        {
            var targets = GetTargetRootChildsByTargets(targetSources);
            var planOfTargets = targets.SelectMany(item => item.QualityPlans).Distinct();
            var planAndChilds = GetPlanRootChildsByPlans(planOfTargets).Distinct();
            var taskOfPlan = planAndChilds.SelectMany(item => item.QualityPlanTasks).Select(item => item.Task).Distinct();
            var tasks = GetTaskRootChildsByTasks(taskOfPlan).Distinct();
            return tasks;
        }
        private IEnumerable<QualityTarget> GetTargetRootChildsByTargets(IEnumerable<QualityTarget> targetSources)
        {
            var root = targetSources;
            var childs = GetTargetChildsByTargets(dbo.QualityTargets, root.Select(i => i.ID).ToList());
            return root.Concat(childs);
        }
        private IEnumerable<QualityTarget> GetTargetChildsByTargets(IEnumerable<QualityTarget> e, IEnumerable<int> ids)
        {
            var childs = e.Where(a => a.ParentID != null && ids.Contains((int)a.ParentID));
            return childs.SelectMany(c => GetTargetChildsByTargets(e, childs.Select(i => i.ID)));
        }
        private IEnumerable<QualityPlan> GetPlanRootChildsByPlans(IEnumerable<QualityPlan> planSources)
        {
            var root = planSources;
            var childs = GetPlanChildsByPlans(dbo.QualityPlans, root.Select(i => i.ID).ToList());
            return root.Concat(childs);
        }
        private IEnumerable<QualityPlan> GetPlanChildsByPlans(IEnumerable<QualityPlan> e, IEnumerable<int> ids)
        {
            var childs = e.Where(a => a.ParentID != null && ids.Contains((int)a.ParentID));
            return childs.SelectMany(c => GetPlanChildsByPlans(e, childs.Select(i => i.ID)));
        }
        private IEnumerable<iDAS.Base.Task> GetTaskRootChildsByTasks(IEnumerable<iDAS.Base.Task> taskSource)
        {
            var root = taskSource;
            var childs = GetTaskChildsByTasks(dbo.Tasks, root.Select(i => i.ID).ToList());
            return root.Concat(childs);
        }
        private IEnumerable<iDAS.Base.Task> GetTaskChildsByTasks(IEnumerable<iDAS.Base.Task> e, IEnumerable<int> ids)
        {
            var childs = e.Where(c => c.ParentID != 0 && ids.Contains(c.ParentID));
            return childs.SelectMany(c => GetTaskChildsByTasks(e, childs.Select(i => i.ID)));
        }
    }
}
