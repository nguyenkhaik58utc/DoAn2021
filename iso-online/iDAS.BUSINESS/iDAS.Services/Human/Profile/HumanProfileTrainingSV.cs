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
    public class HumanProfileTrainingSV
    {
        private HumanProfileTrainingDA ProfileTrainingDA = new HumanProfileTrainingDA();
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanProfileTrainingItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var users = ProfileTrainingDA.GetQuery()
                        .Select(item => new HumanProfileTrainingItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Certificate = item.Certificate,
                            Content = item.Content,
                            Form = item.Form,
                            Reviews = item.Reviews,
                            Result = item.Result,
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
        public List<HumanProfileTrainingItem> GetAllByEmployeeId(int page, int pageSize, out int totalCount, int EmployeeId)
        {
            var users = ProfileTrainingDA.GetQuery(i => i.HumanEmployeeID == EmployeeId)
                        .Select(item => new HumanProfileTrainingItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Certificate = item.Certificate,
                            Content = item.Content,
                            Form = item.Form,
                            Reviews = item.Reviews,
                            Result = item.Result,
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
        public HumanProfileTrainingItem GetById(int Id)
        {
            var dbo = ProfileTrainingDA.Repository;
            var users = ProfileTrainingDA.GetQuery(i => i.ID == Id)
                        .Select(item => new HumanProfileTrainingItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Certificate = item.Certificate,
                            Content = item.Content,
                            Form = item.Form,
                            Reviews = item.Reviews,
                            Result = item.Result,
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
        public void Update(HumanProfileTrainingItem item, int userID)
        {
            var pr = ProfileTrainingDA.GetById(item.ID);
            pr.Name = item.Name;
            pr.Certificate = item.Certificate;
            pr.Result = item.Result;
            pr.Content = item.Content;
            pr.Form = item.Form;
            pr.Reviews = item.Reviews;
            pr.Result = item.Result;
            pr.StartDate = item.StartDate;
            pr.EndDate = item.EndDate;
            pr.CreateAt = item.CreateAt;
            pr.CreateBy = item.CreateBy;
            pr.HumanEmployeeID = item.EmployeeID;
            pr.UpdateAt = DateTime.Now;
            pr.UpdateBy = userID;
            ProfileTrainingDA.Save();
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(HumanProfileTrainingItem item, int userID)
        {
            
            var user = new HumanProfileTraining()
            {
                Name = item.Name,
                Certificate = item.Certificate,
                Content = item.Content,
                Form = item.Form,
                Reviews = item.Reviews,
                Result = item.Result,
                StartDate = item.StartDate,                
                EndDate = item.EndDate,
                UpdateAt = item.UpdateAt,
                UpdateBy = item.UpdateBy,
                HumanEmployeeID = item.EmployeeID,
                CreateAt = DateTime.Now,
                CreateBy = userID,
            };
            ProfileTrainingDA.Insert(user);
            ProfileTrainingDA.Save();
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            ProfileTrainingDA.Delete(id);
            ProfileTrainingDA.Save();
        }

        public HumanTrainingPractionersItem GetPractioner(int id)
        {
            return new HumanTrainingPractionersSV().GetByID(id);
        }

        public void UpdatePractioner(HumanTrainingPractionersItem objEdit, int userId)
        {
            new HumanTrainingPractionersSV().Update(objEdit, userId);
        }

        public void UpdateIsInProile(int id)
        {
            new HumanTrainingPractionersSV().UpdateIsInProile(id);
        }

        public void UpdateJoin(HumanTrainingPractionersItem objEdit, int userId)
        {
            new HumanTrainingPractionersSV().UpdateJoin(objEdit, userId);
        }
    }
}
