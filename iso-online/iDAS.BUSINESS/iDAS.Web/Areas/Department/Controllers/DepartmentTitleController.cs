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
using iDAS.Web.ExtExtend;
using iDAS.Utilities;
using iDAS.Web.API;
using iDAS.Web.Areas.Department.Models;
using System.Threading.Tasks;

namespace iDAS.Web.Areas.Department.Controllers
{
    [BaseSystem(Name = "Danh sách chức danh", IsActive = true, IsShow = false)]
    [SystemAuthorize(CheckAuthorize = false)]
    public partial class DepartmentTitleController : BaseController
    {
        private DepartmentTitleAPI departmentTitleAPI = new DepartmentTitleAPI();

        #region View
        //[BaseSystem(Name = "Xem danh sách")]
        //[SystemAuthorize(CheckAuthorize = true)]
        //public ActionResult Index(int departmentID = 0)
        //{
        //    try
        //    {
        //        ViewData["DepartmentID"] = departmentID;
        //        return new Ext.Net.MVC.PartialViewResult { ViewData = ViewData };
        //    }
        //    catch
        //    {
        //        Ultility.ShowMessageRequest(RequestStatus.Error);
        //    }
        //    return this.Direct();
        //}
        public ActionResult LoadData(StoreRequestParameters parameters, int departmentID = 0, bool viewDelete = false)
        {
            try
            {
                var titles = departmentTitleAPI.GetByDepartment(departmentID);
                return this.Store(titles);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadData1(StoreRequestParameters parameters, int titleID = 0, bool viewDelete = false)
        {
            try
            {
                var titles = departmentTitleAPI.GetByTitle(titleID).Result;
                return this.Store(titles);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        #endregion

        #region Create
        // combobox title
        public  ActionResult GetTitleForCreate(int departmentID)
        {
            var result = departmentTitleAPI.GetTitleForCreate(departmentID).Result;
            return this.Store(result);
        }

        [BaseSystem(Name = "Thêm")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Create(int departmentID = 0)
        {
            try
            {
                ViewData["DepartmentID"] = departmentID;
                return new Ext.Net.MVC.PartialViewResult { ViewData = ViewData };
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }

        [BaseSystem(Name = "Gán")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Create1(int titleID = 0)
        {
            try
            {
                ViewData["TitleID"] = titleID;
                return new Ext.Net.MVC.PartialViewResult { ViewData = ViewData };
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }

        [HttpPost]
        public ActionResult Create(DepartmentTitle departmentTitle)
        {
            if (departmentTitle.DepartmentID == 0 && departmentTitle.Department.ID > 0)
                departmentTitle.DepartmentID = departmentTitle.Department.ID;

            var success = false;
            try
            {
                //if (ModelState.IsValid)
                {
                    departmentTitleAPI.Insert(departmentTitle, User.ID);
                    Ultility.ShowMessageRequest(RequestStatus.CreateSuccess);
                    success = true;
                }
                //else
                //{
                //    Ultility.ShowMessageRequest(RequestStatus.ValidError);
                //    success = false;
                //}
            }
            catch(Exception ex)
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
                success = false;
            }
            return this.FormPanel(success);
        }

        [BaseSystem(Name = "Xóa", IsActive = true, IsShow = false)]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete(int? id)
        {
            var success = false;
            try
            {
                if (ModelState.IsValid)
                {
                    departmentTitleAPI.Delete(id.Value);
                    Ultility.ShowMessageRequest(RequestStatus.DeleteSuccess);
                    success = true;
                }
                else
                {
                    Ultility.ShowMessageRequest(RequestStatus.ValidError);
                    success = false;
                }
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
                success = false;
            }
            return this.FormPanel(success);
        }
        #endregion
    }
}
