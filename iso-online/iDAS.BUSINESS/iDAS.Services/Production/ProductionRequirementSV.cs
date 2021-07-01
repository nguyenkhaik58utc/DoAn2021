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
    public class ProductionRequirementSV
    {
        private ProductionRequirementDA RequirementDA = new ProductionRequirementDA();
        /// <summary>
        /// Lấy danh sách 
        /// || Author: Thanhnv || CreateDate: 01/08/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<ProductionRequirementItem> GetAll(ModelFilter filter)
        {
            var dbo = RequirementDA.Repository;
            var requirements = RequirementDA.GetQuery()
                        .Select(item => new ProductionRequirementItem()
                        {
                            ID = item.ID,
                            HumanEmployee = new HumanEmployeeViewItem()
                            {
                                ID = item.HumanEmployeeID,
                                Name = dbo.HumanEmployees.Where(i => i.ID == item.HumanEmployeeID).Select(i => i.Name).FirstOrDefault()
                            },
                            ProductionLevel = new ProductionLevelItem()
                            {
                                Name = item.ProductionLevel.Name,
                                Color = item.ProductionLevel.Color
                            },
                            Product = new ProductViewItem()
                            {
                                ID = item.StockProductID,
                                Name = dbo.StockProducts.Where(i => i.ID == item.StockProductID).Select(i => i.Name).FirstOrDefault()
                            },
                            Quantity = item.Quantity,
                            FinishTime = item.FinishTime,
                            EndTime = item.EndTime,
                            IsNew = item.IsNew,
                            IsPause = item.IsPause,
                            IsFinish = item.IsFinish,
                            IsDelete = item.IsDelete,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            return requirements;
        }
        public List<ProductionRequirementItem> GetAll()
        {
            var dbo = RequirementDA.Repository;
            var productionRequirements = RequirementDA.GetQuery()
                        .Select(item => new ProductionRequirementItem()
                        {
                            ID = item.ID,
                            Product = new ProductViewItem()
                            {
                                ID = item.StockProductID,
                                Name = dbo.StockProducts.Where(i => i.ID == item.StockProductID).Select(i => i.Name).FirstOrDefault()
                            },
                            EndTime = item.EndTime,
                            Quantity = item.Quantity,
                            CreateAt = item.CreateAt,
                        });
            var result = new List<ProductionRequirementItem>();
            foreach (var item in productionRequirements)
            {
                var name = "Yêu cầu sản xuất "
                                       + item.Product.Name
                                       + ". Số lượng:" + item.Quantity + ". Ngày hoàn thành: "
                                       + item.EndTime.ToString("dd/MM/yyyy");
                item.Name = name;
                result.Add(item);
            }
            return result.OrderByDescending(item => item.CreateAt).ToList();
        }
        /// <summary>
        /// Lấy  theo ID
        /// || Author: Thanhnv || CreateDate: 01/08/2015  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ProductionRequirementItem GetById(int Id)
        {
            var dbo = RequirementDA.Repository;
            var ProductionCommandRequirement = RequirementDA.GetQuery(i => i.ID == Id)
                        .Select(item => new ProductionRequirementItem()
                        {
                            ID = item.ID,
                            HumanEmployee = new HumanEmployeeViewItem()
                            {
                                ID = item.HumanEmployeeID,
                                Name = dbo.HumanEmployees.Where(i => i.ID == item.HumanEmployeeID).Select(i => i.Name).FirstOrDefault(),
                                Role = dbo.HumanOrganizations.Where(i => i.HumanEmployeeID == item.HumanEmployeeID).Select(i => i.HumanRole.Name).FirstOrDefault(),
                                Department = dbo.HumanOrganizations.Where(i => i.HumanEmployeeID == item.HumanEmployeeID).Select(i => i.HumanRole.HumanDepartment.Name).FirstOrDefault(),
                            },
                            ProductionLevel = new ProductionLevelItem()
                            {
                                ID = (int)item.ProductionLevelID,
                                Name = item.ProductionLevel.Name,
                                Color = item.ProductionLevel.Color
                            },
                            Product = new ProductViewItem()
                            {
                                ID = item.StockProductID,
                                Name = dbo.StockProducts.Where(i => i.ID == item.StockProductID).Select(i => i.Name).FirstOrDefault()
                            },
                            IsNew = item.IsNew,
                            Quantity = item.Quantity,
                            EndTime = item.EndTime,
                            FinishTime = item.FinishTime,
                            Note = item.Note,
                            IsDelete = item.IsDelete,
                        })
                        .First();
            return ProductionCommandRequirement;
        }
        /// <summary>
        /// Cập nhật 
        /// || Author: Thanhnv || CreateDate: 01/08/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(ProductionRequirementItem item, int userID)
        {
            var Requirement = RequirementDA.GetQuery(i => i.ID == item.ID).First();
            Requirement.ID = item.ID;
            Requirement.HumanEmployeeID = item.HumanEmployee.ID;
            Requirement.ProductionLevelID = item.ProductionLevel.ID;
            Requirement.StockProductID = item.Product.ID;
            Requirement.Quantity = item.Quantity;
            Requirement.EndTime = item.EndTime;
            Requirement.FinishTime = item.FinishTime;
            Requirement.Note = item.Note;
            Requirement.IsDelete = item.IsDelete;
            Requirement.UpdateAt = DateTime.Now;
            Requirement.UpdateBy = userID;
            RequirementDA.Save();
        }
        /// <summary>
        /// Thêm mới 
        /// || Author: Thanhnv || CreateDate: 01/08/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(ProductionRequirementItem item, int userID)
        {
            var ProductionCommandRequirement = new ProductionRequirement()
            {
                HumanEmployeeID = item.HumanEmployee.ID,
                ProductionLevelID = item.ProductionLevel.ID,
                StockProductID = item.Product.ID,
                Quantity = item.Quantity,
                EndTime = item.EndTime,
                FinishTime = item.FinishTime,
                Note = item.Note,
                IsNew = item.IsNew,
                IsDelete = item.IsDelete,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            RequirementDA.Insert(ProductionCommandRequirement);
            RequirementDA.Save();
        }
        public int InsertFormDevelopmentProduction(ProductionRequirementItem item, int userID)
        {
            var ProductionCommandRequirement = new ProductionRequirement()
            {
                HumanEmployeeID = item.HumanEmployee.ID,
                ProductionLevelID = item.ProductionLevel.ID,
                StockProductID = item.Product.ID,
                Quantity = item.Quantity,
                IsNew = true,
                EndTime = item.EndTime,
                FinishTime = item.FinishTime,
                Note = item.Note,
                IsDelete = item.IsDelete,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            RequirementDA.Insert(ProductionCommandRequirement);
            RequirementDA.Save();
            return ProductionCommandRequirement.ID;
        }
        /// <summary>
        /// Xóa 
        /// || Author: Thanhnv || CreateDate: 01/08/2015  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            RequirementDA.Delete(id);
            RequirementDA.Save();
        }

        public void Pause(int id, bool isPause = true)
        {
            var requirement = RequirementDA.GetQuery(i => i.ID == id).First();
            requirement.IsPause = isPause;
            RequirementDA.Save();
        }
        public void Doing(int id)
        {
            var requirement = RequirementDA.GetQuery(i => i.ID == id).First();
            requirement.IsNew = false;
            RequirementDA.Save();
        }
        public void Finish(int id, DateTime? finishTime)
        {
            var requirement = RequirementDA.GetQuery(i => i.ID == id).First();
            requirement.IsFinish = true;
            requirement.FinishTime = finishTime;
            RequirementDA.Save();
        }
    }
}
