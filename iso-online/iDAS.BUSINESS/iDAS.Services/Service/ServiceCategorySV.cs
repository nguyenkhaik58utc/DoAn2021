using iDAS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class ServiceCategorySV
    {
        private ServiceCategoryDA categoryDA = new ServiceCategoryDA();
        public List<ServiceCategoryItem> GetIsUse(int page, int pageSize, out int total)
        {
            var dbo = categoryDA.Repository;
            List<ServiceCategoryItem> lst = new List<ServiceCategoryItem>();
            var data = categoryDA.GetQuery(t => t.IsUse && !t.IsDelete).OrderBy(t => t.Name).Page(page, pageSize, out total).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new ServiceCategoryItem
                    {
                        ID = item.ID,
                        Name = item.Name
                    });
                }
            }
            return lst;
        }
        public List<ServiceCategoryItem> GetAll(ModelFilter filter)
        {
            var dbo = categoryDA.Repository;
            List<ServiceCategoryItem> lst = new List<ServiceCategoryItem>();
            var data = categoryDA.GetQuery()
                .Select(item => new ServiceCategoryItem() {
                    ID = item.ID,
                    Name = item.Name,
                    Note = item.Note,
                    IsUse = item.IsUse,
                    CreateBy = item.CreateBy,
                    CreateAt = item.CreateAt,
                    UpdateBy = item.UpdateBy,
                    UpdateAt = item.UpdateAt,
                    UpdateByName = dbo.HumanEmployees.FirstOrDefault(t => t.ID == dbo.HumanUsers.FirstOrDefault(h => h.ID == (item.UpdateBy.HasValue?item.UpdateBy.Value:item.CreateBy.Value)).HumanEmployeeID).Name,
                    UpdateByTime = item.UpdateAt.HasValue?item.UpdateAt.Value:item.CreateAt.Value
                
                })
                .Filter(filter)
                .ToList();
           
            return data;
        }
        public void Insert(ServiceCategoryItem obj, int userId)
        {
            ServiceCategory objItem = new ServiceCategory
            {
                Name = obj.Name.Trim(),
                Note = obj.Note,
                IsUse = obj.IsUse,
                CreateAt = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
                UpdateAt = DateTime.Now
            };
            categoryDA.Insert(objItem);
            categoryDA.Save();

        }
        public ServiceCategoryItem GetByID(int id)
        {
            var objNewItem = categoryDA.GetById(id);
            ServiceCategoryItem obj = new ServiceCategoryItem();
            if (objNewItem != null)
            {
                obj.IsUse = objNewItem.IsUse;
                obj.Name = objNewItem.Name;
                obj.Note = objNewItem.Note;
                obj.ID = objNewItem.ID;
            }
            return obj;
        }
        public void Update(ServiceCategoryItem objEdit, int userId)
        {
            var objNewItem = categoryDA.GetById(objEdit.ID);
            objNewItem.Name = objEdit.Name.Trim();
            objNewItem.Note = objEdit.Note;
            objNewItem.IsUse = objEdit.IsUse;
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
        public bool CheckNameExitEdits(int id, string name)
        {
            var rs = (categoryDA.GetQuery().Where(t => t.Name.ToUpper() == name.ToUpper() && t.ID!=id)).ToList();
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
