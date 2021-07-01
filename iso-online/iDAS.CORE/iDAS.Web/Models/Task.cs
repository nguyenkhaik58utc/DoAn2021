using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ext.Net.MVC;

namespace iDAS.Web.Models
{
    //[Model(Name = "Task")]
    public class Task
    {
        public int ID { get; set; }
        public int CampaignID { get; set; }
        public string Name { get; set; }
        public bool StartTime { get; set; }
        public bool EndTime { get; set; }
        public Nullable<System.DateTime> Expense { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}