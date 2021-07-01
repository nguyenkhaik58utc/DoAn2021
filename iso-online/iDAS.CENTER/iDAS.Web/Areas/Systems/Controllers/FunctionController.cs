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
    [BaseSystem(Name = "Quản lý chức năng", IsActive = true, IsShow = true, Position = 3, Icon = "Application")]
    public class FunctionController : BaseController
    {
        private FunctionSV functionService = new FunctionSV();
        private readonly string storeFunctionsID = "StoreFunctions";

        [BaseSystem(Name = "Quản lý chức năng", IsActive = true, IsShow = true)]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(int ID)
        {
            var obj = functionService.GetByID(ID);
            return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
        }

        public ActionResult LoadIcons(int start, int limit)
        {
            int total;
            return this.Store(Ultility.GetIcons(start, limit, out total), total);
        }
        public ActionResult LoadFunctions(StoreRequestParameters parameters, string moduleCode,string systemCode)
        {
            var functions = functionService.getFunctionItem(moduleCode, systemCode);
            Ultility.SetIconUrl(functions);
            Ultility.SetStatus(functions);
            return this.Store(functions);
        }
        [ValidateInput(false)]
        public ActionResult UpdateFunction(string data)
        {
            var function = Ext.Net.JSON.Deserialize<FunctionItem>(data);
            try
            {
                //Ultility.SetIconName(function);
                //functionService.Update(function, User.ID);                
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
                //functionService.Delete(id);
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


        //GetDataSystem
        public ActionResult GetDataSystem()
        {
            SystemSV service = new SystemSV();
            List<SystemItem> lst = service.GetAll();
            return this.Store(lst);
        }
    }
}
