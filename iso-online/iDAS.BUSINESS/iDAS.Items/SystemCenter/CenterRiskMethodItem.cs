﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace iDAS.Items
{
    public class CenterRiskMethodItem
    {
        public int ID { get; set; }
        public string Level { get; set; }
        public string Color { get; set; }
        public string Method { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
    }
}
