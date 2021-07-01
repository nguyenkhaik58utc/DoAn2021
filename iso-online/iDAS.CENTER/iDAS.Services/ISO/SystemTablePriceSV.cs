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
    //public class SystemTablePriceSV
    //{
    //    private SystemTablePriceDA SystemTablePriceDA = new SystemTablePriceDA();
    //    SystemDA cateDA = new SystemDA();
    //    public List<SystemTablePriceItem> GetAll(int page, int pageSize, out int totalCount)
    //    {
    //        var Service = SystemTablePriceDA.GetQuery(i=>!i.IsDelete)
    //                .Select(i => new SystemTablePriceItem()
    //                {
    //                    ID = i.ID,
    //                    OrderName = i.SystemForm.CenterSystem.Name + " - "+i.ISOStandard.Name,
    //                    SystemFormID = i.SystemFormID,
    //                    FormName = i.SystemForm.Name,
    //                    IsDelete = i.IsDelete,
    //                    ISOID = i.ISOStandardID,
    //                    ISOName = i.ISOStandard.Name,
    //                    IsUse = i.IsUse,
    //                    Price = i.Price,
    //                    CenterSystemID = i.CenterSystemID,
    //                    CenterSystemName = i.SystemForm.CenterSystem.Name,
    //                    Description = i.Description,
    //                    CreateAt = i.CreateAt,
    //                    FactoryNaceCode = i.FactoryNaceCode,
    //                    ISONaceCodeID = i.ISONaceCodeID,
    //                    ISONaceCodeName = i.ISONaceCode.Name,
    //                    PriceActive = i.PriceActive,
    //                    PriceOfYearNext = i.PriceOfYearNext,
    //                    TotalPrice = i.TotalPrice,
    //                    CreateBy = i.CreateBy,
    //                    UpdateAt = i.UpdateAt,
    //                    UpdateBy = i.UpdateBy
    //                })
    //                .OrderByDescending(item => item.ID)
    //                .Page(page, pageSize, out totalCount)
    //                .ToList();
    //        return Service;
    //    }
    //    public List<CategoryItem> GetSystemForm(int systemId)
    //    {
    //        var dbo = SystemTablePriceDA.Repository;
    //        var lst = dbo.SystemForms.Where(t => t.CenterSystemID == systemId && t.IsActive && !t.IsDelete).Select(a => new CategoryItem
    //        {
    //            ID = a.ID,
    //            Name = a.Name,

    //        }).ToList();
    //        return lst;
    //    }
    //    public SystemTablePriceItem GetById(int Id)
    //    {
    //        try
    //        {
    //            var dbo = SystemTablePriceDA.Repository;
    //            var Service = SystemTablePriceDA.GetQuery(i => i.ID == Id)
    //                .Select(i => new SystemTablePriceItem()
    //                {
    //                    ID = i.ID,
    //                    SystemFormID = i.SystemFormID,
    //                    FormName = i.SystemForm.Name,
    //                    IsDelete = i.IsDelete,
    //                    ISOID = i.ISOStandardID,
    //                    ISOName = i.ISOStandard.Name,
    //                    IsUse = i.IsUse,
    //                    Price = i.Price,
    //                    CenterSystemID = i.CenterSystemID,
    //                    CenterSystemName = i.SystemForm.CenterSystem.Name,
    //                    Description = i.Description,
    //                    CreateAt = i.CreateAt,
    //                    FactoryNaceCode = i.FactoryNaceCode,
    //                    ISONaceCodeID = i.ISONaceCodeID,
    //                    ISONaceCodeName = i.ISONaceCode.Name,
    //                    PriceActive = i.PriceActive,
    //                    PriceOfYearNext = i.PriceOfYearNext,
    //                    TotalPrice = i.TotalPrice,
    //                    CreateBy = i.CreateBy,
    //                    UpdateAt = i.UpdateAt,
    //                    UpdateBy = i.UpdateBy

    //                }).First();
    //            return Service;
    //        }
    //        catch
    //        {
    //            return null;
    //        }
    //    }


    //    public bool Insert(SystemTablePriceItem i)
    //    {
    //        try
    //        {
    //            SystemTablePrice obj = new SystemTablePrice();
    //            obj.SystemFormID = i.SystemFormID;
    //            obj.IsDelete = false;
    //            obj.ISOStandardID = i.ISOID;
    //            obj.IsUse = i.IsUse;
    //            obj.Price = i.Price;
    //            obj.CenterSystemID = i.CenterSystemID;
    //            obj.Description = i.Description;
    //            obj.CreateAt = i.CreateAt;
    //            obj.FactoryNaceCode = i.FactoryNaceCode;
    //            obj.ISONaceCodeID = i.ISONaceCodeID;
    //            obj.PriceActive = i.PriceActive;
    //            obj.PriceOfYearNext = i.PriceOfYearNext;
    //            obj.TotalPrice = i.TotalPrice;
    //            obj.CreateAt = DateTime.Now;
    //            obj.CreateBy = i.CreateBy;
    //            SystemTablePriceDA.Insert(obj);
    //            SystemTablePriceDA.Save();
    //            return true;
    //        }
    //        catch (Exception)
    //        {

    //            throw;
    //        }

    //    }

    //    public bool Update(SystemTablePriceItem i)
    //    {
    //        try
    //        {
    //            var obj = SystemTablePriceDA.GetById(i.ID);
    //            obj.SystemFormID = i.SystemFormID;
    //            obj.IsDelete = false;
    //            obj.ISOStandardID = i.ISOID;
    //            obj.IsUse = i.IsUse;
    //            obj.Price = i.Price;
    //            obj.CenterSystemID = i.CenterSystemID;
    //            obj.Description = i.Description;
    //            obj.CreateAt = i.CreateAt;
    //            obj.FactoryNaceCode = i.FactoryNaceCode;
    //            obj.ISONaceCodeID = i.ISONaceCodeID;
    //            obj.PriceActive = i.PriceActive;
    //            obj.PriceOfYearNext = i.PriceOfYearNext;
    //            obj.TotalPrice = i.TotalPrice;
    //            obj.UpdateAt = DateTime.Now;
    //            obj.UpdateBy = i.UpdateBy;
    //            SystemTablePriceDA.Update(obj);
    //            SystemTablePriceDA.Save();
    //            return true;
    //        }
    //        catch (Exception)
    //        {
    //            throw;
    //        }
    //    }


    //    public bool Delete(int Id)
    //    {
    //        try
    //        {
    //            var item = SystemTablePriceDA.GetById(Id);
    //            item.IsDelete = true;
    //            SystemTablePriceDA.Update(item);
    //            SystemTablePriceDA.Save();
    //            return true;
    //        }
    //        catch
    //        {
    //            return false;
    //        }
    //    }

    //    public bool CheckExits(int isoId, int systemId, int formId, int naceCodeId)
    //    {
    //        try
    //        {
    //            var rs = SystemTablePriceDA.GetQuery(t => t.ISOStandardID == isoId && t.SystemFormID == systemId && t.SystemFormID == formId && !t.IsDelete && t.ISONaceCodeID==naceCodeId&& t.IsUse)
    //                .ToList();
    //            if (rs.Count > 0)
    //            {
    //                return true;
    //            }
    //            else
    //                return false;
    //        }
    //        catch (Exception ex)
    //        {
    //            return false;
    //        }
    //    }

    //}
}
