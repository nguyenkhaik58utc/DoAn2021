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
    public class DepartmentRequirmentCategorySV
    {
        private DepartmentRequirmentCategoryDA DepartmentRequirmentCategoryDA = new DepartmentRequirmentCategoryDA();
        public List<DepartmentRequirmentItem> GetAll()
        {
            var dbo = DepartmentRequirmentCategoryDA.Repository;
            var category = DepartmentRequirmentCategoryDA.GetQuery()
                        .Select(item => new DepartmentRequirmentItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .ToList();
            return category;
        }
        public void Update(DepartmentRequirmentCategoryItem item, int userID)
        {
            var category = DepartmentRequirmentCategoryDA.GetById(item.ID);
            category.Name = item.Name;
            category.UpdateAt = DateTime.Now;
            category.UpdateBy = userID;
            DepartmentRequirmentCategoryDA.Save();
        }
        public int Insert(DepartmentRequirmentCategoryItem item, int userID)
        {
            var category = new CenterDepartmentRequirmentCategory()
            {
                Name = item.Name,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            DepartmentRequirmentCategoryDA.Insert(category);
            DepartmentRequirmentCategoryDA.Save();
            return category.ID;
        }
        public bool Delete(int id)
        {
            DepartmentRequirmentCategoryDA.Delete(id);
            DepartmentRequirmentCategoryDA.Save();
            return true;
        }
    }
}
