using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iDAS.DA;
using iDAS.Items;
using iDAS.Core;
using iDAS.Base;

namespace iDAS.Services
{
    public class FaqCategorySV : BaseService
    {
        FaqCategoryDA faqCategoryDA = new FaqCategoryDA();

        public List<FaqCategoryItem> GetFaqCategories(int page, int pageSize, out int total)
        {
            var faqCategories = faqCategoryDA.GetQuery()
                        .Where(i => !i.IsDelete)
                        .OrderBy(i => i.Position)
                        .Select(i => new FaqCategoryItem()
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
            return faqCategories;
        }
        public FaqCategoryItem GetFaqCategory(int id)
        {
            var faqCategory = faqCategoryDA.GetQuery()
                                .Where(i => i.ID == id)
                                .Select(i => new FaqCategoryItem()
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
            return faqCategory;
        }
        public void Insert(FaqCategoryItem item)
        {
            var faqCategory = new WebFaqCategory()
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
            faqCategoryDA.Insert(faqCategory);
            faqCategoryDA.Save();
        }
        public void Update(FaqCategoryItem item)
        {
            var faqCategory = faqCategoryDA.GetById(item.ID);
            faqCategory.Name = item.Name;
            faqCategory.Description = item.Description;
            faqCategory.Position = item.Position;
            faqCategory.IsActive = item.IsActive;
            faqCategory.UpdateAt = DateTime.Now;
            faqCategory.UpdateBy = User.ID;
            faqCategoryDA.Save();
        }
        public void Delete(int id)
        {
            var item = faqCategoryDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            faqCategoryDA.Save();
        }
        public bool CheckExist(FaqCategoryItem item)
        {
            var check = faqCategoryDA.GetQuery()
                        .Where(i => i.ID != item.ID || item.ID == 0)
                        .Where(i => i.Name.ToUpper().Equals(item.Name.ToUpper()))
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
    }
}
