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
    public class RiskIncedentSV
    {
        private RiskIncedentDA RiskIncedentDA = new RiskIncedentDA();
        public List<RiskIncedentItem> GetAll(ModelFilter filter)
        {
            var RiskIncedent = RiskIncedentDA.GetQuery()
                        .Select(item => new RiskIncedentItem()
                        {
                            ID = item.ID,
                            RiskID = item.RiskID,
                            RiskName = item.Risk.Name,
                            Content = item.Content,
                            Time = item.Time,
                            Title = item.Title,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            return RiskIncedent;
        }
        public List<RiskIncedentItem> GetByRisk(ModelFilter filter, int riskId)
        {
            var RiskIncedent = RiskIncedentDA.GetQuery(i => i.RiskID == riskId)
                        .Select(item => new RiskIncedentItem()
                        {
                            ID = item.ID,
                            RiskID = item.RiskID,
                            RiskName = item.Risk.Name,
                            Content = item.Content,
                            Time = item.Time,
                            Title = item.Title,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Filter(filter)
                        .ToList();
            return RiskIncedent;
        }
        public RiskIncedentItem GetById(int Id)
        {
            var RiskIncedent = RiskIncedentDA.GetQuery(i => i.ID == Id)
                        .Select(item => new RiskIncedentItem()
                        {
                            ID = item.ID,
                            RiskID = item.RiskID,
                            RiskName = item.Risk.Name,
                            Content = item.Content,
                            Time = item.Time,
                            Title = item.Title
                        })
                        .First();
            return RiskIncedent;
        }
        public void Update(RiskIncedentItem item, int userID)
        {
            var riskIncedent = RiskIncedentDA.GetById(item.ID);
            riskIncedent.Content = item.Content;
            riskIncedent.Time = item.Time;
            riskIncedent.Title = item.Title;
            riskIncedent.UpdateAt = DateTime.Now;
            riskIncedent.UpdateBy = userID;
            RiskIncedentDA.Save();
        }
        public void Insert(RiskIncedentItem item, int userID)
        {
            var RiskIncedent = new RiskIncedent()
            {
                RiskID = item.RiskID,
                Content = item.Content,
                Time = item.Time,
                Title = item.Title,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            RiskIncedentDA.Insert(RiskIncedent);
            RiskIncedentDA.Save();
        }
        public void Delete(int id)
        {
            RiskIncedentDA.Delete(id);
            RiskIncedentDA.Save();
        }
    }
}
