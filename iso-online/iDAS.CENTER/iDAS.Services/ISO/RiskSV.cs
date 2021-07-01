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
    public class RiskSV
    {
        private RiskDA RiskDA = new RiskDA();
        public List<RiskItem> GetByCategory(int page, int pageSize, out int totalCount, int categoryId)
        {
            var Risk = RiskDA.GetQuery(i => i.CenterRiskCategoryID == categoryId)
                        .Select(item => new RiskItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Weakness = item.Weakness,
                            Description = item.Description,
                            IsActive = item.IsActive,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return Risk;
        }
        public RiskItem GetById(int id)
        {
            var dbo = RiskDA.Repository;
            var risk = RiskDA.GetQuery(i => i.ID == id)
                        .Select(item => new RiskItem()
                        {
                            ID = item.ID,
                            CenterRiskCategoryID = item.CenterRiskCategoryID,
                            RiskCategoryName = item.CenterRiskCategory.Name,
                            Name = item.Name,
                            Weakness = item.Weakness,
                            Description = item.Description,
                            IsActive = item.IsActive
                        })
                        .FirstOrDefault();
            return risk;
        }
        public void Update(RiskItem item, int userID)
        {
            var dbo = RiskDA.Repository;
            var risk = RiskDA.GetById(item.ID);
            risk.CenterRiskCategoryID = item.CenterRiskCategoryID;
            risk.Name = item.Name;
            risk.Weakness = item.Weakness;
            risk.Description = item.Description;
            risk.IsActive = item.IsActive;
            risk.UpdateAt = DateTime.Now;
            risk.UpdateBy = userID;
            RiskDA.Save();
        }
        public void Insert(RiskItem item, int userID)
        {
            var risk = new CenterRisk()
            {
                CenterRiskCategoryID = item.CenterRiskCategoryID,
                Name = item.Name,
                Weakness = item.Weakness,
                Description = item.Description,
                IsActive = item.IsActive,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            RiskDA.Insert(risk);
            RiskDA.Save();
        }
        public void Delete(int id)
        {
            RiskDA.Delete(id);
            RiskDA.Save();
        }
    }
}
