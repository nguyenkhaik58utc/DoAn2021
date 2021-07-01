using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using iDAS.DA;
using iDAS.Items;
using iDAS.Core;
using iDAS.Base;
using System.Net.Mail;
using System.Web;

namespace iDAS.Services
{
    public class SystemSV
    {
        SystemDA systemDA = new SystemDA();

        #region Core system
        public void SetupSystem()
        {
            //step1: migration database's schema 
            var success = BaseDbContext.Instance.MigrationDbContext<iDASCenterEntities>(BaseDatabase.GetDatabaseCenter());

            if (success)
            {
                //step2: create database for system
                var context = systemDA.Repository;

                //step3: create account administrator for system
                createAdmin(context);

                //step4: save infomation to database
                context.SaveChanges();
            }
        }
        private void createAdmin(iDASCenterEntities context)
        {
            SystemUser admin = new SystemUser();
            admin.Name = "Administrator";
            admin.Account = "Administrator";
            admin.Password = Encryptor.Encrypt("admin@123");
            admin.IsActive = true;
            admin.IsLocked = false;
            admin.IsChangePass = true;
            admin.IsProtected = true;
            admin.CreateAt = DateTime.Now;
            context.SystemUsers.Add(admin);
        }
        public void UpdateSystem(bool doMigration = false)
        {
            var system = new BaseSystem();
            var modules = system.GetAutoModules<ModuleItem>();
            var functions = system.GetAutoFunctions<FunctionItem>(modules);
            var actions = system.GetAutoActions<ActionItem>(functions);
            var success = true;
            if (doMigration)
            {
                success = BaseDbContext.Instance.MigrationDbContext<iDASCenterEntities>(BaseDatabase.GetDatabaseCenter());
            }
            if (success)
            {
                updateModule(modules);
                updateFunction(functions);
                updateAction(actions);
            }
        }
        private void updateModule(List<ModuleItem> modules)
        {
            var moduleSV = new ModuleSV();
            moduleSV.Update(modules);
        }
        private void updateFunction(List<FunctionItem> functions)
        {
            var functionSV = new FunctionSV();
            functionSV.Update(functions);
        }
        private void updateAction(List<ActionItem> actions)
        {
            var actionSV = new ActionSV();
            actionSV.Update(actions);
        }
        public void RegisterSystem()
        {
            var systemCode = BaseSystem.SystemCode;
            var requestUrl = HttpContext.Current.Request.Url;
            var url = requestUrl.AbsoluteUri.ToString().Replace(requestUrl.AbsolutePath.ToString(), string.Empty);
            var database = BaseDatabase.GetDatabaseCenter();
            var system = systemDA.GetQuery().Where(i => i.SystemCode == systemCode).FirstOrDefault();
            if (system != null)
            {
                system.Url = url;
            }
            else
            {
                system = new CenterSystem()
                {
                    SystemCode = systemCode,
                    Name = systemCode,
                    Url = url,
                    IsActive = true,
                    IsDelete = false,
                    DBSource = Encryptor.Encode(database.DataSource),
                    DBUserID = Encryptor.Encode(database.UserId),
                    DBPassword = Encryptor.Encode(database.Password),
                };
                systemDA.Insert(system);
            }
            systemDA.Save();
        }
        public bool UpdateToCenter(string url)
        {
            url = url + "/Core/System/UpdateToCenter";
            var success = WebClientHelper.Request(url);
            return success;
        }
        public void SetupSystem(int customerId, int systemId, int userid)
        {
            var url = GetUrlSystem(systemId);
            url = url + "/Core/System/Setup?customerID={0}";
            url = string.Format(url, customerId);
            var success = WebClientHelper.Request(url);
        }
        public void UpdateSystem(int customerId, int systemId)
        {
            var url = GetUrlSystem(systemId);
            url = url + "/Core/System/Update?customerID={0}";
            url = string.Format(url, customerId);
            var success = WebClientHelper.Request(url);
        }
        #endregion

        public bool SendMail(CustomerSystemItem customer)
        {
            var mailSV = new MailSV();
            string body = "<p><i>Xin chào <b>" + customer.Name
                    + @"</b></i>!</p><p>Chúc mừng bạn đã đăng ký Tài khoản trên <b><a href='www.idasonline.com' target='_blank'>Hệ thống iDASOnline.com</a></b> thành công!</p>
                    <p>Nếu bạn có bất cứ thắc mắc hay phản hồi về Sản phẩm & Dịch vụ của <b>iDASOnline.com</b>,
                    Xin vui lòng gửi cho chúng tôi những thông tin, ý kiến của bạn.</p>
                    <p><b><i>Cảm ơn bạn đã dành thời gian đồng hành cùng chúng tôi và chúc bạn sức khỏe, thành công trong cuộc sống!</i></b></p><p><b>iDAS Team.</b></p>";
            var email = new MailMessage()
            {
                Body = body,
                IsBodyHtml = true,
                Subject = "[iDASOnline.com] Tạo hệ thống thành công!",
            };
            email.To.Add(customer.Email);
            return mailSV.SendMail(email);
        }
        public List<SystemItem> GetAll()
        {
            var systems = systemDA.GetQuery()
                        .Where(i => !i.IsDelete)
                        .Select(i => new SystemItem()
                        {
                            ID = i.ID,
                            Code = i.SystemCode,
                            Name = i.Name,
                            Url = i.Url,
                            IsActive = i.IsActive,
                            DBSource = i.DBSource,
                            Description = i.Description,
                            CreateAt = i.CreateAt,
                            CreateBy = i.CreateBy,
                            UpdateAt = i.UpdateAt,
                            UpdateBy = i.UpdateBy,
                        })
                        .OrderByDescending(i => i.CreateAt)
                        .ToList();
            return systems;
        }
        public List<SystemItem> GetAll(int page, int pageSize, out int totalCount)
        {
            var systems = systemDA.GetQuery()
                        .Where(i => !i.IsDelete)
                        .Select(i => new SystemItem()
                        {
                            ID = i.ID,
                            Code = i.SystemCode,
                            Name = i.Name,
                            Url = i.Url,
                            IsActive = i.IsActive,
                            DBSource = i.DBSource,
                            Description = i.Description,
                            CreateAt = i.CreateAt,
                            CreateBy = i.CreateBy,
                            UpdateAt = i.UpdateAt,
                            UpdateBy = i.UpdateBy
                        })
                        .OrderByDescending(i => i.CreateAt)
                        .Page(page, pageSize, out totalCount)
                        .ToList();
            return systems;
        }
        public string GetUrlSystem(int systemId)
        {
            var url = systemDA.GetQuery()
                        .Where(i => i.ID == systemId)
                        .Select(i => i.Url)
                        .FirstOrDefault();
            return url;
        }
        public MailItem GetMail(string systemCode)
        {
            var mail = systemDA.GetQuery()
                            .Where(i => i.SystemCode == systemCode)
                            .Where(i => i.IsActive)
                            .Where(i => !i.IsDelete)
                            .Select(i => new MailItem()
                            {
                                Host = i.MailHost,
                                Port = i.MailPort,
                                EnableSsl = i.MailEnableSsl,
                                UserName = i.MailUserName,
                                UserPassword = i.MailUserPassword,
                            })
                            .FirstOrDefault();
            return mail;
        }

