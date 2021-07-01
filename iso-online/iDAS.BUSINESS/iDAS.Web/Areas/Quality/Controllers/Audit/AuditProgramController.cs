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
using iDAS.Web.Controllers;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Quality.Controllers
{
    [BaseSystem(Name = "Chương trình đánh giá", Icon = "ScriptEdit", IsActive = true, IsShow = true, Position = 3, Parent = GroupMenuAuditController.Code)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class AuditProgramController : BaseController
    {
        //
        // GET: /Quality/AuditProgram/
        private QualityAuditEmployeeSV auditObjectSV = new QualityAuditEmployeeSV();
        private QualityAuditProgramSV auditProgramSV = new QualityAuditProgramSV();
        private QualityAuditRecordedVoteSV qualityAuditRecordedVoteSV = new QualityAuditRecordedVoteSV();
        private QualityAuditProgramISOIndexSV qualityAuditProgramISOSV = new QualityAuditProgramISOIndexSV();
        private QualityAuditEmployeeSV auditEmployeeSV = new QualityAuditEmployeeSV();
        private QualityAuditDepartmentSV auditDepartmentSV = new QualityAuditDepartmentSV();
        private ISOClauseSV iSOStandardSV = new ISOClauseSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index(int auditPlanId = 0, int focusId = 0)
        {
            ViewBag.FocusId = focusId;
            return View();
        }
        public ActionResult GetDataISOIndex(StoreRequestParameters parameters, int isoId)
        {
            List<CenterISOClauseItem> lstData;
            lstData = iSOStandardSV.GetAll(isoId);
            return this.Store(lstData);
        }
        public ActionResult GetDataISOIndexEdit(StoreRequestParameters parameters, int isoId, int programId)
        {
            List<CenterISOClauseItem> lstData;
            lstData = iSOStandardSV.GetAllEdit(isoId, programId);
            return this.Store(lstData);
        }
        public ActionResult GetData(StoreRequestParameters parameters, int auditPlanId = 0, int focusId = 0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var Task = auditProgramSV.GetAll(filter, auditPlanId, focusId);
            return this.Store(Task, filter.PageTotal);
        }
        public ActionResult FormListObject()
        {
            return View("LstObject");
        }
        public ActionResult ShowFrmUpdateObject(int employeeId, string auditor, string email, string phone, Guid? fileId, string fileName, bool isCaptain, string role, bool isAuditor)
        {
            var obj = new QualityAuditEmployeeItem();
            obj.Email = email;
            obj.EmployeeID = employeeId;
            obj.EmployeeName = auditor;
            obj.Phone = phone;
            obj.IsAuditor = isAuditor;
            obj.IsCaptain = isCaptain;
            obj.Role = role;
            obj.FileID = fileId;
            obj.FileName = fileName;
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "UpdateObject", Model = obj };
        }
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmAdd(int auditPlanId)
        {
            var qualityAudit = new QualityAuditPlanSV().GetById(auditPlanId);
            var result = new QualityAuditProgramItem()
            {
                StartTimePlan = qualityAudit.StartTime.Date,
                EndTimePlan = qualityAudit.EndTime.Date,
            };
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Create", ViewData = new ViewDataDictionary { { "auditPlanId", auditPlanId } }, Model = result };
        }
        public ActionResult ShowSendApproval(int id)
        {
            var obj = auditProgramSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "SendApproval", Model = obj };
        }
        [BaseSystem(Name = "Sửa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmUpdate(int id)
        {
            var obj = auditProgramSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Update", Model = obj };
        }
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id)
        {
            try
            {
                auditProgramSV.Delete(id);
                X.GetCmp<Store>("stMnAuditProgram").Reload();
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess, error: true);
                throw;
            }
            return this.Direct();
        }
        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmDetail(int id)
        {
            var obj = auditProgramSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "Detail", Model = obj };
        }
        public ActionResult SendApprove(QualityAuditProgramItem obj)
        {
            try
            {
                auditProgramSV.SendApproveProgram(obj, User.ID);
                NotifyController notify = new NotifyController();
                notify.Notify(this.ModuleCode, "Có một chương trình đánh giá nội bộ cần phê duyệt", obj.Name, obj.Approval.ID, User, Common.AuditProgram, "AuditProgramID:" + obj.ID.ToString());
                X.GetCmp<Store>("stMnAuditProgram").Reload();
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
            var obj = auditProgramSV.GetByID(id);
            if (obj.ApprovalBy != User.EmployeeID)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.WARNING,
                    Title = "Thông báo",
                    Message = "Bạn không có quyền phê duyệt chương trình này!"
                });
            }
            else
            {
                return new Ext.Net.MVC.PartialViewResult() { ViewName = "Approval", Model = obj };
            }
            return this.Direct();
        }
        public ActionResult Insert(QualityAuditProgramItem objNew, bool val, string listISOIndex, string listDepartmentId, string arrObject)
        {
            try
            {
                var objectAudit = Ext.Net.JSON.Deserialize<object[]>(arrObject);
                if (objNew.Approval == null || objNew.Approval.ID == 0)
                {
                    Ultility.CreateMessageBox(title: "Thông báo", message: "Bạn phải chọn người phê duyệt chương trình!", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                    return this.Direct();
                }
                if (listISOIndex != null)
                {
                    if (listDepartmentId == null)
                    {
                        Ultility.CreateMessageBox(title: "Thông báo", message: "Bạn phải chọn phòng ban tham gia!", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                        return this.Direct();
                    }
                }
                if (objectAudit.Length == 0)
                {
                    Ultility.CreateMessageBox(title: "Thông báo", message: "Bạn phải chọn thành phần tham gia đánh giá!", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                    return this.Direct();
                }
                else
                {
                    int auditProgramId = auditProgramSV.InsertProgram(objNew, User.ID);
                    if (listISOIndex.Length > 0)
                    {
                        var arrIds = Ext.Net.JSON.Deserialize<string[]>(listISOIndex);
                        if (arrIds.Length > 0)
                        {
                            var qualityAuditProgramISOIndexItems = new List<QualityAuditProgramISOIndexItem>();
                            foreach (var arr in arrIds)
                            {
                                var qualityAuditProgramISOIndexItem = new QualityAuditProgramISOIndexItem();
                                qualityAuditProgramISOIndexItem.ISOIndexID = int.Parse(arr);
                                qualityAuditProgramISOIndexItem.QualityAuditProgramID = auditProgramId;
                                qualityAuditProgramISOIndexItems.Add(qualityAuditProgramISOIndexItem);
                            }
                            qualityAuditProgramISOSV.InsertRange(qualityAuditProgramISOIndexItems, User.ID);
                        }
                    }
                    if (listDepartmentId.Length > 0)
                    {
                        var arrIdDeparts = listDepartmentId.Substring(0, listDepartmentId.Length - 1).Split(',');
                        if (arrIdDeparts.Length > 0)
                        {
                            var AuditDepartmentItems = new List<QualityAuditDepartmentItem>();
                            foreach (var arr in arrIdDeparts)
                            {
                                var AuditDepartmentItem = new QualityAuditDepartmentItem();
                                AuditDepartmentItem.ID = int.Parse(arr);
                                AuditDepartmentItem.AuditProgramID = auditProgramId;
                                AuditDepartmentItems.Add(AuditDepartmentItem);
                            }
                            auditDepartmentSV.InsertRange(AuditDepartmentItems, User.ID);
                        }
                    }
                    if (objectAudit.Length > 0)
                    {
                        var QualityAuditEmployeeItems = new List<QualityAuditEmployeeItem>();
                        foreach (var item in objectAudit)
                        {
                            var s = Ext.Net.JSON.Deserialize<QualityAuditEmployeeItem>(item.ToString());
                            s.AuditProgramID = auditProgramId;
                            s.EmployeeID = s.ID;
                            s.IsCaptain = s.IsCaptain;
                            s.IsAuditor = s.IsAuditor;
                            var QualityAuditEmployeeItem = new QualityAuditEmployeeItem();
                            QualityAuditEmployeeItem.AuditProgramID = auditProgramId;
                            QualityAuditEmployeeItems.Add(s);
                        }
                        auditEmployeeSV.InsertRange(QualityAuditEmployeeItems, User.ID);
                    }
                    if (val)
                    {
                        X.GetCmp<Window>("winNewAuditProgram").Close();
                    }
                    X.GetCmp<Store>("stMnAuditProgram").Reload();
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                    return this.Direct();
                }
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                return this.Direct();
            }
        }
        public ActionResult Update(QualityAuditProgramItem objEdit, string listISOIndex, string listDepartmentId, string listDepartmentRemoveId, string arrObject)
        {
            try
            {
                var objectAudit = Ext.Net.JSON.Deserialize<object[]>(arrObject);
                if (objEdit.Approval == null || objEdit.Approval.ID == 0)
                {
                    Ultility.CreateMessageBox(title: "Thông báo", message: "Bạn phải chọn người phê duyệt chương trình!", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                    return this.Direct(false);
                }
                else if (objectAudit.Length == 0)
                {
                    Ultility.CreateMessageBox(title: "Thông báo", message: "Bạn phải chọn thành phần tham gia!", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                    return this.Direct(false);
                }
                else if (!auditDepartmentSV.CheckExitDepartment(objEdit.ID, listDepartmentId, listDepartmentRemoveId))
                {
                    Ultility.CreateMessageBox(title: "Thông báo", message: "Bạn phải chọn phòng ban tham gia!", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                    return this.Direct(false);
                }
                else
                {
                    auditProgramSV.Update(objEdit, User.ID);
                    if (listISOIndex.Length > 0)
                    {
                        var arrIds = Ext.Net.JSON.Deserialize<int[]>(listISOIndex);
                        if (arrIds.Length > 0)
                        {
                            qualityAuditProgramISOSV.UpdateRange(arrIds, objEdit.ID, User.ID);
                        }
                    }
                    else
                    {
                        qualityAuditProgramISOSV.RemoveAllClause(objEdit.ID);
                    }
                    auditDepartmentSV.UpdateRange(listDepartmentId, listDepartmentRemoveId, objEdit.ID, User.ID);
                    if (objectAudit.Length > 0)
                    {
                        var QualityAuditEmployeeItems = new List<QualityAuditEmployeeItem>();
                        foreach (var item in objectAudit)
                        {
                            var s = Ext.Net.JSON.Deserialize<QualityAuditEmployeeItem>(item.ToString());
                            s.AuditProgramID = objEdit.ID;
                            s.EmployeeID = s.EmployeeID;
                            s.IsAuditor = s.IsAuditor;
                            s.IsCaptain = s.IsCaptain;
                            var QualityAuditEmployeeItem = new QualityAuditEmployeeItem();
                            QualityAuditEmployeeItem.AuditProgramID = objEdit.ID;
                            QualityAuditEmployeeItems.Add(s);
                        }
                        auditEmployeeSV.UpdateRange(QualityAuditEmployeeItems, objEdit.ID, User.ID);
                    }
                    X.GetCmp<Store>("stMnAuditProgram").Reload();
                    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                    return this.Direct(true);
                }
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                return this.Direct();
            }
        }
        public ActionResult ApproveProgram(QualityAuditProgramItem obj)
        {
            try
            {
                auditProgramSV.Approve(obj, User.ID);
                if (obj.CreateByEmployeeID != null)
                {
                    NotifyController notify = new NotifyController();
                    notify.Notify(this.ModuleCode, "Có một chương trình đánh giá nội bộ đã được phê duyệt", obj.Name, obj.CreateByEmployeeID.Value, User, Common.AuditProgram, "AuditProgramID:" + obj.ID.ToString());
                }
                X.GetCmp<Store>("stMnAuditProgram").Reload();
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                throw ex;
            }
            return this.Direct();
        }
        [BaseSystem(Name = "Cập nhật thành phần vắng mặt", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmUpdateAbsent(int id)
        {
            var obj = auditObjectSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "EditAbsent", Model = obj };
        }
        [BaseSystem(Name = "Xem chi tiết thành phần vắng mặt", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmDetailAbsent(int id)
        {
            var obj = auditObjectSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "DetailAbsent", Model = obj };
        }
        public ActionResult UpdateAbsent(QualityAuditEmployeeItem obj)
        {
            auditObjectSV.Update(obj, User.ID);
            return this.Direct();
        }
        public ActionResult ShowFrmAbsent(int id)
        {
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "UpdateAbsent", ViewData = new ViewDataDictionary() { { "programId", id } } };
        }

        #region Phiếu ghi chép
        [BaseSystem(Name = "Xem danh sách phiếu ghi chép", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ResultVoteForm(int programId, string programName)
        {
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewName = "ResultVote",
                Model = new QualityAuditRecordedVoteItem()
                {
                    QualityAuditProgramID = programId,
                    QualityAuditProgram = programName
                }
            };
        }
        public ActionResult LoadProgramVote(StoreRequestParameters parameters, int programId)
        {
            int total;
            var lstData = qualityAuditProgramISOSV.GetProgramVote(parameters.Page, parameters.Limit, out total, programId);
            return this.Store(lstData, total);
        }
        [BaseSystem(Name = "Thêm phiếu ghi chép", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult InsertVoteForm(int auditProgramId)
        {
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewName = "CreateVote",
                Model = new QualityAuditRecordedVoteItem()
                {
                    QualityAuditProgramID = auditProgramId,
                    Auditer = new HumanEmployeeSV().GetEmployeeViewByUser(User.ID),
                    AuditAt = DateTime.Now,
                }
            };
        }
        public ActionResult InsertVotePoint(QualityAuditRecordedVoteItem addVote)
        {
            try
            {
                qualityAuditProgramISOSV.InsertVote(addVote, User.ID);
                X.GetCmp<Store>("stRecordedVotes").Reload();
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                return this.Direct();
            }
            catch(Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                return this.Direct();
            }
        }
        public ActionResult LoadDeparment(int programId)
        {
            var result = qualityAuditProgramISOSV.GetDepartmentByProgram(programId);
            return this.Store(result, result.Count);
        }
        public ActionResult LoadISOIndex(int programId)
        {
            var result = qualityAuditProgramISOSV.GetIsoIndex(programId);
            return this.Store(result);
        }
        /// <summary>
        /// Lấy danh sách module theo của chương trình đánh giá
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public ActionResult LoadModule(int programId)
        {
            var result = qualityAuditProgramISOSV.GetModuleByProgramId(programId);
            return this.Store(result);
        }
        /// <summary>
        /// Lấy danh sách chức năng theo của chương trình đánh giá
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public ActionResult LoadFunction(string moduleCode = "")
        {
            var result = qualityAuditProgramISOSV.GetFunctionByModuleCode(moduleCode);
            return this.Store(result);
        }
        [BaseSystem(Name = "Sửa phiếu ghi chép", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateVoteForm(int id)
        {
            QualityAuditRecordedVoteItem result = qualityAuditProgramISOSV.GetRecordVote(id);
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewName = "UpdateVote",
                Model = result
            };
        }
        /// <summary>
        /// Cập nhật phiếu đánh giá
        /// </summary>
        /// <param name="objEdit"></param>
        /// <returns></returns>
        public ActionResult UpdateVotePoint(QualityAuditRecordedVoteItem objEdit)
        {
            try
            {
                qualityAuditProgramISOSV.UpdateVote(objEdit, User.ID);
                X.GetCmp<Store>("stRecordedVotes").Reload();
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                return this.Direct();
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
                return this.Direct();
            }
        }
        [BaseSystem(Name = "Xóa phiếu ghi chép", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult DeleteVote(int id)
        {
            try
            {
                qualityAuditProgramISOSV.DeleteVote(id);
                X.GetCmp<Store>("stRecordedVotes").Reload();
                Ultility.CreateNotification(message: RequestMessage.SendSuccess);
                return this.Direct(true);
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.SendError, error: true);
                return this.Direct(false);
            }
        }
        [BaseSystem(Name = "Xem chi tiết phiếu ghi chép", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        // Xem phiếu đánh giá
        public ActionResult DetailVoteForm(int id)
        {
            QualityAuditRecordedVoteItem result = qualityAuditProgramISOSV.GetRecordVote(id);
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewName = "DetailVote",
                Model = result
            };
        }
        public ActionResult VerifyVote(string ids)
        {
            var lstRecordVote = new List<int>();
            try
            {
                lstRecordVote = ids.Split(',').Select(i => System.Convert.ToInt32(i)).ToList();
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.SendError, error: true);
            }
            qualityAuditProgramISOSV.VerifyVote(lstRecordVote, User.ID);
            X.GetCmp<Store>("stRecordedVotes").Reload();
            return this.Direct();
        }
        #endregion
    }
}
