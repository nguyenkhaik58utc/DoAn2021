using Ext.Net;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace iDAS.Web.ExtExtend
{
    public class SearchDateContainer : Toolbar.Builder
    {
        public ComboBox.Builder TimeOption { get; set; }
        public DateField.Builder FormDate { get; set; }
        public DateField.Builder ToDate { get; set; }

        public SearchDateContainer()
        {
            this.Border(false);
        }
        public virtual SearchDateContainer FromDateID(string value = "")
        {
            if (!String.IsNullOrWhiteSpace(value))
            {
                FormDate.ID(value);
            }
            return this;
        }
        public virtual SearchDateContainer ToDateID(string value = "")
        {
            if (!String.IsNullOrWhiteSpace(value))
            {
                ToDate.ID(value); 
                TimeOption.Select(3);
            }
            return this;
        }
       
    }
    public static class ExtExtendSearchDate
    {     
        /// <summary>
        /// controler tìm kiếm theo ngày bắt đâu ngày kết thúc
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="X"></param>
        /// <param name="handler">Hàm sự kiện khi thay đổi controler tìm kiếm</param>
        /// <returns></returns>
        public static SearchDateContainer SearchByDate<TModel>(this BuilderFactory<TModel> X,string handler)
        {
            var timeOption = Html.X().ComboBox().ItemID("cbxOptionId").LabelWidth(50).Width(150).ID("CbxoptID")
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
            var startDate = Html.X().DateField().ItemID("fromDateId")
                                    .FieldLabel("Từ")
                                    .ID("dfFromDate")
                                    .Icon(Icon.DateNext)
                                    .LabelWidth(30)
                                    .SelectedDate(DateTime.Now)
                                    .Width(150)
                                    .Listeners(l => l.Change.Handler = "if(this.hasFocus && this.isValid()){" + handler + "};")
                                    .Format("dd/MM/yyyy");
            var endDate = Html.X().DateField().ItemID("toDateId")
                                    .FieldLabel("Đến")
                                    .LabelWidth(30)
                                    .Width(150)
                                    .Icon(Icon.DatePrevious)
                                    .SelectedDate(DateTime.Now)
                                    .ID("dfToDate")
                                    .Listeners(l => l.Change.Handler = "if(this.hasFocus && this.isValid()){" + handler + "};")
                                    .Format("dd/MM/yyyy");
            var result = new SearchDateContainer();
            result.TimeOption = timeOption;
            result.FormDate = startDate;
            result.ToDate = endDate;
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
                                             " + handler+@"
                                    };
</script>";
            result.HtmlBin(c => script)
                    .Items(
                            timeOption.MarginSpec("0 5 0 0"),
                            startDate.MarginSpec("0 5 0 0").PaddingSpec("20 20 20 20"),
                            endDate,
                            Html.X().ButtonRefresh().Handler(handler)
                    );
            return result;
        }
    }
}