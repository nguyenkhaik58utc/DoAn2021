using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Services;
using iDAS.Web;
using iDAS.Utilities;

namespace iDAS.Web.Controllers
{
    public class ChatController : BaseController
    {
        private ServerHub serverHub = new ServerHub();
        private BusinessChatSV chatSV = new BusinessChatSV();
        public ActionResult Index()
        {
            return PartialView();
        }
       
        public ActionResult GetEmployees(StoreRequestParameters parameters, string status)
        {
            List<int> keys = serverHub.GetEmployeeIDs(User.Code);
            object data;
            
            if (status == "offline")
            {
                data = chatSV.GetEmployeeOffline(keys, User.EmployeeID, status);
            }
            else 
            {
                data = chatSV.GetEmployeeOnline(keys, User.EmployeeID, status);
            }
            
            return this.Store(data,keys.Count-1);
        }

        public ActionResult Update(int employeeID, string status)
        {
            try
            {
                if (employeeID != 0)
                {
                    chatSV.Update(User.EmployeeID, employeeID);
                    var employee = chatSV.GetEmployee(User.EmployeeID, employeeID, status);
                    X.GetCmp<Store>("ChatEmployeeStores").GetById(employeeID).Set(employee);
                    X.GetCmp<DataView>("ChatEmployeeView").Refresh();
                }
            }
            catch
            {

            }
            return this.Direct();
        }

        public ActionResult LoadData(int employeeID)
        {
            var data = chatSV.GetChats(User.EmployeeID, employeeID);
            return this.Store(data);
        }

        public ActionResult SetTotalOnline()
        {
            var total = serverHub.GetKeys(User.Code).Count() - 1;
            X.GetCmp<DisplayField>("ChatOnlineTotal").SetValue(total);
            return this.Direct();
        }

        [HttpPost]
        public ActionResult Chat(string content, string employeeID) 
        {
            try
            {
                var employeeIds = iDAS.Utilities.Convert.ToListInt(employeeID);
                chatSV.Insert(content, employeeIds, User.ID);
                iDAS.Web.ClientHub.Chat(User.Code, employeeIds, User.EmployeeID);
                X.GetCmp<Store>("ChatStores").Reload();
            }
            catch {
                
                Ultility.ShowMessageBox(message: "Bạn chưa chọn nhân sự nào!", icon: Ext.Net.MessageBox.Icon.WARNING);
            }
            X.GetCmp<TextField>("ChatContent").SetValue(string.Empty);

            return this.FormPanel(true);
        }
    }
}
