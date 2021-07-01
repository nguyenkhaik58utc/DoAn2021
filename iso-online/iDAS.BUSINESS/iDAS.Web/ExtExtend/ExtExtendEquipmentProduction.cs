using Ext.Net;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace iDAS.Web.ExtExtend
{
    public class EquipmentProductionContainer : Container.Builder
    {
        public EquipmentProductionContainer(Button.Builder button, TextField.Builder textfield)
        {
            _Button = button;
            _TextField = textfield;
            this.Layout(LayoutType.Column).Height(25);
        }
        private Button.Builder _Button;
        private TextField.Builder _TextField;
        public virtual EquipmentProductionContainer ReadOnly(bool value = false)
        {
            _Button.Hidden(value);
            return this;
        }
        public virtual EquipmentProductionContainer AllowBlank(bool value = true)
        {
            _TextField.AllowBlank(value);
            return this;
        }
        public virtual EquipmentProductionContainer FieldLabel(string value = "")
        {
            _TextField.FieldLabel(value);
            return this;
        }
    }
    public static class ExtExtendEquipmentProduction
    {

        #region EquipmentProductionField
        private static EquipmentProductionContainer createEquipmentProductionContainer(BuilderFactory X, string key, EquipmentViewItem value)
        {
            var urlEquipmentProductionWindow = X.XController().UrlHelper.Action("EquipmentProductionWindow", "EquipmentProduction", new { Area = "Equipment" });
            var script = @"<script>
                                var EquipmentProductionContainer;
                                var selectEquipmentProduction = function(records) {
                                    if (records.length > 0){
                                            var record = records[0];
                                            EquipmentProductionContainer.queryById('hdfEquipmentProductionID').setValue(record.get('ID'));
                                            EquipmentProductionContainer.queryById('txtEquipmentProductionName').setValue(record.get('Name'));
                                        }
                                    };
                            </script>";
            var button = X.Button().Icon(Icon.Pencil)
                           .Listeners(ls => ls.Click.Handler = "EquipmentProductionContainer = this.up('container'); onDirectMethod('" + urlEquipmentProductionWindow + "');")
                           .ToolTip("Lựa chọn giá trị");
            var textfield = X.TextField().ColumnWidth(1)
                            .Value(value.Name)
                            .HtmlBin(c => script)
                            .FieldLabel("Sản phẩm")
                            .ItemID("txtEquipmentProductionName")
                            .EmptyText("Lựa chọn sản phẩm")
                            .BlankText("Dữ liệu không được bỏ trống!")
                            .ReadOnly(true)
                            .RightButtons(button);
            var container = new EquipmentProductionContainer(button, textfield);
            container.Items(
                        X.HiddenFor(key + "ID").ItemID("hdfEquipmentProductionID"),
                        textfield
                     );
            return container;
        }
        public static EquipmentProductionContainer EquipmentProductionFieldFor<TModel, TProperty>(this BuilderFactory<TModel> X, Expression<Func<TModel, TProperty>> expression)
           where TProperty : EquipmentViewItem
        {
            var key = X.GetKey(expression);
            var value = X.GetValue(expression);
            var container = createEquipmentProductionContainer(X, key, value);
            return container;
        }
        public static EquipmentProductionContainer EquipmentProductionFieldFor<TModel>(this BuilderFactory<TModel> X, string key)
        {
            var value = X.GetValue<TModel, EquipmentViewItem>(key);
            if (!string.IsNullOrEmpty(key)) key += '.';
            var container = createEquipmentProductionContainer(X, key, value);
            return container;
        }
        #endregion

    }
}