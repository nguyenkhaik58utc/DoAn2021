using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iDAS.Web.Models
{
    public class ModuleModel {
        public string ModuleName { get; set; }
        public List<FunctionModel> Functions { get; set; }
        public string IconName { get; set; }
    }

    public class FunctionModel {
        public string FunctionName { get; set; }
        public List<ActionModel> Actions { get; set; }
        public string IconName { get; set; }
    }

    public class ActionModel {
        public string ActionName { get; set; }
        public string Url { get; set; }
        public string IconName { get; set; }
    }

    public class SystemModel
    {
        public List<ModuleModel> Modules { get; set; }

    }
}