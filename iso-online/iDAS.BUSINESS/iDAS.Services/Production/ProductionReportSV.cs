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
    public class ProductionReportSV
    {
        private ProductionStageDA ProductionStageDA = new ProductionStageDA();
        /// <summary>
        /// Lấy danh sách 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<ProductionStageItem> GetAll(ModelFilter filter)
        {
            var dbo = ProductionStageDA.Repository;
            var ProductionStage = ProductionStageDA.GetQuery()
                        .Select(item => new ProductionStageItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            return ProductionStage;
        }
        /// <summary>
        /// Lấy  theo ID
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ProductionStageItem GetById(int Id)
        {
            var ProductionStage = ProductionStageDA.GetQuery(i => i.ID == Id)
                        .Select(item => new ProductionStageItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                        })
                        .First();
            return ProductionStage;
        }

        /// <summary>
        /// Cập nhật 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(ProductionStageItem item, int userID)
        {
            var ProductionStage = ProductionStageDA.GetById(item.ID);
            ProductionStage.ID = item.ID;
            ProductionStage.ID = item.ID;
            ProductionStage.Name = item.Name;
            ProductionStage.UpdateAt = DateTime.Now;
            ProductionStage.UpdateBy = userID;
            ProductionStageDA.Save();
        }
        /// <summary>
        /// Thêm mới 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(ProductionStageItem item, int userID)
        {
            var ProductionStage = new ProductionStage()
            {
                Name = item.Name,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            ProductionStageDA.Insert(ProductionStage);
            ProductionStageDA.Save();
        }
        /// <summary>
        /// Xóa 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            ProductionStageDA.Delete(id);
            ProductionStageDA.Save();
        }
    }
}
