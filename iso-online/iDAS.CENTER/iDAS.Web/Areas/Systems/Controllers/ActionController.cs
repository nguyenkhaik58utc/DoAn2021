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
using iDAS.Utilities;

namespace iDAS.Web.Areas.Systems.Controllers
{
    public class ActionController : BaseController
    {
        private ActionSV actionService = new ActionSV();
        private readonly string storeActionsID = "StoreActions";

        public ActionResult Detail(int ID)
        {
            //var obj = actionService.GetByID(ID);

            //return new Ext.Net.MVC.PartialViewResult { ViewName = "Detail", Model = obj };
            return View();
        }
        public ActionResult LoadIcons(int start, int limit)
        {
            int total;
            return this.Store(Ultility.GetIcons(start, limit, out total), total);
        }
        public ActionResult LoadActions(StoreRequestParameters parameters, string functionCode, string moduleCode)
        {
            var actions = actionService.GetActions(functionCode, moduleCode);
            Ultility.SetIconUrl(actions);
            Ultility.SetStatus(actions);
            return this.Store(actions);
          //  return View();
        }

        [ValidateInput(false)]
        public ActionResult UpdateAction(string data)
        {
            var action = Ext.Net.JSON.Deserialize<iDAS.Items.ActionItem>(data);
            try
            {
                //Ultility.SetIconName(action);
                //actionService.Update(action, User.ID);              
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.UpdateError, error: true);
            }
            finally
            {
                X.GetCmp<Store>(storeActionsID).Reload();
            }
            return this.Direct();
        }


        public ActionResult DeleteAction(int id)
        {
            try
            {
                //actionService.Delete(id);
                Ultility.CreateNotification(message: Message.DeleteSuccess);
            }
            catch (Exception ex)
            {
                Ultility.CreateNotification(message: Message.DeleteError, error: true);
            }
            finally
            {
                X.GetCmp<Store>(storeActionsID).Reload();
            }
            return this.Direct();
        }
        //GetDataSystem
        //public ActionResult GetDataSystem()
        //{
        //    List<MnCategoriesItem> lst = CommonFunction.GetAllSystem();

        //    return this.Store(lst);
        //}
    
    }
}
