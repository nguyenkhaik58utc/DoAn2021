using iDAS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Services;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class QualityPlanTaskSV
    {
        private QualityPlanTaskDA planTaskDA = new QualityPlanTaskDA();
        private TaskSV taskSV = new TaskSV();
        public List<TaskItem> GetTreeTask(int taskId, int? planID)
        {
            var dbo = planTaskDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = dbo.QualityPlanTasks
               .Where(i => i.QualityPlanID == planID)
               .Join(dbo.Tasks, c => c.TaskID, t => t.ID, (c, t) => new TaskItem()
               {
                   ID = t.ID,
                   DepartmentID = t.HumanDepartmentID,
                   Name = t.Name,
                   ParentID = t.ParentID,
                   CategoryID = t.TaskCategoryID,
                   StartTime = t.StartTime,
                   EndTime = t.EndTime,
                   LevelID = t.TaskLevelID,
                   LevelName = t.TaskLevel.Name,
                   LevelColor = t.TaskLevel.Color,
                   CategoryName = t.TaskCategory.Name,
                   IsNew = t.IsNew,
                   IsEdit = t.IsEdit,
                   IsComplete = t.IsComplete,
                   IsPass = t.IsPass,
                   IsPause = t.IsPause,
                   Rate = t.Rate,
                   Cost = t.Cost,
                   Content = t.Content,
                   Note = t.Note,
                   PerformBy = t.HumanEmployeeID,
                   CreateAt = t.CreateAt,
                   CreateBy = t.CreateBy,
                   UpdateAt = t.UpdateAt,
                   UpdateBy = t.UpdateBy,
                   Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == t.ID) == null
               })
                .OrderByDescending(p => p.CreateAt)
                .Distinct()
                .ToList();               
                var lis = result.Select(t => t.ID).ToArray();
                List<TaskItem> taskremove = new List<TaskItem>();
                foreach (var item in result)
                {
                    if (!lis.Contains(item.ParentID))
                    {
                        taskremove.Add(item);
                    }
                }
                result = taskremove.ToList();
            }
            else
            {
                result = dbo.QualityPlanTasks
               .Join(dbo.Tasks, c => c.TaskID, t => t.ID, (c, t) => new TaskItem()
               {
                   ID = t.ID,
                   DepartmentID = t.HumanDepartmentID,
                   Name = t.Name,
                   ParentID = t.ParentID,
                   CategoryID = t.TaskCategoryID,
                   StartTime = t.StartTime,
                   EndTime = t.EndTime,
                   LevelID = t.TaskLevelID,
                   LevelName = t.TaskLevel.Name,
                   LevelColor = t.TaskLevel.Color,
                   CategoryName = t.TaskCategory.Name,
                   IsNew = t.IsNew,
                   IsEdit = t.IsEdit,
                   IsComplete = t.IsComplete,
                   IsPass = t.IsPass,
                   IsPause = t.IsPause,
                   Rate = t.Rate,
                   Cost = t.Cost,
                   Content = t.Content,
                   Note = t.Note,
                   PerformBy = t.HumanEmployeeID,
                   CreateAt = t.CreateAt,
                   CreateBy = t.CreateBy,
                   UpdateAt = t.UpdateAt,
                   UpdateBy = t.UpdateBy,
                   Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == t.ID) == null
               })
                .OrderByDescending(p => p.CreateAt)
                .Where(i => i.ParentID == taskId)
                .Distinct()
                .ToList();     
            }
            return result;
        }
        public List<TaskItem> GetAll(ModelFilter filter, int planID)
        {
            var dbo = planTaskDA.Repository;
            var tasks = dbo.QualityPlanTasks
                .Where(i => i.QualityPlanID == planID)
                .Join(dbo.Tasks, c => c.TaskID, t => t.ID, (c, t) => new TaskItem()
                {
                    ID = t.ID,
                    DepartmentID = t.HumanDepartmentID,
                    Name = t.Name,
                    CategoryID = t.TaskCategoryID,
                    StartTime = t.StartTime,
                    EndTime = t.EndTime,
                    LevelID = t.TaskLevelID,
                    LevelName = t.TaskLevel.Name,
                    LevelColor = t.TaskLevel.Color,
                    CategoryName = t.TaskCategory.Name,
                    IsNew = t.IsNew,
                    IsEdit = t.IsEdit,
                    IsComplete = t.IsComplete,
                    IsPass = t.IsPass,
                    IsPause = t.IsPause,
                    Rate = t.Rate,
                    Cost = t.Cost,
                    Content = t.Content,
                    Note = t.Note,
                    PerformBy = t.HumanEmployeeID,
                    CreateAt = t.CreateAt,
                    CreateBy = t.CreateBy,
                    UpdateAt = t.UpdateAt,
                    UpdateBy = t.UpdateBy,
                })
                .OrderByDescending(item => item.CreateAt)
                .Filter(filter)
                .ToList();
            return tasks;
        }
        public void Insert(TaskItem taskItem, int planID, int userId, int employeeId)
        {
            var taskID = taskSV.Insert(taskItem, userId, employeeId);
            var task = new QualityPlanTask()
            {
                TaskID = taskID,
                QualityPlanID = planID,
                CreateBy = taskItem.CreateBy,
                CreateAt = DateTime.Now,
            };
            planTaskDA.Insert(task);
            planTaskDA.Save();
        }
        public int InsertPlanTask(TaskItem taskItem, int planID, int userId, int employeeId)
        {
            var taskID = taskSV.Insert(taskItem, userId, employeeId);
            var task = new QualityPlanTask()
            {
                TaskID = taskID,
                QualityPlanID = planID,
                CreateBy = taskItem.CreateBy,
                CreateAt = DateTime.Now,
            };
            planTaskDA.Insert(task);
            planTaskDA.Save();
            return taskID;
        }
        public int InsertQualityPlanTask(TaskItem taskItem, int planID, int userId, int employeeId)
        {
            var taskID = taskSV.Insert(taskItem, userId, employeeId);
            var task = new QualityPlanTask()
            {
                TaskID = taskID,
                QualityPlanID = planID,
                CreateBy = taskItem.CreateBy,
                CreateAt = DateTime.Now,
            };
            planTaskDA.Insert(task);
            planTaskDA.Save();
            return taskID;
        }
        public void Insert(int planID, int taskId, int userId, int employeeId)
        {
            var task = new QualityPlanTask()
            {
                TaskID = taskId,
                QualityPlanID = planID,
                CreateBy = userId,
                CreateAt = DateTime.Now,
            };
            planTaskDA.Insert(task);
            planTaskDA.Save();
        }



    }
}
