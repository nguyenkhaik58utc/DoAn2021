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
    public class TaskPersonalSV
    {
        private TaskDA taskDA = new TaskDA();
        private TaskPersonalDA personDA = new TaskPersonalDA();
        public void UpdateIsNew(iDASBusinessEntities dbo, int taskId, int employeeId)
        {
            var rs = dbo.TaskPersonals.FirstOrDefault(t => t.TaskID == taskId && t.PerformBy == employeeId);
            if (rs != null)
                rs.IsNew = false;
        }
        public List<TaskPersonalItem> GetByEmployeeId(ModelFilter filter, int employeeId)
        {
            var dbo = taskDA.Repository;
            List<TaskPersonalItem> lst = new List<TaskPersonalItem>();
            lst = dbo.Tasks.Where(t => !t.IsPrivate)
                .Join(dbo.TaskPersonals.Where(x => x.IsCreate || x.IsPerform).Where(x => x.PerformBy == employeeId), t => t.ID, tp => tp.TaskID, (t, tp) => new TaskPersonalItem()
                     {
                         ID = tp.ID,
                         TaskID = tp.TaskID,
                         EmployeeID = tp.PerformBy,
                         IsNew = tp.IsNew,
                         IsCreate = tp.IsCreate,
                         IsPerform = tp.IsPerform,
                         CreateAt = tp.CreateAt,
                         CreateBy = tp.CreateBy,
                         UpdateAt = tp.UpdateAt,
                         UpdateBy = tp.UpdateBy,
                         Name = t.Name,
                         TaskInfo = new TaskItem()
                         {
                             ID = t.ID,
                             DepartmentID = t.HumanDepartmentID,
                             Name = t.Name,
                             CategoryID = t.TaskCategoryID,
                             StartTime = t.StartTime,
                             EndTime = t.EndTime,
                             CompleteTime = t.CompleteTime.Value,
                             LevelID = t.TaskLevelID,
                             CategoryName = t.TaskCategory.Name,
                             LevelName = t.TaskLevel.Name,
                             LevelColor = t.TaskLevel.Color,
                             IsNew = t.IsNew,
                             IsEdit = t.IsEdit,
                             IsComplete = t.IsComplete,
                             IsPrivate = t.IsPrivate,
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
                         }
                     })
             .OrderByDescending(t => t.TaskInfo.CreateAt)
             .Filter(filter)
             .ToList();
            foreach (var item in lst)
            {
                item.TaskInfo.IsRequireCheck = dbo.TaskPerforms.Where(c => c.TaskID == item.TaskInfo.ID && !c.IsCheck).FirstOrDefault() != null;
                item.TaskInfo.CreateAt = dbo.TaskPerforms.Where(c => c.TaskID == item.TaskInfo.ID).FirstOrDefault() == null ? item.TaskInfo.CreateAt : dbo.TaskPerforms.Where(c => c.TaskID == item.TaskInfo.ID).OrderByDescending(c => c.CreateAt).FirstOrDefault().CreateAt;
            }
            lst.OrderBy(t => t.TaskInfo.CreateAt);
            return lst;
        }
        public List<TaskPersonalItem> GetTaskNewAssign(int page, int pageSize, out int totalCount, int employeeId)
        {
            var dbo = taskDA.Repository;
            List<TaskPersonalItem> lst = new List<TaskPersonalItem>();
            lst = dbo.Tasks.Where(t => !t.IsPrivate).Join(dbo.TaskPersonals, t => t.ID, tp => tp.TaskID, (t, tp) => new TaskPersonalItem()
            {
                ID = tp.ID,
                TaskID = tp.TaskID,
                EmployeeID = tp.PerformBy,
                IsNew = tp.IsNew,
                IsEdit = t.IsEdit,
                IsCreate = tp.IsCreate,
                IsComplete = t.IsComplete,
                Rate = t.Rate,
                IsPerform = tp.IsPerform,
                CreateAt = tp.CreateAt,
                CreateBy = tp.CreateBy,
                UpdateAt = tp.UpdateAt,
                UpdateBy = tp.UpdateBy,
                TaskInfo = new TaskItem()
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
                    IsNew = t.IsNew,
                    IsEdit = t.IsEdit,
                    IsComplete = t.IsComplete,
                    EmployeesIDCreate = dbo.HumanEmployees.FirstOrDefault(u => u.HumanUsers.FirstOrDefault().ID == t.CreateBy).ID,
                    DepartmentCreateName = dbo.HumanEmployees.FirstOrDefault(u => u.HumanUsers.FirstOrDefault().ID == t.CreateBy) != null ? dbo.HumanEmployees.FirstOrDefault(u => u.HumanUsers.FirstOrDefault().ID == t.CreateBy).HumanOrganizations.FirstOrDefault().HumanRole.HumanDepartment.Name : string.Empty,
                    AvatarCreateName = dbo.HumanEmployees.FirstOrDefault(u => u.HumanUsers.FirstOrDefault().ID == t.CreateBy) != null ? dbo.HumanEmployees.FirstOrDefault(u => u.HumanUsers.FirstOrDefault().ID == t.CreateBy).Avatar : string.Empty,
                    RoleCreateName = dbo.HumanEmployees.FirstOrDefault(u => u.HumanUsers.FirstOrDefault().ID == t.CreateBy) != null ? dbo.HumanEmployees.FirstOrDefault(u => u.HumanUsers.FirstOrDefault().ID == t.CreateBy).HumanOrganizations.FirstOrDefault().HumanRole.Name : string.Empty,
                    CreateByName = dbo.HumanEmployees.FirstOrDefault(u => u.HumanUsers.FirstOrDefault().ID == t.CreateBy) != null ? dbo.HumanEmployees.FirstOrDefault(u => u.HumanUsers.FirstOrDefault().ID == t.CreateBy).Name : string.Empty,
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
                }
            })
             .Where(i => i.IsPerform)
             .Where(i => i.EmployeeID == employeeId && i.IsNew)
             .OrderByDescending(t => t.CreateAt)
             .Page(page, pageSize, out totalCount)
             .ToList();
            return lst;
        }
        public List<TaskPersonalItem> GetTaskDoing(int page, int pageSize, out int totalCount, int employeeId)
        {
            var dbo = taskDA.Repository;
            List<TaskPersonalItem> lst = new List<TaskPersonalItem>();
            lst = dbo.Tasks.Where(t => !t.IsPrivate).Join(dbo.TaskPersonals, t => t.ID, tp => tp.TaskID, (t, tp) => new TaskPersonalItem()
            {
                ID = tp.ID,
                TaskID = tp.TaskID,
                EmployeeID = tp.PerformBy,
                IsNew = tp.IsNew,
                IsCreate = tp.IsCreate,
                IsComplete = t.IsComplete,
                Rate = t.Rate,
                IsPerform = tp.IsPerform,
                CreateAt = tp.CreateAt,
                CreateBy = tp.CreateBy,
                UpdateAt = tp.UpdateAt,
                UpdateBy = tp.UpdateBy,
                TaskInfo = new TaskItem()
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
                    IsNew = t.IsNew,
                    IsEdit = t.IsEdit,
                    IsComplete = t.IsComplete,
                    EmployeesIDCreate = dbo.HumanEmployees.FirstOrDefault(u => u.HumanUsers.FirstOrDefault().ID == t.CreateBy).ID,
                    DepartmentCreateName = dbo.HumanEmployees.FirstOrDefault(u => u.HumanUsers.FirstOrDefault().ID == t.CreateBy) != null ? dbo.HumanEmployees.FirstOrDefault(u => u.HumanUsers.FirstOrDefault().ID == t.CreateBy).HumanOrganizations.FirstOrDefault().HumanRole.HumanDepartment.Name : string.Empty,
                    AvatarCreateName = dbo.HumanEmployees.FirstOrDefault(u => u.HumanUsers.FirstOrDefault().ID == t.CreateBy) != null ? dbo.HumanEmployees.FirstOrDefault(u => u.HumanUsers.FirstOrDefault().ID == t.CreateBy).Avatar : string.Empty,
                    RoleCreateName = dbo.HumanEmployees.FirstOrDefault(u => u.HumanUsers.FirstOrDefault().ID == t.CreateBy) != null ? dbo.HumanEmployees.FirstOrDefault(u => u.HumanUsers.FirstOrDefault().ID == t.CreateBy).HumanOrganizations.FirstOrDefault().HumanRole.Name : string.Empty,
                    CreateByName = dbo.HumanEmployees.FirstOrDefault(u => u.HumanUsers.FirstOrDefault().ID == t.CreateBy) != null ? dbo.HumanEmployees.FirstOrDefault(u => u.HumanUsers.FirstOrDefault().ID == t.CreateBy).Name : string.Empty,
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
                }
            })
             .Where(i => i.IsPerform)
             .Where(i => i.EmployeeID == employeeId && !i.IsNew)
             .Where(i => !i.IsComplete && i.Rate != 100)
             .OrderByDescending(t => t.CreateAt)
             .Page(page, pageSize, out totalCount)
             .ToList();
            return lst;
        }
        public void Update(TaskItem objNew, int employeeID, int userId)
        {
            var TaskPersonals = personDA.GetQuery().Where(i => i.TaskID == objNew.ID).ToList();
            foreach (var TaskPersonal in TaskPersonals)
            {
                TaskPersonal.IsPerform = objNew.PerformBy == employeeID ? true : false;
                TaskPersonal.UpdateAt = DateTime.Now;
                TaskPersonal.UpdateBy = userId;
            }
            personDA.Save();
            Insert(objNew, employeeID, userId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objNew"></param>
        /// <param name="employeeID"></param>
        /// <param name="userId"></param>
        public void Insert(TaskItem objNew, int employeeID, int userId)
        {
            int taskID = objNew.ID;
            int _employeeID;
            _employeeID = employeeID;//EmployeeID đăng nhập
            objNew.CreateBy = userId;// User Đăng nhập
            update(objNew, _employeeID, userId);
            if (!objNew.IsEdit)
            {
                //case perform
                if (objNew.PerformBy == 0 || objNew.PerformBy == null)
                {
                    _employeeID = employeeID;
                }
                else
                {
                    _employeeID = objNew.PerformBy;

                }
                update(objNew, _employeeID, userId);

            }

        }
        private void update(TaskRequestItem objNew, int employeeID, int userId)
        {
            TaskPersonal TaskPersonal;
            TaskPersonal = getTaskPersonal(objNew.ID, employeeID);
            if (TaskPersonal == null)
            {
                insert(objNew, employeeID, userId);
            }
            else
            {
                updatetask(objNew, employeeID, userId);
            }
        }
        private void insert(TaskRequestItem objNew, int employeeId, int userId)
        {
            var dbo = personDA.Repository;
            TaskPersonal TaskPersonal = new TaskPersonal()
            {
                TaskID = objNew.ID,
                PerformBy = employeeId,
                IsPerform = objNew.HumanEmployeeID == employeeId ? true : false,
                IsCreate = dbo.HumanUsers.FirstOrDefault(t => t.ID == objNew.CreateBy).HumanEmployee.ID == employeeId ? true : false,
                IsNew = dbo.HumanUsers.FirstOrDefault(t => t.ID == objNew.CreateBy).HumanEmployee.ID == employeeId ? false : true,
                CreateAt = DateTime.Now,
                CreateBy = userId

            };
            personDA.Insert(TaskPersonal);
            personDA.Save();
        }
        private void updatetask(TaskRequestItem objNew, int employeeId, int userId)
        {
            TaskPersonal TaskPersonal;
            TaskPersonal = getTaskPersonal(objNew.ID, employeeId);
            TaskPersonal.IsPerform = objNew.HumanEmployeeID == employeeId ? true : false;
            TaskPersonal.IsCreate = objNew.CreateBy == userId ? true : false;
            TaskPersonal.UpdateAt = DateTime.Now;
            TaskPersonal.UpdateBy = userId;
            personDA.Save();
        }
        public void Insert(TaskRequestItem objNew, int employeeID, int userId)
        {
            int taskID = objNew.ID;
            int _employeeID;
            _employeeID = employeeID;//EmployeeID đăng nhập
            objNew.CreateBy = userId;// User Đăng nhập
            update(objNew, _employeeID, userId);
            if (!objNew.IsEdit)
            {
                //case perform
                if (objNew.HumanEmployeeID == 0 || objNew.HumanEmployeeID == null)
                {
                    _employeeID = employeeID;
                }
                else
                {
                    _employeeID = objNew.HumanEmployeeID;

                }
                update(objNew, _employeeID, userId);

            }

        }
        private void update(TaskItem objNew, int employeeID, int userId)
        {
            TaskPersonal TaskPersonal;
            TaskPersonal = getTaskPersonal(objNew.ID, employeeID);
            if (TaskPersonal == null)
            {
                insert(objNew, employeeID, userId);
            }
            else
            {
                updatetask(objNew, employeeID, userId);
            }
        }
        private void updatetask(TaskItem objNew, int employeeId, int userId)
        {
            TaskPersonal TaskPersonal;
            TaskPersonal = getTaskPersonal(objNew.ID, employeeId);
            TaskPersonal.IsPerform = objNew.PerformBy == employeeId ? true : false;
            TaskPersonal.IsCreate = objNew.CreateBy == userId ? true : false;
            TaskPersonal.UpdateAt = DateTime.Now;
            TaskPersonal.UpdateBy = userId;
            personDA.Save();
        }
        private void insert(TaskItem objNew, int employeeId, int userId)
        {
            var dbo = personDA.Repository;
            TaskPersonal TaskPersonal = new TaskPersonal()
            {
                TaskID = objNew.ID,
                PerformBy = employeeId,
                IsPerform = objNew.PerformBy == employeeId ? true : false,
                IsCreate = dbo.HumanUsers.FirstOrDefault(t => t.ID == objNew.CreateBy).HumanEmployee.ID == employeeId ? true : false,
                IsNew = dbo.HumanUsers.FirstOrDefault(t => t.ID == objNew.CreateBy).HumanEmployee.ID == employeeId ? false : true,
                CreateAt = DateTime.Now,
                CreateBy = userId

            };
            personDA.Insert(TaskPersonal);
            personDA.Save();
        }
        private TaskPersonal getTaskPersonal(int taskID, int employeeID)
        {
            var TaskPersonal = personDA.GetQuery().Where(i => i.TaskID == taskID && i.PerformBy == employeeID).FirstOrDefault();
            return TaskPersonal;
        }
        public List<TaskPersonalItem> GetTreeTask(int taskId, int employeeId, int focusId = 0, int cateId=0, Nullable<DateTime> fromdate=null, Nullable<DateTime> todate=null, string choice="")
        {
            var dbo = taskDA.Repository;
            var fromDateForQr = System.Convert.ToDateTime(fromdate);
            var toDateQr = System.Convert.ToDateTime(todate).AddDays(1);
            List<TaskPersonalItem> lst = new List<TaskPersonalItem>();
            if (taskId == 0)
            {
                IQueryable<iDAS.Base.Task> query;
                if (focusId != 0)
                {
                    query = dbo.Tasks.Where(t => t.ID == focusId);
                }
                else
                {
                    query = dbo.Tasks
                        .Where(t => !t.IsPrivate).Where(t=>cateId==0 || t.TaskCategoryID==cateId)
                        .Where(t => (choice.Equals("App.Time") && (t.StartTime >= fromDateForQr && t.StartTime <= toDateQr) || (t.EndTime >= fromDateForQr && t.EndTime <= toDateQr) || (t.StartTime <= fromDateForQr && t.EndTime >= toDateQr)) || choice.Equals("App.All"));
                }
                lst = query
                    .Join(dbo.TaskPersonals.Where(x => x.IsCreate || x.IsPerform)
                    .Where(x => x.PerformBy == employeeId), t => t.ID, tp => tp.TaskID, (t, tp) => new TaskPersonalItem()
                    {
                        ID = tp.ID,
                        TaskID = tp.TaskID,
                        EmployeeID = tp.PerformBy,
                        IsNew = tp.IsNew,
                        IsCreate = tp.IsCreate,
                        IsPerform = tp.IsPerform,
                        CreateAt = tp.CreateAt,
                        CreateBy = tp.CreateBy,
                        UpdateAt = tp.UpdateAt,
                        UpdateBy = tp.UpdateBy,
                        Name = t.Name,
                        Parent = t.ParentID,
                        Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == t.ID) == null,
                        TaskInfo = new TaskItem()
                        {
                            ID = t.ID,
                            DepartmentID = t.HumanDepartmentID,
                            Name = t.Name,
                            CategoryID = t.TaskCategoryID,
                            StartTime = t.StartTime,
                            EndTime = t.EndTime,
                            CompleteTime = t.CompleteTime.Value,
                            LevelID = t.TaskLevelID,
                            CategoryName = t.TaskCategory.Name,
                            LevelName = t.TaskLevel.Name,
                            LevelColor = t.TaskLevel.Color,
                            IsRequireCheck = dbo.TaskPerforms.Where(c => c.TaskID == t.ID && !c.IsCheck).FirstOrDefault() != null,
                            IsNew = t.IsNew,
                            IsEdit = t.IsEdit,
                            IsComplete = t.IsComplete,
                            IsPrivate = t.IsPrivate,
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
                        }
                    })
                    .Distinct()
                    .OrderByDescending(t => t.ID)
                    .ToList();
                var lis = lst.Select(t => t.TaskID).ToArray();
                List<TaskPersonalItem> taskremove = new List<TaskPersonalItem>();
                foreach (var item in lst)
                {
                    if (!lis.Contains(item.Parent))
                    {
                        taskremove.Add(item);
                    }
                }
                lst = taskremove;
            }
            else
            {
                lst = dbo.Tasks.Where(t => !t.IsPrivate).Where(t=>cateId==0 || t.TaskCategoryID==cateId)
                    .Join(dbo.TaskPersonals.Where(x => x.IsCreate || x.IsPerform)
                    .Where(x => x.PerformBy == employeeId), t => t.ID, tp => tp.TaskID, (t, tp) => new TaskPersonalItem()
                    {
                        ID = tp.ID,
                        TaskID = tp.TaskID,
                        EmployeeID = tp.PerformBy,
                        IsNew = tp.IsNew,
                        IsCreate = tp.IsCreate,
                        IsPerform = tp.IsPerform,
                        CreateAt = tp.CreateAt,
                        CreateBy = tp.CreateBy,
                        UpdateAt = tp.UpdateAt,
                        UpdateBy = tp.UpdateBy,
                        Name = t.Name,
                        Parent = t.ParentID,
                        Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == t.ID) == null,
                        TaskInfo = new TaskItem()
                        {
                            ID = t.ID,
                            DepartmentID = t.HumanDepartmentID,
                            Name = t.Name,
                            IsRequireCheck = dbo.TaskPerforms.Where(c => c.TaskID == t.ID && !c.IsCheck).FirstOrDefault() != null,
                            CategoryID = t.TaskCategoryID,
                            StartTime = t.StartTime,
                            EndTime = t.EndTime,
                            CompleteTime = t.CompleteTime.Value,
                            LevelID = t.TaskLevelID,
                            CategoryName = t.TaskCategory.Name,
                            LevelName = t.TaskLevel.Name,
                            LevelColor = t.TaskLevel.Color,
                            IsNew = t.IsNew,
                            IsEdit = t.IsEdit,
                            IsComplete = t.IsComplete,
                            IsPrivate = t.IsPrivate,
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
                        }
                    })
                 .Where(t => t.Parent == taskId)
                 .OrderByDescending(t => t.ID)
                 .ToList();
            }
            foreach (var item in lst)
            {
                item.TaskInfo.IsRequireCheck =
                    dbo.TaskPerforms.Where(c => c.TaskID == item.TaskInfo.ID && !c.IsCheck).FirstOrDefault() != null;
                item.TaskInfo.CreateAt =
                    dbo.TaskPerforms.Where(c => c.TaskID == item.TaskInfo.ID).FirstOrDefault() == null
                    ? item.TaskInfo.CreateAt :
                    dbo.TaskPerforms.Where(c => c.TaskID == item.TaskInfo.ID)
                    .OrderByDescending(c => c.CreateAt)
                    .FirstOrDefault().CreateAt;
            }
            return lst;

        }
        public List<TaskPersonalItem> GetTreeTaskCC(int taskId, int employeeId, int focusId = 0, int cateId = 0, Nullable<DateTime> fromdate = null, Nullable<DateTime> todate = null, string choice = "")
        {
            var dbo = taskDA.Repository;
            var fromDateForQr = System.Convert.ToDateTime(fromdate);
            var toDateQr = System.Convert.ToDateTime(todate).AddDays(1);
            List<TaskPersonalItem> lst = new List<TaskPersonalItem>();
            if (taskId == 0)
            {
                IQueryable<iDAS.Base.Task> query;
                if (focusId != 0)
                {
                    query = dbo.Tasks.Where(t => t.ID == focusId);
                }
                else
                {
                    query = dbo.Tasks
                        .Where(t => !t.IsPrivate).Where(t => cateId == 0 || t.TaskCategoryID == cateId)
                        .Where(t => (choice.Equals("App.Time") && (t.StartTime >= fromDateForQr && t.StartTime <= toDateQr) || (t.EndTime >= fromDateForQr && t.EndTime <= toDateQr) || (t.StartTime <= fromDateForQr && t.EndTime >= toDateQr)) || choice.Equals("App.All"));
                }
                lst = query
                    .Join(dbo.TaskCCs
                    .Where(x => x.HumanEmployeeID == employeeId), t => t.ID, tp => tp.TaskID, (t, tp) => new TaskPersonalItem()
                    {
                        ID = tp.ID,
                        TaskID = tp.TaskID,
                        EmployeeID = tp.HumanEmployeeID,
                        IsNew = t.IsNew,
                        CreateAt = tp.CreateAt,
                        CreateBy = tp.CreateBy,
                        Name = t.Name,
                        Parent = t.ParentID,
                        Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == t.ID) == null,
                        TaskInfo = new TaskItem()
                        {
                            ID = t.ID,
                            DepartmentID = t.HumanDepartmentID,
                            Name = t.Name,
                            CategoryID = t.TaskCategoryID,
                            StartTime = t.StartTime,
                            EndTime = t.EndTime,
                            CompleteTime = t.CompleteTime.Value,
                            LevelID = t.TaskLevelID,
                            CategoryName = t.TaskCategory.Name,
                            LevelName = t.TaskLevel.Name,
                            LevelColor = t.TaskLevel.Color,
                            IsRequireCheck = dbo.TaskPerforms.Where(c => c.TaskID == t.ID && !c.IsCheck).FirstOrDefault() != null,
                            IsNew = t.IsNew,
                            IsEdit = t.IsEdit,
                            IsComplete = t.IsComplete,
                            IsPrivate = t.IsPrivate,
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
                        }
                    })
                    .Distinct()
                    .OrderByDescending(t => t.ID)
                    .ToList();
                var lis = lst.Select(t => t.TaskID).ToArray();
                List<TaskPersonalItem> taskremove = new List<TaskPersonalItem>();
                foreach (var item in lst)
                {
                    if (!lis.Contains(item.Parent))
                    {
                        taskremove.Add(item);
                    }
                }
                lst = taskremove;
            }
            else
            {
                lst = dbo.Tasks.Where(t => !t.IsPrivate).Where(t => cateId == 0 || t.TaskCategoryID == cateId)
                   .Join(dbo.TaskCCs
                    .Where(x => x.HumanEmployeeID == employeeId), t => t.ID, tp => tp.TaskID, (t, tp) => new TaskPersonalItem()
                    {
                        ID = tp.ID,
                        TaskID = tp.TaskID,
                        EmployeeID = tp.HumanEmployeeID,
                        IsNew = t.IsNew,
                        CreateAt = tp.CreateAt,
                        CreateBy = tp.CreateBy,
                        Name = t.Name,
                        Parent = t.ParentID,
                        Leaf = dbo.Tasks.FirstOrDefault(i => i.ParentID == t.ID) == null,
                        TaskInfo = new TaskItem()
                        {
                            ID = t.ID,
                            DepartmentID = t.HumanDepartmentID,
                            Name = t.Name,
                            IsRequireCheck = dbo.TaskPerforms.Where(c => c.TaskID == t.ID && !c.IsCheck).FirstOrDefault() != null,
                            CategoryID = t.TaskCategoryID,
                            StartTime = t.StartTime,
                            EndTime = t.EndTime,
                            CompleteTime = t.CompleteTime.Value,
                            LevelID = t.TaskLevelID,
                            CategoryName = t.TaskCategory.Name,
                            LevelName = t.TaskLevel.Name,
                            LevelColor = t.TaskLevel.Color,
                            IsNew = t.IsNew,
                            IsEdit = t.IsEdit,
                            IsComplete = t.IsComplete,
                            IsPrivate = t.IsPrivate,
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
                        }
                    })
                 .Where(t => t.Parent == taskId)
                 .OrderByDescending(t => t.ID)
                 .ToList();
            }
            foreach (var item in lst)
            {
                item.TaskInfo.IsRequireCheck =
                    dbo.TaskPerforms.Where(c => c.TaskID == item.TaskInfo.ID && !c.IsCheck).FirstOrDefault() != null;
                item.TaskInfo.CreateAt =
                    dbo.TaskPerforms.Where(c => c.TaskID == item.TaskInfo.ID).FirstOrDefault() == null
                    ? item.TaskInfo.CreateAt :
                    dbo.TaskPerforms.Where(c => c.TaskID == item.TaskInfo.ID)
                    .OrderByDescending(c => c.CreateAt)
                    .FirstOrDefault().CreateAt;
            }
            return lst;

        }
    }
}
