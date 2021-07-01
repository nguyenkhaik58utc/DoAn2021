using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;

namespace iDAS.Web.Controllers
{
    public class NotifyController : BaseController
    {
        private BusinessNotifySV notifySV = new BusinessNotifySV();
        private HumanUserSV humanUserSV = new HumanUserSV();
        public ActionResult Index()
        {
            if (User.ID > 0)
            {
                try
                {
                    var notify = notifySV.getNotifyUnRead(User.EmployeeID);
                    return View("Index", notify);
                }
                catch (Exception ex)
                {
                    return this.Direct("false");
                }
            }
            else
            {
                return this.Direct("false");
            }
        }
        public ActionResult GetNumberNotify()
        {
            string notifyNewTotal = notifySV.GetNotifyNewTotal(User.EmployeeID);
            return this.Direct(notifyNewTotal);
        }

        public ActionResult GetData(StoreRequestParameters parameters, bool isAll = false)
        {
            ModelFilter filter = ExtExtend.ExtExtendFilter.ConvertToFilter(parameters);
            var notify = notifySV.GetNotify(filter, User.EmployeeID, isAll);
            return this.Store(notify);
        }
        public ActionResult UpdateRead(int id)
        {
            notifySV.UpdateRead(id, User.ID);
            string notifyNewTotal = notifySV.GetNotifyNewTotal(User.EmployeeID);
            return this.Direct(notifyNewTotal);
        }
        public void Notify(string moduleCode, string title, string content, int employeeID, UserPrincipal user, string function, string param)
        {
            try
            {
                var employeeIDs = new List<int>(){
                    employeeID,
                };
                Notify(moduleCode, title, content, employeeIDs, user, function, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void NotifyUser(string moduleCode, string title, string content, int userId, UserPrincipal user, string function, string param)
        {
            try
            {
                var userIDs = new List<int>(){
                    userId,
                };
                var employeeIDs = humanUserSV.GetEmployeeIDByListUser(userIDs);
                Notify(moduleCode, title, content, employeeIDs, user, function, param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Notify(string moduleCode, string title, string content, List<int> employeeIDs, UserPrincipal user, string function, string param)
        {
            try
            {
                BusinessNotifyItem notify = new BusinessNotifyItem()
                {
                    ModuleCode = moduleCode,
                    EmployeeIDs = employeeIDs,
                    Title = title,
                    Content = content,
                    IsRead = false,
                    FunctionName = function,
                    Param = param,
                    CreateAt = DateTime.Now,
                    CreateBy = user.ID,
                };
                notifySV.Insert(notify);
                iDAS.Web.ClientHub.Notify(user.Code, notify.EmployeeIDs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void NotifyUser(string moduleCode, string title, string content, List<int> userIds, UserPrincipal user, string function, string param)
        {
            try
            {
                var employeeIDs = humanUserSV.GetEmployeeIDByListUser(userIds);
                BusinessNotifyItem notify = new BusinessNotifyItem()
                {
                    ModuleCode = moduleCode,
                    EmployeeIDs = employeeIDs,
                    Title = title,
                    Content = content,
                    IsRead = false,
                    FunctionName = function,
                    Param = param,
                    CreateAt = DateTime.Now,
                    CreateBy = user.ID,
                };
                notifySV.Insert(notify);
                iDAS.Web.ClientHub.Notify(user.Code, notify.EmployeeIDs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
