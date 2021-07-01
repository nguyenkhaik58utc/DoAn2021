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
    public class ProductionPerformSV
    {
        private ProductionPerformDA ProductionPerformDA = new ProductionPerformDA();
        /// <summary>
        /// Lấy danh sách 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<ProductionPerformItem> GetAll(ModelFilter filter)
        {
            var dbo = ProductionPerformDA.Repository;
            var ProductionPerform = ProductionPerformDA.GetQuery()
                        .Select(item => new ProductionPerformItem()
                        {
                            ID = item.ID,
                            HumanEmployee = new HumanEmployeeViewItem()
                            {
                                ID = item.HumanEmployeeID,
                                Name = dbo.HumanEmployees.Where(i => i.ID == item.HumanEmployeeID).Select(i => i.Name).FirstOrDefault()
                            },
                            HumanDepartment = new HumanDepartmentViewItem()
                            {
                                ID = item.HumanDepartmentID,
                                Name = dbo.HumanDepartments.Where(i => i.ID == item.HumanDepartmentID).Select(i => i.Name).FirstOrDefault()
                            },
                            ProductionShiftID = item.ProductionShiftID,
                            ShiftName = item.ProductionShift.Name,
                            ProductionCommandID = item.ProductionCommandID,
                            Date = item.Date,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            return ProductionPerform;
        }
        public List<ProductionPerformItem> GetByCommand(ModelFilter filter, int commandId)
        {
            var dbo = ProductionPerformDA.Repository;
            var ProductionPerform = ProductionPerformDA.GetQuery(i => i.ProductionCommandID == commandId)
                        .Select(item => new ProductionPerformItem()
                        {
                            ID = item.ID,
                            HumanEmployee = new HumanEmployeeViewItem()
                            {
                                ID = item.HumanEmployeeID,
                                Name = dbo.HumanEmployees.Where(i => i.ID == item.HumanEmployeeID).Select(i => i.Name).FirstOrDefault()
                            },
                            HumanDepartment = new HumanDepartmentViewItem()
                            {
                                ID = item.HumanDepartmentID,
                                Name = dbo.HumanDepartments.Where(i => i.ID == item.HumanDepartmentID).Select(i => i.Name).FirstOrDefault()
                            },
                            ProductionShiftID = item.ProductionShiftID,
                            ShiftName = item.ProductionShift.Name,
                            ProductionCommandID = item.ProductionCommandID,
                            Date = item.Date,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            return ProductionPerform;
        }
        /// <summary>
        /// Lấy  theo ID
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ProductionPerformItem GetById(int Id)
        {
            var dbo = ProductionPerformDA.Repository;
            var ProductionPerform = ProductionPerformDA.GetQuery(i => i.ID == Id)
                        .Select(item => new ProductionPerformItem()
                        {
                            ID = item.ID,
                            HumanEmployee = new HumanEmployeeViewItem()
                            {
                                ID = item.HumanEmployeeID,
                                Name = dbo.HumanEmployees.Where(i => i.ID == item.HumanEmployeeID).Select(i => i.Name).FirstOrDefault()
                            },
                            HumanDepartment = new HumanDepartmentViewItem()
                            {
                                ID = item.HumanDepartmentID,
                                Name = dbo.HumanDepartments.Where(i => i.ID == item.HumanDepartmentID).Select(i => i.Name).FirstOrDefault()
                            },
                            ProductionShiftID = item.ProductionShiftID,
                            ShiftName = item.ProductionShift.Name,
                            CommandCode = item.ProductionCommand.Code,
                            ProductionCommandID = item.ProductionCommandID,
                            Date = item.Date,
                        })
                        .First();
            return ProductionPerform;
        }

        /// <summary>
        /// Cập nhật 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(ProductionPerformItem item, int userID)
        {
            var dbo = ProductionPerformDA.Repository;
            var perform = ProductionPerformDA.GetById(item.ID);
            perform.HumanEmployeeID = item.HumanEmployee.ID;
            perform.ProductionShiftID = item.ProductionShiftID;
            perform.ProductionCommandID = item.ProductionCommandID;
            perform.Date = item.Date;
            perform.ID = item.ID;
            perform.UpdateAt = DateTime.Now;
            perform.UpdateBy = userID;
            ProductionPerformDA.Save();
        }
        /// <summary>
        /// Thêm mới 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(ProductionPerformItem item, int userID)
        {
            var ProductionPerform = new ProductionPerform()
            {
                HumanEmployeeID = item.HumanEmployee.ID,
                HumanDepartmentID = item.HumanDepartment.ID,
                ProductionShiftID = item.ProductionShiftID,
                ProductionCommandID = item.ProductionCommandID,
                Date = item.Date,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            ProductionPerformDA.Insert(ProductionPerform);
            ProductionPerformDA.Save();
        }
        /// <summary>
        /// Xóa 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            ProductionPerformDA.Delete(id);
            ProductionPerformDA.Save();
        }
        public void InsertProductEmployee(int id, int userId)
        {
            var dbo = ProductionPerformDA.Repository;
            var datetime = DateTime.Now;
            var departmentId = ProductionPerformDA.GetQuery(i => i.ID == id).Select(i => i.HumanDepartmentID).FirstOrDefault();
            var employeeIds = dbo.HumanRoles.Where(i => i.HumanDepartmentID == departmentId)
                .SelectMany(i => i.HumanOrganizations).Distinct()
                .Select(i => i.HumanEmployeeID).ToList();
            if (employeeIds.Count > 0)
            {
                var insertItems = new List<ProductionPerformResult>();
                var employeeExitIds = dbo.ProductionPerformResults.Where(i => i.ProductionPerformID == id && employeeIds.Contains(i.HumanEmployeeID))
                                    .Select(i => i.HumanEmployeeID);
                insertItems = employeeIds.Where(i => !employeeExitIds.Contains(i))
                    .Select(item => new ProductionPerformResult()
                    {
                        ProductionPerformID = id,
                        HumanEmployeeID = item,
                        CreateBy = userId,
                        CreateAt = datetime
                    }).ToList();
                dbo.ProductionPerformResults.AddRange(insertItems);
                dbo.SaveChanges();
            }

        }

        public List<ProductionPerformResultItem> GetResult(ModelFilter filter, int id)
        {
            var dbo = ProductionPerformDA.Repository;
            return dbo.ProductionPerformResults.Where(i => i.ProductionPerformID == id)
                .Select(item => new ProductionPerformResultItem()
                                        {
                                            ID = item.ID,
                                            HumanEmployee = new HumanEmployeeViewItem()
                                            {
                                                ID = item.HumanEmployeeID,
                                                Name = dbo.HumanEmployees.Where(i => i.ID == item.HumanEmployeeID).Select(i => i.Name).FirstOrDefault()
                                            },
                                            Quantity = item.Quantity,
                                            MaterialNumber = item.MaterialNumber,
                                            MaterialExitsNumber = item.MaterialExitsNumber,
                                            IsAbsent = item.IsAbsent,
                                            CreateAt = item.CreateAt
                                        }
                )
                .OrderBy(i => i.CreateAt)
                .Filter(filter)
                .ToList();
        }

        public void UpdateResult(ProductionPerformResultItem performResult, int userId)
        {
            var dbo = ProductionPerformDA.Repository;
            var updateResult = dbo.ProductionPerformResults.Where(i => i.ID == performResult.ID).FirstOrDefault();
            updateResult.Quantity = performResult.Quantity;
            updateResult.MaterialNumber = performResult.MaterialNumber;
            updateResult.MaterialExitsNumber = performResult.MaterialExitsNumber;
            updateResult.IsAbsent = performResult.IsAbsent;
            dbo.SaveChanges();
        }
    }
}
