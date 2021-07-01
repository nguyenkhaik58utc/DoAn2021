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
    public class CustomerCareResultSV
    {
        private CustomerCareResultDA CustomerCareResultDA = new CustomerCareResultDA();
        /// <summary>
        /// Lấy danh sách kết quả chăm sóc
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CustomerCareResultItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var CustomerCareResult = CustomerCareResultDA.GetQuery()
                        .Select(item => new CustomerCareResultItem()
                        {
                            ID = item.ID,
                            Method = item.Method,
                            CustomerID = item.CustomerID,
                            CareID = item.CustomerCareID,
                            IsSuccess = item.IsSuccess,
                            Note = item.Note,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return CustomerCareResult;
        }
        /// <summary>
        /// Lấy danh sách kết quả chăm sóc của khách hàng
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public List<CustomerCareResultItem> GetByCustomer(int page, int pageSize, out int totalCount, int CustomerID)
        {
            var CustomerCareResult = CustomerCareResultDA.GetQuery(i => i.CustomerID == CustomerID)
                        .Select(item => new CustomerCareResultItem()
                        {
                            ID = item.ID,
                            CustomerID = item.CustomerID,
                            Method = item.Method,
                            // Thông tin của khách hàng
                            //Image = item.CustomerID.Image,
                            //Name = item.Customers.Name,
                            //Phone = item.Customer.Phone,
                            //Email = item.Customer.Email,
                            //Address = item.Customer.Address,
                            ////Type = item.Customer.Type,
                            //CareID = item.CareID,
                            CareName = item.CustomerCare.Name,
                            IsPause = item.CustomerCare.IsPause,
                            IsFinish = item.CustomerCare.IsFinish,
                            StartTime = item.CustomerCare.StartTime,
                            EndTime = item.CustomerCare.EndTime,
                            IsSuccess = item.IsSuccess,
                            Note = item.Note,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return CustomerCareResult;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public List<CustomerCareResultItem> GetByGroupIdAndCareId(int page, int pageSize, out int totalCount, int groupID, int careId)
        {
            var dbo = CustomerCareResultDA.Repository;
            var CustomerCareResult = dbo.CustomerCategoryCustomers.Where(i => i.CustomerCategoryID == groupID && i.Customer.IsOfficial == true 
                        && i.Customer.IsDelete == false)
                        .Select(item => item.Customer)
                        .Select(item => new CustomerCareResultItem()
                        {
                            //ID = item.CustomerCareR.FirstOrDefault(i => i.CareID == careId).ID,
                            CustomerID = item.ID,
                            // Thông tin của khách hàng
                            //Image = item.Image,
                            Name = item.Name,
                            Phone = item.Phone,
                            Email = item.Email,
                            Address = item.Address,
                            //Type = item.Type,
                           // CareID = item.CustomerCareResult.FirstOrDefault(i => i.CareID == careId).CareID,
                            IsCare = dbo.CustomerCareResults.FirstOrDefault(i =>i.CustomerID==item.ID&& i.CustomerCareID == careId) != null,
                            Note = item.Note,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return CustomerCareResult;
        }
        /// <summary>
        /// Lấy kết quả chăm sóc theo ID
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CustomerCareResultItem GetById(int Id)
        {
            var dbo = CustomerCareResultDA.Repository;
            var CustomerCareResult = dbo.CustomerCareResults.Where(i => i.ID == Id)
                        .Select(item => new CustomerCareResultItem()
                        {
                            ID = item.ID,
                            CustomerID = item.CustomerID,
                            Method = item.Method,
                           // GroupCustomerID = item.CustomerCare.CustomerCategoryCustomers.FirstOrDefault().CustomerCategory.ID,
                            CareID = item.CustomerCareID,
                            CareName = item.CustomerCare.Name,
                            IsSuccess = item.IsSuccess,
                            Note = item.Note,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            CreateByName = dbo.HumanUsers.FirstOrDefault(i => i.ID == item.CreateBy).HumanEmployee.Name,
                            UpdateByName = dbo.HumanUsers.FirstOrDefault(i => i.ID == item.UpdateBy).HumanEmployee.Name,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .First();
            return CustomerCareResult;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CustomerCareResultItem GetByCareIdAndCustomerId(int CareID, int CustomerID)
        {
            var dbo = CustomerCareResultDA.Repository;
            var CustomerCareResult = dbo.CustomerCareResults
                                        .Where(i => i.CustomerCareID == CareID && i.CustomerID == CustomerID)
                        .Select(item => new CustomerCareResultItem()
                        {
                            ID = item.ID,
                            CustomerID = item.CustomerID,
                            Method = item.Method,
                            Name = item.CustomerCare.Name,
                            CareID = item.CustomerCareID,
                            IsSuccess = item.IsSuccess,
                            Note = item.Note,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            CreateByName = dbo.HumanUsers.FirstOrDefault(i => i.ID == item.CreateBy).HumanEmployee.Name,
                            UpdateByName = dbo.HumanUsers.FirstOrDefault(i => i.ID == item.UpdateBy).HumanEmployee.Name,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .FirstOrDefault();
            return CustomerCareResult;
        }
        /// <summary>
        /// Cập nhật kết quả chăm sóc
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(CustomerCareResultItem item, int userID)
        {
            var CustomerCareResult = CustomerCareResultDA.GetById(item.ID);
            CustomerCareResult.CustomerID = item.CustomerID;
            CustomerCareResult.Method = item.Method;
            CustomerCareResult.CustomerCareID = (int)item.CareID;
            CustomerCareResult.IsSuccess = item.IsSuccess;
            CustomerCareResult.Note = item.Note;
            CustomerCareResult.UpdateAt = DateTime.Now;
            CustomerCareResult.UpdateBy = userID;
            CustomerCareResultDA.Save();
        }
        /// <summary>
        /// Thêm mới kết quả chăm sóc
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(CustomerCareResultItem item, int userID)
        {
            var CustomerCareResult = new CustomerCareResult()
            {
                CustomerID = item.CustomerID,
                CustomerCareID = (int)item.CareID,
                Method = item.Method,
                IsSuccess = item.IsSuccess,
                Note = item.Note,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            CustomerCareResultDA.Insert(CustomerCareResult);
            CustomerCareResultDA.Save();
        }
        /// <summary>
        /// Xóa kết quả chăm sóc
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            CustomerCareResultDA.Delete(id);
            CustomerCareResultDA.Save();
        }
    }
}
