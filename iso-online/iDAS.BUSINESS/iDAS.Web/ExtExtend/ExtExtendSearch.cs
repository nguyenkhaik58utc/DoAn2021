using Ext.Net;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace iDAS.Web.ExtExtend
{
    public class SearchContainer : Container.Builder
    {
        public DepartmentContainer Department { get; set; }
        public ComboBox.Builder TimeOption { get; set; }
        public DateField.Builder FromDate { get; set; }
        public DateField.Builder ToDate { get; set; }
        public SearchContainer()
        {
            this.Border(false);
        }
        public virtual SearchContainer FromDateID(string value = "")
        {
            if (!String.IsNullOrWhiteSpace(value))
            {
                FromDate.ID(value);
            }
            return this;
        }
        public virtual SearchContainer ToDateID(string value = "")
        {
            if (!String.IsNullOrWhiteSpace(value))
            {
                ToDate.ID(value);
            }
            return this;
        }
        public virtual SearchContainer Multi(bool value = false)
        {
            if (Department != null)
            {
                Department.Multiple(value);
            }
            return this;
        }
    }
    public static class ExtExtendCustomSearch
    {
        public static SearchContainer CustomSearch<TModel>(this BuilderFactory<TModel> X, string handler, bool isSearchByDeparment = true, string deparmentId = "deparmentSearchId")
        {
            #region Search time
            var timeOption = Html.X().ComboBox().ItemID("cbxOptionId").LabelWidth(70).Width(150)
                               .SelectedItems(new ListItem("Hôm nay", 0))
                               .FieldLabel("Thời gian")
                               .ValueField("ID")
                               .Listeners(l => l.Select.Handler = "onOptionTimeChange(this);")
                               .Items(
                                       new ListItem("Hôm nay", 0),
                                       new ListItem("Tuần này", 1),
                                       new ListItem("Tháng này", 2),
                                       new ListItem("Quý này", 3),
                                       new ListItem("Năm nay", 4),
                                       new ListItem("Tháng 1", 5),
                                       new ListItem("Tháng 2", 6),
                                       new ListItem("Tháng 3", 7),
                                       new ListItem("Tháng 4", 8),
                                       new ListItem("Tháng 5", 9),
                                       new ListItem("Tháng 6", 10),
                                       new ListItem("Tháng 7", 11),
                                       new ListItem("Tháng 8", 12),
                                       new ListItem("Tháng 9", 13),
                                       new ListItem("Tháng 10", 14),
                                       new ListItem("Tháng 11", 15),
                                       new ListItem("Tháng 12", 16),
                                       new ListItem("Quý 1", 17),
                                       new ListItem("Quý 2", 18),
                                       new ListItem("Quý 3", 19),
                                       new ListItem("Quý 4", 20)
                               );
            var fromdate = Html.X().DateField().ItemID("fromDateId")
                                    .FieldLabel("Từ")
                                    .ID("dfFromDate")
                                    .Icon(Icon.DateNext)
                                    .LabelWidth(30)
                                    .SelectedDate(DateTime.Now)
                                    .Width(150)
                                    .Listeners(l => l.Change.Handler = "if(this.hasFocus && this.isValid()){" + handler + "};")
                                    .Format("dd/MM/yyyy");
            var toDate = Html.X().DateField().ItemID("toDateId")
                                    .FieldLabel("Đến")
                                    .LabelWidth(30)
                                    .Width(150)
                                    .Icon(Icon.DatePrevious)
                                    .SelectedDate(DateTime.Now)
                                    .ID("dfToDate")
                                    .Listeners(l => l.Change.Handler = "if(this.hasFocus && this.isValid()){" + handler + "};")
                                    .Format("dd/MM/yyyy");
            string script =
@"<script>
        var onOptionTimeChange = function (obj) {
                                            var container = obj.up();
                                            var cmpFromDate=container.queryById('fromDateId');
                                            var cmpToDate=container.queryById('toDateId');
                                            var date = new Date();
                                            var option = obj.getValue();
                                            var quarter = Math.floor((date.getMonth() + 3) / 3);
                                            switch (option) {
                                                case 0:
                                                    cmpFromDate.setValue(date);
                                                    cmpToDate.setValue(date);
                                                    break;
                                                case 1:
                                                    var startDay = 1;
                                                    var d = date.getDay();
                                                    var weekStart = new Date(date.valueOf() - (d <= 0 ? 7 - startDay : d - startDay) * 86400000);
                                                    var weekEnd = new Date(weekStart.valueOf() + 6 * 86400000);
                                                    cmpFromDate.setValue(weekStart);
                                                    cmpToDate.setValue(weekEnd);
                                                    break;
                                                case 2:
                                                    cmpFromDate.setValue(new Date(date.getFullYear(), date.getMonth(), 1));
                                                    cmpToDate.setValue(new Date(date.getFullYear(), date.getMonth() + 1, 0));
                                                    break;
                                                case 3:
                                                    cmpFromDate.setValue(new Date(date.getFullYear(), 3 * quarter - 3, 1));
                                                    cmpToDate.setValue(new Date(date.getFullYear(), 3 * quarter + 0, 1));
                                                    break;
                                                case 4:
                                                    cmpFromDate.setValue(new Date(date.getFullYear(), 0, 1));
                                                    cmpToDate.setValue(new Date(date.getFullYear(), 11, 31));
                                                    break;
                                                case 5:
                                                    cmpFromDate.setValue(new Date(date.getFullYear(), 0, 1));
                                                    cmpToDate.setValue(new Date(date.getFullYear(), 0 + 1, 0));
                                                    break;
                                                case 6:
                                                    cmpFromDate.setValue(new Date(date.getFullYear(), 1, 1));
                                                    cmpToDate.setValue(new Date(date.getFullYear(), 1 + 1, 0));
                                                    break;
                                                case 7:
                                                    cmpFromDate.setValue(new Date(date.getFullYear(), 2, 1));
                                                    cmpToDate.setValue(new Date(date.getFullYear(), 2 + 1, 0));
                                                    break;
                                                case 8:
                                                    cmpFromDate.setValue(new Date(date.getFullYear(), 3, 1));
                                                    cmpToDate.setValue(new Date(date.getFullYear(), 3 + 1, 0));
                                                    break;
                                                case 9:
                                                    cmpFromDate.setValue(new Date(date.getFullYear(), 4, 1));
                                                    cmpToDate.setValue(new Date(date.getFullYear(), 4 + 1, 0));
                                                    break;
                                                case 10:
                                                    cmpFromDate.setValue(new Date(date.getFullYear(), 5, 1));
                                                    cmpToDate.setValue(new Date(date.getFullYear(), 5 + 1, 0));
                                                    break;
                                                case 11:
                                                    cmpFromDate.setValue(new Date(date.getFullYear(), 6, 1));
                                                    cmpToDate.setValue(new Date(date.getFullYear(), 6 + 1, 0));
                                                    break;
                                                case 12:
                                                    cmpFromDate.setValue(new Date(date.getFullYear(), 7, 1));
                                                    cmpToDate.setValue(new Date(date.getFullYear(), 7 + 1, 0));
                                                    break;
                                                case 13:
                                                    cmpFromDate.setValue(new Date(date.getFullYear(), 8, 1));
                                                    cmpToDate.setValue(new Date(date.getFullYear(), 8 + 1, 0));
                                                    break;
                                                case 14:
                                                    cmpFromDate.setValue(new Date(date.getFullYear(), 9, 1));
                                                    cmpToDate.setValue(new Date(date.getFullYear(), 9 + 1, 0));
                                                    break;
                                                case 15:
                                                    cmpFromDate.setValue(new Date(date.getFullYear(), 10, 1));
                                                    cmpToDate.setValue(new Date(date.getFullYear(), 10 + 1, 0));
                                                    break;
                                                case 16:
                                                    cmpFromDate.setValue(new Date(date.getFullYear(), 11, 1));
                                                    cmpToDate.setValue(new Date(date.getFullYear(), 11 + 1, 0));
                                                    break;
                                                case 17:
                                                    cmpFromDate.setValue(new Date(date.getFullYear(), 3 * 1 - 3, 1));
                                                    cmpToDate.setValue(new Date(date.getFullYear(), 3 * 1 + 0, 1));
                                                    break;
                                                case 18:
                                                    cmpFromDate.setValue(new Date(date.getFullYear(), 3 * 2 - 3, 1));
                                                    cmpToDate.setValue(new Date(date.getFullYear(), 3 * 2 + 0, 1));
                                                    break;
                                                case 19:
                                                    cmpFromDate.setValue(new Date(date.getFullYear(), 3 * 3 - 3, 1));
                                                    cmpToDate.setValue(new Date(date.getFullYear(), 3 * 3 + 0, 1));
                                                    break;
                                                case 20:
                                                    cmpFromDate.setValue(new Date(date.getFullYear(), 3 * 4 - 3, 1));
                                                    cmpToDate.setValue(new Date(date.getFullYear(), 3 * 4 + 0, 1));
                                                    break;
                                                default:
                                                    cmpFromDate.setValue(new Date(date.getFullYear(), date.getMonth(), 1));
                                                    cmpToDate.setValue(new Date(date.getFullYear(), date.getMonth(), 1));

                                            };
                                             " + handler + @"
                                    };
</script>";
            #endregion
            var search = new SearchContainer();
            search.FromDate = fromdate;
            search.ToDate = toDate;
            search.TimeOption = timeOption;
            if (isSearchByDeparment)
            {
                var department = X.DepartmentFieldSearch(deparmentId, handler);
                search.Department = department;

            }
            search.Layout(LayoutType.VBox)
                           .LayoutConfig(new VBoxLayoutConfig { Align = VBoxAlign.Left })
                           .Items(
                                       Html.X().Button().Icon(Icon.Outline).EnableToggle(true)
                                       .Handler("if(this.next().hidden){ this.next().show(); } else{ this.next().hide(); }"),
                                       Html.X().FormPanel().Hidden(true).Border(false).Frame(true)
                                                        .Width(540).Margin(0).PaddingSpec("2 5 2 5")
                                                       .Layout(LayoutType.VBox)
                                                       .LayoutConfig(new VBoxLayoutConfig { Align = VBoxAlign.Stretch })
                                                       .Items(
                                                                search.Department.Height(24).MarginSpec("0 1 0 0"),
                                                                X.Container().LayoutConfig(new HBoxLayoutConfig { Align = HBoxAlign.Stretch, Pack = BoxPack.Start })
                                                                    .Items(
                                                                        timeOption.MarginSpec("0 5 0 0"),
                                                                        fromdate.MarginSpec("0 5 0 0"),
                                                                        toDate,
                                                                        Html.X().ButtonRefresh().Handler(handler).MarginSpec("0 5 0 5")
                                                                    )
                                                       )
                                   );
            return search;
        }
        private static DepartmentContainer DepartmentFieldSearch(this BuilderFactory X, string valueId, string handler)
        {
            var script = @"<script>
                                var selectDepartment = function(records) {
                                    if (records != null && records.length > 0){
                                        var data = '';
                                        records.forEach(function (obj) {
                                            var item = obj.data.id + ',';
                                            data = data + item;
                                        });
                                        var departmentContainer = App." + valueId + @"Container;
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
            var textfield = X.TextField().ColumnWidth(1)
                            .HtmlBin(c => script)
                            .FieldLabel("Phòng ban").LabelWidth(70)
                            .ItemID("txtDepartmentName")
                            .EmptyText("Lựa chọn phòng ban hoặc đơn vị")
                            .BlankText("Phòng ban không được bỏ trống!")
                            .ReadOnly(true)
                            .RightButtons(button);
            var container = new DepartmentContainer(button, textfield);
            container
                .ID(valueId + "Container")
                .Items(
                    X.HiddenFor(valueId).ItemID("hdfDepartmentID").Listeners(ls => ls.Change.Handler = handler),
                    textfield
                );
            return container;
        }
    }
}