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
    public class CenterRiskMethodSV
    {
        private CenterRiskMethodDA RiskTreatmentDA = new CenterRiskMethodDA();
        public List<CenterRiskMethodItem> GetAll(ModelFilter filter)
        {
            var RiskTreatment = RiskTreatmentDA.GetQuery()
                        .Select(item => new CenterRiskMethodItem()
                        {
                            ID = item.ID,
                            Level = item.Level,
                            Method = item.Method,
                            Description = item.Description,
                            Color = item.Color,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            return RiskTreatment;
        }
    }
}
