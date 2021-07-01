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
    public class HumanProfileCertificateSV
    {
        private HumanProfileCertificateDA ProfileCertificateDA = new HumanProfileCertificateDA();
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanProfileCertificateItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var users = ProfileCertificateDA.GetQuery()
                        .Select(item => new HumanProfileCertificateItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            DateExpiration = item.DateExpiration,
                            DateIssuance = item.DateIssuance,
                            Level = item.Level,
                            Type = item.Type,
                             PlaceOfTraining = item.PlaceOfTraining,
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
        public List<HumanProfileCertificateItem> GetAllByEmployeeID(int page, int pageSize, out int totalCount, int EmployeeId)
        {
            var users = ProfileCertificateDA.GetQuery(i=>i.HumanEmployeeID == EmployeeId)
                        .Select(item => new HumanProfileCertificateItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            DateExpiration = item.DateExpiration,
                            DateIssuance = item.DateIssuance,
                            Level = item.Level,
                            Type = item.Type,
                            PlaceOfTraining = item.PlaceOfTraining,
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
        public HumanProfileCertificateItem GetById(int Id)
        {
            var dbo = ProfileCertificateDA.Repository;
            var users = ProfileCertificateDA.GetQuery(i => i.ID == Id)
                        .Select(item => new HumanProfileCertificateItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            DateExpiration = item.DateExpiration,
                            DateIssuance = item.DateIssuance,
                            Level = item.Level,
                            Type = item.Type,
                            PlaceOfTraining = item.PlaceOfTraining,
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
        public void Update(HumanProfileCertificateItem item, int userID)
        {
            var pr = ProfileCertificateDA.GetById(item.ID);
            pr.Name = item.Name;
            pr.DateExpiration = item.DateExpiration;
            pr.DateIssuance = item.DateIssuance;
            pr.Level = item.Level;
            pr.Type = item.Type;
            pr.PlaceOfTraining = item.PlaceOfTraining;
            pr.CreateAt = item.CreateAt;
            pr.CreateBy = item.CreateBy;
            pr.HumanEmployeeID = item.EmployeeID;
            pr.UpdateAt = DateTime.Now;
            pr.UpdateBy = userID;
            ProfileCertificateDA.Save();
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(HumanProfileCertificateItem item, int userID)
        {
            var user = new HumanProfileCertificate()
            {
                Name = item.Name,
                DateExpiration = item.DateExpiration,
                DateIssuance = item.DateIssuance,
                Level = item.Level,
                Type = item.Type,
                PlaceOfTraining = item.PlaceOfTraining,
                UpdateAt = item.UpdateAt,
                UpdateBy = item.UpdateBy,
                HumanEmployeeID = item.EmployeeID,
                CreateAt = DateTime.Now,
                CreateBy = userID,
            };
            ProfileCertificateDA.Insert(user);
            ProfileCertificateDA.Save();
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            ProfileCertificateDA.Delete(id);
            ProfileCertificateDA.Save();
        }
    }
}
