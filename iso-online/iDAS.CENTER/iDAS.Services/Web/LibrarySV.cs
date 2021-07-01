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
    public class LibrarySV:BaseService
    {
        LibraryDA libraryDA = new LibraryDA();

        public List<LibraryCategoryItem> GetLibraryCategories()
        {
            var dbo = libraryDA.Repository;
            var lst = dbo.WebLibraryCategories
                        .Where(i => i.IsActive)
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
                        .OrderBy(i => i.CreateAt)
                        .ToList();
            return lst;
        }
        public List<LibraryItem> GetLibraries(int page, int pageSize, out int total, int libraryCategoryId)
        {
            var lst = libraryDA.GetQuery()
                        .Where(i => i.WebLibraryCategoryID == libraryCategoryId)
                        .Where(i => !i.IsDelete)
                        .Select(i => new LibraryItem()
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
        public LibraryItem GetLibrary(int id)
        {
            var item = libraryDA.GetQuery()
                        .Where(i => i.ID == id)
                        .Where(i => !i.IsDelete)
                        .Select(i => new LibraryItem()
                        {
                            LibraryCategoryID = i.WebLibraryCategoryID,
                            ID = i.ID,
                            Name = i.Name,
                            Description = i.Description,
                            Tags = i.Tags,
                            IsFirst = i.IsFirst,
                            IsActive = i.IsActive,
                            RefreshAt = i.RefreshAt,
                            CreateAt = i.CreateAt,
                            CreateBy = i.CreateBy,
                            UpdateAt = i.UpdateAt,
                            UpdateBy = i.UpdateBy,
                        })
                        .FirstOrDefault();
            return item;
        }
        public void Insert(LibraryItem item)
        {
            var record = new WebLibrary()
            {
                WebLibraryCategoryID = item.LibraryCategoryID,
                Name = item.Name.Trim(),
                Description = item.Description,
                Tags = item.Tags,
                IsFirst = item.IsFirst,
                IsActive = item.IsActive,
                IsDelete = false,
                RefreshAt = item.RefreshAt,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
                UpdateAt = DateTime.Now,
                UpdateBy = User.ID,
            };
            libraryDA.Insert(record);
            libraryDA.Save();
        }
        public void Update(LibraryItem item)
        {
            var record = libraryDA.GetById(item.ID);
            record.WebLibraryCategoryID = item.LibraryCategoryID;
            record.Name = item.Name;
            record.Description = item.Description;
            record.Tags = item.Tags;
            record.IsFirst = item.IsFirst;
            record.IsActive = item.IsActive;
            record.RefreshAt = item.RefreshAt;
            record.UpdateAt = DateTime.Now;
            record.UpdateBy = User.ID;
            libraryDA.Save();
        }
        public void Delete(int id)
        {
            var item = libraryDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            libraryDA.Save();
        }
        public bool CheckExist(LibraryItem item)
        {
            var check = libraryDA.GetQuery()
                        .Where(i => i.WebLibraryCategoryID == item.LibraryCategoryID)
                        .Where(i => i.ID != item.ID || item.ID == 0)
                        .Where(i => i.Name.ToUpper().Equals(item.Name.ToUpper()))
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
    }
}