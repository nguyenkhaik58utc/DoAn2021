using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iDAS.Core;

namespace iDAS.Web
{
    public class Message
    {
        public const string Notify = "Thông báo!";
        public const string InsertSuccess = "Đã tạo mới dữ liệu thành công!";
        public const string InsertError = "Tạo mới dữ liệu không thành công!";
        public const string UpdateSuccess = "Đã cập nhật dữ liệu thành công!";
        public const string UpdateError = "Cập nhật dữ liệu không thành công!";
        public const string DeleteSuccess = "Đã xóa dữ liệu thành công!";
        public const string DeleteError = "Xóa dữ liệu không thành công!";
        public const string RestoreSuccess = "Đã phục hồi dữ liệu thành công!";
        public const string RestoreError = "Phục hồi dữ liệu không thành công!";
        public const string LoginSuccess = "Đăng nhập hệ thống thành công!";
        public const string LoginFail = "Đăng nhập hệ thống không thành công!";
        public const string LoginInfoIncorrect = "Thông tin tài khoản hoặc mật khẩu không phù hợp!";
        public const string LoginNotActivated = "Tài khoản chưa được kích hoạt!";
        public const string LoginCodeIncorrect = "Thông tin mã khách hàng không đúng hoặc chưa được kích hoạt!";
        public const string RegisterSuccess = "Đăng ký tài khoản thành công!";
        public const string RegisterFail = "Đăng ký tài khoản không thành công!";
        public const string AccountExist = "Tài khoản đã tồn tại trên hệ thống!";
        public const string CookiesNotSupport = "Trình duyệt không hỗ trợ Cookie!";
        public const string CreateDatabaseSuccess = "Đã tạo Database thành công!";
        public const string CreateDatabaseError = "Tạo Database không thành công!";
        public const string DeleteDatabaseSuccess = "Đã xóa Database thành công!";
        public const string DeleteDatabaseError = "Xóa Database không thành công!";
        public const string NotFileImage = "Không đúng định dạng file ảnh!";
        public const string NotFileUpload = "Không có thông tin cho file upload!";
        public const string UploadFileFail = "Tiến trình tải tệp lỗi!"; 
    }

    public class Status {
        public const string Obsolete = "<font color='Gray'><b>Lỗi thời</b></font>";
        public const string Using = "<font color='LimeGreen'><b>Đang sử dụng</b></font>";
        public const string New = "<font color='HotPink'><b>Mới</b></font>";
    }

    public enum FormatFile
    {
        Image,
        Text,
        Video,
        Audio,
        Word,
        Excel,
        Pdf,
    }

    public class PathUpload {
        public const string Logo = "/Upload/Logo/"; 
    }

    public class Code
    {
        public const string ProductCategory = "ProductCategory";
        public const string CustomerCategory = "CustomerCategory";
        public const string ContactMethod = "ContactMethod";
        public const string CareMethod = "CareMethod";
    }
}