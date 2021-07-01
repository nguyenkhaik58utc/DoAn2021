using iDAS.DA.Generic;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
    public class CommonCountrySV
    {
        private CommonCountryDA CountryDA = new CommonCountryDA();

        public List<ComboboxItem> getCombobox()
        {
            List<ComboboxItem> lst = CountryDA.GetQuery().Select(item => new ComboboxItem
            {
                ID = item.ID,
                FullName = item.Code,
                Name = item.Name
            }).ToList();
            return lst;
        }

    }
}
