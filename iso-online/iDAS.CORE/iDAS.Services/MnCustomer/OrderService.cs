using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.DAL.MnCustomer;
using iDAS.Items;
using iDAS.Items.MnCustomer;

namespace iDAS.Services.MnCustomer
{
    public class OrderService
    {
        OrderDA orderDA = new OrderDA();

        public List<OrderItem> GetAll(int campaignId, int page, int pageSize, out int totalCount)
        {
            var orders = orderDA.GetQuery()
                        .Where(item => item.MnCustomerContact.MnCustomerCampaign.ID == campaignId)
                        .Select(item => new OrderItem()
                        {
                            ID = item.ID,
                            ContactID = item.ContactID,
                            CustomerID = item.MnCustomerContact.CustomerID,
                            CustomerName = item.MnCustomerContact.MnCustomerCustomer.Name,
                            ProductID = item.ProductID,
                            ProductName = item.MnCustomerProduct.Name,
                            EmployeeID = item.EmployeeID,
                            EmployeeName = item.SystemUser.FullName,
                            Time = item.Time,
                            Quantity = item.Quantity,
                            Total = item.Total,
                            Status = item.Status,
                            IsContract = item.IsContract,
                            IsDisabled = item.IsDisabled,
                            IsContactBack = item.IsContactBack,
                            IsDisabledContract = item.MnCustomerContracts.FirstOrDefault(i=>i.OrderID == item.ID).IsDisabled,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return orders;
        }

        public OrderItem GetByID(int id)
        {
            var order = orderDA.GetQuery()
                        .Where(item=>item.ID == id)
                        .Select(item => new OrderItem()
                        {
                            ID = item.ID,
                            ContactID = item.ContactID,
                            CustomerID = item.MnCustomerContact.MnCustomerCustomer.ID,
                            CustomerName = item.MnCustomerContact.MnCustomerCustomer.Name,
                            ProductID = item.ProductID,
                            ProductName = item.MnCustomerProduct.Name,
                            Price = item.MnCustomerProduct.Price,
                            EmployeeID = item.EmployeeID,
                            EmployeeName = item.SystemUser.FullName,
                            Quantity = item.Quantity,
                            Time = item.Time,
                            Total = item.Total,
                            Status = item.Status,
                            Note = item.Note,
                            IsContactBack = item.IsContactBack,
                            IsContract = item.IsContract,
                            IsDisabled = item.IsDisabled,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .FirstOrDefault();
            return order;
        }

        public void Insert(OrderItem item)
        {
            var contact = new MnCustomerOrder()
            {
                ContactID = item.ContactID,
                Time = item.Time,
                EmployeeID = item.EmployeeID,
                Status = item.Status,
                CreateAt = DateTime.Now,
                CreateBy = item.CreateBy,
            };
            orderDA.Insert(contact);
            orderDA.Save();
        }

        public void Update(OrderItem item)
        {
            var order = orderDA.GetById(item.ID);
            order.EmployeeID = item.EmployeeID;
            order.Time = item.Time;
            order.Quantity = item.Quantity;
            order.Total = item.Total;
            order.IsContactBack = item.IsContactBack;
            order.IsDisabled = item.IsDisabled;
            order.IsContract = item.IsContract;
            order.Result = item.Result;
            order.Status = (byte)OrderItem.OrderStatus.Order;
            order.Note = item.Note;
            order.UpdateAt = DateTime.Now;
            order.UpdateBy = item.UpdateBy;

            if (item.IsContactBack) {
                order.Status = (byte)OrderItem.OrderStatus.ContactBack;
                order.IsDisabled = true;
                updateContact(item);
                updateDemand(item);
            }

            if (item.IsContract) {
                order.Status = (byte)OrderItem.OrderStatus.Contract;
                order.IsDisabled = true;
                insertContract(item);
            }

            if (item.IsDisabled){
                order.Status = item.Result ? (byte)OrderItem.OrderStatus.Success : (byte)OrderItem.OrderStatus.Fail;
            }

            orderDA.Save();
        }

        private void updateContact(OrderItem item) {
            var dbo = orderDA.SystemContext;
            var contact = dbo.MnCustomerContacts.FirstOrDefault(i => i.ID == item.ContactID);
            contact.Status = (byte)ContactItem.ContactStatus.ContactBack;
            contact.IsDisabled = false;
            contact.UpdateAt = DateTime.Now;
            contact.UpdateBy = item.UpdateBy;
        }

        private void updateDemand(OrderItem item) {
            var dbo = orderDA.SystemContext;
            var contact = dbo.MnCustomerContacts.FirstOrDefault(i => i.ID == item.ContactID);
            var demand = dbo.MnCustomerDemands.FirstOrDefault(i => i.CustomerID == contact.CustomerID && i.ProductID == item.ProductID);
            demand.Time = item.Time;
            demand.Quantity = item.Quantity;
            demand.UpdateAt = DateTime.Now;
            demand.UpdateBy = item.UpdateBy;
        }

        private void insertContract(OrderItem item) {
            var dbo = orderDA.SystemContext;
            var contract = new MnCustomerContract();
            contract.OrderID = item.ID;
            contract.IsDisabled = false;
            contract.Result = false;
            contract.CreateAt = DateTime.Now;
            contract.CreateBy = item.UpdateBy;
            dbo.MnCustomerContracts.Add(contract);
        }

    }
}
