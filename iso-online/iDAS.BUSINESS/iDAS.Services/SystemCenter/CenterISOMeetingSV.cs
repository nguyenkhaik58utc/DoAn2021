using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class ISOMeetingSV
    {
        private CenterISOMeetingDA ISOMeetingDA = new CenterISOMeetingDA();
        public List<CenterISOMeetingItem> GetAll(int page, int pageSize, out int totalCount)
        {

            var IsoMeeting = ISOMeetingDA.GetQuery()
                        .Select(item => new CenterISOMeetingItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Note = item.Note,
                            CreateAt = item.CreateAt,

                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return IsoMeeting;
        }
        /// <summary>
        /// Lấy tất cả  
        /// || Author: Thanhnv || CreateDate: 02/02/2015
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CenterISOMeetingItem> GetByIsoId(int page, int pageSize, out int totalCount, int IsoID)
        {

            var IsoMeeting = ISOMeetingDA.GetQuery(i => i.ISOStandardID == IsoID)
                        .Select(item => new CenterISOMeetingItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Note = item.Note,
                            CreateAt = item.CreateAt,

                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return IsoMeeting;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<CenterISOClauseItem> GetAllActive()
        {
            var dbo = ISOMeetingDA.Repository;
            var users = dbo.ISOStandards.Where(i => i.IsActive == true)
                        .Select(item => new CenterISOClauseItem()
                        {
                            ID = item.ID,
                            Code = item.Code,
                            Name = item.Name,
                            //Description = item.Description,
                            //Icon = item.Icon,
                            IsActive = item.IsActive,
                            //IsShow = item.IsShow,
                            CreateAt = item.CreateAt
                        })
                        .OrderByDescending(i => i.CreateAt).ToList();
            return users;
        }
        
    }
}
