using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iDAS.Items
{
    public class CustomerCategoryDetailItem
    {
        private string _Name = "";
        public int ID { get; set; }
        public List<string> Names { get; set; }
        public string Name
        {
            get
            {
                if (Names != null)
                {
                    foreach (var item in Names)
                    {
                        _Name = item + " >>" + _Name;
                    }
                }
                if (!string.IsNullOrEmpty(_Name))
                    _Name = _Name.TrimEnd('>');
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }
    }
}