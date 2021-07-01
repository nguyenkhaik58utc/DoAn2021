using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Web;
using iDAS.Core;
using iDAS.Services;
using iDAS.Items;
using iDAS.Utilities;

namespace iDAS.Web.Areas.Department.Controllers
{
    [BaseSystem(Name = "Cơ cấu tổ chức", IsActive = true, Icon = "House", IsShow = true, Position = 1)]
    [SystemAuthorize(CheckAuthorize = false)]
    public class DepartmentController : BaseController
    {
        private DepartmentSV departmentSV = new DepartmentSV();
        private Node createNodeDepartment(HumanDepartmentItem department)
        {
            Node node = new Node();
            var css = "DepartmentActive";
            if (department.IsActive == false)
            {
                css = "DepartmentDeactive";
            }
            if (department.IsMerge == true)
            {
                css = "DepartmentMerge";
            }
            if (department.IsCancel == true)
            {
                css = "DepartmentCancel";
            }
            if (department.IsDelete == true)
            {
                css = "DepartmentDelete";
            }
            //if (department.IsSelected)
            //    css = "ForEmployee";
            //else
            //    css = "NotAllow";
            node.NodeID = department.ID.ToString();
            node.Text = department.Name.ToUpper();
            node.Cls = css;
            node.Icon = department.ParentID == 0 ? Icon.House : Icon.Door;
            node.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = department.ParentID.ToString(), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "UpdateAt", Value = department.UpdateAtView, Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "EstablishedDate", Value = department.EstablishedDateView, Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsCancel", Value = JSON.Serialize(department.IsCancel), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsDelete", Value = JSON.Serialize(department.IsDelete), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsActive", Value = JSON.Serialize(department.IsActive), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsSelected", Value = JSON.Serialize(department.IsSelected), Mode = ParameterMode.Value });
            node.Leaf = !department.IsParent;
            return node;
        }
        private Node createNodeDepartment(HumanDepartmentItem department,List<int> lstdepartmentEmlpoyee)
        {
            Node node = new Node();
            var css = "NotAllow";

            // HungNM. Add full role for Giám đốc. 20201015.
            //if (lstdepartmentEmlpoyee.Contains(department.ID) || User.Administrator)
            if (lstdepartmentEmlpoyee.Contains(department.ID) || User.Administrator || departmentSV.isDirectorRole(User.RoleIDs))
            {
                css = "ForEmployee";
                department.IsSelected = true;//Dung tam prop IsSelectded đánh dấu node nào cho phép đc chọn
            }
            // End.

            node.NodeID = department.ID.ToString();
            node.Text = department.Name.ToUpper();
            node.Cls = css;
            node.Icon = department.ParentID == 0 ? Icon.House : Icon.Door;
            node.CustomAttributes.Add(new ConfigItem { Name = "ParentID", Value = department.ParentID.ToString(), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "UpdateAt", Value = department.UpdateAtView, Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "EstablishedDate", Value = department.EstablishedDateView, Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsCancel", Value = JSON.Serialize(department.IsCancel), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsDelete", Value = JSON.Serialize(department.IsDelete), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsActive", Value = JSON.Serialize(department.IsActive), Mode = ParameterMode.Value });
            node.CustomAttributes.Add(new ConfigItem { Name = "IsSelected", Value = JSON.Serialize(department.IsSelected), Mode = ParameterMode.Value });
            node.Leaf = !department.IsParent;
            return node;
        }
        #region View
        [BaseSystem(Name = "Xem danh sách")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoadData(string node, bool viewDeactive = false, bool viewCancel = false, bool viewMerge = false, bool viewDelete = false, string departmentIds = "")
        {
            try
            {
                NodeCollection nodes = new NodeCollection();
                var departmentID = node == "root" ? 0 : System.Convert.ToInt32(node);
                var departments = departmentSV.GetDepartments(departmentID, viewDeactive, viewCancel, viewMerge, viewDelete, departmentIds);
                foreach (var department in departments)
                {
                    nodes.Add(createNodeDepartment(department));
                }
                return this.Content(nodes.ToJson());
            }
            catch {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult List()
        {
            ViewData["Administrator"] = User.Administrator;
            return PartialView();
        }
        public ActionResult DepartmentForMultiSelect()
        {
            ViewData["Administrator"] = User.Administrator;
            return PartialView();
        }
        public ActionResult LoadTreeData(string node, bool viewDeactive = false, bool viewCancel = false, bool viewMerge = false, bool viewDelete = false, string departmentIds = "")
        {
            try
            {
                NodeCollection nodes = new NodeCollection();
                var departmentID = node == "root" ? 0 : System.Convert.ToInt32(node);
                var departments = departmentSV.GetDepartments(departmentID, viewDeactive, viewCancel, viewMerge, viewDelete, departmentIds);
                var lstDep = departmentSV.getListDepartmentID(User.RoleIDs);
                var lstDep1 = new List<int>();
                foreach(int depID in lstDep)
                {
                    if (!lstDep1.Contains(depID))
                    {
                        var lstTemp = departmentSV.GetListDepartmentByParentID(depID).Select(o => o.ID).ToList();
                        lstDep1.AddRange(lstTemp);
                    }
                }
                lstDep1.AddRange(lstDep);
                foreach (var department in departments)
                {
                    nodes.Add(createNodeDepartment(department,lstDep1));
                }
                return this.Content(nodes.ToJson());
            }
            catch
            {
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
        public ActionResult Create(HumanDepartmentItem department)
        {
            var success = false;
            try
            {
                if (ModelState.IsValid)
                {
                    if (!departmentSV.CheckNameExist(department.Name, department.ParentID))
                    {
                        departmentSV.Insert(department);
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
        public ActionResult Update(int departmentId = 0)
        {
            try
            {
                var data = departmentSV.GetDepartment(departmentId);
                return new Ext.Net.MVC.PartialViewResult { Model = data };
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        [HttpPost]
        public ActionResult Update(HumanDepartmentItem department)
        {
            var success = false;
            try
            {
                if (!departmentSV.CheckNameExist(department.Name, department.ParentID, department.ID))
                {
                    departmentSV.Update(department);
                    Ultility.ShowMessageRequest(RequestStatus.UpdateSuccess);
                    success = true;
                }
                else
                {
                    Ultility.ShowMessageRequest(RequestStatus.ExistError);
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
        #region Delete
        [BaseSystem(Name = "Xóa")]
        [SystemAuthorize(CheckAuthorize = true)]
        public ActionResult Delete()
        {
            return this.Direct(true);
        }
        [HttpPost]
        public ActionResult Delete(int departmentId = 0)
        {
            try
            {
                departmentSV.Delete(departmentId);
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
        public ActionResult Detail(int departmentId = 0)
        {
            try
            {
                var data = departmentSV.GetDepartment(departmentId);
                return new Ext.Net.MVC.PartialViewResult { Model = data };
            }
            catch 
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
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
        public ActionResult Restore(int departmentId = 0)
        {
            try
            {
                departmentSV.Restore(departmentId);
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
        public ActionResult Destroy(int departmentId = 0)
        {
            try
            {
                departmentSV.Destroy(departmentId);
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
            try
            {
                return new Ext.Net.MVC.PartialViewResult {};
            }
            catch 
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        [HttpPost]
        public ActionResult Sort(int departmentId, int parentId, bool sort, bool viewDeactive = false, bool viewCancel = false, bool viewMerge = false, bool viewDelete = false)
        {
            try
            {
                var change = departmentSV.Sort(departmentId, parentId, sort, viewDeactive, viewCancel, viewMerge, viewDelete);
                X.GetCmp<Hidden>("SortDepartmentNode").SetValue(change);
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        public ActionResult Move(string[] ids, string targetId, string[] parentIds, string point)
        {
            try
            {
                var department = departmentSV.GetDepartment(System.Convert.ToInt32(ids[0].ToString().Trim('[', '\\', '\"', ']')));
                department.ParentID = System.Convert.ToInt32(targetId);
                departmentSV.Update(department);
                return this.RemoteTree();
            }
            catch
            {
                Ultility.ShowMessageRequest(RequestStatus.Error);
            }
            return this.Direct();
        }
        #endregion
        #region Permission
        [BaseSystem(Name = "Phân quyền")]
        public ActionResult Permission()
        {
            return View();
        }
        #endregion
        #region DepartmentWindow
        public ActionResult DepartmentWindow(string fn = "", bool multi = true, string departmentIds = "")
        {
            ViewData["Fn"] = fn;
            ViewData["Multi"] = multi;
            ViewData["DepartmentIDs"] = departmentIds;
            return new Ext.Net.MVC.PartialViewResult
            {
                ViewData = ViewData,
            };
        }
        #endregion
        public ActionResult DepartmentMultiViewWindow(string ids)
        {
            var result = new List<HumanDepartmentViewItem>();
            if (!string.IsNullOrWhiteSpace(ids))
            {
                var intIds = ids.Split(',').Select(i => System.Convert.ToInt32(i)).ToArray();
                result = departmentSV.GetByIds(intIds);
            }
            return new Ext.Net.MVC.PartialViewResult { ViewName = "DepartmentMultiViewWindow", Model = result };
        }
        
    }
}
