using Ext.Net;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace iDAS.Web.ExtExtend
{
    public class UploadFileContainer : Container.Builder
    {
        internal Hidden.Builder HiddenField { get; set; }
        internal Container.Builder Container { get; set; }
        internal FileUploadField.Builder FileUploadField { get; set; }
        private string _FileFilter = "*";
        private bool _Multiple = true;
        public virtual UploadFileContainer ReadOnly(bool value = false)
        {
            HiddenField.Value(value);
            Container.Hidden(value).Disabled(value);
            return this;
        }
        public virtual UploadFileContainer FileFilter(string value = "")
        {
            _FileFilter = value;
            var config = getConfigFile();
            FileUploadField.Listeners(ls => ls.AfterRender.Handler = config);
            return this;
        }
        public virtual UploadFileContainer Multiple(bool value = true)
        {
            _Multiple = value;
            var config = getConfigFile();
            FileUploadField.Listeners(ls => ls.AfterRender.Handler = config);
            return this;
        }
        private string getConfigFile()
        {
            var config = "this.fileInputEl.set({ accept: '" + _FileFilter + "' });";
            if (_Multiple)
            {
                config = config + "this.fileInputEl.set({ multiple: true });";
            }
            return config;
        }
    }
    public static class ExtExtendUploadFile
    {
        private static UploadFileContainer createFileUploadField<TModel>(BuilderFactory<TModel> X, string key, AttachmentFileItem value)
        {
            key = key.Replace(".", "");
            var container = new UploadFileContainer();
            var hdfReadOnly = X.Hidden().ID(key + "ReadOnly").Value(false);
            if (value.Files.Count > 0)
            {
                var hdfFileRemove = X.Hidden().ID(key + ".ListFileRemove");
                var mnFileUrl = X.XController().UrlHelper.Action("AttachmentFileView", "File", new { Area = "Generic" });
                var fileCount = value.Files.Count == 0 ? string.Empty : "Đã chọn: " + value.Files.Count + " tệp đính kèm";
                var container1 = X.Container().ColumnWidth(0.5).Layout(LayoutType.Column).MarginSpec("0 0 5 0");
                var textField1 = X.TextField().ID(key + "Text1").FieldLabel("Tệp đính kèm").Value(fileCount).EmptyText("Chọn tệp đính kèm ...").ColumnWidth(1);
                var button1 = X.Button().Icon(Icon.FolderPageWhite).Text("Quản lý tệp")
                                .OnClientClick("var params={files:'" + value.ListFile + "', key:'" + key + "'}; onDirectMethod('" + mnFileUrl + "',params);");
                container1.Add(hdfFileRemove);
                container1.Add(textField1);
                container1.Add(button1);
                container.Add(container1);
            }
            var container2 = X.Container().ColumnWidth(0.5).MarginSpec("0 0 5 0").Layout(LayoutType.Column);
            var textField2 = X.TextField().ID(key + "Text2").FieldLabel("Thêm đính kèm").EmptyText("Chọn tệp đính kèm ...").ColumnWidth(1);
            var fileUploadField = X.FileUploadField().ID(key + ".FileAttachments")
                .ButtonOnly(true).Icon(Icon.Attach).ButtonText("Chọn tệp")
                .Listeners(ls => ls.AfterRender.Handler = "this.fileInputEl.set({ accept: '*' });this.fileInputEl.set({ multiple: true });")
                .Listeners(ls => ls.Change.Handler = "App." + key + "Text2.setValue('Đã thêm: '+this.fileInputEl.dom.files.length+' tệp đính kèm')");
            var button2 = X.Button().Icon(Icon.Delete).Text("Hủy")
                            .OnClientClick("Ext.getCmp('" + key + ".FileAttachments').fileInputEl.setValue(null); App." + key + "Text2.setValue(null)");
            container2.Add(textField2);
            container2.Add(fileUploadField);
            container2.Add(button2);
            container2.Add(hdfReadOnly);
            container.Add(container2);
            container.FileUploadField = fileUploadField;
            container.Container = container2;
            container.HiddenField = hdfReadOnly;
            return container;
        }

        public static UploadFileContainer FileUploadFieldFor<TModel, TProperty>(this BuilderFactory<TModel> X, Expression<Func<TModel, TProperty>> expression)
            where TProperty : AttachmentFileItem
        {
            var key = X.GetKey(expression);
            var value = X.GetValue(expression);
            var fileUploadField = createFileUploadField(X, key, value);
            return fileUploadField;
        }
    }
}