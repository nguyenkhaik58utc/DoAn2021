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
    public class StatisticalPlanSV
    {
        iDASBusinessEntities dbo = new QualityPlanDA().Repository;
        public List<StatisticalQualityPlanItem> GetStatistical(int departmetmentId, DateTime fromDate, DateTime toDate)
        {
            var cates = dbo.HumanDepartments
                        .Where(i => i.IsActive && !i.IsDelete && !i.IsCancel && !i.IsMerge && i.ParentID == departmetmentId)
                        .Where(i => !i.IsDestroy)
                        .OrderBy(i => i.Position)
                         .Select(item => new
                         {
                             ID = item.ID,
                             Name = item.Name,
                             IsLeaf = !dbo.HumanDepartments.Any(i => i.ParentID == item.ID && i.IsActive && !i.IsDelete && !i.IsCancel && !i.IsMerge && !i.IsDestroy),
                         })
                         .ToList();
            var result = new List<StatisticalQualityPlanItem>();
            foreach (var cate in cates)
            {
                var item = getByCateId(cate.ID, fromDate, toDate);
                item.DepartmentID = cate.ID;
                item.Name = cate.Name;
                item.IsLeaf = cate.IsLeaf;
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
        public StatisticalQualityPlanItem getByCateId(int departmentId, DateTime fromDate, DateTime toDate, bool isLoadAll = false)
        {
            var PlanTotal = dbo.QualityPlans.Where(item => item.DepartmentID == departmentId && item.StartTime <= toDate
                                                 && (item.EndAt == null || item.EndAt >= fromDate));
            var PlanEndTotal = PlanTotal.Where(item => item.IsEnd);
            var statistical = new StatisticalQualityPlanItem();
            statistical.Total = PlanTotal.Count();
            statistical.New = PlanTotal.Count(item => item.IsEdit && !item.IsApproval);
            statistical.Edit = PlanTotal.Count(item => item.IsEdit && item.IsApproval);
            statistical.Wait = PlanTotal.Count(item => !item.IsEdit && !item.IsApproval);
            statistical.NotApproval = PlanTotal.Count(item => !item.IsEdit && item.IsApproval && !item.IsAccept);
            statistical.Performing = PlanTotal.Count(item => item.IsAccept && !item.IsEnd);
            statistical.EndTotal = PlanEndTotal.Count();
            if (isLoadAll)
            {
                if (statistical.EndTotal != 0)
                {
                    var PlanCompleteTotal = GetPlanComplete(PlanEndTotal);
                    //Hoàn thành
                    statistical.Complete = PlanCompleteTotal.Count();
                    statistical.Ok = GetPlanOK(PlanCompleteTotal).Count();
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
        private IEnumerable<QualityPlan> GetPlanComplete(IEnumerable<QualityPlan> PlanSources)
        {
            //Mục tiêu con
            var Plans = PlanSources.Where(i => !GetAllTaskByPlan(dbo.QualityPlans.Where(p => p.ID == i.ID)).Any(t => !t.IsComplete));
            //Số công việc thành công
            return Plans;
        }
        private IEnumerable<QualityPlan> GetPlanOK(IEnumerable<QualityPlan> PlanSources)
        {
            return PlanSources.Where(i => !GetAllTaskByPlan(dbo.QualityPlans.Where(p => p.ID == i.ID)).Any(t => t.AuditID.HasValue && !t.Audit.IsPass));
        }
        private IEnumerable<iDAS.Base.Task> GetAllTaskByPlan(IEnumerable<QualityPlan> planSources)
        {
            var planAndChilds = GetPlanRootChildsByPlans(planSources).Distinct();
            var taskOfPlan = planAndChilds.SelectMany(item => item.QualityPlanTasks).Select(item => item.Task).Distinct();
            var tasks = GetTaskRootChildsByTasks(taskOfPlan).Distinct();
            return tasks;
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
