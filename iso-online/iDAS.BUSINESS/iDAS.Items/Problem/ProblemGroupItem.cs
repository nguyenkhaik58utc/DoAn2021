using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items.Problem
{
    public class ProblemGroupItem
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string ProblemGroupName { get; set; }
        public string Description { get; set; }
        public bool IsDelete { get; set; }
        public bool IsSelected { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }
        public int ParentID { get; set; }
        public int ProblemTypeID { get; set; }
        public string ProblemTypeName { get; set; }
        public bool IsParent { get; set; }
        public List<ProblemTypeItem> lstType { get; set; }

        public string UpdateAtView
        {
            get
            {
                if (UpdatedAt.HasValue)
                    return UpdatedAt.Value.ToString("dd-MM-yyyy");
                else
                {
                    return string.Empty;
                }
            }
        }

        public string EstablishedDateView
        {
            get
            {
                if (CreatedAt.HasValue)
                    return CreatedAt.Value.ToString("dd-MM-yyyy");
                else return string.Empty;
            }
        }
    }
}
