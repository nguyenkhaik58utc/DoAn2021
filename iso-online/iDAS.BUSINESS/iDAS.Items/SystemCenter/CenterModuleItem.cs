using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class CenterModuleItem : CenterSystemItem, IModule
    {
        public bool IsShow { get; set; }
        public int Position { get; set; }
        public string Icon { get; set; }
    }
}
