using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.DA;
using iDAS.Items;
using iDAS.Base;
using iDAS.Core;
using iDAS.Utilities;

namespace iDAS.Services
{
    public class CustomerRegisterSystemSV
    {
        CustomerRegisterSystemDA CustomerRegisterDA = new CustomerRegisterSystemDA();
        private List<ProductSystemItem> ProductSystem = new List<ProductSystemItem>()
            {
                new ProductSystemItem(){SystemCode="BUSINESS",SystemName ="Doanh nghiệp"},
                new ProductSystemItem(){SystemCode="OFFICE",SystemName="Hành chính công"},
                new ProductSystemItem(){SystemCode="Option",SystemName="Tiện ích"}
            };
        public List<CustomerRegisterSystemItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var dbo = CustomerRegisterDA.Repository;
            var lstCustomer = CustomerRegisterDA.GetQuery()
                                .Select(i => new CustomerRegisterSystemItem()
                                {
                                    ID = i.ID,
                                    RepresentName = i.Customer.Name,
                                    CustomerID = i.CustomerID,
                                    IsContact = i.IsContact,
                                    IsAccept = i.IsAccept,
                                    Email = i.Customer.Email,
                                    Phone = i.Customer.Phone,
                                    ContactAt = i.ContactAt,
                                    ContactName = i.ContactBy.HasValue ? dbo.SystemUsers.FirstOrDefault(t => t.ID == i.ContactBy).Name : string.Empty,
                                    Company = i.Customer.Company,
                                    Note = i.Note,
                                    RegisterAt = i.RegisterAt,
                                    UpdateAt = i.UpdateAt,
                                    IsNew = i.IsNew,
                                })
                                .OrderByDescending(item => item.RegisterAt)
                                .Page(page, pageSize, out totalCount)
                                .ToList();
            totalCount = lstCustomer.Count();
            return lstCustomer;
        }
        public List<CustomerRegisterSystemItem> GetCustomer(int page, int pageSize, out int totalCount)
        {
            var dbo = CustomerRegisterDA.Repository;
            var lstCustomer = CustomerRegisterDA.GetQuery()
                                .Select(i => i.Customer).Distinct()
                                .Select(i => new CustomerRegisterSystemItem()
                                {
                                    RepresentName = i.Name,
                                    CustomerID = i.ID,
                                    IsContact = i.IsContact,
                                    IsAccept = i.IsAccept,
                                    Email = i.Email,
                                    Phone = i.Phone,
                                    ContactAt = i.ContactAt,
                                    ContactName = i.ContactBy.HasValue ? dbo.SystemUsers.FirstOrDefault(t => t.ID == i.ContactBy).Name : string.Empty,
                                    Company = i.Company,
                                    Note = i.Note,
                                    RegisterAt = i.RegisterAt,
                                    UpdateAt = i.UpdateAt,
                                    //IsNew = dbo.CustomerRegisterServices.Where(r => r.CustomerID == i.ID && r.IsNew).FirstOrDefault() != null ? true : false,
                                })
                                .OrderByDescending(item => item.RegisterAt)
                                .Page(page, pageSize, out totalCount)
                                .ToList();
            totalCount = lstCustomer.Count();
            return lstCustomer;
        }
        public CustomerRegisterSystemItem GetById(int Id)
        {
            var dbo = CustomerRegisterDA.Repository;
            var service = CustomerRegisterDA.GetQuery(i => i.ID == Id)
                .Select(i => new CustomerRegisterSystemItem()
                {
                    ID = i.ID,
                    ISOID = i.ISOStandardID,
                    SystemFormID = i.SystemFormID,
                    CustomerID = i.CustomerID,
                    CustomerName = i.Customer.Name,
                    RequireAt = i.RegisterAt,
                    RegisterAt = i.RegisterAt,
                    IsActive = i.IsActive,
                    ActiveAt = i.ActiveAt,
                    ActiveBy = i.ActiveBy,
                    IsContact = i.IsContact,
                    ContactAt = i.ContactAt,
                    ContactBy = i.ContactBy,
                    ContactName = dbo.SystemUsers.FirstOrDefault(u => u.ID == i.ContactBy).Name,
                    IsNew = i.IsNew,
                    IsUse = i.IsUse,
                    IsAccept = i.IsAccept,
                    IsDelete = i.IsDelete,
                    Note = i.Note
                }).First();
            return service;
        }
        public bool Delete(int Id)
        {
            CustomerRegisterDA.Delete(Id);
            CustomerRegisterDA.Save();
            return true;
        }
        public void UpdateAuthenticate(CustomerRegisterSystemItem i, int updateBy)
        {
            var obj = CustomerRegisterDA.GetById(i.ID);
            obj.IsContact = i.IsContact;
            obj.Note = i.Note;
            obj.IsAccept = i.IsAccept;
            obj.ContactAt = DateTime.Now;
            obj.ContactBy = updateBy;
            CustomerRegisterDA.Update(obj);
            CustomerRegisterDA.Save();
        }
        public List<CustomerRegisterSystemItem> GetByCustomer(int page, int pageSize, out int totalCount, int customerID)
        {
            return CustomerRegisterDA.GetQuery(i => i.CustomerID == customerID)
                    .Select(i => new CustomerRegisterSystemItem
                    {
                        ID = i.ID,
                        ISOName = i.ISOStandard.Name,
                        FormID = i.SystemFormID,
                        //FormName = i.SystemForm.Name,
                        ISONaceCodeName = i.ISONaceCode.Name,
                        CustomerID = i.CustomerID,
                        CustomerName = i.Customer.Name,
                        RequireAt = i.RegisterAt,
                        RegisterAt = i.RegisterAt,
                        IsActive = i.IsActive,
                        ActiveAt = i.ActiveAt,
                        ActiveBy = i.ActiveBy,
                        IsContact = i.IsContact,
                        ContactAt = i.ContactAt,
                        ContactBy = i.ContactBy,
                        IsNew = i.IsNew,
                        IsUse = i.IsUse,
                        IsAccept = i.IsAccept,
                        IsDelete = i.IsDelete,
                        Note = i.Note
                    })
                    .OrderByDescending(item => item.RegisterAt)
                    .Page(page, pageSize, out totalCount)
                    .ToList();
        }
        public bool ReportCustomerSystem(int CustomerRegisterId)
        {
            var dbo = CustomerRegisterDA.Repository;
            var customerRegister = CustomerRegisterDA.GetQuery(item => item.ID == CustomerRegisterId).FirstOrDefault();
            var customerSystem = dbo.CustomerSystems.Where(i => i.CustomerID == customerRegister.CustomerID
                //&& i.CenterSystemID == customerRegister.SystemForm.CenterSystemID
                ).FirstOrDefault();
            // Kiểm tra tồn tại của bản ghi
            bool exits = customerSystem != null ? true : false;
            if (exits)
            {
                customerSystem.IsNew = true;
            }
            else
            {
                var customerSystemAdd = new CustomerSystem
                {
                    CustomerID = customerRegister.CustomerID,
                    //CenterSystemID = (int)customerRegister.SystemForm.CenterSystemID,
                    LogoFile = customerRegister.Customer.LogoFile,
                    IsNew = true,
                };
                dbo.CustomerSystems.Add(customerSystemAdd);
            }
            dbo.SaveChanges();
            return true;
        }
        public List<CustomerRegisterProductPakageItem> GetPakage(int page, int pageSize, out int totalCount, int customerId)
        {
            var dbo = CustomerRegisterDA.Repository;
            return dbo.ProductPakages
                    .OrderBy(i => i.ISOStandardID)
                   .Page(page, pageSize, out totalCount)
                   .Select(item => new
                   {
                       ISOCode = item.ISOStandard.Code,
                       SystemCode = item.SystemCode,
                       PakageForm = item.ProductPakageForm.Name,
                       ApplyFor = item.ApplyFor,
                       ProductPakageID = item.ID,
                       RegisterProductPakages = dbo.CustomerRegisterProductPakages.Where(i => i.CustomerID == customerId && i.ProductPakageID == item.ID).FirstOrDefault()
                   })
                   .ToList()
                   .Select(item => new CustomerRegisterProductPakageItem
                   {
                       ID = item.RegisterProductPakages == null ? 0 : item.RegisterProductPakages.ID,
                       ProductPakageName = ProductSystem.Where(i => i.SystemCode == item.SystemCode).Select(i => i.SystemName).FirstOrDefault()
                                            + "/ " + item.ISOCode + "/ " + item.PakageForm,
                       ISOStandardCode = item.ISOCode,
                       ApplyFor = item.ApplyFor,
                       ISONaceCodeID = item.RegisterProductPakages == null ? 0 : item.RegisterProductPakages.ISONaceCodeID,
                       ProductPakageID = item.ProductPakageID,
                       IsSelect = item.RegisterProductPakages != null
                   }).ToList();
        }
        public void UpdatePakage(CustomerRegisterProductPakageItem pakageUpdate, int userId)
        {
            var dbo = CustomerRegisterDA.Repository;
            if (pakageUpdate.IsSelect)
            {
                if (pakageUpdate.ID == 0)
                {
                    dbo.CustomerRegisterProductPakages.Add(new CustomerRegisterProductPakage()
                        {
                            CustomerID = pakageUpdate.CustomerID,
                            ISONaceCodeID = pakageUpdate.ISONaceCodeID,
                            ProductPakageID = pakageUpdate.ProductPakageID,
                            CreateAt = DateTime.Now,
                            CreateBy = userId
                        });
                }
                else
                {
                    var data = dbo.CustomerRegisterProductPakages.Where(i => i.ID == pakageUpdate.ID).FirstOrDefault();
                    data.ISONaceCodeID = pakageUpdate.ISONaceCodeID;
                    data.ProductPakageID = pakageUpdate.ProductPakageID;
                    data.UpdateAt = DateTime.Now;
                    data.UpdateBy = userId;
                }
            }
            else
            {
                if (pakageUpdate.ID != 0)
                {
                    dbo.CustomerRegisterProductPakages.RemoveRange(dbo.CustomerRegisterProductPakages.Where(i => i.ID == pakageUpdate.ID));
                }
                else
                {
                    dbo.CustomerRegisterProductPakages.Add(new CustomerRegisterProductPakage()
                    {
                        CustomerID = pakageUpdate.CustomerID,
                        ISONaceCodeID = pakageUpdate.ISONaceCodeID,
                        ProductPakageID = pakageUpdate.ProductPakageID,
                        CreateAt = DateTime.Now,
                        CreateBy = userId
                    });
                }
            }
            dbo.SaveChanges();
        }
        public List<CustomerRegisterProductBlockItem> GetBlock(int page, int pageSize, out int totalCount, int customerId)
        {
            var dbo = CustomerRegisterDA.Repository;
            return dbo.ProductBlocks
                    .OrderBy(i => i.CategoryCode)
                   .Page(page, pageSize, out totalCount)
                   .Select(item => new
                   {
                       ProductBlockID = item.ID,
                       ProductBlockName = item.Name,
                       RegisterProductBlock = dbo.CustomerRegisterProductBlocks.Where(i => i.CustomerID == customerId && i.ProductBlockID == item.ID).FirstOrDefault()
                   })
                   .ToList()
                   .Select(item => new CustomerRegisterProductBlockItem
                   {
                       ID = item.RegisterProductBlock == null ? 0 : item.RegisterProductBlock.ID,
                       ProductBlockID = item.ProductBlockID,
                       ProductBlockName = item.ProductBlockName,
                       IsSelect = item.RegisterProductBlock != null
                   }).ToList();
        }
        public void UpdateBlock(CustomerRegisterProductBlockItem blockUpdate, int userId)
        {
            var dbo = CustomerRegisterDA.Repository;
            if (blockUpdate.IsSelect)
            {
                if (blockUpdate.ID == 0)
                {
                    dbo.CustomerRegisterProductBlocks.Add(new CustomerRegisterProductBlock()
                    {
                        CustomerID = blockUpdate.CustomerID,
                        ProductBlockID = blockUpdate.ProductBlockID,
                        CreateAt = DateTime.Now,
                        CreateBy = userId
                    });
                }
            }
            else
            {
                if (blockUpdate.ID != 0)
                {
                    dbo.CustomerRegisterProductBlocks.RemoveRange(dbo.CustomerRegisterProductBlocks.Where(i => i.ID == blockUpdate.ID));
                }
            }
            dbo.SaveChanges();
        }

