using Ext.Net;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
namespace iDAS.Web.ExtExtend
{
    public class CustomerCategoryField : Container.Builder
    {
        public Button.Builder ButtonSelect { get; set; }
        public Button.Builder ButtonShow { get; set; }
        public TextField.Builder TextField { get; set; }
        public CustomerCategoryField()
        {
            this.Layout(LayoutType.Column).MarginSpec("0 -1 0 0").Height(25);
        }
        public virtual CustomerCategoryField ReadOnly(bool value = false)
        {
            ButtonSelect.Hidden(value);
            return this;
        }
        public virtual CustomerCategoryField FieldLabel(string value = "Nhóm khách hàng")
        {
            TextField.FieldLabel(value);
            return this;
        }
        public virtual CustomerCategoryField AllowBlank(bool value = true)
        {
            TextField.AllowBlank(value).EmptyText("Nhóm khách hàng")
                        .BlankText("Nhóm khách hàng không được bỏ trống!");
            return this;
        }
    }
    public static class ExtExtendCustomerCategory
    {
        public static CustomerCategoryField CustomerCategoryFieldFor<TModel, TProperty>(this BuilderFactory<TModel> X, Expression<Func<TModel, TProperty>> expression)
            where TProperty : CustomerCategorySelectItem
        {
            var key = X.GetKey(expression);
            var value = X.GetValue(expression);
            var container = createCustomerCategoryField(X, key, value);
            return container;
        }
        public static CustomerCategoryField CustomerCategoryFieldFor<TModel>(this BuilderFactory<TModel> X, string key)
        {
            var value = X.GetValue<TModel, CustomerCategorySelectItem>(key);
            if (!string.IsNullOrEmpty(key)) key += '.';
            var container = createCustomerCategoryField(X, key, value);
            return container;
        }
        private static CustomerCategoryField createCustomerCategoryField(BuilderFactory X, string key, CustomerCategorySelectItem value)
        {
            var urlSelectCustomerWindow = X.XController().UrlHelper.Action("MultipleSelect", "GroupCustomer", new { Area = "Customer" });
            var urlDetailCustomerWindow = X.XController().UrlHelper.Action("MultipleDetail", "GroupCustomer", new { Area = "Customer" });
            var script = @"<script>
                                var openWindowDetailGroupCustomer = function(url){
                                                        var ids = App.hdfCustomerCategory.value;
                                                        if(ids == null || ids==''){
                                                                        Ext.Msg.alert('Thông báo','Không có khách hàng nào!');
                                                                        return;
                                                                };
                                                            var params = {
                                                                        ids: ids,
                                                                    };
                                                        onDirectMethod(url,params);
                                                    };
                                var openWindowSelectGroupCustomer = function(url){
                                                         var ids = App.hdfCustomerCategory.value;
                                                         var params = {
                                                                        ids: ids,
                                                                    };
                                                        onDirectMethod(url,params);
                                                    };
                                var cgContainer; 
                                var OnSelectGroupCustomer = function(records) {
                                        var textValue='Gồm '+records.length+' nhóm khách hàng( ';
                                        var aId = new Array();
                                        var aText = new Array();
                                        for(var i=0; i < records.length; i++){
                                             aText[i]= records[i].text;
                                             aId[i] = records[i].id;
                                            };
                                        textValue += aText.join()+')';
                                        cgContainer.setValue(textValue);
                                        App.hdfCustomerCategory.setValue(aId.join());
                                    };
                            </script>";
            var container = new CustomerCategoryField();
            container.ButtonSelect = X.Button().Text("Chọn").Icon(Icon.Pencil)
                                    .Listeners(ls => ls.Click.Handler = "cgContainer = this.up(); openWindowSelectGroupCustomer('" + urlSelectCustomerWindow + "');")
                                    .ToolTip("Lựa chọn nhóm khách hàng");
            container.ButtonShow = X.Button().Text("Xem").Border(true).Icon(Icon.Zoom)
                                    .Listeners(ls => ls.Click.Handler = "openWindowDetailGroupCustomer('" + urlDetailCustomerWindow + "');")
                                    .ToolTip("Xem chi tiết nhóm khách hàng");
            var textField = X.TextField().Value(value.Name)
                .HtmlBin(c => script)
                .FieldLabel("Nhóm khách hàng")
                .ReadOnly(true)
                .RightButtons(container.ButtonSelect, container.ButtonShow)
                .ItemID("txtCustomerCategoryName");
            container.TextField = textField;
            container.Items(
                X.HiddenFor(key + "GroupIds").ID("hdfCustomerCategory"),
                textField.ColumnWidth(1)
                );
            return container;
        }
    }
}