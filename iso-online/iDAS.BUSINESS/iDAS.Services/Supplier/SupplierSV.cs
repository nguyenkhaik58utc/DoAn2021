using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
using iDAS.Base;
//using iDAS.DA.Generic;

namespace iDAS.Services
{
    public class SupplierSV
    {
        private SupplierDA supplierDA = new SupplierDA();
        public List<ComboboxItem> ComboboxDepartment(int page, int pageSize, out int total, string name)
        {
            var dbo = supplierDA.Repository;
            var data = dbo.HumanDepartments
                        .Where(i => string.IsNullOrEmpty(name) || i.Name.Contains(name))
                        .Where(i => i.IsActive == true || i.IsCancel || i.IsMerge || i.IsDelete)
                        .Where(i => i.IsCancel == false)
                        .Where(i => i.IsMerge == false)
                        .Where(i => i.IsDelete == false)
                        .Where(i => !i.IsDestroy)
                        .OrderBy(i => i.Position)
                        .Select(
                                item => new ComboboxItem()
                                {
                                    ID = item.ID,
                                    Name = item.Name,
                                }
                                )
                        .Page(page, pageSize, out total)
                        .ToList();
            return data;
        }
        public SupplierItem GetDepartmentById(int Id)
        {
            var dbo = supplierDA.Repository;
            var supplier = dbo.HumanDepartments.Where(i => i.ID == Id)
                                .Select(item => new SupplierItem()
                                {
                                    ID = item.ID,
                                    Code = !string.IsNullOrEmpty(item.Code) ? item.Code : string.Empty,
                                    Name = !string.IsNullOrEmpty(item.Name) ? item.Name : string.Empty,
                                    Phone = !string.IsNullOrEmpty(item.Phone) ? item.Phone : string.Empty,
                                    Address = !string.IsNullOrEmpty(item.Address) ? item.Address : string.Empty,
                                    CreateAt = item.CreateAt.Value

                                }).First();
            return supplier;
        }
        public bool CheckNameExits(string name)
        {
            var rs = (supplierDA.GetQuery().Where(t => t.Name.ToUpper() == name.ToUpper())).ToList();
            if (rs.Count >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<ComboboxItem> Combobox(int page, int pageSize, out int total, string query)
        {
            var lst = supplierDA.GetQuery(t => t.Name.Contains(query) || string.IsNullOrEmpty(query)).Select(item => new ComboboxItem()
            {
                ID = item.ID,
                Name = item.Name,
            })
            .OrderBy(i => i.Name)
            .Page(page, pageSize, out total)
            .ToList();
            return lst;
        }
        public bool CheckCodeExits(string code)
        {
            var data = supplierDA.GetQuery().OrderBy(t => t.ID).Where(t => t.Code.ToUpper() == code.ToUpper()).ToList();
            if (data.Count > 0)
            {
                return true;
            }
            return false;
        }
        public SupplierItem GetById(int id)
        {
            SupplierItem obj = new SupplierItem();
            var data = supplierDA.Get(t => t.ID == id).FirstOrDefault();
            if (data != null)
            {
                obj.ID = data.ID;
                obj.Code = data.Code;
                obj.Address = data.Address != null ? data.Address : "";
                obj.Phone = data.Phone != null ? data.Phone : "";
                obj.Fax = data.Fax != null ? data.Fax : "";
                obj.Name = data.Name;
                obj.CountryId = data.CountryId;
                obj.BrandName = data.BrandName;
                obj.Tax = data.Tax != null ? data.Tax : "";
                obj.AcountNumber = data.AcountNumber;
                obj.BankName = data.BankName;
                obj.Status = data.Status;
                obj.SuppliersGroupId = data.SuppliersGroupID;
                obj.SuppliersGroupName = data.SuppliersGroup.Name;
                obj.Description = data.Description;
                obj.Email = data.Email;
                obj.IsCustomer = data.IsCustomer;
                obj.AttachmentFileID = data.AttachmentFileID;
                obj.ImageFile = data.AttachmentFile != null ? new FileItem()
                            {
                                ID = data.AttachmentFile.ID,
                                Name = data.AttachmentFile.Name,
                                Data = data.AttachmentFile.Data,
                            } : null;
                obj.Position = data.Position;
                obj.ProvinceId = data.ProvinceId;
                obj.Representative = data.Representative;
                obj.Website = data.Website;
                obj.Commodity = data.Commodity;
            }
            return obj;
        }
        public void updateIsOwedOn(int supplierid, double? isOwedOn)
        {
            var oldObj = supplierDA.GetById(supplierid);
            oldObj.IsOwedOn = isOwedOn;
            supplierDA.Update(oldObj);
            supplierDA.Save();
        }
        public void updateAddIsOwedOn(int supplierid, double? isOwedOn)
        {
            var oldObj = supplierDA.GetById(supplierid);
            oldObj.IsOwedOn = oldObj.IsOwedOn + isOwedOn;
            supplierDA.Update(oldObj);
            supplierDA.Save();
        }
        public void updateTotalImportValue(int supplierid, double totalImportValue)
        {
            var oldObj = supplierDA.GetById(supplierid);
            oldObj.TotalImportValue = oldObj.TotalImportValue + totalImportValue;
            supplierDA.Update(oldObj);
            supplierDA.Save();
        }
        public string GetMaxCode()
        {
            var lstTmp = supplierDA.GetQuery().OrderByDescending(t => t.ID).FirstOrDefault();
            if (lstTmp != null)
                return lstTmp.Code;
            return "";
        }
        public List<SupplierItem> GetForAddSupplier(string[] strwhere)
        {
            var output = Array.ConvertAll(strwhere, s => int.Parse(s));
            List<SupplierItem> lst = new List<SupplierItem>();
            var data = supplierDA.GetQuery().OrderByDescending(t => t.ID).Where(t => !output.Contains(t.ID)).ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var item in data)
                {
                    lst.Add(new SupplierItem
                    {
                        ID = item.ID,
                        Code = item.Code,
                        Address = item.Address,
                        BrandName = item.BrandName,
                       // CustomerGroupId = item.CustomerGroupId,
                        // CustomerGroupName = item..Name,
                        Description = item.Description,
                        Email = item.Email,
                        Fax = item.Fax,
                        IsCustomer = item.IsCustomer,
                        //Logo = item.Logo,
                        Tax = item.Tax,
                        AcountNumber = item.AcountNumber,
                        BankName = item.BankName,
                        Status = item.Status,
                        Name = item.Name,
                        Phone = item.Phone,
                        Position = item.Position,
                        ProvinceId = item.ProvinceId,
                        // ProvinceName = item.supplierListProvince.Name,
                        Representative = item.Representative,
                        Website = item.Website,
                        // CountryName = item.supplierListProvince.supplierListCountry.Name
                    });
                }
            }
            return lst;
        }
        public List<SupplierItem> GetAll(int page, int pageSize, out int totalCount)
        {
                 
            var data = supplierDA.GetQuery().Select(item => new SupplierItem()
                        {
                            ID = item.ID,
                            Code = item.Code,
                            Address = item.Address,
                            BrandName = item.BrandName,
                            SuppliersGroupId = item.SuppliersGroupID,
                            SuppliersGroupName = item.SuppliersGroup.Name,
                            Description = item.Description,
                            Email = item.Email,
                            Fax = item.Fax,
                            IsCustomer = item.IsCustomer,
                           // Logo = item.Logo,
                            Tax = item.Tax,
                            AcountNumber = item.AcountNumber,
                            BankName = item.BankName,
                            Status = item.Status,
                            Name = item.Name,
                            Phone = item.Phone,
                            Position = item.Position,
                            ProvinceId = item.ProvinceId,                            
                            Representative = item.Representative,
                            Website = item.Website,
                            CountryId = item.CountryId,
                            AttachmentFileID = item.AttachmentFileID,                            
                            FileName = item.AttachmentFile != null ? item.AttachmentFile.Name : ""                          
                        }).OrderByDescending(item => item.ID)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            //var lstCounty = new CommonCountrySV().getCombobox();
            //var lstCity = new CommonCityDA().Repository;
            foreach(var _item in data)
            {
                //_item.CountryName =_item.CountryId.HasValue? lstCounty.FirstOrDefault(i => i.ID == _item.CountryId).Name: string.Empty;
                //_item.ProvinceName = _item.CountryId.HasValue ? lstCity.CommonCities.FirstOrDefault(i => i.ID == _item.ProvinceId).Name : string.Empty;
            }
                        return data;
        }
        private string ReturnStatus(int id)
        {
            string s = "";
            if (id == 1)
            {
                s = "<span style='color:blue'>Đang giao dịch</span>";
            }
            else if (id == 2)
            {
                s = "<span style='color:green'>Đang dự kiến mua hàng</span>";
            }
            else
                s = "<span style='color:red'>Không còn giao dịch</span>";
            return s;

        }


