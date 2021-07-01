using iDAS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
namespace iDAS.Services
{
    public class QualityMeetingTaskSV
    {
        private QualityMeetingTaskDA MeetingTaskDA = new QualityMeetingTaskDA();
        private TaskSV taskSV = new TaskSV();
        public List<TaskItem> GetTreeTask(int taskId, int? problemID)
        {
            var dbo = MeetingTaskDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = dbo.QualityMeetingTasks
                .Where(i => i.QualityMeetingProblemID == problemID)
                .Join(dbo.Tasks, c => c.TaskID, t => t.ID, (c, t) => new TaskItem()
                {
                    ID = t.ID,
                    Name = t.Name,
                    ParentID = t.ParentID,
                    StartTime = t.StartTime,
                    EndTime = t.EndTime,
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
                result = dbo.QualityMeetingTasks
               .Where(i => i.QualityMeetingProblemID == problemID)
               .Join(dbo.Tasks, c => c.TaskID, t => t.ID, (c, t) => new TaskItem()
               {
                   ID = t.ID,
                   Name = t.Name,
                   ParentID = t.ParentID,
                   StartTime = t.StartTime,
                   EndTime = t.EndTime,
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
        public List<TaskItem> GetAll(ModelFilter filter,int problemID)
        {
            var dbo = MeetingTaskDA.Repository;
            var tasks = dbo.QualityMeetingTasks
                .Where(i => i.QualityMeetingProblemID == problemID)
                .Join(dbo.Tasks, c => c.TaskID, t => t.ID, (c, t) => new TaskItem()
                {
                    ID = t.ID,
                   // DepartmentID = t.DepartmentID,
                    Name = t.Name,
                   // CategoryID = t.CategoryID,
                    StartTime = t.StartTime,
                    EndTime = t.EndTime,
                   // LevelID = t.LevelID,
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
                   // PerformBy = t.PerformBy,
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

        public void Insert(TaskItem taskItem, int problemID,int employeeID)
        {
            var taskID = taskSV.Insert(taskItem,(int)taskItem.CreateBy,employeeID);
            var task = new QualityMeetingTask()
            {
                TaskID = taskID,
                 QualityMeetingProblemID = problemID,
                CreateBy = taskItem.CreateBy,
                CreateAt = DateTime.Now,
            };
            MeetingTaskDA.Insert(task);
            MeetingTaskDA.Save();
        }
        public int InsertMeetingTask(TaskItem taskItem, int problemID, int employeeID)
        {
            var taskID = taskSV.Insert(taskItem, (int)taskItem.CreateBy, employeeID);
            var task = new QualityMeetingTask()
            {
                TaskID = taskID,
                QualityMeetingProblemID = problemID,
                CreateBy = taskItem.CreateBy,
                CreateAt = DateTime.Now,
            };
            MeetingTaskDA.Insert(task);
            MeetingTaskDA.Save();
            return taskID;
        }
    }
}
