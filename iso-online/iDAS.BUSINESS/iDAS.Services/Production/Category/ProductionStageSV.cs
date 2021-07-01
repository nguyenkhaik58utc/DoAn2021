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
    public class ProductionStageSV
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
                            Product = new ProductViewItem() { ID = item.StockProductID, Name = item.StockProduct.Name },
                            ResultProduct =
                             new ProductViewItem()
                             {
                                 ID = dbo.StockProducts.Where(i => i.ID == item.ResultProductID).Select(i => i.ID).FirstOrDefault(),
                                 Name = dbo.StockProducts.Where(i => i.ID == item.ResultProductID).Select(i => i.Name).FirstOrDefault()
                             },
                            ResultQuantity = item.ResultQuantity,
                            MenDay = item.MenDay,
                            Position = item.Position,
                            IsActive = item.IsActive,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            return ProductionStage;
        }
        public List<ProductionStageItem> GetByProduct(int productId)
        {
            var dbo = ProductionStageDA.Repository;
            var ProductionStage = ProductionStageDA.GetQuery(i => i.StockProductID == productId)
                        .Select(item => new ProductionStageItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Position = item.Position,
                        })
                        .OrderByDescending(item => item.Position)
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
            var dbo = ProductionStageDA.Repository;
            var ProductionStage = ProductionStageDA.GetQuery(i => i.ID == Id)
                        .Select(item => new ProductionStageItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Product = new ProductViewItem() { ID = item.StockProductID, Name = item.StockProduct.Name },
                            ResultProduct =
                            new ProductViewItem()
                            {
                                ID = dbo.StockProducts.Where(i => i.ID == item.ResultProductID).Select(i => i.ID).FirstOrDefault(),
                                Name = dbo.StockProducts.Where(i => i.ID == item.ResultProductID).Select(i => i.Name).FirstOrDefault()
                            },
                            Quantity = item.Quantity,
                            ResultQuantity = item.ResultQuantity,
                            MenDay = item.MenDay,
                            Position = item.Position,
                            Note = item.Note,
                            IsActive = item.IsActive,
                        })
                        .First();
            ProductionStage.Equipements = dbo.ProductionStageEquipments
                                                .Where(i => i.ProductionStageID == ProductionStage.ID)
                                                .Select(item =>
                                                    new ProductionStageEquipmentItem()
                                                        {
                                                            ID = item.ID,
                                                            EquipmentProductionCategoryID = item.EquipmentProductionCategoryID,
                                                            EquipementName = item.EquipmentProductionCategory.Name,
                                                            Quantity = item.Quantity
                                                        })
                                                .ToList();
            ProductionStage.Materials = dbo.ProductionStageMaterials
                                                .Where(i => i.ProductionStageID == ProductionStage.ID)
                                                .Select(item =>
                                                    new ProductionStageMaterialItem()
                                                    {
                                                        ID = item.ID,
                                                        StockMaterialID = item.StockProductID,
                                                        MaterialName = item.StockProduct.Name,
                                                        Quantity = item.Quantity
                                                    })
                                                .ToList();
            ProductionStage.Products = dbo.ProductionStageProducts
                                                .Where(i => i.ProductionStageID == ProductionStage.ID)
                                                .Select(item =>
                                                    new ProductionStageProductItem()
                                                    {
                                                        ID = item.ID,
                                                        StockProductID = item.StockProductID,
                                                        ProductName = item.StockProduct.Name,
                                                        Quantity = item.Quantity
                                                    })
                                                .ToList();
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
            ProductionStage.Name = item.Name;
            ProductionStage.StockProductID = item.Product.ID;
            ProductionStage.Quantity = item.Quantity;
            if (item.ResultProduct.ID != 0) { ProductionStage.ResultProductID = item.ResultProduct.ID; }
            ProductionStage.ResultQuantity = item.ResultQuantity;
            ProductionStage.MenDay = item.MenDay;
            ProductionStage.Position = item.Position;
            ProductionStage.Note = item.Note;
            ProductionStage.IsActive = item.IsActive;
            ProductionStage.UpdateAt = DateTime.Now;
            ProductionStage.UpdateBy = userID;
            ProductionStageDA.Save();
            var dbo = ProductionStageDA.Repository;
            // thiết bị
            if (item.Equipements.Count > 0)
            {
                dbo.ProductionStageEquipments.RemoveRange(dbo.ProductionStageEquipments.Where(i => i.ProductionStageID == ProductionStage.ID));
                dbo.ProductionStageEquipments.AddRange(
                    item.Equipements.Select(e =>
                        new ProductionStageEquipment()
                        {
                            EquipmentProductionCategoryID = e.EquipmentProductionCategoryID,
                            ProductionStageID = ProductionStage.ID,
                            Quantity = e.Quantity,
                        }));
            }
            // Nguyên vật liệu
            if (item.Materials.Count > 0)
            {
                dbo.ProductionStageMaterials.RemoveRange(dbo.ProductionStageMaterials.Where(i => i.ProductionStageID == ProductionStage.ID));
                dbo.ProductionStageMaterials.AddRange(
                    item.Materials.Select(e =>
                        new ProductionStageMaterial()
                        {
                            StockProductID = e.StockMaterialID,
                            ProductionStageID = ProductionStage.ID,
                            Quantity = e.Quantity,
                        }));
            }
            if (item.Products.Count > 0)
            {
                dbo.ProductionStageProducts.RemoveRange(dbo.ProductionStageProducts.Where(i => i.ProductionStageID == ProductionStage.ID));
                dbo.ProductionStageProducts.AddRange(
                    item.Products.Select(e =>
                        new ProductionStageProduct()
                        {
                            StockProductID = e.StockProductID,
                            ProductionStageID = ProductionStage.ID,
                            Quantity = e.Quantity,
                        }));
            }
            dbo.SaveChanges();
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
                StockProductID = item.Product.ID,
                Quantity = item.Quantity,
                ResultQuantity = item.ResultQuantity,
                MenDay = item.MenDay,
                Position = item.Position,
                Note = item.Note,
                IsActive = item.IsActive,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            if (item.ResultProduct.ID != 0) { ProductionStage.ResultProductID = item.ResultProduct.ID; }
            ProductionStageDA.Insert(ProductionStage);
            ProductionStageDA.Save();
            var dbo = ProductionStageDA.Repository;
            // Thêm mới thiết bị
            if (item.Equipements.Count > 0)
            {
                dbo.ProductionStageEquipments.AddRange(
                    item.Equipements.Select(e =>
                        new ProductionStageEquipment()
                        {
                            EquipmentProductionCategoryID = e.EquipmentProductionCategoryID,
                            ProductionStageID = ProductionStage.ID,
                            Quantity = e.Quantity,
                        }));
            }
            // Nguyên vật liệu
            if (item.Materials.Count > 0)
            {
                dbo.ProductionStageMaterials.AddRange(
                    item.Materials.Select(e =>
                        new ProductionStageMaterial()
                        {
                            StockProductID = e.StockMaterialID,
                            ProductionStageID = ProductionStage.ID,
                            Quantity = e.Quantity,
                        }));
            }
            if (item.Products.Count > 0)
            {
                dbo.ProductionStageProducts.AddRange(
                    item.Products.Select(e =>
                        new ProductionStageProduct()
                        {
                            StockProductID = e.StockProductID,
                            ProductionStageID = ProductionStage.ID,
                            Quantity = e.Quantity,
                        }));
            }
            dbo.SaveChanges();
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
        public List<ProductionStageMaterialItem> GetMaterial(ModelFilter filter)
        {
            var dbo = ProductionStageDA.Repository;
            var result = dbo.ProductionStageMaterials
                                .Select(item => new ProductionStageMaterialItem()
                                {
                                    ID = item.ID,
                                    MaterialName = item.StockProduct.Name,
                                    CreateAt = item.CreateAt
                                }).Filter(filter).ToList();
            return result;
        }
    }
}
