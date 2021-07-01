using Ext.Net;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace iDAS.Web.ExtExtend
{
    public class HtmlEditorContainer : Container.Builder
    {
        internal HtmlEditor _HtmlEditor { get; set; }

        public virtual HtmlEditorContainer ReadOnly(bool value = false)
        {
            _HtmlEditor.ReadOnly = true;
            return this;
        }
    }
    public static class ExtExtendHtmlEditor
    {
        public static HtmlEditorContainer HtmlEditorFieldFor<TModel, TProperty>(this BuilderFactory<TModel> X, Expression<Func<TModel, TProperty>> expression)
        {
            var url = X.XController().UrlHelper.Action("BrowseImage", "File", new { area = "Generic" });
            var render = @"var toolbar = this.getToolbar();
                           var browseImg = function(){ Ext.net.DirectMethod.request({url: '"+url+"'});};"+
                          "toolbar.add({ handler: function () { browseImg(); }, iconCls: 'icon-imageedit', text: '', tooltip: '<b> Insert Image</b><br>Upload a new Image</br>' });";
            var button = X.Button().Icon(Icon.ImageEdit).Hidden(true);
            var htmlEditor = X.HtmlEditorFor(expression).HideLabel(true).ID("txtHtml")
                            .Listeners(ls => ls.Render.Handler = render);
            var container = new HtmlEditorContainer();
            container.Layout(LayoutType.Fit).Items(
                button,
                htmlEditor
            );
            container._HtmlEditor = htmlEditor;
            return container;
        }
    }
}