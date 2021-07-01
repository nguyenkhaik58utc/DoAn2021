using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;

namespace iDAS.Services
{
    public class WebCustomerCitySV : BaseService
    {
        WebCustomerCityDA webCustomerCityDA = new WebCustomerCityDA();

        public List<WebCustomerCityItem> GetWebCustomerCities(int page, int pageSize, out int total)
        {
            var lst = webCustomerCityDA.GetQuery()
                        .Where(i => !i.IsDelete)
                        .Select(i => new WebCustomerCityItem()
                        {
                            ID = i.ID,
                            Name = i.Name,
                            Code = i.Code,
                            CountryCode = i.CountryCode,
                            IsActive = i.IsActive,
                            CreateAt = i.CreateAt,
                            UpdateAt = i.UpdateAt,
                        })
                        .OrderByDescending(i => i.CreateAt)
                        .Page(page, pageSize, out total)
                        .ToList();
            return lst;
        }
        public WebCustomerCityItem GetWebCustomerCity(int id)
        {
            var item = webCustomerCityDA.GetQuery()
                        .Where(i => i.ID == id)
                        .Where(i => !i.IsDelete)
                        .Select(i => new WebCustomerCityItem()
                        {
                            ID = i.ID,
                            Name = i.Name,
                            Code = i.Code,
                            CountryCode = i.CountryCode,
                            IsActive = i.IsActive,
                            CreateAt = i.CreateAt,
                            CreateBy = i.CreateBy,
                            UpdateAt = i.UpdateAt,
                            UpdateBy = i.UpdateBy,
                        })
                        .FirstOrDefault();
            return item;
        }
        public void Insert(WebCustomerCityItem item)
        {
            var record = new WebCustomerCity()
            {
                Name = item.Name.Trim(),
                Code = item.Code,
                CountryCode = item.CountryCode,
                IsActive = item.IsActive,
                IsDelete = false,
                CreateAt = DateTime.Now,
                CreateBy = User.ID,
                UpdateAt = DateTime.Now,
                UpdateBy = User.ID,
            };
            webCustomerCityDA.Insert(record);
            webCustomerCityDA.Save();
        }
        public void Update(WebCustomerCityItem item)
        {
            var record = webCustomerCityDA.GetById(item.ID);
            record.Name = item.Name;
            record.Code = item.Code;
            record.CountryCode = item.CountryCode;
            record.IsActive = item.IsActive;
            record.UpdateAt = DateTime.Now;
            record.UpdateBy = User.ID;
            webCustomerCityDA.Save();
        }
        public void Delete(int id)
        {
            var item = webCustomerCityDA.GetById(id);
            item.IsDelete = true;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = User.ID;
            webCustomerCityDA.Save();
        }
        public bool CheckExist(WebCustomerCityItem item)
        {
            var check = webCustomerCityDA.GetQuery()
                        .Where(i => i.ID != item.ID || item.ID == 0)
                        .Where(i => i.Name.ToUpper().Equals(item.Name.ToUpper()))
                        .Where(i => i.CountryCode.ToUpper().Equals(item.CountryCode.ToUpper()))
                        .Where(i => !i.IsDelete)
                        .Count() > 0;
            return check;
        }
    }
}
