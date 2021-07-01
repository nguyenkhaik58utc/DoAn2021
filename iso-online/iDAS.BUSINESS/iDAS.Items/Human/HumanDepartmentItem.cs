using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class HumanDepartmentItem
    {
        private bool _IsActive = false;
        private bool _IsCancel = false;
        private bool _IsDelete = false;
        private bool _IsParent = false;
        private int _Position = 0;

        public int ID { get; set; }
        public int ParentID { get; set; }
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string NameE { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Tax { get; set; }
        public string Website { get; set; }
        public DateTime? EstablishedDate { get; set; }
        public string EstablishedDateView
        {
            get
            {
                if (EstablishedDate.HasValue)
                    return EstablishedDate.Value.ToString("dd-MM-yyyy");
                else return string.Empty;
            }
        }
        public bool IsActive
        {
            get
            {
                if (IsCancel || IsMerge || IsDelete)
                    return false;
                return _IsActive;
            }
            set
            {
                _IsActive = value;
            }
        }
        public bool IsCancel
        {
            get
            {
                return _IsCancel;
            }
            set
            {
                _IsCancel = value;
            }
        }
        public bool IsMerge { get; set; }
        public bool IsDelete
        {
            get
            {
                return _IsDelete;
            }
            set
            {
                _IsDelete = value;
            }
        }
        public int? CreateBy { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string UpdateAtView
        {
            get
            {
                if (UpdateAt.HasValue)
                    return UpdateAt.Value.ToString("dd-MM-yyyy");
                else
                {
                    return string.Empty;
                }
            }
        }
        public string CreateByName { get; set; }
        public string UpdateByName { get; set; }
        public string Resposibility { get; set; }
        public string Authorize { get; set; }
        public Nullable<int> Position
        {
            get
            {
                return _Position;
            }
            set
            {
                _Position = value.HasValue ? value.Value : 0;
            }
        }
        public bool IsParent
        {
            get
            {
                return _IsParent;
            }
            set
            {
                _IsParent = value;
            }
        }
        public bool IsParentActive
        {
            get;
            set;
        }
        public string History { get; set; }
        public bool IsSelected { get; set; }
    }
    public class DepartmentTreeItem
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsUse { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsCancel { get; set; }
        public bool Leaf { get; set; }
    }
}
