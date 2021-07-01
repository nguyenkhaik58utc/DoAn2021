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
    public class RecruimentProfileSV : BaseService
    {
        RecruimentProfileDA recruimentProfileDA = new RecruimentProfileDA();

        public List<RecruimentProfileItem> GetRecruimentProfiles(int page, int pageSize, out int total, int recruimentId)
        {
            var lst = recruimentProfileDA.GetQuery()
                    .Where(i => i.WebRecruimentID == recruimentId)
                    .Where(i => !i.IsDelete)
                    .Select(i => new RecruimentProfileItem()
                    {
                        ID = i.ID,
                        Name = i.Name,
                        Address = i.Address,
                        Phone = i.Phone,
                        Email = i.Email,
                        Note = i.Note,
                        IsActive = i.IsActive,
                        CreateAt = i.CreateAt,
                        UpdateAt = i.UpdateAt,
                    })
                    .OrderByDescending(i => i.CreateAt)
                    .Page(page, pageSize, out total)
                    .ToList();
            return lst;
        }
        public RecruimentProfileItem GetRecruimentProfile(int id)
        {
            var item = recruimentProfileDA.GetQuery()
                            .Where(i => i.ID == id)
                            .Where(i => !i.IsDelete)
                            .Select(i => new RecruimentProfileItem()
                            {
                                File = new AttachmentFileItem() { ID = i.FileID, Name = i.File.Name },
                                ID = i.ID,
                                Name = i.Name,
                                Address = i.Address,
                                Phone = i.Phone,
                                Email = i.Email,
                                Note = i.Note,
                                IsActive = i.IsActive,
                                CreateAt = i.CreateAt,
                                CreateBy = i.CreateBy,
                                UpdateAt = i.UpdateAt,
                                UpdateBy = i.UpdateBy,
                            })
                            .FirstOrDefault();
            return item;
        }
        public void Insert(RecruimentProfileItem item)
        {
            var fileId = new FileSV().Upload(item.File).FirstOrDefault();
            var recruiment = new WebRecruimentProfile()
            {
                Name = item.Name.Trim(),
                Address = item.Address,
                IsActive = item.IsActive,
                IsDelete = false,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
                UpdateAt = DateTime.Now,
                UpdateBy = User.ID,
            };
            if (fileId != Guid.Empty) {
                recruiment.FileID = fileId;
            }
            recruimentProfileDA.Insert(recruiment);
            recruimentProfileDA.Save();
        }
        public void Update(RecruimentProfileItem item)
        {
            var record = recruimentProfileDA.GetById(item.ID);
            var fileId = new FileSV().Upload(item.File).FirstOrDefault();
            record.Name = item.Name;
            record.Address = item.Address;
            record.IsActive = item.IsActive;
            record.UpdateAt = DateTime.Now;
            record.UpdateBy = User.ID;
            if (fileId != Guid.Empty)
            {
                record.FileID = fileId;
            }
            recruimentProfileDA.Save();
        }
        public void Delete(int id)
        {
            var item = recruimentProfileDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            recruimentProfileDA.Save();
        }
    }
}
