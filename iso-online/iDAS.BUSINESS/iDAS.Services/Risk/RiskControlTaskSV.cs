using iDAS.Base;
using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
   public class RiskControlTaskSV
    {
       private RiskControlTaskDA RiskControlTaskDA = new RiskControlTaskDA();
       private TaskSV taskSV = new TaskSV();
       public List<TaskItem> GetTreeTask(int taskId, int? RiskControlID)
       {
           var dbo = RiskControlTaskDA.Repository;
           var result = new List<TaskItem>();
           if (taskId == 0)
           {
               result = dbo.RiskControlTasks
              .Where(i => i.RiskControlID == RiskControlID)
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
               result = taskremove.OrderByDescending(t => t.CreateAt.Value).ToList();
           }
           else
           {
               result = dbo.RiskControlTasks
                .Where(i => i.RiskControlID == RiskControlID)
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
       public void Insert(TaskItem taskItem, int RiskControlID, int userId, int employeeId)
       {
           var taskID = taskSV.Insert(taskItem, userId, employeeId);
           var task = new RiskControlTask()
           {
               TaskID = taskID,
               RiskControlID = RiskControlID,
               CreateBy = taskItem.CreateBy,
               CreateAt = DateTime.Now,
           };
           RiskControlTaskDA.Insert(task);
           RiskControlTaskDA.Save();
       }
       public int InsertRiskControl(TaskItem taskItem, int RiskControlID, int userId, int employeeId)
       {
           var taskID = taskSV.Insert(taskItem, userId, employeeId);
           var task = new RiskControlTask()
           {
               TaskID = taskID,
               RiskControlID = RiskControlID,
               CreateBy = taskItem.CreateBy,
               CreateAt = DateTime.Now,
           };
           RiskControlTaskDA.Insert(task);
           RiskControlTaskDA.Save();
           return task.TaskID;
       }

    }
}
