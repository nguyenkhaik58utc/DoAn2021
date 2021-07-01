using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.Items;

namespace iDAS.Services
{
    public class BusinessSystemSV
    {
        #region Core system
        public bool SetupSystem(int customerID)
        {
            //step1: get customer's information
            CenterCustomerSV customerSV = new CenterCustomerSV();
            var customer = customerSV.GetCustomer(customerID);
            var database = getDatabase(customer);
            //step2: migration database's schema 
            var success = BaseDbContext.Instance.MigrationDbContext<iDASBusinessEntities>(database);
            //step3: create database and setup system
            if (success)
            {
                var context = BaseDbContext.Instance.GetDbContext<iDASBusinessEntities>(database);
                createAdmin(context);
                createRoot(context, customer.Company);
                context.SaveChanges();
            }
            return success;
        }
        private void createAdmin(iDASBusinessEntities dbContext)
        {
            HumanEmployee employee = new HumanEmployee()
            {
                Name = "Administrator",
                Email = "Admin@gmail.com",
                Gender = true,
                IsNew = false,
            };
            dbContext.HumanEmployees.Add(employee);

            var admin = new HumanUser();
            admin.Account = "Administrator";
            admin.Password = Encryptor.Encrypt("admin@123");
            admin.IsActive = true;
            admin.IsLocked = false;
            admin.IsChangePass = true;
            admin.IsProtected = true;
            admin.CreateAt = DateTime.Now;
            admin.HumanEmployeeID = employee.ID;
            dbContext.HumanUsers.Add(admin);
        }
        private void createRoot(iDASBusinessEntities dbContext, string company)
        {
            var department = new HumanDepartment();
            department.Name = string.IsNullOrEmpty(company) ? "Company's name" : company;
            department.ParentID = 0;
            department.IsActive = true;
            department.CreateAt = DateTime.Now;
            dbContext.HumanDepartments.Add(department);
        }
        private Database getDatabase(CustomerSystem customer) {
            var database = new Database()
            {
                DataSource = customer.DBSource,
                DataName = customer.DBName,
                UserId = customer.DBUserID,
                Password = customer.DBPassword,
            };
            return database;
        }
        public bool UpdateSystem(int customerID)
        {
            //step1: get customer's information
            CenterCustomerSV customerSV = new CenterCustomerSV();
            var customer = customerSV.GetCustomer(customerID);
            var database = getDatabase(customer);
            //step2: migration database's schema
            var success = BaseDbContext.Instance.MigrationDbContext<iDASBusinessEntities>(database);
            //step3: update system to customer's system
            if (success)
            {
                var customerModuleSV = new CenterCustomerModuleSV();
                var modules = customerModuleSV.getModules(customerID);
                var functions = customerModuleSV.getFunctions(modules);
                var actions = customerModuleSV.getActions(functions);
                updateModule(modules.ToList(), database);
                updateFunction(functions.ToList(), database);
                updateAction(actions.ToList(), database);
            }
            return success;
        }
        private void updateModule(List<CenterModule> modules, Database database)
        {
            var moduleSV = new BusinessModuleSV(database);
            moduleSV.Update(modules);
        }
        private void updateFunction(List<CenterFunction> functions, Database database)
        {
            var functionSV = new BusinessFunctionSV(database);
            functionSV.Update(functions);
        }
        private void updateAction(List<CenterAction> actions, Database database)
        {
            var actionSV = new BusinessActionSV(database);
            actionSV.Update(actions);
        }
        #endregion
    }
}
