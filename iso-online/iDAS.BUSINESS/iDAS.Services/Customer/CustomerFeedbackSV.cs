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
    public class CustomerFeedbackSV
    {
        private CustomerFeedbackDA CustomerFeedbackDA = new CustomerFeedbackDA();
        /// <summary>
        /// Lấy danh sách phản hồi
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CustomerFeedbackItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var dbo = CustomerFeedbackDA.Repository;
            var CustomerFeedback = CustomerFeedbackDA.GetQuery()
                        .Select(item => new CustomerFeedbackItem()
                        {
                            ID = item.ID,
                            CustomerID = item.CustomerID,
                            Content = item.Content,
                            Time = item.Time,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return CustomerFeedback;
        }
        public List<CustomerFeedbackItem> GetByCustomer(int page, int pageSize, out int totalCount,int CustomerID)
        {
            var dbo = CustomerFeedbackDA.Repository;
            var CustomerFeedback = CustomerFeedbackDA.GetQuery(i=>i.CustomerID == CustomerID)
                        .Select(item => new CustomerFeedbackItem()
                        {
                            ID = item.ID,
                            CustomerID = item.CustomerID,
                            Content = item.Content,
                            Time = item.Time,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return CustomerFeedback;
        }
        /// <summary>
        /// Lấy phản hồi theo ID
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CustomerFeedbackItem GetById(int Id)
        {
            var CustomerFeedback = CustomerFeedbackDA.GetQuery(i => i.ID == Id)
                        .Select(item => new CustomerFeedbackItem()
                        {
                            ID = item.ID,
                            CustomerID = item.CustomerID,
                            Content = item.Content,
                            Time = item.Time,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .First();
            return CustomerFeedback;
        }

        /// <summary>
        /// Cập nhật phản hồi
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(CustomerFeedbackItem item, int userID)
        {
            var CustomerFeedback = CustomerFeedbackDA.GetById(item.ID);
            CustomerFeedback.CustomerID = item.CustomerID;
            CustomerFeedback.Content = item.Content;
            CustomerFeedback.Time = item.Time;
            CustomerFeedback.UpdateAt = DateTime.Now;
            CustomerFeedback.UpdateBy = userID;
            CustomerFeedbackDA.Save();
        }
        /// <summary>
        /// Thêm mới phản hồi
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(CustomerFeedbackItem item, int userID)
        {
            var CustomerFeedback = new CustomerFeedback()
            {
                CustomerID = item.CustomerID,
                Content = item.Content,
                Time =item.Time,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            CustomerFeedbackDA.Insert(CustomerFeedback);
            CustomerFeedbackDA.Save();
        }
        /// <summary>
        /// Xóa phản hồi
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            CustomerFeedbackDA.Delete(id);
            CustomerFeedbackDA.Save();
        }
    }
}
