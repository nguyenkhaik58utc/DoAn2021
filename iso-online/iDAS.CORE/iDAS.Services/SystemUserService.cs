using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.DAL;
using iDAS.Base;
using iDAS.Core;
using System.Security.Principal;
using iDAS.Items;

namespace iDAS.Services
{
    public class SystemUserService : IUserService
    {
        private SystemUserDA userDA = new SystemUserDA();
        public  SystemUserService() { }
        public SystemUserService(string dbName)
        {
            //var dbContext = BaseDatabase.GetDbContext<iDASDataEntities>(dbName);
            //userDA = new SystemUserDA(dbContext);
        }

        public List<SystemUserItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var users = userDA.GetQuery()
                        .Select(item => new SystemUserItem()
                        {
                            ID = item.ID,
                            Name = item.FullName,
                            Account = item.Account,
                            Email = item.Email,
                            IsActive = item.IsActive,
                            ActiveAt = item.ActiveAt,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .OrderByDescending(item => item.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return users;
        }

        public void Update(SystemUserItem item, int userID)
        {
            var user = userDA.GetById(item.ID);
            user.FullName = item.Name;
            user.Email = item.Email;
            user.Account = item.Account;
            user.Password = string.IsNullOrEmpty(item.Password) ? user.Password : Encryptor.Encrypt(item.Password);
            user.ActiveAt = (user.IsActive != item.IsActive) && (item.IsActive) ? DateTime.Now : new Nullable<DateTime>();
            user.IsActive = item.IsActive;
            user.UpdateAt = DateTime.Now;
            user.UpdateBy = userID;
            userDA.Save();
        }

        public void Insert(SystemUserItem item, int userID)
        {
            var user = new SystemUser()
            {
                FullName = item.Name,
                Account = item.Account,
                Password = Encryptor.Encrypt(item.Password),
                Email = item.Email,
                //IsActive = item.IsActive,
                //ActiveAt = item.IsActive ? DateTime.Now : new Nullable<DateTime>(),
                CreateAt = DateTime.Now,
                CreateBy = userID,
            };
            userDA.Insert(user);
            userDA.Save();
        }

        public void Insert(SystemUserItem item, out int id)
        {
            var user = new SystemUser()
            {
                FullName = item.Name,
                Account = item.Account,
                Password = Encryptor.Encrypt(item.Password),
                Email = item.Email,
                //IsActive = item.IsActive,
                //ActiveAt = item.IsActive ? DateTime.Now : new Nullable<DateTime>(),
                CreateAt = DateTime.Now,
            };
            userDA.Insert(user);
            userDA.Save();
            id = user.ID;
        }

        public void Insert(SystemUserItem item, int userID, out int id)
        {
            var user = new SystemUser()
            {
                FullName = item.Name,
                Account = item.Account,
                Password = Encryptor.Encrypt(item.Password),
                Email = item.Email,
                //IsActive = item.IsActive,
                //ActiveAt = item.IsActive ? DateTime.Now : new Nullable<DateTime>(),
                CreateAt = DateTime.Now,
                CreateBy = userID,
            };
            userDA.Insert(user);
            userDA.Save();
            id = user.ID;
        }

        //public ERegister Register(SystemUserRegisterItem item)
        //{
        //    var check = GetUser(item.Account, Encryptor.Encrypt(item.Password));
        //    ERegister error = ERegister.Fail;
        //    if (check != null)
        //    {
        //        return error = ERegister.AccountExist;
        //    }
        //    var user = new SystemUser()
        //    {
        //        FullName = item.Name,
        //        Account = item.Account,
        //        Password = Encryptor.Encrypt(item.Password),
        //        Email = item.Email,
        //        CreateAt = DateTime.Now,
        //        CreateBy = 0,
        //    };
        //    userDA.Insert(user);
        //    userDA.Save();
        //    error = ERegister.Success;
        //    return error;
        //}

        public void Delete(int id)
        {
            userDA.Delete(id);
            userDA.Save();
        }

        public SystemUser GetUser(string account, string password)
        {
            var user = userDA.GetQuery()
                    .Where(u => u.Account.Contains(account.Trim()))
                    .Where(u => u.Password.Contains(password))
                    .FirstOrDefault(); 
            return user;
        }

        public UserPrincipal GetUserPrincipal(UserPrincipal userPrincipal)
        {
            var user = GetUser(userPrincipal.Account, Encryptor.Encrypt(userPrincipal.Password));
            var newUserPrincipal = new UserPrincipal();

            if (user != null)
            {
                var roleIds = new SystemOrganizationService().GetRoleIds(user.ID);
                var groupIds = new SystemRoleService().GetGroupIds(roleIds);

                newUserPrincipal.ID = user.ID;
                newUserPrincipal.Name = user.FullName;
                newUserPrincipal.Email = user.Email;
                newUserPrincipal.Account = userPrincipal.Account;
                newUserPrincipal.Password = userPrincipal.Password;
                newUserPrincipal.Code = userPrincipal.Code;
                newUserPrincipal.Languague = userPrincipal.Languague;
                newUserPrincipal.Remember = userPrincipal.Remember;
                newUserPrincipal.IsActivated = user.IsActive;
                newUserPrincipal.Administrator = true;
                newUserPrincipal.RoleIDs = roleIds;
                newUserPrincipal.GroupIDs = groupIds;
            }
            return newUserPrincipal;
        }

        public UserPrincipal GetUserPrincipal(IUserLogin userLogin)
        {
            var user = GetUser(userLogin.Account, Encryptor.Encrypt(userLogin.Password));
            var userPrincipal = new UserPrincipal();

            if (user!=null && user.IsActive)
            {
                var roleIds = new SystemOrganizationService().GetRoleIds(user.ID);
                var groupIds = new SystemRoleService().GetGroupIds(roleIds);
                userPrincipal.ID = user.ID;
                userPrincipal.Name = user.FullName;
                userPrincipal.Email = user.Email;
                userPrincipal.Account = userLogin.Account;
                userPrincipal.Password = userLogin.Password;
                userPrincipal.Code = userLogin.Code;
                userPrincipal.Remember = userLogin.Remember;
                userPrincipal.Languague = userLogin.Languague;
                userPrincipal.IsActivated = user.IsActive;
                userPrincipal.Administrator = true;
                userPrincipal.RoleIDs = roleIds;
                userPrincipal.GroupIDs = groupIds;
            }
            return userPrincipal;
        }

        public Database GetDatabaseByCode(string code) 
        {
            Database database = new SystemCustomerService().GetDatabaseByCode(code);
            return database;
        }

        public List<string> GetPermissions(string moduleCode, string functionCode, List<int> roleIds) {
            var actions = new SystemActionService().GetActions(moduleCode, functionCode);
            var actionIds = new SystemPermissionService().GetActionIds(roleIds);
            var permissions = (from a in actions where actionIds.Contains(a.ID) select a.Code).ToList();
            
            return permissions;
        }

        public List<int> GetParentGroupIds(List<int> groupIds) {
            var parentIds = new SystemGroupService().GetParentGroupIDs(groupIds);
            return parentIds;
        }
    }
}
