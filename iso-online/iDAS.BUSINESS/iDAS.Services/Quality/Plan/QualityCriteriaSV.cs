using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDAS.Core;
using iDAS.Base;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public class QualityCriteriaSV
    {
        private QualityCriteriaDA criteriaDA = new QualityCriteriaDA();
        public List<QualityCriteriaItem> GetByCateId( ModelFilter filter, int cateId)
        {
            var dbo = criteriaDA.Repository;
            var criterias = dbo.QualityCriterias.Where(t => t.QualityCriteriaCategoryID == cateId)
                          .Select(item => new QualityCriteriaItem()
                          {
                              ID = item.ID,
                              //CategoryID = item.CriteriaCategoryID,
                              //CategoryName = item.CriteriaCategoryID.Name,
                              Name = item.Name,
                              Note = item.Note,
                              Factory = item.Factory,
                              MaxPoint = item.MaxPoint,
                              MinPoint = item.MinPoint,
                              IsUse = item.IsActive,
                              IsDelete = item.IsDelete,
                              CreateAt = item.CreateAt
                          })
                          .Filter(filter)
                          .ToList();
            return criterias;
        }
        //private IEnumerable<CriteriaCategory> getChilds(IEnumerable<MnCriteriaCategory> e, int id)
        //{
        //    var list1 = e.Where(a => a.ParentID == id);
        //    var list2 = e.Where(a => a.ID == id);
        //    list2.Concat(list1);
        //    return list2.Concat(list1.SelectMany(a => getChilds(e, a.ID)));
        //}
        //private IEnumerable<MnCriteriaCategory> getParents(IEnumerable<MnCriteriaCategory> e, int id)
        //{
        //    var listParentNext = e.Where(a => a.ID == id);
        //    var result = listParentNext.Concat(listParentNext.SelectMany(a => getParents(e, (int)a.ParentID)));
        //    return result;
        //}
        //public List<CriteriaItem> GetByCateIds(int page, int pageSize, out int total, int cateId)
        //{
        //    var dbo = criteriaDA.Repository;
        //    var ids = getParents(dbo.MnCriteriaCategory, cateId).Select(i => i.ID).ToList();
        //   // var ids = getChilds(dbo.MnCriteriaCategory, cateId).Select(i => i.ID).ToList();
        //    var criterias = dbo.MnCriteria.Where(t => ids.Contains(t.CategoryID) && t.IsDelete == false)
        //                  .Select(item => new CriteriaItem()
        //                  {
        //                      ID = item.ID,
        //                      CategoryID = item.CategoryID,
        //                      CategoryName = item.MnCriteriaCategory.Name,
        //                      Name = item.Name,
        //                      Factory = item.Factory,
        //                      MaxPoint = item.MaxPoint,
        //                      MinPoint = item.MinPoint,
        //                      IsUse = item.IsUse,
        //                      IsDelete = item.IsDelete,
        //                      CreateAt = item.CreateAt
        //                  })
        //                  .OrderByDescending(item => item.CreateAt)
        //                  .Page(page, pageSize, out total)
        //                  .ToList();
        //    return criterias;
        //}
        public List<QualityCriteriaItem> GetCriteriaUsedByCateIds(int page, int pageSize, out int total, string strCateIds)
        {
            var ids = strCateIds.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            var dbo = criteriaDA.Repository;
            var criterias = dbo.QualityCriterias.Where(t =>
                t.QualityCriteriaCategoryID.HasValue && ids.Contains(t.QualityCriteriaCategoryID.Value)
                && t.IsActive == true && t.IsDelete == false)
                          .Select(item => new QualityCriteriaItem()
                          {
                              ID = item.ID,
                              //CategoryID = item.CategoryID,
                              CategoryName = item.QualityCriteriaCategory.Name,
                              Name = item.Name,
                              Note = item.Note,
                              Factory = item.Factory,
                              MaxPoint = item.MaxPoint,
                              MinPoint = item.MinPoint,
                              //IsUse = item.IsUse,
                              IsDelete = item.IsDelete,
                              CreateAt = item.CreateAt
                          })
                          .OrderByDescending(item => item.CreateAt)
                          .Page(page, pageSize, out total)
                          .ToList();
            return criterias;
        }
        /// <summary>
        /// VinhDQ
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <param name="strCateIds"></param>
        /// <returns></returns>
        public List<QualityCriteriaItem> GetCriteriaUsedByCateId(int page, int pageSize, out int total, string strCateId)
        {
            int cateID = int.Parse(strCateId);
            var dbo = criteriaDA.Repository;
            var criterias = dbo.QualityCriterias.Where(t =>
                t.QualityCriteriaCategoryID == cateID
                && 
                t.IsActive == true && t.IsDelete == false)
                          .Select(item => new QualityCriteriaItem()
                          {
                              ID = item.ID,
                              //CategoryID = item.CategoryID,
                              CategoryName = item.QualityCriteriaCategory.Name,
                              Name = item.Name,
                              Note = item.Note,
                              Factory = item.Factory,
                              MaxPoint = item.MaxPoint,
                              MinPoint = item.MinPoint,
                              //IsUse = item.IsUse,
                              IsDelete = item.IsDelete,
                              CreateAt = item.CreateAt
                          })
                          .OrderByDescending(item => item.CreateAt)
                          .Page(page, pageSize, out total)
                          .ToList();
            return criterias;
        }
        public QualityCriteriaItem GetById(int id)
        {
            var result = criteriaDA.GetQuery(t => t.ID == id)
                .Select(i => new QualityCriteriaItem()
                                {
                                    ID = i.ID,
                                    Name = i.Name,
                                    Note = i.Note,
                                    IsUse = i.IsActive,
                                    IsDelete = i.IsDelete,
                                    CreateAt = i.CreateAt,
                                    MinPoint = i.MinPoint,
                                    MaxPoint = i.MaxPoint,
                                    Factory = i.Factory,
                                    CategoryID = i.QualityCriteriaCategory.ID,
                                    CategoryName = i.QualityCriteriaCategory.Name,
                                    CreateBy = i.CreateBy,
                                    UpdateBy = i.UpdateBy,
                                    UpdateAt = i.UpdateAt,
                                }
                ).FirstOrDefault();
            return result;
        }
        public int Insert(QualityCriteriaItem objNew, int userId)
        {
            var dbo = criteriaDA.Repository;
            QualityCriteria obj = new QualityCriteria();
            if (dbo.QualityCriterias.FirstOrDefault(t => t.Name.ToUpper() == objNew.Name.ToUpper()) != null)
            {
                return 0;
            }
            obj.CreateAt = DateTime.Now;
            obj.CreateBy = userId;
            obj.Name = objNew.Name;
            obj.Note = objNew.Note;
            obj.IsActive = objNew.IsUse;
            obj.IsDelete = objNew.IsDelete;
            obj.MinPoint = objNew.MinPoint;
            obj.MaxPoint = objNew.MaxPoint;
            obj.Factory = objNew.Factory;
            obj.QualityCriteriaCategoryID = objNew.CategoryID;
            dbo.QualityCriterias.Add(obj);
            dbo.SaveChanges();
            return obj.ID;
        }
        public void Delete(string stringId)
        {
            var ids = stringId.Split(',').Select(n => (object)int.Parse(n)).ToList();
            foreach (var item in ids)
            {
                criteriaDA.GetById(item).IsDelete = true;

            }
            criteriaDA.Save();
        }
        public void Delete(int id)
        {
            criteriaDA.Delete(id);
            criteriaDA.Save();
        }
        public bool Update(QualityCriteriaItem objEdit, int userId)
        {
            var dbo = criteriaDA.Repository;
            QualityCriteria obj = dbo.QualityCriterias.FirstOrDefault(i => i.ID == objEdit.ID);
            if (dbo.QualityCriterias.FirstOrDefault(t => t.Name.ToUpper() == objEdit.Name.ToUpper()) != null
                 && (obj.Name.ToUpper() != objEdit.Name.ToUpper()))
            {
                return false;
            }
            obj.Name = objEdit.Name;
            obj.Note = objEdit.Note;
            obj.MinPoint = objEdit.MinPoint;
            obj.MaxPoint = objEdit.MaxPoint;
            obj.Factory = objEdit.Factory;
            obj.IsActive = objEdit.IsUse;
            obj.IsDelete = objEdit.IsDelete;
            obj.UpdateAt = DateTime.Now;
            obj.UpdateBy = userId;
            dbo.SaveChanges();
            return true;
        }
    }
}
