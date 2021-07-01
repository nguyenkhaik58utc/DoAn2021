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
    public class CustomerOrderSV
    {
        private CustomerOrderDA CustomerOrderDA = new CustomerOrderDA();
        /// <summary>
        /// Lấy danh sách đơn hàng
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CustomerOrderItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var dbo = CustomerOrderDA.Repository;
            var CustomerOrder = CustomerOrderDA.GetQuery()
                        .Select(item => new CustomerOrderItem()
                        {
                            ID = item.ID,
                            CustomerID = item.CustomerID,
                            ServiceID = item.ServiceID,
                            ProductName = item.Service.Name,
                            Quantity = item.Quantity,
                            Time = item.Time,
                            IsFinish = item.IsFinish,
                            IsPause = item.IsPause,
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
            return CustomerOrder;
        }
        public List<CustomerOrderItem> GetByCustomer(int page, int pageSize, out int totalCount, int CustomerID)
        {
            var dbo = CustomerOrderDA.Repository;
            var CustomerOrder = CustomerOrderDA.GetQuery(i => i.CustomerID == CustomerID)
                        .Select(item => new CustomerOrderItem()
                        {
                            ID = item.ID,
                            CustomerID = item.CustomerID,
                            ServiceID = item.ServiceID,
                            ProductName = item.Service.Name,
                            Quantity = item.Quantity,
                            Time = item.Time,
                            IsFinish = item.IsFinish,
                            IsPause = item.IsPause,
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
            return CustomerOrder;
        }

        public List<CustomerOrderSelected> GetByContract(int page, int pageSize, out int totalCount, int contractId)
        {
            var dbo = CustomerOrderDA.Repository;
            var CustomerOrder = CustomerOrderDA.GetQuery(i => i.CustomerContractID == contractId)
                        .Select(item => new CustomerOrderSelected()
                        {
                            ID = item.ID,
                            CustomerID = item.CustomerID,
                            ServiceID = item.ServiceID,
                            ContractID = item.CustomerContractID,
                            ProductName = item.Service.Name,
                            Quantity = item.Quantity,
                            Time = item.Time,
                            IsFinish = item.IsFinish,
                            IsPause = item.IsPause,
                            IsSuccess = item.IsSuccess,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return CustomerOrder;
        }
        public List<CustomerOrderSelected> GetContractSelect(int page, int pageSize, out int totalCount, int contractId, int customerId)
        {
            var dbo = CustomerOrderDA.Repository;
            var CustomerOrder = CustomerOrderDA.GetQuery(i => i.CustomerID == customerId && (i.CustomerContractID == contractId || i.CustomerContractID == null))
                        .Select(item => new CustomerOrderSelected()
                        {
                            ID = item.ID,
                            CustomerID = item.CustomerID,
                            ServiceID = item.ServiceID,
                            ContractID = item.CustomerContractID,
                            ProductName = item.Service.Name,
                            Quantity = item.Quantity,
                            Time = item.Time,
                            IsFinish = item.IsFinish,
                            IsPause = item.IsPause,
                            IsSuccess = item.IsSuccess,
                            Note = item.Note,
                            IsSelect = dbo.CustomerOrders.FirstOrDefault(i => i.ID == item.ID && i.CustomerContractID == contractId) != null ? true : false,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return CustomerOrder;
        }
        /// <summary>
        /// Lấy đơn hàng theo ID
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CustomerOrderItem GetById(int Id)
        {
            var dbo = CustomerOrderDA.Repository;
            var result = CustomerOrderDA.GetQuery(i => i.ID == Id)
                        .Select(item => new CustomerOrderItem()
                        {
                            ID = item.ID,
                            CustomerID = item.CustomerID,
                            ServiceID = item.ServiceID,
                            ProductName = item.Service.Name,
                            Quantity = item.Quantity,
                            Time = item.Time,
                            IsFinish = item.IsFinish,
                            IsPause = item.IsPause,
                            IsSuccess = item.IsSuccess,
                            IsPrice = item.IsPrice,
                            StatusValue = (item.IsPause == true) ? "IsPause" : (item.IsFinish == true ? "IsFinish" : "IsNew"),
                            Note = item.Note
                        })
                        .First();
            if (result != null)
            {
                result.AttachmentFile = new AttachmentFileItem()
                                    {
                                        Files = dbo.CustomerOrderAttachmentFiles.Where(i => i.CustomerOrderID == Id)
                                            .Select(i => i.AttachmentFileID).ToList()
                                    };
            }
            return result;
        }

        /// <summary>
        /// Cập nhật đơn hàng
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(CustomerOrderItem item, int userID)
        {
            var order = CustomerOrderDA.GetById(item.ID);
            order.CustomerID = item.CustomerID;
            order.ServiceID = item.ServiceID;
            order.Quantity = item.Quantity;
            order.Time = item.Time;
            order.IsFinish = item.IsFinish;
            order.IsPause = item.IsPause;
            order.IsSuccess = item.IsSuccess;
            order.Note = item.Note;
            order.IsPrice = item.IsPrice;
            order.CreateAt = item.CreateAt;
            order.CreateBy = item.CreateBy;
            order.UpdateAt = item.UpdateAt;
            order.UpdateBy = item.UpdateBy;
            CustomerOrderDA.Save();
            new FileSV().Upload<CustomerOrderAttachmentFile>(item.AttachmentFile, item.ID);
        }
        public void UpdateContract(CustomerOrderSelected item)
        {
            var order = CustomerOrderDA.GetById(item.ID);
            order.CustomerContractID = item.ContractID;
            CustomerOrderDA.Save();
        }
        /// <summary>
        /// Thêm mới đơn hàng
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(CustomerOrderItem item, int userID)
        {
            var CustomerOrder = new CustomerOrder()
            {
                CustomerID = item.CustomerID,
                ServiceID = item.ServiceID,
                Quantity = item.Quantity,
                Time = item.Time,
                IsFinish = item.IsFinish,
                IsPause = item.IsPause,
                IsSuccess = item.IsSuccess,
                IsPrice = item.IsPrice,
                Note = item.Note,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            CustomerOrderDA.Insert(CustomerOrder);
            CustomerOrderDA.Save();
            new FileSV().Upload<CustomerOrderAttachmentFile>(item.AttachmentFile, CustomerOrder.ID);
        }
        /// <summary>
        /// Xóa đơn hàng
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="id"></param>
        public bool Delete(int id)
        {
            var dbo = CustomerOrderDA.Repository;
            var orderItem = dbo.CustomerOrders.Where(i => i.ID == id).FirstOrDefault();
            if (orderItem != null)
            {
                if (orderItem.CustomerContractID == null || dbo.CustomerContracts.Where(i => i.ID == orderItem.CustomerContractID).FirstOrDefault() == null)
                {
                    CustomerOrderDA.Delete(id);
                    CustomerOrderDA.Save();
                    return true;
                }
            }
            return false;
        }
    }
}
