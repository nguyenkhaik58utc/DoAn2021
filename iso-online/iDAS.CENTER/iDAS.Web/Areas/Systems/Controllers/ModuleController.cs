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

namespace iDAS.Web.Areas.Systems.Controllers
{
    [BaseSystem(Name = "Quản lý module", IsActive = true, IsShow = true, Position = 2, Icon = "ApplicationOsxCascade")]
    public class ModuleController : BaseController
    {
        private ModuleSV moduleService = new ModuleSV();
        private readonly string storeModulesID = "StoreModules";

        [BaseSystem(Name = "Quản lý Module", IsActive = true, IsShow = true)]
        public ActionResult Index()
        {
            return View();
        }
         [BaseSystem(Name = "Chi tiết Module", IsActive = true, IsShow = true)]
        public ActionResult Detail(int ID)
        {
            var obj = moduleService.GetByID(ID);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
        }

         //UpdatePrice
        
        public ActionResult LoadIcons(int start, int limit)
        {
            int total;
            return this.Store(Ultility.GetIcons(start, limit, out total), total);
        }
        [HttpPost]
        public ActionResult LoadModules(string systemCode)
        {
            var modules = moduleService.GetAll(systemCode);
            Ultility.SetIconUrl(modules);
            return this.Store(modules);
        }
        public ActionResult LoadModules(StoreRequestParameters parameters, string systemCode="")
        {
            var modules = moduleService.GetAll(systemCode);
            Ultility.SetIconUrl(modules);
            //var lst = Systems.CommonFunction.GetDataFilter(parameters, modules);
            //return this.Store(modules, lst.Count());
            return View();
        }
        public ActionResult DeleteModule(int id)
        {
            try
            {
                //moduleService.Delete(id);
                Ultility.CreateNotification(message: Message.DeleteSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>(storeModulesID).Reload();
            }
            return this.Direct();
        }
        [ValidateInput(false)]
        public ActionResult UpdateModule(string data)
        {
            var module = Ext.Net.JSON.Deserialize<ModuleItem>(data);
            try
            {
                Ultility.SetIconName(module);
                moduleService.Update(module, User.ID);
                Ultility.CreateNotification(message: Message.UpdateSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>(storeModulesID).Reload();
            }
            return this.Direct();
        }
        public ActionResult UpdateSystem(string systemCode)
        {
            try
            {
                string url = "http://localhost:54400/SystemManagement/System/UpdateSystem?systemCode={0}&userID={1}";
                url = string.Format(url, systemCode, User.ID);

                var success =  WebClientHelper.Request(url);

                Ultility.CreateNotification(message: Message.DeleteSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.DeleteError, error: true);
            }
            return this.Direct();
        }

        //GetDataSystem
        public ActionResult GetDataSystem()
        {
            SystemSV service = new SystemSV();
            List<SystemItem> lst = service.GetAll();
            return this.Store(lst);
        }
    }
}
