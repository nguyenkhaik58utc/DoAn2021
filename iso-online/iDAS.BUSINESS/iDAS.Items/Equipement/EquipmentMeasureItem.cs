using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class EquipmentMeasureItem
    {
        public int ID { get; set; }
        public int EquipmentMeasureCategoryID { get; set; }
        public string EquipmentMeasureCategoryName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public string MadeIn { get; set; }
        public Nullable<System.DateTime> MadeYear { get; set; }
        public Nullable<System.DateTime> UseStartTime { get; set; }
        public string Specifications { get; set; }
        public string Features { get; set; }
        public string SupportInfomation { get; set; }
        public Nullable<float> ScopeStart { get; set; }
        public Nullable<float> ScopeEnd { get; set; }
        public string ScopeUnit { get; set; }
        public Nullable<float> Accuracy { get; set; }
        public string CalibrationPeriodic { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public bool IsUsing { get; set; }
        public bool IsError { get; set; }
        public bool IsFixed { get; set; }
        public bool IsCalibration { get; set; }
        public bool IsAccreditation { get; set; }
        public Nullable<System.DateTime> CalibrationLastTime { get; set; }
        public Nullable<System.DateTime> CalibrationNextTime { get; set; }
        public Nullable<System.DateTime> ExprireTime { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }

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
            if (IsFixed) { status = EStatus.IsFixed; }
            if (IsAccreditation) { status = EStatus.IsAccreditation; }
            if (IsCalibration) { status = EStatus.IsCalibration; }
            switch (status)
            {
                case EStatus.NotUsing: result = "<span style=\"color:#045fb8\">Chưa sử dụng</span>"; break;
                case EStatus.Using: result = "<span style=\"color:#293955\">Đang sử dụng</span>"; break;
                case EStatus.Error: result = "<span style=\"color:blue\">Đã hỏng</span>"; break;
                case EStatus.IsFixed: result = "<span style=\"color:red\">Đã sửa</span>"; break;
                case EStatus.IsAccreditation: result = "<span style=\"color:#8e210b\">Đã kiểm định</span>"; break;
                case EStatus.IsCalibration: result = "<span style=\"color:#3300f1\">Đã hiệu chuẩn</span>"; break;
            }
            return result;
        }
        private enum EStatus
        {
            NotUsing,
            Using,
            Error,
            IsFixed,
            IsCalibration,
            IsAccreditation
        }
        public string ActionForm { get; set; }

        public AttachmentFileItem AttachmentFiles { get; set; }
    }
}
