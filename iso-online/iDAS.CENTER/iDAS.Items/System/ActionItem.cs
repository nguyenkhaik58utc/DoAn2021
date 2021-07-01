using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ActionItem : BaseSystemItem,IAction
    {
        public string FunctionCode { get; set; }
        public string ModuleCode { get; set; }
        public bool IsEnable { get; set; }
    }
}
