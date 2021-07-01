using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.DAL.MnCustomer;
using iDAS.Items.MnCustomer;

namespace iDAS.Services.MnCustomer
{
    public class ProductService
    {
        ProductDA productDA = new ProductDA();

        public List<ProductItem> GetAll(int categoryID, int page, int pageSize, out int totalCount)
        {
            var products = productDA.GetQuery()
                        .Where(item => item.CategoryID == categoryID && categoryID != 0)
                        .Select(item => new ProductItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Code = item.Code,
                            Price = item.Price,
                            CurrencyID = item.CurrencyID,
                            Weight = item.Weight,
                            Size = item.Size,
                            Provider = item.Provider,
                            Maker = item.Maker,
                            MakeIn = item.MakeIn,
                            MakeInView = productDA.SystemContext.InitCountries.Where(c => c.ID == item.MakeIn).Select(c => c.Name).FirstOrDefault(),
                            WarrantyPeriod = item.WarrantyPeriod,
                            MakeDate = item.MakeDate,
                            ExpiryDate = item.ExpiryDate,
                            CategoryID = item.CategoryID,
                            Description = item.Description,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return products;
        }

        public List<ProductItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var products = productDA.GetQuery()
                        .Select(item => new ProductItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Code = item.Code,
                            Price = item.Price,
                            CurrencyID = item.CurrencyID,
                            Weight = item.Weight,
                            Size = item.Size,
                            Provider = item.Provider,
                            Maker = item.Maker,
                            MakeIn = item.MakeIn,
                            MakeInView = productDA.SystemContext.InitCountries.Where(c => c.ID == item.MakeIn).Select(c => c.Name).FirstOrDefault(),
                            WarrantyPeriod = item.WarrantyPeriod,
                            MakeDate = item.MakeDate,
                            ExpiryDate = item.ExpiryDate,
                            CategoryID = item.CategoryID,
                            Description = item.Description,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return products;
        }

        public void Insert(ProductItem item, int userID)
        {
            var product = new MnCustomerProduct()
            {
                Name = item.Name,
                Code = item.Code,
                Price = item.Price,
                CurrencyID = item.CurrencyID,
                Weight = item.Weight,
                Size = item.Size,
                Provider = item.Provider,
                Maker = item.Maker,
                MakeIn = item.MakeIn,
                WarrantyPeriod = item.WarrantyPeriod,
                MakeDate = item.MakeDate,
                ExpiryDate = item.ExpiryDate,
                CategoryID = item.CategoryID,
                Description = item.Description,
                CreateAt = DateTime.UtcNow,
                CreateBy = userID,
            };
            productDA.Insert(product);
            productDA.Save();
        }
    }
}
