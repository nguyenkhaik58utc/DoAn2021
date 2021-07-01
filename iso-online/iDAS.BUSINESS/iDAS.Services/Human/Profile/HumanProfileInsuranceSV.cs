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
    public class HumanProfileInsuranceSV
    {
        private HumanProfileInsuranceDA ProfileInsuranceDA = new HumanProfileInsuranceDA();
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanProfileInsuranceItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var users = ProfileInsuranceDA.GetQuery()
                        .Select(item => new HumanProfileInsuranceItem()
                        {
                            ID = item.ID,
                            Number = item.Number,
                            Type = item.Type,
                            Note = item.Note,
                            StartDate = item.StartDate,
                            EndDate = item.EndDate,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            EmployeeID = item.HumanEmployeeID,
                            InSuranceNorms = item.InSuranceNorms
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
        public List<HumanProfileInsuranceItem> GetAllByEmployeeId(int page, int pageSize, out int totalCount, int EmployeeId)
        {
            var users = ProfileInsuranceDA.GetQuery(i => i.HumanEmployeeID == EmployeeId)
                        .Select(item => new HumanProfileInsuranceItem()
                        {
                            ID = item.ID,
                            Number = item.Number,
                            Type = item.Type,
                            Note = item.Note,
                            StartDate = item.StartDate,
                            EndDate = item.EndDate,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            EmployeeID = item.HumanEmployeeID,
                            InSuranceNorms = item.InSuranceNorms
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
        public HumanProfileInsuranceItem GetById(int Id)
        {
            var dbo = ProfileInsuranceDA.Repository;
            var users = ProfileInsuranceDA.GetQuery(i => i.ID == Id)
                        .Select(item => new HumanProfileInsuranceItem()
                        {
                            ID = item.ID,
                            Number = item.Number,
                            Type = item.Type,
                            Note = item.Note,
                            StartDate = item.StartDate,
                            EndDate = item.EndDate,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            EmployeeID = item.HumanEmployeeID,
                            InSuranceNorms = item.InSuranceNorms
                        })
                        .First();
            return users;
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(HumanProfileInsuranceItem item, int userID)
        {
            var pr = ProfileInsuranceDA.GetById(item.ID);
            pr.Number = item.Number;
            pr.Type = item.Type;
            pr.Note = item.Note;
            pr.StartDate = item.StartDate;
            pr.EndDate = item.EndDate;
            pr.CreateAt = item.CreateAt;
            pr.CreateBy = item.CreateBy;
            pr.InSuranceNorms = item.InSuranceNorms;
            pr.HumanEmployeeID = item.EmployeeID;
            pr.UpdateAt = DateTime.Now;
            pr.UpdateBy = userID;
            ProfileInsuranceDA.Save();
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(HumanProfileInsuranceItem item, int userID)
        {
            var user = new HumanProfileInsurance()
            {
                Number = item.Number,
                Type = item.Type,
                Note = item.Note,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                UpdateAt = item.UpdateAt,
                UpdateBy = item.UpdateBy,
                HumanEmployeeID = item.EmployeeID,
                CreateAt = DateTime.Now,
                CreateBy = userID,
                InSuranceNorms = item.InSuranceNorms
            };
            ProfileInsuranceDA.Insert(user);
            ProfileInsuranceDA.Save();
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            ProfileInsuranceDA.Delete(id);
            ProfileInsuranceDA.Save();
        }
    }
}
