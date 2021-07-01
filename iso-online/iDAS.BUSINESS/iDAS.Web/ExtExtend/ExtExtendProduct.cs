using Ext.Net;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace iDAS.Web.ExtExtend
{
    public class ProductContainer : Container.Builder
    {
        public ProductContainer(Button.Builder button, TextField.Builder textfield)
        {
            _Button = button;
            _TextField = textfield;
            this.Layout(LayoutType.Column).MarginSpec("0 1 0 0").Height(25);
        }
        private Button.Builder _Button;
        private TextField.Builder _TextField;
        public virtual ProductContainer ReadOnly(bool value = false)
        {
            _Button.Hidden(value);
            return this;
        }
        public virtual ProductContainer AllowBlank(bool value = true)
        {
            _TextField.AllowBlank(value);
            return this;
        }
        public virtual ProductContainer FieldLabel(string value = "")
        {
            _TextField.FieldLabel(value);
            return this;
        }
    }
    public static class ExtExtendProduct
    {

        #region productField
        private static ProductContainer createProductContainer(BuilderFactory X, string key, ProductViewItem value)
        {
            var urlproductWindow = X.XController().UrlHelper.Action("ProductWindow", "Product", new { Area = "Production" });
            var script = @"<script>
                                var ProductContainer;
                                var selectProduct = function(records) {
                                    if (records.length > 0){
                                            var record = records[0];
                                            ProductContainer.queryById('hdfproductID').setValue(record.get('ID'));
                                            ProductContainer.queryById('txtproductName').setValue(record.get('Name'));
                                        }
                                    };
                            </script>";
            var button = X.Button().Icon(Icon.Pencil)
                           .Listeners(ls => ls.Click.Handler = "ProductContainer = this.up('container'); onDirectMethod('" + urlproductWindow + "');")
                           .ToolTip("Lựa chọn giá trị");
            var textfield = X.TextField().ColumnWidth(1)
                            .Value(value.Name)
                            .HtmlBin(c => script)
                            .FieldLabel("Sản phẩm")
                            .ItemID("txtproductName")
                            .EmptyText("Lựa chọn sản phẩm")
                            .BlankText("Dữ liệu không được bỏ trống!")
                            .ReadOnly(true)
                            .RightButtons(button);
            var container = new ProductContainer(button, textfield);
            container.Items(
                        X.HiddenFor(key + "ID").ItemID("hdfproductID"),
                        textfield
                     );
            return container;
        }
        public static ProductContainer ProductFieldFor<TModel, TProperty>(this BuilderFactory<TModel> X, Expression<Func<TModel, TProperty>> expression)
           where TProperty : ProductViewItem
        {
            var key = X.GetKey(expression);
            var value = X.GetValue(expression);
            var container = createProductContainer(X, key, value);
            return container;
        }
        public static ProductContainer ProductFieldFor<TModel>(this BuilderFactory<TModel> X, string key)
        {
            var value = X.GetValue<TModel, ProductViewItem>(key);
            if (!string.IsNullOrEmpty(key)) key += '.';
            var container = createProductContainer(X, key, value);
            return container;
        }
        #endregion

    }
}