using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    //public class HumanCriteriaFactorySV
    //{
    //    HumanAuditCriteriaFactoryDA organizationDA = new HumanAuditCriteriaFactoryDA();
    //    public List<HumanAuditCriteriaFactoryItem> GetAllByRoleID(ModelFilter filter, int roleID)
    //    {
    //        var dbo = organizationDA.Repository;
    //        var lst = organizationDA.GetQuery(x=>x.HumanRoleID == roleID).Select(i => new HumanAuditCriteriaFactoryItem()
    //        {
    //            ID = i.ID,
    //            HumanRoleID = i.HumanRoleID,
    //            HumanAuditCriteriaID = i.HumanAuditCriteriaID,
    //            Factory = i.Factory,
    //            HumanAuditCriteriaName = i.HumanAuditCriteria.Name,
    //            HumanAuditCriteriaGroupName = i.HumanAuditCriteria.HumanAuditCriteriaCategory.Name
    //        }).OrderByDescending(item => item.ID)
    //                    .Filter(filter)
    //                    .ToList();
    //        return lst;
    //    }
        
    //    public bool CheckExist(int roleID, int criID)
    //    {
    //        var data = organizationDA.GetQuery(x => x.HumanRoleID == roleID && x.HumanAuditCriteriaID == criID);
    //        if (data != null && data.Count() > 0)
    //            return true;
    //        else
    //            return false;
    //    }

    //    public void Insert(HumanAuditCriteriaFactoryItem data)
    //    {
    //        HumanAuditCriteriaFactory obj = new HumanAuditCriteriaFactory();
    //        if(data!=null)
    //        {
    //            obj.HumanRoleID = data.HumanRoleID;
    //            obj.HumanAuditCriteriaID = data.HumanAuditCriteriaID;
    //            obj.Factory = data.Factory;
    //            organizationDA.Insert(obj);
    //            organizationDA.Save();
    //        }
    //    }
    //    public void Delete(string stringId)
    //    {
    //        var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
    //        organizationDA.DeleteRange(ids);
    //        organizationDA.Save();
    //    }
    //    public bool Delete(int id)
    //    {
    //        var dbo = organizationDA.Repository;
    //        var obj = organizationDA.GetById(id);
    //        organizationDA.Delete(obj);
    //        organizationDA.Save();
    //        return true;
    //    }

    //    public void UpdateFactory(int id, int factory)
    //    {
    //        var obj = organizationDA.GetById(id);
    //        if (obj != null)
    //            obj.Factory = factory;
    //        organizationDA.Save();
    //    }

    //    public void UpdateFactory(int roleId, int critId, int factory)
    //    {
    //        HumanAuditCriteriaFactory obj = organizationDA.GetQuery(x => x.HumanRoleID == roleId && x.HumanAuditCriteriaID == critId).FirstOrDefault();
    //        if (obj != null)
    //            obj.Factory = factory;
    //        else
    //        {
    //            obj = new HumanAuditCriteriaFactory();
    //            obj.HumanRoleID = roleId;
    //            obj.HumanAuditCriteriaID = critId;
    //            obj.Factory = factory;
    //            organizationDA.Insert(obj);
    //        }
    //        organizationDA.Save();
    //    }
    //}
}
