using Ext.Net;
using iDAS.Core;
using iDAS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.ExtExtend
{
    public class ButtonExWordBuilder : Button.Builder
    {
        private string _ModuleCode;
        private string _FunctionCode;
        private string _ExportFileUrl;
        private string _FormID;
        private bool hasTemp = false;       
 
        internal string ExportFileUrl
        {
            set
            {
                _ExportFileUrl = value;
                callExportFile();
            }
        }
        private void callExportFile()
        {
            if(hasTemp)
            {
                this.Listeners(ls => ls.Click.Handler = @"
                            var arrData =new Array();
                            for (var i = 0; i <  App." + _FormID + @".getForm().getFields().items.length; i++) {
                                                var _item = App." + _FormID + @".getForm().getFields().items[i];
                                                arrData.push({key: _item.name,value: _item.value});
                              }
                            var params={functionCode: '" + _FunctionCode + "',moduleCode: '" + _ModuleCode + @"',arrData:JSON.stringify(arrData)};
                            Ext.net.DirectMethod.request({url:'" + _ExportFileUrl + "',isUpload: true,params:params});");//,isUpload: true
            }else
            {
                this.Listeners(ls => ls.Click.Handler = @"
                            var arrData =new Array();
                            for (var i = 0; i <  App." + _FormID + @".getForm().getFields().items.length; i++) {
                                                var _item = App." + _FormID + @".getForm().getFields().items[i];
                                                arrData.push({key: _item.name,value: _item.value});
                              }
                            var params={functionCode: '" + _FunctionCode + "',moduleCode: '" + _ModuleCode + @"',arrData:JSON.stringify(arrData)};
                            Ext.net.DirectMethod.request({url:'" + _ExportFileUrl + "',params:params});");
            }
            
        }
        public ButtonExWordBuilder()
        {
            _ModuleCode = HttpContext.Current.Request.RequestContext.RouteData.DataTokens["area"].ToString();
            _FunctionCode = HttpContext.Current.Request.RequestContext.RouteData.Values["Controller"].ToString();
            TemplateExportSV tempExportSV = new TemplateExportSV();
            var tempExp = tempExportSV.GetFileByFunctionCode(_FunctionCode, 2);
            if (tempExp != null) hasTemp = true;
        }
        public virtual ButtonExWordBuilder ModuleCode(string value = "")
        {
            _ModuleCode = value;
            callExportFile();
            return this;
        }
        public virtual ButtonExWordBuilder FunctionCode(string value = "")
        {
            _FunctionCode = value;
            callExportFile();
            return this;
        }
        public virtual ButtonExWordBuilder FormID(string value = "")
        {
            _FormID = value;
            callExportFile();
            return this;
        }
    }
    public static class ExtExtendExportWord
    {
        public static ButtonExWordBuilder ButtonExportWordFile(this BuilderFactory X)
        {
            var button = new ButtonExWordBuilder();
            button.Icon(Icon.PackageDown);
            button.Text("Xuất Word");
            button.ExportFileUrl = X.XController().UrlHelper.Action("ExportWordFile", "ExportWordFile", new { Area = "Generic" });
            return button;
        }
    }
}