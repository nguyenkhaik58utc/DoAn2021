using Ext.Net;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace iDAS.Web.ExtExtend
{
    public class EquipmentMeasureContainer : Container.Builder
    {
        public EquipmentMeasureContainer(Button.Builder button, TextField.Builder textfield)
        {
            _Button = button;
            _TextField = textfield;
            this.Layout(LayoutType.Column).Height(25);
        }
        private Button.Builder _Button;
        private TextField.Builder _TextField;
        public virtual EquipmentMeasureContainer ReadOnly(bool value = false)
        {
            _Button.Hidden(value);
            return this;
        }
        public virtual EquipmentMeasureContainer AllowBlank(bool value = true)
        {
            _TextField.AllowBlank(value);
            return this;
        }
        public virtual EquipmentMeasureContainer FieldLabel(string value = "")
        {
            _TextField.FieldLabel(value);
            return this;
        }
    }
    public static class ExtExtendEquipmentMeasure
    {

        #region EquipmentMeasureField
        private static EquipmentMeasureContainer createEquipmentMeasureContainer(BuilderFactory X, string key, EquipmentViewItem value)
        {
            var urlEquipmentMeasureWindow = X.XController().UrlHelper.Action("EquipmentMeasureWindow", "EquipmentMeasure", new { Area = "Equipment" });
            var script = @"<script>
                                var EquipmentMeasureContainer;
                                var selectEquipmentMeasure = function(records) {
                                    if (records.length > 0){
                                            var record = records[0];
                                            EquipmentMeasureContainer.queryById('hdfEquipmentMeasureID').setValue(record.get('ID'));
                                            EquipmentMeasureContainer.queryById('txtEquipmentMeasureName').setValue(record.get('Name'));
                                        }
                                    };
                            </script>";
            var button = X.Button().Icon(Icon.Pencil)
                           .Listeners(ls => ls.Click.Handler = "EquipmentMeasureContainer = this.up('container'); onDirectMethod('" + urlEquipmentMeasureWindow + "');")
                           .ToolTip("Lựa chọn giá trị");
            var textfield = X.TextField().ColumnWidth(1)
                            .Value(value.Name)
                            .HtmlBin(c => script)
                            .FieldLabel("Sản phẩm")
                            .ItemID("txtEquipmentMeasureName")
                            .EmptyText("Lựa chọn sản phẩm")
                            .BlankText("Dữ liệu không được bỏ trống!")
                            .ReadOnly(true)
                            .RightButtons(button);
            var container = new EquipmentMeasureContainer(button, textfield);
            container.Items(
                        X.HiddenFor(key + "ID").ItemID("hdfEquipmentMeasureID"),
                        textfield
                     );
            return container;
        }
        public static EquipmentMeasureContainer EquipmentMeasureFieldFor<TModel, TProperty>(this BuilderFactory<TModel> X, Expression<Func<TModel, TProperty>> expression)
           where TProperty : EquipmentViewItem
        {
            var key = X.GetKey(expression);
            var value = X.GetValue(expression);
            var container = createEquipmentMeasureContainer(X, key, value);
            return container;
        }
        public static EquipmentMeasureContainer EquipmentMeasureFieldFor<TModel>(this BuilderFactory<TModel> X, string key)
        {
            var value = X.GetValue<TModel, EquipmentViewItem>(key);
            if (!string.IsNullOrEmpty(key)) key += '.';
            var container = createEquipmentMeasureContainer(X, key, value);
            return container;
        }
        #endregion

    }
}