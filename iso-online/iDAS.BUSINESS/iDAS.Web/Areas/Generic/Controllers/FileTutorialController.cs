using iDAS.Core;
using iDAS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Generic.Controllers
{
    public class FileTutorialController : BaseController
    {
        private CenterTutorialSV tutorialSV = new CenterTutorialSV();
        public ActionResult ViewFile(string moduleCode,string functionCode)
        {
            try
            {
                var data = tutorialSV.ReadFile(moduleCode, functionCode);
                return new Ext.Net.MVC.PartialViewResult
                {
                    Model = data,
                };
            }
            catch
            {
                return new Ext.Net.MVC.PartialViewResult
                {
                    Model = "<html><body><div style='text-align:center; width:100%; font-weight:bold'>Chức năng không có tài liệu hướng dẫn</div><body></html>",
                };
            }
        }
    }
}
