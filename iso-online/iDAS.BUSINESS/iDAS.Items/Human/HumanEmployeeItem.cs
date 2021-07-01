using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;

namespace iDAS.Items
{
    public class HumanEmployeeItem
    {
        public int ID { get; set; }
        public int? UserID { get; set; }
        public Guid? FileID { get; set; }
        public FileItem ImageFile { get; set; }
        //public byte[] Image { get; set; }
        public string ImageUrl
        {
            get
            {
                var url = "data:image;base64,{0}";
                if (ImageFile == null || ImageFile.Data == null || Convert.ToBase64String(ImageFile.Data) == "") return "";
                var data = Convert.ToBase64String(ImageFile.Data);
                url = string.Format(url, data);
                return url;
            }
        }
        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "Yêu cầu nhập!")]
        public string Name { get; set; }
        [Display(Name = "Email")]
        [RegularExpression(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-‌​]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$", ErrorMessage = "Không đúng định dạng email như support@iDASvietnam.com")]
        public string Email { get; set; }
        [Display(Name = "Sô điện thoại")]
        public string Phone { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        public System.DateTime? CreateAt { get; set; }
        public int? CreateBy { get; set; }
        public int? UpdateBy { get; set; }
        public System.DateTime? UpdateAt { get; set; }
        //[Display(Name = "Ảnh đại diện")]
        //public string Avatar { get; set; }
        [Display(Name = "Mã nhân viên")]
        public string Code { get; set; }
        public string CreateName { get; set; }
        public bool Gender { get; set; }
        public System.DateTime? Birthday { get; set; }
        public string sex { get; set; }
        public string WeddingStatus { get; set; }
        public string Role { get; set; }
        public int? RoleID { get; set; }
        public int? DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public bool? IsPerform { get; set; }
        public bool? IsCheck { get; set; }
        public bool? IsAudit { get; set; }
        public bool? IsApproval { get; set; }
        public bool? IsChange { get; set; }
        public bool? IsClosed { get; set; }
        public bool IsNew { get; set; }
        public bool IsTrial { get; set; }
        public bool IsLeave { get; set; }

        #region Profile Employee
        //[HiddenInput(DisplayValue = false)]
        //public int ProfileEmployeeID { get; set; }
        public string Aliases { get; set; }
        public string Nationality { get; set; }
        public string People { get; set; }
        public string Religion { get; set; }
        public string PlaceOfBirth { get; set; }
        public string OfficePhone { get; set; }
        public string HomePhone { get; set; }
        public string NumberOfIdentityCard { get; set; }
        public Nullable<System.DateTime> DateIssueOfIdentityCard { get; set; }
        public string PlaceIssueOfIdentityCard { get; set; }
        public Nullable<System.DateTime> DateOnGroup { get; set; }
        public string PositionGroup { get; set; }
        public string PlaceOfLoadedGroup { get; set; }
        public Nullable<System.DateTime> DateJoinRevolution { get; set; }
        public Nullable<System.DateTime> DateAtParty { get; set; }
        public Nullable<System.DateTime> DateOfJoinParty { get; set; }
        public string PlaceOfLoadedParty { get; set; }
        public string PosititonParty { get; set; }
        public string NumberOfPartyCard { get; set; }
        public Nullable<System.DateTime> DateOnArmy { get; set; }
        public string PositionArmy { get; set; }
        public string ArmyRank { get; set; }
        public string Likes { get; set; }
        public string Forte { get; set; }
        public string Defect { get; set; }
        public string TaxCode { get; set; }
        public string NumberOfBankAccounts { get; set; }
        public string Bank { get; set; }
        public string NumberOfPassport { get; set; }
        public string PlaceOfPassport { get; set; }
        public Nullable<System.DateTime> DateOfIssuePassport { get; set; }
        public Nullable<System.DateTime> PassportExpirationDate { get; set; }
        #endregion

        public string FileName { get; set; }
        public string ChucDanh { get; set; }
        public List<HumanGroupPositionItem> lstHumanGrPos { get; set; }
    }
}
