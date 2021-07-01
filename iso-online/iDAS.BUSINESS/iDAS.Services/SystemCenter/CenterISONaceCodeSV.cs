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
    public class CenterISONaceCodeSV
    {
        private CenterISONaceCodeDA ISONaceCodeDA = new CenterISONaceCodeDA();
        public List<CenterISONaceCodeItem> GetActive()
        {
            var naceCodes = ISONaceCodeDA.GetQuery(i=>i.IsActive)
                        .Select(item => new CenterISONaceCodeItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Code = item.Code,
                            IsActive = item.IsActive,
                            IsDelete = item.IsDelete,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .ToList();
            return naceCodes;
        }
    }
}
