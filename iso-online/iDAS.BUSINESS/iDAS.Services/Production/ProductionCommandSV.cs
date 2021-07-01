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
    public class ProductionCommandSV
    {
        private ProductionCommandDA ProductionCommandDA = new ProductionCommandDA();
        /// <summary>
        /// Lấy danh sách 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<ProductionCommandItem> GetAll(ModelFilter filter)
        {
            var dbo = ProductionCommandDA.Repository;
            var ProductionCommand = ProductionCommandDA.GetQuery()
                        .Select(item => new ProductionCommandItem()
                        {
                            ID = item.ID,
                            Code = item.Code,
                            BatchCode = item.BatchCode,
                            HumanDepartment = new HumanDepartmentViewItem()
                            {
                                ID = item.HumanDepartmentID,
                                Name = item.HumanDepartment.Name
                            },
                            Quantity = item.Quantity,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            IsStart = item.IsStart,
                            IsPause = item.IsPause,
                            IsFinish = item.IsFinish,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            return ProductionCommand;
        }
        public List<ProductionCommandItem> GetByDepartment(ModelFilter filter, int departmentId)
        {
            var dbo = ProductionCommandDA.Repository;
            var ProductionCommand = ProductionCommandDA.GetQuery(i => i.HumanDepartmentID == departmentId)
                        .Select(item => new ProductionCommandItem()
                        {
                            ID = item.ID,
                            Code = item.Code,
                            BatchCode = item.BatchCode,
                            ProductionName = item.ProductionRequirement.StockProduct.Name,
                            Quantity = item.Quantity,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            IsStart = item.IsStart,
                            IsPause = item.IsPause,
                            IsFinish = item.IsFinish,
                            CreateAt = item.CreateAt
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            return ProductionCommand;
        }
        public List<ProductionPlanProductDetailItem> GetPlanByDepartment(ModelFilter filter, int departmentId)
        {
            var dbo = ProductionCommandDA.Repository;
            var today = DateTime.Today;
            var planProductDetails = dbo.ProductionPlanProductDetails.Where(i => i.HumanDepartmentID == departmentId && i.Date == today)
                           .Select(item => new ProductionPlanProductDetailItem()
                           {
                               ID = item.ID,
                               Level = item.ProductionPlanProduct.ProductionPlan.ProductionRequirement.ProductionLevel.Name,
                               Color = item.ProductionPlanProduct.ProductionPlan.ProductionRequirement.ProductionLevel.Color,
                               Menday = item.Menday,
                               Quantity = item.Quantity,
                               Date = item.Date,
                               CreateAt = item.CreateAt
                           })
                       .Filter(filter)
                       .ToList();
            return planProductDetails;
        }
        /// <summary>
        /// Lấy  theo ID
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ProductionCommandItem GetById(int Id)
        {
            var ProductionCommand = ProductionCommandDA.GetQuery(i => i.ID == Id)
                        .Select(item => new ProductionCommandItem()
                        {
                            ID = item.ID,
                            HumanDepartment = new HumanDepartmentViewItem()
                            {
                                ID = item.HumanDepartmentID,
                                Name = item.HumanDepartment.Name
                            },
                            Code = item.Code,
                            BatchCode = item.BatchCode,
                            ProductionRequirementID = item.ProductionRequirementID,
                            ProductionPlanID = item.ProductionPlanID,
                            ProductionStageID = item.ProductionStageID,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Quantity = item.Quantity,
                            Note = item.Note,
                            IsStart = item.IsStart,
                            IsPause = item.IsPause,
                            IsFinish = item.IsFinish,
                        })
                        .First();
            return ProductionCommand;
        }

        /// <summary>
        /// Cập nhật 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(ProductionCommandItem item, int userID)
        {
            var command = ProductionCommandDA.GetById(item.ID);
            command.ID = item.ID;
            command.HumanDepartmentID = item.HumanDepartment.ID;
            command.Code = item.Code;
            command.BatchCode = item.BatchCode;
            command.ProductionRequirementID = item.ProductionRequirementID;
            command.ProductionPlanID = item.ProductionPlanID;
            command.ProductionStageID = item.ProductionStageID;
            command.StartTime = item.StartTime;
            command.EndTime = item.EndTime;
            command.Note = item.Note;
            command.UpdateAt = DateTime.Now;
            command.UpdateBy = userID;
            ProductionCommandDA.Save();
        }
        /// <summary>
        /// Thêm mới 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(ProductionCommandItem item, int userID)
        {
            var ProductionCommand = new ProductionCommand()
            {
                HumanDepartmentID = item.HumanDepartment.ID,
                Code = item.Code,
                BatchCode = item.BatchCode,
                ProductionRequirementID = item.ProductionRequirementID,
                ProductionPlanID = item.ProductionPlanID,
                ProductionStageID = item.ProductionStageID,
                StartTime = item.StartTime,
                EndTime = item.EndTime,
                Quantity = item.Quantity,
                Note = item.Note,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            ProductionCommandDA.Insert(ProductionCommand);
            ProductionCommandDA.Save();
        }
        /// <summary>
        /// Xóa 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            ProductionCommandDA.Delete(id);
            ProductionCommandDA.Save();
        }

        public void Start(int id)
        {
            var command = ProductionCommandDA.GetQuery(i => i.ID == id).FirstOrDefault();
            command.IsStart = true;
            command.ProductionRequirement.IsNew = false;
            ProductionCommandDA.Save();
        }

        public void Pause(int id, bool isPause)
        {
            var command = ProductionCommandDA.GetQuery(i => i.ID == id).FirstOrDefault();
            command.IsStart = true;
            command.IsPause = isPause;
            ProductionCommandDA.Save();
        }

        public void Finish(int id, DateTime? finishTime)
        {
            var command = ProductionCommandDA.GetQuery(i => i.ID == id).FirstOrDefault();
            command.IsFinish = true;
            command.FinishTime = finishTime;
            ProductionCommandDA.Save();
        }
    }
}
