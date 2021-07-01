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
    public class ProductSV : BaseService
    {
        ProductDA productDA = new ProductDA();

        public List<ProductCategoryItem> GetProductCategories()
        {
            var dbo = productDA.Repository;
            var productCategories = dbo.WebProductCategories
                        .Where(i => i.IsActive)
                        .Where(i => !i.IsDelete)
                        .Select(i => new ProductCategoryItem
                        {
                            ID = i.ID,
                            Name = i.Name,
                        })
                        .ToList();
            return productCategories;
        }
        public List<ProductItem> GetProducts(int page, int pageSize, out int total, int productCategoryId)
        {
            var products = productDA.GetQuery()
                            .Where(i => i.WebProductCategoryID == productCategoryId)
                            .Where(i => !i.IsDelete)
                            .Select(i => new ProductItem()
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
            return products;
        }
        public ProductItem GetProduct(int id)
        {
            var product = productDA.GetQuery()
                            .Where(i => i.ID == id)
                            .Where(i => !i.IsDelete)
                            .Select(i => new ProductItem()
                            {
                                Image = new AttachmentFileItem() { ID = i.FileID },
                                ID = i.ID,
                                WebSitemapID = i.WebSitemapID,
                                Sitemap = new SitemapItem() { ID = i.WebSitemapID ?? 0, Text = i.WebSitemap.Text },
                                Name = i.Name,
                                Description = i.Description,
                                Tags = i.Tags,
                                PublicAt = i.PublicAt,
                                TotalRegister = i.TotalRegister,
                                Position = i.Position,
                                Rate = i.Rate,
                                Version = i.Version,
                                IsActive = i.IsActive,
                                CreateAt = i.CreateAt,
                                CreateBy = i.CreateBy,
                                UpdateAt = i.UpdateAt,
                                UpdateBy = i.UpdateBy,
                            })
                            .FirstOrDefault();
            return product;
        }
        public void Insert(ProductItem item)
        {
            var product = new WebProduct()
            {
                WebProductCategoryID = item.ProductCategoryID,
                Name = item.Name.Trim(),
                Description = item.Description,
                WebSitemapID = item.Sitemap.ID == 0 ? new Nullable<int>() : item.Sitemap.ID,
                Tags = item.Tags,
                Rate = item.Rate,
                TotalRegister = item.TotalRegister,
                Position = item.Position,
                PublicAt = item.PublicAt,
                Version = item.Version,
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
                product.FileID = fileId;
            }
            productDA.Insert(product);
            productDA.Save();
        }
        public void Update(ProductItem item)
        {
            var record = productDA.GetById(item.ID);
            record.Name = item.Name;
            record.Description = item.Description;
            record.WebSitemapID = item.Sitemap.ID == 0 ? new Nullable<int>() : item.Sitemap.ID;
            record.Tags = item.Tags;
            record.PublicAt = item.PublicAt;
            record.Rate = item.Rate;
            record.TotalRegister = item.TotalRegister;
            record.Position = item.Position;
            record.Version = item.Version;
            record.IsActive = item.IsActive;
            record.UpdateAt = DateTime.Now;
            record.UpdateBy = User.ID;
            var fileId = new FileSV().Upload(item.Image).FirstOrDefault();
            if (fileId != Guid.Empty)
            {
                record.FileID = fileId;
            }
            productDA.Save();
        }
        public void Delete(int id)
        {
            var item = productDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            productDA.Save();
        }
        public bool CheckExist(ProductItem item)
        {
            var check = productDA.GetQuery()
                        .Where(i => i.ID != item.ID || item.ID == 0)
                        .Where(i => i.WebProductCategoryID == item.ProductCategoryID)
                        .Where(i => i.Name.ToUpper().Equals(item.Name.ToUpper()))
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
    }
}
