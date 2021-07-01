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
        private ISOControlDA ISOControlDA = new ISOControlDA();
        public List<ISOControlItem> GetByISO(int page, int pageSize, out int totalCount, int isoStandardId)
        {
            var riskTempControl = ISOControlDA.GetQuery(i => i.ISOStandardID == isoStandardId)
                        .Select(item => new ISOControlItem()
                        {
                            ID = item.ID,
                            Clause = item.Clause,
                            Target = item.Target,
                            Control = item.Control,
                            Description = item.Description,
                            IsActive = item.IsActive,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                         .Page(page, pageSize, out totalCount)
                        .ToList();
            return riskTempControl;
        }
        public ISOControlItem GetById(int Id)
        {
            var riskTempControl = ISOControlDA.GetQuery(i => i.ID == Id)
                        .Select(item => new ISOControlItem()
                        {
                            ID = item.ID,
                            ISOStandardID = item.ISOStandardID,
                            ISOName = item.ISOStandard.Name,
                            Clause = item.Clause,
                            Target = item.Target,
                            Control = item.Control,
                            Description = item.Description,
                            IsActive = item.IsActive,
                        })
                        .First();
            return riskTempControl;
        }
        public void Update(ISOControlItem item, int userID)
        {
            var riskTempControl = ISOControlDA.GetById(item.ID);
            riskTempControl.ISOStandardID = item.ISOStandardID;
            riskTempControl.Clause = item.Clause;
            riskTempControl.Target = item.Target;
            riskTempControl.Control = item.Control;
            riskTempControl.Description = item.Description;
            riskTempControl.UpdateAt = DateTime.Now;
            riskTempControl.UpdateBy = userID;
            ISOControlDA.Save();
        }
        public void Insert(ISOControlItem item, int userID)
        {
            var riskTempControl = new ISOControl()
            {
                ISOStandardID = item.ISOStandardID,
                Clause = item.Clause,
                Target = item.Target,
                Control = item.Control,
                Description = item.Description,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            ISOControlDA.Insert(riskTempControl);
            ISOControlDA.Save();
        }
        public void Delete(int id)
        {
            ISOControlDA.Delete(id);
            ISOControlDA.Save();
        }
    }
}
