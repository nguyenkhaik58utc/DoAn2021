using Ext.Net;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace iDAS.Web.ExtExtend
{
    public class SitemapContainer : Container.Builder
    {
        public SitemapContainer(Button.Builder button, TextField.Builder textfield)
        {
            _Button = button;
            _TextField = textfield;
            this.Layout(LayoutType.Column).Height(25);
            setButtonHandler();
        }
        private Button.Builder _Button;
        private TextField.Builder _TextField;
        public virtual SitemapContainer ReadOnly(bool value = false)
        {
            _Button.Hidden(value);
            return this;
        }
        public virtual SitemapContainer AllowBlank(bool value = true)
        {
            _TextField.AllowBlank(value);
            return this;
        }
        public virtual SitemapContainer FieldLabel(string value = "")
        {
            _TextField.FieldLabel(value);
            return this;
        }
        
        private void setButtonHandler()
        {
            var handler = "var container = this.up('container'); var value = container.queryById('hdfSitemapID').value; OpenSitemapWindow('{0}',value)";
            handler = string.Format(handler, "selectSitemap");
            _Button.Listeners(ls => ls.Click.Handler = handler);
        }
    }
    public static class ExtExtendSitemap
    {
        private static SitemapContainer createSitemapContainer(BuilderFactory X, string key, SitemapItem value)
        {
            var script = @"<script>
                            var selectSitemap = function(record) {
                                var SitemapContainer = App." + key.Replace(".", "") + @"Container;
                                SitemapContainer.queryById('hdfSitemapID').setValue(record.data.id);
                                SitemapContainer.queryById('txtSitemapName').setValue(record.data.text);
                            };
                        </script>";
            var button = X.Button().Icon(Icon.FolderHome).ToolTip("Lựa chọn trang");
            var textfield = X.TextField().ColumnWidth(1)
                            .Value(value.Text)
                            .HtmlBin(c => script)
                            .FieldLabel("Trang web")
                            .ItemID("txtSitemapName")
                            .EmptyText("Lựa chọn trang web")
                            .AllowBlank(true)
                            .FormBind(false)
                            .ReadOnly(true)
                            .RightButtons(button);
            var container = new SitemapContainer(button, textfield);
            container
                .ID(key.Replace(".", "") + "Container")
                .Items(
                    X.HiddenFor(key + "ID").ItemID("hdfSitemapID").Value(value.ID),
                    textfield
                );
            return container;
        }
        public static SitemapContainer SitemapFieldFor<TModel, TProperty>(this BuilderFactory<TModel> X, Expression<Func<TModel, TProperty>> expression)
           where TProperty : SitemapItem
        {
            var key = X.GetKey(expression);
            var value = X.GetValue(expression);
            var container = createSitemapContainer(X, key, value);
            return container;
        }
    }
}