﻿using iDAS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class CustomerRegisterDatasizeItem
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public Guid? CustomerFileId { get; set; }
        public int CenterSystemID { get; set; }
        public int DataSize { get; set; }
        public string DataSizeName { get { return DataSize + " GB"; } }
        public bool IsNew { get; set; }
        public bool IsContact { get; set; }
        public bool IsAccept { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> RequireAt { get; set; }
        public Nullable<System.DateTime> ContactAt { get; set; }
        public Nullable<System.DateTime> CompleteAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
    }
}
