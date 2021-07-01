using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;
using iDAS.Web.API.Human;
using iDAS.Web.API.Problem;
using iDAS.Web.Areas.Department.Models.Title;
using iDAS.Web.Areas.Document.Models;
using iDAS.Web.Areas.Human.Model;
using iDAS.Web.Areas.Human.Models;
using iDAS.Web.Areas.Problem.Models.RelativePeople;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace iDAS.Web.Areas.Human.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Danh sách nhân sự", IsActive = true, Icon = "GroupLink", IsShow = true, Position = 1)]
    public class V3_EmployeeController : BaseController
    {
        private HumanEmployeeSV employeeService = new HumanEmployeeSV();
        private HumanOrganizationSV organizationService = new HumanOrganizationSV();
        private V3_HumanEmployeeAPI v3_HumanEmployeeAPI = new V3_HumanEmployeeAPI();
        private V3_ProfilePermissionAPI v3_ProfilePermissionAPI = new V3_ProfilePermissionAPI();
        private readonly string storeUsersID = "StoreEmployee";
        private ProblemRelativePeopleAPI apiProblemRelativePeople = new ProblemRelativePeopleAPI();
        
        #region Load dữ liệu
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            UserPrincipal abc= SystemAuthenticate.GetAuthCookies();
            //lay danh sach quyen dua vao view
            int employeeID = User.EmployeeID;
            List<V3_CheckProfileHumanPermissionResponse> lstPermission = v3_ProfilePermissionAPI.GetListPermissionByEmployeeID(employeeID).Data;
           
            var json = new JavaScriptSerializer().Serialize(lstPermission);
            ViewBag.lstPermission = json;
            return View();
        }
        public ActionResult RemoveEmployeePerformDocument(string empIds = default(string))
        {
            if (string.IsNullOrEmpty(empIds))
                return this.Store("Không có dữ liệu");

            if (Session["EmpDocument"] == null)
                return this.Store("Không có dữ liệu");

            if (TempData["tempEmployeeList"] == null)
            {
                List<DocumentEmployeeViewModel> lst = new List<DocumentEmployeeViewModel>();
                List<DocumentEmployeeViewModel> lstTemp = (List<DocumentEmployeeViewModel>)Session["EmpDocument"];
                for (int i = 0; i < lstTemp.Count; i++)
                {
                    lst.Add(lstTemp[i]);
                }

                TempData["tempEmployeeList"] = lst;
            }

            var lstEmp = (List<DocumentEmployeeViewModel>)Session["EmpDocument"];
            var lstChoose = empIds.Split(',');

            for (int j = 0; j < lstEmp.Count(); j++)
            {
                for (int i = 0; i < lstChoose.Length; i++)
                {
                    if (lstEmp[j].ID==int.Parse(lstChoose[i]))
                    {
                        lstEmp.Remove(lstEmp[j]);
                        j--;
                        break;
                    }    
                }
            }
            //Session.Remove("EmpProfile");
            return this.Store(lstEmp);
        }
        public ActionResult ReturnEmployeeDocumentWhenExit()
        {
            // lấy lại dữ liệu trước khi cập nhật
            if (TempData["tempEmployeeList"] != null)
            {
                //List<ProfileUsersMappingBO> lst = new List<ProfileUsersMappingBO>();
                //lst = (List<ProfileUsersMappingBO>)TempData["tempEmployeeList"];
                //Session["EmpProfile"] = lst;
                Session.Remove("tempEmployeeList");
            }
            Session.Remove("EmpDocument");
            Session.Remove("RemoveEmpDocument");
            Session.Remove("LstPermission");
            return this.Direct(result: true);
        }
        public ActionResult AddEmployeePerformDocument(StoreRequestParameters parameters, string empIds = default(string))
        {
            var data = new List<DocumentEmployeeViewModel>();
            if (string.IsNullOrEmpty(empIds))
                return this.Store(data);

            if (Session["EmpDocument"] != null)
                data = (List<DocumentEmployeeViewModel>)Session["EmpDocument"];

            if (TempData["tempEmployeeList"] == null && Session["EmpDocument"] != null)
            {
                List<DocumentEmployeeViewModel> lst = new List<DocumentEmployeeViewModel>();
                List<DocumentEmployeeViewModel> lstTemp = (List<DocumentEmployeeViewModel>)Session["EmpDocument"];
                for (int i = 0; i < lstTemp.Count; i++)
                {
                    lst.Add(lstTemp[i]);
                }

                TempData["tempEmployeeList"] = lst;
            }

            var arrId = empIds.Split(',');
            List<int> lstId = new List<int>();
            for (int i = 0; i < arrId.Length-1; i++)
            {
                if (!string.IsNullOrWhiteSpace(arrId[i]) && (data.Count == 0 || data.Where(x => x.ID == int.Parse(arrId[i])).Count() == 0))
                    lstId.Add(int.Parse(arrId[i]));
            }
            var result = v3_HumanEmployeeAPI.GetByListEmployeeId(lstId).Result.Data;
            data.AddRange(result);
            if (Session["EmpDocument"] != null)
            {
                Session["EmpDocument"] = data;
            }
          
            return this.Store(data);

        }
        public ActionResult LoadUserForDocument(StoreRequestParameters parameters, int departmentId = 0)
        {
            int totalCount = 0;
            if (new DepartmentSV().GetByID(departmentId).ParentID == 0)
                departmentId = 0;

            HumanEmployeeSearchRequest req = new HumanEmployeeSearchRequest();
            req.departmentID = departmentId;
            req.pageNumber = parameters.Page;
            req.pageSize = parameters.Limit;

            var result = v3_HumanEmployeeAPI.GetListByDepartment(req);
            TempData["departmentID"] = departmentId;

         
            if (result.TotalRows != null)
                totalCount = result.TotalRows.Value;
            return this.Store(new Paging<V3_HumanEmployeeResponse>(result.Data, totalCount));
        }

        public ActionResult GetEmployeePerformedDocument( int documentId = 0)
        {
            List<DocumentEmployeeViewModel> data = new List<DocumentEmployeeViewModel>();
            //(List<EmployeeBO>)Session["EmpTaskSchedule"];
            if (Session["EmpDocument"] != null)
            {
                data = (List<DocumentEmployeeViewModel>)Session["EmpDocument"];
            }
            else if (documentId != 0)
            {
                var get = v3_HumanEmployeeAPI.GetByDocument(documentId).Result.Data;
                data = get.Count>0 ? get : new List<DocumentEmployeeViewModel>();
                Session["EmpDocument"] = data;
            }
            return this.Store(data);
        }
        
       [SystemAuthorize(CheckAuthorize = true, ActionCode = "Index")]
        public ActionResult LoadUsers(StoreRequestParameters parameters, int departmentID = 0, string name=null, DateTime? birthDayFrom = null, DateTime? birthDayTo = null, string native = "", int? religion = null, int? ethnic = null,
                                    string placeOfWork = "", string position = "", string departmentName = "",
                                    string eduName = "", int? educationType = null, int? educationResult = null,
                                    string diplomaName = "", int? major = null, int? diplomaEucationType = null, int? diplomaEducationOrg = null, int? educationLevel = null, int? certificateLevel = null,
                                    string certificateName = "", int? level = null, int? certificatetype = null,
                                    string numberOfDecision = "", string reason = "", int? form = null)
        {
            int totalCount = 0;
            if (new DepartmentSV().GetByID(departmentID).ParentID == 0)
                departmentID = 0;

            HumanEmployeeSearchRequest req = new HumanEmployeeSearchRequest();
            req.departmentID = departmentID;
            req.pageNumber = parameters.Page;
            req.pageSize = parameters.Limit;
            req.name = name;
            req.birthDayFrom = birthDayFrom;
            req.birthDayTo = birthDayTo;
            req.native = native;
            req.religion = religion;
            req.ethnic = ethnic;

            req.placeOfWork = placeOfWork;
            req.position = position;
            req.departmentName = departmentName;

            req.eduName = eduName;
            req.educationType = educationType;
            req.educationResult = educationResult;

            req.diplomaName = diplomaName;
            req.major = major;
            req.diplomaEucationType = diplomaEucationType;
            req.diplomaEducationOrg = diplomaEducationOrg;
            req.educationLevel = educationLevel;
            req.certificateLevel = certificateLevel;

            req.certificateName = certificateName;
            req.level = level;
            req.certificatetype = certificatetype;

            req.numberOfDecision = numberOfDecision;
            req.reason = reason;
            req.form = form;


            var result = v3_HumanEmployeeAPI.GetListByDepartment(req);
            TempData["EmployeeList"] = result.Data;
            TempData["departmentID"] = departmentID;

            foreach (V3_HumanEmployeeResponse emp in result.Data)
            {
                List<string> strIDChucDanh = new List<string>();
                string strChucDanh = "";
                if (emp.IsTrial)
                    strChucDanh = "Thử việc";
                else
                {
                    foreach (DepartmentTitleResponse hgpI in emp.ListDepartmentTitle)
                    {
                        strChucDanh += hgpI.TitleName + " -- " + hgpI.DepartmentName + "</br>";
                        strIDChucDanh.Add(hgpI.ID.ToString());
                    }
                }
                emp.ListIDDepartmentTitle = string.Join("|", strIDChucDanh);
                emp.DepartmentTitle = strChucDanh;
            }
            if (result.TotalRows != null)
                totalCount = result.TotalRows.Value;
            return this.Store(new Paging<V3_HumanEmployeeResponse>(result.Data, totalCount));
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


        [HttpPost]
        public ActionResult Delete(HumanEmployeeItem updateData)
        {
            try
            {
                employeeService.Delete(updateData.ID);
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
                if (lstSelectedRole.Contains("chkReview"))
                {
                    lstSelectedRole = "chkView,chkUpdate,chkReview,";
                }
                else if (lstSelectedRole.Contains("chkUpdate"))
                {
                    lstSelectedRole = "chkView,chkUpdate,,";
                }
                else if (lstSelectedRole.Contains("chkView"))
                {
                    lstSelectedRole = "chkView,,,";
                }

                if (lstSelectedRole == "chkView,chkUpdate,," && !isContain(lstRelativeDeptsFilter, deptId + ""))
                {
                    Ultility.ShowMessageBox("Thông báo", "Bạn phải chọn nhân sự liên quan thuộc phòng ban xử lý !", MessageBox.Icon.WARNING, MessageBox.Button.OK);
                }
                else
                {
                    apiProblemRelativePeople.InsertListProblemRelativePeople(problemEventID, lstSelectedRole, deptId, lstHumanEmployeeId);
                    //X.GetCmp<Store>("stListRelativePeople").Reload();
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
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
        public ActionResult EmployeeForDocument(int documentId)
        {
            //  var data = Ext.Net.JSON.Deserialize<Dictionary<string, object>>(parameter ?? string.Empty);
            //  urlStore = urlStore ?? Url.Action("LoadData", "Employee", new { area = "Generic" });
            //ViewData["Parameters"] = data ?? new Dictionary<string, object>();
            //ViewData["UrlStore"] = urlStore;
            //ViewData["Type"] = !string.IsNullOrEmpty(Type) ? Type : "";
            ViewData["documentId"] = documentId;
            //HumanEmployeeSearchRequest req = new HumanEmployeeSearchRequest();
            //req.departmentID = documentId;
            //var department = v3_HumanEmployeeAPI.GetListByDepartment(req).Data;
            //if (department != null)
            //{
            //    ViewData["departmentName"] = department.Name;
            //    ViewData["departmentId"] = department.Id;
            //}
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewData = ViewData,
            };
        }
    }
}
