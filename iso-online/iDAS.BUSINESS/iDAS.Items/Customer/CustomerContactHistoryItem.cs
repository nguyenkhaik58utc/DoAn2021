using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class CustomerContactHistoryItem
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public HumanEmployeeViewItem Employee { get; set; }
        public System.DateTime Time { get; set; }
        public string Requirment { get; set; }
        public decimal? Cost { get; set; }
        public string Content { get; set; }
        public int FormID { get; set; }
        public string FormName { get; set; }
        public string RequirementField { get; set; }
        public bool IsSuccess { get; set; }
        public string Note { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public bool IsPotential { get; set; }
        public bool IsOfficial { get; set; }
        public String SuccessStatus
        {
            get
            {
                var result = "";
                if (IsSuccess)
                {
                    if (!IsPotential && !IsOfficial)
                        result = "<span style=\"color:blue\">Có nhu cầu</span>";
                    else
                    {
                        if (IsPotential)
                            result = "<span style=\"color:blue\">Có hợp đồng</span>";
                        if (IsOfficial)
                            result = "<span style=\"color:blue\">Có tiềm năng</span>";
                    }
                }
                return result;
            }
        }
        public string ActionForm { get; set; }
        public AttachmentFileItem AttachmentFile { get; set; }
    }
}