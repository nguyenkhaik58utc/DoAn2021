using System;

namespace iDAS.Core
{
    /// <summary>
    /// Base System Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class BaseSystemAttribute : Attribute
    {
        private string _Name = string.Empty;
        private bool _IsActive = true;
        private bool _IsShow = false;
        private bool _IsGroup = false;
        private int _Position = 0;
        private string _Parent = string.Empty;
        private string _StartAction = "Index";
        private string _Icon = string.Empty;

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

        public bool IsActive
        {
            get
            {
                return _IsActive;
            }
            set
            {
                _IsActive = value;
            }
        }

        public bool IsShow
        {
            get
            {
                return _IsShow;
            }
            set
            {
                _IsShow = value;
            }
        }

        public bool IsGroup
        {
            get
            {
                return _IsGroup;
            }
            set
            {
                _IsGroup = value;
            }
        }

        public int Position
        {
            get
            {
                return _Position;
            }
            set
            {
                _Position = value;
            }
        }

        public string Parent
        {
            get
            {
                return _Parent;
            }
            set
            {
                _Parent = value;
            }
        }

        public string StartAction
        {
            get
            {
                return _StartAction;
            }
            set
            {
                _StartAction = value;
            }
        }

        public string Icon
        {
            get
            {
                return _Icon;
            }
            set
            {
                _Icon = value;
            }
        }
    }
}
