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
    public class WebCustomerCommentSV : BaseService
    {
        WebCustomerCommentDA webCustomerCommentDA = new WebCustomerCommentDA();

        public List<WebCustomerCommentItem> GetWebCustomerComments(int page, int pageSize, out int total)
        {
            var lst = webCustomerCommentDA.GetQuery()
                        .Where(i => !i.IsDelete)
                        .OrderBy(m => m.IsFirst)
                        .Select(i => new WebCustomerCommentItem()
                        {
                            ID = i.ID,
                            FileID = i.FileID,
                            Name = i.Name,
                            Role = i.Role,
                            Company = i.Company,
                            Comment = i.Comment,
                            IsActive = i.IsActive,
                            CreateAt = i.CreateAt,
                            UpdateAt = i.UpdateAt,
                        })
                        .OrderByDescending(i => i.CreateAt)
                        .Page(page, pageSize, out total)
                        .ToList();
            return lst;
        }
        public WebCustomerCommentItem GetWebCustomerComment(int id)
        {
            var item = webCustomerCommentDA.GetQuery()
                        .Where(i => i.ID == id)
                        .Where(i => !i.IsDelete)
                        .Select(i => new WebCustomerCommentItem()
                        {
                            ID = i.ID,
                            Image = new AttachmentFileItem{ID = i.FileID},
                            Name = i.Name,
                            Role = i.Role,
                            Company = i.Company,
                            Phone = i.Phone,
                            Email = i.Email,
                            Comment = i.Comment,
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
        public void Insert(WebCustomerCommentItem item)
        {
            var record = new WebCustomerComment()
            {
                Name = item.Name.Trim(),
                Role = item.Role,
                Company = item.Company,
                Phone = item.Phone,
                Email = item.Email,
                Comment = item.Comment,
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
            var fileId = new FileSV().Upload(item.Image).FirstOrDefault();
            if (fileId != Guid.Empty) {
                record.FileID = fileId;
            }
            webCustomerCommentDA.Insert(record);
            webCustomerCommentDA.Save();
        }
        public void Update(WebCustomerCommentItem item)
        {
            var record = webCustomerCommentDA.GetById(item.ID);
            record.Name = item.Name;
            record.Role = item.Role;
            record.Company = item.Company;
            record.Phone = item.Phone;
            record.Email = item.Email;
            record.Comment = item.Comment;
            record.Tags = item.Tags;
            record.IsFirst = item.IsFirst;
            record.IsActive = item.IsActive;
            record.RefreshAt = item.RefreshAt;
            record.UpdateAt = DateTime.Now;
            record.UpdateBy = User.ID;
            var fileId = new FileSV().Upload(item.Image).FirstOrDefault();
            if (fileId != Guid.Empty)
            {
                record.FileID = fileId;
            }
            webCustomerCommentDA.Save();
        }
        public void Delete(int id)
        {
            var item = webCustomerCommentDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            webCustomerCommentDA.Save();
        }
        public bool CheckExist(WebCustomerCommentItem item)
        {
            var check = webCustomerCommentDA.GetQuery()
                        .Where(i => i.ID != item.ID || item.ID == 0)
                        .Where(i => i.Name.ToUpper().Equals(item.Name.ToUpper()))
                        .Where(i => i.Role.ToUpper().Equals(item.Role.ToUpper()))
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
    }
}
