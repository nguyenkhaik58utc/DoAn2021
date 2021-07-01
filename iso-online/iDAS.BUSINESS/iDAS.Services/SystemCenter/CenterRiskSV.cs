using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
    public class CenterRiskSV
    {
        private CenterRiskDA riskDA = new CenterRiskDA();
        public List<CenterRiskItem> GetAll(ModelFilter filter)
        {
            var centerRisk = riskDA.GetQuery()
                          .Select(item => new CenterRiskItem()
                          {
                              ID = item.ID,
                              CenterRiskCategoryID = item.CenterRiskCategoryID,
                              Name = item.Name,
                              Description = item.Description,
                              Weakness = item.Weakness,
                              CreateAt = item.CreateAt
                          })
                          .Filter(filter)
                          .ToList();
            return centerRisk;
        }
        public List<CenterRiskItem> GetByCategory(ModelFilter filter, int riskCategoryId)
        {
            var centerRisk = riskDA.GetQuery(i => i.CenterRiskCategoryID == riskCategoryId)
                          .Select(item => new CenterRiskItem()
                          {
                              ID = item.ID,
                              Name = item.Name,
                              Description = item.Description,
                              Weakness = item.Weakness,
                              CreateAt = item.CreateAt
                          })
                          .Filter(filter)
                          .ToList();
            return centerRisk;
        }
        public List<CenterRiskTreatmentItem> GetTreatment(ModelFilter filter, int riskId)
        {
            var centerRiskTreatment = riskDA.GetQuery(i => i.ID == riskId)
                           .SelectMany(i => i.CenterRiskTreatments)
                          .Select(item => new CenterRiskTreatmentItem()
                          {
                              ID = item.ID,
                              CenterRiskMethodID = item.CenterRiskMethodID,
                              Level = item.CenterRiskMethod.Level,
                              Method = item.CenterRiskMethod.Method,
                              Color = item.CenterRiskMethod.Color,
                              Action = item.Action,
                              CreateAt = item.CreateAt
                          })
                          .Filter(filter)
                          .ToList();
            return centerRiskTreatment;
        }
    }
}
