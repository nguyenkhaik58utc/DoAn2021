using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class FunctionItem :ModuleItem, IFunction
    {
        public string ModuleCode { get; set; }
        public string ParentCode { get; set; }
        public bool IsGroup { get; set; }
        public string Url { get; set; }
        public Type Type { get; set; }
        public string ModuleName { get; set; }
    }
}