        public string GetUrlByCode(string systemCode)
        {
            systemCode = systemCode.Trim();
            var modules = systemDA.GetQuery().Where(p => p.SystemCode.Trim().ToUpper().Equals(systemCode.ToUpper())).FirstOrDefault();
            if (modules != null)
                return modules.Url;
            return string.Empty;
        }

        public List<SystemItem> GetAllCbo()
        {

            var lstObj = systemDA.GetQuery()
                        .Select(item => new SystemItem()
                        {
                            ID = item.ID,
                            Code = item.SystemCode,
                            Name = item.Name,
                        })
                        .OrderBy(item => item.Name)

                        .ToList();
            return lstObj;
        }
        public SystemItem GetByID(int id)
        {
            var result = systemDA.GetQuery(i => i.ID == id)
                           .Select(item => new SystemItem
                           {
                               ID = item.ID,
                               Code = item.SystemCode,
                               Name = item.Name,
                               IsActive = item.IsActive,
                               Url = item.Url,
                               DBSource = item.DBSource,
                               DBUserID = item.DBUserID,
                               DBPassword = item.DBPassword,
                               DocumentFolder = item.DocumentFolder,
                               MailHost = item.MailHost,
                               MailPort = item.MailPort,
                               MailEnableSsl = item.MailEnableSsl,
                               MailUserName = item.MailUserName,
                               MailUserPassword = item.MailUserPassword,
                               Description = item.Description,
                               ImageUser = item.ImageUser,
                               ImageLogo = item.ImageLogo,
                               CreateAt = item.CreateAt,
                               CreateBy = item.CreateBy,
                               UpdateAt = item.UpdateAt,
                               UpdateBy = item.UpdateBy
                           }).First();
            return result;
        }
        public SystemItem GetByCode(string systemcode)
        {
            var item = systemDA.GetQuery(i => i.SystemCode.ToUpper() == systemcode.ToUpper()).First();

            if (item != null)
                return convertToCenterSystemItems(item);
            else
                return null;
        }

        public void Update(SystemItem obj)
        {
            var item = systemDA.GetById(obj.ID);
            item.SystemCode = obj.Code;
            item.Name = obj.Name;
            item.Url = obj.Url;
            item.Description = obj.Description;
            item.DBSource = Encryptor.Encode(obj.DBSource);
            item.DBUserID = Encryptor.Encode(obj.DBUserID);
            item.DBPassword = Encryptor.Encode(obj.DBPassword);
            item.DocumentFolder = obj.DocumentFolder;
            if (!string.IsNullOrEmpty(obj.MailHost))
                item.MailHost = Encryptor.Encode(obj.MailHost);
            item.MailPort = obj.MailPort;
            item.MailEnableSsl = obj.MailEnableSsl;
            if (!string.IsNullOrEmpty(obj.MailUserName))
                item.MailUserName = Encryptor.Encode(obj.MailUserName);
            if (!string.IsNullOrEmpty(obj.MailUserPassword))
                item.MailUserPassword = Encryptor.Encode(obj.MailUserPassword);
            if (obj.ImageUser != null)
                item.ImageUser =  obj.ImageUser;
            if (obj.ImageLogo != null)
                item.ImageLogo = obj.ImageLogo;
            item.IsActive = item.IsActive;
            item.UpdateAt = DateTime.Now;
            item.UpdateBy = item.UpdateBy;
            systemDA.Update(item);
            systemDA.Save();
        }


        private SystemItem convertToCenterSystemItems(CenterSystem item)
        {
            try
            {
                SystemItem obj = new SystemItem();
                obj.ID = item.ID;
                obj.Code = item.SystemCode;
                obj.Name = item.Name;
                obj.IsActive = item.IsActive;
                obj.Url = item.Url;
                obj.DBSource = item.DBSource;
                obj.DBUserID = item.DBUserID;
                obj.DBPassword = item.DBPassword;
                obj.Description = item.Description;
                obj.CreateAt = item.CreateAt;
                obj.CreateBy = item.CreateBy;
                obj.UpdateAt = item.UpdateAt;
                obj.UpdateBy = item.UpdateBy;
                return obj;
            }
            catch (Exception)
            {
                throw;
                return null;
            }
        }
    }
}
