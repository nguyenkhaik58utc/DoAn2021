using Ext.Net;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace iDAS.Web.ExtExtend
{

    public class RoleContainer : Container.Builder
    {
        public Button.Builder ButtonRole { get; set; }
        public TextField.Builder TextRole { get; set; }
        public RoleContainer()
        {
            this.Layout(LayoutType.Column).MarginSpec("0 -1 0 0").Height(25);
        }
        public virtual RoleContainer ReadOnly(bool value = false)
        {
            ButtonRole.Hidden(value);
            return this;
        }
        public virtual RoleContainer FieldLabel(string value = "Chức danh")
        {
            TextRole.FieldLabel(value);
            return this;
        }
        public virtual RoleContainer AllowBlank(bool value = true)
        {
            TextRole.AllowBlank(value).EmptyText("Click để lựa chọn chức danh -->")
                        .BlankText("Chức danh không được bỏ trống!");
            return this;
        }
    }
    public static class ExtExtendRole
    {
        public static RoleContainer CreateRoleField<TModel>(BuilderFactory<TModel> X, string roleKey,HumanRoleViewItem role)
        {
            var url = X.XController().UrlHelper.Action("RoleWindow", "Role", new { Area = "Department" });
            var script = @"<script> var ctParent;
                                    function onSelectRoleExtend(records) {
                                            if (records.length == 1){
                                                    ctParent.queryById('hdfRoleID').setValue(records[0].get('ID'));
                                                    ctParent.queryById('txtRoleName').setValue(records[0].get('Name'));
                                                    ctParent.queryById('txtDepartmentName').setValue('Đơn vị: '+records[0].get('DepartmentName'));
                                                }
                                                else {
                                                    Ext.Msg.alert('', 'Lựa chọn duy nhất một bản ghi!');
                                                };
                                            };
                            </script>";
            var button = X.Button().Icon(Icon.Pencil)
                            .Listeners(ls => ls.Click.Handler = @"ctParent= this.up('container');var params = { handleClose: 'onSelectRoleExtend' }; onDirectMethod('" + url + "', params);")
                            .ToolTip("Lựa chọn chức danh");
            var textbox = X.TextField()
                            .ItemID("txtRoleName")
                            .Text(role.Name)
                            .ReadOnly(true)
                            .HtmlBin(c => script);
            var container = new RoleContainer();
            container.ButtonRole = button;
            container.TextRole = textbox;
            container
                .Items(
                        textbox.RightButtons(button).ColumnWidth(0.55),
                        X.TextField().Text("Đơn vị: " + role.Department)
                                    .HideLabel(true).ReadOnly(true)
                                    .ItemID("txtDepartmentName")
                                    .ColumnWidth(0.45).Icon(Icon.Door),
                        X.HiddenFor(roleKey + "ID").ItemID("hdfRoleID").ColumnWidth(1)
                );
            return container;
        }
        public static RoleContainer RoleFieldFor<TModel>(this BuilderFactory<TModel> X, string roleKey)
        {
            var role = X.GetValue<TModel,HumanRoleViewItem>(roleKey);
            if (!string.IsNullOrEmpty(roleKey)) roleKey += '.';
            var container = CreateRoleField(X, roleKey, role);
            return container;
        }
        public static RoleContainer RoleFieldFor<TModel, TProperty>(this BuilderFactory<TModel> X, Expression<Func<TModel, TProperty>> expression)
            where TProperty :HumanRoleViewItem
        {
            var roleKey = X.GetKey(expression);
            var role = X.GetValue(expression);
            var container = CreateRoleField(X, roleKey, role);
            return container;
        }
    }
}