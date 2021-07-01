using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;

namespace iDAS.Services
{
    public class AuditorSV : BaseService
    {
        AuditorDA auditorDA = new AuditorDA();

        public List<AuditorItem> GetAuditors(int page, int pageSize, out int total)
        {
            var lst = auditorDA.GetQuery()
                        .Where(i => !i.IsDelete)
                        .Select(i => new AuditorItem()
                        {
                            ID = i.ID,
                            Name = i.Name,
                            Role = i.Role,
                            Scope = i.Scope,
                            FileID = i.FileID,
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
        public AuditorItem GetAuditor(int id)
        {
            var item = auditorDA.GetQuery()
                        .Where(i => i.ID == id)
                        .Where(i => !i.IsDelete)
                        .Select(i => new AuditorItem()
                        {
                            Image = new AttachmentFileItem() { ID = i.FileID },
                            ID = i.ID,
                            Name = i.Name,
                            Role = i.Role,
                            Scope = i.Scope,
                            YearOfExperience = i.YearOfExperience,
                            Tags = i.Tags,
                            Profile = i.Profile,
                            Position = i.Position,
                            Description = i.Description,
                            IsActive = i.IsActive,
                            CreateAt = i.CreateAt,
                            CreateBy = i.CreateBy,
                            UpdateAt = i.UpdateAt,
                            UpdateBy = i.UpdateBy,
                        })
                        .FirstOrDefault();
            return item;
        }
        public void Insert(AuditorItem item)
        {
            var record = new WebAuditor()
            {
                Name = item.Name.Trim(),
                Description = item.Description,
                Role = item.Role,
                Scope = item.Scope,
                YearOfExperience = item.YearOfExperience,
                Profile = item.Profile,
                Position = item.Position,
                Tags = item.Tags,
                IsActive = item.IsActive,
                IsDelete = false,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
                UpdateAt = DateTime.Now,
                UpdateBy = User.ID,
            };
            var fileId = new FileSV().Upload(item.Image).FirstOrDefault();
            if (fileId != Guid.Empty)
            {
                record.FileID = fileId;
            }
            auditorDA.Insert(record);
            auditorDA.Save();
        }
        public void Update(AuditorItem item)
        {
            var record = auditorDA.GetById(item.ID);
            record.Name = item.Name;
            record.Description = item.Description;
            record.Role = item.Role;
            record.Scope = item.Scope;
            record.Tags = item.Tags;
            record.Position = item.Position;
            record.Profile = item.Profile;
            record.IsActive = item.IsActive;
            record.YearOfExperience = item.YearOfExperience;
            record.UpdateAt = DateTime.Now;
            record.UpdateBy = User.ID;
            var fileId = new FileSV().Upload(item.Image).FirstOrDefault();
            if (fileId != Guid.Empty)
            {
                record.FileID = fileId;
            }
            auditorDA.Save();
        }
        public void Delete(int id)
        {
            var item = auditorDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            auditorDA.Save();
        }
        public bool CheckExist(AuditorItem item)
        {
            var check = auditorDA.GetQuery()
                        .Where(i => i.ID != item.ID || item.ID == 0)
                        .Where(i => i.Name.ToUpper().Equals(item.Name.ToUpper()))
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
    }
}
