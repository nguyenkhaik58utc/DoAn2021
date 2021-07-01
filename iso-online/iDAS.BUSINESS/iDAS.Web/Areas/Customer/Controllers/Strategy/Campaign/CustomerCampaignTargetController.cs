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
namespace iDAS.Web.Areas.Customer.Controllers
{
    public class CustomerCampaignTargetController : BaseController
    {
        private CustomerCampaignTargetSV CustomerCampaignTargetSV = new CustomerCampaignTargetSV();
        public ActionResult LoadTargets(StoreRequestParameters parameters, int paramID = 0)
        {
            int totalCount;
            var Task = CustomerCampaignTargetSV.GetAll(parameters.Page, parameters.Limit, out totalCount, paramID);
            return this.Store(Task, totalCount);
        }
        public ActionResult Insert(QualityTargetItem target, int CampaignID)
        {
            try
            {
                if (target.Approval == null)
                {
                    Ultility.CreateMessageBox(title: "Thông báo", message: "Bạn phải chọn người phê duyệt mục tiêu!", icon: MessageBox.Icon.WARNING, button: MessageBox.Button.OK);
                }
                else
                {
                    target.CreateBy = User.ID;
                    CustomerCampaignTargetSV.Insert(target, CampaignID);
                    X.GetCmp<Window>("WindowTargetAddView").Close();
                    X.GetCmp<Store>("stTarget").Reload();
                    Ultility.CreateNotification(message: RequestMessage.CreateSuccess);
                }  
            }
            catch
            {
                Ultility.CreateNotification(message: RequestMessage.CreateError, error: true);
                throw;
            }
            return this.Direct();
        }    
    }
}
