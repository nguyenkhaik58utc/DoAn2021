using iDAS.Services;
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
   public class QualityNCTaskSV
    {
       private QualityNCTaskDA ncTaskDA = new QualityNCTaskDA();
        private TaskSV taskSV = new TaskSV();
        public List<TaskItem> GetTreeTask(int taskId, int incorrectID)
        {
            var dbo = ncTaskDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = dbo.QualityNCTasks
                .Where(i => i.QualityNCID == incorrectID)
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
                    CategoryName = t.TaskCategory.Name,
                    LevelColor = t.TaskLevel.Color,
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
                .OrderByDescending(item => item.CreateAt)
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
                result = taskremove.OrderByDescending(t => t.CreateAt.Value).ToList();
            }
            else
            {
                result = dbo.QualityNCTasks
                .Where(i => i.QualityNCID == incorrectID)
                .Join(dbo.Tasks, c => c.TaskID, t => t.ID, (c, t) => new TaskItem()
                {
                    ID = t.ID,
                    DepartmentID = t.HumanDepartmentID,
                    Name = t.Name,
                    CategoryID = t.TaskCategoryID,
                    StartTime = t.StartTime,
                    EndTime = t.EndTime,
                    ParentID = t.ParentID,
                    LevelID = t.TaskLevelID,
                    LevelName = t.TaskLevel.Name,
                    CategoryName = t.TaskCategory.Name,
                    LevelColor = t.TaskLevel.Color,
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
               .Where(i => i.ParentID == taskId)
               .OrderByDescending(item => item.CreateAt)
               .Distinct()
               .ToList();
            }
            return result;
        }
        public List<TaskItem> GetAll(ModelFilter filter, int incorrectID)
        {
            var dbo = ncTaskDA.Repository;
            var tasks = dbo.QualityNCTasks
                .Where(i => i.QualityNCID == incorrectID)
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
                    CategoryName = t.TaskCategory.Name,
                    LevelColor = t.TaskLevel.Color,
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

        public void Insert(TaskItem taskItem, int ncID, int userId, int employeeId)
        {
            var taskID = taskSV.Insert(taskItem, userId, employeeId);
            var task = new QualityNCTask()
            {
                TaskID = taskID,
                QualityNCID = ncID,
                CreateBy = taskItem.CreateBy,
                CreateAt = DateTime.Now,
            };
            ncTaskDA.Insert(task);
            ncTaskDA.Save();
        }
        public int InsertNCTask(TaskItem taskItem, int ncID, int userId, int employeeId)
        {
            var taskID = taskSV.Insert(taskItem, userId, employeeId);
            var task = new QualityNCTask()
            {
                TaskID = taskID,
                QualityNCID = ncID,
                CreateBy = taskItem.CreateBy,
                CreateAt = DateTime.Now,
            };
            ncTaskDA.Insert(task);
            ncTaskDA.Save();
            return taskID;
        }
    }
}
