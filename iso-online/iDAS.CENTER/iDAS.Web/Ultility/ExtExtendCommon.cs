using Ext.Net;
using Ext.Net.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using iDAS.Items;

namespace iDAS.Web.ExtExtend
{
    public class DateTimeContainer : Container.Builder
    {
        public DateField.Builder DateField { get; set; }
        public TimeField.Builder TimeField { get; set; }
        public virtual DateTimeContainer FieldLabel(string value)
        {
            this.Layout(LayoutType.Column).MarginSpec("0 -1 0 0");
            DateField.FieldLabel(value);
            return this;
        }
        public virtual DateTimeContainer LabelWidth(int value)
        {
            DateField.LabelWidth(value);
            return this;
        }
        public virtual DateTimeContainer ReadOnly(bool value = false)
        {
            DateField.ReadOnly(value);
            TimeField.ReadOnly(value);
            return this;
        }
        public virtual DateTimeContainer AllowBlank(bool value = true)
        {
            DateField.AllowBlank(value);
            TimeField.AllowBlank(value);
            return this;
        }
    }
    public static class ExtExtendCommon
    {
        public static DateTimeContainer CreateDateTimeContainer<TModel>(BuilderFactory<TModel> X, string datetimeKey, object datetime)
        {
            var container = new DateTimeContainer();
            var guid = Guid.NewGuid().ToString("N");
            var hiddenID = "Hidden" + guid;
            var dateID = "Date" + guid;
            var timeID = "Time" + guid;
            var hidden = X.HiddenFor(datetimeKey).ID(hiddenID).Value(datetime);
            var date = X.DateField().ID(dateID)
                        .ColumnWidth(0.7)
                        .Icon(Icon.Date)
                        .Format("dd/MM/yyyy")
                        .MarginSpec("0 5 0 0")
                        .EmptyText("dd/MM/yyyy")
                        .SubmitValue(false)
                        .Value(datetime)
                        .Listeners(ls => ls.Change.Handler = "App." + hiddenID + ".setValue(App." + dateID + ".rawValue+' '+App." + timeID + ".rawValue)");
            var time = X.TimeField()
                        .ID(timeID)
                        .Icon(Icon.Clock)
                        .HideLabel(true)
                        .ColumnWidth(0.3)
                        .Format("HH:mm:ss")
                        .SubmitValue(false)
                        .Value(datetime)
                        .Listeners(ls => ls.Change.Handler = "App." + hiddenID + ".setValue(App." + dateID + ".rawValue+' '+App." + timeID + ".rawValue)");
            container.DateField = date;
            container.TimeField = time;
            container.Items(a => { a.Add(hidden); a.Add(date); a.Add(time); });
            return container;
        }
        public static DateTimeContainer DateTimeFieldFor<TModel, TProperty>(this BuilderFactory<TModel> X, Expression<Func<TModel, TProperty>> expression)
        {
            var datetime = X.GetValue(expression);
            var datetimeKey = X.GetKey(expression);
            var container = CreateDateTimeContainer(X, datetimeKey, datetime);
            return container;
        }
        public static DateTimeContainer DateTimeFieldFor<TModel>(this BuilderFactory<TModel> X, string datetimeKey)
        {
            var datetime = X.GetValue<TModel, DateTime>(datetimeKey);
            var container = CreateDateTimeContainer(X, datetimeKey, datetime);
            return container;
        }
        public static PagingToolbar.Builder CustomPagingToolbar(this BuilderFactory X, params string[] items)
        {
            return
                X.PagingToolbar()
                    .EmptyMsg("Hiện không có dữ liệu")
                    .NextText("Trang kế tiếp")
                    .PrevText("Trang trước")
                    .LastText("Trang cuối cùng")
                    .FirstText("Trang đầu tiên")
                    .DisplayMsg("Hiển thị từ {0}-{1} của {2} bản ghi")
                    .BeforePageText("Trang")
                    .AfterPageText("của {0}")
                    .RefreshText("Tải lại dữ liệu")
                    .Items(
                        X.Label("Số bản ghi:"),
                        X.ToolbarSpacer(10),
                        X.ComboBox()
                            .Width(50)
                            .Items(items)
                            .SelectedItems(items[0])
                            .Listeners(l => { l.Select.Fn = "onComboBoxSelect"; })
                    )
                    .Plugins(Html.X().ProgressBarPager());
        }
        public static Column.Builder ImageColumn(this BuilderFactory X)
        {
            return X.Column().Renderer("AvatarRenderer").Width(31).Text("").Align(Alignment.Center);
        }
        public static FormPanel.Builder CustomFormPanel(this BuilderFactory X)
        {
            return X.FormPanel()
                    .Header(false)
                    .AutoScroll(true)
                    .Layout(LayoutType.VBox)
                    .LayoutConfig(new VBoxLayoutConfig { Align = VBoxAlign.Stretch })
                    .Frame(true)
                    .FieldDefaults(df => { df.LabelWidth = 120; })
                    .PaddingSpec("5 10 0 10")
                    .StyleSpec("border: none;");
        }
        public static Button.Builder ButtonAdd(this BuilderFactory X)
        {
            return X.Button()
            .ItemID("btnAdd")
            .Icon(Icon.Add)
            .Text("Thêm");
        }
        public static Button.Builder ButtonEdit(this BuilderFactory X)
        {
            return X.Button()
            .ItemID("btnEdit")
            .Icon(Icon.Pencil)
            .Text("Sửa")
            .Disabled(true);
        }
        public static Button.Builder ButtonDelete(this BuilderFactory X)
        {
            return X.Button()
            .ItemID("btnDelete")
            .Icon(Icon.Delete)
            .Text("Xóa")
            .Disabled(true);
        }
        public static Button.Builder ButtonView(this BuilderFactory X)
        {
            return X.Button()
            .ItemID("btnView")
            .Icon(Icon.Zoom)
            .Text("Xem")
            .Disabled(true);
        }
        public static Button.Builder ButtonSend(this BuilderFactory X)
        {
            return X.Button()
            .ItemID("btnSend")
            .Icon(Icon.Mail)
            .Text("Gửi duyệt")
            .Disabled(true);
        }
        public static Button.Builder ButtonApproval(this BuilderFactory X)
        {
            return X.Button()
            .ItemID("btnApproval")
            .Icon(Icon.Tick)
            .Text("Phê duyệt")
            .Disabled(true);
        }
        public static Button.Builder ButtonRestore(this BuilderFactory X)
        {
            return X.Button()
            .ItemID("btnRestore")
            .Icon(Icon.ArrowRotateClockwise)
            .Text("Phục hồi")
            .Disabled(true);
        }
        public static Button.Builder ButtonRefresh(this BuilderFactory X)
        {
            return X.Button()
            .ItemID("btnRefresh")
            .Icon(Icon.ArrowRefresh)
            .Text("Tải lại");
        }
        public static Button.Builder ButtonSave(this BuilderFactory X)
        {
            return X.Button()
            .ItemID("btnSave")
            .Icon(Icon.DatabaseSave)
            .DirectEvents(de =>
             {
                 de.Click.EventMask.ShowMask = true;
                 de.Click.EventMask.Msg = "Đang tải dữ liệu";
             })
            .Text("Lưu lại");
        }
        public static Button.Builder ButtonSaveAndExit(this BuilderFactory X)
        {
            return X.Button()
            .ItemID("btnSaveAndExits")
            .Icon(Icon.DatabaseGo)
            .DirectEvents(de =>
            {
                de.Click.EventMask.ShowMask = true;
                de.Click.EventMask.Msg = "Đang tải dữ liệu";
                de.Click.ExtraParams.Add(new { exit = true });
            })
            .Text("Lưu lại và thoát");
        }
        public static Button.Builder ButtonSendWindow(this BuilderFactory X)
        {
            return X.Button()
            .ItemID("btnSendWindow")
            .Icon(Icon.Mail)
            .DirectEvents(de =>
            {
                de.Click.EventMask.ShowMask = true;
                de.Click.EventMask.Msg = "Đang tải dữ liệu";
            })
            .Text("Gửi duyệt");
        }
        public static Button.Builder ButtonChoice(this BuilderFactory X)
        {
            return X.Button()
            .ItemID("btnAccept")
            .Icon(Icon.Accept)
            .DirectEvents(de =>
            {
                de.Click.EventMask.ShowMask = true;
                de.Click.EventMask.Msg = "Đang tải dữ liệu";
            })
            .Text("Chọn");
         
        }
        public static Button.Builder ButtonExit(this BuilderFactory X)
        {
            return X.Button()
            .ItemID("btnExit")
            .Icon(Icon.Cancel)
            .Text("Thoát")
            .Handler("this.up('window').close();");
        }
        public static Button.Builder ButtonReset(this BuilderFactory X)
        {
            return X.Button()
                    .ItemID("btnReset")
                    .Icon(Icon.ArrowRefresh)
                    .Text("Nhập lại");
        }
        /// <summary>
        /// Get value of Model
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="X"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static TProperty GetValue<TModel, TProperty>(this BuilderFactory<TModel> X, Expression<Func<TModel, TProperty>> expression)
        {
            var arrKey = expression.Body.ToString().Split('.');
            TProperty result = Activator.CreateInstance<TProperty>();
            Object obj = X.HtmlHelper.ViewData.Model;
            if (obj != null && arrKey.Count() > 1)
            {
                for (var i = 1; i < arrKey.Count(); i++)
                {
                    obj = obj.GetType().GetProperty(arrKey[i]).GetValue(obj);
                }
            }
            if (obj != null)
                result = (TProperty)obj;
            return result;
        }
        /// <summary>
        /// Get value of Model
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="X"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static TProperty GetValue<TModel, TProperty>(this BuilderFactory<TModel> X, string propertyKey)
        {
            var arrKey = propertyKey.ToString().Split('.');
            TProperty result = Activator.CreateInstance<TProperty>();
            Object obj = X.HtmlHelper.ViewData.Model;
            if (obj != null && arrKey.Count() > 0)
            {
                for (var i = 0; i < arrKey.Count(); i++)
                {
                    obj = obj.GetType().GetProperty(arrKey[i]).GetValue(obj);
                }
            }
            if (obj != null)
                result = (TProperty)obj;
            return result;
        }
        /// <summary>
        /// Get key of Model
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="X"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetKey<TModel, TProperty>(this BuilderFactory<TModel> X, Expression<Func<TModel, TProperty>> expression)
        {
            var arrKey = expression.Body.ToString().Split('.');
            string result = string.Empty;
            if (arrKey.Count() > 1)
            {
                for (var i = 1; i < arrKey.Count(); i++)
                {
                    result += arrKey[i];
                    result += ".";
                }
            }
            return result;
        }
    }
}