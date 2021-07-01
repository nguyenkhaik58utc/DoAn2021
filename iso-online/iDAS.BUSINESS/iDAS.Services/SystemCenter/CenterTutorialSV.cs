using iDAS.Core;
using iDAS.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
    public class CenterTutorialSV
    {
        private CenterTutorialDA tutorialDA = new CenterTutorialDA();
        public string ReadFile(string moduleCode, string functionCode)
        {
            var systemCode = BaseSystem.SystemCode;
            var file = tutorialDA.GetQuery()
                        .Where(i => i.SystemCode == systemCode)
                        .Where(i => i.ModuleCode == moduleCode)
                        .Where(i => i.FunctionCode == functionCode)
                        .Where(i => i.IsActive)
                        .Where(i => !i.IsDelete)
                        .Select(i => i.File)
                        .FirstOrDefault();
            var html = new FileSV().ReadFile(file.Data, file.Extension);
            return html;
        }
        //public byte[] ReadFile(string moduleCode, string functionCode)
        //{
        //    var systemCode = BaseSystem.SystemCode;
        //    var data = tutorialDA.GetQuery()
        //                .Where(i => i.SystemCode == systemCode)
        //                .Where(i => i.ModuleCode == moduleCode)
        //                .Where(i => i.FunctionCode == functionCode)
        //                .Where(i => i.IsUse)
        //                .Where(i => !i.IsDelete)
        //                .Select(i => i.File.Data)
        //                .FirstOrDefault();
        //    return data;
        //}
    }
}
