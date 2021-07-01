using iDAS.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class ServiceCommandServiceSV
    {
        private ServiceCommandDA commandServiceDA = new ServiceCommandDA();
        private CustomerDA customerDA = new CustomerDA();
        private ServiceDA serviceDA = new ServiceDA();
        private string GetNameById(int id)
        {
            var rs = serviceDA.GetById(id);
            if (rs != null)
                return rs.Name;
            return "ISO đã bị xóa";
        }
        public int[] GetListID()
        {
            try
            {
                var modules = commandServiceDA.GetQuery()
                    .Select(a => a.ID).ToArray();
                return modules;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Insert(ServiceCommand obj, int userId)
        {
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            commandServiceDA.Insert(obj);
            commandServiceDA.Save();
        }
        public void Delete(int id)
        {
            var obj = commandServiceDA.GetById(id);
            commandServiceDA.Delete(obj);
            commandServiceDA.Save();
        }
    }
}
