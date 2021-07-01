using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.DA;
using iDAS.Base;
using iDAS.Core;
using iDAS.Items;

//Create: HuongLL - 22/11/2014
//Ham service cua bang Iso
namespace iDAS.Services
{
    public class ISOStandardSV
    {
        private ISOStandardDA ISOStandardDA = new ISOStandardDA();
        private ISOStandardModuleDA ISOStandardModuleDA = new ISOStandardModuleDA();
        private ModuleDA moduleDA = new ModuleDA();
        public List<ISOStandardItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var licenses = ISOStandardDA.GetQuery()
                             .Select(a => new ISOStandardItem()
                             {
                                 ID = a.ID,
                                 Name = a.Name,
                                 Code = a.Code,
                                 IsAnnex = a.IsAnnex,
                                 IsActive = a.IsActive,
                                 CreateAt = a.CreateAt
                             })
                             .OrderBy(item => item.Name)
                             .Page(page, pageSize, out totalCount)
                             .ToList();
            return licenses;
        }
        public List<ISOStandardItem> GetISOHasAnnex(int page, int pageSize, out int totalCount)
        {
            var licenses = ISOStandardDA.GetQuery(i=>i.IsAnnex)
                             .Select(a => new ISOStandardItem()
                             {
                                 ID = a.ID,
                                 Name = a.Name,
                                 Code = a.Code,
                                 IsAnnex = a.IsAnnex,
                                 IsActive = a.IsActive,
                                 CreateAt = a.CreateAt
                             })
                             .OrderBy(item => item.Name)
                             .Page(page, pageSize, out totalCount)
                             .ToList();
            return licenses;
        }
        public List<ISOStandardItem> GetISOActived()
        {
            var licenses = ISOStandardDA.GetQuery(i=>i.IsActive)
                             .Select(a => new ISOStandardItem()
                             {
                                 ID = a.ID,
                                 Name = a.Name,
                                 Code = a.Code,
                                 IsAnnex = a.IsAnnex,
                                 IsActive = a.IsActive,
                                 CreateAt = a.CreateAt
                             })
                             .OrderByDescending(item => item.Name)
                             .ToList();
            return licenses;
        }
        public ISOStandardItem GetByID(int id)
        {
            ISOStandardItem obj = new ISOStandardItem();
            var item = ISOStandardDA.GetById(id);
            if (item != null)
            {
                obj.ID = item.ID;
                obj.Code = item.Code;
                obj.Name = item.Name;
                obj.IsAnnex = item.IsAnnex;
                obj.UrlAvatar = item.UrlAvatar;
                obj.IsActive = (bool)item.IsActive;
            }
            return obj;
        }
        public int Insert(ISOStandardItem item)
        {
            var iso = new ISOStandard()
            {
                Name = item.Name,
                Code = item.Code,
                UrlAvatar = item.UrlAvatar,
                IsActive = item.IsActive,
                IsAnnex = item.IsAnnex,
                CreateAt = DateTime.Now,
                CreateBy = (int)item.CreateBy
            };
            ISOStandardDA.Insert(iso);
            ISOStandardDA.Save();
            return iso.ID;
        }
        public void Update(ISOStandardItem item)
        {
            var iso = ISOStandardDA.GetById(item.ID);
            iso.Name = item.Name;
            iso.Code = item.Code;
            iso.UrlAvatar = item.UrlAvatar;
            iso.IsActive = item.IsActive;
            iso.IsAnnex = item.IsAnnex;
            iso.UpdateBy = (int)item.UpdateBy;
            ISOStandardDA.Update(iso);
            ISOStandardDA.Save();
        }
        public void Delete(int id)
        {
            var iso = ISOStandardDA.GetById(id);
            ISOStandardDA.Delete(id);
            ISOStandardDA.Save();
        }
        public ISOStandardModule GetByiSOIdandmoduleId(int iSOId, int moduleId)
        {
            try
            {
                var rs = ISOStandardModuleDA.GetQuery(t => t.ISOStandardID == iSOId && t.CenterModuleID == moduleId).FirstOrDefault();
                return rs;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region Module
        public List<ISOStandardModuleItem> GetModuleByIsoNace(int id)
        {
            try
            {
                var modules = ISOStandardModuleDA.GetQuery()
                                  .OrderBy(a => a.CenterModule.Name)
                                  .Select(a => new ISOStandardModuleItem()
                                  {
                                      ID = a.ID,
                                      Code = a.CenterModule.Code,
                                      Module = a.CenterModule.Name,
                                      IsActive = a.IsActive,
                                      IsShow = a.IsShow,
                                      Description = a.CenterModule.Description
                                  }).ToList();
                return modules;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int[] GetModuleIDByIso(int id)
        {
            try
            {
                var modules = ISOStandardModuleDA.GetQuery()
                                  .Where(a => a.ISOStandardID == id)
                                  .OrderBy(a => a.CenterModule.Name)
                                  .Select(a => a.CenterModuleID.Value).ToArray();
                return modules;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int[] GetModuleIDByIsoIDs(int[] id)
        {
            try
            {
                var modules = ISOStandardModuleDA.GetQuery()
                                  .Where(a => id.Contains(a.ISOStandardID))
                                  .OrderBy(a => a.CenterModule.Name)
                                  .Select(a => a.CenterModuleID.Value).ToArray();
                return modules;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void InsertISOModule(ISOStandardModule obj)
        {
            ISOStandardModuleDA.Insert(obj);
            ISOStandardModuleDA.Save();
        }
        public void DeleteISOModule(int id)
        {
            try
            {
                var mpIsoModule = ISOStandardModuleDA.GetById(id);
                ISOStandardModuleDA.Delete(id);
                ISOStandardModuleDA.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<ISOStandardModuleItem> GetModuleAll()
        {
            var lst = moduleDA.GetQuery(p => p.IsActive && p.IsShow)
                                .OrderBy(a => a.Name)
                                  .Select(a => new ISOStandardModuleItem()
                                  {
                                      ID = a.ID,
                                      ModuleID = a.ID,
                                      Module = a.Name,
                                      Description = a.Description,
                                      Price = 0,
                                  }).ToList();

            return lst;
        }
        #endregion
    }
}
