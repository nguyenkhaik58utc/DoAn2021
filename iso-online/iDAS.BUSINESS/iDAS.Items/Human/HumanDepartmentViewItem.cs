using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class HumanDepartmentViewItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<int> IDs { get; set; }
        public string strIDs
        {
            get
            {
                if (IDs != null)
                {
                    return iDAS.Utilities.Convert.ToString(IDs);
                }
                else {
                    return ID.ToString();
                }
            }
            set
            {
                IDs = iDAS.Utilities.Convert.ToListInt(value);
                ID = IDs.Count > 0 ? IDs[0] : 0;
            }
        }
    }
}
