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
    public class ProductionLevelSV
    {
        private ProductionLevelDA CommandLevelDA = new ProductionLevelDA();
        /// <summary>
        /// Lấy danh sách 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<ProductionLevelItem> GetAll(ModelFilter filter)
        {
            var dbo = CommandLevelDA.Repository;
            var ProductionCommandLevel = CommandLevelDA.GetQuery()
                        .Select(item => new ProductionLevelItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Color = item.Color,
                            Note = item.Note,
                            IsActive = item.IsActive,
                            IsDelete = item.IsDelete,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            return ProductionCommandLevel;
        }
        public List<ProductionLevelItem> GetActive()
        {
            var ProductionCommandLevel = CommandLevelDA.GetQuery(i => i.IsActive && !i.IsDelete)
                        .Select(item => new ProductionLevelItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Color = item.Color,
                        })
                        .OrderByDescending(item => item.Name)
                        .ToList();
            return ProductionCommandLevel;
        }
        /// <summary>
        /// Lấy  theo ID
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ProductionLevelItem GetById(int Id)
        {
            var ProductionCommandLevel = CommandLevelDA.GetQuery(i => i.ID == Id)
                        .Select(item => new ProductionLevelItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Color = item.Color,
                            Note = item.Note,
                            IsActive = item.IsActive,
                            IsDelete = item.IsDelete,
                        })
                        .First();
            return ProductionCommandLevel;
        }

        /// <summary>
        /// Cập nhật 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(ProductionLevelItem item, int userID)
        {
            var ProductionCommandLevel = CommandLevelDA.GetQuery(i=>i.ID==item.ID).First();
            ProductionCommandLevel.ID = item.ID;
            ProductionCommandLevel.ID = item.ID;
            ProductionCommandLevel.Name = item.Name;
            ProductionCommandLevel.Color = item.Color;
            ProductionCommandLevel.Note = item.Note;
            ProductionCommandLevel.IsActive = item.IsActive;
            ProductionCommandLevel.IsDelete = item.IsDelete;
            ProductionCommandLevel.UpdateAt = DateTime.Now;
            ProductionCommandLevel.UpdateBy = userID;
            CommandLevelDA.Save();
        }
        /// <summary>
        /// Thêm mới 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(ProductionLevelItem item, int userID)
        {
            var ProductionCommandLevel = new ProductionLevel()
            {
                Name = item.Name,
                Color = item.Color,
                Note = item.Note,
                IsActive = item.IsActive,
                IsDelete = item.IsDelete,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            CommandLevelDA.Insert(ProductionCommandLevel);
            CommandLevelDA.Save();
        }
        /// <summary>
        /// Xóa 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            CommandLevelDA.Delete(id);
            CommandLevelDA.Save();
        }
    }
}
