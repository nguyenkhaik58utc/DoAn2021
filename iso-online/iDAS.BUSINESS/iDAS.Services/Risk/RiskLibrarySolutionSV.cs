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
    public class RiskLibrarySolutionSV
    {
        private RiskLibrarySolutionDA RiskLibrarySolutionDA = new RiskLibrarySolutionDA();
        public List<RiskLibrarySolutionItem> GetAll(ModelFilter filter)
        {
            var RiskLibrarySolution = RiskLibrarySolutionDA.GetQuery()
                        .Select(item => new RiskLibrarySolutionItem()
                        {
                            ID = item.ID,
                            Description = item.Description,
                            Content = item.Content,
                            RiskTempControlID = item.RiskTempControlID,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            return RiskLibrarySolution;
        }
        public List<RiskLibrarySolutionItem> GetByRiskTemp(ModelFilter filter, int riskTempControlId)
        {
            var RiskLibrarySolution = RiskLibrarySolutionDA.GetQuery(i => i.RiskTempControlID == riskTempControlId)
                        .Select(item => new RiskLibrarySolutionItem()
                        {
                            ID = item.ID,
                            Description = item.Description,  
                            Content = item.Content,
                            RiskTempControlID = item.RiskTempControlID,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            return RiskLibrarySolution;
        }
        public RiskLibrarySolutionItem GetById(int Id)
        {
            var RiskLibrarySolution = RiskLibrarySolutionDA.GetQuery(i => i.ID == Id)
                        .Select(item => new RiskLibrarySolutionItem()
                        {
                            ID = item.ID,
                            Description = item.Description,
                            Content = item.Content,
                            RiskTempControlID = item.RiskTempControlID,
                        })
                        .FirstOrDefault();
            return RiskLibrarySolution;
        }
        public void Update(RiskLibrarySolutionItem item, int userID)
        {
            var riskLibrarySolution = RiskLibrarySolutionDA.GetById(item.ID);
            riskLibrarySolution.ID = item.ID;
            riskLibrarySolution.Description = item.Description;
            riskLibrarySolution.Content = item.Content;
            riskLibrarySolution.UpdateAt = DateTime.Now;
            riskLibrarySolution.UpdateBy = userID;
            RiskLibrarySolutionDA.Save();
        }
        public void Insert(RiskLibrarySolutionItem item, int userID)
        {
            var RiskLibrarySolution = new RiskLibrarySolution()
            {
                RiskTempControlID = item.RiskTempControlID,
                Description = item.Description,
                Content = item.Content,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            RiskLibrarySolutionDA.Insert(RiskLibrarySolution);
            RiskLibrarySolutionDA.Save();
        }
        public void Delete(int id)
        {
            RiskLibrarySolutionDA.Delete(id);
            RiskLibrarySolutionDA.Save();
        }
    }
}
