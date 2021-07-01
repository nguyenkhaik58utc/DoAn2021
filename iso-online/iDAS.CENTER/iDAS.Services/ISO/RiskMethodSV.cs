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
    public class RiskMethodSV
    {
        private RiskMethodDA RiskMethodDA = new RiskMethodDA();
        public List<RiskMethodItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var RiskMethod = RiskMethodDA.GetQuery()
                        .Select(item => new RiskMethodItem()
                        {
                            ID = item.ID,
                            Color = item.Color,
                            Level = item.Level,
                            Method = item.Method,
                            Description = item.Description,
                            IsActive  = item.IsActive,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return RiskMethod;
        }
        public RiskMethodItem GetById(int Id)
        {
            var RiskMethod = RiskMethodDA.GetQuery(i => i.ID == Id)
                        .Select(item => new RiskMethodItem()
                        {
                            ID = item.ID,
                            Color = item.Color,
                            Level = item.Level,
                            Method = item.Method,
                            Description = item.Description,
                            IsActive = item.IsActive
                        })
                        .First();
            return RiskMethod;
        }
        public void Update(RiskMethodItem item, int userID)
        {
            var riskMethod = RiskMethodDA.GetById(item.ID);
            riskMethod.Color = item.Color;
            riskMethod.Level = item.Level;
            riskMethod.Method = item.Method;
            riskMethod.Description = item.Description;
            riskMethod.IsActive = item.IsActive;
            riskMethod.UpdateAt = DateTime.Now;
            riskMethod.UpdateBy = userID;
            RiskMethodDA.Save();
        }
        public void Insert(RiskMethodItem item, int userID)
        {
            var RiskMethod = new CenterRiskMethod()
            {
                Color = item.Color,
                Level = item.Level,
                Method = item.Method,
                Description = item.Description,
                IsActive = item.IsActive,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            RiskMethodDA.Insert(RiskMethod);
            RiskMethodDA.Save();
        }
        public void Delete(int id)
        {
            RiskMethodDA.Delete(id);
            RiskMethodDA.Save();
        }
    }
}
