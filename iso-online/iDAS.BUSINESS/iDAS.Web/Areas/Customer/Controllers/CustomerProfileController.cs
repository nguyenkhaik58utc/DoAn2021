using Ext.Net;
using iDAS.Core;
using iDAS.Items;
using iDAS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iDAS.Web.Areas.Customer.Controllers
{
    [SystemAuthorize(CheckAuthorize = false)]
    public class CustomerProfileController : BaseController
    {
        private CustomerSV customerSV = new CustomerSV();
        public ActionResult ProfileCustomerForm(int id = 0)
        {
            var data = new CustomerProfileCustomerItem();
            if (id != 0)
            {
                data = customerSV.GetCustomerProfile(id);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "ProfileCustomer", Model = data };
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Contact(int customerID)
        {
            var data = new CustomerContactItem() { ID = customerID };
            return PartialView(data);
        }
        public ActionResult Info(int customerID)
        {
            CustomerSV NormalCustomerService = new CustomerSV();
            var data = NormalCustomerService.GetById(customerID); ;
            return PartialView(data);
        }
        public ActionResult ContactHistory(int customerID)
        {
            CustomerSV NormalCustomerService = new CustomerSV();
            var data = NormalCustomerService.GetById(customerID); ;
            return PartialView(data);
        }
        public ActionResult Order(int customerID)
        {
            CustomerSV NormalCustomerService = new CustomerSV();
            var data = NormalCustomerService.GetById(customerID); ;
            return PartialView(data);
        }
        public ActionResult Contract(int customerID)
        {
            CustomerSV NormalCustomerService = new CustomerSV();
            var data = NormalCustomerService.GetById(customerID); ;
            return PartialView(data);
        }
        public ActionResult Audit(int customerID)
        {
            CustomerSV NormalCustomerService = new CustomerSV();
            var data = NormalCustomerService.GetById(customerID); ;
            return PartialView(data);
        }
    }
}
