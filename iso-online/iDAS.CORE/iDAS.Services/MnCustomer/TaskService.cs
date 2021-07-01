using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;
using iDAS.DAL.MnCustomer;
using iDAS.Items.MnCustomer;

namespace iDAS.Services.MnCustomer
{
    public class TaskService
    {
        TaskDA taskDA = new TaskDA();

        public List<TaskItem> GetAll(int campaignId,  int page, int pageSize, out int totalCount)
        {
            var dbo = taskDA.SystemContext;
            var tasks = taskDA.GetQuery()
                            .Where(item=>item.CampaignID == campaignId)
                            .Select(item =>
                                 new TaskItem()
                                 {
                                     ID = item.ID,
                                     CampaignID = campaignId,
                                     Name = item.Name,
                                     StartTime = item.StartTime,
                                     EndTime = item.EndTime,
                                     Expense = item.Expense,
                                     UpdateAt = item.UpdateAt,
                                     UpdateBy = item.UpdateBy,
                                     UpdateByName = dbo.SystemUsers.FirstOrDefault(i => i.ID == item.UpdateBy).FullName,
                                 }
                            )
                            .OrderByDescending(item => item.UpdateAt)
                            .Page(page, pageSize, out totalCount)
                            .ToList();
            return tasks;
        }

        public void Insert(TaskItem item)
        {
            var task = new MnCustomerTask()
            {
                CampaignID = item.CampaignID,
                Name = item.Name,
                StartTime = item.StartTime,
                EndTime = item.EndTime,
                Expense = item.Expense,
                UpdateAt = DateTime.Now,
                UpdateBy = item.UpdateBy,
            };
            taskDA.Insert(task);
            taskDA.Save();
        }

        public void Update(TaskItem item)
        {
            var task = taskDA.GetById(item.ID);
            task.CampaignID = item.CampaignID;
            task.Name = item.Name;
            task.StartTime = item.StartTime;
            task.EndTime = item.EndTime;
            task.Expense = item.Expense;
            task.UpdateAt = DateTime.Now;
            task.UpdateBy = item.UpdateBy;
            taskDA.Save();
        }

        public void Delete(int id)
        {
            taskDA.Delete(id);
            taskDA.Save();
        }
        public void DeleteRange(List<object> ids)
        {
            taskDA.DeleteRange(ids);
            taskDA.Save();
        }
    }
}
