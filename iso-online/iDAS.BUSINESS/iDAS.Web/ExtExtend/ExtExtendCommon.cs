using Ext.Net;
using Ext.Net.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using iDAS.Items;
using iDAS.Utilities;
using iDAS.Services;

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
        public static DateTimeContainer CreateDateTimeContainer<TModel>(BuilderFactory<TModel> X, string datetimeKey, object datetime, bool checkclendar, int departmentId, string cls, Nullable<DateTime> maxValue = null, bool isDefault = true)
        {
            if ((datetime == null || datetime.ToString().Contains("01/01/0001")) && isDefault)
            {
                datetime = DateTime.Now;
            }
            var container = new DateTimeContainer();
            var guid = Guid.NewGuid().ToString("N");
            var hiddenID = "Hidden" + guid;
            var dateID = "Date" + guid;
            var timeID = "Time" + guid;
            var hidden = X.HiddenFor(datetimeKey).ID(hiddenID).Value(datetime);
            var date = X.DateField().ID(dateID)
                        .ColumnWidth(0.7)
                        .Icon(Icon.Date)
                        .FieldCls(cls)
                        .Format("dd/MM/yyyy")
                        .MarginSpec("0 5 0 0")
                        .EmptyText("dd/MM/yyyy")
                        .SubmitValue(false)
                        .Value(datetime)
                        .MaxDate(maxValue.HasValue ? maxValue.Value : new DateTime())
                        .Listeners(ls => ls.Change.Handler = "App." + hiddenID + ".setValue(App." + dateID + ".rawValue+' '+App." + timeID + ".rawValue)");

            if (checkclendar)
            {
                date.DisabledDates(m => m.AddRange(new CalendarOverrideSV().GetDateNotWorking(departmentId)));
                date.DisabledDatesText("Ngày nghỉ");
                date.DisabledDaysText("Ngày nghỉ trong tuần");
            }
            var time = X.TimeField()
                        .ID(timeID)
                        .Icon(Icon.Clock)
                        .HideLabel(true)
                        .FieldCls(cls)
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

        public static DateTimeContainer DateTimeFieldFor<TModel, TProperty>(this BuilderFactory<TModel> X, Expression<Func<TModel, TProperty>> expression, Boolean checkclendar = false, int departmentId = 0, string cls = "", Nullable<DateTime> maxValue = null, bool isDefault = true)
        {
            var datetime = X.GetValue(expression);
            var datetimeKey = X.GetKey(expression);
            var container = CreateDateTimeContainer(X, datetimeKey, datetime, checkclendar, departmentId, cls, maxValue, isDefault);
            return container;
        }

        public static DateTimeContainer DateTimeFieldFor<TModel>(this BuilderFactory<TModel> X, string datetimeKey, bool checkclendar = false, int departmentId = 0, string cls = "")
        {
            var datetime = X.GetValue<TModel, DateTime>(datetimeKey);
            var container = CreateDateTimeContainer(X, datetimeKey, datetime, checkclendar, departmentId, cls);
            return container;
        }

        public static DateField.Builder DateFieldFormatFor<TModel, TProperty>(this BuilderFactory<TModel> X, Expression<Func<TModel, TProperty>> expression, bool setId = true, Func<object, object> convert = null, string format = null)
        {
            var component = X.DateFieldFor(expression, setId, convert, format).Format("dd/MM/yyyy").InvalidText("Không đúng định dạng!").HideLabel(true);
            return component;
        }

        public static ComboBox.Builder ComboBoxHourFormatFor<TModel, TProperty>(this BuilderFactory<TModel> X, Expression<Func<TModel, TProperty>> expression, bool setId = true, Func<object, object> convert = null, string format = null)
        {
            var HourCombobox = new List<ListItem>();
            HourCombobox.Add(new ListItem() { Text = "00", Value = "00" });
            HourCombobox.Add(new ListItem() { Text = "01", Value = "01" });
            HourCombobox.Add(new ListItem() { Text = "02", Value = "02" });
            HourCombobox.Add(new ListItem() { Text = "03", Value = "03" });
            HourCombobox.Add(new ListItem() { Text = "04", Value = "04" });
            HourCombobox.Add(new ListItem() { Text = "05", Value = "05" });
            HourCombobox.Add(new ListItem() { Text = "06", Value = "06" });
            HourCombobox.Add(new ListItem() { Text = "07", Value = "07" });
            HourCombobox.Add(new ListItem() { Text = "08", Value = "08" });
            HourCombobox.Add(new ListItem() { Text = "09", Value = "09" });
            HourCombobox.Add(new ListItem() { Text = "10", Value = "10" });
            HourCombobox.Add(new ListItem() { Text = "12", Value = "12" });
            HourCombobox.Add(new ListItem() { Text = "13", Value = "13" });
            HourCombobox.Add(new ListItem() { Text = "14", Value = "14" });
            HourCombobox.Add(new ListItem() { Text = "15", Value = "15" });
            HourCombobox.Add(new ListItem() { Text = "16", Value = "16" });
            HourCombobox.Add(new ListItem() { Text = "17", Value = "17" });
            HourCombobox.Add(new ListItem() { Text = "18", Value = "18" });
            HourCombobox.Add(new ListItem() { Text = "19", Value = "19" });
            HourCombobox.Add(new ListItem() { Text = "20", Value = "20" });
            HourCombobox.Add(new ListItem() { Text = "21", Value = "21" });
            HourCombobox.Add(new ListItem() { Text = "22", Value = "22" });
            HourCombobox.Add(new ListItem() { Text = "23", Value = "23" });
            var component = X.ComboBoxFor(expression, setId, convert, format)
                            .Items(HourCombobox)
                            .Editable(false)
                            .Margin(0).HideLabel(true)
                            .DisplayField("Name").ValueField("Id");
            return component;
        }

        public static ComboBox.Builder ComboBoxMiniturFormatFor<TModel, TProperty>(this BuilderFactory<TModel> X, Expression<Func<TModel, TProperty>> expression, bool setId = true, Func<object, object> convert = null, string format = null)
        {
            var MinitusCombobox = new List<ListItem>();
            MinitusCombobox.Add(new ListItem() { Text = "00", Value = "00" });
            MinitusCombobox.Add(new ListItem() { Text = "15", Value = "15" });
            MinitusCombobox.Add(new ListItem() { Text = "30", Value = "30" });
            MinitusCombobox.Add(new ListItem() { Text = "45", Value = "45" });
            var component = X.ComboBoxFor(expression, setId, convert, format)
                            .Items(MinitusCombobox)
                            .Editable(false)
                            .Margin(0).HideLabel(true)
                            .DisplayField("Name").ValueField("Id");
            return component;
        }

        public static ComboBox.Builder ComboBoxFormatFor<TModel, TProperty>(this BuilderFactory<TModel> X, Expression<Func<TModel, TProperty>> expression, bool setId = true, Func<object, object> convert = null, string format = null)
        {
            var component = X.ComboBoxFor(expression, setId, convert, format)
                            .Margin(0).HideLabel(true)
                            .DisplayField("Name").ValueField("Id")
                            .Validator(i => i.Handler = "return this.value != this.rawValue;")
                            .ValidatorText("Giá trị đã nhập không phù hợp!");
            return component;
        }

        public static PagingToolbar.Builder CustomPagingToolbar(this BuilderFactory X, params string[] items)
        {
            if (items.Length == 0)
            {
                items = new string[] { "20", "40", "60", "80", "100" };
            }
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

        public static DateColumn.Builder DateColumnExtend(this BuilderFactory X, string format = DateFormat.Date)
        {
            var dateColumn = X.DateColumn();
            string script = "";
            switch (format)
            {
                case DateFormat.Date: script = DateFormatFunction.DateFunction; break;
                case DateFormat.DateTime: script = DateFormatFunction.DateTimeFunction; break;
                case DateFormat.StringDateTime: script = DateFormatFunction.StringDateTimeFunction; break;
            }
            dateColumn.Renderer(script);
            return dateColumn;
        }

        public static ImageCommandColumn.Builder ColumnFileExtend(this BuilderFactory X, string attachmentFileIDs)
        {
            var viewfileColumn = X.ImageCommandColumn();
            viewfileColumn.Width(20);
            viewfileColumn.Align(Alignment.Right);
            viewfileColumn.Commands(
                Html.X().ImageCommand()
                    .CommandName("ViewFile")
                    .Icon(Icon.Attach)
                    .Hidden(true)
                    .HideMode(HideMode.Display)
                    .ToolTip(tt => tt.Text = "Xem file"),
                Html.X().ImageCommand()
                    .CommandName("ViewListFile")
                    .Icon(Icon.Attach)
                    .Hidden(true)
                    .HideMode(HideMode.Display)
                    .ToolTip(tt => tt.Text = "Xem danh sách file")
              )
            .PrepareCommand("prepareCommandViewFile")
            .Listeners(ls =>
            {
                ls.Command.Handler = "ViewFileAttach(command, record.data." + attachmentFileIDs + ");";
            });
            return viewfileColumn;
        }

        public static ImageCommandColumn.Builder ColumnFileDownload(this BuilderFactory X, string attachmentFileIDs)
        {
            var downloadfileColumn = X.ImageCommandColumn();
            downloadfileColumn.Width(20);
            downloadfileColumn.Align(Alignment.Right);
            downloadfileColumn.Commands(
                Html.X().ImageCommand()
                    .CommandName("DownloadFile")
                    .Hidden(true)
                    .Icon(Icon.DiskDownload)
                    .HideMode(HideMode.Display)
                    .ToolTip(tt => tt.Text = "Tải tệp")
            )
            .PrepareCommand(@"var data = record.data." + attachmentFileIDs + "; if ('" + attachmentFileIDs + "'=='ID' || data.length == 1) {command.hidden = false;}")
            .Listeners(ls =>
            {
                ls.Command.Handler = "DownloadFileAttach(command, record.data." + attachmentFileIDs + ");";
            });
            return downloadfileColumn;
        }

        public static Column.Builder ImageColumn(this BuilderFactory X)
        {
            return X.Column().Renderer("AvatarRenderer").Width(31).Text("").Align(Alignment.Center);
        }

        public static FormPanel.Builder CustomFormPanel(this BuilderFactory X)
        {
            return X.FormPanel().ItemID("formId")
                    .Header(false)
                    .AutoScroll(true)
                    .Layout(LayoutType.VBox)
                    .LayoutConfig(new VBoxLayoutConfig { Align = VBoxAlign.Stretch })
                    .Frame(true)
                    .PaddingSpec("5 10 0 10")
                    .StyleSpec("border: none;");
        }

        public static Window.Builder CustomWindow(this BuilderFactory X)
        {
            return X.Window().ItemID("windowId")
                    .BodyPadding(0)
                    .Width(550)
                    .Constrain(true)
                    .Modal(true)
                    .Layout(LayoutType.Fit)
                    .Maximizable(true);
        }

        public static Button.Builder ButtonCreate(this BuilderFactory X)
        {
            return X.Button()
            .ItemID("btnAdd")
            .Icon(Icon.Add)
            .Text("Thêm");
        }

        public static Button.Builder ButtonUpdate(this BuilderFactory X)
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
            .Icon(Icon.ArrowRefreshSmall)
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

        public static Button.Builder ButtonExportExcel(this BuilderFactory X)
        {
            return X.Button()
            .ItemID("btnExpExcel")
            .Icon(Icon.DiskDownload)
            .DirectEvents(de =>
            {
                de.Click.EventMask.ShowMask = true;
                de.Click.EventMask.Msg = "Đang tải dữ liệu";
                //de.Click.ExtraParams.Add(new { isUpload = true });
                //de.Click.ExtraParams.Add(new { arrObject = new JRawValue("getObjectPartial()") });
                //de.Click.Action = "ExportExcel";
            })
            .Text("Xuất Excel");
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
            .Icon(Icon.DoorOut)
            .Text("Thoát")
            .Handler("this.up('window').close();");
        }

        public static Button.Builder ButtonReset(this BuilderFactory X)
        {
            return X.Button()
                    .ItemID("btnReset")
                    .Icon(Icon.ArrowRefreshSmall)
                    .Text("Nhập lại");
        }

        public static Button.Builder ButtonDestroy(this BuilderFactory X)
        {
            return X.Button()
                    .ItemID("btnDestroy")
                    .Icon(Icon.Cancel)
                    .Text("Hủy");
        }

        public static Button.Builder ButtonRecycle(this BuilderFactory X)
        {
            return X.Button()
                    .ItemID("btnRecycle")
                    .Icon(Icon.Bin)
                    .Text("Thùng rác");
        }

        public static Button.Builder ButtonList(this BuilderFactory X)
        {
            return X.Button()
                    .ItemID("btnList")
                    .Icon(Icon.ApplicationViewList)
                    .Text("Danh sách");
        }

        #region Ext Handle Generic

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

        // HungNM. Add a new function for fixing Role error. 20201007.
        public static string GetComponentID(this BuilderFactory X)
        {
            var componentId = "component" + Guid.NewGuid().ToString().Replace("-", "");
            return componentId;
        }

        public static Window.Builder WindowFormat(this BuilderFactory X)
        {
            var loadMask = X.Container().ItemID("loadMask").Border(false).Height(2).Hidden(true)
                            .Html("<div class='load-bar'><div class='bar'></div><div class='bar'></div></div>");
            var loadMaskEmpty = X.Container().ItemID("loadMaskEmpty").Border(false).Height(2).Hidden(false);
            var component = X.Window()
                            .IconCls("x-fa fa-pencil-square-o")
                            .Height(480).Width(800)
                            .BodyPadding(0).Modal(true).Constrain(true)
                            .Maximizable(true)
                            .Closable(false)
                            .Shadow(true)
                            .Layout(LayoutType.VBox)
                            .LayoutConfig(new VBoxLayoutConfig { Align = VBoxAlign.Stretch })
                            .Items(loadMask, loadMaskEmpty);
            return component;
        }

        public static FormPanel.Builder FormPanelVBoxFormat(this BuilderFactory X)
        {
            var btnId = X.GetComponentID();
            var component = X.FormPanel()
                            .BodyPadding(0).Border(false).Flex(1).Header(false)
                            .Defaults(X.Parameter().Name("margins").Value("0 0 0 0").Mode(ParameterMode.Value))
                            .Layout(LayoutType.VBox)
                            .LayoutConfig(new VBoxLayoutConfig { Align = VBoxAlign.Stretch });
            return component;
        }

        public static FieldSet.Builder FieldSetVBox(this BuilderFactory X)
        {
            var component = X.FieldSet()
                            .Padding(2)
                            .Defaults(new Parameter().ToBuilder().Name("margins").Value("0 0 0 0").Mode(ParameterMode.Value))
                            .Layout(LayoutType.VBox)
                            .LayoutConfig(new VBoxLayoutConfig { Align = VBoxAlign.Stretch });
            return component;
        }

        public static Container.Builder ContainerHBox(this BuilderFactory X)
        {
            var component = X.Container()
                            .Defaults(new Parameter().ToBuilder().Name("margins").Value("0 0 0 0").Mode(ParameterMode.Value))
                            .Layout(LayoutType.HBox)
                            .LayoutConfig(new HBoxLayoutConfig { Align = HBoxAlign.Stretch });
            return component;
        }

        public static Container.Builder ContainerHBoxTop(this BuilderFactory X)
        {
            var component = X.Container()
                            .Defaults(new Parameter().ToBuilder().Name("margins").Value("0 0 0 0").Mode(ParameterMode.Value))
                            .Layout(LayoutType.HBox)
                            .LayoutConfig(new HBoxLayoutConfig { Align = HBoxAlign.Top });
            return component;
        }

        public static RowNumbererColumn.Builder RowNumbererColumnFormat(this BuilderFactory X)
        {
            var component = X.RowNumbererColumn().Text("STT").Width(46).StyleSpec("border-left:none !important;").Align(0);
            return component;
        }

        public static Button.Builder FormID(this Button.Builder Button, string formId)
        {
            Button.DirectEvents(de => de.Click.FormID = formId);
            return Button;
        }

        public static FieldSet.Builder FieldSetHBox(this BuilderFactory X)
        {
            var component = X.FieldSet()
                            .Padding(2)
                            .Defaults(new Parameter().ToBuilder().Name("margins").Value("0 0 0 0").Mode(ParameterMode.Value))
                            .Layout(LayoutType.HBox)
                            .LayoutConfig(new HBoxLayoutConfig { Align = HBoxAlign.Stretch });
            return component;
        }

        public static DateColumn.Builder DateColumnFormat(this BuilderFactory X)
        {
            var component = X.DateColumn().Format("dd/MM/yyyy");
            return component;
        }

        // End.
        public static Column.Builder ColumnEncodeFormat(this BuilderFactory X)
        {
            var component = X.Column().Renderer(RendererFormat.HtmlEncode)
                .Width(50)
                .Filterable(false);
            return component;
        }

        #endregion Ext Handle Generic
    }
}