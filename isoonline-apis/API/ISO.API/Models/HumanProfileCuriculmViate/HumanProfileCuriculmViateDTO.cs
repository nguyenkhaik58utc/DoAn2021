using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models.HumanProfileCuriculmViate
{
    public class HumanProfileCuriculmViateDTO
    {
        public int ID { get; set; }
        public int HumanEmployeeID { get; set; }
        public string Aliases { get; set; }
        public int NationalityID { get; set; }
        public string NationalityName { get; set; }
        public int EthnicID { get; set; }
        public string EthnicName { get; set; }
        public int ReligionID { get; set; }
        public string ReligionName { get; set; }
        public int CityIDOfBirth { get; set; }
        public string CityIDOfBirthName { get; set; }
        public int DistrictIDOfBirth { get; set; }
        public string DistrictIDOfBirthName { get; set; }
        public int CommuneIDOfBirth { get; set; }
        public string CommuneIDOfBirthName { get; set; }
        public string OfficePhone { get; set; }
        public string HomePhone { get; set; }
        public string NumberOfIdentityCard { get; set; }
        public Nullable<System.DateTime> DateIssueOfIdentityCard { get; set; }
        public string PlaceIssueOfIdentityCard { get; set; }
        public Nullable<System.DateTime> DateOnGroup { get; set; }
        public int YouthPositionID { get; set; }
        public string YouthPositionName { get; set; }
        public string PlaceOfLoadedGroup { get; set; }
        public Nullable<System.DateTime> DateJoinRevolution { get; set; }
        public Nullable<System.DateTime> DateAtParty { get; set; }
        public Nullable<System.DateTime> DateOfJoinParty { get; set; }
        public string PlaceOfLoadedParty { get; set; }
        public int PartyPosititonID { get; set; }
        public string PartyPosititonName { get; set; }
        public string NumberOfPartyCard { get; set; }
        public Nullable<System.DateTime> DateOnMilitary { get; set; }
        public string MilitaryPosition { get; set; }
        public int MilitaryPositionLevelID { get; set; }
        public string MilitaryPositionLevelName { get; set; }
        public string Likes { get; set; }
        public string Forte { get; set; }
        public string Defect { get; set; }
        public string TaxCode { get; set; }
        public string NumberOfBankAccounts { get; set; }
        public string Bank { get; set; }
        public string NumberOfPassport { get; set; }
        public string PlaceOfPassport { get; set; }
        public Nullable<System.DateTime> DateOfIssuePassport { get; set; }
        public Nullable<System.DateTime> PassportExpirationDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
        public int UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public int CityIDOfHomeTown {get;set;}
        public string CityNameHomeTown { get;set;}
        public int DistrictIDOfHomeTown { get;set;}
        public string DistrictNameHomeTown { get;set;}
        public int CommunelIDOfHomeTown { get;set;}
        public string CommuneNameHomeTown { get;set;}
        public string residence { get;set;}
        public string currentAddress { get;set;}
        public int PoliticalTheoryID { get;set;}
        public string PoliticalTheoryName { get;set;}
        public int GovermentManagementID { get;set;}
        public string GovermentManagementName { get;set;}
        public int HeID { get;set;}
    }
    public class HumanProfileCuriculmViateReportDTO: HumanProfileCuriculmViateDTO
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDay { get; set; }
        public string FileName { get; set; }
        public Guid? FileID { get; set; }
        public bool IsTrial { get; set; }
        public string DepartmentTitle { get; set; }
        public bool Gender { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
    }
    public class HumanPoliticInformationDTO
    {
        public int HumanEmployeeID { get; set; }
        public Nullable<System.DateTime> DateOnGroup { get; set; }
        public string YouthPositionName { get; set; }
        public string PlaceOfLoadedGroup { get; set; }

        public Nullable<System.DateTime> DateJoinRevolution { get; set; }
        public Nullable<System.DateTime> DateOfJoinParty { get; set; }
        public string PartyPositionName { get; set; }
        public string PlaceOfLoadedParty { get; set; }
        public string NumberOfPartyCard { get; set; }

        public Nullable<System.DateTime> DateOnArmy { get; set; }
        public string PositionArmy { get; set; }
        public string MilitaryPositionLevelName { get; set; }
        public string PoliticalTheoryName { get; set; }
        public string GovermentManagementName { get; set; }
    }
    public class HumanCommonInformationDTO
    {
        public int HumanEmployeeID { get; set; }
        public string HomePhone { get; set; }
        public string NumberOfIdentityCard { get; set; }
        public Nullable<DateTime> DateIssueOfIdentityCard { get; set; }
        public string PlaceIssueOfIdentityCard { get; set; }
        public string NumberOfPartyCard { get; set; }
        public string NumberOfBankAccounts { get; set; }
        public string Bank { get; set; }
        public string NumberOfPassport { get; set; }
        public string PlaceOfPassport { get; set; }
        public Nullable<DateTime> DateOfIssuePassport { get; set; }
        public Nullable<DateTime> PassportExpirationDate { get; set; }

        public string CityName { get; set; }
        public string DistrictName { get; set; }
        public string CommuneName { get; set; }
        public string ReligionName { get; set; }
        public string EthnicName { get; set; }

        public string CityNameHomeTown { get; set; }
        public string DistrictNameHomeTown { get; set; }
        public string CommuneNameHomeTown { get; set; }
        public string residence { get; set; }
        public string currentAddress { get; set; }
    }
}
