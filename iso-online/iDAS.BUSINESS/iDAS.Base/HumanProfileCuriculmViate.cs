//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace iDAS.Base
{
    using System;
    using System.Collections.Generic;
    
    public partial class HumanProfileCuriculmViate
    {
        public int ID { get; set; }
        public int HumanEmployeeID { get; set; }
        public string Aliases { get; set; }
        public string Nationality { get; set; }
        public string People { get; set; }
        public string Religion { get; set; }
        public string PlaceOfBirth { get; set; }
        public string OfficePhone { get; set; }
        public string HomePhone { get; set; }
        public string NumberOfIdentityCard { get; set; }
        public Nullable<System.DateTime> DateIssueOfIdentityCard { get; set; }
        public string PlaceIssueOfIdentityCard { get; set; }
        public Nullable<System.DateTime> DateOnGroup { get; set; }
        public string PositionGroup { get; set; }
        public string PlaceOfLoadedGroup { get; set; }
        public Nullable<System.DateTime> DateJoinRevolution { get; set; }
        public Nullable<System.DateTime> DateAtParty { get; set; }
        public Nullable<System.DateTime> DateOfJoinParty { get; set; }
        public string PlaceOfLoadedParty { get; set; }
        public string PosititonParty { get; set; }
        public string NumberOfPartyCard { get; set; }
        public Nullable<System.DateTime> DateOnArmy { get; set; }
        public string PositionArmy { get; set; }
        public string ArmyRank { get; set; }
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
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
    
        public virtual HumanEmployee HumanEmployee { get; set; }
    }
}