        public void Insert(SupplierItem data, int p)
        {
            Supplier obj = new Supplier();
            if (data != null)
            {
                obj.Code = data.Code;
                obj.Address = data.Address != null ? data.Address : "";
                obj.Phone = data.Phone != null ? data.Phone : "";
                obj.Fax = data.Fax != null ? data.Fax : "";
                obj.Name = data.Name;
                obj.CountryId = data.CountryId;
                obj.BrandName = data.BrandName;
                obj.Tax = data.Tax != null ? data.Tax : "";
                obj.AcountNumber = data.AcountNumber;
                obj.BankName = data.BankName;
                obj.Status = data.Status;
                obj.SuppliersGroupID = data.SuppliersGroupId;
                obj.Description = data.Description;
                obj.Email = data.Email;
                obj.IsCustomer = data.IsCustomer;
                //obj.Logo = data.Logo;
                obj.Commodity = data.Commodity;
                obj.Position = data.Position;
                obj.ProvinceId = data.ProvinceId;
                obj.Representative = data.Representative;
                obj.Website = data.Website;
                obj.CreateAt = DateTime.Now;
                obj.CreateBy = p;

            }
            if (data.ImageFile != null)
            {
                var imgId = new FileSV().Insert(data.ImageFile, p);
                obj.AttachmentFileID = imgId;
            }
            supplierDA.Insert(obj);
            supplierDA.Save();
        }

