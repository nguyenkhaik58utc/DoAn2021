using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ApprovalItem
    {
        private bool _IsEdit = true;
        private bool _IsAccept;
        private bool _IsApproval;
        private bool? _IsResult = null;
        private bool _IsSendApproval = false;
        private Nullable<int> _ApprovalBy;
        private HumanEmployeeViewItem _Approval;
        private Nullable<DateTime> _ApprovalAt = DateTime.Now;
        public bool IsApproval
        {
            get { return _IsApproval; }
            set { _IsApproval = value; }
        }
        public bool IsEdit
        {
            get { return _IsEdit; }
            set { _IsEdit = value; }
        }
        public bool IsAccept
        {
            get { return _IsAccept; }
            set
            {
                if (value)
                {
                    _IsEdit = false;
                }
                _IsAccept = value;
            }
        }
        public bool? IsResult
        {
            get
            {
                if (_IsApproval)
                {
                    _IsResult = _IsAccept;
                }
                return _IsResult;
            }
            set
            {
                if (value.HasValue)
                {
                    _IsApproval = true;
                    IsAccept = value.Value;
                }
                _IsResult = value;
            }
        }
        public HumanEmployeeViewItem Approval
        {
            get
            {
                return _Approval;
            }
            set
            {
                if (value !=null && value.ID != 0)
                {
                    _ApprovalBy = value.ID;
                }
                _Approval = value;
            }
        }
        public Nullable<int> ApprovalBy
        {
            get { return _ApprovalBy; }
            set
            {
                if (value.HasValue)
                {
                    if (Approval == null) Approval = new HumanEmployeeViewItem();
                    Approval.ID = value.Value;
                }
                _ApprovalBy = value;
            }
        }
        public Nullable<System.DateTime> ApprovalAt
        {
            get
            {
                if (_IsEdit == true && _IsApproval == false)
                    return null;
                else return _ApprovalAt;
            }
            set
            {
                if (value.HasValue)
                {
                    _ApprovalAt = value.Value;
                }
            }
        }
        public string ApprovalNote { get; set; }
        public bool IsSendApproval
        {
            get { return _IsSendApproval; }
            set
            {
                if (value)
                {
                    _IsEdit = false;
                    _IsApproval = false;
                }
                _IsSendApproval = value;
            }
        }
    }

}
