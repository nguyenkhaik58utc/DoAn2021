using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ext.Net;
using Ext.Net.MVC;
using iDAS.Core;
using iDAS.Base;
using iDAS.DA;
using iDAS.Items;

namespace iDAS.Services
{
    public static class LinqHelper
    {
        /// <summary>
        /// 24/03/2015  Thanhnv: LeftJoin
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TInner"></typeparam>
        /// <typeparam name="Tkey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <param name="inner"></param>
        /// <param name="pk"></param>
        /// <param name="fk"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> LeftJoin<TSource, TInner, Tkey, TResult>
            (this IEnumerable<TSource> source,
                  IEnumerable<TInner> inner,
                  Func<TSource, Tkey> pk,
                  Func<TInner, Tkey> fk,
                  Func<TSource, TInner, TResult> result)
        {
            IEnumerable<TResult> _result = Enumerable.Empty<TResult>();
            _result = from s in source
                      join i in inner
                      on pk(s) equals fk(i) into joinData
                      from left in joinData.DefaultIfEmpty()
                      select result(s, left);
            return _result;
        }
    }
    public class AuditResultSV
    {
        private AuditResultDA resultDA = new AuditResultDA();
        private iDAS.Services.QualityNCSV nCService = new iDAS.Services.QualityNCSV();
        private QualityCriteriaCategorySV criteriaCategorySV = new QualityCriteriaCategorySV();
        private QualityCriteriaDA criteriaDA = new QualityCriteriaDA();
        public QualityCriteriaCategoryItem GetCateByID(int id)
        {
           return criteriaCategorySV.GetById(id);
        }
        public AuditResultItem GetByAuditIDAndCriterialID(int auditId, int criteriaId)
        {
            var dbo = resultDA.Repository;
            AuditResultItem obj = new AuditResultItem();
            obj.QualityCriteriaID = criteriaId;
            List<int> lstQualityNCID = dbo.AuditResults.Where(t => t.QualityCriteriaID == criteriaId && t.QualityNCID.HasValue).Select(t => t.QualityNCID.Value).ToList();
            var source = dbo.QualityCAPARequirements
                 .Where(i => lstQualityNCID.Contains(i.QualityNCID) && !i.IsComplete)
                .Select(item => new QualityCAPARequireItem()
                {
                    ID = item.ID,
                    Name = item.Name
                })
                .ToList();
            var result = resultDA.GetQuery(t => t.AuditID == auditId && t.QualityCriteriaID == criteriaId).FirstOrDefault();
            if (result != null)
            {
                obj.Point = result.Point;
                obj.AuditAt = result.AuditAt;
                obj.IsPass = result.IsPass;
                obj.AuditNote = result.AuditNote;
                obj.ResultPass = result.AuditAt.HasValue && result.IsPass ? true : false;
                obj.ResultNotPass = result.AuditAt.HasValue && !result.IsPass ? true : false;
                obj.IsObs = result.QualityNC != null ? result.QualityNC.IsObs : false;
                obj.IsMaximum = result.QualityNC != null ? result.QualityNC.IsMaximum : false;
                obj.IsMedium = result.QualityNC != null ? result.QualityNC.IsMedium : false;
            }
            obj.IsHasCAPARequirement = source.Count > 0 ? true : false;
            obj.QualityCAPARequirementID = source.Count > 0 ? source.FirstOrDefault().ID : 0;
            return obj;
        }

