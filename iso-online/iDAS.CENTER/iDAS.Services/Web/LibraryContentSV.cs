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
    public class LibraryContentSV:BaseService
    {
        LibraryContentDA libraryContentDA = new LibraryContentDA();

        public List<LibraryContentItem> GetLibraryContents(int page, int pageSize, out int total, int libraryId)
        {
            var lst = libraryContentDA.GetQuery()
                        .Where(i => i.WebLibraryID == libraryId)
                        .Where(i => !i.IsDelete)
                        .Select(i => new LibraryContentItem()
                        {
                            ID = i.ID,
                            Name = i.Name,
                            Description = i.Description,
                            IsFirst = i.IsFirst,
                            IsVideo = i.IsVideo,
                            IsActive = i.IsActive,
                            CreateAt = i.CreateAt,
                            UpdateAt = i.UpdateAt,
                        })
                        .OrderByDescending(i => i.CreateAt)
                        .Page(page, pageSize, out total)
                        .ToList();
            return lst;
        }
        public LibraryContentItem GetLibraryContent(int id)
        {
            var item = libraryContentDA.GetQuery()
                        .Where(i => i.ID == id)
                        .Where(i => !i.IsDelete)
                        .Select(i => new LibraryContentItem()
                        {
                            LibraryID = i.WebLibraryID,
                            Image = new AttachmentFileItem(){ID = i.FileID},
                            ID = i.ID,
                            Name = i.Name,
                            Description = i.Description,
                            Position = i.Position,
                            VideoUrl = i.VideoUrl,
                            IsFirst = i.IsFirst,
                            IsVideo = i.IsVideo,
                            IsActive = i.IsActive,
                            CreateAt = i.CreateAt,
                            CreateBy = i.CreateBy,
                            UpdateAt = i.UpdateAt,
                            UpdateBy = i.UpdateBy,
                        })
                        .FirstOrDefault();
            return item;
        }
        public void Insert(LibraryContentItem item)
        {
            var record = new WebLibraryItem()
            {
                WebLibraryID = item.LibraryID,
                Name = item.Name.Trim(),
                Description = item.Description,
                VideoUrl = item.VideoUrl,
                IsFirst = item.IsFirst,
                IsVideo = item.IsVideo,
                IsActive = item.IsActive,
                IsDelete = false,
                Position = item.Position,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
                UpdateAt = DateTime.Now,
                UpdateBy = User.ID,
            };
            var fileId = new FileSV().Upload(item.Image).FirstOrDefault();
            if (fileId != Guid.Empty) {
                record.FileID = fileId;
            }
            libraryContentDA.Insert(record);
            libraryContentDA.Save();
        }
        public void Update(LibraryContentItem item)
        {
            var record = libraryContentDA.GetById(item.ID);
            record.Name = item.Name;
            record.Description = item.Description;
            record.Position = item.Position;
            record.VideoUrl = item.VideoUrl;
            record.IsFirst = item.IsFirst;
            record.IsVideo = item.IsVideo;
            record.IsActive = item.IsActive;
            record.UpdateAt = DateTime.Now;
            record.UpdateBy = User.ID;
            var fileId = new FileSV().Upload(item.Image).FirstOrDefault();
            if (fileId != Guid.Empty)
            {
                record.FileID = fileId;
            }
            libraryContentDA.Save();
        }
        public void Delete(int id)
        {
            var item = libraryContentDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            libraryContentDA.Save();
        }
        public bool CheckExist(LibraryContentItem item)
        {
            var check = libraryContentDA.GetQuery()
                        .Where(i => i.WebLibraryID == item.LibraryID)
                        .Where(i => i.ID != item.ID || item.ID == 0)
                        .Where(i => i.Name.ToUpper().Equals(item.Name.ToUpper()))
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
    }
}