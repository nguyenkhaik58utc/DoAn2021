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
        private SystemFunctionService functionService = new SystemFunctionService();
        private readonly string storeFunctionsID = "StoreFunctions";

        public ActionResult LoadFunctions(StoreRequestParameters parameters, string moduleCode)
        {
            int totalCount;
            var functions = functionService.GetAll(parameters.Page, parameters.Limit, out totalCount, moduleCode);
            Ultility.SetIconUrl(functions);
            Ultility.SetStatus(functions);
            return this.Store(functions, totalCount);
        }
        [ValidateInput(false)]
        public ActionResult UpdateFunction(string data)
        {
            var function = Ext.Net.JSON.Deserialize<SystemFunctionItem>(data);
            try
            {
                Ultility.SetIconName(function);
                functionService.Update(function, User.ID);
                Ultility.CreateNotification(message: Message.UpdateSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>(storeFunctionsID).Reload();
            }
            return this.Direct();
        }
        public ActionResult DeleteFunction(int id)
        {
            try
            {
                functionService.Delete(id);
                Ultility.CreateNotification(message: Message.DeleteSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>(storeFunctionsID).Reload();
            }
            return this.Direct();
        }
    }
}
