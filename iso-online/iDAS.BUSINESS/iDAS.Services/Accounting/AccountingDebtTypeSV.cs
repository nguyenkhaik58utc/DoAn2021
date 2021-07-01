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
    public class AccountingDebtTypeSV
    {
        private AccountingDebtTypeDA AccountingDebtTypeDA = new AccountingDebtTypeDA();
        /// <summary>
        /// Lấy danh sách 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<AccountingDebtTypeItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var AccountingDebtType = AccountingDebtTypeDA.GetQuery()
                        .Select(item => new AccountingDebtTypeItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Value = item.Value,
                            Day = item.Day,
                            IsActive = item.IsActive,
                            IsDelete = item.IsDelete,
                            Note = item.Note,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return AccountingDebtType;
        }
        public List<AccountingDebtTypeItem> GetAll()
        {
            var AccountingDebtType = AccountingDebtTypeDA.GetQuery()
                        .Select(item => new AccountingDebtTypeItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Value = item.Value,
                            Day = item.Day,
                            IsActive = item.IsActive,
                            IsDelete = item.IsDelete,
                            Note = item.Note,
                            CreateAt = item.CreateAt,
                        })
                        .ToList();
            return AccountingDebtType;
        }
        public List<AccountingDebtTypeItem> GetActive(int page, int pageSize, out int totalCount)
        {
            var AccountingDebtType = AccountingDebtTypeDA.GetQuery(i=>i.IsActive==true&&i.IsDelete==false)
                        .Select(item => new AccountingDebtTypeItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Day =item.Day,
                            Value = item.Value,
                            IsActive = item.IsActive,
                            IsDelete = item.IsDelete,
                            Note = item.Note,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return AccountingDebtType;
        }
        /// <summary>
        /// Lấy  theo ID
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public AccountingDebtTypeItem GetById(int Id)
        {
            var AccountingDebtType = AccountingDebtTypeDA.GetQuery(i => i.ID == Id)
                        .Select(item => new AccountingDebtTypeItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Day = item.Day,
                            Value = item.Value,
                            IsActive = item.IsActive,
                            IsDelete = item.IsDelete,
                            Note = item.Note,
                        })
                        .First();
            return AccountingDebtType;
        }

        /// <summary>
        /// Cập nhật 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(AccountingDebtTypeItem item, int userID)
        {
            var AccountingDebtType = AccountingDebtTypeDA.GetById(item.ID);
            AccountingDebtType.ID = item.ID;
            AccountingDebtType.Name = item.Name;
            AccountingDebtType.Day = item.Day;
            AccountingDebtType.Value = item.Value;
            AccountingDebtType.IsActive = item.IsActive;
            AccountingDebtType.IsDelete = item.IsDelete;
            AccountingDebtType.Note = item.Note;
            AccountingDebtType.UpdateAt = DateTime.Now;
            AccountingDebtType.UpdateBy = userID;
            AccountingDebtTypeDA.Save();
        }
        /// <summary>
        /// Thêm mới 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(AccountingDebtTypeItem item, int userID)
        {
            var AccountingDebtType = new AccountingDebtType()
            {
                Name = item.Name,
                Day = item.Day,
                Value = item.Value,
                IsActive = item.IsActive,
                IsDelete = item.IsDelete,
                Note = item.Note,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            AccountingDebtTypeDA.Insert(AccountingDebtType);
            AccountingDebtTypeDA.Save();
        }
        /// <summary>
        /// Xóa 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="id"></param>
        public bool Delete(int id)
        {
            var dbo = AccountingDebtTypeDA.Repository;
            var AccountingDebtType = dbo.AccountingDebtTypes.FirstOrDefault(i => i.ID == id);
            dbo.AccountingDebtTypes.Remove(AccountingDebtType);
            dbo.SaveChanges();
            return true;
        }
    }
}
