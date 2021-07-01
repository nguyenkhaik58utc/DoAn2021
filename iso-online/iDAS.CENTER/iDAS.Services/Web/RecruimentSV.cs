using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Utilities;
using iDAS.Base;

namespace iDAS.Services
{
    public class RecruimentSV : BaseService
    {
        RecruimentDA recruimentDA = new RecruimentDA();

        public List<RecruimentItem> GetRecruiments(int page, int pageSize, out int total)
        {
            var recruiments = recruimentDA.GetQuery()
                        .Where(i => !i.IsDelete)
                        .Select(i => new RecruimentItem()
                        {
                            ID = i.ID,
                            Title = i.Title,
                            Quatity = i.Quatity,
                            IsActive = i.IsActive,
                            RefreshAt = i.RefreshAt,
                            FinishAt = i.FinishAt,
                            CreateAt = i.CreateAt,
                            UpdateAt = i.UpdateAt,
                        })
                        .OrderByDescending(i => i.CreateAt)
                        .Page(page, pageSize, out total)
                        .ToList();
            return recruiments;
        }
        public RecruimentItem GetRecruiment(int id)
        {
            var recruitment = recruimentDA.GetQuery()
                            .Where(i => i.ID == id)
                            .Where(i => !i.IsDelete)
                            .Select(i => new RecruimentItem()
                            {
                                Image = new AttachmentFileItem() { ID = i.FileID },
                                ID = i.ID,
                                Title = i.Title,
                                Description = i.Description,
                                Html = i.Html,
                                Role = i.Role,
                                TotalView = i.TotalView,
                                Quatity = i.Quatity,
                                Address = i.Address,
                                ContactMail = i.ContactMail,
                                ContactPhone = i.ContactPhone,
                                ContactName = i.ContactName,
                                IsFirst = i.IsFirst,
                                IsActive = i.IsActive,
                                FinishAt = i.FinishAt,
                                RefreshAt = i.RefreshAt,
                                CreateAt = i.CreateAt,
                                CreateBy = i.CreateBy,
                                UpdateAt = i.UpdateAt,
                                UpdateBy = i.UpdateBy,
                            })
                            .FirstOrDefault();
            return recruitment;
        }
        public void Insert(RecruimentItem item)
        {
            var fileId = new FileSV().Upload(item.Image).FirstOrDefault();
            var recruiment = new WebRecruiment()
            {
                Title = item.Title.Trim(),
                Description = item.Description,
                Quatity = item.Quatity,
                Html = item.Html,
                Role = item.Role,
                Address = item.Address,
                ContactName = item.ContactName,
                ContactPhone = item.ContactPhone,
                ContactMail = item.ContactMail,
                IsFirst = item.IsFirst,
                IsActive = item.IsActive,
                IsDelete = false,
                TotalView = 0,
                FinishAt = item.FinishAt,
                RefreshAt = item.RefreshAt,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
                UpdateAt = DateTime.Now,
                UpdateBy = User.ID,
            };
            if (fileId != Guid.Empty) {
                recruiment.FileID = fileId;
            }
            recruimentDA.Insert(recruiment);
            recruimentDA.Save();
        }
        public void Update(RecruimentItem item)
        {
            var record = recruimentDA.GetById(item.ID);
            var fileId = new FileSV().Upload(item.Image).FirstOrDefault();
            record.Title = item.Title;
            record.Description = item.Description;
            record.Html = item.Html;
            record.Role = item.Role;
            record.Quatity = item.Quatity;
            record.Address = item.Address;
            record.ContactMail = item.ContactMail;
            record.ContactName = item.ContactName;
            record.ContactPhone = item.ContactPhone;
            record.IsFirst = item.IsFirst;
            record.IsActive = item.IsActive;
            record.FinishAt = item.FinishAt;
            record.RefreshAt = item.RefreshAt;
            record.UpdateAt = DateTime.Now;
            record.UpdateBy = User.ID;
            if (fileId != Guid.Empty)
            {
                record.FileID = fileId;
            } 
            recruimentDA.Save();
        }
        public void Delete(int id)
        {
            var item = recruimentDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            recruimentDA.Save();
        }
    }
}
