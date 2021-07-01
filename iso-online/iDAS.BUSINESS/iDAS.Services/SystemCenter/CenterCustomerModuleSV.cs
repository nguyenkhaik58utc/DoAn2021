using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.DA;
using iDAS.Base;
using iDAS.Core;
namespace iDAS.Services
{
    public class CenterCustomerModuleSV
    {
        CenterCustomerModuleDA customerModuleDA = new CenterCustomerModuleDA();

        public IEnumerable<CenterModule> getModules(int customerID) 
        {
            var systemCode = BaseSystem.SystemCode;
            var modules = customerModuleDA.GetQuery()
                            .Where(i => i.CustomerSystemID == customerID)
                            .Where(i => i.CenterModule.SystemCode == systemCode)
                            .Select(i => i.CenterModule);
            return modules;
        }

        public IEnumerable<CenterFunction> getFunctions(IEnumerable<CenterModule> modules) 
        { 
            var dbo = customerModuleDA.Repository;
            var functions = dbo.CenterFunctions
                            .Join(modules, i => i.SystemCode + i.ModuleCode, o => o.SystemCode + o.Code, (i, o) => i);
            return functions;
        }

        public IEnumerable<CenterAction> getActions(IEnumerable<CenterFunction> functions)
        {
            var dbo = customerModuleDA.Repository;
            var actions = dbo.CenterActions
                            .Join(functions, i => i.SystemCode + i.ModuleCode + i.FunctionCode, o => o.SystemCode + o.ModuleCode + o.Code, (i, o) => i);
            return actions;
        }
    }
}
