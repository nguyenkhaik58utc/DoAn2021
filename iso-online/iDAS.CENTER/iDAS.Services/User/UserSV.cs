using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.DA;
using iDAS.Base;
using iDAS.Core;
using System.Security.Principal;
using iDAS.Items;

namespace iDAS.Services
{
    public class UserSV: IUserService
    {
        private UserDA userDA = new UserDA();

        #region Core system
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
            UserPrincipal newUserPrincipal = null;
            if (user != null)
            {
                newUserPrincipal = new UserPrincipal();
                {
                    var roleIds = new OrganizationSV().GetRoleIds(user.ID);
                    //var groupIds = new RoleSV().GetGroupIds(roleIds);

                    newUserPrincipal.ID = user.ID;
                    newUserPrincipal.Name = user.Name;
                    newUserPrincipal.Email = user.Email;
                    newUserPrincipal.Account = userPrincipal.Account;
                    newUserPrincipal.Password = userPrincipal.Password;
                    newUserPrincipal.Code = userPrincipal.Code;
                    newUserPrincipal.Languague = userPrincipal.Languague;
                    newUserPrincipal.Remember = userPrincipal.Remember;
                    newUserPrincipal.IsActivated = user.IsActive;
                    newUserPrincipal.Administrator = user.IsProtected;
                    //newUserPrincipal.RoleIDs = roleIds;
                    //newUserPrincipal.GroupIDs = groupIds;
                }
            }
            return newUserPrincipal;
        }
        public UserPrincipal GetUserPrincipal(IUserLogin userLogin)
        {
            var user = GetUser(userLogin.Account, Encryptor.Encrypt(userLogin.Password));
            UserPrincipal userPrincipal = null;
            if (user != null)
            {
                userPrincipal = new UserPrincipal();
                if (user.IsActive)
                {
                    var roleIds = new OrganizationSV().GetRoleIds(user.ID);
                    //var groupIds = new RoleSV().GetGroupIds(roleIds);
                    userPrincipal.ID = user.ID;
                    userPrincipal.Name = user.Name;
                    userPrincipal.Email = user.Email;
                    userPrincipal.Account = userLogin.Account;
                    userPrincipal.Password = userLogin.Password;
                    userPrincipal.Code = userLogin.Code;
                    userPrincipal.Remember = userLogin.Remember;
                    userPrincipal.Languague = userLogin.Languague;
                    userPrincipal.IsActivated = user.IsActive;
                    userPrincipal.Administrator = user.IsProtected;
                    //userPrincipal.RoleIDs = roleIds;
                    //userPrincipal.GroupIDs = groupIds;
                }
            }
            return userPrincipal;
        }
        public Database GetDatabaseByCode(string code)
        {
            return null;
        }
        public bool CheckPermissions(string moduleCode, string functionCode, string actionCode, List<int> roleIds)
        {
            //var permissionSV = new PermissionSV();
            //var permissions = permissionSV.CheckPermission(moduleCode, functionCode, actionCode, roleIds);
            //return permissions;
            return true;
        }
        public bool CheckPermissions(string moduleCode, string functionCode, string actionCode, List<int> roleIds, int departmentId)
        {
            //var permissionSV = new PermissionSV();
            //var permissions = permissionSV.CheckPermission(moduleCode, functionCode, actionCode, roleIds);
            //return permissions;
            return true;
        }
        #endregion

        public List<UserItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var users = userDA.GetQuery()
                        .Select(item => new UserItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
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

        public String GetNameByID(int? id)
        {
            if(id> 0)
            {
                  var users = userDA.GetById(id);
                if(users != null)
                    return users.Name;
            }
            return "";
        }

        public void Update(UserItem item, int userID)
        {
            var user = userDA.GetById(item.ID);
            user.Name = item.Name;
            user.Email = item.Email;
            user.Account = item.Account;
            user.Password = string.IsNullOrEmpty(item.Password) ? user.Password : Encryptor.Encrypt(item.Password);
            user.ActiveAt = (user.IsActive != item.IsActive) && (item.IsActive) ? DateTime.Now : new Nullable<DateTime>();
            user.IsActive = item.IsActive;
            user.UpdateAt = DateTime.Now;
            user.UpdateBy = userID;
            userDA.Save();
        }

        public void Insert(UserItem item, int userID)
        {
            var user = new SystemUser()
            {
                Name = item.Name,
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

        public void Insert(UserItem item, out int id)
        {
            var user = new SystemUser()
            {
                Name = item.Name,
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

        public void Insert(UserItem item, int userID, out int id)
        {
            var user = new SystemUser()
            {
                Name = item.Name,
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

        //public ERegister Register(IUserRegister item)
        //{
        //    var check = GetUser(item.Account, Encryptor.Encrypt(item.Password));
        //    ERegister error = ERegister.Fail;
        //    if (check != null)
        //    {
        //        return error = ERegister.AccountExist;
        //    }
        //    var user = new SystemUser()
        //    {
        //        Name = item.Name,
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

        public List<int> GetParentGroupIds(List<int> groupIds)
        {
            var parentIds = new DepartmentSV().GetParentGroupIDs(groupIds);
            return parentIds;
        }

        /// <summary>
        /// Thanhnv 08/12/2014
        /// Lấy thông tin thành viên theo ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public UserItem GetById(int Id)
        {
            try
            {
                var user = userDA.GetQuery(u => u.ID == Id)
                        .Select(item => new UserItem()
                        {
                            ID = item.ID,
                            Name = item.Name,
                            Account = item.Account,
                            Email = item.Email,
                            IsActive = item.IsActive,
                            ActiveAt = item.ActiveAt,
                            CreateAt = item.CreateAt,
                            CreateBy = item.CreateBy,
                            UpdateAt = item.UpdateAt,
                            UpdateBy = item.UpdateBy,
                        })
                        .First();
                return user;
            }
            catch
            {
                return null;
            }
        }
    }
}
