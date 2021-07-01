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
    public class RiskTreatmentSV
    {
        private RiskTreatmentDA RiskTreatmentDA = new RiskTreatmentDA();
        public List<RiskTreatmentItem> GetByRisk(ModelFilter filter, int riskId)
        {
            var dbo = RiskTreatmentDA.Repository;
            var Risk = RiskTreatmentDA.GetQuery(i=>i.CenterRiskID == riskId)
                        .Select(item => new RiskTreatmentItem()
                        {
                            ID = item.ID,
                            Level = item.CenterRiskMethod.Level,
                            Method = item.CenterRiskMethod.Method,
                            Color = item.CenterRiskMethod.Color,
                            Action = item.Action,
                            IsActive = item.IsActive,
                            CreateAt = item.CreateAt,
                        })
                        .Filter(filter)
                        .ToList();
            return Risk;
        }
        public RiskTreatmentItem GetById(int Id)
        {
            var Risk = RiskTreatmentDA.GetQuery(i => i.ID == Id)
                        .Select(item => new RiskTreatmentItem()
                        {
                            ID = item.ID,
                            CenterRiskMethodID = item.CenterRiskMethodID,
                            Level = item.CenterRiskMethod.Level,
                            Method = item.CenterRiskMethod.Method,
                            Color = item.CenterRiskMethod.Color,
                            Action = item.Action,
                            CenterRiskID = item.CenterRiskID,
                            IsActive = item.IsActive
                        })
                        .First();
            return Risk;
        }
        public void Update(RiskTreatmentItem item, int userID)
        {
            var risk = RiskTreatmentDA.GetById(item.ID);
            risk.CenterRiskMethodID = item.CenterRiskMethodID;
            risk.Action = item.Action;
            risk.IsActive = item.IsActive;
            risk.UpdateAt = DateTime.Now;
            risk.UpdateBy = userID;
            RiskTreatmentDA.Save();
        }
        public void Insert(RiskTreatmentItem item, int userID)
        {
            var Risk = new CenterRiskTreatment()
            {
                CenterRiskID = item.CenterRiskID,
                CenterRiskMethodID = item.CenterRiskMethodID,
                Action = item.Action,
                IsActive = item.IsActive,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            RiskTreatmentDA.Insert(Risk);
            RiskTreatmentDA.Save();
        }
        public void Delete(int id)
        {
            RiskTreatmentDA.Delete(id);
            RiskTreatmentDA.Save();
        }
    }
}
