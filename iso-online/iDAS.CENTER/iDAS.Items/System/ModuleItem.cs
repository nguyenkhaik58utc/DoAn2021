using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ModuleItem : BaseSystemItem, IModule
    {
        public bool IsShow { get; set; }
        public int Position { get; set; }
        public string Icon { get; set; }
        public bool IsSelected { get; set; }
        public bool IsRequired { get; set; }
        public bool IsDeleted { get; set; }
    }
}
