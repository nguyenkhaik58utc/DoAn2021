using iDAS.DA.Generic;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
    public class CommonCitySV
    {
        private CommonCityDA cityDA = new CommonCityDA();

        public List<ComboboxItem> getCombobox(string code)
        {
            List<ComboboxItem> lst = cityDA.GetQuery(i => i.CountryCode == code).Select(item => new ComboboxItem
            {
                ID = item.ID,
                Name = item.Name
            }).ToList();
            return lst;
        }
    }
}
