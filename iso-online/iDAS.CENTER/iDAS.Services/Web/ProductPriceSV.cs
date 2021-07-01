using iDAS.Base;
using iDAS.DA;
using iDAS.Items;
using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
    public class ProductPriceSV: BaseService
    {
        ProductPriceDA productPriceDA = new ProductPriceDA();

        public List<ProductMethodItem> GetProductMethods()
        {
            var dbo = productPriceDA.Repository;
            var methods = dbo.WebProductMethods
                            .Where(i => i.IsActive)
                            .Where(i => !i.IsDelete)
                            .Select(i => new ProductMethodItem()
                            {
                                ID = i.ID,
                                Name = i.Name,
                            })
                            .ToList();
            return methods;
        }
        public List<ProductPriceItem> GetProductPrices(int page, int pageSize, out int total, int productId)
        {
            var productPrices = productPriceDA.GetQuery()
                                .Where(i => i.WebProductID == productId)
                                .Where(i => !i.IsDelete)
                                .Select(i => new ProductPriceItem()
                                {
                                    ID = i.ID,
                                    ProductMethodName = i.WebProductMethod.Name,
                                    Price = i.Price,
                                    Note = i .Note,
                                    IsActive = i.IsActive,
                                    CreateAt = i.CreateAt,
                                    UpdateAt = i.UpdateAt,
                                })
                                .OrderByDescending(i => i.CreateAt)
                                .Page(page, pageSize, out total)
                                .ToList();
            return productPrices;
        }
        public ProductPriceItem GetProductPrice(int id)
        {
            var productPrice = productPriceDA.GetQuery()
                            .Where(i => i.ID == id)
                            .Where(i => !i.IsDelete)
                            .Select(i => new ProductPriceItem()
                            {
                                ID = i.ID,
                                ProductMethodID = i.WebProductMethodID,
                                Price = i.Price,
                                Note = i.Note,
                                IsActive = i.IsActive,
                                CreateAt = i.CreateAt,
                                CreateBy = i.CreateBy,
                                UpdateAt = i.UpdateAt,
                                UpdateBy = i.UpdateBy,
                            })
                            .FirstOrDefault();
            return productPrice;
        }
        public void Insert(ProductPriceItem item)
        {
            var productPrice = new WebProductItem()
            {
                WebProductID = item.ProductID,
                WebProductMethodID = item.ProductMethodID,
                Price = item.Price,
                Note = item.Note,
                IsActive = item.IsActive,
                IsDelete = false,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
                UpdateAt = DateTime.Now,
                UpdateBy = User.ID,
            };
            productPriceDA.Insert(productPrice);
            productPriceDA.Save();
        }
        public void Update(ProductPriceItem item)
        {
            var productPrice = productPriceDA.GetById(item.ID);
            productPrice.WebProductMethodID = item.ProductMethodID;
            productPrice.Price = item.Price;
            productPrice.Note = item.Note;
            productPrice.IsActive = item.IsActive;
            productPrice.UpdateAt = DateTime.Now;
            productPrice.UpdateBy = User.ID;
            productPriceDA.Save();
        }
        public void Delete(int id)
        {
            var item = productPriceDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            productPriceDA.Save();
        }
    }
}
