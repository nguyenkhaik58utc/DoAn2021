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
    public class DepartmentBroadSV
    {
        private DepartmentBroadDA DepartmentBroadDA = new DepartmentBroadDA();
        /// <summary>
        /// Lấy danh sách Tiêu chí đánh giá
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<DepartmentBroadItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var dbo = DepartmentBroadDA.Repository;
            var DepartmentBroad = DepartmentBroadDA.GetQuery()
                        .Select(item => new DepartmentBroadItem()
                        {
                            ID = item.ID,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            ;
            return DepartmentBroad;
        }
        /// <summary>
        /// Lấy danh sách tiêu chí theo tiêu chi cha
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="ParentID"></param>
        /// <returns></returns>
        public List<DepartmentBroadItem> GetByCategory(int page, int pageSize, out int total, int categoryId)
        {
            var dbo = DepartmentBroadDA.Repository;
            var criterias = dbo.DepartmentBroads.Where(t => t.DepartmentBroadCategoryID == categoryId)
                          .Select(item => new DepartmentBroadItem()
                          {
                              ID = item.ID,
                              Title = item.Title,
                              Description = item.Description,
                              CreateAt = item.CreateAt
                          })
                          .OrderByDescending(item => item.CreateAt)
                          .Page(page, pageSize, out total)
                          .ToList();
            return criterias;
        }
        /// <summary>
        /// Lấy Tiêu chí đánh giá theo ID
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public DepartmentBroadItem GetById(int Id)
        {
            var DepartmentBroad = DepartmentBroadDA.GetQuery(i => i.ID == Id)
                        .Select(i => new DepartmentBroadItem()
                        {
                            ID = i.ID,
                            CategoryID = i.DepartmentBroadCategory.ID,
                            CategoryName = i.DepartmentBroadCategory.Name,
                            Title = i.Title,
                            Description = i.Description,
                            Content = i.Content,
                        })
                        .First();
            return DepartmentBroad;
        }

        /// <summary>
        /// Cập nhật Tiêu chí đánh giá
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public bool Update(DepartmentBroadItem item, int userID)
        {
            var dbo = DepartmentBroadDA.Repository;
            DepartmentBroad obj = dbo.DepartmentBroads.FirstOrDefault(i => i.ID == item.ID);
            if (dbo.DepartmentBroads.FirstOrDefault(t => t.Title.ToUpper() == item.Title.ToUpper()) != null
                 && (obj.Title.ToUpper() != item.Title.ToUpper()))
            {
                return false;
            }
            obj.Title = item.Title;
            obj.Description = item.Description;
            obj.Content = item.Content;
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
        public bool Insert(DepartmentBroadItem item, int userID)
        {
            var dbo = DepartmentBroadDA.Repository;
            DepartmentBroad obj = new DepartmentBroad();
            if (dbo.DepartmentBroads.FirstOrDefault(t => t.Title.ToUpper() == item.Title.ToUpper()) != null)
            {
                return false;
            }

            obj.Title = item.Title;
            obj.Description = item.Description;
            obj.Content = item.Content;
            obj.DepartmentBroadCategoryID = item.CategoryID;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userID;
            dbo.DepartmentBroads.Add(obj);
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
            DepartmentBroadDA.Delete(id);
            DepartmentBroadDA.Save();
        }
    }
}
