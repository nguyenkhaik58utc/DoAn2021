using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.DA;
using iDAS.Base;
using iDAS.Core;
using iDAS.Items;

namespace iDAS.Services
{
    public class RiskCategorySV
    {
        private RiskCategoryDA RiskCategoryDA = new RiskCategoryDA();
        public List<RiskCategoryItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var RiskCategory = RiskCategoryDA.GetQuery()
                        .Select(item => new RiskCategoryItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            ISOName = item.ISOStandard.Code,
                            IsActive = item.IsActive,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                         .Page(page, pageSize, out totalCount)
                        .ToList();
            return RiskCategory;
        }
        public List<RiskCategoryItem> GetByISO(int page, int pageSize, out int totalCount, int isoStandardId)
        {
            var RiskCategory = RiskCategoryDA.GetQuery(i => i.ISOStandardID == isoStandardId)
                        .Select(item => new RiskCategoryItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Description = item.Description,
                            IsActive = item.IsActive,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                         .Page(page, pageSize, out totalCount)
                        .ToList();
            return RiskCategory;
        }
        public RiskCategoryItem GetById(int Id)
        {
            var RiskCategory = RiskCategoryDA.GetQuery(i => i.ID == Id)
                        .Select(item => new RiskCategoryItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            ISOName = item.ISOStandard.Name,
                            ISOStandardID = item.ISOStandardID,
                            Description = item.Description,
                            IsActive = item.IsActive,
                        })
                        .First();
            return RiskCategory;
        }
        public void Update(RiskCategoryItem item, int userID)
        {
            var riskCategory = RiskCategoryDA.GetById(item.ID);
            riskCategory.Name = item.Name;
            riskCategory.ISOStandardID = item.ISOStandardID;
            riskCategory.Description = item.Description;
            riskCategory.IsActive = item.IsActive;
            riskCategory.UpdateAt = DateTime.Now;
            riskCategory.UpdateBy = userID;
            RiskCategoryDA.Save();
        }
        public void Insert(RiskCategoryItem item, int userID)
        {
            var RiskCategory = new CenterRiskCategory()
            {
                Name = item.Name,
                ISOStandardID = item.ISOStandardID,
                Description = item.Description,
                IsActive = item.IsActive,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            RiskCategoryDA.Insert(RiskCategory);
            RiskCategoryDA.Save();
        }
        public void Delete(int id)
        {
            RiskCategoryDA.Delete(id);
            RiskCategoryDA.Save();
        }
    }
}
