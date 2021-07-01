using iDAS.Base;
using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using System.Data.SqlClient;

namespace iDAS.Services
{
    public class CustomerSystemSV
    {
        CustomerSystemDA customerSystemDA = new CustomerSystemDA();
        public CustomerSystemItem GetCustomer(int customerId, int systemId)
        {
            var customer = customerSystemDA.GetQuery()
                            .Where(i => i.ID == customerId && i.CenterSystemID == systemId)
                            .Select(i => new CustomerSystemItem()
                            {
                                DBSource = i.DBSource,
                                DBName = i.DBName,
                                DBUserID = i.DBUserID,
                                DBPassword = i.DBPassword,
                                Company = i.Customer.Name,
                            }).FirstOrDefault();
            return customer;
        }
        public List<CustomerSystemItem> GetAll(int page, int pageSize, out int totalCount, int sysID = 0, int updateId = 0)
        {
            var dbo = customerSystemDA.Repository;
            var customers = customerSystemDA.GetQuery(i => i.CenterSystemID == sysID)
                    .Select(item => new CustomerSystemItem()
                    {
                        ID = item.ID,
                        CustomerID = item.CustomerID,
                        Code = item.Code,
                        Name = item.Customer.Name,
                        Company = item.Customer.Name,
                        DataSize = item.DataSize,
                        //DataSizePlus = dbo.CustomerRegisterDataSizes.Where(i => i.CustomerID == item.CustomerID && i.CenterSystemID == item.CenterSystemID).Select(i => i.DataSizePlus).FirstOrDefault(),
                        //DataSizeUsing = item.DataSizeUsing,
                        IsUpdate = item.ID == updateId ? true : false,
                        IsNew = item.IsNew,
                        IsActive = item.IsActive,
                        CreateAt = item.CreateAt,
                    })
                    .OrderByDescending(item => item.CreateAt)
                    .Page(page, pageSize, out totalCount)
                    .ToList();
            return customers;
        }

        // HungNM. Fix error logo of company. 20201001.
        public CustomerSystemItem GetCustomerSystemByCustomerID(int CustomerID)
        {            
            var data = customerSystemDA.GetQuery().Where(p => p.CustomerID == CustomerID).Select(i => new CustomerSystemItem() {ID = i.ID, FileID = i.FileID}).FirstOrDefault();            
            return data;
        }
        // End.

        //Lay Code cua khach hang theo CustomerID
        public string GetCodeByCode(string code)
        {

            code = code.Trim().ToLower();
            var data = customerSystemDA.GetQuery(p => p.Code.Trim().ToLower().Equals(code)).FirstOrDefault();
            if (data != null) return data.Code;
            return "";
        }
        public CustomerSystemItem GetCustomerByID(int id, int sysID)
        {
            var customerSystem = customerSystemDA.GetById(id);
            if (customerSystem != null)
            {
                SystemSV sysSV = new SystemSV();
                var sysInfo = sysSV.GetByID(sysID);
                var obj = new CustomerSystemItem
                {
                    ID = id,
                    SystemID = sysID,
                    //System = sysInfo.Name,
                    Name = customerSystem.Customer.Name,
                    Company = customerSystem.Customer.Name,
                    Email = customerSystem.Customer.Email,
                    Phone = customerSystem.Customer.Phone,
                    IsActive = customerSystem.IsActive,
                    Address = customerSystem.Customer.Address,
                    Code = customerSystem.Code,
                    DBName = customerSystem.DBName,
                    DBSource = customerSystem.DBSource,
                    DBUserID = customerSystem.DBUserID,
                    //Logo = iDAS.Utilities.ConstantPath.UploadImageCustomerLogo + customerSystem.Logo,
                    DBPassword = customerSystem.DBPassword,

                };
                if (customerSystem.DBName == null || customerSystem.DBName.Trim().Equals(""))
                    obj.DBName = customerSystem.Code + "_DAS_" + DateTime.Now.ToString("ddMMyyyy");
                if (customerSystem.DBSource == null || customerSystem.DBSource.Trim().Equals(""))
                    obj.DBSource = sysInfo.DBSource;
                if (customerSystem.DBUserID == null || customerSystem.DBUserID.Trim().Equals(""))
                    obj.DBUserID = sysInfo.DBUserID;
                if (customerSystem.DBPassword == null || customerSystem.DBPassword.Trim().Equals(""))
                    obj.DBPassword = sysInfo.DBPassword;
                return obj;
            }
            return null;
        }
        public Database GetDatabaseCustomer(int customerID)
        {
            var customer = customerSystemDA.GetById(customerID);
            var database = new Database();
            database.DataName = customer.DBName;
            database.DataSource = customer.DBSource;
            database.UserId = customer.DBUserID;
            database.Password = customer.DBPassword;
            return database;
        }

