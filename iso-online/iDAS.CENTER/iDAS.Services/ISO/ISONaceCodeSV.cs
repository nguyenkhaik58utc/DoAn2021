using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.DA;
using iDAS.Base;
using iDAS.Core;
using iDAS.Items;

namespace iDAS.Services
{
    public class ISONaceCodeSV
    {
        private ISONaceCodeDA ISONaceCodeDA = new ISONaceCodeDA();
        public List<ISONaceCodeItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var riskTempNaceCode = ISONaceCodeDA.GetQuery()
                        .Select(item => new ISONaceCodeItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Code = item.Code,
                            IsActive = item.IsActive,
                            IsDelete = item.IsDelete,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                         .Page(page, pageSize, out totalCount)
                        .ToList();
            return riskTempNaceCode;
        }
        public List<ISONaceCodeItem> GetActive()
        {
            var riskTempNaceCode = ISONaceCodeDA.GetQuery(i=>i.IsActive)
                        .Select(item => new ISONaceCodeItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Code = item.Code,
                            IsActive = item.IsActive,
                            IsDelete = item.IsDelete,
                            CreateAt = item.CreateAt,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .ToList();
            return riskTempNaceCode;
        }
        public ISONaceCodeItem GetById(int Id)
        {
            var riskTempNaceCode = ISONaceCodeDA.GetQuery(i => i.ID == Id)
                        .Select(item => new ISONaceCodeItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Code = item.Code,
                            IsActive = item.IsActive,
                            IsDelete = item.IsDelete,
                        })
                        .First();
            return riskTempNaceCode;
        }
        public void Update(ISONaceCodeItem item, int userID)
        {
            var isoNaceCode = ISONaceCodeDA.GetById(item.ID);
            isoNaceCode.Name = item.Name;
            isoNaceCode.Code = item.Code;
            isoNaceCode.IsActive = item.IsActive;
            isoNaceCode.UpdateAt = DateTime.Now;
            isoNaceCode.UpdateBy = userID;
            ISONaceCodeDA.Save();
        }
        public void Insert(ISONaceCodeItem item, int userID)
        {
            var naceCode = new ISONaceCode()
            {
                Name = item.Name,
                Code = item.Code,
                IsActive = item.IsActive,
                IsDelete = item.IsDelete,
                CreateAt = DateTime.Now,
                CreateBy = userID
            };
            ISONaceCodeDA.Insert(naceCode);
            ISONaceCodeDA.Save();
        }
        public void Delete(int id)
        {
            ISONaceCodeDA.Delete(id);
            ISONaceCodeDA.Save();
        }
    }
}
