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
    public class HumanProfileCuriculmViateSV
    {
        private HumanProfileCuriculmViateDA ProfileCuriculmViateDA = new HumanProfileCuriculmViateDA();
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<HumanProfileCuriculmViateItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var users = ProfileCuriculmViateDA.GetQuery()
                        .Select(item => new HumanProfileCuriculmViateItem()
                        {
                            ID = item.ID,
                            Aliases = item.Aliases,
                            ArmyRank = item.ArmyRank,
                            Bank = item.Bank,
                            DateAtParty = item.DateAtParty,
                            DateIssueOfIdentityCard = item.DateIssueOfIdentityCard,
                            DateJoinRevolution = item.DateJoinRevolution,
                            DateOfIssuePassport = item.DateOfIssuePassport,
                            DateOfJoinParty = item.DateOfJoinParty,
                            DateOnArmy = item.DateOnArmy,
                            DateOnGroup = item.DateOnGroup,
                            Defect = item.Defect,
                            Forte = item.Forte,
                            HomePhone = item.HomePhone,
                            NumberOfIdentityCard = item.NumberOfIdentityCard,
                            Likes = item.Likes,
                            Nationality = item.Nationality,
                            NumberOfBankAccounts = item.NumberOfBankAccounts,
                            NumberOfPartyCard = item.NumberOfPartyCard,
                            NumberOfPassport = item.NumberOfPassport,
                            OfficePhone = item.OfficePhone,
                            PassportExpirationDate = item.PassportExpirationDate,
                            People = item.People,
                            PlaceIssueOfIdentityCard = item.PlaceIssueOfIdentityCard,
                            PlaceOfBirth = item.PlaceOfBirth,
                            PlaceOfLoadedGroup = item.PlaceOfLoadedGroup,
                            PlaceOfLoadedParty = item.PlaceOfLoadedParty,
                            PlaceOfPassport = item.PlaceOfPassport,
                            PositionArmy = item.PositionArmy,
                            PositionGroup = item.PositionGroup,
                            PosititonParty = item.PosititonParty,
                            Religion = item.Religion,
                            TaxCode = item.TaxCode,
                            //WeddingStatus = item.WeddingStatus,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            //EmployeeID = item.HumanEmployeeID
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return users;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public HumanProfileCuriculmViateItem GetByEmployeeId(int EmployeeId)
        {
            var user = ProfileCuriculmViateDA.GetQuery(i => i.HumanEmployeeID == EmployeeId)
                        .Select(item => new HumanProfileCuriculmViateItem()
                        {
                            ID = item.ID,
                            Aliases = item.Aliases,
                            ArmyRank = item.ArmyRank,
                            Bank = item.Bank,
                            DateAtParty = item.DateAtParty,
                            DateIssueOfIdentityCard = item.DateIssueOfIdentityCard,
                            DateJoinRevolution = item.DateJoinRevolution,
                            DateOfIssuePassport = item.DateOfIssuePassport,
                            DateOfJoinParty = item.DateOfJoinParty,
                            DateOnArmy = item.DateOnArmy,
                            DateOnGroup = item.DateOnGroup,
                            Defect = item.Defect,
                            Forte = item.Forte,
                            HomePhone = item.HomePhone,
                            NumberOfIdentityCard = item.NumberOfIdentityCard,
                            Likes = item.Likes,
                            Nationality = item.Nationality,
                            NumberOfBankAccounts = item.NumberOfBankAccounts,
                            NumberOfPartyCard = item.NumberOfPartyCard,
                            NumberOfPassport = item.NumberOfPassport,
                            OfficePhone = item.OfficePhone,
                            PassportExpirationDate = item.PassportExpirationDate,
                            People = item.People,
                            PlaceIssueOfIdentityCard = item.PlaceIssueOfIdentityCard,
                            PlaceOfBirth = item.PlaceOfBirth,
                            PlaceOfLoadedGroup = item.PlaceOfLoadedGroup,
                            PlaceOfLoadedParty = item.PlaceOfLoadedParty,
                            PlaceOfPassport = item.PlaceOfPassport,
                            PositionArmy = item.PositionArmy,
                            PositionGroup = item.PositionGroup,
                            PosititonParty = item.PosititonParty,
                            Religion = item.Religion,
                            TaxCode = item.TaxCode,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .FirstOrDefault();
            return user;
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public HumanProfileCuriculmViateItem GetById(int Id)
        {
            var dbo = ProfileCuriculmViateDA.Repository;
            var users = ProfileCuriculmViateDA.GetQuery(i => i.ID == Id)
                        .Select(item => new HumanProfileCuriculmViateItem()
                        {
                            ID = item.ID,
                            Aliases = item.Aliases,
                            ArmyRank = item.ArmyRank,
                            Bank = item.Bank,
                            DateAtParty = item.DateAtParty,
                            DateIssueOfIdentityCard = item.DateIssueOfIdentityCard,
                            DateJoinRevolution = item.DateJoinRevolution,
                            DateOfIssuePassport = item.DateOfIssuePassport,
                            DateOfJoinParty = item.DateOfJoinParty,
                            DateOnArmy = item.DateOnArmy,
                            DateOnGroup = item.DateOnGroup,
                            Defect = item.Defect,
                            Forte = item.Forte,
                            HomePhone = item.HomePhone,
                            NumberOfIdentityCard = item.NumberOfIdentityCard,
                            Likes = item.Likes,
                            Nationality = item.Nationality,
                            NumberOfBankAccounts = item.NumberOfBankAccounts,
                            NumberOfPartyCard = item.NumberOfPartyCard,
                            NumberOfPassport = item.NumberOfPassport,
                            OfficePhone = item.OfficePhone,
                            PassportExpirationDate = item.PassportExpirationDate,
                            People = item.People,
                            PlaceIssueOfIdentityCard = item.PlaceIssueOfIdentityCard,
                            PlaceOfBirth = item.PlaceOfBirth,
                            PlaceOfLoadedGroup = item.PlaceOfLoadedGroup,
                            PlaceOfLoadedParty = item.PlaceOfLoadedParty,
                            PlaceOfPassport = item.PlaceOfPassport,
                            PositionArmy = item.PositionArmy,
                            PositionGroup = item.PositionGroup,
                            PosititonParty = item.PosititonParty,
                            Religion = item.Religion,
                            TaxCode = item.TaxCode,
                            //WeddingStatus = item.WeddingStatus,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            //EmployeeID = item.HumanEmployeeID
                        })
                        .First();
            return users;
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Update(HumanProfileCuriculmViateItem item, int userID)
        {
            var pr = ProfileCuriculmViateDA.GetById(item.ID);
            pr.Aliases = item.Aliases;
            pr.ArmyRank = item.ArmyRank;
            pr.Bank = item.Bank;
            pr.DateAtParty = item.DateAtParty;
            pr.DateIssueOfIdentityCard = item.DateIssueOfIdentityCard;
            pr.DateJoinRevolution = item.DateJoinRevolution;
            pr.DateOfIssuePassport = item.DateOfIssuePassport;
            pr.DateOfJoinParty = item.DateOfJoinParty;
            pr.DateOnArmy = item.DateOnArmy;
            pr.DateOnGroup = item.DateOnGroup;
            pr.Defect = item.Defect;
            pr.Forte = item.Forte;
            pr.HomePhone = item.HomePhone;
            pr.NumberOfIdentityCard = item.NumberOfIdentityCard;
            pr.Likes = item.Likes;
            pr.Nationality = item.Nationality;
            pr.NumberOfBankAccounts = item.NumberOfBankAccounts;
            pr.NumberOfPartyCard = item.NumberOfPartyCard;
            pr.NumberOfPassport = item.NumberOfPassport;
            pr.OfficePhone = item.OfficePhone;
            pr.PassportExpirationDate = item.PassportExpirationDate;
            pr.People = item.People;
            pr.PlaceIssueOfIdentityCard = item.PlaceIssueOfIdentityCard;
            pr.PlaceOfBirth = item.PlaceOfBirth;
            pr.PlaceOfLoadedGroup = item.PlaceOfLoadedGroup;
            pr.PlaceOfLoadedParty = item.PlaceOfLoadedParty;
            pr.PlaceOfPassport = item.PlaceOfPassport;
            pr.PositionArmy = item.PositionArmy;
            pr.PositionGroup = item.PositionGroup;
            pr.PosititonParty = item.PosititonParty;
            pr.Religion = item.Religion;
            pr.TaxCode = item.TaxCode;
            //pr.WeddingStatus = item.WeddingStatus;
            pr.CreateAt = item.CreateAt;
            pr.CreateBy = item.CreateBy;
            // pr.EmployeeID = item.HumanEmployeeID;
            pr.UpdateAt = DateTime.Now;
            pr.UpdateBy = userID;
            ProfileCuriculmViateDA.Save();
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        public void Insert(HumanProfileCuriculmViateItem item, int userID)
        {
            var user = new HumanProfileCuriculmViate()
            {
                Aliases = item.Aliases,
                ArmyRank = item.ArmyRank,
                Bank = item.Bank,
                DateAtParty = item.DateAtParty,
                DateIssueOfIdentityCard = item.DateIssueOfIdentityCard,
                DateJoinRevolution = item.DateJoinRevolution,
                DateOfIssuePassport = item.DateOfIssuePassport,
                DateOfJoinParty = item.DateOfJoinParty,
                DateOnArmy = item.DateOnArmy,
                DateOnGroup = item.DateOnGroup,
                Defect = item.Defect,
                Forte = item.Forte,
                HomePhone = item.HomePhone,
                NumberOfIdentityCard = item.NumberOfIdentityCard,
                Likes = item.Likes,
                Nationality = item.Nationality,
                NumberOfBankAccounts = item.NumberOfBankAccounts,
                NumberOfPartyCard = item.NumberOfPartyCard,
                NumberOfPassport = item.NumberOfPassport,
                OfficePhone = item.OfficePhone,
                PassportExpirationDate = item.PassportExpirationDate,
                People = item.People,
                PlaceIssueOfIdentityCard = item.PlaceIssueOfIdentityCard,
                PlaceOfBirth = item.PlaceOfBirth,
                PlaceOfLoadedGroup = item.PlaceOfLoadedGroup,
                PlaceOfLoadedParty = item.PlaceOfLoadedParty,
                PlaceOfPassport = item.PlaceOfPassport,
                PositionArmy = item.PositionArmy,
                PositionGroup = item.PositionGroup,
                PosititonParty = item.PosititonParty,
                Religion = item.Religion,
                TaxCode = item.TaxCode,
                //WeddingStatus = item.WeddingStatus,
                UpdateAt = item.UpdateAt,
                UpdateBy = item.UpdateBy,
                HumanEmployeeID = item.EmployeeID,
                CreateAt = DateTime.Now,
                CreateBy = userID,
            };
            ProfileCuriculmViateDA.Insert(user);
            ProfileCuriculmViateDA.Save();
        }
        /// <summary>
        /// Author: Thanhnv DateTime: 12/12/2014
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            ProfileCuriculmViateDA.Delete(id);
            ProfileCuriculmViateDA.Save();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmployeeID"></param>
        //public void DeleteByEmployeeID(int EmployeeID)
        //{
        //    var pr = ProfileCuriculmViateDA.GetQuery(i => i.EmployeeID == EmployeeID).First();
        //    ProfileCuriculmViateDA.Delete(pr.ID);
        //    ProfileCuriculmViateDA.Save();
        //}
    }
}
