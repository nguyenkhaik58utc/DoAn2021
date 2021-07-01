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
    public class ProductionPlanSV
    {
        private ProductionPlanDA ProductionPlanDA = new ProductionPlanDA();
        /// <summary>
        /// Lấy danh sách 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<ProductionPlanItem> GetAll(ModelFilter filter)
        {
            var dbo = ProductionPlanDA.Repository;
            var ProductionPlan = ProductionPlanDA.GetQuery()
                        .Select(item => new ProductionPlanItem()
                        {
                            ID = item.ID,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Product = new ProductViewItem()
                            {
                                Name = dbo.StockProducts.Where(i => i.ID == item.ProductionRequirement.StockProductID)
                                            .Select(i => i.Name).FirstOrDefault(),
                            },
                            Quantity = item.Quantity,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            return ProductionPlan;
        }
        public List<ProductionPlanItem> GetByRequirement(ModelFilter filter, int requirementId)
        {
            var dbo = ProductionPlanDA.Repository;
            var ProductionPlan = ProductionPlanDA.GetQuery(i => i.ProductionRequirementID == requirementId)
                        .Select(item => new ProductionPlanItem()
                        {
                            ID = item.ID,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Product = new ProductViewItem()
                            {
                                Name = dbo.StockProducts.Where(i => i.ID == item.ProductionRequirement.StockProductID)
                                            .Select(i => i.Name).FirstOrDefault(),
                            },
                            IsDoing = dbo.ProductionCommands.Where(i => i.ProductionPlanID == item.ID).FirstOrDefault() != null,
                            IsFinish = dbo.ProductionCommands.Where(i => i.ProductionPlanID == item.ID).FirstOrDefault() != null
                            && dbo.ProductionCommands.Where(i => !i.IsFinish).FirstOrDefault() == null,
                            Quantity = item.Quantity,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            return ProductionPlan;
        }
        /// <summary>
        /// Lấy  theo ID
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ProductionPlanItem GetById(int Id)
        {
            var dbo = ProductionPlanDA.Repository;
            var ProductionPlan = ProductionPlanDA.GetQuery(i => i.ID == Id)
                        .Select(item => new ProductionPlanItem()
                        {
                            ID = item.ID,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            ProductionRequirement = new ProductionRequirementViewItem()
                            {
                                ID = item.ProductionRequirementID,
                                Quantity = item.ProductionRequirement.Quantity,
                                EndTime = item.ProductionRequirement.EndTime,
                                Level = item.ProductionRequirement.ProductionLevel.Name,
                                Color = item.ProductionRequirement.ProductionLevel.Color
                            },
                            Product = new ProductViewItem()
                            {
                                ID = item.ProductionRequirement.StockProductID,
                                Name = dbo.StockProducts.Where(i => i.ID == item.ProductionRequirement.StockProductID)
                                            .Select(i => i.Name).FirstOrDefault(),
                            },
                            Quantity = item.Quantity,
                            Note = item.Note,
                        })
                        .First();
            return ProductionPlan;
        }
        /// <summary>
        /// Cập nhật 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(ProductionPlanItem item, int userID)
        {
            var plan = ProductionPlanDA.GetById(item.ID);
            plan.ID = item.ID;
            plan.StartTime = item.StartTime;
            plan.EndTime = item.EndTime;
            plan.Quantity = item.Quantity;
            plan.Note = item.Note;
            plan.UpdateAt = DateTime.Now;
            plan.UpdateBy = userID;
            ProductionPlanDA.Save();
        }
        /// <summary>
        /// Thêm mới 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public int Insert(ProductionPlanItem item, int userID)
        {
            var plan = new ProductionPlan()
            {
                ProductionRequirementID = item.ProductionRequirement.ID,
                Quantity = item.Quantity,
                StartTime = item.StartTime,
                EndTime = item.EndTime,
                Note = item.Note,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            ProductionPlanDA.Insert(plan);
            ProductionPlanDA.Save();
            return plan.ID;
        }
        /// <summary>
        /// Xóa 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            ProductionPlanDA.Delete(id);
            ProductionPlanDA.Save();
        }
        public List<ProductionPlanProductItem> GetProducResult(ModelFilter filter, int planId)
        {
            var dbo = ProductionPlanDA.Repository;
            var result = dbo.ProductionPlanProducts.Where(i => i.ProductionPlanID == planId)
                        .Select(item => new ProductionPlanProductItem()
                        {
                            ID = item.ID,
                            ProductResult = new ProductViewItem()
                            {
                                ID = item.StockProductID,
                                Name = item.StockProduct.Name
                            },
                            StageID = dbo.ProductionStages.Where(i => i.StockProductID == item.ProductionPlan.ProductionRequirement.StockProductID
                                                && i.ResultProductID == item.StockProductID).Select(i => i.ID).FirstOrDefault(),
                            StageName = dbo.ProductionStages.Where(i => i.StockProductID == item.ProductionPlan.ProductionRequirement.StockProductID
                                                && i.ResultProductID == item.StockProductID).Select(i => i.Name).FirstOrDefault(),
                            QuantityCalculator = item.QuantityCalculator,
                            QuantityStock = dbo.StockProducts.Where(i => i.ID == item.StockProductID).Select(i => (int)i.Quantity).FirstOrDefault(),
                            Quantity = item.Quantity,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            return result;
        }
        public void CalculatorStage(int planId, int productionId, int quantity)
        {
            var dbo = ProductionPlanDA.Repository;
            var stage = dbo.ProductionStages.Where(i => i.StockProductID == productionId).ToList();
            if (stage.Count > 0)
            {
                var planProduction = stage.Select(item => new ProductionPlanProduct()
                {
                    StockProductID = item.ResultProductID,
                    ProductionPlanID = planId,
                    QuantityCalculator = item.Quantity * quantity,
                    Quantity = item.Quantity * quantity
                }).ToList();
                dbo.ProductionPlanProducts.RemoveRange(dbo.ProductionPlanProducts.Where(i => i.ProductionPlanID == planId));
                dbo.ProductionPlanProducts.AddRange(planProduction);
                dbo.SaveChanges();
            }
        }
        public List<ProductionPlanProductDetailItem> GetPlanStage(int planProductId)
        {
            var dbo = ProductionPlanDA.Repository;
            var planProductDetails = dbo.ProductionPlanProductDetails.Where(i => i.ProductionPlanProductID == planProductId)
                            .Select(item => new ProductionPlanProductDetailItem()
                            {
                                ID = item.ID,
                                HumanDepartment = new HumanDepartmentViewItem()
                                {
                                    ID = item.HumanDepartmentID == null ? 0 : (int)item.HumanDepartmentID,
                                    Name = dbo.HumanDepartments.Where(i => i.ID == item.HumanDepartmentID).Select(i => i.Name).FirstOrDefault()
                                },
                                EmployeeCount = dbo.HumanRoles.Where(i => i.HumanDepartmentID == item.HumanDepartmentID)
                                                                .SelectMany(i => i.HumanOrganizations).Distinct().Count(),
                                CalculatorQuantity = item.CalculatorQuantity,
                                Quantity = item.Quantity,
                                CalculatorMenday = item.CalculatorMenday,
                                Menday = item.Menday,
                                Date = item.Date,
                            })
                        .ToList();
            return planProductDetails;
        }
        public void CaculatorPlan(int planProductId, int stageId, int quality, DateTime startDate, DateTime endDate, int departmentId, int userId)
        {
            var days = endDate.Subtract(startDate).Days + 1;
            if (days > 0)
            {
                var dbo = ProductionPlanDA.Repository;
                var planQuality = quality / days;
                var endQuality = quality % days + planQuality;
                var planDetail = new List<ProductionPlanProductDetail>();
                var stage = dbo.ProductionStages.Where(i => i.ID == stageId).FirstOrDefault();
                var mendays = quality * stage.MenDay / stage.ResultQuantity;
                var planMendays = mendays / days;
                for (var i = 0; i < days; i++)
                {
                    if (i == days - 1) planQuality = endQuality;
                    planDetail.Add(
                        new ProductionPlanProductDetail()
                        {
                            ProductionPlanProductID = planProductId,
                            Date = startDate.AddDays(i),
                            HumanDepartmentID = departmentId == 0 ? null : (int?)departmentId,
                            Quantity = planQuality,
                            CalculatorQuantity = planQuality,
                            Menday = planMendays,
                            CalculatorMenday = planMendays,
                            CreateAt = DateTime.Now,
                            CreateBy = userId
                        }
                        );
                }
                if (planDetail.Count > 0)
                {
                    dbo.ProductionPlanProductDetails.RemoveRange(dbo.ProductionPlanProductDetails.Where(i => i.ProductionPlanProductID == planProductId));
                    dbo.ProductionPlanProductDetails.AddRange(planDetail);
                    dbo.SaveChanges();
                }
            }
        }

        public List<ProductionPlanItem> GetPlanForCommand(int requirementId, int departmentId)
        {
            var dbo = ProductionPlanDA.Repository;
            var ProductionPlan = dbo.ProductionPlans.Where(i => i.ProductionRequirementID == requirementId)
                        .Select(item => new ProductionPlanItem()
                        {
                            ID = item.ID,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            CreateAt = item.CreateAt
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .ToList();
            return ProductionPlan;
        }

        public ProductionPlanProductItem GetSemiProductById(int id)
        {
            var dbo = ProductionPlanDA.Repository;
            return dbo.ProductionPlanProducts.Where(i => i.ID == id).Select(item => new ProductionPlanProductItem()
                {
                    ID = item.ID,
                    StageName = dbo.ProductionStages.Where(i => i.StockProductID == item.ProductionPlan.ProductionRequirement.StockProductID
                                               && i.ResultProductID == item.StockProductID).Select(i => i.Name).FirstOrDefault(),
                    ProductResult = new ProductViewItem()
                    {
                        ID = item.StockProductID,
                        Name = item.StockProduct.Name
                    },
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    QuantityCalculator = item.QuantityCalculator,
                    Quantity = item.Quantity,
                }).FirstOrDefault();
        }

        public void UpdateSemiProduct(ProductionPlanProductItem data, int userId)
        {
            var dbo = ProductionPlanDA.Repository;
            var item = dbo.ProductionPlanProducts.Where(i => i.ID == data.ID).FirstOrDefault();
            item.StartTime = data.StartTime;
            item.EndTime = data.EndTime;
            item.Quantity = data.Quantity;
            item.UpdateBy = userId;
            item.UpdateAt = DateTime.Now;
            dbo.SaveChanges();
        }

        public void DeleteSemiProduct(int id)
        {
            var dbo = ProductionPlanDA.Repository;
            dbo.ProductionPlanProducts.Remove(dbo.ProductionPlanProducts.Where(i => i.ID == id).FirstOrDefault());
            dbo.SaveChanges();
        }

        public ProductionPlanEquipmentItem GetEquipmentById(int id)
        {
            var dbo = ProductionPlanDA.Repository;
            return dbo.ProductionPlanEquipments.Where(i => i.ID == id).Select(item => new ProductionPlanEquipmentItem()
            {
                ID = item.ID,
                ProductionPlanID = item.ProductionPlanID,
                EquipmentProduction = new EquipmentViewItem()
                {
                    ID = item.EquipmentProductionID,
                    Name = item.EquipmentProduction.Name,
                },
                Quantity = item.Quantity,
                Note = item.Note,
            }).FirstOrDefault();
        }
        public List<ProductionPlanEquipmentItem> GetEquipment(int planId)
        {
            var dbo = ProductionPlanDA.Repository;
            return
                dbo.ProductionPlanEquipments.Where(i => i.ProductionPlanID == planId)
                    .Select(item => new ProductionPlanEquipmentItem
                    {
                        ID = item.ID,
                        EquipmentProduction = new EquipmentViewItem()
                        {
                            ID = item.EquipmentProductionID,
                            Name = item.EquipmentProduction.Name
                        },
                        Quantity = item.Quantity

                    }).ToList();
        }
        public void InsertEquipment(ProductionPlanEquipmentItem item, int userId)
        {
            var dbo = ProductionPlanDA.Repository;
            var data = new ProductionPlanEquipment()
            {
                ProductionPlanID = item.ProductionPlanID,
                EquipmentProductionID = item.EquipmentProduction.ID,
                Quantity = item.Quantity,
                Note = item.Note,
                CreateAt = DateTime.Now,
                CreateBy = userId,
            };
            dbo.ProductionPlanEquipments.Add(data);
            dbo.SaveChanges();
        }
        public void UpdateEquipment(ProductionPlanEquipmentItem item, int userId)
        {
            var dbo = ProductionPlanDA.Repository;
            var data = dbo.ProductionPlanEquipments.Where(i => i.ID == item.ID).FirstOrDefault();
            data.EquipmentProductionID = item.EquipmentProduction.ID;
            data.Quantity = item.Quantity;
            data.Note = item.Note;
            data.UpdateBy = userId;
            data.UpdateAt = DateTime.Now;
            dbo.SaveChanges();
        }
        public void DeleteEquipment(int id)
        {
            var dbo = ProductionPlanDA.Repository;
            dbo.ProductionPlanEquipments.Remove(dbo.ProductionPlanEquipments.Where(i => i.ID == id).FirstOrDefault());
            dbo.SaveChanges();
        }
        public ProductionPlanMaterialItem GetMaterialById(int id)
        {
            var dbo = ProductionPlanDA.Repository;
            return dbo.ProductionPlanMaterials.Where(i => i.ID == id).Select(item => new ProductionPlanMaterialItem()
            {
                ID = item.ID,
                ProductionPlanID = item.ProductionPlanID,
                StockMaterialID = item.StockProductID,
                MaterialName = item.StockProduct.Name,
                Quantity = item.Quantity,
                Note = item.Note,
            }).FirstOrDefault();
        }
        public void InsertMaterial(ProductionPlanMaterialItem item, int userId)
        {
            var dbo = ProductionPlanDA.Repository;
            var data = new ProductionPlanMaterial()
            {
                ProductionPlanID = item.ProductionPlanID,
                StockProductID = item.StockMaterialID,
                Quantity = item.Quantity,
                Note = item.Note,
                CreateAt = DateTime.Now,
                CreateBy = userId,
            };
            dbo.ProductionPlanMaterials.Add(data);
            dbo.SaveChanges();
        }
        public void UpdateMaterial(ProductionPlanMaterialItem item, int userId)
        {
            var dbo = ProductionPlanDA.Repository;
            var data = dbo.ProductionPlanMaterials.Where(i => i.ID == item.ID).FirstOrDefault();
            data.StockProductID = item.StockMaterialID;
            data.Quantity = item.Quantity;
            data.Note = item.Note;
            data.UpdateBy = userId;
            data.UpdateAt = DateTime.Now;
            dbo.SaveChanges();
        }
        public void DeleteMaterial(int id)
        {
            var dbo = ProductionPlanDA.Repository;
            dbo.ProductionPlanMaterials.Remove(dbo.ProductionPlanMaterials.Where(i => i.ID == id).FirstOrDefault());
            dbo.SaveChanges();
        }
        public List<ProductionPlanMaterialItem> GetMaterial(int planId)
        {
            var dbo = ProductionPlanDA.Repository;
            return
                dbo.ProductionPlanMaterials.Where(i => i.ProductionPlanID == planId)
                    .Select(item => new ProductionPlanMaterialItem
                    {
                        ID = item.ID,
                        StockMaterialID = item.StockProductID,
                        MaterialName = item.StockProduct.Name,
                        Quantity = item.Quantity
                    }).ToList();
        }
    }
}
