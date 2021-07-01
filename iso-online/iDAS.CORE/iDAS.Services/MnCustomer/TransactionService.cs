using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.DAL.MnCustomer;
using iDAS.Items.MnCustomer;

namespace iDAS.Services.MnCustomer
{
    public class TransactionService
    {
        TransactionDA transactionDA = new TransactionDA();

        public List<TransactionItem> GetAll(int contactID, int page, int pageSize, out int totalCount)
        {
            var transactions = transactionDA.GetQuery()
                        .Where(item => item.ContactID == contactID)
                        .Select(item => new TransactionItem()
                        {
                            ID = item.ID,
                            Index = item.Index,
                            CustomerName = transactionDA.SystemContext.MnCustomerContacts.FirstOrDefault(i => i.ID == item.ContactID).MnCustomerCustomer.Name,
                            EmployeeName = transactionDA.SystemContext.SystemUsers.FirstOrDefault(u => u.ID == transactionDA.SystemContext.MnCustomerContacts.FirstOrDefault(i => i.ID == item.ContactID).EmployeeID).FullName,
                            Time = item.Time,
                            MethodName = transactionDA.SystemContext.MnCustomerMethods.FirstOrDefault(i => i.ID == item.MethodID).Name,
                            TimeBack = item.TimeBack,
                            MethodBackName = transactionDA.SystemContext.MnCustomerMethods.FirstOrDefault(i => i.ID == item.MethodBackID).Name,
                            IsDisabled = item.IsDisabled,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            CreateByName = transactionDA.SystemContext.SystemUsers.FirstOrDefault(u => u.ID == item.CreateBy).FullName,
                        })
                        .OrderBy(item => item.Index)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return transactions;
        }

        //public List<TransactionItem> GetAll(int contactID)
        //{
        //    var transactions = transactionDA.GetQuery()
        //                .Where(item => item.ContactID == contactID)
        //                .Select(item => new TransactionItem()
        //                {
        //                    ID = item.ID,
        //                    EmployeeID = item.EmployeeID,
        //                    CreateAt = item.CreateAt,
        //                    CreateBy = item.CreateBy,
        //                })
        //                .OrderByDescending(item => item.CreateAt)
        //                .ToList();
        //    return transactions;
        //}

        public void Insert(TransactionItem item)
        {
            var transaction = new MnCustomerTransaction()
            {
                ContactID = item.ContactID,
                Index = item.Index,
                IsDisabled = item.IsDisabled,
                Time = item.Time,
                MethodID = item.MethodID,
                TimeBack = item.TimeBack,
                MethodBackID = item.MethodBackID,
                Content = item.Content,
                CreateAt = DateTime.Now,
                CreateBy = item.CreateBy,
            };

            updateContact(item);
            transactionDA.Insert(transaction);
            transactionDA.Save();
        }

        private void updateContact(TransactionItem item)
        {
            var dbo = transactionDA.SystemContext;
            var contact = dbo.MnCustomerContacts.FirstOrDefault(i => i.ID == item.ContactID);
            if (contact == null) return;
            contact.UpdateBy = item.CreateBy;
            var check = checkDemand(contact);

            contact.Total = item.Index;
            contact.TimeNew = item.Time;
            contact.MethodNewID = item.MethodID;
            contact.TimeBack = item.TimeBack;
            contact.MethodBackID = item.MethodBackID;
            contact.IsDisabled = item.IsDisabled || check;
            contact.Status = contact.IsDisabled ? (byte)ContactItem.ContactStatus.Stop : (byte)ContactItem.ContactStatus.Contact;
            contact.UpdateAt = DateTime.Now;
            
        }

        private bool checkDemand(MnCustomerContact contact){
            var campaign = contact.MnCustomerCampaign;
            var demands = contact.MnCustomerCustomer.MnCustomerDemands.Where(i => !i.IsDisabled);
            var check = true;
            foreach (var demand in demands) {
                if (demand.Time.HasValue) {
                    if (DateTime.Now.AddDays(campaign.OrderMaxTime) >= demand.Time && demand.Time >= DateTime.Now)
                    {
                        insertOrder(contact.ID, demand.ProductID, demand.Time.Value, demand.Quantity.Value, contact.UpdateBy);
                        demand.IsDisabled = true;
                    }
                    else {
                        demand.TimeBack = demand.Time.Value.AddDays(-campaign.OrderContactBackTime);
                        check = false;
                    }
                }
            }
            return check;
        }

        private void insertOrder(int contactId,int productId,DateTime orderTime,int quantity, int? userId)
        {
            MnCustomerOrder order = new MnCustomerOrder()
            {
                ContactID = contactId,
                ProductID = productId,
                Time = orderTime,
                Quantity = quantity,
                CreateBy = userId,
                CreateAt = DateTime.Now,
            };
            transactionDA.SystemContext.MnCustomerOrders.Add(order);
        }
    }
}