        public List<CustomerRegisterServiceTrainingItem> GetTraining(int page, int pageSize, out int totalCount, int customerId)
        {
            var dbo = CustomerRegisterDA.Repository;
            return dbo.ServiceTrainings
                    .OrderBy(i => i.CreateAt)
                   .Page(page, pageSize, out totalCount)
                   .Select(item => new
                   {
                       ServiceTrainingID = item.ID,
                       ApplyFor = item.ApplyFor,
                       ISOName = item.ISOStandard.Code,
                       RegisterProductBlock = dbo.CustomerRegisterServiceTrainings.Where(i => i.CustomerID == customerId && i.ServiceTrainingID == item.ID)
                                                        .FirstOrDefault()
                   })
                   .ToList()
                   .Select(item => new CustomerRegisterServiceTrainingItem
                   {
                       ID = item.RegisterProductBlock == null ? 0 : item.RegisterProductBlock.ID,
                       ServiceTrainingID = item.ServiceTrainingID,
                       ApplyFor = item.ApplyFor,
                       ISOName = item.ISOName,
                       IsSelect = item.RegisterProductBlock != null
                   }).ToList();
        }
        public void UpdateTraining(CustomerRegisterServiceTrainingItem trainingUpdate, int userId)
        {
            var dbo = CustomerRegisterDA.Repository;
            if (trainingUpdate.IsSelect)
            {
                if (trainingUpdate.ID == 0)
                {
                    dbo.CustomerRegisterServiceTrainings.Add(new CustomerRegisterServiceTraining()
                    {
                        CustomerID = trainingUpdate.CustomerID,
                        ServiceTrainingID = trainingUpdate.ServiceTrainingID,
                        CreateAt = DateTime.Now,
                        CreateBy = userId
                    });
                }
            }
            else
            {
                if (trainingUpdate.ID != 0)
                {
                    dbo.CustomerRegisterServiceTrainings.RemoveRange(dbo.CustomerRegisterServiceTrainings.Where(i => i.ID == trainingUpdate.ID));
                }
            }
            dbo.SaveChanges();
        }

