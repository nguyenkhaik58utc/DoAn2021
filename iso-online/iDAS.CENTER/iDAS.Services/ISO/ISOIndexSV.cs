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
    public class ISOIndexSV
    {
        ISOIndexDA ISOIndexDA = new ISOIndexDA();
        public List<ISOClauseItem> GetCombo(int page, int pageSize, out int totalCount)
        {
            var lst = ISOIndexDA.GetQuery()
                             .Select(a => new ISOClauseItem()
                             {
                                 ID = a.ID,
                                 Name = a.Clause,
                                 ParentID = a.ParentID,
                                 CreateAt = a.CreateAt
                             })
                             .OrderByDescending(item => item.CreateAt)
                             .Page(page, pageSize, out totalCount)
                             .ToList();
            return lst;
        }
        public List<ISOClauseItem> GetByIso(int page, int pageSize, out int totalCount, int isoStandardId)
        {
            var lst = ISOIndexDA.GetQuery(p => p.ISOStandardID == isoStandardId && p.ParentID == 0)
                             .Select(a => new ISOClauseItem()
                             {
                                 ID = a.ID,
                                 Name = a.Name,
                                 Index = a.Clause,
                                 IsoID = a.ISOStandardID,
                                 IsoName = a.ISOStandard.Name,
                                 IsActive = a.IsActive,
                                 IsDelete = a.IsDelete,
                                 CreateAt = a.CreateAt,
                                 UpdateAt = a.UpdateAt
                             })
                             .OrderByDescending(item => item.CreateAt)
                             .Page(page, pageSize, out totalCount)
                             .ToList();
            return lst;
        }
        public ISOClauseItem GetByID(int id)
        {
            var itm = ISOIndexDA.GetById(id);
            var obj = new ISOClauseItem()
                             {
                                 ID = itm.ID,
                                 IsoID = itm.ISOStandardID,
                                 IsoName = itm.ISOStandard.Name,
                                 Name = itm.Name,
                                 Index = itm.Clause,
                                 Note = itm.Note,
                                 IsActive = (bool)itm.IsActive,
                                 IsDelete = (bool)itm.IsDelete,
                                 CreateAt = itm.CreateAt,
                                 UpdateAt = itm.UpdateAt
                             };
            return obj;
        }
        public int Insert(ISOClauseItem item)
        {
            var iso = new ISOIndex()
            {
                ParentID = 0,
                ISOStandardID = item.IsoID,
                Name = item.Name,
                Clause = item.Index,
                Note = item.Note,
                IsActive = item.IsActive,
                IsDelete = false,
                CreateAt = DateTime.Now,
                CreateBy = item.CreateBy
            };
            ISOIndexDA.Insert(iso);
            ISOIndexDA.Save();
            return iso.ID;
        }
        public void Update(ISOClauseItem item)
        {
            var iso = ISOIndexDA.GetById(item.ID);
            iso.Name = item.Name;
            iso.Clause = item.Index;
            iso.Note = item.Note;
            iso.IsActive = item.IsActive;
            iso.ISOStandardID = item.IsoID;
            iso.UpdateBy = (int)item.UpdateBy;
            ISOIndexDA.Update(iso);
            ISOIndexDA.Save();
        }
        public void Delete(int id)
        {
            var iso = ISOIndexDA.GetById(id);
            ISOIndexDA.Delete(id);
            ISOIndexDA.Save();
        }
        public bool UpdateSystemIsoIndex()
        {
            try
            {
                var dbo = ISOIndexDA.Repository;
                var listIsoIndexs = dbo.ISOIndexes.Where(i => !i.IsUpdateCriteria && i.ParentID == 0).ToList();
                foreach (var isoIndex in listIsoIndexs)
                {
                    var parentId = isoIndex.ID;
                    var isoCriterias = dbo.ISOIndexCriterias.Where(i => i.ISOIndexID == parentId).ToList();
                    foreach (var isoCriteria in isoCriterias)
                    {
                        dbo.ISOIndexes.Add(
                                new ISOIndex()
                                {
                                    ParentID = parentId,
                                    ISOStandardID = isoIndex.ISOStandardID, 
                                    Name = isoCriteria.Name,
                                    Note = isoCriteria.Content,
                                    IsUpdateCriteria =true,
                                    CreateAt = isoCriteria.CreateAt,
                                    CreateBy = isoCriteria.CreateBy,
                                }
                            );
                    }
                    isoIndex.IsUpdateCriteria = true;
                    dbo.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