        public void Update(SupplierItem data, int p)
        {
            var obj = supplierDA.GetById(data.ID);
            if (data.ImageFile != null)
            {
                if (obj.AttachmentFileID == null)
                {
                    data.ImageFile.ID = Guid.NewGuid();
                    new FileSV().Insert(data.ImageFile, p);
                    obj.AttachmentFileID = data.ImageFile.ID;
                }
                else
                {
                    data.ImageFile.ID = (Guid)obj.AttachmentFileID;
                    FileDA FileDA = new FileDA();
                    var file = FileDA.GetById(data.ImageFile.ID);
                    file.ID = data.ImageFile.ID;
                    file.Name = data.ImageFile.Name;
                    file.Extension = data.ImageFile.Extension;
                    //file.Path = item.Path;
                    file.Size = data.ImageFile.Size;
                    file.Type = data.ImageFile.Type;
                    file.Data = data.ImageFile.Data;
                    file.UpdateAt = DateTime.Now;
                    file.UpdateBy = p;
                    FileDA.Save();
                    obj.AttachmentFileID = file.ID;
                }
            }
            if (data != null)
            {
                obj.Code = data.Code;
                obj.Address = data.Address != null ? data.Address : "";
                obj.Phone = data.Phone != null ? data.Phone : "";
                obj.Fax = data.Fax != null ? data.Fax : "";
                obj.Name = data.Name;
                obj.CountryId = data.CountryId;
                obj.BrandName = data.BrandName;
                obj.Tax = data.Tax != null ? data.Tax : "";
                obj.AcountNumber = data.AcountNumber;
                obj.BankName = data.BankName;
                obj.Status = data.Status;
                obj.SuppliersGroupID = data.SuppliersGroupId;
                obj.Description = data.Description;
                obj.Email = data.Email;
                obj.IsCustomer = data.IsCustomer;
                //obj.Logo = data.Logo;
                obj.Commodity = data.Commodity;
                obj.Position = data.Position;
                obj.ProvinceId = data.ProvinceId;
                obj.Representative = data.Representative;
                obj.Website = data.Website;
                obj.CreateAt = DateTime.Now;
                obj.CreateBy = p;
            }
            supplierDA.Update(obj);
            supplierDA.Save();
        }

        public void Delete(int id)
        {
            supplierDA.Delete(id);
            supplierDA.Save();
        }

