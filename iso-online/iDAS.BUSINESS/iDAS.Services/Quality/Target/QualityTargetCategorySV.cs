using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class QualityTargetCategorySV
    {
        private QualityTargetCategoryDA targetTypeDA = new QualityTargetCategoryDA();
        public List<QualityTargetCategoryItem> GetAll(ModelFilter filter)
        {
            try
            {
                var lstPhase = targetTypeDA.GetQuery()
                    .Select(item => new QualityTargetCategoryItem()
                    {
                        ID = item.ID,
                        Name = item.Name,
                        ParentID = item.ParentID,
                        Description = item.Description,
                        CreateAt = item.CreateAt,
                        IsActive = item.IsActive,
                        CreateBy = item.CreateBy,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy
                    })
                        .Filter(filter)
                        .ToList();
                return lstPhase;

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public List<QualityTargetCategoryItem> GetAllActive(ModelFilter filter)
        {
            try
            {
                var lstPhase = targetTypeDA.GetQuery(t => t.IsActive)
                    .Select(item => new QualityTargetCategoryItem()
                    {
                        ID = item.ID,
                        Name = item.Name,
                        ParentID = item.ParentID,
                        Description = item.Description,
                        CreateAt = item.CreateAt,
                        IsActive = item.IsActive,
                        CreateBy = item.CreateBy,
                        UpdateAt = item.UpdateAt,
                        UpdateBy = item.UpdateBy
                    })
                    .Filter(filter)
                    .ToList();
                return lstPhase;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public QualityTargetCategoryItem GetByID(int id)
        {
            var targetType = targetTypeDA.GetById(id);
            QualityTargetCategoryItem obj = new QualityTargetCategoryItem();
            obj.ID = targetType.ID;
            obj.Name = targetType.Name;
            obj.Description = targetType.Description;
            obj.IsActive = targetType.IsActive;
            obj.ParentID = targetType.ParentID;
            obj.IsDelete = targetType.IsDelete;
            obj.CreateBy = targetType.CreateBy;
            obj.UpdateAt = targetType.UpdateAt;
            obj.UpdateBy = targetType.UpdateBy;
            obj.CreateAt = targetType.CreateAt;
            return obj;
        }
        public void Insert(QualityTargetCategoryItem objNew, int userId)
        {
            QualityTargetCategory obj = new QualityTargetCategory();

            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            obj.IsActive = objNew.IsActive;
            obj.Name = objNew.Name.Trim();
            obj.ParentID = objNew.ParentID;
            obj.Description = objNew.Description;
            obj.IsDelete = false;
            targetTypeDA.Insert(obj);
            targetTypeDA.Save();
        }

        public void Update(QualityTargetCategoryItem objEdit, int userId)
        {
            QualityTargetCategory obj = targetTypeDA.GetById(objEdit.ID);
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            obj.Description = objEdit.Description;
            obj.IsActive = objEdit.IsActive;
            obj.Name = objEdit.Name.Trim();
            obj.ParentID = objEdit.ParentID;
            targetTypeDA.Update(obj);
            targetTypeDA.Save();
        }
        public void Delete(int id)
        {
            var obj = targetTypeDA.GetById(id);
            targetTypeDA.Delete(obj);
            targetTypeDA.Save();
        }
    }
}
