using iDAS.Base;
using iDAS.DA;
using iDAS.Items;
using iDAS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;

namespace iDAS.Services
{
    public class NewsCategorySV : BaseService
    {
        NewsCategoryDA newsCategoryDA = new NewsCategoryDA();

        public List<NewsCategoryItem> GetNewsCategories(int page, int pageSize, out int total)
        {
            var newsCategories = newsCategoryDA.GetQuery()
                                .Where(i => !i.IsDelete)
                                .OrderBy(i => i.Position)
                                .Select(i => new NewsCategoryItem()
                                {
                                    ID = i.ID,
                                    Name = i.Name,
                                    Description = i.Description,
                                    IsActive = i.IsActive,
                                    CreateAt = i.CreateAt,
                                    UpdateAt = i.UpdateAt,
                                })
                                .OrderByDescending(i => i.CreateAt)
                                .Page(page, pageSize, out total)
                                .ToList();
            return newsCategories;
        }
        public NewsCategoryItem GetNewsCategory(int id)
        {
            var newsCategory = newsCategoryDA.GetQuery()
                                .Where(i => i.ID == id)
                                .Select(i => new NewsCategoryItem()
                                {
                                    ID = i.ID,
                                    Name = i.Name,
                                    Description = i.Description,
                                    IsActive = i.IsActive,
                                    Position = i.Position,
                                    CreateAt = i.CreateAt,
                                    CreateBy = i.CreateBy,
                                    UpdateAt = i.UpdateAt,
                                    UpdateBy = i.UpdateBy,
                                })
                                .FirstOrDefault();
            return newsCategory;
        }
        public void Insert(NewsCategoryItem item)
        {
            var newsCategory = new WebNewsCategory()
            {
                Name = item.Name.Trim(),
                Description = item.Description,
                IsActive = item.IsActive,
                IsDelete = false,
                Position = item.Position,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
                UpdateAt = DateTime.Now,
                UpdateBy = User.ID,
            };
            newsCategoryDA.Insert(newsCategory);
            newsCategoryDA.Save();
        }
        public void Update(NewsCategoryItem item)
        {
            var newsCategory = newsCategoryDA.GetById(item.ID);
            newsCategory.Name = item.Name;
            newsCategory.Description = item.Description;
            newsCategory.Position = item.Position;
            newsCategory.IsActive = item.IsActive;
            newsCategory.UpdateAt = DateTime.Now;
            newsCategory.UpdateBy = User.ID;
            newsCategoryDA.Save();
        }
        public void Delete(int id)
        {
            var item = newsCategoryDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            newsCategoryDA.Save();
        }
        public bool CheckNameExist(string name)
        {
            var check = newsCategoryDA.GetQuery()
                        .Where(i => i.Name.ToUpper().Equals(name.ToUpper()))
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
        public bool CheckNameExist(string name, int id)
        {
            var check = newsCategoryDA.GetQuery()
                        .Where(i => i.ID != id)
                        .Where(i => i.Name.ToUpper().Equals(name.ToUpper()))
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
    }
}
