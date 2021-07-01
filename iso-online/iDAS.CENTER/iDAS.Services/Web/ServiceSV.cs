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
    public class ServiceSV : BaseService
    {
        ServiceDA serviceDA = new ServiceDA();

        public List<ServiceCategoryItem> GetServiceCategories()
        {
            var dbo = serviceDA.Repository;
            var serviceCategories = dbo.WebServiceCategories
                        .Where(i => i.IsActive)
                        .Where(i => !i.IsDelete)
                        .Select(i => new ServiceCategoryItem
                        {
                            ID = i.ID,
                            Name = i.Name,
                        })
                        .ToList();
            return serviceCategories;
        }
        public List<ServiceItem> GetServices(int page, int pageSize, out int total, int serviceCategoryId)
        {
            var services = serviceDA.GetQuery()
                            .Where(i => i.WebServiceCategoryID == serviceCategoryId)
                            .Where(i => !i.IsDelete)
                            .Select(i => new ServiceItem()
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
            return services;
        }
        public ServiceItem GetService(int id)
        {
            var service = serviceDA.GetQuery()
                            .Where(i => i.ID == id)
                            .Where(i => !i.IsDelete)
                            .Select(i => new ServiceItem()
                            {
                                Image = new AttachmentFileItem() { ID = i.FileID },
                                ID = i.ID,
                                WebSitemapID = i.WebSitemapID,
                                Sitemap = new SitemapItem() { ID = i.WebSitemapID ?? 0, Text = i.WebSitemap.Text },
                                Name = i.Name,
                                Description = i.Description,
                                Tags = i.Tags,
                                Position = i.Position,
                                IsActive = i.IsActive,
                                CreateAt = i.CreateAt,
                                CreateBy = i.CreateBy,
                                UpdateAt = i.UpdateAt,
                                UpdateBy = i.UpdateBy,
                            })
                            .FirstOrDefault();
            return service;
        }
        public void Insert(ServiceItem item)
        {
            var service = new WebService()
            {
                WebServiceCategoryID = item.ServiceCategoryID,
                Name = item.Name.Trim(),
                Description = item.Description,
                WebSitemapID = item.Sitemap.ID == 0 ? new Nullable<int>() : item.Sitemap.ID,
                Tags = item.Tags,
                Position = item.Position,
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
                service.FileID = fileId;
            }
            serviceDA.Insert(service);
            serviceDA.Save();
        }
        public void Update(ServiceItem item)
        {
            var record = serviceDA.GetById(item.ID);
            record.Name = item.Name;
            record.Description = item.Description;
            record.WebSitemapID = item.Sitemap.ID == 0 ? new Nullable<int>() : item.Sitemap.ID;
            record.Tags = item.Tags;
            record.Position = item.Position;
            record.IsActive = item.IsActive;
            record.UpdateAt = DateTime.Now;
            record.UpdateBy = User.ID;
            var fileId = new FileSV().Upload(item.Image).FirstOrDefault();
            if (fileId != Guid.Empty)
            {
                record.FileID = fileId;
            }
            serviceDA.Save();
        }
        public void Delete(int id)
        {
            var item = serviceDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            serviceDA.Save();
        }
        public bool CheckNameExist(string name)
        {
            var check = serviceDA.GetQuery()
                        .Where(i => i.Name.ToUpper().Equals(name.ToUpper()))
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
        public bool CheckNameExist(string name, int id)
        {
            var check = serviceDA.GetQuery()
                        .Where(i => i.ID != id)
                        .Where(i => i.Name.ToUpper().Equals(name.ToUpper()))
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
    }
}
