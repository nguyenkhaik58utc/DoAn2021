using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;
using iDAS.DA;
using iDAS.Items;
using System.Web;

namespace iDAS.Services
{
    public class CenterSystemSV
    {
        CenterSystemDA systemDA = new CenterSystemDA();

        #region Core system
        public void UpdateSystem()
        {
            var system = new BaseSystem();
            var modules = system.GetAutoModules<CenterModuleItem>();
            var functions = system.GetAutoFunctions<CenterFunctionItem>(modules);
            var Actions = system.GetAutoActions<CenterActionItem>(functions);

            updateModule(modules);
            updateFunction(functions);
            updateAction(Actions);
        }
        private void updateModule(List<CenterModuleItem> modules)
        {
            var moduleSV = new CenterModuleSV();
            moduleSV.Update(modules);
        }
        private void updateFunction(List<CenterFunctionItem> functions)
        {
            var functionSV = new CenterFunctionSV();
            functionSV.Update(functions);
        }
        private void updateAction(List<CenterActionItem> actions)
        {
            var actionSV = new CenterActionSV();
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
        #endregion

        public byte[] GetLogoSystem() { 
            var systemCode = BaseSystem.SystemCode;
            var logo = systemDA.GetQuery()
                        .Where(i => i.SystemCode == systemCode)
                        .Select(i => i.ImageLogo)
                        .FirstOrDefault();
            if (logo == null) {
                logo = new byte[1];
            }
            return logo;
        }
        public byte[] GetAvatarSystem() {
            var systemCode = BaseSystem.SystemCode;
            var avatar = systemDA.GetQuery()
                        .Where(i => i.SystemCode == systemCode)
                        .Select(i => i.ImageUser)
                        .FirstOrDefault();
            if (avatar == null)
            {
                avatar = new byte[1];
            }
            return avatar;
        }
    }
}
