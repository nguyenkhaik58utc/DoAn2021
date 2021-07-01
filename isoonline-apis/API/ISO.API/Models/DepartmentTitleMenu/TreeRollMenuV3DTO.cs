using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace ISO.API.Models
{
    public class TreeRollMenuV3DTO
    {
        public int ID { get; set; }
        public int? ParentID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int RowLevel { get; set; }
        public bool IsPermission { get; set; }
        public string DisplayNameInforOnGrid
        {
            get
            {
                var display = new StringBuilder();
                int count = RowLevel * 8;
                while (count > 0)
                {
                    display.Append("&nbsp;");
                    count--;
                }
                if (RowLevel == 0)
                    display.Append("<b style=\"color:blue;\"><i class=\"fa fa-caret-right\"></i> " + Name + "</b>");
                else
                    display.Append(Name);
                return display.ToString();
            }
        }
    }
}
