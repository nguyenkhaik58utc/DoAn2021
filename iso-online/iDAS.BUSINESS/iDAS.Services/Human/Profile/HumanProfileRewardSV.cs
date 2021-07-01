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
    public class HumanProfileRewardSV
    {
        private HumanProfileRewardDA ProfileRewardDA = new HumanProfileRewardDA();
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanProfileRewardItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var users = ProfileRewardDA.GetQuery()
                        .Select(item => new HumanProfileRewardItem()
                        {
                            ID = item.ID,
                            DateOfDecision = item.DateOfDecision,
                            Form = item.Form,
                            NumberOfDecision = item.NumberOfDecision,
                            Reason = item.Reason,
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
        public List<HumanProfileRewardItem> GetAllByEmployeeId(int page, int pageSize, out int totalCount, int EmployeeId)
        {
            var users = ProfileRewardDA.GetQuery(i=>i.HumanEmployeeID == EmployeeId)
                        .Select(item => new HumanProfileRewardItem()
                        {
                            ID = item.ID,
                            DateOfDecision = item.DateOfDecision,
                            Form = item.Form,
                            NumberOfDecision = item.NumberOfDecision,
                            Reason = item.Reason,
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
        public HumanProfileRewardItem GetById(int Id)
        {
            var dbo = ProfileRewardDA.Repository;
            var users = ProfileRewardDA.GetQuery(i => i.ID == Id)
                        .Select(item => new HumanProfileRewardItem()
                        {
                            ID = item.ID,
                            DateOfDecision = item.DateOfDecision,
                            Form = item.Form,
                            NumberOfDecision = item.NumberOfDecision,
                            Reason = item.Reason,
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
        public void Update(HumanProfileRewardItem item, int userID)
        {
            var pr = ProfileRewardDA.GetById(item.ID);
            pr.DateOfDecision = item.DateOfDecision;
            pr.Form = item.Form;
            pr.NumberOfDecision = item.NumberOfDecision;
            pr.Reason = item.Reason;
            pr.CreateAt = item.CreateAt;
            pr.CreateBy = item.CreateBy;
            pr.HumanEmployeeID = item.EmployeeID;
            pr.UpdateAt = DateTime.Now;
            pr.UpdateBy = userID;
            ProfileRewardDA.Save();
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(HumanProfileRewardItem item, int userID)
        {
            var user = new HumanProfileReward()
            {
                DateOfDecision = item.DateOfDecision,
                Form = item.Form,
                NumberOfDecision = item.NumberOfDecision,
                Reason = item.Reason,
                UpdateAt = item.UpdateAt,
                UpdateBy = item.UpdateBy,
                HumanEmployeeID = item.EmployeeID,
                CreateAt = DateTime.Now,
                CreateBy = userID,
            };
            ProfileRewardDA.Insert(user);
            ProfileRewardDA.Save();
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            ProfileRewardDA.Delete(id);
            ProfileRewardDA.Save();
        }
    }
}
