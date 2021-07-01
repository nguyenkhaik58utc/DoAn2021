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
    public class CustomerAssessObjectSV
    {
        private CustomerAssessObjectDA CustomerAssessObjectDA = new CustomerAssessObjectDA();
        /// <summary>
        /// Lấy danh sách đơn hàng
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CustomerAssessObjectItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var dbo = CustomerAssessObjectDA.Repository;
            var CustomerAssessObjects = CustomerAssessObjectDA.GetQuery()
                        .Select(item => new CustomerAssessObjectItem()
                        {
                            ID = item.ID,
                            AuditID = item.CustomerAssessID,
                            CustomerCategoryID = item.CustomerCategoryID,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return CustomerAssessObjects;
        }
        /// <summary>
        /// Lấy danh sách các nhóm đối tượng khách hàng lựa chọn để chăm sóc
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="auditID"></param>
        /// <returns></returns>
        public List<CustomerAssessObjectItem> GetAllCustomerAssessObject(int page, int pageSize, out int totalCount, int auditID)
        {
            var dbo = CustomerAssessObjectDA.Repository;
            var CustomerAssessObjects = dbo.CustomerCategories.Where(i => i.IsDelete == false)
                        .Select(item => new CustomerAssessObjectItem()
                        {
                            ID = dbo.CustomerAssessObjects.FirstOrDefault(i => i.CustomerCategoryID == item.ID && i.CustomerAssessID == auditID).ID,
                            AuditID = auditID,
                            CustomerCategoryID = item.ID,
                            Name = item.Name,
                            IsSelect = dbo.CustomerAssessObjects.FirstOrDefault(i => i.CustomerCategoryID == item.ID && i.CustomerAssessID == auditID) != null
                        })
                        .OrderByDescending(item => item.Name)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return CustomerAssessObjects;
        }
        /// <summary>
        /// Lấy đơn hàng theo ID
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CustomerAssessObjectItem GetById(int Id)
        {
            var CustomerAssessObject = CustomerAssessObjectDA.GetQuery(i => i.ID == Id)
                        .Select(item => new CustomerAssessObjectItem()
                        {
                            ID = item.ID,
                            AuditID = item.CustomerAssessID,
                            CustomerCategoryID = item.CustomerCategoryID,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .First();
            return CustomerAssessObject;
        }

        /// <summary>
        /// Cập nhật đơn hàng
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(CustomerAssessObjectItem item, int userID)
        {
            var CustomerAssessObject = CustomerAssessObjectDA.GetById(item.ID);
            CustomerAssessObject.CustomerCategoryID = item.CustomerCategoryID;
            CustomerAssessObject.CreateAt = item.CreateAt;
            CustomerAssessObject.CreateBy = item.CreateBy;
            CustomerAssessObject.UpdateAt = item.UpdateAt;
            CustomerAssessObject.UpdateBy = item.UpdateBy;
            CustomerAssessObjectDA.Save();
        }
        /// <summary>
        /// Thêm mới đơn hàng
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(CustomerAssessObjectItem item, int userID)
        {
            var CustomerAssessObject = new CustomerAssessObject()
            {
                CustomerAssessID = item.AuditID,
                CustomerCategoryID = item.CustomerCategoryID,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            CustomerAssessObjectDA.Insert(CustomerAssessObject);
            CustomerAssessObjectDA.Save();
        }
        /// <summary>
        /// Xóa đơn hàng
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            CustomerAssessObjectDA.Delete(id);
            CustomerAssessObjectDA.Save();
        }
    }
}
