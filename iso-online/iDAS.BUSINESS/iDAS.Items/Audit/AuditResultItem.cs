using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class AuditResultItem 
    {
        public int ID { get; set; }
        public int AuditID { get; set; }
        public int QualityCAPARequirementID { get; set; }
        public string Name { get; set; }
        public decimal Factory { get; set; }
        public int QualityCriteriaID { get; set; }
        public int MaxPoint { get; set; }
        public bool IsPass { get; set; }
        public int MinPoint { get; set; }
        public int Point { get; set; }
        public int AuditBy { get; set; }
        public System.DateTime? AuditAt { get; set; }
        public string AuditNote { get; set; }
        public bool IsCategory { get; set; }
        public int ParentID { get; set; }
        public bool IsObs { get; set; }
        public bool IsMaximum { get; set; }
        public bool IsMedium { get; set; }
        public bool ResultPass { get; set; }
        public int TotalPoint { get; set; }
        public bool ResultNotPass
        {
            get;
            set;
        }
        public bool IsHasCAPARequirement
        {
            get;
            set;
        }
        public bool IsCorrertive
        {
            get;
            set;
        }

    }
    public class PointItem
    {
        public int ID { get; set; }
        public int Point { get; set; }

    }
}
