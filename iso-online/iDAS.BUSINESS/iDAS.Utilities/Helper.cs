using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Utilities
{
    public static class Helper
    {
        public static IHtmlString Required(this HtmlHelper html) 
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<span style='color:red'> (*)</span>");
            return MvcHtmlString.Create(sb.ToString());
        }
    }
}
