using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.DAL.MnCustomer;
using iDAS.Items.MnCustomer;

namespace iDAS.Services.MnCustomer
{
    public class InitService
    {
        public List<CountryItem> GetCountries()
        {
            CountryDA countryDA = new CountryDA();
            var countries = countryDA.GetQuery()
                        .Select(item => new CountryItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Code = item.Code,
                        })
                        .ToList();
            return countries;
        }

        public List<CityItem> GetCities()
        {
            CityDA cityDA = new CityDA();
            var cities = cityDA.GetQuery()
                        .Select(item => new CityItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Code = item.Code,
                            CountryCode = item.CountryCode,
                        })
                        .ToList();
            return cities;
        }

        public List<CityItem> GetCities(int? countryID)
        {
            CityDA cityDA = new CityDA();
            var country = cityDA.SystemContext.InitCountries.FirstOrDefault(item => item.ID == countryID);
            var countryCode = country == null ? string.Empty : country.Code;
            var cities = cityDA.GetQuery()
                        .Where(item =>item.CountryCode == countryCode)
                        .Select(item => new CityItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Code = item.Code,
                            CountryCode = item.CountryCode,
                        })
                        .ToList();
            return cities;
        }

        public List<DistrictItem> GetDistricts()
        {
            DistrictDA districtDA = new DistrictDA();
            var districts = districtDA.GetQuery()
                        .Select(item => new DistrictItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Code = item.Code,
                            CityCode = item.CityCode,
                            CountryCode = item.CountryCode,
                        })
                        .ToList();
            return districts;
        }

        public List<DistrictItem> GetDistricts(int? cityID)
        {
            DistrictDA districtDA = new DistrictDA();
            var city = districtDA.SystemContext.InitCities.FirstOrDefault(item => item.ID == cityID);
            var countryCode = city == null ? string.Empty : city.CountryCode;
            var cityCode = city == null ? string.Empty : city.Code;
            var districts = districtDA.GetQuery()
                        .Where(item => item.CityCode == cityCode && item.CountryCode == countryCode)
                        .Select(item => new DistrictItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Code = item.Code,
                            CityCode = item.CityCode,
                            CountryCode = item.CountryCode,
                        })
                        .ToList();
            return districts;
        }

        public List<CurrencyItem> GetCurrencies()
        {
            CurrencyDA currencyDA = new CurrencyDA();
            var currencies = currencyDA.GetQuery()
                        .Select(item => new CurrencyItem()
                        {
                            ID = item.ID,
                            Code = item.Code,
                            Symbol = item.Symbol,
                        })
                        .ToList();
            return currencies;
        }
    }
}
