using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class SupplierStatisticalItem
    {
        public string Name
        {
            get;
            set;
        }
        public int CateID { get; set; }
        public int? ParentID { get; set; }
        public bool Leaf { get; set; }
        public double Data1
        {
            get;
            set;
        }
        public double Data2
        {
            get;
            set;
        }
        public double Data3
        {
            get;
            set;
        }
        public double Data4
        {
            get;
            set;
        }
        public double Data5
        {
            get;
            set;
        }
        public double Data6
        {
            get;
            set;
        }
        public double Data7
        {
            get;
            set;
        }
        public double Data8
        {
            get;
            set;
        }
        public double Data9
        {
            get;
            set;
        }
        public decimal TotalAmount
        {
            get;
            set;
        }
        public decimal TotalPayed
        {
            get;
            set;
        }
        public decimal TotalOwe
        {
            get;
            set;
        }
        public decimal TotalRecive
        {
            get;
            set;
        }
    }
}
