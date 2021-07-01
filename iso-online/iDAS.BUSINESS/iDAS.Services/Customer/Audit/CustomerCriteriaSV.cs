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
    public class CustomerCriteriaSV
    {
        private CustomerCriteriaDA CustomerCriteriaDA = new CustomerCriteriaDA();
        /// <summary>
        /// Lấy danh sách Tiêu chí đánh giá
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CustomerCriteriaItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var dbo = CustomerCriteriaDA.Repository;
            var CustomerCriteria = CustomerCriteriaDA.GetQuery()
                        .Select(item => new CustomerCriteriaItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            IsActive = item.IsActive,
                            MinPoint = item.MinPoint,
                            MaxPoint = item.MaxPoint,
                            Factory = item.Factory,
                            IsDelete = item.IsDelete,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return CustomerCriteria;
        }
        /// <summary>
        /// Lấy danh sách tiêu chí theo tiêu chi cha
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public List<CustomerCriteriaItem> GetByCategory(ModelFilter filter, int categoryId)
        {
            var dbo = CustomerCriteriaDA.Repository;
            var criterias = dbo.CustomerCriterias.Where(t => t.CustomerCriteriaCategoryID == categoryId)
                          .Select(item => new CustomerCriteriaItem()
                          {
                              ID = item.ID,
                              Name = item.Name,
                              Factory = item.Factory,
                              MaxPoint = item.MaxPoint,
                              MinPoint = item.MinPoint,
                              IsActive = item.IsActive,
                              IsDelete = item.IsDelete,
                              CreateAt = item.CreateAt
                          })
                          .OrderByDescending(item => item.CreateAt)
                          .Filter(filter)
                          .ToList();
            return criterias;
        }
        /// <summary>
        /// Lấy Tiêu chí đánh giá theo ID
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CustomerCriteriaItem GetById(int Id)
        {
            var CustomerCriteria = CustomerCriteriaDA.GetQuery(i => i.ID == Id)
                        .Select(i => new CustomerCriteriaItem()
                        {
                            ID = i.ID,
                            Name = i.Name,
                            IsActive = i.IsActive,
                            IsDelete = i.IsDelete,
                            CreateAt = i.CreateAt,
                            MinPoint = i.MinPoint,
                            MaxPoint = i.MaxPoint,
                            Factory = i.Factory,
                            CategoryID = i.CustomerCriteriaCategory.ID,
                            CategoryName = i.CustomerCriteriaCategory.Name,
                        })
                        .First();
            return CustomerCriteria;
        }

        /// <summary>
        /// Cập nhật Tiêu chí đánh giá
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public bool Update(CustomerCriteriaItem item, int userID)
        {
            var dbo = CustomerCriteriaDA.Repository;
            CustomerCriteria obj = dbo.CustomerCriterias.FirstOrDefault(i => i.ID == item.ID);
            if (dbo.CustomerCriterias.FirstOrDefault(t => t.CustomerCriteriaCategoryID == item.CategoryID && t.Name.ToUpper() == item.Name.ToUpper()) != null
                 && (obj.Name.ToUpper() != item.Name.ToUpper()))
            {
                return false;
            }
            obj.Name = item.Name;
            obj.MinPoint = item.MinPoint;
            obj.MaxPoint = item.MaxPoint;
            obj.Factory = item.Factory;
            obj.IsActive = item.IsActive;
            obj.IsDelete = item.IsDelete;
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userID;
            dbo.SaveChanges();
            return true;
        }
        /// <summary>
        /// Thêm mới Tiêu chí đánh giá
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public bool Insert(CustomerCriteriaItem item, int userID)
        {
            var dbo = CustomerCriteriaDA.Repository;
            CustomerCriteria obj = new CustomerCriteria();
            if (dbo.CustomerCriterias.FirstOrDefault(t =>t.CustomerCriteriaCategoryID ==item.CategoryID && t.Name.ToUpper() == item.Name.ToUpper()) != null)
            {
                return false;
            }

            obj.Name = item.Name;
            obj.IsActive = item.IsActive;
            obj.IsDelete = item.IsDelete;
            obj.MinPoint = item.MinPoint;
            obj.MaxPoint = item.MaxPoint;
            obj.Factory = item.Factory;
            obj.CustomerCriteriaCategoryID = item.CategoryID;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userID;
            dbo.CustomerCriterias.Add(obj);
            dbo.SaveChanges();
            return true;
        }
        /// <summary>
        /// Xóa Tiêu chí đánh giá
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            CustomerCriteriaDA.Delete(id);
            CustomerCriteriaDA.Save();
        }
    }
}
