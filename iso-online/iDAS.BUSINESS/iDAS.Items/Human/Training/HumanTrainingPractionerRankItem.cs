using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class HumanTrainingPractionerRankItem
    {
        public int ID { get; set; }
        public string RankName { get; set; }
        public string Descripson { get; set; }
        public bool IsUse { get; set; }
        public Nullable<System.DateTime> UpdateAt { get; set; }
        public Nullable<int> CreateBy { get; set; }
        public Nullable<System.DateTime> CreateAt { get; set; }
        public Nullable<int> UpdateBy { get; set; }
    }
}
