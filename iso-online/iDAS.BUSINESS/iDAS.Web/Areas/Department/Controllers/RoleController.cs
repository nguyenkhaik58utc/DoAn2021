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

namespace iDAS.Web.Areas.Department.Controllers
{
    [BaseSystem(Name = "Danh sách chức danh", IsActive = true, IsShow = false)]
    [SystemAuthorize(CheckAuthorize = false)]
    public partial class RoleController : BaseController
    {
        private RoleSV roleSV = new RoleSV();

        #region View
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index(int departmentId = 0)
        {
            try
            {
                ViewData["DepartmentID"] = departmentId;
                return new Ext.Net.MVC.PartialViewResult { ViewData = ViewData };
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult LoadData(StoreRequestParameters parameters, int departmentId = 0, bool viewDelete = false)
        {
            try
            {
                var roles = roleSV.GetRoles(departmentId, viewDelete);
                return this.Store(roles);
            }
            catch {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        #endregion
        #region Create
        [BaseSystem(Name = "Thêm")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Create(int departmentId = 0)
        {
            try
            {
                ViewData["DepartmentID"] = departmentId;
                return new Ext.Net.MVC.PartialViewResult { ViewData = ViewData };
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        [HttpPost]
        public ActionResult Create(HumanRoleItem role)
        {
            var success = false;
            try
            {
                if (ModelState.IsValid)
                {
                    if (!roleSV.CheckNameExist(role.Name, role.DepartmentID))
                    {
                        roleSV.Insert(role);
                        Ultility.ShowMessageRequest(RequestStatus.CreateSuccess);
                        success = true;
                    }
                    else
                    {
                        Ultility.ShowMessageRequest(RequestStatus.ExistError);
                        success = false;
                    }
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
        #region Update
        [BaseSystem(Name = "Sửa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Update(int roleId)
        {
            try
            {
                var role = roleSV.GetRole(roleId);
                return new Ext.Net.MVC.PartialViewResult { Model = role };
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        [HttpPost]
        public ActionResult Update(HumanRoleItem role)
        {
            var success = false;
            try
            {
                if (!roleSV.CheckNameExist(role.Name, role.DepartmentID, role.ID))
                {
                    roleSV.Update(role);
                    Ultility.ShowMessageRequest(RequestStatus.UpdateSuccess);
                    success = true;
                }
                else
                {
                    Ultility.ShowMessageRequest(RequestStatus.ExistError);
                    success = false;
                }
            }
            catch(Exception ex)
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
                success = false;
            }
            return this.FormPanel(success);
        }
        #endregion
        #region Delete
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete()
        {
            return this.Direct(true);
        }
        [HttpPost]
        public ActionResult Delete(int roleId = 0)
        {
            try
            {
                roleSV.Delete(roleId);
                Ultility.ShowMessageRequest(RequestStatus.DeleteSuccess);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.DeleteError);
            }
            return this.Direct();
        }
        #endregion
        #region Detail
        [BaseSystem(Name = "Xem chi tiết")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Detail(int roleId)
        {
            try
            {
                var role = roleSV.GetRole(roleId);
                return new Ext.Net.MVC.PartialViewResult { Model = role };
            }
            catch 
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        #endregion
        #region Move
        [BaseSystem(Name = "Chuyển tổ chức")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Move()
        {
            return this.Direct(true);
        }
        [HttpPost]
        public ActionResult Move(int roleId, int departmentId)
        {
            try
            {
                roleSV.Move(roleId, departmentId);
                Ultility.ShowMessageRequest(RequestStatus.MoveSuccess);
            }
            catch 
            {
                Ultility.ShowMessageRequest(RequestStatus.MoveError);
            }
            return this.Direct();
        }
        #endregion
        #region Restore
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Phục hồi")]
        public ActionResult Restore()
        {
            return this.Direct(true);
        }
        [HttpPost]
        public ActionResult Restore(int roleId = 0)
        {
            try
            {
                roleSV.Restore(roleId);
                Ultility.ShowMessageRequest(RequestStatus.RestoreSuccess);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.RestoreError);
            }
            return this.Direct();
        }
        #endregion
        #region Destroy
        [BaseSystem(Name = "Hủy")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Destroy()
        {
            return this.Direct(true);
        }
        [HttpPost]
        public ActionResult Destroy(int roleId = 0)
        {
            try
            {
                roleSV.Destroy(roleId);
                Ultility.ShowMessageRequest(RequestStatus.DestroySuccess);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.DestroyError);
            }
            return this.Direct();
        }
        #endregion
        #region Sort
        [SystemAuthorize(CheckAuthorize = true)]
        [BaseSystem(Name = "Sắp xếp")]
        public ActionResult Sort()
        {
            return this.Direct(true);
        }
        [HttpPost]
        public ActionResult Sort(int roleId, int departmentId, bool sort)
        {
            try
            {
                var change = roleSV.Sort(roleId, departmentId, sort);
                X.GetCmp<Hidden>("SortRole").SetValue(change);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        #endregion
        public ActionResult LoadRole(int? id = null)
        {
            if (id == null && id != 0) return null;
            var role = roleSV.GetByID(id.Value);
            return this.Store(role);
        }
        
        
        
        /// <summary>
        /// Lấy danh sách chức danh
        /// </summary>
        /// <param name="containerId"></param>
        /// <returns></returns>
        public ActionResult RoleViewAjax(string containerId, string gridRoleId)
        {
            ViewData["GridRoleID"] = gridRoleId;
            var newPartial = new Ext.Net.MVC.PartialViewResult
            {
                ViewName = "RoleView",
                ContainerId = containerId,
                WrapByScriptTag = false,
                RenderMode = RenderMode.AddTo,
                ViewData = ViewData
            };
            return newPartial;
        }
        public ActionResult RoleView()
        {
            return PartialView();
        }
        public ActionResult RoleWindow(string handleClose = "")
        {
            ViewData["HandleClose"] = handleClose;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewData = ViewData,
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public ActionResult LoadOrganizations(int departmentID = 0)
        {
            var roles = roleSV.GetRoles(departmentID);
            return this.Store(roles);
        }
    }
}
