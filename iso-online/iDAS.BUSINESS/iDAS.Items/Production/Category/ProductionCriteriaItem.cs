using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class ProductionCriteriaItem : QualityCriteriaItem
    {
        public int ID { get; set; }
        public int QualityCriteriaID { get; set; }
        public string Offten { get; set; }
        public Nullable<int> EquipmentMeasureCategoryID { get; set; }
    }
}
