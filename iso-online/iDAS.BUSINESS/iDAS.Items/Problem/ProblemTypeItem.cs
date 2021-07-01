using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items.Problem
{
    public class ProblemTypeItem
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string ProblemTypeName { get; set; }
        public string Description { get; set; }
        public bool IsDelete { get; set; }
        public int? CreateBy { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public bool IsDefault { get; set; }
        public string textDefault { get; set; }
        public bool IsSelect { get; set; }

        public ProblemTypeItem()
        {
        }

        public ProblemTypeItem(int iD, string code, string problemTypeName, string description, bool isDelete, int? createBy, DateTime? createAt, int? updateBy, DateTime? updateAt)
        {
            ID = iD;
            Code = code;
            ProblemTypeName = problemTypeName;
            Description = description;
            IsDelete = isDelete;
            CreateBy = createBy;
            CreateAt = createAt;
            UpdateBy = updateBy;
            UpdateAt = updateAt;
        }
    }
}
