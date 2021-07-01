using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iDAS.Core;
using iDAS.Base;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class TaskHistorySV
    {
        private TaskHistoryDA TaskHistoryDA = new TaskHistoryDA();
        private TaskPersonalSV personService = new TaskPersonalSV();
        public Task GetTaskByID(int id)
        {
            var dbo = TaskHistoryDA.Repository;
            return dbo.Tasks.FirstOrDefault(t => t.ID == id);
        }
        public TaskHistory GetById(int id)
        {
            return TaskHistoryDA.GetById(id);
        }
        public void UpdateTaskPerson(TaskItem objNew, int employeeID, int userId)
        {
            personService.Update(objNew, employeeID, userId);
        }
        public List<TaskHistoryItem> GetAll(int page, int pageSize, out int totalCount, int taskId)
        {
            var dbo = TaskHistoryDA.Repository;
            List<TaskHistoryItem> lst = new List<TaskHistoryItem>();
            var tasks = TaskHistoryDA.GetQuery()
                        .Where(t => t.TaskID == taskId)
                        .OrderByDescending(item => item.ID)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            if (tasks.Count > 0)
            {
                foreach (var item in tasks)
                {
                    lst.Add(new TaskHistoryItem
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Content = item.Content,
                        IsComplete = item.IsComplete,
                        CreateBy = item.CreateBy,
                        Color = dbo.TaskLevels.FirstOrDefault(t=>t.ID==item.LevelID).Color,
                        StartTime = item.StartTime,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy,
                        IsChange = item.IsChange,
                        CompleteTime = item.CreateAt,
                        CreateByName = item.CreateBy != null ? dbo.HumanUsers.FirstOrDefault(u => u.ID == item.CreateBy).HumanEmployee.Name : string.Empty,
                        LevelID = item.LevelID,
                        EndTime = item.EndTime,
                        IsPause = item.IsPause,
                        IsPass = item.IsPass,
                        LevelName = dbo.TaskLevels.FirstOrDefault(t=>t.ID==item.LevelID).Name,
                        PerformBy = item.EmployeeID,
                        PerformEmployeesName = item.EmployeeID != null ? dbo.HumanEmployees.FirstOrDefault(u => u.ID == item.EmployeeID).Name : string.Empty,
                        Note = item.Note
                    });
                }
            }
            return lst;
        }
        public void UpdateChange(TaskHistoryItem item, int userID, int employeeId)
        {

            item.PerformBy = item.Perform.ID;
            item.CheckBy = item.Check.ID;
            var taskchange = TaskHistoryDA.GetById(item.ID);
            taskchange.Reason = item.Reason;
            taskchange.ParentID = item.ParentID;
            taskchange.Name = item.Name.Trim();
            taskchange.IsChange = false;
            taskchange.IsComplete = item.IsComplete;
            taskchange.IsPass = item.IsPass;
            taskchange.CategoryID = item.CategoryID;
            taskchange.Note = item.Note;
            taskchange.DepartmentID = item.Department.ID;
            taskchange.Cost = item.Cost;
            taskchange.IsPause = item.IsPause;
            taskchange.EmployeeID = item.PerformBy;
            taskchange.EndTime = item.EndTime;
            taskchange.Content = item.Content;
            taskchange.IsEdit = item.IsEdit;
            taskchange.StartTime = item.StartTime;
            taskchange.CompleteTime = item.CompleteTime;
            taskchange.LevelID = item.LevelID;
            taskchange.CreateAt = item.CreateAt;
            taskchange.UpdateAt = item.UpdateAt;
            taskchange.UpdateBy = item.UpdateBy;
            taskchange.CreateBy = item.CreateBy;
            TaskHistoryDA.Save();
            if (item.AttachmentFiles.FileAttachments.Count > 0)
            {
                var Ids = new FileSV().InsertRange(item.AttachmentFiles.FileAttachments, userID);
                new TaskAttachmentFileSV().InsertRangeByChange(Ids, taskchange.TaskID, userID);
            }
            if (item.AttachmentFiles.FileRemoves.Count > 0)
            {
                new TaskAttachmentFileSV().DeleteRangeByChange(item.AttachmentFiles.FileRemoves);
            }
        }
        public void Insert(TaskItem item, int userID, int employeeId)
        {
            item.PerformBy = item.Perform.ID;
            var taskchange = new TaskHistory()
            {
                TaskID = item.ID,
                ParentID = item.ParentID,
                Reason = item.Reason,
                Name = item.Name.Trim(),
                IsChange = false,
                IsComplete = item.IsComplete,
                IsPass = item.IsPass,
                CategoryID = item.CategoryID,
                Cost = item.Cost,
                Note = item.Note,
                IsEdit = item.IsEdit,
                IsNew = item.IsNew,
                IsPrivate = item.IsPrivate,
                Rate = item.Rate,
                DepartmentID = item.Department.ID,
                IsPause = item.IsPause,
                EmployeeID = item.PerformBy,
                EndTime = item.EndTime,
                Content = item.Content,
                StartTime = item.StartTime,
                CompleteTime = item.CompleteTime,
                LevelID = item.LevelID,
                CreateAt = item.CreateAt,
                UpdateAt = item.UpdateAt,
                UpdateBy = item.UpdateBy,
                CreateBy = item.CreateBy
            };
            TaskHistoryDA.Insert(taskchange);
            TaskHistoryDA.Save();
           
            if (item.AttachmentFiles.FileAttachments.Count > 0)
            {
                var Ids = new FileSV().InsertRange(item.AttachmentFiles.FileAttachments, userID);
                new TaskAttachmentFileSV().InsertRangeByChange(Ids, item.ID, userID);
            }
            if (item.AttachmentFiles.FileRemoves.Count > 0)
            {
                new TaskAttachmentFileSV().DeleteRangeByChange(item.AttachmentFiles.FileRemoves);                
            }
        }
        public void Update(Task obj, int changeId, string changeNote)
        {
            var change = TaskHistoryDA.GetById(changeId);
            change.ParentID = obj.ParentID;
            change.DepartmentID = obj.HumanDepartmentID;
            change.CategoryID = obj.TaskCategoryID;
            change.Cost = obj.Cost;
            change.Content = obj.Content;
            change.CreateAt = obj.CreateAt;
            change.CreateBy = obj.CreateBy;
            change.UpdateAt = obj.UpdateAt;
            change.UpdateBy = obj.UpdateBy;
            change.EndTime = obj.EndTime;
            change.IsChange = true;
            change.ChangeNote = changeNote;
            change.IsPause = obj.IsPause;
            change.IsComplete = obj.IsComplete;
            change.IsPass = obj.IsPass;
            change.LevelID = obj.TaskLevelID;
            change.Name = obj.Name.Trim();
            change.EmployeeID = obj.HumanEmployeeID;
            change.CompleteTime = obj.CompleteTime;
            change.StartTime = obj.StartTime;
            TaskHistoryDA.Update(change);
            TaskHistoryDA.Save();
        }
        public void UpdateNoteChange(TaskHistoryItem obj, int userId)
        {
            var change = TaskHistoryDA.GetById(obj.ID);
            change.IsChange = obj.IsChange;
            change.ChangeNote = obj.ChangeNote;
            TaskHistoryDA.Update(change);
            TaskHistoryDA.Save();
        }
        /// <summary>
        /// UpdateBy: CuongPC
        /// UpdateAt:10/10/2014
        /// </summary>
        /// <param name="id">id của công việc</param>
        /// <returns></returns>
        public TaskHistoryItem GetByID(int id)
        {

            var dbo = TaskHistoryDA.Repository;
            var item = TaskHistoryDA.GetById(id);
            var taskOld = dbo.Tasks.Where(t => t.ID == item.TaskID).FirstOrDefault();
            var employeeSV = new HumanEmployeeSV();
            var change = new TaskHistoryItem();
            change.ID = item.ID;
            change.ParentID = item.ParentID;
            change.ParentOldID = taskOld.ParentID;
            change.TaskID = item.TaskID;
            change.Reason = item.Reason;
            change.CategoryID = item.CategoryID;
            change.CategoryOldID = taskOld.TaskCategoryID;
            change.Cost = item.Cost;
            change.CostOld = taskOld.Cost;
            change.Department = new HumanDepartmentViewItem()
            {
                ID = item.DepartmentID,
                Name = dbo.HumanDepartments.FirstOrDefault(d => d.ID == item.DepartmentID).Name
            };
            change.DepartmentOld = new HumanDepartmentViewItem()
            {
                ID = item.DepartmentID,
                Name = dbo.HumanDepartments.FirstOrDefault(d => d.ID == taskOld.HumanDepartmentID).Name
            };
            change.DepartmentID = item.DepartmentID;
            change.DepartmentOldID = taskOld.HumanDepartmentID;
            change.Note = item.Note;        
            change.Perform = employeeSV.GetEmployeeView(item.EmployeeID);
            change.PerformOld = employeeSV.GetEmployeeView(taskOld.HumanEmployeeID);
            change.PerformEmployeesName = dbo.HumanEmployees.FirstOrDefault(u => u.ID == item.EmployeeID).Name;
            change.PerformBy = item.EmployeeID;
            change.PerformEmployeesName = dbo.HumanEmployees.FirstOrDefault(u => u.ID == item.EmployeeID).Name;
            change.LevelID = (int)item.LevelID;
            change.LevelOldID = taskOld.TaskLevelID;
            change.StartTime = item.StartTime;
            change.StartTimeOld = taskOld.StartTime;
            change.EndTime = item.EndTime;
            change.EndTimeOld = taskOld.EndTime;
            change.Content = item.Content;
            change.ContentOld = taskOld.Content;
            change.Reason = item.Reason;
            change.CompleteTime = item.CompleteTime;
            change.Name = item.Name;
            change.NameOld = taskOld.Name;
            change.IsComplete = item.IsComplete;
            change.IsPass = item.IsPass;
            change.IsPause = item.IsPause;
            change.IsChange = item.IsChange;
            change.UpdateAt = item.UpdateAt;
            change.CreateAt = item.CreateAt;
            change.CreateBy = item.CreateBy;
            change.UpdateBy = item.UpdateBy;
            change.ChangeNote = item.ChangeNote;
            change.AttachmentFiles = new AttachmentFileItem()
            {
                Files = dbo.TaskAttachmentFiles.Where(i => i.TaskID == item.TaskID && !i.IsDelete)
                    .Select(i => i.AttachmentFileID).ToList()
            };
            change.AttachmentFileOlds = new AttachmentFileItem()
            {
                Files = dbo.TaskAttachmentFiles.Where(i => i.TaskID == item.TaskID && !i.IsDelete)
                    .Select(i => i.AttachmentFileID).ToList()
            };
            return change;
        }

    }
}
