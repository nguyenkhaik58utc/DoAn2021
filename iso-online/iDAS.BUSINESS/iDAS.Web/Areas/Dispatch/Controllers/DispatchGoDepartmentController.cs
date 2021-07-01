using iDAS.Core;
using iDAS.Items;

using iDAS.Services;
using Ext.Net.MVC;
using Ext.Net;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Utilities;
using iDAS.Web.Controllers;


namespace iDAS.Web.Areas.Dispatch.Controllers
{

    [BaseSystem(Name = "Công văn đi của tổ chức", IsActive = true, IsShow = true, Position = 1, Icon = "PackageGo", Parent = DispatchGoMenuGroupController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class DispatchGoDepartmentController : BaseController
    {
        private DispatchGoSV dispatchGoSV = new DispatchGoSV();
        private DispatchCategorySV dispatchCateSV = new DispatchCategorySV();
        private DispatchMethodService dispatchMethodSV = new DispatchMethodService();
        private DispatchSercuritySV dispatchSecuritySV = new DispatchSercuritySV();
        private DispatchUrgencySV dispatchUrgencySV = new DispatchUrgencySV();
        private HumanEmployeeSV employeeService = new HumanEmployeeSV();
        string store = "StDispatchGoDepartment";
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index(int focusId = 0)
        {
            ViewBag.FocusId = focusId;
            ViewData["UserLogOn"] = User.EmployeeID;
            return View();
        }
        [BaseSystem(Name = "Thêm")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmAdd()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Insert" };
        }
        [BaseSystem(Name = "Sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmUpdate(int id = 0)
        {
            var obj = dispatchGoSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = obj };
        }
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(string stringId)
        {
            try
            {
                dispatchGoSV.Delete(stringId);
                X.Msg.Notify("Thông báo", "Bạn đã xóa bản ghi thành công!").Show();
                X.GetCmp<Store>("GrDispatchGoDepartment").Reload();
                return this.Direct();
            }
            catch (Exception ex)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "Thông báo",
                    Message = "Đã xảy ra lỗi trong quá trình xóa dữ liệu!"
                });
                return this.Direct("Error");
            }
        }
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmDetail(int id = 0)
        {
            var obj = dispatchGoSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
        }
        [BaseSystem(Name = "Phê duyệt")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmApprove(int id = 0)
        {
            var obj = dispatchGoSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Approve", Model = obj };
        }
        [BaseSystem(Name = "Chuyển nội bộ")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmMove(int id = 0)
        {
            var obj = dispatchGoSV.GetByID(id);
            obj.StrType = DispatchObjectType.Department.ToString();
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Move", Model = obj };
        }
        [BaseSystem(Name = "Chuyển bên ngoài")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmMoveOut(int id = 0)
        {
            var obj = dispatchGoSV.GetObjMoveOutByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "MoveOut", Model = obj };
        }
        [BaseSystem(Name = "Xác nhận")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmVerify(int id = 0)
        {
            var obj = dispatchGoSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Verify", Model = obj };
        }
        //Hiện thị frm cập nhật TT Người nhận CV đi bên ngoài tổ chức
        public ActionResult showFrmChoseObject(int id, string type = "", bool isEdit = false)
        {
            if (id > 0)
            {
                DispatchGoObjectItem obj = new DispatchGoObjectItem { DispatchGo = id };
                if (obj == null) obj = new DispatchGoObjectItem { DispatchGo = id };
                string title = "Thêm mới thông tin chuyển công văn đi ra bên ngoài";
                Icon icon = Ext.Net.Icon.Add;
                bool isBtEdit = false;
                if (isEdit)
                {
                    title = "Cập nhật thông tin chuyển công văn đi ra bên ngoài";
                    icon = Ext.Net.Icon.Pencil;
                    isBtEdit = true;
                    obj = dispatchGoSV.GetMoveOutByID(id);
                }
                if (type.Equals(DispatchObjectType.Department.ToString()))
                {
                    return new Ext.Net.MVC.PartialViewResult { ViewName = "MoveOutDetailDept", Model = obj, ViewData = new ViewDataDictionary { { "Title", title }, { "Icon", icon }, { "IsEdit", isBtEdit } } };
                }
                else if (type.Equals(DispatchObjectType.Employee.ToString()))
                {
                    return new Ext.Net.MVC.PartialViewResult { ViewName = "MoveOutDetailEmployee", Model = obj, ViewData = new ViewDataDictionary { { "Title", title }, { "Icon", icon }, { "IsEdit", isBtEdit } } };

                }

            }

            return null;

        }
        //Hiển thị frm Xác nhận CV đi chi tiết theo ID thuộc bảng DispatchGoOut
        public ActionResult showFrmVerifyOut(int id, int type)
        {
            if (id > 0)
            {
                //var obj = dispatchGoSV.GetMoveOutByID(id);
                var obj = dispatchGoSV.GetMoveOutByID(id);
                if (type == (int)DispatchObjectType.Department)
                {
                    return new Ext.Net.MVC.PartialViewResult { ViewName = "VerifyOutDepartmentDetail", Model = obj };
                }
                else if (type == (int)DispatchObjectType.Employee)
                {
                    return new Ext.Net.MVC.PartialViewResult { ViewName = "VerifyOutEmployeeDetail", Model = obj };
                }
            }

            return null;
        }
        //Hiện thị frm Xem chi tiết chuyển - nhận công văn
        public ActionResult showFrmVerifyDetail(int id, int type, int dispatchtype)
        {
            if (id > 0)
            {
                var obj = dispatchGoSV.GetVerifyDetailByID(id, dispatchtype, type);
                if (dispatchtype == (int)DispatchGoType.OutSide && type == (int)DispatchObjectType.Department)
                {
                    return new Ext.Net.MVC.PartialViewResult { ViewName = "DetailVerifyOutDepartment", Model = obj };
                }
                else if (dispatchtype == (int)DispatchGoType.OutSide && type == (int)DispatchObjectType.Employee)
                {
                    return new Ext.Net.MVC.PartialViewResult { ViewName = "DetailVerifyOutEmployee", Model = obj };
                }
                else if (dispatchtype == (int)DispatchGoType.InSide)
                {
                    return new Ext.Net.MVC.PartialViewResult { ViewName = "DetailVerify", Model = obj };
                }
            }

            return null;

        }
        public ActionResult CheckAttachmentExist(string oriName)
        {
            string filePath = Common.DispatchUpload + oriName;
            string path = Server.MapPath(filePath);
            bool blnCheckName = FileOperations.CheckFileExist(path);
            if (blnCheckName == true)
            {
                return this.Direct("true");
            }
            else
            {
                return this.Direct("false");
            }
        }
        [ValidateInput(false)]
        public ActionResult Insert(DispatchGoItem obj, [Bind(Prefix = "StorageFormID")]string[] borders, bool isEdit)
        {
            string ck = chekValid(obj);
            if (ck.Equals(""))
            {
                setDocIssForm(borders, ref obj);
                obj.IsEdit = isEdit;
                obj.Type = true;
                int id = insert(obj);
                if (id > 0)
                {
                    if (!isEdit)
                    {
                        NotifyController notify = new NotifyController();
                        notify.Notify(this.ModuleCode, "Có một công văn đi của tổ chức cần phê duyệt", obj.Name, obj.EmployeeInfo.ID, User, Common.DispatchGoDepartment, "DispatchID:" + id.ToString());
                    }
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    X.GetCmp<Store>("StDispatchGoDepartment").Reload();
                }
                else
                    Ultility.CreateNotification(message: RequestMessage.CreateError);
                return this.Direct(true);
            }
            else
            {
                Ultility.ShowMessageBox("Thông báo", ck, MessageBox.Icon.WARNING, MessageBox.Button.OK);
                return this.Direct(false);
            }
        }
        [ValidateInput(false)]
        public ActionResult Update(DispatchGoItem obj, [Bind(Prefix = "StorageFormID")]string[] borders, bool isEdit)
        {
            string ck = chekValid(obj, obj.ID);
            if (ck.Equals(""))
            {
                setDocIssForm(borders, ref obj);
                obj.IsEdit = isEdit;
                bool ck1 = update(obj);
                if (ck1)
                {
                    if (!isEdit)
                    {
                        NotifyController notify = new NotifyController();
                        notify.Notify(this.ModuleCode, "Có một công văn đi của tổ chức cần phê duyệt", obj.Name, obj.EmployeeInfo.ID, User, Common.DispatchGoDepartment, "DispatchID:" + obj.ID.ToString());
                    }
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    X.GetCmp<Store>("StDispatchGoDepartment").Reload();
                }
                else
                    Ultility.CreateNotification(message: RequestMessage.UpdateError);
                return this.Direct(true);
            }
            else
            {
                Ultility.ShowMessageBox("Thông báo", ck, MessageBox.Icon.WARNING, MessageBox.Button.OK);
                return this.Direct(false);
            }
        }
        public ActionResult MoveDispatch(DispatchGoItem obj, string data = "")
        {
            if (!data.Trim().Equals(""))
            {
                bool ck = update(obj, true, false, false, data);
                if (ck)
                {
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    if (obj.StrType.Equals(DispatchObjectType.Department.ToString()))
                    {
                        X.GetCmp<Store>("stDept").Reload();
                    }
                    else
                        X.GetCmp<Store>("stEmployee").Reload();
                    X.GetCmp<Store>(store).Reload();
                }
                else
                    Ultility.CreateNotification(message: RequestMessage.UpdateError);
            }
            return this.Direct();
        }
        public ActionResult VerifyDispatch(DispatchGoItem obj, bool isEmployee, string fName, string store = "")
        {

            bool ck = update(obj, false, true, isEmployee);
            if (ck)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                if (!store.Equals(""))
                    X.GetCmp<Store>(store).Reload();
                else
                    X.GetCmp<Store>("StDispatchGoEmployee").Reload();
                X.GetCmp<Window>(fName).Close();
            }
            return this.Direct();
        }
        public ActionResult Approve(DispatchGoItem obj)
        {
            bool ck = updateApprove(obj);
            if (ck)
            {
                NotifyController notify = new NotifyController();
                notify.Notify(this.ModuleCode, "Có một công văn đi của tổ chức đã phê duyệt", obj.Name, obj.EmployeeCreate, User, Common.DispatchGoDepartment, "DispatchID:" + obj.ID.ToString());
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                X.GetCmp<Store>("StDispatchGoDepartment").Reload();
                X.GetCmp<Window>("winApprovalDispatchGo").Close();
            }
            return this.Direct();
        }
        public ActionResult DeleteDepartment(int id)
        {
            dispatchGoSV.DeleteDepatment(id);
            Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            return this.Direct();
        }
        public ActionResult DeleteEmployee(int id)
        {
            dispatchGoSV.DeleteEmployee(id);
            Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            return this.Direct();
        }
        public ActionResult InsertMoveOutDetail(DispatchGoObjectItem obj, int type, string fName)
        {
            obj.Type = type;
            obj.UpdateBy = User.ID;
            obj.ID = 0;
            bool ck = updateMoveOut(obj);
            if (ck)
            {
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                X.GetCmp<Window>(fName).Close();
                X.GetCmp<Store>("stObject").Reload();

            }
            else
                Ultility.CreateNotification(message: RequestMessage.CreateError);

            return this.Direct();
        }
        public ActionResult UpdateMoveOutDetail(DispatchGoObjectItem obj, int type, string fName)
        {

            obj.Type = type;
            obj.UpdateBy = User.ID;
            bool ck = updateMoveOut(obj);
            if (ck)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                X.GetCmp<Window>(fName).Close();
                X.GetCmp<Store>("stObject").Reload();

            }
            else
                Ultility.CreateNotification(message: RequestMessage.UpdateError);

            return this.Direct();
        }
        public ActionResult DeleteMoveOut(int id)
        {
            try
            {
                dispatchGoSV.DeleteMoveOut(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                X.GetCmp<Store>("stObject").Reload();
            }
            catch (Exception)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError);
            }
            return this.Direct();
        }
        public ActionResult UpdateVerifyOutDetail(DispatchGoObjectItem obj, int type, string fName)
        {
            bool ck = updateVerifyOut(obj, User.ID);
            if (ck)
            {
                NotifyController notify = new NotifyController();
                notify.NotifyUser(this.ModuleCode, "Có một công văn đã xác nhận!", obj.Name, obj.UpdateBy.HasValue ? obj.UpdateBy.Value : obj.CreateBy.Value, User, Common.DispatchGoEmployee, "DispatchID:" + obj.ID.ToString());
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                X.GetCmp<Window>(fName).Close();
                X.GetCmp<Store>("stObjectVerify").Reload();
            }
            return this.Direct();
        }
        public ActionResult LoadDispatchGo(StoreRequestParameters parameters, int focusId = 0)
        {
            List<int> us = User.GroupIDs;
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lst = dispatchGoSV.GetAll(filter, us, User.ID, focusId);
            return this.Store(lst, filter.PageTotal);
        }
        public ActionResult GetDataDepartment(int id = 0)
        {
            List<DispatchToObjectItem> lst = new List<DispatchToObjectItem>();
            if (id > 0)
            {
                lst = dispatchGoSV.GetDepartmentByID(id);
            }
            return this.Store(lst);
        }
        public ActionResult GetDataEmployee(int id = 0)
        {
            List<DispatchToObjectItem> lst = new List<DispatchToObjectItem>();
            if (id > 0)
            {
                lst = dispatchGoSV.GetEmployeeByID(id);
            }
            return this.Store(lst);
        }
        public ActionResult GetObjMoveOutByID(int id, string type)
        {
            DispatchGoObjectItem obj = new DispatchGoObjectItem();
            var rs = new JsonResult();
            if (id > 0)
            {
                obj = dispatchGoSV.GetMoveOutByDispatchGoID(id, type);

                if (obj != null)
                {
                    rs.Data = obj;
                }
                else
                    rs.Data = new DispatchGoObjectItem();
            }

            return rs;

        }
        public ActionResult GetDataObjectVerify(StoreRequestParameters parameters, int id = 0)
        {
            List<DispatchGoObjectItem> lst = new List<DispatchGoObjectItem>();
            int totalCount;
            lst = dispatchGoSV.GetAllObjectVerifyByID(parameters.Page, parameters.Limit, out totalCount, id);
            return this.Store(lst, totalCount);
        }
        public ActionResult GetDataMoveOut(int id = 0)
        {
            List<DispatchGoObjectItem> lst = new List<DispatchGoObjectItem>();
            if (id > 0)
            {
                lst = dispatchGoSV.GetAllObjectMoveOutByID(id);
            }
            return this.Store(lst);
        }
        private int insert(DispatchGoItem obj)
        {
            int id = 0;

            obj.CreateBy = User.ID;
            id = dispatchGoSV.Insert(obj);

            return id;
        }
        private bool update(DispatchGoItem obj, bool isMove = false, bool isVerify = false, bool isEmployee = false, string data = "")
        {
            try
            {
                obj.UpdateBy = User.ID;
                if (isMove)
                {
                    data = data.Trim(',');
                    var attachments = JSON.Deserialize<List<int>>(data);
                    List<int> Emp = new List<int>();
                    Emp = dispatchGoSV.GetEmployeeNotExits(obj.ID, attachments);
                    dispatchGoSV.UpdateMove(obj, attachments, User.ID);
                    if (obj.StrType.Equals(DispatchObjectType.Employee.ToString()))
                    {
                        NotifyController notify = new NotifyController();
                        notify.Notify(this.ModuleCode, "Có một công văn đã được chuyển đến bạn yêu cầu xác nhận!", obj.Name, Emp, User, Common.DispatchToInSide, "DispatchID:" + obj.ID.ToString());
                    }
                }
                else
                    dispatchGoSV.Update(obj);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private bool updateMoveOut(DispatchGoObjectItem obj)
        {
            //if (chekValid(obj))
            //{
            obj.UpdateBy = User.ID;
            dispatchGoSV.UpdateMoveOut(obj);
            return true;
            // }
            //return false;
        }
        private bool updateVerifyOut(DispatchGoObjectItem obj, int userId)
        {
            obj.UpdateBy = userId;
            dispatchGoSV.UpdateVerifyOut(obj);
            return true;
        }
        private bool updateApprove(DispatchGoItem obj)
        {
            //if (chekValid(obj))
            //{
            obj.UpdateBy = User.ID;
            dispatchGoSV.UpdateApprove(obj);
            return true;
            //}

            // return false;
        }
        private string chekValid(DispatchGoItem obj, int id = 0)
        {
            obj.Code = obj.Code.Trim();
            var ck = dispatchGoSV.CheckNotExitNumberTo(obj.Code, id);
            if (!ck)
                return "Số ký hiệu đã tồn tại trên hệ thống. Vui lòng nhập Số ký hiệu khác!";
            return "";
        }
        private int setDocIssForm(string[] doc, ref DispatchGoItem obj)
        {
            if (doc != null)
            {
                if (doc.Count() == 2)
                {
                    obj.FormH = true; obj.FormS = true;
                    return (int)StorageForm.Both;
                }
                else
                {
                    if (doc[0].ToString().Equals(StorageForm.SoftCopy.ToString()))
                    {
                        obj.FormS = true;
                        return (int)StorageForm.SoftCopy;
                    }
                    else
                    {
                        obj.FormH = true;
                        return (int)StorageForm.HardCopy;
                    }
                }
            }
            return -1;
        }
        public ActionResult ShowEmployeeCC(int dispatchGoId = 0)
        {
            var obj = dispatchGoSV.GetByID(dispatchGoId);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "EmployeeCC", Model = obj };
        }
        public ActionResult GetAllEmployee(StoreRequestParameters parameters, string query = "")
        {
            int totalCount = 0;
            var result = new List<HumanEmployeeItem>();
            result = employeeService.GetAll(parameters.Page, parameters.Limit, out totalCount, query);
            foreach (HumanEmployeeItem emp in result)
            {
                string strChucDanh = "";
                if (emp.IsTrial)
                    strChucDanh = "Thử việc";
                else
                {
                    foreach (HumanGroupPositionItem hgpI in emp.lstHumanGrPos)
                    {
                        strChucDanh += hgpI.Name + " -- " + hgpI.GroupName + "</br>";
                    }
                }
                emp.ChucDanh = strChucDanh;
            }
            return this.Store(result, totalCount);
        }
        public ActionResult LoadEmployeeCC(StoreRequestParameters parameters, int dispatchGoId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var lsEmployee = dispatchGoSV.GetEmployeeCC(filter, dispatchGoId);
            return this.Store(lsEmployee, filter.PageTotal);
        }
        public ActionResult AddCC(int dispatchGoId = 0, int employeeId = 0)
        {
            dispatchGoSV.AddEmployeeCC(dispatchGoId, employeeId, User.ID);
            var obj = dispatchGoSV.GetByID(dispatchGoId);
            NotifyController notify = new NotifyController();
            notify.Notify(this.ModuleCode, "Có một công văn đi bạn có quyền theo dõi", obj.Name, employeeId, User, Common.DispatchGoCC, "DispatchID:" + dispatchGoId);
            X.GetCmp<Store>("StoreEmployeeCC").Reload();
            return this.Direct();
        }
        public ActionResult DeleteCC(string stringId)
        {
            try
            {
                dispatchGoSV.DeleteCC(stringId);
                X.Msg.Notify("Thông báo", "Bạn đã xóa bản ghi thành công!").Show();
                X.GetCmp<Store>("StoreEmployeeCC").Reload();
                return this.Direct();
            }
            catch (Exception ex)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "Thông báo",
                    Message = "Đã xảy ra lỗi trong quá trình xóa dữ liệu!"
                });
                return this.Direct("Error");
            }
        }
    }
}

