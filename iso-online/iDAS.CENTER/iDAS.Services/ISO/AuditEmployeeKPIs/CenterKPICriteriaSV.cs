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
    public class CenterKPICriteriaSV
    {
        private CenterKPICriteriaDA criteriaDA = new CenterKPICriteriaDA();
        public List<CenterKPICriteriaItem> GetByCateId(ModelFilter filter, int cateId)
        {
            var dbo = criteriaDA.Repository;
            var criterias = dbo.CenterKPICriterias.Where(t => t.CenterKPICriteriaCategoryID == cateId)
                          .Select(item => new CenterKPICriteriaItem()
                          {
                              ID = item.ID,
                              CenterKPICriteriaCategoryID = item.CenterKPICriteriaCategoryID,
                              CategoryName = item.CenterKPICriteriaCategory.Name,
                              Factory = item.Factory,
                              Name = item.Name,
                              CreateAt = item.CreateAt
                          })
                          .Filter(filter)
                          .ToList();
            return criterias;
        }
        public List<CenterKPICriteriaItem> GetByListCateId(ModelFilter filter, string lstCateId)
        {
            List<int> lstCateID = new List<int>();
            foreach (string s in lstCateId.Split(','))
            {
                int result = 0;
                if (int.TryParse(s, out result))
                    lstCateID.Add(result);
            }
            var dbo = criteriaDA.Repository;
            var criterias = dbo.CenterKPICriterias.Where(t => lstCateID.Contains(t.CenterKPICriteriaCategoryID))
                          .Select(item => new CenterKPICriteriaItem()
                          {
                              ID = item.ID,
                              CenterKPICriteriaCategoryID = item.CenterKPICriteriaCategoryID,
                              CategoryName = item.CenterKPICriteriaCategory.Name,
                              Name = item.Name,
                              CreateAt = item.CreateAt
                          })
                          .Filter(filter)
                          .ToList();
            return criterias;
        }
        public CenterKPICriteriaItem GetById(int id)
        {
            var result = criteriaDA.GetQuery(t => t.ID == id)
                .Select(i => new CenterKPICriteriaItem()
                {
                    ID = i.ID,
                    Name = i.Name,
                    CreateAt = i.CreateAt,
                    Note = i.Note,
                    Factory = i.Factory,
                    CenterKPICriteriaCategoryID = i.CenterKPICriteriaCategory.ID,
                    CategoryName = i.CenterKPICriteriaCategory.Name,
                    CreateBy = i.CreateBy,
                    UpdateBy = i.UpdateBy,
                    UpdateAt = i.UpdateAt,
                }
                ).FirstOrDefault();
            return result;
        }
        public int Insert(CenterKPICriteriaItem objNew, int userId, List<CenterKPICriteriaPointItem> lstPoint)
        {
            var dbo = criteriaDA.Repository;
            CenterKPICriteria obj = new CenterKPICriteria();
            if (dbo.CenterKPICriterias.FirstOrDefault(t => t.Name.ToUpper() == objNew.Name.ToUpper() && t.CenterKPICriteriaCategoryID == objNew.CenterKPICriteriaCategoryID) != null)
            {
                return 0;
            }
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            obj.Factory = objNew.Factory.Value;
            obj.Note = objNew.Note;
            obj.Name = objNew.Name;
            obj.CenterKPICriteriaCategoryID = objNew.CenterKPICriteriaCategoryID;
            dbo.CenterKPICriterias.Add(obj);
            dbo.SaveChanges();
            foreach (var item in lstPoint)
            {
                CenterKPICriteriaPoint objPoint = new CenterKPICriteriaPoint();
                objPoint.CreateAt = DateTime.Now;
                objPoint.CreateBy = userId;
                objPoint.Name = item.Name;
                objPoint.Point = item.Point;
                objPoint.Note = item.Note;
                objPoint.CenterKPICriteriaID = obj.ID;
                dbo.CenterKPICriteriaPoints.Add(objPoint);
                dbo.SaveChanges();
            }
            return obj.ID;
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
            var s = dbo.CenterKPICriteriaPoints.Where(t => t.CenterKPICriteriaID == id).ToList();
            dbo.CenterKPICriteriaPoints.RemoveRange(s);
            dbo.SaveChanges();
            criteriaDA.Delete(id);
            criteriaDA.Save();
        }
        //public bool CheckExits(int id, List<CenterKPIGradationCriteriaPointItem> lstPoint)
        //{
        //    var dbo = criteriaDA.Repository;
        //    var points = dbo.CenterKPIGraditionCriteriaPoints.Where(t=>t.CenterKPIGradationCriteriaID==id).Where(t=>t.Name.Trim().ToUpper())
        //}

        public bool Update(CenterKPICriteriaItem objEdit, int userId, List<CenterKPICriteriaPointItem> lstPoint)
        {
            var dbo = criteriaDA.Repository;
            CenterKPICriteria obj = dbo.CenterKPICriterias.FirstOrDefault(i => i.ID == objEdit.ID);
            if (dbo.CenterKPICriterias.FirstOrDefault(t => t.Name.ToUpper() == objEdit.Name.ToUpper()) != null
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
            var s = dbo.CenterKPICriteriaPoints.Where(t => t.CenterKPICriteriaID == objEdit.ID && !arrIdPoint.Contains(t.ID)).ToList();
            dbo.CenterKPICriteriaPoints.RemoveRange(s);
            dbo.SaveChanges();
            foreach (var item in lstPoint)
            {
                if (item.ID == 0)
                {
                    CenterKPICriteriaPoint objPoint = new CenterKPICriteriaPoint();
                    objPoint.CreateAt = DateTime.Now;
                    objPoint.CreateBy = userId;
                    objPoint.Name = item.Name;
                    objPoint.Point = item.Point;
                    objPoint.Note = item.Note;
                    objPoint.CenterKPICriteriaID = objEdit.ID;
                    dbo.CenterKPICriteriaPoints.Add(objPoint);
                    dbo.SaveChanges();
                }
                else
                {
                    var objEditPoint = dbo.CenterKPICriteriaPoints.Where(t => t.ID == item.ID).FirstOrDefault();
                    objEditPoint.UpdateAt = DateTime.Now;
                    objEditPoint.UpdateBy = userId;
                    objEditPoint.Name = item.Name;
                    objEditPoint.Point = item.Point;
                    objEditPoint.Note = item.Note;
                    objEditPoint.CenterKPICriteriaID = objEdit.ID;
                    dbo.SaveChanges();
                }
            }
            return true;
        }
        //public void CoppyData(int id, int cateId, int userId)
        //{
        //    var dbo = criteriaDA.Repository;
        //    var data = dbo.CenterKPICriterias.Where(t => t.CenterKPICriteriaCategoryID == cateId).ToList();
        //    if (data.Count > 0)
        //    {
        //        foreach (var item in data)
        //        {
        //            if (dbo.CenterKPICriterias.Where(t => t.Name.Trim().ToUpper() == item.Name.Trim().ToUpper() && t.CenterKPICriteriaCategoryID == id).FirstOrDefault() == null)
        //            {
        //                CenterKPICriteria obj = new CenterKPICriteria();
        //                obj.Name = item.Name;
        //                obj.Note = item.Note;
        //                obj.CenterKPICriteriaCategoryID = id;
        //                obj.Factory = item.Factory;
        //                obj.CreateAt = DateTime.Now;
        //                obj.CreateBy = userId;
        //                dbo.CenterKPICriterias.Add(obj);
        //                dbo.SaveChanges();
        //                var points = dbo.CenterKPICriteriaPoints.Where(t => t.CenterKPICriteriaID == item.ID).ToList();
        //                if (points.Count > 0)
        //                {
        //                    foreach (var point in points)
        //                    {
        //                        CenterKPICriteriaPoint objPoint = new CenterKPICriteriaPoint();
        //                        objPoint.CenterKPICriteriaID = obj.ID;
        //                        objPoint.Name = point.Name;
        //                        objPoint.Note = point.Note;
        //                        objPoint.Point = point.Point;
        //                        objPoint.CreateAt = DateTime.Now;
        //                        objPoint.CreateBy = userId;
        //                        dbo.CenterKPICriteriaPoints.Add(objPoint);
        //                        dbo.SaveChanges();
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
