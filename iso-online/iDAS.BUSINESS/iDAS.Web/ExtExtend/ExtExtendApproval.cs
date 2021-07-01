using Ext.Net;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace iDAS.Web.ExtExtend
{
    public class ApprovalWindow : Window.Builder
    {
        public FormPanel.Builder Form { get; set; }
        public Container.Builder Container { get; set; }
        public Button.Builder Button { get; set; }
        public ApprovalWindow()
        {
            this.Header(false)
                .Height(1)
                .Maximized(true)
                .Constrain(true)
                .Layout(LayoutType.Fit)
                .Modal(true)
                .Resizable(false)
                .BodyPadding(0)
                .Border(false);
        }
        public virtual ApprovalWindow UrlSubmit(string url)
        {
            Form.Url(url).Method(HttpMethod.POST);
            return this;
        }
        public virtual ApprovalWindow ItemsExtend(params AbstractComponent[] items)
        {
            Container.Items(items);
            return this;
        }
        public virtual ApprovalWindow ScriptSubmit(string value)
        {
            var approvalScript = @" var window = this.up('window');
                                var form = window.down('form');
                                form.submit({
                                    success: function (form, action) {
                                        window.close();
                                        {0} 
                                    }
                                });";
            approvalScript = approvalScript.Replace("{0}", value);
            Button.Handler(approvalScript);
            return this;
        }
    }

    public class AppovalFormPanel : FormPanel.Builder
    {
        public EmployeeFieldSet Employee { get; set; }
        public DateTimeContainer Datetime { get; set; }
        public ComboBox.Builder Result { get; set; }
        public Checkbox.Builder Edit { get; set; }
        public TextArea.Builder Note { get; set; }
        public AppovalFormPanel()
        {
            this.Title("Thông tin phê duyệt").TitleAlign(Ext.Net.TitleAlign.Center)
            .Region(Ext.Net.Region.East)
            .Layout(LayoutType.VBox)
            .LayoutConfig(new VBoxLayoutConfig { Align = VBoxAlign.Stretch })
            .AutoScroll(true)
            .Collapsible(true)
            .Split(true)
            .Frame(true)
            .StyleSpec("-webkit-border-radius: 0; border-radius: 0;")
            .Width(350)
            .Margin(1)
            .FieldDefaults(fs => fs.LabelWidth = 80);
        }
        public virtual AppovalFormPanel ReadOnly(bool value = false)
        {
            if (value)
            {
                Employee.ReadOnly(value);
                Datetime.ReadOnly(value);
                Result.ReadOnly(value);
                Edit.ReadOnly(value);
                Note.ReadOnly(value);
            }
            return this;
        }
        public virtual AppovalFormPanel AllowBlank(bool value = true)
        {
            Employee.AllowBlank(value);
            return this;
        }
        public virtual AppovalFormPanel CallBack(string fnScript = "")
        {
            if (fnScript != "")
            {
                Employee.CallBack(fnScript);
            }
            return this;
        }
    }

    public static class ExtExtendApproval
    {
        public static AppovalFormPanel ApprovalPanelFor<TModel, TProperty>(this BuilderFactory<TModel> X, Expression<Func<TModel, TProperty>> expression)
            where TProperty : ApprovalItem
        {
            var approvalKey = X.GetKey(expression);
            var approval = X.GetValue(expression);
            var employee = X.EmployeeFieldFor(approvalKey + "Approval").ReadOnly(!approval.IsEdit);
            var datetime = X.DateTimeFieldFor(approvalKey + "ApprovalAt").FieldLabel("Thời gian").AllowBlank(approval.IsEdit).ReadOnly(approval.IsEdit);
            var result = X.ComboBoxFor(approvalKey + "IsResult")
                            .EmptyText("Kết quả phê duyệt")
                            .FieldLabel("Kết quả")
                            .BlankText("Kết quả phê duyệt bắt buộc chọn!")
                            .AllowBlank(approval.IsEdit)
                            .ReadOnly(approval.IsEdit)
                            .Editable(false)
                            .Listeners(ls =>
                            {
                                ls.Select.Handler =
                                    @"if(this.value == false)
                                    { this.next().setReadOnly(false); this.next().setValue(true); }
                                    else{ this.next().setReadOnly(true); this.next().setValue(false);};";
                                ls.AfterRender.Handler = "if(this.value == true) {this.next().setReadOnly(true);}";
                            })
                            .Items(new ListItem("Đồng ý", true), new ListItem("Không đồng ý", false));
            var edit = X.CheckboxFor(approvalKey + "IsEdit").FieldLabel("Cho sửa đổi").ReadOnly(approval.IsEdit);
            var note = X.TextAreaFor(approvalKey + "ApprovalNote").HideLabel(true).ReadOnly(approval.IsEdit).Padding(1);
            var panel = new AppovalFormPanel();
            panel.Employee = employee;
            panel.Datetime = datetime;
            panel.Result = result;
            panel.Edit = edit;
            panel.Note = note;
            bool isAdd = false;
            if (!approval.IsAccept && approval.IsEdit && !approval.IsApproval)
                isAdd = true;
            panel.Items(
                employee.Title("Người phê duyệt"),
                datetime.Height(26).Hidden(isAdd),
                result.Hidden(isAdd),
                edit.Hidden(isAdd),
                X.FieldSet().Title("Nhận xét").Hidden(isAdd)
                    .Layout(LayoutType.Fit).Flex(1)
                    .MinHeight(100)
                    .StyleSpec("border:none; border-top: 1px solid #b5b8c8;").Padding(0).Margin(0)
                    .Items(
                        note
                    )
            );
            return panel;
        }

        public static ApprovalWindow ApprovalSendWindow<TModel>(this BuilderFactory<TModel> X, bool notSave = false, int employeeId = 0)
            where TModel : ApprovalItem
        {
            var sendScript = @" var window = this.up('window');
                                var form = window.down('form');
                                form.submit({
                                    params: {
                                        IsSendApproval : true,
                                    },
                                    success: function (form, action) {
                                        window.close(); 
                                    },
                                });";
            var approvalScript = @" var window = this.up('window');
                                var form = window.down('form');
                                form.submit({
                                    params: {
                                        IsEdit : false,
                                        IsApproval: true,
                                        IsAccept: true,
                                    },
                                    success: function (form, action) {
                                        window.close(); 
                                    }
                                });";
            var saveScript = @" var window = this.up('window');
                                var form = window.down('form');
                                form.submit({
                                    success: function (form, action) {
                                        window.close(); 
                                    },
                                });";
            var form = X.FormPanel().Layout(LayoutType.Border);
            var container = X.Container().Layout(LayoutType.Fit).Region(Region.Center).Margin(1);
            var window = new ApprovalWindow();
            window.Form = form;
            window.Container = container;
            var approvalpanel = X.ApprovalPanelFor(m => m).AllowBlank(false)
                //.CallBack("CheckSendApproval")
                ;
            if (employeeId != 0)
            {
                string checkEmployeeScript = @"<script>
                                    var CheckSendApproval= function(recorditem)
                                                                    { 
                                                                        if(recorditem.get('ID') ==" + employeeId + @")
                                                                            {
                                                                                App.btnSendApprovalExtend.hide(); 
                                                                                App.btnApprovalExtend.show();
                                                                            }
                                                                        else
                                                                            { 
                                                                                App.btnSendApprovalExtend.show(); 
                                                                                App.btnApprovalExtend.hide();
                                                                            }
                                                                    };
                                </script>";
                approvalpanel.HtmlBin(c => checkEmployeeScript);
            }
            window.Items(
                form.Items(
                    container,
                    approvalpanel
                )
            )
            .Buttons(
                X.Button().Icon(Icon.Mail).ID("btnSendApprovalExtend").Text("Gửi duyệt").Handler(sendScript),
                X.Button().Icon(Icon.PageEdit).ID("btnApprovalExtend").Text("Phê duyệt").Handler(approvalScript).Hidden(true),
                X.ButtonSave().Handler(saveScript).Hidden(notSave),
                X.ButtonExit()
            );
            return window;
        }

        public static ApprovalWindow ApprovalWindow<TModel>(this BuilderFactory<TModel> X)
            where TModel : ApprovalItem
        {
            var approvalScript = @" var window = this.up('window');
                                var form = window.down('form');
                                form.submit({
                                    success: function (form, action) {
                                        window.close();
                                    },
                                });";
            var button = X.Button().Icon(Icon.PageEdit).Text("Phê duyệt").Handler(approvalScript);
            var form = X.FormPanel().Layout(LayoutType.Border);
            var container = X.Container().Layout(LayoutType.Fit).Region(Region.Center).Margin(1);
            var window = new ApprovalWindow();
            window.Form = form;
            window.Container = container;
            window.Button = button;
            window.Items(
                form.Items(
                    container,
                    X.ApprovalPanelFor(m => m).AllowBlank(true)
                )
            )
            .Buttons(
                button,
                X.ButtonExit()
            );
            return window;
        }

        public static ApprovalWindow ApprovalDetailWindow<TModel>(this BuilderFactory<TModel> X)
           where TModel : ApprovalItem
        {
            var form = X.FormPanel().Layout(LayoutType.Border);
            var container = X.Container().Layout(LayoutType.Fit).Region(Region.Center).Margin(1);
            var window = new ApprovalWindow();
            window.Form = form;
            window.Container = container;
            window.Items(
                form.Items(
                    container,
                    X.ApprovalPanelFor(m => m).ReadOnly(true)
                )
            )
            .Buttons(
              X.ButtonExit()
            );
            return window;
        }
    }
}