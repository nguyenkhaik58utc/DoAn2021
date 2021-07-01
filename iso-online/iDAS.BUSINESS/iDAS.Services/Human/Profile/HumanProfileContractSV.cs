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
    public class HumanProfileContractSV
    {
        private HumanProfileContractDA ProfileContractDA = new HumanProfileContractDA();
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanProfileContractItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var users = ProfileContractDA.GetQuery()
                        .Select(item => new HumanProfileContractItem()
                        {
                            ID = item.ID,
                            Condition = item.Condition,
                            NumberOfContracts = item.NumberOfContracts,
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
        public List<HumanProfileContractItem> GetAllByEmployeeId(int page, int pageSize, out int totalCount, int EmployeeId)
        {
            var users = ProfileContractDA.GetQuery(i => i.HumanEmployeeID == EmployeeId)
                        .Select(item => new HumanProfileContractItem()
                        {
                            ID = item.ID,
                            Condition = item.Condition,
                            NumberOfContracts = item.NumberOfContracts,
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
        public HumanProfileContractItem GetById(int Id)
        {
            var dbo = ProfileContractDA.Repository;
            var users = ProfileContractDA.GetQuery(i => i.ID == Id)
                        .Select(item => new HumanProfileContractItem()
                        {
                            ID = item.ID,
                            Condition = item.Condition,
                            NumberOfContracts = item.NumberOfContracts,
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
        public void Update(HumanProfileContractItem item, int userID)
        {
            var pr = ProfileContractDA.GetById(item.ID);
            pr.Condition = item.Condition;
            pr.NumberOfContracts = item.NumberOfContracts;
            pr.Type = item.Type;
            pr.StartDate = item.StartDate;
            pr.EndDate = item.EndDate;
            pr.CreateAt = item.CreateAt;
            pr.CreateBy = item.CreateBy;
            pr.HumanEmployeeID = item.EmployeeID;
            pr.UpdateAt = DateTime.Now;
            pr.UpdateBy = userID;
            ProfileContractDA.Save();
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(HumanProfileContractItem item, int userID)
        {
            var user = new HumanProfileContract()
            {
                Condition = item.Condition,
                NumberOfContracts = item.NumberOfContracts,
                Type = item.Type,
                StartDate = item.StartDate,
                EndDate = item.EndDate,
                UpdateAt = item.UpdateAt,
                UpdateBy = item.UpdateBy,
                HumanEmployeeID = item.EmployeeID,
                CreateAt = DateTime.Now,
                CreateBy = userID,
            };
            ProfileContractDA.Insert(user);
            ProfileContractDA.Save();
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            ProfileContractDA.Delete(id);
            ProfileContractDA.Save();
        }
    }
}
