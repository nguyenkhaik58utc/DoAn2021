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
    public class TaskPerformSV
    {
        private TaskPersonalSV personService = new TaskPersonalSV();
        private TaskSV tasksService = new TaskSV();
        private TaskPerformDA performsDA = new TaskPerformDA();
        private TaskDA tasksDA = new TaskDA();
        public List<TaskPerformItem> GetByTaskId(int page, int pageSize, out int totalCount, int taskId)
        {
            var dbo = performsDA.Repository;
            var checks = performsDA.GetQuery(t => t.TaskID == taskId)
                        .Select(item => new TaskPerformItem()
                        {
                            ID = item.ID,
                            PerformBy = item.EmployeeID,
                            Content = item.Content,
                            Time = item.Time,
                            TaskID = item.TaskID,
                            Feedback = item.Feedback,
                            Rate = item.Rate,
                            IsCheck = item.IsCheck,
                            UpdateAt = item.UpdateAt,
                            CreateAt = item.CreateAt,
                            UpdateBy = item.UpdateBy,
                            PerformByName = item.EmployeeID != null ? dbo.HumanEmployees.FirstOrDefault(t => t.ID == item.EmployeeID).Name : string.Empty,
                            AttachmentFileIDs = item.TaskPerformAttachmentFiles.Select(t=>t.AttachmentFileID).ToList()

                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return checks;
        }
        public List<TaskPerformItem> GetByTaskIdUnCheck(int page, int pageSize, out int totalCount, int taskId)
        {
            var dbo = performsDA.Repository;
            var checks = performsDA.GetQuery(t => t.TaskID == taskId && !t.IsCheck)
                        .Select(item => new TaskPerformItem()
                        {
                            ID = item.ID,
                            PerformBy = item.EmployeeID,
                            Content = item.Content,
                            Time = item.Time,
                            TaskID = item.TaskID,
                            Feedback = item.Feedback,
                            Rate = item.Rate,
                            UpdateAt = item.UpdateAt,
                            CreateAt = item.CreateAt,
                            UpdateBy = item.UpdateBy,
                            PerformByName = item.EmployeeID != null ? dbo.HumanEmployees.FirstOrDefault(t => t.ID == item.EmployeeID).Name : string.Empty,

                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return checks;
        }
    
        public void Insert(TaskPerformItem obj, int userId, int employeeId)
        {
            var dbo = performsDA.Repository;
            var perform = new TaskPerform();
            perform.CreateAt = DateTime.Now;
            perform.CreateBy = userId;
            perform.Content = obj.Content.Trim();
            perform.EmployeeID = (int)obj.PerformBy;
            perform.Rate = obj.Rate;
            perform.Feedback = obj.Feedback;
            perform.TaskID = obj.TaskID;
            perform.Time = obj.Time;
            performsDA.Insert(perform);
            personService.UpdateIsNew(dbo, obj.TaskID, employeeId);          
            tasksService.UpdateRateTask(dbo, obj.TaskID, obj.Rate);  
            performsDA.Save();
            new FileSV().Upload<TaskPerformAttachmentFile>(obj.AttachmentFiles, perform.ID);
            //var Ids = new FileSV().InsertRange(obj.AttachmentFiles.FileAttachments, userId);
            //new TaskPerformAttachmentFileSV().InsertRange(Ids, perform.ID, userId);
           
        }
        public TaskPerformItem GetByID(int id, int idPerform)
        {
            var dbo = tasksDA.Repository;
            var employeeSV = new HumanEmployeeSV();
            var item = tasksDA.GetById(id);
            var itemperform = performsDA.GetById(idPerform);
            var perform = new TaskPerformItem();
            perform.ID = item.ID;
            perform.PerformBy = item.HumanEmployeeID;
            perform.Feedback = itemperform.Feedback;
            perform.Perform = employeeSV.GetEmployeeView(item.HumanEmployeeID);          
            perform.Content = itemperform.Content;
            perform.Rate = itemperform.Rate;
            perform.Time = itemperform.Time;
            perform.CreateByName = item.CreateBy != null ? dbo.HumanUsers.FirstOrDefault(u => u.ID == item.CreateBy).HumanEmployee.Name : null;
            perform.UpdateByName = item.UpdateBy.HasValue ? dbo.HumanUsers.FirstOrDefault(u => u.ID == item.UpdateBy).HumanEmployee.Name : null;
            perform.AttachmentFiles = new AttachmentFileItem()
            {
                Files = dbo.TaskPerformAttachmentFiles.Where(i => i.TaskPerformID == idPerform)
                    .Select(i => i.AttachmentFileID).ToList()
            };
            return perform;
        }
    }
}
