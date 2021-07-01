using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class EquipmentProductionItem
    {
        public int ID { get; set; }
        public int EquipmentProductionCategoryID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string MadeIn { get; set; }
        public Nullable<System.DateTime> MadeYear { get; set; }
        public Nullable<System.DateTime> UseStartTime { get; set; }
        public string Specifications { get; set; }
        public string MaintainContent { get; set; }
        public string MaintainOther { get; set; }
        public Nullable<int> MaintainPeriodic { get; set; }
        public string SupportInfomation { get; set; }
        public string Features { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public bool IsUsing { get; set; }
        public bool IsError { get; set; }
        public bool IsFixed { get; set; }
        public bool IsMaintain { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public AttachmentFileItem AttachmentFiles { get; set; }
        public string ActionForm { get; set; }
        public string Status
        {
            get
            {
                return getStatus();
            }
        }
        private string getStatus()
        {
            string result = "";
            EStatus status = EStatus.NotUsing;
            if (IsUsing)
            {
                status = EStatus.Using;
            }
            if (IsError) { status = EStatus.Error; }
            if (IsFixed) { status = EStatus.Fix; }
            if (IsMaintain) { status = EStatus.Maintain; }
            switch (status)
            {
                case EStatus.NotUsing: result = "<span style=\"color:#045fb8\">Chưa sử dụng</span>"; break;
                case EStatus.Using: result = "<span style=\"color:#293955\">Đang sử dụng</span>"; break;
                case EStatus.Error: result = "<span style=\"color:blue\">Đang hỏng</span>"; break;
                case EStatus.Fix: result = "<span style=\"color:red\">Đang sửa</span>"; break;
                case EStatus.Maintain: result = "<span style=\"color:#8e210b\">Đang bảo dưỡng</span>"; break;
            }
            return result;
        }
        private enum EStatus
        {
            NotUsing,
            Using,
            Error,
            Fix,
            Maintain
        }
    }
}
