using iDAS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Services;
using iDAS.Items;
using iDAS.Utilities;
using iDAS.Web.Controllers;

namespace iDAS.Web.Areas.Human.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Đợt đánh giá nhân sự", Icon = "ApplicationForm", IsActive = true, IsShow = true, Position = 3, Parent = GroupAuditKPIController.Code)]
    public class AuditGradationController : BaseController
    {
        private HumanAuditTickSV humanAuditTickSV = new HumanAuditTickSV();
        private HumanAuditGradationSV humanAuditGradationSV = new HumanAuditGradationSV();
        private HumanAuditDepartmentSV humanAuditEmployeeSV = new HumanAuditDepartmentSV();
        private HumanAuditGradationRoleSV humanAuditGradationRoleSV = new HumanAuditGradationRoleSV();
        private HumanCriteriaCategorySV humanCriteriaCategorySV = new HumanCriteriaCategorySV();
        private HumanCriteriaCategorySV humanCategoryCriteriaSV = new HumanCriteriaCategorySV();
        private HumanAuditGradationCriteriaSV humanAuditGradationCriteriaSV = new HumanAuditGradationCriteriaSV();
        private HumanAuditLevelSV levelSV = new HumanAuditLevelSV();
        private HumanCriteriaSV humanCriteriaSV = new HumanCriteriaSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index(int focusId = 0)
        {
            ViewBag.FocusId = focusId;
            return View();
        }
        public ActionResult ShowSendApproval(int id)
        {
            var obj = humanAuditGradationSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "SendApproval", Model = obj };
        }
        public ActionResult GetRoleAuditData(StoreRequestParameters parameters, int humanAuditGradationId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            List<HumanAuditGradationRoleItem> lstData;
            lstData = humanAuditGradationRoleSV.GetAll(filter, humanAuditGradationId);
            return this.Store(lstData, filter.PageTotal);
        }
        public ActionResult GetCriterialData(StoreRequestParameters parameters, int humanAuditGradationRoleId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            List<HumanAuditGradationCriteriaItem> lstData;
            lstData = humanAuditGradationCriteriaSV.GetAll(filter, humanAuditGradationRoleId);
            return this.Store(lstData, filter.PageTotal);
        }
        public ActionResult GetDataCoppy(StoreRequestParameters parameters)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            List<HumanAuditCriteriaCategoryItem> lstData;
            lstData = humanCategoryCriteriaSV.GetDataCoppy(filter);
            return this.Store(lstData, filter.PageTotal);
        }
        public ActionResult ShowCoppy(int id = 0)
        {
            try
            {
                ViewData["HumanAuditGradationRoleID"] = id;
                return new Ext.Net.MVC.PartialViewResult { ViewName = "CoppyData", ViewData = ViewData };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult CoppyData(int humanAuditGradationRoleId = 0, string stringCateId = "0")
        {
            try
            {
                humanAuditGradationCriteriaSV.CoppyData(humanAuditGradationRoleId, stringCateId, User.ID);
                X.GetCmp<Store>("stCriterial").Reload();
                X.GetCmp<Window>("winCoppyDataCateCriteria").Close();
            }
            catch (Exception ex)
            {

                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "Thông báo",
                    Message = "Đã xảy ra lỗi trong quá trình coppy dữ liệu!"
                });
            }
            return this.Direct();
        }
        public ActionResult GetData(StoreRequestParameters parameters, int focusId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            List<HumanAuditGradationItem> lstData;
            lstData = humanAuditGradationSV.GetAll(filter, focusId);
            return this.Store(lstData, filter.PageTotal);
        }
        public ActionResult GetDataQuestion()
        {
            List<HumanAuditCriteriaCategoryItem> lstData;
            lstData = humanCriteriaCategorySV.GetCombobox();
            return this.Store(lstData);
        }
        public ActionResult GetDataLevel()
        {
            List<ComboboxItem> lstData = new List<ComboboxItem>();
            //lstData = HumanAuditGradationSV.GetLevelCombo();
            return this.Store(lstData);
        }
        public ActionResult GetDataAuditDepartment(StoreRequestParameters parameters, int AuditGradationId = 0)
        {
            List<HumanAuditDepartmentItem> lstData;
            int total;
            lstData = humanAuditEmployeeSV.GetAll(parameters.Page, parameters.Limit, out total, AuditGradationId);
            return this.Store(new Paging<HumanAuditDepartmentItem>(lstData, total));
        }
        public ActionResult FormListObject()
        {
            return View("LstObject");
        }
        public ActionResult ChoiceObject(int phaseAuditId = 0, int recordIndex = 0, string commandName = "")
        {
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "ChoiceObject", ViewData = new ViewDataDictionary { { "AuditGradationId", phaseAuditId }, { "recordIndex", recordIndex }, { "commandName", commandName } } };
        }
        public ActionResult SaveAuditObject(string stringId, int AuditGradationId)
        {
            try
            {
                if (stringId.Trim() != "")
                {
                    humanAuditEmployeeSV.InsertObject(stringId, User.ID, AuditGradationId);
                    X.GetCmp<Store>("stMnObject").Reload();
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                throw;
            }
            return this.Direct();
        }
        public ActionResult ChoiceObjectAdd(int recordIndex = 0, string commandName = "")
        {
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "ChoiceObjectAdd", ViewData = new ViewDataDictionary { { "recordIndex", recordIndex }, { "commandName", commandName } } };
        }
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmAdd()
        {
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Create" };
        }
        [BaseSystem(Name = "Sửa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowUpdate(int id)
        {
            var obj = humanAuditGradationSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Edit", Model = obj };
        }
        public ActionResult Insert(HumanAuditGradationItem objNew, bool val, string arrObject, bool allow = true)
        {
            try
            {
                var objectAudit = Ext.Net.JSON.Deserialize<object[]>(arrObject);
                if (objectAudit.Length == 0)
                {
                    Ultility.CreateMessageBox(title: "Thông báo", message: "Bạn phải chọn phòng ban cần đánh giá!", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                    return this.Direct(false);
                }
                else if (!allow)
                {
                    Ultility.CreateMessageBox(title: "Thông báo", message: "Bạn phải chọn người quản lý và lãnh đạo đánh giá cho tất cả các phòng ban đánh giá!", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                    return this.Direct(false);
                }
                else if (!objNew.Name.Trim().Equals("") && !humanAuditGradationSV.CheckNameExits(objNew.Name.Trim()))
                {
                    int AuditGradationId = humanAuditGradationSV.Insert(objNew, User.ID);
                    if (objectAudit.Length > 0)
                    {
                        var HumanAuditDepartmentItems = new List<HumanAuditDepartmentItem>();
                        foreach (var item in objectAudit)
                        {
                            var s = Ext.Net.JSON.Deserialize<HumanAuditDepartmentItem>(item.ToString());
                            s.HumanAuditGradationID = AuditGradationId;
                            s.HumanDepartmentID = s.HumanDepartmentID;
                            s.EmployeeAuditManagementID = s.EmployeeAuditManagementID;
                            s.EmployeeAuditLeaderID = s.EmployeeAuditLeaderID;
                            HumanAuditDepartmentItems.Add(s);
                        }
                        humanAuditEmployeeSV.InsertRange(HumanAuditDepartmentItems, User.ID);
                    }
                    if (val == true)
                    {
                        X.GetCmp<Window>("winNewAuditGradation").Close();
                    }
                    X.Msg.Notify("Thông báo", "Bạn đã thêm mới thành công!").Show();
                    X.GetCmp<Store>("stMnAuditGradation").Reload();
                    X.GetCmp<GridPanel>("grMnAuditGradation").Refresh();
                    return this.Direct(true);
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Thông báo",
                        Message = "Tên đợt đánh giá đã tồn tại vui lòng nhập tên khác!"
                    });
                    return this.Direct(false);
                }
            }
            catch
            {
                return this.Direct(false);
            }
        }
        public ActionResult Update(HumanAuditGradationItem objEdit, string arrObject)
        {
            try
            {
                List<HumanAuditDepartmentItem> objectAudit = new List<HumanAuditDepartmentItem>();
                objectAudit = Ext.Net.JSON.Deserialize<HumanAuditDepartmentItem[]>(arrObject).ToList();
                if (objectAudit.Count == 0)
                {
                    Ultility.CreateMessageBox(title: "Thông báo", message: "Bạn phải chọn phòng ban cần đánh giá!", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                    return this.Direct(false);
                }
                else if (!objEdit.Name.Trim().Equals("") && !humanAuditGradationSV.CheckNameEditExits(objEdit.ID, objEdit.Name.Trim()))
                {
                    humanAuditGradationSV.Update(objEdit, User.ID, objectAudit);
                    X.Msg.Notify("Thông báo", "Bạn đã cập nhật thành công!").Show();
                    X.GetCmp<Store>("stMnAuditGradation").Reload();
                    return this.Direct(true);
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Thông báo",
                        Message = "Tên đợt đánh giá đã tồn tại vui lòng nhập tên khác!"
                    });
                    return this.Direct(false);
                }
            }
            catch
            {
                return this.Direct(false);
            }
        }
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id)
        {
            try
            {
                humanAuditGradationSV.Delete(id);
                X.GetCmp<Store>("stRoleAudit").Reload();
                X.GetCmp<Store>("stCriterial").Reload();
                X.GetCmp<Store>("stMnAuditGradation").Reload();
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                return this.Direct();
            }
            catch
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "Thông báo",
                    Message = "Đợt đánh giá năng lực đã thực hiện không được xóa!"
                });
                return this.Direct("Error");
            }
        }
        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowDetail(int id)
        {
            var obj = humanAuditGradationSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Detail", Model = obj };
        }
        public ActionResult SendTickAudit(int AuditGradationId = 0)
        {

            try
            {
                if (!humanAuditGradationCriteriaSV.CheckCriterialExit(AuditGradationId))
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.WARNING,
                        Title = "Thông báo",
                        Message = "Có một chức danh đánh giá chưa thiết lập tiêu chí đánh giá yêu cầu kiểm tra lại!"
                    });
                    return this.Direct();
                }
                else
                {
                    NotifyController notify = new NotifyController();
                    var roleAudits = humanAuditTickSV.GetRoleAuditGradation(AuditGradationId);
                    var obj = humanAuditGradationSV.GetByID(AuditGradationId);
                    if (roleAudits.Count > 0)
                    {
                        foreach (var item in roleAudits)
                        {
                            var employees = humanAuditTickSV.GetEmployees(item.HumanRoleID);
                            if (employees.Count > 0)
                            {
                                foreach (var employee in employees)
                                {
                                    int tickId = humanAuditTickSV.Insert(item.GradationRoleID, User.ID, employee);
                                    notify.Notify(this.ModuleCode, "Bạn có một phiếu đánh giá nhân sự cần thực hiện", obj.Name, employee, User, Common.AuditEmployee, "AuditTickID:" + tickId.ToString());
                                }
                            }
                        }
                        humanAuditTickSV.SetPerform(AuditGradationId);
                    }
                    X.GetCmp<Store>("stMnAuditGradation").Reload();
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    return this.Direct();
                }
            }
            catch
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "Thông báo",
                    Message = "Gửi phiếu đánh giá bị lỗi!"
                });
                return this.Direct("Error");
            }
        }
        public ActionResult Department(string fn = "", bool multi = true, string departmentIds = "")
        {
            ViewData["Fn"] = fn;
            ViewData["Multi"] = multi;
            ViewData["DepartmentIDs"] = departmentIds;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewData = ViewData,
            };
        }
        public DirectResult HandleChangeRoleAudits(int id, string field, string oldValue, string newValue)
        {
            humanAuditGradationRoleSV.UpdateByID(id, field, oldValue, newValue);
            X.GetCmp<Store>("stRoleAudit").Reload();
            return this.Direct();
        }
        public ActionResult GetDataPoint(StoreRequestParameters parameters, int criteriaId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            List<HumanAuditGraditionCriteriaPointItem> lstData;
            lstData = humanAuditGradationCriteriaSV.GetByCateId(filter, criteriaId);
            return this.Store(lstData, filter.PageTotal);
        }
        #region Thêm mới tiêu chí đánh giá
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thêm mới tiêu chí")]
        public ActionResult FormAddRoleCriteria(int roleAuditId)
        {
            try
            {
                var data = new HumanAuditGradationCriteriaItem()
                {
                    HumanAuditGradationRoleID = roleAuditId,
                };
                return new Ext.Net.MVC.PartialViewResult { ViewName = "AddCriterial", Model = data };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult InsertCriteria(HumanAuditGradationCriteriaItem objNew, string strPoint)
        {
            try
            {
                List<HumanAuditGraditionCriteriaPointItem> lstPointItem = new List<HumanAuditGraditionCriteriaPointItem>();
                lstPointItem = Ext.Net.JSON.Deserialize<HumanAuditGraditionCriteriaPointItem[]>(strPoint).ToList();
                if (lstPointItem.Count > 0)
                {
                    if (lstPointItem.FirstOrDefault(x => string.IsNullOrEmpty(x.Name.Trim())) != null)
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.ERROR,
                            Title = "Thông báo",
                            Message = "Tên điểm đánh giá không được để trống!"
                        });
                        return this.Direct(false);
                    }
                }
                objNew.CreateBy = User.ID;
                if (humanAuditGradationCriteriaSV.Insert(objNew, User.ID, lstPointItem) != 0)
                {
                    if (objNew.InsertToCate)
                    {
                        humanCriteriaSV.InsertFromGradation(objNew, User.ID, lstPointItem);
                    }
                    X.GetCmp<FormPanel>("frCriteria").Reset();
                    X.GetCmp<Store>("stCriterial").Reload();
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    return this.Direct(true);
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Thông báo",
                        Message = "Tên tiêu chí đánh giá đã tồn tại!"
                    });
                    return this.Direct(false);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError);
                return this.Direct(false);
            }

        }
        #endregion
        #region Cập nhật tiêu chí đánh giá
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Sửa tiêu chí")]
        public ActionResult ShowUpdateCriteria(int id)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "EditCriterial", Model = humanAuditGradationCriteriaSV.GetById(id) };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult UpdateCriteria(HumanAuditGradationCriteriaItem objEdit, string strPoint)
        {
            try
            {
                List<HumanAuditGraditionCriteriaPointItem> lstPointItem = new List<HumanAuditGraditionCriteriaPointItem>();
                lstPointItem = Ext.Net.JSON.Deserialize<HumanAuditGraditionCriteriaPointItem[]>(strPoint).ToList();
                if (lstPointItem.Count > 0)
                {
                    if (lstPointItem.FirstOrDefault(x => string.IsNullOrEmpty(x.Name.Trim())) != null)
                    {
                        X.Msg.Show(new MessageBoxConfig
                        {
                            Buttons = MessageBox.Button.OK,
                            Icon = MessageBox.Icon.ERROR,
                            Title = "Thông báo",
                            Message = "Tên điểm đánh giá không được để trống!"
                        });
                        return this.Direct(false);
                    }
                }
                if (humanAuditGradationCriteriaSV.Update(objEdit, User.ID, lstPointItem))
                {
                    X.GetCmp<FormPanel>("frCriteria").Reset();
                    X.GetCmp<Store>("stCriterial").Reload();
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    return this.Direct(true);
                }
                else
                {
                    Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                    return this.Direct(false);
                }

            }
            catch
            {
                return this.Direct(false);
            }
        }
        #endregion
        #region Xóa bộ tiêu chí đánh giá
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xóa tiêu chí")]
        public ActionResult DeleteCriteria(int id)
        {
            try
            {
                humanAuditGradationCriteriaSV.Delete(id);
                X.GetCmp<Store>("stCriterial").Reload();
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                return this.Direct();
            }
            catch
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "Thông báo",
                    Message = "Tiêu chí đánh giá đã được sử dụng không được xóa!"
                });
                return this.Direct("Error");
            }
        }
        #endregion
        #region Xem chi tiết tiêu chí đánh giá
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem chi tiết tiêu chí")]
        public ActionResult ShowDetailCriteria(int id)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "DetailCriterial", Model = humanAuditGradationCriteriaSV.GetById(id) };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        #endregion
        public ActionResult AuditLevels(int id = 0)
        {
            ViewData["CateId"] = id;
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "AuditLevels", ViewData = ViewData };
        }
        public ActionResult GetAuditLevel(StoreRequestParameters parameters, int cateId = 0)
        {
            List<HumanAuditLevelItem> lstData;
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            lstData = levelSV.GetByCateId(filter, cateId);
            return this.Store(lstData, filter.PageTotal);
        }
        #region Thêm mới tiêu chí đánh giá
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Thêm mới hình thức xếp loại")]
        public ActionResult FormAddAuditLevel(int roleAuditId = 0)
        {
            try
            {
                var data = new HumanAuditLevelItem()
                {
                    HumanAuditGradationRoleID = roleAuditId,
                };
                return new Ext.Net.MVC.PartialViewResult { ViewName = "CreateAuditLevel", Model = data };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult InsertAuditLevel(HumanAuditLevelItem objNew)
        {
            try
            {
                objNew.CreateBy = User.ID;
                if (levelSV.Insert(objNew, User.ID) != 0)
                {
                    X.GetCmp<FormPanel>("frAuditLevel").Reset();
                    X.GetCmp<Store>("stAuditLevel").Reload();
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Thông báo",
                        Message = "Tên hình thức xếp loại đã tồn tại!"
                    });
                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError);
            }
            return this.Direct();
        }
        #endregion
        #region Cập nhật tiêu chí đánh giá
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Sửa hình thức xếp loại")]
        public ActionResult ShowUpdateAuditLevel(int id)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "EditAuditLevel", Model = levelSV.GetById(id) };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult UpdateAuditLevel(HumanAuditLevelItem objEdit)
        {
            try
            {
                if (levelSV.Update(objEdit, User.ID))
                {
                    X.GetCmp<FormPanel>("frAuditLevel").Reset();
                    X.GetCmp<Store>("stAuditLevel").Reload();
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                }
                else
                {
                    Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                }
                return this.Direct();
            }
            catch
            {
                return this.Direct("Error");
            }
        }
        #endregion
        #region Xem chi tiết tiêu chí đánh giá
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xem chi tiết hình thức xếp loại")]
        public ActionResult ShowDetailAuditLevel(int id)
        {
            try
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "DetailAuditLevel", Model = levelSV.GetById(id) };
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
        #endregion
        #region Xóa bộ tiêu chí đánh giá
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Xóa hình thức xếp loại")]
        public ActionResult DeleteAuditLevel(int id)
        {
            try
            {
                levelSV.Delete(id);
                X.GetCmp<Store>("stAuditLevel").Reload();
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                return this.Direct();
            }
            catch
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "Thông báo",
                    Message = "Đã được sử dụng không được xóa!"
                });
                return this.Direct("Error");
            }
        }
        #endregion
    }
}
