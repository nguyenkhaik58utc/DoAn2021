using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class ServiceSV
    {
        private ServiceDA serviceDA = new ServiceDA();
        private ServiceCommandDA commandServiceDA = new ServiceCommandDA();
        // MpServiceStagesOfServiceService mpServiceStagesOfServiceService = new MpServiceStagesOfServiceService();
        public List<ServiceCategoryItem> GetCateAll()
        {
            List<ServiceCategoryItem> lst = new List<ServiceCategoryItem>();
            var dbo = serviceDA.Repository;
            var data = dbo.ServiceCategories.Where(t => !t.IsDelete && t.IsUse).ToList();
            if (data.Count > 0)
            {
              
                foreach (var item in data)
                {
                    lst.Add(new ServiceCategoryItem
                    {

                        Name = item.Name,
                        ID = item.ID
                    });
                }

            }
            return lst;
        }
        public bool CheckCodeExits(int groupid, string code)
        {
            var rs = (serviceDA.GetQuery().Where(t => t.Code.ToUpper() == code.ToUpper() && t.ServiceCategoryID == groupid)).ToList();
            if (rs.Count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CheckCodeExitEdits(int id,int groupid, string code)
        {
            var rs = (serviceDA.GetQuery().Where(t => t.Code.ToUpper() == code.ToUpper() && t.ServiceCategoryID == groupid && t.ID != id)).ToList();
            if (rs.Count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<ServiceItem> GetAllForCommand(int page, int pageSize, out int total, int commandId)
        {
            List<ServiceItem> lst = new List<ServiceItem>();
            var isos = serviceDA.GetQuery().OrderByDescending(t => t.CreateAt).Page(page, pageSize, out total).ToList();
            if (isos.Count > 0)
            {
                foreach (var item in isos)
                {
                    lst.Add(new ServiceItem
                    {
                        ID = item.ID,
                        Code = item.Code,
                        Name = item.Name,
                    });
                }
            }
            return lst;
        }
        public void Insert(ServiceItem objNew, int userId)
        {
            var obj = new Base.Service();
            obj.ServiceCategoryID = objNew.CategoryID;
            obj.Code = objNew.Code;
            obj.Cost = objNew.Cost;
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            obj.IsDelete = objNew.IsDelete;
            obj.IsUse = objNew.IsUse;
            obj.Name = objNew.Name;
            obj.Note = objNew.Note;
            serviceDA.Insert(obj);
            serviceDA.Save();
        }
        public void Update(ServiceItem objNew, int userId)
        {
            var obj = serviceDA.GetById(objNew.ID);
            obj.ServiceCategoryID = objNew.CategoryID;
            obj.Code = objNew.Code;
            obj.Cost = objNew.Cost;
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            obj.IsDelete = objNew.IsDelete;
            obj.IsUse = objNew.IsUse;
            obj.Name = objNew.Name;
            obj.Note = objNew.Note;
            serviceDA.Update(obj);
            serviceDA.Save();
        }
        public void Delete(string stringId)
        {
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            serviceDA.DeleteRange(ids);
            serviceDA.Save();
        }
        public ServiceItem GetByID(int id)
        {
            ServiceItem obj = new ServiceItem();
            var service = serviceDA.GetById(id);
            if (service != null)
            {
                obj.ID = service.ID;
                obj.Note = service.Note;
                obj.Name = service.Name;
                obj.CategoryID = service.ServiceCategoryID;
                obj.Note = service.Note;
                obj.Code = service.Code;
                obj.Cost = service.Cost;
                obj.IsUse = service.IsUse;
                obj.IsDelete = service.IsDelete;
            }
            return obj;
        }
        public List<ServiceItem> GetAll(ModelFilter filter)
        {
            try
            {
                List<ServiceItem> lst = new List<ServiceItem>();
                var lstService = serviceDA.GetQuery()
                    .OrderBy(t => t.Name)
                    .Filter(filter)
                    .ToList();
                if (lstService != null && lstService.Count > 0)
                {
                    foreach (var item in lstService)
                    {
                        lst.Add(new ServiceItem
                        {
                            ID = item.ID,
                            Name = item.Name,
                            CategoryID = item.ServiceCategoryID,
                            CategoryName = item.ServiceCategory.Name,
                            Code = item.Code,
                            Cost = item.Cost,
                            Note = item.Note,
                            IsDelete = item.IsDelete,
                            IsUse = Boolean.Parse(item.IsUse.ToString())
                        });
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public List<ServiceItem> GetByCate(ModelFilter filter, int cateId)
        {
            try
            {
                List<ServiceItem> lst = new List<ServiceItem>();
                var lstService = serviceDA.GetQuery(t=>t.ServiceCategoryID==cateId)
                    .OrderBy(t => t.Name)
                    .Filter(filter)
                    .ToList();
                if (lstService != null && lstService.Count > 0)
                {
                    foreach (var item in lstService)
                    {
                        lst.Add(new ServiceItem
                        {
                            ID = item.ID,
                            Name = item.Name,
                            CategoryID = item.ServiceCategoryID,
                            CategoryName = item.ServiceCategory.Name,
                            Code = item.Code,
                            Cost = item.Cost,
                            Note = item.Note,
                            IsDelete = item.IsDelete,
                            IsUse = Boolean.Parse(item.IsUse.ToString())
                        });
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public List<ServiceItem> GetForCombobox()
        {
            try
            {
                List<ServiceItem> lst = new List<ServiceItem>();
                var lstService = serviceDA.GetQuery(t=>!t.IsDelete && t.IsUse).OrderBy(t => t.Name).ToList();
                if (lstService != null && lstService.Count > 0)
                {
                    foreach (var item in lstService)
                    {
                        lst.Add(new ServiceItem
                        {
                            ID = item.ID,
                            Name = item.Name
                        });
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
