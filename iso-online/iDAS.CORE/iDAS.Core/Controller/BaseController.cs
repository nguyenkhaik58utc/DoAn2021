using System;
using System.Threading;
using System.Web.Mvc;
namespace iDAS.Core
{
    /// <summary>
    /// Base Controller
    /// </summary>
    public class BaseController : Controller
    {
        public BaseController() { }

        private UserPrincipal _User;
        private string _ModuleCode;
        private string _FunctionCode;
        private string _ActionCode;

        public new UserPrincipal User
        {
            get
            {
                if (_User == null)
                {
                    _User = base.User as UserPrincipal;
                }
                return _User;
            }
        }
        public string ModuleCode
        {
            get
            {
                if (string.IsNullOrEmpty(_ModuleCode))
                {
                    _ModuleCode = this.GetAreaNameController();
                }
                return _ModuleCode;
            }
        }
        public string FunctionCode
        {
            get
            {
                if (string.IsNullOrEmpty(_FunctionCode))
                {
                    _FunctionCode = this.GetNameController();
                }
                return _FunctionCode;
            }
        }
        public string ActionCode
        {
            get
            {
                if (string.IsNullOrEmpty(_ActionCode))
                {
                    _ActionCode = "Index";
                }
                return _ActionCode;
            }
            set
            {
                _ActionCode = value;
            }
        }

        public bool IsInternal
        {
            get
            {
                if (Session["Internal"] == null)
                    return false;
                else
                    return (bool)Session["Internal"];
            }
            set { Session["Internal"] = value; }
        }

        /// <summary>
        /// Check permission of user 
        /// </summary>
        public bool CheckPermission(string actionCode = "", int departmentId = 0)
        {
            bool flag = false;
            if (this.User == null)
            {
                return flag;
            }
            if (!string.IsNullOrEmpty(actionCode))
            {
                this.ActionCode = actionCode;
            }
            return this.CheckPermission(this.ModuleCode, this.FunctionCode, this.ActionCode, departmentId);
        }

        /// <summary>
        /// Check permission of user 
        /// </summary>
        public bool CheckPermission(string moduleCode, string functionCode, string actionCode, int departmentId = 0)
        {
            bool flag = this.checkPermissionAdmin();
            if (flag)
            {
                return flag;
            }
            if (departmentId != 0)
            {
                return BaseUser.CheckPermissions(moduleCode, functionCode, actionCode, this.User.RoleIDs, departmentId);
            }
            return BaseUser.CheckPermissions(moduleCode, functionCode, actionCode, this.User.RoleIDs);
        }

        /// <summary>
        /// Check permission administrator
        /// </summary>
        private bool checkPermissionAdmin()
        {
            return this.User.Administrator;
        }

        /// <summary>
        /// Setup global culture of user 
        /// </summary>
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string culture = BaseCulture.GetCultureOfUserPrincipal(User);
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            return base.BeginExecuteCore(callback, state);
        }
    }
}
