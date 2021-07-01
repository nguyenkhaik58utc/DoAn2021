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
    public class HumanProfileDiplomaSV
    {

        private HumanProfileDiplomaDA ProfileDiplomaDA = new HumanProfileDiplomaDA();
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanProfileDiplomaItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var users = ProfileDiplomaDA.GetQuery()
                        .Select(item => new HumanProfileDiplomaItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Condition = item.Condition,
                            DateOfGraduation = item.DateOfGraduation,
                            Faculty = item.Faculty,
                            FormOfTrainning = item.FormOfTrainning,
                            Level = item.Level,
                            Major = item.Major,
                            Place = item.Place,
                            Rank = item.Rank,
                            Type = item.Type,
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
        public List<HumanProfileDiplomaItem> GetAllByEmployeeId(int page, int pageSize, out int totalCount, int EmployeeId)
        {
            var users = ProfileDiplomaDA.GetQuery(i => i.HumanEmployeeID == EmployeeId)
                        .Select(item => new HumanProfileDiplomaItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Condition = item.Condition,
                            DateOfGraduation = item.DateOfGraduation,
                            Faculty = item.Faculty,
                            FormOfTrainning = item.FormOfTrainning,
                            Level = item.Level,
                            Major = item.Major,
                            Place = item.Place,
                            Rank = item.Rank,
                            Type = item.Type,
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
        public HumanProfileDiplomaItem GetById(int Id)
        {
            var dbo = ProfileDiplomaDA.Repository;
            var users = ProfileDiplomaDA.GetQuery(i => i.ID == Id)
                        .Select(item => new HumanProfileDiplomaItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Condition = item.Condition,
                            DateOfGraduation = item.DateOfGraduation,
                            Faculty = item.Faculty,
                            FormOfTrainning = item.FormOfTrainning,
                            Level = item.Level,
                            Major = item.Major,
                            Place = item.Place,
                            Rank = item.Rank,
                            Type = item.Type,
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
        public void Update(HumanProfileDiplomaItem item, int userID)
        {
            var pr = ProfileDiplomaDA.GetById(item.ID);
            pr.Name = item.Name;
            pr.Condition = item.Condition;
            pr.DateOfGraduation = item.DateOfGraduation;
            pr.Faculty = item.Faculty;
            pr.FormOfTrainning = item.FormOfTrainning;
            pr.Level = item.Level;
            pr.Major = item.Major;
            pr.Place = item.Place;
            pr.Rank = item.Rank;
            pr.Type = item.Type;
            pr.StartDate = item.StartDate;
            pr.EndDate = item.EndDate;
            pr.CreateAt = item.CreateAt;
            pr.CreateBy = item.CreateBy;
            pr.HumanEmployeeID = item.EmployeeID;
            pr.UpdateAt = DateTime.Now;
            pr.UpdateBy = userID;
            ProfileDiplomaDA.Save();
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(HumanProfileDiplomaItem item, int userID)
        {
            var user = new HumanProfileDiploma()
            {
                Name = item.Name,
                Condition = item.Condition,
                DateOfGraduation = item.DateOfGraduation,
                Faculty = item.Faculty,
                FormOfTrainning = item.FormOfTrainning,
                Level = item.Level,
                Major = item.Major,
                Place = item.Place,
                Rank = item.Rank,
                Type = item.Type,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                UpdateAt = item.UpdateAt,
                UpdateBy = item.UpdateBy,
                HumanEmployeeID = item.EmployeeID,
                CreateAt = DateTime.Now,
                CreateBy = userID,
            };
            ProfileDiplomaDA.Insert(user);
            ProfileDiplomaDA.Save();
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            ProfileDiplomaDA.Delete(id);
            ProfileDiplomaDA.Save();
        }
    }
}
