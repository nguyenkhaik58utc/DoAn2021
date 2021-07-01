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
    public class HumanAbsentTypeSV
    {
        HumanAbsentTypeDA organizationDA = new HumanAbsentTypeDA();
        public List<HumanAbsentTypeItem> GetAll(ModelFilter filter)
        {
            var dbo = organizationDA.Repository;
            var lst = organizationDA.GetQuery().Select(i => new HumanAbsentTypeItem()
            {
                ID = i.ID,
                Name = i.Name,
                Code = i.Code,
                MaxDayAbsent = i.MaxDayAbsent,
                Note = i.Note
            }).OrderByDescending(item => item.ID)
                        .Filter(filter)
                        .ToList();
            return lst;
        }
    }
}
