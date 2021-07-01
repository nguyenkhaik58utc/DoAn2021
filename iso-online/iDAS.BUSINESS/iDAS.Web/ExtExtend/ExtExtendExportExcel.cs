using Ext.Net;
using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.ExtExtend
{
    public class ButtonExBuilder : Button.Builder
    {
        private string _ModuleCode;
        private string _FunctionCode;
        private string _ViewFileUrl;       
        private string _GridID;
        private string _Title;
        private string _ExportFileUrl;
        private bool _isTree;
        internal string OpenWindowUrl
        {
            set
            {
                _ViewFileUrl = value;
                callViewFile();
            }
        }
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
            if(_isTree)
            this.Listeners(ls => ls.Click.Handler = @"
                var arr = new Array();
                for (var i = 0; i < App." + _GridID + @".columns.length; i++) {
                if (App." + _GridID + @".columns[i].dataIndex != 'ID')
                    arr.push({ dataIndex: App." + _GridID + @".columns[i].dataIndex,text:App." + _GridID + @".columns[i].text,hidden:App." + _GridID + @".columns[i].hidden,position:i+1});
                  }
                var arrData =new Array();
                for (var i = 0; i <  App." + _GridID + @".selModel.getStore().data.items.length; i++) {
                                    arrData.push( App." + _GridID + @".selModel.getStore().data.items[i].data);
                  }
                    var params={title:'" + _Title + @"',functionCode: '" + _FunctionCode + @"', arrObject:JSON.stringify(arr),arrData:JSON.stringify(arrData)};
                            Ext.net.DirectMethod.request({url:'" + _ExportFileUrl + "',isUpload: true,params:params});");
            else
                this.Listeners(ls => ls.Click.Handler = @"
                var arr = new Array();
                for (var i = 0; i < App." + _GridID + @".columns.length; i++) {
                if (App." + _GridID + @".columns[i].dataIndex != 'ID')
                    arr.push({ dataIndex: App." + _GridID + @".columns[i].dataIndex,text:App." + _GridID + @".columns[i].text,hidden:App." + _GridID + @".columns[i].hidden,position:i+1});
                  }
                var arrData =new Array();
                for (var i = 0; i <  App." + _GridID + @".getStore().data.items.length; i++) {
                                    arrData.push( App." + _GridID + @".getStore().data.items[i].data);
                  }
                    var params={title:'" + _Title + @"',functionCode: '" + _FunctionCode + @"', arrObject:JSON.stringify(arr),arrData:JSON.stringify(arrData)};
                            Ext.net.DirectMethod.request({url:'" + _ExportFileUrl + "',isUpload: true,params:params});");
        }
        private void callViewFile()
        {
            if(_isTree)               
                this.Listeners(ls => ls.Click.Handler = @"
                    var arr = new Array();
                    for (var i = 0; i < App."+_GridID + @".columns.length; i++) {
                    if (App."+_GridID + @".columns[i].dataIndex != 'ID')
                        arr.push({ dataIndex: App."+_GridID + @".columns[i].dataIndex,text:App."+_GridID + @".columns[i].text,hidden:App."+_GridID + @".columns[i].hidden,position:i+1});
                      }
                    var arrData =new Array();
                    for (var i = 0; i <  App." + _GridID + @".selModel.getStore().data.items.length; i++) {
                                        arrData.push( App." + _GridID + @".selModel.getStore().data.items[i].data);
                      }
                        var params={currentPage: 1, pageSize:10, arrObject:JSON.stringify(arr),arrData:JSON.stringify(arrData)};onDirectMethod('" + _ViewFileUrl + "',params);");
            else
                this.Listeners(ls => ls.Click.Handler = @"
                    var arr = new Array();
                    for (var i = 0; i < App." + _GridID + @".columns.length; i++) {
                    if (App." + _GridID + @".columns[i].dataIndex != 'ID')
                        arr.push({ dataIndex: App." + _GridID + @".columns[i].dataIndex,text:App." + _GridID + @".columns[i].text,hidden:App." + _GridID + @".columns[i].hidden,position:i+1});
                      }
                    var arrData =new Array();
                    for (var i = 0; i <  App." + _GridID + @".getStore().data.items.length; i++) {
                                        arrData.push( App." + _GridID + @".getStore().data.items[i].data);
                      }
                        var params={currentPage: App." + _GridID + @".getStore().currentPage, pageSize:App." + _GridID + @".getStore().pageSize, arrObject:JSON.stringify(arr),arrData:JSON.stringify(arrData)};onDirectMethod('" + _ViewFileUrl + "',params);");
        }

        public ButtonExBuilder()
        {
            _ModuleCode = HttpContext.Current.Request.RequestContext.RouteData.DataTokens["area"].ToString();
            _FunctionCode = HttpContext.Current.Request.RequestContext.RouteData.Values["Controller"].ToString();
        }
        public virtual ButtonExBuilder ModuleCode(string value = "")
        {
            _ModuleCode = value;
            callExportFile();
            return this;
        }
        public virtual ButtonExBuilder FunctionCode(string value = "")
        {
            _FunctionCode = value;
            callExportFile();
            return this;
        }
        
        public virtual ButtonExBuilder GridID(string value = "")
        {
            _GridID = value;
            callViewFile();
            return this;
        }
        public virtual ButtonExBuilder GridIDGetFile(string value = "")
        {
            _GridID = value;
            callExportFile();
            return this;
        }
        public virtual ButtonExBuilder TitleFile(string value = "")
        {
            _Title = value;
            callExportFile();
            return this;
        }
        public virtual ButtonExBuilder isTree(bool value=false)
        {
            _isTree = value;
            if (string.IsNullOrEmpty(_ExportFileUrl))
                callViewFile();
            else
                callExportFile();
            return this;
        }
        
    }
    public static class ExtExtendExportExcel
    {
        public static ButtonExBuilder ButtonOpenWindowExpExcel(this BuilderFactory X)
        {
            var button = new ButtonExBuilder();
            button.Icon(Icon.DiskDownload);
            button.Text("Xuất Excel");
            button.OpenWindowUrl = X.XController().UrlHelper.Action("ExportWindow", "ExportExcelFile", new { Area = "Generic" });            
            return button;
        }
        public static ButtonExBuilder ButtonExportExcelFile(this BuilderFactory X)
        {
            var button = new ButtonExBuilder();
            button.Icon(Icon.DiskDownload);
            button.Text("Xuất Excel");
            button.isTree(false);
            button.ExportFileUrl = X.XController().UrlHelper.Action("ExportExcelFile", "ExportExcelFile", new { Area = "Generic" });
            return button;
        }
    }
}