        public List<CustomerRegisterServiceConsultancyItem> GetConsultancy(int page, int pageSize, out int totalCount, int customerId)
        {
            var dbo = CustomerRegisterDA.Repository;
            return dbo.ServiceConsultancies
                    .OrderBy(i => i.CreateAt)
                   .Page(page, pageSize, out totalCount)
                   .Select(item => new
                   {
                       ServiceConsultancyID = item.ID,
                       ApplyFor = item.ApplyFor,
                       ISOName = item.ISOStandard.Code,
                       RegisterProductBlock = dbo.CustomerRegisterServiceConsultancies.Where(i => i.CustomerID == customerId && i.ServiceConsultancyID == item.ID)
                                                        .FirstOrDefault()
                   })
                   .ToList()
                   .Select(item => new CustomerRegisterServiceConsultancyItem
                   {
                       ID = item.RegisterProductBlock == null ? 0 : item.RegisterProductBlock.ID,
                       ServiceConsultancyID = item.ServiceConsultancyID,
                       ApplyFor = item.ApplyFor,
                       ISOName = item.ISOName,
                       IsSelect = item.RegisterProductBlock != null
                   }).ToList();
        }
        public void UpdateConsultancy(CustomerRegisterServiceConsultancyItem consultancyUpdate, int userId)
        {
            var dbo = CustomerRegisterDA.Repository;
            if (consultancyUpdate.IsSelect)
            {
                if (consultancyUpdate.ID == 0)
                {
                    dbo.CustomerRegisterServiceConsultancies.Add(new CustomerRegisterServiceConsultancy()
                    {
                        CustomerID = consultancyUpdate.CustomerID,
                        ServiceConsultancyID = consultancyUpdate.ServiceConsultancyID,
                        CreateAt = DateTime.Now,
                        CreateBy = userId
                    });
                }
            }
            else
            {
                if (consultancyUpdate.ID != 0)
                {
                    dbo.CustomerRegisterServiceConsultancies.RemoveRange(dbo.CustomerRegisterServiceConsultancies.Where(i => i.ID == consultancyUpdate.ID));
                }
            }
            dbo.SaveChanges();
        }
    }
}
