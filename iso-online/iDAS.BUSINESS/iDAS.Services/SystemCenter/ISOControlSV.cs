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
    public class ISOControlSV
    {
        private ISOControlDA RiskTempControlDA = new ISOControlDA();
        public List<ISOControlItem> GetAll(ModelFilter filter)
        {
            var RiskTempControl = RiskTempControlDA.GetQuery()
                        .Select(item => new ISOControlItem()
                        {
                            ID = item.ID,
                            ISOCode = item.ISOStandard.Code,
                            Clause = item.Clause,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                         .Filter(filter)
                        .ToList();
            return RiskTempControl;
        }
        public List<ISOControlItem> GetActive(ModelFilter filter)
        {
            var RiskTempControl = RiskTempControlDA.GetQuery(i=>i.ISOStandard.IsActive)
                        .Select(item => new ISOControlItem()
                        {
                            ID = item.ID,
                            Clause = item.Clause,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                         .Filter(filter)
                        .ToList();
            return RiskTempControl;
        }
        public ISOControlItem GetByID(int id)
        {
            var RiskTempControl = RiskTempControlDA.GetQuery(i => i.ID == id)
                        .Select(item => new ISOControlItem()
                        {
                            ID = item.ID,
                            ISOCode = item.ISOStandard.Code,
                            Clause = item.Clause,
                            Target = item.Target,
                            Control = item.Control,
                            Description = item.Description,
                            CreateAt = item.CreateAt,
                        })
                        .FirstOrDefault();
            return RiskTempControl;
        }
    }
}
