using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.Utilities;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class TaskSV
    {
        private TaskCalendarSV taskCalendarSV = new TaskCalendarSV();
        private TaskDA tasksDA = new TaskDA();
        private TaskPerformDA taskReportDA = new TaskPerformDA();
        private TaskPersonalSV personService = new TaskPersonalSV();
        public void Insert(TaskPerformItem obj, int userId, int employeeId)
        {
            var dbo = tasksDA.Repository;
            var perform = new TaskPerform();
            perform.CreateAt = DateTime.Now;
            perform.CreateBy = userId;
            perform.Content = obj.Content.Trim();
            perform.EmployeeID = (int)obj.PerformBy;
            perform.Rate = obj.Rate;
            perform.Feedback = obj.Feedback;
            perform.TaskID = obj.TaskID;
            perform.Time = obj.Time;
            dbo.TaskPerforms.Add(perform);
            personService.UpdateIsNew(dbo, obj.TaskID, employeeId);
            UpdateRateTask(dbo, obj.TaskID, obj.Rate);
            dbo.SaveChanges();
            new FileSV().Upload<TaskPerformAttachmentFile>(obj.AttachmentFiles, perform.ID);
            //var Ids = new FileSV().InsertRange(obj.AttachmentFiles.FileAttachments, userId);
            //new TaskPerformAttachmentFileSV().InsertRange(Ids, perform.ID, userId);
        }
        public void UpdateRateTask(iDASBusinessEntities dbo, int taskId, decimal rate)
        {
            var obj = dbo.Tasks.FirstOrDefault(t => t.ID == taskId);
            if (!obj.IsComplete)
            {
                obj.Rate = rate;
            }
        }
        public bool CheckNameTaskExist(string name)
        {
            var rs = tasksDA.GetQuery(t => t.Name.ToUpper() == name.ToUpper()).FirstOrDefault();
            if (rs != null)
                return true;
            return false;
        }
        public bool CheckNameTaskExistEdit(int id, string name)
        {
            var rs = tasksDA.GetQuery(t => t.ID != id && t.Name.ToUpper() == name.ToUpper()).FirstOrDefault();
            if (rs != null)
                return true;
            return false;
        }
        public void UpdateTaskForApprovePlan(int[] lstTaskId, int planId, int userId, int employeessId)
        {
            var lst = tasksDA.GetQuery(t => lstTaskId.Contains(t.ID)).ToList();
            foreach (var item in lst)
            {
                item.IsPrivate = false;
                item.IsEdit = false;
                item.IsNew = false;
                item.UpdateAt = DateTime.Now;
                item.UpdateBy = userId;
            }
            tasksDA.UpdateRange(lst);
            tasksDA.Save();
        }
        public void UpdateIsAudit(int taskId, bool isPass)
        {
            var rs = tasksDA.GetById(taskId);
            rs.IsPass = isPass;
            tasksDA.Save();
        }
        public List<TaskItem> GetByCombobox()
        {
            List<TaskItem> lst = new List<TaskItem>();
            var tasks = tasksDA.GetQuery()
                         .Where(t => !t.IsPrivate)
                        .ToList();
            foreach (var item in tasks)
            {
                lst.Add(new TaskItem
                {
                    ID = item.ID,
                    Name = item.Name
                });
            }
            return lst;
        }
        public List<TaskItem> GetTreeAllData(int nodeId)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();

            result = tasksDA
                       .GetQuery(i => (i.ParentID == nodeId || i.ID == nodeId))
                       .Where(i => !i.IsPrivate)
                       .Select(item => new TaskItem()
                       {
                           ID = item.ID,
                           ParentID = item.ParentID,
                           DepartmentID = item.HumanDepartmentID,
                           Name = item.Name,
                           CategoryID = item.TaskCategoryID,
                           CategoryName = item.TaskCategory.Name,
                           StartTime = item.StartTime,
                           CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                           EndTime = item.EndTime,
                           LevelID = item.TaskLevelID,
                           LevelName = item.TaskLevel.Name,
                           LevelColor = item.TaskLevel.Color,
                           IsNew = item.IsNew,
                           IsEdit = item.IsEdit,
                           IsComplete = item.IsComplete,
                           IsPass = item.IsPass,
                           IsPause = item.IsPause,
                           Rate = item.Rate,
                           Cost = item.Cost,
                           Content = item.Content,
                           Note = item.Note,
                           PerformBy = item.HumanEmployeeID,
                           CreateAt = item.CreateAt,
                           CreateBy = item.CreateBy,
                           UpdateAt = item.UpdateAt,
                           UpdateBy = item.UpdateBy,
                           Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID) == null
                       })
                       .ToList();
            return result;
        }
        public List<TaskItem> GetTreeTask(int taskId, int departmentId, bool isAdmin, int employeeID, int userId, Nullable<DateTime> fromdate, Nullable<DateTime> todate, string choice)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            var fromDateForQr = System.Convert.ToDateTime(fromdate);
            var toDateQr = System.Convert.ToDateTime(todate).AddDays(1);
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                      .Where(t => t.HumanDepartmentID == departmentId)
                      .Where(t => !t.IsPrivate)
                      .Where(t => t.HumanDepartmentID == departmentId || t.ParentID == 0)
                      .Where(t => isAdmin || t.CreateBy == userId || t.TaskPersonals.Where(x => x.PerformBy == employeeID).Count() > 0)
                      .Where(t => (choice.Equals("App.Time") && (t.StartTime >= fromDateForQr && t.StartTime <= toDateQr) || (t.EndTime >= fromDateForQr && t.EndTime <= toDateQr) || (t.StartTime <= fromDateForQr && t.EndTime >= toDateQr)) || choice.Equals("App.All"))
                      .Distinct()
                      .OrderByDescending(t => t.ID)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID) == null
                      })
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                           .GetQuery()
                           .Where(t => t.ParentID == taskId)
                           .Where(t => !t.IsPrivate)
                           .Where(t => isAdmin || t.CreateBy == userId || t.TaskPersonals.Where(x => x.PerformBy == employeeID).Count() > 0)
                           .Where(t => (choice.Equals("App.Time") && (t.StartTime >= fromDateForQr && t.StartTime <= toDateQr) || (t.EndTime >= fromDateForQr && t.EndTime <= toDateQr) || (t.StartTime <= fromDateForQr && t.EndTime >= toDateQr)) || choice.Equals("App.All"))
                           .Distinct()
                           .OrderByDescending(t => t.ID)
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.HumanDepartmentID != departmentId ? item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")" : item.Name,
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID) == null
                           })
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreeTaskParent(int taskId, int departmentId, bool isAdmin, int employeeID, int userId)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                      .Where(t => t.HumanDepartmentID == departmentId)
                      .Where(t => !t.IsPrivate)
                      .Where(t => !t.IsPause)
                      .Where(t => t.HumanDepartmentID == departmentId || t.ParentID == 0)
                      .Where(t => isAdmin || t.CreateBy == userId || t.TaskPersonals.Where(x => x.PerformBy == employeeID).Count() > 0)
                      .Distinct()
                      .OrderByDescending(t => t.ID)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID) == null
                      })
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                           .GetQuery()
                           .Where(t => t.ParentID == taskId)
                           .Where(t => !t.IsPrivate)
                           .Where(t => !t.IsPause)
                           .Where(t => isAdmin || t.CreateBy == userId || t.TaskPersonals.Where(x => x.PerformBy == employeeID).Count() > 0)
                           .Distinct()
                           .OrderByDescending(t => t.ID)
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.HumanDepartmentID != departmentId ? item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")" : item.Name,
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID) == null
                           })
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetAll(ModelFilter filter, int departmentId)
        {
            var dbo = tasksDA.Repository;
            List<TaskItem> lst = new List<TaskItem>();
            var tasks = tasksDA.GetQuery()
                         .Where(t => t.HumanDepartmentID == departmentId)
                         .Where(t => !t.IsPrivate)
                         .Filter(filter)
                        .ToList();
            if (tasks.Count > 0)
            {
                foreach (var item in tasks)
                {
                    lst.Add(new TaskItem
                    {
                        ID = item.ID,
                        ParentID = item.ParentID,
                        DepartmentID = item.HumanDepartmentID,
                        Name = item.Name,
                        CategoryID = item.TaskCategoryID,
                        CategoryName = item.TaskCategory.Name,
                        StartTime = item.StartTime,
                        CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                        EndTime = item.EndTime,
                        LevelID = item.TaskLevelID,
                        LevelName = item.TaskLevel.Name,
                        LevelColor = item.TaskLevel.Color,
                        IsNew = item.IsNew,
                        IsEdit = item.IsEdit,
                        IsComplete = item.IsComplete,
                        IsPass = item.IsPass,
                        IsPause = item.IsPause,
                        Rate = item.Rate,
                        Cost = item.Cost,
                        Content = item.Content,
                        Note = item.Note,
                        PerformBy = item.HumanEmployeeID,
                        CreateAt = item.CreateAt,
                        CreateBy = item.CreateBy,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy,
                    });
                }
            }
            return lst;
        }
        public TaskPerformItem GetByTaskID(int id)
        {
            var dbo = tasksDA.Repository;
            var employeeSV = new HumanEmployeeSV();
            var item = tasksDA.GetById(id);
            var perform = new TaskPerformItem();
            perform.Rate = item.Rate;
            perform.ContentTask = item.Content;
            perform.Time = DateTime.Now;
            perform.PerformBy = item.HumanEmployeeID;
            perform.TaskID = item.ID;
            perform.Perform = employeeSV.GetEmployeeView(item.HumanEmployeeID);
            return perform;
        }
        public TaskCheckItem GetCheckByTaskID(int id)
        {
            var dbo = tasksDA.Repository;
            var perf = dbo.TaskPerforms.Where(t => t.TaskID == id && !t.IsCheck).OrderByDescending(t => t.CreateAt).ToList();
            var employeeSV = new HumanEmployeeSV();
            var item = tasksDA.GetById(id);
            var check = new TaskCheckItem();
            var strContent = "";
            if (perf.Count > 0)
            {
                int CreateByID = perf[0].CreateBy.Value;
                decimal RateMax = perf[0].Rate;
                DateTime TimeMax = perf[0].Time;
                check.Perform = employeeSV.GetEmployeeView(dbo.HumanUsers.FirstOrDefault(t => t.ID == CreateByID).HumanEmployeeID);
                check.RatePerform = RateMax;
                check.TimePerform = TimeMax;
                foreach (var s in perf)
                {

                    if (s.Content != null && !string.IsNullOrEmpty(s.Content))
                    {
                        strContent += "&rarr;<u>Thời gian: <i>" + s.Time.ToShortDateString() + " " + s.Time.ToShortTimeString() + "</i></u>" + "<br/><br/> &#8226;&nbsp;<b>Nội dung báo cáo:</b> <br/><p style='text-align: justify; line-height: 17px;'>" + s.Content + "</p><br/>";
                    }
                    if (s.Feedback != null && !string.IsNullOrEmpty(s.Feedback))
                    {
                        strContent += "&#8226;&nbsp;<b>Nội dung phản hồi: </b>" + "<br/><p style='text-align: justify; line-height: 17px;'>" + s.Feedback + "</p><br/>";
                    }
                }
                List<int> lstPerformID = perf.Select(t => t.ID).ToList();
                check.AttachmentFiles = new AttachmentFileItem()
                {
                    Files = dbo.TaskPerformAttachmentFiles.Where(i => lstPerformID.Contains(i.TaskPerformID))
                        .Select(i => i.AttachmentFileID).ToList()
                };
            }
            check.ContentTask = item.Content;
            check.ContentPerform = strContent;
            check.CompleteTime = item.CompleteTime.Value;
            check.Rate = item.Rate;
            check.Time = DateTime.Now;
            check.Checker = employeeSV.GetEmployeeView(dbo.HumanUsers.FirstOrDefault(t => t.ID == item.CreateBy).HumanEmployeeID);
            check.TaskID = item.ID;
            check.IsComplete = item.IsComplete;
            return check;
        }
        /// <summary>
        /// UpdateBy: CuongPC
        /// UpdateAt:10/10/2014
        /// </summary>
        /// <param name="id">id của công việc</param>
        /// <returns></returns>        
        public TaskItem GetByID(int id)
        {
            var dbo = tasksDA.Repository;
            var item = tasksDA.GetById(id);
            var task = new TaskItem();
            if (item != null)
            {
                var employeeSV = new HumanEmployeeSV();
                task.ID = item.ID;
                task.ParentID = item.ParentID;
                task.PerformBy = item.HumanEmployeeID;
                task.DepartmentID = item.HumanDepartmentID;
                task.DepartmentName = dbo.HumanDepartments.FirstOrDefault(i => i.ID == item.HumanDepartmentID).Name;
                task.Department = new HumanDepartmentViewItem()
                             {
                                 ID = item.HumanDepartmentID,
                                 Name = dbo.HumanDepartments.FirstOrDefault(d => d.ID == item.HumanDepartmentID).Name
                             };
                task.Cost = item.Cost;
                task.Content = item.Content;
                task.Create = employeeSV.GetEmployeeView(dbo.HumanUsers.FirstOrDefault(t => t.ID == item.CreateBy).HumanEmployeeID);
                task.Perform = employeeSV.GetEmployeeView(item.HumanEmployeeID);
                task.CategoryID = item.TaskCategoryID;
                task.CategoryName = item.TaskCategory.Name;
                task.Cost = item.Cost;
                task.CompleteTime = item.CompleteTime.HasValue ? item.CompleteTime.Value : item.EndTime;
                task.Rate = item.Rate;
                task.LevelID = item.TaskLevelID;
                task.IsNew = item.IsNew;
                task.LevelColor = item.TaskLevel.Color;
                task.LevelName = item.TaskLevel.Name;
                task.StartTime = item.StartTime;
                task.EndTime = item.EndTime;
                task.Content = item.Content;
                task.Name = item.Name;
                task.IsComplete = item.IsComplete;
                task.IsEdit = item.IsEdit;
                task.IsPause = item.IsPause;
                task.Content = item.Content;
                task.IsPass = item.IsPass;
                task.UpdateAt = item.UpdateAt;
                task.UpdateBy = item.UpdateBy;
                task.CreateAt = item.CreateAt;
                task.CreateBy = item.CreateBy;
            }
            task.AttachmentFiles = new AttachmentFileItem()
            {
                Files = dbo.TaskAttachmentFiles.Where(i => i.TaskID == id && !i.IsChange)
                    .Select(i => i.AttachmentFileID).ToList()
            };
            return task;
        }
        public TaskItem GetByIDForPlan(int id)
        {

            var item = tasksDA.GetById(id);
            var task = new TaskItem();
            if (item != null)
            {
                var employeeSV = new HumanEmployeeSV();
                task.ID = item.ID;
                task.ParentID = item.ParentID;
                task.PerformBy = item.HumanEmployeeID;
                task.DepartmentID = item.HumanDepartmentID;
                task.Cost = item.Cost;
                task.Content = item.Content;
                task.Perform = employeeSV.GetEmployeeView(item.HumanEmployeeID);
                task.CategoryID = item.TaskCategoryID;
                task.CategoryName = item.TaskCategory.Name;
                task.Cost = item.Cost;
                task.Rate = item.Rate;
                task.LevelID = item.TaskLevelID;
                task.IsNew = item.IsNew;
                task.LevelColor = item.TaskLevel.Color;
                task.LevelName = item.TaskLevel.Name;
                task.StartTime = item.StartTime;
                task.EndTime = item.EndTime;
                task.CompleteTime = item.CompleteTime.Value;
                task.Content = item.Content;
                task.Name = item.Name;
                task.IsComplete = item.IsComplete;
                task.IsEdit = item.IsEdit;
                task.IsPause = item.IsPause;
                task.Content = item.Content;
                task.IsPass = item.IsPass;
                task.UpdateAt = item.UpdateAt;
            }
            return task;
        }
        public void UpdateTask(TaskItem item, int userID, int employeeId)
        {
            var task = tasksDA.GetById(item.ID);
            task.ParentID = item.ParentID;
            task.Name = item.Name.Trim();
            task.HumanEmployeeID = item.Perform.ID != 0 ? item.Perform.ID : employeeId;
            task.IsEdit = item.IsEdit;
            task.Cost = item.Cost;
            task.TaskCategoryID = item.CategoryID;
            task.IsNew = item.IsNew;
            task.Content = item.Content;
            task.HumanDepartmentID = item.Department.ID;
            task.IsPause = item.IsPause;
            task.StartTime = item.StartTime;
            task.EndTime = item.EndTime;
            task.Content = item.Content;
            task.TaskLevelID = item.LevelID;
            task.UpdateAt = DateTime.Now;
            task.UpdateBy = userID;
            tasksDA.Save();
            new FileSV().Upload<TaskAttachmentFile>(item.AttachmentFiles, item.ID);
            item.PerformBy = task.HumanEmployeeID;
            personService.Update(item, employeeId, userID);
        }
        public int Insert(TaskItem item, int userID, int employeeId)
        {
            var task = new iDAS.Base.Task()
             {
                 ParentID = item.ParentID,
                 HumanDepartmentID = item.Department.ID,
                 TaskCategoryID = item.CategoryID,
                 TaskLevelID = item.LevelID,
                 Name = item.Name.Trim(),
                 StartTime = item.StartTime,
                 CompleteTime = item.EndTime,
                 EndTime = item.EndTime,
                 IsPrivate = item.IsPrivate,
                 IsNew = item.IsNew,
                 IsEdit = item.IsEdit,
                 IsComplete = item.IsComplete,
                 IsPass = item.IsPass,
                 IsPause = item.IsPause,
                 Rate = item.Rate,
                 Cost = item.Cost,
                 HumanEmployeeID = item.Perform.ID != 0 ? item.Perform.ID : employeeId,
                 Note = item.Note,
                 Content = item.Content,
                 CreateAt = DateTime.Now,
                 CreateBy = userID
             };
            tasksDA.Insert(task);
            tasksDA.Save();
            new FileSV().Upload<TaskAttachmentFile>(item.AttachmentFiles, task.ID);
            item.ID = task.ID;
            item.PerformBy = task.HumanEmployeeID;
            personService.Insert(item, employeeId, userID);
            return task.ID;
        }
        public int Insert(TaskRequestItem item, int userID, int employeeId)
        {
            var task = new iDAS.Base.Task()
            {
                ParentID = item.ParentID.HasValue ? item.ParentID.Value : 0,
                HumanDepartmentID = item.Department.ID,
                TaskCategoryID = item.TaskCategoryID,
                TaskLevelID = item.TaskLevelID,
                Name = item.Name.Trim(),
                StartTime = item.StartTime,
                CompleteTime = item.EndTime,
                EndTime = item.EndTime,
                IsPrivate = false,
                IsNew = false,
                IsEdit = item.IsEdit,
                IsComplete = false,
                IsPass = false,
                IsPause = false,
                Rate = 0,
                Cost = item.Cost,
                HumanEmployeeID = item.Perform.ID != 0 ? item.Perform.ID : employeeId,
                Note = item.Note,
                Content = item.Contents,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            tasksDA.Insert(task);
            tasksDA.Save();
            item.ID = task.ID;
            item.HumanEmployeeID = task.HumanEmployeeID;
            personService.Insert(item, employeeId, userID);
            return task.ID;
        }
        public void UpdateChange(TaskHistory obj)
        {
            var dbo = tasksDA.Repository;
            var task = tasksDA.GetById(obj.TaskID);
            task.Content = obj.Content;
            task.ParentID = obj.ParentID;
            task.EndTime = obj.EndTime;
            task.CompleteTime = obj.CompleteTime;
            task.HumanDepartmentID = obj.DepartmentID;
            task.TaskCategoryID = obj.CategoryID;
            task.Cost = obj.Cost;
            task.IsPause = obj.IsPause;
            task.IsComplete = obj.IsComplete;
            task.IsPass = obj.IsPass;
            task.TaskLevelID = obj.LevelID;
            task.Name = obj.Name.Trim();
            task.Content = obj.Content;
            task.HumanEmployeeID = obj.EmployeeID;
            task.StartTime = obj.StartTime;
            tasksDA.Update(task);
            List<Guid> lstDelete = dbo.TaskAttachmentFiles.Where(t => t.TaskID == obj.TaskID).Where(t => t.IsDelete).Select(t => t.AttachmentFileID).ToList();
            new TaskAttachmentFileSV().DeleteRange(lstDelete);
            new FileSV().DeleteRange(lstDelete);
            var lstfile = dbo.TaskAttachmentFiles.Where(t => t.TaskID == obj.TaskID).ToList();
            foreach (var item in lstfile)
            {
                item.IsChange = false;
            }
            tasksDA.Save();
        }
        public void Update(TaskItem item, int userID)
        {
            var task = tasksDA.GetById(item.ID);
            task.ParentID = item.ParentID;
            task.Name = item.Name.Trim();
            task.Content = item.Content;
            task.Content = item.Content;
            task.TaskLevelID = item.LevelID;
            task.StartTime = item.StartTime;
            task.UpdateAt = DateTime.Now;
            task.UpdateBy = userID;
            tasksDA.Save();
        }
        public int Update(TaskItem item)
        {
            var task = tasksDA.GetById(item.ID);
            task.ParentID = item.ParentID;
            task.Name = item.Name.Trim();
            task.Content = item.Content;
            task.Content = item.Content;
            task.TaskLevelID = item.LevelID;
            task.StartTime = item.StartTime;
            task.UpdateAt = DateTime.Now;
            task.UpdateBy = item.UpdateBy;
            tasksDA.Save();
            return task.ID;
        }
        public void PauseTask(int taskId, int userID, int employeeId)
        {
            var task = tasksDA.GetById(taskId);
            task.IsPause = task.IsPause ? false : true;
            tasksDA.Save();
        }
        public void UpdateIsPass(int taskId, decimal rate, bool isComplete, DateTime completeTime)
        {
            var task = tasksDA.GetById(taskId);
            task.ID = task.ID;
            task.Rate = rate;
            if (isComplete)
            {
                task.CompleteTime = completeTime;
            }
            task.IsComplete = isComplete;
            tasksDA.Update(task);
            tasksDA.Save();
        }
        public void Delete(string stringId)
        {
            var taskIds = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            tasksDA.DeleteRange(taskIds);
            tasksDA.Save();
        }
        // Dùng cho thống kê
        /// <summary>
        /// Dùng cho thống kê theo phòng ban
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="departmentId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<TaskItem> GetTreeTotalTaskByDepartmentID(int taskId, int? departmentId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                      .Where(t => t.HumanDepartmentID == departmentId)
                      .Where(t => !t.IsNew)
                      .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                      .Where(t => t.HumanDepartmentID == departmentId || t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanDepartmentID == departmentId) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                           .GetQuery()
                           .Where(t => t.ParentID == taskId)
                           .Where(t => t.HumanDepartmentID == departmentId)
                           .Where(t => !t.IsNew)
                           .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.HumanDepartmentID != departmentId ? item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")" : item.Name,
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanDepartmentID == departmentId) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreeTotalTaskAll(int taskId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                      .Where(t => !t.IsNew)
                      .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                      .Where(t => t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                           .GetQuery()
                           .Where(t => t.ParentID == taskId)
                           .Where(t => !t.IsNew)
                           .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")",
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreePauseTaskByDepartmentID(int taskId, int? departmentId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                      .Where(t => t.HumanDepartmentID == departmentId)
                         .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsPause)
                      .Where(t => t.HumanDepartmentID == departmentId || t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanDepartmentID == departmentId) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                           .GetQuery()
                           .Where(t => t.ParentID == taskId)
                                      .Where(t => t.HumanDepartmentID == departmentId)
                               .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsPause)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.HumanDepartmentID != departmentId ? item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")" : item.Name,
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanDepartmentID == departmentId) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreePauseTaskAll(int taskId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                      .Where(t => !t.IsNew)
                      .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                      .Where(t => t.IsPause)
                      .Where(t => t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                           .GetQuery()
                           .Where(t => t.ParentID == taskId)
                           .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsPause)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")",
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreeOverTimeTaskByDepartmentID(int taskId, int? departmentId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                      .Where(t => t.HumanDepartmentID == departmentId)
                      .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => !t.IsComplete && t.Rate != 100 && !t.IsPause)
                        .Where(t => t.EndTime <= DateTime.Now)
                      .Where(t => t.HumanDepartmentID == departmentId || t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanDepartmentID == departmentId) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                           .GetQuery()
                           .Where(t => t.ParentID == taskId)
                                      .Where(t => t.HumanDepartmentID == departmentId)
                          .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => !t.IsComplete && t.Rate != 100 && !t.IsPause)
                        .Where(t => t.EndTime <= DateTime.Now)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.HumanDepartmentID != departmentId ? item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")" : item.Name,
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanDepartmentID == departmentId) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreeOverTimeTaskAll(int taskId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                      .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => !t.IsComplete && t.Rate != 100 && !t.IsPause)
                        .Where(t => t.EndTime <= DateTime.Now)
                    // .Where(t => t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                        .GetQuery()
                        .Where(t => t.ParentID == taskId)
                        .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => !t.IsComplete && t.Rate != 100 && !t.IsPause)
                        .Where(t => t.EndTime <= DateTime.Now)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")",
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreeDoingTaskByDepartmentID(int taskId, int? departmentId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                      .Where(t => t.HumanDepartmentID == departmentId)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => !t.IsComplete && !t.IsPause)
                   .Where(t => t.Rate == 100 || t.EndTime >= DateTime.Now)
                      .Where(t => t.HumanDepartmentID == departmentId || t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanDepartmentID == departmentId) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                            .GetQuery()
                            .Where(t => t.ParentID == taskId)
                                       .Where(t => t.HumanDepartmentID == departmentId)
                            .Where(t => !t.IsNew)
                            .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                            .Where(t => !t.IsComplete && !t.IsPause)
                            .Where(t => t.Rate == 100 || t.EndTime >= DateTime.Now)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.HumanDepartmentID != departmentId ? item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")" : item.Name,
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanDepartmentID == departmentId) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreeDoingTaskAll(int taskId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                    .GetQuery()
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => !t.IsComplete && !t.IsPause)
                    .Where(t => t.Rate == 100 || t.EndTime >= DateTime.Now)
                    // .Where(t =>t.HumanDepartmentID == departmentId || t.ParentID == 0)
                    .Select(item => new TaskItem()
                    {
                        ID = item.ID,
                        ParentID = item.ParentID,
                        DepartmentID = item.HumanDepartmentID,
                        Name = item.Name,
                        CategoryID = item.TaskCategoryID,
                        CategoryName = item.TaskCategory.Name,
                        StartTime = item.StartTime,
                        CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                        EndTime = item.EndTime,
                        LevelID = item.TaskLevelID,
                        LevelName = item.TaskLevel.Name,
                        LevelColor = item.TaskLevel.Color,
                        IsNew = item.IsNew,
                        IsEdit = item.IsEdit,
                        IsComplete = item.IsComplete,
                        IsPass = item.IsPass,
                        IsPause = item.IsPause,
                        Rate = item.Rate,
                        Cost = item.Cost,
                        Content = item.Content,
                        Note = item.Note,
                        PerformBy = item.HumanEmployeeID,
                        CreateAt = item.CreateAt,
                        CreateBy = item.CreateBy,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy,
                        Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID) == null
                    })
                      .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                            .GetQuery()
                            .Where(t => t.ParentID == taskId)
                            .Where(t => !t.IsNew)
                            .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                            .Where(t => !t.IsComplete && !t.IsPause)
                            .Where(t => t.Rate == 100 || t.EndTime >= DateTime.Now)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")",
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreeFinishTaskByDepartmentID(int taskId, int? departmentId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                      .Where(t => t.HumanDepartmentID == departmentId)
                       .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => t.IsComplete && t.Rate == 100 && !t.IsPause)
                        .Where(t => t.EndTime >= t.CompleteTime)
                      .Where(t => t.HumanDepartmentID == departmentId || t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanDepartmentID == departmentId) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                           .GetQuery()
                           .Where(t => t.ParentID == taskId)
                                      .Where(t => t.HumanDepartmentID == departmentId)
                            .Where(t => !t.IsNew)
                            .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                            .Where(t => t.IsComplete && t.Rate == 100 && !t.IsPause)
                            .Where(t => t.EndTime >= t.CompleteTime)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.HumanDepartmentID != departmentId ? item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")" : item.Name,
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanDepartmentID == departmentId) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreeFinishTaskAll(int taskId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                       .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => t.IsComplete && t.Rate == 100 && !t.IsPause)
                        .Where(t => t.EndTime >= t.CompleteTime)
                    // .Where(t => t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                            .GetQuery()
                            .Where(t => t.ParentID == taskId)
                            .Where(t => !t.IsNew)
                            .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                            .Where(t => t.IsComplete && t.Rate == 100 && !t.IsPause)
                            .Where(t => t.EndTime >= t.CompleteTime)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")",
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreeFinishOverTimeTaskByDepartmentID(int taskId, int? departmentId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                      .Where(t => t.HumanDepartmentID == departmentId)
                     .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsComplete && t.Rate == 100)
                    .Where(t => t.EndTime < t.CompleteTime)
                      .Where(t => t.HumanDepartmentID == departmentId || t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanDepartmentID == departmentId) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                           .GetQuery()
                           .Where(t => t.ParentID == taskId)
                                      .Where(t => t.HumanDepartmentID == departmentId)
                         .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsComplete && t.Rate == 100)
                    .Where(t => t.EndTime < t.CompleteTime)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.HumanDepartmentID != departmentId ? item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")" : item.Name,
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanDepartmentID == departmentId) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreeFinishOverTimeTaskAll(int taskId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                     .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsComplete && t.Rate == 100)
                    .Where(t => t.EndTime < t.CompleteTime)
                    //    .Where(t =>t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                           .GetQuery()
                           .Where(t => t.ParentID == taskId)
                         .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsComplete && t.Rate == 100)
                    .Where(t => t.EndTime < t.CompleteTime)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")",
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }


        /// <summary>
        /// Thống kê công việc đánh giá
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="departmentId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>

        public List<TaskItem> GetTreeAuditNotTaskByDepartmentID(int taskId, int? departmentId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                    .GetQuery()
                    .Where(t => t.HumanDepartmentID == departmentId)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => !t.AuditID.HasValue)
                    .Where(t => t.HumanDepartmentID == departmentId || t.ParentID == 0)
                    .Select(item => new TaskItem()
                    {
                        ID = item.ID,
                        ParentID = item.ParentID,
                        DepartmentID = item.HumanDepartmentID,
                        Name = item.Name,
                        CategoryID = item.TaskCategoryID,
                        CategoryName = item.TaskCategory.Name,
                        StartTime = item.StartTime,
                        CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                        EndTime = item.EndTime,
                        LevelID = item.TaskLevelID,
                        LevelName = item.TaskLevel.Name,
                        LevelColor = item.TaskLevel.Color,
                        IsNew = item.IsNew,
                        IsEdit = item.IsEdit,
                        IsComplete = item.IsComplete,
                        IsPass = item.IsPass,
                        IsPause = item.IsPause,
                        Rate = item.Rate,
                        Cost = item.Cost,
                        Content = item.Content,
                        Note = item.Note,
                        PerformBy = item.HumanEmployeeID,
                        CreateAt = item.CreateAt,
                        CreateBy = item.CreateBy,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy,
                        Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanDepartmentID == departmentId) == null
                    })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                            .GetQuery()
                            .Where(t => t.ParentID == taskId)
                                       .Where(t => t.HumanDepartmentID == departmentId)
                            .Where(t => !t.IsNew)
                            .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                            .Where(t => !t.AuditID.HasValue)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.HumanDepartmentID != departmentId ? item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")" : item.Name,
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanDepartmentID == departmentId) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreeAuditNotTaskAll(int taskId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                    .GetQuery()
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => !t.AuditID.HasValue)
                    // .Where(t =>t.HumanDepartmentID == departmentId || t.ParentID == 0)
                    .Select(item => new TaskItem()
                    {
                        ID = item.ID,
                        ParentID = item.ParentID,
                        DepartmentID = item.HumanDepartmentID,
                        Name = item.Name,
                        CategoryID = item.TaskCategoryID,
                        CategoryName = item.TaskCategory.Name,
                        StartTime = item.StartTime,
                        CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                        EndTime = item.EndTime,
                        LevelID = item.TaskLevelID,
                        LevelName = item.TaskLevel.Name,
                        LevelColor = item.TaskLevel.Color,
                        IsNew = item.IsNew,
                        IsEdit = item.IsEdit,
                        IsComplete = item.IsComplete,
                        IsPass = item.IsPass,
                        IsPause = item.IsPause,
                        Rate = item.Rate,
                        Cost = item.Cost,
                        Content = item.Content,
                        Note = item.Note,
                        PerformBy = item.HumanEmployeeID,
                        CreateAt = item.CreateAt,
                        CreateBy = item.CreateBy,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy,
                        Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID) == null
                    })
                      .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                            .GetQuery()
                            .Where(t => t.ParentID == taskId)
                             .Where(t => !t.IsNew)
                            .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                            .Where(t => !t.AuditID.HasValue)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")",
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreeAuditGoodTaskByDepartmentID(int taskId, int? departmentId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                      .Where(t => t.HumanDepartmentID == departmentId)
                          .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.AuditID.HasValue)
                      .Where(t => t.Audit.IsPass)
                      .Where(t => t.HumanDepartmentID == departmentId || t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanDepartmentID == departmentId) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                           .GetQuery()
                           .Where(t => t.ParentID == taskId)
                            .Where(t => t.HumanDepartmentID == departmentId)
                            .Where(t => !t.IsNew)
                            .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                            .Where(t => t.AuditID.HasValue)
                              .Where(t => t.Audit.IsPass)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.HumanDepartmentID != departmentId ? item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")" : item.Name,
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanDepartmentID == departmentId) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreeAuditGoodTaskAll(int taskId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                      .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => t.AuditID.HasValue)
                      .Where(t => t.Audit.IsPass)
                    // .Where(t => t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                            .GetQuery()
                            .Where(t => t.ParentID == taskId)
                            .Where(t => !t.IsNew)
                            .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                            .Where(t => t.AuditID.HasValue)
                            .Where(t => t.Audit.IsPass)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")",
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreeAuditFailTaskByDepartmentID(int taskId, int? departmentId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                      .Where(t => t.HumanDepartmentID == departmentId)
                         .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.AuditID.HasValue)
                      .Where(t => !t.Audit.IsPass)
                      .Where(t => t.HumanDepartmentID == departmentId || t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanDepartmentID == departmentId) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                           .GetQuery()
                           .Where(t => t.ParentID == taskId)
                                      .Where(t => t.HumanDepartmentID == departmentId)
                            .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.AuditID.HasValue)
                      .Where(t => !t.Audit.IsPass)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.HumanDepartmentID != departmentId ? item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")" : item.Name,
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanDepartmentID == departmentId) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreeAuditFailTaskAll(int taskId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                        .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.AuditID.HasValue)
                      .Where(t => !t.Audit.IsPass)
                    //    .Where(t =>t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                           .GetQuery()
                           .Where(t => t.ParentID == taskId)
                           .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.AuditID.HasValue)
                      .Where(t => !t.Audit.IsPass)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")",
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }


        /// <summary>
        /// Dùng cho thống kê theo cá nhân
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="departmentId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<TaskItem> GetTreeTotalTaskByEmployeeID(int taskId, int? employeeId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                      .Where(t => t.HumanEmployeeID == employeeId)
                      .Where(t => !t.IsNew)
                      .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                      .Where(t => t.HumanEmployeeID == employeeId || t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanEmployeeID == employeeId) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                           .GetQuery()
                           .Where(t => t.ParentID == taskId)
                                .Where(t => t.HumanEmployeeID == employeeId)
                           .Where(t => !t.IsNew)
                           .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")",
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanEmployeeID == employeeId) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreePauseTaskByEmployeeID(int taskId, int? employeeId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                      .Where(t => t.HumanEmployeeID == employeeId)
                         .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsPause)
                      .Where(t => t.HumanEmployeeID == employeeId || t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanEmployeeID == employeeId) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                           .GetQuery()
                           .Where(t => t.ParentID == taskId)
                                .Where(t => t.HumanEmployeeID == employeeId)
                               .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsPause)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")",
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanEmployeeID == employeeId) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreeOverTimeTaskByEmployeeID(int taskId, int? employeeId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                     .Where(t => t.HumanEmployeeID == employeeId)
                      .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => !t.IsComplete && t.Rate != 100 && !t.IsPause)
                        .Where(t => t.EndTime <= DateTime.Now)
                      .Where(t => t.HumanEmployeeID == employeeId || t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanEmployeeID == employeeId) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                           .GetQuery()
                           .Where(t => t.ParentID == taskId)
                                .Where(t => t.HumanEmployeeID == employeeId)
                          .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => !t.IsComplete && t.Rate != 100 && !t.IsPause)
                        .Where(t => t.EndTime <= DateTime.Now)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")",
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanEmployeeID == employeeId) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreeDoingTaskByEmployeeID(int taskId, int? employeeId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                   .Where(t => t.HumanEmployeeID == employeeId)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => !t.IsComplete && !t.IsPause)
                   .Where(t => t.Rate == 100 || t.EndTime >= DateTime.Now)
                      .Where(t => t.HumanEmployeeID == employeeId || t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanEmployeeID == employeeId) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                            .GetQuery()
                            .Where(t => t.ParentID == taskId)
                                 .Where(t => t.HumanEmployeeID == employeeId)
                            .Where(t => !t.IsNew)
                            .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                            .Where(t => !t.IsComplete && !t.IsPause)
                            .Where(t => t.Rate == 100 || t.EndTime >= DateTime.Now)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")",
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanEmployeeID == employeeId) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreeFinishTaskByEmployeeID(int taskId, int? employeeId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                         .Where(t => t.HumanEmployeeID == employeeId)
                       .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => t.IsComplete && t.Rate == 100 && !t.IsPause)
                        .Where(t => t.EndTime >= t.CompleteTime)
                      .Where(t => t.HumanEmployeeID == employeeId || t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanEmployeeID == employeeId) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                           .GetQuery()
                           .Where(t => t.ParentID == taskId)
                            .Where(t => t.HumanEmployeeID == employeeId)
                            .Where(t => !t.IsNew)
                            .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                            .Where(t => t.IsComplete && t.Rate == 100 && !t.IsPause)
                            .Where(t => t.EndTime >= t.CompleteTime)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")",
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanEmployeeID == employeeId) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreeFinishOverTimeTaskByEmployeeID(int taskId, int? employeeId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                      .Where(t => t.HumanEmployeeID == employeeId)
                     .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsComplete && t.Rate == 100)
                    .Where(t => t.EndTime < t.CompleteTime)
                      .Where(t => t.HumanEmployeeID == employeeId || t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanEmployeeID == employeeId) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                           .GetQuery()
                           .Where(t => t.ParentID == taskId)
                             .Where(t => t.HumanEmployeeID == employeeId)
                         .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsComplete && t.Rate == 100)
                    .Where(t => t.EndTime < t.CompleteTime)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")",
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanEmployeeID == employeeId) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }

        public List<TaskItem> GetTreeAuditNotTaskByEmployeeID(int taskId, int? employeeId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                        .GetQuery()
                        .Where(t => t.HumanEmployeeID == employeeId)
                         .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => !t.AuditID.HasValue)
                        .Where(t => t.HumanEmployeeID == employeeId || t.ParentID == 0)
                        .Select(item => new TaskItem()
                        {
                            ID = item.ID,
                            ParentID = item.ParentID,
                            DepartmentID = item.HumanDepartmentID,
                            Name = item.Name,
                            CategoryID = item.TaskCategoryID,
                            CategoryName = item.TaskCategory.Name,
                            StartTime = item.StartTime,
                            CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                            EndTime = item.EndTime,
                            LevelID = item.TaskLevelID,
                            LevelName = item.TaskLevel.Name,
                            LevelColor = item.TaskLevel.Color,
                            IsNew = item.IsNew,
                            IsEdit = item.IsEdit,
                            IsComplete = item.IsComplete,
                            IsPass = item.IsPass,
                            IsPause = item.IsPause,
                            Rate = item.Rate,
                            Cost = item.Cost,
                            Content = item.Content,
                            Note = item.Note,
                            PerformBy = item.HumanEmployeeID,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanEmployeeID == employeeId) == null
                        })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                            .GetQuery()
                            .Where(t => t.ParentID == taskId)
                            .Where(t => t.HumanEmployeeID == employeeId)
                            .Where(t => !t.IsNew)
                            .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                            .Where(t => !t.AuditID.HasValue)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")",
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanEmployeeID == employeeId) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreeAuditGoodTaskByEmployeeID(int taskId, int? employeeId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                      .Where(t => t.HumanEmployeeID == employeeId)
                      .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => t.AuditID.HasValue)
                        .Where(t => t.Audit.IsPass)
                      .Where(t => t.HumanEmployeeID == employeeId || t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanEmployeeID == employeeId) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                           .GetQuery()
                           .Where(t => t.ParentID == taskId)
                            .Where(t => t.HumanEmployeeID == employeeId)
                         .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => t.AuditID.HasValue)
                        .Where(t => t.Audit.IsPass)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")",
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanEmployeeID == employeeId) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreeAuditFailTaskByEmployeeID(int taskId, int? employeeId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                      .Where(t => t.HumanEmployeeID == employeeId)
                     .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => t.AuditID.HasValue)
                        .Where(t => !t.Audit.IsPass)
                      .Where(t => t.HumanEmployeeID == employeeId || t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanEmployeeID == employeeId) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                           .GetQuery()
                           .Where(t => t.ParentID == taskId)
                             .Where(t => t.HumanEmployeeID == employeeId)
                        .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => t.AuditID.HasValue)
                        .Where(t => !t.Audit.IsPass)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")",
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.HumanEmployeeID == employeeId) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }

        /// <summary>
        /// Dùng cho thống kê theo nhóm công việc
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="departmentId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<TaskItem> GetTreeTotalTaskByCateID(int taskId, int? cateId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                      .Where(t => t.TaskCategoryID == cateId)
                      .Where(t => !t.IsNew)
                      .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                      .Where(t => t.TaskCategoryID == cateId || t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.TaskCategoryID == cateId) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                           .GetQuery()
                           .Where(t => t.ParentID == taskId)
                                .Where(t => t.TaskCategoryID == cateId)
                           .Where(t => !t.IsNew)
                           .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")",
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.TaskCategoryID == cateId) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreePauseTaskByCateID(int taskId, int? cateId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                      .Where(t => t.TaskCategoryID == cateId)
                         .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsPause)
                      .Where(t => t.TaskCategoryID == cateId || t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.TaskCategoryID == cateId) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                           .GetQuery()
                           .Where(t => t.ParentID == taskId)
                                .Where(t => t.TaskCategoryID == cateId)
                               .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsPause)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")",
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.TaskCategoryID == cateId) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreeOverTimeTaskByCateID(int taskId, int? cateId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                     .Where(t => t.TaskCategoryID == cateId)
                      .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => !t.IsComplete && t.Rate != 100 && !t.IsPause)
                        .Where(t => t.EndTime <= DateTime.Now)
                      .Where(t => t.TaskCategoryID == cateId || t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.TaskCategoryID == cateId) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                           .GetQuery()
                           .Where(t => t.ParentID == taskId)
                                .Where(t => t.TaskCategoryID == cateId)
                          .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => !t.IsComplete && t.Rate != 100 && !t.IsPause)
                        .Where(t => t.EndTime <= DateTime.Now)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")",
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.TaskCategoryID == cateId) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreeDoingTaskByCateID(int taskId, int? cateId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                   .Where(t => t.TaskCategoryID == cateId)
                    .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => !t.IsComplete && !t.IsPause)
                    .Where(t => t.Rate == 100 || t.EndTime >= DateTime.Now)
                      .Where(t => t.TaskCategoryID == cateId || t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.TaskCategoryID == cateId) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                            .GetQuery()
                            .Where(t => t.ParentID == taskId)
                                 .Where(t => t.TaskCategoryID == cateId)
                            .Where(t => !t.IsNew)
                            .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                            .Where(t => !t.IsComplete && !t.IsPause)
                            .Where(t => t.Rate == 100 || t.EndTime >= DateTime.Now)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")",
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.TaskCategoryID == cateId) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreeFinishTaskByCateID(int taskId, int? cateId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                         .Where(t => t.TaskCategoryID == cateId)
                       .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => t.IsComplete && t.Rate == 100 && !t.IsPause)
                        .Where(t => t.EndTime >= t.CompleteTime)
                      .Where(t => t.TaskCategoryID == cateId || t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.TaskCategoryID == cateId) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                           .GetQuery()
                           .Where(t => t.ParentID == taskId)
                            .Where(t => t.TaskCategoryID == cateId)
                            .Where(t => !t.IsNew)
                            .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                            .Where(t => t.IsComplete && t.Rate == 100 && !t.IsPause)
                            .Where(t => t.EndTime >= t.CompleteTime)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")",
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.TaskCategoryID == cateId) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreeFinishOverTimeTaskByCateID(int taskId, int? cateId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                      .Where(t => t.TaskCategoryID == cateId)
                     .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsComplete && t.Rate == 100)
                    .Where(t => t.EndTime < t.CompleteTime)
                      .Where(t => t.TaskCategoryID == cateId || t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.TaskCategoryID == cateId) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                           .GetQuery()
                           .Where(t => t.ParentID == taskId)
                           .Where(t => t.TaskCategoryID == cateId)
                         .Where(t => !t.IsNew)
                    .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                    .Where(t => t.IsComplete && t.Rate == 100)
                    .Where(t => t.EndTime < t.CompleteTime)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")",
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.TaskCategoryID == cateId) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }

        /// <summary>
        /// View task audit
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="cateId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>

        public List<TaskItem> GetTreeAuditNotTaskByCateID(int taskId, int? cateId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                   .Where(t => t.TaskCategoryID == cateId)
                      .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => !t.AuditID.HasValue)
                      .Where(t => t.TaskCategoryID == cateId || t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.TaskCategoryID == cateId) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                            .GetQuery()
                            .Where(t => t.ParentID == taskId)
                                 .Where(t => t.TaskCategoryID == cateId)
                               .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => !t.AuditID.HasValue)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")",
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.TaskCategoryID == cateId) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreeAuditGoodTaskByCateID(int taskId, int? cateId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                         .Where(t => t.TaskCategoryID == cateId)
                        .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => t.AuditID.HasValue)
                        .Where(t => t.Audit.IsPass)
                      .Where(t => t.TaskCategoryID == cateId || t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.TaskCategoryID == cateId) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                           .GetQuery()
                           .Where(t => t.ParentID == taskId)
                            .Where(t => t.TaskCategoryID == cateId)
                             .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => t.AuditID.HasValue)
                        .Where(t => t.Audit.IsPass)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")",
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.TaskCategoryID == cateId) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<TaskItem> GetTreeAuditFailTaskByCateID(int taskId, int? cateId, DateTime fromDate, DateTime toDate)
        {
            var dbo = tasksDA.Repository;
            var result = new List<TaskItem>();
            if (taskId == 0)
            {
                result = tasksDA
                      .GetQuery()
                      .Where(t => t.TaskCategoryID == cateId)
                     .Where(t => !t.IsNew)
                        .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                        .Where(t => t.AuditID.HasValue)
                        .Where(t => !t.Audit.IsPass)
                      .Where(t => t.TaskCategoryID == cateId || t.ParentID == 0)
                      .Select(item => new TaskItem()
                      {
                          ID = item.ID,
                          ParentID = item.ParentID,
                          DepartmentID = item.HumanDepartmentID,
                          Name = item.Name,
                          CategoryID = item.TaskCategoryID,
                          CategoryName = item.TaskCategory.Name,
                          StartTime = item.StartTime,
                          CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                          EndTime = item.EndTime,
                          LevelID = item.TaskLevelID,
                          LevelName = item.TaskLevel.Name,
                          LevelColor = item.TaskLevel.Color,
                          IsNew = item.IsNew,
                          IsEdit = item.IsEdit,
                          IsComplete = item.IsComplete,
                          IsPass = item.IsPass,
                          IsPause = item.IsPause,
                          Rate = item.Rate,
                          Cost = item.Cost,
                          Content = item.Content,
                          Note = item.Note,
                          PerformBy = item.HumanEmployeeID,
                          CreateAt = item.CreateAt,
                          CreateBy = item.CreateBy,
                          UpdateAt = item.UpdateAt,
                          UpdateBy = item.UpdateBy,
                          Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.TaskCategoryID == cateId) == null
                      })
                       .OrderByDescending(p => p.CreateAt)
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
                result = taskremove;
            }
            else
            {
                result = tasksDA
                           .GetQuery()
                           .Where(t => t.ParentID == taskId)
                           .Where(t => t.TaskCategoryID == cateId)
                            .Where(t => !t.IsNew)
                            .Where(t => (t.StartTime >= fromDate && t.StartTime <= toDate) || (t.EndTime >= fromDate && t.EndTime <= toDate) || (t.StartTime <= fromDate && t.EndTime >= toDate))
                            .Where(t => t.AuditID.HasValue)
                            .Where(t => !t.Audit.IsPass)
                           .Distinct()
                           .Select(item => new TaskItem()
                           {
                               ID = item.ID,
                               ParentID = item.ParentID,
                               DepartmentID = item.HumanDepartmentID,
                               Name = item.Name + " - (Thuộc: " + dbo.HumanDepartments.FirstOrDefault(t => t.ID == item.HumanDepartmentID).Name + ")",
                               CategoryID = item.TaskCategoryID,
                               CategoryName = item.TaskCategory.Name,
                               StartTime = item.StartTime,
                               CompleteTime = item.CompleteTime != null ? item.CompleteTime.Value : item.EndTime,
                               EndTime = item.EndTime,
                               LevelID = item.TaskLevelID,
                               LevelName = item.TaskLevel.Name,
                               LevelColor = item.TaskLevel.Color,
                               IsNew = item.IsNew,
                               IsEdit = item.IsEdit,
                               IsComplete = item.IsComplete,
                               IsPass = item.IsPass,
                               IsPause = item.IsPause,
                               Rate = item.Rate,
                               Cost = item.Cost,
                               Content = item.Content,
                               Note = item.Note,
                               PerformBy = item.HumanEmployeeID,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy,
                               Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == item.ID && i.TaskCategoryID == cateId) == null
                           })
                           .OrderByDescending(p => p.CreateAt)
                           .ToList();
            }
            foreach (var item in result)
            {
                item.TotalTime = taskCalendarSV.TimeCalculator(item.StartTime, item.EndTime);
            }
            return result;
        }
        public List<HumanEmployeeItem> GetEmployeeCC(ModelFilter filter, int taskId)
        {
            var dbo = tasksDA.Repository;
            var data = dbo.TaskCCs.Where(t => t.TaskID == taskId)
                        .Select(item => new HumanEmployeeItem()
                        {
                            ID = item.ID,
                            FileID = item.HumanEmployee.AttachmentFileID,
                            FileName = item.HumanEmployee.AttachmentFile.Name,
                            Role = item.HumanEmployee.HumanOrganizations.Where(i => i.HumanEmployeeID == item.HumanEmployeeID).Select(i => i.HumanRole.Name).FirstOrDefault(),
                            DepartmentName = item.HumanEmployee.HumanOrganizations.Where(i => i.HumanEmployeeID == item.HumanEmployeeID)
                                                    .Select(i => i.HumanRole.HumanDepartment.Name).FirstOrDefault(),
                            Code = item.HumanEmployee.Code,
                            Name = item.HumanEmployee.Name,
                            Email = item.HumanEmployee.Email,
                            Phone = item.HumanEmployee.Phone,
                            Address = item.HumanEmployee.Address,
                            Birthday = item.HumanEmployee.Birthday,
                            Gender = item.HumanEmployee.Gender,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            lstHumanGrPos = item.HumanEmployee.HumanOrganizations.Where(i => i.HumanEmployeeID == item.HumanEmployeeID
                                && i.HumanRole.IsActive && !i.HumanRole.IsDestroy && !i.HumanRole.IsDelete
                                ).Select(i => new HumanGroupPositionItem()
                                {
                                    ID = i.ID,
                                    Name = i.HumanRole.Name,
                                    GroupName = i.HumanRole.HumanDepartment.Name
                                }).ToList()
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            return data;
        }
        public void AddEmployeeCC(int taskId, int employeeId, int userId)
        {
            var dbo = tasksDA.Repository;
            var obj = dbo.TaskCCs.Where(t => t.TaskID == taskId && t.HumanEmployeeID == employeeId).FirstOrDefault();
            if (obj == null && employeeId != 0)
            {
                var cc = new TaskCC();
                cc.HumanEmployeeID = employeeId;
                cc.TaskID = taskId;
                cc.CreateAt = DateTime.Now;
                cc.CreateBy = userId;
                dbo.TaskCCs.Add(cc);
                dbo.SaveChanges();
            }
        }
        public void DeleteCC(string stringId)
        {
            var dbo = tasksDA.Repository;
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            if (ids.Count > 0)
                foreach (var item in ids)
                {
                    dbo.TaskCCs.Remove(dbo.TaskCCs.Where(t => t.ID == (int)item).FirstOrDefault());
                    dbo.SaveChanges();
                }

        }
    }
}
