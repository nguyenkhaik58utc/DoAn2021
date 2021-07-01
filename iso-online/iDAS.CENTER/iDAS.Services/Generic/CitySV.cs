using iDAS.DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using iDAS.Items;

namespace iDAS.Services
{
   public class CitySV
    {
       CityDA cityDA = new CityDA();
       public List<CityItem> GetAllCity()
       {
           var city = cityDA.GetQuery()
                   .Select(i => new CityItem()
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
