using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;
namespace iDAS.Services
{
   public class ConfigSV
   {

       private ConfigDA configsDA = new ConfigDA();
       public List<ConfigItem> GetAll(int page, int pageSize, out int totalCount)
       {
           var configs = configsDA.GetQuery()
                       .Select(item => new ConfigItem()
                       {
                           ID = item.ID,
                           Name = item.Name,
                           IsActive = item.IsActive,
                           Description = item.Description,
                           Value = item.Value,
                           CreateAt = item.CreateAt,
                           UpdateAt = item.UpdateAt,
                       })
                       .OrderByDescending(item => item.CreateAt)
                       .Page(page, pageSize, out totalCount)
                       .ToList();
           return configs;
       }

       public void Update(ConfigItem item, int userID)
       {
           var config = configsDA.GetById(item.ID);
           config.Name = item.Name;
           config.Value = item.Value;
           config.IsActive = item.IsActive;
           config.Description = item.Description;
           config.UpdateAt = DateTime.Now;
           config.UpdateBy = userID;
           configsDA.Save();
       }
       public bool  CheckNameExits(string name)
       {
           var config = configsDA.GetQuery(m => m.Name.ToUpper().Equals(name.ToUpper())).FirstOrDefault();
           if (config != null)
               return true;
           return false;
       }
       public bool CheckNameEditExits(int id, string name)
       {
           var config = configsDA.GetQuery(m => m.ID!= id && m.Name.ToUpper().Equals(name.ToUpper())).FirstOrDefault();
           if (config!=null)         
               return true;   
           return false;
       }

       public void Insert(ConfigItem item, int userID)
       {
           var group = new SystemConfig()
           {
               Value = item.Value,
               Name = item.Name,
               IsActive = item.IsActive,
               Description = item.Description,
               CreateAt = DateTime.Now,
               CreateBy = userID            
           };
           configsDA.Insert(group);
           configsDA.Save();
       }
       public void Delete(string stringId)
       {
           var configIds = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
           configsDA.DeleteRange(configIds);
           configsDA.Save();
       }
       public ConfigItem GetByID(int id)
       {

           var item = configsDA.GetById(id);
           var config = new ConfigItem();
           config.ID = item.ID;
           config.Value = item.Value;
           config.Name = item.Name;
           config.IsActive = item.IsActive;
           config.Description = item.Description;
           config.CreateAt = item.CreateAt;
           config.CreateByName = item.CreateBy.HasValue?configsDA.Repository.SystemUsers.FirstOrDefault(u => u.ID == item.CreateBy).Name:null;
           config.UpdateAt = item.UpdateAt;
           config.UpdateByName =item.UpdateBy.HasValue?configsDA.Repository.SystemUsers.FirstOrDefault(u => u.ID == item.UpdateBy).Name:null;
           return config;
       }      
   }
}
