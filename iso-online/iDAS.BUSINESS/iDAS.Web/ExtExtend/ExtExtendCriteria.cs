using Ext.Net;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace iDAS.Web.ExtExtend
{

    public class CriteriaContainer : Container.Builder
    {
        public Button.Builder ButtonCriteria { get; set; }
        public TextField.Builder TextCriteria { get; set; }
        public CriteriaContainer()
        {
            this.Layout(LayoutType.Column).MarginSpec("0 -1 0 0").Height(25);
        }
        public virtual CriteriaContainer ReadOnly(bool value = false)
        {
            ButtonCriteria.Hidden(value);
            return this;
        }
        public virtual CriteriaContainer FieldLabel(string value = "Tiêu chí")
        {
            TextCriteria.FieldLabel(value);
            return this;
        }
        public virtual CriteriaContainer AllowBlank(bool value = true)
        {
            TextCriteria.AllowBlank(value).EmptyText("Click để lựa chọn tiêu chí-->")
                        .BlankText("Tiêu chí không được bỏ trống!");
            return this;
        }
    }
    public static class ExtExtendCriteria
    {
        public static CriteriaContainer CreateCriteriaField<TModel>(BuilderFactory<TModel> X, string CriteriaKey, AuditCriteriaViewItem Criteria)
        {
            var urlCriteriaWindow = X.XController().UrlHelper.Action("WindowCriteria", "CriteriaCategory", new { Area = "Quality" });
            var script = @"<script>
                                    var openCriteriaWindow = function (url) {
                                        onDirectMethod(url);
                                    };
                                    var ctParent;
                                    function onSelectCriteriaExtend(records) {
                                            if (records.length == 1){
                                                    ctParent.queryById('hdfCriteriaID').setValue(records[0].get('ID'));
                                                    ctParent.queryById('txtCriteriaName').setValue(records[0].get('Name'));
                                                    ctParent.queryById('txtCategoryName').setValue('Bộ tiêu chí: '+records[0].get('CategoryName'));
                                                }
                                                else {
                                                    Ext.MessageBox.show({ title: 'Thông báo', msg: 'Lựa chọn duy nhất một bản ghi!', buttons: { yes: 'Đồng ý' }, iconCls: '#Information' });
                                                };
                                            };
                            </script>";
            var button = X.Button().Icon(Icon.Pencil)
                                    .ItemID("btnCriteriaSelect")
                                    .Listeners(ls => ls.Click.Handler = "ctParent= this.up('container'); openCriteriaWindow('" + urlCriteriaWindow + "');")
                                    .ToolTip("Lựa chọn tiêu chí");
            var textbox = X.TextField()
                        .ItemID("txtCriteriaName")
                        .Text(Criteria.Name)
                        .ReadOnly(true)
                        .ColumnWidth(0.55)
                        .HtmlBin(c => script);
            var container = new CriteriaContainer();
            container.ButtonCriteria = button;
            container.TextCriteria = textbox;
            container
                .Items(
                        X.HiddenFor(CriteriaKey + "ID").ItemID("hdfCriteriaID").ColumnWidth(0.5),
                            textbox.RightButtons(
                               button
                            ),
                        X.TextField().Text("Bộ tiêu chí: " + Criteria.Category)
                        .HideLabel(true).ReadOnly(true)
                        .ItemID("txtCategoryName")
                        .ColumnWidth(0.45).Icon(Icon.Door)
                );
            return container;
        }
        public static CriteriaContainer CriteriaFieldFor<TModel>(this BuilderFactory<TModel> X, string CriteriaKey)
        {
            var Criteria = X.GetValue<TModel, AuditCriteriaViewItem>(CriteriaKey);
            if (!string.IsNullOrEmpty(CriteriaKey)) CriteriaKey += '.';
            var container = CreateCriteriaField(X, CriteriaKey, Criteria);
            return container;
        }
        public static CriteriaContainer CriteriaFieldFor<TModel, TProperty>(this BuilderFactory<TModel> X, Expression<Func<TModel, TProperty>> expression)
            where TProperty : AuditCriteriaViewItem
        {
            var CriteriaKey = X.GetKey(expression);
            var Criteria = X.GetValue(expression);
            var container = CreateCriteriaField(X, CriteriaKey, Criteria);
            return container;
        }
    }
}