        // HungNM. Fix error logo of company. 20201001.
        public void UpdateLogo(CustomerSystemItem CustomerSystem)
        {
            var customerUpdate = customerSystemDA.GetById(CustomerSystem.ID);
            
            customerUpdate.FileID = CustomerSystem.FileID;
            customerSystemDA.Update(customerUpdate);
            customerSystemDA.Save();
        }
        // End.

        public void Update(CustomerSystemItem CustomerSystem, int p)
        {
            var customerUpdate = customerSystemDA.GetById(CustomerSystem.ID);
            customerUpdate.Code = CustomerSystem.Code;
            customerUpdate.IsNew = CustomerSystem.IsNew;
            customerUpdate.IsActive = CustomerSystem.IsActive;

            // HungNM. Fix error logo of company. 20201001.
            customerUpdate.FileID = CustomerSystem.FileID;
            // End.

            customerSystemDA.Update(customerUpdate);
            customerSystemDA.Save();
        }
        public bool UpdateLogo(int id, int updateBy)
        {
            try
            {
                var dbo = customerSystemDA.Repository;
                var obj = customerSystemDA.GetById(id);
                //obj.LogoFile = dbo.Customers.FirstOrDefault(t => t.ID == obj.Customer.ID).LogoFile;
                obj.UpdateAt = DateTime.Now;
                obj.UpdateBy = updateBy;
                customerSystemDA.Update(obj);
                customerSystemDA.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateDataBase(CustomerSystemItem i, int updateBy)
        {
            try
            {
                var item = customerSystemDA.GetById(i.ID);
                item.Code = i.Code;
                item.DBName = i.DBName;
                item.DBSource = i.DBSource;
                item.DBUserID = i.DBUserID;
                item.DBPassword = i.DBPassword;

                item.UpdateAt = DateTime.Now;
                item.UpdateBy = updateBy;

                customerSystemDA.Update(item);
                customerSystemDA.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void Active(int customerId, int systemId, int userId, bool IsActive = true)
        {
            var customer = customerSystemDA.GetQuery().Where(i => i.ID == customerId && i.CenterSystemID == systemId).FirstOrDefault();
            customer.UpdateAt = DateTime.Now;
            customer.UpdateBy = userId;
            customer.IsActive = IsActive;
            customerSystemDA.Save();
        }
        /// <summary>
        /// Lấy danh sách dịch vụ hê thống theo mã khách hàng hệ thống
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="customerSystemID"></param>
        /// <returns></returns>
        //public List<CustomerSystemServiceItem> GetServiceByCustomerSystemID(int page, int pageSize, out int totalCount, int customerSystemID)
        //{
        //    var dbo = customerSystemDA.Repository;
        //    var customerSystem = dbo.CustomerSystems.FirstOrDefault(i => i.ID == customerSystemID);
        //    //return dbo.CustomerRegisterSystems.Where(i => i.CustomerID == customerSystem.CustomerID
        //    //    //&& i.SystemForm.CenterSystemID == customerSystem.CenterSystemID
        //    //     )
        //    //        .Select(i => new CustomerSystemServiceItem
        //    //        {
        //    //            ID = i.ID,
        //    //            //SystemID = i.SystemForm.CenterSystemID,
        //    //            ISOID = i.ISOStandardID,
        //    //            ISOName = i.ISOStandard.Name,
        //    //            FormID = i.SystemFormID,
        //    //            //FormName = i.SystemForm.Name,
        //    //            //ServiceName = i.SystemForm.Service.Name,
        //    //            CustomerID = i.CustomerID,
        //    //            CustomerName = i.Customer.Name,
        //    //            RequireAt = i.RegisterAt,
        //    //            RegisterAt = i.RegisterAt,
        //    //            IsActive = i.IsActive,
        //    //            IsContact = i.IsContact,
        //    //            IsNew = i.IsNew,
        //    //            IsUse = i.IsUse,
        //    //            IsAccept = i.IsAccept,
        //    //            IsDelete = i.IsDelete,
        //    //        })
        //    //        .OrderByDescending(item => item.RegisterAt)
        //    //        .Page(page, pageSize, out totalCount)
        //    //        .ToList();
        //    totalCount = 0;
        //    return new List<CustomerSystemServiceItem>();
        //}
        public void UpdateSystem(int customerSystemID, int systemId)
        {
            var dbo = customerSystemDA.Repository;
            var customerSystem = dbo.CustomerSystems.Where(i => i.ID == customerSystemID).FirstOrDefault();
            var systemCode = customerSystem.CenterSystem.SystemCode;
            var database = new Database()
            {
                Code = customerSystem.Code,
                DataName = customerSystem.DBName,
                DataSource = customerSystem.DBSource,
                Password = customerSystem.DBPassword,
                UserId = customerSystem.DBUserID,
            };
            new CustomerSystemModuleSV().UpdateModule(customerSystemID);

        }
        /// <summary>
        /// cập nhật module và function của khách hàng
        /// </summary>
        /// <param name="customerSystemID"></param>
        /// <param name="isoId"></param>
        public void UpdateCustomerRegisterSystem(int ServiceId, int customerSystemID, int isoId, int systemId)
        {
            var dbo = customerSystemDA.Repository;
            // Thông tin khách hàng hệ thống
            var customerSystem = dbo.CustomerSystems.Where(i => i.ID == customerSystemID).FirstOrDefault();
            customerSystem.IsActive = true;
            customerSystem.IsNew = false;
            //kích hoạt dịch vụ đăng ký
            //var CustomerRegister = dbo.CustomerRegisterSystems.FirstOrDefault(i => i.ID == ServiceId);
            //CustomerRegister.IsActive = true;
            //CustomerRegister.IsNew = false;
            dbo.SaveChanges();
            //
            var systemCode = customerSystem.CenterSystem.SystemCode;
            var database = new Database()
            {
                Code = customerSystem.Code,
                DataName = customerSystem.DBName,
                DataSource = customerSystem.DBSource,
                Password = customerSystem.DBPassword,
                UserId = customerSystem.DBUserID,
            };
            // Cập nhật module hệ thống của khách hàng
            List<ModuleItem> modules = dbo.ISOStandardModules.Where(i => i.ISOStandardID == isoId && i.CenterModule.SystemCode == systemCode)
                       .Select(item => new ModuleItem()
                       {
                           ID = item.ID,
                           Code = item.CenterModule.Code,
                           Name = item.CenterModule.Name,
                           Position = item.CenterModule.Position,
                           SystemCode = item.CenterModule.SystemCode,
                           Icon = item.CenterModule.Icon,
                           IsActive = item.IsActive,
                           IsShow = item.IsShow,
                           Description = item.CenterModule.Description
                       })
                       .ToList();
            //var moduleSV = new ModuleSV(database);
            //moduleSV.UpdateOrInsert(modules);
            //cập nhật function hệ thống khách hàng
            var functions = dbo.CenterFunctions.Where(i => i.SystemCode == systemCode)
                       .Join(dbo.ISOStandardModules.Where(i => i.ISOStandardID == isoId && i.CenterModule.SystemCode == systemCode), i => i.ModuleCode, j => j.CenterModule.Code,
                       (item, module) =>
                          new FunctionItem()
                          {
                              SystemCode = item.SystemCode,
                              ModuleCode = item.ModuleCode,
                              ParentCode = item.ParentCode,
                              Code = item.Code,
                              Name = item.Name,
                              IsActive = item.IsActive,
                              IsShow = item.IsShow,
                              IsDelete = item.IsDelete,
                              Position = item.Position,
                              Url = item.Url,
                              Icon = item.Icon,
                              IsGroup = item.IsGroup,
                          })
                      .ToList();
            //var functionSV = new FunctionSV(database);
            //functionSV.UpdateOrInsert(functions, systemCode);
            // Cập nhật Action hệ thống khách hàng
            var actions = dbo.CenterActions.Where(i => i.SystemCode == systemCode)
                            .Join(dbo.ISOStandardModules.Where(i => i.ISOStandardID == isoId && i.CenterModule.SystemCode == systemCode), i => i.ModuleCode, j => j.CenterModule.Code,
                                (item, module) =>
                                    new ActionItem()
                                    {
                                        SystemCode = item.SystemCode,
                                        Code = item.Code,
                                        Name = item.Name,
                                        ModuleCode = item.ModuleCode,
                                        FunctionCode = item.FunctionCode,
                                        IsActive = item.IsActive,
                                        IsDelete = item.IsDelete,
                                        Description = item.Description,
                                    }
                            ).ToList();
            //var actionSV = new ActionSV(database);
            //actionSV.Update(actions);
        }

        /// <summary>
        /// Hủy module và function của khách hàng
        /// </summary>
        /// <param name="customerSystemID"></param>
        /// <param name="isoId"></param>
        public void CancelCustomerRegisterSystem(int ServiceId, int customerSystemID, int isoId, int systemId)
        {
            var dbo = customerSystemDA.Repository;
            // Thông tin khách hàng hệ thống
            var customerSystem = dbo.CustomerSystems.Where(i => i.ID == customerSystemID).FirstOrDefault();
            customerSystem.IsNew = false;
            //Hủy dịch vụ đăng ký
            //var CustomerRegister = dbo.CustomerRegisterSystems.FirstOrDefault(i => i.ID == ServiceId);
            //CustomerRegister.IsActive = true;
            //CustomerRegister.IsNew = false;
            dbo.SaveChanges();
            //
            var systemCode = customerSystem.CenterSystem.SystemCode;
            var database = new Database()
            {
                Code = customerSystem.Code,
                DataName = customerSystem.DBName,
                DataSource = customerSystem.DBSource,
                Password = customerSystem.DBPassword,
                UserId = customerSystem.DBUserID,
            };
            //Hủy module hệ thống của khách hàng
            List<ModuleItem> modules = dbo.ISOStandardModules.Where(i => i.ISOStandardID == isoId && i.CenterModule.SystemCode == systemCode)
                       .Select(item => new ModuleItem()
                       {
                           ID = item.ID,
                           Code = item.CenterModule.Code,
                           Name = item.CenterModule.Name,
                           Position = item.CenterModule.Position,
                           SystemCode = item.CenterModule.SystemCode,
                           Icon = item.CenterModule.Icon,
                           IsActive = item.IsActive,
                           IsShow = item.IsShow,
                           Description = item.CenterModule.Description
                       })
                       .ToList();
            //var moduleSV = new ModuleSV(database);
            //moduleSV.Cancel(modules);
            //Hủy function hệ thống khách hàng
            var functions = dbo.CenterFunctions.Where(i => i.SystemCode == systemCode)
                       .Join(dbo.ISOStandardModules.Where(i => i.ISOStandardID == isoId && i.CenterModule.SystemCode == systemCode), i => i.ModuleCode, j => j.CenterModule.Code,
                       (item, module) =>
                          new FunctionItem()
                          {
                              SystemCode = item.SystemCode,
                              ModuleCode = item.ModuleCode,
                              ParentCode = item.ParentCode,
                              Code = item.Code,
                              Name = item.Name,
                              IsActive = item.IsActive,
                              IsShow = item.IsShow,
                              IsDelete = item.IsDelete,
                              Position = item.Position,
                              Url = item.Url,
                              Icon = item.Icon,
                              IsGroup = item.IsGroup,
                          })
                      .ToList();
            //var functionSV = new FunctionSV(database);
            //functionSV.Cancel(functions, systemCode);
            // Hủy Action hệ thống khách hàng
            var actions = dbo.CenterActions.Where(i => i.SystemCode == systemCode)
                            .Join(dbo.ISOStandardModules.Where(i => i.ISOStandardID == isoId && i.CenterModule.SystemCode == systemCode), i => i.ModuleCode, j => j.CenterModule.Code,
                                (item, module) =>
                                    new ActionItem()
                                    {
                                        SystemCode = item.SystemCode,
                                        Code = item.Code,
                                        Name = item.Name,
                                        ModuleCode = item.ModuleCode,
                                        FunctionCode = item.FunctionCode,
                                        IsActive = item.IsActive,
                                        IsDelete = item.IsDelete,
                                        Description = item.Description,
                                    }
                            ).ToList();
            //var actionSV = new ActionSV(database);
            //actionSV.Cancel(actions);
        }

        public byte[] GetLogoFileById(int id)
        {
            //var data = customerSystemDA.GetQuery(i => i.ID == id).Select(i => i.LogoFile).FirstOrDefault();
            //if (data != null)
            //{
            //    return data;
            //}
            return null;
        }

        public void UpdateDataSize(int customerId, int systemId)
        {
            var dbo = customerSystemDA.Repository;
            var regisDataSize = dbo.CustomerRegisterDataSizes.Where(i => i.CustomerID == customerId && i.CenterSystemID == systemId).FirstOrDefault();
            var customerSystem = dbo.CustomerSystems.Where(i => i.CustomerID == customerId && i.CenterSystemID == systemId).FirstOrDefault();
            if (regisDataSize != null)
            {
                //var datasize = regisDataSize.DataSize + regisDataSize.DataSizePlus;
                //customerSystem.DataSize = datasize;
                //regisDataSize.DataSize = datasize;
                //regisDataSize.DataSizePlus = 0;
            }
            dbo.SaveChanges();
        }

        public List<CustomerSystemModuleItem> GetModule(int page, int pageSize, out int totalCount, int systemID, int customerSystemId)
        {
            var dbo = customerSystemDA.Repository;
            var systemCode = dbo.CenterSystems.Where(i => i.ID == systemID).Select(i => i.SystemCode).FirstOrDefault();
            return dbo.CenterModules.Where(i => i.SystemCode == systemCode)
                    .OrderByDescending(item => item.CreateAt)
                    .Page(page, pageSize, out totalCount).ToList()
                    .Select(item => new
                    {
                        CenterModuleID = item.ID,
                        CenterModuleName = item.Name,
                        CustomerModule = dbo.CustomerSystemModules
                                            .Where(i => i.CustomerSystemID == customerSystemId && i.CenterModuleID == item.ID)
                                            .FirstOrDefault()
                    })
                    .Select(
                        item => new CustomerSystemModuleItem()
                        {
                            ID = item.CustomerModule != null ? item.CustomerModule.ID : 0,
                            CenterModuleID = item.CenterModuleID,
                            CenterModuleName = item.CenterModuleName,
                            IsSelect = item.CustomerModule != null
                        }
                    ).ToList();
        }

        public void UpdateModule(CustomerSystemModuleItem moduleItem, int userId)
        {
            var dbo = customerSystemDA.Repository;

            // HungNM. Fix error module registration. 20200923.
            var dboCustomerSystemItem = new CustomerSystemItem();
            int isActive = 0;
            //string DBname = dbo.CustomerSystems.Where(i => i.ID == moduleItem.CustomerSystemID).Select(i => i.DBName).FirstOrDefault();
            dboCustomerSystemItem = customerSystemDA.GetQuery(i => i.ID == moduleItem.CustomerSystemID).Select(item => new CustomerSystemItem()
            {
                DBName = item.DBName,
                DBSource = item.DBSource,
                DBUserID = item.DBUserID,
                DBPassword = item.DBPassword
            }).FirstOrDefault();
            string CenterModuleCode = dbo.CenterModules.Where(i => i.ID == moduleItem.CenterModuleID).Select(i => i.Code).FirstOrDefault();
            // End.
            
            if (moduleItem.IsSelect)
            {
                if (moduleItem.ID == 0)
                {
                    dbo.CustomerSystemModules.Add(new CustomerSystemModule()
                    {
                        CenterModuleID = moduleItem.CenterModuleID,
                        CustomerSystemID = moduleItem.CustomerSystemID,
                        CreateAt = DateTime.Now,
                        CreateBy = userId
                    });

                    // HungNM. Fix error module registration. 20200923.
                    isActive = 1;
                    // End.

                }
            }
            else
            {
                if (moduleItem.ID != 0)
                {
                    dbo.CustomerSystemModules.RemoveRange(dbo.CustomerSystemModules.Where(i => i.ID == moduleItem.ID));
                }
            }
            dbo.SaveChanges();

            // HungNM. Fix error module registration. 20200923.
            string _ConnectionString = "data source={0};initial catalog={1};persist security info=True;user id={2};password={3};multipleactiveresultsets=True";
            var connectionString = string.Format(_ConnectionString, dboCustomerSystemItem.DBSource, dboCustomerSystemItem.DBName, dboCustomerSystemItem.DBUserID, dboCustomerSystemItem.DBPassword);
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand("", connection))
            {
                command.Connection = connection;
                command.CommandText = "Update [" + dboCustomerSystemItem.DBName + "].dbo.BusinessModules SET isActive = " + isActive.ToString() + " WHERE Code = N'" + CenterModuleCode + "'";
                //command.Parameters.AddWithValue("@isActive", isActive);
                //command.Parameters.AddWithValue("@CenterModuleCode", CenterModuleCode);
                connection.Open();
                command.ExecuteNonQuery();
            }
            // End.

        }
    }
}
