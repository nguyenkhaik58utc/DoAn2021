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
using System.IO;
using iDAS.Utilities;
using iDAS.Web.Controllers;
using System.Xml;
using System.Xml.Xsl;

namespace iDAS.Web.Areas.Suppliers.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    [BaseSystem(Name = "Đề nghị mua hàng hóa", Icon = "ApplicationEdit", IsActive = true, IsShow = true, Position = 1, Parent = GroupOrdersController.Code)]
    public class SuppliersOrdersController : BaseController
    {
        //
        // GET: /Suppliers/SuppliersOrders/
        private SuppliersOrderRequirementSV SuppOrderSV = new SuppliersOrderRequirementSV();
        private SuppliersOrderRequirementDetailSV SuppOrderDetailSV = new SuppliersOrderRequirementDetailSV();
        private StockProductGroupSV product_GroupService = new StockProductGroupSV();
        private StockProductSV productService = new StockProductSV();


        public static List<SuppliersOrderRequirementDetailItem> lstOrderDetail = new List<SuppliersOrderRequirementDetailItem>();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index(int focusId = 0)
        {
            ViewBag.FocusId = focusId;
            return View();
        }
        /// <summary>
        /// Load đề nghị vật tư
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public ActionResult LoadRequirement(StoreRequestParameters parameters, int focusId=0)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var Task = SuppOrderSV.GetOrderRequirement(filter, focusId);
            return this.Store(Task, filter.PageTotal);
        }

        public ActionResult GetSuppliersList(StoreRequestParameters par, int id)
        {
            var data = new SuppliersBidOrderSV().GetAllbySuppliersOrderID(id);
            return this.Store(data);
        }
        public ActionResult GetData(StoreRequestParameters parameters)
        {
            return this.Store(lstOrderDetail);
        }
        [BaseSystem(Name = "Xem chi tiết", IsActive = true, IsShow = true)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowDetail(int id)
        {
            var obj = SuppOrderSV.GetById(id);
            lstOrderDetail = obj.SuppliersOrderRequirementDetails.ToList();
            return new Ext.Net.MVC.PartialViewResult() { ViewName = "DetailRequirement", Model = obj };
        }
        [BaseSystem(Name = "Thêm", IsActive = true, IsShow = true)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmAdd()
        {
            var data = new SuppliersOrderRequirementItem();
            string CodeAuto = Common.NextID(SuppOrderSV.GetMaxCode(), "YC");
            data.CODE = CodeAuto;
            lstOrderDetail = new List<SuppliersOrderRequirementDetailItem>();
            return new Ext.Net.MVC.PartialViewResult { ViewName = "UpdateRequirement", Model = data };
        }
        [BaseSystem(Name = "Gửi Y/C báo giá", IsActive = true, IsShow = true)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult SuppliersList(int id)
        {
            var data = new SuppliersOrderSV().GetById(id);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "SuppliersList", Model = data };
        }
        [BaseSystem(Name = "Sửa", IsActive = true, IsShow = true)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult UpdateForm(int id = 0)
        {
            var data = SuppOrderSV.GetById(id);
            lstOrderDetail = data.SuppliersOrderRequirementDetails.ToList();
            return new Ext.Net.MVC.PartialViewResult { ViewName = "UpdateRequirement", Model = data };
        }
        
        public ActionResult Update(SuppliersOrderRequirementItem data)
        {
            bool success = false;
            try
            {
                if (data.Approval.ID == 0)
                {
                    X.Msg.Show(new MessageBoxConfig
                    {
                        Buttons = MessageBox.Button.OK,
                        Icon = MessageBox.Icon.WARNING,
                        Title = "Thông báo",
                        Message = "Bạn phải chọn người phê duyệt đề nghị!"
                    });
                    return this.Direct(false);
                }
                else if (lstOrderDetail.Count < 1)
                {                    
                    Ultility.CreateMessageBox(title: "Thông báo", message: "Bạn phải chọn sản phẩm cho đơn hàng!", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                    return this.Direct(success);
                }else if(!checkQuantity())
                {
                    Ultility.CreateMessageBox(title: "Thông báo", message: "Bạn phải nhập số lượng sản phẩm cho đơn hàng!", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                    return this.Direct(success);
                }
                else
                {
                    if (data.IsEdit == false)
                    {
                        data.Status = 3;//Chờ duyệt
                    }
                    else if (data.Status != 2)
                        data.Status = 1;//Mới



                    if (data.ID == 0)
                    {
                        data.IsAccept = false;
                        data.CreateAt = DateTime.Now;
                        data.CreateBy = User.ID;
                        int id = SuppOrderSV.Insert(data);
                        if (id > 0)
                        {
                            foreach (SuppliersOrderRequirementDetailItem odt in lstOrderDetail)
                            {
                                SuppOrderDetailSV.Insert(odt, id);
                            }
                        }
                        if (data.IsApproval==false && data.IsEdit==false)
                        {
                            NotifyController notify = new NotifyController();
                            notify.Notify(this.ModuleCode, "Có một đề nghị mua hàng cần phê duyệt", data.Name, data.Approval.ID, User, Common.SuppliersOrders, "SuppliersOrderID:" + id.ToString());
                        }
                        Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                        lstOrderDetail = new List<SuppliersOrderRequirementDetailItem>();
                        success = true;
                    }
                    else
                    {

                        if (data.IsSendApproval && (data.Approval.ID == 0 || data.Approval == null))
                        {
                            Ultility.CreateMessageBox(title: "Thông báo", message: "Bạn phải chọn người phê duyệt đề nghị!", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                        }
                        else
                        {
                            data.UpdateBy = User.ID; data.UpdateAt = DateTime.Now;
                            SuppOrderSV.Update(data);
                            //Xóa và add lại orderdetail
                            SuppOrderDetailSV.DeleteByIdSuppOrderID(data.ID);
                            foreach (SuppliersOrderRequirementDetailItem odt in lstOrderDetail)
                            {
                                SuppOrderDetailSV.Insert(odt, data.ID);
                            }
                            Ultility.CreateNotification(message: RequestMessage.UpdateSuccess);
                            success = true;
                        }
                    }

                }
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreRequirement").Reload();                
            }
            return this.Direct(success);
        }
        
        public DirectResult GetFile(StoreRequestParameters parameters)
        {
            int totalCount;
            List<SuppliersOrderRequirementItem> lst = SuppOrderSV.GetOrderRequirement(1, 20, out totalCount);
            //ExportToExcel.CreateExcelDocument<SuppliersOrderRequirementItem>(lst, "Temp.xlsx",this.Response);
            return this.Direct();
            
        }
        [ValidateInput(false)]
        public DirectResult ExportExcel(string arrObject)
        {
            List<SuppliersOrderRequirementItem> lst = Ext.Net.JSON.Deserialize<SuppliersOrderRequirementItem[]>(arrObject).ToList();
            Dictionary<string, string> dictNameValue = new Dictionary<string, string>() { { "ID", "ID" }, { "CreateUserName", "Người Tạo" }, { "Name", "Tiêu Đề" }, { "StatusDispExcel", "Trạng Thái" } };
            
            //ExportToExcel.CreateExcelDocument<SuppliersOrderRequirementItem>(lst, "Temp.xlsx", this.Response,dictNameValue,_title);
            //ExportToExcel.CreateExcelDocument<SuppliersOrderRequirementItem>(lst, "D:\\Temp\\temp.xlsx", dictNameValue);
            return this.Direct();
            
        }
        
        private bool checkQuantity()
        {
            foreach (SuppliersOrderRequirementDetailItem obj in lstOrderDetail)
            {
                if (obj.Quantity.Value < 1)
                    return false;
            }
            return true;
        }
        [BaseSystem(Name = "Xóa ", IsActive = true, IsShow = true)]
        [SystemAuthorize(AllowAnonymous = false, CheckAuthorize = true)]
        public ActionResult Delete(string stringId)
        {
            try
            {
                int ids = int.Parse(stringId);
                SuppOrderSV.Delete(ids);
                X.Msg.Notify("Thông báo", "Bạn đã xóa bản ghi thành công!").Show();
                X.GetCmp<Store>("StoreRequirement").Reload();
                return this.Direct();
            }
            catch (Exception ex)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.ERROR,
                    Title = "Thông báo",
                    Message = "Xóa bản ghi không thành công!"
                });
                return this.Direct("Error");
            }
        }
        public ActionResult DeleteProductInStorage(string stringId = "0")
        {
            try
            {
                int id = int.Parse(stringId);
                lstOrderDetail.Remove(lstOrderDetail.FirstOrDefault(i => i.ID == id));
                X.GetCmp<Store>("stProductOrder").Reload();
                return this.Direct();
            }
            catch
            {
                return this.Direct("Error");
            }

        }
        public ActionResult ShowFrmFindProduct()
        {
            var data = product_GroupService.GetListAll();
            return new Ext.Net.MVC.PartialViewResult { ViewName = "FrmFindProducts", Model = data };
        }
        public ActionResult GetDataProductGroup()
        {
            List<StockProductGroupItem> lst;
            lst = product_GroupService.GetListAll();
            return this.Store(lst);

        }
        public ActionResult GetDataOfProducts(string stringId = "0")
        {
            string[] arrayId = stringId.Split(',');
            var data2 = productService.GetProductsByGroup_Id(arrayId);
            X.GetCmp<Store>("stProducts").LoadData(data2);
            X.GetCmp<GridPanel>("gpProducts").Refresh();
            return this.Direct();
        }
        public ActionResult GetProducts(string stringId = "0")
        {
            try
            {
                string[] arrayId = stringId.Split(',');
                int rs = lstOrderDetail.Count > 0 ? lstOrderDetail.LastOrDefault().ID : 0;
                var data = SuppOrderDetailSV.GetListProductByStringId(arrayId, rs);
                lstOrderDetail.AddRange(data);
                X.GetCmp<Store>("stProductOrder").LoadData(lstOrderDetail);
                X.GetCmp<Store>("stProductOrder").Reload();
                X.GetCmp<Window>("frmProducts").Close();
                X.GetCmp<GridPanel>("gpProductOder").Refresh();
                return this.Direct();
                
            }
            catch (Exception ex)
            {
                return this.Direct("Error");
            }

        }
        public DirectResult HandleChanges(int id, string field, string oldValue, string newValue, Nullable<int> quantity, Nullable<decimal> unitPrice, string note, object product)
        {
            SuppliersOrderRequirementDetailItem obj = lstOrderDetail.FirstOrDefault(i => i.ID == id);

            obj.Quantity = quantity.HasValue ? quantity.Value : 0;
            obj.Note = note;
            //X.GetCmp<Store>("stProductOrder").GetById(id).Commit();
            X.GetCmp<Store>("stProductOrder").Reload();
            return this.Direct();
        }
        public ActionResult ApproveForm(int id = 0)
        {

            var data = new SuppliersOrderRequirementItem();

            if (id != 0)
            {
                data = SuppOrderSV.GetById(id);
                lstOrderDetail = data.SuppliersOrderRequirementDetails.ToList();
            }
            if (data.ApprovalBy != User.EmployeeID)
            {
                X.Msg.Show(new MessageBoxConfig
                {
                    Buttons = MessageBox.Button.OK,
                    Icon = MessageBox.Icon.WARNING,
                    Title = "Thông báo",
                    Message = "Bạn không có quyền phê duyệt đề nghị này!"
                });
                return this.Direct();
            }
            else
            {
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Approve", Model = data };
            }
        }
        public ActionResult Appproved(SuppliersOrderRequirementItem updateData)
        {
            bool success = false;
            try
            {
                if (updateData.ID != 0)
                {
                    SuppOrderSV.Approved(updateData);                    
                    NotifyController notify = new NotifyController();
                    notify.Notify(this.ModuleCode, "Có một đề nghị mua hàng đã được phê duyệt", updateData.CreateUserName, updateData.CreateUserID, User, Common.SuppliersOrders, "SuppliersOrderID:" + updateData.ID.ToString());
                    Ultility.CreateNotification(message: RequestMessage.ApproveSuccess);
                    success = true;
                }
            }
            catch(Exception ex)
            {
                Ultility.CreateNotification(message: RequestMessage.ApproveError + "--" + ex.Message , error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreRequirement").Reload();
            }
            return this.FormPanel(success);
        }

    }
}
