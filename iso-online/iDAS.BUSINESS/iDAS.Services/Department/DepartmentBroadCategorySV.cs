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
    public class DepartmentBroadCategorySV
    {
        private DepartmentBroadCategoryDA BroadCategoryDA = new DepartmentBroadCategoryDA();
        /// <summary>
        /// Lấy danh sách bộ tiêu chí đánh giá
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<DepartmentBroadCategoryItem> GetTreeData(string nodeId)
        {
            int parentId = nodeId == "root" ? 0 : Convert.ToInt32(nodeId);
            var dbo = BroadCategoryDA.Repository;
            var lsDataNode = dbo.DepartmentBroadCategories.Where(i => i.ParentID == parentId)
                                            .Select(item => new DepartmentBroadCategoryItem()
                                            {
                                                ID = item.ID,
                                                Name = item.Name,
                                                Description = item.Description,
                                                ParentID = item.ParentID,
                                                Leaf = dbo.DepartmentBroadCategories.FirstOrDefault(i => i.ParentID == item.ID) == null
                                            }
                                           ).ToList();
            return lsDataNode;
        }
        /// <summary>
        /// Lấy bộ tiêu chí đánh giá theo ID
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public DepartmentBroadCategoryItem GetById(int Id)
        {
            var DepartmentBroad = BroadCategoryDA.GetQuery(i => i.ID == Id)
                        .Select(item => new DepartmentBroadCategoryItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            ParentID = item.ParentID,
                            Description = item.Description,
                            CreateAt = item.CreateAt,
                        })
                        .First();
            return DepartmentBroad;
        }

        /// <summary>
        /// Cập nhật bộ tiêu chí đánh giá
        /// || Author: Thanhnv || CreateDate: 23/01/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public bool Update(DepartmentBroadCategoryItem item, int userID)
        {
            var dbo = BroadCategoryDA.Repository;
            DepartmentBroadCategory obj = dbo.DepartmentBroadCategories.FirstOrDefault(i => i.ID == item.ID);
            if (dbo.DepartmentBroadCategories.FirstOrDefault(t => t.Name.ToUpper() == item.Name.ToUpper()) != null
                 && (obj.Name.ToUpper() != item.Name.ToUpper()))
            {
                return false;
            }
            obj.Name = item.Name;
            obj.ParentID = item.ParentID;
            obj.Description = item.Description;
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
        public bool Insert(DepartmentBroadCategoryItem objNew, int userID)
        {
            var dbo = BroadCategoryDA.Repository;
            DepartmentBroadCategory obj = new DepartmentBroadCategory();
            if (dbo.DepartmentBroadCategories.FirstOrDefault(t => t.Name.ToUpper() == objNew.Name.ToUpper()) != null)
            {
                return false;
            }
            obj.Name = objNew.Name;
            obj.ParentID = objNew.ParentID;
            obj.Description = objNew.Description;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = objNew.CreateBy;
            dbo.DepartmentBroadCategories.Add(obj);
            dbo.SaveChanges();
            return true;
        }
        public bool Delete(int id)
        {
            var dbo = BroadCategoryDA.Repository;
            var childs = dbo.DepartmentBroadCategories.FirstOrDefault(i => i.ParentID == id);
            if (childs != null)
            {
                return false;
            }
            else
            {
                var obj = BroadCategoryDA.GetById(id);
                BroadCategoryDA.Delete(obj);
                BroadCategoryDA.Save();
                return true;
            }
        }
    }
}
