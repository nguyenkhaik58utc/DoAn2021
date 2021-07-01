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
    public class HumanTrainingPractionerRankSV
    {
        HumanTrainingPractionerRankDA sLevelDA = new HumanTrainingPractionerRankDA();
        public object GetAll(int page, int pageSize, out int totalCount)
        {
            var services = sLevelDA.GetQuery()
                    .Select(i => new HumanTrainingPractionerRankItem()
                    {
                        ID = i.ID,
                        RankName = i.RankName,
                        Descripson = i.Descripson,
                        IsUse = i.IsUse,
                        CreateAt = i.CreateAt
                    })
                    .OrderByDescending(item => item.CreateAt)
                    .Page(page, pageSize, out totalCount)
                    .ToList();
            return services;
        }

        public HumanTrainingPractionerRankItem GetByID(int id)
        {
            var i = sLevelDA.GetById(id);
            if (i != null)
                return new HumanTrainingPractionerRankItem()
                {
                    ID = i.ID,
                    RankName = i.RankName,
                    Descripson = i.Descripson,
                    IsUse = i.IsUse,
                    CreateAt = i.CreateAt,
                    CreateBy = i.CreateBy,
                    UpdateAt = i.UpdateAt,
                    UpdateBy = i.UpdateBy
                };
            return null;
        }

        public bool CheckNotIsUse(int id)
        {
            HumanTrainingPractionerDA doc = new HumanTrainingPractionerDA();
            var obj = doc.GetQuery(p => p.Rank == id);
            if (obj.Count() > 0)
                return false;

            return true;
        }

        public HumanTrainingPractionerRankItem GetByName(string name)
        {
            name = name.Trim().ToLower();
            var i = sLevelDA.GetQuery(p => p.RankName.Trim().ToLower().Equals(name))
                  .FirstOrDefault();
            if (i != null)
                return new HumanTrainingPractionerRankItem()
                {
                    ID = i.ID,
                    RankName = i.RankName,
                    Descripson = i.Descripson,
                    IsUse = i.IsUse,
                    CreateAt = i.CreateAt,
                    CreateBy = i.CreateBy,
                    UpdateAt = i.UpdateAt,
                    UpdateBy = i.UpdateBy
                };

            return null;
        }

        public void Insert(HumanTrainingPractionerRankItem obj)
        {
            try
            {
                var itm = new HumanTrainingPractionerRank
                {
                    RankName = obj.RankName,
                    Descripson = obj.Descripson,
                    IsUse = obj.IsUse,
                    CreateAt = DateTime.Now,
                    CreateBy = obj.CreateBy
                };
                sLevelDA.Insert(itm);
                sLevelDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Update(HumanTrainingPractionerRankItem obj)
        {
            var itm = sLevelDA.GetById(obj.ID);
            itm.RankName = obj.RankName;
            itm.Descripson = obj.Descripson;
            itm.IsUse = obj.IsUse;
            itm.UpdateAt = DateTime.Now;
            itm.UpdateBy = obj.UpdateBy;
            sLevelDA.Update(itm);
            sLevelDA.Save();
        }

        public void Delete(int id)
        {
            try
            {
                var item = sLevelDA.GetById(id);
                sLevelDA.Delete(item);
                sLevelDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<HumanTrainingPractionerRankItem> GetAllUse(int page, int pageSize, out int totalCount)
        {
            var services = sLevelDA.GetQuery().Where(x=>x.IsUse==true)
                    .Select(i => new HumanTrainingPractionerRankItem()
                    {
                        ID = i.ID,
                        RankName = i.RankName,
                        Descripson = i.Descripson,
                        IsUse = i.IsUse
                    })
                    .OrderByDescending(item => item.IsUse)
                    .Page(page, pageSize, out totalCount)
                    .ToList();
            return services;
        }
    }
}
