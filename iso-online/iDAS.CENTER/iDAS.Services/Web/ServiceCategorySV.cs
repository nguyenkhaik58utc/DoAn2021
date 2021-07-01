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
    public class ServiceCategorySV :BaseService
    {
        ServiceCategoryDA serviceCategoryDA = new ServiceCategoryDA();

        public List<ServiceCategoryItem> GetServiceCategories(int page, int pageSize, out int total)
        {
            var serviceCategories = serviceCategoryDA.GetQuery()
                                    .Where(i => !i.IsDelete)
                                    .Select(i => new ServiceCategoryItem()
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
            return serviceCategories;
        }
        public ServiceCategoryItem GetServiceCategory(int id)
        {
            var serviceCategory = serviceCategoryDA.GetQuery()
                                    .Where(i => i.ID == id)
                                    .Where(i => !i.IsDelete)
                                    .Select(i => new ServiceCategoryItem()
                                    {
                                        Image = new AttachmentFileItem() { ID = i.FileID },
                                        ID = i.ID,
                                        WebSitemapID = i.WebSitemapID,
                                        Sitemap = new SitemapItem(){ID = i.WebSitemapID??0, Text = i.WebSitemap.Text},
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
            return serviceCategory;
        }
        public void Insert(ServiceCategoryItem item)
        {
            var fileId = new FileSV().Upload(item.Image).FirstOrDefault();
            var serviceCategory = new WebServiceCategory()
            {
                WebSitemapID = item.Sitemap.ID == 0 ? new Nullable<int>() : item.Sitemap.ID,
                Name = item.Name.Trim(),
                Description = item.Description,
                Tags = item.Tags,
                Position = item.Position,
                IsActive = item.IsActive,
                IsDelete = false,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
                UpdateAt = DateTime.Now,
                UpdateBy = User.ID,
            };
            if (fileId != Guid.Empty) {
                serviceCategory.FileID = fileId;
            }
            serviceCategoryDA.Insert(serviceCategory);
            serviceCategoryDA.Save();
        }
        public void Update(ServiceCategoryItem item)
        {
            var serviceCategory = serviceCategoryDA.GetById(item.ID);
            var fileId = new FileSV().Upload(item.Image).FirstOrDefault();
            serviceCategory.WebSitemapID = item.Sitemap.ID == 0 ? new Nullable<int>() : item.Sitemap.ID;
            serviceCategory.Name = item.Name;
            serviceCategory.Description = item.Description;
            serviceCategory.Tags = item.Tags;
            serviceCategory.Position = item.Position;
            serviceCategory.IsActive = item.IsActive;
            serviceCategory.UpdateAt = DateTime.Now;
            serviceCategory.UpdateBy = User.ID;
            if (fileId != Guid.Empty)
            {
                serviceCategory.FileID = fileId;
            }
            serviceCategoryDA.Save();
        }
        public void Delete(int id)
        {
            var item = serviceCategoryDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            serviceCategoryDA.Save();
        }
        public bool CheckNameExist(string title)
        {
            var check = serviceCategoryDA.GetQuery()
                        .Where(i => i.Name.ToUpper().Equals(title.ToUpper()))
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
        public bool CheckNameExist(string title, int id)
        {
            var check = serviceCategoryDA.GetQuery()
                        .Where(i => i.ID != id)
                        .Where(i => i.Name.ToUpper().Equals(title.ToUpper()))
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
    }
}