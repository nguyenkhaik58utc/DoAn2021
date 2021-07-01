using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class ProductionShiftSV
    {
        private ProductionShiftDA productionShiftDA = new ProductionShiftDA();
        /// <summary>
        /// Lấy danh sách 
        /// || Author: Thanhnv || CreateDate:20/07/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<ProductionShiftItem> GetAll(ModelFilter filter)
        {
            var dbo = productionShiftDA.Repository;
            var ProductionShift = productionShiftDA.GetQuery()
                        .Select(item => new ProductionShiftItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            IsUse = item.IsUse,
                            CreateAt = item.CreateAt,
                        })
                        .Filter(filter)
                        .ToList();
            return ProductionShift;
        }
        public List<ProductionShiftItem> GetUsing()
        {
            var result = productionShiftDA.GetQuery(i => i.IsUse)
                        .Select(item => new ProductionShiftItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                        })
                        .OrderByDescending(item => item.Name)
                        .ToList();
            return result;
        }
        /// <summary>
        /// Lấy  theo ID
        /// || Author: Thanhnv || CreateDate:20/07/2015  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ProductionShiftItem GetById(int Id)
        {
            var ProductionShift = productionShiftDA.GetQuery(i => i.ID == Id)
                        .Select(item => new ProductionShiftItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            IsUse = item.IsUse,
                        })
                        .First();
            return ProductionShift;
        }

        /// <summary>
        /// Cập nhật 
        /// || Author: Thanhnv || CreateDate:20/07/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(ProductionShiftItem item, int userID)
        {
            var ProductionShift = productionShiftDA.GetById(item.ID);
            ProductionShift.ID = item.ID;
            ProductionShift.ID = item.ID;
            ProductionShift.Name = item.Name;
            ProductionShift.StartTime = item.StartTime;
            ProductionShift.EndTime = item.EndTime;
            ProductionShift.IsUse = item.IsUse;
            ProductionShift.UpdateAt = DateTime.Now;
            ProductionShift.UpdateBy = userID;
            productionShiftDA.Save();
        }
        /// <summary>
        /// Thêm mới 
        /// || Author: Thanhnv || CreateDate:20/07/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(ProductionShiftItem item, int userID)
        {
            var ProductionShift = new ProductionShift()
            {
                Name = item.Name,
                StartTime = item.StartTime,
                EndTime = item.EndTime,
                IsUse = item.IsUse,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            productionShiftDA.Insert(ProductionShift);
            productionShiftDA.Save();
        }
        /// <summary>
        /// Xóa 
        /// || Author: Thanhnv || CreateDate:20/07/2015  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            productionShiftDA.Delete(id);
            productionShiftDA.Save();
        }
    }
}
