using iDAS.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Items;

namespace iDAS.Services
{
   public class CountrySV
    {
       CountryDA countryDA = new CountryDA();
       public List<CountryItem> GetAllCountry ()
       {
           var city = countryDA.GetQuery()
                   .Select(i => new CountryItem()
                   {
                       ID = i.ID,
                       Name = i.Name,                      
                   })
                   .OrderByDescending(item => item.Name)
                   .ToList();
           return city;
       }
    }
}
