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
    public class WebCustomerContactSV : BaseService
    {
        WebCustomerContactDA webCustomerContactDA = new WebCustomerContactDA();

        public List<WebCustomerContactItem> GetWebCustomerContacts(int page, int pageSize, out int total)
        {
            var lst = webCustomerContactDA.GetQuery()
                        .Where(i => !i.IsDelete)
                        .Select(i => new WebCustomerContactItem()
                        {
                            ID = i.ID,
                            Name = i.Name,
                            Email = i.Email,
                            Content = i.Content,
                            IsRead = i.IsRead,
                            IsSent = i.IsSent,
                            CreateAt = i.CreateAt,
                            UpdateAt = i.UpdateAt,
                        })
                        .OrderByDescending(i => i.CreateAt)
                        .Page(page, pageSize, out total)
                        .ToList();
            return lst;
        }
        public WebCustomerContactItem GetWebCustomerContact(int id)
        {
            var item = webCustomerContactDA.GetQuery()
                        .Where(i => i.ID == id)
                        .Where(i => !i.IsDelete)
                        .Select(i => new WebCustomerContactItem()
                        {
                            ID = i.ID,
                            Name = i.Name,
                            Email = i.Email,
                            IsRead = i.IsRead,
                            IsSent = i.IsSent,
                            Content = i.Content,
                            ContentSent = i.ContentSent,
                            CreateAt = i.CreateAt,
                            CreateBy = i.CreateBy,
                            UpdateAt = i.UpdateAt,
                            UpdateBy = i.UpdateBy,
                        })
                        .FirstOrDefault();
            return item;
        }
        public void Insert(WebCustomerContactItem item)
        {
            var record = new WebContact()
            {
                Name = item.Name.Trim(),
                Email = item.Email,
                Content = item.Content,
                ContentSent = item.ContentSent,
                IsRead = item.IsRead,
                IsSent = item.IsSent,
                IsDelete = false,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
                UpdateAt = DateTime.Now,
                UpdateBy = User.ID,
            };
            webCustomerContactDA.Insert(record);
            webCustomerContactDA.Save();
        }
        public void Update(WebCustomerContactItem item)
        {
            var record = webCustomerContactDA.GetById(item.ID);
            record.Name = item.Name;
            record.Email = item.Email;
            record.Content = item.Content;
            record.ContentSent = item.ContentSent;
            record.IsRead = item.IsRead;
            record.IsSent = item.IsSent;
            record.UpdateAt = DateTime.Now;
            record.UpdateBy = User.ID;
            webCustomerContactDA.Save();
        }
        public void Delete(int id)
        {
            var item = webCustomerContactDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            webCustomerContactDA.Save();
        }
    }
}
