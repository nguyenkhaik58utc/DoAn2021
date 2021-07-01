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
    public class ProductMethodSV : BaseService
    {
        ProductMethodDA productMethodDA = new ProductMethodDA();

        public List<ProductMethodItem> GetProductMethods(int page, int pageSize, out int total)
        {
            var ProductMethods = productMethodDA.GetQuery()
                                .Where(i => !i.IsDelete)
                                .Select(i => new ProductMethodItem()
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
            return ProductMethods;
        }
        public ProductMethodItem GetProductMethod(int id)
        {
            var productMethod = productMethodDA.GetQuery()
                                .Where(i => i.ID == id)
                                .Select(i => new ProductMethodItem()
                                {
                                    ID = i.ID,
                                    Name = i.Name,
                                    Description = i.Description,
                                    Html = i.Html,
                                    Tags = i.Tags,
                                    IsActive = i.IsActive,
                                    CreateAt = i.CreateAt,
                                    CreateBy = i.CreateBy,
                                    UpdateAt = i.UpdateAt,
                                    UpdateBy = i.UpdateBy,
                                })
                                .FirstOrDefault();
            return productMethod;
        }
        public void Insert(ProductMethodItem item)
        {
            var productMethod = new WebProductMethod()
            {
                Name = item.Name.Trim(),
                Description = item.Description,
                Html = item.Html,
                Tags = item.Tags,
                IsActive = item.IsActive,
                IsDelete = false,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
                UpdateAt = DateTime.Now,
                UpdateBy = User.ID,
            };
            productMethodDA.Insert(productMethod);
            productMethodDA.Save();
        }
        public void Update(ProductMethodItem item)
        {
            var productMethod = productMethodDA.GetById(item.ID);
            productMethod.Name = item.Name;
            productMethod.Description = item.Description;
            productMethod.Html = item.Html;
            productMethod.Tags = item.Tags;
            productMethod.IsActive = item.IsActive;
            productMethod.UpdateAt = DateTime.Now;
            productMethod.UpdateBy = User.ID;
            productMethodDA.Save();
        }
        public void Delete(int id)
        {
            var item = productMethodDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            productMethodDA.Save();
        }
        public bool CheckNameExist(string name)
        {
            var check = productMethodDA.GetQuery()
                        .Where(i => i.Name.ToUpper().Equals(name.ToUpper()))
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
        public bool CheckNameExist(string name, int id)
        {
            var check = productMethodDA.GetQuery()
                        .Where(i => i.ID != id)
                        .Where(i => i.Name.ToUpper().Equals(name.ToUpper()))
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
    }
}
