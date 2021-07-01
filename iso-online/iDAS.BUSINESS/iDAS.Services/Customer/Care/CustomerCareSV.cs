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
    public class CustomerCareSV
    {
        private CustomerCareDA CustomerCareDA = new CustomerCareDA();
        /// <summary>
        /// Lấy danh sách chăm sóc
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CustomerCareItem> GetAll(ModelFilter filter)
        {
            var dbo = CustomerCareDA.Repository;
            var CustomerCare = CustomerCareDA.GetQuery()
                        .Select(item => new CustomerCareItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Note = item.Note,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            Method = item.Method,
                            Cost = item.Cost,
                            IsFinish = item.IsFinish,
                            IsPause = item.IsPause,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            ;
            return CustomerCare;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CustomerCareItem> GetByCustomerID(int page, int pageSize, out int totalCount, int CustomerID)
        {
            var dbo = CustomerCareDA.Repository;
            var cartegoryIDs = dbo.CustomerCategoryCustomers.Where(i => i.CustomerID == CustomerID).Select(i => i.CustomerCategoryID);
            var CustomerCare = dbo.CustomerCareObjects.Where(i => cartegoryIDs.Contains(i.CustomerCategory.ID))
                .Select(item => item.CustomerCare).Distinct()
                        .Select(item => new CustomerCareItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                        })
                        .OrderByDescending(item => item.Name)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return CustomerCare;
        }
        /// <summary>
        /// Lấy chăm sóc theo ID
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CustomerCareItem GetById(int Id)
        {
            var CustomerCare = CustomerCareDA.GetQuery(i => i.ID == Id)
                        .Select(item => new CustomerCareItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Note = item.Note,
                            StartTime = item.StartTime,
                            StatusValue = (item.IsPause == true) ? "IsPause" : (item.IsFinish == true ? "IsFinish" : "IsNew"),
                            EndTime = item.EndTime,
                            Method = item.Method,
                            Cost = item.Cost,
                            IsFinish = item.IsFinish,
                            IsPause = item.IsPause,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .First();
            return CustomerCare;
        }

        /// <summary>
        /// Cập nhật chăm sóc
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(CustomerCareItem item, int userID)
        {
            var CustomerCare = CustomerCareDA.GetById(item.ID);
            CustomerCare.Name = item.Name;
            CustomerCare.Method = item.Method;
            CustomerCare.Cost = item.Cost;
            CustomerCare.StartTime = item.StartTime;
            CustomerCare.EndTime = item.EndTime;
            CustomerCare.IsFinish = item.IsFinish;
            CustomerCare.IsPause = item.IsPause;
            CustomerCare.Note = item.Note;
            CustomerCare.UpdateAt = DateTime.Now;
            CustomerCare.UpdateBy = userID;
            CustomerCareDA.Save();
        }
        /// <summary>
        /// Thêm mới chăm sóc
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(CustomerCareItem item, int userID)
        {
            var CustomerCare = new CustomerCare()
            {
                Name = item.Name,
                Note = item.Note,
                StartTime = item.StartTime,
                EndTime = item.EndTime,
                Method = item.Method,
                Cost = item.Cost,
                IsFinish = item.IsFinish,
                IsPause = item.IsPause,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            CustomerCareDA.Insert(CustomerCare);
            CustomerCareDA.Save();
        }
        /// <summary>
        /// Xóa chăm sóc
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            CustomerCareDA.Delete(id);
            CustomerCareDA.Save();
        }
    }
}
