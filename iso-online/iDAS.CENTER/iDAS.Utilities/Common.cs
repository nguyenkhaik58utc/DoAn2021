using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Linq;
using System.Net.Mail;
using System.Collections.Generic;


namespace iDAS.Utilities
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
        public const string DeleteErrorByUsed = "Xóa dữ liệu không thành công do đang được sử dụng!";
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
        public const string DataExist = "Bản ghi đã tồn tại!";
        public const string CookiesNotSupport = "Trình duyệt không hỗ trợ Cookie!";
        public const string CreateDatabaseSuccess = "Đã tạo Database thành công!";
        public const string CreateDatabaseError = "Tạo Database không thành công!";
        public const string DeleteDatabaseSuccess = "Đã xóa Database thành công!";
        public const string DeleteDatabaseError = "Xóa Database không thành công!";
        public const string NotFileImage = "Không đúng định dạng file ảnh!";
        public const string NotFileUpload = "Không có thông tin cho file upload!";
        public const string UploadFileFail = "Tiến trình tải tệp lỗi!";
    }
    public class Common
    {
        /// <summary>
        /// 
        /// </summary>
        /// Author: Duynv
        /// CreatedDate: 16/4/2014
        public static int GetNumber(string input)
        {
            try
            {
                return System.Convert.ToInt32(Regex.Replace(input, "\\D", string.Empty));
            }
            catch
            {
                return 0;
            }
        }

        //
        public enum FormName
        {

            Insert=1,
            Edit=2,
            Detail=3,
            EditDetail = 4,
            AddModule=5,
            IsoStandard=6,
            IsoMeeting=7
        }
        //
        public enum SystemName
        {
            Web = 1,
            DN = 2,
            HCC = 3,
            TV = 4,
            HT = 5
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// Author: Duynv
        /// CreatedDate: 16/4/2014
        public static List<int> String2ListInt(string input)
        {

            List<int> lst = new List<int>();
            if (!string.IsNullOrEmpty(input))
            {
                input = input.Trim(',');
                try { var ArrTmp = input.Split(','); foreach (var item in ArrTmp) { lst.Add(GetNumber(item)); } }
                catch (Exception) { }
            }
            return lst;
        }
    }

    public class ConstantPath
    {
        public static string UploadVideo = "/Videos/";
        public static string UploadImageNews = "/images/news/";
        public static string UploadImageBanner = "/images/banner/";     
        public static string UploadImageCustomerLogo = "/Content/themes/Customer/logo/";
        public static string UploadImageRepresent = "/images/customers/represents/";
        public static string UploadImageServiceAvatar = "/images/service/";
    }

    /// <summary>
    /// Khai bao object luu thong tin ma mau
    /// </summary>
    /// Created: HuongLL
    public class ColorObject
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class Control
    {
        /// <summary>
        /// FixPreventativeActionSV
        /// </summary>
        /// Author: Duynv
        /// CreatedDate: 16/4/2014
        public void AddForm()
        {
        }
        public void Dopdowlist()
        {
        }

    }
    /// Author: Duynv
    /// CreatedDate: 16/4/2014
    public class GetData
    {
        private static string constBold = "<span style=\"font-weight: bold; text-align: center\">DATA</span>";
        private static string constNomal = "<span style=\"font-weight: normal; text-align: center; width:100%;display: inline-block\">DATA</span>";
        private static string constNomalNoBlock = "<span style=\"font-weight: normal; text-align: center; width:100%\">DATA</span>";
        private static string constNomalColor = "<span style=\"font-weight: normal; text-align: center; width:100%;display: inline-block;color:[COLOR]\">DATA</span>";

        public struct Sex
        {
            public static string MaleVi = "Nam";
            public static string FeMaleVi = "Nữ";
            public static string MaleEn = "Male";
            public static string FeMaleEn = "Female";
            public static int MaleInt = 1;
            public static int FeMaleInt = 0;
        };

        /// <summary>
        /// Trả về trạng thái dạng chuỗi
        /// </summary>
        /// <param name="status"></param>
        /// <returns>return string data</returns>
        public static string Status(int status, bool ishtml = true)
        {
            if (ishtml)
            {
                string rs = "<span style=\"color:red\">Tạo mới</span>";
                if (status == 2)
                    rs = "<span style=\"color:red\">Chờ duyệt</span>";
                if (status == 3)
                    rs = "<span style=\"color:blue\">Đã duyệt</span>";
                if (status == 5)
                    rs = "<span style=\"color:blue\">Đã duyệt chương trình</span>";
                if (status == 6)
                    rs = "<span style=\"color:blue\">Đã duyệt biên bản</span>";
                if (status == 7)
                    rs = "<span style=\"color:blue\">Đã hoàn thành</span>";
                if (status == 8) // dùng cho nhân sự
                    rs = "<span style=\"color:red\">Chưa tạo kế hoạch</span>";
                if (status == 9) // dùng cho nhân sự
                    rs = "<span style=\"color:blue\">Đã duyệt kế hoạch</span>";

                if (status == 80) // dùng cho nhân sự
                    rs = "<span style=\"color:red\">Chờ duyệt kế hoạch</span>";


                if (status == 300)
                    rs = "<span style=\"color:red\">Không duyệt</span>";
                if (status == 500)
                    rs = "<span style=\"color:red\">Không duyệt chương trình</span>";
                if (status == 600)
                    rs = "<span style=\"color:red\">Không duyệt biên bản</span>";
                if (status == 700)
                    rs = "<span style=\"color:red\">Không hoàn thành</span>";
                if (status == 800)
                    rs = "<span style=\"color:red\">Không duyệt kế hoạch</span>";

                return rs;
            }
            else
            {
                string rs = "Tạo mới";
                if (status == 2)
                    rs = "Chờ duyệt";
                if (status == 3)
                    rs = "Đã duyệt";
                if (status == 5)
                    rs = "Đã duyệt chương trình";
                if (status == 6)
                    rs = "Đã duyệt biên bản";
                if (status == 7)
                    rs = "Đã hoàn thành";
                if (status == 8)
                    rs = "Chưa tạo kế hoạch";
                if (status == 9)
                    rs = "Đã duyệt kế hoạch";

                if (status == 300)
                    rs = "Không duyệt";
                if (status == 500)
                    rs = "Không duyệt chương trình";
                if (status == 600)
                    rs = "Không duyệt biên bản";
                if (status == 700)
                    rs = "Không hoàn thành";
                if (status == 800)
                    rs = "Không duyệt kế hoạch";
                return rs;
            }
        }

        public static string check(Nullable<bool> check)
        {
            string rs = "";
            if (check == false)
                rs = "<span style=\"color:red\">Không đạt</span>";
            if (check == true)
                rs = "<span style=\"color:blue\">Đạt</span>";
            return rs;
        }
        public static string StatusDispatch(int status)
        {
            string rs = "";
            if (status == 12)
                rs = "<span style=\"color:Green\">Chưa thực hiện</span>";
            if (status == 13)
                rs = "<span style=\"color:#E7D56F\">Đang thực hiện</span>";
            if (status == 14)
                rs = "<span style=\"color:blue\">Đã hoàn thành</span>";
            if (status == 15)
                rs = "<span style=\"color:red\">Quá hạn</span>";
            return rs;
        }
        public static string StatusConformity(int status)
        {
            string rs = "<span style=\"color:Green\">Chưa thực hiện</span>";
            if (status == 13)
                rs = "<span style=\"color:#E7D56F\">Đang thực hiện</span>";
            if (status == 14)
                rs = "<span style=\"color:blue\">Đã hoàn thành</span>";
            if (status == 15)
                rs = "<span style=\"color:red\">Quá hạn</span>";
            return rs;
        }
        public static string StatusType(int? status)
        {
            string rs = string.Empty;
            if (status == 1)
                rs = "Độ khẩn";
            if (status == 2)
                rs = "Độ mật";
            if (status == 3)
                rs = "Phương thức gửi";
            if (status == 4)
                rs = "Trạng thái";
            return rs;
        }

        public static string Status(bool status)
        {
            if (status)
                return constNomalColor.Replace("DATA", "Có mặt").Replace("[COLOR]", "#00CC00");
            else
                return constNomalColor.Replace("DATA", "Vắng mặt").Replace("[COLOR]", "#EE0000");
        }
        public static string StatusIsUse(bool? status)
        {
            if (status.Value)
                return constNomalColor.Replace("DATA", "Hiển thị").Replace("[COLOR]", "#00CC00");
            else
                return constNomalColor.Replace("DATA", "Không hiển thị").Replace("[COLOR]", "#EE0000");
        }
        public static string StatusUse(bool? isuse)
        {
            if (isuse.Value)
                return constNomalColor.Replace("DATA", "Sử dụng").Replace("[COLOR]", "#00CC00");
            else
                return constNomalColor.Replace("DATA", "Hủy").Replace("[COLOR]", "#EE0000");
        }
        public static string StatusActive(bool? status)
        {
            if(status != null)
            {
                if (status.Value)
                    return constNomalColor.Replace("DATA", "Hiển thị").Replace("[COLOR]", "#00CC00");
                else
                    return constNomalColor.Replace("DATA", "Không hiển thị").Replace("[COLOR]", "#EE0000");

            }
            return "";

            
        }        
        public static string StatusNewsIsUse(bool? status)
        {
            if (status.Value)
                return constNomalColor.Replace("DATA", "Đã duyệt").Replace("[COLOR]", "#00CC00");
            else
                return constNomalColor.Replace("DATA", "Chưa duyệt").Replace("[COLOR]", "#EE0000");
        }
        /// <summary>
        /// Trả về trạng thái dạng chuỗi
        /// </summary>
        /// <param name="status"></param>
        /// <returns>return string data</returns>
        public static string StatusImplementation(int status, DateTime? LastRequiteDate)
        {

            string rs = "<span style=\"color:red\">Chưa thực hiện</span>";
            if (status == 2)
                rs = "<span style=\"color:red\">Đang thực hiện</span>";
            if (status == 3)
                rs = "<span style=\"color:blue\">Chờ duyệt</span>";
            if (status == 4)
                rs = "<span style=\"color:blue\">Đạt</span>";
            if (status == 5)
                rs = "<span style=\"color:red\">Không đạt</span>";
            if (LastRequiteDate.HasValue) if (status <= 2 && System.Convert.ToDateTime(LastRequiteDate) >= DateTime.Now)
                    rs = "<span style=\"color:red\">Chờ duyệt</span>";
            return rs;
        }
        /// <summary>
        /// Trả về trạng thái dạng int
        /// </summary>
        /// <param name="status"></param>
        /// <returns>return int data</returns>
        public static int Status(string status)
        {
            status = status.ToLower();
            if (status == "<span>chờ duyệt")
                return 2;
            if (status == "đã duyệt")
                return 3;
            return 1;
        }
       
        /// <summary>
        /// Có sử dụng thẻ html hay không, mặc định có thẻ html kèm theo
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ishtml"></param>
        /// <returns></returns>
        public static string GetDate(DateTime dt, bool ishtml = true)
        {
            if (ishtml)
                return constNomal.Replace("DATA", dt.ToString("dd/MM/yyyy"));
            else
                return dt.ToString("dd/MM/yyyy");
        }
        //public static string GetDate(DateTime? dt)
        //{
        //    if (dt.HasValue)
        //        return constNomal.Replace("DATA", ((DateTime)dt).ToString("dd/MM/yyyy"));
        //    return "Chưa có dữ liệu";
        //}
        public static string GetDate(DateTime? dt, bool ishtml = false)
        {
            if (!ishtml)
                return ((DateTime)dt).ToString("dd/MM/yyyy");
            if (dt.HasValue)
                return constNomal.Replace("DATA", ((DateTime)dt).ToString("dd/MM/yyyy"));
            return "Chưa có dữ liệu";
        }
        public static string GetDateNoBlock(DateTime dt)
        {
            return constNomalNoBlock.Replace("DATA", dt.ToString("dd/MM/yyyy"));
        }
        /// <summary>
        /// Trả về trạng thái dạng int
        /// </summary>
        /// <param name="ns"></param>
        /// <returns>return int data</returns>
        public static string ColorNC(int ns)
        {
            string rs = "<span style=\"width:88px;height:16px;display:inline-block;background-color:[COLOR]\">&nbsp[TEXT]</span>";
            if (ns == 21)
                return rs.Replace("[COLOR]", "red").Replace("[TEXT]", "Điểm nặng (M)");
            return rs.Replace("[COLOR]", "yellow").Replace("[TEXT]", "Điểm nhẹ (m)");
            //return rs;
        }
        public static string Color(string data, string codeColor)
        {
            string rs = "<span style=\"display:inline-block;color:[COLOR]\">&nbsp" + data + "</span>";
            rs = rs.Replace("[COLOR]", codeColor);
            return rs;
        }

        /// <summary>
        /// Trả về trạng thái dạng int
        /// </summary>
        /// <param name="ns"></param>
        /// <returns>return int data</returns>
        public static string ColorRender(Nullable<int> ns)
        {
            string rs = "<span style=\"width:84px;height:16px;display:inline-block;background-color:[COLOR]\"><center>[TEXT]</center></span>";
            if (ns == 1)
                rs = rs.Replace("[COLOR]", "red").Replace("[TEXT]", "Cao");
            else if (ns == 2)
                rs = rs.Replace("[COLOR]", "yellow").Replace("[TEXT]", "Trung bình"); ;
            rs = rs.Replace("[COLOR]", "green").Replace("[TEXT]", "Thấp"); ;
            return rs;
        }
        /// <summary>
        /// Trả về trạng thái dạng int
        /// </summary>
        /// <param name="ns"></param>
        /// <returns>return int data</returns>
        public static string ColorRenderNC(Nullable<int> nc)
        {
            string rs = "<span style=\"width:84px;height:16px;display:inline-block;background-color:[COLOR]\"><center>[TEXT]</center></span>";
            if (nc == 21)
                rs = rs.Replace("[COLOR]", "#ff0000").Replace("[TEXT]", "Điểm nặng (M)");
            if (nc == 23)
                rs = rs.Replace("[COLOR]", "#ffcc00").Replace("[TEXT]", "Điểm nhẹ (m)");
            if (nc == 54)
                rs = rs.Replace("[COLOR]", "#99cc00").Replace("[TEXT]", "Điểm lưu ý (OBS)");
            return rs;
        }
        /// <summary>
        /// Hàm sinh ID tự tăng dạng chuỗi
        /// </summary>
        /// <param name="lastID"></param>
        /// <param name="prefixID"></param>
        /// <returns></returns>
        /// Create by CuongPC
        public static string NextID(string lastID, string prefixID)
        {
            if (lastID == "")
            {
                return prefixID + "0001";  // fixwidth default
            }
            int nextID = int.Parse(lastID.Remove(0, prefixID.Length)) + 1;
            int lengthNumerID = lastID.Length - prefixID.Length;
            string zeroNumber = "";
            for (int i = 1; i <= lengthNumerID; i++)
            {
                if (nextID < Math.Pow(10, i))
                {
                    for (int j = 1; j <= lengthNumerID - i; i++)
                    {
                        zeroNumber += "0";
                    }
                    return prefixID + zeroNumber + nextID.ToString();
                }
            }
            return prefixID + nextID;

        }

        /// <summary>
        /// Tra ra mang cac ma mau sac
        /// </summary>
        /// <returns> List<ColorObject> </returns>
        /// Created: HuongLL
        public static System.Collections.Generic.List<ColorObject> RenderColor()
        {
            System.Collections.Generic.List<ColorObject> ColorList = new System.Collections.Generic.List<ColorObject> { 
                new ColorObject { Id = "#000000", Name = "#000000" },
                new ColorObject { Id = "#993300", Name = "#993300" },
                new ColorObject { Id = "#333300", Name = "#333300" },
                new ColorObject { Id = "#003300", Name = "#003300" },
                new ColorObject { Id = "#003366", Name = "#003366" },
                new ColorObject { Id = "#000080", Name = "#000080" },
                new ColorObject { Id = "#333399", Name = "#333399" },
                new ColorObject { Id = "#333333", Name = "#333333" },
                new ColorObject { Id = "#800000", Name = "#800000" },
                new ColorObject { Id = "#ff6600", Name = "#ff6600" },
                new ColorObject { Id = "#808000", Name = "#808000" },
                new ColorObject { Id = "#008000", Name = "#008000" },
                new ColorObject { Id = "#008080", Name = "#008080" },
                new ColorObject { Id = "#0000ff", Name = "#0000ff" },
                new ColorObject { Id = "#666699", Name = "#666699" },
                new ColorObject { Id = "#808080", Name = "#808080" },
                new ColorObject { Id = "#ff0000", Name = "#ff0000" },
                new ColorObject { Id = "#ff9900", Name = "#ff9900" },
                new ColorObject { Id = "#99cc00", Name = "#99cc00" },
                new ColorObject { Id = "#339966", Name = "#339966" },
                new ColorObject { Id = "#33cccc", Name = "#33cccc" },
                new ColorObject { Id = "#3366ff", Name = "#3366ff" },
                new ColorObject { Id = "#800080", Name = "#800080" },
                new ColorObject { Id = "#ff00ff", Name = "#ff00ff" },
                new ColorObject { Id = "#ffcc00", Name = "#ffcc00" },
                new ColorObject { Id = "#ffff00", Name = "#ffff00" },
                new ColorObject { Id = "#00ff00", Name = "#00ff00" },
                new ColorObject { Id = "#00ffff", Name = "#00ffff" },
                new ColorObject { Id = "#00ccff", Name = "#00ccff" },
                new ColorObject { Id = "#993366", Name = "#993366" },
                new ColorObject { Id = "#c0c0c0", Name = "#c0c0c0" },
                new ColorObject { Id = "#ff99cc", Name = "#ff99cc" },
                new ColorObject { Id = "#ffcc99", Name = "#ffcc99" },
                new ColorObject { Id = "#ffff99", Name = "#ffff99" },
                new ColorObject { Id = "#ccffcc", Name = "#ccffcc" },
                new ColorObject { Id = "#ccffff", Name = "#ccffff" },
                new ColorObject { Id = "#99ccff", Name = "#99ccff" },
                new ColorObject { Id = "#cc99ff", Name = "#cc99ff" },
                new ColorObject { Id = "#ffffff", Name = "#ffffff" }
            };
            return ColorList;
        }
    }

    //Create: CuongPC
    public class ConvertData
    {
        //Sinh chuoi ngau nhien
        public static string RandomStringByDefault(int length)
        {
            string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random _random = new Random(Environment.TickCount);
            StringBuilder builder = new StringBuilder(length);

            for (int i = 0; i < length; ++i)
                builder.Append(chars[_random.Next(chars.Length)]);

            return builder.ToString();
        }

        //Ma hoa mat khau
        public static string Encrypt(string toEncrypt)
        {
            try
            {
                MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
                byte[] hashedBytes;
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(toEncrypt));
                StringBuilder s = new StringBuilder();
                foreach (byte _hashedByte in hashedBytes)
                {
                    s.Append(_hashedByte.ToString("x2"));
                }
                string str = s.ToString().Substring(0, 20);
                return s.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string Encrypt(string toEncrypt, int maxLength)
        {
            try
            {
                MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
                byte[] hashedBytes;
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(toEncrypt));
                StringBuilder s = new StringBuilder();
                foreach (byte _hashedByte in hashedBytes)
                {
                    s.Append(_hashedByte.ToString("x2"));
                }
                if (maxLength > 0 && maxLength <= s.Length)
                {
                    string str = s.ToString().Substring(0, maxLength);
                    return str;
                }

                return s.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        //Cat chuoi theo ki tu phan cach
        public static string[] SplitString(string str, char chStr)
        {
            if (str != null && !str.Equals(""))
            {
                string[] arrayId = str.Split(chStr);
                return arrayId;
            }
            else
                return null;
        }

        //Chuyen du lieu dang mang string thanh mang kieu int
        public static int[] ArrayIntbyString(string str, char chStr)
        {

            str = str.Trim(chStr);
            //TH mang chi co 1 phan tu

            if (!str.Contains(chStr.ToString()))
            {
                int[] myInts = new int[1] {  System.Convert.ToInt16(str) };
                return myInts;
            }

            if (str != null && !str.Equals(""))
            {
                string[] arrayId = SplitString(str, chStr);
                int[] myInts = arrayId != null ? Array.ConvertAll(arrayId, s => int.Parse(s)) : null;
                return myInts;
            }
            return null;

        }
    }
}