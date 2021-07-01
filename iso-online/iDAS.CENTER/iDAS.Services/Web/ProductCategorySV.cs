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
    public class ProductCategorySV :BaseService
    {
        ProductCategoryDA productCategoryDA = new ProductCategoryDA();

        public List<SystemItem> GetSystems()
        {
            var dbo = productCategoryDA.Repository;
            var systems = dbo.CenterSystems
                        .Where(i => i.IsActive)
                        .Where(i => !i.IsDelete)
                        .Select(i => new SystemItem
                        {
                            ID = i.ID,
                            Name = i.Name,
                        })
                        .ToList();
            return systems;
        }
        public List<ProductCategoryItem> GetProductCategories(int page, int pageSize, out int total)
        {
            var productCategories = productCategoryDA.GetQuery()
                                    .Where(i => !i.IsDelete)
                                    .Select(i => new ProductCategoryItem()
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
            return productCategories;
        }
        public ProductCategoryItem GetProductCategory(int id)
        {
            var productCategory = productCategoryDA.GetQuery()
                                    .Where(i => i.ID == id)
                                    .Where(i => !i.IsDelete)
                                    .Select(i => new ProductCategoryItem()
                                    {
                                        Image = new AttachmentFileItem() { ID = i.FileID },
                                        ID = i.ID,
                                        CenterSystemID = i.CenterSystemID,
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
            return productCategory;
        }
        public void Insert(ProductCategoryItem item)
        {
            var fileId = new FileSV().Upload(item.Image).FirstOrDefault();
            var productCategory = new WebProductCategory()
            {
                CenterSystemID = item.CenterSystemID,
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
                productCategory.FileID = fileId;
            }
            productCategoryDA.Insert(productCategory);
            productCategoryDA.Save();
        }
        public void Update(ProductCategoryItem item)
        {
            var productCategory = productCategoryDA.GetById(item.ID);
            var fileId = new FileSV().Upload(item.Image).FirstOrDefault();
            productCategory.CenterSystemID = item.CenterSystemID;
            productCategory.WebSitemapID = item.Sitemap.ID == 0 ? new Nullable<int>() : item.Sitemap.ID;
            productCategory.Name = item.Name;
            productCategory.Description = item.Description;
            productCategory.Tags = item.Tags;
            productCategory.Position = item.Position;
            productCategory.IsActive = item.IsActive;
            productCategory.UpdateAt = DateTime.Now;
            productCategory.UpdateBy = User.ID;
            if (fileId != Guid.Empty)
            {
                productCategory.FileID = fileId;
            }
            productCategoryDA.Save();
        }
        public void Delete(int id)
        {
            var item = productCategoryDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            productCategoryDA.Save();
        }
        public bool CheckNameExist(string title)
        {
            var check = productCategoryDA.GetQuery()
                        .Where(i => i.Name.ToUpper().Equals(title.ToUpper()))
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
        public bool CheckNameExist(string title, int id)
        {
            var check = productCategoryDA.GetQuery()
                        .Where(i => i.ID != id)
                        .Where(i => i.Name.ToUpper().Equals(title.ToUpper()))
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
    }
}