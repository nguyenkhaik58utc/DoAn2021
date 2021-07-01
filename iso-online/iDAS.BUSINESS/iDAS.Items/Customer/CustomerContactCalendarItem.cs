using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class CustomerContactCalendarItem
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerContactFormName { get; set; }
        public int CustomerContactFormID { get; set; }
        public bool IsNew { get; set; }
        public bool IsPerformBackContact
        {
            get { return !IsNew; }
            set { IsNew = !value; }
        }
        public Nullable<System.DateTime> Time { get; set; }
        public Nullable<int> Content { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public HumanEmployeeViewItem AppointmentEmployee { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public bool IsReadOnly { get; set; }
    }
    public class CustomerContactCalendarViewItem : EventModel
    {
        public int ID { get; set; }
        public int DepartmentID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int EventID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}