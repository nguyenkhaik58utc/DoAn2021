using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;
using iDAS.DAL.MnCustomer;
using iDAS.Items.MnCustomer;

namespace iDAS.Services.MnCustomer
{
    public class ObjectService
    {
        ObjectDA objectDA = new ObjectDA();

        public List<ObjectItem> GetAllCategories(int campaignId, string code, int page, int pageSize, out int totalCount)
        {
            var dbo = objectDA.SystemContext;
            var objects = dbo.MnCustomerCategories.Where(item=>item.Code == code)
                            .Select(item =>
                                 new ObjectItem()
                                 {
                                     ID = item.MnCustomerObjects.FirstOrDefault(i => i.CampaignID == campaignId).ID,
                                     CampaignID = campaignId,
                                     CategoryID = item.ID,
                                     CategoryName = item.Name,
                                     IsSelected = item.MnCustomerObjects.FirstOrDefault(i => i.CampaignID == campaignId) != null,
                                     CreateAt = item.MnCustomerObjects.FirstOrDefault(i => i.CampaignID == campaignId).CreateAt,
                                     CreateBy = item.MnCustomerObjects.FirstOrDefault(i => i.CampaignID == campaignId).CreateBy,
                                     CreateByName = dbo.SystemUsers.FirstOrDefault(i => i.ID == item.MnCustomerObjects.FirstOrDefault(d => d.CampaignID == campaignId).CreateBy).FullName,
                                 }
                            )
                            .OrderByDescending(item => item.CreateAt)
                            .Page(page, pageSize, out totalCount)
                            .ToList();

            return objects;
        }

        public List<ObjectItem> GetAll(int campaignId, int page, int pageSize, out int totalCount)
        {
            var dbo = objectDA.SystemContext;
            var objects = objectDA.GetQuery().Where(item => item.CampaignID == campaignId)
                            .Select(item => new ObjectItem()
                                 {
                                     ID = item.ID,
                                     CampaignID = item.CampaignID,
                                     CategoryID = item.CategoryID,
                                     CategoryName = dbo.MnCustomerCategories.FirstOrDefault(i => i.ID == item.CategoryID).Name,
                                     CreateAt = item.CreateAt,
                                     CreateBy = item.CreateBy,
                                     CreateByName = dbo.SystemUsers.FirstOrDefault(i => i.ID == item.CreateBy).FullName,
                                 }
                            )
                            .OrderByDescending(item => item.CreateAt)
                            .Page(page, pageSize, out totalCount)
                            .ToList();
            return objects;
        }

        public void Insert(ObjectItem item)
        {
            var _object = new MnCustomerObject()
            {
                CampaignID = item.CampaignID,
                CategoryID = item.CategoryID,
                CreateAt = DateTime.UtcNow,
                CreateBy = item.CreateBy,
            };
            objectDA.Insert(_object);
            objectDA.Save();
        }

        

        public void Delete(int id)
        {
            objectDA.Delete(id);
            objectDA.Save();
        }
    }
}
