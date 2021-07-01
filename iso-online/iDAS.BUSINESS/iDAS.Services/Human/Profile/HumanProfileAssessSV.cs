using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using System.Security.Principal;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class HumanProfileAssessSV
    {
        private HumanProfileAssessDA ProfileAssessDA = new HumanProfileAssessDA();
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanProfileAssessItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var users = ProfileAssessDA.GetQuery()
                        .Select(item => new HumanProfileAssessItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Note = item.Note,
                            Result = item.Result,
                            Score = item.Score,
                            StartDate = item.StartDate,
                            EndDate = item.EndDate,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            EmployeeID = item.HumanEmployeeID
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return users;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanProfileAssessItem> GetAllByEmployeeId(int page, int pageSize, out int totalCount, int EmployeeId)
        {
            var users = ProfileAssessDA.GetQuery(m => m.HumanEmployeeID == EmployeeId)
                        .Select(item => new HumanProfileAssessItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Note = item.Note,
                            Result = item.Result,
                            Score = item.Score,
                            StartDate = item.StartDate,
                            EndDate = item.EndDate,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return users;
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public HumanProfileAssessItem GetById(int Id)
        {
            var dbo = ProfileAssessDA.Repository;
            var users = ProfileAssessDA.GetQuery(i => i.ID == Id)
                        .Select(item => new HumanProfileAssessItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Note = item.Note,
                            Result = item.Result,
                            Score = item.Score,
                            StartDate = item.StartDate,
                            EndDate = item.EndDate,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            EmployeeID = item.HumanEmployeeID
                        })
                        .First();
            return users;
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(HumanProfileAssessItem item, int userID)
        {
            var pr = ProfileAssessDA.GetById(item.ID);
            pr.Name = item.Name;
            pr.Note = item.Note;
            pr.Result = item.Result;
            pr.Score = item.Score;
            pr.StartDate = item.StartDate;
            pr.EndDate = item.EndDate;
            pr.CreateAt = item.CreateAt;
            pr.CreateBy = item.CreateBy;
            pr.HumanEmployeeID = item.EmployeeID;
            pr.UpdateAt = DateTime.Now;
            pr.UpdateBy = userID;
            ProfileAssessDA.Save();
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(HumanProfileAssessItem item, int userID)
        {
            var user = new HumanProfileAssess()
            {
                Name = item.Name,
                Note = item.Note,
                Result = item.Result,
                Score = item.Score,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                UpdateAt = item.UpdateAt,
                UpdateBy = item.UpdateBy,
                HumanEmployeeID = item.EmployeeID,
                CreateAt = DateTime.Now,
                CreateBy = userID,
            };
            ProfileAssessDA.Insert(user);
            ProfileAssessDA.Save();
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            ProfileAssessDA.Delete(id);
            ProfileAssessDA.Save();
        }
    }
}
