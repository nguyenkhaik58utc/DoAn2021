using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class AuditCriteriaViewItem
    {
        private string _Category="";
        public int ID { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public string Category
        {
            get
            {
                if (Categories != null)
                {
                    foreach (var item in Categories)
                    {
                        _Category = item + ">>" + _Category;
                    }
                }
                if (!string.IsNullOrEmpty(_Category)) _Category.TrimEnd('>');
                return _Category;
            }
            set {
                _Category = value;
            }
        }
    }
}
