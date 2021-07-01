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

    public class HumanProfileSalarySV
    {
        private HumanProfileSalaryDA ProfileSalaryDA = new HumanProfileSalaryDA();
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanProfileSalaryItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var users = ProfileSalaryDA.GetQuery()
                        .Select(item => new HumanProfileSalaryItem()
                        {
                            ID = item.ID,
                            Level = item.Level,
                            Wage = item.Wage,
                            DateOfApp= item.DateOfApp,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            EmployeeID = item.HumanEmployeeID,
                            Note =item.Note
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
        public List<HumanProfileSalaryItem> GetAllByEmployeeId(int page, int pageSize, out int totalCount, int EmployeeId)
        {
            var users = ProfileSalaryDA.GetQuery(i => i.HumanEmployeeID == EmployeeId)
                        .Select(item => new HumanProfileSalaryItem()
                        {
                            ID = item.ID,
                            Level = item.Level,
                            Wage = item.Wage,
                            DateOfApp = item.DateOfApp,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            EmployeeID = item.HumanEmployeeID,
                            Note = item.Note
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
        public HumanProfileSalaryItem GetById(int Id)
        {
            var dbo = ProfileSalaryDA.Repository;
            var users = ProfileSalaryDA.GetQuery(i => i.ID == Id)
                        .Select(item => new HumanProfileSalaryItem()
                        {
                            ID = item.ID,
                            Level = item.Level,
                            Wage = item.Wage,
                            DateOfApp = item.DateOfApp,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            EmployeeID = item.HumanEmployeeID,
                            Note = item.Note
                        })
                        .First();
            return users;
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(HumanProfileSalaryItem item, int userID)
        {
            var pr = ProfileSalaryDA.GetById(item.ID);
            pr.Level = item.Level;
            pr.Wage = item.Wage;
            pr.Note = item.Note;
            pr.DateOfApp= item.DateOfApp;
            pr.CreateAt = item.CreateAt;
            pr.CreateBy = item.CreateBy;
            pr.HumanEmployeeID = item.EmployeeID;
            pr.UpdateAt = DateTime.Now;
            pr.UpdateBy = userID;
            ProfileSalaryDA.Save();
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(HumanProfileSalaryItem item, int userID)
        {
            var user = new HumanProfileSalary()
            {
                Level = item.Level,
                Wage = item.Wage,
                DateOfApp = item.DateOfApp,
                UpdateAt = item.UpdateAt,
                UpdateBy = item.UpdateBy,
                HumanEmployeeID = item.EmployeeID,
                CreateAt = DateTime.Now,
                CreateBy = userID,
                Note =item.Note
            };
            ProfileSalaryDA.Insert(user);
            ProfileSalaryDA.Save();
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            ProfileSalaryDA.Delete(id);
            ProfileSalaryDA.Save();
        }
    }
}
