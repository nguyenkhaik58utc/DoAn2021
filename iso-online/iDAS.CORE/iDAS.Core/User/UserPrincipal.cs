using System.Collections.Generic;
using System.Security.Principal;

namespace iDAS.Core
{
    /// <summary>
    /// User principal
    /// </summary>
    public class UserPrincipal : IPrincipal, IUserLogin
    {
        public UserPrincipal() { }

        public UserPrincipal(IIdentity identity)
        {
            this._Identity = identity;
        }

        private IIdentity _Identity;
        private int _ID;
        private int _EmployeeID;
        private string _Name = string.Empty;
        private string _Account = string.Empty;
        private string _Password = string.Empty;
        private string _Email = string.Empty;
        private string _Code = string.Empty;
        private string _Logo = string.Empty;
        private string _Avatar = string.Empty;
        private string _Languague = string.Empty;
        private bool _Remember = true;
        private bool _Administrator = false;
        private bool _IsLocked = false;
        private bool _IsActivated = false;
        private bool _IsResetUser = true;
        private List<int> _RoleIDs = new List<int>();
        private List<int> _GroupIDs = new List<int>();

        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }
        public int EmployeeID
        {
            get
            {
                return _EmployeeID;
            }
            set
            {
                _EmployeeID = value;
            }
        }
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }
        public string Account
        {
            get
            {
                return _Account;
            }
            set
            {
                _Account = value;
            }
        }
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
            }
        }
        public string Email
        {
            get
            {
                if (string.IsNullOrEmpty(_Email))
                {
                    _Email = string.Empty;
                }
                return _Email;
            }
            set
            {
                _Email = value;
            }
        }
        public string Code
        {
            get
            {
                return _Code;
            }
            set
            {
                _Code = value;
            }
        }
        public string SystemCode
        {
            get
            {
                return BaseConfig.SystemCode;
            }
        }
        public string Logo
        {
            get
            {
                return _Logo;
            }
            set
            {
                _Logo = value;
            }
        }
        public string Avatar
        {
            get
            {
                return _Avatar;
            }
            set
            {
                _Avatar = value;
            }
        }
        public string Languague
        {
            get
            {
                return _Languague;
            }
            set
            {
                _Languague = value;
            }
        }
        public bool Remember
        {
            get
            {
                return _Remember;
            }
            set
            {
                _Remember = value;
            }
        }
        public bool Administrator
        {
            get { return _Administrator; }
            set { _Administrator = value; }
        }
        public bool IsLocked
        {
            get { return _IsLocked; }
            set { _IsLocked = value; }
        }
        public bool IsActivated
        {
            get { return _IsActivated; }
            set { _IsActivated = value; }
        }
        public List<int> RoleIDs
        {
            get { return _RoleIDs; }
            set { _RoleIDs = value; }
        }
        public List<int> GroupIDs
        {
            get { return _GroupIDs; }
            set { _GroupIDs = value; }
        }
        public string PathUpload
        {
            get { return BaseConfig.PathUpload; }
        }
        internal bool IsResetUser
        {
            get
            {
                return _IsResetUser;
            }
            set
            {
                _IsResetUser = value;
            }
        }

        /// <summary>
        /// Initialize indentity of the current principal
        /// </summary>
        public void SetIdentity()
        {
            _Identity = new GenericIdentity(Name);
        }

        /// <summary>
        /// Get the identity of the current principal
        /// </summary>
        public IIdentity Identity
        {
            get
            {
                return _Identity;
            }
        }

        /// <summary>
        /// Determines whether the current principal belongs to the specified role
        /// </summary>
        public bool IsInRole(string role)
        {
            return false;
        }


    }
}
