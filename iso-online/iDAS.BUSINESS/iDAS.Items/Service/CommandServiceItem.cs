﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
  public class ServiceCommandServiceItem
    {
        public int ID { get; set; }
        public int ServiceID { get; set; }
        public string Name { get; set; }
        public string ContractName { get; set; }
        public string CustomerName { get; set; }
        public int CommandID { get; set; }
        public Nullable<System.DateTime> Time { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}
