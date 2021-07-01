using Ext.Net;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace iDAS.Web.ExtExtend
{
    public class UploadImageFieldSet : FieldSet.Builder
    {
        internal Image ImagePreview { get; set; }
        internal FileUploadField FileUploadField { get; set; }

        public virtual UploadImageFieldSet ReadOnly(bool value = false)
        {
            FileUploadField.Hidden = true;
            return this;
        }
        public virtual UploadImageFieldSet ImagePreviewHeight(int value)
        {
            ImagePreview.Height = value;
            return this;
        }
        public virtual UploadImageFieldSet ButtonOnly(bool value = false)
        {
            FileUploadField.ButtonOnly = value;
            return this;
        }

    }
    public static class ExtExtendUploadImage
    {
        private static UploadImageFieldSet createImageUploadField<TModel>(BuilderFactory<TModel> X, string key, AttachmentFileItem value)
        {
            key = key.Replace(".", "");
            var script = @" var file = this.fileInputEl.dom.files[0]; var url = URL.createObjectURL(file); App." + key + "ImagePreview.setImageUrl(url);";

            var hiddenField = X.Hidden().ID(key + ".ID").Value(value.ID);
            var fileUploadField = X.FileUploadField().ID(key + ".FileAttachments")
                .MarginSpec("5 25 0 25")
                .ButtonOnly(false).Icon(Icon.ImageAdd).ButtonText("Chọn tệp")
                .Listeners(ls => ls.AfterRender.Fn = "function(cmp){cmp.fileInputEl.set({ accept:'image/*' });}")
                .Listeners(ls => ls.Change.Handler = script);
            var imageButton = X.Image().ID(key + "ImagePreview").ImageUrl(X.XController().UrlHelper.Action("LoadImage", "File", new { area = "", fileId = value.ID }));

            var fieldset = new UploadImageFieldSet();
            fieldset.Layout(LayoutType.Auto)
                .Margin(0)
                .Padding(5)
                .Items(
                    hiddenField,
                    imageButton,
                    fileUploadField
                );
            fieldset.ImagePreview = imageButton;
            fieldset.FileUploadField = fileUploadField;
            return fieldset;
        }

        public static UploadImageFieldSet ImageUploadFieldFor<TModel, TProperty>(this BuilderFactory<TModel> X, Expression<Func<TModel, TProperty>> expression)
            where TProperty : AttachmentFileItem
        {
            var key = X.GetKey(expression);
            var value = X.GetValue(expression);
            var fileUploadField = createImageUploadField(X, key, value);
            return fileUploadField;
        }
    }
}