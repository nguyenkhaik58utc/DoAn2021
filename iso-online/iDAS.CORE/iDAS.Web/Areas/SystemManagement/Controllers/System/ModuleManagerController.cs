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

namespace iDAS.Web.Areas.SystemManagement.Controllers
{
    public partial class SystemManagerController : BaseController
    {
        private SystemModuleService moduleService = new SystemModuleService();
        private readonly string storeModulesID = "StoreModules";

        [HttpPost]
        public ActionResult LoadModules()
        {
            var modules = moduleService.GetAll();
            return this.Store(modules);
        }
        public ActionResult LoadModules(StoreRequestParameters parameters)
        {
            int totalCount;
            var modules = moduleService.GetAll(parameters.Page, parameters.Limit, out totalCount);
            Ultility.SetIconUrl(modules);
            Ultility.SetStatus(modules);
            return this.Store(modules, totalCount);
        }
        public ActionResult DeleteModule(int id)
        {
            try
            {
                moduleService.Delete(id);
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
            var module = Ext.Net.JSON.Deserialize<SystemModuleItem>(data);
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
    }
}
