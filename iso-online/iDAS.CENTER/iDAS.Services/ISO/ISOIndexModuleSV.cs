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
    public class ISOIndexModuleSV
    {
        ISOIndexModuleDA ISOIndexModuleDA = new ISOIndexModuleDA();
        public List<ISOIndexModuleItem> GetByClause(int page, int pageSize, out int totalCount, int IsoIndexId, string systemCode)
        {
            var dbo = ISOIndexModuleDA.Repository;
            var IsoIndexItem = dbo.ISOIndexes.Where(p => p.ID == IsoIndexId).FirstOrDefault();
            var result = dbo.ISOStandards.Where(i => i.ID == IsoIndexItem.ISOStandardID)
                                .SelectMany(i => i.ISOStandardModules)
                                .Where(p => p.CenterModule.SystemCode.Trim().ToUpper().Equals(systemCode.ToUpper()))
                                .Select(item => new ISOIndexModuleItem()
                                {
                                    ModuleName = item.CenterModule.Name,
                                    CenterModuleID = item.CenterModule.ID,
                                    IsSelect = dbo.ISOIndexModules.FirstOrDefault(i => i.CenterModuleID == item.CenterModuleID && i.ISOIndexID == IsoIndexId) != null ? true : false,
                                    SystemName = dbo.CenterSystems.FirstOrDefault(i => i.SystemCode == item.CenterModule.SystemCode).Name
                                })
                                .OrderByDescending(item => item.ModuleName)
                                .Page(page, pageSize, out totalCount)
                                .ToList();
            return result;
        }
        public void Insert(ISOIndexModuleItem item)
        {
            var iso = new ISOIndexModule()
            {
                CenterModuleID = item.CenterModuleID,
                ISOIndexID = item.ISOIndexID,
                IsUse = true,
                IsDelete = false,
                CreateAt = DateTime.Now,
            };
            ISOIndexModuleDA.Insert(iso);
            ISOIndexModuleDA.Save();
        }
        public void Delete(int CenterModuleID, int ISOIndexID)
        {
            var iso = ISOIndexModuleDA.GetQuery(i=>i.CenterModuleID== CenterModuleID && i.ISOIndexID == ISOIndexID).FirstOrDefault();
            ISOIndexModuleDA.Delete(iso.ID);
            ISOIndexModuleDA.Save();
        }
    }

}
