using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class BusinessActionItem : BusinessSystemItem, IAction
    {
        public string ModuleCode { get; set; }
        public string FunctionCode { get; set; }
        public string ModuleName { get; set; }
        public string FunctionName { get; set; }
    }
}
