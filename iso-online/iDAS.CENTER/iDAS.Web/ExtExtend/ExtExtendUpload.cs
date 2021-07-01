using Ext.Net;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace iDAS.Web.ExtExtend
{
    public class UploadFieldSet : FieldSet.Builder
    {
        internal Image _ImagePreview { get; set; }
        internal FileUploadField _FileUploadField { get; set; }
       
        public virtual UploadFieldSet ReadOnly(bool value = false)
        {
            _FileUploadField.Hidden = true;
            return this;
        }
        public virtual UploadFieldSet ButtonOnly(bool value = false)
        {
            _FileUploadField.ButtonOnly = value;
            return this;
        }
    }
    public static class ExtExtendUpload
    {
        private static UploadFieldSet createUploadField<TModel>(BuilderFactory<TModel> X, string key, AttachmentFileItem value)
        {
            key = key.Replace(".", "");
            var script = @" var file = this.fileInputEl.dom.files[0]; var url = URL.createObjectURL(file);";

            var hiddenField = X.Hidden().ID(key + ".ID").Value(value.ID);
            var fileUploadField = X.FileUploadField().ID(key + ".FileAttachments")
                .StyleSpec("text-align:left")
                .Value(value.Name)
                .FieldLabel("Tệp đính kèm")
                .ButtonOnly(false).Icon(Icon.ImageAdd).ButtonText("Chọn tệp")
                .Listeners(ls => ls.AfterRender.Fn = "function(cmp){cmp.fileInputEl.set({ accept:'*' });}")
                .Listeners(ls => ls.Change.Handler = script);
           
            var fieldset = new UploadFieldSet();
            fieldset.Layout(LayoutType.Form)
                .Margin(0)
                .Padding(0)
                .Border(false)
                .Items(
                    hiddenField,
                    fileUploadField
                );
            fieldset._FileUploadField = fileUploadField;
            return fieldset;
        }

        public static UploadFieldSet UploadFieldFor<TModel, TProperty>(this BuilderFactory<TModel> X, Expression<Func<TModel, TProperty>> expression)
            where TProperty : AttachmentFileItem
        {
            var key = X.GetKey(expression);
            var value = X.GetValue(expression);
            var fileUploadField = createUploadField(X, key, value);
            return fileUploadField;
        }
    }
}