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
    public class TaskCheckSV
    {
        private TaskCheckDA checksDA = new TaskCheckDA();
        private TaskSV tasksService = new TaskSV();
        private TaskPersonalSV personService = new TaskPersonalSV();
        private TaskDA tasksDA = new TaskDA();
        public List<TaskCheckItem> GetByTaskId(int page, int pageSize, out int totalCount, int taskId)
        {
            var dbo = checksDA.Repository;
            var checks = checksDA.GetQuery(t => t.TaskID == taskId)
                        .Select(item => new TaskCheckItem()
                        {
                            ID = item.ID,
                            CheckBy = item.EmployeeID,
                            IsPass = item.IsPass,
                            Content = item.Content,
                            Time = item.Time,
                            Rate = item.Rate,
                            Feedback = item.Feedback,
                            UpdateAt = item.UpdateAt,
                            CreateAt = item.CreateAt,
                            UpdateBy = item.UpdateBy,
                            CheckByName = item.EmployeeID != null ? dbo.HumanEmployees.FirstOrDefault(t => t.ID == item.EmployeeID).Name : string.Empty,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return checks;
        }
        public int Insert(TaskCheckItem obj, int userId, int employeeId)
        {
            var dbo = checksDA.Repository;
            var check = new TaskCheck();
            check.CreateAt = DateTime.Now;
            check.CreateBy = userId;
            check.Content = obj.Content.Trim();
            check.EmployeeID = dbo.HumanUsers.FirstOrDefault(u => u.ID == userId).HumanEmployee.ID;
            check.Rate = (decimal)obj.Rate;
            check.Feedback = obj.Feedback;
            check.TaskID = obj.TaskID;
            check.Time = obj.Time;
            check.IsPass = obj.IsComplete;
            personService.UpdateIsNew(dbo, obj.TaskID, employeeId);                  
            checksDA.Insert(check);
            var perform = dbo.TaskPerforms.Where(t => !t.IsCheck && t.TaskID==obj.TaskID).ToList();
            if (perform.Count > 0)
                foreach (var item in perform)
                {
                    item.IsCheck = true;
                }
            checksDA.Save();
            return check.ID;
        }
        public TaskCheckItem GetByID(int id, int idchecks)
        {
            var dbo = tasksDA.Repository;
            var employeeSV = new HumanEmployeeSV();
            var item = tasksDA.GetById(id);
            var itemperform = checksDA.GetById(idchecks);
            var check = new TaskCheckItem();
            check.ID = itemperform.ID;
            check.Feedback = itemperform.Feedback;
            check.Checker = employeeSV.GetEmployeeView(dbo.HumanUsers.FirstOrDefault(t => t.ID == item.CreateBy).HumanEmployeeID);            
            check.Content = itemperform.Content;
            check.Rate = itemperform.Rate;           
            check.Time = itemperform.Time;
            check.IsComplete = itemperform.IsPass;
            check.CreateByName = item.CreateBy != null ? dbo.HumanUsers.FirstOrDefault(u => u.ID == item.CreateBy).HumanEmployee.Name : null;
            check.UpdateByName = item.UpdateBy.HasValue ? dbo.HumanUsers.FirstOrDefault(u => u.ID == item.UpdateBy).HumanEmployee.Name : null;
            return check;
        }
        public bool GetIsFinish(int taskId)
        {
            var data = checksDA.GetQuery(t => t.IsPass && t.TaskID == taskId).FirstOrDefault();
            if (data != null)
                return true;
            return false;
        }
    }
}
