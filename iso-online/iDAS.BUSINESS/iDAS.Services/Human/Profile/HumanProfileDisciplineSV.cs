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
    public class HumanProfileDisciplineSV
    {
        private HumanProfileDisciplineDA ProfileDisciplineDA = new HumanProfileDisciplineDA();
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanProfileDisciplineItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var users = ProfileDisciplineDA.GetQuery()
                        .Select(item => new HumanProfileDisciplineItem()
                        {
                            ID = item.ID,
                            NumberOfDecision = item.NumberOfDecision,
                            DateOfDecision = item.DateOfDecision,
                            Reason = item.Reason,
                            Form = item.Form,
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
        public List<HumanProfileDisciplineItem> GetAllByEmployeeId(int page, int pageSize, out int totalCount, int employeeId)
        {
            var users = ProfileDisciplineDA.GetQuery(i => i.HumanEmployeeID == employeeId)
                        .Select(item => new HumanProfileDisciplineItem()
                        {
                            ID = item.ID,
                            NumberOfDecision = item.NumberOfDecision,
                            DateOfDecision = item.DateOfDecision,
                            Reason = item.Reason,
                            Form = item.Form,
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
        public HumanProfileDisciplineItem GetById(int Id)
        {
            var dbo = ProfileDisciplineDA.Repository;
            var users = ProfileDisciplineDA.GetQuery(i => i.ID == Id)
                        .Select(item => new HumanProfileDisciplineItem()
                        {
                            ID = item.ID,
                            NumberOfDecision = item.NumberOfDecision,
                            DateOfDecision = item.DateOfDecision,
                            Reason = item.Reason,
                            Form = item.Form,
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
        public void Update(HumanProfileDisciplineItem item, int userID)
        {
            var pr = ProfileDisciplineDA.GetById(item.ID);
            pr.NumberOfDecision = item.NumberOfDecision;
            pr.DateOfDecision = item.DateOfDecision;
            pr.Reason = item.Reason;
            pr.Form = item.Form;
            pr.CreateAt = item.CreateAt;
            pr.CreateBy = item.CreateBy;
            pr.HumanEmployeeID = item.EmployeeID;
            pr.UpdateAt = DateTime.Now;
            pr.UpdateBy = userID;
            ProfileDisciplineDA.Save();
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(HumanProfileDisciplineItem item, int userID)
        {
            var user = new HumanProfileDiscipline()
            {
                NumberOfDecision = item.NumberOfDecision,
                DateOfDecision = item.DateOfDecision,
                Reason = item.Reason,
                Form = item.Form,
                UpdateAt = item.UpdateAt,
                UpdateBy = item.UpdateBy,
                HumanEmployeeID = item.EmployeeID,
                CreateAt = DateTime.Now,
                CreateBy = userID,
            };
            ProfileDisciplineDA.Insert(user);
            ProfileDisciplineDA.Save();
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            ProfileDisciplineDA.Delete(id);
            ProfileDisciplineDA.Save();
        }
    }
}
