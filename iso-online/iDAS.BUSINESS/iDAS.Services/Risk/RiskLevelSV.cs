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
    public class RiskLevelSV
    {
        private RiskLevelDA RiskLevelDA = new RiskLevelDA();
        public List<CenterRiskMethodItem> GetAllRiskMethod()
        {
            return new CenterRiskMethodDA().GetQuery()
                .Select(item => new CenterRiskMethodItem()
                {
                    ID = item.ID,
                    Level = item.Level,
                    Method = item.Method,
                    Color = item.Color,
                    Description = item.Description,
                })
                .ToList();
        }
        public List<RiskLevelItem> GetAll(ModelFilter filter)
        {
            var result = new List<RiskLevelItem>();
            var riskLevels = RiskLevelDA.GetQuery()
                        .Select(item => new RiskLevelItem()
                        {
                            ID = item.ID,
                            CenterRiskMethodID = item.CenterRiskMethodID,
                            MaxPoint = item.PointMax,
                            MinPoint = item.PointMin,
                            CreateAt = item.CreateAt,
                        })
                        .Filter(filter)
                        .ToList();
            var riskMethods = GetAllRiskMethod();
            foreach (var riskLevel in riskLevels)
            {
                var riskMethod = riskMethods.Where(i => i.ID == riskLevel.CenterRiskMethodID).FirstOrDefault();
                if (riskMethod != null)
                {
                    riskLevel.Level = riskMethod.Level;
                    riskLevel.Color = riskMethod.Color;
                    riskLevel.Description = riskMethod.Description;
                    riskLevel.Method = riskMethod.Method;
                }
                result.Add(riskLevel);
            }
            return result;
        }
        public RiskLevelItem GetById(int Id)
        {
            var riskLevel = RiskLevelDA.GetQuery(i => i.ID == Id)
                        .Select(item => new RiskLevelItem()
                        {
                            ID = item.ID,
                            CenterRiskMethodID = item.CenterRiskMethodID,
                            MaxPoint = item.PointMax,
                            MinPoint = item.PointMin,
                        })
                        .First();
            var riskMethod = GetAllRiskMethod().Where(i => i.ID == riskLevel.CenterRiskMethodID).FirstOrDefault();
            if (riskMethod != null)
            {
                riskLevel.Color = riskMethod.Color;
                riskLevel.Description = riskMethod.Description;
                riskLevel.Method = riskMethod.Level;
            }
            return riskLevel;
        }
        public bool Update(RiskLevelItem item, int userID)
        {
            var RiskLevel = RiskLevelDA.GetById(item.ID);
            if (RiskLevelDA.GetQuery().Any(i => i.CenterRiskMethodID == item.CenterRiskMethodID && i.ID != item.ID))
                return false;
            RiskLevel.ID = item.ID;
            RiskLevel.CenterRiskMethodID = item.CenterRiskMethodID;
            RiskLevel.PointMax = item.MaxPoint;
            RiskLevel.PointMin = item.MinPoint;
            RiskLevel.UpdateAt = DateTime.Now;
            RiskLevel.UpdateBy = userID;
            RiskLevelDA.Update(RiskLevel);
            RiskLevelDA.Save();
            return true;
        }
        public bool Insert(RiskLevelItem item, int userID)
        {
            if (RiskLevelDA.GetQuery().Any(i => i.CenterRiskMethodID == item.CenterRiskMethodID))
                return false;
            var RiskLevel = new RiskLevel()
            {
                CenterRiskMethodID = item.CenterRiskMethodID,
                PointMax = item.MaxPoint,
                PointMin = item.MinPoint,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            RiskLevelDA.Insert(RiskLevel);
            RiskLevelDA.Save();
            return true;
        }
        public void Delete(int id)
        {
            RiskLevelDA.Delete(id);
            RiskLevelDA.Save();
        }
    }
}
