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
    public class HumanProfileRelationshipSV
    {
        private HumanProfileRelationshipDA ProfileRelationshipDA = new HumanProfileRelationshipDA();
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanProfileRelationshipItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var users =  ProfileRelationshipDA.GetQuery()
                        .Select(item => new HumanProfileRelationshipItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Age = item.Age,
                            IsMale = item.IsMale,
                            Job = item.Job,
                            PlaceOfJob = item.PlaceOfJob,
                            Relationship = item.Relationship,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            EmployeeID = item.HumanEmployeeID,
                            Phone = item.Phone,
                            Adress = item.Adress
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
        public List<HumanProfileRelationshipItem> GetAllByEmployeeId(int page, int pageSize, out int totalCount, int EmployeeId)
        {
            var users = ProfileRelationshipDA.GetQuery(i => i.HumanEmployeeID == EmployeeId)
                        .Select(item => new HumanProfileRelationshipItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Age = item.Age,
                            IsMale = item.IsMale,
                            Job = item.Job,
                            PlaceOfJob = item.PlaceOfJob,
                            Relationship = item.Relationship,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            EmployeeID = item.HumanEmployeeID,
                            Phone = item.Phone,
                            Adress = item.Adress
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
        public HumanProfileRelationshipItem GetById(int Id)
        {
            var dbo =  ProfileRelationshipDA.Repository;
            var users =  ProfileRelationshipDA.GetQuery(i => i.ID == Id)
                        .Select(item => new HumanProfileRelationshipItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Age = item.Age,
                            IsMale = item.IsMale,
                            sex = item.IsMale == true ? "Nam" : "Nữ",
                            Job = item.Job,
                            PlaceOfJob = item.PlaceOfJob,
                            Relationship = item.Relationship,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            EmployeeID = item.HumanEmployeeID,
                            Phone = item.Phone,
                            Adress = item.Adress
                        })
                        .First();
            return users;
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(HumanProfileRelationshipItem item, int userID)
        {
            var pr =  ProfileRelationshipDA.GetById(item.ID);
            pr.Name = item.Name;
            pr.Age = item.Age;
            pr.IsMale = item.IsMale;
            pr.Job = item.Job;
            pr.PlaceOfJob = item.PlaceOfJob;
            pr.Relationship = item.Relationship;
            pr.Phone = item.Phone;
            pr.Adress = item.Adress;
            pr.CreateAt = item.CreateAt;
            pr.CreateBy = item.CreateBy;
            pr.HumanEmployeeID = item.EmployeeID;
            pr.UpdateAt = DateTime.Now;
            pr.UpdateBy = userID;
             ProfileRelationshipDA.Save();
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(HumanProfileRelationshipItem item, int userID)
        {
            var user = new HumanProfileRelationship()
            {
                Name = item.Name,
                Age = item.Age,
                IsMale = item.IsMale,
                Job = item.Job,
                PlaceOfJob = item.PlaceOfJob,
                Relationship = item.Relationship,
                UpdateAt = item.UpdateAt,
                UpdateBy = item.UpdateBy,
                HumanEmployeeID = item.EmployeeID,
                CreateAt = DateTime.Now,
                CreateBy = userID,
                Phone = item.Phone,
                Adress = item.Adress
            };
             ProfileRelationshipDA.Insert(user);
             ProfileRelationshipDA.Save();
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
             ProfileRelationshipDA.Delete(id);
             ProfileRelationshipDA.Save();
        }
    }
}
