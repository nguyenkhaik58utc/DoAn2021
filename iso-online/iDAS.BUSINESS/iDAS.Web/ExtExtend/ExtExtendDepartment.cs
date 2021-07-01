using Ext.Net;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace iDAS.Web.ExtExtend
{
    public class DepartmentPanel : Panel.Builder
    {
        public TreePanel.Builder Department { get; set; }
        public DepartmentPanel()
        {
            this.Title("SƠ ĐỒ TỔ CHỨC")
                .TitleAlign(Ext.Net.TitleAlign.Center)
                .Collapsible(true)
                .Region(Ext.Net.Region.West)
                .Width(330)
                .Split(true)
                .Layout(LayoutType.Fit);
        }
        public virtual DepartmentPanel Handler(string handler = "")
        {
            if (handler != "")
            {
                Department.Listeners(ls => ls.SelectionChange.Handler = @"if (selected.length > 0) {" + handler + "(selected);};");
            }
            return this;
        }
    }
    public class DepartmentContainer : Container.Builder
    {
        public DepartmentContainer(Button.Builder button, TextField.Builder textfield)
        {
            _Button = button;
            _TextField = textfield;
            this.Layout(LayoutType.Column).Height(25);
            setButtonHandler();
        }
        private Button.Builder _Button;
        private TextField.Builder _TextField;
        private string _Multiple = "true";
        private string _Handler = "selectDepartment";
        public virtual DepartmentContainer ReadOnly(bool value = false)
        {
            _Button.Hidden(value);
            return this;
        }
        public virtual DepartmentContainer AllowBlank(bool value = true)
        {
            _TextField.AllowBlank(value);
            return this;
        }
        public virtual DepartmentContainer FieldLabel(string value = "")
        {
            _TextField.FieldLabel(value);
            return this;
        }
        public virtual DepartmentContainer Multiple(bool value = true)
        {
            _Multiple = value ? "true" : "false";
            setButtonHandler();
            return this;
        }
        public virtual DepartmentContainer Handler(string handler = "")
        {
            _Handler = handler;
            setButtonHandler();
            return this;
        }
        private void setButtonHandler() {
            var handler = "var container = this.up('container'); var value = container.queryById('hdfDepartmentID').value; OpenDepartmentWindow('{0}',{1},value)";
            handler = string.Format(handler, _Handler, _Multiple);
            _Button.Listeners(ls => ls.Click.Handler = handler);
        }
    }
    public static class ExtExtendDepartment
    {
        #region DepartmentTree
        private static TreePanel.Builder createDepartmentTree(BuilderFactory X)
        {
            var loadHandler = @"if (records.length > 0) {
                                    for (var i = 0; i < records.length; i++) {
                                        var node = records[i];
                                        if (node.getDepth() < 2 && !node.data.leaf && !node.data.expanded) {
                                            node.expand();
                                        }
                                    }
                                };
                                if (this.selModel.getCount() <= 0) {
                                    this.selModel.select(0);
                                };";
            var url = X.XController().UrlHelper.Action("LoadData", "Department", new { Area = "Department" });
            var tree = X.TreePanel()
                        .Header(false)
                        .TitleAlign(TitleAlign.Center)
                        .Layout(LayoutType.Fit)
                        .Icon(Icon.TextListBullets)
                        .ForceFit(true)
                        .RootVisible(false)
                        .RowLines(true)
                        .SingleExpand(false)
                        .Border(false)
                        .Region(Region.West)
                        .HideHeaders(true)
                        .Listeners(ls => ls.Load.Handler = loadHandler)
                        .MultiSelect(false)
                        .Store(
                            X.TreeStore()
                                .Proxy(X.AjaxProxy().Url(url).Reader(X.JsonReader().Root("data")))
                                .Model(
                                    X.Model().Fields(
                                        X.ModelField().Name("id"),
                                        X.ModelField().Name("text"),
                                        X.ModelField().Name("ParentID")
                                    )
                                )
                        )
                        .ColumnModel(X.TreeColumn().Align(Alignment.Left).DataIndex("text").Flex(1))
                        .BottomBarItem(
                            Html.X().Button().Text("Mở rộng").Handler("this.up('treepanel').expandAll();").Icon(Icon.ControlAddBlue),
                            Html.X().ToolbarSeparator(),
                            Html.X().Button().Text("Thu gọn").Handler("this.up('treepanel').collapseAll();").Icon(Icon.ControlRemoveBlue)
                        );
            return tree;
        }
        public static DepartmentPanel DepartmentPanel(this BuilderFactory X)
        {
            var department = createDepartmentTree(X);
            var panel = new DepartmentPanel();
            panel.Department = department;
            panel.Items(department);
            return panel;
        }
        #endregion

        #region DepartmentField
        private static DepartmentContainer createDepartmentContainer(BuilderFactory X, string key, HumanDepartmentViewItem value, string cls)
        {
            var script = @"<script>
                                var selectDepartment = function(records) {
                                    if (records != null && records.length > 0){
                                        var data = '';
                                        records.forEach(function (obj) {
                                            var item = obj.data.id + ',';
                                            data = data + item;
                                        });
                                        var departmentContainer = App." + key.Replace(".","") + @"Container;
                                        var text = records[0].data.text;    
                                        if(records.length > 1){
                                            text = 'Đã chọn: '+ records.length +' Phòng ban';
                                        };        
                                        departmentContainer.queryById('hdfDepartmentID').setValue(data);
                                        departmentContainer.queryById('txtDepartmentName').setValue(text);
                                    };
                                };
                            </script>";
            var button = X.Button().Icon(Icon.FolderHome).ToolTip("Lựa chọn phòng ban");
            var textfield = X.TextField()
                            .ColumnWidth(1)
                            .Value(value.Name)
                            .HtmlBin(c => script)                           
                            .FieldLabel("Phòng ban")
                            .ItemID("txtDepartmentName")
                            .EmptyText("Lựa chọn phòng ban hoặc đơn vị")
                            .BlankText("Phòng ban không được bỏ trống!")
                            .FieldCls(cls)
                            .ReadOnly(true)
                            .RightButtons(button);
            var container = new DepartmentContainer(button, textfield);
            container
                .ID(key.Replace(".", "") + "Container")
                .Items(
                    X.HiddenFor(key + "strIDs").ItemID("hdfDepartmentID"),
                    textfield
                );
            return container;
        }
        public static DepartmentContainer DepartmentFieldFor<TModel, TProperty>(this BuilderFactory<TModel> X, Expression<Func<TModel, TProperty>> expression, string cls="")
           where TProperty : HumanDepartmentViewItem
        {
            var key = X.GetKey(expression);
            var value = X.GetValue(expression);
            var container = createDepartmentContainer(X, key, value, cls);
            return container;
        }
        public static DepartmentContainer DepartmentFieldFor<TModel>(this BuilderFactory<TModel> X, string key, string cls)
        {
            var value = X.GetValue<TModel, HumanDepartmentViewItem>(key);
            if (!string.IsNullOrEmpty(key)) key += '.';
            var container = createDepartmentContainer(X, key, value, cls);
            return container;
        }
        #endregion
    }
}