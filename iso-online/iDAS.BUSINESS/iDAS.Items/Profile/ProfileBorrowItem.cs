using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ProfileBorrowItem
    {
        public int ID { get; set; }
        public string Code { get; set; }//Ma HS
        public string Name { get; set; }//Ten HS
        public int ProfileID { get; set; }
        public string Security { get; set; }//Chuc vu
        public string Color { get; set; }//Chuc vu
        public string Receiver { get; set; }//Chuc vu

        public int CategoryID { get; set; }
        public int DepartmentID { get; set; }

        public string Category { get; set; }//DMuc So Muon HS

        //
        public int BorrowBy { get; set; }//Người mượn
        public string Borrower { get; set; }//người mượn
        public DateTime BorrowAt { get; set; }//TG tiep mượn
        public string Position { get; set; }//Chuc vu

        public DateTime LimitAt { get; set; }//Ngay hen tra
        public DateTime? ReturnAt { get; set; }//Ngay tra thuc te
      
        public bool IsReturn { get; set; }//Đã trả HS

      
        public string Note { get; set; }//Ghi chú 

        public DateTime? CreateAt { get; set; }
        public int? CreateBy { get; set; }//Người mượn
        public string CreateName { get; set; }
        public int? UpdateBy { get; set; }//Người mượn
        public string UpdateName { get; set; }
        public DateTime? UpdateAt { get; set; }//Người mượn


        public int ReceiveID { get; set; }
      

        public HumanEmployeeViewItem EmployeeInfo { get; set; }
    }
}
