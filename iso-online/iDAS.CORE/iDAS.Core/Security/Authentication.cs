using Newtonsoft.Json;
using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace iDAS.Core
{
    public enum ELogin
    {
        Success,
        Fail,
        Locked,
        NotSupportCookie,
        NotActivated,
        ErrorInfo,
        ErrorCode,
    }

    /// <summary>
    /// Manager Authenticate
    /// </summary>
    public class SystemAuthenticate
    {
        public SystemAuthenticate() { }

        private static string _LoginUrl = FormsAuthentication.LoginUrl;
        private static string _LoginSuccessUrl = FormsAuthentication.DefaultUrl;
        private static string _AccessDeniedUrl = BaseConfig.AccessDeniedUrl;
        private static string _AccessDeniedScript = BaseConfig.AccessDeniedScript;

        public static string LoginUrl
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
        public static string LoginSuccessUrl
        {
            get
            {
                if (HttpContext.Current.Session["LoginSuccessUrl"] == null)
                {
                    HttpContext.Current.Session["LoginSuccessUrl"] = _LoginSuccessUrl;
                }
                return HttpContext.Current.Session["LoginSuccessUrl"] as string;
            }
            set
            {
                HttpContext.Current.Session["LoginSuccessUrl"] = value;
            }
        }
        public static string AccessDeniedUrl
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
        public static string AccessDeniedScript
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

        /// <summary>
        /// Set user principal to system
        /// </summary>
        public static void SetUserPrincipal()
        {
            var userPrincipal = GetAuthCookies();
            if (userPrincipal == null || !userPrincipal.IsActivated || userPrincipal.IsLocked)
            {
                userPrincipal = new UserPrincipal();
                userPrincipal.IsResetUser = false;
            }
            userPrincipal.SetIdentity();
            HttpContext.Current.User = userPrincipal;
        }

        /// <summary>
        /// Reset user principal of system
        /// </summary>
        public static void ResetUserPrincipal()
        {
            var userPrincipal = HttpContext.Current.User as UserPrincipal;
            if (userPrincipal != null && userPrincipal.IsResetUser)
            {
                //case system center's database not config
                if (BaseDatabase.DatabaseCenter == null)
                {
                    HttpContext.Current.Response.Redirect(BaseConfig.ConfigUrl, true);
                }
                else
                {
                    BaseUser.SetDatabaseByCode(userPrincipal.Code);

                    if (BaseDatabase.DatabaseByCode != null)
                    {
                        userPrincipal = BaseUser.GetUserPrincipal(userPrincipal);
                    }
                    if (BaseDatabase.DatabaseByCode == null || userPrincipal == null || !userPrincipal.IsActivated || userPrincipal.IsLocked)
                    {
                        userPrincipal = new UserPrincipal();
                        SetAuthCookies(userPrincipal);
                    }
                }
                userPrincipal.SetIdentity();
                HttpContext.Current.User = userPrincipal;
            }
        }

        /// <summary>
        /// Get info of user principal from cookies
        /// </summary>
        public static UserPrincipal GetAuthCookies()
        {
            IPrincipal _IUser = HttpContext.Current.User;
            UserPrincipal user = null;
            if (_IUser != null && _IUser.Identity.IsAuthenticated && _IUser.Identity.AuthenticationType == "Forms")
            {
                user = JsonConvert.DeserializeObject<UserPrincipal>(_IUser.Identity.Name);
                decryptUserPrincipal(user);
            }
            return user;
        }

        /// <summary>
        /// Set info of user pricipal to cookies
        /// </summary>
        public static void SetAuthCookies(UserPrincipal user)
        {
            if (user != null)
            {
                encryptUserPrincipal(user);
                string data = JsonConvert.SerializeObject(user);
                FormsAuthentication.SetAuthCookie(data, user.Remember);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(string.Empty, true);
            }
        }

        /// <summary>
        /// Login system
        /// </summary>
        public static ELogin Login(IUserLogin user)
        {
            ELogin error;
            try
            {
                if (!FormsAuthentication.CookiesSupported) error = ELogin.NotSupportCookie;
                else
                {
                    BaseUser.SetDatabaseByCode(user.Code);
                    if (BaseDatabase.DatabaseByCode == null)
                        error = ELogin.ErrorCode;
                    else
                        error = authenticate(user);
                }
            }
            catch(Exception ex)
            {                
                error = ELogin.Fail;
            }
            return error;
        }

        /// <summary>
        /// Logout system
        /// </summary>
        public static void Logout()
        {
            FormsAuthentication.SignOut();
        }

        private static ELogin authenticate(IUserLogin user)
        {
            ELogin error = ELogin.ErrorInfo;
            var userPrincipal = BaseUser.GetUserPrincipal(user);
            if (userPrincipal != null)
            {
                if (!userPrincipal.IsActivated) error = ELogin.NotActivated;
                else
                    if (userPrincipal.IsLocked) error = ELogin.Locked;
                else
                {
                    error = ELogin.Success;
                    SetAuthCookies(userPrincipal);
                }
            }
            return error;
        }

        private static void encryptUserPrincipal(UserPrincipal user)
        {
            try
            {
                user.Name = Encryptor.Encode(user.Name);
                user.Account = Encryptor.Encode(user.Account);
                user.Password = Encryptor.Encode(user.Password);
                user.Email = Encryptor.Encode(user.Email);
                // HungNM. 20200915.
                //user.Code = Encryptor.Encode(user.Code);
                user.Code = Encryptor.Encode(user.Code == null ? "" : user.Code);
                // End.
            }
            catch
            {
                throw new ArgumentException();
            }
        }

        private static void decryptUserPrincipal(UserPrincipal user)
        {
            try
            {
                user.Name = Encryptor.Decode(user.Name);
                user.Account = Encryptor.Decode(user.Account); ;
                user.Password = Encryptor.Decode(user.Password);
                user.Email = Encryptor.Decode(user.Email);
                // HungNM. 20200915.
                //user.Code = Encryptor.Decode(user.Code);
                user.Code = Encryptor.Decode(user.Code == null ? "" : user.Code);
                // End.
            }
            catch
            {
                throw new ArgumentException();
            }
        }
    }
}
