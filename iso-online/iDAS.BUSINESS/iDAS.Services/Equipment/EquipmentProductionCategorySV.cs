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
    public class EquipmentProductionCategorySV
    {
        private EquipmentProductionCategoryDA CategoryDA = new EquipmentProductionCategoryDA();
        public void UpdateRoot(int userId)
        {
            var dataRoot = CategoryDA.GetQuery(i => i.ParentID == 0);
            if (dataRoot.FirstOrDefault() == null)
            {
                var root = new EquipmentProductionCategoryItem()
                {
                    Name = "Nhóm thiết bị sản xuất",
                    ParentID = 0,
                    IsDelete = false,
                };
                Insert(root, userId);
            }
        }
        public IEnumerable<EquipmentProductionCategory> getChilds(IEnumerable<EquipmentProductionCategory> e, int? id)
        {
            var EquipmentProductionCategory = e.Where(a => a.ParentID == id);
            var EquipmentProductionCategoryFirst = e.Where(a => a.ID == id);
            EquipmentProductionCategoryFirst.Concat(EquipmentProductionCategory);
            return EquipmentProductionCategoryFirst.Concat(EquipmentProductionCategory.SelectMany(a => getChilds(e, a.ID)));
        }
        private IEnumerable<EquipmentProductionCategory> getParents(IEnumerable<EquipmentProductionCategory> e, int? id)
        {
            var listParentNext = e.Where(a => a.ID == id);
            var result = listParentNext.Concat(listParentNext.SelectMany(a => getParents(e, a.ParentID)));
            return result;
        }
        /// <summary>
        /// Lấy danh sách thiết bị sản xuất cho combobox
        /// </summary>
        /// <returns></returns>
        public List<EquipmentProductionCategoryItem> GetAll()
        {
            var category = CategoryDA.GetQuery()
                        .Select(item => new EquipmentProductionCategoryItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            IsDelete = item.IsDelete,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .OrderByDescending(item => item.Name)
                        .ToList();
            ;
            return category;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public List<EquipmentProductionCategoryItem> GetTreeData(int nodeId)
        {
            var dbo = CategoryDA.Repository;
            var category = dbo.EquipmentProductionCategories
                       .Where(i => (i.ParentID != null && i.ParentID == nodeId))
                       .Select(item => new EquipmentProductionCategoryItem()
                       {
                           ID = item.ID,
                           ParentID = item.ParentID,
                           Name = item.Name,
                           IsDelete = item.IsDelete,
                           Leaf = dbo.EquipmentProductionCategories.FirstOrDefault(i => i.ParentID == item.ID) == null
                       })
                       .ToList();
            return category;
        }

        public EquipmentProductionCategoryItem GetById(int Id)
        {
            var result = CategoryDA.GetQuery(i => i.ID == Id)
                        .Select(item => new EquipmentProductionCategoryItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                        })
                        .First();
            return result;
        }
        /// <summary>
        /// Cập nhật nhóm thiết bị sản xuất
        /// || Author: Thanhnv || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(EquipmentProductionCategoryItem item, int userID)
        {
            var dbo = CategoryDA.Repository;
            var EquipmentProductionCategory = CategoryDA.GetById(item.ID);
            EquipmentProductionCategory.ID = item.ID;
            EquipmentProductionCategory.Name = item.Name;
            EquipmentProductionCategory.IsDelete = item.IsDelete;
            EquipmentProductionCategory.CreateAt = item.CreateAt;
            EquipmentProductionCategory.CreateBy = item.CreateBy;
            EquipmentProductionCategory.UpdateAt = DateTime.Now;
            EquipmentProductionCategory.UpdateBy = userID;
            CategoryDA.Save();
        }
        /// <summary>
        /// Thêm mới nhóm thiết bị sản xuất
        /// || Author: Thanhnv || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public int Insert(EquipmentProductionCategoryItem item, int userID)
        {
            var EquipmentProductionCategory = new EquipmentProductionCategory()
            {
                ID = item.ID,
                ParentID = item.ParentID,
                Name = item.Name,
                IsActive  = item.IsActive,
                IsDelete = false,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            CategoryDA.Insert(EquipmentProductionCategory);
            CategoryDA.Save();
            return EquipmentProductionCategory.ID;
        }
        /// <summary>
        /// Xóa nhóm thiết bị sản xuất
        /// || Author: Thanhnv || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="id"></param>
        public bool Delete(int id)
        {
            var dbo = CategoryDA.Repository;
            if (dbo.EquipmentProductionCategories.Where(i => i.ParentID == id).FirstOrDefault() == null)
            {
                dbo.EquipmentProductionCategories.Remove(dbo.EquipmentProductionCategories.Where(i => i.ID == id).FirstOrDefault());
                dbo.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool checkNameExist(int? parentID, string name)
        {
            var obj = CategoryDA.GetQuery(i => i.ParentID == parentID && i.Name.ToString().ToUpper() == name.ToString().ToUpper()).FirstOrDefault();
            if (obj != null)
                return true;
            else
                return false;
        }
        public bool checkNameExist(int? parentID, string name, int id)
        {
            var obj = CategoryDA.GetQuery(i => i.ParentID == parentID && i.Name.ToString().ToUpper() == name.ToString().ToUpper()).FirstOrDefault();
            var obj1 = CategoryDA.GetById(id);
            if (obj != null && obj1.Name != name)
                return true;
            else
                return false;
        }
    }
}
