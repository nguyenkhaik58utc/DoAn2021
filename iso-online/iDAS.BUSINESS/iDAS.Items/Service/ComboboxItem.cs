using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ComboboxItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string FullName { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        public int PointFrom { get; set; }
        public int PointTo { get; set; }

    }
    public class UnitStockItem
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
    }
}
