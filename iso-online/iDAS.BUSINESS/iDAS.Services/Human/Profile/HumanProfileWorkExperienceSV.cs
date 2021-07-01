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
    public class HumanProfileWorkExperienceSV
    {
        private HumanProfileWorkExperienceDA ProfileWorkExperienceDA = new HumanProfileWorkExperienceDA();
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanProfileWorkExperienceItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var users = ProfileWorkExperienceDA.GetQuery()
                        .Select(item => new HumanProfileWorkExperienceItem()
                        {
                            ID = item.ID,
                            Department = item.Department,
                            JobDescription = item.JobDescription,
                            PlaceOfWork = item.PlaceOfWork,
                            Position = item.Position, 
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
        public List<HumanProfileWorkExperienceItem> GetAllByEmployeeId(int page, int pageSize, out int totalCount, int EmployeeId)
        {
            var users = ProfileWorkExperienceDA.GetQuery(i => i.HumanEmployeeID == EmployeeId)
                        .Select(item => new HumanProfileWorkExperienceItem()
                        {
                            ID = item.ID,
                            Department = item.Department,
                            JobDescription = item.JobDescription,
                            PlaceOfWork = item.PlaceOfWork,
                            Position = item.Position,
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
        /// Author: Thanhnv DateTime: 12/12/2014 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public HumanProfileWorkExperienceItem GetById(int Id)
        {
            var dbo = ProfileWorkExperienceDA.Repository;
            var users = ProfileWorkExperienceDA.GetQuery(i => i.ID == Id)
                        .Select(item => new HumanProfileWorkExperienceItem()
                        {
                            ID = item.ID,
                            Department = item.Department,
                            JobDescription = item.JobDescription,
                            PlaceOfWork = item.PlaceOfWork,
                            Position = item.Position,
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
        public void Update(HumanProfileWorkExperienceItem item, int userID)
        {
            var pr = ProfileWorkExperienceDA.GetById(item.ID);
            pr.Department = item.Department;
            pr.JobDescription = item.JobDescription;
            pr.PlaceOfWork = item.PlaceOfWork;
            pr.Position = item.Position;
            pr.StartDate = item.StartDate;
            pr.EndDate = item.EndDate;
            pr.CreateAt = item.CreateAt;
            pr.CreateBy = item.CreateBy;
            pr.HumanEmployeeID = item.EmployeeID;
            pr.UpdateAt = DateTime.Now;
            pr.UpdateBy = userID;
            ProfileWorkExperienceDA.Save();
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(HumanProfileWorkExperienceItem item, int userID)
        {
            var user = new HumanProfileWorkExperience()
            {
                Department = item.Department,
                JobDescription = item.JobDescription,
                PlaceOfWork = item.PlaceOfWork,
                Position = item.Position,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                UpdateAt = item.UpdateAt,
                UpdateBy = item.UpdateBy,
                HumanEmployeeID = item.EmployeeID,
                CreateAt = DateTime.Now,
                CreateBy = userID,
            };
            ProfileWorkExperienceDA.Insert(user);
            ProfileWorkExperienceDA.Save();
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            ProfileWorkExperienceDA.Delete(id);
            ProfileWorkExperienceDA.Save();
        }
    }
}
