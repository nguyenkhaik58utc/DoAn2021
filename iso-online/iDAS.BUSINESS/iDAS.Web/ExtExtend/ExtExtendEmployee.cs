using Ext.Net;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.ExtExtend
{
    public class EmployeeFieldSet : FieldSet.Builder
    {
        public Button.Builder ButtonEmployee { get; set; }
        public TextField.Builder TextEmployee { get; set; }
        private string _LoadImageUrl = (new UrlHelper(HttpContext.Current.Request.RequestContext)).Action("LoadImage", "File", new { Area = "Generic" });
        public EmployeeFieldSet()
        {
            var script = @"<script>
                            var openEmployeeWindow = function (url) {
                                var params = {
                                    multi : false,    
                                };
                                onDirectMethod(url, params);
                            };
                            var employeeFieldSet;
                            var selectEmployee = function(records) { 
                                if (records.length>0){
                                    var record = records[0];
                                    var urlImage='" + _LoadImageUrl + @"?fileId='+record.data.FileID+'&fileName='+record.data.FileName;
                                    employeeFieldSet.queryById('imgApprovalAvatar').setImageUrl(urlImage);
                                    employeeFieldSet.queryById('dfApproveName').setValue(record.get('Name'));
                                    employeeFieldSet.queryById('dfDepartment').setValue(record.get('DepartmentName'));
                                    employeeFieldSet.queryById('dfRole').setValue(record.get('Role'));
                                    employeeFieldSet.queryById('hdfemployeeID').setValue(record.get('ID'));
                                }
                            } 
                        </script>";
            this.Layout(LayoutType.HBox)
                            .LayoutConfig(new HBoxLayoutConfig { Align = HBoxAlign.Middle, Pack = BoxPack.Start })
                            .Height(105)
                            .HtmlBin(c => script)
                            .PaddingSpec("0 0 0 5");
        }
        public virtual EmployeeFieldSet ReadOnly(bool value = false)
        {
            ButtonEmployee.Hidden(value);
            return this;
        }
        public virtual EmployeeFieldSet AllowBlank(bool value = true)
        {
            if (!value)
            {
                TextEmployee.EmptyText("Nhân sự")
                            .BlankText("Nhân sự không được bỏ trống!")
                            .FieldStyle("font-weight:bold").MarginSpec("0 0 5 5")
                            .HideLabel(true)
                            .Flex(1)
                            .AllowOnlyWhitespace(false)
                            .Listeners(ls => ls.ValidityChange.Handler =
                                @"if(!isValid){ this.show(); this.up().queryById('dfApproveName').hide();} 
                                            else{this.hide();this.up().queryById('dfApproveName').show();}");
            }
            return this;
        }
        public virtual EmployeeFieldSet CallBack(string fnScript = "")
        {
            if (fnScript != "")
            {
                var script = @"<script>
                            var openEmployeeWindow = function (url) {
                                var params = {
                                    multi : false,    
                                };
                                onDirectMethod(url, params);
                            };
                            var employeeFieldSet;
                            var selectEmployee = function(records) { 
                                if (records.length>0){
                                    var record = records[0];
                                    employeeFieldSet.queryById('imgApprovalAvatar').setImageUrl(record.get('Avatar'));
                                    employeeFieldSet.queryById('dfApproveName').setValue(record.get('Name'));
                                    employeeFieldSet.queryById('dfDepartment').setValue(record.get('DepartmentName'));
                                    employeeFieldSet.queryById('dfRole').setValue(record.get('Role'));
                                    employeeFieldSet.queryById('hdfemployeeID').setValue(record.get('ID')); 
                                    " + fnScript + @"(record);
                                };
                            }; 
                        </script>";
                this.HtmlBin(c => script);
            }
            return this;
        }
    }

    public class EmployeeBoxSet : Container.Builder
    {
        public EmployeeFieldSet EmployeeFieldSet { get; set; }
        public DisplayField.Builder dsField { get; set; }
        public EmployeeBoxSet()
        {
            this.Layout(LayoutType.HBox)
                .Height(95)
                .LayoutConfig(new HBoxLayoutConfig { Align = HBoxAlign.Top, Pack = BoxPack.Start });
        }
        public virtual EmployeeBoxSet ReadOnly(bool value = false)
        {
            EmployeeFieldSet.ReadOnly(value);
            return this;
        }
        public virtual EmployeeBoxSet FieldLabel(string value = "Nhân sự")
        {
            dsField.Value(value + ":");
            return this;
        }
        public virtual EmployeeBoxSet LabelWidth(int value = 100)
        {
            dsField.Width(value + 5);
            return this;
        }
        public virtual EmployeeBoxSet AllowBlank(bool value = true)
        {
            EmployeeFieldSet.AllowBlank(value);
            return this;
        }
    }

    public static class ExtExtendEmployee
    {
        private static EmployeeFieldSet createEmployeeFieldSet<TModel>(BuilderFactory<TModel> X, string employeeKey, HumanEmployeeViewItem employee)
        {
            var urlEmployeeWindow = X.XController().UrlHelper.Action("EmployeeWindow", "Employee", new { Area = "Human" });
            var button = X.Button()
                            .Icon(Icon.UserEdit)
                            .ToolTip("Lựa chọn nhân sự")
                            .Listeners(ls => ls.Click.Handler = @"employeeFieldSet = this.up('fieldset'); openEmployeeWindow('" + urlEmployeeWindow + "');");
            var textbox = X.TextFieldFor(employeeKey + "ID").ItemID("hdfemployeeID").Hidden(true);
            var container = new EmployeeFieldSet();
            container.ButtonEmployee = button;
            container.TextEmployee = textbox;
            container.ItemID("ctnEmployee").Items(
                                X.ImageButton()
                                    .ItemID("imgApprovalAvatar")
                                    .ToolTip("Xem chi tiết nhân sự")
                                    .ImageUrl(employee.Avatar)
                                    .Height(80).Width(60)
                                    .StyleSpec("border: 1px solid #b5b8c8;")
                                    .Listeners(ls => ls.Click.Handler = @"var id = this.up().queryById('hdfemployeeID').getValue(); if (id == '' || id == 0) { return; };openEmployeeDetail(id);"),
                                X.Container()
                                    .Layout(LayoutType.Form)
                                    .Height(80)
                                    .StyleSpec("border: 1px solid #b5b8c8;").MarginSpec("0 5 0 5").Padding(0)
                                    .Flex(1)
                                    .Items(
                                        X.Container()
                                            .Layout(LayoutType.HBox)
                                            .LayoutConfig(new HBoxLayoutConfig { Align = HBoxAlign.Middle, Pack = BoxPack.Start })
                                            .StyleSpec("border-bottom: 1px solid #b5b8c8;")
                                            .Items(
                                                textbox.Listeners(ls => ls.AfterRender.Handler = "if(this.value == 0){this.setValue(null);}"),
                                                X.DisplayFieldFor(employeeKey + "Name")
                                                    .EmptyText("Nhân sự")
                                                    .ItemID("dfApproveName")
                                                    .FieldStyle("font-weight:bold").MarginSpec("0 0 5 5")
                                                    .HideLabel(true)
                                                    .Flex(1),
                                                button
                                            ),
                                        X.DisplayFieldFor(employeeKey + "Department")
                                            .ItemID("dfDepartment")
                                            .LabelStyle("font-size: 1em;font-weight: bold;")
                                            .MarginSpec("0 0 0 5")
                                            .FieldLabel("Đơn vị").LabelWidth(70),
                                        X.DisplayFieldFor(employeeKey + "Role")
                                            .ItemID("dfRole")
                                            .LabelStyle("font-size: 1em;font-weight: bold;")
                                            .MarginSpec("-4 0 0 5")
                                            .FieldLabel("Chức danh").LabelWidth(70)
                                    )
            );
            return container;
        }

        /// <summary>
        /// Employee field for model
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="X"></param>
        /// <param name="expression">Expression</param>
        /// <param name="select"></param>
        /// <returns></returns>
        public static EmployeeFieldSet EmployeeFieldFor<TModel, TProperty>(this BuilderFactory<TModel> X, Expression<Func<TModel, TProperty>> expression)
            where TProperty : HumanEmployeeViewItem
        {
            var employeeKey = X.GetKey(expression);
            var employee = X.GetValue(expression);
            var container = createEmployeeFieldSet(X, employeeKey, employee);
            return container;
        }

        /// <summary>
        /// Employee field for model
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="X"></param>
        /// <param name="employeeKey">String</param>
        /// <param name="select"></param>
        /// <returns></returns>
        public static EmployeeFieldSet EmployeeFieldFor<TModel>(this BuilderFactory<TModel> X, string employeeKey)
        {
            var employee = X.GetValue<TModel, HumanEmployeeViewItem>(employeeKey);
            if (!string.IsNullOrEmpty(employeeKey)) employeeKey += '.';
            var container = createEmployeeFieldSet(X, employeeKey, employee);
            return container;
        }

        public static EmployeeBoxSet EmployeeBoxFor<TModel, TProperty>(this BuilderFactory<TModel> X, Expression<Func<TModel, TProperty>> expression)
            where TProperty : HumanEmployeeViewItem
        {
            var employeeKey = X.GetKey(expression);
            var employee = X.GetValue(expression);
            var employeeField = createEmployeeFieldSet(X, employeeKey, employee);
            var dsField = X.DisplayField();
            var container = new EmployeeBoxSet();
            container.dsField = dsField;
            container.EmployeeFieldSet = employeeField;
            container.Items(dsField, employeeField.Flex(1).StyleSpec("background-color: white;").Height(90));
            return container;

        }

    }
}