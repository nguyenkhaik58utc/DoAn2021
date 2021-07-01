using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class DispatchMethodItem
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int DepartmentID { get; set; }
        public String Department { get; set; }

        //public int PublicNumber { get; set; }
        //public DateTime PublicDate { get; set; }
        //public int? PublicBy { get; set; }
        //public string PublicName { get; set; }
        public bool IsActive { get; set; }//ND Yêu cầu

        public string Note { get; set; }//ND Yêu cầu

        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public string CreateByName { get; set; }
        public string UpdateByName { get; set; }


        public bool IsDelete { get; set; }
    }
}
