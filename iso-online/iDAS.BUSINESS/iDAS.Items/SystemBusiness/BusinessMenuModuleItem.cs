using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class BusinessMenuModuleItem
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public bool IsShow { get; set; }
        public bool Leaf { get; set; }
        public int IDModule { get; set; }
        public bool IsEnable { get; set; }
    }
}
