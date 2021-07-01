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
    public class ISOIndexFunctionSV
    {
        ISOIndexFunctionDA ISOIndexFunctionDA = new ISOIndexFunctionDA();
        public List<ISOIndexFunctionItem> GetByClause(int page, int pageSize, out int totalCount, int IsoIndexId, string systemCode)
        {
            var dbo = ISOIndexFunctionDA.Repository;
            var IsoIndexItem = dbo.ISOIndexes.Where(p => p.ID == IsoIndexId).FirstOrDefault();
            var moduleIds = dbo.ISOIndexModules.Where(i => i.ISOIndexID == IsoIndexId && i.CenterModule.SystemCode == systemCode)
                                    .Select(i => i.CenterModule.Code).ToList();
            var result = dbo.CenterFunctions.Where(i => !i.IsDelete && i.IsShow && !i.IsGroup)
                                .Where(i => moduleIds.Contains(i.ModuleCode) && i.SystemCode.Equals(systemCode))
                                .Select(item => new ISOIndexFunctionItem()
                                {
                                    FunctionName = item.Name,
                                    CenterFunctionID = item.ID,
                                    IsSelect = dbo.ISOIndexFunctions.Any(i => i.CenterFunctionID == item.ID && i.ISOIndexID == IsoIndexId) ? true : false,
                                    SystemName = dbo.CenterSystems.Where(i => i.SystemCode == item.SystemCode).Select(i => i.Name).FirstOrDefault()
                                })
                                .OrderByDescending(item => item.FunctionName)
                                .Page(page, pageSize, out totalCount)
                                .ToList();
            return result;
        }
        public void Insert(ISOIndexFunctionItem item)
        {
            var iso = new ISOIndexFunction()
            {
                CenterFunctionID = item.CenterFunctionID,
                ISOIndexID = item.ISOIndexID,
                IsUse = true,
                IsDelete = false,
                CreateAt = DateTime.Now,
            };
            ISOIndexFunctionDA.Insert(iso);
            ISOIndexFunctionDA.Save();
        }
        public void Delete(int CenterFunctionID, int ISOIndexID)
        {
            var iso = ISOIndexFunctionDA.GetQuery(i => i.CenterFunctionID == CenterFunctionID && i.ISOIndexID == ISOIndexID).FirstOrDefault();
            ISOIndexFunctionDA.Delete(iso.ID);
            ISOIndexFunctionDA.Save();
        }
    }

}
