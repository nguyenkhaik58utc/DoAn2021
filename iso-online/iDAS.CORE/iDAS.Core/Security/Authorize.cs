using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
namespace iDAS.Core
{
    /// <summary>
    /// Authorize user system
    /// </summary>
    public class SystemAuthorizeAttribute : AuthorizeAttribute
    {
        private string _LoginUrl = FormsAuthentication.LoginUrl;
        private string _AccessDeniedUrl = SystemAuthenticate.AccessDeniedUrl;
        private string _AccessDeniedScript = SystemAuthenticate.AccessDeniedScript;
        private bool _AllowAnonymous = false;
        private bool _CheckAuthorize = true;
        private string _ModuleCode = string.Empty;
        private string _FunctionCode = string.Empty;
        private string _ActionCode = string.Empty;

        public string LoginUrl
        {
            get
            {
                return _LoginUrl;
            }
            set
            {
                _LoginUrl = value;
            }
        }
        public string AccessDeniedUrl
        {
            get
            {
                return _AccessDeniedUrl;
            }
            set
            {
                _AccessDeniedUrl = value;
            }
        }
        public string AccessDeniedScript
        {
            get
            {
                return _AccessDeniedScript;
            }
            set
            {
                _AccessDeniedScript = value;
            }
        }
        public bool AllowAnonymous
        {
            get
            {
                return _AllowAnonymous;
            }
            set
            {
                _AllowAnonymous = value;
            }
        }
        public bool CheckAuthorize
        {
            get
            {
                return _CheckAuthorize;
            }
            set
            {
                _CheckAuthorize = value;
            }
        }
        public string ModuleCode
        {
            get
            {
                return _ModuleCode;
            }
            set
            {
                _ModuleCode = value;
            }
        }
        public string FunctionCode
        {
            get
            {
                return _FunctionCode;
            }
            set { _FunctionCode = value; }
        }
        public string ActionCode
        {
            get
            {
                return _ActionCode;
            }
            set { _ActionCode = value; }
        }
        internal bool CheckInternal
        {
            get
            {
                if (HttpContext.Current.Session["Internal"] == null)
                {
                    return false;
                }
                else
                {
                    return (bool)HttpContext.Current.Session["Internal"];
                }
            }
        }


        /// <summary>
        /// Handle authenticate and authorize of user
        /// </summary>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            //case allow anonymou
            if (_AllowAnonymous) return;

            //case request is the server's internal 
            if (CheckInternal) return;

            //case user is not authenticated 
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult(LoginUrl);
            }

            //case user is authenticated
            else
            {
                var permission = true;
                //case check authorize 
                if (_CheckAuthorize)
                {
                    var authorizes = filterContext.ActionDescriptor.GetCustomAttributes(typeof(SystemAuthorizeAttribute), false);
                    foreach (SystemAuthorizeAttribute authorize in authorizes)
                    {
                        permission = permission && authorize.CheckAuthorize == false;
                    }
                    if (!permission)
                    {
                        var controller = filterContext.Controller as BaseController;
                        if (!string.IsNullOrEmpty(_ModuleCode) || !string.IsNullOrEmpty(_FunctionCode) || !string.IsNullOrEmpty(_ActionCode))
                        {
                            _ModuleCode = string.IsNullOrEmpty(_ModuleCode) ? controller.ModuleCode : _ModuleCode;
                            _FunctionCode = string.IsNullOrEmpty(_FunctionCode) ? controller.FunctionCode : _FunctionCode;
                            _ActionCode = string.IsNullOrEmpty(_ActionCode) ? filterContext.ActionDescriptor.ActionName : _ActionCode;
                            permission = controller.CheckPermission(_ModuleCode, _FunctionCode, _ActionCode);
                        }
                        else
                        {
                            controller.ActionCode = filterContext.ActionDescriptor.ActionName;
                            permission = controller.CheckPermission();
                        }
                    }
                }

                //case authorized is false
                if (!permission)
                {

                    if (string.IsNullOrEmpty(AccessDeniedScript))
                    {
                        AccessDeniedUrl = string.IsNullOrEmpty(AccessDeniedUrl) ? LoginUrl : AccessDeniedUrl;
                        filterContext.Result = new RedirectResult(AccessDeniedUrl);
                    }
                    else
                    {
                        filterContext.Result = new JavaScriptResult() { Script = AccessDeniedScript };
                    }
                }
            }
        }
    }
}
