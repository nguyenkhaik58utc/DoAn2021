using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
    public class TaskCommentSV
    {
        private TaskCommentDA tcDA = new TaskCommentDA();
        public TaskCommentItem GetById(int ID)
        {
            var dbo = tcDA.Repository;
            var rpt = tcDA.GetQuery(i => i.ID == ID)
                       .Select(item => new TaskCommentItem()
                       {
                           ID = item.ID,
                           TaskID = item.TaskID,
                           EmployeeID= item.EmployeeID,
                           Comment = item.Comment,
                           Employee = new HumanEmployeeViewItem()
                           {
                               Name = dbo.HumanEmployees.Any(x=>x.ID==item.EmployeeID)?dbo.HumanEmployees.FirstOrDefault(x=>x.ID==item.EmployeeID).Name : "",
                               ID = item.EmployeeID,
                           },
                           
                       })
                       .First();
            return rpt;
        }

        public void Update(TaskCommentItem item, int p)
        {
            var taskCom = tcDA.GetById(item.ID);
            taskCom.Comment= item.Comment;
            tcDA.Save();
        }

        public void Insert(TaskCommentItem item, int userID)
        {
            var taskCom = new TaskComment()
            {
                EmployeeID = item.EmployeeID,
                TaskID = item.TaskID,
                Comment = item.Comment,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            tcDA.Insert(taskCom);
            tcDA.Save();
        }

        public void Delete(int id)
        {
            tcDA.Delete(id);
            tcDA.Save();
        }

        public List<TaskCommentItem> GetAll(ModelFilter filter, int ID,int UserID)
        {
            var dbo = tcDA.Repository;
            var rs = tcDA.GetQuery(x => x.TaskID == ID).Select(item => new TaskCommentItem()
                {
                    TaskID = item.TaskID,
                    ID= item.ID,
                    EmployeeID = item.EmployeeID,
                    Comment = item.Comment,
                    EmployeeName = dbo.HumanEmployees.FirstOrDefault(x=>x.ID == item.EmployeeID).Name,
                    IsEdit = item.CreateBy.HasValue? item.CreateBy==UserID : false
                }).OrderByDescending(i => i.ID)
                .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
                .ToList();
            return rs;
        }
    }
}
