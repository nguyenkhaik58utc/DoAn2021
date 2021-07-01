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
    public class CustomerFormSV
    {
        private CustomerContactFormDA CustomerFormDA = new CustomerContactFormDA();
        /// <summary>
        /// Lấy danh sách 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CustomerFormItem> GetAll(ModelFilter filter)
        {
            var dbo = CustomerFormDA.Repository;
            var CustomerForm = CustomerFormDA.GetQuery()
                        .Select(item => new CustomerFormItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Color = item.Color,
                            IsActive = item.IsActive,
                            IsDelete = item.IsDelete,
                            Note = item.Note,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            return CustomerForm;
        }
        public List<CustomerFormItem> GetActive()
        {
            var CustomerForm = CustomerFormDA.GetQuery(i => i.IsActive && !i.IsDelete)
                        .Select(item => new CustomerFormItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Color = item.Color,
                            RequirementField = item.RequirementField
                        })
                        .OrderByDescending(item => item.Name)
                        .ToList();
            return CustomerForm;
        }
        /// <summary>
        /// Lấy  theo ID
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CustomerFormItem GetById(int Id)
        {
            var CustomerForm = CustomerFormDA.GetQuery(i => i.ID == Id)
                        .Select(item => new CustomerFormItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Color = item.Color,
                            RequirementField = item.RequirementField,
                            IsActive = item.IsActive,
                            IsDelete = item.IsDelete,
                            Note = item.Note,
                        })
                        .First();
            return CustomerForm;
        }

        /// <summary>
        /// Cập nhật 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(CustomerFormItem item, int userID)
        {
            var CustomerForm = CustomerFormDA.GetById(item.ID);
            CustomerForm.ID = item.ID;
            CustomerForm.ID = item.ID;
            CustomerForm.Name = item.Name;
            CustomerForm.Color = item.Color;
            CustomerForm.RequirementField = item.RequirementField;
            CustomerForm.IsActive = item.IsActive;
            CustomerForm.IsDelete = item.IsDelete;
            CustomerForm.Note = item.Note;
            CustomerForm.UpdateAt = DateTime.Now;
            CustomerForm.UpdateBy = userID;
            CustomerFormDA.Save();
        }
        /// <summary>
        /// Thêm mới 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(CustomerFormItem item, int userID)
        {
            var CustomerForm = new CustomerContactForm()
            {
                Name = item.Name,
                Color = item.Color,
                RequirementField = item.RequirementField,
                IsActive = item.IsActive,
                IsDelete = item.IsDelete,
                Note = item.Note,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            CustomerFormDA.Insert(CustomerForm);
            CustomerFormDA.Save();
        }
        /// <summary>
        /// Xóa 
        /// || Author: Thanhnv || CreateDate: 29/05/2015  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            CustomerFormDA.Delete(id);
            CustomerFormDA.Save();
        }
    }
}