        private IEnumerable<QualityCriteriaCategory> getChilds(IEnumerable<QualityCriteriaCategory> e, int id)
        {
            var listGet = e.Where(a => a.ParentID == id);
            var listFirst = e.Where(a => a.ID == id);
            listFirst.Concat(listGet);
            return listFirst.Concat(listGet.SelectMany(a => getChilds(e, a.ID)));
        }
        public List<AuditResultItem> GetByAuditID(int page, int pageSize, out int totalCount, int cateId, int auditId)
        {
            var dbo = resultDA.Repository;
            var result = getChilds(dbo.QualityCriteriaCategories, cateId).SelectMany(i => i.QualityCriterias.Where(c => !c.IsDelete && c.IsActive))
                            .LeftJoin(dbo.AuditResults.Where(i => i.AuditID == auditId), cr => cr.ID, ac => ac.QualityCriteriaID,
                                (item, ac) => new AuditResultItem()
                                {
                                    ID = ac == null ? 0 : ac.ID,
                                    QualityCriteriaID = item.ID,
                                    Name = item.Name,
                                    MinPoint = item.MinPoint,
                                    MaxPoint = item.MaxPoint,
                                    Factory = item.Factory,
                                    Point = ac == null ? 0 : ac.Point,
                                    AuditAt = ac == null ? null : ac.AuditAt,
                                    IsPass = ac == null ? false : ac.IsPass,
                                    AuditNote = ac == null ? null : ac.AuditNote,
                                    ResultPass = ac == null ? false : (ac.AuditAt.HasValue && ac.IsPass ? true : false),
                                    ResultNotPass = ac == null ? false : (ac.AuditAt.HasValue && !ac.IsPass ? true : false),

                                    IsObs = ac == null ? false : (ac.AuditAt.HasValue && ac.IsPass ? true : false) ? false : ac == null ? false : (ac.QualityNC != null ? ac.QualityNC.IsObs : false),
                                    IsMaximum = ac == null ? false : (ac.AuditAt.HasValue && ac.IsPass ? true : false) ? false : ac == null ? false : (ac.QualityNC != null ? ac.QualityNC.IsMaximum : false),
                                    IsMedium = ac == null ? false : (ac.AuditAt.HasValue && ac.IsPass ? true : false) ? false : ac == null ? false : (ac.QualityNC != null ? ac.QualityNC.IsMedium : false),

                                })
                                .OrderBy(m => m.Name)
                                .Page(page, pageSize, out totalCount)
                                .ToList();
            if (result.Count() > 0)
            {
                result.First().TotalPoint = getChilds(dbo.QualityCriteriaCategories, cateId).SelectMany(i => i.QualityCriterias.Where(c => !c.IsDelete && c.IsActive))
                        .LeftJoin(dbo.AuditResults.Where(i => i.AuditID == auditId),
                                        cr => cr.ID,
                                        ac => ac.QualityCriteriaID,
                                        (cr, ac) => new PointItem() { Point = ac == null ? 0 : ac.Point }).Sum(i => i.Point);
            }
            return result;


        }
        public bool SetEditResult(int criteriaId)
        {
            var rs = resultDA.GetQuery(t => t.QualityCriteriaID == criteriaId).OrderByDescending(t => t.CreateAt).FirstOrDefault();
            if (rs != null)
            {
                if (rs.QualityNC == null || rs.QualityNC.IsCAPA)
                    return true;
                return false;
            }
            else
                return true;

        }
        public void InsertByCriteriaID(int criteriaID, int NCID, int createby, int auditby)
        {
            AuditResult objI = new AuditResult();
            objI.AuditNote = string.Empty;
            objI.QualityCriteriaID = criteriaID;
            objI.QualityNCID = NCID;
            objI.IsPass = false;
            objI.Point = 0;
            objI.CreateAt = DateTime.Now;
            objI.CreateBy = createby;
            objI.AuditAt = DateTime.Now;
            objI.AuditBy = auditby;
            resultDA.Insert(objI);
            resultDA.Save();
        }

        public void Delete(int auditId, int criteriaId)
        {
            var result = resultDA.GetQuery(t => t.AuditID == auditId && t.QualityCriteriaID == criteriaId).FirstOrDefault();
            if (result != null)
                resultDA.Delete(result);
            resultDA.Save();

        }
        public int GetMin(int criteriaId)
        {
            var dbo = resultDA.Repository;
            return dbo.QualityCriterias.Where(t => t.ID == criteriaId).FirstOrDefault().MinPoint;

        }
        public int GetMax(int criteriaId)
        {
            var dbo = resultDA.Repository;
            return dbo.QualityCriterias.Where(t => t.ID == criteriaId).FirstOrDefault().MaxPoint;
        }
        public void InsertOrUpdate(AuditResultItem obj, int userId, int employeeId)
        {
            var dbo = resultDA.Repository;
            var result = resultDA.GetQuery(t => t.AuditID == obj.AuditID && t.QualityCriteriaID == obj.QualityCriteriaID).FirstOrDefault();
            if (result != null)
            {
                result.AuditNote = obj.AuditNote;
                result.QualityCriteriaID = obj.QualityCriteriaID;
                if (result.AuditID != 0 || result.AuditID != null)
                {
                    result.AuditID = obj.AuditID;

                }
                else
                {
                    result.AuditID = obj.AuditID;

                }
                result.IsPass = (bool)obj.IsPass;
                result.Point = (int)obj.Point;
                result.UpdateAt = DateTime.Now;
                result.UpdateBy = userId;
                result.AuditAt = DateTime.Now;
                result.AuditBy = employeeId;
                resultDA.Update(result);
                if (obj.IsCorrertive)
                {
                    var capa = dbo.QualityCAPARequirements.Where(t => t.ID == obj.QualityCAPARequirementID).FirstOrDefault();
                    capa.IsAuditBack = true;
                    capa.AuditBackPass = obj.IsPass;
                    capa.AuditBackTime = DateTime.Now;
                    capa.AuditBackNote = obj.AuditNote;
                    capa.AuditBackBy = userId;
                }
            }
            else
            {
                AuditResult objI = new AuditResult();
                objI.AuditNote = obj.AuditNote;
                objI.QualityCriteriaID = obj.QualityCriteriaID;
                if (obj.AuditID == 0 || obj.AuditID == null)
                {
                    objI.AuditID = obj.AuditID;

                }
                else
                {
                    objI.AuditID = obj.AuditID;

                }
                objI.IsPass = (bool)obj.IsPass;
                objI.Point = (int)obj.Point;
                objI.UpdateAt = DateTime.Now;
                objI.UpdateBy = userId;
                objI.AuditAt = DateTime.Now;
                objI.AuditBy = employeeId;
                resultDA.Insert(objI);
                if (obj.IsCorrertive)
                {
                    var capa = dbo.QualityCAPARequirements.Where(t => t.ID == obj.QualityCAPARequirementID).FirstOrDefault();
                    capa.IsAuditBack = true;
                    capa.AuditBackPass = obj.IsPass;
                    capa.AuditBackTime = DateTime.Now;
                    capa.AuditBackNote = obj.AuditNote;
                    capa.AuditBackBy = userId;
                }
            }
            resultDA.Save();
        }
    }
}
