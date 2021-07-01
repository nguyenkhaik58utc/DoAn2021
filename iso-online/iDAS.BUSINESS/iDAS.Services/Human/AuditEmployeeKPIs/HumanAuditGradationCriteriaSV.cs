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
    public class HumanAuditGradationCriteriaSV
    {
        private HumanAuditGradationCriteriaDA humanAuditGradationCriteriaDA = new HumanAuditGradationCriteriaDA();
        public bool CheckCriterialExit(int humanAuditGradationId)
        {
            var dbo = humanAuditGradationCriteriaDA.Repository;
            var roleAudits = dbo.HumanAuditDepartments.Where(t => t.HumanAuditGradationID == humanAuditGradationId)
                .SelectMany(t => t.HumanAuditGradationRoles.Where(x => x.IsAudit).Select(x => x.ID))
                .ToList();
            foreach (var item in roleAudits)
            {
                if (dbo.HumanAuditGradationCriterias.Where(t => t.HumanAuditGradationRoleID == item).FirstOrDefault() == null)
                    return false;
            }
            return true;
        }
        public HumanAuditGradationCriteriaItem GetById(int id)
        {
            var result = humanAuditGradationCriteriaDA.GetQuery(t => t.ID == id)
                .Select(i => new HumanAuditGradationCriteriaItem()
                {
                    ID = i.ID,
                    Name = i.Name,
                    CreateAt = i.CreateAt,
                    Note = i.Note,
                    Factory = i.Factory,
                    HumanAuditGradationRoleID = i.HumanAuditGradationRoleID,
                    HumanAuditCriteriaCategoryName = i.HumanAuditCriteriaCategoryName,
                    CreateBy = i.CreateBy,
                    UpdateBy = i.UpdateBy,
                    UpdateAt = i.UpdateAt,
                }
                ).FirstOrDefault();
            return result;
        }
        public int Insert(HumanAuditGradationCriteriaItem objNew, int userId, List<HumanAuditGraditionCriteriaPointItem> lstPoint)
        {
            var dbo = humanAuditGradationCriteriaDA.Repository;
            HumanAuditGradationCriteria obj = new HumanAuditGradationCriteria();
            if (dbo.HumanAuditGradationCriterias.FirstOrDefault(t => t.Name.ToUpper() == objNew.Name.ToUpper() && t.HumanAuditGradationRoleID == objNew.HumanAuditGradationRoleID) != null)
            {
                return 0;
            }
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            obj.Factory = objNew.Factory;
            obj.Note = objNew.Note;
            obj.Name = objNew.Name;
            obj.HumanAuditCriteriaCategoryName = objNew.HumanAuditCriteriaCategoryName.Trim();
            obj.HumanAuditGradationRoleID = objNew.HumanAuditGradationRoleID;
            dbo.HumanAuditGradationCriterias.Add(obj);
            dbo.SaveChanges();
            foreach (var item in lstPoint)
            {
                HumanAuditGradationCriteriaPoint objPoint = new HumanAuditGradationCriteriaPoint();
                objPoint.CreateAt = DateTime.Now;
                objPoint.CreateBy = userId;
                objPoint.Name = item.Name;
                objPoint.Point = item.Point;
                objPoint.Note = item.Note;
                objPoint.HumanAuditGradationCriteriaID = obj.ID;
                dbo.HumanAuditGradationCriteriaPoints.Add(objPoint);
                dbo.SaveChanges();
            }
            return obj.ID;
        }
        public bool Update(HumanAuditGradationCriteriaItem objEdit, int userId, List<HumanAuditGraditionCriteriaPointItem> lstPoint)
        {
            var dbo = humanAuditGradationCriteriaDA.Repository;
            HumanAuditGradationCriteria obj = dbo.HumanAuditGradationCriterias.FirstOrDefault(i => i.ID == objEdit.ID);
            if (dbo.HumanAuditGradationCriterias.FirstOrDefault(t => t.Name.ToUpper() == objEdit.Name.ToUpper()) != null
                 && (obj.Name.ToUpper() != objEdit.Name.ToUpper()))
            {
                return false;
            }
            obj.Name = objEdit.Name;
            obj.HumanAuditCriteriaCategoryName = objEdit.HumanAuditCriteriaCategoryName;
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            obj.Factory = objEdit.Factory;
            obj.Note = objEdit.Note;
            dbo.SaveChanges();
            var arrIdPoint = lstPoint.Select(t => t.ID).ToList();
            var s = dbo.HumanAuditGradationCriteriaPoints.Where(t => t.HumanAuditGradationCriteriaID == objEdit.ID && !arrIdPoint.Contains(t.ID)).ToList();
            dbo.HumanAuditGradationCriteriaPoints.RemoveRange(s);
            dbo.SaveChanges();
            foreach (var item in lstPoint)
            {
                if (item.ID == 0)
                {
                    HumanAuditGradationCriteriaPoint objPoint = new HumanAuditGradationCriteriaPoint();
                    objPoint.CreateAt = DateTime.Now;
                    objPoint.CreateBy = userId;
                    objPoint.Name = item.Name;
                    objPoint.Point = item.Point;
                    objPoint.Note = item.Note;
                    objPoint.HumanAuditGradationCriteriaID = objEdit.ID;
                    dbo.HumanAuditGradationCriteriaPoints.Add(objPoint);
                    dbo.SaveChanges();
                }
                else
                {
                    var objEditPoint = dbo.HumanAuditGradationCriteriaPoints.Where(t => t.ID == item.ID).FirstOrDefault();
                    objEditPoint.UpdateAt = DateTime.Now;
                    objEditPoint.UpdateBy = userId;
                    objEditPoint.Name = item.Name;
                    objEditPoint.Point = item.Point;
                    objEditPoint.Note = item.Note;
                    objEditPoint.HumanAuditGradationCriteriaID = objEdit.ID;
                    dbo.SaveChanges();
                }
            }
            return true;
        }
        public List<HumanAuditGraditionCriteriaPointItem> GetByCateId(ModelFilter filter, int CriteriaId)
        {
            var dbo = humanAuditGradationCriteriaDA.Repository;
            var criterias = dbo.HumanAuditGradationCriteriaPoints.Where(t => t.HumanAuditGradationCriteriaID == CriteriaId)
                          .Select(item => new HumanAuditGraditionCriteriaPointItem()
                          {
                              ID = item.ID,
                              Point = item.Point,
                              HumanAuditGradationCriteriaID = item.HumanAuditGradationCriteriaID,
                              Note = item.Note,
                              Name = item.Name,
                              CreateAt = item.CreateAt
                          })
                          .Filter(filter)
                          .OrderByDescending(t => t.Point)
                          .ToList();
            return criterias;
        }
        public List<HumanAuditGradationCriteriaItem> GetAll(ModelFilter filter, int humanAuditGradationRoleId = 0)
        {
            var dbo = humanAuditGradationCriteriaDA.Repository;
            var gradationCriterias = dbo.HumanAuditGradationCriterias.Where(t => t.HumanAuditGradationRoleID == humanAuditGradationRoleId)
                 .Select(item => new HumanAuditGradationCriteriaItem()
                 {
                     ID = item.ID,
                     Factory = item.Factory,
                     Name = item.Name,
                     HumanAuditCriteriaCategoryName = item.HumanAuditCriteriaCategoryName,
                     Note = item.Note,
                     UpdateAt = item.UpdateAt,
                     UpdateBy = item.UpdateBy,
                     CreateAt = item.CreateAt
                 })
                 .Filter(filter)
                 .ToList();
            return gradationCriterias;
        }
        public void CoppyData(int humanAuditGradationRoleId, string stringCateId, int userId)
        {
            var dbo = humanAuditGradationCriteriaDA.Repository;
            var ids = stringCateId.Split(',').Select(n => int.Parse(n)).ToList();
            var data = dbo.HumanAuditCriterias.Where(t => ids.Contains(t.HumanAuditCriteriaCategoryID)).ToList();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    if (dbo.HumanAuditGradationCriterias.Where(t => t.Name.Trim().ToUpper() == item.Name.Trim().ToUpper() && t.HumanAuditGradationRoleID == humanAuditGradationRoleId).FirstOrDefault() == null)
                    {
                        HumanAuditGradationCriteria obj = new HumanAuditGradationCriteria();
                        obj.Name = item.Name;
                        obj.Note = item.Note;
                        obj.HumanAuditGradationRoleID = humanAuditGradationRoleId;
                        obj.HumanAuditCriteriaCategoryName = item.HumanAuditCriteriaCategory.Name;
                        obj.Factory = item.Factory;
                        obj.CreateAt = DateTime.Now;
                        obj.CreateBy = userId;
                        dbo.HumanAuditGradationCriterias.Add(obj);
                        dbo.SaveChanges();
                        var points = dbo.HumanAuditCriteriaPoints.Where(t => t.HumanAuditCriteriaID == item.ID).ToList();
                        if (points.Count > 0)
                        {
                            foreach (var point in points)
                            {
                                HumanAuditGradationCriteriaPoint objPoint = new HumanAuditGradationCriteriaPoint();
                                objPoint.HumanAuditGradationCriteriaID = obj.ID;
                                objPoint.Name = point.Name;
                                objPoint.Note = point.Note;
                                objPoint.Point = point.Point;
                                objPoint.CreateAt = DateTime.Now;
                                objPoint.CreateBy = userId;
                                dbo.HumanAuditGradationCriteriaPoints.Add(objPoint);
                                dbo.SaveChanges();
                            }
                        }
                    }
                }
            }
        }
        public void Delete(int id)
        {
            var dbo = humanAuditGradationCriteriaDA.Repository;
            var s = dbo.HumanAuditGradationCriteriaPoints.Where(t => t.HumanAuditGradationCriteriaID == id).ToList();
            dbo.HumanAuditGradationCriteriaPoints.RemoveRange(s);
            dbo.SaveChanges();
            humanAuditGradationCriteriaDA.Delete(id);
            humanAuditGradationCriteriaDA.Save();
        }
    }
}
