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
    public class CategoryService
    {
        CategoryDA categoryDA = new CategoryDA();

        public List<CategoryItem> GetAll(string code)
        {
            var categories = categoryDA.GetQuery()
                        .Where(item => item.Code == code)
                        .Select(item => new CategoryItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Code = item.Code,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .ToList();
            return categories;
        }

        public List<CategoryItem> GetAll(string code, int page, int pageSize, out int totalCount)
        {
            var categories = categoryDA.GetQuery()
                        .Where(item => item.Code == code)
                        .Select(item => new CategoryItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Code = item.Code,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            CreateByView = categoryDA.SystemContext.SystemUsers.Where(u => u.ID == item.CreateBy).Select(u => u.FullName).FirstOrDefault()??string.Empty,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            UpdateByView = categoryDA.SystemContext.SystemUsers.Where(u => u.ID == item.UpdateBy).Select(u => u.FullName).FirstOrDefault()??string.Empty,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return categories;
        }


        public void Insert(CategoryItem item)
        {
            var category = new MnCustomerCategory()
            {
                Name = item.Name,
                Code = item.Code,
                CreateAt = DateTime.UtcNow,
                CreateBy = item.CreateBy,
            };
            categoryDA.Insert(category);
            categoryDA.Save();
        }

        public void Update(CategoryItem item)
        {
            var category = categoryDA.GetById(item.ID);
            category.Name = item.Name;
            category.UpdateAt = DateTime.Now;
            category.UpdateBy = item.UpdateBy;
            categoryDA.Save();
        }

        public void Delete(int id)
        {
            categoryDA.Delete(id);
            categoryDA.Save();
        }

        public void DeleteRange(List<object> ids)
        {
            categoryDA.DeleteRange(ids);
            categoryDA.Save();
        }
    }
}
