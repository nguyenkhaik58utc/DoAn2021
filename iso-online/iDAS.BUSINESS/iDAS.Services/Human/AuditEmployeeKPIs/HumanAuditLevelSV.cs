using iDAS.Base;
using iDAS.Core;
using iDAS.DA;
using iDAS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDAS.Services
{
    public class HumanAuditLevelSV
    {
        private HumanAuditLevelDA levelDA = new HumanAuditLevelDA();
        public List<HumanAuditLevelItem> GetByCateId(ModelFilter filter, int cateId)
        {
            var dbo = levelDA.Repository;
            var criterias = dbo.HumanAuditLevels.Where(t => t.HumanAuditGradationRoleID == cateId)
                          .Select(item => new HumanAuditLevelItem()
                          {
                              ID = item.ID,
                              //HumanAuditLevelCategoryID = item.HumanAuditLevelCategoryID,
                              //CategoryName = item.HumanAuditLevelCategory.Name,
                              Name = item.Name,
                              MinPoint = item.MinPoint,
                              MaxPoint = item.MaxPoint,
                              Note = item.Note,
                              CreateAt = item.CreateAt
                          })
                          .Filter(filter)
                          .ToList();
            return criterias;
        }
        public List<ComboboxItem> GetCombobox(int humanAuditDepartmentId, int humanRoleId)
        {
            var dbo = levelDA.Repository;
            var idCateRole = dbo.HumanAuditGradationRoles.Where(t => t.HumanAuditDepartmentID == humanAuditDepartmentId && t.HumanRoleID == humanRoleId)
                .FirstOrDefault() != null ? dbo.HumanAuditGradationRoles.Where(t => t.HumanAuditDepartmentID == humanAuditDepartmentId && t.HumanRoleID == humanRoleId)
                .FirstOrDefault().ID : 0;
            var criterias = dbo.HumanAuditLevels.Where(t => t.HumanAuditGradationRoleID == idCateRole)
                          .Select(item => new ComboboxItem()
                          {
                              ID = item.ID,
                              Name = item.Name,
                              PointFrom = item.MinPoint.HasValue ? item.MinPoint.Value : 0,
                              PointTo = item.MaxPoint.HasValue ? item.MaxPoint.Value : 0
                          })
                          .ToList();
            return criterias;
        }
        public HumanAuditLevelItem GetById(int id)
        {
            var result = levelDA.GetQuery(t => t.ID == id)
                .Select(i => new HumanAuditLevelItem()
                {
                    ID = i.ID,
                    Name = i.Name,
                    CreateAt = i.CreateAt,
                    Note = i.Note,
                    MinPoint = i.MinPoint,
                    MaxPoint = i.MaxPoint,
                    //HumanAuditLevelCategoryID = i.HumanAuditLevelCategory.ID,
                    //CategoryName = i.HumanAuditLevelCategory.Name,
                    CreateBy = i.CreateBy,
                    UpdateBy = i.UpdateBy,
                    UpdateAt = i.UpdateAt,
                }
                ).FirstOrDefault();
            return result;
        }
        public int Insert(HumanAuditLevelItem objNew, int userId)
        {
            var dbo = levelDA.Repository;
            HumanAuditLevel obj = new HumanAuditLevel();
            if (dbo.HumanAuditLevels.FirstOrDefault(t => t.Name.ToUpper() == objNew.Name.ToUpper() && t.HumanAuditGradationRoleID == objNew.HumanAuditGradationRoleID) != null)
            {
                return 0;
            }
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            obj.Note = objNew.Note;
            obj.Name = objNew.Name;
            obj.MinPoint = objNew.MinPoint;
            obj.MaxPoint = objNew.MaxPoint;
            obj.HumanAuditGradationRoleID = objNew.HumanAuditGradationRoleID;
            dbo.HumanAuditLevels.Add(obj);
            dbo.SaveChanges();
            return obj.ID;
        }
        public void Delete(string stringId)
        {
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            foreach (var item in ids)
            {
                levelDA.Delete(item);
            }
            levelDA.Save();
        }
        public void Delete(int id)
        {
            var dbo = levelDA.Repository;
            levelDA.Delete(id);
            levelDA.Save();
        }
        public bool Update(HumanAuditLevelItem objEdit, int userId)
        {
            var dbo = levelDA.Repository;
            HumanAuditLevel obj = dbo.HumanAuditLevels.FirstOrDefault(i => i.ID == objEdit.ID);
            if (dbo.HumanAuditLevels.FirstOrDefault(t => t.Name.ToUpper() == objEdit.Name.ToUpper()) != null
                 && (obj.Name.ToUpper() != objEdit.Name.ToUpper()))
            {
                return false;
            }
            obj.Name = objEdit.Name;
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            obj.MinPoint = objEdit.MinPoint;
            obj.MaxPoint = objEdit.MaxPoint;
            obj.Note = objEdit.Note;
            dbo.SaveChanges();
            return true;
        }
    }
}
