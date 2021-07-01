using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class HumanRecruitmentProfileItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? Birthday { get; set; }
        public string Address { get; set; }
        public bool Gender { get; set; }
        public bool? WeddingStatus { get; set; }
        public Nullable<decimal> Salary { get; set; }
        public string Nationality { get; set; }
        public string People { get; set; }
        public string Religion { get; set; }
        public string PlaceOfBirth { get; set; }
        public string NumberOfIdentityCard { get; set; }
        public DateTime? DateIssueOfIdentityCard { get; set; }
        public string PlaceIssueOfIdentityCard { get; set; }
        public string HomePhone { get; set; }
        public string PlaceOfTranning { get; set; }
        public string Specicalization { get; set; }
        public string LevelOfComputerization { get; set; }
        public string ForeignLanguage { get; set; }
        public string Literacy { get; set; }
        public string Qualifications { get; set; }
        public string ListOfCertificates { get; set; }
        public string Experience { get; set; }
        public short? YearsOfExperience { get; set; }
        public bool IsDelete { get; set; }
        public bool IsEmployee { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public int EmployeeID { get; set; }
    }
}