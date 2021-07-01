using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class HumanTrainingPractionersItem : HumanEmployeeItem
    {
        public int DetailID { get; set; }
        public HumanEmployeeViewItem EmployeesRegister { get; set; }   
        public int EmployeesID { get; set; }
        public int Number { get; set; }
        public bool IsInProfile { get; set; }
        public string EmployeesName { get; set; }
        public string Content { get; set; }
        public string Certificate { get; set; }
        public string StatusInProfile { get; set; }
        public bool IsRegister { get; set; }
        public bool IsCommit { get; set; }
        public bool IsAcceptCommit { get; set; }
        public string ContentCommit { get; set; }
        public System.DateTime TimeRegister { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public bool IsJoin { get; set; }
        public string ResonUnJoin { get; set; }
        public Nullable<int> NumberPresence { get; set; }
        public Nullable<int> NumberAbsence { get; set; }
        public Nullable<decimal> TotalPoint { get; set; }
        public Nullable<int> RankID { get; set; }
        public string RankName { get; set; }
        public string FormName { get; set; }
        public string CommentOfTeacher { get; set; }
    }
}
