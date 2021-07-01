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
    public class QualityCAPATaskSV
    {
        private QualityCAPATaskDA CAPATaskDA = new QualityCAPATaskDA();
        private TaskSV taskSV = new TaskSV();
        public List<TaskItem> GetTreeTask(int taskId, int CAPAID)
        {
            var dbo = CAPATaskDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = dbo.QualityCAPATasks
                .Where(i => i.QualityCAPAID == CAPAID)
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
                    CategoryName = t.TaskCategory.Name,
                    LevelName = t.TaskLevel.Name,
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
                result = dbo.QualityCAPATasks
                .Where(i => i.QualityCAPAID == CAPAID)
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
                    CategoryName = t.TaskCategory.Name,
                    LevelName = t.TaskLevel.Name,
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
        public List<TaskItem> GetAll(ModelFilter filter, int CAPAID)
        {
            var dbo = CAPATaskDA.Repository;
            var tasks = dbo.QualityCAPATasks
                .Where(i => i.QualityCAPAID == CAPAID)
                .Join(dbo.Tasks, c => c.TaskID, t => t.ID, (c, t) => new TaskItem()
                {
                    ID = t.ID,
                    DepartmentID = t.HumanDepartmentID,
                    Name = t.Name,
                    CategoryID = t.TaskCategoryID,
                    StartTime = t.StartTime,
                    EndTime = t.EndTime,
                    LevelID = t.TaskLevelID,
                    CategoryName = t.TaskCategory.Name,
                    LevelName = t.TaskLevel.Name,
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

        public void Insert(TaskItem taskItem, int CAPAID, int userId, int employeeId)
        {
            var taskID = taskSV.Insert(taskItem, userId, employeeId);
            var task = new QualityCAPATask()
            {
                TaskID = taskID,
                QualityCAPAID = CAPAID,
                CreateBy = taskItem.CreateBy,
                CreateAt = DateTime.Now,
            };
            CAPATaskDA.Insert(task);
            CAPATaskDA.Save();
        }
        public int InsertCAPATask(TaskItem taskItem, int CAPAID, int userId, int employeeId)
        {
            var taskID = taskSV.Insert(taskItem, userId, employeeId);
            var task = new QualityCAPATask()
            {
                TaskID = taskID,
                QualityCAPAID = CAPAID,
                CreateBy = taskItem.CreateBy,
                CreateAt = DateTime.Now,
            };
            CAPATaskDA.Insert(task);
            CAPATaskDA.Save();
            return taskID;
        }
    }
}
