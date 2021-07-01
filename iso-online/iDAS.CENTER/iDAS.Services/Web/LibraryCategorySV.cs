using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.DA;
using iDAS.Items;
using iDAS.Base;
using iDAS.Core;
namespace iDAS.Services
{
    public class LibraryCategorySV : BaseService
    {
        LibraryCategoryDA libraryCategoryDA = new LibraryCategoryDA();

        public List<LibraryCategoryItem> GetLibraryCategories(int page, int pageSize, out int total)
        {
            var lst = libraryCategoryDA.GetQuery()
                        .Where(i => !i.IsDelete)
                        .OrderBy(i => i.Position)
                        .Select(i => new LibraryCategoryItem()
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
            return lst;
        }
        public LibraryCategoryItem GetLibraryCategory(int id)
        {
            var item = libraryCategoryDA.GetQuery()
                        .Where(i => i.ID == id)
                        .Select(i => new LibraryCategoryItem()
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
            return item;
        }
        public void Insert(LibraryCategoryItem item)
        {
            var record = new WebLibraryCategory()
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
            libraryCategoryDA.Insert(record);
            libraryCategoryDA.Save();
        }
        public void Update(LibraryCategoryItem item)
        {
            var record = libraryCategoryDA.GetById(item.ID);
            record.Name = item.Name;
            record.Description = item.Description;
            record.Position = item.Position;
            record.IsActive = item.IsActive;
            record.UpdateAt = DateTime.Now;
            record.UpdateBy = User.ID;
            libraryCategoryDA.Save();
        }
        public void Delete(int id)
        {
            var item = libraryCategoryDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            libraryCategoryDA.Save();
        }
        public bool CheckExist(LibraryCategoryItem item)
        {
            var check = libraryCategoryDA.GetQuery()
                        .Where(i => i.ID != item.ID || item.ID == 0)
                        .Where(i => i.Name.ToUpper().Equals(item.Name.ToUpper()))
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
    }
}
