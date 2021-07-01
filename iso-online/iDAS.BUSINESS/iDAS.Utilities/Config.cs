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
        #endregion

        public const string NotFileImage = "Không đúng định dạng file ảnh!";
        public const string NotFileUpload = "Không có thông tin cho file upload!";
        public const string UploadFileFail = "Tiến trình tải tệp lỗi!";
        public const string ApproveSuccess = "Phê duyệt thành công";
        public const string ApproveError = "Phê duyệt không thành công";
        public const string NotPermission = "Bạn không có quyền thực hiện chức năng này";
        public const string ApproveByAnother = "Hồ sơ này đã được phê duyệt bởi người khác";
        public const string Approving = "Hồ sơ đang trong quá trình phê duyệt";
        public const string SendSuccess = "Gửi thành công";
        public const string SendError = "Gửi không thành công";
        public const string ActiveSuccess = "Kích hoạt thành công";
        public const string ActiveError = "Kích hoạt thất bại";
        public const string DeActiveSuccess = "Hủy kích hoạt thành công";
        public const string DeActiveError = "Hủy kích hoạt thất bại";
        public const string NotReview = "Hồ sơ chưa có kết quả đánh giá";
        public const string DataUsing = "Dữ liệu đang sử dụng";
        public const string NameExist = "Dữ liệu bị trùng tên";
        public const string ImportSucess = "Nhập dữ liệu thành công";
        public const string ImportError = "Có lỗi trong quá trình nhập dữ liệu";
        public const string ExportSucess = "Xuất dữ liệu thành công";
        public const string ExportError = "Có lỗi trong quá trình xuất dữ liệu";
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

    public class PathUpload
    {
        public const string Logo = "/Upload/Logo/";
        public const string Root = "/Upload/";
        public const string HumanEmployeeAvatar = "/Human/Emloyee/Avatar/";
        public const string HumanRecruitmentAvatar = "/Human/Recruitment/Avatar/";
        public const string CustomerAvatar = "/Customer/Avatar";
    }

    public class Code
    {
        public const string ProductCategory = "ProductCategory";
        public const string CustomerCategory = "CustomerCategory";
        public const string ContactMethod = "ContactMethod";
        public const string CareMethod = "CareMethod";
    }
    public class Form
    {
        public const string Add = "add";
        public const string Edit = "edit";
        public const string View = "view";
        public const string Send = "send";
        public const string Approve = "approve";
        public const string InterView = "interview";
        public const string Task = "task";
    }
    public class CustomerStatus
    {
        public const String New = "IsNew";
        public const String Potential = "IsPotential";
        public const String Official = "IsOfficial";
        public const String Pause = "IsPause";
        public const String Finish = "IsFinish";
    }
    public class StoreParam
    {
        public const string DepartmentStore = "StoreDepartmentView";
    }
    public class Password
    {
        public const string EmptyPass = "***********";
    }
    public class DateFormat
    {
        public const string Date = "dd/MM/yyyy";
        public const string DateTime = "dd/MM/yyyy hh:mm";
        public const string StringDateTime = "dateString";
    }
    public class DateFormatFunction
    {
        public const string DateFunction = @"function (value) {
                                                                if(value)
                                                                {
                                                                    var datetime = new Date(value);
                                                                    var date = datetime.getDate();
                                                                    var month = datetime.getMonth()+1;
                                                                    var year = datetime.getFullYear();
                                                                    if (date < 10) date = '0' + date.toString();
                                                                    if (month < 10) month = '0' + month.toString();
                                                                    return date + '/' + month + '/' + year;
                                                                }
                                                            }";
        public const string DateTimeFunction = @"function (value) {
                                                                if(value)
                                                                {
                                                                    var datetime = new Date(value);
                                                                    var date = datetime.getDate();
                                                                    var month = datetime.getMonth()+1;
                                                                    var year = datetime.getFullYear();
                                                                    var hours = datetime.getHours();
                                                                    var minutes = datetime.getMinutes();
                                                                    if (date < 10) date = '0' + date.toString();
                                                                    if (month < 10) month = '0' + month.toString();
                                                                    if (hours < 10) hours = '0' + hours.toString();
                                                                    if (minutes < 10) minutes = '0' + minutes.toString();
                                                                    return date + '/' + month + '/' + year +'   '+hours+':'+minutes ;
                                                                }
                                                            }";
        public const string StringDateTimeFunction = @"function (value) {
                                                                    if(value)
                                                                    {
                                                                        var datetime = new Date(value);
                                                                        return datetime.toLocaleDateString();
                                                                    }
                                                                }";
    }
}
