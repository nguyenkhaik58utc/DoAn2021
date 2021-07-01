using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ext.Net.MVC;
using Ext.Net;


namespace iDAS.Utilities
{
    public class Common
    {
        public const string Task = "/Task/TaskPerson/TaskByPersonal";//Ok
        public const string Target = "/Quality/Target";//Ok
        public const string SuppliersOrders = "/Suppliers/SuppliersOrders";//Ok
        public const string CommandService = "/Service/Command";//Ok
        public const string PlanProductionDevlopment = "/DevelopmentProduct/Plan";//Ok
        public const string Document = "/Document/Document";
        public const string DocumentOut = "/Document/DocumentOut";
        public const string SuppliersPlanRate = "/Suppliers/SuppliersPlanRate";
        public const string DocumentRequest = "/Document/Request";//Ok
        public const string AuditProgram = "/Quality/AuditProgram";//Ok
        public const string AuditPlan = "/Quality/AuditPlan";
        public const string PlanService = "/Service/Plan";
        public const string DispatchGoEmployee = "/Dispatch/DispatchGoEmployee";
        public const string DispatchGoDepartment = "/Dispatch/DispatchGoDepartment";
        public const string DispatchToDepartment = "/Dispatch/DispatchToDepartment";
        public const string DispatchToEmployee = "/Dispatch/DispatchToEmployee";
        public const string DispatchToInSide = "/Dispatch/DispatchToInSide";
        public const string PhaseAudit = "/Human/PhaseAudit";
        public const string TrainingRequirement = "/Human/TrainingRequirement";
        public const string TrainingPlan = "/Human/TrainingPlan";
        public const string RecruitmentRequirement = "/Human/RecruitmentRequirement";
        public const string RecruitmentProfileInterview = "/Human/RecruitmentProfileInterview";
        public const string RecruitmentPlan = "/Human/RecruitmentPlan";
        public const string ProfileWorkTrialPropose = "/Human/ProfileWorkTrialPropose";
        public const string ProfileWorkTrialManager = "/Human/ProfileWorkTrialAuditManager";
        public const string ProfileWorkTrialEmployee = "/Human/ProfileWorkTrialAuditEmployee";
        public const string AnswerAudit = "/Human/AnswerAudit";
        public const string MeetingPlan = "/Quality/MeetingPlan";
        public const string CAPARequire = "/Quality/CAPARequire";
        public const string AuditEmployee = "/Human/AuditEmployee";
        public const string AuditManagement = "/Human/AuditManagement";
        public const string AuditLeader = "/Human/AuditLeader";
        public const string TaskRequest = "/Task/TaskRequest";
        public const string DispatchGoCC = "/Dispatch/DispatchGoCC";
        public const string DispatchToCC = "/Dispatch/DispatchToCC";
        public const string TaskCC = "/Task/TaskCC";
        public enum RefType
        {
            inward_Start=0, //Nhập Kho Đầu Kỳ
            inward=1, //Nhập kho
            outward=2, //Xuất kho
            adjustment=5, // Kiểm kê
            build=7, //Đóng gói - lắp ráp
            build_Dismantle=8,// tháo dỡ
            transfer=9 // Chuyển kho

        }
        public static string GetRefTypeName(int reftype)
        {
            switch (reftype)
            {
                case (int)RefType.inward_Start: return "Nhập Kho Đầu Kỳ";
                case (int)RefType.inward: return "Nhập Kho";
                case (int)RefType.outward: return "Xuất Kho";
                case (int)RefType.adjustment: return "Kiểm Kê";
                case (int)RefType.build: return "Đóng Gói - Lắp Ráp";
                case (int)RefType.build_Dismantle: return "Tháo Gỡ";
                case (int)RefType.transfer: return "Chuyển Kho";

                default: return "";

            }

        }
        public enum Product_Type
        {
            hh = 1, //Hàng hóa
            btp = 2, //Bán thành phẩm
            nl = 3, //Nguyên liệu
            ngl = 4, // Nhiên liệu
            tp = 5, //Thành phẩm
            splr = 6,// Sản phẩm lắp ráp
            spcpt =7


        }
        public enum Style_Cell
        {
            Default = 4,
            Title = 2, 
            Header = 1, 
            Italic = 3
        }
        public enum Type_Export
        {
            Default = 1,
            ToFile =2
        }
        public static string GetProductTypeName(int productType)
        {
            switch (productType)
            {
                case (int)Product_Type.hh: return "Hàng hóa";
                case (int)Product_Type.btp: return "Bán thành phẩm";
                case (int)Product_Type.nl: return "Nguyên liệu";
                case (int)Product_Type.ngl: return "Nhiên liệu";
                case (int)Product_Type.tp: return "Thành phẩm";
                case (int)Product_Type.splr: return "Sản phẩm lắp ráp";
                case (int)Product_Type.spcpt: return "Sản phẩm chưa phát triển";
                default: return "";

            }

        }
        public static string GetFormName(int form)
        {
            if (form == 0)
                return " Đạt: ";
            else if (form == 1)
                return " Tăng: ";
            else
                return " Giảm: ";
        }
        public static string GetTypeName(bool type)
        {
            if (type)
                return " Tổ chức ";
            else 
                return " Cá nhân: ";            
        }
        //Cat chuoi theo ki tu phan cach
        /// <summary>
        /// CuongPC
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string SubString(string str, int length)
        {
            var strSub = "";
            if (str != null && str.Length > length)
            {
                strSub = str.Substring(0, str.LastIndexOf(" ", length)) + "...";
                return strSub;
            }
            else
            {
                return str;
            }
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
        public static string RandomStringByDefault(int length)
        {
            string chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Random _random = new Random(Environment.TickCount);
            StringBuilder builder = new StringBuilder(length);

            for (int i = 0; i < length; ++i)
                builder.Append(chars[_random.Next(chars.Length)]);

            return builder.ToString();
        }
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
        public static string UploadPathStock = "~/Areas/Stock/Upload/";
        public static string DocumentUpload = "~/Upload/Document/";
        public static string DispatchUpload = "~/Upload/Dispatch/";
        public static string TemporaryUpload = "~/Upload/Temporary/";
        public static string FileImportCustomer = "/Upload/";
        public static string FileTempExport = "/Upload/";
        /// <summary>
        /// Khai bao object luu thong tin ma mau
        /// </summary>
        /// Created: CuongPC
        public class ColorObject
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
        public static string StatusColor(int status, bool isReq=false)
        {
            string rs = "";
            //string rs = "<span style=\"color:Green\">Chưa thực hiện</span>";
            //if (status == 13)
            //    rs = "<span style=\"color:#E7D56F\">Đang thực hiện</span>";
            if (status == (int) DocumentStatus.Issued && isReq)
                rs = "<span style=\"color:blue\">Ban hành <span style=\"color:red\">(!)</span></span>";
            else if (status == (int) DocumentStatus.Issued && !isReq)
                rs = "<span style=\"color:blue\">Ban hành </span>";
            else  if (status == (int)DocumentStatus.Obsolete)
                rs = "<span style=\"color:red\">Lỗi thời</span>";
            return rs;
        }

        public static string ImageFile(string text)
        {
            string imgUrl = "";
            switch (text)
            {
                case "doc":
                case "docx":
                    imgUrl = "<img src= '../../../../images/icons/PageWord.png'>";
                    break;
                case "xls":
                case "xlsx":
                    imgUrl = "<img src= '../../../../images/icons/PageExcel.png'>";
                    break;
                case "ppt":
                case "pptx":
                    imgUrl = "<img src= '../../../../images/icons/PageWhitePowerpoint.png'>";
                    break;
                case "pdf":
                    imgUrl = "<img src= '../../../../images/icons/PageWhiteAcrobat.png'>";
                    break;
                case "txt":
                    imgUrl = "<img src= '../../../../images/icons/PageWhiteText.png'>";
                    break;
                case "xml":
                    imgUrl = "<img src= '../../../../images/icons/PageXML.png'>";
                    break;
                case "jpg":
                case "jpeg":
                case "btm":
                case "gif":
                case "png":
                    imgUrl = "<img src= '../../../../images/icons/Picture.png'>";
                    break;
                default:
                    imgUrl = "<img src= '../../../../images/icons/PageWhiteError.png'>";
                    break;
            }
            return imgUrl;
        }
        public enum DocumentStatus
        {
            New=1,//Tạo mới
            Modified=2,//Sửa đổi
            Approve=3,//Duyệt đạt
            Issued=4,//Ban hanh 
            
            Obsolete=5,//Loi thoi
            PreApprove=6,//Chờ duyệt
            ApproveFail = 7,//Duyệt ko đạt
            
          
        }

        public enum ApproveStatus
        {
            Accept = 1,//Tạo mới
            Reject = 2,//Sửa đổi
           
        }
        public class GetData
        {
           
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

            public static string GetPositionInfo(string Department, string Role)
            {
                string role = "N/A", dept = "N/A";
                if (Role != null && !Role.Trim().Equals(""))
                    role = Role;
                if (Department != null && !Department.Trim().Equals(""))
                    dept = Department;

                return "Phòng ban:" + role + "<br> Chức danh:" + dept;

            }
          
        }
        
        public static string ColorToHexString(Color color)
        {
            return "#" + color.R.ToString("X2") + color.B.ToString("X2") + color.G.ToString("X2");
        }

        public static void ShowExtMsg(MessageBox.Button extMsgBtn, MessageBox.Icon extMsgIcon, string title,  string msg,int width=300)
        {
            X.Msg.Show(new MessageBoxConfig
            {
                Buttons =extMsgBtn ,// MessageBox.Button.OK,
                Icon =  extMsgIcon ,//MessageBox.Icon.INFO,
                Title = title,
                Width = width,
                Message = msg
            });


        }
    }
}
