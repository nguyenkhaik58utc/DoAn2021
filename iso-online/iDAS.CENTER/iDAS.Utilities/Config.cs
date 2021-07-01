using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Utilities
{
    public enum RequestStatus
    {
        #region Form Handle
        Error,
        ValidError,
        CreateSuccess,
        CreateError,
        UpdateSuccess,
        UpdateError,
        DeleteSuccess,
        DeleteError,
        DestroySuccess,
        DestroyError,
        RestoreSuccess,
        RestoreError,
        MoveSuccess,
        MoveError,
        ExistError,
        #endregion

        #region Form Login
        LoginSuccess,
        LoginFail,
        LoginInfoIncorrect,
        LoginNotActivated,
        LoginCodeIncorrect,
        CookiesNotSupport,
        #endregion
    }
    public class RequestMessage
    {
        public const string Notify = "Thông báo!";

        #region Form Handle
        public const string Error = "Hệ thống xử lý phát sinh lỗi!";
        public const string ValidError = "Thông tin dữ liệu chưa phù hợp!";
        public const string CreateSuccess = "Đã thêm mới dữ liệu thành công!";
        public const string CreateError = "Thêm mới dữ liệu không thành công!";
        public const string UpdateSuccess = "Đã cập nhật dữ liệu thành công!";
        public const string UpdateError = "Cập nhật dữ liệu không thành công!";
        public const string DeleteSuccess = "Đã xóa dữ liệu thành công!";
        public const string DeleteError = "Xóa dữ liệu không thành công!";
        public const string DestroySuccess = "Đã hủy bỏ dữ liệu thành công!";
        public const string DestroyError = "Hủy bỏ dữ liệu không thành công!";
        public const string RestoreSuccess = "Đã phục hồi dữ liệu thành công!";
        public const string RestoreError = "Phục hồi dữ liệu không thành công!";
        public const string MoveSuccess = "Đã chuyển bản ghi dữ liệu thành công!";
        public const string MoveError = "Chuyển bản ghi dữ liệu không thành công!";
        public const string ExistError = "Hệ thống đã tồn tại bản ghi này!";
        #endregion

        #region Form Login
        public const string LoginSuccess = "Đăng nhập hệ thống thành công!";
        public const string LoginFail = "Đăng nhập hệ thống không thành công!";
        public const string LoginInfoIncorrect = "Thông tin tài khoản hoặc mật khẩu không phù hợp!";
        public const string LoginNotActivated = "Tài khoản chưa được kích hoạt!";
        public const string LoginCodeIncorrect = "Thông tin mã khách hàng không đúng hoặc chưa được kích hoạt!";
        public const string CookiesNotSupport = "Trình duyệt không hỗ trợ Cookie!";
        public const string DataUsing = "Dữ liệu đang sử dụng";
        public const string NameExist = "Dữ liệu bị trùng tên";
        #endregion
    }
}
