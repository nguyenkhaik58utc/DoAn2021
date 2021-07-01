using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISO.API.Models
{
    public class HumanDepartmentDTO
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NameE { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Tax { get; set; }
        public string Website { get; set; }
        public DateTime EstablishedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public bool IsCancel { get; set; }
        public bool IsMerge { get; set; }
        public bool IsDestroy { get; set; }
        public int Position { get; set; }
        public string History { get; set; }
        public string Resposibility { get; set; }
        public string Authorize { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateAt { get; set; }
        public int UpdateBy { get; set; }
        public DateTime UpdateAt { get; set; }

    }
    public class HumanDepartmentReqModel
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
