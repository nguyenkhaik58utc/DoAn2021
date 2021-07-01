using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ProductionRequirementViewItem
    {
        public int ID { get; set; }
        public string Name
        {
            get
            {
                return "Sản xuất " + Quantity +
                                " sản phẩm; Thời hạn: " + EndTime.Value.ToString("dd/MM/yyyy")
                                + "; Mức độ:  <span style='background-color:" + Color + ";font-size: 13px;font-weight: bold;'>"
                                + Level + "  " + "</span>";
            }
        }
        public System.DateTime? EndTime { get; set; }
        public string Level { get; set; }
        public int Quantity { get; set; }
        public string Color { get; set; }
    }
}
