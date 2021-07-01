using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class AccountingPaymentPlanItem :QualityPlanItem
    {
        public int QualityPlanID { get; set; }
        public int AccountingPaymentID { get; set; }
        public string AccountingPayment { get; set; }
        public decimal? TotalContract { get; set; }
        public DateTime FinishDate { get; set; }
        public Nullable<decimal> RatePlan { get; set; }
        public Nullable<decimal> RateReal { get; set; }
        public Nullable<decimal> ValuePlan { get; set; }
        public Nullable<decimal> ValueReal { get; set; }
        public Nullable<System.DateTime> TimePlan { get; set; }
        public Nullable<System.DateTime> TimeReal { get; set; }
    }
}
