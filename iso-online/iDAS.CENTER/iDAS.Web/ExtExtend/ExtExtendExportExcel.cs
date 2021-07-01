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
    public class ExportExcelWindow : Window.Builder
    {
        public FormPanel.Builder Form { get; set; }
        public Container.Builder Container { get; set; }
        public Button.Builder Button { get; set; }
        public ExportExcelWindow()
        {
            this.Header(false)
                .Height(1)
                .Maximized(true)
                .Constrain(true)
                .Layout(LayoutType.Fit)
                .Modal(true)
                .Resizable(false)
                .BodyPadding(0)
                .Border(false);
        }
        public virtual ExportExcelWindow UrlSubmit(string url)
        {
            Form.Url(url).Method(HttpMethod.POST);
            return this;
        }
        public virtual ExportExcelWindow ItemsExtend(params AbstractComponent[] items)
        {
            Container.Items(items);
            return this;
        }
        public virtual ExportExcelWindow ScriptSubmit(string value)
        {
            var approvalScript = @" var window = this.up('window');
                                var form = window.down('form');
                                form.submit({
                                    success: function (form, action) {
                                        window.close();
                                        {0} 
                                    }
                                });";
            approvalScript = approvalScript.Replace("{0}", value);
            Button.Handler(approvalScript);
            return this;
        }
    }
    public static class ExtExtendExportExcel
    {
    }
}