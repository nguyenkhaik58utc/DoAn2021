using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ext.Net;
using Ext.Net.MVC;
namespace iDAS.Web.ExtExtend
{
    public class ButtonBuilder : Button.Builder
    {
        private string _ModuleCode;
        private string _FunctionCode;
        private string _ViewFileUrl;
        internal string ViewFileUrl
        {
            set
            {
                _ViewFileUrl = value;
                callViewFile();
            }
        }
        private void callViewFile()
        {
            this.Listeners(ls => ls.Click.Handler = "var params={moduleCode:'" + _ModuleCode + "', functionCode:'" + _FunctionCode + "'};onDirectMethod('" + _ViewFileUrl + "',params);");
        }

        public ButtonBuilder() {
            _ModuleCode = HttpContext.Current.Request.RequestContext.RouteData.DataTokens["area"].ToString();
            _FunctionCode = HttpContext.Current.Request.RequestContext.RouteData.Values["Controller"].ToString();
        }
        public virtual ButtonBuilder ModuleCode(string value = "") {
            _ModuleCode = value;
            callViewFile();
            return this;
        }
        public virtual ButtonBuilder FunctionCode(string value = "")
        {
            _FunctionCode = value;
            callViewFile();
            return this;
        }
    }
    public static class ExtExtendTutorial
    {
        public static ButtonBuilder ButtonTutorial(this BuilderFactory X)
        {
            var button = new ButtonBuilder();
            button.Icon(Icon.BookOpen);
            button.Text("Hướng dẫn");
            button.ViewFileUrl = X.XController().UrlHelper.Action("ViewFile", "FileTutorial", new { Area = "Generic" });

            return button;
        }
    }
}