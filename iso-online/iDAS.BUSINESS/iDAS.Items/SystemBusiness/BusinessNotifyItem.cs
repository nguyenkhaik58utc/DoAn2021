using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class BusinessNotifyItem
    {
        private List<int> _EmployeeIDs;

        public int ID { get; set; }
        public int HumanEmployeeID { get; set; }
        public List<int> EmployeeIDs { 
            get{
                if (_EmployeeIDs == null)
                {
                    _EmployeeIDs = new List<int>();
                    _EmployeeIDs.Add(HumanEmployeeID);
                }
                return _EmployeeIDs;
            }
            set {
                _EmployeeIDs = value;
            }
        }
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public Nullable<System.DateTime> ReadTime { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public string DateSend { get; set; }
        public string TimeSend { get; set; }
        public string EmployeeSend { get; set; }
        public string FunctionName { get; set; }
        public string Param { get; set; }
        public string Icon { get; set; }
        public string FunctionTitle { get; set; }

        public string FuctionCode { get; set; }
    }
    
}
