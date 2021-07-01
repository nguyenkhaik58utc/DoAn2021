using System;
using System.Collections.Generic;
using System.Linq;
using iDAS.Core;
using iDAS.Base;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class TaskCategorySV
    {
        private TaskCategoryDA categoryDA = new TaskCategoryDA();
        public List<TaskCategoryItem> GetAll(int page, int pageSize, out int total)
        {
            var dbo = categoryDA.Repository;
            List<TaskCategoryItem> lst = new List<TaskCategoryItem>();
            var data = categoryDA.Get(t => !t.IsDelete).OrderBy(t=>t.CreateAt).Page(page, pageSize, out total).ToList();
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
                    lst.Add(new TaskCategoryItem
                    {
                        ID = item.ID,
                        Name = item.Name,
                        Note = item.Note,
                        IsActive = item.IsActive,
                        CreateBy = item.CreateBy,
                        CreateAt = item.CreateAt,
                        UpdateBy = item.UpdateBy,
                        UpdateAt = item.UpdateAt,
                        UpdateByName = updator,
                        //UpdateAt = (DateTime)updatetime
                    });
                }
            }
            return lst;
        }
        public List<TaskCategoryItem> GetAllIsUse(int page, int pageSize, out int total)
        {
            var dbo = categoryDA.Repository;
            List<TaskCategoryItem> lst = new List<TaskCategoryItem>();
            var data = categoryDA.GetQuery(t => t.IsActive && !t.IsDelete).OrderBy(t => t.CreateAt).Page(page, pageSize, out total).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new TaskCategoryItem
                    {
                        ID = item.ID,
                        Name = item.Name
                    });
                }
            }
            return lst;
        }
        public void Insert(string name, bool inused, string des, int userId)
        {
            TaskCategory objItem = new TaskCategory
            {
                Name = name.Trim(),
                Note = des,
                IsActive = inused,
                CreateAt = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
                UpdateAt = DateTime.Now
            };
            categoryDA.Insert(objItem);
            categoryDA.Save();

        }
        public void Update(int id, string name, string des, bool inused, int userId)
        {
            var objNewItem = categoryDA.GetById(id);
            objNewItem.Name = name.Trim();
            objNewItem.Note = des;
            objNewItem.IsActive = inused;
            objNewItem.UpdateAt = DateTime.Now;
            objNewItem.UpdateBy = userId;
            categoryDA.Update(objNewItem);
            categoryDA.Save();
        }
        public void Delete(string stringId)
        {
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            categoryDA.DeleteRange(ids);
            categoryDA.Save();
        }
        public int GetFirst()
        {
            var rs = categoryDA.GetQuery().FirstOrDefault();
            if (rs != null)
                return rs.ID;
            return 0;
        }
        public bool CheckNameExits(string name)
        {
            var rs = (categoryDA.GetQuery().Where(t => t.Name.ToUpper() == name.ToUpper())).ToList();
            if (rs.Count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public TaskCategory GetByID(int id)
        {
          return  categoryDA.GetById(id);
        }
    }
}
