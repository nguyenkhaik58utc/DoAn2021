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
    public class HumanCriteriaSV
    {
        private HumanAuditCriteriaDA criteriaDA = new HumanAuditCriteriaDA();
        public List<HumanAuditCriteriaItem> GetByCateId(ModelFilter filter, int cateId)
        {
            var dbo = criteriaDA.Repository;
            var criterias = dbo.HumanAuditCriterias.Where(t => t.HumanAuditCriteriaCategoryID == cateId)
                          .Select(item => new HumanAuditCriteriaItem()
                          {
                              ID = item.ID,
                              HumanAuditCriteriaCategoryID = item.HumanAuditCriteriaCategoryID,
                              CategoryName = item.HumanAuditCriteriaCategory.Name,
                              Factory = item.Factory,
                              Name = item.Name,
                              CreateAt = item.CreateAt
                          })
                          .Filter(filter)
                          .ToList();
            return criterias;
        }
        public List<HumanAuditCriteriaItem> GetByListCateId(ModelFilter filter, string lstCateId)
        {
            List<int> lstCateID = new List<int>();
            foreach (string s in lstCateId.Split(','))
            {
                int result = 0;
                if (int.TryParse(s, out result))
                    lstCateID.Add(result);
            }
            var dbo = criteriaDA.Repository;
            var criterias = dbo.HumanAuditCriterias.Where(t => lstCateID.Contains(t.HumanAuditCriteriaCategoryID))
                          .Select(item => new HumanAuditCriteriaItem()
                          {
                              ID = item.ID,
                              HumanAuditCriteriaCategoryID = item.HumanAuditCriteriaCategoryID,
                              CategoryName = item.HumanAuditCriteriaCategory.Name,
                              Name = item.Name,
                              CreateAt = item.CreateAt
                          })
                          .Filter(filter)
                          .ToList();
            return criterias;
        }
        public HumanAuditCriteriaItem GetById(int id)
        {
            var result = criteriaDA.GetQuery(t => t.ID == id)
                .Select(i => new HumanAuditCriteriaItem()
                {
                    ID = i.ID,
                    Name = i.Name,
                    CreateAt = i.CreateAt,
                    Note = i.Note,
                    Factory = i.Factory,
                    HumanAuditCriteriaCategoryID = i.HumanAuditCriteriaCategory.ID,
                    CategoryName = i.HumanAuditCriteriaCategory.Name,
                    CreateBy = i.CreateBy,
                    UpdateBy = i.UpdateBy,
                    UpdateAt = i.UpdateAt,
                }
                ).FirstOrDefault();
            return result;
        }
        public int Insert(HumanAuditCriteriaItem objNew, int userId, List<HumanAuditGradationCriteriaPointItem> lstPoint)
        {
            var dbo = criteriaDA.Repository;
            HumanAuditCriteria obj = new HumanAuditCriteria();
            if (dbo.HumanAuditCriterias.FirstOrDefault(t => t.Name.ToUpper() == objNew.Name.ToUpper() && t.HumanAuditCriteriaCategoryID == objNew.HumanAuditCriteriaCategoryID) != null)
            {
                return 0;
            }
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            obj.Factory = objNew.Factory.Value;
            obj.Note = objNew.Note;
            obj.Name = objNew.Name;
            obj.HumanAuditCriteriaCategoryID = objNew.HumanAuditCriteriaCategoryID;
            dbo.HumanAuditCriterias.Add(obj);
            dbo.SaveChanges();
            foreach (var item in lstPoint)
            {
                HumanAuditCriteriaPoint objPoint = new HumanAuditCriteriaPoint();
                objPoint.CreateAt = DateTime.Now;
                objPoint.CreateBy = userId;
                objPoint.Name = item.Name;
                objPoint.Point = item.Point;
                objPoint.Note = item.Note;
                objPoint.HumanAuditCriteriaID = obj.ID;
                dbo.HumanAuditCriteriaPoints.Add(objPoint);
                dbo.SaveChanges();
            }
            return obj.ID;
        }
        public void InsertFromGradation(HumanAuditGradationCriteriaItem objNew, int userId, List<HumanAuditGraditionCriteriaPointItem> lstPoint)
        {
            var dbo = criteriaDA.Repository;
            HumanAuditCriteria obj = new HumanAuditCriteria();
            if (dbo.HumanAuditCriterias.FirstOrDefault(t => t.Name.ToUpper() == objNew.Name.ToUpper() && t.HumanAuditCriteriaCategoryID == dbo.HumanAuditCriteriaCategories.Where(x=>x.Name.Trim().ToUpper().Equals(objNew.HumanAuditCriteriaCategoryName.Trim().ToUpper())).FirstOrDefault().ID) == null)
            {
                obj.CreateAt = DateTime.Now;
                obj.CreateBy = userId;
                obj.Factory = objNew.Factory;
                obj.Note = objNew.Note;
                obj.Name = objNew.Name;
                obj.HumanAuditCriteriaCategoryID = dbo.HumanAuditCriteriaCategories.Where(x => x.Name.Trim().ToUpper().Equals(objNew.HumanAuditCriteriaCategoryName.Trim().ToUpper())).FirstOrDefault().ID;
                dbo.HumanAuditCriterias.Add(obj);
                dbo.SaveChanges();
                foreach (var item in lstPoint)
                {
                    HumanAuditCriteriaPoint objPoint = new HumanAuditCriteriaPoint();
                    objPoint.CreateAt = DateTime.Now;
                    objPoint.CreateBy = userId;
                    objPoint.Name = item.Name;
                    objPoint.Point = item.Point;
                    objPoint.Note = item.Note;
                    objPoint.HumanAuditCriteriaID = obj.ID;
                    dbo.HumanAuditCriteriaPoints.Add(objPoint);
                    dbo.SaveChanges();
                }
            }
           
        }
        public void Delete(string stringId)
        {
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            foreach (var item in ids)
            {
                criteriaDA.Delete(item);
            }
            criteriaDA.Save();
        }
        public void Delete(int id)
        {
            var dbo = criteriaDA.Repository;
            var s = dbo.HumanAuditCriteriaPoints.Where(t => t.HumanAuditCriteriaID == id).ToList();
            dbo.HumanAuditCriteriaPoints.RemoveRange(s);
            dbo.SaveChanges();
            criteriaDA.Delete(id);
            criteriaDA.Save();
        }
        //public bool CheckExits(int id, List<HumanAuditGradationCriteriaPointItem> lstPoint)
        //{
        //    var dbo = criteriaDA.Repository;
        //    var points = dbo.HumanAuditGraditionCriteriaPoints.Where(t=>t.HumanAuditGradationCriteriaID==id).Where(t=>t.Name.Trim().ToUpper())
        //}

        public bool Update(HumanAuditCriteriaItem objEdit, int userId, List<HumanAuditGradationCriteriaPointItem> lstPoint)
        {
            var dbo = criteriaDA.Repository;
            HumanAuditCriteria obj = dbo.HumanAuditCriterias.FirstOrDefault(i => i.ID == objEdit.ID);
            if (dbo.HumanAuditCriterias.FirstOrDefault(t => t.Name.ToUpper() == objEdit.Name.ToUpper()) != null
                 && (obj.Name.ToUpper() != objEdit.Name.ToUpper()))
            {
                return false;
            }
            obj.Name = objEdit.Name;
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            obj.Factory = objEdit.Factory.Value;
            obj.Note = objEdit.Note;
            dbo.SaveChanges();
            var arrIdPoint = lstPoint.Select(t => t.ID).ToList();
            var s = dbo.HumanAuditCriteriaPoints.Where(t => t.HumanAuditCriteriaID == objEdit.ID && !arrIdPoint.Contains(t.ID)).ToList();
            dbo.HumanAuditCriteriaPoints.RemoveRange(s);
            dbo.SaveChanges();
            foreach (var item in lstPoint)
            {
                if (item.ID == 0)
                {
                    HumanAuditCriteriaPoint objPoint = new HumanAuditCriteriaPoint();
                    objPoint.CreateAt = DateTime.Now;
                    objPoint.CreateBy = userId;
                    objPoint.Name = item.Name;
                    objPoint.Point = item.Point;
                    objPoint.Note = item.Note;
                    objPoint.HumanAuditCriteriaID = objEdit.ID;
                    dbo.HumanAuditCriteriaPoints.Add(objPoint);
                    dbo.SaveChanges();
                }
                else
                {
                    var objEditPoint = dbo.HumanAuditCriteriaPoints.Where(t => t.ID == item.ID).FirstOrDefault();
                    objEditPoint.UpdateAt = DateTime.Now;
                    objEditPoint.UpdateBy = userId;
                    objEditPoint.Name = item.Name;
                    objEditPoint.Point = item.Point;
                    objEditPoint.Note = item.Note;
                    objEditPoint.HumanAuditCriteriaID = objEdit.ID;
                    dbo.SaveChanges();
                }
            }
            return true;
        }
        public void CoppyData(int id, int cateId, int userId)
        {
            var dbo = criteriaDA.Repository;
            var data = dbo.HumanAuditCriterias.Where(t => t.HumanAuditCriteriaCategoryID == cateId).ToList();
            if (data.Count > 0)
            {
                foreach (var item in data)
                {
                    if (dbo.HumanAuditCriterias.Where(t => t.Name.Trim().ToUpper() == item.Name.Trim().ToUpper() && t.HumanAuditCriteriaCategoryID == id).FirstOrDefault() == null)
                    {
                        HumanAuditCriteria obj = new HumanAuditCriteria();
                        obj.Name = item.Name;
                        obj.Note = item.Note;
                        obj.HumanAuditCriteriaCategoryID = id;
                        obj.Factory = item.Factory;
                        obj.CreateAt = DateTime.Now;
                        obj.CreateBy = userId;
                        dbo.HumanAuditCriterias.Add(obj);
                        dbo.SaveChanges();
                        var points = dbo.HumanAuditCriteriaPoints.Where(t => t.HumanAuditCriteriaID == item.ID).ToList();
                        if (points.Count > 0)
                        {
                            foreach (var point in points)
                            {
                                HumanAuditCriteriaPoint objPoint = new HumanAuditCriteriaPoint();
                                objPoint.HumanAuditCriteriaID = obj.ID;
                                objPoint.Name = point.Name;
                                objPoint.Note = point.Note;
                                objPoint.Point = point.Point;
                                objPoint.CreateAt = DateTime.Now;
                                objPoint.CreateBy = userId;
                                dbo.HumanAuditCriteriaPoints.Add(objPoint);
                                dbo.SaveChanges();
                            }
                        }
                    }
                }
            }
        }
    }
}
