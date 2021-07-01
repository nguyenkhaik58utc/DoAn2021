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
    public class EquipmentMeasureCategorySV
    {
        private EquipmentMeasureCategoryDA CategoryDA = new EquipmentMeasureCategoryDA();
        public void UpdateRoot(int userId)
        {
            var dataRoot = CategoryDA.GetQuery(i => i.ParentID == 0);
            if (dataRoot.FirstOrDefault() == null)
            {
                var root = new EquipmentMeasureCategoryItem()
                {
                    Name = "Nhóm thiết bị đo",
                    ParentID = 0,
                    IsDelete = false,
                };
                Insert(root, userId);
            }
        }
        public IEnumerable<EquipmentMeasureCategory> getChilds(IEnumerable<EquipmentMeasureCategory> e, int? id)
        {
            var EquipmentMeasureCategory = e.Where(a => a.ParentID == id);
            var EquipmentMeasureCategoryFirst = e.Where(a => a.ID == id);
            EquipmentMeasureCategoryFirst.Concat(EquipmentMeasureCategory);
            return EquipmentMeasureCategoryFirst.Concat(EquipmentMeasureCategory.SelectMany(a => getChilds(e, a.ID)));
        }
        private IEnumerable<EquipmentMeasureCategory> getParents(IEnumerable<EquipmentMeasureCategory> e, int? id)
        {
            var listParentNext = e.Where(a => a.ID == id);
            var result = listParentNext.Concat(listParentNext.SelectMany(a => getParents(e, a.ParentID)));
            return result;
        }
        /// <summary>
        /// Lấy danh sách thiết bị đo cho combobox
        /// </summary>
        /// <returns></returns>
        public List<EquipmentMeasureCategoryItem> GetAll()
        {
            var category = CategoryDA.GetQuery()
                        .Select(item => new EquipmentMeasureCategoryItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                        })
                        .OrderByDescending(item => item.Name)
                        .ToList();
            return category;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeId"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public List<EquipmentMeasureCategoryItem> GetTreeData(int nodeId)
        {
            var dbo = CategoryDA.Repository;
            var category = dbo.EquipmentMeasureCategories
                       .Where(i => (i.ParentID != null && i.ParentID == nodeId))
                       .Select(item => new EquipmentMeasureCategoryItem()
                       {
                           ID = item.ID,
                           ParentID = item.ParentID,
                           Name = item.Name,
                           IsDelete = item.IsDelete,
                           Leaf = dbo.EquipmentMeasureCategories.FirstOrDefault(i => i.ParentID == item.ID) == null
                       })
                       .ToList();
            return category;
        }

        public EquipmentMeasureCategoryItem GetById(int Id)
        {
            var result = CategoryDA.GetQuery(i => i.ID == Id)
                        .Select(item => new EquipmentMeasureCategoryItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                        })
                        .First();
            return result;
        }
        /// <summary>
        /// Cập nhật nhóm thiết bị đo
        /// || Author: Thanhnv || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(EquipmentMeasureCategoryItem item, int userID)
        {
            var dbo = CategoryDA.Repository;
            var EquipmentMeasureCategory = CategoryDA.GetById(item.ID);
            EquipmentMeasureCategory.ID = item.ID;
            EquipmentMeasureCategory.Name = item.Name;
            EquipmentMeasureCategory.IsDelete = item.IsDelete;
            EquipmentMeasureCategory.CreateAt = item.CreateAt;
            EquipmentMeasureCategory.CreateBy = item.CreateBy;
            EquipmentMeasureCategory.UpdateAt = DateTime.Now;
            EquipmentMeasureCategory.UpdateBy = userID;
            CategoryDA.Save();
        }
        /// <summary>
        /// Thêm mới nhóm thiết bị đo
        /// || Author: Thanhnv || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public int Insert(EquipmentMeasureCategoryItem item, int userID)
        {
            var EquipmentMeasureCategory = new EquipmentMeasureCategory()
            {
                ID = item.ID,
                ParentID = item.ParentID,
                Name = item.Name,
                IsActive  = item.IsActive,
                IsDelete = false,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            CategoryDA.Insert(EquipmentMeasureCategory);
            CategoryDA.Save();
            return EquipmentMeasureCategory.ID;
        }
        /// <summary>
        /// Xóa nhóm thiết bị đo
        /// || Author: Thanhnv || CreateDate: 26/06/2015  
        /// </summary>
        /// <param name="id"></param>
        public bool Delete(int id)
        {
            var dbo = CategoryDA.Repository;
            if (dbo.EquipmentMeasureCategories.Where(i => i.ParentID == id).FirstOrDefault() == null)
            {
                dbo.EquipmentMeasureCategories.Remove(dbo.EquipmentMeasureCategories.Where(i => i.ID == id).FirstOrDefault());
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
