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
    public class IsoMeetingService
    {
        ISOMeetingDA isoMeetingDA = new ISOMeetingDA();
        public List<ISOMeetingItem> GetByISO(int page, int pageSize, out int totalCount, int isoID)
        {
            var lst = isoMeetingDA.GetQuery(p => p.ISOStandardID == isoID)
                             .OrderBy(a => a.Name)
                             .Select(a => new ISOMeetingItem()
                             {
                                 ID = a.ID,
                                 Name = a.Name,
                                 Note = a.Note,
                                 IsoID = a.ISOStandardID,
                                 IsoName = a.ISOStandard.Name,
                                 IsUse = (bool)a.IsUse,
                                 CreateAt = a.CreateAt,
                                 UpdateAt = a.UpdateAt
                             })
                             .OrderByDescending(item => item.CreateAt)
                             .Page(page, pageSize, out totalCount)
                             .ToList();
            return lst;
        }
        public ISOMeetingItem GetByID(int id)
        {
            var itm = isoMeetingDA.GetById(id);
            var obj = new ISOMeetingItem()
            {
                ID = itm.ID,
                IsoID = itm.ISOStandardID,
                IsoName = itm.ISOStandard.Name,
                Name = itm.Name,

                Note = itm.Note,
                IsUse = (bool)itm.IsUse,

                CreateAt = itm.CreateAt,
                UpdateAt = itm.UpdateAt
            };
            return obj;
        }
        public int Insert(ISOMeetingItem item)
        {
            var iso = new ISOMeeting()
            {
                ISOStandardID = item.IsoID,
                Name = item.Name,

                Note = item.Note,
                //IsActive = item.IsActive,
                IsUse = item.IsUse,
                CreateAt = DateTime.Now,
                CreateBy = item.CreateBy
            };
            isoMeetingDA.Insert(iso);
            isoMeetingDA.Save();
            return iso.ID;
        }
        public void Update(ISOMeetingItem item)
        {
            var iso = isoMeetingDA.GetById(item.ID);
            iso.Name = item.Name;

            iso.Note = item.Note;
            iso.IsUse = item.IsUse;
            iso.ISOStandardID = item.IsoID;
            iso.UpdateBy = (int)item.UpdateBy;
            isoMeetingDA.Update(iso);
            isoMeetingDA.Save();
        }

        public void Delete(int id)
        {
            var iso = isoMeetingDA.GetById(id);
            isoMeetingDA.Delete(id);
            isoMeetingDA.Save();
        }
    }
}
