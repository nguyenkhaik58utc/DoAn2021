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
    public class CustomerTypeSV
    {
        private CustomerTypeDA CustomerTypeDA = new CustomerTypeDA();
        /// <summary>
        /// Lấy danh sách 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CustomerTypeItem> GetAll(ModelFilter filter)
        {
            var CustomerType = CustomerTypeDA.GetQuery()
                        .Select(item => new CustomerTypeItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            IsActive = item.IsActive,
                            IsDelete = item.IsDelete,
                            Note = item.Note,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            return CustomerType;
        }
        public List<CustomerTypeItem> GetActived()
        {
            var CustomerTypes = CustomerTypeDA.GetQuery(i=>i.IsActive&&!i.IsDelete)
                        .Select(item => new CustomerTypeItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                        })
                        .OrderByDescending(item => item.Name)
                        .ToList();
            return CustomerTypes;
        }
        /// <summary>
        /// Lấy  theo ID
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CustomerTypeItem GetById(int Id)
        {
            var CustomerType = CustomerTypeDA.GetQuery(i => i.ID == Id)
                        .Select(item => new CustomerTypeItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            IsActive = item.IsActive,
                            IsDelete = item.IsDelete,
                            Note = item.Note,
                        })
                        .First();
            return CustomerType;
        }

        /// <summary>
        /// Cập nhật 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(CustomerTypeItem item, int userID)
        {
            var CustomerType = CustomerTypeDA.GetById(item.ID);
            CustomerType.ID = item.ID;
            CustomerType.ID = item.ID;
            CustomerType.Name = item.Name;
            CustomerType.IsActive = item.IsActive;
            CustomerType.IsDelete = item.IsDelete;
            CustomerType.Note = item.Note;
            CustomerType.UpdateAt = DateTime.Now;
            CustomerType.UpdateBy = userID;
            CustomerTypeDA.Save();
        }
        /// <summary>
        /// Thêm mới 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(CustomerTypeItem item, int userID)
        {
            var CustomerType = new CustomerType()
            {
                Name = item.Name,
                IsActive = item.IsActive,
                IsDelete = item.IsDelete,
                Note = item.Note,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            CustomerTypeDA.Insert(CustomerType);
            CustomerTypeDA.Save();
        }
        /// <summary>
        /// Xóa 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="id"></param>
        public bool Delete(int id)
        {
            var dbo = CustomerTypeDA.Repository;
            var customerType = dbo.CustomerTypes.FirstOrDefault(i => i.ID == id);
            if (customerType != null && dbo.Customers.FirstOrDefault(i => i.CustomerTypeID == customerType.ID) == null)
            {
                dbo.CustomerTypes.Remove(customerType);
                dbo.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
