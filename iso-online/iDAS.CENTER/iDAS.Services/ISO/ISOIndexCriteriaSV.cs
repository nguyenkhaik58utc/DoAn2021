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
    public class ISOIndexCriteriaSV
    {
        ISOIndexCriteriaDA ISOIndexCriteriaDA = new ISOIndexCriteriaDA();
        public List<ISOIndexCriteriaItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var result = ISOIndexCriteriaDA.GetQuery()
                             .OrderBy(a => a.Name)
                             .Select(item => new ISOIndexCriteriaItem()
                             {
                                 ID = item.ID,
                                 Name = item.Name,
                                 Content = item.Content,
                                 ISOIndexID = item.ISOIndexID,
                                 IsDelete = item.IsDelete,
                                 IsUse = item.IsUse,
                                 CreateAt = item.CreateAt,
                             })
                             .OrderByDescending(item => item.CreateAt)
                             .Page(page, pageSize, out totalCount)
                             .ToList();
            return result;
        }
        public List<ISOIndexCriteriaItem> GetByIsoIndexID(int page, int pageSize, out int totalCount, int isoIndexID)
        {
            var result = ISOIndexCriteriaDA.GetQuery(p => p.ISOIndexID == isoIndexID)
                             .OrderBy(a => a.Name)
                             .Select(item => new ISOIndexCriteriaItem()
                             {
                                 ID = item.ID,
                                 Name = item.Name,
                                 Content = item.Content,
                                 ISOIndexID = item.ISOIndexID,
                                 IsDelete = item.IsDelete,
                                 IsUse = item.IsUse,
                                 CreateAt = item.CreateAt,
                             })
                             .OrderByDescending(item => item.CreateAt)
                             .Page(page, pageSize, out totalCount)
                             .ToList();
            return result;
        }
        public ISOIndexCriteriaItem GetByID(int id)
        {
            var item = ISOIndexCriteriaDA.GetById(id);
            var obj = new ISOIndexCriteriaItem()
            {
                ID = item.ID,
                Name = item.Name,
                Content = item.Content,
                ISOIndexID = item.ISOIndexID,
                IsDelete = item.IsDelete,
                IsUse = item.IsUse,
            };
            return obj;
        }
        public int Insert(ISOIndexCriteriaItem item)
        {
            var iso = new ISOIndexCriteria()
            {
                Name = item.Name,
                Content = item.Content,
                ISOIndexID = item.ISOIndexID,
                IsUse = item.IsUse,
                CreateAt = DateTime.Now,
                CreateBy = item.CreateBy
            };
            ISOIndexCriteriaDA.Insert(iso);
            ISOIndexCriteriaDA.Save();
            return iso.ID;
        }
        public void Update(ISOIndexCriteriaItem item)
        {
            var iso = ISOIndexCriteriaDA.GetById(item.ID);
            iso.Name = item.Name;
            iso.Content = item.Content;
            iso.ISOIndexID = item.ISOIndexID;
            iso.IsUse = item.IsUse;
            iso.UpdateAt = DateTime.Now;
            iso.UpdateBy = item.UpdateBy;
            ISOIndexCriteriaDA.Update(iso);
            ISOIndexCriteriaDA.Save();
        }
        public void Delete(int id)
        {
            var iso = ISOIndexCriteriaDA.GetById(id);
            ISOIndexCriteriaDA.Delete(id);
            ISOIndexCriteriaDA.Save();
        }
    }

}
