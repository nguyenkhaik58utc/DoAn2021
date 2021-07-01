using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Items;
using iDAS.Services;
using iDAS.Base;
using iDAS.Web.Controllers;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Service.Controllers
{
    [BaseSystem(Name = "Lệnh cung cấp dịch vụ", Icon = "RubyAdd", IsActive = true, IsShow = true, Position = 3)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class CommandController : BaseController
    {
        //
        // GET: /Service/Command/
        private ServiceCommandServiceSV commandServiceSV = new ServiceCommandServiceSV();
        private ServiceContractCommandSV contractCommandService = new ServiceContractCommandSV();
        private ServiceCommandService commandService = new ServiceCommandService();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index(int focusId = 0)
        {
            ViewBag.FocusId = focusId;
            return View();
        }
        public ActionResult ShowSendApproval(int id)
        {
            var obj = commandService.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "SendApproval", Model = obj };
        }
        public ActionResult GetData(StoreRequestParameters parameters, int focusId = 0, int serviceId=0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            List<ServiceCommandItem> lstData;
            lstData = commandService.GetAll(filter, focusId, serviceId);
            return this.Store(lstData, filter.PageTotal);
        }
        public ActionResult GetDataIsApproval(StoreRequestParameters parameters)
        {
            List<ServiceCommandItem> lstData;
            int total;
            lstData = commandService.GetAllIsApproval(parameters.Page, parameters.Limit, out total);
            return this.Store(new Paging<ServiceCommandItem>(lstData, total));
        }
        public ActionResult ApproveCommand(ServiceCommandItem obj)
        {
            try
            {
                commandService.Approve(obj, User.ID);
                List<int> lstEmployeesID = new List<int>();
                if (obj.UpdateByEmployeeID.HasValue && obj.UpdateByEmployeeID != obj.ApprovalBy)
                {
                    lstEmployeesID.Add(obj.UpdateByEmployeeID.Value);
                }
                if (obj.CreateByEmployeeID.HasValue && obj.CreateByEmployeeID != obj.ApprovalBy)
                {
                    lstEmployeesID.Add(obj.CreateByEmployeeID.Value);
                }
                NotifyController notify = new NotifyController();
                notify.Notify(this.ModuleCode, "Có một lệnh cung cấp dịch vụ được phê duyệt", obj.Name, lstEmployeesID, User, Common.CommandService, "CommandServiceID:" + obj.ID.ToString());
                if (obj.IsAccept && obj.IsApproval)
                {
                    notify.Notify(this.ModuleCode, "Bạn có một lệnh cung cấp dịch vụ yêu cầu kiểm soát và thực hiện", obj.Name, obj.Perform.ID, User, Common.CommandService, "CommandServiceID:" + obj.ID.ToString());
                }
                X.GetCmp<Store>("stMnCommand").Reload();
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw ex;
            }
            return this.Direct();
        }
        public ActionResult ShowApprove(int id)
        {
            var obj = commandService.GetByID(id);
            if (obj.ApprovalBy != User.EmployeeID)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.WARNING,
                    Title = "Thông báo",
                    Message = "Bạn không có quyền phê duyệt lệnh này!"
                });
            }
            else
            {
                return new Ext.Net.MVC.PartialViewResult() { ViewName = "Approve", Model = obj };
            }
            return this.Direct();
        }
        public ActionResult GetDataContract(StoreRequestParameters parameters, int commandId)
        {
            List<CustomerContractItem> lstData;
            int total;
            lstData = contractCommandService.GetAllForCommand(parameters.Page, parameters.Limit, out total, commandId);
            return this.Store(new Paging<CustomerContractItem>(lstData, total));
        }
        public ActionResult GetDataContractEdit(StoreRequestParameters parameters, int commandId)
        {
            List<CustomerContractItem> lstData;
            int total;
            lstData = contractCommandService.GetAllForCommandEdit(parameters.Page, parameters.Limit, out total, commandId);
            return this.Store(new Paging<CustomerContractItem>(lstData, total));
        }
        public ActionResult GetDataContractShow(StoreRequestParameters parameters, int commandId)
        {
            List<CustomerContractItem> lstData;
            int total;
            lstData = contractCommandService.GetAllForCommandShow(parameters.Page, parameters.Limit, out total, commandId);
            return this.Store(new Paging<CustomerContractItem>(lstData, total));
        }
        [BaseSystem(Name = "Thêm lệnh cung cấp dịch đã duyệt", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmAddApprove()
        {
            var employeeSV = new HumanEmployeeSV();
            ServiceCommandItem obj = new ServiceCommandItem();
            obj.Approval = employeeSV.GetEmployeeView(User.EmployeeID);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "CreateApprove", Model = obj };
        }
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmAdd()
        {
             var employeeSV = new HumanEmployeeSV();
            ServiceCommandItem obj = new ServiceCommandItem();
            obj.Approval = employeeSV.GetEmployeeView(User.EmployeeID);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Create" , Model= obj};
        }
        public ActionResult SendApprove(ServiceCommandItem obj)
        {
            try
            {
                if (obj.Approval.ID == 0)
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.WARNING,
                        Title = "Thông báo",
                        Message = "Bạn phải chọn người phê duyệt lệnh cung cấp dịch vụ!"
                    });
                    return this.Direct();
                }
                else
                {
                    commandService.SendApprove(obj, User.ID);
                    NotifyController notify = new NotifyController();
                    notify.Notify(this.ModuleCode, "Bạn có một lệnh cung cấp dịch vụ yêu cầu phê duyệt", obj.Name, obj.Approval.ID, User, Common.CommandService, "CommandServiceID:" + obj.ID.ToString());
                    X.GetCmp<Store>("stMnCommand").Reload();
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    return this.Direct(true);
                }
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw ex;
            }
        }
        public ActionResult ShowListService(int id)
        {
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "ListService", ViewData = new ViewDataDictionary() { { "commandId", id } } };
        }
        [BaseSystem(Name = "Sửa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowUpdate(int id)
        {
            var obj = commandService.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Edit", Model = obj };
        }
        public ActionResult InsertApprove(ServiceCommandItem objNew, bool val, string strContractID)
        {
            if (!string.IsNullOrEmpty(strContractID))
            {
                try
                {
                    if (objNew.Approval.ID == 0 && !objNew.IsEdit)
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.WARNING,
                            Title = "Thông báo",
                            Message = "Bạn phải chọn người phê duyệt lệnh cung cấp dịch vụ!"
                        });
                        return this.Direct();
                    }
                    else if (!objNew.Name.Trim().Equals("") && !commandService.CheckNameExits(objNew.Name.Trim()))
                    {
                          int commandID =  commandService.InsertCommandApproveService(objNew, strContractID, User.ID);
                         NotifyController notify = new NotifyController();
                         notify.Notify(this.ModuleCode, "Bạn có một lệnh cung cấp dịch vụ yêu cầu kiểm soát và thực hiện", objNew.Name, objNew.Perform.ID, User, Common.CommandService, "CommandServiceID:" + commandID.ToString());
                        if (val == true)
                        {
                            X.GetCmp<Window>("winNewCommandApprove").Close();
                        }
                        X.Msg.Notify("Thông báo", "Bạn đã thêm mới thành công!").Show();
                        X.GetCmp<Store>("stMnCommand").Reload();
                        X.GetCmp<GridPanel>("grMnCommand").Refresh();
                    }
                    else
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.ERROR,
                            Title = "Thông báo",
                            Message = "Tên lệnh cung cấp dịch vụ đã tồn tại vui lòng nhập tên khác!"
                        });
                        return this.Direct("ErrorExited");
                    }
                    return this.Direct();
                }
                catch (Exception ex)
                {
                    return this.Direct("Error");
                }
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.WARNING,
                    Title = "Thông báo",
                    Message = "Bạn phải chọn ít nhất một hợp đồng để thực hiện tạo lệnh!"
                });
                return this.Direct();
            }
        }
        public ActionResult Insert(ServiceCommandItem objNew, bool val, string strContractID)
        {
            if (!string.IsNullOrEmpty(strContractID))
            {
                try
                {
                    if (objNew.Approval.ID == 0 && !objNew.IsEdit)
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.WARNING,
                            Title = "Thông báo",
                            Message = "Bạn phải chọn người phê duyệt lệnh cung cấp dịch vụ!"
                        });
                        return this.Direct();
                    }
                    else if (!objNew.Name.Trim().Equals("") && !commandService.CheckNameExits(objNew.Name.Trim()))
                    {
                        int commandId = commandService.InsertCommandService(objNew, strContractID, User.ID);
                        if (val == true)
                        {
                            X.GetCmp<Window>("winNewCommand").Close();
                        }
                        if (!objNew.IsEdit)
                        {
                            NotifyController notify = new NotifyController();
                            notify.Notify(this.ModuleCode, "Yêu cầu phê duyệt lệnh cung cấp dịch vụ", objNew.Name, objNew.Approval.ID, User, Common.CommandService, "CommandID:" + commandId.ToString());
                        }
                        X.Msg.Notify("Thông báo", "Bạn đã thêm mới thành công!").Show();
                        X.GetCmp<Store>("stMnCommand").Reload();
                        X.GetCmp<GridPanel>("grMnCommand").Refresh();
                    }
                    else
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.ERROR,
                            Title = "Thông báo",
                            Message = "Tên lệnh cung cấp dịch vụ đã tồn tại vui lòng nhập tên khác!"
                        });
                        return this.Direct("ErrorExited");
                    }
                    return this.Direct();
                }
                catch (Exception ex)
                {
                    return this.Direct("Error");
                }
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.WARNING,
                    Title = "Thông báo",
                    Message = "Bạn phải chọn ít nhất một hợp đồng để thực hiện tạo lệnh!"
                });
                return this.Direct();
            }
        }
        public ActionResult Update(ServiceCommandItem objEdit, string strContractID)
        {
            if (!string.IsNullOrEmpty(strContractID))
            {
                try
                {
                    if (objEdit.Approval.ID == 0 && !objEdit.IsEdit)
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.WARNING,
                            Title = "Thông báo",
                            Message = "Bạn phải chọn người phê duyệt lệnh cung cấp dịch vụ!"
                        });
                        return this.Direct();
                    }
                    else
                    {
                        commandService.Update(objEdit, strContractID, User.ID);
                        if (!objEdit.IsEdit)
                        {
                            NotifyController notify = new NotifyController();
                            notify.Notify(this.ModuleCode, "Yêu cầu phê duyệt lệnh cung cấp dịch vụ", objEdit.Name, objEdit.Approval.ID, User, Common.CommandService, "CommandID:" + objEdit.ID.ToString());
                        }
                        X.Msg.Notify("Thông báo", "Bạn đã cập nhật thành công!").Show();
                        X.GetCmp<Store>("stMnCommand").Reload();
                        X.GetCmp<Window>("winEditCommand").Close();
                        return this.Direct();
                    }
                }
                catch (Exception ex)
                {
                    return this.Direct("Error");
                }
            }
            else
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.WARNING,
                    Title = "Thông báo",
                    Message = "Bạn phải chọn ít nhất một hợp đồng để thực hiện tạo lệnh!"
                });
                return this.Direct();
            }
        }
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(string stringId)
        {
            try
            {
                commandService.Delete(stringId);
                X.GetCmp<Store>("stMnCommand").Reload();
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                return this.Direct();
            }
            catch (Exception ex)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "Thông báo",
                    Message = "Lệnh cung cấp đã được sử dụng không được xóa!"
                });
                return this.Direct("Error");
            }
        }
        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowDetail(int id)
        {
            var obj = commandService.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Detail", Model = obj };
        }
    }
}
