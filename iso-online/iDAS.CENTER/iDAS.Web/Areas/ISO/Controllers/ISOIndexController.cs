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
    [BaseSystem(Name = "Quản lý điều khoản ISO", IsActive = true, IsShow = true, Position = 3, Icon = "FolderTable")]
    [SystemAuthorize(CheckAuthorize = false)]
    public class ISOIndexController : BaseController
    {
        ISOIndexSV isoIndexSV = new ISOIndexSV();
        ISOStandardSV isoSV = new ISOStandardSV();
        ISOIndexModuleSV ISOIndexModuleSV = new ISOIndexModuleSV();
        ISOIndexFunctionSV ISOIndexFunctionSV = new ISOIndexFunctionSV();
        [BaseSystem(Name = "Xem danh sách", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadCombo(StoreRequestParameters parameters)
        {
            int totalCount;
            var modules = isoIndexSV.GetCombo(parameters.Page, parameters.Limit, out totalCount);
            return this.Store(modules, totalCount);
        }
        public ActionResult LoadData(StoreRequestParameters parameters, int isoStandardId)
        {
            int totalCount;
            var modules = isoIndexSV.GetByIso(parameters.Page, parameters.Limit, out totalCount, isoStandardId);
            return this.Store(modules, totalCount);
        }
        [BaseSystem(Name = "Thêm, sửa, xem chi tiết", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult ShowFrmEdit(int id = 0, int type = 0)
        {
            try
            {
                if (id > 0)
                {
                    var obj = isoIndexSV.GetByID(id);
                    if (type == (int)iDAS.Utilities.Common.FormName.Edit)
                        return new Ext.Net.MVC.PartialViewResult { ViewName = "Update", Model = obj };
                    else if (type == (int)iDAS.Utilities.Common.FormName.Detail)
                        return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
                }
                return new Ext.Net.MVC.PartialViewResult { ViewName = "Insert" };
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult LoadIso()
        {
            var modules = isoSV.GetISOActived();
            return this.Store(modules);
        }
        [ValidateInput(false)]
        public ActionResult Insert(ISOClauseItem obj)
        {
            obj.CreateBy = User.ID;
            isoIndexSV.Insert(obj);

            Ultility.CreateNotification(message: Message.InsertSuccess);
            X.GetCmp<Store>("StoreISOIndex").Reload();

            return this.Direct();
        }
        [ValidateInput(false)]
        public ActionResult Update(ISOClauseItem obj)
        {
            obj.UpdateBy = User.ID;
            isoIndexSV.Update(obj);
            Ultility.CreateNotification(message: Message.UpdateSuccess);
            X.GetCmp<Store>("StoreISOIndex").Reload();

            return this.Direct();
        }
        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int id)
        {
            if (id > 0)
            {
                try
                {
                    isoIndexSV.Delete(id);
                    Ultility.CreateNotification(message: Message.DeleteSuccess);
                    X.GetCmp<Store>("StoreISOIndex").Reload();
                }
                catch (Exception ex)
                {
                    Ultility.CreateNotification(message: Message.DeleteDatabaseError);
                }
            }
            return this.Direct();
        }
        #region Chọn module
        public ActionResult ChooseModuleForm(int id = 0)
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "ChooseModule", Model = new ISOIndexModuleItem { ISOIndexID = id } };
        }
        public ActionResult GetDataSystem()
        {
            SystemSV systemSV = new SystemSV();
            var modules = systemSV.GetAll();
            return this.Store(modules);
        }
        public ActionResult LoadModulesClause(StoreRequestParameters parameters, int IsoIndexId = 0, string systemCode = "")
        {
            int totalCount;
            var modules = ISOIndexModuleSV.GetByClause(parameters.Page, parameters.Limit, out totalCount, IsoIndexId, systemCode);
            return this.Store(modules, totalCount);
        }
        public ActionResult EditModule(string data, int IsoIndexId = 0)
        {
            var module = Ext.Net.JSON.Deserialize<ISOIndexModuleItem>(data);
            try
            {
                if (IsoIndexId != 0)
                {
                    if (module != null && module.IsSelect)
                    {
                        module.ISOIndexID = IsoIndexId;
                        ISOIndexModuleSV.Insert(module);
                    }
                    if (module != null && !module.IsSelect)
                    {
                        ISOIndexModuleSV.Delete(module.CenterModuleID, IsoIndexId);
                    }
                    Ultility.CreateNotification(message: Message.UpdateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreChooseModules").Reload();
            }
            return this.Direct();
        }
        #endregion

        #region Chọn Function
        public ActionResult ChooseFunctionForm(int id = 0)
        {
            return new Ext.Net.MVC.PartialViewResult { ViewName = "ChooseFunction", Model = new ISOIndexFunctionItem { ISOIndexID = id } };
        }
        public ActionResult LoadFunctionClause(StoreRequestParameters parameters, int IsoIndexId = 0, string systemCode = "")
        {
            int totalCount;
            var functions = ISOIndexFunctionSV.GetByClause(parameters.Page, parameters.Limit, out totalCount, IsoIndexId, systemCode);
            return this.Store(functions, totalCount);
        }
        public ActionResult EditFunction(string data, int IsoIndexId = 0)
        {
            var module = Ext.Net.JSON.Deserialize<ISOIndexFunctionItem>(data);
            try
            {
                if (IsoIndexId != 0)
                {
                    if (module != null && module.IsSelect)
                    {
                        module.ISOIndexID = IsoIndexId;
                        ISOIndexFunctionSV.Insert(module);
                    }
                    if (module != null && !module.IsSelect)
                    {
                        ISOIndexFunctionSV.Delete(module.CenterFunctionID, IsoIndexId);
                    }
                    Ultility.CreateNotification(message: Message.UpdateSuccess);
                }
            }
            catch
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>("StoreChooseFunction").Reload();
            }
            return this.Direct();
        }
        #endregion

        public ActionResult UpdateSystemIso()
        {
            var result = isoIndexSV.UpdateSystemIsoIndex();
            return this.Direct(result);
        }
    }
}
