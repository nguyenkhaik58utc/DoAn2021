using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iDAS.Core;
using iDAS.Services;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web;
using iDAS.Items;
using iDAS.Web.Areas.Problem.Models.RelativePeople;
using iDAS.Utilities;
using iDAS.Web.API.Problem;

namespace iDAS.Web.Areas.Human.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Danh sách nhân sự", IsActive = true, Icon = "GroupLink", IsShow = true, Position = 1)]
    public class EmployeeController : BaseController
    {
        private HumanEmployeeSV employeeService = new HumanEmployeeSV();
        private HumanOrganizationSV organizationService = new HumanOrganizationSV();
        private readonly string storeUsersID = "StoreEmployee";

        // HungNM. APIs for ProblemRelativePeople.
        private ProblemRelativePeopleAPI apiProblemRelativePeople = new ProblemRelativePeopleAPI();
        // End. HungNM.

        #region Load dữ liệu
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        [SystemAuthorize(CheckAuthorize = true, ActionCode = "Index")]
        public ActionResult LoadUsers(StoreRequestParameters parameters, int departmentID = 0, string querySearch = "")
        {
            int totalCount = 0;
            var result = new List<HumanEmployeeItem>();
            if (new DepartmentSV().GetByID(departmentID).ParentID == 0)
            {
                result = employeeService.GetAll(parameters.Page, parameters.Limit, out totalCount, querySearch);
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
            }
            else
            {
                result = employeeService.GetAllByDepartmentId(parameters.Page, parameters.Limit, out totalCount, departmentID, querySearch);
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

            }
            return this.Store(result, totalCount);
        }
        #endregion

        #region Thêm
        [BaseSystem(Name = "Thêm mới")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Insert()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Insert" };
        }
        [HttpPost]
        public ActionResult Insert(HumanEmployeeItem employee)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    var fileUploadField = X.GetCmp<FileUploadField>("FileUploadFieldId");
                    if (fileUploadField.PostedFile != null && fileUploadField.PostedFile.ContentLength > 0)
                    {
                        employee.ImageFile = new FileItem()
                        {
                            ID = Guid.NewGuid(),
                            Name = fileUploadField.FileName,
                            Size = fileUploadField.FileBytes.Length,
                            Type = fileUploadField.PostedFile.ContentType,
                            Extension = string.IsNullOrWhiteSpace(fileUploadField.FileName) ? "" : fileUploadField.FileName.Substring(fileUploadField.FileName.IndexOf('.') + 1),
                            Data = fileUploadField.FileBytes
                        };
                    }
                    var id = employeeService.Insert(employee, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    return this.Direct(id);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
            }
            return this.FormPanel();
        }
        /// <summary>
        /// Thêm mới nhân sự đã trúng tuyển
        /// </summary>
        /// <param name="ProfileInterviewID"></param>
        /// <param name="RequirementID"></param>
        /// <returns></returns>
        [SystemAuthorize(CheckAuthorize = true, ActionCode = "Insert")]
        public ActionResult AddFormFromRecruitment(int ProfileID = 0)
        {
            var recruitmentProfileServices = new HumanRecruitmentProfileSV();
            var data = recruitmentProfileServices.GetEmployeeByRecruitmentProfileID(ProfileID);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Insert", Model = data };
        }
        #endregion

        #region Sửa
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Sửa")]
        [HttpGet]
        public ActionResult Update(int Id)
        {
            var data = employeeService.GetById(Id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = data };
        }
        [HttpPost]
        public ActionResult Update(HumanEmployeeItem updateData)
        {
            try
            {
                var fileUploadField = X.GetCmp<FileUploadField>("FileUploadFieldEdit");
                // Đường dẫn ảnh đại diện cho nhân sự
                if (fileUploadField.PostedFile != null && fileUploadField.PostedFile.ContentLength > 0)
                {
                    updateData.ImageFile = new FileItem()
                    {
                        Name = fileUploadField.FileName,
                        Size = fileUploadField.FileBytes.Length,
                        Type = fileUploadField.PostedFile.ContentType,
                        Extension = string.IsNullOrWhiteSpace(fileUploadField.FileName) ? "" : fileUploadField.FileName.Substring(fileUploadField.FileName.IndexOf('.') + 1),
                        Data = fileUploadField.FileBytes
                    };
                }
                employeeService.Update(updateData, User.ID);
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                return this.Direct();

            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                return RedirectToAction("Index");
            }
        }


        #endregion

        #region Xóa
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xóa")]
        public ActionResult DeleteUser(int id)
        {
            try
            {
                employeeService.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                X.GetCmp<Store>(storeUsersID).Reload();
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError, error: true);
            }
            return this.Direct();
        }
        #endregion

        #region Xem
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem chi tiết")]
        public ActionResult DetailForm(int Id)
        {
            var data = employeeService.GetById(Id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = data };
        }
        #endregion

        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thiết lập chức danh")]
        public ActionResult SettingForm(int Id)
        {
            var data = new HumanEmployeeItem();
            data.ID = Id;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Setting", Model = data };
        }

        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thiết lập chức danh")]
        public ActionResult SettingFormV3(int Id)
        {
            var data = new HumanEmployeeItem();
            data.ID = Id;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "SettingV3", Model = data };
        }

        #region Tài khoản
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Tài khoản")]
        [HttpGet]
        public ActionResult UpdateAccount(int Id)
        {
            var accountServices = new HumanUserSV();
            var data = accountServices.GetByEmployeeId(Id);
            if (data == null)
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "InsertAccount", Model = new HumanUserItem { EmployeeID = Id } };
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "UpdateAccount", Model = data };
        }
        [HttpPost]
        public ActionResult UpdateAccount(HumanUserItem updateData)
        {
            try
            {
                var accountServices = new HumanUserSV();
                if (updateData.ID == 0)
                {
                    accountServices.Insert(updateData, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                else
                {
                    if (updateData.Password == Password.EmptyPass) updateData.Password = "";
                    accountServices.Update(updateData, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct();
        }
        [HttpPost]
        public ActionResult InsertAccount(HumanUserItem updateData)
        {
            try
            {
                var userServices = new HumanUserSV();
                //if (this.ModelState.IsValid)
                //{
                    userServices.Insert(updateData, User.ID);
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                //}
                //else
                //{
                //    Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                //}
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
            }
            return this.Direct();
        }
        #endregion

        [BaseSystem(Name = "Chuyển tổ chức")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowChangeDepartment(string strEmployeesId, int departmentID)
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "ChangeDepartment", ViewData = new ViewDataDictionary { { "strEmployeesId", strEmployeesId }, { "departmentID", departmentID } } };
        }

        #region form dùng chung
        public ActionResult EmployeeView(bool multi = true)
        {
            ViewData["ModeMulti"] = multi;
            return PartialView();
        }
        public ActionResult EmployeeViewMulti(string gridEmployeeId)
        {
            ViewData["GridEmployeeID"] = gridEmployeeId;
            return PartialView();
        }
        public ActionResult EmployeeWindow(bool multi = true)
        {
            ViewData["ModeMulti"] = multi;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewData = ViewData,
            };
        }

        // HungNM. Add new form for Problem, insert ProblemRelativePeople.
        public ActionResult tabFormRelativePeople(int? problemEventID)
        {
            string lstRelativeDeptsFilter = "";
            var resp = new iDAS.Web.API.ProblemEventAPI().GetDepartmentByProblemEvent(problemEventID.HasValue ? problemEventID.Value : 0).Result;
            foreach (var i in resp)
            {
                lstRelativeDeptsFilter += i.ObjectID + ",";
            }
            var model = new ProblemRelativePeopleChkCbo();
            model.lstRelativeDeptsFilter = lstRelativeDeptsFilter;
            return PartialView("ProblemRelativePeople", model);
        }
        // Get all relative people. 20201104_10h49.
        public ActionResult GetListRelativePeople(int problemEventID, string listType)
        {
            var resp = apiProblemRelativePeople.GetListProblemRelativePeople(problemEventID, listType + "" == "" ? "rdbAll" : listType);
            int total = 0;
            if (resp.Result != null && resp.Result.Count > 0)
                total = resp.Result.Count;

            foreach (var item in resp.Result)
            {
                item.txtVaiTro = "" + (item.View ? "Người theo dõi," : "") + (item.Update ? "Người xử lý," : "") + (item.Review ? "Người đánh giá," : "");
                if (item.txtVaiTro != "")
                {
                    item.txtVaiTro = item.txtVaiTro.Remove(item.txtVaiTro.Length - 1);
                }
            }

            return this.Store(new Paging<ProblemRelativePeopleDTO>(resp.Result, total));
        }
        public ActionResult EmployeeWindowNewVer3(int problemEventID, string lstRelativeDeptsFilter, string lstSelectedRole)
        {
            ViewData["problemEventID"] = problemEventID + "";
            ViewData["lstRelativeDeptsFilter"] = lstRelativeDeptsFilter;
            ViewData["lstSelectedRole"] = lstSelectedRole;
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "EmployeeWindowNewVer3", ViewData = ViewData };
        }
        public ActionResult EmployeeViewNewVer3(string gridProblemRelativePeople = "", string hdfDepartmentId = "")
        {
            ViewData["GridProblemRelativePeople"] = gridProblemRelativePeople;
            ViewData["HdfDepartmentID"] = hdfDepartmentId;
            return PartialView();
        }
        public ActionResult SaveRelativePeople(int problemEventID, string lstRelativeDeptsFilter, string lstSelectedRole, int deptId, string lstHumanEmployeeId)
        {
            try
            {
                

                string tmpLstHumanEmployeeId = "";

                if (lstHumanEmployeeId != "")
                {
                    tmpLstHumanEmployeeId = lstHumanEmployeeId.Remove(lstHumanEmployeeId.Length - 1);
                }
                var resp = apiProblemRelativePeople.GetListRoleDepartmentFromListHumanEmployeeID(tmpLstHumanEmployeeId);

                int tmpHumanEmployeeID = 0;
                int deptId2Save = 0;
                int HumanEmployeeId2Save = 0;
                string tmpNameInSave = "";
                bool isSave = false;
                string notSaveNames = "";

                for (int i = 0; i < resp.Count; i++)
                {
                    var item = resp[i];
                    if (tmpHumanEmployeeID != item.HumanEmployeeID)     // Begin seeking or begin a new HumanEmployeeID
                    {
                        // Begin seeking
                        if (tmpHumanEmployeeID == 0)
                        {
                            tmpHumanEmployeeID = item.HumanEmployeeID;
                            deptId2Save = item.HumanDepartmentID;
                            HumanEmployeeId2Save = item.HumanEmployeeID;
                            tmpNameInSave = item.Name;
                            // check the same to deptId
                            if ((deptId2Save == deptId) || (i == resp.Count - 1))
                            {
                                // check warning to save to DB
                                if (lstSelectedRole.Contains("chkUpdate") && !isContain(lstRelativeDeptsFilter, deptId2Save + ""))
                                {
                                    notSaveNames += tmpNameInSave + ",";
                                    Ultility.ShowMessageBox("Thông báo", "Bạn phải chọn nhân sự liên quan " + notSaveNames.Remove(notSaveNames.Length-1) + " thuộc phòng ban xử lý !", MessageBox.Icon.WARNING, MessageBox.Button.OK);
                                }
                                else
                                {
                                    if (!isSave)
                                    {
                                        apiProblemRelativePeople.InsertListProblemRelativePeople(problemEventID, lstSelectedRole, deptId2Save, HumanEmployeeId2Save + ",");
                                        //X.GetCmp<Store>("stListRelativePeople").Reload();
                                        Ultility.CreateNotification(message: RequestMessage.UpdateSuccess + " cho nhân sự " + tmpNameInSave);
                                        isSave = true;
                                    }
                                }
                            }
                        }
                        else // Begin an other new HumanEmployeeId
                        {
                            // save the first record if not still saving
                            if (isSave) // already saving then not saving
                            {
                                isSave = false;
                            }
                            else // not still saving, begin saving the first record. Then assign to the other new HumanEmployeeId
                            {
                                // check warning to save the first record to DB
                                if (lstSelectedRole.Contains("chkUpdate") && !isContain(lstRelativeDeptsFilter, deptId2Save + ""))
                                {
                                    notSaveNames += tmpNameInSave + ",";
                                    Ultility.ShowMessageBox("Thông báo", "Bạn phải chọn nhân sự liên quan " + notSaveNames.Remove(notSaveNames.Length-1) + " thuộc phòng ban xử lý !", MessageBox.Icon.WARNING, MessageBox.Button.OK);
                                }
                                else
                                {
                                    apiProblemRelativePeople.InsertListProblemRelativePeople(problemEventID, lstSelectedRole, deptId2Save, HumanEmployeeId2Save + ",");
                                    //X.GetCmp<Store>("stListRelativePeople").Reload();
                                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess + " cho nhân sự " + tmpNameInSave);
                                }
                            }
                            // Assign to the other new HumanEmployeeId
                            tmpHumanEmployeeID = item.HumanEmployeeID;
                            deptId2Save = item.HumanDepartmentID;
                            HumanEmployeeId2Save = item.HumanEmployeeID;
                            tmpNameInSave = item.Name;
                            // check if the last record then saving to DB
                            if (i == resp.Count - 1)
                            {
                                // check warning to save the first record to DB
                                if (lstSelectedRole.Contains("chkUpdate") && !isContain(lstRelativeDeptsFilter, deptId2Save + ""))
                                {
                                    notSaveNames += tmpNameInSave + ",";
                                    Ultility.ShowMessageBox("Thông báo", "Bạn phải chọn nhân sự liên quan " + notSaveNames.Remove(notSaveNames.Length-1) + " thuộc phòng ban xử lý !", MessageBox.Icon.WARNING, MessageBox.Button.OK);
                                }
                                else
                                {
                                    apiProblemRelativePeople.InsertListProblemRelativePeople(problemEventID, lstSelectedRole, deptId2Save, HumanEmployeeId2Save + ",");
                                    //X.GetCmp<Store>("stListRelativePeople").Reload();
                                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess + " cho nhân sự " + tmpNameInSave);
                                }
                            }
                        }
                    }
                    else
                    {
                        // Check the same to deptId
                        if ((item.HumanDepartmentID == deptId) || (i == resp.Count - 1))
                        {
                            int tmpdeptId2Save01 = 0;
                            string tmpNameInSave01 = "";
                            int tmpHumanEmployeeId2Save01 = 0;

                            if (item.HumanDepartmentID == deptId)
                            {
                                // Get the current record.
                                tmpdeptId2Save01 = item.HumanDepartmentID;
                                tmpNameInSave01 = item.Name;
                                tmpHumanEmployeeId2Save01 = item.HumanEmployeeID;
                            }
                            else
                            {
                                // Get the first record of the current HumanDepartmentId.
                                tmpdeptId2Save01 = deptId2Save;
                                tmpNameInSave01 = tmpNameInSave;
                                tmpHumanEmployeeId2Save01 = HumanEmployeeId2Save;
                            }

                            // check warning to save to DB
                            if (lstSelectedRole.Contains("chkUpdate") && !isContain(lstRelativeDeptsFilter, tmpdeptId2Save01 + ""))
                            {
                                notSaveNames += tmpNameInSave01 + ",";
                                Ultility.ShowMessageBox("Thông báo", "Bạn phải chọn nhân sự liên quan " + notSaveNames.Remove(notSaveNames.Length-1) + " thuộc phòng ban xử lý !", MessageBox.Icon.WARNING, MessageBox.Button.OK);
                            }
                            else
                            {
                                if (!isSave)
                                {
                                    apiProblemRelativePeople.InsertListProblemRelativePeople(problemEventID, lstSelectedRole, tmpdeptId2Save01, tmpHumanEmployeeId2Save01 + ",");
                                    //X.GetCmp<Store>("stListRelativePeople").Reload();
                                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess + " cho nhân sự " + tmpNameInSave01);
                                    isSave = true;
                                }
                            }
                        }

                    }
                }

                //if (lstSelectedRole == "chkView,chkUpdate,," && !isContain(lstRelativeDeptsFilter, deptId + ""))
                //if (lstSelectedRole.Contains("chkUpdate") && !isContain(lstRelativeDeptsFilter, deptId + ""))
                //{
                //    Ultility.ShowMessageBox("Thông báo", "Bạn phải chọn nhân sự liên quan thuộc phòng ban xử lý !", MessageBox.Icon.WARNING, MessageBox.Button.OK);
                //}
                //else
                //{
                //    apiProblemRelativePeople.InsertListProblemRelativePeople(problemEventID, lstSelectedRole, deptId, lstHumanEmployeeId);
                //    //X.GetCmp<Store>("stListRelativePeople").Reload();
                //    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                //}
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct();
        }

        public bool isContain(string lstParent, string strSub)
        {
            bool rsl = false;

            if (lstParent.Length > 0)
            {
                string[] tmp = lstParent.Remove(lstParent.Length - 1).Split(',');
                for (int i = 0; i < tmp.Length; i++)
                {
                    if (tmp[i] == strSub)
                    {
                        rsl = true;
                    }
                }
            }
            return rsl;
        }
        // Delete relative people
        public ActionResult DeleteRelativePeople(string lstDeletedRelatePeople)
        {
            try
            {
                apiProblemRelativePeople.DeleteListProblemRelativePeople(lstDeletedRelatePeople);
                X.GetCmp<Store>("stListRelativePeople").Reload();
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            return this.Direct();
        }
        // HungNM. End.
        #endregion

    }
}
