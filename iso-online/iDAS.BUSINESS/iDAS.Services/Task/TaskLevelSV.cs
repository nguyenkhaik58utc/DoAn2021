using System;
using System.Collections.Generic;
using System.Linq;
using iDAS.Core;
using iDAS.Base;
using iDAS.DA;
using iDAS.Items;


namespace iDAS.Services
{
    public class TaskLevelSV
    {
        private TaskLevelDA taskLevelDA=new TaskLevelDA();
        public List<TaskLevelItem> GetAll(int page, int pageSize, out int total)
        {
            var dbo = taskLevelDA.Repository;
            List<TaskLevelItem> lst = new List<TaskLevelItem>();
            var data = taskLevelDA.GetQuery().OrderByDescending(t => t.CreateAt).Page(page, pageSize, out total).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    DateTime? updatetime;
                    string updator;
                    if (item.UpdateBy == null)
                    {
                        updator = dbo.HumanUsers.FirstOrDefault(t => t.ID == item.UpdateBy).HumanEmployee.Name;
                        updatetime = item.CreateAt;
                    }
                    else
                    {
                        updator = null;
                        updatetime = (DateTime)item.UpdateAt;
                    }
                    lst.Add(new TaskLevelItem
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Color = item.Color,
                        Note = item.Note,
                        IsActive = item.IsActive,
                        CreateBy = item.CreateBy,
                        CreateAt = item.CreateAt,
                        UpdateBy = item.UpdateBy,
                        UpdateAt = item.UpdateAt,
                        UpdateByName = updator,
                        UpdateByTime = (DateTime)updatetime
                    });
                }
            }
            return lst;
        }
        public List<TaskLevelItem> GetAllIsUse(int page, int pageSize, out int total)
        {
            var dbo = taskLevelDA.Repository;
            List<TaskLevelItem> lst = new List<TaskLevelItem>();
            var data = taskLevelDA.GetQuery(t => t.IsActive && !t.IsDelete).OrderByDescending(t => t.CreateAt).Page(page, pageSize, out total).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {

                    lst.Add(new TaskLevelItem
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Color = item.Color
                    });
                }
            }
            return lst;
        }
        public void Insert(string name, string color, bool inused, string des, int userId)
        {
            TaskLevel objItem = new TaskLevel
            {
                Name = name.Trim(),
                Color = color,
                Note = des,
                IsActive = inused,
                CreateAt = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
                UpdateAt = DateTime.Now
            };
            taskLevelDA.Insert(objItem);
            taskLevelDA.Save();

        }
        public void Update(int id, string name, string des, string color, bool inused, int userId)
        {
            var objNewItem = taskLevelDA.GetById(id);
            objNewItem.Name = name.Trim();
            objNewItem.Color = color;
            objNewItem.Note = des;
            objNewItem.IsActive = inused;
            objNewItem.UpdateAt = DateTime.Now;
            objNewItem.UpdateBy = userId;
            taskLevelDA.Update(objNewItem);
            taskLevelDA.Save();
        }
        public void Delete(string stringId)
        {
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            taskLevelDA.DeleteRange(ids);
            taskLevelDA.Save();
        }
        public int GetFirst()
        {
            var rs = taskLevelDA.GetQuery().FirstOrDefault();
            if (rs != null)
                return rs.ID;
            return 0;
        }
        public bool CheckNameExits(string name)
        {
            var rs = (taskLevelDA.GetQuery().Where(t => t.Name.ToUpper() == name.ToUpper())).ToList();
            if (rs.Count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CheckNameEditExits(int id, string name)
        {
            var rs = (taskLevelDA.GetQuery().Where(t => t.Name.ToUpper() == name.ToUpper())).FirstOrDefault();
            if (rs != null && rs.ID != id)
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
