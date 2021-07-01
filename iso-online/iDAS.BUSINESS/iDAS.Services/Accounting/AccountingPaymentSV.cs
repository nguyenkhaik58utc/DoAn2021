using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Services;
using iDAS.Base;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class AccountingPaymentSV
    {
        AccountingPaymentDA accountingPaymentDA = new AccountingPaymentDA();
        public List<AccountingPaymentItem> GetAll(int page, int pageSize, out int total)
        {
            var accountingPayment = accountingPaymentDA.GetQuery()
                           .Select(item => new AccountingPaymentItem
                           {
                               ID = item.ID,
                               CustomerContractID = item.CustomerContractID,
                               Content = item.Content,
                               Note = item.Note,
                               Rate = item.Rate,
                               Value = item.Value,
                               IsCustomer = item.IsCustomer,
                               IsDelete = item.IsDelete,
                               CreateAt = item.CreateAt
                           }
                           )
                          .OrderByDescending(item => item.CreateAt)
                          .Page(page, pageSize, out total)
                          .ToList();
            return accountingPayment;
        }

        public List<AccountingPaymentItem> GetByContract(int page, int pageSize, out int totalCount, int contractId)
        {
            var AccountingPayment = accountingPaymentDA.GetQuery(i => i.CustomerContractID == contractId)
                    .Select(item => new AccountingPaymentItem()
                    {
                        ID = item.ID,
                        CustomerContractID = item.CustomerContractID,
                        Content = item.Content,
                        Rate = item.Rate,
                        Value = item.Value,
                        Note = item.Note,
                        IsCustomer = item.IsCustomer,
                        CreateAt = item.CreateAt,
                    })
                    .OrderByDescending(item => item.CreateAt)
                    .Page(page, pageSize, out totalCount)
                    .ToList();
            return AccountingPayment;
        }

        public List<AccountingPaymentItem> GetByContractForAccounting(int page, int pageSize, out int totalCount, int contractId)
        {
            var AccountingPayment = accountingPaymentDA.GetQuery(i => i.CustomerContractID == contractId
                                                                    && i.IsCustomer == false)
                    .Select(item => new AccountingPaymentItem()
                    {
                        ID = item.ID,
                        CustomerContractID = item.CustomerContractID,
                        Content = item.Content,
                        Rate = item.Rate,
                        Value = item.Value,
                        Note = item.Note,
                        IsCustomer = item.IsCustomer,
                        CreateAt = item.CreateAt,
                    })
                    .OrderByDescending(item => item.CreateAt)
                    .Page(page, pageSize, out totalCount)
                    .ToList();
            return AccountingPayment;
        }
        public List<AccountingPaymentItem> GetByContractForCustomer(int page, int pageSize, out int totalCount, int contractId)
        {
            var AccountingPayment = accountingPaymentDA.GetQuery(i => i.CustomerContractID == contractId
                                                                    && i.IsCustomer)
                    .Select(item => new AccountingPaymentItem()
                    {
                        ID = item.ID,
                        CustomerContractID = item.CustomerContractID,
                        Content = item.Content,
                        Rate = item.Rate,
                        Value = item.Value,
                        Note = item.Note,
                        IsCustomer = item.IsCustomer,
                        CreateAt = item.CreateAt,
                    })
                    .OrderByDescending(item => item.CreateAt)
                    .Page(page, pageSize, out totalCount)
                    .ToList();
            return AccountingPayment;
        }
        public AccountingPaymentItem GetByID(int id)
        {
            return accountingPaymentDA.GetQuery(i => i.ID == id)
            .Select(item => new AccountingPaymentItem
            {
                ID = item.ID,
                CustomerContractID = item.CustomerContractID,
                Content = item.Content,
                Note = item.Note,
                Rate = item.Rate,
                Value = item.Value,
                TotalContract = item.CustomerContract.Total,
                IsCustomer = item.IsCustomer,
                IsDelete = item.IsDelete,
                CreateAt = item.CreateAt
            }).First();
        }

        public AccountingPaymentItem GetByContract(int contractId)
        {
            return accountingPaymentDA.GetQuery(i => i.ID == contractId)
            .Select(item => new AccountingPaymentItem
            {
                ID = item.ID,
                CustomerContractID = item.CustomerContractID,
                Content = item.Content,
                Note = item.Note,
                Rate = item.Rate,
                Value = item.Value,
                IsCustomer = item.IsCustomer,
                IsDelete = item.IsDelete,
                CreateAt = item.CreateAt
            }).First();
        }

        public void Insert(AccountingPaymentItem item, int userId)
        {
            AccountingPayment obj = new AccountingPayment();
            obj.CustomerContractID = item.CustomerContractID;
            obj.Content = item.Content;
            obj.Note = item.Note;
            obj.Rate = item.Rate;
            obj.Value = item.Value;
            obj.IsCustomer = item.IsCustomer;
            obj.IsDelete = item.IsDelete;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            accountingPaymentDA.Insert(obj);
            accountingPaymentDA.Save();
        }

        public void Update(AccountingPaymentItem item, int userId)
        {
            AccountingPayment obj = accountingPaymentDA.GetById(item.ID);
            obj.Content = item.Content;
            obj.Note = item.Note;
            obj.Rate = item.Rate;
            obj.Value = item.Value;
            obj.IsCustomer = item.IsCustomer;
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            accountingPaymentDA.Update(obj);
            accountingPaymentDA.Save();
        }

        public void Delete(int id)
        {
            var audits = accountingPaymentDA.GetById(id);
            accountingPaymentDA.Delete(audits);
            accountingPaymentDA.Save();
        }
    }
}
