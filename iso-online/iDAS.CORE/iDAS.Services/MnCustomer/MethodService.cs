using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;
using iDAS.DAL.MnCustomer;
using iDAS.Items.MnCustomer;

namespace iDAS.Services.MnCustomer
{
    public class MethodService
    {
        MethodDA methodDA = new MethodDA();

        public List<MethodItem> GetAll(string code)
        {
            var methods = methodDA.GetQuery()
                        .Where(item => item.Code == code)
                        .Select(item => new MethodItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Code = item.Code,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .ToList();
            return methods;
        }

        public List<MethodItem> GetAll(string code, int page, int pageSize, out int totalCount)
        {
            var methods = methodDA.GetQuery()
                        .Where(item => item.Code == code)
                        .Select(item => new MethodItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Code = item.Code,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            CreateByView = methodDA.SystemContext.SystemUsers.Where(u => u.ID == item.CreateBy).Select(u => u.FullName).FirstOrDefault() ?? string.Empty,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                            UpdateByView = methodDA.SystemContext.SystemUsers.Where(u => u.ID == item.UpdateBy).Select(u => u.FullName).FirstOrDefault() ?? string.Empty,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return methods;
        }


        public void Insert(MethodItem item)
        {
            var method = new MnCustomerMethod()
            {
                Name = item.Name,
                Code = item.Code,
                CreateAt = DateTime.UtcNow,
                CreateBy = item.CreateBy,
            };
            methodDA.Insert(method);
            methodDA.Save();
        }

        public void Update(MethodItem item)
        {
            var method = methodDA.GetById(item.ID);
            method.Name = item.Name;
            method.UpdateAt = DateTime.Now;
            method.UpdateBy = item.UpdateBy;
            methodDA.Save();
        }

        public void Delete(int id)
        {
            methodDA.Delete(id);
            methodDA.Save();
        }

        public void DeleteRange(List<object> ids)
        {
            methodDA.DeleteRange(ids);
            methodDA.Save();
        }
    }
}
