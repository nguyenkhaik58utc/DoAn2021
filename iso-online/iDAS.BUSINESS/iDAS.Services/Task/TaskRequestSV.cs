using iDAS.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Items;
using iDAS.Base;

namespace iDAS.Services
{
    public class TaskRequestSV
    {
        private TaskRequestDA taskRequestDA = new TaskRequestDA();
        public List<TaskRequestItem> GetAll(ModelFilter filter, int focusId = 0, int employeeId = 0)
        {
            List<TaskRequestItem> lst = new List<TaskRequestItem>();
            var dbo = taskRequestDA.Repository;
            IQueryable<iDAS.Base.TaskRequest> query = dbo.TaskRequests
                .Where(x=>x.HumanEmployeeID==employeeId||x.HumanEmployee.HumanUsers.FirstOrDefault(t=>t.ID==x.CreateBy.Value).HumanEmployeeID==employeeId||x.ApprovalBy==employeeId)
                .Where(x=>x.HumanEmployeeID==employeeId || !x.IsNew);
            if (focusId != 0)
            {
                filter.SortName = null;
                query = query.OrderBy(i => i.ID == focusId ? false : true);
            }
            var request = query
                .Filter(filter)
                .ToList();
            if (request.Count > 0)
            {
                foreach (var item in request)
                {
                    lst.Add(new TaskRequestItem
                    {
                        ID = item.ID,
                        ParentID = item.ParentID,
                        HumanDepartmentID = item.HumanDepartmentID,
                        Name = item.Name,
                        TaskCategoryID = item.TaskCategoryID,
                        CategoryName = dbo.TaskCategories.FirstOrDefault(t=>t.ID==item.TaskCategoryID).Name,
                        StartTime = item.StartTime,
                        EndTime = item.EndTime,
                        TaskLevelID = item.TaskLevelID,
                        LevelName = item.TaskLevel.Name,
                        LevelColor = item.TaskLevel.Color,
                        IsNew = item.IsNew,
                        IsEdit = item.IsEdit,
                        IsAccept = item.IsAccept,
                        IsApproval = item.IsApproval,
                        ApprovalNote = item.Note,
                        Cost = item.Cost,
                        Note = item.Note,
                        CreateAt = item.CreateAt,
                        CreateBy = item.CreateBy,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy,
                    });
                }
            }
            return lst;
        }
        public TaskRequestItem GetById(int id)
        {
            var dbo = taskRequestDA.Repository;
            TaskRequestItem task = new TaskRequestItem();
            var item = taskRequestDA.GetById(id);
            if (item != null)
            {
                var employeeSV = new HumanEmployeeSV();
                task.ID = item.ID;
                task.ParentID = item.ParentID;
                task.HumanEmployeeID = item.HumanEmployeeID;
                task.HumanDepartmentID = item.HumanDepartmentID;
                task.DepartmentName = dbo.HumanDepartments.FirstOrDefault(i => i.ID == item.HumanDepartmentID).Name;
                task.Department = new HumanDepartmentViewItem()
                {
                    ID = item.HumanDepartmentID,
                    Name = dbo.HumanDepartments.FirstOrDefault(d => d.ID == item.HumanDepartmentID).Name
                };
                task.Cost = item.Cost;
                task.Contents = item.Contents;
                task.Create = employeeSV.GetEmployeeView(dbo.HumanUsers.FirstOrDefault(t => t.ID == item.CreateBy).HumanEmployeeID);
                task.Perform = employeeSV.GetEmployeeView(item.HumanEmployeeID);
                task.Approval = employeeSV.GetEmployeeView(item.ApprovalBy);
                task.ApprovalBy = item.ApprovalBy;
                task.TaskCategoryID = item.TaskCategoryID;
                task.CategoryName = dbo.TaskCategories.FirstOrDefault(t=>t.ID==item.TaskCategoryID).Name;
                task.Cost = item.Cost;
                task.TaskLevelID = item.TaskLevelID;
                task.IsNew = item.IsNew;
                task.LevelColor = item.TaskLevel.Color;
                task.LevelName = item.TaskLevel.Name;
                task.StartTime = item.StartTime;
                task.EndTime = item.EndTime;
                task.Contents = item.Contents;
                task.Reason = item.Reason;
                task.Name = item.Name;
                task.IsEdit = item.IsEdit;
                task.IsNew = item.IsNew;
                task.IsAccept = item.IsAccept;
                task.IsApproval = item.IsApproval;
                task.IsDelete = item.IsDelete;
                task.Note = item.Note;
                task.ApprovalNote = item.Note;
                task.UpdateAt = item.UpdateAt;
                task.UpdateBy = item.UpdateBy;
                task.CreateAt = item.CreateAt;
                task.CreateBy = item.CreateBy;
            }
            return task;
        }
        public void Approve(TaskRequestItem objEdit, int userId)
        {
            TaskRequest obj = taskRequestDA.GetById(objEdit.ID);
            obj.IsAccept = objEdit.IsAccept;
            obj.IsApproval = true;
            obj.Note = objEdit.ApprovalNote;
            obj.ApprovalAt = DateTime.Now;
            obj.IsEdit = objEdit.IsEdit;
            taskRequestDA.Update(obj);
            taskRequestDA.Save();
        }
        public void SendApprove(TaskRequestItem objEdit, int userId)
        {
            TaskRequest obj = taskRequestDA.GetById(objEdit.ID);
            obj.ApprovalBy = objEdit.Approval.ID;
            obj.IsEdit = false;
            obj.IsApproval = false;
            obj.IsNew = false;
            obj.IsAccept = obj.IsAccept;
            taskRequestDA.Update(obj);
            taskRequestDA.Save();
        }
        public void Insert(TaskRequestItem objNew, int userId)
        {

            TaskRequest obj = new TaskRequest();
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            obj.Name = objNew.Name;
            obj.ApprovalBy = objNew.Approval.ID;
            obj.Contents = objNew.Contents;
            obj.Cost = objNew.Cost;
            obj.HumanDepartmentID = objNew.Department.ID;
            obj.HumanEmployeeID = objNew.Perform.ID;
            obj.Reason = objNew.Reason;
            obj.TaskLevelID = objNew.TaskLevelID;
            obj.TaskCategoryID = objNew.TaskCategoryID;
            obj.StartTime = objNew.StartTime;
            obj.EndTime = objNew.EndTime;
            obj.IsAccept = objNew.IsAccept;
            obj.IsApproval = objNew.IsApproval;
            obj.IsEdit = true;
            obj.IsNew = true;
            obj.Note = objNew.Note;
            obj.ParentID = objNew.ParentID;
            obj.TaskCategoryID = objNew.TaskCategoryID;
            obj.Reason = objNew.Reason;
            taskRequestDA.Insert(obj);
            taskRequestDA.Save();
        }
        public void Delete(string stringId)
        {
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            taskRequestDA.DeleteRange(ids);
            taskRequestDA.Save();
        }
        public void Update(TaskRequestItem objEdit, int userId)
        {
            TaskRequest obj = taskRequestDA.GetById(objEdit.ID);
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            obj.ParentID = objEdit.ParentID;
            obj.Name = objEdit.Name;
            obj.ApprovalBy = objEdit.Approval.ID;
            obj.Contents = objEdit.Contents;
            obj.Cost = objEdit.Cost;
            obj.HumanDepartmentID = objEdit.Department.ID;
            obj.HumanEmployeeID = objEdit.Perform.ID;
            obj.Reason = objEdit.Reason;
            obj.TaskLevelID = objEdit.TaskLevelID;
            obj.TaskCategoryID = objEdit.TaskCategoryID;
            obj.StartTime = objEdit.StartTime;
            obj.EndTime = objEdit.EndTime;
            obj.IsAccept = objEdit.IsAccept;
            obj.IsApproval = objEdit.IsApproval;
            taskRequestDA.Update(obj);
            taskRequestDA.Save();
        }
      
        public bool CheckCodeExits(string name)
        {
            var rs = taskRequestDA.GetQuery(t => t.Name.ToUpper() == name.ToUpper()).ToList();
            if (rs.Count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CheckCodeExitEdits(int id, string name)
        {
            var rs = taskRequestDA.GetQuery().Where(t => t.Name.ToUpper() == name.ToUpper() && t.ID != id).ToList();
            if (rs.Count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
