using System;
using System.Collections.Generic;
using System.Linq;
using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
using iDAS.Utilities;

namespace iDAS.Services
{
    public class HumanUserSV : IUserService
    {
        private HumanUserDA userDA = new HumanUserDA();

        public List<HumanUserItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var users = userDA.GetQuery()
                        .Select(item => new HumanUserItem()
                        {
                            ID = item.ID,
                            //FullName = item.Name,
                            Account = item.Account,
                            //Email = item.Email,
                            IsActive = item.IsActive,
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
        public List<int> GetEmployeeIDByListUser(List<int> userIds)
        {
            var user = userDA.GetQuery(item => userIds.Contains(item.ID) && item.HumanEmployeeID.HasValue)
                   .Select(t => t.HumanEmployeeID.Value).Distinct().ToList();
            return user;
        }
        public HumanUserItem GetById(int Id)
        {
            var user = userDA.GetQuery(item => item.ID == Id)
                   .Select(item => new HumanUserItem()
                   {
                       ID = item.ID,
                       EmployeeID = item.HumanEmployeeID != null? item.HumanEmployeeID.Value : 0,
                       Account = item.Account,
                       Password = item.Password,
                       IsLocked = item.IsLocked,
                       IsProtected = item.IsProtected,
                       IsActive = item.IsActive,
                       CreateAt = item.CreateAt,
                       CreateBy = item.CreateBy,
                       UpdateAt = item.UpdateAt,
                       UpdateBy = item.UpdateBy,
                   })
                   .First();
            return user;
        }
        /// <summary>
        /// Lấy thông tin tài khoản theo mã nhân viên
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public HumanUserItem GetByEmployeeId(int EmployeeId)
        {
            var user = userDA.GetQuery(item => item.HumanEmployeeID == EmployeeId)
                   .Select(item => new HumanUserItem()
                   {
                       ID = item.ID,
                       Account = item.Account,
                       IsLocked = item.IsLocked,
                       IsProtected = item.IsProtected,
                       IsActive = item.IsActive,
                       CreateAt = item.CreateAt,
                       CreateBy = item.CreateBy,
                       UpdateAt = item.UpdateAt,
                       UpdateBy = item.UpdateBy,
                   })
                   .FirstOrDefault();
            return user;
        }
        public void Update(HumanUserItem item, int userID)
        {
            var user = userDA.GetById(item.ID);
            user.Account = item.Account;
            user.Password = string.IsNullOrEmpty(item.Password) ? user.Password : Encryptor.Encrypt(item.Password);
            user.IsLocked = item.IsLocked;
            user.IsProtected = item.IsProtected;
            user.IsActive = item.IsActive;
            user.UpdateAt = DateTime.Now;
            user.UpdateBy = userID;
            userDA.Save();
        }
        public void ChangePassword(HumanUserItem item, int userID)
        {
            var user = userDA.GetById(item.ID);
            user.Password = string.IsNullOrEmpty(item.PasswordNew) ? user.Password : Encryptor.Encrypt(item.PasswordNew);
            user.UpdateAt = DateTime.Now;
            user.UpdateBy = userID;
            userDA.Save();
        }
        public void Insert(HumanUserItem item, int userID)
        {
            var user = new HumanUser()
            {
                Account = item.Account,
                Password = Encryptor.Encrypt(item.Password),
                IsLocked = item.IsLocked,
                IsProtected = item.IsProtected,
                IsActive = item.IsActive,
                CreateAt = DateTime.Now,
                CreateBy = userID,
                HumanEmployeeID = item.EmployeeID
            };
            userDA.Insert(user);
            userDA.Save();
        }

        //HuongLL bổ sung
        public int Insert(HumanUserItem item)
        {
            var user = new HumanUser()
            {
                Account = item.Account,
                Password = Encryptor.Encrypt(item.Password),
                IsLocked = false,
                IsProtected = item.IsProtected,
                IsActive = false,
                CreateAt = DateTime.Now,
                HumanEmployeeID = item.EmployeeID
            };
            userDA.Insert(user);
            userDA.Save();
            return user.ID;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userID"></param>
        /// <param name="EmployeeId"></param>
        public void InsertFromEmployee(HumanUserItem item, int userID, int EmployeeId)
        {
            var user = new HumanUser()
            {
                Account = item.Account,
                Password = Encryptor.Encrypt(item.Password),
                IsLocked = item.IsLocked,
                IsProtected = item.IsProtected,
                IsActive = item.IsActive,
                CreateAt = DateTime.Now,
                CreateBy = userID,
                HumanEmployeeID = EmployeeId
            };
            userDA.Insert(user);
            userDA.Save();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="id"></param>
        public void Insert(HumanUserItem item, out int id)
        {
            var user = new HumanUser()
            {
                Account = item.Account,
                Password = Encryptor.Encrypt(item.Password),
                //IsActive = item.IsActive,
                CreateAt = DateTime.Now,
            };
            userDA.Insert(user);
            userDA.Save();
            id = user.ID;
        }

        public void Insert(HumanUserItem item, int userID, out int id)
        {
            var user = new HumanUser()
            {
                //Name = item.FullName,
                Account = item.Account,
                Password = Encryptor.Encrypt(item.Password),
                //Email = item.Email,
                //IsActive = item.IsActive,
                CreateAt = DateTime.Now,
                CreateBy = userID,
            };
            userDA.Insert(user);
            userDA.Save();
            id = user.ID;
        }

        public void Delete(int id)
        {
            userDA.Delete(id);
            userDA.Save();
        }

        //HuongLL: get Name by ID
        public String GetNameByUserID(int? id)
        {
            if (id > 0)
            {
                var users = userDA.GetById(id);
                if (users != null)
                    return users.HumanEmployee.Name;
            }
            return "";
        }
        
        public List<int> GetParentGroupIds(List<int> groupIds)
        {
            var parentIds = new DepartmentSV().GetParentGroupIDs(groupIds);
            return parentIds;
        }

        //review 1 - 8/2015
        public HumanUser GetUser(string account, string password)
        {
            var user = userDA.GetQuery()
                    .Where(i => i.Account.Contains(account.Trim()))
                    .Where(i => i.Password.Contains(password))
                    .FirstOrDefault();

            // HungNM. Add errorCode for logging in.
            if (user == null)
            {
                var checkAccount = userDA.GetQuery()
                    .Where(i => i.Account.Contains(account.Trim()))
                    .FirstOrDefault();
                if (checkAccount == null)
                {
                    Ultility.SetErrorCodeLogin("NotActive");
                }
                else
                {
                    Ultility.SetErrorCodeLogin("errorPassword");
                }
            }
            // End.

            return user;
        }
        public UserPrincipal GetUserPrincipal(IUserLogin userLogin)
        {
            var user = GetUser(userLogin.Account, userLogin.Password);
            var userPrincipal = new UserPrincipal();
            if (user != null)
            {
                var roleIds = new HumanOrganizationSV().GetRoleIds(user.HumanEmployeeID);
                var groupIds = new RoleSV().GetDepartmentIDs(roleIds);

                userPrincipal.ID = user.ID;
                userPrincipal.EmployeeID = user.HumanEmployee.ID;
                userPrincipal.Name = user.HumanEmployee != null ? user.HumanEmployee.Name : "Adminstrator";
                userPrincipal.Email = user.HumanEmployee != null ? user.HumanEmployee.Email : string.Empty;
                userPrincipal.Account = user.Account;
                userPrincipal.Password = user.Password;
                userPrincipal.Avatar = user.HumanEmployee != null ? user.HumanEmployee.Avatar : string.Empty;
                userPrincipal.IsActivated = user.IsActive;
                userPrincipal.IsLocked = user.IsLocked;
                userPrincipal.Administrator = user.IsProtected;

                // HungNM. Thiet lap giam doc hoac chu tich hoi dong quan tri luon la co toan quyen nhu Administrator. Dung 1 flag trong bang de xem co la giam doc hoac chu tich
                // hoi dong quan tri khong. 20201026.
                //if (user.ID == 10)    // lenh nay check flag = 1 hay = 0. Bang 1 la chu tich hoi dong quan tri, giam doc, ... -> quyen nhu Administrator.
                //{
                //    userPrincipal.Administrator = true;
                //}
                // End.

                userPrincipal.RoleIDs = roleIds;
                userPrincipal.GroupIDs = groupIds;
                userPrincipal.Code = userLogin.Code;
                userPrincipal.Languague = userLogin.Languague;
                userPrincipal.Remember = userLogin.Remember;
            }
            return userPrincipal;
        }
        public bool CheckPermissions(string moduleCode, string functionCode, string actionCode, List<int> roleIds)
        {
            var permissionSV = new PermissionSV();
            var permissions = permissionSV.CheckPermission(moduleCode, functionCode, actionCode, roleIds);
            // HungNM. Don't check by the old way. 20201207_10h21.
            permissions = true;
            // End.
            return permissions;
        }
        public bool CheckPermissions(string moduleCode, string functionCode, string actionCode, List<int> roleIds, int departmentId)
        {
            var permissionSV = new PermissionSV();
            var permissions = permissionSV.CheckPermission(moduleCode, functionCode, actionCode, roleIds, departmentId);
            // HungNM. Don't check by the old way. 20201207_10h21.
            permissions = true;
            // End.
            return permissions;
        }
        public Database GetDatabaseByCode(string code)
        {
            CenterCustomerSV customerSV = new CenterCustomerSV();
            Database database = customerSV.GetDatabaseByCode(code);
            return database;
        }
        public ELogin Login(IUserLogin userLogin)
        {
            var user = GetUser(userLogin.Account, userLogin.Password);
            if (user == null) return ELogin.ErrorInfo;
            if (!user.IsActive) return ELogin.NotActivated;
            return ELogin.Success;
        }
    }
}
