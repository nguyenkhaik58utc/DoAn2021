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
    public class ProductDataSizeSV: BaseService
    {
        ProductDataSizeDA productDataSizeDA = new ProductDataSizeDA();

        public List<ProductDataSizeItem> GetProductDataSizes(int page, int pageSize, out int total)
        {
            var lst = productDataSizeDA.GetQuery()
                        .Where(i => !i.IsDelete)
                        .Select(i => new ProductDataSizeItem()
                        {
                            ID = i.ID,
                            Size = i.Size,
                            Unit = i.Unit,
                            UseTime = i.UseTime,
                            IsUpgrade = i.IsUpgrade,
                            IsActive = i.IsActive,
                            CreateAt = i.CreateAt,
                            UpdateAt = i.UpdateAt,
                        })
                        .OrderByDescending(i => i.CreateAt)
                        .Page(page, pageSize, out total)
                        .ToList();
            return lst;
        }
        public ProductDataSizeItem GetProductDataSize(int id)
        {
            var item = productDataSizeDA.GetQuery()
                        .Where(i => i.ID == id)
                        .Where(i => !i.IsDelete)
                        .Select(i => new ProductDataSizeItem()
                        {
                            ID = i.ID,
                            Size = i.Size,
                            Unit = i.Unit,
                            UseTime = i.UseTime,
                            Price = i.Price,
                            Note = i.Note,
                            IsUpgrade = i.IsUpgrade,
                            IsActive = i.IsActive,
                            CreateAt = i.CreateAt,
                            CreateBy = i.CreateBy,
                            UpdateAt = i.UpdateAt,
                            UpdateBy = i.UpdateBy,
                        })
                        .FirstOrDefault();
            return item;
        }
        public void Insert(ProductDataSizeItem item)
        {
            var record = new WebProductDataSize()
            {
                Size = item.Size,
                Unit = item.Unit,
                Price = item.Price,
                UseTime = item.UseTime,
                Note = item.Note,
                IsUpgrade = item.IsUpgrade,
                IsActive = item.IsActive,
                IsDelete = false,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
                UpdateAt = DateTime.Now,
                UpdateBy = User.ID,
            };
            productDataSizeDA.Insert(record);
            productDataSizeDA.Save();
        }
        public void Update(ProductDataSizeItem item)
        {
            var record = productDataSizeDA.GetById(item.ID);
            record.Size = item.Size;
            record.Unit = item.Unit;
            record.Price = item.Price;
            record.UseTime = item.UseTime;
            record.Note = item.Note;
            record.IsUpgrade = item.IsUpgrade;
            record.IsActive = item.IsActive;
            record.UpdateAt = DateTime.Now;
            record.UpdateBy = User.ID;
            productDataSizeDA.Save();
        }
        public void Delete(int id)
        {
            var item = productDataSizeDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            productDataSizeDA.Save();
        }
        public bool CheckExist(ProductDataSizeItem item)
        {
            var check = productDataSizeDA.GetQuery()
                        .Where(i => i.ID != item.ID||item.ID == 0)
                        .Where(i => i.Size == item.Size)
                        .Where(i => i.IsUpgrade == item.IsUpgrade)
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
    }
}
