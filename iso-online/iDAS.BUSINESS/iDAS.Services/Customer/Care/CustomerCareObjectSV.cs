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
    public class CustomerCareObjectSV
    {
        private CustomerCareObjectDA CustomerCareObjectDA = new CustomerCareObjectDA();
        /// <summary>
        /// Lấy danh sách đơn hàng
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CustomerCareObjectItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var dbo = CustomerCareObjectDA.Repository;
            var CustomerCareObject = CustomerCareObjectDA.GetQuery()
                        .Select(item => new CustomerCareObjectItem()
                        {
                            ID = item.ID,
                            CareID = item.CustomerCareID,
                            CategoryID = item.CustomerCategoryID,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return CustomerCareObject;
        }
        /// <summary>
        /// Lấy danh sách các nhóm đối tượng khách hàng lựa chọn để chăm sóc
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="CareID"></param>
        /// <returns></returns>
        public List<CustomerCareObjectItem> GetAllCustomerCareObject(int page, int pageSize, out int totalCount, int CareID)
        {
            var dbo = CustomerCareObjectDA.Repository;
            var groupCriteria = dbo.CustomerCategories.Where(i => i.IsDelete == false)
                        .Select(item => new CustomerCareObjectItem()
                        {
                            ID = dbo.CustomerCareObjects.FirstOrDefault(i => i.CustomerCategoryID == item.ID 
                                && i.CustomerCareID == CareID
                                ).ID,
                            CareID = CareID,
                            CategoryID = item.ID,
                            Name = item.Name,
                            Note = item.Note,
                            IsSelect = dbo.CustomerCareObjects.FirstOrDefault(i => i.CustomerCategoryID == item.ID
                                && i.CustomerCareID == CareID
                                ) != null
                        })
                        .OrderByDescending(item => item.Name)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return groupCriteria;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="CareID"></param>
        /// <returns></returns>
        public List<CustomerCareObjectItem> GetCustomerCareObject(int page, int pageSize, out int totalCount, int CareID)
        {
            var dbo = CustomerCareObjectDA.Repository;
            var groupCriteria = dbo.CustomerCareObjects.Where(i =>i.CustomerCareID == CareID)
                        .Select(item => new CustomerCareObjectItem()
                        {
                            ID = item.ID,
                            CareID = item.CustomerCareID,
                            CategoryID = item.CustomerCategory.ID,
                            Name = item.CustomerCategory.Name,
                            Note = item.CustomerCategory.Note,
                            IsSelect = true
                        })
                        .OrderByDescending(item => item.Name)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return groupCriteria;
        }
        /// <summary>
        /// Lấy đơn hàng theo ID
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CustomerCareObjectItem GetById(int Id)
        {
            var CustomerCareObject = CustomerCareObjectDA.GetQuery(i => i.ID == Id)
                        .Select(item => new CustomerCareObjectItem()
                        {
                            ID = item.ID,
                            CareID = item.CustomerCareID,
                            CategoryID = item.CustomerCategoryID,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .First();
            return CustomerCareObject;
        }

        /// <summary>
        /// Cập nhật đơn hàng
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(CustomerCareObjectItem item, int userID)
        {
            var CustomerCareObject = CustomerCareObjectDA.GetById(item.ID);
            CustomerCareObject.CustomerCategoryID = item.CategoryID;
            CustomerCareObject.CreateAt = item.CreateAt;
            CustomerCareObject.CreateBy = item.CreateBy;
            CustomerCareObject.UpdateAt = item.UpdateAt;
            CustomerCareObject.UpdateBy = item.UpdateBy;
            CustomerCareObjectDA.Save();
        }
        /// <summary>
        /// Thêm mới đơn hàng
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(CustomerCareObjectItem item, int userID)
        {
            var CustomerCareObject = new CustomerCareObject()
            {
                CustomerCareID = item.CareID,
                CustomerCategoryID = item.CategoryID,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            CustomerCareObjectDA.Insert(CustomerCareObject);
            CustomerCareObjectDA.Save();
        }
        /// <summary>
        /// Xóa đơn hàng
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            CustomerCareObjectDA.Delete(id);
            CustomerCareObjectDA.Save();
        }
    }
}
