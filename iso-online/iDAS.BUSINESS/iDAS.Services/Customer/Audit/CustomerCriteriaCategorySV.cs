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
    public class CustomerCriteriaCategorySV
    {
        private CustomerCriteriaCategoryDA CriteriaCategoryDA = new CustomerCriteriaCategoryDA();
        /// <summary>
        /// Lấy danh sách bộ tiêu chí đánh giá
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CustomerCriteriaCategoryItem> GetTreeData(string nodeId)
        {
            int parentId = nodeId == "root" ? 0 : Convert.ToInt32(nodeId);
            var dbo = CriteriaCategoryDA.Repository;
            var lsDataNode = dbo.CustomerCriteriaCategories.Where(i => i.ParentID == parentId && i.IsDelete == false)
                                            .Select(item => new CustomerCriteriaCategoryItem()
                                            {
                                                ID = item.ID,
                                                Name = item.Name,
                                                ParentID = item.ParentID,
                                                IsDelete = item.IsDelete,
                                                IsActive = item.IsActive,
                                                Leaf = dbo.CustomerCriteriaCategories.FirstOrDefault(i => i.ParentID == item.ID) == null
                                            }
                                           ).ToList();
            return lsDataNode;
        }
        public List<CustomerCriteriaCategoryItem> GetActive()
        {
            return CriteriaCategoryDA.GetQuery(i => i.IsActive && !i.IsDelete).Select(item => new CustomerCriteriaCategoryItem()
                                            {
                                                ID = item.ID,
                                                Name = item.Name
                                            }).ToList();

        }
        /// <summary>
        /// Lấy bộ tiêu chí đánh giá theo ID
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public CustomerCriteriaCategoryItem GetById(int Id)
        {
            var CustomerCriteria = CriteriaCategoryDA.GetQuery(i => i.ID == Id)
                        .Select(item => new CustomerCriteriaCategoryItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            ParentID = item.ParentID,
                            Note = item.Note,
                            IsActive = item.IsActive,
                            IsDelete = item.IsDelete,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .First();
            return CustomerCriteria;
        }

        /// <summary>
        /// Cập nhật bộ tiêu chí đánh giá
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public bool Update(CustomerCriteriaCategoryItem item, int userID)
        {
            var dbo = CriteriaCategoryDA.Repository;
            CustomerCriteriaCategory obj = dbo.CustomerCriteriaCategories.FirstOrDefault(i => i.ID == item.ID);
            if (dbo.CustomerCriteriaCategories.FirstOrDefault(t => t.Name.ToUpper() == item.Name.ToUpper()) != null
                 && (obj.Name.ToUpper() != item.Name.ToUpper()))
            {
                return false;
            }
            obj.Name = item.Name;
            obj.ParentID = item.ParentID;
            obj.Note = item.Note;
            obj.IsActive = item.IsActive;
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userID;
            dbo.SaveChanges();
            return true;
        }
        /// <summary>
        /// Thêm mới bộ tiêu chí đánh giá
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public bool Insert(CustomerCriteriaCategoryItem objNew, int userID)
        {
            var dbo = CriteriaCategoryDA.Repository;
            CustomerCriteriaCategory obj = new CustomerCriteriaCategory();
            if (dbo.CustomerCriteriaCategories.FirstOrDefault(t => t.Name.ToUpper() == objNew.Name.ToUpper()) != null)
            {
                return false;
            }
            obj.Name = objNew.Name;
            obj.ParentID = objNew.ParentID;
            obj.Note = objNew.Note;
            obj.IsTerm = objNew.IsTerm;
            obj.IsActive = objNew.IsActive;
            obj.IsDelete = objNew.IsDelete;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = objNew.CreateBy;
            dbo.CustomerCriteriaCategories.Add(obj);
            dbo.SaveChanges();
            return true;
        }
        public bool Delete(int id)
        {
            var dbo = CriteriaCategoryDA.Repository;
            var childs = dbo.CustomerCriteriaCategories.FirstOrDefault(i => i.ParentID == id);
            if (childs != null)
            {
                return false;
            }
            else
            {
                var obj = CriteriaCategoryDA.GetById(id);
                CriteriaCategoryDA.Delete(obj);
                CriteriaCategoryDA.Save();
                return true;
            }
        }
    }
}