        public List<SupplierItem> GetAllByGroupID(ModelFilter filter, int GroupID)
        {
            var dbo = supplierDA.Repository;
            var rs = getChilds(dbo.SuppliersGroups, GroupID);
            var lstSupp = rs.SelectMany(i=>i.Suppliers).ToList();
                  
            
            var data = lstSupp.Select(item => new SupplierItem()
            {
                ID = item.ID,
                Code = item.Code,
                Address = item.Address,
                BrandName = item.BrandName,
                SuppliersGroupId= item.SuppliersGroupID,
                SuppliersGroupName = item.SuppliersGroup.Name,
                Description = item.Description,
                Email = item.Email,
                Fax = item.Fax,
                IsCustomer = item.IsCustomer,                
                Tax = item.Tax,
                AcountNumber = item.AcountNumber,
                BankName = item.BankName,
                Status = item.Status,
                Name = item.Name,
                Phone = item.Phone,
                Position = item.Position,
                ProvinceId = item.ProvinceId,
                Representative = item.Representative,
                Website = item.Website,
                Commodity = item.Commodity,
                CountryId = item.CountryId,
                AttachmentFileID = item.AttachmentFileID,
                FileName = item.AttachmentFile != null ? item.AttachmentFile.Name : "" 
            }).OrderByDescending(item => item.ID)
                        .Filter(filter)
                        .ToList();
            //var lstCounty = new CommonCountrySV().getCombobox();
            //var lstCity = new CommonCityDA().Repository;
            foreach (var _item in data)
            {
                //_item.CountryName = _item.CountryId.HasValue ? lstCounty.FirstOrDefault(i => i.ID == _item.CountryId).Name : string.Empty;
                //_item.ProvinceName = _item.CountryId.HasValue ? lstCity.CommonCities.FirstOrDefault(i => i.ID == _item.ProvinceId).Name : string.Empty;
            }
            return data;
        }
        private IEnumerable<SuppliersGroup> getChilds(IEnumerable<SuppliersGroup> e, int id)
        {
            var suppGroup = e.Where(a => a.ParentID == id);
            var suppGroupFirst = e.Where(a => a.ID == id);
            suppGroupFirst.Concat(suppGroup);
            return suppGroupFirst.Concat(suppGroup.SelectMany(a => getChilds(e, a.ID)));
        }

        public List<SupplierItem> GetByCategory(ModelFilter filter, int cateId)
        {
            var dbo = supplierDA.Repository;
            var lstChild = new SuppliersGroupSV().getChilds(dbo.SuppliersGroups, cateId).Select(t => t.ID).ToList();
            var data = supplierDA.GetQuery(x=>lstChild.Contains(x.SuppliersGroupID)).Select(item => new SupplierItem()
            {
                ID = item.ID,
                Code = item.Code,
                Address = item.Address,
                BrandName = item.BrandName,
                SuppliersGroupId = item.SuppliersGroupID,
                SuppliersGroupName = item.SuppliersGroup.Name,
                Description = item.Description,
                Email = item.Email,
                Fax = item.Fax,
                IsCustomer = item.IsCustomer,
                Tax = item.Tax,
                AcountNumber = item.AcountNumber,
                BankName = item.BankName,
                Status = item.Status,
                Name = item.Name,
                Phone = item.Phone,
                Position = item.Position,
                ProvinceId = item.ProvinceId,
                Representative = item.Representative,
                Website = item.Website,
                CountryId = item.CountryId,
                AttachmentFileID = item.AttachmentFileID,
                FileName = item.AttachmentFile != null ? item.AttachmentFile.Name : ""
            })
            .OrderByDescending(item => item.ID)
            .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
            .ToList();
            return data;
        }

        public List<SupplierItem> GetSupplierTrans(ModelFilter filter, int cateId, DateTime fromDate, DateTime toDate)
        {
            var dbo = supplierDA.Repository;
            var lstChild = new SuppliersGroupSV().getChilds(dbo.SuppliersGroups, cateId).Select(t => t.ID).ToList();
            var rs= dbo.SuppliersOrders.Where(x => lstChild.Contains(x.Supplier.SuppliersGroupID))
                        .Where(item => item.OrderDate.HasValue)
                        .Where(item => item.OrderDate >= fromDate && item.OrderDate <= toDate)
                        .Select(item => new SupplierItem()
                        {
                            ID = item.Supplier.ID,
                            Code = item.Supplier.Code,
                            Address = item.Supplier.Address,
                            BrandName = item.Supplier.BrandName,
                            SuppliersGroupId = item.Supplier.SuppliersGroupID,
                            SuppliersGroupName = item.Supplier.SuppliersGroup.Name,
                            Description = item.Supplier.Description,
                            Email = item.Supplier.Email,
                            Fax = item.Supplier.Fax,
                            IsCustomer = item.Supplier.IsCustomer,
                            Tax = item.Supplier.Tax,
                            AcountNumber = item.Supplier.AcountNumber,
                            BankName = item.Supplier.BankName,
                            Status = item.Supplier.Status,
                            Name = item.Supplier.Name,
                            Phone = item.Supplier.Phone,
                            Position = item.Supplier.Position,
                            ProvinceId = item.Supplier.ProvinceId,
                            Representative = item.Supplier.Representative,
                            Website = item.Supplier.Website,
                            CountryId = item.Supplier.CountryId,
                            AttachmentFileID = item.Supplier.AttachmentFileID,
                            FileName = item.Supplier.AttachmentFile != null ? item.Supplier.AttachmentFile.Name : ""
                        })
                        .Distinct()
            .OrderByDescending(item => item.ID)
            .Page(filter.PageIndex, filter.PageSize, out filter.PageTotal)
            .ToList();
            return rs;
        }
    }
}
