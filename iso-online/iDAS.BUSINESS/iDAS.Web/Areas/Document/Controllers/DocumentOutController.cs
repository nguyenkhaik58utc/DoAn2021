using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using iDAS.Utilities;
using iDAS.Web.API;
using iDAS.Web.Areas.Document.Models;
using iDAS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Document.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Tài liệu bên ngoài", Icon = "Page", IsActive = true, IsShow = true, Position = 2, Parent = GroupDocumentsController.Code)]
    public class DocumentOutController : BaseController
    {
        private DocumentAPI api = new DocumentAPI();

        DocumentSV documentSV = new DocumentSV();
        DocumentDistributionSV documentDistributionSV = new DocumentDistributionSV();
        DocumentCategorySV documentCateSV = new DocumentCategorySV();
        DocumentDistributionSV distributionSV = new DocumentDistributionSV();
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            ViewData["UserLogOn"] = User.EmployeeID;
            return View();
        }
        public ActionResult LoadData(StoreRequestParameters parameters, int folderID = 0)
        {
            //ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            //var lst = documentSV.GetAllTypeOutByDepartmentID(filter, groupId, focusId);
            //return this.Store(lst, filter.PageTotal);

            var resp = api.GetData(parameters.Page, parameters.Limit, folderID, User.ID, User.EmployeeID, true);
            var lstDocument = resp.Data;// tai lieu noi bo
            for (int i = 0; i < lstDocument.Count; i++)
            {
                int code = 0;
                lstDocument[i].Status = documentSV.getStatus(lstDocument[i].IsEdit, lstDocument[i].IsApproval, lstDocument[i].IsAccept, lstDocument[i].IsPublic, lstDocument[i].IsObsolete, ref code, lstDocument[i].ParentID, true);
                lstDocument[i].StatusCode = code;

                lstDocument[i].FlagModified = false;
                if (lstDocument[i].IsPublic && !lstDocument[i].IsObsolete && lstDocument[i].ParentID > 0)
                    lstDocument[i].FlagModified = true;
            }

            return this.Store(new Paging<DocumentList>(lstDocument, resp.TotalRows.Value));
        }
        [BaseSystem(Name = "Thêm ")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Insert(int folderID)
        {
            DocumentItem d = new DocumentItem();
            d.FolderID = folderID;
            d.TypeID = (int)DocumentType.Out;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Insert", Model = d };
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Insert(DocumentItem obj, [Bind(Prefix = "StorageFormID")]string[] borders, bool IsSendApproval = false)
        {
            setDocIssForm(borders, ref obj);
            obj.CreateBy = User.ID;
            obj.IsEdit = !IsSendApproval;
            int id = insert(obj);
            if (id > 0)
            {
                if (IsSendApproval)
                {
                    NotifyController notify = new NotifyController();
                    notify.Notify(this.ModuleCode, "Có một tài liệu yêu cầu phê duyệt", obj.Name, obj.Approval.ID, User, Common.DocumentOut, "DocumentID:" + id.ToString());
                }
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                X.GetCmp<Store>("StDocument").Reload();
                X.GetCmp<Hidden>("hdID").Text = id.ToString();
                X.GetCmp<Window>("winAddDocument").Close();
            }
            return this.Direct();
        }
        public ActionResult GetDataCboType()
        {
            var lst = new List<DocumentSecurityItem> {
             new DocumentSecurityItem{ID= (int)DocumentType.Out, Name="Bên ngoài"},
             new DocumentSecurityItem{ID= (int)DocumentType.In, Name="Nội bộ"}
            };
            return this.Store(lst);
        }
        [BaseSystem(Name = "Sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Update(int id)
        {
            var obj = documentSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = obj };
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(DocumentItem obj, [Bind(Prefix = "StorageFormID")]string[] borders, bool IsSendApproval = false)
        {
            setDocIssForm(borders, ref obj);
            obj.UpdateBy = User.ID;
            obj.IsEdit = !IsSendApproval;
            if (update(obj))
            {
                if (IsSendApproval)
                {
                    NotifyController notify = new NotifyController();
                    notify.Notify(this.ModuleCode, "Có một tài liệu yêu cầu phê duyệt", obj.Name, obj.Approval.ID, User, Common.DocumentOut, "DocumentID:" + obj.ID.ToString());
                }
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                X.GetCmp<Store>("StDocument").Reload();
                X.GetCmp<Window>("winEditDocument").Close();
            }
            return this.Direct();
        }
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id)
        {
            try
            {
                documentSV.Delete(id);
                Ultility.CreateNotification(message: RequestMessage.DeleteSuccess);
                X.GetCmp<Store>("StDocument").Reload();
            }
            catch (Exception)
            {
                Ultility.CreateNotification(message: RequestMessage.DeleteError);
            }
            return this.Direct();
        }
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Detail(int id)
        {
            var obj = documentSV.GetDetailByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
        }
        public ActionResult LoadDocumentIssued(StoreRequestParameters parameters, int docID = 0)
        {
            int totalCount;
            var lst = documentDistributionSV.GetDetailByDocID(parameters.Page, parameters.Limit, out totalCount, docID);
            return this.Store(lst, totalCount);
        }
        [BaseSystem(Name = "Phê duyệt")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Approve(int id)
        {
            var objApp = documentSV.GetDetailByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Approve", Model = objApp };
        }
        [HttpPost]
        public ActionResult Approve(DocumentItem obj)
        {
            obj.UpdateBy = User.ID;
            if (updateApprove(obj))
            {
                NotifyController notify = new NotifyController();
                notify.Notify(this.ModuleCode, "Có một tài liệu đã được phê duyệt", obj.Name, obj.EmployeesCreateID, User, Common.DocumentOut, "DocumentID:" + obj.ID.ToString());
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                X.GetCmp<Store>("StDocument").Reload();
                X.GetCmp<Window>("winApprovalDocument").Close();
            }
            else
                Common.ShowExtMsg(MessageBox.Button.OK, MessageBox.Icon.ERROR, "Lỗi", "Cập nhật dữ liệu không thành công!");

            return this.Direct();

        }
        [BaseSystem(Name = "Ban hành")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Issued(int id)
        {
            var obj = documentSV.GetDetailByID(id);
            obj.StatusApprove = Common.DocumentStatus.Issued.ToString();
            return new Ext.Net.MVC.PartialViewResult { ViewName = "IssuedUpdate", Model = obj };
        }
        public ActionResult IssuedUpdate(DocumentItem obj)
        {
            obj.UpdateBy = User.ID;
            string ck = updateIssued(obj);
            if (ck.Trim().Equals(""))
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                X.GetCmp<Store>("StDocument").Reload();
                X.GetCmp<Window>("winApprovalDocumentUpdate").Close();
            }
            else
                Common.ShowExtMsg(MessageBox.Button.OK, MessageBox.Icon.ERROR, "Lỗi", ck);
            return this.Direct();
        }
        [BaseSystem(Name = "Sửa đổi")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Modified(int id)
        {
            var obj = documentSV.GetByID(id);
            if (obj.FlagModified == true)
            {
                Ultility.ShowMessageBox("Thông báo", "Tài liệu ban hành được chọn đã có Tài liệu sửa đổi!", MessageBox.Icon.WARNING, MessageBox.Button.OK);
                return this.Direct();
            }
            if (obj.IssuedTime != null)
                obj.IssuedTime = (int)obj.IssuedTime + 1;
            obj.IsAccept = false;
            obj.IsEdit = true;
            obj.IsApproval = false;
            return new Ext.Net.MVC.PartialViewResult { ViewName = "ModifiedAdd", Model = obj };
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult InsertDocument(DocumentItem obj, [Bind(Prefix = "StorageFormID")]string[] borders, bool IsSendApproval = false)
        {
            try
            {
                setDocIssForm(borders, ref obj);
                obj.CreateBy = User.ID;
                obj.IsEdit = !IsSendApproval;
                int id = insert(obj);
                if (IsSendApproval)
                {
                    NotifyController notify = new NotifyController();
                    notify.Notify(this.ModuleCode, "Có một tài liệu bên ngoài sửa đổi yêu cầu phê duyệt", obj.Name, obj.Approval.ID, User, Common.DocumentOut, "DocumentID:" + id.ToString());
                }
                X.GetCmp<Store>("StDocument").Reload();
                X.GetCmp<Window>("winAddDocument").Close();
                Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
            }
            catch (Exception)
            {
                return this.Direct();
            }
            return this.Direct();
        }
        [BaseSystem(Name = "Phân phối")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Distribution(int id)
        {
            var obj = documentSV.GetByID(id);
            var objN = new DocumentDistributionItem
            {
                DocumentID = obj.ID
            };
            return new Ext.Net.MVC.PartialViewResult { ViewName = "DistributionList", Model = objN };
        }
        public ActionResult AddDistribution(int DocID)
        {
            var obj = documentSV.GetByID(DocID);
            var objN = new DocumentDistributionItem
            {
                DocumentID = obj.ID
            };
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Distribution", Model = objN };
        }
        public ActionResult UpdateDistribution(int id)
        {
            var obj = distributionSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "UpdateDistribution", Model = obj };
        }
        public ActionResult UpdateIssue(DocumentDistributionItem obj, bool isExit)
        {

            obj.UpdateBy = User.ID;
            obj.CreateBy = User.ID;
            int idIns = 0;
            if (obj.Departments != null && obj.Departments.IDs.Count > 0)
            {
                foreach (int DepID in obj.Departments.IDs)
                {
                    obj.DepartmentID = DepID;
                    var objExist = distributionSV.GetByDepIDDocID(DepID, obj.DocumentID.Value);
                    if (objExist == null)
                    {
                        updateObsolete(obj, ref idIns, false);
                    }
                }
            }
            else
                updateObsolete(obj, ref idIns, false);
            Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);

            //if (updateObsolete(obj, ref idIns, false))
            //{
            //    Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
            //    if (idIns > 0) X.GetCmp<Hidden>("hrID").SetValue(idIns);
            //}
            X.GetCmp<Store>("StoreDocumentDistributionList").Reload();
            return this.Direct();

        }
        public ActionResult GetObjByDepartment(int id, int docID)
        {
            var obj = distributionSV.GetByDepIDDocID(id, docID);
            var rs = new JsonResult();
            if (obj != null)
            {
                rs.Data = obj;
            }
            else
            {
                DocumentItem objN = documentSV.GetByID(docID);
                rs.Data = new DocumentDistributionItem { DocumentID = docID, ID = 0, DepartmentID = id, IssuedDate = objN.IssuedDate };
            }

            return rs;
        }
        [BaseSystem(Name = "Thu hồi")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult Obsolete(int id)
        {
            var obj = documentSV.GetByID(id);
            var objN = new DocumentDistributionItem
            {
                Number = 0,
                DistributionDate = null,
                DocumentID = obj.ID
            };
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Obsolete", Model = objN };
        }
        public ActionResult UpdateObs(DocumentDistributionItem obj, bool isExit)
        {

            obj.UpdateBy = User.ID;
            obj.CreateBy = User.ID;
            int id = 0;
            if (updateObsolete(obj, ref id, true))
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                // X.GetCmp<Store>("StDocument").Reload();
            }
            return this.Direct();

        }
        [BaseSystem(Name = "Phê duyệt thu hồi")]
        [SystemAuthorize(CheckAuthorize = true)]
        [HttpGet]
        public ActionResult ApproveObsolete(int id)
        {
            var obj = documentSV.GetByID(id);
            obj.StatusApprove = "Lỗi thời";
            return new Ext.Net.MVC.PartialViewResult { ViewName = "ApproveObsolete", Model = obj };
        }
        public ActionResult ShowFrmDocCate(int id = 0)
        {
            var obj = documentSV.GetByID(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "ChoseDocumentCate", Model = obj };
        }
        public ActionResult LoadCateByDepartment(StoreRequestParameters parameters, int departmentID = 0)
        {
            int totalCount;
            var lst = documentCateSV.GetAll(parameters.Page, parameters.Limit, out totalCount, departmentID);
            return this.Store(lst, totalCount);
        }
        private string checkValid(DocumentItem objDraft, int id = 0)
        {
            bool ckexit = true;
            objDraft.Code = objDraft.Code.Trim();
            objDraft.Version = objDraft.Version.Trim();
            if (objDraft.ParentID > 0)
                return "";
            if (id > 0)
            {
                var docItem = documentSV.GetByID(id);
                if (docItem != null)
                {
                    if (docItem.Code.Trim().ToUpper().Equals(objDraft.Code.ToUpper()))
                        ckexit = false;
                }
            }
            if (ckexit)
            {
                var doc = documentSV.GetByCodeVerson(objDraft.Code);
                if (doc != null)
                {
                    return "Mã Tài liệu đã tồn tại trong hệ thống! Vui lòng nhập mã khác.";
                }
            }
            if (CheckValidation.HasSpecialCharacters(objDraft.Code.Trim(), "@#$%^&*()~"))
                return "Mã Tài liệu không được chứa ký tự đặc biệt! Vui lòng nhập mã khác.";
            return "";
        }
        private int insert(DocumentItem objDraft)
        {
            try
            {
                int id = 0;
                string ck = checkValid(objDraft);
                if (ck.Trim().Equals(""))
                {
                    id = documentSV.Insert(objDraft, User.EmployeeID);
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Thông báo",
                        Message = ck
                    });
                }
                return id;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        private bool update(DocumentItem objDraft)
        {
            try
            {
                string ck = checkValid(objDraft, objDraft.ID);
                if (ck.Trim().Equals(""))
                {
                    objDraft.UpdateBy = User.ID;
                    documentSV.Update(objDraft, User.EmployeeID);
                    return true;
                }
                else
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.ERROR,
                        Title = "Thông báo",
                        Message = ck

                    });
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        private bool updateApprove(DocumentItem objDraft)
        {
            try
            {
                documentSV.UpdateApprove(objDraft);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private string updateIssued(DocumentItem objDraft)
        {
            try
            {
                if (objDraft.StatusApprove.Equals(iDAS.Utilities.Common.DocumentStatus.Issued.ToString()))
                {
                    var lst = documentSV.GetByParentID(objDraft.ID);
                    if (lst != null && lst.Count() > 0)
                    {
                        return "Đã có Tài liệu sửa đổi thay thế Tài liệu này nên không được phép chuyển trạng thái Ban hành!";
                    }
                }
                else if (objDraft.StatusApprove.Equals(iDAS.Utilities.Common.DocumentStatus.Obsolete.ToString()))
                {
                    var obj = documentSV.GetByID(objDraft.ID);
                    if (obj != null && obj.IsApproval && obj.IsAccept && !obj.IsPublic)
                    {
                        return "Không cho phép chuyển tài liệu có trạng thái Duyệt sang trạng thái Lỗi thời! Vui lòng kiểm tra lại!";
                    }
                }
                documentSV.UpdateIssued(objDraft);
                return "";
            }
            catch (Exception)
            {
                return "";
            }
        }
        private bool updateObsolete(DocumentDistributionItem objDraft, ref int idinsert, bool isObs = false)
        {
            try
            {
                string msg = "";
                if (isObs && (objDraft.FormHO == null || objDraft.FormHO == false) && (objDraft.FormSO == null || objDraft.FormSO == false))
                    msg = "Hình thức thu hồi Tài liệu bắt buộc phải chọn!";
                else if (!isObs && objDraft.FormH == false && objDraft.FormS == false)
                    msg = "Hình thức phân phối Tài liệu bắt buộc phải chọn!";
                else if (!isObs && objDraft.DistributionDate < objDraft.IssuedDate)
                    msg = "Ngày phân phối phải lớn hơn hoặc bằng Ngày ban hành Tài liệu (" + ((DateTime)objDraft.IssuedDate).ToString("dd/MM/yyyy") + ")!";
                else if (isObs && objDraft.NumberObsolete > objDraft.Number)
                    msg = "Số lượng thu hồi phải nhỏ hơn hoặc bằng Số lượng phân phối Tài liệu!";
                else if (isObs && objDraft.ObsoleteDate < objDraft.DistributionDate)
                    msg = "Ngày thu hồi phải lớn hơn hoặc bằng Ngày phân phối Tài liệu!";
                if (!msg.Trim().Equals(""))
                {
                    Common.ShowExtMsg(MessageBox.Button.OK, MessageBox.Icon.ERROR, "Thông báo", msg);
                    return false;
                }
                if (objDraft.DocumentDistributionID > 0 && isObs)
                    distributionSV.UpdateObsolete(objDraft);
                else if (objDraft.DocumentDistributionID > 0 && !isObs)
                    distributionSV.UpdateDistribution(objDraft);
                else
                    idinsert = distributionSV.Insert(objDraft);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private int setDocIssForm(string[] doc, ref DocumentItem obj)
        {
            if (doc != null)
            {
                if (doc.Count() == 2)
                {
                    obj.FormH = true;
                    obj.FormS = true;
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
        public ActionResult GetObjObsoleteByDepartment(int id, int docID)
        {
            var obj = distributionSV.GetByDepIDDocID(id, docID, true);
            var rs = new JsonResult();
            if (obj != null)
            {
                rs.Data = obj;
            }
            else
                rs.Data = new DocumentDistributionItem { DocumentID = docID, ID = 0, DepartmentID = id, DistributionDate = null };
            return rs;
        }
        public ActionResult GetDataCate()
        {
            DocumentCategorySV documentCategorySV = new DocumentCategorySV();
            var lst = documentCategorySV.GetAll();
            return this.Store(lst);
        }
        public ActionResult GetDataCboSecurity()
        {
            DocumentSecuritySV docSercuritySV = new DocumentSecuritySV();
            var lst = docSercuritySV.GetAll();
            return this.Store(lst);
        }
        public ActionResult GetDataCboPersonApprove()
        {
            HumanEmployeeSV employeeSV = new HumanEmployeeSV();
            var lst = employeeSV.GetAllCbo();
            return this.Store(lst);
        }
        public ActionResult LoadDocumentDistributionList(int docId)
        {
            var lst = distributionSV.GetDepDistribuByDocID(docId);
            return this.Store(lst, lst.Count);
        }
    }
}
