using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Services;
using iDAS.Items;
using iDAS.Core;
using iDAS.Utilities;
using System.Collections;
using iDAS.Web.Areas.Systems;


namespace iDAS.Web.Areas.ISO.Controllers
{
    [BaseSystem(Name = "Quản lý tiêu chuẩn ISO", IsActive = true, IsShow = true, Position = 2)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ISOStandardController : BaseController
    {
        ISOStandardSV isoSV = new ISOStandardSV();
        private readonly string storeMnISOModule = "stModuleIsoNace";
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }

        [BaseSystem(Name = "Thêm, sửa, xem chi tiết, thiết lập module, xem danh sách điều khoản, xem danh sách họp xem xét", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrm(int id = 0, int type = 0)
        {
            try
            {
                var obj = isoSV.GetByID(id);
                if (type == (int)Common.FormName.Edit)
                    return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = obj };
                else if (type == (int)Common.FormName.Detail)
                    return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
                else if (type == (int)Common.FormName.IsoStandard)
                    return new Ext.Net.MVC.PartialViewResult { ViewName = "ISOStandard", Model = obj };
                else if (type == (int)Common.FormName.IsoMeeting)
                    return new Ext.Net.MVC.PartialViewResult { ViewName = "ISOMeeting", Model = obj };
                else if (type == (int)Common.FormName.EditDetail)
                {
                    ISONaceCodeItem obj1 = new ISONaceCodeItem();
                    var data = isoSV.GetByID(id);
                    if (data != null)
                    {
                        obj1.ID = data.ID;
                        obj1.Name = data.Name;
                        obj1.Code = data.Code;
                    }
                    return new Ext.Net.MVC.PartialViewResult { ViewName = "ChooseModule", Model = obj1 };

                }
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Insert", Model = obj };
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult LoadData(StoreRequestParameters parameters)
        {
            int totalCount;
            var modules = isoSV.GetAll(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(modules, totalCount);
        }
        public ActionResult LoadISOHasAnnex(StoreRequestParameters parameters)
        {
            int totalCount;
            var modules = isoSV.GetISOHasAnnex(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(modules, totalCount);
        }

        #region Thêm
        public ActionResult ShowFrmInsert()
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Insert" };
        }
        [ValidateInput(false)]
        public ActionResult InsertIso(ISOStandardItem obj)
        {
            if (insert(obj))
            {
                Ultility.CreateNotification(message: Message.InsertSuccess);
                X.GetCmp<Store>("stIso").Reload();
            }
            else
            {
                Ultility.CreateNotification(message: Message.InsertError, error: true);
            }
            return this.Direct();
        }
        private bool insert(ISOStandardItem obj)
        {
            try
            {
                obj.CreateBy = User.ID;
                var id = isoSV.Insert(obj);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Sửa
        [ValidateInput(false)]
        public ActionResult UpdateIso(ISOStandardItem obj)
        {
            if (update(obj))
            {
                Ultility.CreateNotification(message: Message.UpdateSuccess);
                X.GetCmp<Store>("stIso").Reload();
            }
            else
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true);
            }
            return this.Direct();
        }
        private bool update(ISOStandardItem obj)
        {
            try
            {
                obj.UpdateBy = User.ID;
                isoSV.Update(obj);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Xóa
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id)
        {
            if (id > 0)
            {
                try
                {
                    isoSV.Delete(id);
                    Ultility.CreateNotification(message: Message.DeleteSuccess);
                    X.GetCmp<Store>("stIso").Reload();
                }
                catch (Exception ex)
                {
                    Ultility.CreateNotification(message: Message.DeleteDatabaseError);
                }

            }
            return this.Direct();
        }
        #endregion

        #region Thiết lập moule
        public ActionResult LoadModules(StoreRequestParameters parameters, int iSONaneCodeId, string systemCode)
        {
            int totalCount;
            var modules = new ModuleSV().GetModuleSelected(iSONaneCodeId, systemCode, parameters.Page, parameters.Limit, out totalCount);
            Ultility.SetIconUrl(modules);
            Ultility.SetStatus(modules);
            return this.Store(modules, totalCount);
        }
        public ActionResult ShowFrmAddModule(string selection)
        {
            X.GetCmp<Window>("winAddModuleIso").Close();
            SelectedRowCollection src = JSON.Deserialize<SelectedRowCollection>(selection);
            int id = 0;
            if (src != null && src.Count() > 0)
            {
                id = System.Convert.ToInt32(src[0].RecordID);//IsoNaceCodeID  

            }
            var arID = isoSV.GetModuleByIsoNace(id);
            SelectedRowCollection selected = new SelectedRowCollection();
            if (arID != null && arID.Count() > 0)
            {
                foreach (var item in arID)
                {
                    selected.Add(new SelectedRow(item.ToString()));
                }
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "AddModule", Model = null, ViewData = { { "Selected", selected } } };
        }
        public ActionResult GetDataModuleByIsoNace(string IsoNaceID)
        {
            int i = 0;
            if (!IsoNaceID.Trim().Equals("")) i = System.Convert.ToInt32(IsoNaceID);
            var arID = isoSV.GetModuleIDByIso(i);
            List<ISOStandardModuleItem> lst = new List<ISOStandardModuleItem>();
            lst = isoSV.GetModuleAll();
            return this.Store(lst);
        }
        public ActionResult InsertIsoModule(ISONaceCodeItem obj, string selection)
        {
            //Lay Danh sach tieu chuan Iso
            selection = selection.Trim(',');
            //int naceIsoID = obj.IsoNaceID;
            if (!selection.Equals(""))
            {
                //Lay Danh sach cac gia tri cach nhau ;
                var arObj = ConvertData.SplitString(selection, ',');
                if (arObj.Count() > 0)
                {
                    foreach (var item in arObj)
                    {
                        //them moi hoac update
                        var objNace = ConvertData.SplitString(item, ';');
                        int moduleID = System.Convert.ToInt16(objNace[0]);
                        bool isActive = System.Convert.ToBoolean(objNace[1]);
                        bool isShow = System.Convert.ToBoolean(objNace[2]);
                        decimal price = System.Convert.ToDecimal(objNace[3]);
                    }
                }
            }
            return this.Direct();

        }
        public ActionResult UpdateModuleSelected(string data, int iSOId, string SystemCode)
        {
            var module = Ext.Net.JSON.Deserialize<ModuleItem>(data);
            try
            {
                var systemItem = new SystemSV().GetByCode(SystemCode);
                var systemId = systemItem.ID;
                var obj = isoSV.GetByiSOIdandmoduleId(iSOId, module.ID);
                if (obj == null && module.IsSelected)
                {
                    obj = new iDAS.Base.ISOStandardModule();
                    obj.ISOStandardID = iSOId;
                    obj.CenterModuleID = module.ID;
                    obj.IsShow = module.IsShow;
                    obj.IsActive = module.IsActive;
                    obj.IsRequired = module.IsRequired;
                    isoSV.InsertISOModule(obj);
                }
                if (obj != null && !module.IsSelected)
                {
                    isoSV.DeleteISOModule(obj.ID);
                }
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>(storeMnISOModule).Reload();
            }
            return this.Direct();
        }
        public ActionResult UpdateModuleSelecteds(string data, int iSOId, bool isSelected)
        {
            try
            {
                var ISOModule = Ext.Net.JSON.Deserialize<List<int>>(data);
                foreach (var isomodule in ISOModule)
                {
                    var obj = isoSV.GetByiSOIdandmoduleId(iSOId, isomodule);
                    if (obj == null && isSelected)
                    {
                        obj = new iDAS.Base.ISOStandardModule();
                        obj.ISOStandardID = iSOId;
                        obj.CenterModuleID = isomodule;
                        obj.IsShow = obj.IsShow;
                        obj.IsActive = obj.IsActive;
                        obj.IsRequired = obj.IsRequired;
                        isoSV.InsertISOModule(obj);
                    }
                    if (obj != null && !isSelected)
                    {
                        isoSV.DeleteISOModule(obj.ID);
                    }
                }
            }
            catch (Exception ex)
            {
                Ultility.ShowMessageBox(message: Message.UpdateError, icon: MessageBox.Icon.INFO);
            }
            finally
            {
                X.GetCmp<Store>(storeMnISOModule).Reload();
            }

            return this.Direct();
        }
        public ActionResult GetDataSystem()
        {
            SystemSV systemSV = new SystemSV();
            var modules = systemSV.GetAll();
            return this.Store(modules);
        }
        #endregion

        #region Danh sách điều khoản
        public ActionResult LoadIsoStandard(StoreRequestParameters parameters, int id)
        {
            ISOIndexSV isStandardSV = new ISOIndexSV();
            int totalCount = 0;
            var lst = isStandardSV.GetByIso(parameters.Page, parameters.Limit, out totalCount, id);

            return this.Store(lst, totalCount);
        }
        #endregion

        #region Danh sách họp xem xét
        public ActionResult LoadIsoMeeting(StoreRequestParameters parameters, int id)
        {
            IsoMeetingService isoMeetingSV = new IsoMeetingService();
            int totalCount = 0;
            var lst = isoMeetingSV.GetByISO(parameters.Page, parameters.Limit, out totalCount, id);

            return this.Store(lst, totalCount);
        }
        #endregion

        public ActionResult FillLstIso()
        {
            List<ISOStandardItem> lst = new List<ISOStandardItem>();
            ISOStandardSV isoSV = new ISOStandardSV();
            lst = isoSV.GetISOActived();
            return this.Store(lst);
        }
    }
}
