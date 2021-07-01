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
    public class WebCustomerSV : BaseService
    {
        WebCustomerDA WebCustomerDA = new WebCustomerDA();

        public List<WebCustomerItem> GetWebCustomers(int page, int pageSize, out int total)
        {
            var lst = WebCustomerDA.GetQuery()
                        .Where(i => !i.IsDelete)
                        .Select(i => new WebCustomerItem()
                        {
                            ID = i.ID,
                            FileID = i.FileID,
                            Name = i.Name,
                            Email = i.Email,
                            Phone = i.Phone,
                            Address = i.Address,
                            IsActive = i.IsActive,
                            CreateAt = i.CreateAt,
                            UpdateAt = i.UpdateAt,
                        })
                        .OrderByDescending(i => i.CreateAt)
                        .Page(page, pageSize, out total)
                        .ToList();
            return lst;
        }
        public WebCustomerItem GetWebCustomer(int id)
        {
            var item = WebCustomerDA.GetQuery()
                        .Where(i => i.ID == id)
                        .Where(i => !i.IsDelete)
                        .Select(i => new WebCustomerItem()
                        {
                            ID = i.ID,
                            Image = new AttachmentFileItem(){ ID = i.FileID},
                            Name = i.Name,
                            Address = i.Address,
                            Phone = i.Phone,
                            Email = i.Email,
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
        public void Insert(WebCustomerItem item)
        {
            var record = new WebCustomer()
            {
                Name = item.Name.Trim(),
                Phone = item.Phone,
                Email = item.Email,
                Address = item.Address,
                Description = item.Description,
                IsActive = item.IsActive,
                IsDelete = false,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
                UpdateAt = DateTime.Now,
                UpdateBy = User.ID,
            };
            var fileId = new FileSV().Upload(item.Image).FirstOrDefault();
            if (fileId != Guid.Empty) {
                record.FileID = fileId;
            }
            WebCustomerDA.Insert(record);
            WebCustomerDA.Save();
        }
        public void Update(WebCustomerItem item)
        {
            var record = WebCustomerDA.GetById(item.ID);
            record.Name = item.Name;
            record.Phone = item.Phone;
            record.Email = item.Email;
            record.Address = item.Address;
            record.Description = item.Description;
            record.IsActive = item.IsActive;
            record.UpdateAt = DateTime.Now;
            record.UpdateBy = User.ID;
            var fileId = new FileSV().Upload(item.Image).FirstOrDefault();
            if (fileId != Guid.Empty)
            {
                record.FileID = fileId;
            }
            WebCustomerDA.Save();
        }
        public void Delete(int id)
        {
            var item = WebCustomerDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            WebCustomerDA.Save();
        }
        public bool CheckExist(WebCustomerItem item)
        {
            var check = WebCustomerDA.GetQuery()
                        .Where(i => i.ID != item.ID || item.ID == 0)
                        .Where(i => i.Name.ToUpper().Equals(item.Name.ToUpper()))
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
    }
}
